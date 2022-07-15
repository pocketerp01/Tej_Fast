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

public partial class iPrint : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", opt;
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
                    frm_qstr = Guid.NewGuid().ToString("N").Substring(0, 20) + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
                    str = Request.QueryString["STR"].Trim().ToString();
                    frm_cocd = str.Split('@')[2].ToString().Trim().ToUpper();
                    frm_myear = str.Split('@')[3].ToString().Trim().ToUpper().Substring(0, 4);
                    frm_mbr = str.Split('@')[3].ToString().Trim().ToUpper().Substring(4, 2);
                    frm_uname = str.Split('@')[4].ToString().Trim().ToUpper();
                    hfhcid.Value = str.Split('@')[6].ToString().Trim();
                    hfval.Value = str.Split('@')[7].ToString().Trim();

                    string cIP = fgen.GetXMLTag(frm_cocd);
                    string cSN = "XE";
                    if (cIP.Length < 2)
                    {
                        cIP = fgen.GetXMLTag(frm_cocd + "_IP");
                        cSN = fgen.GetXMLTag(frm_cocd + "_SN");
                    }
                    string constr = ConnInfo.connStringManual(frm_cocd, cIP, cSN);
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
        string doc_GST = "";
        string chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
        //Member GCC Country
        if (chk_opt == "Y")
        {
            doc_GST = "GCC";
        }
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        switch (iconID)
        {
            //GE
            case "F1001":
                #region GE
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.*,B.INAME,B.CPARTNO,B.UNIT FROM IVOUCHERP A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' ORDER BY A.MORDER");
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
                dt = new DataTable();
                SQuery = "select f.addr1 as caddr1,f.addr2 as caddr2,f.addr3 as caddr3,f.addr4 as caddr4,f.mobile as ctel,f.aname,f.gst_no as cgst_no,f.email as cemail,t.name as mrrtype,i.unit as iunit,i.iname,i.cpartno as icpartno,b.amt_sale as totamt,b.bill_tot as grandtot, b.amt_exc as cgst_val,b.rvalue as sgst_val,B.EXCB_CHG AS TXBL,a.* from ivoucher a,item i,famst f,type t,ivchctrl b  where trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||TRIM(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and trim(a.type)=trim(t.type1) and t.id='M' AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' order by a.vchdate,a.vchnum,a.MORDER";
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
                SQuery = "SELECT 'Purchase Requisition' AS HEADER, B.INAME AS ITEM_NAME,b.CINAME,B.CPARTNO,B.HSCODE,C.INAME AS SUBNAME ,A.* FROM POMAS A,ITEM B ,ITEM C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR((A.ICODE),1,4)=TRIM(C.ICODE)  AND A.BRANCHCD||A.TYPE||TRIM(a.ordno)||TO_CHAR(A.orddt,'YYYYMMDD')='" + barCode + "' ORDER BY A.SRNO ";
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
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0012", "OPT_ENABLE");
                if (frm_vty != "54")
                {
                    //SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,D.ANAME AS CUST,D.ADDR1 AS ADRES1,D.ADDR2 AS ADRES2,D.ADDR3 AS ADRES3,D.GIRNO AS CUSTPAN,D.STAFFCD,D.PERSON AS CPERSON,D.EMAIL AS CMAIL,D.TELNUM AS CONT,D.STATEN AS CSTATE, D.GST_NO AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,B.NAME AS TYPENAME,C.INAME,C.CPARTNO AS  PARTNO,C.PUR_UOM AS CMT,C.NO_PROC AS Sunit,C.UNIT AS CUNIT,C.HSCODE,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.orddt,a.ordno,A.srno ";
                    SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,d.ANAME,TRIM(D.ANAME) AS CUST,TRIM(D.ADDR1) AS ADRES1,TRIM(D.ADDR2) AS ADRES2,TRIM(D.ADDR3) AS ADRES3,TRIM(D.GIRNO) AS CUSTPAN,TRIM(D.STAFFCD) AS STAFFCD,TRIM(D.PERSON) AS CPERSON,TRIM(D.EMAIL) AS CMAIL,TRIM(D.TELNUM) AS CONT,TRIM(D.STATEN) AS CSTATE, TRIM(D.GST_NO) AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,TRIM(B.NAME) AS TYPENAME,TRIM(C.INAME) AS INAME,TRIM(C.CPARTNO) AS  PARTNO,TRIM(C.PUR_UOM) AS CMT,TRIM(C.NO_PROC) AS Sunit,TRIM(C.UNIT) AS CUNIT,TRIM(C.HSCODE) AS HSCODE,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE,nvl(d.email,'-') as p_email,A.srno FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD||A.TYPE||TRIM(a.ordno)||TO_CHAR(A.orddt,'YYYYMMDD') in ('" + barCode + "') ORDER BY a.orddt,a.ordno,A.srno ";
                }
                else
                {
                    //SQuery = " select distinct a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'Import Purchase Order' as header,a.currency,a.delv_item,a.amdtno, b.aname,b.addr1,b.addr2,b.addr3,b.addr4,b.email,B.TELNUM,B.MOBILE,c.hscode,c.iname,c.unit as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,a.icode,a.qtyord as qtyord,a.prate,a.pdisc,a.payment as pay_term,a.transporter as shipp_frm,a.desp_to as shipp_to ,a.mode_tpt ,a.delv_term as etd,a.tr_insur as insurance,a.packing,a.remark,a.cscode1,a.cscode, a.pdiscamt, a.qtybal,d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ")";
                    SQuery = " select distinct a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'Import Purchase Order' as header,NVL(a.currency,0) AS currency,trim(a.delv_item) as delv_item,a.amdtno, trim(b.aname) as aname,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,trim(b.email) as email,B.TELNUM,B.MOBILE,trim(c.hscode) as hscode,trim(c.iname) as iname,trim(c.ciname) as ciname,trim(c.unit) as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(a.icode) as icode,nvl(a.qtyord,0) as qtyord,nvl(a.prate,0) as prate,nvl(a.pdisc,0) as pdisc,trim(a.payment) as pay_term,trim(a.transporter) as shipp_frm,trim(a.desp_to) as shipp_to,trim(a.mode_tpt) as mode_tpt,trim(a.delv_term) as etd,trim(a.tr_insur) as insurance,trim(a.packing) as packing,trim(a.remark) as remark,a.cscode1,a.cscode,nvl(a.pdiscamt,0) as pdiscamt,nvl(a.qtybal,0) as qtybal,trim(d.aname) as consign,trim(d.addr1) as daddr1,trim(d.addr2) as daddr2,trim(d.addr3) as daddr3,trim(d.addr4) as daddr4,trim(d.telnum) as dtel, trim(d.rc_num) as dtinno,trim(d.exc_num) as dcstno,trim(d.acode) as mycode,trim(d.staten) as dstaten,trim(d.gst_no) as dgst_no,trim(d.girno) as dpanno,substr(d.gst_no,0,2) as dstatecode,nvl(b.email,'-') as p_email,a.desc_,A.srno from  famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD||A.TYPE||TRIM(a.ordno)||TO_CHAR(A.orddt,'YYYYMMDD') in ('" + barCode + "') ORDER BY a.ordno,A.srno";
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    frm_vty = dt.Rows[0]["TYPE"].ToString().Trim();

                    if (!dt.Columns.Contains("POPREFIX")) dt.Columns.Add("POPREFIX");


                    SQuery = "SELECT DISTINCT BRANCHCD||TYPE||TRIM(vCHNUM)||TO_CHAR(VCHDATE,'YYYYmmdd') AS FSTR, TERMS||' '||CONDI AS POTERMS_FORM,SNO FROM POTERM WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY SNO";
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
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);

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

                    if (frm_rptName.Length <= 1) frm_rptName = "std_po";
                    if (frm_cocd == "HPPI" || frm_cocd == "SPPI" || doc_GST == "GCC") frm_rptName = "std_po_UAE";
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
                }
                #endregion
                    //Print_Report_BYDS(frm_cocd, frm_mbr, "std_po", frm_rptName, dsRep, "P.O. Entry Report");
                break;
            //S.O.
            case "F1005":
                #region S.O.
                SQuery = "Select 'SOMAS' as TAB_NAME,'SO Number' as h1,'SO Dated' as h2,G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'SOMAS' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from somas a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' AND A.BRANCHCD||A.TYPE||TRIM(a.ordno)||TO_CHAR(A.orddt,'YYYYMMDD')='" + barCode + "' order by a.ordno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));

                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_so", frm_rptName, dsRep, "S.O. Entry Report");
                }
                #endregion
                break;
            //INV
            case "F1006":
                #region INV

                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_pu as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' order by vchdate,a.vchnum,a.MORDER";

                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                    }

                    dt.TableName = "Prepcur";
                    repCount = 4;
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));

                    // VIPIN                                        
                    //if (frm_cocd == "PPAP")
                    {
                        dt1 = new DataTable("barcode");
                        dt1.Columns.Add(new DataColumn("img1_desc", typeof(string)));
                        dt1.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
                        string col2 = "";
                        mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no from type where id='B' and type1='" + dt.Rows[0]["branchcd"].ToString().Trim().Replace("/", "") + "'", "gst_no");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            col1 = dt.Rows[i]["branchcd"].ToString().Trim().Replace("/", "") + "," + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "");

                            if (frm_cocd.Equals("PPAP") || frm_cocd.Equals("ADZO"))
                            {
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
                            }
                        }
                        fpath = Server.MapPath(@"BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        if (frm_cocd == "PPAP" || frm_cocd.Equals("ADZO")) fgen.prnt_QRbar(frm_cocd, col2, col1.Replace("*", "").Replace("/", "") + ".png");
                        else fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");

                        DataRow dr = dt1.NewRow();
                        FilStr = new FileStream(fpath, FileMode.Open);
                        BinRed = new BinaryReader(FilStr);

                        dr["img1_desc"] = col1.Trim();
                        dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                        dt1.Rows.Add(dr);
                        FilStr.Close();
                        BinRed.Close();

                        dsRep.Tables.Add(dt1);
                    }

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
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                    }
                    mdr = dt1.NewRow();
                    mdr["poterms"] = mq10;
                    dt1.Rows.Add(mdr);
                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);

                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report");
                }
                //printDefault(frm_cocd, frm_mbr, "std_invcl", "std_invcl", dsRep, "Invoice Challan");
                #endregion
                break;
            //CHL
            case "F1007":
                #region CHL

                SQuery = "SELECT D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' AND D.ID='M' ORDER BY A.ICODE";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }

                openPrintAgain(frm_cocd, frm_mbr, "Direct", "F1007A", frm_cDt1, scode);

                Print_Report_BYDS(frm_cocd, frm_mbr, "std_chl", frm_rptName, dsRep, "Challan Report");
                //Thread td = new Thread(() => openPrintAgain(frm_cocd, frm_mbr, "Direct", "F1007A", frm_cDt1, scode));
                //td.Start();
                #endregion
                break;
            //CHL2
            case "F1007A":
                #region CHL2
                SQuery = "SELECT B.INAME,B.UNIT AS UNIT2, A.BRANCHCD AS MBR,A.TYPE AS BTYPE,a.vchnum,A.VCHNUM AS BVCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BVCHDATE1,A.ICODE AS BICODE,A.ACODE AS BACODE,A.IQTYOUT AS BQTY,A.IQTY_WT AS WT_REC FROM RGPMST A,ITEM B  WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE)";
                SQuery = "SELECT D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' AND D.ID='M' ORDER BY A.ICODE";
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
                SQuery = "select 'Material Issue Request' as header,'Material Issue Request' as h1,'Issue Agst Job Card' as h2, C.NAME AS DPT_NAME,I.INAME,I.CPARTNO,I.UNIT AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM IVOUCHER A, ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND C.ID='M' AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "'  order by A.VCHNUM DESC";
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
                SQuery = "SELECT D.ANAME AS SUPPLIER,E.NAME AS ENAME, c.name,TO_CHAR(A.GEDATE,'DD/MM/YYYY') AS GDATE, B.INAME,B.UNIT AS BUNIT,B.CPARTNO, A.* FROM IVOUCHER A,ITEM B ,TYPE C,FAMST D,TYPE E WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and trim(a.acode)=trim(c.type1) and c.id='M' AND E.ID='M'  AND  trim(a.TYPE)=trim(E.type1) AND TRIM(A.VCODE)=TRIM(D.ACODE) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "'";
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
                SQuery = "select i.iname,i.unit,i.cpartno as icpart,i.cdrgno,a.* from inspmst a,item i  where  trim(i.icode)=trim(a.icode) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' order by srno";
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
                SQuery = "SELECT 'Inward Inspection Report' AS HEADER , F.ANAME,I.INAME,I.CPARTNO AS ICPARTNO,A.* FROM INSPVCH A,FAMST F, ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_InwardInwReport", frm_rptName, dsRep, "INW INSP. RPT");
                }
                #endregion
                break;
            //OUT INSP. TEMP
            case "F1012":
                #region OUT INSP. TEMP
                SQuery = "select i.iname,i.unit,i.cdrgno,I.CPARTNO AS ICPARTNO,a.* from inspmst a,item i  where  trim(i.icode)=trim(a.icode) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' ORDER BY SRNO";
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
                SQuery = "SELECT 'Pre Dispatch Inspection Report' AS HEADER , F.ANAME,F.ADDR1 AS FDDR,I.INAME,I.CPARTNO AS ICPARTNO,i.unit as iunit,A.* FROM inspvch A,FAMST F, ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_OutwardInsReport", frm_rptName, dsRep, "OUT INSP. RPT");
                }
                #endregion
                break;
            //PURCH SCH
            case "F1014":
                #region PURCH SCH
                SQuery = "select d.mthname, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.* from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum)  AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "'";
                col1 = "YES";
                if (col1 == "YES")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' ORDER BY A.ICODE DESC";
                }
                if (col1 == "NO")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "'";
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
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) ASDAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "' ORDER BY A.ICODE DESC";  //using round off
                }
                if (col1 == "NO")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "'";
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
                SQuery = "Select  G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'SOMAS' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from somasq a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD')='" + barCode + "' order by a.ordno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_pi", frm_rptName, dsRep, "std_pi");
                }
                #endregion
                break;
            //M.S.O Print
            case "F1017":
                #region M.S.O Print
                SQuery = "Select  G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'SOMAS' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from SOMASM a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'YYYYMMDD')='" + barCode + "' order by a.ordno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_mso", frm_rptName, dsRep, "std_mso");
                }
                #endregion
                break;
            //Process Plan Print
            case "F1018":
                #region Process Plan Print
                SQuery = "SELECT  B.INAME AS ITEMNAME,B.CDRGNO AS CUST_IT_CODE,C.ANAME AS CUSTOEMR,a.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,A.TITLE as Remarks,A.ACODE,A.ICODE,A.CPARTNO,A.SRNO,A.BTCHNO AS SR,COL1 AS PROCESS,A.COL2 AS SPECIFICATION,A.COL3 AS Reqmt,A.COL4 as RMK, A.COL5 AS ERPCODE,A.COL6 AS UOM,A.COL9 AS COBB_IN,A.COL10 AS FLUTE,A.COL11 AS HEIGHT,A.COL12 AS DIENO,A.COL13 AS TYPE_OF_ITEM,A.COL14 AS CTN_SIZE_OD,A.COL15 as PLy,A.COL16 AS CTN_SIZE_ID,A.COL17,A.COL18 AS Std_Rej_Allow,A.REJQTY  AS UPS,A.REMARK2,REMARK3,REMARK4,A.ENT_BY,TO_cHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,A.APP_BY,A.APP_DT,A.EDT_BY,TO_CHAR(A.EDT_DT,'DD/MM/YYYY') AS EDT_DT,A.AMDCOMMENT AS AMEN1,A.AMDDT AS AMDT1,A.AMDCOMMENT2 AS AMEN2 ,A.AMDDT2,A.AMDCOMMENT3 AS AMEN3,A.AMDDT3,A.AMDCOMMENT4 AS AMEN4,A.AMDDT4,A.AMDCOMMENT5 AS AMEN5,A.AMDDT5,A.AMDNO FROM  INSPMST  A,ITEM B ,FAMST C WHERE A.BRANCHCD='" + frm_mbr + "' AND A .TYPE='70'AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')='" + barCode + "'  AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) ORDER BY A.SRNO";
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
                DataSet ds = new DataSet();
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
            //GST Debit Note
            case "F1022":
                #region GST Debit Note
                SQuery = "SELECT F.ANAME,F.ADDR1 AS FDDR1,F.ADDR2 AS FADDR2,F.ADDR3 AS FADDR3,F.STATEN AS FSTATE,SUBSTR(F.GST_NO,0,2) AS FSTATECODE,F.GIRNO AS FGIRNO,F.GST_NO AS FGST_NO,I.INAME,I.UNIT AS IUNIT,I.HSCODE,A.* FROM IVOUCHER A,FAMST F ,ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') IN (" + scode + ")";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_GSTDebitNote", frm_rptName, dsRep, "std_GSTDebitNote");
                }
                #endregion
                break;
            //GST Credit Note
            case "F1023":
                #region GST Credit Note
                SQuery = "SELECT F.ANAME,F.ADDR1 AS FDDR1,F.ADDR2 AS FADDR2,F.ADDR3 AS FADDR3,F.STATEN AS FSTATE,SUBSTR(F.GST_NO,0,2) AS FSTATECODE,F.GIRNO AS FGIRNO,F.GST_NO AS FGST_NO,I.INAME,I.UNIT AS IUNIT,I.HSCODE,A.* FROM IVOUCHER A,FAMST F ,ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') IN (" + scode + ")";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_GSTCreditNote", frm_rptName, dsRep, "std_GSTCreditNote");
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
                #region Packing List
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
                string header_n = "Packing List";//for cmpl new format
                SQuery = "select '" + header_n + "' as header, trim(a.col9)||trim(a.col10)||trim(a.icode) as fstr,trim(a.vchnum) as vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,i.iname,i.hscode,trim(a.acode) as partycode,a.SRNO,is_number(a.COL1) AS carton_wt,a.COL2 AS DESC_,is_number(a.COL3) AS QTY_PR_pallet,is_number(a.col4) as pr_pallet_wt,is_number(a.col5) as tot_wt,is_number(a.col11) as pallet_wt,a.col6 as pallet_dimen,is_number(a.col7) as no_of_pallet ,is_number(a.col3)*is_number(a.col7) as tot_qty,a.col13 as pallet_no,I.TARRIFNO, B.GST_NO,B.TELNUM ,B.PAYMENT,B.EMAIL,b.aname,b.addr1,b.addr2,b.addr3,b.addr4,B.COUNTRY,a.col9,a.col10  from scratch  a ,item i,famst b where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(b.acode) and TRIM(A.BRANCHCD)||trim(a.type)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + scode + "'  order by a.srno";
                dsRep = new DataSet(); dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                fgen.send_cookie("seekSql", SQuery);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region
                        dr1 = dtm.NewRow();
                        dr1["Header"] = dt.Rows[i]["header"].ToString().Trim();
                        dr1["vchnum"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr1["vchdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                        dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr1["hscode"] = dt.Rows[i]["hscode"].ToString().Trim();
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
                        dr1["TARRIFNO"] = dt.Rows[i]["TARRIFNO"].ToString().Trim();
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
                        dtm.Rows.Add(dr1);
                        #endregion
                    }
                    SQuery = "select  a.finvno,trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr1,b.acode,a.icode,a.vchnum as svch,to_char(a.vchdate,'dd/mm/yyyy') as svchdt,b.cscode from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='4' and a.type!='4F' AND a.vchnum='" + dt.Rows[0]["col9"].ToString().Trim() + "' and to_char(a.vchdate,'dd/mm/yyyy')='" + dt.Rows[0]["col10"].ToString().Trim() + "'";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        dtm.Rows[j]["finvno"] = dt1.Rows[j]["finvno"].ToString().Trim();
                        dtm.Rows[j]["svch"] = dt1.Rows[j]["svch"].ToString().Trim();
                        dtm.Rows[j]["svchdt"] = dt1.Rows[j]["svchdt"].ToString().Trim();
                        dtm.Rows[j]["cscode"] = dt1.Rows[j]["cscode"].ToString().Trim();
                    }
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    ///////////					
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
                break;
        }
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
            conv_pdf(data_set, rptfile);
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

        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            if (report.ToUpper() == "STD_OUTWARDINSREPORT*") { }
            else
                conv_pdf(data_set, rptfile);
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
}