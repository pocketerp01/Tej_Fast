using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;


public partial class fin_base_controls_deskBox89 : System.Web.UI.UserControl
{
    fgenDB fgen = new fgenDB();
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, db_query = "", col1;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, DateRange, vardate, fromdt, todt, squery, PrdRange, this_mth, mdt2_memv = "";
    string db_sql = "";
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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!IsPostBack)
            {
                ViewState["graphScripts"] = null;
                fillBox();
            }
            if (ViewState["graphScripts"] != null)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AAA" + "_", (string)ViewState["graphScripts"], false);

        }
    }
    public void fillBox()
    {
        this_mth = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + vardate + "','dd/mm/yyyy'),'yyyymm') as cmth from dual", "cmth");
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        squery = "";

        string iconTab = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB");

        double st_row = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ST_ROW").toDouble();
        double end_row = fgenMV.Fn_Get_Mvar(frm_qstr, "U_END_ROW").toDouble();
        double counter = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COUNTER").toDouble();

        double totRows = 10;
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
        // VIPIN
        squery = "SELECT * FROM (select db_query,SRNO,obj_name,rno from (SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name,rownum as rno FROM DSK_CONFIG a where SUBSTR(upper(obj_name),1,3) IN ('TXT','GRA') ORDER BY /*A.vchnum,*/ a.srno) ) WHERE SRNO between " + st_row + " and " + end_row + "";

        //if (counter % 2 != 0)
        //{
        //    squery = "select * from (select db_query,SRNO,obj_name,rownum as rno from (SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a where SUBSTR(upper(obj_name),1,3) IN ('TXT') ORDER BY A.obj_name)) order by rno";
        //}
        //else
        //{
        //    squery = "select * from (select db_query,SRNO,obj_name,rownum as rno from (SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a where SUBSTR(upper(obj_name),1,3) IN ('GRA') ORDER BY A.obj_name)) order by rno";
        //}
        counter++;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COUNTER", counter.ToString());


        //if (iconTab.ToUpper() == "FIN_MRSYS")

        frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
        if (frm_ulvl.toDouble() > 0)
        {
            squery = "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a WHERE SUBSTR(upper(a.obj_name),1,3) IN ('TXT','GRA')  and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) IN (SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) as fstr FROM DSK_WCONFIG A WHERE A.USERID='" + frm_UserID + "' and trim(a.USERNAME)='" + frm_uname + "') order by A.vchnum,a.srno ";
            squery = "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a WHERE SUBSTR(upper(a.obj_name),1,3) IN ('TXT','GRA')  and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.FRM_TITLE) IN (SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) as fstr FROM DSK_WCONFIG A WHERE A.USERID='" + frm_UserID + "' and trim(a.USERNAME)='" + frm_uname + "') order by A.vchnum,a.srno ";
        }
        dt2 = fgen.getdata(frm_qstr, frm_cocd, squery);

        // VIPIN
        if (dt2.Rows.Count <= 0 && st_row > 1)
        {
            counter = 1;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COUNTER", "0");            
            Response.Redirect("~/tej-base/" + "dskGridDash" + ".aspx?STR=" + frm_qstr, false);
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
                DataTable myDt = new DataTable();
                int k = 5;
                for (int i = 0; i < k; i++)
                {
                    if (i == 0) myDt.Columns.Add("fstr", typeof(string));
                    else myDt.Columns.Add("field" + (i), typeof(string));
                }
                DataRow myDr;
                foreach (var str in squery.Split('~'))
                {
                    dt = fgen.getdata(frm_qstr, frm_cocd, str);
                    if (dt.Rows.Count <= 0)
                    {
                        string cond = "GRAPH";
                        if (!str.ToUpper().Contains("GRAPH")) cond = "-";
                        string myHeadingDe = "";
                        if (str.ToUpper().Contains(" AS MDATA2"))
                            myHeadingDe = System.Text.RegularExpressions.Regex.Split(str.ToUpper(), " AS MDATA2")[0].ToString();
                        if (myHeadingDe.ToString().Length > 0)
                        {
                            int startInd = myHeadingDe.IndexOf(',');
                            int lastInd = myHeadingDe.IndexOf(':');
                            myHeadingDe = myHeadingDe.Substring(startInd, (lastInd - startInd));
                            myHeadingDe = myHeadingDe.Replace('"', ' ');
                            myHeadingDe = myHeadingDe.Replace(',', ' ');
                            myHeadingDe = myHeadingDe.Replace('"', ' ');
                            myHeadingDe = myHeadingDe.Replace("'", "");
                        }

                        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT 'No Data Exist' AS F1,'" + myHeadingDe.toProper() + "' AS F2,'No Data Exist' AS F3,'-' AS F4,'" + cond + "' AS F5 FROM DUAL");
                    }
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

                                myDr[1] = dr[2];
                                myDr[2] = fgen.Fn_FillChart(frm_cocd, frm_qstr, "", dr[3].ToString().Trim().ToLower(), dr[2].ToString(), "", dt, "", "VIPIN");

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

                ListBox1.DataSource = myDt;
                ListBox1.DataBind();

                if (myDt.Rows.Count > 0)
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    str.Append("$(document).ready(function () {");
                    str.Append("setInterval(function () {");
                    str.Append("window.location = window.location.href;");
                    str.Append("}, 120000);});");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Refresh", str.ToString(), true);
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

        squery = "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO FROM DSK_CONFIG a WHERE upper(A.OBJ_NAME)='POP_" + objName + "' ORDER BY A.vchnum,a.srno";

        ///if (iconTab.ToUpper() == "FIN_MRSYS")
        frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");

        if (frm_ulvl.toDouble() > 0)
        {
            squery = "SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO FROM DSK_CONFIG a WHERE upper(A.OBJ_NAME)='POP_" + objName + "' and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) IN (SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) as fstr FROM DSK_WCONFIG A WHERE A.USERID='" + frm_UserID + "' and trim(a.USERNAME)='" + frm_uname + "') and A.OBJ_NAME='" + objName + "' order by A.vchnum,a.srno";

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
            fgen.Fn_open_sseek("Details", frm_qstr);
        }
        string old_cntr = "";
        old_cntr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OLD_CNTR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COUNTER", old_cntr.ToString());
        //fillBox();
    }
    protected void ListBox1_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlGenericControl mainDiv = (HtmlGenericControl)e.Item.FindControl("mainBox");
            HtmlGenericControl Div1 = (HtmlGenericControl)e.Item.FindControl("Div1");
            HtmlGenericControl boxBodyC = (HtmlGenericControl)e.Item.FindControl("boxBodyC");
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

                if (!str.ToUpper().Contains("NO DATA"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), mydiv.ClientID + "_", str, false);
                    ViewState["graphScripts"] += str;
                    f1.Visible = false;
                }
                else
                {
                    f1.Visible = true;
                    f1.InnerText = "";
                    //mydiv.Style["background-image"] = "../tej-base/images/nodata.gif";
                    mydiv.Style["background"] = "url(../tej-base/images/nodata.gif) no-repeat center";
                }

                mainDiv.Attributes.Add("class", "col-md-4 zoom");
                mydiv.Style.Add("height", "290px");
                deskBackG.Attributes.Add("class", "content-wrapper grad2");


                f2.Visible = false;
                f3.Visible = false;
                f4.Visible = false;
                brLine.Visible = false;
                Div1.Style.Add("display", "none");
                //overLay.Visible = false;
            }
            else
            {
                string _backColor = backColor(e.Item.DataItemIndex);
                boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + _backColor);
                if (f1.InnerText.ToUpper().Contains("SALE INVOICE"))
                {
                    mainDiv.Attributes.Add("class", "col-md-4 zoom");
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#ffb5ad");
                }
                if (f1.InnerText.ToUpper().Contains("SALE"))
                {
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#ffb5ad");
                }
                if (f1.InnerText.ToUpper().Contains("PURCH"))
                {
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#F0EAD6");
                }
                if (f1.InnerText.ToUpper().Contains("MRR") || f1.InnerText.ToUpper().Contains("GATE*") || f1.InnerText.ToUpper().Contains("CHL") || f1.InnerText.ToUpper().Contains("ISSUE") || f1.InnerText.ToUpper().Contains("RETURN"))
                {
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#dd4477");
                }
                if (f1.InnerText.ToUpper().Contains("COLLECTION") || f1.InnerText.ToUpper().Contains("PAYMENT") || f1.InnerText.ToUpper().Contains("CHL") || f1.InnerText.ToUpper().Contains("ISSUE") || f1.InnerText.ToUpper().Contains("RETURN"))
                {
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#abb9f3");
                }
                if (f1.InnerText.ToUpper().Contains("SALES RETURN") || f1.InnerText.ToUpper().Contains("REJN"))
                {
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#f7ffad");
                }
                if (f1.InnerText.ToUpper().Contains("MRR") || f1.InnerText.ToUpper().Contains("GATE"))
                {
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#ffbd80");
                }
                if (f1.InnerText.ToUpper().Contains("QA"))
                {
                    boxBodyC.Attributes.Add("style", "min-height: 150px; color: #000; background-color:" + "#99ffcc");
                }
                if (f1.InnerText.Trim().Length > 23) f1.Style.Add("font-size", "14px");
                if (f3.InnerText.Trim().Length > 25) f1.Style.Add("font-size", "12px");
            }
        }
    }
    string backColor(int index)
    {
        string backcolor = "#c0cffd";
        if (index > 40) index -= 40;
        if (index > 30) index -= 30;
        if (index > 20) index -= 20;
        if (index > 10) index -= 10;

        switch (index)
        {
            case 0:
                backcolor = "#dc3912";
                break;
            case 1:
                backcolor = "#ffa31a";
                break;
            case 2:
                backcolor = "#c0cffd";
                break;
            case 3:
                backcolor = "#f5cfa9";
                break;
            case 4:
                backcolor = "#00FFFF";
                break;
            case 5:
                backcolor = "#b82e2e";
                break;
            case 6:
                backcolor = "#ff4d4d";
                break;
            case 7:
                backcolor = "#00FFFF";
                break;
            case 8:
                backcolor = "#f79bba";
                break;
            case 9:
                backcolor = "#ff4000";
                break;
        }

        return backcolor;
    }
    protected void ListBox1_DataBound(object sender, EventArgs e)
    {

    }
}