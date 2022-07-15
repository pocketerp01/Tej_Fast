using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_prod3 : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv, db1, db2; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    DataRow dr1;
    string frm_AssiID;
    string frm_UserID, frm_cDt1, frm_myear;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
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
                co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
            }

            hfhcid.Value = frm_formID;

            if (!Page.IsPostBack)
            {
                col1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT BRN||'~'||PRD AS PP FROM FIN_MSYS WHERE UPPER(TRIM(ID))='" + frm_formID + "'", "PP");
                if (col1.Length > 1)
                {
                    hfaskBranch.Value = col1.Split('~')[0].ToString();
                    hfaskPrdRange.Value = col1.Split('~')[1].ToString();
                }
                show_data();
            }
        }
    }

    public void show_data()
    {
        HCID = hfhcid.Value.Trim(); SQuery = ""; fgen.send_cookie("MPRN", "N");
        fgen.send_cookie("REPLY", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", "");

        // asking for Branch Consolidate Popup
        if (hfaskBranch.Value == "Y")
        { hfaskBranch.Value = "Y"; fgen.msg("-", "CMSG", "Do you want to see consolidate report'13'(No for branch wise)"); }
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "89554":
                case "F60121":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "FB3057":
                    fgen.Fn_open_dtbox("-", frm_qstr);
                    break;
                case "22610A":
                case "22610B":
                    fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
                    break;

                case "P15005Y":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", HCID);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F15127":
                    SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;

                case "F40055":
                    SQuery = "SELECT '00' AS FSTR,'InHouse' AS PRINT_TYPE,'-' as v FROM DUAL UNION ALL SELECT '01' AS FSTR,'Supplied' AS PRINT_TYPE,'-' as v FROM DUAL UNION ALL SELECT '02' AS FSTR,'All' AS PRINT_TYPE,'-' as v FROM DUAL";
                    header_n = "Select Rejection Type";
                    break;

                case "F40058":
                    SQuery = "SELECT 'YES' AS FSTR, 'Do You Want to see Report Item Wise' as message,'YES' AS ans from dual UNION ALL SELECT 'NO' AS FSTR,'Do You Want to See Report for all Items' as message,'NO' AS ans FROM DUAL";
                    break;
                case "F35228C":
                case "F35228D":
                    fgen.Fn_open_Act_itm_prd("", frm_qstr);
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
            }
        }
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        val = hfhcid.Value.Trim();
        fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popup
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            hfcode.Value = "";
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15127")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
        }
        // else if branch selection box opens then it comes here
        else if (Request.Cookies["REPLY"].Value.Length > 0)
        {
            value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            switch (val)
            {
                default:
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (hfaskBranch.Value == "Y")
                    {
                        if (value1 == "Y") hfbr.Value = "ABR";
                        else hfbr.Value = "";
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                    }
                    break;
                case "25156":
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                // if we want to ask another popup's
                // Month Popup Instead of Date Range *************
                case "89553":
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                    fgen.send_cookie("xid", "FINSYS_S");
                    fgen.send_cookie("srchSql", SQuery);
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    //fgen.Fn_open_sseek("Select Month");
                    break;

            }
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        val = hfhcid.Value.Trim();
        //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            value3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");

            fromdt = value1;
            todt = value2;
            cldt = value3;

            cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
            cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

            xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
            xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
            xprd2 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')";
            yr_fld = year;

            co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");


            if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
            else branch_Cd = "branchcd='" + mbr + "'";

            // after prdDmp this will run   

            switch (val)
            {
                case "F39131":
                    // Gate Inward Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type>'14' and a.type like '1%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production (Std) Entry Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;


                case "S06005E":
                    // open graph
                    SQuery = "select month_name,count(*) as  tot_bas,count(*) as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Query Graph", "line", "Month Wise", "-", SQuery, "");
                    break;

                case "S15115I":
                    // open drill down form
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                    fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                    fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                    fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;
                case "F35228":
                    SQuery = "SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='15' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(invdate,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='17' AND VCHDATE " + xprdrange + ")A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C.BRANCHCD='" + mbr + "' GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode ORDER BY A.VCHDATE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production (Std) Entry Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F35228A":
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    string r10 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R10'", "params");
                    if (r10.Length < 5) r10 = "01/01/2010";
                    mq0 = "select a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.mname as main_grp,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,a.imin as min_,a.imax as max_ from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.cpartno,b.packsize,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y') a ";
                    SQuery = "SELECT B.stock_qty,A.*,'-' as remarks FROM (SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,sum(a.prod)-sum(a.fg_rcv) as WIP_Material,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='15' AND VCHDATE " + xprdrange + " AND VCHDATE>TO_dATE('" + r10 + "','dd/mm/yyyy') UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='17' AND VCHDATE " + xprdrange + " AND VCHDATE>TO_dATE('" + r10 + "','dd/mm/yyyy') ) A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C.BRANCHCD='" + mbr + "' GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode HAVING sum(a.prod)-sum(a.fg_rcv)>0) A, (" + mq0 + ") B WHERE TRIM(A.erpcode)=TRIM(b.ERPCODE)  ORDER BY A.Entry_DATE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production (Std) Pending for QC Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F35228B":
                    cond = "";
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    mq0 = "SELECT round(avg(iqtyout)) as MNTH_AVG,icode FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '4%' AND VCHDATE BETWEEN TO_DATE(TO_CHAR(SYSDATE-90,'DD/MM/YYYY'),'DD/MM/YYYY') AND TO_DATE(TO_CHAR(SYSDATE,'DD/MM/YYYY'),'DD/MM/YYYY') group by icode";
                    mq1 = "SELECT ERPCODE,SUM(pending_qc) AS pending_qc FROM (SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,sum(a.prod)-sum(a.fg_rcv) as pending_qc,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='15' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='17' AND VCHDATE " + xprdrange + ")A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C." + branch_Cd + " GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode HAVING sum(a.prod)-sum(a.fg_rcv)>0) GROUP BY ERPCODE ";
                    SQuery = "select f.name||' ('||a.branchcd||')' as plant_,a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.MAT1 as dept,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,C.pending_qc as in_process,a.imin as min_,a.imax as max_,B.MNTH_AVG from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.cpartno,b.packsize,b.mat1,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,a.branchcd from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and a.erpcode like '9%' and d.id='Y') a LEFT OUTER JOIN (" + mq0 + ") B ON A.ERPCODE=TRIM(B.ICODE) LEFT OUTER JOIN (" + mq1 + ") c ON TRIM(A.ERPCODE)=TRIM(C.erpcode) ,type f where trim(a.branchcd)=trim(f.type1) and f.id='B' order by a.erpcode,a.iname";
                    SQuery = "select f.name||' ('||a.branchcd||')' as plant_,a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.MAT1 as dept,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,C.pending_qc as in_process,a.imin as min_,a.imax as max_,B.MNTH_AVG from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.irate,b.cpartno,b.packsize,e.name mat1,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,a.branchcd from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b left outer join typegrp e on trim(b.mat9)=trim(e.acref) and e.id='WI' and e.branchcd='" + mbr + "' ,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and a.erpcode like '9%' and d.id='Y') a LEFT OUTER JOIN (" + mq0 + ") B ON A.ERPCODE=TRIM(B.ICODE) LEFT OUTER JOIN (" + mq1 + ") c ON TRIM(A.ERPCODE)=TRIM(C.erpcode) ,type f where trim(a.branchcd)=trim(f.type1) and f.id='B' order by a.erpcode,a.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Plant Stock Report Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F35228B1":
                    cond = "";
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    mq0 = "SELECT round(avg(iqtyout)) as MNTH_AVG,icode FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '4%' AND VCHDATE BETWEEN TO_DATE(TO_CHAR(SYSDATE-90,'DD/MM/YYYY'),'DD/MM/YYYY') AND TO_DATE(TO_CHAR(SYSDATE,'DD/MM/YYYY'),'DD/MM/YYYY') group by icode";
                    mq1 = "SELECT ERPCODE,SUM(pending_qc) AS pending_qc FROM (SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,sum(a.prod)-sum(a.fg_rcv) as pending_qc,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='15' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='17' AND VCHDATE " + xprdrange + ")A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C." + branch_Cd + " GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode HAVING sum(a.prod)-sum(a.fg_rcv)>0) GROUP BY ERPCODE ";
                    SQuery = "select a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.MAT1 as dept,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,C.pending_qc as in_process,a.imin as min_,a.imax as max_,B.MNTH_AVG,a.irate as Rate,(a.irate * a.closing_stk) as stock_Value from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.irate,b.cpartno,b.packsize,b.mat1,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and a.erpcode like '9%' and d.id='Y') a LEFT OUTER JOIN (" + mq0 + ") B ON A.ERPCODE=TRIM(B.ICODE) LEFT OUTER JOIN (" + mq1 + ") c ON TRIM(A.ERPCODE)=TRIM(C.erpcode) order by a.erpcode,a.iname";
                    SQuery = "select f.name||' ('||a.branchcd||')' as plant_,a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.MAT1 as dept,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,C.pending_qc as in_process,a.imin as min_,a.imax as max_,B.MNTH_AVG,a.irate as Rate,(a.irate * a.closing_stk) as stock_Value from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.irate,b.cpartno,b.packsize,b.mat1,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,a.branchcd from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and a.erpcode like '9%' and d.id='Y') a LEFT OUTER JOIN (" + mq0 + ") B ON A.ERPCODE=TRIM(B.ICODE) LEFT OUTER JOIN (" + mq1 + ") c ON TRIM(A.ERPCODE)=TRIM(C.erpcode) ,type f where trim(a.branchcd)=trim(f.type1) and f.id='B' order by a.erpcode,a.iname";
                    SQuery = "select f.name||' ('||a.branchcd||')' as plant_,a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.MAT1 as dept,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,C.pending_qc as in_process,a.imin as min_,a.imax as max_,B.MNTH_AVG,a.irate as Rate,(a.irate * a.closing_stk) as stock_Value from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.irate,b.cpartno,b.packsize,e.name mat1,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,a.branchcd from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b left outer join typegrp e on trim(b.mat9)=trim(e.acref) and e.id='WI' and e.branchcd='" + mbr + "' ,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and a.erpcode like '9%' and d.id='Y') a LEFT OUTER JOIN (" + mq0 + ") B ON A.ERPCODE=TRIM(B.ICODE) LEFT OUTER JOIN (" + mq1 + ") c ON TRIM(A.ERPCODE)=TRIM(C.erpcode) ,type f where trim(a.branchcd)=trim(f.type1) and f.id='B' order by a.erpcode,a.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Plant Stock Report Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F35228C":
                    SQuery = "SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,sum(a.prod)-sum(a.fg_rcv) as pending_qc,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='15' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='17' AND VCHDATE " + xprdrange + ")A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C.BRANCHCD='" + mbr + "' GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode HAVING sum(a.prod)-sum(a.fg_rcv)>0 ORDER BY A.VCHDATE";
                    string part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    string cond1 = "";
                    cond = "";
                    cond1 = cond.ToUpper().Replace("ICODE", "A.ICODE");
                    if (part_cd.Length > 3)
                    {
                        part_cd = " and substr(a.icode,1,4)='" + part_cd + "'";
                    }

                    mdt = new DataTable();
                    //mdt.Columns.Add("Customer");
                    mdt.Columns.Add("ERP_Code");
                    mdt.Columns.Add("Product");
                    mdt.Columns.Add("Internal_Name");
                    mdt.Columns.Add("Series");
                    mdt.Columns.Add("Dept");
                    //mdt.Columns.Add("Schedule_Date");
                    mdt.Columns.Add("PO_Qty");
                    //mdt.Columns.Add("Pending_Sch");
                    //mdt.Columns.Add("Schedule_Today");
                    mdt.Columns.Add("Stock");
                    mdt.Columns.Add("In_Proc_Stock");
                    mdt.Columns.Add("MIN");
                    mdt.Columns.Add("MAX");
                    mdt.Columns.Add("Month_Avg");

                    SQuery = "Select b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,c.MAT1 as dept,c.imin,c.imax,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + xprdrange + " group by trim(a.acode),trim(A.icode) union all  Select trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2 from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + xprdrange + " group by trim(a.acode),trim(a.Icode)) a, famst b,item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) " + part_cd + " " + cond1 + " group by b.Aname,c.Iname,c.Unit,c.imin,c.imax,c.cpartno,c.MAT1,trim(A.icode),trim(A.acode) Order by B.aname,c.Iname";

                    SQuery = "Select b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,SUM(a.qtyord) as Order_Qty,sum(a.Soldqty) as Despatch_Qty,sum(a.bal_qty) as Pend_Qty,sum(round(a.bal_qty*a.srate,2)) as Pend_Value,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from wbvu_pending_so a, famst b,item c where a.branchcd='" + mbr + "' and a.orddt " + xprdrange + " and trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) " + part_cd + " and a.bal_qty>0 GROUP BY b.Aname,c.Iname,c.cpartno,trim(a.acode),trim(a.icode) ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        cond = "";
                        frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                        xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                        mq0 = "SELECT round(avg(iqtyout)) as MNTH_AVG,icode FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '4%' AND VCHDATE BETWEEN TO_DATE(TO_CHAR(SYSDATE-90,'DD/MM/YYYY'),'DD/MM/YYYY') AND TO_DATE(TO_CHAR(SYSDATE,'DD/MM/YYYY'),'DD/MM/YYYY') group by icode";
                        mq1 = "SELECT ERPCODE,SUM(pending_qc) AS pending_qc FROM (SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,sum(a.prod)-sum(a.fg_rcv) as pending_qc,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='15' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='17' AND VCHDATE " + xprdrange + ")A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C." + branch_Cd + " GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode HAVING sum(a.prod)-sum(a.fg_rcv)>0) GROUP BY ERPCODE ";
                        SQuery = "select f.name||' ('||a.branchcd||')' as plant_,a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.MAT1 as dept,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,C.pending_qc as in_process,a.imin as min_,a.imax as max_,B.MNTH_AVG from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.cpartno,b.packsize,b.mat1,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,a.branchcd from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + " " + cond + " and STORE='Y' GROUP BY trim(icode),BRANCHCD) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and a.erpcode like '9%' and d.id='Y') a LEFT OUTER JOIN (" + mq0 + ") B ON A.ERPCODE=TRIM(B.ICODE) LEFT OUTER JOIN (" + mq1 + ") c ON TRIM(A.ERPCODE)=TRIM(C.erpcode) ,type f where trim(a.branchcd)=trim(f.type1) and f.id='B' order by a.erpcode,a.iname";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);

                        dt3 = new DataTable();
                        //dt3 = fgen.getdata(frm_qstr, co_cd, "SELECT sum(is_number(DAY" + fgen.padlc(Convert.ToInt32(DateTime.Now.Day), 2) + ")) as DAYM,ACODE,ICODE FROM SCHEDULE WHERE BRANCHCD='" + mbr + "' AND TYPE='46' and TO_CHAR(vchdate,'MM/YYYY')='" + Convert.ToDateTime(todt).ToString("MM/yyyy") + "' " + cond + " group by ACODE,ICODE order by acode,icode ");

                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, co_cd, "SELECT sum(qtyord) as ordqty,ACODE,ICODE FROM SOMAS WHERE BRANCHCD='" + mbr + "' AND TYPE like '4%' and TO_CHAR(orddt,'MM/YYYY')='" + Convert.ToDateTime(todt).ToString("MM/yyyy") + "' " + cond + " group by ACODE,ICODE order by acode,icode ");

                        DataTable dt4 = new DataTable();
                        SQuery = "Select c.Iname as Part_Name,c.cpartno as Part_Number,e.name as dept,c.imin,c.imax,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + xprdrange + " group by trim(a.acode),trim(A.icode) union all  Select trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2 from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + xprdrange + " group by trim(a.acode),trim(a.Icode)) a,item c left outer join typegrp e on trim(c.mat9)=trim(e.acref) and e.id='WI' and e.branchcd='" + mbr + "' where trim(A.icode)=trim(c.icode) " + part_cd + " " + cond1 + " group by c.Iname,c.Unit,c.imin,c.imax,c.cpartno,c.MAT1,trim(A.icode),trim(A.acode),E.NAME Order by c.Iname";
                        //dt4 = fgen.getdata(frm_qstr, co_cd, SQuery);

                        foreach (DataRow dr in dt.Rows)
                        {
                            oporow = mdt.NewRow();
                            //oporow["Customer"] = dr["Customer_Name"].ToString();
                            oporow["ERP_Code"] = dr["ERP_acode"].ToString().Trim();
                            oporow["Product"] = dr["part_name"].ToString();
                            oporow["Internal_Name"] = dr["part_number"].ToString();
                            //oporow["Schedule_Date"] = "";
                            oporow["Series"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "subgrup_name");
                            oporow["Dept"] = fgen.seek_iname_dt(dt4, "ERP_Icode='" + dr["ERP_Icode"].ToString().Trim() + "'", "dept");
                            oporow["PO_Qty"] = dr["Pend_Qty"];
                            //oporow["Pending_Sch"] = fgen.seek_iname_dt(dt4, "ERP_Icode='" + dr["ERP_Icode"].ToString().Trim() + "'", "Difference");
                            //oporow["Schedule_Today"] = fgen.seek_iname_dt(dt3, "ACODE='" + dr["ERP_acode"].ToString().Trim() + "' AND ICODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "DAYM");
                            oporow["Stock"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "stock_qty");
                            oporow["In_Proc_Stock"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "in_process");
                            oporow["MIN"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "min_");
                            oporow["MAX"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "max_");
                            oporow["Month_Avg"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "MNTH_AVG");
                            mdt.Rows.Add(oporow);
                        }
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = mdt;
                    fgen.Fn_open_rptlevelJS("Pending Order Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F35228D":
                    SQuery = "SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,sum(a.prod)-sum(a.fg_rcv) as pending_qc,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='15' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='17' AND VCHDATE " + xprdrange + ")A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C.BRANCHCD='" + mbr + "' GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode HAVING sum(a.prod)-sum(a.fg_rcv)>0 ORDER BY A.VCHDATE";
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    cond1 = "";
                    cond = "";
                    cond1 = cond.ToUpper().Replace("ICODE", "A.ICODE");
                    if (part_cd.Length > 3)
                    {
                        part_cd = " and substr(a.icode,1,4)='" + part_cd + "'";
                    }

                    mdt = new DataTable();
                    //mdt.Columns.Add("Customer");
                    mdt.Columns.Add("ERP_Code");
                    mdt.Columns.Add("Product");
                    mdt.Columns.Add("Internal_Name");
                    mdt.Columns.Add("Series");
                    mdt.Columns.Add("Dept");
                    //mdt.Columns.Add("Schedule_Date");
                    //mdt.Columns.Add("PO_Qty");
                    mdt.Columns.Add("Pending_Sch");
                    mdt.Columns.Add("Schedule_Today");
                    mdt.Columns.Add("Stock");
                    mdt.Columns.Add("In_Proc_Stock");
                    mdt.Columns.Add("MIN");
                    mdt.Columns.Add("MAX");
                    mdt.Columns.Add("Month_Avg");

                    SQuery = "Select b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,c.MAT1 as dept,c.imin,c.imax,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + xprdrange + " group by trim(a.acode),trim(A.icode) union all  Select trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2 from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + xprdrange + " group by trim(a.acode),trim(a.Icode)) a, famst b,item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) " + part_cd + " " + cond1 + " group by b.Aname,c.Iname,c.Unit,c.imin,c.imax,c.cpartno,c.MAT1,trim(A.icode),trim(A.acode) Order by B.aname,c.Iname";
                    SQuery = "Select c.Iname as Part_Name,c.cpartno as Part_Number,e.name as dept,c.imin,c.imax,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + xprdrange + " group by trim(a.acode),trim(A.icode) union all  Select trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2 from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + xprdrange + " group by trim(a.acode),trim(a.Icode)) a,item c left outer join typegrp e on trim(c.mat9)=trim(e.acref) and e.id='WI' and e.branchcd='" + mbr + "' where trim(A.icode)=trim(c.icode) " + part_cd + " " + cond1 + " group by c.Iname,c.Unit,c.imin,c.imax,c.cpartno,c.MAT1,trim(A.icode),trim(A.acode),E.NAME Order by c.Iname";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        cond = "";
                        frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                        xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                        mq0 = "SELECT round(avg(iqtyout)) as MNTH_AVG,icode FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '4%' AND VCHDATE BETWEEN TO_DATE(TO_CHAR(SYSDATE-90,'DD/MM/YYYY'),'DD/MM/YYYY') AND TO_DATE(TO_CHAR(SYSDATE,'DD/MM/YYYY'),'DD/MM/YYYY') group by icode";
                        mq1 = "SELECT ERPCODE,SUM(pending_qc) AS pending_qc FROM (SELECT c.NAME AS STAGE,A.VCHDATE AS Entry_DATE,B.INAME AS PRODUCT_NAME,B.CPARTNO AS PARTNO,b.icode as erpcode,SUM(A.PROD) AS PROD,SUM(A.FG_RCV) AS FG_RCV,sum(a.prod)-sum(a.fg_rcv) as pending_qc,TRIM(A.BTCHNO) AS BATCH_NO FROM (SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTY_CHL AS PROD,0 AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='15' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD,TYPE,TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PROD,IQTYIN AS FG_RCV,trim(btchno) AS BTCHNO,STAGE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE='17' AND VCHDATE " + xprdrange + ")A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODe) AND TRIM(A.STAGE)=TRIM(C.ACREF) AND C.ID='WI' AND C." + branch_Cd + " GROUP BY A.VCHDATE,B.INAME,B.CPARTNO,TRIM(A.btchno),c.NAME,b.icode HAVING sum(a.prod)-sum(a.fg_rcv)>0) GROUP BY ERPCODE ";
                        SQuery = "select f.name||' ('||a.branchcd||')' as plant_,a.erpcode,a.iname as product_name,a.cpartno as internal_name,a.sname as subgrup_name,a.MAT1 as dept,a.closing_stk as stock_qty,a.unit,'-' as stock_loc,a.packsize as pack_std,C.pending_qc as in_process,a.imin as min_,a.imax as max_,B.MNTH_AVG from (select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.cpartno,b.packsize,b.mat1,b.unit,b.imin,b.imax,b.cdrgno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,a.branchcd from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + " " + cond + " and STORE='Y' GROUP BY trim(icode),BRANCHCD) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and a.erpcode like '9%' and d.id='Y') a LEFT OUTER JOIN (" + mq0 + ") B ON A.ERPCODE=TRIM(B.ICODE) LEFT OUTER JOIN (" + mq1 + ") c ON TRIM(A.ERPCODE)=TRIM(C.erpcode) ,type f where trim(a.branchcd)=trim(f.type1) and f.id='B' order by a.erpcode,a.iname";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);

                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, co_cd, "SELECT sum(is_number(DAY" + Convert.ToInt32(DateTime.Now.Day) + ")) as DAYM,ACODE,ICODE FROM SCHEDULE WHERE BRANCHCD='" + mbr + "' AND TYPE='46' and TO_CHAR(vchdate,'MM/YYYY')='" + Convert.ToDateTime(todt).ToString("MM/yyyy") + "' " + cond + " group by ACODE,ICODE order by acode,icode ");

                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, co_cd, "SELECT sum(qtyord) as ordqty,ACODE,ICODE FROM SOMAS WHERE BRANCHCD='" + mbr + "' AND TYPE like '4%' and TO_CHAR(orddt,'MM/YYYY')='" + Convert.ToDateTime(todt).ToString("MM/yyyy") + "' " + cond + " group by ACODE,ICODE order by acode,icode ");

                        DataTable dt4 = new DataTable();
                        SQuery = "Select b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,SUM(a.qtyord) as Order_Qty,sum(a.Soldqty) as Despatch_Qty,sum(a.bal_qty) as Pend_Qty,sum(round(a.bal_qty*a.srate,2)) as Pend_Value,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from wbvu_pending_so a, famst b,item c where a.branchcd='" + mbr + "' and a.orddt " + xprdrange + " and trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) " + part_cd + " and a.bal_qty>0 GROUP BY b.Aname,c.Iname,c.cpartno,trim(a.acode),trim(a.icode) ";
                        //dt4 = fgen.getdata(frm_qstr, co_cd, SQuery);

                        foreach (DataRow dr in dt.Rows)
                        {
                            oporow = mdt.NewRow();
                            //oporow["Customer"] = dr["Customer_Name"].ToString();
                            oporow["ERP_Code"] = dr["ERP_acode"].ToString().Trim();
                            oporow["Product"] = dr["part_name"].ToString();
                            oporow["Internal_Name"] = dr["part_number"].ToString();
                            //oporow["Schedule_Date"] = "";
                            oporow["Series"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "subgrup_name");
                            oporow["Dept"] = dr["dept"];
                            //oporow["PO_Qty"] = fgen.seek_iname_dt(dt4, "ERP_Icode='" + dr["ERP_Icode"].ToString().Trim() + "'", "Pend_Qty");
                            oporow["Pending_Sch"] = dr["Difference"];
                            oporow["Schedule_Today"] = fgen.seek_iname_dt(dt3, "ACODE='" + dr["ERP_acode"].ToString().Trim() + "' AND ICODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "DAYM");
                            oporow["Stock"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "stock_qty");
                            oporow["In_Proc_Stock"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "in_process");
                            oporow["MIN"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "min_");
                            oporow["MAX"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "max_");
                            oporow["Month_Avg"] = fgen.seek_iname_dt(dt2, "ERPCODE='" + dr["ERP_Icode"].ToString().Trim() + "'", "MNTH_AVG");
                            mdt.Rows.Add(oporow);
                        }
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = mdt;
                    fgen.Fn_open_rptlevelJS("Pending Schedule Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "FB3057":
                    mq0 = "";
                    r10 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R10'", "params");
                    r10 = mq0.Length > 1 ? mq0 : r10;
                    xprdrange = "<= to_date('" + todt + "','dd/mm/yyyy') and " + (r10.Length > 2 ? " a.vchdate>=to_Date('" + r10 + "','dd/mm/yyyy') " : " a.vchdate>=to_Date('" + cDT1 + "','dd/mm/yyyy')");
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, "SELECT DISTINCT TYPE1,name,place FROM TYPE WHERE ID='1' AND TYPE1 LIKE '6%' ORDER BY place,TYPE1");
                    mq0 = ""; mq10 = ""; mq1 = ""; mq2 = "";
                    var t = from type in dt.AsEnumerable()
                    select new
                    {
                        type1 = type.Field<string>("type1").Replace("(", "").Replace(")", "").Replace("/", "_").Replace(" ", "_"),
                        name = type.Field<string>("name").Replace("(", "").Replace(")", "").Replace("/", "_").Replace(" ", "_").Replace("&", "and")
                    };
                    foreach (var r in t)
                    {
                        mq0 += (mq0.Length > 0 ? "," : "") + "DECODE(TRIM(A.STAGE),'" + r.type1 + "',round(sum(a.iqtyin-a.iqtyout)),0) AS " + r.name;
                        mq10 += (mq10.Length > 0 ? "," : "") + "0 AS " + r.name;
                        mq1 += (mq1.Length > 0 ? "," : "") + "SUM(" + r.name + ")  AS " + r.name;
                        mq2 += (mq2.Length > 0 ? "+" : "") + "SUM(" + r.name + ")";
                    }
                    SQuery = "SELECT a.revis_no as Job_No,trim(A.icode) as Icode,B.INAME AS ITEM_NAME,B.CPARTNO AS PART_NO,SUM(CUTTING)  AS CUTTING,SUM(BOARING)  AS BOARING" +
                        ",SUM(PRE_M_C_FOR_CNC)  AS PRE_M_C_FOR_CNC,SUM(TURNING_OR_LATHE)  AS TURNING_OR_LATHE,SUM(CNC)  AS CNC,SUM(VMC)  AS VMC,SUM(MILLING)  AS MILLING" +
                        ",SUM(HARDENING_HT)  AS HARDENING_HT,SUM(FINAL_M_C)  AS FINAL_M_C,SUM(CYLINDRICAL_GRINDING)  AS CYLINDRICAL_GRINDING,SUM(SURFACE_GRINDING)  AS " +
                        "SURFACE_GRINDING,SUM(DIAMOND_POLISHING)  AS DIAMOND_POLISHING,SUM(PLATING)  AS PLATING,SUM(WELDING)  AS WELDING,SUM(FINISHING)  AS FINISHING," +
                        "SUM(ASSEMBLY)  AS ASSEMBLY,SUM(FINAL_QUALITY)  AS FINAL_QUALITY,SUM(PROD_REJ)  AS PROD_REJ,(SUM(CUTTING) + SUM(BOARING) + SUM(PRE_M_C_FOR_CNC)" +
                        " + SUM(TURNING_OR_LATHE) + SUM(CNC) + SUM(VMC) + SUM(MILLING) + SUM(HARDENING_HT) + SUM(FINAL_M_C) + SUM(CYLINDRICAL_GRINDING) + " +
                        "SUM(SURFACE_GRINDING) + SUM(DIAMOND_POLISHING) + SUM(PLATING) + SUM(WELDING) + SUM(FINISHING) + SUM(ASSEMBLY) + SUM(FINAL_QUALITY) + SUM(PROD_REJ))" +
                        " AS Total,trim(A.icode) as erpcode from(SELECT TRIM(A.ICODe) AS ICODE, a.revis_no, DECODE(TRIM(A.STAGE), '61', round(sum(a.qtyin - a.qtyout)), 0) " +
                        "AS CUTTING, DECODE(TRIM(A.STAGE), '62',round(sum(a.qtyin - a.qtyout)), 0) AS BOARING, DECODE(TRIM(A.STAGE), '63', round(sum(a.qtyin - a.qtyout)), 0)" +
                        " AS PRE_M_C_FOR_CNC,DECODE(TRIM(A.STAGE), '64', round(sum(a.qtyin - a.qtyout)), 0) AS TURNING_OR_LATHE, DECODE(TRIM(A.STAGE), '65', " +
                        "round(sum(a.qtyin - a.qtyout)), 0) AS CNC, DECODE(TRIM(A.STAGE), '66', round(sum(a.qtyin - a.qtyout)), 0) AS VMC, DECODE(TRIM(A.STAGE), '67', " +
                        "round(sum(a.qtyin - a.qtyout)), 0) AS MILLING, DECODE(TRIM(A.STAGE), '68', round(sum(a.qtyin - a.qtyout)), 0) AS HARDENING_HT, DECODE(TRIM(A.STAGE)" +
                        ", '69', round(sum(a.qtyin - a.qtyout)), 0) AS FINAL_M_C, DECODE(TRIM(A.STAGE), '6A', round(sum(a.qtyin - a.qtyout)), 0) AS CYLINDRICAL_GRINDING," +
                        " DECODE(TRIM(A.STAGE), '6B', round(sum(a.qtyin - a.qtyout)), 0) AS SURFACE_GRINDING, DECODE(TRIM(A.STAGE), '6C', round(sum(a.qtyin - a.qtyout)), 0)" +
                        " AS DIAMOND_POLISHING, DECODE(TRIM(A.STAGE), '6D', round(sum(a.qtyin - a.qtyout)), 0) AS PLATING, DECODE(TRIM(A.STAGE), '6E', round(sum(a.qtyin - a.qtyout)), 0) AS WELDING, DECODE(TRIM(A.STAGE), '6F', round(sum(a.qtyin - a.qtyout)), 0) AS FINISHING, DECODE(TRIM(A.STAGE), '6G', round(sum(a.qtyin - a.qtyout)), 0) AS ASSEMBLY, DECODE(TRIM(A.STAGE), '6H', round(sum(a.qtyin - a.qtyout)), 0) AS FINAL_QUALITY, DECODE(TRIM(A.STAGE), '6R', round(sum(a.qtyin - a.qtyout)), 0) AS PROD_REJ, 0 as rej FROM" +
                      " (" + fgen.WIPSTKQry(co_cd, frm_qstr, mbr, r10, todt) + " ) A group by A.icode,A.revis_no,A.STAGE) a,item b where trim(a.icodE)= trim(b.icodE) group by trim(a.icode),b.iname,a.revis_no,b.cpartno having (SUM(CUTTING) + SUM(BOARING) + SUM(PRE_M_C_FOR_CNC) + SUM(TURNING_OR_LATHE) + SUM(CNC) + SUM(VMC) + SUM(MILLING) + SUM(HARDENING_HT) + SUM(FINAL_M_C) + SUM(CYLINDRICAL_GRINDING) + SUM(SURFACE_GRINDING) + SUM(DIAMOND_POLISHING) + SUM(PLATING) + SUM(WELDING) + SUM(FINISHING) + SUM(ASSEMBLY) + SUM(FINAL_QUALITY) + SUM(PROD_REJ)) <> 0 order by trim(A.revis_no) desc, trim(A.icode) asc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("WIP Columnar Stock Report (as on " + fromdt + ")", frm_qstr);
                    break;
            }
        }
    }
}