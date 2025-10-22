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
    /// </summary>
    public class BaseLSystem
    {
        private string axiom;
        private double angle;
        private double initialDirection;
        private Dictionary<char, string> rules;
        private Random random;

        public double Scale { get; set; } = 1.0;
        public int Iterations { get; set; } = 4;
        public double StepLength { get; set; } = 10.0;
        public bool UseRandomness { get; set; } = false;
        public double RandomnessFactor { get; set; } = 0.1;

        public BaseLSystem()
        {
            rules = new Dictionary<char, string>();
            random = new Random();
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл конфигурации не найден: {filePath}");

            var lines = File.ReadAllLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();

            if (lines.Length < 1)
                throw new InvalidDataException("Файл конфигурации пуст");

            // Парсинг первой строки: <атом> <угол поворота> <начальное направление>
            var firstLineParts = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (firstLineParts.Length < 3)
                throw new InvalidDataException("Неверный формат первой строки конфигурационного файла");

            axiom = firstLineParts[0];
            angle = double.Parse(firstLineParts[1]);
            initialDirection = double.Parse(firstLineParts[2]);

            // Парсинг правил
            rules.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var ruleParts = lines[i].Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (ruleParts.Length == 2)
                {
                    char symbol = ruleParts[0].Trim()[0];
                    string replacement = ruleParts[1].Trim();
                    rules[symbol] = replacement;
                }
            }
        }

        public List<PointF> GeneratePoints(Size canvasSize)
        {
            string result = GenerateString(axiom, Iterations);
            return InterpretString(result, canvasSize);
        }

        private string GenerateString(string input, int iterations)
        {
            if (iterations == 0)
                return input;

            StringBuilder result = new StringBuilder();
            foreach (char c in input)
            {
                if (rules.ContainsKey(c))
                {
                    if (UseRandomness && random.NextDouble() < RandomnessFactor)
                    {
                        // Случайная замена на один из возможных вариантов
                        var possibleReplacements = GetPossibleReplacements(c);
                        if (possibleReplacements.Count > 0)
                        {
                            int index = random.Next(possibleReplacements.Count);
                            result.Append(possibleReplacements[index]);
                        }
                        else
                        {
                            result.Append(rules[c]);
                        }
                    }
                    else
                    {
                        result.Append(rules[c]);
                    }
                }
                else
                {
                    result.Append(c);
                }
            }

            return GenerateString(result.ToString(), iterations - 1);
        }

        private List<string> GetPossibleReplacements(char symbol)
        {
            return new List<string> { rules[symbol] };
        }

        private List<PointF> InterpretString(string lSystemString, Size canvasSize)
        {
            List<PointF> points = new List<PointF>();
            Stack<TurtleState> stateStack = new Stack<TurtleState>();

            TurtleState currentState = new TurtleState
            {
                Position = new PointF(canvasSize.Width / 2f, canvasSize.Height / 2f),
                Direction = initialDirection,
                Step = (float)(StepLength * Scale)
            };

            points.Add(currentState.Position);

            foreach (char c in lSystemString)
            {
                switch (c)
                {
                    case 'F':
                    case 'G':
                    case 'A':
                    case 'B':
                        // Движение вперед с рисованием
                        currentState.Position = CalculateNewPosition(currentState);
                        points.Add(currentState.Position);
                        break;

                    case '+':
                        // Поворот направо
                        currentState.Direction -= GetAdjustedAngle();
                        break;

                    case '-':
                        // Поворот налево
                        currentState.Direction += GetAdjustedAngle();
                        break;

                    case '[':
                        // Сохранение состояния (начало ветвления)
                        stateStack.Push(currentState.Clone());
                        break;

                    case ']':
                        // Восстановление состояния (конец ветвления)
                        if (stateStack.Count > 0)
                        {
                            currentState = stateStack.Pop();
                            points.Add(new PointF(float.NaN, float.NaN)); // Маркер разрыва
                            points.Add(currentState.Position);
                        }
                        break;

                }
            }

            return NormalizePoints(points, canvasSize);
        }

        private PointF CalculateNewPosition(TurtleState state)
        {
            double rad = state.Direction * Math.PI / 180.0;
            float newX = state.Position.X + (float)(state.Step * Math.Cos(rad));
            float newY = state.Position.Y + (float)(state.Step * Math.Sin(rad));
            return new PointF(newX, newY);
        }

        private double GetAdjustedAngle()
        {
            if (UseRandomness)
            {
                double variation = (random.NextDouble() - 0.5) * 2 * RandomnessFactor * angle;
                return angle + variation;
            }
            return angle;
        }

        private List<PointF> NormalizePoints(List<PointF> points, Size canvasSize)
        {
            if (points.Count == 0) return points;

            // Находим границы
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            foreach (var point in points)
            {
                if (!float.IsNaN(point.X) && !float.IsNaN(point.Y))
                {
                    minX = Math.Min(minX, point.X);
                    minY = Math.Min(minY, point.Y);
                    maxX = Math.Max(maxX, point.X);
                    maxY = Math.Max(maxY, point.Y);
                }
            }

            // Масштабируем и центрируем
            float width = maxX - minX;
            float height = maxY - minY;

            if (width == 0 || height == 0)
                return points;

            float scaleX = (canvasSize.Width * 0.8f) / width;
            float scaleY = (canvasSize.Height * 0.8f) / height;
            float scale = Math.Min(scaleX, scaleY);

            List<PointF> normalizedPoints = new List<PointF>();
            foreach (var point in points)
            {
                if (float.IsNaN(point.X) || float.IsNaN(point.Y))
                {
                    normalizedPoints.Add(point);
                }
                else
                {
                    float x = (point.X - minX) * scale + (canvasSize.Width - width * scale) / 2;
                    float y = (point.Y - minY) * scale + (canvasSize.Height - height * scale) / 2;
                    normalizedPoints.Add(new PointF(x, y));
                }
            }

            return normalizedPoints;
        }
    }

    public class TurtleState
    {
        public PointF Position { get; set; }
        public double Direction { get; set; }
        public float Step { get; set; }

        public TurtleState Clone()
        {
            return new TurtleState
            {
                Position = new PointF(Position.X, Position.Y),
                Direction = Direction,
                Step = Step
            };
        }
    }

    // Пример использования в WinForms
    public class LSystemPanel : Panel
    {
        public BaseLSystem lSystem;
        private List<PointF> points;

        public LSystemPanel()
        {
            lSystem = new BaseLSystem();
            points = new List<PointF>();
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
        }

        public void LoadLSystem(string filePath)
        {
            lSystem.LoadFromFile(filePath);
            GenerateFractal();
        }

        public void GenerateFractal()
        {
            points = lSystem.GeneratePoints(this.Size);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (points.Count == 0) return;

            using (Pen pen = new Pen(Color.Black, 1))
            {
                for (int i = 1; i < points.Count; i++)
                {
                    if (!float.IsNaN(points[i].X) && !float.IsNaN(points[i].Y) &&
                        !float.IsNaN(points[i - 1].X) && !float.IsNaN(points[i - 1].Y))
                    {
                        e.Graphics.DrawLine(pen, points[i - 1], points[i]);
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (points.Count > 0)
            {
                GenerateFractal();
            }
        }
    }
}
