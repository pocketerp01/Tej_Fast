using System;
using System.Web;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
//using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;


    public class fgenDB : fgenLG
    {
        string urights, mq0, pk_error, opt_freez;
        string printRegHeadings;
        string MV_CLIENT_GRP = "", chartLegType, frm_grp, frm_qtr;
        string firm = "";

        DataTable dt_menu = new DataTable();
        /// <summary>
        /// Creating increment number with the limit
        /// </summary>
        /// <param name="co_Cd">The valued entered by User</param>
        /// <param name="squery">Query for max number</param>
        /// <param name="limit">Limit for length of the number</param>
        /// <param name="col1">Field Name</param>
        /// <returns>Number with length for ex. 000001</returns>
        public string next_no(string Qstr, string co_Cd, string squery, int limit, string col1)
        {
            Int64 i = 0;
            string count = "", result = "";
            using (DataTable dt_fgen = getdata(Qstr, co_Cd, squery))
            {
                if (dt_fgen.Rows.Count > 0) count = dt_fgen.Rows[0]["" + col1 + ""].ToString().Trim();
                else count = "0";
                if (count.Trim() == "" || count.Trim() == "0" || count.Trim() == "-")
                {
                    i = 1;
                }
                else
                {
                    try
                    {
                        i = Convert.ToInt64(count);
                        i++;
                    }
                    catch
                    {
                        i = 100001;
                    }

                }
                result = padlc(i, limit);
            }
            return result;
        }



        /// <summary>
        /// Check the single field from database
        /// </summary>
        /// <param name="co_Cd">The valued entered by User</param>
        /// <param name="Squery">Query to search a field</param>
        /// <param name="Seek_Val1">Field Name</param>
        /// <returns>IF field is having some value then value else returns 0</returns>
        public string seek_iname(string Qstr, string co_Cd, string Squery, string Seek_Val1)
        {
            string ReturnVal = "";
            using (DataTable dt_fgen = getdata(Qstr, co_Cd, Squery))
            {
                if (dt_fgen.Rows.Count > 0)
                {
                    ReturnVal = "0";
                    //FILL_ERR(Squery);
                    if (Seek_Val1.Length > 0) ReturnVal = dt_fgen.Rows[0][Seek_Val1].ToString().Trim();
                    else ReturnVal = dt_fgen.Rows[0][0].ToString().Trim();
                    if (ReturnVal == "" || ReturnVal == "-") ReturnVal = "0";
                }
                else ReturnVal = "0";
            }
            return ReturnVal.Trim();
        }
        public string SeekWipStock(string cocd, string frmQstr, string branchcd, string icode, string stage, string trackno, string startdt, string todt, string cond)
        {
            todt = fgenMV.Fn_Get_Mvar(frmQstr, "U_CDT2");
            startdt = fgenMV.Fn_Get_Mvar(frmQstr, "U_CDT1");
            //string fromdt = fgenMV.Fn_Get_Mvar(frmQstr, "U_MDT1");
            //if (fromdt == "0") fromdt = DateTime.Now.ToString("dd/MM/yyyy");
            string year = fgenMV.Fn_Get_Mvar(frmQstr, "U_YEAR");

            //var squery = "select * from (" + WIPSTKQry(cocd, frmQstr, branchcd, startdt, todt,true) + ") where STAGE='" + stage + "' " +
            //     "AND TRIM(ICODE)='" + icode + "' " + cond + "";

            var squery = WIPSTKQry(cocd, frmQstr, branchcd, startdt, todt, cond + " AND TRIM(ICODE)='" + icode + "'" + " and trim(STAGE)='" + stage + "'");
            mq0 = seek_iname(frmQstr, cocd, squery, "BAL");
            return mq0;
        }
        public string WIPSTKQry(string cocd, string qstr, string branchcd, string startdt, string todt, string seekstockcond = "")
        {
            mq0 = "SELECT trim(ICODE) AS ICODE,TRIM(STAGE) AS STAGE, trim(REVIS_NO) AS REVIS_NO,trim(branchcd) AS branchcd,SUM(IQTYIN) AS QTYIN,SUM(IQTYOUT) AS QTYOUT,SUM(IQTYIN)-SUM(IQTYOUT) AS BAL FROM (" +
                "SELECT revis_no,branchcd, VCHNUM, VCHDATE, TYPE, TRIM(ICODE) ICODE,IQTYOUT AS IQTYIN,0 AS IQTYOUT, STAGE AS STAGE FROM IVOUCHER WHERE BRANCHCD = '" + branchcd + "' " +
                "AND TYPE LIKE '3%' AND TYPE!= '39' AND VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') AND STORE = 'Y' " +
                "UNION ALL SELECT revis_no,branchcd, VCHNUM, VCHDATE, TYPE, TRIM(ICODE) ICODE,IQTYIN AS IQTYIN, 0 AS IQTYOUT, STAGE AS STAGE FROM IVOUCHER WHERE BRANCHCD = '" + branchcd + "' " +
                "AND TYPE LIKE '1%' AND TYPE='15' AND STORE = 'W' UNION ALL SELECT revis_no,branchcd, VCHNUM, VCHDATE, TYPE, TRIM(ICODE) ICODE,0 as IQTYIN, IQTYIN AS IQTYOUT, STAGE AS STAGE FROM IVOUCHER WHERE BRANCHCD = '" + branchcd + "' " +
                "AND TYPE LIKE '1%' AND TYPE='15' AND STORE = 'Y' UNION ALL SELECT revis_no,branchcd, VCHNUM, VCHDATE, TYPE, TRIM(ICODE) AS ICODE,0 AS IQTYIN, " +
                "IQTYOUT AS IQTYOUT, STAGE AS STAGE FROM IVOUCHER WHERE BRANCHCD = '" + branchcd + "' AND TYPE = '3A' AND VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT revis_no,branchcd, VCHNUM, VCHDATE, TYPE, TRIM(ICODE) AS ICODE, IQTYOUT AS IQTYIN,0 AS IQTYOUT, " +
                "IOPR AS STAGE FROM IVOUCHER WHERE BRANCHCD = '" + branchcd + "' AND TYPE = '3A' AND VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT revis_no,branchcd, VCHNUM, VCHDATE, TYPE, TRIM(ICODE),0 AS IQTYIN, IQTYOUT, STAGE AS STAGE FROM " +
                " IVOUCHER WHERE BRANCHCD = '" + branchcd + "' AND TYPE LIKE '39%' AND VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') " +
                "AND STORE = 'W' UNION ALL select revis_no,branchcd, VCHNUM, VCHDATE, TYPE, trim(icode) as icode,0 AS iqtyin, iqtyin as iqtyout,STAGE AS STAGE from " +
                "ivoucher where branchcd = '" + branchcd + "' AND TYPE = '09' AND store = 'W' AND INSPECTED = 'Y' and VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') UNION ALL select revis_no,branchcd, VCHNUM, VCHDATE, TYPE, " +
                "trim(icode) as icode,IQTYIN AS iqtyin,0 AS iqtyout, IOPR AS STAGE from ivoucher where branchcd = '" + branchcd + "' AND TYPE = '09'  and store = 'W' AND " +
                "INSPECTED = 'Y' and VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') UNION ALL select revis_no,branchcd, VCHNUM, VCHDATE, TYPE, trim(icode) as icode,0 AS iqtyin, iqtyout, STAGE AS STAGE from ivoucher " +
                "where branchcd = '" + branchcd + "' AND TYPE = '21' AND store = 'Y' and VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') UNION ALL select revis_no,branchcd, VCHNUM, VCHDATE, TYPE, trim(icode) as icode,iqtyout AS iqtyin," +
                "0 AS iqtyout, IOPR AS STAGE from ivoucher where branchcd = '" + branchcd + "' AND TYPE = '21'  and store = 'Y' and VCHDATE between to_date('" + startdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')) t " + seekstockcond + " GROUP BY trim(ICODE), trim(REVIS_NO),TRIM(STAGE),trim(branchcd) having SUM(IQTYIN)-SUM(IQTYOUT)<>0 AND LENGTH(trim(REVIS_NO))>1";
            return mq0;
        }

        public DataTable getdata(string Qstr, string pco_Cd, string query1)
        {
            DataTable dt_get = new DataTable();
            //cow
            try
            {
                if (query1 == "" || query1 == null) return null;
                if (query1.Contains("0 WHERE UPPER(TRIM(FRM_NAME))='"))
                {
                    FILL_ERR(pco_Cd + " : Datatable Filling :=> " + query1 + " :: " + Qstr);
                    return null;
                }
                if (pco_Cd == "0" || pco_Cd == "" || pco_Cd == " " || pco_Cd == null) pco_Cd = Qstr.Split('^')[0];
                string constr = fgenMV.Fn_Get_Mvar(Qstr, "CONN");
                if (constr == "0") { constr = ConnInfo.connString(pco_Cd); }

                if (!constr.ToUpper().Contains("USER ID")) { constr = ConnInfo.connString(pco_Cd); }
                if (!constr.ToUpper().Contains("PASSWORD")) { constr = ConnInfo.connString(pco_Cd); }

                using (OracleConnection fcon = new OracleConnection(constr))
                {
                    fcon.Open();
                    //FILL_ERR("Datatable Filling :=> " + query1);
                    using (OracleCommand cmd = new OracleCommand(query1, fcon))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                if (dr.HasRows) dt_get.Load(dr);
                                //dr.Close();
                                //dr.Dispose(); 
                                //cmd.Dispose();
                                //FILL_ERR(query1);
                                //int c = make_int(fgenMV.Fn_Get_Mvar(Qstr, "U_Q_COUNTER"));
                                //c++;
                                //fgenMV.Fn_Set_Mvar(Qstr, "U_Q_COUNTER", c.ToString());
                            }
                            //if (!dr.IsClosed)
                            //{
                            //    dr.Close();
                            //    dr.Dispose();
                            //}
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                FILL_ERR(pco_Cd + " : Datatable Filling :=> " + EX.Message.ToString().Trim() + " : " + query1);
            }
            //cow
            return dt_get;
        }
        public OracleDataReader fillOracleDataReader(string Qstr, string pco_Cd, string query1)
        {
            OracleDataReader myReader = null;
            try
            {
                if (query1 == "" || query1 == null) return null;
                if (pco_Cd == "0") pco_Cd = Qstr.Split('^')[0];
                string constr = fgenMV.Fn_Get_Mvar(Qstr, "CONN");

                if (constr == "0") { constr = ConnInfo.connString(pco_Cd); }

                if (!constr.ToUpper().Contains("USER ID")) { constr = ConnInfo.connString(pco_Cd); }
                if (!constr.ToUpper().Contains("PASSWORD")) { constr = ConnInfo.connString(pco_Cd); }


                using (OracleConnection fcon = new OracleConnection(constr))
                {
                    fcon.Open();
                    using (OracleCommand cmd = new OracleCommand(query1, fcon))
                    {
                        myReader = cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception EX)
            {
                FILL_ERR(pco_Cd + " : Datatable Filling :=> " + EX.Message.ToString().Trim() + " : " + query1);
            }
            return myReader;
        }
        public DataSet getDS(string Qstr, string pco_Cd, string query1)
        {
            if (query1 == "" || query1 == null) return null;
            if (pco_Cd == "0") pco_Cd = Qstr.Split('^')[0];
            DataSet ds_fgen = new DataSet();
            string constr = fgenMV.Fn_Get_Mvar(Qstr, "CONN");
            if (constr == "0") { constr = ConnInfo.connString(pco_Cd); }

            if (!constr.ToUpper().Contains("USER ID")) { constr = ConnInfo.connString(pco_Cd); }
            if (!constr.ToUpper().Contains("PASSWORD")) { constr = ConnInfo.connString(pco_Cd); }


            using (OracleConnection fcon = new OracleConnection(constr))
            {
                fcon.Open();
                using (OracleCommand cmd = new OracleCommand(query1, fcon))
                {
                    using (OracleDataReader dr_fgen = cmd.ExecuteReader())
                    {
                        DataTable dt_fgen = new DataTable();
                        if (dr_fgen.HasRows) dt_fgen.Load(dr_fgen);
                        ds_fgen.Tables.Add(dt_fgen);
                        dr_fgen.Dispose();
                        cmd.Dispose();
                    }
                }
            }
            return ds_fgen;
        }
        public void execute_cmd(string Qstr, string pco_Cd, string query1)
        {
            query1 = query1.Replace("&quot;", "").Replace("&nbsp;", "").Replace("&#39;", "'");
            try
            {
                if (pco_Cd == "0") pco_Cd = Qstr.Split('^')[0];
                string constr = fgenMV.Fn_Get_Mvar(Qstr, "CONN");
                if (constr == "0") { constr = ConnInfo.connString(pco_Cd); }

                if (!constr.ToUpper().Contains("USER ID")) { constr = ConnInfo.connString(pco_Cd); }
                if (!constr.ToUpper().Contains("PASSWORD")) { constr = ConnInfo.connString(pco_Cd); }


                using (OracleConnection fcon = new OracleConnection(constr))
                {
                    fcon.Open();
                    using (OracleCommand cmd = new OracleCommand(query1, fcon))
                    {
                        //FILL_ERR(query1);
                        //int c = make_int(fgenMV.Fn_Get_Mvar(Qstr, "U_Q_COUNTER"));
                        //c++;
                        //fgenMV.Fn_Set_Mvar(Qstr, "U_Q_COUNTER", c.ToString());
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            //cow
            //22/08/2020
            catch (Exception ex)
            {
                FILL_ERR("In Execute Cmd :=> " + ex.Message + " : " + query1);
            }
        }
        public string getOption(string Qstr, string cocd, string optName, string variable)
        {
            string ReturnVal = "";
            ReturnVal = seek_iname(Qstr, cocd, "SELECT " + variable + " FROM FIN_RSYS_OPT WHERE OPT_ID='" + optName + "' ", variable);
            return ReturnVal.Trim();
        }
        public string getOptionPW(string Qstr, string cocd, string optName, string variable, string branchcd)
        {
            string ReturnVal = "";
            ReturnVal = seek_iname(Qstr, cocd, "SELECT " + variable + " FROM FIN_RSYS_OPT_PW WHERE OPT_ID='" + optName + "' and BRANCHCD='" + branchcd + "' ", variable);
            return ReturnVal.Trim();
        }
        public string Fn_chk_can_edit(string Qstr, string co_cd, string userid, string formid)
        {
            urights = seek_iname(Qstr, co_cd, "SELECT RCAN_EDIT FROM FIN_MRSYS WHERE USERID='" + userid + "' and ID='" + formid + "'", "RCAN_EDIT");
            if (urights == "N") urights = "N";
            else urights = "Y";
            return urights;
        }

        public string Fn_chk_can_add(string Qstr, string co_cd, string userid, string formid)
        {
            urights = seek_iname(Qstr, co_cd, "SELECT RCAN_add FROM FIN_MRSYS WHERE USERID='" + userid + "' and ID='" + formid + "'", "RCAN_add");
            if (urights == "N") urights = "N";
            else urights = "Y";
            return urights;
        }

        public string Fn_chk_can_prn(string Qstr, string co_cd, string userid, string formid)
        {
            urights = seek_iname(Qstr, co_cd, "SELECT RCAN_PRN FROM FIN_MRSYS WHERE USERID='" + userid + "' and ID='" + formid + "'", "RCAN_PRN");
            if (urights == "N") urights = "N";
            else urights = "Y";
            return urights;
        }

        //---------------------------------------------------------------------------------------
        public string Fn_next_doc_no(string Qstr, string co_cd, string my_tbl, string my_no_fld, string my_dt_fld, string my_mbr, string my_vty, string my_vdt, string my_uname, string my_frm)
        {
            double i = 0;
            string next_vnum = "";
            string last_vnum = "";
            string frm_vnum = "";
            string task_ok = "Y";
            string CDT1 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT2 + "','dd/mm/yyyy')";
            do
            {
                int vi_chk = 0;
                do
                {
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + "", 6, "vch");
                    next_vnum = frm_vnum;
                    last_vnum = seek_iname(Qstr, co_cd, "select max(" + my_no_fld + ") as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + "", "vch");
                    vi_chk++;
                    if (vi_chk > 10)
                    {
                        fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                        task_ok = "N";
                    }
                }
                while (((make_double(next_vnum) - 1) != make_double(last_vnum) && make_double(next_vnum) > 1));
                //if (my_vty.Length == 4)
                //{
                //    pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty + frm_vnum + CDT1, my_mbr, my_vty.Substring(2, 2), frm_vnum, my_vdt, "", my_uname);
                //}
                //else
                {
                    pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty + frm_vnum + CDT1, my_mbr, my_vty, frm_vnum, my_vdt, "", my_uname);
                }
                if (i > 10)
                {
                    FILL_ERR(my_uname + " --> Next_no Fun Prob ==> " + my_frm + " ==> In Save Function");
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + "", 6, "vch");
                    pk_error = "N";
                    i = 0;
                }
                i++;
            }
            while (pk_error == "Y");

            string col3;
            col3 = seek_iname(Qstr, co_cd, "SELECT BRANCHCD||" + my_no_fld + " AS CNT FROM " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " AND " + my_no_fld + "='" + frm_vnum + "'", "CNT");
            if (col3 != "0")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                task_ok = "N";
            }

            i = 0;
            if (task_ok == "Y")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "Y");
            }
            return frm_vnum;
        }
        public string Fn_next_doc_no(string Qstr, string co_cd, string my_tbl, string my_no_fld, string my_dt_fld, string my_mbr, string my_vty, string my_vdt, string my_uname, string my_frm, string extraCondition)
        {
            double i = 0;
            string next_vnum = "";
            string last_vnum = "";
            string frm_vnum = "";
            string task_ok = "Y";
            string CDT1 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT2 + "','dd/mm/yyyy')";
            do
            {
                int vi_chk = 0;
                do
                {
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", 6, "vch");
                    next_vnum = frm_vnum;
                    last_vnum = seek_iname(Qstr, co_cd, "select max(" + my_no_fld + ") as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", "vch");
                    vi_chk++;
                    if (vi_chk > 10)
                    {
                        fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                        task_ok = "N";
                    }
                }
                while (((make_double(next_vnum) - 1) != make_double(last_vnum) && make_double(next_vnum) > 1));
                //if (my_vty.Length == 4)
                //{
                //    pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty + frm_vnum + CDT1, my_mbr, my_vty.Substring(2, 2), frm_vnum, my_vdt, "", my_uname);
                //}
                //else
                {
                    pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty + frm_vnum + CDT1, my_mbr, my_vty, frm_vnum, my_vdt, "", my_uname);
                }
                if (i > 10)
                {
                    FILL_ERR(my_uname + " --> Next_no Fun Prob ==> " + my_frm + " ==> In Save Function");
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + "", 6, "vch");
                    pk_error = "N";
                    i = 0;
                }
                i++;
            }
            while (pk_error == "Y");

            string col3;
            col3 = seek_iname(Qstr, co_cd, "SELECT BRANCHCD||" + my_no_fld + " AS CNT FROM " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " AND " + my_no_fld + "='" + frm_vnum + "'", "CNT");
            if (col3 != "0")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                task_ok = "N";
            }

            i = 0;
            if (task_ok == "Y")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "Y");
            }
            return frm_vnum;
        }
        public DataTable add_apprvlLogo(string fCocd, DataTable dataTable, string branchCode)
        {
            DataTable dtN = new DataTable();
            try
            {
                FileStream FilStr;
                BinaryReader BinRed;
                string fpath = HttpContext.Current.Server.MapPath("~/erp_docs/apr_logo.png");
                //if (branchCode != "")
                //{
                //    fpath = HttpContext.Current.Server.MapPath("~/erp_docs/logo/mlogo_" + fCocd + "_" + branchCode + ".jpg");
                if (!File.Exists(fpath)) fpath = HttpContext.Current.Server.MapPath("~/erp_docs/apr_logo.png");
                //}
                if (dataTable.Rows.Count > 0)
                {
                    if (!dataTable.Columns.Contains("apprLogo")) dataTable.Columns.Add("apprLogo", typeof(System.Byte[]));
                }
                dtN = dataTable.Clone();
                foreach (DataRow dr in dataTable.Rows)
                {
                    FilStr = new FileStream(fpath, FileMode.Open);
                    BinRed = new BinaryReader(FilStr);
                    dr["apprLogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                    FilStr.Close();
                    BinRed.Close();
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
                dtN.TableName = dataTable.TableName.ToString();
            }
            catch
            {
                FILL_ERR("Logo File not found in erp_docs folder " + HttpContext.Current.Server.MapPath("~/erp_docs/apr_logo.png"));
            }
            return dtN;
        }
        public string Fn_next_doc_no(string Qstr, string co_cd, string my_tbl, string my_no_fld, string my_dt_fld, string my_mbr, string my_vty, string my_vdt, string my_uname, string my_frm, string extraCondition, string numberPattern)
        {
            double i = 0;
            string next_vnum = "";
            string last_vnum = "";
            string frm_vnum = "";
            string task_ok = "Y";
            string CDT1 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT2 + "','dd/mm/yyyy')";
            do
            {
                int vi_chk = 0;
                do
                {
                    if (numberPattern == "PROD")
                    {
                        frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " and acode='" + extraCondition + "'", 6, "vch");
                        if (frm_vnum == "000001")
                            frm_vnum = extraCondition.Right(1) + frm_vnum.Right(5);
                        next_vnum = frm_vnum;
                        last_vnum = seek_iname(Qstr, co_cd, "select max(" + my_no_fld + ") as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " and acode='" + extraCondition + "'", "vch");
                        vi_chk++;
                        if (vi_chk > 10)
                        {
                            fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                            task_ok = "N";
                        }
                    }
                    else
                    {
                        frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", 6, "vch");
                        next_vnum = frm_vnum;
                        last_vnum = seek_iname(Qstr, co_cd, "select max(" + my_no_fld + ") as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", "vch");
                        vi_chk++;
                        if (vi_chk > 10)
                        {
                            fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                            task_ok = "N";
                        }
                    }
                }
                while (((make_double(next_vnum) - 1) != make_double(last_vnum) && make_double(next_vnum.Right(5)) > 1));
                //if (my_vty.Length == 4)
                //{
                //    pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty + frm_vnum + CDT1, my_mbr, my_vty.Substring(2, 2), frm_vnum, my_vdt, "", my_uname);
                //}
                //else
                {
                    pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty + frm_vnum + CDT1, my_mbr, my_vty, frm_vnum, my_vdt, "", my_uname);
                }
                if (i > 10)
                {
                    FILL_ERR(my_uname + " --> Next_no Fun Prob ==> " + my_frm + " ==> In Save Function");
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + "", 6, "vch");
                    pk_error = "N";
                    i = 0;
                }
                i++;
            }
            while (pk_error == "Y");

            string col3;
            col3 = seek_iname(Qstr, co_cd, "SELECT BRANCHCD||" + my_no_fld + " AS CNT FROM " + my_tbl + " where branchcd='" + my_mbr + "' and type='" + my_vty + "' and " + my_dt_fld + " " + xdt_Range + " AND " + my_no_fld + "='" + frm_vnum + "'", "CNT");
            if (col3 != "0")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                task_ok = "N";
            }

            i = 0;
            if (task_ok == "Y")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "Y");
            }
            return frm_vnum;
        }
        public string Fn_next_doc_no_inv(string Qstr, string co_cd, string my_tbl, string my_no_fld, string my_dt_fld, string my_mbr, string my_vty, string my_vdt, string my_uname, string my_frm)
        {
            double i = 0;
            string next_vnum = "";
            string last_vnum = "";
            string frm_vnum = "";
            string task_ok = "Y";
            string CDT1 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT2 + "','dd/mm/yyyy')";
            do
            {
                int vi_chk = 0;
                do
                {
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + "", 6, "vch");
                    next_vnum = frm_vnum;
                    last_vnum = seek_iname(Qstr, co_cd, "select max(" + my_no_fld + ") as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + "", "vch");
                    vi_chk++;
                    if (vi_chk > 10)
                    {
                        fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                        task_ok = "N";
                    }
                }
                while (((make_double(next_vnum) - 1) != make_double(last_vnum) && make_double(next_vnum) > 1));

                pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty.Substring(0, 1) + frm_vnum + CDT1, my_mbr, my_vty, frm_vnum, my_vdt, "", my_uname);
                if (i > 10)
                {
                    FILL_ERR(my_uname + " --> Next_no Fun Prob ==> " + my_frm + " ==> In Save Function");
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + "", 6, "vch");
                    pk_error = "N";
                    i = 0;
                }
                i++;
            }
            while (pk_error == "Y");

            string col3;
            col3 = seek_iname(Qstr, co_cd, "SELECT BRANCHCD||" + my_no_fld + " AS CNT FROM " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " AND " + my_no_fld + "='" + frm_vnum + "'", "CNT");
            if (col3 != "0")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                task_ok = "N";
            }

            i = 0;
            if (task_ok == "Y")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "Y");
            }
            return frm_vnum;
        }
        public string Fn_next_doc_no_inv(string Qstr, string co_cd, string my_tbl, string my_no_fld, string my_dt_fld, string my_mbr, string my_vty, string my_vdt, string my_uname, string my_frm, string extraCondition)
        {
            double i = 0;
            string next_vnum = "";
            string last_vnum = "";
            string frm_vnum = "";
            string task_ok = "Y";
            string CDT1 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT2 + "','dd/mm/yyyy')";
            do
            {
                int vi_chk = 0;
                do
                {
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", 6, "vch");
                    next_vnum = frm_vnum;
                    last_vnum = seek_iname(Qstr, co_cd, "select max(" + my_no_fld + ") as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", "vch");
                    vi_chk++;
                    if (vi_chk > 10)
                    {
                        fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                        task_ok = "N";
                    }
                }
                while (((make_double(next_vnum) - 1) != make_double(last_vnum) && make_double(next_vnum) > 1));

                pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr + my_vty.Substring(0, 1) + frm_vnum + CDT1, my_mbr, my_vty, frm_vnum, my_vdt, "", my_uname);
                if (i > 10)
                {
                    FILL_ERR(my_uname + " --> Next_no Fun Prob ==> " + my_frm + " ==> In Save Function");
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", 6, "vch");
                    pk_error = "N";
                    i = 0;
                }
                i++;
            }
            while (pk_error == "Y");

            string col3;
            col3 = seek_iname(Qstr, co_cd, "SELECT BRANCHCD||" + my_no_fld + " AS CNT FROM " + my_tbl + " where branchcd='" + my_mbr + "' and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " AND " + my_no_fld + "='" + frm_vnum + "' " + extraCondition + "", "CNT");
            if (col3 != "0")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                task_ok = "N";
            }

            i = 0;
            if (task_ok == "Y")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "Y");
            }
            return frm_vnum;
        }
        public string Fn_next_doc_no_invBR(string Qstr, string co_cd, string my_tbl, string my_no_fld, string my_dt_fld, string my_mbr, string my_vty, string my_vdt, string my_uname, string my_frm, string extraCondition)
        {
            double i = 0;
            string next_vnum = "";
            string last_vnum = "";
            string frm_vnum = "";
            string task_ok = "Y";
            string CDT1 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(Qstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT2 + "','dd/mm/yyyy')";
            do
            {
                int vi_chk = 0;
                do
                {
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd in (" + my_mbr + ") and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", 6, "vch");
                    next_vnum = frm_vnum;
                    last_vnum = seek_iname(Qstr, co_cd, "select max(" + my_no_fld + ") as vch from " + my_tbl + " where branchcd in (" + my_mbr + ") and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", "vch");
                    vi_chk++;
                    if (vi_chk > 10)
                    {
                        fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                        task_ok = "N";
                    }
                }
                while (((make_double(next_vnum) - 1) != make_double(last_vnum) && make_double(next_vnum) > 1));

                pk_error = chk_pk(Qstr, co_cd, my_tbl.ToUpper() + my_mbr.Replace("'", "").Left(2) + my_vty.Substring(0, 1) + frm_vnum + CDT1, my_mbr.Replace("'", "").Left(2), my_vty, frm_vnum, my_vdt, "", my_uname);
                if (i > 10)
                {
                    FILL_ERR(my_uname + " --> Next_no Fun Prob ==> " + my_frm + " ==> In Save Function");
                    frm_vnum = next_no(Qstr, co_cd, "select max(" + my_no_fld + ")+" + 0 + " as vch from " + my_tbl + " where branchcd in (" + my_mbr + ") and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " " + extraCondition + "", 6, "vch");
                    pk_error = "N";
                    i = 0;
                }
                i++;
            }
            while (pk_error == "Y");

            string col3;
            col3 = seek_iname(Qstr, co_cd, "SELECT BRANCHCD||" + my_no_fld + " AS CNT FROM " + my_tbl + " where branchcd in (" + my_mbr + ") and substr(type,1,1)='" + my_vty.Substring(0, 1) + "' and " + my_dt_fld + " " + xdt_Range + " AND " + my_no_fld + "='" + frm_vnum + "' " + extraCondition + "", "CNT");
            if (col3 != "0")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "N");
                task_ok = "N";
            }

            i = 0;
            if (task_ok == "Y")
            {
                fgenMV.Fn_Set_Mvar(Qstr, "U_NUM_OK", "Y");
            }
            return frm_vnum;
        }
        //-----------------------------------------------------------------------------
        public string Fn_chk_doc_freeze(string Qstr, string co_cd, string ctrl_br, string ctrl_id, string doc_Dt)
        {
            opt_freez = seek_iname(Qstr, co_cd, "SELECT trim(opt_param)||'@'||trim(opt_param2) as fstr FROM FIN_RSYS_opt_PW WHERE branchcd='" + ctrl_br + "' and opt_id='" + ctrl_id + "'", "fstr");
            string roll_Days;
            string fixd_Date;
            urights = "0";
            if (opt_freez != "0")
            {
                roll_Days = opt_freez.Split('@')[0].ToString();
                fixd_Date = opt_freez.Split('@')[1].ToString();
                string mqry;
                mqry = "SELECT (case when to_datE('" + doc_Dt + "','dd/mm/yyyy')<to_datE(sysdate,'dd/mm/yyyy')-" + make_double(roll_Days) + " then 'Y' else 'N' end)  as fstr FROM FIN_RSYS_opt_PW WHERE branchcd='" + ctrl_br + "' and opt_id='" + ctrl_id + "'";
                opt_freez = seek_iname(Qstr, co_cd, mqry, "fstr");
                if (opt_freez == "Y") urights = "1";

                if (fixd_Date.Length > 5)
                {
                    mqry = "SELECT (case when to_datE('" + doc_Dt + "','dd/mm/yyyy')<to_datE('" + fixd_Date + "','yyyy-mm-dd') then 'Y' else 'N' end) as fstr FROM FIN_RSYS_opt_PW WHERE branchcd='" + ctrl_br + "' and opt_id='" + ctrl_id + "'";
                    opt_freez = seek_iname(Qstr, co_cd, mqry, "fstr");
                    if (opt_freez == "Y") urights = "2";
                }
            }
            return urights;

        }

        public string Fn_chk_can_del(string Qstr, string co_cd, string userid, string formid)
        {
            urights = seek_iname(Qstr, co_cd, "SELECT RCAN_del FROM FIN_MRSYS WHERE USERID='" + userid + "' and ID='" + formid + "'", "RCAN_del");
            if (urights == "N") urights = "N";
            else urights = "Y";
            return urights;
        }
        public void chk_create_tab(string Qstr, string CoCD_Fgen)
        {
            string oraSQuery = "";
            string mhd;

            //-------------------------
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'FIN_RSYS_UPD'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "create table FIN_RSYS_UPD(IDNO varchar2(6) Default '-',ent_by varchar2(10) default '-',ent_Dt date default sysdate)";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            //-------------------------

            fgenMV.iconTableFull = new DataTable();
            fgenMV.iconTableFull = getdata(Qstr, CoCD_Fgen, "SELECT DISTINCT NVL(ID,'-') AS ID FROM FIN_MSYS ORDER BY NVL(ID,'-')");

            fgenMV.fin_rsys_upd = new DataTable();
            fgenMV.fin_rsys_upd = getdata(Qstr, CoCD_Fgen, "SELECT NVL(IDNO,'-') AS ID FROM FIN_RSYS_UPD ORDER BY NVL(IDNO,'-')");

            mhd = chk_RsysUpd("CT0001");
            if (mhd == "0" || mhd == "")
            {
                add_RsysUpd(Qstr, CoCD_Fgen, "CT0001", "DEV_A");
                //execute_cmd(Qstr, CoCD_Fgen, "insert into FIN_RSYS_UPD values ('CT0001','DEV_A',sysdate)");

                mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='FIN_MSYS'", "tname");
                if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "CREATE TABLE FIN_MSYS(ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(180) default '-',ALLOW_LEVEL NUMBER(2),WEB_aCTION VARCHAR2(50) default '-',SEARCH_KEY VARCHAR2(50) default '-',submenu char(1)default 'N',submenuid char(15) default '-',form varchar2(10) default '-',param varchar2(40) default '-',imagef varchar2(50) default '-',CSS varchar2(30) default 'fa-edit',PRD varchar2(1) default '-',BRN varchar2(1) default '-',BNR varchar2(1) default '-')");

                mq0 = check_filed_name(Qstr, CoCD_Fgen, "FIN_MSYS", "VISI");
                if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MSYS ADD VISI CHAR(1)");

                mq0 = seek_iname(Qstr, CoCD_Fgen, "select distinct constraint_name from user_constraints where table_name='FIN_MSYS'", "constraint_name");
                if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MSYS ADD CONSTRAINT FINRSYS_PK PRIMARY KEY (ID)");

                mq0 = seek_iname(Qstr, CoCD_Fgen, "select distinct constraint_name from user_constraints where table_name='FIN_RSYS_UPD'", "constraint_name");
                if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_RSYS_UPD ADD CONSTRAINT FINRSYSUPD_PK PRIMARY KEY (IDNO)");

                //execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MSYS ADD CONSTRAINT FINRSYS_PK PRIMARY KEY (ID)");
                //execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_RSYS_UPD ADD CONSTRAINT FINRSYSUPD_PK PRIMARY KEY (IDNO)");

                mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='FIN_MRSYS'", "tname");
                if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "create table FIN_MRSYS(USERID VARCHAR2(10),USERNAME VARCHAR2(30),BRANCHCD CHAR(2),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE,ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(50),ALLOW_LEVEL NUMBER(2),WEB_ACTION  VARCHAR2(50),SEARCH_KEY  vARCHAR2(50),SUBMENU  CHAR(1),SUBMENUID CHAR(15),FORM VARCHAR2(10),PARAM  VARCHAR2(10),USER_COLOR VARCHAR(10) DEFAULT '00578b',IDESC VARCHAR(50) DEFAULT '-',CSS varchar2(30) default 'fa-edit',RCAN_ADD CHAR(1) DEFAULT 'Y',RCAN_EDIT CHAR(1) DEFAULT 'Y',RCAN_DEL CHAR(1) DEFAULT 'Y',VISI CHAR(1))");

                mq0 = check_filed_name(Qstr, CoCD_Fgen, "FIN_MRSYS", "VISI");
                if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MRSYS ADD VISI CHAR(1)");

                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "create TABLE WSR_CTRL (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL_PK PRIMARY KEY (FINPKFLD) )";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL1'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "create TABLE WSR_CTRL1 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL1_PK PRIMARY KEY (FINPKFLD) )";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL2'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "create TABLE WSR_CTRL2 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL2_PK PRIMARY KEY (FINPKFLD) )";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL3'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "create TABLE WSR_CTRL3 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL3_PK PRIMARY KEY (FINPKFLD) )";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL4'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "create TABLE WSR_CTRL4 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL4_PK PRIMARY KEY (FINPKFLD) )";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'FIN_RSYS_OPT'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE FIN_RSYS_OPT(BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE DEFAULT SYSDATE,OPT_ID VARCHAR2(6) DEFAULT '-',OPT_TEXT VARCHAR2(200) DEFAULT '-',OPT_ENABLE VARCHAR2(1) DEFAULT '-',OPT_PARAM VARCHAR2(20) DEFAULT '-',OPT_PARAM2 VARCHAR2(20) DEFAULT '-',OPT_EXCL VARCHAR2(20) DEFAULT '-',ENT_BY VARCHAR2(10) DEFAULT '-',ENT_DT DATE DEFAULT SYSDATE,EDT_BY VARCHAR2(10) DEFAULT '-',EDT_DT DATE DEFAULT SYSDATE)";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='SYS_CONFIG'", "tname");
                if (mq0 == "0" || mq0 == "")
                {
                    execute_cmd(Qstr, CoCD_Fgen, "CREATE TABLE SYS_CONFIG ( BRANCHCD  CHAR(2),  TYPE  CHAR(2),  VCHNUM    CHAR(6),  VCHDATE   DATE,  SRNO  NUMBER(4),FRM_NAME  VARCHAR2(10),FRM_TITLE CHAR(30), OBJ_NAME  CHAR(20), OBJ_CAPTION  CHAR(30), OBJ_VISIBLE  CHAR(1), OBJ_WIDTH NUMBER(5), COL_NO    NUMBER(5), ENT_ID    CHAR(6), ENT_BY    CHAR(15), ENT_DT    DATE, EDT_BY    CHAR(15), EDT_DT    DATE, FRM_HEADER   CHAR(30), OBJ_MAXLEN   NUMBER(6), OBJ_READONLY     VARCHAR2(1), OBJ_FMAND     VARCHAR2(1) DEFAULT 'N' )");
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='REP_CONFIG'", "tname");
                if (mq0 == "0" || mq0 == "")
                {
                    execute_cmd(Qstr, CoCD_Fgen, "CREATE TABLE REP_CONFIG (BRANCHCD  CHAR(2), TYPE CHAR(2), VCHNUM    CHAR(6), VCHDATE   DATE, SRNO      NUMBER(4), FRM_NAME  VARCHAR2(10), FRM_TITLE CHAR(30), OBJ_NAME  VARCHAR2(100), OBJ_CAPTION  VARCHAR2(40), OBJ_VISIBLE  CHAR(1), OBJ_WIDTH NUMBER(5), COL_NO    NUMBER(5), ENT_ID    CHAR(6), ENT_BY    CHAR(15), ENT_DT    DATE, EDT_BY    CHAR(15), EDT_DT    DATE, FRM_HEADER   CHAR(30), OBJ_MAXLEN   NUMBER(6), OBJ_READONLY VARCHAR2(1), TABLE1    VARCHAR2(20), TABLE2    VARCHAR2(20), TABLE3    VARCHAR2(20), TABLE4    VARCHAR2(20), DATE_FLD  VARCHAR2(20), SORT_FLD  VARCHAR2(40), JOIN_COND VARCHAR2(175))");
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'UDF_CONFIG'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE UDF_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), FRM_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', OBJ_NAME varchar2(20)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(1) default '-')";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DBD_CONFIG'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE DBD_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), FRM_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', OBJ_NAME varchar2(20)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(1) default '-', OBJ_SQL VARCHAR2(1000) default '-')";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DSK_CONFIG'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE DSK_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), FRM_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', OBJ_NAME varchar2(100)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(1) default '-', OBJ_SQL VARCHAR2(1000) default '-', OBJ_SQL2 VARCHAR2(1000) default '-')";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DSK_WCONFIG'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE DSK_WCONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), USERID VARCHAr(10), USERNAME VARCHAR(30),OBJ_NAME VARCHAR(100) )";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'UDF_DATA'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE UDF_DATA (BRANCHCD CHAR(2),PAR_TBL VARCHAR2(30),PAR_FLD VARCHAR2(30),UDF_NAME VARCHAR2(30),UDF_VALUE VARCHAR2(100),SRNO NUMBER(4))";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DBD_TV_CONFIG'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE DBD_TV_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), VERT_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', FRM_NAME varchar2(50)  default '-', OBJ_NAME varchar2(20)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(10) default '-', OBJ_SQL VARCHAR2(1000) default '-')";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = check_filed_name(Qstr, CoCD_Fgen, "DBD_TV_CONFIG", "OBJ_READONLY");
                if (mq0 == "0")
                {
                    oraSQuery = "alter table DBD_TV_CONFIG modify OBJ_READONLY VARCHAR2(10) default '-'";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
                mq0 = check_filed_name(Qstr, CoCD_Fgen, "DBD_TV_CONFIG", "FRM_NAME");
                if (mq0 == "0")
                {
                    oraSQuery = "alter table DBD_TV_CONFIG add FRM_NAME VARCHAR2(50) default '-'";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
            }
            mhd = chk_RsysUpd("CT0002");
            if (mhd == "0" || mhd == "")
            {
                //execute_cmd(Qstr, CoCD_Fgen, "insert into FIN_RSYS_UPD values ('CT0002','DEV_A',sysdate)");
                add_RsysUpd(Qstr, CoCD_Fgen, "CT0002", "DEV_A");

                mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DSC_INFO'", "TNAME");
                if (mq0 == "0")
                {
                    oraSQuery = "CREATE TABLE DSC_INFO (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, REMARKS VARCHAR(80),FILENAME VARCHAR(60), FILEPATH VARCHAR(80), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate)";
                    execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
                }
            }
            //mhd = check_filed_name(Qstr, CoCD_Fgen, "FIN_MSYS", "MSRNO"); if (mhd == "0") execute_cmd(Qstr, CoCD_Fgen, "Alter Table FIN_MSYS add MSRNO NUMBER(7,3) default 1");
        }
        public DataTable search_vip(string Qstr, string co_Cd, string Query, string SearchText)
        {
            DataTable vdt = new DataTable();
            using (DataTable dt_srch_vp = getdata(Qstr, co_Cd, "Select * from ( " + Query + " ) where rownum<3"))
            {
                mq0 = "";
                foreach (DataColumn dc in dt_srch_vp.Columns)
                {
                    if (mq0.Length > 0) mq0 = mq0 + "||" + dc.ToString();
                    else mq0 = dc.ToString();
                }
                vdt = getdata(Qstr, co_Cd, "Select * from ( " + Query + " ) where upper(trim(" + mq0 + ")) like '%" + SearchText.Trim().ToUpper() + "%'");
            }
            return vdt;
        }
        public DataTable search_vip1(string Qstr, string co_Cd, string Query, string SearchText, DataTable dt_SEARCH)
        {
            string mq0 = "";
            DataTable vdt = new DataTable();
            vdt = null;
            try
            {
                if (dt_SEARCH.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dt_SEARCH.Columns)
                    {
                        if (mq0.Length > 0) mq0 = mq0 + "||" + dc.ToString();
                        else mq0 = dc.ToString();
                    }
                }
                else
                {
                    DataTable dt_srch_vp = new DataTable();
                    dt_srch_vp = getdata(Qstr, co_Cd, "Select * from ( " + Query + " ) where rownum<3");

                    foreach (DataColumn dc in dt_srch_vp.Columns)
                    {
                        if (mq0.Length > 0) mq0 = mq0 + "||" + dc.ToString();
                        else mq0 = dc.ToString();
                    }
                }

                vdt = getdata(Qstr, co_Cd, "Select * from ( " + Query + " ) where upper(trim(" + mq0 + ")) like '%" + SearchText.Trim().ToUpper() + "%'");
            }
            catch (Exception ex)
            {
                FILL_ERR("In Search String :=> " + ex.Message);
            }
            return vdt;
        }
        public void save_info(string Qstr, string pco_Cd, string mbr, string zvnum, string zvdate, string zuser, string ztype, string zremark)
        {
            using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "fininfo"))
            {
                DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                fgen_oporow["BRANCHCD"] = mbr;
                fgen_oporow["TYPE"] = ztype;
                fgen_oporow["VCHNUM"] = zvnum;
                fgen_oporow["VCHDATE"] = zvdate;
                fgen_oporow["ENT_BY"] = zuser;
                fgen_oporow["ENT_DT"] = System.DateTime.Now;
                fgen_oporow["fcomment"] = zremark;
                mq0 = GetIpAddress().ToString().ToUpper() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);
                fgen_oporow["terminal"] = mq0;
                fgen_oporow["Iremarks"] = zremark;
                fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                save_data(Qstr, pco_Cd, fgen_oDS, "fininfo");
            }
        }

        public void save_type(string Qstr, string pco_Cd, string mid, string mtype1, string mname)
        {
            string chk_type = "";
            chk_type = seek_iname(Qstr, pco_Cd, "SELECT ID from type where id='" + mid + "' and trim(type1)='" + mtype1.Trim() + "'", "ID");
            if (chk_type.Trim() == "0")
            {
                using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "TYPE"))
                {
                    DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                    fgen_oporow["TBRANCHCD"] = "00";
                    fgen_oporow["ID"] = mid;
                    fgen_oporow["TYPE1"] = mtype1;
                    fgen_oporow["NAME"] = mname;
                    fgen_oporow["ENT_BY"] = "DEV_A";
                    fgen_oporow["ENT_DT"] = System.DateTime.Now;
                    fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                    save_data(Qstr, pco_Cd, fgen_oDS, "TYPE");
                }
            }
            else
            {
                return;
            }
        }


        public void save_info_mac(string Qstr, string pco_Cd, string mbr, string zvnum, string zvdate, string zuser, string ztype, string zremark)
        {
            using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "fininfo"))
            {
                DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                fgen_oporow["BRANCHCD"] = mbr;
                fgen_oporow["TYPE"] = ztype;
                fgen_oporow["VCHNUM"] = zvnum;
                fgen_oporow["VCHDATE"] = zvdate;
                fgen_oporow["ENT_BY"] = zuser;
                fgen_oporow["ENT_DT"] = System.DateTime.Now;
                fgen_oporow["fcomment"] = zremark;
                mq0 = GetIpAddress().ToString().ToUpper().Trim() + "," + GetMACAddress().ToString().Trim() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);
                fgen_oporow["terminal"] = mq0;
                fgen_oporow["Iremarks"] = zremark;
                fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                save_data(Qstr, pco_Cd, fgen_oDS, "fininfo");
            }
        }

        public string save_Mailbox2(string Uniq_QSTR, string compCode, string curr_form, string cur_br, string msg_2_save, string from_Usr, string m_ed_mode)
        {
            string subj = "New : ";
            if (m_ed_mode.Trim() == "Y")
            {
                subj = "Edit : ";
            }
            string mUsrcode = seek_iname(Uniq_QSTR, compCode, "select userid as cSource from evas where username='" + from_Usr + "'", "cSource");
            string mq0;
            mq0 = GetIpAddress().ToString().ToUpper() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
            if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);

            string terminal = seek_iname(Uniq_QSTR, compCode, "select userenv('terminal')||' ,'||sysdate||' '||to_char(sysdate,'HH:MI:SS PM') as cSource from dual", "cSource");
            DataTable dtMailMgr = new DataTable();
            dtMailMgr = getdata(Uniq_QSTR, compCode, "select distinct Ecode from (SELECT trim(a.ECODE) as Ecode FROM WB_MAIL_MGR a WHERE A.TYPE='MM' AND TRIM(a.RCODE)='" + curr_form + "' union all Select trim('" + mUsrcode + "') as Ecode from dual) order by Ecode");
            foreach (DataRow dr in dtMailMgr.Rows)
            {
                try
                {
                    string vnum = next_no(Uniq_QSTR, compCode, "select max(vchnum) as vchnum from mailbox2 where type='10'", 6, "vchnum");
                    using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, "mailbox2"))
                    {
                        DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                        fgen_oporow["BRANCHCD"] = cur_br;
                        fgen_oporow["TYPE"] = "10";
                        fgen_oporow["VCHNUM"] = vnum;
                        fgen_oporow["VCHDATE"] = DateTime.Now.ToString("dd/MM/yyyy");

                        fgen_oporow["msgto"] = dr["ECODE"].ToString().Trim();
                        fgen_oporow["msgfrom"] = from_Usr;
                        fgen_oporow["terminal"] = terminal;

                        fgen_oporow["msgtxt"] = subj + " " + msg_2_save + " Msg From Computer : " + terminal;
                        fgen_oporow["msgdt"] = mUsrcode;
                        fgen_oporow["msgseen"] = "N";

                        fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                        save_data(Uniq_QSTR, compCode, fgen_oDS, "mailbox2");
                    }
                }
                catch (Exception ex) { FILL_ERR("In Mailbox2 Saving :=> " + ex.Message.ToString().Trim()); }
            }
            return "";
        }

        public string save_Mailbox3(string Uniq_QSTR, string compCode, string cur_br, string mail_to, string msg_2_save, string from_Usr, string m_ed_mode)
        {
            string mUsrcode = seek_iname(Uniq_QSTR, compCode, "select userid as cSource from evas where username='" + from_Usr + "'", "cSource");
            string mq0;
            mq0 = GetIpAddress().ToString().ToUpper() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
            if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);

            string terminal = seek_iname(Uniq_QSTR, compCode, "select userenv('terminal')||' ,'||sysdate||' '||to_char(sysdate,'HH:MI:SS PM') as cSource from dual", "cSource");
            {
                try
                {
                    string vnum = next_no(Uniq_QSTR, compCode, "select max(vchnum) as vchnum from mailbox3 where type='20'", 6, "vchnum");
                    using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, "mailbox3"))
                    {
                        DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                        fgen_oporow["BRANCHCD"] = cur_br;
                        fgen_oporow["TYPE"] = "20";
                        fgen_oporow["VCHNUM"] = vnum;
                        fgen_oporow["VCHDATE"] = DateTime.Now.ToString("dd/MM/yyyy");

                        fgen_oporow["msgto"] = mail_to;
                        fgen_oporow["msgfrom"] = from_Usr;

                        fgen_oporow["msgtxt"] = msg_2_save;

                        fgen_oporow["ent_by"] = from_Usr;
                        fgen_oporow["ent_dt"] = fgen_oporow["VCHDATE"];

                        fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                        save_data(Uniq_QSTR, compCode, fgen_oDS, "mailbox3");
                    }
                }
                catch (Exception ex) { FILL_ERR("In Mailbox2 Saving :=> " + ex.Message.ToString().Trim()); }
            }
            return "";
        }

        public void save_SYSOPT(string Qstr, string pco_Cd, string mbr, string ztype, string zvdate, string zuser, string zopt_id, string zopt_text, string zopt_enable, string zopt_param)
        {
            string doc_no;
            string mhd;
            mhd = "N";
            mhd = seek_iname(Qstr, pco_Cd, "Select 'Y' as opt_exist from fin_rsys_OPT where trim(OPT_ID)='" + zopt_id.ToUpper() + "'", "opt_exist");
            if (mhd != "Y")
            {
                using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "fin_rsys_opt"))
                {
                    DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                    fgen_oporow["BRANCHCD"] = mbr;
                    fgen_oporow["TYPE"] = ztype;

                    doc_no = zopt_id.Substring(1, 4);
                    fgen_oporow["VCHNUM"] = doc_no.PadLeft(6, '0');
                    fgen_oporow["VCHDATE"] = zvdate;

                    fgen_oporow["OPT_ID"] = zopt_id.ToUpper();
                    fgen_oporow["OPT_TEXT"] = zopt_text.ToUpper();
                    fgen_oporow["OPT_ENABLE"] = zopt_enable.ToUpper();
                    fgen_oporow["OPT_PARAM"] = zopt_param.ToUpper();
                    fgen_oporow["OPT_PARAM2"] = "-";
                    fgen_oporow["OPT_EXCL"] = "-";

                    fgen_oporow["ENT_BY"] = zuser;
                    fgen_oporow["ENT_DT"] = System.DateTime.Now;
                    fgen_oporow["EDT_BY"] = "-";
                    fgen_oporow["EDT_DT"] = System.DateTime.Now;

                    fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                    save_data(Qstr, pco_Cd, fgen_oDS, "fin_rsys_opt");
                }
            }
            if (zopt_id.Right(4).toDouble() > 1000)
            {
                DataTable dtBranch = new DataTable();
                dtBranch = getdata(Qstr, pco_Cd, "SELECT TYPE1 FROM TYPE WHERE ID='B' ORDER BY TYPE1");
                foreach (DataRow drBranch in dtBranch.Rows)
                {
                    mhd = seek_iname(Qstr, pco_Cd, "Select 'Y' as opt_exist from FIN_RSYS_OPT_PW where trim(OPT_ID)='" + zopt_id.ToUpper() + "' and branchcd='" + drBranch["type1"].ToString().Trim() + "' ", "opt_exist");
                    if (mhd != "Y")
                    {
                        using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "fin_rsys_opt_pw"))
                        {
                            DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                            fgen_oporow["BRANCHCD"] = drBranch["type1"].ToString().Trim();
                            fgen_oporow["TYPE"] = ztype;

                            doc_no = zopt_id.Substring(1, 4);
                            fgen_oporow["VCHNUM"] = doc_no.PadLeft(6, '0');
                            fgen_oporow["VCHDATE"] = zvdate;

                            fgen_oporow["OPT_ID"] = zopt_id.ToUpper();
                            fgen_oporow["OPT_TEXT"] = zopt_text.ToUpper();
                            fgen_oporow["OPT_ENABLE"] = zopt_enable.ToUpper();
                            fgen_oporow["OPT_PARAM"] = zopt_param.ToUpper();
                            fgen_oporow["OPT_PARAM2"] = "-";


                            fgen_oporow["ENT_BY"] = zuser;
                            fgen_oporow["ENT_DT"] = System.DateTime.Now;
                            fgen_oporow["EDT_BY"] = "-";
                            fgen_oporow["EDT_DT"] = System.DateTime.Now;

                            fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                            save_data(Qstr, pco_Cd, fgen_oDS, "fin_rsys_opt_pw");
                        }
                    }
                }
            }
        }

        public void save_datax(string Comp_Code, DataSet oDs, string tab_name)
        {
            using (OracleConnection fcon = new OracleConnection(fgenCO.connStr))
            {
                fcon.Open();
                using (OracleDataAdapter fgen_da = new OracleDataAdapter("select * from " + tab_name + " where 1=2", fcon))
                {
                    using (OracleCommandBuilder cb = new OracleCommandBuilder(fgen_da))
                    {
                        string field_type = "";
                        for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
                        {
                            for (int z = 0; z < oDs.Tables[0].Columns.Count; z++)
                            {
                                field_type = oDs.Tables[0].Columns[z].DataType.Name.ToString();
                                if (field_type.ToUpper() == "DATETIME" && oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "DECIMAL" && oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else oDs.Tables[0].Rows[i][z] = oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Replace("&nbsp;", "-").Replace("&amp;", "-").Replace(@"\", "/").Trim();
                            }
                        }
                        oDs.Tables[0].TableName = tab_name;
                        fgen_da.Update(oDs, tab_name);
                        oDs.Dispose();
                    }
                }
            }
        }
        public string save_data(string uniqStr, string Comp_Code, DataSet _oDs, string tab_name)
        {
            if (Comp_Code == "0") Comp_Code = uniqStr.Split('^')[0];
            string constr = fgenMV.Fn_Get_Mvar(uniqStr, "CONN");
            string saveSuccessed = "N";
            if (constr == "0") { constr = ConnInfo.connString(Comp_Code); }
            //cow
            try
            {
                using (OracleConnection fcon = new OracleConnection(constr))
                {
                    fcon.Open();
                    using (OracleDataAdapter fgen_da = new OracleDataAdapter("select * from " + tab_name + " where 1=2", fcon))
                    {
                        using (OracleCommandBuilder cb = new OracleCommandBuilder(fgen_da))
                        {
                            string field_type = "";
                            for (int i = 0; i < _oDs.Tables[0].Rows.Count; i++)
                            {
                                for (int z = 0; z < _oDs.Tables[0].Columns.Count; z++)
                                {
                                    field_type = _oDs.Tables[0].Columns[z].DataType.Name.ToString();
                                    if (field_type.ToUpper() == "DATETIME" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else if (field_type.ToUpper() == "DECIMAL" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else if (field_type.ToUpper() == "DOUBLE" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else if (field_type.ToUpper() == "SINGLE" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else if (field_type.ToUpper() == "INT16" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else if (field_type.ToUpper() == "INT32" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else if (field_type.ToUpper() == "INT64" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else if (field_type.ToUpper() == "BOOL" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                    else _oDs.Tables[0].Rows[i][z] = _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Replace("&nbsp;", "-").Replace("&amp;", "-").Replace(@"\", "/").Trim();
                                }
                            }
                            _oDs.Tables[0].TableName = tab_name;
                            fgen_da.Update(_oDs, tab_name);
                            _oDs.Dispose();
                            saveSuccessed = "Y";
                            cb.Dispose();
                        }
                        fgen_da.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                FILL_ERR("In Save-Data Fn " + ex.Message);
                saveSuccessed = "N";
                throw;
            }
            //cow
            return saveSuccessed;
        }
        public void chk()
        {

        }
        public void saveWithThread()
        {
            DataSet set = new DataSet();
            System.Threading.Thread saveThread = new System.Threading.Thread(new System.Threading.ThreadStart(chk));
            saveThread.Start();
        }

        public void updSave(string uniqStr, string Comp_Code, DataTable oDTable, string tab_name, string primaryKeyField)
        {
            string constr = fgenMV.Fn_Get_Mvar(uniqStr, "CONN");
            if (constr == "0") { constr = ConnInfo.connString(Comp_Code); }
            DataSet odS = new DataSet();
            string deletePk = "N";
            if (oDTable.PrimaryKey.Length == 0 || oDTable.PrimaryKey == null)
            {
                deletePk = "Y";
                execute_cmd(uniqStr, Comp_Code, "ALTER TABLE " + tab_name + " ADD CONSTRAINT " + primaryKeyField + "_PK PRIMARY KEY (" + primaryKeyField + ") ");
                //oDTable.PrimaryKey = new DataColumn[] { oDTable.Columns[primaryKeyField] };
            }
            oDTable.TableName = tab_name;
            odS.Tables.Add(oDTable);
            using (OracleConnection fcon = new OracleConnection(constr))
            {
                //fcon.Open();
                using (OracleDataAdapter fgen_da = new OracleDataAdapter("select * from " + tab_name + "", fcon))
                {
                    using (OracleCommandBuilder cb = new OracleCommandBuilder(fgen_da))
                    {
                        fgen_da.UpdateCommand = cb.GetUpdateCommand(true);
                        fgen_da.Update(odS, tab_name);

                        if (deletePk == "Y") execute_cmd(uniqStr, Comp_Code, "ALTER TABLE " + tab_name + " DROP CONSTRAINT " + primaryKeyField + "_PK ");
                    }
                }
            }
        }
        public DataSet fill_schema(string Qstr, string pco_CD, string tab_name)
        {
            DataSet fgen_oDS = new DataSet();
            if (pco_CD == "0") pco_CD = Qstr.Split('^')[0];
            string constr = fgenMV.Fn_Get_Mvar(Qstr, "CONN");
            if (constr == "0") { constr = ConnInfo.connString(pco_CD); }

            if (!constr.ToUpper().Contains("USER ID")) { constr = ConnInfo.connString(pco_CD); }
            if (!constr.ToUpper().Contains("PASSWORD")) { constr = ConnInfo.connString(pco_CD); }


            using (OracleConnection fcon = new OracleConnection(constr))
            {
                fcon.Open();
                using (OracleDataAdapter fgen_da = new OracleDataAdapter(new OracleCommand("SELECT * FROM " + tab_name + " where 1=2 ", fcon)))
                {
                    using (OracleCommandBuilder cb = new OracleCommandBuilder(fgen_da))
                    {
                        fgen_da.FillSchema(fgen_oDS, SchemaType.Source);
                    }
                }
            }
            return fgen_oDS;
        }
        public string chk_pk(string Qstr, string pco_cd, string pk_str, string mbr, string vty, string vchnum, string vchdate, string acode, string uname)
        {
            try
            {
                using (DataSet oDS1 = fill_schema(Qstr, pco_cd, "Wsr_ctrl"))
                {
                    DataRow fgen_oporow = null;
                    fgen_oporow = oDS1.Tables[0].NewRow();
                    fgen_oporow["FINPKFLD"] = pk_str.ToUpper();
                    fgen_oporow["BRANCHCD"] = mbr;
                    fgen_oporow["TYPE"] = vty;
                    fgen_oporow["VCHNUM"] = vchnum;
                    fgen_oporow["VCHDATE"] = vchdate;
                    fgen_oporow["ENT_BY"] = uname;
                    fgen_oporow["ENT_DT"] = System.DateTime.Now.ToString("dd/MM/yyyy");
                    fgen_oporow["PRINTED"] = 0;
                    fgen_oporow["ACODE"] = acode;
                    oDS1.Tables[0].Rows.Add(fgen_oporow);
                    pk_error = (save_data(Qstr, pco_cd, oDS1, "wsr_ctrl") == "Y") ? "N" : "Y";
                }
            }
            catch (Exception ex)
            {
                pk_error = "Y";
                FILL_ERR("In PK Function :=>" + ex.Message);
            }
            return pk_error;
        }
        /// <summary>
        /// Primary Key Checking, with new tables
        /// </summary>
        /// <param name="Qstr"></param>
        /// <param name="pco_cd"></param>
        /// <param name="pk_str"></param>
        /// <param name="mbr"></param>
        /// <param name="vty"></param>
        /// <param name="vchnum"></param>
        /// <param name="vchdate"></param>
        /// <param name="acode"></param>
        /// <param name="uname"></param>
        /// <param name="tb">Type 1,2,3 or 4, this will save the data in new table</param>
        /// <returns></returns>
        public string chk_pk(string Qstr, string pco_cd, string pk_str, string mbr, string vty, string vchnum, string vchdate, string acode, string uname, int tbNo)
        {
            try
            {
                string tabName = "WSR_CTRL" + tbNo.ToString();
                using (DataSet oDS1 = fill_schema(Qstr, pco_cd, tabName))
                {
                    DataRow fgen_oporow = null;
                    fgen_oporow = oDS1.Tables[0].NewRow();
                    fgen_oporow["FINPKFLD"] = pk_str.ToUpper();
                    fgen_oporow["BRANCHCD"] = mbr;
                    fgen_oporow["TYPE"] = vty;
                    fgen_oporow["VCHNUM"] = vchnum;
                    fgen_oporow["VCHDATE"] = vchdate;
                    fgen_oporow["ENT_BY"] = uname;
                    fgen_oporow["ENT_DT"] = System.DateTime.Now.ToString("dd/MM/yyyy");
                    fgen_oporow["PRINTED"] = 0;
                    fgen_oporow["ACODE"] = acode;
                    oDS1.Tables[0].Rows.Add(fgen_oporow);
                    pk_error = (save_data(Qstr, pco_cd, oDS1, "wsr_ctrl") == "Y") ? "N" : "Y";
                }
            }
            catch (Exception ex)
            {
                pk_error = "Y";
                FILL_ERR("In PK Function :=>" + ex.Message);
            }
            return pk_error;
        }
        /// <summary>
        /// Primary Key Checking, with new tables
        /// </summary>
        /// <param name="Qstr"></param>
        /// <param name="pco_cd"></param>
        /// <param name="pk_str"></param>
        /// <param name="mbr"></param>
        /// <param name="vty"></param>
        /// <param name="vchnum"></param>
        /// <param name="vchdate"></param>
        /// <param name="acode"></param>
        /// <param name="uname"></param>
        /// <param name="tb">Manually write Table Name</param>
        /// <returns></returns>
        public string chk_pk(string Qstr, string pco_cd, string pk_str, string mbr, string vty, string vchnum, string vchdate, string acode, string uname, string tabName)
        {
            try
            {
                using (DataSet oDS1 = fill_schema(Qstr, pco_cd, tabName))
                {
                    DataRow fgen_oporow = null;
                    fgen_oporow = oDS1.Tables[0].NewRow();
                    fgen_oporow["FINPKFLD"] = pk_str.ToUpper();
                    fgen_oporow["BRANCHCD"] = mbr;
                    fgen_oporow["TYPE"] = vty;
                    fgen_oporow["VCHNUM"] = vchnum;
                    fgen_oporow["VCHDATE"] = vchdate;
                    fgen_oporow["ENT_BY"] = uname;
                    fgen_oporow["ENT_DT"] = System.DateTime.Now.ToString("dd/MM/yyyy");
                    fgen_oporow["PRINTED"] = 0;
                    fgen_oporow["ACODE"] = acode;
                    oDS1.Tables[0].Rows.Add(fgen_oporow);
                    pk_error = (save_data(Qstr, pco_cd, oDS1, tabName) == "Y") ? "N" : "Y";
                }
            }
            catch (Exception ex)
            {
                pk_error = "Y";
                FILL_ERR("In PK Function :=>" + ex.Message);
            }
            return pk_error;
        }
        public DataSet Get_Type_Data(string Qstr, string pco_Cd, string mbr, DataSet ds)
        {
            string branchNameAsFirmName = "", br_name = "name";
            string footerGeneratedBy = "Generated By Tejaxo ERP Web";
            string printRegHeadings = fgenMV.Fn_Get_Mvar(Qstr, "U_PRINT_REG_HEADINGS");
            firm = checkSpecialFirm(pco_Cd, mbr);

            MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(Qstr, "U_CLIENT_GRP");
            if (MV_CLIENT_GRP == "SG_TYPE" || pco_Cd == "MLGI" || pco_Cd == "ADVG" || pco_Cd == "HIMT" || pco_Cd == "MASS"|| pco_Cd == "MAST" || pco_Cd == "KCLM") branchNameAsFirmName = "Y";

            firm = "'" + firm + "' as firm";
            if (branchNameAsFirmName == "Y" && firm.ToUpper().Left(5) != "'AKIT")
            {
                firm = "name as firm";
                br_name = "' '";
                if (MV_CLIENT_GRP == "SG_TYPE") br_name = "'UNIT OF SALMAN GROUP'";
                if (MV_CLIENT_GRP == "SG_TYPE" && mbr == "00")
                {
                    firm = "'SALMAN GROUP' AS FIRM";
                    br_name = "name";
                }
            }
            if (firm.ToUpper().Left(5) == "'AKIT") br_name = "' '";
            if (pco_Cd == "0") pco_Cd = Qstr.Split('^')[0];
            if (fgenMV.Fn_Get_Mvar(Qstr, "U_UATS") == "Y") firm = "'Tejaxo UAT SERVER[" + pco_Cd + "]' as firm";
            using (OracleConnection fcon = new OracleConnection(fgenMV.Fn_Get_Mvar(Qstr, "CONN")))
            {
                fcon.Open();
                //if (pco_Cd == "DESH")
                //{
                //    using (OracleDataAdapter fgen_da = new OracleDataAdapter("select " + br_name + " as brName,addr as brAddr,addr1 as brAddr1,addr2 as brAddr2,place as brplace,tele as brTele,fax as brFax,TO_CHAR(rcdate,'DD/MM/YYYY') AS brRCDATE,TO_CHAR(cstdt,'DD/MM/YYYY') AS brcstdt,ec_code as brec_code,exc_regn as brexc_regn,exc_rang as brexc_rang,exc_div as brexc_div, RCNUM as brRCNUM, cstno as brcstno,LOWER(email) as br_email," + firm + ",'" + pco_Cd + "' as co_cd,'" + footerGeneratedBy + "' as footerGeneratedBy, LOWER(website) AS brwebsite,exc_tarrif as brexc_tarrif,gir_num as brgir_num,zipcode as brzipcode, bank_pf as brbank_pf, mfg_licno as brmfg_licno, est_code as brest_code, tds_num as brtds_num, exc_item as brexc_item, BANKNAME as brBANKNAME, BANKaddr as brBANKaddr, BANKaddr1 as brBANKaddr1 , BANKac as brBANKac , vat_form as brvat_form, stform as brstform, IFSC_CODE as brIFSC_CODE, RADDR as brRADDR, RADDR1 as brRADDR1, haddr as brhaddr, haddr1 as brhaddr1 , rphone as brrphone, hphone as brhphone, email1 as bremail1, email2 as bremail2, email3 as bremail3, email4 as bremail4, email5 as bremail5, co_cin as brco_cin, countrynm as brcountrynm, msme_no as brmsme_no, exc_Addr as brexc_Addr , gst_no as brgst_no , substr(a.gst_no,0,2) as brstatecode, bond_ut as brbond_ut, STATENM as brSTATENM,cexc_comm,AUDIT_," + printRegHeadings + ",br_curren,exc_rang as paisa_curren,num_fmt1,num_fmt2,NOTIFICATION from type a where a.type1='" + mbr + "' and upper(a.id)='B'", fcon))
                //    {
                //        fgen_da.Fill(ds, "Type");
                //    }
                //}
                //else
                //{
                using (OracleDataAdapter fgen_da = new OracleDataAdapter("select " + br_name + " as brName,addr as brAddr,addr1 as brAddr1,addr2 as brAddr2,place as brplace,tele as brTele,fax as brFax,TO_CHAR(rcdate,'DD/MM/YYYY') AS brRCDATE,TO_CHAR(cstdt,'DD/MM/YYYY') AS brcstdt,ec_code as brec_code,exc_regn as brexc_regn,exc_rang as brexc_rang,exc_div as brexc_div, RCNUM as brRCNUM, cstno as brcstno,LOWER(email) as br_email," + firm + ",'" + pco_Cd + "' as co_cd,'" + footerGeneratedBy + "' as footerGeneratedBy, LOWER(website) AS brwebsite,exc_tarrif as brexc_tarrif,gir_num as brgir_num,zipcode as brzipcode, bank_pf as brbank_pf, mfg_licno as brmfg_licno, est_code as brest_code, tds_num as brtds_num, exc_item as brexc_item, BANKNAME as brBANKNAME, BANKaddr as brBANKaddr, BANKaddr1 as brBANKaddr1 , BANKac as brBANKac , vat_form as brvat_form, stform as brstform, IFSC_CODE as brIFSC_CODE, RADDR as brRADDR, RADDR1 as brRADDR1, haddr as brhaddr, haddr1 as brhaddr1 , rphone as brrphone, hphone as brhphone, email1 as bremail1, email2 as bremail2, email3 as bremail3, email4 as bremail4, email5 as bremail5, co_cin as brco_cin, countrynm as brcountrynm, msme_no as brmsme_no, exc_Addr as brexc_Addr , gst_no as brgst_no , substr(a.gst_no,0,2) as brstatecode, bond_ut as brbond_ut, STATENM as brSTATENM,cexc_comm,AUDIT_," + printRegHeadings + ",br_curren,exc_rang as paisa_curren,num_fmt1,num_fmt2,NOTIFICATION from type a where upper(a.id)='B'", fcon))
                {
                    fgen_da.Fill(ds, "Type");
                }
                //}
            }
            return ds;
        }
        public string checkSpecialFirm(string pcocd, string fmbr)
        {
            switch (pcocd)
            {
                case "PRIN":
                    switch (fmbr)
                    {
                        case "00":
                            firm = "PREM INDUSTRIES UNIT III";
                            break;
                        case "01":
                            firm = "PREM INDUSTRIES UNIT II";
                            break;
                        case "02X":
                            firm = "PREM INDUSTRIES UNIT II";
                            break;
                        case "03":
                            firm = "PREM INDUSTRIES UNIT I";
                            break;
                        case "04":
                        case "02":
                        case "06":
                        case "07":
                            firm = "PREM INDUSTRIES";
                            break;
                        case "05":
                            firm = "PREM INDUSTRIES WAREHOUSE";
                            break;
                        case "06x":
                            firm = "PREM INDUSTRIES LUCKNOW WAREHOUSE";
                            break;
                        default:
                            firm = "PREM INDUSTRIES UNIT III";
                            break;
                    }
                    break;
                case "OTTO":
                    switch (fmbr)
                    {
                        case "01":
                            firm = "OTTOMAN TUBES PVT LTD.";
                            break;
                        default:
                            firm = "OTTOMAN INDUSTRIES PVT LTD.";
                            break;
                    }
                    break;
                case "BNPL":
                    switch (fmbr)
                    {
                        case "00":
                            firm = "BNPACK CORRUGATED PRIVATE LIMITED";
                            break;
                    }
                    break;
                default:
                    firm = fgenCO.chk_co(pcocd);
                    break;
            }
            try
            {
                firm = fgenCO.chk_Akito(HttpContext.Current.Request.Cookies["UNAME"].Value.ToString(), firm);
            }
            catch { }
            return firm;
        }
        public DataTable Get_Type_Data(string Qstr, string pco_Cd, string mbr)
        {
            string sPrg_Id = "";
            string branchNameAsFirmName = "", br_name = "name";
            sPrg_Id = fgenMV.Fn_Get_Mvar(Qstr, "U_FORMID");
            string footerGeneratedBy = "Generated By Tejaxo ERP Web    ID:" + sPrg_Id;
            if (pco_Cd == "SRIS") footerGeneratedBy = "Generated By SRISOL ERP Web    ID:" + sPrg_Id;
            printRegHeadings = fgenMV.Fn_Get_Mvar(Qstr, "U_PRINT_REG_HEADINGS");
            DataTable ds = new DataTable();
            firm = checkSpecialFirm(pco_Cd, mbr);

            MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(Qstr, "U_CLIENT_GRP");
            if (MV_CLIENT_GRP == "SG_TYPE" || pco_Cd == "MLGI" || pco_Cd == "ADVG" || pco_Cd == "HIMT" || pco_Cd == "MASS" || pco_Cd == "MAST" || pco_Cd == "KCLM") branchNameAsFirmName = "Y";

            firm = "'" + firm + "' as firm";
            if (branchNameAsFirmName == "Y" && firm.ToUpper().Left(5) != "'AKIT")
            {
                firm = "name as firm";
                br_name = "' '";
                if (MV_CLIENT_GRP == "SG_TYPE") br_name = "'UNIT OF SALMAN GROUP'";
                if (MV_CLIENT_GRP == "SG_TYPE" && mbr == "00")
                {
                    firm = "'SALMAN GROUP' AS FIRM";
                    br_name = "name";
                }
            }
            if (firm.ToUpper().Left(5) == "'AKIT") br_name = "' '";
            if (fgenMV.Fn_Get_Mvar(Qstr, "U_UATS") == "Y") firm = "'Tejaxo UAT SERVER[" + pco_Cd + "]' as firm";
            ds = getdata(Qstr, pco_Cd, "select " + br_name + " as brName,addr as brAddr,addr1 as brAddr1,addr2 as brAddr2,place as brplace,tele as brTele,fax as brFax,TO_CHAR(rcdate,'DD/MM/YYYY') AS brRCDATE,TO_CHAR(cstdt,'DD/MM/YYYY') AS brcstdt,ec_code as brec_code,exc_regn as brexc_regn,exc_rang as brexc_rang,exc_div as brexc_div, RCNUM as brRCNUM, cstno as brcstno,LOWER(email) as br_email," + firm + ",'" + pco_Cd + "' as co_cd,'" + footerGeneratedBy + "' as footerGeneratedBy, LOWER(website) AS brwebsite,exc_tarrif as brexc_tarrif,gir_num as brgir_num,zipcode as brzipcode, bank_pf as brbank_pf, mfg_licno as brmfg_licno, est_code as brest_code, tds_num as brtds_num, exc_item as brexc_item, BANKNAME as brBANKNAME, BANKaddr as brBANKaddr, BANKaddr1 as brBANKaddr1 , BANKac as brBANKac , vat_form as brvat_form, stform as brstform, IFSC_CODE as brIFSC_CODE, RADDR as brRADDR, RADDR1 as brRADDR1, haddr as brhaddr, haddr1 as brhaddr1 , rphone as brrphone, hphone as brhphone, email1 as bremail1, email2 as bremail2, email3 as bremail3, email4 as bremail4, email5 as bremail5, co_cin as brco_cin, countrynm as brcountrynm, msme_no as brmsme_no, exc_Addr as brexc_Addr , gst_no as brgst_no , substr(a.gst_no,0,2) as brstatecode, bond_ut as brbond_ut, STATENM as brSTATENM,cexc_comm,AUDIT_," + printRegHeadings + ",br_curren,exc_rang as paisa_curren,num_fmt1,num_fmt2,NOTIFICATION from type a where a.type1='" + mbr + "' and upper(a.id)='B'");
            ds.TableName = "Type";
            return ds;
        }
        public DataTable Get_Type_Data(string Qstr, string pco_Cd, string mbr, string printLogo)
        {
            string sPrg_Id = "";
            string branchNameAsFirmName = "", br_name = "name";
            string branchWiseLogo = "N"; // we can pick it from control panels or do we need to hard code
            MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(Qstr, "U_CLIENT_GRP");
            if (MV_CLIENT_GRP == "SG_TYPE" || pco_Cd == "MLGI" || pco_Cd == "VIGP") branchWiseLogo = "Y";

            sPrg_Id = fgenMV.Fn_Get_Mvar(Qstr, "U_FORMID");
            string footerGeneratedBy = "Generated By Tejaxo ERP Web    ID:" + sPrg_Id;
            if (pco_Cd == "SRIS") footerGeneratedBy = "Generated By SRISOL ERP Web    ID:" + sPrg_Id;
            DataTable ds = new DataTable();
            printRegHeadings = fgenMV.Fn_Get_Mvar(Qstr, "U_PRINT_REG_HEADINGS");
            firm = checkSpecialFirm(pco_Cd, mbr);


            MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(Qstr, "U_CLIENT_GRP");
            if (MV_CLIENT_GRP == "SG_TYPE" || pco_Cd == "MLGI" || pco_Cd == "ADVG" || pco_Cd == "HIMT" || pco_Cd == "MASS" || pco_Cd == "MAST" || pco_Cd == "KCLM") branchNameAsFirmName = "Y";

            firm = "'" + firm + "' as firm";
            if (branchNameAsFirmName == "Y" && firm.ToUpper().Left(5) != "'AKIT")
            {
                firm = "name as firm";
                br_name = "' '";
                if (MV_CLIENT_GRP == "SG_TYPE") br_name = "'UNIT OF SALMAN GROUP'";
                if (MV_CLIENT_GRP == "SG_TYPE" && mbr == "00")
                {
                    firm = "'SALMAN GROUP' AS FIRM";
                    br_name = "name";
                }
            }
            if (firm.ToUpper().Left(5) == "'AKIT") br_name = "' '";
            if (fgenMV.Fn_Get_Mvar(Qstr, "U_UATS") == "Y") firm = "'Tejaxo UAT SERVER[" + pco_Cd + "]' as firm";
            ds = getdata(Qstr, pco_Cd, "select " + br_name + " as brName,addr as brAddr,addr1 as brAddr1,addr2 as brAddr2,place as brplace,tele as brTele,fax as brFax,TO_CHAR(rcdate,'DD/MM/YYYY') AS brRCDATE,TO_CHAR(cstdt,'DD/MM/YYYY') AS brcstdt,ec_code as brec_code,exc_regn as brexc_regn,exc_rang as brexc_rang,exc_div as brexc_div, RCNUM as brRCNUM, cstno as brcstno,LOWER(email) as br_email," + firm + ",'" + pco_Cd + "' as co_cd,'" + footerGeneratedBy + "' as footerGeneratedBy, LOWER(website) AS brwebsite,exc_tarrif as brexc_tarrif,gir_num as brgir_num,zipcode as brzipcode, bank_pf as brbank_pf, mfg_licno as brmfg_licno, est_code as brest_code, tds_num as brtds_num, exc_item as brexc_item, BANKNAME as brBANKNAME, BANKaddr as brBANKaddr, BANKaddr1 as brBANKaddr1 , BANKac as brBANKac , vat_form as brvat_form, stform as brstform, IFSC_CODE as brIFSC_CODE, RADDR as brRADDR, RADDR1 as brRADDR1, haddr as brhaddr, haddr1 as brhaddr1 , rphone as brrphone, hphone as brhphone, email1 as bremail1, email2 as bremail2, email3 as bremail3, email4 as bremail4, email5 as bremail5, co_cin as brco_cin, countrynm as brcountrynm, msme_no as brmsme_no, exc_Addr as brexc_Addr , gst_no as brgst_no , substr(a.gst_no,0,2) as brstatecode, bond_ut as brbond_ut, STATENM as brSTATENM,cexc_comm,AUDIT_," + printRegHeadings + ",br_curren,exc_rang as paisa_curren,num_fmt1,num_fmt2,NOTIFICATION from type a where a.type1='" + mbr + "' and upper(a.id)='B'");
            ds.TableName = "Type";
            if (printLogo == "Y")
            {
                if (branchWiseLogo == "Y") ds = addLogo(pco_Cd, ds, mbr);
                else ds = addLogo(pco_Cd, ds, "");
            }
            return ds;
        }

        DataTable addLogo(string fCocd, DataTable dataTable, string branchCode)
        {
            DataTable dtN = new DataTable();
            try
            {
                FileStream FilStr;
                BinaryReader BinRed;
                string fpath = HttpContext.Current.Server.MapPath("~/erp_docs/logo/mlogo_" + fCocd + ".jpg");
                if (branchCode != "")
                {
                    fpath = HttpContext.Current.Server.MapPath("~/erp_docs/logo/mlogo_" + fCocd + "_" + branchCode + ".jpg");
                    if (!File.Exists(fpath)) fpath = HttpContext.Current.Server.MapPath("~/erp_docs/logo/mlogo_" + fCocd + ".jpg");
                }
                if (dataTable.Rows.Count > 0)
                {
                    if (!dataTable.Columns.Contains("mLogo")) dataTable.Columns.Add("mLogo", typeof(System.Byte[]));
                }
                dtN = dataTable.Clone();
                foreach (DataRow dr in dataTable.Rows)
                {
                    FilStr = new FileStream(fpath, FileMode.Open);
                    BinRed = new BinaryReader(FilStr);
                    dr["mLogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                    FilStr.Close();
                    BinRed.Close();
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
                dtN.TableName = dataTable.TableName.ToString();
            }
            catch
            {
                FILL_ERR("Logo File not found in erp_docs folder " + HttpContext.Current.Server.MapPath("~/erp_docs/logo/mlogo_" + fCocd + ".jpg"));
            }
            return dtN;
        }
        public string check_control(string Qstr, string pco_Cd, string control_name)
        {
            string vp = seek_iname(Qstr, pco_Cd, "Select PARAMS as vip from controlS WHERE ID='" + control_name + "' AND ENABLE_YN='Y'", "vip");
            return vp;
        }
        public string check_filed_name(string Qstr, string pco_Cd, string Table_Name, string Filed_Name)
        {
            string mhd = seek_iname(Qstr, pco_Cd, "SELECT upper(COLUMN_NAME) as COLUMN_NAME FROM USER_TAB_COLUMNS WHERE upper(TABLE_NAME)='" + Table_Name.Trim().ToUpper() + "' AND upper(COLUMN_NAME)='" + Filed_Name.Trim().ToUpper() + "'", "column_name").Trim();
            return mhd.Trim();
        }
        public string Fn_curr_dt(string Pco_Cd, string Pqstr)
        {
            string rdate = "";
            string xdate = seek_iname(Pqstr, Pco_Cd, "Select to_char(sysdate,'dd/mm/yyyy') as fstr from dual", "fstr");
            string xcdt2 = fgenMV.Fn_Get_Mvar(Pqstr, "U_CDT2");
            try
            {
                if (Convert.ToDateTime(xdate) > Convert.ToDateTime(xcdt2))
                    rdate = xcdt2;
                else
                    rdate = xdate;
            }
            catch { }
            return rdate;
        }
        public string Fn_curr_dt_time(string Qstr, string Pco_Cd)
        {
            string xdate = seek_iname(Qstr, Pco_Cd, "Select to_char(sysdate,'dd/mm/yyyy HH24:MI:SS') as fstr from dual", "fstr");
            return xdate;
        }
        public DataTable fill_icon_grid(string _co_Cd, string _tab_name, string _cond, string q_str)
        {
            if (dt_menu.Rows.Count > 0 && dt_menu.TableName == q_str) { }
            else
            {
                if (_cond.Length > 2) _cond = "where " + _cond;
                dt_menu = new DataTable();
                string qr = "";
                //string COL = "DISTINCT ID as fstr,ID,MLEVEL,TEXT,ALLOW_LEVEL,WEB_ACTION,SEARCH_KEY,SUBMENU,SUBMENUID,FORM,PARAM,CSS,BRN,PRD,VISI,MSRNO";
                string COL = "DISTINCT ID as fstr,ID,MLEVEL,TEXT,ALLOW_LEVEL,WEB_ACTION,SEARCH_KEY,SUBMENU,SUBMENUID,FORM,PARAM,CSS,BRN,PRD,VISI";

                if (_tab_name == "FIN_MRSYS")
                {
                    //qr = "select " + COL + " from FIN_MSYS where trim(id) in (select distinct trim(id) from " + _tab_name + " " + _cond + " ) and NVL(VISI,'Y')!='N' /*AND SUBSTR(ID,1,1)='F'*/ order by MSRNO,id";
                    qr = "select " + COL + " from FIN_MSYS where trim(id) in (select distinct trim(id) from " + _tab_name + " " + _cond + " ) and NVL(VISI,'Y')!='N' /*AND SUBSTR(ID,1,1)='F'*/ order by id";
                    if (fgenMV.Fn_Get_Mvar(q_str, "U_ULEVEL") == "E")
                        //qr = "select " + COL + " from FIN_MSYS where trim(id) in (select distinct id from " + _tab_name + " where trim(id) in (select distinct trim(id) from " + _tab_name + " " + _cond + " ) and NVL(VISI,'Y')!='N' /*AND SUBSTR(ID,1,1)='F'*/) order by MSRNO,id";
                        qr = "select " + COL + " from FIN_MSYS where trim(id) in (select distinct id from " + _tab_name + " where trim(id) in (select distinct trim(id) from " + _tab_name + " " + _cond + " ) and NVL(VISI,'Y')!='N' /*AND SUBSTR(ID,1,1)='F'*/) order by id";
                }
                else
                {
                    //qr = "select " + COL + " from " + _tab_name + " where trim(id) not in ('97000','97001','97010') and NVL(VISI,'Y')!='N' " + _cond.Replace("where", "") + " /*AND SUBSTR(ID,1,1)='F'*/ order by MSRNO,id";
                    qr = "select " + COL + " from " + _tab_name + " where trim(id) not in ('97000','97001','97010') and NVL(VISI,'Y')!='N' " + _cond.Replace("where", "") + " /*AND SUBSTR(ID,1,1)='F'*/ order by id";
                }

                dt_menu = getdata(q_str, _co_Cd, qr);
                dt_menu.TableName = q_str;
            }
            return dt_menu;
        }
        public void Fn_Print_Report(string pco_Cd, string Uniq_QSTR, string mbr, string query, string xml, string report)
        {
            DataSet dsPrintRpt = new DataSet();
            using (DataTable dtPrintRpt = getdata(Uniq_QSTR, pco_Cd, query))
            {
                dtPrintRpt.TableName = "Prepcur";
                dsPrintRpt.Tables.Add(dtPrintRpt);
                dsPrintRpt = Get_Type_Data(Uniq_QSTR, pco_Cd, mbr, dsPrintRpt);
                string xfilepath = HttpContext.Current.Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
                string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";

                dsPrintRpt.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
                HttpContext.Current.Session["RPTDATA"] = dsPrintRpt;
                send_cookie("RPTFILE", rptfile);
            }
            if (dsPrintRpt.Tables[0].Rows.Count > 0)
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + Uniq_QSTR + "','95%','95%','');", true);
                }
            }
            else
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','420px','420px','Tejaxo Report Viewer');", true);
                }
            }
        }
        public void Print_Report(string pco_Cd, string Uniq_QSTR, string mbr, string query, string xml, string report)
        {
            DataSet dsPrintRpt = new DataSet();
            using (DataTable dtPrintRpt = getdata(Uniq_QSTR, pco_Cd, query))
            {
                dtPrintRpt.TableName = "Prepcur";
                dsPrintRpt.Tables.Add(dtPrintRpt);
                dsPrintRpt = Get_Type_Data(Uniq_QSTR, pco_Cd, mbr, dsPrintRpt);
                string xfilepath = HttpContext.Current.Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
                string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";

                dsPrintRpt.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
                HttpContext.Current.Session["RPTDATA"] = dsPrintRpt;
                send_cookie("RPTFILE", rptfile);
            }
            if (dsPrintRpt.Tables[0].Rows.Count > 0)
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','95%','95%','Tejaxo Report Viewer');", true);
                }
            }
            else
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','420px','420px','Tejaxo Report Viewer');", true);
                }
            }
        }
        public void Print_Report_BYDS(string pco_Cd, string Uniq_QSTR, string mbr, string xml, string report, DataSet dsPrintRpt, string mTitle)
        {
            if (dsPrintRpt.Tables[0].TableName != "Prepcur") dsPrintRpt.Tables[0].TableName = "Prepcur";
            dsPrintRpt = Get_Type_Data(Uniq_QSTR, pco_Cd, mbr, dsPrintRpt);
            string xfilepath = HttpContext.Current.Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
            string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";
            dsPrintRpt.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
            HttpContext.Current.Session["RPTDATA"] = dsPrintRpt;
            send_cookie("RPTFILE", rptfile);

            if (dsPrintRpt.Tables[0].Rows.Count > 0)
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','95%','95%','" + mTitle + "');", true);
                }
            }
            else
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','420px','420px','" + mTitle + "');", true);
                }
            }
        }

        public void Print_Report_BYDS_ADVG(string pco_Cd, string Uniq_QSTR, string mbr, string xml, string report, DataSet dsPrintRpt, string mTitle)
        {
            if (dsPrintRpt.Tables[0].TableName != "Prepcur") dsPrintRpt.Tables[0].TableName = "Prepcur";
            dsPrintRpt = Get_Type_Data(Uniq_QSTR, pco_Cd, mbr, dsPrintRpt);
            string xfilepath = HttpContext.Current.Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
            string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";
            dsPrintRpt.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
            HttpContext.Current.Session["RPTDATA"] = dsPrintRpt;
            send_cookie("RPTFILE", rptfile);

            if (dsPrintRpt.Tables[0].Rows.Count > 0)
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report_ADVG.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','95%','95%','" + mTitle + "');", true);
                }
            }
            else
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report_ADVG.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','420px','420px','" + mTitle + "');", true);
                }
            }
        }
        public void Print_Report_BYDS(string pco_Cd, string Uniq_QSTR, string mbr, string xml, string report, DataSet dsPrintRpt, string mTitle, string printLogo)
        {
            if (dsPrintRpt.Tables[0].TableName != "Prepcur") dsPrintRpt.Tables[0].TableName = "Prepcur";
            if (printLogo == "Y") dsPrintRpt.Tables.Add(Get_Type_Data(Uniq_QSTR, pco_Cd, mbr, "Y"));
            else dsPrintRpt = Get_Type_Data(Uniq_QSTR, pco_Cd, mbr, dsPrintRpt);
            string xfilepath = HttpContext.Current.Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
            string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";
            dsPrintRpt.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
            HttpContext.Current.Session["RPTDATA"] = dsPrintRpt;
            send_cookie("RPTFILE", rptfile);

            if (dsPrintRpt.Tables[0].Rows.Count > 0)
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','95%','95%','" + mTitle + "');", true);
                }
            }
            else
            {
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','420px','420px','" + mTitle + "');", true);
                }
            }
        }
        /// <summary>
        /// Query should display only two columns
        /// first column will become heading
        /// second column will become value against the column one
        /// </summary>
        /// <param name="compCode">Company Code</param>
        /// <param name="Uniq_QSTR">Uniq Qstr coming from URL</param>
        /// <param name="title">Title for Chart Popup</param>
        /// <param name="graphType">ex: pie, bar, line, column</param>
        /// <param name="graphUpperHeader">Header for Graph Top</param>
        /// <param name="graphHeader">Header for Graph after Top</param>
        /// <param name="graphQuery">Graph Query, it must display only two columns</param>
        public void Fn_FillChart(string compCode, string Uniq_QSTR, string title, string graphType, string graphUpperHeader, string graphHeader, string graphQuery, string graphUnit)
        {
            string sb = Fn_FillChart(compCode, Uniq_QSTR, title, graphType, graphHeader, "", graphQuery, graphUnit, "container", "", "");
            fgenMV.Fn_Set_Mvar(Uniq_QSTR, "GraphData", sb.ToString().Trim());
            if (graphType == "funnel") Fn_Open_ChartFunnel(title, Uniq_QSTR);
            else Fn_Open_Chart(title, Uniq_QSTR);
        }
        /// <summary>
        /// Query should display only two columns
        /// first column will become heading
        /// second column will become value against the column one
        /// </summary>
        /// <param name="compCode">Company Code</param>
        /// <param name="Uniq_QSTR">Uniq Qstr coming from URL</param>
        /// <param name="title">Title for Chart Popup</param>
        /// <param name="graphType">ex: pie, bar, line, column</param>
        /// <param name="graphUpperHeader">Header for Graph Top</param>
        /// <param name="graphHeader">Header for Graph after Top</param>
        /// <param name="graphDataTable">Datatable instead of Query</param>
        public void Fn_FillChart(string compCode, string Uniq_QSTR, string title, string graphType, string graphUpperHeader, string graphHeader, DataTable graphDataTable, string graphUnit)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$(document).ready(function () {");
            sb.Append(chartColorTheme());
            sb.Append("$('#container').highcharts({");
            sb.Append("chart: {");
            sb.Append("type: '" + graphType + "'");
            sb.Append("},");
            sb.Append("title: {");
            sb.Append("text: '" + graphUpperHeader + "'");
            sb.Append("},");
            sb.Append("subtitle: {");
            sb.Append("text: '" + graphHeader + "'");
            sb.Append("},");
            sb.Append("tooltip: {");
            sb.Append("formatter: function() {");
            chartLegType = "this.x";
            if (graphType == "pie") chartLegType = "this.point.name";
            sb.Append("return " + chartLegType + " + ' ('+ this.y + ') ' + ' " + graphUnit + "';");
            sb.Append("}");
            sb.Append("},");
            sb.Append("plotOptions: {");
            sb.Append("" + graphType + " : {");
            sb.Append("dataLabels: {");
            sb.Append("enabled: true,");
            sb.Append("formatter: function () {");
            sb.Append("return " + chartLegType + " + ': (' + this.y + ') ';");
            sb.Append("},");
            sb.Append("style: {");
            sb.Append("color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }");
            sb.Append("}");
            sb.Append("}");
            sb.Append("},");
            sb.Append("series: [{");
            sb.Append("data: [");

            string colData = "";
            string colHeader = "";
            DataTable grDt = new DataTable();
            grDt = graphDataTable;
            foreach (DataRow dr in grDt.Rows)
            {
                if (colData.Length > 0)
                {
                    colData = colData + ", " + "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + " } ";
                }
                else
                {
                    colData = "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + " } ";
                }
                if (colHeader.Length > 0)
                {
                    colHeader = colHeader + ", " + "'" + dr[0].ToString().Trim() + "'";
                }
                else
                {
                    colHeader = "'" + dr[0].ToString().Trim() + "'";
                }
            }

            sb.Append(colData);

            sb.Append("]");
            sb.Append("}],");
            sb.Append("xAxis: {");
            sb.Append("categories: [ " + colHeader + " ],");
            sb.Append("crosshair: true");
            sb.Append("},");
            sb.Append("});");
            sb.Append("});");
            sb.Append(@"</script>");

            fgenMV.Fn_Set_Mvar(Uniq_QSTR, "GraphData", sb.ToString().Trim());
            if (graphType == "funnel") Fn_Open_ChartFunnel(title, Uniq_QSTR);
            else Fn_Open_Chart(title, Uniq_QSTR);
        }
        /// <summary>
        /// Query should display only two columns
        /// first column will become heading
        /// second column will become value against the column one
        /// will return chart string
        /// </summary>
        /// <param name="compCode">Company Code</param>
        /// <param name="Uniq_QSTR">Uniq Qstr coming from URL</param>
        /// <param name="title">Title for Chart Popup</param>
        /// <param name="graphType">ex: pie, bar, line, column</param>
        /// <param name="graphUpperHeader">Header for Graph Top</param>
        /// <param name="graphHeader">Header for Graph after Top</param>
        /// <param name="graphQuery">Graph Query, it must display only two columns</param>
        public string Fn_FillChart(string compCode, string Uniq_QSTR, string title, string graphType, string graphUpperHeader, string graphHeader, string graphQuery, string graphUnit, string graphDiv, string bottomTitle, string leftTitle)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            DataTable grDt = new DataTable();
            grDt = getdata(Uniq_QSTR, compCode, graphQuery);
            if (grDt == null) return "";
            if (grDt.Rows.Count > 0)
            {
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$(document).ready(function () {");
                sb.Append(chartColorTheme());
                sb.Append("$('#" + graphDiv + "').highcharts({");
                sb.Append("chart: {");
                sb.Append("type: '" + graphType + "'");
                sb.Append("},");
                sb.Append("title: {");
                sb.Append("text: '" + graphUpperHeader + "'");
                sb.Append("},");
                sb.Append("subtitle: {");
                sb.Append("text: '" + graphHeader + "'");
                sb.Append("},");
                if (graphType != "funnel")
                {
                    sb.Append("tooltip: {");
                    sb.Append("formatter: function() {");
                    chartLegType = "this.x";
                    if (graphType == "pie") chartLegType = "this.point.name";
                    sb.Append("return " + chartLegType + " + ' ('+ this.y + ') ' + ' " + graphUnit + "';");
                    sb.Append("}");
                    sb.Append("},");
                    sb.Append("plotOptions: {");
                    sb.Append("" + graphType + " : {");
                    sb.Append("dataLabels: {");
                    sb.Append("enabled: true,");
                    sb.Append("formatter: function () {");
                    sb.Append("return " + chartLegType + " + ': (' + this.y + ') ';");
                    sb.Append("},");
                    sb.Append("style: {");
                    sb.Append("color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }");
                    sb.Append("}");
                    sb.Append("}");
                    sb.Append("},");
                }
                if (graphType == "funnel")
                {
                    sb.Append("plotOptions: {series: {");
                    sb.Append("center: ['50%', '50%'],");
                    sb.Append("neckWidth: '30%',");
                    sb.Append("neckHeight: '25%',");
                    sb.Append("width: '60%'");
                    sb.Append("}},");
                }
                string colData = "";
                string colHeader = "";
                string pieRmk = ",sliced: true, selected: true";
                if (graphType != "pie") pieRmk = "";

                if (grDt.Columns.Count <= 2)
                {
                    sb.Append("series: [{");
                    sb.Append("name : '" + bottomTitle + "', ");
                    sb.Append("data: [");

                    foreach (DataRow dr in grDt.Rows)
                    {
                        if (colData.Length > 0)
                        {
                            colData = colData + ", " + "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + " } ";
                        }
                        else
                        {
                            colData = "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + pieRmk + " } ";
                        }

                        if (colHeader.Length > 0)
                        {
                            colHeader = colHeader + ", " + "'" + dr[0].ToString().Trim() + "'";
                        }
                        else
                        {
                            colHeader = "'" + dr[0].ToString().Trim() + "'";
                        }
                    }

                    sb.Append(colData);

                    sb.Append("]");
                    sb.Append("}],");
                }
                else
                {
                    sb.Append("series: [");
                    colData = "";
                    for (int i = 0; i < grDt.Rows.Count; i++)
                    {
                        mq0 = "";
                        for (int j = 0; j < grDt.Columns.Count; j++)
                        {
                            if (j > 0)
                            {
                                if (graphType == "funnel")
                                {
                                    if (mq0.Length > 0) mq0 = mq0 + "," + "['" + grDt.Columns[j].ColumnName + " (" + make_double(grDt.Rows[i][j].ToString()).ToString() + ")'," + make_double(grDt.Rows[i][j].ToString()).ToString() + "]";
                                    else mq0 = "['" + grDt.Columns[j].ColumnName + " (" + make_double(grDt.Rows[i][j].ToString()).ToString() + ") '," + make_double(grDt.Rows[i][j].ToString()).ToString() + "]";
                                }
                                else
                                {
                                    if (mq0.Length > 0) mq0 = mq0 + "," + make_double(grDt.Rows[i][j].ToString()).ToString();
                                    else mq0 = make_double(grDt.Rows[i][j].ToString()).ToString();
                                }
                            }
                        }
                        if (colData.Length > 0)
                        {
                            colData = colData + ", " + "{ name : '" + grDt.Rows[i][0].ToString().Trim() + "', data : [" + mq0 + "] } ";
                        }
                        else
                        {
                            colData = " { name : '" + grDt.Rows[i][0].ToString().Trim() + "', data : [" + mq0 + "] } ";
                        }
                    }
                    int l = 0;
                    foreach (DataColumn dc in grDt.Columns)
                    {
                        if (l > 0)
                        {
                            if (colHeader.Length > 0)
                            {
                                colHeader = colHeader + ", " + "'" + dc.ColumnName.ToString().Trim().Replace("-", "_") + "'";
                            }
                            else
                            {
                                colHeader = "'" + dc.ColumnName.ToString().Trim().Replace("-", "_") + "'";
                            }
                        }
                        l++;
                    }

                    sb.Append(colData);

                    sb.Append("],");
                }

                sb.Append("xAxis: {");
                sb.Append("categories: [ " + colHeader + " ],");
                sb.Append("crosshair: true");
                sb.Append("},");

                sb.Append("yAxis: {");
                sb.Append("title : { text : '" + leftTitle + "' } , min : 0");
                sb.Append("}, ");

                sb.Append("});");
                sb.Append("});");
                sb.Append(@"</script>");
            }
            return sb.ToString().Trim();
        }
        /// <summary>
        /// Query should display only two columns
        /// first column will become heading
        /// second column will become value against the column one
        /// /// will return chart string
        /// </summary>
        /// <param name="compCode">Company Code</param>
        /// <param name="Uniq_QSTR">Uniq Qstr coming from URL</param>
        /// <param name="title">Title for Chart Popup</param>
        /// <param name="graphType">ex: pie, bar, line, column</param>
        /// <param name="graphUpperHeader">Header for Graph Top</param>
        /// <param name="graphHeader">Header for Graph after Top</param>
        /// <param name="graphDataTable">Datatable instead of Query</param>
        public string Fn_FillChart(string compCode, string Uniq_QSTR, string title, string graphType, string graphUpperHeader, string graphHeader, DataTable graphDataTable, string graphUnit, string graphDiv)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$(document).ready(function () {");
            sb.Append(chartColorTheme());
            sb.Append("$('#" + graphDiv + "').highcharts({");
            sb.Append("chart: {");
            sb.Append("type: '" + graphType + "'");
            sb.Append("},");
            sb.Append("title: {");
            sb.Append("text: '" + graphUpperHeader + "'");
            sb.Append("},");
            sb.Append("subtitle: {");
            sb.Append("text: '" + graphHeader + "'");
            sb.Append("},");
            sb.Append("tooltip: {");
            sb.Append("formatter: function() {");
            chartLegType = "this.x";
            if (graphType == "pie") chartLegType = "this.point.name";
            sb.Append("return " + chartLegType + " + ' ('+ this.y + ') ' + ' " + graphUnit + "';");
            sb.Append("}");
            sb.Append("},");
            sb.Append("plotOptions: {");
            sb.Append("" + graphType + " : {");
            sb.Append("dataLabels: {");
            sb.Append("enabled: true,");

            //sb.Append("align: 'left',");
            //sb.Append("rotation: 270,");
            //sb.Append("x: 2,");
            //sb.Append("y: -10,");

            sb.Append("formatter: function () {");
            sb.Append("return " + chartLegType + " + ': (' + this.y + ') ';");
            sb.Append("},");
            sb.Append("style: {");
            sb.Append("color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }");
            sb.Append("}");
            sb.Append("}");
            sb.Append("},");
            sb.Append("series: [{");
            sb.Append("data: [");

            string colData = "";
            string colHeader = "";
            DataTable grDt = new DataTable();
            grDt = graphDataTable;
            foreach (DataRow dr in grDt.Rows)
            {
                if (colData.Length > 0)
                {
                    colData = colData + ", " + "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + " } ";
                }
                else
                {
                    colData = "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + " } ";
                }
                if (colHeader.Length > 0)
                {
                    colHeader = colHeader + ", " + "'" + dr[0].ToString().Trim() + "'";
                }
                else
                {
                    colHeader = "'" + dr[0].ToString().Trim() + "'";
                }
            }

            sb.Append(colData);

            sb.Append("]");
            sb.Append("}],");
            sb.Append("xAxis: {");
            sb.Append("categories: [ " + colHeader + " ],");
            sb.Append("crosshair: true");
            sb.Append("},");
            sb.Append("});");
            sb.Append("});");
            sb.Append(@"</script>");

            return sb.ToString().Trim();
        }
        public string Fn_FillChart(string compCode, string Uniq_QSTR, string title, string graphType, string graphUpperHeader, string graphHeader, DataTable graphDataTable, string graphUnit, string graphDiv, int maxWidth)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$(document).ready(function () {");
            sb.Append(chartColorTheme());
            sb.Append("$('#" + graphDiv + "').highcharts({");
            sb.Append("chart: {");
            sb.Append("type: '" + graphType + "'");
            sb.Append("},");
            sb.Append("title: {");
            sb.Append("text: '" + graphUpperHeader + "'");
            sb.Append("},");
            sb.Append("subtitle: {");
            sb.Append("text: '" + graphHeader + "'");
            sb.Append("},");
            sb.Append("tooltip: {");
            sb.Append("formatter: function() {");
            chartLegType = "this.x";
            if (graphType == "pie") chartLegType = "this.point.name";
            sb.Append("return " + chartLegType + " + ' ('+ this.y + ') ' + ' " + graphUnit + "';");
            sb.Append("}");
            sb.Append("},");
            sb.Append("plotOptions: {");
            sb.Append("" + graphType + " : {");
            sb.Append("dataLabels: {");
            sb.Append("enabled: true,");
            sb.Append("formatter: function () {");
            sb.Append("return " + chartLegType + " + ': (' + this.y + ') ';");
            sb.Append("},");
            sb.Append("style: {");
            sb.Append("color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }");
            sb.Append("}");
            sb.Append("}");
            sb.Append("},");
            sb.Append("series: [{");
            sb.Append("data: [");

            string colData = "";
            string colHeader = "";
            DataTable grDt = new DataTable();
            grDt = graphDataTable;
            foreach (DataRow dr in grDt.Rows)
            {
                if (colData.Length > 0)
                {
                    colData = colData + ", " + "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + " } ";
                }
                else
                {
                    colData = "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + " } ";
                }
                if (colHeader.Length > 0)
                {
                    colHeader = colHeader + ", " + "'" + dr[0].ToString().Trim() + "'";
                }
                else
                {
                    colHeader = "'" + dr[0].ToString().Trim() + "'";
                }
            }

            sb.Append(colData);

            sb.Append("]");
            sb.Append("}],");
            sb.Append("xAxis: {");
            sb.Append("categories: [ " + colHeader + " ],");
            sb.Append("crosshair: true");
            sb.Append("},");
            sb.Append("});");

            sb.Append("responsive: {");
            sb.Append("rules: [{");
            sb.Append("condition: {");
            sb.Append("maxWidth: " + maxWidth + "");
            sb.Append("},");
            sb.Append("chartOptions: {");
            sb.Append("legend: {");
            sb.Append("layout: 'horizontal',");
            sb.Append("align: 'center',");
            sb.Append("verticalAlign: 'bottom'");
            sb.Append("}");
            sb.Append("}");
            sb.Append("}]");
            sb.Append("}");

            sb.Append("});");
            sb.Append(@"</script>");

            return sb.ToString().Trim();
        }
        public string track_save(string compCode, string Uniq_QSTR, string Action, string Type, string Uname, string Pwd, string nPwd)
        {
            try
            {
                string vnum = next_no(Uniq_QSTR, compCode, "select max(vchnum) as vchnum from log_track where type='" + Type + "'", 6, "vchnum");
                string terminal = seek_iname(Uniq_QSTR, compCode, "select userenv('terminal')||' ,'||sysdate||' '||to_char(sysdate,'HH:MI:SS PM') as cSource from dual", "cSource");
                using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, "log_track"))
                {
                    DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                    fgen_oporow["BRANCHCD"] = "00";
                    fgen_oporow["TYPE"] = Type;
                    fgen_oporow["VCHNUM"] = vnum;
                    fgen_oporow["VCHDATE"] = DateTime.Now.ToString("dd/MM/yyyy");
                    fgen_oporow["FCOMMENT"] = Action;
                    fgen_oporow["ENT_BY"] = Uname;
                    fgen_oporow["ENT_DT"] = System.DateTime.Now;
                    fgen_oporow["OPASS"] = Pwd;
                    fgen_oporow["NPASS"] = nPwd;
                    mq0 = GetIpAddress().ToString().ToUpper() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                    if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);
                    fgen_oporow["terminal"] = mq0 + " " + terminal;
                    fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                    save_data(Uniq_QSTR, compCode, fgen_oDS, "log_track");
                }
            }
            catch (Exception ex) { FILL_ERR("In Log Track Saving :=> " + ex.Message.ToString().Trim()); }
            return "";
        }
        public string Dsk_Tile_save(string compCode, string Uniq_QSTR, string mobj_subject, string mobj_name, string mobj_sql)
        {
            string alr_avail = "";
            string mbr = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_MBR");
            alr_avail = seek_iname(Uniq_QSTR, compCode, "SELECT count(*)as cntr FROM dsk_config WHERE branchcd='" + mbr + "' and trim(upper(obj_name))=trim(upper('" + mobj_name + "'))", "cntr");

            if (make_double(alr_avail) > 0)
            { }
            else
            {
                try
                {

                    string vnum = next_no(Uniq_QSTR, compCode, "select max(vchnum) as vchnum from dsk_config where BRANCHCD='" + mbr + "' and type='80'", 6, "vchnum");
                    using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, "dsk_Config"))
                    {
                        DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                        fgen_oporow["BRANCHCD"] = mbr;
                        fgen_oporow["TYPE"] = "80";
                        fgen_oporow["VCHNUM"] = vnum;
                        fgen_oporow["VCHDATE"] = DateTime.Now.ToString("dd/MM/yyyy");
                        fgen_oporow["srno"] = 1;
                        fgen_oporow["frm_name"] = "F05101";
                        fgen_oporow["frm_title"] = mobj_subject;
                        fgen_oporow["obj_name"] = mobj_name;
                        fgen_oporow["obj_Caption"] = "lbl";
                        fgen_oporow["obj_visible"] = "Y";
                        fgen_oporow["obj_width"] = 0;
                        fgen_oporow["col_no"] = 0;

                        fgen_oporow["ENT_ID"] = "FIN";
                        fgen_oporow["ENT_BY"] = "FIN";
                        fgen_oporow["ENT_DT"] = System.DateTime.Now;

                        fgen_oporow["EdT_BY"] = "-";
                        fgen_oporow["EdT_DT"] = System.DateTime.Now;

                        fgen_oporow["frm_header"] = "-";
                        fgen_oporow["obj_maxlen"] = 50;
                        fgen_oporow["obj_readonly"] = "N";
                        fgen_oporow["obj_sql"] = mobj_sql;
                        fgen_oporow["obj_sql2"] = "N";

                        fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                        save_data(Uniq_QSTR, compCode, fgen_oDS, "dsk_Config");
                    }
                }
                catch (Exception ex) { FILL_ERR("In Dsk Config Saving :=> " + ex.Message.ToString().Trim()); }
            }
            return "";
        }

        public void vSave(string Uniq_QSTR, string compCode, string branchcd, string voucherType, string voucherNo, DateTime voucherDt, int voucherSrno,
        string vAcode, string vRcode, double dramt, double cramt, string voucherInvno, DateTime voucherInvDate, string voucherNaration, double voucherFcrate, double voucherFcrate1,
        double voucherTfcr, double voucherTfcdr, double voucherTfccr, string voucherRefnum, DateTime voucherRefdt, string voucherEntBy, DateTime voucherEntDt, string voucherTax, double voucherStax, double qty, string gstVch_no, string tbl_name)
        {
            using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, tbl_name))
            {
                DataRow oporow = fgen_oDS.Tables[0].NewRow();
                oporow["branchcd"] = branchcd;
                oporow["DEPCD"] = branchcd;

                oporow["type"] = voucherType;
                oporow["vchnum"] = voucherNo;
                oporow["vchdate"] = voucherDt;
                oporow["srno"] = voucherSrno;
                oporow["acode"] = vAcode;
                oporow["rcode"] = vRcode;
                oporow["dramt"] = dramt;
                oporow["cramt"] = cramt;

                if (voucherType == "58" || voucherType == "59")
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;

                    // for long voucher no.
                    string lvchn_5859_en = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859");
                    string lvchn_5859_dt = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859_date");
                    string CDT1_5859 = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_CDT1").Right(2);
                    if (lvchn_5859_en == "Y" && IsDate(lvchn_5859_dt))
                    {
                        if (Convert.ToDateTime(lvchn_5859_dt) <= voucherDt)
                        {
                            oporow["invno"] = CDT1_5859 + branchcd + voucherType + "-" + voucherNo;
                        }
                        else
                        {
                            oporow["invno"] = branchcd + voucherType + voucherNo;
                        }
                    }
                    else
                    {
                        oporow["invno"] = branchcd + voucherType + voucherNo;
                    }

                    oporow["invdate"] = voucherDt;
                }
                else
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;

                    oporow["invno"] = voucherInvno;
                    oporow["invdate"] = voucherInvDate;
                }

                oporow["naration"] = voucherNaration;
                oporow["fcrate"] = voucherFcrate;
                oporow["fcrate1"] = voucherFcrate;
                oporow["tfcr"] = voucherTfcr;
                oporow["tfcdr"] = voucherTfcdr;
                oporow["tfccr"] = voucherTfccr;
                oporow["refnum"] = voucherRefnum;
                oporow["refdate"] = voucherRefdt;
                oporow["st_entform"] = "-";
                oporow["quantity"] = qty;

                oporow["tax"] = voucherTax;
                oporow["stax"] = voucherStax;

                oporow["ent_by"] = voucherEntBy;
                oporow["ent_date"] = voucherEntDt;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = voucherEntDt;
                oporow["GSTVCH_NO"] = gstVch_no;
                fgen_oDS.Tables[0].Rows.Add(oporow);
                save_data(Uniq_QSTR, compCode, fgen_oDS, tbl_name);
            }
        }

        public void vSave(string Uniq_QSTR, string compCode, string branchcd, string voucherType, string voucherNo, DateTime voucherDt, int voucherSrno,
        string vAcode, string vRcode, double dramt, double cramt, string voucherInvno, DateTime voucherInvDate, string voucherNaration, double voucherFcrate, double voucherFcrate1,
        double voucherTfcr, double voucherTfcdr, double voucherTfccr, string voucherRefnum, DateTime voucherRefdt, string voucherEntBy, DateTime voucherEntDt, string voucherTax, double voucherStax, double qty, string gstVch_no, string app_by, DateTime app_dt, string vari_vch, string tbl_name)
        {
            using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, tbl_name))
            {
                DataRow oporow = fgen_oDS.Tables[0].NewRow();
                oporow["branchcd"] = branchcd;
                oporow["DEPCD"] = branchcd;

                oporow["type"] = voucherType;
                oporow["vchnum"] = voucherNo;
                oporow["vchdate"] = voucherDt;
                oporow["srno"] = voucherSrno;
                oporow["acode"] = vAcode;
                oporow["rcode"] = vRcode;
                oporow["dramt"] = dramt;
                oporow["cramt"] = cramt;

                if (voucherType == "58" || voucherType == "59")
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;

                    // for long voucher no.
                    string lvchn_5859_en = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859");
                    string lvchn_5859_dt = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859_date");
                    string CDT1_5859 = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_CDT1").Right(2);
                    if (lvchn_5859_en == "Y" && IsDate(lvchn_5859_dt))
                    {
                        if (Convert.ToDateTime(lvchn_5859_dt) <= voucherDt)
                        {
                            oporow["invno"] = CDT1_5859 + branchcd + voucherType + "-" + voucherNo;
                        }
                        else
                        {
                            oporow["invno"] = branchcd + voucherType + voucherNo;
                        }
                    }
                    else
                    {
                        oporow["invno"] = branchcd + voucherType + voucherNo;
                    }

                    oporow["invdate"] = voucherDt;
                }
                else
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;

                    oporow["invno"] = voucherInvno;
                    oporow["invdate"] = voucherInvDate;
                }

                oporow["naration"] = voucherNaration;
                oporow["fcrate"] = voucherFcrate;
                oporow["fcrate1"] = voucherFcrate;
                oporow["tfcr"] = voucherTfcr;
                oporow["tfcdr"] = voucherTfcdr;
                oporow["tfccr"] = voucherTfccr;
                oporow["refnum"] = voucherRefnum;
                oporow["refdate"] = voucherRefdt;
                oporow["st_entform"] = "-";
                oporow["quantity"] = qty;

                oporow["tax"] = voucherTax;
                oporow["stax"] = voucherStax;

                oporow["ent_by"] = voucherEntBy;
                oporow["ent_date"] = voucherEntDt;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = voucherEntDt;

                oporow["app_by"] = app_by;
                oporow["app_date"] = app_dt;

                if (vari_vch == "Y") oporow["pflag"] = "V";
                else oporow["pflag"] = "-";

                oporow["GSTVCH_NO"] = gstVch_no;
                fgen_oDS.Tables[0].Rows.Add(oporow);
                save_data(Uniq_QSTR, compCode, fgen_oDS, tbl_name);
            }
        }

        public void vSave(string Uniq_QSTR, string compCode, string branchcd, string voucherType, string voucherNo, DateTime voucherDt, int voucherSrno,
        string vAcode, string vRcode, double dramt, double cramt, string voucherInvno, DateTime voucherInvDate, string voucherNaration, double voucherFcrate, double voucherFcrate1,
        double voucherTfcr, double voucherTfcdr, double voucherTfccr, string voucherRefnum, DateTime voucherRefdt, string voucherEntBy, DateTime voucherEntDt, string voucherTax, double voucherStax, double qty, string gstVch_no, string app_by, DateTime app_dt, string vari_vch, string tbl_name, string depCd)
        {
            using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, tbl_name))
            {
                DataRow oporow = fgen_oDS.Tables[0].NewRow();
                oporow["branchcd"] = branchcd;
                oporow["DEPCD"] = depCd;

                oporow["type"] = voucherType;
                oporow["vchnum"] = voucherNo;
                oporow["vchdate"] = voucherDt;
                oporow["srno"] = voucherSrno;
                oporow["acode"] = vAcode;
                oporow["rcode"] = vRcode;
                oporow["dramt"] = dramt;
                oporow["cramt"] = cramt;

                if (voucherType == "58" || voucherType == "59")
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;
                    // for long voucher no.
                    string lvchn_5859_en = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859");
                    string lvchn_5859_dt = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859_date");
                    string CDT1_5859 = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_CDT1").Right(2);
                    if (lvchn_5859_en == "Y" && IsDate(lvchn_5859_dt))
                    {
                        if (Convert.ToDateTime(lvchn_5859_dt) <= voucherDt)
                        {
                            oporow["invno"] = CDT1_5859 + branchcd + voucherType + "-" + voucherNo;
                        }
                        else
                        {
                            oporow["invno"] = branchcd + voucherType + voucherNo;
                        }
                    }
                    else
                    {
                        oporow["invno"] = branchcd + voucherType + voucherNo;
                    }

                    oporow["invdate"] = voucherDt;

                    if (compCode == "ADMC")
                    {
                        oporow["invno"] = voucherInvno;
                        oporow["invdate"] = voucherInvDate;
                    }
                }
                else
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;

                    oporow["invno"] = voucherInvno;
                    oporow["invdate"] = voucherInvDate;
                }

                oporow["naration"] = voucherNaration;
                oporow["fcrate"] = voucherFcrate;
                oporow["fcrate1"] = voucherFcrate;
                oporow["tfcr"] = voucherTfcr;
                oporow["tfcdr"] = voucherTfcdr;
                oporow["tfccr"] = voucherTfccr;
                oporow["refnum"] = voucherRefnum;
                oporow["refdate"] = voucherRefdt;
                oporow["st_entform"] = "-";
                oporow["quantity"] = qty;

                oporow["tax"] = voucherTax;
                oporow["stax"] = voucherStax;

                oporow["ent_by"] = voucherEntBy;
                oporow["ent_date"] = voucherEntDt;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = voucherEntDt;

                oporow["app_by"] = app_by;
                oporow["app_date"] = app_dt;

                if (vari_vch == "Y") oporow["pflag"] = "V";
                else oporow["pflag"] = "-";

                oporow["GSTVCH_NO"] = gstVch_no;
                fgen_oDS.Tables[0].Rows.Add(oporow);
                save_data(Uniq_QSTR, compCode, fgen_oDS, tbl_name);
            }
        }

        public void vSave(string Uniq_QSTR, string compCode, string branchcd, string voucherType, string voucherNo, DateTime voucherDt, int voucherSrno,
        string vAcode, string vRcode, double dramt, double cramt, string voucherInvno, DateTime voucherInvDate, string voucherNaration, double voucherFcrate, double voucherFcrate1,
        double voucherTfcr, double voucherTfcdr, double voucherTfccr, string voucherRefnum, DateTime voucherRefdt, string voucherEntBy, DateTime voucherEntDt, string voucherTax, double voucherStax, double qty, string gstVch_no, string app_by, DateTime app_dt, string vari_vch, string tbl_name, string depCd, string origInvno, DateTime origInvdate)
        {
            using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, tbl_name))
            {
                DataRow oporow = fgen_oDS.Tables[0].NewRow();
                oporow["branchcd"] = branchcd;
                oporow["DEPCD"] = depCd;

                oporow["type"] = voucherType;
                oporow["vchnum"] = voucherNo;
                oporow["vchdate"] = voucherDt;
                oporow["srno"] = voucherSrno;
                oporow["acode"] = vAcode;
                oporow["rcode"] = vRcode;
                oporow["dramt"] = dramt;
                oporow["cramt"] = cramt;

                if (voucherType == "58" || voucherType == "59")
                {
                    oporow["originv_no"] = origInvno;
                    oporow["originv_dt"] = origInvdate;
                    // for long voucher no.
                    string lvchn_5859_en = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859");
                    string lvchn_5859_dt = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859_date");
                    string CDT1_5859 = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_CDT1").Right(2);
                    if (lvchn_5859_en == "Y" && IsDate(lvchn_5859_dt))
                    {
                        if (Convert.ToDateTime(lvchn_5859_dt) <= voucherDt)
                        {
                            oporow["invno"] = CDT1_5859 + branchcd + voucherType + "-" + voucherNo;
                        }
                        else
                        {
                            oporow["invno"] = branchcd + voucherType + voucherNo;
                        }
                    }
                    else
                    {
                        oporow["invno"] = branchcd + voucherType + voucherNo;
                    }

                    oporow["invdate"] = voucherDt;

                    if (compCode == "ADMC")
                    {
                        oporow["invno"] = voucherInvno;
                        oporow["invdate"] = voucherInvDate;
                    }
                }
                else
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;

                    oporow["invno"] = voucherInvno;
                    oporow["invdate"] = voucherInvDate;
                }
                oporow["naration"] = voucherNaration;
                oporow["fcrate"] = voucherFcrate;
                oporow["fcrate1"] = voucherFcrate;
                oporow["tfcr"] = voucherTfcr;
                oporow["tfcdr"] = voucherTfcdr;
                oporow["tfccr"] = voucherTfccr;
                oporow["refnum"] = voucherRefnum;
                oporow["refdate"] = voucherRefdt;
                oporow["st_entform"] = "-";
                oporow["quantity"] = qty;

                oporow["tax"] = voucherTax;
                oporow["stax"] = voucherStax;

                oporow["ent_by"] = voucherEntBy;
                oporow["ent_date"] = voucherEntDt;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = voucherEntDt;

                oporow["app_by"] = app_by;
                oporow["app_date"] = app_dt;

                if (vari_vch == "Y") oporow["pflag"] = "V";
                else oporow["pflag"] = "-";

                oporow["GSTVCH_NO"] = gstVch_no;
                fgen_oDS.Tables[0].Rows.Add(oporow);
                save_data(Uniq_QSTR, compCode, fgen_oDS, tbl_name);
            }
        }

        public void vSavePV(string Uniq_QSTR, string compCode, string branchcd, string voucherType, string voucherNo, DateTime voucherDt, int voucherSrno,
        string vAcode, string vRcode, double dramt, double cramt, string voucherInvno, DateTime voucherInvDate, string voucherNaration, double voucherFcrate, double voucherFcrate1,
        double voucherTfcr, double voucherTfcdr, double voucherTfccr, string voucherRefnum, DateTime voucherRefdt, string voucherEntBy, DateTime voucherEntDt, string voucherTax, double voucherStax, double qty, string gstVch_no, string app_by, DateTime app_dt, string vari_vch, string tbl_name, string depCd, string mrnnum, string mrndate, string holdYN)
        {
            using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, tbl_name))
            {
                DataRow oporow = fgen_oDS.Tables[0].NewRow();
                oporow["branchcd"] = branchcd;
                oporow["DEPCD"] = depCd;

                oporow["type"] = voucherType;
                oporow["vchnum"] = voucherNo;
                oporow["vchdate"] = voucherDt;
                oporow["srno"] = voucherSrno;
                oporow["acode"] = vAcode;
                oporow["rcode"] = vRcode;
                oporow["dramt"] = dramt;
                oporow["cramt"] = cramt;

                if (voucherType == "58" || voucherType == "59")
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;
                    // for long voucher no.
                    string lvchn_5859_en = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859");
                    string lvchn_5859_dt = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_lvch_5859_date");
                    string CDT1_5859 = fgenMV.Fn_Get_Mvar(Uniq_QSTR, "U_CDT1").Right(2);
                    if (lvchn_5859_en == "Y" && IsDate(lvchn_5859_dt))
                    {
                        if (Convert.ToDateTime(lvchn_5859_dt) <= voucherDt)
                        {
                            oporow["invno"] = CDT1_5859 + branchcd + voucherType + "-" + voucherNo;
                        }
                        else
                        {
                            oporow["invno"] = branchcd + voucherType + voucherNo;
                        }
                    }
                    else
                    {
                        oporow["invno"] = branchcd + voucherType + voucherNo;
                    }

                    oporow["invdate"] = voucherDt;

                    if (compCode == "ADMC")
                    {
                        oporow["invno"] = voucherInvno;
                        oporow["invdate"] = voucherInvDate;
                    }
                }
                else
                {
                    oporow["originv_no"] = voucherInvno;
                    oporow["originv_dt"] = voucherInvDate;

                    oporow["invno"] = voucherInvno;
                    oporow["invdate"] = voucherInvDate;
                }

                oporow["naration"] = voucherNaration;
                oporow["fcrate"] = voucherFcrate;
                oporow["fcrate1"] = voucherFcrate;

                oporow["tfcr"] = voucherTfcr;
                oporow["tfcdr"] = voucherTfcdr;
                oporow["tfccr"] = voucherTfccr;

                oporow["refnum"] = voucherRefnum;
                oporow["refdate"] = voucherRefdt;
                oporow["st_entform"] = "-";
                oporow["quantity"] = qty;

                oporow["tax"] = voucherTax;
                oporow["stax"] = voucherStax;

                oporow["ent_by"] = voucherEntBy;
                oporow["ent_date"] = voucherEntDt;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = voucherEntDt;

                oporow["app_by"] = app_by;
                oporow["app_date"] = app_dt;

                oporow["mrnnum"] = mrnnum;
                oporow["mrndate"] = mrndate;

                if (vari_vch == "Y") oporow["pflag"] = "V";
                else oporow["pflag"] = "-";

                oporow["GSTVCH_NO"] = gstVch_no;

                if (fgenMV.Fn_Get_Mvar(Uniq_QSTR, "REQ_APP") == "0" || fgenMV.Fn_Get_Mvar(Uniq_QSTR, "REQ_APP") == "")
                {
                    fgenMV.Fn_Set_Mvar(Uniq_QSTR, "REQ_APP", getOption(Uniq_QSTR, compCode, "W0096", "OPT_ENABLE"));
                }
                if (fgenMV.Fn_Get_Mvar(Uniq_QSTR, "REQ_APP") == "Y" && voucherType.Left(1) == "5")
                {
                    oporow["dramt"] = 0;
                    oporow["cramt"] = 0;

                    oporow["tfcdr"] = dramt;
                    oporow["tfccr"] = cramt;
                }

                oporow["RG23NO"] = holdYN;

                fgen_oDS.Tables[0].Rows.Add(oporow);
                save_data(Uniq_QSTR, compCode, fgen_oDS, tbl_name);
            }
        }
        /// <summary>
        /// opening,Rcpt,Issued,Closing_Stk,IMIN,IMAX,IORD,ALLFLD for all stock fields combine with ~
        /// </summary>
        /// <param name="co_cd"></param>
        /// <param name="mbr"></param>
        /// <param name="icode"></param>
        /// <param name="consolidate"></param>
        /// <param name="value">opening,Rcpt,Issued,Closing_Stk,IMIN,IMAX,IORD,ALLFLD for all stock fields combine with ~ </param>
        /// <returns></returns>
        public string seek_istock(string frmQstr, string co_cd, string mbr, string icode, string stockDate, bool consolidate, string valuetoShow, string condition)
        {

            string CDT1 = fgenMV.Fn_Get_Mvar(frmQstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(frmQstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT1 + "','dd/mm/yyyy')-1";

            string fromdt = fgenMV.Fn_Get_Mvar(frmQstr, "U_MDT1");
            if (fromdt == "0") fromdt = DateTime.Now.ToString("dd/MM/yyyy");
            string year = fgenMV.Fn_Get_Mvar(frmQstr, "U_YEAR");
            if (stockDate == "") stockDate = CDT2;

            string xprdrange = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + stockDate + "','dd/mm/yyyy')";
            string cond = " AND trim(icode)='" + icode + "'";
            string branch_Cd = "BRANCHCD='" + mbr + "'";
            if (consolidate) branch_Cd = "BRANCHCD not in ('DD','88')";

            string SQuery = "Select " + valuetoShow + " as retvalue from (" +
                "select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (" +
                "Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " " + cond + " " +
                "union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' " + cond + " " + condition + " GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";
            mq0 = seek_iname(frmQstr, co_cd, SQuery, "retvalue");
            return mq0;
        }
        public string seek_istock(string frmQstr, string co_cd, string mbr, string icode, string stockDate, bool consolidate, string valuetoShow, string condition, string store)
        {

            string CDT1 = fgenMV.Fn_Get_Mvar(frmQstr, "U_CDT1");
            string CDT2 = fgenMV.Fn_Get_Mvar(frmQstr, "U_CDT2");
            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT1 + "','dd/mm/yyyy')-1";

            string fromdt = fgenMV.Fn_Get_Mvar(frmQstr, "U_MDT1");
            if (fromdt == "0") fromdt = DateTime.Now.ToString("dd/MM/yyyy");
            string year = fgenMV.Fn_Get_Mvar(frmQstr, "U_YEAR");
            if (stockDate == "") stockDate = CDT2;

            string xprdrange = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + stockDate + "','dd/mm/yyyy')";
            string cond = " AND trim(icode)='" + icode + "'";
            string branch_Cd = "BRANCHCD='" + mbr + "'";
            if (consolidate) branch_Cd = "BRANCHCD not in ('DD','88')";

            string SQuery = "Select " + valuetoShow + " as retvalue from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " " + cond + " union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='" + store + "' " + cond + " " + condition + " GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";
            if (store != "Y") SQuery = "Select " + valuetoShow + " as retvalue from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (select branchcd,trim(icode) as icode,0 as opening,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as imin , 0 as imax,0 as iord from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='" + store + "' " + cond + " " + condition + " GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";
            mq0 = seek_iname(frmQstr, co_cd, SQuery, "retvalue");
            return mq0;
        }
        public string makeRepQuery(string frm_qstr, string co_cd, string formName, string branchCD, string vty, string prdRange)
        {
            string retQuery = "";
            string tbl_flds = seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + formName + "' and srno=0", "fstr");
            string datefld = "";
            string sortfld = "";
            string joinfld = "", table1 = "", table2 = "", table3 = "", table4 = "", rep_flds = "";
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                joinfld = tbl_flds.Split('@')[2].ToString();

                table1 = tbl_flds.Split('@')[3].ToString();
                table2 = tbl_flds.Split('@')[4].ToString();
                table3 = tbl_flds.Split('@')[5].ToString();
                table4 = tbl_flds.Split('@')[6].ToString();
                if (table4 == "0" || table4 == "-")
                    table4 = "-";

                sortfld = sortfld.Replace("`", "'");
                joinfld = joinfld.Replace("`", "'");
                rep_flds = seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + formName + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }

            if (vty.Length > 1) vty = "and " + vty;
            if (prdRange.Trim().Length > 2)
            {
                retQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " " + (table4 == "-" ? "" : ", " + table4) + " where a." + branchCD + " " + vty + " and " + datefld + " " + prdRange + (joinfld.Length > 1 ? "  and " + joinfld : " ") + (sortfld.Length > 1 ? " order by " + sortfld + " " : "");
            }
            else
            {
                retQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " " + (table4 == "-" ? "" : ", " + table4) + " where a." + branchCD + " " + vty + (joinfld.Length > 1 ? "  and " + joinfld : " ") + (sortfld.Length > 1 ? " order by " + sortfld + " " : "");
            }


            return retQuery;
        }
        public string makeRepQuery(string frm_qstr, string co_cd, string formName, string branchCD, string vty, string prdRange, string extraCond)
        {
            string retQuery = "";
            string tbl_flds = seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + formName + "' and srno=0", "fstr");
            string datefld = "";
            string sortfld = "";
            string joinfld = "", table1 = "", table2 = "", table3 = "", table4 = "", rep_flds = "";
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                joinfld = tbl_flds.Split('@')[2].ToString();

                table1 = tbl_flds.Split('@')[3].ToString();
                table2 = tbl_flds.Split('@')[4].ToString();
                table3 = tbl_flds.Split('@')[5].ToString();
                table4 = tbl_flds.Split('@')[6].ToString();

                sortfld = sortfld.Replace("`", "'");
                joinfld = joinfld.Replace("`", "'");
                rep_flds = seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + formName + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }
            if (vty.Length > 1) vty = "and " + vty;
            retQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branchCD + " " + vty + " and " + datefld + " " + prdRange + " and " + joinfld + " and " + extraCond + " order by " + sortfld;

            return retQuery;
        }
        public bool checkDB(string _qstr, string comp)
        {
            bool chkD = false;
            if (comp == "0") comp = _qstr.Split('^')[0];
            string constr = ConnInfo.connString(comp);
            fgenCO.connStr = constr;
            try
            {
                using (OracleConnection con = new OracleConnection(constr))
                {
                    con.Open();
                    fgenMV.Fn_Set_Mvar(_qstr, "CONN", constr);
                    con.Close();

                    chkD = true;
                }
            }
            catch (Exception err)
            {
                FILL_ERR(err.Message);
                chkD = false;
            }
            return chkD;
        }
        public DataTable GetYearDetails(string _qstr, string comp, string year)
        {
            DataTable dtGetDetails = new DataTable();
            dtGetDetails = getdata(_qstr, comp, "select code,to_char(fmdate,'yyyy')||'-'||to_char(todate,'yyyy') as fstr,to_char(fmdate,'dd/mm/yyyy') as cdt1,to_char(todate,'dd/mm/yyyy') as cdt2,branch from co where trim(code)='" + comp + year + "'");
            return dtGetDetails;
        }
        public DataTable CheckUserDetails(string _qstr, string comp, string userName)
        {
            DataTable dtGetDetails = new DataTable();
            dtGetDetails = getdata(_qstr, comp, "select a.*,trim(upper(a.level3pw)) as password from evas a where TRIM(UPPER(a.USERNAME))='" + userName + "' AND TRIM(UPPER(a.USERNAME)) LIKE '" + userName + "%'");
            HttpContext.Current.Session["dtGetD"] = dtGetDetails;
            return dtGetDetails;
        }
        public bool MatchUser(string _qstr, string comp, string UserName, string UserPwd)
        {
            bool result = false;
            DataTable dtFFF = new DataTable();
            if (HttpContext.Current.Session["dtGetD"] != null) dtFFF = (DataTable)HttpContext.Current.Session["dtGetD"];
            else dtFFF = CheckUserDetails(_qstr, comp, UserName);
            if (dtFFF.Rows.Count <= 0) dtFFF = CheckUserDetails(_qstr, comp, UserName);
            if (dtFFF.Rows.Count > 0)
            {
                if (dtFFF.Rows[0]["username"].ToString().Trim() == UserName) result = true;
                else result = false;
            }
            return result;
        }
        public bool MatchPwd(string _qstr, string comp, string UserName, string UserPwd)
        {
            bool result = false;
            DataTable dtFFF = new DataTable();
            if (HttpContext.Current.Session["dtGetD"] != null) dtFFF = (DataTable)HttpContext.Current.Session["dtGetD"];
            else dtFFF = CheckUserDetails(_qstr, comp, UserName);
            if (dtFFF.Rows.Count <= 0) dtFFF = CheckUserDetails(_qstr, comp, UserName);
            if (dtFFF.Rows.Count > 0)
            {
                if (dtFFF.Rows[0]["level3pw"].ToString().ToUpper().Trim() == UserPwd.Trim()) result = true;
                else result = false;
            }
            return result;
        }
        public string GetUserValue(string _qstr, string comp, string UserName, string fieldName)
        {
            string result = "";
            DataTable dtFFF = new DataTable();
            if (HttpContext.Current.Session["dtGetD"] != null) dtFFF = (DataTable)HttpContext.Current.Session["dtGetD"];
            else dtFFF = CheckUserDetails(_qstr, comp, UserName);
            if (dtFFF.Rows.Count <= 0) dtFFF = CheckUserDetails(_qstr, comp, UserName);
            if (dtFFF.Rows.Count > 0)
            {
                result = seek_iname_dt(dtFFF, "username='" + UserName + "'", fieldName);
            }
            return result;
        }
        public bool confirmUser(DataTable MainDT, string UserName, string UserPwd)
        {
            bool result = false;
            try
            {
                if (MainDT.Rows[0]["level3pw"].ToString().ToUpper().Trim() == UserPwd)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        public void send_Activity_mail(string _qstr, string comp, string SName, string formId, string subject, string msg, string entby)
        {
            DataTable dtMailMgr = new DataTable();
            string mUsrcode = seek_iname(_qstr, comp, "select userid as cSource from evas where username='" + entby + "'", "cSource");
            string frm_mbr = fgenMV.Fn_Get_Mvar(_qstr, "U_MBR");
            dtMailMgr = getdata(_qstr, comp, "select distinct trim(ECODE) As ecode,trim(emailid) As emailid,trim(username) as username from (SELECT a.ECODE,b.emailid,b.username FROM WB_MAIL_MGR a,EVAS B WHERE TRIM(A.ECODE)=TRIM(B.USERID) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='MM' AND TRIM(a.RCODE)='" + formId + "' AND TRIM(NVL(B.EMAILID,'-'))!='-' union all SELECT b.userid as ECODE,b.emailid,b.username FROM EVAS B WHERE TRIM(b.userid)='" + mUsrcode + "' AND TRIM(NVL(B.EMAILID,'-'))!='-') ");
            foreach (DataRow dr in dtMailMgr.Rows)
            {
                send_mail(comp, SName, dr["emailid"].ToString().Trim(), "", "", subject, msg);
            }
        }
        public void send_Activity_msg(string _qstr, string comp, string formId, string subject, string entby)
        {
            DataTable dtMailMgr = new DataTable();
            dtMailMgr = getdata(_qstr, comp, "SELECT a.ECODE,b.CONTACTNO,b.username FROM WB_MAIL_MGR a,EVAS B WHERE TRIM(A.ECODE)=TRIM(B.USERID) AND A.TYPE='NF' AND TRIM(a.RCODE)='" + formId + "' AND TRIM(NVL(B.CONTACTNO,'-'))!='-' ORDER BY a.ECODE");
            foreach (DataRow dr in dtMailMgr.Rows)
            {
                send_sms(_qstr, comp, dr["CONTACTNO"].ToString().Trim(), subject, entby);
            }
        }

        public string bindautodata(string qstr, string cocd, string query)
        {
            DataTable dtBind = null;
            dtBind = getdata(qstr, cocd, query);
            StringBuilder output = new StringBuilder();
            output.Append("[");
            for (int i = 0; i < dtBind.Rows.Count; ++i)
            {
                output.Append("\"" + dtBind.Rows[i][0].ToString() + "\"");

                if (i != (dtBind.Rows.Count - 1))
                {
                    output.Append(",");
                }
            }
            output.Append("];");
            return output.ToString();
        }
        public string chk_RsysUpd(string IdNo)
        {
            string result = "0";
            result = seek_iname_dt(fgenMV.fin_rsys_upd, "ID='" + IdNo + "'", "ID");
            return result;
        }
        public string add_RsysUpd(string Qstr, string CoCD_Fgen, string IdNo, string added_by)
        {
            //to add into fin_rsys_upd and refresh memory table
            //to avoid primary key error
            string result = "0";
            execute_cmd(Qstr, CoCD_Fgen, "insert into FIN_rSYS_UPD values ('" + IdNo + "','" + added_by + "',sysdate)");
            execute_cmd(Qstr, CoCD_Fgen, "commit");

            fgenMV.fin_rsys_upd = new DataTable();
            fgenMV.fin_rsys_upd = getdata(Qstr, CoCD_Fgen, "SELECT NVL(IDNO,'-') AS ID FROM FIN_RSYS_UPD ORDER BY NVL(IDNO,'-')");

            return result;
        }
        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }
            return sOTP;
        }
        public string gen_otp(string frm_qstr, string comp_code)
        {
            string return_otp;
            //return_otp = seek_iname(frm_qstr, comp_code, "select substr(round(to_Char(sysdate +3,'dmyhhmiss')/2),1,6) as otp from dual", "otp");
            //return_otp = seek_iname(frm_qstr, comp_code, "select rpad(round(to_Char(sysdate +3,'ssmihh')/3),6,'0') as otp from dual", "otp");
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            return_otp = GenerateRandomOTP(6, saAllowedCharacters);
            return return_otp;
        }
        string chartColorTheme()
        {
            string jsResu = "";
            StringBuilder jsRb = new StringBuilder();
            jsRb.Append("Highcharts.theme = {");
            jsRb.Append("colors: ['#03AAF1', '#4CE210', '#F74500', '#FFA804', '#40D550', '#F15C00', ");
            jsRb.Append("'#9035EA', '#F7437C', '#0017FF'],");
            jsRb.Append("chart: {");
            jsRb.Append("backgroundColor: {");
            jsRb.Append("linearGradient: [0, 0, 500, 500],");
            jsRb.Append("stops: [");
            jsRb.Append("[0, 'rgb(255, 255, 255)'],");
            jsRb.Append("[1, 'rgb(240, 240, 255)']");
            jsRb.Append("]");
            jsRb.Append("},");
            jsRb.Append("},");
            jsRb.Append("title: {");
            jsRb.Append("style: {");
            jsRb.Append("color: '#000',");
            jsRb.Append("font: 'bold 16px Trebuchet MS Verdana, sans-serif'");
            jsRb.Append("}");
            jsRb.Append("},");
            jsRb.Append("subtitle: {");
            jsRb.Append("style: {");
            jsRb.Append("color: '#666666',");
            jsRb.Append("font: 'bold 12px Trebuchet MS Verdana, sans-serif'");
            jsRb.Append("}");
            jsRb.Append("},");

            jsRb.Append("legend: {");
            jsRb.Append("itemStyle: {");
            jsRb.Append("font: '9pt Trebuchet MS, Verdana, sans-serif',");
            jsRb.Append("color: 'black'");
            jsRb.Append("},");
            jsRb.Append("itemHoverStyle:{");
            jsRb.Append("color: 'gray'");
            jsRb.Append("}   ");
            jsRb.Append("}");
            jsRb.Append("};");
            jsRb.Append("Highcharts.setOptions(Highcharts.theme);");
            jsResu = jsRb.ToString();
            return jsResu;
        }

        //SELECT cols.table_name, cols.column_name, cols.position, cons.status,CONS.constraint_name, cons.owner,cons.constraint_type FROM all_constraints cons, all_cons_columns cols WHERE cols.table_name = 'ITEM' AND cons.constraint_name = cols.constraint_name AND cons.owner = cols.owner  ORDER BY cols.table_name, cols.position

        public void dPrintIE(string qstr, string comp_code, string frm_mbr, string userID, string formID, string CDT1, string fstr)
        {
            string cond = "";
            if (HttpContext.Current.Request.Url.AbsolutePath.Contains("tej-wfin")) cond = "tej-wfin/";
            string pageurl = cond + "tej-base/dprint.aspx?STR=ERP@" + DateTime.Now.ToString("dd") + "@" + comp_code + "@" + CDT1.Substring(6, 4) + frm_mbr + "@" + userID + "@BVAL@" + formID + "@" + fstr + "";
            string url = HttpContext.Current.Request.Url.Authority;
            string finalurl = "http://" + url + "//" + pageurl;

            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "window.open('" + finalurl + "','_newtab');", true);
        }
        string _dscAuthName { get; set; }
        string _dscNametoPrint { get; set; }
        string _dscPanNo { get; set; }
        public string dscAuthName(string qstr, string comp_code, string mbr, string userName)
        {
            if (_dscAuthName == "" || _dscAuthName == null)
            {
                _dscAuthName = seek_iname(qstr, comp_code, "SELECT DSC_DESG AS FULL_NAME FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "' ", "FULL_NAME");
            }
            return _dscAuthName;
        }
        public string dscNametoPrint(string qstr, string comp_code, string mbr, string userName)
        {
            if (_dscNametoPrint == "" || _dscNametoPrint == null)
            {
                if (getOption(qstr, comp_code, "W0057", "OPT_ENABLE") == "Y") _dscNametoPrint = seek_iname(qstr, comp_code, "SELECT (CASE WHEN TRIM(NVL(FULL_NAME,'0'))='0' THEN USERNAME ELSE FULL_NAME END) AS FULL_NAME FROM EVAS WHERE TRIM(UPPER(USERNAME))='" + userName + "' ", "FULL_NAME");
                else _dscNametoPrint = seek_iname(qstr, comp_code, "SELECT DSC_NAME AS FULL_NAME FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "' ", "FULL_NAME");
            }
            return _dscNametoPrint;
        }
        public string dscPanNo(string qstr, string comp_code, string mbr, string userName)
        {
            if (_dscPanNo == "" || _dscPanNo == null)
            {
                _dscPanNo = seek_iname(qstr, comp_code, "SELECT DSC_PAN AS FULL_NAME FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "' ", "FULL_NAME");
            }
            return _dscPanNo;
        }
        public string dscDimension(string qstr, string comp_code, string mbr, string ReportAction)
        {
            return seek_iname(qstr, comp_code, "SELECT NUM4||'~'||NUM5 AS FSTR FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + ReportAction + "'", "FSTR");
        }

        public void save_dsc_info(string Qstr, string pco_Cd, string mbr, string ztype, string zvnum, string zvdate, string zformID, string zfilename, string zfilepath, string zuser)
        {
            string dscRmk = "";
            using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "DSC_INFO"))
            {
                DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                fgen_oporow["BRANCHCD"] = mbr;
                fgen_oporow["TYPE"] = ztype;
                fgen_oporow["VCHNUM"] = zvnum;
                fgen_oporow["VCHDATE"] = zvdate;

                switch (zformID)
                {
                    case "F70203":
                    case "P70106D":
                    case "P70106C":
                        dscRmk = "Accounts Documented Signed by " + zuser + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        break;
                    case "F1002":
                        dscRmk = "MRR Documented Signed by " + zuser + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        break;
                    case "F1004":
                        dscRmk = "Sales Order Documented Signed by " + zuser + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        break;
                    case "F1005":
                        dscRmk = "Purchase Order Documented Signed by " + zuser + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        break;
                    case "F1006":
                        dscRmk = "Invoice Documented Signed by " + zuser + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        break;
                    case "F1007":
                        dscRmk = "Challan Documented Signed by " + zuser + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        break;
                }

                fgen_oporow["REMARKS"] = dscRmk;
                fgen_oporow["FILENAME"] = zfilename;
                fgen_oporow["FILEPATH"] = zfilepath;

                fgen_oporow["ENT_BY"] = zuser;
                fgen_oporow["ENT_DT"] = System.DateTime.Now;
                fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                save_data(Qstr, pco_Cd, fgen_oDS, "DSC_INFO");
            }
        }

        /// <summary>
        /// Query should display only two columns
        /// first column will become heading
        /// second column will become value against the column one
        /// will return chart string
        /// </summary>
        /// <param name="compCode">Company Code</param>
        /// <param name="Uniq_QSTR">Uniq Qstr coming from URL</param>
        /// <param name="title">Title for Chart Popup</param>       
        /// <param name="graphType">ex: pie, bar, line, column</param>
        /// <param name="graphUpperHeader">Header for Graph Top</param>
        /// <param name="graphHeader">Header for Graph after Top</param>
        /// <param name="graphQuery">Graph Query, it must display only two columns</param>
        /// <param name="graphQuery2nd">drill down graph query, it must show only three colm, first two colm should be string and 3 colm must be number</param>
        public string Fn_FillChartDrill(string compCode, string Uniq_QSTR, string title, string graphType, string graphUpperHeader, string graphHeader, string graphQuery, string graphQuery2nd, string graphUnit, string graphDiv, string bottomTitle, string leftTitle)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            DataTable grDt, grDt2 = new DataTable();
            grDt = getdata(Uniq_QSTR, compCode, graphQuery);
            grDt2 = getdata(Uniq_QSTR, compCode, graphQuery2nd);
            if (grDt.Rows.Count > 0)
            {
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$(document).ready(function () {");
                sb.Append(chartColorTheme());
                sb.Append("$('#" + graphDiv + "').highcharts({");
                sb.Append("chart: {");
                sb.Append("type: '" + graphType + "'");
                sb.Append("},");
                sb.Append("title: {");
                sb.Append("text: '" + graphUpperHeader + "'");
                sb.Append("},");
                sb.Append("subtitle: {");
                sb.Append("text: '" + graphHeader + "'");
                sb.Append("},");

                sb.Append("tooltip: {");
                sb.Append("formatter: function() {");
                chartLegType = "this.x";
                if (graphType == "pie") chartLegType = "this.point.name";
                sb.Append("return " + chartLegType + " + ' ('+ this.y + ') ' + ' " + graphUnit + "';");
                sb.Append("}");
                sb.Append("},");
                sb.Append("plotOptions: {");
                sb.Append("" + graphType + " : {");
                sb.Append("dataLabels: {");
                sb.Append("enabled: true,");
                sb.Append("formatter: function () {");
                sb.Append("return " + chartLegType + " + ': (' + this.y + ') ';");
                sb.Append("},");
                sb.Append("style: {");
                sb.Append("color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }");
                sb.Append("}");
                sb.Append("}");
                sb.Append("},");


                string colData = "";
                string colHeader = "";
                string pieRmk = ",sliced: true, selected: true";
                if (graphType != "pie") pieRmk = "";

                if (grDt.Columns.Count <= 2)
                {
                    sb.Append("series: [{");
                    sb.Append("name : '" + bottomTitle + "', ");
                    sb.Append("data: [");

                    foreach (DataRow dr in grDt.Rows)
                    {
                        if (colData.Length > 0)
                        {
                            colData = colData + ", " + "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + ", drilldown : '" + dr[0].ToString().Trim() + "' } ";
                        }
                        else
                        {
                            colData = "{ name : '" + dr[0].ToString().Trim() + "', y : " + make_double(dr[1].ToString().Trim()) + pieRmk + ", drilldown : '" + dr[0].ToString().Trim() + "' } ";
                        }

                        if (colHeader.Length > 0)
                        {
                            colHeader = colHeader + ", " + "'" + dr[0].ToString().Trim() + "'";
                        }
                        else
                        {
                            colHeader = "'" + dr[0].ToString().Trim() + "'";
                        }
                    }

                    sb.Append(colData);

                    sb.Append("]");
                    sb.Append("}],");
                }
                else
                {
                    sb.Append("series: [");
                    colData = "";
                    for (int i = 0; i < grDt.Rows.Count; i++)
                    {
                        mq0 = "";
                        for (int j = 0; j < grDt.Columns.Count; j++)
                        {
                            if (j > 0)
                            {
                                if (mq0.Length > 0) mq0 = mq0 + "," + make_double(grDt.Rows[i][j].ToString()).ToString();
                                else mq0 = make_double(grDt.Rows[i][j].ToString()).ToString();
                            }
                        }
                        if (colData.Length > 0)
                        {
                            colData = colData + ", " + "{ name : '" + grDt.Rows[i][0].ToString().Trim() + "', data : [" + mq0 + "] } ";
                        }
                        else
                        {
                            colData = " { name : '" + grDt.Rows[i][0].ToString().Trim() + "', data : [" + mq0 + "] } ";
                        }
                    }
                    int l = 0;
                    foreach (DataColumn dc in grDt.Columns)
                    {
                        if (l > 0)
                        {
                            if (colHeader.Length > 0)
                            {
                                colHeader = colHeader + ", " + "'" + dc.ColumnName.ToString().Trim().Replace("-", "_") + "'";
                            }
                            else
                            {
                                colHeader = "'" + dc.ColumnName.ToString().Trim().Replace("-", "_") + "'";
                            }
                        }
                        l++;
                    }

                    sb.Append(colData);

                    sb.Append("],");
                }

                if (grDt2.Rows.Count > 0)
                {
                    sb.Append("drilldown: {");
                    sb.Append("series: [");
                    DataTable dtdist = new DataTable();
                    DataView dv = new DataView(grDt2);
                    string drillheader = "";
                    dtdist = dv.ToTable(true, grDt2.Columns[0].ColumnName);
                    for (int i = 0; i < dtdist.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.Append("name: '" + dtdist.Rows[i][0].ToString().Trim() + "',");
                        sb.Append("id: '" + dtdist.Rows[i][0].ToString().Trim() + "',");
                        drillheader = "";
                        dv = new DataView(grDt2, grDt2.Columns[0].ColumnName + "='" + dtdist.Rows[i][0].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        for (int x = 0; x < dv.Count; x++)
                        {
                            drillheader += "," + "['" + dv[x].Row[1].ToString().Trim() + "', " + dv[x].Row[2].ToString().Trim() + "]";
                        }
                        sb.Append("data: [ " + drillheader.TrimStart(',') + " ],");
                        sb.Append("},");
                    }
                    sb.Append("]");
                    sb.Append("},");
                }

                sb.Append("xAxis: {");
                sb.Append("type: 'category'");
                sb.Append("},");

                sb.Append("yAxis: {");
                sb.Append("title : { text : '" + leftTitle + "' } , min : 0");
                sb.Append("}, ");

                sb.Append("});");
                sb.Append("});");
                sb.Append(@"</script>");
            }

            fgenMV.Fn_Set_Mvar(Uniq_QSTR, "GraphData", sb.ToString().Trim());
            if (graphType == "funnel") Fn_Open_ChartFunnel(title, Uniq_QSTR);
            else Fn_Open_Chart(title, Uniq_QSTR);
            return sb.ToString().Trim();
        }
        public bool specialUser(string cocd, string username, string pwd)
        {
            username = username.ToUpper();
            pwd = pwd.ToUpper();
            if ((username == "tej-COMP" || username == "tej-PACK" || username == "tej-INDIA" || username == "tej-PLAST" || username == "tej-FORG" || username == "tej-LABEL" || username == "tej-PHARMA")
                && pwd == "SAZ" + DateTime.Now.ToString("yyMM"))
                return true;
            else return false;
        }

        public void calc_cogs(string frm_qstr, string frm_cocd, string frm_mbr, string xcode, string xvchdt, string vchStr)
        {
            double op_Qty, op_val;
            double rc_Qty, rc_val;
            double is_Qty, is_val;
            double rt_Qty, rt_val;
            double cp_Qty, cp_val;
            double cl_Qty, cl_val;

            DataTable rssample1X = new DataTable();
            DataTable rssampleX = new DataTable();

            string s_code1 = "00000000";
            string s_code2 = "49999999";
            if (xcode != "")
            {
                if (xcode.Contains("~"))
                {
                    s_code1 = xcode.Split('~')[0];
                    s_code2 = xcode.Split('~')[1];
                }
                else
                {
                    s_code1 = xcode;
                    s_code2 = xcode;
                }
            }
            if (xcode == "~")
            {
                s_code1 = "00000000";
                s_code2 = "49999999";
            }

            string frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
            string frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
            string frm_myear = frm_CDT1.Right(4);

            string inventStartDt = "01/01/2020";
            string col3 = seek_iname(frm_qstr, frm_cocd, "SELECT INVN_STDT FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "INVN_STDT");
            if (CheckIsDate(col3)) inventStartDt = col3;

            string xprd1 = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + frm_CDT1 + "','dd/mm/yyyy')-1";
            string xprd2 = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + frm_CDT1 + "','dd/mm/yyyy')-1";
            string xprd3 = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + xvchdt + "','dd/mm/yyyy')";

            string MQ0 = "select '--' as fstr1,b.BRANCHCD||trim(a.icode) as fstr,to_DatE('" + inventStartDt + "','dd/mm/yyyy') as Vchdate,'00' as type,'00' as vchnum,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as iqtyin,0 as iqtyout,b.irate as ichgs,0 as ipack,'1' AS SRNO from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + frm_mbr + "' and trim(icode) between '" + s_code1 + "' and '" + s_code2 + "' union all  ";
            string MQ1 = "select icode,(iqtyin)-(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and store='Y' and trim(icode) between '" + s_code1 + "' and '" + s_code2 + "'  union all ";
            string MQ2 = "select icode,0 as op,(iqtyin) as cdr,(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and store='Y' and trim(icode) between '" + s_code1 + "' and '" + s_code2 + "'  ) a,item b where trim(A.icode)=trim(B.icode) group by b.branchcd||trim(a.icode),b.irate having sum(a.opening)+sum(a.cdr)-sum(a.ccr)<>0 ";
            string seekSql = MQ0 + MQ1 + MQ2;

            string PrdRange = " between to_date('" + inventStartDt + "','dd/mm/yyyy') and to_Date('" + xvchdt + "','dd/mm/yyyy')";

            seekSql = "select  * from (select * from (" + seekSql + ") union all select a.branchcd||to_char(A.vchdate,'YYYYMMDD')||a.type||a.vchnum||trim(a.icode)||TO_CHAR(A.IQTYIN+A.IQTYOUT,'9999999999.999')||a.srno as fstr1,a.BRANCHCD||trim(a.icode) as fstr,a.vchdate,a.type,a.vchnum,a.iqtyin,a.iqtyout,a.ichgs+nvl(a.rlprc,0) as ichgs,a.ipack,A.SRNO from ivoucher a where a.BRANCHCD='" + frm_mbr + "' and a.type like '%' and a.vchdate " + PrdRange + " and trim(A.icode) between '" + s_code1 + "' and '" + s_code2 + "' and a.store='Y' ) ORDER BY FSTR,vchdate,type,vchnum";
            rssampleX = getdata(frm_qstr, frm_cocd, seekSql);

            seekSql = "select a.branchcd||to_char(A.vchdate,'YYYYMMDD')||a.type||a.vchnum||trim(a.icode)||TO_CHAR(A.IQTYIN+A.IQTYOUT,'9999999999.999')||trim(a.srno) as fstr1,a.BRANCHCD||trim(a.icode) as fstr,a.vchdate,a.type,a.vchnum,a.iqtyin,a.iqtyout,a.ichgs,a.irate,A.SRNO,a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'yyyymmdd')||trim(a.icode) as xxxx from ivoucher a where a.BRANCHCD='" + frm_mbr + "' and (substr(a.type,1,1) in ('1','3','2','4')) and a.vchdate " + PrdRange + " and trim(A.icode) between '" + s_code1 + "' and '" + s_code2 + "' and a.store='Y' " + (vchStr == "" ? "" : "and a.branchcd||a.type||Trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + vchStr + "'") + " ORDER BY a.branchcd||to_char(A.vchdate,'YYYYMMDD')||a.type||a.vchnum||trim(a.icode)";
            //
            DataSet dsUpdt = new DataSet();
            string constr = fgenMV.Fn_Get_Mvar(frm_qstr, "CONN");
            if (constr == "0") { constr = ConnInfo.connString(frm_cocd); }

            if (!constr.ToUpper().Contains("USER ID")) { constr = ConnInfo.connString(frm_cocd); }
            if (!constr.ToUpper().Contains("PASSWORD")) { constr = ConnInfo.connString(frm_cocd); }

            OracleConnection fcon = new OracleConnection(constr);
            OracleDataAdapter adp = new OracleDataAdapter(seekSql, fcon);
            //adp.SelectCommand = new OracleCommand(seekSql, fcon);
            adp.Fill(dsUpdt, "mytb");
            adp.TableMappings.Add("IVOUCHER", "mytb");
            rssample1X = dsUpdt.Tables[0];
            OracleCommandBuilder cb = new OracleCommandBuilder(adp);

            double curr_wac;
            double curr_waval;
            double MY_CNTR;
            double MY_LASTRT;
            string mcode = "";

            if (rssampleX == null) return;


            DataTable dtistRows = new DataTable();
            DataView disr = new DataView(rssampleX);
            dtistRows = disr.ToTable(true, "fstr");

            foreach (DataRow rssampleRow in dtistRows.Rows)
            {
                MY_LASTRT = 0; op_Qty = 0; op_val = 0; rc_Qty = 0; rc_val = 0; is_Qty = 0; is_val = 0; rt_Qty = 0; rt_val = 0; cp_Qty = 0; cp_val = 0; cl_Qty = 0; cl_val = 0; curr_wac = 0; curr_waval = 0;
                mcode = rssampleRow["fstr"].ToString().Trim();

                DataView rssample = new DataView(rssampleX, "FSTR='" + mcode + "'", "", DataViewRowState.CurrentRows);
                for (int r = 0; r < rssample.Count; r++)
                {
                    if (rssample[r].Row["VCHNUM"].ToString().Trim() == "00")
                    {
                        op_Qty = rssample[r].Row["iqtyin"].ToString().toDouble();
                        op_val = Math.Round(rssample[r].Row["iqtyin"].ToString().toDouble() * rssample[r].Row["ichgs"].ToString().toDouble(), 10);
                        cl_Qty = rssample[r].Row["iqtyin"].ToString().toDouble();
                        cl_val = Math.Round(rssample[r].Row["iqtyin"].ToString().toDouble() * rssample[r].Row["ichgs"].ToString().toDouble(), 10);
                        MY_LASTRT = Math.Round(cl_val / cl_Qty, 10);
                        curr_waval = curr_waval + op_val;
                    }
                    else
                    {
                        if (MY_LASTRT == 0) MY_LASTRT = seek_iname(frm_qstr, frm_cocd, "select irate from item where trim(icodE)='" + mcode.Right(8) + "'", "irate").ToString().toDouble();

                        if (rssample[r].Row["Type"].ToString().Left(1) == "0")
                        {
                            if (rssample[r].Row["VCHNUM"].ToString() == "000181")
                            {

                            }

                            rc_Qty = rc_Qty + rssample[r].Row["iqtyin"].ToString().toDouble();
                            rc_val = rc_val + (rssample[r].Row["iqtyin"].ToString().toDouble() * rssample[r].Row["ichgs"].ToString().toDouble());
                            curr_waval = curr_waval + (rssample[r].Row["iqtyin"].ToString().toDouble() * rssample[r].Row["ichgs"].ToString().toDouble());
                            MY_LASTRT = rssample[r].Row["ichgs"].ToString().toDouble();
                            cl_Qty = cl_Qty + rssample[r].Row["iqtyin"].ToString().toDouble();
                        }
                        else
                        {
                            is_Qty = is_Qty + rssample[r].Row["iqtyout"].ToString().toDouble() - rssample[r].Row["iqtyin"].ToString().toDouble();
                            if (cl_Qty > 0)
                            {
                                is_val = rssample[r].Row["iqtyout"].ToString().toDouble() * (curr_waval / cl_Qty);
                                rt_val = rssample[r].Row["iqtyin"].ToString().toDouble() * (curr_waval / cl_Qty);
                                MY_LASTRT = Math.Round(curr_waval / cl_Qty, 10);
                                curr_waval = curr_waval - is_val + rt_val;
                            }
                            else
                            {
                                is_val = rssample[r].Row["iqtyout"].ToString().toDouble() * MY_LASTRT;
                                rt_val = rssample[r].Row["iqtyin"].ToString().toDouble() * MY_LASTRT;
                                curr_waval = curr_waval - is_val + rt_val;
                            }

                            cl_Qty = cl_Qty - rssample[r].Row["iqtyout"].ToString().toDouble() + rssample[r].Row["iqtyin"].ToString().toDouble();
                        }
                    }
                    if (rssample1X.Rows.Count > 0)
                    {
                        int index = -1;
                        DataView dvx = new DataView(rssample1X, "FSTR1='" + rssample[r].Row["fstr1"].ToString().Trim() + "' ", "", DataViewRowState.CurrentRows);
                        if (dvx.Count > 0)
                        {
                            if (MY_LASTRT == 0) MY_LASTRT = seek_iname(frm_qstr, frm_cocd, "select irate from item where trim(icodE)='" + mcode.Right(8) + "'", "irate").ToString().toDouble();

                            if (dvx[0].Row["TYPE"].ToString().Trim().Left(1) == "4")
                                execute_cmd(frm_qstr, frm_cocd, "UPDATE IVOUCHER A SET A.RLPRC=" + MY_LASTRT + " WHERE a.branchcd||to_char(A.vchdate,'YYYYMMDD')||a.type||a.vchnum||trim(a.icode)||TO_CHAR(A.IQTYIN+A.IQTYOUT,'9999999999.999')||trim(a.srno)='" + rssample[r].Row["fstr1"].ToString().Trim() + "'");
                            else
                                execute_cmd(frm_qstr, frm_cocd, "UPDATE IVOUCHER A SET A.IRATE=" + MY_LASTRT + ", A.ICHGS=" + MY_LASTRT + " WHERE a.branchcd||to_char(A.vchdate,'YYYYMMDD')||a.type||a.vchnum||trim(a.icode)||TO_CHAR(A.IQTYIN+A.IQTYOUT,'9999999999.999')||trim(a.srno)='" + rssample[r].Row["fstr1"].ToString().Trim() + "'");
                        }
                        //DataRow[] rows = rssample1X.Select("FSTR1='" + rssample[r].Row["fstr1"].ToString().Trim() + "'");
                        //if (rows.Length > 0)
                        //{
                        //    index = rssample1X.Rows.IndexOf(rows[0]);
                        //    if (index != -1)
                        //    {
                        //        dsUpdt.Tables["mytb"].Rows[index]["ICHGS"] = MY_LASTRT;
                        //        dsUpdt.Tables["mytb"].Rows[index]["IRATE"] = MY_LASTRT;

                        //        //fcon.Open();
                        //        //adp.Update(dsUpdt, "mytb");
                        //        //fcon.Close();

                        //        OracleCommandBuilder cbx = new OracleCommandBuilder(adp);
                        //        adp.UpdateCommand = cbx.GetUpdateCommand();
                        //        adp.InsertCommand = cbx.GetInsertCommand();
                        //        adp.Update(dsUpdt);
                        //    }
                        //}
                    }
                }

                StreamWriter w = new StreamWriter(HttpContext.Current.Server.MapPath("~//tej-base//TextFile.txt"), false);
                w.WriteLine("Processed Rows : " + rssampleX.Rows.IndexOf(rssampleRow) + " out of " + rssampleX.Rows.Count);
                w.Flush();
                w.Close();
            }
        }

        public void wac_ac_vouchers(string frm_qstr, string frm_cocd, string frm_mbr, string fromDt, string toDt, string vty, string vchStr)
        {
            string frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
            string frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
            string SQuery = "";
            string cond = "", cond1 = "";
            string vchType = "", qty = "";
            string acode = "", rcode = "", rmk = "";
            if (vty.Left(1) == "3" || vty == "3C")
            {
                acode = "cons_code";
                rcode = "PURCH_CODE";
                vchType = "3C";
                rmk = "ISSUE";
                qty = "iqtyout";
                cond = "A.TYPE!='36X' AND  a.type like '3%'  and a." + qty + ">0";
            }
            if (vty.Left(1) == "1" || vty == "3R")
            {
                acode = "PURCH_CODE";
                rcode = "cons_code";
                vchType = "3R";
                rmk = "RETURN";
                qty = "iqtyin";
                cond = "a.type like '1%' and a.type<'15' AND A." + qty + ">0";
            }

            if (vchStr != "")
            {
                cond += " and a.type||'-'||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')='" + vchStr + "' ";
                cond1 = " and TRIM(INVNO)||TO_CHAR(INVDATE,'DD/MM/YYYY')='" + vchStr + "' ";
            }
            SQuery = "Select a.branchcd,a.type,a.vchnum,a.vchdate,a.ent_by,a.ent_Dt,substr(a.icode,1,4) As item_subg,sum(a." + qty + ") as qtyout,round(sum(a." + qty + "*a.irate),2) As Amt from ivoucher a, item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and " + cond + " and a.vchdate BETWEEN to_Date('" + fromDt + "','dd/mm/yyyy') and to_Date('" + toDt + "','dd/mm/yyyy') and a.store='Y' group by a.branchcd,a.type,a.ent_by,a.ent_Dt,a.vchnum,a.vchdate,substr(a.icode,1,4)";
            SQuery = "Select trim(b.purch_code) As purch_code,trim(b.cons_code) As cons_code,a.* from (" + SQuery + ") a left outer join (Select icode,nvl(no_proc,'-') as purch_code,nvl(location,'-') as cons_code from item where length(Trim(icode))=4 and trim(nvl(no_proc,'-'))!='-' and trim(nvl(location,'-'))!='-') b on trim(A.item_subg)=trim(b.icode) order by a.branchcd,a.type,a.vchdate,a.vchnum,trim(A.item_subg)";

            DataTable dtIfrs = new DataTable();
            dtIfrs = getdata(frm_qstr, frm_cocd, SQuery);

            execute_cmd(frm_qstr, frm_cocd, "DELETE FROM VOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + vchType + "' AND INVDATE BETWEEN to_Date('" + fromDt + "','dd/mm/yyyy') and to_Date('" + toDt + "','dd/mm/yyyy') " + cond1 + " ");
            string newVchnum = "";
            string frm_vty, frm_vnum = "", frm_vchdate = "", vardate = "", frm_uname = "", last_vchnum = "";
            int morder = 0;
            foreach (DataRow drIfrs in dtIfrs.Rows)
            {
                if (drIfrs["AMT"].ToString().toDouble() > 0)
                {
                    frm_vty = drIfrs["type"].ToString();
                    frm_vnum = drIfrs["vchnum"].ToString();
                    frm_vchdate = drIfrs["VCHDATE"].ToString();
                    vardate = drIfrs["ent_dt"].ToString();
                    frm_uname = drIfrs["ent_by"].ToString();
                    if (last_vchnum != frm_vnum)
                    {
                        morder = 0;
                        newVchnum = next_no(frm_qstr, frm_cocd, "SELECT MAX(VCHNUM) AS VCH FROM VOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + vchType + "' AND VCHDATE BETWEEN to_Date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + frm_CDT2 + "','dd/mm/yyyy')", 6, "VCH");
                    }
                    vSave(frm_qstr, frm_cocd, frm_mbr, vchType, newVchnum, Convert.ToDateTime(frm_vchdate), morder, drIfrs[acode].ToString().Trim(), drIfrs[rcode].ToString().Trim(), Math.Abs(make_double(drIfrs["AMT"].ToString().Trim(), 2)), 0, frm_vty + "-" + frm_vnum, Convert.ToDateTime(frm_vchdate), rmk + " (" + frm_vty + "/" + frm_vnum + " " + frm_vchdate + ")", 0, 0, 0, Math.Abs(make_double(drIfrs["AMT"].ToString().Trim(), 2)), 0, "-", Convert.ToDateTime(frm_vchdate), frm_uname, Convert.ToDateTime(vardate), "DN", 0, 0, "", "-", Convert.ToDateTime(vardate), "-", "VOUCHER", "00");
                    morder++;
                    vSave(frm_qstr, frm_cocd, frm_mbr, vchType, newVchnum, Convert.ToDateTime(frm_vchdate), morder, drIfrs[rcode].ToString().Trim(), drIfrs[acode].ToString().Trim(), 0, Math.Abs(make_double(drIfrs["AMT"].ToString().Trim(), 2)), frm_vty + "-" + frm_vnum, Convert.ToDateTime(frm_vchdate), rmk + " (" + frm_vty + "/" + frm_vnum + " " + frm_vchdate + ")", 0, 0, 0, 0, Math.Abs(make_double(drIfrs["AMT"].ToString().Trim(), 2)), "-", Convert.ToDateTime(frm_vchdate), frm_uname, Convert.ToDateTime(vardate), "CN", 0, 0, "", "-", Convert.ToDateTime(vardate), "-", "VOUCHER", "00");
                    morder++;
                }
            }
        }
    }

