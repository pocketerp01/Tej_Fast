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

public partial class ppc_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdRange1, xprd1, xprd2, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, branch_Cd, header_n, footer_n, footer_erp;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, cond = " ", party_cd, part_cd, pdfView = "", data_found = "";
    double fullQty = 0;
    double batchQty = 0;
    int srno = 1, z = 0;
    DataRow dr;
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
            No_Data_Found.Visible = false;
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
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";


                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCH_CD");
                    footer_erp = "Generated on Tejaxo ERP Web";

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                }
                else Response.Redirect("~/login.aspx");

            }
            if (!Page.IsPostBack)
            {
                printCrpt(hfhcid.Value);
                if (data_found == "N")
                {
                    No_Data_Found.Visible = true;
                    divReportViewer.Visible = false;
                }
                else
                {
                    divReportViewer.Visible = true;
                    CrystalReportViewer1.RefreshReport();
                    CrystalReportViewer1.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            fgen.FILL_ERR(ex.Message);
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
        string opt = "";
        data_found = "Y";
        switch (iconID)
        {
            //Extusion Sticker
            case "F39101":
            case "F39106":
                #region Extusion Sticker
                SQuery = "Select a.branchcd,trim(a.icode)||trim(a.btchno) as fstr,A.MORDER, a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1 from ivoucher a ,item c where trim(a.icode)=trim(c.icode)  AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 1)
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);
                    frm_rptName = "prod_stk";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "prod_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F35136":
                DataSet ds = new DataSet();
                header_n = "Daily Production Plan";
                //SQuery = "select '" + header_n + "' as header, C.NAME AS SHIFT,b.iname,b.cpartno,nvl(trim(b.packsize),1) as packsize,trim(a.iqtyin)/nvl(trim(b.packsize),1) as no_pack,a.* from prod_sheet a,item b,type c where  trim(a.icode)=trim(b.icode) and trim(a.shftcode)=trim(c.type1) and c.id='D' and TRIM(a.BRANCHCD)='" + frm_mbr + "' and TRIM(a.TYPE)='" + frm_vty + "' and TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + hfval.Value + "' order by a.srno";
                SQuery = "select '" + header_n + "' as header, trim(C.NAME) AS SHIFT,trim(b.iname) as iname,trim(b.cpartno) as cpartno,a.vchnum as vchnum,trim(a.srno) as srno, to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.remarks) as remarks,a.iqtyin,trim(a.ent_by) as ent_by, trim(a.edt_by) as edt_by,  to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt, to_char(a.edt_dt,'dd/mm/yyyy') as edt_dt,case when nvl(b.packsize,0)= 0 then 1 else b.packsize end as packsize,case when nvl(b.packsize,0)= 0 then a.iqtyin else ceil(a.iqtyin/b.packsize) end as no_pack,0 as no_of_btch from prod_sheet a,item b,type c  where  trim(a.icode)=trim(b.icode) and trim(a.shftcode)=trim(c.type1) and c.id='D' and TRIM(a.BRANCHCD)='" + frm_mbr + "' and TRIM(a.TYPE)='" + frm_vty + "' and TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + hfval.Value + "' order by to_number(trim(a.srno))";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq0 = "select distinct trim(icode) as icode,main_issue_no from itemosp where substr(trim(icode),1,2) in ('70','75') order by icode";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["icode"].ToString().Trim().Substring(0, 2) == "70" || dr["icode"].ToString().Trim().Substring(0, 2) == "75")
                        {
                            dr["no_pack"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr["icode"].ToString().Trim() + "'", "main_issue_no"));
                            if (fgen.make_double(dr["no_pack"].ToString().Trim()) > 0)
                            {
                                dr["no_of_btch"] = fgen.make_double(dr["iqtyin"].ToString().Trim()) / fgen.make_double(dr["no_pack"].ToString().Trim());
                            }
                        }
                    }
                    dt.TableName = "Prepcur";
                    ds.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "daily_prod", "daily_prod", ds, "");
                }
                else
                {
                    data_found = "N";
                }
                break;            
        }
    }

    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";
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
            if (pdfView == "Y") conv_pdf(data_set, rptfile);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
        }
        data_set.Dispose();
    }

    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";

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
            if (pdfView == "Y") conv_pdf(data_set, rptfile);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
        }
        data_set.Dispose();
    }

    public override void VerifyRenderingInServerForm(Control control)
    { return; }

    private ReportDocument GetReportDocument(DataSet rptDS, string rptFileName)
    {
        string repFilePath = Server.MapPath("" + rptFileName + "");
        repDoc = new ReportDocument();
        repDoc.Load(repFilePath, OpenReportMethod.OpenReportByDefault);
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
        if (data_found == "N")
        {
            return;
        }
        else
        {
            repDoc.Close();
            repDoc.Dispose();
        }
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

    protected void btnexp_Click(object sender, EventArgs e)
    {
        DataSet ds = (DataSet)Session["data_set"];
        if (ds.Tables[0].Rows.Count > 0)
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            fgen.exp_to_excel(ds.Tables[0], "ms-excel", "xls", frm_FileName);
        }
    }

    protected void btnexptopdf_Click(object sender, EventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnprint1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            conv_pdf(ds, rpt);
        }
        catch (Exception ex) { ex.Message.ToString(); }
    }

}