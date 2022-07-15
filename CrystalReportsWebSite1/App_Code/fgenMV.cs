using System;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.Generic;
using Models;


    public class fgenMV
    {
        static DataRow fgen_oporow;
     
        
        public static HttpContext context;
        public static StreamReader sr_fgen;
        public static DataTable dt_uniq, iconTableFull, fin_rsys_upd;
        public static string fnVl;
      
    
        /// <summary>
        /// Function to Set value 
        /// </summary>
        /// <param name="UNIQ_ID">Query String with uniq key work</param>
        /// <param name="P_VAR_NAME">Parameter Name</param>
        /// <param name="P_VAR_VALUE">Parameter Value</param>
        public static void Fn_Set_Mvar(string UNIQ_ID, string P_VAR_NAME, string P_VAR_VALUE)
        {
            Singleton.Fn_Set_Mvar(UNIQ_ID, P_VAR_NAME, P_VAR_VALUE);
            return;
            if (UNIQ_ID != null)
            {
                try
                {
                    if (dt_uniq == null || dt_uniq.Columns.Count <= 0)
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

                if (dt_uniq.Rows.Count > 1)
                {
                    try
                    {
                        DataView dt_view;
                        dt_view = new DataView(dt_uniq, "UID='" + UNIQ_ID.Trim() + "' and MV_NAME='" + P_VAR_NAME.ToString().Trim() + "' ", "UID", DataViewRowState.CurrentRows);
                        if (dt_view.Count > 0)
                        {
                            for (int i = 0; i < dt_uniq.Rows.Count; i++)
                            {
                                if ((dt_uniq.Rows[i]["UID"].ToString().Trim() + dt_uniq.Rows[i]["MV_NAME"].ToString().Trim()) == (dt_view[0]["UID"].ToString().Trim() + dt_view[0]["MV_NAME"].ToString().Trim()))
                                {
                                    try
                                    {
                                        dt_uniq.Rows[i].Delete();
                                        break;
                                    }
                                    catch { break; }
                                }
                            }
                        }
                    }
                    catch { }
                }
                try
                {
                    if (P_VAR_VALUE != null)
                    {
                        fgen_oporow = null;
                        fgen_oporow = dt_uniq.NewRow();
                        fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
                        fgen_oporow["UID"] = UNIQ_ID.ToUpper();
                        fgen_oporow["MV_NAME"] = P_VAR_NAME;
                        fgen_oporow["MV_VALUE"] = EncryptDecrypt.Encrypt(P_VAR_VALUE).ToString();
                        fgen_oporow["MV_DATE"] = DateTime.Now.ToString("dd/MM/yyyy");
                        if (fgen_oporow != null)
                        {
                            if (fgen_oporow[1] != null)
                                dt_uniq.Rows.Add(fgen_oporow);
                        }
                        //if (P_VAR_NAME != "U_SEEKSQL") Fn_Set_in_TXT(UNIQ_ID, dt_uniq);
                    }
                }
                catch { }
            }
        }
        //public static void Fn_Set_in_TXT(string UNIQ_ID, DataTable file_to_write_txt)
        //{
        //    StreamWriter w = null;
        //    try
        //    {
        //        string vpath = "";
        //        try
        //        {
        //            vpath = HttpContext.Current.Server.MapPath("~/tej-base/Log_File/" + UNIQ_ID + ".txt");
        //        }
        //        catch
        //        {
        //            vpath = fgenMV.context.Server.MapPath("~/tej-base/Log_File/" + UNIQ_ID + ".txt");
        //        }
        //        del_file(vpath);
        //        if (file_to_write_txt.Rows.Count > 0)
        //        {
        //            if (file_to_write_txt.Rows[0]["MV_VALUE"].ToString().Length > 1)
        //                w = new StreamWriter(vpath, true);
        //        }
        //        foreach (DataRow dr_file_to_write_txt in file_to_write_txt.Rows)
        //        {
        //            if (dr_file_to_write_txt["UID"].ToString().Trim().ToUpper() == UNIQ_ID.ToUpper())
        //            {
        //                if (dr_file_to_write_txt["MV_VALUE"].ToString().Trim().Length > 2 && dr_file_to_write_txt["UID"].ToString().Trim().Length > 2)
        //                    w.WriteLine(UNIQ_ID + "~" + dr_file_to_write_txt["MV_NAME"].ToString().Trim() + "~" + dr_file_to_write_txt["MV_VALUE"].ToString().Trim());
        //            }
        //        }
        //        w.Flush();
        //        w.Close();
        //    }
        //    catch(Exception ee)
        //    {
        //        w.Flush();
        //        w.Close();
        //    }
        //}
        //public static DataTable Fn_Read_TXT(string UNIQ_ID, string srch_var_name)
        //{
        //    if (UNIQ_ID.Contains("STR")) UNIQ_ID = UNIQ_ID.Split('=')[1];
        //    string vpath = "";
        //    try
        //    {
        //        vpath = HttpContext.Current.Server.MapPath("~/tej-base/Log_File/" + UNIQ_ID + ".txt");
        //    }
        //    catch
        //    {
        //        vpath = fgenMV.context.Server.MapPath("~/tej-base/Log_File/" + UNIQ_ID + ".txt");
        //    }

        //    try
        //    {
        //        if (File.Exists(vpath)) sr_fgen = new StreamReader(vpath);
        //    }
        //    catch
        //    {
        //        sr_fgen.Close();
        //        sr_fgen = new StreamReader(vpath);
        //    }

        //    string[] str = sr_fgen.ReadToEnd().Trim().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        //    try
        //    {
        //        if (dt_uniq.Columns.Count <= 0)
        //        {
        //            vpath = "ALL";
        //            dt_uniq = new DataTable();
        //            dt_uniq.Columns.Add(new DataColumn("srno", typeof(string)));
        //            dt_uniq.Columns.Add(new DataColumn("UID", typeof(string)));
        //            dt_uniq.Columns.Add(new DataColumn("MV_NAME", typeof(string)));
        //            dt_uniq.Columns.Add(new DataColumn("MV_VALUE", typeof(string)));
        //            dt_uniq.Columns.Add(new DataColumn("MV_DATE", typeof(string)));
        //        }
        //    }
        //    catch
        //    {
        //        dt_uniq = new DataTable();
        //        dt_uniq.Columns.Add(new DataColumn("srno", typeof(string)));
        //        dt_uniq.Columns.Add(new DataColumn("UID", typeof(string)));
        //        dt_uniq.Columns.Add(new DataColumn("MV_NAME", typeof(string)));
        //        dt_uniq.Columns.Add(new DataColumn("MV_VALUE", typeof(string)));
        //        dt_uniq.Columns.Add(new DataColumn("MV_DATE", typeof(string)));
        //        vpath = "ALL";
        //    }
        //    foreach (string item in str)
        //    {
        //        string suc = "0";
        //        if (item.Split('~')[0].ToString().Trim().ToUpper() == UNIQ_ID.ToUpper())
        //        {
        //            if (vpath == "ALL")
        //            {
        //                fgen_oporow = null;
        //                fgen_oporow = dt_uniq.NewRow();
        //                fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
        //                fgen_oporow["UID"] = UNIQ_ID.ToUpper();
        //                fgen_oporow["MV_NAME"] = item.Split('~')[1].ToString().Trim();
        //                fgen_oporow["MV_VALUE"] = item.Split('~')[2].ToString().Trim();
        //                fgen_oporow["MV_DATE"] = DateTime.Now.ToString("dd/MM/yyyy");
        //                suc = "1";

        //            }
        //            else
        //            {
        //                fgen_oporow = null;
        //                fgen_oporow = dt_uniq.NewRow();
        //                if (dt_uniq.Rows.Count <= 0)
        //                {
        //                    fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
        //                    fgen_oporow["UID"] = UNIQ_ID.ToUpper();
        //                    fgen_oporow["MV_NAME"] = item.Split('~')[1].ToString().Trim();
        //                    fgen_oporow["MV_VALUE"] = item.Split('~')[2].ToString().Trim();
        //                    fgen_oporow["MV_DATE"] = DateTime.Now.ToString("dd/MM/yyyy");
        //                    suc = "1";
        //                }
        //                else
        //                {
        //                    if (item.Split('~')[1].ToString().Trim() == srch_var_name)
        //                    {
        //                        fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
        //                        fgen_oporow["UID"] = UNIQ_ID.ToUpper();
        //                        fgen_oporow["MV_NAME"] = item.Split('~')[1].ToString().Trim();
        //                        fgen_oporow["MV_VALUE"] = item.Split('~')[2].ToString().Trim();
        //                        fgen_oporow["MV_DATE"] = DateTime.Now.ToString("dd/MM/yyyy");
        //                        suc = "1";
        //                    }
        //                }
        //            }
        //            if (suc == "1") dt_uniq.Rows.Add(fgen_oporow);
        //        }
        //    }
        //    sr_fgen.Close();
        //    return dt_uniq;
        //}
        public static void Fn_Delete_Older_Data()
        {
            try
            {
                if (dt_uniq != null)
                {
                    if (dt_uniq.Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow dr_u in dt_uniq.Rows)
                        {
                            if (Convert.ToDateTime(dr_u["MV_DATE"].ToString().Trim()) <= System.DateTime.Now.AddDays(-2))
                            {
                                dt_uniq.Rows[i].Delete();
                            }
                            i++;
                        }
                    }
                }
            }
            catch { }
        }
        public static void FN_Delete_Older_Files()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/tej-base/Log_File/"));
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

                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/tej-base/Barcode/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/tej-base/xmlfile/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/tej-base/temp/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/temp/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/logs/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
                di = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/tempgen/"));
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-1)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
                di = new DirectoryInfo("c:\\tej_erp\\np\\");
                if (di.Exists)
                {
                    files = di.GetFiles();
                    filesToBeDeleted = files.Where(r => (Convert.ToDateTime(r.CreationTime) <= DateTime.Now.Date.AddDays(-2)));
                    foreach (FileInfo file in filesToBeDeleted)
                    {
                        file.Delete();
                    }
                }
            }
            catch { }
        }
        public static DataTable Fn_Mvar_Rows(string Q_ID)
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
        public static string Fn_Get_Mvar(string UNIQ_ID, string P_VAR_NAME)
        {
            return Singleton.Fn_Get_Mvar(UNIQ_ID, P_VAR_NAME);
            string P_VAR_VALUE = "0";
            fgenDB fgen = new fgenDB();
            if (UNIQ_ID != null)
            {
                if (fgen.checkActivation(UNIQ_ID) == true)
                {
                    //try
                    {
                        //if (dt_uniq == null || dt_uniq.Rows.Count <= 0) dt_uniq = Fn_Read_TXT(UNIQ_ID, P_VAR_NAME);
                        if (dt_uniq == null || dt_uniq.Rows.Count <= 0)
                        {
                            fgen.FILL_ERR("Values Lost : " + UNIQ_ID + " " + P_VAR_NAME);
                            dt_uniq = new DataTable();
                            dt_uniq.Columns.Add(new DataColumn("srno", typeof(string)));
                            dt_uniq.Columns.Add(new DataColumn("UID", typeof(string)));
                            dt_uniq.Columns.Add(new DataColumn("MV_NAME", typeof(string)));
                            dt_uniq.Columns.Add(new DataColumn("MV_VALUE", typeof(string)));
                            dt_uniq.Columns.Add(new DataColumn("MV_DATE", typeof(string)));

                            fgen_oporow = null;
                            string myVals = HttpContext.Current.Request.Cookies["MY_VALS" + UNIQ_ID].Value.ToString().Trim();
                            //fgen.send_cookie("MY_VALS" + get_qstr, "U_COCD:" + co_cd + "~" + "U_FYEAR:" + yr + "~" + "U_MBR:" + mbr + "~" + "U_UNAME:" + uname + "~" + "U_ULEVEL:" + frm_ulevel + "~" + "U_CDT1:" + frm_CDT1 + "~" + "U_CDT2:" + frm_CDT2);

                            fgen_oporow = dt_uniq.NewRow();
                            foreach (string myVal in myVals.Split('~'))
                            {
                                fgen_oporow["srno"] = (dt_uniq.Rows.Count).ToString();
                                fgen_oporow["UID"] = UNIQ_ID.ToUpper();
                                fgen_oporow["MV_NAME"] = myVal.Split(':')[0];
                                fgen_oporow["MV_VALUE"] = myVal.Split(':')[0];
                                fgen_oporow["MV_DATE"] = DateTime.Now.ToString("dd/MM/yyyy");
                                dt_uniq.Rows.Add(fgen_oporow);
                            }

                        }
                        DataView dt_view = new DataView();
                        try
                        {
                            dt_view = new DataView(dt_uniq, "UID='" + UNIQ_ID.Trim() + "' and MV_NAME='" + P_VAR_NAME.Trim() + "'", "UID", DataViewRowState.CurrentRows);
                        }
                        catch
                        {
                            System.Threading.Thread.Sleep(500);
                            dt_view = new DataView(dt_uniq, "UID='" + UNIQ_ID.Trim() + "' and MV_NAME='" + P_VAR_NAME.Trim() + "'", "UID", DataViewRowState.CurrentRows);
                        }
                        if (dt_view.Count > 0)
                        {
                            P_VAR_VALUE = EncryptDecrypt.Decrypt(dt_view[0]["MV_VALUE"].ToString().Trim()).ToString();
                        }
                        else
                        {
                            //dt_uniq = Fn_Read_TXT(UNIQ_ID, P_VAR_NAME);
                            dt_view = new DataView(dt_uniq, "UID='" + UNIQ_ID.Trim() + "' and MV_NAME='" + P_VAR_NAME.Trim() + "'", "UID", DataViewRowState.CurrentRows);
                            if (dt_view.Count > 0)
                            {
                                P_VAR_VALUE = EncryptDecrypt.Decrypt(dt_view[0]["MV_VALUE"].ToString().Trim()).ToString();
                            }
                        }
                        if (P_VAR_NAME == "U_SYS_COM_QRY" && P_VAR_VALUE == "0")
                        {
                            P_VAR_VALUE = "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND,NVL(OBJ_CAPTION_REG,'-') AS OBJ_CAPTION_REG FROM SYS_CONFIG ";
                            Fn_Set_Mvar(UNIQ_ID, P_VAR_NAME, P_VAR_VALUE);
                        }
                    }
                    //catch (Exception EX)
                    //{
                    //    fgen.FILL_ERR("FN_GET_MVAR :=> " + EX.Message.ToString().Trim());
                    //}
                }
                //else { HttpContext.Current.Response.Redirect("~/Login.aspx"); }
            }
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
