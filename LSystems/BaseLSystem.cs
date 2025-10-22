using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS332_Lab5.LSistems
{
    /// <summary>
    /// Базовая L-система.
    /// Умеет:
    /// - Чтение описания L-систем из текстовых файлов
    /// - Поддержка разветвлений (скобки в правилах)
    /// - Добавление случайности в построение
    /// </summary>
    public class BaseLSystem
    {
        private Random random = new Random();

        // Основные параметры системы
        public string Axiom { get; private set; } = "F";
        public float Angle { get; private set; } = 90f;
        public float InitialDirection { get; private set; } = 0f;
        public int Iterations { get; set; } = 3;
        public float StepLength { get; set; } = 10f;
        public float Randomness { get; set; } = 0f; // 0-1, степень случайности
        public float Scale { get; set; } = 1.0f; // Масштаб
        public float ScaleFactor { get; set; } = 0.8f; // Коэффициент масштабирования для каждой итерации

        // Правила замены
        private Dictionary<char, string> rules = new Dictionary<char, string>();

        // Стек для ветвлений
        private Stack<(PointF position, float angle, float stepLength)> stateStack = new Stack<(PointF, float, float)>();

        // Результат построения
        public List<LineSegment> Segments { get; private set; } = new List<LineSegment>();

        // Границы рисунка для автоматического масштабирования
        public RectangleF Bounds { get; private set; }

        public class LineSegment
        {
            public PointF Start { get; set; }
            public PointF End { get; set; }
            public Color Color { get; set; } = Color.Black;
            public float Width { get; set; } = 1f;
        }

        // Загрузка системы из файла
        public bool LoadFromFile(string filename)
        {
            try
            {
                rules.Clear();
                var lines = File.ReadAllLines(filename).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                if (lines.Length < 1)
                    return false;

                // Первая строка: аксиома, угол, начальное направление
                var firstLine = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (firstLine.Length >= 1) Axiom = firstLine[0];
                if (firstLine.Length >= 2) Angle = float.Parse(firstLine[1]);
                if (firstLine.Length >= 3) InitialDirection = float.Parse(firstLine[2]);

                // Остальные строки: правила
                for (int i = 1; i < lines.Length; i++)
                {
                    var ruleParts = lines[i].Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (ruleParts.Length == 2)
                    {
                        char predecessor = ruleParts[0][0];
                        rules[predecessor] = ruleParts[1];
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла: {ex.Message}");
                return false;
            }
        }

        // Генерация строки после N итераций
        public string GenerateString(int iterations)
        {
            string current = Axiom;

            for (int i = 0; i < iterations; i++)
            {
                string next = "";
                foreach (char c in current)
                {
                    if (rules.ContainsKey(c))
                    {
                        // Добавляем случайность в замену
                        if (Randomness > 0 && random.NextDouble() < Randomness)
                        {
                            next += c; // Иногда оставляем оригинальный символ
                        }
                        else
                        {
                            next += rules[c];
                        }
                    }
                    else
                    {
                        next += c;
                    }
                }
                current = next;
            }

            return current;
        }

        // Визуализация L-системы
        public void Render(Graphics graphics, PointF startPosition, int iterations)
        {
            Segments.Clear();
            stateStack.Clear();

            string lstring = GenerateString(iterations);
            PointF currentPosition = startPosition;
            float currentAngle = InitialDirection;
            float currentStepLength = StepLength * Scale;

            // Автоматическое масштабирование длины шага в зависимости от итераций
            if (ScaleFactor != 1.0f)
            {
                currentStepLength *= (float)Math.Pow(ScaleFactor, iterations);
            }

            foreach (char c in lstring)
            {
                switch (c)
                {
                    case 'F': // Движение вперед с рисованием
                    case 'G': // Движение вперед с рисованием (альтернатива)
                        PointF newPosition = CalculateNewPosition(currentPosition, currentAngle, currentStepLength);
                        Segments.Add(new LineSegment
                        {
                            Start = currentPosition,
                            End = newPosition,
                            Width = Math.Max(0.5f, currentStepLength / 10f) // Толщина линии зависит от масштаба
                        });
                        currentPosition = newPosition;
                        break;

                    case 'f': // Движение вперед без рисования
                    case 'g': // Движение вперед без рисования (альтернатива)
                        currentPosition = CalculateNewPosition(currentPosition, currentAngle, currentStepLength);
                        break;

                    case '+': // Поворот налево
                        currentAngle += GetAngleWithRandomness();
                        break;

                    case '-': // Поворот направо
                        currentAngle -= GetAngleWithRandomness();
                        break;

                    case '[': // Сохраняем состояние (начало ветвления)
                        stateStack.Push((currentPosition, currentAngle, currentStepLength));
                        // Уменьшаем длину шага для ветвей (опционально)
                        currentStepLength *= ScaleFactor;
                        break;

                    case ']': // Восстанавливаем состояние (конец ветвления)
                        if (stateStack.Count > 0)
                        {
                            var state = stateStack.Pop();
                            currentPosition = state.position;
                            currentAngle = state.angle;
                            currentStepLength = state.stepLength;
                        }
                        break;

                        // Игнорируем другие символы
                }
            }

            // Вычисляем границы рисунка
            CalculateBounds();

            // Отрисовка всех сегментов
            foreach (var segment in Segments)
            {
                using (Pen pen = new Pen(segment.Color, segment.Width))
                {
                    graphics.DrawLine(pen, segment.Start, segment.End);
                }
            }
        }

        // Визуализация с автоматическим масштабированием под размер контрола
        public void RenderAutoScale(Graphics graphics, RectangleF bounds)
        {
            if (Segments.Count == 0) return;

            // Вычисляем масштаб для fitting'а
            float scaleX = bounds.Width / Bounds.Width;
            float scaleY = bounds.Height / Bounds.Height;
            float autoScale = Math.Min(scaleX, scaleY) * 0.9f; // 90% чтобы были отступы

            PointF center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
            PointF offset = new PointF(
                center.X - (Bounds.X + Bounds.Width / 2) * autoScale,
                center.Y - (Bounds.Y + Bounds.Height / 2) * autoScale
            );

            // Отрисовка с масштабированием
            foreach (var segment in Segments)
            {
                PointF start = new PointF(
                    segment.Start.X * autoScale + offset.X,
                    segment.Start.Y * autoScale + offset.Y
                );
                PointF end = new PointF(
                    segment.End.X * autoScale + offset.X,
                    segment.End.Y * autoScale + offset.Y
                );

                using (Pen pen = new Pen(segment.Color, Math.Max(0.5f, segment.Width * autoScale)))
                {
                    graphics.DrawLine(pen, start, end);
                }
            }
        }

        // Вычисление границ рисунка
        private void CalculateBounds()
        {
            if (Segments.Count == 0)
            {
                Bounds = RectangleF.Empty;
                return;
            }

            float minX = Segments[0].Start.X;
            float minY = Segments[0].Start.Y;
            float maxX = Segments[0].Start.X;
            float maxY = Segments[0].Start.Y;

            foreach (var segment in Segments)
            {
                minX = Math.Min(minX, Math.Min(segment.Start.X, segment.End.X));
                minY = Math.Min(minY, Math.Min(segment.Start.Y, segment.End.Y));
                maxX = Math.Max(maxX, Math.Max(segment.Start.X, segment.End.X));
                maxY = Math.Max(maxY, Math.Max(segment.Start.Y, segment.End.Y));
            }

            Bounds = new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        private PointF CalculateNewPosition(PointF position, float angle, float distance)
        {
            float angleRad = angle * (float)Math.PI / 180f;
            float dx = (float)Math.Cos(angleRad) * distance;
            float dy = (float)Math.Sin(angleRad) * distance;

            return new PointF(position.X + dx, position.Y + dy);
        }

        private float GetAngleWithRandomness()
        {
            if (Randomness == 0)
                return Angle;

            // Добавляем случайное отклонение к углу
            float randomFactor = (float)(random.NextDouble() * 2 - 1) * Randomness;
            return Angle * (1 + randomFactor * 0.5f); // ±50% отклонение
        }

        // Сохранение системы в файл
        public void SaveToFile(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine($"{Axiom} {Angle} {InitialDirection}");

                    foreach (var rule in rules)
                    {
                        writer.WriteLine($"{rule.Key} {rule.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения файла: {ex.Message}");
            }
        }

        // Добавление/изменение правила
        public void SetRule(char predecessor, string successor)
        {
            rules[predecessor] = successor;
        }

        // Удаление правила
        public void RemoveRule(char predecessor)
        {
            rules.Remove(predecessor);
        }

        // Получение всех правил
        public Dictionary<char, string> GetRules()
        {
            return new Dictionary<char, string>(rules);
        }

        // Методы для управления масштабированием
        public void ZoomIn()
        {
            Scale *= 1.2f;
        }

        public void ZoomOut()
        {
            Scale /= 1.2f;
        }

        public void ResetZoom()
        {
            Scale = 1.0f;
        }

        // Установка масштаба по ширине контрола
        public void FitToSize(float width, float height)
        {
            if (Bounds.IsEmpty) return;

            float scaleX = width / Bounds.Width;
            float scaleY = height / Bounds.Height;
            Scale = Math.Min(scaleX, scaleY) * 0.9f;
        }
    }
}
