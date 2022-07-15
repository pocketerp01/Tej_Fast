using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;



    public class Multiton
    {
        //read-only dictionary to track multitons
        private static IDictionary<string, Multiton> _Tracker = new Dictionary<string, Multiton> { };


        //public String U_COCD, UserCode;
        //public String U_MBR;
        //public String U_CLIENT_ID, U_FIRM, U_FIRM_TYPE, U_YEAR, U_FYEAR, U_DATERANGE, U_CDT1, U_CDT2, U_UNAME, U_USERID, U_ULEVEL, U_MBR_NAME;
        //public string controls_mst, subdomain_mst, cg_com_name, role_mst, username_mst, userid_mst, utype_mst,
        //    clientid_mst, unitid_mst, clientname_mst, unitname_mst, Ac_Year_id, Ac_Year, Ac_From_Date,
        //    Ac_dbo.to_date, year_to, year_from, com_yr, Module_id, Module_Name, M_Module3, ulevel_mst, urights_mst,
        //    pp_filepath, pp_filename, cp_filepath, cp_filename;
        public string UserCode, frm_mbr, frm_vty, frm_fchar, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, DateRange;
        public string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2, CSR;

        public string timezone, dateformat, timeformat, datetimeformat;


        private Multiton(string key, string Usercode)
        {
            frm_qstr = key;

            if (Usercode == null || Usercode == "") Usercode = (String)Multiton.GetSession(key, "userCode");
            if (!key.Trim().Equals("IGuid"))
            {
                UserCode = Usercode;
                frm_cocd = Multiton.Get_Mvar(frm_qstr, "U_COCD");
                frm_uname = Multiton.Get_Mvar(frm_qstr, "U_UNAME");
                frm_myear = Multiton.Get_Mvar(frm_qstr, "U_YEAR");
                frm_ulvl = Multiton.Get_Mvar(frm_qstr, "U_ULEVEL");
                frm_mbr = Multiton.Get_Mvar(frm_qstr, "U_MBR");
                DateRange = Multiton.Get_Mvar(frm_qstr, "U_DATERANGE");
                frm_UserID = Multiton.Get_Mvar(frm_qstr, "U_USERID");
                frm_CDT1 = Multiton.Get_Mvar(frm_qstr, "U_Cdt1");
                frm_CDT2 = Multiton.Get_Mvar(frm_qstr, "U_Cdt2");
                CSR = Multiton.Get_Mvar(frm_qstr, "C_S_R");
                timezone = "+05:30";
                dateformat = "dd/MM/yyyy";
                timeformat = "H:mm:ss";
                datetimeformat = dateformat + " " + timeformat;
                ///for ERP
                ///
            }
            UserCode = Usercode;
        }
        //public void AddVal()
        //{
        //}
        public static Multiton GetInstance(string key)
        {
            Multiton item = null;
            lock (_Tracker)
            {
                if (!_Tracker.TryGetValue(key, out item))
                {
                    item = new Multiton(key, "");
                    _Tracker.Add(key, item);
                }
            }
            return item;
        }
        public static Multiton GetInstance(string key, string UserCode)
        {

            Multiton item = null;
            lock (_Tracker)
            {
                if (!_Tracker.TryGetValue(key, out item))
                {
                    item = new Multiton(key, UserCode);
                    _Tracker.Add(key, item);
                }
                if (item.UserCode == null)
                {
                    string cc = GetCO_CD();
                    SetSession(key, "userCode", cc);
                    item = new Multiton(key, cc);
                }
            }
            return item;
        }
        public static Multiton GetInstance(string key, string Co_cd, bool Refresh)
        {

            Multiton item = null;
            if (Refresh) _Tracker.Remove(key);
            lock (_Tracker)
            {
                if (!_Tracker.TryGetValue(key, out item))
                {
                    item = new Multiton(key, Co_cd);
                    _Tracker.Add(key, item);
                }
            }
            return item;
        }
        public static Multiton ReNew(string key, string Co_cd)
        {
            return GetInstance(key, Co_cd, true);

        }
        //public static Multiton GetInstance(string key, bool Refresh)
        //{

        //    Multiton item = null;
        //    if (Refresh) _Tracker.Remove(key);
        //    lock (_Tracker)
        //    {
        //        if (!_Tracker.TryGetValue(key, out item))
        //        {
        //            item = new Multiton(key);
        //            _Tracker.Add(key, item);
        //        }
        //        if (item.OConn == null) item = new Multiton(key);
        //    }
        //    return item;
        //}
        //public static Multiton ReNew(string key)
        //{
        //    return GetInstance(key, true);

        //}
        public static string GetCO_CD()
        {
            string path = HttpContext.Current.Server.MapPath("~/foxtns.txt");

            string str = "";
            StreamReader srciplCO = null;
            if (File.Exists(path)) srciplCO = new StreamReader(path);


            str = srciplCO.ReadToEnd().Trim();
            if (str.Contains("\r")) str = str.Replace("\r", ",");
            if (str.Contains("\n")) str = str.Replace("\n", ",");
            str = str.Replace(",,", ",");

            string Co = str.Split(',')[0].ToUpper();

            srciplCO.Close();
            return Co;
        }
        public static string GetCookie(string MyGuid, string name)
        {
            string val = "";
            if (HttpContext.Current.Request.Cookies[MyGuid + "_" + name] != null)
            {
                val = HttpContext.Current.Request.Cookies[MyGuid + "_" + name].Value.ToString();
            }
            return val;
        }
        public static void SetCookie(string MyGuid, string name, string value)
        {
            //Writing Multiple values in single cookie
            HttpContext.Current.Response.Cookies.Remove(MyGuid + "_" + name);
            HttpCookie hc = new HttpCookie(MyGuid + "_" + name);
            hc.Value = value;
            HttpContext.Current.Response.Cookies.Add(hc);
        }
        public static void SetSession(string MyGuid, string SessionName, object value)
        {
            HttpContext.Current.Session[MyGuid + "_" + SessionName] = value;
        }
        public static object GetSession(string MyGuid, string SessionName)
        {
            return HttpContext.Current.Session[MyGuid + "_" + SessionName];
        }
        public static string getUserCode()
        {

            return "EIT";
            string res = "-";
            //if (HttpContext.Current.Session[MyGuid + "_cocd_mst"] != null)
            //{
            //    res = (String)HttpContext.Current.Session[MyGuid + "_cocd_mst"];
            //}


            string url = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).OriginalString;

            //http://localhost:13660/erp/login_main
            //url = "http://test.skyinfy.com/erp/school_admin/school_admin_configpage";
            //url = "http://skyinfy.com/erp/school_admin/school_admin_configpage";

            //url = "https://cali.skyinfy.com/home/login";
            //url = "http://name.skyinfy.com/Inventory/mat_req?m_id=b8LgZhj%2BfsvYOywM82wsGg%3D%3D&mid=Tf2i5qHhrAg%3D";
            var cnt = 0;
            try
            {
                String[] HEADS = url.Split('/');
                if (HEADS[2].ToString().Trim().ToUpper().Contains(".COM") || HEADS[2].ToString().Trim().ToUpper().Contains(".IN"))
                {
                    //if (url.ToLower().Contains("https"))
                    //{
                    //    cnt = url.Replace("https://", "").Split('/')[0].Split('.').Count();
                    //    if (cnt >= 3) res = url.Replace("https://", "").Split('/')[0].Split('.')[0].ToString();
                    //    //if (res.Trim().ToUpper().Equals("WWW")) res = (String)HttpContext.Current.Session[MyGuid + "_cocd_mst"];
                    //}
                    //else if (url.ToLower().Contains("http"))
                    //{

                    //    cnt = url.Replace("http://", "").Split('/')[0].Split('.').Count();
                    //    if (cnt >= 3) res = url.Replace("http://", "").Split('/')[0].Split('.')[0].ToString();
                    //    //if (res.Trim().ToUpper().Equals("WWW")) res = (String)HttpContext.Current.Session[MyGuid + "_cocd_mst"];

                    //}
                    res = HEADS[2].Split('.')[0];
                }
            }
            catch (Exception err)
            { }

            //string path = @"C:\skyinfy\mytns2.txt";
            string path = HttpRuntime.AppDomainAppPath + "\\mytns2.txt";

            string str = "", srv = "", PWD = "", constr = "", IP = "";
            if (res.Trim().Length < 2)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path);
                        str = sr.ReadToEnd().Trim();
                        if (str.Contains("\r")) str = str.Replace("\r", ",");
                        if (str.Contains("\n")) str = str.Replace("\n", ",");
                        str = str.Replace(",,", ",");
                        IP = str.Split(',')[1];
                        if (res.Trim().Equals("-")) res = str.Split(',')[0].ToLower();
                        sr.Close();
                    }
                }
                catch (Exception err)
                {
                    //showmsg(1, err.Message.ToString(), 2);
                }
            }
            return res.ToUpper();
        }
        public static string GetError(Exception exception)
        {
            int lineno = 0;
            int i = 0;
            string fName = "";
            StackFrame fram;
            try
            {
                do
                {
                    fram = new System.Diagnostics.StackTrace(exception, true).GetFrame(i);
                    lineno = fram.GetFileLineNumber();
                    i++;
                }
                while (lineno < 1);
                fName = fram.GetFileName().Split('\\').Last();
            }
            catch (Exception err)
            {
                return exception.Message;
            }
            return lineno + " in File " + fName;
        }

        //public static string connStringo(string pco_cd)
        //{
        //    string path = HttpContext.Current.Server.MapPath("~/mytns2.txt");
        //    string str = "", srv = "", constr = "";
        //    try
        //    {
        //        if (File.Exists(path))
        //        {
        //            StreamReader sr = new StreamReader(path);
        //            str = sr.ReadToEnd().Trim();
        //            if (str.Contains("\r")) str = str.Replace("\r", ",");
        //            if (str.Contains("\n")) str = str.Replace("\n", ",");
        //            str = str.Replace(",,", ",");
        //            return str;
        //        }
        //    }
        //    catch { }
        //    return constr;
        //}
        public static string connString(string co_cd)
        {
            //if (co_cd == null) co_cd = Multiton.GetCO_CD();
            //string cd = "", IP = "", srv = "", sp_cd = "", nPwd = "";
            //string path = HttpContext.Current.Server.MapPath("~/foxtns.txt");
            //string str = "";
            //try
            //{
            //    cd = co_cd;
            //    if (File.Exists(path)) { }

            //    using (StreamReader sr_fgen = new StreamReader(path))
            //    {
            //        str = sr_fgen.ReadToEnd().Trim();
            //        if (str.Contains("\r")) str = str.Replace("\r", ",");
            //        if (str.Contains("\n")) str = str.Replace("\n", ",");
            //        str = str.Replace(",,", ",");
            //        IP = str.Split(',')[1]; srv = str.Split(',')[2];
            //        sr_fgen.Close();

            //        if (str.Contains("3881") || str.Contains("4881") || str.Contains("5881") || str.Contains("6881") || str.Contains("7881"))
            //        { sp_cd = str.Split(',')[3]; }
            //    }
            //}
            //catch { }

            //nPwd = "" + co_cd + "PD";
            //string cPwd = "";
            ////cPwd = Fin_Conn_DLL.GetConnPwd(co_cd, IP, srv);
            //if (cPwd.Length > 0) nPwd = cPwd;
            //string constr = "Data Source=(DESCRIPTION="
            //     + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + IP + ")(PORT=1521)))"
            //     + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + srv + ")));"
            //     + "User Id= PD" + co_cd + "; Password= " + nPwd + ";";

            //constr += "Min Pool Size=10;Connection Lifetime=120;Connection Timeout=60;Incr Pool Size=5; Decr Pool Size=2";


            String constr = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=KIOT;Persist Security Info=True;User ID=SA;Password=LEADER";
            return constr;
        }


        public static string connString_old(string pco_cd)
        {

            string tpath = HttpContext.Current.Server.MapPath("~/kabiradb.txt");
            string str = "";
            try
            {
                if (File.Exists(tpath))
                {
                    StreamReader sr = new StreamReader(tpath);
                    str = sr.ReadToEnd().Trim();
                    if (str.Contains("\r")) str = str.Replace("\r", ",");
                    if (str.Contains("\n")) str = str.Replace("\n", ",");
                    str = str.Replace(",,", ",");

                }
            }
            catch { }
            //Enter in the user id and password.

            string conString = "User Id=kabiradb;Password=Baghel@12345;";
            string path = HttpContext.Current.Server.MapPath("~/" + str);
            //Enter port, host name or IP, service name, and wallet directory for your Oracle Autonomous DB.
            conString += "Data Source=(description=(address=(protocol=tcps)(port=1522)(host=adb.ap-mumbai-1.oraclecloud.com))" +
                "(connect_data=(service_name=ansuj5efkmfech0_kabiradb_high.atp.oraclecloud.com))(SECURITY = (MY_WALLET_DIRECTORY = " + path + ")));";
            return conString;

        }

        
        static DataRow fgen_oporow;
        public static StreamReader sr_fgen;
        public static DataTable dt_uniq;
        public static string fnVl;

        /// <summary>
        /// Function to Set value 
        /// </summary>
        /// <param name="UNIQ_ID">Query String with uniq key work</param>
        /// <param name="P_VAR_NAME">Parameter Name</param>
        /// <param name="P_VAR_VALUE">Parameter Value</param>
        public static void Set_Mvar(string UNIQ_ID, string P_VAR_NAME, string P_VAR_VALUE)
        {
             fgenMV.Fn_Set_Mvar(UNIQ_ID, P_VAR_NAME, P_VAR_VALUE);
            return;
            Multiton multiton = Multiton.GetInstance(UNIQ_ID);
           

            try
            {
                if (dt_uniq.Columns.Count <= 0)
                {
                    dt_uniq = new DataTable();
                    dt_uniq.Columns.Add(new DataColumn("srno", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("UID", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("MV_NAME", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("MV_VALUE", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("MV_DATE", typeof(string)));
                }
            }
            catch (Exception ERR)
            {
                dt_uniq = new DataTable();
                dt_uniq.Columns.Add(new DataColumn("srno", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("UID", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("MV_NAME", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("MV_VALUE", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("MV_DATE", typeof(string)));
            }

            if (dt_uniq.Rows.Count > 0)
            {
                DataView dt_view;
                dt_view = new DataView(dt_uniq, "UID='" + UNIQ_ID.Trim() + "' and MV_NAME='" + P_VAR_NAME.ToString().Trim() + "' ", "UID", DataViewRowState.CurrentRows);
                if (dt_view.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow dr_u in dt_uniq.Rows)
                    {
                        if ((dr_u["UID"].ToString().Trim() + dr_u["MV_NAME"].ToString().Trim()) == (dt_view[0]["UID"].ToString().Trim() + dt_view[0]["MV_NAME"].ToString().Trim()))
                        {
                            dt_uniq.Rows[i].Delete();
                            break;
                        }
                        i++;
                    }
                }
            }
            if (P_VAR_VALUE != null)
            {
                fgen_oporow = null;
                fgen_oporow = dt_uniq.NewRow();
                fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
                fgen_oporow["UID"] = UNIQ_ID.ToUpper();
                fgen_oporow["MV_NAME"] = P_VAR_NAME;
                fgen_oporow["MV_VALUE"] = EncryptDecrypt.Encrypt(P_VAR_VALUE).ToString();
                fgen_oporow["MV_DATE"] = DateTime.Now.ToString("dd/MM/yyyy");
                dt_uniq.Rows.Add(fgen_oporow);

                if (P_VAR_NAME != "U_SEEKSQL") Set_in_TXT(UNIQ_ID, dt_uniq);
            }
            fgenMV.dt_uniq = dt_uniq;
        }
        public static void Set_in_TXT(string UNIQ_ID, DataTable file_to_write_txt)
        {
            try
            {
                string vpath = HttpContext.Current.Server.MapPath("~/forms/tej-base/Log_File/" + UNIQ_ID + ".txt");
                del_file(vpath);
                StreamWriter w = new StreamWriter(vpath, true);
                foreach (DataRow dr_file_to_write_txt in file_to_write_txt.Rows)
                {
                    if (dr_file_to_write_txt["UID"].ToString().Trim().ToUpper() == UNIQ_ID.ToUpper())
                        w.WriteLine(UNIQ_ID + "~" + dr_file_to_write_txt["MV_NAME"].ToString().Trim() + "~" + dr_file_to_write_txt["MV_VALUE"].ToString().Trim());
                }
                w.Flush();
                w.Close();
            }
            catch { }
        }
        public static DataTable Read_TXT(string UNIQ_ID, string srch_var_name)
        {
            string vpath = HttpContext.Current.Server.MapPath("~/forms/tej-base/Log_File/" + UNIQ_ID + ".txt");

            if (File.Exists(vpath)) sr_fgen = new StreamReader(vpath);

            string[] str = sr_fgen.ReadToEnd().Trim().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (dt_uniq.Columns.Count <= 0)
                {
                    dt_uniq = new DataTable();
                    dt_uniq.Columns.Add(new DataColumn("srno", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("UID", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("MV_NAME", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("MV_VALUE", typeof(string)));
                    dt_uniq.Columns.Add(new DataColumn("MV_DATE", typeof(string)));
                }
            }
            catch
            {
                dt_uniq = new DataTable();
                dt_uniq.Columns.Add(new DataColumn("srno", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("UID", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("MV_NAME", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("MV_VALUE", typeof(string)));
                dt_uniq.Columns.Add(new DataColumn("MV_DATE", typeof(string)));
            }
            foreach (string item in str)
            {
                string suc = "0";
                if (item.Split('~')[0].ToString().Trim().ToUpper() == UNIQ_ID.ToUpper())
                {
                    fgen_oporow = null;
                    fgen_oporow = dt_uniq.NewRow();
                    if (dt_uniq.Rows.Count <= 0)
                    {
                        fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
                        fgen_oporow["UID"] = UNIQ_ID.ToUpper();
                        fgen_oporow["MV_NAME"] = item.Split('~')[1].ToString().Trim();
                        fgen_oporow["MV_VALUE"] = item.Split('~')[2].ToString().Trim();
                        fgen_oporow["MV_DATE"] = item.Split('~')[3].ToString().Trim();
                        suc = "1";
                    }
                    else
                    {
                        if (item.Split('~')[1].ToString().Trim() == srch_var_name)
                        {
                            fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
                            fgen_oporow["UID"] = UNIQ_ID.ToUpper();
                            fgen_oporow["MV_NAME"] = item.Split('~')[1].ToString().Trim();
                            fgen_oporow["MV_VALUE"] = item.Split('~')[2].ToString().Trim();
                            fgen_oporow["MV_DATE"] = item.Split('~')[3].ToString().Trim();
                            suc = "1";
                        }
                    }
                    if (suc == "1") dt_uniq.Rows.Add(fgen_oporow);
                }
            }
            sr_fgen.Close();
            return dt_uniq;
        }
        public static void Delete_Older_Data()
        {
            try
            {
                if (dt_uniq.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow dr_u in dt_uniq.Rows)
                    {
                        if (Convert.ToDateTime(dr_u["MV_DATE"].ToString().Trim()) <= System.DateTime.Now.AddDays(-2))
                        {
                            dt_uniq.Rows[i].Delete();
                            fgenMV.dt_uniq.Rows[i].Delete();
                        }
                        i++;
                    }
                }
            }
            catch { }
        }
        public static void Delete_Older_Files()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/forms/tej-base/Log_File/"));
                var files = di.GetFiles();
                var filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-2)));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-2)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }

                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/forms/tej-base/Barcode/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/forms/tej-base/xmlfile/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
            }
            catch { }
        }
        public static DataTable Mvar_Rows(string Q_ID)
        {
            DataTable dtvaluesend = new DataTable();
            try
            {
                if (dt_uniq.Rows.Count > 0) { }
                DataView dt_view;
                dt_view = new DataView(dt_uniq, "UID='" + Q_ID.ToString().Trim().ToUpper() + "'", "UID", DataViewRowState.CurrentRows);
                if (dt_view.Count > 0)
                {
                    dtvaluesend = dt_view.ToTable();
                }
            }
            catch { }
            return dtvaluesend;
        }
        /// <summary>
        /// To get values 
        /// </summary>
        /// <param name="UNIQ_ID">This is Unique ID</param>
        /// <param name="P_VAR_NAME">This will be value</param>
        /// <returns>Find the value and send</returns>
        public static string Get_Mvar(string UNIQ_ID, string P_VAR_NAME)
        {
            return fgenMV.Fn_Get_Mvar(UNIQ_ID, P_VAR_NAME);
            string P_VAR_VALUE = "0";
            try
            {
                if (dt_uniq.Rows.Count <= 0) dt_uniq = Read_TXT(UNIQ_ID, P_VAR_NAME);

                DataView dt_view;
                dt_view = new DataView(dt_uniq, "UID='" + UNIQ_ID.Trim() + "' and MV_NAME='" + P_VAR_NAME.Trim() + "'", "UID", DataViewRowState.CurrentRows);
                if (dt_view.Count > 0)
                {

                    P_VAR_VALUE = EncryptDecrypt.Decrypt(dt_view[0]["MV_VALUE"].ToString().Trim()).ToString();
                }
                else
                {
                    dt_uniq = Read_TXT(UNIQ_ID, P_VAR_NAME);
                    dt_view = new DataView(dt_uniq, "UID='" + UNIQ_ID.Trim() + "' and MV_NAME='" + P_VAR_NAME.Trim() + "'", "UID", DataViewRowState.CurrentRows);
                    if (dt_view.Count > 0)
                    {
                        P_VAR_VALUE = EncryptDecrypt.Decrypt(dt_view[0]["MV_VALUE"].ToString().Trim()).ToString();
                    }
                }

            }
            catch { }
            return P_VAR_VALUE;
        }
        public static void del_file(string Full_Path_to_del)
        {
            try
            {
                if (System.IO.File.Exists(Full_Path_to_del)) System.IO.File.Delete(Full_Path_to_del);
            }
            catch { }
        }
    }
