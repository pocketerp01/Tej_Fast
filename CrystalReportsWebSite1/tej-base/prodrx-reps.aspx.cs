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

public partial class prodrx_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdRange1, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl, query1;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, pdfView = "", data_found = "", mq2 = "", mq8 = "";
    string party_cd, part_cd, header_n, cond = "", cond1 = "";
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    int i0 = 0, i1, i2, i3, i4;

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
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm, dtdrsim, dticode, mdt;
        double double1 = 0, double2 = 0, double3 = 0, double4 = 0;
        DataView view1im, view2, dv, dv1, dv2;
        DataRow mdr, dr1, oporow;
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
            //Mixing Ticket barcode

            case "F38501":
                #region
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                if (mq0 == "TB")
                {
                    header_n = "Printing Sub-Vessel Tag";
                }
                else if (mq0 == "GB")
                {
                    header_n = "Pigment Sub-Vessel Tag";
                }
                else if (mq0 == "MB")
                {
                    header_n = "Main Mixing (Return) Sub-Vessel Tag";
                }
                else if (mq0 == "MJ")
                {
                    header_n = "Vessel Tfr Sticker";
                }
                else
                {
                    header_n = "Main Mixing Sub-Vessel Tag";
                }
                /////////===========//=============                         
                frm_rptName = "QR_KLAS";
                if (mq0 == "MB" || mq0 == "MR" || mq0 == "MJ")
                {
                    SQuery = "select '" + header_n + "'||'-'||substr(c.name,5,50) as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode)||a.srno as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,b.maker as color,nvl(b.IWEIGHT,0) as gsm,nvl(trim(a.thru),'-') as vessel_no, to_number(d.acref) as vessel_weight,nvl(a.iqtyin,0) as qty,nvl(trim(a.invno),'-') as plan_no, to_char(invdate,'dd/mm/yyyy') as plan_dt,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,UPPER(A.BTCHNO) AS BTCHNO from  ivoucherw a LEFT JOIN TYPEGRP D ON SUBSTR(TRIM(A.THRU),1,3)=TRIM(D.TYPE1) AND D.ID='VS',item b,type c where  trim(a.icode)=trim(b.icode) and substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) ='" + mq1 + "' AND A.IQTYIN>0";
                }
                else
                {
                    frm_rptName = "QR_KLAS";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode)||a.srno as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,b.maker as color,nvl(b.IWEIGHT,0) as gsm,nvl(trim(a.thru),'-') as vessel_no,to_number(c.acref) as vessel_weight,nvl(a.iqtyin,0) as qty, nvl(trim(a.invno),'-') as plan_no, to_char(invdate,'dd/mm/yyyy') as plan_dt,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE from  ivoucherw a LEFT JOIN TYPEGRP C ON SUBSTR(TRIM(A.THRU),1,3)=TRIM(c.TYPE1) AND C.ID='VS' , item b   where  trim(a.icode)=trim(b.icode)   and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) ='" + mq1 + "'"; //after add vessel code
                }
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt = fgen.addBarCode(dt, "QR", true);
                double db = 0;
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    if (mq0 == "MB" || mq0 == "MR" || mq0 == "MJ")
                        SQuery = "SELECT distinct max(a.sampqty) AS QTY,'-----' as gsm,max(a.contplan) as plan_qty,a.wono,b.iname,b.maker as colour,A.ICODE FROM INSPVCH a,item b where trim(a.wono)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "'  AND a.BTCHNO='" + dt.Rows[0]["plan_no"].ToString().Trim() + "' and to_char(to_date(a.btchdt,'dd/MM/yyyy'),'dd/MM/yyyy')='" + dt.Rows[0]["plan_dt"].ToString().Trim() + "'  AND TRIM(A.OBSV16)='" + dt.Rows[0]["icode"].ToString().Trim() + "' group by a.wono,b.iname,b.maker,a.icode";
                    else SQuery = "SELECT distinct max(a.sampqty) AS QTY,'-----' as gsm,max(a.obsv5) as plan_qty,a.wono,b.iname,b.maker as colour,A.ICODE FROM INSPVCH a,item b where trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "'  AND a.BTCHNO='" + dt.Rows[0]["plan_no"].ToString().Trim() + "' and to_char(to_date(a.btchdt,'dd/MM/yyyy'),'dd/MM/yyyy')='" + dt.Rows[0]["plan_dt"].ToString().Trim() + "'  AND TRIM(A.icode)='" + dt.Rows[0]["icode"].ToString().Trim() + "' group by a.wono,b.iname,b.maker,a.icode";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        dt1.Rows[i]["gsm"] = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MAX(IBQTY)  AS BM_QTY  FROM ITEMOSP WHERE BRANCHCD!='DD'  AND TYPE='BM'  AND IBCODE='" + dt1.Rows[i]["icode"].ToString().Trim() + "'", "BM_QTY"));
                    }

                    dt1.TableName = "plndt";
                    dsRep.Tables.Add(fgen.mTitle(dt1, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "QR_KLAS", frm_rptName, dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;


            /////=====================kalssik reports
            //=====================1//=====================
            case "RPT9":
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED Machine
                mq10 = "PROD_SHEET";
                if (frm_formID == "15220C") mq10 = "PROD_SHEETK";
                xprdRange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";
                SQuery = " select 'Time Sheet for Machine : " + mq0 + "' as header,'" + fromdt + " ' as frmdt,'" + todt + "' as todt,BRANCHCD,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,iname,SHIFT,MACHINE,STIME,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,ETIME,PLANQTY,QTY,BOXES,TSLOT,EFF,enqno,enqdt,jroll,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime,opname from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll,opname FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,(CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end)  as jroll,trim(c.exc_time) as opname FROM ITEM A,PROD_SHEET C WHERE c.branchcd='" + frm_mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdRange + " GROUP BY  (CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT,trim(c.exc_time) UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll,null as opname	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + frm_mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdRange + ") A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll,opname FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,(CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end)as jroll,trim(c.exc_time) as opname FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + frm_mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdRange + " GROUP BY  (CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT,trim(c.exc_time) UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll ,null as opname	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + frm_mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdRange + " ) A )    ORDER BY VCHDATE,SHIFT,NSTIME";
                if (frm_formID == "15220C")
                    SQuery = " select 'Time Sheet for Machine : " + mq0 + "' as header,'" + fromdt + " ' as frmdt,'" + todt + "' as todt,BRANCHCD,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,iname,SHIFT,MACHINE,STIME,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,ETIME,PLANQTY,QTY,BOXES,TSLOT,EFF,enqno,enqdt,jroll,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime,opname from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll,opname FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,(CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end)  as jroll,trim(c.exc_time) as opname FROM ITEM A,PROD_SHEETk C WHERE c.branchcd='" + frm_mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdRange + " GROUP BY  (CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT,trim(c.exc_time) UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll,null as opname	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + frm_mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdRange + ") A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll,opname FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,(CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end)as jroll,trim(c.exc_time) as opname FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + frm_mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdRange + " GROUP BY  (CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT,trim(c.exc_time) UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll ,null as opname	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + frm_mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdRange + " ) A )    ORDER BY VCHDATE,SHIFT,NSTIME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "time_sheet_klas", "time_sheet_klas", dsRep, "");
                }
                break;







            //=====================2//=====================
            case "RPT11":
            case "RPT14":
                #region
                mq10 = "PROD_SHEETS";
                if (iconID == "RPT14") mq10 = "PROD_SHEETK";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                xprdRange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";
                SQuery = " select 'Time Sheet : " + mq1 + " ' as header,'" + fromdt + " ' as frmdt,'" + todt + "' as todt,BRANCHCD,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,iname,SHIFT,MACHINE,STIME,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,ETIME,PLANQTY,QTY,BOXES,TSLOT,EFF,enqno,enqdt,jroll,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime,opname from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll,opname FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,(CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end)  as jroll,trim(c.exc_time) as opname FROM ITEM A," + mq10 + " C WHERE c.branchcd='" + frm_mbr + "' and TRIM(C.acode) in (" + hfval.Value + ") and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdRange + " and TRIM(C.ENAME)='" + mq1 + "' GROUP BY  (CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT,trim(c.exc_time) UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll,null as opname	 FROM  INSPVCHS a WHERE  a.TYPE='58' and a.branchcd='" + frm_mbr + "' and TRIM(a.acode) in (" + hfval.Value + ") and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdRange + ") A  )    ORDER BY VCHDATE,SHIFT,NSTIME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "time_sheet_klas_s", "time_sheet_klas_s", dsRep, "");
                }
                #endregion
                break;
            ////===========//===========//===========//===========//===================



            //=====================3//=====================
            case "RPT12":
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED Machine                
                mq10 = "PROD_SHEETK";
                xprdRange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";
                SQuery = " select 'Time Sheet for Machine : " + mq0 + "' as header,'" + fromdt + " ' as frmdt,'" + todt + "' as todt,BRANCHCD,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,iname,SHIFT,MACHINE,STIME,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,ETIME,PLANQTY,QTY,BOXES,TSLOT,EFF,enqno,enqdt,jroll,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime,opname from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll,opname FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,(CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end)  as jroll,trim(c.exc_time) as opname FROM ITEM A,PROD_SHEETk C WHERE c.branchcd='" + frm_mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdRange + " GROUP BY  (CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT,trim(c.exc_time) UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll,null as opname	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + frm_mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdRange + ") A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll,opname FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,(CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end)as jroll,trim(c.exc_time) as opname FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + frm_mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdRange + " GROUP BY  (CASE when length(trim(c.remarks2))=11 then trim(substr(c.remarks2,1,9)) else  trim(substr(c.remarks2,1,10)) end),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT,trim(c.exc_time) UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll ,null as opname	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + frm_mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdRange + " ) A )    ORDER BY VCHDATE,SHIFT,NSTIME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "time_sheet_klas", "time_sheet_klas", dsRep, "");
                }
                break;
            ////===========//===========//====================



            //=====================4//=====================
            case "RPT15":
                #region jubmo roll details
                string value1 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED
                //if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
                //else branch_Cd = "branchcd='" + mbr + "'";
                frm_cDt1 = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(fmdate,'dd/mm/yyyy') as fromdt from co where code='" + frm_cocd + frm_myear + "'", "fromdt");
                frm_cDt2 = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(todate,'dd/mm/yyyy') as todate from co where code='" + frm_cocd + frm_myear + "'", "todate");
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
                mq2 = "SELECT A.ACODE,F.ANAME,A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT FROM SOMAS A,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A.branchcd='" + frm_mbr + "' AND A.TYPE LIKE'4%' AND ORDDT BETWEEN TO_DATE('01/04/2017','DD/MM/YYYY') AND TO_DATE('" + frm_cDt2 + "','DD/MM/YYYY')";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                mq0 = "SELECT TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||A.COL6 AS GRP,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ENQNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS ENQDT,SUM(is_number(A.COL3)) AS WEIGHT,SUM(is_number(A.COL4)) AS MTR,A.COL6 AS ROLL,A.SUPCL_BY AS MAC,A.COL23 AS SHIFT,I.INAME,I.WT_RR,I.MAKER,a.icode FROM COSTESTIMATE A,ITEM I  WHERE TRIM(A.ACODE)=TRIM(I.ICODE) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='40' and a.vchdate > to_Date('01/04/2008','dd/mm/yyyy') AND A.COL6 IN( '" + mq0.Trim() + "') GROUP BY TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),A.ACODE,A.ENQNO,A.ENQDT,A.COL6,A.SUPCL_BY,A.COL23,I.INAME,I.WT_RR,I.MAKER,a.icode ORDER BY VCHDATE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count < 1)
                {
                    mq0 = "SELECT TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||A.COL6 AS GRP,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ENQNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS ENQDT,SUM(is_number(A.COL3)) AS WEIGHT,SUM(is_number(A.COL4)) AS MTR,A.COL6 AS ROLL,A.SUPCL_BY AS MAC,A.COL23 AS SHIFT,I.INAME,I.WT_RR,I.MAKER,a.icode FROM COSTESTIMATEK A,ITEM I  WHERE TRIM(A.ACODE)=TRIM(I.ICODE) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='40' and a.vchdate > to_Date('01/04/2008','dd/mm/yyyy') AND A.COL6 IN( '" + mq0.Trim() + "') GROUP BY TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),A.ACODE,A.ENQNO,A.ENQDT,A.COL6,A.SUPCL_BY,A.COL23,I.INAME,I.WT_RR,I.MAKER,a.icode ORDER BY VCHDATE";
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
                dr1 = null; i0 = 0;
                if (dt.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dt);
                    dtdrsim = new DataTable();
                    dtdrsim = view1.ToTable(true, "VCHDATE", "ACODE", "ROLL");
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        view1im = new DataView(dt, "VCHDATE='" + dr0["VCHDATE"].ToString().Trim() + "' AND ACODE='" + dr0["ACODE"].ToString().Trim() + "' AND ROLL='" + dr0["ROLL"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dticode = new DataTable();
                        dticode = view1im.ToTable();
                        DataTable dticode2 = new DataTable();
                        if (dt1.Rows.Count > 0)
                        {
                            view2 = new DataView(dt1, "ICODE='" + dr0["ACODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
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
                                    value1 = dticode.Rows[i]["VCHDATE"].ToString().Trim() + dr0["ACODE"].ToString().Trim() + dticode.Rows[i]["ROLL"].ToString().Trim();
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

                                    value1 = dticode.Rows[i]["VCHDATE"].ToString().Trim() + dr0["ACODE"].ToString().Trim() + dticode.Rows[i]["ROLL"].ToString().Trim();
                                }
                            }
                        }
                    }
                    fpath = Server.MapPath(@"~\tej-base\BarCode\KLAS_STK_" + i0 + ".png");
                    del_file(fpath);
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
                if (dtm.Rows.Count > 0)
                {
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "crptKlasJumboRoll", "crptKlasJumboRoll", dsRep, "");
                }
                #endregion
                break;

            ////===========//===========//===========//===========//===================





            //=====================5//=====================
            case "RPT18"://need to check,,,data not coming in exe also..matching pending
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "Select  A.SRNO,a.branchcd,a.type,a.vchnum,a.vchdate,a.mrrdate,A.QTY1,a.mrrnum,a.acode,b.aname,a.icode,a.btchno,a.col1,a.col2,a.col3,a.col4,a.col5,a.obsv1,a.obsv2,a.obsv3,a.obsv4,a.obsv5,a.obsv16,a.ent_by from multivch a ,famst b WHERE trim(a.acode)=trim(b.acode) and a.BRANCHCD||a.TYPE||a.VCHNUM||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') in (" + mq0.Trim() + ") ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "fab_insp", "fab_insp", dsRep, "");
                }
                break;
            ////===========//===========//===========//===========//===================




            //=====================6//=====================
            case "RPT21"://invalid relational operatoor error coming...need to take qry from sir
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                xprdRange1 = "";//null isley kiya qki main code mebnullpas  ki hui h
                SQuery = "Select jrno,QUALITY,SUM(JRQTY) AS JRQTY,SUM(TOTAL) AS TOTAL,SUM(STD) AS STD,(case when SUM(TOTAL)>0 and sum(std)>0 then ROUND(SUM(std)/SUM(TOTAL)*100,2) else 0 end) AS STD_PER,SUM(COM) AS COM,(case when SUM(TOTAL)>0 and sum(com)>0 then ROUND(SUM(com)/SUM(TOTAL)*100,2) else 0 end) AS COM_PER,SUM(NS) AS NS,(case when SUM(TOTAL)>0 and sum(NS)>0 then ROUND(SUM(NS)/SUM(TOTAL)*100,2) else 0 end) AS NS_PER,SUM(SS) AS SS,(case when SUM(TOTAL)>0 and sum(SS)>0 then ROUND(SUM(SS)/SUM(TOTAL)*100,2) else 0 end) AS SS_PER,SUM(SL) AS SL,(case when SUM(TOTAL)>0 and sum(SL)>0 then ROUND(SUM(SL)/SUM(TOTAL)*100,2) else 0 end) AS SL_PER,SUM(SAM) AS SAM,(case when SUM(TOTAL)>0 and sum(SAM)>0 then ROUND(SUM(SAM)/SUM(TOTAL)*100,2) else 0 end) AS SAM_PER,SUM(CUT) AS CUT,(case when SUM(TOTAL)>0 and sum(CUT)>0 then ROUND(SUM(CUT)/SUM(TOTAL)*100,2) else 0 end) AS CUT_PER,sum(insproll) as insproll,sum(undpr) as undpr FROM (SELECT a.jrno,TRIM((CASE WHEN NVL(TRIM(B.CPARTNO),'-')='-' THEN NULL ELSE B.CPARTNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.CDRGNO),'-')='-' THEN NULL ELSE B.CDRGNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.BINNO),'-')='-' THEN NULL ELSE B.BINNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.SALLOY),'-')='-' THEN NULL ELSE B.SALLOY END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.NO_PROC),'-')='-' THEN NULL ELSE B.NO_PROC END)) AS QUALITY,0 AS JRQTY,ROUND(SUM(A.STD)+SUM(A.COM)+SUM(A.NS)+SUM(A.SS)+SUM(A.SL)+SUM(A.SAM)+SUM(A.CUT)) AS TOTAL,SUM(A.STD) AS STD,SUM(A.COM) AS COM,SUM(A.NS) AS NS,SUM(A.SS) AS SS,SUM(A.SL) AS SL,SUM(A.SAM) AS SAM,SUM(A.CUT) AS CUT,(a.insproll) as insproll,0 as undpr FROM (SELECT TRIM(A.ICODE) AS ICODE,DECODE(TRIM(UPPER(A.DESC_)),'IST/STD',SUM(A.QTY),0) AS STD,DECODE(TRIM(UPPER(A.DESC_)),'COM',SUM(A.QTY),0) AS COM,DECODE(TRIM(UPPER(A.DESC_)),'NS',SUM(A.QTY),0) AS NS,DECODE(TRIM(UPPER(A.DESC_)),'SS',SUM(A.QTY),0) AS SS,DECODE(TRIM(UPPER(A.DESC_)),'SL',SUM(A.QTY),0) AS SL,sum(a.sqty)AS SAM,SUM(A.cqty) AS CUT,(a.insproll) as insproll,jrno FROM (SELECT trim(a.btchno) as jrno,(b.qty) as insproll,sum(a.iqtyin) as qty,sum(nvl(a.st_nmodv,0)) as sqty,sum(nvl(a.et_paid,0)) as cqty,trim(a.icodE) as icode,(CASE WHEN TRIM(UPPER(A.DESC_))='STD' THEN 'IST/STD' WHEN TRIM(UPPER(A.DESC_))='COM' THEN 'COM' WHEN TRIM(UPPER(A.DESC_))='NS' THEN 'NS' WHEN TRIM(upper(A.DESC_))='SL' THEN 'SL' WHEN TRIM(upper(A.DESC_))='SS' THEN 'SS' end) AS DESC_ FROM IVOUCHER A,costestimate b WHERE a.branchcd||trim(a.btchno)=b.branchcd||trim(b.col6) and A.branchcd='" + frm_mbr + "' AND A.TYPE='16' and b.type='40' " + xprdRange1 + " and a.store='Y' and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + mq0 + "') GROUP BY TRIM(A.ICODE),TRIM(UPPER(A.DESC_)),b.qty,trim(a.btchno) ) A group BY TRIM(UPPER(A.DESC_)),TRIM(A.ICODe),(a.insproll),jrno) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) group BY a.jrno,(a.insproll),TRIM((CASE WHEN NVL(TRIM(B.CPARTNO),'-')='-' THEN NULL ELSE B.CPARTNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.CDRGNO),'-')='-' THEN NULL ELSE B.CDRGNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.BINNO),'-')='-' THEN NULL ELSE B.BINNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.SALLOY),'-')='-' THEN NULL ELSE B.SALLOY END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.NO_PROC),'-')='-' THEN NULL ELSE B.NO_PROC END)) UNION ALL SELECT null as jrno,TRIM((CASE WHEN NVL(TRIM(B.CPARTNO),'-')='-' THEN NULL ELSE B.CPARTNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.CDRGNO),'-')='-' THEN NULL ELSE B.CDRGNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.BINNO),'-')='-' THEN NULL ELSE B.BINNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.SALLOY),'-')='-' THEN NULL ELSE B.SALLOY END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.NO_PROC),'-')='-' THEN NULL ELSE B.NO_PROC END)) AS QUALITY,SUM(A.QTY) AS JRQTY,0 AS TOTAL,0 AS STD,0 AS COM,0 AS NS,0 AS SS,0 AS sl,0 AS SAM,0 AS CUT,0 as insproll,0 as undpr FROM COSTESTIMATE A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='40' " + xprdRange1 + " GROUP BY TRIM((CASE WHEN NVL(TRIM(B.CPARTNO),'-')='-' THEN NULL ELSE B.CPARTNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.CDRGNO),'-')='-' THEN NULL ELSE B.CDRGNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.BINNO),'-')='-' THEN NULL ELSE B.BINNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.SALLOY),'-')='-' THEN NULL ELSE B.SALLOY END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.NO_PROC),'-')='-' THEN NULL ELSE B.NO_PROC END)) union all SELECT null as jrno,TRIM((CASE WHEN NVL(TRIM(B.CPARTNO),'-')='-' THEN NULL ELSE B.CPARTNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.CDRGNO),'-')='-' THEN NULL ELSE B.CDRGNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.BINNO),'-')='-' THEN NULL ELSE B.BINNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.SALLOY),'-')='-' THEN NULL ELSE B.SALLOY END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.NO_PROC),'-')='-' THEN NULL ELSE B.NO_PROC END)) AS QUALITY,0 AS JRQTY,0 AS TOTAL,0 AS STD,0 AS COM,0 AS NS,0 AS SS,0 AS sl,0 AS SAM,0 AS CUT,0 as insproll,SUM(A.QTY) as undpr FROM COSTESTIMATE A,ITEM B,ivoucher c WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and C.branchcd||trim(c.btchno)=A.branchcd||trim(a.col6) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='40' and c.type='16' " + xprdRange1 + " GROUP BY TRIM((CASE WHEN NVL(TRIM(B.CPARTNO),'-')='-' THEN NULL ELSE B.CPARTNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.CDRGNO),'-')='-' THEN NULL ELSE B.CDRGNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.BINNO),'-')='-' THEN NULL ELSE B.BINNO END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.SALLOY),'-')='-' THEN NULL ELSE B.SALLOY END))||'/'||TRIM((CASE WHEN NVL(TRIM(B.NO_PROC),'-')='-' THEN NULL ELSE B.NO_PROC END)) ) GROUP BY QUALITY,jrno having sum(insproll)>0 ORDER BY JRQTY desc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "yld_rptjr", "yld_rptjr", dsRep, "");
                }
                break;
            ////===========//===========//===========//===========//===================






            //=====================7//=====================
            case "RPT22":
                #region Already Running Code . Commented on 17 Sept 2018
                //xprdrange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";

                //SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,b.VCHDATE,substr(trim(A.BTCHNO),0,9) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a,(SELECT  BRANCHCD,VCHDATE,TRIM(ICODE) AS ICODE,SUBSTR(TRIM(COL6),0,9) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),SUBSTR(TRIM(COL6),0,9),VCHDATE) b,item c where TRIM(A.BRANCHCD)||TRIM(A.ICODE)||SUBSTR(TRIM(A.BTCHNO),0,9)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) and trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),b.vchdate,substr(trim(A.BTCHNO),0,9),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) ,c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt ";
                //dt = new DataTable();
                //dt = fgen.getdata(co_cd, SQuery);

                //dt2 = new DataTable();
                //dt2 = fgen.getdata(co_cd, "SELECT branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy') as fstr,NVL(TRIM(col1),'-') AS COL1,sum(qty) as qty FROM costestimate WHERE " + branch_Cd + " and TYPE='RR' and vchdate " + xprdrange + " group by NVL(TRIM(col1),'-'),branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy') order by branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')");

                //dv = new DataView(dt);
                //dv.Sort = "btchno,icode";

                //dt1 = new DataTable();
                //dt1 = dv.ToTable(true, "btchno", "icode");

                //mdt = new DataTable();
                //mdt = dt.Clone();
                //mdt.Columns.Add(new DataColumn("purpose", typeof(string)));

                //oporow = null;
                //foreach (DataRow dr1 in dt1.Rows)
                //{
                //    if (dr1["btchno"].ToString().Trim() == "M2/600249") { }
                //    dv1 = new DataView(dt, "btchno='" + dr1["btchno"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' ", "btchno", DataViewRowState.CurrentRows);
                //    if (dv1.Count > 0)
                //    {
                //        oporow = mdt.NewRow();
                //        oporow["fromdt"] = dv1[0].Row["fromdt"].ToString().Trim();
                //        oporow["todate"] = dv1[0].Row["todate"].ToString().Trim();

                //        oporow["icode"] = dv1[0].Row["icode"].ToString().Trim();
                //        oporow["iname"] = dv1[0].Row["iname"].ToString().Trim();
                //        oporow["cpartno"] = dv1[0].Row["cpartno"].ToString().Trim();
                //        oporow["unit"] = dv1[0].Row["unit"].ToString().Trim();
                //        oporow["specs"] = dv1[0].Row["specs"].ToString().Trim();
                //        oporow["color"] = dv1[0].Row["color"].ToString().Trim();
                //        oporow["width"] = dv1[0].Row["width"].ToString().Trim();
                //        oporow["thick"] = dv1[0].Row["thick"].ToString().Trim();
                //        oporow["vchdate"] = dv1[0].Row["vchdate"].ToString().Trim();
                //        oporow["btchno"] = dv1[0].Row["btchno"].ToString().Trim();

                //        double1 = 0; double2 = 0; double3 = 0; double4 = 0;
                //        mq0 = ""; mq10 = "";
                //        for (int i = 0; i < dv1.Count; i++)
                //        {
                //            double1 += fgen.make_double(dv1[i].Row["sampl"].ToString().Trim());
                //            double2 += fgen.make_double(dv1[i].Row["fst"].ToString().Trim());
                //            double3 += fgen.make_double(dv1[i].Row["sec"].ToString().Trim());
                //            double4 += fgen.make_double(dv1[i].Row["ns"].ToString().Trim());

                //            mq10 = dv1[i].Row["fstr"].ToString().Trim();
                //        }

                //        dv2 = new DataView(dt2, "fstr='" + mq10 + "'", "fstr", DataViewRowState.CurrentRows);
                //        dv2.Sort = "col1";
                //        mq2 = "";
                //        for (int x = 0; x < dv2.Count; x++)
                //        {
                //            if (fgen.make_double(dv2[x].Row["qty"].ToString().Trim()) > 0)
                //            {
                //                if (mq0.Length > 0)
                //                {
                //                    mq0 = mq0 + ", " + dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                //                }
                //                else mq0 = dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                //            }
                //        }

                //        oporow["sampl"] = double1;
                //        oporow["fst"] = double2;
                //        oporow["sec"] = double3;
                //        oporow["ns"] = double4;
                //        oporow["purpose"] = mq0;

                //        oporow["qty"] = dv1[0].Row["qty"].ToString().Trim();

                //        mdt.Rows.Add(oporow);
                //    }
                //}
                //fgen.Print_Report_BYDT(co_cd, mbr, "yld_klas_dwise", "yld_klas_dwise", mdt);
                ////fgen.Print_Report(co_cd, mbr, SQuery, "yld_klas_dwise", "yld_klas_dwise");
                #endregion
                #region
                xprdRange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";

                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,b.VCHDATE,substr(trim(A.BTCHNO),0,9) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a,(SELECT  BRANCHCD,VCHDATE,TRIM(ICODE) AS ICODE,SUBSTR(TRIM(COL6),0,9) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE branchcd='" + frm_mbr + "' AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),SUBSTR(TRIM(COL6),0,9),VCHDATE) b,item c where TRIM(A.BRANCHCD)||TRIM(A.ICODE)||SUBSTR(TRIM(A.BTCHNO),0,9)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) and trim(a.icode)=trim(c.icode) and A.branchcd='" + frm_mbr + "' AND A.TYPE='16' and A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by trim(a.icode),b.vchdate,substr(trim(A.BTCHNO),0,9),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) ,c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt "; //old in this btchno only 9 digit
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,b.VCHDATE,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,VCHDATE,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE branchcd='" + frm_mbr + "' AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6),VCHDATE) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A.branchcd='" + frm_mbr + "' AND A.TYPE='16' and A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by trim(a.icode),b.vchdate,trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) ,c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt ";  //Isme ivch se left join kiya h costestimate ko...qhi ivch me btchno aara tha but costesmate me ni tha...so query me b ni aara tha
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE branchcd='" + frm_mbr + "' AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6)) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A.branchcd='" + frm_mbr + "' AND A.TYPE='16' and A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) ,c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt ";  ////isme vchdate ivoucher se pic ki hai qki left join krne par vchdate cost table se blank aari thi
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(IS_NUMBER(a.idiamtr)) + sum(IS_NUMBER(a.rej_sdp)) + SUM(IS_NUMBER(NVL(B.SCRP2,'0')))  as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT a.BRANCHCD,TRIM(a.ICODE) AS ICODE,TRIM(a.COL6) AS COL6,a.QTY AS QTY,SUM(b.SCRP2) AS SCRP2 FROM costestimate a,costestimate b WHERE a.branchcd||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)=b.branchcd||trim(B.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode) and a.branchcd='" + frm_mbr + "' AND a.TYPE='40' AND B.TYPE='25' GROUP BY a.BRANCHCD,TRIM(a.ICODE),TRIM(a.COL6),a.QTY) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A.branchcd='" + frm_mbr + "' AND A.TYPE='16' and A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) ,c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy') as fstr,NVL(TRIM(col1),'-') AS COL1,sum(qty) as qty FROM costestimate WHERE branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + " group by NVL(TRIM(col1),'-'),branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy') order by branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')");

                dv = new DataView(dt);
                dv.Sort = "btchno,icode";

                dt1 = new DataTable();
                dt1 = dv.ToTable(true, "btchno", "icode");

                mdt = new DataTable();
                mdt = dt.Clone();
                mdt.Columns.Add(new DataColumn("purpose", typeof(string)));

                oporow = null;
                foreach (DataRow dr3 in dt1.Rows)
                {
                    if (dr3["btchno"].ToString().Trim() == "M2/600249") { }
                    dv1 = new DataView(dt, "btchno='" + dr3["btchno"].ToString().Trim() + "' and icode='" + dr3["icode"].ToString().Trim() + "' ", "btchno", DataViewRowState.CurrentRows);
                    if (dv1.Count > 0)
                    {
                        oporow = mdt.NewRow();
                        oporow["fromdt"] = dv1[0].Row["fromdt"].ToString().Trim();
                        oporow["todate"] = dv1[0].Row["todate"].ToString().Trim();

                        oporow["icode"] = dv1[0].Row["icode"].ToString().Trim();
                        oporow["iname"] = dv1[0].Row["iname"].ToString().Trim();
                        oporow["cpartno"] = dv1[0].Row["cpartno"].ToString().Trim();
                        oporow["unit"] = dv1[0].Row["unit"].ToString().Trim();
                        oporow["specs"] = dv1[0].Row["specs"].ToString().Trim();
                        oporow["color"] = dv1[0].Row["color"].ToString().Trim();
                        oporow["width"] = dv1[0].Row["width"].ToString().Trim();
                        oporow["thick"] = dv1[0].Row["thick"].ToString().Trim();
                        oporow["vchdate"] = dv1[0].Row["vchdate"].ToString().Trim();
                        oporow["btchno"] = dv1[0].Row["btchno"].ToString().Trim();

                        double1 = 0; double2 = 0; double3 = 0; double4 = 0;
                        mq0 = ""; mq10 = "";
                        for (int i = 0; i < dv1.Count; i++)
                        {
                            double1 += fgen.make_double(dv1[i].Row["sampl"].ToString().Trim());
                            double2 += fgen.make_double(dv1[i].Row["fst"].ToString().Trim());
                            double3 += fgen.make_double(dv1[i].Row["sec"].ToString().Trim());
                            double4 += fgen.make_double(dv1[i].Row["ns"].ToString().Trim());
                            mq10 = dv1[i].Row["fstr"].ToString().Trim();
                        }

                        dv2 = new DataView(dt2, "fstr='" + mq10 + "'", "fstr", DataViewRowState.CurrentRows);
                        dv2.Sort = "col1";
                        mq2 = "";
                        for (int x = 0; x < dv2.Count; x++)
                        {
                            if (fgen.make_double(dv2[x].Row["qty"].ToString().Trim()) > 0)
                            {
                                if (mq0.Length > 0)
                                {
                                    mq0 = mq0 + ", " + dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                                }
                                else mq0 = dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                            }
                        }
                        oporow["sampl"] = double1;
                        oporow["fst"] = double2;
                        oporow["sec"] = double3;
                        oporow["ns"] = double4;
                        oporow["purpose"] = mq0;
                        if (dv1[0].Row["qty"].ToString().Trim() == "0" || dv1[0].Row["qty"].ToString().Trim() == "")
                        {
                            oporow["qty"] = "0";
                        }
                        else
                        {
                            oporow["qty"] = dv1[0].Row["qty"].ToString().Trim();
                        }
                        mdt.Rows.Add(oporow);
                    }
                }
                if (mdt.Rows.Count > 0)
                {
                    mdt.TableName = "Prepcur";
                    dsRep.Tables.Add(mdt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "yld_klas_dwise", "yld_klas_dwise", dsRep, "");
                }
                #endregion
                break;
            ////===========//===========//===========//===========//===================









            //=====================8//=====================
            case "RPT23":
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT  '" + fromdt + "' as fromdt,'" + todt + "' as todt, to_char(a.vchdate,'yyyymmdd') as vdd,a.vchdate,A.ENT_BY,A.STORE_NO,A.ICODE,B.INAME,trim(B.CPARTNO) as CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY) AS SPECS,B.MAKER AS COLOR,B.WT_NET AS WIDTH,B.WT_RR AS THICK,A.BTCHNO AS BTCHNO,A.O_DEPTT,SUM(A.FST) AS FST,SUM(A.SEC) AS SEC,SUM(A.NS) AS NS,A.QTY AS QTY,sum(a.sampl) as sampl,rtrim(xmlagg(xmlelement(e,replace(a.SEC_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') SEC_RMK,rtrim(xmlagg(xmlelement(e,replace(a.ns_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') NS_RMK,a.purpose FROM (SELECT (case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.VCHDATE,A.BTCHNO as BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,B.QTY as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimate where branchcd='" + frm_mbr + "' and TYPE='RR' group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATE B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='16' AND B.TYPE='40' group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,A.BTCHNO,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE,B.QTY) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and TRIM(substr(A.BTCHNO,1,9)) in (" + mq0 + ") GROUP BY A.ENT_BY,A.STORE_NO,A.ICODE,A.BTCHNO,A.O_DEPTT,B.INAME,B.CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY),B.MAKER,B.WT_NET,B.WT_RR,a.vchdate,a.purpose,A.QTY order by a.o_deptt,vdd,A.BTCHNO,a.icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "yld_klas_jrwise", "yld_klas_jrwise", dsRep, "");
                }
                break;
            ////===========//===========//===========//===========//===================





            //=====================9//=====================				
            case "RPT28":
                cond = ""; cond1 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (mq0 != "" && mq0 != "-") cond = " and b.acode IN (" + mq0 + ")";
                if (mq1 != "" && mq1 != "-") cond1 = " and b.bssch IN (" + mq1 + ")";
                string xxprd = " between to_Date('01/01/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') ";
                SQuery = "select C.Iname as Product,sum(d.Qlty1) as Gr_1,sum(d.Qlty2) as Gr_1B,sum(D.Qlty3) as Gr_ns,sum(d.Qlty1)+sum(d.Qlty2)+sum(D.Qlty3) as Grade_Tot,C.MAker as Color,C.Cpartno,D.Icode from (select decode(trim(b.desc_),'1',sum(bal),0) as Qlty1,decode(trim(b.desc_),'1B',sum(bal),0) as Qlty2,decode(trim(b.desc_),'NS',sum(bal),0) as Qlty3,A.icode from (select trim(icode)as icode,trim(invno) as Roll_no,sum(iqtyin)-sum(outq) as bal from (Select icode,invno,iqtyin,0 as outq From ivoucher where branchcd='" + frm_mbr + "' and type='16'  union all Select icode,no_bdls,0 as iqtyin,qtysupp From despatch where branchcd='" + frm_mbr + "' and substr(type,1,1)='4'  ) group by trim(icode),trim(invno) ) a, ivoucher b where b.branchcd='" + frm_mbr + "' and b.type='16' and trim(b.icode)||trim(b.invno)=trim(a.icode)||trim(a.roll_no) group by a.icode,b.desc_)d , item c where trim(d.icode)=trim(c.icode) group by c.Iname,c.Cpartno,D.icode,C.Maker ";
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view gr_w_stk as (" + SQuery + ")");

                query1 = "select branchcd,type,ciname,cpartno,pordno,porddt,acode,icode,ordno,orddt,qtyord,0 as sale,upper(packinst) as packinst,cu_chldt from somas where branchcd='" + frm_mbr + "' and type like '4%' and TYPE!='4A' and TYPE!='4C' AND  orddt " + xxprd + " AND TRIM(NVL(ICAT,'-'))<>'Y' AND TRIM(NVL(APP_BY,'-'))<>'-' and trim(desc_)='1' union all select branchcd,type,null as ciname,null as cpartno,null as pordno,null as porddt,acode,icode,ponum,podate,0 as qtyord,iqtyout as sale,null as packinst,null as cu_Chldt from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and TYPE!='4A' and TYPE!='4C' AND vchdate " + xxprd + " and store='Y' ";
                query1 = "select b.Aname,s.gr_1 as stock,max(nvl(A.ciname,'-')) as cINAME,max(nvl(A.cu_Chldt,a.porddt)) as cu_Chldt,(case when max(trim(nvl(A.packinst,'-')))='-' then 'Other Pending Orders' else max(trim(nvl(A.packinst,'-'))) end ) as packinst,sum(a.qtyord) as qtyord,sum(a.sale) as qty_out,sum(a.qtyord)-sum(a.sale) as bal,c.Unit,max(nvl(a.cpartno,'-')) as Part_no,max(nvl(a.pordno,'-')) as PO_NO,max(a.porddt) as PO_DT,a.ordno,to_char(a.orddt,'dd/MM/yyyy') as orddt,trim(a.acode) as Acode,trim(a.icode) as Icode,a.type,a.branchcd from (" + query1 + ")a LEFT OUTER JOIN gr_w_stk S ON trim(a.icode)=trim(s.icode),famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.Icode) " + cond + " " + cond1 + " group by a.branchcd,a.type,b.aname,c.unit,c.iname,trim(a.acode),trim(a.icode),a.ordno,a.orddt,s.gr_1 having sum(a.qtyord)-sum(a.sale)>0 order by A.ORDNO,B.aname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, query1);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Crpt_KLAS", "Crpt_KLAS", dsRep, "");
                }
                break;
            ////===========//===========//================////===========//===========//===========//=============				




            //=====================10//=====================
            case "RPT29":
                cond = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (hfval.Value != "" && hfval.Value != "-") cond = " and b.acode IN (" + hfval.Value + ")";
                xxprd = " between to_Date('01/01/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') ";
                SQuery = "select C.Iname as Product,sum(d.Qlty1) as Gr_1,sum(d.Qlty2) as Gr_1B,sum(D.Qlty3) as Gr_ns,sum(d.Qlty1)+sum(d.Qlty2)+sum(D.Qlty3) as Grade_Tot,C.MAker as Color,C.Cpartno,D.Icode from (select decode(trim(b.desc_),'1',sum(bal),0) as Qlty1,decode(trim(b.desc_),'1B',sum(bal),0) as Qlty2,decode(trim(b.desc_),'NS',sum(bal),0) as Qlty3,A.icode from (select trim(icode)as icode,trim(invno) as Roll_no,sum(iqtyin)-sum(outq) as bal from (Select icode,invno,iqtyin,0 as outq From ivoucher where branchcd='" + frm_mbr + "' and type='16'  union all Select icode,no_bdls,0 as iqtyin,qtysupp From despatch where branchcd='" + frm_mbr + "' and substr(type,1,1)='4'  ) group by trim(icode),trim(invno) ) a, ivoucher b where b.branchcd='" + frm_mbr + "' and b.type='16' and trim(b.icode)||trim(b.invno)=trim(a.icode)||trim(a.roll_no) group by a.icode,b.desc_)d , item c where trim(d.icode)=trim(c.icode) group by c.Iname,c.Cpartno,D.icode,C.Maker ";
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view gr_w_stk as (" + SQuery + ")");
                query1 = "select branchcd,type,ciname,cpartno,pordno,porddt,acode,icode,ordno,orddt,qtyord,0 as sale,upper(packinst) as packinst,cu_chldt from somas where branchcd='" + frm_mbr + "' and type like '4%' and TYPE!='4A' and TYPE!='4C' AND  orddt " + xxprd + " AND TRIM(NVL(ICAT,'-'))<>'Y' AND TRIM(NVL(APP_BY,'-'))<>'-' and trim(desc_)='1' union all select branchcd,type,null as ciname,null as cpartno,null as pordno,null as porddt,acode,icode,ponum,podate,0 as qtyord,iqtyout as sale,null as packinst,null as cu_Chldt from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and TYPE!='4A' and TYPE!='4C' AND vchdate " + xxprd + " and store='Y' ";
                query1 = "select b.Aname,s.gr_1 as stock,max(nvl(A.ciname,'-')) as cINAME,max(nvl(A.cu_Chldt,a.porddt)) as cu_Chldt,(case when max(trim(nvl(A.packinst,'-')))='-' then 'Other Pending Orders' else max(trim(nvl(A.packinst,'-'))) end ) as packinst,sum(a.qtyord) as qtyord,sum(a.sale) as qty_out,sum(a.qtyord)-sum(a.sale) as bal,c.Unit,max(nvl(a.cpartno,'-')) as Part_no,max(nvl(a.pordno,'-')) as PO_NO,max(a.porddt) as PO_DT,a.ordno,a.orddt,trim(a.acode) as Acode,trim(a.icode) as Icode,a.type,a.branchcd from (" + query1 + ")a LEFT OUTER JOIN gr_w_stk S ON trim(a.icode)=trim(s.icode),famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.Icode) " + cond + " group by a.branchcd,a.type,b.aname,c.unit,c.iname,trim(a.acode),trim(a.icode),a.ordno,a.orddt,s.gr_1 having sum(a.qtyord)-sum(a.sale)>0 order by A.ORDNO,B.aname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, query1);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Crpt_KLAS2", "Crpt_KLAS2", dsRep, "");
                }
                break;
            ////===========//===========//===========//===========//===================






            //=====================11//=====================				

            case "RPT20":
                #region Yield Grading Report
                //branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                #region Already Running Code Commented on 17 Sept 2018
                //xprdrange1 = " ";
                //xprdrange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";
                //SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,to_char(a.vchdate,'yyyymmdd') as vdd,a.vchdate,A.ENT_BY,A.STORE_NO,A.ICODE,B.INAME,trim(B.CPARTNO) as CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY) AS SPECS,B.MAKER AS COLOR,B.WT_NET AS WIDTH,B.WT_RR AS THICK,SUBSTR(A.BTCHNO,0,9) AS BTCHNO,A.O_DEPTT,SUM(A.FST) AS FST,SUM(A.SEC) AS SEC,SUM(A.NS) AS NS,sum(distinct A.QTY) AS QTY,sum(a.sampl) as sampl,rtrim(xmlagg(xmlelement(e,replace(a.SEC_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') SEC_RMK,rtrim(xmlagg(xmlelement(e,replace(a.ns_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') NS_RMK,a.purpose FROM (SELECT (case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimate where " + branch_Cd + " and TYPE='RR' and vchdate " + xprdrange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATE B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||substr(TRIM(A.BTCHNO),0,9)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||substr(TRIM(B.COL6),0,9) AND A." + branch_Cd + " AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE UNION ALL SELECT (case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimateK where " + branch_Cd + " and TYPE='RR' and vchdate " + xprdrange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATEK B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||substr(TRIM(A.BTCHNO),0,9)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||substr(TRIM(B.COL6),0,9) AND A." + branch_Cd + " AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE) NOT IN ('90010215','90010548','90010228','90030048','90030822','90031097') GROUP BY A.ENT_BY,A.STORE_NO,A.ICODE,SUBSTR(A.BTCHNO,0,9),A.O_DEPTT,B.INAME,B.CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY),B.MAKER,B.WT_NET,B.WT_RR,a.vchdate,a.purpose order by a.o_deptt,vdd,SUBSTR(A.BTCHNO,0,9),a.icode";
                //fgen.Print_Report(co_cd, mbr, SQuery, "yld_klas", "yld_klas");
                #endregion
                #region new code
                xprdRange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,to_char(a.vchdate,'yyyymmdd') as vdd,A.FSTR,a.vchdate,A.ENT_BY,A.STORE_NO,A.ICODE,B.INAME,trim(B.CPARTNO) as CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY) AS SPECS,B.MAKER AS COLOR,B.WT_NET AS WIDTH,B.WT_RR AS THICK,SUBSTR(A.BTCHNO,0,9) AS BTCHNO,A.O_DEPTT,SUM(A.FST) AS FST,SUM(A.SEC) AS SEC,SUM(A.NS) AS NS,sum(distinct A.QTY) AS QTY,sum(a.sampl) as sampl,rtrim(xmlagg(xmlelement(e,replace(a.SEC_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') SEC_RMK,rtrim(xmlagg(xmlelement(e,replace(a.ns_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') NS_RMK,a.purpose FROM (SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimate where branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATE B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||substr(TRIM(A.BTCHNO),0,9)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||substr(TRIM(B.COL6),0,9) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy')  UNION ALL  SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimateK where branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATEK B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||substr(TRIM(A.BTCHNO),0,9)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||substr(TRIM(B.COL6),0,9) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') ) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE) NOT IN ('90010215','90010548','90010228','90030048','90030822','90031097') GROUP BY A.ENT_BY,A.STORE_NO,A.ICODE,SUBSTR(A.BTCHNO,0,9),A.O_DEPTT,B.INAME,B.CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY),B.MAKER,B.WT_NET,B.WT_RR,a.vchdate,a.purpose,A.FSTR order by a.o_deptt,vdd,SUBSTR(A.BTCHNO,0,9),a.icode"; //old in this btchno only 9 digit .....This is running qry
                // JOINING OF IVOUCHER AND COSTESTIMATEK(SAME WITH COSTESTIMATE) IS BASED ON "SUBSTR(TRIM(A.BTCHNO),0,9)= SUBSTR(TRIM(B.COL6),0,9)" WHICH GIVES EXTRA ROWS SO JOINING DONE WITHOUT SUBSTR ON 04/05/2019
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,to_char(a.vchdate,'yyyymmdd') as vdd,A.FSTR,a.bvchnum,a.vchdate,A.ENT_BY,A.STORE_NO,A.ICODE,B.INAME,trim(B.CPARTNO) as CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY) AS SPECS,B.MAKER AS COLOR,B.WT_NET AS WIDTH,B.WT_RR AS THICK,SUBSTR(A.BTCHNO,0,9) AS BTCHNO,A.O_DEPTT,SUM(A.FST) AS FST,SUM(A.SEC) AS SEC,SUM(A.NS) AS NS,sum(distinct A.QTY) AS QTY,sum(a.sampl) as sampl,rtrim(xmlagg(xmlelement(e,replace(a.SEC_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') SEC_RMK,rtrim(xmlagg(xmlelement(e,replace(a.ns_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') NS_RMK,a.purpose FROM (SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.vchnum as bvchnum,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimate where branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATE B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(B.COL6) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),b.vchnum UNION ALL  SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.vchnum as bvchnum,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimateK where branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATEK B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(B.COL6) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),b.vchnum) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE) NOT IN ('90010215','90010548','90010228','90030048','90030822','90031097') GROUP BY A.ENT_BY,A.STORE_NO,A.ICODE,SUBSTR(A.BTCHNO,0,9),A.O_DEPTT,B.INAME,B.CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY),B.MAKER,B.WT_NET,B.WT_RR,a.vchdate,a.purpose,A.FSTR,a.bvchnum order by a.o_deptt,vdd,SUBSTR(A.BTCHNO,0,9),a.icode";
                //  SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,b.VCHDATE,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,VCHDATE,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6),VCHDATE) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),b.vchdate,trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) ,c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt ";  //Isme ivch se left join kiya h costestimate ko...qhi ivch me btchno aara tha but costesmate me ni tha...so query me b ni aara tha
                //   SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6)) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) ,c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt ";  ////isme vchdate ivoucher se pic ki hai qki left join krne par vchdate cost table se blank aari thi
                //SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.qty, a.store_no,a.fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,a.iname,a.cpartno,a.unit,a.specs,a.color,a.width,a.thick,a.icode,a.vchdate,a.btchno,a.o_deptt,a.sampl,a.fst,a.sec,a.ns,a.vdd from (SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,a.store_no,a.ENT_BY,a.t_deptt,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,to_char(a.vchdate,'yyyyMMdd') as vdd,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6)) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY),c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt,a.ENT_BY,a.t_deptt,a.store_no,to_char(a.vchdate,'yyyyMMdd') ) a order by  vdd";
                // SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.qty, a.store_no,a.fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,a.iname,a.cpartno,a.unit,a.specs,a.color,a.width,a.thick,a.icode,a.vchdate,a.btchno,a.o_deptt,a.sampl,a.fst,a.sec,a.ns,a.vdd from (SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,a.store_no,a.ENT_BY,a.t_deptt,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,to_char(a.vchdate,'yyyyMMdd') as vdd,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6)) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY),c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt,a.ENT_BY,a.t_deptt,a.store_no,to_char(a.vchdate,'yyyyMMdd') UNION ALL SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,a.store_no,a.ENT_BY,a.t_deptt,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,to_char(a.vchdate,'yyyyMMdd') as vdd,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimateK WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6)) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY),c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt,a.ENT_BY,a.t_deptt,a.store_no,to_char(a.vchdate,'yyyyMMdd') ) a order by  vdd"; //WITH UNION ALL
                // SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,a.qty, a.store_no,a.fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,a.iname,a.cpartno,a.unit,a.specs,a.color,a.width,a.thick,a.icode,a.vchdate,a.btchno,a.o_deptt,a.sampl,a.fst,a.sec,a.ns,a.vdd from (SELECT SUM(QTY) AS QTY,STORE_NO,FSTR,t_deptt,ent_by,iname,cpartno,unit,specs,color,width,thick,icode,vchdate,vdd,btchno,o_deptt,sampl,fst,sec,ns from (SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,a.store_no,a.ENT_BY,a.t_deptt,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,to_char(a.vchdate,'yyyyMMdd') as vdd,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimate WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6)) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY),c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt,a.ENT_BY,a.t_deptt,a.store_no,to_char(a.vchdate,'yyyyMMdd') UNION ALL SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,a.store_no,a.ENT_BY,a.t_deptt,C.iname,TRIM(c.CPARTNO) AS CPARTNO,c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY) AS SPECS,c.MAKER AS COLOR,c.WT_NET AS WIDTH,c.WT_RR AS THICK,TRIM(A.ICODE) AS ICODE,to_Char(A.vchdate,'dd/mm/yyyy') as VCHDATE,to_char(a.vchdate,'yyyyMMdd') as vdd,trim(A.BTCHNO) as BTCHNO,a.o_deptt,sum(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',SUM(A.IQTYIN),0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',SUM(A.IQTYIN),0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'NS',SUM(A.IQTYIN),0) AS NS,B.QTY as qty FROM ivoucher a LEFT OUTER JOIN (SELECT  BRANCHCD,TRIM(ICODE) AS ICODE,TRIM(COL6) AS COL6,SUM(QTY) AS QTY FROM costestimateK WHERE " + branch_Cd + " AND TYPE='40' GROUP BY BRANCHCD,TRIM(ICODE),TRIM(COL6)) b ON  TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(b.col6) ,item c where trim(a.icode)=trim(c.icode) and A." + branch_Cd + " AND A.TYPE='16' and A.VCHDATE " + xprdrange + " " + xprdrange1 + " group by trim(a.icode),to_Char(A.vchdate,'dd/mm/yyyy'),trim(A.BTCHNO),UPPER(TRIM(A.DESC_)),B.QTY,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),C.iname,TRIM(c.CPARTNO),c.UNIT,trim(c.CDRGNO||'-'||c.SALLOY),c.MAKER ,c.WT_NET,c.WT_RR,a.o_deptt,a.ENT_BY,a.t_deptt,a.store_no,to_char(a.vchdate,'yyyyMMdd') )  GROUP BY STORE_NO,FSTR,t_deptt,ent_by,iname,cpartno,unit,specs,color,width,thick,icode,vchdate,vdd,btchno,o_deptt,sampl,fst,sec,ns ) a order by  vdd"; //again sum on qty for make 1 row
                // AFTER ADDING MORE ITEMS IN NOT IN CLAUSE ON 27 JAN 2020
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,to_char(a.vchdate,'yyyymmdd') as vdd,A.FSTR,a.bvchnum,a.vchdate,A.ENT_BY,A.STORE_NO,A.ICODE,B.INAME,trim(B.CPARTNO) as CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY) AS SPECS,B.MAKER AS COLOR,B.WT_NET AS WIDTH,B.WT_RR AS THICK,SUBSTR(A.BTCHNO,0,9) AS BTCHNO,A.O_DEPTT,SUM(A.FST) AS FST,SUM(A.SEC) AS SEC,SUM(A.NS) AS NS,sum(distinct A.QTY) AS QTY,sum(a.sampl) as sampl,rtrim(xmlagg(xmlelement(e,replace(a.SEC_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') SEC_RMK,rtrim(xmlagg(xmlelement(e,replace(a.ns_RMK,'-',null)||',')).extract('//text()').extract('//text()'),',') NS_RMK,a.purpose FROM (SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.vchnum as bvchnum,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimate where branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATE B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(B.COL6) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),b.vchnum UNION ALL  SELECT a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,(case when nvl(trim(A.t_deptt),'-')='-' then a.ent_by else trim(a.t_deptt) end) as ENT_BY,A.STORE_NO,TRIM(A.ICODE) AS ICODE,b.vchnum as bvchnum,b.VCHDATE,A.BTCHNO,A.INVNO,A.O_DEPTT,(a.idiamtr+a.rej_sdp) as sampl,DECODE(UPPER(TRIM(A.DESC_)),'1',A.IQTYIN,0) AS FST,DECODE(UPPER(TRIM(A.DESC_)),'1B',A.IQTYIN,0) AS SEC,DECODE(UPPER(TRIM(A.DESC_)),'1B',replace(trim(A.PURPOSE),'0',null),'-') AS SEC_RMK,DECODE(UPPER(TRIM(A.DESC_)),'NS',A.IQTYIN,0) AS NS,DECODE(UPPER(TRIM(A.DESC_)),'NS',replace(trim(A.PURPOSE),'0',null),'-') AS NS_RMK ,sum(B.QTY) as qty,purpose1 as purpose FROM IVOUCHER A LEFT OUTER JOIN (select rtrim(xmlagg(xmlelement(e,(col1||'-'||qty)||',  ')).extract('//text()').extract('//text()'),',') as purpose1,branchcd,vchnum,vchdate,type from costestimateK where branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + "  group by branchcd,vchnum,vchdate,type) c ON A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.type='RR' ,COSTESTIMATEK B WHERE TRIM(A.BRANCHCD)||TRIM(A.ICODE)||TRIM(A.BTCHNO)=TRIM(B.BRANCHCD)||TRIM(B.ICODE)||TRIM(B.COL6) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='16' AND B.TYPE='40' AND TRIM(B.JSTATUS)='P' AND A.VCHDATE " + xprdRange + " " + xprdRange1 + " group by a.t_deptt,a.ent_by,a.store_no,trim(a.icode),b.vchdate,a.btchno,a.invno,a.o_deptt,(a.idiamtr+a.rej_sdp),UPPER(TRIM(A.DESC_)),a.iqtyin,c.purpose1,A.PURPOSE,a.branchcd||trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy'),b.vchnum) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE) NOT IN ('90010215','90010548','90010228','90030048','90030822','90031097','90013548','90013552','90013553','90013554','90013555','90013556','90013557','90013558','90040050') GROUP BY A.ENT_BY,A.STORE_NO,A.ICODE,SUBSTR(A.BTCHNO,0,9),A.O_DEPTT,B.INAME,B.CPARTNO,B.UNIT,trim(B.CDRGNO||'-'||B.SALLOY),B.MAKER,B.WT_NET,B.WT_RR,a.vchdate,a.purpose,A.FSTR,a.bvchnum order by a.o_deptt,vdd,SUBSTR(A.BTCHNO,0,9),a.icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy') as fstr,NVL(TRIM(col1),'-') AS COL1,sum(qty) as qty FROM costestimate WHERE branchcd='" + frm_mbr + "' and TYPE='RR' and vchdate " + xprdRange + " group by NVL(TRIM(col1),'-'),branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy') order by branchcd||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')");

                dv = new DataView(dt);
                dv.Sort = "o_deptt,btchno,icode";

                dt1 = new DataTable();
                dt1 = dv.ToTable(true, "o_deptt", "btchno", "icode");

                mdt = new DataTable();
                mdt = dt.Clone();
                mdt.Columns.Add(new DataColumn("purpose1", typeof(string)));

                dt3 = new DataTable();
                mq8 = "select trim(vchnum) as vchnum,to_char(vchdate,'yyyymmdd') as vchdate,sum(qty) as qty,substr(trim(col6),0,9) as btchno from costestimate where branchcd='" + frm_mbr + "' and type='40' and jstatus='P' group by  trim(vchnum),to_char(vchdate,'yyyymmdd'),substr(trim(col6),0,9)";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq8);

                oporow = null;
                foreach (DataRow dr3 in dt1.Rows)
                {
                    #region
                    if (dr3["btchno"].ToString().Trim() == "M2/600249") { }
                    dv1 = new DataView(dt, "o_deptt='" + dr3["o_deptt"].ToString().Trim() + "' and btchno='" + dr3["btchno"].ToString().Trim() + "' and icode='" + dr3["icode"].ToString().Trim() + "'", "btchno", DataViewRowState.CurrentRows);
                    #region Already Running Code
                    // RESON FOR COMMENT - USER SAYS ONE JUMBO ROLL IS PRESENT ON TWO MACHINES BUT REPORT IS SHOWING ONLY ONE
                    //if (dv1.Count > 0)
                    //{
                    //    oporow = mdt.NewRow();
                    //    oporow["fromdt"] = dv1[0].Row["fromdt"].ToString().Trim();
                    //    oporow["todate"] = dv1[0].Row["todate"].ToString().Trim();
                    //    oporow["store_no"] = dv1[0].Row["store_no"].ToString().Trim();
                    //    oporow["icode"] = dv1[0].Row["icode"].ToString().Trim();
                    //    oporow["iname"] = dv1[0].Row["iname"].ToString().Trim();
                    //    oporow["cpartno"] = dv1[0].Row["cpartno"].ToString().Trim();
                    //    oporow["unit"] = dv1[0].Row["unit"].ToString().Trim();
                    //    oporow["specs"] = dv1[0].Row["specs"].ToString().Trim();
                    //    oporow["color"] = dv1[0].Row["color"].ToString().Trim();
                    //    oporow["width"] = dv1[0].Row["width"].ToString().Trim();
                    //    oporow["thick"] = dv1[0].Row["thick"].ToString().Trim();
                    //    oporow["vchdate"] = dv1[0].Row["vchdate"].ToString().Trim();
                    //    oporow["btchno"] = dv1[0].Row["btchno"].ToString().Trim();
                    //    oporow["ent_by"] = dv1[0].Row["ent_by"].ToString().Trim();

                    //    double1 = 0; double2 = 0; double3 = 0; double4 = 0;
                    //    mq0 = ""; mq10 = "";
                    //    for (int i = 0; i < dv1.Count; i++)
                    //    {
                    //        double1 += fgen.make_double(dv1[i].Row["sampl"].ToString().Trim());
                    //        double2 += fgen.make_double(dv1[i].Row["fst"].ToString().Trim());
                    //        double3 += fgen.make_double(dv1[i].Row["sec"].ToString().Trim());
                    //        double4 += fgen.make_double(dv1[i].Row["ns"].ToString().Trim());
                    //        mq10 = dv1[i].Row["fstr"].ToString().Trim();
                    //    }

                    //    dv2 = new DataView(dt2, "fstr='" + mq10 + "'", "fstr", DataViewRowState.CurrentRows);
                    //    dv2.Sort = "col1";
                    //    mq2 = "";
                    //    for (int x = 0; x < dv2.Count; x++)
                    //    {
                    //        if (fgen.make_double(dv2[x].Row["qty"].ToString().Trim()) > 0)
                    //        {
                    //            if (mq0.Length > 0)
                    //            {
                    //                mq0 = mq0 + ", " + dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                    //            }
                    //            else mq0 = dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                    //        }
                    //    }
                    //    oporow["sampl"] = double1;
                    //    oporow["fst"] = double2;
                    //    oporow["sec"] = double3;
                    //    oporow["ns"] = double4;
                    //    oporow["purpose"] = mq0;
                    //    if (dv1[0].Row["qty"].ToString().Trim() == "0" || dv1[0].Row["qty"].ToString().Trim() == "")
                    //    {
                    //        oporow["qty"] = "0";
                    //    }
                    //    else
                    //    {
                    //        oporow["qty"] = dv1[0].Row["qty"].ToString().Trim();
                    //    }
                    //    mdt.Rows.Add(oporow);
                    //}
                    #endregion
                    for (int i = 0; i < dv1.Count; i++)
                    {
                        double1 = 0; double2 = 0; double3 = 0; double4 = 0;
                        mq0 = ""; mq10 = "";
                        oporow = mdt.NewRow();
                        oporow["fromdt"] = dv1[i].Row["fromdt"].ToString().Trim();
                        oporow["todate"] = dv1[i].Row["todate"].ToString().Trim();
                        oporow["store_no"] = dv1[i].Row["store_no"].ToString().Trim();
                        oporow["icode"] = dv1[i].Row["icode"].ToString().Trim();
                        oporow["iname"] = dv1[i].Row["iname"].ToString().Trim();
                        oporow["cpartno"] = dv1[i].Row["cpartno"].ToString().Trim();
                        oporow["unit"] = dv1[i].Row["unit"].ToString().Trim();
                        oporow["specs"] = dv1[i].Row["specs"].ToString().Trim();
                        oporow["color"] = dv1[i].Row["color"].ToString().Trim();
                        oporow["width"] = dv1[i].Row["width"].ToString().Trim();
                        oporow["thick"] = dv1[i].Row["thick"].ToString().Trim();
                        oporow["vchdate"] = dv1[i].Row["vchdate"].ToString().Trim();
                        oporow["btchno"] = dv1[i].Row["btchno"].ToString().Trim();
                        oporow["ent_by"] = dv1[i].Row["ent_by"].ToString().Trim();
                        oporow["o_deptt"] = dv1[i].Row["o_deptt"].ToString().Trim();

                        double1 += fgen.make_double(dv1[i].Row["sampl"].ToString().Trim());
                        double2 += fgen.make_double(dv1[i].Row["fst"].ToString().Trim());
                        double3 += fgen.make_double(dv1[i].Row["sec"].ToString().Trim());
                        double4 += fgen.make_double(dv1[i].Row["ns"].ToString().Trim());
                        mq10 = dv1[i].Row["fstr"].ToString().Trim();

                        dv2 = new DataView(dt2, "fstr='" + mq10 + "'", "fstr", DataViewRowState.CurrentRows);
                        dv2.Sort = "col1";
                        mq2 = "";
                        for (int x = 0; x < dv2.Count; x++)
                        {
                            if (fgen.make_double(dv2[x].Row["qty"].ToString().Trim()) > 0)
                            {
                                if (mq0.Length > 0)
                                {
                                    mq0 = mq0 + ", " + dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                                }
                                else mq0 = dv2[x].Row["col1"].ToString().Trim() + "-" + dv2[x].Row["qty"].ToString().Trim();
                            }
                        }
                        oporow["sampl"] = double1;
                        oporow["fst"] = double2;
                        oporow["sec"] = double3;
                        oporow["ns"] = double4;
                        oporow["purpose1"] = mq0;
                        if (dv1[0].Row["qty"].ToString().Trim() == "0" || dv1[0].Row["qty"].ToString().Trim() == "")
                        {
                            oporow["qty"] = "0";
                        }
                        else
                        {
                            // oporow["qty"] = dv1[i].Row["qty"].ToString().Trim();
                            if (dt3.Rows.Count > 0)
                            {
                                oporow["qty"] = fgen.seek_iname_dt(dt3, "vchnum='" + dv1[i].Row["bvchnum"].ToString().Trim() + "' and vchdate='" + dv1[i].Row["vdd"].ToString().Trim() + "' and btchno='" + dv1[i].Row["btchno"].ToString().Trim() + "'", "qty");
                            }
                        }
                        mdt.Rows.Add(oporow);
                    }
                    #endregion
                }
                if (mdt.Rows.Count > 0)
                {
                    mdt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(mdt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "yld_klas", "yld_klas", dsRep, header_n);
                }
                #endregion
                #endregion
                break;

            ////===========//===========//================////===========//===========//===========//=============






            //=====================12//=====================				

            case "RPT30":
                #region Order Size Report Summary
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,count(distinct a.ordno) as ordno,(case when a.head='" + (i4 + 1) + "' then 'Above' else to_char(a.HEAD) end) as head,sum(a.qty) as qty from (" +
                " select " + i0 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)<=" + i0 + " union all" +
                " select " + i1 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " union all" +
                " select " + i2 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " union all" +
                " select " + i3 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " union all" +
                " select " + i4 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " union all" +
                " select " + (i4 + 1) + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)>" + (i4 + 1) + ")" +
                " a group by a.HEAD HAVING SUM(QTY)>0 ORDER BY a.HEAD";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt1", "krpt1", dsRep, "");
                }
                #endregion
                break;

            ////===========//===========//===========//===========//===================				





            //=====================13//=====================
            case "RPT31":
                #region Order Size Report Customer Wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,count(distinct a.ordno) as ordno,(case when a.head='" + (i4 + 1) + "' then 'Above' else to_char(a.HEAD) end) as head,a.acode,b.aname,sum(a.qty) as qty from (" +
                " select " + i0 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)<=" + i0 + " union all" +
                " select " + i1 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " union all" +
                " select " + i2 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " union all" +
                " select " + i3 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " union all" +
                " select " + i4 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " union all" +
                " select " + (i4 + 1) + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)>" + (i4 + 1) + ")" +
                " a,famst b where trim(a.acode)=trim(b.acode) group by a.acode,a.HEAD,b.aname HAVING SUM(QTY)>0 ORDER BY a.HEAD,b.aname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt2", "krpt2", dsRep, "");
                }
                #endregion
                break;
            ////===========//===========//===========//===========//===================				





            //=====================14//=====================
            case "RPT8":
                #region   Order Size Report Group,Customer Wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,count(distinct a.ordno) as ordno,(case when a.head='" + (i4 + 1) + "' then 'Above' else to_char(a.HEAD) end) as head,a.acode,b.aname,b.bssch,c.name,sum(a.qty) as qty from (" +
                " select " + i0 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)<=" + i0 + " union all" +
                " select " + i1 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " union all" +
                " select " + i2 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " union all" +
                " select " + i3 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " union all" +
                " select " + i4 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " union all" +
                " select " + (i4 + 1) + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)>" + (i4 + 1) + ")" +
                " a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' group by a.acode,a.HEAD,b.aname,b.bssch,c.name HAVING SUM(QTY)>0 ORDER BY a.HEAD,b.bssch,b.aname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt3", "krpt3", dsRep, "");
                }
                #endregion
                break;
            ////===========//===========//================////===========//===========//===========//=============





            //=====================15//=====================				

            case "RPT32":
                #region  Order Size Report Product Wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Order Size Report Product Group Wise' as header,count(distinct a.ordno) as ordno,(case when a.head='" + (i4 + 1) + "' then 'Above' else to_char(a.HEAD) end) as head,b.icode as acode,b.iname as aname,sum(a.qty) as qty from (" +
                " select " + i0 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)<=" + i0 + " union all" +
                " select " + i1 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " union all" +
                " select " + i2 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " union all" +
                " select " + i3 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " union all" +
                " select " + i4 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " union all" +
                " select " + (i4 + 1) + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)>" + (i4 + 1) + ")" +
                " a,item b where substr(a.icode,1,4)=trim(b.icode) group by a.HEAD,b.icode,b.iname HAVING SUM(QTY)>0 ORDER BY a.HEAD,b.iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt4", "krpt4", dsRep, "");
                }

                #endregion
                break;

            ////===========//===========//================////===========//===========//===========//===========//===========//============




            //=====================16//=====================				

            case "RPT33":
                #region  order size report part no wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Order Size Report Part no Wise' as header,count(distinct a.ordno) as ordno,(case when a.head='" + (i4 + 1) + "' then 'Above' else to_char(a.HEAD) end) as head,b.cpartno as acode,b.cpartno as aname,sum(a.qty) as qty from (" +
                " select " + i0 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)<=" + i0 + " union all" +
                " select " + i1 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " union all" +
                " select " + i2 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " union all" +
                " select " + i3 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " union all" +
                " select " + i4 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " union all" +
                " select " + (i4 + 1) + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)>" + (i4 + 1) + ")" +
                " a,item b where trim(a.icode)=trim(b.icode) group by a.HEAD,b.cpartno HAVING SUM(QTY)>0 ORDER BY a.HEAD,b.cpartno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt4", "krpt4", dsRep, "");
                }
                #endregion
                break;

            ////===========//===========//================////===========//===========//===========//=============



            //=====================17//=====================				

            case "RPT34":
                #region  order size report R/Paper Wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Order Size Report R/Paper Wise' as header,count(distinct a.ordno) as ordno,(case when a.head='" + (i4 + 1) + "' then 'Above' else to_char(a.HEAD) end) as head,b.cdrgno as acode,b.cdrgno as aname,sum(a.qty) as qty from (" +
                " select " + i0 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)<=" + i0 + " union all" +
                " select " + i1 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " union all" +
                " select " + i2 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " union all" +
                " select " + i3 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " union all" +
                " select " + i4 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " union all" +
                " select " + (i4 + 1) + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)>" + (i4 + 1) + ")" +
                " a,item b where trim(a.icode)=trim(b.icode) group by a.HEAD,b.cdrgno HAVING SUM(QTY)>0 ORDER BY a.HEAD,b.cdrgno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt4", "krpt4", dsRep, "");
                }
                #endregion
                break;

            ////===========//===========//================18////===========//===========//===========//=============				

            case "RPT35":
                #region  order size report customer group wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Order Size Report Customer Group Wise' as header,count(distinct a.ordno) as ordno,(case when a.head='" + (i4 + 1) + "' then 'Above' else to_char(a.HEAD) end) as head,b.bssch as acode,c.name as aname,sum(a.qty) as qty from (" +
                " select " + i0 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)<=" + i0 + " union all" +
                " select " + i1 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " union all" +
                " select " + i2 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " union all" +
                " select " + i3 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " union all" +
                " select " + i4 + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " union all" +
                " select " + (i4 + 1) + " as head,ordno,orddt,acode,icode,sum(QTYORD) as qty from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode having sum(QTYORD)>" + (i4 + 1) + ")" +
                " a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' group by a.HEAD,b.bssch,c.name HAVING SUM(QTY)>0 ORDER BY a.HEAD,b.bssch";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt4", "krpt4", dsRep, "");
                }
                #endregion
                break;

            ////===========//===========//===========//===============19////===========//===========//===========//===========//=============				

            case "RPT36":
                #region  order size report MTR wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                i0 = Convert.ToInt32(mq0.Split(',')[0].ToString());
                i1 = Convert.ToInt32(mq0.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq0.Split(',')[2].ToString());
                i3 = Convert.ToInt32(mq0.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq0.Split(',')[4].ToString());
                SQuery = "Select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Order Size Report Customer Group Wise' as header,'" + i0 + "' as h0,'" + i1 + "' as h1,'" + i2 + "' as h2,'" + i3 + "' as h3,'" + i4 + "' as h4,b.aname,count(distinct ordno) as ordno,a.acode,a.icode,sum(a.qty1) as qty1,sum(a.qty2) as qty2,sum(a.qty3) as qty3,sum(a.qty4) as qty4,sum(a.qty5) as qty5,sum(a.qty6) as qty6 from (select ordno,orddt,acode,icode,(case when sum(QTYORD)<=" + i0 + " then sum(QTYORD) else 0 end) as qty1,(case when sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " then sum(QTYORD) else 0 end) as qty2,(case when sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " then sum(QTYORD) else 0 end) as qty3,(case when sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " then sum(QTYORD) else 0 end) as qty4,(case when sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " then sum(QTYORD) else 0 end) as qty5,(case when sum(QTYORD)>" + (i4 + 1) + " then sum(QTYORD) else 0 end) as qty6 from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode) a,famst b where trim(a.acode)=trim(b.acode) group by a.acode,a.icode,b.aname order by b.aname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt5", "krpt5", dsRep, "");
                }
                #endregion
                break;

            ////===========//===========//===========//===============20////===========//===========//===========//===========//=============				

            case "RPT37":
                #region order size report MTR wise summary
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (mq0 != "Y")
                {
                    i0 = Convert.ToInt32(mq1.Split(',')[0].ToString());
                    i1 = Convert.ToInt32(mq1.Split(',')[1].ToString()); i2 = Convert.ToInt32(mq1.Split(',')[2].ToString());
                    i3 = Convert.ToInt32(mq1.Split(',')[3].ToString()); i4 = Convert.ToInt32(mq1.Split(',')[4].ToString());
                }
                else
                {
                    if (mq1.Trim().Length > 0)
                    {
                        if (mq1.Trim().Length > 6) cond = "and b.bssch in (" + mq1.Trim() + ")";
                        else cond = "and b.bssch in (" + mq1.Trim() + ")";
                    }
                }
                SQuery = "Select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Order Size Report Customer Group Wise' as header,'" + i0 + "' as h0,'" + i1 + "' as h1,'" + i2 + "' as h2,'" + i3 + "' as h3,'" + i4 + "' as h4,b.aname,b.bssch,c.name,count(distinct ordno) as ordno,a.acode,a.icode,sum(a.qty1) as qty1,sum(a.qty2) as qty2,sum(a.qty3) as qty3,sum(a.qty4) as qty4,sum(a.qty5) as qty5,sum(a.qty6) as qty6 from (select ordno,orddt,acode,icode,(case when sum(QTYORD)<=" + i0 + " then sum(QTYORD) else 0 end) as qty1,(case when sum(QTYORD) between " + (i0 + 1) + " and " + i1 + " then sum(QTYORD) else 0 end) as qty2,(case when sum(QTYORD) between " + (i1 + 1) + " and " + i2 + " then sum(QTYORD) else 0 end) as qty3,(case when sum(QTYORD) between " + (i2 + 1) + " and " + i3 + " then sum(QTYORD) else 0 end) as qty4,(case when sum(QTYORD) between " + (i3 + 1) + " and " + i4 + " then sum(QTYORD) else 0 end) as qty5,(case when sum(QTYORD)>" + (i4 + 1) + " then sum(QTYORD) else 0 end) as qty6 from somas where branchcd='" + frm_mbr + "' and type like '4%' and type not in ('45','4C','4E','4K') and orddt " + xprdRange + " group by ordno,orddt,acode,icode) a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' " + cond + " group by a.acode,a.icode,b.aname,b.bssch,c.name order by b.bssch,b.aname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "krpt5_s", "krpt5_s", dsRep, "");
                }
                //   fgen.Print_Report(co_cd, mbr, SQuery, "krpt5", "krpt5_s");
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