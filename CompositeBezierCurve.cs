using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace CS332_Lab5.BezierCurves
{
    public class CompositeBezierCurve
    {
        private List<PointF> controlPoints;
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

        public IReadOnlyList<PointF> ControlPoints => controlPoints.AsReadOnly();
        public int SegmentCount => Math.Max(0, (controlPoints.Count - 1) / 3);
        public bool HasPoints => controlPoints.Count >= 4;

        public CompositeBezierCurve()
        {
            controlPoints = new List<PointF>();
        }

        public void AddPoint(PointF point)
        {
            if (controlPoints.Count == 0)
            {
                // Первая точка
                controlPoints.Add(point);
            }
            else if ((controlPoints.Count - 1) % 3 == 0)
            {
                PointF lastPoint = controlPoints.Last();

                // Направление для плавного соединения
                Vector2 direction;
                if (controlPoints.Count >= 4)
                {
                    PointF prevControl = controlPoints[controlPoints.Count - 2];
                    direction = new Vector2(lastPoint.X - prevControl.X, lastPoint.Y - prevControl.Y).Normalize();
                }
                else
                {
                    direction = new Vector2(1, 0);
                }

                float defaultLength = 50f;

                PointF control1 = new PointF(
                    lastPoint.X + direction.X * defaultLength,
                    lastPoint.Y + direction.Y * defaultLength
                );

                PointF control2 = new PointF(
                    point.X - direction.X * defaultLength,
                    point.Y - direction.Y * defaultLength
                );

                controlPoints.Add(control1);
                controlPoints.Add(control2);
                controlPoints.Add(point);

                if (SmoothConnectionsEnabled)
                    UpdateAllSmoothConnections();
            }
        }

        public void RemovePoint(int pointIndex)
        {
            if (pointIndex < 0 || pointIndex >= controlPoints.Count) return;

            if (pointIndex % 3 == 0)
            {
                // Удаляем весь сегмент, связанный с этой якорной точкой
                if (pointIndex == 0 && controlPoints.Count >= 4)
                    controlPoints.RemoveRange(0, 4);
                else if (pointIndex == controlPoints.Count - 1 && controlPoints.Count >= 4)
                    controlPoints.RemoveRange(controlPoints.Count - 4, 4);
                else if (pointIndex > 0 && pointIndex < controlPoints.Count - 1)
                {
                    int start = pointIndex - 1;
                    if (start + 3 < controlPoints.Count)
                        controlPoints.RemoveRange(start, 3);
                }

                if (SmoothConnectionsEnabled)
                    UpdateAllSmoothConnections();
            }
            else
            {
                controlPoints.RemoveAt(pointIndex);
                if (SmoothConnectionsEnabled)
                    UpdateAllSmoothConnections();
            }
        }

        public int HitTest(PointF point)
        {
            for (int i = 0; i < controlPoints.Count; i++)
            {
                if (Distance(point, controlPoints[i]) < HitTestRadius)
                    return i;
            }
            return -1;
        }

        public void MovePoint(int pointIndex, PointF newPosition)
        {
            if (pointIndex < 0 || pointIndex >= controlPoints.Count) return;

            controlPoints[pointIndex] = newPosition;

            if (pointIndex % 3 == 0)
            {
                UpdateSmoothConnection(pointIndex);
            }
            else
            {
                int anchor = FindAnchorForControlPoint(pointIndex);
                if (anchor != -1)
                    UpdateSmoothConnection(anchor);
            }
        }

        private void UpdateSmoothConnection(int anchorIndex)
        {
            if (anchorIndex < 0 || anchorIndex >= controlPoints.Count) return;
            if (anchorIndex % 3 != 0) return;

            if (anchorIndex > 0)
                UpdateControlPointsForAnchor(anchorIndex);
            if (anchorIndex < controlPoints.Count - 3)
                UpdateControlPointsForAnchor(anchorIndex);
        }

        private void UpdateControlPointsForAnchor(int anchorIndex)
        {
            if (!SmoothConnectionsEnabled) return;
            // anchorIndex должен быть кратен 3 (якорная точка)
            if (anchorIndex % 3 != 0) return;
            if (anchorIndex < 3 || anchorIndex > controlPoints.Count - 4) return;

            PointF prevAnchor = controlPoints[anchorIndex - 3];
            PointF nextAnchor = controlPoints[anchorIndex + 3];
            PointF currentAnchor = controlPoints[anchorIndex];

            Vector2 tangent = new Vector2(nextAnchor.X - prevAnchor.X, nextAnchor.Y - prevAnchor.Y).Normalize();

            float distPrev = Distance(currentAnchor, prevAnchor);
            float distNext = Distance(currentAnchor, nextAnchor);

            // Контрольные точки по касательной
            controlPoints[anchorIndex - 1] = new PointF(
                currentAnchor.X - tangent.X * distPrev * 0.3f,
                currentAnchor.Y - tangent.Y * distPrev * 0.3f);

            controlPoints[anchorIndex + 1] = new PointF(
                currentAnchor.X + tangent.X * distNext * 0.3f,
                currentAnchor.Y + tangent.Y * distNext * 0.3f);
        }

        private int FindAnchorForControlPoint(int controlIndex)
        {
            if (controlIndex % 3 == 1)
                return controlIndex + 1;
            else if (controlIndex % 3 == 2)
                return controlIndex - 1;
            return -1;
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

        private PointF[] GetSegmentPoints(int segmentIndex)
        {
            int startIndex = segmentIndex * 3;
            return new PointF[]
            {
                controlPoints[startIndex],
                controlPoints[startIndex + 1],
                controlPoints[startIndex + 2],
                controlPoints[startIndex + 3]
            };
        }

        private float Distance(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        // === Отрисовка ===
        public void Draw(Graphics g)
        {
            if (controlPoints.Count < 4) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (ShowPolygon)
                DrawControlPolygon(g);

            DrawBezierCurves(g);
            DrawControlPoints(g);
        }

        private void DrawBezierCurves(Graphics g)
        {
            using (Pen pen = new Pen(CurveColor, CurveWidth))
            {
                for (int i = 0; i < SegmentCount; i++)
                {
                    PointF[] seg = GetSegmentPoints(i);
                    PointF[] curve = new PointF[SegmentsPerCurve + 1];

                    for (int j = 0; j <= SegmentsPerCurve; j++)
                    {
                        float t = j / (float)SegmentsPerCurve;
                        curve[j] = CalculateBezierPoint(seg[0], seg[1], seg[2], seg[3], t);
                    }

                    g.DrawLines(pen, curve);
                }
            }
        }

        private void DrawControlPolygon(Graphics g)
        {
            using (Pen polyPen = new Pen(PolygonColor, PolygonWidth) { DashStyle = DashStyle.Dash })
            {
                for (int i = 0; i < SegmentCount; i++)
                {
                    int start = i * 3;
                    g.DrawLine(polyPen, controlPoints[start], controlPoints[start + 1]);
                    g.DrawLine(polyPen, controlPoints[start + 2], controlPoints[start + 3]);

                    if (ShowTangents)
                        g.DrawLine(polyPen, controlPoints[start + 1], controlPoints[start + 2]);
                }
            }
        }

        private void DrawControlPoints(Graphics g)
        {
            for (int i = 0; i < controlPoints.Count; i++)
            {
                Color color = (i % 3 == 0) ? PointColor : TangentColor;
                float r = (i % 3 == 0) ? PointRadius : PointRadius * 0.7f;

                using (Brush brush = new SolidBrush(color))
                    g.FillEllipse(brush, controlPoints[i].X - r, controlPoints[i].Y - r, r * 2, r * 2);

                if (i == selectedPointIndex)
                {
                    using (Pen p = new Pen(Color.Yellow, 2))
                        g.DrawEllipse(p, controlPoints[i].X - r - 2, controlPoints[i].Y - r - 2, (r + 2) * 2, (r + 2) * 2);
                }
            }
        }

        public void Clear()
        {
            controlPoints.Clear();
            selectedPointIndex = -1;
        }

        public void SelectPoint(int index) => selectedPointIndex = index;
        public void DeselectPoint() => selectedPointIndex = -1;

        public void UpdateAllSmoothConnections()
        {
            for (int i = 3; i < controlPoints.Count - 3; i += 3)
                UpdateControlPointsForAnchor(i);
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
