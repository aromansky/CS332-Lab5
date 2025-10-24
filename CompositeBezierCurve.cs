using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace CS332_Lab5.BezierCurves
{
    public class CompositeBezierCurve
    {
        private List<PointF> MyPoints;
        private List<Line> lines;

        public bool SmoothConnectionsEnabled { get; set; } = true;
        private int selectedPointIndex = -1;
        private const float PointRadius = 6f;
        private const float HitTestRadius = 8f;

        public Color CurveColor { get; set; } = Color.Blue;
        public Color PolygonColor { get; set; } = Color.Black;
        public Color PointColor { get; set; } = Color.Red;
        public Color TangentColor { get; set; } = Color.Green;
        public float CurveWidth { get; set; } = 3f;
        public float PolygonWidth { get; set; } = 1f;
        public bool ShowPolygon { get; set; } = true;
        public bool ShowTangents { get; set; } = true;
        public int SegmentsPerCurve { get; set; } = 50;

        public IReadOnlyList<PointF> ControlPoints => MyPoints.AsReadOnly();
        public int SegmentCount => Math.Max(0, (MyPoints.Count - 1) / 3);
        public bool HasPoints => MyPoints.Count >= 4;

        private const float STEP = 0.02f;

        public CompositeBezierCurve()
        {
            MyPoints = new List<PointF>();
            lines = new List<Line>();
        }

        private void DrawPoints(Graphics g)
        {
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Color color = (i % 3 == 0) ? PointColor : TangentColor;
                float r = (i % 3 == 0) ? PointRadius : PointRadius * 0.7f;

                using (Brush brush = new SolidBrush(color))
                    g.FillEllipse(brush, MyPoints[i].X - r, MyPoints[i].Y - r, r * 2, r * 2);

                if (i == selectedPointIndex)
                {
                    using (Pen p = new Pen(Color.Yellow, 2))
                        g.DrawEllipse(p, MyPoints[i].X - r - 2, MyPoints[i].Y - r - 2, (r + 2) * 2, (r + 2) * 2);
                }
            }
        }

        public void Draw(Graphics g)
        {
            if (MyPoints.Count < 4) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисование контрольных линий
            for (int i = 0; i < MyPoints.Count; i += 3)
            {
                if (i + 1 < MyPoints.Count)
                {
                    g.DrawLine(new Pen(PolygonColor, PolygonWidth),
                              MyPoints[i], MyPoints[i + 1]);
                }

                if (i + 1 < MyPoints.Count && i + 2 < MyPoints.Count)
                {
                    g.DrawLine(new Pen(PolygonColor, PolygonWidth),
                              MyPoints[i + 1], MyPoints[i + 2]);
                }

                if (i + 2 < MyPoints.Count && i + 3 < MyPoints.Count)
                {
                    g.DrawLine(new Pen(PolygonColor, PolygonWidth),
                              MyPoints[i + 2], MyPoints[i + 3]);
                }
            }

            // Рисование кривых Безье
            using (Pen curvePen = new Pen(CurveColor, CurveWidth))
            {
                foreach (var line in lines)
                {
                    g.DrawLine(curvePen, line.Begin, line.End);
                }
            }

            DrawPoints(g);
        }

        public int NearestAnchorIndex(PointF pos, float radius)
        {
            if (MyPoints.Count == 0)
                return -1;

            float bestDistance = radius * radius;
            int bestIndex = -1;

            for (int i = 0; i < MyPoints.Count; i += 3)
            {
                float d2 = DistanceSquared(pos, MyPoints[i]);
                if (d2 < bestDistance)
                {
                    bestDistance = d2;
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        private PointF CalculateBezierPoint(PointF p0, PointF p1, PointF p2, PointF p3, float t)
        {
            float u = 1 - t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t2 = t * t;
            float t3 = t2 * t;

            float x = u3 * p0.X + 3 * u2 * t * p1.X + 3 * u * t2 * p2.X + t3 * p3.X;
            float y = u3 * p0.Y + 3 * u2 * t * p1.Y + 3 * u * t2 * p2.Y + t3 * p3.Y;

            return new PointF(x, y);
        }

        public void Update()
        {
            lines.Clear();
            if (MyPoints.Count < 4)
                return;

            for (int i = 0; i < MyPoints.Count - 3; i += 3)
            {
                PointF p0 = MyPoints[i];
                PointF p1 = MyPoints[i + 1];
                PointF p2 = MyPoints[i + 2];
                PointF p3 = MyPoints[i + 3];

                PointF prev = p0;
                float t = STEP;
                while (t <= 1.0f)
                {
                    PointF cur = CalculateBezierPoint(p0, p1, p2, p3, t);
                    lines.Add(new Line(prev, cur, CurveWidth, CurveColor));
                    prev = cur;
                    t += STEP;
                }
            }
        }

        public void AddPoint(PointF clickPos)
        {
            int n = MyPoints.Count;

            switch (n % 3)
            {
                case 0:
                    // Просто добавляем новую опорную точку
                    MyPoints.Add(clickPos);
                    break;
                case 1:
                    // Первая контрольная точка после опорной
                    if (n == 1)
                    {
                        // Если это вторая точка в кривой, просто добавляем
                        MyPoints.Add(clickPos);
                    }
                    else
                    {
                        PointF lastAnchor = MyPoints[n - 1];
                        PointF prevControl = MyPoints[n - 2];

                        // Находим середину между последней опорной и предыдущей контрольной
                        PointF mid = new PointF(
                            (lastAnchor.X + prevControl.X) * 0.5f,
                            (lastAnchor.Y + prevControl.Y) * 0.5f
                        );

                        // Перемещаем последнюю опорную на середину
                        PointF coord = MyPoints[n - 1];
                        MyPoints[n - 1] = mid;
                        MyPoints.Add(coord);
                        // Клик пользователя становится новой контрольной точкой
                        MyPoints.Add(clickPos);
                    }
                    break;
                case 2:
                    // Вторая контрольная просто добавляется
                    MyPoints.Add(clickPos);
                    break;
            }

            Update();
        }

        public void RemovePoint(PointF pos, float r)
        {
            if (MyPoints.Count < 4)
            {
                Clear();
                return;
            }

            int? idx = NearestAnchorIndex(pos, r);
            if (idx.HasValue && idx.Value != -1)
            {
                int index = idx.Value;
                if (index % 3 != 0)
                    return;

                if (index == 0)
                {
                    int end = Math.Min(index + 3, MyPoints.Count);
                    MyPoints.RemoveRange(index, end - index);
                }
                else if (index + 1 == MyPoints.Count)
                {
                    MyPoints.RemoveRange(index - 2, 3);
                }
                else
                {
                    MyPoints.RemoveRange(index - 1, 3);
                }
                Update();
            }
        }

        public void MovePoint(int index, PointF newPos)
        {
            if (index >= MyPoints.Count)
                return;

            PointF delta = new PointF(newPos.X - MyPoints[index].X, newPos.Y - MyPoints[index].Y);
            MyPoints[index] = newPos;

            // Если index — опорная точка, сдвинем соседние control
            if (index % 3 == 0)
            {
                if (index > 0)
                {
                    MyPoints[index - 1] = new PointF(
                        MyPoints[index - 1].X + delta.X,
                        MyPoints[index - 1].Y + delta.Y);
                }
                if (index + 1 < MyPoints.Count)
                {
                    MyPoints[index + 1] = new PointF(
                        MyPoints[index + 1].X + delta.X,
                        MyPoints[index + 1].Y + delta.Y);
                }
            }

            Update();
        }

        public void Clear()
        {
            MyPoints.Clear();
            lines.Clear();
            selectedPointIndex = -1;
        }

        // Сохраняем старый интерфейс для обратной совместимости
        public void RemovePoint(int pointIndex)
        {
            if (pointIndex < 0 || pointIndex >= MyPoints.Count) return;

            if (pointIndex % 3 == 0)
            {
                RemovePoint(MyPoints[pointIndex], HitTestRadius);
            }
            else
            {
                MyPoints.RemoveAt(pointIndex);
                Update();
            }
        }

        public int HitTest(PointF point)
        {
            for (int i = 0; i < MyPoints.Count; i++)
            {
                if (Distance(point, MyPoints[i]) < HitTestRadius)
                    return i;
            }
            return -1;
        }

        private float Distance(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private float DistanceSquared(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        public void SelectPoint(int index) => selectedPointIndex = index;
        public void DeselectPoint() => selectedPointIndex = -1;

        public void UpdateAllSmoothConnections()
        {
            // В правильной реализации нет автоматического сглаживания
            // Эта функция оставлена для обратной совместимости
            Update();
        }

        // Вспомогательный класс для линий
        private struct Line
        {
            public PointF Begin { get; }
            public PointF End { get; }
            public float Width { get; }
            public Color Color { get; }

            public Line(PointF begin, PointF end, float width, Color color)
            {
                Begin = begin;
                End = end;
                Width = width;
                Color = color;
            }
        }
    }

    public struct Vector2
    {
        public float X, Y;
        public Vector2(float x, float y) { X = x; Y = y; }
        public float Length => (float)Math.Sqrt(X * X + Y * Y);

        public Vector2 Normalize()
        {
            float len = Length;
            return len > 0 ? new Vector2(X / len, Y / len) : new Vector2(0, 0);
        }

        public static Vector2 operator *(Vector2 v, float s) => new Vector2(v.X * s, v.Y * s);
    }
}