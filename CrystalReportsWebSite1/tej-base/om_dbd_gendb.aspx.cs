using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dbd_gendb : System.Web.UI.Page
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
    string Prg_Id;

    fgenDB fgen = new fgenDB();

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
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_BYPASS1") == "Y")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_BYPASS1", "N");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", PrdRange);

                    printHeads();
                    printGraph();
                }
                else
                {
                    if (Prg_Id == "******")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "PARTY");
                    }
                    else fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    askPopUp();
                }
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

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE1'", "dbval");
        if (db_query.Contains("@"))
        {
            lblBox1Header.Text = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);

            lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
        }

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE2'", "dbval");
        if (db_query.Contains("@"))
        {
            lblBox2Header.Text = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);

            lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
        }

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE3'", "dbval");
        if (db_query.Contains("@"))
        {
            lblBox3Header.Text = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);

            lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
        }

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='TILE4'", "dbval");
        if (db_query.Contains("@"))
        {
            lblBox4Header.Text = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            if (db_sql.Substring(0, 7) == "PERCENT")
            {
                if (fgen.make_double(lblBox1Count.Text) > 0)
                    lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select round((" + lblBox3Count.Text + " / " + lblBox1Count.Text + ")*100,2) as cnt1 from dual ", "cnt1") + " %";
            }
            else
            {
                db_sql = db_sql.Replace("`", "'");
                db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                db_sql = db_sql.Replace("DT_RANGE", DateRange);
                db_sql = db_sql.Replace("CURR_MTH", this_mth);
                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, db_sql, "cnt1");
            }

        }
    }
    void printGraph()
    {
        mdt2_memv = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        this_mth = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + mdt2_memv + "','dd/mm/yyyy'),'yyyymm') as cmth from dual", "cmth");

        val_legnd = "Value";
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

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


        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        string db_query;
        string db_sql;

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART1'", "dbval");
        if (db_query.Contains("@"))
        {
            stitle1 = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);
            squery1 = db_sql;
        }

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART2'", "dbval");
        if (db_query.Contains("@"))
        {
            stitle2 = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);
            squery2 = db_sql;
        }

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART3'", "dbval");
        if (db_query.Contains("@"))
        {
            stitle3 = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);
            squery3 = db_sql;
        }

        db_query = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(a.obj_caption)||'@'||trim(a.obj_SQL) as dbval from dbd_config a where a.branchcd!='DD' and  a.frm_name='" + Prg_Id + "'  and upper(trim(a.obj_name))='CHART4'", "dbval");
        if (db_query.Contains("@"))
        {
            stitle4 = db_query.Split('@')[0].ToString();
            db_sql = db_query.Split('@')[1].ToString();
            db_sql = db_sql.Replace("`", "'");
            db_sql = db_sql.Replace("BR_VAR", frm_mbr);
            db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
            db_sql = db_sql.Replace("DT_RANGE", DateRange);
            db_sql = db_sql.Replace("CURR_MTH", this_mth);
            squery4 = db_sql;
        }


        stitle5 = "";
        squery5 = "";
        stitle6 = "";
        squery6 = "";
        val_legnd = "";



        string gt1 = "column";
        string gt2 = "bar";
        string gt3 = "pie";
        string gt4 = "line";
        switch (Prg_Id)
        {
            case "F15152":
                gt1 = "column";
                gt2 = "bar";
                gt3 = "pie";
                gt4 = "line";
                break;
            case "F15156":
                gt1 = "pie";
                gt2 = "bar";
                gt3 = "column";
                gt4 = "line";
                break;
            case "F20141":
                gt1 = "bar";
                gt2 = "spline";
                gt3 = "pie";
                gt4 = "line";
                break;
            case "F25176":
                gt1 = "area";
                gt2 = "pie";
                gt3 = "bar";
                gt4 = "line";
                break;
            case "F25181":
                gt1 = "column";
                gt2 = "pie";
                gt3 = "area";
                gt4 = "line";
                break;
            case "F30152":
                gt1 = "area";
                gt2 = "column";
                gt3 = "bar";
                gt4 = "sline";
                break;
            case "F47152":
                gt1 = "pie";
                gt2 = "spline";
                gt3 = "bar";
                gt4 = "line";
                break;
            case "F50152":
                gt1 = "pie";
                gt2 = "spline";
                gt3 = "bar";
                gt4 = "area";
                break;
        }

        lblChart1Header.Text = stitle1;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle1, gt1, stitle1, val_legnd, squery1, val_legnd, "chart1", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart1", chartScript, false);

        lblChart2Header.Text = stitle2;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle2, gt2, stitle2, val_legnd, squery2, val_legnd, "chart2", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart2", chartScript, false);

        lblChart3Header.Text = stitle3;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle3, gt3, stitle3, val_legnd, squery3, val_legnd, "chart3", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart3", chartScript, false);

        lblChart4Header.Text = stitle4;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle4, gt4, stitle4, val_legnd, squery4, val_legnd, "chart4", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart4", chartScript, false);

        //lblChart5Header.Text = stitle5;
        //sQuery = "SELECT TO_cHAR(VCHDATE,'MM/YY') AS YR,ROUND(SUM(iqtyin/100),2) AS QTYOUT FROM IVOUCHER A WHERE BRANCHCD='00' AND TYPE LIKE '1%' GROUP BY TO_cHAR(VCHDATE,'MM/YY') ";
        //chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle5, "pie", stitle1, val_legnd , squery5 , "chart5");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart5", chartScript, false);

        //lblChart6Header.Text = stitle6;
        //sQuery = "SELECT TO_cHAR(orddt,'MM/YY') AS YR,ROUND(SUM(qtyord/100000),2) AS QTYOUT FROM pomas A WHERE BRANCHCD='00' AND TYPE LIKE '5%' GROUP BY TO_cHAR(orddt,'MM/YY') ";
        //chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle6, "gauge", stitle1, val_legnd , squery6 , "chart6");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart6", chartScript, false);

        //lblChart7Header.Text = "G.E";
        //sQuery = "SELECT TO_cHAR(VCHDATE,'MM/YY') AS YR,ROUND(SUM(iqtyin/100),2) AS QTYOUT FROM IVOUCHER A WHERE BRANCHCD='00' AND TYPE LIKE '1%' GROUP BY TO_cHAR(VCHDATE,'MM/YY') ";
        //chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, "G.E.", "line", stitle1, "Value in Lacs", sQuery, "chart7");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart7", chartScript, false);
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

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "RPT");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", db_sql);
        fgen.Fn_open_sseek(stitle1, frm_qstr);
        //fgen.Fn_open_rptlevel(stitle1, frm_qstr);

        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", db_sql);
        //fgen.Fn_open_sseek("", frm_qstr);

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
}