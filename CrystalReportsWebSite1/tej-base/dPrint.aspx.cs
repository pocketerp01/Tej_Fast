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
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

public partial class fin_base_dPrint : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_userid, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath = "", fpath1, fpath2, fpath3, fpath4, fpath4_irn = "", frm_cDt2, col1, col2, col3, printBar = "N", frm_ulvl;
    fgenDB fgen = new fgenDB();
    DataSet ds = new DataSet();
    DataSet dsRep = new DataSet();
    private DataSet DsImages = new DataSet();
    string prpdt = "", rmvdt = "", blogo_opt = "";
    FileStream FilStr = null; FileStream FilStr1 = null; FileStream FilStr2 = null; FileStream FilStr3 = null; FileStream FilStr4 = null;
    BinaryReader BinRed = null; BinaryReader BinRed1 = null; BinaryReader BinRed2 = null; BinaryReader BinRed3 = null; BinaryReader BinRed4 = null;
    string header_n = ""; string PARTY = "";
    DataRow oporrow, dr1;
    string reportActionCode = ""; string mq5 = "";
    double db = 0, db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0, db9 = 0, db10 = 0, db11 = 0, db12 = 0, db13 = 0, db14 = 0;
    double db15 = 0, db16 = 0, db17 = 0, db18 = 0, db19 = 0, db20 = 0, db21 = 0, db22 = 0, db23 = 0, db24 = 0, db25 = 0, db26 = 0, db27 = 0;
    string value3 = ""; string value2 = ""; string value1 = "";
    string signedQRCode, signedInvoice, Irn, IrnQrCodeValue, invoiceBarcodeImage;
    DataTable dt7, dt8, dtm3, dt2;
    DataTable Wp_dt;
    double papergiven = 0;
    double jcqty1 = 0;
    int index = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["frm_pdfname"] = null;
        try
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Guid.NewGuid().ToString().Substring(0, 20).ToUpper();
                    str = Request.QueryString["STR"].Trim().ToString();
                    frm_cocd = str.Split('@')[2].ToString().Trim().ToUpper();
                    frm_myear = str.Split('@')[3].ToString().Trim().ToUpper().Substring(0, 4);
                    frm_mbr = str.Split('@')[3].ToString().Trim().ToUpper().Substring(4, 2);
                    frm_uname = str.Split('@')[4].ToString().Trim().ToUpper();
                    frm_userid = frm_uname;
                    hfhcid.Value = str.Split('@')[6].ToString().Trim();
                    hfval.Value = str.Split('@')[7].ToString().Trim();
                    try
                    {
                        hfclose.Value = str.Split('@')[8].ToString().Trim();
                    }
                    catch { hfclose.Value = ""; }

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
            //if (!Page.IsPostBack)
            {
                btnPrintToPrinter.Visible = false;
                printCrpt(hfhcid.Value);
            }
            if (hfclose.Value == "CLOSE*")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
            }
        }
        catch (Exception ex)
        {
            fgen.FILL_ERR(ex.Message + "--> dprint");
            if (hfclose.Value == "CLOSE*")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
            }
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
        }
    }
    void printCrpt(string iconID)
    {
        var v1 = ";;;".Split(';');
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;
        DataRow mdr, dr1;
        dsRep = new DataSet();
        string barCode = hfval.Value;
        string scode = barCode;
        string sname = "";
        string mq10 = "", mq1 = "", mq0 = "", mq4 = "", mq2 = "", mq3 = "", mq11 = "", mq12 = "", mq13 = "";
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        frm_ulvl = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ULEVEL FROM EVAS WHERE USERID='" + frm_userid + "' ", "ULEVEL");
        reportActionCode = iconID;
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
            //M.R.R.
            case "F1002":
                #region M.R.R.
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                if (scode.Length > 20)
                    sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                else sname = "'" + sname + "'" + " and " + "'" + sname + "'";
                dt = new DataTable();
                //SQuery = "SELECT 'Purchase Requisition' AS HEADER, B.INAME AS ITEM_NAME,B.CPARTNO,B.HSCODE,C.INAME AS SUBNAME ,A.* FROM POMAS A,ITEM B ,ITEM C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR((A.ICODE),1,4)=TRIM(C.ICODE)  AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') in ('" + scode + "') ORDER BY A.SRNO ";
                SQuery = "select f.addr1 as caddr1,f.addr2 as caddr2,f.addr3 as caddr3,f.addr4 as caddr4,f.mobile as ctel,f.aname,f.gst_no as cgst_no,f.email as cemail,t.name as mrrtype,i.unit as iunit,i.iname,i.cpartno as icpartno,b.amt_sale as totamt,b.bill_tot as grandtot, b.amt_exc as cgst_val,b.rvalue as sgst_val,B.EXCB_CHG AS TXBL,trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.* from ivoucher a,item i,famst f,type t,ivchctrl b  where trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||TRIM(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and trim(a.type)=trim(t.type1) and t.id='M' and trim(a.branchcd)='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchnum between " + sname + " and a.vchdate " + xprdRange + " order by a.vchdate,a.vchnum,a.MORDER";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_mrr", frm_rptName, dsRep, "M.R.R Report");
                }
                #endregion
                break;
            //P.R.
            case "F1003":
                #region P.R.
                sname = "";
                dt = new DataTable();
                SQuery = "SELECT 'Purchase Requisition' AS HEADER, B.INAME AS ITEM_NAME,b.CINAME,B.CPARTNO,B.HSCODE,C.INAME AS SUBNAME ,A.* FROM POMAS A,ITEM B ,ITEM C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR((A.ICODE),1,4)=TRIM(C.ICODE)  AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') in ('" + scode + "') ORDER BY A.SRNO ";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_pr", frm_rptName, dsRep, "P.R Report");
                }
                #endregion
                break;
            //P.O.
            case "F1004":
                #region P.O.
                sname = "";
                v1 = scode.Split(';');
                for (int k = 0; k < v1.Length; k++)
                {
                    if (!v1[k].Contains("/"))
                    {
                        if (sname.Length > 0)
                        {
                            if (v1[k].ToString().ToString().Length > 6)
                            {
                                sname = sname + "," + "'" + v1[k].ToString().Substring(3, 6) + "'";
                                frm_mbr = v1[k].ToString().Substring(0, 2);
                                frm_vty = v1[k].ToString().Substring(2, 2);
                            }
                            else sname = sname + "," + "'" + v1[k].ToString() + "'";
                        }
                        else
                        {
                            if (v1[k].ToString().ToString().Length > 6)
                            {
                                sname = "'" + v1[k].ToString().Substring(4, 6) + "'";
                                frm_mbr = v1[k].ToString().Substring(0, 2);
                                frm_vty = v1[k].ToString().Substring(2, 2);
                            }
                            else sname = "'" + v1[k].ToString() + "'";
                        }
                    }
                }
                SQuery = "SELECT D.ANAME AS CUST,D.ADDR1 AS ADRES1,D.ADDR2 AS ADRES2,D.ADDR3 AS ADRES3,D.GIRNO AS CUSTPAN,D.STAFFCD,D.PERSON AS CPERSON,D.EMAIL AS CMAIL,D.mobile AS CONT,D.STATEN AS CSTATE, D.GST_NO AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,B.NAME AS TYPENAME,C.INAME,c.CINAME,C.CPARTNO AS  PARTNO,C.PUR_UOM AS CMT,C.NO_PROC AS Sunit,C.UNIT AS CUNIT,C.HSCODE,a.type,a.ordno as vchnum,to_Char(a.orddt,'dd/mm/yyyy') as vchdate,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.ordno in (" + sname + ") and a.orddt " + xprdRange + " ORDER BY a.ordno,A.ICODE ";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));

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
                    mdr["poterms"] = mq10;
                    dt3.Rows.Add(mdr);
                    dt3.TableName = "type1";
                    dsRep.Tables.Add(dt3);
                    if (frm_cocd == "DREM")
                    {
                        mq2 = "";
                        if (dt.Rows[0]["type"].ToString().Trim() == "52")
                        {
                            mq2 = "SELECT COL1,SRNO FROM DOCTERMS WHERE BRANCHCD='00' AND TYPE='70' AND DOCTYPE='PO2' ORDER BY SRNO";
                        }
                        else
                        {
                            mq2 = "SELECT COL1,SRNO FROM DOCTERMS WHERE BRANCHCD='00' AND TYPE='70' AND DOCTYPE='PO1' ORDER BY SRNO";
                        }
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                        mq10 = "";
                        dt4 = new DataTable();
                        mdr = null;
                        dt4.Columns.Add("poaddterms", typeof(string));
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            mq10 += dt2.Rows[i]["COL1"].ToString().Trim() + Environment.NewLine;
                        }
                        mdr = dt4.NewRow();
                        mdr["poaddterms"] = mq10;
                        dt4.Rows.Add(mdr);
                        dt4.TableName = "AddTerms";
                        dsRep.Tables.Add(dt4);
                        mq3 = "select trim(vchnum) as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,terms,condi from poterm where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchnum in (" + sname + ") and vchdate " + xprdRange + " order by vchnum,sno";
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq3);
                        dt4 = new DataTable();
                        dt4.Columns.Add("vchnum", typeof(string));
                        dt4.Columns.Add("vchdate", typeof(string));
                        dt4.Columns.Add("terms", typeof(string));
                        dt4.Columns.Add("condi", typeof(string));
                        mq11 = ""; mq12 = ""; mq13 = ""; mq0 = "";
                        if (dt3.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                if (mq0 != dt3.Rows[i]["vchnum"].ToString().Trim())
                                {
                                    if (mq12.Length > 1)
                                    {
                                        mdr = dt4.NewRow();
                                        mdr["vchnum"] = mq10;
                                        mdr["vchdate"] = mq11;
                                        mdr["terms"] = mq12;
                                        mdr["condi"] = mq13;
                                        dt4.Rows.Add(mdr);
                                    }
                                    mdr = null;
                                    mq10 = dt3.Rows[i]["vchnum"].ToString().Trim();
                                    mq11 = dt3.Rows[i]["vchdate"].ToString().Trim();
                                    mq12 = ""; mq13 = "";
                                }
                                mq0 = dt3.Rows[i]["vchnum"].ToString().Trim();
                                mq12 += dt3.Rows[i]["terms"].ToString().Trim() + Environment.NewLine;
                                mq13 += dt3.Rows[i]["condi"].ToString().Trim() + Environment.NewLine;
                            }
                            mdr = dt4.NewRow();
                            mdr["vchnum"] = mq10;
                            mdr["vchdate"] = mq11;
                            mdr["terms"] = mq12;
                            mdr["condi"] = mq13;
                            dt4.Rows.Add(mdr);
                        }
                        else
                        {
                            // WHEN THERE IS NO ANNEXURE'S TERM & CONDITIONS
                            mq2 = "select distinct a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt from pomas a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.ordno in (" + sname + ") and a.orddt " + xprdRange + "";
                            dt3 = new DataTable();
                            dt3 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                            foreach (DataRow dr in dt3.Rows)
                            {
                                mdr = dt4.NewRow();
                                mdr["vchnum"] = dr["ordno"].ToString().Trim();
                                mdr["vchdate"] = dr["orddt"].ToString().Trim();
                                mdr["terms"] = "-";
                                mdr["condi"] = "-";
                                dt4.Rows.Add(mdr);
                            }
                        }
                        dt4.TableName = "Product_Terms";
                        dsRep.Tables.Add(dt4);
                        frm_rptName = "std_po_Drem_Terms";
                    }
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_po", frm_rptName, dsRep, "P.O. Entry Report", "Y");
                }
                #endregion
                break;
            //S.O.
            case "F1005":
                #region S.O.
                sname = "";
                v1 = scode.Split(';');
                for (int k = 0; k < v1.Length; k++)
                {
                    if (!v1[k].Contains("/"))
                    {
                        if (sname.Length > 0)
                        {
                            if (v1[k].ToString().ToString().Length > 6)
                            {
                                sname = sname + "," + "'" + v1[k].ToString().Substring(3, 6) + "'";
                                frm_mbr = v1[k].ToString().Substring(0, 2);
                                frm_vty = v1[k].ToString().Substring(2, 2);
                            }
                            else sname = sname + "," + "'" + v1[k].ToString() + "'";
                        }
                        else
                        {
                            if (v1[k].ToString().ToString().Length > 6)
                            {
                                sname = "'" + v1[k].ToString().Substring(4, 6) + "'";
                                frm_mbr = v1[k].ToString().Substring(0, 2);
                                frm_vty = v1[k].ToString().Substring(2, 2);
                            }
                            else sname = "'" + v1[k].ToString() + "'";
                        }
                    }
                }
                barCode = v1[v1.Length - 1].ToString();
                SQuery = "Select 'SOMAS' as TAB_NAME,'SO Number' as h1,'SO Dated' as h2,G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'SOMAS' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,a.ordno as vchnum,to_char(a.orddt,'dd/mm/yyyy') as vchdate,a.type,A.* from somas a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.ordno in (" + sname + ") and a.orddt " + xprdRange + " order by a.ordno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));

                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_so", frm_rptName, dsRep, "S.O. Entry Report", "Y");
                }
                #endregion
                break;
            //INV

            case "F1006":
            case "F1033":
                if (frm_cocd == "SSPL" || frm_cocd == "SWRN")
                {
                    #region INV SSPL AND SWRN
                    scode = scode.Replace(";", "");
                    mq1 = scode.Substring(2, 2);
                    mq11 = ""; mq12 = ""; mq13 = "";
                    mq13 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ENABLE_YN FROM CONTROLS WHERE ID='B23'", "ENABLE_YN");
                    if (mq13 == "Y")
                    {
                        // NAME OF SIGNATORY
                        mq11 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT params FROM CONTROLS WHERE ID='B23'", "params");
                    }

                    mq13 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ENABLE_YN FROM CONTROLS WHERE ID='B24'", "ENABLE_YN");
                    if (mq13 == "Y")
                    {
                        // DESIGNATION OF SIGNATORY
                        mq12 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT params FROM CONTROLS WHERE ID='B24'", "params");
                    }

                    // ORIGINAL  SQuery = "select distinct a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'Tax Invoice' as header,b.country,b.staten as state,substr(trim(b.gst_no),1,2) as statecode,A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt. '||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,D.CINAME,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,d.cdrgno as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,C.tcsamt,B.VENCODE,c.ins_amt,'" + mq11 + "' as sign_name,'" + mq12 + "' as sign_desig from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) and trim(A.BRANCHCD)||trim(A.TYPE)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + scode + "')  order by vchdate,a.vchnum,a.MORDER";
                    //if (mq1 == "41" && frm_cocd == "SSPL")
                    //{
                    //    SQuery = "select distinct a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'Tax Invoice' as header,b.country,b.staten as state,substr(trim(b.gst_no),1,2) as statecode,A.MORDER, 'N' as logo_yn, a.branchcd,a.type,d.cpartno as dpartno,a.exc_57f4,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.bill_tot,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.freight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,c.ins_no,b.aname,a.srno,a.icode,a.purpose as iname,d.cdrgno as cpartno,a.irate,a.revis_no as cdrgno,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,c.cscode,a.no_bdls as pkg,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,t.type1,C.tcsamt,B.VENCODE,c.ins_amt,'" + mq11 + "' as sign_name,'" + mq12 + "' as sign_desig,replace(i.desc_,'Agst Chl #','') as matrec,A.TC_NO from sale c,item d,ivoucher a left join ivoucher i on trim(a.branchcd)||trim(a.tc_no)||trim(a.acode)||trim(a.icode)=trim(i.branchcd)||trim(i.vchnum)||trim(i.acode)||trim(i.icode) and i.type='25',famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) and trim(A.BRANCHCD)||trim(A.TYPE)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + scode + "')  order by vchdate,a.vchnum,a.MORDER";
                    //}
                    //else if (mq1 == "49" && frm_cocd == "SWRN")
                    //{
                    //    SQuery = "select distinct a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'Tax Invoice' as header,b.country,b.staten as state,substr(trim(b.gst_no),1,2) as statecode,A.MORDER, 'N' as logo_yn, a.branchcd,a.type,d.cpartno as dpartno,a.exc_57f4,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.bill_tot,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.freight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,c.ins_no,b.aname,a.srno,a.icode,a.purpose as iname,d.cdrgno as cpartno,a.irate,a.revis_no as cdrgno,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,c.cscode,a.no_bdls as pkg,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,t.type1,C.tcsamt,B.VENCODE,c.ins_amt,'" + mq11 + "' as sign_name,'" + mq12 + "' as sign_desig,replace(i.desc_,'Agst Chl #','') as matrec,A.TC_NO from sale c,item d,ivoucher a left join ivoucher i on trim(a.branchcd)||trim(a.tc_no)||trim(a.acode)||trim(a.icode)=trim(i.branchcd)||trim(i.vchnum)||trim(i.acode)||trim(i.icode) and i.type='27',famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) and trim(A.BRANCHCD)||trim(A.TYPE)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + scode + "')  order by vchdate,a.vchnum,a.MORDER";
                    //}
                    //else
                    //{
                    //    SQuery = "select distinct a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'Tax Invoice' as header,b.country,b.staten as state,substr(trim(b.gst_no),1,2) as statecode,A.MORDER, 'N' as logo_yn, a.branchcd,a.type,d.cpartno as dpartno,a.exc_57f4,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.bill_tot,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.freight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,c.ins_no,b.aname,a.srno,a.icode,a.purpose as iname,d.cdrgno as cpartno,a.irate,a.revis_no as cdrgno,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,c.cscode,a.no_bdls as pkg,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,t.type1,C.tcsamt,B.VENCODE,c.ins_amt,'" + mq11 + "' as sign_name,'" + mq12 + "' as sign_desig,NULL AS matrec,null as TC_NO from ivoucher a,sale c,item d,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) and trim(A.BRANCHCD)||trim(A.TYPE)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + scode + "')  order by vchdate,a.vchnum,a.MORDER";
                    //}
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //----NEW
                    string cscode = "";
                    SQuery = "select distinct a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'Tax Invoice' as header,b.country,b.staten as state,A.MORDER, 'N' as logo_yn, a.branchcd,a.type,d.cpartno as dpartno,a.exc_57f4,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel, b.aname,a.srno,a.icode,a.purpose as iname,d.cdrgno as cpartno,a.irate,a.revis_no as cdrgno,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.no_bdls as pkg,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.staffcd as statecode,B.VENCODE,'" + mq11 + "' as sign_name,'" + mq12 + "' as sign_desig,A.TC_NO,to_char(a.refdate,'dd/mm/yyyy') as refdate from ivoucher a,item d,famst b  where trim(a.acode)=trim(b.acode)  and trim(A.icode)=trim(d.icode) and trim(A.BRANCHCD)||trim(A.TYPE)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + scode + "') order by vchdate,a.vchnum,a.MORDER";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    string SQuery1 = "select c.branchcd||c.type||Trim(c.vchnum)||to_Char(c.vchdate,'yyyymmdd') as fstr,c.vchnum,to_char(c.vchdate,'dd/mm/yyyy') as vchdate,c.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.bill_tot,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.freight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,c.ins_no,c.cscode,C.tcsamt,c.ins_amt,c.exc_57f4 as insur_policy_no from sale c where trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAR(c.vchdate,'DD/MM/YYYY') in ('" + scode + "') ";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

                    #region Columns Creation
                    dt2 = new DataTable();
                    dt2 = dt.Clone();
                    dt2.Columns.Add("chlnum", typeof(string));
                    dt2.Columns.Add("sd_val", typeof(string));
                    dt2.Columns.Add("chldate", typeof(string));
                    dt2.Columns.Add("pono", typeof(string));
                    dt2.Columns.Add("podate", typeof(string));
                    dt2.Columns.Add("mode_tpt", typeof(string));
                    dt2.Columns.Add("mo_vehi", typeof(string));
                    dt2.Columns.Add("insur_no", typeof(string));
                    dt2.Columns.Add("st_entform", typeof(string));
                    dt2.Columns.Add("ins_cert", typeof(string));
                    dt2.Columns.Add("grno", typeof(string));
                    dt2.Columns.Add("grdate", typeof(string));
                    dt2.Columns.Add("stform_no", typeof(string));
                    dt2.Columns.Add("bill_tot", typeof(double));
                    dt2.Columns.Add("amt_sale", typeof(double));
                    dt2.Columns.Add("amt_exc", typeof(double));
                    dt2.Columns.Add("rvalue", typeof(double));
                    dt2.Columns.Add("amt_job", typeof(double));
                    dt2.Columns.Add("st_amt", typeof(double));
                    dt2.Columns.Add("amt_rea", typeof(double));
                    dt2.Columns.Add("remvdate", typeof(string));
                    dt2.Columns.Add("remvtime", typeof(string));
                    dt2.Columns.Add("bill_qty", typeof(string));
                    dt2.Columns.Add("naration", typeof(string));
                    dt2.Columns.Add("freight", typeof(string));
                    dt2.Columns.Add("invtime", typeof(string));
                    dt2.Columns.Add("st31_form", typeof(string));
                    dt2.Columns.Add("ins_co", typeof(string));
                    dt2.Columns.Add("ins_no", typeof(string));
                    dt2.Columns.Add("cscode", typeof(string));
                    dt2.Columns.Add("tcsamt", typeof(double));
                    dt2.Columns.Add("ins_amt", typeof(double));
                    dt2.Columns.Add("matrec", typeof(string));
                    dt2.Columns.Add("matrecdt", typeof(string));
                    dt2.Columns.Add("insur_policy_no", typeof(string));
                    #endregion

                    if (dt.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dt);
                        dt3 = new DataTable();
                        dt3 = dv.ToTable(true, "fstr");
                        foreach (DataRow dr in dt3.Rows)
                        {
                            dt4 = new DataTable();
                            if (dt1.Rows.Count > 0)
                            {
                                DataView dv1 = new DataView(dt1, "fstr='" + dr["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt4 = dv1.ToTable();
                            }

                            DataView dv2 = new DataView(dt, "fstr='" + dr["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt5 = new DataTable();
                            dt5 = dv2.ToTable();
                            int i = 0;
                            for (int k = 0; k < dt5.Rows.Count; k++)
                            {
                                dr1 = dt2.NewRow();
                                #region Ivoucher Table
                                dr1["fstr"] = dt5.Rows[k]["fstr"].ToString();
                                dr1["vchnum"] = dt5.Rows[k]["vchnum"].ToString();
                                dr1["vchdate"] = dt5.Rows[k]["vchdate"].ToString();
                                dr1["acode"] = dt5.Rows[k]["acode"].ToString();
                                dr1["header"] = dt5.Rows[k]["header"].ToString();
                                dr1["country"] = dt5.Rows[k]["country"].ToString();
                                dr1["state"] = dt5.Rows[k]["state"].ToString();
                                dr1["statecode"] = dt5.Rows[k]["statecode"].ToString();
                                dr1["MORDER"] = dt5.Rows[k]["MORDER"].ToString();
                                dr1["logo_yn"] = dt5.Rows[k]["logo_yn"].ToString();
                                dr1["branchcd"] = dt5.Rows[k]["branchcd"].ToString();
                                dr1["type"] = dt5.Rows[k]["type"].ToString();
                                dr1["dpartno"] = dt5.Rows[k]["dpartno"].ToString();
                                dr1["exc_57f4"] = dt5.Rows[k]["exc_57f4"].ToString();
                                dr1["caddr1"] = dt5.Rows[k]["caddr1"].ToString();
                                dr1["caddr2"] = dt5.Rows[k]["caddr2"].ToString();
                                dr1["caddr3"] = dt5.Rows[k]["caddr3"].ToString();
                                dr1["caddr4"] = dt5.Rows[k]["caddr4"].ToString();
                                dr1["ctel"] = dt5.Rows[k]["ctel"].ToString();
                                dr1["aname"] = dt5.Rows[k]["aname"].ToString();
                                dr1["srno"] = dt5.Rows[k]["srno"].ToString();
                                dr1["icode"] = dt5.Rows[k]["icode"].ToString();
                                dr1["iname"] = dt5.Rows[k]["iname"].ToString();
                                dr1["cpartno"] = dt5.Rows[k]["cpartno"].ToString();
                                dr1["irate"] = dt5.Rows[k]["irate"].ToString();
                                dr1["cdrgno"] = dt5.Rows[k]["cdrgno"].ToString();
                                dr1["cdisc"] = dt5.Rows[k]["cdisc"].ToString();
                                dr1["iamount"] = dt5.Rows[k]["iamount"].ToString();
                                dr1["qty"] = dt5.Rows[k]["qty"].ToString();
                                dr1["desc_"] = dt5.Rows[k]["desc_"].ToString();
                                dr1["pkg"] = dt5.Rows[k]["pkg"].ToString();
                                dr1["unit"] = dt5.Rows[k]["unit"].ToString();
                                dr1["cgst"] = dt5.Rows[k]["cgst"].ToString();
                                dr1["cgst_val"] = dt5.Rows[k]["cgst_val"].ToString();
                                dr1["sgst"] = dt5.Rows[k]["sgst"].ToString();
                                dr1["sgst_val"] = dt5.Rows[k]["sgst_val"].ToString();
                                dr1["iopr"] = dt5.Rows[k]["iopr"].ToString();
                                dr1["hscode"] = dt5.Rows[k]["hscode"].ToString();
                                dr1["cgst_no"] = dt5.Rows[k]["cgst_no"].ToString();
                                dr1["VENCODE"] = dt5.Rows[k]["VENCODE"].ToString();
                                dr1["sign_name"] = dt5.Rows[k]["sign_name"].ToString();
                                dr1["sign_desig"] = dt5.Rows[k]["sign_desig"].ToString();
                                dr1["TC_NO"] = dt5.Rows[k]["TC_NO"].ToString();
                                dr1["refdate"] = dt5.Rows[k]["refdate"].ToString();
                                if (mq1 == "41" && frm_cocd == "SSPL")
                                {
                                    dr1["matrec"] = fgen.seek_iname(frm_qstr, frm_cocd, "select replace(desc_,'Agst Chl #','') as desc_ from ivoucher where branchcd='" + scode.Substring(0, 2) + "' and type='25' and vchnum='" + dt5.Rows[i]["tc_no"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt5.Rows[i]["refdate"].ToString().Trim() + "'", "desc_");
                                }
                                else if (mq1 == "49" && frm_cocd == "SWRN")
                                {
                                    dr1["matrec"] = fgen.seek_iname(frm_qstr, frm_cocd, "select replace(desc_,'Agst Chl #','') as desc_ from ivoucher where branchcd='" + scode.Substring(0, 2) + "' and type='27' and vchnum='" + dt5.Rows[i]["tc_no"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt5.Rows[i]["refdate"].ToString().Trim() + "'", "desc_");
                                }
                                #endregion
                                #region Sale Table
                                if (dt4.Rows.Count > 0)
                                {
                                    dr1["chlnum"] = dt4.Rows[i]["chlnum"].ToString();
                                    dr1["sd_val"] = dt4.Rows[i]["sd_val"].ToString();
                                    dr1["chldate"] = dt4.Rows[i]["chldate"].ToString();
                                    dr1["pono"] = dt4.Rows[i]["pono"].ToString();
                                    dr1["podate"] = dt4.Rows[i]["podate"].ToString();
                                    dr1["mode_tpt"] = dt4.Rows[i]["mode_tpt"].ToString();
                                    dr1["mo_vehi"] = dt4.Rows[i]["mo_vehi"].ToString();
                                    dr1["insur_no"] = dt4.Rows[i]["insur_no"].ToString();
                                    dr1["st_entform"] = dt4.Rows[i]["st_entform"].ToString();
                                    dr1["ins_cert"] = dt4.Rows[i]["ins_cert"].ToString();
                                    dr1["grno"] = dt4.Rows[i]["grno"].ToString();
                                    dr1["grdate"] = dt4.Rows[i]["grdate"].ToString();
                                    dr1["stform_no"] = dt4.Rows[i]["stform_no"].ToString();
                                    dr1["bill_tot"] = dt4.Rows[i]["bill_tot"].ToString();
                                    dr1["amt_sale"] = dt4.Rows[i]["amt_sale"].ToString();
                                    dr1["amt_exc"] = dt4.Rows[i]["amt_exc"].ToString();
                                    dr1["rvalue"] = dt4.Rows[i]["rvalue"].ToString();
                                    dr1["amt_job"] = dt4.Rows[i]["amt_job"].ToString();
                                    dr1["st_amt"] = dt4.Rows[i]["st_amt"].ToString();
                                    dr1["amt_rea"] = dt4.Rows[i]["amt_rea"].ToString();
                                    dr1["remvdate"] = dt4.Rows[i]["remvdate"].ToString();
                                    dr1["remvtime"] = dt4.Rows[i]["remvtime"].ToString();
                                    dr1["bill_qty"] = dt4.Rows[i]["bill_qty"].ToString();
                                    dr1["naration"] = dt4.Rows[i]["naration"].ToString();
                                    dr1["freight"] = dt4.Rows[i]["freight"].ToString();
                                    dr1["invtime"] = dt4.Rows[i]["invtime"].ToString();
                                    dr1["st31_form"] = dt4.Rows[i]["st31_form"].ToString();
                                    dr1["ins_co"] = dt4.Rows[i]["ins_co"].ToString();
                                    dr1["ins_no"] = dt4.Rows[i]["ins_no"].ToString();
                                    dr1["cscode"] = dt4.Rows[i]["cscode"].ToString();
                                    cscode = dt4.Rows[i]["cscode"].ToString().Trim();
                                    dr1["tcsamt"] = dt4.Rows[i]["tcsamt"].ToString();
                                    dr1["ins_amt"] = dt4.Rows[i]["ins_amt"].ToString();
                                    dr1["insur_policy_no"] = dt4.Rows[i]["insur_policy_no"].ToString();
                                }
                                #endregion
                                dt2.Rows.Add(dr1);
                            }
                        }
                    }
                    dsRep = new DataSet();
                    if (dt2.Rows.Count > 0)
                    {
                        dt2.Columns.Add(new DataColumn("PkgN", typeof(double)));
                        foreach (DataRow dr in dt2.Rows)
                        {
                            dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                        }
                        dt2.TableName = "Prepcur";
                        if (mq1 == "41" && frm_cocd == "SWRN")
                        {
                            repCount = 3;
                            dsRep.Tables.Add(fgen.mTitle_41(dt2, repCount));
                        }
                        else
                        {
                            repCount = 5;
                            if (frm_cocd == "MLGI" || frm_cocd == "WING" || frm_cocd == "AEPL") repCount = 4;
                            dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                        }
                        if (frm_cocd == "WING" || frm_cocd == "AEPL") repCount = 5;
                        //csmst                
                        SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,/*substr(d.gst_no,0,2)*/ cstaffcd as dstatecode from csmst d where trim(d.acode)= '" + cscode.Trim() + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count <= 0)
                        {
                            dt = new DataTable();
                            SQuery = "Select 'SAME AS RECIPIENT' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        }
                        dt.TableName = "csmst";
                        dsRep.Tables.Add(dt);

                        if (frm_cocd == "SSPL")
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_Inv_SPL", frm_rptName, dsRep, "Invoice Entry Report", "N");
                        }
                        else
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_Inv_SWRN", "std_Inv_SWRN", dsRep, "Invoice Entry Report", "N");
                        }
                    }
                    #endregion
                }
                else
                {
                    #region INV
                    //scode = "004506794431/03/2019";
                    scode = scode.Replace(";", "");
                    frm_mbr = scode.Substring(0, 2);
                    frm_vty = scode.Substring(2, 2);
                    if (scode.Length > 20)
                    {
                        sname = scode.Substring(4, 6);
                        if (scode.Length > 20)
                            sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                        else sname = "'" + sname + "'" + " and " + "'" + sname + "'";

                        if (iconID == "F1033")
                        {
                            frm_vty = "4F";
                            scode = "a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4F%' AND TRIM(a.vchnum) BETWEEN " + sname + " AND A.VCHDATE  " + xprdRange + " ";
                        }
                        else scode = "a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' AND A.TYPE!='4F' AND TRIM(a.vchnum) BETWEEN " + sname + " AND A.VCHDATE  " + xprdRange + " ";
                    }
                    else scode = "a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')='" + scode + "'";

                    string tcsrate = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(params,0) as params from controls where id='D38'", "params");
                    prpdt = fgen.seek_iname(frm_qstr, frm_cocd, "select ENABLE_YN AS prepdt  from controls where id='O13'", "prepdt");
                    rmvdt = fgen.seek_iname(frm_qstr, frm_cocd, "select ENABLE_YN AS rmvdt  from controls where id='O14'", "rmvdt");
                    blogo_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select ENABLE_YN AS rmvdt from controls where id='M87'", "rmvdt");
                    if (blogo_opt == "0" || blogo_opt == "-") blogo_opt = "N";
                    string skipinvoice = "";
                    // skip invoice condition
                    switch (frm_cocd)
                    {
                        case "BONY":
                        case "DLJM":
                        case "SEPL":
                        case "PGEL":
                        case "SDM":
                        case "SFAB":
                            if (frm_ulvl != "0")
                            {
                                DataTable dtskipin = new DataTable();
                                dtskipin = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR FROM DSC_INFO A WHERE " + scode + " ");
                                for (int i = 0; i < dtskipin.Rows.Count; i++)
                                    skipinvoice = "," + "'" + dtskipin.Rows[i][0].ToString().Trim() + "'";
                                if (skipinvoice != "")
                                    skipinvoice = " AND a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') NOT IN (" + skipinvoice.TrimStart(',') + ")";
                            }
                            break;
                    }
                    // query for invoice print
                    switch (frm_cocd)
                    {
                        case "CRP":
                        case "ATOP":
                        case "PIPL":
                        case "SAIL":                        
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno,B.TELNUM,a.branchcd,a.cess_pu,B.TELNUM,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "JSGI":
                        case "SKYP":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno,B.TELNUM,a.branchcd,a.cess_pu,B.TELNUM,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3||' TIN : '||TRIM(b.rc_num) as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "VCL":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,B.TELNUM,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,B.TELNUM,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "MINV":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Receiver' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,B.TELNUM,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,A.IQTY_CHL,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by a.vchnum,vchdate,a.MORDER";//,a.morder,a.icode
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Receiver' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddrR3,'Contact No.'||b.person||' /M '||b.mobile||' T:'||B.TELNUM||' TIN:'||b.rc_num AS caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,B.TELNUM,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,A.IQTY_CHL,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by a.vchnum,vchdate,a.MORDER";//,a.morder,a.icode
                            break;
                        case "SFAB":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,B.TELNUM,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.9999') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "BONY":///////===============BONY ME SALE ME DESP_FROM FIELD AGAR BLANK NAHI HAI TO ALAG RPT CALL HOGI WARNA GST_INV_BC rpt CALL HOGI.....                        
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.9999') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddrR3,b.addr3||' TIN:'||b.rc_num AS caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.9999') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "IPP":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "DLJM":
                        case "SEPL":
                        case "UKB":
                        case "ALIN":
                        case "LRFP":
                        case "KESR":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,'-' as destcount,0 as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno,B.TELNUM, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,'-' as mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,substr(c.drv_name,1,25) as drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,0 as tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.iqtyout<=0 then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,C.tcsamt,0 as acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p ,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,'-' as destcount,0 as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr4 as caddr4,b.addr3||' PIN:'||B.PINCODE AS caddr3,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno,B.TELNUM, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,'-' as mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,substr(c.drv_name,1,25) as drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,0 as tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.iqtyout<=0 then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,C.tcsamt,0 as acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p ,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "PGEL":
                            //SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,'-' as destcount,0 as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno,B.TELNUM, b.rc_num as tinno,b.exc_num as pcstno,B.PINCODE AS CPINCODE, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,'-' as mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,substr(c.drv_name,1,25) as drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,0 as tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.iqtyout<=0 then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,C.tcsamt,0 as acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p ,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,'-' as destcount,0 as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno,B.TELNUM, b.rc_num as tinno,b.exc_num as pcstno,B.PINCODE AS CPINCODE, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,'-' as mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,substr(c.drv_name,1,25) as drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,0 as tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.iqtyout<=0 then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,C.tcsamt,0 as acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p ,nvl(C.full_invno,'-') as full_invno,B.CUST_BANK,K.RTG_BANK,K.RTG_aCTY,K.RTG_IFSC,K.RTG_ACNO,K.RTG_aDDR from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' LEFT OUTER JOIN FAMST K ON TRIM(B.CUST_BANK)=TRIM(K.ACODE) where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,'-' as destcount,0 as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,B.addr3||' PIN:'||B.PINCODE as  CADDR3,  b.addr3 as caddrR3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno,B.TELNUM, b.rc_num as tinno,b.exc_num as pcstno,B.PINCODE AS CPINCODE, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,'-' as mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,substr(c.drv_name,1,25) as drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,0 as tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.iqtyout<=0 then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,C.tcsamt,0 as acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p ,nvl(C.full_invno,'-') as full_invno,B.CUST_BANK,K.RTG_BANK,K.RTG_aCTY,K.RTG_IFSC,K.RTG_ACNO,K.RTG_aDDR from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' LEFT OUTER JOIN FAMST K ON TRIM(B.CUST_BANK)=TRIM(K.ACODE) where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "SFL1":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'" + prpdt + "' as prepdt,A.IWEIGHT, A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,B.TELNUM,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,'-' AS acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,round((a.irate- round((a.irate*a.ichgs)/100,2)),2) as netrate,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "SFL2":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'" + prpdt + "' as prepdt,A.IWEIGHT, A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,B.TELNUM,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,round((a.irate- round((a.irate*a.ichgs)/100,2)),2) as netrate,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "SFLG":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'" + prpdt + "' as prepdt,A.IWEIGHT, A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,B.TELNUM,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,round((a.irate- round((a.irate*a.ichgs)/100,2)),2) as netrate,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "MPI":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Receiver' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,B.TELNUM,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,'-' as plant,'-' as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "STUD":
                            if (frm_vty == "4F")
                                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,A.MORDER,'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect,a.iweight,b.payment,nvl(a.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,f.telnum as tpt_telnum,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,h.invno AS Hinvno,TO_CHAR(h.invdate,'DD/MM/YYYY') AS Hinvdate,h.ship2,h.ship3,h.ship4,h.ship5,h.lbnetwt,h.REMARK3 AS NETWT,h.lbgrswt,h.exprmk1,h.exprmk2,h.exprmk3,h.exprmk4,h.exprmk5,h.addl1,h.addl2,h.addl3,h.addl4,h.addl5,h.tmaddl1,h.tmaddl2,h.tmaddl3,h.addl6,nvl(C.full_invno,'-') as full_invno from ivoucher a left join hundi h on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h.branchcd)||trim(h.acode)||trim(h.invno)||to_char(h.invdate,'dd/mm/yyyy'),sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            else
                                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,B.COUNTRY,d.packsize,f.telnum as tpt_telnum,nvl(a.et_paid,0) as et_paid,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a,sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "WPPL":
                            //pkgsum formula in rpt==>tonumber(prepcur.pkg)=====
                            //running total in rpt==> doing sum of(@pkgsum)====
                            //if({#pkgsum1})<0 then true else false====this is hide/view condtion on running total field in rpt
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,d.prt_nm1,d.prt_nm2,d.prt_nm3,d.prt_nm4,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno,rank() over(order by a.iamount desc) as rank, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno,Is_number(substr(a.naration,1,2)) as conv,a.naration as pkg1, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,d.prt_nm1,d.prt_nm2,d.prt_nm3,d.prt_nm4,nvl(C.full_invno,'-') as full_invno,(case when nvl(d.prt_nm1,'-')!='-' then d.prt_nm1 else '' end)||chr(13)||(case when nvl(d.prt_nm2,'-')!='-' then d.prt_nm2 else '' end)||chr(13)||(case when nvl(d.prt_nm3,'-')!='-' then d.prt_nm3 else '' end)||chr(13)||(case when nvl(d.prt_nm4,'-')!='-' then d.prt_nm4 else '' end) as wppl_name  from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + "  and a.iamount>0  order by vchdate,a.vchnum,a.MORDER";
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno,rank() over(order by a.iamount desc) as rank, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,d.packsize,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno,Is_number(substr(a.naration,1,2)) as conv,a.naration as pkg1, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,d.prt_nm1,d.prt_nm2,d.prt_nm3,d.prt_nm4,nvl(C.full_invno,'-') as full_invno,(case when nvl(d.prt_nm1,'-')!='-' then d.prt_nm1 else '' end)||chr(13)||(case when nvl(d.prt_nm2,'-')!='-' then d.prt_nm2 else '' end)||chr(13)||(case when nvl(d.prt_nm3,'-')!='-' then d.prt_nm3 else '' end)||chr(13)||(case when nvl(d.prt_nm4,'-')!='-' then d.prt_nm4 else '' end) as wppl_name  from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + "  and a.iamount>0  order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "VPAC":
                        case "GIPL":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno,rank() over(order by a.iamount desc) as rank, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,d.packsize,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno,Is_number(substr(a.naration,1,2)) as conv,a.naration as pkg1, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,'-' AS prt_nm1,'-' AS prt_nm2,'-' as prt_nm3,'-' as prt_nm4,nvl(C.full_invno,'-') as full_invno,'-' as wppl_name  from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + "  and a.iamount>0  order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "KLAS":
                            //SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,(case when a.type='4H' then d.iname WHEN TRIM(NVL(A.EXC_57F4,'-'))!='-' THEN TRIM(NVL(A.EXC_57F4,'-')) else d.ciname end) as ciname,(case when a.type='4H' then d.iname else d.cpartno end) as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.type='4H' then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            if (frm_vty == "4F") SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,(case when a.type='4H' then d.iname WHEN TRIM(NVL(A.purpose,'-'))!='-' THEN TRIM(NVL(A.purpose,'-')) else d.ciname end) as ciname,(case when a.type='4H' then d.iname else d.cpartno end) as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,(case when a.type='4F' then h.exprmk5 else c.destin end) as destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.type='4H' then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,B.country,F.VEN_CODE AS VENCODE,(case when a.type='4F' THEN 'N/A' ELSE t.type1 END) AS type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno,A.acpt_ud,C.insp_Amt,C.curren from ivoucher a left join hundi h on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h.branchcd)||trim(h.acode)||trim(h.invno)||to_char(h.invdate,'dd/mm/yyyy') left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            else SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,(case when a.type='4H' then d.iname WHEN TRIM(NVL(A.purpose,'-'))!='-' THEN TRIM(NVL(A.purpose,'-')) else d.ciname end) as ciname,(case when a.type='4H' then d.iname else d.cpartno end) as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.type='4H' then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,B.country,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno,A.acpt_ud,C.insp_Amt,C.curren from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        case "SDM":
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,'-' as destcount,0 as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno,B.TELNUM, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,'-' as mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,substr(c.drv_name,1,25) as drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,0 as tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.iqtyout<=0 then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,B.PINCODE,b.staten,(case when nvl(F.VEN_CODE,'-')!='-' then F.VEN_CODE ELSE B.VENCODE END) AS VENCODE,t.type1,t1.name,C.tcsamt,0 as acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p ,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                        default:
                            SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt, A.MORDER, 'N' as logo_yn,'" + prpdt + "' as prepdt, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,B.TELNUM,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,A.IQTY_CHL,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,nvl(C.full_invno,'-') as full_invno,round((a.irate- round((a.irate*a.ichgs)/100,2)),2) as netrate from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            if ((frm_cocd == "YTEC" && (frm_vty != "4F" || frm_vty != "4P")))
                                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,c.amt_extexc as tool_val,a.type,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,to_char(a.podate,'Mon yyyy') as po_month,'-' as destcount,0 as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno,B.TELNUM, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,'-' as mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,substr(c.drv_name,1,25) as drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,0 as tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,(case when a.iqtyout<=0 then a.iqty_chl else a.iqtyout end) as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,C.tcsamt,0 as acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p ,nvl(C.full_invno,'-') as full_invno from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " order by vchdate,a.vchnum,a.MORDER";
                            if (frm_cocd == "YTEC" && (frm_vty == "4F" || frm_vty == "4P"))
                                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,A.MORDER,'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect,a.iweight,b.payment,nvl(a.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,a.iqty_chlwt,c.exc_not_no,c.no_bdls,C.EXC_57F4 AS SHIPPING_MARK,C.DLV_TERMS ,c.mode_tpt,c.mo_vehi,c.insur_no,c.insp_amt,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,nvl(c.retention,0) as retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,f.telnum as tpt_telnum,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,h.invno AS Hinvno,TO_CHAR(h.invdate,'DD/MM/YYYY') AS Hinvdate,h.ship2,h.ship3,h.ship4,h.ship5,h.lbnetwt,h.REMARK3 AS NETWT,h.lbgrswt,h.exprmk1,h.exprmk2,h.exprmk3,h.exprmk4,h.exprmk5,h.addl1,h.addl2,h.addl3,h.addl4,h.addl5,h.tmaddl1,h.tmaddl2,h.tmaddl3,h.addl6,nvl(C.full_invno,'-') as full_invno from ivoucher a left join hundi h on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h.branchcd)||trim(h.acode)||trim(h.invno)||to_char(h.invdate,'dd/mm/yyyy'),sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " order by vchdate,a.vchnum,a.MORDER";
                            break;
                    }


                    string yr = ""; mq11 = "";
                    mq4 = barCode.Substring(0, 6);
                    string CURR = frm_cDt1.Substring(8, 2);
                    int nxt = Convert.ToInt32(CURR) + 1;

                    if (frm_vty == "4F")
                    {
                        reportActionCode = "1030";
                        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + reportActionCode.Replace("F", "") + "' ", "ACREF");
                    }
                    dsRep = new DataSet();
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (frm_cocd == "WPPL")
                    {//fryt item should not coming on challan print....so rmv 59 series item from subreprt_dt
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY') AS FSTR,'" + repCount + "' AS REPCOUNT,'" + blogo_opt + "' as blogo_opt,'Same as Buyer' as text,'" + prpdt + "' as prepdt,'" + rmvdt + "' as rmvdt, A.MORDER, 'N' as logo_yn,A.BINNO as polineitemno,rank() over(order by a.iamount desc) as rank, a.branchcd,a.cess_pu,substr(b.aname,1,5) as party_,(case when  nvl(trim(b.pname),'-')='-' then b.aname else b.pname end) as partyy,b.pname as full_party_name,a.unit as unloading_point,a.mfgdt as plant,a.expdt as line_item,NVL(c.DESP_FROM,'-') as despfrom,g.ADDR1 as gadr1,g.ADDR2 as gadr2,g.ADDR3 as gadr3,g.RC_NUM AS gTINNO,g.PINCODE as gpincode,g.MOBILE as gmob,g.EMAIL as gemail,g.WEBSITE as gwebsite,g.staten as gstate,c.amt_extexc as tool_val,a.type,d.siname as item_name,d.ciname,d.cpartno as dpartno,nvl(d.iweight,0) as iwt,d.maker as color,d.packsize,to_char(a.podate,'Mon yyyy') as po_month,nvl(c.destcount,'-') as destcount,is_number(c.destcount) as destcount1,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,to_char(A.exc_amt,'99,99,99,999.99') as exc_amt1,a.vchnum,a.o_deptt,TO_CHAR(a.exc_rate,'99,99,99,999.99') AS exc_rate1,a.exc_rate,to_char(a.vchdate,'yyyyMMdd') AS vchd1,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,B.TELNUM,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno,Is_number(substr(a.naration,1,2)) as conv,a.naration as pkg1, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,b.staffcd,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,to_char(c.bill_qty,'99,99,99,999.999') as  bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname,a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,TO_CHAR(a.irate,'99,99,99,999.99') AS IRATE1,A.IRATE,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,TO_char(c.bill_tot,'99,99,99,999.99') as bill_tot1,a.exc_RATE as cgst,a.exc_amt as cgst_val, TO_CHAR(a.cess_percent,'99,99,99,999.99') as sgst1,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,F.VEN_CODE AS VENCODE,t.type1,t1.name,(case when is_number(" + tcsrate + ") > 0 then " + tcsrate + " else b.CESSRATE end) as tcsrate,C.tcsamt,c.acvdrt,C.AMDNO,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,d.prt_nm1,d.prt_nm2,d.prt_nm3,d.prt_nm4,nvl(C.full_invno,'-') as full_invno,(case when nvl(d.prt_nm1,'-')!='-' then d.prt_nm1 else '' end)||chr(13)||(case when nvl(d.prt_nm2,'-')!='-' then d.prt_nm2 else '' end)||chr(13)||(case when nvl(d.prt_nm3,'-')!='-' then d.prt_nm3 else '' end)||chr(13)||(case when nvl(d.prt_nm4,'-')!='-' then d.prt_nm4 else '' end) as wppl_name  from ivoucher a left outer join famstbal f on trim(a.acode)=trim(f.acode) and f.branchcd='" + frm_mbr + "' ,sale c left join csmst er on trim(c.cscode)=trim(er.acode) LEFT join famst g on trim(c.desp_from)=trim(g.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND " + scode + " " + skipinvoice + " and substr(trim(a.icode),1,2)!='59' order by vchdate,a.vchnum,a.MORDER";
                    }
                    if (frm_cocd == "VPAC" || frm_cocd == "GIPL" || frm_cocd == "WPPL")
                    {
                        Wp_dt = new DataTable();
                        Wp_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    }

                    //string einv_qrcode_to_add = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from stock where id='einv_qrcode_to_add' ", "enable_yn");
                    //if (einv_qrcode_to_add == "Y")
                    string einv_qrcode_to_add = "";
                    {
                        dt.Columns.Add("OM_QR");
                        dt.Columns.Add("OM_OM");
                        dt.Columns.Add("EINV_NO");
                        einv_qrcode_to_add = "N";
                        foreach (DataRow drin in dt.Rows)
                        {
                            dt8 = new DataTable();
                            dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT a.BRANCHCD||a.DOC_TYPE||TRIM(a.DOC_NO)||TO_CHAr(a.DOC_DT,'DDMMYYYY') AS FSTR,a.irnqr_1||(case when length(trim(nvl(a.irnqr_2,'-'))) > 1 then trim(a.irnqr_2) else '' end) as qrval,a.irn_no FROM EINV_REC a WHERE a.BRANCHCD||a.DOC_TYPE||TRIM(a.DOC_NO)||TO_CHAr(a.DOC_DT,'DDMMYYYY')='" + drin["fstr"].ToString().Trim() + "' ");

                            if (dt8.Rows.Count > 0)
                            {
                                einv_qrcode_to_add = "Y";
                                drin["OM_QR"] = dt8.Rows[0]["qrval"];
                                drin["OM_OM"] = "-";
                                drin["EINV_NO"] = dt8.Rows[0]["irn_no"];
                            }
                        }
                    }

                    mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no from type where id='B' and type1='" + frm_mbr + "'", "gst_no");

                    #region=============CURSOR FOR MINV
                    if (frm_cocd == "MINV")
                    {
                        dt4 = new DataTable(); //here dt ko blnk isley kiya qki niche dt hi pass hogi to minv ke case me dt me hi fill kiya h data dt4 se
                        dt4 = dt;
                        dt = new DataTable();
                        if (dt4.Rows.Count > 0)
                        {
                            dt = dt4.Clone();
                            dr1 = dt.NewRow();
                            DataView view1im = new DataView(dt4);
                            DataTable dtdrsim = new DataTable();
                            dtdrsim = view1im.ToTable(true, "fstr", "icode"); //MAIN ...old                                               
                            foreach (DataRow dr0 in dtdrsim.Rows)
                            {
                                DataView viewim = new DataView(dt4, "icode='" + dr0["icode"] + "' and fstr='" + dr0["fstr"] + "'", "morder", DataViewRowState.CurrentRows); //old
                                // DataView viewim = new DataView(dt4, "icode='" + dr0["icode"] + "'","", DataViewRowState.CurrentRows);
                                dr1 = dt.NewRow();
                                dt1 = new DataTable();
                                dt1 = viewim.ToTable();
                                mq1 = "";
                                mq1 = ""; mq2 = ""; mq3 = ""; mq4 = "";
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0;
                                db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0; db22 = 0; db23 = 0; db24 = 0; db25 = 0; db26 = 0; db27 = 0;
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    #region
                                    dr1["fstr"] = dt1.Rows[i]["fstr"].ToString().Trim();
                                    dr1["blogo_opt"] = dt1.Rows[i]["blogo_opt"].ToString().Trim();
                                    dr1["prepdt"] = dt1.Rows[i]["prepdt"].ToString().Trim();
                                    dr1["rmvdt"] = dt1.Rows[i]["rmvdt"].ToString().Trim();
                                    dr1["morder"] = dt1.Rows[i]["morder"].ToString().Trim();
                                    dr1["text"] = dt1.Rows[i]["text"].ToString().Trim();
                                    dr1["logo_yn"] = dt1.Rows[i]["logo_yn"].ToString().Trim();
                                    mq1 += "," + dt1.Rows[i]["polineitemno"].ToString().Trim();
                                    dr1["polineitemno"] = mq1.TrimStart(',');
                                    dr1["branchcd"] = dt1.Rows[i]["branchcd"].ToString().Trim();
                                    dr1["cess_pu"] = dt1.Rows[i]["cess_pu"].ToString().Trim();//rate
                                    dr1["party_"] = dt1.Rows[i]["party_"].ToString().Trim();
                                    dr1["partyy"] = dt1.Rows[i]["partyy"].ToString().Trim();
                                    dr1["full_party_name"] = dt1.Rows[i]["full_party_name"].ToString().Trim();
                                    dr1["unloading_point"] = dt1.Rows[i]["unloading_point"].ToString().Trim();
                                    dr1["plant"] = dt1.Rows[i]["plant"].ToString().Trim();
                                    dr1["line_item"] = dt1.Rows[i]["line_item"].ToString().Trim();
                                    db += fgen.make_double(dt1.Rows[i]["tool_val"].ToString().Trim());
                                    dr1["tool_val"] = db;
                                    dr1["type"] = dt1.Rows[i]["type"].ToString().Trim();
                                    dr1["ciname"] = dt1.Rows[i]["ciname"].ToString().Trim();
                                    dr1["dpartno"] = dt1.Rows[i]["dpartno"].ToString().Trim();
                                    db1 += fgen.make_double(dt1.Rows[i]["iwt"].ToString().Trim());////need to ask...pic max or sum...
                                    dr1["iwt"] = db1;
                                    dr1["po_month"] = dt1.Rows[i]["po_month"].ToString().Trim();
                                    dr1["destcount"] = dt1.Rows[i]["destcount"].ToString().Trim();
                                    dr1["destcount1"] = dt1.Rows[i]["destcount1"].ToString().Trim();
                                    dr1["po"] = dt1.Rows[i]["po"].ToString().Trim();
                                    dr1["mrp"] = fgen.make_double(dt1.Rows[i]["mrp"].ToString().Trim());
                                    dr1["finvno"] = dt1.Rows[i]["finvno"].ToString().Trim();
                                    dr1["exc_57f4"] = dt1.Rows[i]["exc_57f4"].ToString().Trim();
                                    db2 += fgen.make_double(dt1.Rows[i]["iexc_Addl"].ToString().Trim());
                                    dr1["iexc_Addl"] = db2;
                                    db3 = fgen.make_double(dt1.Rows[i]["exc_amt"].ToString().Trim());
                                    dr1["exc_amt"] = db3;
                                    dr1["vchnum"] = dt1.Rows[i]["vchnum"].ToString().Trim();
                                    mq2 += "," + dt1.Rows[0]["o_deptt"].ToString().Trim();
                                    dr1["o_deptt"] = mq2.TrimStart(',');
                                    dr1["exc_rate"] = fgen.make_double(dt1.Rows[i]["exc_rate"].ToString().Trim());
                                    dr1["vchd1"] = dt1.Rows[i]["vchd1"].ToString().Trim();
                                    dr1["vchdate"] = dt1.Rows[i]["vchdate"].ToString().Trim();
                                    dr1["acode"] = dt1.Rows[i]["acode"].ToString().Trim();
                                    dr1["chlnum"] = dt1.Rows[i]["chlnum"].ToString().Trim();
                                    dr1["sd_val"] = dt1.Rows[i]["sd_val"].ToString().Trim();
                                    dr1["chldate"] = dt1.Rows[i]["chldate"].ToString().Trim();
                                    dr1["caddr1"] = dt1.Rows[i]["caddr1"].ToString().Trim();
                                    dr1["caddr2"] = dt1.Rows[i]["caddr2"].ToString().Trim();
                                    dr1["caddr3"] = dt1.Rows[i]["caddr3"].ToString().Trim();
                                    dr1["caddr4"] = dt1.Rows[i]["caddr4"].ToString().Trim();
                                    dr1["ctel"] = dt1.Rows[i]["ctel"].ToString().Trim();
                                    dr1["dlno"] = dt1.Rows[i]["dlno"].ToString().Trim();
                                    dr1["cperson"] = dt1.Rows[i]["cperson"].ToString().Trim();
                                    dr1["TELNUM"] = dt1.Rows[i]["TELNUM"].ToString().Trim();
                                    dr1["cstno"] = dt1.Rows[i]["cstno"].ToString().Trim();
                                    dr1["tinno"] = dt1.Rows[i]["tinno"].ToString().Trim();
                                    dr1["pcstno"] = dt1.Rows[i]["pcstno"].ToString().Trim();
                                    dr1["pono"] = dt1.Rows[i]["pono"].ToString().Trim();
                                    dr1["podate"] = dt1.Rows[i]["podate"].ToString().Trim();
                                    dr1["exc_not_no"] = dt1.Rows[i]["exc_not_no"].ToString().Trim();
                                    dr1["no_bdls"] = dt1.Rows[i]["no_bdls"].ToString().Trim();
                                    dr1["mode_tpt"] = dt1.Rows[i]["mode_tpt"].ToString().Trim();
                                    dr1["mo_vehi"] = dt1.Rows[i]["mo_vehi"].ToString().Trim();
                                    dr1["insur_no"] = dt1.Rows[i]["insur_no"].ToString().Trim();
                                    dr1["st_entform"] = dt1.Rows[i]["st_entform"].ToString().Trim();
                                    dr1["ins_cert"] = dt1.Rows[i]["ins_cert"].ToString().Trim();
                                    dr1["grno"] = dt1.Rows[i]["grno"].ToString().Trim();
                                    dr1["staffcd"] = dt1.Rows[i]["staffcd"].ToString().Trim();
                                    dr1["stform_no"] = dt1.Rows[i]["stform_no"].ToString().Trim();
                                    dr1["mcomment"] = dt1.Rows[i]["mcomment"].ToString().Trim();
                                    dr1["remvdate"] = dt1.Rows[i]["remvdate"].ToString().Trim();
                                    dr1["remvtime"] = dt1.Rows[i]["remvtime"].ToString().Trim();
                                    db4 += fgen.make_double(dt1.Rows[i]["bill_qty"].ToString().Trim());
                                    dr1["bill_qty"] = db4;
                                    dr1["naration"] = dt1.Rows[i]["naration"].ToString().Trim();
                                    dr1["st_type"] = dt1.Rows[i]["st_type"].ToString().Trim();
                                    dr1["st_rate"] = dt1.Rows[i]["st_rate"].ToString().Trim();//rate
                                    dr1["drv_name"] = dt1.Rows[i]["drv_name"].ToString().Trim();
                                    dr1["drv_mobile"] = dt1.Rows[i]["drv_mobile"].ToString().Trim();
                                    dr1["freight"] = dt1.Rows[i]["freight"].ToString().Trim();
                                    db5 += fgen.make_double(dt1.Rows[i]["weight"].ToString().Trim());
                                    dr1["weight"] = db5;
                                    dr1["invtime"] = dt1.Rows[i]["invtime"].ToString().Trim();
                                    dr1["st31_form"] = dt1.Rows[i]["st31_form"].ToString().Trim();
                                    dr1["ins_co"] = dt1.Rows[i]["ins_co"].ToString().Trim();
                                    dr1["grdate"] = dt1.Rows[i]["grdate"].ToString().Trim();
                                    dr1["stform_dt"] = dt1.Rows[i]["stform_dt"].ToString().Trim();
                                    dr1["cscode"] = dt1.Rows[i]["cscode"].ToString().Trim();
                                    db6 += fgen.make_double(dt1.Rows[i]["act_tpt_amt"].ToString().Trim());
                                    dr1["act_tpt_amt"] = db6;
                                    dr1["ins_no"] = dt1.Rows[i]["ins_no"].ToString().Trim();
                                    dr1["destin"] = dt1.Rows[i]["destin"].ToString().Trim();
                                    dr1["pack_rate"] = dt1.Rows[i]["pack_rate"].ToString().Trim();///rate
                                    dr1["tptbill_no"] = dt1.Rows[i]["tptbill_no"].ToString().Trim();
                                    dr1["sta_rate"] = dt1.Rows[i]["sta_rate"].ToString().Trim();//rate
                                    db7 += fgen.make_double(dt1.Rows[i]["sta_amt"].ToString().Trim());
                                    dr1["sta_amt"] = db7;
                                    db8 += fgen.make_double(dt1.Rows[i]["totdisc_amt"].ToString().Trim());
                                    dr1["totdisc_amt"] = db8;
                                    db9 += fgen.make_double(dt1.Rows[i]["tsubs_amt"].ToString().Trim());
                                    dr1["tsubs_amt"] = db9;
                                    db10 += fgen.make_double(dt1.Rows[i]["retention"].ToString().Trim());
                                    dr1["retention"] = db10;
                                    db11 = fgen.make_double(dt1.Rows[i]["bill_tot"].ToString().Trim());///fdfffff
                                    dr1["bill_tot"] = db11;
                                    db12 += fgen.make_double(dt1.Rows[i]["amt_sttt"].ToString().Trim());
                                    dr1["amt_sttt"] = db12;
                                    db13 += fgen.make_double(dt1.Rows[i]["amt_stsc"].ToString().Trim());
                                    dr1["amt_stsc"] = db13;
                                    db14 = fgen.make_double(dt1.Rows[i]["amt_sale"].ToString().Trim()); ///fffdfd
                                    dr1["amt_sale"] = db14;
                                    db15 = fgen.make_double(dt1.Rows[i]["amt_exc"].ToString().Trim());////fff
                                    dr1["amt_exc"] = db15;
                                    db16 += fgen.make_double(dt1.Rows[i]["rvalue"].ToString().Trim());
                                    dr1["rvalue"] = db16;
                                    db17 += fgen.make_double(dt1.Rows[i]["amt_job"].ToString().Trim());
                                    dr1["amt_job"] = db17;
                                    db18 += fgen.make_double(dt1.Rows[i]["st_amt"].ToString().Trim());
                                    dr1["st_amt"] = db18;
                                    db19 += fgen.make_double(dt1.Rows[i]["amt_rea"].ToString().Trim());
                                    dr1["amt_rea"] = db19;
                                    dr1["aname"] = dt1.Rows[i]["aname"].ToString().Trim();
                                    dr1["location"] = dt1.Rows[i]["location"].ToString().Trim();
                                    dr1["srno"] = dt1.Rows[i]["srno"].ToString().Trim();
                                    dr1["icode"] = dt1.Rows[i]["icode"].ToString().Trim();
                                    dr1["iname"] = dt1.Rows[i]["iname"].ToString().Trim();
                                    dr1["cpartno"] = dt1.Rows[i]["cpartno"].ToString().Trim();
                                    dr1["irate"] = fgen.make_double(dt1.Rows[i]["irate"].ToString().Trim());//item rate
                                    dr1["cdrgno"] = dt1.Rows[i]["cdrgno"].ToString().Trim();
                                    dr1["pordno"] = dt1.Rows[i]["pordno"].ToString().Trim();
                                    dr1["ordno"] = dt1.Rows[i]["ordno"].ToString().Trim();
                                    dr1["orddt"] = dt1.Rows[i]["orddt"].ToString().Trim();
                                    dr1["nsp_flag"] = dt1.Rows[i]["nsp_flag"].ToString().Trim();
                                    db20 += fgen.make_double(dt1.Rows[i]["bal"].ToString().Trim());
                                    dr1["bal"] = db20;
                                    dr1["cdisc"] = fgen.make_double(dt1.Rows[i]["cdisc"].ToString().Trim());
                                    db21 += fgen.make_double(dt1.Rows[i]["iamount"].ToString().Trim());
                                    dr1["iamount"] = db21;
                                    db22 += fgen.make_double(dt1.Rows[i]["qty"].ToString().Trim());
                                    dr1["qty"] = db22;
                                    dr1["desc_"] = dt1.Rows[i]["desc_"].ToString().Trim();
                                    dr1["strt"] = dt1.Rows[i]["strt"].ToString().Trim();
                                    dr1["stcd"] = dt1.Rows[i]["stcd"].ToString().Trim();
                                    dr1["stk"] = fgen.make_double(dt1.Rows[i]["stk"].ToString().Trim());
                                    db23 += fgen.make_double(dt1.Rows[i]["pkg"].ToString().Trim());
                                    dr1["pkg"] = db23;
                                    dr1["ent_by"] = dt1.Rows[i]["ent_by"].ToString().Trim();
                                    dr1["ent_dt"] = dt1.Rows[i]["ent_dt"].ToString().Trim();
                                    mq3 += "," + dt1.Rows[i]["btchno"].ToString().Trim();
                                    dr1["btchno"] = mq3.TrimStart(',');
                                    dr1["expdt"] = dt1.Rows[i]["expdt"].ToString().Trim();
                                    dr1["mfgdt"] = dt1.Rows[i]["mfgdt"].ToString().Trim();
                                    dr1["unit"] = dt1.Rows[i]["unit"].ToString().Trim();
                                    dr1["cgst"] = fgen.make_double(dt1.Rows[i]["cgst"].ToString().Trim());//cgst rate
                                    db24 += fgen.make_double(dt1.Rows[i]["cgst_val"].ToString().Trim());
                                    dr1["cgst_val"] = db24;
                                    dr1["sgst"] = fgen.make_double(dt1.Rows[i]["sgst"].ToString().Trim());//sgst rate
                                    db25 += fgen.make_double(dt1.Rows[i]["sgst_val"].ToString().Trim());
                                    dr1["sgst_val"] = db25;
                                    dr1["iopr"] = dt1.Rows[i]["iopr"].ToString().Trim();
                                    dr1["hscode"] = dt1.Rows[i]["hscode"].ToString().Trim();
                                    dr1["cgst_no"] = dt1.Rows[i]["cgst_no"].ToString().Trim();
                                    dr1["girno"] = dt1.Rows[i]["girno"].ToString().Trim();
                                    dr1["staten"] = dt1.Rows[i]["staten"].ToString().Trim();
                                    dr1["VENCODE"] = dt1.Rows[i]["VENCODE"].ToString().Trim();
                                    dr1["type1"] = dt1.Rows[i]["type1"].ToString().Trim();
                                    dr1["name"] = dt1.Rows[i]["name"].ToString().Trim();
                                    dr1["tcsrate"] = fgen.make_double(dt1.Rows[i]["tcsrate"].ToString().Trim());
                                    db26 = fgen.make_double(dt1.Rows[i]["tcsamt"].ToString().Trim()); //fdfdf
                                    dr1["tcsamt"] = db26;
                                    db27 += fgen.make_double(dt1.Rows[i]["acvdrt"].ToString().Trim());
                                    dr1["acvdrt"] = db27;
                                    dr1["AMDNO"] = dt1.Rows[i]["AMDNO"].ToString().Trim();
                                    dr1["doc_tot"] = dt1.Rows[i]["doc_tot"].ToString().Trim();
                                    dr1["consign_p"] = dt1.Rows[i]["consign_p"].ToString().Trim();
                                    dr1["daddr1_p"] = dt1.Rows[i]["daddr1_p"].ToString().Trim();
                                    dr1["daddr2_p"] = dt1.Rows[i]["daddr2_p"].ToString().Trim();
                                    dr1["daddr3_p"] = dt1.Rows[i]["daddr3_p"].ToString().Trim();
                                    dr1["daddr4_p"] = dt1.Rows[i]["daddr4_p"].ToString().Trim();
                                    dr1["dtel_p"] = dt1.Rows[i]["dtel_p"].ToString().Trim();
                                    dr1["dtinno_p"] = dt1.Rows[i]["dtinno_p"].ToString().Trim();
                                    dr1["dcstno_p"] = dt1.Rows[i]["dcstno_p"].ToString().Trim();
                                    dr1["mycode_p"] = dt1.Rows[i]["mycode_p"].ToString().Trim();
                                    dr1["dstaten_p"] = dt1.Rows[i]["dstaten_p"].ToString().Trim();
                                    dr1["dgst_no_p"] = dt1.Rows[i]["dgst_no_p"].ToString().Trim();
                                    dr1["dpanno_p"] = dt1.Rows[i]["dpanno_p"].ToString().Trim();
                                    dr1["dstatecode_p"] = dt1.Rows[i]["dstatecode_p"].ToString().Trim();
                                    #endregion
                                }
                                dt.Rows.Add(dr1);
                            }
                        }
                    }
                    #endregion

                    #region for UKB (concatenate finvno in variable and after that update into dt by variable)
                    if (frm_cocd == "UKB")
                    {
                        dt4 = new DataTable();
                        dt4 = dt;
                        mq1 = "";
                        if (dt4.Rows.Count > 0)
                        {
                            dr1 = dt.NewRow();
                            DataView view1im = new DataView(dt4);
                            DataTable dtdrsim = new DataTable();
                            dtdrsim = view1im.ToTable(true, "FINVNO"); //MAIN                  
                            foreach (DataRow dr0 in dtdrsim.Rows)
                            {
                                DataView viewim = new DataView(dt4, "FINVNO='" + dr0["FINVNO"].ToString().Trim() + "'", "morder", DataViewRowState.CurrentRows);
                                dt1 = new DataTable();
                                dt1 = viewim.ToTable();
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    mq1 += "," + dt1.Rows[i]["finvno"].ToString().Trim();       //concatenate in mq1 var                         
                                }
                            }
                        }
                        mq2 = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        { //loop for update value in each row
                            mq2 = mq1.TrimStart(',');
                            if (mq2.ToString().Length > 74)
                            {
                                dt.Rows[i]["finvno"] = mq2.Substring(0, 66);
                            }
                            else
                            {
                                dt.Rows[i]["finvno"] = mq2;
                            }
                        }
                    }
                    #endregion

                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                        if (frm_vty == "4F")
                        {
                            dt.Columns.Add("EXP_YR", typeof(string));
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (frm_cocd == "SFLG")
                            {
                                mq0 = System.Text.RegularExpressions.Regex.Match(dr["pkg"].ToString(), @"\d+").Value;
                                dr["pkgN"] = fgen.make_double(mq0);
                            }
                            else
                            {
                                dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                            }
                            if (frm_vty == "4F")
                            {
                                yr = nxt.ToString();
                                yr = "EXP/" + dr["vchnum"].ToString().Trim().Substring(2, 4) + "/" + CURR + "-" + yr + "";
                                dr["EXP_YR"] = yr;
                            }
                        }
                        dt.TableName = "Prepcur";
                        repCount = 4;
                        // report copy function
                        switch (frm_cocd)
                        {
                            case "CRP":
                            case "LRFP":
                            case "KLAS":
                                repCount = 3;
                                break;
                            case "SAIP":
                            case "SAIL":
                            case "MINV":
                            case "IPP":
                            case "WING":
                            case "AEPL":
                            case "ALIN":
                            case "VPAC":
                                repCount = 5;
                                break;
                            case "MLGI":
                                repCount = 1;
                                break;
                        }
                        if (frm_vty == "4F")
                        {
                            if (frm_cocd != "ELEC")
                                repCount = 1;
                            switch (frm_cocd)
                            {
                                case "WING":
                                    repCount = 5;
                                    break;
                                case "KLAS":
                                    repCount = 3;
                                    break;
                                case "ATOP":
                                case "SKYP":
                                    repCount = 4;
                                    break;
                                default:
                                    repCount = 1;
                                    break;
                            }
                        }
                        string mq14 = ""; mq11 = ""; mq12 = ""; mq13 = ""; string PartyName = "";
                        #region Barcode
                        DataTable dtBarCode = new DataTable();
                        DataTable mainDt = new DataTable();
                        DataView distBarCode = new DataView(dt);
                        mainDt = dt;
                        dtBarCode = distBarCode.ToTable(true, "FSTR");
                        //if (frm_cocd == "PPAP")
                        //dt1 = new DataTable("barcode");
                        mainDt.Columns.Add(new DataColumn("img1_desc", typeof(string)));
                        mainDt.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
                        mainDt.Columns.Add(new DataColumn("img2_desc", typeof(string)));
                        mainDt.Columns.Add(new DataColumn("img2", typeof(System.Byte[])));
                        mainDt.Columns.Add(new DataColumn("img3_desc", typeof(string)));
                        mainDt.Columns.Add(new DataColumn("img3", typeof(System.Byte[])));
                        mainDt.Columns.Add(new DataColumn("img4_desc", typeof(string)));
                        mainDt.Columns.Add(new DataColumn("img4", typeof(System.Byte[])));
                        mainDt.Columns.Add(new DataColumn("irn_qr_desc", typeof(string)));
                        mainDt.Columns.Add(new DataColumn("irn_qr_img", typeof(System.Byte[])));
                        // generating barcode, qr code
                        foreach (DataRow drBarcode in dtBarCode.Rows)
                        {
                            dt = new DataTable();
                            DataView distDt = new DataView(mainDt, "FSTR='" + drBarcode["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            if (distDt.Count > 0)
                                dt = distDt.ToTable();

                            // setting barcode value
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                invoiceBarcodeImage = dt.Rows[i]["fstr"].ToString().Trim();
                                switch (frm_cocd)
                                {
                                    case "VCL":
                                        col1 = dt.Rows[i]["fstr"].ToString().Trim();
                                        break;
                                    case "DLJM":
                                    case "SEPL":
                                    case "SDM":
                                    case "UKB":
                                    case "WPPL":
                                    case "VPAC":
                                    case "PGEL":
                                        col1 = dt.Rows[i]["branchcd"].ToString().Trim() + dt.Rows[i]["type"].ToString().Trim() + dt.Rows[i]["vchnum"].ToString().Trim() + dt.Rows[i]["vchdate"].ToString().Trim();
                                        break;
                                    case "PPAP":
                                    case "ADZO":
                                        #region
                                        if (i > 0)
                                        {
                                            col2 = col2 + (char)13;
                                        }
                                        col2 = col2 + "AC"
                                               + dt.Rows[i]["finvno"].ToString().Trim().Replace("/", "")
                                               + dt.Rows[i]["exc_57f4"].ToString().Trim().Replace("/", "")
                                               + (char)13
                                               + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "")
                                               + (char)9
                                               + Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString()).ToString("ddMMyyyy").Trim().Replace("/", "")
                                               + dt.Rows[i]["qty"].ToString().Trim().Replace("/", "")
                                               + (char)9
                                               + Convert.ToDecimal(fgen.make_double(fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim().Replace("/", ""), 2) + fgen.make_double(dt.Rows[i]["exc_amt"].ToString().Trim().Replace("/", "")) + fgen.make_double(dt.Rows[i]["cess_pu"].ToString().Trim().Replace("/", "")), 2)).ToString("0.00")
                                               + (char)9
                                               + "8708.99.00"
                                               + "0.00"
                                               + (char)9
                                               + Convert.ToDecimal(fgen.make_double(dt.Rows[i]["cess_pu"].ToString().Trim().Replace("/", ""))).ToString("0.00")
                                               + (char)9;

                                        if (dt.Rows[i]["iopr"].ToString().Trim().Replace("/", "").ToUpper().Equals("IG"))
                                        {
                                            col2 = col2 + Convert.ToDecimal(fgen.make_double(dt.Rows[i]["exc_amt"].ToString().Trim().Replace("/", ""))).ToString("0.00");
                                        }
                                        else col2 = col2 + "0.00";

                                        col2 = col2 + (char)9
                                            + "0.00"
                                            + (char)9
                                             + Convert.ToDecimal(fgen.make_double(dt.Rows[i]["irate"].ToString().Trim().Replace("/", ""), 2)).ToString("0.00")
                                              + (char)9
                                              + Convert.ToDecimal(fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim().Replace("/", ""), 2)).ToString("0.00")
                                              + (char)9;

                                        if (dt.Rows[i]["iopr"].ToString().Trim().Replace("/", "").ToUpper().Equals("CG"))
                                        {
                                            col2 = col2 + Convert.ToDecimal(fgen.make_double(dt.Rows[i]["exc_amt"].ToString().Trim().Replace("/", ""), 2)).ToString("0.00");
                                        }
                                        else col2 = col2 + "0.00";

                                        col2 = col2 + (char)9
                                             + "0.00"
                                             + (char)9
                                               + "0.00"
                                             + (char)9
                                           + Convert.ToDecimal(fgen.make_double(fgen.make_double(dt.Rows[i]["iamount"].ToString()) + (fgen.make_double(dt.Rows[i]["qty"].ToString()) * fgen.make_double(dt.Rows[i]["iexc_addl"].ToString())), 2)).ToString("0.00")
                                                 + (char)9
                                                   + "0.00"
                                             + (char)9
                                             + Convert.ToDecimal(fgen.make_double(dt.Rows[i]["iexc_Addl"].ToString().Trim().Replace("/", ""), 2)).ToString("0.00")
                                              + (char)9
                                                   + "0.00"
                                             + (char)9
                                             + mq10
                                           + (char)9;
                                        #endregion
                                        break;
                                    default:
                                        if (frm_cocd == "SAIP" || (frm_cocd == "IPP" && dt.Rows[0]["party_"].ToString().Trim().Contains("BAJAJ")))
                                        {
                                            mq11 = dt.Rows[i]["DESTCOUNT"].ToString().Trim(); //first barcode value
                                            mq12 = dt.Rows[i]["PONO"].ToString().Trim();//2nd barcode value
                                            mq13 = dt.Rows[i]["EXC_57F4"].ToString().Trim();//3rd barcode value
                                            mq14 = dt.Rows[i]["BILL_TOT"].ToString().Trim();//4th barcode value    
                                            PartyName = dt.Rows[i]["aname"].ToString().Trim();
                                        }
                                        else col1 = dt.Rows[i]["branchcd"].ToString().Trim().Replace("/", "") + "," + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "");
                                        break;
                                }

                                if (einv_qrcode_to_add == "Y")
                                    IrnQrCodeValue = dt.Rows[i]["OM_QR"].ToString().Trim();
                            }

                            // deleting exising barcode file
                            switch (frm_cocd)
                            {
                                case "DLJM":
                                case "SEPL":
                                case "SDM":
                                case "UKB":
                                case "WPPL":
                                case "VPAC":
                                case "PGEL":
                                    fpath = Server.MapPath(@"BarCode\" + invoiceBarcodeImage + ".png");
                                    del_file(fpath);
                                    break;
                                case "VCL":
                                    fpath = Server.MapPath(@"BarCode\" + invoiceBarcodeImage + ".png");
                                    del_file(fpath);
                                    fpath1 = Server.MapPath(@"BarCode\" + invoiceBarcodeImage + "1" + ".png");
                                    del_file(fpath1);
                                    break;
                                default:
                                    if (frm_cocd == "SAIP" || (frm_cocd == "IPP" && dt.Rows[0]["party_"].ToString().Trim().Contains("BAJAJ")))
                                    {
                                        fpath1 = Server.MapPath(@"BarCode\" + mq11.Trim().Replace("*", "").Replace("/", "") + "1" + ".png");
                                        fpath2 = Server.MapPath(@"BarCode\" + mq12.Trim().Replace("*", "").Replace("/", "") + "2" + ".png");
                                        fpath3 = Server.MapPath(@"BarCode\" + mq13.Trim().Replace("*", "").Replace("/", "") + "3" + ".png");
                                        fpath4 = Server.MapPath(@"BarCode\" + mq14.Trim().Replace("*", "").Replace("/", "") + "4" + ".png"); ;
                                        del_file(fpath1);
                                        del_file(fpath2);
                                        del_file(fpath3);
                                        del_file(fpath4);
                                    }
                                    else
                                    {
                                        fpath = Server.MapPath(@"BarCode\" + invoiceBarcodeImage + ".png");
                                        del_file(fpath);
                                    }
                                    break;
                            }

                            // create barcode , qrcode file
                            switch (frm_cocd)
                            {
                                case "PPAP":
                                case "ADZO":
                                    fgen.prnt_QRbar(frm_cocd, col2, invoiceBarcodeImage + ".png");
                                    break;
                                case "AEPL":
                                case "WING":
                                case "SAIL":
                                case "ATOP":
                                case "PIPL":
                                case "BONY":
                                case "IPP":
                                case "SFAB":
                                case "JSGI":
                                    #region barcc
                                    {
                                        col3 = "";
                                        col1 = "";
                                        if (dt.Rows.Count > 0)
                                        {
                                            int i = 0;
                                            col3 = dt.Rows[i]["vchnum"].ToString().Trim();
                                            if (frm_cocd == "SAIL" && dt.Rows[i]["aname"].ToString().Contains("LIFELONG"))
                                            { //for lifelong ...                                      
                                                col1 += "*" + dt.Rows[i]["pono"].ToString().Trim().Split('/')[1] + "|||" + dt.Rows[i]["vchnum"].ToString().Trim() + "|" + dt.Rows[i]["vchd1"].ToString().Trim() + "|" + dt.Rows[i]["mo_vehi"].ToString().Trim() + "|||||" + dt.Rows[i]["bill_tot"].ToString().Trim() + "|||||||";
                                            }
                                            ////////////////FOR BONY
                                            else if ((frm_cocd == "SFAB" || frm_cocd == "BONY" || frm_cocd == "IPP") && dt.Rows[i]["aname"].ToString().Contains("TATA"))
                                            {
                                                string vfullinvno = "";
                                                if (dt.Rows[i]["full_invno"].ToString().Trim().Length <= 6)
                                                { vfullinvno = dt.Rows[i]["vchnum"].ToString().Trim().TrimStart('0'); }
                                                else
                                                { vfullinvno = dt.Rows[i]["full_invno"].ToString().Trim().TrimStart('0'); }
                                                col1 += dt.Rows[i]["pono"].ToString().Trim() + ",10," + dt.Rows[i]["bill_qty"].ToString().Trim() + "," + vfullinvno + "," + dt.Rows[i]["vchdate"].ToString().Trim().Substring(0, 2) + "." + dt.Rows[i]["vchdate"].ToString().Trim().Substring(3, 2) + "." + dt.Rows[i]["vchdate"].ToString().Trim().Substring(6, 4) + "," + dt.Rows[i]["irate1"].ToString().Trim() + "," + dt.Rows[i]["irate1"].ToString().Trim() + "," + dt.Rows[i]["vencode"].ToString().Trim() + "," + dt.Rows[i]["exc_57f4"].ToString().Trim();
                                            }
                                            ////====================
                                            else
                                            {
                                                col1 += dt.Rows[i]["VENCODE"].ToString().Trim() + (char)9 + dt.Rows[i]["pono"].ToString().Trim() + (char)9 + dt.Rows[i]["vchnum"].ToString().Trim() + (char)9 + Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString().Trim()).ToString("dd.MM.yyyy") + (char)9 + mq10 + (char)9 + dt.Rows[i]["BILL_TOT"].ToString().Trim() + (char)9 + dt.Rows[i]["amt_sale"].ToString().Trim() + (char)9 + dt.Rows[i]["mo_vehi"].ToString().Trim();

                                                if (dt.Rows[i]["iopr"].ToString().Trim() == "IG")///real                                    
                                                {
                                                    col1 += (char)9 + "0.00" + (char)9 + dt.Compute("sum(cgst_val)", "").ToString() + (char)9 + "0.00" + (char)9 + dt.Rows[i]["tcsamt"].ToString().Trim();
                                                }
                                                else
                                                {
                                                    col1 += (char)9 + dt.Compute("sum(cgst_val)", "").ToString() + (char)9 + "0.00" + (char)9 + dt.Compute("sum(cgst_val)", "").ToString().Trim() + (char)9 + dt.Rows[i]["tcsamt"].ToString().Trim();
                                                }
                                            }
                                        }
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            if (frm_cocd == "SAIL" && dt.Rows[i]["aname"].ToString().Contains("LIFELONG"))
                                            {
                                                col1 += "~" + dt.Rows[i]["exc_57f4"].ToString().Trim() + "|" + dt.Rows[i]["qty"].ToString().Trim() + "|" + (dt.Rows[i]["qty"].ToString().Trim().toDouble(2) * dt.Rows[i]["iwt"].ToString().Trim().toDouble(2)) + "|" + dt.Rows[i]["irate"].ToString().Trim() + "|" + dt.Rows[i]["hscode"].ToString().Trim() + "|" + dt.Rows[i]["polineitemno"].ToString().Trim();
                                            }
                                            else if (frm_cocd == "SAIL")
                                            {
                                                col1 += (char)9 + dt.Rows[i]["exc_57f4"].ToString().Trim() + (char)9 + dt.Rows[i]["hscode"].ToString().Trim() + (char)9 + dt.Rows[i]["qty"].ToString().Trim() + (char)9 + dt.Rows[i]["irate"].ToString().Trim();
                                            }
                                            //////////////////// change for bony
                                            else if ((frm_cocd == "SFAB" || frm_cocd == "BONY" || frm_cocd == "IPP") && dt.Rows[i]["aname"].ToString().Contains("TATA"))
                                            {
                                                if (dt.Rows[i]["iopr"].ToString().Trim() == "IG")
                                                {
                                                    col1 += ",0.00,0.00," + dt.Rows[i]["exc_amt1"].ToString().Trim().Replace(",", "") + "," + "0.00";
                                                }
                                                else
                                                {
                                                    col1 += "," + dt.Rows[i]["exc_amt"].ToString().Trim().Replace(",", "") + "," + dt.Rows[i]["cess_pu"].ToString().Trim().Replace(",", "") + ",0.00," + "0.00";
                                                }
                                                //=============
                                                if (dt.Rows[i]["iopr"].ToString().Trim() == "IG")
                                                {
                                                    col1 += ",0.00,0.00," + dt.Rows[i]["exc_rate1"].ToString().Trim().Replace(",", "") + "," + "0.00" + ",0.00";
                                                }
                                                else
                                                {
                                                    col1 += "," + dt.Rows[i]["exc_rate1"].ToString().Trim().Replace(",", "") + "," + dt.Rows[i]["SGST1"].ToString().Trim().Replace(",", "") + ",0.00," + "0.00," + "0.00";
                                                }
                                                col1 += "," + dt.Rows[i]["bill_tot1"].ToString().Trim().Replace(",", "") + "," + dt.Rows[i]["hscode"].ToString().Trim();
                                            }
                                            //============================
                                            else
                                            {
                                                col1 += (char)9 + dt.Rows[i]["EXC_57F4"].ToString().Trim() + (char)9 + dt.Rows[i]["hscode"].ToString().Trim() + (char)9 + dt.Rows[i]["qty"].ToString().Trim() + (char)9 + dt.Rows[i]["irate"].ToString().Trim();
                                            }
                                        }
                                        if (frm_cocd == "SAIL" && dt.Rows[0]["aname"].ToString().Contains("DELLORTO"))
                                        {
                                            col3 = dt.Rows[0]["vchnum"].ToString().Trim();
                                            col1 = dt.Rows[0]["vencode"].ToString().Trim() + "," + dt.Rows[0]["pono"].ToString().Trim() + "," + dt.Rows[0]["vchnum"].ToString().Trim() + "," + "," + Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("ddMMyyyy") + "," + dt.Rows[0]["bill_tot"].ToString().Trim() + "," + dt.Rows[0]["exc_57f4"].ToString().Trim() + "," + dt.Rows[0]["qty"].ToString().Trim();
                                            fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + ".png");
                                            fgen.prnt_QRbar(frm_cocd, col1, col3 + ".png");//for testing   
                                        }
                                        ////////////CHANGE===========BONY ME HERO PE 2D BARCODE AAYEGA ONLY.....BAKI SAB PAR QR CODE AAYEGA
                                        else if (frm_cocd == "BONY" || frm_cocd == "SFAB" || frm_cocd == "JSGI" || frm_cocd == "IPP")
                                        {
                                            if (dt.Rows[0]["aname"].ToString().Contains("TATA"))
                                            {
                                                col3 = dt.Rows[0]["vchnum"].ToString().Trim();
                                                fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + ".png");
                                                fgen.prnt_QRbar(frm_cocd, col1, col3 + ".png");
                                            }
                                            else if (dt.Rows[0]["aname"].ToString().Contains("HERO"))
                                            {
                                                if (frm_cocd == "BONY" || frm_cocd == "SFAB")
                                                {
                                                    // ********************** commenting old running method, which was used to print 2d on 32 bit system                                                    
                                                    fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + "");
                                                    fgen.prnt_2Dbar32bit(frm_cocd, col1, fpath.Replace(".png", ""));//uncomment                                   
                                                    fpath = fpath.Replace(".png", "") + ".bmp";

                                                }
                                                else
                                                {
                                                    fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + ".png");
                                                    fgen.prnt_2DbarAll(frm_cocd, col1, fpath);
                                                }
                                            }
                                            else
                                            {
                                                col3 = dt.Rows[0]["vchnum"].ToString().Trim();
                                                col1 = dt.Rows[0]["vchnum"].ToString().Trim();
                                                fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + ".png");
                                                fgen.prnt_QRbar(frm_cocd, col1, col3 + ".png");
                                            }
                                        }
                                        ////////=================================                         
                                        else
                                        {
                                            if (frm_cocd == "BONY" || frm_cocd == "SFAB")
                                            {
                                                // ********************** commenting old running method, which was used to print 2d on 32 bit system
                                                //
                                                fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + "");
                                                fgen.prnt_2Dbar32bit(frm_cocd, col1, fpath.Replace(".png", ""));//uncomment                                   
                                                fpath = fpath + ".bmp";
                                            }
                                            else
                                            {                                                
                                                fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + ".png");
                                                fgen.prnt_2DbarAll(frm_cocd, col1, fpath);
                                            }
                                        }

                                        if (frm_cocd == "SAIL" || frm_cocd == "ATOP" || frm_cocd == "PIPL" || frm_cocd == "BONY" || frm_cocd == "SFAB" || frm_cocd == "JSGI")
                                        {
                                            if (frm_cocd == "BONY" && dt.Rows[0]["despfrom"].ToString().Trim() != "-")
                                            {
                                                frm_rptName = "gst_inv_bcdesp"; //this rpt for bony..open only when then desp_from field is not blank...else gst_inv_bc called
                                            }
                                            else
                                            {
                                                frm_rptName = "gst_inv_bc";//
                                            }
                                        }
                                        else if (frm_cocd == "IPP" && PartyName.Contains("BAJAJ"))
                                        {
                                            fgen.prnt_Code128bar(frm_cocd, mq11, mq11.Replace("/", "") + "1" + ".png");
                                            fgen.prnt_Code128bar(frm_cocd, mq12, mq12.Replace("/", "") + "2" + ".png");
                                            fgen.prnt_Code128bar(frm_cocd, mq13, mq13.Replace("/", "") + "3" + ".png");
                                            fgen.prnt_Code128bar(frm_cocd, mq14, mq14.Replace("/", "") + "4" + ".png");
                                        }
                                        else
                                        {
                                            frm_rptName = "std_inv2d";
                                        }
                                    }
                                    #endregion
                                    break;
                                case "SFLG":
                                case "SFL2":
                                    #region
                                    col3 = "";
                                    col1 = "";
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        col3 = dt.Rows[i]["vchnum"].ToString().Trim();
                                        col1 += dt.Rows[i]["pono"].ToString().Trim() + ",10," + dt.Rows[i]["bill_Qty"].ToString().Trim() + "," + dt.Rows[i]["full_invno"].ToString().Trim() + "," + Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString().Trim()).ToString("dd.MM.yyyy") + "," + dt.Rows[i]["irate"].ToString().Trim() + "," + (dt.Rows[i]["iamount"].ToString().Trim().toDouble() / dt.Rows[i]["qty"].ToString().Trim().toDouble()).toDouble(2) + "," + dt.Rows[i]["vencode"].ToString().Trim() + "," + dt.Rows[i]["exc_57f4"].ToString().Trim();

                                        if (dt.Rows[i]["iopr"].ToString().Trim() == "CG") col1 += "," + dt.Rows[i]["cgst_val"].ToString().Trim().toDouble(2);
                                        else col1 += "," + "0.00";
                                        if (dt.Rows[i]["iopr"].ToString().Trim() == "CG") col1 += "," + dt.Rows[i]["sgst_val"].ToString().Trim().toDouble(2);
                                        else col1 += "," + "0.00";
                                        if (dt.Rows[i]["iopr"].ToString().Trim() == "IG") col1 += "," + dt.Rows[i]["cgst_val"].ToString().Trim().toDouble(2);
                                        else col1 += "," + "0.00";
                                        col1 += "," + "0.00";

                                        if (dt.Rows[i]["iopr"].ToString().Trim() == "CG") col1 += "," + dt.Rows[i]["cgst"].ToString().Trim().toDouble(2).ToString("f");
                                        else col1 += "," + "0.00";
                                        if (dt.Rows[i]["iopr"].ToString().Trim() == "CG") col1 += "," + dt.Rows[i]["sgst_val"].ToString().Trim().toDouble(2).ToString("f");
                                        else col1 += "," + "0.00";
                                        if (dt.Rows[i]["iopr"].ToString().Trim() == "IG") col1 += "," + dt.Rows[i]["cgst"].ToString().Trim().toDouble(2).ToString("f");
                                        else col1 += "," + "0.00";

                                        col1 += "," + "0.00";

                                        col1 += "," + "0.00" + "," + dt.Rows[i]["bill_tot"].ToString().Trim() + "," + dt.Rows[i]["hscode"].ToString().Trim();
                                    }

                                    fpath = Server.MapPath(@"~\tej-base\BarCode\" + col3 + "" + ".png");
                                    del_file(fpath);
                                    fgen.prnt_QRbar(frm_cocd, col1, col3 + ".png");
                                    #endregion
                                    break;
                                case "DLJM":
                                case "SEPL":
                                case "SDM":
                                case "UKB":
                                case "WPPL":
                                case "VPAC":
                                case "PGEL":
                                    fgen.prnt_Code128bar(frm_cocd, col1, invoiceBarcodeImage + ".png");
                                    break;
                                case "VCL":
                                    fgen.prnt_QRbar(frm_cocd, mq12, invoiceBarcodeImage + "1" + ".png");//om_qr code for last page
                                    break;
                                default:
                                    if (frm_cocd == "SAIP" || (frm_cocd == "IPP" && PartyName.Contains("BAJAJ")))
                                    {
                                        fgen.prnt_Code128bar(frm_cocd, mq11, mq11.Replace("/", "") + "1" + ".png");
                                        fgen.prnt_Code128bar(frm_cocd, mq12, mq12.Replace("/", "") + "2" + ".png");
                                        fgen.prnt_Code128bar(frm_cocd, mq13, mq13.Replace("/", "") + "3" + ".png");
                                        fgen.prnt_Code128bar(frm_cocd, mq14, mq14.Replace("/", "") + "4" + ".png");
                                    }
                                    else fgen.prnt_QRbar(frm_cocd, col1, invoiceBarcodeImage + ".png");
                                    break;
                            }
                            if (einv_qrcode_to_add == "Y")
                                fgen.prnt_QRbar(frm_cocd, IrnQrCodeValue, invoiceBarcodeImage + "_IRN.png");

                            //DataRow dr = dt1.NewRow();


                            foreach (DataRow dr in mainDt.Rows)
                            {
                                if (dr["FSTR"].ToString().Trim() == drBarcode["FSTR"].ToString().Trim())
                                {
                                    // inserting barcode , qrcode file to main table
                                    switch (frm_cocd)
                                    {
                                        case "VCL":
                                            FilStr = new FileStream(fpath1, FileMode.Open);
                                            BinRed = new BinaryReader(FilStr);
                                            dr["img2_desc"] = mq12.Trim();
                                            dr["img2"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                            break;
                                        default:
                                            if (frm_cocd == "SAIP" || (frm_cocd == "IPP" && PartyName.Contains("BAJAJ")))
                                            {
                                                #region
                                                FilStr = new FileStream(fpath1, FileMode.Open);
                                                BinRed = new BinaryReader(FilStr);
                                                FilStr1 = new FileStream(fpath2, FileMode.Open);
                                                BinRed1 = new BinaryReader(FilStr1);
                                                FilStr2 = new FileStream(fpath3, FileMode.Open);
                                                BinRed2 = new BinaryReader(FilStr2);
                                                FilStr3 = new FileStream(fpath4, FileMode.Open);
                                                BinRed3 = new BinaryReader(FilStr3);

                                                dr["img1_desc"] = mq11.Trim();
                                                dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                                FilStr.Dispose();
                                                dr["img2_desc"] = mq12.Trim();
                                                dr["img2"] = BinRed1.ReadBytes((int)BinRed1.BaseStream.Length);
                                                FilStr1.Dispose();
                                                dr["img3_desc"] = mq13.Trim();
                                                dr["img3"] = BinRed2.ReadBytes((int)BinRed2.BaseStream.Length);
                                                FilStr2.Dispose();
                                                dr["img4_desc"] = mq14.Trim();
                                                dr["img4"] = BinRed3.ReadBytes((int)BinRed3.BaseStream.Length);
                                                FilStr3.Dispose();
                                                #endregion
                                            }
                                            else
                                            {
                                                FilStr = new FileStream(fpath, FileMode.Open);
                                                BinRed = new BinaryReader(FilStr);
                                                dr["img1_desc"] = col1.Trim();
                                                dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                            }
                                            break;
                                    }

                                    dr["FSTR"] = drBarcode["fstr"].ToString().Trim();

                                    FilStr.Close();
                                    BinRed.Close();

                                    if (einv_qrcode_to_add == "Y")
                                    {
                                        fpath4_irn = Server.MapPath(@"BarCode\" + invoiceBarcodeImage + "_IRN.png");
                                        FilStr4 = new FileStream(fpath4_irn, FileMode.Open);
                                        BinRed4 = new BinaryReader(FilStr4);
                                        dr["irn_qr_desc"] = IrnQrCodeValue.Trim();
                                        dr["irn_qr_img"] = BinRed4.ReadBytes((int)BinRed4.BaseStream.Length);
                                        FilStr4.Dispose();
                                        BinRed4.Dispose();
                                    }

                                    if (frm_cocd == "SAIP" || (frm_cocd == "IPP" && PartyName.Contains("BAJAJ")))
                                    {
                                        FilStr1.Close();
                                        BinRed1.Close();
                                        FilStr2.Close();
                                        BinRed2.Close();
                                        FilStr3.Close();
                                        BinRed3.Close();
                                    }
                                }
                            }
                        }
                        //dsRep.Tables.Add(dt1);
                        //dt1.Dispose();                        
                        #endregion
                        dsRep.Tables.Add(fgen.mTitle(frm_cocd, mainDt, repCount));
                        //csmst                
                        SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count <= 0)
                        {
                            dt = new DataTable();
                            SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        }
                        dt.TableName = "csmst";
                        dsRep.Tables.Add(dt);
                        // inv terms
                        SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='INV' ORDER BY SRNO";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        mq10 = "";
                        dt1 = new DataTable();
                        mdr = null;
                        dt1.Columns.Add("poterms", typeof(string));
                        if (frm_cocd == "STUD")
                        {
                            dt1.Columns.Add("poterms1", typeof(string));
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                            if (frm_cocd == "STUD")
                            {
                                if (i > 2)
                                {
                                    mq11 += dt.Rows[i]["POTERMS"].ToString().Trim();//+ Environment.NewLine
                                }
                            }
                        }
                        mdr = dt1.NewRow();
                        mdr["poterms"] = mq10;
                        if (frm_cocd == "STUD")
                        {
                            mdr["poterms1"] = mq11;
                        }
                        dt1.Rows.Add(mdr);
                        dt1.TableName = "INV_TERMS";
                        dsRep.Tables.Add(dt1);

                        // invoice rpt name

                        switch (frm_cocd)
                        {
                            case "ELEC":
                            case "MLGI":
                            case "SAIP":
                                frm_rptName = "std_inv_DSC";
                                break;
                            case "STUD":
                                if (frm_vty == "44") frm_rptName = "std_inv_stud44";
                                else if (frm_vty == "43") frm_rptName = "std_inv_stud43";
                                else if (frm_vty == "4F") frm_rptName = "EXPINV_STUD";
                                else frm_rptName = "std_inv_stud_all";
                                break;
                            case "LRFP":
                                if (frm_vty == "41") frm_rptName = "std_LFRPe2_g";
                                else frm_rptName = "std_LFRP2_g";
                                break;
                            case "SDM":
                                frm_rptName = "gst_inv_bc_SDM";
                                break;
                        }

                        //printing invoice
                        switch (frm_cocd)
                        {
                            case "SFLG":
                            case "SFL2":
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_sflg", "std_inv_sflg", dsRep, "Invoice Entry Report", "Y");
                                break;
                            case "DLJM":
                            case "SEPL":
                            case "SDM":
                            case "UKB":
                            case "MINV":
                            case "CRP":
                            case "ALIN":
                            case "PGEL":
                                frm_rptName = "gst_inv_bc";
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                                break;
                            case "VCL":
                                frm_rptName = "gst_inv_vcl";
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                                break;
                            case "SAIP":
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_SAIP_INV", "std_SAIP_INV", dsRep, "Invoice Entry Report", "Y");
                                break;
                            case "IPP":
                                if (PartyName.Contains("BAJAJ")) Print_Report_BYDS(frm_cocd, frm_mbr, "std_SAIP_INV", "std_SAIP_INV", dsRep, "Invoice Entry Report", "Y");
                                else Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", "gst_inv_bc", dsRep, "Invoice Entry Report", "Y");
                                break;
                            case "YTEC":
                                if (frm_vty == "4F" || frm_vty == "4P")
                                {
                                    frm_rptName = "ExpInv_YTEC";
                                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_EXP", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                                }
                                else
                                {
                                    frm_rptName = "gst_inv_bc";
                                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                                }
                                break;
                            case "VPAC":
                                Wp_dt.TableName = "subrpt_dt";
                                dsRep.Tables.Add(Wp_dt);
                                Print_Report_BYDS(frm_cocd, frm_mbr, "gst_inv_wp", "gst_inv_vpac", dsRep, "Invoice Entry Report", "Y"); //real                                
                                break;
                            case "GIPL":
                                Wp_dt.TableName = "subrpt_dt";
                                dsRep.Tables.Add(Wp_dt);
                                Print_Report_BYDS(frm_cocd, frm_mbr, "gst_inv_wp", "gst_inv_GIPL", dsRep, "Invoice Entry Report", "Y"); //real                                
                                break;
                            case "WPPL":
                                Wp_dt.TableName = "subrpt_dt";
                                dsRep.Tables.Add(Wp_dt);
                                Print_Report_BYDS(frm_cocd, frm_mbr, "gst_inv_wp", "gst_inv_wp", dsRep, "Invoice Entry Report", "Y");
                                break;
                            case "KLAS":
                                Print_Report_BYDS(frm_cocd, frm_mbr, "gst_inv_rx", "gst_inv_rx", dsRep, "Invoice Entry Report", "Y");
                                break;
                            default:
                                if (frm_vty == "4F" && frm_cocd != "YTEC")
                                {
                                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_EXP", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                                }
                                else
                                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                                break;
                        }
                    }
                    #endregion
                }
                break;
            //CHL
            //CHL
            case "F1007":
                if (frm_cocd == "SSPL" || frm_cocd == "SWRN")
                {
                    #region
                    frm_mbr = scode.Substring(0, 2);
                    frm_vty = scode.Substring(2, 2);
                    sname = scode.Substring(4, 6);
                    sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                    mq0 = ""; mq1 = ""; mq10 = "";
                    mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ENABLE_YN FROM CONTROLS WHERE ID='B23'", "ENABLE_YN");
                    if (mq10 == "Y")
                    {
                        // NAME OF SIGNATORY
                        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT params FROM CONTROLS WHERE ID='B23'", "params");
                    }
                    mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ENABLE_YN FROM CONTROLS WHERE ID='B24'", "ENABLE_YN");
                    if (mq10 == "Y")
                    {
                        // DESIGNATION OF SIGNATORY
                        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT params FROM CONTROLS WHERE ID='B24'", "params");
                    }
                    if (frm_vty == "21")
                    {
                        #region JobWork Challan
                        dsRep = new DataSet();
                        SQuery = "SELECT 'DELIVERY CHALLAN FOR JOBWORK' as header,A.MODE_TPT,A.DESC_,nvl(a.post,0) as post,A.FREIGHT,a.rcode,B.HSCODE,A.RTN_DATE,c.EMAIL AS CEMAIL,substr(trim(c.gst_no),1,2) as statecode,c.country,c.WEBSITE AS CWEBSITE,C.STATEN AS CSTATEN,A.THRU,D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM,C.GST_NO AS CGSTNO ,C.RC_NUM AS PARTY_TIN,C.GIRNO AS CGIRNO,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,B.CDRGNO,C.VENCODE,(case when nvl(A.PURPOSE,'-')!='-' then a.purpose else a.desc_ end) as purpose,(case when nvl(a.no_bdls,'-')='-' then '0' else a.no_bdls end) as no_bdls,A.TPT_NAMES,'" + mq0 + "' as sign_name,'" + mq1 + "' as sign_desig,b.maker FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND D.ID='M' AND NVL(A.IQTYOUT,0)>0 ORDER BY A.ICODE";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            dt.Columns.Add(new DataColumn("NO_BDLS_N", typeof(double)));
                            foreach (DataRow dr in dt.Rows)
                            {
                                dr["NO_BDLS_N"] = fgen.make_double(fgen.getNumericOnly(dr["NO_BDLS"].ToString()));
                            }
                            dt.TableName = "Prepcur";
                            repCount = 5;
                            dsRep.Tables.Add(fgen.mTitle3(dt, repCount));

                            //csmst
                            SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["rcode"].ToString().Trim() + "'";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count <= 0)
                            {
                                dt = new DataTable();
                                SQuery = "Select 'SAME AS PROCESSOR' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            }
                            dt.TableName = "csmst";
                            dsRep.Tables.Add(dt);
                            if (frm_cocd == "SSPL")
                            {
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_Challan_SPL", frm_rptName, dsRep, "Challan Report");
                            }
                            else
                            {
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_Challan_SWRN", frm_rptName, dsRep, "Challan Report");
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region Delivery Challan
                        dsRep = new DataSet();
                        SQuery = "SELECT 'DELIVERY CHALLAN' as header,A.MODE_TPT,A.DESC_,nvl(a.post,0) as post,A.FREIGHT,a.rcode,B.HSCODE,A.RTN_DATE,c.EMAIL AS CEMAIL,substr(trim(c.gst_no),1,2) as statecode,c.country,c.WEBSITE AS CWEBSITE,C.STATEN AS CSTATEN,A.THRU,D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM,C.GST_NO AS CGSTNO ,C.RC_NUM AS PARTY_TIN,C.GIRNO AS CGIRNO,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,B.CDRGNO,C.VENCODE,(case when nvl(a.no_bdls,'-')='-' then '0' else a.no_bdls end) as no_bdls,A.TPT_NAMES,'" + mq0 + "' as sign_name,'" + mq1 + "' as sign_desig,b.maker FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND D.ID='M' AND NVL(A.IQTYOUT,0)>0 ORDER BY A.ICODE";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            dt.Columns.Add(new DataColumn("NO_BDLS_N", typeof(double)));
                            foreach (DataRow dr in dt.Rows)
                            {
                                dr["NO_BDLS_N"] = fgen.make_double(fgen.getNumericOnly(dr["NO_BDLS"].ToString()));
                            }
                            dt.TableName = "Prepcur";
                            repCount = 5;
                            dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                            //csmst
                            SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["rcode"].ToString().Trim() + "'";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count <= 0)
                            {
                                dt = new DataTable();
                                SQuery = "Select 'SAME AS RECIPIENT' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            }
                            dt.TableName = "csmst";
                            dsRep.Tables.Add(dt);
                            if (frm_cocd == "SSPL")
                            {
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_Del_Chl_SPL", "std_Del_Chl_SPL", dsRep, "Challan Report");
                            }
                            else
                            {
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_Del_Chl_SWRN", "std_Del_Chl_SWRN", dsRep, "Challan Report");
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region CHL
                    frm_mbr = scode.Substring(0, 2);
                    frm_vty = scode.Substring(2, 2);
                    sname = scode.Substring(4, 6);
                    if (scode.Length > 20)
                        sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                    else sname = "'" + sname + "'" + " and " + "'" + sname + "'";

                    //if (frm_cocd == "STUD")
                    {
                        ////SQuery = "SELECT A.RTN_DATE,c.EMAIL AS CEMAIL,c.WEBSITE AS CWEBSITE,C.STATEN AS CSTATEN,A.THRU,D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM,C.GST_NO AS CGSTNO ,C.RC_NUM AS PARTY_TIN,C.GIRNO AS CGIRNO,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND D.ID='M' ORDER BY A.ICODE";
                        ////SQuery = "SELECT A.RTN_DATE,c.EMAIL AS CEMAIL,c.WEBSITE AS CWEBSITE,C.STATEN AS CSTATEN,A.THRU,D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM,c.staten,C.GST_NO AS CGSTNO ,C.RC_NUM AS PARTY_TIN,C.GIRNO AS CGIRNO,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL,a.exc_amt as cgst_val,a.cess_pu as sgst_val,a.post FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND D.ID='M' ORDER BY A.ICODE";
                        ////dt = new DataTable();
                        ////dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        ////dt.TableName = "Prepcur";
                        ////dsRep = new DataSet();
                        ////dsRep.Tables.Add(dt);
                        ////mq0 = "SELECT B.INAME,B.UNIT AS UNIT2,B.CPARTNO, A.BRANCHCD AS MBR,A.TYPE AS BTYPE,A.VCHNUM AS BVCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BVCHDATE1,A.ICODE AS BICODE,A.ACODE AS BACODE,A.IQTYOUT AS BQTY,A.IQTY_WT AS WT_REC FROM RGPMST A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " ORDER BY b.cpartno";
                        ////dt1 = new DataTable();
                        ////dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        ////dt1.TableName = "FAMST";
                        ////dsRep.Tables.Add(dt1);
                        ////frm_rptName = "std_Challan_basic";

                        string skipinvoice = "";

                        if (frm_ulvl != "0" && (frm_cocd == "BONY" || frm_cocd == "DLJM" || frm_cocd == "SEPL"))
                        {
                            DataTable dtskipin = new DataTable();
                            dtskipin = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR FROM DSC_INFO A WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " ");
                            for (int i = 0; i < dtskipin.Rows.Count; i++)
                            {
                                skipinvoice = "," + "'" + dtskipin.Rows[i][0].ToString().Trim() + "'";
                            }
                            if (skipinvoice != "")
                            {
                                skipinvoice = " AND a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') NOT IN (" + skipinvoice.TrimStart(',') + ")";
                            }
                        }

                        SQuery = "SELECT D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND D.ID='M' ORDER BY A.ICODE";
                        SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,D.NAME,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,'-' AS btoprint,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO as mo_vehi,B.INAME,B.UNIT,B.CPARTNO AS APART,b.hscode,C.ANAME,C.ADDR1 as caddr1,C.ADDR2 as caddr2,C.ADDR3 as caddr3,C.ADDR4 as caddr4,c.staten,t.type1,c.gst_no as cgst_no,c.girno,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A.approxval AS IAMT,a.post,A.IRATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS remvtime,a.thru as ins_no,a.rtn_date as remv_date ,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL,a.exc_rate as cgst,a.exc_amt as cgst_val,a.cess_pu as sgst_val,a.cess_percent as sgst,a.cess_pu+a.exc_amt as taxval FROM IVOUCHER A,ITEM B,TYPE D ,FAMST C left join type t on trim(c.staten)=trim(t.name) and t.id='{' WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND D.ID='M' ORDER BY A.ICODE";
                        SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,D.NAME,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,'-' AS btoprint,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO as mo_vehi,B.INAME,B.UNIT,B.CPARTNO AS APART,b.hscode,C.ANAME,C.ADDR1 as caddr1,C.ADDR2 as caddr2,C.ADDR3 as caddr3,C.ADDR4 as caddr4,C.VENCODE,c.staten,t.type1,c.gst_no as cgst_no,c.girno,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A.approxval AS IAMT,a.post,A.IRATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS remvtime,a.thru as ins_no,a.rtn_date as remv_date ,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL,a.exc_rate as cgst,a.exc_amt as cgst_val,a.cess_pu as sgst_val,a.cess_percent as sgst,a.cess_pu+a.exc_amt as taxval FROM IVOUCHER A,ITEM B,TYPE D ,FAMST C left join type t on trim(c.staten)=trim(t.name) and t.id='{' WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND D.ID='M' ORDER BY a.vchnum,a.morder,a.srno,A.ICODE";//ADD VENCODE FOR SAIL
                        dsRep = new DataSet();
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            dt.TableName = "Prepcur";
                            repCount = 3;
                            if (frm_cocd == "SAIL" || frm_cocd == "SAIP") repCount = 4;
                            else if (frm_cocd == "YTEC") repCount = 5;
                            if (frm_cocd == "SAIP") dsRep.Tables.Add(fgen.mTitle4(dt, repCount));
                            else dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                        }
                        frm_rptName = "std_chl_GST";
                    }
                    //else
                    //{
                    //    SQuery = "SELECT D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND D.ID='M' ORDER BY A.ICODE";
                    //    dsRep = new DataSet();
                    //    dt = new DataTable();
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        dt.TableName = "Prepcur";
                    //        dsRep.Tables.Add(dt);
                    //    }

                    //    openPrintAgain(frm_cocd, frm_mbr, "Direct", "F1007A", frm_cDt1, scode);
                    //}
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_chl", frm_rptName, dsRep, "Challan Report", "Y");// earlier without logo fun was called
                    //Thread td = new Thread(() => openPrintAgain(frm_cocd, frm_mbr, "Direct", "F1007A", frm_cDt1, scode));
                    //td.Start();
                    #endregion
                }
                break;

            //CHL2
            case "F1007A":
                #region CHL2
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";

                SQuery = "SELECT B.INAME,B.UNIT AS UNIT2, A.BRANCHCD AS MBR,A.TYPE AS BTYPE,a.vchnum,A.VCHNUM AS BVCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BVCHDATE1,A.ICODE AS BICODE,A.ACODE AS BACODE,A.IQTYOUT AS BQTY,A.IQTY_WT AS WT_REC FROM RGPMST A,ITEM B  WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE)";
                SQuery = "SELECT D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND D.ID='M' ORDER BY A.ICODE";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_chl2", "std_chl2", dsRep, "Challan Report");
                #endregion
                break;

            //ISSUE
            case "F1008":
                #region Issue
                SQuery = "select 'Material Issue Request' as header,'Material Issue Request' as h1,'Issue Agst Job Card' as h2, C.NAME AS DPT_NAME,I.INAME,I.CPARTNO,I.UNIT AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM IVOUCHER A, ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND C.ID='M' AND   trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'  order by A.VCHNUM DESC";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_iss", frm_rptName, dsRep, "Store Issue Report");
                #endregion
                break;
            //RETURN
            case "F1009":
                #region Retrun
                SQuery = "SELECT D.ANAME AS SUPPLIER,E.NAME AS ENAME, c.name,TO_CHAR(A.GEDATE,'DD/MM/YYYY') AS GDATE, B.INAME,B.UNIT AS BUNIT,B.CPARTNO, A.* FROM IVOUCHER A,ITEM B ,TYPE C,FAMST D,TYPE E WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and trim(a.acode)=trim(c.type1) and c.id='M' AND E.ID='M'  AND  trim(a.TYPE)=trim(E.type1) AND TRIM(A.VCODE)=TRIM(D.ACODE) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_ret", frm_rptName, dsRep, "Store Return Report");
                #endregion
                break;
            //INW INSP. TEMP
            case "F1010":
                #region INW INSP. TEMP
                SQuery = "select i.iname,i.unit,i.cpartno as icpart,i.cdrgno,a.* from inspmst a,item i  where  trim(i.icode)=trim(a.icode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN ('" + scode + "') order by srno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_InwardInsTemp", frm_rptName, dsRep, "INW INSP. TEMP");
                }
                #endregion
                break;
            //INW INSP. RPT
            case "F1011":
                #region INW INSP. RPT
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";

                SQuery = "SELECT 'Inward Inspection Report' AS HEADER , F.ANAME,I.INAME,I.CPARTNO AS ICPARTNO,A.* FROM INSPVCH A,FAMST F, ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchnum between " + sname + " and a.vchdate " + xprdRange + " ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_InwardInwReport", frm_rptName, dsRep, "INW INSP. RPT", "Y");
                }
                #endregion
                break;
            //OUT INSP. TEMP
            case "F1012":
                #region OUT INSP. TEMP
                SQuery = "select i.iname,i.unit,i.cdrgno,I.CPARTNO AS ICPARTNO,a.* from inspmst a,item i  where  trim(i.icode)=trim(a.icode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + scode + "') ORDER BY SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_OutwardInsTemplate", frm_rptName, dsRep, "INW INSP. TEMP");
                #endregion
                break;
            //OUT INSP. RPT
            case "F1013":
                #region OUT INSP. RPT
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                SQuery = "SELECT 'Pre Dispatch Inspection Report' AS HEADER , F.ANAME,F.ADDR1 AS FDDR,I.INAME,I.CPARTNO AS ICPARTNO,i.unit as iunit,A.* FROM inspvch A,FAMST F, ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchnum between " + sname + " and a.vchdate " + xprdRange + " ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_OutwardInsReport", frm_rptName, dsRep, "OUT INSP. RPT", "Y");
                }
                #endregion
                break;
            //PURCH SCH
            case "F1014":
                #region PURCH SCH
                SQuery = "select d.mthname, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.* from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum)  AND  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'";
                col1 = "YES";
                if (col1 == "YES")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "' ORDER BY A.ICODE DESC";
                }
                if (col1 == "NO")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'";
                }

                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_po_schedule", frm_rptName, dsRep, "PURCHASE SCHD RPT");
                }
                #endregion
                break;
            //SALE SCH
            case "F1015":
                #region SALE SCH
                col1 = "YES";
                if (col1 == "YES")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) ASDAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "' ORDER BY A.ICODE DESC";  //using round off
                }
                if (col1 == "NO")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'";
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_so_schedule", frm_rptName, dsRep, "SALES SCHD RPT");
                }
                #endregion
                break;
            //P.I
            case "F1016":
                #region P.I
                if (frm_cocd == "KLAS")
                {
                    header_n = "Export Proforma Invoice";
                    SQuery = "SELECT '" + header_n + "' as header, NVL(b.PERSON,'-') AS PERSON,NVL(B.ANAME,'-') AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,B.EMAIL,B.TELNUM,B.PERSON AS kind_Atn,C.SINAME,C.CINAME AS PROD_CODE,C.MAKER AS COLOR,C.unit as bunit,c.hscode,A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR, a.ordno as ord_no,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ord_dt,a.* FROM SOMASQ A left outer join  csmst d on trim(a.cscode)=trim(d.ACODE),FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "' ORDER BY A.srno";//withiut csmst
                    SQuery = "SELECT '" + header_n + "' as header,a.ordno as vchnum,TO_CHAR(A.ORDDT,'DD/MM/YYYY') as vchdate, NVL(b.PERSON,'-') AS PERSON,NVL(B.ANAME,'-') AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,B.EMAIL,B.TELNUM,B.PERSON AS kind_Atn,C.SINAME,C.CINAME AS PROD_CODE,C.MAKER AS COLOR,C.unit as bunit,c.hscode,A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR, a.ordno as ord_no,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ord_dt,a.*,(case when length(trim(er.pname))>3 then er.pname else er.aname end) as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p FROM SOMASQ A left outer join  csmst er on trim(a.cscode)=trim(er.ACODE),FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "' ORDER BY A.srno";
                    //(case when length(trim(er.pname))>3 then er.pname else er.aname end) as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dsRep = new DataSet();
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);
                        frm_rptName = "EXP_PERF_INV_KLAS";//31 TYPE FOR KLAS..THIS IS MATCHED
                        Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "", "Y");
                    }
                }
                else
                {
                    SQuery = "Select  G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'SOMAS' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from somasq a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + scode + "' order by a.ordno";
                    dsRep = new DataSet();
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_pi", frm_rptName, dsRep, "std_pi", "Y");
                    }
                }
                #endregion
                break;
            //M.S.O Print
            case "F1017":
                #region M.S.O Print
                SQuery = "Select  G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'SOMAS' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from SOMASM a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + scode + "' order by a.ordno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_mso", frm_rptName, dsRep, "std_mso", "Y");
                }
                #endregion
                break;
            //Process Plan Print
            case "F1018":
                #region Process Plan Print
                SQuery = "SELECT  B.INAME AS ITEMNAME,B.CDRGNO AS CUST_IT_CODE,C.ANAME AS CUSTOEMR,a.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,A.TITLE as Remarks,A.ACODE,A.ICODE,A.CPARTNO,A.SRNO,A.BTCHNO AS SR,COL1 AS PROCESS,A.COL2 AS SPECIFICATION,A.COL3 AS Reqmt,A.COL4 as RMK, A.COL5 AS ERPCODE,A.COL6 AS UOM,A.COL9 AS COBB_IN,A.COL10 AS FLUTE,A.COL11 AS HEIGHT,A.COL12 AS DIENO,A.COL13 AS TYPE_OF_ITEM,A.COL14 AS CTN_SIZE_OD,A.COL15 as PLy,A.COL16 AS CTN_SIZE_ID,A.COL17,A.COL18 AS Std_Rej_Allow,A.REJQTY  AS UPS,A.REMARK2,REMARK3,REMARK4,A.ENT_BY,TO_cHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,A.APP_BY,A.APP_DT,A.EDT_BY,TO_CHAR(A.EDT_DT,'DD/MM/YYYY') AS EDT_DT,A.AMDCOMMENT AS AMEN1,A.AMDDT AS AMDT1,A.AMDCOMMENT2 AS AMEN2 ,A.AMDDT2,A.AMDCOMMENT3 AS AMEN3,A.AMDDT3,A.AMDCOMMENT4 AS AMEN4,A.AMDDT4,A.AMDCOMMENT5 AS AMEN5,A.AMDDT5,A.AMDNO FROM  INSPMST  A,ITEM B ,FAMST C WHERE A.BRANCHCD='" + frm_mbr + "' AND A .TYPE='70'AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + scode + "'  AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "STD_PPLAN", frm_rptName, dsRep, "STD_PPLAN");
                }
                #endregion
                break;
            //BOM Print
            case "F1019":
                #region BOM Print
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "STD_PRODN", frm_rptName, dsRep, "STD_PRODN");
                }
                #endregion
                break;
            //Job Card Print
            case "F1020":
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                if (scode.Length > 20)
                    sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                else sname = "'" + sname + "'" + " and " + "'" + sname + "'";
                if (frm_cocd == "CRP")
                {
                    #region job card print
                    dt2 = new DataTable();
                    dt2.Columns.Add("col_1", typeof(string));
                    dt2.Columns.Add("col_2", typeof(string));
                    dt2.Columns.Add("col_3", typeof(string));
                    dt2.Columns.Add("col_4", typeof(string));
                    dt2.Columns.Add("col_5", typeof(string));
                    dt2.Columns.Add("col_6", typeof(string));
                    dt2.Columns.Add("col_7", typeof(string));
                    dt2.Columns.Add("col_8", typeof(string));
                    dt2.Columns.Add("col_9", typeof(string));
                    dt2.Columns.Add("col_10", typeof(string));
                    dt2.Columns.Add("col_11", typeof(string));
                    dt2.Columns.Add("col_12", typeof(string));
                    dt2.Columns.Add("total", typeof(double));
                    ////
                    dt2.Columns.Add("ENTBY1", typeof(string));
                    dt2.Columns.Add("ENTDT1", typeof(string));
                    dt2.Columns.Add("vchnum", typeof(string));
                    dt2.Columns.Add("vchdate", typeof(string));
                    dt2.Columns.Add("CONVDATE", typeof(string));
                    dt2.Columns.Add("sotype", typeof(string));
                    dt2.Columns.Add("icode", typeof(string));
                    dt2.Columns.Add("Qty", typeof(string));
                    dt2.Columns.Add("entby2", typeof(string));
                    dt2.Columns.Add("entdt2", typeof(string));
                    dt2.Columns.Add("sheets", typeof(string));
                    dt2.Columns.Add("wstg", typeof(string));
                    dt2.Columns.Add("wt_shet", typeof(double));
                    dt2.Columns.Add("edt_by", typeof(string));
                    dt2.Columns.Add("edt_dt", typeof(string));
                    dt2.Columns.Add("iname", typeof(string));
                    dt2.Columns.Add("cpartno", typeof(string));
                    dt2.Columns.Add("party", typeof(string));
                    dt2.Columns.Add("mkt_rmk", typeof(string));
                    dt2.Columns.Add("app_by", typeof(string));
                    dt2.Columns.Add("prod_type", typeof(string));
                    dt2.Columns.Add("OD", typeof(string));
                    dt2.Columns.Add("PLY", typeof(double));
                    dt2.Columns.Add("ID", typeof(string));
                    dt2.Columns.Add("Corrug", typeof(string));
                    dt2.Columns.Add("UPS", typeof(double));
                    dt2.Columns.Add("DIE", typeof(string));
                    dt2.Columns.Add("FSTR", typeof(string));
                    dt2.Columns.Add("PPRMK", typeof(string));
                    dt2.Columns.Add("SOTOLR", typeof(string));
                    dt2.Columns.Add("REMARKS", typeof(string));
                    dt2.Columns.Add("LIN_MTR", typeof(string));
                    dt2.Columns.Add("CLOSE_RMK", typeof(string));
                    dt2.Columns.Add("ALL_WST", typeof(double));
                    dt2.Columns.Add("cutsize", typeof(double));
                    dt2.Columns.Add("reelsize", typeof(double));
                    dt2.Columns.Add("SHEET_QTY", typeof(double));
                    ///===============new filds  
                    dt2.Columns.Add("spec_no", typeof(string));
                    dt2.Columns.Add("cust_dlvdt", typeof(string));
                    dt2.Columns.Add("ppc_dlvdt", typeof(string));
                    dt2.Columns.Add("cylinder_z", typeof(string));
                    dt2.Columns.Add("gap_acros", typeof(string));
                    dt2.Columns.Add("gap_Around", typeof(string));
                    dt2.Columns.Add("lbl_acros", typeof(string));
                    dt2.Columns.Add("lbl_Around", typeof(string));
                    dt2.Columns.Add("lbl_hght", typeof(string));
                    dt2.Columns.Add("lbl_wdth", typeof(string));
                    frm_vty = "30";
                    SQuery = "select DISTINCT substr(trim(a.col1),1,50) as col1,substr(trim(a.col2),1,50) as col2,substr(trim(a.col3),1,50) as col3,substr(trim(a.col4),1,50) as col4,substr(trim(a.col5),1,50) as col5,substr(trim(a.col6),1,50) as col6,substr(trim(a.col7),1,50) as col7,substr(trim(a.col8),1,50) as col8,substr(trim(a.col9),1,40) as col9,substr(trim(a.col10),1,40) as col10,substr(trim(a.col11),1,40) as col11,substr(trim(a.col12),1,40) as col12, A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.cdrgno,nvl(b.imagef,'-') as imagef,b.cpartno,c.aname as party from costestimate a,item b,famst c,inspmst d where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.icode)=trim(d.icode) and d.type='70' and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum) between " + sname + " and a.vchdate " + xprdRange + " order by a.srno"; //REAL
                    // SQuery = "select DISTINCT substr(trim(a.col1),1,50) as col1,substr(trim(a.col2),1,50) as col2,substr(trim(a.col3),1,50) as col3,substr(trim(a.col4),1,50) as col4,substr(trim(a.col5),1,50) as col5,substr(trim(a.col6),1,50) as col6,substr(trim(a.col7),1,50) as col7,substr(trim(a.col8),1,50) as col8,substr(trim(a.col9),1,40) as col9,substr(trim(a.col10),1,40) as col10,substr(trim(a.col11),1,40) as col11,substr(trim(a.col12),1,40) as col12, A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.cdrgno,nvl(b.imagef,'-') as imagef,b.cpartno,c.aname as party from costestimate a,item b,famst c,inspmst d where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.icode)=trim(d.icode) and d.type='70' and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum)||TO_CHAR(A.VCHDATE,'dd/MM/yyyy') in (" + barCode + ") order by a.srno";
                    dt2.Columns.Add("imagef", typeof(string));
                    //========================dt for loop only on so dt and process plan dt 
                    dt8 = new DataTable();
                    dtm3 = new DataTable();
                    dt1 = new DataTable();
                    DataTable mdt = new DataTable();
                    DataTable dt10 = new DataTable();
                    dt1 = dt2.Clone();
                    dt3 = dt2.Clone();
                    dt4 = dt2.Clone();
                    dt6 = new DataTable();//for more thn 47 rows in job card
                    dt5 = dt2.Clone();
                    dt6 = dt2.Clone();
                    SQuery = "select DISTINCT A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR,a.vchnum,a.icode,substr(a.convdate,1,20) as so from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum) between " + sname + " and a.vchdate " + xprdRange + "  order by fstr";//real
                    //SQuery = "select DISTINCT A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR,a.vchnum,a.icode,substr(a.convdate,1,20) as so from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy') in (" + barCode + ") order by fstr";
                    DataTable dt9 = new DataTable();
                    dt9 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = "";
                    double db1 = 0, db2 = 0, db3 = 0;
                    index = 0;

                    for (int j = 0; j < dt9.Rows.Count; j++)
                    {
                        mq1 = ""; mq2 = ""; mq3 = ""; mq5 = "";
                        mq1 += ",'" + dt9.Rows[j]["icode"].ToString().Trim() + "'";
                        mq2 += ",'" + dt9.Rows[j]["so"].ToString().Trim() + "'";
                        mq3 += ",'" + dt9.Rows[j]["FSTR"].ToString().Trim() + "'";
                        mq5 += ",'" + dt9.Rows[j]["vchnum"].ToString().Trim() + "'";
                        mq4 = "";
                        mq4 = "SELECT branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr, branchcd,type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,icode, (case when qtyord>0 then round(qtysupp/qtyord*100,2) else 0 end) as so_tol  FROM somas  where branchcd='" + frm_mbr + "' and type='40' and orddt " + xprdRange + " and trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') in (" + mq2.TrimStart(',') + ")";//real
                        //mq4 = "SELECT branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr, branchcd,type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,icode, (case when qtyord>0 then round(qtysupp/qtyord*100,2) else 0 end) as so_tol  FROM somas  where branchcd='" + frm_mbr + "' and type='40' and orddt " + DateRange + " and trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') in (" + mq2.TrimStart(',') + ")";
                        dtm3 = fgen.getdata(frm_qstr, frm_cocd, mq4);
                        dt7 = new DataTable();
                        SQuery = "select distinct trim(d.icode) as icode, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,trim(d.TITLE) as title,trim(d.REMARK2) as REMARK2,trim(d.REMARK3) as REMARK3,trim(d.REMARK4) as REMARK4 from inspmst d where d.branchcd='" + frm_mbr + "' and d.type='70' and trim(d.icode) in (" + mq1.TrimStart(',') + ") order by icode";
                        dt7 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        // dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT icode,btchdt FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' and vchdate " + DateRange + ""); //old
                        dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,icode,btchdt,col13 as cylinder,col14 as lbl_AROUND,grade as gap_acros,col15 as lbl_acros,col16 as gap_around,maintdt as lbl_width FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' and trim(icode) in (" + mq1.TrimStart(',') + ")");//after add some new fileds
                        //////////change in above qry
                        //============================================
                        SQuery = "select DISTINCT A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.iweight,b.cdrgno,b.cpartno,c.aname as party,nvl(b.imagef,'-') as imagef from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) IN (" + mq3.TrimStart(',') + ") order by a.vchnum,a.srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//main dt
                        ds = new DataSet();
                        ////
                        SQuery = "select  A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR,sum(is_number(col5)) as total from costestimate a where  trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) IN (" + mq3.TrimStart(',') + ") and nvl(col1,'-')  in ('3','4','5','6','7','8','9') group by A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE)";
                        dt10 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//dt10 only for total in report
                        papergiven = 0;
                        jcqty1 = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr1 = dt1.NewRow();
                            if (int.Parse(dt.Rows[i]["srno"].ToString()) >= 1 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 7)
                            {
                                #region
                                dr1["col_1"] = dt.Rows[i]["COL1"].ToString().Trim();
                                dr1["col_9"] = dt.Rows[i]["SRNO"].ToString().Trim();
                                dr1["col_2"] = dt.Rows[i]["col3"].ToString().Trim();
                                dr1["col_3"] = dt.Rows[i]["col7"].ToString().Trim().toDouble(3).ToString("f");
                                dr1["total"] = fgen.make_double(fgen.seek_iname_dt(dt10, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "total"));
                                dr1["ENTBY1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entby");//d
                                dr1["ENTDT1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entDT");//d
                                dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
                                dr1["VCHDATE"] = dt.Rows[i]["VCH"].ToString().Trim();
                                dr1["CONVDATE"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(4, 6) + " " + dt.Rows[i]["convdate"].ToString().Trim().Substring(10, 10);
                                dr1["sotype"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(2, 2);
                                dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                                dr1["Qty"] = dt.Rows[i]["Qty"].ToString().Trim();
                                dr1["entby2"] = dt.Rows[i]["ent_by"].ToString().Trim();
                                dr1["entdt2"] = dt.Rows[i]["ent_dt"].ToString().Trim();
                                dr1["sheets"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                                dr1["wstg"] = fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                                //changed by vipin
                                dr1["cutsize"] = fgen.make_double(dt.Rows[i]["col19"].ToString().Trim());
                                dr1["reelsize"] = fgen.make_double(dt.Rows[i]["col18"].ToString().Trim());
                                dr1["SHEET_QTY"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                                if (dt.Rows[i]["col9"].ToString().Trim().Length > 1)
                                {
                                    if (dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "07" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "80" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "81")
                                        papergiven += dt.Rows[i]["col7"].ToString().toDouble();
                                    else papergiven += dt.Rows[i]["col7"].ToString().toDouble() * fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IWEIGHT FROM ITEM WHERE TRIM(ICODE)='" + dt.Rows[i]["col9"].ToString().Trim() + "'", "IWEIGHT").toDouble();
                                }
                                if (jcqty1 <= 0)
                                    jcqty1 = dt.Rows[i]["qty"].ToString().toDouble() + dt.Rows[i]["col15"].ToString().toDouble() * dt.Rows[i]["col13"].ToString().toDouble();
                                //dr1["wt_shet"] = Math.Round((dt.Rows[i]["col7"].ToString().Trim().toDouble() + dt.Rows[i]["iweight"].ToString().Trim().toDouble()) / (dt.Rows[i]["qty"].ToString().Trim().toDouble() + dt.Rows[i]["col13"].ToString().Trim().toDouble() + dt.Rows[i]["col15"].ToString().Trim().toDouble()), 3);
                                dr1["edt_by"] = dt.Rows[i]["edt_by"].ToString().Trim();
                                dr1["edt_dt"] = dt.Rows[i]["edt_Dt"].ToString().Trim();
                                dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                                dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                                dr1["party"] = dt.Rows[i]["party"].ToString().Trim();
                                dr1["mkt_rmk"] = dt.Rows[i]["col12"].ToString().Trim();
                                dr1["app_by"] = dt.Rows[i]["app_by"].ToString().Trim();
                                dr1["prod_type"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "prd_typ");//d
                                dr1["OD"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "OD");//d
                                dr1["ID"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ID");//d
                                dr1["Corrug"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "corrug");//d
                                dr1["UPS"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "REJQTY"));//d
                                dr1["DIE"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "COL12"); //d
                                dr1["PLY"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ply"));//d
                                dr1["PPRMK"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "title") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark2") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark3") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark4");
                                dr1["REMARKS"] = dt.Rows[i]["REMARKS"].ToString().Trim();
                                dr1["ALL_WST"] = dt.Rows[i]["COL22"].ToString().Trim().toDouble();
                                db1 = fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt"));
                                db2 = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                                db3 = (db1 / 100) * db2;
                                dr1["LIN_MTR"] = db3;
                                dr1["SOTOLR"] = fgen.seek_iname_dt(dtm3, "fstr='" + dt.Rows[i]["convdate"].ToString().Trim().Substring(0, 20) + "'", "so_tol");
                                dr1["CLOSE_RMK"] = dt.Rows[i]["COMMENTS5"].ToString().Trim();
                                dr1["FSTR"] = dt.Rows[i]["FSTR"].ToString().Trim();
                                dr1["imagef"] = dt.Rows[i]["imagef"].ToString().Trim();
                                //=======new
                                dr1["spec_no"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchnum");
                                dr1["cust_dlvdt"] = Convert.ToDateTime(dt.Rows[i]["enqdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                dr1["ppc_dlvdt"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchdate");
                                dr1["cylinder_z"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "cylinder");
                                dr1["gap_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_acros");
                                dr1["gap_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_around");
                                dr1["lbl_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_acros");
                                dr1["lbl_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_AROUND");
                                dr1["lbl_hght"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt");
                                dr1["lbl_wdth"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_width");
                                dt1.Rows.Add(dr1);
                                #endregion
                            }
                            else if (int.Parse(dt.Rows[i]["srno"].ToString()) == 8)//|| int.Parse(dt.Rows[i]["srno"].ToString()) == 10)//flute size or cut size
                            {
                                #region
                                dr1["col_1"] = dt.Rows[i]["COL1"].ToString().Trim();
                                dr1["col_9"] = dt.Rows[i]["col2"].ToString().Trim();
                                dr1["col_2"] = dt.Rows[i]["col3"].ToString().Trim();
                                dr1["col_3"] = dt.Rows[i]["col7"].ToString().Trim().toDouble(3).ToString("f");
                                dr1["total"] = fgen.make_double(fgen.seek_iname_dt(dt10, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "total"));
                                dr1["ENTBY1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entby");//d
                                dr1["ENTDT1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entDT");//d
                                dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
                                dr1["VCHDATE"] = dt.Rows[i]["VCH"].ToString().Trim();
                                dr1["CONVDATE"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(4, 6) + " " + dt.Rows[i]["convdate"].ToString().Trim().Substring(10, 10);
                                dr1["sotype"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(2, 2);
                                dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                                dr1["Qty"] = dt.Rows[i]["Qty"].ToString().Trim();
                                dr1["entby2"] = dt.Rows[i]["ent_by"].ToString().Trim();
                                dr1["entdt2"] = dt.Rows[i]["ent_dt"].ToString().Trim();
                                dr1["sheets"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                                dr1["wstg"] = fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                                dr1["cutsize"] = fgen.make_double(dt.Rows[i]["col19"].ToString().Trim());
                                dr1["reelsize"] = fgen.make_double(dt.Rows[i]["col18"].ToString().Trim());
                                dr1["SHEET_QTY"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                                //changed by vipin
                                if (dt.Rows[i]["col9"].ToString().Trim().Length > 1)
                                {
                                    if (dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "07" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "80" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "81")
                                        papergiven += dt.Rows[i]["col7"].ToString().toDouble();
                                    else papergiven += dt.Rows[i]["col7"].ToString().toDouble() * fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IWEIGHT FROM ITEM WHERE TRIM(ICODE)='" + dt.Rows[i]["col9"].ToString().Trim() + "'", "IWEIGHT").toDouble();
                                }
                                if (jcqty1 <= 0)
                                    jcqty1 = dt.Rows[i]["qty"].ToString().toDouble() + dt.Rows[i]["col15"].ToString().toDouble() * dt.Rows[i]["col13"].ToString().toDouble();
                                //dr1["wt_shet"] = Math.Round((dt.Rows[i]["col7"].ToString().Trim().toDouble() + dt.Rows[i]["iweight"].ToString().Trim().toDouble()) / (dt.Rows[i]["qty"].ToString().Trim().toDouble() + dt.Rows[i]["col13"].ToString().Trim().toDouble() + dt.Rows[i]["col15"].ToString().Trim().toDouble()), 3);
                                dr1["edt_by"] = dt.Rows[i]["edt_by"].ToString().Trim();
                                dr1["edt_dt"] = dt.Rows[i]["edt_Dt"].ToString().Trim();
                                dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                                dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                                dr1["party"] = dt.Rows[i]["party"].ToString().Trim();
                                dr1["mkt_rmk"] = dt.Rows[i]["col12"].ToString().Trim();
                                dr1["app_by"] = dt.Rows[i]["app_by"].ToString().Trim();
                                dr1["prod_type"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "prd_typ");//d
                                dr1["OD"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "OD");//d
                                dr1["ID"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ID");//d
                                dr1["Corrug"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "corrug");//d
                                dr1["UPS"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "REJQTY"));//d
                                dr1["DIE"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "COL12"); //d
                                dr1["PLY"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ply"));//d
                                dr1["PPRMK"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "title") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark2") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark3") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark4");
                                dr1["REMARKS"] = dt.Rows[i]["REMARKS"].ToString().Trim();
                                dr1["ALL_WST"] = dt.Rows[i]["COL22"].ToString().Trim().toDouble();
                                db1 = fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt"));
                                db2 = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                                db3 = (db1 / 100) * db2;
                                dr1["LIN_MTR"] = db3;
                                dr1["SOTOLR"] = fgen.seek_iname_dt(dtm3, "fstr='" + dt.Rows[i]["convdate"].ToString().Trim().Substring(0, 20) + "'", "so_tol");
                                dr1["CLOSE_RMK"] = dt.Rows[i]["COMMENTS5"].ToString().Trim();
                                dr1["FSTR"] = dt.Rows[i]["FSTR"].ToString().Trim();
                                dr1["imagef"] = dt.Rows[i]["imagef"].ToString().Trim();
                                //=======new
                                dr1["spec_no"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchnum");
                                dr1["cust_dlvdt"] = Convert.ToDateTime(dt.Rows[i]["enqdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                dr1["ppc_dlvdt"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchdate");
                                dr1["cylinder_z"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "cylinder");
                                dr1["gap_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_acros");
                                dr1["gap_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_around");
                                dr1["lbl_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_acros");
                                dr1["lbl_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_AROUND");
                                dr1["lbl_hght"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt");
                                dr1["lbl_wdth"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_width");
                                dt1.Rows.Add(dr1);
                                #endregion
                            }
                            else if (int.Parse(dt.Rows[i]["srno"].ToString()) == 13 || int.Parse(dt.Rows[i]["srno"].ToString()) == 14)
                            {
                                #region
                                dr1["col_1"] = dt.Rows[i]["COL1"].ToString().Trim();
                                dr1["total"] = fgen.make_double(fgen.seek_iname_dt(dt10, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "total"));
                                dr1["col_4"] = dt.Rows[i]["col2"].ToString().Trim();
                                dr1["col_5"] = dt.Rows[i]["col3"].ToString().Trim();
                                dr1["col_3"] = dt.Rows[i]["col7"].ToString().Trim().toDouble(3).ToString("f");
                                dr1["ENTBY1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entby");//d
                                dr1["ENTDT1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entDT");//d
                                dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
                                dr1["VCHDATE"] = dt.Rows[i]["VCH"].ToString().Trim();
                                dr1["CONVDATE"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(4, 6) + " " + dt.Rows[i]["convdate"].ToString().Trim().Substring(10, 10);
                                dr1["sotype"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(2, 2);
                                dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                                dr1["Qty"] = dt.Rows[i]["Qty"].ToString().Trim();
                                dr1["entby2"] = dt.Rows[i]["ent_by"].ToString().Trim();
                                dr1["entdt2"] = dt.Rows[i]["ent_dt"].ToString().Trim();
                                dr1["sheets"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                                dr1["wstg"] = fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                                dr1["cutsize"] = fgen.make_double(dt.Rows[i]["col19"].ToString().Trim());
                                dr1["reelsize"] = fgen.make_double(dt.Rows[i]["col18"].ToString().Trim());
                                dr1["SHEET_QTY"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                                //changed by vipin
                                if (dt.Rows[i]["col9"].ToString().Trim().Length > 1)
                                {
                                    if (dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "07" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "80" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "81")
                                        papergiven += dt.Rows[i]["col7"].ToString().toDouble();
                                    else papergiven += dt.Rows[i]["col7"].ToString().toDouble() * fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IWEIGHT FROM ITEM WHERE TRIM(ICODE)='" + dt.Rows[i]["col9"].ToString().Trim() + "'", "IWEIGHT").toDouble();
                                }
                                if (jcqty1 <= 0)
                                    jcqty1 = dt.Rows[i]["qty"].ToString().toDouble() + dt.Rows[i]["col15"].ToString().toDouble() * dt.Rows[i]["col13"].ToString().toDouble();
                                //dr1["wt_shet"] = Math.Round((dt.Rows[i]["col7"].ToString().Trim().toDouble() + dt.Rows[i]["iweight"].ToString().Trim().toDouble()) / (dt.Rows[i]["qty"].ToString().Trim().toDouble() + dt.Rows[i]["col13"].ToString().Trim().toDouble() + dt.Rows[i]["col15"].ToString().Trim().toDouble()), 3);
                                dr1["edt_by"] = dt.Rows[i]["edt_by"].ToString().Trim();
                                dr1["edt_dt"] = dt.Rows[i]["edt_Dt"].ToString().Trim();
                                dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                                dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                                dr1["party"] = dt.Rows[i]["party"].ToString().Trim();
                                dr1["mkt_rmk"] = dt.Rows[i]["col12"].ToString().Trim();
                                dr1["app_by"] = dt.Rows[i]["app_by"].ToString().Trim();
                                dr1["prod_type"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "prd_typ");//d
                                dr1["OD"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "OD");//d
                                dr1["ID"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ID");//d
                                dr1["Corrug"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "corrug");//d
                                dr1["UPS"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "REJQTY"));//d
                                dr1["DIE"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "COL12"); //d
                                dr1["PLY"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ply"));//d
                                dr1["PPRMK"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "title") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark2") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark3") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark4");
                                dr1["REMARKS"] = dt.Rows[i]["REMARKS"].ToString().Trim();
                                dr1["ALL_WST"] = dt.Rows[i]["COL22"].ToString().Trim().toDouble();
                                db1 = fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt"));
                                db2 = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                                db3 = (db1 / 100) * db2;
                                dr1["LIN_MTR"] = db3;
                                dr1["SOTOLR"] = fgen.seek_iname_dt(dtm3, "fstr='" + dt.Rows[i]["convdate"].ToString().Trim().Substring(0, 20) + "'", "so_tol");
                                dr1["CLOSE_RMK"] = dt.Rows[i]["COMMENTS5"].ToString().Trim();
                                dr1["FSTR"] = dt.Rows[i]["FSTR"].ToString().Trim();
                                dr1["imagef"] = dt.Rows[i]["imagef"].ToString().Trim();
                                //=======new
                                dr1["spec_no"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchnum");
                                dr1["cust_dlvdt"] = Convert.ToDateTime(dt.Rows[i]["enqdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                dr1["ppc_dlvdt"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchdate");
                                dr1["cylinder_z"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "cylinder");
                                dr1["gap_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_acros");
                                dr1["gap_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_around");
                                dr1["lbl_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_acros");
                                dr1["lbl_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_AROUND");
                                dr1["lbl_hght"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt");
                                dr1["lbl_wdth"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_width");
                                dt1.Rows.Add(dr1);
                                #endregion
                            }
                        }
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dt1.Rows[i]["wt_shet"] = (papergiven / jcqty1).toDouble(3);
                        }
                        #region Process Plan Date Filling
                        ///for 2
                        index = 0;
                        for (int i = 18; i < dt.Rows.Count; i++)
                        {
                            if (int.Parse(dt.Rows[i]["srno"].ToString()) >= 18 && int.Parse(dt.Rows[i]["srno"].ToString()) < 23)
                            {
                                dt1.Rows[index]["col_4"] = dt.Rows[i]["col2"].ToString().Trim();
                                dt1.Rows[index]["col_5"] = dt.Rows[i]["col3"].ToString().Trim();
                                dt1.Rows[index]["col_6"] = dt.Rows[i]["col5"].ToString().Trim();
                                //dr1["total"] = fgen.make_double(fgen.seek_iname_dt(dt10, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "total"));
                                dt1.Rows[index]["total"] = fgen.make_double(fgen.seek_iname_dt(dt10, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "total"));
                                index++;
                            }

                        }
                        ///for 3 th column
                        //  index = 0;
                        for (int i = 0; i < 7; i++)
                        {
                            #region
                            switch (Convert.ToString(i))
                            {
                                case "0":
                                    dt1.Rows[i]["col_7"] = "Top Paper";
                                    break;
                                case "1":
                                    dt1.Rows[i]["col_7"] = "Fluting";
                                    break;
                                case "2":
                                    dt1.Rows[i]["col_7"] = "Backing";
                                    break;
                                case "3":
                                    dt1.Rows[i]["col_7"] = "Fluting";
                                    break;
                                case "4":
                                    dt1.Rows[i]["col_7"] = "Backing";
                                    break;
                                case "5":
                                    dt1.Rows[i]["col_7"] = "Fluting";
                                    break;
                                case "6":
                                    dt1.Rows[i]["col_7"] = "Backing";
                                    break;
                            }
                            #endregion
                        }

                        //for 4th column
                        for (int i = 0; i < 7; i++)
                        {
                            #region
                            switch (Convert.ToString(i))
                            {
                                case "0":
                                    dt1.Rows[i]["col_8"] = "Deccal";
                                    break;
                                case "1":
                                    dt1.Rows[i]["col_8"] = "CuttinG Size";
                                    break;
                                case "2":
                                    dt1.Rows[i]["col_8"] = "Fluting";
                                    break;
                                case "3":
                                    dt1.Rows[i]["col_8"] = "No of Paper";
                                    break;
                                case "4":
                                    dt1.Rows[i]["col_8"] = "No. of LIner";
                                    break;
                                case "5":
                                    dt1.Rows[i]["col_8"] = "";
                                    break;
                                case "6":
                                    dt1.Rows[i]["col_8"] = "";
                                    break;
                            }
                            #endregion
                        }
                        #endregion
                        mdt.Merge(dt1);
                        dt1 = new DataTable();
                        dt1 = dt2.Clone();
                    }
                    // if (dt1.Rows.Count > 0)
                    if (mdt.Rows.Count > 0)
                    {
                        //dt1 = fgen.addBarCode(dt1, "fstr", true);
                        //dt1.TableName = "Prepcur";
                        //dt1.Columns.Add("ImgPath", typeof(string));
                        //dt1.Columns.Add("jcImg", typeof(System.Byte[]));
                        mdt = fgen.addBarCode(mdt, "fstr", true);
                        mdt.TableName = "Prepcur";
                        mdt.Columns.Add("ImgPath", typeof(string));
                        mdt.Columns.Add("jcImg", typeof(System.Byte[]));
                        FileStream FilStr;
                        BinaryReader BinRed;
                        //foreach (DataRow dr in dt1.Rows)
                        foreach (DataRow dr in mdt.Rows)
                        {
                            dr["ImgPath"] = "-";
                            try
                            {
                                fpath = dr["imagef"].ToString().Trim();
                                if (fpath != "-")
                                {
                                    FilStr = new FileStream(fpath, FileMode.Open);
                                    BinRed = new BinaryReader(FilStr);
                                    dr["ImgPath"] = fpath;
                                    dr["jcImg"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                    FilStr.Close();
                                    BinRed.Close();
                                }
                            }
                            catch { }
                        }
                    }
                    //ds.Tables.Add(dt1);
                    ds.Tables.Add(mdt);
                    #endregion
                    #region
                    //  if (dt1.Rows.Count > 0)
                    if (mdt.Rows.Count > 0)
                    {
                        //mq0 = "";
                        //mq0 = "SELECT distinct A.NAME AS STAGE,A.TYPE1,b.icode,b.srno FROM TYPE A,ITWSTAGE B WHERE A.ID='K' AND TRIM(A.TYPE1)=TRIM(B.STAGEC) AND  trim(B.branchcd)='00'  order by b.icode,b.srno";
                        //dt8 = new DataTable();
                        //dt8 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        mq1 = "SELECT distinct A.NAME AS STAGE,A.TYPE1,b.icode,b.srno FROM TYPE A,ITWSTAGE B,costestimate c WHERE A.ID='K' AND TRIM(A.TYPE1)=TRIM(B.STAGEC) and trim(b.icode)=trim(c.icode) AND  trim(c.branchcd)='" + frm_mbr + "' and  trim(c.type)='" + frm_vty + "' and trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by b.icode,b.srno";//original
                        mq1 = "SELECT distinct A.NAME AS STAGE,A.TYPE1,b.icode,b.srno FROM TYPE A,ITWSTAGE B,costestimate c WHERE A.ID='K' AND TRIM(A.TYPE1)=TRIM(B.STAGEC) and trim(b.icode)=trim(c.icode) AND  trim(c.branchcd)='" + frm_mbr + "' and  trim(c.type)='" + frm_vty + "' and trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by b.icode,b.srno";//testingg
                        dtm = new DataTable();
                        dtm = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        //mq2 = "select icode,a2 AS NET_PRODN,a4 AS REJ,job_no, JOB_DT,STAGE,mchcode,type from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type in ('86','88') and job_no||job_dt in (" + barCode + ")";
                        mq2 = "select icode,sum(a2) AS NET_PRODN,sum(a4) AS REJ,job_no, JOB_DT,STAGE,type from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type in ('86','88') and job_no||job_dt in (" + barCode + ") group by icode,job_no, JOB_DT,STAGE,type";//original
                        mq2 = "select icode,sum(a2) AS NET_PRODN,sum(a4) AS REJ,job_no, JOB_DT,STAGE,type from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type in ('86','88') and job_no||job_dt in (" + barCode + ") group by icode,job_no, JOB_DT,STAGE,type";//testing
                        DataTable dtm1 = new DataTable();
                        dtm1 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                        DataTable dtm2 = new DataTable();
                        mq3 = "select icode,IS_NUMBER(col4) AS NET_PRODN,IS_NUMBER(col5) AS REJ,enqno AS JOBNO,TO_CHAR(enqdt,'DD/MM/YYYY') AS JOBDT from costestimate WHERE BRANCHCD='" + frm_mbr + "' AND type='60' and trim(enqno)||to_char(enqdt,'dd/mm/yyyy') in (" + barCode + ")";//original
                        mq3 = "select icode,IS_NUMBER(col4) AS NET_PRODN,IS_NUMBER(col5) AS REJ,enqno AS JOBNO,TO_CHAR(enqdt,'DD/MM/YYYY') AS JOBDT from costestimate WHERE BRANCHCD='" + frm_mbr + "' AND type='60' and trim(enqno)||to_char(enqdt,'dd/mm/yyyy') in (" + barCode + ")";//for testing
                        dtm2 = fgen.getdata(frm_qstr, frm_cocd, mq3);
                        dtm.Columns.Add("MACHINENAME", typeof(double));
                        dtm.Columns.Add("NET_PRODN", typeof(double));
                        dtm.Columns.Add("REJ", typeof(double));
                        if (dtm.Rows.Count > 0)
                        {
                            if (dtm1.Rows.Count > 0 || dtm2.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtm.Rows.Count; i++)
                                {
                                    if (dtm.Rows[i]["type1"].ToString().Trim() == "08")
                                    {
                                        if (dtm2.Rows.Count > 0)
                                        {
                                            dtm.Rows[i]["NET_PRODN"] = dtm2.Rows[0]["NET_PRODN"].ToString().Trim();
                                            dtm.Rows[i]["REJ"] = dtm2.Rows[0]["REJ"].ToString().Trim();
                                        }
                                    }
                                    else
                                    {
                                        if (dtm1.Rows.Count > 0)
                                        {
                                            dtm.Rows[i]["NET_PRODN"] = fgen.seek_iname_dt(dtm1, "STAGE='" + dtm.Rows[i]["TYPE1"].ToString().Trim() + "'", "NET_PRODN");
                                            dtm.Rows[i]["REJ"] = fgen.seek_iname_dt(dtm1, "STAGE='" + dtm.Rows[i]["TYPE1"].ToString().Trim() + "'", "REJ");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            mq0 = "";
                            mq0 = "SELECT '-' as STAGE,'-' as TYPE1,'-' as icode,'-' as srno,'-' as NET_PRODN,'-' as REJ from dual";
                            dtm = new DataTable();
                            dtm = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        }
                        //============
                        dtm.TableName = "type1";
                        ds.Tables.Add(dtm);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_jobCard_CRP", "std_jobCard_CRP", ds, "");

                    }
                    #endregion
                }
                else
                {
                    #region Job Card Print
                    dt2 = new DataTable();//add dummy column in this
                    dt2.Columns.Add("col_1", typeof(string));
                    dt2.Columns.Add("col_2", typeof(string));
                    dt2.Columns.Add("col_3", typeof(string));
                    dt2.Columns.Add("col_4", typeof(string));
                    dt2.Columns.Add("col_5", typeof(string));
                    dt2.Columns.Add("col_6", typeof(string));
                    dt2.Columns.Add("col_7", typeof(string));
                    dt2.Columns.Add("col_8", typeof(string));
                    dt2.Columns.Add("col_9", typeof(string));
                    dt2.Columns.Add("col_10", typeof(string));
                    dt2.Columns.Add("col_11", typeof(string));
                    dt2.Columns.Add("col_12", typeof(string));
                    ////
                    dt2.Columns.Add("ENTBY1", typeof(string));
                    dt2.Columns.Add("ENTDT1", typeof(string));
                    dt2.Columns.Add("vchnum", typeof(string));
                    dt2.Columns.Add("vchdate", typeof(string));
                    dt2.Columns.Add("CONVDATE", typeof(string));
                    dt2.Columns.Add("icode", typeof(string));
                    dt2.Columns.Add("Qty", typeof(string));
                    dt2.Columns.Add("entby2", typeof(string));
                    dt2.Columns.Add("entdt2", typeof(string));
                    dt2.Columns.Add("sheets", typeof(string));
                    dt2.Columns.Add("wstg", typeof(string));
                    dt2.Columns.Add("wt_shet", typeof(string));
                    dt2.Columns.Add("edt_by", typeof(string));
                    dt2.Columns.Add("edt_dt", typeof(string));
                    dt2.Columns.Add("iname", typeof(string));
                    dt2.Columns.Add("cpartno", typeof(string));
                    dt2.Columns.Add("party", typeof(string));
                    dt2.Columns.Add("mkt_rmk", typeof(string));
                    dt2.Columns.Add("app_by", typeof(string));
                    dt2.Columns.Add("prod_type", typeof(string));
                    dt2.Columns.Add("OD", typeof(string));
                    dt2.Columns.Add("PLY", typeof(string));
                    dt2.Columns.Add("ID", typeof(string));
                    dt2.Columns.Add("Corrug", typeof(string));
                    dt2.Columns.Add("UPS", typeof(string));
                    dt2.Columns.Add("DIE", typeof(string));
                    SQuery = "select DISTINCT d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.cdrgno,b.cpartno,c.aname as party from costestimate a,item b,famst c,inspmst d where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.icode)=trim(d.icode) and d.type='70' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)='" + scode + "' order by a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    ds = new DataSet();
                    dt1 = dt2.Clone();
                    dt3 = dt2.Clone();
                    dt4 = dt2.Clone();
                    dt6 = new DataTable();//for more thn 47 rows in job card
                    dt5 = dt2.Clone();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt1.NewRow();
                        if (int.Parse(dt.Rows[i]["srno"].ToString()) <= 15)
                        {
                            dr1["col_1"] = dt.Rows[i]["col2"].ToString().Trim();
                            dr1["col_2"] = dt.Rows[i]["col3"].ToString().Trim();
                            dr1["col_3"] = dt.Rows[i]["col5"].ToString().Trim();
                            dr1["ENTBY1"] = dt.Rows[i]["entby"].ToString().Trim();
                            dr1["ENTDT1"] = dt.Rows[i]["entDT"].ToString().Trim();
                            dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
                            dr1["VCHDATE"] = dt.Rows[i]["VCH"].ToString().Trim();
                            dr1["CONVDATE"] = dt.Rows[i]["convdate"].ToString().Trim();
                            dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["Qty"] = dt.Rows[i]["Qty"].ToString().Trim();
                            dr1["entby2"] = dt.Rows[i]["ent_by"].ToString().Trim();
                            dr1["entdt2"] = dt.Rows[i]["ent_dt"].ToString().Trim();
                            dr1["sheets"] = dt.Rows[i]["col14"].ToString().Trim();
                            dr1["wstg"] = dt.Rows[i]["col15"].ToString().Trim();
                            dr1["wt_shet"] = dt.Rows[i]["irate"].ToString().Trim();
                            dr1["edt_by"] = dt.Rows[i]["edt_by"].ToString().Trim();
                            dr1["edt_dt"] = dt.Rows[i]["edt_Dt"].ToString().Trim();
                            dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            dr1["party"] = dt.Rows[i]["party"].ToString().Trim();
                            dr1["mkt_rmk"] = dt.Rows[i]["col12"].ToString().Trim();
                            dr1["app_by"] = dt.Rows[i]["app_by"].ToString().Trim();
                            dr1["prod_type"] = dt.Rows[i]["prd_typ"].ToString().Trim();
                            dr1["OD"] = dt.Rows[i]["OD"].ToString().Trim();
                            dr1["ID"] = dt.Rows[i]["ID"].ToString().Trim();
                            dr1["Corrug"] = dt.Rows[i]["corrug"].ToString().Trim();
                            dr1["UPS"] = dt.Rows[i]["REJQTY"].ToString().Trim();
                            dr1["DIE"] = dt.Rows[i]["COL12"].ToString().Trim();
                            dr1["PLY"] = dt.Rows[i]["ply"].ToString().Trim();
                            dt1.Rows.Add(dr1);
                        }
                    }
                    ///for 2
                    for (int i = 16; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt3.NewRow();
                        if (int.Parse(dt.Rows[i]["srno"].ToString()) > 15 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 31)
                        {
                            dr1["col_4"] = dt.Rows[i]["col2"].ToString().Trim();
                            dr1["col_5"] = dt.Rows[i]["col3"].ToString().Trim();
                            dr1["col_6"] = dt.Rows[i]["col5"].ToString().Trim();
                            dt3.Rows.Add(dr1);
                        }
                    }

                    for (int i = 32; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt4.NewRow();
                        if (int.Parse(dt.Rows[i]["srno"].ToString()) > 31 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 47)
                        {
                            dr1["col_7"] = dt.Rows[i]["col2"].ToString().Trim();
                            dr1["col_8"] = dt.Rows[i]["col3"].ToString().Trim();
                            dr1["col_9"] = dt.Rows[i]["col5"].ToString().Trim();
                            dt4.Rows.Add(dr1);
                        }
                    }

                    if (dt.Rows.Count > 47)
                    {
                        for (int i = 47; i < dt.Rows.Count; i++)
                        {
                            dr1 = dt6.NewRow();
                            if (int.Parse(dt.Rows[i]["srno"].ToString()) > 47 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 63)
                            {
                                dr1["col_10"] = dt.Rows[i]["col2"].ToString().Trim();
                                dr1["col_11"] = dt.Rows[i]["col3"].ToString().Trim();
                                dr1["col_12"] = dt.Rows[i]["col5"].ToString().Trim();
                                dt6.Rows.Add(dr1);
                            }
                        }
                    }

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        try
                        {
                            dt1.Rows[i]["col_4"] = dt3.Rows[i]["col_4"].ToString().Trim();
                        }
                        catch { }
                        try
                        {
                            dt1.Rows[i]["col_5"] = dt3.Rows[i]["col_5"].ToString().Trim();
                        }
                        catch { }
                        try
                        {
                            dt1.Rows[i]["col_6"] = dt3.Rows[i]["col_6"].ToString().Trim();
                        }
                        catch { }
                    }

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        try
                        {
                            dt1.Rows[i]["col_7"] = dt4.Rows[i]["col_7"].ToString().Trim();
                        }
                        catch { }
                        try
                        {
                            dt1.Rows[i]["col_8"] = dt4.Rows[i]["col_8"].ToString().Trim();
                        }
                        catch { }
                        try
                        {
                            dt1.Rows[i]["col_9"] = dt4.Rows[i]["col_9"].ToString().Trim();
                        }
                        catch { }
                    }

                    if (dt.Rows.Count > 47)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            try
                            {
                                dt1.Rows[i]["col_10"] = dt6.Rows[i]["col_7"].ToString().Trim();
                            }
                            catch { }
                            try
                            {
                                dt1.Rows[i]["col_11"] = dt6.Rows[i]["col_8"].ToString().Trim();
                            }
                            catch { }
                            try
                            {
                                dt1.Rows[i]["col_12"] = dt6.Rows[i]["col_9"].ToString().Trim();
                            }
                            catch { }
                        }
                    }

                    ds.Tables.Add(dt1);

                    dsRep = new DataSet();
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt1);

                        mq1 = "SELECT distinct A.NAME AS STAGE,A.TYPE1 FROM TYPE A,ITWSTAGE B,costestimate c  WHERE A.ID='K'  AND TRIM(A.TYPE1)=TRIM(B.STAGEC) AND  trim(c.branchcd)||trim(c.type)||trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy')||trim(c.icode)='" + scode + "' and trim(b.icode)=trim(c.icode)";
                        dtm = new DataTable();
                        dtm = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        dtm.TableName = "type1";

                        dsRep.Tables.Add(dtm);

                        Print_Report_BYDS(frm_cocd, frm_mbr, "STD_JC", frm_rptName, dsRep, "STD_JC");
                    }
                    #endregion
                }
                break;
            //Prodn Slip Print
            case "F1021":
                #region Prodn Slip Print
                SQuery = "select b.iname,b.unit as bunit,b.cpartno,a.rej_rw,a.naration as rmk,a.invno, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.iqtyin,a.iqtyout,a.icode,a.desc_,a.ent_by,a.ent_dt,a.edt_by,a.edt_dt from ivoucher  a,item b  where  trim(a.icode)=trim(b.icode) and  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "STD_PRODN", frm_rptName, dsRep, "STD_PRODN");
                }
                #endregion
                break;
            case "F70201":
            case "F70203":
                #region Simple Voucher Print
                col1 = "Select a.type,trim(substr(NVL(a.naration,'-'),1,220)) AS NARATION,nvl(a.mrnnum,'-') as mrnnum,NVL(a.mrndate,A.VCHDATE) AS MRNDATE,NVL(a.tax,'-') AS TAX,A.COSTCD,NVL(a.refnum,'-') AS REFNUM,NVL(a.invno,'-') AS INVNO,";
                col2 = " NVL(a.invdate,A.VCHDATE) AS INVDATE,nvl(a.CCENT,'-') as ccent,a.acode,a.rcode,a.vchnum,a.vchdate,nvl(a.app_by,'-') as app_by,nvl(a.app_date,a.vchdate) as app_Date,a.dramt,a.cramt,nvl(a.quantity,0)as quantity,NVL(a.refdate,A.VCHDATE) AS REFDATE,";
                col3 = " NVL(b.PERSON,'-') AS PERSON,NVL(b.aname,'-') AS ANAME,NVL(B.ANAME,'-') AS PARTY,a.ent_by,nvl(b.payment,'-') as pnm,a.tfcdr,a.tfccr,nvl(a.FCTYPE,'-') ";
                SQuery = col1 + col2 + col3 + " FCTYPE,c.name as VchTypeName,(CASE WHEN NVL(A.CHECK_BY,'-')='-' THEN 'Un-Approved' WHEN NVL(A.CHECK_BY,'-')!='-' AND NVL(A.APP_BY,'-')='-' THEN 'Checked (Un-Approved)' WHEN NVL(A.CHECK_BY,'-')!='-' AND NVL(A.APP_BY,'-')!='-' THEN 'Approved' end) as rptHeader,a.check_by from VOUCHER a left outer join famst b on TRIM(A.ACODE)=TRIM(B.ACODE) ,type c where a.type=c.type1 and c.id='V' and a.branchcd||a.type||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + scode + "' order by a.srno ";
                if (frm_cocd == "KLAS")
                {
                    SQuery = col1 + col2 + col3 + " FCTYPE,c.name as VchTypeName,'-' AS rptHeader,'-'AS check_by from VOUCHER a left outer join famst b on TRIM(A.ACODE)=TRIM(B.ACODE) ,type c where a.type=c.type1 and c.id='V' and a.branchcd||a.type||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "' order by a.srno ";
                }
                else
                {
                    SQuery = col1 + col2 + col3 + " FCTYPE,c.name as VchTypeName,(CASE WHEN NVL(A.CHECK_BY,'-')='-' THEN 'Un-Approved' WHEN NVL(A.CHECK_BY,'-')!='-' AND NVL(A.APP_BY,'-')='-' THEN 'Checked (Un-Approved)' WHEN NVL(A.CHECK_BY,'-')!='-' AND NVL(A.APP_BY,'-')!='-' THEN 'Approved' end) as rptHeader,a.check_by from VOUCHER a left outer join famst b on TRIM(A.ACODE)=TRIM(B.ACODE) ,type c where a.type=c.type1 and c.id='V' and a.branchcd||a.type||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "' order by a.srno ";
                }

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    frm_rptName = "vch_rpt_simple";
                    if (frm_cocd == "KLAS" && barCode.Substring(2, 2) == "31") frm_rptName = "vch_rpt_simple_dr";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "vch_rpt_simple", frm_rptName, dsRep, "", "Y");
                }
                #endregion
                break;
            //GST Debit Note

            case "F1022":
                #region GST Debit Note
                scode = scode.Replace(";", "");
                if (scode.Length > 20)
                {
                    frm_mbr = scode.Substring(0, 2);
                    frm_vty = scode.Substring(2, 2);
                    sname = scode.Substring(4, 6);
                    if (scode.Length > 20)
                        sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                    else sname = "'" + sname + "'" + " and " + "'" + sname + "'";

                    scode = "a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '" + frm_vty + "' AND TRIM(a.vchnum) BETWEEN " + sname + " AND A.VCHDATE  " + xprdRange + " ";
                }
                else scode = "a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')='" + scode + "'";
                mq0 = "";
                if (frm_cocd == "SAIL")
                {
                    mq0 = "DEBIT NOTE / SUPPLEMENTARY INVOICE";
                }
                else
                {
                    mq0 = "DEBIT NOTE";
                }
                SQuery = "SELECT '" + mq0 + "' AS HEADER,(case when trim(nvl(f.pname,'-'))!='-' then f.pname else F.ANAME end) as aname,F.ADDR1 AS FDDR1,F.ADDR2 AS FADDR2,F.ADDR3 AS FADDR3,F.STATEN AS FSTATE,SUBSTR(F.GST_NO,0,2) AS FSTATECODE,F.GIRNO AS FGIRNO,F.GST_NO AS FGST_NO,(case when length(trim(I.cINAME))>5 then i.ciname else i.iname end) as iname,I.UNIT AS IUNIT,I.HSCODE,A.*,F.VENCODE,I.CPARTNO FROM IVOUCHER A,FAMST F ,ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND " + scode + " ORDER BY A.VCHNUM";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    repCount = 2;
                    if (frm_cocd == "KLAS") repCount = 3;
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_vch_rpt", frm_rptName, dsRep, "std_vch_rpt", "Y");
                }
                #endregion

                break;

            case "F1023":
                # region GST Credit Note
                scode = scode.Replace(";", "");
                if (scode.Length > 20)
                {
                    frm_mbr = scode.Substring(0, 2);
                    frm_vty = scode.Substring(2, 2);
                    sname = scode.Substring(4, 6);
                    if (scode.Length > 20)
                        sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                    else sname = "'" + sname + "'" + " and " + "'" + sname + "'";

                    scode = "a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '" + frm_vty + "' AND TRIM(a.vchnum) BETWEEN " + sname + " AND A.VCHDATE  " + xprdRange + " ";
                }
                else scode = "a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')='" + scode + "'";
                SQuery = "SELECT 'CREDIT NOTE' AS HEADER,(case when trim(nvl(f.pname,'-'))!='-' then f.pname else F.ANAME end) as aname,F.ADDR1 AS FDDR1,F.ADDR2 AS FADDR2,F.ADDR3 AS FADDR3,F.STATEN AS FSTATE,SUBSTR(F.GST_NO,0,2) AS FSTATECODE,F.GIRNO AS FGIRNO,F.GST_NO AS FGST_NO,(case when length(trim(i.ciname))>5 then i.ciname else I.INAME end) as iname,I.UNIT AS IUNIT,I.HSCODE,A.*,F.VENCODE,I.CPARTNO FROM IVOUCHER A,FAMST F ,ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND " + scode + " ORDER BY A.VCHNUM";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    repCount = 2;
                    if (frm_cocd == "KLAS") repCount = 3;
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Vch_Rpt", frm_rptName, dsRep, "std_Vch_Rpt", "Y");
                }
                #endregion
                break;
            //GST Advance Voucher
            case "F1024":
                #region GST Advance Voucher
                SQuery = "SELECT d.name as header,b.aname as party,B.STATEN,b.staffcd,B.GRP,b.addr1,b.addr2,b.addr3,b.gst_no as gst,b.girno as pann,c.iname, A.* FROM IVOUCHER A,famst b,item c,type d WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(d.type1) and d.id='V' and TRIM(A.BRANCHCD)||TRIM(a.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + scode + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Advance_Voucher", frm_rptName, dsRep, "std_Advance_Voucher");
                }
                #endregion
                break;
            //RCM Voucher
            case "F1025":
                #region RCM Voucher
                SQuery = "SELECT d.name as header,b.aname as party,b.staffcd,B.STATEN,B.GRP,b.addr1,b.addr2,b.addr3,b.gst_no as gst,b.girno as pann,c.iname, A.* FROM IVOUCHER A,famst b,item c,type d WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(d.type1) and d.id='V' and TRIM(A.BRANCHCD)||TRIM(a.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + scode + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_RCMVOUCHER", frm_rptName, dsRep, "std_RCMVOUCHER");
                }
                #endregion
                break;
            //Purchase Voucher
            case "F1026":
                #region Purchase Voucher
                SQuery = "SELECT d.name as header,b.aname as party,B.STATEN,b.staffcd,B.GRP,b.addr1,b.addr2,b.addr3,b.gst_no as gst,b.girno as pann,c.iname, A.* FROM IVOUCHER A,famst b,item c,type d WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(d.type1) and d.id='V' and TRIM(A.BRANCHCD)||TRIM(a.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + scode + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pur_Voucher", frm_rptName, dsRep, "std_Pur_Voucher");
                }
                #endregion
                break;
            //Refund Voucher
            case "F1027":
                #region Refund Voucher
                SQuery = "SELECT d.name as header,b.aname as party,B.STATEN,b.staffcd,B.GRP,b.addr1,b.addr2,b.addr3,b.gst_no as gst,b.girno as pann,c.iname, A.* FROM IVOUCHER A,famst b,item c,type d WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(d.type1) and d.id='V' and TRIM(A.BRANCHCD)||TRIM(a.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + scode + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Refund_Voucher", frm_rptName, dsRep, "std_Refund_Voucher");
                }
                #endregion
                break;
            //Service Voucher
            case "F1028":
                #region Service Voucher
                SQuery = "SELECT d.name as header,b.aname as party,B.STATEN,b.staffcd,B.GRP,b.addr1,b.addr2,b.addr3,b.gst_no as gst,b.girno as pann,c.iname, A.* FROM IVOUCHER A,famst b,item c,type d WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(d.type1) and d.id='V' and TRIM(A.BRANCHCD)||TRIM(a.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + scode + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Srvice_Voucher", frm_rptName, dsRep, "std_Srvice_Voucher");
                }
                #endregion
                break;
            // CMPL Packing List
            case "F1029":
                if (frm_cocd == "CMPL")
                {
                    #region
                    dtm = new DataTable();
                    dtm.Columns.Add("Header", typeof(string));
                    dtm.Columns.Add("vchnum", typeof(string));
                    dtm.Columns.Add("vchdate", typeof(string));
                    dtm.Columns.Add("icode", typeof(string));
                    dtm.Columns.Add("iname", typeof(string));
                    dtm.Columns.Add("hscode", typeof(string));
                    dtm.Columns.Add("partycode", typeof(string));
                    dtm.Columns.Add("srno", typeof(double));
                    dtm.Columns.Add("carton_wt", typeof(double));
                    dtm.Columns.Add("desc_", typeof(string));
                    dtm.Columns.Add("QTY_PR_pallet", typeof(double));
                    dtm.Columns.Add("pr_pallet_wt", typeof(double));
                    dtm.Columns.Add("tot_wt", typeof(double));
                    dtm.Columns.Add("pallet_wt", typeof(double));
                    dtm.Columns.Add("pallet_dimen", typeof(string));
                    dtm.Columns.Add("no_of_pallet", typeof(double));
                    dtm.Columns.Add("tot_qty", typeof(double));
                    dtm.Columns.Add("net_wt", typeof(double));
                    dtm.Columns.Add("pallet_no", typeof(string));
                    dtm.Columns.Add("TARRIFNO", typeof(string));
                    dtm.Columns.Add("GST_NO", typeof(string));
                    dtm.Columns.Add("TELNUM", typeof(string));
                    dtm.Columns.Add("PAYMENT", typeof(string));
                    dtm.Columns.Add("EMAIL", typeof(string));
                    dtm.Columns.Add("aname", typeof(string));
                    dtm.Columns.Add("addr1", typeof(string));
                    dtm.Columns.Add("addr2", typeof(string));
                    dtm.Columns.Add("addr3", typeof(string));
                    dtm.Columns.Add("addr4", typeof(string));
                    dtm.Columns.Add("COUNTRY", typeof(string));
                    dtm.Columns.Add("col9", typeof(string));
                    dtm.Columns.Add("col10", typeof(string));
                    dtm.Columns.Add("finvno", typeof(string));
                    dtm.Columns.Add("svch", typeof(string));
                    dtm.Columns.Add("svchdt", typeof(string));
                    dtm.Columns.Add("cscode", typeof(string));
                    dtm.Columns.Add("cbm", typeof(double));
                    dtm.Columns.Add("tot_cbm", typeof(double));
                    #endregion
                    #region
                    header_n = "Packing List";//for cmpl new format
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                    dsRep = new DataSet(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
                    SQuery = "select distinct trim(icode) as icode,trim(iname) as iname,TARRIFNO,hscode from item where substr(trim(icode),1,1)='9'";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    SQuery = "select distinct '" + header_n + "' as header, trim(a.col9)||trim(a.col10)||trim(a.icode) as fstr,trim(a.vchnum) as vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as partycode,a.SRNO,is_number(a.COL1) AS carton_wt,a.COL2 AS DESC_,is_number(a.COL3) AS QTY_PR_pallet,is_number(a.col4) as pr_pallet_wt,is_number(a.col5) as tot_wt,is_number(a.col11) as pallet_wt,a.col6 as pallet_dimen,is_number(a.col7) as no_of_pallet ,is_number(a.col3)*is_number(a.col7) as tot_qty,a.col13 as pallet_no,is_number(a.col12) as cbm,is_number(a.col38) as net_wt,is_number(a.col14) as tot_cbm,B.GST_NO,B.TELNUM ,B.PAYMENT,B.EMAIL,b.aname,b.addr1,b.addr2,b.addr3,b.addr4,B.COUNTRY,a.col9,a.col10  from scratch  a ,item i,famst b where  trim(a.acode)=trim(b.acode) and TRIM(A.BRANCHCD)||trim(a.type)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "'  order by a.srno,icode";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    SQuery = "select a.finvno,trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr1,trim(b.acode) as scode,trim(a.icode) as sicode,a.vchnum as svch,to_char(a.vchdate,'dd/mm/yyyy') as svchdt,b.cscode from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type='4F' and a.vchdate " + xprdRange + "";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//for finvno and cscode
                    if (dt.Rows.Count > 0)
                    {
                        int cnt = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region
                            mq1 = "";
                            dr1 = dtm.NewRow();
                            dr1["Header"] = dt.Rows[i]["header"].ToString().Trim();
                            dr1["vchnum"] = dt.Rows[i]["vchnum"].ToString().Trim();
                            dr1["vchdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                            mq1 = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["svch"] = dt.Rows[i]["col9"].ToString().Trim();
                            dr1["svchdt"] = dt.Rows[i]["col10"].ToString().Trim();
                            dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr1["icode"].ToString().Trim() + "'", "iname");
                            dr1["hscode"] = fgen.seek_iname_dt(dt2, "icode='" + dr1["icode"].ToString().Trim() + "'", "hscode");
                            dr1["TARRIFNO"] = fgen.seek_iname_dt(dt2, "icode='" + dr1["icode"].ToString().Trim() + "'", "TARRIFNO");
                            dr1["finvno"] = fgen.seek_iname_dt(dt3, "sicode='" + dr1["icode"].ToString().Trim() + "' and svch='" + dt.Rows[i]["col9"].ToString().Trim() + "' and svchdt='" + dt.Rows[i]["col10"].ToString().Trim() + "'", "finvno");
                            dr1["cscode"] = fgen.seek_iname_dt(dt3, "sicode='" + dr1["icode"].ToString().Trim() + "' and svch='" + dt.Rows[i]["col9"].ToString().Trim() + "' and svchdt='" + dt.Rows[i]["col10"].ToString().Trim() + "'", "cscode");
                            dr1["net_wt"] = fgen.make_double(dt.Rows[i]["net_wt"].ToString().Trim());
                            dr1["partycode"] = dt.Rows[i]["partycode"].ToString().Trim();
                            dr1["srno"] = fgen.make_double(dt.Rows[i]["srno"].ToString().Trim());
                            dr1["carton_wt"] = fgen.make_double(dt.Rows[i]["carton_wt"].ToString().Trim());
                            dr1["desc_"] = dt.Rows[i]["desc_"].ToString().Trim();
                            dr1["QTY_PR_pallet"] = fgen.make_double(dt.Rows[i]["QTY_PR_pallet"].ToString().Trim());
                            dr1["pr_pallet_wt"] = fgen.make_double(dt.Rows[i]["pr_pallet_wt"].ToString().Trim());
                            dr1["tot_wt"] = fgen.make_double(dt.Rows[i]["tot_wt"].ToString().Trim());
                            dr1["pallet_wt"] = fgen.make_double(dt.Rows[i]["pallet_wt"].ToString().Trim());
                            dr1["pallet_dimen"] = dt.Rows[i]["pallet_dimen"].ToString().Trim();
                            dr1["no_of_pallet"] = fgen.make_double(dt.Rows[i]["no_of_pallet"].ToString().Trim());
                            dr1["tot_qty"] = fgen.make_double(dt.Rows[i]["tot_qty"].ToString().Trim());
                            dr1["pallet_no"] = dt.Rows[i]["pallet_no"].ToString().Trim();
                            dr1["GST_NO"] = dt.Rows[i]["GST_NO"].ToString().Trim();
                            dr1["TELNUM"] = dt.Rows[i]["TELNUM"].ToString().Trim();
                            dr1["PAYMENT"] = dt.Rows[i]["PAYMENT"].ToString().Trim();
                            dr1["EMAIL"] = dt.Rows[i]["EMAIL"].ToString().Trim();
                            dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                            dr1["addr1"] = dt.Rows[i]["addr1"].ToString().Trim();
                            dr1["addr2"] = dt.Rows[i]["addr2"].ToString().Trim();
                            dr1["addr3"] = dt.Rows[i]["addr3"].ToString().Trim();
                            dr1["addr4"] = dt.Rows[i]["addr4"].ToString().Trim();
                            dr1["COUNTRY"] = dt.Rows[i]["COUNTRY"].ToString().Trim();
                            dr1["col9"] = dt.Rows[i]["col9"].ToString().Trim();
                            dr1["col10"] = dt.Rows[i]["col10"].ToString().Trim();
                            dr1["cbm"] = dt.Rows[i]["cbm"].ToString().Trim();
                            dr1["tot_cbm"] = dt.Rows[i]["tot_cbm"].ToString().Trim();
                            dtm.Rows.Add(dr1);
                            cnt++;
                            #endregion
                        }
                        dtm.TableName = "Prepcur";
                        dsRep.Tables.Add(dtm);
                        ///===================                  	
                        SQuery = "Select distinct d.tdsnum, d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dsRep.Tables[0].Rows[0]["cscode"].ToString().Trim() + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count <= 0)
                        {
                            dt = new DataTable();
                            SQuery = "Select 'Consignee' as tdsnum,'Same as Buyer' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                            SQuery = "SELECT ANAME AS consign,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 daddr4,'-' as dtel,'-' as dtinno,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM FAMST WHERE ACODE='" + dsRep.Tables[0].Rows[0]["partycode"].ToString().Trim() + "'";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        }
                        dt.TableName = "csmst";
                        dsRep.Tables.Add(dt);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_PackingList_New", "std_PackingList_New", dsRep, header_n);
                    }
                    #endregion
                }
                else if (frm_cocd == "KRML")
                {
                    #region KRML PACKING LIST
                    SQuery = "";
                    dt = new DataTable();
                    mq2 = ""; mq3 = ""; mq4 = "";
                    SQuery = "select a.vchnum as invno,TO_CHAR(a.vchdate,'DD/MM/YYYY') AS INVDATE,A.ACODE AS ACODE,A.ICODE,b.aname,a.col2 as iname,a.col3 as qty,'-' as partno,b.payterm,b.payment,b.ADDR1,b.ADDR2,b.ADDR3,b.ADDR4,(case when nvl(trim(a.col4),'-')='-' then '0' else a.col4 end) as col4,(case when nvl(trim(a.col5),'-')='-' then '0' else a.col5 end) as col5,a.col6,(case when nvl(trim(a.col7),'-')='-' then '0' else a.col7 end) as col7,a.col8,a.col9,a.col10,A.COL13,a.col46 from scratch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||TRIM(A.vchnum)||TO_CHAR(A.vchDATE,'DD/MM/YYYY') = '" + barCode.Substring(0, 20) + "' order by a.srno";//00PL00000223/07/2019
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dsRep = new DataSet();
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);

                        SQuery = "select a.invno AS Hinvno,TO_CHAR(a.invdate,'DD/MM/YYYY') AS Hinvdate,a.ship2,a.ship3,a.ship4,a.ship5,a.lbnetwt,a.lbgrswt,a.exprmk1,a.exprmk2,a.exprmk3,a.exprmk4,a.exprmk5,a.addl1,a.addl2,a.addl3,a.addl4,a.addl5,a.tmaddl1,a.tmaddl2,a.tmaddl3,a.addl6,a.remark as packrem,s.ANAME AS nconsign ,s.ADDR1 as ndaddr1,s.ADDR2 as ndaddr2,s.ADDR3 as ndaddr3,s.ADDR4 as ndaddr4,s.email AS NEMAIL,s.telnum AS NTELNUM,s.fax AS NFAX from hundip A left join csmst s on trim(a.ship3)=trim(s.acode) where a.branchcd='" + frm_mbr + "' and a.type='IV' and a.acode='" + dt.Rows[0]["ACODE"].ToString().Trim() + "' and trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')='" + dt.Rows[0]["col9"].ToString().Trim() + dt.Rows[0]["col10"].ToString().Trim() + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        mq2 = "select sum(round(iweight,3)) as iweight,sum(round(is_number(btchno),3)) as btchno from ivoucherp where branchcd||type||TRIM(vchnum)||TO_CHAR(vchDATE,'DD/MM/YYYY') = '" + frm_mbr + "4F" + dt.Rows[0]["col9"].ToString().Trim() + dt.Rows[0]["col10"].ToString().Trim() + "'";
                        mq3 = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "iweight");
                        mq4 = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "btchno");

                        SQuery = "select pono,to_char(podate,'dd/mm/yyyy') as podate,mo_vehi,no_bdls,stform_no,ins_co,curren,cscode,acode,lc_dtl,pi_dtl,freight,advrcvd,dlv_terms,vehi_fitno,retention,amt_rea,destin,destcount,rg23c as insurance_no,mcomment," + mq3 + " as iweight," + mq4 + " as btchno from salep where branchcd||type||TRIM(vchnum)||TO_CHAR(vchDATE,'DD/MM/YYYY') = '" + frm_mbr + "4F" + dt.Rows[0]["col9"].ToString().Trim() + dt.Rows[0]["col10"].ToString().Trim() + "'";//00PL00000218/07/2019
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        dt3.TableName = "sale";
                        dsRep.Tables.Add(dt3);

                        SQuery = "SELECT ANAME AS consign ,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 as daddr4,email,telnum,fax,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM CSMST WHERE ACODE='" + dt3.Rows[0]["cscode"].ToString().Trim() + "'";
                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //if (dt1.Rows.Count <= 0)
                        //{
                        //    dt1 = new DataTable();
                        //    SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as email,'-' as telnum,'-' as fax,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                        //    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //}
                        dt1.TableName = "csmst";
                        dsRep.Tables.Add(dt1);

                        if (dt2.Rows.Count < 1)
                        {
                            SQuery = "select '-' as Hinvno,'-' as Hinvdate,'-' as ship2,'-' as ship3,'-' as ship4,'-' as ship5,'-' as lbnetwt,'-' as lbgrswt,'-' as exprmk1,'-' as exprmk2,'-' as exprmk3,'-' as exprmk4,'-' as exprmk5,'-' as addl1,'-' as addl2,'-' as addl3,'-' as addl4,'-' as addl5,'-' as tmaddl1,'-' as tmaddl2,'-' as tmaddl3,'-' as addl6 from dual";
                            dt2 = new DataTable();
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        }
                        dt2.TableName = "hundi";
                        dsRep.Tables.Add(dt2);
                        frm_rptName = "KRML_einv_pl";
                        Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "");
                    }
                    #endregion

                }
                else if (frm_cocd == "KLAS")
                {
                    #region
                    //tej-base/dprint.aspx?STR=ERP@24@klas@202000@000023@BVAL@F1029@004000000911/05/2020==============string
                    //scode = scode.Replace(";", "");
                    //frm_mbr = scode.Substring(0, 2);
                    //frm_vty = scode.Substring(2, 2);
                    //if (scode.Length > 20)
                    //{
                    //    sname = scode.Substring(4, 6);
                    //    if (scode.Length > 20)
                    //        sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                    //    else sname = "'" + sname + "'" + " and " + "'" + sname + "'";
                    //    scode = "a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' AND TRIM(a.packno) BETWEEN " + sname + " AND A.packdate  " + xprdRange + " ";
                    //}
                    //else scode = "a.BRANCHCD||a.TYPE||TRIM(a.packno)||TO_CHAr(a.packdate,'DD/MM/YYYY')='" + scode + "'";

                    sname = "";
                    v1 = scode.Split(';');
                    for (int k = 0; k < v1.Length; k++)
                    {
                        if (!v1[k].Contains("/"))
                        {
                            if (sname.Length > 0)
                            {
                                if (v1[k].ToString().ToString().Length > 6)
                                {
                                    sname = sname + "," + "'" + v1[k].ToString().Substring(3, 6) + "'";
                                    frm_mbr = v1[k].ToString().Substring(0, 2);
                                    frm_vty = v1[k].ToString().Substring(2, 2);
                                }
                                else sname = sname + "," + "'" + v1[k].ToString() + "'";
                            }
                            else
                            {
                                if (v1[k].ToString().ToString().Length > 6)
                                {
                                    sname = "'" + v1[k].ToString().Substring(4, 6) + "'";
                                    frm_mbr = v1[k].ToString().Substring(0, 2);
                                    frm_vty = v1[k].ToString().Substring(2, 2);
                                }
                                else sname = "'" + v1[k].ToString() + "'";
                            }
                        }
                    }

                    header_n = "Packing List";//Despatch Note
                    dt = new DataTable();
                    SQuery = "SELECT '" + header_n + "' AS HEADER,a.branchcd,a.type,trim(a.packno) as vchnum,to_char(a.packdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as acode,D.NAME,TRIM(B.ANAME) AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,a.packno,to_char(a.packdate,'dd/mm/yyyy') as vchdate,a.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT,a.fdue as grade, A.PORDNO,A.PORDDT,A.ICODE,c.siname,C.INAME,C.CPARTNO,C.MAKER AS COLOR,A.ORDLINE,A.QTYSUPP AS QTY,A.QTYORD AS ORD_qTY,A.IRATE,C.UNIT,nvl(a.cscode,'-') as cscode,A.GRNO AS J_ROLLNO,substr(A.NO_BDLS,1,1) as roll_no,A.NO_BDLS AS ROLL,A.WEIGHT AS STD_PKG,nvl(g.ANAME,'-') AS CONSG,nvl(g.addr1,'-') as cdr1,nvl(g.addr2,'-') as cadr2,nvl(g.addr3,'-') as cadr3,a.ent_by FROM DESPATCH  a left outer join csmst G on trim(a.cscode)=trim(g.acode),FAMST B ,ITEM C ,TYPE D  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(a.TYPE)=TRIM(D.TYPE1) AND D.ID='V' AND a.branchcd='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND TRIM(A.PACKNO) in (" + sname + ") AND A.PACKDATE " + xprdRange + " order by a.srno";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);
                        frm_rptName = "KLAS_Desp_Adv";
                        Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "KLAS_Desp_Adv");
                    }
                    #endregion
                }
                break;

            //MRR Sticker
            case "S1002":
                #region MRR Sticker
                SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode)||trim(a.btchno) as fstr,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1 from ivoucher a,famst b ,item c,type d where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                if (frm_cocd == "SVPL") SQuery = "Select a.branchcd,e.kclreelno as fstr,e.reelwin as PACKSIZE,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,(CASE WHEN LENGTH(NVL(e.RLOCN,'-'))>2 THEN A.LOCATION ELSE c.binno END) as locn,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,c.packsize AS PACKSIZE2 from ivoucher a,famst b ,item c,type d,reelvch e where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=e.branchcd||e.type||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt1.TableName = "barcode";
                dt1 = fgen.addBarCode(dt1, "fstr", true);
                dsRep.Tables.Add(dt1);
                frm_rptName = "mrr_stk";
                if (frm_cocd == "SVPL") frm_rptName = "mrr_stk_svpl";
                Print_Report_BYDS(frm_cocd, frm_mbr, "mrr_stk", frm_rptName, dsRep, "Sticker", "Y");
                #endregion
                break;
            case "F25245R":
                #region Return Sticker
                SQuery = "Select a.branchcd,e.kclreelno as fstr,e.reelwin as iqtyin,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.iqtyin as ivchin,A.IQTY_WT,a.invno,a.invdate,a.col1,c.packsize from ivoucher a,item c,reelvch e where trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=e.branchcd||e.type||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt1.TableName = "barcode";
                dt1 = fgen.addBarCode(dt1, "fstr", true);
                dsRep.Tables.Add(dt1);
                frm_rptName = "mrr_stk";
                if (frm_cocd == "SVPL") frm_rptName = "ret_stk_svpl";
                Print_Report_BYDS(frm_cocd, frm_mbr, "ret_stk", frm_rptName, dsRep, "Sticker", "Y");
                #endregion
                break;
            //FG Sticker
            case "F25245A":
                #region FG Sticker
                if (!barCode.Contains("'")) barCode = "'" + barCode + "'";
                SQuery = "Select distinct a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode) as fstr,A.MORDER,a.type,a.vchnum as docno,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.ciname,c.cpartno,C.MAT5,a.btchno,(case when a.iqtyin>0 then a.iqtyin else a.iqty_chl end) as iqtyin,A.iqtyin as IQTY_WT,a.invno,a.invdate,c.packsize from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                //**** BATCH QTY FORM                
                dt = new DataTable();
                dt = dt1.Clone();
                dt.Columns.Add("binqty", typeof(double));
                dt.Columns.Add("header", typeof(string));
                dt.Columns.Add("srno", typeof(string));
                dt.Columns.Add("vchnum", typeof(string));
                double fullQty = 0;
                double batchQty = 0;

                int z = 0, srno = 0;
                dr1 = null;
                foreach (DataRow dtr1 in dt1.Rows)
                {
                    fullQty = fgen.make_double(dtr1["iqtyin"].ToString());
                    batchQty = fgen.make_double(dtr1["packsize"].ToString());
                    //if (fullQty == batchQty && z == 0) break;
                    if (batchQty == 0) batchQty = fullQty;
                    do
                    {
                        dr1 = dt.NewRow();
                        foreach (DataColumn dc in dt1.Columns)
                        {
                            dr1[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                        }
                        if (fullQty <= batchQty)
                        {
                            batchQty = fullQty;
                            fullQty = fullQty - batchQty;
                        }
                        else fullQty = fullQty - batchQty;
                        dr1["binqty"] = batchQty;
                        //dr1["fstr"] = dtr1["fstr"].ToString() + "~" + srno.ToString() + "~" + batchQty.ToString();
                        dr1["header"] = "";
                        dr1["vchnum"] = dr1["docno"].ToString() + "" + srno.ToString();
                        dt.Rows.Add(dr1);
                        srno++;
                    }
                    while (fullQty != 0);
                    z++;
                }

                dt.TableName = "barcode";
                dt = fgen.addBarCode(dt, "fstr", true);
                dsRep.Tables.Add(fgen.mTitle(dt, 1));


                Print_Report_BYDS(frm_cocd, frm_mbr, "fg_stk", "prod_stkSACL", dsRep, "Sticker", "Y");
                #endregion
                break;
            case "F25194":
                #region
                double totqty; double packqty; double fillqty;
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                header_n = "WIP Transfer Material Tags";

                SQuery = "select '" + header_n + "' as header , 0 as tagqty, A.vchnum ,to_char(A.vchdate,'dd/mm/yyyy') as vchdate , A.IQTYOUT, B.NAME, T.NAME AS WipNAME , C.ICODE , C.INAME , C.BINNO , C.PACKSIZE , d.ANAME  from ivoucher a, type b,TYPE T ,ITEM C, FAMST D    where TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.IOPR)=TRIM(T.TYPE1)  AND TRIM(A.ICODE)=TRIM(C.ICODE)  AND TRIM(C.AC_ACODE)=TRIM(D.ACODE)  AND  A.branchcd||A.type||A.vchnum||to_char(A.vchdate,'dd/mm/yyyy') in ('" + barCode + "') AND B.ID='1' AND T.ID='1' ORDER BY INAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("grp", typeof(string));
                dt1 = new DataTable();
                dt1 = dt.Clone();
                dr1 = null;
                int srn = 0;
                foreach (DataRow drwIP in dt.Rows)
                {
                    totqty = fgen.make_double(drwIP["iqtyout"].ToString().Trim());
                    packqty = fgen.make_double(drwIP["packsize"].ToString().Trim());
                    if (packqty == 0) packqty = totqty;
                    fillqty = 0;
                    do
                    {
                        if (totqty > packqty)
                        {
                            fillqty = packqty;
                            totqty = totqty - packqty;
                        }
                        else
                        {
                            fillqty = totqty;
                            totqty = totqty - fillqty;
                        }
                        dr1 = dt1.NewRow();
                        dr1["tagqty"] = fillqty;
                        dr1["grp"] = drwIP["icode"].ToString().Trim() + fillqty + srn;
                        dr1["vchnum"] = drwIP["vchnum"].ToString().Trim();
                        dr1["vchdate"] = Convert.ToDateTime(drwIP["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        dr1["iname"] = drwIP["iname"].ToString().Trim();
                        dr1["aname"] = drwIP["aname"].ToString().Trim();
                        dr1["name"] = drwIP["name"].ToString().Trim();
                        dr1["wipname"] = drwIP["wipname"].ToString().Trim();
                        dr1["icode"] = drwIP["icode"].ToString().Trim();
                        dr1["packsize"] = drwIP["packsize"].ToString().Trim();
                        dr1["binno"] = drwIP["binno"].ToString().Trim();
                        dr1["iqtyout"] = drwIP["iqtyout"].ToString().Trim();

                        dt1.Rows.Add(dr1);
                        srn++;
                    }
                    while (totqty != 0);
                }
                dt1.TableName = "Prepcur";
                dsRep.Tables.Add(dt1);
                Print_Report_BYDS(frm_cocd, frm_mbr, "AmarWipStk", "AmarWipStk", dsRep, header_n);
                #endregion
                break;
            case "F35101":
                #region job card print

                frm_mbr = barCode.Substring(0, 2);
                frm_vty = barCode.Substring(2, 2);

                dt2 = new DataTable();
                dt2.Columns.Add("col_1", typeof(string));
                dt2.Columns.Add("col_2", typeof(string));
                dt2.Columns.Add("col_3", typeof(string));
                dt2.Columns.Add("col_4", typeof(string));
                dt2.Columns.Add("col_5", typeof(string));
                dt2.Columns.Add("col_6", typeof(string));
                dt2.Columns.Add("col_7", typeof(string));
                dt2.Columns.Add("col_8", typeof(string));
                dt2.Columns.Add("col_9", typeof(string));
                dt2.Columns.Add("col_10", typeof(string));
                dt2.Columns.Add("col_11", typeof(string));
                dt2.Columns.Add("col_12", typeof(string));
                ////
                dt2.Columns.Add("ENTBY1", typeof(string));
                dt2.Columns.Add("ENTDT1", typeof(string));
                dt2.Columns.Add("vchnum", typeof(string));
                dt2.Columns.Add("vchdate", typeof(string));
                dt2.Columns.Add("CONVDATE", typeof(string));
                dt2.Columns.Add("sotype", typeof(string));
                dt2.Columns.Add("icode", typeof(string));
                dt2.Columns.Add("Qty", typeof(string));
                dt2.Columns.Add("entby2", typeof(string));
                dt2.Columns.Add("entdt2", typeof(string));
                dt2.Columns.Add("sheets", typeof(string));
                dt2.Columns.Add("wstg", typeof(string));
                dt2.Columns.Add("wt_shet", typeof(double));
                dt2.Columns.Add("edt_by", typeof(string));
                dt2.Columns.Add("edt_dt", typeof(string));
                dt2.Columns.Add("iname", typeof(string));
                dt2.Columns.Add("cpartno", typeof(string));
                dt2.Columns.Add("party", typeof(string));
                dt2.Columns.Add("mkt_rmk", typeof(string));
                dt2.Columns.Add("app_by", typeof(string));
                dt2.Columns.Add("prod_type", typeof(string));
                dt2.Columns.Add("OD", typeof(string));
                dt2.Columns.Add("PLY", typeof(double));
                dt2.Columns.Add("ID", typeof(string));
                dt2.Columns.Add("Corrug", typeof(string));
                dt2.Columns.Add("UPS", typeof(double));
                dt2.Columns.Add("DIE", typeof(string));
                dt2.Columns.Add("FSTR", typeof(string));
                dt2.Columns.Add("PPRMK", typeof(string));
                dt2.Columns.Add("SOTOLR", typeof(string));
                dt2.Columns.Add("REMARKS", typeof(string));
                dt2.Columns.Add("LIN_MTR", typeof(string));
                dt2.Columns.Add("CLOSE_RMK", typeof(string));
                dt2.Columns.Add("ALL_WST", typeof(double));
                ///===============new filds
                dt2.Columns.Add("spec_no", typeof(string));
                dt2.Columns.Add("cust_dlvdt", typeof(string));
                dt2.Columns.Add("ppc_dlvdt", typeof(string));
                dt2.Columns.Add("cylinder_z", typeof(string));
                dt2.Columns.Add("gap_acros", typeof(string));
                dt2.Columns.Add("gap_Around", typeof(string));
                dt2.Columns.Add("lbl_acros", typeof(string));
                dt2.Columns.Add("lbl_Around", typeof(string));
                dt2.Columns.Add("lbl_hght", typeof(string));
                dt2.Columns.Add("lbl_wdth", typeof(string));

                SQuery = "select DISTINCT substr(trim(a.col1),1,50) as col1,substr(trim(a.col2),1,50) as col2,substr(trim(a.col3),1,50) as col3,substr(trim(a.col4),1,50) as col4,substr(trim(a.col5),1,50) as col5,substr(trim(a.col6),1,50) as col6,substr(trim(a.col7),1,50) as col7,substr(trim(a.col8),1,50) as col8,substr(trim(a.col9),1,40) as col9,substr(trim(a.col10),1,40) as col10,substr(trim(a.col11),1,40) as col11,substr(trim(a.col12),1,40) as col12, A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.cdrgno,nvl(b.imagef,'-') as imagef,b.cpartno,c.aname as party from costestimate a,item b,famst c,inspmst d where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.icode)=trim(d.icode) and d.type='70' and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.srno";
                dt2.Columns.Add("imagef", typeof(string));
                SQuery = "select DISTINCT A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.iweight,b.cdrgno,b.cpartno,c.aname as party,nvl(b.imagef,'-') as imagef,SUBSTR(A.CONVDATE,1,20) AS SODETAIL from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.branchcd)||A.TYPE||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in ('" + barCode + "') order by a.vchnum,a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dtm3 = new DataTable();
                dt7 = new DataTable();
                dt8 = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    dtm3 = fgen.getdata(frm_qstr, frm_cocd, "SELECT branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr, branchcd,type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,icode, (case when qtyord>0 then round(qtysupp/qtyord*100,2) else 0 end) as so_tol  FROM somas  where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)='" + dt.Rows[0]["SODETAIL"].ToString().Trim() + dt.Rows[0]["icode"].ToString().Trim() + "' ");
                    dt7 = fgen.getdata(frm_qstr, frm_cocd, "select distinct trim(d.icode) as icode, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,trim(d.TITLE) as title,trim(d.REMARK2) as REMARK2,trim(d.REMARK3) as REMARK3,trim(d.REMARK4) as REMARK4 from inspmst d where d.branchcd='" + frm_mbr + "' and d.type='70' and TRIM(ICODE)='" + dt.Rows[0]["icode"].ToString().Trim() + "' order by icode");
                    dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,icode,btchdt,col13 as cylinder,col14 as lbl_AROUND,grade as gap_acros,col15 as lbl_acros,col16 as gap_around,maintdt as lbl_width FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' and TRIM(ICODE)='" + dt.Rows[0]["icode"].ToString().Trim() + "' ");//after add some new fileds
                }

                ds = new DataSet();
                dt1 = dt2.Clone();
                dt3 = dt2.Clone();
                dt4 = dt2.Clone();
                dt6 = new DataTable();//for more thn 47 rows in job card
                dt5 = dt2.Clone();
                dt6 = dt2.Clone();
                papergiven = 0;
                jcqty1 = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr1 = dt1.NewRow();
                    if (int.Parse(dt.Rows[i]["srno"].ToString()) <= 15)
                    {
                        dr1["col_1"] = dt.Rows[i]["col2"].ToString().Trim();
                        dr1["col_2"] = dt.Rows[i]["col3"].ToString().Trim();
                        // dr1["col_3"] = dt.Rows[i]["col5"].ToString().Trim();
                        dr1["col_3"] = dt.Rows[i]["col7"].ToString().Trim().toDouble(3).ToString("f");
                        dr1["ENTBY1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entby");//d
                        dr1["ENTDT1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entDT");//d
                        dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
                        dr1["VCHDATE"] = dt.Rows[i]["VCH"].ToString().Trim();
                        dr1["CONVDATE"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(4, 6) + " " + dt.Rows[i]["convdate"].ToString().Trim().Substring(10, 10);
                        dr1["sotype"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(2, 2);
                        dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr1["Qty"] = dt.Rows[i]["Qty"].ToString().Trim();
                        dr1["entby2"] = dt.Rows[i]["ent_by"].ToString().Trim();
                        dr1["entdt2"] = dt.Rows[i]["ent_dt"].ToString().Trim();
                        dr1["sheets"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                        dr1["wstg"] = fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                        // old field     //dr1["wt_shet"] = dt.Rows[i]["irate"].ToString().Trim();                                               
                        //changed by vipin
                        if (dt.Rows[i]["col9"].ToString().Trim().Length > 1)
                        {
                            if (dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "07" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "80" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "81")
                                papergiven += dt.Rows[i]["col7"].ToString().toDouble();
                            else papergiven += dt.Rows[i]["col7"].ToString().toDouble() * fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IWEIGHT FROM ITEM WHERE TRIM(ICODE)='" + dt.Rows[i]["col9"].ToString().Trim() + "'", "IWEIGHT").toDouble();
                        }
                        if (jcqty1 <= 0)
                            jcqty1 = dt.Rows[i]["qty"].ToString().toDouble() + dt.Rows[i]["col15"].ToString().toDouble() * dt.Rows[i]["col13"].ToString().toDouble();
                        //dr1["wt_shet"] = Math.Round((dt.Rows[i]["col7"].ToString().Trim().toDouble() + dt.Rows[i]["iweight"].ToString().Trim().toDouble()) / (dt.Rows[i]["qty"].ToString().Trim().toDouble() + dt.Rows[i]["col13"].ToString().Trim().toDouble() + dt.Rows[i]["col15"].ToString().Trim().toDouble()), 3);
                        dr1["edt_by"] = dt.Rows[i]["edt_by"].ToString().Trim();
                        dr1["edt_dt"] = dt.Rows[i]["edt_Dt"].ToString().Trim();
                        dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                        dr1["party"] = dt.Rows[i]["party"].ToString().Trim();
                        dr1["mkt_rmk"] = dt.Rows[i]["col12"].ToString().Trim();
                        dr1["app_by"] = dt.Rows[i]["app_by"].ToString().Trim();
                        dr1["prod_type"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "prd_typ");//d
                        dr1["OD"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "OD");//d
                        dr1["ID"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ID");//d
                        dr1["Corrug"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "corrug");//d
                        dr1["UPS"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "REJQTY"));//d
                        dr1["DIE"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "COL12"); //d
                        dr1["PLY"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ply"));//d
                        dr1["PPRMK"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "title") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark2") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark3") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark4");
                        dr1["REMARKS"] = dt.Rows[i]["REMARKS"].ToString().Trim();
                        dr1["ALL_WST"] = dt.Rows[i]["COL22"].ToString().Trim().toDouble();
                        db1 = fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt"));
                        db2 = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                        db3 = (db1 / 100) * db2;
                        dr1["LIN_MTR"] = db3;
                        dr1["SOTOLR"] = fgen.seek_iname_dt(dtm3, "fstr='" + dt.Rows[i]["convdate"].ToString().Trim().Substring(0, 20) + "'", "so_tol");
                        dr1["CLOSE_RMK"] = dt.Rows[i]["COMMENTS5"].ToString().Trim();
                        dr1["FSTR"] = dt.Rows[i]["FSTR"].ToString().Trim();
                        dr1["imagef"] = dt.Rows[i]["imagef"].ToString().Trim();
                        //=======new fields
                        dr1["spec_no"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchnum");
                        dr1["cust_dlvdt"] = Convert.ToDateTime(dt.Rows[i]["enqdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        dr1["ppc_dlvdt"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchdate");
                        dr1["cylinder_z"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "cylinder");
                        dr1["gap_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_acros");
                        dr1["gap_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_around");
                        dr1["lbl_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_acros");
                        dr1["lbl_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_AROUND");
                        dr1["lbl_hght"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt");
                        dr1["lbl_wdth"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_width");
                        dt1.Rows.Add(dr1);
                    }
                }
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    dt1.Rows[i]["wt_shet"] = (papergiven / jcqty1).toDouble(3);
                }
                #region Process Plan Date Filling
                ///for 2
                index = 0;
                for (int i = 16; i < dt.Rows.Count; i++)
                {
                    if (int.Parse(dt.Rows[i]["srno"].ToString()) > 15 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 31)
                    {
                        dt1.Rows[index]["col_4"] = dt.Rows[i]["col2"].ToString().Trim();
                        dt1.Rows[index]["col_5"] = dt.Rows[i]["col3"].ToString().Trim();
                        dt1.Rows[index]["col_6"] = dt.Rows[i]["col5"].ToString().Trim();
                        index++;
                    }
                }
                index = 0;
                for (int i = 32; i < dt.Rows.Count; i++)
                {
                    if (int.Parse(dt.Rows[i]["srno"].ToString()) > 31 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 47)
                    {
                        dt1.Rows[index]["col_7"] = dt.Rows[i]["col2"].ToString().Trim();
                        dt1.Rows[index]["col_8"] = dt.Rows[i]["col3"].ToString().Trim();
                        dt1.Rows[index]["col_9"] = dt.Rows[i]["col5"].ToString().Trim();
                        index++;
                    }
                }
                ///for 4 th column
                index = 0;
                if (dt.Rows.Count > 47)
                {
                    for (int i = 47; i < dt.Rows.Count; i++)
                    {
                        if (int.Parse(dt.Rows[i]["srno"].ToString()) > 47 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 63)
                        {
                            dt1.Rows[index]["col_10"] = dt.Rows[i]["col2"].ToString().Trim();
                            dt1.Rows[index]["col_11"] = dt.Rows[i]["col3"].ToString().Trim();
                            dt1.Rows[index]["col_12"] = dt.Rows[i]["col5"].ToString().Trim();
                            index++;
                        }
                    }
                }
                #endregion
                if (dt1.Rows.Count > 0)
                {
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dt1.TableName = "Prepcur";
                    dt1.Columns.Add("ImgPath", typeof(string));
                    dt1.Columns.Add("jcImg", typeof(System.Byte[]));
                    FileStream FilStr;
                    BinaryReader BinRed;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        dr["ImgPath"] = "-";
                        try
                        {
                            fpath = dr["imagef"].ToString().Trim();
                            if (fpath != "-")
                            {
                                FilStr = new FileStream(fpath, FileMode.Open);
                                BinRed = new BinaryReader(FilStr);
                                dr["ImgPath"] = fpath;
                                dr["jcImg"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                FilStr.Close();
                                BinRed.Close();
                            }
                        }
                        catch { }
                    }
                }
                ds.Tables.Add(dt1);
                #endregion
                if (dt1.Rows.Count > 0)
                {
                    barCode = "'" + barCode.Substring(4, 16) + "'";

                    #region
                    mq1 = "SELECT distinct trim(c.branchcd)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) as fstr,A.NAME AS STAGE,A.TYPE1,b.icode,b.srno,c.vchnum as job_no,to_char(c.vchdate,'dd/mm/yyyy') as job_dt FROM TYPE A,ITWSTAGE B,costestimate c WHERE A.ID='K' AND TRIM(A.TYPE1)=TRIM(B.STAGEC) and trim(b.icode)=trim(c.icode) AND  trim(c.branchcd)='" + frm_mbr + "' and  trim(c.type)='" + frm_vty + "' and trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by fstr,b.icode,b.srno"; //27jan
                    dtm = new DataTable();
                    dtm = fgen.getdata(frm_qstr, frm_cocd, mq1);
                    //mq2 = "select icode,a2 AS NET_PRODN,a4 AS REJ,job_no, JOB_DT,STAGE,mchcode,type from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type in ('86','88') and job_no||job_dt in (" + barCode + ")";
                    mq2 = "select icode,sum(a2) AS NET_PRODN,sum(a4) AS REJ,job_no, JOB_DT,STAGE,type from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type in ('86','88') and job_no||job_dt in (" + barCode + ") group by icode,job_no, JOB_DT,STAGE,type";
                    DataTable dtm1 = new DataTable();
                    dtm1 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                    DataTable dtm2 = new DataTable();
                    mq3 = "select icode,IS_NUMBER(col4) AS NET_PRODN,IS_NUMBER(col5) AS REJ,enqno AS JOB_NO,TO_CHAR(enqdt,'DD/MM/YYYY') AS JOB_DT from costestimate WHERE BRANCHCD='" + frm_mbr + "' AND type='60' and trim(enqno)||to_char(enqdt,'dd/mm/yyyy') in (" + barCode + ")";
                    dtm2 = fgen.getdata(frm_qstr, frm_cocd, mq3);
                    dtm.Columns.Add("MACHINENAME", typeof(double));
                    dtm.Columns.Add("NET_PRODN", typeof(double));
                    dtm.Columns.Add("REJ", typeof(double));
                    if (dtm.Rows.Count > 0)
                    {
                        if (dtm1.Rows.Count > 0 || dtm2.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtm.Rows.Count; i++)
                            {
                                if (dtm.Rows[i]["type1"].ToString().Trim() == "08")
                                {
                                    if (dtm2.Rows.Count > 0)
                                    {
                                        dtm.Rows[i]["NET_PRODN"] = fgen.seek_iname_dt(dtm2, "icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and JOB_NO='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "NET_PRODN");
                                        dtm.Rows[i]["REJ"] = fgen.seek_iname_dt(dtm2, "icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and JOB_NO='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "REJ");
                                    }
                                }
                                else
                                {
                                    if (dtm1.Rows.Count > 0)
                                    {
                                        dtm.Rows[i]["NET_PRODN"] = fgen.seek_iname_dt(dtm1, "STAGE='" + dtm.Rows[i]["TYPE1"].ToString().Trim() + "' and icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and job_no='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "NET_PRODN");
                                        dtm.Rows[i]["REJ"] = fgen.seek_iname_dt(dtm1, "STAGE='" + dtm.Rows[i]["TYPE1"].ToString().Trim() + "' and icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and job_no='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "REJ");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        mq0 = "";
                        mq0 = "SELECT '-' as STAGE,'-' as TYPE1,'-' as icode,'-' as srno,'-' as NET_PRODN,'-' as REJ from dual";
                        dtm = new DataTable();
                        dtm = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    }
                    //============
                    dtm.TableName = "type1";
                    ds.Tables.Add(dtm);

                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_jobCard", "std_jobCard", ds, "");
                    }
                    #endregion
                }
                break;

            case "F30111":
                #region INW INSP. RPT
                SQuery = "SELECT 'Inward Inspection Report' AS HEADER , F.ANAME,I.INAME,I.CPARTNO AS ICPARTNO,A.* FROM INSPVCH A,FAMST F, ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) and a.branchcd||a.type||trim(a.mrrNUM)||TRIM(a.mrrdate)||trim(a.icode) in ('" + barCode + "') ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_InwardInwReport", "std_InwardInwReport", dsRep, "INW INSP. RPT", "Y");
                }
                else
                {

                }
                #endregion
                break;

            case "F1030": // KRML EXPORT INVOICE
                if (frm_cocd == "KRML")
                {
                    SQuery = fgen.seek_iname(frm_qstr, frm_cocd, "select distinct max(trim(a.desc_)) as aa from ivoucherp a where a.branchcd||a.type||TRIM(A.vchnum)||TO_CHAR(A.vchDATE,'DD/MM/YYYY') = '" + barCode.Substring(0, 20) + "'", "aa");
                    if (SQuery == "" || SQuery == "-" || SQuery == "0")
                    { SQuery = "select a.vchnum as invno,TO_CHAR(a.vchdate,'DD/MM/YYYY') AS INVDATE,A.ACODE AS ACODE,A.ICODE,b.aname,c.iname,c.unit,a.iqtyout as qty,a.irate/a.acpt_ud as rate,a.iqtyout *(a.irate/a.acpt_ud) as iamount ,trim(c.cpartno) as partno,b.payterm,b.payment,A.FaBTYPE,a.finvno,is_number(a.btchno) as btchno,a.iweight,b.ADDR1,b.ADDR2,b.ADDR3,b.ADDR4,a.acpt_ud as ex_rate from ivoucherp a , famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||TRIM(A.vchnum)||TO_CHAR(A.vchDATE,'DD/MM/YYYY') = '" + barCode.Substring(0, 20) + "'"; }
                    else
                    { SQuery = "select invno,invdate,max(naration) as unit,max(desc_) as iname,acode,aname,sum(iamount) as iamount,sum(iamount) as rate,round(sum(is_number(btchno)),3) as btchno,1 as qty,round(sum(iweight),3) as iweight,payterm,payment,FaBTYPE,finvno,ADDR1,ADDR2,ADDR3,ADDR4,acpt_ud from (select a.vchnum as invno,TO_CHAR(a.vchdate,'DD/MM/YYYY') AS INVDATE,trim(nvl(a.naration,'-')) as naration,trim(nvl(a.desc_,'-')) as desc_,A.ACODE AS ACODE,A.ICODE,b.aname,c.iname,c.unit,a.iqtyout as qty,a.irate/a.acpt_ud as rate,a.iqtyout * (a.irate/a.acpt_ud ) as iamount ,trim(c.cpartno) as partno,b.payterm,b.payment,A.FaBTYPE,a.finvno,a.btchno,a.iweight,b.ADDR1,b.ADDR2,b.ADDR3,b.ADDR4,a.acpt_ud  from ivoucherp a , famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||TRIM(A.vchnum)||TO_CHAR(A.vchDATE,'DD/MM/YYYY') = '" + barCode.Substring(0, 20) + "') group by invno,invdate,acode,aname,payterm,payment,FaBTYPE,finvno,ADDR1,ADDR2,ADDR3,ADDR4,acpt_ud "; }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dsRep = new DataSet();
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);

                        SQuery = "select a.invno AS Hinvno,TO_CHAR(a.invdate,'DD/MM/YYYY') AS Hinvdate,a.ship2,a.ship3,a.ship4,a.ship5,a.lbnetwt,a.lbgrswt,a.exprmk1,a.exprmk2,a.exprmk3,a.exprmk4,a.exprmk5,a.addl1,a.addl2,a.addl3,a.addl4,a.addl5,a.tmaddl1,a.tmaddl2,a.tmaddl3,a.addl6,a.remark as packrem,trim(s.ANAME) AS nconsign ,trim(s.ADDR1) as ndaddr1,trim(s.ADDR2) as ndaddr2,trim(s.ADDR3) as ndaddr3,trim(s.ADDR4) as ndaddr4,trim(s.email) AS NEMAIL,trim(s.mobile) AS NTELNUM,trim(s.fax) AS NFAX,trim(s.exc_num) as nIEC, trim(s.person) as nperson, trim(girno) as panno,trim(exc_num) as eximno from hundi A left join csmst s on trim(a.ship3)=trim(s.acode) where a.branchcd='" + frm_mbr + "' and a.type='IV' and a.acode='" + dt.Rows[0]["ACODE"].ToString().Trim() + "' and trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')='" + barCode.Substring(4, 16) + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        SQuery = "select pono,to_char(podate,'dd/mm/yyyy') as podate,mo_vehi,no_bdls,stform_no,ins_co,curren,cscode,acode,lc_dtl,pi_dtl,freight,advrcvd,dlv_terms,vehi_fitno,retention,amt_rea,destin,destcount,rg23c as insurance_no,mcomment,insp_amt from salep where branchcd||type||TRIM(vchnum)||TO_CHAR(vchDATE,'DD/MM/YYYY') = '" + barCode.Substring(0, 20) + "'";
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        dt3.TableName = "sale";
                        dsRep.Tables.Add(dt3);

                        SQuery = "SELECT ANAME AS consign ,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 as daddr4,trim(nvl(email,'-')) as demail,trim(nvl(mobile,'-')) as dmobile,fax,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,trim(nvl(girno,'-')) as dpanno,substr(gst_no,0,2) as dstatecode,trim(nvl(exc_num,'-')) as dIEC,trim(nvl(person,'-')) as dperson FROM CSMST WHERE ACODE='" + dt3.Rows[0]["CSCODE"].ToString().Trim() + "'";
                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt1.Rows.Count <= 0)
                        {
                            dt1 = new DataTable();
                            SQuery = "Select '-' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as email,'-' as telnum,'-' as fax,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode,'-' as dmobile,'-' as dperson,'-' as dIEC from dual";
                            dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        }
                        dt1.TableName = "csmst";
                        dsRep.Tables.Add(dt1);

                        if (dt2.Rows.Count < 1)
                        {
                            SQuery = "select '-' as Hinvno,'-' as Hinvdate,'-' as ship2,'-' as ship3,'-' as ship4,'-' as ship5,'-' as lbnetwt,'-' as lbgrswt,'-' as exprmk1,'-' as exprmk2,'-' as exprmk3,'-' as exprmk4,'-' as exprmk5,'-' as addl1,'-' as addl2,'-' as addl3,'-' as addl4,'-' as addl5,'-' as tmaddl1,'-' as tmaddl2,'-' as tmaddl3,'-' as addl6,'-' as nperson,'-' as ntelnum,'-' as niec,'-' as nemail from dual";
                            dt2 = new DataTable();
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        }
                        dt2.TableName = "hundi";
                        dsRep.Tables.Add(dt2);
                        frm_rptName = "KRML_Inv_O";
                        Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "");
                    }
                }
                else if (frm_cocd == "KLAS")
                {
                    header_n = "Export Proforma Invoice";
                    SQuery = "SELECT '" + header_n + "' as header, NVL(b.PERSON,'-') AS PERSON,NVL(B.ANAME,'-') AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,B.EMAIL,B.TELNUM,B.PERSON AS kind_Atn,C.SINAME,C.CINAME AS PROD_CODE,C.MAKER AS COLOR,C.unit as bunit,c.hscode,A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR, a.ordno as ord_no,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ord_dt,a.* FROM SOMASQ A left outer join  csmst d on trim(a.cscode)=trim(d.ACODE),FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "' ORDER BY A.srno";//withiut csmst
                    SQuery = "SELECT '" + header_n + "' as header, NVL(b.PERSON,'-') AS PERSON,NVL(B.ANAME,'-') AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,B.EMAIL,B.TELNUM,B.PERSON AS kind_Atn,C.SINAME,C.CINAME AS PROD_CODE,C.MAKER AS COLOR,C.unit as bunit,c.hscode,A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR, a.ordno as ord_no,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ord_dt,a.*,(case when length(trim(er.pname))>3 then er.pname else er.aname end) as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p FROM SOMASQ A left outer join  csmst er on trim(a.cscode)=trim(er.ACODE),FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||A.ORDNO||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "' ORDER BY A.srno";
                    //(case when length(trim(er.pname))>3 then er.pname else er.aname end) as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dsRep = new DataSet();
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);
                        frm_rptName = "EXP_PERF_INV_KLAS";//31 TYPE FOR KLAS..THIS IS MATCHED
                        Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "", "Y");
                    }
                }
                break;
            case "F10194":
                dt = new DataTable();
                dt = Session["send_dt"] as DataTable;
                DataTable dtcopy = dt.Copy();
                DataColumn dc1 = new DataColumn();
                dc1.ColumnName = "header";
                dc1.DefaultValue = "WIP VALUATION REPORT";
                dtcopy.Columns.Add(dc1);
                dsRep = new DataSet();
                dtcopy.TableName = "Prepcur";
                dsRep.Tables.Add(dtcopy);
                frm_rptName = "std_wip_valuation";
                Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "");
                break;
            case "F10184":
                dt = new DataTable();
                dt = Session["send_dt"] as DataTable;
                DataTable dtcopy1 = dt.Copy();
                DataColumn dc2 = new DataColumn();
                dc2.ColumnName = "header";
                dc2.DefaultValue = "FG VALUATION REPORT";
                dtcopy1.Columns.Add(dc2);
                dsRep = new DataSet();
                dtcopy1.TableName = "Prepcur";
                dsRep.Tables.Add(dtcopy1);
                frm_rptName = "std_wip_valuation";
                Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "");
                break;


            //---------------------------
            case "JOB_RPT"://this jobcard only for CRP

                break;

            case "F1031":
                scode = scode.Replace(";", "");
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                if (scode.Length > 20)
                {
                    sname = scode.Substring(4, 6);
                    if (scode.Length > 20)
                        sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                    else sname = "'" + sname + "'" + " and " + "'" + sname + "'";

                    scode = "a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' AND TRIM(a.vchnum) BETWEEN " + sname + " AND A.VCHDATE  " + xprdRange + " ";
                }
                else scode = "a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')='" + scode + "'";

                dtm = new DataTable();
                dtm.Columns.Add("FSTR", typeof(string));
                dtm.Columns.Add("VCHNUM", typeof(string));
                dtm.Columns.Add("VCHDATE", typeof(string));
                dtm.Columns.Add(new DataColumn("img1_desc", typeof(string)));
                dtm.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
                dtm.Columns.Add(new DataColumn("img2_desc", typeof(string)));
                dtm.Columns.Add(new DataColumn("img2", typeof(System.Byte[])));
                dtm.Columns.Add(new DataColumn("img3_desc", typeof(string)));
                dtm.Columns.Add(new DataColumn("img3", typeof(System.Byte[])));
                dtm.Columns.Add(new DataColumn("img4_desc", typeof(string)));
                dtm.Columns.Add(new DataColumn("img4", typeof(System.Byte[])));
                dtm.Columns.Add("REVISION_NO", typeof(string));
                dtm.Columns.Add(new DataColumn("img5_desc", typeof(string)));
                dtm.Columns.Add(new DataColumn("img5", typeof(System.Byte[])));
                dtm.Columns.Add("DELIVERY_LOC", typeof(string));

                mq0 = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,a.iqtyout,a.exc_57f4,f.vencode,i.packsize,i.purchased,f.addr1,f.addr2,f.addr3,f.addr4 from ivoucher a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and " + scode + " and nvl(a.iqtyout,0)>0";//by yogita
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);

                mq1 = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.pono,to_char(a.podate,'dd/mm/yyyy') as podate,a.cscode,b.addr1,b.addr2,b.addr3,b.addr4 from sale a left join csmst b on trim(a.cscode)=trim(b.acode) where " + scode + ""; //by yogita
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                string path1 = "", path2 = "", path3 = "", path4 = "", path5 = "";
                mq5 = ""; string mq6 = "", mq7 = "", mq8 = "";
                if (dt.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dt);
                    dt2 = new DataTable();
                    dt2 = view1.ToTable(true, "fstr");
                    foreach (DataRow dr in dt2.Rows)
                    {
                        DataView view2 = new DataView(dt, "fstr='" + dr["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt3 = new DataTable();
                        dt3 = view1.ToTable();
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            db = Math.Round(fgen.make_double(dt3.Rows[i]["iqtyout"].ToString().Trim()) / fgen.make_double(dt3.Rows[i]["packsize"].ToString().Trim()), 0);
                            db = fgen.make_double(dt3.Rows[i]["iqtyout"].ToString().Trim()) / fgen.make_double(dt3.Rows[i]["packsize"].ToString().Trim());
                            string[] boxes = db.ToString().Split('.');
                            db3 = fgen.make_double(dt3.Rows[i]["packsize"].ToString().Trim()) * fgen.make_double(boxes[0].ToString());
                            db4 = fgen.make_double(dt3.Rows[i]["iqtyout"].ToString().Trim()) - db3;
                            db5 = fgen.make_double(boxes[0].ToString());
                            if (fgen.make_double(dt3.Rows[i]["packsize"].ToString().Trim()) > 0)
                            {
                                for (int k = 0; k < db5; k++)
                                {
                                    #region
                                    db1 = 0; db2 = 0;
                                    dr1 = dtm.NewRow();
                                    dr1["fstr"] = dt3.Rows[i]["fstr"].ToString().Trim();
                                    dr1["vchnum"] = dt3.Rows[i]["vchnum"].ToString().Trim();
                                    dr1["vchdate"] = dt3.Rows[i]["vchdate"].ToString().Trim();
                                    mq2 = dt3.Rows[i]["exc_57f4"].ToString().Trim();
                                    db1 = fgen.make_double(dt3.Rows[i]["packsize"].ToString().Trim());
                                    db2 = fgen.make_double(dt3.Rows[i]["iqtyout"].ToString().Trim());
                                    mq3 = db1.ToString();
                                    mq4 = fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "pono");
                                    mq5 = dt3.Rows[i]["vencode"].ToString().Trim();
                                    dr1["revision_no"] = "";//revision... new field will be given in item master
                                    mq6 = dt3.Rows[i]["vchnum"].ToString().Trim() + dt3.Rows[i]["icode"].ToString().Trim();
                                    mq7 = fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "cscode");
                                    if (mq7.Length <= 1)
                                    {
                                        mq8 = dt3.Rows[i]["addr1"].ToString().Trim() + " " + dt3.Rows[i]["addr2"].ToString().Trim() + " " + dt3.Rows[i]["addr3"].ToString().Trim() + " " + dt3.Rows[i]["addr4"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq8 = fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr1") + " " + fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr2") + " " + fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr3") + " " + fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr4");
                                    }
                                    dr1["delivery_loc"] = mq8;

                                    path1 = Server.MapPath(@"~\tej-base\BarCode\" + mq2.Trim().Replace("*", "").Replace("/", "") + "1" + ".png");
                                    path2 = Server.MapPath(@"~\tej-base\BarCode\" + mq3.Trim().Replace("*", "").Replace("/", "") + "2" + ".png");
                                    path3 = Server.MapPath(@"~\tej-base\BarCode\" + mq4.Trim().Replace("*", "").Replace("/", "") + "3" + ".png");
                                    path4 = Server.MapPath(@"~\tej-base\BarCode\" + mq5.Trim().Replace("*", "").Replace("/", "") + "4" + ".png");
                                    path5 = Server.MapPath(@"~\tej-base\BarCode\" + mq6.Trim().Replace("*", "").Replace("/", "") + "5" + ".png");

                                    del_file(path1);
                                    del_file(path2);
                                    del_file(path3);
                                    del_file(path4);
                                    del_file(path5);

                                    fgen.prnt_Code128bar(frm_cocd, mq2, mq2.Replace("/", "") + "1" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq3, mq3.Replace("/", "") + "2" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq4, mq4.Replace("/", "") + "3" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq5, mq5.Replace("/", "") + "4" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq6, mq6.Replace("/", "") + "5" + ".png");

                                    FilStr = new FileStream(path1, FileMode.Open);
                                    BinRed = new BinaryReader(FilStr);

                                    FilStr1 = new FileStream(path2, FileMode.Open);
                                    BinRed1 = new BinaryReader(FilStr1);

                                    FilStr2 = new FileStream(path3, FileMode.Open);
                                    BinRed2 = new BinaryReader(FilStr2);

                                    FilStr3 = new FileStream(path4, FileMode.Open);
                                    BinRed3 = new BinaryReader(FilStr3);

                                    FilStr4 = new FileStream(path5, FileMode.Open);
                                    BinRed4 = new BinaryReader(FilStr4);

                                    dr1["img1_desc"] = mq2.Trim();
                                    dr1["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                    FilStr.Dispose();

                                    dr1["img2_desc"] = mq3.Trim();
                                    dr1["img2"] = BinRed1.ReadBytes((int)BinRed1.BaseStream.Length);
                                    FilStr1.Dispose();

                                    dr1["img3_desc"] = mq4.Trim();
                                    dr1["img3"] = BinRed2.ReadBytes((int)BinRed2.BaseStream.Length);
                                    FilStr2.Dispose();

                                    dr1["img4_desc"] = mq5.Trim();
                                    dr1["img4"] = BinRed3.ReadBytes((int)BinRed3.BaseStream.Length);
                                    FilStr3.Dispose();

                                    dr1["img5_desc"] = mq6.Trim();
                                    dr1["img5"] = BinRed4.ReadBytes((int)BinRed4.BaseStream.Length);
                                    FilStr4.Dispose();

                                    FilStr.Close();
                                    BinRed.Close();
                                    FilStr1.Close();
                                    BinRed1.Close();
                                    FilStr2.Close();
                                    BinRed2.Close();
                                    FilStr3.Close();
                                    BinRed3.Close();
                                    FilStr4.Close();
                                    BinRed4.Close();
                                    dtm.Rows.Add(dr1);
                                    #endregion
                                }
                                if (db4 > 0)
                                {
                                    #region
                                    dr1 = dtm.NewRow();
                                    dr1["fstr"] = dt3.Rows[i]["fstr"].ToString().Trim();
                                    dr1["vchnum"] = dt3.Rows[i]["vchnum"].ToString().Trim();
                                    dr1["vchdate"] = dt3.Rows[i]["vchdate"].ToString().Trim();
                                    mq2 = dt3.Rows[i]["exc_57f4"].ToString().Trim();
                                    mq3 = db4.ToString();
                                    mq4 = fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "pono");
                                    mq5 = dt3.Rows[i]["vencode"].ToString().Trim();
                                    dr1["revision_no"] = "";//revision... new field will be given in item master
                                    mq6 = dt3.Rows[i]["vchnum"].ToString().Trim() + dt3.Rows[i]["icode"].ToString().Trim();
                                    mq7 = fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "cscode");
                                    if (mq7.Length <= 1)
                                    {
                                        mq8 = dt3.Rows[i]["addr1"].ToString().Trim() + " " + dt3.Rows[i]["addr2"].ToString().Trim() + " " + dt3.Rows[i]["addr3"].ToString().Trim() + " " + dt3.Rows[i]["addr4"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq8 = fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr1") + " " + fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr2") + " " + fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr3") + " " + fgen.seek_iname_dt(dt1, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "' and cscode='" + mq7 + "'", "addr4");
                                    }
                                    dr1["delivery_loc"] = mq8;

                                    path1 = Server.MapPath(@"~\tej-base\BarCode\" + mq2.Trim().Replace("*", "").Replace("/", "") + "1" + ".png");
                                    path2 = Server.MapPath(@"~\tej-base\BarCode\" + mq3.Trim().Replace("*", "").Replace("/", "") + "2" + ".png");
                                    path3 = Server.MapPath(@"~\tej-base\BarCode\" + mq4.Trim().Replace("*", "").Replace("/", "") + "3" + ".png");
                                    path4 = Server.MapPath(@"~\tej-base\BarCode\" + mq5.Trim().Replace("*", "").Replace("/", "") + "4" + ".png");
                                    path5 = Server.MapPath(@"~\tej-base\BarCode\" + mq6.Trim().Replace("*", "").Replace("/", "") + "5" + ".png");

                                    del_file(path1);
                                    del_file(path2);
                                    del_file(path3);
                                    del_file(path4);
                                    del_file(path5);

                                    fgen.prnt_Code128bar(frm_cocd, mq2, mq2.Replace("/", "") + "1" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq3, mq3.Replace("/", "") + "2" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq4, mq4.Replace("/", "") + "3" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq5, mq5.Replace("/", "") + "4" + ".png");
                                    fgen.prnt_Code128bar(frm_cocd, mq6, mq6.Replace("/", "") + "5" + ".png");

                                    FilStr = new FileStream(path1, FileMode.Open);
                                    BinRed = new BinaryReader(FilStr);

                                    FilStr1 = new FileStream(path2, FileMode.Open);
                                    BinRed1 = new BinaryReader(FilStr1);

                                    FilStr2 = new FileStream(path3, FileMode.Open);
                                    BinRed2 = new BinaryReader(FilStr2);

                                    FilStr3 = new FileStream(path4, FileMode.Open);
                                    BinRed3 = new BinaryReader(FilStr3);

                                    FilStr4 = new FileStream(path5, FileMode.Open);
                                    BinRed4 = new BinaryReader(FilStr4);

                                    dr1["img1_desc"] = mq2.Trim();
                                    dr1["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                    FilStr.Dispose();

                                    dr1["img2_desc"] = mq3.Trim();
                                    dr1["img2"] = BinRed1.ReadBytes((int)BinRed1.BaseStream.Length);
                                    FilStr1.Dispose();

                                    dr1["img3_desc"] = mq4.Trim();
                                    dr1["img3"] = BinRed2.ReadBytes((int)BinRed2.BaseStream.Length);
                                    FilStr2.Dispose();

                                    dr1["img4_desc"] = mq5.Trim();
                                    dr1["img4"] = BinRed3.ReadBytes((int)BinRed3.BaseStream.Length);
                                    FilStr3.Dispose();

                                    dr1["img5_desc"] = mq6.Trim();
                                    dr1["img5"] = BinRed4.ReadBytes((int)BinRed4.BaseStream.Length);
                                    FilStr4.Dispose();

                                    FilStr.Close();
                                    BinRed.Close();
                                    FilStr1.Close();
                                    BinRed1.Close();
                                    FilStr2.Close();
                                    BinRed2.Close();
                                    FilStr3.Close();
                                    BinRed3.Close();
                                    FilStr4.Close();
                                    BinRed4.Close();
                                    dtm.Rows.Add(dr1);
                                    #endregion
                                }
                            }
                        }
                    }
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_sale_barcode", "std_sale_barcode", dsRep, header_n);
                }
                break;
            case "F1032":
                # region Vendor GST Debit / Credit Note
                scode = scode.Replace(";", "");
                if (scode.Length > 20)
                {
                    frm_mbr = scode.Substring(0, 2);
                    frm_vty = scode.Substring(2, 2);
                    sname = scode.Substring(4, 6);
                    if (scode.Length > 20)
                        sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";
                    else sname = "'" + sname + "'" + " and " + "'" + sname + "'";

                    scode = "a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '" + frm_vty + "' AND TRIM(a.vchnum) BETWEEN " + sname + " AND A.VCHDATE  " + xprdRange + " ";
                }
                else scode = "a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')='" + scode + "'";
                header_n = "GENERAL CREDIT NOTE";
                if (frm_vty == "31") header_n = "GENERAL DEBIT NOTE";
                if (frm_vty == null)
                {
                    if (barCode.Substring(2, 2) == "31") header_n = "GENERAL DEBIT NOTE";
                }
                SQuery = "SELECT '" + header_n + "' AS HEADER,(case when trim(nvl(f.pname,'-'))!='-' then f.pname else F.ANAME end) as aname,F.ADDR1 AS FDDR1,F.ADDR2 AS FADDR2,F.ADDR3 AS FADDR3,F.STATEN AS FSTATE,SUBSTR(F.GST_NO,0,2) AS FSTATECODE,F.GIRNO AS FGIRNO,F.GST_NO AS FGST_NO,(case when length(trim(i.ciname))>5 then i.ciname else I.INAME end) as iname,I.UNIT AS IUNIT,I.HSCODE,A.*,F.VENCODE,I.CPARTNO FROM IVOUCHERP A,FAMST F ,ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND " + scode + " ORDER BY A.VCHNUM";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    repCount = 2;
                    if (frm_cocd == "KLAS") repCount = 3;
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Vch_Rpt", "std_Vch_RptVen", dsRep, "std_Vch_Rpt", "Y");
                }
                #endregion
                break;
            case "95040":
                SQuery = "Select distinct (case when nvl(trim(a.mode_tpt),'-')='-' then c.cpartno else a.mode_tpt end) as vi from ivoucher a,item c where trim(a.icode)=trim(c.icode) and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')||TRIM(A.INVNO) in (" + barCode + ") ";
                if (barCode.Contains("'")) { }
                else barCode = "'" + barCode + "'";
                SQuery = "Select distinct (case when nvl(trim(a.mode_tpt),'-')='-' then c.cpartno else a.mode_tpt end) as vi,a.icode,A.DESC_,trim(C.cpartno)||'/'||trim(C.cdrgno) as pname,trim(C.cpartno) as pname1,c.mat10,trim(c.iname) as iname,c.ciname,C.wt_RR,TRIM(C.BINNO)||'/'||TRIM(C.SALLOY) AS BINNO,C.CDRGNO,a.vchnum as bill_no,to_chaR(a.vchdate,'dd/mm/yyyy') as bill_Dt,c.cpartno,c.unit,(a.iqtyin) AS iqtyin,c.maker,C.WT_NET,A.INVNO,A.BTCHNO,a.EXC_57F4,a.srno as thru,a.finvno,a.binno as ibinno,a.mode_tpt,a.freight from ivoucher a,item c where trim(a.icode)=trim(c.icode) and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')||TRIM(A.INVNO) in (" + barCode + ") order by a.vchnum";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt = new DataTable("barcode");
                dt.Columns.Add(new DataColumn("img1_desc", typeof(string)));
                dt.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
                DsImages.Tables.Add(dt);
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    value1 = dt1.Rows[i]["vi"].ToString().Trim().Replace("/", "");
                    fpath = Server.MapPath(@"BarCode\" + value1.Trim().Replace("*", "").Replace("/", "") + i + ".png");
                    del_file(fpath);
                    if (frm_cocd == "KLAS")
                    {
                        value2 = dt1.Rows[i]["binno"].ToString().Trim();
                        if (dt1.Rows[i]["mat10"].ToString().Trim().Length > 4)
                        {
                            value2 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(C.BINNO)||'/'||trim(C.salloy) as pname from item c where trim(c.icode)='" + dt1.Rows[i]["mat10"].ToString().Trim() + "'", "pname");
                            value3 = dt1.Rows[i]["pname1"].ToString().Trim() + "/" + fgen.seek_iname(frm_qstr, frm_cocd, "select trim(C.cdrgno)||'/'||'" + value2 + "'||'/'||trim(C.maker) as pname from item c where trim(c.icode)='" + dt1.Rows[i]["mat10"].ToString().Trim() + "'", "pname");

                            value2 = dt1.Rows[i]["binno"].ToString().Trim();
                        }
                        else value3 = dt1.Rows[i]["Pname"].ToString().Trim();

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT COL4 FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' and TYPE='69' AND TRIM(ICODE)='" + dt1.Rows[i]["icode"].ToString().Trim() + "' and upper(col4) not like 'KPL%' order by srno");
                        int pv = 0;
                        if (dt.Rows.Count > 0) value2 = "";
                        foreach (DataRow drIcode in dt.Rows)
                        {
                            if (drIcode["col4"].ToString().Trim().Length > 1 && pv < 2)
                            {
                                if (value2.Length > 0) value2 = value2 + "/" + drIcode["col4"].ToString().Trim();
                                else value2 = drIcode["col4"].ToString().Trim();
                            }
                            pv++;
                        }

                        value3 = value3.Trim().Replace("-/", "");
                        value1 = "" + value3;
                        value1 = value1 + ", " + dt1.Rows[i]["desc_"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["ciname"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["maker"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["cdrgno"].ToString().Trim();
                        value1 = value1 + ", " + value2;
                        value1 = value1 + ", " + dt1.Rows[i]["wt_rr"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["wt_net"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["iqtyin"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["ibinno"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["invno"].ToString().Trim();
                        value1 = value1 + ", " + dt1.Rows[i]["bill_Dt"].ToString().Trim();
                        //value1 = value1 + ", " + dt1.Rows[i]["btchno"].ToString().Trim();

                        fpath = Server.MapPath(@"BarCode\a" + i + ".png");
                        fgen.prnt_QRbar(frm_cocd, value1, "a" + i + ".png");
                    }
                    else fgen.prnt_1Dbar(frm_cocd, dt1.Rows[i]["vi"].ToString().Trim(), value1.Replace("*", "").Replace("/", "") + i + ".png");
                    DataRow dr = this.DsImages.Tables["barcode"].NewRow();
                    FilStr = new FileStream(fpath, FileMode.Open);
                    BinRed = new BinaryReader(FilStr);

                    dr["img1_desc"] = value1.Trim();
                    dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                    this.DsImages.Tables["barcode"].Rows.Add(dr);
                    FilStr.Close();
                    BinRed.Close();
                }

                if (frm_cocd == "KLAS") SQuery = "Select distinct a.icode,A.DESC_, '" + value3 + "' as pname,c.ciname,C.wt_RR,'" + value2 + "' AS BINNO,C.CDRGNO,a.vchnum as bill_no,to_chaR(a.vchdate,'dd/mm/yyyy') as bill_Dt,c.cpartno,c.unit,(a.iqtyin) AS iqtyin,c.maker,(case when nvl(trim(a.gsm),'0')>0 then a.gsm else C.WT_NET end) as WT_NET,A.INVNO,A.BTCHNO,a.EXC_57F4,a.srno as thru,a.finvno,a.binno as ibinno,a.mode_tpt,a.freight from ivoucher a,item c where trim(a.icode)=trim(c.icode) and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')||TRIM(A.INVNO) in (" + barCode + ") order by a.vchnum";
                else SQuery = "Select distinct a.icode,c.iname,c.ciname,C.wt_RR,a.vchnum as bill_no,to_chaR(a.vchdate,'dd/mm/yyyy') as bill_Dt,c.salloy,c.cpartno,c.unit,round(a.iqtyin/0.915)||' YDS' AS iqtyin,c.maker,C.WT_NET,A.INVNO,A.BTCHNO,a.EXC_57F4,a.thru,a.finvno,a.form31,a.mode_tpt,a.freight from ivoucher a,item c where trim(a.icode)=trim(c.icode) and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')||TRIM(A.INVNO) in (" + barCode + ") order by a.vchnum";
                value3 = "";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.TableName = "Prepcur";
                DsImages.Tables.Add(dt);
                Print_Report_BYDS(frm_cocd, frm_mbr, "klas_stk", "klas_stk", DsImages, "");
                break;
            #region KLAS
            case "95040A":
                string year = frm_myear;
                string jroll = barCode.Trim().Substring(20, barCode.Trim().Length - 20);
                //if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
                //else branch_Cd = "branchcd='" + mbr + "'";
                string cDT1 = frm_cDt1;
                string cDT2 = frm_cDt1;
                dtm = new DataTable();
                dtm.Columns.Add("JumboRollNo", typeof(string));
                dtm.Columns.Add("ProductName", typeof(string));
                dtm.Columns.Add("Thickness", typeof(string));
                dtm.Columns.Add("Weight_kg", typeof(string));
                dtm.Columns.Add("Customer", typeof(string));
                dtm.Columns.Add("Date", typeof(string));
                dtm.Columns.Add("Color", typeof(string));
                dtm.Columns.Add("Line", typeof(string));
                dtm.Columns.Add("Shift", typeof(string));
                dtm.Columns.Add("ProcessName", typeof(string));
                dtm.Columns.Add("Machine", typeof(string));
                dtm.Columns.Add("ItemtobeUsed", typeof(string));
                dtm.Columns.Add("InkColor", typeof(string));
                dtm.Columns.Add("CompletedInQty", typeof(double));
                dtm.Columns.Add("CompletedOutQty", typeof(double));
                dtm.Columns.Add("SpecialInstructions", typeof(string));
                dtm.Columns.Add("Quantity_Mtrs", typeof(string));
                dtm.Columns.Add("Srno", typeof(string));
                dtm.Columns.Add("GRP", typeof(string));
                dtm.Columns.Add("icode", typeof(string));
                dtm.Columns.Add("barcode", typeof(Byte[]));
                dtm.Columns.Add("barcodeDesc", typeof(string));
                dtm.Columns.Add("Agrp", typeof(string));
                mq2 = "SELECT A.ACODE,F.ANAME,A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT FROM SOMAS A,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE'4%' AND ORDDT BETWEEN TO_DATE('01/04/2017','DD/MM/YYYY') AND TO_DATE('" + cDT2 + "','DD/MM/YYYY')";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                mq0 = "SELECT TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||A.COL6 AS GRP,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ENQNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS ENQDT,SUM(is_number(A.COL3)) AS WEIGHT,SUM(is_number(A.COL4)) AS MTR,A.COL6 AS ROLL,A.COL25 AS MAC,A.COL23 AS SHIFT,I.INAME,I.WT_RR,I.MAKER,a.icode FROM COSTESTIMATE A,ITEM I  WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='40' and a.vchdate > to_Date('01/04/2008','dd/mm/yyyy') AND A.COL6 IN( '" + jroll + "') GROUP BY TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),A.ACODE,A.ENQNO,A.ENQDT,A.COL6,A.COL25,A.COL23,I.INAME,I.WT_RR,I.MAKER,a.icode ORDER BY VCHDATE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count == 0)
                {
                    mq0 = "SELECT TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||A.COL6 AS GRP,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ENQNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS ENQDT,SUM(is_number(A.COL3)) AS WEIGHT,SUM(is_number(A.COL4)) AS MTR,A.COL6 AS ROLL,A.COL25 AS MAC,A.COL23 AS SHIFT,I.INAME,I.WT_RR,I.MAKER,a.icode FROM COSTESTIMATEK A,ITEM I  WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='40' and a.vchdate > to_Date('01/04/2008','dd/mm/yyyy') AND A.COL6 IN( '" + jroll + "') GROUP BY TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),A.ACODE,A.ENQNO,A.ENQDT,A.COL6,A.COL25,A.COL23,I.INAME,I.WT_RR,I.MAKER,a.icode ORDER BY VCHDATE";
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                }
                string ticode = "";
                if (dt.Rows.Count > 0) ticode = dt.Rows[0]["icode"].ToString().Trim();

                mq1 = "SELECT VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,COL1 AS PROCESS,COL2 AS MAC,COL3 AS MODULE,COL4 AS EQUIP,COL5 AS COLOR,Srno,TRIM(CPARTNO) AS AGRP FROM INSPMST WHERE TYPE='69' and trim(icode)='" + ticode + "' ORDER BY SRNO";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                if (dt1.Rows.Count < 1)
                {
                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(mat10) as icode from item where trim(icode)='" + ticode + "'", "icode");
                    mq1 = "SELECT VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,'" + ticode.Trim() + "' AS ICODE,COL1 AS PROCESS,COL2 AS MAC,COL3 AS MODULE,COL4 AS EQUIP,COL5 AS COLOR,Srno,TRIM(CPARTNO) AS AGRP FROM INSPMST WHERE TYPE='69' and trim(icode)='" + mq0 + "' ORDER BY SRNO";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                }


                int i0 = 0;
                dr1 = null; i0 = 0;
                if (dt.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dt);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1.ToTable(true, "VCHDATE", "ICODE", "ROLL");
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataView view1im = new DataView(dt, "VCHDATE='" + dr0["VCHDATE"].ToString().Trim() + "' AND ICODE='" + dr0["ICODE"].ToString().Trim() + "' AND ROLL='" + dr0["ROLL"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        DataTable dticode = new DataTable();
                        dticode = view1im.ToTable();
                        DataTable dticode2 = new DataTable();
                        if (dt1.Rows.Count > 0)
                        {
                            DataView view2 = new DataView(dt1, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode2 = view2.ToTable();
                        }
                        if (dticode2.Rows.Count > 0)
                        {
                            int i = 0;
                            for (int k = 0; k < dticode2.Rows.Count; k++)
                            {
                                if (dticode2.Rows[k]["Equip"].ToString().Trim().Length > 1)
                                {
                                    dr1 = dtm.NewRow();
                                    dr1["icode"] = dticode.Rows[i]["ICODE"].ToString().Trim();
                                    dr1["GRP"] = dticode.Rows[i]["GRP"].ToString().Trim();
                                    dr1["DATE"] = dticode.Rows[i]["VCHDATE"].ToString().Trim();
                                    dr1["ProductName"] = dticode.Rows[i]["INAME"].ToString().Trim();
                                    dr1["Thickness"] = dticode.Rows[i]["WT_RR"].ToString().Trim();
                                    dr1["Weight_kg"] = dticode.Rows[i]["WEIGHT"].ToString().Trim();
                                    if (dt2.Rows.Count > 0)
                                    {
                                        dr1["Customer"] = fgen.seek_iname_dt(dt2, "ordno='" + dticode.Rows[i]["enqno"].ToString().Trim() + "' and orddt='" + dticode.Rows[i]["enqdt"].ToString().Trim() + "'", "aname");
                                    }
                                    dr1["Color"] = dticode.Rows[i]["MAKER"].ToString().Trim();
                                    dr1["Line"] = dticode.Rows[i]["MAC"].ToString().Trim();
                                    dr1["Shift"] = dticode.Rows[i]["SHIFT"].ToString().Trim();
                                    dr1["JumboRollNo"] = dticode.Rows[i]["ROLL"].ToString().Trim();
                                    dr1["Quantity_Mtrs"] = dticode.Rows[i]["MTR"].ToString().Trim();
                                    dr1["ProcessName"] = dticode2.Rows[k]["module"].ToString().Trim();
                                    dr1["Machine"] = dticode2.Rows[k]["Mac"].ToString().Trim();
                                    dr1["ItemtobeUsed"] = dticode2.Rows[k]["Equip"].ToString().Trim();
                                    dr1["InkColor"] = dticode2.Rows[k]["Color"].ToString().Trim();
                                    dr1["CompletedOutQty"] = 0;
                                    dr1["CompletedInQty"] = 0;
                                    dr1["SpecialInstructions"] = "";
                                    dr1["Srno"] = dticode2.Rows[k]["Srno"].ToString().Trim();
                                    dr1["Agrp"] = dticode2.Rows[k]["Agrp"].ToString().Trim();
                                    dtm.Rows.Add(dr1);

                                    value1 = dticode.Rows[i]["VCHDATE"].ToString().Trim() + dr0["ICODE"].ToString().Trim() + dticode.Rows[i]["ROLL"].ToString().Trim();
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                //if (dticode2.Rows[k]["Equip"].ToString().Trim().Length > 0)
                                {
                                    dr1 = dtm.NewRow();
                                    dr1["icode"] = dticode.Rows[i]["ICODE"].ToString().Trim();
                                    dr1["GRP"] = dticode.Rows[i]["GRP"].ToString().Trim();
                                    dr1["DATE"] = dticode.Rows[i]["VCHDATE"].ToString().Trim();
                                    dr1["ProductName"] = dticode.Rows[i]["INAME"].ToString().Trim();
                                    dr1["Thickness"] = dticode.Rows[i]["WT_RR"].ToString().Trim();
                                    dr1["Weight_kg"] = dticode.Rows[i]["WEIGHT"].ToString().Trim();
                                    if (dt2.Rows.Count > 0)
                                    {
                                        dr1["Customer"] = fgen.seek_iname_dt(dt2, "ordno='" + dticode.Rows[i]["enqno"].ToString().Trim() + "' and orddt='" + dticode.Rows[i]["enqdt"].ToString().Trim() + "'", "aname");
                                    }
                                    dr1["Color"] = dticode.Rows[i]["MAKER"].ToString().Trim();
                                    dr1["Line"] = dticode.Rows[i]["MAC"].ToString().Trim();
                                    dr1["Shift"] = dticode.Rows[i]["SHIFT"].ToString().Trim();
                                    dr1["JumboRollNo"] = dticode.Rows[i]["ROLL"].ToString().Trim();
                                    dr1["Quantity_Mtrs"] = dticode.Rows[i]["MTR"].ToString().Trim();
                                    dtm.Rows.Add(dr1);

                                    value1 = dticode.Rows[i]["VCHDATE"].ToString().Trim() + dr0["ICODE"].ToString().Trim() + dticode.Rows[i]["ROLL"].ToString().Trim();
                                }
                            }
                        }
                    }
                    fpath = Server.MapPath(@"BarCode\KLAS_STK_" + i0 + ".png");
                    fgen.del_file(fpath);
                    fgen.prnt_QRbar(frm_cocd, value1, "KLAS_STK_" + i0 + ".png");

                    FileStream FilStr = new FileStream(fpath, FileMode.Open);
                    BinaryReader BinRed = new BinaryReader(FilStr);
                    foreach (DataRow drrr in dtm.Rows)
                    {
                        drrr["barcodedesc"] = value1.Trim();
                        drrr["barcode"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                    }
                    FilStr.Close();
                    BinRed.Close();
                    i0++;
                }
                ds = new DataSet();
                dtm.TableName = "Prepcur";
                ds.Tables.Add(dtm);
                Print_Report_BYDS(frm_cocd, frm_mbr, "crptKlasJumboRoll", "crptKlasJumboRoll", ds, "");
                break;
            #endregion
            case "300062":// android btn id
            case "F20234":
                #region
                SQuery = "select * from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + barCode + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("pic", typeof(System.Byte[]));
                dt.Columns.Add("img1", typeof(System.Byte[]));
                dt.Columns.Add("img1_desc", typeof(string));
                fpath = "";
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        col1 = scode;

                        fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);

                        fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");

                        FileStream FilStr;
                        BinaryReader BinRed;

                        FilStr = new FileStream(fpath, FileMode.Open);
                        BinRed = new BinaryReader(FilStr);

                        dr["img1_desc"] = col1.Trim();
                        dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                        FilStr.Close();
                        BinRed.Close();

                        fpath = dr["COL14"].ToString().Trim();
                        try
                        {
                            FilStr = new FileStream(fpath, FileMode.Open);
                            BinRed = new BinaryReader(FilStr);
                            dr["pic"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                            FilStr.Close();
                            BinRed.Close();
                        }
                        catch { }
                    }
                    catch { }
                }
                dt.TableName = "Prepcur";
                dsRep.Tables.Add(dt);
                Print_Report_BYDS(frm_cocd, frm_mbr, "vmrec_tag", "vmrec_tag", dsRep, "");
                #endregion
                break;
        }
    }
    string dscCheck = "N";
    void po()
    {
        if (hfhcid.Value == "F1004")
            dscCheck = "Y";
    }
    void so()
    {
        if (hfhcid.Value == "F1005")
            dscCheck = "Y";
    }
    void invoice()
    {
        if (hfhcid.Value == "F1006" || hfhcid.Value == "F1033")
            dscCheck = "Y";
    }
    void chl()
    {
        if (hfhcid.Value == "F1007" || hfhcid.Value == "F1007A")
            dscCheck = "Y";
    }
    void mrr()
    {
        if (hfhcid.Value == "F1002")
            dscCheck = "Y";
    }
    void drcr()
    {
        if (hfhcid.Value == "F1023" || hfhcid.Value == "F1022" || hfhcid.Value == "F1032")
            dscCheck = "Y";
    }
    void PI()
    {
        if (hfhcid.Value == "F1016")
            dscCheck = "Y";
    }
    void PL()
    {
        if (hfhcid.Value == "F1029")
            dscCheck = "Y";
    }

    bool isDSC_enabled()
    {
        switch (frm_cocd)
        {
            case "STUD":
                invoice();
                po();
                so();
                chl();
                mrr();
                drcr();
                break;
            case "SAIL":
                invoice();
                drcr();
                chl();
                break;
            case "IPP":
                invoice();
                drcr();
                chl();
                break;
            case "CRP":
                invoice();
                drcr();
                break;
            case "MLGI":
                invoice();
                drcr();
                break;
            case "WING":
                invoice();
                drcr();
                break;
            case "AEPL":
                invoice();
                drcr();
                break;
            case "PIPL":
            case "ELEC":
            case "JSGI":
                invoice();
                break;
            case "SAIP":
                invoice();
                chl();
                drcr();
                break;
            case "SFLG":
                invoice();
                drcr();
                break;
            case "SFL1":
                invoice();
                break;
            case "SFL2":
                invoice();
                break;
            case "ATOP":
                invoice();
                break;
            case "DLJM":
                invoice();
                chl();
                break;
            case "UKB":
                invoice();
                break;
            case "BONY":
            case "SFAB":
                invoice();
                break;
            case "MINV":
                invoice();
                drcr();
                break;
            case "YTEC":
                invoice();
                chl();
                drcr();
                break;
            case "KLAS":
                po();
                invoice();
                PI();
                PL();
                chl();
                drcr();
                break;
            case "SDM":
                invoice();
                chl();
                drcr();
                break;
            case "VPAC":
            case "GIPL":
                invoice();
                break;
        }
        if (dscCheck == "Y") return true;
        return false;
    }

    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";
        data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));
        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            if (isDSC_enabled())
            {
                Session["rptfile"] = rptfile;
                Session["data_set"] = data_set;
                printDsc(data_set, rptfile);
            }
            else
            {
                conv_pdf(data_set, rptfile);
            }
        }
        else
        {
        }
        data_set.Dispose();
    }
    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";
        if (addlogo == "Y") data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr, "Y"));
        else data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));
        // for blogo===========ADD FOR IPP
        if (hfhcid.Value == "F1006" || hfhcid.Value == "F1033")
        {
            DataTable dtBLOGO = new DataTable();
            dtBLOGO.Columns.Add(new DataColumn("Blogo_desc", typeof(string)));
            dtBLOGO.Columns.Add(new DataColumn("Blogo", typeof(System.Byte[])));
            dtBLOGO.TableName = "blogo";
            oporrow = dtBLOGO.NewRow();
            if (blogo_opt == "Y")
            {
                FileStream FilStr;
                BinaryReader BinRed;
                fpath = @"c:\TEJ_ERP\logo\blogo_" + co_Cd + "_" + mbr + ".jpg";
                FilStr = new FileStream(fpath, FileMode.Open);
                BinRed = new BinaryReader(FilStr);
                oporrow["Blogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                FilStr.Close();
                BinRed.Close();
            }
            dtBLOGO.Rows.Add(oporrow);
            data_set.Tables.Add(dtBLOGO);//but it is conditionaly added
        }
        ///============for blogo

        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            if (report.ToUpper() == "STD_OUTWARDINSREPORT*") { }
            else if (isDSC_enabled())
            {
                Session["rptfile"] = rptfile;
                Session["data_set"] = data_set;
                printDsc(data_set, rptfile);
            }
            else //{ }
            {
                if (hfhcid.Value == "F25245A" && (fgenMV.Fn_Get_Mvar(frm_qstr, "U_DPRINT") != "Y"))
                {
                    Session["rptfile"] = rptfile;
                    Session["data_set"] = data_set;
                    btnPrintToPrinter.Visible = true;
                }
                else conv_pdf(data_set, rptfile);
            }
        }
        else
        {
        }
        data_set.Dispose();
    }
    void printDefault(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";
        Session["xmlfile"] = xfilepath;
        Session["rptfile"] = rptfile;
        Session["data_set"] = data_set;
        //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('http://localhost:30070/tej-base/dPrint.aspx?STR=ERP@28@ADWA@201700@000002@BVAL@F1006@004000021219/08/2017', null, 'status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no');", true);
        //ScriptManager.RegisterStartupScript(this,typeof(string), "OpenWindow", "window.open('dprint1.aspx','_newtab');", true);
        Response.Redirect("www.google.com");
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
    private ReportDocument GetReportDocumentDP(DataSet rptDS, string rptFileName)
    {
        string repFilePath = Server.MapPath("" + rptFileName + "");
        repDoc.Load(repFilePath);
        repDoc.Refresh();
        repDoc.SetDataSource(rptDS);
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
            if (frm_cocd == "SAIP")
            {
                fpath = path;
            }
            else
            {
                fpath = Server.MapPath(path);
            }

            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }
    //public void del_file(string path)
    //{
    //    try
    //    {
    //        fpath = Server.MapPath(path);
    //        if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
    //    }
    //    catch { }
    //}
    public void openPrintAgain(string comp_code, string frm_mbr, string userID, string formID, string CDT1, string fstr)
    {
        string pageurl = "../tej-base/dprint.aspx?STR=ERP@" + DateTime.Now.ToString("dd") + "@" + comp_code + "@" + CDT1.Substring(6, 4) + frm_mbr + "@" + userID + "@BVAL@" + formID + "@" + fstr + "";
        col1 = pageurl;
        Thread.Sleep(2000);
        Task t1 = Task.Factory.StartNew(Open);
    }
    void Open()
    {
        //Response.Write("<script>window.open('" + col1 + "');</script>");
        Response.Redirect("login.aspx", false);
    }
    void printDsc(DataSet dataSet, string rptFile)
    {
        //string frm_FileName = frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yy").Trim();
        //DataSet ds = dataSet;
        //string rpt = rptFile;
        //DataTable dtDistEntryNo = new DataTable();
        //DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
        //if (hfhcid.Value == "F1006") dtDistEntryNo = dv.ToTable(true, "BRANCHCD", "VCHNUM", "TYPE", "VCHDATE", "FULL_INVNO");
        //else dtDistEntryNo = dv.ToTable(true, "BRANCHCD", "VCHNUM", "TYPE", "VCHDATE");
        //string frm_pdfName = "", cust_pdfname = "";
        //string[] allFiles = new string[dtDistEntryNo.Rows.Count];
        //string tiffPath = @"c:\TEJ_ERP\DSC_pdf\";
        //string filenamePr = "";

        ////left pad
        //int a = 710;
        //// bottom pad
        //int b = 50;

        //col1 = fgen.dscDimension(frm_qstr, frm_cocd, frm_mbr, reportActionCode);

        //if (col1.Split('~')[0].ToString().toDouble() > 0)
        //{
        //    a = fgen.make_int(col1.Split('~')[0].ToString());
        //    b = fgen.make_int(col1.Split('~')[1].ToString());
        //}

        ////width
        //int c = a + 100;
        ////height
        //int d = b + 40;

        //string dscPanNo = "", dscAuthName = "", dscNametoPrint = "";
        //dscAuthName = fgen.dscAuthName(frm_qstr, frm_cocd, frm_mbr, frm_uname);
        //dscNametoPrint = fgen.dscNametoPrint(frm_qstr, frm_cocd, frm_mbr, frm_uname);
        //dscPanNo = fgen.dscPanNo(frm_qstr, frm_cocd, frm_mbr, frm_uname);

        //frm_uname = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT USERNAME FROM EVAS WHERE USERID='" + frm_userid + "'", "USERNAME");

        //if (frm_cocd == "IPP")
        //{
        //    PARTY = ds.Tables[0].Rows[0]["party_"].ToString().Trim();
        //}

        //switch (hfhcid.Value)
        //{
        //    case "F70203":
        //        a = 460;
        //        b = 530;
        //        //width
        //        c = a + 100;
        //        //height
        //        d = b + 50;
        //        tiffPath += "ACC_Pdf\\";
        //        filenamePr = frm_cocd + "_" + "AC" + "_" + frm_mbr;
        //        break;
        //    case "F1002":
        //        a = 460;
        //        b = 450;
        //        //width
        //        c = a + 100;
        //        //height
        //        d = b + 50;
        //        tiffPath += "MRR_Pdf\\";
        //        filenamePr = frm_cocd + "_" + "MRR" + "_" + frm_mbr;
        //        break;
        //    case "F1004":
        //        if (frm_cocd == "STUD" || frm_cocd == "MLGI")
        //        {
        //            a = 710;
        //            b = 54;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //        }
        //        tiffPath += "PO_Pdf\\";
        //        filenamePr = frm_cocd + "_" + "PO" + "_" + frm_mbr;
        //        break;
        //    case "F1005":
        //        if (frm_cocd == "STUD" || frm_cocd == "MLGI")
        //        {
        //            a = 710;
        //            b = 40;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //        }
        //        tiffPath += "SO_Pdf\\";
        //        filenamePr = frm_cocd + "_" + "SO" + "_" + frm_mbr;
        //        if (frm_vty == "4F")
        //        {
        //            a = 460;
        //            b = 33;
        //        }
        //        break;
        //    case "F1006":
        //    case "F1033":
        //        if (frm_cocd == "IPP" || frm_cocd == "SFLG" || frm_cocd == "MLGI") a = 0;
        //        if (a == 0)
        //        {
        //            a = 710;
        //            b = 40;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //            if (frm_cocd == "MLGI") b = 15;
        //        }
        //        if (frm_cocd == "SAIP" || frm_cocd == "GIPL" || (frm_cocd == "IPP" && PARTY.Contains("BAJAJ")))
        //        {
        //            a = 460;
        //            b = 40;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //            if (frm_cocd == "GIPL") d = b + 70;
        //        }
        //        tiffPath += "Inv_Pdf\\";
        //        filenamePr = frm_cocd + "_" + "INV" + "_" + frm_mbr;
        //        if (frm_vty == "4F" && frm_cocd != "STUD" && frm_cocd != "KLAS" && frm_cocd != "ATOP" && frm_cocd != "WING")
        //        {
        //            a = 460;
        //            b = 33;
        //        }
        //        if (frm_vty == "4F" && frm_cocd == "ATOP")
        //        {
        //            a = 710;
        //            b = 50;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //        }
        //        if ((frm_vty == "4F" || frm_vty == "4P") && frm_cocd == "YTEC")
        //        { //DSC NOT SET ASPER ABOVE PARAMETERS 460,33 THATS WHY CHANGE FOR YTEC
        //            a = 350;
        //            b = 10;
        //        }
        //        break;
        //    case "F1007":
        //        //if (frm_cocd == "STUD")
        //        {
        //            a = 710;
        //            b = 40;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //        }
        //        tiffPath += "CHL_Pdf\\";
        //        filenamePr = frm_cocd + "_" + "CHL" + "_" + frm_mbr;
        //        break;
        //    case "F1022":
        //    case "F1023":
        //    case "F1032":
        //        a = 680;
        //        b = 55;
        //        //width
        //        c = a + 100;
        //        //height
        //        d = b + 50;
        //        tiffPath += "ACC_Pdf\\";
        //        filenamePr = frm_cocd + "_" + "AC" + "_" + frm_mbr;
        //        break;
        //    case "F1016":
        //        //if (a == 0)
        //        {
        //            a = 460;
        //            b = 40;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //        }
        //        tiffPath += "PI_PDF\\";
        //        filenamePr = frm_cocd + "_" + "PI" + "_" + frm_mbr;
        //        break;
        //    case "F1029":
        //        //if (a == 0)
        //        {
        //            a = 460;
        //            b = 40;
        //            //width
        //            c = a + 100;
        //            //height
        //            d = b + 50;
        //        }
        //        tiffPath += "PL_PDF\\";
        //        filenamePr = frm_cocd + "_" + "PL" + "_" + frm_mbr;
        //        break;
        //}

        //int k = 0;

        //foreach (DataRow dr in dtDistEntryNo.Rows)
        //{
        //    try
        //    {
        //        if (Session["rptfile"] != null)
        //            frm_rptName = (string)Session["rptfile"];
        //        DataTable newDt = new DataTable();
        //        DataSet newDs = new DataSet();
        //        DataView dvN = new DataView(ds.Tables[0], "VCHNUM='" + dr["vchnum"].ToString().Trim() + "' ", "", DataViewRowState.CurrentRows);
        //        newDt = dvN.ToTable();
        //        newDs.Tables.Add(newDt);

        //        DataRow nedr;
        //        for (int i = 0; i < ds.Tables.Count; i++)
        //        {
        //            if (ds.Tables[i].TableName.ToUpper() != "PREPCUR")
        //            {
        //                newDt = new DataTable();
        //                newDt = ds.Tables[i].Clone();
        //                for (int x = 0; x < ds.Tables[i].Rows.Count; x++)
        //                {
        //                    nedr = newDt.NewRow();
        //                    for (int y = 0; y < ds.Tables[i].Columns.Count; y++)
        //                    {
        //                        nedr[y] = ds.Tables[i].Rows[x][y];
        //                    }
        //                                newDt.Rows.Add(nedr);
        //                }
        //                newDt.TableName = ds.Tables[i].TableName;
        //                newDs.Tables.Add(newDt);
        //            }
        //        }
        //        repDoc = new ReportDocument();
        //        GetReportDocumentDP(newDs, frm_rptName);

        //        frm_pdfName = filenamePr + "_" + dr["TYPE"].ToString().Trim() + "_" + dr["vchnum"].ToString().Trim() + "_" + Convert.ToDateTime(dr["VCHDATE"].ToString().Trim()).ToString("dd_MM_yyyy") + ".pdf";
        //        cust_pdfname = frm_pdfName;
        //        try
        //        {
        //            //cust_pdfname = newDs.Tables[0].Rows[0]["VENCODE"].ToString().Trim() + "_" + dr["vchnum"].ToString().Trim() + "_" + Convert.ToDateTime(dr["VCHDATE"].ToString().Trim()).ToString("ddMMyyyy") + ".pdf";
        //        }
        //        catch { }
        //        frm_FileName = Server.MapPath(@"~\tej-base\xmlfile\" + frm_pdfName);

        //        repDoc.ExportToDisk(ExportFormatType.PortableDocFormat, frm_FileName);
        //        if (hfhcid.Value == "F1029" && frm_cocd == "KLAS")
        //        {
        //            repDoc.ExportToDisk(ExportFormatType.Excel, tiffPath + frm_pdfName.Replace("pdf", "xls"));
        //            //conv_excel(data_set, rptfile);
        //        }
        //        repDoc.Dispose();

        //        FileInfo fi = new FileInfo(frm_FileName);
        //        BinaryReader br = new BinaryReader(fi.OpenRead());

        //        Webtel_e_Sign.Res rr = new Webtel_e_Sign.Res();
        //        Webtel_e_Sign.ESign aa = new Webtel_e_Sign.ESign(ConnInfo.IP, "FIN" + frm_cocd, ConnInfo.nPwd, ConnInfo.srv, "1521", "2");

        //        //-2 for last page
        //        //-1 for every page
        //        rr = aa.SignPDF(br.ReadBytes((int)fi.Length), dscAuthName, dscNametoPrint, a, b, c, d, "", frm_pdfName, -1, "", -1);

        //        if (rr.Error_Detail != "")
        //        {
        //            fgen.FILL_ERR(rr.Error_Detail);
        //        }

        //        fgen.save_dsc_info(frm_qstr, frm_cocd, frm_mbr, dr["TYPE"].ToString().Trim(), dr["vchnum"].ToString().Trim(), Convert.ToDateTime(dr["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy"), hfhcid.Value, frm_pdfName, frm_FileName, frm_uname);

        //        File.WriteAllBytes(tiffPath + frm_pdfName, rr.OutputFile);

        //        allFiles[k] = frm_pdfName;
        //        br.Close();
        //        br.Dispose();
        //        k++;
        //        br.Dispose();
        //        ////convertPdfToDSC(v);            
        //        if (hfhcid.Value == "F1006" || hfhcid.Value == "F1033")
        //        {
        //            ////////=====================================for update app_by in voucher for view invoice
        //            string mq0 = "", mq1 = "";
        //            if (frm_cocd == "STUD" || frm_cocd == "KLAS")
        //            {
        //                mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(trim(app_by),'-') as app_by from voucher where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + hfval.Value + "'", "app_by");
        //                if (mq0.Length < 2)
        //                {
        //                    mq1 = "update voucher set app_by='[A]" + frm_uname + "' where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + hfval.Value + "'";
        //                    fgen.execute_cmd(frm_qstr, frm_cocd, mq1);
        //                }
        //            }
        //            if (frm_cocd == "BONY")
        //            {
        //                mq1 = "UPDATE SALE SET KATAWT='1' ,PRTD_BY='" + frm_uname + " " + DateTime.Now.ToString("dd/MM/yyyy HH24:mm") + "' WHERE TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dr["BRANCHCD"].ToString().Trim() + dr["TYPE"].ToString().Trim() + dr["vchnum"].ToString().Trim() + Convert.ToDateTime(dr["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
        //                fgen.execute_cmd(frm_qstr, frm_cocd, mq1);
        //            }
        //            if (frm_cocd == "DLJM")
        //            {
        //                mq1 = "UPDATE SALE SET PRTD_BY='" + frm_uname + " " + DateTime.Now.ToString("dd/MM/yyyy HH24:mm") + "' WHERE TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dr["BRANCHCD"].ToString().Trim() + dr["TYPE"].ToString().Trim() + dr["vchnum"].ToString().Trim() + Convert.ToDateTime(dr["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
        //                fgen.execute_cmd(frm_qstr, frm_cocd, mq1);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        fgen.FILL_ERR("DSC Conv : " + ex.Message + " " + frm_pdfName);
        //    }
        //}

        //PdfReader reader = null;
        //Document sourceDocument = null;
        //PdfCopy pdfCopyProvider = null;
        //PdfImportedPage importedPage;

        //ViewState["frm_pdfname"] = allFiles;
        //DirectoryInfo di = new DirectoryInfo(Server.MapPath(@"~\tej-base\xmlfile\"));
        //allFiles = (string[])ViewState["frm_pdfname"];
        //var files = di.GetFiles();

        //string newtiffPath = @"c:\TEJ_ERP\DSC_pdf\CUSTOMER_INV_Pdf\";
        //bool splitFile = false;
        //switch (frm_cocd)
        //{
        //    case "KLAS":
        //        if (hfhcid.Value == "F1006" || hfhcid.Value == "F1033" || hfhcid.Value == "F1007" || hfhcid.Value == "F1007A" || hfhcid.Value == "F1023" || hfhcid.Value == "F1022" || hfhcid.Value == "F1032")
        //            splitFile = true;
        //        break;
        //    case "SAIP":
        //    case "SAIL":
        //        if (hfhcid.Value == "F1006" || hfhcid.Value == "F1033" || hfhcid.Value == "F1007" || hfhcid.Value == "F1007A")
        //            splitFile = true;
        //        break;
        //}

        //if (splitFile)
        //{
        //    string mTitleSrnoCond = "AND MTITLESRNO='0'";
        //    foreach (DataRow dr in dtDistEntryNo.Rows)
        //    {
        //        try
        //        {
        //            if (frm_cocd == "SAIP")
        //                frm_rptName = "~/tej-base/REPORT/std_SAIP_INV.rpt";
        //            if (frm_cocd == "SAIL")
        //                frm_rptName = "~/tej-base/REPORT/gst_inv_bc.rpt";
        //            if (frm_cocd == "KLAS")
        //                frm_rptName = "~/tej-base/REPORT/gst_inv_rx.rpt";
        //            if (hfhcid.Value == "F1007")
        //            {
        //                newtiffPath = @"c:\TEJ_ERP\DSC_pdf\CUSTOMER_CHL_Pdf\";
        //                frm_rptName = "~/tej-base/REPORT/std_chl_GST.rpt";
        //            }
        //            if (hfhcid.Value == "F1023" || hfhcid.Value == "F1022" || hfhcid.Value == "F1032")
        //            {
        //                mTitleSrnoCond = "AND MTITLESRNO<>'2'";
        //                newtiffPath = @"c:\TEJ_ERP\DSC_pdf\PARTY_ACC_PDF\";
        //                frm_rptName = "~/tej-base/REPORT/std_vch_rpt.rpt";
        //            }

        //            DataTable newDt = new DataTable();
        //            DataSet newDs = new DataSet();
        //            DataView dvN = new DataView(ds.Tables[0], "VCHNUM='" + dr["vchnum"].ToString().Trim() + "' " + mTitleSrnoCond + "", "", DataViewRowState.CurrentRows);
        //            newDt = dvN.ToTable();
        //            newDs.Tables.Add(newDt);
        //            DataRow nedr;
        //            for (int i = 0; i < ds.Tables.Count; i++)
        //            {
        //                if (ds.Tables[i].TableName.ToUpper() != "PREPCUR")
        //                {
        //                    newDt = new DataTable();
        //                    newDt = ds.Tables[i].Clone();
        //                    for (int x = 0; x < ds.Tables[i].Rows.Count; x++)
        //                    {
        //                        nedr = newDt.NewRow();
        //                        for (int y = 0; y < ds.Tables[i].Columns.Count; y++)
        //                        {
        //                            nedr[y] = ds.Tables[i].Rows[x][y];
        //                        }
        //                        newDt.Rows.Add(nedr);
        //                    }
        //                    newDt.TableName = ds.Tables[i].TableName;
        //                    newDs.Tables.Add(newDt);
        //                }
        //            }

        //            repDoc = new ReportDocument();
        //            GetReportDocumentDP(newDs, frm_rptName);

        //            if (frm_cocd == "KLAS") frm_pdfName = "ORIG_" + frm_pdfName;
        //            else
        //            {
        //                if (hfhcid.Value == "F1006")
        //                    frm_pdfName = newDs.Tables[0].Rows[0]["VENCODE"].ToString().Trim() + "_" + dr["full_invno"].ToString().Trim().Replace("/", "-") + "_" + Convert.ToDateTime(dr["VCHDATE"].ToString().Trim()).ToString("ddMMyyyy") + ".pdf";
        //                else frm_pdfName = newDs.Tables[0].Rows[0]["VENCODE"].ToString().Trim() + "_" + dr["vchnum"].ToString().Trim().Replace("/", "-") + "_" + Convert.ToDateTime(dr["VCHDATE"].ToString().Trim()).ToString("ddMMyyyy") + ".pdf";
        //            }
        //            frm_FileName = Server.MapPath(@"~\tej-base\xmlfile\" + frm_pdfName);

        //            repDoc.Refresh();
        //            repDoc.ExportToDisk(ExportFormatType.PortableDocFormat, frm_FileName);
        //            repDoc.Dispose();

        //            FileInfo fi = new FileInfo(frm_FileName);
        //            BinaryReader br = new BinaryReader(fi.OpenRead());

        //            Webtel_e_Sign.Res rr = new Webtel_e_Sign.Res();
        //            Webtel_e_Sign.ESign aa = new Webtel_e_Sign.ESign(ConnInfo.IP, "FIN" + frm_cocd, ConnInfo.nPwd, ConnInfo.srv, "1521", "2");

        //            //-2 for last page
        //            //-1 for every page
        //            rr = aa.SignPDF(br.ReadBytes((int)fi.Length), dscAuthName, dscNametoPrint, a, b, c, d, "", frm_pdfName, -1, "", -1);

        //            if (rr.Error_Detail != "")
        //            {
        //                fgen.FILL_ERR(rr.Error_Detail);
        //            }

        //            File.WriteAllBytes(newtiffPath + frm_pdfName, rr.OutputFile);

        //            br.Close();
        //            br.Dispose();
        //            br.Dispose();
        //            //convertPdfToDSC(v);                              
        //        }
        //        catch (Exception ex)
        //        {
        //            fgen.FILL_ERR("DSC Conv : " + ex.Message + " " + frm_pdfName);
        //        }
        //    }
        //}
        //reader = null;
        //sourceDocument = null;
        //pdfCopyProvider = null;
        //importedPage = null;

        ////DirectoryInfo di = new DirectoryInfo(tiffPath);        

        //string outputPdfPath = @"c:\TEJ_ERP\pdf\temp" + hfhcid.Value + ".pdf";
        //string outputPdfPath2 = @"c:\TEJ_ERP\pdf\tempnew" + hfhcid.Value + ".pdf";

        //del_file(outputPdfPath);
        //del_file(outputPdfPath2);

        //sourceDocument = new Document();
        //pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

        //sourceDocument.Open();


        //for (int i = 0; i < allFiles.Length; i++)
        //{
        //    var filesToDownload = files.Where(r => r.Name.Contains(allFiles[i].ToString()));

        //    foreach (FileInfo file in filesToDownload)
        //    {
        //        string filePath = file.FullName.Replace(@"\", "/");
        //        if (!filePath.Contains("ORIG_" + frm_cocd))
        //        {
        //            reader = new PdfReader(filePath);

        //            for (int x = 1; x <= reader.NumberOfPages; x++)
        //            {
        //                importedPage = pdfCopyProvider.GetImportedPage(reader, x);
        //                pdfCopyProvider.AddPage(importedPage);
        //            }
        //            reader.Close();
        //        }
        //    }
        //}
        //pdfCopyProvider.Close();
        //sourceDocument.Close();

        //{
        //    FileInfo fi = new FileInfo(outputPdfPath);
        //    BinaryReader br = new BinaryReader(fi.OpenRead());

        //    Webtel_e_Sign.Res rr = new Webtel_e_Sign.Res();
        //    Webtel_e_Sign.ESign aa = new Webtel_e_Sign.ESign(ConnInfo.IP, "FIN" + frm_cocd, ConnInfo.nPwd, ConnInfo.srv, "1521", "2");

        //    //-2 for last page
        //    //-1 for every page
        //    rr = aa.SignPDF(br.ReadBytes((int)fi.Length), dscAuthName, dscNametoPrint, a, b, c, d, "", "temp", -1, "", -1);

        //    if (rr.Error_Detail != "")
        //    {
        //        fgen.FILL_ERR(rr.Error_Detail);
        //    }

        //    File.WriteAllBytes(outputPdfPath2, rr.OutputFile);
        //    br.Close();
        //}
        //ds.Dispose();

        //try
        //{
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.WriteFile(outputPdfPath2);
        //    Response.End();
        //    //HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}
        //catch { }

        ////ScriptManager.RegisterStartupScript(this, this.GetType(), "filePopup", js.ToString(), true);        

        //if (hfclose.Value == "CLOSE*")
        //{
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
        //}
    }
    public void convertPdfToDSC(string[] args)
    {
        try
        {
            System.Diagnostics.Process process1 = new System.Diagnostics.Process();
            string myExeFile = HttpContext.Current.Server.MapPath("~\\tej-base\\myFiles\\convPdfWithDsc.exe");
            process1.StartInfo.FileName = myExeFile;
            process1.StartInfo.Arguments = "" + args[0] + " " + args[1] + " " + args[2] + " " + args[3] + " " + args[4] + " " + args[5] + " " + args[6] + " " + args[7];
            fgen.FILL_ERR("" + args[0] + " " + args[1] + " " + args[2] + " " + args[3] + " " + args[4] + " " + args[5] + " " + args[6] + " " + args[7]);
            process1.Start();
            process1.WaitForExit();
            process1.Close();
        }
        catch (Exception ex)
        {
            fgen.FILL_ERR("DSC Exe Calling : " + ex.Message);
        }
    }
    public void readjson(string jsonfilepath)
    {
        if (File.Exists(jsonfilepath))
        {
            using (StreamReader r = new StreamReader(jsonfilepath))
            {
                string json = r.ReadToEnd();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var items = jss.Deserialize<invFieldJSON[]>(json);
                foreach (var item in items)
                {
                    if (item != null)
                    {
                        signedInvoice = item.SignedInvoice;
                        signedQRCode = item.SignedQRCode;
                        Irn = item.Irn;
                    }
                }
            }
        }
    }
    protected void btnPrintToPrinter_Click(object sender, EventArgs e)
    {
        string repFilePath = Server.MapPath((string)Session["rptfile"]);
        try
        {
            dsRep = new DataSet();
            dsRep = (DataSet)Session["data_set"];
            repDoc = new ReportDocument();
            repDoc.Load(repFilePath);
            repDoc.Refresh();
            repDoc.SetDataSource(dsRep);
            if (File.Exists("c:\\TEJ_erp\\mfile.pdf")) File.Delete("c:\\TEJ_erp\\mfile.pdf");
            repDoc.ExportToDisk(ExportFormatType.PortableDocFormat, "c:\\TEJ_erp\\mfile.pdf");
            repDoc.Dispose();
        }
        catch { }
        string myExeFile = @"c:\TEJ_ERP\myDirectPrint1.exe";
        if (File.Exists(myExeFile))
        {
            System.Diagnostics.Process process1 = new System.Diagnostics.Process();
            process1.StartInfo.FileName = myExeFile;
            process1.Start();
            process1.WaitForExit();
            process1.Close();
            process1.Kill();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
        }
        //repDoc.PrintToPrinter(1, true, 0, 0);
    }
}

public class invFieldJSON
{
    public string ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
    public string Status { get; set; }
    public string GSTIN { get; set; }
    public string DocNo { get; set; }
    public string DocType { get; set; }
    public string DocDate { get; set; }
    public string Irn { get; set; }
    public string AckDate { get; set; }
    public string AckNo { get; set; }
    public string SignedInvoice { get; set; }
    public string SignedQRCode { get; set; }
    public string IrnStatus { get; set; }

}