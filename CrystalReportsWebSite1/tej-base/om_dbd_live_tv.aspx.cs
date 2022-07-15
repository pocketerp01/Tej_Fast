using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dbd_live_tv : System.Web.UI.Page
{
    string DateRange, PrdRange, sQuery, chartScript = "";
    string frm_mbr, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    string popUpType = "";
    string this_mth = "", mdt2_memv = "";
    string squery1 = "", squery2 = "", squery3 = "", squery4 = "";
    string stitle1, stitle2, stitle3, stitle4, stitle5, stitle6;
    string val_legnd1, val_legnd2, val_legnd3, val_legnd4;
    string wdays, CSR;
    string Client_Code, indType = "";
    string Prg_Id;
    string leftHeading1, leftHeading2, leftHeading3, leftHeading4;
    string bottomHeading1, bottomHeading2, bottomHeading3, bottomHeading4;
    string gu1, gu2, gu3, gu4;
    string gl1, gl2, gl3, gl4;
    string chart1, chart2, chart3, chart4;
    int kz = 0;
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
                indType = fgen.getOption(frm_qstr, frm_cocd, "W0000", "OPT_PARAM");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_INDTYPE", indType);

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CSNO", "0");
                timer1.Enabled = false;
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

                btnPause.Visible = true;
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_THISMONTH", this_mth);

        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");

        indType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_INDTYPE");
        DataTable dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,a.OBJ_READONLY as graphtype FROM dbd_TV_config a where a.branchcd!='DD' and A.FRM_NAME='" + indType + "' order by a.srno ");
        ViewState["dt"] = dt;
        if (dt.Rows.Count > 0)
        {
            dispGrph(dt);
            timer1.Enabled = true;
        }
        else timer1.Enabled = false;
    }
    void dispGrph(DataTable dtGph)
    {
        this_mth = fgenMV.Fn_Get_Mvar(frm_qstr, "U_THISMONTH");
        int l = Convert.ToInt16(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CSNO"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CSNOOLD", l.ToString());
        int k = 0;
        chart1 = ""; chart2 = ""; chart3 = ""; chart4 = "";
        for (int i = l; i < dtGph.Rows.Count; i++)
        {
            string db_query = "";
            string db_sql = "";
            db_query = dtGph.Rows[i]["db_query"].ToString().Trim();

            if (db_query.Contains("@"))
            {
                db_sql = db_query.Split('@')[1].ToString();
                db_sql = db_sql.Replace("`", "'");
                db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                db_sql = db_sql.Replace("DT_RANGE", DateRange);
                db_sql = db_sql.Replace("CURR_MTH", this_mth);
                if (k == 0)
                {
                    chart1 = dtGph.Rows[i]["graphtype"].ToString().Trim().ToLower();
                    stitle1 = db_query.Split('@')[0].ToString();
                    squery1 = db_sql;
                }
                if (k == 1)
                {
                    chart2 = dtGph.Rows[i]["graphtype"].ToString().Trim().ToLower();
                    stitle2 = db_query.Split('@')[0].ToString();
                    squery2 = db_sql;
                }
                if (k == 2)
                {
                    chart3 = dtGph.Rows[i]["graphtype"].ToString().Trim().ToLower();
                    stitle3 = db_query.Split('@')[0].ToString();
                    squery3 = db_sql;
                }
                if (k == 3)
                {
                    chart4 = dtGph.Rows[i]["graphtype"].ToString().Trim().ToLower();
                    stitle4 = db_query.Split('@')[0].ToString();
                    squery4 = db_sql;
                }
            }
            k++;
            l++;
            if (k == 4) break;
        }
        if (chart1.Length < 2) chart1 = "line"; if (chart2.Length < 2) chart2 = "line"; if (chart3.Length < 2) chart3 = "line"; if (chart4.Length < 2) chart4 = "line";
        chartDiv1.Attributes.Add("class", "col-lg-6"); chartDiv2.Attributes.Add("class", "col-lg-6"); chartDiv3.Attributes.Add("class", "col-lg-6"); chartDiv4.Attributes.Add("class", "col-lg-6");
        chartDiv1.Visible = true; chartDiv2.Visible = true; chartDiv3.Visible = true; chartDiv4.Visible = true;

        chartScript = "";
        lblChart1Header.Text = stitle1;
        if (squery1.Length > 1) chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle1, chart1, gu1, gl1, squery1, val_legnd1, "chart1", bottomHeading1, leftHeading1);
        if (chartScript.Length > 1) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart1", chartScript, false);
        else chartDiv1.Visible = false;

        chartScript = "";
        lblChart2Header.Text = stitle2;
        if (squery2.Length > 1) chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle2, chart2, gu2, gl2, squery2, val_legnd2, "chart2", bottomHeading2, leftHeading2);
        if (chartScript.Length > 1) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart2", chartScript, false);
        else chartDiv2.Visible = false;

        chartScript = "";
        lblChart3Header.Text = stitle3;
        if (squery3.Length > 1) chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle3, chart3, gu3, gl3, squery3, val_legnd3, "chart3", bottomHeading3, leftHeading3);
        if (chartScript.Length > 1) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart3", chartScript, false);
        else chartDiv3.Visible = false;

        chartScript = "";
        lblChart4Header.Text = stitle4;
        if (squery4.Length > 1) chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle4, chart4, gu4, gl4, squery4, val_legnd4, "chart4", bottomHeading4, leftHeading4);
        if (chartScript.Length > 1) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart4", chartScript, false);
        else chartDiv4.Visible = false;

        if (chartDiv1.Visible == false) chartDiv2.Attributes.Add("class", "col-lg-12");
        if (chartDiv2.Visible == false) chartDiv1.Attributes.Add("class", "col-lg-12");
        if (chartDiv3.Visible == false) chartDiv4.Attributes.Add("class", "col-lg-12");
        if (chartDiv4.Visible == false) chartDiv3.Attributes.Add("class", "col-lg-12");
        if (chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
        if (chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }

        if (l >= dtGph.Rows.Count) l = 0;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CSNO", l.ToString());
        if (chartDiv1.Visible == false && chartDiv2.Visible == false && chartDiv3.Visible == false && chartDiv4.Visible == false) timer1_Tick("", EventArgs.Empty);
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
    protected void timer1_Tick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt"];
        dispGrph(dt);
    }
    protected void btnPlay_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = true;
        btnPause.Visible = true;
        btnPlay.Visible = false;

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt"];
        dispGrph(dt);
    }
    protected void btnPause_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = false;
        btnPlay.Visible = true;
        btnPause.Visible = false;

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt"];
        dispGrph(dt);
    }
    protected void btnLeft_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = false;
        btnPlay.Visible = true;
        btnPause.Visible = false;

        int l = Convert.ToInt16(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CSNO"));
        int l1 = Convert.ToInt16(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CSNOOLD"));
        if ((l1 - 4) < 0) l1 = 4;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CSNO", (l1 - 4).ToString());

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt"];
        dispGrph(dt);
    }
    protected void btnRight_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = false;
        btnPlay.Visible = true;
        btnPause.Visible = false;
        int l = Convert.ToInt16(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CSNO"));
        int l1 = Convert.ToInt16(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CSNOOLD"));

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CSNO", (l1 + 4).ToString());

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dt"];
        dispGrph(dt);
    }
}