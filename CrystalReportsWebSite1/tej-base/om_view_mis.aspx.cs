using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

using System.Text;

public partial class om_view_mis : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR, spcode;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2, insvchDT, dt4, dt5;
    double month, to_cons, itot_stk, itv, db, db1, db2, db3, db4, db5, db6 = 0, db7, db8, db9, db10, db11, db12, db13, db14, db15; DataRow oporow, ROWICODE, ROWICODE2, dro; DataView dv;
    DataTable itemospDT = new DataTable();
    DataTable itemospDT2 = new DataTable();
    DataTable fullDt = new DataTable();
    DataTable bomanx = new DataTable();
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID;
    string rmcBranch = "";
    fgenDB fgen = new fgenDB();
    int runningNo = 0;
    string xday, xmonth, xyear, xselected_date, scode, mtnno, xdt;
    string f1, f2, f3, f4, ppmdate, pmdate, sysdt, lmdt, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12;
    double nmth1, nmth2, totamt, salemat = 0; string popsql;
    double CUST = 0, NMTH = 0, AVG_CUST = 0;
    double msrno = 0;
    DataTable dtSort = new DataTable();
    DataRow drDtSort;
    string ACUST, MCUST; StringBuilder sbgauge = new StringBuilder(); StringBuilder sbg = new StringBuilder();
    string rateCond = "0"; string home_curr = "", home_div_iden = "", home_divider = "", numbr_fmt = "", numbr_fmt2 = "", frm_prodsheet = "", frm_inspvch = "";

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
                //mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
            }
            sysdt = fgen.Fn_curr_dt(co_cd, frm_qstr);
            hfhcid.Value = frm_formID;

            home_curr = "INR";
            home_divider = "100000";
            home_div_iden = "Lacs";
            numbr_fmt = "9,99,99,99,999.99";
            numbr_fmt2 = "9,99,99,99,999";
            frm_prodsheet = "prod_sheet";
            frm_inspvch = "inspvch";
            if (co_cd == "HPPI" || co_cd == "SPPI")
            {
                frm_prodsheet = "prod_sheetk";
                frm_inspvch = "inspvchk";
            }

            if (co_cd == "BAUS" || co_cd == "STLC" || co_cd == "ZEEP")
            {
                home_curr = "USD";
                home_divider = "1000";
                home_div_iden = "Thousands";
                numbr_fmt = "999,999,999.99";
                numbr_fmt2 = "999,999,999";
            }
            if (co_cd == "BMED" || co_cd == "GESD" || co_cd == "HPPI" || co_cd == "SPPI")
            {
                home_curr = "AED";
                home_divider = "1000";
                home_div_iden = "Thousands";
                numbr_fmt = "999,999,999.99";
                numbr_fmt2 = "999,999,999";
            }
            if (co_cd == "MFLX")
            {
                home_curr = "MUR";
                home_divider = "1000";
                home_div_iden = "Thousands";
                numbr_fmt = "999,999,999.99";
                numbr_fmt2 = "999,999,999";
            }

            if (!Page.IsPostBack)
            {
                col1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT BRN||'~'||PRD AS PP FROM FIN_MSYS WHERE UPPER(TRIM(ID))='" + frm_formID + "'", "PP");
                if (col1.Length > 1)
                {
                    hfaskBranch.Value = col1.Split('~')[0].ToString();
                    hfaskPrdRange.Value = col1.Split('~')[1].ToString();
                }
                div2.Visible = false;
                div4.Visible = false;
                div5.Visible = false;
                g1.Visible = false;
                g2.Visible = false;
                g3.Visible = false;
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

                case "F05124":
                    SQuery = "";
                    fgen.Fn_open_PartyItemDateRangeBox("FG Profitability Cost [Selection Menu]", frm_qstr);
                    break;
                case "F05125D":
                case "F05125E":
                    SQuery = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "F05125":
                    SQuery = "";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID2", "Tejaxo");
                    if (co_cd == "MINV" || co_cd == "DREM" || co_cd == "UKB") fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID2", "FINSYS_S");
                    fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
                    break;
                case "F05125A":
                case "F05125C":
                    SQuery = "";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID2", "FINSYS_S");
                    fgen.Fn_open_PartyItemDateRangeBox("BOM Cost - Working File [Selection Menu]", frm_qstr);
                    break;
                case "F05110":
                    SQuery = "";
                    fgen.Fn_open_dtbox("-", frm_qstr);
                    break;

                #region JMOBILE
                #region DAILY MIS
                case "F05202":
                    i0 = 1;
                    fgen.drillQuery(0, "select a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as fstr,'-' as gstr ,b.name as location,to_char(a.vchdate,'dd/MM/yyyy') as dated,to_char(round(sum(a.amt_sale)/" + home_divider + ",2),'" + numbr_fmt + "') as basic_sale from sale a, type b where a.branchcd!= 'DD' and to_date(to_char(a.vchdate,'dd/MM/yyyy'),'dd/MM/yyyy') between to_date('" + sysdt + "','dd/MM/yyyy')-30 and to_date('" + sysdt + "','dd/MM/yyyy') and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,to_char(a.vchdate,'dd/MM/yyyy'),a.vchdate order by a.vchdate desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "select a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as fstr,a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as gstr,b.aname as Party_Name,to_char(a.vchdate,'dd/MM/yyyy') as Sale_date, to_char(round(sum(a.amt_sale)/" + home_divider + ",2),'99,99,99.99') as basic from sale a ,famst b where trim(a.acode)=trim(b.acode) group by a.branchcd,a.acode,b.aname, to_char(a.vchdate,'dd/MM/yyyy') order by basic desc", frm_qstr);
                    fgen.drillQuery(2, "select a.branchcd as fstr,a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as gstr,a.invno as Inv_no,to_char(a.invdate,'dd/MM/yyyy') as Inv_Date,a.icode, b.iname as Item_Name,a.iqtyout as Qty,to_char(round(a.irate,2),'" + numbr_fmt + "') as Rate, to_char(round((a.iamount)/" + home_divider + ",2),'" + numbr_fmt + "') as Amount,c.mo_vehi as truck_no,c.drv_name,c.drv_mobile from ivoucher a,item b,sale c where trim(a.branchcd)||a.type||trim(a.vchnum)||to_char(a.vchdate,'ddmmyyyy')=trim(c.branchcd)||c.type||trim(c.vchnum)||to_char(c.vchdate,'ddmmyyyy') and trim(a.icode)=trim(b.icode) order by a.invno", frm_qstr);
                    fgen.Fn_DrillReport("Daily Sales Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;

                case "F05205":
                    i0 = 1;
                    fgen.drillQuery(0, "select a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as fstr,'-' as gstr,b.name as location,to_char(a.vchdate,'dd/MM/yyyy') as dated,to_char(round(sum(a.iamount)/" + home_divider + ",2),'" + numbr_fmt + "') as mrr_value from ivoucher a, type b  where a.branchcd!='DD' and (a.type='02' or a.type='05') and a.vchdate between to_date('" + sysdt + "','dd/MM/yyyy')-30 and to_date('" + sysdt + "','dd/MM/yyyy')and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,to_char(a.vchdate,'dd/MM/yyyy'),a.vchdate order by a.vchdate desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "select a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as fstr,a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as gstr,b.aname as Party_Name,to_char(a.vchdate,'dd/MM/yyyy') as MRR_date,to_char(round(sum(a.iamount/" + home_divider + "),2),'99,99,99.99') as Basic from ivoucher a, famst b  where trim(a.acode)=trim(b.acode) and (a.type='02' or a.type='05') group by a.branchcd,a.acode,b.aname ,to_char(a.vchdate,'dd/MM/yyyy') order by basic desc", frm_qstr);
                    fgen.drillQuery(2, "select a.branchcd as fstr,a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as gstr,a.vchnum as MRR_no,to_char(a.vchdate,'dd/MM/yyyy') as MRR_Date,a.icode, b.iname as Item_Name,a.iqtyin as Qty,to_char(round(a.irate,2),'" + numbr_fmt + "') as Rate, to_char(round((a.iamount)/" + home_divider + ",2),'" + numbr_fmt + "') as Amount from ivoucher a,item b where trim(a.icode)=trim(b.icode) order by a.vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Daily Material Inward Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;

                case "F05208":
                    i0 = 1;
                    fgen.drillQuery(0, "Select a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as fstr,'-' as gstr,b.name as location,to_char(a.vchdate,'dd/MM/yyyy') as dated, to_char(round(sum(a.cramt-a.dramt)/" + home_divider + ",2),'" + numbr_fmt + "') as amount  from voucher a, type b where a.branchcd!='88' and substr(a.type,1,1)='1' and substr(a.acode,1,2) in('16') and a.vchdate between to_date('" + sysdt + "','dd/MM/yyyy')-30 and to_date('" + sysdt + "','dd/MM/yyyy') and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,a.vchdate,to_char(a.vchdate,'dd/MM/yyyy') order by a.vchdate desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "Select a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as fstr,a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as gstr,b.aname as Party_Name,to_char(a.vchdate,'dd/MM/yyyy') as Collection_Date, to_char(round(sum(a.cramt-a.dramt)/" + home_divider + ",2),'99,99,99.99') as Amount from voucher a, famst b where trim(a.acode)=trim(b.acode) and substr(a.type,1,1)='1' and substr(a.acode,1,2) in('16') group by a.branchcd,a.acode,b.aname ,to_char(a.vchdate,'dd/MM/yyyy') order by Amount desc", frm_qstr);
                    fgen.drillQuery(2, "select a.branchcd as fstr,a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as gstr,a.vchnum as cheque_no,to_char(a.vchdate,'dd/MM/yyyy') as cheque_Date, to_char(round((a.cramt-a.dramt)/" + home_divider + ",2),'99,99,99.99') as Amount from voucher a where substr(a.type,1,1)='1' and substr(a.acode,1,2) in('16') order by a.vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Daily Collection Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;

                case "F05211":
                    i0 = 1;
                    fgen.drillQuery(0, "Select a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as fstr,'-' as gstr,b.name as location,to_char(a.vchdate,'dd/MM/yyyy') as dated,to_char(round(sum(a.dramt-a.cramt)/" + home_divider + ",2),'" + numbr_fmt + "') as amount from voucher a, type b where a.branchcd!='88' and substr(a.type,1,1)='2' and substr(a.acode,1,2) in('05','06') and a.vchdate between to_date('" + sysdt + "','dd/MM/yyyy')-30 and to_date('" + sysdt + "','dd/MM/yyyy') and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,a.vchdate,to_char(a.vchdate,'dd/MM/yyyy') order by a.vchdate desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "Select a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as fstr,a.branchcd||to_char(a.vchdate,'dd/MM/yyyy') as gstr,trim(b.aname) as party_name,to_char(a.vchdate,'dd/MM/yyyy') as Payment_Date,to_char(round(sum(a.dramt-a.cramt)/" + home_divider + ",2),'99,99,99.99') as Amount from voucher a, famst b where trim(a.acode)=trim(b.acode) and substr(a.type,1,1)='2' and substr(a.acode,1,2) in('05','06') group by a.branchcd,a.acode,b.aname, to_char(a.vchdate,'dd/MM/yyyy') order by amount desc", frm_qstr);
                    fgen.drillQuery(2, "select a.branchcd as fstr,a.branchcd||trim(a.acode)||to_char(a.vchdate,'dd/MM/yyyy') as gstr,a.vchnum as cheque_no,to_char(a.vchdate,'dd/MM/yyyy') as cheque_Date, to_char(round((a.dramt-a.cramt)/" + home_divider + ",2),'99,99,99.99') as Amount from voucher a where substr(a.type,1,1)='2' and substr(a.acode,1,2) in('05','06') order by a.vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Daily Payment Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;
                #endregion

                #region MONTHLY MIS
                case "F05214":
                    i0 = 1;
                    fgen.drillQuery(0, "select a.branchcd||to_char(a.vchdate,'yyyymm') as fstr,'' as gstr,b.name as location,to_char(a.vchdate,'yyyymm') as dated,to_char(round(sum(a.amt_sale)/" + home_divider + ",2),'" + numbr_fmt + "') as basic_sale from sale a,type b where a.branchcd!= 'DD' and to_date(to_char(a.vchdate,'dd/MM/yyyy'),'dd/MM/yyyy') between to_date('" + sysdt + "','dd/MM/yyyy')-365 and to_date('" + sysdt + "','dd/MM/yyyy')and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,to_char(a.vchdate,'yyyymm') order by to_char(a.vchdate,'yyyymm') desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm') as gstr,b.aname as Party_Name,to_char(a.vchdate,'yyyymm') as Sale_month,to_char(round(sum(a.amt_sale/" + home_divider + "),2),'99,99,99.99') as basic from sale a, famst b where trim(a.acode)=trim(b.acode) group by a.branchcd,a.acode,b.aname, to_char(a.vchdate,'yyyymm') order by BASIC desc", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as gstr,a.vchnum as Inv_no,to_char(a.vchdate,'dd/MM/yyyy') as Inv_Date,a.icode, b.iname as Item_Name,a.iqtyout as Qty,to_char(round(a.irate,2),'" + numbr_fmt + "') as Rate, to_char(round((a.iamount)/" + home_divider + ",2),'" + numbr_fmt + "') as Amount,c.mo_vehi as truck_no,c.drv_name,c.drv_mobile from ivoucher a,item b,sale c where trim(a.branchcd)||a.type||trim(a.vchnum)||to_char(a.vchdate,'ddmmyyyy')=trim(c.branchcd)||c.type||trim(c.vchnum)||to_char(c.vchdate,'ddmmyyyy') and trim(a.icode)=trim(b.icode) and a.type like '4%' order by a.vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Monthly Sales Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;

                case "F05217":
                    i0 = 1;
                    fgen.drillQuery(0, "select a.branchcd||to_char(a.vchdate,'yyyymm') as fstr,'' as gstr,b.name as location,to_char(a.vchdate,'yyyymm') as dated,to_char(round(sum(a.iamount)/" + home_divider + ",2),'" + numbr_fmt + "') as mrr_value from ivoucher a,type b where a.branchcd!='DD' and (a.type='02' or a.type='05') and to_date(to_char(a.vchdate,'dd/MM/yyyy'),'dd/MM/yyyy') between to_DatE('" + sysdt + "','dd/MM/yyyy')-365 and to_date('" + sysdt + "','dd/MM/yyyy') and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,to_char(a.vchdate,'yyyymm') order by to_char(a.vchdate,'yyyymm') desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "Select trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm') as gstr, b.aname as Vendor_Name, to_char(a.vchdate,'yyyymm') as MRR_Month,to_char(round(sum(a.iamount)/" + home_divider + ",2),'99,99,99.99') as Basic from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and (a.type='02' or a.type='05') group by a.branchcd,a.acode,b.aname ,to_char(a.vchdate,'yyyymm') order by basic desc", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as gstr,a.vchnum as MRR_no,to_char(a.vchdate,'dd/MM/yyyy') as MRR_Date,a.icode, b.iname as Item_Name,a.iqtyin as Qty,to_char(round(a.irate,2),'" + numbr_fmt + "') as Rate, to_char(round((a.iamount)/" + home_divider + ",2),'" + numbr_fmt + "') as Amount from ivoucher a,item b where trim(a.icode)=trim(b.icode) and (a.type='02' or a.type='05') order by a.vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Monthly Material Inward Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;

                case "F05220":
                    i0 = 1;
                    fgen.drillQuery(0, "Select a.branchcd||to_char(a.vchdate,'yyyymm') as fstr,'' as gstr,b.name as location,to_char(a.vchdate,'yyyymm') as dated,to_char(round(sum(a.cramt-a.dramt)/" + home_divider + ",2),'" + numbr_fmt + "') as amount from voucher a, type b where a.branchcd!='88' and substr(a.type,1,1)='1' and substr(a.acode,1,2) in('16') and to_date(to_char(a.vchdate,'dd/MM/yyyy'),'dd/MM/yyyy') between to_date('" + sysdt + "','dd/MM/yyyy')-365 and to_date('" + sysdt + "','dd/MM/yyyy')and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,to_char(a.vchdate,'yyyymm') order by to_char(a.vchdate,'yyyymm') desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "Select trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm') as gstr,b.aname,to_char(a.vchdate,'yyyymm') as Collection_Month,to_char(round(sum(a.cramt-a.dramt)/" + home_divider + ",2),'99,99,99.99') as Amount from voucher a, famst b where trim(a.acode)=trim(b.acode) and substr(a.type,1,1)='1' and substr(a.acode,1,2) in('16') group by a.branchcd,a.acode,b.aname ,to_char(a.vchdate,'yyyymm') order by amount desc", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as gstr,a.vchnum as cheque_no,to_char(a.vchdate,'dd/MM/yyyy') as cheque_Date,to_char(round(((a.cramt-a.dramt)/" + home_divider + "),2),'99,99,99.99') as Amount from voucher a where substr(a.type,1,1)='1' and substr(a.acode,1,2) in('16') order by a.vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Monthly Collection Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;

                case "F05223":
                    i0 = 1;
                    fgen.drillQuery(0, "Select a.branchcd||to_char(a.vchdate,'yyyymm') as fstr,'' as gstr,b.name as location,to_char(a.vchdate,'yyyymm') as dated,to_char(round(sum(a.dramt-a.cramt)/" + home_divider + ",2),'" + numbr_fmt + "') as amount from voucher a,type b where a.branchcd!='88' and substr(a.type,1,1)='2' and substr(a.acode,1,2) in('05','06') and to_date(to_char(a.vchdate,'dd/MM/yyyy'),'dd/MM/yyyy') between to_date('" + sysdt + "','dd/MM/yyyy')-365 and to_date('" + sysdt + "','dd/MM/yyyy') and trim(a.branchcd)=trim(b.type1) and trim(upper(b.id))='B' group by a.branchcd,b.name,to_char(a.vchdate,'yyyymm') order by to_char(a.vchdate,'yyyymm') desc,b.name", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm') as gstr, b.aname,to_char(a.vchdate,'yyyymm') as Payment_Month,to_char(round(sum(a.dramt-a.cramt)/" + home_divider + ",2),'99,99,99.99') as Amount from voucher a, famst b where trim(a.acode)=trim(b.acode) and substr(a.type,1,1)='2' and substr(a.acode,1,2) in('05','06') group by a.branchcd,a.acode,b.aname ,to_char(a.vchdate,'yyyymm') order by amount desc", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.branchcd)||to_char(a.vchdate,'yyyymm')||trim(a.acode) as gstr,a.vchnum as cheque_no,to_char(a.vchdate,'dd/MM/yyyy') as cheque_Date,to_char(round(((a.dramt-a.cramt)/" + home_divider + "),2),'99,99,99.99') as Amount from voucher a where substr(a.type,1,1)='2' and substr(a.acode,1,2) in('05','06') order by a.vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Monthly Payment Report (Fig. In " + home_div_iden + ")", frm_qstr);
                    break;
                #endregion

                #region GRAPH : SALES
                case "F05226":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select b.aname as Mont_Name,sum(a.amt_sale) as saleamt,sum(a.bill_qty) as qty,a.acode from sale a,famst b where trim(a.acode)=trim(b.acode) and a.vchdate " + xprdrange + " and  a.branchcd <> 'DD'  and a.type not in ('47','4A') and substr(a.acode,1,2)!='02'  group by b.aname,a.acode order by saleamt desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(3);
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Sales Breakup (Top 10 Parties)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05229"://SALES VS COLL MONTH WISE
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select Month_Name,sum(sales) as sales,sum(collection) as collection from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,0 as sales,sum(cramt)-sum(Dramt) as collection,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + xprdrange + " and a.branchcd<>'88' and substr(type,1,1)='1' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(dramt)-sum(cramt) as sales,0 as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + xprdrange + " and a.branchcd<>'88' and substr(type,1,1)='4' and substr(acode,1,2) IN('16') and type!='47' group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) ) group by Month_Name,mth order by mth";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChart("Bar", "Sales vs Collection Month Wise");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05232": //NEW SO RECEIVE TREND
                    i0 = 1;
                    xprdrange1 = " between to_date('" + cDT1.Substring(0, 6) + Convert.ToString(fgen.make_double(cDT1.Substring(6, 4)) - 1) + "','dd/mm/yyyy')-1 and to_Date('" + cDT1 + "','dd/mm/yyyy')-1";
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select month_name,curr_yr,past_yr from (select upper(month_name) as month_name,sum(past_yr) as past_yr,sum(curr_yr) as curr_yr,mthnum,max(mthsno) as mthsno  from (select substr(mthname,1,3) as month_name,0 as past_yr,0 as curr_yr,mthnum,mthsno from mths union all select substr(to_Char(a.ORDDT,'MONTH'),1,3) as Month_Name,0 as past_yr,sum(a.qtyord*a.irate) as curr_yr,to_Char(a.ORDDT,'MM') as mth,0 from sOMAS a where a.ORDDT  " + xprdrange + " and a.branchcd <> 'DD' group by to_Char(a.ORDDT,'MM') ,substr(to_Char(a.ORDDT,'MONTH'),1,3) union all select substr(to_Char(a.ORDDT,'MONTH'),1,3) as Month_Name,sum(a.qtyord*a.irate) as past_yr,0 as curr_yr,to_Char(a.ORDDT,'MM') as mth,0 from sOMAS a where a.ORDDT  " + xprdrange1 + " and a.branchcd <> 'DD' group by to_Char(a.ORDDT,'MM') ,substr(to_Char(a.ORDDT,'MONTH'),1,3) ) group by upper(month_name),mthnum ) order by mthsno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChart("Bar", "New SO Received Trend");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05235"://COMP OF CY SALES TO LAST YEAR
                    i0 = 1;
                    string ctdt = "", xstr = "", kyrstr = "";
                    ctdt = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='R01'", "PARAMS").Trim();
                    xprdrange = " between to_date('" + ctdt + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select to_char(fmdate,'dd/mm/yyyy') as fmdate,to_char(todate,'dd/mm/yyyy') as todate from co where substr(code,1,length(trim(code))-4) like '" + co_cd + "' and fmdate " + xprdrange + " order by fmdate";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    foreach (DataRow dr in dt.Rows)
                    {
                        kyrstr = dr["fmdate"].ToString().Trim().Substring(8, 2) + "-" + dr["todate"].ToString().Trim().Substring(8, 2);
                        xstr = xstr + "SELECT 'F.Y :'||'" + kyrstr + "' as Month_Name,round(sum(a.Amt_sale)/" + home_divider + "00,2) as tot_bas,round(sum(a.bill_tot)/" + home_divider + "00,2) as tot_Gross from sale a where branchcd<>'DD' and a.type!='47' and a.vchdate between to_DATE('" + dr["fmdate"].ToString().Trim() + "','dd/mm/yyyy') and to_DATE('" + dr["todate"].ToString().Trim() + "','dd/mm/yyyy') group by 'F.Y :'||'" + kyrstr + "' union all ";
                    }
                    SQuery = xstr + " SELECT '-' as yrstr,0 as Bas_tot,0 as gr_tot from sale where 1=2 ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        StringBuilder sb = new System.Text.StringBuilder();
                        if (dt.Rows.Count == 1)
                        {

                            sb.Append("Your Tejaxo ERP has data of CY onwards,");
                            sb.Append("so, this Graph cannot be generated this year");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('" + sb.ToString() + "');", true);
                        }
                        hdnHAxisTitle_Bar.Value = "Year (F.Y Denotes Financial Year)";
                        OpenChartColumn("Bar", "Comparison of CY Sales To Last Year (Totals)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05238"://COMPARISON OF CY SALES TO LAST YEAR(MONTH ON MONTH)
                    i0 = 1;
                    xprdrange1 = " between to_date('" + cDT1.Substring(0, 6) + Convert.ToString(fgen.make_double(cDT1.Substring(6, 4)) - 1) + "','dd/mm/yyyy')-1 and to_Date('" + cDT1 + "','dd/mm/yyyy')-1";
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select * from (select upper(month_name) as month_name,sum(past_yr) as past_yr,sum(curr_yr) as curr_yr,mthnum,max(mthsno) as mthsno  from (select substr(mthname,1,3) as month_name,0 as past_yr,0 as curr_yr,mthnum,mthsno from mths union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,0 as past_yr,sum(a.amt_sale) as curr_yr,to_Char(a.vchdate,'MM') as mth,0 from sale a where a.vchdate  " + xprdrange + " and a.branchcd <> 'DD' group by to_Char(a.vchdate,'MM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.amt_sale) as past_yr,0 as curr_yr,to_Char(a.vchdate,'MM') as mth,0 from sale a where a.vchdate  " + xprdrange1 + " and a.branchcd <> 'DD' group by to_Char(a.vchdate,'MM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) ) group by upper(month_name),mthnum ) order by mthsno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(4);
                        dt.Columns.RemoveAt(3);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChart("Bar", "Comparison Of CY Sales To Last Year (Month On Month)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05241":
                    i0 = 1;
                    xdt = sysdt.Substring(6, 4) + sysdt.Substring(3, 2);
                    SQuery = "Select * from(Select * from(select substr(b.aname,1,10) as aname, trim(a.acode) as acode,sum(a.schedule) as schedule,sum(a.sale) as sales from (select acode,icode,total as schedule,0 as sale from schedule a where branchcd!='DD' and type like '4%' and to_chaR(vchdate,'yyyymm') ='" + xdt + "' union all select acode,icode,0 as schedule,iqtyout as sale from ivoucher a where branchcd!='DD' and type like '4%' and to_chaR(vchdate,'yyyymm')='" + xdt + "')a,famst b where trim(A.acodE)=trim(B.acode) group by b.aname,trim(a.Acode) having sum(a.schedule)>0 order by b.aname)order by schedule desc )where rownum<11 ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(1);
                        date1 = new DateTime(Convert.ToInt32(sysdt.Substring(6, 4)), Convert.ToInt32(sysdt.Substring(3, 2)), 1);
                        xmonth = date1.ToString("MMMM");
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Customers";
                        OpenChart("Bar", "Schedule vs Despatch (For The Month Of " + xmonth + ")");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05244":
                    i0 = 1;
                    GetPPMonth();
                    SQuery = "select  b.aname,a.acode,sum(a.ppmonth) as ppmonth,sum(a.pmonth) as pmonth,sum(a.ppmonth)-sum(a.pmonth) as Diff from (select trim(acode) as acode,sum(bill_tot) as ppmonth,0 as pmonth from sale where branchcd!='DD' and type!='47' and to_chaR(vchdate,'yyyymm')='" + ppmdate + "' group by trim(acode) union all select trim(acode) as acode,0 as ppmonth,sum(bill_tot) as pmonth from sale where branchcd!='DD' and type!='47' and to_chaR(vchdate,'yyyymm') = '" + pmdate + "' group by trim(acode))a, famst b where trim(a.acode)=trim(b.acode) group by b.aname,a.acode";
                    SQuery = "select aname,trim(acode) as acode,ppmonth as m_" + er2 + ",pmonth as m_" + er1 + ", (Diff*-1) as Diff,ROUND(((pmonth-ppmonth)/pmonth)*100,2) as Percentg from (" + SQuery + ") where ppmonth<pmonth order by diff desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Party Wise Sales Up", frm_qstr);
                    break;

                case "F05247":
                    i0 = 1;
                    GetPPMonth();
                    SQuery = "select b.aname,a.acode,sum(a.ppmonth) as ppmonth,sum(a.pmonth) as pmonth,sum(a.ppmonth)-sum(a.pmonth) as Diff from (select trim(acode) as acode,sum(bill_tot) as ppmonth,0 as pmonth from sale where branchcd!='DD' and type!='47' and to_chaR(vchdate,'yyyymm')='" + ppmdate + "' group by trim(acode) union all select trim(acode) as acode,0 as ppmonth,sum(bill_tot) as pmonth from sale where branchcd!='DD' and type!='47' and to_chaR(vchdate,'yyyymm')='" + pmdate + "' group by trim(acode))a, famst b where trim(a.acode)=trim(b.acode) group by b.aname,a.acode";
                    SQuery = "select aname,trim(acode),ppmonth as m_" + er2 + ",pmonth as m_" + er1 + ", Diff,ROUND(((ppmonth-pmonth)/ppmonth)*100,2) AS Percentg from (" + SQuery + ") where ppmonth>pmonth order by diff desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Party Wise Sales Down", frm_qstr);
                    break;

                case "F05250":
                    i0 = 1;
                    GetPPMonth();
                    SQuery = "select b.iname,a.icode,sum(a.ppmonth) as ppmonth,sum(a.pmonth) as pmonth,sum(a.ppmonth)-sum(a.pmonth) as Diff from (select trim(icode) as icode,sum(iamount) as ppmonth,0 as pmonth from ivoucher where branchcd!='DD' and type!='47' and type like '4%' and to_chaR(vchdate,'yyyymm')='" + ppmdate + "' group by trim(icode) union all select trim(icode) as icode,0 as ppmonth,sum(iamount) as pmonth from ivoucher where branchcd!='DD' and type!='47' and type like '4%' and to_chaR(vchdate,'yyyymm')='" + pmdate + "' group by trim(icode))a, item b where trim(a.icode)=trim(b.icode) group by b.iname,a.icode";
                    SQuery = "select trim(iname) as item_name,trim(icode) as icode,ppmonth as m_" + er2 + ",pmonth as m_" + er1 + ",(Diff*-1) as Diff,ROUND(((pmonth-ppmonth)/pmonth)*100,2) as Percentag from (" + SQuery + ") where ppmonth<pmonth   order by diff desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item Wise Sales Up", frm_qstr);
                    break;

                case "F05253":
                    i0 = 1;
                    GetPPMonth();
                    SQuery = "select b.iname,a.icode,sum(a.ppmonth) as ppmonth,sum(a.pmonth) as pmonth,sum(a.ppmonth)-sum(a.pmonth) as Diff from (select trim(icode) as icode,sum(iamount) as ppmonth,0 as pmonth from ivoucher where branchcd!='DD' and type!='47' and type like '4%' and to_chaR(vchdate,'yyyymm')='" + ppmdate + "' group by trim(icode) union all select trim(icode) as icode,0 as ppmonth,sum(iamount) as pmonth from ivoucher where branchcd!='DD' and type!='47' and type like '4%' and to_chaR(vchdate,'yyyymm')='" + pmdate + "' group by trim(icode))a, item b where trim(a.icode)=trim(b.icode) group by b.iname,a.icode";
                    SQuery = "select trim(iname) as item_name,trim(icode) as icode,ppmonth as m_" + er2 + ",pmonth as m_" + er1 + ",Diff,ROUND(((ppmonth-pmonth)/ppmonth)*100,2) AS Percentg  from (" + SQuery + ") where ppmonth>pmonth  order by diff desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item Wise Sales Down", frm_qstr);
                    break;

                case "F05256":
                    i0 = 1;
                    sbgauge = new StringBuilder();
                    sbgauge.Append("<html><body>");
                    CUST = 0; NMTH = 0; AVG_CUST = 0;
                    xdt = sysdt.Substring(6, 4) + sysdt.Substring(3, 2);
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select mth,count(*) as customers from (Select distinct to_Char(vchdate,'YYYYMM') as mth,acode from sale where branchcd<>'DD' and vchdate " + xprdrange + ") group by mth";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    foreach (DataRow dr in dt.Rows)
                    {
                        CUST = CUST + fgen.make_double(dr["customers"].ToString().Trim());
                        NMTH = NMTH + 1;
                    }
                    AVG_CUST = Math.Round(CUST / NMTH, 0);
                    SQuery = "";
                    SQuery = "Select ROUND((count(*) /" + AVG_CUST + ")*100,0) as CUST_TCH from (Select distinct to_Char(vchdate,'YYYYMM') as mth,acode from sale where branchcd<>'DD' and TO_CHAr(vchdate,'YYYYMM')= '" + xdt + "')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "")
                        {
                            fgen.msg("-", "AMSG", "No Data Exists");
                            return;
                        }
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        OpenChart("Gauge", "Average Number Of Active Customers In A Month");
                        ACUST = Convert.ToString(AVG_CUST).ToString().Replace(",", "");
                        ACUST = Spell.SpellAmount.comma(Convert.ToDecimal(ACUST));

                        MCUST = dt.Rows[0][0].ToString().Trim();
                        MCUST = MCUST.Replace(",", "");
                        MCUST = Spell.SpellAmount.comma(Convert.ToDecimal(MCUST));

                        sbgauge.Append("<br><b>Current Month customers served = " + Math.Round((fgen.make_double(MCUST) * fgen.make_double(ACUST) / 100), 0) + "");
                        sbgauge.Append("<br>Average number of customers served = " + ACUST + "</b>");

                        sbgauge.Append("</body></html>");
                        div2.InnerHtml = sbgauge.ToString();
                        div2.Visible = true;
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05259":
                    i0 = 1;
                    sbgauge = new StringBuilder();
                    sbgauge.Append("<html><body>");
                    CUST = 0; NMTH = 0; AVG_CUST = 0;
                    xdt = sysdt.Substring(6, 4) + sysdt.Substring(3, 2);
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select to_Char(vchdate,'YYYYMM') as mth,sum(amt_sale) as tot from sale where branchcd<>'DD' and type not in ('47','4A') and vchdate " + xprdrange + " group by to_Char(vchdate,'YYYYMM')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    foreach (DataRow dr in dt.Rows)
                    {
                        CUST = CUST + fgen.make_double(dr["tot"].ToString().Trim());
                        NMTH = NMTH + 1;
                    }
                    AVG_CUST = Math.Round(CUST / NMTH, 0);
                    SQuery = "";
                    SQuery = "Select ROUND((tot /" + AVG_CUST + ")*100,2) as CUST_TCH , tot from (Select to_Char(vchdate,'YYYYMM') as mth,sum(amt_sale) as tot from sale where branchcd<>'DD' and type not in ('47','4A') and TO_CHAr(vchdate,'YYYYMM')= '" + xdt + "')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        NMTH = 0;
                        NMTH = Math.Round(fgen.make_double(dt.Rows[0][1].ToString().Trim()), 0);
                        MCUST = fgen.make_double(NMTH.ToString()).ToString().Replace(",", "");
                        MCUST = Spell.SpellAmount.comma(Convert.ToDecimal(MCUST));
                        if (dt.Rows[0][0].ToString() == "")
                        {
                            fgen.msg("-", "AMSG", "No Data Exists");
                            return;
                        }
                        dt.Columns.RemoveAt(1);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        OpenChart("Gauge", "Speedometer: Sales By Value Percent Of Monthly Average");
                        ACUST = Convert.ToString(AVG_CUST).ToString().Replace(",", "");
                        ACUST = Spell.SpellAmount.comma(Convert.ToDecimal(ACUST));
                        sbgauge.Append("<br><b>Current Month Sales = " + MCUST + "");
                        sbgauge.Append("<br>Average Monthly Sales = " + ACUST + "</b>");
                        sbgauge.Append("</body></html>");
                        div2.InnerHtml = sbgauge.ToString();
                        div2.Visible = true;
                        sbg = new StringBuilder();
                        sbg.Append("<html><body>");
                        sbg.Append("<br>Basic Sales Value (All Plants) - As Per Invoices Raised");
                        sbg.Append("<br>But Excluding Purchase Returns And Inter Plant Sales ");
                        sbg.Append("</body></html>");
                        div5.InnerHtml = sbg.ToString();
                        div5.Visible = true;
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05262":
                    i0 = 1;
                    sbgauge = new StringBuilder();
                    sbgauge.Append("<html><body>");
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    xprdrange1 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    CUST = 0; NMTH = 0; AVG_CUST = 0;
                    SQuery = "Select to_Char(vchdate,'YYYYMM') as mth,sum(amt_sale) as tot from sale where branchcd<>'DD' and type not in ('47','4A') and vchdate " + xprdrange + " group by to_Char(vchdate,'YYYYMM')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    foreach (DataRow dr in dt.Rows)
                    {
                        CUST = CUST + fgen.make_double(dr["tot"].ToString().Trim());
                        NMTH = NMTH + 1;
                    }
                    AVG_CUST = Math.Round(CUST / NMTH, 0);
                    double proj_12mth = Math.Round(AVG_CUST * 12, 0);
                    MCUST = fgen.seek_iname(frm_qstr, co_cd, "Select sum(amt_sale) as tot from sale where branchcd<>'DD' and type not in ('47','4A') and vchdate " + xprdrange1 + " ", "tot");
                    SQuery = "";
                    SQuery = "Select ROUND((" + MCUST + "/" + proj_12mth + ")* 100,2) as CUST_TCH from dual";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "")
                        {
                            fgen.msg("-", "AMSG", "No Data Exists");
                            return;
                        }
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        OpenChart("Gauge", " Speedometer: Yearly Target");
                        AVG_CUST = Math.Round(AVG_CUST * 12, 0);
                        ACUST = Convert.ToString(AVG_CUST).ToString().Replace(",", "");
                        ACUST = Spell.SpellAmount.comma(Convert.ToDecimal(ACUST));
                        NMTH = Math.Round(fgen.make_double(MCUST), 0);
                        MCUST = Convert.ToString(NMTH).Replace(",", "");
                        MCUST = Spell.SpellAmount.comma(Convert.ToDecimal(MCUST));
                        sbgauge.Append("<br><b>YTD Sales = " + MCUST + "");
                        sbgauge.Append("<br>Target = Average Monthly Sales x 12 = " + ACUST + "</b>");
                        sbgauge.Append("</body></html>");
                        div2.InnerHtml = sbgauge.ToString();
                        div2.Visible = true;
                        sbg = new StringBuilder();
                        sbg.Append("<html><body>");
                        sbg.Append("<br>Basic Sales Value (All Plants) - As Per Invoices Raised");
                        sbg.Append("<br>But Excluding Purchase Returns And Inter Plant Sales ");
                        sbg.Append("</body></html>");
                        div5.InnerHtml = sbg.ToString();
                        div5.Visible = true;
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;
                #endregion

                #region GRAPH : FINANCE
                case "F05264":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select b.aname as Mont_Name,sum(a.amt_sale) as saleamt,sum(a.TOTqty) as qty,a.acode from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.vchdate " + xprdrange + " and a.branchcd<>'88' and a.type like '0%' and a.type<'08'  and  a.type not in ('04') and substr(A.acode,1,2)!='02' group by b.aname,a.acode order by saleamt desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(3);
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Purchase Breakup (Top 10 Vendors)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05267":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + xprdrange + " and  a.branchcd <> '88'  and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        string yr_op = ""; double run_Tot = 0;
                        int k = 0;
                        year = "yr_" + year;
                        yr_op = fgen.seek_iname(frm_qstr, co_cd, "Select sum(" + year + ") as opt from famstbal a where  a.branchcd <> 'DD' and substr(a.Acode,1,2)='16'", "opt");
                        if (yr_op == "") yr_op = "0";

                        dt.Columns.Add(new DataColumn("cum_tot", typeof(Decimal)));
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (k == 0)
                                run_Tot = run_Tot + (fgen.make_double(yr_op) + fgen.make_double(dr["tot_bas"].ToString()));
                            else
                                run_Tot = run_Tot + fgen.make_double(dr["tot_bas"].ToString());

                            dr["cum_tot"] = run_Tot;
                            k++;
                        }
                        dt.Columns.RemoveAt(3);
                        dt.Columns.RemoveAt(2);
                        dt.Columns.RemoveAt(1);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChartColumn("Bar", "Month Wise Debtor Closing Balance");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05270":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(cramt)-sum(Dramt) as collection,sum(cramt)-sum(Dramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + xprdrange + " and a.branchcd <> '88' and substr(type,1,1)='1' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(3);
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChartColumn("Bar", "Collection Trend");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05273":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.dramt)-sum(A.cramt) as expense,count(a.vchnum) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + xprdrange + " and a.branchcd <> '88' and substr(A.acode,1,1)='3' group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(3);
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChartColumn("Bar", "Expense Trend");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05276":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.amt_sale) as sale,sum(a.bill_qty) as qty,to_Char(a.vchdate,'YYYYMM') as mth from sale a where a.vchdate " + xprdrange + " and  a.branchcd <> 'DD' and a.type!='47' group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(3);
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChart("Bar", "Sales Trend, Current Year (" + cDT1 + " To " + sysdt + ")");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;
                #endregion

                #region GRAPH : SALARIES
                case "F05278":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select b.deptt_text as deptt,sum(a.totern) as totl from pay a,empmas b where a.branchcd <> 'DD' and a.date_ " + xprdrange + " and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode group by b.deptt_text order by totl desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Salary Breakup Department Wise");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05281":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select b.desg_text as desg,sum(a.totern) as totl from pay a,empmas b where a.branchcd <> 'DD' and a.date_ " + xprdrange + " and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode group by b.desg_text order by totl desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Salary Breakup Desgination Wise");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05284":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select c.name,B.grade,sum(a.totern) as totl from pay a,empmas b,type c  WHERE a.grade=c.type1 and c.id='I' and type1 like '0%' and a.branchcd <> 'DD' and a.date_ " + xprdrange + " and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode  group by B.GRADE,c.name order by totl desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(1);
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Salary Breakup Grade Wise");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05287":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    if (Convert.ToInt32(sysdt.Substring(3, 2)) > 3 && Convert.ToInt32(sysdt.Substring(3, 2)) < 12) fromdt = "01/04/" + sysdt.Substring(6, 4);
                    else
                    {
                        fromdt = "01/04/" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) - 1);
                    }
                    xprdrange1 = " between to_date('" + fromdt.Substring(0, 6) + Convert.ToString(fgen.make_double(fromdt.Substring(6, 4)) - 1) + "','dd/mm/yyyy')-1 and to_Date('" + fromdt + "','dd/mm/yyyy')-1";
                    SQuery = "select * from (select upper(month_name) as month_name,sum(past_yr) as past_yr,sum(curr_yr) as curr_yr,mthnum,max(mthsno) as mthsno  from (select substr(mthname,1,3) as month_name,0 as past_yr,0 as curr_yr,mthnum,mthsno from mths union all select substr(to_Char(a.date_,'MONTH'),1,3) as Month_Name,0 as past_yr,sum(a.totern) as curr_yr,to_Char(a.date_,'MM') as mth,0 from pay a where a.date_ " + xprdrange + " and a.branchcd <> 'DD' group by to_Char(a.date_,'MM') ,substr(to_Char(a.date_,'MONTH'),1,3) union all select substr(to_Char(a.date_,'MONTH'),1,3) as Month_Name,sum(a.totern) as past_yr,0 as curr_yr,to_Char(a.date_,'MM') as mth,0 from pay a where a.date_  " + xprdrange1 + " and a.branchcd <> 'DD' group by to_Char(a.date_,'MM') ,substr(to_Char(a.date_,'MONTH'),1,3) ) group by upper(month_name),mthnum ) order by mthsno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(4);
                        dt.Columns.RemoveAt(3);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChart("Bar", "Salary Trend");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;
                #endregion

                #region GRAPH : PRODUCTION
                case "F05289":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select round(sum(a.a1),2) as rzn1,round(sum(a.a2),2) as rzn2,round(sum(a.a3),2) as rzn3,round(sum(a.a4),2) as rzn4,round(sum(a.a5),2) as rzn5,round(sum(a.a6),2) as rzn6,round(sum(a.a7),2) as rzn7,round(sum(a.a8),2) as rzn8,round(sum(a.a9),2) as rzn9,round(sum(a.a10),2) as rzn10,round(sum(a.a11),2) as rzn11,round(sum(a.a12),2) as rzn12 ";
                    RowtoColumnData();
                    if (dt.Rows.Count > 0)
                    {
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Reasons";
                        OpenChartColumn("Pie", "Main Quality Problems");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05292":
                    i0 = 1;
                    SQuery = "select round(sum(a.num1)/60,2) as rzn1,round(sum(a.num2)/60,2) as rzn2,round(sum(a.num3)/60,2) as rzn3,round(sum(a.num4)/60,2) as rzn4,round(sum(a.num5)/60,2) as rzn5,round(sum(a.num6)/60,2) as rzn6,round(sum(a.num7)/60,2) as rzn7,round(sum(a.num8)/60,2) as rzn8,round(sum(a.num9)/60,2) as rzn9,round(sum(a.num10)/60,2) as rzn10,round(sum(a.num11)/60,2) as rzn11,round(sum(a.num12)/60,2) as rzn12 ";
                    RowtoColumnData();
                    if (dt.Rows.Count > 0)
                    {
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Reasons";
                        OpenChartColumn("Pie", "Main Down Time Reasons");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05295":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round((((sum(nvl(a.a2,0)+nvl(a.a4,0)))-(sum(nvl(a.a2,0))))/sum(nvl(a.a2,0)+nvl(a.a4,0)))*" + home_divider + "0,0) as tot_bas,to_Char(a.vchdate,'YYYYMM') as mth from " + frm_prodsheet + " a where a.vchdate " + xprdrange + " and a.branchcd<>'DD' and a.type in ('86','88') group by to_Char(a.vchdate,'YYYYMM'),substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChartColumn("Bar", "Monthly Production PPM");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05298":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    string timefor = "";
                    timefor = "a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+ a.num7+a.num8+a.num9+a.num10+a.num11+ a.num12";
                    SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round(sum(" + timefor + ")/60,2) as tot_bas,to_Char(a.vchdate,'YYYYMM') as mth from " + frm_prodsheet + " a where a.vchdate " + xprdrange + " and a.branchcd<>'DD' and a.type in ('86','88') group by to_Char(a.vchdate,'YYYYMM'),substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
                    if (co_cd == "HPPI" || co_cd == "SPPI")
                    {
                        //timefor = "a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+ a.num7+a.num8+a.num9+a.num10+a.num11+ a.num12";
                        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round(sum(" + timefor + ")/60,2) as tot_bas,to_Char(a.vchdate,'YYYYMM') as mth from " + frm_prodsheet + " a where a.vchdate " + xprdrange + " and a.branchcd<>'DD' and a.type in ('86','88') group by to_Char(a.vchdate,'YYYYMM'),substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";

                        SQuery = "SELECT MONTH_NAME,SUM(TOT_bAS) AS TOT_bAS,mth FROM (SELECT substr(to_Char(vchdate,'MONTH'),1,3) as Month_Name,SUM(IS_NUMBER(COL3)) AS TOT_BAS,to_Char(vchdate,'YYYYMM') as mth FROM " + frm_inspvch + " WHERE BRANCHCD='" + mbr + "' AND TYPE='55' AND VCHDATE " + xprdrange + " GROUP BY COL1,substr(to_Char(vchdate,'MONTH'),1,3),to_Char(vchdate,'YYYYMM')) GROUP BY mth,MONTH_NAME ORDER BY mth";
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChartColumn("Bar", "Monthly Down Time in Hrs");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05301":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select to_CHAR(a.dlv_date, 'MONTH') as delvdt1 ,  sum(a.ACTUALCOST ) as qty ,to_CHAR(a.dlv_date, 'yyyymm') as delvdt from budgmst a, famst b  where a.branchcd<>'DD' and a.type='46' and trim(a.acode)=trim(b.acode) and a.dlv_Date " + xprdrange + " and a.ACTUALcost>a.jobcardqty and a.jobcardrqd='Y' and a.ACTUALCOST>0  group by to_CHAR(a.dlv_date, 'yyyymm'), to_CHAR(a.dlv_date, 'MONTH') order by to_CHAR(a.dlv_date, 'yyyymm')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Pending Sales Schedules Vs Job Card YTD (Month Wise)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05304":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select  distinct to_CHAR(a.vchdate, 'MONTH') as delvdt1, count(distinct a.vchnum) as cnt ,to_CHAR(a.vchdate, 'yyyymm') as delvdt from costestimate a where a.branchcd<>'DD' and a.type='30' and a.vchdate " + xprdrange + " and a.status!='Y' and a.qty>0  group by to_CHAR(a.vchdate, 'MONTH') ,to_CHAR(a.vchdate, 'yyyymm') order by to_char(a.vchdate, 'yyyymm')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Pending Job Cards Not Yet Closed YTD (Month Wise)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05307":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select B.ANAME,sum(a.ACTUALCOST ) as qty from budgmst a, famst b  where a.branchcd<>'DD' and a.type='46' and  trim(a.acode)=trim(b.acode) and a.dlv_Date " + xprdrange + "  and a.ACTUALcost>a.jobcardqty and a.jobcardrqd='Y' and a.ACTUALCOST>0  group by B.ANAME order by qty desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Pending Sales Schedules Vs Job Card YTD (Party Wise Top 10)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05310":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select  distinct B.ANAME,count(distinct a.vchnum) as cnt,to_CHAR(a.vchdate, 'yyyymm') as delvdt from costestimate a,famst b where a.branchcd<>'DD' and trim(a.acode)=trim(b.acode) and a.type='30' and a.vchdate " + xprdrange + " and a.status!='Y' and a.qty>0  group by B.ANAME,to_CHAR(a.vchdate, 'yyyymm') order by to_char(a.vchdate, 'yyyymm')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(2);
                        hdnChartData.Value = DataTableToJSArray_Top10(dt, frm_formID);
                        OpenChart("Pie", "Pending Job Cards (Not Yet Closed) YTD (Party Wise Top 10)");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;
                #endregion

                #region GRID  : PRODUCTION
                case "F05312"://Job Cards Not Made
                    i0 = 1;
                    SQuery = "Select distinct trim(c.maker)||':'||a.socat as Category,B.aname as Customer,C.iname as Item,C.cpartno as Partno ,a.desC_ as Delv_date,BUDGETCOST as Delv_Qty,ACTUALCOST as XWORD ,a.icode,trim(a.solink)||trim(a.srno) as solink,a.SoRemarks,a.jobcardqty,a.jobcardno,a.rowid as Iden,a.jobcardrqd,Req_Closedby,a.vchnum as Ordno,to_char(a.vchdate,'yyyymmdd') as Orddt from budgmst a, famst b , item c where nvl(a.app_by,'-')!='-' and a.branchcd!='DD' and a.type='46' and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND A.VCHDATE between to_date('" + sysdt + "','dd/MM/yyyy')-200 and to_date('" + sysdt + "','dd/MM/yyyy') and trim(a.acode) like '%' and trim(a.icode) like '%' and a.socat like '%' and a.ACTUALcost>a.jobcardqty and a.jobcardrqd='Y' and a.ACTUALCOST>0 ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Job Cards Not Made", frm_qstr);
                    break;

                case "F05315"://Issues against Job Card ( Reqd Vs Actual )
                    i0 = 1;
                    SQuery = "select distinct A.vchnum AS Job_No,to_char(A.vchdate,'dd/mm/yyyy') as Dated,A.type,B.INAME as Item_Name,A.QTY as Qty,to_char(A.vchdate,'yyyymmdd') as vdd from costestimate A,ITEM B  WHERE a.branchcd!='DD' and a.type='30' and A.vchnum<>'000000' and A.SRNO=0 AND trim(A.ICODE)=trim(B.ICODE) order by vdd desc ,A.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Issues Against Job Card (Reqd Vs Actual)", frm_qstr);
                    break;

                case "F05318"://Paper Allocated As Issue
                    i0 = 1;
                    SQuery = "select distinct TRIM(ICODE) as icode,TRIM(invno) as Job_No,TRIM(invdate) as Job_Dt,SUM(TAKEN) AS ALLOC,SUM(ISS) AS ISS,sum(taken)-sum(iss) as Balance  from (select ICODE,trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,iqtyout-nvl(iqtyin,0) as iss,0 as taken from ivoucher where branchcd!='DD' and (type='30' or type='13') and substr(icode,1,2)<='02' and vchdate between to_date('" + sysdt + "','dd/MM/yyyy')-200 and to_date('" + sysdt + "','dd/MM/yyyy')  union all select COL9,trim(vchnum) as job_no,TO_CHAR(vchdate,'DD/MM/YYYY')As job_dt,0 as iss,to_number(col7) as taken from costestimate where trim(col7)!='-' and branchcd!='DD' and type='30' and  substr(COL9,1,2) IN('01','02') AND (VCHNUM,VCHDATE) IN ( select DISTINCT trim(invno) as invno,invdate from ivoucher where branchcd!='DD' and type='30' and vchdate between to_date('" + sysdt + "','dd/MM/yyyy')-200 and to_date('" + sysdt + "','dd/MM/yyyy')  and substr(icode,1,2)<='02')) group by TRIM(INVNO),TRIM(invdate),TRIM(ICODE)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Paper Allocated Vs Issue", frm_qstr);
                    break;

                case "F05321"://JobCard Completion Report
                    i0 = 1;
                    SQuery = "select distinct a.vchnum as Job_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,e.iname,a.acode as Customer,a.icode as Item,a.qty as Job_Qty,a.COL13 as Ups,decode(trim(a.dropdate),'-',1,trim(a.dropdate)) as Sheets,a.COL15 as Wastage,a.COL16 as Color,a.COL17 as tot_sheet,nvl(b.tot,0) as prodn,to_char(a.vchdate,'yyyymmdd') as vdd from item e,costestimate a left outer join (Select invno,invdate,icode,sum(iqtyin) as tot from ivoucher where branchcd!='DD' and type in('15','16') group by invno,invdate,icode) b on trim(a.icode)=trim(b.icode) and trim(a.vchnum)=trim(b.invno) and a.vchdate=b.invdate where a.branchcd!='DD' and a.type='30' and a.srno= 1 and trim(a.icode)=trim(e.icode)  and a.vchdate between to_date('" + sysdt + "','dd/MM/yyyy')-200 and to_date('" + sysdt + "','dd/MM/yyyy') order by vdd desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("JobCard Completion Report", frm_qstr);
                    break;

                case "F05324"://Pending Sales Schedules (Job Card Not Made) YT
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select distinct to_CHAR(a.dlv_date, 'MONTH') as delvdt1 ,  sum(ACTUALCOST ) as qty ,to_CHAR(a.dlv_date, 'yyyymm') as delvdt   from budgmst a, famst b , item c where a.branchcd <> 'DD' and a.type='46' and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and a.dlv_Date " + xprdrange + "  and a.ACTUALcost>a.jobcardqty and a.jobcardrqd='Y' and a.ACTUALCOST>0  group by to_CHAR(a.dlv_date, 'yyyymm'), to_CHAR(a.dlv_date, 'MONTH') order by to_CHAR(a.dlv_date, 'yyyymm')";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Sales Schedules (Job Card Not Made) YT", frm_qstr);
                    break;

                case "F05327": //Pending Job Cards(Not Yet Closed) YTD Grid
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "Select distinct to_CHAR(a.vchdate, 'MONTH') as delvdt1, count(distinct a.vchnum) as cnt ,to_CHAR(a.vchdate, 'yyyymm') as delvdt   from costestimate a where a.branchcd <> 'DD' and a.type='30' and a.vchdate " + xprdrange + "  and a.status!='Y' and a.qty>0  group by to_CHAR(a.vchdate, 'MONTH') ,to_CHAR(a.vchdate, 'yyyymm') order by to_char(a.vchdate, 'yyyymm')";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Job Cards (Not Yet Closed) YTD", frm_qstr);
                    break;

                case "F05330"://Sale Schedule vs Shipment Made
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select distinct b.aname,c.iname,sum(budgetcost) as plan_qty,sum(sale ) as sale_qty,trim(a.acode) as acode,trim(a.icode) as icode from (select acode,icode,budgetcost,0 as sale from budgmst where branchcd!='DD' and type='46' and vchdate " + xprdrange + " union all select acode,icode,0 as budgetcost,iqtyout as sale from ivoucher where branchcd!='DD' and type like '4%' and type!='47'  and vchdate " + xprdrange + " )a, famst b , item c where trim(A.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,a.icode,b.aname,c.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sale Schedule Vs Shipment Made", frm_qstr);
                    break;
                #endregion

                #region GRAPH : QC
                case "F05332"://Customer Wise rejection PPm
                case "F05335"://Customer wise Rejections Percent
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    popsql = "select upper(trim(b.aname)) as Customer_Name,sum(a.iqtyout) as Sale,0 as Rejn,a.acode from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd!='DD' and a.vchdate " + xprdrange + " and substr(a.type,1,1)='4' and a.type not in ('47','45') group by upper(trim(b.aname)),a.acode union all select upper(trim(b.aname)) as Customer_Name,0 as Sale,sum(a.iqtyin) as Rejn,a.acode from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd!='DD' and a.vchdate " + xprdrange + " and substr(a.type,1,2)='04' group by upper(trim(b.aname)),a.acode";
                    SQuery = "Select * from(select Customer_Name,sum(sale) as Sale_qty,sum(rejn) as Rejn_Rcv_Qty,decode(sum(sale),0,'N/a',round((sum(rejn)/sum(sale)),5)*" + home_divider + "0) as Rejn_PPM,decode(sum(sale),0,'N/a',round(((sum(rejn)/sum(sale)))*100,2)) as Rejn_percent,acode from (" + popsql + ") group by Customer_Name,acode having sum(sale)>0 Order by round((sum(rejn)/sum(sale)),5)*" + home_divider + "0 desc) where rownum<11";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt1 = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(5);
                        dt.Columns.RemoveAt(4);
                        dt.Columns.RemoveAt(2);
                        dt.Columns.RemoveAt(1);
                        dt1.Columns.Add(new DataColumn("Cust_Name", typeof(string)));
                        dt1.Columns.Add(new DataColumn("rej_val", typeof(decimal)));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow nrow = dt1.NewRow();
                            nrow["Cust_Name"] = dt.Rows[i][0].ToString().Trim();
                            nrow["rej_val"] = dt.Rows[i][1].ToString().Trim();
                            dt1.Rows.Add(nrow);
                        }
                        dt = new DataTable();
                        dt = dt1;
                        if (frm_formID == "F05332")
                        {
                            header_n = "Customer Wise Rejections PPM";
                            hdnVAxisTitle_Bar.Value = "Rejection PPM";
                        }
                        else
                        {
                            header_n = "Customer Wise Rejections Percent (These Are The Customers With Worst Rejection-To-Sales Ratio)";
                            hdnVAxisTitle_Bar.Value = "Rejection Percentage";
                        }
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Customers";
                        OpenChartColumn("Bar", header_n);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;

                case "F05338": //Sale VS Rejection
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    popsql = "select upper(trim(b.aname)) as Customer_Name,sum(a.iqtyout) as Sale,0 as Rejn,a.acode from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd!='DD' and a.vchdate " + xprdrange + " and substr(a.type,1,1)='4' and a.type not in ('47','45') group by upper(trim(b.aname)),a.acode union all select upper(trim(b.aname)) as Customer_Name,0 as Sale,sum(a.iqtyin) as Rejn,a.acode from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd!='DD' and a.vchdate " + xprdrange + " and substr(a.type,1,2)='04' group by upper(trim(b.aname)),a.acode";
                    SQuery = "Select * from(select Customer_Name,sum(sale) as Sale_qty,sum(rejn) as Rejn_Rcv_Qty,decode(sum(sale),0,'N/a',round((sum(rejn)/sum(sale)),5)*" + home_divider + "0) as Rejn_PPM,decode(sum(sale),0,'N/a',round(((sum(rejn)/sum(sale)))*100,2)) as Rejn_percent,acode from (" + popsql + ") group by Customer_Name,acode having sum(sale)>0 Order by round((sum(rejn)/sum(sale)),5)*" + home_divider + "0 desc) where rownum<11";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.RemoveAt(5);
                        dt.Columns.RemoveAt(4);
                        dt.Columns.RemoveAt(3);
                        hdnChartData.Value = DataTableToJSArray(dt, frm_formID);
                        hdnHAxisTitle_Bar.Value = "Year " + cDT1.Substring(6, 4) + "-" + Convert.ToString(fgen.make_double(sysdt.Substring(6, 4)) + 1);
                        OpenChart("Bar", "Sales vs Rejection");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Data Exists");
                    }
                    break;
                #endregion

                #region FINANCIAL STATISTICS
                case "F05340":
                    i0 = 1;
                    SQuery = "select m.aname as Party,to_char(sum(n.total),'99,99,99,999') as Total_Amt, to_char(sum(n.slab1),'99,99,99,999') as Current_Os, to_char(sum(n.slab2),'99,99,99,999') as OVER_30_60, to_Char(sum(n.slab3),'99,99,99,999') as OVER_61_90, to_char(sum(n.slab4),'99,99,99,999') as OVER_90_180,to_char(sum(n.slab5),'99,99,99,999') as OVER_181,n.acode,m.Payment as P_days,m.Climit  as Cr_limit, '-' as Zcode from (SELECT acode,dramt-cramt as total,(CASE WHEN (sysdate-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (sysdate-invdate BETWEEN 30 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (sysdate-invdate BETWEEN 60 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (sysdate-invdate BETWEEN 90 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (sysdate-invdate > 180) THEN dramt-cramt END) as slab5 from  recdata where branchcd <> 'DD' and branchcd <> '88' ) n left outer join famst m on trim(n.acode)=trim(m.acode) where substr(n.acode,1,2) in ('16') and n.total<>0 group by n.acode,m.aname,m.addr1,m.climit,m.payment,m.bssch order by Total_Amt desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Debtors Ageing 30-60-90", frm_qstr);
                    break;

                case "F05343":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select distinct a.branchcd AS BRANCHCD,B.ANAME AS PARTY_NAME, A.vchnum as Vch_No,to_char(A.vchdate,'dd/mm/yyyy') as Vch_Date,to_char(sum(a.dramt),'99," + numbr_fmt + "') as Amount,a.naration,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD from voucher A, FAMST B where TRIM(A.ACODe)=TRIM(B.ACODe) AND SUBSTR(a.ACODE,1,2) IN ('05','06') AND A.type like '2%' and a.branchcd <> '88' and A.vchdate " + xprdrange + " group by b.aname,a.vchnum,to_char(A.vchdate,'dd/mm/yyyy'),a.naration,TO_CHAR(A.VCHDATE,'YYYYMMDD'),a.branchcd ORDER BY VDD DESC,A.vchnum DESC";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Payments Recently Made", frm_qstr);
                    break;

                case "F05346":
                    i0 = 1;
                    scode = "";
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    m4 = Convert.ToString(year).Trim() + "04";
                    m5 = Convert.ToString(year).Trim() + "05";
                    m6 = Convert.ToString(year).Trim() + "06";
                    m7 = Convert.ToString(year).Trim() + "07";
                    m8 = Convert.ToString(year).Trim() + "08";
                    m9 = Convert.ToString(year).Trim() + "09";
                    m10 = Convert.ToString(year).Trim() + "10";
                    m11 = Convert.ToString(year).Trim() + "11";
                    m12 = Convert.ToString(year).Trim() + "12";
                    m1 = Convert.ToString(fgen.make_double(year) + 1).Trim() + "01";
                    m2 = Convert.ToString(Convert.ToDecimal(year) + 1).Trim() + "02";
                    m3 = Convert.ToString(Convert.ToDecimal(year) + 1).Trim() + "03";
                    SQuery = "select * from (" + mq0 + mq1 + mq2 + " FCTYPE,d.aname as bankname,c.acode as bankcd,a.srno from VOUCHER a,famst b,type c,famst d where TRIM(A.ACODE)=TRIM(B.ACODE) and TRIM(A.type)=TRIM(c.type1) and c.id='V' and trim(c.acode)=trim(d.acode) and a.branchcd||a.type||a.VCHNUM||to_char(a.vchdate,'dd/mm/yyyy') ='" + scode + "') where trim(acode)<>trim(bankcd) order by srno ";
                    SQuery = "Select  b.Aname as Account,substr(a.acode,1,2) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.Dramt-a.cramt),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.Dramt-a.cramt),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.Dramt-a.cramt),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.Dramt-a.cramt),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.Dramt-a.cramt),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.Dramt-a.cramt),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.Dramt-a.cramt),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.Dramt-a.cramt),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.Dramt-a.cramt),0) as Dec ";
                    SQuery = SQuery + " ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.Dramt-a.cramt),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.Dramt-a.cramt),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.Dramt-a.cramt),0) as Mar,a.acode from voucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd <> '88' and a.vchdate " + xprdrange + " and substr(a.acode,1,2)>='20' group by a.acode,b.aname,to_char(vchdate,'yyyymm'),substr(a.acode,1,2)  ";
                    SQuery = "Select a.Account,to_char(sum(a.April)+sum(a.may)+sum(a.june)+sum(a.july)+sum(a.August)+sum(a.sept)+sum(a.oct)+sum(a.nov)+sum(a.dec)+sum(a.jan)+sum(a.feb)+sum(a.mar),'99,99,99,999') as total,to_char(sum(a.April),'99,99,99,999') as April,to_char(sum(a.May),'99,99,99,999') as May,to_char(sum(a.June),'99,99,99,999') as June,to_char(sum(a.July),'99,99,99,999') as July,to_char(sum(a.August),'99,99,99,999') as August,to_char(sum(a.Sept),'99,99,99,999') as Sept,to_char(sum(a.oct),'99,99,99,999') as Oct,to_char(sum(a.Nov),'99,99,99,999') as Nov,to_char(sum(a.Dec),'99,99,99,999') as Dec,to_char(sum(a.Jan),'99,99,99,999') as Jan,to_char(sum(a.Feb),'99,99,99,999') as Feb,to_char(sum(a.Mar),'99,99,99,999') as Mar,a.Acode,b.name,a.grp from (" + SQuery + ")a , type b where b.id='Z' and substr(a.acode,1,2)=trim(b.type1) group by a.Grp,a.account,a.acode,b.name order by a.acode ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("P & L -12 Month Trend", frm_qstr);
                    break;



                case "F05352":
                    i0 = 1;
                    xprdrange1 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + cDT1 + "','dd/mm/yyyy')-1";
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    mq0 = "select 'NODRILL' as fstr, b.aname,a.Acode,to_char(sum(a.opening),'99," + numbr_fmt + "') as opening,to_char(sum(a.cdr),'99," + numbr_fmt + "') as debits,to_char(sum(a.ccr),'99," + numbr_fmt + "') as credits,to_char(sum(a.opening)+sum(a.cdr)-sum(a.ccr),'99," + numbr_fmt + "') as closing,b.bssch from (Select acode,  yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from FAMSTbal where branchcd <> 'DD' union all  ";
                    mq1 = mq0 + " select Acode,sum(DRAMT)-sum(CRAMT) as op,0 as cdr,0 as ccr,0 as clos from voucher where branchcd <> '88' and type like '%' and vchdate " + xprdrange1 + " GROUP BY ACODE union all ";
                    SQuery = mq1 + " select Acode,0 as op,sum(DRAMT) as cdr,sum(CRAMT) as ccr,0 as clos from voucher where branchcd <> '88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE )a, famst b where trim(A.acode)=trim(b.acode) and trim(nvl(b.fbt,'N'))='Y' group by b.aname,b.bssch,a.ACODE having abs(sum(a.opening))+abs(sum(a.cdr))+abs(sum(a.ccr))<>0 ";

                    xprdrange1 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = "";
                    xdt = sysdt.Substring(6, 4) + sysdt.Substring(3, 2);
                    mq0 = "select 'NODRILL' as fstr, b.aname,a.Acode,to_char(sum(a.opening),'99," + numbr_fmt + "') as opening,to_char(sum(a.cdr),'99," + numbr_fmt + "') as debits,to_char(sum(a.ccr),'99," + numbr_fmt + "') as credits,to_char(sum(a.opening)+sum(a.cdr)-sum(a.ccr),'99," + numbr_fmt + "') as closing,b.bssch from (Select acode, yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from FAMSTbal where 1=2 and branchcd <> 'DD' union all  ";
                    mq1 = mq0 + " select Acode,sum(DRAMT)-sum(CRAMT) as op,0 as cdr,0 as ccr,0 as clos from voucher where branchcd <> '88' and type like '%' and vchdate " + xprdrange1 + " and 1=2 GROUP BY ACODE union all ";
                    mq3 = mq1 + " select Acode,0 as op,sum(DRAMT) as cdr,sum(CRAMT) as ccr,0 as clos from voucher where branchcd <> '88' and type like '%' and to_char(vchdate,'yyyymm')='" + xdt + "' GROUP BY ACODE )a, famst b where trim(A.acode)=trim(b.acode) and trim(nvl(b.fbt,'N'))='Y' group by b.aname,b.bssch,a.ACODE having abs(sum(a.opening))+abs(sum(a.cdr))+abs(sum(a.ccr))<>0 ";

                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);

                    DataTable dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq3);

                    DataTable dt3 = new DataTable();
                    dt3.Columns.Add(new DataColumn("acode", typeof(string)));
                    dt3.Columns.Add(new DataColumn("aname", typeof(string)));
                    dt3.Columns.Add(new DataColumn("pmnth", typeof(decimal)));
                    dt3.Columns.Add(new DataColumn("cmnth", typeof(decimal)));

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow nrow = dt3.NewRow();
                        nrow["acode"] = dt1.Rows[i][2].ToString().Trim();
                        nrow["aname"] = dt1.Rows[i][1].ToString().Trim();
                        nrow["pmnth"] = dt1.Rows[i][6].ToString().Trim();
                        nrow["cmnth"] = 0;
                        dt3.Rows.Add(nrow);
                    }
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        DataRow nrow = dt3.NewRow();
                        nrow["acode"] = dt2.Rows[i][2].ToString().Trim();
                        nrow["aname"] = dt2.Rows[i][1].ToString().Trim();
                        nrow["pmnth"] = 0;
                        nrow["cmnth"] = dt2.Rows[i][6].ToString().Trim();
                        dt3.Rows.Add(nrow);
                    }
                    DataTable dt4 = new DataTable();
                    if (dt3.Rows.Count > 0)
                    {
                        DataView dtview = new DataView(dt3);
                        dtview.Sort = "acode";
                        dt4 = dtview.ToTable();
                    }
                    DataTable dt5 = new DataTable();
                    dt5.Columns.Add(new DataColumn("Acode", typeof(string)));
                    dt5.Columns.Add(new DataColumn("Account_Name", typeof(string)));
                    dt5.Columns.Add(new DataColumn("Upto_Last_Month", typeof(decimal)));
                    dt5.Columns.Add(new DataColumn("Month_Avg", typeof(decimal)));
                    dt5.Columns.Add(new DataColumn("Current_Month", typeof(decimal)));
                    dt5.Columns.Add(new DataColumn("Diff_Amt", typeof(decimal)));

                    string xmdt = sysdt.Substring(3, 2);

                    mtnno = fgen.seek_iname(frm_qstr, co_cd, "select mthsno from mths where mthnum='" + xmdt + "'", "mthsno");
                    int j = 0;
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        if (dt4.Rows[i][2].ToString().Trim() != "0")
                        {
                            DataRow nrow = dt5.NewRow();
                            nrow["acode"] = dt4.Rows[i][0].ToString().Trim();
                            nrow["Account_Name"] = dt4.Rows[i][1].ToString().Trim();
                            nrow["Upto_Last_Month"] = Math.Round(fgen.make_double(dt4.Rows[i][2].ToString().Trim()), 0);
                            nrow["Month_Avg"] = Math.Round(fgen.make_double(dt4.Rows[i][2].ToString().Trim()) / Convert.ToInt32(mtnno), 0);
                            nrow["Current_Month"] = Math.Round(fgen.make_double(dt4.Rows[i][3].ToString().Trim()), 0);
                            nrow["Diff_Amt"] = Math.Round(fgen.make_double(nrow["Month_Avg"].ToString().Trim()) - fgen.make_double(nrow["Current_Month"].ToString().Trim()), 0);
                            dt5.Rows.Add(nrow);
                        }
                        else
                        {
                            j = dt5.Rows.Count - 1;
                            dt5.Rows[j][4] = Math.Round(fgen.make_double(dt4.Rows[i][3].ToString().Trim()), 0);
                            dt5.Rows[j][5] = Math.Round(fgen.make_double(dt5.Rows[j][3].ToString().Trim()) - fgen.make_double(dt5.Rows[j][4].ToString().Trim()), 0);
                        }
                    }

                    nmth1 = 0; nmth2 = 0; totamt = 0;

                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        if (dt5.Rows[i][0].ToString().Trim().Substring(0, 2) == "20")
                        {
                            nmth1 = Math.Round(nmth1 + fgen.make_double(dt5.Rows[i][4].ToString().Trim()), 2);
                        }
                        else
                        {
                            nmth2 = Math.Round(nmth2 + fgen.make_double(dt5.Rows[i][4].ToString().Trim()), 2);
                        }
                    }
                    totamt = nmth1 - nmth2;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    Session["send_dt"] = dt5;
                    fgen.Fn_open_rptlevelJS("Fund Flow", frm_qstr);
                    break;

                case "F05355":
                    i0 = 1;
                    xprdrange = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + sysdt + "','dd/mm/yyyy')";
                    SQuery = "select distinct B.ANAME, A.vchnum as Vch_No,to_char(A.vchdate,'dd/mm/yyyy') as Vch_Date,to_char(sum(a.dramt),'99," + numbr_fmt + "') as Amount,a.naration,to_char(A.vchdate,'yyyymmdd') as vdd from voucher A, FAMST B where TRIM(A.ACODe)=TRIM(B.ACODe) AND SUBSTR(a.ACODE,1,2) IN ('05','06') AND A.type in ('10','20') and a.branchcd <> '88' and A.vchdate " + xprdrange + " group by b.aname,a.vchnum,to_char(A.vchdate,'dd/mm/yyyy'),a.naration, to_char(A.vchdate,'yyyymmdd') order by vdd desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Cash Book Entry", frm_qstr);
                    break;
                #endregion
                #endregion
            }

            if (i0 != 1)
            {
                if (SQuery.Length > 1)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                }
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
            else
            {
                switch (val)
                {
                    case "F05110":
                        DataTable dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable();
                        DataRow dr2 = dtm.NewRow(); dt3 = new DataTable(); dt4 = new DataTable();
                        dt2 = new DataTable();
                        dtm.Columns.Add("Customer", typeof(string));
                        dtm.Columns.Add("Item", typeof(string));
                        dtm.Columns.Add("Mfg_Plant", typeof(string));
                        dtm.Columns.Add("PO", typeof(string));
                        dtm.Columns.Add("PO_Due_Date", typeof(string));
                        dtm.Columns.Add("Late_Days", typeof(string));
                        dtm.Columns.Add("PO_Qty", typeof(double));
                        dtm.Columns.Add("Pending_Qty", typeof(double));
                        dtm.Columns.Add("Planned_Qty", typeof(double));
                        dtm.Columns.Add("Finished_Stock_Qty", typeof(double));
                        dtm.Columns.Add("Qty_On_Water", typeof(double));
                        dtm.Columns.Add("Qty_in_Air", typeof(double));
                        dtm.Columns.Add("Past_Due_Qty", typeof(double));
                        dtm.Columns.Add("Total_in_6_Week_Mrp", typeof(double));

                        SQuery = "select * from (select Product_Name,party,acode,fstr,substr(nvl(Last_rmk,'-'),12,20) as Plan_Rmk,Plan_Qty,RG1_Qty,da_Qty,CI_qty,txi_Qty,Curr_Stock,Pend_Ord,Pend_DA,Pend_CI,Pend_TI,Unit,Part_no,Erp_Code,length(substr(trim(nvl(Last_rmk,'-')),1,50)) as crit_len  from (select trim(b.iname) as Product_Name,trim(c.aname) as party,max(nvl(mcomment,'-')) as Last_rmk,sum(Qty) as Plan_Qty,sum(PRD_Qty) as RG1_Qty,sum(da_Qty) as da_Qty,sum(CI_qty) as CI_qty,sum(txi_Qty) as txi_Qty,sum(fgstk) as Curr_Stock,sum(Order_bal) as Pend_Ord,sum(Qty)-sum(da_Qty) as Pend_DA,sum(Qty)-sum(CI_qty) as Pend_CI,sum(Qty)-sum(txi_Qty) as Pend_TI,b.unit,b.cpartno as Part_no,trim(a.ICODE) as Erp_Code,trim(a.acode) as acode,trim(a.acode)||trim(a.icode) as fstr  from  (Select to_Char(edt_Dt,'yyyy-mm-dd')||'-'||trim(nvl(mcomment,'-')) as mcomment,acode,ICODE,nvl(num01,0) as Qty,0 as fgstk,0 as DA_qty,0 as CI_Qty,0 as txi_qty,0 as prd_Qty,0 as Order_bal from sl_plan where branchcd='" + mbr + "' and type='SL' and num01>0 and wk_Ref>0 and wk_ref in (" + value1 + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,qtysupp as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,0 as Order_bal from despatch where branchcd='" + mbr + "' and type like '4F' and to_Char(packdate,'yyyy')||to_Char(packdate,'ww') in (" + value1 + ")  union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,iqtyout as cust_inv,0 as tax_inv,0 as prd_Qty,0 as Order_bal from ivoucherp where branchcd='" + mbr + "' and type like '4F' and to_Char(vchdate,'yyyy')||to_Char(vchdate,'ww') in (" + value1 + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,iqtyout as tax_inv,0 as prd_Qty,0 as Order_bal from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_Char(vchdate,'yyyy')||to_Char(vchdate,'ww') in (" + mbr + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,iqtyin as prd_Qty,0 as Order_bal from ivoucher where branchcd='" + mbr + "' and type like '17%' and to_Char(vchdate,'yyyy')||to_Char(vchdate,'ww') in (" + value1 + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,qtyord as Order_bal from somas where weight='" + mbr + "' and type like '4%' union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,-1*iqtyout as Order_bal from ivoucher where branchcd='" + mbr + "' and type like '4%' ) a,item b,famst c where trim(a.icode)=trim(B.icode) and trim(a.acode)=trim(c.acode) group by trim(b.iname),trim(a.acode),trim(c.aname),b.cpartno,b.unit,trim(a.ICODE) ) ) order by crit_len desc,acode asc";
                        SQuery = "select * from (select Product_Name,party,acode,fstr,substr(nvl(Last_rmk,'-'),12,20) as Plan_Rmk,Plan_Qty,RG1_Qty,da_Qty,CI_qty,txi_Qty,Curr_Stock,Pend_Ord,Pend_DA,Pend_CI,Pend_TI,Unit,Part_no,Erp_Code,length(substr(trim(nvl(Last_rmk,'-')),1,50)) as crit_len  from (select trim(b.iname) as Product_Name,trim(c.aname) as party,max(nvl(mcomment,'-')) as Last_rmk,sum(Qty) as Plan_Qty,sum(PRD_Qty) as RG1_Qty,sum(da_Qty) as da_Qty,sum(CI_qty) as CI_qty,sum(txi_Qty) as txi_Qty,sum(fgstk) as Curr_Stock,sum(Order_bal) as Pend_Ord,sum(Qty)-sum(da_Qty) as Pend_DA,sum(Qty)-sum(CI_qty) as Pend_CI,sum(Qty)-sum(txi_Qty) as Pend_TI,b.unit,b.cpartno as Part_no,trim(a.ICODE) as Erp_Code,trim(a.acode) as acode,trim(a.acode)||trim(a.icode) as fstr  from  (Select to_Char(edt_Dt,'yyyy-mm-dd')||'-'||trim(nvl(mcomment,'-')) as mcomment,acode,ICODE,nvl(num01,0) as Qty,0 as fgstk,0 as DA_qty,0 as CI_Qty,0 as txi_qty,0 as prd_Qty,0 as Order_bal from sl_plan where branchcd='" + mbr + "' and type='SL' and num01>0 and wk_Ref>0 union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,qtysupp as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,0 as Order_bal from despatch where branchcd='" + mbr + "' and type like '4F' and to_Char(packdate,'yyyy')||to_Char(packdate,'ww') in (" + value1 + ")  union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,iqtyout as cust_inv,0 as tax_inv,0 as prd_Qty,0 as Order_bal from ivoucherp where branchcd='" + mbr + "' and type like '4F' and to_Char(vchdate,'yyyy')||to_Char(vchdate,'ww') in (" + value1 + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,iqtyout as tax_inv,0 as prd_Qty,0 as Order_bal from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_Char(vchdate,'yyyy')||to_Char(vchdate,'ww') in (" + mbr + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,iqtyin as prd_Qty,0 as Order_bal from ivoucher where branchcd='" + mbr + "' and type like '17%' and to_Char(vchdate,'yyyy')||to_Char(vchdate,'ww') in (" + value1 + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,qtyord as Order_bal from somas where weight='" + mbr + "' and type like '4%' union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,-1*iqtyout as Order_bal from ivoucher where branchcd='" + mbr + "' and type like '4%' ) a,item b,famst c where trim(a.icode)=trim(B.icode) and trim(a.acode)=trim(c.acode) group by trim(b.iname),trim(a.acode),trim(c.aname),b.cpartno,b.unit,trim(a.ICODE) ) ) order by crit_len desc,acode asc";
                        SQuery = "select * from (select Product_Name,party,acode,fstr,substr(nvl(Last_rmk,'-'),12,20) as Plan_Rmk,Plan_Qty,Pend_Ord,Unit,Part_no,Erp_Code,length(substr(trim(nvl(Last_rmk,'-')),1,50)) as crit_len,SORDNO,SORDDT  from (select trim(b.iname) as Product_Name,trim(c.aname) as party,max(nvl(mcomment,'-')) as Last_rmk,sum(Qty) as Plan_Qty,sum(Order_bal) as Pend_Ord,b.unit,b.cpartno as Part_no,trim(a.ICODE) as Erp_Code,trim(a.acode) as acode,trim(a.acode)||trim(a.icode) as fstr,TRIM(SORDNO) AS SORDNO,SORDDT  from  (Select to_Char(edt_Dt,'yyyy-mm-dd')||'-'||trim(nvl(mcomment,'-')) as mcomment,acode,ICODE,nvl(num01,0) as Qty,0 as fgstk,0 as DA_qty,0 as CI_Qty,0 as txi_qty,0 as prd_Qty,0 as Order_bal,SORDNO,TO_CHAR(SORDDT,'DD/MM/YYYY') AS SORDDT from sl_plan where branchcd='" + mbr + "' and type='SL' and num01>0 and wk_Ref>0 and wk_ref in (" + value1 + ") union all  Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,qtyord as Order_bal,ORDNO,TO_CHAR(ORDDT,'DD/MM/YYYY') AS ORDDT from somas where weight='" + mbr + "' and type like '4%' union all Select '-' as mcomment,acode,ICODE,0 as Qty,0 as fgstk,0 as da,0 as cust_inv,0 as tax_inv,0 as prd_Qty,-1*iqtyout as Order_bal,ponum,to_char(podate,'dd/mm/yyyy') as podate from ivoucher where branchcd='" + mbr + "' and type like '4%')a,item b,famst c where trim(a.icode)=trim(B.icode) and trim(a.acode)=trim(c.acode) group by trim(b.iname),trim(a.acode),trim(c.aname),b.cpartno,b.unit,trim(a.ICODE),TRIM(SORDNO),SORDDT)) /*where erp_code='90010216'*/ order by crit_len desc,acode asc";
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery); //main dt shayad somas
                        ///////////
                        mq0 = "select trim(acode)||trim(icode) as fstr,trim(acode) as acode,trim(icode) as icode,pordno as po,to_char(porddt,'dd/mm/yyyy') as po_due_date,sum(qtyord) as po_qty,trim(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt from somas where weight='" + mbr + "' and type like '4F' group by trim(acode),trim(icode),pordno,to_char(porddt,'dd/mm/yyyy'),trim(ordno),to_char(orddt,'dd/mm/yyyy') order by po";
                        dt1 = fgen.getdata(frm_qstr, co_cd, mq0); //po qty and pono from this 
                        //////////
                        mq1 = "select type1,name from type where id='B'";
                        dt3 = fgen.getdata(frm_qstr, co_cd, mq1); //BRANCH NAME COMES FROM 
                        //
                        mq2 = "Select  ICODE,CLOSING from MP_FG_STK_04 ";
                        dt4 = fgen.getdata(frm_qstr, co_cd, mq2);

                        //mq3 = "select a.icode,sum(a.sent_Qty)-sum(a.rcvd_Qty) as Bal_Qty,sum(a.iamount) as Amt,max(a.linkno) as linkno,(case when named like '%AIR%' then 1 else 2 end) as tpt from (Select (a.iqtyout) as sent_Qty,trim(a.icode) as icode, 0 as rcvd_Qty,round(a.iqtyout*a.iqty_chlwt,2) as iamount,a.branchcd||a.icode||to_char(a.vchdate,'dd/mm/yyyy') as linkno,TRIM(b.MODE_TPT) as named from ivoucher a ,SALE b where a.branchcd||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=B.branchcd||B.VCHNUM||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') and a.branchcd!='DD' and a.type='4F' and a.acode like '02%' and a.vchdate>to_DatE('01/12/2017','dd/mm/yyyy')  union all Select 0 as SENTQTY,trim(icode) as icode, (iqtyin) as rcvd,0 as iamount,null as linkno,null as named from finsagi.ivoucher where branchcd='10' and type like '0U%' and vchdate>to_DatE('01/12/2017','dd/mm/yyyy'))a group by a.icode,(case when named like '%AIR%' then 1 else 2 end) HAVING sum(a.sent_Qty)-sum(a.rcvd_Qty)>0 order by a.icode";
                        mq3 = "select trim(a.icode) as icode,trim(a.acode) as acode,a.tpt,sum(a.sent_Qty)-sum(a.rcvd_Qty) as Bal_Qty,sum(a.iamount) as Amt,max(trim(a.linkno)) as linkno from (select icode,acode,sent_Qty,rcvd_Qty,iamount,linkno as linkno,(case when upper(named) like '%AIR%' then 1 else 2 end) as tpt from (Select (a.iqtyout) as sent_Qty,trim(a.icode) as icode, 0 as rcvd_Qty,round(a.iqtyout*a.iqty_chlwt,2) as iamount,a.branchcd||trim(a.icode)||trim(a.acode)||to_char(a.vchdate,'dd/mm/yyyy') as linkno,TRIM(b.MODE_TPT) as named,trim(a.acode) as acode from ivoucher a ,SALE b where a.branchcd||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=B.branchcd||B.VCHNUM||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') and a.branchcd!='DD' and a.type='4F' and a.acode like '02%' and a.vchdate>to_DatE('01/12/2017','dd/mm/yyyy') union all Select 0 as SENTQTY,trim(icode) as icode, (iqtyin) as rcvd,0 as iamount,null as linkno,null as named,trim(acode) as acode from finsagi.ivoucher where branchcd='10' and type like '0U%' and vchdate>to_DatE('01/12/2017','dd/mm/yyyy')))a group by trim(a.icode),trim(a.acode),a.tpt HAVING sum(a.sent_Qty)-sum(a.rcvd_Qty)>0 order by icode";
                        dt5 = fgen.getdata(frm_qstr, co_cd, mq3);//qty in air water, add acode
                        ////=================
                        if (dt.Rows.Count > 0)
                        {
                            DataView view1im = new DataView(dt);
                            DataTable dtdrsim = new DataTable();
                            dtdrsim = view1im.ToTable(true, "fstr"); //MAIN dt view              
                            foreach (DataRow dr0 in dtdrsim.Rows)
                            {
                                DataView viewim = new DataView(dt, "fstr='" + dr0["fstr"] + "'", "", DataViewRowState.CurrentRows);
                                dt2 = viewim.ToTable();
                                dticode = new DataTable();
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; ded1 = ""; ded2 = "";
                                if (dt1.Rows.Count > 0)
                                {
                                    DataView viewim1 = new DataView(dt1, "fstr='" + dr0["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dticode = viewim1.ToTable();
                                }
                                #region
                                for (int i = 0; i < dticode.Rows.Count; i++)
                                {
                                    dr2 = dtm.NewRow();
                                    if (i == 0)
                                    {
                                        dr2["Mfg_Plant"] = fgen.seek_iname_dt(dt3, "type1='" + mbr + "'", "name");
                                        dr2["Finished_Stock_Qty"] = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "closing"));
                                        //   dr2["Pending_Qty"] = fgen.make_double(fgen.seek_iname_dt(dt, "fstr='" + dticode.Rows[i]["fstr"].ToString().Trim() + "'", "pend_ord"));//fgen.make_double(dr0["pend_ord"].ToString().Trim());

                                        dr2["Qty_On_Water"] = fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "' and acode='" + dticode.Rows[i]["acode"].ToString().Trim() + "' and tpt=2 ", "bal_qty"));
                                        dr2["Qty_in_Air"] = fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "' and acode='" + dticode.Rows[i]["acode"].ToString().Trim() + "' and tpt=1", "bal_qty"));
                                        //  db1 = fgen.make_double(dr2["Pending_Qty"].ToString().Trim());
                                        db5 = fgen.make_double(dr2["Qty_On_Water"].ToString().Trim());
                                        db6 = fgen.make_double(dr2["Qty_in_Air"].ToString().Trim());
                                    }
                                    dr2["Customer"] = fgen.seek_iname_dt(dt, "fstr='" + dticode.Rows[i]["fstr"].ToString().Trim() + "'", "party");
                                    dr2["item"] = fgen.seek_iname_dt(dt, "fstr='" + dticode.Rows[i]["fstr"].ToString().Trim() + "'", "Product_Name"); //dr0["Product_Name"].ToString().Trim();
                                    dr2["PO"] = dticode.Rows[i]["po"].ToString().Trim();
                                    dr2["PO_Due_Date"] = dticode.Rows[i]["po_due_date"].ToString().Trim();

                                    dr2["Late_Days"] = fgen.make_double(fgen.seek_iname(frm_qstr, co_cd, "select to_date('" + hf1.Value + "','dd/mm/yyyy')-to_date('" + dr2["PO_Due_Date"].ToString().Trim() + "','dd/mm/yyyy') as days from dual", "days"));


                                    dr2["PO_Qty"] = dticode.Rows[i]["po_qty"].ToString().Trim();
                                    dr2["Planned_Qty"] = fgen.make_double(fgen.seek_iname_dt(dt, "fstr='" + dticode.Rows[i]["fstr"].ToString().Trim() + "' AND SORDNO='" + dticode.Rows[i]["ordno"].ToString().Trim() + "' and SORDdt='" + dticode.Rows[i]["orddt"].ToString().Trim() + "'", "plan_qty"));//fgen.make_double(dr0["PLAN_QTY"].ToString().Trim());

                                    dr2["Pending_Qty"] = fgen.make_double(fgen.seek_iname_dt(dt, "fstr='" + dticode.Rows[i]["fstr"].ToString().Trim() + "' AND SORDNO='" + dticode.Rows[i]["ordno"].ToString().Trim() + "' and SORDdt='" + dticode.Rows[i]["orddt"].ToString().Trim() + "'", "Pend_Ord"));//fgen.make_double(dr0["pend_ord"].ToString().Trim());

                                    db = fgen.make_double(dr2["Late_Days"].ToString().Trim());
                                    db1 = fgen.make_double(dr2["Pending_Qty"].ToString().Trim());
                                    db2 = fgen.make_double(dr2["Planned_Qty"].ToString().Trim());
                                    if (db < 0)
                                    {
                                        db3 = db1 + db3;
                                    }
                                    db4 = db4 + db2;
                                    ded1 = dr2["Customer"].ToString().Trim();
                                    ded2 = dr2["item"].ToString().Trim();

                                    db8 += fgen.make_double(dr2["PO_Qty"].ToString().Trim());
                                    db9 += fgen.make_double(dr2["Pending_Qty"].ToString().Trim());
                                    db10 += fgen.make_double(dr2["Planned_Qty"].ToString().Trim());
                                    dtm.Rows.Add(dr2);
                                }
                                if (dticode.Rows.Count > 0)
                                {
                                    dr2 = dtm.NewRow();
                                    if (db3 > 0)
                                    {
                                        db7 = db3 - db5 - db6;
                                    }
                                    else
                                    {
                                        db7 = 0;
                                    }
                                    dr2["Customer"] = ded1;
                                    dr2["item"] = ded2;
                                    dr2["Mfg_Plant"] = "TOTAL";
                                    dr2["Past_Due_Qty"] = db7;
                                    dr2["Total_in_6_Week_Mrp"] = db4;
                                    dr2["PO_Qty"] = db8;
                                    dr2["Planned_Qty"] = db9;
                                    dr2["Pending_Qty"] = db10;
                                    dtm.Rows.Add(dr2);
                                }
                                #endregion
                            }
                        }
                        if (dtm.Rows.Count > 0)
                        {
                            Session["send_dt"] = dtm;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevelJS("Order Tracking Report For Week " + value1 + "", frm_qstr);
                        }
                        break;
                }
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
        else
        {
            switch (val)
            {
                case "F05125":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                    fgen.Fn_ValueBox("-", frm_qstr);
                    break;

            }
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        val = frm_formID;
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
            xprdrange1 = xprd1;
            yr_fld = year;

            co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");

            string mhd = "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", "INR");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,99,99,999.99");

            mhd = fgen.seek_iname(frm_qstr, co_cd, "select opt_param from fin_rsys_opt_pw where branchcd='" + mbr + "' and opt_id='W2015'", "opt_param");
            if (mhd != "0" && mhd != "-") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", mhd);

            mhd = fgen.seek_iname(frm_qstr, co_cd, "select opt_param from fin_rsys_opt_pw where branchcd='" + mbr + "' and opt_id='W2016'", "opt_param");
            if (mhd != "0" && mhd != "-" && mhd != "I") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,999,999,999.99");

            string coma_sepr;
            coma_sepr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BR_COMMA");

            //if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
            //else 
            branch_Cd = "branchcd='" + mbr + "'";

            // after prdDmp this will run   

            switch (val)
            {
                case "F05101":
                    // Drill Down Day Wise Sales
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymmdd') as fstr,'-' as gstr,to_char(vchdate,'dd/mm/yyyy') as Sales_Date,to_char(sum(amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(bill_tot),'" + coma_sepr + "') as Gross_Amount from sale where branchcd='" + mbr + "' and vchdate " + xprdrange + " group by to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by to_char(vchdate,'yyyymmdd') desc", frm_qstr, "4#5#", "3#4#5#", "200#500#500");
                    fgen.drillQuery(1, "select trim(a.Acode)||to_char(a.vchdate,'yyyymmdd') as fstr,to_char(a.vchdate,'yyyymmdd') as gstr,b.Aname as Customer,to_char(sum(a.amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(a.bill_tot),'" + coma_sepr + "') as Gross_Amount,b.Staten,trim(a.acode) as Account_Code from sale a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),b.aname,b.staten order by b.aname", frm_qstr, "4#5#", "", "");
                    fgen.drillQuery(2, "select trim(a.Icode)||trim(a.Acode)||to_char(a.vchdate,'yyyymmdd') as fstr,trim(a.Acode)||to_char(a.vchdate,'yyyymmdd') as gstr,a.Purpose as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Quantity_Sold,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,a.exc_57f4 as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),a.purpose,a.exc_57f4,trim(a.icode) order by a.purpose", frm_qstr, "4#5#", "", "");
                    fgen.drillQuery(3, "select trim(a.Vchnum) as fstr,trim(a.Icode)||trim(a.Acode)||to_char(a.vchdate,'yyyymmdd') as gstr,a.Vchnum as Inv_no,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,a.Purpose as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Quantity_Sold,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,a.exc_57f4 as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),to_char(a.vchdate,'dd/mm/yyyy'),a.purpose,a.exc_57f4,a.vchnum,trim(a.icode) order by a.purpose", frm_qstr, "6#7#", "3#4#5#6#7#", "100#100#500#100#100#");
                    fgen.Fn_DrillReport("Date Wise Sales Drill Down", frm_qstr);
                    break;
                case "F05102":
                    // Drill Down Month Wise Sales
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,to_char(vchdate,'MON-YYYY') as Sales_Month,to_char(sum(amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(bill_tot),'" + coma_sepr + "') as Gross_Amount from sale where branchcd='" + mbr + "' and vchdate " + xprdrange + " group by to_char(vchdate,'MON-YYYY'),to_char(vchdate,'yyyymm') order by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.Acode) as fstr,to_char(a.vchdate,'yyyymm') as gstr,b.Aname as Customer,to_char(sum(a.amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(a.bill_tot),'" + coma_sepr + "') as Gross_Amount,b.Staten,trim(a.acode) as Account_Code from sale a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymm'),trim(a.Acode),b.aname,b.staten order by b.aname", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.Icode) as fstr,trim(a.Acode) as gstr,a.Purpose as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Quantity_Sold,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,a.exc_57f4 as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymm'),trim(a.Acode),a.purpose,a.exc_57f4,trim(a.icode) order by a.purpose", frm_qstr);
                    fgen.drillQuery(3, "select trim(a.Vchnum) as fstr,trim(a.icode) as gstr,a.Vchnum as Inv_no,to_char(a.Vchdate,'dd/mm/yyyy') as Inv_Date,a.Purpose as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Quantity_Sold,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,a.exc_57f4 as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),a.purpose,a.exc_57f4,a.vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),trim(a.icode) order by a.purpose", frm_qstr);
                    fgen.Fn_DrillReport("Month Wise Sales Drill Down", frm_qstr);
                    break;
                case "F05111":
                case "F05103":
                    // Drill Down Plant Wise Sales
                    fgen.drillQuery(0, "select a.branchcd as fstr,'-' as gstr,b.Name as Plant_Name,to_char(sum(a.amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(a.bill_tot),'" + coma_sepr + "') as Gross_Amount from sale a,type b where b.id='B' and a.branchcd=b.type1 and a.branchcd!='DD' and a.vchdate " + xprdrange + " group by a.branchcd,b.name order by a.branchcd", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.Acode)||trim(a.branchcd) as fstr,a.branchcd as gstr,b.Aname as Customer,to_char(sum(a.amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(a.bill_tot),'" + coma_sepr + "') as Gross_Amount,b.Staten,trim(a.acode) as Account_Code from sale a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='DD' and a.vchdate " + xprdrange + " group by trim(a.Acode)||trim(a.branchcd),a.branchcd,trim(a.Acode),b.aname,b.staten order by b.aname", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.Acode)||trim(a.icode)||trim(a.branchcd) as fstr,trim(a.Acode)||trim(a.branchcd) as gstr,a.Purpose as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Quantity_Sold,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,a.exc_57f4 as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='DD' and a.type like '4%' and a.vchdate " + xprdrange + " group by trim(a.Acode)||trim(a.icode)||trim(a.branchcd),trim(a.Acode)||trim(a.branchcd),trim(a.Acode),a.purpose,a.exc_57f4,trim(a.icode) order by a.purpose", frm_qstr);
                    fgen.drillQuery(3, "select trim(a.Vchnum) as fstr,trim(a.icode) as gstr,a.Vchnum as Inv_no,to_char(a.Vchdate,'dd/mm/yyyy') as Inv_Date,a.Purpose as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Quantity_Sold,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,a.exc_57f4 as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='DD' and a.type like '4%' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),a.purpose,a.exc_57f4,a.vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),trim(a.icode) order by a.purpose", frm_qstr);
                    fgen.Fn_DrillReport("Plant Wise Sales Drill Down", frm_qstr);
                    break;
                case "F05106":
                    // Schedule Vs Dispatch
                    fgen.drillQuery(0, "select trim(a.acode) as fstr,'-' as gstr,b.Aname as Customer_Name,b.staten as State_name,sum(a.budgetcost) as Schedule_Value,sum(a.sales) as Despatch_Value,sum(a.budgetcost)-sum(a.sales) as Shortfall_Value,trim(a.acode) as Erp_Code from (Select acode,round(Total*irate,2) as budgetcost,0 as sales from schedule where branchcd='" + mbr + "' and type='46' and vchdate " + xprdrange + " union all Select acode,0 as budgetcost,amt_Sale as sales from sale where branchcd='" + mbr + "' and type like '4%' and type not in ('45','47') and vchdate " + xprdrange + ")a,famst b where trim(A.acode)=trim(B.acode) group by b.aname,b.staten,trim(a.acode) order by B.aname", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.acode)||trim(a.icode) as fstr,trim(a.acode) as gstr,b.Aname as Customer_Name,c.Iname as Item_name,sum(a.budgetcost) as Schedule_Value,sum(a.sales) as Despatch_Value,sum(a.budgetcost)-sum(a.sales) as Shortfall_Value,trim(a.acode) as Erp_Acode,trim(a.Icode) as Erp_Icode from (Select acode,icode,round(Total*irate,2) as budgetcost,0 as sales from schedule where branchcd='" + mbr + "' and type='46' and vchdate " + xprdrange + " union all Select acode,icode,0 as budgetcost,iamount as sales from ivoucher where branchcd='" + mbr + "' and type like '4%' and type not in ('45','47') and vchdate " + xprdrange + ")a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.Icode)=trim(c.Icode) group by b.aname,c.iname,trim(a.acode),trim(a.Icode) order by B.aname,c.Iname", frm_qstr);

                    fgen.Fn_DrillReport("Schedule Vs Dispatch Drill Down", frm_qstr);
                    break;

                case "F61143":
                    // Customer Compl D/Down
                    fgen.drillQuery(0, "select trim(a.acode) as fstr,'-' as gstr,b.Aname as Customer_Name,b.staten as State_name,sum(a.budgetcost) as Complaints,trim(a.acode) as Erp_Code from (Select acode,1 as budgetcost from wb_Ccm_log where branchcd='" + mbr + "' and type='CC' and ccmdt " + xprdrange + ")a,famst b where trim(A.acode)=trim(B.acode) group by b.aname,b.staten,trim(a.acode) order by B.aname", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.acode)||trim(a.icode) as fstr,trim(a.acode) as gstr,b.Aname as Customer_Name,c.Iname as Item_name,a.comp_type,a.ccmno as comp_no,to_char(a.ccmdt,'dd/mm/yyyy') as comp_dt,A.lremarks,A.oremarks,A.inv_Ref,A.curr_Stat,trim(a.acode) as Erp_Acode,trim(a.Icode) as Erp_Icode from (Select acode,icode,1 as budgetcost,comp_type,ccmno,ccmdt,lremarks,oremarks,inv_Ref,curr_Stat from wb_ccm_log where branchcd='" + mbr + "' and type='CC' and ccmdt " + xprdrange + ")a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.Icode)=trim(c.Icode) order by B.aname,c.Iname,a.ccmdt", frm_qstr);

                    fgen.Fn_DrillReport("Customer Complaint Instances Drill Down Report", frm_qstr);
                    break;

                case "F61145":
                    // reason Compl D/Down
                    fgen.drillQuery(0, "select trim(a.acode) as fstr,'-' as gstr,trim(A.ACODE) as Complaint_type,sum(a.budgetcost) as Complaints,a.branchcd from (Select COMP_TYPE AS acode,1 as budgetcost,branchcd from wb_Ccm_log where branchcd='" + mbr + "' and type='CC' and ccmdt " + xprdrange + ")a group by trim(a.acode),a.branchcd order by trim(A.ACODE)", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.acode)||trim(a.icode) as fstr,trim(a.comp_type) as gstr,b.Aname as Customer_Name,c.Iname as Item_name,a.comp_type,a.ccmno as comp_no,to_char(a.ccmdt,'dd/mm/yyyy') as comp_dt,A.lremarks,A.oremarks,A.inv_Ref,A.curr_Stat,trim(a.acode) as Erp_Acode,trim(a.Icode) as Erp_Icode from (Select acode,icode,1 as budgetcost,comp_type,ccmno,ccmdt,lremarks,oremarks,inv_Ref,curr_Stat from wb_ccm_log where branchcd='" + mbr + "' and type='CC' and ccmdt " + xprdrange + ")a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.Icode)=trim(c.Icode) order by B.aname,c.Iname,a.ccmdt", frm_qstr);

                    fgen.Fn_DrillReport("Customer Complaint Instances Drill Down Report", frm_qstr);
                    break;

                case "F61147":
                    // product Compl D/Down
                    fgen.drillQuery(0, "select trim(a.Icode) as fstr,'-' as gstr,b.Iname as Product_Name,b.cpartno as Product_Num,sum(a.budgetcost) as Complaints,trim(a.icode) as Erp_Code from (Select Icode,1 as budgetcost from wb_Ccm_log where branchcd='" + mbr + "' and type='CC' and ccmdt " + xprdrange + ")a,item b where trim(A.icode)=trim(B.icode) group by b.iname,b.cpartno,trim(a.icode) order by B.Iname", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.acode)||trim(a.icode) as fstr,trim(a.Icode) as gstr,b.Aname as Customer_Name,c.Iname as Item_name,a.comp_type,a.ccmno as comp_no,to_char(a.ccmdt,'dd/mm/yyyy') as comp_dt,A.lremarks,A.oremarks,A.inv_Ref,A.curr_Stat,trim(a.acode) as Erp_Acode,trim(a.Icode) as Erp_Icode from (Select acode,icode,1 as budgetcost,comp_type,ccmno,ccmdt,lremarks,oremarks,inv_Ref,curr_Stat from wb_ccm_log where branchcd='" + mbr + "' and type='CC' and ccmdt " + xprdrange + ")a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.Icode)=trim(c.Icode) order by B.aname,c.Iname,a.ccmdt", frm_qstr);

                    fgen.Fn_DrillReport("Customer Complaint Instances Drill Down Report", frm_qstr);
                    break;


                case "F05162":
                    // Drill Down Inwards
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymmdd') as fstr,'-' as gstr,to_char(vchdate,'dd/mm/yyyy') as Purchase_Date,to_char(sum(amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(bill_tot),'" + coma_sepr + "') as Gross_Amount from ivchctrl where branchcd='" + mbr + "' and type like '0%' and type not in ('04','08') and vchdate " + xprdrange + " group by to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by to_char(vchdate,'yyyymmdd')", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.Acode) as fstr,to_char(a.vchdate,'yyyymmdd') as gstr,b.Aname as Supplier,to_char(sum(a.amt_sale),'" + coma_sepr + "') as Basic_Amount,to_char(sum(a.bill_tot),'" + coma_sepr + "') as Gross_Amount,b.Staten,trim(a.acode) as Account_Code from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and type like '0%' and a.type not in ('04','08') and  a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),b.aname,b.staten order by b.aname", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.acode)||trim(a.Icode) as fstr,trim(a.Acode) as gstr,c.Iname as Item_Name,to_char(sum(a.iqtyin),'" + coma_sepr + "') as Quantity_Rcvd,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.type not in ('04','08') and  a.vchdate " + xprdrange + " and a.store in ('Y','N') group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.drillQuery(3, "select trim(a.vchnum) as fstr,trim(a.acode)||trim(a.Icode) as gstr,a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,c.Iname as Item_Name,to_char(sum(a.iqtyin),'" + coma_sepr + "') as Quantity_Rcvd,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Basic_Amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.type not in ('04','08') and  a.vchdate " + xprdrange + " and a.store in ('Y','N') group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

                case "F05165":
                    // Drill Down Inwards
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymmdd') as fstr,'-' as gstr,to_char(vchdate,'dd/mm/yyyy') as Challan_Date,to_char(sum(iqtyout),'" + coma_sepr + "') as Chl_Quantity,to_char(sum(iamount),'" + coma_sepr + "') as Chl_Amount from ivoucher where branchcd='" + mbr + "' and type like '2%' and vchdate " + xprdrange + " group by to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by to_char(vchdate,'yyyymmdd')", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.Acode) as fstr,to_char(a.vchdate,'yyyymmdd') as gstr,b.Aname as Supplier,to_char(sum(a.iqtyout),'" + coma_sepr + "') as chl_Qty,to_char(sum(a.iamount),'" + coma_sepr + "') as Chl_Amount,b.Staten,trim(a.acode) as Account_Code from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and type like '2%' and  a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),b.aname,b.staten order by b.aname", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.acode)||trim(a.Icode) as fstr,trim(a.Acode) as gstr,c.Iname as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Chl_Qty,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Chl_amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '2%' and a.vchdate " + xprdrange + " group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.drillQuery(3, "select trim(a.vchnum) as fstr,trim(a.acode)||trim(a.Icode) as gstr,a.vchnum as Chl_No,to_char(a.vchdate,'dd/mm/yyyy') as Chl_Dt,c.Iname as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Chl_Qty,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Chl_Amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '2%' and a.vchdate " + xprdrange + " group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

                case "F05166":
                    // Drill Down Issues
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymmdd') as fstr,'-' as gstr,to_char(vchdate,'dd/mm/yyyy') as Issue_Date,to_char(sum(iqtyout),'" + coma_sepr + "') as Issue_Quantity,to_char(sum(ROUND(IQTYOUT * (case when nvl(ichgs,0)>0 then nvl(ichgs,0) else nvl(IRATE,0) end), 2)),'" + coma_sepr + "') as Issue_Amount from ivoucher where branchcd='" + mbr + "' and type like '3%' and type!='36' and vchdate " + xprdrange + " and store='Y' group by to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by to_char(vchdate,'yyyymmdd')", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.Acode) as fstr,to_char(a.vchdate,'yyyymmdd') as gstr,b.Name as Department,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Issue_Qty,to_char(sum(ROUND(A.IQTYOUT * (case when nvl(a.ichgs,0)>0 then nvl(a.ichgs,0) else nvl(a.IRATE,0) end), 2)),'" + coma_sepr + "') as Issue_Amount,trim(a.acode) as Account_Code from ivoucher a,type b where b.id='M' and trim(a.acode)=trim(b.type1) and a.branchcd='" + mbr + "' and type like '3%' and a.type!='36' and  a.vchdate " + xprdrange + "  and store='Y' group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),b.Name order by b.name", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.acode)||trim(a.Icode) as fstr,trim(a.Acode) as gstr,c.Iname as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Issue_Qty,c.Unit,to_char(sum(ROUND(A.IQTYOUT * (case when nvl(a.IRATE,0)>0 then nvl(a.IRATE,0) WHEN NVL(C.IQD,0)>0 THEN NVL(C.IQD,0) else nvl(C.IRATE,0) end), 2)),'" + coma_sepr + "') as Issue_amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,type b,item c where b.id='M' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '3%' and a.type!='36' and  a.vchdate " + xprdrange + "  and a.store='Y' group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.drillQuery(3, "select trim(a.vchnum) as fstr,trim(a.acode)||trim(a.Icode) as gstr,a.vchnum as Issue_No,to_char(a.vchdate,'dd/mm/yyyy') as Issue_Dt,c.Iname as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Issue_Qty,c.Unit,to_char(sum(ROUND(A.IQTYOUT * (case when nvl(a.IRATE,0)>0 then nvl(a.IRATE,0) WHEN NVL(C.IQD,0)>0 THEN NVL(C.IQD,0) else nvl(C.IRATE,0) end), 2)),'" + coma_sepr + "') as Issue_Amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,type b,item c where b.id='M' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '3%' and a.type!='36' and  a.vchdate " + xprdrange + "  and a.store='Y' group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;
                case "F05167":
                    // Drill Down Return
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymmdd') as fstr,'-' as gstr,to_char(vchdate,'dd/mm/yyyy') as Return_Date,to_char(sum(iqtyin),'" + coma_sepr + "') as Return_Quantity,to_char(sum(iamount),'" + coma_sepr + "') as Return_Amount from ivoucher where branchcd='" + mbr + "' and type like '1%' and type<='14' and vchdate " + xprdrange + " and store='Y' group by to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by to_char(vchdate,'yyyymmdd')", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.Acode) as fstr,to_char(a.vchdate,'yyyymmdd') as gstr,b.Name as Department,to_char(sum(a.iqtyin),'" + coma_sepr + "') as Return_Qty,to_char(sum(a.iamount),'" + coma_sepr + "') as Return_Amount,trim(a.acode) as Account_Code from ivoucher a,type b where b.id='M' and trim(a.acode)=trim(b.type1) and a.branchcd='" + mbr + "' and type like '1%' and a.type<='14'  and  a.vchdate " + xprdrange + "  and store='Y' group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),b.Name order by b.name", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.acode)||trim(a.Icode) as fstr,trim(a.Acode) as gstr,c.Iname as Item_Name,to_char(sum(a.iqtyin),'" + coma_sepr + "') as Return_Qty,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Return_amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,type b,item c where b.id='M' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '1%' and a.type<='14'  and  a.vchdate " + xprdrange + "  and a.store='Y' group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.drillQuery(3, "select trim(a.vchnum) as fstr,trim(a.acode)||trim(a.Icode) as gstr,a.vchnum as Return_No,to_char(a.vchdate,'dd/mm/yyyy') as Return_Dt,c.Iname as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Return_Qty,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Return_Amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,type b,item c where b.id='M' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '1%' and a.type<='14'  and  a.vchdate " + xprdrange + "  and a.store='Y' group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

                case "F05172":
                    // Drill Down Return
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymmdd') as fstr,'-' as gstr,to_char(vchdate,'dd/mm/yyyy') as Return_Date,to_char(sum(iqtyin),'" + coma_sepr + "') as Return_Quantity,to_char(sum(iamount),'" + coma_sepr + "') as Return_Amount from ivoucher where branchcd='" + mbr + "' and type like '1%' and type<='14' and vchdate " + xprdrange + " and store='Y' group by to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by to_char(vchdate,'yyyymmdd')", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.Acode) as fstr,to_char(a.vchdate,'yyyymmdd') as gstr,b.Name as Department,to_char(sum(a.iqtyin),'" + coma_sepr + "') as Return_Qty,to_char(sum(a.iamount),'" + coma_sepr + "') as Return_Amount,trim(a.acode) as Account_Code from ivoucher a,type b where b.id='M' and trim(a.acode)=trim(b.type1) and a.branchcd='" + mbr + "' and type like '1%' and a.type<='14'  and  a.vchdate " + xprdrange + "  and store='Y' group by to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),b.Name order by b.name", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.acode)||trim(a.Icode) as fstr,trim(a.Acode) as gstr,c.Iname as Item_Name,to_char(sum(a.iqtyin),'" + coma_sepr + "') as Return_Qty,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Return_amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,type b,item c where b.id='M' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '1%' and a.type<='14'  and  a.vchdate " + xprdrange + "  and a.store='Y' group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.drillQuery(3, "select trim(a.vchnum) as fstr,trim(a.acode)||trim(a.Icode) as gstr,a.vchnum as Return_No,to_char(a.vchdate,'dd/mm/yyyy') as Return_Dt,c.Iname as Item_Name,to_char(sum(a.iqtyout),'" + coma_sepr + "') as Return_Qty,c.Unit,to_char(sum(a.iamount),'" + coma_sepr + "') as Return_Amount,c.Cpartno as Part_no,trim(a.icode) as Item_Code from ivoucher a,type b,item c where b.id='M' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '1%' and a.type<='14'  and  a.vchdate " + xprdrange + "  and a.store='Y' group by trim(a.acode)||trim(a.Icode),to_char(a.vchdate,'yyyymmdd'),trim(a.Acode),c.iname,c.cpartno,c.unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode) order by c.Iname", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

                case "F05101xx":
                    SQuery = "select b.Aname as Customer_Name,b.staten as State_name,sum(a.budgetcost) as Target_Sale,sum(a.sales) as Ach_Sales,sum(a.budgetcost)-sum(a.sales) as Difference,trim(a.acode) as Erp_Code from (Select acode,budgetcost,0 as sales from budgmst where branchcd='" + mbr + "' and type='TC' and vchdate " + xprdrange + " union all Select acode,0 as budgetcost,amt_Sale as sales from sale where branchcd='" + mbr + "' and type like '4%' and type not in ('45','47') and vchdate " + xprdrange + ")a,famst b where trim(A.acode)=trim(B.acode) group by b.aname,b.staten,trim(a.acode) order by B.aname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Target Vs Achieved Sales " + fromdt + " to " + todt, frm_qstr);
                    break;


                case "F05111XXX":
                    SQuery = "select b.Name as Plant_Name,b.statenm as State_name,sum(a.Bsales) as Basic_Sales,sum(a.Gsales) as Gross_Sales,sum(a.Taxes) as Taxes,trim(a.branchcd) as Plant_Code from (Select branchcd,amt_Sale as Bsales,bill_tot as Gsales,amt_Exc+nvl(rvalue,0) as Taxes from sale where branchcd!='DD' and type like '4%' and type not in ('45','47') and vchdate " + xprdrange + " and substr(Acode,1,2)!='02')a,type b where trim(A.branchcd)=trim(B.type1) and b.id='B' group by b.Name,b.statenm,trim(a.branchcd) order by B.name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Plant Wise Sales " + fromdt + " to " + todt, frm_qstr);
                    break;


                case "S06005E":
                    // open graph
                    SQuery = "select month_name,count(*) as  tot_bas,count(*) as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Query Graph", "line", "Month Wise", "-", SQuery, "");
                    break;

                case "S15115IX":
                    // open drill down form
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                    fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                    fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                    fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;
                case "F05124":
                case "29155":
                    #region checking Cyclical BOM

                    SQuery = "select branchcd||'-'||trim(icode)||'-'||trim(ibcode) as bom_link,branchcd,type,vchnum,vchdate,ent_by,ent_dt,edt_by,edt_dt from itemosp where branchcd!='DD' and branchcd||'-'||trim(icode)||'-'||trim(ibcode) in (Select branchcd||'-'||trim(ibcode)||'-'||trim(icode) from itemosp where branchcd!='DD')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevelJS("Cyclical Bom With (parent -> child -> parent", frm_qstr);

                        return;
                    }

                    SQuery = "select branchcd||'-'||trim(ibcode)||'-'||trim(icode) as bom_link,branchcd,type,vchnum,vchdate,ent_by,ent_dt,edt_by,edt_dt from itemosp where branchcd!='DD' and branchcd||'-'||trim(ibcode)||'-'||trim(icode) in (Select branchcd||'-'||trim(icode)||'-'||trim(ibcode) from itemosp where branchcd!='DD')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("Cyclical Bom With (parent -> child -> parent", frm_qstr);

                        return;
                    }

                    SQuery = "select B.INAME ,B.cdrgno,A.vchnum,A.vchdate,a.icode,count(vchnum) as lines from itemosp A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and trim(A.icode)=trim(a.ibcode) AND A.type='BM' and A.branchcd='" + mbr + "' and A.vchnum<>'000000' group by B.INAME ,B.cdrgno,A.vchnum,A.vchdate,a.icode order by A.vchdate desc ,A.vchnum desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("Cyclical Bom With (parent -> child -> parent", frm_qstr);

                        return;
                    }
                    #endregion
                    cond = "";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(a.icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";

                    cond = cond.Length > 2 ? cond : " AND TRIM(A.ICODE) LIKE '9%' ";

                    branch_Cd = (co_cd == "KHEM" || co_cd == "KCLG" || co_cd == "BUPL" || co_cd == "XDIL" || co_cd == "DREM" || co_cd == "LOGW" || co_cd == "ROOP" || co_cd == "WING" || co_cd == "SDM" || co_cd == "DLJM" || co_cd == "AEPL") ? "BRANCHCD NOT IN ('DD','88')" : "BRANCHCD='" + mbr + "'";
                    SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as itrate,b.iname as itemname,B.UNIT AS BUNIT,b.cpartno,c.iname as piname,c.cpartno as pcpartno from itemosp a,item b,item c where trim(a.ibcode)=trim(b.icode) and trim(a.icode)=trim(c.icodE) " + cond + " AND a." + branch_Cd + " order by a.srno,a.icode";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE") == "06")
                    {
                        SQuery = "Select a.branchcd,'1' as srno,a.icode,'-' as ibcode,'1' as main_issue_no,'1' as ibqty,(case when B.IQD>0 then B.IQD else B.irate end) as itrate,b.iname as itemname,B.UNIT AS BUNIT,b.cpartno,b.iname as piname,b.cpartno as pcpartno from INSPMST a,item b where trim(a.icode)=trim(b.icodE) " + cond + " AND a." + branch_Cd + " and a.type='70' and a.srno='1' order by a.srno,a.icode";
                        SQuery = "Select a.branchcd,'1' as srno,a.icode,a.col5 as ibcode,'1' as main_issue_no,a.numwt as ibqty,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,B.UNIT AS BUNIT,b.cpartno,c.iname as piname,c.cpartno as pcpartno,(case when B.IQD>0 then B.IQD else B.irate end) as itrate from INSPMST a,item b,item c where trim(a.col5)=trim(b.icode) and trim(a.icode)=trim(c.icodE) " + cond + " AND a.branchcd!='DD' and a.type='70' order by a.srno,a.icode";
                    }

                    dt3 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    DataTable vdt = new DataTable();
                    mdt = new DataTable();
                    mdt.Columns.Add(new DataColumn("branchcd", typeof(string)));
                    mdt.Columns.Add(new DataColumn("lvl", typeof(string)));
                    mdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("pcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("vcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibqty", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("irate", typeof(string)));
                    mdt.Columns.Add(new DataColumn("val", typeof(string)));

                    mdt.Columns.Add(new DataColumn("iname", typeof(string)));
                    mdt.Columns.Add(new DataColumn("pcpartno", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibname", typeof(string)));
                    mdt.Columns.Add(new DataColumn("cpartno", typeof(string)));
                    mdt.Columns.Add(new DataColumn("unit", typeof(string)));
                    mdt.Columns.Add(new DataColumn("jr", typeof(string)));

                    DataTable fmdt = new DataTable();
                    fmdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("val", typeof(string)));
                    if (co_cd == "NIRM" || co_cd == "NEOP" || co_cd == "DREM") fmdt.Columns.Add(new DataColumn("JO_Val", typeof(string)));
                    if (co_cd == "DREM") fmdt.Columns.Add(new DataColumn("lot_size", typeof(string)));

                    if (val == "F05124" || val == "29155a" || val == "29157")
                    {
                        fmdt.Columns.Add(new DataColumn("srate", typeof(string)));
                        fmdt.Columns.Add(new DataColumn("sqty", typeof(string)));
                        fmdt.Columns.Add(new DataColumn("acode", typeof(string)));
                    }
                    int v = 0;

                    SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,b.cpartno,b.unit AS BUNIT from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a." + branch_Cd + " order by a.srno,a.icode,a.ibcode";
                    if (co_cd == "KCLG") SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,b.unit AS BUNIT,b.cpartno from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD = ('02') order by a.srno,a.icode,a.ibcode";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE") == "06")
                    {
                        SQuery = "Select a.branchcd,'1' as srno,a.icode,a.col5 as ibcode,'1' as main_issue_no,(case when B.IQD>0 then B.IQD else B.irate end) as itrate,a.numwt as ibqty,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,B.UNIT AS BUNIT,b.cpartno,c.iname as piname,c.cpartno as pcpartno from INSPMST a,item b,item c where trim(a.col5)=trim(b.icode) and trim(a.icode)=trim(c.icodE) AND a.branchcd!='DD' and a.type='70' order by a.srno,a.icode";
                        //SQuery = "Select a.branchcd,'1' as srno,a.icode,'-' as ibcode,'1' as main_issue_no,'1' as ibqty,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,B.UNIT AS BUNIT,b.cpartno,b.iname as piname,b.cpartno as pcpartno from INSPMST a,item b where trim(a.icode)=trim(b.icodE) AND a." + branch_Cd + " and a.type='70' and a.srno='1' order by a.srno,a.icode";
                        SQuery = "";
                    }
                    vdt = fgen.getdata(frm_qstr, co_cd, SQuery); v = 0;
                    dt2 = new DataTable();
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR9") == "Y") rmcBranch = "BRANCHCD NOT IN ('DD','88')";
                    else rmcBranch = "BRANCHCD='" + mbr + "'";
                    rateCond = " type like '0%' ";
                    if (co_cd == "SAGM") rateCond = "type in ('02','07')";

                    SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where " + rmcBranch + " and " + rateCond + " and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and substr(icode,1,1)<7 /*and icode like '9%'*/ order by icode,vdd desc";
                    if (co_cd == "BUPL") SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where " + rmcBranch + " and type in ('02','05','07') and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) /* and icode like '9%'*/ order by icode,vdd desc";
                    //if (co_cd == "KCLG") SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where BRANCHCD not in ('DD','88') and type like '0%' and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  /*and icode like '9%'*/ order by icode,vdd desc";
                    if (val == "F05124")
                    {
                        //wtd avg rate
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR10") == "N")
                            SQuery = "Select trim(icode) as icode,round((sum(iqtyin*ichgs) / sum(iqtyin)) ,3) as rate,1 AS VDD from ivoucher where " + rmcBranch + " and " + rateCond + " and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and substr(icode,1,1)<7 /*and icode like '9%'*/ group by trim(icode) order by icode";
                    }
                    if (val != "29157")
                    {
                        if (co_cd != "KCLG") dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }
                    DataTable dtPo = new DataTable();
                    SQuery = "SELECT distinct TRIM(ICODe) AS ICODE,PRATE,TO_CHAR(ORDDT,'YYYYMMDD') AS VDD FROM POMAS WHERE BRANCHCD='" + mbr + "' and type='53' and orddt>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and icode like '7%' /* and orddt " + xprdrange + "*/ order by vdd desc ";
                    if (co_cd == "NIRM") dtPo = fgen.getdata(frm_qstr, co_cd, SQuery);

                    if (co_cd == "DREM")
                    {
                        SQuery = "Select c.iname,c.cpartno,b.name as names,a.* from itemospanx a,type b,item c  where b.id='1' and trim(a.icode)=trim(c.icode) and trim(a.stg_Cd)=trim(b.type1) and a.branchcd!='DD'";
                        bomanx = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }
                    DataView dist1_view = new DataView(dt3);
                    DataTable dt_dist = new DataTable();
                    if (dist1_view.Count > 0)
                    {
                        dist1_view.Sort = "icode";
                        dt_dist = dist1_view.ToTable(true, "icode");
                    }
                    DataTable mdt1;
                    DataView mvdview;
                    DataRow dro1;
                    double mainLotSize = 0, joVal = 0, db5, db6;
                    foreach (DataRow dt_dist_row in dt_dist.Rows)
                    {
                        mdt1 = new DataTable();
                        mdt1 = mdt.Clone();
                        mvdview = new DataView(dt3, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "icode,ibcode", DataViewRowState.CurrentRows);
                        dt = new DataTable();
                        mvdview.Sort = "srno,icode";
                        dt = mvdview.ToTable();
                        // filling parent
                        foreach (DataRow drc in dt.Rows)
                        {
                            double cVa = 0;
                            dro = mdt1.NewRow();
                            dro["lvl"] = "1";
                            dro["branchcd"] = drc["branchcd"].ToString().Trim();
                            dro["icode"] = drc["icode"].ToString().Trim();
                            dro["vcode"] = drc["icode"].ToString().Trim();
                            dro["iname"] = drc["piname"].ToString().Trim();
                            dro["pcpartno"] = drc["pcpartno"].ToString().Trim();
                            dro["ibname"] = drc["itemname"].ToString().Trim();
                            dro["cpartno"] = drc["cpartno"].ToString().Trim();
                            dro["unit"] = drc["Bunit"].ToString().Trim();
                            dro["pcode"] = drc["icode"].ToString().Trim();
                            mainLotSize = fgen.make_double(drc["main_issue_no"].ToString().Trim());
                            if (mainLotSize <= 0) mainLotSize = 1;
                            dro["ibqty"] = fgen.make_double(drc["ibqty"].ToString()) / mainLotSize;
                            dro["ibcode"] = drc["ibcode"].ToString().Trim();
                            dro["irate"] = drc["itrate"].ToString().Trim();
                            if (co_cd == "NIRM" && dtPo != null)
                            {
                                //cVa += fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + drc["ibcode"].ToString().Trim() + "'", "prate"));
                                //cVa += fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + drc["icode"].ToString().Trim() + "'", "prate"));
                                dro["jr"] = cVa;
                            }
                            dro["val"] = "0";
                            mdt1.Rows.Add(dro);
                        }
                        i0 = 1; v = 0;
                        for (int i = v; i < mdt1.Rows.Count; i++)
                        {
                            //vipin
                            if (vdt.Rows.Count <= 0) break;
                            DataView vdview = new DataView(vdt, "icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                            if (vdview.Count > 0)
                            {
                                DataView vdview1 = new DataView(mdt1, "icode='" + mdt1.Rows[i]["icode"].ToString().Trim() + "' and ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "' and ibqty='" + mdt1.Rows[i]["ibqty"] + "'", "ibcode", DataViewRowState.CurrentRows);
                                if (vdview1.Count <= 0) vdview1 = new DataView(mdt1, "ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "ibcode", DataViewRowState.CurrentRows);

                                for (int x = 0; x < vdview.Count; x++)
                                {
                                    if (mq0 != vdview[x].Row["icode"].ToString().Trim())
                                    {
                                        value3 = fgen.seek_iname_dt(mdt1, "IBCODE='" + vdview[x].Row["icode"].ToString().Trim() + "'", "LVL", "LVL DESC");
                                        if (value3 == "0")
                                            i0 += 1;
                                        else i0 = fgen.make_int(value3) + 1;
                                    }
                                    dro = mdt1.NewRow();
                                    dro["lvl"] = i0.ToString();
                                    dro["icode"] = vdview[x].Row["icode"].ToString().Trim();
                                    dro["branchcd"] = vdview[x].Row["branchcd"].ToString().Trim();
                                    mq0 = vdview[x].Row["icode"].ToString().Trim();
                                    double lotSize = fgen.make_double(vdview[x].Row["MAIN_ISSUE_NO"].ToString().Trim());
                                    if (lotSize <= 0) lotSize = 1;
                                    dro["ibqty"] = (Convert.ToDouble(vdview[x].Row["ibqty"]) * (Convert.ToDouble(vdview1[0].Row["ibqty"]) / lotSize)).ToString();
                                    dro["ibcode"] = vdview[x].Row["ibcode"].ToString().Trim();
                                    //dro["irate"] = vdview[0].Row["bchrate"];
                                    dro["irate"] = vdview[x].Row["bchrate"];
                                    dro["ibname"] = vdview[x].Row["itemname"];
                                    dro["cpartno"] = vdview[x].Row["cpartno"].ToString().Trim();
                                    dro["unit"] = vdview[x].Row["Bunit"].ToString().Trim();

                                    if (co_cd == "NIRM" && dtPo != null)
                                        dro["jr"] = fgen.seek_iname_dt(dtPo, "icode='" + vdview[x].Row["ibcode"].ToString().Trim() + "'", "prate");

                                    dro["val"] = "0";
                                    if (mdt1.Rows[i]["lvl"].ToString() == "1")
                                    {
                                        mq7 = "";
                                        dro["pcode"] = mdt1.Rows[i]["icode"].ToString().Trim();
                                        mq7 = mdt1.Rows[i]["icode"].ToString().Trim();
                                    }
                                    else dro["pcode"] = mq7;

                                    dro["vcode"] = mdt1.Rows[i]["icode"].ToString().Trim();
                                    v++;

                                    mdt1.Rows.Add(dro);
                                } vdview1.Dispose();
                            } vdview.Dispose();

                            //mdt1.DefaultView.Sort = "pcode,lvl,icode";
                            //mdt1 = mdt1.DefaultView.ToTable();
                        }

                        //mdt1.DefaultView.Sort = "pcode,lvl,icode";
                        //mdt1 = mdt1.DefaultView.ToTable();

                        // seeking LC and update value
                        value1 = "";
                        for (int i = 0; i < mdt1.Rows.Count; i++)
                        {

                            DataView vdview = new DataView(mdt1, "branchcd='" + mdt1.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                            if (vdview.Count <= 0)
                            {
                                if (co_cd != "KCLG")
                                {
                                    if (val != "29157")
                                    {
                                        if (dt2.Rows.Count > 0)
                                        {
                                            DataView sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                            if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                            else
                                            {
                                                sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                            }
                                        }
                                    }
                                }
                            }
                            else mdt1.Rows[i]["irate"] = "0";
                            vdview.Dispose();


                            string fl1 = "";
                            string fl2 = "";
                            fl1 = mdt1.Rows[i]["ibqty"].ToString();
                            fl2 = mdt1.Rows[i]["irate"].ToString();
                            mdt1.Rows[i]["val"] = fgen.make_double(fl1) * fgen.make_double(fl2);

                            //mdt1.Rows[i]["val"] = Convert.ToDouble(fgen.make_double(mdt1.Rows[i]["ibqty"].ToString()) * fgen.make_double(mdt1.Rows[i]["irate"].ToString()));

                            double dvl = 0;
                            if (co_cd == "NIRM")
                            {
                                dvl += fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "prate"));
                                if (fgen.make_double(mdt1.Rows[i]["jr"].ToString()) <= 0)
                                    mdt1.Rows[i]["jr"] = dvl;
                            }
                        }

                        mq0 = "0";
                        mq7 = "0";

                        if (co_cd == "NIRM")
                        {
                            dist1_view = new DataView(mdt1);
                            DataTable dt_dist1 = new DataTable();
                            if (dist1_view.Count > 0)
                            {
                                dist1_view.Sort = "pcode";
                                dt_dist1 = dist1_view.ToTable(true, "pcode");
                            }
                            foreach (DataRow drdist1 in dt_dist1.Rows)
                            {
                                dro = mdt1.NewRow();
                                dro["icode"] = drdist1["pcode"].ToString().Trim();
                                dro["ibcode"] = drdist1["pcode"].ToString().Trim();
                                dro["pcode"] = drdist1["pcode"].ToString().Trim();
                                dro["ibqty"] = 0;
                                dro["irate"] = 0;
                                dro["val"] = 0;
                                dro["jr"] = fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + drdist1["pcode"].ToString().Trim() + "'", "prate"));
                                mdt1.Rows.Add(dro);
                            }
                        }

                        // making final value
                        //if (co_cd == "NIRM")
                        {
                            DataView vdview = new DataView(mdt1, "pcode='" + dt_dist_row["icode"].ToString().Trim() + "'", "pcode", DataViewRowState.CurrentRows);
                            for (int i = 0; i < vdview.Count; i++)
                            {
                                if (Convert.ToDouble(mq0) > 0) mq0 = Math.Round(Convert.ToDouble(mq0) + Convert.ToDouble(vdview[i].Row["val"].ToString().Trim()), 5).ToString();
                                else mq0 = vdview[i].Row["val"].ToString().Trim();

                                if (co_cd == "NIRM")
                                {
                                    joVal += fgen.make_double(vdview[i].Row["jr"].ToString().Trim());
                                }
                            }
                        }
                        if (joVal <= 0)
                        {
                            //for (int i = 0; i < dt_dist.Rows.Count; i++)
                            {
                                double dvl = 0;
                                dvl = fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "prate"));
                                mdt1.Rows[0]["jr"] = dvl;
                                joVal += dvl;
                            }
                        }
                        //vdview.Dispose();

                        db6 = 0;
                        db5 = 0;
                        double mul_fact = 0;

                        if (co_cd == "DREM")
                        {
                            if (bomanx.Rows.Count > 0)
                            {
                                mul_fact = 0;
                                DataView vdview = new DataView(bomanx, "ICODE='" + mdt1.Rows[0]["PCODE"].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                for (int i = 0; i < vdview.Count; i++)
                                {
                                    if (mainLotSize > 0)
                                        mul_fact = fgen.make_double(mdt1.Rows[0]["IBQTY"].ToString().Trim(), 0);
                                    if (mul_fact < 1) mul_fact = 1;

                                    db5 = ((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact);
                                    db6 += db5;
                                }
                            }
                        }

                        for (int f = 0; f < mdt1.Rows.Count; f++)
                        {
                            mdt.ImportRow(mdt1.Rows[f]);
                            if (co_cd == "DREM")
                            {
                                if (bomanx.Rows.Count > 0)
                                {
                                    mul_fact = 0;
                                    DataView vdview = new DataView(bomanx, "ICODE='" + mdt1.Rows[f]["IBCODE"].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                    for (int i = 0; i < vdview.Count; i++)
                                    {
                                        if (mainLotSize > 0)
                                            mul_fact = fgen.make_double(mdt1.Rows[f]["IBQTY"].ToString().Trim(), 0);
                                        if (mul_fact < 1) mul_fact = 1;

                                        db5 = ((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact);
                                        db6 += db5;
                                    }
                                }
                            }
                        }

                        mdt1.Dispose();

                        // mdt is table which is having Bom in Expended Form
                        dro1 = fmdt.NewRow();
                        dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
                        dro1["val"] = mq0;
                        if (co_cd == "NIRM")
                            dro1["jo_val"] = joVal;
                        if (co_cd == "DREM")
                        {
                            if (db6 > 0)
                                dro1["jo_val"] = fgen.make_double(db6 / mainLotSize, 4);

                            dro1["lot_size"] = mainLotSize;
                        }
                        fmdt.Rows.Add(dro1);
                        // fmdt is table which is only having Parant Bom icode and Value
                        double finRate = joVal + fgen.make_double(mq0);
                        if (co_cd == "NIRM" || co_cd == "SAGM")
                        {
                            fgen.execute_cmd(frm_qstr, co_cd, "UPDATE ITEM SET IRATE= '" + finRate + "' WHERE TRIM(ICODE)='" + dt_dist_row["icode"].ToString().Trim() + "'");
                        }
                        if (co_cd == "PRAG" || co_cd == "IAIJ" || co_cd == "DREM*" || (co_cd == "BUPL" && dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7"))
                        {
                            fgen.execute_cmd(frm_qstr, co_cd, "UPDATE ITEM SET IRATE= '" + mq0 + "' WHERE TRIM(ICODE)='" + dt_dist_row["icode"].ToString().Trim() + "'");
                        }
                    }
                    if (val == "F05124" || val == "29155a")
                    {
                        #region Print Report
                        mq7 = ""; mq5 = "-";
                        mdt1 = new DataTable();
                        mdt1 = fmdt.Clone();
                        dro1 = null;
                        dt = new DataTable();
                        cond = "";
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR7").Length > 1) cond = " and trim(a.acode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR7") + "'";
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR7").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR8").Length > 1) cond = " and trim(a.acode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR7") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR8") + "'";
                        SQuery = "Select ROUND((case when a.irate>0 then A.irate else b.irate end)-(CASE WHEN A.ICHGS>0 THEN ROUND(A.IRATE * (A.ICHGS/100),2) ELSE 0 END ),2) as rate,SUM(a.iqtyout) AS iqtyout,TRIM(a.acode) AS acode,TRIM(a.icode) AS icode from ivoucher a ,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type like '4%' AND A.VCHDATE " + xprdrange + " and a.icode like '9%' " + cond + " GROUP BY TRIM(a.acode),TRIM(a.icode),a.irate,B.irate,A.ICHGS order by TRIM(A.ACODE),TRIM(a.icode)";

                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        DataView sort_view = new DataView();
                        if (dt.Rows.Count > 0)
                        {
                            sort_view = dt.DefaultView;
                            sort_view.Sort = "acode";
                            dt3 = new DataTable();
                            dt3 = sort_view.ToTable(true, "acode");
                        }
                        for (int i = 0; i < fmdt.Rows.Count; i++)
                        {
                            //foreach (DataRow drr3 in dt3.Rows)
                            {
                                int k = 0;
                                //vdview = new DataView(dt, "acode='" + drr3["acode"].ToString().Trim() + "' and icode='" + fmdt.Rows[i]["icode"].ToString().Trim() + "'", "acode", DataViewRowState.CurrentRows);
                                DataView vdview = new DataView();
                                if (dt.Rows.Count > 0)
                                    vdview = new DataView(dt, "icode='" + fmdt.Rows[i]["icode"].ToString().Trim() + "'", "acode,icode", DataViewRowState.CurrentRows);
                                for (int x = 0; x < vdview.Count; x++)
                                {
                                    dro1 = mdt1.NewRow();
                                    dro1["icode"] = fmdt.Rows[i]["icode"].ToString().Trim();
                                    dro1["val"] = fmdt.Rows[i]["val"].ToString().Trim();
                                    //if (k == 0) 
                                    dro1["srate"] = vdview[x].Row["rate"].ToString().Trim();
                                    //else dro1["srate"] = "0";
                                    dro1["sqty"] = vdview[x].Row["iqtyout"].ToString().Trim();
                                    dro1["acode"] = vdview[x].Row["acode"].ToString().Trim();
                                    mdt1.Rows.Add(dro1);

                                    //break;
                                    k = 1;
                                }
                            }
                        }
                        fmdt = new DataTable();
                        fmdt = mdt1;

                        fgen.execute_cmd(frm_qstr, co_cd, "DELETE FROM EXTRUSION WHERE BRANCHCD='" + mbr + "' AND TYPE='EX' AND VCHNUM='000000' ");

                        DataSet oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, co_cd, "EXTRUSION");
                        dro = null;
                        foreach (DataRow fdr in fmdt.Rows)
                        {
                            dro = oDS.Tables[0].NewRow();
                            dro["BRANCHCD"] = mbr;
                            dro["type"] = "EX";
                            dro["vchnum"] = "000000";
                            dro["vchdate"] = DateTime.Now;
                            dro["icode"] = fdr["icode"].ToString().Trim();
                            dro["qty"] = Math.Round(Convert.ToDouble(fdr["val"].ToString().Replace(" ", "0")), 5);
                            dro["btchno"] = Math.Round(Convert.ToDouble(fdr["srate"].ToString().Replace(" ", "0")), 5);
                            dro["start1"] = Math.Round(Convert.ToDouble(fdr["sqty"].ToString().Replace(" ", "0")), 5);
                            //dro["comments"] = fmdt.Rows[i]["ibqty"];                            
                            dro["chars"] = fdr["acode"];
                            dro["ent_by"] = uname;
                            dro["ent_dt"] = DateTime.Now;

                            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR10") == "N") dro["start2"] = 0;
                            else dro["start2"] = 1;
                            dro["close1"] = 0;
                            dro["close2"] = 0;
                            dro["rpm1"] = 0;
                            dro["rpm2"] = 0;
                            dro["DISPERSION1"] = 0;
                            dro["DISPERSION2"] = 0;
                            dro["srno"] = 0;
                            dro["btchdt"] = DateTime.Now;
                            dro["extloss"] = 0;

                            oDS.Tables[0].Rows.Add(dro);
                        }
                        fgen.save_data(frm_qstr, co_cd, oDS, "EXTRUSION");
                        oDS.Dispose(); mdt.Dispose(); fmdt.Dispose();

                        {
                            cond = "and substr(icode,1,1)='9'";
                            mq0 = "select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " " + cond + " union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' " + cond + " GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE";

                        }

                        SQuery = "Select '" + fromdt + "' as fromdt,'" + todt + "' as todt, a.chars as partycode,c.aname as party,trim(a.icode) as erpcode,b.iname as product,b.cpartno,b.unit,round(sum(to_number(replace(NVL(a.comments,0),'-','0'))),5) as bom_qty,(a.qty) as Bom_val,TO_NUMBER(sum(a.start1)) as Sal_qty,max(is_number(a.btchno)) as sal_rate,'" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR10") + "' as lcp from extrusion a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.chars)=trim(c.acode) AND A.BRANCHCD='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' group by trim(a.icode),a.chars,b.iname,b.cpartno,b.unit,a.chars,c.aname,a.qty order by c.aname,trim(a.icode),b.iname";

                        if (co_cd == "KHEM" || co_cd == "KCLG" || co_cd == "XDIL" || co_cd == "GTCF") SQuery = "Select '" + fromdt + "' as fromdt,'" + todt + "' as todt, a.chars as partycode,c.aname as party,trim(a.icode) as erpcode,b.iname as product,b.cpartno,b.unit,round(sum(to_number(replace(NVL(a.comments,0),'-','0'))),5) as bom_qty,(a.qty) as Bom_val,TO_NUMBER(sum(a.start1)) as Sal_qty,max(is_number(a.btchno)) as sal_rate from extrusion a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.chars)=trim(c.acode) AND A.BRANCHCD='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' group by trim(a.icode),a.chars,b.iname,b.cpartno,b.unit,a.chars,c.aname,a.qty order by c.aname,trim(a.icode),b.iname";
                        if (val == "29155a") fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "bomvssal", "bomvssalp");
                        else fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "bomvssal", "bomvssal");
                        #endregion
                    }
                    else if (val == "29157")
                    {
                        #region
                        dist1_view = new DataView(mdt);
                        dt = new DataTable();
                        dt = dist1_view.ToTable(true, "lvl");

                        dist1_view = new DataView(mdt, "LVL=1", "pcode", DataViewRowState.CurrentRows);
                        dt2 = new DataTable();
                        dt2 = dist1_view.ToTable(true, "pcode", "INAME", "PCPARTNO");

                        dt3 = new DataTable();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt3.Columns.Add("lvl" + (i + 1), typeof(double));
                            dt3.Columns.Add("pcode" + (i + 1), typeof(string));
                            if (i == 0)
                            {
                                dt3.Columns.Add("piname", typeof(string));
                                dt3.Columns.Add("ppartcode", typeof(string));
                            }
                            dt3.Columns.Add("icode" + (i + 1), typeof(string));
                            dt3.Columns.Add("partcode" + (i + 1), typeof(string));
                            dt3.Columns.Add("iname" + (i + 1), typeof(string));
                            dt3.Columns.Add("qty" + (i + 1), typeof(double));
                        }
                        dt3.Columns.Add("IBCODE", typeof(double));
                        dt3.Columns.Add("NTWT", typeof(double));
                        dt3.Columns.Add("NTWTTOT", typeof(double));
                        dt3.Columns.Add("GRWT", typeof(double));
                        dt3.Columns.Add("GRWTTOT", typeof(double));
                        dt3.Columns.Add("UNIT", typeof(string));

                        dro = null;

                        double lvl = 0;
                        string ibcode = "";
                        DataTable dtn = new DataTable();
                        ViewState["CINDEX"] = "1";
                        for (int k = 0; k < dt2.Rows.Count; k++)
                        {
                            dtn = getChildItems(mdt, dt2.Rows[k]["pcode"].ToString().Trim(), 1, dt3, "Y", "");
                        }
                        //cow
                        for (int r = 0; r < dt3.Rows.Count; r++)
                        {
                            for (int c = 0; c < dt3.Columns.Count; c++)
                            {
                                if (dt3.Columns[c].ColumnName.ToUpper().Contains("ICODE"))
                                {
                                    fgen.valFound = "N";
                                    string colName = dt3.Columns[c].ColumnName.ToString();
                                    string colName2 = dt3.Columns[c].ColumnName.ToString().ToUpper().Replace("ICODE", "PCODE");
                                    mq0 = fgen.seek_iname_dt(vdt, "IBCODE='" + dt3.Rows[r][colName].ToString() + "' AND ICODE='" + dt3.Rows[r][colName2].ToString() + "'", "SUB_ISSUE_NO");
                                    mq1 = fgen.seek_iname_dt(vdt, "IBCODE='" + dt3.Rows[r][colName].ToString() + "' AND ICODE='" + dt3.Rows[r][colName2].ToString() + "'", "IBDIEPC");
                                    int prvLevel = fgen.make_int(colName.ToUpper().Replace("ICODE", "")) - 1;
                                    if (prvLevel > 0)
                                    {
                                        //mq7 = fgen.seek_iname_dt(dt3, "ICODE" + prvLevel + "='" + dt3.Rows[r][colName2].ToString() + "'", "QTY" + prvLevel);
                                        mq7 = dt3.Rows[r]["QTY" + prvLevel].ToString();
                                    }
                                    else mq7 = "0";

                                    //if (fgen_fun.valFound == "Y")
                                    if (prvLevel > 0)
                                    {
                                        dt3.Rows[r]["NTWT"] = mq0;
                                        dt3.Rows[r]["NTWTTOT"] = fgen.make_double(mq0) * fgen.make_double(mq7);

                                        dt3.Rows[r]["GRWT"] = mq1;
                                        dt3.Rows[r]["GRWTTOT"] = fgen.make_double(mq1) * fgen.make_double(mq7);
                                    }
                                    mq0 = "0"; mq1 = "0"; mq7 = "0";
                                }
                            }

                            dt3.Rows[r]["piname"] = fgen.seek_iname_dt(dt2, "PCODE='" + dt3.Rows[r]["PCODE1"].ToString().Trim() + "'", "INAME");
                            dt3.Rows[r]["ppartcode"] = fgen.seek_iname_dt(dt2, "PCODE='" + dt3.Rows[r]["PCODE1"].ToString().Trim() + "'", "PCPARTNO");
                        }

                        for (int c = 0; c < dt3.Columns.Count; c++)
                        {
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("LVL", "BOM LEVEL ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("PCODE", "PARENT CODE ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("PINAME", "Main Product ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("INAME", "PRODUCT ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("ICODE", "ERP CODE ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("QTY", "QUANTITY ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("PARTCODE", "PART CODE ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("NTWT", "Net Wt ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("NTWTTOT", "Tot Net Wt ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("GRWT", "Gross Wt ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("GRWTTOT", "Tot Gross Wt ");
                            dt3.Columns[c].ColumnName = dt3.Columns[c].ColumnName.ToUpper().Replace("UNIT", "UOM ");
                            dt3.AcceptChanges();
                        }

                        dt3.Columns.Remove("IBCODE");
                        Session["send_dt"] = dt3;
                        fgen.Fn_open_rptlevel("Expended BOM", frm_qstr);
                        #endregion
                    }
                    else
                    {
                        #region CostSheet Report
                        string std_loss1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='B25' AND ENABLE_YN='Y'", "PARAMS");
                        if (fgen.make_double(std_loss1) <= 0) std_loss1 = "3";

                        DataSet oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, co_cd, "EXTRUSION");
                        dro = null;
                        foreach (DataRow fdr in fmdt.Rows)
                        {
                            dro = oDS.Tables[0].NewRow();
                            dro["BRANCHCD"] = mbr;
                            dro["type"] = "EX";
                            dro["vchnum"] = "000000";
                            dro["vchdate"] = DateTime.Now;
                            dro["icode"] = fdr["icode"].ToString().Trim();
                            dro["qty"] = Math.Round(Convert.ToDouble(fdr["val"].ToString().Trim()), 5);
                            dro["start1"] = 0;
                            dro["start2"] = 0;
                            dro["ent_by"] = uname;
                            dro["ent_dt"] = DateTime.Now;
                            if (co_cd == "NIRM" || co_cd == "DREM")
                                dro["start2"] = fgen.make_double(fdr["jo_Val"].ToString().Trim(), 4);
                            if (co_cd == "DREM")
                                dro["close1"] = fgen.make_double(fdr["lot_size"].ToString().Trim(), 4);
                            else dro["close1"] = 0;
                            dro["close2"] = 0;
                            dro["rpm1"] = 0;
                            dro["rpm2"] = 0;
                            dro["DISPERSION1"] = 0;
                            dro["DISPERSION2"] = 0;
                            dro["srno"] = 0;
                            dro["btchdt"] = DateTime.Now;
                            dro["extloss"] = 0;
                            oDS.Tables[0].Rows.Add(dro);
                        }
                        fgen.save_data(frm_qstr, co_cd, oDS, "EXTRUSION");
                        oDS.Dispose(); mdt.Dispose(); fmdt.Dispose();
                        SQuery = "Select distinct a.icode,b.iname as product,b.cpartno,b.unit,a.qty as val,a.start2 as job_Val,(a.qty+a.start2) as tot_value from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        if (co_cd == "DREM") SQuery = "Select distinct a.icode as erpcode,b.iname as product,b.cpartno,b.unit,a.close1 as lot_size,(a.close1 * a.qty) as matl_cost_amt,a.qty as per_pcs_cost,(a.close1 * a.start2) as process_cost_amt,a.start2 as per_pcs_proc_value,(a.close1 * a.start2) + (a.close1 * a.qty) as total_cost,(a.qty+a.start2) as per_pcs_tot_cost from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        fgen.send_cookie("seekSql", SQuery);
                        fgen.Fn_open_rptlevel("Cost Sheet of Products for the period " + fromdt + " and " + todt + "", frm_qstr);
                        #endregion
                    }
                    break;

                case "F05125":
                case "F05125A":
                case "F05125C":
                    fgen.execute_cmd(frm_qstr, co_cd, "delete from extrusion where branchcd='" + mbr + "' and type='EX' AND TRIM(ENT_BY)='" + uname + "'");
                    mdt = new DataTable(); dt3 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); mdt1 = new DataTable();

                    cond = "";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(a.icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";

                    cond = cond.Length > 2 ? cond : " AND TRIM(A.ICODE) LIKE '9%' ";

                    branch_Cd = (co_cd == "KHEM" || co_cd == "KCLG" || co_cd == "BUPL" || co_cd == "XDIL" || co_cd == "DREM" || co_cd == "LOGW" || co_cd == "ROOP" || co_cd == "WING" || co_cd == "SDM" || co_cd == "DLJM" || co_cd == "AEPL" || co_cd == "MINV") ? "BRANCHCD NOT IN ('DD','88')" : "BRANCHCD='" + mbr + "'";

                    SQuery = "Select a.ICODE,A.IBCODE,A.IBQTY,A.MAIN_ISSUE_NO,A.IBWT,A.IOQTY,A.IBDIEPC,A.SUB_ISSUE_NO,a.srno,0 as irate,0 as val,'1' as lvl,(case when B.IQD>0 then B.IQD else B.irate end) as itrate from itemosp2 a,item b where trim(a.ibcode)=trim(b.icode) " + cond + " AND a." + branch_Cd + " order by a.srno,a.icode";
                    if (co_cd != "MINV")
                        itemospDT2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (itemospDT2.Rows.Count <= 0)
                    {
                        string spCond = "";
                        if (co_cd == "MINV")
                        {
                            spCond = "and trim(a.icode) in (select erpcode from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " " + cond.Replace("a.", "") + " union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' " + cond.Replace("a.", "") + " GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) where Closing_Stk!=0 )";
                        }
                        SQuery = "Select a.ICODE,A.IBCODE,A.IBQTY,A.MAIN_ISSUE_NO,A.IBWT,A.IBDIEPC,A.SUB_ISSUE_NO,A.IOQTY,a.srno,0 as irate,0 as val,'1' as lvl,(case when B.IQD>0 then B.IQD else B.irate end) as itrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) " + cond + " AND a." + branch_Cd + " " + spCond + " order by a.srno,a.icode";
                        itemospDT2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }

                    mdt.Columns.Add(new DataColumn("lvl", typeof(string)));
                    mdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("pcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("spcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibqty", typeof(double)));
                    mdt.Columns.Add(new DataColumn("grwt", typeof(double)));
                    mdt.Columns.Add(new DataColumn("ntwt", typeof(double)));
                    mdt.Columns.Add(new DataColumn("ibcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("irate", typeof(double)));
                    mdt.Columns.Add(new DataColumn("nrate", typeof(double)));
                    mdt.Columns.Add(new DataColumn("val", typeof(double)));
                    mdt.Columns.Add(new DataColumn("nval", typeof(double)));
                    mdt.Columns.Add(new DataColumn("pcost", typeof(double)));
                    mdt.Columns.Add(new DataColumn("fcost", typeof(double)));
                    mdt.Columns.Add(new DataColumn("lot_size", typeof(string)));
                    mdt.Columns.Add(new DataColumn("msrno", typeof(double)));

                    mdt.Columns.Add("BRANCHCD", typeof(string));
                    mdt.Columns.Add("VCHNUM", typeof(string));
                    mdt.Columns.Add("VCHDATE", typeof(string));
                    mdt.Columns.Add("ACODE", typeof(string));
                    mdt.Columns.Add("TYPE", typeof(string));
                    mdt.Columns.Add("sort", typeof(string));

                    fmdt = new DataTable();
                    fmdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("val", typeof(double)));
                    fmdt.Columns.Add(new DataColumn("nval", typeof(double)));
                    fmdt.Columns.Add(new DataColumn("jo_val", typeof(double)));
                    fmdt.Columns.Add(new DataColumn("lot_size", typeof(string)));

                    fullDt = new DataTable();
                    fullDt = mdt.Clone();
                    fullDt.Columns.Add("srno", typeof(int));
                    //fullDt.Columns.Add("BRANCHCD", typeof(string));
                    //fullDt.Columns.Add("VCHNUM", typeof(string));
                    //fullDt.Columns.Add("VCHDATE", typeof(string));
                    //fullDt.Columns.Add("ACODE", typeof(string));
                    //fullDt.Columns.Add("sort", typeof(string));

                    SQuery = "Select a.ICODE,A.IBCODE,A.IBQTY,A.MAIN_ISSUE_NO,A.IBWT,A.IBDIEPC,A.SUB_ISSUE_NO,A.IOQTY,a.srno,0 as irate,0 as val,'1' as lvl,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a." + branch_Cd + " order by a.srno,a.icode,a.ibcode";
                    itemospDT = fgen.getdata(frm_qstr, co_cd, SQuery); v = 0;

                    dt2 = new DataTable();
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR9") == "Y") rmcBranch = "BRANCHCD NOT IN ('DD','88')";
                    else rmcBranch = "BRANCHCD='" + mbr + "'";

                    cond = "and (trim(nvl(finvno,'-'))!='-' or type='0U')";
                    if (co_cd == "MINV") cond = "";
                    rateCond = " type like '0%' ";
                    if (co_cd == "SAGM") rateCond = "type in ('02','07')";
                    if (co_cd == "BUPL") rateCond = "type in ('02','05','07')";
                    SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,trim(acode) as acode,trim(vchnum) as vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,to_Char(vchdate,'yyyymmdd') as vdd,TYPE from ivoucher where " + rmcBranch + " and " + rateCond + " " + cond + " and vchdate>=(to_date('" + todt + "','dd/mm/yyyy')-500) and vchdate<=to_date('" + todt + "','dd/mm/yyyy') /*and icode like '9%'*/ order by icode,vdd desc";
                    //wtd avg rate
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR10") == "N")
                    {
                        SQuery = "Select '-' as branchcd,trim(icode) as icode,round((sum(iqty_chl*ichgs) / sum(iqty_chl)) ,3) as rate,'-' as acode,'-' as vchnum,'-' as vchdate,'-' as type,1 AS VDD from ivoucher where " + rmcBranch + " and " + rateCond + " " + cond + " and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY') and substr(icode,1,1)<9 group by trim(icode) order by icode";
                        if (co_cd == "SAGM" && mbr == "08") rmcBranch = "branchcd in ('06','08')";
                        SQuery = "select branchcd,icode, round((case when sum(iqty_chl * ichgs)>0 then (sum(iqty_chl * ichgs) / sum(iqty_chl)) else 0 end),3) as rate,acode,vchnum,vchdate,type,vdd from (Select '-' as branchcd,trim(icode) as icode,iqty_chl,ICHGS,'-' as acode,'-' as vchnum,'-' as vchdate,'-' as type,1 AS VDD from ivoucher where " + rmcBranch + " and " + rateCond + " " + cond + " and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY') and substr(icode,1,1)<'9' union all Select '-' as branchcd,trim(A.icode) as icode,A.YR_" + year + ",b.irate,'-' as acode,'-' as vchnum,'-' as vchdate,'-' as type,1 AS VDD from ITEMBAL A,ITEM B WHERE A." + rmcBranch + " and TRIM(A.ICODE)=TRIM(b.ICODE) AND substr(A.icode,1,1)<'9' ) group by branchcd,icode,acode,vchnum,vchdate,type,vdd";
                    }
                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);

                    SQuery = "SELECT DISTINCT ICODE,MAINTDT,SRNO FROM INSPMST WHERE " + branch_Cd + " AND TYPE='70' AND SRNO=1 ORDER BY ICODE ";
                    insvchDT = fgen.getdata(frm_qstr, co_cd, SQuery);

                    bomanx = new DataTable();
                    if (co_cd == "DREM")
                    {
                        SQuery = "Select c.iname,c.cpartno,b.name as names,a.* from itemospanx a,type b,item c  where b.id='1' and trim(a.icode)=trim(c.icode) and trim(a.stg_Cd)=trim(b.type1) and a.branchcd!='DD'";
                        bomanx = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }

                    dist1_view = new DataView(itemospDT2);
                    dt_dist = new DataTable();
                    if (dist1_view.Count > 0)
                    {
                        dist1_view.Sort = "icode";
                        dt_dist = dist1_view.ToTable(true, "icode");
                    }
                    mainLotSize = 0;
                    msrno = 0;
                    double bomSrno = 1;
                    foreach (DataRow dt_dist_row in dt_dist.Rows)
                    {
                        runningNo = dt_dist.Rows.IndexOf(dt_dist_row);
                        mdt1 = new DataTable();
                        mdt1 = mdt.Clone();
                        mvdview = new DataView(itemospDT2, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "icode,ibcode", DataViewRowState.CurrentRows);
                        dt = new DataTable();
                        mvdview.Sort = "srno,icode";
                        dt = mvdview.ToTable();
                        // filling parent
                        double cVa = 0;
                        double db1 = 0;
                        double db2 = 0;
                        double mul_fact = 0; db5 = 0; db6 = 0;
                        DataRow fdtr;
                        bomSrno = msrno;
                        msrno++;
                        foreach (DataRow drc in dt.Rows)
                        {
                            dro = mdt1.NewRow();
                            dro["lvl"] = "1";
                            dro["icode"] = drc["icode"].ToString().Trim();
                            dro["pcode"] = drc["icode"].ToString().Trim();
                            dro["spcode"] = drc["icode"].ToString().Trim();
                            spcode = drc["icode"].ToString().Trim();
                            mainLotSize = fgen.make_double(drc["main_issue_no"].ToString().Trim());
                            ViewState["MAINLOTSIZE"] = mainLotSize;
                            if (co_cd == "NEOP") mainLotSize = 1;
                            if (co_cd == "MINV")
                            {
                                mainLotSize = fgen.make_double(drc["IBWT"].ToString().Trim());
                                if (mainLotSize <= 0) mainLotSize = 100;
                            }
                            if (mainLotSize <= 0) mainLotSize = 1;
                            dro["ibqty"] = fgen.make_double(fgen.make_double(drc["ibqty"].ToString(), 6) / mainLotSize, 6);
                            dro["ibcode"] = drc["ibcode"].ToString().Trim();
                            dro["irate"] = drc["itrate"];

                            dro["GRWT"] = drc["IBDIEPC"];
                            dro["NTWT"] = drc["SUB_ISSUE_NO"];

                            dro["nval"] = 0;
                            dro["nrate"] = 0;
                            dro["val"] = 0;
                            dro["msrno"] = msrno;
                            mdt1.Rows.Add(dro);
                            msrno++;
                        }
                        db6 = 0;
                        db5 = 0;
                        if (co_cd == "DREM")
                        {
                            if (bomanx.Rows.Count > 0)
                            {
                                mul_fact = 0;
                                DataView vdview = new DataView(bomanx, "ICODE='" + mdt1.Rows[0]["PCODE"].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                for (int i = 0; i < vdview.Count; i++)
                                {
                                    if (mainLotSize > 0)
                                        mul_fact = fgen.make_double(mdt1.Rows[0]["IBQTY"].ToString().Trim(), 0);
                                    if (mul_fact < 1) mul_fact = 1;

                                    db5 = Math.Round(((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact), 5);
                                    mdt1.Rows[0]["PCOST"] = fgen.make_double((db5 / mainLotSize) + mdt1.Rows[0]["PCOST"].ToString().toDouble(5), 5);
                                    db6 += db5;
                                }

                                foreach (DataRow mdr1 in mdt1.Rows)
                                {
                                    mul_fact = 0;
                                    vdview = new DataView(bomanx, "ICODE='" + mdr1["ibCODE"].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                    for (int i = 0; i < vdview.Count; i++)
                                    {
                                        if (mainLotSize > 0)
                                            mul_fact = fgen.make_double(mdr1["IBQTY"].ToString().Trim(), 0);
                                        if (mul_fact < 1) mul_fact = 1;

                                        db5 = Math.Round(((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact), 5);
                                        mdr1["PCOST"] = fgen.make_double((db5 / mainLotSize) + mdr1["PCOST"].ToString().toDouble(5), 5);
                                        db6 += db5;
                                    }
                                }
                            }
                        }
                        ViewState["db6"] = "0";
                        foreach (DataRow mdr1 in mdt1.Rows)
                        {
                            spcode = mdr1["ibcode"].ToString().Trim();
                            if ((co_cd == "MINV" || co_cd == "BUPL") && (mdr1["ibcode"].ToString().Trim().Substring(0, 1) == "7" || mdr1["ibcode"].ToString().Trim().Substring(0, 1) == "8" || mdr1["ibcode"].ToString().Trim().Substring(0, 1) == "9")) mdr1["irate"] = getBOMVal(mdr1["ibcode"].ToString().Trim(), dt_dist_row["icode"].ToString().Trim());
                            else if (mdr1["ibcode"].ToString().Trim().Substring(0, 1) == "7")
                            {
                                mdr1["irate"] = getBOMVal(mdr1["ibcode"].ToString().Trim(), dt_dist_row["icode"].ToString().Trim());

                                if (mdr1["irate"].ToString().toDouble() == 0)
                                {
                                    if (dt2.Rows.Count <= 0)
                                    { }
                                    else
                                    {
                                        DataView sort_view = new DataView(dt2, "trim(icode)='" + mdr1["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                        if (sort_view.Count > 0) mdr1["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                        else
                                        {
                                            sort_view = new DataView(dt2, "trim(icode)='" + mdr1["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                            if (sort_view.Count > 0) mdr1["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                        }
                                    }

                                }
                            }
                            else
                            {
                                if (dt2.Rows.Count > 0)
                                {
                                    DataView sort_view = new DataView(dt2, "trim(icode)='" + mdr1["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                    if (sort_view.Count > 0)
                                    {
                                        mdr1["irate"] = sort_view[0].Row["rate"].ToString().Trim();

                                        mdr1["BRANCHCD"] = sort_view[0].Row["BRANCHCD"].ToString().Trim();
                                        mdr1["VCHNUM"] = sort_view[0].Row["VCHNUM"].ToString().Trim();
                                        mdr1["VCHDATE"] = sort_view[0].Row["VCHDATE"].ToString().Trim();
                                        mdr1["ACODE"] = sort_view[0].Row["ACODE"].ToString().Trim();
                                        mdr1["TYPE"] = sort_view[0].Row["TYPE"].ToString().Trim();
                                    }
                                }
                            }

                            mdr1["val"] = fgen.make_double(fgen.make_double(mdr1["irate"].ToString()) * fgen.make_double(mdr1["ibqty"].ToString()), 5);

                            db1 += fgen.make_double(mdr1["val"].ToString());

                            if (mdr1["ibcode"].ToString().Trim().Substring(0, 1) == "2")
                                db2 += fgen.make_double(mdr1["val"].ToString(), 5);

                            fdtr = fullDt.NewRow();
                            fdtr["srno"] = runningNo;
                            fdtr["lvl"] = mdr1["lvl"].ToString().Trim();
                            fdtr["sort"] = "B";
                            fdtr["pcode"] = dt_dist_row["icode"].ToString().Trim();
                            fdtr["icode"] = mdr1["icode"].ToString().Trim();
                            fdtr["ibqty"] = mdr1["ibqty"].ToString().Trim();
                            fdtr["ibcode"] = mdr1["ibcode"].ToString().Trim();
                            fdtr["irate"] = mdr1["irate"].ToString().Trim();
                            fdtr["val"] = mdr1["val"].ToString().Trim().toDouble(5);

                            fdtr["GRWT"] = mdr1["grwt"];
                            fdtr["NTWT"] = mdr1["ntwt"];

                            fdtr["PCOST"] = mdr1["PCOST"];
                            //fdtr["lot_size"] = mdr1["lot_size"];

                            fdtr["BRANCHCD"] = mdr1["BRANCHCD"].ToString().Trim();
                            fdtr["VCHNUM"] = mdr1["VCHNUM"].ToString().Trim();
                            fdtr["VCHDATE"] = mdr1["VCHDATE"].ToString().Trim();
                            fdtr["ACODE"] = mdr1["ACODE"].ToString().Trim();
                            fdtr["TYPE"] = mdr1["TYPE"].ToString().Trim();
                            fdtr["msrno"] = mdr1["msrno"];

                            fullDt.Rows.Add(fdtr);
                        }

                        db6 += ViewState["db6"].ToString().toDouble(5);

                        //double finRate = joVal + fgen.make_double(mq0);
                        dro1 = fmdt.NewRow();
                        dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
                        dro1["val"] = db1;
                        dro1["jo_val"] = db2;
                        if (co_cd == "DREM")
                        {
                            if (db6 > 0)
                                dro1["jo_val"] = fgen.make_double(db6 / mainLotSize, 5);

                            //dro1["lot_size"] = mainLotSize;
                        }
                        fmdt.Rows.Add(dro1);

                        fdtr = fullDt.NewRow();
                        fdtr["srno"] = runningNo;
                        fdtr["lvl"] = "0";
                        fdtr["sort"] = "A";
                        fdtr["pcode"] = dt_dist_row["icode"].ToString().Trim();
                        fdtr["icode"] = dt_dist_row["icode"].ToString().Trim();
                        fdtr["val"] = db1.toDouble(5);
                        fdtr["pcost"] = fgen.make_double(db6 / mainLotSize, 5);
                        fdtr["lot_size"] = mainLotSize;
                        fdtr["msrno"] = bomSrno.ToString() + ".1";
                        fdtr["fcost"] = Math.Round(fgen.make_double(fdtr["val"].ToString(), 5) + fgen.make_double(fdtr["pcost"].ToString(), 5), 5);
                        fullDt.Rows.Add(fdtr);
                        bomSrno++;
                    }
                    if (val == "F05125A" || val == "F05125C")
                    {
                        if (co_cd == "SAGM")
                        {
                            for (int i = 0; i < fullDt.Rows.Count; i++)
                            {
                                if (fullDt.Rows[i]["lvl"].ToString().toDouble() == 1)
                                {
                                    if (fullDt.Rows[i]["ibcode"].ToString().Substring(0, 1) == "7")
                                    {
                                        fullDt.Rows[i]["nrate"] = getNrate(fullDt, fullDt.Rows[i]["pcode"].ToString(), fullDt.Rows[i]["ibcode"].ToString()).toDouble(4);
                                        fullDt.Rows[i]["nval"] = fullDt.Rows[i]["nrate"].ToString().toDouble() * fullDt.Rows[i]["ntwt"].ToString().toDouble();
                                    }
                                }
                            }
                        }
                        if (co_cd == "DREM")
                        {
                            string lvl = fullDt.Compute("max(lvl)", "").ToString();
                            for (int i = 0; i < lvl.toDouble(); i++)
                            {
                                fullDt.Columns.Add("V" + (i + 1), typeof(double));
                            }
                            for (int i = 0; i < lvl.toDouble(); i++)
                            {
                                fullDt.Columns.Add("P" + (i + 1), typeof(double));
                            }
                            fullDt.DefaultView.Sort = "lvl desc,PCODE";
                            fullDt = fullDt.DefaultView.ToTable();
                            spcode = "";
                            for (int i0 = 0; i0 < fullDt.Rows.Count; i0++)
                            {
                                if (fullDt.Rows[i0]["lvl"].ToString().toDouble() > 0)
                                {
                                    spcode = fullDt.Rows[i0]["spcode"].ToString();
                                    try
                                    {
                                        fullDt.Rows[i0]["V" + fullDt.Rows[i0]["lvl"]] = getVal(fullDt, fullDt.Rows[i0]["pcode"].ToString(), fullDt.Rows[i0]["icode"].ToString(), fullDt.Rows[i0]["ibcode"].ToString()).toDouble();
                                        fullDt.Rows[i0]["val"] = fullDt.Rows[i0]["V" + fullDt.Rows[i0]["lvl"]].ToString().toDouble();
                                    }
                                    catch { }
                                    double mul_fact = 0;
                                    db6 = 0;
                                    string pickcode = "";

                                    if (bomanx.Rows.Count > 0)
                                    {
                                        mul_fact = 0;
                                        pickcode = "IBCODE";
                                        if (fullDt.Rows[i0]["lvl"].ToString() == "2")
                                        {

                                        }

                                        DataView vdview = new DataView(bomanx, "" + "icode" + "='" + fullDt.Rows[i0][pickcode].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                        for (int i = 0; i < vdview.Count; i++)
                                        {
                                            if (mainLotSize > 0)
                                                mul_fact = fgen.make_double(fullDt.Rows[i0]["IBQTY"].ToString().Trim(), 0);
                                            if (mul_fact < 1) mul_fact = 1;

                                            db5 = Math.Round(((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact), 5);
                                            if (fullDt.Rows[i0]["lvl"].ToString() == "1") fullDt.Rows[i0]["P2"] = (db5 / mainLotSize) + fullDt.Rows[i0]["P2"].ToString().toDouble();
                                            else fullDt.Rows[i0]["P" + fullDt.Rows[i0]["lvl"]] = (db5 / mainLotSize) + fullDt.Rows[i0]["P" + fullDt.Rows[i0]["lvl"]].ToString().toDouble();
                                        }
                                        if (fullDt.Rows[i0]["lvl"].ToString() == "1")
                                        {
                                            pickcode = "ICODE";
                                            if (fullDt.Rows[i0][pickcode].ToString().Trim() == "71110152")
                                            {

                                            }
                                            mhd = fullDt.Compute("sum(p1)", "LVL='1' AND ICODE='" + fullDt.Rows[i0][pickcode].ToString().Trim() + "'").ToString();
                                            if (mhd.toDouble() <= 0)
                                            {
                                                vdview = new DataView(bomanx, "" + "icode" + "='" + fullDt.Rows[i0][pickcode].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                                for (int i = 0; i < vdview.Count; i++)
                                                {
                                                    if (mainLotSize > 0)
                                                        mul_fact = fgen.make_double(fullDt.Rows[i0]["IBQTY"].ToString().Trim(), 0);
                                                    if (mul_fact < 1) mul_fact = 1;

                                                    db5 = Math.Round(((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact), 5);
                                                    fullDt.Rows[i0]["P1"] = (db5 / mainLotSize) + fullDt.Rows[i0]["P1"].ToString().toDouble();
                                                }

                                                pickcode = "IBCODE";
                                                //                                                mhd = fullDt.Compute("sum(p2)", "LVL='1' AND ICODE='" + fullDt.Rows[i0][pickcode].ToString().Trim() + "'").ToString();                                                
                                                {
                                                    vdview = new DataView(bomanx, "" + "icode" + "='" + fullDt.Rows[i0][pickcode].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                                    if (vdview.Count > 0) fullDt.Rows[i0]["P2"] = "0";
                                                    for (int i = 0; i < vdview.Count; i++)
                                                    {
                                                        if (mainLotSize > 0)
                                                            mul_fact = fgen.make_double(fullDt.Rows[i0]["IBQTY"].ToString().Trim(), 0);
                                                        if (mul_fact < 1) mul_fact = 1;

                                                        db5 = Math.Round(((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact), 5);
                                                        fullDt.Rows[i0]["P2"] = (db5 / mainLotSize) + fullDt.Rows[i0]["P2"].ToString().toDouble();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                pickcode = "IBCODE";
                                                //mhd = fullDt.Compute("sum(p2)", "LVL='1' AND ICODE='" + fullDt.Rows[i0][pickcode].ToString().Trim() + "'").ToString();                                                
                                                {
                                                    vdview = new DataView(bomanx, "" + "icode" + "='" + fullDt.Rows[i0][pickcode].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                                    if (vdview.Count > 0) fullDt.Rows[i0]["P2"] = "0";
                                                    for (int i = 0; i < vdview.Count; i++)
                                                    {
                                                        if (mainLotSize > 0)
                                                            mul_fact = fgen.make_double(fullDt.Rows[i0]["IBQTY"].ToString().Trim(), 0);
                                                        if (mul_fact < 1) mul_fact = 1;

                                                        db5 = Math.Round(((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact), 5);
                                                        fullDt.Rows[i0]["P2"] = (db5 / mainLotSize) + fullDt.Rows[i0]["P2"].ToString().toDouble();
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //fullDt.Rows[i0]["P" + fullDt.Rows[i0]["lvl"]] = fullDt.Rows[i0]["pcost"].ToString();
                                }
                            }


                            fullDt.DefaultView.Sort = "SRNO,LVL,SORT,PCODE,ICODE";
                            fullDt = fullDt.DefaultView.ToTable();
                            DataView dvDi = new DataView(fullDt);
                            dt = dvDi.ToTable(true, "PCODE");
                            dtSort = fullDt.Clone();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                fillBomLevelWise(dt.Rows[i]["PCODE"].ToString().Trim(), "");
                            }
                            fullDt = dtSort;
                        }

                        if (co_cd != "DREM")
                        {
                            DataView dsor = new DataView(fullDt, "", "SRNO,LVL,SORT,PCODE,ICODE", DataViewRowState.CurrentRows);
                            fullDt = new DataTable();
                            fullDt = dsor.ToTable();
                        }

                        DataTable toShow = new DataTable();
                        toShow.Columns.Add("Level", typeof(string));
                        toShow.Columns.Add("ERPCode", typeof(string));
                        toShow.Columns.Add("Name", typeof(string));
                        toShow.Columns.Add("ChildCode", typeof(string));
                        toShow.Columns.Add("ChildName", typeof(string));
                        toShow.Columns.Add("PartName", typeof(string));
                        toShow.Columns.Add("Gr.Wt(BOM)", typeof(string));
                        toShow.Columns.Add("Nt.Wt(BOM)", typeof(string));
                        toShow.Columns.Add("Qty", typeof(string));
                        toShow.Columns.Add("UOM", typeof(string));
                        string totlvlcount = "";
                        if (co_cd == "DREM")
                        {
                            string lvl = fullDt.Compute("max(lvl)", "").ToString();
                            totlvlcount = lvl;
                            for (int i = 0; i < lvl.toDouble(); i++)
                            {
                                toShow.Columns.Add("V" + (i + 1), typeof(string));
                            }
                            for (int i = 0; i < lvl.toDouble(); i++)
                            {
                                toShow.Columns.Add("P" + (i + 1), typeof(string));
                            }
                        }
                        toShow.Columns.Add("LandedCost", typeof(string));
                        toShow.Columns.Add("Cost", typeof(string));
                        if (co_cd == "SAGM")
                        {
                            toShow.Columns.Add("Net_Rate", typeof(string));
                            toShow.Columns.Add("Net_Cost", typeof(string));
                        }
                        if (val == "F05125C")
                        {
                            toShow.Columns.Add("Sales_Qty", typeof(string));
                            toShow.Columns.Add("Mat_Cost", typeof(string));
                            if (co_cd == "SAGM")
                            {
                                toShow.Columns.Add("Mat_Net_Cost", typeof(string));
                            }
                        }
                        if (co_cd == "DREM")
                        {
                            toShow.Columns.Add("ProcessCost", typeof(string));
                            toShow.Columns.Add("FinalCost", typeof(string));
                            toShow.Columns.Add("LotSize", typeof(string));
                        }
                        toShow.Columns.Add("BranchCode", typeof(string));
                        toShow.Columns.Add("BranchName", typeof(string));
                        toShow.Columns.Add("VendorCode", typeof(string));
                        toShow.Columns.Add("VendorName", typeof(string));
                        toShow.Columns.Add("MRR_Date", typeof(string));
                        toShow.Columns.Add("MRR_No", typeof(string));
                        toShow.Columns.Add("MRR_Type", typeof(string));


                        DataTable dtItem = new DataTable();
                        dtItem = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,INAME,CPARTNO,UNIT FROM ITEM WHERE LENGTH(TRIM(ICODe))>4 ORDER BY ICODE ");

                        DataTable dtFamst = new DataTable();
                        dtFamst = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(aCODE) AS aCODE,ANAME FROM FAMST WHERE SUBSTR(ACODE,1,2) IN ('02','05','06','16') ORDER BY ACODE ");

                        DataTable dtBranch = new DataTable();
                        dtBranch = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(type1) AS type1,NAME FROM type WHERE ID='B' ORDER BY TYPE1 ");

                        DataRow drToShow;

                        foreach (DataRow drr in fullDt.Rows)
                        {
                            if (drr["LVL"] == "0" && toShow.Rows.Count > 0)
                            {
                                drToShow = toShow.NewRow();
                                toShow.Rows.Add(drToShow);
                            }
                            drToShow = toShow.NewRow();
                            if (drr["LVL"] == "0") drToShow["Level"] = "FG Item";
                            else
                            {
                                int pad = fgen.make_int(drr["LVL"].ToString());
                                drToShow["Level"] = drr["LVL"].ToString().Trim().PadLeft((pad * 2), '_');
                            }

                            drToShow["ERPCODE"] = drr["ICODE"].ToString().Trim();
                            drToShow["Name"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drr["ICODE"].ToString().Trim() + "'", "INAME");
                            if (drr["IBCODE"].ToString().Trim().Length > 0)
                            {
                                drToShow["childcode"] = drr["IBCODE"].ToString().Trim();
                                drToShow["childname"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drr["IBCODE"].ToString().Trim() + "'", "INAME");
                                drToShow["uom"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drr["IBCODE"].ToString().Trim() + "'", "unit");
                                drToShow["PartName"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drr["IBCODE"].ToString().Trim() + "'", "cpartno"); ;
                            }

                            drToShow["Gr.Wt(BOM)"] = drr["grwt"].ToString().Trim();
                            drToShow["Nt.Wt(BOM)"] = drr["ntwt"].ToString().Trim();

                            drToShow["Qty"] = drr["IBQTY"].ToString().Trim().toDouble(6);
                            drToShow["LandedCost"] = drr["IRATE"].ToString().Trim();
                            drToShow["Cost"] = drr["VAL"].ToString().Trim().toDouble(5);

                            if (co_cd == "SAGM")
                            {
                                drToShow["Net_Rate"] = drr["nrate"].ToString().Trim().toDouble(5);
                                if (drr["LVL"] == "0")
                                    drToShow["Net_Cost"] = fullDt.Compute("sum(nval)", "PCODE='" + drr["PCODE"].ToString().Trim() + "' AND LVL=1").ToString().toDouble(5);
                                else
                                    drToShow["Net_Cost"] = drr["nval"].ToString().Trim().toDouble(5);
                            }

                            if (co_cd == "DREM")
                            {
                                if (drr["LVL"] != "0")
                                {
                                    for (int p = 1; p <= Convert.ToInt32(totlvlcount); p++)
                                    {
                                        drToShow["V" + p] = drr["V" + p].ToString().toDouble(5);
                                        drToShow["P" + p] = drr["P" + p].ToString().toDouble(5);
                                    }
                                }

                                drToShow["ProcessCost"] = drr["pcost"].ToString().Trim().toDouble(5);
                                drToShow["FinalCost"] = drr["fcost"].ToString().Trim().toDouble(5);

                                drToShow["LotSize"] = drr["lot_size"].ToString().Trim();
                            }

                            if (drr["branchcd"].ToString().Trim().Length > 0)
                            {
                                drToShow["branchcode"] = drr["branchcd"].ToString().Trim();
                                drToShow["branchName"] = fgen.seek_iname_dt(dtBranch, "TYPE1='" + drr["branchcd"].ToString().Trim() + "'", "NAME");
                            }
                            if (drr["acode"].ToString().Trim().Length > 0)
                            {
                                drToShow["Vendorcode"] = drr["acode"].ToString().Trim();
                                drToShow["Vendorname"] = fgen.seek_iname_dt(dtFamst, "ACODE='" + drr["acode"].ToString().Trim() + "'", "aNAME");
                            }
                            drToShow["MRR_No"] = drr["vchnum"].ToString().Trim();
                            drToShow["MRR_Date"] = drr["vchdate"].ToString().Trim();
                            drToShow["MRR_Type"] = drr["type"].ToString().Trim();

                            toShow.Rows.Add(drToShow);
                        }

                        if (val == "F05125C")
                        {
                            DataTable dtSale = new DataTable();
                            dtSale = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,SUM(IQTYOUT) AS QTY_SALE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdrange + " GROUP BY TRIM(ICODE) ORDER BY ICODE");

                            foreach (DataRow drtoshow in toShow.Rows)
                            {
                                if (drtoshow["Level"].ToString().ToUpper().Trim().Contains("FG"))
                                {
                                    mhd = fgen.seek_iname_dt(dtSale, "ICODE='" + drtoshow["ERPCODE"].ToString().Trim() + "'", "QTY_SALE");
                                    if (mhd != "0")
                                    {
                                        drtoshow["Sales_Qty"] = mhd;
                                        drtoshow["Mat_Cost"] = Math.Round(mhd.toDouble() * drtoshow["Cost"].ToString().toDouble(), 3);
                                        if (co_cd == "SAGM")
                                        {
                                            drtoshow["Mat_Net_Cost"] = Math.Round(mhd.toDouble() * drtoshow["Net_Cost"].ToString().toDouble(), 3);
                                        }
                                    }
                                }
                            }
                        }

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                        Session["send_dt"] = toShow;

                        if (toShow.Rows.Count > 3000) fgen.exp_to_excel(toShow, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim());
                        else
                            fgen.Fn_open_rptlevelJS("BOM Cost - Working File [" + (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR9") == "Y" ? "All Plant" : "Single Plant") + "]", frm_qstr);
                    }
                    else
                    {
                        string std_loss = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='B25' AND ENABLE_YN='Y'", "PARAMS");
                        if (fgen.make_double(std_loss) <= 0) std_loss = "3";
                        string fixed_profit = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                        if (fgen.make_double(fixed_profit) <= 0) fixed_profit = "35";

                        DataSet oDS = new DataSet();
                        if (co_cd != "MINV")
                        {
                            oDS = fgen.fill_schema(frm_qstr, co_cd, "extrusion");
                            dro = null;
                            foreach (DataRow fdr in fmdt.Rows)
                            {
                                dro = oDS.Tables[0].NewRow();
                                dro["BRANCHCD"] = mbr;
                                dro["type"] = "EX";
                                dro["vchnum"] = "000000";
                                dro["vchdate"] = DateTime.Now;
                                dro["icode"] = fdr["icode"].ToString().Trim();
                                dro["qty"] = Math.Round(Convert.ToDouble(fdr["val"].ToString().Trim()), 2);
                                dro["start1"] = 0;
                                dro["start2"] = 0;
                                dro["ent_by"] = uname;
                                dro["ent_dt"] = DateTime.Now;
                                //if (co_cd == "NIRM")
                                dro["start2"] = fdr["jo_Val"].ToString().Trim();
                                dro["close1"] = fgen.make_double(fdr["val"].ToString().Trim()) - fgen.make_double(fdr["jo_Val"].ToString().Trim());

                                if (co_cd == "NIRM" || co_cd == "PRAG" || co_cd == "DREM")
                                    dro["start2"] = fgen.make_double(fdr["jo_Val"].ToString().Trim(), 4);
                                if (co_cd == "DREM")
                                    dro["close1"] = fgen.make_double(fdr["lot_size"].ToString().Trim(), 4);
                                else dro["close1"] = 0;

                                dro["close2"] = 0;
                                dro["rpm1"] = 0;
                                dro["rpm2"] = 0;
                                dro["DISPERSION1"] = 0;
                                dro["DISPERSION2"] = 0;
                                dro["srno"] = 0;
                                dro["btchdt"] = DateTime.Now;
                                dro["extloss"] = 0;
                                oDS.Tables[0].Rows.Add(dro);
                            }
                            fgen.save_data(frm_qstr, co_cd, oDS, "extrusion");
                            oDS.Dispose(); mdt.Dispose(); fmdt.Dispose();
                        }
                        SQuery = "Select distinct a.icode,b.iname as product,b.cpartno,b.unit,a.close1 as rm_cost,'" + fgen.make_double(std_loss) + " %' as std_loss,to_number(a.start2) as pm_cost ,a.close1 + (a.close1 * (" + fgen.make_double(std_loss) + " / 100)) + to_number(a.start2) as total_pmrc,'" + fixed_profit + " %' as fixed_profit,(a.close1 + (a.close1 * (" + fgen.make_double(std_loss) + " / 100)) + to_number(a.start2)) + ((a.close1 + (a.close1 * (" + fgen.make_double(std_loss) + " / 100)) + to_number(a.start2)) * (" + fixed_profit + " / 100)) as fixed_sale_price from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        if (co_cd == "DREM") SQuery = "Select distinct a.icode as erpcode,b.iname as product,b.cpartno,b.unit,a.close1 as lot_size,(a.close1 * a.qty) as matl_cost_amt,a.qty as per_pcs_cost,(a.close1 * a.start2) as process_cost_amt,a.start2 as per_pcs_proc_value,(a.close1 * a.start2) + (a.close1 * a.qty) as total_cost,(a.qty+a.start2) as per_pcs_tot_cost from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        cond = "and substr(icode,1,1)='9'";
                        branch_Cd = "BRANCHCD='" + mbr + "'";
                        mq0 = "select * from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " " + cond + " union all select branchcd,TRIM(ICODE) AS ICODE,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as clos, 0 as aaa1,0 as aaa2 FROM IVOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " " + cond + " and store='Y'  GROUP BY TRIM(ICODE),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' " + cond + " GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) where closing_stk!=0 ";

                        if (co_cd == "MINV")
                        {
                            //SQuery = "SELECT A.*,ROUND(A.CLOSING_STK*A.total_cost,3) AS STK_VALUE FROM (Select distinct a.icode,b.iname as product,b.cpartno,b.unit,a.close1 as rm_cost,b.oprate3 as std_wstg,to_number(b.oprate2) as pm_cost ,a.close1 + (a.close1 * (is_number(b.oprate3) / 100)) + to_number(b.oprate2) as total_pmrc,b.oprate1 as manuf_cost,(a.close1 + (a.close1 * (to_number(b.oprate3) / 100)) + to_number(b.oprate2) + to_number(b.oprate1)) as total_cost,c.opening,c.Rcpt,c.Issued,c.closing_stk from extrusion a,item b,(" + mq0 + ") C where trim(a.icode)=trim(b.icode) and trim(a.icode)=trim(c.erpcode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX') A ORDER BY A.ICODE ";
                            DataTable toShow = new DataTable();
                            toShow.Columns.Add("icode", typeof(string));
                            toShow.Columns.Add("product", typeof(string));
                            toShow.Columns.Add("cpartno", typeof(string));
                            toShow.Columns.Add("unit", typeof(string));
                            toShow.Columns.Add("rm_cost", typeof(string));
                            toShow.Columns.Add("std_wstg", typeof(string));
                            toShow.Columns.Add("pm_cost", typeof(string));
                            toShow.Columns.Add("total_pmrc", typeof(string));
                            toShow.Columns.Add("manuf_cost", typeof(string));
                            toShow.Columns.Add("total_cost", typeof(string));
                            toShow.Columns.Add("opening", typeof(string));
                            toShow.Columns.Add("Rcpt", typeof(string));
                            toShow.Columns.Add("Issued", typeof(string));
                            toShow.Columns.Add("closing_stk", typeof(string));
                            toShow.Columns.Add("stk_value", typeof(string));
                            DataRow drShow;

                            DataTable dtStock = new DataTable();
                            dtStock = fgen.getdata(frm_qstr, co_cd, mq0);

                            DataTable dtItem = new DataTable();
                            dtItem = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,INAME,CPARTNO,UNIT,oprate3,oprate2,oprate1 FROM ITEM WHERE LENGTH(TRIM(ICODe))>4 ORDER BY ICODE ");

                            foreach (DataRow fdr in fmdt.Rows)
                            {
                                drShow = toShow.NewRow();
                                drShow["icode"] = fdr["icode"];
                                DataView d = new DataView(dtItem, "ICODE='" + fdr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                if (d.Count > 0)
                                {
                                    drShow["product"] = d[0].Row["iname"];
                                    drShow["cpartno"] = d[0].Row["cpartno"];
                                    drShow["unit"] = d[0].Row["unit"];
                                    drShow["std_wstg"] = d[0].Row["oprate3"];
                                    drShow["pm_cost"] = d[0].Row["oprate2"];
                                    drShow["manuf_cost"] = d[0].Row["oprate1"];
                                }
                                drShow["rm_cost"] = fgen.make_double(fdr["val"].ToString().Trim()) - fgen.make_double(fdr["jo_Val"].ToString().Trim());
                                drShow["total_pmrc"] = drShow["rm_cost"].ToString().toDouble() + (drShow["rm_cost"].ToString().toDouble() * (drShow["std_wstg"].ToString().toDouble() * .01)) + drShow["pm_cost"].ToString().toDouble();
                                drShow["total_cost"] = drShow["total_pmrc"].ToString().toDouble() + drShow["manuf_cost"].ToString().toDouble();
                                if (dtStock.Rows.Count > 0)
                                {
                                    d = new DataView(dtStock, "ERPCODE='" + fdr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    if (d.Count > 0)
                                    {
                                        drShow["opening"] = d[0].Row["opening"];
                                        drShow["Rcpt"] = d[0].Row["Rcpt"];
                                        drShow["Issued"] = d[0].Row["Issued"];
                                        drShow["closing_stk"] = d[0].Row["closing_stk"];
                                    }
                                }
                                drShow["stk_value"] = drShow["closing_stk"].ToString().toDouble() * drShow["total_cost"].ToString().toDouble();
                                toShow.Rows.Add(drShow);
                            }
                            //SQuery = "SELECT A.*,ROUND(A.CLOSING_STK*A.total_cost,3) AS STK_VALUE FROM (Select distinct a.icode,b.iname as product,b.cpartno,b.unit,a.close1 as rm_cost,b.oprate3 as std_wstg,to_number(b.oprate2) as pm_cost ,a.close1 + (a.close1 * (is_number(b.oprate3) / 100)) + to_number(b.oprate2) as total_pmrc,b.oprate1 as manuf_cost,(a.close1 + (a.close1 * (to_number(b.oprate3) / 100)) + to_number(b.oprate2) + to_number(b.oprate1)) as total_cost,c.opening,c.Rcpt,c.Issued,c.closing_stk from extrusion a,item b,(" + mq0 + ") C where trim(a.icode)=trim(b.icode) and trim(a.icode)=trim(c.erpcode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX') A ORDER BY A.ICODE ";
                            Session["send_dt"] = toShow;
                            SQuery = "";
                        }

                        if (co_cd == "MINV")
                        {
                            dt5 = new DataTable();
                            dt5 = fgen.getdata(frm_qstr, co_cd, SQuery);
                            foreach (DataRow dr5 in dt5.Rows)
                            {
                                fgen.execute_cmd(frm_qstr, co_cd, "UPDATE ITEM SET IRATE1='" + dr5["total_cost"].ToString().Trim().toDouble() + "' where TRIM(ICODE)='" + dr5["ICODE"].ToString().Trim() + "'");
                            }
                        }

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Cost Sheet of Products for the period " + fromdt + " and " + todt + "", frm_qstr);
                    }
                    break;
                case "F05110":
                    hf1.Value = value1;
                    SQuery = "Select Wk_ref as Week_no,wk_Ref,sum(num01) as Qty,max(TO_CHAR(vchdate,'DD/MM/YYYY')) as Last_dt,to_char(max(Vchdate),'yyyymmdd') As dt_Str from sl_plan where branchcd='" + mbr + "' and type='SL' and vchdate  between to_Date('13/09/2017','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')  and num01>0 and wk_Ref>0 and upper(trim(isarch))<>'Y' group by wk_Ref having sum(num01)>0 order by wk_Ref desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek("Select Week", frm_qstr);
                    break;
                case "F05125D":
                case "F05125E":
                    SQuery = "SELECT A.BRANCHCD,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,(A.IQTYOUT-a.iqtyin) as Quantity,(case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end) as rate,abs(round((a.iqtyin-A.IQTYOUT) * (case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end))) as value,'From Item Master Rate' as rate_from,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODe)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='" + (val == "F05125D" ? "36" : "3F") + "' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC ";
                    if (frm_formID == "F05125E")
                    {
                        SQuery = "SELECT A.BRANCHCD,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.ACODE AS STG_CODE,C.NAME AS STAGE_NAME,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,(A.IQTYOUT-a.iqtyin) as Quantity,(case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end) as rate,abs(round((a.iqtyin-A.IQTYOUT) * (case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end))) as value,'From Item Master Rate' as rate_from,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODe)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACREF) AND C.ID='WI' AND C.BRANCHCD='" + mbr + "' AND A.BRANCHCD='" + mbr + "' AND A.TYPE='" + (val == "F05125D" ? "36" : "3F") + "' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC ";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    // mrr table
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where BRANCHCD='" + mbr + "' and type in ('02','07') and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and vchdate<=to_date('" + todt + "','dd/mm/yyyy') /*and icode like '9%'*/ order by icode,vdd desc");
                    // sale table
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where BRANCHCD='" + mbr + "' and substr(type,1,1) in ('4') and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and vchdate<=to_date('" + todt + "','dd/mm/yyyy') and icode like '9%' order by icode,vdd desc");
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["erpcode"].ToString().Trim().Substring(0, 1) != "9")
                        {
                            mhd = fgen.seek_iname_dt(dt2, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "rate");
                            if (mhd.toDouble() > 0)
                            {
                                dr["rate"] = mhd;
                                dr["value"] = Math.Round(mhd.toDouble() * dr["Quantity"].ToString().toDouble(), 2);
                                dr["rate_from"] = "Latest MRR";
                            }
                        }
                        if (dr["erpcode"].ToString().Trim().Substring(0, 1) == "9")
                        {
                            mhd = fgen.seek_iname_dt(dt3, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "rate");
                            if (mhd.toDouble() > 0)
                            {
                                dr["rate"] = mhd;
                                dr["value"] = Math.Round(mhd.toDouble() * dr["Quantity"].ToString().toDouble(), 2);
                                dr["rate_from"] = "Latest Invoice Rate";
                            }
                        }
                    }
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("" + (val == "F05125D" ? "Store Variance Details " : "WIP Variance Details ") + " of Products for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;
                case "F05124J":
                    DataTable dtm = new DataTable();
                    dtm.Columns.Add("SrNo", typeof(string));
                    dtm.Columns.Add("JobCard_No", typeof(string));
                    dtm.Columns.Add("JobCard_Dt", typeof(string));
                    dtm.Columns.Add("Item_Code", typeof(string));
                    dtm.Columns.Add("Item_Name", typeof(string));
                    dtm.Columns.Add("Box_Wt", typeof(double));
                    dtm.Columns.Add("Total_Qty", typeof(double));
                    dtm.Columns.Add("Box_Cost_SP", typeof(double));
                    dtm.Columns.Add("JobCost_AsPer_SalePrice", typeof(double));
                    dtm.Columns.Add("Total_Paper_Cost", typeof(double));
                    dtm.Columns.Add("PaperCost_Per_KG", typeof(double));
                    dtm.Columns.Add("Avg_Conv_Cost", typeof(double));
                    dtm.Columns.Add("Act_Conv_Cost", typeof(double));
                    dtm.Columns.Add("Profit_Per_KG", typeof(double));
                    dtm.Columns.Add("Total_Profit", typeof(double));

                    mq0 = "SELECT trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy') as fstr,A.SALERATE,A.ENQNO AS JOBNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS JOBDT,A.ACODE AS ICODE,B.INAME AS PRODUCT,c.col15 as ply,c.rejqty as ups,A.TOT_BOX_RCV as TOT_BOX_REQ,a.STD_SHT_RQ as STD_SHT_RQ_WITH_WSTG, /* (a.STD_SHT_RQ*(is_number(c.col2)/1000)) AS std_MTR_reqd,*/ round(a.enr2/a.STD_SHT_RQ,4) AS STD_WT_PER_SHEET_per_JC,round((a.enr2/a.STD_SHT_RQ)/c.rejqty,4) AS STD_WT_PERBOX_per_JC,A.COL3 AS NO_OF_SHT_RCVD,A.QTYOUT AS BOX_to_be_PRODUCED,round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3) AS STD_WT_rQD,A.QTYIN AS WT_CONSUME,a.scrp1 as gsm_var,a.scrp2 as fala,a.time1 as tore,a.time2 as core,(A.QTYIN - (round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ) /c.rejqty,3)),3))) as PAPER_WST_corr,d.mlt_loss as print_stg_rej,(d.mlt_loss * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)) as prt_wt_loss,D.mlt_loss1 as gluing,(d.mlt_loss1 * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)) as glu_wt_loss , D.mlt_loss2 as other_S,(d.mlt_loss2 * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)) as oth_wt_loss, (  (A.QTYIN - round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)) + (d.mlt_loss * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)) + (D.mlt_loss1 * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3))) + (d.mlt_loss2 * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)) as paper_Wstg ,A.IQTYIN AS FINAL_BOX_PROD,(CASE WHEN (A.TOT_BOX_RCV-A.IQTYIN)<0 THEN ABS(A.TOT_BOX_RCV-A.IQTYIN) ELSE 0 END) AS EXCESS,(CASE WHEN (A.TOT_BOX_RCV-A.IQTYIN)>0 THEN ABS(A.TOT_BOX_RCV-A.IQTYIN) ELSE 0 END) AS SHORTAGE,round((case when A.QTYIN>0 and a.val>0 then (a.val/A.QTYIN) else 0 end),4) as matl_avg_rate, round((case when A.QTYIN>0 and a.val>0 then round(a.val/A.QTYIN,3) else 0 end) * ( (A.QTYIN - round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)) + (d.mlt_loss * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)) + (d.mlt_loss1 * round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)))  ) as cost_of_wstg,(A.IQTYIN*A.SALERATE) as job_vALUE,A.VAL AS mat_cost, (case when (A.IQTYIN*A.SALERATE)>0 then round(((A.IQTYIN*A.SALERATE)-a.val) / (A.IQTYIN*A.SALERATE) ,3)*100 else 0 end ) as jOB_proft_per,round((A.IQTYIN*A.SALERATE)-a.val ,3) as JOB_profit_Value,(case when (A.QTYOUT*A.SALERATE)>0 then round(((A.QTYOUT*A.SALERATE)-a.val) / (A.QTYOUT*A.SALERATE) ,3)*100 else 0 end ) as corr_per,round((A.qtyout*A.SALERATE)-a.val ,3) as CORR_PROF_VAL, (round((A.iqtyin*A.SALERATE)-a.val ,3) - round((A.qtyout*A.SALERATE)-a.val ,3))  as diff_VALUE ,ROUND((CASE WHEN A.TOT_BOX_RCV>0 THEN ((A.TOT_BOX_RCV-A.QTYOUT)/A.TOT_BOX_RCV)*100 ELSE 0 END),2)  as variation_per FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,c.IRATE AS SALERATE FROM (select enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from (select INVNO AS ENQNO,INVDATE AS ENQDT,TRIM(ICODE) AS ACODE,0 as qtyin,0 as qtyout,0 AS COL3,0 as scrp1,0 as scrp2,0 as time1,0 as time2,SUM(IQTYIN) AS IQTYIN,0 AS VAL from IVOUCHER where BRANCHCD='" + mbr + "' AND type='16' and INVDATE " + xprdrange + " group by INVNO,INVDATE,TRIM(ICODE) union all select A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS QTYIN1,SUM(ROUND(A.ITATE*B.IRATE,2)) AS VAL from costestimate A,REELVCH B where TRIM(A.ICODE)||TRIM(A.COL6)=TRIM(B.ICODE)||TRIM(B.KCLREELNO) AND A.BRANCHCD='" + mbr + "' AND A.type='25' AND B.TYPE='02' and A.enqdt " + xprdrange + " group by A.enqno,A.enqdt,TRIM(A.aCODE) union all select a.enqno,a.enqdt,TRIM(a.aCODE) AS ACODE,0 as qtyin,sum(a.qty + is_number(replace(nvl(b.COL3,'0'),'-','0')) ) as qtyout,is_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate a left outer join " + frm_inspvch + " b on trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and b.type='45' where a.BRANCHCD='" + mbr + "' and a.type='40' and a.enqdt " + xprdrange + " group by a.enqno,a.enqdt,TRIM(a.aCODE),is_number(replace(nvl(a.COL3,'0'),'-','0')) ) group by enqno,enqdt,acode) A ";
                    mq1 = " ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b.BRANCHCD='" + mbr + "' AND B.TYPE='30' AND B.SRNO=0) A,ITEM B,inspmst c,(SELECT SUM(A.MLT_LOSS) AS MLT_LOSS,SUM(A.MLT_LOSS1) AS MLT_LOSS1,SUM(A.MLT_LOSS2) AS MLT_LOSS2,A.job_no,A.job_Dt,A.icode FROM (select sum(A.mlt_loss) as mlt_loss,0 AS mlt_loss1,0 AS mlt_loss2 ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from " + frm_prodsheet + " A ,TYPE B where TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='09' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss, sum(A.mlt_loss) as mlt_loss1,0 AS mlt_loss2 ,A.job_no,A.job_Dt,TRIM(A.icode) AS ICODE from " + frm_prodsheet + " A,TYPE B where TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='06' group by A.job_no,A.job_Dt,TRIM(A.ICODE) UNION select 0 AS mlt_loss,0 AS mlt_loss1,sum(A.mlt_loss) as mlt_loss2 ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from " + frm_prodsheet + " A,TYPE B where TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='11' group by A.job_no,A.job_Dt,TRIM(A.icode)) A GROUP BY A.job_no,A.job_Dt,A.icode) D WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy')||trim(a.acode)=trim(D.job_no)||to_char(to_Date(D.job_Dt,'dd/mm/yyyy'),'dd/mm/yyyy')||trim(D.icode) and c.type='70' and c.srno=10 ORDER BY TO_CHAR(A.ENQDT,'YYYYmmdd') desc,a.enqno desc";
                    SQuery = mq0 + mq1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    mq2 = "select sum(qty) as qty,trim(acode) as acode,trim(enqno) as enqno,to_char(enqdt,'dd/mm/yyyy') as enqdt from costestimate where branchcd='" + mbr + "' and type='40' and enqdt " + xprdrange + " group by acode,enqno,to_char(enqdt,'dd/mm/yyyy')";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq2);

                    mq3 = "select  trim(a.icode) as icode,a.col6,TRIM(a.acode) as acode,trim(a.enqno) as enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,b.irate,a.itate,b.vchnum,to_char(b.vchdate,'dd/mm/yyyy') AS VCHDATE from costestimate a ,REELVCH B where TRIM(A.ICODE)||TRIM(A.COL6)=TRIM(B.ICODE)||TRIM(B.KCLREELNO) AND A.BRANCHCD='" + mbr + "' AND A.type='25' AND B.TYPE='02' and A.enqdt " + xprdrange + "";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq3);

                    mq4 = "SELECT DISTINCT TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IRATE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='02' AND VCHDATE BETWEEN TO_DATE('01/04/2017','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq4);

                    int count11 = 1;
                    DataRow dr1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dtm.NewRow();
                        db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0;
                        dr1["srno"] = count11;
                        dr1["JobCard_No"] = dt.Rows[i]["JOBNO"].ToString();
                        dr1["JobCard_Dt"] = dt.Rows[i]["JOBDT"].ToString();
                        dr1["Item_Code"] = dt.Rows[i]["ICODE"].ToString();
                        dr1["Item_Name"] = dt.Rows[i]["PRODUCT"].ToString();
                        dr1["Box_Wt"] = dt.Rows[i]["STD_WT_PERBOX_per_JC"].ToString().toDouble();
                        if (dt1.Rows.Count > 0)
                            dr1["Total_Qty"] = (fgen.seek_iname_dt(dt1, "ENQNO='" + dt.Rows[i]["JOBNO"].ToString() + "' AND ENQDT='" + dt.Rows[i]["JOBDT"].ToString() + "' AND ACODE='" + dt.Rows[i]["ICODE"].ToString() + "'", "QTY")).toDouble();

                        dt5 = new DataTable();
                        if (dt2.Rows.Count > 0)
                        {
                            DataView view1 = new DataView(dt2, "ENQNO='" + dt.Rows[i]["JOBNO"].ToString() + "' AND ENQDT='" + dt.Rows[i]["JOBDT"].ToString() + "' AND ACODE='" + dt.Rows[i]["ICODE"].ToString() + "'", "", DataViewRowState.CurrentRows);
                            dt5 = view1.ToTable(true, "icode");
                        }
                        for (int k = 0; k < dt5.Rows.Count; k++)
                        {
                            DataView view2 = new DataView(dt2, "icode='" + dt5.Rows[k]["icode"].ToString().Trim() + "' and ENQNO='" + dt.Rows[i]["JOBNO"].ToString() + "' AND ENQDT='" + dt.Rows[i]["JOBDT"].ToString() + "' AND ACODE='" + dt.Rows[i]["ICODE"].ToString() + "'", "", DataViewRowState.CurrentRows);
                            DataTable dt6 = new DataTable();
                            dt6 = view2.ToTable();
                            db10 = 0;
                            for (int l = 0; l < dt6.Rows.Count; l++)
                            {
                                db9 = fgen.make_double(dt6.Rows[l]["irate"].ToString());
                                if (db9 == 0)
                                {
                                    db11 = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dt6.Rows[l]["icode"].ToString().Trim() + "' and vchnum='" + dt6.Rows[l]["vchnum"].ToString().Trim() + "' and vchdate='" + dt6.Rows[l]["vchdate"].ToString().Trim() + "'", "irate"));
                                }
                                else
                                {
                                    db11 = db9;
                                }
                                db10 += fgen.make_double(dt6.Rows[l]["itate"].ToString());
                                db12 = db10 * db11;
                            }
                            db13 += db12;
                            db14 += db10;
                        }
                        db15 = db13 / db14;
                        dr1["Total_Paper_Cost"] = db13;
                        dr1["PaperCost_Per_KG"] = Math.Round(db15, 2);

                        dr1["Box_Cost_SP"] = fgen.make_double(dt.Rows[i]["SALERATE"].ToString());
                        dr1["Act_Conv_Cost"] = 7.85;

                        db1 = fgen.make_double(dr1["Box_Cost_SP"].ToString());
                        db2 = fgen.make_double(dr1["Total_Paper_Cost"].ToString());
                        db3 = fgen.make_double(dr1["Total_Qty"].ToString());
                        db5 = db3 * fgen.make_double(dr1["Box_Wt"].ToString());
                        db4 = db5 * fgen.make_double(dr1["Act_Conv_Cost"].ToString());
                        dr1["Avg_Conv_Cost"] = Math.Round(db4, 2);
                        db7 = db4 / db5;
                        dr1["JobCost_AsPer_SalePrice"] = db1 * db3;
                        dr1["PaperCost_Per_KG"] = dr1["PaperCost_Per_KG"].ToString().Replace("Infinity", "0").Replace("NaN", "0");
                        dr1["Total_Profit"] = (fgen.make_double(dr1["JobCost_AsPer_SalePrice"].ToString()) - db2 - fgen.make_double(dr1["Avg_Conv_Cost"].ToString())).toDouble(2);
                        dr1["Total_Profit"] = dr1["Total_Profit"].ToString().Replace("Infinity", "0").Replace("NaN", "0").toDouble(2);
                        dr1["Profit_Per_KG"] = Math.Round(fgen.make_double(dr1["Total_Profit"].ToString()) / db5, 2);
                        dr1["Profit_Per_KG"] = dr1["Profit_Per_KG"].ToString().Replace("Infinity", "0").Replace("NaN", "0").toDouble(2);
                        count11++;
                        dtm.Rows.Add(dr1);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dtm;
                    fgen.Fn_open_rptlevelJS("Job Card Wise Daily Profitability Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;
            }
        }
    }

    DataTable getChildItems(DataTable BomDt, string IBCODE, double lvl, DataTable cloneTable, string _newRow, string vcode)
    {
        DataTable dtC = new DataTable();
        dtC = cloneTable.Clone();
        dro = null;
        string newRow = "N";
        newRow = _newRow;
        string cond = " and VCODE='" + vcode + "'";
        if (vcode == "") cond = "";
        DataView dvC = new DataView(BomDt, "ICODE='" + IBCODE + "' AND LVL='" + lvl + "' " + cond + "", "LVL", DataViewRowState.CurrentRows);
        //if (dvC.Count > 0) newRow = "Y";
        if (dvC.Count > 0)
        {
            for (int l = 0; l < dvC.Count; l++)
            {
                lvl = fgen.make_double(dvC[l].Row["LVL"].ToString().Trim());

                ViewState["LVL" + lvl] = dvC[l].Row["LVL"].ToString().Trim();
                ViewState["PCODE" + lvl] = dvC[l].Row["icode"].ToString().Trim();
                ViewState["ICODE" + lvl] = dvC[l].Row["IBCODE"].ToString().Trim();
                ViewState["INAME" + lvl] = dvC[l].Row["IBNAME"].ToString().Trim();
                ViewState["partcode" + lvl] = dvC[l].Row["CPARTNO"].ToString().Trim();
                ViewState["QTY" + lvl] = dvC[l].Row["IBQTY"].ToString().Trim();
                ViewState["IBCODE" + lvl] = dvC[l].Row["IBCODE"].ToString().Trim();
                ViewState["UNIT" + lvl] = dvC[l].Row["UNIT"].ToString().Trim();

                string allow = "N";
                if (newRow != "N")
                {
                    if (lvl > 0)
                    {
                        try
                        {
                            if (ViewState["PCODE" + (lvl - 1)].ToString().Trim() == dvC[l].Row["VCODE"].ToString().Trim()) allow = "Y";
                            else allow = "N";
                        }
                        catch { allow = "Y"; }
                    }
                    else allow = "Y";
                    if (allow == "Y")
                    {
                        dro = dtC.NewRow();
                        if (lvl > 1)
                        {
                            double mlvl = lvl;
                            do
                            {
                                dro["lvl" + mlvl] = mlvl;
                                dro["pcode" + mlvl] = ViewState["PCODE" + mlvl].ToString();
                                dro["icode" + mlvl] = ViewState["ICODE" + mlvl].ToString();
                                dro["iname" + mlvl] = ViewState["INAME" + mlvl].ToString();
                                dro["partcode" + mlvl] = ViewState["partcode" + mlvl].ToString();
                                dro["qty" + mlvl] = ViewState["QTY" + mlvl].ToString();
                                dro["IBCODE"] = ViewState["IBCODE" + mlvl].ToString();
                                dro["UNIT"] = ViewState["UNIT" + mlvl].ToString();

                                mlvl -= 1;
                            } while (mlvl != 0);
                        }

                        dro["lvl" + lvl] = dvC[l].Row["LVL"].ToString().Trim();
                        dro["pcode" + lvl] = dvC[l].Row["icode"].ToString().Trim();
                        dro["icode" + lvl] = dvC[l].Row["ibcode"].ToString().Trim();
                        dro["iname" + lvl] = dvC[l].Row["ibname"].ToString().Trim();
                        dro["partcode" + lvl] = dvC[l].Row["cpartno"].ToString().Trim();
                        dro["qty" + lvl] = dvC[l].Row["ibqty"].ToString().Trim();
                        dro["IBCODE"] = dvC[l].Row["ibcode"].ToString().Trim();
                        dro["UNIT"] = dvC[l].Row["UNIT"].ToString().Trim();

                        dtC.Rows.Add(dro);

                        cloneTable.ImportRow(dro);
                    }
                }
                else
                {
                    int index = Convert.ToInt16(ViewState["CINDEX"]);
                    if (lvl > 1)
                    {
                        double mlvl = lvl;
                        do
                        {
                            cloneTable.Rows[index]["lvl" + mlvl] = mlvl;
                            cloneTable.Rows[index]["pcode" + mlvl] = ViewState["PCODE" + mlvl].ToString();
                            cloneTable.Rows[index]["icode" + mlvl] = ViewState["ICODE" + mlvl].ToString();
                            cloneTable.Rows[index]["iname" + mlvl] = ViewState["INAME" + mlvl].ToString();
                            cloneTable.Rows[index]["partcode" + mlvl] = ViewState["partcode" + mlvl].ToString();
                            cloneTable.Rows[index]["qty" + mlvl] = ViewState["QTY" + mlvl].ToString();
                            cloneTable.Rows[index]["IBCODE"] = ViewState["IBCODE" + mlvl].ToString();
                            cloneTable.Rows[index]["UNIT"] = ViewState["UNIT" + mlvl].ToString();

                            mlvl -= 1;
                        } while (mlvl != 0);
                    }

                    cloneTable.Rows[index]["lvl" + lvl] = dvC[l].Row["LVL"].ToString().Trim();
                    cloneTable.Rows[index]["pcode" + lvl] = dvC[l].Row["icode"].ToString().Trim();
                    cloneTable.Rows[index]["icode" + lvl] = dvC[l].Row["ibcode"].ToString().Trim();
                    cloneTable.Rows[index]["iname" + lvl] = dvC[l].Row["ibname"].ToString().Trim();
                    cloneTable.Rows[index]["partcode" + lvl] = dvC[l].Row["cpartno"].ToString().Trim();
                    cloneTable.Rows[index]["qty" + lvl] = dvC[l].Row["ibqty"].ToString().Trim();
                    cloneTable.Rows[index]["IBCODE"] = dvC[l].Row["ibcode"].ToString().Trim();
                    cloneTable.Rows[index]["UNIT"] = dvC[l].Row["UNIT"].ToString().Trim();
                }
                newRow = "Y";

                ViewState["CINDEX"] = cloneTable.Rows.Count - 1;
                lvl += 1;
                string sendVcode = "";
                if (lvl < 3)
                {
                    if (dvC[l].Row["vcode"].ToString().Trim().Substring(0, 1) != "*") sendVcode = dvC[l].Row["vcode"].ToString().Trim();
                }
                if (lvl > 2)
                {
                    if (dvC[l].Row["vcode"].ToString().Trim().Substring(0, 1) != "9") sendVcode = dvC[l].Row["icode"].ToString().Trim();
                    sendVcode = dvC[l].Row["icode"].ToString().Trim();
                }
                getChildItems(BomDt, dvC[l].Row["ibcode"].ToString().Trim(), lvl, cloneTable, "N", sendVcode);
            }
        }

        return dtC;
    }

    string get7seriesValues(string sfIcode, DataTable insvchDT, DataTable itemospDT, string maintdt, DataTable bomFilledTable)
    {
        string valueof7series = "";
        {
            mq5 = maintdt;
            int startingPoint = 0;
            string icode = sfIcode;
            int v = bomFilledTable.Rows.Count - 1;
            DataView vdview = new DataView(itemospDT, "icode='" + sfIcode + "'", "icode", DataViewRowState.CurrentRows);
            if (vdview.Count > 0)
            {
                for (int i = v; i < bomFilledTable.Rows.Count; i++)
                {
                    i0 = 1;
                    {
                        if (startingPoint != 0)
                        {
                            icode = bomFilledTable.Rows[i]["ibcode"].ToString().Trim();
                        }
                        startingPoint = 1;
                        vdview = new DataView(itemospDT, "icode='" + icode + "'", "icode", DataViewRowState.CurrentRows);

                        DataView vdview1 = new DataView(bomFilledTable, "icode='" + bomFilledTable.Rows[i]["icode"].ToString().Trim() + "' and ibcode='" + icode + "' and ibqty='" + bomFilledTable.Rows[i]["ibqty"] + "'", "ibcode", DataViewRowState.CurrentRows);
                        if (vdview1.Count <= 0) vdview1 = new DataView(bomFilledTable, "ibcode='" + icode + "'", "ibcode", DataViewRowState.CurrentRows);
                        double mainIqty = fgen.make_double(vdview1[0].Row["ibqty"].ToString(), 6);
                        double lotSize = 1;
                        for (int x = 0; x < vdview.Count; x++)
                        {
                            if (mq0 != vdview[x].Row["icode"].ToString().Trim()) i0 += 1;
                            dro = bomFilledTable.NewRow();
                            dro["lvl"] = 2;
                            dro["icode"] = vdview[x].Row["icode"].ToString().Trim();
                            dro["branchcd"] = vdview[x].Row["branchcd"].ToString().Trim();
                            mq0 = vdview[x].Row["icode"].ToString().Trim();
                            lotSize = fgen.make_double(vdview[x].Row["MAIN_ISSUE_NO"].ToString().Trim());
                            if (lotSize <= 0) lotSize = 1;
                            if (co_cd == "NEOP") lotSize = 100;
                            //mainIqty = 1;
                            dro["ibqty"] = Math.Round(fgen.make_double(vdview[x].Row["ibqty"].ToString(), 6) * (mainIqty / lotSize), 6) * fgen.make_double(mq5, 6);
                            dro["ibcode"] = vdview[x].Row["ibcode"].ToString().Trim();
                            //dro["irate"] = vdview[0].Row["bchrate"];
                            dro["irate"] = vdview[x].Row["bchrate"];

                            dro["val"] = "0";
                            if (bomFilledTable.Rows[i]["lvl"].ToString() == "1")
                            {
                                mq7 = "";
                                dro["pcode"] = bomFilledTable.Rows[i]["icode"].ToString().Trim();
                                mq7 = bomFilledTable.Rows[i]["icode"].ToString().Trim();
                            }
                            else dro["pcode"] = mq7;
                            v++;

                            bomFilledTable.Rows.Add(dro);
                        } vdview1.Dispose();
                    } vdview.Dispose();
                }
                icode = "";
            }
        }

        for (int i = 0; i < bomFilledTable.Rows.Count; i++)
        {
            DataView vdview = new DataView(bomFilledTable, "branchcd='" + bomFilledTable.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + bomFilledTable.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
            if (vdview.Count <= 0)
            {
                if (co_cd != "KCLG")
                {
                    DataView sort_view = new DataView(dt2, "branchcd='" + bomFilledTable.Rows[i]["branchcd"].ToString().Trim() + "' and trim(icode)='" + bomFilledTable.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                    if (sort_view.Count > 0) bomFilledTable.Rows[i]["irate"] = fgen.make_double(sort_view[0].Row["rate"].ToString().Trim(), 3);
                    else
                    {
                        sort_view = new DataView(dt2, "trim(icode)='" + bomFilledTable.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                        if (sort_view.Count > 0) bomFilledTable.Rows[i]["irate"] = fgen.make_double(sort_view[0].Row["rate"].ToString().Trim(), 3);
                    }
                }
            }
            else bomFilledTable.Rows[i]["irate"] = "0";
            vdview.Dispose();
            bomFilledTable.Rows[i]["val"] = fgen.make_double(fgen.make_double(bomFilledTable.Rows[i]["ibqty"].ToString()) * fgen.make_double(bomFilledTable.Rows[i]["irate"].ToString()), 2);
        }
        return valueof7series;
    }

    double getBOMVal(string sfIcode, string PCODE)
    {
        double db6g = 0;
        double bomVal = 0;
        i0 = 1;
        int v = 0;
        mq5 = fgen.seek_iname_dt(insvchDT, "ICODE='" + sfIcode + "'", "maintdt");
        if (fgen.make_double(mq5) <= 0) mq5 = "1";

        DataTable childItemBom = new DataTable();

        childItemBom.Columns.Add(new DataColumn("lvl", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("icode", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("pcode", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("spcode", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("ibqty", typeof(double)));
        childItemBom.Columns.Add(new DataColumn("ibcode", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("irate", typeof(double)));
        childItemBom.Columns.Add(new DataColumn("val", typeof(double)));

        childItemBom.Columns.Add(new DataColumn("grwt", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("ntwt", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("lot_size", typeof(string)));

        childItemBom.Columns.Add(new DataColumn("branchcd", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("vchnum", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("vchdate", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("acode", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("TYPE", typeof(string)));
        childItemBom.Columns.Add(new DataColumn("msrno", typeof(double)));

        DataView childView = new DataView(itemospDT, "icode='" + sfIcode + "'", "icode,ibcode", DataViewRowState.CurrentRows);
        DataRow childRow = null;
        double mainLotSize = 0;
        for (int i = 0; i < childView.Count; i++)
        {
            childRow = childItemBom.NewRow();
            childRow["lvl"] = "2";
            childRow["icode"] = childView[i].Row["icode"].ToString().Trim();
            childRow["pcode"] = PCODE;
            childRow["spcode"] = spcode;
            mainLotSize = fgen.make_double(childView[i].Row["main_issue_no"].ToString().Trim());
            if (co_cd == "NEOP") mainLotSize = 1;
            if (co_cd == "MINV")
            {
                mainLotSize = fgen.make_double(childView[i].Row["IBWT"].ToString().Trim());
                if (mainLotSize <= 0) mainLotSize = 100;
            }
            if (mainLotSize <= 0) mainLotSize = 1;
            childRow["ibqty"] = fgen.make_double(fgen.make_double(childView[i].Row["ibqty"].ToString(), 6) / mainLotSize, 6);
            childRow["ibcode"] = childView[i].Row["ibcode"].ToString().Trim();
            childRow["irate"] = childView[i].Row["bchrate"];

            childRow["GRWT"] = childView[i].Row["IBDIEPC"].ToString().Trim();
            childRow["NTWT"] = childView[i].Row["SUB_ISSUE_NO"].ToString().Trim();

            //childRow["lot_size"] = mainLotSize;
            childRow["val"] = 0;
            childRow["msrno"] = msrno;
            msrno++;
            childItemBom.Rows.Add(childRow);
        }
        v = 0;
        i0 = 1;
        for (int i = v; i < childItemBom.Rows.Count; i++)
        {
            if (childItemBom.Rows[i]["ibcode"].ToString().Trim() == "71010057")
            {

            }
            DataView vdview = new DataView(itemospDT, "icode='" + childItemBom.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
            if (vdview.Count > 0)
            {
                DataView vdview1 = new DataView(childItemBom, "icode='" + childItemBom.Rows[i]["icode"].ToString().Trim() + "' and ibcode='" + childItemBom.Rows[i]["ibcode"].ToString().Trim() + "' and ibqty='" + childItemBom.Rows[i]["ibqty"] + "'", "ibcode", DataViewRowState.CurrentRows);
                if (vdview1.Count <= 0) vdview1 = new DataView(childItemBom, "ibcode='" + childItemBom.Rows[i]["ibcode"].ToString().Trim() + "'", "ibcode", DataViewRowState.CurrentRows);

                for (int x = 0; x < vdview.Count; x++)
                {
                    if (mq0 != vdview[x].Row["icode"].ToString().Trim())
                    {
                        value3 = fgen.seek_iname_dt(childItemBom, "IBCODE='" + vdview[x].Row["icode"].ToString().Trim() + "'", "LVL", "LVL DESC");
                        if (value3 == "0")
                            i0 += 1;
                        else i0 = fgen.make_int(value3) + 1;
                    }
                    dro = childItemBom.NewRow();
                    dro["lvl"] = i0.ToString();
                    dro["icode"] = vdview[x].Row["icode"].ToString().Trim();
                    mq0 = vdview[x].Row["icode"].ToString().Trim();
                    dro["spcode"] = spcode;
                    double lotSize = fgen.make_double(vdview[x].Row["MAIN_ISSUE_NO"].ToString().Trim());
                    if (co_cd == "MINV")
                    {
                        lotSize = fgen.make_double(vdview[x].Row["IBWT"].ToString().Trim());
                        if (lotSize <= 0) lotSize = 100;
                    }
                    if (lotSize <= 0) lotSize = 1;
                    dro["ibqty"] = Math.Round(Convert.ToDouble(vdview[x].Row["ibqty"]) * (Convert.ToDouble(vdview1[0].Row["ibqty"]) / lotSize), 6).ToString();
                    dro["ibcode"] = vdview[x].Row["ibcode"].ToString().Trim();
                    dro["irate"] = vdview[x].Row["bchrate"];
                    dro["val"] = "0";

                    dro["GRWT"] = vdview[x].Row["IBDIEPC"].ToString().Trim();
                    dro["NTWT"] = vdview[x].Row["SUB_ISSUE_NO"].ToString().Trim();
                    //dro["lot_size"] = lotSize;

                    if (childItemBom.Rows[i]["lvl"].ToString() == "1")
                    {
                        mq7 = "";
                        mq7 = childItemBom.Rows[i]["icode"].ToString().Trim();
                    }
                    dro["pcode"] = PCODE;
                    dro["msrno"] = msrno;
                    msrno++;
                    v++;

                    childItemBom.Rows.Add(dro);
                } vdview1.Dispose();
            } vdview.Dispose();
        }

        value1 = "";
        if (childItemBom.Rows.Count > 0)
        {
            for (int i = 0; i < childItemBom.Rows.Count; i++)
            {
                if (childItemBom.Rows[i]["ibcode"].ToString().Trim() == "10480031")
                {

                }

                DataView vdview = new DataView(childItemBom, "icode='" + childItemBom.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                if (vdview.Count <= 0)
                {
                    if (co_cd != "KCLG")
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            DataView sort_view = new DataView(dt2, "trim(icode)='" + childItemBom.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                            if (sort_view.Count > 0)
                            {
                                childItemBom.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                            }
                            else
                            {
                                sort_view = new DataView(dt2, "trim(icode)='" + childItemBom.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                if (sort_view.Count > 0)
                                {
                                    childItemBom.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                }
                            }
                            if (sort_view.Count > 0)
                            {
                                childItemBom.Rows[i]["BRANCHCD"] = sort_view[0].Row["BRANCHCD"].ToString().Trim();
                                childItemBom.Rows[i]["VCHNUM"] = sort_view[0].Row["VCHNUM"].ToString().Trim();
                                childItemBom.Rows[i]["VCHDATE"] = sort_view[0].Row["VCHDATE"].ToString().Trim();
                                childItemBom.Rows[i]["ACODE"] = sort_view[0].Row["ACODE"].ToString().Trim();
                                childItemBom.Rows[i]["type"] = sort_view[0].Row["type"].ToString().Trim();
                            }
                        }
                    }
                }
                else childItemBom.Rows[i]["irate"] = "0";
                vdview.Dispose();
                childItemBom.Rows[i]["val"] = fgen.make_double(Convert.ToDouble(childItemBom.Rows[i]["ibqty"]) * Convert.ToDouble(childItemBom.Rows[i]["irate"]), 5);
            }
        }
        mq0 = "0";
        DataRow dr = null;
        double mul_fact = 0;
        for (int i = 0; i < childItemBom.Rows.Count; i++)
        {
            if (Convert.ToDouble(mq0) > 0) mq0 = Math.Round(Convert.ToDouble(mq0) + Convert.ToDouble(childItemBom.Rows[i]["val"].ToString().Trim()), 5).ToString();
            else mq0 = childItemBom.Rows[i]["val"].ToString().Trim();

            dr = fullDt.NewRow();
            dr["srno"] = runningNo;
            dr["sort"] = "C";
            dr["lvl"] = childItemBom.Rows[i]["lvl"].ToString().Trim();
            dr["pcode"] = PCODE;
            dr["icode"] = childItemBom.Rows[i]["icode"].ToString().Trim();
            dr["ibqty"] = childItemBom.Rows[i]["ibqty"].ToString().Trim();
            dr["ibcode"] = childItemBom.Rows[i]["ibcode"].ToString().Trim();
            dr["spcode"] = childItemBom.Rows[i]["spcode"].ToString().Trim();
            dr["irate"] = childItemBom.Rows[i]["irate"].ToString().Trim();
            dr["val"] = childItemBom.Rows[i]["val"].ToString().Trim();

            dr["GRWT"] = childItemBom.Rows[i]["GRWT"].ToString().Trim();
            dr["NTWT"] = childItemBom.Rows[i]["NTWT"].ToString().Trim();

            //dr["lot_size"] = childItemBom.Rows[i]["lot_size"].ToString().Trim();

            dr["BRANCHCD"] = childItemBom.Rows[i]["BRANCHCD"].ToString().Trim();
            dr["VCHNUM"] = childItemBom.Rows[i]["VCHNUM"].ToString().Trim();
            dr["VCHDATE"] = childItemBom.Rows[i]["VCHDATE"].ToString().Trim();
            dr["ACODE"] = childItemBom.Rows[i]["ACODE"].ToString().Trim();
            dr["TYPE"] = childItemBom.Rows[i]["TYPE"].ToString().Trim();

            dr["msrno"] = childItemBom.Rows[i]["msrno"];

            if (co_cd == "DREM")
            {
                if (bomanx.Rows.Count > 0)
                {
                    mul_fact = 0;
                    DataView vdview = new DataView(bomanx, "ICODE='" + childItemBom.Rows[i]["IBCODE"].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                    for (int ki = 0; ki < vdview.Count; ki++)
                    {
                        if (ViewState["MAINLOTSIZE"] != null)
                        {
                            if (ViewState["MAINLOTSIZE"].ToString().toDouble() > 0)
                                mainLotSize = ViewState["MAINLOTSIZE"].ToString().toDouble();
                        }
                        if (mainLotSize > 0)
                            mul_fact = fgen.make_double(childItemBom.Rows[ki]["IBQTY"].ToString().Trim(), 0);
                        if (mul_fact < 1) mul_fact = 1;

                        db5 = Math.Round(((fgen.make_double(vdview[ki].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact), 5);
                        dr["PCOST"] = fgen.make_double((db5 / mainLotSize) + dr["PCOST"].ToString().toDouble(), 5);
                        db6g += db5;
                    }
                }
            }

            fullDt.Rows.Add(dr);
        }
        bomVal = fgen.make_double((fgen.make_double(mq0)) * fgen.make_double(mq5), 5);
        if (co_cd == "MINV") bomVal = fgen.make_double((fgen.make_double(mq0)) * fgen.make_double(mq5), 2);
        db6g += ViewState["db6"].ToString().toDouble();
        ViewState["db6"] = db6g.ToString();
        return bomVal;
    }

    public void RowtoColumnData()
    {
        SQuery = SQuery + " from " + frm_prodsheet + " a where a.vchdate " + xprdrange + " and a.branchcd <> 'DD' and a.type in ('86','88') ";

        if (co_cd == "HPPI" || co_cd == "SPPI")
        {
            string ru = "N";
            HCID = hfhcid.Value.Trim();
            switch (HCID)
            {
                case "F05292":
                    ru = "Y";
                    SQuery = "SELECT COL1 AS MONTH_NAME,SUM(IS_NUMBER(COL3)) AS TOT_BAS FROM " + frm_inspvch + " WHERE BRANCHCD='" + mbr + "' AND TYPE='55' AND VCHDATE " + xprdrange + " GROUP BY COL1 having SUM(IS_NUMBER(COL3))>0";
                    break;
                case "F05289":
                    ru = "Y";
                    SQuery = "SELECT COL1 AS MONTH_NAME,SUM(IS_NUMBER(COL3)) AS TOT_BAS FROM " + frm_inspvch + " WHERE BRANCHCD='" + mbr + "' AND TYPE='45' AND VCHDATE " + xprdrange + " GROUP BY COL1 having SUM(IS_NUMBER(COL3))>0";
                    break;
            }
            if (ru == "Y")
            {
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, co_cd, SQuery);
            }
            return;
        }

        {

            dt1 = new DataTable();
            dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);

            dt2 = new DataTable();
            dt = new DataTable();
            if (frm_formID == "F05292")
                SQuery = "select * from(Select TYPE1,name from type where id='4'  order by type1) where rownum<13";
            else if (frm_formID == "F05289")
                SQuery = "select * from(Select TYPE1,name from type where id='8'  order by type1) where rownum<13";
            dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);

            dt.Columns.Add(new DataColumn("month_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("TOT_BAS", typeof(Decimal)));

            int jq = 0;
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                try
                {
                    if (fgen.make_double(dt1.Rows[0][jq].ToString().Trim()) <= 0) { }
                    else
                    {
                        DataRow nrow = dt.NewRow();
                        nrow["month_Name"] = dt2.Rows[jq]["name"].ToString().Trim();
                        nrow["tot_bas"] = dt1.Rows[0][jq].ToString().Trim();
                        dt.Rows.Add(nrow);

                    }
                    jq = jq + 1;
                }
                catch { }
            }
        }
    }

    public void GetPPMonth()
    {
        xday = ""; xmonth = ""; xyear = ""; xselected_date = "";
        xday = sysdt.Substring(0, 2);
        xday = "01";
        xmonth = sysdt.Substring(3, 2);
        xyear = sysdt.Substring(6, 4);
        xselected_date = xday + "/" + xmonth + "/" + xyear;
        DateTime mdate = DateTime.ParseExact(xselected_date, "dd/MM/yyyy", null);
        xselected_date = mdate.AddDays(-1).ToString("dd/MM/yyyy");
        pmdate = xselected_date.Substring(0, 10);
        xday = ""; xmonth = ""; xyear = ""; xselected_date = "";
        xday = pmdate.Substring(0, 2);
        xday = "01";
        xmonth = pmdate.Substring(3, 2);
        xyear = pmdate.Substring(6, 4);
        xselected_date = xday + "/" + xmonth + "/" + xyear;
        mdate = DateTime.ParseExact(xselected_date, "dd/MM/yyyy", null);
        xselected_date = mdate.AddDays(-1).ToString("dd/MM/yyyy");
        ppmdate = xselected_date.Substring(0, 10);
        f1 = ""; f2 = "";
        f1 = pmdate.Substring(3, 2);
        f2 = pmdate.Substring(6, 4);
        pmdate = f2 + f1;
        er1 = f1 + f2;
        f1 = ""; f2 = "";
        f1 = ppmdate.Substring(3, 2);
        f2 = ppmdate.Substring(6, 4);
        ppmdate = f2 + f1;
        er2 = f1 + f2;
    }

    public void previousdate()
    {
        xday = sysdt.Substring(0, 2);
        xday = "01";
        xmonth = sysdt.Substring(3, 2);
        xyear = sysdt.Substring(6, 4);
        xselected_date = xday + "/" + xmonth + "/" + xyear;
        DateTime dt1 = DateTime.ParseExact(xselected_date, "dd/MM/yyyy", null);
        xselected_date = dt1.AddDays(-1).ToString("dd/MM/yyyy");
        todt = xselected_date.Substring(0, 10);
    }

    public string DataTableToJSArray_Top10(DataTable dt, string mode)
    {
        // WHEN WE HAVE TO SHOW DATA OF ANY TOP 10 
        double icount = 0;
        string rowDataStr = "", colStr = "";
        StringBuilder sb = new StringBuilder();
        sb.Append("[");

        if (dt.Rows.Count > 0)
        {
            //add header
            colStr = "";
            icount = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                if (colStr.Length > 0)
                    colStr += ",";
                switch (mode)
                {
                    case "F05256":
                        dc.ColumnName = "% Mthly Avg";
                        break;
                    case "F05259":
                        dc.ColumnName = "% Mth Avg";
                        break;
                    case "F05262":
                        dc.ColumnName = "% Yr.Target";
                        break;
                }
                colStr += "'" + dc.ColumnName + "'";
            }
            sb.Append("[" + colStr + "]");
            icount = 0;
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                DataRow dr = dt.Rows[count];
                rowDataStr = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (rowDataStr.Length > 0)
                        rowDataStr += ",";
                    if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                    {
                        if (count > 9)
                        {
                            icount = icount + fgen.make_double(dr[dc].ToString().Trim());
                        }
                        else
                            rowDataStr += dr[dc].ToString().Trim();
                    }
                    else
                    {
                        if (count > 9) { }
                        else rowDataStr += "'" + dr[dc].ToString().Trim() + "'";
                    }

                    rowDataStr = rowDataStr.Replace("\r\n", "").Trim();
                }
                if (icount > 0) { }
                else
                {
                    sb.Append(",");
                    sb.Append("[" + rowDataStr + "]");
                }
            }
        }
        if (icount == 0)
        {
            sb.Append("]");
        }
        else
        {
            rowDataStr = "";
            rowDataStr += "'OTHERS'";
            rowDataStr += ",";
            rowDataStr += icount.ToString();
            sb.Append(",");
            sb.Append("[" + rowDataStr + "]");
            sb.Append("]");
        }
        return sb.ToString();
    }

    public string DataTableToJSArray(DataTable dt, string mode)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        if (dt.Rows.Count > 0)
        {
            //add header
            string colStr = "";
            foreach (DataColumn dc in dt.Columns)
            {
                if (colStr.Length > 0)
                    colStr += ",";
                switch (mode)
                {
                    case "F05256":
                        dc.ColumnName = "% Mthly Avg";
                        break;
                    case "F05259":
                        dc.ColumnName = "% Mth Avg";
                        break;
                    case "F05262":
                        dc.ColumnName = "% Yr.Target";
                        break;
                }
                colStr += "'" + dc.ColumnName + "'";
            }
            sb.Append("[" + colStr + "]");

            // Add records
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                DataRow dr = dt.Rows[count];
                string rowDataStr = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (rowDataStr.Length > 0)
                        rowDataStr += ",";
                    if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                        rowDataStr += dr[dc].ToString().Trim();
                    else
                        rowDataStr += "'" + dr[dc].ToString().Trim() + "'";

                    rowDataStr = rowDataStr.Replace("\r\n", "").Trim();
                }

                sb.Append(",");
                sb.Append("[" + rowDataStr + "]");
            }
        }
        sb.Append("]");
        return sb.ToString();
    }

    public void OpenChart(string chartname, string chart_title)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        switch (chartname)
        {
            case "Gauge":
                sb.Append(@"google.load('visualization', '1', { packages: ['gauge'] });");
                sb.Append(@"var options;");
                break;
            default:
                if (chartname == "Bar" || chartname == "Pie")
                    sb.Append(@"google.load('visualization', '1', { packages: ['corechart'] });");
                break;
        }
        sb.Append(@"google.setOnLoadCallback(drawChart);");
        sb.Append(@"function drawChart() {");
        var data = "google.visualization.arrayToDataTable(" + hdnChartData.Value + ",false);";
        sb.Append(@"data =" + data.ToString());
        switch (chartname)
        {
            case "Pie":
                // sb.Append(@"var options = {'title': '" + chart_title + "','width': 1000,'height': 800,titleFontSize:20,titleTextStyle:{ bold : false },legend: {position: 'labeled'}");
                sb.Append(@"var options = {'title': '" + chart_title + "',titleFontSize:20,titleTextStyle:{ bold : false },legend: {position: 'labeled'}");
                switch (frm_formID)
                {
                    case "":
                        break;

                    default:
                        if (frm_formID == "F05278")
                            sb.Append(@",is3D: true,");
                        else if (frm_formID == "F05281")
                            sb.Append(@",pieHole: 0.4,");
                        else if (frm_formID == "F05264" || frm_formID == "F05301" || frm_formID == "F05304" || frm_formID == "F05307" || frm_formID == "F05310")
                        {
                            sb.Append(@",slices: { ");
                            sb.Append(@"1: {offset: 0.1},");
                            sb.Append(@"4: {offset: 0.2},");
                            sb.Append(@"6: {offset: 0.3},");
                            sb.Append(@"8: {offset: 0.2},");
                            sb.Append(@"10: {offset: 0.1},");
                            sb.Append(@"12: {offset: 0.1},");
                            sb.Append(@"14: {offset: 0.2},");
                            sb.Append(@"15: {offset: 0.1},");
                            sb.Append(@"20: {offset: 0.2},");
                            sb.Append(@"30: {offset: 0.1},");
                            sb.Append(@"},");
                        }
                        break;
                }
                sb.Append(@"};");
                sb.Append(@"chart = new google.visualization.PieChart(document.getElementById('chart'));");
                break;

            case "Bar":
                //sb.Append(@"var options = {'title': '" + chart_title + "','width': 1000,'height': 800, titleFontSize:20,titleTextStyle:{ bold : false },");
                sb.Append(@"var options = {'title': '" + chart_title + "',titleFontSize:20,titleTextStyle:{ bold : false },");
                sb.Append(@"hAxis: { title: '" + hdnHAxisTitle_Bar.Value + "', titleTextStyle: { color: 'red' } },");

                switch (frm_formID)
                {
                    case "F05338":
                        sb.Append(@"colors: ['6B8E23','FCB441'], ");
                        sb.Append(@"isStacked: true,");
                        break;
                    case "GP22":
                        sb.Append(@"chart = new google.visualization.AreaChart(document.getElementById('chart'));");
                        break;
                    case "F05238":
                        sb.Append(@"colors: ['000080','32CD32'], ");
                        break;
                    case "F05232":
                        sb.Append(@"colors: ['056492','DF3A02'], ");
                        break;
                    case "F05241":
                        sb.Append(@"colors: ['000080','FF0000'], ");
                        break;
                    default:
                        break;
                }
                sb.Append(@"};");
                switch (frm_formID)
                {
                    case "F05338":
                        sb.Append(@"chart = new google.visualization.ColumnChart(document.getElementById('chart'));");
                        break;
                    default:
                        if (frm_formID == "F05241" || frm_formID == "F05238" || frm_formID == "F05232" || frm_formID == "F05229")
                            sb.Append(@"chart = new google.visualization.ColumnChart(document.getElementById('chart'));");
                        else if (frm_formID == "F05276" || frm_formID == "F05287")
                            sb.Append(@"chart = new google.visualization.SteppedAreaChart(document.getElementById('chart'));");
                        break;
                }
                break;

            case "Gauge":
                sb.Append(@"options = {");
                sb.Append(@"width: 400, height: 400,");
                sb.Append(@"greenFrom: 75,greenTo: 100,");
                sb.Append(@"redFrom: 0, redTo: 25,");
                sb.Append(@"yellowFrom:25, yellowTo: 75,");
                sb.Append(@"minorTicks: 5");
                sb.Append(@"};");
                sb.Append(@"chart = new google.visualization.Gauge(document.getElementById('chart'));");
                break;
        }
        sb.Append(@"chart.draw(data, options);");
        sb.Append(@"}");
        if (chartname == "Gauge")
        {
            sb.Append(@"function changeTemp(dir) {");
            sb.Append(@"data.setValue(0, 0, data.getValue(0, 0) + dir * 10);");
            sb.Append(@"chart.draw(data, options);");
            sb.Append(@"}");
            div4.Visible = true;
            g1.Visible = true;
            g2.Visible = true;
        }
        sb.Append(@"</script>");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);
    }

    public void OpenChartColumn(string chartname, string chart_title)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"function drawChart() {");
        var data = "google.visualization.arrayToDataTable(" + hdnChartData.Value + ",false);";
        sb.Append(@"data =" + data.ToString());
        sb.Append(@"var group = google.visualization.data.group(data, [0], []);");
        sb.Append(@"var columns = [0];");
        sb.Append(@"for (var i = 0; i < group.getNumberOfRows(); i++) {");
        sb.Append(@"var label = group.getValue(i, 0);");
        sb.Append(@"columns.push({");
        sb.Append(@"type: 'number',");
        sb.Append(@"calc: (function (name) {");
        sb.Append(@"return function (dt, row) {");
        sb.Append(@"return (dt.getValue(row, 0) == name) ? dt.getValue(row, 1) : null;");
        sb.Append(@"}");
        sb.Append(@"})(label)");
        sb.Append(@"});");
        sb.Append(@"}");
        sb.Append(@"var chart = new google.visualization.ChartWrapper({");
        sb.Append(@"chartType: 'ColumnChart',");
        sb.Append(@"containerId: 'chart',");
        sb.Append(@"dataTable: data,");
        sb.Append(@"options: {");
        sb.Append(@"'is3D': true,");
        sb.Append(@"'isStacked': true,");
        sb.Append(@"'legend':'none',");
        sb.Append(@" 'title' :'" + chart_title + "', titleFontSize:20,titleTextStyle:{ bold : true }, ");
        sb.Append(@"'hAxis': { title: '" + hdnHAxisTitle_Bar.Value + "', titleTextStyle: { color: 'red' } }");
        if (frm_formID == "F05332" || frm_formID == "F05335")
        {
            sb.Append(@",'vAxis': { title: '" + hdnVAxisTitle_Bar.Value + "', titleTextStyle: { color: 'red' } }");
        }
        sb.Append(@"},");
        sb.Append(@"view: {");
        sb.Append(@"columns: columns");
        sb.Append(@"}");
        sb.Append(@"});");
        sb.Append(@"chart.draw();");
        sb.Append(@"}");
        sb.Append(@"google.setOnLoadCallback(drawChart);");
        sb.Append(@"google.load('visualization', '1', {packages: ['corechart']});");
        sb.Append(@"</script>");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);
    }

    string getNrate(DataTable dtTable, string pCode, string icodeVar)
    {
        string val = "";
        string mhd = fgen.seek_iname_dt(dtTable, "PCODE='" + pCode + "' and ICODE='" + icodeVar + "'", "IBCODE");
        if (mhd.Substring(0, 1) == "7")
            val = getNrate(dtTable, pCode, mhd.Trim());
        else val = fgen.seek_iname_dt(dtTable, "PCODE='" + pCode + "' and IBCODE='" + mhd + "'", "irate");

        return val;
    }

    string getVal(DataTable dtTable, string pCode, string iCode, string icodeVar)
    {
        string val = "";
        string mhd = "";
        cond = "";
        if (spcode != "") cond = " AND SPCODE='" + spcode + "'";
        if (icodeVar.Substring(0, 1) == "7")
        {
            val = dtTable.Compute("SUM(VAL)", "PCODE='" + pCode + "' and iCODE='" + icodeVar + "' " + cond + " ").ToString().toDouble(4).ToString();
        }
        if (val.toDouble() <= 0)
        {
            mhd = fgen.seek_iname_dt(dtTable, "PCODE='" + pCode + "' and ICODE='" + icodeVar + "' " + cond + " ", "IBCODE");
            if (mhd.Substring(0, 1) == "7")
                //val = getVal(dtTable, pCode, mhd.Trim());
                val = dtTable.Compute("SUM(VAL)", "PCODE='" + pCode + "' and iCODE='" + mhd + "' " + cond + " ").ToString().toDouble(4).ToString();
            else if (mhd == "0") val = dtTable.Compute("SUM(VAL)", "PCODE='" + pCode + "' and ICODE='" + iCode + "' and IBCODE='" + icodeVar + "' " + cond + " ").ToString().toDouble(4).ToString();
            //else val = dtTable.Compute("SUM(VAL)", "PCODE='" + pCode + "' and IBCODE='" + mhd + "'").ToString();
            //DataView ddd = new DataView(dtTable, "PCODE='" + pCode + "' and IBCODE='" + icodeVar + "'", "", DataViewRowState.CurrentRows);
        }

        return val;
    }
    int m = 1;
    string fillBomLevelWise(string pCode, string ibCd)
    {
        spcode = "";
        dv = new DataView(fullDt, "PCODE='" + pCode + "' AND LVL IN ('0','1')", "PCODE,LVL,ICODE", DataViewRowState.CurrentRows);
        if (dv.Count > 0)
        {
            for (int i = 0; i < dv.Count; i++)
            {
                drDtSort = dtSort.NewRow();
                for (int c = 0; c < dtSort.Columns.Count; c++)
                    drDtSort[dtSort.Columns[c].ColumnName] = dv[i][dtSort.Columns[c].ColumnName];
                dtSort.Rows.Add(drDtSort);
                run2(pCode, dv[i]["IBCODE"].ToString().Trim());
            }
        }
        return "";
    }
    void run2(string pCode, string bcod)
    {
        cond = "";
        if (spcode != "") cond = " AND SPCODE='" + spcode + "'";
        DataView cdv = new DataView(fullDt, "PCODE='" + pCode + "' AND ICODE='" + bcod + "' " + cond + " ", "", DataViewRowState.CurrentRows);
        if (cdv.Count > 0)
        {
            for (int y = 0; y < cdv.Count; y++)
            {
                drDtSort = dtSort.NewRow();
                for (int c = 0; c < dtSort.Columns.Count; c++)
                    drDtSort[dtSort.Columns[c].ColumnName] = cdv[y][dtSort.Columns[c].ColumnName];
                dtSort.Rows.Add(drDtSort);

                bcod = cdv[y]["ibcode"].ToString().Trim();
                DataView cdv1 = new DataView(fullDt, "PCODE='" + pCode + "' AND ICODE='" + bcod + "' ", "", DataViewRowState.CurrentRows);
                spcode = "";
                if (cdv1.Count > 0)
                {
                    spcode = cdv[y]["spcode"].ToString().Trim();
                    run2(pCode, bcod);
                }
                m++;
            }
        }
    }
}