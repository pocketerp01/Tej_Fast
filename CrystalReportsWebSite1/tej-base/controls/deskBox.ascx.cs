using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.Web.UI.HtmlControls;
using System.Threading;
using System.Web.Services;
using System.ComponentModel;


public partial class fin_base_controls_deskBox : System.Web.UI.UserControl
{
    fgenDB fgen = new fgenDB();
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, db_query = "", col1;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, DateRange, vardate, fromdt, todt, squery, PrdRange, this_mth, mdt2_memv = "";
    string db_sql = "";
    DataTable dt2 = new DataTable();
    DataTable myDt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        fgenMV.context = HttpContext.Current;
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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    hfQstr.Value = frm_qstr;
                }
                else Response.Redirect("~/login.aspx");
            }
            //fgen.FILL_ERR("Desktop Page Loaded : Total Query ran before Tiles loading : " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_Q_COUNTER") + " ");
            if (!IsPostBack)
            {
                //threadload();
                //fillBox();                
            }
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "FS_LOG") == "Y")
            {
                time1.Interval = 10000;               
                fgenMV.Fn_Set_Mvar(frm_qstr, "FS_LOG", "N");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Refresh12", "changeText();", true);                
            }
            else
            {
                changeText.Style.Add("display", "none");
                time1.Interval = 30000;
            }
        }
    }
    /// <summary>
    /// Thread working added on 09/04/2020 -- VV
    /// </summary>
    void threadload()
    {
        Thread bgThread = new Thread(fillBox);
        bgThread.IsBackground = true;
        bgThread.Start();
    }
    //[WebMethod]
    public void fillBox()
    {
        myDt = new DataTable();
        this_mth = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + vardate + "','dd/mm/yyyy'),'yyyymm') as cmth from dual", "cmth");
        DataTable dt = new DataTable();
        squery = "";

        string iconTab = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB");

        double st_row = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ST_ROW").toDouble();
        double end_row = fgenMV.Fn_Get_Mvar(frm_qstr, "U_END_ROW").toDouble();
        double counter = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COUNTER").toDouble();

        double totRows = 20;
        if (counter == 1)
        {
            st_row = 1;
            end_row = totRows;
        }
        else
        {
            st_row = end_row + 1;
            end_row = end_row + totRows;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ST_ROW", st_row.ToString());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_END_ROW", end_row.ToString());

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_CNTR", counter.ToString());

        //squery = "SELECT * FROM (select db_query,SRNO,obj_name,rno from (SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name,rownum as rno FROM DSK_CONFIG a where SUBSTR(upper(obj_name),1,3) IN ('TXT','GRA') ORDER BY A.vchnum,a.srno) ) WHERE RNO between " + st_row + " and " + end_row + "";

        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_LTILE") == "TXT")
        {
            squery = "select * from (select db_query,SRNO,obj_name,rownum as rno from (SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a where BRANCHCD='" + frm_mbr + "' AND SUBSTR(upper(obj_name),1,3) IN ('GRA') ORDER BY A.obj_name)) order by rno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_LTILE", "GRA");
        }
        else
        {
            squery = "select * from (select db_query,SRNO,obj_name,rownum as rno from (SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a where BRANCHCD='" + frm_mbr + "' AND SUBSTR(upper(obj_name),1,3) IN ('TXT') ORDER BY A.obj_name)) order by rno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_LTILE", "TXT");
        }
        counter++;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COUNTER", counter.ToString());


        //if (iconTab.ToUpper() == "FIN_MRSYS")

        frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
        if (frm_ulvl.toDouble() > 0)
        {
            squery = "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a WHERE BRANCHCD='" + frm_mbr + "' AND SUBSTR(upper(a.obj_name),1,3) IN ('TXT','GRA')  and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) IN (SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) as fstr FROM DSK_WCONFIG A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.USERID='" + frm_UserID + "' and trim(a.USERNAME)='" + frm_uname + "') order by A.vchnum,a.srno ";
        }

        dt2 = fgen.getdata(frm_qstr, frm_cocd, squery);

        if (dt2.Rows.Count <= 0 && frm_ulvl.toDouble() > 0)
        {
            Session["myDt"] = null;
            ListBox1.DataSource = null;
            ListBox1.DataBind();
        }

        // VIPIN
        if (dt2.Rows.Count <= 0 && st_row > 1)
        {
            counter = 1;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COUNTER", "0");
            try
            {
                Response.Redirect("~/tej-base/" + "dskGridDash" + ".aspx?STR=" + frm_qstr, false);
            }
            catch { }
            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COUNTER", "1");
            //System.Text.StringBuilder str = new System.Text.StringBuilder();
            //str.Append("$(document).ready(function () {");
            //str.Append("window.location = window.location.href; });");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Refresh12", str.ToString(), true);
        }

        squery = "";
        dt = new DataTable();
        for (int i = 0; i < dt2.Rows.Count; i++)
        {

            db_query = dt2.Rows[i]["db_query"].ToString().Trim();
            if (db_query.Contains("@"))
            {
                db_sql = db_query.Split('@')[1].ToString();
                db_sql = db_sql.Replace("`", "'");
                db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                db_sql = db_sql.Replace("DT_RANGE", DateRange);
                db_sql = db_sql.Replace("CURR_MTH", this_mth);
            }

            if (squery.Length > 0) squery += " ~ " + db_sql;
            else squery = db_sql;
        }
        if (squery.Length > 1)
        {
            try
            {

                int k = 5;
                for (int i = 0; i < k; i++)
                {
                    if (i == 0)
                    {
                        if (!myDt.Columns.Contains("fstr"))
                            myDt.Columns.Add("fstr", typeof(string));
                    }
                    else
                    {
                        if (!myDt.Columns.Contains("field" + (i))) myDt.Columns.Add("field" + (i), typeof(string));
                    }
                }
                DataRow myDr;
                foreach (var str in squery.Split('~'))
                {
                    dt = fgen.getdata(frm_qstr, frm_cocd, str);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr[4].ToString().Trim().ToUpper().Contains("GRAPH"))
                            {
                                col1 = dr[4].ToString();
                                if (!dr[4].ToString().Trim().ToUpper().Contains('$')) col1 = dr[4] + "$" + "-";

                                myDr = myDt.NewRow();
                                myDr[0] = col1.Split('$')[1];

                                myDr[2] = fgen.Fn_FillChart(frm_cocd, frm_qstr, "", dr[3].ToString().Trim().ToLower(), dr[2].ToString(), fgenCO.chk_co(frm_cocd), dt, "", "VIPIN");

                                myDr[3] = dr[3];
                                myDr[4] = col1.Split('$')[0];
                                myDt.Rows.Add(myDr);
                                break;

                            }
                            myDr = myDt.NewRow();
                            for (int i = 0; i < k; i++)
                            {
                                try { myDr[i] = dr[i]; }
                                catch { }
                            }
                            myDt.Rows.Add(myDr);
                        }
                    }
                }

                if (myDt.Rows.Count > 0)
                {
                    Session["myDt"] = myDt;                    
                }
            }
            catch
            {
                string abc = "";
            }

        }
    }
    protected void ListBox1_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        ListViewItem item = (ListViewItem)ListBox1.Items[e.NewSelectedIndex];
        Label c = (Label)item.FindControl("lbl");
        string objName = c.Text.ToUpper();

        this_mth = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + this_mth + "','dd/mm/yyyy'),'yyyymm') as cmth from dual", "cmth");
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        squery = "";

        string iconTab = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB");

        squery = "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO FROM DSK_CONFIG a WHERE A.BRANCHCD='" + frm_mbr + "' AND upper(A.OBJ_NAME)='POP_" + objName + "' ORDER BY A.vchnum,a.srno";

        ///if (iconTab.ToUpper() == "FIN_MRSYS")
        frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");

        if (frm_ulvl.toDouble() > 0)
        {
            squery = "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO FROM DSK_CONFIG a WHERE A.BRANCHCD='" + frm_mbr + "' AND upper(A.OBJ_NAME)='POP_" + objName + "' and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) IN (SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) as fstr FROM DSK_WCONFIG A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.USERID='" + frm_UserID + "' and trim(a.USERNAME)='" + frm_uname + "') and A.OBJ_NAME='" + objName + "' order by A.vchnum,a.srno";

        }
        dt2 = fgen.getdata(frm_qstr, frm_cocd, squery);

        squery = "";
        for (int i = 0; i < dt2.Rows.Count; i++)
        {

            db_query = dt2.Rows[i]["db_query"].ToString().Trim();
            if (db_query.Contains("@"))
            {
                db_sql = db_query.Split('@')[1].ToString();
                db_sql = db_sql.Replace("`", "'");
                db_sql = db_sql.Replace("BR_VAR", frm_mbr);
                db_sql = db_sql.Replace("PRD_RANGE", PrdRange);
                db_sql = db_sql.Replace("DT_RANGE", DateRange);
                db_sql = db_sql.Replace("CURR_MTH", this_mth);
            }

            if (squery.Length > 0) squery += " UNION ALL " + db_sql;
            else squery = db_sql;
        }
        if (squery.Length > 1)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
            //fgen.Fn_open_sseek("Details", frm_qstr);

            ScriptManager.RegisterStartupScript(this.upd1, this.GetType(), "PopUP", "OpenSingle('../tej-base/Sseek.aspx?STR=" + frm_qstr + "','80%','65%','Details');", true);
        }
        string old_cntr = "";
        old_cntr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OLD_CNTR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COUNTER", old_cntr.ToString());
        if (objName.Contains("GRAPH"))
        {
            myDt = (DataTable)Session["myDt"];
            ListBox1.DataSource = myDt;
            ListBox1.DataBind();
        }
        //threadload();
    }
    protected void ListBox1_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlGenericControl mainDiv = (HtmlGenericControl)e.Item.FindControl("mainBox");
            HtmlGenericControl f1 = (HtmlGenericControl)e.Item.FindControl("f1");
            HtmlGenericControl f2 = (HtmlGenericControl)e.Item.FindControl("f2");
            HtmlGenericControl f3 = (HtmlGenericControl)e.Item.FindControl("f3");
            HtmlGenericControl f4 = (HtmlGenericControl)e.Item.FindControl("f4");
            HtmlGenericControl brLine = (HtmlGenericControl)e.Item.FindControl("brLine");
            HtmlGenericControl mydiv = (HtmlGenericControl)e.Item.FindControl("chart1");
            HtmlGenericControl overLay = (HtmlGenericControl)e.Item.FindControl("overLay");
            if (f4.InnerText.ToUpper().Trim() == "GRAPH")
            {
                string str = f2.InnerText.Trim().Replace("VIPIN", mydiv.ClientID);
                ScriptManager.RegisterStartupScript(this.upd1, this.GetType(), mydiv.ClientID + "_", str, false);

                mainDiv.Attributes.Add("class", "col-md-4 zoom");

                mydiv.Style.Add("height", "262px");
                f1.Visible = false;
                f2.Visible = false;
                f3.Visible = false;
                f4.Visible = false;
                brLine.Visible = false;
                //overLay.Visible = false;
            }
        }
    }
    protected void br_Click(object sender, EventArgs e)
    {
        if (Session["myDt"] != null) myDt = (DataTable)Session["myDt"];
        ListBox1.DataSource = myDt;
        ListBox1.DataBind();
    }
    protected void time1_Tick(object sender, EventArgs e)
    {
        threadload();
        br_Click("", EventArgs.Empty);
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        time1.Enabled = true;
    }
    protected void Page_Unload(object sender, EventArgs e)
    {

    }
}

