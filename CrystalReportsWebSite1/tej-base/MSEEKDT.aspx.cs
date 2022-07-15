using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class MSEEKDT : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "", Value2 = "", Value3 = "", Value4 = "", Value5 = "", Value6 = "", Value7 = "", Value8 = "", Value9 = "", Value10 = "";
    string HCID, co_cd; int col_count = 0;
    string frm_qstr, frm_url, frm_cocd, frm_mbr;
    fgenDB fgen = new fgenDB();

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
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                }
            }
            //--------------------------            
            co_cd = frm_cocd;

            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            //try
            //{
            //    if (Request.Cookies["U_SEEKSQL_" + frm_qstr] != null)
            //    {
            //        HCID = Request.Cookies["U_XID_" + frm_qstr].Value.ToString().Trim();
            //        query1 = Request.Cookies["U_SEEKSQL_" + frm_qstr].Value.ToString().Trim();
            //        if (query1.Length < 5) query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            //    }
            //}
            //catch { query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL"); }
            if (!Page.IsPostBack) { fill_data(); }

            txtsearch.Attributes["onkeydown"] = "if (event.keyCode == 40) { $('[TabIndex=1]').focus(); }";
            txtsearch.Focus();

            if (GridView1.Rows.Count > 0)
            {
                col_count = GridView1.HeaderRow.Cells.Count;
                double wid = 0;
                for (int i = 0; i < col_count; i++)
                {
                    if (i > 0)
                    {
                        double ad = 50;
                        ad = 10;
                        if (GridView1.Rows[0].Cells[i].Text.Trim().Length < 2) ad = 30;
                        else if (GridView1.Rows[0].Cells[i].Text.Trim().Length < 5) ad = 25;
                        else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 50) ad = 2;
                        else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 25) ad = 5;
                        else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 20) ad = 8;
                        wid += fgen.make_double(GridView1.Rows[0].Cells[i].Text.Trim().Length, 0) * ad;
                    }
                }
                try { GridView1.Width = Convert.ToUInt16(wid + 1); }
                catch { GridView1.Width = 1500; }
                if (col_count > 15 && GridView1.Width.ToString().toDouble() < 2000)
                    GridView1.Width = 2000;

                if (GridView1.Width.Value <= 800) GridView1.Width = Unit.Percentage(100);
            }
        }
    }
    public void fill_data()
    {
        int vartkrow;
        if (query1 == null || query1 == "0" || query1 == "") { }
        else
        {
            dt = (DataTable)Session["send_dt"];
            ViewState["sg1"] = dt;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            lblTotcount.Text = "Total Rows : " + GridView1.Rows.Count;
            dt.Dispose();
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);            
            e.Row.ToolTip = "Click to select this row.";
            e.Row.Cells[1].Visible = false;
            GridView1.HeaderRow.Cells[1].Visible = false;

            if (ViewState["hf"] != null)
            {
                hf1.Value = ViewState["hf"].ToString();
                if (hf1.Value != "")
                {
                    if (hf1.Value.ToUpper().Trim().Contains(e.Row.Cells[1].Text.ToUpper().Trim()))
                    {
                        ((CheckBox)e.Row.FindControl("btnchk")).Checked = true;
                    }
                }
            }
        }
    }
    protected void srch_Click(object sender, EventArgs e)
    {
        store();

        ImageButton btn = (ImageButton)sender;
        if (btn != srch) return;
        if (tkrow.Text == "0") tkrow.Text = "200";
        DataTable dt1 = new DataTable();
        query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        //try
        //{
        //    if (Request.Cookies["U_SEEKSQL_" + frm_qstr] != null)
        //    {
        //        query1 = Request.Cookies["U_SEEKSQL_" + frm_qstr].Value.ToString().Trim();
        //        if (query1.Length < 5) query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        //    }
        //}
        //catch { }
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
            if (dt1.Rows.Count > 0)
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
    }
    protected void tkrow_TextChanged(object sender, EventArgs e)
    {
        fill_data();
    }
    protected void imgproc_Click(object sender, ImageClickEventArgs e)
    {
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID") != "--")
        {
            ImageButton btn = (ImageButton)sender;
            if (btn != imgproc) return;
            if (GridView1.Rows.Count <= 0) return;
            if (GridView1.HeaderRow.Cells.Count <= 0)
            {
                return;
            }
            int iColumnas = GridView1.HeaderRow.Cells.Count - 1;
            int k = 0;
            //foreach (GridViewRow row in GridView1.Rows)
            //{
            //    CheckBox chk1 = (CheckBox)row.FindControl("btnchk");
            //    if (chk1.Checked == true)
            //    {
            //        Value1 += hf1.Value + "," + "'" + row.Cells[1].Text.Trim() + "'";
            //        Value2 += hf1.Value + "," + "'" + row.Cells[2].Text.Trim() + "'";

            //        if (iColumnas >= 3) Value3 += hf1.Value + "," + "'" + row.Cells[3].Text.Trim() + "'";
            //        if (iColumnas >= 4) Value4 += hf1.Value + "," + "'" + row.Cells[4].Text.Trim() + "'";
            //        if (iColumnas >= 5) Value5 += hf1.Value + "," + "'" + row.Cells[5].Text.Trim() + "'";
            //        if (iColumnas >= 6) Value6 += hf1.Value + "," + "'" + row.Cells[6].Text.Trim() + "'";
            //        if (iColumnas >= 7) Value7 += hf1.Value + "," + "'" + row.Cells[7].Text.Trim() + "'";
            //        if (iColumnas >= 8) Value8 += hf1.Value + "," + "'" + row.Cells[8].Text.Trim() + "'";
            //        if (iColumnas >= 9) Value9 += hf1.Value + "," + "'" + row.Cells[9].Text.Trim() + "'";
            //        if (iColumnas >= 10) Value10 += hf1.Value + "," + "'" + row.Cells[10].Text.Trim() + "'";

            //        k++;
            //    }
            //}
            store();

            Value1 = (hf1.Value);
            Value2 = (hf2.Value);
            Value3 = (hf3.Value);
            Value4 = (hf4.Value);
            Value5 = (hf5.Value);
            Value6 = (hf6.Value);
            Value7 = (hf7.Value);
            Value8 = (hf8.Value);
            Value9 = (hf9.Value);
            Value10 = (hf10.Value);

            if (frm_cocd == "SFLG")
            {
                if (k > 999)
                {
                    fgen.msg("-", "AMSG", "Can not select more then 999 Rows!!");
                    return;
                }
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_TYPSTRING", Value1);
            if (Value1.Length > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", Value2.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", Value3.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", Value4.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", Value5.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", Value6.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", Value7.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", Value8.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", Value9.TrimStart(','));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", Value10.TrimStart(','));

                //fgen.send_cookie("U_COL1", Value1.TrimStart(','));
                //fgen.send_cookie("U_COL2", Value2.TrimStart(','));
                //fgen.send_cookie("U_COL3", Value3.TrimStart(','));
                //fgen.send_cookie("U_COL4", Value4.TrimStart(','));
                //fgen.send_cookie("U_COL5", Value5.TrimStart(','));
                //fgen.send_cookie("U_COL6", Value6.TrimStart(','));
                //fgen.send_cookie("U_COL7", Value7.TrimStart(','));
                //fgen.send_cookie("U_COL8", Value8.TrimStart(','));
                //fgen.send_cookie("U_COL9", Value9.TrimStart(','));
                //fgen.send_cookie("U_COL10", Value10.TrimStart(','));

                switch (HCID)
                {
                    case "Tejaxo":
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
                        break;
                    case "Tejaxo_S":
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF_s');", true);
                        break;
                    case "DATA":
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
                        break;
                    case "YR":
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ctl00_btnhideF');", true);
                        break;
                    case "IBOX":
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btniBox');", true);
                        break;
                }
            }
            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "--");
        }
    }
    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_excel(dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("", "AMSG", "No Data to Export");
        dt.Dispose();
    }
    protected void btnexptopdf_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_pdf(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_word(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0) e.Row.TabIndex = 1;
            else e.Row.TabIndex = 2;
            if (Convert.ToDouble(e.Row.RowIndex.ToString()) == 0) e.Row.Attributes["onfocus"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "if (event.keyCode != 13) { javascript:return SelectSibling(event); } else { document.getElementById('imgproc').click(); }";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
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
            case "FINSYS":
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
        }
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        var D = (DataTable)ViewState["sg1"];
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
        GridView1.DataSource = D;
        GridView1.DataBind();
    }
    void store()
    {
        if (GridView1.Rows.Count > 0)
        {
            int iColumnas = GridView1.HeaderRow.Cells.Count - 1;
            int k = 0;
            if (ViewState["hf"] != null) hf1.Value = ViewState["hf"].ToString();
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chk1 = (CheckBox)row.FindControl("btnchk");
                if (chk1.Checked == true)
                {
                    hf1.Value += "," + "'" + row.Cells[1].Text.Trim() + "'";
                    hf2.Value += "," + "'" + row.Cells[2].Text.Trim() + "'";

                    if (iColumnas >= 3) hf3.Value += "," + "'" + row.Cells[3].Text.Trim() + "'";
                    if (iColumnas >= 4) hf4.Value += "," + "'" + row.Cells[4].Text.Trim() + "'";
                    if (iColumnas >= 5) hf5.Value += "," + "'" + row.Cells[5].Text.Trim() + "'";
                    if (iColumnas >= 6) hf6.Value += "," + "'" + row.Cells[6].Text.Trim() + "'";
                    if (iColumnas >= 7) hf7.Value += "," + "'" + row.Cells[7].Text.Trim() + "'";
                    if (iColumnas >= 8) hf8.Value += "," + "'" + row.Cells[8].Text.Trim() + "'";
                    if (iColumnas >= 9) hf9.Value += "," + "'" + row.Cells[9].Text.Trim() + "'";
                    if (iColumnas >= 10) hf10.Value += "," + "'" + row.Cells[10].Text.Trim() + "'";

                    k++;
                }
            }
            if (k > 0)
                ViewState["hf"] = hf1.Value;
            if (frm_cocd == "SFLG")
            {
                if (k > 999)
                {
                    fgen.msg("-", "AMSG", "Can not select more then 999 Rows!!");
                    return;
                }
            }
        }
    }
    string removeduplicate(string valfield)
    {
        string result = "";
        foreach (string item in valfield.Split(','))
        {
            if (!result.Contains(item))
            {
                result += "," + item;
            }
        }
        return result;
    }

    protected void GridView1_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        store();
        GridView1.PageIndex = e.NewPageIndex;
        DataTable dt1 = new DataTable();
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        dt1 = dt;
        ViewState["sg1"] = dt1;
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
}