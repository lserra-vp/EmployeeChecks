using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Threading;
using System.Web.Security;
using System.Web.SessionState;

namespace BackgroundProcess
{
    public class BackgroundProcess:System.Web.HttpApplication
    {
        public static BackgroundWorker worker = new BackgroundWorker();
        public static bool stopWorker = false;

        int[] processArray = new int[3] { 0, 0, 0 };

        protected void Application_Start(object sender, EventArgs e)
        {
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);

            //Run worker Asynchronously
            worker.RunWorkerAsync();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (worker != null)
                worker.CancelAsync();
        }

        private int CheckProcessArray(int array_position)
        {
            return processArray[array_position];
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            //Background work to be activated
            if (CheckProcessArray(0) == 1)
            {
                genesys_wait.Visible = true;
            }
            else
            {
                genesys_wait.Visible = false;
            }

            if (processArray[1] == 1)
            {
                impact_wait.Visible = true;
            }
            else
            {
                impact_wait.Visible = false;
            }

            if (processArray[2] == 1)
            {
                injixo_wait.Visible = true;
            }
            else
            {
                injixo_wait.Visible = false;
            }
        }

        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}