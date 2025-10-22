using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS332_Lab5.LSystems
{
    using CS332_Lab5.LSistems;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class RealisticTreeLSystem : BaseLSystem
    {
        public float BaseThickness { get; set; } = 5.0f;
        public float ThicknessDecay { get; set; } = 0.7f;
        public Color TrunkColor { get; set; } = Color.Brown;
        public Color LeafColor { get; set; } = Color.Green;
        public int BranchingLevels { get; set; } = 1;

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
                    // Переход на новую ветвь
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
                        i++;
                    }
                    continue;
                }

                var segment = new TreeSegment
                {
                    StartPoint = currentInfo.StartPoint,
                    EndPoint = points[i],
                    Thickness = currentInfo.Thickness,
                    Level = currentInfo.Level,
                    Color = CalculateColor(currentInfo.Level)
                };

                treeSegments.Add(segment);

                currentInfo.StartPoint = points[i];

                // Возврат из ветвления
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

            return treeSegments;
        }

        private Color CalculateColor(int level)
        {
            if (level == 0)
                return TrunkColor;

            float transition = Math.Min(1.0f, level / (float)BranchingLevels);

            int r = (int)(TrunkColor.R * (1 - transition) + LeafColor.R * transition);
            int g = (int)(TrunkColor.G * (1 - transition) + LeafColor.G * transition);
            int b = (int)(TrunkColor.B * (1 - transition) + LeafColor.B * transition);

            return Color.FromArgb(r, g, b);
        }

        public void DrawTree(Graphics graphics, List<TreeSegment> segments)
        {
            // Сортируем по уровню и толщине: толстые линии рисуются первыми (на заднем плане)
            foreach (var segment in segments.OrderBy(s => s.Level).ThenByDescending(s => s.Thickness))
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

    // Класс для хранения информации о сегментах дерева
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

    // Вспомогательный класс для отслеживания состояния при построении дерева
    public class TreeSegmentInfo
    {
        public PointF StartPoint { get; set; }
        public int Level { get; set; }
        public float Thickness { get; set; }
    }
}
