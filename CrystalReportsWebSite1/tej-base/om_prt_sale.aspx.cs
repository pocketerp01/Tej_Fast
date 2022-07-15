using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_sale : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7D, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID;
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

                case "F50141":
                case "F50142":
                case "F50143":
                case "F50271":
                    SQuery = "select trim(type1) as fstr,name,type1 as code from type where ID='V'  AND type1 like '4%' AND TYPE1 NOT IN ('4F','47') ORDER BY code";
                    header_n = "Select Sale Type";
                    break;

                case "F50144":
                    SQuery = "SELECT  TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')  AS FSTR, A.VCHNUM AS INVOICE_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INVOICE_DT,B.ANAME AS CUSTOMER,A.ACODE AS CCUSTOMER_CODE,A.BILL_TOT,A.TYPE FROM SALEP A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '4%'  AND a.type not in ('4F','47') and A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM DESC";
                    header_n = "Select Entry";
                    break;

                case "F50222":
                case "F50223":
                case "F50224":
                case "F50228":
                case "F50240":
                case "F50241":
                case "F50313":
                case "F50311":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F50266":
                case "F50267":
                case "F50268":
                    SQuery = "select distinct a.acode as fstr,a.acode,b.aname as party from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type='4F' and a.vchdate " + xprdrange + "";
                    i0 = 1;
                    header_n = "Select Party";
                    break;

                case "F50312":
                case "F49212":
                    SQuery = "SELECT type1 as fstr, NAME,TYPE1 as code FROM TYPE WHERE ID='V' AND TYPE1 LIKE '4%' ORDER BY TYPE1";
                    header_n = "Select Type";
                    i0 = 1;
                    break;

                case "F50316":
                    SQuery = "SELECT name AS FSTR,TYPE1,NAME FROM TYPEGRP WHERE ID='CN' ORDER BY type1";
                    header_n = "Select Country";
                    break;

                case "MTH_WEEK":
                    SQuery = "SELECT MTHNUM AS FSTR,MTHNUM,MTHNAME AS MONTH FROM MTHS";
                    header_n = "Select Type";
                    break;

                case "SALE_REJ": //sale & rejection====s/m module==mktg reports
                    fgen.Fn_open_dtbox("-", frm_qstr);
                    break;

                case "TARIF_BILL": //all tarrif bills details
                    SQuery = "SELECT type1 as fstr, NAME,TYPE1 as code FROM TYPE WHERE ID='V' AND TYPE1 LIKE '4%' ORDER BY TYPE1";
                    header_n = "Select Type"; //open mseek
                    i0 = 1;
                    break;

                case "F50386": //ITEM WISE WISE
                case "F50388"://SUBGROUP WISE
                case "F50390"://MAIN GROUP WISE                         
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F50321"://WPPL REPORT
                    SQuery = "SELECT DISTINCT trim(A.ACODE) AS FSTR,trim(A.ACODE) AS CODE,trim(B.ANAME) AS CUSTOMER FROM SOMAS A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd='" + mbr + "' and substr(trim(a.type),1,1)='4' and a.type!='47' and a.orddt " + xprdrange + " ORDER BY FSTR";
                    header_n = "Select Customer";
                    break;
                case "F50275":
                    SQuery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR, TRIM(A.ACODE) AS PARTY, TRIM(B.ANAME) AS PARTY_NAME FROM sale A, FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '4%' AND a.TYPE!='47'";
                    i0 = 1;
                    break;

                case "F50278":
                    SQuery = "select DISTINCT  trim(name) as fstr,name as state from type where id='{' ORDER BY NAME"; //from master
                    header_n = "Select State";
                    break;

                case "F50279":
                case "F50328":
                case "F50329":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;                
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F50266" || HCID == "F50267" || HCID == "F50268" || HCID == "F50271" || HCID == "F50316" || HCID == "MTH_WEEK" || HCID == "F50386" || HCID == "F50388" || HCID == "F50390" || HCID == "F49212")
                {
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                }
                else
                {
                    fgen.Fn_open_mseek(header_n, frm_qstr);
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
            if (val == "F50141" || val == "F50142" || val == "F50143" || val == "F50271" || val == "F50316" || val == "F49212")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                //fgen.Fn_open_prddmp1("-", frm_qstr);
                if (val == "F49212")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                    SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS inv_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS inv_DT,A.ACODE AS CODE,B.ANAME AS PARTY,b.email,A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM sale A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' AND A.TYPE like '" + value1 + "%' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC, A.VCHNUM DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID1", "FINSYS_K");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_RangeBox("-", frm_qstr);
                }
                else
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
            }
            else if (hf1.Value == "IVAL")
            {
                switch (val)
                {
                    case "F50266":
                        hf1.Value = "";
                        value1 = Request.Cookies["REPLY"].Value;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_sales_reps(frm_qstr);
                        break;
                    //case "F50267":
                    //case "F50268":
                    //    hf1.Value = "";
                    //   // value1 = Request.Cookies["REPLY"].Value;
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", value1);
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    //    fgen.fin_sales_reps(frm_qstr);
                    //    break;

                }
            }
            #region this region add by yogita
            else
            {
                switch (val)
                {
                    case "F50144":
                        // DOM PI
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50144");
                        fgen.fin_sales_reps(frm_qstr);
                        break;
                    default:
                        break;

                    case "F50266":
                        hfcode.Value = value1;
                        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        //fgen.Fn_ValueBox("Fill Days", frm_qstr);
                        //fgen.Fn_open_prddmp1("-", frm_qstr);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfcode.Value);
                        hf1.Value = "IVAL";
                        fgen.Fn_ValueBox("Fill Days", frm_qstr);
                        break;

                    case "F50267":
                    case "F50268":
                    case "F50312":
                        hfcode.Value = value1;
                        //fgen.Fn_ValueBox("Fill Days", frm_qstr);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "MTH_WEEK": //monthly week wise report===s/m module====mktg reports
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_sales_reps(frm_qstr);
                        break;

                    case "TARIF_BILL":
                        hfcode.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    /////
                    case "F50380":
                    case "F50382":
                    case "F50384":
                        if (value1 == "Y") hfbr.Value = "ABR";
                        else hfbr.Value = "";
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F50321":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "COL4", value1);//SELECTED PARTY
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;
                    case "F50275":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "select TRIM(NAME) AS FSTR,TRIM(NAME) AS STATE from type where ID='{' ORDER BY NAME";
                            header_n = "Select State";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;
                            fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        }
                        break;

                    case "F50278":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "COL4", value1);//SELECTED PARTY
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;

                 
                }
            }
            // ELSE STATEMENT IS ENDING HERE
            #endregion
        }
        // else if branch selection box opens then it comes here
        else if (Request.Cookies["REPLY"].Value.Length > 0)
        {
            value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            switch (val)
            {

                case "F50386": //ITEM WISE WISE
                case "F50388"://SUBGROUP WISE
                case "F50390"://MAIN GROUP WISE                    
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    break;
                //AKSHAY REPORT
                case "F50380":
                case "F50382":
                case "F50384":
                    if (hfaskBranch.Value == "Y")
                    {
                        if (value1 == "Y") hfbr.Value = "ABR";
                        else hfbr.Value = "";
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                    }
                    break;


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
            }
        }
        else
        {
            // ADD BY MADHVI FOR SHOWING THE DATE RANGE WHEN USER PRESS ESC 
            //fgen.Fn_open_prddmp1("-", frm_qstr); //commented by madhvi
            switch (val)
            {
                case "F50275":
                    if (hfval.Value == "")
                    {
                        hfval.Value = "%";
                        SQuery = "select TRIM(NAME) AS FSTR,TRIM(NAME) AS STATE from type where ID='{' ORDER BY NAME";
                        header_n = "Select State";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else
                    {
                        hf1.Value = "%";
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    }
                    break;

                case "F50278":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "COL4", value1);//SELECTED PARTY
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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
                // ------------ MERGE BY MADHVI ON 13TH JAN 2018 , MADE BY YOGITA ---------- //

                case "F50141":
                    // SALES REGISTER (DOM.)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50141");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50142":
                    // CUST. WISE REGISTER (DOM.)
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_Col1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50142");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50143":
                    // PRODUCT WISE REGISTER (DOM.)
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_Col1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50143");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                // ------------ MERGE BY MADHVI ON 11TH JAN 2018 , MADE BY YOGITA ---------- //

                case "F50222":
                    // PARTY WISE TOTAL SALES SUMMARY (DOM.)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50222");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50223":
                    // PRODUCT WISE TOTAL SALES SUMMARY (DOM.)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50223");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50224":
                    // PARTY WISE TOTAL QTY (DOM.)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50224");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50228":
                    // 31 DAYS WISE SALES VALUE (DOM.)
                    DateTime date1 = Convert.ToDateTime(fromdt);
                    DateTime date2 = Convert.ToDateTime(todt);
                    TimeSpan days = date2 - date1;
                    if (days.TotalDays > 31)
                    {
                        fgen.msg("-", "AMSG", "Please Select 31 Days Only");
                        return;
                    }
                    else
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50228");
                        fgen.fin_sales_reps(frm_qstr);
                    }
                    break;

                case "F50229":
                    // PARTY WISE TOTAL VALUE (DOM.)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50229");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                // ------------ MERGE BY MADHVI ON 22ND JAN 2018 , MADE BY YOGITA ON 20TH JAN 2018---------- //

                case "F50240":
                    // SCHEDULE VS DISPATCH 31 DAY
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50240");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50241":
                    // SCHEDULE VS DISPATCH 12 MONTH
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F50241");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                //case "F50266": //this is  after daterange box
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                //    hf1.Value = "IVAL";
                //    fgen.Fn_ValueBox("Fill Days", frm_qstr);
                //    break;

                case "F50267":
                case "F50268":
                case "F50312":
                case "F50316"://country wise sales
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    //hf1.Value = "IVAL";
                    //fgen.Fn_ValueBox("Fill Days", frm_qstr);
                    break;

                case "F50386"://MAIN GROUP WISE
                case "F50388"://sub GROUP WISE
                case "F50390"://Item Wise WISE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_BRANCHCD", branch_Cd);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50380": //done
                case "F50382":
                case "F50384":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_BRANCHCD", branch_Cd);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;
                case "F50313":
                case "F50311":
                case "F50314":
                case "F50315":
                case "ITEM_GR_WT": //item Grwt/Nwt===s/m module===mktg reports
                case "TARIF"://TARRIF WISE SUMMARY===s/m module===mktg reports
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "SALE_REJ":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);//sale & rej summary
                    break;

                case "TARIF_BILL"://Tarrif wise Invoice wise Report===s/m module===mktg reports
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50321":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;
                case "F50275":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50278":
                    mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", mq3);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F49212":
                    //
                    if (co_cd == "MLGI" || co_cd == "WING")
                        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F49212");
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50279":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                case "F50328":
                case "F50329":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;

                default:
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_sales_reps(frm_qstr);
                    break;
            }
        }
    }
}