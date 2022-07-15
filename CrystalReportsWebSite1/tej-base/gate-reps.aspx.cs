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

public partial class gate_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, pdfView = "";
    string party_cd, part_cd;
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
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

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                }
                else Response.Redirect("~/login.aspx");

            }
            //if (!Page.IsPostBack)
            {
                printCrpt(hfhcid.Value);
                CrystalReportViewer1.Focus();
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
        switch (iconID)
        {
            //GE
            case "F1001":
                #region GE
                dsRep = new DataSet();
                dt = new DataTable();
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0011", "OPT_ENABLE");
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,A.*,nvl(a.btchno,'-') as batch_no,B.INAME,B.CPARTNO,B.UNIT,C.aname,C.addr1,C.addr2,C.addr3,C.staten,C.email,C.website,C.gst_no FROM IVOUCHERP A,ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.vchdate,a.vchnum,A.MORDER");
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("captured_img", typeof(System.Byte[]));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq0 = dt.Rows[i]["tpt_names"].ToString().Trim(); //image path
                        //for image
                        if (mq0 != "" && mq0 != "-")
                        {
                            fpath = Server.MapPath("~/tej-base/") + mq0;
                            FilStr = new FileStream(fpath, FileMode.Open);
                            BinRed = new BinaryReader(FilStr);
                            dt.Rows[i]["captured_img"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                            FilStr.Close();
                            BinRed.Close();
                        }
                    }
                    dt.TableName = "Prepcur";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                }

                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, "select C.aname,C.addr1,C.addr2,C.addr3,C.staten,C.email,C.website,C.gst_no from famst where trim(acode)='" + dsRep.Tables[0].Rows[0]["acode"].ToString().Trim() + "'");
                    //dt.TableName = "FAMST";
                    //dsRep.Tables.Add(dt);
                    frm_rptName = frm_rptName.Length < 2 ? "std_ge" : frm_rptName;
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_ge", frm_rptName, dsRep, "Gate Entry Report");
                }
                #endregion
                break;
            case "F1001_GO":
                #region GE
                dsRep = new DataSet();
                dt = new DataTable();
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0011", "OPT_ENABLE");
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,A.*,B.INAME,B.CPARTNO,B.UNIT FROM IVOUCHERP A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.vchdate,a.vchnum,A.MORDER");
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                }

                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select aname,addr1,addr2,addr3,staten,email,website,gst_no from famst where trim(acode)='" + dsRep.Tables[0].Rows[0]["acode"].ToString().Trim() + "'");
                    dt.TableName = "FAMST";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_gout", "std_gout", dsRep, "Gate Outward Report");
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

            case "F20132":
                // Gate Inward Register
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");


                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,A.SRNO,TRIM(A.ACODE) AS ACODE,(CASE WHEN A.PONUM='-' THEN '000000' ELSE A.PONUM END) AS PONUM,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.type as grp,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,A.IQTY_CHL,A.NARATION,I.INAME,I.UNIT,I.CPARTNO AS PARTNO,F.ANAME,TRIM(F.ADDR1)||TRIM(F.ADDR2) AS ADDRESS,A.MODE_TPT,A.DESC_ FROM IVOUCHERP A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='00' AND A.VCHDATE " + xprdRange + " and a.acode like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_GE_REG", "std_GE_REG", dsRep, "Gate Inward Register");
                }
                break;

            case "F20133":
                // Gate Outward Register
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,A.SRNO,TRIM(A.ACODE) AS ACODE,(CASE WHEN A.PONUM='-' THEN '000000' ELSE A.PONUM END) AS PONUM,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.type as grp,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,A.IQTY_CHL,A.NARATION,I.INAME,I.CPARTNO AS PARTNO,I.UNIT,F.ANAME,TRIM(F.ADDR1)||TRIM(F.ADDR2) AS ADDRESS,a.mode_tpt,a.desc_ FROM IVOUCHERP A,item I,famst f WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='2G' AND A.VCHDATE " + xprdRange + " ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_GE_OutWard_REG", "std_GE_OutWard_REG", dsRep, "Gate Outward Register");
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