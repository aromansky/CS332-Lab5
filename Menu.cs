using CS332_Lab5.BezierCurves;
using CS332_Lab5.LSistems;
using CS332_Lab5.LSystems;
using CS332_Lab5.MountainGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;


namespace CS332_Lab5
{
    public partial class Menu : Form
    {
        private BaseLSystem lSystem;
        private List<PointF> points;
        private List<TreeSegment> treeSegments;

        private MidpointDisplacement mountainGenerator;
        Timer previewTimer;

        private CompositeBezierCurve bezierCurve;

        private bool isTreeMode = false;
        private string fname = String.Empty;

        private bool task1 = false;
        private bool task2 = false;
        private bool task3 = false;

        private bool isDragging = false;
        private int draggedPointIndex = -1;

        public Menu()
        {
            lSystem = new BaseLSystem();
            points = new List<PointF>();
            treeSegments = new List<TreeSegment>();
            InitializeComponent();


            previewTimer = new Timer { Interval = 100 };
            mountainGenerator = new MidpointDisplacement();

            ChangeVisibleTask2(false);
            ChangeVisibleTask3(false);

            bezierCurve = new CompositeBezierCurve();

            Type type = bezierPanel.GetType();
            PropertyInfo propertyInfo = type.GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo.SetValue(bezierPanel, true, null);
        }

        private void GenerateFractal()
        {
            if (lSystem != null)
            {
                if (isTreeMode && lSystem is RealisticTreeLSystem treeSystem)
                {
                    treeSegments = treeSystem.GenerateTree(this.ClientSize);
                    points = new List<PointF>(); 
                }
                else
                {
                    points = lSystem.GeneratePoints(this.ClientSize);
                    treeSegments = new List<TreeSegment>(); 
                }
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.White);

            if (isTreeMode && treeSegments.Count > 0)
            {
                DrawTree(e.Graphics);
            }
            else if (points.Count > 0)
            {
                DrawStandardFractal(e.Graphics);
            }
        }

        private void DrawStandardFractal(Graphics graphics)
        {
            using (Pen pen = new Pen(Color.Black, 1))
            {
                for (int i = 1; i < points.Count; i++)
                {
                    if (!float.IsNaN(points[i].X) && !float.IsNaN(points[i].Y) &&
                        !float.IsNaN(points[i - 1].X) && !float.IsNaN(points[i - 1].Y))
                    {
                        graphics.DrawLine(pen, points[i - 1], points[i]);
                    }
                }
            }
        }

