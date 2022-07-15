using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using CrystalDecisions.CrystalReports.Engine;

public partial class rptlevelJS : System.Web.UI.Page
{
    string Squery, co_cd, frm_url, frm_qstr, HCID, frm_mbr, fromdt, todt, header_n, formid, gridshow = "N", frm_CDT1;
    DataTable dt, dt10Col;
    fgenDB fgen = new fgenDB();
    ReportDocument repDoc = new ReportDocument();
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
                        formid = frm_qstr.Split('@')[1].ToString();
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                    }
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                }
            }
            //--------------------------                        
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
            formid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");

            if (!Page.IsPostBack) fill_grid();

            //double wid = 0;
            //for (int i = 0; i < col_count; i++)
            //{
            //    int ad = 50;
            //    ad = 10;
            //    if (GridView1.Rows[0].Cells[i].Text.Trim().Length < 2) ad = 30;
            //    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length < 5) ad = 25;
            //    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 50) ad = 2;
            //    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 25) ad = 5;
            //    else if (GridView1.Rows[0].Cells[i].Text.Trim().Length > 20) ad = 8;
            //    if (fgen.make_double(GridView1.Rows[0].Cells[i].Text.Length, 0) * ad > 180) wid += 180;
            //    else wid += fgen.make_double(GridView1.Rows[0].Cells[i].Text.Length, 0) * ad;

            //    widthMake = (GridView1.Rows[0].Cells[i].Text.Length * ad);

            //    //GridView1.Columns[i].HeaderStyle.Width = widthMake;
            //    //GridView1.HeaderRow.Cells[i].Width = widthMake;                    
            //    GridView1.Columns[i].HeaderStyle.Width = widthMake;
            //}
            //try { GridView1.Width = Convert.ToUInt16(wid + 100); }
            //catch { GridView1.Width = 1500; }

            //if (GridView1.Width.Value <= 800) GridView1.Width = Unit.Percentage(100);
            //if (GridView1.Width.Value > 1500) GridView1.Width = 1500; 

            //GridView1.UseAccessibleHeader = true;
            //GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

        }
    }
    public void fill_grid()
    {
        gridshow = "N";
        hfqry.Value = "";
        dt = new DataTable();
        Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        tdGrid.Visible = false;

        if (formid == "F10194")//bom costing
        {
            btnHelp.Visible = true;
            btnPrint.Visible = true;
        }
        else
        {
            btnHelp.Visible = false;
            btnPrint.Visible = false;
        }
        if (formid == "F10184")
        {
            btnPrint.Visible = true;
        }
        //gridshow = "Y";
        if (Squery.Length > 5)
        {
            if (co_cd == "AVON")
            {
                if (Squery.Contains("TEXT()"))
                {
                    Squery = Squery.Replace("TEXT()", "text()");
                }
            }
            formid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            dt = fgen.getdata(frm_qstr, co_cd, "select * from ( " + Squery + " ) where rownum<=" + tkrow.Text.Trim() + "");

            if (dt.Rows.Count > 40000)
            {
                gridshow = "N";
            }
            if (formid == "F70375") // BONY - HSN WISE NON MRR REPORT
            {
                gridshow = "Y";
            }
            if (gridshow == "Y")
            {
                DataTable neWDt = dt.Copy();
                ViewState["sg1"] = neWDt;
                makeColNameAsMine(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();

                hideAndRenameCol();
                tdDiv.Visible = false;
                tdGrid.Visible = true;
            }
            else
            {
                ViewState["sg1"] = dt;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "").ToString(), false);
            }

            //if (formid == "F70375")
            //{
            //    GridView1.DataSource = dt;
            //    GridView1.DataBind();
            //    tdDiv.Visible = false;
            //    tdGrid.Visible = true;
            //}
            //else
            //{
            //    if (dt.Rows.Count > 50000)
            //    {
            //        GridView1.DataSource = dt;
            //        GridView1.DataBind();
            //        tdDiv.Visible = false;
            //        tdGrid.Visible = true;
            //    }
            //    else
            //    {
            //        ViewState["sg1"] = dt;
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "").ToString(), false);
            //    }
            //}
        }
        else if (Session["send_dt"] != null)
        {
            dt = (DataTable)Session["send_dt"];
            ViewState["sg1"] = dt;
            if (dt.Rows.Count > 10000 || formid == "F40353" || formid == "F70377") // F40353 - YPPL OVERALL PRODUCTION REPORT .... F70377 - ADVG OUTSTANDING REPORT
            {
                //makeColNameAsMine(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                //hideAndRenameCol();
                tdDiv.Visible = false;
                tdGrid.Visible = true;
            }
            else ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "").ToString(), false);
        }
        if (dt.Rows.Count > 0)
        {
            datadiv.Visible = true; div2.Visible = false;
            lblTotcount.Text = "Total Rows : " + dt.Rows.Count;
        }
        else
        {
            datadiv.Visible = false; div2.Visible = true;
        }
        //hfqry.Value = Squery;

        if (dt.Rows.Count > 0)
        {
            if (formid == "F10194E" || formid == "F10198W" || formid == "F10194F" || formid == "F10198")
            {
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = "A";
                dr[1] = "B";
                dr[2] = "C";
                dr[3] = "D";
                dr[4] = "E";
                dr[5] = "F";
                dr[6] = "G";
                dr[7] = "H";
                dr[8] = "I";
                dr[9] = "J";
                dr[10] = "K";
                dr[12] = "L";
                //dr[13] = "M";
                dr[14] = "N";
                dr[15] = "O";
                dr[16] = "P=O*K";
                dr[17] = "Q";
                dr[18] = "R";
                dr[19] = "S=R * F";
                dr[20] = "T";
                dr[21] = "U=TxO";
                dr[22] = "V=SxO";
                dr[23] = "W=U+P";
                dr[24] = "X=V+P";

                dt.Rows.InsertAt(dr, 0);

                //dt.Columns[0].ColumnName = "Level";
                //dt.Columns[1].ColumnName = "ERP Item Code";
                //dt.Columns[2].ColumnName = "Item Name";

                //dt.Columns[3].ColumnName = "Child ERP Code";
                //dt.Columns[4].ColumnName = "Child Item Name";

                //dt.Columns[5].ColumnName = "Gross Wt as per BOM/pc";
                //dt.Columns[6].ColumnName = "Net Wt as per BOM/pc";
                //dt.Columns[7].ColumnName = "Qnty/pc (as per BOM)";

                //dt.Columns[8].ColumnName = "Rate taken ( parameter)";
                //dt.Columns[9].ColumnName = "UNIT";
                //dt.Columns[10].ColumnName = "BOM Cost (calculated)";
                //dt.Columns[11].ColumnName = "Gross Wt x Rate";

                //dt.Columns[12].ColumnName = "Net Wt x Rate";
                //dt.Columns[13].ColumnName = "Gross wt with Process Loss";
                //dt.Columns[14].ColumnName = "Cost incl process Loss";
                //dt.Columns[15].ColumnName = "Closing Stock Qnty";

                //dt.Columns[16].ColumnName = "Closing Stock Qnty x BOM Rate";
                //dt.Columns[17].ColumnName = "Closing Stock x Gross Wt";
                //dt.Columns[18].ColumnName = "Processing Cost Rate =from Finance Process Rate Masters/kg";
                //dt.Columns[19].ColumnName = "Processing Cost = Rate/kg x Net Wt";
                //dt.Columns[20].ColumnName = "Processing Cost = Rate/kg x Gross Wt";

                //dt.Columns[21].ColumnName = "Processing Cost = Rate/kg x Gross Wt x Closing Stock Qnty";
                //dt.Columns[22].ColumnName = "Processing Cost = Rate/kg x Net Wt x Closing Stock Qnty";
                //dt.Columns[23].ColumnName = "Material cost + Processing Cost on Gross Basis";
                //dt.Columns[24].ColumnName = "Material cost + Processing Cost on Net Basis";
                //colColNameAsMine(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                //hideAndRenameCol();
                tdDiv.Visible = false;
                tdGrid.Visible = true;
                if (co_cd == "SAGM")
                {
                    if (GridView1.Rows.Count > 0)
                    {
                        GridView1.Rows[0].BackColor = System.Drawing.Color.LightSkyBlue;
                    }
                    fgen.exp_to_excel(dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim());
                }
                //ViewState["sg1"] = dt;
            }
        }


    }

    void makeColNameAsMine(DataTable dtColNameTable)
    {
        int colFound = dtColNameTable.Columns.Count;
        int colSrno = 1;
        int totCol = 48;
        for (int i = 0; i <= totCol + 2; i++)
        {
            if (colFound > i) dtColNameTable.Columns[i].ColumnName = "sg1_f" + colSrno;
            else dtColNameTable.Columns.Add("sg1_f" + colSrno, typeof(string));
            colSrno++;
        }
    }

    void hideAndRenameCol()
    {
        DataTable dtColNameTab = (DataTable)ViewState["sg1"];
        string MCOLS_2RALIGN = ""; string MCOLS_2RESIZE = ""; string MCOLS_WIDTHS = "";
        int colFound = dtColNameTab.Columns.Count;
        int totResrvCol = 0;
        int totcol = 48;
        int x = 0;
        x = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "FRMWINDOWSIZE"));
        if (x == 0) x = 1000;
        double totWidth = 0;
        int widthMake = 0;
        for (int i = totResrvCol; i <= totcol + 2; i++)
        {
            if (colFound + totResrvCol > i)
            {
                GridView1.HeaderRow.Cells[i].Text = dtColNameTab.Columns[i - totResrvCol].ColumnName;
                if (GridView1.Rows.Count > 0)
                {
                    widthMake = (GridView1.Rows[0].Cells[i].Text.Length) * 10;
                    if (widthMake < 50) widthMake = 50;
                    if (widthMake > 200) widthMake = 200;
                    totWidth += widthMake.ToString().toDouble();

                    GridView1.Columns[i].HeaderStyle.Width = widthMake;
                    GridView1.Columns[i].ItemStyle.HorizontalAlign = HorizontalAlign.Left;

                    if (GridView1.Columns[i].HeaderStyle.CssClass == "hidden")
                    {
                        if (i > 2)
                        {
                            GridView1.Columns[i].HeaderStyle.CssClass = "";
                            GridView1.Rows[0].Cells[i].CssClass = "";
                        }
                    }
                }
            }
            else
            {
                try
                {
                    GridView1.Columns[i].HeaderStyle.CssClass = "hidden";
                    GridView1.Rows[0].Cells[i].CssClass = "hidden";
                }
                catch { }
            }
        }
        //if (totWidth < 1200 && x > 600)
        //{
        //    double gridFoundWidth = Math.Round(x * .93);
        //    string sepWidth = Math.Round((gridFoundWidth / (colFound - 2)) * 1).ToString();
        //    string restWidth = ((Convert.ToInt32(sepWidth)) / (colFound - 3)).ToString();
        //    int multp = 2;

        //    for (int i = 0; i <= colFound; i++)
        //    {
        //        if (i == 0) GridView1.Columns[i].HeaderStyle.Width = (Convert.ToInt16(sepWidth) * multp);
        //        else GridView1.Columns[i].HeaderStyle.Width = (Convert.ToInt16(sepWidth) - Convert.ToInt16(restWidth));
        //        totWidth += Convert.ToDouble(sepWidth);
        //    }
        //}
    }
    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        srchMthd();
    }
    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        formid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (formid == "F70375")
        {
            dt = new DataTable();
            Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            dt = fgen.getdata(frm_qstr, co_cd, Squery);
        }
        else
        {
            dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];
        }

        if (dt.Rows.Count > 0)
        {
            if (formid == "F60101")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["css_no"] = dt.Rows[i]["css_no"].ToString().Replace("\\n", "\n");
                }
            }
            fgen.exp_to_excel(dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim());
            //fgen.ExportGridToExcel(GridView1, co_cd + "_" + DateTime.Now.ToString().Trim() + ".xls");
        }
        else
        {
            fgen.msg("-", "AMSG", "No Data to Export");
        }
        dt.Dispose();
    }
    protected void btnexptopdf_Click(object sender, ImageClickEventArgs e)
    {
        MakeStdRpt();
        //dt = new DataTable();
        //dt = (DataTable)ViewState["sg1"];
        //if (dt.Rows.Count > 0) fgen.exp_to_pdf(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        //else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_word(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnhide_Click(object sender, EventArgs e)
    {
        fill_grid();
    }
    protected void txtsearch_TextChanged(object sender, EventArgs e)
    {
        srchMthd();
    }
    void srchMthd()
    {
        DataTable dt1 = new DataTable();
        Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        hfqry.Value = Squery;
        if (hfqry.Value.Length > 5)
        {
            Squery = hfqry.Value;
        }
        else if (Session["send_dt"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["send_dt"];
        }
        if (txtsearch.Text == "")
        {
            if (Squery.Length > 5)
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
            if (Squery.Length > 5)
            {
                dt1 = fgen.search_vip(frm_qstr, co_cd, Squery, txtsearch.Text.Trim().ToUpper());
            }
            else
            {
                dt1 = fgen.searchDataTable(txtsearch.Text, dt);
            }
        }

        //Squery = hfqry.Value;
        //if (txtsearch.Text == "") dt1 = fgen.getdata(frm_qstr, co_cd, "select * from ( " + Squery + " ) where rownum<=" + tkrow.Text.Trim() + "");
        //else dt1 = fgen.search_vip(frm_qstr, co_cd, Squery, txtsearch.Text.Trim().ToUpper());
        //ViewState["sg1"] = dt1;
        datadiv.Visible = true;
        if (formid == "F40353")
        {
            DataRow oporow = dt1.NewRow();
            double itot_stk = 0; double to_cons = 0; double itv = 0;
            string mq1 = "";
            foreach (DataColumn dc in dt1.Columns)
            {
                itot_stk = 0; to_cons = 0; itv = 0;
                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 7 || dc.Ordinal == 29 || dc.Ordinal == 30 || dc.Ordinal == 15)
                {

                }
                else if (dc.Ordinal == 17)
                {
                    mq1 = "sum([" + dc.ColumnName + "])";
                    itot_stk += fgen.make_double(dt1.Compute(mq1, "").ToString());
                    oporow[dc] = Math.Round(itot_stk, 0);
                }
                else if (dc.Ordinal == 21)
                {
                    mq1 = "sum([Rejection_sheet_wt+Paper_wt_diff_as_per_Job_Card_KG])";
                    to_cons = fgen.make_double(dt1.Compute(mq1, "").ToString());
                    mq1 = "sum([Paper_consumed_Actual_KG])";
                    itv = fgen.make_double(dt1.Compute(mq1, "").ToString());
                    oporow[dc] = Math.Round((to_cons / itv) * 100, 2);
                }
                else if (dc.Ordinal == 28)
                {
                    mq1 = "sum([Total_Rejection_Wt_KG])";
                    to_cons = fgen.make_double(dt1.Compute(mq1, "").ToString());
                    mq1 = "sum([Paper_consumed_Actual_KG])";
                    itv = fgen.make_double(dt1.Compute(mq1, "").ToString());
                    oporow[dc] = Math.Round((to_cons / itv) * 100, 2);
                }
                else
                {
                    mq1 = "sum([" + dc.ColumnName + "])";
                    try
                    {
                        itot_stk += fgen.make_double(dt1.Compute(mq1.ToString(), "").ToString());
                    }
                    catch { }
                    oporow[dc] = itot_stk;
                }
            }
            oporow["PRODUCT"] = "GRAND TOTAL";
            dt1.Rows.InsertAt(oporow, 0);
            ViewState["sg1"] = dt1;
            GridView1.DataSource = dt1;
            GridView1.DataBind();
            tdDiv.Visible = false;
            tdGrid.Visible = true;
        }
        else
        {
            //gridshow = "Y";
            if (gridshow == "Y")
            {
                DataTable neWDt = dt1.Copy();
                if (neWDt.Rows.Count > 0)
                {
                    ViewState["sg1"] = neWDt;
                    dt = neWDt;
                    makeColNameAsMine(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    hideAndRenameCol();
                    tdDiv.Visible = false;
                    tdGrid.Visible = true;
                }
                //GridView1.DataSource = dt1;
                //GridView1.DataBind();
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt1, "").ToString(), false);
        }
        if (dt1.Rows.Count > 0)
        {
            datadiv.Visible = true; div2.Visible = false;
            lblTotcount.Text = "Total Rows : " + dt1.Rows.Count;
        }
        else
        {
            datadiv.Visible = false; div2.Visible = true;
        }
        dt1.Dispose();
    }
    protected void tkrow_TextChanged(object sender, EventArgs e)
    {
        fill_grid();
    }

    private void MakeStdRpt()
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        dt10Col = new DataTable();
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
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        header_n = fgenMV.Fn_Get_Mvar(frm_qstr, "U_HEADER");
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

                header_n = fgen.seek_iname_dt(dtx, "ID='" + formid + "'", "TEXT");
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
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        GridView1.PageIndex = e.NewPageIndex;
        DataTable dt1 = new DataTable();
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        dt1 = dt;
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
        tdDiv.Visible = false;
        tdGrid.Visible = true;
    }
    protected void btnHelp_Click(object sender, ImageClickEventArgs e)
    {
        fgen.Fn_open_helpBox("Help on this Report", frm_qstr);
        fill_grid();
    }
    protected void btnPrint_Click(object sender, ImageClickEventArgs e)
    {
        string pageurl = "../tej-base/dprint.aspx?STR=ERP@" + DateTime.Now.ToString("dd") + "@" + co_cd + "@" + frm_CDT1.Substring(6, 4) + frm_mbr + "@000001@BVAL@" + formid + "@" + formid + "";
        Response.Write("<script>window.open('" + pageurl + "');</script>");
        fill_grid();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (formid == "F70377")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView1.RowStyle.HorizontalAlign = HorizontalAlign.Right;
            }
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
}