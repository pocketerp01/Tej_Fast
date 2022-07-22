using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Net.Mail;
using System.Threading;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using iTextSharp.text;


public partial class Login : System.Web.UI.Page
{
    string frm_mbr = "", str = "", val1, val2, val3, firm, val4, branch_allow, byPass = "N", iconID = "";
    string value1, value2, value3, mhd, col2 = "", squery = "", qstr, uniq_id, frmUserID, frmgroup="", str1 = "";
    string ulevel; DataTable dt;
    string landingPage = "desktop";
    DataSet ds = new DataSet();
    MailMessage mail = new MailMessage();
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Tejaxo : Business Extension Technology";
        txtcompcode.Focus();
        if (!Page.IsPostBack)
        {
            fgenMV.Fn_Delete_Older_Data();
            fgenMV.FN_Delete_Older_Files();
            create_mytns();
            txtcompcode.Value = fgenCO.GetCO_CD();
            Page.Title = "ERP LOGIN PAGE";
            if (Convert.ToInt32(DateTime.Now.ToString("MM")) > 3) txtyear.Value = DateTime.Now.ToString("yyyy");
            else txtyear.Value = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 1).ToString();
            string logopath = "";
            logopath = "~/tej-base/images/f_logo.jpg";
            imglogo.Src = logopath;
            //send_mail();
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //#region direct open from Finsys
            //if (url.Contains("STR"))
            //{
            //    if (Request.QueryString["STR"].Length > 0)
            //    {
            //        str = Request.QueryString["STR"].Trim().ToString();
            //        branch_allow = "";
            //        val1 = str.Split('@')[2];
            //        val2 = str.Split('@')[3];
            //        val3 = str.Split('@')[4];
            //        if (val2.Trim().Length == 6)
            //        {
            //            branch_allow = val2.Substring(4, 2);
            //            val2 = val2.Substring(0, 4);
            //        }
            //        try
            //        {
            //            iconID = str.Split('@')[6];
            //            value2 = str.Split('@')[7];
            //        }
            //        catch { }
            //    }
            //    byPass = "Y";
            //    if (str.Contains("C_S_R"))
            //    {
            //        login_fun("TEST", val2, "SUPPORT", val1, branch_allow);
            //    }
            //    else login_fun(val1, val2, val3, "", branch_allow);
            //}
            //else byPass = "N";
            //fgen.send_cookie("MPRN", "N");
            //#endregion

