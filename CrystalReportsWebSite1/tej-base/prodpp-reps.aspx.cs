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

public partial class prodpp_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, next_year, DateRange, frm_myear, branch_Cd, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    double db1, db2, db3, db4;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, header_n, cond = "", xprdRange1 = "", data_found = "", WB_TABNAME = "", WB_TABNAME2 = "", pdfView = "";
    fgenDB fgen = new fgenDB();
    string year = "";
    string ind_Ptype = "";
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dtdrsim; DataView view1im;
    double db11 = 0;
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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                    branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCH_CD");
                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    ind_Ptype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
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
        string mq10, mq1 = "", mq0;
        string party_cd = "";
        string part_cd = "";
        string mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq11, mq12, ded1, ded2;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string opt = "";
        data_found = "Y";
        switch (iconID)
        {
            case "F39201":
                SQuery = "select 'Material Issue Request' as header,'Material Issue Request' as h1,'Issue Agst Job Card' as h2, C.NAME AS DPT_NAME,I.INAME,I.CPARTNO,I.UNIT AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM wb_iss_req A, ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND C.ID='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                dsRep = new DataSet();
                break;

            case "F15235":
                header_n = "Material Consumption Report";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,t.name as deptt,a.acode,a.icode,c.iname as itemname,c.cpartno,c.unit,sum(iqtyout) as iqtyout,sum(iqtyin) as iqtyin,sum(iqtyout-iqtyin) as diff from (Select trim(acode) as acode,trim(icode) as icode,iqtyout as iqtyout,0 as iqtyin from ivoucher where branchcd='" + frm_mbr + "' and type like '3%' and type<>'36' and vchdate " + xprdRange + " and store='Y' union all Select trim(acode) as acode,trim(icode) as icode, 0 as iqtyout ,iqtyin from ivoucher where branchcd='" + frm_mbr + "' and type like '1%' and type<'15' and vchdate " + xprdRange + " and store='Y' ) a,item c ,type t where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(t.type1) and t.id='M' group by a.acode,a.icode,c.iname,c.cpartno,c.unit,t.name order by a.acode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "STD_ISS_rq", "STD_ISS_rq", dsRep, "Store Issue Request");
                }
                else
                {
                    data_found = "N";
                }
                break;

            //prodn entry
            case "F39119":
                dsRep = new DataSet();
                dt = new DataTable();
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0011", "OPT_ENABLE");
                SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,A.*,B.INAME,B.CPARTNO,B.UNIT FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.vchdate,a.vchnum,A.MORDER";
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
                else
                {
                    data_found = "N";
                }
                break;

            case "F20132":
                // Gate Inward Register
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,A.SRNO,TRIM(A.ACODE) AS ACODE,(CASE WHEN A.PONUM='-' THEN '000000' ELSE A.PONUM END) AS PONUM,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.type as grp,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,A.IQTY_CHL,A.NARATION,I.INAME,I.UNIT,I.CPARTNO AS PARTNO,F.ANAME,TRIM(F.ADDR1)||TRIM(F.ADDR2) AS ADDRESS,A.MODE_TPT,A.DESC_ FROM IVOUCHERP A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='00' AND A.VCHDATE " + xprdRange + " ORDER BY A.SRNO";
                //dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_Matl_Consumption", "std_Matl_Consumption", dsRep, header_n);
                break;

            //MADE BY AKSHAY ...MERGED BY YOGITA..........
            case "F40132":  //ok kclg 2016-2018
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq0 = ""; mq1 = ""; mq2 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (col1 == "1") { mq1 = ""; }
                else if (col1 == "2") { mq1 = ""; }
                else if (col1 == "3") { mq1 = "AND A.MCHCODE='" + mq0 + "'"; }
                else { mq1 = "AND trim(A.OPR_DTL)='" + mq0 + "'"; }

                if (frm_cocd == "SPIR")
                {
                    WB_TABNAME = "PROD_SHEETK";
                }
                else
                {
                    WB_TABNAME = "PROD_SHEET";
                }
                header_n = "  Daily Prodn Report";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt, '" + header_n + "' as header, A.MSEQ,A.TYPE ,B.INAME ,A.NUM1 AS M_RDY, A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE, A.iqtyin+a.mlt_loss AS PRODN , A.MLT_LOSS as rejn,A.IQTYIN AS NET_PRODN ,A.IQTYOUT AS PLAN_QTY ,A.JOB_NO ,A.PREVCODE AS SHIFT ,A.TSLOT ,A.MCSTART ,A.MCSTOP  , A.OPR_DTL  ,A.ENAME,A.REMARKS2 ,ROUND((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000)) as ppm ,ROUND((((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000))/10000),2) as ppm_prc FROM "+WB_TABNAME+" A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE IN('86','88') AND TRIM(NVL(A.MLT_LOSS,'0'))<>'0' AND TRIM(NVL(A.IQTYIN,'0'))<>'0' and trim(nvl(a.iqtyin+a.mlt_loss,'0'))<>'0'  AND A.VCHDATE " + xprdRange + "  ORDER BY TO_CHAR(A.VCHDATE,'yyyy/MM/dd'),A.PREVCODE ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt, '" + header_n + "' as header, A.MSEQ,A.TYPE ,B.INAME ,A.NUM1 AS M_RDY, A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE, A.iqtyin+a.mlt_loss AS PRODN , A.MLT_LOSS as rejn,A.IQTYIN AS NET_PRODN ,A.IQTYOUT AS PLAN_QTY ,A.JOB_NO ,A.PREVCODE AS SHIFT ,A.TSLOT ,A.MCSTART ,A.MCSTOP  , A.OPR_DTL  ,A.ENAME,A.REMARKS2 ,ROUND((nvl(A.MLT_LOSS,'0')/( nvl(A.iqtyin,'0')+nvl(a.mlt_loss,'1'))*1000000)) as ppm ,ROUND((((nvl(A.MLT_LOSS,'0')/(nvl(A.iqtyin,'0')+nvl(a.mlt_loss,'1'))*1000000))/10000),2) as ppm_prc FROM " + WB_TABNAME + " A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE IN('86','88') AND A.VCHDATE " + xprdRange + " " + mq1 + " ORDER BY TO_CHAR(A.VCHDATE,'yyyy/MM/dd'),A.PREVCODE ";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Daily_Prodn", "std_Daily_Prodn", dsRep, header_n);
                }
                //old code call back
                //header_n = "  Daily Prodn Report";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt, '" + header_n + "' as header, A.MSEQ,A.TYPE ,B.INAME ,A.NUM1 AS M_RDY, A.VCHNUM ,A.VCHDATE, A.iqtyin+a.mlt_loss AS PRODN , A.MLT_LOSS as rejn,A.IQTYIN AS NET_PRODN ,A.IQTYOUT AS PLAN_QTY ,A.JOB_NO ,A.PREVCODE AS SHIFT ,A.TSLOT ,A.MCSTART ,A.MCSTOP  , A.OPR_DTL  ,A.ENAME,A.REMARKS2 ,ROUND((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000)) as ppm ,ROUND((((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000))/10000),2) as ppm_prc FROM PROD_SHEET A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE IN('86','88') AND TRIM(NVL(A.MLT_LOSS,'0'))<>'0' AND TRIM(NVL(A.IQTYIN,'0'))<>'0' and trim(nvl(a.iqtyin+a.mlt_loss,'0'))<>'0'  AND A.VCHDATE " + xprdRange + "  ORDER BY A.VCHDATE,A.PREVCODE ";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //if (dt.Rows.Count > 0)
                //{
                //    dt.TableName = "Prepcur";
                //    dsRep.Tables.Add(dt);
                //    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Daily_Prodn", "std_Daily_Prodn", dsRep, header_n);
                //}
                else
                {
                    data_found = "N";
                }
                break;

            case "F40133"://ok kclg 2016-2018
                header_n = "Monthly  Prodn Analysis ";
                dsRep = new DataSet();
                //SQuery = "SELECT '" + frm_cDt1.Substring(6, 4) + "' as PERIOD, '" + frm_cDt2.Substring(6, 4) + "' AS P2 , '" + header_n + "' as header, (sum(april)+sum(may)+sum(june)+sum(july)+sum(august)+sum(sept)+sum(oct)+sum(nov)+sum(dec)+sum(jan)+sum(feb)+sum(mar)) as total ,Item,Partno,sum(April) as April, sum(May) as May,sum(June) as June,  sum(July) as July, sum(August) as August,sum(Sept) as Sept,sum(oct) as Oct,sum(Nov) as Nov,sum(Dec) as Dec,sum(Jan) as Jan,sum(Feb) as Feb,sum(Mar) as Mar,icode from (Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'yyyymm'),201704,sum(a.iqtyin),0) as April, decode(to_chaR(vchdate,'yyyymm'),201705,sum(a.iqtyin),0) as May, decode(to_chaR(vchdate,'yyyymm'),201706,sum(a.iqtyin),0) as June, decode(to_chaR(vchdate,'yyyymm'),201707,sum(a.iqtyin),0) as July, decode(to_chaR(vchdate,'yyyymm'),201708,sum(a.iqtyin),0) as August,decode(to_chaR(vchdate,'yyyymm'),201709, sum(a.iqtyin),0) as Sept, decode(to_chaR(vchdate,'yyyymm'),201710,sum(a.iqtyin),0) as Oct, decode(to_chaR(vchdate,'yyyymm'),201711,sum(a.iqtyin),0) as Nov, decode(to_chaR(vchdate,'yyyymm'),201712,sum(a.iqtyin),0) as Dec , decode(to_chaR(vchdate,'yyyymm'),201801,sum(a.iqtyin),0) as Jan, decode(to_chaR(vchdate,'yyyymm'),201802,sum(a.iqtyin),0) as Feb, decode(to_chaR(vchdate,'yyyymm'),201803,sum(a.iqtyin),0) as Mar,      a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + frm_mbr + "'  and substr(a.type,1,2)='15' and a.vchdate " + xprdRange + "  group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'yyyymm')  ) group by item,partno,icode order by item  ";
                SQuery = "SELECT '" + frm_cDt1.Substring(6, 4) + "' as PERIOD, '" + frm_cDt2.Substring(6, 4) + "' AS P2 , '" + header_n + "' as header, (sum(april)+sum(may)+sum(june)+sum(july)+sum(august)+sum(sept)+sum(oct)+sum(nov)+sum(dec)+sum(jan)+sum(feb)+sum(mar)) as total ,Item,Partno,sum(April) as April, sum(May) as May,sum(June) as June,  sum(July) as July, sum(August) as August,sum(Sept) as Sept,sum(oct) as Oct,sum(Nov) as Nov,sum(Dec) as Dec,sum(Jan) as Jan,sum(Feb) as Feb,sum(Mar) as Mar,icode from (Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'mm'),04,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as April, decode(to_chaR(vchdate,'mm'),05,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as May, decode(to_chaR(vchdate,'mm'),06,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as June, decode(to_chaR(vchdate,'mm'),07,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as July, decode(to_chaR(vchdate,'mm'),08,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as August,decode(to_chaR(vchdate,'mm'),09, sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Sept, decode(to_chaR(vchdate,'mm'),10,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Oct, decode(to_chaR(vchdate,'mm'),11,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Nov, decode(to_chaR(vchdate,'mm'),12,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Dec , decode(to_chaR(vchdate,'mm'),01,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Jan, decode(to_chaR(vchdate,'mm'),02,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Feb, decode(to_chaR(vchdate,'mm'),03,sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Mar,a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + frm_mbr + "'  and substr(a.type,1,2)='15' and a.store='W' and a.vchdate " + xprdRange + "  group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'mm')  ) group by item,partno,icode order by item  ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Month_Wise_Prodn_Analysis", "std_Month_Wise_Prodn_Analysis", dsRep, header_n);
                }
                //old code call back
                //header_n = "Monthly  Prodn Analysis ";
                //dsRep = new DataSet();
                //SQuery = "SELECT '" + frm_cDt1.Substring(6, 4) + "' as PERIOD, '" + frm_cDt2.Substring(6, 4) + "' AS P2 , '" + header_n + "' as header, (sum(april)+sum(may)+sum(june)+sum(july)+sum(august)+sum(sept)+sum(oct)+sum(nov)+sum(dec)+sum(jan)+sum(feb)+sum(mar)) as total ,Item,Partno,sum(April) as April, sum(May) as May,sum(June) as June,  sum(July) as July, sum(August) as August,sum(Sept) as Sept,sum(oct) as Oct,sum(Nov) as Nov,sum(Dec) as Dec,sum(Jan) as Jan,sum(Feb) as Feb,sum(Mar) as Mar,icode from (Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'yyyymm'),201704,sum(a.iqtyin),0) as April, decode(to_chaR(vchdate,'yyyymm'),201705,sum(a.iqtyin),0) as May, decode(to_chaR(vchdate,'yyyymm'),201706,sum(a.iqtyin),0) as June, decode(to_chaR(vchdate,'yyyymm'),201707,sum(a.iqtyin),0) as July, decode(to_chaR(vchdate,'yyyymm'),201708,sum(a.iqtyin),0) as August,decode(to_chaR(vchdate,'yyyymm'),201709, sum(a.iqtyin),0) as Sept, decode(to_chaR(vchdate,'yyyymm'),201710,sum(a.iqtyin),0) as Oct, decode(to_chaR(vchdate,'yyyymm'),201711,sum(a.iqtyin),0) as Nov, decode(to_chaR(vchdate,'yyyymm'),201712,sum(a.iqtyin),0) as Dec , decode(to_chaR(vchdate,'yyyymm'),201801,sum(a.iqtyin),0) as Jan, decode(to_chaR(vchdate,'yyyymm'),201802,sum(a.iqtyin),0) as Feb, decode(to_chaR(vchdate,'yyyymm'),201803,sum(a.iqtyin),0) as Mar,      a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + frm_mbr + "'  and substr(a.type,1,2)='15' and a.vchdate " + xprdRange + "  group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'yyyymm')  ) group by item,partno,icode order by item  ";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //if (dt.Rows.Count > 0)
                //{
                //    dt.TableName = "Prepcur";
                //    dsRep.Tables.Add(dt);
                //    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Month_Wise_Prodn_Analysis", "std_Month_Wise_Prodn_Analysis", dsRep, header_n);
                //}
                else
                {
                    data_found = "N";
                }
                break;

            //need icon for this on test......made by akshay.......merged by yogita
            //corrugation...this icon on kclg
            case "F40145":
            case "F40106":
            case "F39102":
            case "F40107":
                // CORRUGATION REPORT
                if (frm_cocd == "SPIR" || frm_cocd == "STLC")
                {
                    WB_TABNAME = "INSPVCHK";
                    WB_TABNAME2 = "COSTESTIMATEK";
                }
                else
                {
                    WB_TABNAME = "INSPVCH";
                    WB_TABNAME2 = "COSTESTIMATE";
                }

                if (ind_Ptype == "01" || ind_Ptype == "12")
                {
                    WB_TABNAME = "INSPVCHK";
                    WB_TABNAME2 = "COSTESTIMATEK";
                }


                mq1 = "";
                mq1 = barCode;
                mq1 = mq1.Replace("'", "");
                DataTable dtdummy = new DataTable();
                dtdummy.Columns.Add("h2", typeof(string));
                dtdummy.Columns.Add("h3", typeof(string));
                dtdummy.Columns.Add("h4", typeof(string));
                dtdummy.Columns.Add("h5", typeof(string));
                // headings
                dtdummy.Columns.Add("h6", typeof(string));
                dtdummy.Columns.Add("h7", typeof(string));
                dtdummy.Columns.Add("h8", typeof(string));
                dtdummy.Columns.Add("h9", typeof(string));
                dtdummy.Columns.Add("Total", typeof(double));
                dtdummy.Columns.Add("header1", typeof(string));
                dtdummy.Columns.Add("header", typeof(string));
                dtdummy.Columns.Add("acode", typeof(string));
                dtdummy.Columns.Add("prodNo", typeof(string));
                dtdummy.Columns.Add("ProdDt", typeof(string));
                dtdummy.Columns.Add("Incharge", typeof(string));
                dtdummy.Columns.Add("Shift", typeof(string));
                dtdummy.Columns.Add("mchname", typeof(string));
                dtdummy.Columns.Add("SerialNo", typeof(string));
                dtdummy.Columns.Add("Itemcode", typeof(string));
                dtdummy.Columns.Add("Item_name", typeof(string));
                dtdummy.Columns.Add("BatchNo", typeof(string));
                dtdummy.Columns.Add("pkg", typeof(double));
                dtdummy.Columns.Add("Qty", typeof(double));
                dtdummy.Columns.Add("batchwise", typeof(double));
                dtdummy.Columns.Add("Unit", typeof(string));
                dtdummy.Columns.Add("output", typeof(double));

                #region
                //input
                SQuery = "SELECT A.SUPCL_BY AS MCHNAME ,A.ENQNO,A.COL23 AS COL23,A.ACODE AS ACODE ,A.VCHNUM AS VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.SRNO,A.ICODE AS  ICODE ,B.INAME AS INPUT,B.UNIT,A.COL3 AS BOXES,A.COL4 AS BATCH_QTY,A.COL5 AS COL5,a.col4 ,A.COL6 AS BATCH_NO,c.name as header  FROM " + WB_TABNAME2 + " A ,ITEM B ,type c  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and trim(a.col21)=trim(c.type1) and c.id='1' AND TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + mq1 + "' AND A.TYPE='25' ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                // output dt2
                SQuery = " SELECT A.COL24 AS col_24 ,A.ACODE AS ACODE ,A.VCHNUM AS VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE ,A.SRNO AS SNO,A.ICODE AS ICODE  ,B.INAME AS Iname ,B.UNIT AS UOM,A.COL3 AS BOXES1,A.COL4 AS BATCH_QTY1,A.COL5 AS COL5_,A.COL6 AS BATCH_NO1  FROM " + WB_TABNAME2 + " A ,ITEM B WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + mq1 + "'  AND TYPE='40'";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //REJECTION
                SQuery = "SELECT ACODE AS ACODE,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO, COL1 AS REJ_NAME,COL2 AS REJ_CODE,COL3 AS REJN_QTY,TRIM(ICODE) AS ICODE  FROM " + WB_TABNAME + " WHERE TYPE='45'  AND TRIM(BRANCHCD)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||TRIM(ICODE)='" + mq1 + "'  ORDER BY VCHNUM DESC";
                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                //DOWNTIME
                SQuery = " SELECT ACODE,VCHNUM  ,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO AS SR,COL1 AS down_NAME,COL2 AS DOWN_CODE,COL3 AS DOWN_MINS,TRIM(ICODE) AS ICODE  FROM " + WB_TABNAME + " WHERE TYPE='55'  AND TRIM(BRANCHCD)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||TRIM(ICODE)='" + mq1 + "'  ORDER BY VCHNUM DESC";
                dt4 = new DataTable();
                dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dr1 = null;
                if (dt.Rows.Count > 0)
                {
                    DataView View1 = new DataView(dt);
                    dt6 = new DataTable();
                    dt6 = View1.ToTable(true, "VCHNUM", "vchdate", "acode");
                    foreach (DataRow dr in dt6.Rows)
                    {
                        DataView View2 = new DataView(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  acode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        DataTable dt7 = new DataTable();
                        dt7 = View2.ToTable();

                        for (int i = 0; i < dt7.Rows.Count; i++)   // Input
                        {
                            dr1 = dtdummy.NewRow();
                            dr1["h2"] = "Input";
                            dr1["header1"] = "1";
                            dr1["h3"] = "ERP Code";
                            dr1["h4"] = "Item Name";
                            dr1["h5"] = "Qty";
                            dr1["h6"] = "Batch No";
                            dr1["h7"] = "Pkg";
                            dr1["h8"] = "Unit";
                            dr1["h9"] = "Batch Wise Wt";
                            dr1["header1"] = "1";
                            dr1["header"] = dt7.Rows[i]["header"].ToString();
                            dr1["prodNo"] = dt7.Rows[i]["VCHNUM"].ToString();
                            dr1["ProdDt"] = dt7.Rows[i]["VCHDATE"].ToString();
                            dr1["acode"] = dt7.Rows[i]["acode"].ToString();
                            dr1["Shift"] = dt7.Rows[i]["COL23"].ToString();
                            dr1["mchname"] = dt7.Rows[i]["MCHNAME"].ToString();
                            dr1["Itemcode"] = dt7.Rows[i]["ICODE"].ToString();
                            dr1["Item_name"] = dt7.Rows[i]["INPUT"].ToString();
                            dr1["BatchNo"] = dt7.Rows[i]["BATCH_NO"].ToString();
                            dr1["pkg"] = fgen.make_double(dt7.Rows[i]["BOXES"].ToString());
                            dr1["Qty"] = fgen.make_double(dt7.Rows[i]["COL4"].ToString());
                            dr1["batchwise"] = fgen.make_double(dt7.Rows[i]["BATCH_QTY"].ToString());
                            dr1["Unit"] = dt7.Rows[i]["UNIT"].ToString();
                            dtdummy.Rows.Add(dr1);
                        }
                        DataTable dt8 = new DataTable();
                        if (dt2.Rows.Count > 0)
                        {
                            DataView View3 = new DataView(dt2, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  acode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt8 = View3.ToTable();
                        }
                        for (int i = 0; i < dt8.Rows.Count; i++)   // output
                        {
                            dr1 = dtdummy.NewRow();
                            dr1["h2"] = "Output";
                            dr1["header1"] = "2";
                            dr1["h3"] = "ERP Code";
                            dr1["h4"] = "Item Name";
                            dr1["h5"] = "Qty";
                            dr1["h6"] = "Batch No";
                            dr1["h7"] = "Pkg";
                            dr1["h8"] = "Unit";
                            dr1["h9"] = "Batch Wise Wt";
                            dr1["Incharge"] = dt8.Rows[i]["col_24"].ToString();
                            dr1["prodNo"] = dt8.Rows[i]["VCHNUM"].ToString();
                            dr1["ProdDt"] = dt8.Rows[i]["VCHDATE"].ToString();
                            dr1["BatchNo"] = dt8.Rows[i]["BATCH_NO1"].ToString();
                            dr1["Item_name"] = dt8.Rows[i]["Iname"].ToString();
                            dr1["acode"] = dt8.Rows[i]["acode"].ToString();
                            dr1["unit"] = dt8.Rows[i]["UOM"].ToString();
                            dr1["Qty"] = fgen.make_double(dt8.Rows[i]["COL5_"].ToString());// for total field
                            dr1["batchwise"] = fgen.make_double(dt8.Rows[i]["BATCH_QTY1"].ToString());
                            dr1["Itemcode"] = dt8.Rows[i]["ICODE"].ToString();
                            dtdummy.Rows.Add(dr1);
                        }
                        DataTable dt9 = new DataTable();
                        if (dt3.Rows.Count > 0)
                        {
                            DataView View4 = new DataView(dt3, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  icode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt9 = View4.ToTable();
                        }
                        mq0 = dr1["Incharge"].ToString();
                        double db1 = 0;
                        for (int j = 0; j < dt9.Rows.Count; j++)    //  REJECTION
                        {
                            dr1 = dtdummy.NewRow();
                            dr1["h2"] = "Rejection";
                            dr1["header1"] = "3";
                            dr1["h3"] = "Code";
                            dr1["h4"] = "Name";
                            dr1["h5"] = "Rejection Qty";
                            dr1["prodNo"] = dt9.Rows[j]["VCHNUM"].ToString();
                            dr1["ProdDt"] = dt9.Rows[j]["VCHDATE"].ToString();
                            dr1["Qty"] = fgen.make_double(dt9.Rows[j]["REJn_QTY"].ToString());
                            db1 += fgen.make_double(dt9.Rows[j]["REJn_QTY"].ToString()); // for total field
                            dr1["Item_name"] = dt9.Rows[j]["REJ_NAME"].ToString();
                            dr1["Itemcode"] = dt9.Rows[j]["REJ_CODE"].ToString();
                            dr1["total"] = db1;
                            dr1["Incharge"] = mq0;
                            dtdummy.Rows.Add(dr1);
                        }
                        DataTable dt10 = new DataTable();
                        if (dt4.Rows.Count > 0)
                        {
                            DataView View5 = new DataView(dt4, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  icode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt10 = View5.ToTable();
                        }

                        mq0 = dr1["Incharge"].ToString();
                        double db2 = 0;
                        for (int j = 0; j < dt10.Rows.Count; j++)    // DOWNTIME
                        {
                            dr1 = dtdummy.NewRow();
                            dr1["h2"] = "DownTime";
                            dr1["header1"] = "4";
                            dr1["h3"] = "Code";
                            dr1["h4"] = "Name";
                            dr1["h5"] = "Downtime Mins";
                            dr1["prodNo"] = dt10.Rows[j]["VCHNUM"].ToString();
                            dr1["ProdDt"] = dt10.Rows[j]["VCHDATE"].ToString();
                            dr1["Qty"] = fgen.make_double(dt10.Rows[j]["DOWN_MINS"].ToString());
                            db2 += fgen.make_double(dt10.Rows[j]["DOWN_MINS"].ToString());
                            dr1["Item_name"] = dt10.Rows[j]["DOWN_NAME"].ToString();
                            dr1["Itemcode"] = dt10.Rows[j]["DOWN_CODE"].ToString();
                            dr1["total"] = db2;
                            dr1["Incharge"] = mq0;
                            dtdummy.Rows.Add(dr1);
                        }
                    }
                }
                #endregion

                if (dtdummy.Rows.Count > 0)
                {
                    dtdummy.TableName = "Prepcur";
                    dsRep.Tables.Add(dtdummy);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Corrugation_Prodn", "std_Corrugation_Prodn", dsRep, "");
                }
                #region old code change(call back) after correction
                //// CORRUGATION REPORT
                //mq1 = "";
                //mq1 = barCode;
                //mq1 = mq1.Replace("'", "");
                //DataTable dtdummy = new DataTable();
                //dtdummy.Columns.Add("h2", typeof(string));
                //dtdummy.Columns.Add("h3", typeof(string));
                //dtdummy.Columns.Add("h4", typeof(string));
                //dtdummy.Columns.Add("h5", typeof(string));
                //// headings
                //dtdummy.Columns.Add("h6", typeof(string));
                //dtdummy.Columns.Add("h7", typeof(string));
                //dtdummy.Columns.Add("h8", typeof(string));
                //dtdummy.Columns.Add("h9", typeof(string));
                //dtdummy.Columns.Add("Total", typeof(double));
                //dtdummy.Columns.Add("header1", typeof(string));
                //dtdummy.Columns.Add("header", typeof(string));
                //dtdummy.Columns.Add("acode", typeof(string));
                //dtdummy.Columns.Add("prodNo", typeof(string));
                //dtdummy.Columns.Add("ProdDt", typeof(string));
                //dtdummy.Columns.Add("Incharge", typeof(string));
                //dtdummy.Columns.Add("Shift", typeof(string));
                //dtdummy.Columns.Add("mchname", typeof(string));
                //dtdummy.Columns.Add("SerialNo", typeof(string));
                //dtdummy.Columns.Add("Itemcode", typeof(string));
                //dtdummy.Columns.Add("Item_name", typeof(string));
                //dtdummy.Columns.Add("BatchNo", typeof(string));
                //dtdummy.Columns.Add("pkg", typeof(double));
                //dtdummy.Columns.Add("Qty", typeof(double));
                //dtdummy.Columns.Add("batchwise", typeof(double));
                //dtdummy.Columns.Add("Unit", typeof(string));
                //dtdummy.Columns.Add("output", typeof(double));

                //#region
                ////input
                //SQuery = "SELECT A.SUPCL_BY AS MCHNAME ,A.ENQNO,A.COL23 AS COL23,A.ACODE AS ACODE ,A.VCHNUM AS VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.SRNO,A.ICODE AS  ICODE ,B.INAME AS INPUT,B.UNIT,A.COL3 AS BOXES,A.COL4 AS BATCH_QTY,A.COL5 AS COL5,a.col4  ,A.COL6 AS BATCH_NO  FROM COSTESTIMATE A ,ITEM B WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + mq1 + "' AND A.TYPE='25' ";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); ;
                //// output dt2
                //SQuery = " SELECT A.COL24 AS col_24 ,A.ACODE AS ACODE ,A.VCHNUM AS VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE ,A.SRNO AS SNO,A.ICODE AS ICODE  ,B.INAME AS Iname ,B.UNIT AS UOM,A.COL3 AS BOXES1,A.COL4 AS BATCH_QTY1,A.COL5 AS COL5_,A.COL6 AS BATCH_NO1  FROM COSTESTIMATE A ,ITEM B WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + mq1 + "'  AND TYPE='40'";
                //dt2 = new DataTable();
                //dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ////REJECTION
                //SQuery = "SELECT ACODE AS ACODE,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO, COL1 AS REJ_NAME,COL2 AS REJ_CODE,COL3 AS REJN_QTY,TRIM(ICODE) AS ICODE  FROM INSPVCH WHERE TYPE='45'  AND TRIM(BRANCHCD)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||TRIM(ICODE)='" + mq1 + "'  ORDER BY VCHNUM DESC";
                //dt3 = new DataTable();
                //dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                ////DOWNTIME
                //SQuery = " SELECT ACODE,VCHNUM  ,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO AS SR,COL1 AS down_NAME,COL2 AS DOWN_CODE,COL3 AS DOWN_MINS,TRIM(ICODE) AS ICODE  FROM INSPVCH WHERE TYPE='55'  AND TRIM(BRANCHCD)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||TRIM(ICODE)='" + mq1 + "'  ORDER BY VCHNUM DESC";
                //dt4 = new DataTable();
                //dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //dr1 = null;
                //if (dt.Rows.Count > 0)
                //{
                //    DataView View1 = new DataView(dt);
                //    dt6 = new DataTable();
                //    dt6 = View1.ToTable(true, "VCHNUM", "vchdate", "acode");
                //    foreach (DataRow dr in dt6.Rows)
                //    {
                //        DataView View2 = new DataView(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  acode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //        DataTable dt7 = new DataTable();
                //        dt7 = View2.ToTable();

                //        for (int i = 0; i < dt7.Rows.Count; i++)   // Input
                //        {
                //            dr1 = dtdummy.NewRow();
                //            dr1["h2"] = "Input";
                //            dr1["header1"] = "1";
                //            dr1["h3"] = "ERP Code";
                //            dr1["h4"] = "Item Name";
                //            dr1["h5"] = "Qty";
                //            dr1["h6"] = "Batch No";
                //            dr1["h7"] = "Pkg";
                //            dr1["h8"] = "Unit";
                //            dr1["h9"] = "Batch Wise Wt";
                //            dr1["header1"] = "1";
                //            dr1["header"] = "CORRUGATION";
                //            dr1["prodNo"] = dt7.Rows[i]["VCHNUM"].ToString();
                //            dr1["ProdDt"] = dt7.Rows[i]["VCHDATE"].ToString();
                //            dr1["acode"] = dt7.Rows[i]["acode"].ToString();
                //            dr1["Shift"] = dt7.Rows[i]["COL23"].ToString();
                //            dr1["mchname"] = dt7.Rows[i]["MCHNAME"].ToString();
                //            dr1["Itemcode"] = dt7.Rows[i]["ICODE"].ToString();
                //            dr1["Item_name"] = dt7.Rows[i]["INPUT"].ToString();
                //            dr1["BatchNo"] = dt7.Rows[i]["BATCH_NO"].ToString();
                //            dr1["pkg"] = fgen.make_double(dt7.Rows[i]["BOXES"].ToString());
                //            dr1["Qty"] = fgen.make_double(dt7.Rows[i]["COL4"].ToString());
                //            dr1["batchwise"] = fgen.make_double(dt7.Rows[i]["BATCH_QTY"].ToString());
                //            dr1["Unit"] = dt7.Rows[i]["UNIT"].ToString();
                //            dtdummy.Rows.Add(dr1);
                //        }
                //        DataTable dt8 = new DataTable();
                //        if (dt2.Rows.Count > 0)
                //        {
                //            DataView View3 = new DataView(dt2, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  acode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //            dt8 = View3.ToTable();
                //        }
                //        for (int i = 0; i < dt8.Rows.Count; i++)   // output
                //        {
                //            dr1 = dtdummy.NewRow();
                //            dr1["h2"] = "Output";
                //            dr1["header1"] = "2";
                //            dr1["h3"] = "ERP Code";
                //            dr1["h4"] = "Item Name";
                //            dr1["h5"] = "Qty";
                //            dr1["h6"] = "Batch No";
                //            dr1["h7"] = "Pkg";
                //            dr1["h8"] = "Unit";
                //            dr1["h9"] = "Batch Wise Wt";
                //            dr1["Incharge"] = dt8.Rows[i]["col_24"].ToString();
                //            dr1["prodNo"] = dt8.Rows[i]["VCHNUM"].ToString();
                //            dr1["ProdDt"] = dt8.Rows[i]["VCHDATE"].ToString();
                //            dr1["BatchNo"] = dt8.Rows[i]["BATCH_NO1"].ToString();
                //            dr1["Item_name"] = dt8.Rows[i]["Iname"].ToString();
                //            dr1["acode"] = dt8.Rows[i]["acode"].ToString();
                //            dr1["unit"] = dt8.Rows[i]["UOM"].ToString();
                //            dr1["Qty"] = fgen.make_double(dt8.Rows[i]["COL5_"].ToString());// for total field
                //            dr1["batchwise"] = fgen.make_double(dt8.Rows[i]["BATCH_QTY1"].ToString());
                //            dr1["Itemcode"] = dt8.Rows[i]["ICODE"].ToString();
                //            dtdummy.Rows.Add(dr1);
                //        }
                //        DataTable dt9 = new DataTable();
                //        if (dt3.Rows.Count > 0)
                //        {
                //            DataView View4 = new DataView(dt3, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  icode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //            dt9 = View4.ToTable();
                //        }
                //        mq0 = dr1["Incharge"].ToString();
                //        double db1 = 0;
                //        for (int j = 0; j < dt9.Rows.Count; j++)    //  REJECTION
                //        {
                //            dr1 = dtdummy.NewRow();
                //            dr1["h2"] = "Rejection";
                //            dr1["header1"] = "3";
                //            dr1["h3"] = "Code";
                //            dr1["h4"] = "Name";
                //            dr1["h5"] = "Rejection Qty";
                //            dr1["prodNo"] = dt9.Rows[j]["VCHNUM"].ToString();
                //            dr1["ProdDt"] = dt9.Rows[j]["VCHDATE"].ToString();
                //            dr1["Qty"] = fgen.make_double(dt9.Rows[j]["REJn_QTY"].ToString());
                //            db1 += fgen.make_double(dt9.Rows[j]["REJn_QTY"].ToString()); // for total field
                //            dr1["Item_name"] = dt9.Rows[j]["REJ_NAME"].ToString();
                //            dr1["Itemcode"] = dt9.Rows[j]["REJ_CODE"].ToString();
                //            dr1["total"] = db1;
                //            dr1["Incharge"] = mq0;
                //            dtdummy.Rows.Add(dr1);
                //        }
                //        DataTable dt10 = new DataTable();
                //        if (dt4.Rows.Count > 0)
                //        {
                //            DataView View5 = new DataView(dt4, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "' and  icode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //            dt10 = View5.ToTable();
                //        }

                //        mq0 = dr1["Incharge"].ToString();
                //        double db2 = 0;
                //        for (int j = 0; j < dt10.Rows.Count; j++)    // DOWNTIME
                //        {
                //            dr1 = dtdummy.NewRow();
                //            dr1["h2"] = "DownTime";
                //            dr1["header1"] = "4";
                //            dr1["h3"] = "Code";
                //            dr1["h4"] = "Name";
                //            dr1["h5"] = "Downtime Mins";
                //            dr1["prodNo"] = dt10.Rows[j]["VCHNUM"].ToString();
                //            dr1["ProdDt"] = dt10.Rows[j]["VCHDATE"].ToString();
                //            dr1["Qty"] = fgen.make_double(dt10.Rows[j]["DOWN_MINS"].ToString());
                //            db2 += fgen.make_double(dt10.Rows[j]["DOWN_MINS"].ToString());
                //            dr1["Item_name"] = dt10.Rows[j]["DOWN_NAME"].ToString();
                //            dr1["Itemcode"] = dt10.Rows[j]["DOWN_CODE"].ToString();
                //            dr1["total"] = db2;
                //            dr1["Incharge"] = mq0;
                //            dtdummy.Rows.Add(dr1);
                //        }
                //    }
                //}
                //#endregion
                //if (dtdummy.Rows.Count > 0)
                //{
                //    dtdummy.TableName = "Prepcur";
                //    dsRep.Tables.Add(dtdummy);
                //    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Corrugation_Prodn", "std_Corrugation_Prodn", dsRep, "");
                //}
                //else
                //{
                //    data_found = "N";
                //}
                #endregion
                break;

            case "F40140": //ok kclg 2016-2018
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                header_n = " Production Slip";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt, '" + header_n + "' as header, A.BRANCHCD,A.DESC_,A.VCHNUM,A.INVNO,A.VCHDATE,A.ICODE,A.IQTYIN,A.IQTYOUT,A.NARATION,A.ENT_BY,A.EDT_BY, B.INAME,B.UNIT,B.CPARTNO,B.CINAME  from ivoucher A, ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'DD/MM/YYYY')='" + mq0 + "' ORDER BY B.INAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Production_Slip", "std_Production_Slip", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40142":
                header_n = "Details of Items Produced ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,  A.VCHNUM , A.VCHDATE , to_char(a.vchdate,'dd') as vdd , trim(A.ICODE) AS CODE , A.IQTYIN AS PROD_QTY , B.INAME , trim(B.CPARTNO) as cpartno, (A.IQTYIN*A.IRATE) AS PROD_VALUE  FROM PROD_SHEET A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE)  AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE ='61' AND A.VCHDATE " + xprdRange + " AND A.IQTYIN!='0' ORDER BY A.VCHDATE,A.VCHNUM,B.INAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Items_Produced", "std_Items_Produced", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40146": //THIS IS FOR DIRECT PRINT
            case "F40112": //THIS IS FOR FORM PRINT BUTTON
                mq1 = " ";
                // mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = barCode;
                mq1 = mq1.Replace("'", "");
                header_n = "Sorting & Packing Data";
                SQuery = "SELECT '" + header_n + "' as header, b.iname as name,substr(c.aname,1,11) as party,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.srno,a.col1,a.col2,a.col3 as no,a.col4 as tc,a.col5 as tr,a.itate as rate , round((a.col5/a.col4),2) as rej_perc , (a.col5*a.itate) as rej_val , round((a.col4*a.itate),2) as prod_val ,a.col6,a.col7,a.col8,a.col9,a.col10,a.col11,a.col12,a.col13,a.col14,a.col15,a.col16,a.col17,a.col18,a.col19,a.col20,a.col21,a.col22,a.col23,a.col24,a.col25 from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq1 + "' order by srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.TableName = "Prepcur";
                dsRep.Tables.Add(dt);
                if (dt.Rows.Count > 0)
                {
                    dt2 = new DataTable();
                    dt2.Columns.Add("h1", typeof(string));
                    dt2.Columns.Add("co1", typeof(string));
                    dt2.Columns.Add("co2", typeof(string));
                    dt2.Columns.Add("co3", typeof(string));
                    dt2.Columns.Add("co4", typeof(string));
                    dt2.Columns.Add("co5", typeof(string));
                    dt2.Columns.Add("co6", typeof(string));
                    dt2.Columns.Add("co7", typeof(string));
                    dt2.Columns.Add("co8", typeof(string));
                    dt2.Columns.Add("co9", typeof(string));
                    dt2.Columns.Add("co10", typeof(string));
                    dt2.Columns.Add("co11", typeof(string));
                    dt2.Columns.Add("co12", typeof(string));
                    dt2.Columns.Add("co13", typeof(string));
                    dt2.Columns.Add("co14", typeof(string));
                    dt2.Columns.Add("co15", typeof(string));
                    dt2.Columns.Add("co16", typeof(string));
                    dt2.Columns.Add("co17", typeof(string));
                    dt2.Columns.Add("co18", typeof(string));
                    dt2.Columns.Add("co19", typeof(string));
                    dt2.Columns.Add("co20", typeof(string));

                    mq0 = "select type1,name from type where id='8'  and  rownum<=20  order by type1";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    dr1 = null;
                    int k = 1;
                    dr1 = dt2.NewRow();

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        dr1["h1"] = "Defect Categories";
                        dr1["co" + k] = dt1.Rows[i]["name"].ToString();
                        k++;
                    }
                    dt2.Rows.Add(dr1);
                    dt2.TableName = "Heading";
                    dsRep.Tables.Add(dt2);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Sorting_Packing", "Sorting_Packing", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;
            //5 APPRIL TASK

            case "F40112A":
                mq1 = barCode;
                mq1 = mq1.Replace("'", "");
                //SQuery = "SELECT '" + header_n + "' as header, b.iname as name,substr(c.aname,1,11) as party,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.srno,a.col1,a.col2,a.col3 as no,a.col4 as tc,a.col5 as tr,a.itate as rate , round((a.col5/a.col4),2) as rej_perc , (a.col5*a.itate) as rej_val , round((a.col4*a.itate),2) as prod_val ,a.col6,a.col7,a.col8,a.col9,a.col10,a.col11,a.col12,a.col13,a.col14,a.col15,a.col16,a.col17,a.col18,a.col19,a.col20,a.col21,a.col22,a.col23,a.col24,a.col25 from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq1 + "' order by srno";
                SQuery = "SELECT b.iname as name,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.srno,a.col1,a.col2,a.col4 as qty,a.col6,a.col7,a.col8,a.col9,a.col10,trim(a.enqno) as jord_no,to_char(a.enqdt,'dd/mm/yyyy') as jord_date,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,c.binno,c.mtime,b.unit  from costestimate a,item b,ivoucher c where trim(a.icode)=trim(b.icode) and TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(c.BRANCHCD)||TRIM(c.VCHNUM)||TO_CHAR(c.VCHDATE,'DD/MM/YYYY') and c.type='16' and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq1 + "' order by srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SP_IdTags", "SP_IdTags", dsRep, header_n);
                }

                break;

            case "F40111": //THIS IS FOR FORM PRINT BUTTON
            case "WORK_ORDER_DIRECT": // THIS IS FOR DIRECT PRINT
            case "F35107": // MACHINE PLANNING
                #region Work_order Production (Job Wise Production Entry Form)
                mq1 = "";
                mq1 = barCode;                
                header_n = "Work Order Production Report";
                SQuery = "SELECT '" + header_n + "' as header, C.NAME AS AME ,A.REMARKS,A.REMARKS2,A.ENT_BY, B.INAME,B.CPARTNO,A.PREVCODE,A.ENAME,A.JOB_NO,A.SUBCODE,A.VAR_CODE,A.BRANCHCD,A.TYPE,A.VCHNUM,to_char(A.VCHDATE,'dd/mm/yyyy') as vchdate ,A.ACODE,A.ICODE,A.A1, A.TOTAL,A.UN_MELT,A.A2,A.A4,A.A5 ,A.A6 AS VARIANCE,A.SRNO FROM PROD_SHEET A, ITEM B,TYPE C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(C.TYPE1) AND C.ID='K' AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + mq1 + ") ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.TableName = "Prepcur";
                dsRep.Tables.Add(dt);
                if (dt.Rows.Count > 0)
                {
                    DataTable dtd = new DataTable();
                    dtd.Columns.Add("h1", typeof(string));
                    dtd.Columns.Add("h2", typeof(string));
                    dtd.Columns.Add("h3", typeof(string));
                    dtd.Columns.Add("h4", typeof(string));
                    dtd.Columns.Add("h5", typeof(string));
                    dtd.Columns.Add("h6", typeof(string));
                    dtd.Columns.Add("h7", typeof(string));
                    dtd.Columns.Add("h8", typeof(string));
                    dtd.Columns.Add("h9", typeof(string));
                    dtd.Columns.Add("h10", typeof(string));
                    dtd.Columns.Add("h11", typeof(string));
                    dtd.Columns.Add("h12", typeof(string));
                    dtd.Columns.Add("h13", typeof(string));
                    dtd.Columns.Add("h14", typeof(string));
                    dtd.Columns.Add("h15", typeof(string));
                    dtd.Columns.Add("h16", typeof(string));
                    dtd.Columns.Add("h17", typeof(string));
                    dtd.Columns.Add("h18", typeof(string));
                    dtd.Columns.Add("h19", typeof(string));
                    dtd.Columns.Add("h20", typeof(string));
                    dtd.Columns.Add("h21", typeof(string));
                    dtd.Columns.Add("h22", typeof(string));
                    SQuery = "SELECT TYPE1,SUBSTR(NAME,1,15) AS NAME FROM TYPE WHERE ID='4' AND ROWNUM<=12 ORDER BY TYPE1";
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dr1 = null;
                    if (dt.Rows.Count > 0)
                    {
                        dr1 = dtd.NewRow();
                        dr1["h1"] = "Item Code/ Job.No";
                        dr1["h2"] = "Item Name";
                        dr1["h3"] = "W.O.Qty/Target";
                        dr1["h4"] = "Make Ready (mins)";
                        dr1["h5"] = "Podn Run (mins)";
                        dr1["h6"] = "Prod OK Qty";
                        dr1["h7"] = "Rej Qty";
                        dr1["h8"] = "Toatl Qty";
                        dr1["h9"] = "Variance";
                        dr1["h10"] = "Prodn Time";
                        dr1["h11"] = "D/n Time";
                        int l = 12;
                        for (int j = 0; j < 10; j++)
                        {
                            dr1["h" + l] = dt4.Rows[j]["Name"].ToString();
                            l++;
                        }
                        dtd.Rows.Add(dr1);
                        dtd.TableName = "heading";
                        dsRep.Tables.Add(dtd);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "WO_Prod", "WO_Prod", dsRep, header_n);
                    }
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F40141": //DONE
                header_n = "Production Summary";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(a.BCD),TRIM(B.INAME) AS INAME,SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(NVL(a.iqtyin,0)) AS NET_PRODN, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) AS rej_PERC, round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) AS prod_perc, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode) AND A.BRANCHCD='" + frm_mbr + "' AND  a.type ='61' AND  a.vchdate " + xprdRange + " group by trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.iname),TRIM(b.cpartno) ORDER BY VAR_CODE,ename,iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Production_summary", "std_Production_summary", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40143":  //Production with Rej % Itemwise
                if (frm_cocd == "SPIR" || frm_cocd == "STLC")
                {
                    WB_TABNAME = "prod_sheetK";
                    col1 = "86";
                }
                else
                {
                    WB_TABNAME = "prod_sheet";
                    col1 = "61";
                }
                header_n = "Production with Rej % Itemwise ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, SUM(A.MLT_LOSS) AS REJN_QTY , SUM(A.MLT_LOSS*A.IRATE) AS REJN_VALUE,trim(A.ICODE) AS CODE, SUM(A.IQTYIN) AS PROD_QTY, TRIM(B.INAME) AS INAME , trim(B.CPARTNO) as cpartno , SUM(A.IQTYIN*A.IRATE) AS PROD_VALUE,ROUND(((SUM(A.MLT_LOSS)/SUM(A.IQTYIN))*100),2)  AS REJ_PERC FROM " + WB_TABNAME + " A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND TYPE ='" + col1 + "' AND A.IQTYIN!='0'  AND A.MLT_LOSS!='0' and A.VCHDATE " + xprdRange + "  GROUP BY B.INAME,B.CPARTNO,A.ICODE ORDER BY B.INAME,A.ICODE ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Prod_with_Reje", "std_Prod_with_Reje", dsRep, header_n);
                }

                //header_n = "Production with Rej % Itemwise ";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, SUM(A.MLT_LOSS) AS REJN_QTY , SUM(A.MLT_LOSS*A.IRATE) AS REJN_VALUE,trim(A.ICODE) AS CODE, SUM(A.IQTYIN) AS PROD_QTY, TRIM(B.INAME) AS INAME , trim(B.CPARTNO) as cpartno , SUM(A.IQTYIN*A.IRATE) AS PROD_VALUE,ROUND(((SUM(A.MLT_LOSS)/SUM(A.IQTYIN))*100),2)  AS REJ_PERC FROM PROD_SHEET A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND TYPE='61' AND A.VCHDATE " + xprdRange + " AND A.IQTYIN!='0'  AND A.MLT_LOSS!='0'  GROUP BY B.INAME,B.CPARTNO,A.ICODE ORDER BY B.INAME,A.ICODE";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //if (dt.Rows.Count > 0)
                //{
                //    dt.TableName = "Prepcur";
                //    dsRep.Tables.Add(dt);
                //    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Prod_with_Reje", "std_Prod_with_Reje", dsRep, header_n);
                //}
                else
                {
                    data_found = "N";
                }
                break;

            case "F40150": //DONE
                header_n = "Details of Items Rejected";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,  TRIM(A.VCHNUM) AS VCHNUM ,A.VCHDATE , to_char(a.vchdate,'dd') as vdd , TRIM(A.ICODE) AS CODE , A.MLT_LOSS AS REJN_QTY, B.INAME ,TRIM(B.CPARTNO) AS Part_Code,(A.MLT_LOSS*A.IRATE) AS REJN_VALUE FROM PROD_SHEET A ,ITEM B WHERE TRIM (A.ICODE) = TRIM (B.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND TYPE ='61' AND A.VCHDATE " + xprdRange + " AND A.MLT_LOSS!='0' ORDER BY A.VCHDATE, A.VCHNUM,B.INAME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Items_Rejected", "std_Items_Rejected", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40148":
                header_n = "Production Summary (Month Wise)";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, var_code as shift ,sum(target) as target_t,VCHDATE as vdd , vhd, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND(((SUM(REJN))/(SUM(TOTL_PRODN))*100),2) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from ( select  to_char(a.vchdate,'Month/ yyyy') as vhd ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE ,SUM(a.BCD),SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(a.iqtyin) AS NET_PRODN, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) AS rej_PERC, round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) AS prod_perc, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode)  and a.branchcd='" + frm_mbr + "' AND A.type = '61' and A.vchdate " + xprdRange + " group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(b.cpartno) ORDER BY VAR_CODE) group by var_code,VCHDATE,vhd order by VCHDATE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Mth_Prod_Smry", "std_Mth_Prod_Smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40149":
                header_n = "Production Summary (M/C Wise)";
                //  SQuery = "SELECT '" + header_n + "' as header,  var_code as shift ,ename as machine,sum(target) as target_t,VCHDATE, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND((SUM(REJN))/(SUM(TOTL_PRODN))*100) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from ( select  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(a.BCD),SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(a.iqtyin) AS NET_PRODN, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) AS rej_PERC, round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) AS prod_perc, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode) AND  type like '61' and a.branchcd='" + frm_mbr + "' AND  vchdate "+xprdRange+" group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.cpartno) ORDER BY VAR_CODE,ename ) group by var_code,ename,VCHDATE order by ename,VCHDATE ";
                SQuery = "SELECT '" + header_n + "' as header,  var_code as shift ,TRIM(ename) as machine,sum(target) as target_t,VCHDATE, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND((SUM(REJN))/(SUM(TOTL_PRODN))*100) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from ( select  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(a.BCD),SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(a.iqtyin) AS NET_PRODN, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) AS rej_PERC, round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) AS prod_perc, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode) AND a.branchcd='" + frm_mbr + "'  AND  a.type='61' and  a.vchdate " + xprdRange + " group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.cpartno) ORDER BY VAR_CODE,ename ) group by var_code,ename,VCHDATE order by ename,VCHDATE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Mach_Prod_Smry", "std_Mach_Prod_Smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40139":
                #region
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                header_n = "Item Wise Production Summary";
                // SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt, '" + header_n + "' as header, A.BRANCHCD,A.DESC_,A.VCHNUM,A.INVNO,A.VCHDATE,A.ICODE,A.IQTYIN,A.IQTYOUT,A.NARATION,A.ENT_BY,A.EDT_BY, B.INAME,B.UNIT,B.CPARTNO,B.CINAME  from ivoucher A, ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'DD/MM/YYYY')='" + mq0 + "' ORDER BY B.INAME ";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt ,'" + header_n + "' as header, substr(b.iname,1,30) as iname ,trim(substr(b.cpartno,1,25))  as cpartno,a.branchcd,TRIM(a.type) AS TYPE, to_char(a.vchdate) as vchdate,trim(a.icode) as code , substr(a.icode,1,2) as ode ,NVL(a.iqtyin,0) as tot_prdn ,NVL(a.iqtyout,0) as rejection, ((a.iqtyout/a.iqtyin)*100) as ppm  from ivoucher a , item b where trim(a.icode)= trim(b.icode) and a.branchcd='" + frm_mbr + "' and type ='" + mq0 + "'  and a.vchdate " + xprdRange + " and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%'  and trim(substr(a.iqtyin,'0'))<>'0'order by b.iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Items_Wise_Production", "Items_Wise_Production", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "15250H": // wfinsys_erp id  // ABOX REPORT
            case "F40309": // ok
                cond = "='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1 ";
                mq1 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a,item b  where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "'  and trim(a.icode) " + cond + "  union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from (select type,store,branchcd,vchnum,vchdate,icode,(CASE WHEN (TYPE='36' or TYPE LIKE'1%') THEN 0 ELSE IQTYIN END ) AS IQTYIN,(CASE WHEN (TYPE='36' or TYPE LIKE'1%') and iqtyin!=0 THEN -(IQTYIN) ELSE IQTYOUT END ) AS IQTYOUT FROM IVOUCHER) where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and store='Y' and trim(icode) " + cond + "  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from (select type,store,branchcd,vchnum,vchdate,icode,(CASE WHEN (TYPE='36' or TYPE LIKE'1%') THEN 0 ELSE IQTYIN END ) AS IQTYIN,(CASE WHEN (TYPE='36' or TYPE LIKE'1%') and iqtyin!=0 THEN -(IQTYIN) ELSE IQTYOUT END ) AS IQTYOUT FROM IVOUCHER) where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " and store='Y' and trim(icode) " + cond + "  GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode)";
                mq2 = "SELECT trim(A.cUST_NAME) AS cUST_NAME,nvl(a.fg_len,0) as fg_len,nvl(a.fg_ups,0) as fg_ups,MAX(boxes) as boxes,MAX(REQD) as reqd ,sum(A.iqtyin) as iqtyin ,A.VCHDATE as vchdate,a.icode,sum(A.IQTYOUT) as iqtyout,SUM(RETN) AS RETN,a.invno,a.invdate FROM ( Select cUST_NAME AS cUST_NAME,fg_len,fg_ups,MAX(boxes) as boxes,MAX(REQD) as reqd ,sum(A.iqtyin) as iqtyin,A.VCHDATE as vchdate,a.icode,sum(A.IQTYOUT) as iqtyout,SUM(RETN) AS RETN,a.invno,a.invdate from(select NULL AS cUST_NAME,null as fg_len,null as fg_ups,0 as boxes, 0 AS REQD ,A.iqtyin,A.VCHDATE,a.genum,A.GEDATE,TRIM(a.icode) AS ICODE,A.ACODE,0 AS IQTYOUT,0 as retn,a.Exc_57f4,a.REJ_RW,a.o_deptt,A.DESC_,A.BTCHNO,A.BTCHDT,NULL AS INVNO,a.naration,NULL AS INVDATE,a.unit,a.inspected,nvl(a.tc_no,'-') as tc_no,nvl(c.name,'-') as name,nvl(c.addr2,'-') as prefic from (select type1,name,addr2 from type where id='M') c, ivoucher A where trim(a.type)=trim(c.type1) and a.branchcd='" + frm_mbr + "' and a.store='Y' and A.VCHDATE " + xprdRange + " and trim(a.icode) " + cond + " and  (nvl(a.iqtyout,0)>0 or nvl(a.iqtyin,0)>0) UNION ALL select DISTINCT  A.CUST_NAME AS cUST_NAME,a.fg_len,a.fg_ups ,A.PRD  as boxes,MAX(A.THRT) AS REQD ,0 AS Iqtyin,C.VCHDATE AS INVDATE,null as a,null as b,TRIM(C.ICODE) AS ICODE,C.ACODE,max(C.REELWOUT) AS IQTYOUT,max(REELWIN) AS RETN,null as c,null as d,null as e,null as f,null as g,null as h,TRIM(C.JOB_NO) AS JOB_NO,null as i,C.JOB_DT AS JOB_DT,null as j,null as k,null as tc_no,null as name,null as prefic FROM REELVCH C LEFT OUTER JOIN (SELECT A.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,A.ICODE,TRIM(A.COL9),C.INAME AS CUST_NAME,a.col19  as fg_len,a.col13 as fg_ups,SUM(A.COL7),MAX(A.QTY),ROUND(SUM(A.COL7)/MAX(A.QTY),8) AS REQD,SUM(B.QTY) AS PRD,ROUND(SUM(A.COL7)/MAX(A.QTY),4)*SUM(B.QTY) AS THRT FROM (SELECT BRANCHCD,TYPE,ICODE,VCHNUM,VCHDATE,COL9,COL19,col13,SUM(IS_NUMBER( COL7)) AS COL7,MAX(QTY) AS QTY FROM COSTESTIMATE WHERE branchcd='" + frm_mbr + "' GROUP BY BRANCHCD,TYPE,ICODE,VCHNUM,VCHDATE,COL9,COL19,col13 )A,( SELECT BRANCHCD,TYPE,ICODE,SUM(QTY) AS QTY ,ENQNO,ENQDT FROM COSTESTIMATE WHERE branchcd='" + frm_mbr + "' GROUP BY BRANCHCD,TYPE,ICODE,ENQNO,ENQDT) B,ITEM C WHERE  A.branchcd='" + frm_mbr + "' AND B.TYPE='40' AND A.TYPE='30' AND TRIM(A.VCHNUM)=TRIM(B.ENQNO) AND TRIM(A.VCHDATE)=TRIM(B.ENQDT) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(a.COL9) " + cond + " GROUP BY A.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,A.ICODE,TRIM(A.COL9),C.INAME,a.col19,a.col13) A ON TRIM(C.JOB_NO)=TRIM(A.VCHNUM) AND TRIM(C.JOB_DT)=TO_CHAR(A.VCHDATE,'DD/MM/YYYY')  WHERE   trim(C.icode) " + cond + " AND C.branchcd='" + frm_mbr + "' and substr(C.type,1,1) in ('3','1') AND A.TYPE='30'  and C.vchdate " + xprdRange + " GROUP BY  A.CUST_NAME,a.fg_len,a.fg_ups,C.VCHDATE,TRIM(C.ICODE),TRIM(C.JOB_NO),C.ACODE,C.JOB_DT,C.KCLREELNO,A.PRD ) a group by a.cUST_NAME,a.fg_len ,a.fg_ups ,A.VCHDATE ,a.icode,a.invno,a.invdate ) A WHERE A.vchdate IS NOT NULL group by A.cUST_NAME,a.fg_len,a.fg_ups ,A.VCHDATE,a.icode,a.invno,a.invdate";
                SQuery = "select * from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt ,'Items Detail' as header, to_char(v.vchdate,'dd/MM/yyyy') as Op_Date,(case when v.iqtyin<>0 then 0 else 1 end) ordflg ,to_char(v.vchdate,'yyyyMMdd') as vdd, i.icode,i.iname,I.MILL,I.RCT,I.GSM,I.R_LEN,I.BFACTOR,I.unit,i.cpartno, sum(v.iqtyout) AS IQTYOUT,o.opening as OBAL,o.cl,sum(v.iqtyin) as iqtyin,sum(v.RETN) AS RETN,sum(v.iqtyout)-sum(v.retn) as cons,SUM(v.reqd) as reqd,(case when v.fg_len<>0 then round( (sum(v.iqtyout)-sum(v.retn))/((((I.R_LEN*v.fg_len*I.GSM)/10000*1.45)/1000)/v.fg_ups),2) else 0 end) as boxes,V.CUST_NAME from (" + mq1 + ") O , (select a.icode,a.iname,PUR_UOM AS MILL,MQTY9 AS RCT,OPRATE1 AS R_LEN,OPRATE3 AS GSM,a.cpartno,A.BFACTOR,a.imin,a.imax,a.iord,a.location,a.iweight,a.issu_uom,a.unit,a.binno,0 as iopqty from item a where length(trim(a.icode))>4  order by a.icode) i left outer join(" + mq2 + ") v on  trim(i.icode)=trim(v.icode)  where trim(i.icode)=trim(o.icode)  GROUP BY i.icode,i.unit,i.iname,i.cpartno,O.OPENING,O.CL,to_char(v.vchdate,'dd/MM/yyyy'),to_char(v.vchdate,'yyyyMMdd'),V.CUST_NAME,I.MILL,I.RCT,I.GSM,I.R_LEN,I.BFACTOR,(case when v.iqtyin<>0 then 0 else 1 end),v.fg_len,v.fg_ups ORDER BY ICODE,vdd,ordflg)";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "item_dtl", "item_dtl", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "15250": // wfinsys_erp id  // ABOX REPORT
            case "F40330":
                header_n = "BF Wise,GSM Wise,Reel Wise Production Report";
                SQuery = "select 1 as cc,'" + header_n + "' as header, '" + fromdt + "' as fromdate,'" + todt + "' as todate , a.store_no,trim(a.acode) as acode,trim(a.icode) as icode,trim(b.name) as name,trim(a.iname) as iname,trim(a.invno) as invno,trim(a.type) as type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.wt_md) as gsm,SUM(a.BF12) AS BF12,SUM(a.BF14) AS BF14,SUM(a.BF16) AS BF16,SUM(a.BF18) AS BF18,SUM(a.BF20) AS BF20,SUM(a.BF22) AS BF22,SUM(a.BF24) AS BF25,SUM(a.BF26) AS BF26,SUM(a.BF28) AS BF28,SUM(a.BF30) AS BF30,SUM(a.BF32) AS BF32,SUM(a.BF34) AS BF34,SUM(a.BF12+a.BF14+a.BF16+a.BF18+a.BF20+a.BF22+a.BF24+a.BF26+a.BF28+a.BF30+a.BF32+a.BF34) AS TOT from (select a.store_no,a.type,a.vchnum,a.vchdate,a.invno,a.icode,c.iname,c.wt_md,TRIM(a.acode) as acode,DECODE(TRIM(B.PREFX),'12',sum(a.iqtyin),0) AS BF12,DECODE(TRIM(B.PREFX),'14',sum(a.iqtyin),0) AS BF14,DECODE(TRIM(B.PREFX),'16',sum(a.iqtyin),0) AS BF16,DECODE(TRIM(B.PREFX),'18',sum(a.iqtyin),0) AS BF18,DECODE(TRIM(B.PREFX),'20',sum(a.iqtyin),0) AS BF20,DECODE(TRIM(B.PREFX),'22',sum(a.iqtyin),0) AS BF22,DECODE(TRIM(B.PREFX),'24',sum(a.iqtyin),0) AS BF24,DECODE(TRIM(B.PREFX),'26',sum(a.iqtyin),0) AS BF26,DECODE(TRIM(B.PREFX),'28',sum(a.iqtyin),0) AS BF28,DECODE(TRIM(B.PREFX),'30',sum(a.iqtyin),0) AS BF30,DECODE(TRIM(B.PREFX),'32',sum(a.iqtyin),0) AS BF32,DECODE(TRIM(B.PREFX),'34',sum(a.iqtyin),0) AS BF34,DECODE(TRIM(B.PREFX),'36',sum(a.iqtyin),0) AS BF36 from ivoucher a,item b,item c where SUBSTR(A.ICODE,1,4)=TRIM(B.ICODE) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='16' and a.vchdate " + xprdRange + " group by a.type,a.vchnum,a.vchdate,TRIM(a.acode),substr(A.icode,0,4),c.wt_md,trim(b.prefx),a.icode,c.iname,a.invno,a.store_no) a,type b where b.id='D' and trim(a.acode)=trim(b.type1) and (a.BF12+a.BF14+a.BF16+a.BF18+a.BF20+a.BF22+a.BF24+a.BF26+a.BF28+a.BF30+a.BF32+a.BF34)>0 group by a.wt_md,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.icode,a.iname,a.acode,b.name,a.invno,a.acode,a.store_no order by a.icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "BF_GSM_Reel_Wise_Prod", "BF_GSM_Reel_Wise_Prod", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40346": // MADE BY YOGITA  // WORK ORDER SUMMARY          
                header_n = "Work Order Summary";
                //mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                dt = new DataTable();
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd,trim(a.icode) as icode,A.EMPCODE,nvl(b.iname,'-')  as iname,nvl(b.cpartno,'-') as part,nvl(a1,0) as a1,nvl(a2,0)  as a2,nvl(a.a3,0) as a3,nvl(a.a4,0) as a4,nvl(a.a5,0) as a5,nvl(a.total,0) as total,nvl(a.un_melt,0) as un_melt,a.stage,nvl(a.iqtyout,0) as qty,a.srno,a.mchcode,a.prevstage,a.prevcode,a.shftcode,a.job_no, a.job_Dt,a.ename,d.name as stagename  from prod_sheet a,item b,type d where  trim(a.icode)=trim(b.icode) and trim(a.stage)=trim(d.type1) and d.id='K' and a.branchcd='" + frm_mbr + "' and a.type='90' and a.vchdate " + xprdRange + "  order by vdd,srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "wo_smry", "wo_smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F40344":// CHECK FOR TYPE MADE FOR RIKI
                header_n = "Production Plan";
                #region
                cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                string cond1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").Substring(3, 7);
                // cond1.Substring(4, 6);
                dtm = new DataTable();
                dtm.Columns.Add("ICODE", typeof(string));
                dtm.Columns.Add("INAME", typeof(string));
                dtm.Columns.Add("NAME2", typeof(string));
                dtm.Columns.Add("CPARTNO", typeof(string));
                dtm.Columns.Add("VCHNUM", typeof(string));
                dtm.Columns.Add("VCHDATE", typeof(string));
                dtm.Columns.Add("SALES_PLAN", typeof(double));
                dtm.Columns.Add("STOCK_QTY", typeof(double));
                dtm.Columns.Add("AGST_BUFFR", typeof(double));
                dtm.Columns.Add("PROD_PLAN", typeof(double));
                dtm.Columns.Add("ACHIEVED", typeof(double));
                dtm.Columns.Add("BALANCE", typeof(double));
                dtm.Columns.Add("perct", typeof(double));

                // SQuery = "select trim(a.icode) as icode ,trim(b.iname) as iname ,b.cpartno, trim(c.iname) as name2 , trim(a.vchnum) as vchnum, to_char(a.vchdate,'DD/MM/YYYY') as vchdate , a.a2 as sales_plan , a.a1 as stock_qty ,a.a4 as agst_buffr, a.a6 as prod_plan from prod_sheet a , item b , item c  where trim(a.icode)=trim(b.icode) and trim(substr(a.icode,1,4))=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='10' and a.vchnum='" + cond + "'  order by icode";
                SQuery = "select trim(a.icode) as icode ,trim(b.iname) as iname ,b.cpartno, trim(c.iname) as name2 , trim(a.vchnum) as vchnum, to_char(a.vchdate,'DD/MM/YYYY') as vchdate , a.a2 as sales_plan , a.a1 as stock_qty ,a.a4 as agst_buffr, a.a6 as prod_plan from prod_sheet a , item b , item c  where trim(a.icode)=trim(b.icode) and trim(substr(a.icode,1,4))=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + cond + "'  order by icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                SQuery = "select SUM(IQTYIN) AS ACHIEVED,trim(icode) as icode from ivoucher where branchcd='" + frm_mbr + "' and type in('15','16','17','18') and store ='Y' and TO_CHAR(VCHDATE,'MM/YYYY')='" + cond1 + "' group by icode";
                //SQuery = "select IQTYIN AS ACHIEVED,icode from ivoucher where branchcd='00' and type in('15','16','17','18') and store ='Y' and TO_CHAR(VCHDATE,'MM/YYYY')='08/2017'";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    DataView View1 = new DataView(dt);
                    dt6 = new DataTable();
                    dt6 = View1.ToTable(true, "icode");
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataView View2 = new DataView(dt, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt7 = new DataTable();
                        dt7 = View2.ToTable();
                        for (int i = 0; i < dt7.Rows.Count; i++)
                        {
                            ded1 = ""; ded2 = ""; db1 = 0; db2 = 0; db3 = 0; db4 = 0;
                            dr1 = dtm.NewRow();
                            dr1["ICODE"] = dt7.Rows[i]["ICODE"].ToString();
                            dr1["INAME"] = dt7.Rows[i]["INAME"].ToString();
                            dr1["NAME2"] = dt7.Rows[i]["NAME2"].ToString();
                            dr1["CPARTNO"] = dt7.Rows[i]["CPARTNO"].ToString();
                            dr1["VCHNUM"] = dt7.Rows[i]["VCHNUM"].ToString();
                            dr1["VCHDATE"] = dt7.Rows[i]["VCHDATE"].ToString();
                            dr1["SALES_PLAN"] = dt7.Rows[i]["SALES_PLAN"].ToString();
                            dr1["STOCK_QTY"] = dt7.Rows[i]["STOCK_QTY"].ToString();
                            dr1["AGST_BUFFR"] = dt7.Rows[i]["AGST_BUFFR"].ToString();
                            dr1["PROD_PLAN"] = dt7.Rows[i]["PROD_PLAN"].ToString();
                            dr1["ACHIEVED"] = fgen.make_double(fgen.seek_iname_dt(dt1, "ICODE='" + dr1["ICODE"].ToString().Trim() + "'", "ACHIEVED"));
                            db1 = fgen.make_double(dr1["PROD_PLAN"].ToString().Trim());
                            db2 = fgen.make_double(dr1["ACHIEVED"].ToString().Trim());
                            db3 = db1 - db2;
                            db4 = db2 / db1 * 100;
                            dr1["BALANCE"] = db3;
                            dr1["perct"] = db4;
                            dtm.Rows.Add(dr1);
                        }
                    }
                }
                if (dtm.Rows.Count > 0)
                {
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Production_Plan", "Production_Plan", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F40345":// CHECK FOR TYPE MADE FOR RIKI
                header_n = "Sales Plan";
                #region
                cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                cond1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").Substring(3, 7);
                //  SQuery = "SELECT '" + fromdt + "' as frmdt ,'" + todt + "' as todt,'" + header_n + "' as header ,A.VCHNUM,to_char(A.VCHDATE,'Month/YYYY') as vchdate ,A.ICODE ,a.cust,A.TARGET,A.TARGET*B.IRATE AS TARGET_VALUE , B.INAME,C.INAME AS NAME2 ,B.UNIT, B.IRATE,B.CPARTNO,B.CINAME    from mthlyplan A , ITEM B,ITEM C   where TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,4)) =TRIM(C.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE ='10'  AND A.VCHNUM='" + cond + "' and A.vchdate between to_date ('01/04/2017','DD/MM/YYYY') and to_date('31/03/2018','DD/MM/YYYY')  ORDER BY A.ICODE";
                SQuery = "SELECT '" + fromdt + "' as frmdt ,'" + todt + "' as todt,'" + header_n + "' as header ,A.VCHNUM,to_char(A.VCHDATE,'Month/YYYY') as vchdate ,A.ICODE ,a.cust,A.TARGET,A.TARGET*B.IRATE AS TARGET_VALUE , B.INAME,C.INAME AS NAME2 ,B.UNIT, B.IRATE,B.CPARTNO,B.CINAME    from mthlyplan A , ITEM B,ITEM C   where TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,4)) =TRIM(C.ICODE) AND a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + cond + "' ORDER BY A.ICODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Sales_Plan", "Sales_Plan", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F40118":
                header_n = "Label Report";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, a.branchcd,a.type,a.vchnum,a.acode,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.col1) as col1,trim(a.col2) as col2,a.col3,a.col4,a.qty1,a.qty2,a.qty3,a.qty4,a.obsv1,a.obsv2,a.obsv3,a.obsv4,a.obsv5,a.obsv6,a.obsv7,a.obsv8,a.obsv9,a.obsv10 as sales_exec,(case when nvl(a.acode,'-')!='-' then f.aname else a.obsv11 end) as cust_name,a.obsv12 as Tag_name,a.obsv13 as paper_gsm,a.obsv14 as sheet_size,a.obsv15 as paper_size ,a.contplan as colors,a.srno from inspvch a left join famst f on trim(a.acode)=trim(f.acode) where a.branchcd||a.type||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Label_Report", "Label_Report", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F35242":
            case "F40215":
                #region
                dt = new DataTable(); dt1 = new DataTable();
                dt1.Columns.Add(new DataColumn("fstr", typeof(string)));
                dt1.Columns.Add(new DataColumn("ticketno", typeof(string)));
                dt1.Columns.Add(new DataColumn("ticketdt", typeof(string)));
                dt1.Columns.Add(new DataColumn("lineno", typeof(string)));
                dt1.Columns.Add(new DataColumn("shift", typeof(string)));
                dt1.Columns.Add(new DataColumn("icode", typeof(string)));
                dt1.Columns.Add(new DataColumn("acode", typeof(string)));
                dt1.Columns.Add(new DataColumn("customer", typeof(string)));
                dt1.Columns.Add(new DataColumn("mrp", typeof(double)));
                dt1.Columns.Add(new DataColumn("product", typeof(string)));
                dt1.Columns.Add(new DataColumn("batchno", typeof(string)));
                dt1.Columns.Add(new DataColumn("pkd", typeof(string)));
                dt1.Columns.Add(new DataColumn("ap_no", typeof(string)));
                dt1.Columns.Add(new DataColumn("rmk", typeof(string)));
                dt1.Columns.Add(new DataColumn("size", typeof(string)));
                dt1.Columns.Add(new DataColumn("time", typeof(string)));
                dt1.Columns.Add(new DataColumn("total_ctn", typeof(double)));
                dt1.Columns.Add(new DataColumn("unit_ctn", typeof(double)));
                dt1.Columns.Add(new DataColumn("total_qty", typeof(double)));
                dt1.Columns.Add(new DataColumn("Chemist", typeof(string)));
                dt1.Columns.Add(new DataColumn("Line_Leader", typeof(string)));

                cond = "and trim(a.branchcd)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY')||trim(a.acode) in (" + barCode + ")";
                // SQuery = "SELECT DISTINCT trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(acode) as fstr,A.VCHNUM AS TICKETNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS TICKET_DT,a.enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,A.COL6 AS BTCHNO,nvl(A.QTY,0) as qty,A.ICODE,B.INAME AS PRODUCT,A.COL23 AS SHIFT,A.COL24 AS LINE_LEADER,A.COL25 AS LINENO,a.comments2 as chemist,SUBSTR(TRIM(A.comments3),1,10) AS PKD,nvl(B.PACKSIZE,0) AS UNIT_CTN,nvl(b.mrp,0) as mrp FROM COSTESTIMATE A ,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and a.type='40' " + cond + "  ORDER BY A.VCHNUM";
                SQuery = "SELECT DISTINCT trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(acode) as fstr,A.VCHNUM AS TICKETNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS TICKET_DT,b.name as empname,a.enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,A.COL6 AS BTCHNO,nvl(A.QTY,0) as qty,A.ICODE,B.INAME AS PRODUCT,A.COL23 AS SHIFT,A.COL24 AS LINE_LEADER,A.COL25 AS LINENO,a.comments2 as chemist,SUBSTR(TRIM(A.comments3),1,10) AS PKD,nvl(B.PACKSIZE,0) AS UNIT_CTN,nvl(b.mrp,0) as mrp FROM COSTESTIMATE A left join empmas b on  substr(trim(a.comments2),1,6)=trim(b.empcode),ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and a.type='40' " + cond + "  ORDER BY A.VCHNUM";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//main dt

                mq1 = "SELECT distinct a.col25 as time,trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr FROM COSTESTIMATE A WHERE a.TYPE='25' " + cond + "";
                dt2 = new DataTable();//TYPE 25 DT
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                mq3 = "SELECT trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr,a.cust_ref as ap_no,a.remarks  FROM PROD_SHEET a WHERE a.TYPE='86' and trim(a.branchcd)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY')||trim(a.icode) in (" + barCode + ") ORDER BY a.VCHNUM";
                dt4 = new DataTable();
                dt4 = fgen.getdata(frm_qstr, frm_cocd, mq3);//prod_sheet table for ap no and remark

                mq4 = "select trim(a.acode) as acode,trim(b.aname) as cust,trim(a.icode) as icode,a.desc_,a.cdrgno from somas a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '4%' and orddt " + DateRange + "";
                dt5 = new DataTable();
                dt5 = fgen.getdata(frm_qstr, frm_cocd, mq4);//somas table for party code
                double db = 0;
                //===============================================================               
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr1 = dt1.NewRow();
                    db = 0; db1 = 0; db2 = 0; mq1 = ""; mq2 = "";
                    mq1 = dt.Rows[i]["LINE_LEADER"].ToString().Trim();
                    mq2 = mq1.Split(' ')[0].ToString();
                    dr1["Line_Leader"] = mq2;
                    dr1["Chemist"] = dt.Rows[i]["empname"].ToString().Trim();
                    dr1["pkd"] = dt.Rows[i]["PKD"].ToString().Trim();
                    dr1["fstr"] = dt.Rows[i]["fstr"].ToString().Trim();
                    dr1["ticketno"] = dt.Rows[i]["TICKETNO"].ToString().Trim();
                    dr1["ticketdt"] = dt.Rows[i]["TICKET_DT"].ToString().Trim();
                    dr1["lineno"] = dt.Rows[i]["LINENO"].ToString().Trim();
                    dr1["shift"] = dt.Rows[i]["SHIFT"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                    dr1["ap_no"] = fgen.seek_iname_dt(dt4, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "ap_no");
                    dr1["rmk"] = fgen.seek_iname_dt(dt4, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "remarks");
                    dr1["acode"] = fgen.seek_iname_dt(dt5, "cdrgno='" + dr1["ap_no"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "'", "acode");
                    dr1["customer"] = fgen.seek_iname_dt(dt5, "cdrgno='" + dr1["ap_no"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "'", "cust");
                    dr1["mrp"] = fgen.make_double(dt.Rows[i]["MRP"].ToString().Trim());
                    dr1["product"] = dt.Rows[i]["PRODUCT"].ToString().Trim();
                    dr1["batchno"] = dt.Rows[i]["BTCHNO"].ToString().Trim();
                    dr1["size"] = "";// dt.Rows[i][""].ToString().Trim();                   
                    dr1["time"] = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "time");
                    ///=============================================   
                    dr1["total_qty"] = fgen.make_double(dt.Rows[i]["QTY"].ToString().Trim());
                    db = fgen.make_double(dt.Rows[i]["QTY"].ToString().Trim());
                    dr1["unit_ctn"] = fgen.make_double(dt.Rows[i]["UNIT_CTN"].ToString().Trim());
                    db1 = fgen.make_double(dt.Rows[i]["UNIT_CTN"].ToString().Trim());
                    if (db1 > 0)
                    {
                        db2 = db / db1;
                    }
                    else
                    {
                        db2 = 0;
                    }
                    dr1["total_ctn"] = db2;
                    dt1.Rows.Add(dr1);
                }
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "Prepcur";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ACCR_Stkr", "ACCR_Stkr", dsRep, "-", "Y");
                }
                #endregion
                break;
            case "F40106S":
            case "F39102S":
            case "F40107S":
                #region WIP Sticker
                if (ind_Ptype == "01" || ind_Ptype == "12")
                {
                    WB_TABNAME = "INSPVCHK";
                    WB_TABNAME2 = "COSTESTIMATEK";
                }

                SQuery = "Select a.branchcd,trim(e.kclreelno) as fstr,e.kclreelno,e.REELWIN,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,a.tpt_names,a.mr_gdate from ivoucher a,famst b ,item c,type d,reelvch e where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_chaR(a.vchdate,'dd/mm/yyyy')=e.branchcd||e.type||trim(e.vchnum)||to_chaR(e.vchdate,'dd/mm/yyyy') and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                SQuery = "SELECT trim(A.ICODE)||trim(A.COL6) AS FSTR,A.ICODE,B.INAME,A.COL6 AS BTCHNO,A.VCHNUM,TO_cHAR(a.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ENQNO,A.ENQDT,A.COL21,A.COL22,A.COL23,A.ENT_BY,A.ENT_dT,A.COL3,A.COL4,A.COL5,A.SRNO FROM " + WB_TABNAME2 + " A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "'AND A.TYPE='40' AND TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + barCode + "' ORDER BY A.VCHNUM,A.SRNO";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);
                    frm_rptName = "wip_stk";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "wip_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F40356":

                //do run_query with "select distinct a.icode,a.maintdt,a.btchdt,a.rejqty from inspmst a where a.branchcd='&mbr' and a.type='70' order by a.icode ","pplan"
                //do run_query with "select b.iname,b.cpartno,a.branchcd||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,b.iweight,a.* from prod_Sheet a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='&mbr' and a.type='88' and a.vchdate "+ prdrange+" order by a.vchdate,a.vchnum","prodn"

                //do run_query with "select distinct branchcd||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr,num1,scrp1,scrp2,time1,time2 from costestimate where branchcd='&mbr' and type='40' and vchdate "+ prdrange+"   ","outp"

                //select a.PREVCODE as shift_name,a.iqtyin*a.iweight as prod_Wt,b.rejqty as ups,round(a.iqtyout,0) as Mplan_qty,round((a.iqtyin/b.rejqty)+(a.mlt_loss/b.rejqty),0) as Mprod_qty, a.mlt_loss as Mrejn_qty,round(a.iqtyin,0) as Mok_qty,b.maintdt,b.btchdt, round((val(b.btchdt)/100)*round((a.iqtyin/b.rejqty)+(a.mlt_loss/b.rejqty),0),0) as linmtr , a.* from prodn a,pplan b where allt(A.icode)=allt(B.icode) into cursor c1

                //sele c1

                //select b.scrp1+b.scrp2+b.time1+b.time2 as Reel_wstg,b.num1 as pap_use,a.* from c1 a,outp b where allt(A.fstr)==allt(B.fstr) into cursor c1 order by a.vchdate,a.ename,a.shift_name,a.ent_Dt,a.mcstart
                //sele c1

                break;

            case "F40216":
                #region Corrugation dpr
                header_n = "Corrugation Production Report(DPR Report)";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,to_char(a.vchdate,'dd/mm/yyyy') as vcd,b.scrp1+b.scrp2+b.time1+b.time2 as Reel_wstg,b.num1 as pap_use,is_number(a.tslot) as t_slot,a.* from (select a.PREVCODE as shift_name,a.iqtyin*a.iweight as prod_Wt,b.rejqty as ups,round(a.iqtyout,0) as Mplan_qty,round((a.iqtyin/b.rejqty)+(a.mlt_loss/b.rejqty),0) as Mprod_qty, a.mlt_loss as Mrejn_qty,round(a.iqtyin,0) as Mok_qty,b.maintdt,b.btchdt, round(((b.btchdt)/100)*round((a.iqtyin/b.rejqty)+(a.mlt_loss/b.rejqty),0),0) as linmtr, a.* from (select b.iname,b.cpartno,a.branchcd||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,b.iweight,a.* from prod_Sheet a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='88' and a.vchdate " + xprdRange + " order by a.vchdate,a.vchnum) a,(select distinct a.icode,a.maintdt,a.btchdt,a.rejqty from inspmst a where a.branchcd='" + frm_mbr + "' and a.type='70' order by a.icode) b where trim(A.icode)=trim(B.icode) ) a,(select distinct branchcd||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr,num1,scrp1,scrp2,time1,time2 from costestimate where branchcd='" + frm_mbr + "' and type='40' and vchdate " + xprdRange + ") b where trim(A.fstr)=trim(B.fstr)";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "corr_dpr_pcon", "corr_dpr_pcon", dsRep, header_n);
                }
                #endregion
                break;

            case "F40217":
                #region Rejection Report DayWise(Corrugation)
                header_n = "31 Day Rejn Report(First Stage)";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                year = "";
                if (Convert.ToInt32(mq0) > 3)
                {
                    year = frm_myear;
                    mq1 = mq0 + "/" + year;
                }
                else
                {
                    int j = Convert.ToInt32(frm_myear) + 1;
                    year = Convert.ToString(j);
                    mq1 = mq0 + "/" + year;
                }
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname from mths where mthnum='" + mq0 + "'", "mthname");
                SQuery = "SELECT '" + header_n + "' as header,'" + mq1 + "' as mth,'" + mq3 + "' as mthname,'" + year + "' as year_,A.ICODE,B.INAME,B.CPARTNO,sum(a.day1+a.day2+a.day3+a.day4+a.day5+a.day6+a.day7+a.day8+a.day9+a.day10+a.day11+a.day12+a.day13+a.day14+a.day15+a.day16+a.day17+a.day18+a.day19+a.day20+a.day21+a.day22+a.day23+a.day24+a.day25+a.day26+a.day27+a.day28+a.day29+a.day30+a.day31) as tot,SUM(a.DAY1) AS day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day29,sum(a.day30) as day_30,sum(a.day31) as day_31 from (SELECT ICODE,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TO_CHAR(VCHDATE,'YYYY/MM/DD') AS VDD,(Case when to_char(vchdate,'dd')='01' then IS_NUMBER(COL3) else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then IS_NUMBER(COL3) else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then IS_NUMBER(COL3) else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then IS_NUMBER(COL3) else 0 end) as DAY4,(Case when to_char(vchdate,'dd')='05' then IS_NUMBER(COL3) else 0 end) as DAY5,(Case when to_char(vchdate,'dd')='06' then IS_NUMBER(COL3) else 0 end) as DAY6,(Case when to_char(vchdate,'dd')='07' then IS_NUMBER(COL3) else 0 end) as DAY7,(Case when to_char(vchdate,'dd')='08' then IS_NUMBER(COL3) else 0 end) as DAY8,(Case when to_char(vchdate,'dd')='09' then IS_NUMBER(COL3) else 0 end) as DAY9,(Case when to_char(vchdate,'dd')='10' then IS_NUMBER(COL3) else 0 end) as DAY10,(Case when to_char(vchdate,'dd')='11' then IS_NUMBER(COL3) else 0 end) as DAY11,(Case when to_char(vchdate,'dd')='12' then IS_NUMBER(COL3) else 0 end) as DAY12,(Case when to_char(vchdate,'dd')='13' then IS_NUMBER(COL3) else 0 end) as DAY13,(Case when to_char(vchdate,'dd')='14' then IS_NUMBER(COL3) else 0 end) as DAY14,(Case when to_char(vchdate,'dd')='15' then IS_NUMBER(COL3) else 0 end) as DAY15,(Case when to_char(vchdate,'dd')='16' then IS_NUMBER(COL3) else 0 end) as DAY16,(Case when to_char(vchdate,'dd')='17' then IS_NUMBER(COL3) else 0 end) as DAY17,(Case when to_char(vchdate,'dd')='18' then IS_NUMBER(COL3) else 0 end) as DAY18,(Case when to_char(vchdate,'dd')='19' then IS_NUMBER(COL3) else 0 end) as DAY19,(Case when to_char(vchdate,'dd')='20' then IS_NUMBER(COL3) else 0 end) as DAY20,(Case when to_char(vchdate,'dd')='21' then IS_NUMBER(COL3) else 0 end) as DAY21,(Case when to_char(vchdate,'dd')='22' then IS_NUMBER(COL3) else 0 end) as DAY22,(Case when to_char(vchdate,'dd')='23' then IS_NUMBER(COL3) else 0 end) as DAY23,(Case when to_char(vchdate,'dd')='24' then IS_NUMBER(COL3) else 0 end) as DAY24,(Case when to_char(vchdate,'dd')='25' then IS_NUMBER(COL3) else 0 end) as DAY25,(Case when to_char(vchdate,'dd')='26' then IS_NUMBER(COL3) else 0 end) as DAY26,(Case when to_char(vchdate,'dd')='27' then IS_NUMBER(COL3) else 0 end) as DAY27,(Case when to_char(vchdate,'dd')='28' then IS_NUMBER(COL3) else 0 end) as DAY28,(Case when to_char(vchdate,'dd')='29' then IS_NUMBER(COL3) else 0 end) as DAY29,(Case when to_char(vchdate,'dd')='30' then IS_NUMBER(COL3) else 0 end) as DAY30,(Case when to_char(vchdate,'dd')='31' then IS_NUMBER(COL3) else 0 end) as DAY31 FROM INSPVCH  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='45' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq1 + "') a,item b where trim(a.icode)=trim(b.icode) group by  A.ICODE,B.INAME,B.CPARTNo";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "day_wise_rej_pcon", "day_wise_rej_pcon", dsRep, header_n);
                }
                #endregion
                break;

            case "F40218":
                #region Rejection Report Reason Wise (Corrugation)
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                header_n = "Machine Wise Rejn Report";
                mq0 = ""; mq1 = "";
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='RJC61' and rownum<12 order by code");

                for (int i = 0; i < 12; i++)
                {
                    dt1.Columns.Add("HH" + i + "", typeof(string));
                }
                ///fill heading names
                dr1 = dt1.NewRow();
                for (int i = 0; i < 12; i++)
                {
                    if (i < dt.Rows.Count)
                    {
                        dr1["HH" + i + ""] = dt.Rows[i]["name"].ToString().Trim();
                    }
                    else
                    {
                        dr1["HH" + i + ""] = "Blank" + i + "";
                    }
                }
                dt1.Rows.Add(dr1);
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, vdd,machine,vdd1 as mth,substr(mthname,1,4) as mthname,sum(hh0+hh1+hh2+hh3+hh4+hh5+hh6+hh7+hh8+hh9+hh10+hh11) as tot,sum(hh0) as hh0,sum(hh1) as hh1,sum(hh2) as hh2,sum(hh3) as hh3,sum(hh4) as hh4,sum(hh5) as hh5,sum(hh6) as hh6,sum(hh7) as hh7,sum(hh8) as hh8,sum(hh9) as hh9,sum(hh10) as hh10,sum(hh11) as hh11,sum(hh12) as hh12 from (select TO_CHAR(a.vchdate,'mm/yyyy') as vdd, a.title as machine,TO_CHAR(a.vchdate,'mm') as vdd1,b.mthname,decode(col2,'100',is_number(col3),0) as hh0,decode(col2,'101',is_number(col3),0) as hh1,decode(col2,'102',is_number(col3),0) as hh2,decode(col2,'103',is_number(col3),0) as hh3,decode(col2,'104',is_number(col3),0) as hh4,decode(col2,'105',is_number(col3),0) as hh5,decode(col2,'106',is_number(col3),0) as hh6,decode(col2,'107',is_number(col3),0) as hh7,decode(col2,'108',is_number(col3),0) as hh8,decode(col2,'109',is_number(col3),0) as hh9,decode(col2,'110',is_number(col3),0) as hh10,decode(col2,'111',is_number(col3),0) as hh11,decode(col2,'112',is_number(col3),0) as hh12   from inspvch a,mths b where to_char(a.vchdate,'mm')=trim(b.mthnum) and  a.branchcd='" + frm_mbr + "' and a.type='45' AND a.VCHDATE  " + xprdRange + " AND a.ACODE LIKE '6%' ) group by vdd,machine,vdd1 ,mthname   ORDER BY vdd1";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt2.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(dt2);
                    dt1.TableName = "headings";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "reason_corrug_PCON", "reason_corrug_PCON", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F40219":
                #region DownTime Report DayWise(Corrugation)==icon name
                header_n = "31 Day DownTime Report(First Stage)";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                year = "";
                if (Convert.ToInt32(mq0) > 3)
                {
                    year = frm_myear;
                    mq1 = mq0 + "/" + year;
                }
                else
                {
                    int j = Convert.ToInt32(frm_myear) + 1;
                    year = Convert.ToString(j);
                    mq1 = mq0 + "/" + year;
                }
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname from mths where mthnum='" + mq0 + "'", "mthname");
                SQuery = "SELECT '" + header_n + "' as header,'" + mq1 + "' as mth,'" + mq3 + "' as mthname,'" + year + "' as year_,A.ICODE,B.INAME,B.CPARTNO,sum(a.day1+a.day2+a.day3+a.day4+a.day5+a.day6+a.day7+a.day8+a.day9+a.day10+a.day11+a.day12+a.day13+a.day14+a.day15+a.day16+a.day17+a.day18+a.day19+a.day20+a.day21+a.day22+a.day23+a.day24+a.day25+a.day26+a.day27+a.day28+a.day29+a.day30+a.day31) as tot,SUM(a.DAY1) AS day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day29,sum(a.day30) as day_30,sum(a.day31) as day_31 from (SELECT ICODE,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TO_CHAR(VCHDATE,'YYYY/MM/DD') AS VDD,(Case when to_char(vchdate,'dd')='01' then IS_NUMBER(COL3) else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then IS_NUMBER(COL3) else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then IS_NUMBER(COL3) else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then IS_NUMBER(COL3) else 0 end) as DAY4,(Case when to_char(vchdate,'dd')='05' then IS_NUMBER(COL3) else 0 end) as DAY5,(Case when to_char(vchdate,'dd')='06' then IS_NUMBER(COL3) else 0 end) as DAY6,(Case when to_char(vchdate,'dd')='07' then IS_NUMBER(COL3) else 0 end) as DAY7,(Case when to_char(vchdate,'dd')='08' then IS_NUMBER(COL3) else 0 end) as DAY8,(Case when to_char(vchdate,'dd')='09' then IS_NUMBER(COL3) else 0 end) as DAY9,(Case when to_char(vchdate,'dd')='10' then IS_NUMBER(COL3) else 0 end) as DAY10,(Case when to_char(vchdate,'dd')='11' then IS_NUMBER(COL3) else 0 end) as DAY11,(Case when to_char(vchdate,'dd')='12' then IS_NUMBER(COL3) else 0 end) as DAY12,(Case when to_char(vchdate,'dd')='13' then IS_NUMBER(COL3) else 0 end) as DAY13,(Case when to_char(vchdate,'dd')='14' then IS_NUMBER(COL3) else 0 end) as DAY14,(Case when to_char(vchdate,'dd')='15' then IS_NUMBER(COL3) else 0 end) as DAY15,(Case when to_char(vchdate,'dd')='16' then IS_NUMBER(COL3) else 0 end) as DAY16,(Case when to_char(vchdate,'dd')='17' then IS_NUMBER(COL3) else 0 end) as DAY17,(Case when to_char(vchdate,'dd')='18' then IS_NUMBER(COL3) else 0 end) as DAY18,(Case when to_char(vchdate,'dd')='19' then IS_NUMBER(COL3) else 0 end) as DAY19,(Case when to_char(vchdate,'dd')='20' then IS_NUMBER(COL3) else 0 end) as DAY20,(Case when to_char(vchdate,'dd')='21' then IS_NUMBER(COL3) else 0 end) as DAY21,(Case when to_char(vchdate,'dd')='22' then IS_NUMBER(COL3) else 0 end) as DAY22,(Case when to_char(vchdate,'dd')='23' then IS_NUMBER(COL3) else 0 end) as DAY23,(Case when to_char(vchdate,'dd')='24' then IS_NUMBER(COL3) else 0 end) as DAY24,(Case when to_char(vchdate,'dd')='25' then IS_NUMBER(COL3) else 0 end) as DAY25,(Case when to_char(vchdate,'dd')='26' then IS_NUMBER(COL3) else 0 end) as DAY26,(Case when to_char(vchdate,'dd')='27' then IS_NUMBER(COL3) else 0 end) as DAY27,(Case when to_char(vchdate,'dd')='28' then IS_NUMBER(COL3) else 0 end) as DAY28,(Case when to_char(vchdate,'dd')='29' then IS_NUMBER(COL3) else 0 end) as DAY29,(Case when to_char(vchdate,'dd')='30' then IS_NUMBER(COL3) else 0 end) as DAY30,(Case when to_char(vchdate,'dd')='31' then IS_NUMBER(COL3) else 0 end) as DAY31 FROM INSPVCH  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='55' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq1 + "') a,item b where trim(a.icode)=trim(b.icode) group by  A.ICODE,B.INAME,B.CPARTNo";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "day_wise_rej_pcon", "day_wise_rej_pcon", dsRep, header_n);
                }
                #endregion
                break;

            case "F40220":
                #region DownTime Report Reason Wise (Corrugation)
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                header_n = "Machine Down Time Report(In Mins)";
                mq0 = ""; mq1 = "";
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='DTC61' and rownum<13 order by code");

                for (int i = 0; i < 12; i++)
                {
                    dt1.Columns.Add("HH" + i + "", typeof(string));
                }
                ///fill heading names
                dr1 = dt1.NewRow();
                for (int i = 0; i < 12; i++)
                {
                    if (i < dt.Rows.Count)
                    {
                        dr1["HH" + i + ""] = dt.Rows[i]["name"].ToString().Trim();
                    }
                    else
                    {
                        dr1["HH" + i + ""] = "Blank" + i + "";
                    }
                }
                dt1.Rows.Add(dr1);
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, vdd,machine,vdd1 as mth,substr(mthname,1,4) as mthname,sum(hh0+hh1+hh2+hh3+hh4+hh5+hh6+hh7+hh8+hh9+hh10+hh11) as tot,sum(hh0) as hh0,sum(hh1) as hh1,sum(hh2) as hh2,sum(hh3) as hh3,sum(hh4) as hh4,sum(hh5) as hh5,sum(hh6) as hh6,sum(hh7) as hh7,sum(hh8) as hh8,sum(hh9) as hh9,sum(hh10) as hh10,sum(hh11) as hh11,sum(hh12) as hh12 from (select TO_CHAR(a.vchdate,'mm/yyyy') as vdd, a.title as machine,TO_CHAR(a.vchdate,'mm') as vdd1,b.mthname,decode(col2,'100',is_number(col3),0) as hh0,decode(col2,'101',is_number(col3),0) as hh1,decode(col2,'102',is_number(col3),0) as hh2,decode(col2,'103',is_number(col3),0) as hh3,decode(col2,'104',is_number(col3),0) as hh4,decode(col2,'105',is_number(col3),0) as hh5,decode(col2,'106',is_number(col3),0) as hh6,decode(col2,'107',is_number(col3),0) as hh7,decode(col2,'108',is_number(col3),0) as hh8,decode(col2,'109',is_number(col3),0) as hh9,decode(col2,'110',is_number(col3),0) as hh10,decode(col2,'111',is_number(col3),0) as hh11,decode(col2,'112',is_number(col3),0) as hh12   from inspvch a,mths b where to_char(a.vchdate,'mm')=trim(b.mthnum) and  a.branchcd='" + frm_mbr + "' and a.type='55' AND a.VCHDATE  " + xprdRange + " AND a.ACODE LIKE '6%' ) group by vdd,machine,vdd1 ,mthname   ORDER BY vdd1";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt2.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(dt2);
                    dt1.TableName = "headings";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "reason_corrug_PCON", "reason_corrug_PCON", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
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
            //conv_pdf(data_set, rptfile);
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