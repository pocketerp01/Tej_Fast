using System;
using System.Text;
using System.Data;
using System.IO;
using System.Data.OleDb;
using Oracle.ManagedDataAccess.Client;

namespace Fin_WFinConn_DLL
{
    public static class Fin_Conn_DLL
    {
        public static StreamReader sReader;
        public static string GetConnPwd(string compCode, string serverIP, string serviceName)
        {
            string sysUser = "SY" + "ST" + "EM";
            string sysPwd = "LE" + "AD" + "ER";

            string sIP = serverIP;
            string port = "1521";
            if (serverIP.Contains(":"))
            {
                port = serverIP.Split(':')[1];
                sIP = serverIP.Split(':')[0];
            }

            string conStr = "provider=MSDAORA.1;data source=" + sIP + " ;user id=" + sysUser.Trim().ToUpper() + " ;password=" + sysPwd + "";
            conStr = "Data Source=(DESCRIPTION="
           + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + sIP.Trim() + ")(PORT=" + port + ")))"
           + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + serviceName + ")));"
           + "User Id= " + sysUser + "; Password= " + sysPwd + ";";
            string newPwd = "";
            newPwd = _getData("select tname from tab where tname='REPCAT$_SYNC'", conStr);
            if (newPwd == "REPCAT$_SYNC")
            {
                newPwd = _getData("select TRIM(CREF)||SUBSTR(TRIM(GREFSPW),1,4)||TRIM(pref2)||'-'||MREFPSW AS PREF2 from REPCAT$_SYNC where trim(cref)='" + compCode.Trim() + "'", conStr);
                if (newPwd.Length > 0)
                {
                    newPwd = newPwd.Split('-')[1].Trim() == "OLDRULE" ? compCode + "FIN" : newPwd.Split('-')[0].Trim();
                }
            }
            else newPwd = compCode + "FIN";
            return newPwd;
        }
        public static string _getData(string cmdQuery, string connectionString)
        {
            string rVal = "";
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                using (OracleCommand cmd = new OracleCommand(cmdQuery, con))
                {
                    using (OracleDataReader oledbDr = cmd.ExecuteReader())
                    {
                        if (oledbDr != null)
                        {
                            DataTable dt = new DataTable();
                            if (oledbDr.HasRows) dt.Load(oledbDr);
                            rVal = (dt.Rows.Count > 0) ? dt.Rows[0][0].ToString().Trim() : "";
                        }
                    }
                }
            }
            return rVal;
        }
    }
}
