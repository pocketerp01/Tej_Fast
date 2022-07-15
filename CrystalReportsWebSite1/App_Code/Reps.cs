﻿using System;

using System.Data;
using System.IO;
using System.Linq;
using System.Web;


using Models;

/// <summary>
/// Summary description for sgenFun
/// </summary>
/// 
public class Reps
{
    string MyGuid = "", pdfView = "", data_found = "", cond = "", xprd1 = "", cond1 = "", frm_IndType = "", DateRange = "";
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery = "", branch_Cd="", frm_rptName,
          str, xprdRange, xprdRange1, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl, frm_UserID, fromdt, todt, header_n;
    fgenDB fgen;
    DataRow mdr, dr1;
    DataSet dsRep, ds;
    DataTable ph_tbl;
    DataRow dr, dro, dro1, dr2;
    DataView vdview = new DataView();
    FileStream FilStr;
    BinaryReader BinRed;
    double db = 0, db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0;


    string sname = "";
    string mq10, mq1, mq0, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq11, mq12, ded1;
    int repCount = 1;
    string opt = "";
    string barCode = "";
    string scode = "";
   

    DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;

    string party_cd = "";
    string part_cd = "";
    string hfhcid = "";
    string hfval = "";
    public Reps(string myguid)
    {
        MyGuid = myguid;
        cond = ""; cond1 = ""; xprd1 = "";
        fgen = new fgenDB();
    }

