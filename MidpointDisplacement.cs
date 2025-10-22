using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CS332_Lab5.MountainGenerator
{
    public class MidpointDisplacement
    {
        private Random random;
        private List<double[]> iterationsHistory;

        public double Roughness { get; set; } = 0.5;
        public double InitialHeight { get; set; } = 0.0;
        public int Iterations { get; set; } = 8;
        public int CurrentIteration { get; private set; } = 0;

        public double[] CurrentHeights { get; private set; }
        public int PointsCount => CurrentHeights?.Length ?? 0;

        public MidpointDisplacement()
        {
            random = new Random();
            iterationsHistory = new List<double[]>();
        }

        public void Generate()
        {
            iterationsHistory.Clear();
            CurrentIteration = 0;

            int pointsCount = 2;
            CurrentHeights = new double[pointsCount];
            CurrentHeights[0] = InitialHeight;
            CurrentHeights[1] = InitialHeight;

            iterationsHistory.Add((double[])CurrentHeights.Clone());

            for (int iter = 0; iter < Iterations; iter++)
            {
                PerformIteration();
                CurrentIteration++;
                iterationsHistory.Add((double[])CurrentHeights.Clone());
            }
        }

        public void StepByStep()
        {
            if (CurrentIteration == 0)
            {
                int pointsCount = 2;
                CurrentHeights = new double[pointsCount];
                CurrentHeights[0] = InitialHeight;
                CurrentHeights[1] = InitialHeight;
                iterationsHistory.Clear();
                iterationsHistory.Add((double[])CurrentHeights.Clone());
            }
            else if (CurrentIteration < Iterations)
            {
                PerformIteration();
                CurrentIteration++;
                iterationsHistory.Add((double[])CurrentHeights.Clone());
            }
        }

        public void Reset()
        {
            CurrentIteration = 0;
            CurrentHeights = null;
            iterationsHistory.Clear();
        }

        public void GoToIteration(int iteration)
        {
            if (iteration >= 0 && iteration < iterationsHistory.Count)
            {
                CurrentIteration = iteration;
                CurrentHeights = (double[])iterationsHistory[iteration].Clone();
            }
        }

        private void PerformIteration()
        {
            int oldCount = CurrentHeights.Length;
            int newCount = oldCount * 2 - 1;
            double[] newHeights = new double[newCount];

            for (int i = 0; i < oldCount; i++)
            {
                newHeights[i * 2] = CurrentHeights[i];
            }

            double segmentLength = 1.0 / (oldCount - 1); 

            for (int i = 1; i < newCount; i += 2)
            {
                double left = newHeights[i - 1];
                double right = newHeights[i + 1];
                double average = (left + right) / 2.0;

                double displacement = (random.NextDouble() - 0.5) * 2.0 * Roughness * segmentLength;
                newHeights[i] = average + displacement;
            }

            CurrentHeights = newHeights;
        }

        public PointF[] GetPointsForDrawing(Size canvasSize, float scale = 0.8f)
        {
            if (CurrentHeights == null || CurrentHeights.Length < 2)
                return new PointF[0];

            PointF[] points = new PointF[CurrentHeights.Length];

            double minHeight = CurrentHeights.Min();
            double maxHeight = CurrentHeights.Max();
            double heightRange = maxHeight - minHeight;

            if (heightRange == 0) heightRange = 1; 

            float canvasHeight = canvasSize.Height * scale;
            float canvasWidth = canvasSize.Width * scale;
            float startX = (canvasSize.Width - canvasWidth) / 2;
            float startY = (canvasSize.Height - canvasHeight) / 2 + canvasHeight;

            for (int i = 0; i < CurrentHeights.Length; i++)
            {
                float x = startX + (i * canvasWidth) / (CurrentHeights.Length - 1);

                double normalizedHeight = (CurrentHeights[i] - minHeight) / heightRange;
                float y = startY - (float)(normalizedHeight * canvasHeight);

                points[i] = new PointF(x, y);
            }

            return points;
        }

        public Bitmap GeneratePreview(Size size, Color mountainColor, Color background)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            using (Brush brush = new SolidBrush(background))
            {
                g.FillRectangle(brush, 0, 0, size.Width, size.Height);

                if (CurrentHeights != null && CurrentHeights.Length >= 2)
                {
                    PointF[] points = GetPointsForDrawing(size);
                    if (points.Length > 0)
                    {
                        using (Pen pen = new Pen(mountainColor, 2))
                        using (Brush fillBrush = new SolidBrush(Color.FromArgb(50, mountainColor)))
                        {
                            PointF[] filledPoints = new PointF[points.Length + 2];
                            Array.Copy(points, filledPoints, points.Length);
                            filledPoints[filledPoints.Length - 2] = new PointF(points[points.Length - 1].X, size.Height);
                            filledPoints[filledPoints.Length - 1] = new PointF(points[0].X, size.Height);

                            g.FillPolygon(fillBrush, filledPoints);
                            g.DrawLines(pen, points);
                        }
                    }
                }
            }
            return bmp;
        }
    }
}