            lbldttime.Text = "26/02/2022 17:00";
        }
    }
    public void login_fun(string co_str, string year_str, string user_str, string pwd_str, string branch_str)
    {
        //try
        string ind_Ptype = "";
        {
            txtcompcode.Value = txtcompcode.Value.ToUpper();

            uniq_id = co_str + "^" + Guid.NewGuid().ToString("N").Substring(0, 20) + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
            uniq_id = uniq_id.ToUpper();

            //if (fgen.checkActivation() == false)
            //{
            //    fgenMV.Fn_Set_Mvar(uniq_id, "U_COCD", co_str);
            //    fgen.ActiveBox("Activate Your System", uniq_id);
            //    return;
            //}

            #region Copydata

            
            bool check = false;
            try
            {
                //check = Convert.ToBoolean(Convert.ToInt16(ConnInfo.GETBOOL(5)));
            }
            catch { }
            if (check)
            {
                string Originalcode = ConnInfo.GETBOOL(6);/* "MAST";*/
                string Targetcode = ConnInfo.GETBOOL(7);/* "DURG";*/
                string Scode = "PD" + Originalcode;
                string Tcode = "PD" + Targetcode;
                DataTable dts = fgen.getdata(uniq_id, Targetcode, " SELECT table_name, owner from all_tables where owner='" + Scode + "'");

                foreach (DataRow dr in dts.Rows)
                {

                    string tname = dr["table_name"].ToString().Trim().ToUpper();
                    string seekv = fgen.seek_iname(uniq_id, Targetcode, "SELECT table_name from all_tables where owner='" + Tcode + "' " +
                        "and upper(table_name)='" + tname + "' ", "table_name");
                    if (tname == "ITEM")
                    { 
                    }
                    if (tname == seekv)
                    {

                        var coltab = fgen.getdata(uniq_id, Originalcode, "select column_name,data_type,(CASE WHEN DATA_TYPE='NUMBER' " +
                                   "THEN DATA_PRECISION||','||data_scale ELSE DATA_LENGTH||'' END) AS DLENGTH from user_tab_columns where " +
                                   "TABLE_NAME = '" + tname + "'");
                        DataTable dttempD = fgen.fill_schema(uniq_id, Targetcode, tname).Tables[0];

                        foreach (DataRow drc in coltab.Rows)
                        {
                            var tcol = drc["column_name"].ToString().Trim().ToUpper();
                            var ttype = drc["data_type"].ToString().Trim().ToUpper();
                            var tlen = drc["DLENGTH"].ToString().Trim().ToUpper();

                            if (!dttempD.Columns.Contains(tcol))
                            {
                                fgen.execute_cmd(uniq_id, Targetcode, "alter table " + tname + " add " + tcol + " " + ttype + " (" + tlen + ")");
                            }
                        }
                    }
                    else
                    {
                        fgen.execute_cmd(uniq_id, co_str, "CREATE TABLE " + tname + "  AS SELECT * FROM " + Scode + "." + tname + " WHERE 1=2");

                    }
                }
            }
            #endregion


            if (fgen.checkDB(uniq_id, co_str) == true)
            {
                //fgenMV.Fn_Set_Mvar(uniq_id, "U_EXETIME", lbldttime.Text.Trim());
                //fgenMV.Fn_Set_Mvar(uniq_id, "U_HELPLINE", "ERP © 1992-2021 | Helpline # +91- (8 Lines)");
                if (co_str.Trim().Length > 0)
                {
                    Session["dt_menu" + qstr] = null;
                    Session["mymst"] = null;
                    firm = fgenCO.chk_co(co_str);
                    if (firm == "XXXX") fgen.msg("-", "AMSG", "Invalid Company Code");
                    else
                    {
                        fgenCO.chk_grp(co_str, out frmgroup);
                        if (frmgroup == "0") fgen.msg("-", "AMSG", "Invalid Company Group");
                        fgenMV.Fn_Set_Mvar(uniq_id, "U_COGRP", frmgroup);

                        fgenMV.Fn_Set_Mvar(uniq_id, "U_DPRINT", "N");
                        if (ConnInfo.dtOnly == "Y") fgenMV.Fn_Set_Mvar(uniq_id, "U_DTON", "Y");
                        else fgenMV.Fn_Set_Mvar(uniq_id, "U_DTON", "N");

                        dt = new DataTable();
                        dt = fgen.GetYearDetails(uniq_id, co_str, year_str);
                        if (dt.Rows.Count <= 0) fgen.msg("-", "AMSG", "Not a Valid Year");
                        else
                        {
                            branch_allow = dt.Rows[0]["branch"].ToString().Trim();

                            fgenMV.Fn_Set_Mvar(uniq_id, "U_COCD", co_str);
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_YEAR", year_str);
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_FYEAR", dt.Rows[0]["fstr"].ToString().Trim());
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_DATERANGE", " between to_date('" + dt.Rows[0]["cdt1"].ToString().Trim() + "','dd/mm/yyyy') and to_date('" + dt.Rows[0]["cdt2"].ToString().Trim() + "','dd/mm/yyyy')");
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_CDT1", dt.Rows[0]["cdt1"].ToString().Trim());
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_CDT2", dt.Rows[0]["cdt2"].ToString().Trim());
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_EXETIME", lbldttime.Text);
                            fgenMV.Fn_Set_Mvar(uniq_id, "C_S_R", "-");
                            fgenMV.Fn_Set_Mvar(uniq_id, "FS_LOG", "Y"); // Added for Tiles Dashboard Thread Working 09/04/2020 -- VV
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_SERVERIP", fgenCO.GetServerIP());

                            //
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_Q_COUNTER", "1");
                            //                            

                            col2 = dt.Rows[0]["fstr"].ToString().Trim();

                            DataTable dtUserFromMaster = new DataTable();
                            if (fgen.MatchUser(uniq_id, co_str, user_str, pwd_str) == false)
                            {
                                dtUserFromMaster = fgen.getdata(uniq_id, co_str, "select aname,acode,'M' as ulevel,branchcd as allowbr,acode AS USERID,trim(upper(weblogin)) AS LEVEL3PW,email as emailid from famst where upper(TRIM(acode))='" + user_str + "'");
                                ulevel = "M";
                            }
                            if (byPass == "Y")
                            {
                                fgenMV.Fn_Set_Mvar(uniq_id, "C_S_R", pwd_str);
                                if (co_str == "TEST" && user_str == "SUPPORT") squery = "SELECT USERNAME FROM EVAS WHERE USERNAME='" + user_str + "'";
                                else squery = "SELECT USERNAME FROM EVAS WHERE USERID='" + user_str + "'";
                                user_str = fgen.seek_iname(uniq_id, co_str, squery, "USERNAME");
                                pwd_str = fgen.seek_iname(uniq_id, co_str, "SELECT trim(upper(LEVEL3PW)) as level3pw FROM EVAS WHERE USERNAME='" + user_str + "'", "level3pw");
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_ICONID", iconID);
                                fgenMV.Fn_Set_Mvar(uniq_id, "REPID", iconID);
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_COL1", value2);
                                if (pwd_str.Length > 1) ulevel = "";
                            }

                            if (fgen.MatchUser(uniq_id, co_str, user_str, pwd_str) == false && (dtUserFromMaster == null || dtUserFromMaster.Rows.Count <= 0))
                            { fgen.msg("-", "AMSG", "This Username does not exist"); return; }
                            else
                            {
                                if (fgen.MatchPwd(uniq_id, co_str, user_str, pwd_str) == false)
                                {
                                    if (dtUserFromMaster.Rows.Count > 1)
                                    {
                                        dt = new DataTable();
                                        dt = fgen.getdata(uniq_id, co_str, "select aname,acode,'M' as ulevel,branchcd as allowbr,acode AS USERID,NULL AS Deptt,email as emailid from famst where TRIM(acode)='" + user_str + "' and trim(upper(weblogin))='" + pwd_str + "'");
                                        squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode,GST_No from type where id='B' order by type1";
                                    }
                                    else if (byPass == "Y")
                                    {
                                        fgenMV.Fn_Set_Mvar(uniq_id, "C_S_R", pwd_str);
                                        pwd_str = fgen.seek_iname(uniq_id, co_str, "SELECT trim(upper(LEVEL3PW)) as level3pw FROM EVAS WHERE USERNAME='" + user_str + "'", "level3pw");
                                        fgenMV.Fn_Set_Mvar(uniq_id, "U_ICONID", iconID);
                                    }
                                }
                                //*****
                                //if (co_str == "TEST**")
                                //{
                                //    byPass = "Y";
                                //    iconID = "F60146";
                                //    if (fgen.GetUserValue(uniq_id, co_str, user_str, "deptt") == "10 : DEVELOPMENT") iconID = "F60151";
                                //    //if (dt.Rows[0]["Deptt"].ToString().Trim().ToUpper() == "10 : DEVELOPMENT") iconID = "F60151";
                                //}
                                mhd = "0";
                                mhd = fgen.MatchPwd(uniq_id, co_str, user_str, pwd_str) == true ? "1" : "0";
                                if (mhd == "0" && (dtUserFromMaster == null || dtUserFromMaster.Rows.Count <= 0))
                                {
                                    if (fgen.GetUserValue(uniq_id, co_str, user_str, "password") == "####")
                                    {
                                        fgen.msg("-", "AMSG", "Dear " + user_str + ",You have done 10 unsuccessfull attempts, thats why a/c has been locked. Please contact administrator to unlock a/c!!");
                                    }
                                    else if (fgen.GetUserValue(uniq_id, co_str, user_str, "password") == "----")
                                    {
                                        fgen.msg("-", "AMSG", "Dear " + user_str + ",your a/c has been Disposed. Please contact administrator.!!");
                                    }
                                    else fgen.msg("-", "AMSG", "Wrong Password");

                                    //******* If Login Failed --> save tracking
                                    //#region Login Scurity working
                                    //if (co_str == "MSES")
                                    //{
                                    //    // Saving a Track Record if Login Failed
                                    //    //fgen.track_save(co_str, uniq_id, "LOGIN FAILED", "LF", user_str, pwd_str, "");
                                    //    // Checking total failure of the day
                                    //    mhd = fgen.seek_iname(uniq_id, co_str, "select count(*) as vchnum from log_track where (ent_By)='" + user_str + "' and type='LF' and to_char(ent_dt,'YYYYMMDD')=to_char(sysdate,'YYYYMMDD')", "vchnum");
                                    //    // if total failure is more then 9 then blocking the user.
                                    //    if (fgen.make_double(mhd) > 9)
                                    //    {
                                    //        fgen.execute_cmd(uniq_id, co_str, "UPDATE EVAS SET LEVEL3PW='####' WHERE UPPER(TRIM(USERNAME))='" + user_str + "' ");
                                    //        fgen.msg("-", "AMSG", "Dear " + user_str + ", You have done 10 unsuccessfull attempts, thats why a/c has been locked");
                                    //    }
                                    //}
                                    //#endregion
                                    //*******
                                    return;
                                }
                                else if ((dt.Rows.Count > 0))
                                {
                                    //*****
                                    if (co_str == "TEST")
                                    {
                                        byPass = "Y";
                                        iconID = "F60146";
                                        if (fgen.GetUserValue(uniq_id, co_str, user_str, "deptt").ToUpper().Trim() == "10 : DEVELOPMENT") iconID = "F60151";
                                    }
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_BYPASS", byPass);
                                    //*****
                                    mhd = fgen.seek_iname(uniq_id, co_str, "select upper(tname) as tname from tab where upper(tname)='TYPEOLD'", "tname");
                                    if (mhd == "TYPEOLD") fgen.execute_cmd(uniq_id, co_str, "RENAME TYPEOLD TO TYPE");


                                    // Fill Unique id and values
                                    ulevel = (ulevel == "" || ulevel == null) ? fgen.GetUserValue(uniq_id, co_str, user_str, "ulevel") : ulevel;
                                    frmUserID = dtUserFromMaster.Rows.Count > 0 ? user_str : fgen.GetUserValue(uniq_id, co_str, user_str, "userid");
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_UNAME", user_str);
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_USERID", frmUserID);
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_ULEVEL", ulevel);
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_PWD", txtPassword.Value.Trim());
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_DEP_NAME", fgen.GetUserValue(uniq_id, co_str, user_str, "deptt"));
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_EMAILID", (ulevel == "M" ? dtUserFromMaster.Rows[0]["emailid"].ToString().Trim() : fgen.GetUserValue(uniq_id, co_str, user_str, "emailid")));
                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_DP_IMG", fgen.GetUserValue(uniq_id, co_str, user_str, "ICONS"));

                                    fgenMV.Fn_Set_Mvar(uniq_id, "U_SYS_COM_QRY", "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND,NVL(OBJ_CAPTION_REG,'-') AS OBJ_CAPTION_REG FROM SYS_CONFIG ");

                                    get_tabname(co_str, uniq_id, user_str, ulevel);

                                    val1 = fgen.GetUserValue(uniq_id, co_str, user_str, "allowbr");
                                    if (branch_str.Length > 1) branch_allow = val1;
                                    Session.Add("pc_uname", user_str);

                                    col2 = "";
                                    //val1 = fgen.GetUserValue(uniq_id, co_str, user_str, "allowbr");
                                    if (val1 == "-" || val1 == null || val1.Length == 0)
                                    {
                                        dt = new DataTable();
                                        squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode from type where id='B' order by type1";
                                    }
                                    else
                                    {
                                        dt = new DataTable();
                                        dt = fgen.getdata(uniq_id, co_str, "select distinct type1 from type where id='B'");

                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            if (val1.Contains(dr["type1"].ToString().Trim()))
                                            {
                                                if (col2.Length > 0) col2 = col2 + "," + "'" + dr["type1"].ToString().Trim() + "'";
                                                else col2 = "'" + dr["type1"].ToString().Trim() + "'";
                                            }
                                        }
                                        dt = new DataTable();
                                        squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode from type where id='B' and type1 in (" + col2 + ") order by type1";
                                    }
                                }
                                //***************************


                                //**************************                            


                                //Variable for ClientGRP 17/8/2020
                                //**************************                     
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_CLIENT_GRP", txtcompcode.Value.ToUpper());
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_TAXVAR", "GST_NO");
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_PRINT_REG_HEADINGS", "'VAT %' as h1,' ' as h2,' ' as h3,'Tax 4' as h4,'GST No' as gstname ");
                                switch (txtcompcode.Value.ToUpper())
                                {
                                    case "SGRP":
                                    case "UATS":
                                    case "UAT2":
                                    case "AERO":
                                        fgenMV.Fn_Set_Mvar(uniq_id, "U_CLIENT_GRP", "SG_TYPE");
                                        fgenMV.Fn_Set_Mvar(uniq_id, "U_TAXVAR", "TRN_NO");
                                        fgenMV.Fn_Set_Mvar(uniq_id, "U_PRINT_REG_HEADINGS", "'VAT %' as h1,' ' as h2,' ' as h3,'Tax 4' as h4,'Tax Registration Number' as gstname ");

                                        fgenMV.Fn_Set_Mvar(uniq_id, "U_HELPLINE", "ERP © 1992-2021 | Helpline # ");
                                        break;
                                }
                                //**************************                            

                                fgenMV.Fn_Set_Mvar(uniq_id, "U_COUNTER", "1");


                                ind_Ptype = fgen.getOption(uniq_id, co_str, "W1000", "OPT_PARAM");
                                ind_Ptype = ind_Ptype.Length > 1 ? ind_Ptype : "01";
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_IND_PTYPE", ind_Ptype);


                                string mld_Ptype = fgen.getOption(uniq_id, co_str, "W0000", "OPT_PARAM2");
                                mld_Ptype = mld_Ptype.Length > 1 ? mld_Ptype : "61";
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_MLD_PTYPE", mld_Ptype);

                                if (1 == 1)
                                {
                                    // checking tables in db
                                    //fgen.chk_create_tab(uniq_id, co_str);

                                    //Dml_wfin dml_wfin = new Dml_wfin();
                                    //dml_wfin.chkTab(uniq_id, co_str);

                                    // adding icons common
                                    //Create_Icons iconCommon = new Create_Icons();
                                    //iconCommon.chk_icon(uniq_id, co_str);

                                    // adding icons co. wise
                                    //AddIcons addIcons = new AddIcons();
                                    //addIcons.add(uniq_id, co_str);

                                    // New icons co. wise
                                    //Icons_DevA icon_DevA = new Icons_DevA();
                                    //icon_DevA.add(uniq_id, co_str);
                                    // ------------------------------------------------------------------
                                }



                                if (ulevel == "M")
                                {
                                    squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode from type where id='B' and type1 ='00' order by type1";
                                    custVendPort(co_str, user_str);
                                }

                                // ------------------------------------------------------------------
                                // new updates for use in OCT,Nov,Dec 2018

                                //Upd_2018 Upd_18dml = new Upd_2018();

                                //oct2018 updates
                                //Upd_18dml.Upd_Oct(uniq_id, co_str);

                                //Nov2018 updates
                                //Upd_18dml.Upd_Nov(uniq_id, co_str);

                                //Dec2018 updates
                                //Upd_18dml.Upd_Dec(uniq_id, co_str);

                                // ------------------------------------------------------------------

                                //Apr2020 updates
                                //Upd_18dml.Upd_Apr(uniq_id, co_str);

                                dt = fgen.getdata(uniq_id, co_str, squery);
                                //if (dt.Rows.Count > 1 && branch_allow == "Y")
                                //{
                                ////    fgenMV.Fn_Set_Mvar(uniq_id, "U_XID", "DATA");
                                ////    fgenMV.Fn_Set_Mvar(uniq_id, "U_SEEKSQL", squery);
                                ////    fgen.Fn_open_sseek("Select Branch Name", uniq_id);

                                ////    uniq_id = hf_unqid.Value;
                                ////    value1 = fgenMV.Fn_Get_Mvar(uniq_id, "U_COL1");
                                ////    value2 = fgenMV.Fn_Get_Mvar(uniq_id, "U_COL2");
                                ////    value3 = fgenMV.Fn_Get_Mvar(uniq_id, "U_COL3");
                                ////    if (value1.Length > 0) { }
                                ////    else return;
                                ////    if (value1 == "0") return;

                                ////    fgenMV.Fn_Set_Mvar(uniq_id, "U_MBR", dt.Rows[0][0].ToString());
                                ////    fgenMV.Fn_Set_Mvar(uniq_id, "U_MBR_NAME", dt.Rows[0][2].ToString());
                                //    frm_mbr = dt.Rows[0][0].ToString();


                                //    ind_Ptype = fgen.getOptionPW(uniq_id, txtcompcode.Value.ToUpper().Trim(), "W1000", "OPT_PARAM", dt.Rows[0][0].ToString());
                                //    ind_Ptype = ind_Ptype.Length > 1 ? ind_Ptype : "01";
                                //    fgenMV.Fn_Set_Mvar(uniq_id, "U_IND_PTYPE", ind_Ptype);
                                //    reDirectPage(uniq_id, "");
                                //}
                                //else
                                //{
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_MBR", dt.Rows[0]["code"].ToString().Trim().ToUpper());
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_MBR_NAME", dt.Rows[0]["Branch_name"].ToString().Trim().ToUpper());
                                frm_mbr = dt.Rows[0]["code"].ToString().Trim().ToUpper();

                                ind_Ptype = fgen.getOptionPW(uniq_id, co_str, "W1000", "OPT_PARAM", dt.Rows[0]["code"].ToString().Trim().ToUpper());
                                ind_Ptype = ind_Ptype.Length > 1 ? ind_Ptype : "01";
                                fgenMV.Fn_Set_Mvar(uniq_id, "U_IND_PTYPE", ind_Ptype);
                                iconID = iconID.Length > 0 ? "@" + iconID : "";
                                reDirectPage(uniq_id, iconID);
                                //}
                            }
                        }
                    }
                }
                else { fgen.msg("-", "AMSG", "Please Enter Company Code"); }
            }
            else { fgen.msg("-", "AMSG", "Connection Error'13'Please check Company code and server IP !!"); }
            hf_unqid.Value = uniq_id;
        }

        //string name = "";
        //dt = fgen.getdata(uniq_id, co_str, "select table_name from user_tables ");
        //foreach (DataRow dr in dt.Rows)
        //{
        //    string tab = dr[0].ToString().Trim();
        //    fgen.execute_cmd(uniq_id, co_str, "update " + tab + " set ment_by='BRIJESH'");
        //    fgen.execute_cmd(uniq_id, co_str, "update " + tab + " set ent_by='BRIJESH'");

        //    fgen.execute_cmd(uniq_id, co_str, "update " + tab + " set mEDT_BY='BRIJESH' where trim(mEDT_BY)<>'-'");
        //    fgen.execute_cmd(uniq_id, co_str, "update " + tab + " set edt_by='BRIJESH' where trim(edt_by)<>'-'");

        //}

        //catch (Exception ex)
        //{
        //    fgen.msg("-", "AMSG", "Connection Error'13'Please check Company code and server IP !!");
        //    fgen.FILL_ERR("Login Time :=> " + ex.Message.ToString());
        //}
    }
    protected void btnLogin_ServerClick(object sender, EventArgs e)
    {
        if (!checkLogoFile())
        {
            //fgen.msg("-", "AMSG", "Logo File Not Found!!'13'Please check file c:/TEJ_erp/logo/mlogo_" + txtcompcode.Value.ToUpper() + ".jpg");
            fgen.send_cookie("COCD", txtcompcode.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('../tej-base/logoUpload.aspx','320px','250px','Logo File Not Found!!');", true);
            return;
        }

        if (txtcompcode.Value.Trim().ToUpper() == "AMAR")
        {
            if (DateTime.Now > Convert.ToDateTime("05/08/2018"))
            {
                string mhd = "";
                mhd = fgen.seek_iname(qstr, txtcompcode.Value.ToUpper().Trim(), "select idno from FIN_RSYS_UPD where trim(idno)='DACT101'", "idno");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(qstr, txtcompcode.Value.ToUpper().Trim(), "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DACT101') ");
                    fgen.execute_cmd(qstr, txtcompcode.Value.ToUpper().Trim(), "UPDATE FIN_MSYS SET VISI='N' WHERE FORM='fin90_w1'");
                    fgen.execute_cmd(qstr, txtcompcode.Value.ToUpper().Trim(), "UPDATE FIN_MSYS SET VISI='N' WHERE SUBMENUID='fin20_e5'");
                }
                //fgen.msg("-", "AMSG", "Sorry!!Can't Login Anymore'13'Contact to Finsys team");
                //return;
            }
        }

        //for (int i = 0; i < 10; i++)
        //{
        //    Thread.Sleep(1000);
        //    Page.ClientScript.RegisterClientScriptBlock(GetType(), "myScript" + i, "<script>alert('hello world');</script>");
        //}

        login_fun(txtcompcode.Value.Trim().ToUpper(), txtyear.Value.Trim().ToUpper(), txtusername.Value.Trim().ToUpper(), txtPassword.Value.Trim().ToUpper(), "");
        //fgen.Fn_open_prddmp1("-", uniq_id);        
        Session["dtGetD"] = null;
    }
    bool checkLogoFile()
    {
        try
        {
            if (!File.Exists(HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "LOGO\\MLOGO_" + txtcompcode.Value.Trim().ToUpper() + ".jpg")) return false;
            File.Copy(HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "LOGO\\MLOGO_" + txtcompcode.Value.Trim().ToUpper() + ".jpg", Server.MapPath("~/bg-image/logo.jpg"), true);

            if (File.Exists(HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "LOGO\\LOGO\\desktop.jpg"))
                File.Copy(HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "LOGO\\logo\\desktop.jpg", Server.MapPath("~/bg-image/desktop.jpg"), true);
        }
        catch { }
        return true;
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        uniq_id = hf_unqid.Value;
        value1 = fgenMV.Fn_Get_Mvar(uniq_id, "U_COL1");
        value2 = fgenMV.Fn_Get_Mvar(uniq_id, "U_COL2");
        value3 = fgenMV.Fn_Get_Mvar(uniq_id, "U_COL3");
        if (value1.Length > 0) { }
        else return;
        if (value1 == "0") return;

        fgenMV.Fn_Set_Mvar(uniq_id, "U_MBR", value1);
        fgenMV.Fn_Set_Mvar(uniq_id, "U_MBR_NAME", value3);
        frm_mbr = value1;

        string ind_Ptype = "";
        ind_Ptype = fgen.getOptionPW(uniq_id, txtcompcode.Value.ToUpper().Trim(), "W1000", "OPT_PARAM", value1);
        ind_Ptype = ind_Ptype.Length > 1 ? ind_Ptype : "01";
        fgenMV.Fn_Set_Mvar(uniq_id, "U_IND_PTYPE", ind_Ptype);
        reDirectPage(uniq_id, "");
    }
    protected void btnhideFS_Click(object sender, EventArgs e)
    {
        uniq_id = hf_unqid.Value;
        string _confirmation = fgenMV.Fn_Get_Mvar(uniq_id, "U_CONFIRM");
        if (_confirmation == "1")
        {
            otpConfirmation(true, uniq_id, "");
        }
    }
    void reDirectPage(string _qstr, string _iconId)
    {
        uniq_id = _qstr;

        Opts_wfin dskti = new Opts_wfin();
        dskti.Desk_Tiles(uniq_id, txtcompcode.Value.ToUpper().Trim(), fgenMV.Fn_Get_Mvar(_qstr, "U_MBR"));

        //runMyCmd(uniq_id, txtcompcode.Value.ToUpper().Trim(), value1);

        fgenMV.Fn_Set_Mvar(uniq_id, "FRMWINDOWSIZE", hfWindowSize.Value.ToString());

        string cd = txtcompcode.Value.Trim().ToUpper();
        string _mailOtp = fgen.getOption(_qstr, cd, "W0030", "OPT_ENABLE");
        string _smsOtp = fgen.getOption(_qstr, cd, "W0031", "OPT_ENABLE");
        if (_mailOtp == "" || _mailOtp == "N" || _mailOtp == "0")
            _mailOtp = fgenMV.Fn_Get_Mvar(uniq_id, "U_DTON") == "Y" ? "Y" : "";
        ulevel = fgenMV.Fn_Get_Mvar(uniq_id, "U_ULEVEL");
        if (ulevel == "0")
            _mailOtp = "";
        if (cd != "LRFP" && cd != "SGRP" && cd != "UAT2" && cd != "UATS")
        {
            if (fgenMV.Fn_Get_Mvar(_qstr, "U_ULEVEL") == "M")
            {
                // in case user is customer or vendor
                _mailOtp = fgen.getOption(_qstr, cd, "W0054", "OPT_ENABLE");
                _smsOtp = fgen.getOption(_qstr, cd, "W0055", "OPT_ENABLE");
            }
        }
        //if (_mailOtp != "Y") otpConfirmation(true, uniq_id, _iconId);
        otpConfirmation(true, uniq_id, _iconId);
        //else if (_mailOtp == "Y")
        //{
        //    string _emailID = fgenMV.Fn_Get_Mvar(_qstr, "U_EMAILID");
        //    if (_emailID != null && _emailID != "0")
        //    {
        //        string _otp = fgen.genOtp(qstr, cd, 1);
        //        string mail_title = "Tejaxo";
        //        System.Text.StringBuilder msb = new System.Text.StringBuilder();
        //        msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
        //        msb.Append("Dear " + txtusername.Value.ToUpper().Trim() + ",<br/><br/>");
        //        msb.Append("Your OTP is <br/><br/>");
        //        msb.Append("" + _otp + " <br/><br/>");
        //        msb.Append("Thanks & Regards,<br>");
        //        msb.Append("For " + fgenCO.chk_co(txtcompcode.Value.Trim()) + "<br><br>");
        //        msb.Append("<h5>This Report is Auto generated from the " + mail_title + ".<br>The above details are based on the *data entered* in the ERP. </h5>");
        //        msb.Append("</body></html>");

        //        //Sending E-mail
        //        fgen.send_mail(cd, "Tejaxo", _emailID, "", "", "OTP to Login in ERP", msb.ToString());

        //        fgenMV.Fn_Set_Mvar(_qstr, "U_OTP", _otp);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('../tej-base/confirmOtp.aspx?STR=" + uniq_id + "','320px','190px','ERP');", true);
        //    }
        //    else
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + txtusername.Value.ToUpper().Trim() + ", You have not entered email id in your master details. Please fill master for recieving mail for portal login.!!");
        //    }
        //}
        //else if (_smsOtp == "Y")
        //{

        //}
    }
    void otpConfirmation(bool _result, string _qstr, string _iconId)
    {
        string co = _qstr.Split('^')[0];
        uniq_id = "STR=" + _qstr;
        landingPage = (co == "DLJM") ? "desktop" : "desktop_wt";
        landingPage = (fgenMV.Fn_Get_Mvar(_qstr, "U_ULEVEL") == "M") ? "desktop_cv" : landingPage;
        //landingPage = "desktop_vn";
        if (_result == true)
        {
            //if (HttpContext.Current.Request.Url.AbsoluteUri.ToString().Contains("STR") || byPass == "Y") ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "window.location='tej-base/" + landingPage + ".aspx?" + uniq_id + _iconId + "';", true);
            //else

            Response.Redirect("~/tej-base/desktop_wt.aspx?" + uniq_id + "&guid=" + EncryptDecrypt.Encrypt(_qstr), false);
            //Response.Redirect("~/home/dashboard?" + uniq_id + "&guid=" + EncryptDecrypt.Encrypt(_qstr), false);
        }
    }
    public void get_tabname(string co_CD, string muniq_id, string uname, string mulevel)
    {
        mhd = ""; string tab_name, cond;

        DataTable dt1 = new DataTable();
        dt1 = fgen.getdata(muniq_id, co_CD, "SELECT * FROM (Select distinct id from FIN_MRSYS where trim(upper(USERNAME))='" + uname + "' ) WHERE ROWNUM<3 ");
        if (dt1.Rows.Count > 0 && mulevel == "0") { tab_name = "FIN_MRSYS"; cond = " trim(upper(username))='" + uname + "'"; }
        else if (dt1.Rows.Count <= 0 && mulevel == "M") { tab_name = "FIN_MRSYS"; cond = " trim(upper(userid))='" + uname + "'"; }
        else if (dt1.Rows.Count <= 0 && mulevel == "0") { tab_name = "FIN_MSYS"; cond = ""; }
        else { tab_name = "FIN_MRSYS"; cond = " trim(upper(username))='" + uname + "'"; }

        fgenMV.Fn_Set_Mvar(uniq_id, "U_ICONTAB", tab_name);
        fgenMV.Fn_Set_Mvar(uniq_id, "U_ICONCOND", cond);
    }
    protected void lnkchng_ServerClick(object sender, EventArgs e)
    {
        uniq_id = Guid.NewGuid().ToString("N").Substring(0, 20) + DateTime.Now.ToString("dd-MM-yyyy_hh_mm_ss");
        if (txtusername.Value == "" || txtusername.Value == null) fgen.msg("-", "AMSG", "Please Enter User Name First");
        else
        {
            firm = fgenCO.chk_co(txtcompcode.Value.Trim().ToUpper());
            if (firm == "XXXX")
            { fgen.msg("-", "AMSG", "Invalid Company Code"); txtcompcode.Focus(); }
            else
            {
                string constr = ConnInfo.connString(txtcompcode.Value.Trim().ToUpper());
                fgenCO.connStr = ConnInfo.connString(txtcompcode.Value.Trim().ToUpper());
                fgenMV.Fn_Set_Mvar(uniq_id, "CONN", constr);
                mhd = fgen.seek_iname(uniq_id, txtcompcode.Value.Trim().ToUpper(), "select username from evas where TRIM(UPPER(USERNAME))='" + txtusername.Value.ToUpper().Trim() + "'", "username");
                if (mhd == "0")
                {
                    if (txtcompcode.Value.Trim().ToUpper() == "LIVN" || txtcompcode.Value.Trim().ToUpper() == "JSGI" || txtcompcode.Value.Trim().ToUpper() == "PRPL")
                    {
                        mhd = fgen.seek_iname(uniq_id, txtcompcode.Value.Trim().ToUpper(), "select acode from famst where TRIM(UPPER(acode))='" + txtusername.Value.ToUpper().Trim() + "'", "acode");
                        if (mhd != "0")
                        {
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_COCD", txtcompcode.Value.Trim().ToUpper());
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_UNAME", txtusername.Value.ToUpper().Trim());
                            fgenMV.Fn_Set_Mvar(uniq_id, "U_PWD", txtPassword.Value.Trim());

                            if (txtcompcode.Value.Trim().ToUpper() == "LIVN" || txtcompcode.Value.Trim().ToUpper() == "JSGI" || txtcompcode.Value.Trim().ToUpper() == "PRPL") ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('tej-base/cpwd.aspx?STR=" + uniq_id + "','350px','390px','Change Password');", true);
                            else ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('tej-base/cpwd.aspx?STR=" + uniq_id + "','350px','390px','ERP');", true);
                        }
                        else fgen.msg("-", "AMSG", "Wrong UserName or UserName is does not exist");
                    }
                    else fgen.msg("-", "AMSG", "Wrong UserName or UserName is does not exist");
                }
                else
                {
                    fgenMV.Fn_Set_Mvar(uniq_id, "U_COCD", txtcompcode.Value.Trim().ToUpper());
                    fgenMV.Fn_Set_Mvar(uniq_id, "U_UNAME", txtusername.Value.ToUpper().Trim());
                    fgenMV.Fn_Set_Mvar(uniq_id, "U_PWD", txtPassword.Value.Trim());

                    if (txtcompcode.Value.Trim().ToUpper() == "LIVN" || txtcompcode.Value.Trim().ToUpper() == "JSGI" || txtcompcode.Value.Trim().ToUpper() == "PRPL") ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('tej-base/cpwd.aspx?STR=" + uniq_id + "','350px','390px','Change Password');", true);
                    else ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('tej-base/cpwd.aspx?STR=" + uniq_id + "','350px','390px','ERP');", true);
                }
            }
        }
    }
    public void create_mytns()
    {
        string path = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
        try
        {
            if (File.Exists(path)) { }
            else
            {
                StreamWriter w = new StreamWriter(path, true);
                w.WriteLine("Company Code");
                w.WriteLine("Server IP");
                w.WriteLine("Service Name");
                w.Flush();
                w.Close();
            }
        }
        catch
        {
            StreamWriter w = new StreamWriter(path, true);
            w.WriteLine("Company Code");
            w.WriteLine("Server IP");
            w.WriteLine("Service Name");
            w.Flush();
            w.Close();
        }
        path = Server.MapPath("~/tej-base/xmlfile");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Server.MapPath("~/tej-base/barcode");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Server.MapPath("~/tej-base/upload");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Server.MapPath("~/tej-base/log_file");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Server.MapPath("~/logs");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Server.MapPath("~/temp");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Server.MapPath("~/tempgen");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = Server.MapPath("~/tempsig");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "tempsig";

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "UPLOAD";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "tiff";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "np";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "DSC_pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "PO_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "MRR_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "SO_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "SOQ_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "INV_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "CUSTOMER_INV_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "CUSTOMER_CHL_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "PARTY_ACC_PDF";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "ACC_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "CHL_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "PI_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path = HttpRuntime.AppDomainAppPath + "\\erp_docs\\" + "PL_Pdf";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }
    void custVendPort(string co_str, string user_str)
    {
        Create_Icons ICO = new Create_Icons();
        Opts_wfin addRightsIcon = new Opts_wfin();
        switch (co_str)
        {
            case "KPAC":
            case "PRPL":
            case "JRAJ":
            case "JGLO":
            case "UNIQ":
            case "JGLR":
            case "MINV":
            case "APTP":
            case "VPAC":
            case "SGRP":
            case "UATS":
            case "UAT2":
            case "UMED":
                if (user_str.Substring(0, 2) == "16")
                {
                    //addRightsIcon.Icon_Mkt_ord_for_customer(uniq_id, co_str);
                    addRightsIcon.IconCustomerRequestforCustomer(uniq_id, co_str);
                    addRightsIcon.Icon_Cust_port_new(uniq_id, co_str);
                }
                if (user_str.Substring(0, 2) == "05" || user_str.Substring(0, 2) == "06")
                {
                    addRightsIcon.Icon_Supp_port_new(uniq_id, co_str);
                }
                break;

            case "ADVG":
                ICO.add_iconRights(uniq_id, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_iconRights(uniq_id, "F79000", 2, "Customer Portal", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_iconRights(uniq_id, "F79150", 3, "Feature Reports", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e5", "fa-edit");
                ICO.add_iconRights(uniq_id, "F79155", 4, "Download Valve T.C.", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e5", "fa-edit");
                break;
            case "LRFP":
                ICO.add_iconRights(uniq_id, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_iconRights(uniq_id, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_iconRights(uniq_id, "F25118", 3, "Rejection Entry", 3, "../tej-base/om_cust_rej.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_iconRights(uniq_id, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_iconRights(uniq_id, "F25144A", 3, "Crate Register Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_iconRights(uniq_id, "F25144B", 3, "Crate Register Detail", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit");

                ICO.add_iconRights(uniq_id, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_iconRights(uniq_id, "F25144A", 3, "Crate Register Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit");

                if (user_str.Substring(0, 2) == "16")
                {
                    addRightsIcon.Icon_Mkt_ord_for_customer(uniq_id, co_str);
                    ICO.add_iconRights(uniq_id, "F47108", 4, "Target Despatch", 3, "../tej-base/om_disptgt.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                    addRightsIcon.IconCustomerRequestforCustomer(uniq_id, co_str);
                    addRightsIcon.Icon_Cust_port_new(uniq_id, co_str);
                }
                break;
        }
    }
    public string send_mail()
    {
        string s = "";
        try
        {
            dt = new DataTable();

            dt.Columns.Add("ACODE", typeof(string));

            DataRow dr;
            dr = dt.NewRow();
            dr["acode"] = "000001";
            dt.Rows.Add(dr);

            ds = new DataSet();
            ds.Tables.Add(dt);
            mail = new MailMessage();

            SendEmailInBackgroundThread(mail);
        }
        catch { }
        return s;
    }
    void SendEmail(Object mailVal)
    {
        ReportDocument rpt = new ReportDocument();
        string xfilepath = "";
        xfilepath = Server.MapPath("~/tej-base/xmlfile/testing.xml");
        string rptfile = "";
        rptfile = Server.MapPath("~/tej-base/Report/testing.rpt");

        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
                rpt.Load(rptfile);
                rpt.Refresh();
                rpt.SetDataSource(ds);

                CrystalReportViewer1.ReportSource = ds;
                CrystalReportViewer1.DataBind();
            }

        }
        catch { }
    }
    void thread2(Object mailVal)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "Alert('~/tej-base/msg.aspx','-');", false);
        }
        catch { }
    }
    void thread3(Object mailVal)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('~/tej-base/sseek.aspx','1%','1%','-');", false);
        }
        catch { }
    }
    void thread4(Object mailVal)
    {
        try
        {
            //fgen.Fn_open_mseek("-", "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('~/tej-base/mseek.aspx','1%','1%','-');", false);
        }
        catch { }
    }
    void thread5(Object mailVal)
    {
        try
        {
            //fgen.Fn_Print_Report(txtcompcode.Value, "", "00", "", "", "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('~/tej-base/frm_report.aspx','1%','1%','-');", false);
        }
        catch { }
    }
    void thread6(Object mailVal)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('~/tej-base/om_pur_req.aspx','1%','1%','-');", false);
        }
        catch { }
    }
    void thread7(Object mailVal)
    {

        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('~/tej-base/om_po_entry.aspx','1%','1%','-');", false);
        }

        catch { }
    }
    void thread8(Object mailVal)
    {

        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('~/tej-base/om_Act_itm_prd.aspx','1%','1%','-');", false);
        }

        catch { }
    }
    void SendEmailInBackgroundThread(MailMessage mailMessage)
    {
        Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);

        bgThread = new Thread(new ParameterizedThreadStart(thread2));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);

        bgThread = new Thread(new ParameterizedThreadStart(thread3));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);

        bgThread = new Thread(new ParameterizedThreadStart(thread4));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);

        bgThread = new Thread(new ParameterizedThreadStart(thread5));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);

        bgThread = new Thread(new ParameterizedThreadStart(thread6));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);

        bgThread = new Thread(new ParameterizedThreadStart(thread7));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);

        bgThread = new Thread(new ParameterizedThreadStart(thread8));
        bgThread.IsBackground = true;
        bgThread.Start(mailMessage);
    }
    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        //repDoc.Close();
        //repDoc.Dispose();
    }
    void runMyCmd(string qs, string cocd, string branchcd)
    {
        mhd = fgen.chk_RsysUpd("DKSRNO" + branchcd);
        //mhd = "0";
        if (mhd == "0" || mhd == "")
        {
            fgen.execute_cmd(qs, cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DKSRNO" + branchcd + "') ");
            #region query to Run
            string queryToRu = "UPDATE DSK_CONFIG SET SRNO=999 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' " +
                "~UPDATE DSK_CONFIG SET SRNO=1 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000029'" +
                "~UPDATE DSK_CONFIG SET SRNO=1 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000030'" +
                "~UPDATE DSK_CONFIG SET SRNO=2 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000033'" +
                "~UPDATE DSK_CONFIG SET SRNO=2 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000034'" +
                "~UPDATE DSK_CONFIG SET SRNO=3 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000035'" +
                "~UPDATE DSK_CONFIG SET SRNO=3 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000036'" +
                "~UPDATE DSK_CONFIG SET SRNO=4 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000001'" +
                "~UPDATE DSK_CONFIG SET SRNO=4 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000002'" +
                "~UPDATE DSK_CONFIG SET SRNO=5 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000003'" +
                "~UPDATE DSK_CONFIG SET SRNO=5 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000004'" +
                "~UPDATE DSK_CONFIG SET SRNO=6 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000051'" +
                "~UPDATE DSK_CONFIG SET SRNO=6 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000052'" +
                "~UPDATE DSK_CONFIG SET SRNO=7 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000079'" +
                "~UPDATE DSK_CONFIG SET SRNO=7 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000080'" +
                "~UPDATE DSK_CONFIG SET SRNO=8 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000041'" +
                "~UPDATE DSK_CONFIG SET SRNO=8 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000042'" +
                "~UPDATE DSK_CONFIG SET SRNO=9 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000025'" +
                "~UPDATE DSK_CONFIG SET SRNO=9 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000026'" +
                "~UPDATE DSK_CONFIG SET SRNO=10 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000089'" +
                "~UPDATE DSK_CONFIG SET SRNO=10 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000090'" +

                "~UPDATE DSK_CONFIG SET SRNO=11 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000031'" +
                "~UPDATE DSK_CONFIG SET SRNO=11 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000032'" +
                "~UPDATE DSK_CONFIG SET SRNO=12 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000007'" +
                "~UPDATE DSK_CONFIG SET SRNO=12 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000008'" +
                "~UPDATE DSK_CONFIG SET SRNO=13 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000005'" +
                "~UPDATE DSK_CONFIG SET SRNO=13 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000006'" +
                "~UPDATE DSK_CONFIG SET SRNO=14 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000009'" +
                "~UPDATE DSK_CONFIG SET SRNO=14 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000010'" +
                "~UPDATE DSK_CONFIG SET SRNO=15 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000015'" +
                "~UPDATE DSK_CONFIG SET SRNO=15 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000016'" +
                "~UPDATE DSK_CONFIG SET SRNO=16 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000011'" +
                "~UPDATE DSK_CONFIG SET SRNO=16 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000012'" +
                "~UPDATE DSK_CONFIG SET SRNO=17 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000037'" +
                "~UPDATE DSK_CONFIG SET SRNO=17 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000038'" +
                "~UPDATE DSK_CONFIG SET SRNO=18 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000085'" +
                "~UPDATE DSK_CONFIG SET SRNO=18 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000086'" +
                "~UPDATE DSK_CONFIG SET SRNO=19 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000087'" +
                "~UPDATE DSK_CONFIG SET SRNO=19 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000088'" +
                "~UPDATE DSK_CONFIG SET SRNO=20 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000017'" +
                "~UPDATE DSK_CONFIG SET SRNO=20 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000018'" +

                "~UPDATE DSK_CONFIG SET SRNO=21 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000055'" +
                "~UPDATE DSK_CONFIG SET SRNO=21 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000056'" +
                "~UPDATE DSK_CONFIG SET SRNO=22 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000021'" +
                "~UPDATE DSK_CONFIG SET SRNO=22 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000022'" +
                "~UPDATE DSK_CONFIG SET SRNO=23 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000057'" +
                "~UPDATE DSK_CONFIG SET SRNO=23 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000058'" +
                "~UPDATE DSK_CONFIG SET SRNO=24 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000023'" +
                "~UPDATE DSK_CONFIG SET SRNO=24 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000024'" +
                "~UPDATE DSK_CONFIG SET SRNO=25 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000019'" +
                "~UPDATE DSK_CONFIG SET SRNO=25 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000020'" +
                "~UPDATE DSK_CONFIG SET SRNO=26 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000039'" +
                "~UPDATE DSK_CONFIG SET SRNO=26 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000040'" +
                "~UPDATE DSK_CONFIG SET SRNO=27 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000043'" +
                "~UPDATE DSK_CONFIG SET SRNO=27 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000044'" +
                "~UPDATE DSK_CONFIG SET SRNO=28 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000045'" +
                "~UPDATE DSK_CONFIG SET SRNO=28 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000046'" +
                "~UPDATE DSK_CONFIG SET SRNO=29 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000047'" +
                "~UPDATE DSK_CONFIG SET SRNO=29 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000048'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.1 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000081'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.1 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000082'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.2 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000083'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.2 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000084'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.3 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001494'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.3 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001495'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.4 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001492'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.4 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001493'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.4 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000061'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.4 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000062'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.5 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000059'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.5 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000060'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.6 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001506'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.6 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001507'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.7 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001508'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.7 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='001509'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.8 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000049'" +
                "~UPDATE DSK_CONFIG SET SRNO=29.8 WHERE BRANCHCD='" + branchcd + "' AND TYPE='80' AND VCHNUM='000050'";
            #endregion
            foreach (string s in queryToRu.Split('~'))
            {
                fgen.execute_cmd(qs, cocd, s);
            }
        }
    }
    protected void lnkfpass_ServerClick(object sender, EventArgs e)
    {
        string html_body = "";
        html_body = html_body + "Dear " + txtusername.Value.ToUpper().Trim();
        html_body = html_body + "<br>Please find the password to access ERP web software.<br>";
        uniq_id = Guid.NewGuid().ToString("N").Substring(0, 20) + DateTime.Now.ToString("dd-MM-yyyy_hh_mm_ss");
        if (txtusername.Value == "" || txtusername.Value == null) fgen.msg("-", "AMSG", "Please Enter User Name First");
        else
        {
            firm = fgenCO.chk_co(txtcompcode.Value.Trim().ToUpper());
            if (firm == "XXXX")
            { fgen.msg("-", "AMSG", "Invalid Company Code"); txtcompcode.Focus(); }
            else
            {
                string constr = ConnInfo.connString(txtcompcode.Value.Trim().ToUpper());
                fgenCO.connStr = ConnInfo.connString(txtcompcode.Value.Trim().ToUpper());
                fgenMV.Fn_Set_Mvar(uniq_id, "CONN", constr);
                mhd = fgen.seek_iname(uniq_id, txtcompcode.Value.Trim().ToUpper(), "select username from evas where TRIM(UPPER(USERNAME))='" + txtusername.Value.ToUpper().Trim() + "'", "username");
                if (mhd == "0")
                {
                    if (txtcompcode.Value.Trim().ToUpper() == "LIVN" || txtcompcode.Value.Trim().ToUpper() == "JSGI" || txtcompcode.Value.Trim().ToUpper() == "PRPL")
                    {
                        mhd = fgen.seek_iname(uniq_id, txtcompcode.Value.Trim().ToUpper(), "select acode from famst where TRIM(UPPER(acode))='" + txtusername.Value.ToUpper().Trim() + "'", "acode");
                        if (mhd != "0")
                        {
                            html_body = html_body + "Please find the password to access ERP web software.<br>";
                        }
                        else fgen.msg("-", "AMSG", "Wrong UserName or UserName is does not exist");
                    }
                    else fgen.msg("-", "AMSG", "Wrong UserName or UserName is does not exist");
                }
                else
                {
                    mhd = fgen.seek_iname(uniq_id, txtcompcode.Value.Trim().ToUpper(), "select emailid||'~'||level3pw as fstr from evas where TRIM(UPPER(USERNAME))='" + txtusername.Value.ToUpper().Trim() + "'", "fstr");
                    html_body = html_body + "Password : " + mhd.Split('~')[1] + "  <br>";

                    fgen.send_mail(txtcompcode.Value.Trim().ToUpper(), " ERP", mhd.Split('~')[0], "", "", " ERP password to access ", html_body);

                    fgen.msg("-", "AMSG", "Detail has been sent to email id : " + mhd.Split('~')[0]);

                }
            }
        }
    }

  
}