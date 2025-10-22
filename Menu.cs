using CS332_Lab5.LSistems;
using CS332_Lab5.LSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS332_Lab5
{
    public partial class Menu : Form
    {
        private BaseLSystem lSystem;
        private List<PointF> points;
        private List<TreeSegment> treeSegments; 
        private bool isTreeMode = false;
        private string fname = String.Empty;

        private bool task1 = false;

        public Menu()
        {
            lSystem = new BaseLSystem();
            points = new List<PointF>();
            treeSegments = new List<TreeSegment>();
            InitializeComponent();
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

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
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
        }

        private void task1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            task1 = true;
            ChangeVisibleTask1(task1);
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
    }
}