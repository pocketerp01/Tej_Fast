using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.ComponentModel;
using System.Windows;

using System.Globalization;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;

public partial class drillDown : System.Web.UI.Page
{
    fgenDB fgen = new fgenDB();
    string Squery, co_cd, frm_url, frm_qstr, frm_formID; DataTable dt;
    int col_count = 0;
    string frm_mbr = "";
    string ind_curr = "Y";
    int totCol = 50;
    string gvSortExpression { get; set; }
    bool runOnce = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        //CultureInfo myCultureInfo = new CultureInfo("en-IN");
        Page.Culture = "en-In";
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
            //Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
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
            string mhc = "";
            dt = fgen.getdata(frm_qstr, co_cd, "select * from ( " + gridQuery + " ) where rownum<=" + tkrow.Text.Trim() + "");
            //mhc = (ind_curr == "INR" || ind_curr == "IND") ? "en-IN" : "en-US";
            //CultureInfo myCultureInfo = new CultureInfo("" + mhc + "");
            //dt.Locale = myCultureInfo;
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID");
            switch (frm_formID)
            {
                case "F70156":
                case "F70189":
                    if (hfLevel.Value == "0")
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;
                        dt = new DataTable();
                        dt.Columns.Add("fstr");
                        dt.Columns.Add("gstr");
                        dt.Columns.Add("0");
                        dt.Columns.Add("1");
                        dt.Columns.Add("2");
                        dt.Columns.Add("3");
                        dt.Columns.Add("4");
                        dt.Columns.Add("5");
                        DataRow oporow = null;
                        double labamt = 0, astamt = 0;
                        for (int i = 0; i < (dt1.Rows.Count + 5); i++)
                        {
                            oporow = dt.NewRow();
                            dt.Rows.Add(oporow);
                        }
                        int xx = 0, xx2 = 0;
                        double totvalCol1 = 0, totvalCol2 = 0;
                        if (frm_formID == "F70189")
                        {
                            foreach (DataRow dr1 in dt1.Rows)
                            {
                                if (dr1["group_code"].ToString().Substring(0, 1) != "2")
                                {
                                    dt.Rows[xx]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx]["0"] = dr1["group_name"];
                                    labamt = dr1["expenses"].ToString().toDouble() - dr1["incomes"].ToString().toDouble();
                                    dt.Rows[xx]["1"] = (labamt < 0 ? Math.Abs(labamt).ToString() + " Cr" : labamt.ToString());
                                    dt.Rows[xx]["4"] = dr1["group_code"];
                                    xx++;
                                    totvalCol1 += labamt.toDouble(2);
                                }
                                else
                                {
                                    dt.Rows[xx2]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx2]["2"] = dr1["group_name"];
                                    astamt = dr1["incomes"].ToString().toDouble() - dr1["expenses"].ToString().toDouble();
                                    dt.Rows[xx2]["3"] = (astamt < 0 ? Math.Abs(astamt).ToString() + " Cr" : astamt.ToString());
                                    dt.Rows[xx2]["5"] = dr1["group_code"];
                                    xx2++;
                                    totvalCol2 += astamt.toDouble(2);
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow dr1 in dt1.Rows)
                            {
                                if (dr1["group_code"].ToString().Substring(0, 1) == "0")
                                {
                                    dt.Rows[xx]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx]["0"] = dr1["group_name"];
                                    labamt = dr1["Liabilities"].ToString().toDouble() - dr1["assets"].ToString().toDouble();
                                    dt.Rows[xx]["1"] = (labamt < 0 ? Math.Abs(labamt).ToString() + " Cr" : labamt.ToString());
                                    dt.Rows[xx]["4"] = dr1["group_code"];
                                    xx++;
                                    totvalCol1 += labamt.toDouble(2);
                                }
                                else
                                {
                                    dt.Rows[xx2]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx2]["2"] = dr1["group_name"];
                                    astamt = dr1["assets"].ToString().toDouble() - dr1["Liabilities"].ToString().toDouble();
                                    dt.Rows[xx2]["3"] = (astamt < 0 ? Math.Abs(astamt).ToString() + " Cr" : astamt.ToString());
                                    dt.Rows[xx2]["5"] = dr1["group_code"];
                                    xx2++;
                                    totvalCol2 += astamt.toDouble(2);
                                }
                            }
                        }
                        double prof_fig = 0;
                        prof_fig = Math.Round(totvalCol2 - totvalCol1, 2);
                        astamt = 0;
                        labamt = 0;
                        if (prof_fig != 0)
                        {
                            if (prof_fig > 0)
                            {
                                dt.Rows[xx]["fstr"] = "-";
                                dt.Rows[xx]["0"] = "Profit/Surplus";
                                labamt = prof_fig;
                                dt.Rows[xx]["1"] = (labamt < 0 ? Math.Abs(labamt).ToString() + " Cr" : labamt.ToString());
                                dt.Rows[xx]["4"] = "-";
                                xx++;
                            }
                            else
                            {
                                dt.Rows[xx2]["fstr"] = "-";
                                dt.Rows[xx2]["2"] = "Loss/Deficit";
                                astamt = prof_fig;
                                dt.Rows[xx2]["3"] = (astamt < 0 ? Math.Abs(astamt).ToString() + " Cr" : astamt.ToString());
                                dt.Rows[xx2]["5"] = "-";
                                xx2++;
                            }
                        }

                        totvalCol1 += Math.Abs(labamt).toDouble(2);
                        totvalCol2 += Math.Abs(astamt).toDouble(2);

                        dt.Rows[dt.Rows.Count - 3]["1"] = "----------------------------";
                        dt.Rows[dt.Rows.Count - 2]["1"] = totvalCol1.toDouble(2);
                        dt.Rows[dt.Rows.Count - 1]["1"] = "----------------------------";

                        dt.Rows[dt.Rows.Count - 3]["3"] = "----------------------------";
                        dt.Rows[dt.Rows.Count - 2]["3"] = totvalCol2.toDouble(2);
                        dt.Rows[dt.Rows.Count - 1]["3"] = "----------------------------";
                    }
                    break;
            }
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
    void searchFunc()
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
            dt = dt1;
            fillGrid();
            lblTotcount.InnerText = "Total Rows : " + sg1.Rows.Count;
        }
        dt1.Dispose();
    }
    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        searchFunc();
    }

    void fillGrid()
    {
        if (dt != null)
        {
            if (!dt.Columns.Contains("FSTR")) dt.Columns.Add("FSTR").SetOrdinal(0);
            if (!dt.Columns.Contains("GSTR")) dt.Columns.Add("GSTR").SetOrdinal(1);
            DataTable neWDt = dt.Copy();
            ViewState["sg1"] = neWDt;
            makeColNameAsMine(dt);
            sg1.DataSource = dt;
            sg1.DataBind();
            hideAndRenameCol();
        }
        else
        {
            sg1.DataSource = null;
            sg1.DataBind();
        }
    }

    void makeColNameAsMine(DataTable dtColNameTable)
    {
        int colFound = dtColNameTable.Columns.Count;
        int colSrno = 1;
        for (int i = 2; i <= totCol + 1; i++)
        {
            if (colFound > i) dtColNameTable.Columns[i].ColumnName = "sg1_f" + colSrno;
            else dtColNameTable.Columns.Add("sg1_f" + colSrno, typeof(string));
            colSrno++;
        }
    }

    void hideAndRenameCol()
    {
        DataTable dtColNameTab = (DataTable)ViewState["sg1"];
        string MCOLS_2RALIGN = ""; string MCOLS_2RESIZE = ""; string MCOLS_WIDTHS = ""; string MCOLS_2RALIGNdeci = "";
        int colFound = dtColNameTab.Columns.Count;
        int totResrvCol = 1;
        int x = 0;
        x = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "FRMWINDOWSIZE"));
        if (x == 0) x = 1000;
        double totWidth = 0;
        int widthMake = 0;
        for (int i = totResrvCol; i <= totCol + 2; i++)
        {
            if (colFound + totResrvCol > i)
            {
                sg1.HeaderRow.Cells[i].Text = dtColNameTab.Columns[i - totResrvCol].ColumnName;
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
                        if (i > 2)
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
        if (totWidth < 1200 && x > 600 && sg1.Rows.Count > 0)
        {
            double gridFoundWidth = Math.Round(x * .93);
            string sepWidth = Math.Round((gridFoundWidth / (colFound - 2)) * 1).ToString();
            string restWidth = ((Convert.ToInt32(sepWidth)) / (colFound - 3)).ToString();
            int multp = 2;

            for (int i = 3; i <= colFound; i++)
            {
                if (i == 3) sg1.Columns[i].HeaderStyle.Width = (Convert.ToInt16(sepWidth) * multp);
                else sg1.Columns[i].HeaderStyle.Width = (Convert.ToInt16(sepWidth) - Convert.ToInt16(restWidth));
                totWidth += Convert.ToDouble(sepWidth);
            }
        }

        //---------------------------------------------2/8/2020 pkg

        int mvl1 = 0; int mvl2 = 0;
        int index = 0;
        string str1; string findstr;
        findstr = "#";

        MCOLS_2RALIGN = fgenMV.Fn_Get_Mvar(frm_qstr, "M_DQ_COLS_2RALIGN" + fgen.make_int(hfLevel.Value));
        MCOLS_2RESIZE = fgenMV.Fn_Get_Mvar(frm_qstr, "M_DQ_COLS_2RESIZE" + fgen.make_int(hfLevel.Value));
        MCOLS_WIDTHS = fgenMV.Fn_Get_Mvar(frm_qstr, "M_DQ_COLS_WIDTHS" + fgen.make_int(hfLevel.Value));

        mvl1 = 0;
        index = 0;
        while (MCOLS_2RALIGN.Contains("#"))
        {
            mvl1 = Convert.ToInt32(MCOLS_2RALIGN.Split('#')[0].ToString());

            sg1.Columns[mvl1].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            str1 = MCOLS_2RALIGN;
            index = str1.IndexOf(findstr);

            //****************** commas,decimal.00
            // string mhc = "";
            // mhc = (ind_curr == "INR" || ind_curr == "IND") ? "en-IN" : "en-US";
            // CultureInfo cultureInfo = new CultureInfo("" + mhc + "");
            // string omom1 = "";
            //omom1= fgen.make_double(fgen.make_double(sg1.Columns[mvl1].ToString()), 2, true);
            // if (omom1.toDouble() != 0)
            //     sg1.Columns[mvl1].AccessibleHeaderText = Convert.ToString(fgen.make_double(omom1).ToString("N", CultureInfo.CreateSpecificCulture("" + mhc + "")));
            // index = MCOLS_2RALIGNdeci.IndexOf(findstr);
            // if (index < 0)
            // { }
            // else
            // {
            //     MCOLS_2RALIGNdeci = fgen.Right(MCOLS_2RALIGNdeci, MCOLS_2RALIGNdeci.Length - (index + 1));
            // }
            //****************

            //sg1.Columns[mvl1].ItemStyle.value

            if (index < 0)
            { }
            else
            {
                MCOLS_2RALIGN = fgen.Right(MCOLS_2RALIGN, MCOLS_2RALIGN.Length - (index + 1));
            }
        }

        while (MCOLS_2RESIZE.Contains("#"))
        {
            mvl1 = Convert.ToInt32(MCOLS_2RESIZE.Split('#')[0].ToString());
            mvl2 = Convert.ToInt32(MCOLS_WIDTHS.Split('#')[0].ToString());

            sg1.Columns[mvl1].HeaderStyle.Width = mvl2;

            str1 = MCOLS_2RESIZE;
            index = str1.IndexOf(findstr);
            if (index < 0)
            { }
            else
            {
                MCOLS_2RESIZE = fgen.Right(MCOLS_2RESIZE, MCOLS_2RESIZE.Length - (index + 1));
            }

            str1 = MCOLS_WIDTHS;
            index = str1.IndexOf(findstr);
            if (index < 0)
            { }
            else
            {
                MCOLS_WIDTHS = fgen.Right(MCOLS_WIDTHS, MCOLS_WIDTHS.Length - (index + 1));
            }
        }

        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID");
        switch (frm_formID)
        {
            case "F70156":
                if (hfLevel.Value == "0")
                {
                    sg1.HeaderRow.Cells[3].Text = "L i a b i l i t i e s";
                    sg1.HeaderRow.Cells[4].Text = "Amount";
                    sg1.HeaderRow.Cells[5].Text = "A s s e t s";
                    sg1.HeaderRow.Cells[6].Text = "Amount";
                    sg1.HeaderRow.Cells[7].Text = "GrpCode";
                    sg1.HeaderRow.Cells[8].Text = "GrpCode";
                }
                break;
            case "F70189":
                if (hfLevel.Value == "0")
                {
                    sg1.HeaderRow.Cells[3].Text = "E x p e n s e s";
                    sg1.HeaderRow.Cells[4].Text = "Amount";
                    sg1.HeaderRow.Cells[5].Text = "I n c o m e s ";
                    sg1.HeaderRow.Cells[6].Text = "Amount";
                    sg1.HeaderRow.Cells[7].Text = "GrpCode";
                    sg1.HeaderRow.Cells[8].Text = "GrpCode";

                }
                break;
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
        //MakeStdRpt();
        if (dt.Rows.Count > 0) fgen.exp_to_pdf(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("", "AMSG", "No Data to Export"); dt.Dispose();
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
            if ((frm_formID == "F70556" && drillPost == 3) || (frm_formID == "F70189" || frm_formID == "F70156" && drillPost == 4))
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
                            myFormID = "@F50101";
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

        btnBack.Visible = true;
        if (Squery == "" || Squery == "0")
        {
            //"No Drill Further"            
        }
        else if (Squery.ToUpper() == "SEND_DT")
        {
            hfLevel.Value = "0";
            fill_grid("");
        }
        else
        {
            lblMsg.InnerText = "Drill Level - " + (dLevel + 1);
            string fstrGstr = "GSTR";
            if (oldDLevel > dLevel) fstrGstr = "GSTR";
            if (Squery.Contains("='GSTR'") || Squery.Contains("='FSTR'"))
            {
                if (valSelected == "") valSelected = fgenMV.Fn_Get_Mvar(frm_qstr, "U_LVAL" + dLevel);
                if (valSelected == "0" || valSelected == "") Squery = "" + Squery;
                else
                {
                    Squery = Squery.Replace("='GSTR'", "='" + valSelected + "'");
                    Squery = Squery.Replace("='FSTR'", "='" + valSelected + "'");
                    Squery = "select * from (" + Squery + ") ";
                }
            }
            else
            {
                if (valSelected == "") valSelected = fgenMV.Fn_Get_Mvar(frm_qstr, "U_LVAL" + dLevel);
                if (valSelected == "0" || valSelected == "") Squery = "" + Squery;
                else Squery = "select * from (" + Squery + ") where " + fstrGstr + "='" + valSelected + "' ";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_LVAL" + dLevel, valSelected);
            hfLevel.Value = dLevel.ToString();
            fill_grid(Squery);
        }
        return Squery;
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string findstr = ""; string mhc = "";
        string MCOLS_2RALIGNdeci = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";
            e.Row.ToolTip = "Click to select this row.";

            sg1.Columns[0].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[0].CssClass = "hidden";
            sg1.Columns[1].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[1].CssClass = "hidden";
            sg1.Columns[2].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[2].CssClass = "hidden";

            for (int i = 0; i < e.Row.Cells.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.ToolTip = "You can click this cell";
                cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }

            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";

            if (e.Row.Cells[3].Text.ToUpper() == "REPORT TOTAL" || e.Row.Cells[3].Text.ToUpper() == "GRANT TOTAL")
            {
                e.Row.BackColor = System.Drawing.Color.GreenYellow;
            }
        }

        if (runOnce != true)
        {
            int mvl1 = 0;
            int index = 0;
            findstr = "#";
            MCOLS_2RALIGNdeci = fgenMV.Fn_Get_Mvar(frm_qstr, "M_DQ_COLS_2RALIGN" + fgen.make_int(hfLevel.Value));

            mhc = (ind_curr == "INR" || ind_curr == "IND") ? "{0:#,###.##}" : "{0:n2}";

            while (MCOLS_2RALIGNdeci.Contains("#"))
            {
                mvl1 = Convert.ToInt32(MCOLS_2RALIGNdeci.Split('#')[0].ToString());

                ((BoundField)sg1.Columns[mvl1]).DataFormatString = mhc;

                index = MCOLS_2RALIGNdeci.IndexOf(findstr);
                if (index < 0)
                { }
                else
                {
                    MCOLS_2RALIGNdeci = fgen.Right(MCOLS_2RALIGNdeci, MCOLS_2RALIGNdeci.Length - (index + 1));
                }
            }

            runOnce = true;
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
        searchFunc();
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

    private void MakeStdRpt()
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Columns.Contains("FSTR")) dt.Columns.Remove("FSTR");
        if (dt.Columns.Contains("GSTR")) dt.Columns.Remove("GSTR");
        DataTable dt10Col = new DataTable();
        dt10Col.Columns.Add("Header", typeof(string));
        dt10Col.Columns.Add("FromDt", typeof(string));
        dt10Col.Columns.Add("ToDt", typeof(string));
        dt10Col.Columns.Add("F1", typeof(string));
        dt10Col.Columns.Add("F2", typeof(string));
        dt10Col.Columns.Add("F3", typeof(string));
        dt10Col.Columns.Add("F4", typeof(string));
        dt10Col.Columns.Add("F5", typeof(string));
        dt10Col.Columns.Add("F6", typeof(string));
        dt10Col.Columns.Add("F7", typeof(string));
        dt10Col.Columns.Add("F8", typeof(string));
        dt10Col.Columns.Add("F9", typeof(string));
        dt10Col.Columns.Add("F10", typeof(string));
        dt10Col.Columns.Add("H1", typeof(string));
        dt10Col.Columns.Add("H2", typeof(string));
        dt10Col.Columns.Add("H3", typeof(string));
        dt10Col.Columns.Add("H4", typeof(string));
        dt10Col.Columns.Add("H5", typeof(string));
        dt10Col.Columns.Add("H6", typeof(string));
        dt10Col.Columns.Add("H7", typeof(string));
        dt10Col.Columns.Add("H8", typeof(string));
        dt10Col.Columns.Add("H9", typeof(string));
        dt10Col.Columns.Add("H10", typeof(string));
        int colCount = 1, colIndex = 1, dtColIndex = 0;
        colCount = dt.Columns.Count;
        string fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        string todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        string header_n = fgenMV.Fn_Get_Mvar(frm_qstr, "U_HEADER");
        if (colCount > 10)
        {
            colCount = 10;
        }
        if (header_n == "0")
        {
            if (Session["dt_menu" + frm_qstr] != null)
            {
                DataTable dtx = new DataTable();
                dtx = (DataTable)Session["dt_menu" + frm_qstr];

                header_n = fgen.seek_iname_dt(dtx, "ID='" + frm_formID + "'", "TEXT");
            }
        }
        DataRow dr1 = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dr1 = dt10Col.NewRow();
            dr1["Header"] = header_n;
            dr1["FromDt"] = fromdt;
            dr1["ToDt"] = todt;
            colIndex = 1;
            dtColIndex = 0;
            for (int k = 1; k <= colCount; k++)
            {
                dr1["F" + k] = dt.Rows[i][dtColIndex].ToString().Trim(); // field's data
                dr1["H" + k] = dt.Columns[dtColIndex].ColumnName; // field column name
                colIndex++;
                dtColIndex++;
            }
            dt10Col.Rows.Add(dr1);
        }

        string xml = "10ColStd";
        DataSet data_set = new DataSet();
        string report = "10ColStd";
        frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
        dt10Col.TableName = "Prepcur";
        data_set.Tables.Add(dt10Col);
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";
        data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_cd, frm_mbr));
        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            string frm_FileName = co_cd + "_" + DateTime.Now.ToString().Trim();
            repDoc = GetReportDocument(data_set, rptfile);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, frm_FileName);
        }
    }
    ReportDocument repDoc = new ReportDocument();
    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        repDoc.Close();
        repDoc.Dispose();
    }
    private ReportDocument GetReportDocument(DataSet rptDS, string rptFileName)
    {
        string repFilePath = Server.MapPath("" + rptFileName + "");
        repDoc = new ReportDocument();
        repDoc.Load(repFilePath);
        repDoc.Refresh();
        repDoc.SetDataSource(rptDS);
        rptDS.Dispose();
        return repDoc;
    }
}