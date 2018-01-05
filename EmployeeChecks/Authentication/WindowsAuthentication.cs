using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EmployeeChecks.Authentication
{
    public class WindowsAuthentication
    {
        //AD Authentication

        private string sLoginID;
        internal void Authenticate()
        {
            if ((HttpContext.Current != null))
            {
                if (HttpContext.Current.Request.LogonUserIdentity.IsAuthenticated == true)
                {
                    sLoginID = LoginID();

                }
                else
                {
                    sLoginID = HttpContext.Current.Request.UserHostAddress.ToString();
                }
            }
            else
            {
                sLoginID = Environment.MachineName;
            }

            HttpContext.Current.Session["loginId"] = sLoginID;
        }

        //Helper Methods - ADA

        public string SessionName()
        {
            var sessionName = LoginID();

            return sessionName;
        }

        private string LoginID()
        {
            string sName = HttpContext.Current.Request.LogonUserIdentity.Name;

            var session = sName.Substring(sName.IndexOf("\\") + 1);

            return session;
        }

        //Departmental Authentication
        internal bool LoadMe()
        {
            bool loadMe;

            var sqlConnection = CoreHRSqlConnection(); //Creates a connection to the CoreHR Live Database.
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT DEPARTMENT_DESCRIPTION, LDAP FROM vwLiveEmployees WHERE DEPARTMENT_DESCRIPTION = 'VOXPRO' AND LDAP ='" + sLoginID + "' AND [REPORTS_TO_PERSON_REFERENCE] = 431";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = sqlConnection;

            sqlConnection.Open();

            reader = cmd.ExecuteReader();
            loadMe = LoadMeChecker(reader);

            sqlConnection.Close();

            return loadMe;
        }

        private SqlConnection CoreHRSqlConnection()
        {
            var connect = ConfigurationManager.ConnectionStrings["RE_INT_IE_CoreConnectionStringEmployees"]; //Connection may change depending on connection string name.
            SqlConnection sqlConnection = new SqlConnection(connect.ConnectionString);

            return sqlConnection;
        }

        private bool LoadMeChecker(SqlDataReader reader)
        {
            var rowCount = 0;
            bool loadMe;

            while (reader.Read())
            {
                rowCount += 1;
            }

            if (rowCount > 0)
            {
                loadMe = true;
            }
            else
            {
                loadMe = false;
            }

            return loadMe;
        }
    }
}