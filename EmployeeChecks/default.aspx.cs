using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Threading;
using ClosedXML.Excel;
using OfficeOpenXml;

namespace EmployeeChecks
{
    public partial class _default : System.Web.UI.Page
    {
        //String strConnectionServerRoot = "Data Source=vpro-sql1;Integrated Security=True";
        //String strConnectionMain = "Data Source=vpro-sql1;Initial Catalog=CapacityPlanning;Integrated Security=True";
        //String sqlMainCommand = "SELECT * FROM KeyValuationActual";
        public static BackgroundWorker worker = new BackgroundWorker();
        public static bool stopWorker = false;

        int[] checkArray = new int[3] { 0, 0, 0 };

        String strConnection = "Data Source=vpro-sql1;Initial Catalog=RE_INT_IE_DataQuality;Integrated Security=True;MultipleActiveResultSets=True";

        DataBaseOperations dbOperations = new DataBaseOperations();

        string[] sqlArray = new string[3] { "SELECT [PERSON_REFERENCE], [Agent],[Person Ref] FROM [RE_INT_IE_DataQuality].[dbo].[Employee_DQ_WFM_Genesys]",
            "SELECT [Person Ref], [Agent] FROM [RE_INT_IE_DataQuality].[dbo].[Employee_DQ_WFM_Impact360]",
            "SELECT [Person Ref], [Agent] FROM [RE_INT_IE_DataQuality].[dbo].[Employee_DQ_RE_INT_IE_Injixo]"};

        protected void Page_Load(object sender, EventArgs e)
        {
            var auth = new Authentication.WindowsAuthentication();
            var session = auth.SessionName();

            auth.Authenticate();

            sessioname.Text = session;

            var loadMe = auth.LoadMe();

            if (loadMe == false)
            {
                //MsgBox("No User found! Redirecting...");
                Response.Redirect("http://www.voxprogroup.com/");
            }

            InitializeBackgroundWorker();
            InitializeElements();
        }

        private void InitializeBackgroundWorker()
        {
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
            //Run worker Asynchronously
            worker.RunWorkerAsync();
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if(worker != null)
            {
                Thread.Sleep(3000);
                if (!stopWorker)
                {
                    worker.RunWorkerAsync();
                }
                else
                {
                    while (stopWorker)
                    {
                        Thread.Sleep(6000);
                    }
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            //Background work to be activated
            if(checkArray[0] == 1)
            {
                genesys_wait.Visible = true;
            }
            else
            {
                genesys_wait.Visible = false;
            }

            if (checkArray[1] == 1)
            {
                impact_wait.Visible = true;
            }
            else
            {
                impact_wait.Visible = false;
            }

            if (checkArray[2] == 1)
            {
                injixo_wait.Visible = true;
            }
            else
            {
                injixo_wait.Visible = false;
            }

            //https://chsakell.com/2013/07/20/background-processes-in-asp-net-web-forms/
        }

        private void InitializeElements()
        {
            injixo_results_grid.DataBinding += new EventHandler(this.ChangeCheckArray);
            InitializeSearchButtons();
        }

        private void InitializeSearchButtons()
        {
            genesys_check.Click += new EventHandler(this.GetGenesysCheck_Click);
            injixo_check.Click += new EventHandler(this.GetInjixoCheck_Click);
        }

        private void IntializeModifyButtons()
        {
            //injixo_modify.Click += new EventHandler(this.GetInjixoCheck_Click);
        }

        private void GetInjixoCheck_Click(object sender, EventArgs e)
        {
            
            try
            {
                checkArray[2] = 1;
                DataSet ds = new DataSet();
                ds = dbOperations.ExecuteQuerytoDataset(strConnection, sqlArray[2]);

                //injixo_results_grid.DataBound += ChangeCheckArray(2, 0);
                injixo_results_grid.DataSource = ds;
                injixo_results_grid.DataBind();

                injixo_modify.Visible = true;
            }
            catch (Exception ex)
            {
                QueryMessages_txt.Text = (ex.Message);
            }
        }

        private void ChangeCheckArray(object sender, EventArgs e)
        {

            GridView grid = (GridView) sender;
            string id = grid.ID;

            switch (id)
            {
                case "injixo_results_grid":
                    checkArray[2] = 0;
                    break;
                default:
                    break;
            }
            //checkArray[position] = value;
        }

        private void GetGenesysCheck_Click(object sender, EventArgs e)
        {
            try
            {
                genesys_results_grid.DataSource = dbOperations.ExecuteQuerytoDataset(strConnection, sqlArray[0]);
                genesys_results_grid.DataBind();
            }
            catch (Exception ex)
            {
                QueryMessages_txt.Text = (ex.Message);
            }
        }

        /// <summary>
        ///  After a click at the "Export to excel" button, it triggers the process to gather the full data and export an Excel file
        /// </summary>
        protected void ExportToExcel_click(object sender, EventArgs e, string check)
        {
            SqlConnection conn = new SqlConnection(strConnectionForUpdates);
            SqlCommand cmd = new SqlCommand("SELECT * FROM KeyValuationActual_destination", conn);
            DataTable dt = new DataTable();

            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            conn.Close();


            XLWorkbook workbook = new XLWorkbook();
            workbook.Worksheets.Add(dt, "Valuations");

            HttpResponse httpResponse = Response;
            httpResponse.Clear();
            //Response.Buffer = true;
            httpResponse.AddHeader("content-disposition", "attachment;filename="+check+"_" + DateTime.Today.ToString("yyyyMMdd") + ".xlsx");
            //Response.Charset = "";
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (ExcelPackage xlpk = new ExcelPackage())
            {
                ExcelWorksheet ws = xlpk.Workbook.Worksheets.Add("Valuations");
                ws.Cells["A1"].LoadFromDataTable(dt, true);

                var ms = new System.IO.MemoryStream();
                xlpk.SaveAs(ms);
                ms.WriteTo(httpResponse.OutputStream);
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

            da.Dispose();
        }
    }
}