        private void DrawTree(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var segment in treeSegments.OrderByDescending(s => s.Level))
            {
                using (var pen = new Pen(segment.Color, segment.Thickness))
                {
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    graphics.DrawLine(pen, segment.StartPoint, segment.EndPoint);
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            lSystem.Iterations = (int)numericUpDown1.Value;
            GenerateFractal();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lSystem.UseRandomness = checkBox1.Checked;

            label2.Visible = checkBox1.Checked;
            numericUpDown2.Enabled = checkBox1.Checked;
            numericUpDown2.Visible = checkBox1.Checked;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            lSystem.RandomnessFactor = (int)numericUpDown2.Value / 100.00;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                lSystem = new RealisticTreeLSystem();
                isTreeMode = true;

                var treeSystem = (RealisticTreeLSystem)lSystem;
                treeSystem.BaseThickness = 6.0f;
                treeSystem.ThicknessDecay = 0.7f;
                treeSystem.TrunkColor = Color.SaddleBrown;
                treeSystem.LeafColor = Color.ForestGreen;
                treeSystem.BranchingLevels = 4;

                treeSystem.Iterations = (int)numericUpDown1.Value;
                treeSystem.UseRandomness = checkBox1.Checked;
                treeSystem.RandomnessFactor = (int)numericUpDown2.Value / 100.00;
            }
            else
            {
                lSystem = new BaseLSystem();
                isTreeMode = false;

                lSystem.Iterations = (int)numericUpDown1.Value;
                lSystem.UseRandomness = checkBox1.Checked;
                lSystem.RandomnessFactor = (int)numericUpDown2.Value / 100.00;
            }

            if (!String.IsNullOrEmpty(fname))
            {
                lSystem.LoadFromFile(fname);
                GenerateFractal();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (points.Count > 0 || treeSegments.Count > 0)
            {
                GenerateFractal();
            }
        }

        private void ChangeVisibleTask1(bool flag)
        {
            numericUpDown1.Visible = flag;
            numericUpDown2.Visible = flag && checkBox1.Checked; 
            checkBox1.Visible = flag;
            checkBox2.Visible = flag;

            label1.Visible = flag; 
            label2.Visible = flag && checkBox1.Checked; 

            numericUpDown1.Enabled = flag;
            numericUpDown2.Enabled = flag && checkBox1.Checked;
            checkBox1.Enabled = flag;
            checkBox2.Enabled = flag;

            button1.Enabled = flag;
            button1.Visible = flag;

            this.Invalidate();
            points.Clear();
            treeSegments.Clear();
        }

        private void ChangeVisibleTask2(bool flag)
        {
            mountainPreview.Visible = flag;
            roughnessTrackBar.Visible = flag;
            iterationsTrackBar.Visible = flag;
            generateMountainBtn.Visible = flag;
            resetMountainBtn.Visible = flag;
            iterationViewTrackBar.Visible = flag;
            roughnessLabel.Visible = flag;
            iterationsLabel.Visible = flag;
            currentIterationLabel.Visible = flag;

            if (flag)
            {
                UpdateMountainPreview();
                previewTimer.Start();
            }
            else
            {
                previewTimer.Stop();
            }
        }

        private void ChangeVisibleTask3(bool visible)
        {
            bezierPanel.Visible = visible;
            addPointBtn.Visible = visible;
            removePointBtn.Visible = visible;
            clearCurveBtn.Visible = visible;
            showPolygonCheck.Visible = visible;
            showTangentsCheck.Visible = visible;
            bezierStatusLabel.Visible = visible;

            if (visible)
            {
                bezierPanel.Invalidate();
                
                bezierStatusLabel.Text = "Кликните на экран чтобы добавить точки. Для удаления точки кликните правой кнопкой мыши.";
            }
        }

        private void UpdateMountainParameters()
        {
            mountainGenerator.Roughness = roughnessTrackBar.Value / 50.0;
            mountainGenerator.Iterations = iterationsTrackBar.Value;

            roughnessLabel.Text = $"Шероховатость: {mountainGenerator.Roughness}";
            iterationsLabel.Text = $"Итерации: {mountainGenerator.Iterations}";

            iterationViewTrackBar.Maximum = mountainGenerator.Iterations;
            UpdateMountainPreview();
        }

        private void UpdateMountainPreview()
        {
            if (task2 && mountainPreview.Visible)
            {
                var preview = mountainGenerator.GeneratePreview(
                    mountainPreview.Size,
                    Color.DarkGreen,
                    Color.LightBlue);

                mountainPreview.Image?.Dispose();
                mountainPreview.Image = preview;

                currentIterationLabel.Text = $"Итерация: {mountainGenerator.CurrentIteration}/{mountainGenerator.Iterations}";
            }
        }

        private void task1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            task1 = true;
            task2 = false;
            task3 = false;
            ChangeVisibleTask1(task1);
            ChangeVisibleTask2(!task1);
            ChangeVisibleTask3(!task1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "L-System files (*.lsys)|*.lsys|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fname = dialog.FileName;
                    lSystem.LoadFromFile(fname);
                    lSystem.Iterations = (int)numericUpDown1.Value;
                    GenerateFractal();
                }
            }
        }

        private void generateMountainBtn_Click(object sender, EventArgs e)
        {
            mountainGenerator.Generate();
            iterationViewTrackBar.Maximum = mountainGenerator.Iterations;
            iterationViewTrackBar.Value = mountainGenerator.Iterations;
            UpdateMountainPreview();
        }

        

        private void resetMountainBtn_Click(object sender, EventArgs e)
        {
            mountainGenerator.Reset();
            iterationViewTrackBar.Value = 0;
            UpdateMountainPreview();
        }

