using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;
using System.IO;
using System.Net.Mail;

using MessagingToolkit.QRCode.Codec;
using System.Drawing;

public partial class fin_oth_reps_dPrintOt : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N";
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Guid.NewGuid().ToString().Substring(0, 20);
                    str = Request.QueryString["STR"].Trim().ToString();
                    frm_cocd = str.Split('@')[2].ToString().Trim().ToUpper();
                    frm_myear = str.Split('@')[3].ToString().Trim().ToUpper().Substring(0, 4);
                    frm_mbr = str.Split('@')[3].ToString().Trim().ToUpper().Substring(4, 2);
                    frm_uname = str.Split('@')[4].ToString().Trim().ToUpper();
                    hfhcid.Value = str.Split('@')[6].ToString().Trim();
                    hfval.Value = str.Split('@')[7].ToString().Trim();

                    string constr = ConnInfo.connString(frm_cocd);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "CONN", constr);
                    DataTable dtw = new DataTable();
                    dtw = fgen.getdata(frm_qstr, frm_cocd, "select code,to_char(fmdate,'yyyy')||'-'||to_char(todate,'yyyy') as fstr,to_char(fmdate,'dd/mm/yyyy') as cdt1,to_char(todate,'dd/mm/yyyy') as cdt2,branch from co where trim(code)='" + frm_cocd + frm_myear + "'");
                    if (dtw.Rows.Count > 0)
                    {
                        frm_cDt1 = dtw.Rows[0]["cdt1"].ToString().Trim();
                        frm_cDt2 = dtw.Rows[0]["cdt2"].ToString().Trim();
                        xprdRange = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')";
                    }
                }

                fgenMV.Fn_Delete_Older_Data();
                fgenMV.FN_Delete_Older_Files();

            }
            //else Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
            if (!Page.IsPostBack)
            {
                printCrpt(hfhcid.Value);
            }
        }
        catch
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
        }
    }
    void printCrpt(string iconID)
    {
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;
        DataRow mdr, dr1;
        DataSet dsRep = new DataSet();
        string barCode = hfval.Value;
        string scode = barCode;
        string sname = "";
        string mq10, mq1, mq0;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        switch (iconID)
        {
            //GE
            case "F1001":
                #region GE
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.*,B.INAME,B.CPARTNO,B.UNIT FROM IVOUCHERP A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "' ORDER BY A.MORDER");
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                }
                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select aname,addr1,addr2,addr3,staten,email,website,gst_no from famst where trim(acode)='" + dsRep.Tables[0].Rows[0]["acode"].ToString().Trim() + "'");
                    dt.TableName = "FAMST";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_ge", frm_rptName, dsRep, "Gate Entry Report");
                }
                #endregion
                break;
        }
    }    
    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-oth-reps/" + report.Trim() + ".rpt";
        data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));
        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
        }
        else
        {
        }
        data_set.Dispose();
    }
    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-oth-reps/" + report.Trim() + ".rpt";

        if (addlogo == "Y") data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr, "Y"));
        else data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));

        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
        }
        else
        {
        }
        data_set.Dispose();
    }
    public override void VerifyRenderingInServerForm(Control control)
    { return; }
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
    protected void Page_UnLoad(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch (Exception ex) { fgen.FILL_ERR(ex.Message.ToString().Trim() + "==> dprint ==> At the Time of Page UnLoad."); }
    }
    protected override void OnUnload(EventArgs e)
    {
        try
        {
            base.OnUnload(e);
            this.Unload += new EventHandler(Report_Default_Unload);
        }
        catch { }
    }
    void Report_Default_Unload(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch { }
    }
    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        repDoc.Close();
        repDoc.Dispose();
    }
    public void conv_pdf(DataSet dataSet, string rptFile)
    {
        //if (1 == 2)
        {
            repDoc = GetReportDocument(dataSet, rptFile);
            Stream oStream = repDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            byte[] byteArray = null;
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteArray);

            Response.Flush();
            Response.Close();
            repDoc.Clone();
            repDoc.Dispose();
        }
    }
    public void del_file(string path)
    {
        try
        {
            fpath = Server.MapPath(path);
            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }
}