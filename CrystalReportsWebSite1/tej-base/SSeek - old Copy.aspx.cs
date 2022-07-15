using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


public partial class SSeek : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "-", Value2 = "-", Value3 = "-", Value4 = "-", Value5 = "-", Value6 = "-", Value7 = "-", Value8 = "-", Value9 = "-", Value10 = "-";
    string HCID, co_cd; int col_count = 0;
    string frm_qstr, frm_url, frm_cocd, frm_mbr, frm_formID;
    fgenDB fgen = new fgenDB();
    string gvSortExpression { get; set; }

    protected void Pre_Init(object sender, EventArgs e)
    {
        //GridView1.Sorting += new GridViewSortEventHandler(GridView1_Sorting);
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
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                }
            }
            //--------------------------                        
            if (frm_qstr.Contains("~"))
            {
                if (frm_cocd != frm_qstr.Split('~')[0].ToString())
                {
                    frm_cocd = frm_qstr.Split('~')[0].ToString();
                }
            }
            co_cd = frm_cocd;
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            if (!Page.IsPostBack) { fill_data(); }

            txtsearch.Attributes["onkeydown"] = "if (event.keyCode == 40) { $('[TabIndex=1]').focus(); }";
            
            if (GridView1.Rows.Count > 0)
            {
                col_count = GridView1.HeaderRow.Cells.Count;
                double wid = 0;
                for (int i = 0; i < col_count; i++)
                {
                    double ad = 50;
                    ad = 10;
                    if (GridView1.Rows[0].Cells[i].Text.Trim().Length < 2) ad = 30;
                    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length < 5) ad = 25;
                    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 50) ad = 2;
                    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 25) ad = 5;
                    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 20) ad = 8;
                    if (fgen.make_double(GridView1.Rows[0].Cells[i].Text.Length, 0) * ad > 180) wid += 180;
                    else wid += fgen.make_double(GridView1.Rows[0].Cells[i].Text.Length, 0) * ad;
                }
                try { GridView1.Width = Convert.ToUInt16(wid + 100); }
                catch { GridView1.Width = 1500; }

                if (GridView1.Width.Value <= 800) GridView1.Width = Unit.Percentage(100);
                if (GridView1.Width.Value > 1500) GridView1.Width = 1500; 

                //GridView1.UseAccessibleHeader = true;
                //GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            txtsearch.Focus();
        }
    }
    public void fill_data()
    {
        if (query1 == null || query1 == "0" || query1 == "") { }
        else
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, co_cd, "select * from ( " + query1 + " ) where rownum<=" + fgen.make_double(tkrow.Text.Trim()) + "");
            ViewState["sg1"] = dt;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            lblTotcount.Text = "Total Rows : " + GridView1.Rows.Count;
            dt.Dispose();
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = GridView1.SelectedRow;

        Value1 = row.Cells[1].Text.Trim();
        Value2 = row.Cells[2].Text.Trim();
        int iColumnas = GridView1.HeaderRow.Cells.Count - 1;

        if (iColumnas >= 3) Value3 = row.Cells[3].Text.Trim();
        if (iColumnas >= 4) Value4 = row.Cells[4].Text.Trim();
        if (iColumnas >= 5) Value5 = row.Cells[5].Text.Trim();
        if (iColumnas >= 6) Value6 = row.Cells[6].Text.Trim();
        if (iColumnas >= 7) Value7 = row.Cells[7].Text.Trim();
        if (iColumnas >= 8) Value8 = row.Cells[8].Text.Trim();
        if (iColumnas >= 9) Value9 = row.Cells[9].Text.Trim();
        if (iColumnas >= 10) Value10 = row.Cells[10].Text.Trim();

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", Value2);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", Value3);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", Value4);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", Value5);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", Value6);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", Value7);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", Value8);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", Value9);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", Value10);

        switch (HCID)
        {
            case "Tejaxo":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
                break;
            case "FINSYS_S":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF_s');", true);
                break;
            case "DATA":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
            case "YR":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
            case "IBOX":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btniBox');", true);
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
            e.Row.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
            GridView1.HeaderRow.Cells[0].Style["display"] = "none";
            GridView1.HeaderRow.Cells[1].Style["display"] = "none";

            if (frm_cocd == "TEST")
            {
                if (frm_formID == "F60101")
                {
                    if (e.Row.Cells.Count > 7)
                    {
                        if (e.Row.Cells[7].Text == "Y" || e.Row.Cells[13].Text.Trim().Length > 2)
                        {
                            e.Row.BackColor = System.Drawing.Color.FromName("#3c8dbc");
                            e.Row.ForeColor = System.Drawing.Color.WhiteSmoke;
                        }
                    }
                }
            }
        }
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
    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_excel(dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export");
        dt.Dispose();
    }
    protected void btnexptopdf_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_pdf(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export");
        dt.Dispose();
    }
    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_word(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export");
        dt.Dispose();
    }
    protected void srch_Click(object sender, EventArgs e)
    {
        DataTable dt1 = new DataTable();
        query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        if (tkrow.Text == "0") tkrow.Text = "200";
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        //if (txtsearch.Text.Length <= 0) query1 = "select * from (" + query1 + ") where rownum<=" + fgen.make_double(tkrow.Text.Trim()) + "";

        //dt1 = fgen.searchDataTable(txtsearch.Text.Trim(), dt);
        //if (dt1.Rows.Count <= 0)
        {
            dt1 = new DataTable();
            dt1 = fgen.search_vip1(frm_qstr, co_cd, query1, txtsearch.Text.Trim().ToUpper(), dt);
        }
        ViewState["sg1"] = dt1;
        if (dt1 != null)
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
    protected void btnhide_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", "");

        switch (HCID)
        {
            case "Tejaxo":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
                break;
            case "FINSYS_S":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF_s');", true);
                break;
            case "DATA":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
            case "YR":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                break;
            case "IBOX":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btniBox');", true);
                break;
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
    }
}