using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TeamConfigurator.Interfaces;

namespace Desktop
{
    public partial class Main : Form
    {
        private ProblemResult result;

        private Dictionary<int, Dictionary<int, bool>> relationships;

        private ISolver solver;

        private string outputFileName;

        private StreamWriter file;

        public Main()
        {
            InitializeComponent();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            //Stream stream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var configuration = new SolverConfiguration<GeneticAlgorithmConfiguration>();
                    configuration.Path = openFileDialog.FileName;
                    solver = new GeneticAlgorithm.GeneticAlgorithm(configuration.Configuration);

                    //if ((stream = openFileDialog.OpenFile()) != null)
                    //{
                    //    using (stream)
                    //    {
                    using (TextReader reader = File.OpenText(configuration.Configuration.File))
                    {
                        outputFileName = string.Format("{0}_{1}.txt", configuration.Configuration.File.Replace(".dat", ""), DateTime.Now.Ticks);

                        string text = reader.ReadLine();
                        string[] bits = text.Split(' ');

                        int members = int.Parse(bits[0]);
                        int relations = int.Parse(bits[1]);

                        configuration.Configuration.PeopleNumber = members;

                        relationships = new Dictionary<int, Dictionary<int, bool>>();
                        for (int i = 1; i <= members; i++)
                        {
                            relationships.Add(i, new Dictionary<int, bool>());
                        }

                        for (int i = 0; i < relations; i++)
                        {
                            text = reader.ReadLine();
                            bits = text.Split(' ');

                            int memberOne = int.Parse(bits[0]);
                            int memberTwo = int.Parse(bits[1]);

                            relationships[memberOne].Add(memberTwo, true);
                        }

                        solver.Relationships = relationships;
                        startToolStripButton.Enabled = true;
                    }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void startToolStripButton_Click(object sender, EventArgs e)
        {
            startToolStripButton.Enabled = false;
            file = new StreamWriter(outputFileName);

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

            // Write progress statistics.
            /*foreach (var group in result.Groups.Keys)
            {
                file.Write(string.Format("{0}: ", group));

                foreach (var member in result.Groups[group])
                {
                    file.Write(string.Format("{0} ", member));
                }

                file.WriteLine();
            }
            file.WriteLine();*/
            file.WriteLine("Fitness: {0}, at progress {1}%", result.Fitness, e.ProgressPercentage);
            //file.WriteLine("=====================================================================");
            //file.WriteLine();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Get result and redraw the solution.
            result = e.UserState as ProblemResult;
            Refresh();

            // Write statistics and results.
            foreach (var group in result.Groups.Keys)
            {
                file.Write(string.Format("{0}: ", group));

                foreach (var member in result.Groups[group])
                {
                    file.Write(string.Format("{0} ", member));
                }

                file.WriteLine();
            }
            file.WriteLine();
            file.WriteLine("Fitness: {0}, at progress 100%", result.Fitness);
            file.WriteLine("=====================================================================");
            file.WriteLine();

            file.Close();

            startToolStripButton.Enabled = true;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            var random = new Random();
            var panel = sender as Panel;
            var g = e.Graphics;

            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));

            if (result != null)
            {
                var rows = GCD(Size.Width, result.Groups.Keys.Count);
                var columns = result.Groups.Keys.Count / rows;

                if (rows == 1 || columns == 1)
                {
                    rows = 2;
                    columns = (result.Groups.Keys.Count / 2) + 1;
                }

                g.FillRectangle(new SolidBrush(Color.White), panel.DisplayRectangle);
                var black = new SolidBrush(Color.Black);

                var region_x = Size.Width / columns;
                var region_y = Size.Height / rows;

                int group = 1;
                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        if (result.Groups.ContainsKey(group))
                        {
                            KnownColor randomColorName = names[random.Next(names.Length)];
                            Color randomColor = Color.FromKnownColor(randomColorName);
                            var brush = new SolidBrush(randomColor);

                            foreach (var member in result.Groups[group])
                            {
                                int x = (column * region_x) + random.Next(region_x);
                                int y = (row * region_y) + random.Next(region_y);
                                g.FillEllipse(brush, new Rectangle(x, y, 4, 4));
                                //g.DrawString(member + "", new Font("Times New Roman", 1), black, x, y);
                            }
                        }
                    }
                }
            }

        }

        private static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}