    void Fill_Mst()
    {
         hfhcid = Multiton.Get_Mvar(frm_qstr, "REPID");
         hfval = (string)Multiton.GetSession(MyGuid, "SSEEKVAL");
        dsRep = new DataSet();
         barCode = hfval;
        scode = barCode;
        frm_qstr = MyGuid;
        frm_cocd = Multiton.Get_Mvar(frm_qstr, "U_COCD");
        frm_uname = Multiton.Get_Mvar(frm_qstr, "U_UNAME");
        frm_myear = Multiton.Get_Mvar(frm_qstr, "U_YEAR");
        frm_ulvl = Multiton.Get_Mvar(frm_qstr, "U_ULEVEL");
        branch_Cd = frm_mbr = Multiton.Get_Mvar(frm_qstr, "U_MBR");
        frm_vty = Multiton.Get_Mvar(frm_qstr, "U_VTY");
        xprdRange = Multiton.Get_Mvar(frm_qstr, "U_PRDRANGE");
        frm_UserID = Multiton.Get_Mvar(frm_qstr, "U_USERID");
        frm_cDt1 = Multiton.Get_Mvar(frm_qstr, "U_Cdt1");
        frm_cDt2 = Multiton.Get_Mvar(frm_qstr, "U_Cdt2");
        fromdt = Multiton.Get_Mvar(frm_qstr, "U_MDT1");
        todt = Multiton.Get_Mvar(frm_qstr, "U_MDT2");
        frm_IndType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");


    }
    public void PurchaseReps(string iconID)
    {
        Fill_Mst();
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string  chk_opt = "";
      
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
                SQuery = "select trim((d.mthname) as mthname,trim(C.ANAME) as aname,trim(C.ADDR1) as ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,TRIM(C.RC_NUM) AS TIN,c.gst_no,TRIM(B.INAME) AS INAME,A.* from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "')";
                col1 = "N";
                if (col1 == "Y")
                {
                    //SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,C.EMAIL,C.WEBSITE,C.GST_NO AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY A.ICODE DESC";
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, trim(C.ANAME) as aname,trim(C.ADDR1) as ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,TRIM(C.RC_NUM) AS TIN,c.gst_no,TRIM(B.INAME) AS INAME,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,Round((nvl(DAY1,0)/1000),1) as  DAY1 ,round((nvl(A.DAY2,0)/1000),1) AS DAY2,round((nvl(A.DAY3,0)/10000),1) AS DAY3,round((nvl(A.DAY4,0)/1000),1) AS DAY4,round((nvl(A.DAY5,0)/1000),1) AS DAY5,round((nvl(A.DAY6,0)/1000),1)  AS DAY6,round((nvl(A.DAY7,0)/1000),1) AS DAY7,round((nvl(A.DAY8,0)/1000),1) AS DAY8,round((nvl(A.DAY9,0)/1000),1) AS DAY9,round((nvl(A.DAY10,0)/1000),1) AS DAY10,round((nvl(A.DAY11,0)/1000),1) AS DAY11,round((nvl(A.DAY12,0)/1000),1) AS DAY12,round((nvl(A.DAY13,0)/1000),1) AS DAY13,round((nvl(A.DAY14,0)/1000),1) AS DAY14,round((nvl(A.DAY15,0)/1000),1) AS DAY15,round((nvl(A.DAY16,0)/1000),1) AS DAY16,round((nvl(A.DAY17,0)/1000),1) AS DAY17,round((nvl(A.DAY18,0)/1000),1) AS DAY18,round((nvl(A.DAY19,0)/1000),1) AS DAY19,round((nvl(A.DAY20,0)/1000),1) AS DAY20,round((nvl(A.DAY21,0)/1000),1) AS DAY21,round((nvl(A.DAY22,0)/1000),1) AS DAY22,round((nvl(A.DAY23,0)/1000),1) AS DAY23,round((nvl(A.DAY24,0)/1000),1) AS DAY24,round((nvl(A.DAY25,0)/1000),1) AS DAY25,round((nvl(A.DAY26,0)/1000),1) AS DAY26,round((nvl(A.DAY27,0)/1000),1) AS DAY27,round((nvl(A.DAY28,0)/1000),1) AS DAY28,round((nvl(A.DAY29,0)/1000),1) AS DAY29,round((nvl(A.DAY30,0)/1000),1)  AS DAY30,round((nvl(A.DAY31,0)/1000),1) AS DAY31,round((nvl(A.TOTAL,0)/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,nvl(A.APP_BY,'-') as app_by,A.APP_DT,trim(C.EMAIL) as email,trim(C.WEBSITE) as website,trim(C.GST_NO) AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY A.ICODE DESC";
                }
                if (col1 == "N")
                {
                    //SQuery = "select a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,C.EMAIL,C.WEBSITE,C.GST_NO AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "')";
                    SQuery = "select trim(a.branchcd)||trim(a.type)||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'" + col1 + "' as col1,TRIM(d.mthname) AS MTHNAME,to_char(a.vchdate,'YYYY') AS YEAR_,trim(C.ANAME) as aname,trim(C.ADDR1) as ADDR1,TRIM(C.ADDR2) AS ADDR2,TRIM(C.ADDR3) AS ADDR3,TRIM(C.RC_NUM) AS TIN,TRIM(B.INAME) AS INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,nvl(A.DAY1,0) as DAY1,NVL(A.DAY2,0) AS DAY2,NVL(A.DAY3,0) AS DAY3,NVL(A.DAY4,0) AS DAY4,NVL(A.DAY5,0) AS DAY5,NVL(A.DAY6,0) AS DAY6,NVL(A.DAY7,0) AS DAY7,NVL(A.DAY8,0) AS DAY8,NVL(A.DAY9,0) AS DAY9,NVL(A.DAY10,0) AS DAY10,NVL(A.DAY11,0) AS DAY11,NVL(A.DAY12,0)  AS DAY12,NVL(A.DAY13,0) AS DAY13,NVL(A.DAY14,0) AS DAY14,NVL(A.DAY15,0) AS DAY15,NVL(A.DAY16,0) AS DAY16,NVL(A.DAY17,0) AS DAY17,NVL(A.DAY18,0) AS DAY18,NVL(A.DAY19,0) AS DAY19,NVL(A.DAY20,0) AS DAY20,NVL(A.DAY21,0) AS DAY21,NVL(A.DAY22,0) AS DAY22,NVL(A.DAY23,0) AS DAY23,NVL(A.DAY24,0) AS DAY24,NVL(A.DAY25,0) AS DAY25,NVL(A.DAY26,0) AS DAY26,NVL(A.DAY27,0) AS DAY27,NVL(A.DAY28,0) AS DAY28,NVL(A.DAY29,0) AS DAY29,NVL(A.DAY30,0) AS DAY30,NVL(A.DAY31,0) AS DAY31,NVL(A.TOTAL,0) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT,TRIM(C.EMAIL) AS EMAIL,TRIM(C.WEBSITE) AS WEBSITE,TRIM(C.GST_NO) AS PARTY_GST from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "')";
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
                    //SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,D.ANAME AS CUST,D.ADDR1 AS ADRES1,D.ADDR2 AS ADRES2,D.ADDR3 AS ADRES3,D.GIRNO AS CUSTPAN,D.STAFFCD,D.PERSON AS CPERSON,D.EMAIL AS CMAIL,D.TELNUM AS CONT,D.STATEN AS CSTATE, D.GST_NO AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,B.NAME AS TYPENAME,C.INAME,C.CPARTNO AS  PARTNO,C.PUR_UOM AS CMT,C.NO_PROC AS Sunit,C.UNIT AS CUNIT,C.HSCODE,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY a.orddt,a.ordno,A.srno ";
                    SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,d.ANAME,TRIM(D.ANAME) AS CUST,TRIM(D.ADDR1) AS ADRES1,TRIM(D.ADDR2) AS ADRES2,TRIM(D.ADDR3) AS ADRES3,TRIM(D.GIRNO) AS CUSTPAN,TRIM(D.STAFFCD) AS STAFFCD,TRIM(D.PERSON) AS CPERSON,TRIM(D.EMAIL) AS CMAIL,TRIM(D.TELNUM) AS CONT,TRIM(D.STATEN) AS CSTATE, TRIM(D.GST_NO) AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,TRIM(B.NAME) AS TYPENAME,TRIM(C.INAME) AS INAME,TRIM(C.CPARTNO) AS  PARTNO,TRIM(C.PUR_UOM) AS CMT,TRIM(C.NO_PROC) AS Sunit,TRIM(C.UNIT) AS CUNIT,TRIM(C.HSCODE) AS HSCODE,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE,nvl(d.email,'-') as p_email,A.srno FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY a.orddt,a.ordno,A.srno ";
                }
                else
                {
                    //SQuery = " select distinct a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'Import Purchase Order' as header,a.currency,a.delv_item,a.amdtno, b.aname,b.addr1,b.addr2,b.addr3,b.addr4,b.email,B.TELNUM,B.MOBILE,c.hscode,c.iname,c.unit as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,a.icode,a.qtyord as qtyord,a.prate,a.pdisc,a.payment as pay_term,a.transporter as shipp_frm,a.desp_to as shipp_to ,a.mode_tpt ,a.delv_term as etd,a.tr_insur as insurance,a.packing,a.remark,a.cscode1,a.cscode, a.pdiscamt, a.qtybal,d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in ('" + barCode + "')";
                    SQuery = " select distinct a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'Import Purchase Order' as header,NVL(a.currency,0) AS currency,trim(a.delv_item) as delv_item,a.amdtno, trim(b.aname) as aname,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,trim(b.email) as email,B.TELNUM,B.MOBILE,trim(c.hscode) as hscode,trim(c.iname) as iname,trim(c.ciname) as ciname,trim(c.unit) as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(a.icode) as icode,nvl(a.qtyord,0) as qtyord,nvl(a.prate,0) as prate,nvl(a.pdisc,0) as pdisc,trim(a.payment) as pay_term,trim(a.transporter) as shipp_frm,trim(a.desp_to) as shipp_to,trim(a.mode_tpt) as mode_tpt,trim(a.delv_term) as etd,trim(a.tr_insur) as insurance,trim(a.packing) as packing,trim(a.remark) as remark,a.cscode1,a.cscode,nvl(a.pdiscamt,0) as pdiscamt,nvl(a.qtybal,0) as qtybal,trim(d.aname) as consign,trim(d.addr1) as daddr1,trim(d.addr2) as daddr2,trim(d.addr3) as daddr3,trim(d.addr4) as daddr4,trim(d.telnum) as dtel, trim(d.rc_num) as dtinno,trim(d.exc_num) as dcstno,trim(d.acode) as mycode,trim(d.staten) as dstaten,trim(d.gst_no) as dgst_no,trim(d.girno) as dpanno,substr(d.gst_no,0,2) as dstatecode,nvl(b.email,'-') as p_email,a.desc_,A.srno from  famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY a.ordno,A.srno";
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("POPREFIX")) dt.Columns.Add("POPREFIX");


                    SQuery = "SELECT DISTINCT BRANCHCD||TYPE||TRIM(vCHNUM)||TO_CHAR(VCHDATE,'YYYYmmdd') AS FSTR, TERMS||' '||CONDI AS POTERMS_FORM,SNO FROM POTERM WHERE BRANCHCD='" + frm_mbr + "' and TYPE ='" + frm_vty + "' and TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY SNO";
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
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_po", frm_rptName, dsRep, "P.O. Entry Report","Y");
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
                //SQuery = "SELECT 'Purchase Requisition' AS HEADER, B.INAME AS ITEM_NAME,B.CPARTNO,B.HSCODE,b.unit as iunit,trim(C.NAME)||'->'||trim(a.payment) AS MAINGRP ,A.* FROM POMAS A,ITEM B ,type C  WHERE c.id='Y' and TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(A.ICODE,1,2)=TRIM(C.type1) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY A.SRNO ";
                SQuery = "SELECT 'Purchase Requisition' AS HEADER, TRIM(B.INAME) AS ITEM_NAME,TRIM(B.CPARTNO) AS CPARTNO,TRIM(B.HSCODE) AS HSCODE,TRIM(b.unit) as iunit,trim(C.NAME) as subname,trim(C.NAME)||'->'||trim(a.payment) AS MAINGRP ,A.*,b.irate as item_rate FROM POMAS A,ITEM B ,type C  WHERE c.id='Y' and TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(A.ICODE,1,2)=TRIM(C.type1) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY A.SRNO";
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
                SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'-' AS btoprint,d.ANAME,TRIM(D.ANAME) AS CUST,TRIM(D.ADDR1) AS ADRES1,TRIM(D.ADDR2) AS ADRES2,TRIM(D.ADDR3) AS ADRES3,TRIM(D.GIRNO) AS CUSTPAN,TRIM(D.STAFFCD) AS STAFFCD,TRIM(D.PERSON) AS CPERSON,TRIM(D.EMAIL) AS CMAIL,TRIM(D.TELNUM) AS CONT,TRIM(D.STATEN) AS CSTATE, TRIM(D.GST_NO) AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,TRIM(B.NAME) AS TYPENAME,TRIM(C.INAME) AS INAME,TRIM(C.CPARTNO) AS  PARTNO,TRIM(C.PUR_UOM) AS CMT,TRIM(C.NO_PROC) AS Sunit,TRIM(C.UNIT) AS CUNIT,TRIM(C.HSCODE) AS HSCODE,A.*,'" + header_n + "' AS CASE,nvl(d.email,'-') as p_email FROM WB_PORFQ A,TYPE B,ITEM C,wbvu_fam_vend D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in ('" + barCode + "') ORDER BY a.orddt,a.ordno,A.srno ";
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

    public void SalesReps(string iconID)
    {
        Fill_Mst();
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        data_found = "Y";
        string doc_GST = "";
        string chk_opt = "";
        chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
        if (chk_opt == "Y")
        //Member GCC Country
        {
            doc_GST = "GCC";
        }
        switch (iconID)
        {
            case "F50101":
            case "F50106":
            case "F55106":
            case "F1006":
            case "F1006A":
            case "F50271":
                #region INV
                if (iconID == "F50271")
                {
                    //frm_vty = hfval.Value.Replace("'", "");
                    frm_vty = hfval;
                    barCode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                }
                string yr = "", CURR = "";
                int nxt=0;
                mq4 = barCode.Substring(0, 6);
                try
                {
                    CURR = fromdt.Substring(8, 2);
                    nxt = Convert.ToInt32(CURR) + 1;
                }
                catch { }
                scode = scode.Replace(";", "");
                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,C.tcsamt,a.vchdate as vdd from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') order by vdd,a.vchnum,a.morder ";
                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,C.tcsamt,a.vchdate as vdd,c.acvdrt,a.doc_tot from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode)  AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') order by vdd,a.vchnum,a.morder";

                if (iconID == "F50106" || iconID == "F55106")
                {
                    SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,to_number(a.cess_pu) cess_pu,a.type," +
                        "d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp," +
                        "a.finvno,a.exc_57f4,a.iexc_Addl,to_number(A.exc_amt) exc_amt ,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val," +
                        "to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno," +
                        "b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate," +
                        "c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate," +
                        "c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as" +
                        " grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt," +
                        "c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode," +
                        "a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt," +
                        "a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,to_number(a.iamount) iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg," +
                        "a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,to_number(a.exc_RATE) as cgst," +
                        "to_number(a.exc_amt) as cgst_val,to_number(a.cess_percent) as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1," +
                        "'  PROFORMA  '||t1.name as Name,C.tcsamt,a.col1,a.col2,a.col3,a.col4,a.col5,a.col6,a.col7,a.col8,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p" +
                        ",er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p" +
                        ",er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucherp a,salep c  left join csmst er on trim(c.cscode)=trim(er.acode)" +
                        ",item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) " +
                        "and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and" +
                        " trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') " +
                        "in ('" + barCode.Replace("'", "") + "') order by a.vchnum,a.MORDER";
                }
                else
                {
                    SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,to_number(a.cess_pu) CESS_PU,a.type,d.ciname," +
                        "d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4," +
                        "a.iexc_Addl,to_number(A.exc_amt) EXC_AMT,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate," +
                        "b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno," +
                        " b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno," +
                        "c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime," +
                        "c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no," +
                        "c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno," +
                        "a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag," +
                        "a.approxval as bal,a.ichgs as cdisc,to_number(a.iamount) IAMOUNT,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by," +
                        "to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,to_number(a.exc_RATE) as cgst,a.exc_amt as cgst_val," +
                        "to_number(a.cess_percent) as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot,er.aname as consign_p," +
                        "er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p," +
                        "er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucher a,sale c left join csmst er on trim(c.cscode)=trim(er.acode)," +
                        "item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' " +
                        "and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) " +
                        "AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') order by vchdate,a.vchnum,a.MORDER";
                }

                if (frm_cocd == "STUD")
                {
                    if (frm_vty == "4F")
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER,'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect,a.iweight,b.payment,nvl(a.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,f.telnum as tpt_telnum,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,h.invno AS Hinvno,TO_CHAR(h.invdate,'DD/MM/YYYY') AS Hinvdate,h.ship2,h.ship3,h.ship4,h.ship5,h.lbnetwt,h.REMARK3 AS NETWT,h.lbgrswt,h.exprmk1,h.exprmk2,h.exprmk3,h.exprmk4,h.exprmk5,h.addl1,h.addl2,h.addl3,h.addl4,h.addl5,h.tmaddl1,h.tmaddl2,h.tmaddl3,h.addl6 from ivoucher a left join hundi h on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h.branchcd)||trim(h.acode)||trim(h.invno)||to_char(h.invdate,'dd/mm/yyyy'),sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') order by vchdate,a.vchnum,a.MORDER";
                    }
                    else
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,to_number(a.exc_RATE) as cgst,to_number(a.exc_amt) as cgst_val,to_number(a.cess_percent) as sgst,to_number(a.cess_pu) as sgst_val,a.iopr,d.hscode,to_number(b.gst_no) as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,B.COUNTRY,d.packsize,f.telnum as tpt_telnum,nvl(a.et_paid,0) as et_paid,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucher a,sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') order by vchdate,a.vchnum,a.MORDER";
                    }
                }

                if (frm_cocd == "SAIA") frm_rptName = "std_inv_saia";
                if (frm_rptName.Length < 2)
                {
                    if (iconID == "F1006A" || iconID == "F50101" || iconID == "F50106")
                    {
                        if (frm_cocd == "KRSM")
                        {
                            frm_rptName = "std_inv_bank";
                        }
                        else
                        {
                            frm_rptName = "std_inv";
                        }
                    }
                    if (frm_cocd == "AGRM") frm_rptName = "std_inv_agrm";
                }
                if (iconID == "F50106" || iconID == "F55106")
                {
                    frm_rptName = "std_Perf_inv";
                    if (frm_cocd == "KRSM")
                    {
                        frm_rptName = "std_inv_banK_PRO";
                    }

                }
                if (frm_rptName.Length < 2) frm_rptName = "std_inv";

                if (frm_cocd == "HPPI" || doc_GST == "GCC") frm_rptName = "std_inv_UAE";
                if (doc_GST == "GCC" && iconID == "F50106") frm_rptName = "std_pi_UAE";


                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    if (frm_vty != "4F")
                    {
                        DataTable dtHSDetails = new DataTable();
                        dtHSDetails.Columns.Add("FSTR", typeof(string));
                        dtHSDetails.Columns.Add("HSCODE_H", typeof(string));
                        dtHSDetails.Columns.Add("AMT", typeof(decimal));
                        dtHSDetails.Columns.Add("CGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("SGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("IGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("GST_H", typeof(decimal));

                        dtHSDetails = dt.AsEnumerable().GroupBy(r => new
                        {
                            fstr = r.Field<string>("FSTR"),
                            hscode = r.Field<string>("HSCODE").Trim(),
                            cgst = r.Field<decimal>("CGST"),
                            sgst = r.Field<decimal>("SGST"),
                            igst = r.Field<decimal>("CGST")
                        })
                             .Select(g =>
                             {
                                 var row = dtHSDetails.NewRow();
                                 row["FSTR"] = g.Key.fstr;
                                 row["HSCODE_H"] = g.Key.hscode;
                                 if (g.Key.sgst > 0)
                                 {
                                     row["CGST_H"] = g.Key.cgst;
                                     row["SGST_H"] = g.Key.sgst;
                                     row["IGST_H"] = 0;
                                 }
                                 else
                                 {
                                     row["CGST_H"] = 0;
                                     row["SGST_H"] = 0;
                                     row["IGST_H"] = g.Key.igst;
                                 }
                                 row["AMT"] = g.Sum(r => r.Field<decimal>("IAMOUNT"));
                                 row["GST_H"] = g.Sum(r => r.Field<decimal>("EXC_AMT")) + g.Sum(r => r.Field<decimal>("CESS_PU"));
                                 return row;
                             }).CopyToDataTable();

                        dtHSDetails.TableName = "dtHSDetails";
                        dsRep.Tables.Add(dtHSDetails);
                    }
                    dt.Columns.Add(new DataColumn("amtToword", typeof(string)));
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    if (frm_vty == "4F")
                    {
                        dt.Columns.Add("EXP_YR", typeof(string));
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                        dr["amtToword"] = fgen.ConvertNumbertoWords(dr["bill_tot"].ToString().Trim());
                        if (frm_vty == "4F")
                        {
                            yr = nxt.ToString();
                            yr = "EXP/" + dr["vchnum"].ToString().Trim().Substring(2, 4) + "/" + CURR + "-" + yr + "";
                            dr["EXP_YR"] = yr;
                        }
                    }

                    dt.TableName = "Prepcur";
                    repCount = 4;
                    if (iconID == "F50106" || iconID == "F55106") repCount = 1;
                    if (frm_vty == "4F")
                    {
                        repCount = 4;
                    }
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));


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
                            col1 = dt.Rows[i]["fstr"].ToString().Trim();
                            #region PPAP
                            if (frm_cocd.Equals("PPAP"))
                            {
                                col2 = col2 + "AC"
                                       + dt.Rows[i]["pordno"].ToString().Trim().Replace("/", "")
                                       + dt.Rows[i]["cpartno"].ToString().Trim().Replace("/", "")
                                       + (char)13
                                       + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "")
                                       + (char)9
                                       + dt.Rows[i]["vchdate"].ToString().Trim().Replace("/", "")
                                       + dt.Rows[i]["qty"].ToString().Trim().Replace("/", "")
                                       + (char)9
                                       + dt.Rows[i]["bill_tot"].ToString().Trim().Replace("/", "")
                                       + (char)9
                                       + "8708.99.00"
                                       + "0.00"
                                       + (char)9
                                       + dt.Rows[i]["cess_pu"].ToString().Trim().Replace("/", "")
                                       + (char)9;
                                if (dt.Rows[i]["iopr"].ToString().Trim().Replace("/", "").ToUpper().Equals("IG"))
                                {
                                    col2 = col2 + dt.Rows[i]["exc_amt"].ToString().Trim().Replace("/", "");
                                }
                                col2 = col2 + (char)9
                                    + "0.00"
                                    + (char)9
                                     + dt.Rows[i]["irate"].ToString().Trim().Replace("/", "")
                                      + (char)9
                                      + dt.Rows[i]["iamount"].ToString().Trim().Replace("/", "")
                                      + (char)9;
                                if (dt.Rows[i]["iopr"].ToString().Trim().Replace("/", "").ToUpper().Equals("CG"))
                                {
                                    col2 = col2 + dt.Rows[i]["exc_amt"].ToString().Trim().Replace("/", "");
                                }
                                col2 = col2 + (char)9
                                     + "0.00"
                                     + (char)9
                                       + "0.00"
                                     + (char)9
                                   + dt.Rows[i]["iamount"].ToString().Trim().Replace("/", "")
                                         + (fgen.make_double(dt.Rows[i]["qty"].ToString().Trim().Replace("/", "")) * fgen.make_double(dt.Rows[i]["iexc_Addl"].ToString().Trim().Replace("/", ""))).ToString()
                                         + (char)9
                                           + "0.00"
                                     + (char)9
                                     + dt.Rows[i]["iexc_Addl"].ToString().Trim().Replace("/", "")
                                      + (char)9
                                           + "0.00"
                                     + (char)9
                                     + mq10
                                   + (char)9;
                            }
                            #endregion
                        }
                        fpath =HttpContext.Current.Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        if (frm_cocd == "PPAP") fgen.prnt_QRbar(frm_cocd, col2, col1.Replace("*", "").Replace("/", "") + ".png");
                        else if (frm_cocd == "WING")
                        {
                            fpath = HttpContext.Current.Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "").Replace(",", ""));
                            fgen.FILL_ERR(fpath);
                            fgen.prnt_2Dbar32bit(frm_cocd, col1, fpath);
                            fpath = fpath + ".bmp";
                            frm_rptName = "std_inv2d";
                        }
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

                    ////csmst                
                    //SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count <= 0)
                    //{
                    //    dt = new DataTable();
                    //    SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //}
                    //dt.TableName = "csmst";
                    //dsRep.Tables.Add(dt);

                    // inv terms
                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='INV' ORDER BY SRNO";
                    if (iconID == "F50106" || iconID == "F55106")
                        SQuery = "SELECT DISTINCT udf_name||' '||udf_value AS POTERMS,SRNO FROM udf_data WHERE BRANCHCD='" + frm_mbr + "' AND PAR_FLD='" + frm_mbr + frm_vty + barCode.Replace("'", "") + "' ORDER BY SRNO";
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
                    //btnexpwithsig.Visible = true;
                    if (frm_cocd == "STUD")
                    {
                        if (frm_vty == "44")
                        {
                            frm_rptName = "std_inv_stud44";
                        }
                        else if (frm_vty == "43")
                        {
                            frm_rptName = "std_inv_stud43";
                        }
                        else if (frm_vty == "4F")
                        {
                            frm_rptName = "ExpInv_STUD";
                        }
                        else
                        {
                            frm_rptName = "std_inv_stud_all";
                        }
                    }
                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);
                    if (frm_vty == "4F")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_EXP", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                    }
                    else
                    {
                        if (frm_cocd == "KRSM")
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_bank", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                        }
                        else
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                        }
                    }
                    // btnexpwithsig_Click(null, null); // FOR DIRECT DOWNLOAD
                }
                else
                {
                    data_found = "N";
                }
                //printDefault(frm_cocd, frm_mbr, "std_invcl", "std_invcl", dsRep, "Invoice Challan");
                #endregion
                break;

            case "F50111":
                header_n = "Despatch Note";
                dt = new DataTable();
                //SQuery = "SELECT '" + header_n + "' AS HEADER,trim(a.acode) as acode,D.NAME,TRIM(B.ANAME) AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,a.packno,to_char(a.packdate,'dd/mm/yyyy') as vchdate,a.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT, A.PORDNO,A.PORDDT,A.ICODE,C.INAME,C.CPARTNO,A.ORDLINE,A.QTYSUPP AS QTY,A.QTYORD AS ORD_qTY,A.IRATE,C.UNIT,nvl(a.cscode,'-') as cscode,nvl(g.ANAME,'-') AS CONSG,nvl(g.addr1,'-') as cdr1,nvl(g.addr2,'-') as cadr2,nvl(g.addr3,'-') as cadr3 FROM DESPATCH  a left outer join csmst G on trim(a.cscode)=trim(g.acode),FAMST B ,ITEM C ,TYPE D  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(a.TYPE)=TRIM(D.TYPE1) AND D.ID='V' AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and TRIM(A.PACKNO)||TO_CHAR(A.PACKDATE,'DD/MM/YYYY') in ('" + barCode + "')";
                SQuery = "SELECT '" + header_n + "' AS HEADER,trim(a.acode) as acode,D.NAME,TRIM(B.ANAME) AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,a.packno,to_char(a.packdate,'dd/mm/yyyy') as vchdate,a.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT, A.PORDNO,A.PORDDT,A.ICODE,C.INAME,C.CPARTNO,A.ORDLINE,A.QTYSUPP AS QTY,A.QTYORD AS ORD_qTY,A.IRATE,C.UNIT,nvl(a.cscode,'-') as cscode,A.NO_BDLS AS ROLL,A.WEIGHT AS STD_PKG,nvl(g.ANAME,'-') AS CONSG,nvl(g.addr1,'-') as cdr1,nvl(g.addr2,'-') as cadr2,nvl(g.addr3,'-') as cadr3 FROM DESPATCH  a left outer join csmst G on trim(a.cscode)=trim(g.acode),FAMST B ,ITEM C ,TYPE D  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(a.TYPE)=TRIM(D.TYPE1) AND D.ID='V' AND  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and TRIM(A.PACKNO)||TO_CHAR(A.PACKDATE,'DD/MM/YYYY') in ('" + barCode + "')";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    frm_rptName = "std_Disp_Adv";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Disp_Adv", "std_Disp_Adv", dsRep, "std_Disp_Adv");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49212":
                #region INV
                if (xprdRange.Length == 1)
                {
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                }

                cond = "and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "')";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1) { cond = "and TRIM(a.vchnum)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "' and a.vchdate " + xprdRange + " "; }
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) { cond = "and TRIM(a.vchnum) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and a.vchdate " + xprdRange + " "; }
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR5").Length > 1) cond = cond + " and trim(a.acode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR5") + "' ";

                yr = "";
                CURR = frm_cDt1.Substring(8, 2);
                nxt = Convert.ToInt32(CURR) + 1;

                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";
                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,a.acode from ivoucher a,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";

                if (frm_cocd == "STUD")
                {
                    if (frm_vty == "4F")
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER,'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect,a.iweight,b.payment,nvl(a.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,f.telnum as tpt_telnum,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,h.invno AS Hinvno,TO_CHAR(h.invdate,'DD/MM/YYYY') AS Hinvdate,h.ship2,h.ship3,h.ship4,h.ship5,h.lbnetwt,h.REMARK3 AS NETWT,h.lbgrswt,h.exprmk1,h.exprmk2,h.exprmk3,h.exprmk4,h.exprmk5,h.addl1,h.addl2,h.addl3,h.addl4,h.addl5,h.tmaddl1,h.tmaddl2,h.tmaddl3,h.addl6 from ivoucher a left join hundi h on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h.branchcd)||trim(h.acode)||trim(h.invno)||to_char(h.invdate,'dd/mm/yyyy'),sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";
                    }
                    else
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,B.COUNTRY,d.packsize,f.telnum as tpt_telnum,nvl(a.et_paid,0) as et_paid,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucher a,sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";
                    }
                }

                if (frm_rptName.Length < 2)
                {
                    if (iconID == "F1006A" || iconID == "F50101" || iconID == "F50106") frm_rptName = "std_inv";
                    if (frm_cocd == "AGRM") frm_rptName = "std_inv_agrm";
                    if (iconID == "F50106" && frm_cocd == "MULT") frm_rptName = "std_Perf_inv";
                }
                if (frm_cocd == "SAIA") frm_rptName = "std_inv_saia";
                if (frm_rptName.Length < 2) frm_rptName = "std_inv";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    if (frm_vty != "4F")
                    {
                        DataTable dtHSDetails = new DataTable();
                        dtHSDetails.Columns.Add("FSTR", typeof(string));
                        dtHSDetails.Columns.Add("HSCODE_H", typeof(string));
                        dtHSDetails.Columns.Add("AMT", typeof(decimal));
                        dtHSDetails.Columns.Add("CGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("SGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("IGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("GST_H", typeof(decimal));

                        dtHSDetails = dt.AsEnumerable().GroupBy(r => new
                        {
                            fstr = r.Field<string>("FSTR"),
                            hscode = r.Field<string>("HSCODE").Trim(),
                            cgst = r.Field<decimal>("CGST"),
                            sgst = r.Field<decimal>("SGST"),
                            igst = r.Field<decimal>("CGST")
                        })
                             .Select(g =>
                             {
                                 var row = dtHSDetails.NewRow();
                                 row["FSTR"] = g.Key.fstr;
                                 row["HSCODE_H"] = g.Key.hscode;
                                 if (g.Key.sgst > 0)
                                 {
                                     row["CGST_H"] = g.Key.cgst;
                                     row["SGST_H"] = g.Key.sgst;
                                     row["IGST_H"] = 0;
                                 }
                                 else
                                 {
                                     row["CGST_H"] = 0;
                                     row["SGST_H"] = 0;
                                     row["IGST_H"] = g.Key.igst;
                                 }
                                 row["AMT"] = g.Sum(r => r.Field<decimal>("IAMOUNT"));
                                 row["GST_H"] = g.Sum(r => r.Field<decimal>("EXC_AMT")) + g.Sum(r => r.Field<decimal>("CESS_PU"));
                                 return row;
                             }).CopyToDataTable();

                        dtHSDetails.TableName = "dtHSDetails";
                        dsRep.Tables.Add(dtHSDetails);

                        //DataTable dtHSDetails = new DataTable();
                        //dtHSDetails.Columns.Add("FSTR", typeof(string));
                        //dtHSDetails.Columns.Add("HSCODE_H", typeof(string));
                        //dtHSDetails.Columns.Add("AMT", typeof(decimal));
                        //dtHSDetails.Columns.Add("CGST_H", typeof(decimal));
                        //dtHSDetails.Columns.Add("SGST_H", typeof(decimal));
                        //dtHSDetails.Columns.Add("IGST_H", typeof(decimal));
                        //dtHSDetails.Columns.Add("GST_H", typeof(decimal));

                        //dtHSDetails = dt.AsEnumerable().GroupBy(r => new
                        //{
                        //    fstr = r.Field<string>("FSTR"),
                        //    hscode = r.Field<string>("HSCODE").Trim(),
                        //    cgst = r.Field<decimal>("CGST"),
                        //    sgst = r.Field<decimal>("SGST"),
                        //    igst = r.Field<decimal>("CGST")
                        //})
                        //     .Select(g =>
                        //     {
                        //         var row = dtHSDetails.NewRow();
                        //         row["FSTR"] = g.Key.fstr;
                        //         row["HSCODE_H"] = g.Key.hscode;
                        //         if (g.Key.sgst > 0)
                        //         {
                        //             row["CGST_H"] = g.Key.cgst;
                        //             row["SGST_H"] = g.Key.sgst;
                        //             row["IGST_H"] = 0;
                        //         }
                        //         else
                        //         {
                        //             row["CGST_H"] = 0;
                        //             row["SGST_H"] = 0;
                        //             row["IGST_H"] = g.Key.igst;
                        //         }
                        //         row["AMT"] = g.Sum(r => r.Field<decimal>("IAMOUNT"));
                        //         row["GST_H"] = g.Sum(r => r.Field<decimal>("EXC_AMT")) + g.Sum(r => r.Field<decimal>("CESS_PU"));

                        //         return row;
                        //     }
                        //  ).CopyToDataTable();

                        //dtHSDetails.TableName = "dtHSDetails";
                        //dsRep.Tables.Add(dtHSDetails);
                    }
                    dt.Columns.Add(new DataColumn("amtToword", typeof(string)));
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    if (frm_vty == "4F")
                    {
                        dt.Columns.Add("EXP_YR", typeof(string));
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                        dr["amtToword"] = fgen.ConvertNumbertoWords(dr["bill_tot"].ToString().Trim());
                        if (frm_vty == "4F")
                        {
                            yr = nxt.ToString();
                            yr = "EXP/" + dr["vchnum"].ToString().Trim().Substring(2, 4) + "/" + CURR + "-" + yr + "";
                            dr["EXP_YR"] = yr;
                        }
                    }

                    dt.TableName = "Prepcur";
                    repCount = 1;
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));

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
                        }
                        fpath =HttpContext.Current.Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        if (frm_cocd == "PPAP") fgen.prnt_QRbar(frm_cocd, col2, col1.Replace("*", "").Replace("/", "") + ".png");
                        else if (frm_cocd == "WING")
                        {
                            fpath = HttpContext.Current.Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "").Replace(",", ""));
                            fgen.prnt_2Dbar32bit(frm_cocd, col1, fpath);
                            fpath = fpath + ".bmp";
                            frm_rptName = "std_inv2d";
                        }
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
                    //SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count <= 0)
                    //{
                    //    dt = new DataTable();
                    //    SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //}
                    //dt.TableName = "csmst";
                    //dsRep.Tables.Add(dt);

                    // inv terms
                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='INV' ORDER BY SRNO";
                    if (iconID == "F50106")
                        SQuery = "SELECT DISTINCT udf_name||' '||udf_value AS POTERMS,SRNO FROM udf_data WHERE BRANCHCD='" + frm_mbr + "' AND PAR_FLD='" + frm_mbr + frm_vty + barCode.Replace("'", "") + "' ORDER BY SRNO";
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
                    //btnexpwithsig.Visible = true;

                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);

                    if (frm_cocd == "STUD")
                    {
                        if (frm_vty == "44")
                        {
                            frm_rptName = "std_inv_stud44";
                        }
                        else if (frm_vty == "43")
                        {
                            frm_rptName = "std_inv_stud43";
                        }
                        else if (frm_vty == "4F")
                        {
                            frm_rptName = "ExpInv_STUD";
                        }
                        else
                        {
                            frm_rptName = "std_inv_stud_all";
                        }
                    }
                    if (frm_vty == "4F")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_EXP", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                    }
                    else
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                    }
                    // btnexpwithsig_Click(null, null); // FOR DIRECT DOWNLOAD                                       
                }
                else
                {
                    data_found = "N";
                }
                //printDefault(frm_cocd, frm_mbr, "std_invcl", "std_invcl", dsRep, "Invoice Challan");
                #endregion
                break;
            // ------------ MERGE BY MADHVI ON 13TH JAN 2018 , MADE BY YOGITA ---------- //

            case "F50266": //new code 14 feb 2019...yet not merged only pdf is send to client for testing           
                #region Material Lying with Godown  (invoice-summary)

                //for financial year
                int CurrentYear = DateTime.Today.Year;
                int PreviousYear = DateTime.Today.Year - 1;
                int NextYear = DateTime.Today.Year + 1;
                string PreYear = PreviousYear.ToString();
                string NexYear = NextYear.ToString();
                string CurYear = CurrentYear.ToString();
                string FinYear = null;

                if (DateTime.Today.Month > 3)
                    FinYear = CurYear + "-" + NexYear;
                else
                    FinYear = PreYear + "-" + CurYear;
                mq6 = FinYear.Substring(2, 2);
                ///////
                ph_tbl = new DataTable();
                header_n = "Material Lying with Godown(Invoice Wise-Detail)";
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("days", typeof(string));
                ph_tbl.Columns.Add("sno", typeof(string));
                ph_tbl.Columns.Add("acode", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("part", typeof(string));
                ph_tbl.Columns.Add("invno", typeof(string));
                ph_tbl.Columns.Add("invdate", typeof(string));
                ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                ph_tbl.Columns.Add("inv_qty", typeof(double));
                ph_tbl.Columns.Add("lifted_qty", typeof(double));
                ph_tbl.Columns.Add("bal_qty", typeof(double));
                ph_tbl.Columns.Add("rate", typeof(double));
                ph_tbl.Columns.Add("amount", typeof(double));
                mq5 = ""; mq11 = "";
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col4");
                mq11 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");

                DateTime tim = Convert.ToDateTime(mq11).AddDays(-Convert.ToInt32(mq5));
                xprdRange = " between to_date('05/07/2018','dd/mm/yyyy') and to_date('" + tim.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                xprdRange = " between to_date('01/01/2018','dd/mm/yyyy') and to_date('" + tim.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";  //NEW


                // SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select trim(vchnum) as vchnum,LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,0 as picked,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,0 as picked,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE " + xprdRange + " and store_no='Y'  union all  select trim(vchnum) as vchnum,null AS VCHDATE,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4,0 AS RATE from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate " + xprdRange + " and  trim(acode) ='" + mq1 + "' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink"; //as per bansal sir...rate comes from warehouse master

                SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                SQuery = "select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F'  and trim(acode) ='" + mq1 + "' and vchdate  " + xprdRange + " and store='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum "; //transaction table
                dt1 = new DataTable();//
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //old
                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //new 4feb19
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = ""; mq4 = "";
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["days"] = mq5;
                    dr1["sno"] = i + 1;
                    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                    mq3 = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["erp_code"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                    dr1["rate"] = db4;
                    dr1["inv_qty"] = db3;
                    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                    db2 = db * db1;
                    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                    if (db != 0)
                    {
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL_InvWise", "Mat_Lying_wid_Godown_ERAL_InvWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50267":
                #region
                ////for financial year
                //CurrentYear = DateTime.Today.Year;
                //PreviousYear = DateTime.Today.Year - 1;
                //NextYear = DateTime.Today.Year + 1;
                //PreYear = PreviousYear.ToString();
                //NexYear = NextYear.ToString();
                //CurYear = CurrentYear.ToString();
                //FinYear = null;
                //if (DateTime.Today.Month > 3)
                //    FinYear = CurYear + "-" + NexYear;
                //else
                //    FinYear = PreYear + "-" + CurYear;
                //mq6 = FinYear.Substring(2, 2);
                ////Material Lying with Godown  (item wise-DETAIL)
                //ph_tbl = new DataTable();
                //header_n = "Material Lying with Godown(Item Wise-Detail)";
                //ph_tbl.Columns.Add("header", typeof(string));
                //ph_tbl.Columns.Add("days", typeof(string));
                //ph_tbl.Columns.Add("sno", typeof(string));
                //ph_tbl.Columns.Add("acode", typeof(string));
                //ph_tbl.Columns.Add("aname", typeof(string));
                //ph_tbl.Columns.Add("icode", typeof(string));
                //ph_tbl.Columns.Add("iname", typeof(string));
                //ph_tbl.Columns.Add("part", typeof(string));
                //ph_tbl.Columns.Add("invno", typeof(string));
                //ph_tbl.Columns.Add("invdate", typeof(string));
                //ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                //ph_tbl.Columns.Add("inv_qty", typeof(double));
                //ph_tbl.Columns.Add("lifted_qty", typeof(double));
                //ph_tbl.Columns.Add("bal_qty", typeof(double));
                //ph_tbl.Columns.Add("rate", typeof(double));
                //ph_tbl.Columns.Add("amount", typeof(double));
                //mq5 = "";
                ////mq5 = fgen.seek_iname(frm_qstr, frm_cDt1, "select to_date('" + fromdt + "','dd/mm/yyyy')-to_date('" + todt + "','dd/mm/yyyy') as days from dual", "days");
                //mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                //mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");


                //SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                //SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                //dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                //mq0 = "select  vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE ='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate";
                //dt2 = new DataTable();
                //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = "";
                //    dr1 = ph_tbl.NewRow();
                //    dr1["header"] = header_n;
                //    dr1["sno"] = i + 1;
                //    dr1["days"] = mq5;
                //    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                //    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                //    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                //    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                //    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                //    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                //    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                //    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                //    dr1["inv_qty"] = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    //   dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                //    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));

                //    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                //    dr1["rate"] = db4;
                //    dr1["inv_qty"] = db3;
                //    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                //    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                //    db2 = db * db1;
                //    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                //    if (db != 0)
                //    {
                //        ph_tbl.Rows.Add(dr1);
                //    }
                //}
                //if (ph_tbl.Rows.Count > 0)
                //{
                //    ph_tbl.TableName = "Prepcur";
                //    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                //    Print_Report_BYDS_pdf(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL", "Mat_Lying_wid_Godown_ERAL", dsRep, header_n);
                //}
                //else
                //{
                //    data_found = "N";
                //}
                #endregion
                #region Material Lying with Godown  (item wise-Detail)

                ph_tbl = new DataTable();
                header_n = "Material Lying with Godown(item wise-Detail)";
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("days", typeof(string));
                ph_tbl.Columns.Add("sno", typeof(string));
                ph_tbl.Columns.Add("acode", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("part", typeof(string));
                ph_tbl.Columns.Add("invno", typeof(string));
                ph_tbl.Columns.Add("invdate", typeof(string));
                ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                ph_tbl.Columns.Add("inv_qty", typeof(double));
                ph_tbl.Columns.Add("lifted_qty", typeof(double));
                ph_tbl.Columns.Add("bal_qty", typeof(double));
                ph_tbl.Columns.Add("rate", typeof(double));
                ph_tbl.Columns.Add("amount", typeof(double));
                ph_tbl.Columns.Add("rep_dt", typeof(string));
                mq5 = ""; mq11 = ""; mq1 = ""; mq10 = "";
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                mq10 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                mq11 = fgen.seek_iname(frm_qstr, frm_cDt1, "select to_date('" + mq10 + "','dd/mm/yyyy')-to_date('" + todt + "','dd/mm/yyyy') as days from dual", "days");

                xprdRange = " between to_date('01/01/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";  //NEW


                // SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select trim(vchnum) as vchnum,LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,0 as picked,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,0 as picked,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE " + xprdRange + " and store_no='Y'  union all  select trim(vchnum) as vchnum,null AS VCHDATE,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4,0 AS RATE from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate " + xprdRange + " and  trim(acode) ='" + mq1 + "' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink"; //as per bansal sir...rate comes from warehouse master

                SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                SQuery = "select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F'  and trim(acode) ='" + mq1 + "' and vchdate  " + xprdRange + " and store='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum "; //transaction table
                dt1 = new DataTable();//
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //old
                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //new 4feb19
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = ""; mq4 = "";
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["days"] = mq11;
                    dr1["sno"] = i + 1;
                    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                    mq3 = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["erp_code"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                    dr1["rate"] = db4;
                    dr1["inv_qty"] = db3;
                    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                    db2 = db * db1;
                    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                    dr1["rep_dt"] = Convert.ToString(todt);
                    if (db != 0)
                    {
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL", "Mat_Lying_wid_Godown_ERAL", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50268":
                #region this table for detail
                //ph_tbl = new DataTable();
                //header_n = "Material Lying with Godown(Item Wise-Concise)";
                //ph_tbl.Columns.Add("header", typeof(string));
                //ph_tbl.Columns.Add("days", typeof(string));
                //ph_tbl.Columns.Add("sno", typeof(string));
                //ph_tbl.Columns.Add("acode", typeof(string));
                //ph_tbl.Columns.Add("aname", typeof(string));
                //ph_tbl.Columns.Add("icode", typeof(string));
                //ph_tbl.Columns.Add("iname", typeof(string));
                //ph_tbl.Columns.Add("part", typeof(string));
                //ph_tbl.Columns.Add("invno", typeof(string));
                //ph_tbl.Columns.Add("invdate", typeof(string));
                //ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                //ph_tbl.Columns.Add("inv_qty", typeof(double));
                //ph_tbl.Columns.Add("lifted_qty", typeof(double));
                //ph_tbl.Columns.Add("bal_qty", typeof(double));
                //ph_tbl.Columns.Add("rate", typeof(double));
                //ph_tbl.Columns.Add("amount", typeof(double));
                /////// dtm table using for sumamry
                //dtm = new DataTable();
                //dtm.Columns.Add("header", typeof(string));
                //dtm.Columns.Add("days", typeof(string));
                //dtm.Columns.Add("sno", typeof(string));
                //dtm.Columns.Add("acode", typeof(string));
                //dtm.Columns.Add("aname", typeof(string));
                //dtm.Columns.Add("icode", typeof(string));
                //dtm.Columns.Add("iname", typeof(string));
                //dtm.Columns.Add("part", typeof(string));
                //dtm.Columns.Add("invno", typeof(string));
                //dtm.Columns.Add("invdate", typeof(string));
                //dtm.Columns.Add("mat_lift_dt", typeof(string));
                //dtm.Columns.Add("inv_qty", typeof(double));
                //dtm.Columns.Add("lifted_qty", typeof(double));
                //dtm.Columns.Add("bal_qty", typeof(double));
                //dtm.Columns.Add("rate", typeof(double));
                //dtm.Columns.Add("amount", typeof(double));
                /////
                //mq5 = "";
                //mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                //mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                ////SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,max(A.vchdate) as vchdate,max(a.vdd) as vdd1,max(a.rate_val) as rate,trim(a.acode) as acode,trim(c.aname) as aname from (select trim(vchnum) as vchnum,LOC_REF AS VCHDATE, LOC_REF as vdd, trim(icode) as icode,trim(maincode) as acode,sum(iqtyin) as op,0 as inv,0 as picked,wolink,sum(ngqty*IQTYIN)/sum(iqtyin) AS RATE_VAL from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' group by trim(vchnum) ,LOC_REF ,trim(icode) ,trim(maincode) ,wolink union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,0 as op,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and vchdate " + xprdRange + " and trim(acode) ='" + mq1 + "' and store_no='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum  union all   select trim(vchnum) as vchnum,null AS VCHDATE, null as vdd,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,col4 as col4,0 AS RATE_VAL from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate  " + xprdRange + "  and  trim(acode) ='" + mq1 + "') a,item b,famst c  where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vdd1,wolink ";               
                ////new 14 FEB 2019
                //SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                //SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                //dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                //mq0 = "select  vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE ='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate";
                //dt2 = new DataTable();
                //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = "";
                //    dr1 = ph_tbl.NewRow();
                //    dr1["header"] = header_n;
                //    dr1["sno"] = i + 1;
                //    dr1["days"] = mq5;
                //    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                //    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                //    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                //    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                //    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                //    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                //    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                //    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                //    dr1["inv_qty"] = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                //    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                //    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                //    dr1["rate"] = db4;
                //    dr1["inv_qty"] = db3;
                //    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                //    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                //    db2 = db * db1;
                //    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                //    if (db != 0)
                //    {
                //        ph_tbl.Rows.Add(dr1);
                //    }
                //}

                //if (ph_tbl.Rows.Count > 0)
                //{
                //    DataView view1im = new DataView(ph_tbl);
                //    DataTable dtdrsim = new DataTable();
                //    dtdrsim = view1im.ToTable(true, "ACODE", "icode"); //MAIN                  
                //    foreach (DataRow dr0 in dtdrsim.Rows)
                //    {
                //        DataView view2 = new DataView(ph_tbl, "acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //        dt5 = new DataTable();
                //        dt5 = view2.ToTable();
                //        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; mq2 = ""; mq3 = "";
                //        dr1 = dtm.NewRow();
                //        for (int i = 0; i < dt5.Rows.Count; i++)
                //        {
                //            dr1["header"] = header_n;
                //            dr1["days"] = dt5.Rows[i]["days"].ToString().Trim();
                //            dr1["sno"] = i + 1;
                //            dr1["acode"] = dt5.Rows[i]["acode"].ToString().Trim();
                //            dr1["aname"] = dt5.Rows[i]["aname"].ToString().Trim();
                //            dr1["icode"] = dt5.Rows[i]["icode"].ToString().Trim();
                //            dr1["iname"] = dt5.Rows[i]["iname"].ToString().Trim();
                //            dr1["part"] = dt5.Rows[i]["part"].ToString().Trim();
                //            dr1["mat_lift_dt"] = dt5.Rows[i]["mat_lift_dt"].ToString().Trim();
                //            db3 += fgen.make_double(dt5.Rows[i]["inv_qty"].ToString().Trim());
                //            dr1["inv_qty"] = db3;
                //            db4 += fgen.make_double(dt5.Rows[i]["lifted_qty"].ToString().Trim());
                //            dr1["lifted_qty"] = db4;
                //            db5 += fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                //            dr1["bal_qty"] = db5;
                //            dr1["rate"] = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                //            db = fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                //            db1 = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                //            db2 += db * db1;
                //            dr1["amount"] = db2;
                //        }
                //        if (db != 0)
                //        {
                //            dtm.Rows.Add(dr1);
                //        }
                //    }
                //}
                //if (dtm.Rows.Count > 0)
                //{
                //    dtm.TableName = "Prepcur";
                //    dsRep.Tables.Add(fgen.mTitle(dtm, repCount));
                //    Print_Report_BYDS_pdf(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL_ItemWise", "Mat_Lying_wid_Godown_ERAL_ItemWise", dsRep, header_n);
                //}
                //else
                //{
                //    data_found = "N";
                //}
                #endregion
                #region Material Lying with Godown  (Item Wise-Summary)

                ph_tbl = new DataTable();
                header_n = "Material Lying with Godown(Item Wise-Summary)";
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("days", typeof(string));
                ph_tbl.Columns.Add("sno", typeof(string));
                ph_tbl.Columns.Add("acode", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("part", typeof(string));
                ph_tbl.Columns.Add("invno", typeof(string));
                ph_tbl.Columns.Add("invdate", typeof(string));
                ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                ph_tbl.Columns.Add("inv_qty", typeof(double));
                ph_tbl.Columns.Add("lifted_qty", typeof(double));
                ph_tbl.Columns.Add("bal_qty", typeof(double));
                ph_tbl.Columns.Add("rate", typeof(double));
                ph_tbl.Columns.Add("amount", typeof(double));
                ph_tbl.Columns.Add("rep_dt", typeof(string));
                ///// dtm table using for sumamry
                dtm = new DataTable();
                dtm.Columns.Add("header", typeof(string));
                dtm.Columns.Add("days", typeof(string));
                dtm.Columns.Add("sno", typeof(string));
                dtm.Columns.Add("acode", typeof(string));
                dtm.Columns.Add("aname", typeof(string));
                dtm.Columns.Add("icode", typeof(string));
                dtm.Columns.Add("iname", typeof(string));
                dtm.Columns.Add("part", typeof(string));
                dtm.Columns.Add("invno", typeof(string));
                dtm.Columns.Add("invdate", typeof(string));
                dtm.Columns.Add("mat_lift_dt", typeof(string));
                dtm.Columns.Add("inv_qty", typeof(double));
                dtm.Columns.Add("lifted_qty", typeof(double));
                dtm.Columns.Add("bal_qty", typeof(double));
                dtm.Columns.Add("rate", typeof(double));
                dtm.Columns.Add("amount", typeof(double));
                dtm.Columns.Add("rep_dt", typeof(string));
                ///
                mq5 = ""; mq11 = ""; mq1 = ""; mq10 = "";
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                mq10 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                mq11 = fgen.seek_iname(frm_qstr, frm_cDt1, "select to_date('" + mq10 + "','dd/mm/yyyy')-to_date('" + todt + "','dd/mm/yyyy') as days from dual", "days");

                xprdRange = " between to_date('01/01/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";  //NEW


                // SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select trim(vchnum) as vchnum,LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,0 as picked,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,0 as picked,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE " + xprdRange + " and store_no='Y'  union all  select trim(vchnum) as vchnum,null AS VCHDATE,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4,0 AS RATE from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate " + xprdRange + " and  trim(acode) ='" + mq1 + "' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink"; //as per bansal sir...rate comes from warehouse master

                SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                SQuery = "select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F'  and trim(acode) ='" + mq1 + "' and vchdate  " + xprdRange + " and store='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum "; //transaction table
                dt1 = new DataTable();//
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //old
                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //new 4feb19
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = ""; mq4 = "";
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["days"] = mq11;
                    dr1["sno"] = i + 1;
                    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                    mq3 = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["erp_code"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                    dr1["rate"] = db4;
                    dr1["inv_qty"] = db3;
                    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                    db2 = db * db1;
                    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                    dr1["rep_dt"] = Convert.ToString(todt);
                    if (db != 0)
                    {
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    DataView view1im = new DataView(ph_tbl);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "ACODE", "icode"); //MAIN                  
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataView view2 = new DataView(ph_tbl, "acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt5 = new DataTable();
                        dt5 = view2.ToTable();
                        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; mq2 = ""; mq3 = "";
                        dr1 = dtm.NewRow();
                        for (int i = 0; i < dt5.Rows.Count; i++)
                        {
                            dr1["header"] = header_n;
                            dr1["days"] = dt5.Rows[i]["days"].ToString().Trim();
                            dr1["sno"] = i + 1;
                            dr1["acode"] = dt5.Rows[i]["acode"].ToString().Trim();
                            dr1["aname"] = dt5.Rows[i]["aname"].ToString().Trim();
                            dr1["icode"] = dt5.Rows[i]["icode"].ToString().Trim();
                            dr1["iname"] = dt5.Rows[i]["iname"].ToString().Trim();
                            dr1["part"] = dt5.Rows[i]["part"].ToString().Trim();
                            dr1["mat_lift_dt"] = dt5.Rows[i]["mat_lift_dt"].ToString().Trim();
                            db3 += fgen.make_double(dt5.Rows[i]["inv_qty"].ToString().Trim());
                            dr1["inv_qty"] = db3;
                            db4 += fgen.make_double(dt5.Rows[i]["lifted_qty"].ToString().Trim());
                            dr1["lifted_qty"] = db4;
                            db5 += fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                            dr1["bal_qty"] = db5;
                            dr1["rate"] = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                            db = fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                            db1 = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                            db2 += db * db1;
                            dr1["amount"] = db2;
                            dr1["rep_dt"] = dt5.Rows[i]["rep_dt"].ToString().Trim();
                        }
                        if (db != 0)
                        {
                            dtm.Rows.Add(dr1);
                        }
                    }
                }
                if (dtm.Rows.Count > 0)
                {
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dtm, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL_ItemWise", "Mat_Lying_wid_Godown_ERAL_ItemWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50141":
                // Sales Register (Dom.)
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type IN (" + frm_vty + ")  AND A.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY A.morder";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sale_REG", "std_Sale_REG", dsRep, "Sales Register (Dom.)");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50142":
                #region
                // Cust. Wise Register (Dom.)
                //mq12 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //if (mq12.Length > 0)
                //{
                // SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,'Customer Wise Sales Report (Dom.)' as header, a.*, trim(a.acode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1  from ivoucher a,famst b,item i where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type='" + frm_vty + "' AND A.vchdate " + xprdRange + " and a.acode in (" + mq12 + ") ORDER BY vdd,A.morder";
                SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Customer Wise Sales Report (Dom.)' as header, a.*, trim(a.acode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY A.morder";
                //}
                //else
                //{
                //    SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Customer Wise Sales Report (Dom.)' as header, a.*, trim(a.acode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.acode like '%' ORDER BY A.morder";
                //}
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Sale_REG", "std_Party_Sale_REG", dsRep, "Party Wise Sales Register (Dom.)");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50143":
                #region
                // Product Wise Register (Dom.)
                // mq12 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                //if (mq12.Length > 0)
                //{
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Product Wise Sales Report (Dom.)' as header, a.*, trim(a.icode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'  ORDER BY A.icode,a.vchdate,a.vchnum";
                //}
                //else
                //{
                //    SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Product Wise Sales Report (Dom.)' as header, a.*, trim(a.icode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.icode like '%' and nvl(a.iqtyout,0)>0 ORDER BY A.morder";
                //}
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Item_Sale_REG", "std_Item_Sale_REG", dsRep, "Item Wise Sales Register (Dom.)");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50144":
                #region
                // Domestic Proforma Invoice Print
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,trim(a.desc_) as desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,a.vchdate as vdd from ivoucherp a,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' AND trim(a.icode)=trim(d.icode)  AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + mq1 + ") order by vdd,a.vchnum,a.morder";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    SQuery = "select c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as  remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,'0' as tcsamt  from salep c where  TRIM(c.BRANCHCD)||TRIM(c.TYPE)||TRIM(c.VCHNUM)||TO_CHAR(c.VCHDATE,'DD/MM/YYYY') in (" + mq1 + ") ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.TableName = "SALES_P";
                    dsRep.Tables.Add(dt);
                    //-----------------------------------------------csmst      
                    SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dsRep.Tables[1].Rows[0]["cscode"].ToString().Trim() + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count <= 0)
                    {
                        dt = new DataTable();
                        SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                        SQuery = "SELECT ANAME AS consign ,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 daddr4,'-' as dtel,'-' as dtinno,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM FAMST WHERE ACODE='" + dsRep.Tables[0].Rows[0]["acode"].ToString().Trim() + "'";
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
                    DataRow dr = null;
                    dt1.Columns.Add("poterms", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                    }
                    dr = dt1.NewRow();
                    dr["poterms"] = mq10;
                    dt1.Rows.Add(dr);
                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_PI", "std_inv_PI", dsRep, header_n);
                    HttpContext.Current.Session["mydataset"] = dsRep;
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            // ------------ MERGE BY MADHVI ON 11TH JAN 2018 , MADE BY YOGITA ---------- //

            case "F50222":
                header_n = "Party Wise Total Sale (DOM)";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, B.NAME AS SALESNAME,A.TYPE,A.ACODE,C.ANAME,SUM(BASIC) AS BASIC,SUM(SGST) AS SGST,SUM(IGST) AS IGST,SUM(CGST) AS CGST  FROM(SELECT  A.type,a.acode , A.AMT_SALE AS BASIC,(Case when POST='I' then A.AMT_EXC else 0 end) as IGST,(Case when POST='C' then A.AMT_EXC else 0 end) as CGST,(Case when POST='C' then A.AMT_EXC else 0 end) as SGST FROM SALE A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdRange + " and a.acode like '" + party_cd + "%') A ,FAMST C,TYPE B WHERE  TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(B.TYPE1) AND B.ID='V' group by b.name,a.type,a.acode,C.ANAME ORDER BY A.ACODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Sale_DOM", "std_Party_Sale_DOM", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50223":
                header_n = "Product Wise Total Sale (DOM)";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                // EARLIER IT WAS ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) AS IRATE BUT CHANGED TO (CASE WHEN SUM(IQTYOUT)> 0 THEN ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) ELSE 0 END)AS IRATE BECAUSE IT IS GIVING DIVISOR IS EQUAL TO ZERO ERROR BY MADHVI ON 18 MAY 2018 
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, C.INAME AS SUBGNAME, SUBSTR(TRIM(A.ICODE),1,4) AS SUBGRP,B.INAME,B.CPARTNO, A.ICODE,SUM(A.IQTYOUT) AS QTY,(CASE WHEN SUM(IQTYOUT)> 0 THEN ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) ELSE 0 END)AS IRATE from ivoucher a,item b,ITEM C  where trim(a.icode)=trim(b.icode) and SUBSTR(TRIM(A.ICODE),1,4)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type like '4%' and  a.VCHDATE " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and length(Trim(C.icode))=4 GROUP BY C.INAME, SUBSTR(TRIM(A.ICODE),1,4),b.iname,b.cpartno,a.icode order by b.iname";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, C.INAME AS SUBGNAME, SUBSTR(TRIM(A.ICODE),1,4) AS SUBGRP,B.INAME,B.CPARTNO, A.ICODE,SUM(A.IQTYOUT) AS QTY,(CASE WHEN SUM(IQTYOUT)> 0 THEN ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) ELSE 0 END)AS IRATE from ivoucher a,item b,ITEM C  where trim(a.icode)=trim(b.icode) and SUBSTR(TRIM(A.ICODE),1,4)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type like '4%' and  a.VCHDATE " + xprdRange + " and a.icode like '" + party_cd + "%'  and length(Trim(C.icode))=4 GROUP BY C.INAME, SUBSTR(TRIM(A.ICODE),1,4),b.iname,b.cpartno,a.icode order by b.iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Prod_Sale_DOM", "std_Prod_Sale_DOM", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50224":
                header_n = "Party Wise Total Qty(DOM)";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Length > 2)
                {
                    cond = " and trim(a.icode) in (" + party_cd + ") and trim(a.icode) in (" + party_cd + ")";
                }
                else
                {
                    cond = " and trim(a.icode) like '%' and trim(a.icode) like '%'";
                }
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.morder as store,A.ACODE,C.ANAME,b.INAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select ACODE,icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,morder from IVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + ") a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) " + cond + " group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,a.morder order by store";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Prod_wise", "std_Party_Prod_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50228":
                header_n = "31 Day Sales Report With Quantity";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                // WRITTEN ON PRT_SALE PAGE AS IT IS NOT GIVING ALERT MSG 
                //DateTime date1 = Convert.ToDateTime(fromdt);
                //DateTime date2 = Convert.ToDateTime(todt);
                //TimeSpan days = date2 - date1;
                //if (days.TotalDays > 31)
                //{
                //    fgen.msg("-", "AMSG", "Please Select 31 Days Only"); return;
                //}
                //else
                //{
                #region 31 Days
                SQuery = "SELECT ICODE,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS day,IQTYOUT,TO_CHAR(VCHDATE,'yyyymmdd') AS VCH FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' ORDER BY VCH";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt1 = new DataTable();
                dt1 = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < 31; i++)
                    {
                        DataRow ft = dt1.NewRow();
                        ft["day"] = fgen.padlc(i + 1, 2);
                        ded1 = ft["day"].ToString();
                        dt1.Rows.Add(ft);
                    }
                }
                mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq11 = ""; mq12 = "";
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (mq0.Length > 0)
                    {
                        mq0 = mq0 + ",decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',iqtyout,0) as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq0 = "decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',iqtyout,0) as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    if (mq7.Length > 0)
                    {
                        mq7 = mq7 + ",A.DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq7 = "A.DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    //FOR SUM 
                    if (mq11.Length > 0)
                    {
                        mq11 = mq11 + ",sum(DAY_" + dt1.Rows[j]["day"].ToString().Trim() + ") as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq11 = "sum(DAY_" + dt1.Rows[j]["day"].ToString().Trim() + ") as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                }
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header," + mq7 + ",a.icode,b.iname,d.aname,A.ACODE,c.mthname,a.vch1 from (SELECT " + mq11 + ",ICODE,ACODE,vchdate,vch1 FROM (SELECT to_char(vchdate,'MM') as vchdate,to_char(vchdate,'yyyy') as vch1, ICODE,ACODE,BRANCHCD," + mq0 + " from ivoucher  where branchcd='" + frm_mbr + "' and type like '4%' AND  vchdate " + xprdRange + ") GROUP BY ICODE,vchdate,vch1,acode) a,item b,famst D,mths c WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(D.ACODE) and trim(a.vchdate)=trim(c.mthnum) order by a.acode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Daywise31_Sale", "Daywise31_Sale", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50229":
                header_n = "Party Wise Total Value(DOM)";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, A.morder as STORE, A.ACODE,C.ANAME,b.INAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select ACODE,icode,(Case when to_char(vchdate,'mm')='04' then nvl(iamount,'0')   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(iamount,'0')   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(iamount,'0')   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(iamount,'0')   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(iamount,'0') else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(iamount,'0')   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(iamount,'0')   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(iamount,'0')   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(iamount,'0')   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(iamount,'0')   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(iamount,'0')   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(iamount,'0')   else 0 end) as mar,morder from IVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,A.morder order by STORE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Prod_wise", "std_Party_Prod_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;
            // ------------ MERGE BY MADHVI ON 22ND JAN 2018 , MADE BY YOGITA ON 20TH JAN 2018 ---------- //

            case "F50240":
                header_n = "Schedule Vs Dispatch 31 Day";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                party_cd = frm_ulvl == "M" ? frm_uname : party_cd;
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31,sum(A.Rday1) as Rday1,sum(A.Rday2) as Rday2,sum(A.Rday3) as Rday3,sum(A.Rday4) as Rday4,sum(A.Rday5) as Rday5,sum(A.Rday6) as Rday6,sum(A.Rday7) as Rday7,sum(A.Rday8) as Rday8,sum(A.Rday9) as Rday9, sum(A.Rday10) as Rday10,sum(A.Rday11) as Rday11,sum(A.Rday12) as Rday12,sum(A.Rday13) as Rday13,sum(A.Rday14) as Rday14,sum(A.Rday15) as Rday15,sum(A.Rday16) as Rday16,sum(A.Rday17) as Rday17,sum(A.Rday18) as Rday18,sum(A.Rday19) as Rday19,sum(A.Rday20) as Rday20,sum(A.Rday21) as Rday21,sum(A.Rday22) as Rday22,sum(A.Rday23) as Rday23,sum(A.Rday24) as Rday24,sum(A.Rday25) as Rday25,sum(A.Rday26) as Rday26,sum(A.Rday27) as Rday27,sum(A.Rday28) as Rday28,sum(A.Rday29) as Rday29,sum(A.Rday30) as Rday30,sum(A.Rday31) as Rday31 from (SELECT Acode,icode,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31,0 AS Rday1,0 AS Rday2,0 AS Rday3,0 AS Rday4,0 AS Rday5,0 AS Rday6,0 AS Rday7,0 AS Rday8,0 AS Rday9,0 AS Rday10,0 AS Rday11,0 AS Rday12,0 AS Rday13,0 Rday14,0 AS Rday15,0 AS Rday16,0 AS Rday17,0 AS Rday18,0 AS Rday19,0 AS Rday20,0 AS Rday21,0 AS Rday22,0 AS Rday23,0 AS Rday24,0 AS Rday25,0 AS Rday26,0 AS Rday27,0 AS Rday28,0 AS Rday29,0 AS Rday30,0 AS Rday31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='46' and vchdate " + xprdRange + " UNION ALL SELECT acode,icode,0 as DAY1,0 as day2,0 as day3,0 as day4,0 as day5,0 as day6,0 as day7,0 as day8,0 as day9,0 as day10 ,0 as day11,0 as day12, 0 as day13,0 as day14,0 as day15,0 as day16,0 as day17,0 as day18,0 as day19,0 as day20,0 as day21,0 as day22,0 as day23,0 as day24,0 as day25,0 as day26,0 as day27,0 as day28,0 as day29,0 as day30,0 as day31,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as Rday1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as Rday2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as Rday3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0)  a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' group by a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit order by a.icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Desp_DayWise", "std_Sch_Vs_Desp_DayWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50241":
                header_n = "Schedule Vs Dispatch 12 Month";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                party_cd = frm_ulvl == "M" ? frm_uname : party_cd;
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar from (select acode,icode,(case when to_char(vchdate,'mm')='04' then total else 0 end) as apr,(case when to_char(vchdate,'mm')='05' then total else 0 end) as may,(case when to_char(vchdate,'mm')='06' then total else 0 end) as jun,(case when to_char(vchdate,'mm')='07' then total else 0 end) as jul,(case when to_char(vchdate,'mm')='08' then total else 0 end) as aug,(case when to_char(vchdate,'mm')='09' then total else 0 end) as sep,(case when to_char(vchdate,'mm')='10' then total else 0 end) as oct,(case when to_char(vchdate,'mm')='11' then total else 0 end) as nov,(case when to_char(vchdate,'mm')='12' then total else 0 end) as dec,(case when to_char(vchdate,'mm')='01' then total else 0 end) as jan,(case when to_char(vchdate,'mm')='02' then total else 0 end) as feb,(case when to_char(vchdate,'mm')='03' then total else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar  from schedule where branchcd='" + frm_mbr + "' and type='46' and vchdate " + xprdRange + " union all select acode ,icode,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Desp_Mth", "std_Sch_Vs_Desp_Mth", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            //MAKE AND MERGED BY YOGITA

            case "F50386": //ITEM WISE WISE
            case "F50388"://SUBGROUP WISE
            case "F50390"://MAIN GROUP WISE
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (iconID == "F50386")
                {
                    header_n = "Item Wise Sales Report";
                    if (part_cd.Length < 1 && party_cd.Length < 1)
                    {
                        cond = " and acode like '%' and icode like '%' ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length > 1)
                    {
                        cond = " and acode in (" + party_cd + ") and icode in (" + part_cd + ") ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length < 1)
                    {
                        cond = " and acode in (" + party_cd + ") and icode like '%'";
                    }
                    else if (party_cd.Length < 1 && part_cd.Length > 1)
                    {
                        cond = " and acode like '%' and icode in (" + part_cd + ") ";
                    }
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.acode,trim(f.aname) as party,a.icode,trim(b.iname) as iname,substr(trim(a.icode),1,2) as mcode,trim(c.name) as mname,substr(trim(a.icode),1,4) as scode,trim(d.iname) as sname,sum(a.ord_qty) as ord_qty,sum(a.so_qty) as so_qty,sum(a.inv_qty) as inv_qty from (Select type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,nvl(qtyord,0) as ord_qty,0 as so_qty,0 as inv_qty from somasq where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + "  " + cond + " union all select type,org_invno as ordno,to_char(org_invdt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,nvl(qtyord,0) as so_qty,0 as inv_qty from somas where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + "  union all select type,ponum as ordno,to_char(podate,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,0 as so_qty,nvl(iqtyout,0) as inv_qty from ivoucher where " + branch_Cd + " and type like '4%' and type not in ('45','47') and vchdate " + xprdRange + " " + cond + " ) a,item b,type c,item d,famst f where trim(a.icode)=trim(b.icode) and substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' and  substr(trim(a.icode),1,4)=trim(d.icode) and length(trim(d.icode))='4' and trim(a.acode)=trim(f.acode) group by a.acode,a.icode,trim(b.iname),substr(trim(a.icode),1,2),trim(c.name),substr(trim(a.icode),1,4),trim(d.iname),trim(f.aname)";
                    frm_rptName = "ITEM_WISE_STUD";
                }
                if (iconID == "F50388")
                {
                    header_n = "Sub Group Wise Sales Report";
                    if (part_cd.Length < 1 && party_cd.Length < 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,4) like '%' ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length > 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,4) in (" + part_cd + ") ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length < 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,4) like '%'";
                    }
                    else if (party_cd.Length < 1 && part_cd.Length > 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,4) in (" + part_cd + ") ";
                    }
                    SQuery = "select  '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.acode,trim(f.aname) as party,substr(trim(a.icode),1,2) as mcode,trim(c.name) as mname,substr(trim(a.icode),1,4) as scode,trim(d.iname) as sname,sum(a.ord_qty) as ord_qty,sum(a.so_qty) as so_qty,sum(a.inv_qty) as inv_qty from (Select type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,nvl(qtyord,0) as ord_qty,0 as so_qty,0 as inv_qty from somasq where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + " union all select type,org_invno as ordno,to_char(org_invdt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,nvl(qtyord,0) as so_qty,0 as inv_qty from somas where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + "  union all select type,ponum as ordno,to_char(podate,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,0 as so_qty,nvl(iqtyout,0) as inv_qty from ivoucher where " + branch_Cd + " and type like '4%' and type not in ('45','47') and vchdate " + xprdRange + " " + cond + " ) a,item d,famst f,TYPE C where  substr(trim(a.icode),1,4)=trim(d.icode) and length(trim(d.icode))='4' and trim(a.acode)=trim(f.acode) AND  substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' group by a.acode,substr(trim(a.icode),1,4),trim(d.iname),trim(f.aname),substr(trim(a.icode),1,2),trim(c.name) order by mcode";
                    frm_rptName = "SG_WISE_STUD";
                }
                if (iconID == "F50390")
                {
                    header_n = "Main Group Wise Sales Report";
                    if (part_cd.Length < 1 && party_cd.Length < 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,2) like '%' ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length > 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,2) in (" + part_cd + ") ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length < 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,2) like '%'";
                    }
                    else if (party_cd.Length < 1 && part_cd.Length > 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,2) in (" + part_cd + ") ";
                    }
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.acode,trim(f.aname) as party,substr(trim(a.icode),1,2) as mcode,trim(c.name) as mname,sum(a.ord_qty) as ord_qty,sum(a.so_qty) as so_qty,sum(a.inv_qty) as inv_qty from (Select type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,nvl(qtyord,0) as ord_qty,0 as so_qty,0 as inv_qty from somasq where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + " union all select type,org_invno as ordno,to_char(org_invdt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,nvl(qtyord,0) as so_qty,0 as inv_qty from somas where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + "  union all select type,ponum as ordno,to_char(podate,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,0 as so_qty,nvl(iqtyout,0) as inv_qty from ivoucher where " + branch_Cd + " and type like '4%' and type not in ('45','47') and vchdate " + xprdRange + " " + cond + " ) a,type c,famst f where  substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' and trim(a.acode)=trim(f.acode) group by a.acode,substr(trim(a.icode),1,2),trim(c.name),trim(f.aname)";
                    frm_rptName = "MG_WISE_STUD";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //made by akshay

            case "F50380":
                header_n = "Order Main Group Wise";
                dt = new DataTable();
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,B.ICODE as scode,B.INAME as sname,C.TYPE1 as mcode,C.NAME as mname,SUM(A.QTYORD) as qty FROM SOMASQ A ,ITEM B ,TYPE C  WHERE TRIM(SUBSTR(A.ICODE,1,4))=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1)  AND C.ID='Y' AND  A.BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY B.ICODE,B.INAME,C.TYPE1,C.NAME ORDER BY B.ICODE";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,C.TYPE1 as mcode,C.NAME as mname,SUM(A.QTYORD) as qty FROM SOMASQ A ,TYPE C  WHERE  TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1)  AND C.ID='Y' AND  A." + branch_Cd + " AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY C.TYPE1,C.NAME ORDER BY c.type1";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Ord_Main_Grp_Wise", "Ord_Main_Grp_Wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50382":
                header_n = "Order Sub Group Wise";
                dt = new DataTable();
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,B.ICODE as scode,B.INAME as sname,C.TYPE1,C.NAME,SUM(A.QTYORD) as qty FROM SOMASQ A ,ITEM B ,TYPE C  WHERE TRIM(SUBSTR(A.ICODE,1,4))=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1)  AND C.ID='Y' AND  A." + branch_Cd + " AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY B.ICODE,B.INAME,C.TYPE1,C.NAME ORDER BY B.ICODE";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Ord_Sub_Grp_Wise", "Ord_Sub_Grp_Wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50384":
                header_n = "Order Item Wise";
                dt = new DataTable();
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,B.ICODE AS SCODE,B.INAME AS SNAME,C.TYPE1 AS MCODE,C.NAME AS MNAME,A.ICODE,D.INAME ,SUM(A.QTYORD) as qty FROM SOMASQ A ,ITEM B ,TYPE C,ITEM D  WHERE TRIM(SUBSTR(A.ICODE,1,4))=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1) AND TRIM(A.ICODE)=TRIM(D.ICODE)  AND C.ID='Y' AND  A." + branch_Cd + " AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY B.ICODE,B.INAME,C.TYPE1,C.NAME,A.ICODE,D.INAME  ORDER BY B.ICODE";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Ord_Item_Wise", "Ord_Item_Wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50311":
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and A.acode like '" + party_cd + "%' and A.icode like '" + part_cd + "%' ";
                header_n = "Sale Register With Item Details";
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.vdd,trim(a.vchnum)||a.vchdate||trim(a.acode) as fstr,a.vchnum as invno,a.vchdate as invdt,trim(a.acode) as acode,trim(a.icode) as icode,trim(c.iname) as item_name,trim(b.aname) as party,sum(a.iqtyout) as qty ,sum(a.basis) as basis,sum(a.cgst) as cgst,sum(a.sgst) as sgst,sum(a.igst) as igst,a.disc,a.irate,a.bill_tot,a.mode_tpt from (select distinct  a.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as VCHDATE,to_char(a.vchdate,'yyyymmdd') as vdd,a.ACODE,a.icode,a.iqtyout,B.AMT_SALE as basis,a.irate, (case when b.st_type='CG' THEN b.aMT_exc else 0 end) AS CGST,(case when b.st_Type='IG' THEN b.aMT_exc else 0 end) AS IGST,rvalue as sgst,b.totdisc_Amt as disc,b.bill_tot,b.mode_tpt  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and  a.branchcd='" + frm_mbr + "' and a.type like '4%' " + cond + " and a.vchdate " + xprdRange + " ) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.vchnum,a.vchdate,trim(a.acode),trim(b.aname),a.disc,trim(a.icode),trim(c.iname),a.irate,a.bill_tot,a.mode_tpt,a.vdd  order by fstr,vdd asc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_11Col", "SaleReg_11Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50312":
                #region
                header_n = "Sale Register 10 Col";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.vchnum as invno,a.vchdate as invdt,trim(a.acode) as acode,trim(b.aname) as party,sum(a.basis) as basis,sum(a.cgst) as cgst,sum(a.sgst) as sgst,a.disc from (select distinct  a.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as VCHDATE,a.ACODE,a.IAMOUNT as basis, a.EXC_aMT AS CGSt,a.CESS_PU as sgst,b.totdisc_Amt as disc  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and  a.branchcd='" + frm_mbr + "' and a.type in (" + mq0 + ")  and a.vchdate " + xprdRange + " ) a,famst b where trim(a.acode)=trim(b.acode) group by a.vchnum,a.vchdate,trim(a.acode),trim(b.aname),a.disc order by invno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_10Col", "SaleReg_10Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50313":
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                cond = " and a.acode like '" + party_cd + "%'";
                header_n = "Sale Register Party Wise(Gross Total Amount)";
                SQuery = "select distinct '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.VCHNUM,TO_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyyMMdd') as vdd,trim(a.acode) as acode,trim(c.aname) as party,a.grno,to_char(a.grdate,'dd/mm/yyyy') as grdate,a.mode_tpt,a.bill_tot from SALE a,famst c where trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + xprdRange + " " + cond + "  order by vchnum,vdd asc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_2Col", "SaleReg_2Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50314":
                #region
                header_n = "Sale Register 5 Col";
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.vchnum as invno,a.vchdate as invdt,trim(a.acode) as acode,trim(b.aname) as party,sum(a.basis) as basis,sum(a.cgst) as cgst,sum(a.sgst) as sgst  from (select distinct  VCHNUM,to_char(vchdate,'dd/mm/yyyy') as VCHDATE,ACODE,IAMOUNT as basis, EXC_aMT AS CGSt,CESS_PU as sgst  from ivoucher where branchcd='" + frm_mbr + "' and type like '4%'  and vchdate " + xprdRange + " ) a,famst b where trim(a.acode)=trim(b.acode) group by a.vchnum,a.vchdate,trim(a.acode),trim(b.aname) order by invno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_5Col", "SaleReg_5Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50315":
                #region
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("todt", typeof(string));
                dt2.Columns.Add("icode", typeof(string));
                dt2.Columns.Add("Iname", typeof(string));
                dt2.Columns.Add("tot", typeof(double));
                dt2.Columns.Add("qty", typeof(double));
                dt2.Columns.Add("basic", typeof(double));
                //   dr1 = new DataRow();
                header_n = "Items Covering 80% Value " + fromdt + " To " + todt + "";
                mq0 = "";
                mq0 = "SELECT SUM(bill_tot) AS TOT FROM SALE  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdRange + "";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0); //tot sale from sale
                ////////////
                SQuery = "select distinct trim(a.icode) as icode,b.iname,sum(a.iqtyout) as qty,sum(a.exc_amt)+sum(a.cess_pu)+sum(a.iamount) as basic from ivoucher a,item b  where  trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '4%' AND a.VCHDATE " + xprdRange + " and a.icode like '9%' group by trim(a.icode),b.iname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    db = 0; db2 = 0;
                    db = fgen.make_double(dt1.Rows[0]["TOT"].ToString().Trim()) * 80 / 100;
                    db2 = Math.Round(db, 2);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        db1 = 0;
                        dr1 = dt2.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["tot"] = fgen.make_double(dt1.Rows[0]["TOT"].ToString().Trim());
                        dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr1["Iname"] = dt.Rows[i]["iname"].ToString().Trim().ToUpper();
                        dr1["qty"] = fgen.make_double(dt.Rows[i]["qty"].ToString().Trim());
                        dr1["basic"] = fgen.make_double(dt.Rows[i]["basic"].ToString().Trim());
                        db1 = fgen.make_double(dt.Rows[i]["basic"].ToString().Trim());
                        if (db1 <= db2) //if basic is less than or eql to tot then row will be add
                        {
                            dt2.Rows.Add(dr1);
                        }
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ItemCovering_80per", "ItemCovering_80per", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50316":
                #region
                header_n = "Country Wise Sales";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and A.acode like '" + party_cd + "%' and A.icode like '" + part_cd + "%' ";
                dt = new DataTable();
                SQuery = "SELECT '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + mq0 + "' as country, trim(a.vchnum)||' '||to_char(a.vchdate,'dd/mm/yyyy') as bill_details,a.vchnum,to_char(a.vchdate,'yyyyMMdd') as vdd,a.acode,b.aname,a.icode,c.iname,sum(a.iqtyout) as qty,a.irate,sum(a.iamount) as amt ,0 as disc FROM IVOUCHER a,famst b,item c WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' AND a.VCHDATE " + xprdRange + " " + cond + " and b.country='" + mq0 + "'  group by  trim(a.vchnum)||' '||to_char(a.vchdate,'dd/mm/yyyy'),a.vchnum,to_char(a.vchdate,'yyyyMMdd'),a.acode,b.aname,a.icode,c.iname,a.irate  order by aname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Country_wise_sale", "Country_wise_sale", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //made and merged by yogita on 02 feb 2019
            case "ITEM_GR_WT":
                #region
                dt = new DataTable();
                header_n = "Item Wise GrWt/Nwt";
                SQuery = "select distinct '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.ACODE,A.ICODE,SUM(A.IQTYOUT) AS QTY,SUM(A.IAMOUNT) AS AMT,b.iname,b.cpartno,c.aname as party  from ivoucher A,item b,famst c where  trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and A.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + xprdRange + " GROUP BY a.ACODE,a.ICODE,b.iname,c.aname,b.cpartno order by party,b.iname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "item_wise_GrWt_Nwt", "item_wise_GrWt_Nwt", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "MTH_WEEK": //monthly week wise analysis
                #region
                dt = new DataTable(); string myear = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq1) > 3)
                {
                    myear = frm_myear;
                }
                else
                {
                    int d = Convert.ToInt32(frm_myear) + 1;
                    myear = Convert.ToString(d);
                }
                mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ADD_MONTHS(TO_DATE('" + mq1 + "/" + myear + "','MM/yyyy') , 1 ) - TO_DATE('" + mq1 + "/" + myear + "','MM/yyyy') as DAYS  FROM DUAL", "DAYS");
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname from mths where mthnum='" + mq1 + "'", "mthname");

                header_n = "Weeky Sales Analysis for the Month " + mq3 + "/" + myear + "";

                SQuery = "select '" + header_n + "' as header,'" + mq1 + "/" + myear + "' as mthyear,'" + mq3 + "' as mthname, trim(a.acode) as acode,trim(b.aname) as party,trim(a.subitem) as itemcode,sum(a.week1+a.week2+a.week3+a.week4) as tot,sum(week1) as week1,sum(week2) as week2,sum(week3) as week3,sum(week4) as week4 from (select distinct  a.acode,substr(trim(a.icode),1,4) as subitem ,a.iqtyout as week1,0 as week2,0 as week3,0 as week4   from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('01/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('07/" + mq1 + "/" + myear + "','dd/mm/yyyy')  union all  select distinct  a.acode,substr(trim(a.icode),1,4) as subitem,0 as week1 ,a.iqtyout as week2 ,0 as week3 ,0 as week4  from ivoucher a where  a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('08/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('14/" + mq1 + "/" + myear + "','dd/mm/yyyy')  union all  select distinct  a.acode,substr(trim(a.icode),1,4) as subitem,0 as week1,0 as week2 ,a.iqtyout as week3,0 as week4 from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('15/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('21/" + mq1 + "/" + myear + "','dd/mm/yyyy') union all  select distinct  a.acode,substr(trim(a.icode),1,4) as subitem,0 as week1,0 as week2,0 as week3 ,a.iqtyout as week4   from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('22/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('" + mq2 + "/" + mq1 + "/" + myear + "','dd/mm/yyyy') ) a,famst b where trim(a.acode)=trim(b.acode)  group by trim(a.acode) ,trim(b.aname),trim(a.subitem) order by itemcode";

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mthly_week_analysis", "Mthly_week_analysis", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "SALE_REJ":///made on 1 feb19
                #region
                dt2 = new DataTable(); dt3 = new DataTable();
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("sale_mthname", typeof(string));
                dt2.Columns.Add("basic_mthname", typeof(string));
                dt2.Columns.Add("invno", typeof(string));
                dt2.Columns.Add("party", typeof(string));
                dt2.Columns.Add("acode", typeof(string));
                dt2.Columns.Add("join", typeof(string));
                dt2.Columns.Add("qty", typeof(double));
                dt2.Columns.Add("value", typeof(double));
                dt2.Columns.Add("totsale", typeof(double));
                dt2.Columns.Add("basicsale", typeof(double));

                dt = new DataTable(); dt1 = new DataTable(); mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");//SELECTED DATE
                mq2 = mq1.Substring(3, 7);
                mq4 = Convert.ToDateTime(fgen.seek_iname(frm_qstr, frm_cocd, "select to_date('" + mq2 + "','MM/yyyy')-1 as lastdt from dual", "lastdt")).ToString("dd/MM/yyyy");
                mq3 = mq4.Substring(3, 7);//last month as per select month
                mq5 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname as mth from mths where mthnum='" + mq1.Substring(3, 2) + "'", "mth");
                mq6 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname as mth from mths where mthnum='" + mq4.Substring(3, 2) + "'", "mth");

                header_n = "Sales & Rejection Summary (Basic Value) " + mq1 + " To " + mq1 + "";
                SQuery = "select a.vchnum,a.acode,b.aname,sum(a.amt_sale) as value from sale a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '4%' and to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "' group by a.vchnum,a.acode,b.aname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ///////               
                mq0 = "select sum(a.amt_sale) as totsale from sale a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '4%' and to_char(a.vchdate,'mm/yyyy')='" + mq3 + "' ";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                mq0 = "select sum(a.amt_sale) as basicsale from sale a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '4%' and to_char(a.vchdate,'mm/yyyy')='" + mq2 + "' ";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt2.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["sale_mthname"] = mq6;
                        dr1["basic_mthname"] = mq5;
                        dr1["invno"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr1["party"] = dt.Rows[i]["aname"].ToString().Trim().ToUpper();
                        dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                        dr1["join"] = dr1["invno"].ToString().Trim() + "   " + dr1["party"].ToString().Trim().ToUpper();
                        dr1["qty"] = 0;
                        dr1["value"] = fgen.make_double(dt.Rows[i]["value"].ToString().Trim());
                        dr1["totsale"] = fgen.make_double(dt1.Rows[0]["totsale"].ToString().Trim());
                        dr1["basicsale"] = fgen.make_double(dt3.Rows[0]["basicsale"].ToString().Trim());
                        dt2.Rows.Add(dr1);
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "sales_rejection_smry", "sales_rejection_smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "TARIF":///made on 1 feb19
                #region
                header_n = "Tarrif Wise Analysis";
                dt = new DataTable();
                //SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, tarrifno,type,name,cgst_rate_unit,igst_rate_unit,sum(qty) as qty,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic from  (select b.tarrifno,a.type,c.name,(case when a.iopr='CG' then c.num4 else 0 end) as cgst_rate_unit,(case when a.iopr='IG' then  c.num6 else 0 end) as igst_rate_unit,a.iqtyout as qty,a.exc_amt as igst,a.cess_pu as sgst,a.iamount as basic from ivoucher a,item b,TYPEGRP C where trim(a.icode)=trim(b.icode) AND trim(b.tarrifno)=trim(c.acref) and c.id='T1' and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.vchdate " + xprdRange + " ) group by tarrifno,type,name,cgst_rate_unit,igst_rate_unit order by type";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,tarrifno,tarrifno||cgst_rate_unit as grp,type,typname,name,cgst_rate_unit,sum(qty) as qty,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic from  (select b.tarrifno,a.type,d.name as typname,c.name,(case when a.iopr='CG' then c.num4 else c.num6 end) as cgst_rate_unit,a.iqtyout as qty,a.exc_amt as igst,a.cess_pu as sgst,a.iamount as basic from ivoucher a,item b,TYPEGRP C,type d where trim(a.icode)=trim(b.icode) AND trim(b.tarrifno)=trim(c.acref) and c.id='T1' and trim(a.type)=trim(d.type1) and d.id='V' and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.vchdate " + xprdRange + " ) group by tarrifno,type,name,cgst_rate_unit,typname order by type";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "tarrif_wise_analysis", "tarrif_wise_analysis", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "TARIF_BILL":///made on 1 feb19.done
                #region mrp vaue and adi amt in rpt file is pending to set in rpt
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                dt = new DataTable();
                header_n = "Tarrif wise Invoice wise Report";
                // SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, vchnum,vchdate,vdd,acode,aname,sum(cgst) as cgst,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic,exc_tarrif,hs_name,sum(total) as tot,sum(bill_qty) as qty  from (select a.vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd,a.acode,b.aname,(case when a.st_type='CG' then a.amt_exc else 0 end) as cgst,(case when a.st_Type='IG'  then a.amt_exc else 0 end) as igst,a.rvalue as sgst,a.amt_sale as basic,a.exc_tarrif,a.exc_item as hs_name,a.bill_tot as total,a.bill_qty from sale a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + frm_mbr + "' and a.type in (" + mq1 + ") and a.vchdate " + xprdRange + " ) group by vchnum,vchdate,vdd,acode,aname,exc_tarrif,hs_name  order by vchnum";// in this qry igst,cgst,sgst are diff
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, vchnum,vchdate,vdd,acode,aname,sum(cgst) as cgst,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic,exc_tarrif,hs_name,sum(total) as tot,sum(bill_qty) as qty  from (select a.vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd,a.acode,b.aname,a.amt_exc as cgst,0 as igst,a.rvalue as sgst,a.amt_sale as basic,a.exc_tarrif,a.exc_item as hs_name,a.bill_tot as total,a.bill_qty from sale a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + frm_mbr + "' and a.type in (" + mq1 + ") and a.vchdate " + xprdRange + " ) group by vchnum,vchdate,vdd,acode,aname,exc_tarrif,hs_name  order by vchnum,vdd"; //in this cgst/igst are same
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "tarrif_bill_wise", "tarrif_bill_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "A3":
                header_n = "SOP";
                SQuery = "SELECT A.*,I.INAME,B.INAME AS BINAME FROM INSPMST A,ITEM I,ITEM B WHERE TRIM(A.COL1)=TRIM(I.ICODE) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.COL1)||TRIM(A.ICODE)='" + barCode + "' AND LENGTH(TRIM(I.ICODE))=4 ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "crpt_NeopSOP", "crpt_NeopSOP", dsRep, "");
                }
                break;

            case "A2":
                header_n = "Sales Projection Sheet";
                SQuery = "Select a.*,b.iname,b.cpartno,c.aname from mthlyplan a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.CUST)=trim(c.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "soproj", "soproj", dsRep, "");
                }
                break;

            case "F50321":
                #region
                ph_tbl = new DataTable();
                #region
                ph_tbl.Columns.Add("HEADER", typeof(string));
                ph_tbl.Columns.Add("FSTR", typeof(string));
                ph_tbl.Columns.Add("FROMDT", typeof(string));
                ph_tbl.Columns.Add("TODT", typeof(string));
                ph_tbl.Columns.Add("Our_Order_No", typeof(string));
                ph_tbl.Columns.Add("Date", typeof(string));
                ph_tbl.Columns.Add("ACODE", typeof(string));
                ph_tbl.Columns.Add("Party", typeof(string));
                ph_tbl.Columns.Add("Item", typeof(string));
                ph_tbl.Columns.Add("ErpCode", typeof(string));
                ph_tbl.Columns.Add("Customer_Order_No", typeof(string));
                ph_tbl.Columns.Add("Customer_Order_Date", typeof(string));
                ph_tbl.Columns.Add("Order_Line_No", typeof(string));
                ph_tbl.Columns.Add("Order_Qty", typeof(double));
                ph_tbl.Columns.Add("Tolerance_Qty", typeof(double));
                ph_tbl.Columns.Add("Sale_Qty", typeof(double));
                ph_tbl.Columns.Add("Invoice_No", typeof(string));
                ph_tbl.Columns.Add("Invoice_Date", typeof(string));
                ph_tbl.Columns.Add("Balance_Order_Qty", typeof(double));
                ph_tbl.Columns.Add("Rate", typeof(double));
                ph_tbl.Columns.Add("Bsr_Stock", typeof(double));
                ph_tbl.Columns.Add("Bal_Order_Req_To_Desp_Bsr_Qty", typeof(double));
                #endregion
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt5 = new DataTable(); dt6 = new DataTable();
                mq0 = ""; mq1 = ""; mq2 = "";
                header_n = "Pending Order Register";
                xprdRange1 = "between to_Date('01/04/" + frm_myear + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1"; //for one fetching day closing
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                string cond1 = "", cond2 = "";
                if (party_cd.Length > 2)
                {
                    cond = "and trim(a.icode) in (" + party_cd + ")";
                    cond2 = "and trim(icode) in (" + party_cd + ")";
                }
                else
                {
                    cond = "and trim(a.icode) like '%'";
                    cond2 = "and trim(icode) like '%'";
                }

                if (part_cd.Length > 2)
                {
                    cond1 = "and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + part_cd + ")";
                }
                else
                { }
                mq3 = "";
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4");
                mq0 = "select trim(a.branchcd)||trim(a.type)||trim(a.acode)||trim(a.icode)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.cdrgno) as fstr,a.branchcd,a.type, a.ordno as ordno,to_char(a.orddt,'dd/mm/yyyy') as ord_date,to_char(a.orddt,'yyyymmdd') as vdd,a.cdrgno,trim(a.acode) as acode,trim(a.icode) as icode,b.aname as party,trim(c.iname) as item,a.weight as ord_line_no,a.pordno as cust_ordno,to_Char(a.porddt,'dd/mm/yyyy') as cust_ordt,sum(a.qtyord) as order_Qty,sum(a.qtysupp) as Tolerance_Qty, nvl(a.irate,0) as irate,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as order_nal from somas a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and substr(trim(a.type),1,1)='4' and a.type!='47' " + cond + " and a.acode in (" + mq3 + ") and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + part_cd + ") group by a.ordno,to_char(a.orddt,'dd/mm/yyyy'),trim(a.acode),trim(a.icode),b.aname,trim(c.iname) ,a.pordno,to_Char(a.porddt,'dd/mm/yyyy'),a.weight,a.irate,to_char(a.orddt,'yyyymmdd'),a.branchcd,a.type,a.cdrgno order by vdd,ordno,icode asc";
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);//main dt 

                mq0 = "";
                mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select  to_char(to_date('" + frm_cDt1 + "','dd/mm/yyyy')+600,'dd/MM/yyyy') as dd from dual", "dd");//add 600 days in date for invoice ...inv next year b ban skta hai

                xprdRange = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_date('" + mq0 + "','dd/MM/yyyy')";
                mq1 = "select a.branchcd,a.type,A.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdt ,trim(a.acode) as acode,trim(a.icode) as icode,a.prnum,sum(a.iqtyout) as sale_qty,a.binno as lineno,a.irate,sum(a.iamount) as sale_val,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate,a.prnum,b.mo_vehi  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + xprdRange + " " + cond + " and a.acode in (" + mq3 + ") group by a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.icode) ,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy'),a.prnum,b.mo_vehi,a.irate,a.branchcd,a.type,a.binno,a.prnum order by invno,invdt asc";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                mq2 = "select trim(a.icode) as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + frm_mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + frm_mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + frm_mbr + "' AND vchdate " + xprdRange + " and store='Y' " + cond2 + " and substr(trim(icode),1,1)='9' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);//stock dt
                header_n = "Pending SO BSR Qty";
                if (dt.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "branchcd", "type", "acode", "icode", "ordno", "ord_date", "cdrgno"); //MAIN                  
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        dt3 = new DataTable(); dt4 = new DataTable();
                        DataView viewim = new DataView(dt, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' and type='" + dr0["type"].ToString().Trim() + "' and acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "' and ordno='" + dr0["ordno"].ToString().Trim() + "' and ord_date='" + dr0["ord_date"].ToString().Trim() + "' and cdrgno='" + dr0["cdrgno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt3 = viewim.ToTable();//somas view
                        dr1 = ph_tbl.NewRow();
                        ////invoice view
                        if (dt1.Rows.Count > 0)
                        {
                            DataView viewim1 = new DataView(dt1, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' and type='" + dr0["type"].ToString().Trim() + "' and acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "' and ponum='" + dr0["ordno"].ToString().Trim() + "' and podate='" + dr0["ord_date"].ToString().Trim() + "' and prnum='" + dr0["cdrgno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt4 = viewim1.ToTable();
                        }
                        db6 = 0;//for bal order qty
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            #region order details
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0;
                            dr1 = ph_tbl.NewRow();
                            dr1["FSTR"] = dt3.Rows[i]["fstr"].ToString().Trim();
                            dr1["header"] = header_n;
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;
                            dr1["Our_Order_No"] = dt3.Rows[i]["ordno"].ToString().Trim();
                            dr1["Date"] = dt3.Rows[i]["ord_date"].ToString().Trim();
                            dr1["Acode"] = dt3.Rows[i]["acode"].ToString().Trim();
                            dr1["Party"] = dt3.Rows[i]["party"].ToString().Trim();
                            dr1["Item"] = dt3.Rows[i]["item"].ToString().Trim();
                            dr1["ErpCode"] = dt3.Rows[i]["icode"].ToString().Trim();
                            dr1["Customer_Order_No"] = dt3.Rows[i]["cust_ordno"].ToString().Trim();
                            dr1["Customer_Order_Date"] = dt3.Rows[i]["cust_ordt"].ToString().Trim();
                            dr1["Order_Line_No"] = dt3.Rows[i]["ord_line_no"].ToString().Trim();
                            dr1["Order_Qty"] = dt3.Rows[i]["order_Qty"].ToString().Trim();
                            db1 = fgen.make_double(dr1["Order_Qty"].ToString().Trim());
                            //=================                                  
                            for (int j = 0; j < dt4.Rows.Count; j++)
                            {
                                #region filling invoice details on basis of order
                                if (j != 0)
                                {
                                    dr1 = ph_tbl.NewRow();   /// for invoice
                                    dr1["FSTR"] = dt3.Rows[i]["fstr"].ToString().Trim();
                                    dr1["header"] = header_n;
                                    dr1["fromdt"] = fromdt;
                                    dr1["todt"] = todt;
                                    dr1["Our_Order_No"] = dt3.Rows[i]["ordno"].ToString().Trim();
                                    dr1["Date"] = dt3.Rows[i]["ord_date"].ToString().Trim();
                                    dr1["Acode"] = dt3.Rows[i]["acode"].ToString().Trim();
                                    dr1["Party"] = dt3.Rows[i]["party"].ToString().Trim();
                                    dr1["Item"] = dt3.Rows[i]["item"].ToString().Trim();
                                    dr1["ErpCode"] = dt3.Rows[i]["icode"].ToString().Trim();
                                    dr1["Customer_Order_No"] = dt3.Rows[i]["cust_ordno"].ToString().Trim();
                                    dr1["Customer_Order_Date"] = dt3.Rows[i]["cust_ordt"].ToString().Trim();
                                    dr1["Order_Line_No"] = dt3.Rows[i]["ord_line_no"].ToString().Trim();
                                    dr1["Order_Qty"] = dt3.Rows[i]["order_Qty"].ToString().Trim();
                                    db1 = fgen.make_double(dr1["Order_Qty"].ToString().Trim());
                                }
                                dr1["Sale_Qty"] = dt4.Rows[j]["sale_qty"].ToString().Trim();
                                db2 = fgen.make_double(dr1["Sale_Qty"].ToString().Trim());
                                db3 = db1 - db2;//bal order qty
                                if (db3 > 0)
                                {
                                    db4 = fgen.make_double(dt3.Rows[i]["Tolerance_Qty"].ToString().Trim().Split('.')[0].ToString());
                                    dr1["Tolerance_Qty"] = db4;
                                }
                                else
                                {
                                    dr1["Tolerance_Qty"] = 0;
                                }
                                dr1["Invoice_No"] = dt4.Rows[j]["invno"].ToString().Trim();
                                dr1["Invoice_Date"] = dt4.Rows[j]["invdt"].ToString().Trim();
                                if (j == 0)
                                {
                                    dr1["Balance_Order_Qty"] = db3;
                                    db6 = db3;
                                }
                                else
                                {
                                    dr1["Balance_Order_Qty"] = db6 - db2;
                                    db6 = fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                }
                                dr1["Rate"] = fgen.make_double(dt4.Rows[j]["irate"].ToString().Trim());
                                dr1["Bsr_Stock"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt4.Rows[j]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                db8 = fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                db5 = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim()) - fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                if (db8 >= 0)
                                {
                                    if (db5 > 0)
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = db5;
                                    }
                                    else
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim());
                                    }
                                }
                                #endregion
                                ph_tbl.Rows.Add(dr1);
                            }
                            if (dt4.Rows.Count == 0)
                            {
                                db2 = fgen.make_double(dr1["Sale_Qty"].ToString().Trim());
                                db3 = db1 - db2;//bal order qty
                                if (db3 > 0)
                                {
                                    db4 = fgen.make_double(dt3.Rows[i]["Tolerance_Qty"].ToString().Trim().Split('.')[0].ToString());
                                    dr1["Tolerance_Qty"] = db4;
                                }
                                else
                                {
                                    dr1["Tolerance_Qty"] = 0;
                                }
                                dr1["Bsr_Stock"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt3.Rows[i]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                dr1["Balance_Order_Qty"] = db3;
                                db8 = fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                db5 = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim()) - fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                if (db8 >= 0)
                                {
                                    if (db5 > 0)
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = db5;
                                    }
                                    else
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim());
                                    }
                                }
                                ph_tbl.Rows.Add(dr1);
                            }
                            #endregion
                        }
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Pending_SO_BSR_QTY", "Pending_SO_BSR_QTY", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50275":
                header_n = "Main Group,Sub Group,Party Wise Sale Qty Report";
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq9 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (mq10.Length <= 1)
                {
                    mq2 = "and trim(a.acode) like '%'";
                }
                else
                {
                    mq2 = "and trim(a.acode) in (" + mq10 + ")";
                }
                //-------------------------
                if (mq9.Length <= 1)
                {
                    mq3 = "and trim(c.staten) like '%'";
                }
                else
                {
                    mq3 = "and trim(c.staten) in (" + mq9 + ")";
                }
                //----------------------------------
                if (party_cd.Length <= 1)
                {
                    mq1 = "and trim(d.type1) like '%'";
                }
                else
                {
                    mq1 = "and trim(d.type1) = '" + party_cd + "'";
                }

                if (part_cd.Length <= 1)
                {
                    mq0 = "and substr(trim(a.icode),1,4) like '%'";
                }
                else
                {
                    mq0 = "and substr(trim(a.icode),1,4) in (" + part_cd + ")";
                }

                dt = new DataTable();
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header, trim(a.acode) as acode,c.aname as party,c.staten as state,substr(trim(a.icode),1,4) as sub_grp,trim(b.iname) as sub_nm,substr(trim(a.icode),1,2) as main_grp,trim(d.name) as mgname,sum(a.iqtyout) as iqtyout,sum(a.iamount) as iamount ,sum(a.return) as ret_qty,sum(a.ret_value) as ret_value ,sum(a.avg_qty) as avg_qty,sum(a.avg_amt) as avg_amt from (select a.acode,a.icode , a.iqtyout,a.iamount ,0 as return,0 as ret_value,0 as avg_qty,0 as avg_amt from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and a.vchdate " + xprdRange + " and nvl(a.iqtyout,0)>0 union all select a.acode,a.icode ,0 as  iqtyout,0 as iamount ,a.iqtyin as return,a.iamount as ret_value,0 as avg_qty,0 as avg_amt from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type ='04' and a.vchdate " + xprdRange + ") a , item b ,famst c,type d where trim(a.acode)=trim(c.acode) and substr(trim(a.icode),1,4)=trim(b.icode) and substr(trim(a.icode),1,2)=trim(d.type1) and d.id='Y' and length(trim(b.icode))=4  " + mq0 + " " + mq1 + " " + mq2 + " " + mq3 + " group by trim(a.acode),c.aname ,substr(trim(a.icode),1,4) ,trim(b.iname),substr(trim(a.icode),1,2) ,trim(d.name),c.staten order by main_grp,sub_grp,acode ";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                TimeSpan t = Convert.ToDateTime(todt) - Convert.ToDateTime(fromdt).AddDays(1);

                foreach (DataRow dr in dt.Rows)
                {
                    dr["avg_qty"] = (fgen.make_double(dr["iqtyout"].ToString().Trim()) - fgen.make_double(dr["ret_qty"].ToString().Trim())) / t.TotalDays;
                    dr["avg_amt"] = (fgen.make_double(dr["iamount"].ToString().Trim()) - fgen.make_double(dr["ret_value"].ToString().Trim())) / t.TotalDays;
                }
                if (dt.Rows.Count > 0)
                {

                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Sale_Qty_Himt", "Sale_Qty_Himt", dsRep, header_n);
                }
                break;

            case "F50278":
                #region
                header_n = "Monthly State Wise,Group Wise,Sales Trend";
                DataTable dt10 = new DataTable();
                dt10.Columns.Add("Header", typeof(string));
                dt10.Columns.Add("fromdt", typeof(string));
                dt10.Columns.Add("todt", typeof(string));
                dt10.Columns.Add("State", typeof(string));
                dt10.Columns.Add("MCODE", typeof(string));
                dt10.Columns.Add("MNAME", typeof(string));
                dt10.Columns.Add("SUBCODE", typeof(string));
                dt10.Columns.Add("SUBNAME", typeof(string));
                dt10.Columns.Add("icode", typeof(string));
                dt10.Columns.Add("iname", typeof(string));
                dt10.Columns.Add("apr_qty", typeof(double));
                dt10.Columns.Add("apr_val", typeof(double));
                dt10.Columns.Add("may_qty", typeof(double));
                dt10.Columns.Add("may_val", typeof(double));
                dt10.Columns.Add("june_qty", typeof(double));
                dt10.Columns.Add("june_val", typeof(double));
                dt10.Columns.Add("july_qty", typeof(double));
                dt10.Columns.Add("july_val", typeof(double));
                dt10.Columns.Add("aug_qty", typeof(double));
                dt10.Columns.Add("aug_val", typeof(double));
                dt10.Columns.Add("sep_qty", typeof(double));
                dt10.Columns.Add("sep_val", typeof(double));
                dt10.Columns.Add("oct_qty", typeof(double));
                dt10.Columns.Add("oct_val", typeof(double));
                dt10.Columns.Add("nov_qty", typeof(double));
                dt10.Columns.Add("nov_val", typeof(double));
                dt10.Columns.Add("dec_qty", typeof(double));
                dt10.Columns.Add("dec_val", typeof(double));
                dt10.Columns.Add("jan_qty", typeof(double));
                dt10.Columns.Add("jan_val", typeof(double));
                dt10.Columns.Add("feb_qty", typeof(double));
                dt10.Columns.Add("feb_val", typeof(double));
                dt10.Columns.Add("mar_qty", typeof(double));
                dt10.Columns.Add("mar_val", typeof(double));
                dt10.Columns.Add("total_qty", typeof(double));
                dt10.Columns.Add("total_val", typeof(double));
                mq3 = ""; mq4 = "";
                mq4 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                if (mq4.Length > 1)
                {
                    mq3 = "and f.staten in (" + mq4 + ")";
                }
                else
                {
                    mq3 = "and f.staten like '%'";
                }
                if (party_cd.Length <= 1)
                {
                    party_cd = "and A.MGCODE like '%'";
                }
                else
                {
                    party_cd = "and A.MGCODE='" + party_cd + "'";
                }
                if (part_cd.Length <= 1)
                {
                    part_cd = "and A.SUBCODE like '%'";
                }
                else
                {
                    part_cd = " and A.SUBCODE in (" + part_cd + ")";
                }
                //SQuery = "select  a.MGCODE,t.name as mgname,a.subcode,d.iname as subname,trim(f.staten) as state,sum(a.apr_qty) as apr_qty,sum(a.apr_val) as apr_val,sum(a.may_qty) as may_qty,sum(a.may_val) as may_val,sum(a.june_qty) as june_qty,sum(a.june_val) as june_val,sum(a.july_qty) as july_qty,sum(a.july_val) as july_val,sum(a.aug_qty) as aug_qty,sum(a.aug_val) as aug_val,sum(a.sep_qty) as sep_qty,sum(a.sep_val) as sep_val,sum(a.oct_qty) as oct_qty,sum(a.oct_val) as oct_val,sum(a.nov_qty) as nov_qty,sum(a.nov_val) as nov_val,sum(a.dec_qty) as dec_qty,sum(a.dec_val) as dec_val,sum(a.jan_qty) as jan_qty,sum(a.jan_val) as jan_val,sum(a.feb_qty) as feb_qty,sum(a.feb_val) as feb_val,sum(a.mar_qty) as mar_qty,sum(a.mar_val) as mar_val,sum(a.apr_qty)+sum(a.may_qty)+sum(a.june_qty)+sum(a.july_qty)+sum(a.aug_qty)+sum(a.sep_qty)+sum(a.oct_qty)+sum(a.nov_qty)+sum(a.dec_qty)+sum(a.jan_qty)+sum(a.feb_qty)+sum(a.mar_qty) as total_qty,sum(a.apr_val)+sum(a.may_val)+sum(a.june_val)+sum(a.july_val)+sum(a.aug_val)+sum(a.sep_val)+sum(a.oct_val)+sum(a.nov_val)+sum(a.dec_val)+sum(a.jan_val)+sum(a.feb_val)+sum(a.mar_val) as total_val from (select substr(trim(icode),1,2) AS MGCODE,substr(trim(icode),1,4) as SUBCODE,trim(acode) as acode, (case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as apr_qty,(case when to_char(vchdate,'mm')='04' then iamount else 0 end) as apr_val,(case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as may_qty,(case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may_val,(case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as june_qty,(case when to_char(vchdate,'mm')='06' then iamount else 0 end) as june_val,(case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as july_qty,(case when to_char(vchdate,'mm')='07' then iamount else 0 end) as july_val,(case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as aug_qty,(case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug_val,(case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as sep_qty,(case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep_val,(case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as oct_qty,(case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct_val,(case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as nov_qty,(case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov_val,(case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as dec_qty,(case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec_val,(case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as jan_qty,(case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan_val,(case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as feb_qty,(case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb_val,(case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as mar_qty,(case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar_val from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a, famst f,type t,ITEM D where trim(a.acode)=trim(f.acode) " + mq3 + " " + part_cd + " " + party_cd + " and trim(a.mgcode)=trim(t.type1) and t.id='Y' and trim(a.subcode)=trim(d.icode) and length(trim(d.icode))=4 group by trim(f.staten),a.MGCODE,a.subcode,t.name,d.iname order by state,a.MGCODE,a.subcode";
                SQuery = "select  a.MGCODE,t.name as mgname,a.subcode,d.iname as subname,a.icode,e.iname,trim(f.staten) as state,sum(a.apr_qty) as apr_qty,sum(a.apr_val) as apr_val,sum(a.may_qty) as may_qty,sum(a.may_val) as may_val,sum(a.june_qty) as june_qty,sum(a.june_val) as june_val,sum(a.july_qty) as july_qty,sum(a.july_val) as july_val,sum(a.aug_qty) as aug_qty,sum(a.aug_val) as aug_val,sum(a.sep_qty) as sep_qty,sum(a.sep_val) as sep_val,sum(a.oct_qty) as oct_qty,sum(a.oct_val) as oct_val,sum(a.nov_qty) as nov_qty,sum(a.nov_val) as nov_val,sum(a.dec_qty) as dec_qty,sum(a.dec_val) as dec_val,sum(a.jan_qty) as jan_qty,sum(a.jan_val) as jan_val,sum(a.feb_qty) as feb_qty,sum(a.feb_val) as feb_val,sum(a.mar_qty) as mar_qty,sum(a.mar_val) as mar_val,sum(a.apr_qty)+sum(a.may_qty)+sum(a.june_qty)+sum(a.july_qty)+sum(a.aug_qty)+sum(a.sep_qty)+sum(a.oct_qty)+sum(a.nov_qty)+sum(a.dec_qty)+sum(a.jan_qty)+sum(a.feb_qty)+sum(a.mar_qty) as total_qty,sum(a.apr_val)+sum(a.may_val)+sum(a.june_val)+sum(a.july_val)+sum(a.aug_val)+sum(a.sep_val)+sum(a.oct_val)+sum(a.nov_val)+sum(a.dec_val)+sum(a.jan_val)+sum(a.feb_val)+sum(a.mar_val) as total_val from (select substr(trim(icode),1,2) AS MGCODE,substr(trim(icode),1,4) as SUBCODE,trim(icode) as icode,trim(acode) as acode, (case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as apr_qty,(case when to_char(vchdate,'mm')='04' then iamount else 0 end) as apr_val,(case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as may_qty,(case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may_val,(case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as june_qty,(case when to_char(vchdate,'mm')='06' then iamount else 0 end) as june_val,(case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as july_qty,(case when to_char(vchdate,'mm')='07' then iamount else 0 end) as july_val,(case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as aug_qty,(case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug_val,(case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as sep_qty,(case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep_val,(case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as oct_qty,(case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct_val,(case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as nov_qty,(case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov_val,(case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as dec_qty,(case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec_val,(case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as jan_qty,(case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan_val,(case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as feb_qty,(case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb_val,(case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as mar_qty,(case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar_val from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a, famst f,type t,ITEM D,item e where trim(a.acode)=trim(f.acode) " + mq3 + " " + part_cd + " " + party_cd + " and trim(a.mgcode)=trim(t.type1) and t.id='Y' and trim(a.subcode)=trim(d.icode) and length(trim(d.icode))=4  and trim(a.icode)=trim(e.icode) group by trim(f.staten),a.MGCODE,a.subcode,t.name,d.iname,a.icode,e.iname order by state,a.MGCODE,a.subcode";
                dt5 = new DataTable();

                dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        #region
                        dr1 = dt10.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["State"] = dt5.Rows[i]["state"].ToString().Trim();
                        dr1["MCODE"] = dt5.Rows[i]["mgcode"].ToString().Trim();
                        dr1["MNAME"] = dt5.Rows[i]["mgname"].ToString().Trim();
                        dr1["SUBCODE"] = dt5.Rows[i]["subcode"].ToString().Trim();
                        dr1["SUBNAME"] = dt5.Rows[i]["subname"].ToString().Trim();
                        dr1["icode"] = dt5.Rows[i]["icode"].ToString().Trim();
                        dr1["iname"] = dt5.Rows[i]["iname"].ToString().Trim();
                        dr1["apr_qty"] = fgen.make_double(dt5.Rows[i]["apr_qty"].ToString().Trim());
                        dr1["may_qty"] = fgen.make_double(dt5.Rows[i]["may_qty"].ToString().Trim());
                        dr1["june_qty"] = fgen.make_double(dt5.Rows[i]["june_qty"].ToString().Trim());
                        dr1["july_qty"] = fgen.make_double(dt5.Rows[i]["july_qty"].ToString().Trim());
                        dr1["aug_qty"] = fgen.make_double(dt5.Rows[i]["aug_qty"].ToString().Trim());
                        dr1["sep_qty"] = fgen.make_double(dt5.Rows[i]["sep_qty"].ToString().Trim());
                        dr1["oct_qty"] = fgen.make_double(dt5.Rows[i]["oct_qty"].ToString().Trim());
                        dr1["nov_qty"] = fgen.make_double(dt5.Rows[i]["nov_qty"].ToString().Trim());
                        dr1["dec_qty"] = fgen.make_double(dt5.Rows[i]["dec_qty"].ToString().Trim());
                        dr1["jan_qty"] = fgen.make_double(dt5.Rows[i]["jan_qty"].ToString().Trim());
                        dr1["feb_qty"] = fgen.make_double(dt5.Rows[i]["feb_qty"].ToString().Trim());
                        dr1["mar_qty"] = fgen.make_double(dt5.Rows[i]["mar_qty"].ToString().Trim());
                        dr1["apr_val"] = fgen.make_double(dt5.Rows[i]["apr_val"].ToString().Trim());
                        dr1["may_val"] = fgen.make_double(dt5.Rows[i]["may_val"].ToString().Trim());
                        dr1["june_val"] = fgen.make_double(dt5.Rows[i]["june_val"].ToString().Trim());
                        dr1["july_val"] = fgen.make_double(dt5.Rows[i]["july_val"].ToString().Trim());
                        dr1["aug_val"] = fgen.make_double(dt5.Rows[i]["aug_val"].ToString().Trim());
                        dr1["sep_val"] = fgen.make_double(dt5.Rows[i]["sep_val"].ToString().Trim());
                        dr1["oct_val"] = fgen.make_double(dt5.Rows[i]["oct_val"].ToString().Trim());
                        dr1["nov_val"] = fgen.make_double(dt5.Rows[i]["nov_val"].ToString().Trim());
                        dr1["dec_val"] = fgen.make_double(dt5.Rows[i]["dec_val"].ToString().Trim());
                        dr1["jan_val"] = fgen.make_double(dt5.Rows[i]["jan_val"].ToString().Trim());
                        dr1["feb_val"] = fgen.make_double(dt5.Rows[i]["feb_val"].ToString().Trim());
                        dr1["mar_val"] = fgen.make_double(dt5.Rows[i]["mar_val"].ToString().Trim());
                        dr1["total_qty"] = fgen.make_double(dt5.Rows[i]["total_qty"].ToString().Trim());
                        dr1["total_Val"] = fgen.make_double(dt5.Rows[i]["total_val"].ToString().Trim());
                        dt10.Rows.Add(dr1);
                        #endregion
                    }
                }
                if (dt10.Rows.Count > 0)
                {
                    dt10.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt10, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mthly_Grp_State_Wise_Sales_trnd", "Mthly_Grp_State_Wise_Sales_trnd", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50279":
                #region
                dtm = new DataTable();//add dummy column in this
                dtm.Columns.Add("col_1", typeof(string));
                dtm.Columns.Add("col_2", typeof(string));
                dtm.Columns.Add("col_3", typeof(string));
                dtm.Columns.Add("col_4", typeof(string));
                dtm.Columns.Add("col_5", typeof(string));
                dtm.Columns.Add("col_6", typeof(string));
                dtm.Columns.Add("col_7", typeof(string));
                dtm.Columns.Add("col_8", typeof(string));
                dt4 = dtm.Clone();
                dt5 = dtm.Clone();
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond1 = ""; cond2 = ""; string cond3 = "";
                if (party_cd.Length < 2)
                {
                    cond = "and substr(trim(icode),1,2) like '%'";
                    cond2 = "and substr(trim(a.icode),1,2) like '%'";
                }
                else
                {
                    cond = "and substr(trim(icode),1,2) in (" + party_cd + ") ";
                    cond2 = "and substr(trim(a.icode),1,2) in (" + party_cd + ") ";
                }
                if (part_cd.Length < 2)
                {
                    cond1 = "and substr(trim(icode),1,4) like '%'";
                    cond3 = "and substr(trim(a.icode),1,4) like '%'";
                }
                else
                {
                    cond1 = "and substr(trim(icode),1,4) in (" + part_cd + ")";
                    cond3 = "and substr(trim(a.icode),1,4) in (" + part_cd + ")";
                }
                dsRep = new DataSet();
                header_n = "Sales Trend Statement";
                int cnt = 0, cnt1 = 0;
                string footer_n = "";
                //SQuery = "select a.acode,b.aname,a.subcode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(acode) as acode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where type like '4%' and type!='47' and vchdate " + xprdRange + " )  a,famst b,item c  where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))>4  group by a.acode,b.aname,a.subcode order by acode";
                //SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + footer_n + "' as footer, a.acode,b.aname,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,famst b,item c ,type d where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.acode,b.aname,a.subcode,c.iname,a.mcode,d.name order by type";
                //SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + footer_n + "' as footer, a.acode,b.aname,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar)/100000,2) as total,round(sum(a.apr)/100000,2) as apr,round(sum(a.may)/100000,2) as may,round(sum(a.jun)/100000,2) as jun,round(sum(a.jul)/100000,2) as jul,round(sum(a.aug)/100000,2) as aug,round(sum(a.sep)/100000,2) as sep,round(sum(a.oct)/100000,2) as oct,round(sum(a.nov)/100000,2) as nov,round(sum(a.dec)/100000,2) as dec,round(sum(a.jan)/100000,2) as jan,round(sum(a.feb)/100000,2) as feb,round(sum(a.mar)/100000,2) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,famst b,item c ,type d where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.acode,b.aname,a.subcode,c.iname,a.mcode,d.name order by type";
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + footer_n + "' as footer,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar)/100000,2) as total,round(sum(a.apr)/100000,2) as apr,round(sum(a.may)/100000,2) as may,round(sum(a.jun)/100000,2) as jun,round(sum(a.jul)/100000,2) as jul,round(sum(a.aug)/100000,2) as aug,round(sum(a.sep)/100000,2) as sep,round(sum(a.oct)/100000,2) as oct,round(sum(a.nov)/100000,2) as nov,round(sum(a.dec)/100000,2) as dec,round(sum(a.jan)/100000,2) as jan,round(sum(a.feb)/100000,2) as feb,round(sum(a.mar)/100000,2) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,item c ,type d where trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.subcode,c.iname,a.mcode,d.name order by total asc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    #region
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    SQuery = "select count (mth) as mth from (select to_char(vchdate,'mm') as mth ,sum(bill_tot/100000) as salee from sale where  branchcd='" + frm_mbr + "' and  type like '4%'  and vchdate " + xprdRange + " group by to_char(vchdate,'mm')) where salee>0";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "Mth";
                        dsRep.Tables.Add(dt);
                    }
                    //////////
                    SQuery = "select sum(total) as basic_tot from (select a.acode,b.aname,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar)/100000,2) as total,round(sum(a.apr)/100000,2) as apr,round(sum(a.may)/100000,2) as may,round(sum(a.jun)/100000,2) as jun,round(sum(a.jul)/100000,2) as jul,round(sum(a.aug)/100000,2) as aug,round(sum(a.sep)/100000,2) as sep,round(sum(a.oct)/100000,2) as oct,round(sum(a.nov)/100000,2) as nov,round(sum(a.dec)/100000,2) as dec,round(sum(a.jan)/100000,2) as jan,round(sum(a.feb)/100000,2) as feb,round(sum(a.mar)/100000,2) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,famst b,item c ,type d where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.acode,b.aname,a.subcode,c.iname,a.mcode,d.name order by type)";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "basic_tot";
                        dsRep.Tables.Add(dt);
                    }
                    ////////////////////////////====================
                    header_n = "Our Top 10 Customer(In Order of Turnover)";
                    //SQuery = "select * from (select '" + header_n + "' as headerr, a.acode,b.aname,ROUND(sum(a.iqtyout*a.irate/100000),2) as salee_ FROM IVOUCHER a,famst b where   trim(a.acode)=trim(b.acode)  and SUBSTR(TRIM(A.ACODE),1,2) IN ('02','16') and  a.branchcd='" + frm_mbr + "' and a.type like '4%'  and  a.vchdate " + xprdRange + "  group by a.acode,b.aname order by  salee_ desc) where rownum<='10'"; //all customer without using any selection                   
                    //SQuery = "select * from (select '" + header_n + "' as headerr, a.acode,b.aname,ROUND(sum(a.bill_tot/100000),2) as salee_ FROM sale a,famst b where   trim(a.acode)=trim(b.acode)  and SUBSTR(TRIM(A.ACODE),1,2) IN ('02','16') and  a.branchcd='" + frm_mbr + "' and a.type like '4%'  and  a.vchdate " + xprdRange + " " + cond + " " + cond1 + "  group by a.acode,b.aname order by  salee_ desc) where rownum<='10'"; //customer as per selection group or subgroup
                    SQuery = "select * from (select '" + header_n + "' as headerr, a.acode,b.aname,ROUND(sum(a.iqtyout*a.irate/100000),2) as salee_ FROM IVOUCHER a,famst b where   trim(a.acode)=trim(b.acode)  and SUBSTR(TRIM(A.ACODE),1,2) IN ('02','16') and  a.branchcd='" + frm_mbr + "' and a.type like '4%' AND A.TYPE!='47' and  a.vchdate " + xprdRange + " " + cond + " " + cond1 + "  group by a.acode,b.aname order by  salee_ desc) where rownum<='10'"; //customer as per selection group or subgroup
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //party dt
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            dr1 = dt4.NewRow();
                            dr1["col_1"] = dt2.Rows[i]["acode"].ToString().Trim();
                            dr1["col_2"] = dt2.Rows[i]["aname"].ToString().Trim();
                            dr1["col_3"] = dt2.Rows[i]["salee_"].ToString().Trim();
                            dr1["col_4"] = dt2.Rows[i]["headerr"].ToString().Trim();
                            dt4.Rows.Add(dr1);
                        }
                    }
                    //======================================
                    header_n = "Our Top 10 Sale Items(In Order of Turnover)";
                    //SQuery = "select  distinct *  from (select '" + header_n + "' as header, a.icode,b.iname,ROUND(sum(a.iqtyout*a.irate/100000),2)  as salee from ivoucher a,item b where   trim(a.icode)=trim(b.icode) and a.icode like '9%'   and  a.branchcd='" + frm_mbr + "' and a.type like '4%' AND A.TYPE!='47' and  a.vchdate  " + xprdRange + "  group by a.icode,b.iname order by  salee desc) where rownum<='10'";
                    // SQuery = "select  distinct *  from (select '" + header_n + "' as header, a.icode,b.iname,ROUND(sum(a.iamount/100000),2) as salee from ivoucher a,item b where   trim(a.icode)=trim(b.icode) and a.icode like '9%'   and  a.branchcd='" + frm_mbr + "' and a.type like '4%'  and  a.vchdate  " + xprdRange + " and substr(trim(a.icode),1,2) like '" + party_cd + "%' and substr(trim(a.icode),1,4) like '" + part_cd + "%' group by a.icode,b.iname order by  salee desc) where rownum<='10'";
                    SQuery = "select  distinct *  from (select '" + header_n + "' as header, a.icode,b.iname,ROUND(sum(a.iqtyout*a.irate/100000),2)  as salee from ivoucher a,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and a.type like '4%' AND A.TYPE!='47' and  a.vchdate  " + xprdRange + " " + cond2 + " " + cond3 + "  group by a.icode,b.iname order by  salee desc) where rownum<='10'";// as per se;ection                                    
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //item dt
                    //if (dt3.Rows.Count > 0)
                    //{
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        dr1 = dt5.NewRow();
                        dr1["col_5"] = dt3.Rows[i]["icode"].ToString().Trim();
                        dr1["col_6"] = dt3.Rows[i]["iname"].ToString().Trim();
                        dr1["col_7"] = dt3.Rows[i]["salee"].ToString().Trim();
                        dr1["col_8"] = dt3.Rows[i]["header"].ToString().Trim();
                        dt5.Rows.Add(dr1);
                    }
                    // }
                    cnt = dt4.Rows.Count;
                    cnt1 = dt5.Rows.Count;

                    if (dt5.Rows.Count > 0)
                    {//// if there no any row in top sale item dt
                        if (cnt == cnt1)
                        {//if dt5 had rows equal to dt4 and dt5 less than dt4
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                dt4.Rows[i]["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dt4.Rows[i]["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dt4.Rows[i]["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dt4.Rows[i]["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                            }
                        }
                        if (cnt > cnt1)
                        {
                            for (int i = 0; i < cnt1; i++)
                            {
                                dt4.Rows[i]["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dt4.Rows[i]["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dt4.Rows[i]["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dt4.Rows[i]["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                            }
                        }
                        if (cnt < cnt1)
                        {//dt4 less than dt5
                            for (int i = 0; i < cnt; i++)
                            {
                                dt4.Rows[i]["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dt4.Rows[i]["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dt4.Rows[i]["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dt4.Rows[i]["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                            }
                            for (int i = cnt; i < cnt1; i++)
                            {
                                dr1 = dt4.NewRow();
                                dr1["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dr1["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dr1["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dr1["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                                dt4.Rows.Add(dr1);
                            }
                        }
                    }
                    dt4.TableName = "subrpt";
                    dsRep.Tables.Add(dt4);
                    //================
                    //  SQuery = "select sum(amt_sale/100000) as amt_Sale,sum(amt_exc/100000) as cgst,sum(rvalue/100000) as sgst  from sale where  branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and  vchdate " + xprdRange + ""; //old qry
                    SQuery = "select sum(b.amt_sale/100000) as amt_Sale,sum(b.amt_exc/100000) as cgst,sum(b.rvalue/100000) as sgst  from ivoucher a,sale b where  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and a.vchdate " + xprdRange + " " + cond2 + " " + cond3 + ""; //new qry ...data cominmg as per grp selection
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "mid";
                        dsRep.Tables.Add(dt);
                    }
                    #endregion
                    Print_Report_BYDS(frm_cocd, frm_mbr, "HIMT_Sales_Trend", "HIMT_Sales_Trend", dsRep, header_n);//Sales_Trend_HIMT
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F47111D":
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond1 = "";
                if (party_cd.Length < 2)
                {
                    cond = "and trim(acode) like '%'";
                }
                else
                {
                    cond = "and trim(acode) ='" + party_cd + "'";
                }
                if (part_cd.Length < 2)
                {
                    cond1 = "and trim(icode) like '%'";
                }
                else
                {
                    cond1 = "and trim(icode)='" + part_cd + "'";
                }
                mq0 = ""; mq1 = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");//CHOICE
                dt = new DataTable();
                mq0 = "select substr(solink,1,20) as solink,ACODE,icode,BUDGETCOST as qty,actualCOST as tentqty,000000000.00 AS job,000000000.00 AS SALE from budgmst  where  BRANCHCD='" + frm_mbr + "' AND type='46' and to_Date(desc_,'dd/mm/yyyy') " + xprdRange + " " + cond + " " + cond1 + " and BUDGETCOST+actualCOST>0 union all select convdate AS SOLINK,ACODE,icode,000000000.00 AS QTY ,000000000.00 AS tentqty,qty as job,000000000.00 AS SALE  from costestimate where BRANCHCD='" + frm_mbr + "' AND SUBSTR(type,1,2)='30' and VCHDATE " + xprdRange + " " + cond + " " + cond1 + " and srno=1 union all  select BRANCHCD||TYPE||PONUM||TO_CHAR(PODATE,'DD/MM/YYYY') AS SOLINK,ACODE,icode,000000000.00 AS qty,000000000.00 AS tentqty,000000000.00 as job,IQTYOUT as sale from IVOUCHER where  BRANCHCD='" + frm_mbr + "' AND SUBSTR(type,1,1)='4' and VCHDATE " + xprdRange + " " + cond + " " + cond1 + " and IQTYOUT>0";
                if (mq1 == "Y")//SALE ORDER WISE
                {
                    header_n = "Delivery Monitoring Report";
                    SQuery = "select '" + fromdt + "' AS FROMDT,'" + todt + "' AS TODT,'" + header_n + "'  AS HEADER,substr(a.solink,1,20) as solink,trim(B.ANAME) as aname,trim(C.INAME) AS CINAME,trim(C.CPARTNO) as cpartno,trim(A.ACODE) as acode,trim(A.ICODE) as icode,SUM(A.QTY) AS QTY,SUM(A.tentqty) AS tQTY,SUM(A.job) AS jQTY,SUM(A.SALE) AS SALEs  FROM ( " + mq0 + " )  A,FAMST B , ITEM C WHERE trim(A.ACODE)=trim(B.ACODE) AND trim(A.ICODE)=trim(C.ICODE) GROUP BY substr(a.solink,1,20),trim(A.ACODE),trim(B.ANAME),trim(C.INAME),trim(A.ICODE),trim(C.CPARTNO)  ORDER BY ACODE,CPARTNO,solink"; //this qry for sale order wise
                    frm_rptName = "Sales_Monitoring_Rep_SO"; //rpt name
                }
                else
                { //ITEM WISE
                    header_n = "Delivery Monitoring Report";
                    SQuery = "select '" + fromdt + "' AS FROMDT,'" + todt + "' AS TODT,'" + header_n + "' AS HEADER, trim(B.ANAME) as aname,trim(C.INAME) AS CINAME,trim(C.CPARTNO) AS CPARTNO,trim(A.ACODE) as acode,trim(A.ICODE) as icode,SUM(A.QTY) AS QTY,SUM(A.tentqty) AS tQTY,SUM(A.job) AS jQTY,SUM(A.SALE) AS SALEs  FROM (" + mq0 + ") A,FAMST B , ITEM C WHERE trim(A.ACODE)=trim(B.ACODE) AND trim(A.ICODE)=trim(C.ICODE) GROUP BY trim(B.ANAME),trim(C.INAME),trim(C.CPARTNO),trim(A.ACODE),trim(A.ICODE)  ORDER BY acode,cpartno";
                    frm_rptName = "Sales_Monitoring_Rep";//rpt name
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50328":
                #region
                header_n = "Schedule Vs Rcpt Vs Despatch Detail Report";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Length < 2)
                {
                    party_cd = "16";
                }
                if (part_cd.Length < 2)
                {
                    part_cd = "9";
                }

                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,A.ident,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31 from (SELECT Acode,icode,'1)S' as ident,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='46' and VCHDATE " + xprdRange + " UNION ALL  SELECT acode,icode,'2)R' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyin else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyin else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyin else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyin else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyin else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyin else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyin else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyin else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyin else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyin else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyin else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyin else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyin else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyin else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyin else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyin else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyin else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyin else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyin else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyin  else 0 end) as DAY20,(Case when to_char(vchdate,'dd')='21' then iqtyin else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyin  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyin else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyin  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyin  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyin else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyin else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyin  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyin  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyin  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyin  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type='08' and store='Y' and VCHDATE " + xprdRange + " and nvl(iqtyin,0)>0 union all SELECT acode,icode,'3)D' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and SUBSTR(TYPE,1,1) IN ('2','4') AND STORE='Y' and VCHDATE " + xprdRange + " and nvl(iqtyout,0)>0) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'group by A.ident,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit order by a.icode,A.ident";//with sum
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Rcpt_Vs_Desp_DayWise", "std_Sch_Vs_Rcpt_Vs_Desp_DayWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50329":
                #region
                header_n = "Schedule Vs Rcpt Vs Despatch Summary Report";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Length < 2)
                {
                    party_cd = "16";
                }
                if (part_cd.Length < 2)
                {
                    part_cd = "9";
                }
                mq0 = fromdt;
                mq1 = todt;
                mq2 = "";
                double days = DateTime.DaysInMonth(fgen.make_int(mq0.Substring(6, 4)), fgen.make_int(mq0.Substring(3, 2)));
                double d1 = fgen.make_double(todt.Substring(0, 2));
                xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                //mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as IOPQTY,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " " + cond + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " " + cond + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE"; //op bal and clos bal dt
                mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as op from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " " + cond + " and store='Y'  GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";//only op dt//nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq2);//stock dt
                //====================
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + days + "' as days,'" + d1 + "' as d1,ACODE,ICODE,ANAME,INAME,CPARTNO,UNIT,SUM(SCH_QTY) AS SCH_QTY,SUM(RCPT_qTY) AS RCPT_qTY,SUM(DESP_QTY) AS DESP_QTY FROM(select acode,icode,aname,iname,cpartno,unit,decode(ident,'1)S',TOT,0) as sch_qty,decode(ident,'2)R',TOT,0) AS RCPT_qTY,decode(ident,'3)D',TOT,0) AS DESP_QTY FROM (select a.acode,a.icode, A.ident,trim(a.acode)||trim(a.icode) as fstr,b.aname,c.iname,c.cpartno,c.unit,(a.day1+a.day2+a.day3+a.day4+a.day5+a.day6 +a.day7+a.day8+a.day9+a.day10+a.day11+a.day12+a.day13+a.day14+a.day15+a.day16+a.day17+a.day18+a.day19+a.day20+a.day21+a.day22+a.day23+a.day24+a.day25+a.day26+a.day27+a.day28+a.day29 +a.day30+a.day31) AS TOT  from (SELECT Acode,icode,'1)S' as ident,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='46' and VCHDATE " + xprdRange + " UNION ALL  SELECT acode,icode,'2)R' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyin else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyin else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyin else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyin else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyin else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyin else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyin else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyin else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyin else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyin else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyin else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyin else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyin else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyin else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyin else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyin else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyin else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyin else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyin else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyin  else 0 end) as DAY20,(Case when to_char(vchdate,'dd')='21' then iqtyin else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyin  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyin else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyin  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyin  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyin else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyin else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyin  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyin  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyin  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyin  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type='08' and store='Y' and VCHDATE " + xprdRange + " and nvl(iqtyin,0)>0 union all SELECT acode,icode,'3)D' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and SUBSTR(TYPE,1,1) IN ('2','4') AND STORE='Y'  and VCHDATE " + xprdRange + " and nvl(iqtyout,0)>0) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.icode,A.ident) ) GROUP BY ACODE,ICODE,ANAME,INAME,CPARTNO,UNIT order by iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("sch_expted", typeof(double));
                dt.Columns.Add("rcpt_expted", typeof(double));
                dt.Columns.Add("desp_expted", typeof(double));
                dt.Columns.Add("rcpt_compliance", typeof(double));
                dt.Columns.Add("desp_compliance", typeof(double));
                dt.Columns.Add("op", typeof(double));
                dr1 = dt.NewRow();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //expected formula====
                        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; double db7 = 0;
                        dt.Rows[i]["op"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "op"));
                        db = (d1 / days);
                        dt.Rows[i]["sch_expted"] = Math.Round(db * fgen.make_double(dt.Rows[i]["SCH_QTY"].ToString().Trim().Replace("NaN", "0")), 2);
                        dt.Rows[i]["rcpt_expted"] = Math.Round(db * fgen.make_double(dt.Rows[i]["RCPT_qTY"].ToString().Trim().Replace("NaN", "0")), 2);//as per client no need to show in report
                        dt.Rows[i]["desp_expted"] = Math.Round(db * fgen.make_double(dt.Rows[i]["DESP_QTY"].ToString().Trim().Replace("NaN", "0")), 2);//as per client no need to show in report                 
                        db1 = fgen.make_double(dt.Rows[i]["RCPT_qTY"].ToString().Trim().Replace("NaN", "0"));
                        db2 = fgen.make_double(dt.Rows[i]["rcpt_expted"].ToString().Trim().Replace("NaN", "0"));
                        db7 = fgen.make_double(dt.Rows[i]["sch_expted"].ToString().Trim().Replace("NaN", "0"));
                        if (db1 != 0 && db7 != 0)
                        {
                            db3 = Math.Round((db1 / db7) * 100, 2);//as per client                        
                        }
                        else
                        {
                            db3 = 0;
                        }
                        dt.Rows[i]["rcpt_compliance"] = db3;
                        db4 = fgen.make_double(dt.Rows[i]["DESP_QTY"].ToString().Trim().Replace("NaN", "0"));
                        if (db4 != 0 && db7 != 0)
                        {
                            db6 = Math.Round((db4 / db7) * 100, 2);
                        }
                        else
                        {
                            db6 = 0;
                        }
                        dt.Rows[i]["desp_compliance"] = db6;
                    }
                    ///============================
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Std_sch_vs_rcpt_desp_smry", "Std_sch_vs_rcpt_desp_smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
        }
    }

    public void Prodductionreps(string iconID)
    {
        Fill_Mst();
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;
        DataRow mdr, dr1;
        DataSet dsRep = new DataSet();
        string sname = "";
        string mq10, mq1, mq0;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string opt = "";
        switch (iconID)
        {
            case "F39201":
                #region Issue reqd
                SQuery = "select 'Material Issue Request' as header,'Material Issue Request' as h1,'Issue Agst Job Card' as h2, C.NAME AS DPT_NAME,I.INAME,I.CPARTNO,I.UNIT AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM wb_iss_req A, ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND C.ID='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "STD_ISS_rq", "STD_ISS_rq", dsRep, "Store Issue Request");
                #endregion
                break;
            case "F39211":
                #region job work  reqd
                SQuery = "select 'Job Work Request' as header,'Job Work Request' as h1,'Issue Agst Job Card' as h2, C.ANAME AS DPT_NAME,I.INAME,I.CPARTNO,I.UNIT AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM wb_iss_req A, ITEM I ,famst C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.acode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "STD_CHL_rq", "STD_CHL_rq", dsRep, "Store Issue Request");
                #endregion
                break;

            //prodn entry
            case "F50114":

            case "F39119":
                #region prodn
                dsRep = new DataSet();
                dt = new DataTable();
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0011", "OPT_ENABLE");
                SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,A.*,B.INAME,B.CPARTNO,B.UNIT as iunit FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.vchdate,a.vchnum,A.MORDER";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
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
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select aname,addr1,addr2,addr3,staten,email,website,gst_no from famst where trim(acode)='120000'");
                    dt.TableName = "FAMST";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_prod", "std_prod", dsRep, "Production Note Report");
                }
                #endregion
                break;

            case "F20132":
                // Gate Inward Register
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,A.SRNO,TRIM(A.ACODE) AS ACODE,(CASE WHEN A.PONUM='-' THEN '000000' ELSE A.PONUM END) AS PONUM,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.type as grp,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,A.IQTY_CHL,A.NARATION,I.INAME,I.UNIT,I.CPARTNO AS PARTNO,F.ANAME,TRIM(F.ADDR1)||TRIM(F.ADDR2) AS ADDRESS,A.MODE_TPT,A.DESC_ FROM IVOUCHERP A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='00' AND A.VCHDATE " + xprdRange + " ORDER BY A.SRNO";
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
            case "F35101":
                if (frm_IndType == "12")
                {
                    //SQuery = "select trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,'" + header_n + "' AS HEADER,null as pordno,null as pordt,null as saleid,null as ppcdt, b.aname,c.iname,c.cpartno,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,a.status,a.convdate,substr(A.convdate,5,6)||'  '||substr(A.convdate,11,20)  as orderno,a.srno,nvl(a.qty,0) as qty,a.remarks as peqty,a.col1,a.col2 as spec,a.col3 as particulr,a.col5 as bom_Qty,a.col6 as extra_qty,a.col7 as read_Qty, a.col9 as erpcde,a.enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,a.col14,a.col17,a.col24,a.itate,a.remarks,a.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_Dt,nvl(c.imagef,'-') as imagef,trim(c.cdrgno) as refno,C.UNIT from costestimate a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.vchnum,a.srno";
                    SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,'JOB ORDER' AS HEADER,s.pordno,s.porddt,s.busi_expect as saleid, b.aname,c.iname,c.cpartno,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,a.status,a.convdate,substr(A.convdate,5,6)||'  '||substr(A.convdate,11,20)  as orderno,a.comments as rmk,nvl(a.app_by,'-') as appby,a.srno,nvl(a.qty,0) as qty,a.remarks as peqty,a.col1,a.col2 as spec,a.col3 as particulr,a.col5 as bom_Qty,a.col6 as extra_qty,a.col7 as read_Qty, a.col9 as erpcde,a.enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,a.col14,a.col17,a.col24,a.itate,a.remarks,a.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_Dt,nvl(c.imagef,'-') as imagef,trim(c.cdrgno) as refno,C.UNIT,to_char(d.vchdate,'dd/mm/yyyy') as ppcdt from costestimate a left outer join somas s on trim(a.convdate)=trim(s.branchcd)||trim(s.type)||trim(s.ordno)||to_char(s.orddt,'dd/mm/yyyy') ,famst b,item c,INSPMST D where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.icode)=trim(d.icode) and d.type='70' and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.vchnum,a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt = fgen.addBarCode(dt, "fstr", true);
                        dt.TableName = "Prepcur";
                        dt.Columns.Add("ImgPath", typeof(string));
                        dt.Columns.Add("jcImg", typeof(System.Byte[]));
                        FileStream FilStr;
                        BinaryReader BinRed;
                        foreach (DataRow dr in dt.Rows)
                        {
                            #region
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
                            #endregion
                        }
                        //===
                        ds = new DataSet();
                        ds.Tables.Add(dt);
                        //===============
                        mq1 = "SELECT distinct trim(c.branchcd)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) as fstr1,A.NAME AS STAGE,b.icode as code,c.vchnum as job_no,to_char(c.vchdate,'dd/mm/yyyy') as job_dt,a.type1 FROM TYPE A,ITWSTAGE B,costestimate c WHERE A.ID='K' AND TRIM(A.TYPE1)=TRIM(B.STAGEC) and trim(b.icode)=trim(c.icode) AND  trim(c.branchcd)='" + frm_mbr + "' and trim(c.type)='" + frm_vty + "' and trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by fstr1,b.icode,a.type1";
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        dt4 = new DataTable();
                        dt4.Columns.Add("fstr1", typeof(string));
                        dt4.Columns.Add("stage", typeof(string));

                        if (dt3.Rows.Count > 0)
                        {
                            //  DataRow dr6 = new DataRow();
                            // foreach(DataRow dr3 in dt3.Rows)
                            mq1 = ""; mq2 = "";
                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                mq1 += dt3.Rows[i]["STAGE"].ToString().Trim() + ">";
                                mq2 = dt3.Rows[i]["fstr1"].ToString().Trim();
                            }
                            //for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                dr1 = dt4.NewRow();
                                dr1["STAGE"] = mq1;
                                dr1["fstr1"] = mq2;
                                dt4.Rows.Add(dr1);
                            }
                        }
                        //============   
                        dt4.TableName = "stages";
                        ds.Tables.Add(dt4);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_jc_poly", "std_jc_poly", ds, "");
                    }
                }
                else
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
                    SQuery = "select DISTINCT A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.iweight,b.cdrgno,b.cpartno,c.aname as party,nvl(b.imagef,'-') as imagef from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.vchnum,a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    DataTable dtm3 = new DataTable();
                    dtm3 = fgen.getdata(frm_qstr, frm_cocd, "SELECT branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr, branchcd,type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,icode, (case when qtyord>0 then round(qtysupp/qtyord*100,2) else 0 end) as so_tol  FROM somas  where branchcd='" + frm_mbr + "' and type='40' and orddt " + DateRange + "");
                    DataTable dt7 = new DataTable();
                    dt7 = fgen.getdata(frm_qstr, frm_cocd, "select distinct trim(d.icode) as icode, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,trim(d.TITLE) as title,trim(d.REMARK2) as REMARK2,trim(d.REMARK3) as REMARK3,trim(d.REMARK4) as REMARK4 from inspmst d where d.branchcd='" + frm_mbr + "' and d.type='70' and vchdate " + DateRange + "  order by icode");
                    DataTable dt8 = new DataTable();
                    // dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT icode,btchdt FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' and vchdate " + DateRange + ""); //old
                    dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,icode,btchdt,col13 as cylinder,col14 as lbl_AROUND,grade as gap_acros,col15 as lbl_acros,col16 as gap_around,maintdt as lbl_width FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' and vchdate " + DateRange + "");//after add some new fileds

                    ds = new DataSet();
                    dt1 = dt2.Clone();
                    dt3 = dt2.Clone();
                    dt4 = dt2.Clone();
                    dt6 = new DataTable();//for more thn 47 rows in job card
                    dt5 = dt2.Clone();
                    dt6 = dt2.Clone();
                    double papergiven = 0;
                    double jcqty1 = 0;
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
                    int index = 0;
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
                            mq0 = "SELECT '-' as fstr,'-' as STAGE,'-' as TYPE1,'-' as icode,'-' as srno,'-' as job_no,'-' as job_dt,'-' as NET_PRODN,'-' as REJ,'-' as machinename from dual";
                            dtm = new DataTable();
                            dtm = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        }
                        //============
                        dtm.TableName = "type1";
                        ds.Tables.Add(dtm);
                        switch (frm_IndType)
                        {
                            case "13":
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_jc_Label", "std_jc_Label", ds, "");//FOR LSBEL RPT
                                break;
                            case "12":
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_jc_Flex", "std_jc_Flex", ds, ""); //FOR FLEX REPT                            
                                break;
                            default:
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_jobCard", "std_jobCard", ds, "");
                                break;
                        }
                        #endregion
                    }
                }
                break;

            case "F35106":
            case "F35107": // Machine Planning
                #region planning sheet
                dtm = new DataTable();
                dtm.Columns.Add("header", typeof(string));
                dtm.Columns.Add("fstr", typeof(string));
                dtm.Columns.Add("jobno", typeof(string));
                dtm.Columns.Add("jobdt", typeof(string));
                dtm.Columns.Add("fromdt", typeof(string));
                dtm.Columns.Add("todt", typeof(string));
                dtm.Columns.Add("vchnum", typeof(string));
                dtm.Columns.Add("vchdate", typeof(string));
                dtm.Columns.Add("icode", typeof(string));
                dtm.Columns.Add("partno", typeof(string));
                dtm.Columns.Add("iname", typeof(string));
                dtm.Columns.Add("desc", typeof(string));
                dtm.Columns.Add("sheet", typeof(string));
                dtm.Columns.Add("ups", typeof(string));
                dtm.Columns.Add("flute", typeof(string));
                dtm.Columns.Add("std_bs", typeof(string));
                dtm.Columns.Add("std_cs", typeof(string));
                dtm.Columns.Add("std_board", typeof(string));
                dtm.Columns.Add("reel_cm", typeof(string));
                dtm.Columns.Add("LIN_MTR", typeof(string));
                dtm.Columns.Add("TOT_WT", typeof(double));
                dtm.Columns.Add("cut_mm", typeof(string));
                dtm.Columns.Add("OD", typeof(string));
                dtm.Columns.Add("joint", typeof(string));
                dtm.Columns.Add("ID", typeof(string));
                dtm.Columns.Add("col1", typeof(string));
                dtm.Columns.Add("col2", typeof(string));
                dtm.Columns.Add("col3", typeof(string));
                dtm.Columns.Add("col4", typeof(string));
                dtm.Columns.Add("col5", typeof(string));
                dtm.Columns.Add("color", typeof(string));
                dtm.Columns.Add("tot_plan_wt", typeof(double));

                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,b.iname,b.cpartno,a1 as sheet,a.job_no,a.job_dt,trim(a.job_no)||trim(a.job_dt) as fstr,a.ename ,a.tempr from prod_sheet A ,item b where  TRIM(a.icode)=trim(b.icode)  AND /*a.branchcd='" + frm_mbr + "' and a.type='90' and a.vchdate " + xprdRange + "*/ a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ")  order by a.srno,a.vchnum";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt1 = new DataTable();
                SQuery = "select  col1,col2,col14,COL16,icode,MAINTDT,REJQTY AS UPS from inspmst where branchcd='" + frm_mbr + "' and type='70' and vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') order by srno"; ///process plan 
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                DataTable DT23 = new DataTable();
                SQuery = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col7,COL17 AS TOT_WT ,icode  from costestimate where branchcd='" + frm_mbr + "' and  type='30' and vchdate " + DateRange + " order by srno";
                DT23 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //jobcard query
                double d = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr0 = dtm.NewRow();
                        dr0["header"] = "Planning Sheet";
                        dr0["vchnum"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr0["vchdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                        dr0["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr0["partno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                        dr0["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr0["desc"] = dt.Rows[i]["tempr"].ToString().Trim();
                        dr0["sheet"] = dt.Rows[i]["sheet"].ToString().Trim();
                        dr0["jobno"] = dt.Rows[i]["job_no"].ToString().Trim();
                        dr0["jobdt"] = dt.Rows[i]["job_dt"].ToString().Trim();
                        dr0["fromdt"] = dt.Rows[i]["frmdt"].ToString().Trim();
                        dr0["todt"] = dt.Rows[i]["todt"].ToString().Trim();
                        dr0["fstr"] = dt.Rows[i]["fstr"].ToString().Trim();

                        dt2 = new DataTable();
                        if (dt1.Rows.Count > 0)
                        {
                            DataView viewim = new DataView(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt2 = viewim.ToTable();
                        }
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr0["OD"] = dt2.Rows[j]["col14"].ToString().Trim();
                                dr0["ID"] = dt2.Rows[j]["col16"].ToString().Trim();
                                dr0["reel_cm"] = dt2.Rows[j]["MAINTDT"].ToString().Trim();
                                dr0["ups"] = dt2.Rows[j]["ups"].ToString().Trim();
                            }
                            mq2 = "";
                            mq2 = dt2.Rows[j]["col1"].ToString().Trim();
                            if (mq2 == "FLUTE TYPE")
                            {
                                dr0["flute"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            //////////////////
                            if (mq2 == "BURSTING STRENGTH")
                            {
                                dr0["std_bs"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            //////////////////////
                            if (mq2 == "COMPR.STRENGTH")
                            {
                                dr0["std_cs"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            ////////////////
                            if (mq2 == "BOARD GSM")
                            {
                                dr0["std_board"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            //////////////
                            if (mq2 == "CUT SIZE (MM)")
                            {
                                dr0["cut_mm"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            if (mq2.Contains("COLOR") || mq2.Contains("COLOUR"))
                            {
                                dr0["color"] += dt2.Rows[j]["col2"].ToString().Trim() + ",";
                            }
                            if (mq2 == "STAPLING/GLUING")
                            {
                                dr0["JOINT"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            ///////////////                                                                       

                        }
                        int l = 1;
                        dt3 = new DataTable();
                        ////////from jobcard
                        if (DT23.Rows.Count > 0)
                        {
                            DataView viewim1 = new DataView(DT23, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and vchnum='" + dt.Rows[i]["job_no"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt3 = viewim1.ToTable();
                        }
                        if (dt3.Rows.Count > 0)
                        {
                            for (int j = 1; j <= 5; j++)
                            {
                                dr0["TOT_WT"] = fgen.make_double(dt3.Rows[0]["TOT_WT"].ToString().Trim());
                                if (dt3.Rows.Count > j)
                                {
                                    mq3 = "";
                                    mq3 = dt3.Rows[j]["col7"].ToString().Trim();
                                    if (mq3 != "0")
                                    {
                                        dr0["col" + l] = "[" + dt3.Rows[j]["col2"].ToString().Trim() + "]" + dt3.Rows[j]["col3"].ToString().Trim() + " [" + dt3.Rows[j]["col7"].ToString().Trim() + "kg]";
                                    }
                                }
                                l++;
                            }

                        }
                        dtm.Rows.Add(dr0);
                    }
                    //////for bar code
                    dtm = fgen.addBarCode(dtm, "fstr", true);
                }
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_ControlPlanning_sheet", "std_ControlPlanning_sheet", dsRep, "");
                }
                #endregion
                break;
        }
    }


    public void Enggreps(string iconID)
    {
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;
        DataRow mdr, dr1;
        DataSet dsRep = new DataSet();
        string sname = "";
        string mq10, mq1, mq0;
        int repCount = 1;
        data_found = "Y";
        string opt = "";
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
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //BOM Layered
            case "F10131L":
                #region BOM
                dsRep = new DataSet();

                //********************                
                DataTable mdt = new DataTable(); dt3 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); DataTable mdt1 = new DataTable();
                SQuery = "Select A.BRANCHCD,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.srno,A.ICODE,A.IBCODE,A.IBQTY,(case when B.IQD>0 then B.IQD else B.irate end) AS itrate,b.iname as ibname,b.cpartno as bcpartno,b.unit as bunit,substr(a.ibcat,2,6) as ibcat,a.main_issue_no,a.sub_issue_no,a.st_type,a.ibwt,c.iname as iname,c.cpartno as cpartno,c.unit,a.ent_by,a.ent_dt from itemosp a,item b,item c where trim(a.ibcode)=trim(b.icode) and trim(A.icodE)=trim(c.icode) AND a.BRANCHCD='" + frm_mbr + "' and a.type='BM' and a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "' order by a.srno,a.icode";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                DataTable vdt = new DataTable();
                mdt.Columns.Add(new DataColumn("branchcd", typeof(string)));
                mdt.Columns.Add(new DataColumn("type", typeof(string)));

                mdt.Columns.Add(new DataColumn("srno", typeof(double)));
                mdt.Columns.Add(new DataColumn("vchnum", typeof(string)));
                mdt.Columns.Add(new DataColumn("vchdate", typeof(string)));

                mdt.Columns.Add(new DataColumn("bvchnum", typeof(string)));
                mdt.Columns.Add(new DataColumn("bvchdate", typeof(string)));

                mdt.Columns.Add(new DataColumn("lvl", typeof(double)));
                mdt.Columns.Add(new DataColumn("icode", typeof(string)));
                mdt.Columns.Add(new DataColumn("pcode", typeof(string)));
                mdt.Columns.Add(new DataColumn("mqty", typeof(double)));
                mdt.Columns.Add(new DataColumn("ibqty", typeof(double)));
                mdt.Columns.Add(new DataColumn("ibcode", typeof(string)));
                mdt.Columns.Add(new DataColumn("irate", typeof(double)));
                mdt.Columns.Add(new DataColumn("val", typeof(double)));
                mdt.Columns.Add(new DataColumn("ibcat", typeof(string)));

                mdt.Columns.Add(new DataColumn("iname", typeof(string)));
                mdt.Columns.Add(new DataColumn("sname", typeof(string)));
                mdt.Columns.Add(new DataColumn("cpartno", typeof(string)));
                mdt.Columns.Add(new DataColumn("unit", typeof(string)));

                mdt.Columns.Add(new DataColumn("ibname", typeof(string)));
                mdt.Columns.Add(new DataColumn("bcpartno", typeof(string)));
                mdt.Columns.Add(new DataColumn("bunit", typeof(string)));

                mdt.Columns.Add(new DataColumn("ent_by", typeof(string)));
                mdt.Columns.Add(new DataColumn("ent_dt", typeof(DateTime)));

                mdt.Columns.Add(new DataColumn("star", typeof(string)));

                DataTable fmdt = new DataTable();
                fmdt.Columns.Add(new DataColumn("icode", typeof(string)));
                fmdt.Columns.Add(new DataColumn("val", typeof(string)));

                //SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD='" + frm_mbr + "' order by a.srno,a.icode,a.ibcode";
                //vdt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                int v = 0;
                int srno = 1;
                dt2 = new DataTable();
                //SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where branchcd='" + mbr + "' and type like '0%' and trim(nvl(finvno,'-'))!='-' and vchdate>=(sysdate-500)  /*and icode like '9%'*/ order by icode,vdd desc";                
                DataView dist1_view = new DataView(dt3);
                DataTable dt_dist = new DataTable();
                if (dist1_view.Count > 0)
                {
                    dist1_view.Sort = "icode";
                    dt_dist = dist1_view.ToTable(true, "icode");
                }
                foreach (DataRow dt_dist_row in dt_dist.Rows)
                {
                    mdt1 = new DataTable();
                    mdt1 = mdt.Clone();
                    DataView mvdview = new DataView(dt3, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "icode,ibcode", DataViewRowState.CurrentRows);
                    dt = new DataTable();
                    mvdview.Sort = "srno,icode";
                    dt = mvdview.ToTable();
                    // filling parent
                    foreach (DataRow drc in dt.Rows)
                    {
                        dro = mdt1.NewRow();
                        dro["lvl"] = "1";
                        dro["branchcd"] = drc["branchcd"].ToString().Trim();

                        dro["srno"] = srno;
                        dro["vchnum"] = drc["vchnum"].ToString().Trim();
                        dro["vchdate"] = drc["vchdate"].ToString().Trim();

                        dro["bvchnum"] = "**********";
                        dro["bvchdate"] = "**********";

                        dro["icode"] = drc["icode"].ToString().Trim();
                        dro["pcode"] = drc["icode"].ToString().Trim();
                        dro["ibqty"] = drc["ibqty"];
                        dro["ibcode"] = drc["ibcode"].ToString().Trim();
                        dro["irate"] = drc["itrate"].ToString().Trim();
                        dro["ibcat"] = drc["ibcat"].ToString().Trim();

                        dro["iname"] = drc["iname"].ToString().Trim();
                        dro["cpartno"] = drc["cpartno"].ToString().Trim();
                        dro["unit"] = drc["unit"].ToString().Trim();

                        dro["ibname"] = drc["ibname"].ToString().Trim();
                        dro["bcpartno"] = drc["bcpartno"].ToString().Trim();
                        dro["bunit"] = drc["bunit"].ToString().Trim();

                        dro["ent_by"] = drc["ent_by"].ToString().Trim();
                        dro["ent_dt"] = drc["ent_dt"].ToString().Trim();

                        dro["sname"] = drc["iname"].ToString().Trim();
                        dro["mqty"] = drc["ibqty"];

                        dro["val"] = "0";
                        mdt1.Rows.Add(dro);

                        srno++;
                    }
                    int i0 = 1; v = 0;
                    for (int i = v; i < mdt1.Rows.Count; i++)
                    {
                        srno = 1;
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.BRANCHCD,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.srno,A.ICODE,A.IBCODE,A.IBQTY,(case when B.IQD>0 then B.IQD else B.irate end) AS BCHRATE,b.iname as ibname,b.cpartno as bcpartno,b.unit as bunit,substr(a.ibcat,2,6) as ibcat,a.main_issue_no,a.sub_issue_no,a.st_type,a.ibwt FROM ITEMOSP A,ITEM B WHERE TRIM(A.IBCODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "' order by a.srno");
                        if (dt2.Rows.Count > 0)
                        {
                            DataView vdview1 = new DataView(mdt1, "icode='" + mdt1.Rows[i]["icode"].ToString().Trim() + "' and ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "' and ibqty='" + mdt1.Rows[i]["ibqty"] + "'", "ibcode", DataViewRowState.CurrentRows);
                            if (vdview1.Count <= 0) vdview1 = new DataView(mdt1, "ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "ibcode", DataViewRowState.CurrentRows);
                            i0 += 1;

                            for (int x = 0; x < dt2.Rows.Count; x++)
                            {
                                //if (mq0 != dt2.Rows[x]["icode"].ToString().Trim()) i0 += 1;
                                dro = mdt1.NewRow();
                                dro["lvl"] = i0.ToString();
                                dro["srno"] = srno;

                                dro["icode"] = dt2.Rows[x]["icode"].ToString().Trim();
                                dro["branchcd"] = dt2.Rows[x]["branchcd"].ToString().Trim();
                                dro["ibcat"] = dt2.Rows[x]["ibcat"].ToString().Trim();

                                dro["vchnum"] = vdview1[0].Row["vchnum"].ToString().Trim();
                                dro["vchdate"] = vdview1[0].Row["vchdate"].ToString().Trim();

                                dro["bvchnum"] = dt2.Rows[x]["vchnum"].ToString().Trim();
                                dro["bvchdate"] = dt2.Rows[x]["vchdate"].ToString().Trim();

                                mq0 = dt2.Rows[x]["icode"].ToString().Trim();
                                //dro["ibqty"] = (Convert.ToDouble(dt2.Rows[x]["ibqty"]) * Convert.ToDouble(vdview1[0].Row["ibqty"])).ToString();
                                dro["ibqty"] = dt2.Rows[x]["ibqty"];
                                dro["ibcode"] = dt2.Rows[x]["ibcode"].ToString().Trim();
                                dro["irate"] = dt2.Rows[x]["bchrate"];

                                dro["iname"] = vdview1[0].Row["iname"].ToString().Trim();
                                dro["cpartno"] = vdview1[0].Row["cpartno"].ToString().Trim();
                                dro["unit"] = vdview1[0].Row["unit"].ToString().Trim();

                                dro["ibname"] = dt2.Rows[x]["ibname"].ToString().Trim();
                                dro["bcpartno"] = dt2.Rows[x]["bcpartno"].ToString().Trim();
                                dro["bunit"] = dt2.Rows[x]["bunit"].ToString().Trim();

                                dro["ent_by"] = vdview1[0].Row["ent_by"].ToString().Trim();
                                dro["ent_dt"] = vdview1[0].Row["ent_dt"].ToString().Trim();

                                dro["val"] = "0";

                                dro["sname"] = fgen.seek_iname_dt(mdt1, "ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "ibname");
                                dro["mqty"] = fgen.seek_iname_dt(mdt1, "ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "ibqty");

                                if (mdt1.Rows[i]["lvl"].ToString() == "1")
                                {
                                    mq7 = "";
                                    dro["pcode"] = mdt1.Rows[i]["icode"].ToString().Trim();
                                    mq7 = mdt1.Rows[i]["icode"].ToString().Trim();
                                }
                                else dro["pcode"] = mq7;
                                v++;

                                srno++;

                                mdt1.Rows.Add(dro);
                            }
                            vdview1.Dispose();
                        }
                        vdview.Dispose();
                    }

                    DataView sort_view = new DataView();
                    sort_view = mdt1.DefaultView;
                    sort_view.Sort = "lvl,srno,pcode,icode";
                    mdt1 = new DataTable();
                    mdt1 = sort_view.ToTable(true);
                    sort_view.Dispose();

                    // seeking LC and update value
                    for (int i = 0; i < mdt1.Rows.Count; i++)
                    {
                        vdview = new DataView(mdt1, "branchcd='" + mdt1.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                        if (vdview.Count <= 0)
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                sort_view = new DataView(dt2, "branchcd='" + mdt1.Rows[i]["branchcd"].ToString().Trim() + "' and trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                else
                                {
                                    sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                    if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                }
                            }
                        }
                        else mdt1.Rows[i]["irate"] = "0";
                        vdview.Dispose();
                        mdt1.Rows[i]["val"] = Convert.ToDouble(Convert.ToDouble(mdt1.Rows[i]["ibqty"]) * Convert.ToDouble(mdt1.Rows[i]["irate"]));
                    }

                    mq0 = "0";
                    // making final value
                    vdview = new DataView(mdt1, "pcode='" + dt_dist_row["icode"].ToString().Trim() + "'", "pcode", DataViewRowState.CurrentRows);
                    for (int i = 0; i < vdview.Count; i++)
                    {
                        if (Convert.ToDouble(mq0) > 0) mq0 = Math.Round(Convert.ToDouble(mq0) + Convert.ToDouble(vdview[i].Row["val"].ToString().Trim()), 2).ToString();
                        else mq0 = vdview[i].Row["val"].ToString().Trim();
                    }
                    vdview.Dispose();

                    for (int f = 0; f < mdt1.Rows.Count; f++)
                    {
                        mdt.ImportRow(mdt1.Rows[f]);
                    }

                    //has child
                    if (mdt.Rows.Count > 0)
                    {
                        dist1_view = new DataView(mdt1, "", "", DataViewRowState.CurrentRows);
                        dt_dist = new DataTable();
                        dt_dist = dist1_view.ToTable(true, "icode");

                        foreach (DataRow dr in dt_dist.Rows)
                        {
                            for (int f = 0; f < mdt.Rows.Count; f++)
                            {
                                if (mdt.Rows[f]["ibcode"].ToString().Trim() == dr["icode"].ToString().Trim())
                                {
                                    mdt.Rows[f]["star"] = "*";
                                }
                            }
                        }
                    }


                    mdt1.Dispose();
                    // mdt is table which is having Bom in Expended Form
                    dro1 = fmdt.NewRow();
                    dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
                    dro1["val"] = mq0;
                    fmdt.Rows.Add(dro1);
                    // fmdt is table which is only having Parant Bom icode and Value                        
                }

                //********************
                if (mdt.Rows.Count > 0)
                {
                    mdt.TableName = "Prepcur";
                    dsRep.Tables.Add(mdt);
                }
                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    Print_Report_BYDS(frm_cocd, frm_mbr, "bom_entry_L", "bom_entry_L", dsRep, "BOM Entry Report");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F10131":

                break;
            case "F10188":
                SQuery = "select a.*,b.aname as aname,C.Iname from scratch2 a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icodE)=trim(c.icode) and a.type='LC' and A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode.Trim() + "'";

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);

                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "lcostsheet", "lcostsheet", dsRep, "Costing Sheet");
                    }
                }
                break;

            case "F10055":
                dsRep = new DataSet();
                dt = new DataTable();
                frm_rptName = "cnitc";
                if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI")
                {
                    frm_rptName = "csydb";
                    SQuery = "Select a.*,(case when trim(nvl(b.INAME,'-'))='-' then a.t121 else b.INAME end) as INAME from (select a.*,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as aname from (Select * from somas_anx a where A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode.Trim() + "') a left outer join famst b on trim(a.acode)=trim(b.acode)) a left outer join item b on trim(a.icode)=trim(b.icode) ";
                }
                else SQuery = "select a.*,b.aname,c.iname from somas_anx a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icodE)=trim(c.icode) and a.type='PN' and A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode.Trim() + "'";

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                }
                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    Print_Report_BYDS(frm_cocd, frm_mbr, "cnitc", frm_rptName, dsRep, "Costing Sheet");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10135":
                pdfView = "Y";
                header_n = "Process Plan";
                SQuery = "SELECT B.INAME AS ITEMNAME,B.CDRGNO AS CUST_IT_CODE,C.ANAME AS CUSTOEMR,a.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,A.TITLE as Remarks,A.ACODE,A.ICODE,A.CPARTNO,A.SRNO,A.BTCHNO AS SR,COL1 AS PROCESS,A.COL2 AS SPECIFICATION,A.COL3 AS Reqmt,A.COL4 as RMK, A.COL5 AS ERPCODE,A.COL6 AS UOM,A.COL9 AS COBB_IN,A.COL10 AS FLUTE,A.COL11 AS HEIGHT,A.COL12 AS DIENO,A.COL13 AS TYPE_OF_ITEM,A.COL14 AS CTN_SIZE_OD,A.COL15 as PLy,A.COL16 AS CTN_SIZE_ID,A.COL17,A.COL18 AS Std_Rej_Allow,A.REJQTY  AS UPS,A.REMARK2,REMARK3,REMARK4,A.ENT_BY,TO_cHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,A.APP_BY,A.APP_DT,A.EDT_BY,TO_CHAR(A.EDT_DT,'DD/MM/YYYY') AS EDT_DT,A.AMDCOMMENT AS AMEN1,A.AMDDT AS AMDT1,A.AMDCOMMENT2 AS AMEN2 ,A.AMDDT2,A.AMDCOMMENT3 AS AMEN3,A.AMDDT3,A.AMDCOMMENT4 AS AMEN4,A.AMDDT4,A.AMDCOMMENT5 AS AMEN5,A.AMDDT5,A.AMDNO,nvl(b.IMAGEF,'-') as IMAGEF FROM  INSPMST  A,ITEM B ,FAMST C WHERE A.BRANCHCD='" + frm_mbr + "' AND A .TYPE='70' AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("planImg", typeof(System.Byte[]));
                    FileStream FilStr;
                    BinaryReader BinRed;
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            fpath = dr["imagef"].ToString().Trim();
                            FilStr = new FileStream(fpath, FileMode.Open);
                            BinRed = new BinaryReader(FilStr);
                            dr["planImg"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                            FilStr.Close();
                            BinRed.Close();
                        }
                        catch { }
                    }

                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Process_Plan", "Process_Plan", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10133":
                header_n = "Item Stage Mapping";
                SQuery = "SELECT DISTINCT C.NAME AS STAGES,B.VCHNUM,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCHDATE,B.STAGEC,B.ICODE,D.INAME,B.MTIME1,B.SRNO,A.MCHCODE,A.MCHNAME AS SATGE_NAME FROM ITWSTAGE B LEFT OUTER JOIN PMAINT A ON TRIM(A.ACODE)||'/'||TRIM(A.SRNO)=TRIM(B.OPCODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='10', TYPE C,ITEM D WHERE  B.BRANCHCD='" + frm_mbr + "' AND B.TYPE='10'  AND TRIM(B.STAGEC)=TRIM(C.TYPE1) AND C.ID='K' AND TRIM(B.ICODE)=TRIM(D.ICODE) AND  TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')='" + barCode + "' ORDER BY B.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_StageMapping", "std_StageMapping", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10144":
            case "F10149":
                xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                header_n = "Box Costing";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (iconID == "F10144")
                {
                    SQuery = "SELECT '" + header_n + "' as header, a.code,a.aname,a.iname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.trannum,a.lt,a.wd,a.ht,a.ply,a.flute,a.cs,a.caliper,a.z,a.rqect,a.rqbs,a.rqgsm,a.deckle,a.length,a.area,a.minect,a.maxect,a.avgect,a.mincs,a.maxcs,a.avgcs,a.mingsm,a.maxgsm,a.avggsm,a.minbs,a.maxbs,a.avgbs,a.minwt,a.maxwt,a.avgwt,a.contribution,a.contamt,a.tconcst as conver_cost,a.cstpkg as cost_kg,a.papcst as papercost,a.pawastage as paper_wastg,a.pawastageamt as pap_wstg_amt,a.boxcost,a.h_16,a.n_16,a.h_18,a.n_18,a.h_20,a.n_20,a.h_22,a.n_22,a.h_24,a.n_24,a.h_28,a.n_28,a.h_35,a.n_35,a.h_45,a.n_45   FROM wb_corrcst_TRANS a WHERE a.branchcd='" + frm_mbr + "' and trim(a.branchcd)||trim(a.TRANNUM)='" + barCode + "'  and vchdate " + xprdRange + " ";
                }
                else
                {
                    SQuery = "SELECT '" + header_n + "' as header, a.code,a.aname,a.iname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.trannum,a.lt,a.wd,a.ht,a.ply,a.flute,a.cs,a.caliper,a.z,a.rqect,a.rqbs,a.rqgsm,a.deckle,a.length,a.area,a.minect,a.maxect,a.avgect,a.mincs,a.maxcs,a.avgcs,a.mingsm,a.maxgsm,a.avggsm,a.minbs,a.maxbs,a.avgbs,a.minwt,a.maxwt,a.avgwt,a.contribution,a.contamt,a.tconcst as conver_cost,a.cstpkg as cost_kg,a.papcst as papercost,a.pawastage as paper_wastg,a.pawastageamt as pap_wstg_amt,a.boxcost,a.h_16,a.n_16,a.h_18,a.n_18,a.h_20,a.n_20,a.h_22,a.n_22,a.h_24,a.n_24,a.h_28,a.n_28,a.h_35,a.n_35,a.h_45,a.n_45   FROM wb_corrcst_TRANS a WHERE a.branchcd='" + frm_mbr + "' and trim(a.branchcd)||trim(a.TRANNUM)='" + mq0 + "'  and vchdate " + xprdRange + "";
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ////dt1 for layers in rpt
                #region colm for left side  in rpt
                dtm = new DataTable();
                dtm.Columns.Add("gsm_0", typeof(string));
                dtm.Columns.Add("gsm_1", typeof(string));
                dtm.Columns.Add("gsm_2", typeof(string));
                dtm.Columns.Add("gsm_3", typeof(string));
                dtm.Columns.Add("gsm_4", typeof(string));

                dtm.Columns.Add("bf_0", typeof(string));
                dtm.Columns.Add("bf_1", typeof(string));
                dtm.Columns.Add("bf_2", typeof(string));
                dtm.Columns.Add("bf_3", typeof(string));
                dtm.Columns.Add("bf_4", typeof(string));

                dtm.Columns.Add("rctgrade_0", typeof(string));
                dtm.Columns.Add("rctgrade_1", typeof(string));
                dtm.Columns.Add("rctgrade_2", typeof(string));
                dtm.Columns.Add("rctgrade_3", typeof(string));
                dtm.Columns.Add("rctgrade_4", typeof(string));

                dtm.Columns.Add("rct_0", typeof(string));
                dtm.Columns.Add("rct_1", typeof(string));
                dtm.Columns.Add("rct_2", typeof(string));
                dtm.Columns.Add("rct_3", typeof(string));
                dtm.Columns.Add("rct_4", typeof(string));

                dtm.Columns.Add("t_rct_0", typeof(string));
                dtm.Columns.Add("t_rct_1", typeof(string));
                dtm.Columns.Add("t_rct_2", typeof(string));
                dtm.Columns.Add("t_rct_3", typeof(string));
                dtm.Columns.Add("t_rct_4", typeof(string));


                dtm.Columns.Add("cost_0", typeof(string));
                dtm.Columns.Add("cost_1", typeof(string));
                dtm.Columns.Add("cost_2", typeof(string));
                dtm.Columns.Add("cost_3", typeof(string));
                dtm.Columns.Add("cost_4", typeof(string));

                dtm.Columns.Add("tot_t_Rct", typeof(string));
                dtm.Columns.Add("tot_cost", typeof(string));
                #endregion

                #region for right side in rpt
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("strch_rt", typeof(string)); //00
                ph_tbl.Columns.Add("strch_flg", typeof(string));
                ph_tbl.Columns.Add("strch_amt", typeof(string));
                ph_tbl.Columns.Add("pva_rt", typeof(string));//01
                ph_tbl.Columns.Add("pva_flg", typeof(string));
                ph_tbl.Columns.Add("pva_amt", typeof(string));
                ph_tbl.Columns.Add("power_rt", typeof(string));//02
                ph_tbl.Columns.Add("power_flg", typeof(string));
                ph_tbl.Columns.Add("power_amt", typeof(string));
                ph_tbl.Columns.Add("fuel_rt", typeof(string));//03
                ph_tbl.Columns.Add("fuel_flg", typeof(string));
                ph_tbl.Columns.Add("fuel_amt", typeof(string));
                ph_tbl.Columns.Add("pins_rt", typeof(string));//04
                ph_tbl.Columns.Add("pins_flg", typeof(string));
                ph_tbl.Columns.Add("pins_amt", typeof(string));
                ph_tbl.Columns.Add("ink_rt", typeof(string));//05
                ph_tbl.Columns.Add("ink_flg", typeof(string));
                ph_tbl.Columns.Add("ink_amt", typeof(string));
                ph_tbl.Columns.Add("labr_rt", typeof(string));//06
                ph_tbl.Columns.Add("labr_flg", typeof(string));
                ph_tbl.Columns.Add("labr_amt", typeof(string));
                ph_tbl.Columns.Add("admin_rt", typeof(string));//07
                ph_tbl.Columns.Add("admin_flg", typeof(string));
                ph_tbl.Columns.Add("admin_amt", typeof(string));
                ph_tbl.Columns.Add("trans_rt", typeof(string));//08
                ph_tbl.Columns.Add("trans_flg", typeof(string));
                ph_tbl.Columns.Add("trans_amt", typeof(string));
                ph_tbl.Columns.Add("mat_rt", typeof(string));//09
                ph_tbl.Columns.Add("mat_flg", typeof(string));
                ph_tbl.Columns.Add("mat_amt", typeof(string));

                #endregion
                if (iconID == "F10144")
                {
                    mq1 = "SELECT a.code,a.srno,a.trannum,a.trandt,a.gsm,a.bf,a.rctgrade,a.rct,a.t_rct,a.cost,a.desc_ as layer,a.totrct,a.totcost  FROM wb_CORRCST_LAYER a WHERE  trim(a.TRANNUM)='" + barCode + "'";
                }
                else
                {
                    mq1 = "SELECT a.code,a.srno,a.trannum,a.trandt,a.gsm,a.bf,a.rctgrade,a.rct,a.t_rct,a.cost,a.desc_ as layer,a.totrct,a.totcost  FROM wb_CORRCST_LAYER a WHERE  trim(a.TRANNUM)='" + mq0 + "'";
                }
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                /////
                if (iconID == "F10144")
                {
                    mq2 = "SELECT a.code,a.trannum,a.trandt, a.srno,a.rate,a.flag,a.amt,a.desc_ as item FROM wb_CORRCST_CONVC a WHERE trim(a.TRANNUM)='" + barCode + "'";
                }
                else
                {
                    mq2 = "SELECT a.code,a.trannum,a.trandt, a.srno,a.rate,a.flag,a.amt,a.desc_ as item FROM wb_CORRCST_CONVC a WHERE trim(a.TRANNUM)='" + mq0 + "'";
                }

                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                if (dt.Rows.Count > 0)
                {
                    #region

                    dr1 = dtm.NewRow();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i]["srno"].ToString() == "00")
                        {
                            dr1["gsm_0"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_0"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_0"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_0"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_0"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_0"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "01")
                        {
                            dr1["gsm_1"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_1"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_1"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_1"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_1"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_1"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "02")
                        {
                            dr1["gsm_2"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_2"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_2"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_2"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_2"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_2"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "03")
                        {
                            dr1["gsm_3"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_3"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_3"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_3"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_3"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_3"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "04")
                        {
                            dr1["gsm_4"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_4"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_4"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_4"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_4"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_4"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                    }
                    dtm.Rows.Add(dr1);
                    #endregion
                }
                if (dt2.Rows.Count > 0)
                {
                    #region
                    dr2 = ph_tbl.NewRow();
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        if (dt2.Rows[j]["srno"].ToString() == "00")
                        {
                            dr2["strch_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["strch_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["strch_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "01")
                        {
                            dr2["pva_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["pva_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["pva_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "02")
                        {
                            dr2["power_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["power_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["power_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "03")
                        {
                            dr2["fuel_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["fuel_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["fuel_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "04")
                        {
                            dr2["pins_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["pins_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["pins_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "05")
                        {
                            dr2["ink_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["ink_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["ink_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "06")
                        {
                            dr2["labr_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["labr_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["labr_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "07")
                        {
                            dr2["admin_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["admin_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["admin_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "08")
                        {
                            dr2["trans_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["trans_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["trans_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "09")
                        {
                            dr2["mat_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["mat_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["mat_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                    }
                    ph_tbl.Rows.Add(dr2);
                    #endregion
                }
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dtm.TableName = "layers";
                    dsRep.Tables.Add(dtm);
                    ph_tbl.TableName = "conversion_cost";
                    dsRep.Tables.Add(ph_tbl);
                    //  dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "box_dimsn", "box_dimsn", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;


            case "F10150":
            case "F10145": //for form
                xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                header_n = "CSBS Estimation";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = "";
                mq2 = "SELECT distinct boxtypecode ,flute,trim(imagepath) as fstr FROM  wb_corrcst_flutem  where branchcd !='DD' and trim(boxtypecode) !='-'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                if (iconID == "F10145")
                {
                    SQuery = "select '" + header_n + "' as header,a.* ,b.name as box_name from wb_corrcst_csbs a, wb_corrcst_flutem b where trim(a.boxtypecode)=trim(b.boxtypecode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + barCode + "'";
                }
                else
                {
                    SQuery = "select '" + header_n + "' as header,a.* from wb_corrcst_csbs a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + mq0 + "'";
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("mLogo", typeof(System.Byte[]));
                if (dt.Rows.Count > 0)
                {
                    mq1 = fgen.seek_iname_dt(dt1, " boxtypecode='" + dt.Rows[0]["boxtypecode"].ToString().Trim() + "'", "fstr");
                    if (mq1 != "")
                    {
                        try
                        {
                            fpath = mq1;
                            FilStr = new FileStream(fpath, FileMode.Open);
                            BinRed = new BinaryReader(FilStr);
                            dt.Rows[0]["mLogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                            FilStr.Close();
                            BinRed.Close();
                        }
                        catch { }
                    }
                    ////////////////
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "cal_req_comp", "cal_req_comp", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10256":
                header_n = "Costing Sheet";
                SQuery = "select '" + header_n + "' as header, a.* from wb_tran_cost a where a.branchcd||trim(a.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ")";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    pdfView = "Y";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "cost_print_SURY", "cost_print_SURY", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;
            case "F10185":
                header_n = "Pre Costing Report";
                //if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI" || frm_cocd == "MAYU" || frm_cocd == "KCLG" || frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "VPAC")
                {
                    SQuery = "Select a.*,(case when trim(nvl(b.INAME,'-'))='-' then a.t121 else b.INAME end) as INAME from (select a.*,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as aname from (Select * from somas_anx a where A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "') a left outer join famst b on trim(a.acode)=trim(b.acode)) a left outer join item b on trim(a.icode)=trim(b.icode)";
                }
                //else
                //{
                //    SQuery = "select a.*,b.aname,c.iname from somas_anx a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icodE)=trim(c.icode) and a.type='PN' and A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "'";
                //}
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);

                    frm_rptName = "cnitc";
                    if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI" || frm_cocd == "MAYU" || frm_cocd == "KCLG") frm_rptName = "csydb";
                    else if (frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "VPAC") frm_rptName = "costingbest";

                    Print_Report_BYDS(frm_cocd, frm_mbr, "cnitc", frm_rptName, dsRep, header_n, "Y");
                }
                break;
            case "F10134":
                #region laminate bom
                header_n = "Laminate Bom";
                mq10 = ""; dt = new DataTable();
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT '" + header_n + "' as header, a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as parent_icode,trim(b.iname) as p_iname,b.cpartno as prod_Code,c.irate,trim(a.icode) as child_icode,a.sampqty as qty,a.col1 as child_iname, nvl(a.qty1,0) as thick,nvl(a.qty2,0) as density,nvl(a.qty3,0) as gsm_wet,nvl(a.qty4,0) as solid,nvl(a.qty5,0) as gsm_Dry,nvl(a.qty6,0) as percentage,nvl(a.qty7,0) as grid_width,nvl(a.qty8,0) as grid_qty,a.srno,a.obsv1 as slit_reel_wdth,a.obsv2 as reel_weight,a.obsv3 as core_size_inch,a.obsv4 as core_type,a.obsv5 as pack_type,a.obsv6 as widht,a.obsv7 as trim_wstg,a.obsv8 as std_wstg,a.obsv9 as tot_wstg,a.obsv10 as sqm_lami,a.amdtno FROM INSPVCH A,ITEM B,item c WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(a.icode)=trim(c.icode) AND TRIM(A.BRANCHCD)='" + frm_mbr + "' and TRIM(A.TYPE)='" + frm_vty + "' and TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") order by a.srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Laminate_Bom", "std_Laminate_Bom", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F10134A":
                #region poly bom
                header_n = "Ploy Bom";
                mq10 = ""; dt = new DataTable();
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT  '" + header_n + "' as header, a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as parent_icode,trim(b.iname) as p_iname,b.cpartno as prod_Code,c.irate,trim(a.icode) as child_icode,a.sampqty as qty,a.col1 as child_iname, nvl(a.qty1,0) as thick,nvl(a.qty2,0) as density,nvl(a.qty3,0) as gsm_wet,nvl(a.qty4,0) as solid,nvl(a.qty5,0) as gsm_Dry,nvl(a.qty6,0) as percentage,nvl(a.qty7,0) as grid_width,nvl(a.qty8,0) as grid_qty,a.srno,a.obsv1 as slit_reel_wdth,a.obsv2 as reel_weight,a.obsv3 as core_size_inch,a.obsv4 as core_type,a.obsv5 as pack_type,a.obsv6 as widht,a.obsv7 as trim_wstg,a.obsv8 as std_wstg,a.obsv9 as tot_wstg,a.obsv10 as sqm_lami,a.amdtno FROM INSPVCH A,ITEM B,item c WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(a.icode)=trim(c.icode) AND TRIM(A.BRANCHCD)='" + frm_mbr + "' and TRIM(A.TYPE)='" + frm_vty + "' and TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") order by a.srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Poly_Bom", "std_Poly_Bom", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F10134B":
                #region pouch bom
                header_n = "Pouch-Bom";
                mq10 = ""; dt = new DataTable();
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT  '" + header_n + "' as header,a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as parent_icode,trim(b.iname) as p_iname,b.cpartno as prod_Code,c.irate,trim(a.icode) as child_icode,a.sampqty as qty,a.col1 as child_iname, nvl(a.qty1,0) as thick,nvl(a.qty2,0) as gsm,nvl(a.qty3,0) as p_length,nvl(a.qty4,0) as p_width,nvl(a.qty5,0) as p_area,nvl(a.qty6,0) as p_Area_s,nvl(a.qty7,0) as wights,nvl(a.qty8,0) as qty_lamk,a.srno,a.obsv1 as slit_reel_wdth,a.obsv2 as reel_weight,a.obsv3 as core_size_inch,a.obsv4 as core_type,a.obsv5 as pack_type,a.obsv6 as widht,a.obsv7 as trim_wstg,a.obsv8 as std_wstg,a.obsv9 as tot_wstg,a.obsv10 as sqm_lami,a.amdtno FROM INSPVCH A,ITEM B, item c WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(a.icode)=trim(c.icode) AND TRIM(A.BRANCHCD)='" + frm_mbr + "' and TRIM(A.TYPE)='" + frm_vty + "' and TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") order by a.srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pouch_Bom", "std_Pouch_Bom", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F10351":
            case "F10352":
            case "F10353":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Service Req Entry";
                if (iconID == "F10351")
                {
                    //SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,c.iname from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    frm_rptName = "serv_req_entry";
                }
                else if (iconID == "F10352")
                {
                    // SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname from wb_service a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,c.iname from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    frm_rptName = "serv_req_entry";
                }
                else
                {
                    //SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,(case when nvl(a.chk_by,'-')='-' then 'OPEN' ELSE 'CLOSE' END) AS status from wb_service a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,c.iname,(case when nvl(a.chk_by,'-')='-' then 'OPEN' ELSE 'CLOSE' END) AS status from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    frm_rptName = "serv_req_entry_eng";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "");
                }
                break;

            case "F10196": ///label costing mlab
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Label Costing";
                //SQuery = "select '" + header_n + "' as header, a.*,b.aname,c.iname from wb_CYLINDER a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                SQuery = "select '" + header_n + "' as header, a.* from wb_CYLINDER a where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";  //WITHOUT JOINING      
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "lbl_costing_MLAB", "lbl_costing_MLAB", dsRep, header_n);
                }
                break;

            case "F10199": //SPPI OFFSET LABEL COSTING PRINT             
            case "F10197": //SPPI LABEL COSTING PRINT             with cyl
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Offset-Label Costing";
                SQuery = "select '" + header_n + "' as header,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,is_number(a.t1) as lbl_wid,is_number(a.t2) as lbl_hyt,is_number(a.t3) as qty,is_number(a.t4) as acros,is_number(a.t5) as arnd,is_number(a.t6) as ups,is_number(a.t7) as actl_wid_matl,is_number(a.t8) as seting_wstg_color,is_number(a.t9) as rung_mtr_mtr,is_number(a.t10) as tot_sqm,is_number(a.t11) as passes,is_number(a.t12) as req_wid,is_number(a.t13) as color,is_number(a.t14) as tot_wstg,is_number(a.t15) as gap_Acros,is_number(a.t16) as gap_Arnd,is_number(a.t17) as diff,is_number(a.t18) as rung_mtr_mm,is_number(a.t19) as tot_rmtr_used,is_number(a.t20) as prod_cost,is_number(a.t21) as margin_percent,is_number(a.t22) as margin_aed,is_number(a.t23) as total,is_number(a.t24) as vat_percent,is_number(a.t25) as vat_val,is_number(a.t26) as gd_tot,a.t27,is_number(a.t28) as matl1_rate,a.t29,is_number(a.t30) as matl2_rate,a.t31,is_number(a.t32) as matl3_rate,a.t33,is_number(a.t34) as matl4_tot,a.t35,is_number(a.t36) as ink_rt,a.t37,a.t38,is_number(a.t39) as ink_cost,a.t40,is_number(a.t41) as plate_rt,is_number(a.t42) as plate_cost,a.t43,is_number(a.t44) as var_rt,is_number(a.t45) as t45, is_number(a.t46) as var_cost,a.t47,is_number(a.t48) as die_rt,is_number(a.t49) as t49,is_number(a.t50) as t50,is_number(a.t51) as t51,is_number(a.t52) as t52,is_number(a.t53) as die_cost,a.t54,is_number(a.t55) as emb_rt,is_number(a.t56) as t56,is_number(a.t57) as t57,is_number(a.t58) as t58,is_number(a.t59)  as t59,is_number(a.t60) as emb_cost,a.t61,is_number(a.t62) as emb_white_rt,is_number(a.t63) as t63,is_number(a.t64) as t64,is_number(a.t65) as t65,is_number(a.t66) as t66,is_number(a.t67) as emb_whitw_cost,a.t68,is_number(a.t69) as mach1_cost,a.t70,is_number(a.t71) as mch2_cost,is_number(a.t72) as t72,is_number(a.t73) as t73,is_number(a.t74) as t74,a.t75 as mcha_Code,a.t76 as mch2_code,is_number(a.t77) as t77,is_number(a.t78) as t78 , b.aname,c.iname from SOMAS_ANX a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Offset_lbl_costing_SPPI", "Offset_lbl_costing_SPPI", dsRep, header_n);
                }
                break;

            case "F10186C":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Detailed Flexible Costing";
                SQuery = "select '" + header_n + "' as header,a.vchnum as vch,to_char(a.vchdate,'dd/mm/yyyy') as vchd,A.* from wb_precost a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Pre_Cost_SPPI", "Pre_Cost_SPPI", dsRep, header_n);
                }
                break;
            case "F10135S":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                header_n = "SHADE / SPECIAL PRODUCT DEVLOPMENT REQUEST";
                SQuery = "Select '" + header_n + "' as header,A.ACODE,E.USERNAME,I.COL1 AS MASTER,a.branchcd,A.ebr,(CASE when nvl(A.PROD_cAT,'-')='LP' THEN '(Liquid Paint Division)' ELSE '(Powder Coating Division)' END) AS PROD_CATG, nvl(A.PROD_cAT,'LP') as PROD_cAT,nvl(A.PROD_NAME,'-') as PROD_NAME,nvl(A.HO_STATUS,'-') as HO_STATUS,A.COL4 AS MDNAME,A.col56,nvl(A.col57,'-') as col57, nvl(A.num1,0) as num1,nvl(A.num2,0) as num2,A.ENQ_STATUS,(CASE WHEN trim(NVL(A.col55,'0')) = '0' THeN 'Basic' else 'Selling' end ) as col55,(CASE WHEN trim(NVL(A.EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end ) as EMAIL_ID,(CASE WHEN trim(NVL(A.col26,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col26,(CASE WHEN trim(NVL(col23,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col23,(CASE WHEN trim(NVL(A.col24,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col24,A.invno,to_char(A.invdate,'dd/mm/yyyy') as invdate, NVL(A.COL51,'-') AS COL51,NVL(A.COL52,'0') AS COL52,NVL(A.COL53,'-') AS COL53,A.COL54,A.vchnum,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,A.col1,A.col2,A.col3,A.col21, A.col11,A.col5,A.col59 AS COL4,A.col6,A.col7,A.col8,A.col9,A.col10,A.col12,A.col27,A.col15,A.col18,nvl(A.col16,'-') as col16,nvl(A.col17,'-') as col17,nvl(A.col19,'-') as col19,A.col28,A.col22,A.col35,A.col37,A.col39,A.col25,A.col20,A.col13,A.col14,A.remarks,A.col40,NVL(A.col30,'-') AS COL30,NVL(A.col31,'-') AS COL31,NVL(A.col32,'-') AS COL32,NVL(A.col33,'-') AS COL33,NVL(A.col34,'-') AS col34,NVL(A.col36,'-') AS col36,NVL(A.col38,'-') AS col38,A.col41,A.col42,NVL(A.col43,'-') AS col43,TO_CHAR(NVL(A.docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(A.col44,'-') AS  col44,NVL(A.col45,'-') AS col45,NVL(A.col46,'-') AS col46,NVL(A.COL47,'-') AS col47,NVL(A.col48,'-') AS col48,NVL(A.col49,'-') AS col49,TO_CHAR(NVL(A.COL50,SYSDATE),'DD/MM/YYYY') as col50,A.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_dt,NVL(A.col60,'-') AS col60,NVL(A.col61,'-') AS col61,NVL(A.col62,'-') AS col62,NVL(A.col63,'-') AS col63,NVL(A.col64,'-') AS col64,NVL(A.col65,'-') AS col65,NVL(A.col66,'-') AS col66,NVL(A.col67,'-') AS col67,NVL(A.col68,'-') AS col68,NVL(A.col69,'-') AS col69, NVL(A.col70,'-') AS col70,NVL(A.col71,'-') AS col71,NVL(A.col72,'-') AS col72,NVL(A.col73,'-') AS col73,NVL(A.col74,'-') AS col74,NVL(A.col75,'-') AS col75,NVL(A.col76,'-') AS col76,NVL(A.col77,'-') AS col77,NVL(A.col78,'-') AS col78,NVL(A.col79,'-') AS col79,NVL(A.col80,'-') AS col80,NVL(A.col81,'-') AS col81,NVL(A.col82,'-') AS col82,NVL(A.col83,'-') AS col83,NVL(A.col84,'-') AS col84,NVL(A.col85,'-') AS col85,NVL(A.col86,'-') AS col86,NVL(A.col87,'-') AS col87,A.SDR_NO,TO_CHAR(A.SDR_DATE,'DD/MM/YYYY') AS SDR_DATE from EVAS E,scratch A left join inspmst i on trim(a.col30)=trim(i.acode) and i.type='SF' where TRIM(A.ACODE)=TRIM(E.USERID) AND A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_CHAr(A.vchdate,'DD/MM/YYYY') in '" + mq1 + "' order by a.col30";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SDR", "SDR", dsRep, header_n);
                }
                break;
            case "F10125":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                header_n = "SHADE / SPECIAL PRODUCT DEVLOPMENT REQUEST";
                SQuery = "Select * from typegrp where id='BN' and branchcd='" + frm_mbr + "' AND trim(VCHNUM)||to_Char(vchdate,'dd/mm/yyyy') in (" + mq1 + ") order by TYPE1";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dt = fgen.addBarCode(dt, "name", true);
                    dsRep.Tables.Add(dt);

                    Print_Report_BYDS(frm_cocd, frm_mbr, "BIN_Stkr", "BIN_Stkr", dsRep, header_n);
                }
                break;
        }
    }

    private void Print_Report_BYDS(string frm_cocd, string frm_mbr, string xmlname, string frm_rptName, DataSet dsRep, string title
        , string addlogo = "N")
    {
        Multiton multiton = Multiton.GetInstance(MyGuid);

        if (addlogo == "Y") dsRep.Tables.Add(fgen.Get_Type_Data(MyGuid, frm_cocd, Multiton.Get_Mvar(MyGuid, "U_MBR"), "Y"));
        else dsRep.Tables.Add(fgen.Get_Type_Data(MyGuid, frm_cocd, Multiton.Get_Mvar(MyGuid, "U_MBR")));
        Multiton.SetSession(MyGuid, "Data", null);
        Multiton.SetSession(MyGuid, "DataDS", dsRep);
        Multiton.SetSession(MyGuid, "Report", frm_rptName);
        Multiton.SetSession(MyGuid, "title", title);
        //PrintRptNew(title);
        //ShowRpt_xml();
    }

    public void open_report_byDs_ERP(string usercode, DataSet ds, string rptname, string title, bool addlogo)
    {
        Multiton multiton = Multiton.GetInstance(MyGuid);
        if (addlogo) ds.Tables.Add(fgen.Get_Type_Data(MyGuid, usercode, Multiton.Get_Mvar(MyGuid, "U_MBR"), "Y"));
        else ds.Tables.Add(fgen.Get_Type_Data(MyGuid, usercode, Multiton.Get_Mvar(MyGuid, "U_MBR")));
        Multiton.SetSession(MyGuid, "Data", null);
        Multiton.SetSession(MyGuid, "DataDS", ds);
        Multiton.SetSession(MyGuid, "Report", rptname);
        Multiton.SetSession(MyGuid, "title", title);
        //PrintRptNew(title);
        //ShowRpt_xml();

    }
    //public void PrintRptNew(string Title)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"]; 
    //    controller.ViewBag.scripCall += "showRptnew('" + Title + "');";
    //}
    public void del_file(string path)
    {
        try
        {
            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }

}

