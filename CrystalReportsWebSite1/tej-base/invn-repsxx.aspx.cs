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

public partial class invn_repsxx : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N";
    string frm_FileName = "", frm_formID = "";
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
        DataTable dt ;

        DataSet dsRep = new DataSet();
        string barCode = hfval.Value;
        string scode = barCode;
        string sname = "";
        
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
            //CHL
            case "F1007":
                #region CHL
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";

                SQuery = "SELECT D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND D.ID='M' ORDER BY A.ICODE";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_chl", frm_rptName, dsRep, "Challan Report");
                #endregion
                break;
            //CHL2
            case "F1007A":
                #region CHL2
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";

                SQuery = "SELECT B.INAME,B.UNIT AS UNIT2, A.BRANCHCD AS MBR,A.TYPE AS BTYPE,A.VCHNUM AS BVCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BVCHDATE1,A.ICODE AS BICODE,A.ACODE AS BACODE,A.IQTYOUT AS BQTY,A.IQTY_WT AS WT_REC FROM RGPMST A,ITEM B  WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE)";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_chl2", frm_rptName, dsRep, "Challan Report");
                #endregion
                break;
            //M.R.R.
            case "F25101":
            case "F1002":
                #region M.R.R.
                //frm_mbr = scode.Substring(0, 2);
                //frm_vty = scode.Substring(2, 2);
                //sname = scode.Substring(4, 6);
                //sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                dt = new DataTable();
                //SQuery = "SELECT 'Purchase Requisition' AS HEADER, B.INAME AS ITEM_NAME,B.CPARTNO,B.HSCODE,C.INAME AS SUBNAME ,A.* FROM POMAS A,ITEM B ,ITEM C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR((A.ICODE),1,4)=TRIM(C.ICODE)  AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') in ('" + scode + "') ORDER BY A.SRNO ";
                SQuery = "select f.addr1 as caddr1,f.addr2 as caddr2,f.addr3 as caddr3,f.addr4 as caddr4,f.mobile as ctel,f.aname,f.gst_no as cgst_no,f.email as cemail,t.name as mrrtype,i.unit as iunit,i.iname,i.cpartno as icpartno,b.amt_sale as totamt,b.bill_tot as grandtot, b.amt_exc as cgst_val,b.rvalue as sgst_val,B.EXCB_CHG AS TXBL,a.* from ivoucher a,item i,famst f,type t,ivchctrl b  where trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||TRIM(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and trim(a.type)=trim(t.type1) and t.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.vchdate,a.vchnum,A.MORDER";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));

                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_mrr", frm_rptName, dsRep, "M.R.R Report");
                }
                #endregion
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
            //conv_pdf(data_set, rptfile);
        }
        else
        {
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
            conv_pdf(data_set, rptfile);
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