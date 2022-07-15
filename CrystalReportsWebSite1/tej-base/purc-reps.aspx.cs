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

//using Microsoft.Reporting.WebForms;

public partial class purc_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, header_n, cond = "", pdfView = "";
    string branch_Cd = "", xprd1 = "", firm, xhtml_tag, subj, party_cd, part_cd, cond1, data_found = "N";
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    string pdfdoc = "", pdffirm = "", pdfno = "", pdfdt = "";
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

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                }
                else Response.Redirect("~/login.aspx");
            }
            //if (!Page.IsPostBack)
            {
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "USEND_MAIL") == "Y")
                {
                    tremail.Visible = true;
                    pdfView = "N";
                }
                else tremail.Visible = false;

                printCrpt(hfhcid.Value);

                if (data_found == "N")
                {
                    No_Data_Found.Visible = true;
                    divReportViewer.Visible = false;
                }
                else
                {
                    divReportViewer.Visible = true;
                    //repDoc.SetDatabaseLogon("FIN" + frm_cocd, ConnInfo.nPwd);
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
        string party_cd = "";
        string part_cd = "";
        string mq10, mq1, mq0, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq11, mq12, ded1;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string opt = "", chk_opt = "";
        data_found = "Y";
        string doc_GST = "";
        chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
        //Member GCC Country
        if (chk_opt == "Y")
        {
            doc_GST = "GCC";
        }
        switch (iconID)
        {
            case "F1014"://////11.8.18
                #region PURCH SCH
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0013", "OPT_ENABLE");
                SQuery = "select trim((d.mthname) as mthname,trim(C.ANAME) as aname,trim(C.ADDR1) as ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,TRIM(C.RC_NUM) AS TIN,c.gst_no,TRIM(B.INAME) AS INAME,A.* from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")";
                col1 = "N";
                if (col1 == "Y")
                {
                    //SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,C.EMAIL,C.WEBSITE,C.GST_NO AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY A.ICODE DESC";
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, trim(C.ANAME) as aname,trim(C.ADDR1) as ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,TRIM(C.RC_NUM) AS TIN,c.gst_no,TRIM(B.INAME) AS INAME,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,Round((nvl(DAY1,0)/1000),1) as  DAY1 ,round((nvl(A.DAY2,0)/1000),1) AS DAY2,round((nvl(A.DAY3,0)/10000),1) AS DAY3,round((nvl(A.DAY4,0)/1000),1) AS DAY4,round((nvl(A.DAY5,0)/1000),1) AS DAY5,round((nvl(A.DAY6,0)/1000),1)  AS DAY6,round((nvl(A.DAY7,0)/1000),1) AS DAY7,round((nvl(A.DAY8,0)/1000),1) AS DAY8,round((nvl(A.DAY9,0)/1000),1) AS DAY9,round((nvl(A.DAY10,0)/1000),1) AS DAY10,round((nvl(A.DAY11,0)/1000),1) AS DAY11,round((nvl(A.DAY12,0)/1000),1) AS DAY12,round((nvl(A.DAY13,0)/1000),1) AS DAY13,round((nvl(A.DAY14,0)/1000),1) AS DAY14,round((nvl(A.DAY15,0)/1000),1) AS DAY15,round((nvl(A.DAY16,0)/1000),1) AS DAY16,round((nvl(A.DAY17,0)/1000),1) AS DAY17,round((nvl(A.DAY18,0)/1000),1) AS DAY18,round((nvl(A.DAY19,0)/1000),1) AS DAY19,round((nvl(A.DAY20,0)/1000),1) AS DAY20,round((nvl(A.DAY21,0)/1000),1) AS DAY21,round((nvl(A.DAY22,0)/1000),1) AS DAY22,round((nvl(A.DAY23,0)/1000),1) AS DAY23,round((nvl(A.DAY24,0)/1000),1) AS DAY24,round((nvl(A.DAY25,0)/1000),1) AS DAY25,round((nvl(A.DAY26,0)/1000),1) AS DAY26,round((nvl(A.DAY27,0)/1000),1) AS DAY27,round((nvl(A.DAY28,0)/1000),1) AS DAY28,round((nvl(A.DAY29,0)/1000),1) AS DAY29,round((nvl(A.DAY30,0)/1000),1)  AS DAY30,round((nvl(A.DAY31,0)/1000),1) AS DAY31,round((nvl(A.TOTAL,0)/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,nvl(A.APP_BY,'-') as app_by,A.APP_DT,trim(C.EMAIL) as email,trim(C.WEBSITE) as website,trim(C.GST_NO) AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY A.ICODE DESC";
                }
                if (col1 == "N")
                {
                    //SQuery = "select a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,C.EMAIL,C.WEBSITE,C.GST_NO AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")";
                    SQuery = "select trim(a.branchcd)||trim(a.type)||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'" + col1 + "' as col1,TRIM(d.mthname) AS MTHNAME,to_char(a.vchdate,'YYYY') AS YEAR_,trim(C.ANAME) as aname,trim(C.ADDR1) as ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,TRIM(C.RC_NUM) AS TIN,TRIM(B.INAME) AS INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,nvl(A.DAY1,0) as DAY1,NVL(A.DAY2,0) AS DAY2,NVL(A.DAY3,0) AS DAY3,NVL(A.DAY4,0) AS DAY4,NVL(A.DAY5,0) AS DAY5,NVL(A.DAY6,0) AS DAY6,NVL(A.DAY7,0) AS DAY7,NVL(A.DAY8,0) AS DAY8,NVL(A.DAY9,0) AS DAY9,NVL(A.DAY10,0) AS DAY10,NVL(A.DAY11,0) AS DAY11,NVL(A.DAY12,0)  AS DAY12,NVL(A.DAY13,0) AS DAY13,NVL(A.DAY14,0) AS DAY14,NVL(A.DAY15,0) AS DAY15,NVL(A.DAY16,0) AS DAY16,NVL(A.DAY17,0) AS DAY17,NVL(A.DAY18,0) AS DAY18,NVL(A.DAY19,0) AS DAY19,NVL(A.DAY20,0) AS DAY20,NVL(A.DAY21,0) AS DAY21,NVL(A.DAY22,0) AS DAY22,NVL(A.DAY23,0) AS DAY23,NVL(A.DAY24,0) AS DAY24,NVL(A.DAY25,0) AS DAY25,NVL(A.DAY26,0) AS DAY26,NVL(A.DAY27,0) AS DAY27,NVL(A.DAY28,0) AS DAY28,NVL(A.DAY29,0) AS DAY29,NVL(A.DAY30,0) AS DAY30,NVL(A.DAY31,0) AS DAY31,NVL(A.TOTAL,0) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,TRIM(C.EMAIL) AS EMAIL,TRIM(C.WEBSITE) AS WEBSITE,TRIM(C.GST_NO) AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")";
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    pdfView = "Y";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_po_schedule", "std_po_schedule", dsRep, "PURCHASE SCHD RPT");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //P.O
            case "F1004": ////11.8.18
                #region P.O.
                sname = "";
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0012", "OPT_ENABLE");
                if (frm_vty != "54")
                {
                    //SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,D.ANAME AS CUST,D.ADDR1 AS ADRES1,D.ADDR2 AS ADRES2,D.ADDR3 AS ADRES3,D.GIRNO AS CUSTPAN,D.STAFFCD,D.PERSON AS CPERSON,D.EMAIL AS CMAIL,D.TELNUM AS CONT,D.STATEN AS CSTATE, D.GST_NO AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,B.NAME AS TYPENAME,C.INAME,C.CPARTNO AS  PARTNO,C.PUR_UOM AS CMT,C.NO_PROC AS Sunit,C.UNIT AS CUNIT,C.HSCODE,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.orddt,a.ordno,A.srno ";
                    SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,d.ANAME,TRIM(D.ANAME) AS CUST,D.BUYCODE AS OLDCODE,TRIM(D.ADDR1) AS ADRES1,TRIM(D.ADDR2) AS ADRES2,TRIM(D.ADDR3) AS ADRES3,TRIM(D.GIRNO) AS CUSTPAN,TRIM(D.STAFFCD) AS STAFFCD,TRIM(D.PERSON) AS CPERSON,TRIM(D.EMAIL) AS CMAIL,TRIM(D.TELNUM) AS CONT,TRIM(D.STATEN) AS CSTATE, TRIM(D.GST_NO) AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,TRIM(B.NAME) AS TYPENAME,TRIM(C.INAME) AS INAME,TRIM(C.CPARTNO) AS  PARTNO,TRIM(C.PUR_UOM) AS CMT,TRIM(C.NO_PROC) AS Sunit,TRIM(C.UNIT) AS CUNIT,TRIM(C.HSCODE) AS HSCODE,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE,nvl(d.email,'-') as p_email,A.srno FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.orddt,a.ordno,A.srno ";
                }
                else
                {
                    //SQuery = " select distinct a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'Import Purchase Order' as header,a.currency,a.delv_item,a.amdtno, b.aname,b.addr1,b.addr2,b.addr3,b.addr4,b.email,B.TELNUM,B.MOBILE,c.hscode,c.iname,c.unit as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,a.icode,a.qtyord as qtyord,a.prate,a.pdisc,a.payment as pay_term,a.transporter as shipp_frm,a.desp_to as shipp_to ,a.mode_tpt ,a.delv_term as etd,a.tr_insur as insurance,a.packing,a.remark,a.cscode1,a.cscode, a.pdiscamt, a.qtybal,d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ")";
                    SQuery = " select distinct a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'Import Purchase Order' as header,NVL(a.currency,0) AS currency,trim(a.delv_item) as delv_item,a.amdtno, trim(b.aname) as aname,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,trim(b.email) as email,B.TELNUM,B.MOBILE,trim(c.hscode) as hscode,trim(c.iname) as iname,trim(c.ciname) as ciname,trim(c.unit) as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(a.icode) as icode,nvl(a.qtyord,0) as qtyord,nvl(a.prate,0) as prate,nvl(a.pdisc,0) as pdisc,trim(a.payment) as pay_term,trim(a.transporter) as shipp_frm,trim(a.desp_to) as shipp_to,trim(a.mode_tpt) as mode_tpt,trim(a.delv_term) as etd,trim(a.tr_insur) as insurance,trim(a.packing) as packing,trim(a.remark) as remark,a.cscode1,a.cscode,nvl(a.pdiscamt,0) as pdiscamt,nvl(a.qtybal,0) as qtybal,trim(d.aname) as consign,trim(d.addr1) as daddr1,trim(d.addr2) as daddr2,trim(d.addr3) as daddr3,trim(d.addr4) as daddr4,trim(d.telnum) as dtel, trim(d.rc_num) as dtinno,trim(d.exc_num) as dcstno,trim(d.acode) as mycode,trim(d.staten) as dstaten,trim(d.gst_no) as dgst_no,trim(d.girno) as dpanno,substr(d.gst_no,0,2) as dstatecode,nvl(b.email,'-') as p_email,a.desc_,A.srno,TRIM(C.CPARTNO) AS  PARTNO from  famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.ordno,A.srno";
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    if (frm_cocd == "KRSM")
                    {
                        string appby = fgen.seek_iname_dt(dt, "app_by = '-'", "app_by");
                        if (appby != "-")
                        {
                            dt = fgen.add_apprvlLogo(frm_cocd, dt, branch_Cd);
                        }
                    }
                    if (!dt.Columns.Contains("POPREFIX")) dt.Columns.Add("POPREFIX");
                    SQuery = "SELECT DISTINCT BRANCHCD||TYPE||TRIM(vCHNUM)||TO_CHAR(VCHDATE,'YYYYmmdd') AS FSTR, TERMS||' '||CONDI AS POTERMS_FORM," +
                        "SNO FROM POTERM WHERE BRANCHCD='" + frm_mbr + "' and TYPE ='" + frm_vty + "' and TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') in (" + barCode + ") ORDER BY SNO";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.Columns.Add("POTERMS_FORM", typeof(string));
                    DataView dv = new DataView(dt, "", "", DataViewRowState.CurrentRows);
                    dt6 = dv.ToTable(true, "fstr");
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt6.Rows.Count; i++)
                        {
                            dv = new DataView(dt1, "FSTR='" + dt6.Rows[i]["fstr"].ToString() + "'", "", DataViewRowState.CurrentRows);
                            if (dv.Count > 0)
                            {
                                mq10 = "";
                                for (int x = 0; x < dv.Count; x++)
                                {
                                    mq10 += dv[i].Row["poterms_form"].ToString() + Environment.NewLine;
                                }
                                foreach (DataRow drc in dt.Rows)
                                {
                                    if (drc["fstr"].ToString() == dt6.Rows[i]["fstr"].ToString())
                                        drc["poterms_form"] = mq10;
                                }
                            }
                        }
                    }
                    dt.TableName = "Prepcur";
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dt1.TableName = "POTERM";
                    dsRep.Tables.Add(dt1);

                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='PO' ORDER BY SRNO";
                   
                    
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt1.TableName = "type1";
                    mq10 = "";
                    dt3 = new DataTable();
                    mdr = null;
                    dt3.Columns.Add("poterms", typeof(string));
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        mq10 += dt1.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                    }
                    mdr = dt3.NewRow();
                    if(frm_cocd=="MASS"|| frm_cocd == "MAST") { }
                    else mdr["poterms"] = mq10;
                    dt3.Rows.Add(mdr);
                    dt3.TableName = "type1";
                    dsRep.Tables.Add(dt3);

                    if (frm_rptName.Length <= 1) frm_rptName = "std_po";
                    if (frm_cocd == "HPPI" || frm_cocd == "SPPI" || doc_GST == "GCC") frm_rptName = "std_po_UAE";
                    if (frm_cocd == "KRSM") frm_rptName = "std_po_krs";
                    if (frm_vty == "54")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_Imp_PO", "std_Imp_PO", dsRep, "Import P.O. Entry Report", "Y");
                    }
                    else
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_po", frm_rptName, dsRep, "P.O. Entry Report", "Y");
                    }
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //P.R.
            case "F15101":
            case "F1003":
                #region P.R.
                sname = "";
                dt = new DataTable();
                //SQuery = "SELECT 'Purchase Requisition' AS HEADER, B.INAME AS ITEM_NAME,B.CPARTNO,B.HSCODE,b.unit as iunit,trim(C.NAME)||'->'||trim(a.payment) AS MAINGRP ,A.* FROM POMAS A,ITEM B ,type C  WHERE c.id='Y' and TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(A.ICODE,1,2)=TRIM(C.type1) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") ORDER BY A.SRNO ";
                SQuery = "SELECT 'Purchase Requisition' AS HEADER, TRIM(B.INAME) AS ITEM_NAME,TRIM(B.CPARTNO) AS CPARTNO,TRIM(B.HSCODE) AS HSCODE,TRIM(b.unit) as iunit,trim(C.NAME) as subname,trim(C.NAME)||'->'||trim(a.payment) AS MAINGRP ,A.*,b.irate as item_rate FROM POMAS A,ITEM B ,type C  WHERE c.id='Y' and TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(A.ICODE,1,2)=TRIM(C.type1) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") ORDER BY A.SRNO";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    pdfView = "N";

                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    //if (frm_rptName.Length <= 2) 
                    frm_rptName = "std_pr_landscape";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_pr", frm_rptName, dsRep, "P.R Report");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            // MERGE BY MADHVI, CREATED BY YOGITA
            case "F15132":////////10.8.18
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                header_n = "P.R. Register";
                //pkgg 11/3/2018
                //SQuery = "select a.icode, C.Iname as Item,c.cpartno, A.ordno as Pr_No,to_chaR(a.orddt,'dd/mm/yyyy') as Pr_Date,a.Bank  as Department,A.qtyord as Pr_Qty,C.unit,A.desc_ as Remarks,A.delv_item as Earliest_By,A.Ent_By,'" + fromdt + "' as frmdate,'" + todt + "' as todate,to_char(a.del_date,'dd/mm/yyyy') as del_date,decode(a.pflag,0,'CLOSED','OPEN') as PStatus from pomas a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='60' and a.orddt " + xprdRange + " and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%'  ORDER BY A.SRNO";
                SQuery = "select trim(a.icode) as icode,trim(C.Iname) as Item,trim(c.cpartno) as cpartno, A.ordno as Pr_No,to_chaR(a.orddt,'dd/mm/yyyy') as Pr_Date,a.Bank  as Department,nvl(A.qtyord,0) as Pr_Qty,C.unit,trim(A.desc_) as Remarks,A.delv_item as Earliest_By,A.Ent_By,'01/04/2017' as frmdate,'10/08/2017' as todate,to_char(a.del_date,'dd/mm/yyyy') as del_date,decode(a.pflag,0,'CLOSED','OPEN') as PStatus from pomas a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='60' and a.orddt " + xprdRange + " and trim(a.acode) like '" + party_cd + "%' and substr(trim(a.icode),1,2) like '" + part_cd + "%'  ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PR_REG", "std_PR_REG", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                //else
                //{
                //   string errmsg = "No data";
                //    this.Page.clientscript(this.GetType(),"ex","alert('"+ errmsg + "');", true);
                //}
                #endregion
                break;

            case "F15133"://////10.8.18
                #region
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                header_n = "P.O. Register";
                if (frm_vty.Contains("%"))
                {
                    //SQuery = "SELECT DISTINCT  '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as pono, C.INAME,C.UNIT AS CUNIT,C.IWEIGHT AS WT,B.ANAME,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES,TRIM(B.ADDR1) AS BADRES1,TRIM(B.ADDR2) AS BASRED2,C.CPARTNO AS PARTNO,B.ADDR3 AS ADRES3,TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD') AS GRP,A.branchcd,a.type,a.tax,a.ordno,a.orddt,a.acode,a.unit,(case when type='54' then (a.prate* a.wk3) else a.prate end ) as prate,a.pdisc,a.pexc,a.ptax,a.pamt,a.refdate,a.desc_,a.qtyord,a.pordno, a.porddt,a.rate_cd,a.srno,a.pcess,'Rs.' as inr,(case when type='54' then (a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0))*A.WK3 else (a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0)) end) as netrate,(case when type='54' then (a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0))*A.WK3* a.qtyord else ((a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0))*a.qtyord) end) as itmamt,a.pdiscamt, a.icode, decode(nvl(pflag,0),1,'Closed','Current') as POStatus FROM POMAS A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE like '5%' AND A.ORDDT " + xprdRange + " and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' ORDER BY a.orddt,a.type,a.ordno,a.srno";
                    SQuery = "SELECT DISTINCT  '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as pono,trim(C.INAME) as iname,C.UNIT AS CUNIT,nvl(C.IWEIGHT,0) AS WT,trim(B.ANAME) as aname,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES,TRIM(B.ADDR1) AS BADRES1,TRIM(B.ADDR2) AS BASRED2,trim(C.CPARTNO) AS PARTNO,B.ADDR3 AS ADRES3,TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD') AS GRP,A.branchcd,a.type,a.tax,a.ordno,a.orddt,trim(a.acode) as acode,a.unit,(case when type='54' then (nvl(a.prate,0)* nvl(a.wk3,0)) else nvl(a.prate,0) end ) as prate,nvl(a.pdisc,0) as pdisc,nvl(a.pexc,0) as pexc,nvl(a.ptax,0) as ptax,nvl(a.pamt,0) as pamt,a.refdate,a.desc_,nvl(a.qtyord,0) as qtyord,a.pordno, a.porddt,nvl(a.rate_cd,0) as rate_cd,a.srno,nvl(a.pcess,0) as pcess,t.br_curren as inr,(case when type='54' then (nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0))*nvl(A.WK3,0) else (nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0)) end) as netrate,(case when type='54' then (nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0))*nvl(A.WK3,0)* nvl(a.qtyord,0) else ((nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0))*nvl(a.qtyord,0)) end) as itmamt,nvl(a.pdiscamt,0) as pdiscamt,trim(a.icode) as icode, decode(nvl(pflag,0),1,'Closed','Current') as POStatus FROM POMAS A,FAMST B,ITEM C,TYPE T WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.BRANCHCD)=TRIM(T.TYPE1) AND T.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE like '5%' AND A.ORDDT " + xprdRange + " and a.acode like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' ORDER BY a.orddt,a.type,a.ordno,a.srno";
                }
                else
                {
                    //SQuery = "SELECT DISTINCT  '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as pono, C.INAME,C.UNIT AS CUNIT,C.IWEIGHT AS WT,B.ANAME,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES,TRIM(B.ADDR1) AS BADRES1,TRIM(B.ADDR2) AS BASRED2,C.CPARTNO AS PARTNO,B.ADDR3 AS ADRES3,TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD') AS GRP,a.type,a.tax,a.ordno,a.orddt,a.acode,a.unit,(case when type='54' then (a.prate* a.wk3) else a.prate end ) as prate,a.pdisc,a.pexc,a.ptax,a.pamt,a.refdate,a.desc_,a.qtyord,a.pordno, a.porddt,a.rate_cd,a.srno,a.pcess,'Rs.' as inr,(case when type='54' then (a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0))*A.WK3 else (a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0)) end) as netrate,(case when type='54' then (a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0))*A.WK3* a.qtyord else ((a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0))*a.qtyord) end) as itmamt, a.pdiscamt, a.icode, decode(nvl(pflag,0),1,'Closed','Current') as POStatus FROM POMAS A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + " and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' ORDER BY a.orddt,a.type,a.ordno,a.srno";
                    SQuery = "SELECT DISTINCT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as pono,trim(C.INAME) as iname,C.UNIT AS CUNIT,nvl(C.IWEIGHT,0) AS WT,trim(B.ANAME) as aname,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES,TRIM(B.ADDR1) AS BADRES1,TRIM(B.ADDR2) AS BASRED2,trim(C.CPARTNO) AS PARTNO,B.ADDR3 AS ADRES3,TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD') AS GRP,A.branchcd,a.type,a.tax,a.ordno,a.orddt,trim(a.acode) as acode,a.unit,(case when type='54' then (nvl(a.prate,0)* nvl(a.wk3,0)) else nvl(a.prate,0) end ) as prate,nvl(a.pdisc,0) as pdisc,nvl(a.pexc,0) as pexc,nvl(a.ptax,0) as ptax,nvl(a.pamt,0) as pamt,a.refdate,a.desc_,nvl(a.qtyord,0) as qtyord,a.pordno, a.porddt,nvl(a.rate_cd,0) as rate_cd,a.srno,nvl(a.pcess,0) as pcess,t.br_curren as inr,(case when type='54' then (nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0))*nvl(A.WK3,0) else (nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0)) end) as netrate,(case when type='54' then (nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0))*nvl(A.WK3,0)* nvl(a.qtyord,0) else ((nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0))*nvl(a.qtyord,0)) end) as itmamt,nvl(a.pdiscamt,0) as pdiscamt,trim(a.icode) as icode, decode(nvl(pflag,0),1,'Closed','Current') as POStatus FROM POMAS A,FAMST B,ITEM C,TYPE T WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.BRANCHCD)=TRIM(T.TYPE1) AND T.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + " and a.acode like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' ORDER BY a.orddt,a.type,a.ordno,a.srno";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PO_REG", "std_PO_REG", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F15251": ////////11.8.18
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                header_n = "Import P.O. Register in FC";
                //SQuery = "SELECT DISTINCT  '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as pono, C.INAME,C.UNIT AS CUNIT,C.IWEIGHT AS WT,B.ANAME,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES,TRIM(B.ADDR1) AS BADRES1,TRIM(B.ADDR2) AS BASRED2,C.CPARTNO AS PARTNO,B.ADDR3 AS ADRES3,TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD') AS GRP,a.type,a.tax,a.ordno,a.orddt,a.acode,a.unit, a.prate ,a.pdisc,a.pexc,a.ptax,a.pamt,a.refdate,a.desc_,a.qtyord,a.pordno, a.porddt,a.rate_cd ,a.srno,a.pcess,a.currency,(a.prate*((100-a.pdisc)/100)-nvl(a.pdiscamt,0)) as netrate, decode(nvl(pflag,0),1,'Closed','Current') as POStatus FROM POMAS A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE = '54' AND A.ORDDT " + xprdRange + " and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' ORDER BY a.orddt,a.type,a.ordno,a.srno";
                SQuery = "SELECT DISTINCT  '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as pono,trim(C.INAME) AS INAME,A.ICODE,A.PDISCAMT,C.UNIT AS CUNIT,nvl(C.IWEIGHT,0) AS WT,trim(B.ANAME) as aname,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES,TRIM(B.ADDR1) AS BADRES1,TRIM(B.ADDR2) AS BASRED2,trim(C.CPARTNO) AS PARTNO,trim(B.ADDR3) AS ADRES3,TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD') AS GRP,a.type,a.tax,a.ordno,a.orddt,trim(a.acode) as acode,a.unit, nvl(a.prate,0) as prate ,nvl(a.pdisc,0) as pdisc,nvl(a.pexc,0) as pexc,nvl(a.ptax,0) as ptax,nvl(a.pamt,0) as pamt,a.refdate,a.desc_,nvl(a.qtyord,0) as qtyord,a.pordno,a.porddt,nvl(a.rate_cd,0) as rate_cd ,a.srno,nvl(a.pcess,0) as pcess,nvl(a.currency,0) as currency,(nvl(a.prate,0)*((100-nvl(a.pdisc,0))/100)-nvl(a.pdiscamt,0)) as netrate, decode(nvl(pflag,0),1,'Closed','Current') as POStatus,t.br_curren as inr FROM POMAS A,FAMST B,ITEM C,TYPE T WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.BRANCHCD)=TRIM(T.TYPE1) AND T.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE = '54' AND A.ORDDT " + xprdRange + " and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' ORDER BY a.orddt,a.type,a.ordno,a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PO_REG_fc", "std_PO_REG_fc", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F15134":////////11.8.18
                #region P.O. Schedule Repor
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                //    header_n = "P.O. Schedule Report";
                //    col1 = "NO"; // AS PER DISCUSSION WITH PUNEET SIR ON 19TH JAN 2018 , THERE IS NO NEED OF ASKING "DO YOU WANT TO SEE FIGURE IN THOUSANDS". SO VALUE OF COL1 IS HARD CODED.
                //    if (col1 == "YES")
                //    {
                //        SQuery = "select d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,B.UNIT,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((A.DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and to_char(vchdate,'mm')=trim(d.mthnum) AND AND trim(a.branchcd)='" + frm_mbr + "' and type='66' and a.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' ORDER BY a.vchnum, A.SRNO";
                //    }
                //    else if (col1 == "NO")
                //    {
                //        SQuery = "select d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,B.UNIT,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,A.DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,C.EMAIL,C.WEBSITE,C.GST_NO AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) and to_char(vchdate,'mm')=trim(d.mthnum) AND trim(a.branchcd)='" + frm_mbr + "' and type='66' and a.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' ORDER BY a.vchnum, A.SRNO";
                //    }
                //    dt = new DataTable();
                //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //    if (dt.Rows.Count > 0)
                //    {
                //        dt.TableName = "Prepcur";
                //        dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                //        Print_Report_BYDS(frm_cocd, frm_mbr, "std_po_schedule", "std_po_schedule", dsRep, header_n);
                //    }
                //    break;

                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (party_cd.Length > 4)
                    cond = " and a.acode IN (" + party_cd + ")";
                else cond = " and a.acode like '%'";

                cond1 = "";
                if (frm_cocd == "BUPL")
                {
                    cond1 = "||'\n'||'HS : '||trim(b.hscode)";
                    cond += " AND trim(NVL(A.APP_BY,'-'))!='-'";
                }

                header_n = "P.O. Schedule Report";
                // col1 = "NO"; // AS PER DISCUSSION WITH PUNEET SIR ON 19TH JAN 2018 , THERE IS NO NEED OF ASKING "DO YOU WANT TO SEE FIGURE IN THOUSANDS". SO VALUE OF COL1 IS HARD CODED.
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL11");
                if (col1 == "Y")
                {
                    //SQuery = "select '" + col1 + "' AS COL1,d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,B.UNIT,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((A.DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,C.EMAIL,C.WEBSITE,C.GST_NO AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and to_char(vchdate,'mm')=trim(d.mthnum) AND trim(a.branchcd)='" + frm_mbr + "' and type='66' and a.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY a.vchnum, A.SRNO";
                    SQuery = "select '" + col1 + "' AS COL1,trim(d.mthname) as mthname,to_char(a.vchdate,'YYYY') AS YEAR_,trim(C.ANAME) as aname,trim(C.ADDR1) AS ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,C.RC_NUM AS TIN, trim(B.INAME)" + cond1 + " as iname,B.UNIT,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,trim(A.ACODE) as acode,trim(A.ICODE) as icode,Round((A.DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,trim(C.EMAIL) AS EMAIL,TRIM(C.WEBSITE) AS WEBSITE,TRIM(C.GST_NO) AS PARTY_GST,trim(C.EMAIL) AS p_EMAIL from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and to_char(vchdate,'mm')=trim(d.mthnum) AND trim(a.branchcd)='" + frm_mbr + "' and type='66' and a.vchdate " + xprdRange + " " + cond + " and a.icode like '" + part_cd + "%' ORDER BY a.vchnum, A.SRNO";
                }
                else if (col1 == "N")
                {
                    // SQuery = "select '" + col1 + "' AS COL1,d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,B.UNIT,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,A.DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,C.EMAIL,C.WEBSITE,C.GST_NO AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) and to_char(vchdate,'mm')=trim(d.mthnum) AND trim(a.branchcd)='" + frm_mbr + "' and type='66' and a.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY a.vchnum, A.SRNO";
                    SQuery = "select '" + col1 + "' AS COL1,TRIM(d.mthname)  as mthname,to_char(a.vchdate,'YYYY') AS YEAR_,trim(C.ANAME) as aname,trim(C.ADDR1) AS ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,C.RC_NUM AS TIN,TRIM(B.INAME)" + cond1 + " AS INAME,B.UNIT,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,A.DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,TRIM(A.REMARKS) AS REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,TRIM(C.EMAIL) AS EMAIL,TRIM(C.WEBSITE) AS WEBSITE,C.GST_NO AS PARTY_GST,trim(C.EMAIL) AS p_EMAIL from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) and to_char(vchdate,'mm')=trim(d.mthnum) AND trim(a.branchcd)='" + frm_mbr + "' and type='66' and a.vchdate " + xprdRange + " " + cond + " and a.icode like '" + part_cd + "%' ORDER BY a.vchnum, A.SRNO";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_po_schedule", "std_po_schedule", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F15135":////////11.8.18
                header_n = "Approved Price Register";
                // SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as acode,trim(b.aname) as aname,trim(a.icode) as icode,trim(c.iname) as iname,trim(c.cpartno) as cpartno,c.unit,nvl(a.irate,0) as irate,nvl(a.disc,0) as disc,nvl(a.pexc,0) as cgst,nvl(a.pcess,0) as sgst,nvl(a.ptax,0) as igst,nvl(a.jwrate,0) as jwrate,a.pfchg as pf_chrg,a.row_text as des,a.remark,a.srno from appvendvch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='10' and a.vchdate " + xprdRange + " order by a.vchnum, a.srno";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as acode,trim(b.aname) as aname,trim(a.icode) as icode,trim(c.iname) as iname,trim(c.cpartno) as cpartno,c.unit,nvl(a.irate,0) as irate,nvl(a.disc,0) as disc,nvl(a.pexc,0) as cgst,nvl(a.pcess,0) as sgst,nvl(a.ptax,0) as igst,nvl(a.jwrate,0) as jwrate,a.pfchg as pf_chrg,a.row_text as des,a.remark,a.srno from appvendvch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='10' and a.vchdate " + xprdRange + " AND A.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.vchnum, a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_App_Price_Reg", "std_App_Price_Reg", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15136": //13.8.18
                header_n = "Closed P.R. Registers";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE, a.branchcd,a.a_acode,b.name, a.pr_no,a.pr_dt,a.icode,c.iname,C.CPARTNO,a.prqty, a.poqty,A.TERM  from (select fstr, branchcd,max(type),max(acode) as a_acode,pr_no,pr_dt,icode, sum(pr_qty) as PRQty , sum(po_qty) as POQty,max (pflag) as flag,TERM from (select (trim(ordno)||'-'||to_Char( orddt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr, branchcd,type,acode,ordno as pr_no,orddt as pr_dt,icode, qtyord as pr_qty, 0 as po_qty,pflag,TERM from pomas where type='60' and branchcd!= 'DD' and branchcd='" + frm_mbr + "' and orddt " + xprdRange + " union all select (trim(pr_no)||'-'||to_Char( pr_dt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr, branchcd,null as type,null as acode,pr_no as pr_no,pr_dt as pr_dt,icode, 0 as pr_qty, qtyord as po_qty,pflag,TERM from pomas where type like '5%' and branchcd not in ('DD','AM') and branchcd='" + frm_mbr + "' and pr_dt " + xprdRange + ") group by fstr, branchcd,pr_no,pr_dt,icode,TERM )a, type b, item c where a.flag=0 and trim(a.a_acode)=trim(b.type1) and b.id='M' and trim(a.icode)=trim(c.icode) order by a.pr_no";
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE, a.branchcd,a.a_acode,trim(b.name) as name, a.pr_no,a.pr_dt,a.icode,trim(c.iname) as iname,trim(C.CPARTNO) as cpartno,a.prqty, a.poqty,A.TERM  from  (select fstr, branchcd,max(type),max(acode) as a_acode,pr_no,pr_dt,icode, sum(pr_qty) as PRQty , sum(po_qty) as POQty,max (pflag) as flag,TERM from (select (trim(ordno)||'-'||to_Char( orddt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr,trim(branchcd) as branchcd,type,trim(acode) as acode,ordno as pr_no,orddt as pr_dt,trim(icode) as icode,nvl(qtyord,0) as pr_qty, 0 as po_qty,pflag,TERM from pomas where type='60' and branchcd!= 'DD' and branchcd='" + frm_mbr + "' and orddt " + xprdRange + " and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%' union all select (trim(pr_no)||'-'||to_Char( pr_dt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr, branchcd,null as type,null as acode,pr_no as pr_no,pr_dt as pr_dt,trim(icode) as icode, 0 as pr_qty, nvl(qtyord,0) as po_qty,pflag,TERM from pomas where type like '5%' and branchcd not in ('DD','AM') and branchcd='" + frm_mbr + "' and pr_dt " + xprdRange + " and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%') group by fstr, branchcd,pr_no,pr_dt,icode,TERM )a, type b, item c where a.flag=0 and trim(a.a_acode)=trim(b.type1) and b.id='M' and trim(a.icode)=trim(c.icode) order by a.pr_no";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Closed_PR", "std_Closed_PR", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15137": ///////13.8.18
                #region Import Purchase Order
                header_n = "Import Purchase Order";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                // SQuery = "select distinct '" + header_n + "' as header,a.currency,a.delv_item,a.amdtno, b.aname,b.addr1,b.addr2,b.addr3,b.addr4,b.email,B.TELNUM,B.MOBILE,c.hscode,c.iname,c.unit as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,a.icode,a.qtyord as qtyord,a.prate,a.pdisc,a.payment as pay_term,a.transporter as shipp_frm,a.desp_to as shipp_to ,a.mode_tpt ,a.delv_term as etd,a.tr_insur as insurance,a.packing,a.remark,a.cscode1,a.cscode, a.pdiscamt, a.qtybal from pomas a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + mq1 + ")";
                //SQuery = " select distinct '" + header_n + "' as header,a.currency,a.delv_item,a.amdtno, b.aname,b.addr1,b.addr2,b.addr3,b.addr4,b.email,B.TELNUM,B.MOBILE,c.hscode,c.iname,c.unit as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,A.PBASIS,A.DOC_THR,a.acode,a.icode,a.qtyord as qtyord,a.prate,a.pdisc,a.payment as pay_term,a.transporter as shipp_frm,a.desp_to as shipp_to ,a.mode_tpt ,a.delv_term as etd,a.tr_insur as insurance,a.packing,a.remark,a.cscode1,a.cscode, a.pdiscamt, a.qtybal,d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + mq1 + ") ORDER BY ICODE";
                SQuery = " select distinct '" + header_n + "' as header,trim(a.currency) as currency,trim(a.delv_item) as delv_item,a.amdtno,trim(b.aname) as aname,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,trim(b.email) as email,B.TELNUM,B.MOBILE,trim(c.hscode) as hscode,trim(c.iname) as iname,c.unit as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(A.PBASIS) as PBASIS,trim(A.DOC_THR) AS DOC_THR,trim(a.acode) as acode,trim(a.icode) as icode,nvl(a.qtyord,0) as qtyord,nvl(a.prate,0) as prate,nvl(a.pdisc,0) as pdisc,trim(a.payment) as pay_term,trim(a.transporter) as shipp_frm,trim(a.desp_to) as shipp_to ,trim(a.mode_tpt) as mode_tpt,trim(a.delv_term) as etd,trim(a.tr_insur) as insurance,a.packing,a.remark,a.cscode1,a.cscode, nvl(a.pdiscamt,0) as pdiscamt,nvl(a.qtybal,0) as qtybal,trim(d.aname) as consign,trim(d.addr1) as daddr1,trim(d.addr2) as daddr2,trim(d.addr3) as daddr3,trim(d.addr4) as daddr4,trim(d.telnum) as dtel,trim(d.rc_num) as dtinno,trim(d.exc_num) as dcstno,trim(d.acode) as mycode,trim(d.staten) as dstaten,trim(d.gst_no) as dgst_no,trim(d.girno) as dpanno,substr(d.gst_no,0,2) as dstatecode from famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + mq1 + ") ORDER BY ICODE";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    //-----------------------------------------------csmst      
                    //SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dsRep.Tables[0].Rows[0]["cscode1"].ToString().Trim() + "'";
                    //dt = new DataTable();
                    // dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count <= 0)
                    //{
                    //    dt = new DataTable();
                    //    SQuery = "SELECT ANAME AS consign ,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 daddr4,'-' as dtel,'-' as dtinno,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM FAMST WHERE ACODE='" + dsRep.Tables[0].Rows[0]["acode"].ToString().Trim() + "'";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //}
                    //dt.TableName = "csmst";
                    //dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Imp_PO", "std_Imp_PO", dsRep, header_n, "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F15140": ///////13.8.18
                #region Pending PR register
                header_n = "Pending Purchase Requisition Register";
                // //SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,SUBSTR(TRIM(A.FSTR),10,6) AS ORDNO,TO_DATE(SUBSTR(TRIM(A.FSTR),0,8),'YYYY/MM/DD') AS ORDDT, I.INAME,I.CPARTNO,I.UNIT,A.ERP_CODE AS ICODE,A.DEPTT AS DEPARTMENT,SUM(A.QTYORD) AS QTYORD,SUM(SOLDQTY) AS SOLDQTY,SUM(QTYORD)-SUM(SOLDQTY) AS BAL  FROM WBVU_PR_4PO A,ITEM  I WHERE TRIM(A.ERP_CODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' GROUP BY I.INAME,I.CPARTNO,I.UNIT,A.BRANCHCD,A.ERP_CODE,A.DEPTT,SUBSTR(TRIM(A.FSTR),10,6),TO_DATE(SUBSTR(TRIM(A.FSTR),0,8),'YYYY/MM/DD')";
                // SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt, a.ordno,to_char(a.orddt,'dd-Mon-yy') as orddt,a.icode,a.iname,a.req_qty, a.ord_qty,a.deptt as deptt,a.bal_qty as qtyleft,a.cpartno as part, a.unit,A.DESC_ AS RMK from wbvu_pending_pr a where a.branchcd='" + frm_mbr + "' and a.orddt " + xprdRange + " order by a.ordno , a.orddt ";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,a.ordno,to_char(a.orddt,'dd-Mon-yy') as orddt,trim(a.icode) as icode,trim(a.iname) as iname,a.req_qty,nvl(a.ord_qty,0) as ord_qty,trim(a.deptt) as deptt,nvl(a.bal_qty,0) as qtyleft,trim(a.cpartno) as part, a.unit,a.desc_ AS RMK from wbvu_pending_pr a where a.branchcd='" + frm_mbr + "' and a.orddt " + xprdRange + " AND substr(trim(a.icode),1,2) like '" + party_cd + "%' and substr(trim(a.icode),1,4) like '" + part_cd + "%' order by a.ordno , a.orddt";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PR_Pending_REG", "std_PR_Pending_REG", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F15141":
                header_n = "P.R. Vs P.O. Report";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select distinct '" + fromdt + "' as fromdt,'" + todt + "' as todt, a.party,a.pono,trim(b.iname) as iname,trim(B.CPARTNO) as cpartno,b.unit,a.fstr,a.ordno,a.vd as prdt,a.erp_Code,a.qtyord,a.prate,a.soldqty ,a.bank, a.delv_item,a.desc_,a.orddt as podt,a.acode,a.ent_by,a.ent_dt, a.srno from (select to_ChaR(a.orddt,'YYYYMMDD')||'-'||a.ordno||'-'||trim(a.Icode) as fstr,trim(a.ordno) as ordno,to_char(a.orddt,'dd/MM/yyyy') as vd,null as orddt,trim(a.Icode) as ERP_code,nvl(a.Qtyord,0) as qtyord,nvl(a.prate,0) as prate,0 as Soldqty,nvl(a.bank,'-') As bank,nvl(a.delv_item,'-') As delv_item,nvl(a.desc_,'-') as desc_ ,trim(a.acode) as acode,b.name as party,null as pono,a.ent_by,to_char(a.ent_dt,'dd/MM/yyyy') as ent_dt,'1' as srno from pomas a,type b where trim(a.acode)=trim(b.type1) and a.branchcd='" + frm_mbr + "' and a.type='60' and b.id='M' and trim(a.pflag)!=0 and trim(a.app_by)!='-' and a.orddt>=to_Date('01/04/2017','dd/mm/yyyy') and substr(trim(a.icode),1,2) like '" + party_cd + "%' and substr(trim(a.icode),1,4) like '" + part_cd + "%' union all SELECT to_ChaR(a.pr_Dt,'YYYYMMDD')||'-'||a.pr_no||'-'||trim(a.Icode) as fstr,trim(a.pr_no) as ordno,to_char(a.pr_dt,'dd/MM/yyyy') as vd,to_ChaR(a.orddt,'dd/mm/yyyy') as orddt,trim(a.Icode) as ERP_code,0 as Qtyord,0 as prate,nvl(qtyord,0) as soldqty,null as bank,null as delv_item,null as desc_,trim(a.acode) as acode,trim(b.aname) as party,a.ordno as pono,null as en,null as ent_by,'2' as srno from pomas a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt>=to_Date('01/04/2017','dd/mm/yyyy') AND LENGTH(TRIM(a.PR_NO))=6 AND TRIM(a.PR_NO)!='000000' and substr(trim(a.icode),1,2) like '" + party_cd + "%' and substr(trim(a.icode),1,4) like '" + part_cd + "%') a,item b where  trim(a.erp_code)=trim(b.icode) and to_date(a.vd,'dd/mm/yyyy') " + xprdRange + " order by a.ordno, a.fstr, a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PR_VS_PO", "std_PR_VS_PO", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15142":///////13.8.18
                #region Pending Purchase Order Register With Line No
                header_n = "Pending Purchase Order Register With Line No.";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and A.acode like '" + party_cd + "%' and A.icode like '" + part_cd + "%' ";
                // mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                if (frm_vty.Contains("%"))
                {
                    //SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,f.aname,i.iname,i.cpartno,i.unit,A.*, (a.prate* a.qtyord) as netval from WBVU_pendING_PO A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE like '5%' and A.ORDDT  " + xprdRange + " ORDER BY a.ordno";
                    SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,trim(f.aname) as aname,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,A.*, (nvl(a.prate,0)* nvl(a.qtyord,0)) as netval from WBVU_pendING_PO A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE like '5%' and A.ORDDT  " + xprdRange + "  " + cond + "  ORDER BY a.ordno";
                }
                else
                {
                    //SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,f.aname,i.iname,i.cpartno,i.unit,A.*, (a.prate* a.qtyord) as netval from WBVU_pendING_PO A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' and A.TYPE in (" + frm_vty + ") AND A.ORDDT  " + xprdRange + " ORDER BY a.ordno";
                    SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,trim(f.aname) as aname,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,A.*, (nvl(a.prate,0)* nvl(a.qtyord,0)) as netval from WBVU_pendING_PO A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' and A.TYPE in (" + frm_vty + ") AND A.ORDDT  " + xprdRange + " " + cond + " ORDER BY a.ordno";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pending_PO", "std_Pending_PO", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F15143": ///////13.8.18L:L
                header_n = "P.O. Vs MRR Report";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and A.acode like '" + party_cd + "%' and A.erp_code like '" + part_cd + "%' ";
                // ADD TYPE!='04' ON 06 JUNE 2018
                if (frm_vty.Contains("%"))
                {
                    //SQuery = "select distinct '" + fromdt + "' as fromdt,'" + todt + "' as todt,a.fstr,a.erp_Code,a.ordno,A.VD as orddt,a.mrr,a.mrrdt,a.qtyord,a.soldqty,a.prate,a.acode,a.party,b.iname,B.CPARTNO,a.counter  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,ordno,to_char(orddt,'dd/MM/yyyy') as vd,null as mrr,null as mrrdt,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,null as party,acode,'1' as counter from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '%' union all SELECT trim(a.icode)||'-'||to_ChaR(a.podate,'YYYYMMDD')||'-'||a.ponum||'-'||trim(a.ordlineno) as fstr,trim(a.Icode) as ERP_code,trim(a.ponum) as ordno,to_char(a.podate,'dd/MM/yyyy') as vd,trim(a.vchnum) as mrr,to_char(a.vchdate,'dd/mm/yyyy') as mrrdt,0 as Qtyord,a.iqty_chl as qtyord,0 as irate, b.aname as party,a.acode,'2' as counter from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and type!='04' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(a.Acode) like '%' and length(trim(a.ponum))=6 and trim(a.ponum)!='000000') a,item b where trim(a.erp_code)=trim(b.icode) and to_date(a.vd,'dd/mm/yyyy') " + xprdRange + " ORDER BY A.ACODE,A.ERP_code,A.ORDNO, a.counter";
                    SQuery = "select distinct '" + fromdt + "' as fromdt,'" + todt + "' as todt,trim(a.ordno)||trim(a.type) as grp,a.fstr,a.erp_Code,a.ordno,A.VD as orddt,a.mrr,a.mrrdt,a.qtyord,a.soldqty,a.prate,a.acode,a.party,trim(b.iname) as iname,trim(B.CPARTNO) as cpartno,b.unit,a.counter,A.TYPE from (sELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,ordno,to_char(orddt,'dd/MM/yyyy') as vd,null as mrr,null as mrrdt,nvl(Qtyord,0) as qtyord,0 as Soldqty,((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0)) as prate,null as party,trim(acode) as acode,'1' as counter,TYPE from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT trim(a.icode)||'-'||to_ChaR(a.podate,'YYYYMMDD')||'-'||a.ponum||'-'||trim(a.ordlineno) as fstr,trim(a.Icode) as ERP_code,trim(a.ponum) as ordno,to_char(a.podate,'dd/MM/yyyy') as vd,trim(a.vchnum) as mrr,to_char(a.vchdate,'dd/mm/yyyy') as mrrdt,0 as Qtyord,nvl(a.iqty_chl,0) as qtyord,0 as irate, trim(b.aname) as party,trim(a.acode) as acode,'2' as counter,A.TYPE from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and type!='04' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(a.Acode) like '%' and length(trim(a.ponum))=6 and trim(a.ponum)!='000000') a,item b where trim(a.erp_code)=trim(b.icode) and to_date(a.vd,'dd/mm/yyyy') " + xprdRange + " " + cond + " ORDER BY A.ACODE,A.ERP_code,A.ORDNO, a.counter,a.type";
                }
                else
                {
                    //SQuery = "select distinct '" + fromdt + "' as fromdt,'" + todt + "' as todt,a.fstr,a.erp_Code,a.ordno,A.VD as orddt,a.mrr,a.mrrdt,a.qtyord,a.soldqty,a.prate,a.acode,a.party,b.iname,B.CPARTNO, a.counter  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,ordno,to_char(orddt,'dd/MM/yyyy') as vd,null as mrr,null as mrrdt,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,null as party,acode,'1' as counter from pomas where branchcd='" + frm_mbr + "' and type in (" + frm_vty + ") and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '%' union all SELECT trim(a.icode)||'-'||to_ChaR(a.podate,'YYYYMMDD')||'-'||a.ponum||'-'||trim(a.ordlineno) as fstr,trim(a.Icode) as ERP_code,trim(a.ponum) as ordno,to_char(a.podate,'dd/MM/yyyy') as vd,trim(a.vchnum) as mrr,to_char(a.vchdate,'dd/mm/yyyy') as mrrdt,0 as Qtyord,a.iqty_chl as qtyord,0 as irate, b.aname as party,a.acode,'2' as counter from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and type!='04' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(a.Acode) like '%' and length(trim(a.ponum))=6 and trim(a.ponum)!='000000') a,item b where trim(a.erp_code)=trim(b.icode) and to_date(a.vd,'dd/mm/yyyy') " + xprdRange + " ORDER BY A.ACODE,A.ERP_code,A.ORDNO, a.counter ";
                    SQuery = "select distinct '" + fromdt + "' as fromdt,'" + todt + "' as todt,trim(a.ordno)||trim(a.type) as grp,a.fstr,a.erp_Code,a.ordno,A.VD as orddt,a.mrr,a.mrrdt,a.qtyord,a.soldqty,a.prate,a.acode,a.party,trim(b.iname) as iname,trim(B.CPARTNO) as cpartno,b.unit,a.counter,A.TYPE from (sELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,ordno,to_char(orddt,'dd/MM/yyyy') as vd,null as mrr,null as mrrdt,nvl(Qtyord,0) as qtyord,0 as Soldqty,((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0)) as prate,null as party,trim(acode) as acode,'1' as counter,TYPE from pomas where branchcd='" + frm_mbr + "' and type in (" + frm_vty + ") and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT trim(a.icode)||'-'||to_ChaR(a.podate,'YYYYMMDD')||'-'||a.ponum||'-'||trim(a.ordlineno) as fstr,trim(a.Icode) as ERP_code,trim(a.ponum) as ordno,to_char(a.podate,'dd/MM/yyyy') as vd,trim(a.vchnum) as mrr,to_char(a.vchdate,'dd/mm/yyyy') as mrrdt,0 as Qtyord,nvl(a.iqty_chl,0) as qtyord,0 as irate, trim(b.aname) as party,trim(a.acode) as acode,'2' as counter,A.TYPE from  ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and type!='04' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(a.Acode) like '%' and length(trim(a.ponum))=6 and trim(a.ponum)!='000000') a,item b where trim(a.erp_code)=trim(b.icode) and to_date(a.vd,'dd/mm/yyyy') " + xprdRange + " " + cond + " ORDER BY A.ACODE,A.ERP_code,A.ORDNO, a.counter,a.type ";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PO_VS_MRR", "std_PO_VS_MRR", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15144":  //TRIM PENDING IN THIS...QKI DATA NI THA
                // THIS REPORT IS MADE FOR GRIP
                header_n = "PR/PO/MRR Work Order No. Wise Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                //  SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header, TRIM(X.PR_NO)||TRIM(X.PR_DT) AS GRP,X.Ent_by,x.pr_no,x.psize as wono,y.Iname,x.pr_dt,x.PR_Status,x.app_by,y.cpartno,x.quot,x.icode,x.pr_Qty,y.unit,x.po_no,x.po_date,x.vendor,x.po_Qty,X.dlv_Date,x.mrr_no,x.mrr_Dt,x.mrr_qty,x.rej_qty,x.deptt,x.ent_dt,y.vat_code as Vref from (select m.psize,M.Ent_By,m.pr_no,m.pr_dt,(Case when m.pflag=1 then 'Curr' else 'Closed' End) as PR_Status,m.app_by,m.quot,m.icode,m.pr_Qty,m.vendor,m.po_no,m.po_date,m.po_Qty,m.po_madeby,M.Dlv_Date,nvl(n.vchnum,'-') as mrr_no,nvl(n.vchdate,sysdate) as mrr_Dt,nvl(n.iqtyin,0) as mrr_qty,nvl(n.rej_Rw,0) as rej_qty,m.deptt,m.ent_Dt,n.genum,n.gedate from  (select x.ordno as pr_no,x.psize,x.orddt as pr_Dt,x.Ent_By,x.app_by,x.quot,x.icode,x.pflag,x.qtyord as pr_qty,nvl(y.ordno,'-') as po_no,y.aname as Vendor,nvl(y.orddt,sysdate) as po_Date,nvl(y.qtyord,0) as po_qty,nvl(y.delv_item,'-') as Dlv_Date,y.po_madeby,x.deptt,x.ent_Dt from (Select ordno,orddt,icode,psize,qtyord,st38no as quot,ent_Dt,ent_by,bank as Deptt,app_by,pflag,acode from pomas where branchcd='" + frm_mbr + "' and substr(type,1,1)='6' and psize IN (" + mq0 + ") and orddt  " + xprdRange + " ) x left outer join (select a.ordno,a.orddt,a.pr_no,a.pr_dt,a.icode,a.QTYORD,a.delv_item,a.acode,b.aname,a.ent_by as po_madeby from pomas a, famst b  where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' and a.orddt  " + xprdRange + " )  y on trim(x.icode)=trim(y.icode) and trim(x.ordno)=trim(y.pr_no) and trim(x.orddt)=trim(y.pr_Dt) ) m left outer join (select vchnum,vchdate,ponum,podate,icode,iqtyin,rej_rw,genum,gedate from ivoucher where   branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and store='Y' and vchdate  " + xprdRange + "  ) n on m.icode=n.icode and m.po_no=n.ponum and m.po_date=n.podate order by m.pr_dt,m.pr_no,n.vchdate) x left outer join item y on trim(x.icode)=trim(y.icode) order by x.pr_dt,x.pr_no";
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header, TRIM(X.PR_NO)||TRIM(X.PR_DT)||trim(X.PSIZE) AS GRP,X.Ent_by,x.pr_no,x.psize as wono,y.Iname,x.pr_dt,x.PR_Status,x.app_by,y.cpartno,x.quot,x.icode,x.pr_Qty,y.unit,x.po_no,x.po_date,x.vendor,x.po_Qty,X.dlv_Date,x.mrr_no,x.mrr_Dt,x.mrr_qty,x.rej_qty,x.deptt,x.ent_dt,y.vat_code as Vref from (select m.psize,M.Ent_By,m.pr_no,m.pr_dt,(Case when m.pflag=1 then 'Curr' else 'Closed' End) as PR_Status,m.app_by,m.quot,m.icode,m.pr_Qty,m.vendor,m.po_no,m.po_date,m.po_Qty,m.po_madeby,M.Dlv_Date,nvl(n.vchnum,'-') as mrr_no,nvl(n.vchdate,sysdate) as mrr_Dt,nvl(n.iqtyin,0) as mrr_qty,nvl(n.rej_Rw,0) as rej_qty,m.deptt,m.ent_Dt,n.genum,n.gedate from  (select x.ordno as pr_no,x.psize,x.orddt as pr_Dt,x.Ent_By,x.app_by,x.quot,x.icode,x.pflag,x.qtyord as pr_qty,nvl(y.ordno,'-') as po_no,y.aname as Vendor,nvl(y.orddt,sysdate) as po_Date,nvl(y.qtyord,0) as po_qty,nvl(y.delv_item,'-') as Dlv_Date,y.po_madeby,x.deptt,x.ent_Dt from (Select ordno,orddt,icode,psize,qtyord,st38no as quot,ent_Dt,ent_by,bank as Deptt,app_by,pflag,acode from pomas where branchcd='" + frm_mbr + "' and substr(type,1,1)='6' and trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(psize) IN (" + mq0 + ") and orddt  " + xprdRange + " ) x left outer join (select a.ordno,a.orddt,a.pr_no,a.pr_dt,a.icode,a.QTYORD,a.delv_item,a.acode,b.aname,a.ent_by as po_madeby from pomas a, famst b  where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' and a.orddt  " + xprdRange + " )  y on trim(x.icode)=trim(y.icode) and trim(x.ordno)=trim(y.pr_no) and trim(x.orddt)=trim(y.pr_Dt) ) m left outer join (select vchnum,vchdate,ponum,podate,icode,iqtyin,rej_rw,genum,gedate from ivoucher where   branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and store='Y' and vchdate  " + xprdRange + "  ) n on m.icode=n.icode and m.po_no=n.ponum and m.po_date=n.podate order by m.pr_dt,m.pr_no,n.vchdate) x left outer join item y on trim(x.icode)=trim(y.icode) order by x.pr_dt,x.pr_no";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PR_VS_PO_Vs_MRR", "std_PR_VS_PO_Vs_MRR", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15222": //////////13.8.18
                header_n = "Sch Vs Rcpt Day Wise Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                //SQuery = "select '" + mq1 + "' as frmdt,'" + frm_myear + "' as todt,'" + header_n + "' as header,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31,sum(a.Rday1) as Rday1,sum(a.Rday2) as Rday2 ,sum(a.Rday3) as Rday3 ,sum(a.Rday4) as Rday4,sum(a.Rday5) as Rday5,sum(a.Rday6) as Rday6,sum(a.Rday7) as Rday7,sum(a.Rday8) as Rday8,sum(a.Rday9) as Rday9,sum(a.Rday10) as Rday10,sum(a.Rday11) as Rday11,sum(a.Rday12) as Rday12,sum(a.Rday13) as Rday13,sum(a.Rday14) as Rday14,sum(a.Rday15) as Rday15,sum(a.Rday16) as Rday16,sum(a.Rday17) as Rday17,sum(a.Rday18)as Rday18,sum(a.Rday19) as Rday19 ,sum(a.Rday20) as Rday20,sum(a.Rday21) as Rday21,sum(a.Rday22) as Rday22,sum(a.Rday23)as Rday23,sum(a.Rday24)as Rday24,sum(a.Rday25) as Rday25,sum(a.Rday26) as Rday26,sum(a.Rday27) as Rday27,sum(a.Rday28) as Rday28,sum(a.Rday29) as Rday29,sum(a.Rday30) as Rday30,sum(a.Rday31) as Rday31 from (SELECT Acode,icode,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31,0 AS Rday1,0 AS Rday2,0 AS Rday3,0 AS Rday4,0 AS Rday5,0 AS Rday6,0 AS Rday7,0 AS Rday8,0 AS Rday9,0 AS Rday10,0 AS Rday11,0 AS Rday12,0 AS Rday13,0 Rday14,0 AS Rday15,0 AS Rday16,0 AS Rday17,0 AS Rday18,0 AS Rday19,0 AS Rday20,0 AS Rday21,0 AS Rday22,0 AS Rday23,0 AS Rday24,0 AS Rday25,0 AS Rday26,0 AS Rday27,0 AS Rday28,0 AS Rday29,0 AS Rday30,0 AS Rday31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='66' and  to_char(vchdate,'MM/YYYY')='" + mq0 + "/" + frm_myear + "'  UNION ALL SELECT acode,icode,0 as DAY1,0 as day2,0 as day3,0 as day4,0 as day5,0 as day6,0 as day7,0 as day8,0 as day9,0 as day10 ,0 as day11,0 as day12, 0 as day13,0 as day14,0 as day15,0 as day16,0 as day17,0 as day18,0 as day19,0 as day20,0 as day21,0 as day22,0 as day23,0 as day24,0 as day25,0 as day26,0 as day27,0 as day28,0 as day29,0 as day30,0 as day31,(Case when to_char(vchdate,'dd')='01' then iqtyin else 0 end) as Rday1,(Case when to_char(vchdate,'dd')='02' then iqtyin else 0 end) as Rday2,(Case when to_char(vchdate,'dd')='03' then iqtyin else 0 end) as Rday3,(Case when to_char(vchdate,'dd')='04' then iqtyin else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyin else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyin else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyin else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyin else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyin else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyin else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyin else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyin else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyin else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyin else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyin else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyin else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyin else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyin else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyin else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyin  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyin else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyin  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyin else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyin  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyin  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyin else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyin else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyin  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyin  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyin  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyin  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type!='04' and to_char(vchdate,'MM/YYYY')='" + mq0 + "/" + frm_myear + "'  and store='Y') a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit order by a.acode";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (frm_cocd == "HPPI" || frm_cocd == "SPPI" || doc_GST == "GCC")
                {
                    frm_myear = frm_cDt1.Substring(6, 4);
                }
                else
                {
                    if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                    {

                    }
                    else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                }
                SQuery = "select '" + mq1 + "' as frmdt,'" + frm_myear + "' as todt,'" + header_n + "' as header,a.acode,a.icode,trim(b.aname) as aname,trim(c.iname) as iname,trim(c.cpartno) as cpartno,c.unit,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31,sum(a.Rday1) as Rday1,sum(a.Rday2) as Rday2 ,sum(a.Rday3) as Rday3 ,sum(a.Rday4) as Rday4,sum(a.Rday5) as Rday5,sum(a.Rday6) as Rday6,sum(a.Rday7) as Rday7,sum(a.Rday8) as Rday8,sum(a.Rday9) as Rday9,sum(a.Rday10) as Rday10,sum(a.Rday11) as Rday11,sum(a.Rday12) as Rday12,sum(a.Rday13) as Rday13,sum(a.Rday14) as Rday14,sum(a.Rday15) as Rday15,sum(a.Rday16) as Rday16,sum(a.Rday17) as Rday17,sum(a.Rday18)as Rday18,sum(a.Rday19) as Rday19 ,sum(a.Rday20) as Rday20,sum(a.Rday21) as Rday21,sum(a.Rday22) as Rday22,sum(a.Rday23)as Rday23,sum(a.Rday24)as Rday24,sum(a.Rday25) as Rday25,sum(a.Rday26) as Rday26,sum(a.Rday27) as Rday27,sum(a.Rday28) as Rday28,sum(a.Rday29) as Rday29,sum(a.Rday30) as Rday30,sum(a.Rday31) as Rday31 from (SELECT trim(Acode) as acode,trim(icode) as icode,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31,0 AS Rday1,0 AS Rday2,0 AS Rday3,0 AS Rday4,0 AS Rday5,0 AS Rday6,0 AS Rday7,0 AS Rday8,0 AS Rday9,0 AS Rday10,0 AS Rday11,0 AS Rday12,0 AS Rday13,0 Rday14,0 AS Rday15,0 AS Rday16,0 AS Rday17,0 AS Rday18,0 AS Rday19,0 AS Rday20,0 AS Rday21,0 AS Rday22,0 AS Rday23,0 AS Rday24,0 AS Rday25,0 AS Rday26,0 AS Rday27,0 AS Rday28,0 AS Rday29,0 AS Rday30,0 AS Rday31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='66' and  to_char(vchdate,'MM/YYYY')='" + mq0 + "/" + frm_myear + "' and acode like '" + party_cd + "%' and icode like '" + part_cd + "%'  UNION ALL SELECT trim(acode) as acode,trim(icode) as icode,0 as DAY1,0 as day2,0 as day3,0 as day4,0 as day5,0 as day6,0 as day7,0 as day8,0 as day9,0 as day10 ,0 as day11,0 as day12, 0 as day13,0 as day14,0 as day15,0 as day16,0 as day17,0 as day18,0 as day19,0 as day20,0 as day21,0 as day22,0 as day23,0 as day24,0 as day25,0 as day26,0 as day27,0 as day28,0 as day29,0 as day30,0 as day31,(Case when to_char(vchdate,'dd')='01' then nvl(iqtyin,0) else 0 end) as Rday1,(Case when to_char(vchdate,'dd')='02' then nvl(iqtyin,0) else 0 end) as Rday2,(Case when to_char(vchdate,'dd')='03' then nvl(iqtyin,0) else 0 end) as Rday3,(Case when to_char(vchdate,'dd')='04' then nvl(iqtyin,0) else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then nvl(iqtyin,0) else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then nvl(iqtyin,0) else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then nvl(iqtyin,0) else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then nvl(iqtyin,0) else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then nvl(iqtyin,0) else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then nvl(iqtyin,0) else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then nvl(iqtyin,0) else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then nvl(iqtyin,0) else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then nvl(iqtyin,0) else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then nvl(iqtyin,0) else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then nvl(iqtyin,0) else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then nvl(iqtyin,0) else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then nvl(iqtyin,0) else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then nvl(iqtyin,0) else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then nvl(iqtyin,0) else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then nvl(iqtyin,0)  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then nvl(iqtyin,0) else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then nvl(iqtyin,0)  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then nvl(iqtyin,0) else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then nvl(iqtyin,0)  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then nvl(iqtyin,0)  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then nvl(iqtyin,0) else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then nvl(iqtyin,0) else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then nvl(iqtyin,0)  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then nvl(iqtyin,0)  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then nvl(iqtyin,0)  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then nvl(iqtyin,0)  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type!='04' and to_char(vchdate,'MM/YYYY')='" + mq0 + "/" + frm_myear + "' and store='Y' and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,a.icode,trim(b.aname),trim(c.iname),trim(c.cpartno),c.unit order by a.acode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    pdfView = "Y";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Rcpt_DayWise", "std_Sch_Vs_Rcpt_DayWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15223": //////////13.8.18
                header_n = "Sch Vs Rcpt Total Basis Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                //SQuery = "select '" + mq1 + "' as frmdt,'" + frm_myear + "' as todt,'" + header_n + "' as header, a.acode,b.aname as Party,a.icode,c.iname as Item_Name,c.cpartno,c.unit,sum(a.sch_tot) as sch_tot,sum(a.rcpt_tot) as rcpt_tot,sum(a.sch_tot-a.rcpt_tot) as diff ,round(sum(a.rcpt_tot)/sum(NULLIF(A.sch_tot,0)),2) as per from (SELECT ACODE,ICODE,SUM(day1+day2+day3+day4+day5+day6+day7+day8+day9+day10+day11+day12+day13+day14+day15+day16+day17+day18+day19+day20+day21+day22+day23+day24+day25+day26+day27+day28+day29+day30+day31)  AS rcpt_tot,0 as sch_tot  FROM (SELECT acode,icode,(Case when to_char(vchdate,'dd')='01' then iqtyin  else 0 end) as day1,(Case when to_char(vchdate,'dd')='02' then iqtyin   else 0 end) as day2,(Case when to_char(vchdate,'dd')='03' then iqtyin else 0 end) as day3,(Case when to_char(vchdate,'dd')='04' then iqtyin   else 0 end) as day4,(Case when to_char(vchdate,'dd')='05' then iqtyin   else 0 end) as day5,(Case when to_char(vchdate,'dd')='06' then iqtyin   else 0 end) as day6 ,(Case when to_char(vchdate,'dd')='07' then iqtyin else 0 end) as day7,(Case when to_char(vchdate,'dd')='08' then iqtyin   else 0 end) as day8,(Case when to_char(vchdate,'dd')='09' then iqtyin   else 0 end) as day9,(Case when to_char(vchdate,'dd')='10' then iqtyin   else 0 end) as day10,(Case when to_char(vchdate,'dd')='11' then iqtyin else 0 end) as day11,(Case when to_char(vchdate,'dd')='12' then iqtyin  else 0 end) as day12,(Case when to_char(vchdate,'dd')='13' then iqtyin   else 0 end) as day13,(Case when to_char(vchdate,'dd')='14' then iqtyin   else 0 end) as day14,(Case when to_char(vchdate,'dd')='15'  then iqtyin   else 0 end) as day15,(Case when to_char(vchdate,'dd')='16' then iqtyin   else 0 end) as day16,(Case when to_char(vchdate,'dd')='17' then iqtyin   else 0 end) as day17,(Case when to_char(vchdate,'dd')='18' then iqtyin   else 0 end) as day18,(Case when to_char(vchdate,'dd')='19' then iqtyin  else 0 end) as day19,(Case when to_char(vchdate,'dd')='20' then iqtyin  else 0 end) as day20,(Case when to_char(vchdate,'dd')='21'  then iqtyin  else 0 end) as day21,(Case when to_char(vchdate,'dd')='22'  then iqtyin  else 0 end) as day22,(Case when to_char(vchdate,'dd')='23'  then iqtyin  else 0 end) as day23,(Case when to_char(vchdate,'dd')='24'  then iqtyin  else 0 end) as day24,(Case when to_char(vchdate,'dd')='25'  then iqtyin  else 0 end) as day25,(Case when to_char(vchdate,'dd')='26'  then iqtyin  else 0 end) as day26,(Case when to_char(vchdate,'dd')='27'  then iqtyin  else 0 end) as day27,(Case when to_char(vchdate,'dd')='28'  then iqtyin  else 0 end) as day28,(Case when to_char(vchdate,'dd')='29'  then iqtyin  else 0 end) as day29,(Case when to_char(vchdate,'dd')='30' then iqtyin else 0 end) as day30,(Case when to_char(vchdate,'dd')='31'  then iqtyin  else 0 end) as day31 from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and to_char(vchdate,'mm/yyyy')='" + mq0 + "/" + frm_myear + "' and store='Y') GROUP BY ACODE,ICODE union all select acode,icode,0 as rcpt_tot, total as sch_tot from schedule where branchcd='" + frm_mbr + "' and type='66' and to_char(vchdate,'mm/yyyy')='" + mq0 + "/" + frm_myear + "') a,famst b ,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (frm_cocd == "HPPI" || frm_cocd == "SPPI" || doc_GST == "GCC")
                {
                    frm_myear = frm_cDt1.Substring(6, 4);
                }
                else
                {
                    if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                    {

                    }
                    else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                }
                SQuery = "select '" + mq1 + "' as frmdt,'" + frm_myear + "' as todt,'" + header_n + "' as header, a.acode,trim(b.aname) as Party,a.icode,trim(c.iname) as Item_Name,trim(c.cpartno) as cpartno,c.unit,sum(a.sch_tot) as sch_tot,sum(a.rcpt_tot) as rcpt_tot,sum(a.sch_tot-a.rcpt_tot) as diff ,round(sum(a.rcpt_tot)/sum(NULLIF(A.sch_tot,0)),2) as per from (SELECT     ACODE,ICODE,SUM(day1+day2+day3+day4+day5+day6+day7+day8+day9+day10+day11+day12+day13+day14+day15+day16+day17+day18+day19+day20+day21+day22+day23+day24+day25+day26+day27+day28+day29+day30+day31)  AS rcpt_tot,0 as sch_tot  FROM (SELECT trim(acode) as acode,trim(icode) as icode,(Case when to_char(vchdate,'dd')='01' then nvl(iqtyin,0)  else 0 end) as day1,(Case when to_char(vchdate,'dd')='02' then nvl(iqtyin,0)   else 0 end) as day2,(Case when to_char(vchdate,'dd')='03' then nvl(iqtyin,0) else 0 end) as day3,(Case when to_char(vchdate,'dd')='04' then nvl(iqtyin,0)   else 0 end) as day4,(Case when to_char(vchdate,'dd')='05' then nvl(iqtyin,0)   else 0 end) as day5,(Case when to_char(vchdate,'dd')='06' then nvl(iqtyin,0)   else 0 end) as day6 ,(Case when to_char(vchdate,'dd')='07' then nvl(iqtyin,0) else 0 end) as day7,(Case when to_char(vchdate,'dd')='08' then nvl(iqtyin,0) else 0 end) as day8,(Case when to_char(vchdate,'dd')='09' then nvl(iqtyin,0)   else 0 end) as day9,(Case when to_char(vchdate,'dd')='10' then nvl(iqtyin,0) else 0 end) as day10,(Case when to_char(vchdate,'dd')='11' then nvl(iqtyin,0) else 0 end) as day11,(Case when to_char(vchdate,'dd')='12' then nvl(iqtyin,0)  else 0 end) as day12,(Case when to_char(vchdate,'dd')='13' then nvl(iqtyin,0)   else 0 end) as day13,(Case when to_char(vchdate,'dd')='14' then nvl(iqtyin,0)   else 0 end) as day14,(Case when to_char(vchdate,'dd')='15'  then nvl(iqtyin,0)   else 0 end) as day15,(Case when to_char(vchdate,'dd')='16' then nvl(iqtyin,0)   else 0 end) as day16,(Case when to_char(vchdate,'dd')='17' then nvl(iqtyin,0)   else 0 end) as day17,(Case when to_char(vchdate,'dd')='18' then nvl(iqtyin,0)   else 0 end) as day18,(Case when to_char(vchdate,'dd')='19' then nvl(iqtyin,0)  else 0 end) as day19,(Case when to_char(vchdate,'dd')='20' then nvl(iqtyin,0)  else 0 end) as day20,(Case when to_char(vchdate,'dd')='21'  then nvl(iqtyin,0)  else 0 end) as day21,(Case when to_char(vchdate,'dd')='22'  then nvl(iqtyin,0)  else 0 end) as day22,(Case when to_char(vchdate,'dd')='23'  then nvl(iqtyin,0)  else 0 end) as day23,(Case when to_char(vchdate,'dd')='24'  then nvl(iqtyin,0)  else 0 end) as day24,(Case when to_char(vchdate,'dd')='25'  then nvl(iqtyin,0)  else 0 end) as day25,(Case when to_char(vchdate,'dd')='26'  then nvl(iqtyin,0)  else 0 end) as day26,(Case when to_char(vchdate,'dd')='27'  then nvl(iqtyin,0)  else 0 end) as day27,(Case when to_char(vchdate,'dd')='28'  then nvl(iqtyin,0)  else 0 end) as day28,(Case when to_char(vchdate,'dd')='29'  then nvl(iqtyin,0)  else 0 end) as day29,(Case when to_char(vchdate,'dd')='30' then nvl(iqtyin,0) else 0 end) as day30,(Case when to_char(vchdate,'dd')='31'  then nvl(iqtyin,0)  else 0 end) as day31 from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and to_char(vchdate,'mm/yyyy')='" + mq0 + "/" + frm_myear + "' and store='Y' and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') GROUP BY ACODE,ICODE union all select trim(acode) as acode,trim(icode) as icode,0 as rcpt_tot, nvl(total,0) as sch_tot from schedule where branchcd='" + frm_mbr + "' and type='66' and to_char(vchdate,'mm/yyyy')='" + mq0 + "/" + frm_myear + "' and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,famst b ,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,trim(b.aname),a.icode,trim(c.iname),trim(c.cpartno),c.unit"; ;
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Rcpt_TotBasis", "std_Sch_Vs_Rcpt_TotBasis", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15230":///////13.8.18
                // header_n = "Price Comparison Chart Vendor Wise";
                header_n = "Monthly Comparative Purchase Rates for Each Vendor Item Wise";
                // SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, B.UNIT,A.ACODE,C.ANAME,b.INAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select  ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end ) as may,(Case when to_char(ORDDT,'mm')='06' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as mar  from POMAS where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + " ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, B.UNIT,A.acode,trim(C.ANAME) as aname,trim(b.INAME) as iname,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,trim(b.cpartno) as cpartno,trim(b.hscode) as hscode  from (select  trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(ORDDT,'mm')='04' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end ) as may,(Case when to_char(ORDDT,'mm')='06' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as mar from pomas where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,trim(b.iname),trim(b.cpartno),trim(b.hscode),A.ACODE,C.ANAME,B.UNIT ORDER BY ANAME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Price_Comp_Chart_VendorWise", "std_Price_Comp_Chart_VendorWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15231":
                header_n = "Monthly Comparative Purchase Rates for Each Item Vendor Wise";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, B.UNIT,A.ACODE,C.ANAME,b.INAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select  ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as mar  from POMAS where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + " ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY item_code";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, B.UNIT,A.ACODE,trim(C.ANAME) as aname,trim(b.INAME) as iname,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,trim(b.cpartno) as cpartno,trim(b.hscode) as hscode  from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(ORDDT,'mm')='04' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end) else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  else 0 end) as mar  from POMAS where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,trim(b.iname),trim(b.cpartno),trim(b.hscode),A.ACODE,trim(C.ANAME),B.UNIT ORDER BY item_code";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Price_Comp_Chart_ItemWise", "std_Price_Comp_Chart_ItemWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15232": ///////13.8.18
                header_n = "Price Comparison Chart Plant Wise";
                //SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, a.acode,A.ICODE,C.INAME,c.cpartno,c.unit,b.aname,max(a.plant00) as plant00,max(a.plant01) as plant01 ,max(a.plant02) as plant02 ,max(a.plant03) as plant03 ,max(a.plant04) as plant04 ,max(a.plant05) as plant05 ,max(a.plant06) as plant06 ,max(a.plant07) as plant07,max(a.plant08) as plant08 from ( select acode,ICODE,(Case when branchcd='" + frm_mbr + "' then ((prate*(100-pdisc)/100)-pdiscamt) else 0 end) as plant00,(Case when branchcd='01' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant01,(Case when branchcd='02' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )else 0 end) as plant02,(Case when branchcd='03' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant03,(Case when branchcd='04' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant04,(Case when branchcd='05' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant05,(Case when branchcd='06' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant06,(Case when branchcd='07' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant07 ,(Case when branchcd='08' then ((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant08 from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + xprdRange + ") a,famst b,ITEM C where trim(a.acode)=trim(b.acode) AND TRIM(A.ICODE)=TRIM(C.ICODE) group by a.acode,b.aname,C.INAME,A.ICODE,c.cpartno,c.unit order by a.acode";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, a.acode,A.ICODE,trim(C.INAME) as iname,trim(c.cpartno) as cpartno,c.unit,trim(b.aname) as aname,max(a.plant00) as plant00,max(a.plant01) as plant01 ,max(a.plant02) as plant02 ,max(a.plant03) as plant03 ,max(a.plant04) as plant04 ,max(a.plant05) as plant05 ,max(a.plant06) as plant06 ,max(a.plant07) as plant07,max(a.plant08) as plant08 from (select trim(acode) as acode,trim(ICODE) as icode,(Case when branchcd='00' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0)) else 0 end) as plant00,(Case when branchcd='01' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant01,(Case when branchcd='02' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )else 0 end) as plant02,(Case when branchcd='03' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant03,(Case when branchcd='04' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant04,(Case when branchcd='05' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant05,(Case when branchcd='06' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant06,(Case when branchcd='07' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant07 ,(Case when branchcd='08' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end ) else 0 end) as plant08 from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,trim(b.aname),trim(C.INAME),A.ICODE,trim(c.cpartno),c.unit order by a.acode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Price_Comp_Chart_PlantWise", "std_Price_Comp_Chart_PlantWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15235":///////13.8.18
                header_n = "Material Consumption Report Deptt Wise";
                //SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,t.name as deptt,a.acode,a.icode,c.iname as itemname,c.cpartno,c.unit,sum(iqtyout) as iqtyout,sum(iqtyin) as iqtyin,sum(iqtyout-iqtyin) as diff from (Select trim(acode) as acode,trim(icode) as icode,iqtyout as iqtyout,0 as iqtyin from ivoucher where branchcd='" + frm_mbr + "' and type like '3%' and type<>'36' and vchdate " + xprdRange + " and store='Y' union all Select trim(acode) as acode,trim(icode) as icode, 0 as iqtyout ,iqtyin from ivoucher where branchcd='" + frm_mbr + "' and type like '1%' and type<'15' and vchdate " + xprdRange + " and store='Y' ) a,item c ,type t where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(t.type1) and t.id='M' group by a.acode,a.icode,c.iname,c.cpartno,c.unit,t.name order by a.acode";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,trim(t.name) as deptt,a.acode,trim(a.icode) as icode,trim(c.iname) as itemname,trim(c.cpartno) as cpartno,c.unit,sum(iqtyout) as iqtyout,sum(iqtyin) as iqtyin,sum(iqtyout-iqtyin) as diff from (Select trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout,0 as iqtyin from ivoucher where branchcd='" + frm_mbr + "' and type like '3%' and type<>'36' and vchdate " + xprdRange + " and store='Y' and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%' union all Select trim(acode) as acode,trim(icode) as icode, 0 as iqtyout ,nvl(iqtyin,0) as iqtyin from ivoucher where branchcd='" + frm_mbr + "' and type like '1%' and type<'15' and vchdate " + xprdRange + " and store='Y' and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%') a,item c ,type t where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(t.type1) and t.id='M' group by a.acode,a.icode,trim(c.iname),trim(c.cpartno),c.unit,trim(t.name) order by a.acode ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Matl_Consumption", "std_Matl_Consumption", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15236"://////13.8.18
                header_n = "Supplier,Item Wise 12 Month P.O. Qty Report";
                //SQuery = " SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,A.ACODE,C.ANAME,b.INAME,B.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then QTYORD   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then QTYORD   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then QTYORD   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then QTYORD   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then QTYORD   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then QTYORD   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then QTYORD   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then QTYORD   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then QTYORD   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then QTYORD   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then QTYORD   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then QTYORD   else 0 end) as mar  from POMAS where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + ") a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.unit ORDER BY A.ACODE";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = " SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,A.ACODE,trim(C.ANAME) as aname,trim(b.INAME) as iname,B.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,trim(b.cpartno) as cpartno,trim(b.hscode) as hscode from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(ORDDT,'mm')='04' then nvl(QTYORD,0)   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then nvl(QTYORD,0)   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then nvl(QTYORD,0)   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then nvl(QTYORD,0)   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then nvl(QTYORD,0)   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then nvl(QTYORD,0)   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then nvl(QTYORD,0)   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then nvl(QTYORD,0)   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then nvl(QTYORD,0)   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then nvl(QTYORD,0)   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then nvl(QTYORD,0)   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then nvl(QTYORD,0)   else 0 end) as mar  from POMAS  where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,trim(b.iname),trim(b.cpartno),trim(b.hscode),A.ACODE,trim(C.ANAME),B.unit ORDER BY A.ACODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Supp_Item_POQty", "std_Supp_Item_POQty", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15237"://///13.8.18
                header_n = "Supplier,Item Wise 12 Month P.O. Value Report";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, A.ACODE,C.ANAME,b.INAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then QTYORD*PRATE   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then QTYORD*PRATE   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then QTYORD*PRATE   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then QTYORD*PRATE   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then QTYORD*PRATE   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then QTYORD*PRATE   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then QTYORD*PRATE   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then QTYORD*PRATE   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then QTYORD*PRATE   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then QTYORD*PRATE   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then QTYORD*PRATE   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then QTYORD*PRATE   else 0 end) as mar  from POMAS where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + ") a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME ORDER BY A.ACODE";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,A.ACODE,trim(C.ANAME) as aname,trim(b.INAME) as iname,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,trim(b.cpartno) as cpartno,trim(b.hscode) as hscode,trim(b.unit) as unit from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(ORDDT,'mm')='04' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then nvl(QTYORD,0)*nvl(PRATE,0)   else 0 end) as mar  from POMAS where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,trim(b.iname),trim(b.cpartno),trim(b.hscode),A.ACODE,trim(C.ANAME),trim(b.unit) ORDER BY A.ACODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Supp_Item_POQty", "std_Supp_Item_POQty", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15238"://13.8.18
                header_n = "Delivery Date Vs Rcpt Date";
                //SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.*,to_date(del,'dd/mm/yyyy')-to_date(vch,'dd/mm/yyyy') as diff,b.aname as party,c.iname as item_name,c.cpartno as part,c.unit from  (select acode,icode,ordno,max(del_date) as del,max(vchdate) as vch,sum(qty) as qty,sum(recd) as recd, max(type) AS TYPE from (select trim(acode) as acode,trIm(icode) as icode,trim(type) as type,trim(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,to_char(del_date,'dd/mm/yyyy') as del_date,null as vchdate,qtyord as qty, 0 as recd from pomas where branchcd='" + frm_mbr + "' and  type like '5%' and orddt " + xprdRange + " union all select trim(acode) as acode,trim(icode) as icode,null as type,trim(ponum) AS PONUM, to_char(podate,'dd/mm/yyyy') as podate ,null as del_date,to_char(vchdate,'dd/mm/yyyy') as vchdate,0 as qty, iqtyin as recd from ivoucher where branchcd='" + frm_mbr + "'and substr(potype,1,1)='5'  and vchdate " + xprdRange + " and store='Y') group by acode,icode,ordno) a,famst b, item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) order by a.vch";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.*,to_date(del,'dd/mm/yyyy')-to_date(vch,'dd/mm/yyyy') as diff,trim(b.aname) as party,trim(c.iname) as item_name,trim(c.cpartno) as part,c.unit from (select acode,icode,ordno,max(del_date) as del,max(vchdate) as vch,sum(qty) as qty,sum(recd) as recd, max(type) AS TYPE from (select trim(acode) as acode,trIm(icode) as icode,trim(type) as type,trim(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,to_char(del_date,'dd/mm/yyyy') as del_date,null as vchdate,nvl(qtyord,0) as qty, 0 as recd from pomas where branchcd='" + frm_mbr + "' and  type like '5%' and orddt " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all select trim(acode) as acode,trim(icode) as icode,null as type,trim(ponum) AS PONUM, to_char(podate,'dd/mm/yyyy') as podate ,null as del_date,to_char(vchdate,'dd/mm/yyyy') as vchdate,0 as qty,nvl(iqtyin,0) as recd from ivoucher where branchcd='" + frm_mbr + "'and substr(potype,1,1)='5'  and vchdate " + xprdRange + " and store='Y' and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' ) group by acode,icode,ordno) a,famst b, item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) order by a.vch";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_DelvDt_RcptDt", "std_DelvDt_RcptDt", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15239"://13.8.18
                header_n = "PO Item With Rate Increase/Decrease";
                //SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.ordno as po_no,to_char(a.orddt,'dd/MM/yyyy') as po_dt,a.app_by, a.acode,b.aname as party,a.icode,c.iname as item_name,c.cpartno as part,c.unit,a.prate,a.nxtmth,(a.prate-a.nxtmth) as diff,(case when prate>nxtmth then 'Inc' else 'Dec' end) as status  from pomas a,famst b ,item c  where nvl(trim(a.nxtmth),0)!='0' and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.prate != a.nxtmth and a.app_by !='-' and substr(a.icode,1,2) != '59' and a.branchcd='" + frm_mbr + "' and a.type like '5%' and  a.orddt " + xprdRange + " order by a.acode,po_no";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.ordno as po_no,to_char(a.orddt,'dd/MM/yyyy') as po_dt,a.app_by, trim(a.acode) as acode,trim(b.aname) as party,trim(a.icode) as icode,trim(c.iname) as item_name,trim(c.cpartno) as part,c.unit,nvl(a.prate,0) as prate,nvl(a.nxtmth,0) as nxtmth,(nvl(a.prate,0)-nvl(a.nxtmth,0)) as diff,(case when nvl(prate,0)>nvl(nxtmth,0) then 'Inc' else 'Dec' end) as status  from pomas a,famst b ,item c  where nvl(trim(a.nxtmth),0)!='0' and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.prate != a.nxtmth and a.app_by !='-' and substr(a.icode,1,2) != '59' and a.branchcd='" + frm_mbr + "' and a.type like '5%' and  a.orddt " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.acode,po_no";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Item_Inc_Decrease", "std_Item_Inc_Decrease", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15241":////14.8.18
                header_n = "Supplier History Card";
                //SQuery = "select distinct '" + header_n + "' as header,'" + fromdt + "' as fmdt,'" + todt + "' as todt,b.iname,b.cpartno,c.aname, c.addr1,c.addr2, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,a.iqtyin as recvd,a.rej_rw as rej,a.iqty_chl as advised,a.acpt_ud as accpted,a.desc_,a.naration,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdt from ivoucher a,item b,famst c where a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + xprdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a.vchnum";

                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select distinct '" + header_n + "' as header,'" + fromdt + "' as fmdt,'" + todt + "' as todt,trim(b.iname) as iname,trim(b.cpartno) as cpartno,trim(c.aname) as aname, trim(c.addr1) as addr1,trim(c.addr2) as addr2, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as acode,trim(a.icode) as icode,nvl(a.iqtyin,0) as recvd,nvl(a.rej_rw,0) as rej,nvl(a.iqty_chl,0) as advised,nvl(a.acpt_ud,0) as accpted,a.desc_,a.naration,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdt from ivoucher a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + xprdRange + "  and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and a.store='Y' order by a.vchnum";
                //select 'Supplier History Card' as header,'01/04/2019' as fmdt,'08/11/2019' as todt,a.acode,a.icode,trim(b.iname) as iname,trim(b.cpartno) as cpartno,trim(c.aname) as aname, trim(c.addr1) as addr1,trim(c.addr2) as addr2, a.vchnum,a.vchdate,sum(a.recvd) as recvd,sum(a.rej) as rej,sum(a.advised) as advised,sum(a.accpted) as accpted,a.desc_,a.naration,a.invno,a.invdt from (select trim(a.acode) as acode,trim(a.icode) as icode,a.vchnum,to_char(a.vchdate) as vchdate,nvl(a.iqtyin,0) as recvd,0 as rej,nvl(a.iqty_chl,0) as advised,nvl(a.acpt_ud,0) as accpted,a.desc_,a.naration,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdt from ivoucher a where a.branchcd='00' and a.type like '0%' and a.vchdate  between to_date('01/04/2019','dd/mm/yyyy') and to_date('08/11/2019','dd/mm/yyyy')  and a.acode like '%' and a.icode like '%' and a.store='Y' and a.vchnum='000003' union all select trim(a.acode) as acode,trim(a.icode) as icode,a.vchnum,to_char(a.vchdate) as vchdate,0 as recvd,nvl(a.rej_rw,0) as rej,0 as advised,0 as accpted,a.desc_,a.naration,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdt from ivoucher a where a.branchcd='00' and a.type like '0%' and a.vchdate  between to_date('01/04/2019','dd/mm/yyyy') and to_date('08/11/2019','dd/mm/yyyy') AND A.STORE='R'  and a.acode like '%' and a.icode like '%' and a.vchnum='000003' ) a,famst c,item b where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) group by a.acode,a.icode,trim(b.iname) ,trim(b.cpartno) ,trim(c.aname) , trim(c.addr1),trim(c.addr2) , a.vchnum,a.vchdate,a.desc_,a.naration,a.invno,a.invdt
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_History_card", "std_History_card", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15244":////14.8.18
                header_n = "Closed P.O. Register";
                //SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.type,a.ordno,to_char(a.orddt,'dd-Mon-yy') as orddt,a.icode,b.iname,a.qtyord,a.bank as deptt,a.qtybal,trim(a.test)||':'||to_char(a.invdate,'dd/mm/yyyy') as term,a.srno,b.cpartno as part,b.unit from pomas a,item b where  trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + xprdRange + " and a.pflag='1' order by a.srno";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,trim(a.type) as type,a.ordno,to_char(a.orddt,'dd-Mon-yy') as orddt,trim(a.icode) as icode,trim(b.iname) as iname,nvl(a.qtyord,0) as qtyord,trim(a.bank) as deptt,nvl(a.qtybal,0) as qtybal,trim(a.test)||':'||to_char(a.invdate,'dd/mm/yyyy') as term,a.srno,trim(b.cpartno) as part,b.unit from pomas a,item b where  trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + xprdRange + " and a.pflag='1' and a.icode like '" + party_cd + "%' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Closed_PO", "std_Closed_PO", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            ///changes by yogita and make rpt for this 4may 2018......cow
            case "F15240":///////14.8.18
                header_n = "PO Item With Qty Increase/Decrease";
                //SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.ordno as po_no,to_char(a.orddt,'dd/mm/yyyy') as po_Dt,a.app_by, a.acode,b.aname as party,a.icode,c.iname as item_name,c.cpartno as part,c.unit,a.prate,a.nxtmth,(a.prate-a.nxtmth) as diff,(case when prate>nxtmth then 'Increase' else 'Decrease' end) as status  from pomas a,famst b ,item c  where nvl(trim(a.nxtmth),0)!='0' and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.prate>a.nxtmth and a.app_by !='-' and a.branchcd='" + frm_mbr + "' and a.type like '5%' and  a.orddt " + xprdRange + " order by a.acode";
                //SQuery = "Select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.ordno as po_no, to_char(A.Orddt,'dd/mm/yyyy') as po_dt,to_char(a.orddt,'yyyymmdd') as vdd,a.app_by,trim(a.acode) as acode,B.aname as party,a.icode,C.iname as item_name,c.cpartno as part,c.unit,A.Qtyord as Qty_ord,(case when a.qtyord>A.wk1 then 'Increase' else 'Decrease' END) as Status, a.wk1 as PR_Qty,a.splrmk as Reason,A.Ent_By from pomas a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.Icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' and a.orddt " + xprdRange + "  and a.qtyord<>a.wk1 and a.wk1<>0 order by vdd";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "Select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.ordno as po_no, to_char(A.Orddt,'dd/mm/yyyy') as po_dt,to_char(a.orddt,'yyyymmdd') as vdd,a.app_by,trim(a.acode) as acode,trim(B.aname) as party,trim(a.icode) as icode,trim(C.iname) as item_name,trim(c.cpartno) as part,c.unit,nvl(A.Qtyord,0) as Qty_ord,(case when nvl(a.qtyord,0)>nvl(A.wk1,0) then 'Increase' else 'Decrease' END) as Status, nvl(a.wk1,0) as PR_Qty,trim(a.splrmk) as Reason,A.Ent_By from pomas a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.Icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' and a.orddt " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and a.qtyord<>a.wk1 and a.wk1<>0 order by vdd";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "PO_qty_Inc_Decrease", "PO_qty_Inc_Decrease", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15247": //  BY AKSHAY ON 08 JUNE 2018............14.8.18
                // header_n = "Schedule vs Dispatch (Qty Based)";
                header_n = "Schedule vs Receipt (Qty Based)";
                mq0 = "to_date('" + todt + "','dd/mm/yyyy')-1";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //ALREADY COMMENTED QUERY  SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, c.aname ,b.iname, b.unit,trim(substr(b.cpartno,1,15)) as partno , sum(a.y_sale) as Y_sale,sum(a.sch) as tot_sch,sum(a.tot_desp) as tot_desp,(sum(a.sch)-sum(a.tot_desp)) as bal  from (select TRIM(ICODE) AS ICODE , trim(acode) as acode,total as SCH,0 as y_sale,0 as TOT_DESP  from schedule where branchcd='" + frm_mbr + "' and type='46' and VCHDATE " + xprdRange + " union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH,0 as y_sale ,sum(iqtyout) as qty  from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE " + xprdRange + " group by  icode,acode union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH, SUM(Iqtyout) as y_sale ,0  AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE=" + mq0 + "    group by acode,ICODE ) a , ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.acode ,b.iname, b.unit, b.cpartno,c.aname having sum(a.sch)>0 order by c.aname";
                // DATA IS PICKED FROM TYPE 46 OF SCH,TYPE LIKE '4%' OF IVOUCHER THAT'S WHY BELOW QUERY IS COMMENTED AND NEW QUERY IS MADE  ON 14 NOV 2018 
                // SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, trim(c.aname) as aname ,trim(b.iname) as iname,  b.unit,trim(substr(b.cpartno,1,15)) as partno , sum(a.y_sale) as Y_sale,sum(a.sch) as tot_sch,sum(a.tot_desp) as tot_desp,(sum(a.sch)-sum(a.tot_desp)) as bal  from (select TRIM(ICODE) AS ICODE , trim(acode) as acode,nvl(total,0) as SCH,0 as y_sale,0 as TOT_DESP  from schedule where branchcd='" + frm_mbr + "' and type='46' and VCHDATE " + xprdRange + " union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH,0 as y_sale ,sum(nvl(iqtyout,0)) as qty from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE " + xprdRange + " group by  trim(acode),trim(ICODE) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH, SUM(nvl(Iqtyout,0)) as y_sale ,0  AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE=" + mq0 + "    group by trim(acode),trim(ICODE) ) a , ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.acode ,b.iname, b.unit, b.cpartno,c.aname having sum(a.sch)>0 order by aname";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, trim(c.aname) as aname ,trim(b.iname) as iname,  b.unit,trim(substr(b.cpartno,1,15)) as partno , sum(a.y_sale) as Y_sale,sum(a.sch) as tot_sch,sum(a.tot_desp) as tot_desp,(sum(a.sch)-sum(a.tot_desp)) as bal  from (select TRIM(ICODE) AS ICODE , trim(acode) as acode,nvl(total,0) as SCH,0 as y_sale,0 as TOT_DESP  from schedule where branchcd='" + frm_mbr + "' and type='66' and VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH,0 as y_sale ,sum(nvl(iqtyin,0)) as qty from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by  trim(acode),trim(ICODE) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH, SUM(nvl(Iqtyin,0)) as y_sale ,0  AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE=" + mq0 + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by trim(acode),trim(ICODE) ) a , ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.acode ,b.iname, b.unit, b.cpartno,c.aname having sum(a.sch)>0 order by aname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Schd_Vs_Disp_2", "std_Schd_Vs_Disp_2", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15248": //  BY AKSHAY ON 08 JUNE 2018////////////14.8.18
                header_n = "Schedule vs Dispatch (Value Based)";
                mq0 = "to_date('" + todt + "','dd/mm/yyyy')-1";
                //ALREADY COMMENTED QUERY SQuery = "SELECT  '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, C.ANAME,B.INAME,B.UNIT,TRIM(substr(B.CPARTNO,1,15)) AS PARTNO,SUM(A.Y_SALE) AS Y_SALE,SUM(A.SCH) AS TOT_SCH,SUM(A.TOT_DESP) AS TOT_DESP, SUM(A.SCH)-SUM(A.TOT_DESP) AS BAL FROM (select TRIM(ICODE) AS ICODE , trim(acode) as acode,(sum(total)*irate) as SCH,0 as y_sale,0 as TOT_DESP  from schedule where branchcd='" + frm_mbr + "' and type='46' and VCHDATE " + xprdRange + " group by ICODE,ACODE,IRATE union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH,0 as y_sale ,SUM(IAMOUNT) AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE " + xprdRange + "  group by acode,ICODE union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH, SUM(IAMOUNT) as y_sale ,0  AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE = " + mq0 + " group by acode,ICODE ) A,ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) GROUP BY C.ANAME,B.INAME,B.UNIT,B.CPARTNO having sum(a.SCH)>0 ORDER BY C.ANAME ";
                // DATA IS PICKED FROM TYPE 46 OF SCH,TYPE LIKE '4%' OF IVOUCHER THAT'S WHY BELOW QUERY IS COMMENTED AND NEW QUERY IS MADE  ON 14 NOV 2018
                // SQuery = "SELECT  '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, trim(C.ANAME) as aname,trim(B.INAME) as iname,B.UNIT,TRIM(substr(B.CPARTNO,1,15)) AS PARTNO,SUM(A.Y_SALE) AS Y_SALE,SUM(A.SCH) AS TOT_SCH,SUM(A.TOT_DESP) AS TOT_DESP, SUM(A.SCH)-SUM(A.TOT_DESP) AS BAL FROM (select TRIM(ICODE) AS ICODE , trim(acode) as acode,(sum(nvl(total,0))*nvl(irate,0)) as SCH,0 as y_sale,0 as TOT_DESP  from schedule  where branchcd='" + frm_mbr + "' and type='46' and VCHDATE " + xprdRange + "  group by trim(ICODE),trim(ACODE),irate union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH,0 as y_sale ,SUM(nvl(IAMOUNT,0)) AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE " + xprdRange + "   group by trim(ICODE),trim(ACODE) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH, SUM(nvl(IAMOUNT,0)) as y_sale ,0  AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE = " + mq0 + " group by trim(ICODE),trim(ACODE) ) A,ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) GROUP BY trim(C.ANAME),trim(B.INAME),B.UNIT,TRIM(substr(B.CPARTNO,1,15))  having sum(a.SCH)>0 ORDER BY ANAME";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "SELECT  '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, trim(C.ANAME) as aname,trim(B.INAME) as iname,B.UNIT,TRIM(substr(B.CPARTNO,1,15)) AS PARTNO,SUM(A.Y_SALE) AS Y_SALE,SUM(A.SCH) AS TOT_SCH,SUM(A.TOT_DESP) AS TOT_DESP, SUM(A.SCH)-SUM(A.TOT_DESP) AS BAL FROM (select TRIM(ICODE) AS ICODE , trim(acode) as acode,(sum(nvl(total,0))*nvl(irate,0)) as SCH,0 as y_sale,0 as TOT_DESP  from schedule  where branchcd='" + frm_mbr + "' and type='66' and VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by trim(ICODE),trim(ACODE),irate union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH,0 as y_sale ,SUM(nvl(IAMOUNT,0)) AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by trim(ICODE),trim(ACODE) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH, SUM(nvl(IAMOUNT,0)) as y_sale ,0  AS TOT_DESP from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE = " + mq0 + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by trim(ICODE),trim(ACODE) ) A,ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) GROUP BY trim(C.ANAME),trim(B.INAME),B.UNIT,TRIM(substr(B.CPARTNO,1,15))  having sum(a.SCH)>0 ORDER BY ANAME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Schd_Vs_Disp", "std_Schd_Vs_Disp", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15249": //  BY AKSHAY ON 08 JUNE 2018//14.8.18
                // header_n = "Schedule vs Dispatch (Qty, Value Based)";
                header_n = "Schedule vs Receipt (Qty, Value Based)";
                mq0 = "to_date('" + todt + "','dd/mm/yyyy')-1";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //ALREADY COMMENTED QUERY SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.ICODE,A.ACODE,B.INAME,C.ANAME,B.UNIT,TRIM(substr(B.CPARTNO,1,15)) AS PARTNO ,SUM(A.SCH_QTY) AS SCH_QTY ,SUM(A.SCH_VALUE) AS SCH_VALUE, SUM(A.Y_SALEQTY) AS Y_SALEQTY,SUM(A.y_SALERS) AS y_SALERS,SUM(A.DESP_QTY) AS DESP_QTY,SUM(A.DESP_VALUE) AS DESP_VALUE from(select TRIM(ICODE) AS ICODE , trim(acode) as acode, total as SCH_QTY ,(sum(total)*irate) as SCH_VALUE,0 as y_saleQTY,0 AS Y_SALERS,0 as DESP_QTY,0 AS DESP_VALUE  from schedule where branchcd='" + frm_mbr + "' and type='46' and VCHDATE " + xprdRange + " group by ICODE,ACODE,IRATE,total union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH_QTY,0 AS SCH_VALUE,0 AS Y_SALEQTY,0 AS Y_SALERS, sum(iqtyout) AS DESP_QTY  ,SUM(IAMOUNT) AS DESP_VALUE from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE " + xprdRange + " group by acode,ICODE union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH_QTY,0 AS SCH_VALUE,SUM(IAMOUNT) AS Y_SALERS, sum(iqtyout) AS Y_SALEQTY ,0 AS DESP_QTY,0 AS DESP_VALUE from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE=" + mq0 + " group by acode,ICODE)A, ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) GROUP BY A.ICODE,A.ACODE,B.INAME,C.ANAME,B.UNIT,B.CPARTNO having sum(a.sch_qty)>0 order by c.aname ";
                // DATA IS PICKED FROM TYPE 46 OF SCH,TYPE LIKE '4%' OF IVOUCHER THAT'S WHY BELOW QUERY IS COMMENTED AND NEW QUERY IS MADE  ON 14 NOV 2018 
                // SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,  A.ICODE,A.ACODE,trim(B.INAME)  as iname,trim(C.ANAME) as aname,B.UNIT,TRIM(substr(B.CPARTNO,1,15)) AS PARTNO ,SUM(A.SCH_QTY) AS SCH_QTY ,SUM(A.SCH_VALUE) AS SCH_VALUE, SUM(A.Y_SALEQTY) AS Y_SALEQTY,SUM(A.y_SALERS) AS y_SALERS,SUM(A.DESP_QTY) AS DESP_QTY,SUM(A.DESP_VALUE) AS DESP_VALUE from (select TRIM(ICODE) AS ICODE , trim(acode) as acode, nvl(total,0) as SCH_QTY ,(sum(nvl(total,0))*nvl(irate,0)) as SCH_VALUE,0 as y_saleQTY,0 AS Y_SALERS,0 as DESP_QTY,0 AS DESP_VALUE  from schedule where branchcd='" + frm_mbr + "' and type='46' and VCHDATE " + xprdRange + " group by TRIM(ICODE),trim(acode),IRATE,nvl(total,0) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH_QTY,0 AS SCH_VALUE,0 AS Y_SALEQTY,0 AS Y_SALERS, sum(nvl(iqtyout,0)) AS DESP_QTY  ,SUM(nvl(IAMOUNT,0)) AS DESP_VALUE from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE " + xprdRange + " group by TRIM(ICODE),trim(acode) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH_QTY,0 AS SCH_VALUE,SUM(nvl(IAMOUNT,0)) AS Y_SALERS, sum(nvl(iqtyout,0)) AS Y_SALEQTY ,0 AS DESP_QTY,0 AS DESP_VALUE from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE=" + mq0 + " group by TRIM(ICODE),trim(acode)) A, ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) GROUP BY A.ICODE,A.ACODE,trim(B.INAME),trim(C.ANAME),B.UNIT,TRIM(substr(B.CPARTNO,1,15))  having sum(a.sch_qty)>0 order by aname ";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,  A.ICODE,A.ACODE,trim(B.INAME)  as iname,trim(C.ANAME) as aname,B.UNIT,TRIM(substr(B.CPARTNO,1,15)) AS PARTNO ,SUM(A.SCH_QTY) AS SCH_QTY ,SUM(A.SCH_VALUE) AS SCH_VALUE, SUM(A.Y_SALEQTY) AS Y_SALEQTY,SUM(A.y_SALERS) AS y_SALERS,SUM(A.DESP_QTY) AS DESP_QTY,SUM(A.DESP_VALUE) AS DESP_VALUE from (select TRIM(ICODE) AS ICODE , trim(acode) as acode, nvl(total,0) as SCH_QTY ,(sum(nvl(total,0))*nvl(irate,0)) as SCH_VALUE,0 as y_saleQTY,0 AS Y_SALERS,0 as DESP_QTY,0 AS DESP_VALUE  from schedule where branchcd='" + frm_mbr + "' and type='66' and VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by TRIM(ICODE),trim(acode),IRATE,nvl(total,0) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH_QTY,0 AS SCH_VALUE,0 AS Y_SALEQTY,0 AS Y_SALERS, sum(nvl(iqtyin,0)) AS DESP_QTY  ,SUM(nvl(IAMOUNT,0)) AS DESP_VALUE from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by TRIM(ICODE),trim(acode) union all select TRIM(ICODE) AS ICODE ,trim(acode) as acode,0 as SCH_QTY,0 AS SCH_VALUE,SUM(nvl(IAMOUNT,0)) AS Y_SALERS, sum(nvl(iqtyin,0)) AS Y_SALEQTY ,0 AS DESP_QTY,0 AS DESP_VALUE from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE=" + mq0 + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' group by TRIM(ICODE),trim(acode)) A, ITEM B, FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) GROUP BY A.ICODE,A.ACODE,trim(B.INAME),trim(C.ANAME),B.UNIT,TRIM(substr(B.CPARTNO,1,15))  having sum(a.sch_qty)>0 order by aname ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Schd_Vs_Disp_3", "std_Schd_Vs_Disp_3", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15250":
                header_n = "Pending Purchase Order Register WithOut Line No.";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and A.acode like '" + party_cd + "%' and A.icode like '" + part_cd + "%' ";
                // mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                if (frm_vty.Contains("%"))
                {
                    //SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,f.aname,i.iname,i.cpartno,i.unit,A.* from WBVU_pendING_PO_old A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE like '5%' and A.ORDDT  " + xprdRange + " ORDER BY a.ordno";
                    SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,trim(f.aname) as aname,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,0 as stock,A.* from WBVU_pendING_PO_old A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE like '5%' and A.ORDDT  " + xprdRange + " " + cond + " ORDER BY a.ordno";
                }
                else
                {
                    //SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,f.aname,i.iname,i.cpartno,i.unit,A.* from WBVU_pendING_PO_old A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' and A.TYPE in (" + frm_vty + ") AND A.ORDDT  " + xprdRange + " ORDER BY a.ordno";
                    SQuery = "select '" + fromdt + "' AS frmdt,'" + todt + "' as todt1,'" + header_n + "' AS HEADER,A.TYPE||TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO) AS GRP,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORD_DT,substr(trim(a.icode),1,2) as mg,trim(f.aname) as aname,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,0 as stock,A.* from WBVU_pendING_PO_old A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT  " + xprdRange + " " + cond + " ORDER BY a.ordno";
                }
                xprd1 = " BETWEEN TO_DATE('" + frm_cDt1 + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')-1";
                dt2 = new DataTable();
                mq3 = "select TRIM(A.ICODE) AS ICODE,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr from itembal where BRANCHCD='" + frm_mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprd1 + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%'  AND VCHDATE between to_Date('" + todt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy') and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY TRIM(A.ICODE) ORDER BY ICODE";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq3);

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["stock"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "Closing_Stk").toDouble(2);
                    }
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pending_PO", "std_Pending_PO", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F15605":
                #region
                header_n = "RFQ Comparison";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                dt2 = new DataTable();
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("icode", typeof(string));
                #region//FOR FIRST COLUMN
                dt2.Columns.Add("col_1", typeof(string));
                dt2.Columns.Add("col_2", typeof(string));
                dt2.Columns.Add("col_3", typeof(string));
                dt2.Columns.Add("col_4", typeof(double));//basic
                dt2.Columns.Add("col_5", typeof(double));//disc
                dt2.Columns.Add("col_6", typeof(double));//cgst
                dt2.Columns.Add("col_7", typeof(double));//sgst/utgst
                dt2.Columns.Add("col_8", typeof(double));
                dt2.Columns.Add("col_9", typeof(double)); //igst %
                dt2.Columns.Add("col_10", typeof(string)); //tax category
                dt2.Columns.Add("col_11", typeof(double));//total value
                dt2.Columns.Add("col_12", typeof(string)); //price basis
                dt2.Columns.Add("col_13", typeof(string));//frisght
                dt2.Columns.Add("col_14", typeof(string));//payment term
                dt2.Columns.Add("col_15", typeof(string));//delv term
                //dt2.Columns.Add("col_16", typeof(string));
                #endregion
                #region      //FOR 2 COLUMN
                dt2.Columns.Add("col_17", typeof(string));
                dt2.Columns.Add("col_18", typeof(string));
                dt2.Columns.Add("col_19", typeof(string));
                dt2.Columns.Add("col_20", typeof(double));
                dt2.Columns.Add("col_21", typeof(double));
                dt2.Columns.Add("col_22", typeof(double));
                dt2.Columns.Add("col_23", typeof(double));
                dt2.Columns.Add("col_24", typeof(double));
                dt2.Columns.Add("col_25", typeof(double));
                dt2.Columns.Add("col_26", typeof(string));
                dt2.Columns.Add("col_27", typeof(double));
                dt2.Columns.Add("col_28", typeof(string));
                dt2.Columns.Add("col_29", typeof(string));
                dt2.Columns.Add("col_30", typeof(string));
                dt2.Columns.Add("col_31", typeof(string));
                //dt2.Columns.Add("col_32", typeof(string));
                #endregion
                #region///FOR 3 COLUMN
                dt2.Columns.Add("col_33", typeof(string));
                dt2.Columns.Add("col_34", typeof(string));
                dt2.Columns.Add("col_35", typeof(string));
                dt2.Columns.Add("col_36", typeof(double));
                dt2.Columns.Add("col_37", typeof(double));
                dt2.Columns.Add("col_38", typeof(double));
                dt2.Columns.Add("col_39", typeof(double));
                dt2.Columns.Add("col_40", typeof(double));
                dt2.Columns.Add("col_41", typeof(double));
                dt2.Columns.Add("col_42", typeof(string));
                dt2.Columns.Add("col_43", typeof(double));
                dt2.Columns.Add("col_44", typeof(string));
                dt2.Columns.Add("col_45", typeof(string));
                dt2.Columns.Add("col_46", typeof(string));
                dt2.Columns.Add("col_47", typeof(string));
                //dt2.Columns.Add("col_48", typeof(string));
                #endregion
                dr1 = dt2.NewRow();
                //  SQuery = "select a.* from WB_PORFQ a where trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode) in (" + mq0 + ") order by ordno";
                SQuery = "select a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,nvl(a.PO_TOLR,0) as extra,a.DELV_TERM,a.doc_thr as payterm,c.aname as vendor,b.iname as item,trim(a.icode) as icode,trim(a.acode) as acode,nvl(a.prate,0) as basic_rate,a.tax as tax_catg,a.pexc as tax, nvl(a.pdisc,0) as disc,a.pbasis,a.rate_cd as taxable,a.othamt1 as tax_total,a.othamt2 as cgst,a.othamt3 as sgst_utgst,a.freight,a.tr_insur as insur_term  from WB_PORFQ a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and  trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode) in (" + mq0 + ") order by ordno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dr1 = dt2.NewRow();
                    dr1["header"] = header_n;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //for 1st column
                        dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        if (i == 0)
                        {
                            #region
                            dr1["col_1"] = "RFQ # " + dt.Rows[i]["ordno"].ToString().Trim() + " Dt. " + dt.Rows[i]["orddt"].ToString().Trim();
                            dr1["col_2"] = dt.Rows[i]["vendor"].ToString().Trim();
                            dr1["col_3"] = dt.Rows[i]["item"].ToString().Trim();
                            dr1["col_4"] = fgen.make_double(dt.Rows[i]["basic_rate"].ToString().Trim());
                            dr1["col_5"] = fgen.make_double(dt.Rows[i]["disc"].ToString().Trim());
                            dr1["col_6"] = fgen.make_double(dt.Rows[i]["cgst"].ToString().Trim());
                            dr1["col_7"] = fgen.make_double(dt.Rows[i]["sgst_utgst"].ToString().Trim());
                            dr1["col_8"] = fgen.make_double(dt.Rows[i]["extra"].ToString().Trim());
                            dr1["col_9"] = fgen.make_double(dt.Rows[i]["tax"].ToString().Trim());
                            dr1["col_10"] = dt.Rows[i]["tax_catg"].ToString().Trim();
                            dr1["col_11"] = fgen.make_double(dt.Rows[i]["tax_total"].ToString().Trim());
                            dr1["col_12"] = dt.Rows[i]["pbasis"].ToString().Trim();
                            dr1["col_13"] = dt.Rows[i]["freight"].ToString().Trim();
                            dr1["col_14"] = dt.Rows[i]["payterm"].ToString().Trim();
                            dr1["col_15"] = dt.Rows[i]["DELV_TERM"].ToString().Trim();
                            //   dr1["col_16"] = dt.Rows[i][""].ToString().Trim();                         
                            #endregion
                        }
                        //for 2nd column
                        if (i == 1)
                        {
                            #region
                            dr1["col_17"] = "RFQ # " + dt.Rows[i]["ordno"].ToString().Trim() + " Dt. " + dt.Rows[i]["orddt"].ToString().Trim();
                            dr1["col_18"] = dt.Rows[i]["vendor"].ToString().Trim();
                            dr1["col_19"] = dt.Rows[i]["item"].ToString().Trim();
                            dr1["col_20"] = fgen.make_double(dt.Rows[i]["basic_rate"].ToString().Trim());
                            dr1["col_21"] = fgen.make_double(dt.Rows[i]["disc"].ToString().Trim());
                            dr1["col_22"] = fgen.make_double(dt.Rows[i]["cgst"].ToString().Trim());
                            dr1["col_23"] = fgen.make_double(dt.Rows[i]["sgst_utgst"].ToString().Trim());
                            dr1["col_24"] = fgen.make_double(dt.Rows[i]["extra"].ToString().Trim());
                            dr1["col_25"] = fgen.make_double(dt.Rows[i]["tax"].ToString().Trim());
                            dr1["col_26"] = dt.Rows[i]["tax_catg"].ToString().Trim();
                            dr1["col_27"] = fgen.make_double(dt.Rows[i]["tax_total"].ToString().Trim());
                            dr1["col_28"] = dt.Rows[i]["pbasis"].ToString().Trim();
                            dr1["col_29"] = dt.Rows[i]["freight"].ToString().Trim();
                            dr1["col_30"] = dt.Rows[i]["payterm"].ToString().Trim();
                            dr1["col_31"] = dt.Rows[i]["DELV_TERM"].ToString().Trim();
                            //   dr1["col_32"] = dt.Rows[i][""].ToString().Trim();                        
                            #endregion
                        }
                        //for 3rd column
                        if (i == 2)
                        {
                            #region
                            dr1["col_33"] = "RFQ # " + dt.Rows[i]["ordno"].ToString().Trim() + " Dt. " + dt.Rows[i]["orddt"].ToString().Trim();
                            dr1["col_34"] = dt.Rows[i]["vendor"].ToString().Trim();
                            dr1["col_35"] = dt.Rows[i]["item"].ToString().Trim();
                            dr1["col_36"] = fgen.make_double(dt.Rows[i]["basic_rate"].ToString().Trim());
                            dr1["col_37"] = fgen.make_double(dt.Rows[i]["disc"].ToString().Trim());
                            dr1["col_38"] = fgen.make_double(dt.Rows[i]["cgst"].ToString().Trim());
                            dr1["col_39"] = fgen.make_double(dt.Rows[i]["sgst_utgst"].ToString().Trim());
                            dr1["col_40"] = fgen.make_double(dt.Rows[i]["extra"].ToString().Trim());
                            dr1["col_41"] = fgen.make_double(dt.Rows[i]["tax"].ToString().Trim());
                            dr1["col_42"] = dt.Rows[i]["tax_catg"].ToString().Trim();
                            dr1["col_43"] = fgen.make_double(dt.Rows[i]["tax_total"].ToString().Trim());
                            dr1["col_44"] = dt.Rows[i]["pbasis"].ToString().Trim();
                            dr1["col_45"] = dt.Rows[i]["freight"].ToString().Trim();
                            dr1["col_46"] = dt.Rows[i]["payterm"].ToString().Trim();
                            dr1["col_47"] = dt.Rows[i]["DELV_TERM"].ToString().Trim();
                            // dr1["col_48"] = dt.Rows[i][""].ToString().Trim();                         
                            #endregion
                        }
                    }
                    dt2.Rows.Add(dr1);
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(dt2);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_RFQ", "std_RFQ", dsRep, header_n);
                }
                #endregion
                break;

            case "F15601":
                #region RFQ
                sname = "";
                header_n = "Request For Quotation";
                SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'-' AS btoprint,d.ANAME,TRIM(D.ANAME) AS CUST,TRIM(D.ADDR1) AS ADRES1,TRIM(D.ADDR2) AS ADRES2,TRIM(D.ADDR3) AS ADRES3,TRIM(D.GIRNO) AS CUSTPAN,TRIM(D.STAFFCD) AS STAFFCD,TRIM(D.PERSON) AS CPERSON,TRIM(D.EMAIL) AS CMAIL,TRIM(D.TELNUM) AS CONT,TRIM(D.STATEN) AS CSTATE, TRIM(D.GST_NO) AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,TRIM(B.NAME) AS TYPENAME,TRIM(C.INAME) AS INAME,TRIM(C.CPARTNO) AS  PARTNO,TRIM(C.PUR_UOM) AS CMT,TRIM(C.NO_PROC) AS Sunit,TRIM(C.UNIT) AS CUNIT,TRIM(C.HSCODE) AS HSCODE,A.*,'" + header_n + "' AS CASE,nvl(d.email,'-') as p_email FROM WB_PORFQ A,TYPE B,ITEM C,wbvu_fam_vend D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.orddt,a.ordno,A.srno ";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    pdfView = "Y";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='RFQ' ORDER BY SRNO";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt1.TableName = "type1";
                    mq10 = "";
                    dt3 = new DataTable();
                    mdr = null;
                    dt3.Columns.Add("poterms", typeof(string));
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            mq10 += dt1.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                        }
                    }
                    else
                    {
                        mq10 = "-"; // IF THERE ARE NO RFQ TERMS AND CONDITIONS THEN '-' IS PASSED SO THAT IT DOES NOT GIVE TABLE NOT FOUND ERROR.
                    }
                    mdr = dt3.NewRow();
                    mdr["poterms"] = mq10;
                    dt3.Rows.Add(mdr);
                    dt3.TableName = "type1";
                    dsRep.Tables.Add(dt3);
                    frm_rptName = "RFQ";
                    if (doc_GST == "GCC") frm_rptName = "RFQ_INTL";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "RFQ", frm_rptName, dsRep, header_n, "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F15120":
                header_n = "Supplier Performance Rating";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select distinct a.* from SCRATCH a where a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')='" + mq0 + "' and a.type='1S' and a.branchcd='" + frm_mbr + "' order by a.col1 asc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_supp_rat", "std_supp_rat", dsRep, header_n);
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

        //DataSet dsk = new DataSet();
        //dsk.ReadXml(xfilepath);
        //dsk.WriteXmlSchema(Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xsd"));

        //ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/tej-base/test_pr.rdlc");

        //ReportDataSource rds = new ReportDataSource("Prepcur", dsk.Tables[0]);
        //ReportViewer1.LocalReport.DataSources.Clear();
        //ReportViewer1.LocalReport.DataSources.Add(rds);

        //ReportViewer1.LocalReport.DataSources.Add(dsk);        

        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
            data_found = "Y";
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
            data_found = "Y";
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
            //repDoc.Close();
            //repDoc.Dispose();
            CrystalReportViewer1.Dispose();
            CrystalReportViewer1 = null;
            GC.Collect();
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
            if (frm_cocd == "KRSM" && hfhcid.Value== "F1004")
            {
                try { pdfno = ds.Tables["Prepcur"].Rows[0]["fstr"].ToString(); } catch { }
                try { pdffirm = ds.Tables["Prepcur"].Rows[0]["Aname"].ToString(); } catch { }
                try { pdfdoc = ds.Tables["Prepcur"].Rows[0]["typename"].ToString(); } catch { }
                frm_FileName = pdfdoc.Replace(' ', '_') + "__" + pdffirm.Replace(' ', '_') + "__" + pdfno;
            }
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

    protected void btnsendmail_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string aname1 = "", mq1 = "";
            DataTable dt = new DataTable();
            DataTable mdt = new DataTable();
            DataTable fdt = new DataTable();
            DataSet data_set = new DataSet();
            data_set = (DataSet)Session["data_set"];
            DataView dv = new DataView(data_set.Tables[0], "", "acode", DataViewRowState.CurrentRows);

            fdt = data_set.Tables[0];
            mdt = dv.ToTable(true, "acode", "p_email");
            DataSet dsRep = new DataSet();
            DataRow dr;
            dt = fdt.Clone();

            foreach (DataRow dr1 in mdt.Rows)
            {
                if (dr1["p_email"].ToString().Length > 2)
                {
                    dsRep = new DataSet();
                    dt = new DataTable();
                    dt = fdt.Clone();
                    DataTable dt1 = new DataTable();
                    dv = new DataView(fdt, "acode='" + dr1["acode"].ToString().Trim() + "'", "acode", DataViewRowState.CurrentRows);
                    dt1 = dv.ToTable();
                    foreach (DataRow drdt1 in dt1.Rows)
                    {
                        dr = dt.NewRow();
                        aname1 = drdt1["aname"].ToString().Trim();
                        foreach (DataColumn dcdt in dt.Columns)
                        {
                            if (drdt1[dcdt.ColumnName] == null) dr[dcdt.ColumnName] = 0;
                            else dr[dcdt.ColumnName] = drdt1[dcdt.ColumnName];
                        }
                        dt.Rows.Add(dr);
                    }

                    string repname = "";
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    switch (hfhcid.Value)
                    {
                        case "F1004":
                            repname = frm_cocd == "DREM" ? "std_po_drem" : "std_po";
                            break;
                        case "F15134":
                            repname = "std_po_schedule";
                            break;
                    }

                    html_body(aname1, mq1);
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);

                    Print_Report_BYDS(frm_cocd, frm_mbr, repname, repname, dsRep, "", "Y");
                    Attachment atchfile = new Attachment(repDoc.ExportToStream(ExportFormatType.PortableDocFormat), frm_cocd + "_" + subj.Replace(" ", "_") + ".pdf");
                    fgen.send_mail(frm_qstr, frm_cocd, "Tejaxo ERP", dr1["p_email"].ToString().Trim(), txtemailcc.Text, txtemailbcc.Text, subj, xhtml_tag, atchfile);
                    if (hfhcid.Value == "F15134")
                    {
                        if (dt.Rows.Count > 0)
                            fgen.save_Mailbox3(frm_qstr, frm_cocd, frm_mbr, dr1["p_email"].ToString().Trim(), subj + " (Party Code : " + dr1["acode"].ToString().Trim() + ", Mth : " + dt.Rows[0]["MTHNAME"].ToString() + ") ", frm_uname, "");
                    }
                    CrystalReportViewer1.Dispose();
                }
            }
            fgen.send_cookie("Send_Mail", "N");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnsendmail');", true);
        }
        catch (Exception ex)
        {
        }
    }

    public void html_body(string party_name, string oth_var1)
    {
        firm = fgenCO.chk_co(frm_cocd); xhtml_tag = "";
        firm = firm.Replace("XXXX", frm_cocd);

        xhtml_tag = xhtml_tag + "<br>M/s " + party_name + "<br>";
        xhtml_tag = xhtml_tag + "<h4><B>Dear Sir/Mam, </B></h4>";
        switch (hfhcid.Value)
        {
            case "F1004":
                subj = "Tejaxo ERP: Purchase Order from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>We are glad to release '" + oth_var1 + "' for your kind Persuel.";
                xhtml_tag = xhtml_tag + "<BR>Please Find the Attached Pdf.<BR>";
                xhtml_tag = xhtml_tag + "<BR>In case any Clarification Please do revert.<BR>";
                break;
            case "F15134":
                subj = "Tejaxo ERP: Purchase Schedule from " + firm + "";
                xhtml_tag = xhtml_tag + "<B>Please find the attached schedule for the month. </B><BR>";
                xhtml_tag = xhtml_tag + "Kindly acknowledge the receipt.<BR><BR>";

                xhtml_tag = xhtml_tag + "1) Please maintain the VMI(Vendor management Inventory) 20% qty of above schedule<BR>";
                xhtml_tag = xhtml_tag + "2) Please adhere to weekly schedule only<BR>";

                xhtml_tag = xhtml_tag + "Please Deliver Material as per daily/weekly Schedule.<BR>";
                xhtml_tag = xhtml_tag + "<BR>In case any Clarification Please do revert.<BR>";
                break;
        }
        xhtml_tag = xhtml_tag + "<br><br><b>Thanks & Regards,</b>";
        xhtml_tag = xhtml_tag + "<br><b>" + firm + "</b>";
        if (frm_cocd != "BUPL")
            xhtml_tag = xhtml_tag + "<br><br><br>Note: This is an automatically generated email from Tejaxo ERP, Please do not reply";
        xhtml_tag = xhtml_tag + "</body></html>";
    }
}