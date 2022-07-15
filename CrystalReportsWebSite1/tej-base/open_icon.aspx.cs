using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class open_icon : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1, Value2, Value3, HCID, co_cd, frm_qstr, frm_url, frm_formID;
    fgenDB fgen = new fgenDB();

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //GridView1.Sorting += new GridViewSortEventHandler(GridView1_Sorting1);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            //-----------------
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
                    if (frm_qstr.Contains("@"))
                    {
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                        frm_formID = frm_qstr.Split('@')[0].ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                    }
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                }
            }
            //--------------------------                        

            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            if (!Page.IsPostBack) { fill_data(); }

            txtsearch.Attributes["onkeydown"] = "if (event.keyCode == 40) { $('[TabIndex=1]').focus(); }";
            txtsearch.Focus();
        }
    }
    [WebMethod]
    public void fill_data()
    {
        if (query1.Length > 1)
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, co_cd, "select * from ( " + query1 + " ) where rownum<=" + tkrow.Text.Trim() + "");
            ViewState["DATA"] = dt;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            lblTotcount.Text = "Total Rows : " + GridView1.Rows.Count;
            setColGridWidth();

            //List<msys> li = new List<msys>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    msys msy = new msys();
            //    msy.id = dt.Rows[i]["fstr"].ToString().Trim();
            //    msy.text = dt.Rows[i]["text"].ToString().Trim();
            //    msy.webAction = dt.Rows[i]["web"].ToString().Trim();
            //    msy.description = dt.Rows[i]["description"].ToString().Trim();
            //    li.Add(msy);
            //}

            //JavaScriptSerializer js = new JavaScriptSerializer();
            //Context.Response.Write(js.Serialize(li));
        }
        else
        {
            if (Session["send_icondt"] != null)
            {
                dt = new DataTable();
                dt = (DataTable)Session["send_icondt"];
                ViewState["DATA"] = dt;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                lblTotcount.Text = "Total Rows : " + GridView1.Rows.Count;
                setColGridWidth();
            }
        }
    }
    void setColGridWidth()
    {
        int x = (Request.Browser.ScreenPixelsWidth) * 2;
        x = fgen.make_int(Session["hfWindowSize"].ToString());
        if (x == 0) x = 1000;
        double gridFoundWidth = Math.Round(x * .80);
        string col1 = Math.Round((gridFoundWidth * .40)).ToString();
        string col2 = Math.Round((gridFoundWidth * .50)).ToString();
        GridView1.Columns[3].HeaderStyle.Width = fgen.make_int(col1);
        GridView1.Columns[4].HeaderStyle.Width = fgen.make_int(col2);
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = GridView1.SelectedRow;
        Value1 = row.Cells[1].Text.Trim();
        Value2 = row.Cells[2].Text.Trim();

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", Value2);

        switch (HCID)
        {
            case "OPEN_ICON":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
            case "BOOKMARK":
                string tab_name = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB");
                string cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONCOND");
                if (cond.Length > 2) cond = " and " + cond;
                if (Session["dt_menu" + frm_qstr] == null)
                {
                    dt = fgen.fill_icon_grid(co_cd, tab_name, cond, frm_qstr);
                }
                else dt = (DataTable)Session["dt_menu" + frm_qstr];
                int iconLevel = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4"));
                iconLevel = iconLevel + 1;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", iconLevel.ToString());
                switch (iconLevel)
                {
                    case 2:
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", fgen.seek_iname_dt(dt, "ID='" + Value1 + "' AND MLEVEL='1'", "FORM"));
                        break;
                    case 3:
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", fgen.seek_iname_dt(dt, "ID='" + Value1 + "' AND MLEVEL='2'", "SUBMENUID"));
                        break;
                }

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", Value2);
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", fgen.seek_iname_dt(dt, "ID='" + Value1 + "'", "WEB_ACTION"));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
            case "MREPORTS":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
                break;
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex) + ";}";
            e.Row.ToolTip = "Click to select this row.";
        }
    }
    protected void srch_Click(object sender, EventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["DATA"];
        DataTable dt1 = new DataTable();

        string tab_name = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB");

        co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");

        string cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONCOND");

        if (cond.Length > 2) cond = " and " + cond;
        //*************
        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        string squery = "select distinct trim(id) as fstr,web_Action,trim(text) as Text,SEARCH_KEY ,trim(id) as id from FIN_MSYS where trim(nvl(web_Action,'-'))!='-' and trim(id) in (select trim(id) from " + tab_name + " where 1=1 " + cond + " ) and NVL(VISI,'Y')!='N' order by trim(text),trim(id)";

        //query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        query1 = squery;
        dt1 = fgen.search_vip(frm_qstr, co_cd, query1, txtsearch.Text.Trim().ToUpper());
        if (dt1.Rows.Count > 0 && dt1 != null)
        {
            GridView1.DataSource = dt1;
            GridView1.DataBind(); dt1.Dispose();
            lblTotcount.Text = "Total Rows : " + GridView1.Rows.Count;
        }
        else
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
    }
    protected void tkrow_TextChanged(object sender, EventArgs e)
    {
        fill_data();
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //e.Row.Attributes["onclick"] = string.Format("sortTable({0});", e.Row.RowIndex);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0) e.Row.TabIndex = 1;
            else e.Row.TabIndex = 2;
            if (Convert.ToDouble(e.Row.RowIndex.ToString()) == 0) e.Row.Attributes["onfocus"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "if (event.keyCode != 13) { javascript:return SelectSibling(event); }";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
    protected void GridView1_Sorting1(object sender, GridViewSortEventArgs e)
    {
        var D = (DataTable)ViewState["sg1"];
        Image sortImage = new Image();
        if (ViewState["sortDir"] == null) ViewState["sortDir"] = " ASC";
        if ((string)ViewState["sortDir"] == " ASC")
        {
            ViewState["sortDir"] = " DESC";
            D.DefaultView.Sort = e.SortExpression + (string)ViewState["sortDir"];
        }
        else
        {
            ViewState["sortDir"] = " ASC";
            D.DefaultView.Sort = e.SortExpression + (string)ViewState["sortDir"];
        }
        gvSortExpression = e.SortExpression;
        GridView1.DataSource = D;
        GridView1.DataBind();
        lblTotcount.Text = "Total Rows : " + GridView1.Rows.Count;
    }
    string gvSortExpression { get; set; }
    protected void btnhide_Click(object sender, EventArgs e)
    {
        int iconLevel = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", "");
        iconLevel = (iconLevel - 1);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", (iconLevel).ToString());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        switch (HCID)
        {
            case "BOOKMARK":
                if (iconLevel < 1)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "EXIT");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", "EXIT");
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
            default:
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
        }
    }
    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "EXIT");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", "EXIT");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
    }
}
class msys
{
    public string id { get; set; }
    public string webAction { get; set; }
    public string text { get; set; }
    public string description { get; set; }
}