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
        private BaseLSystem lsystem = new BaseLSystem();
     

        public Menu()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "L-system files (*.lsys)|*.lsys|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (lsystem.LoadFromFile(dialog.FileName))
                {
                    lsystem.Iterations = 4;
                    lsystem.StepLength = 5f;
                    lsystem.Randomness = 0; // 10% случайности
                    panel1.Invalidate(); // Перерисовать
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            lsystem.RenderAutoScale(e.Graphics, panel1.ClientRectangle);

            PointF center = new PointF(panel1.Width / 2, panel1.Height / 2);
            lsystem.Render(e.Graphics, center, lsystem.Iterations);
        }



        private void trackBarIterations_Scroll(object sender, EventArgs e)
        {
            lsystem.Iterations = trackBarIterations.Value;
            panel1.Invalidate();
        }
    }
}
