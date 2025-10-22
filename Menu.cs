using CS332_Lab5.LSistems;
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


        public Menu()
        {
            lSystem = new BaseLSystem();
            points = new List<PointF>();
            InitializeComponent();
        }


        private void GenerateFractal()
        {
            if (lSystem != null)
            {
                points = lSystem.GeneratePoints(this.ClientSize);
                this.Invalidate();
            }
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "L-System files (*.lsys)|*.lsys|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    lSystem.LoadFromFile(dialog.FileName);
                    lSystem.Iterations = (int)numericUpDown1.Value;
                    GenerateFractal();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.White);

            if (points == null || points.Count == 0)
                return;

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
    }
}
