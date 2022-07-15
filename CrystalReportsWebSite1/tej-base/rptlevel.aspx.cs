using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using System.Globalization;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;

public partial class rptlevel : System.Web.UI.Page
{
    fgenDB fgen = new fgenDB();
    string Squery, co_cd, frm_url, frm_qstr, frm_formID; DataTable dt;
    int col_count = 0;
    string frm_mbr = "";
    string ind_curr = "Y";
    int totCol = 50;
    string gvSortExpression { get; set; }
    protected void Pre_Init(object sender, EventArgs e)
    {
        sg1.Sorting += new GridViewSortEventHandler(sg1_Sorting);
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
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                }
            }
            //--------------------------                                    
            Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMDRILLID", frm_formID);
            ind_curr = fgen.seek_iname(frm_qstr, co_cd, "select trim(opt_param) as curr from fin_rsys_opt_pw where branchcd='" + frm_mbr + "' and opt_id='W2015'", "curr");

            if (!Page.IsPostBack)
            {
                setDrillLevel(0, 0, "");

            }
            txtsearch.Attributes["onkeydown"] = "if (event.keyCode == 40) { $('[TabIndex=1]').focus(); }";

            txtsearch.Focus();
        }
    }
    public void fill_grid(string gridQuery)
    {
        hfqry.Value = ""; dt = new DataTable();
        if (gridQuery.Length > 5)
        {
            dt = fgen.getdata(frm_qstr, co_cd, "select * from ( " + gridQuery + " ) where rownum<=" + tkrow.Text.Trim() + "");
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID");
        }
        else
        {
            dt = (DataTable)Session["send_dt"];
        }
        if (dt.Rows.Count > 0)
        {
            fillGrid();
        }
        else
        {
            sg1.DataSource = null;
            sg1.DataBind();
        }
        lblTotcount.InnerText = "Total Rows : " + sg1.Rows.Count;
        hfqry.Value = Squery;
    }

    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        searcFun();
    }

    void fillGrid()
    {
        DataTable neWDt = dt.Copy();
        ViewState["sg1"] = neWDt;
        makeColNameAsMine(dt);
        sg1.DataSource = dt;
        sg1.DataBind();
        hideAndRenameCol();
        sg1.Columns[0].Visible = false;
    }

    void makeColNameAsMine(DataTable dtColNameTable)
    {
        int colFound = dtColNameTable.Columns.Count;
        int colSrno = 1;
        for (int i = 0; i <= totCol + 1; i++)
        {
            if (colFound > i) dtColNameTable.Columns[i].ColumnName = "sg1_f" + colSrno;
            else dtColNameTable.Columns.Add("sg1_f" + colSrno, typeof(string));
            colSrno++;
        }
    }

    void hideAndRenameCol()
    {
        DataTable dtColNameTab = (DataTable)ViewState["sg1"];        
        int colFound = dtColNameTab.Columns.Count;
        int totResrvCol = 0;
        int x = 0;
        x = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "FRMWINDOWSIZE"));
        if (x == 0) x = 1000;
        double totWidth = 0;
        int widthMake = 0;
        for (int i = totResrvCol; i <= totCol; i++)
        {
            if (colFound + totResrvCol > i)
            {
                sg1.HeaderRow.Cells[i].Text = dtColNameTab.Columns[i].ColumnName;
                if (sg1.Rows.Count > 0)
                {
                    widthMake = (sg1.Rows[0].Cells[i].Text.Length) * 10;
                    if (widthMake < 50) widthMake = 50;
                    if (widthMake > 200) widthMake = 200;
                    totWidth += widthMake.ToString().toDouble();
                    sg1.Columns[i].HeaderStyle.Width = widthMake;
                    sg1.Columns[i].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    if (sg1.Columns[i].HeaderStyle.CssClass == "hidden")
                    {
                        if (i > 0)
                        {
                            sg1.Columns[i].HeaderStyle.CssClass = "";
                            sg1.Rows[0].Cells[i].CssClass = "";
                        }
                    }
                }
            }
            else
            {
                try
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[0].Cells[i].CssClass = "hidden";
                }
                catch { }
            }
        }
    }
    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_excel(dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnexptopdf_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_pdf(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_word(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        int drillBack = fgen.make_int(hfLevel.Value);
        string selVal = "";
        for (int z = 0; z < lblMsgSel.InnerText.Split(':').Length - 1; z++)
        {
            selVal += lblMsgSel.InnerText.Split(':')[z].ToString();
        }
        lblMsgSel.InnerText = selVal;
        drillBack = drillBack - 1;
        if (drillBack == 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_LVAL", "");
        }
        if (drillBack < 0)
        {
            if (drillBack == -1)
            {
                hfLevel.Value = "-1";
                lblMsg.InnerText = "Press Esc/Back Button one more time to Exit";
            }
            if (drillBack < -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyClose();", true);
            }
        }
        else setDrillLevel(drillBack, drillBack + 1, "");
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        return;
        txtsearch.Text = "";
        var grid = (GridView)sender;
        GridViewRow row = sg1.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (selectedCellIndex < 0) selectedCellIndex = 0;
        string mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading        
        if (selectedCellIndex > 0) selectedCellIndex -= 1;

        string Value1 = row.Cells[1].Text.Trim();
        string Value2 = row.Cells[3].Text.Trim();
        if (Convert.ToInt32(hfLevel.Value.toDouble().ToString()) == -1) hfLevel.Value = "0";
        int drillPost = Convert.ToInt32(hfLevel.Value.toDouble().ToString());
        drillPost = drillPost + 1;
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID");
        switch (frm_formID)
        {
            case "F70156":
            case "F70189":
                if ((drillPost - 1) == 0)
                {
                    if (selectedCellIndex < 4)
                    {
                        Value2 = row.Cells[3].Text.Trim();
                        Value1 = row.Cells[7].Text.Trim();
                    }
                    else
                    {
                        Value1 = row.Cells[8].Text.Trim();
                        Value2 = row.Cells[5].Text.Trim();
                    }
                }
                break;
        }

        if (setDrillLevel(drillPost, drillPost - 1, Value1).Length > 2)
        {
            if (Value2 != "-") lblMsgSel.InnerText += " : " + Value2.Replace("&amp;", "&");
            lblMsgSel.InnerText = lblMsgSel.InnerText.Replace(": All", "");
        }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "N");
            // here to add any condition
            if (frm_formID == "F70556" && drillPost == 3)
            {
                hideAndRenameCol();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1);
                if (Value1.Length > 2)
                {
                    string myFormName = "", myFormID = "";
                    switch (Value1.Substring(2, 1))
                    {
                        case "5":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_pinv_entry.aspx";
                            myFormID = "@F70116";
                            break;
                        case "4":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_inv_entry.aspx";
                            myFormID = "@F70116";
                            break;
                        case "2":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_inv_entry.aspx";
                            myFormID = "@F70116";
                            break;
                    }


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + myFormName + "?STR=" + frm_qstr + myFormID + "','98%','98%','');", true);
                }
            }
        }
    }
    string setDrillLevel(int dLevel, int oldDLevel, string valSelected)
    {
        if (dLevel == 0) lblMsgSel.InnerText = " : All";

        Squery = fgen.getDrillQuery(dLevel, frm_qstr);
        Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        fill_grid(Squery);
        btnBack.Visible = false;
        return Squery;
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex);
            //e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";            
            

            //****************

            //for (int i = 0; i < e.Row.Cells.Count - 1; i++)
            {
                //TableCell cell = e.Row.Cells[i];
                //cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                //cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                //cell.ToolTip = "You can click this cell";
                //cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }

            //e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";
        }
    }

    //protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        
    //    }
    //}


    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
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
    protected void txtsearch_TextChanged(object sender, EventArgs e)
    {
        searcFun();
    }
    void searcFun()
    {
        DataTable dt1 = new DataTable();
        Squery = hfqry.Value;
        if (hfqry.Value.Length > 5)
        {
            Squery = hfqry.Value;
        }
        else if (Session["send_dt"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["send_dt"];
        }
        if (txtsearch.Text.Length <= 0) Squery = "select * from (" + Squery + ") where rownum<=" + fgen.make_double(tkrow.Text.Trim()) + "";

        //dt = new DataTable();
        //dt = (DataTable)ViewState["sg1"];
        //{
        //    dt = new DataTable();
        //    dt = fgen.search_vip(frm_qstr, co_cd, Squery, txtsearch.Text.Trim().ToUpper());
        //}

        if (txtsearch.Text == "")
        {
            if (hfqry.Value.Length > 5)
            {
                dt1 = fgen.getdata(frm_qstr, co_cd, "select * from ( " + Squery + " ) where rownum<=" + tkrow.Text.Trim() + "");
            }
            else
            {
                dt1 = fgen.searchDataTable(txtsearch.Text, dt);
            }
        }
        else
        {
            if (hfqry.Value.Length > 5)
            {
                dt1 = fgen.search_vip(frm_qstr, co_cd, Squery, txtsearch.Text.Trim().ToUpper());
            }
            else
            {
                dt1 = fgen.searchDataTable(txtsearch.Text, dt);
            }
        }

        {
            dt = new DataTable();
            dt = dt1;
            dt1.Dispose();

            fillGrid();
            lblTotcount.InnerText = "Total Rows : " + sg1.Rows.Count;
        }        
    }
    void makeSum(DataTable newDt)
    {
        if (newDt.Rows.Count > 0)
        {
            DataRow dro = newDt.NewRow();

            foreach (DataColumn dc in newDt.Columns)
            {
                if (dc.Ordinal == 2)
                {
                    dro[2] = "Total";
                }
                else
                {
                    double mysum = 0;
                    foreach (DataRow drc in newDt.Rows)
                    {
                        if (dc.ColumnName.ToString().ToUpper() == "FSTR" || dc.ColumnName.ToString().ToUpper() == "GSTR" || dc.ColumnName.ToString().ToUpper() == "ERPCODE" || dc.ColumnName.ToString().ToUpper() == "PRODUCT" || dc.ColumnName.ToString().ToUpper() == "ITEMNAME" || dc.ColumnName.ToString().ToUpper() == "ITEM_NAME"
                    || dc.ColumnName.ToString().ToUpper() == "CUSTOMER" || dc.ColumnName.ToString().ToUpper() == "PARTY" || dc.ColumnName.ToString().ToUpper() == "PARTY_NAME" || dc.ColumnName.ToString().ToUpper() == "CUSTOMER_NAME"
                            || dc.ColumnName.ToString().ToUpper() == "PART_NO" || dc.ColumnName.ToString().ToUpper() == "CPARTNO" || dc.ColumnName.ToString().ToUpper() == "VDD" || dc.ColumnName.ToString().ToUpper() == "STAGE" || dc.ColumnName.ToString().ToUpper() == "JOB_CARD"
                            || dc.ColumnName.ToString().ToUpper() == "YLB" || dc.ColumnName.ToString().ToUpper() == "PARTNO" || dc.ColumnName.ToString().ToUpper() == "PALLET_NO" || dc.ColumnName.ToString().ToUpper() == "BATCH_NO"
                            || dc.ColumnName.ToString().ToUpper() == "CUSTOMER_CODE" || dc.ColumnName.ToString().ToUpper() == "CUST_PO_NO" || dc.ColumnName.ToString().ToUpper() == "ITEM_CODE"
                            || dc.ColumnName.ToString().ToUpper() == "ENTRY_NO" || dc.ColumnName.ToString().ToUpper() == "ENTRY_DT" || dc.ColumnName.ToString().ToUpper() == "UNIT" || dc.ColumnName.ToString().ToUpper() == "GRP" || dc.ColumnName.ToString().ToUpper() == "ENTRYNO" || dc.ColumnName.ToString().ToUpper() == "ORDER_NO") { dro[dc] = "-"; }
                        else
                        {
                            mysum += drc[dc].ToString().toDouble(2);
                            dro[dc] = mysum > 0 ? mysum.ToString() : "";
                        }
                    }
                }
            }
            newDt.Rows.InsertAt(dro, 0);
        }
    }
    protected void btnexptocsv_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0)
        {
            string zipFilePath = "c:\\TEJ_erp\\Upload\\" + co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";
            fgen.CreateCSVFile(dt, zipFilePath);
            Session["FileName"] = co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";
            Session["FilePath"] = co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";

            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");

        }
        else fgen.msg("-", "AMSG", "No Data to Export");
        dt.Dispose();
    }
    protected void sg1_Sorting(object sender, GridViewSortEventArgs e)
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
        sg1.DataSource = D;
        sg1.DataBind();
    }

}