using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TeamConfigurator.Interfaces;

namespace Desktop
{
    public partial class Main : Form
    {
        private ProblemResult result;

        private ISolver solver;

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
            startToolStripButton.Enabled = false;

            worker.RunWorkerAsync(new ProblemResult() { Groups = new Dictionary<int, List<int>>() });
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ProblemResult result = e.Argument as ProblemResult;

            solver.Start(worker, result);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Get result and redraw the solution.
            result = e.UserState as ProblemResult;
            Refresh();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Get result and redraw the solution.
            result = e.UserState as ProblemResult;
            Refresh();

            // Write statistics and results.

            startToolStripButton.Enabled = true;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            var g = e.Graphics;

            var rows = GCD(Size.Width, result.Groups.Keys.Count);
            var columns = 

            g.FillRectangle(new SolidBrush(Color.White), panel.DisplayRectangle);

            foreach (var group in result.Groups.Keys)
            {
                var brush = new SolidBrush(Color.FromArgb(group));

            }
            
            g.FillEllipse

            /*Point[] points = new Point[4];

            points[0] = new Point(0, 0);
            points[1] = new Point(0, panel.Height);
            points[2] = new Point(panel.Width, panel.Height);
            points[3] = new Point(panel.Width, 0);

            Brush brush = new SolidBrush(Color.DarkGreen);

            g.FillPolygon(brush, points);*/

        }

        private static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}
