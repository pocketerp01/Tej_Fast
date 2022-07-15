using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

using CrystalDecisions.CrystalReports.Engine;

public partial class rptlevel_img : System.Web.UI.Page
{
    string Squery, co_cd, frm_url, frm_qstr, HCID, frm_mbr, fromdt, todt, header_n, formid, frm_CDT1;
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
        }
    }
    public void fill_grid()
    {
        hfqry.Value = "";
        dt = new DataTable();
        Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");

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

            ViewState["sg1"] = dt;
            //GridView1.DataSource = dt;
            //GridView1.DataBind();            
        }
        else if (Session["send_dt"] != null)
        {
            dt = (DataTable)Session["send_dt"];
            ViewState["sg1"] = dt;
            //GridView1.DataSource = dt;
            //GridView1.DataBind();            
        }
        lblTotcount.Text = "Total Rows : " + dt.Rows.Count;

        DataTable myDt = new DataTable();

        myDt.Columns.Add("field1");
        myDt.Columns.Add("imgsrc");
        DataRow myDr = null;
        StringBuilder col1 = new StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            myDr = myDt.NewRow();
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.ToUpper() != "IMG_SRC")
                {
                    col1.Append(dc.ColumnName + " : " + dr[dc.ColumnName].ToString() + "</br>");
                }
            }
            myDr["field1"] = col1;
            myDr["imgsrc"] = (dr["IMG_SRC"].ToString() != "-" ? dr["IMG_SRC"].ToString() : "~/tej-base/images/nodata.gif");
            myDt.Rows.Add(myDr);
            col1.Clear();
        }

        ListBox1.DataSource = myDt;
        ListBox1.DataBind();

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
        lblTotcount.Text = "Total Rows : " + dt1.Rows.Count;
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
        Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        if (formid == "F40353")
        {
            if (Session["send_dt"] != null)
            {
                dt = (DataTable)Session["send_dt"];
                ViewState["sg1"] = dt;
            }
        }
        else
        {
            dt = fgen.getdata(frm_qstr, co_cd, "select * from ( " + Squery + " ) where rownum<=" + tkrow.Text.Trim() + "");
        }
        //GridView1.DataSource = dt;
        //GridView1.DataBind();        
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