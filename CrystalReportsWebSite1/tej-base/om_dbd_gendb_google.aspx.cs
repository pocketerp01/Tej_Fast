using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Text;

public partial class om_dbd_gendb_google : System.Web.UI.Page
{
    string DateRange, PrdRange, sQuery, chartScript;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    string val_legnd, popUpType = "", cond;
    string this_mth = "", mdt2_memv = "";
    string squery1, squery2, squery3, squery4, squery5, squery6;
    string stitle1, stitle2, stitle3, stitle4, stitle5, stitle6;
    string wdays, CSR;
    string Client_Code;
    string Prg_Id; int ID = 0;
    DataTable dt, dt1, dt2, dt3, dt4, iconDt;
    fgenDB fgen = new fgenDB();
    int kz = 0; DateTime date1;
    string xday, xmonth, xyear, xselected_date, scode, mtnno, xdt, er1, er2;
    string f1, f2, f3, f4, ppmdate, pmdate, sysdt, lmdt, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, xprdrange, xprdrange1;
    double nmth1, nmth2, totamt, salemat = 0; string popsql, todt, fromdt;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
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
                    PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                }
                else Response.Redirect("~/login.aspx");
            }
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (!Page.IsPostBack)
            {
                //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_BYPASS1") == "Y")
                //{
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_BYPASS1", "N");
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", PrdRange);

                //    printHeads();
                //    printGraph();
                //}
                //else
                //{
                //    if (Prg_Id == "******")
                //    {
                //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "PARTY");
                //    }
                //    else fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                //    askPopUp();
                //}
                iconDt = new DataTable();
                iconDt = fgen.getdata(frm_qstr, frm_cocd, "select distinct id from FIN_MSYS where id in ('F05365','F05366','F05367','F05368','F05369','F05370') order by id");
                ViewState["icodeDt"] = iconDt;
                printGraph();
            }
        }
    }

    void askPopUp()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        popUpType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_POPUPTYPE");
        switch (Prg_Id)
        {
            case "S15115G":
                if (popUpType == "PARTY")
                {
                    sQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
                    fgen.Fn_open_sseek("", frm_qstr);
                }
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;

            case "P17005A":
                if (popUpType == "PARTY")
                {
                    sQuery = "SELECT Vchnum,Name,Vchnum AS CODE,proj_refno as Ref FROM proj_dtl WHERE branchcd!='DD' ORDER BY Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
                    fgen.Fn_open_sseek("", frm_qstr);
                }
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;

            default:
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;
        }
    }

    void printHeads()
    {
        mdt2_memv = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        this_mth = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + mdt2_memv + "','dd/mm/yyyy'),'yyyymm') as cmth from dual", "cmth");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

        if (Prg_Id == "F05191") { Prg_Id = "F15152"; }
        if (Prg_Id == "F05192") { Prg_Id = "F15156"; }
        if (Prg_Id == "F05193") { Prg_Id = "F20141"; }
        if (Prg_Id == "F05194") { Prg_Id = "F25176"; }
        if (Prg_Id == "F05195") { Prg_Id = "F25181"; }
        if (Prg_Id == "F05196") { Prg_Id = "F30152"; }
        if (Prg_Id == "F05197") { Prg_Id = "F47152"; }
        if (Prg_Id == "F05198") { Prg_Id = "F50152"; }

        string db_query;
        string db_sql;

        Client_Code = "%";

        switch (Prg_Id)
        {
            case "F05365":
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, squery1);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Sales Breakup (Top 10 Parties)", "chart1");
                }
                break;
        }
        //db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE1'", "dbval");
        //if (db_query.Contains("@"))
        //{
        //    lblBox1Header.Text = db_query.Split('@')[0].ToString();
        //    db_sql = db_query.Split('@')[1].ToString();
        //    db_sql = db_sql.Replace("`", "'");
        //    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
        //    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
        //    db_sql = db_sql.Replace("DT_RANGE", DateRange);
        //    db_sql = db_sql.Replace("CURR_MTH", this_mth);

        //    lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
        //}

        //db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE2'", "dbval");
        //if (db_query.Contains("@"))
        //{
        //    lblBox2Header.Text = db_query.Split('@')[0].ToString();
        //    db_sql = db_query.Split('@')[1].ToString();
        //    db_sql = db_sql.Replace("`", "'");
        //    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
        //    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
        //    db_sql = db_sql.Replace("DT_RANGE", DateRange);
        //    db_sql = db_sql.Replace("CURR_MTH", this_mth);

        //    lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
        //}

        //db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE3'", "dbval");
        //if (db_query.Contains("@"))
        //{
        //    lblBox3Header.Text = db_query.Split('@')[0].ToString();
        //    db_sql = db_query.Split('@')[1].ToString();
        //    db_sql = db_sql.Replace("`", "'");
        //    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
        //    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
        //    db_sql = db_sql.Replace("DT_RANGE", DateRange);
        //    db_sql = db_sql.Replace("CURR_MTH", this_mth);

        //    lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
        //}

        //db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE4'", "dbval");
        //if (db_query.Contains("@"))
        //{
        //    lblBox4Header.Text = db_query.Split('@')[0].ToString();
        //    db_sql = db_query.Split('@')[1].ToString();
        //    if (db_sql.Substring(0, 7) == "PERCENT")
        //    {
        //        if (fgen.make_double(lblBox1Count.Text) > 0)
        //            lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select round((" + lblBox3Count.Text + " / " + lblBox1Count.Text + ")*100,2) as cnt1 from dual ", "cnt1") + " %";
        //    }
        //    else
        //    {
        //        db_sql = db_sql.Replace("`", "'");
        //        db_sql = db_sql.Replace("BR_VAR", frm_mbr);
        //        db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
        //        db_sql = db_sql.Replace("DT_RANGE", DateRange);
        //        db_sql = db_sql.Replace("CURR_MTH", this_mth);
        //        lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
        //    }

        //}
    }

    void printGraph()
    {
        // mdt2_memv = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        mdt2_memv = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
        this_mth = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + mdt2_memv + "','dd/mm/yyyy'),'yyyymm') as cmth from dual", "cmth");
        val_legnd = "Value";
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        dt = new DataTable();
        sysdt = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        chartDiv1.Visible = true; chartDiv2.Visible = true; chartDiv3.Visible = true; chartDiv4.Visible = true; chartDiv5.Visible = true; chartDiv6.Visible = true;
        chartDiv1.Attributes.Add("class", "col-lg-6"); chartDiv2.Attributes.Add("class", "col-lg-6");
        chartDiv3.Attributes.Add("class", "col-lg-6"); chartDiv4.Attributes.Add("class", "col-lg-6");
        chartDiv5.Attributes.Add("class", "col-lg-6"); chartDiv6.Attributes.Add("class", "col-lg-6");
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        string db_query;
        string db_sql;

        switch (Prg_Id)
        {
            #region GRAPH : SALES
            case "F05365": // SALES BREAKUP (TOP 10 PARTIES)
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery1);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Sales Breakup (Top 10 Parties)", "chart1");
                }
                else
                {
                    chartDiv1.Visible = false;
                }

                // SALES VS COLL MONTH WISE
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05229";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery2 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery2);
                if (dt.Rows.Count > 0)
                {
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + sysdt.Substring(6, 4);
                    OpenChart("Bar", "Sales vs Collection Month Wise", "chart2");
                }
                else
                {
                    chartDiv2.Visible = false;
                }

                // NEW SO RECEIVE TREND
                xprdrange1 = " between to_date('" + frm_CDT1.Substring(0, 6) + Convert.ToString(fgen.make_double(frm_CDT1.Substring(6, 4)) - 1) + "','dd/mm/yyyy')-1 and to_Date('" + frm_CDT1 + "','dd/mm/yyyy')-1";
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05232";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART3'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", xprdrange1);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery3 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery3);
                if (dt.Rows.Count > 0)
                {
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + sysdt.Substring(6, 4);
                    OpenChart("Bar", "New SO Received Trend", "chart3");
                }
                else
                {
                    chartDiv3.Visible = false;
                }

                // COMP OF CY SALES TO LAST YEAR
                string ctdt = "", kyrstr = "";
                ctdt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='R01'", "PARAMS").Trim();
                squery4 = "Select to_char(fmdate,'dd/mm/yyyy') as fmdate,to_char(todate,'dd/mm/yyyy') as todate from co where substr(code,1,length(trim(code))-4) like '" + frm_cocd + "' and fmdate " + xprdrange + " order by fmdate";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery4);
                foreach (DataRow dr in dt.Rows)
                {
                    kyrstr = dr["fmdate"].ToString().Trim().Substring(8, 2) + "-" + dr["todate"].ToString().Trim().Substring(8, 2);
                }
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART4'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("YEAR", kyrstr);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery5 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery5);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    StringBuilder sb = new System.Text.StringBuilder();
                    if (dt.Rows.Count == 1)
                    {
                        //sb.Append("Your Tejaxo ERP has data of CY onwards,");
                        //sb.Append("so, this Graph cannot be generated this year");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('" + sb.ToString() + "');", true);
                    }
                    hdnHAxisTitle_Bar.Value = "Year (F.Y Denotes Financial Year)";
                    OpenChartColumn("Bar", "Comparison of CY Sales To Last Year (Totals)", "chart4");
                }
                else
                {
                    chartDiv4.Visible = false;
                }

                // SCHEDULE VS DESPATCH
                //  xdt = sysdt.Substring(6, 4) + sysdt.Substring(3, 2);
                cond = "F05241";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART5'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery6 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery6);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(1);
                    date1 = new DateTime(Convert.ToInt32(sysdt.Substring(6, 4)), Convert.ToInt32(sysdt.Substring(3, 2)), 1);
                    xmonth = date1.ToString("MMMM");
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Customers";
                    OpenChart("Bar", "Schedule vs Despatch (For The Month Of " + xmonth + ")", "chart5");
                }
                else
                {
                    chartDiv5.Visible = false;
                }
                chartDiv5.Attributes.Add("class", "col-lg-12");
                chartDiv6.Visible = false;
                if (chartDiv1.Visible == false && chartDiv2.Visible == false && chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv5.Attributes.Add("class", "col-lg-12"); }
                if (chartDiv3.Visible == false && chartDiv4.Visible == false && chartDiv5.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }
                if (chartDiv5.Visible == false && chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
                break;
            #endregion

            #region GRAPH : FINANCE
            case "F05366": // PURCHASE BREAKUP (TOP 10 VENDORS)
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05264";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery1);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Purchase Breakup (Top 10 Vendors)", "chart1");
                }
                else
                {
                    chartDiv1.Visible = false;
                }

                // EXPENSE TREND
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery2 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery2);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    // hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + fgen.make_double(sysdt.Substring(6, 4));
                    OpenChartColumn("Bar", "Expense Trend", "chart2");
                }
                else
                {
                    chartDiv2.Visible = false;
                }

                chartDiv3.Visible = false;
                chartDiv4.Visible = false;
                chartDiv5.Visible = false;
                chartDiv6.Visible = false;

                if (chartDiv1.Visible == false) { chartDiv2.Attributes.Add("class", "col-lg-12"); }
                if (chartDiv2.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); }

                //if (chartDiv1.Visible == false && chartDiv2.Visible == false && chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv5.Attributes.Add("class", "col-lg-12"); }
                // if (chartDiv3.Visible == false && chartDiv4.Visible == false && chartDiv5.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }
                // if (chartDiv5.Visible == false  && chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
                break;
            #endregion

            #region GRAPH : SALARIES (** ADDED TO THE ICON ID , AS PER SANGEET SIR SALARY GRAPHS ARE NOT REQUIRED)
            case "F05367**": // SALARY BREAKUP DEPARTMENT WISE
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05278";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery1);
                if (dt.Rows.Count > 0)
                {
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Salary Breakup Department Wise", "chart1");
                }
                else
                {
                    chartDiv1.Visible = false;
                }

                // SALARY BREAKUP DESGINATION WISE
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery2 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery2);
                if (dt.Rows.Count > 0)
                {
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Salary Breakup Desgination Wise", "chart2");
                }
                else
                {
                    chartDiv2.Visible = false;
                }

                // SALARY BREAKUP GRADE WISE
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART3'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery3 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery3);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(1);
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Salary Breakup Grade Wise", "chart3");
                }
                else
                {
                    chartDiv3.Visible = false;
                }

                // SALARY TREND
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                if (Convert.ToInt32(sysdt.Substring(3, 2)) > 3 && Convert.ToInt32(sysdt.Substring(3, 2)) < 12) fromdt = "01/04/" + sysdt.Substring(6, 4);
                else
                {
                    fromdt = "01/04/" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) - 1);
                }
                xprdrange1 = " between to_date('" + fromdt.Substring(0, 6) + Convert.ToString(fgen.make_double(fromdt.Substring(6, 4)) - 1) + "','dd/mm/yyyy')-1 and to_Date('" + fromdt + "','dd/mm/yyyy')-1";
                cond = "F05287";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART4'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", xprdrange1);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery4 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery4);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(4);
                    dt.Columns.RemoveAt(3);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    OpenChart("Bar", "Salary Trend", "chart4");
                }
                else
                {
                    chartDiv4.Visible = false;
                }
                chartDiv5.Visible = false;
                chartDiv6.Visible = false;
                if (chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
                if (chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }
                break;
            #endregion

            #region GRAPH : SALES PART 2 .... ICON ID USED IS BELONGS TO SALARIES GRAPHS
            case "F05367":
                // MONTH WISE DEBTOR CLOSING BALANCE
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery1);
                if (dt.Rows.Count > 0)
                {
                    string yr_op = ""; double run_Tot = 0;
                    int k = 0;
                    frm_myear = "yr_" + frm_myear;
                    yr_op = fgen.seek_iname(frm_qstr, frm_cocd, "Select sum(" + frm_myear + ") as opt from famstbal a where  a.branchcd <> 'DD' and substr(a.Acode,1,2)='16'", "opt");
                    if (yr_op == "") yr_op = "0";

                    dt.Columns.Add(new DataColumn("cum_tot", typeof(Decimal)));
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (k == 0)
                            run_Tot = run_Tot + (fgen.make_double(yr_op) + fgen.make_double(dr["tot_bas"].ToString()));
                        else
                            run_Tot = run_Tot + fgen.make_double(dr["tot_bas"].ToString());

                        dr["cum_tot"] = run_Tot;
                        k++;
                    }
                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);
                    dt.Columns.RemoveAt(1);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    //  hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + sysdt.Substring(6, 4);
                    OpenChartColumn("Bar", "Month Wise Debtor Closing Balance", "chart1");
                }
                else
                {
                    chartDiv1.Visible = false;
                }

                // COLLECTION TREND
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery2 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery2);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    // hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + fgen.make_double(sysdt.Substring(6, 4));
                    OpenChartColumn("Bar", "Collection Trend", "chart2");
                }
                else
                {
                    chartDiv2.Visible = false;
                }

                //COMPARISON OF CY SALES TO LAST YEAR(MONTH ON MONTH)
                xprdrange1 = " between to_date('" + frm_CDT1.Substring(0, 6) + Convert.ToString(fgen.make_double(frm_CDT1.Substring(6, 4)) - 1) + "','dd/mm/yyyy')-1 and to_Date('" + frm_CDT1 + "','dd/mm/yyyy')-1";
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05238";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART3'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", xprdrange1);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery3 = db_sql;
                }

                xprdrange1 = " between to_date('" + frm_CDT1.Substring(0, 6) + Convert.ToString(fgen.make_double(frm_CDT1.Substring(6, 4)) - 1) + "','dd/mm/yyyy')-1 and to_Date('" + frm_CDT1 + "','dd/mm/yyyy')-1";
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                squery3 = "select * from (select upper(month_name) as month_name,sum(past_yr) as past_yr,sum(curr_yr) as curr_yr,mthnum,max(mthsno) as mthsno  from (select substr(mthname,1,3) as month_name,0 as past_yr,0 as curr_yr,mthnum,mthsno from mths union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,0 as past_yr,sum(a.amt_sale) as curr_yr,to_Char(a.vchdate,'MM') as mth,0 from sale a where a.vchdate  " + xprdrange + " and a.branchcd <> 'DD' group by to_Char(a.vchdate,'MM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.amt_sale) as past_yr,0 as curr_yr,to_Char(a.vchdate,'MM') as mth,0 from sale a where a.vchdate  " + xprdrange1 + " and a.branchcd <> 'DD' group by to_Char(a.vchdate,'MM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) ) group by upper(month_name),mthnum ) order by mthsno";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery3);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(4);
                    dt.Columns.RemoveAt(3);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + sysdt.Substring(6, 4);
                    OpenChart("Bar", "Comparison Of CY Sales To Last Year (Month On Month)", "chart3");
                }
                else
                {
                    chartDiv3.Visible = false;
                }

                // SALES TREND, CURRENT YEAR
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05276";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART4'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery4 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery4);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    // hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + fgen.make_double(sysdt.Substring(6, 4));
                    OpenChart("Bar", "Sales Trend, Current Year (" + frm_CDT1 + " To " + sysdt + ")", "chart4");
                }
                else
                {
                    chartDiv4.Visible = false;
                }
                chartDiv3.Attributes.Add("class", "col-lg-12");
                chartDiv4.Attributes.Add("class", "col-lg-12");
                chartDiv5.Visible = false;
                chartDiv6.Visible = false;
                if (chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }
                break;
            #endregion

            #region GRAPH : PRODUCTION
            case "F05368": // MAIN QUALITY PROBLEMS
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05289";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                RowtoColumnData(squery1);
                if (dt.Rows.Count > 0)
                {
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Reasons";
                    OpenChartColumn("Pie", "Main Quality Problems", "chart1");
                }
                else
                {
                    chartDiv1.Visible = false;
                }

                // MAIN DOWN TIME REASONS
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05292";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery2 = db_sql;
                }
                RowtoColumnData(squery2);
                if (dt.Rows.Count > 0)
                {
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Reasons";
                    OpenChartColumn("Pie", "Main Down Time Reasons", "chart2");
                }
                else
                {
                    chartDiv2.Visible = false;
                }

                // MONTHLY PRODUCTION PPM
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART3'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery3 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery3);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    OpenChartColumn("Bar", "Monthly Production PPM", "chart3");
                }
                else
                {
                    chartDiv3.Visible = false;
                }

                // MONTHLY DOWN TIME IN HRS
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART4'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery4 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery4);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    OpenChartColumn("Bar", "Monthly Down Time in Hrs", "chart4");
                }
                else
                {
                    chartDiv4.Visible = false;
                }
                chartDiv5.Visible = false;
                chartDiv6.Visible = false;
                chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12");
                if (chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
                if (chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }
                break;
            #endregion

            #region GRAPH : PENDING PRODUCTION
            case "F05369": // PENDING SALES SCHEDULES VS JOB CARD YTD (MONTH WISE)
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05301";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery1);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Pending Sales Sch. Vs Job Card YTD (Month Wise)", "chart1");
                }
                else
                {
                    chartDiv1.Visible = false;
                }

                // PENDING JOB CARDS NOT YET CLOSED YTD (MONTH WISE)
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05304";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery2 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery2);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Pending Job Cards Not Yet Closed YTD (Month Wise)", "chart2");
                }
                else
                {
                    chartDiv2.Visible = false;
                }

                // PENDING SALES SCHEDULES VS JOB CARD YTD (PARTY WISE TOP 10)
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05307";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART3'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery3 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery3);
                if (dt.Rows.Count > 0)
                {
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Pending Sales Schedules Vs Job Card YTD (Party Wise Top 10)", "chart3");
                }
                else
                {
                    chartDiv3.Visible = false;
                }

                // PENDING JOB CARDS (NOT YET CLOSED) YTD (PARTY WISE TOP 10)
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05310";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART4'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery4 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery4);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(2);
                    hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                    OpenChart("Pie", "Pending Job Cards (Not Yet Closed) YTD (Party Wise Top 10)", "chart4");
                }
                else
                {
                    chartDiv4.Visible = false;
                }
                chartDiv5.Visible = false;
                chartDiv6.Visible = false;
                if (chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
                if (chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }
                break;

            #endregion

            #region GRAPH : QC
            case "F05370": // CUSTOMER WISE REJECTION PPM
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05332";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery1 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery1);
                dt1 = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(5);
                    dt.Columns.RemoveAt(4);
                    dt.Columns.RemoveAt(2);
                    dt.Columns.RemoveAt(1);
                    dt1.Columns.Add(new DataColumn("Cust_Name", typeof(string)));
                    dt1.Columns.Add(new DataColumn("rej_val", typeof(decimal)));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow nrow = dt1.NewRow();
                        nrow["Cust_Name"] = dt.Rows[i][0].ToString().Trim();
                        nrow["rej_val"] = dt.Rows[i][1].ToString().Trim();
                        dt1.Rows.Add(nrow);
                    }
                    dt = new DataTable();
                    dt = dt1;
                    hdnVAxisTitle_Bar.Value = "Rejection PPM";
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Customers";
                    OpenChartColumn("Bar", "Customer Wise Rejections PPM", "chart1");
                }
                else
                {
                    chartDiv1.Visible = false;
                }

                // CUSTOMER WISE REJECTIONS PERCENT
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05335";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery2 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery2);
                dt1 = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(5);
                    dt.Columns.RemoveAt(4);
                    dt.Columns.RemoveAt(2);
                    dt.Columns.RemoveAt(1);
                    dt1.Columns.Add(new DataColumn("Cust_Name", typeof(string)));
                    dt1.Columns.Add(new DataColumn("rej_val", typeof(decimal)));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow nrow = dt1.NewRow();
                        nrow["Cust_Name"] = dt.Rows[i][0].ToString().Trim();
                        nrow["rej_val"] = dt.Rows[i][1].ToString().Trim();
                        dt1.Rows.Add(nrow);
                    }
                    dt = new DataTable();
                    dt = dt1;
                    hdnVAxisTitle_Bar.Value = "Rejection Percentage";
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Customers";
                    OpenChartColumn("Bar", "Customer Wise Rejections Percent (These Are The Customers With Worst Rejection-To-Sales Ratio)", "chart2");
                }
                else
                {
                    chartDiv2.Visible = false;
                }

                // SALE VS REJECTION
                xprdrange = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                cond = "F05338";
                db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART3'", "dbval");
                if (db_query.Contains("@"))
                {
                    stitle1 = db_query.Split('@')[0].ToString();
                    db_sql = db_query.Split('@')[1].ToString();
                    db_sql = db_sql.Replace("`", "'");
                    db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                    db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                    db_sql = db_sql.Replace("DT_RANGE", xprdrange);
                    db_sql = db_sql.Replace("CURR_MTH", this_mth);
                    squery3 = db_sql;
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, squery3);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.RemoveAt(5);
                    dt.Columns.RemoveAt(4);
                    dt.Columns.RemoveAt(3);
                    hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                    hdnHAxisTitle_Bar.Value = "Year " + frm_CDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                    OpenChart("Bar", "Sales vs Rejection", "chart3");
                }
                else
                {
                    chartDiv3.Visible = false;
                }
                chartDiv1.Attributes.Add("class", "col-lg-12");
                chartDiv2.Attributes.Add("class", "col-lg-12");
                chartDiv3.Attributes.Add("class", "col-lg-12");
                chartDiv4.Visible = false;
                chartDiv5.Visible = false;
                chartDiv6.Visible = false;
                break;
            #endregion
        }

        kz++;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ1", kz.ToString());
        if (chartDiv1.Visible == false && chartDiv2.Visible == false && chartDiv3.Visible == false && chartDiv4.Visible == false && chartDiv5.Visible == false && chartDiv6.Visible == false)
        {
            timer1_Tick("", EventArgs.Empty);
        }
    }

    protected void btnBox1_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_RPT_CODE", "RPT1");
        pop_func();
    }

    protected void btnBox2_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_RPT_CODE", "RPT2");
        pop_func();
    }

    protected void btnBox3_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_RPT_CODE", "RPT3");
        pop_func();
    }

    protected void btnBox4_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_RPT_CODE", "RPT4");
        pop_func();
    }

    void pop_func()
    {
        string db_query;
        string db_sql;
        string stitle1;
        string rpt_cd;
        mdt2_memv = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        this_mth = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + mdt2_memv + "','dd/mm/yyyy'),'yyyymm') as cmth from dual", "cmth");

        popUpType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_POPUPTYPE");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        rpt_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_RPT_CODE");

        if (Prg_Id == "F05191") { Prg_Id = "F15152"; }
        if (Prg_Id == "F05192") { Prg_Id = "F15156"; }
        if (Prg_Id == "F05193") { Prg_Id = "F20141"; }
        if (Prg_Id == "F05194") { Prg_Id = "F25176"; }
        if (Prg_Id == "F05195") { Prg_Id = "F25181"; }
        if (Prg_Id == "F05196") { Prg_Id = "F30152"; }
        if (Prg_Id == "F05197") { Prg_Id = "F47152"; }
        if (Prg_Id == "F05198") { Prg_Id = "F50152"; }

        db_sql = "Select Sysdate from dual";
        stitle1 = "";
        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='" + rpt_cd + "'", "dbval");
        if (db_query.Contains("@"))
        {
            stitle1 = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", db_sql);
        fgen.Fn_open_rptlevel(stitle1, frm_qstr);
        printGraph();
        printHeads();
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        askPopUp();
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        printGraph();
        printHeads();
    }

    public void RowtoColumnData(string query)
    {
        //squery1 = squery1 + " from prod_sheet a where a.vchdate " + xprdrange + " and a.branchcd <> 'DD' and a.type in ('86','88') ";
        sQuery = query;
        dt1 = new DataTable();
        dt1 = fgen.getdata(frm_qstr, frm_cocd, sQuery);

        dt2 = new DataTable();
        dt = new DataTable();
        if (cond == "F05292")
        {
            squery5 = "select * from(Select TYPE1,name from type where id='4'  order by type1) where rownum<13";
        }
        else if (cond == "F05289")
        {
            squery5 = "select * from(Select TYPE1,name from type where id='8'  order by type1) where rownum<13";
        }
        dt2 = fgen.getdata(frm_qstr, frm_cocd, squery5);

        dt.Columns.Add(new DataColumn("month_Name", typeof(string)));
        dt.Columns.Add(new DataColumn("TOT_BAS", typeof(Decimal)));

        int jq = 0;
        for (int i = 0; i < dt2.Rows.Count; i++)
        {
            try
            {
                if (fgen.make_double(dt1.Rows[0][jq].ToString().Trim()) <= 0) { }
                else
                {
                    DataRow nrow = dt.NewRow();
                    nrow["month_Name"] = dt2.Rows[jq]["name"].ToString().Trim();
                    nrow["tot_bas"] = dt1.Rows[0][jq].ToString().Trim();
                    dt.Rows.Add(nrow);
                }
                jq = jq + 1;
            }
            catch { }
        }
    }

    public string DataTableToJSArray_Top10(DataTable dt, string mode)
    {
        // WHEN WE HAVE TO SHOW DATA OF ANY TOP 10 
        double icount = 0;
        string rowDataStr = "", colStr = "";
        StringBuilder sb = new StringBuilder();
        sb.Append("[");

        if (dt.Rows.Count > 0)
        {
            //add header
            colStr = "";
            icount = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                if (colStr.Length > 0)
                    colStr += ",";
                switch (mode)
                {
                    case "F05256":
                        dc.ColumnName = "% Mthly Avg";
                        break;
                    case "F05259":
                        dc.ColumnName = "% Mth Avg";
                        break;
                    case "F05262":
                        dc.ColumnName = "% Yr.Target";
                        break;
                }
                colStr += "'" + dc.ColumnName + "'";
            }
            sb.Append("[" + colStr + "]");
            icount = 0;
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                DataRow dr = dt.Rows[count];
                rowDataStr = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (rowDataStr.Length > 0)
                        rowDataStr += ",";
                    if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                    {
                        if (count > 9)
                        {
                            icount = icount + fgen.make_double(dr[dc].ToString().Trim());
                        }
                        else
                            rowDataStr += dr[dc].ToString().Trim();
                    }
                    else
                    {
                        if (count > 9) { }
                        else rowDataStr += "'" + dr[dc].ToString().Trim() + "'";
                    }

                    rowDataStr = rowDataStr.Replace("\r\n", "").Trim();
                }
                if (icount > 0) { }
                else
                {
                    sb.Append(",");
                    sb.Append("[" + rowDataStr + "]");
                }
            }
        }
        if (icount == 0)
        {
            sb.Append("]");
        }
        else
        {
            rowDataStr = "";
            rowDataStr += "'OTHERS'";
            rowDataStr += ",";
            rowDataStr += icount.ToString();
            sb.Append(",");
            sb.Append("[" + rowDataStr + "]");
            sb.Append("]");
        }
        return sb.ToString();
    }

    public string DataTableToJSArray(DataTable dt, string mode)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        if (dt.Rows.Count > 0)
        {
            //add header
            string colStr = "";
            foreach (DataColumn dc in dt.Columns)
            {
                if (colStr.Length > 0)
                    colStr += ",";
                switch (mode)
                {
                    case "F05256":
                        dc.ColumnName = "% Mthly Avg";
                        break;
                    case "F05259":
                        dc.ColumnName = "% Mth Avg";
                        break;
                    case "F05262":
                        dc.ColumnName = "% Yr.Target";
                        break;
                }
                colStr += "'" + dc.ColumnName + "'";
            }
            sb.Append("[" + colStr + "]");

            // Add records
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                DataRow dr = dt.Rows[count];
                string rowDataStr = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (rowDataStr.Length > 0)
                        rowDataStr += ",";
                    if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                        rowDataStr += dr[dc].ToString().Trim();
                    else
                        rowDataStr += "'" + dr[dc].ToString().Trim() + "'";

                    rowDataStr = rowDataStr.Replace("\r\n", "").Trim();
                }

                sb.Append(",");
                sb.Append("[" + rowDataStr + "]");
            }
        }
        sb.Append("]");
        return sb.ToString();
    }

    public void OpenChart(string chartname, string chart_title, string chart_div)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        switch (chartname)
        {
            case "Gauge":
                sb.Append(@"google.load('visualization', '1', { packages: ['gauge'] });");
                sb.Append(@"var options;");
                break;
            default:
                if (chartname == "Bar" || chartname == "Pie")
                    sb.Append(@"google.load('visualization', '1', { packages: ['corechart'] });");
                break;
        }
        sb.Append(@"google.setOnLoadCallback(drawChart);");
        sb.Append(@"function drawChart() {");
        var data = "google.visualization.arrayToDataTable(" + hdnChartData.Value + ",false);";
        sb.Append(@"data =" + data.ToString());
        switch (chartname)
        {
            case "Pie":
                // sb.Append(@"var options = {'title': '" + chart_title + "','width': 1000,'height': 800,titleFontSize:20,titleTextStyle:{ bold : false },legend: {position: 'labeled'}");
                sb.Append(@"var options = {'title': '" + chart_title + "','width': '100%',titleFontSize:20,titleTextStyle:{ bold : false },legend: {position: 'labeled'}");
                switch (cond)
                {
                    case "":
                        break;

                    default:
                        if (cond == "F05278")
                            sb.Append(@",is3D: true,");
                        else if (cond == "F05281")
                            sb.Append(@",pieHole: 0.4,");
                        else if (cond == "F05264" || cond == "F05301" || cond == "F05304" || cond == "F05307" || cond == "F05310")
                        {
                            sb.Append(@",slices: { ");
                            sb.Append(@"1: {offset: 0.1},");
                            sb.Append(@"4: {offset: 0.2},");
                            sb.Append(@"6: {offset: 0.3},");
                            sb.Append(@"8: {offset: 0.2},");
                            sb.Append(@"10: {offset: 0.1},");
                            sb.Append(@"12: {offset: 0.1},");
                            sb.Append(@"14: {offset: 0.2},");
                            sb.Append(@"15: {offset: 0.1},");
                            sb.Append(@"20: {offset: 0.2},");
                            sb.Append(@"30: {offset: 0.1},");
                            sb.Append(@"},");
                        }
                        break;
                }
                sb.Append(@"};");
                sb.Append(@"chart = new google.visualization.PieChart(document.getElementById('" + chart_div + "'));");
                break;

            case "Bar":
                //sb.Append(@"var options = {'title': '" + chart_title + "','width': 1000,'height': 800, titleFontSize:20,titleTextStyle:{ bold : false },");
                sb.Append(@"var options = {'title': '" + chart_title + "','width': '100%',titleFontSize:20,titleTextStyle:{ bold : false },");
                sb.Append(@"hAxis: { title: '" + hdnHAxisTitle_Bar.Value + "', titleTextStyle: { color: 'red' } },");

                switch (cond)
                {
                    case "F05338":
                        sb.Append(@"colors: ['6B8E23','FCB441'], ");
                        sb.Append(@"isStacked: true,");
                        break;
                    case "GP22":
                        sb.Append(@"chart = new google.visualization.AreaChart(document.getElementById('" + chart_div + "'));");
                        break;
                    case "F05238":
                        sb.Append(@"colors: ['000080','32CD32'], ");
                        break;
                    case "F05232":
                        sb.Append(@"colors: ['056492','DF3A02'], ");
                        break;
                    case "F05241":
                        sb.Append(@"colors: ['000080','FF0000'], ");
                        break;
                    default:
                        break;
                }
                sb.Append(@"};");
                switch (cond)
                {
                    case "F05338":
                        sb.Append(@"chart = new google.visualization.ColumnChart(document.getElementById('" + chart_div + "'));");
                        break;
                    default:
                        if (cond == "F05241" || cond == "F05238" || cond == "F05232" || cond == "F05229")
                            sb.Append(@"chart = new google.visualization.ColumnChart(document.getElementById('" + chart_div + "'));");
                        else if (cond == "F05276" || cond == "F05287")
                            sb.Append(@"chart = new google.visualization.SteppedAreaChart(document.getElementById('" + chart_div + "'));");
                        break;
                }
                break;

            case "Gauge":
                sb.Append(@"options = {");
                sb.Append(@"width: 400, height: 400,");
                sb.Append(@"greenFrom: 75,greenTo: 100,");
                sb.Append(@"redFrom: 0, redTo: 25,");
                sb.Append(@"yellowFrom:25, yellowTo: 75,");
                sb.Append(@"minorTicks: 5");
                sb.Append(@"};");
                sb.Append(@"chart = new google.visualization.Gauge(document.getElementById('" + chart_div + "'));");
                break;
        }
        sb.Append(@"chart.draw(data, options);");
        sb.Append(@"window.addEventListener('resize', drawChart, false);");
        sb.Append(@"}");
        if (chartname == "Gauge")
        {
            sb.Append(@"function changeTemp(dir) {");
            sb.Append(@"data.setValue(0, 0, data.getValue(0, 0) + dir * 10);");
            sb.Append(@"chart.draw(data, options);");
            sb.Append(@"}");
        }
        sb.Append(@"</script>");
        ScriptManager.RegisterStartupScript(this, this.GetType(), chart_div, sb.ToString(), false);
    }

    public void OpenChartColumn(string chartname, string chart_title, string chart_div)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"function drawChart() {");
        var data = "google.visualization.arrayToDataTable(" + hdnChartData.Value + ",false);";
        sb.Append(@"data =" + data.ToString());
        sb.Append(@"var group = google.visualization.data.group(data, [0], []);");
        sb.Append(@"var columns = [0];");
        sb.Append(@"for (var i = 0; i < group.getNumberOfRows(); i++) {");
        sb.Append(@"var label = group.getValue(i, 0);");
        sb.Append(@"columns.push({");
        sb.Append(@"type: 'number',");
        sb.Append(@"calc: (function (name) {");
        sb.Append(@"return function (dt, row) {");
        sb.Append(@"return (dt.getValue(row, 0) == name) ? dt.getValue(row, 1) : null;");
        sb.Append(@"}");
        sb.Append(@"})(label)");
        sb.Append(@"});");
        sb.Append(@"}");
        sb.Append(@"var chart = new google.visualization.ChartWrapper({");
        sb.Append(@"chartType: 'ColumnChart',");
        sb.Append(@"containerId: '" + chart_div + "',");
        sb.Append(@"dataTable: data,");
        sb.Append(@"options: {");
        sb.Append(@"'is3D': true,");
        sb.Append(@"'isStacked': true,");
        sb.Append(@"'legend':'none',");
        sb.Append(@" 'title' :'" + chart_title + "','width': '100%', titleFontSize:20,titleTextStyle:{ bold : false }, ");
        sb.Append(@"'hAxis': { title: '" + hdnHAxisTitle_Bar.Value + "', titleTextStyle: { color: 'red' } }");
        if (cond == "F05332" || cond == "F05335")
        {
            sb.Append(@",'vAxis': { title: '" + hdnVAxisTitle_Bar.Value + "', titleTextStyle: { color: 'red' } }");
        }
        sb.Append(@"},");
        sb.Append(@"view: {");
        sb.Append(@"columns: columns");
        sb.Append(@"}");
        sb.Append(@"});");
        sb.Append(@"chart.draw();");
        sb.Append(@"}");
        sb.Append(@"google.setOnLoadCallback(drawChart);");
        sb.Append(@"google.load('visualization', '1', {packages: ['corechart']});");
        sb.Append(@"</script>");
        ScriptManager.RegisterStartupScript(this, this.GetType(), chart_div, sb.ToString(), false);
    }

    protected void timer1_Tick(object sender, EventArgs e)
    {
        iconDt = (DataTable)ViewState["icodeDt"];
        kz = Convert.ToInt32(fgenMV.Fn_Get_Mvar(frm_qstr, "U_KZ1"));
        for (int i = 0; i < iconDt.Rows.Count; i++)
        {
            if (i == kz)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", iconDt.Rows[i]["id"].ToString().Trim());
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID2", iconDt.Rows[i]["id"].ToString().Trim());
                printGraph();
                //upd.Update();                                
                break;
            }
            if (kz >= iconDt.Rows.Count)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ1", "0");
                kz = 0;
                i = 0;
                chartDiv1.Visible = false;
                chartDiv2.Visible = false;
                chartDiv3.Visible = false;
                chartDiv4.Visible = false;
                chartDiv5.Visible = false;
                chartDiv6.Visible = false;
            }
        }
    }

    protected void btnPrev_Click(object sender, ImageClickEventArgs e)
    {
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMBTNID");
        if (frm_formID == "0")
        {
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        }
        ID = fgen.make_int(frm_formID.Substring(1, 5)) - 1;
        if (ID <= 05364)
        {
            ID = 05370;
        }
        frm_formID = "F0" + ID.ToString();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMBTNID", frm_formID);
        kz = Convert.ToInt32(fgenMV.Fn_Get_Mvar(frm_qstr, "U_KZ1"));
        printGraph();
    }

    protected void btnNext_Click(object sender, ImageClickEventArgs e)
    {
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMBTNID");
        if (frm_formID == "0")
        {
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        }
        ID = fgen.make_int(frm_formID.Substring(1, 5)) + 1;
        if (ID >= 05371)
        {
            ID = 05365;
        }
        frm_formID = "F0" + ID.ToString();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMBTNID", frm_formID);
        kz = Convert.ToInt32(fgenMV.Fn_Get_Mvar(frm_qstr, "U_KZ1"));
        printGraph();
    }
}