        private void task2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task2 = true;
            task1 = false;
            task3 = false;

            ChangeVisibleTask3(!task2);
            ChangeVisibleTask2(task2);
            ChangeVisibleTask1(!task2);
            

            if (task2)
            {
                UpdateMountainParameters();
            }
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void roughnessTrackBar_Scroll(object sender, EventArgs e)
        {
            UpdateMountainParameters();
        }

        private void iterationViewTrackBar_Scroll(object sender, EventArgs e)
        {
            mountainGenerator.GoToIteration(iterationViewTrackBar.Value);
            UpdateMountainPreview();
        }

        private void bezierPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawBezierCurve(e.Graphics);
        }

        private void DrawBezierCurve(Graphics graphics)
        {
            graphics.Clear(Color.PaleTurquoise);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (bezierCurve != null)
            {
                bezierCurve.Draw(graphics);
            }
        }

        private void BezierPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (!task3) return;

            PointF mousePos = new PointF(e.X, e.Y);
    
            // Проверяем, попали ли в точку
            int hitIndex = bezierCurve.HitTest(mousePos);
    
            if (e.Button == MouseButtons.Left)
            {
                if (hitIndex >= 0)
                {
                    // Начали перетаскивание точки
                    isDragging = true;
                    draggedPointIndex = hitIndex;
                    bezierCurve.SelectPoint(hitIndex);
                    bezierStatusLabel.Text = $"Перетаскивание точки {hitIndex}. Отпустите чтобы бросить.";
                }
                else
                {
                    // Добавляем новую точку
                    bezierCurve.AddPoint(mousePos);
                    bezierStatusLabel.Text = $"Точка добавлена. Всего точек: {bezierCurve.ControlPoints.Count}";
                }
                bezierPanel.Invalidate();
            }
            else if (e.Button == MouseButtons.Right && hitIndex >= 0)
            {
                // Удаляем точку правой кнопкой мыши
                bezierCurve.RemovePoint(hitIndex);
                bezierStatusLabel.Text = $"Точка {hitIndex} удалена. Всего точек: {bezierCurve.ControlPoints.Count}";
                bezierPanel.Invalidate();
            }
        }

        private void BezierPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!task3) return;

            if (isDragging && draggedPointIndex >= 0)
            {
                PointF mousePos = new PointF(e.X, e.Y);
                bezierCurve.MovePoint(draggedPointIndex, mousePos);
                bezierPanel.Invalidate();
            }
        }

        private void BezierPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (!task3) return;

            isDragging = false;
            draggedPointIndex = -1;
            bezierCurve.DeselectPoint();
            bezierStatusLabel.Text = "Готово. Клик - добавить точки, перетащить - переместить, правый клик - удалить.";
            bezierPanel.Invalidate();
        }

        private void AddPointBtn_Click(object sender, EventArgs e)
        {
            bezierStatusLabel.Text = "Режим добавления точек: Кликните на холст чтобы добавить контрольные точки";
        }

        private void RemovePointBtn_Click(object sender, EventArgs e)
        {
            bezierStatusLabel.Text = "Режим удаления точек: Кликните на точки чтобы удалить их";
        }

        private void ClearCurveBtn_Click(object sender, EventArgs e)
        {
            bezierCurve.Clear();
            bezierPanel.Invalidate();
            bezierStatusLabel.Text = "Кривая очищена. Кликните чтобы добавить новые точки.";
        }

        private void ShowPolygonCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (bezierCurve != null)
            {
                bezierCurve.ShowPolygon = showPolygonCheck.Checked;
                bezierPanel.Invalidate();
            }
        }

        private void ShowTangentsCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (bezierCurve != null)
            {
                bezierCurve.SmoothConnectionsEnabled = showTangentsCheck.Checked;
                bezierPanel.Invalidate();
            }
        }

        private void task3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task3 = !task3;
            task1 = false;
            task2 = false;
            
            ChangeVisibleTask1(!task3);
            ChangeVisibleTask2(!task3);

            ChangeVisibleTask3(task3);
        }
    }
}