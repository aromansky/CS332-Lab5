using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CS332_Lab5.LSystems
{
    using CS332_Lab5.LSistems;

    public class RealisticTreeLSystem : BaseLSystem
    {
        public float BaseThickness { get; set; } = 8.0f;
        public float ThicknessDecay { get; set; } = 0.85f;
        public Color TrunkColor { get; set; } = Color.FromArgb(101, 67, 33); // Темно-коричневый
        public Color LeafColor { get; set; } = Color.FromArgb(34, 139, 34);  // Зеленый
        public int BranchingLevels { get; set; } = 5;

        private List<TreeSegment> treeSegments;

        public RealisticTreeLSystem()
        {
            treeSegments = new List<TreeSegment>();
            UseRandomness = true;
            RandomnessFactor = 0.15;
        }

        public List<TreeSegment> GenerateTree(Size canvasSize)
        {
            var points = GeneratePoints(canvasSize);
            return ConvertPointsToTreeSegments(points);
        }

        protected new List<PointF> InterpretString(string lSystemString, Size canvasSize)
        {
            treeSegments.Clear();
            var points = base.InterpretString(lSystemString, canvasSize);
            return points;
        }

        private List<TreeSegment> ConvertPointsToTreeSegments(List<PointF> points)
        {
            treeSegments.Clear();
            Stack<TreeSegmentInfo> segmentStack = new Stack<TreeSegmentInfo>();

            int currentLevel = 0;
            float currentThickness = BaseThickness;

            TreeSegmentInfo currentInfo = new TreeSegmentInfo
            {
                Level = currentLevel,
                Thickness = currentThickness,
                StartPoint = points.Count > 0 ? points[0] : PointF.Empty
            };

            for (int i = 1; i < points.Count; i++)
            {
                if (float.IsNaN(points[i].X) || float.IsNaN(points[i].Y))
                {
                    if (i + 1 < points.Count && !float.IsNaN(points[i + 1].X))
                    {
                        segmentStack.Push(currentInfo);

                        currentLevel++;
                        currentThickness *= ThicknessDecay;

                        currentInfo = new TreeSegmentInfo
                        {
                            Level = currentLevel,
                            Thickness = currentThickness,
                            StartPoint = points[i + 1]
                        };

                        i++; // Пропускаем NaN и переходим к новой точке
                    }
                    continue;
                }

                var segment = new TreeSegment
                {
                    StartPoint = currentInfo.StartPoint,
                    EndPoint = points[i],
                    Thickness = currentInfo.Thickness,
                    Level = currentInfo.Level,
                    // временно ставим цвет; окончательно установим ниже по фактической максимальной глубине
                    Color = TrunkColor
                };

                treeSegments.Add(segment);

                currentInfo.StartPoint = points[i];

                if (i + 1 < points.Count && float.IsNaN(points[i + 1].X))
                {
                    if (segmentStack.Count > 0)
                    {
                        currentInfo = segmentStack.Pop();
                        currentLevel = currentInfo.Level;
                        currentThickness = currentInfo.Thickness;
                    }
                }
            }

            // вычисляем фактическую максимальную глубину (max Level)
            int maxLevel = 0;
            if (treeSegments.Count > 0)
                maxLevel = treeSegments.Max(s => s.Level);

            // пересчитываем цвета с использованием фактического maxLevel
            for (int i = 0; i < treeSegments.Count; i++)
            {
                treeSegments[i].Color = CalculateColor(treeSegments[i].Level, maxLevel);
            }

            return treeSegments;
        }


        private Color CalculateColor(int level, int maxLevel)
        {
            if (maxLevel <= 0) // защита от деления на ноль
                return TrunkColor;

            // нормализуем уровень по фактической глубине
            float t = level / (float)maxLevel;

            // Clamp
            t = Math.Max(0f, Math.Min(1f, t));

            // Smoothstep
            t = t * t * (3 - 2 * t);

            int r = (int)(TrunkColor.R * (1 - t) + LeafColor.R * t);
            int g = (int)(TrunkColor.G * (1 - t) + LeafColor.G * t);
            int b = (int)(TrunkColor.B * (1 - t) + LeafColor.B * t);

            return Color.FromArgb(r, g, b);
        }

        //public void DrawTree(Graphics graphics, List<TreeSegment> segments)
        //{
        //    // Сначала рисуем толстые линии (нижние уровни)
        //    foreach (var segment in segments.OrderBy(s => s.Level).ThenByDescending(s => s.Thickness))
        //    {
        //        using (var pen = new Pen(segment.Color, segment.Thickness))
        //        {
        //            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        //            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        //            graphics.DrawLine(pen, segment.StartPoint, segment.EndPoint);
        //        }
        //    }
        //}

        private void DrawTree(Graphics graphics, List<TreeSegment> segments)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Рисуем от низких уровней (ствол) к высоким (ветви/листья)
            foreach (var segment in treeSegments.OrderBy(s => s.Level).ThenByDescending(s => s.Thickness))
            {
                using (var pen = new Pen(segment.Color, segment.Thickness))
                {
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    graphics.DrawLine(pen, segment.StartPoint, segment.EndPoint);
                }
            }
        }

        public new void DrawTree(Graphics graphics, Size canvasSize)
        {
            var segments = GenerateTree(canvasSize);
            DrawTree(graphics, segments);
        }

        public void DrawLeaves(Graphics graphics, List<TreeSegment> segments, float leafSize = 3.0f)
        {
            var endPoints = segments
                .Where(s => s.Level >= BranchingLevels - 1)
                .Select(s => s.EndPoint)
                .Distinct();

            using (var leafBrush = new SolidBrush(LeafColor))
            {
                foreach (var point in endPoints)
                {
                    graphics.FillEllipse(leafBrush,
                        point.X - leafSize / 2,
                        point.Y - leafSize / 2,
                        leafSize, leafSize);
                }
            }
        }
    }

    public class TreeSegment
    {
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        public float Thickness { get; set; }
        public int Level { get; set; }
        public Color Color { get; set; }
        public float Length => CalculateLength();

        private float CalculateLength()
        {
            float dx = EndPoint.X - StartPoint.X;
            float dy = EndPoint.Y - StartPoint.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }

    public class TreeSegmentInfo
    {
        public PointF StartPoint { get; set; }
        public int Level { get; set; }
        public float Thickness { get; set; }
    }
}
