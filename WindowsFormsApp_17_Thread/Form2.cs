using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_17_Thread
{
    public partial class Form2 : Form
    {
        Random rand = new Random();
        private BackgroundWorker[] worker = new BackgroundWorker[5];
        private ProgressBar[] progressBars = new ProgressBar[5];

        public Form2()
        {
            InitializeComponent();

            progressBars[0] = (ProgressBar)this.Controls.Find("progressBar1", true)[0];
            progressBars[1] = (ProgressBar)this.Controls.Find("progressBar2", true)[0];
            progressBars[2] = (ProgressBar)this.Controls.Find("progressBar3", true)[0];
            progressBars[3] = (ProgressBar)this.Controls.Find("progressBar4", true)[0];
            progressBars[4] = (ProgressBar)this.Controls.Find("progressBar5", true)[0];

            for (int i = 0; i < 5; i++)
            {
                worker[i] = new BackgroundWorker();
                worker[i].WorkerReportsProgress = true;
                worker[i].DoWork += Worker_DoWork;
                worker[i].ProgressChanged += Worker_ProgressChanged;
            }
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            object[] arguments = (object[])e.Argument;
            int index = (int)arguments[0];
            int speed = (int)arguments[1];

            int progress = 0;
            while (progress < 100)
            {
                Thread.Sleep(speed);
                progress += 10;
                if (progress > 100) progress = 100;

                worker.ReportProgress(progress, index);
            }
        }
        private void btn_Start_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                int speed = rand.Next(100, 1000);
                worker[i].RunWorkerAsync(new object[] { i, speed });
            }
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int index = (int)e.UserState;
            progressBars[index].Value = e.ProgressPercentage;
        }
    }
}
