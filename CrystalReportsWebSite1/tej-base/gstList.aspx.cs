using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;
using System.Web.UI.HtmlControls;

using System.Net.Mail;

public partial class gstList : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, uname, frm_cocd, col1, col2, col3, cstr, vchnum, fromdt, todt, DateRange, year, ulvl, cDT1;
    string frm_mbr, mq1, frm_vty, frm_vnum, frm_url, frm_qstr, frm_uname, frm_PageName;
    string frm_tabname, typePopup = "Y", frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    DataRow oporow, dr1; int dhd; DataSet oDS;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    DataTable sg1_dt; DataRow sg1_dr;
    int i = 0, z = 0;
    string save_it;
    FileStream FilStr = null; BinaryReader BinRed = null;
    string Prg_Id;
    double totDel = 0;
    string pk_error = "Y", chk_rights = "N", PrdRange, cmd_query;
    ////       
    OracleConnection con = new OracleConnection();
    DataTable dt, dt1, dt3, dt4, dt5;
    DataSet ds;
    OracleCommand cmd;
    OracleDataAdapter da;
    MemoryStream oStream, oStream1;
    ReportDocument repDoc = new ReportDocument();
    // OracleConnection con;
    fgenDB fgen = new fgenDB();
    string btnmode, vardate, tabname, query, query1, pgname, condition, timeout, DRID, DRTYP;
    string tco_cd, mbr, cdt1, cdt2, scode, sname, seek, headername, daterange, ulevel, mlvl;
    string rptfilepath, rptpath, xmlpath, acessuser, smbr, sstring, pageid, appuser; string otp, mobileno;
    int limit;
    string app_col, app_level, app_txt, mail_txt, app_flag, app_status, filename, mypath;
    string fName, fpath, extension;
    StringBuilder sb;
    string fullname, sendtoemail, subject, mailpath, mailport, xmltag, compnay_code, mailmsg, mflag, branchname, col4, col5, col6, col7, fullname1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            //  btnnew.Focus();
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
                    if (frm_qstr.Contains("@"))
                    {
                        frm_formID = frm_qstr.Split('@')[1].ToString();
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                    }
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                //  doc_addl.Value = "1";
                fgen.DisableForm(this.Controls);
                // enablectrl();
                //getColHeading();
            }
            setColHeadings();
            //set_Val();       
            fgen.EnableForm(this.Controls);
        }
    }
    public void enablectrl()
    {
        btnexit.Visible = true;
        create_tab();
        sg1_add_blankrows();
        GridView1.DataSource = sg1_dt;
        GridView1.DataBind();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        //btnnew.Disabled = true;
        //btnedit.Disabled = true;
        //btnsave.Disabled = true;
        //btnlist.Disabled = true;
        //btnprint.Disabled = true;
        //btndel.Disabled = true;
        //btnhideF.Enabled = true;
        //btnhideF_s.Enabled = true;
        btnexit.Visible = false;
        //btncancel.Visible = true;
    }
    //--------------------------
    public void disp_data(string scode)
    {
        query = "";
        btnmode = hfbtnmode.Value;
        switch (btnmode)
        {
            case "FR":
                query = "select userid as fstr,username as assignto,userid from evas order by userid";
                break;
            case "TO":
                query = "select acode as fstr,fname as First_Name,Lname as last_name,CNAME AS COMPANY_NAME,mobile,email AS PRIMARY_EMAIL,PEMAIL AS SECONDARY_EMAIL,website,PADDR1 as paddress,PADDR2 as pcountry,PADDR3 as pstate,PADDR4 as pcity,PADDR5 as ppostalcode,PADDR6 as pregion,dept as department from contmst  where  type='TM' order by fname ";
                break;
            case "BR":
                query = "select type1 as fstr,name as Branch_name, type1 as code from type where id='B' order by type1";
                break;
            default:
                if (btnmode == "VI" || btnmode == "DI")
                {
                    if (pageid == "46101" || frm_cocd == "MMC")
                        query = "select '" + scode + "' as ftr,filename,filetype as file_type from filetable where branchcd||type||vchnum||to_char(vchdate,'ddmmyyyy') = '" + scode + "'";
                    if ((pageid == "51103" || pageid == "60412" || pageid == "42515") && btnmode == "DI")
                    {
                    }
                }
                break;
        }
        if (query == "") { }
        else
        {
            if (btnmode == "SURE_S") Response.Cookies["popupid"].Value = "FINSYS_S";
            else Response.Cookies["popupid"].Value = "Tejaxo";
            Response.Cookies["seeksql"].Value = query;
        }
    }
    public void sseekfunc(string scode)
    {
        // clearcontrol();
        disp_data(scode);
        OpenPopup("SSEEK");
    }
    public void AlertMsg(string msgtype, string msgname)
    {
        switch (msgtype)
        {
            case "AMSG":
                alermsg.InnerHtml = msgname;
                alermsg.Style.Add("display", "block");
                break;
        }
    }
    public void OpenPopup(string popuptype)
    {
        headername = "";
        btnmode = hfbtnmode.Value;
        switch (popuptype)
        {
            case "SSEEK":
                switch (btnmode)
                {
                    case "VI":
                        switch (pageid)
                        {
                            #region
                            //case "46101":
                            //    headername = "Resume Review";
                            //    break;
                            //case "46103":
                            //    headername = "PREMAGMA Information Library";
                            //    break;
                            //case "47103":
                            //    headername = "IGES Information Library";
                            //    break;
                            //case "48103":
                            //    headername = "PI Report in Information Library";
                            //    break;
                            //case "49103":
                            //    headername = "First PC Casting Report in Information Library";
                            //    break;
                            //case "43105":
                            //    headername = "Drawing Issue Preview";
                            //    break;
                            //case "52103":
                            //    headername = "SW Information Library";
                            //    break;
                            //case "53103":
                            //    headername = "MAGMA Information Library";
                            //    break;
                            //case "43106":
                            //    headername = "Drawing Information Library";
                            //    break;
                            //case "51103":
                            //    headername = "Trial Information Library";
                            //    break;
                            //case "60412":
                            //    lblhead.Text = "Quality Information Library";
                            //    break;
                            //case "51503":
                            //    headername = "Methor Card Information Library";
                            //    break;
                            //case "60110a":
                            //    lblhead.Text = "Moulding Plan Information Library";
                            //    break;
                            //case "60113":
                            //    lblhead.Text = "Moulding Plan Information Library";
                            //    break;
                            //case "60213":
                            //    lblhead.Text = "Pouring Information Library";
                            //    break;
                            //case "60313":
                            //    lblhead.Text = "Knock Out Information Library";
                            //    break;
                            //case "42515":
                            //    lblhead.Text = "Closing Review Information Library";
                            //    break;
                            #endregion
                        }
                        break;
                    case "BR":
                        headername = "Branch Master";
                        break;
                    default:
                        if (btnmode == "FR" || btnmode == "TO")
                            headername = "User Master";
                        break;
                }
                break;
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','SSeek.aspx','75%','82%',false);});", true);
    }

    public void BindData(string query)
    {
        lblshow.Text = ""; query1 = "";
        //clearcontrol();

        dt = new DataTable();

        if (txtrows.Value == "")
        {
            txtrows.Value = fgen.GetXMLTag("Show_row");
            query1 = "Select * from(" + query + ") where rownum<=" + txtrows.Value + "";
        }
        else
            query1 = "Select * from(" + query + ") where rownum<=" + txtrows.Value + "";


        //con.Open();

        dt5 = new DataTable();
        app_col = ""; app_level = ""; app_txt = "";

        if (pageid == "40106")
        {
            app_col = "col37";
            app_txt = "Verify";
        }
        if (pageid == "40107")
        {
            app_col = "col39";
            app_txt = "Approve";
        }

        if (co_cd == "SHOP" || co_cd == "SNPX")
        {

            da = new OracleDataAdapter("select distinct " + app_col + "  from scratch where branchcd = '" + mbr + "' and type='RQ'  ", con);
            da.Fill(dt5);

            foreach (DataRow dr in dt5.Rows)
            {
                app_level = dr[0].ToString().Trim();
            }

            if (app_level == uname)
            {
                da = new OracleDataAdapter(query1, con);
                da.Fill(dt);
            }
            else
            {
                AlertMsg("AMSG", "Sorry!! you are not authorized to " + app_txt + " order reqisition. ");
                con.Close();
                return;
            }
        }
        else
        {
            dt = fgen.getdata(frm_qstr, frm_cocd, query1);
        }
        if (dt.Rows.Count == 0 && hfbtnmode.Value != "TR")
            AlertMsg("AMSG", "No " + pgname + " exists in Database for this user");

        ViewState["SDATA"] = dt;
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridView1.Visible = true;

        if (hfbtnmode.Value == "EX")
        {
            if (dt.Rows.Count > 0)
            {
                headername = Label1.Text;
                //fgen.ExportData(dt, "ms-excel", "xls", headername);
                fgen.exp_to_excel(dt, "ms-excel", "xls", headername);
            }
        }

        if (pageid == "40103" && (co_cd == "JSGI" || co_cd == "DLJM" || co_cd == "SDM"))
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                //((TextBox)row.FindControl("txttout")).Text = fgen.InserTime(vardate).Substring(11, 5);
            }
        }

        lblshow.Text = "Showing " + dt.Rows.Count + " Rows ";
    }

    protected void btnexp_Click(object sender, EventArgs e)
    {
        hfbtnmode.Value = "EX";
        sg1_dt = new DataTable();
        sg1_dt = (DataTable)ViewState["sg1"];
        //   fgen.ExportData(dt, "ms-excel", "xls", "TEST_" + DateTime.Now.ToString().Replace("/", "_"));
        //     fgen.exp_to_excel(dt, "ms-excel", "xls", "TEST_" + DateTime.Now.ToString().Replace("/", "_"));
        if (sg1_dt.Rows.Count > 0) fgen.exp_to_excel(sg1_dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Replace("/", "_").Trim());
        else fgen.msg("-", "AMSG", "No Data to Export"); sg1_dt.Dispose();

    }
    public void clearctrl()
    {
        // hffield.Value = "";
    }
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f21", typeof(string)));
    }
    public void sg1_add_blankrows()
    {
        if (sg1_dt != null)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f5"] = "-";
            sg1_dr["sg1_f6"] = "-";
            sg1_dr["sg1_f7"] = "-";
            sg1_dr["sg1_f8"] = "-";
            sg1_dr["sg1_f9"] = "-";
            sg1_dr["sg1_f10"] = "-";
            sg1_dr["sg1_f11"] = "-";
            sg1_dr["sg1_f12"] = "-";
            sg1_dr["sg1_f13"] = "-";
            sg1_dr["sg1_f14"] = "-";
            sg1_dr["sg1_f15"] = "-";
            sg1_dr["sg1_f16"] = "-";
            sg1_dr["sg1_f17"] = "-";
            sg1_dr["sg1_f18"] = "-";
            sg1_dr["sg1_f19"] = "-";
            sg1_dr["sg1_f20"] = "-";
            sg1_dr["sg1_f21"] = "-";
            sg1_dt.Rows.Add(sg1_dr);
        }
    }
    protected void btntrans_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        hfbtnmode.Value = "SUMM1";
        query = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')||trim(A.COL1) AS FSTR,A.CO_CD AS COMP_CODE,E.FULL_NAME AS COMP_NAME,A.COL2 AS NO_OF_DEL,A.COL3 AS AMT,A.COL4 AS PAYMENT_DATE,A.COL11 AS PAYMENT_MODE,(CASE WHEN A.COL11='NEFT' THEN A.COL5||A.COL6 else A.COL6 END) AS NO,A.COL7 AS CONTACT_PER,A.COL8 AS EMAIL_ID,A.COL9 AS MOBILE,A.COL1 AS DELEGATE_NAME,A.COL10 AS DESIGNATION,A.COL12 AS DELEGATE_MOBILE,A.SRNO,A.COL13 AS EMAIL_SENT,A.PSTATUS FROM GST A,EVAS E WHERE UPPER(TRIM(USERNAME))=UPPER(TRIM(A.CO_CD)) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='G1' ORDER BY FSTR,A.SRNO";
        query = "SELECT a.FSTR,a.type,a.COMP_CODE,E.FULL_NAME AS COMP_NAME,a.NO_OF_DEL,a.AMT,a.CONTACT_PER,a.EMAIL_ID,a.MOBILE,a.DELEGATE_NAME,a.DESIGNATION,a.DELEGATE_MOBILE,a.SRNO,a.EMAIL_SENT,A.PSTATUS FROM (SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Paid' as type,CO_CD AS COMP_CODE,num1 AS NO_OF_DEL,COL3 AS AMT,COL7 AS CONTACT_PER,COL8 AS EMAIL_ID,COL9 AS MOBILE,COL1 AS DELEGATE_NAME,COL10 AS DESIGNATION,COL12 AS DELEGATE_MOBILE,SRNO,COL14 AS EMAIL_SENT,A.PSTATUS FROM GST A WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='G1' AND A.VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') UNION ALL SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Free' as type, COCODE AS COMP_CODE,1 AS NO_OF_DEL,'-' AS AMT,'-' AS CONTACT_PER,EMAILID AS EMAIL_ID,'-' AS MOBILE,VNAME AS DELEGATE_NAME,DESG AS DESIGNATION,MOBILE AS DELEGATE_MOBILE,SRNO,nvl(COL14,'-') AS EMAIL_SENT,'-' as pstatus FROM SEMINAR WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00' AND VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY')) A,EVAS E WHERE UPPER(TRIM(E.USERNAME))=UPPER(TRIM(A.COMP_CODE)) ORDER BY COMP_CODE,srno";

        query = "SELECT a.FSTR,a.type,a.COMP_CODE,E.FULL_NAME AS COMP_NAME,a.EMAIL_ID,a.DELEGATE_NAME,a.DESIGNATION,a.DELEGATE_MOBILE,a.SRNO,a.EMAIL_SENT,A.PSTATUS FROM (SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Paid' as type, COCODE AS COMP_CODE,EMAILID AS EMAIL_ID,VNAME AS DELEGATE_NAME,DESG AS DESIGNATION,MOBILE AS DELEGATE_MOBILE,SRNO,nvl(COL14,'-') AS EMAIL_SENT,'-' as pstatus FROM SEMINAR WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00' AND VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND NVL(PAID_ENTRY,'-')='Y' union all SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Free' as type, COCODE AS COMP_CODE,EMAILID AS EMAIL_ID,VNAME AS DELEGATE_NAME,DESG AS DESIGNATION,MOBILE AS DELEGATE_MOBILE,SRNO,nvl(COL14,'-') AS EMAIL_SENT,'-' as pstatus FROM SEMINAR WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00' AND VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND NVL(PAID_ENTRY,'-')!='Y') A,EVAS E WHERE UPPER(TRIM(E.USERNAME))=UPPER(TRIM(A.COMP_CODE)) ORDER BY COMP_CODE,type,srno"; //new

        query = "SELECT a.FSTR,a.type,a.COMP_CODE,A.CONAME AS COMP_NAME,a.EMAIL_ID,a.DELEGATE_NAME,a.DESIGNATION,a.DELEGATE_MOBILE,a.SRNO,a.EMAIL_SENT,A.PSTATUS,A.GENT_BY,TO_CHAR(A.GENT_dT,'DD/MM/YYYY HH:MM:SS') AS GENT_DT FROM (SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Paid' as type, COCODE AS COMP_CODE,EMAILID AS EMAIL_ID,VNAME AS DELEGATE_NAME,DESG AS DESIGNATION,MOBILE AS DELEGATE_MOBILE,SRNO,nvl(COL14,'-') AS EMAIL_SENT,'-' as pstatus,CONAME,GENT_BY,GENT_dT FROM SEMINAR WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00' AND VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND NVL(PAID_ENTRY,'-')='Y' union all SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Free' as type, COCODE AS COMP_CODE,EMAILID AS EMAIL_ID,VNAME AS DELEGATE_NAME,DESG AS DESIGNATION,MOBILE AS DELEGATE_MOBILE,SRNO,nvl(COL14,'-') AS EMAIL_SENT,'-' as pstatus,CONAME,GENT_BY,GENT_dT FROM SEMINAR WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00' AND VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY')) A ORDER BY COMP_CODE,type,srno";
        //bindval();

        txtTotDel.Text = "";
        txtFreeDel.Text = "";
        ViewState["SSQUERY"] = query;
        dt = new DataTable();
        create_tab();
        sg1_dr = null;
        dt = fgen.getdata(frm_qstr, frm_cocd, query);
        for (int d = 0; d < dt.Rows.Count; d++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f5"] = dt.Rows[d]["fstr"].ToString().Trim();
            sg1_dr["sg1_f6"] = dt.Rows[d]["type"].ToString().Trim();
            sg1_dr["sg1_f7"] = dt.Rows[d]["COMP_CODE"].ToString().Trim();
            sg1_dr["sg1_f8"] = dt.Rows[d]["COMP_NAME"].ToString().Trim();
            // sg1_dr["sg1_f9"] = dt.Rows[d]["NO_OF_DEL"].ToString().Trim();
            // sg1_dr["sg1_f10"] = dt.Rows[d]["AMT"].ToString().Trim();
            // sg1_dr["sg1_f11"] = dt.Rows[d]["CONTACT_PER"].ToString().Trim();
            sg1_dr["sg1_f12"] = dt.Rows[d]["EMAIL_ID"].ToString().Trim();
            // sg1_dr["sg1_f13"] = dt.Rows[d]["MOBILE"].ToString().Trim();
            sg1_dr["sg1_f14"] = dt.Rows[d]["DELEGATE_NAME"].ToString().Trim();
            sg1_dr["sg1_f15"] = dt.Rows[d]["DESIGNATION"].ToString().Trim();
            sg1_dr["sg1_f16"] = dt.Rows[d]["DELEGATE_MOBILE"].ToString().Trim();
            sg1_dr["sg1_f17"] = dt.Rows[d]["SRNO"].ToString().Trim();
            sg1_dr["sg1_f18"] = dt.Rows[d]["EMAIL_SENT"].ToString().Trim();
            sg1_dr["sg1_f19"] = dt.Rows[d]["PSTATUS"].ToString().Trim();
            sg1_dr["sg1_f20"] = dt.Rows[d]["GENT_BY"].ToString().Trim();
            sg1_dr["sg1_f21"] = dt.Rows[d]["GENT_dT"].ToString().Trim();
            sg1_dt.Rows.Add(sg1_dr);
        }
        ViewState["sg1"] = sg1_dt;
        GridView1.DataSource = sg1_dt;
        GridView1.DataBind();
        txtrows.Value = "Showing " + dt.Rows.Count + " Rows ";
        lblshow.Text = "Showing " + dt.Rows.Count + " Rows ";
        lblshow.Text = "";
        mq1 = "select  COUNT(*) AS DD from seminar  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00'";
        totDel = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, mq1, "DD"));
        txtTotDel.Text = "Total Delegates : " + totDel.ToString();
        dt.Dispose(); sg1_dt.Dispose();
        return;
    }

    public void GetLeadData(string scode, string reason, string rdate, string cuser)
    {
        //if ((co_cd == "NIRM" || co_cd == "PRAG") && (pageid == "40102" || pageid == "40103"))
        //    cmd = new OracleCommand("update " + tabname + " set app_by='" + appuser + "',app_Dt=to_date('" + vardate + "','dd/mm/yyyy'),reason='" + reason + "',RDate=to_date('" + rdate + "','dd/mm/yyyy') where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        //else if (pageid == "40106" || pageid == "40102" || pageid == "40104")
        //    cmd = new OracleCommand("update " + tabname + " set chk_by='" + appuser + "',chk_Dt=to_date('" + vardate + "','dd/mm/yyyy'),naration='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        //else if (pageid == "40107" || pageid == "40105" || pageid == "40101" || pageid == "40201" || pageid == "40204a" || pageid == "40203" || pageid == "40205a")
        //{
        //    // Mobile No for JSGI
        //    otp = "";
        //    mobileno = fgen.seek_iname(frm_qstr,frm_cocd, "SELECT COL23 FROM SCRATCH2 WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY')='" + scode + "'", "COL23");
        //    //otp = fgen.gen_otp(co_cd);
        //    otp = fgen.gen_otp(frm_qstr, frm_cocd);
        //    if (co_cd == "JSGI" && pageid == "40101")
        //        cmd = new OracleCommand("update " + tabname + " set col28='" + otp + "', app_by='" + appuser + "', app_Dt=to_date('" + vardate + "','dd/mm/yyyy'),reason='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        //    else
        //        cmd = new OracleCommand("update " + tabname + " set app_by='" + appuser + "',app_Dt=to_date('" + vardate + "','dd/mm/yyyy'),reason='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        //}
        //else if (pageid == "40103" && (co_cd == "JSGI" || co_cd == "DLJM" || co_cd == "SDM"))
        //    cmd = new OracleCommand("update " + tabname + " set COL38='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        //else if (pageid == "40108")
        //    cmd = new OracleCommand("update " + tabname + " set COL24='" + appuser + "',reason='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);

        //cmd.ExecuteNonQuery();

        //if (co_cd == "SHOP" || co_cd == "SNPX") mflag = send_mail(mflag, cuser, scode, reason);
        //if (pageid == "40108" || pageid == "40201" || pageid == "40204a") mflag = send_email(mflag, cuser, scode, reason);

        //if (co_cd == "JSGI" && pageid == "40101")
        //{
        //    // vipin
        //    col2 = fgen.seek_iname(frm_qstr,frm_cocd, "Select trim(col16) as name from scratch2 where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", "name");
        //    //fgen.send_sms(mobileno, "Dear " + col2 + ", Welcome to " + co_cd + ", Please show this OTP " + otp + " at the Gate.");
        //    fgen.send_sms2(co_cd, mobileno, "Dear " + col2 + ", Welcome to " + co_cd + ", Please show this OTP " + otp + " at the Gate.");
        //}
    }
    protected void btnfrom_Click(object sender, ImageClickEventArgs e)
    {
        hfbtnmode.Value = "FR";
        sseekfunc("");
    }
    protected void btnto_Click(object sender, ImageClickEventArgs e)
    {
        hfbtnmode.Value = "TO";
        sseekfunc("");
    }

    protected void btnmbr_Click(object sender, ImageClickEventArgs e)
    {
        hfbtnmode.Value = "BR";
        sseekfunc("");
    }
    public void OpenMyFile(string fpath, string extension)
    {
        i = 0;
        i = fpath.IndexOf(@"\Uploads");
        fName = fpath.Substring(i, fpath.Length - i);

        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".pdf")
            viewpic(fName);
        else
            viewpic("XXXX");

        if (hfbtnmode.Value == "DI") DownloadFile(fName);
    }

    public void DownloadFile(string filepath)
    {

        filename = ""; mypath = "";
        filename = filepath.Remove(0, 9);
        mypath = Server.MapPath("~" + filepath);
        Response.Clear();
        Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
        Response.ContentType = "application/octet-stream";
        Response.WriteFile(mypath);
        Response.Flush();
        Response.End();
    }

    public void viewpic(string imgpath)
    {
        Session["MYURL"] = imgpath;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('Attachment Preview Window','View.aspx','95%','95%');});", true);
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Desktop.aspx");
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        scode = ""; sname = ""; seek = "";

        if (Request.Cookies["Column1"].Value != null)
        {
            scode = Request.Cookies["Column1"].Value.ToString().Trim();
            scode = scode.Replace("&AMP;", "&").Trim();
        }
        if (Request.Cookies["Column2"].Value != null)
        {
            sname = Request.Cookies["Column2"].Value.ToString().Trim();
            sname = sname.Replace("&AMP;", "&").Trim();
        }
        if (Request.Cookies["Column3"].Value != null)
        {
            seek = Request.Cookies["Column3"].Value.ToString().Trim();
            seek = seek.Replace("&AMP;", "&").Trim();
        }
        btnmode = hfbtnmode.Value;
        con.Open();
        switch (btnmode)
        {
            case "FR":
                txtfrom.Text = sname;
                break;
            case "TO":
                txtto.Text = sname;
                break;
            case "BR":
                if (smbr.Contains(scode)) { }
                else
                {
                    if (Convert.ToInt32(scode) < 3)
                        AlertMsg("AMSG", "Please select sun impex branch location");
                    else
                        AlertMsg("AMSG", "Please select Shimla branch location");

                    txtbname.BorderColor = Color.Red;
                    return;
                }
                txtbcode.Text = scode;
                txtbname.Text = sname;
                break;
            default:
                if (btnmode == "VI" || btnmode == "DI")
                {
                    if (pageid == "46101" || co_cd == "MMC")
                    {
                        i = 0;
                        fName = ""; fpath = ""; extension = "";

                        dt = new DataTable();
                        da = new OracleDataAdapter("select filepath,filetype from filetable where branchcd||type||vchnum||to_char(vchdate,'ddmmyyyy') = '" + scode + "' and trim(filename)='" + sname + "'", con);
                        da.Fill(dt);
                        fpath = dt.Rows[0][0].ToString().Trim();
                        extension = dt.Rows[0][1].ToString().Trim();
                        OpenMyFile(fpath, extension);
                    }
                }
                break;
        }
        con.Close();
        if (btnmode == "FR" || btnmode == "TO")
        {
            bindval();
        }
    }
    public void bindval()
    {
        condition = "";
        ViewState["SSQUERY"] = query;
        BindData(query);
    }
    public void crystal_rpt()
    {
        if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
            rptpath = "~/Report/RFQ.rpt";
        else if (pageid == "40106" || pageid == "40107")
        {
            if (Convert.ToInt32(mbr) < 3)
                rptpath = "~/Report/BAR_SHOP.rpt";
            else
                rptpath = "~/Report/BAR_SNPX.rpt";
        }
        Response.Cookies["rptfile"].Value = rptpath;
        CrystalDecisions.CrystalReports.Engine.ReportDocument report;
        report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rptfilepath = Server.MapPath("" + rptpath + "");
        report.Load(rptfilepath);
        report.SetDataSource(ds);
        CRV1.ReportSource = report;
        CRV1.DataBind();
        oStream = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
    }
    public void send_crystal_rpt(string scode)
    {
        if (pageid != "51503" && pageid != "60110a" && pageid != "60113" && pageid != "60213" && pageid != "60313")
        {
            ds = new DataSet();

            if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
                da = new OracleDataAdapter("select DISTINCT a.* ,b.remaRKS,a.person as fname,c.mobile,c.email as pemail,C.PADDR1,C.PADDR2,C.PADDR3,C.PADDR4,C.PADDR5,C.PADDR6 from " + tabname + " a left outer join CONTMST C on TRIM(A.ACODE)=TRIM(c.ACODE), description b where a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'DDMMYYYY') and a.branchcd = '" + mbr + "' and a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY')='" + scode.Substring(2, 16) + "' order by a.srno", con);
            else if (pageid == "40106" || pageid == "40107")
                da = new OracleDataAdapter("select  DISTINCT b.INAME,b.INAME AS INAME1, b.col17,b.col19,b.col37,b.col39, B.CHK_BY,B.APP_BY,b.val23,B.VAL24,B.VAL25,B.VAL26,B.VAL27,B.VAL28,B.VAL29,B.VAL30,B.VAL31,B.VAL32,B.VAL33,B.VAL34,B.VAL35,B.VAL36,B.VAL37,B.VAL38,B.VAL39,B.VAL40,B.VAL41,B.VAL42,B.VAL43,B.VAL44,B.VAL45,B.VAL46,B.VAL47,b.val48, B.RMK1,B.RMK2,B.RMK3,B.RMK4,B.RMK5,B.RMK6,B.RMK7,B.RMK8,B.RMK9,B.RMK10,B.RMK11,B.RMK12,B.RMK13,B.RMK14,B.RMK15,B.RMK16,B.RMK17,B.RMK18,B.RMK19, b.col20,b.vchnum as reqno,to_char(b.vchdate,'dd/mm/yyyy') as reqdt, b.srno AS SNO,b.val22,b.col15,b.col16, b.col23,b.COL2,b.COL3,b.COL4,b.COL6,b.COL7,b.COL8,b.COL9,b.COL10,b.col40, b.COL12, A.vchnum,A.vchdate,A.cname,A.assg,A.cyname,A.person,C.mobile,'-' as pterm, b.acode as code, b.vchnum as dno,to_char(b.vchdate,'dd/mm/yyyy') as docdt,b.ent_by as enby,b.ent_dt as endt,b.edt_by as edby,b.edt_dt as eddt,b.col5,b.col18, b.col11,b.col24,b.col25,b.col31,b.val1,b.val2,b.val3,b.val4,b.val5,b.val6,b.val7,b.val8,b.val9,b.val10,b.val11,b.val12,b.val13,b.val14,b.val15,b.val16,b.val17,b.val18,b.val19,b.val20,b.val21, 0 as srno,'-' as PSGRP,0 as QTY,'-' as UNIT,0 as QRATE,'-' as QCURR,0 as qval,0 as camt,'-' as chk_dept,'-' as app_dept,'-' as ent_dept FROM LEADMST A, scratch b,CONTMST C where TRIM(A.ACODE)=TRIM(C.ACODE) AND a.vchnum||to_char(a.vchdate,'DDMMYYYY')=b.invno||to_char(b.invdate,'DDMMYYYY') and  b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'DDMMYYYY')='" + scode + "' order by b.srno  ", con);
            else if (pageid == "40102" || pageid == "40103" || pageid == "40104" || pageid == "40105" || pageid == "40101" || pageid == "40108" || pageid == "40201" || pageid == "40204a" || pageid == "40203" || pageid == "40205a")
                da = new OracleDataAdapter("select * from " + tabname + "  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + scode + "'", con);
            da.Fill(ds, "Prepcur");
        }
    }

    public string GetData(string val)
    {
        col2 = string.Empty;

        dt3 = new DataTable();
        da = new OracleDataAdapter("select erpdeptt from evas where upper(trim(username)) in ('" + val + "')", con);
        da.Fill(dt3);

        dt4 = new DataTable();
        if (dt3.Rows.Count > 0)
        {
            da = new OracleDataAdapter("select replace(name,'&','') as DepartmenT from TYPE where trim(type1) = '" + dt3.Rows[0][0].ToString().Trim() + "' and id='M' and substr(type1,1,1) in('6')", con);
            da.Fill(dt4);
        }
        if (dt4.Rows.Count > 0)
            col2 = dt4.Rows[0][0].ToString().Trim();
        return col2;
    }

    public void callscript()
    {
        //ScriptManager.RegisterStartupScript(GridView1, this.GetType(), "jcall", "gridviewScroll();", true);
    }

    protected void LnkBtnd_Click(object sender, EventArgs e)
    {

        //clearcontrol();
        //col1 = "";

        //LinkButton selectButton = (LinkButton)sender;
        //GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        //col1 = GridView1.Rows[row.RowIndex].Cells[6].Text.Trim().ToString();

        //if (co_cd == "MMC")
        //{
        //    hfbtnmode.Value = "DI";
        //    if (pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313")
        //    {
        //        //col1 = mbr + "TN" + GridView1.Rows[row.RowIndex].Cells[9].Text.Trim() + GridView1.Rows[row.RowIndex].Cells[10].Text.Trim().Replace("/", "");
        //        hfbtnmode.Value = "VI";
        //        con.Open();
        //        send_crystal_rpt(col1);
        //        con.Close();
        //        headername = "" + pgname + " Master Print";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
        //    }
        //    else
        //        sseekfunc(col1);
        //}
    }

    protected void LnkBtnv_Click(object sender, EventArgs e)
    {

        //clearcontrol();
        //col1 = "";

        //LinkButton selectButton = (LinkButton)sender;
        //GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        //col1 = GridView1.Rows[row.RowIndex].Cells[6].Text.Trim().ToString();

        //if (pageid == "46101" || co_cd == "MMC")
        //{
        //    hfbtnmode.Value = "VI";
        //    if (pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313")
        //    {
        //        col1 = mbr + "TN" + GridView1.Rows[row.RowIndex].Cells[9].Text.Trim() + GridView1.Rows[row.RowIndex].Cells[10].Text.Trim().Replace("/", "");
        //        hfbtnmode.Value = "DI";
        //    }
        //    sseekfunc(col1);
        //}
        //else
        //{
        //    con.Open();
        //    send_crystal_rpt(col1);
        //    con.Close();
        //    headername = "" + pgname + " Master Print";
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
        //}
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < GridView1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < GridView1.Columns.Count; j++)
                {
                    GridView1.Rows[sg1r].Cells[j].ToolTip = GridView1.Rows[sg1r].Cells[j].Text;
                    if (GridView1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        GridView1.Rows[sg1r].Cells[j].Text = GridView1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }
            LinkButton vlink = (LinkButton)e.Row.FindControl("LnkBtnv");
            LinkButton mlink = (LinkButton)e.Row.FindControl("LnkBtnd");
            mlink.Visible = false;
            //if (frm_cocd == "MMC") mlink.Visible = true;
            //if (pageid == "51103" || pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313" || pageid == "60412" || pageid == "42515") mlink.Text = "View Details";
            //if (pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313") vlink.Visible = false;

            //if (hfbtnmode.Value == "SUMM" || hfbtnmode.Value == "SUMM1")
            {
                e.Row.Cells[0].Style["display"] = "none";
                GridView1.HeaderRow.Cells[0].Style["display"] = "none";
                //e.Row.Cells[1].Style["display"] = "none";
                //GridView1.HeaderRow.Cells[1].Style["display"] = "none";

                //  e.Row.Cells[2].Style["display"] = "none";
                // GridView1.HeaderRow.Cells[2].Style["display"] = "none";
                e.Row.Cells[3].Style["display"] = "none";
                GridView1.HeaderRow.Cells[3].Style["display"] = "none";
                e.Row.Cells[4].Style["display"] = "none";
                GridView1.HeaderRow.Cells[4].Style["display"] = "none";
                e.Row.Cells[5].Style["display"] = "none";
                GridView1.HeaderRow.Cells[5].Style["display"] = "none";
                e.Row.Cells[6].Style["display"] = "none";
                GridView1.HeaderRow.Cells[6].Style["display"] = "none";

                if (hfbtnmode.Value == "SUMM1")
                {
                    //  e.Row.Cells[8].Style["display"] = "none";
                    //  GridView1.HeaderRow.Cells[9].Style["display"] = "none";
                    //e.Row.Cells[9].Style["display"] = "none";
                    //GridView1.HeaderRow.Cells[9].Style["display"] = "none";
                    e.Row.Cells[10].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[10].Style["display"] = "none";
                    e.Row.Cells[11].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[11].Style["display"] = "none";
                    e.Row.Cells[12].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[12].Style["display"] = "none";
                    e.Row.Cells[14].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[14].Style["display"] = "none";
                    if (e.Row.Cells[15].Text == "Y")
                        ((CheckBox)e.Row.FindControl("chkno")).Checked = true;
                    if (e.Row.Cells[16].Text.Trim() == "Y")
                        ((RadioButton)e.Row.FindControl("radOk")).Checked = true;
                }
                if (hfbtnmode.Value == "SUMM2")
                {
                    e.Row.Cells[10].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[10].Style["display"] = "none";
                    e.Row.Cells[11].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[11].Style["display"] = "none";
                    e.Row.Cells[12].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[12].Style["display"] = "none";

                    e.Row.Cells[14].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[14].Style["display"] = "none";
                    if (e.Row.Cells[15].Text == "Y")
                        ((CheckBox)e.Row.FindControl("chkno")).Checked = true;
                    if (e.Row.Cells[16].Text.Trim() == "Y")
                        ((RadioButton)e.Row.FindControl("radOk")).Checked = true;
                }
                if (hfbtnmode.Value == "SUMM3")
                {
                    //GridView1.HeaderRow.Cells[15].Style["display"] = "none";
                    //e.Row.Cells[15].Style["display"] = "none";
                    //GridView1.HeaderRow.Cells[16].Style["display"] = "none";
                    //e.Row.Cells[16].Style["display"] = "none";
                    //GridView1.HeaderRow.Cells[17].Style["display"] = "none";
                    //e.Row.Cells[17].Style["display"] = "none";
                    //GridView1.HeaderRow.Cells[18].Style["display"] = "none";
                    //e.Row.Cells[18].Style["display"] = "none";
                    if (e.Row.Cells[11].Text == "Y")
                        ((CheckBox)e.Row.FindControl("chkno")).Checked = true;
                }
                if (hfbtnmode.Value == "SUMM4")
                {
                    e.Row.Cells[10].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[10].Style["display"] = "none";
                    e.Row.Cells[11].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[11].Style["display"] = "none";
                    e.Row.Cells[12].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[12].Style["display"] = "none";

                    e.Row.Cells[14].Style["display"] = "none";
                    GridView1.HeaderRow.Cells[14].Style["display"] = "none";
                    if (e.Row.Cells[15].Text == "Y")
                        ((CheckBox)e.Row.FindControl("chkno")).Checked = true;
                    if (e.Row.Cells[16].Text.Trim() == "Y")
                        ((RadioButton)e.Row.FindControl("radOk")).Checked = true;
                }
            }
        }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        // ViewState["SSQUERY"] = query;
        query = ViewState["SSQUERY"].ToString();
        BindData(query);
        txtsearch.Text = "";
    }
    protected void btnRep2_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        query = "";
        hfbtnmode.Value = "SUMM2";
        query = "SELECT distinct TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD') AS FSTR,A.CO_CD AS COMP_CODE,E.FULL_NAME AS COMP_NAME,A.COL2 AS NO_OF_DEL,A.COL3 AS AMT,A.COL4 AS PAYMENT_DATE,A.COL11 AS PAYMENT_MODE,(CASE WHEN A.COL11='NEFT' THEN A.COL5||A.COL6 else A.COL6 END) AS NO,A.COL7 AS CONTACT_PER,A.COL8 AS EMAIL_ID,A.COL9 AS MOBILE,A.COL13 AS EMAIL_SENT,A.PSTATUS FROM GST A,EVAS E WHERE UPPER(TRIM(USERNAME))=UPPER(TRIM(A.CO_CD)) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='G1' ORDER BY FSTR";
        query = "SELECT TRIM(a.BRANCHCD)||TRIM(a.TYPE)||TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'YYYYMMDD') AS FSTR,'Free' as type,a.COCODE AS COMP_CODE,E.FULL_NAME AS COMP_NAME,a.EMAILID AS EMAIL_ID,a.VNAME AS DELEGATE_NAME,a.DESG AS DESIGNATION,a.MOBILE AS DELEGATE_MOBILE,a.SRNO,nvl(a.col14,'-') AS EMAIL_SENT,'-' AS PSTATUS FROM SEMINAR a, EVAS E WHERE UPPER(TRIM(E.USERNAME))=UPPER(TRIM(A.Cocode)) and nvl(a.paid_entry,'-')!='Y' and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='00' AND a.VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND a.SRNO=1  ORDER BY COMP_CODE,srno";
        query = "SELECT TRIM(a.BRANCHCD)||TRIM(a.TYPE)||TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'YYYYMMDD') AS FSTR,'Free' as type,a.COCODE AS COMP_CODE,A.CONAME AS COMP_NAME,a.EMAILID AS EMAIL_ID,a.VNAME AS DELEGATE_NAME,a.DESG AS DESIGNATION,a.MOBILE AS DELEGATE_MOBILE,a.SRNO,nvl(a.col14,'-') AS EMAIL_SENT,'-' AS PSTATUS,A.GENT_BY,TO_CHAR(A.GENT_DT,'DD/MM/YYYY HH:MM:SS') AS GENT_dT FROM SEMINAR a WHERE  nvl(a.paid_entry,'-')!='Y' and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='00' AND a.VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND a.SRNO=1  ORDER BY COMP_CODE,srno";
        ViewState["SSQUERY"] = query;
        //  bindval();
        totDel = 0;
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, query);
        create_tab();
        sg1_dr = null;
        for (int d = 0; d < dt.Rows.Count; d++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f5"] = dt.Rows[d]["fstr"].ToString().Trim();
            sg1_dr["sg1_f6"] = dt.Rows[d]["type"].ToString().Trim();
            sg1_dr["sg1_f7"] = dt.Rows[d]["COMP_CODE"].ToString().Trim();
            sg1_dr["sg1_f8"] = dt.Rows[d]["COMP_NAME"].ToString().Trim();
            // sg1_dr["sg1_f9"] = dt.Rows[d]["NO_OF_DEL"].ToString().Trim();
            //sg1_dr["sg1_f10"] = dt.Rows[d]["AMT"].ToString().Trim();
            // sg1_dr["sg1_f11"] = dt.Rows[d]["CONTACT_PER"].ToString().Trim();
            sg1_dr["sg1_f12"] = dt.Rows[d]["EMAIL_ID"].ToString().Trim();
            //  sg1_dr["sg1_f13"] = dt.Rows[d]["MOBILE"].ToString().Trim();
            sg1_dr["sg1_f14"] = dt.Rows[d]["DELEGATE_NAME"].ToString().Trim();
            sg1_dr["sg1_f15"] = dt.Rows[d]["DESIGNATION"].ToString().Trim();
            sg1_dr["sg1_f16"] = dt.Rows[d]["DELEGATE_MOBILE"].ToString().Trim();
            sg1_dr["sg1_f17"] = dt.Rows[d]["SRNO"].ToString().Trim();
            sg1_dr["sg1_f18"] = dt.Rows[d]["EMAIL_SENT"].ToString().Trim();
            sg1_dr["sg1_f19"] = dt.Rows[d]["PSTATUS"].ToString().Trim();
            sg1_dr["sg1_f20"] = dt.Rows[d]["GENT_BY"].ToString().Trim();
            sg1_dr["sg1_f21"] = dt.Rows[d]["GENT_dT"].ToString().Trim();
            sg1_dt.Rows.Add(sg1_dr);
        }
        // sg1_add_blankrows();
        ViewState["sg1"] = sg1_dt;
        GridView1.DataSource = sg1_dt;
        GridView1.DataBind();
        // lblshow.Text = "Showing " + dt.Rows.Count + " Rows ";
        lblshow.Text = "";
        txtrows.Value = "Showing " + dt.Rows.Count + " Rows ";
        dt.Dispose(); sg1_dt.Dispose();
      
        mq1 = "select sum(Srno) as srno from (select cocode,(case when srno=2 then 1 else srno end) as srno from seminar where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_date('18/09/2019','dd/mm/yyyy'))";
        mq1 = "select  COUNT(*) AS DD from seminar   WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00' AND NVL(PAID_eNTRY,'-')!='Y'";
        totDel = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, mq1, "DD"));
        // txtTotDel.Text = "Total Paid Delegates : " + totDel.ToString();
        txtTotDel.Text = "Total Free Delegates : " + totDel.ToString();
        return;
    }

    protected void btnRep3_Click(object sender, EventArgs e)
    {
        query = "";
        hfbtnmode.Value = "SUMM3";
        query = "SELECT distinct TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD') AS FSTR,A.col1 as company,a.col2 as participant,a.col3 as Contact_No,a.col4 as EmailID,A.COL5 AS EMAIL_SENT,A.PSTATUS from GST_OW a ORDER BY FSTR";
        ViewState["SSQUERY"] = query;
        bindval();
        txtTotDel.Text = "";
        return;
    }
    void getColHeading()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            dtCol = fgen.getdata(frm_qstr, frm_cocd, fgenMV.Fn_Get_Mvar(frm_qstr, "U_SYS_COM_QRY") + " WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
        }
        ViewState["d" + frm_qstr + frm_formID] = dtCol;
    }
    //------------------------------------------------------------------------------------
    void setColHeadings()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            getColHeading();
        }
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null) return;
        if (GridView1.Rows.Count <= 0) return;
        for (int sR = 0; sR < GridView1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = GridView1.HeaderRow.Cells[sR].Text.Trim();

            //for (int K = 0; K < GridView1.Rows.Count; K++)
            //{
            //    if (orig_name.ToLower().Contains("GridView1_t11")) ((TextBox)GridView1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
            //    ((TextBox)GridView1.Rows[K].FindControl("GridView1_t10")).Attributes.Add("readonly", "readonly");
            //    ((TextBox)GridView1.Rows[K].FindControl("GridView1_t11")).Attributes.Add("readonly", "readonly");
            //    ((TextBox)GridView1.Rows[K].FindControl("GridView1_t16")).Attributes.Add("readonly", "readonly");
            //}
            orig_name = orig_name.ToUpper();
            //if (GridView1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
            if (sR == tb_Colm)
            {
                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    GridView1.Columns[sR].Visible = false;
                }
                // Setting Heading Name
                GridView1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    GridView1.HeaderRow.Cells[sR].Width = Convert.ToInt32(mcol_width);
                    GridView1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }

        //txtlbl2.Attributes.Add("readonly", "readonly");
        //txtlbl3.Attributes.Add("readonly", "readonly");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //txtlbl5.Attributes.Add("readonly", "readonly");
        //txtlbl6.Attributes.Add("readonly", "readonly");

        //my_Tabs
        //txtlbl2.Attributes["required"] = "true";
        //txtlbl2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E0FF00");
        // to hide and show to tab panel
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {

            //case "M12008":
            //    tab3.Visible = false;
            //    tab4.Visible = false;
            //    break;
            //case "F60101":
            //    AllTabs.Visible = false;
            //    break;
        }

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //--------------------------------
    protected void btnRep4_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        query = "";
        hfbtnmode.Value = "SUMM4";
        query = "SELECT distinct TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD') AS FSTR,'Paid' AS TYPE,A.CO_CD AS COMP_CODE,E.FULL_NAME AS COMP_NAME,A.NUM1 AS NO_OF_DEL,A.COL3 AS AMT,A.COL7 AS CONTACT_PER,A.COL8 AS EMAIL_ID,A.COL9 AS MOBILE,nvl(A.COL14,'-') AS EMAIL_SENT,nvl(A.PSTATUS,'-') as pstatus FROM GST A,EVAS E WHERE UPPER(TRIM(USERNAME))=UPPER(TRIM(A.CO_CD)) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='G1' AND A.VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') ORDER BY FSTR";
        //query = "SELECT a.FSTR,a.type,a.COMP_CODE,E.FULL_NAME AS COMP_NAME,a.NO_OF_DEL ,a.AMT,a.CONTACT_PER,a.EMAIL_ID,a.MOBILE,a.DELEGATE_NAME,a.DESIGNATION,a.DELEGATE_MOBILE,a.SRNO,a.EMAIL_SENT,A.PSTATUS FROM (SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Paid' as type ,CO_CD AS COMP_CODE,num1 AS NO_OF_DEL,COL3 AS AMT,COL7 AS CONTACT_PER,COL8 AS EMAIL_ID,COL9 AS MOBILE,COL1 AS DELEGATE_NAME,COL10 AS DESIGNATION,COL12 AS DELEGATE_MOBILE,SRNO,COL14 AS EMAIL_SENT,A.PSTATUS FROM GST A WHERE BRANCHCD='" + mbr + "' AND TYPE='G1' AND A.VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') UNION ALL SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'YYYYMMDD') AS FSTR,'Free' as type, COCODE AS COMP_CODE,1 AS NO_OF_DEL,'-' AS AMT,'-' AS CONTACT_PER,EMAILID AS EMAIL_ID,'-' AS MOBILE,VNAME AS DELEGATE_NAME,DESG AS DESIGNATION,MOBILE AS DELEGATE_MOBILE,SRNO,nvl(col14,'-') AS EMAIL_SENT,'-' AS PSTATUS FROM SEMINAR WHERE BRANCHCD='" + mbr + "' AND TYPE='00' AND VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND SRNO=1) A,EVAS E WHERE UPPER(TRIM(E.USERNAME))=UPPER(TRIM(A.COMP_CODE)) ORDER BY COMP_CODE,srno";
        query = "SELECT TRIM(a.BRANCHCD)||TRIM(a.TYPE)||TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'YYYYMMDD') AS FSTR,'Paid' as type,a.COCODE AS COMP_CODE,E.FULL_NAME AS COMP_NAME,a.EMAILID AS EMAIL_ID,a.VNAME AS DELEGATE_NAME,a.DESG AS DESIGNATION,a.MOBILE AS DELEGATE_MOBILE,a.SRNO,nvl(a.col14,'-') AS EMAIL_SENT,'-' AS PSTATUS FROM SEMINAR a, EVAS E WHERE UPPER(TRIM(E.USERNAME))=UPPER(TRIM(A.Cocode)) and nvl(a.paid_entry,'-')='Y' and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='00' AND a.VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND a.SRNO=1 ORDER BY COMP_CODE,srno";
        query = "SELECT TRIM(a.BRANCHCD)||TRIM(a.TYPE)||TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'YYYYMMDD') AS FSTR,'Paid' as type,a.COCODE AS COMP_CODE,A.CONAME AS COMP_NAME,a.EMAILID AS EMAIL_ID,a.VNAME AS DELEGATE_NAME,a.DESG AS DESIGNATION,a.MOBILE AS DELEGATE_MOBILE,a.SRNO,nvl(a.col14,'-') AS EMAIL_SENT,'-' AS PSTATUS,A.GENT_BY,TO_CHAR(A.GENT_dT,'DD/MM/YYYY HH:MM:SS') AS GENT_dT FROM SEMINAR a  WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='00' AND a.VCHDATE>=TO_DATE('18/09/2019','DD/MM/YYYY') AND a.SRNO=1 and nvl(a.paid_entry,'-')='Y' ORDER BY COMP_CODE,srno";

        ViewState["SSQUERY"] = query;
        // bindval();
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, query);
        create_tab();
        sg1_dr = null;
        for (int d = 0; d < dt.Rows.Count; d++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f5"] = dt.Rows[d]["fstr"].ToString().Trim();
            sg1_dr["sg1_f6"] = dt.Rows[d]["type"].ToString().Trim();
            sg1_dr["sg1_f7"] = dt.Rows[d]["COMP_CODE"].ToString().Trim();
            sg1_dr["sg1_f8"] = dt.Rows[d]["COMP_NAME"].ToString().Trim();
            // sg1_dr["sg1_f9"] = dt.Rows[d]["NO_OF_DEL"].ToString().Trim();
            //sg1_dr["sg1_f10"] = dt.Rows[d]["AMT"].ToString().Trim();
            // sg1_dr["sg1_f11"] = dt.Rows[d]["CONTACT_PER"].ToString().Trim();
            sg1_dr["sg1_f12"] = dt.Rows[d]["EMAIL_ID"].ToString().Trim();
            //  sg1_dr["sg1_f13"] = dt.Rows[d]["MOBILE"].ToString().Trim();
            sg1_dr["sg1_f14"] = dt.Rows[d]["DELEGATE_NAME"].ToString().Trim();
            sg1_dr["sg1_f15"] = dt.Rows[d]["DESIGNATION"].ToString().Trim();
            sg1_dr["sg1_f16"] = dt.Rows[d]["DELEGATE_MOBILE"].ToString().Trim();
            sg1_dr["sg1_f17"] = dt.Rows[d]["SRNO"].ToString().Trim();
            sg1_dr["sg1_f18"] = dt.Rows[d]["EMAIL_SENT"].ToString().Trim();
            sg1_dr["sg1_f19"] = dt.Rows[d]["PSTATUS"].ToString().Trim();
            sg1_dr["sg1_f20"] = dt.Rows[d]["GENT_BY"].ToString().Trim();
            sg1_dr["sg1_f21"] = dt.Rows[d]["GENT_dT"].ToString().Trim();
            sg1_dt.Rows.Add(sg1_dr);
        }
        // sg1_add_blankrows();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        GridView1.DataSource = sg1_dt;
        GridView1.DataBind();
        txtrows.Value = "Showing " + dt.Rows.Count + " Rows ";
        lblshow.Text = "Showing " + sg1_dt.Rows.Count + " Rows ";
        lblshow.Text = "";
        Session["dt"] = sg1_dt;
        // dt.Dispose(); sg1_dt.Dispose();      
        double totDel = 0;
        foreach (GridViewRow gr in GridView1.Rows)
        {
            totDel += fgen.make_double(gr.Cells[10].Text);
        }
        mq1 = "select  COUNT(*) AS DD from seminar  WHERE BRANCHCD='00' AND TYPE='00' AND NVL(PAID_eNTRY,'-')!='N'";
        totDel = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, mq1, "DD"));
        txtTotDel.Text = "Total Paid Delegates : " + totDel.ToString();
        return;
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        string emailto = "", fstr = "", cond = "", tabname = "", fieldName = "", mailSubject = "", mailBody = "";
        string html2 = "";
        string attchment = "";
        CrystalDecisions.CrystalReports.Engine.ReportDocument report;
        report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        foreach (GridViewRow gr in GridView1.Rows)
        {
            if (((CheckBox)gr.FindControl("chkno")).Checked)
            {
                subject = "Confirmation Mail for GST-Conclave and Entry Pass";
                html2 = "";
                html2 = "<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>";
                html2 = html2 + "<div style='text-align:center'><u><b>CONFIRMATION MAIL CUM ENTRY PASS</u></b></div>";
                html2 = html2 + "Dear Sir/Mam,<br/>";
                html2 = html2 + "Greetings of the Day!!<br/>";
                html2 = html2 + "We thank you for registering yourself for the ensuing Conclave.<br/>";
                html2 = html2 + "Attached Please find the  Entry Pass for the same.<br/>";
                html2 = html2 + "Looking forward to welcome you at the venue.<br/><br/>";
                html2 = html2 + "Your Sincerely<br/>";
                html2 = html2 + "<b>Team Tejaxo</b><br><br>";
                html2 = html2 + "<a href=\"www.pocketdriver.in\" target=\"_blank\">www.pocketdriver.in</a> | email :<a href=\"coordination@pocketdriver.in\" target=\"_blank\">coordination@pocketdriver.in </a> | Mobile : 9310008916 <br> ";
                html2 = html2 + "Support numbers 9015-220-220 (10 Lines) |, <br>";
                html2 = html2 + "We make Software, for increasing the Smoothness of your Business Operations., <br>";
                html2 = html2 + "Pocketdriver Limited, the OEM of Tejaxo ERP packages, <br><br>";
                html2 = html2 + "<b>Note: This is the system generated E-Mail. Please do not Reply in case any clarification contact Finsys Team</b><br><br>";

                html2 = html2 + "</body></html>";

                if (hfbtnmode.Value == "SUMM1" && gr.Cells[15].Text != "Y")
                {
                    #region mail not sent at
                    fgen.msg("-", "AMSG", "Please select data from Summary for sending mail");
                    return;
                    emailto = gr.Cells[15].Text;
                    fstr = gr.Cells[6].Text;
                    cond = " TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')||trim(A.COL1)";
                    tabname = "GST";
                    fieldName = "A.COL13";

                    oStream1 = new MemoryStream();
                    DataSet ds = new DataSet();
                    da = new OracleDataAdapter("SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')||trim(A.COL1) AS FSTR,A.CO_CD AS COMP_CODE,E.FULL_NAME AS COMP_NAME,A.COL2 AS NO_OF_DEL,A.COL3 AS AMT,A.COL4 AS PAYMENT_DATE,A.COL11 AS PAYMENT_MODE,(CASE WHEN A.COL11='NEFT' THEN A.COL5||A.COL6 else A.COL6 END) AS NO,A.COL7 AS CONTACT_PER,A.COL8 AS EMAIL_ID,A.COL9 AS MOBILE,A.COL1 AS DELEGATE_NAME,A.COL10 AS DESIGNATION,A.COL12 AS DELEGATE_MOBILE,A.SRNO,A.COL13 AS EMAIL_SENT FROM GST A,EVAS E WHERE UPPER(TRIM(USERNAME))=UPPER(TRIM(A.CO_CD)) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='G1' AND " + cond + "='" + fstr + "' ", con);
                    da.Fill(ds, "Prepcur");

                    xmlpath = Server.MapPath("~/xmlfile/crpt_FinsGST.xml");
                    ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);

                    rptpath = "~/Report/crpt_FinsGST.rpt";
                    report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rptfilepath = Server.MapPath("" + rptpath + "");
                    report.Load(rptfilepath);
                    report.SetDataSource(ds);
                    CRV1.ReportSource = report;
                    CRV1.DataBind();
                    oStream1 = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    #endregion
                }
                if (hfbtnmode.Value == "SUMM2" && gr.Cells[15].Text != "Y")
                {
                    #region free
                    tabname = "SEMINAR";
                    emailto = gr.Cells[13].Text;
                    fstr = gr.Cells[6].Text;
                    cond = " TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')";
                    fieldName = "A.COL14";
                    oStream1 = new MemoryStream();
                    DataSet ds = new DataSet();
                    query = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')||TRIM(VNAME) AS FSTR,TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD') AS FSTR1,'Free' AS DELE_TYPE,A.COCODE AS COMP_CODE,A.CONAME AS COMP_NAME,0 AS NO_OF_DEL,'-' AS AMT,'-' AS PAYMENT_DATE,'-' AS PAYMENT_MODE,'-' AS NO,'-' AS CONTACT_PER,A.EMAILID AS EMAIL_ID,'-' AS MOBILE,A.VNAME AS DELEGATE_NAME,A.DESG AS DESIGNATION,A.MOBILE AS DELEGATE_MOBILE,A.SRNO,NVL(A.COL14,'-') AS EMAIL_SENT FROM SEMINAR a WHERE  nvl(a.paid_entry,'-')!='Y' AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='00' AND " + cond + "='" + fstr + "' AND A.COCODE='NEWC' order by srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, query);
                    dt1 = new DataTable();
                    dt1 = dt.Clone();
                    dt1.Columns.Add(new DataColumn("QR", typeof(System.Byte[])));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region
                        dr1 = dt1.NewRow();
                        col1 = fstr + dt.Rows[i]["SRNO"].ToString().Trim();
                        fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");
                        FilStr = new FileStream(fpath, FileMode.Open);
                        BinRed = new BinaryReader(FilStr);
                        //dr1["img1_desc"] = col1.Trim();
                        dr1["QR"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                        FilStr.Close();
                        BinRed.Close();
                        dr1["FSTR"] = dt.Rows[i]["fstr"].ToString().Trim();
                        dr1["DELE_TYPE"] = dt.Rows[i]["DELE_TYPE"].ToString().Trim();
                        dr1["COMP_CODE"] = dt.Rows[i]["COMP_CODE"].ToString().Trim();
                        dr1["COMP_NAME"] = dt.Rows[i]["COMP_NAME"].ToString().Trim();
                        dr1["NO_OF_DEL"] = dt.Rows[i]["NO_OF_DEL"].ToString().Trim();
                        dr1["AMT"] = dt.Rows[i]["AMT"].ToString().Trim();
                        dr1["PAYMENT_DATE"] = dt.Rows[i]["PAYMENT_DATE"].ToString().Trim();
                        dr1["PAYMENT_MODE"] = dt.Rows[i]["PAYMENT_MODE"].ToString().Trim();
                        dr1["NO"] = dt.Rows[i]["NO"].ToString().Trim();
                        dr1["CONTACT_PER"] = dt.Rows[i]["CONTACT_PER"].ToString().Trim();
                        dr1["EMAIL_ID"] = dt.Rows[i]["EMAIL_ID"].ToString().Trim();
                        dr1["MOBILE"] = dt.Rows[i]["MOBILE"].ToString().Trim();
                        dr1["DELEGATE_NAME"] = dt.Rows[i]["DELEGATE_NAME"].ToString().Trim();
                        dr1["DESIGNATION"] = dt.Rows[i]["DESIGNATION"].ToString().Trim();
                        dr1["DELEGATE_MOBILE"] = dt.Rows[i]["DELEGATE_MOBILE"].ToString().Trim();
                        dr1["SRNO"] = dt.Rows[i]["SRNO"].ToString().Trim();
                        dr1["EMAIL_SENT"] = dt.Rows[i]["EMAIL_SENT"].ToString().Trim();
                        dt1.Rows.Add(dr1);
                        #endregion
                    }
                    dt1.TableName = "Prepcur";
                    ds.Tables.Add(dt1);
                    xmlpath = Server.MapPath("~/tej-base/xmlfile/crpt_FinsGST.xml");
                    ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);
                    rptpath = "~/tej-base/Report/crpt_FinsGST.rpt";
                    report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rptfilepath = Server.MapPath("" + rptpath + "");
                    report.Load(rptfilepath);
                    report.SetDataSource(ds);
                    CRV1.ReportSource = report;
                    CRV1.DataBind();
                    // oStream1 = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    #endregion
                }
                if (hfbtnmode.Value == "SUMM3" && gr.Cells[11].Text != "Y")
                {
                    #region
                    oStream1 = null;
                    emailto = gr.Cells[10].Text;
                    fstr = gr.Cells[6].Text;
                    cond = " TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')";
                    tabname = "GST_OW";
                    fieldName = "A.COL5";
                    html2 = "";

                    subject = "Confirmation Mail for GST-Conclave";
                    html2 = "<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>";
                    html2 = html2 + "<div style='text-align:center'><u><b>CONFIRMATION MAIL</u></b></div>";
                    html2 = html2 + "Event :- GST - Live Tejaxo ERP<br/>";
                    html2 = html2 + "Day & Date :- Tuesday , 27th June 2017<br/>";
                    html2 = html2 + "Time & Slot :- 4:29 P.M. - Exact Time followed by dinner.<br/>";
                    html2 = html2 + "<u>Venue</u> :- Hotel Vibe by Lalit , <br/>";
                    html2 = html2 + "Sector - 37 , Mathura Road, Faridabad.<br/><br/>";
                    html2 = html2 + "Dear Sir/Mam, <br/>";
                    html2 = html2 + "We Sincerely Thanks You for registering yourself.<br/>";
                    html2 = html2 + "The Registratoin details are as follows<br/>";
                    html2 = html2 + "Name of Company : <b>" + gr.Cells[7].Text.Trim() + "</b> <br/>";
                    html2 = html2 + "Name of Participant <b>: " + gr.Cells[8].Text.Trim() + "</b> <br/>";
                    html2 = html2 + "looking forward to welcome you at the conclave.<br/>";
                    html2 = html2 + "Request<br/>";
                    html2 = html2 + "1.) This session is exclusively for Owners/Directors/CEO's. <br/>";
                    html2 = html2 + "2.) Please do not bring along or nominated/delegate to your employee.";
                    html2 = html2 + "</body></html>";
                    #endregion
                }
                if (hfbtnmode.Value == "SUMM4" && gr.Cells[15].Text != "Y")
                {//paid 
                    tabname = "SEMINAR";
                    emailto = gr.Cells[13].Text;
                    fstr = gr.Cells[6].Text;
                    cond = " TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')";
                    fieldName = "A.COL14";
                    oStream1 = new MemoryStream();
                    ds = new DataSet();
                    // query = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')||TRIM(A.COL1) AS FSTR,'PAID' AS DELE_TYPE,A.CO_CD AS COMP_CODE,E.FULL_NAME AS COMP_NAME,A.NUM1 AS NO_OF_DEL,A.COL3 AS AMT,A.COL4 AS PAYMENT_DATE,A.COL11 AS PAYMENT_MODE,(CASE WHEN A.COL11='NEFT' THEN A.COL5||A.COL6 ELSE A.COL6 END) AS NO,A.COL7 AS CONTACT_PER,A.COL8 AS EMAIL_ID,A.COL9 AS MOBILE,A.COL1 AS DELEGATE_NAME,A.COL10 AS DESIGNATION,A.COL12 AS DELEGATE_MOBILE,A.SRNO,NVL(A.COL14,'-') AS EMAIL_SENT FROM GST A,EVAS E WHERE UPPER(TRIM(E.USERNAME))=UPPER(TRIM(A.CO_CD)) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='G1' AND " + cond + "='" + fstr + "'";
                    query = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')||TRIM(VNAME) AS FSTR,TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD') AS FSTR1,'Paid' AS DELE_TYPE,A.COCODE AS COMP_CODE,E.FULL_NAME AS COMP_NAME,0 AS NO_OF_DEL,'-' AS AMT,'-' AS PAYMENT_DATE,'-' AS PAYMENT_MODE,'-' AS NO,'-' AS CONTACT_PER,A.EMAILID AS EMAIL_ID,'-' AS MOBILE,A.VNAME AS DELEGATE_NAME,A.DESG AS DESIGNATION,A.MOBILE AS DELEGATE_MOBILE,A.SRNO,NVL(A.COL14,'-') AS EMAIL_SENT FROM SEMINAR a, EVAS E WHERE UPPER(TRIM(E.USERNAME))=UPPER(TRIM(A.Cocode)) and nvl(a.paid_entry,'-')='Y' AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='00' AND " + cond + "='" + fstr + "' order by srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, query);
                    dt1 = new DataTable();
                    dt1 = dt.Clone();
                    dt1.Columns.Add(new DataColumn("QR", typeof(System.Byte[])));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region
                        dr1 = dt1.NewRow();
                        col1 = fstr + dt.Rows[i]["SRNO"].ToString().Trim();//FOR QR
                        fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");
                        FilStr = new FileStream(fpath, FileMode.Open);
                        BinRed = new BinaryReader(FilStr);
                        //dr1["img1_desc"] = col1.Trim();
                        dr1["QR"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                        FilStr.Close();
                        BinRed.Close();
                        dr1["FSTR"] = dt.Rows[i]["fstr"].ToString().Trim();
                        dr1["DELE_TYPE"] = dt.Rows[i]["DELE_TYPE"].ToString().Trim();
                        dr1["COMP_CODE"] = dt.Rows[i]["COMP_CODE"].ToString().Trim();
                        dr1["COMP_NAME"] = dt.Rows[i]["COMP_NAME"].ToString().Trim();
                        dr1["NO_OF_DEL"] = dt.Rows[i]["NO_OF_DEL"].ToString().Trim();
                        dr1["AMT"] = dt.Rows[i]["AMT"].ToString().Trim();
                        dr1["PAYMENT_DATE"] = dt.Rows[i]["PAYMENT_DATE"].ToString().Trim();
                        dr1["PAYMENT_MODE"] = dt.Rows[i]["PAYMENT_MODE"].ToString().Trim();
                        dr1["NO"] = dt.Rows[i]["NO"].ToString().Trim();
                        dr1["CONTACT_PER"] = dt.Rows[i]["CONTACT_PER"].ToString().Trim();
                        dr1["EMAIL_ID"] = dt.Rows[i]["EMAIL_ID"].ToString().Trim();
                        dr1["MOBILE"] = dt.Rows[i]["MOBILE"].ToString().Trim();
                        dr1["DELEGATE_NAME"] = dt.Rows[i]["DELEGATE_NAME"].ToString().Trim();
                        dr1["DESIGNATION"] = dt.Rows[i]["DESIGNATION"].ToString().Trim();
                        dr1["DELEGATE_MOBILE"] = dt.Rows[i]["DELEGATE_MOBILE"].ToString().Trim();
                        dr1["SRNO"] = dt.Rows[i]["SRNO"].ToString().Trim();
                        dr1["EMAIL_SENT"] = dt.Rows[i]["EMAIL_SENT"].ToString().Trim();
                        dt1.Rows.Add(dr1);
                        #endregion
                    }
                    dt1.TableName = "Prepcur";
                    ds.Tables.Add(dt1);
                    xmlpath = Server.MapPath("~/tej-base/xmlfile/crpt_FinsGST.xml");
                    ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);

                    rptpath = "~/tej-base/Report/crpt_FinsGST.rpt";
                    report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rptfilepath = Server.MapPath("" + rptpath + "");
                    report.Load(rptfilepath);
                    report.SetDataSource(ds);
                    CRV1.ReportSource = report;
                    CRV1.DataBind();
                }
                mailBody = html2;
                //  emailto = "YOGITA@pocketdriver.in";           
                string mailstatus = "";
                if (hfbtnmode.Value == "SUMM3")
                    mailstatus = fgen.send_mail(frm_cocd, "", emailto, "", "", subject, mailBody);
                else
                {
                    Attachment atchfile = new Attachment(report.ExportToStream(ExportFormatType.PortableDocFormat), frm_cocd + "_" + subject + ".pdf");
                    mailstatus = fgen.send_mail(frm_qstr, frm_cocd, "Tejaxo ERP", emailto, "", "", subject, mailBody, atchfile, "1");
                }
                if (mailstatus == "Y" || mailstatus == "1")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + tabname + " A SET " + fieldName + "='Y' WHERE " + cond + "='" + fstr + "'");
                }
            }
        }
        return;
    }

    public void del_file(string path)
    {
        try
        {
            fpath = Server.MapPath(path);
            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        srchMthd();
        #region
        // clearcontrol();

        //sstring = "";
        //dt = new DataTable();

        //sstring = txtsearch.Text.Trim().ToString();
        //if (sstring == "")
        //    dt = (DataTable)ViewState["sg1"];
        //else
        //{
        //    query = "";
        //    query = (string)ViewState["SSQUERY"];
        //    da = new OracleDataAdapter(query, con);
        //    dt = new DataTable();
        //    dt = fgen.getdata(frm_qstr, co_cd, query);
        //    dt1 = new DataTable();
        //    dt1 = fgen.searchDataTable(sstring, dt);
        //    dt = new DataTable();
        //    dt = dt1;
        //}
        //if (dt.Rows.Count > 0)
        //{
        //    if (hfbtnmode.Value == "SUMM4")
        //    {
        //        GridView1.DataSource = dt;
        //        GridView1.DataBind();
        //        GridView1.Visible = true;
        //    }
        //    ViewState["sg1"] = dt;
        //    lblshow.Text = "";
        //    lblshow.Text = "Shwoing " + dt.Rows.Count + " Rows ";
        //}
        //else
        //    AlertMsg("AMSG", "search criteria does not match!");
        #endregion
    }
    void srchMthd()
    {
        DataTable dt1 = new DataTable();
        sstring = txtsearch.Text.Trim().ToString();
        if (sstring == "")
            dt = (DataTable)ViewState["sg1"];
        else
        {
            query = (string)ViewState["SSQUERY"];
            hfqry.Value = query;
            if (hfqry.Value.Length > 5)
            {
                query = hfqry.Value;
            }
            else if (Session["sg1"] != null)
            {
                dt = new DataTable();
                dt = (DataTable)ViewState["sg1"];
            }
            if (txtsearch.Text == "")
            {
                if (query.Length > 5)
                {
                    dt1 = fgen.getdata(frm_qstr, co_cd, "select * from ( " + query + " ) where rownum<=" + lblshow.Text.Trim() + "");
                }
                else
                {
                    dt1 = fgen.searchDataTable(txtsearch.Text, dt);
                }
            }
            else
            {
                if (query.Length > 5)
                {
                    dt1 = fgen.search_vip(frm_qstr, co_cd, query, txtsearch.Text.Trim().ToUpper());
                }
                else
                {
                    dt1 = fgen.searchDataTable(txtsearch.Text, dt);
                }
            }
            if (dt1.Rows.Count > 0)
            { //for details
                #region
                create_tab();
                sg1_dr = null;
                for (int d = 0; d < dt1.Rows.Count; d++)
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_f5"] = dt1.Rows[d]["fstr"].ToString().Trim();
                    sg1_dr["sg1_f6"] = dt1.Rows[d]["type"].ToString().Trim();
                    sg1_dr["sg1_f7"] = dt1.Rows[d]["COMP_CODE"].ToString().Trim();
                    sg1_dr["sg1_f8"] = dt1.Rows[d]["COMP_NAME"].ToString().Trim();
                    sg1_dr["sg1_f12"] = dt1.Rows[d]["EMAIL_ID"].ToString().Trim();
                    sg1_dr["sg1_f14"] = dt1.Rows[d]["DELEGATE_NAME"].ToString().Trim();
                    sg1_dr["sg1_f15"] = dt1.Rows[d]["DESIGNATION"].ToString().Trim();
                    sg1_dr["sg1_f16"] = dt1.Rows[d]["DELEGATE_MOBILE"].ToString().Trim();
                    sg1_dr["sg1_f17"] = dt1.Rows[d]["SRNO"].ToString().Trim();
                    sg1_dr["sg1_f18"] = dt1.Rows[d]["EMAIL_SENT"].ToString().Trim();
                    sg1_dr["sg1_f19"] = dt1.Rows[d]["PSTATUS"].ToString().Trim();
                    sg1_dr["sg1_f20"] = dt1.Rows[d]["GENT_BY"].ToString().Trim();
                    sg1_dr["sg1_f21"] = dt1.Rows[d]["GENT_dT"].ToString().Trim();
                    sg1_dt.Rows.Add(sg1_dr);
                }
                ViewState["sg1"] = sg1_dt;
                GridView1.DataSource = sg1_dt;
                GridView1.DataBind();
                #endregion
            }
            datadiv.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt1, "").ToString(), false);
            if (dt1.Rows.Count > 0)
            {
                datadiv.Visible = true; //div2.Visible = false;
                lblshow.Text = "Total Rows : " + dt1.Rows.Count;
            }
            else
            {
                datadiv.Visible = false; //div2.Visible = true;
                lblshow.Text = "Total Rows : " + dt1.Rows.Count;
                GridView1.DataSource = null;
                GridView1.DataBind();
                ViewState["sg1"] = null;
                //fgen.msg("-", "AMSG", "No Data Found");
                // return;
            }
            dt1.Dispose();
        }
    }
    protected void radOk_Click(object sender, EventArgs e)
    {
        //RadioButton radOk = (RadioButton)sender;
        //GridViewRow row = (GridViewRow)radOk.Parent.Parent;
        //col1 = GridView1.Rows[row.RowIndex].Cells[6].Text.Trim().ToString();
        //fgen.execute_cmd(co_cd, "UPDATE GST SET PSTATUS='Y' WHERE BRANCHCD||tYPE||TRIM(vCHNUM)||TO_CHAR(vCHDATE,'YYYYMMDD')='" + col1 + "'");
    }
}