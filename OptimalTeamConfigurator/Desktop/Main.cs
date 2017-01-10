using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            /*
             var solverConfiguration = new SolverConfiguration<GeneticAlgorithmConfiguration>();
                        solverConfiguration.Path = args[1];

                        var solver = new GeneticAlgorithm.GeneticAlgorithm(solverConfiguration.Configuration);
                        solver.Solve();
             */
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void startToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            var g = e.Graphics;

            g.FillRectangle(new SolidBrush(Color.White), panel.DisplayRectangle);

            /*Point[] points = new Point[4];

            points[0] = new Point(0, 0);
            points[1] = new Point(0, panel.Height);
            points[2] = new Point(panel.Width, panel.Height);
            points[3] = new Point(panel.Width, 0);

            Brush brush = new SolidBrush(Color.DarkGreen);

            g.FillPolygon(brush, points);*/

        }
    }
}
