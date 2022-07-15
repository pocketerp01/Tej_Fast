using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_sale : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, frm_cocd, SQuery, xprdrange, cond1, cond2, xprdRange1, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, mq11, mq12, mq13, mq14, mq15, mq16, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld, myear, frm_cDt1, frm_cDt2;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2, ph_tbl, dt4, dt5, dtm, dt6, dt7, dt8, dt9, dt10, dt11;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2, dr1, dr10; DataView dv, dv1, view1, dtview;
    string opbalyr, param, eff_Dt, xprdrange1, DateRange, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID, year2, path;
    string fileName, filepath;
    string frm_UserID, party_cd, part_cd;
    double db1, db2, db3, db4, db5, db6, db7, db8, db9, db10, db11, db12, db13, db14, db15, db19, db16, db17, db18, db20, db21, db22, db23, d1;
    double db = 0;
    DataRow dr2;
    string zipFilePath = "", zipFileName = "";

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
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                frm_cocd = co_cd;
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
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
                case "F50277":
                case "F50276":
                    SQuery = "select TRIM(NAME) AS FSTR,TRIM(NAME) AS STATE from type where ID='{' ORDER BY NAME";
                    header_n = "Select State";
                    break;
                case "F50132":
                case "F50133":
                case "F50134":
                case "F50126":
                case "F50127":
                    SQuery = "select trim(type1) as fstr,name,type1 as code from type where ID='V' AND type1 like '4%' AND TYPE1 NOT IN ('4F','47') ORDER BY code";
                    header_n = "Select Sale Type";
                    break;

                case "F50242":
                case "F50244":
                case "F50245":
                    SQuery = "SELECT trim(mthnum) as fstr, mthnum as code,mthname as name FROM MTHS";
                    header_n = "Select Month";
                    break;

                case "F50225":
                case "F50226":
                case "F50227":
                case "F50128":
                case "F50231":
                case "F50232":
                case "F50233":
                case "F50234":
                case "F50235":
                case "F50236":
                case "F50250":
                case "F50251":
                case "F50252":
                case "F50255":
                case "F50256":
                case "F50257":
                case "F50258":
                case "F50264":
                case "F50265":
                case "F50308":
                case "F50180":
                case "F50181":
                case "F50156": //PLANT WISE SALE                
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F50149": // CLPL
                    //  case "F50155": // CLPL AFTER LAST CHANGES DATE RANGE IS ASKED
                    fgen.Fn_open_dtbox("-", frm_qstr);
                    break;

                case "F50157":
                    SQuery = "Select distinct trim(acode) as fstr,aname as buying_house,acode as code from famst where substr(trim(acode),1,2)='09' order by buying_house";
                    header_n = "Select Buyer";
                    break;
                //case "F50306":
                //    SQuery = "SELECT DISTINCT TRIM(ACODE) AS fstr,trim(acode) as acode,TRIM(ANAME) AS PARTY FROM FAMST WHERE TRIM(ACODE) LIKE '16%' ORDER BY ACODE";
                //    header_n = "Select Party";
                //    break;

                case "F50306":
                case "F50301": //IAIJ REPORT...SELECT INVOICE AFTER THAT CREATING TXT FILE
                case "F50325":
                case "F50326":
                    SQuery = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F50137A":
                    SQuery = "";
                    fgen.msg("-", "CMSG", "Do You Want to Check All Data'13'No for Select Delay Hours");
                    break;

                case "F50321":
                    SQuery = "SELECT 'Y' AS  FSTR,'Pending Order' as Choice,'Y' as opt from dual union all SELECT 'N' AS  FSTR,'All Order' as Choice,'N' as opt from dual";
                    header_n = "Select Option";
                    break;

                case "F50273":
                    SQuery = "select trim(type1) as fstr,name,type1 as code from type where ID='V' AND type1 like '4%' AND TYPE1 NOT IN ('4F','47') ORDER BY code";
                    header_n = "Select Sale Type"; //OPEN MSEEK
                    break;

                case "F50269":
                    SQuery = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F50322":
                case "F50323":
                case "F50324":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F50327":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    //below old
                    // SQuery = "SELECT trim(mthnum) as fstr, mthnum as code,mthname as name FROM MTHS";
                    //header_n = "Select Month";
                    break;

                case "F50154":
                    SQuery = "select 'Y' AS FSTR,'ALL' AS OPT,'ALL ORDERS' AS CHOICE_ FROM DUAL UNION ALL select 'N' AS FSTR,'PEND' AS OPT,'PENDING ORDERS' AS CHOICE_ FROM DUAL";
                    header_n = "Select Choice";
                    break;

                case "F50330"://CSV REPROT FOR VELVIN
                    SQuery = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F50242" || HCID == "F50244" || HCID == "F50245" || HCID == "F50157" || HCID == "F50321" || HCID == "F50154")//||HCID=="F50327"
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
            if (val == "F50132" || val == "F50126" || val == "F50127" || val == "F50128" || val == "F50133" || val == "F50134" || val == "F50137A" || val == "F50273" || val == "F50269")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                if (val == "F50137A")
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                else fgen.Fn_open_Act_itm_prd("-", frm_qstr);
            }
            #region this region add by yogita
            //THIS ELSE STATEMENT ADD BY YOGITA 
            else
            {
                switch (val)
                {
                    // ------------ MERGE BY MADHVI ON 22TH JAN 2018 , MADE BY YOGITA ON 20TH JAN 2018--------- //
                    case "F50242":
                    // SCHEDULE VS PRODUCTION VS DISPATCH SUMMARY
                    case "F50244":
                    // SCHEDULE VS DISPATCH CUSTOMER WISE SUMMARY
                    case "F50245":
                        // SCHEDULE VS DISPATCH CUSTOMER WISE,ITEM WISE SUMMARY
                        if (Convert.ToInt32(value1) <= 3)
                        {
                            i1 = Convert.ToInt16(year) + 1;
                        }
                        else
                        {
                            i1 = Convert.ToInt16(year);
                        }
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        hf1.Value = value1 + "/" + i1;
                        break;

                    case "F50157":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F50321":
                        if (hf1.Value == "")
                        {
                            hf1.Value = value1;//fstr value
                            fgenMV.Fn_Set_Mvar(frm_qstr, "COL5", value1);
                            SQuery = "SELECT DISTINCT A.ACODE AS FSTR,A.ACODE AS CODE,B.ANAME AS CUSTOMER FROM SOMAS A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE)";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Customer", frm_qstr);
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "COL4", value1);
                            fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        }
                        break;

                    case "F50277":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "COL4", value1);//SELECTED PARTY
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;

                    case "F50276":
                        hfcode.Value = value1;
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;

                    case "F50154":
                        hfval.Value = value1;//selected value
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;

                    case "F50054":
                        SQuery = "";
                        hfcode.Value = value1;
                        if (hfcode.Value == "GROUP LEVEL")
                        {
                            SQuery = "SELECT TRIM(A.TYPE1) AS FSTR,trim(A.TYPE1) AS CODE,A.NAME as GroupName FROM TYPE A  WHERE  A. ID='Y' and substr(trim(a.type1),1,1) in ('0','1','2','3')  ORDER BY FSTR";
                            hfHead.Value = "MAINGRP";
                        }
                        else if (hfcode.Value == "SUB GROUP LEVEL")
                        {
                            SQuery = "select  DISTINCT TRIM(A.icode) AS FSTR,TRIM(A.icode) AS SUBGRPCODE,A.INAME AS SUBGRPNAME  FROM ITEM A  WHERE length(TRIM(A.icode))=4  and substr(TRIM(A.icode),1,1) in ('0','1','2','3') ORDER BY FSTR";
                            hfHead.Value = "SUBGRP";
                        }
                        else if (hfcode.Value == "ITEM LEVEL")
                        {
                            SQuery = "select  DISTINCT TRIM(A.ICODE) AS FSTR,A.ICODE,A.INAME AS ITEMNAME FROM ITEM A  WHERE  length(TRIM(A.icode))=8  AND substr(trim(a.icode),1,1) in ('0','1','2','3')  ORDER BY FSTR ";
                            hfHead.Value = "ITEM";
                        }
                        if (SQuery.Length > 1)
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Group Item", frm_qstr);
                        }
                        else
                        {
                            hfSales.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    default:
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
                case "F50137A":
                    hfcode.Value = "0";
                    if (value1 == "Y") fgen.Fn_open_prddmp1("-", frm_qstr);
                    else
                    {
                        fgen.Fn_ValueBox("-", frm_qstr);
                    }
                    break;
                case "F50269":
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F50054":
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
                    else branch_Cd = "branchcd='" + mbr + "'";
                    SQuery = "SELECT 'GROUP LEVEL' AS FSTR,'GROUP LEVEL' AS  OPTION_  FROM DUAL UNION ALL SELECT 'SUB GROUP LEVEL' AS FSTR,'SUB GROUP LEVEL' AS  OPTION_  FROM DUAL UNION ALL SELECT 'ITEM LEVEL' AS FSTR,'ITEM LEVEL' AS OPTION_  FROM DUAL";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Select Option", frm_qstr);
                    break;
            }
        }
        //when press escape
        else
        {
            // ADD BY MADHVI FOR SHOWING THE DATE RANGE WHEN USER PRESS ESC 
            switch (val)
            {
                case "F50277":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "COL4", value1);//SELECTED PARTY
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F50276":
                    hfcode.Value = value1;
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
            //fgen.Fn_open_prddmp1("-", frm_qstr);
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
                // ------------ MERGE BY MADHVI ON 13 JAN 2018 , MADE BY YOGITA ---------- //

                case "F50126":
                    // ORDER DATA CHECKLIST (DOM.)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Order Data Checklist (Dom.) of (" + hfcode.Value + ") for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50127":
                    //Pending Order Checklist (Dom.)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Order Checklist (Dom.) of (" + hfcode.Value + ") for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50128":
                    //Pending Order Checklist (Dom.)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    string mthdt2 = "", mthdt1 = "";
                    mthdt1 = fgen.seek_iname(frm_qstr, co_cd, "select to_chaR(to_DatE('" + value1 + "','dd/mm/yyyy'),'yyyymm')  as mthn from dual ", "mthn");
                    mthdt2 = fgen.seek_iname(frm_qstr, co_cd, "select to_chaR(to_DatE('" + value2 + "','dd/mm/yyyy'),'yyyymm')  as mthn from dual ", "mthn");

                    // A.SCH_MTH INVALID IDENTIFIER ERROR IS COMING SO CREATE REPORT THROUGH VIEW
                    // SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.sch_mth between '" + mthdt1 + "' and '" + mthdt2 + "' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", "-");
                    SQuery = "SELECT F.ANAME as Customer_NAme,F.Addr1 as Address,I.INAME as ITem_NAme,I.Cpartno as Part_No,A.SCH_QTY,A.SALE_QTY,A.BAL_QTY,A.ACODE as ERP_Act_Cd,A.Icode as ERP_Item_Cd FROM WBVU_SALE_SCH A, FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND A." + branch_Cd + " AND A.SCH_MTH between '" + mthdt1 + "' and '" + mthdt2 + "' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY F.ANAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Schedule Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50132":
                    // SALES DATA SEARCH(DOM.) CHECKLIST
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Data Checklist (Dom.) of (" + hfcode.Value + ") for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50133":
                    // CUSTOMER WISE SALES(DOM.) CHECKLIST
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd == "0") party_cd = "";
                    if (part_cd == "0") part_cd = "";
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Sales(Dom.) Checklist of (" + hfcode.Value + ") for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50134":
                    // PRODUCT WISE SALES(DOM.) CHECKLIST
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Product Wise Sales(Dom.) Checklist of (" + hfcode.Value + ") for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50156":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    // PLANT WISE SALES DATA
                    // ORIGINAL --------------------------- QUERY IS COMING BLANK FROM REPS CONFIG SO CREATED THROUGH MANUAL QUERY ON 05 JULY 2018
                    // SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type like '4%'", xprdrange);
                    // fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    // header_n = fgen.seek_iname(frm_qstr, co_cd, "select name from type where id='V' and type1='" + hfcode.Value + "'", "name");
                    // ---------------------------
                    dt2 = new DataTable();
                    dt2.Columns.Add("Month_Name", typeof(string));
                    dt = new DataTable();
                    mq0 = "select distinct type1,name from type where id='B' order by type1";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    foreach (DataRow dr in dt.Rows)
                    {
                        dt2.Columns.Add(dr["type1"].ToString().Trim(), typeof(double));
                    }
                    dt2.Columns.Add("Total", typeof(double));
                    mq1 = "select to_char(vchdate,'Month yyyy') as vchdate,sum(iamount) as amt,branchcd,to_char(vchdate,'yyyymm') as vdd from ivoucher where branchcd!='DD' and type like '4%' and vchdate " + xprdrange + " group by to_char(vchdate,'Month yyyy'),branchcd,to_char(vchdate,'yyyymm') order by vdd";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);
                    dr1 = null;
                    if (dt1.Rows.Count > 0)
                    {
                        dv = new DataView(dt1);
                        dticode = new DataTable();
                        dticode = dv.ToTable(true, "vdd");
                        foreach (DataRow dr3 in dticode.Rows)
                        {
                            dv1 = new DataView(dt1, "vdd='" + dr3["vdd"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode2 = new DataTable();
                            dticode2 = dv1.ToTable();
                            dr1 = dt2.NewRow();
                            db1 = 0;
                            for (int i = 0; i < dticode2.Rows.Count; i++)
                            {
                                dr1["Month_Name"] = dticode2.Rows[i]["vchdate"].ToString().Trim();
                                mq2 = dticode2.Rows[i]["branchcd"].ToString().Trim();
                                dr1[mq2] = dticode2.Rows[i]["amt"].ToString().Trim();
                                db1 += fgen.make_double(dticode2.Rows[i]["amt"].ToString().Trim());
                                dr1["total"] = db1;
                            }
                            dt2.Rows.Add(dr1);
                        }
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataColumn dc in dt2.Columns)
                        {
                            i1 = dc.Ordinal;
                            if (i1 > 0)
                            {
                                string name = dc.ToString().Trim();
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                try
                                {
                                    if (myname != "0")
                                    {
                                        dt2.Columns[i1].ColumnName = "(" + name + ") " + myname;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt2;
                    fgen.Fn_open_rptlevel("Plant Wise Sales Data for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                // ---------------------------------------------------------------------------------------- //


                // ------------ MERGE BY MADHVI ON 11TH JAN 2018 , MADE BY YOGITA ---------- //

                // NOTE - AS PER PUNEET SIR THERE IS NO MEANING OF SHOWING TOTAL QUANTITIES BASED ON PARTY WISE SO I M COMMENTING IT AND ON THIS ID PRINTABLE REPORT WILL BE CREATED .
                //case "F50224":
                //     PARTY WISE 12 MONTH SALES (QTY)
                //    SQuery = "select a.acode as code,b.aname as Account,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from ( select acode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else   0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + ") a,famst b where trim(a.acode)=trim(b.acode) group by a.acode,b.aname";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Party Wise 12 Month Sales (Qty) for the Period " + value1 + " to " + value2, frm_qstr);
                //    break;

                case "F50225":
                    // PARTY WISE 12 MONTH SALES (VALUE)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd == "0") party_cd = "";
                    if (part_cd == "0") part_cd = "";
                    SQuery = "select a.acode as code,b.aname as Account,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from ( select acode,(Case when to_char(vchdate,'mm')='04' then nvl(IAMOUNT,'0') else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(IAMOUNT,'0') else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(IAMOUNT,'0') else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(IAMOUNT,'0') else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(IAMOUNT,'0') else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(IAMOUNT,'0') else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(IAMOUNT,'0') else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(IAMOUNT,'0') else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(IAMOUNT,'0') else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(IAMOUNT,'0') else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(IAMOUNT,'0') else   0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(IAMOUNT,'0') else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,famst b where trim(a.acode)=trim(b.acode) group by a.acode,b.aname order by code";
                    SQuery = "select a.acode as code,b.aname as Account,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from ( select acode,(Case when to_char(vchdate,'mm')='04' then nvl(IAMOUNT,'0') else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(IAMOUNT,'0') else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(IAMOUNT,'0') else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(IAMOUNT,'0') else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(IAMOUNT,'0') else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(IAMOUNT,'0') else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(IAMOUNT,'0') else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(IAMOUNT,'0') else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(IAMOUNT,'0') else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(IAMOUNT,'0') else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(IAMOUNT,'0') else   0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(IAMOUNT,'0') else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and acode like '" + party_cd + "%' ) a,famst b where trim(a.acode)=trim(b.acode) group by a.acode,b.aname order by code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Party Wise 12 Month Sales (Value) for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50226":
                    // PRODUCT WISE 12 MONTH SALES (QTY)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd == "0") party_cd = "";
                    if (part_cd == "0") part_cd = "";
                    //SQuery = "select a.icode as item_code,b.iname as item_name,b.cpartno as partno,b.hscode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from ( select icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,iqtyout  from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,ITEM b where trim(a.icode)=trim(b.icode)  group by a.icode,b.iname,b.cpartno,b.hscode order by item_code";
                    SQuery = "select a.icode as item_code,b.iname as item_name,b.cpartno as partno,b.hscode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from ( select icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,iqtyout  from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and icode like '" + party_cd + "%') a,ITEM b where trim(a.icode)=trim(b.icode)  group by a.icode,b.iname,b.cpartno,b.hscode order by item_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Product Wise 12 Month Sales (Qty) for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50227":
                    // PRODUCT WISE 12 MONTH SALES (VALUE)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    //  SQuery = "select a.icode as item_code,b.INAME AS ITEM_NAME,b.cpartno as partno,b.hscode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from ( select icode,(Case when to_char(vchdate,'mm')='04' then nvl(IAMOUNT,'0') else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(IAMOUNT,'0') else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(nvl(IAMOUNT,'0'),'0') else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(IAMOUNT,'0') else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(IAMOUNT,'0') else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(IAMOUNT,'0') else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(IAMOUNT,'0') else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(IAMOUNT,'0') else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(IAMOUNT,'0') else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(IAMOUNT,'0') else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(IAMOUNT,'0') else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(IAMOUNT,'0') else 0 end) as mar  from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') a,ITEM b where trim(a.icode)=trim(b.icode)  group by a.icode,b.iname,b.cpartno,b.hscode ORDER BY item_code";
                    SQuery = "select a.icode as item_code,b.INAME AS ITEM_NAME,b.cpartno as partno,b.hscode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from ( select icode,(Case when to_char(vchdate,'mm')='04' then nvl(IAMOUNT,'0') else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(IAMOUNT,'0') else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(nvl(IAMOUNT,'0'),'0') else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(IAMOUNT,'0') else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(IAMOUNT,'0') else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(IAMOUNT,'0') else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(IAMOUNT,'0') else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(IAMOUNT,'0') else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(IAMOUNT,'0') else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(IAMOUNT,'0') else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(IAMOUNT,'0') else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(IAMOUNT,'0') else 0 end) as mar  from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and icode like '" + party_cd + "%') a,ITEM b where trim(a.icode)=trim(b.icode)  group by a.icode,b.iname,b.cpartno,b.hscode ORDER BY item_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Product Wise 12 Month Sales (Value) for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                // ---------------------------------------------------------------------------------------- //


                // ------------ MERGE BY MADHVI ON 20TH JAN 2018 , MADE BY YOGITA ON 19TH JAN 2018---------- //

                case "F50231":
                    // DISTRICT WISE SALES VALUE
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    // ON 20 JUNE 2018 , BILLTOT WAS REPLACED WITH IAMOUNT 
                    // SQuery = "SELECT B.DISTRICT,SUM(A.BILL_TOT) AS SALE FROM (SELECT TRIM(ACODE) AS ACODE,BILL_TOT FROM SALE WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%'  AND VCHDATE " + xprdrange + ") A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND B.DISTRICT LIKE '" + party_cd + "%'  GROUP BY B.DISTRICT ORDER BY B.DISTRICT";
                    SQuery = "SELECT B.DISTRICT,SUM(A.IAMOUNT) AS SALE FROM (SELECT TRIM(ACODE) AS ACODE,IAMOUNT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%'  AND VCHDATE " + xprdrange + ") A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND B.DISTRICT LIKE '" + party_cd + "%'  GROUP BY B.DISTRICT ORDER BY B.DISTRICT";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("District Wise Sales Value for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50232":
                    // STATE WISE SALES VALUE
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    // ON 20 JUNE 2018 , BILLTOT WAS REPLACED WITH IAMOUNT 
                    // SQuery = "SELECT B.STATEN AS STATE,SUM(A.BILL_TOT) AS SALE FROM (SELECT TRIM(ACODE) AS ACODE,BILL_TOT FROM SALE WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%'  AND VCHDATE " + xprdrange + ") A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND B.STATEN LIKE '" + party_cd + "%' GROUP BY B.STATEN ORDER BY STATE";
                    SQuery = "SELECT B.STATEN AS STATE,SUM(A.IAMOUNT) AS SALE FROM (SELECT TRIM(ACODE) AS ACODE,IAMOUNT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%'  AND VCHDATE " + xprdrange + ") A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND B.STATEN LIKE '" + party_cd + "%' GROUP BY B.STATEN ORDER BY STATE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("State Wise Sales Value for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50233":
                    // ZONE WISE SALES VALUE
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    // ON 20 JUNE 2018 , BILLTOT WAS REPLACED WITH IAMOUNT 
                    // SQuery = "SELECT B.ZONAME AS ZONE,SUM(A.BILL_TOT) AS SALE FROM (SELECT TRIM(ACODE) AS ACODE,BILL_TOT FROM SALE WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%'  AND VCHDATE " + xprdrange + ") A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND B.ZONAME LIKE '" + party_cd + "%' GROUP BY B.ZONAME ORDER BY ZONE";
                    SQuery = "SELECT B.ZONAME AS ZONE,SUM(A.IAMOUNT) AS SALE FROM (SELECT TRIM(ACODE) AS ACODE,IAMOUNT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%'  AND VCHDATE " + xprdrange + ") A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND B.ZONAME LIKE '" + party_cd + "%' GROUP BY B.ZONAME ORDER BY ZONE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Zone Wise Sales Value for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                // ---------------------------------------------------------------------------------------- //
                // ------------ MERGE BY MADHVI ON 22TH JAN 2018 , MADE BY YOGITA ON 20TH JAN 2018--------- //
                case "F50234":
                    // MARKETING PERSON WISE SALES VALUE
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "SELECT TRIM(B.BSSCH) AS MARKETING_CODE,C.NAME AS MARKETING_PERSON,SUM(A.BILL_TOT) AS SALE  FROM (select trim(acode) as acode,bill_tot from sale where branchcd='" + mbr + "' and type like '4%'  and vchdate " + xprdrange + " ) A,FAMST B,TYPEGRP C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(B.BSSCH)=TRIM(C.TYPE1) AND C.ID='A' AND B.BSSCH LIKE '" + party_cd + "%' GROUP BY TRIM(B.BSSCH),C.NAME ORDER BY MARKETING_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Marketing Person Wise Sales Value for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                // ---------------------------------------------------------------------------------------- //


                // ------------ MERGE BY MADHVI ON 20TH JAN 2018 , MADE BY YOGITA ON 19TH JAN 2018--------- //
                case "F50235":
                    // CUSTOMER GROUP WISE SALES VALUE
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "SELECT B.MKTGGRP AS CUSTOMER_GROUP ,SUM(A.BILL_TOT) AS SALE FROM (SELECT TRIM(ACODE) AS ACODE,BILL_TOT FROM SALE WHERE BRANCHCD='" + mbr + "'  AND TYPE LIKE '4%'  AND VCHDATE " + xprdrange + ") A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND B.MKTGGRP LIKE '" + party_cd + "%' GROUP BY B.MKTGGRP ORDER BY CUSTOMER_GROUP";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Group Wise Sales Value for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50236":
                    // PRODUCT SUB GROUP WISE SALES VALUE
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    //SQuery = "SELECT SUBSTR(TRIM(A.ICODE),0,2) AS MAINGRP_CODE,C.NAME AS MAINGRP_NAME,SUBSTR(TRIM(A.ICODE),0,4) AS SUB_GROUP,B.INAME AS SUB_GRP_NAME,SUM(A.IAMOUNT) AS SALE FROM (SELECT TRIM(ICODE)  AS ICODE,IAMOUNT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdrange + ") A,ITEM B,TYPE C WHERE SUBSTR(TRIM(A.ICODE),0,4)=TRIM(B.ICODE) AND LENGTH (TRIM(B.ICODE))=4  AND SUBSTR(TRIM(A.ICODE),0,2)=TRIM(C.TYPE1) AND C.ID='Y' AND SUBSTR(A.ICODE,1,4) LIKE '" + part_cd + "%' GROUP BY SUBSTR(TRIM(A.ICODE),0,4),SUBSTR(TRIM(A.ICODE),0,2),B.INAME,C.NAME ORDER BY MAINGRP_CODE";
                    SQuery = "SELECT SUBSTR(TRIM(A.ICODE),1,2) AS MAINGRP_CODE,C.NAME AS MAINGRP_NAME,SUBSTR(TRIM(A.ICODE),1,4) AS SUB_GROUP,B.INAME AS SUB_GRP_NAME,SUM(A.IAMOUNT) AS SALE FROM (SELECT DISTINCT TRIM(ICODE)  AS ICODE,IAMOUNT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdrange + ") A,ITEM B,TYPE C WHERE SUBSTR(TRIM(A.ICODE),1,4)=TRIM(B.ICODE) AND LENGTH(TRIM(B.ICODE))=4  AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(C.TYPE1) AND C.ID='Y' AND SUBSTR(A.ICODE,1,4) LIKE '" + part_cd + "%' AND SUBSTR(A.ICODE,1,2) LIKE '" + party_cd + "%' GROUP BY SUBSTR(TRIM(A.ICODE),1,4),SUBSTR(TRIM(A.ICODE),1,2),B.INAME,C.NAME ORDER BY MAINGRP_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Product Sub Group Wise Sales Value for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                // ------------ MERGE BY MADHVI ON 22TH JAN 2018 , MADE BY YOGITA ON 20TH JAN 2018--------- //

                case "F50250":
                    // SCHEDULE VS DISPATCH QTY YEAR ON YEAR
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    mq0 = "";
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(fmdate,'yyyy') as year  from co where code!='" + co_cd + year + "' and to_char(fmdate,'yyyy')<'" + year + "' order by year desc", "year");
                    SQuery = "select trim(A.ACODE) AS CUST_CODE,TRIM(B.ANAME) AS CUSTOMER_NAME,SUM(A.sch_qty16) AS SCH_QTY_YR" + mq0 + ",SUM(A.desp_qty16) AS DISP_QTY_YR" + mq0 + ",SUM(A.sch_qty17) AS SCH_QTY_YR" + year + ",SUM(A.desp_qty17) AS DISP_QTY_YR" + year + " from (select acode,sch_qty16,sch_qty17,0 as desp_qty16,0 as desp_qty17 from (select trim(acode) as acode,sum(total) as sch_qty16,0 AS sch_qty17 from schedule where branchcd='" + mbr + "' and type='46' and to_char(vchdate,'yyyy')='" + mq0 + "' group by acode union all select trim(acode) as acode,0 as sch_qty16,sum(total) AS Sch_qty17 from schedule where branchcd='" + mbr + "' and type='46' and to_char(vchdate,'yyyy')='" + year + "' group by acode) union all select acode,0 as sch_qty16,0 as sch_qty17,desp_qty16,desp_qty17 from (select trim(acode) as acode,sum(iqtyout) as desp_qty16,0 as desp_qty17 from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_char(vchdate,'yyyy')='" + mq0 + "' group by acode union all select trim(acode) as acode,0 as desp_qty16,sum(iqtyout) as desp_qty17 from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_char(vchdate,'yyyy')='" + year + "' /*and nvl(iqtyout,0)>0*/  group by acode)) A,FAMST B WHERE  TRIM(A.ACODE)=TRIM(B.ACODE) and a.acode like '" + party_cd + "%' GROUP BY A.ACODE,B.ANAME ORDER BY CUST_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Schedule Vs Dispatch Qty Year On Year for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F50251":
                    // SCHEDULE VS DISPATCH VALUE YEAR ON YEAR
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    mq0 = "";
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(fmdate,'yyyy') as year  from co where code!='" + co_cd + year + "' and to_char(fmdate,'yyyy')<'" + year + "' order by year desc", "year");
                    SQuery = "select trim(A.ACODE) AS CUST_CODE,trim(B.ANAME) AS CUST_NAME,SUM(A.sch_qty16) AS SCH_QTY_YR" + mq0 + ",SUM(A.desp_qty16) AS DISP_QTY_YR" + mq0 + ",SUM(A.sch_qty17) AS SCH_QTY_YR" + year + ",SUM(A.desp_qty17) AS DISP_QTY_YR" + year + "  from (select acode,sch_qty16,sch_qty17,0 as desp_qty16,0 as desp_qty17 from (select trim(acode) as acode,sum(total*irate) as sch_qty16,0 AS sch_qty17 from schedule where branchcd='" + mbr + "' and type='46' and to_char(vchdate,'yyyy')='" + mq0 + "' group by acode union all select trim(acode) as acode,0 as sch_qty16,sum(total*irate) AS Sch_qty17 from schedule where branchcd='" + mbr + "' and type='46' and to_char(vchdate,'yyyy')='" + year + "' group by acode) union all  select acode,0 as sch_qty16,0 as sch_qty17,desp_qty16,desp_qty17 from (select trim(acode) as acode,sum(iamount) as desp_qty16,0 as desp_qty17 from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_char(vchdate,'yyyy')='" + mq0 + "' /*and nvl(iqtyout,0)>0*/ group by acode union all select trim(acode) as acode,0 as desp_qty16,sum(iamount) as desp_qty17 from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_char(vchdate,'yyyy')='" + year + "' /*and nvl(iqtyout,0)>0*/ group by acode)) A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.acode like '" + party_cd + "%' GROUP BY A.ACODE,B.ANAME order by CUST_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Schedule Vs Dispatch Value Year On Year for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F50252":
                    // SCHEDULE VS DISPATCH VALUE YEAR ON YEAR
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    
                    mq0 = "select c.Aname as Customer,b.Iname as Item_Name,a.* from (select '02-Supply' as Data_Grp,(nvl(day1,0)+nvl(day2,0)+nvl(day3,0)+nvl(day4,0)+nvl(day5,0)+nvl(day6,0)+nvl(day7,0)+nvl(day8,0)+nvl(day9,0)+nvl(day10,0)+nvl(day11,0)+nvl(day12,0)+nvl(day13,0)+nvl(day14,0)+nvl(day15,0)+nvl(day16,0)+nvl(day17,0)+nvl(day18,0)+nvl(day19,0)+nvl(day20,0)+nvl(day21,0)+nvl(day22,0)+nvl(day23,0)+nvl(day24,0)+nvl(day25,0)+nvl(day26,0)+nvl(day27,0)+nvl(day28,0)+nvl(day29,0)+nvl(day30,0)+nvl(day31,0)) as Total,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31,Acode,icode from (WITH pivot_data AS (SELECT to_Char(a.vchdate,'dd') as  Mth_no, upper(Trim(A.acode)) as Acode,trim(a.icode) as Icode, sum(nvl(a.iqtyout,0))  as sal FROM ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " group by to_Char(a.vchdate,'dd'),upper(Trim(A.acode)),trim(a.icode) " ;
                    mq1 = ")SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 ))) ";
                    mq2 = "union all select '01-Schedule' as Data_Grp,(nvl(day1,0)+nvl(day2,0)+nvl(day3,0)+nvl(day4,0)+nvl(day5,0)+nvl(day6,0)+nvl(day7,0)+nvl(day8,0)+nvl(day9,0)+nvl(day10,0)+nvl(day11,0)+nvl(day12,0)+nvl(day13,0)+nvl(day14,0)+nvl(day15,0)+nvl(day16,0)+nvl(day17,0)+nvl(day18,0)+nvl(day19,0)+nvl(day20,0)+nvl(day21,0)+nvl(day22,0)+nvl(day23,0)+nvl(day24,0)+nvl(day25,0)+nvl(day26,0)+nvl(day27,0)+nvl(day28,0)+nvl(day29,0)+nvl(day30,0)+nvl(day31,0)) as Total ,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31,Acode,Icode from (WITH pivot_data AS (SELECT to_Char(a.dlv_Date,'dd') as  Mth_no, upper(Trim(A.acode)) as Acode,trim(a.icode) as Icode, sum(nvl(a.budgetcost,0)) as sal FROM Budgmst a where a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + xprdrange + " group by to_Char(a.dlv_Date,'dd'),upper(Trim(A.acode)),trim(a.icode) ";
                    mq3 = ") SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 ))))a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by  c.aname,b.iname,a.Data_Grp";

                    
                    
                    //mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(fmdate,'yyyy') as year  from co where code!='" + co_cd + year + "' and to_char(fmdate,'yyyy')<'" + year + "' order by year desc", "year");
                    SQuery = mq0+mq1+mq2+mq3;
                    //"select trim(A.ACODE) AS CUST_CODE,trim(B.ANAME) AS CUST_NAME,SUM(A.sch_qty16) AS SCH_QTY_YR" + mq0 + ",SUM(A.desp_qty16) AS DISP_QTY_YR" + mq0 + ",SUM(A.sch_qty17) AS SCH_QTY_YR" + year + ",SUM(A.desp_qty17) AS DISP_QTY_YR" + year + "  from (select acode,sch_qty16,sch_qty17,0 as desp_qty16,0 as desp_qty17 from (select trim(acode) as acode,sum(total*irate) as sch_qty16,0 AS sch_qty17 from schedule where branchcd='" + mbr + "' and type='46' and to_char(vchdate,'yyyy')='" + mq0 + "' group by acode union all select trim(acode) as acode,0 as sch_qty16,sum(total*irate) AS Sch_qty17 from schedule where branchcd='" + mbr + "' and type='46' and to_char(vchdate,'yyyy')='" + year + "' group by acode) union all  select acode,0 as sch_qty16,0 as sch_qty17,desp_qty16,desp_qty17 from (select trim(acode) as acode,sum(iamount) as desp_qty16,0 as desp_qty17 from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_char(vchdate,'yyyy')='" + mq0 + "' /*and nvl(iqtyout,0)>0*/ group by acode union all select trim(acode) as acode,0 as desp_qty16,sum(iamount) as desp_qty17 from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_char(vchdate,'yyyy')='" + year + "' /*and nvl(iqtyout,0)>0*/ group by acode)) A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.acode like '" + party_cd + "%' GROUP BY A.ACODE,B.ANAME order by CUST_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Schedule Vs Dispatch Value Year On Year for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                // ---------------------------------------------------------------------------------------- //


                // ------------ MERGE BY MADHVI ON 19TH JAN 2018 , MADE BY YOGITA ON 18TH JAN 2018--------- //

                case "F50255":
                    // PRODUCTS WHERE SALES ARE GROWING
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq11 = "";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-6),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-5),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-4),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq3 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-3),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq4 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-2),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq5 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-1),'mm'),'mm/YYYY') as curr from dual", "curr");

                    mq6 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq0 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq7 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq1 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq8 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq2 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq9 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq3 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq10 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq4 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq11 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq5 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    SQuery = "SELECT A.ICODE AS ITEM_CODE,B.INAME AS ITEM_NAME,B.CPARTNO AS PARTNO,B.UNIT,SUM(CURR1) AS " + mq6 + ",SUM(A.CURR2) AS " + mq7 + ",SUM(CURR3) AS " + mq8 + ",SUM(A.CURR4) AS " + mq9 + ",SUM(CURR5) AS " + mq10 + ",SUM(A.CURR6) AS " + mq11 + " FROM(SELECT TRIM(ICODE) AS ICODE,SUM(IQTYOUT) AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq0 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,SUM(IQTYOUT) AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq1 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,SUM(IQTYOUT) AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq2 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,SUM(IQTYOUT) AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq3 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,SUM(IQTYOUT) AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq4 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,SUM(IQTYOUT) AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq5 + "' GROUP BY  TRIM(ICODE))A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and a.icode like '" + party_cd + "%' GROUP BY A.ICODE,B.INAME,B.CPARTNO,B.UNIT having round((sum(curr4)+sum(curr5)+sum(curr6))/3,2)>round((sum(curr1)+sum(curr2)+sum(curr3))/3,2) ORDER BY ITEM_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Products Where Sales are Growing (Based on 3 Months Avg. Out Qty.) from " + mq0 + " To " + mq5 + "", frm_qstr);
                    break;

                case "F50256":
                    // CUSTOMERS WHERE SALES ARE GROWING
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq11 = "";
                    cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-6),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-5),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-4),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq3 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-3),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq4 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-2),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq5 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-1),'mm'),'mm/YYYY') as curr from dual", "curr");

                    mq6 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq0 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq7 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq1 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq8 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq2 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq9 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq3 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq10 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq4 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq11 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq5 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    SQuery = "SELECT A.ACODE AS CUSTOMER_CODE,B.ANAME AS CUSTOMER_NAME,SUM(CURR1) AS " + mq6 + ",SUM(A.CURR2) AS " + mq7 + ",SUM(CURR3) AS " + mq8 + ",SUM(A.CURR4) AS " + mq9 + ",SUM(CURR5) AS " + mq10 + ",SUM(A.CURR6) AS " + mq11 + " FROM(SELECT TRIM(ACODE) AS ACODE,SUM(IAMOUNT) AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq0 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,SUM(IAMOUNT) AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq1 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,SUM(IAMOUNT) AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq2 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,SUM(IAMOUNT) AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq3 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,SUM(IAMOUNT) AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq4 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,SUM(IAMOUNT) AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq5 + "' GROUP BY  TRIM(ACODE))A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.ACODE like '" + party_cd + "%' GROUP BY A.ACODE,B.ANAME having round((sum(curr4)+sum(curr5)+sum(curr6))/3,2)>round((sum(curr1)+sum(curr2)+sum(curr3))/3,2) ORDER BY CUSTOMER_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customers Where Sales are Growing (Based on 3 Months Avg. Amt.) from " + mq0 + " To " + mq5 + "", frm_qstr);
                    break;

                case "F50257":
                    // PRODUCTS WHERE SALES ARE FALLING
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq11 = "";
                    cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-6),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-5),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-4),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq3 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-3),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq4 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-2),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq5 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-1),'mm'),'mm/YYYY') as curr from dual", "curr");

                    mq6 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq0 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq7 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq1 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq8 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq2 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq9 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq3 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq10 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq4 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq11 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq5 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    SQuery = "SELECT A.ICODE AS ITEM_CODE,B.INAME AS ITEM_NAME,B.CPARTNO AS PARTNO,B.UNIT,SUM(CURR1) AS " + mq6 + ",SUM(A.CURR2) AS " + mq7 + ",SUM(CURR3) AS " + mq8 + ",SUM(A.CURR4) AS " + mq9 + ",SUM(CURR5) AS " + mq10 + ",SUM(A.CURR6) AS " + mq11 + " FROM(SELECT TRIM(ICODE) AS ICODE,SUM(IQTYOUT) AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq0 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,SUM(IQTYOUT) AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq1 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,SUM(IQTYOUT) AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq2 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,SUM(IQTYOUT) AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq3 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,SUM(IQTYOUT) AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq4 + "' GROUP BY  TRIM(ICODE) union all SELECT TRIM(ICODE) AS ICODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,SUM(IQTYOUT) AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq5 + "' GROUP BY  TRIM(ICODE))A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and a.icode like '" + party_cd + "%' GROUP BY A.ICODE,B.INAME,B.CPARTNO,B.UNIT having round((sum(curr4)+sum(curr5)+sum(curr6))/3,2)<round((sum(curr1)+sum(curr2)+sum(curr3))/3,2) ORDER BY ITEM_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Products Where Sales are Falling (Based on 3 Months Avg. Out Qty.) from " + mq0 + " To " + mq5 + "", frm_qstr);
                    break;

                case "F50258":
                    // CUSTOMERS WHERE SALES ARE FALLING
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq11 = "";
                    cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-6),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-5),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-4),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq3 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-3),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq4 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-2),'mm'),'mm/YYYY') as curr from dual", "curr");
                    mq5 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + cond + "','dd/mm/yyyy'),-1),'mm'),'mm/YYYY') as curr from dual", "curr");

                    mq6 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq0 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq7 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq1 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq8 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq2 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq9 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq3 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq10 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq4 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    mq11 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + mq5 + "','mm/yyyy'),'Monyy') as curr from dual", "curr");
                    SQuery = "SELECT A.ACODE AS CUSTOMER_CODE,B.ANAME AS CUSTOMER_NAME,SUM(CURR1) AS " + mq6 + ",SUM(A.CURR2) AS " + mq7 + ",SUM(CURR3) AS " + mq8 + ",SUM(A.CURR4) AS " + mq9 + ",SUM(CURR5) AS " + mq10 + ",SUM(A.CURR6) AS " + mq11 + " FROM(SELECT TRIM(ACODE) AS ACODE,SUM(IAMOUNT) AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq0 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,SUM(IAMOUNT) AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq1 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,SUM(IAMOUNT) AS CURR3,0 AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq2 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,SUM(IAMOUNT) AS CURR4,0 AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq3 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,SUM(IAMOUNT) AS CURR5,0 AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq4 + "' GROUP BY  TRIM(ACODE) union all SELECT TRIM(ACODE) AS ACODE,0 AS CURR1,0 AS CURR2,0 AS CURR3,0 AS CURR4,0 AS CURR5,SUM(IAMOUNT) AS CURR6 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + mq5 + "' GROUP BY  TRIM(ACODE))A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.ACODE like '" + party_cd + "%' GROUP BY A.ACODE,B.ANAME having round((sum(curr4)+sum(curr5)+sum(curr6))/3,2)<round((sum(curr1)+sum(curr2)+sum(curr3))/3,2) ORDER BY CUSTOMER_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customers Where Sales are Falling (Based on 3 Months Avg. Amt.) from " + mq0 + " To " + mq5 + "", frm_qstr);
                    break;

                // ------------ MERGE BY MADHVI ON 22TH JAN 2018 , MADE BY YOGITA ON 20TH JAN 2018--------- //
                case "F50264":
                    // PRODUCTS WISE SALES VS RETURNS , PPM
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "select trim(a.icode) as item_code,b.iname as item_name,b.cpartno as partNo,b.unit,sum(sale) as sale,sum(return) as return from (select trim(icode) as icode,iqtyout as sale,0 as return from ivoucher where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and nvl(iqtyout,0)>0 union all select trim(icode) as icode,0 as sale, (iqtyin+nvl(rej_rw,0)) as return from ivoucher where branchcd='" + mbr + "' and type='04' and vchdate " + xprdrange + " and store='Y') a,item b where trim(a.icode)=trim(b.icode) and a.iCODE like '" + party_cd + "%' group by trim(a.icode),b.iname,b.cpartno,b.unit order by item_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Products Wise Sales Vs Returns,PPM from " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F50265":
                    // CUSTOMER, PRODUCTS WISE SALES VS RETURNS , PPM
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select trim(a.acode) as Cust_Code,c.ANAME AS CUST_NAME, trim(a.icode) as item_code,b.iname as item_name,b.cpartno as partNo,b.unit,sum(sale) as sale,sum(return) as return from (select trim(acode) as acode, trim(icode) as icode,iqtyout as sale,0 as return from ivoucher where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " /*and nvl(iqtyout,0)>0*/ union all select trim(acode) as  acode,trim(icode) as icode,0 as sale, (iqtyin+nvl(rej_rw,0)) as return from ivoucher where branchcd='" + mbr + "' and type='04' and vchdate " + xprdrange + " and store='Y') a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.aCODE like '" + party_cd + "%' and a.iCODE like '" + part_cd + "%' group by TRIM(a.acode),c.aname, trim(a.icode),b.iname,b.cpartno,b.unit order by Cust_Code,item_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer,Products Wise Sales Vs Returns,PPM from " + value1 + " To " + value2 + "", frm_qstr);
                    break;
                // ---------------------------------------------------------------------------------------- //               

                case "F50149": // BRN=N PRD=N
                    #region CLPL Sales Report By Madhvi
                    dtm = new DataTable();
                    dtm.Columns.Add("Agent", typeof(string));
                    dtm.Columns.Add("Yearly_Tgt", typeof(double));
                    dtm.Columns.Add("Monthly_Tgt", typeof(double));
                    dtm.Columns.Add("Today_Sale_UnitI", typeof(double));
                    dtm.Columns.Add("Till_Dt_Sale_UnitI", typeof(double));
                    dtm.Columns.Add("Monthly_Sale_UnitI", typeof(double));
                    dtm.Columns.Add("Today_Sale_UnitII", typeof(double));
                    dtm.Columns.Add("Till_Dt_Sale_UnitII", typeof(double));
                    dtm.Columns.Add("Monthly_Sale_UnitII", typeof(double));
                    dtm.Columns.Add("Today_Sale_UnitIII", typeof(double));
                    dtm.Columns.Add("Till_Dt_Sale_UnitIII", typeof(double));
                    dtm.Columns.Add("Monthly_Sale_UnitIII", typeof(double));
                    dtm.Columns.Add("Total_Sale", typeof(double));
                    dtm.Columns.Add("Bal_Tgt", typeof(double));
                    dtm.Columns.Add("Sale_Per", typeof(double));

                    mq0 = "";
                    mq0 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                    string yesterday = "";
                    //yesterday = System.DateTime.Now.Date.ToString("MMM");
                    date1 = Convert.ToDateTime(todt);
                    yesterday = date1.ToString("MMM");
                    mq1 = "";
                    //mq1 = System.DateTime.Now.Date.ToString("MM");
                    mq1 = date1.ToString("MM/yyyy");
                    // mq2 = System.DateTime.Now.Date.AddDays(-1).ToString("dd/MM/yyyy");
                    mq2 = todt;
                    year2 = (Convert.ToInt32(year) + 1).ToString();

                    // MONTHLY SALE
                    dt5 = new DataTable();
                    mq5 = "SELECT A.BRANCH0,SUM(GROSS0) AS MGROSS0,A.BRANCH1,SUM(GROSS1) AS MGROSS1,A.BRANCH2,SUM(GROSS2) AS MGROSS2,F.BSSCH,T.NAME FROM (SELECT TRIM(A.BRANCHCD) AS BRANCH0,TRIM(A.ACODE) AS BSSCH,A.BILL_TOT AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='00' AND A.TYPE LIKE '4%' AND a.vchdate between to_date('01/" + mq1 + "','dd/MM/yyyy') and to_date('" + mq2 + "','dd/MM/yyyy') UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,TRIM(A.BRANCHCD) AS BRANCH1,A.BILL_TOT AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='01' AND A.TYPE LIKE '4%' AND a.vchdate between to_date('01/" + mq1 + "','dd/MM/yyyy') and to_date('" + mq2 + "','dd/MM/yyyy') UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,TRIM(A.BRANCHCD) AS BRANCH2,A.BILL_TOT AS GROSS2 FROM SALE A WHERE A.BRANCHCD='02' AND A.TYPE LIKE '4%' AND a.vchdate between to_date('01/" + mq1 + "','dd/MM/yyyy') and to_date('" + mq2 + "','dd/MM/yyyy'))A,FAMST F,TYPEGRP T WHERE TRIM(A.BSSCH)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' GROUP BY A.BRANCH0,A.BRANCH1,A.BRANCH2,F.BSSCH,T.NAME ORDER BY T.NAME";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq5);

                    //YEARLY AND MONTHLY TGT 
                    dt2 = new DataTable();
                    mq3 = "SELECT TRIM(ACODE) AS BSSCH,SUM(DAY1) AS APR,SUM(DAY2) AS MAY,SUM(DAY3) AS JUN,SUM(DAY4) AS JUL,SUM(DAY5) AS AUG,SUM(DAY6) AS SEP,SUM(DAY7) AS OCT,SUM(DAY8) AS NOV,SUM(DAY9) AS DEC,SUM(DAY10) AS JAN,SUM(DAY11) AS FEB,SUM(DAY12) AS MAR ,SUM(TOTAL) AS TOT FROM SCHEDULE WHERE BRANCHCD NOT IN ('DD','88') AND TYPE='98' AND VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('31/03/" + year2 + "','DD/MM/YYYY')  GROUP BY TRIM(ACODE) ORDER BY BSSCH";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq3);

                    // TODAY SALE
                    dt1 = new DataTable();
                    // COMMENTED ON 04 MAY 2018 AS PREV DAY SALE NEEDS TO SHOW
                    // mq1 = "SELECT A.BRANCH0,SUM(GROSS0) AS GROSS0,A.BRANCH1,SUM(GROSS1) AS GROSS1,A.BRANCH2,SUM(GROSS2) AS GROSS2,F.BSSCH,T.NAME FROM (SELECT TRIM(A.BRANCHCD) AS BRANCH0,TRIM(A.ACODE) AS BSSCH,A.BILL_TOT AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='00' AND A.TYPE LIKE '4%' AND  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq0 + "' UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,TRIM(A.BRANCHCD) AS BRANCH1,A.BILL_TOT AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='01' AND A.TYPE LIKE '4%' AND  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq0 + "' UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,TRIM(A.BRANCHCD) AS BRANCH2,A.BILL_TOT AS GROSS2 FROM SALE A WHERE A.BRANCHCD='02' AND A.TYPE LIKE '4%' AND  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq0 + "')A,FAMST F,TYPEGRP T WHERE TRIM(A.BSSCH)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A'  GROUP BY A.BRANCH0,A.BRANCH1,A.BRANCH2,F.BSSCH,T.NAME";
                    mq4 = "SELECT A.BRANCH0,SUM(GROSS0) AS GROSS0,A.BRANCH1,SUM(GROSS1) AS GROSS1,A.BRANCH2,SUM(GROSS2) AS GROSS2,F.BSSCH,T.NAME FROM (SELECT TRIM(A.BRANCHCD) AS BRANCH0,TRIM(A.ACODE) AS BSSCH,A.BILL_TOT AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='00' AND A.TYPE LIKE '4%' AND  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq2 + "' UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,TRIM(A.BRANCHCD) AS BRANCH1,A.BILL_TOT AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='01' AND A.TYPE LIKE '4%' AND  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq2 + "' UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,TRIM(A.BRANCHCD) AS BRANCH2,A.BILL_TOT AS GROSS2 FROM SALE A WHERE A.BRANCHCD='02' AND A.TYPE LIKE '4%' AND  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq2 + "')A,FAMST F,TYPEGRP T WHERE TRIM(A.BSSCH)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A'  GROUP BY A.BRANCH0,A.BRANCH1,A.BRANCH2,F.BSSCH,T.NAME";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq4);

                    // TILL DATE SALE
                    dt = new DataTable();
                    // COMMENTED ON 04 MAY 2018 AS PREV DAY SALE NEEDS TO SHOW
                    // squery = "SELECT A.BRANCH0,SUM(GROSS0) AS GROSS0,A.BRANCH1,SUM(GROSS1) AS GROSS1,A.BRANCH2,SUM(GROSS2) AS GROSS2,F.BSSCH,T.NAME FROM (SELECT TRIM(A.BRANCHCD) AS BRANCH0,TRIM(A.ACODE) AS BSSCH,A.BILL_TOT AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='00' AND A.TYPE LIKE '4%' AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq0 + "','DD/MM/YYYY') UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,TRIM(A.BRANCHCD) AS BRANCH1,A.BILL_TOT AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='01' AND A.TYPE LIKE '4%' AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq0 + "','DD/MM/YYYY') UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,TRIM(A.BRANCHCD) AS BRANCH2,A.BILL_TOT AS GROSS2 FROM SALE A WHERE A.BRANCHCD='02' AND A.TYPE LIKE '4%' AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq0 + "','DD/MM/YYYY'))A,FAMST F,TYPEGRP T WHERE TRIM(A.BSSCH)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' GROUP BY A.BRANCH0,A.BRANCH1,A.BRANCH2,F.BSSCH,T.NAME ORDER BY T.NAME";
                    SQuery = "SELECT A.BRANCH0,SUM(GROSS0) AS GROSS0,A.BRANCH1,SUM(GROSS1) AS GROSS1,A.BRANCH2,SUM(GROSS2) AS GROSS2,F.BSSCH,T.NAME FROM (SELECT TRIM(A.BRANCHCD) AS BRANCH0,TRIM(A.ACODE) AS BSSCH,A.BILL_TOT AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='00' AND A.TYPE LIKE '4%' AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq2 + "','DD/MM/YYYY') UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,TRIM(A.BRANCHCD) AS BRANCH1,A.BILL_TOT AS GROSS1,'02' AS BRANCH2,0 AS GROSS2 FROM SALE A WHERE A.BRANCHCD='01' AND A.TYPE LIKE '4%' AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq2 + "','DD/MM/YYYY') UNION ALL SELECT '00' AS BRANCH0,TRIM(A.ACODE) AS BSSCH,0 AS GROSS0,'01' AS BRANCH1,0 AS GROSS1,TRIM(A.BRANCHCD) AS BRANCH2,A.BILL_TOT AS GROSS2 FROM SALE A WHERE A.BRANCHCD='02' AND A.TYPE LIKE '4%' AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq2 + "','DD/MM/YYYY'))A,FAMST F,TYPEGRP T WHERE TRIM(A.BSSCH)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' GROUP BY A.BRANCH0,A.BRANCH1,A.BRANCH2,F.BSSCH,T.NAME ORDER BY T.NAME";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0;
                    dr1 = null;
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt);
                        dt3 = new DataTable(); ;
                        dt3 = view1.ToTable(true, "bssch");

                        foreach (DataRow dr in dt3.Rows)
                        {
                            DataView view2 = new DataView(dt, "bssch='" + dr["bssch"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt4 = new DataTable();
                            dt4 = view2.ToTable();
                            dr1 = dtm.NewRow();
                            foreach (DataRow drs in dt4.Rows)
                            {
                                mq3 = "0"; mq4 = "0"; mq5 = "0"; mq6 = "0"; mq7 = "0"; mq8 = "0"; mq9 = "0"; mq10 = "0"; mq11 = "0"; mq12 = "0"; mq13 = "0"; mq14 = "0"; mq15 = "0"; mq16 = "0";
                                if (dt1.Rows.Count > 0)
                                {
                                    mq3 = fgen.seek_iname_dt(dt1, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "gross0");
                                    mq4 = fgen.seek_iname_dt(dt1, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "gross1");
                                    mq5 = fgen.seek_iname_dt(dt1, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "gross2");
                                }
                                if (dt2.Rows.Count > 0)
                                {
                                    mq11 = fgen.seek_iname_dt(dt2, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "tot");
                                    mq12 = fgen.seek_iname_dt(dt2, "bssch='" + drs["Bssch"].ToString().Trim() + "'", yesterday);
                                }
                                if (dt5.Rows.Count > 0) //MONTH DATA
                                {
                                    mq13 = fgen.seek_iname_dt(dt5, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "mgross0");
                                    mq14 = fgen.seek_iname_dt(dt5, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "mgross1");
                                    mq15 = fgen.seek_iname_dt(dt5, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "mgross2");
                                }
                                db3 += fgen.make_double(mq3);
                                db5 += fgen.make_double(mq4);
                                db7 += fgen.make_double(mq5);
                                db4 += fgen.make_double(drs["gross0"].ToString().Trim());
                                db6 += fgen.make_double(drs["gross1"].ToString().Trim());
                                db8 += fgen.make_double(drs["gross2"].ToString().Trim());
                                db13 += fgen.make_double(mq13);
                                db14 += fgen.make_double(mq14);
                                db15 += fgen.make_double(mq15);

                                dr1["Agent"] = drs["name"].ToString().Trim();
                                dr1["Yearly_Tgt"] = mq11;
                                dr1["Monthly_Tgt"] = mq12;
                                dr1["Today_Sale_UnitI"] = mq3;
                                dr1["Till_Dt_Sale_UnitI"] = fgen.make_double(drs["gross0"].ToString().Trim());
                                dr1["Monthly_Sale_UnitI"] = mq13;
                                dr1["Today_Sale_UnitII"] = mq4;
                                dr1["Till_Dt_Sale_UnitII"] = fgen.make_double(drs["gross1"].ToString().Trim());
                                dr1["Monthly_Sale_UnitII"] = mq14;
                                dr1["Today_Sale_UnitIII"] = mq5;
                                dr1["Till_Dt_Sale_UnitIII"] = fgen.make_double(drs["gross2"].ToString().Trim());
                                dr1["Monthly_Sale_UnitIII"] = mq15;
                                // mq7 = (fgen.make_double(drs["gross0"].ToString().Trim()) + fgen.make_double(drs["gross1"].ToString().Trim()) + fgen.make_double(drs["gross2"].ToString().Trim())).ToString();
                                mq16 = (fgen.make_double(mq13) + fgen.make_double(mq14) + fgen.make_double(mq15)).ToString();
                                dr1["Total_Sale"] = mq16;
                                // mq8 = (fgen.make_double(mq12) - fgen.make_double(mq7)).ToString();
                                mq8 = (fgen.make_double(mq12) - fgen.make_double(mq16)).ToString();
                                dr1["Bal_Tgt"] = mq8;
                                // mq10 = (Math.Round(((fgen.make_double(mq8) / fgen.make_double(mq7))), 2)).ToString().Replace("Infinity", "0");
                                mq10 = (Math.Round((Convert.ToDouble(mq16) / Convert.ToDouble(mq12)) * 100, 2)).ToString().Replace("Infinity", "0").Replace("NaN", "0");
                                dr1["Sale_Per"] = fgen.make_double(mq10);
                            }
                            dtm.Rows.Add(dr1);
                        }
                    }
                    dr1 = dtm.NewRow();
                    db1 = 0; db2 = 0; db3 = 0;
                    foreach (DataColumn dc in dtm.Columns)
                    {
                        db1 = 0;
                        if (dc.Ordinal == 0)
                        {

                        }
                        else if (dc.Ordinal == 14)
                        {
                            //db1 = Math.Round((db3 / db2), 2); // SALE PER
                            db1 = Math.Round((db2 / db3), 2); // SALE PER
                            dr1[dc] = db1;
                        }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(dtm.Compute(mq1, "").ToString());
                            if (dc.Ordinal == 12)
                            {
                                db2 = db1;
                            }
                            if (dc.Ordinal == 2)
                            {
                                db3 = db1;
                            }
                            dr1[dc] = db1;
                        }
                    }

                    if (dtm.Rows.Count > 0)
                    {
                        dr1["Agent"] = "Total";
                        dtm.Rows.InsertAt(dr1, 0);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dtm;
                    fgen.Fn_open_rptlevel("Sales Report From " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "F50150": // BRN=N PRD=Y
                    #region CLPL Daily Sales Summary Person Wise Report By Madhvi
                    SQuery = "SELECT A.BRANCHCD,B.NAME AS BRANCH,T.NAME,SUM(A.AMT_SALE) AS SALE_VALUE,SUM(A.AMT_EXC+NVL(RVALUE,0)) AS GST_OtherCharges,SUM(A.BILL_TOT) AS GROSS_AMT FROM SALE A,FAMST F,TYPEGRP T,TYPE B  WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND T.ID='A' AND B.ID='B' /*AND A." + branch_Cd + "*/ AND A.BRANCHCD NOT IN ('DD','88') AND A.TYPE LIKE '4%' AND  A.vchdate " + xprdrange + " GROUP BY T.NAME,B.NAME,A.BRANCHCD ORDER BY T.NAME";
                    SQuery = "SELECT T.NAME,SUM(A.AMT_SALE) AS SALE_VALUE,SUM(A.AMT_EXC+NVL(RVALUE,0)) AS GST_OtherCharges,SUM(A.BILL_TOT) AS GROSS_AMT FROM SALE A,FAMST F,TYPEGRP T  WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' /*AND A." + branch_Cd + "*/ AND A.BRANCHCD NOT IN ('DD','88') AND A.TYPE LIKE '4%' AND  A.vchdate " + xprdrange + " GROUP BY T.NAME ORDER BY T.NAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Daily Sales Summary Person Wise Report From " + fromdt + " To " + todt, frm_qstr);
                    #endregion
                    break;

                case "F50155old": // BRN=N PRD=N
                    #region CLPL Collection OLD Report
                    dtm = new DataTable();
                    dtm.Columns.Add("srno", typeof(Int32));
                    dtm.Columns.Add("Agent", typeof(string));
                    dtm.Columns.Add("Outstanding", typeof(double));
                    dtm.Columns.Add("Target", typeof(double));
                    dtm.Columns.Add("Today_Coll", typeof(double));
                    dtm.Columns.Add("Monthly_Coll", typeof(double));
                    dtm.Columns.Add("Bal_Tgt", typeof(double));
                    dtm.Columns.Add("PDC", typeof(double));
                    dtm.Columns.Add("Coll_Per", typeof(double));
                    dtm.Columns.Add("Chq_Amt", typeof(double));

                    dt2 = new DataTable();
                    dt2.Columns.Add("srno", typeof(Int32));
                    dt2.Columns.Add("Agent", typeof(string));
                    dt2.Columns.Add("Cheque_No", typeof(string));
                    dt2.Columns.Add("Cheque_Dt", typeof(string));
                    dt2.Columns.Add("Debit_Amount", typeof(double));
                    dt2.Columns.Add("Credit_Amount", typeof(double));

                    mq0 = "";
                    //   mq0 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                    mq0 = todt;
                    mq1 = "";
                    // mq1 = System.DateTime.Now.Date.ToString("MM");
                    mq1 = (Convert.ToDateTime(todt)).ToString("MM/yyyy");
                    mq10 = "";
                    //mq10 = System.DateTime.Now.Date.AddDays(-1).ToString("dd/MM/yyyy");
                    mq10 = (Convert.ToDateTime(todt).AddDays(-1)).ToString("dd/MM/yyyy");
                    // string xserver_dt = System.DateTime.Now.Date.ToString("MM/yyyy");
                    string xserver_dt = (Convert.ToDateTime(todt)).ToString("MM/yyyy");
                    //string xserver_dt1 = System.DateTime.Now.Date.ToString("MMM");
                    string xserver_dt1 = (Convert.ToDateTime(todt)).ToString("MMM");
                    year2 = (Convert.ToInt32(year) + 1).ToString();

                    // CHQ BOUNCE
                    dt9 = new DataTable();
                    dt9 = fgen.getdata(frm_qstr, co_cd, "select a.type,a.acode,f.bssch,t.name,a.refnum,to_char(a.refdate,'dd/mm/yyyy') as refdate,a.dramt,a.cramt from voucher a ,famst f,typegrp t where trim(a.acode)=trim(f.acode) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' and a.branchcd!='88' and substr(trim(a.type),1,1) in ('1','2') and a.vchdate>=to_date('05/08/2018','dd/mm/yyyy') and substr(trim(a.acode),1,2)='16' order by type");

                    // CHQ AMT
                    dt8 = new DataTable();
                    string er4 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.CRAMT) AS AMT FROM VOUCHER A,FAMST F WHERE TRIM(A.RCODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='2' AND SUBSTR(TRIM(A.RCODE),1,2)='16' GROUP BY TRIM(F.BSSCH)";
                    dt8 = fgen.getdata(frm_qstr, co_cd, er4);

                    // TARGET
                    dt7 = new DataTable();
                    string er3 = "SELECT TRIM(ACODE) AS BSSCH,SUM(DAY1) AS APR,SUM(DAY2) AS MAY,SUM(DAY3) AS JUN,SUM(DAY4) AS JUL,SUM(DAY5) AS AUG,SUM(DAY6) AS SEP,SUM(DAY7) AS OCT,SUM(DAY8) AS NOV,SUM(DAY9) AS DEC,SUM(DAY10) AS JAN,SUM(DAY11) AS FEB,SUM(DAY12) AS MAR FROM SCHEDULE WHERE TYPE='98' AND VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('31/03/" + year2 + "','DD/MM/YYYY') GROUP BY TRIM(ACODE) ORDER BY BSSCH";
                    dt7 = fgen.getdata(frm_qstr, co_cd, er3);

                    // PDC
                    dt6 = new DataTable();
                    string er2 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.CRAMT) AS PDC FROM VOUCHER A,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1' AND SUBSTR(TRIM(A.ACODE),1,2)='16' AND VCHDATE>TO_DATE('" + mq10 + "','DD/MM/YYYY') GROUP BY TRIM(F.BSSCH)";
                    dt6 = fgen.getdata(frm_qstr, co_cd, er2);

                    // OUTSTANDING
                    dt5 = new DataTable();
                    string er1 = "SELECT M.* FROM (SELECT  BSSCH,SUM(NET) AS NET FROM (SELECT '00' AS DTREP, '" + mq0 + "' AS SDATE,TRIM(A.ACODE) AS ACODE,A.BRANCHCD,B.PAYMENT,B.ANAME AS BR_NAME,A.INVNO,A.INVDATE,TO_DATE('" + mq0 + "','DD/MM/YYYY')-(A.INVDATE+B.PAY_NUM) AS ODUEDAYS,A.INVDATE+B.PAY_NUM AS DUE_DT,A.DRAMT,A.CRAMT,A.DRAMT-A.CRAMT AS NET,TRIM(B.BSSCH) AS BSSCH FROM RECDATA A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd not in ('DD','88')) GROUP BY BSSCH) M WHERE  M.NET!=0";
                    dt5 = fgen.getdata(frm_qstr, co_cd, er1);

                    // TODAY COLLECTION
                    dt1 = new DataTable();
                    mq1 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.CRAMT-A.DRAMT) AS COLL FROM VOUCHER A,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1' AND SUBSTR(A.ACODE,1,2) IN('16') AND A.VCHDATE=TO_DATE('" + mq0 + "','dd/mm/yyyy')  GROUP BY TRIM(F.BSSCH)";
                    mq1 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.DRAMT) AS COLL FROM VOUCHER A,FAMST F WHERE TRIM(A.RCODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1'  AND TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq10 + "' AND SUBSTR(TRIM(A.RCODE),1,2)='16' GROUP BY TRIM(F.BSSCH)";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    // TILL DATE COLLECTION NOW MONTHLY COLLECTION
                    dt = new DataTable();
                    SQuery = "SELECT TRIM(F.BSSCH) AS BSSCH,T.NAME,SUM(A.CRAMT-A.DRAMT) AS COLL FROM VOUCHER A,FAMST F,TYPEGRP T WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A'  AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1' AND SUBSTR(A.ACODE,1,2) IN('16') AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq0 + "','DD/MM/YYYY') GROUP BY TRIM(F.BSSCH),T.NAME";
                    SQuery = "SELECT TRIM(F.BSSCH) AS BSSCH,T.NAME,SUM(A.DRAMT) AS COLL FROM VOUCHER A,FAMST F,TYPEGRP T WHERE TRIM(A.RCODE)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1'  AND TO_CHAR(A.VCHDATE,'MM/YYYY')='" + xserver_dt + "' AND SUBSTR(TRIM(A.RCODE),1,2)='16' GROUP BY TRIM(F.BSSCH),T.NAME ORDER BY T.NAME";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0;
                    int count = 1;

                    if (dt.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt);
                        dt3 = new DataTable(); ;
                        dt3 = view1.ToTable(true, "bssch");

                        foreach (DataRow dr in dt3.Rows)
                        {
                            DataView view2 = new DataView(dt, "bssch='" + dr["bssch"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt4 = new DataTable();
                            dt4 = view2.ToTable();
                            dr1 = dtm.NewRow();
                            foreach (DataRow drs in dt4.Rows)
                            {
                                mq3 = "0"; mq4 = "0"; mq5 = "0"; mq6 = "0"; mq7 = "0";
                                if (dt1.Rows.Count > 0)
                                {
                                    mq3 = fgen.seek_iname_dt(dt1, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "COLL");
                                }

                                if (dt5.Rows.Count > 0)
                                {
                                    mq4 = fgen.seek_iname_dt(dt5, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "NET");
                                }

                                if (dt6.Rows.Count > 0)
                                {
                                    mq5 = fgen.seek_iname_dt(dt6, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "PDC");
                                }

                                if (dt7.Rows.Count > 0)
                                {
                                    mq6 = fgen.seek_iname_dt(dt7, "bssch='" + drs["Bssch"].ToString().Trim() + "'", xserver_dt1.ToUpper());
                                }

                                if (dt8.Rows.Count > 0)
                                {
                                    mq7 = fgen.seek_iname_dt(dt8, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "AMT");
                                }

                                db3 += fgen.make_double(mq3);
                                db4 += fgen.make_double(drs["COLL"].ToString().Trim());
                                dr1["srno"] = count;
                                dr1["agent"] = drs["name"].ToString().Trim();
                                dr1["outstanding"] = mq4;
                                dr1["target"] = mq6;
                                dr1["Today_Coll"] = mq3;
                                dr1["Monthly_Coll"] = drs["COLL"].ToString().Trim();
                                dr1["Bal_Tgt"] = (fgen.make_double(mq6) - fgen.make_double(drs["COLL"].ToString().Trim()));
                                db5 += fgen.make_double(mq6) - fgen.make_double(drs["COLL"].ToString().Trim());
                                dr1["PDC"] = mq5;
                                db6 += fgen.make_double(mq5);
                                d1 = Math.Round((fgen.make_double(drs["COLL"].ToString().Trim()) + fgen.make_double(mq5)) / fgen.make_double(mq6) * 100, 2);
                                dr1["Coll_Per"] = fgen.make_double(d1.ToString());
                                db7 += fgen.make_double(d1.ToString());
                                dr1["chq_amt"] = mq7;
                            }
                            dtm.Rows.Add(dr1);
                            count++;
                        }
                    }

                    dr1 = dtm.NewRow();
                    db1 = 0; db2 = 0; db3 = 0; db4 = 0;
                    foreach (DataColumn dc in dtm.Columns)
                    {
                        db1 = 0;
                        if (dc.Ordinal == 0 || dc.Ordinal == 1)
                        {

                        }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(dtm.Compute(mq1, "").ToString());
                            dr1[dc] = db1;
                        }
                    }

                    if (dtm.Rows.Count > 0)
                    {
                        dr1["Agent"] = "Total";
                        dtm.Rows.InsertAt(dr1, 0);
                    }

                    if (dt9.Rows.Count > 0)
                    {
                        view1 = new DataView(dt9);
                        dt10 = new DataTable();
                        dt10 = view1.ToTable(true, "acode", "refnum", "refdate");
                        count = 1; d1 = 1;
                        foreach (DataRow dr in dt10.Rows)
                        {
                            dtview = new DataView(dt9, "acode='" + dr["acode"].ToString().Trim() + "' and refnum='" + dr["refnum"].ToString().Trim() + "' and refdate='" + dr["refdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt11 = new DataTable();
                            dt11 = dtview.ToTable();
                            db1 = 0;
                            string bounce = "";
                            int i = 0;
                            dr1 = null;
                            if (dt11.Rows.Count > 1)
                            {
                                for (i = 0; i < dt11.Rows.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        db1 = fgen.make_double(dt11.Rows[i]["cramt"].ToString().Trim());
                                    }
                                    db2 = fgen.make_double(dt11.Rows[i]["dramt"].ToString().Trim());
                                    if (db1 == db2)
                                    {
                                        bounce = "YES";
                                    }
                                }
                                if (bounce == "YES")
                                {
                                    for (i = 0; i < dt11.Rows.Count; i++)
                                    {
                                        if (d1 == 1)
                                        {
                                            dr1 = dt2.NewRow();
                                            dr1["Agent"] = "Cheque Bounce Details";
                                            dt2.Rows.Add(dr1);
                                        }
                                        dr1 = dt2.NewRow();
                                        db1 = fgen.make_double(dt11.Rows[i]["cramt"].ToString().Trim());
                                        dr1["srno"] = count;
                                        dr1["Agent"] = dt11.Rows[i]["name"].ToString().Trim();
                                        dr1["Cheque_No"] = dt11.Rows[i]["refnum"].ToString().Trim();
                                        dr1["Cheque_Dt"] = dt11.Rows[i]["refdate"].ToString().Trim();
                                        dr1["Debit_Amount"] = dt11.Rows[i]["dramt"].ToString().Trim();//right
                                        dr1["Credit_Amount"] = dt11.Rows[i]["cramt"].ToString().Trim();//right
                                        dt2.Rows.Add(dr1);
                                        count++;
                                        d1++;
                                    }
                                }
                            }
                        }
                    }
                    mdt = new DataTable();
                    mdt.Merge(dtm);
                    mdt.Merge(dt2);
                    if (mdt.Rows.Count > 0)
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = mdt;
                        fgen.Fn_open_rptlevel("Collection Report From " + fromdt + " To " + todt + "", frm_qstr);
                    }
                    #endregion
                    break;

                case "F50155": // BRN=N PRD=Y
                    #region CLPL Collection Report
                    dtm = new DataTable();
                    dtm.Columns.Add("Srno", typeof(Int32));
                    dtm.Columns.Add("Agent", typeof(string));
                    dtm.Columns.Add("Outstanding", typeof(double));
                    dtm.Columns.Add("Target", typeof(double));
                    dtm.Columns.Add("Today_Coll", typeof(double));
                    dtm.Columns.Add("Monthly_Coll", typeof(double));
                    dtm.Columns.Add("Bal_Tgt", typeof(double));
                    dtm.Columns.Add("PDC", typeof(double));
                    dtm.Columns.Add("Coll_Per", typeof(double));
                    dtm.Columns.Add("Bounce_Chq_Amt", typeof(double));
                    dtm.Columns.Add("Net_Coll_Amt", typeof(double));

                    //dt2 = new DataTable(); // METHOD OF SHOWING CHQ BOUNCE AMT IS CHANGED NOW
                    //dt2.Columns.Add("srno", typeof(Int32));
                    //dt2.Columns.Add("Agent", typeof(string));
                    //dt2.Columns.Add("Party_Code", typeof(string));
                    //dt2.Columns.Add("Party", typeof(string));
                    //dt2.Columns.Add("Cheque_No", typeof(string));
                    //dt2.Columns.Add("Cheque_Dt", typeof(string));
                    //dt2.Columns.Add("Debit_Amount", typeof(double));
                    //dt2.Columns.Add("Credit_Amount", typeof(double));

                    mq0 = "";
                    mq0 = todt;
                    mq1 = "";
                    mq1 = (Convert.ToDateTime(todt)).ToString("MM/yyyy");
                    mq10 = "";
                    mq10 = (Convert.ToDateTime(todt).AddDays(-1)).ToString("dd/MM/yyyy");
                    xserver_dt = (Convert.ToDateTime(todt)).ToString("MM/yyyy");
                    xserver_dt1 = (Convert.ToDateTime(todt)).ToString("MMM");
                    year2 = (Convert.ToInt32(year) + 1).ToString();
                    // NOTE WRITTEN ON 26 OCT 2018 BY MADHVI
                    // HEADING IS BOUNCE CHQ AMT BUT DATA SHOWN IN THAT FIELD IS TOTAL CHQ AMT SO REPLACING IT WITH CHQ BOUNCE AMT BCOZ AS PER USER HE WANTS TOTAL MONTH COLLECTION - TOTAL CHQ BOUNCE AMT OF THAT MONTH

                    // CHQ BOUNCE
                    dt9 = new DataTable();
                    //ALREADY COMMENTED dt9 = fgen.getdata(frm_qstr, co_cd, "select a.type,a.acode,f.aname,f.bssch,t.name,a.refnum,to_char(a.refdate,'dd/mm/yyyy') as refdate,a.dramt,a.cramt from voucher a ,famst f,typegrp t where trim(a.acode)=trim(f.acode) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' and a.branchcd!='88' and substr(trim(a.type),1,1) in ('1','2') and to_char(a.vchdate,'MM/YYYY')='" + xserver_dt + "' and substr(trim(a.acode),1,2)='16' order by type");
                    // COMMENTED ON 26 OCT 2018 PLUS ORIGINAL BEFORE CHANGE DONE ON 26 OCT 2018 dt9 = fgen.getdata(frm_qstr, co_cd, "select a.type,a.acode,f.aname,f.bssch,t.name,a.refnum,to_char(a.refdate,'dd/mm/yyyy') as refdate,a.dramt,a.cramt from voucher a ,famst f,typegrp t where trim(a.acode)=trim(f.acode) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' and a.branchcd!='88' and substr(trim(a.type),1,1) in ('1','2') and a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and substr(trim(a.acode),1,2)='16' order by type");
                    // CHQ BOUNCE TYPE IS 34 SO FETCHING DATA FROM THAT TYPE ON 26 OCT 2018 
                    er5 = "select trim(f.bssch) as bssch,sum(a.dramt) as dramt,sum(a.cramt) as cramt from voucher a ,famst f,typegrp t where trim(a.acode)=trim(f.acode) and trim(f.bssch)=trim(t.type1) and t.id='A' and a.branchcd!='88' and a.type='34' and a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and substr(trim(a.acode),1,2)='16' group by trim(f.bssch) order by bssch";
                    dt9 = fgen.getdata(frm_qstr, co_cd, er5);

                    // CHQ AMT // READ NOTE
                    //dt8 = new DataTable();
                    //er4 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.CRAMT) AS AMT FROM VOUCHER A,FAMST F WHERE TRIM(A.RCODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='2' and /*to_char(vchdate,'MM/YYYY')='" + xserver_dt + "'*/ a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') AND SUBSTR(TRIM(A.RCODE),1,2)='16' GROUP BY TRIM(F.BSSCH)";
                    //dt8 = fgen.getdata(frm_qstr, co_cd, er4);

                    // TARGET
                    dt7 = new DataTable();
                    er3 = "SELECT TRIM(ACODE) AS BSSCH,SUM(DAY1) AS APR,SUM(DAY2) AS MAY,SUM(DAY3) AS JUN,SUM(DAY4) AS JUL,SUM(DAY5) AS AUG,SUM(DAY6) AS SEP,SUM(DAY7) AS OCT,SUM(DAY8) AS NOV,SUM(DAY9) AS DEC,SUM(DAY10) AS JAN,SUM(DAY11) AS FEB,SUM(DAY12) AS MAR FROM SCHEDULE WHERE TYPE='97' AND VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('31/03/" + year2 + "','DD/MM/YYYY') GROUP BY TRIM(ACODE) ORDER BY BSSCH";
                    dt7 = fgen.getdata(frm_qstr, co_cd, er3);

                    // PDC
                    dt6 = new DataTable();
                    er2 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.CRAMT) AS PDC FROM VOUCHER A,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1' AND SUBSTR(TRIM(A.ACODE),1,2)='16' AND VCHDATE>TO_DATE('" + mq10 + "','DD/MM/YYYY') GROUP BY TRIM(F.BSSCH)";
                    dt6 = fgen.getdata(frm_qstr, co_cd, er2);

                    // OUTSTANDING
                    dt5 = new DataTable();
                    er1 = "SELECT M.* FROM (SELECT  BSSCH,SUM(NET) AS NET FROM (SELECT '00' AS DTREP, '" + mq0 + "' AS SDATE,TRIM(A.ACODE) AS ACODE,A.BRANCHCD,B.PAYMENT,B.ANAME AS BR_NAME,A.INVNO,A.INVDATE,TO_DATE('" + mq0 + "','DD/MM/YYYY')-(A.INVDATE+B.PAY_NUM) AS ODUEDAYS,A.INVDATE+B.PAY_NUM AS DUE_DT,A.DRAMT,A.CRAMT,A.DRAMT-A.CRAMT AS NET,TRIM(B.BSSCH) AS BSSCH FROM RECDATA A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd not in ('DD','88')) GROUP BY BSSCH) M WHERE  M.NET!=0";
                    dt5 = fgen.getdata(frm_qstr, co_cd, er1);

                    // TODAY COLLECTION
                    dt1 = new DataTable();
                    mq1 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.CRAMT-A.DRAMT) AS COLL FROM VOUCHER A,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1' AND SUBSTR(A.ACODE,1,2) IN('16') AND A.VCHDATE=TO_DATE('" + mq0 + "','dd/mm/yyyy')  GROUP BY TRIM(F.BSSCH)";
                    mq1 = "SELECT TRIM(F.BSSCH) AS BSSCH,SUM(A.DRAMT) AS COLL FROM VOUCHER A,FAMST F WHERE TRIM(A.RCODE)=TRIM(F.ACODE) AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1'  AND TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq10 + "' AND SUBSTR(TRIM(A.RCODE),1,2)='16' GROUP BY TRIM(F.BSSCH)";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    // TILL DATE COLLECTION NOW MONTHLY COLLECTION
                    dt = new DataTable();
                    SQuery = "SELECT TRIM(F.BSSCH) AS BSSCH,T.NAME,SUM(A.CRAMT-A.DRAMT) AS COLL FROM VOUCHER A,FAMST F,TYPEGRP T WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A'  AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1' AND SUBSTR(A.ACODE,1,2) IN('16') AND A.VCHDATE BETWEEN TO_DATE('01/04/" + year + "','DD/MM/YYYY') AND TO_DATE('" + mq0 + "','DD/MM/YYYY') GROUP BY TRIM(F.BSSCH),T.NAME";
                    SQuery = "SELECT TRIM(F.BSSCH) AS BSSCH,T.NAME,SUM(A.DRAMT) AS COLL FROM VOUCHER A,FAMST F,TYPEGRP T WHERE TRIM(A.RCODE)=TRIM(F.ACODE) AND TRIM(F.BSSCH)=TRIM(T.TYPE1) AND T.ID='A' AND A.BRANCHCD!='DD' AND SUBSTR(A.TYPE,1,1)='1'  AND /*TO_CHAR(A.VCHDATE,'MM/YYYY')='" + xserver_dt + "'*/ a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') AND SUBSTR(TRIM(A.RCODE),1,2)='16' GROUP BY TRIM(F.BSSCH),T.NAME ORDER BY T.NAME";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0;
                    count = 1;

                    if (dt.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt);
                        dt3 = new DataTable(); ;
                        dt3 = view1.ToTable(true, "bssch");

                        foreach (DataRow dr in dt3.Rows)
                        {
                            DataView view2 = new DataView(dt, "bssch='" + dr["bssch"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt4 = new DataTable();
                            dt4 = view2.ToTable();
                            dr1 = dtm.NewRow();
                            foreach (DataRow drs in dt4.Rows)
                            {
                                mq3 = "0"; mq4 = "0"; mq5 = "0"; mq6 = "0"; mq7 = "0";
                                if (dt1.Rows.Count > 0)
                                {
                                    mq3 = fgen.seek_iname_dt(dt1, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "COLL");
                                }

                                if (dt5.Rows.Count > 0)
                                {
                                    mq4 = fgen.seek_iname_dt(dt5, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "NET");
                                }

                                if (dt6.Rows.Count > 0)
                                {
                                    mq5 = fgen.seek_iname_dt(dt6, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "PDC");
                                }

                                if (dt7.Rows.Count > 0)
                                {
                                    mq6 = fgen.seek_iname_dt(dt7, "bssch='" + drs["Bssch"].ToString().Trim() + "'", xserver_dt1.ToUpper());
                                }

                                // COMMENTIING REASON WRITTEN ABOVE
                                //if (dt8.Rows.Count > 0)
                                //{
                                //    mq7 = seek_iname_dt(dt8, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "AMT");
                                //}

                                if (dt9.Rows.Count > 0)
                                {
                                    mq7 = fgen.seek_iname_dt(dt9, "bssch='" + drs["Bssch"].ToString().Trim() + "'", "DRAMT");
                                }

                                db3 += fgen.make_double(mq3);
                                db4 += fgen.make_double(drs["COLL"].ToString().Trim());
                                dr1["srno"] = count;
                                dr1["agent"] = drs["name"].ToString().Trim();
                                dr1["outstanding"] = mq4;
                                dr1["target"] = mq6;
                                dr1["Today_Coll"] = mq3;
                                dr1["Monthly_Coll"] = drs["COLL"].ToString().Trim();
                                dr1["Bal_Tgt"] = (fgen.make_double(mq6) - fgen.make_double(drs["COLL"].ToString().Trim()));
                                db5 += fgen.make_double(mq6) - fgen.make_double(drs["COLL"].ToString().Trim());
                                dr1["PDC"] = mq5;
                                db6 += fgen.make_double(mq5);
                                d1 = Math.Round((fgen.make_double(drs["COLL"].ToString().Trim()) + fgen.make_double(mq5)) / fgen.make_double(mq6) * 100, 2);
                                dr1["Coll_Per"] = fgen.make_double(d1.ToString());
                                db7 += fgen.make_double(d1.ToString());
                                dr1["Bounce_Chq_Amt"] = mq7;
                                dr1["Net_Coll_Amt"] = fgen.make_double(drs["COLL"].ToString().Trim()) - fgen.make_double(mq7);
                            }
                            dtm.Rows.Add(dr1);
                            count++;
                        }
                    }

                    dr1 = dtm.NewRow();
                    db1 = 0; db2 = 0; db3 = 0; db4 = 0;
                    foreach (DataColumn dc in dtm.Columns)
                    {
                        db1 = 0;
                        if (dc.Ordinal == 0 || dc.Ordinal == 1)
                        {

                        }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(dtm.Compute(mq1, "").ToString());
                            dr1[dc] = db1;
                        }
                    }

                    if (dtm.Rows.Count > 0)
                    {
                        dr1["Agent"] = "Total";
                        dtm.Rows.InsertAt(dr1, 0);
                    }

                    // COMMENT ON 26 OCT 2018 AS CHQ BOUNCE DATA SHOWING METHOD IS CHANGED NOW
                    #region Chq Bounce Details
                    //if (dt9.Rows.Count > 0)
                    //{
                    //    view1 = new DataView(dt9);
                    //    dt10 = new DataTable();
                    //    dt10 = view1.ToTable(true, "acode", "refnum", "refdate");
                    //    count = 1; d1 = 1;
                    //    foreach (DataRow dr in dt10.Rows)
                    //    {
                    //        dtview = new DataView(dt9, "acode='" + dr["acode"].ToString().Trim() + "' and refnum='" + dr["refnum"].ToString().Trim() + "' and refdate='" + dr["refdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    //        dt11 = new DataTable();
                    //        dt11 = dtview.ToTable();
                    //        db1 = 0; db2 = 0;
                    //        string bounce = "";
                    //        int i = 0;
                    //        if (dt11.Rows.Count > 1)
                    //        {
                    //            mq4 = "SUM(CRAMT)";
                    //            mq5 = "SUM(DRAMT)";

                    //            db1 += fgen.make_double(dt11.Compute(mq4, "").ToString());
                    //            db2 += fgen.make_double(dt11.Compute(mq5, "").ToString());
                    //            if (db1 == db2)
                    //            {
                    //                bounce = "YES";
                    //            }
                    //        }
                    //        if (bounce == "YES")
                    //        {
                    //            for (i = 0; i < dt11.Rows.Count; i++)
                    //            {
                    //                if (d1 == 1)
                    //                {
                    //                    dr1 = dt2.NewRow();
                    //                    dr1["Agent"] = "Cheque Bounce Details";
                    //                    dt2.Rows.Add(dr1);
                    //                }
                    //                dr1 = dt2.NewRow();
                    //                dr1["srno"] = count;
                    //                dr1["Agent"] = dt11.Rows[i]["name"].ToString().Trim();
                    //                dr1["Party_Code"] = dt11.Rows[i]["acode"].ToString().Trim();
                    //                dr1["Party"] = dt11.Rows[i]["aname"].ToString().Trim();
                    //                dr1["Cheque_No"] = dt11.Rows[i]["refnum"].ToString().Trim();
                    //                dr1["Cheque_Dt"] = dt11.Rows[i]["refdate"].ToString().Trim();
                    //                dr1["Debit_Amount"] = dt11.Rows[i]["dramt"].ToString().Trim();//right
                    //                dr1["Credit_Amount"] = dt11.Rows[i]["cramt"].ToString().Trim();//right
                    //                dt2.Rows.Add(dr1);
                    //                count++;
                    //                d1++;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    mdt = new DataTable();
                    mdt.Merge(dtm);
                    //  mdt.Merge(dt2);
                    if (mdt.Rows.Count > 0)
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = mdt;
                        fgen.Fn_open_rptlevel("Collection Report From " + fromdt + " To " + todt + "", frm_qstr);
                    }
                    #endregion
                    break;

                case "F50157": // BRN=N PRD=N
                    #region CLPL Buying House
                    dtm = new DataTable();
                    //dtm.Columns.Add("Buyer_Name", typeof(string));
                    dtm.Columns.Add("Inv_No", typeof(string));
                    dtm.Columns.Add("Inv_Dt", typeof(string));
                    dtm.Columns.Add("Customer_Code", typeof(string));
                    dtm.Columns.Add("Customer_Name", typeof(string));
                    dtm.Columns.Add("Sales_Code", typeof(string));
                    dtm.Columns.Add("Bill_Total_Amount", typeof(double));
                    dtm.Columns.Add("Branch_Code", typeof(string));
                    dtm.Columns.Add("Branch_Name", typeof(string));

                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    mq1 = "select branchcd,trim(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,busi_potent,trim(busi_expect) as busi_expect from somas where branchcd!='DD' and type like '4%' and orddt " + xprdrange + " AND SUBSTR(TRIM(BUSI_POTENT),1,6)='" + mq2 + "' order by ordno";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);

                    // mq0 = "Select a.branchcd,a.Vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,a.acode,trim(a.ponum) as ponum,to_char(a.podate,'dd/mm/yyyy') as podate from ivoucher where branchcd!='DD' and type like '4%' and vchdate " + xprdrange + " order by Inv_No";
                    //  dt1 = new DataTable();
                    // dt1 = fgen.getdata(frm_qstr, co_cd, mq0);

                    dt = new DataTable();
                    SQuery = "Select trim(a.branchcd) as branchcd,trim(t.name) as name,trim(a.Vchnum) as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,trim(a.acode) as acode,trim(b.aname) as aname,trim(c.bill_tot) as bill_tot,trim(a.ponum) as ponum,to_char(a.podate,'dd/mm/yyyy') as podate from ivoucher a,sale c,famst b,type t where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(b.acode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd!='DD' and a.type like '4%' and a.vchdate " + xprdrange + " order by Inv_No";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dr1 = null; db1 = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dtm.NewRow();
                        ded1 = fgen.seek_iname_dt(dt2, "ordno='" + dt.Rows[i]["ponum"].ToString().Trim() + "' and orddt='" + dt.Rows[i]["podate"].ToString().Trim() + "'", "ordno");
                        if (ded1.Length == 6)
                        {
                            dr1["Inv_No"] = dt.Rows[i]["Inv_No"].ToString().Trim();
                            dr1["Inv_Dt"] = dt.Rows[i]["Inv_Date"].ToString().Trim();
                            dr1["Customer_Code"] = dt.Rows[i]["acode"].ToString().Trim();
                            dr1["Customer_Name"] = dt.Rows[i]["aname"].ToString().Trim();
                            dr1["Sales_Code"] = fgen.seek_iname_dt(dt2, "ordno='" + dt.Rows[i]["ponum"].ToString().Trim() + "' and orddt='" + dt.Rows[i]["podate"].ToString().Trim() + "'", "busi_expect");
                            dr1["Bill_Total_Amount"] = dt.Rows[i]["bill_tot"].ToString().Trim();
                            dr1["Branch_Code"] = dt.Rows[i]["branchcd"].ToString().Trim();
                            dr1["Branch_Name"] = dt.Rows[i]["name"].ToString().Trim();
                            dtm.Rows.Add(dr1);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        dv = new DataView(dtm);
                        dticode = new DataTable();
                        dticode = dv.ToTable(true, "Inv_No", "Inv_Dt", "Customer_Code", "Customer_Name", "Sales_Code", "Bill_Total_Amount", "Branch_Code", "Branch_Name");
                        dr1 = dticode.NewRow();
                        foreach (DataColumn dc in dticode.Columns)
                        {
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 6 || dc.Ordinal == 7)
                            {

                            }
                            else
                            {
                                mq1 = "sum(" + dc.ColumnName + ")";
                                db1 += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                dr1[dc] = db1;
                            }
                        }

                        dr1["Customer_Name"] = "Grand Total";
                        //  dticode.Rows.InsertAt(dr1, 0);
                        dticode.Rows.Add(dr1);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    Session["send_dt"] = dticode;
                    fgen.Fn_open_rptlevel("Buying House Report of " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + " From " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "F50306": //NITC STATIC ICON
                    #region nitc report
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (mq2.Length < 1)
                    {
                        fgen.msg("-", "AMSG", "Please select Invoice to generate CSV file");
                        return;
                    }
                    dt = new DataTable();
                    mq0 = "SELECT 'NITHYA PACKAGING PVT LTD' AS COMPANY_NAME, replace(TRIM(B.ADDR),',','') AS CO_ADDR,replace(TRIM(B.ADDR1),',','') AS CO_ADDR1,TRIM(B.PLACE) AS CO_ADDR3,TRIM(B.STATENM) AS CO_STATE,TRIM(B.COUNTRYNM) AS COUNTRYNM,TRIM(B.TELE) AS TELE,TRIM(B.GIR_NUM) AS CO_PAN,TRIM(B.GST_NO) AS CO_GST,A.ACODE,replace(TRIM(D.ANAME),',','') AS ANAME,replace(TRIM(D.ADDR1),',','') AS ADDR1,replace(TRIM(D.ADDR2),',','') AS ADDR2,replace(TRIM(D.ADDR3),',','') AS ADDR3,replace(TRIM(D.ADDR4),',','') AS ADDR4,TRIM(D.GST_NO) AS GST_NO,TRIM(D.GIRNO) AS PAN,TRIM(D.VENCODE) AS VENDOR_CODE,TRIM(D.BUYCODE) AS PLANT_CODE,TRIM(D.TELNUM) AS TELNUM,TRIM(D.EMAIL) AS EMAIL,TRIM(D.MOBILE) AS MOBILE,TRIM(D.PAYMENT) AS PAYMENT,TRIM(D.STATEN) AS STATEN,TRIM(D.COUNTRY) AS COUNTRY,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.VCHNUM,A.ICODE,TRIM(A.EXC_57F4) AS PART_NO,TRIM(C.HSCODE) AS HSCODE,TRIM(A.PURPOSE) AS PART_NAME,C.UNIT,A.IQTYOUT,A.IRATE,A.ICHGS AS DISCOUNT,TRIM(E.CURREN) AS CURRENCY,(CASE WHEN (A.CESS_PERCENT = 0) THEN 0 ELSE A.EXC_RATE END) AS CGST_RT,(CASE WHEN (A.CESS_PERCENT = 0) THEN 0 ELSE A.EXC_AMT END) AS CGST_AMT,A.CESS_PERCENT AS SGST_RT,A.CESS_PU AS SGST_AMT,(CASE WHEN (A.CESS_PERCENT = 0) THEN A.EXC_RATE  ELSE 0 END ) AS IGST_RT ,(CASE WHEN (A.CESS_PERCENT = 0) THEN A.EXC_AMT ELSE 0 END) as IGST_AMT ,A.IAMOUNT AS BASIC,(A.CESS_PU + A.EXC_AMT) AS TOT_TAX,(A.CESS_PU + A.IAMOUNT + A.EXC_AMT) AS TOTAL_AMOUNT,'-' AS TOT_AMT_WRD,'-' AS TOTAL_TAX_AMOUNT_WORD,TRIM(E.NARATION) AS REMARKS,TRIM(NVL(E.vehi_fitno,'-')) AS HEADER_NOTES,TRIM(NVL(E.EXC_NOT_NO,'-')) AS PERSON_REFERENCE, TRIM(NVL(E.Drv_licno,'-')) AS BUSINESS_AREA,TRIM(A.FINVNO) AS PONUM, TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,TRIM(NVL(A.BINNO,'-')) AS PO_LINE_NO,TRIM(NVL(E.MCOMMENT,'-')) AS DESCRIPTION,TRIM(NVL(E.MO_VEHI,'-')) AS VEHICLE_NO,TRIM(NVL(E.DRV_NAME,'-')) AS DRIVER_NAME,TRIM(NVL(E.INS_NO,'-')) AS TRANSPORTER_NAME,TRIM(NVL(E.ST_ENTFORM,'-')) AS E_WAY_BILL_NO, TO_CHAR(E.REACH_DT,'DD/MM/YYYY') AS DELIVERY_DATE,'BANK' AS PAYMENT_METHOD, TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS PAYMENT_DATE, TRIM(B.BANKNAME) AS BANK_NAME, nvl(trim(B.BANKAC),'-') AS ACCOUNT_NO, TRIM(B.IFSC_CODE) AS IFSC_CODE FROM TYPE B, IVOUCHER A, ITEM C, FAMST D , SALE E WHERE a.branchcd='" + mbr + "' and B.TYPE1='00' AND TRIM(B.TYPE1)=TRIM(A.BRANCHCD) and B.ID='B' AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.ACODE)= TRIM(D.ACODE) AND A.BRANCHCD||A.TYPE||A.VCHNUM||A.VCHDATE||TRIM(A.ACODE)=E.BRANCHCD||E.TYPE||E.VCHNUM||E.VCHDATE||TRIM(E.ACODE)AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) ='" + mq2 + "'";
                    mq0 = "SELECT 'NITHYA PACKAGING PVT LTD' AS COMPANY_NAME, replace(TRIM(B.ADDR),',','') AS CO_ADDR,replace(TRIM(B.ADDR1),',','') AS CO_ADDR1,TRIM(B.PLACE) AS CO_ADDR3,TRIM(B.STATENM) AS CO_STATE,TRIM(B.COUNTRYNM) AS COUNTRYNM,TRIM(B.TELE) AS TELE,TRIM(B.GIR_NUM) AS CO_PAN,TRIM(B.GST_NO) AS CO_GST,A.ACODE,replace(TRIM(D.ANAME),',','') AS ANAME,replace(TRIM(D.ADDR1),',','') AS ADDR1,replace(TRIM(D.ADDR2),',','') AS ADDR2,replace(TRIM(D.ADDR3),',','') AS ADDR3,replace(TRIM(D.ADDR4),',','') AS ADDR4,TRIM(D.GST_NO) AS GST_NO,TRIM(D.GIRNO) AS PAN,TRIM(D.VENCODE) AS VENDOR_CODE,TRIM(D.BUYCODE) AS PLANT_CODE,TRIM(D.TELNUM) AS TELNUM,TRIM(D.EMAIL) AS EMAIL,TRIM(D.MOBILE) AS MOBILE,TRIM(D.PAYMENT) AS PAYMENT,TRIM(D.STATEN) AS STATEN,TRIM(D.COUNTRY) AS COUNTRY,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.VCHNUM as vchnum,TRIM(A.ICODE) AS ICODE,TRIM(A.EXC_57F4) AS PART_NO,TRIM(C.HSCODE) AS HSCODE,TRIM(A.PURPOSE) AS PART_NAME,C.UNIT,A.IQTYOUT,A.IRATE,A.ICHGS AS DISCOUNT,TRIM(E.CURREN) AS CURRENCY,(CASE WHEN (A.CESS_PERCENT = 0) THEN 0 ELSE A.EXC_RATE END) AS CGST_RT,(CASE WHEN (A.CESS_PERCENT = 0) THEN 0 ELSE A.EXC_AMT END) AS CGST_AMT,A.CESS_PERCENT AS SGST_RT,A.CESS_PU AS SGST_AMT,(CASE WHEN (A.CESS_PERCENT = 0) THEN A.EXC_RATE  ELSE 0 END ) AS IGST_RT ,(CASE WHEN (A.CESS_PERCENT = 0) THEN A.EXC_AMT ELSE 0 END) as IGST_AMT ,A.IAMOUNT AS BASIC,(A.CESS_PU + A.EXC_AMT) AS TOT_TAX,(A.CESS_PU + A.IAMOUNT + A.EXC_AMT) AS TOTAL_AMOUNT,'-' AS TOT_AMT_WRD,'-' AS TOTAL_TAX_AMOUNT_WORD,TRIM(E.NARATION) AS REMARKS,TRIM(NVL(E.vehi_fitno,'-')) AS HEADER_NOTES,TRIM(NVL(E.EXC_NOT_NO,'-')) AS PERSON_REFERENCE, TRIM(NVL(E.Drv_licno,'-')) AS BUSINESS_AREA,TRIM(A.FINVNO) AS PONUM, TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,TRIM(NVL(A.BINNO,'-')) AS PO_LINE_NO,TRIM(NVL(E.MCOMMENT,'-')) AS DESCRIPTION,TRIM(NVL(E.MO_VEHI,'-')) AS VEHICLE_NO,TRIM(NVL(E.DRV_NAME,'-')) AS DRIVER_NAME,TRIM(NVL(E.INS_NO,'-')) AS TRANSPORTER_NAME,TRIM(NVL(E.ST_ENTFORM,'-')) AS E_WAY_BILL_NO, TO_CHAR(E.REACH_DT,'DD/MM/YYYY') AS DELIVERY_DATE,'BANK' AS PAYMENT_METHOD, TO_CHAR(E.DUE_DT,'DD/MM/YYYY') AS PAYMENT_DATE, TRIM(B.BANKNAME) AS BANK_NAME, nvl(trim(B.BANKAC),'-') AS ACCOUNT_NO, TRIM(B.IFSC_CODE) AS IFSC_CODE FROM TYPE B, IVOUCHER A, ITEM C, FAMST D , SALE E WHERE a.branchcd='" + mbr + "' and B.TYPE1='00' AND TRIM(B.TYPE1)=TRIM(A.BRANCHCD) and B.ID='B' AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.ACODE)= TRIM(D.ACODE) AND A.BRANCHCD||A.TYPE||A.VCHNUM||A.VCHDATE||TRIM(A.ACODE)=E.BRANCHCD||E.TYPE||E.VCHNUM||E.VCHDATE||TRIM(E.ACODE)AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) ='" + mq2 + "' and nvl(a.irate,0)>0";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns["TOT_AMT_WRD"].MaxLength = 400;
                        dt.Columns["TOTAL_TAX_AMOUNT_WORD"].MaxLength = 400;
                        string[] ponum; string po = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            db1 = 0; db2 = 0; mq3 = "";
                            po = dt.Rows[i]["ponum"].ToString().Trim().Replace("DT.", "<");
                            ponum = po.Split('<');
                            if (ponum.Length > 1)
                            {
                                dt.Rows[i]["ponum"] = ponum[0];
                            }
                            db1 = fgen.make_double(dt.Rows[i]["total_amount"].ToString().Trim());
                            if (db1 != 0)
                            {
                                mq1 = fgen.ConvertNumbertoWords(dt.Rows[i]["TOTAL_AMOUNT"].ToString().Trim()); //this fun convert number to word.
                                dt.Rows[i]["TOT_AMT_WRD"] = mq1;
                            }
                            else
                            {
                                dt.Rows[i]["TOT_AMT_WRD"] = "-";
                            }
                            /////////////////////////////
                            mq1 = "";
                            mq3 = dt.Rows[i]["VCHNUM"].ToString().Trim();
                            db2 = fgen.make_double(dt.Rows[i]["TOT_TAX"].ToString().Trim());
                            if (db2 != 0)
                            {
                                mq1 = fgen.ConvertNumbertoWords(dt.Rows[i]["TOT_TAX"].ToString().Trim()); //this fun convert number to word.
                                dt.Rows[i]["TOTAL_TAX_AMOUNT_WORD"] = mq1;
                            }
                            else
                            {
                                dt.Rows[i]["TOTAL_TAX_AMOUNT_WORD"] = "-";
                            }
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                        }
                        dr1 = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            db10 = 0;
                            if (dc.Ordinal == 42 || dc.Ordinal == 43 || dc.Ordinal == 44)
                            {
                                er1 = "sum(" + dc.ColumnName + ")";
                                db10 += fgen.make_double(dt.Compute(er1, "").ToString());
                                dr1[dc] = db10;
                            }
                            if (dc.Ordinal == 44)
                            {
                                dr1["TOT_AMT_WRD"] = fgen.ConvertNumbertoWords(db10.ToString());
                            }
                            if (dc.Ordinal == 43)
                            {
                                dr1["TOTAL_TAX_AMOUNT_WORD"] = fgen.ConvertNumbertoWords(db10.ToString());
                            }
                        }
                        dr1["PART_NAME"] = "TOTAL";
                        dt.Rows.InsertAt(dr1, dt.Rows.Count + 1);
                        Session["send_dt"] = dt;
                        fileName = co_cd + "_" + mq3 + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xls";
                        filepath = Server.MapPath("~/tej-base/Upload/") + fileName;
                        //fgen.CreateCSVFile(dt, filepath);
                        fgen.CreateCSVFile(dt, @"c:\TEJ_ERP\UPLOAD\" + fileName);

                        Session["FilePath"] = fileName;
                        Session["FileName"] = fileName;
                        Response.Write("<script>");
                        Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                        Response.Write("</script>");

                        //fgen.CreateCSVFile(dt, @"c:\TEJ_ERP\" + co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv");
                        //fgen.Fn_open_rptlevel("NITC REPORT from " + value1 + " To " + value2 + "", frm_qstr);
                        fgen.msg("-", "AMSG", "The file has been downloaded!!");
                    }
                    break;
                    #endregion

                ///iaij..........160ct2018
                case "F50301":
                    #region code for IAIJ ....creating text file by database
                    string tym = DateTime.Now.ToString("dd/mm/yyyy HH:MM:ss");

                    mq3 = "";
                    mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    mq0 = "select '856H' AS HEADER,A.VCHNUM ,TO_CHAR(A.VCHDATE,'YYYYMMDD HH:MM') AS VCHDATE,B.VENCODE AS VEND_CODE,'ZZ' AS FIX,'F159B' AS FIX1,'ZZ' AS FIX2,'0' AS FIX3,TO_CHAR(A.VCHDATE,'YYYYMMDD HH:MM') AS VCHDATE1,'INVOICE GROSS WT' AS INV_GROSS_WT,'UNIT' AS UNIT,'NETWT' AS NET_WEIGHT,'UNIT' AS UNIT1,'NIT' AS NIT,'MOTOR' AS MOTOR,'BLANK' AS BLNK1,'BLANK' AS BLNK2,A.VCHNUM AS INVNO,A.VCHNUM AS INV_NO,B.VENCODE AS VENDOR_CD,B.BUYCODE AS FORD_CODE,B.BUYCODE AS FORD_CD,'BIN' AS PCKG_TYPE,'90' AS PCKG_CODE,'2' AS NO_PCKG,'TL' AS FIXED,'1' AS FIXED1,A.MO_VEHI AS VEHICLE          from sale A ,FAMST B where TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq3 + "' AND TRIM(A.ACODE)=TRIM(B.ACODE)";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0); //FROM SALE

                    mq1 = "select '856R' AS HEADER,B.CPARTNO,'EA' AS FIXED,IQTYOUT AS ACTUAL_QTY,'EA' AS FIXED1 from ivoucher a,ITEM B where TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq3 + "' AND TRIM(A.ICODE)=TRIM(B.ICODE)";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);     //FROM IVCH

                    //  path = Server.MapPath("~/upload/IAIJ_" + tym.Replace(":", "_").Replace("/", "_") + ".txt");
                    path = "c:/tej_erp/Upload/IAIJ_" + tym.Replace(":", "_").Replace("/", "_") + ".txt";
                    if (dt.Rows.Count > 0)
                    {
                        StreamWriter w = new StreamWriter(path, true);
                        #region for first row heading
                        w.Write("Header No." + (char)9);
                        w.Write("Invoice No./ASN No." + (char)9);
                        w.Write("YYYYMMDDHHMM" + (char)9);
                        w.Write("VENDOR_CODE" + (char)9);
                        w.Write("FIX" + (char)9);
                        w.Write("FIX" + (char)9);
                        w.Write("FIX" + (char)9);
                        w.Write("FIX" + (char)9);
                        w.Write("icode" + (char)9);
                        w.Write("YYYYMMDDHHMM" + (char)9);
                        w.Write("INVOICE_GROSS WT" + (char)9);
                        w.Write("UNIT" + (char)9);
                        w.Write("NET WEIGHT" + (char)9);
                        w.Write("UNIT" + (char)9);
                        w.Write("NIT" + (char)9);
                        w.Write("MOTOR" + (char)9);
                        w.Write("BLANK" + (char)9);
                        w.Write("BLANK" + (char)9);
                        w.Write("INVOICE_NO" + (char)9);
                        w.Write("INVOICE_NO." + (char)9);
                        w.Write("VENDOR_CODE" + (char)9);
                        w.Write("FORD_CODE" + (char)9);
                        w.Write("FORD_CODE" + (char)9);
                        w.Write("PACKAGING TYPE" + (char)9);
                        w.Write("PACKAGING CODE." + (char)9);
                        w.Write("NO. OF PACKAGES" + (char)9);
                        w.Write("FIXED" + (char)9);
                        w.Write("FIXED" + (char)9);
                        w.Write("VEHICLE_NO." + (char)9);
                        w.WriteLine("");
                        int m = 0, n = 0;
                        /////filing value
                        w.Write(dt.Rows[0]["header"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["vchnum"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["vchdate"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["VEND_CODE"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FIX"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FIX1"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FIX2"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FIX3"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["VCHDATE1"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["INV_GROSS_WT"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["UNIT"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["NET_WEIGHT"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["UNIT1"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["NIT"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["MOTOR"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["BLNK1"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["BLNK2"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["INVNO"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["INV_NO"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["VENDOR_CD"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FORD_CODE"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FORD_CD"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["PCKG_TYPE"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["PCKG_CODE"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["NO_PCKG"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FIXED"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["FIXED1"].ToString().Trim() + (char)9);
                        w.Write(dt.Rows[0]["VEHICLE"].ToString().Trim() + (char)9);
                        w.WriteLine("");
                        #endregion
                        //////////////after fill sale data again create heaer for invoice
                        w.Write("Header No." + (char)9);
                        w.Write("Part No." + (char)9);
                        w.Write("FIXED" + (char)9);
                        w.Write("ACTUAL QTY" + (char)9);
                        w.Write("FIXED" + (char)9);
                        w.Write("COMMULATIVE QTY - FROM IST JANUARY" + (char)9);
                        w.WriteLine("");

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                /////(char)9   for space like tab in txt file
                                w.Write(dt1.Rows[i]["HEADER"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["CPARTNO"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["FIXED"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["ACTUAL_QTY"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["FIXED1"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["ACTUAL_QTY"].ToString().Trim() + (char)9); //COMMULATIVE QT
                                db = fgen.make_double(dt1.Rows[i]["ACTUAL_QTY"].ToString().Trim());
                                w.WriteLine("");
                            }
                            else if (i > m)
                            {
                                db = db + fgen.make_double(dt1.Rows[i]["ACTUAL_QTY"].ToString().Trim());
                                w.Write(dt1.Rows[i]["HEADER"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["CPARTNO"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["FIXED"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["ACTUAL_QTY"].ToString().Trim() + (char)9);
                                w.Write(dt1.Rows[i]["FIXED1"].ToString().Trim() + (char)9);
                                w.Write(db.ToString().Trim() + (char)9); //COMMULATIVE QT
                                w.WriteLine("");
                                m++;
                            }
                            else
                            { }
                        }
                        w.Flush();
                        w.Close();
                        try
                        {
                            filepath = path;
                            Session["FilePath"] = "iaij_" + tym.Replace(":", "_").Replace("/", "_") + ".txt";
                            Session["FileName"] = "iaij_" + tym.Replace(":", "_").Replace("/", "_") + ".txt";
                            Response.Write("<script>");
                            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                            Response.Write("</script>");
                        }
                        catch { }
                        fgen.msg("-", "AMSG", "Text file is Downloaded");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Data Not Found");
                    }
                    #endregion
                    break;

                case "F50307"://WPPL sg-tg report
                    #region sg-tg report
                    header_n = "SG-TG Report";
                    SQuery = "SELECT TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) as icode,trim(a.enqno) as enqno,TO_CHAR(a.enqdt,'DD/MM/YYYY') AS ENQDT,A.qty AS QTY,TRIM(C.INAME) as iname,c.mqty8 as sg,c.mqty9 as tg FROM COSTESTIMATE A, ITEM C  WHERE  TRIM(A.ICODE)=TRIM(C.ICODE) AND a.branchcd='" + mbr + "' and  A.TYPE='40'  AND a.vchdate " + xprdrange + " ORDER BY INAME,enqno ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    mq1 = "SELECT TRIM(VCHNUM) AS VCHNUM ,to_char(vchdate,'dd/mm/yyyy') as vchdate,TRIM(icode) AS ICODE,sum(is_number(col3)) as rej FROM INSPVCH WHERE BRANCHCD='" + mbr + "' AND TYPE='45' AND vchdate " + xprdrange + " GROUP BY TRIM(VCHNUM) ,to_char(vchdate,'dd/mm/yyyy') ,TRIM(icode)";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    mq2 = "SELECT DISTINCT TRIM(A.VCHNUM) AS VCHNUM ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,TRIM(a.icode) as icode,TRIM(A.ACODE) AS ACODE FROM COSTESTIMATE A WHERE a.branchcd='" + mbr + "' AND  A.TYPE='30' AND A.VCHDATE BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY')-100 AND TO_DATE('" + todt + "','DD/MM/YYYY') ";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    mq3 = "SELECT TRIM(ACODE) AS ACODE ,TRIM(ANAME) AS ANAME FROM FAMST ";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    DataTable ph_tbl = new DataTable();
                    dr1 = null;
                    ph_tbl.Columns.Add(new DataColumn("Serial_No", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("ERP_Code", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Production_Date", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Job_Ticket_No", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Item_Description", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("SG", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("TG", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("Quantity_manufactured", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("SG_Total", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("TG_Total", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("Total", typeof(double)));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = ph_tbl.NewRow();
                        dr1["Serial_No"] = i + 1;
                        dr1["ERP_Code"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr1["Production_Date"] = dt.Rows[i]["vchdate"].ToString().Trim();
                        dr1["Job_Ticket_No"] = dt.Rows[i]["enqno"].ToString().Trim();
                        dr1["Item_Description"] = dt.Rows[i]["iname"].ToString().Trim();
                        mq5 = fgen.seek_iname_dt(dt2, "vchnum='" + dt.Rows[i]["enqno"].ToString().Trim() + "' and vchdate='" + dt.Rows[i]["enqdt"].ToString().Trim() + "' and icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' ", "ACODE");
                        dr1["Customer_Name"] = fgen.seek_iname_dt(dt3, "ACODE='" + mq5 + "'", "ANAME");

                        dr1["SG"] = fgen.make_double(dt.Rows[i]["sg"].ToString().Trim());
                        dr1["TG"] = fgen.make_double(dt.Rows[i]["tg"].ToString().Trim());
                        double qty = 0;
                        double rej = 0;
                        qty = fgen.make_double(dt.Rows[i]["qty"].ToString().Trim());
                        rej = fgen.make_double(fgen.seek_iname_dt(dt1, " vchnum='" + dt.Rows[i]["vchnum"].ToString().Trim() + "' and vchdate='" + dt.Rows[i]["vchdate"].ToString().Trim() + "' and icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' ", "rej"));
                        dr1["Quantity_manufactured"] = fgen.make_double(qty.ToString()) + fgen.make_double(rej.ToString());
                        dr1["SG_Total"] = Math.Round((fgen.make_double(dr1["sg"].ToString()) * fgen.make_double(dr1["Quantity_manufactured"].ToString())) / 1000, 2);
                        dr1["TG_Total"] = Math.Round((fgen.make_double(dr1["tg"].ToString()) * fgen.make_double(dr1["Quantity_manufactured"].ToString())) / 1000, 2);
                        dr1["Total"] = Math.Round(fgen.make_double(dr1["sg_total"].ToString()) + fgen.make_double(dr1["tg_total"].ToString()), 2);
                        ph_tbl.Rows.Add(dr1);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = ph_tbl;
                    fgen.Fn_open_rptlevel(header_n + " From  " + fromdt + " To  " + todt + "", frm_qstr);
                    # endregion sg-tg
                    break;

                case "F50308":
                    #region
                    header_n = "Generate Invoice-TradeShift";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE").Length > 1)
                    {
                        SQuery = "SELECT DISTINCT 'WORTH PERIPHERALS LTD.UNIT-2'||T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number, TRIM(B.ANAME)||','||TRIM(B.ADDR1)||','||TRIM(B.ADDR2)||','||TRIM(B.ADDR3) AS PARTY_Name_and_AddR_Bill_To,B.GST_NO AS PARTY_GST_NO_BILL,GIRNO AS PARTY_PAN_BILL,B.BUYCODE AS Party_Plant_Code,trim(a.icode) as icode,A.INVNO AS INVNO,to_char(A.INVDATE,'dd/mm/yyyy') AS INVDATE,'INR' AS CURRENCY,B.STATEN||','||B.STAFFCD AS PLACE_SUPPLY,A.IAMOUNT AS AMOUNT,A.EXC_RATE,a.iopr, (CASE WHEN A.IOPR='IG' THEN A.EXC_RATE ELSE 0 END ) AS IGST_PERC,(CASE WHEN A.IOPR='CG' THEN A.EXC_RATE ELSE 0 END ) AS CGST_PERC,(CASE WHEN A.IOPR='CG' THEN A.CESS_PERCENT ELSE 0 END ) AS SGST_PERC,(CASE WHEN A.IOPR='CG' THEN a.exc_amt ELSE 0 END) as cgst_val,(CASE WHEN A.IOPR='CG' THEN a.exc_amt ELSE 0 END) as Sgst_val,(CASE WHEN A.IOPR='IG' THEN a.exc_amt ELSE 0 END) as Igst_val,'' AS HEADER_NOTE,'' AS TOTAL_AMT,TRIM(A.finvno) as ponum1,''AS BUSINESS_AREA,IS_NUMBER(A.binno) AS PO_LINE_NO,c.unit as UNIT ,A.IQTYOUT AS QTY,A.IRATE AS UNIT_PRICE,C.HSCODE AS HSN_NO,a.iamount AS TOTAL_LINE_AMOUNT,'' AS CREDIT_NOTE_NO,'' AS DEBIT_NOTE_NO,s.cscode, C.PRT_NM2||' '||C.PRT_NM3 as desc_,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podt,a.acode,trim(a.ponum)||to_char(a.podate,'dd/mm/yyyy')||trim(a.icode)||trim(a.acode)||trim(a.prnum) as fstr  FROM IVOUCHER A,FAMST B,ITEM C, TYPE T, sale s WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.BRANCHCD)=TRIM(T.TYPE1) and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') AND A.TYPE LIKE '4%' AND T.ID='B' AND A.IRATE > 0 and substr(trim(a.icode),1,2)!='59' AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy') in (" + part_cd + ") ORDER BY INVNO"; // and a.icode not like '59%'   004001144021/10/201816N01601144021/10/2018
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                        mq0 = "select distinct trim(ORDNO)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||trim(acode)||trim(cdrgno) as fstr, trim(icode) as icode,trim(acode) as acode,trim(ORDNO) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,SD from somas where branchcd='" + mbr + "' and type like '4%' ORDER BY ORDNO ";  //and orddt  between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, co_cd, mq0);

                        mq1 = "Select distinct trim(d.aname)||','||trim(d.addr1)||','||trim(d.addr2)||','||trim(d.addr3)||','||trim(d.addr4) as name,d.acode as acode,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d ";
                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                        ph_tbl = new DataTable();
                        ph_tbl.Columns.Add("Supplier_Comp_Name_Addr", typeof(string));
                        ph_tbl.Columns.Add("Supplier_Gst_No", typeof(string));
                        ph_tbl.Columns.Add("Supplier_Pan_No", typeof(string));
                        ////FROM FAMST or csmst
                        ph_tbl.Columns.Add("Party_Name_and_Addr_Bill_To", typeof(string));
                        ph_tbl.Columns.Add("Party_Name_and_Addr_Ship_To", typeof(string));
                        ph_tbl.Columns.Add("Party_Gst_No_Bill_To", typeof(string));
                        ph_tbl.Columns.Add("Party_Gst_No_Ship_To", typeof(string));
                        ph_tbl.Columns.Add("Party_Pan_No_Bill_To", typeof(string));
                        ph_tbl.Columns.Add("Party_Pan_No_Ship_To", typeof(string));
                        ph_tbl.Columns.Add("Party_Plant_Code", typeof(string));
                        //from ivch/....
                        ph_tbl.Columns.Add("Invoice_Dt", typeof(string));
                        ph_tbl.Columns.Add("Invoice_Number", typeof(string));
                        ph_tbl.Columns.Add("Currency", typeof(string));
                        ph_tbl.Columns.Add("Place_Of_Supply", typeof(string));
                        ph_tbl.Columns.Add("Amt_Excl_Tax", typeof(string));
                        ph_tbl.Columns.Add("Freight", typeof(string));
                        ph_tbl.Columns.Add("IGST", typeof(string));
                        ph_tbl.Columns.Add("SGST", typeof(string));
                        ph_tbl.Columns.Add("CGST", typeof(string));
                        ph_tbl.Columns.Add("Total_Amt", typeof(string));
                        ph_tbl.Columns.Add("Header_Notes", typeof(string));
                        ph_tbl.Columns.Add("Total_Amt_Word", typeof(string));
                        ph_tbl.Columns.Add("Purc_Order_No", typeof(string));
                        ph_tbl.Columns.Add("Bus_Area", typeof(string));
                        ph_tbl.Columns.Add("Purc_Order_No2", typeof(string));
                        ph_tbl.Columns.Add("Pur_Order_Line_No", typeof(string));
                        ph_tbl.Columns.Add("Description", typeof(string));
                        ph_tbl.Columns.Add("UOM", typeof(string));
                        ph_tbl.Columns.Add("Quantity", typeof(string));
                        ph_tbl.Columns.Add("Unit_Price", typeof(string));
                        ph_tbl.Columns.Add("HSN_SAC_No", typeof(string));
                        ph_tbl.Columns.Add("Total_Line_Amt", typeof(string));
                        ph_tbl.Columns.Add("Tax_Line_Amt", typeof(string));
                        ph_tbl.Columns.Add("IGST_Perct", typeof(string));
                        ph_tbl.Columns.Add("SGST_Perct", typeof(string));
                        ph_tbl.Columns.Add("CGST_Perct", typeof(string));
                        ph_tbl.Columns.Add("Credit_Note_No", typeof(string));
                        ph_tbl.Columns.Add("Debit_Note_No", typeof(string));

                        if (dt.Rows.Count > 0)
                        {
                            DataView view1im = new DataView(dt);
                            dt6 = new DataTable();
                            dt6 = view1im.ToTable(true, "INVNO", "INVDATE");
                            foreach (DataRow dr0 in dt6.Rows)
                            {
                                //dt6 = view1.ToTable(true, "INVNO");
                                DataView view1 = new DataView(dt, "INVNO='" + dr0["INVNO"].ToString().Trim() + "' AND INVDATE='" + dr0["INVDATE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt3 = new DataTable();
                                dt3 = view1.ToTable();
                                db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; mq5 = "";
                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    db9 = fgen.make_double(dt3.Rows[i]["amount"].ToString().Trim());
                                    // for freight value
                                    db1 = fgen.make_double(fgen.seek_iname_dt(dt2, "ordno='" + dt3.Rows[i]["ponum"].ToString().Trim() + "' and orddt='" + dt3.Rows[i]["podt"].ToString().Trim() + "'and icode='" + dt3.Rows[i]["icode"].ToString().Trim() + "' and acode='" + dt3.Rows[i]["acode"].ToString().Trim() + "'", "sd"));
                                    db2 = db1 * fgen.make_double(dt3.Rows[i]["qty"].ToString().Trim());
                                    db10 = db2;
                                    //for tax value
                                    db3 = (db2 * fgen.make_double(dt3.Rows[i]["exc_rate"].ToString().Trim())) / 100;
                                    if (dt3.Rows[i]["IOPR"].ToString().Trim() == "IG")
                                    {
                                        db11 = Math.Round(fgen.make_double(dt3.Rows[i]["Igst_val"].ToString().Trim()) + db3, 2);
                                        db12 = 0;
                                        db13 = 0;
                                    }
                                    else
                                    {
                                        db11 = 0;
                                        db12 = Math.Round(fgen.make_double(dt3.Rows[i]["Sgst_val"].ToString().Trim()) + db3, 2);
                                        db13 = Math.Round(fgen.make_double(dt3.Rows[i]["Cgst_val"].ToString().Trim()) + db3, 2);
                                    }
                                    db14 += db9 + db10 + db11 + db12 + db12;
                                }
                                mq5 = (db14).ToString("N").Replace(",", "");

                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    dr1 = ph_tbl.NewRow();
                                    dr1["Supplier_Comp_Name_Addr"] = dt3.Rows[i]["supplier_Company_Name_Addr"].ToString().Trim();
                                    dr1["Supplier_Gst_No"] = dt3.Rows[i]["Supplier_GST_number"].ToString().Trim();
                                    dr1["Supplier_Pan_No"] = dt3.Rows[i]["Supplier_PAN_number"].ToString().Trim();
                                    dr1["Party_Name_and_Addr_Bill_To"] = dt3.Rows[i]["PARTY_Name_and_AddR_Bill_To"].ToString().Trim();

                                    if (dt1.Rows.Count > 0)
                                    {
                                        dr1["Party_Name_and_Addr_Ship_To"] = fgen.seek_iname_dt(dt1, "acode='" + dt3.Rows[i]["cscode"].ToString().Trim() + "'", "name");
                                        if (dr1["Party_Name_and_Addr_Ship_To"].ToString().Trim() == "0")
                                        {
                                            dr1["Party_Name_and_Addr_Ship_To"] = dt3.Rows[i]["PARTY_Name_and_AddR_Bill_To"].ToString().Trim();
                                        }
                                    }

                                    dr1["Party_Gst_No_Bill_To"] = dt3.Rows[i]["PARTY_GST_NO_BILL"].ToString().Trim();
                                    if (dt1.Rows.Count > 0)
                                    {
                                        dr1["Party_Gst_No_Ship_To"] = fgen.seek_iname_dt(dt1, "acode='" + dt3.Rows[i]["cscode"].ToString().Trim() + "'", "dgst_no");
                                        if (dr1["Party_Gst_No_Ship_To"].ToString().Trim() == "0")
                                        {
                                            dr1["Party_Gst_No_Ship_To"] = dt3.Rows[i]["PARTY_GST_NO_BILL"].ToString().Trim();
                                        }
                                    }

                                    dr1["Party_Pan_No_Bill_To"] = dt3.Rows[i]["PARTY_PAN_BILL"].ToString().Trim();
                                    if (dt1.Rows.Count > 0)
                                    {
                                        dr1["Party_Pan_No_Ship_To"] = fgen.seek_iname_dt(dt1, "acode='" + dt3.Rows[i]["cscode"].ToString().Trim() + "'", "dpanno");
                                        if (dr1["Party_Pan_No_Ship_To"].ToString().Trim() == "0")
                                        {
                                            dr1["Party_Pan_No_Ship_To"] = dt3.Rows[i]["PARTY_PAN_BILL"].ToString().Trim();
                                        }
                                    }
                                    dr1["Party_Plant_Code"] = dt3.Rows[i]["Party_Plant_Code"].ToString().Trim();

                                    dr1["Invoice_Dt"] = dt3.Rows[i]["INVDATE"].ToString().Trim();
                                    dr1["Invoice_Number"] = dt3.Rows[i]["INVNO"].ToString().Trim();
                                    dr1["Currency"] = dt3.Rows[i]["CURRENCY"].ToString().Trim();
                                    dr1["Place_Of_Supply"] = dt3.Rows[i]["PLACE_SUPPLY"].ToString().Trim();

                                    dr1["Amt_Excl_Tax"] = fgen.make_double(dt3.Rows[i]["amount"].ToString().Trim());

                                    // for freight value
                                    //db1 = fgen.make_double(fgen.seek_iname_dt(dt2, "ordno='" + dt3.Rows[i]["ponum"].ToString().Trim() + "' and orddt='" + dt3.Rows[i]["podt"].ToString().Trim() + "'and icode='" + dt3.Rows[i]["icode"].ToString().Trim() + "' and acode='" + dt3.Rows[i]["acode"].ToString().Trim() + "'", "sd"));
                                    db1 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "sd"));

                                    db2 = db1 * fgen.make_double(dt3.Rows[i]["qty"].ToString().Trim());
                                    dr1["Freight"] = db2;
                                    //for tax value
                                    db3 = (db2 * fgen.make_double(dt3.Rows[i]["exc_rate"].ToString().Trim())) / 100;
                                    if (dt3.Rows[i]["IOPR"].ToString().Trim() == "IG")
                                    {
                                        dr1["IGST"] = Math.Round(fgen.make_double(dt3.Rows[i]["Igst_val"].ToString().Trim()) + db3, 2);
                                        dr1["SGST"] = "0";
                                        dr1["CGST"] = "0";
                                    }
                                    else
                                    {
                                        dr1["IGST"] = "0";
                                        dr1["SGST"] = Math.Round(fgen.make_double(dt3.Rows[i]["Sgst_val"].ToString().Trim()) + db3, 2);
                                        dr1["CGST"] = Math.Round(fgen.make_double(dt3.Rows[i]["Cgst_val"].ToString().Trim()) + db3, 2);
                                    }
                                    dr1["Total_Amt"] = (fgen.make_double(dr1["Amt_Excl_Tax"].ToString()) + fgen.make_double(dr1["Freight"].ToString()) + fgen.make_double(dr1["IGST"].ToString()) + fgen.make_double(dr1["SGST"].ToString()) + fgen.make_double(dr1["CGST"].ToString())).ToString("N").Replace(",", "");

                                    dr1["Header_Notes"] = dt3.Rows[i]["HEADER_NOTE"].ToString().Trim();

                                    dr1["Total_Amt_Word"] = fgen.ConvertNumbertoWords(mq5);

                                    er5 = dt3.Rows[i]["ponum1"].ToString().Trim();
                                    er5 = er5.ToUpper().Replace("DT.", "^");
                                    dr1["Purc_Order_No"] = er5.Split('^')[0].ToString();
                                    dr1["Bus_Area"] = dt3.Rows[i]["BUSINESS_AREA"].ToString().Trim();
                                    dr1["Purc_Order_No2"] = er5.Split('^')[0].ToString();
                                    dr1["Pur_Order_Line_No"] = dt3.Rows[i]["PO_LINE_NO"].ToString().Trim();
                                    dr1["Description"] = dt3.Rows[i]["DESC_"].ToString().Trim();
                                    dr1["UOM"] = dt3.Rows[i]["UNIT"].ToString().Trim();
                                    dr1["Quantity"] = dt3.Rows[i]["QTY"].ToString().Trim();
                                    dr1["Unit_Price"] = dt3.Rows[i]["UNIT_PRICE"].ToString().Trim();
                                    dr1["HSN_SAC_No"] = dt3.Rows[i]["HSN_NO"].ToString().Trim();

                                    dr1["Total_Line_Amt"] = Math.Round(fgen.make_double(dt3.Rows[i]["amount"].ToString().Trim()) + db2, 2);
                                    dr1["Tax_Line_Amt"] = Math.Round(fgen.make_double(dr1["IGST"].ToString()) + fgen.make_double(dr1["CGST"].ToString()) + fgen.make_double(dr1["SGST"].ToString()), 2);
                                    dr1["IGST_Perct"] = dt3.Rows[i]["IGST_PERC"].ToString().Trim();
                                    dr1["SGST_Perct"] = dt3.Rows[i]["SGST_PERC"].ToString().Trim();
                                    dr1["CGST_Perct"] = dt3.Rows[i]["CGST_PERC"].ToString().Trim();
                                    dr1["Credit_Note_No"] = dt3.Rows[i]["CREDIT_NOTE_NO"].ToString().Trim();
                                    dr1["Debit_Note_No"] = dt3.Rows[i]["DEBIT_NOTE_NO"].ToString().Trim();
                                    if (dt3.Rows[i]["icode"].ToString().Trim().Substring(0, 1) != "5")
                                    {
                                        ph_tbl.Rows.Add(dr1);
                                    }
                                }
                            }
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = ph_tbl;
                        fileName = co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
                        filepath = Server.MapPath("~/tej-base/Upload/") + fileName;
                        //fgen.CreateCSVFile(dt, filepath);
                        //fgen.CreateCSVFile(ph_tbl, @"c:\TEJ_ERP\UPLOAD\" + fileName);
                        fgen.exp_to_excel(ph_tbl, "ms-excel", "xls", @"c:\TEJ_ERP\UPLOAD\" + fileName, "N");
                        Session["FilePath"] = fileName;
                        Session["FileName"] = fileName;
                        Response.Write("<script>");
                        Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                        Response.Write("</script>");
                        fgen.msg("-", "AMSG", "The file has been downloaded!!");
                        //fgen.Fn_open_rptlevel(header_n + " For the Period " + fromdt + " To " + todt + "", frm_qstr);

                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please Select Invoices !! '13' You Want to See..");
                    }
                    #endregion
                    break;

                case "F50180":
                    // open drill down form
                    fgen.drillQuery(0, "select trim(a.acode) as fstr,'' as gstr,trim(b.aname) as party_name,trim(a.ordno) as ORDER_NO, to_char(a.orddt,'dd/mm/yyyy') as ORDER_DT, sum(a.qtyord) as Qty_Ordered, trim(a.acode) as party_code from Somas a , famst b  where trim(a.acode)=trim(b.acode) and  a.branchcd='" + mbr + "' and a.type like '4%' and a.orddt " + xprdrange + " group by a.ordno,a.orddt,a.acode,b.aname", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.acode)||trim(a.icode) as fstr,trim(a.acode) as gstr,trim(a.ordno) as ponum, to_char(a.orddt,'dd/mm/yyyy') as podt, trim(a.acode) as party_code,trim(b.aname) as party_name, sum(a.qtyord) as Qty,trim(a.icode) as item_code,trim(c.iname) as item_name from Somas a , famst b ,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.orddt " + xprdrange + " group by a.ordno,a.orddt,a.acode,b.aname,a.icode,c.iname", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.acode)||trim(a.icode) as gstr, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.icode) as item_code,c.iname as item_name,trim(a.acode) as party_code,trim(b.aname) as Party_name,a.iqtyout as qty,a.iamount as amount,trim(ponum) as ponum,to_char(podate,'dd/mm/yyyy') as podate from ivoucher a , famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and  a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + "", frm_qstr);
                    fgen.Fn_DrillReport("Sales Order Drill Report for the period " + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F50181":
                    // open drill down form
                    fgen.drillQuery(0, "SELECT trim(a.acode) as fstr,'' as gstr,trim(B.ANAME) AS PARTY_NAME,SUM(A.TOT_SCH) AS TOT_SCH,SUM(A.TOT_rcpt) AS TOTAL_Dispatch,SUM(A.TOT_SCH)-SUM(A.TOT_rcpt) AS Balance, (case when sum(a.tot_sch)=0 then 0 else round((sum(a.tot_rcpt)/sum(a.tot_sch))*100,2) end) as bal_perc ,trim(A.ACODE) AS PARTY_CODE FROM (SELECT TYPE,VCHNUM,VCHDATE,ACODE,TOTAL AS TOT_SCH,0 AS TOT_rcpt FROM SCHEDULE WHERE BRANCHCD='" + mbr + "' AND TYPE='46' AND VCHDATE " + xprdrange + " UNION ALL SELECT TYPE,VCHNUM,VCHDATE,ACODE,0 AS TOT_SCH,iqtyout AS TOT_rcpt FROM ivoucher WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdrange + ") A , FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) GROUP BY A.ACODE,B.ANAME", frm_qstr);
                    fgen.drillQuery(1, "SELECT trim(a.icode)||trim(a.acode) as fstr,trim(a.acode) as gstr,trim(B.ANAME) AS PARTY_NAME ,SUM(A.TOT_SCH) AS TOT_SCH,SUM(A.tot_rcpt) AS TOTAL_DISP,SUM(A.TOT_SCH)-SUM(A.tot_rcpt) AS BAL, (case when sum(a.tot_sch)=0 then 0 else round((sum(a.tot_rcpt)/sum(a.tot_sch))*100,2) end) as bal_perc,trim(A.ACODE) AS PARTY_CODE,trim(a.icode) as Item_code,trim(c.iname) as item_name FROM (SELECT TYPE,VCHNUM,VCHDATE,ACODE,icode,TOTAL AS TOT_SCH,0 AS TOT_rcpt FROM SCHEDULE WHERE BRANCHCD='" + mbr + "' AND TYPE='66' AND VCHDATE " + xprdrange + " UNION ALL SELECT TYPE,VCHNUM,VCHDATE,ACODE,icode,0 AS TOT_SCH,iqtyin AS TOT_rcpt FROM ivoucher WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdrange + ") A , FAMST B, item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode)  GROUP BY A.ACODE,B.ANAME ,a.icode,c.iname", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.icode)||trim(a.acode) as gstr, trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as Party_code,trim(c.aname) as Party_name,trim(a.icode) as Item_code,trim(b.iname) as Item_name,a.iqtyout as qty,a.iamount as bill_amt,trim(a.invno) as Invno,to_char(a.invdate,'dd/mm/yyyy') as invdate from ivoucher a , item b ,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + "", frm_qstr);
                    fgen.Fn_DrillReport("Schedule VS Dispatch Drill Report for the period " + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F50245":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select TRIM(a.acode) as party_code,b.aname as party_name,TRIM(a.icode) as item_code,c.iname as item_name,c.cpartno as partNo,c.unit,sum(a.sch_qty) as sch_qty,sum(a.desp_qty) as desp_qty from (select trim(acode) as acode,trim(icode) as icode,total as sch_qty,0 as desp_qty from schedule where branchcd='" + mbr + "' and type='46' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' union all select trim(acode) as acode,trim(icode) as icode,0 as sch_qty,iqtyout as desp_qty from ivoucher where branchcd='" + mbr + "' and type like '4%' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' /*and nvl(iqtyout,0)>0*/) a ,famst b,item c where trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit order by party_code,item_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                    fgen.Fn_open_rptlevel("Schedule Vs Dispatch Customer Wise,Item Wise Summary for the Month " + header_n + " " + year + "", frm_qstr);
                    break;

                case "F50244":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "select trim(a.acode) as party_code ,b.aname as party_name,sum(a.sch_qty) as sch_qty,sum(a.desp_qty) as desp_qty from (select trim(acode) as acode,total as sch_qty,0 as desp_qty from schedule where branchcd='" + mbr + "' and type='46' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' union all select trim(acode) as acode,0 as sch_qty,iqtyout as desp_qty from ivoucher where branchcd='" + mbr + "' and type like '4%' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' /*and nvl(iqtyout,0)>0*/) a ,famst b where TRIM(A.ACODE) LIKE '" + party_cd + "%'  and trim(a.acode)=trim(b.acode) group by a.acode,b.aname order by party_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                    fgen.Fn_open_rptlevel("Schedule Vs Dispatch Customer Wise Summary for the Month " + header_n + " " + year + "", frm_qstr);
                    break;

                case "F50242":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    SQuery = "select  trim(a.icode) as item_code,b.iname as item_name,b.cpartno as partno,b.unit,sum(a.sch_qty) as sch_qty,sum(a.prod_qty) as prod_qty,sum(a.desp_qty) as desp_qty from (select trim(icode) as icode,total as sch_qty,0 as prod_qty,0 as desp_qty from schedule where branchcd='" + mbr + "' and type='46' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' union all select trim(icode) as icode,0 as sch_qty,iqtyin as prod_qty,0 as desp_qty from ivoucher where branchcd='" + mbr + "' and type in('15','16') and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' AND STORE='Y' union all select trim(icode) as icode,0 as sch_qty,0 as prod_qty,iqtyout as desp_qty from ivoucher where branchcd='" + mbr + "' and type like '4%' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' /*and nvl(iqtyout,0)>0*/) a ,item b where TRIM(A.ICODE) LIKE '" + party_cd + "%' AND trim(a.icode)=trim(b.icode) group by a.icode,b.iname,b.cpartno,b.unit order by item_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                    fgen.Fn_open_rptlevel("Schedule Vs Production Vs Dispatch Summary for the Month " + header_n + " " + year + "", frm_qstr);
                    break;

                case "F50325":
                    #region
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("Dated", typeof(string));
                    ph_tbl.Columns.Add("Inv_No", typeof(string));
                    ph_tbl.Columns.Add("Customer", typeof(string));
                    ////FROM FAMST or csmst
                    ph_tbl.Columns.Add("Part_No", typeof(string));
                    ph_tbl.Columns.Add("Part_Name", typeof(string));
                    ph_tbl.Columns.Add("Qty_Sold", typeof(double));
                    ph_tbl.Columns.Add("Rate", typeof(double));
                    ph_tbl.Columns.Add("PO_Line_No", typeof(double));
                    ph_tbl.Columns.Add("Basic", typeof(double));
                    //ph_tbl.Columns.Add("Gst_Rate", typeof(double));
                    ph_tbl.Columns.Add("PO_Ref", typeof(string));
                    ph_tbl.Columns.Add("PO_Date", typeof(string));
                    ph_tbl.Columns.Add("HSN_Code", typeof(string));
                    ph_tbl.Columns.Add("Ship_From_Address", typeof(string));
                    ph_tbl.Columns.Add("Ship_From_GSTIN", typeof(string));
                    ph_tbl.Columns.Add("Ship_To_Address", typeof(string));
                    ph_tbl.Columns.Add("Ship_To_GSTIN", typeof(string));
                    ph_tbl.Columns.Add("Document_Type_Invoice_or_Credit_Note", typeof(string));
                    //
                    ph_tbl.Columns.Add("CGST_Rate", typeof(double));
                    ph_tbl.Columns.Add("Input_Tax_CGST", typeof(double));
                    ph_tbl.Columns.Add("Tax_Type_CGST", typeof(string));
                    //
                    ph_tbl.Columns.Add("SGST_Rate", typeof(double));
                    ph_tbl.Columns.Add("Input_Tax_SGST", typeof(double));
                    ph_tbl.Columns.Add("Tax_Type_SGST", typeof(string));
                    //
                    ph_tbl.Columns.Add("IGST_Rate", typeof(double));
                    ph_tbl.Columns.Add("Input_Tax_IGST", typeof(double));
                    ph_tbl.Columns.Add("Tax_Type_IGST", typeof(string));
                    //
                    ph_tbl.Columns.Add("Payable_Tax_On_Reverse_Charge_Basis", typeof(string));
                    //  ph_tbl.Columns.Add("Tax_Type", typeof(string));//Tax Type (IGST/ CGST/ UGST/ SGST)
                    ph_tbl.Columns.Add("Currency", typeof(string));
                    ph_tbl.Columns.Add("Vehicle_No", typeof(string));
                    hfcode.Value = value1;
                    //SQuery = "select a.branchcd,a.inv_no,a.inv_date,a.customer_Code,a.PO_LINE_NO,a.customer,a.ship_to_Address,a.ship_to_gstin,a.part_no,a.part_name,sum(a.qty_sold) as qty_sold,a.irate,sum(a.basic) as basic,a.gst_rate,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.tax_type,a.currency,a.cscode from (select  a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'dd/mm/yyyy') as inv_date,trim(a.acode) as customer_Code,IS_NUMBER(A.binno) AS PO_LINE_NO,b.aname as customer,trim(b.addr1)||trim(b.addr2)||trim(b.addr3)||trim(b.addr4)||trim(b.pincode) as ship_to_Address,trim(b.gst_no) as ship_to_gstin,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,a.exc_rate as gst_rate,a.finvno as po_ref,c.hscode as hsn_code,T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number ,(Case when A.IOPR='CG' then 'cgst' else 'IGST' end ) as tax_type ,'INR'  as currency,s.cscode  from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + hfcode.Value + ")) a group by a.branchcd,a.inv_no,a.inv_date,a.customer_Code,a.PO_LINE_NO,a.customer,a.ship_to_Address,a.ship_to_gstin,a.part_no,a.part_name,a.irate,a.gst_rate,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.tax_type,a.currency,a.cscode  ORDER BY INV_NO"; //in this qry sum of qty
                    // SQuery = "select a.branchcd,a.inv_no,a.inv_date,a.customer_Code,a.PO_LINE_NO,a.customer,a.ship_to_Address,a.ship_to_gstin,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.gst_rate,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.tax_type,a.currency,a.cscode from (select  a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'dd/mm/yyyy') as inv_date,trim(a.acode) as customer_Code,IS_NUMBER(A.binno) AS PO_LINE_NO,b.aname as customer,trim(b.addr1)||trim(b.addr2)||trim(b.addr3)||trim(b.addr4)||trim(b.pincode) as ship_to_Address,trim(b.gst_no) as ship_to_gstin,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,a.exc_rate as gst_rate,a.finvno as po_ref,c.hscode as hsn_code,T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number ,(Case when A.IOPR='CG' then 'cgst' else 'IGST' end ) as tax_type ,'INR'  as currency,s.cscode  from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + hfcode.Value + ")) a  ORDER BY INV_NO"; //without sum
                    //   SQuery = "select a.branchcd,a.inv_no,a.inv_date,a.customer_Code,a.PO_LINE_NO,a.customer,a.vehicle_no,a.ship_to_Address,a.ship_to_gstin,A.gst_rate,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.cgst_rate,a.cgst_amt,a.igst_rate,a.igst_amt,a.sgst_rate,a.sgst_amt,a.tax_type_sg,a.tax_type_cg,a.tax_type_ig,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.currency,a.cscode from (select  a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'dd/mm/yyyy') as inv_date,trim(a.acode) as customer_Code,IS_NUMBER(A.binno) AS PO_LINE_NO,b.aname as customer,trim(b.addr1)||trim(b.addr2)||trim(b.addr3)||trim(b.addr4)||trim(b.pincode) as ship_to_Address,trim(b.gst_no) as ship_to_gstin,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,(Case when A.IOPR='CG' then a.exc_rate else 0 end ) as cgst_rate,(Case when A.IOPR='CG' then a.exc_amt else 0 end ) as cgst_amt,(Case when A.IOPR='IG' then a.exc_rate else 0 end ) as igst_rate,(Case when A.IOPR='IG' then a.exc_amt else 0 end ) as igst_amt,a.cess_percent as sgst_rate,a.cess_pu as sgst_amt,a.exc_rate as gst_rate,a.finvno as po_ref,c.hscode as hsn_code,T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number ,(Case when A.IOPR='CG' then 'CGST' else '-' end ) as tax_type_cg ,(Case when A.IOPR='CG' then 'SGST' else '-' end ) as tax_type_sg,(Case when A.IOPR='IG' then 'IGST' else '-' end ) as tax_type_ig  ,'INR'  as currency,s.cscode,s.mode_tpt,s.mo_vehi as vehicle_no  from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + hfcode.Value + ")) a  ORDER BY INV_NO";//without sum
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");//PARTY
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");  //FSTR
                    cond = "and a.acode like '" + mq1 + "%' ";
                    SQuery = "select a.branchcd,a.inv_no,a.inv_date,a.customer_Code,a.PO_LINE_NO,a.customer,a.vehicle_no,a.ship_to_Address,a.ship_to_gstin,A.gst_rate,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.cgst_rate,a.cgst_amt,a.igst_rate,a.igst_amt,a.sgst_rate,a.sgst_amt,a.tax_type_sg,a.tax_type_cg,a.tax_type_ig,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.currency,a.cscode,a.pono,a.podate from (select  a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'dd/mm/yyyy') as inv_date,trim(a.acode) as customer_Code,IS_NUMBER(A.binno) AS PO_LINE_NO,b.aname as customer,trim(b.addr1)||trim(b.addr2)||trim(b.addr3)||trim(b.addr4)||trim(b.pincode) as ship_to_Address,trim(b.gst_no) as ship_to_gstin,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,(Case when A.IOPR='CG' then a.exc_rate else 0 end ) as cgst_rate,(Case when A.IOPR='CG' then a.exc_amt else 0 end ) as cgst_amt,(Case when A.IOPR='IG' then a.exc_rate else 0 end ) as igst_rate,(Case when A.IOPR='IG' then a.exc_amt else 0 end ) as igst_amt,a.cess_percent as sgst_rate,a.cess_pu as sgst_amt,a.exc_rate as gst_rate,a.finvno as po_ref,c.hscode as hsn_code,T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number ,(Case when A.IOPR='CG' then 'CGST' else '-' end ) as tax_type_cg ,(Case when A.IOPR='CG' then 'SGST' else '-' end ) as tax_type_sg,(Case when A.IOPR='IG' then 'IGST' else '-' end ) as tax_type_ig  ,'INR'  as currency,s.cscode,s.mode_tpt,s.mo_vehi as vehicle_no,s.pono,to_char(s.podate,'dd/mm/yyyy') as podate  from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') " + cond + " AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + mq2 + ")) a  ORDER BY INV_NO";//without su;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    //////
                    mq1 = "Select distinct trim(d.aname)||','||trim(d.addr1)||','||trim(d.addr2)||','||trim(d.addr3)||','||trim(d.addr4) as name,d.acode as acode,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d ";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);
                    dr1 = ph_tbl.NewRow();

                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "inv_no"); //MAIN                  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "inv_no='" + dr0["inv_no"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            db19 = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                #region
                                dr1 = ph_tbl.NewRow();
                                dr1["Dated"] = dt1.Rows[i]["inv_date"].ToString().Trim();
                                dr1["Inv_No"] = dt1.Rows[i]["inv_no"].ToString().Trim();
                                dr1["Customer"] = dt1.Rows[i]["customer"].ToString().Trim();
                                dr1["Part_No"] = dt1.Rows[i]["part_no"].ToString().Trim();
                                dr1["Part_Name"] = dt1.Rows[i]["part_name"].ToString().Trim();
                                dr1["Qty_Sold"] = dt1.Rows[i]["qty_sold"].ToString().Trim();
                                dr1["Rate"] = dt1.Rows[i]["irate"].ToString().Trim();
                                dr1["PO_Line_No"] = fgen.make_double(dt1.Rows[i]["PO_LINE_NO"].ToString().Trim());
                                dr1["Basic"] = dt1.Rows[i]["basic"].ToString().Trim();
                                // dr1["Gst_Rate"] = dt1.Rows[i]["gst_rate"].ToString().Trim();
                                dr1["PO_Ref"] = dt1.Rows[i]["po_ref"].ToString().Trim();
                                dr1["PO_Date"] = dt1.Rows[i]["podate"].ToString().Trim();
                                dr1["HSN_Code"] = dt1.Rows[i]["hsn_code"].ToString().Trim();
                                dr1["Ship_From_Address"] = dt1.Rows[i]["supplier_Company_Name_Addr"].ToString().Trim();
                                dr1["Ship_From_GSTIN"] = dt1.Rows[i]["Supplier_gst_number"].ToString().Trim();
                                if (dt2.Rows.Count > 0)
                                {
                                    dr1["Ship_To_Address"] = fgen.seek_iname_dt(dt2, "acode='" + dt1.Rows[i]["cscode"].ToString().Trim() + "'", "name");
                                    if (dr1["Ship_To_Address"].ToString().Trim() == "0")
                                    {
                                        dr1["Ship_To_Address"] = dt1.Rows[i]["ship_to_Address"].ToString().Trim();
                                    }
                                    dr1["Ship_To_GSTIN"] = fgen.seek_iname_dt(dt2, "acode='" + dt1.Rows[i]["cscode"].ToString().Trim() + "'", "dgst_no");
                                    if (dr1["Ship_To_GSTIN"].ToString().Trim() == "0")
                                    {
                                        dr1["Ship_To_GSTIN"] = dt1.Rows[i]["Ship_To_GSTIN"].ToString().Trim();
                                    }
                                }
                                dr1["Document_Type_Invoice_or_Credit_Note"] = "Invoice";// dt1.Rows[i][""].ToString().Trim();
                                //////////
                                dr1["CGST_Rate"] = fgen.make_double(dt1.Rows[i]["CGST_Rate"].ToString().Trim());
                                dr1["Input_Tax_CGST"] = fgen.make_double(dt1.Rows[i]["CGST_AMT"].ToString().Trim());
                                dr1["Tax_Type_CGST"] = "CGST";// dt1.Rows[i]["Tax_Type_CG"].ToString().Trim();
                                dr1["SGST_Rate"] = fgen.make_double(dt1.Rows[i]["SGST_Rate"].ToString().Trim());
                                dr1["Input_Tax_SGST"] = fgen.make_double(dt1.Rows[i]["SGST_AMT"].ToString().Trim());
                                dr1["Tax_Type_SGST"] = "SGST";// dt1.Rows[i]["Tax_Type_SG"].ToString().Trim();
                                dr1["IGST_Rate"] = fgen.make_double(dt1.Rows[i]["IGST_Rate"].ToString().Trim());
                                dr1["Input_Tax_IGST"] = fgen.make_double(dt1.Rows[i]["IGST_AMT"].ToString().Trim());
                                dr1["Tax_Type_IGST"] = "IGST"; dt1.Rows[i]["Tax_Type_IG"].ToString().Trim();
                                // dr1["Input_Tax"] = 0;// Math.Round(fgen.make_double(dr1["Basic"].ToString().Trim()) * fgen.make_double(dr1["Gst_Rate"].ToString().Trim()) / 100, 2);
                                dr1["Payable_Tax_On_Reverse_Charge_Basis"] = "NO";// dt1.Rows[i][""].ToString().Trim();
                                //dr1["Tax_Type"] = dt1.Rows[i]["tax_type"].ToString().Trim();
                                dr1["Currency"] = dt1.Rows[i]["currency"].ToString().Trim();
                                dr1["Vehicle_No"] = dt1.Rows[i]["vehicle_no"].ToString().Trim(); ; //vehicle_no
                                ph_tbl.Rows.Add(dr1);
                                #endregion
                            }
                            #region new

                            fileName = dt.Rows[0]["customer_Code"].ToString().Trim() + "_" + dr0["inv_no"].ToString().Trim() + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
                            filepath = @"c:\TEJ_ERP\UPLOAD\" + fileName;///testing
                            Session["send_dt"] = ph_tbl;
                            mq7 = @"c:\TEJ_ERP\UPLOAD\";
                            if (!Directory.Exists(mq7)) Directory.CreateDirectory(mq7);
                            fileName = dt.Rows[0]["customer_Code"].ToString().Trim() + "_" + dr0["inv_no"].ToString().Trim();
                            filepath = @"c:\TEJ_ERP\UPLOAD\" + fileName + ".xls";

                            fgen.CreateCSVFile(ph_tbl, filepath);

                            //fgen.exp_to_excel(ph_tbl, "ms-excel", "xls", fileName, "N");

                            zipFilePath += "," + filepath;
                            zipFileName += "," + fileName;

                            ph_tbl.Clear();
                            #endregion
                        }
                        zipFilePath = zipFilePath.TrimStart(',');
                        zipFileName = zipFileName.TrimStart(',');

                        Session["FilePath"] = zipFilePath;
                        Session["FileName"] = "Tungston Report";

                        Response.Write("<script>");
                        Response.Write("window.open('../tej-base/makeZipDwnload.aspx','_blank')");
                        Response.Write("</script>");
                        ded1 = @"c:\TEJ_ERP\UPLOAD";

                        fgen.msg("-", "AMSG", "File has been downloaded");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please Select Invoices !! '13' You Want to See..");
                    }
                    #endregion
                    break;

                case "F50326":
                    #region
                    ph_tbl = new DataTable();
                    #region
                    dr1 = ph_tbl.NewRow();
                    ph_tbl.Columns.Add("A", typeof(string));//a
                    ph_tbl.Columns.Add("B", typeof(string));//b
                    ph_tbl.Columns.Add("C", typeof(string));//c
                    ph_tbl.Columns.Add("D", typeof(string));//d
                    ph_tbl.Columns.Add("E", typeof(string));//e
                    ph_tbl.Columns.Add("F", typeof(string));//f
                    ph_tbl.Columns.Add("G", typeof(string));//g
                    ph_tbl.Columns.Add("H", typeof(string));//h
                    ph_tbl.Columns.Add("I", typeof(string));//i
                    ph_tbl.Columns.Add("J", typeof(string));//j
                    ph_tbl.Columns.Add("K", typeof(string));//k
                    ph_tbl.Columns.Add("L", typeof(string));//L
                    ph_tbl.Columns.Add("M", typeof(string));//M
                    ph_tbl.Columns.Add("N", typeof(string));//N
                    ph_tbl.Columns.Add("O", typeof(string));//O
                    ph_tbl.Columns.Add("P", typeof(string));//P
                    ph_tbl.Columns.Add("Q", typeof(string));//Q
                    ph_tbl.Columns.Add("R", typeof(string));//R
                    ph_tbl.Columns.Add("S", typeof(string));//S
                    ph_tbl.Columns.Add("T", typeof(string));//T
                    ph_tbl.Columns.Add("U", typeof(string));//U
                    ph_tbl.Columns.Add("V", typeof(string));//V
                    ph_tbl.Columns.Add("W", typeof(string));//W
                    ph_tbl.Columns.Add("X", typeof(string));//X
                    ph_tbl.Columns.Add("Y", typeof(string));//Y
                    ph_tbl.Columns.Add("Z", typeof(string));//Z
                    ph_tbl.Columns.Add("AA", typeof(string));//AA
                    ph_tbl.Columns.Add("AB", typeof(string));//AB
                    ph_tbl.Columns.Add("AC", typeof(string));//AC
                    ph_tbl.Columns.Add("AD", typeof(string));//AD
                    ph_tbl.Columns.Add("AE", typeof(string));//AE
                    ph_tbl.Columns.Add("AF", typeof(string));//AF
                    ph_tbl.Columns.Add("AG", typeof(string));//AG
                    ph_tbl.Columns.Add("AH", typeof(string));//AH
                    ph_tbl.Columns.Add("AI", typeof(string));//AI
                    ph_tbl.Columns.Add("AJ", typeof(string));//AJ
                    ph_tbl.Columns.Add("AK", typeof(string));//AK
                    ph_tbl.Columns.Add("AL", typeof(string));//AL
                    ph_tbl.Columns.Add("AM", typeof(string));//AM
                    ph_tbl.Columns.Add("AN", typeof(string));//AN
                    ph_tbl.Columns.Add("AO", typeof(string));//AO
                    ph_tbl.Columns.Add("AP", typeof(string));//AP
                    ph_tbl.Columns.Add("AQ", typeof(string));//AQ
                    ph_tbl.Columns.Add("AR", typeof(string));//AR
                    ph_tbl.Columns.Add("AS", typeof(string));//AS
                    ph_tbl.Columns.Add("AT", typeof(string));//AT
                    ph_tbl.Columns.Add("AU", typeof(string));//AU
                    ph_tbl.Columns.Add("AV", typeof(string));//AV
                    ph_tbl.Columns.Add("AW", typeof(string));//AW
                    ph_tbl.Columns.Add("AX", typeof(string));//AX
                    ph_tbl.Columns.Add("AY", typeof(string));//AY
                    ph_tbl.Columns.Add("AZ", typeof(string));//AZ
                    ph_tbl.Columns.Add("BA", typeof(string));//BA
                    ph_tbl.Columns.Add("BB", typeof(string));//BB
                    ph_tbl.Columns.Add("BC", typeof(string));//BC                                          
                    ph_tbl.Rows.Add(dr1);
                    #endregion
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");//PARTY
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");  //FSTR
                    cond = "and a.acode like '" + mq1 + "%' ";
                    //SQuery = "select a.branchcd,a.inv_no,to_char(a.inv_date,'dd/mm/yyyy') as inv_date,a.ACODE,a.PO_LINE_NO,a.customer,a.vehicle_no,a.icode,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.pono,a.podate from (select  a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'yyyy-MM-dd')||'T'||to_char(a.vchdate,'HH:MM:SS') as inv_date,trim(a.icode) as icode,trim(a.acode) as acode,A.DESC_ AS PO_LINE_NO,b.aname as customer,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,s.mode_tpt,s.mo_vehi as vehicle_no,s.pono,to_char(s.podate,'yyyy-MM-dd')||'T'||to_char(s.podate,'HH:MM:SS') as podate  from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') " + cond + " AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + mq2 + ")) a  ORDER BY INV_NO,a.icode";
                    SQuery = "select a.branchcd,a.inv_no,a.inv_date,a.ACODE,a.PO_LINE_NO,a.customer,a.vehicle_no,a.icode,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.pono,a.podate from (select  a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'yyyy-MM-dd')||'T'||'00:00:00+05:30' as inv_date,trim(a.icode) as icode,trim(a.acode) as acode,A.DESC_ AS PO_LINE_NO,b.aname as customer,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,s.mode_tpt,s.mo_vehi as vehicle_no,s.pono,to_char(s.podate,'yyyy-MM-dd')||'T'||'00:00:00+05:30' as podate  from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') " + cond + " AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + mq2 + ")) a  ORDER BY INV_NO,a.icode";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "inv_no"); //MAIN                  

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "inv_no='" + dr0["inv_no"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            db19 = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < 6; j++)
                                    {
                                        #region this loop for add heading in 5 rows as per excel sheet
                                        string var = Convert.ToString(j);
                                        switch (var)
                                        {
                                            case "0":
                                                #region for first row of header
                                                dr1 = ph_tbl.NewRow();
                                                dr1["A"] = "UTF-8";
                                                ph_tbl.Rows.Add(dr1);
                                                #endregion
                                                break;
                                            case "1":
                                                #region for 2 row in header
                                                dr1 = ph_tbl.NewRow();
                                                dr1["A"] = "_csv_version:1.0";
                                                dr1["B"] = "_csv_serial:SERIAL_DEFAULT_SHIP_NOTICE_V_1";
                                                dr1["C"] = "_csv_type:ShipNotice";
                                                dr1["D"] = "_csv_template:Standard_Template";
                                                ph_tbl.Rows.Add(dr1);
                                                #endregion
                                                break;
                                            case "2":
                                                #region for 3 row in header
                                                dr1 = ph_tbl.NewRow();
                                                dr1["A"] = "Ship Notice Number";
                                                dr1["B"] = "Ship Notice Date";
                                                dr1["C"] = "Order ID";
                                                dr1["D"] = "Order Date";
                                                dr1["E"] = "Service Level";
                                                dr1["F"] = "Shipment_Type";
                                                dr1["G"] = "Shipment_Date";
                                                dr1["H"] = "Delivery_Date";
                                                dr1["I"] = "Ship From";
                                                dr1["J"] = "Ship From Street";
                                                dr1["K"] = "Ship From Municipality";
                                                dr1["L"] = "Ship From Postal Code";
                                                dr1["M"] = "Ship From City";
                                                dr1["N"] = "Ship From State";
                                                dr1["O"] = "Ship From Country_Code";
                                                dr1["P"] = "Ship From Country";
                                                dr1["Q"] = "Ship To";
                                                dr1["R"] = "Ship To Street";
                                                dr1["S"] = "Ship To Municipality";
                                                dr1["T"] = "Ship To Postal Code";
                                                dr1["U"] = "Ship To City";
                                                dr1["V"] = "Ship To State";
                                                dr1["W"] = "Ship To Country Code";
                                                dr1["X"] = "Ship To Country";
                                                dr1["Y"] = "Carrier Name";
                                                dr1["Z"] = "Tracking Number";
                                                dr1["AA"] = "Tracking Date";
                                                dr1["AB"] = "Shipping Method";
                                                dr1["AC"] = "Item Ship Notice Line Number";
                                                dr1["AD"] = "Item Line Number";
                                                dr1["AE"] = "Item Parent Line Number";
                                                dr1["AF"] = "Item Supplier Part ID";
                                                dr1["AG"] = "Item Quantity";
                                                dr1["AH"] = "Item Unit Of Measure";
                                                dr1["AI"] = "Item Unit Price Currency";
                                                dr1["AJ"] = "Item Unit Price Amount";
                                                dr1["AK"] = "Item Description";
                                                dr1["AL"] = "Item Hazard Code";
                                                dr1["AM"] = "Item Hazard Domain";
                                                dr1["AN"] = "Item Hazard Description";
                                                dr1["AO"] = "Item Batch ID";
                                                dr1["AP"] = "Item Asset Tag";
                                                dr1["AQ"] = "Item Serial Number";
                                                dr1["AR"] = "Item Customer Part ID";
                                                dr1["AS"] = "Schedule Line Number";
                                                dr1["AT"] = "Customer Location";
                                                dr1["AU"] = "Ordering Address ID";
                                                dr1["AV"] = "Ship notice item-Shipping container serial code";
                                                dr1["AW"] = "Ship notice item-Unit net weight";
                                                dr1["AX"] = "Ship notice item-Unit net weight UOM";
                                                dr1["AY"] = "Ship notice item-Gross weight";
                                                dr1["AZ"] = "Ship notice item-Gross weight UOM";
                                                dr1["BA"] = "Agreement ID";
                                                dr1["BB"] = "Agreement Type";
                                                dr1["BC"] = "Agreement Date";
                                                ph_tbl.Rows.Add(dr1);
                                                #endregion
                                                break;
                                            case "3":
                                                #region for 4 row in header
                                                dr1 = ph_tbl.NewRow();
                                                dr1["A"] = "Required";
                                                dr1["B"] = "Required";
                                                dr1["C"] = "Required";
                                                dr1["D"] = "Required";
                                                dr1["E"] = "Optional";
                                                dr1["F"] = "Optional";
                                                dr1["G"] = "Optional";
                                                dr1["H"] = "Optional";
                                                dr1["I"] = "Optional";
                                                dr1["J"] = "Optional";
                                                dr1["K"] = "Optional";
                                                dr1["L"] = "Optional";
                                                dr1["M"] = "Optional";
                                                dr1["N"] = "Optional";
                                                dr1["O"] = "Optional";
                                                dr1["P"] = "Optional";
                                                dr1["Q"] = "Optional";
                                                dr1["R"] = "Optional";
                                                dr1["S"] = "Optional";
                                                dr1["T"] = "Optional";
                                                dr1["U"] = "Optional";
                                                dr1["V"] = "Optional";
                                                dr1["W"] = "Optional";
                                                dr1["X"] = "Optional";
                                                dr1["Y"] = "Optional";
                                                dr1["Z"] = "Optional";
                                                dr1["AA"] = "Optional";
                                                dr1["AB"] = "Optional";
                                                dr1["AC"] = "Required";
                                                dr1["AD"] = "Required";
                                                dr1["AE"] = "Optional";
                                                dr1["AF"] = "Optional";
                                                dr1["AG"] = "Required";
                                                dr1["AH"] = "Optional";
                                                dr1["AI"] = "Required";
                                                dr1["AJ"] = "Required";
                                                dr1["AK"] = "Optional";
                                                dr1["AL"] = "Optional";
                                                dr1["AM"] = "Optional";
                                                dr1["AN"] = "Optional";
                                                dr1["AO"] = "Optional";
                                                dr1["AP"] = "Optional";
                                                dr1["AQ"] = "Optional";
                                                dr1["AR"] = "Optional";
                                                dr1["AS"] = "Optional";
                                                dr1["AT"] = "Optional";
                                                dr1["AU"] = "Optional";
                                                dr1["AV"] = "Optional";
                                                dr1["AW"] = "Optional";
                                                dr1["AX"] = "Optional";
                                                dr1["AY"] = "Optional";
                                                dr1["AZ"] = "Optional";
                                                dr1["BA"] = "Optional";
                                                dr1["BB"] = "Optional";
                                                dr1["BC"] = "Optional";
                                                ph_tbl.Rows.Add(dr1);
                                                #endregion
                                                break;
                                            case "4":
                                                #region for 5 row in header
                                                dr1 = ph_tbl.NewRow();
                                                dr1["A"] = "String";
                                                dr1["B"] = "Date";
                                                dr1["C"] = "String";
                                                dr1["D"] = "Date";
                                                dr1["E"] = "String";
                                                dr1["F"] = "String";
                                                dr1["G"] = "Date";
                                                dr1["H"] = "Date";
                                                dr1["I"] = "String";
                                                dr1["J"] = "String";
                                                dr1["K"] = "String";
                                                dr1["L"] = "String";
                                                dr1["M"] = "String";
                                                dr1["N"] = "String";
                                                dr1["O"] = "String";
                                                dr1["P"] = "String";
                                                dr1["Q"] = "String";
                                                dr1["R"] = "String";
                                                dr1["S"] = "String";
                                                dr1["T"] = "String";
                                                dr1["U"] = "String";
                                                dr1["V"] = "String";
                                                dr1["W"] = "String";
                                                dr1["X"] = "String";
                                                dr1["Y"] = "String";
                                                dr1["Z"] = "String";
                                                dr1["AA"] = "Date";
                                                dr1["AB"] = "String";
                                                dr1["AC"] = "Integer";
                                                dr1["AD"] = "Integer";
                                                dr1["AE"] = "Integer";
                                                dr1["AF"] = "String";
                                                dr1["AG"] = "Decimal";
                                                dr1["AH"] = "String";
                                                dr1["AI"] = "String";
                                                dr1["AJ"] = "Decimal";
                                                dr1["AK"] = "String";
                                                dr1["AL"] = "String";
                                                dr1["AM"] = "String";
                                                dr1["AN"] = "String";
                                                dr1["AO"] = "String";
                                                dr1["AP"] = "String";
                                                dr1["AQ"] = "String";
                                                dr1["AR"] = "String";
                                                dr1["AS"] = "Integer";
                                                dr1["AT"] = "String";
                                                dr1["AU"] = "String";
                                                dr1["AV"] = "String";
                                                dr1["AW"] = "String";
                                                dr1["AX"] = "String";
                                                dr1["AY"] = "String";
                                                dr1["AZ"] = "String";
                                                dr1["BA"] = "String";
                                                dr1["BB"] = "String";
                                                dr1["BC"] = "Date";
                                                ph_tbl.Rows.Add(dr1);
                                                #endregion
                                                break;
                                            case "5":
                                                #region for 6 row in header
                                                dr1 = ph_tbl.NewRow();
                                                dr1["A"] = "Ship_notice_ID";
                                                dr1["B"] = "Ship notice date; format: YYYY-MM-DDThh:mm:ss-ZZ; example: 2015-03-30T09:26:32-07:00";
                                                dr1["C"] = "Purchase_order_ID";
                                                dr1["D"] = "Purchase order date; format: YYYY-MM-DDThh:mm:ss-ZZ; example: 2015-03-30T09:26:32-07:00";
                                                dr1["E"] = "The level ofservice (such as overnight) provided by the carrier for this shipment";
                                                dr1["F"] = "Shipment type (actual or planned)";
                                                dr1["G"] = "Shipment date; format: YYYY-MM-DDThh:mm:ss-ZZ; example: 2015-03-30T09:26:32-07:00";
                                                dr1["H"] = "Delivery date; format: YYYY-MM-DDThh:mm:ss-ZZ; example: 2015-03-30T09:26:32-07:00";
                                                dr1["I"] = "Ship From name";
                                                dr1["J"] = "Ship From street address";
                                                dr1["K"] = "Ship From municipality";
                                                dr1["L"] = "Ship From postal code";
                                                dr1["M"] = "Ship From city";
                                                dr1["N"] = "Ship From state";
                                                dr1["O"] = "Ship From country code";
                                                dr1["P"] = "Ship from country";
                                                dr1["Q"] = "Ship To name";
                                                dr1["R"] = "Ship To street address";
                                                dr1["S"] = "Ship To municipality";
                                                dr1["T"] = "Ship To postal code";
                                                dr1["U"] = "Ship To city";
                                                dr1["V"] = "Ship To state";
                                                dr1["W"] = "Ship To country code";
                                                dr1["X"] = "Ship To country";
                                                dr1["Y"] = "Carrier name (Airborne Express or DHL or FedEx or UPS or US Postal Service or Other)";
                                                dr1["Z"] = "Tracking number";
                                                dr1["AA"] = "Tracking date; format: YYYY-MM-DDThh:mm:ss-ZZ; example: 2015-03-30T09:26:32-07:00";
                                                dr1["AB"] = "Shipping method (Air or Motor or Rail or Ship)";
                                                dr1["AC"] = "Ship notice line number";
                                                dr1["AD"] = "Purchase order line number reference";
                                                dr1["AE"] = "Parent line number";
                                                dr1["AF"] = "Supplier Part ID";
                                                dr1["AG"] = "Item quantity";
                                                dr1["AH"] = "UN/CEFACT unit of measure code; for example EA for each or PK for pack or MO for Month or HUR for Hour";
                                                dr1["AI"] = "Unit price currency";
                                                dr1["AJ"] = "Unit price amount";
                                                dr1["AK"] = "Item description";
                                                dr1["AL"] = "Item Hazard Code";
                                                dr1["AM"] = "Item Hazard Domain";
                                                dr1["AN"] = "Item Hazard Description";
                                                dr1["AO"] = "Item Batch ID";
                                                dr1["AP"] = "Item Asset Tag";
                                                dr1["AQ"] = "Item Serial Number";
                                                dr1["AR"] = "Item Customer Part ID";
                                                dr1["AS"] = "Schedule Line Number";
                                                dr1["AT"] = "Customer Location";
                                                dr1["AU"] = "Ordering Address ID";
                                                dr1["AV"] = "Item - Shipping container serial code";
                                                dr1["AW"] = "Item - Unit net weight";
                                                dr1["AX"] = "UN/CEFACT unit of measure code; for example KG for kilogram or LB for pound";
                                                dr1["AY"] = "Item - Gross weight";
                                                dr1["AZ"] = "UN/CEFACT unit of measure code; for example KG for kilogram or LB for pound";
                                                dr1["BA"] = "Agreement ID (The id of a master agreement)";
                                                dr1["BB"] = "Agreement Type (The type of the master agreement being referenced; for example: scheduling_agreement)";
                                                dr1["BC"] = "Agreement Date (The date and time the master agreement request was created)";
                                                ph_tbl.Rows.Add(dr1);
                                                #endregion
                                                break;
                                        }
                                        #endregion
                                    }
                                }
                                mq1 = ""; mq2 = "";
                                #region FILL VALUES
                                dr1 = ph_tbl.NewRow();
                                dr1["A"] = dt1.Rows[i]["inv_no"].ToString().Trim();
                                dr1["B"] = dt1.Rows[i]["inv_date"].ToString().Trim();
                                dr1["C"] = dt1.Rows[i]["PONO"].ToString().Trim();
                                dr1["D"] = dt1.Rows[i]["PODATE"].ToString().Trim();
                                dr1["E"] = "";
                                dr1["F"] = "actual";
                                dr1["G"] = dt1.Rows[i]["inv_date"].ToString().Trim();
                                dr1["H"] = dt1.Rows[i]["inv_date"].ToString().Trim();
                                dr1["I"] = "";
                                dr1["J"] = "";
                                dr1["K"] = "";
                                dr1["L"] = "";
                                dr1["M"] = "";
                                dr1["N"] = "";
                                dr1["O"] = "";
                                dr1["P"] = "";
                                dr1["Q"] = "";
                                dr1["R"] = "";
                                dr1["S"] = "";
                                dr1["T"] = "";
                                dr1["U"] = "";
                                dr1["V"] = "";
                                dr1["W"] = "";
                                dr1["X"] = "";
                                dr1["Y"] = "OTHER";
                                dr1["Z"] = dt1.Rows[i]["vehicle_no"].ToString().Trim();
                                dr1["AA"] = "";
                                dr1["AB"] = "";
                                dr1["AC"] = dt1.Rows[i]["PO_LINE_NO"].ToString().Trim();
                                dr1["AD"] = dt1.Rows[i]["PO_LINE_NO"].ToString().Trim();
                                dr1["AE"] = "";
                                dr1["AF"] = "";
                                dr1["AG"] = dt1.Rows[i]["qty_sold"].ToString().Trim();
                                dr1["AH"] = "";
                                dr1["AI"] = "INR";
                                dr1["AJ"] = dt1.Rows[i]["IRATE"].ToString().Trim();
                                dr1["AK"] = "";
                                dr1["AL"] = "";
                                dr1["AM"] = "";
                                dr1["AN"] = "";
                                dr1["AO"] = "";
                                dr1["AP"] = "";
                                dr1["AQ"] = "";
                                dr1["AR"] = "";
                                dr1["AS"] = "";
                                dr1["AT"] = "";
                                dr1["AU"] = "";
                                dr1["AV"] = "";
                                dr1["AW"] = "";
                                dr1["AX"] = "";
                                dr1["AY"] = "";
                                dr1["AZ"] = "";
                                dr1["BA"] = "";
                                dr1["BB"] = "";
                                dr1["BC"] = "";
                                ph_tbl.Rows.Add(dr1);
                                #endregion
                            }
                            ph_tbl.Rows.RemoveAt(0);
                            #region new
                            fileName = dt.Rows[0]["ACODE"].ToString().Trim() + "_" + dr0["inv_no"].ToString().Trim() + ".xls";
                            filepath = @"c:\TEJ_ERP\UPLOAD\" + fileName;///testing                            
                            Session["send_dt"] = ph_tbl;
                            // fgen.CreateCSVFile(ph_tbl, @"c:\TEJ_ERP\UPLOAD\" + fileName);
                            fgen.exp_to_excel_multi(ph_tbl, filepath);
                            zipFilePath += "," + filepath;
                            zipFileName += "," + fileName;

                            #endregion

                            ph_tbl.Clear();
                        }
                        zipFilePath = zipFilePath.TrimStart(',');
                        zipFileName = zipFileName.TrimStart(',');

                        Session["FilePath"] = zipFilePath;
                        Session["FileName"] = "Honda Invoice";
                        Response.Write("<script>");
                        Response.Write("window.open('../tej-base/makeZipDwnload.aspx','_blank')");
                        Response.Write("</script>");

                        ded1 = @"c:\TEJ_ERP\UPLOAD";


                        fgen.msg("-", "AMSG", "File has been downloaded");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please Select Invoices !! '13' You Want to See..");
                    }
                    #endregion
                    break;
                case "F50137A":
                    cond = " showing all data (in minutes)";
                    if (hfcode.Value.toDouble() > 0)
                    {
                        cond = " (showing for delay " + hfcode.Value.toDouble() + " hours)";
                        SQuery = "select * from (SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,b.aname as truck_owner,A.COL1 AS TRUCKNO,A.COL2 AS TIME_IN,A.COL7 AS TRUCK_IN_BY,A.COL3 AS LOADING_TIME,A.COL12 AS LOADED_BY,A.COL4 AS LOADING_COMPLETE,A.COL14 AS LOADING_COMPLETE_BY,A.COL5 AS TRUCK_OUT,A.COL16 AS TRUCK_OUT_BY,round(round(to_date(A.COL5,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL2,'dd/mm/yyyy hh24:mi:SS'),2)* 24)  as time_taken,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM SCRATCH2 A,FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='TC' AND A.VCHDATE " + xprdrange + " and trim(a.col2)!='-' and trim(a.col5)!='-' ORDER BY VDD DESC,A.VCHNUM desc) where time_taken>" + hfcode.Value + " ";
                    }
                    else
                    {
                        SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,b.aname as truck_owner,A.COL1 AS TRUCKNO,A.COL2 AS TIME_IN,A.COL7 AS TRUCK_IN_BY,A.COL3 AS LOADING_TIME,A.COL12 AS LOADED_BY,(case when length(trim(nvl(a.col3,'-')))>4 then round(round(to_date(A.COL3,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL2,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takena,A.COL4 AS LOADING_COMPLETE,A.COL14 AS LOADING_COMPLETE_BY,(case when length(trim(nvl(a.col3,'-')))>4 and length(trim(nvl(a.col4,'-')))>4 then round(round(to_date(A.COL4,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL3,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takenb,A.COL5 AS TRUCK_OUT,A.COL16 AS TRUCK_OUT_BY,(case when length(trim(nvl(a.col5,'-')))>4 and length(trim(nvl(a.col4,'-')))>4 then round(round(to_date(A.COL5,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL4,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takenc,(case when length(trim(nvl(a.col5,'-')))>4 and length(trim(nvl(a.col2,'-')))>4 then round(round(to_date(A.COL5,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL2,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takend,A.COL25 AS SUPERVISIOR_NAME,A.COL21 AS LOADING_COMPLETED_PER,A.COL22 AS LOADING_DONE_BY,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM SCRATCH2 A,FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='TC' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC,A.VCHNUM desc ";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Truck Details for the period of " + fromdt + " to " + todt + cond;
                    fgen.Fn_open_rptlevel(header_n, frm_qstr);
                    break;
                case "F50137B":
                    SQuery = "SELECT ENTRYNO,ENTRYDT,TRUCK_OWNER,time_takena AS E_TO_L,time_takenB AS L_TO_C,time_takenC AS C_TO_O,time_takenD AS S_TO_E FROM (SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,b.aname as truck_owner,A.COL1 AS TRUCKNO,A.COL2 AS TIME_IN,A.COL7 AS TRUCK_IN_BY,A.COL3 AS LOADING_TIME,A.COL12 AS LOADED_BY,(case when length(trim(nvl(a.col3,'-')))>4 then round(round(to_date(A.COL3,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL2,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takena,A.COL4 AS LOADING_COMPLETE,A.COL14 AS LOADING_COMPLETE_BY,(case when length(trim(nvl(a.col3,'-')))>4 and length(trim(nvl(a.col4,'-')))>4 then round(round(to_date(A.COL4,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL3,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takenb,A.COL5 AS TRUCK_OUT,A.COL16 AS TRUCK_OUT_BY,(case when length(trim(nvl(a.col5,'-')))>4 and length(trim(nvl(a.col4,'-')))>4 then round(round(to_date(A.COL5,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL4,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takenc,(case when length(trim(nvl(a.col5,'-')))>4 and length(trim(nvl(a.col2,'-')))>4 then round(round(to_date(A.COL5,'dd/mm/yyyy hh24:mi:SS')-to_date(A.COL2,'dd/mm/yyyy hh24:mi:SS'),4)* 60 * 24) else null end) as time_takend,A.COL25 AS SUPERVISIOR_NAME,A.COL21 AS LOADING_COMPLETED_PER,A.COL22 AS LOADING_DONE_BY,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM SCRATCH2 A,FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='TC' AND A.VCHDATE " + xprdrange + ") ORDER BY VDD DESC,ENTRYNO desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Truck Details for the period of " + fromdt + " to " + todt + cond;
                    fgen.Fn_open_rptlevel(header_n, frm_qstr);
                    break;
                case "F50321":

                    #region
                    ph_tbl = new DataTable();
                    #region
                    ph_tbl.Columns.Add("Our_Order_No", typeof(string));
                    ph_tbl.Columns.Add("Date", typeof(string));
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
                    header_n = "Order Vs Bsr Stock Report";
                    xprdRange1 = "between to_Date('01/04/" + year + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1"; //for fetching day closing
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
                    {
                    }
                    mq4 = "";
                    mq4 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL5"); //selectiON VAlue
                    mq3 = "";
                    mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4"); //party
                    if (mq4 == "N") //FOR ALL ORDER
                    {
                        mq0 = "select a.branchcd,a.type, a.ordno as ordno,to_char(a.orddt,'dd/mm/yyyy') as ord_date,to_char(a.orddt,'yyyymmdd') as vdd,a.cdrgno,trim(a.acode) as acode,trim(a.icode) as icode,b.aname as party,trim(c.iname) as item,a.weight as ord_line_no,a.pordno as cust_ordno,to_Char(a.porddt,'dd/mm/yyyy') as cust_ordt,sum(a.qtyord) as order_Qty,sum(a.qtysupp) as Tolerance_Qty, nvl(a.irate,0) as irate,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as order_nal from somas a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' " + cond + " and a.acode in (" + mq3 + ") and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + part_cd + ") group by a.ordno,to_char(a.orddt,'dd/mm/yyyy'),trim(a.acode),trim(a.icode),b.aname,trim(c.iname) ,a.pordno,to_Char(a.porddt,'dd/mm/yyyy'),a.weight,a.irate,to_char(a.orddt,'yyyymmdd'),a.branchcd,a.type,a.cdrgno order by vdd,ordno asc";
                    }
                    else //FOR PENDING ORDER ONLY
                    {
                        mq0 = "select a.branchcd,a.type, a.ordno as ordno,to_char(a.orddt,'dd/mm/yyyy') as ord_date,to_char(a.orddt,'yyyymmdd') as vdd,a.cdrgno,trim(a.acode) as acode,trim(a.icode) as icode,b.aname as party,trim(c.iname) as item,a.weight as ord_line_no,a.pordno as cust_ordno,to_Char(a.porddt,'dd/mm/yyyy') as cust_ordt,sum(a.qtyord) as order_Qty,sum(a.qtysupp) as Tolerance_Qty, nvl(a.irate,0) as irate,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as order_nal from somas a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' " + cond + " and a.acode in (" + mq3 + ") and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + part_cd + ") and trim(a.icat)!='Y'  group by a.ordno,to_char(a.orddt,'dd/mm/yyyy'),trim(a.acode),trim(a.icode),b.aname,trim(c.iname) ,a.pordno,to_Char(a.porddt,'dd/mm/yyyy'),a.weight,a.irate,to_char(a.orddt,'yyyymmdd'),a.branchcd,a.type,a.cdrgno order by vdd,ordno asc"; //(trim(check_by)!='-' or trim(app_by)!='-')
                    }
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);//main dt 

                    mq0 = "";
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + cDT1 + "','dd/mm/yyyy')+600,'dd/MM/yyyy') as dd from dual", "dd");//add 600 days in date for invoice ...inv next year b ban skta hai

                    //xprdrange = "between to_date('" + cDT1 + "','dd/MM/yyyy') and to_date('" + mq0 + "','dd/MM/yyyy')";
                    //mq1 = "select a.branchcd,a.type,A.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdt ,trim(a.acode) as acode,trim(a.icode) as icode,a.prnum,sum(a.iqtyout) as sale_qty,a.binno as lineno,a.irate,sum(a.iamount) as sale_val,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate,a.prnum,b.mo_vehi  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " " + cond + " and a.acode in (" + mq3 + ") group by a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.icode) ,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy'),a.prnum,b.mo_vehi,a.irate,a.branchcd,a.type,a.binno,a.prnum order by invno,invdt asc";//OLD........IN THIS INV KABI B BANE HO AA JAYENGE BUT USER WANT DATE FILTER IN INVOICE

                    mq1 = "select a.branchcd,a.type,A.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdt ,trim(a.acode) as acode,trim(a.icode) as icode,a.prnum,sum(a.iqtyout) as sale_qty,a.binno as lineno,a.irate,sum(a.iamount) as sale_val,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate,a.prnum,b.mo_vehi  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " " + cond + " and a.acode in (" + mq3 + ") group by a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.icode) ,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy'),a.prnum,b.mo_vehi,a.irate,a.branchcd,a.type,a.binno,a.prnum order by invno,invdt asc";//OLD........IN THIS INV KABI B BANE HO AA JAYENGE BUT USER WANT DATE FILTER IN INVOICE
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //invoice dt

                    mq2 = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + " and substr(trim(icode),1,1)='9' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);//stock dt
                    double totbal = 0; int cnt = 0; double BSR_sTK = 0; double ord_line_qty = 0, qty2 = 0;
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "acode", "icode"); //MAIN      
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            ord_line_qty = 0; qty2 = 0;
                            dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();
                            DataView viewim = new DataView(dt, "acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "' ", "", DataViewRowState.CurrentRows);
                            dt5 = viewim.ToTable();//icode base from somas
                            totbal = 0;
                            ////////////////////        
                            dr1 = ph_tbl.NewRow();
                            BSR_sTK = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dr0["icode"].ToString().Trim() + "'", "Closing_Stk"));
                            foreach (DataRow dr2 in dt5.Rows)
                            {
                                DataView viewim1 = new DataView(dt, "branchcd='" + dr2["branchcd"].ToString().Trim() + "' and type='" + dr2["type"].ToString().Trim() + "' and acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "' and ordno='" + dr2["ordno"].ToString().Trim() + "' and ord_date='" + dr2["ord_date"].ToString().Trim() + "' and cdrgno='" + dr2["cdrgno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt3 = viewim1.ToTable(); //somas view                              
                                ////invoice view
                                if (dt1.Rows.Count > 0)
                                {
                                    DataView viewim2 = new DataView(dt1, "branchcd='" + dr2["branchcd"].ToString().Trim() + "' and type='" + dr2["type"].ToString().Trim() + "' and acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "' and ponum='" + dr2["ordno"].ToString().Trim() + "' and podate='" + dr2["ord_date"].ToString().Trim() + "' and prnum='" + dr2["cdrgno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dt4 = viewim2.ToTable();//inv view
                                }
                                db6 = 0;//for bal order qty
                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    #region order details
                                    db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; double min = 0, var = 0;
                                    dr1 = ph_tbl.NewRow();
                                    dr1["Our_Order_No"] = dt3.Rows[i]["ordno"].ToString().Trim();
                                    dr1["Date"] = dt3.Rows[i]["ord_date"].ToString().Trim();
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
                                            db1 = fgen.make_double(dr1["Order_Qty"].ToString().Trim());
                                        }
                                        dr1["Sale_Qty"] = dt4.Rows[j]["sale_qty"].ToString().Trim();
                                        db2 = fgen.make_double(dr1["Sale_Qty"].ToString().Trim());
                                        db3 = db1 - db2;
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
                                            if (db6 > 0)
                                            {
                                                totbal = totbal + db3;
                                            }
                                        }
                                        else
                                        {
                                            dr1["Balance_Order_Qty"] = db6 - db2;
                                            db6 = fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                            if (db6 > 0)
                                            {
                                                totbal = totbal + db6;
                                            }
                                        }
                                        dr1["Rate"] = fgen.make_double(dt4.Rows[j]["irate"].ToString().Trim());
                                        //  dr1["Bsr_Stock"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt4.Rows[j]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                        db10 = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim());
                                        #endregion
                                        ph_tbl.Rows.Add(dr1);
                                    }
                                    cnt = 0;
                                    cnt = ph_tbl.Rows.IndexOf(dr1);
                                    if (fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim()) > 0)
                                    {
                                        ord_line_qty += fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim()); //order line ke base pr +ve value ka sum as per icode
                                    }
                                    if (dt4.Rows.Count == 0)
                                    {//agar koi b invoice na bani ho then this work                                        
                                        #region
                                        db3 = db1;//bal order qty
                                        if (db3 > 0)
                                        {
                                            db4 = fgen.make_double(dt3.Rows[i]["Tolerance_Qty"].ToString().Trim().Split('.')[0].ToString());
                                            dr1["Tolerance_Qty"] = db4;
                                        }
                                        else
                                        {
                                            dr1["Tolerance_Qty"] = 0;
                                        }
                                        //dr1["Bsr_Stock"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt3.Rows[i]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                        db11 = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim());
                                        dr1["Balance_Order_Qty"] = db3;
                                        if (db3 > 0)
                                        {
                                            totbal = totbal + db3;
                                        }
                                        if (fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim()) > 0)
                                        {
                                            ord_line_qty = +fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim()); //order line ke base pr +ve value ka sum as per icode
                                        }
                                        ph_tbl.Rows.Add(dr1);
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            cnt = 0;
                            cnt = ph_tbl.Rows.IndexOf(dr1);
                            ph_tbl.Rows[cnt]["Bsr_Stock"] = BSR_sTK;
                            if (ord_line_qty > 0)
                            {
                                qty2 = ord_line_qty - BSR_sTK;
                                if (qty2 > 0)
                                {
                                    ph_tbl.Rows[cnt]["Bal_Order_Req_To_Desp_Bsr_Qty"] = 0;
                                }
                                else
                                {
                                    ph_tbl.Rows[cnt]["Bal_Order_Req_To_Desp_Bsr_Qty"] = qty2;
                                }
                            }
                            else
                            {
                                ph_tbl.Rows[cnt]["Bal_Order_Req_To_Desp_Bsr_Qty"] = BSR_sTK;
                            }
                        }//item view close
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Data Not Found");
                    }
                    #endregion
                    break;

                case "F50269"://phgl report
                    #region
                    #region
                    header_n = "Customer Cash Disc";
                    dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable();
                    dtm.Columns.Add("sno", typeof(int));
                    dtm.Columns.Add("Invoice_No", typeof(string));
                    dtm.Columns.Add("Invoice_Date", typeof(string));
                    dtm.Columns.Add("Customer_Code", typeof(string));
                    dtm.Columns.Add("Customer_Name", typeof(string));
                    dtm.Columns.Add("TSI_Name", typeof(string));
                    dtm.Columns.Add("ASM_Name", typeof(string));
                    dtm.Columns.Add("RSM_Name", typeof(string));
                    dtm.Columns.Add("Segment", typeof(string));
                    dtm.Columns.Add("Invoiced_Amount", typeof(double));//gross 
                    dtm.Columns.Add("W_O_Tax_Invoice_Value", typeof(double));   //basic              
                    dtm.Columns.Add("Invoice_Due_Date", typeof(string));//invdate+pymt term
                    dtm.Columns.Add("Payment_Amt_LineWise", typeof(double));//rec payment...cramt from voucher...this is gross value...with all tax
                    dtm.Columns.Add("W_O_Tax_Payment_Value", typeof(double));//same as above but basic value.....without tax                  
                    dtm.Columns.Add("Payment_Doc_No", typeof(string)); //voucher no
                    dtm.Columns.Add("Payment_Date", typeof(string));//voucher date
                    dtm.Columns.Add("Realization_Days", typeof(double));//pymt date-inv date
                    dtm.Columns.Add("Balance_Due", typeof(double));//basic-Payment_Amt_LineWise                   
                    dtm.Columns.Add("CD_Percentage_3.0", typeof(double)); //for 0-7 days   3%
                    dtm.Columns.Add("CD_Percentage_2.5", typeof(double));//8-14 days  2.5 %
                    dtm.Columns.Add("CD_Percentage_2.0", typeof(double));//15-30 days  2.0%
                    dtm.Columns.Add("CD_Percentage_1.0", typeof(double));//31-45   1.0%
                    #endregion
                    mq0 = ""; mq1 = ""; int sno = 1;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (party_cd.Length < 2)
                    {
                        mq0 = "and substr(trim(a.acode),1,2)='16'";
                    }
                    else
                    {
                        mq0 = "and a.acode in (" + party_cd + ")";
                    }
                    //SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,a.branchcd, a.st_Type,a.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdate,trim(a.acode) as acode,sum(nvl(a.amt_sale,0))  as basic,sum(nvl(a.bill_tot,0)) as gross from sale a where A." + branch_Cd + " and a.type like '4%'  and a.vchdate>to_date('31/03/2018','dd/mm/yyyy')  " + mq0 + " group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode),a.st_Type,a.branchcd order by a.branchcd,invno";
                    SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,a.branchcd, b.st_Type,a.vchnum,a.invno,to_char(a.vchdate,'dd/mm/yyyy') as invdate,trim(a.acode) as acode,sum(nvl(b.amt_sale,0))  as basic,sum(nvl(b.bill_tot,0)) as gross from ivoucher a ,sale b where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode) and A." + branch_Cd + " and a.type like '4%'  and a.vchdate>to_date('31/03/2018','dd/mm/yyyy')  " + mq0 + " group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode),b.st_Type,a.branchcd,a.invno order by a.branchcd,a.invno"; //change on 5sept 2020 asper prefix is showing in voucher
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);//invoice 

                    mq1 = "select A.BRANCHCD, A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,b.aname,B.mktggrp as tsi,substr(trim(mktggrp),1,3) as tsi_code,B.PAYMENT AS PYMT_TERM,B.del_note as segment,SUM(NVL(A.CRAMT,0)) AS REC_PYMT,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdt from voucher A,FAMST B where trim(a.acode)=trim(b.acode) and A." + branch_Cd + " AND substr(trim(a.type),1,1)='1'  and a.vchdate " + xprdrange + " " + mq0 + " group by  A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),TRIM(A.ACODE),A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy'),b.aname,B.PAYMENT,B.del_note,B.mktggrp,substr(trim(b.mktggrp),1,3),A.BRANCHCD ORDER BY A.BRANCHCD,acode,a.vchnum";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);//main dt

                    mq2 = "select TYPE1,NAME,ACREF AS ASM,ACREF2 AS RSM  from typegrp where id='EM'  ORDER BY TYPE1 ";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq2);//asm and rsm master dt
                    ///
                    mq3 = "SELECT DISTINCT trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,a.branchcd,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.vchnum,a.invno,trim(a.acode) as acode,a.iopr,(case when a.iopr='IG'  then a.exc_rate else 0 end) as igst,(case when a.iopr='CG' then a.exc_rate else 0 end) as cgst,a.cess_percent AS SGST FROM  ivoucher a WHERE A." + branch_Cd + " and a.type like '4%' " + mq0 + " and a.vchdate " + DateRange + "";
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq3);//ivch table

                    if (dt1.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt1);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "acode", "VCHNUM", "vchdate"); //MAIN     
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt1, "acode='" + dr0["acode"].ToString().Trim() + "' and VCHNUM='" + dr0["VCHNUM"].ToString().Trim() + "' and vchdate='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt2 = viewim.ToTable();//main dt
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                #region
                                dr2 = dtm.NewRow();
                                mq3 = ""; db = 0; int days = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; mq4 = "";
                                mq3 = dt2.Rows[i]["PYMT_TERM"].ToString().Trim();
                                dr2["sno"] = sno++;
                                dr2["Invoice_No"] = dt2.Rows[i]["invno"].ToString().Trim();
                                dr2["Invoice_Date"] = dt2.Rows[i]["invdt"].ToString().Trim();
                                dr2["Customer_Code"] = dt2.Rows[i]["ACODE"].ToString().Trim();
                                dr2["Customer_Name"] = dt2.Rows[i]["aname"].ToString().Trim();
                                dr2["TSI_Name"] = dt2.Rows[i]["tsi"].ToString().Trim();
                                dr2["ASM_Name"] = fgen.seek_iname_dt(dt3, "type1='" + dt2.Rows[i]["tsi_code"].ToString().Trim() + "'", "ASM");
                                dr2["RSM_Name"] = fgen.seek_iname_dt(dt3, "type1='" + dt2.Rows[i]["tsi_code"].ToString().Trim() + "'", "RSM");
                                dr2["Segment"] = dt2.Rows[i]["segment"].ToString().Trim();
                                dr2["Invoiced_Amount"] = fgen.make_double(fgen.seek_iname_dt(dt, "invno='" + dr2["Invoice_No"].ToString().Trim() + "' and invdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "gross"));
                                dr2["W_O_Tax_Invoice_Value"] = fgen.make_double(fgen.seek_iname_dt(dt, "invno='" + dr2["Invoice_No"].ToString().Trim() + "' and invdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "basic"));
                                mq4 = fgen.seek_iname_dt(dt, "invno='" + dr2["Invoice_No"].ToString().Trim() + "' and invdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "st_Type");
                                db = fgen.make_double(dr2["W_O_Tax_Invoice_Value"].ToString().Trim());//inv amt
                                dr2["Invoice_Due_Date"] = Convert.ToDateTime(fgen.seek_iname(frm_qstr, co_cd, "SELECT TO_dATE('" + dr2["Invoice_Date"].ToString().Trim() + "','DD/MM/YYYY')+" + mq3 + " AS DUEDATE FROM DUAL", "DUEDATE")).ToString("dd/MM/yyyy");
                                dr2["Payment_Amt_LineWise"] = fgen.make_double(dt2.Rows[i]["REC_PYMT"].ToString().Trim());
                                if (mq4 == "IG")
                                {
                                    //db3 = fgen.make_double(fgen.seek_iname_dt(dt4, "vchnum='" + dr2["Invoice_No"].ToString().Trim() + "' and vchdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "igst"));//old
                                    db3 = fgen.make_double(fgen.seek_iname_dt(dt4, "invno='" + dr2["Invoice_No"].ToString().Trim() + "' and vchdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "igst"));
                                    db5 = 100 + db3;
                                }
                                else if (mq4 == "CG")
                                {
                                    //db3 = fgen.make_double(fgen.seek_iname_dt(dt4, "vchnum='" + dr2["Invoice_No"].ToString().Trim() + "' and vchdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "cgst"));//old
                                    //db4 = fgen.make_double(fgen.seek_iname_dt(dt4, "vchnum='" + dr2["Invoice_No"].ToString().Trim() + "' and vchdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "sgst"));//old
                                    db3 = fgen.make_double(fgen.seek_iname_dt(dt4, "invno='" + dr2["Invoice_No"].ToString().Trim() + "' and vchdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "cgst"));
                                    db4 = fgen.make_double(fgen.seek_iname_dt(dt4, "invno='" + dr2["Invoice_No"].ToString().Trim() + "' and vchdate='" + dr2["Invoice_Date"].ToString().Trim() + "' and acode='" + dr2["Customer_Code"].ToString().Trim() + "'", "sgst"));
                                    db5 = 100 + db3 + db4;
                                }
                                if (db5 > 0)
                                {
                                    dr2["W_O_Tax_Payment_Value"] = Math.Round(fgen.make_double(dr2["Payment_Amt_LineWise"].ToString().Trim()) / db5 * 100, 4);
                                }
                                else
                                {
                                    dr2["W_O_Tax_Payment_Value"] = 0;
                                }
                                db2 = fgen.make_double(dr2["W_O_Tax_Payment_Value"].ToString().Trim());//basi amt   
                                dr2["Payment_Doc_No"] = dt2.Rows[i]["VCHNUM"].ToString().Trim();
                                dr2["Payment_Date"] = dt2.Rows[i]["VCHDATE"].ToString().Trim();
                                dr2["Realization_Days"] = fgen.seek_iname(frm_qstr, co_cd, "select to_date('" + dr2["Payment_Date"].ToString().Trim() + "','DD/MM/YYYY')-TO_dATE('" + dr2["Invoice_Date"].ToString().Trim() + "','DD/MM/YYYY') AS Real_Days from dual", "Real_Days");
                                days = Convert.ToInt32(dr2["Realization_Days"].ToString().Trim());
                                dr2["Balance_Due"] = Math.Round(fgen.make_double(dr2["W_O_Tax_Invoice_Value"].ToString().Trim()) - fgen.make_double(dr2["Payment_Amt_LineWise"].ToString().Trim()), 4);
                                //==============================if days 0-7 then 3.0 disc will come
                                if (days == 0 || days == 1 || days == 2 || days == 3 || days == 4 || days == 5 || days == 6 || days == 7 || days < 0)
                                {
                                    if (db2 < db || db2 == db)
                                    {
                                        db1 = db2 * 3.0 / 100;
                                    }
                                    else if (db2 > db)
                                    {
                                        db1 = db * 3.0 / 100;
                                    }
                                    dr2["CD_Percentage_3.0"] = Math.Round(db1, 4);
                                }
                                else
                                {
                                    dr2["CD_Percentage_3.0"] = 0;
                                }
                                if (days == 8 || days == 9 || days == 10 || days == 11 || days == 12 || days == 13 || days == 14)
                                {
                                    if (db2 < db || db2 == db)
                                    {
                                        db1 = db2 * 2.5 / 100;
                                    }
                                    else if (db2 > db)
                                    {
                                        db1 = db * 2.5 / 100;
                                    }
                                    dr2["CD_Percentage_2.5"] = Math.Round(db1, 4);
                                }
                                else
                                {
                                    dr2["CD_Percentage_2.5"] = 0;
                                }
                                if (days == 15 || days == 16 || days == 17 || days == 18 || days == 19 || days == 20 || days == 21 || days == 22 || days == 23 || days == 24 || days == 25 || days == 26 || days == 27 || days == 28 || days == 29 || days == 30)
                                {
                                    if (db2 < db || db2 == db)
                                    {
                                        db1 = db2 * 2.0 / 100;
                                    }
                                    else if (db2 > db)
                                    {
                                        db1 = db * 2.0 / 100;
                                    }
                                    dr2["CD_Percentage_2.0"] = Math.Round(db1, 4);
                                }
                                else
                                {
                                    dr2["CD_Percentage_2.0"] = 0;
                                }
                                if (days == 31 || days == 32 || days == 33 || days == 34 || days == 35 || days == 36 || days == 37 || days == 38 || days == 39 || days == 40 || days == 41 || days == 42 || days == 43 || days == 44 || days == 45)
                                {
                                    if (db2 < db || db2 == db)
                                    {
                                        db1 = db2 * 1.0 / 100;
                                    }
                                    else if (db2 > db)
                                    {
                                        db1 = db * 1.0 / 100;
                                    }
                                    dr2["CD_Percentage_1.0"] = Math.Round(db1, 4);
                                }
                                else
                                {
                                    dr2["CD_Percentage_1.0"] = 0;
                                }
                                dtm.Rows.Add(dr2);
                                #endregion
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F50276":
                    party_cd = "";
                    part_cd = "";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq0 = hfcode.Value;
                    if (mq0.Length <= 1)
                    { cond = "f.staten like '%'"; }
                    else { cond = "f.staten in (" + mq0 + ")"; }

                    if (party_cd.Length <= 1)
                    {
                        mq1 = "and substr(trim(icode),1,2) like '%'";
                    }
                    else
                    {
                        mq1 = "and substr(trim(icode),1,2) = '" + party_cd + "'";
                    }
                    if (part_cd.Length <= 1)
                    {
                        mq2 = "and substr(trim(icode),1,4) like '%'";
                    }
                    else
                    {
                        mq2 = "and substr(trim(icode),1,4) in (" + part_cd + ")";
                    }
                    SQuery = "select trim(f.staten) as state,sum(a.apr_qty) as apr_qty,sum(a.apr_val) as apr_val,sum(a.may_qty) as may_qty,sum(a.may_val) as may_val,sum(a.june_qty) as june_qty,sum(a.june_val) as june_val,sum(a.july_qty) as july_qty,sum(a.july_val) as july_val,sum(a.aug_qty) as aug_qty,sum(a.aug_val) as aug_val,sum(a.sep_qty) as sep_qty,sum(a.sep_val) as sep_val,sum(a.oct_qty) as oct_qty,sum(a.oct_val) as oct_val,sum(a.nov_qty) as nov_qty,sum(a.nov_val) as nov_val,sum(a.dec_qty) as dec_qty,sum(a.dec_val) as dec_val,sum(a.jan_qty) as jan_qty,sum(a.jan_val) as jan_val,sum(a.feb_qty) as feb_qty,sum(a.feb_val) as feb_val,sum(a.mar_qty) as mar_qty,sum(a.mar_val) as mar_val,sum(a.apr_qty)+sum(a.may_qty)+sum(a.june_qty)+sum(a.july_qty)+sum(a.aug_qty)+sum(a.sep_qty)+sum(a.oct_qty)+sum(a.nov_qty)+sum(a.dec_qty)+sum(a.jan_qty)+sum(a.feb_qty)+sum(a.mar_qty) as total_qty,sum(a.apr_val)+sum(a.may_val)+sum(a.june_val)+sum(a.july_val)+sum(a.aug_val)+sum(a.sep_val)+sum(a.oct_val)+sum(a.nov_val)+sum(a.dec_val)+sum(a.jan_val)+sum(a.feb_val)+sum(a.mar_val) as total_val from (select trim(icode) as icode,trim(acode) as acode, (case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as apr_qty,(case when to_char(vchdate,'mm')='04' then iamount else 0 end) as apr_val,(case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as may_qty,(case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may_val,(case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as june_qty,(case when to_char(vchdate,'mm')='06' then iamount else 0 end) as june_val,(case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as july_qty,(case when to_char(vchdate,'mm')='07' then iamount else 0 end) as july_val,(case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as aug_qty,(case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug_val,(case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as sep_qty,(case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep_val,(case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as oct_qty,(case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct_val,(case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as nov_qty,(case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov_val,(case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as dec_qty,(case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec_val,(case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as jan_qty,(case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan_val,(case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as feb_qty,(case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb_val,(case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as mar_qty,(case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar_val from ivoucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and vchdate " + xprdrange + " and nvl(iqtyout,0)>0 " + mq1 + " " + mq2 + ") a, famst f where trim(a.acode)=trim(f.acode) and " + cond + " group by trim(f.staten) order by state";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        oporow = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            to_cons = 0;
                            if (dc.Ordinal == 0)
                            {

                            }
                            else
                            {
                                mq1 = "sum(" + dc.ColumnName + ")";
                                to_cons += fgen.make_double(dt.Compute(mq1, "").ToString());
                                oporow[dc] = to_cons;
                            }
                        }
                        oporow["state"] = "TOTAL";
                        dt.Rows.Add(oporow);
                    }
                    Session["send_dt"] = dt;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    header_n = "State Sales Summary , Monthwise Report With Qty And Value From " + fromdt + " To " + todt;
                    fgen.Fn_open_rptlevel(header_n, frm_qstr);
                    break;

                case "F50277":
                    #region
                    header_n = "Statewise, Groupwise, Subgroupwise, Sale Summary Report";
                    dt10 = new DataTable();
                    dt10.Columns.Add("State", typeof(string));
                    dt10.Columns.Add("GroupCode", typeof(string));
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
                    SQuery = "select  a.MGCODE,t.name as mgname,a.subcode,d.iname as subname,trim(f.staten) as state,sum(a.apr_qty) as apr_qty,sum(a.apr_val) as apr_val,sum(a.may_qty) as may_qty,sum(a.may_val) as may_val,sum(a.june_qty) as june_qty,sum(a.june_val) as june_val,sum(a.july_qty) as july_qty,sum(a.july_val) as july_val,sum(a.aug_qty) as aug_qty,sum(a.aug_val) as aug_val,sum(a.sep_qty) as sep_qty,sum(a.sep_val) as sep_val,sum(a.oct_qty) as oct_qty,sum(a.oct_val) as oct_val,sum(a.nov_qty) as nov_qty,sum(a.nov_val) as nov_val,sum(a.dec_qty) as dec_qty,sum(a.dec_val) as dec_val,sum(a.jan_qty) as jan_qty,sum(a.jan_val) as jan_val,sum(a.feb_qty) as feb_qty,sum(a.feb_val) as feb_val,sum(a.mar_qty) as mar_qty,sum(a.mar_val) as mar_val,sum(a.apr_qty)+sum(a.may_qty)+sum(a.june_qty)+sum(a.july_qty)+sum(a.aug_qty)+sum(a.sep_qty)+sum(a.oct_qty)+sum(a.nov_qty)+sum(a.dec_qty)+sum(a.jan_qty)+sum(a.feb_qty)+sum(a.mar_qty) as total_qty,sum(a.apr_val)+sum(a.may_val)+sum(a.june_val)+sum(a.july_val)+sum(a.aug_val)+sum(a.sep_val)+sum(a.oct_val)+sum(a.nov_val)+sum(a.dec_val)+sum(a.jan_val)+sum(a.feb_val)+sum(a.mar_val) as total_val from (select substr(trim(icode),1,2) AS MGCODE,substr(trim(icode),1,4) as SUBCODE,trim(acode) as acode, (case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as apr_qty,(case when to_char(vchdate,'mm')='04' then iamount else 0 end) as apr_val,(case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as may_qty,(case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may_val,(case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as june_qty,(case when to_char(vchdate,'mm')='06' then iamount else 0 end) as june_val,(case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as july_qty,(case when to_char(vchdate,'mm')='07' then iamount else 0 end) as july_val,(case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as aug_qty,(case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug_val,(case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as sep_qty,(case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep_val,(case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as oct_qty,(case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct_val,(case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as nov_qty,(case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov_val,(case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as dec_qty,(case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec_val,(case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as jan_qty,(case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan_val,(case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as feb_qty,(case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb_val,(case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as mar_qty,(case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar_val from ivoucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and vchdate " + xprdrange + " and nvl(iqtyout,0)>0) a, famst f,type t,ITEM D where trim(a.acode)=trim(f.acode) " + mq3 + " " + part_cd + " " + party_cd + " and trim(a.mgcode)=trim(t.type1) and t.id='Y' and trim(a.subcode)=trim(d.icode) and length(trim(d.icode))=4 group by trim(f.staten),a.MGCODE,a.subcode,t.name,d.iname order by state,a.MGCODE,a.subcode";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "state"); //MAIN      
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            dt4 = new DataTable(); dt5 = new DataTable();
                            DataView viewim = new DataView(dt, "state='" + dr0["state"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt4 = viewim.ToTable();//state
                            dt6 = viewim.ToTable(true, "mgcode", "state");//DISTINCT MAIN GRP
                            dr1 = dt10.NewRow();
                            dr1["State"] = dr0["state"].ToString().Trim();
                            dt10.Rows.Add(dr1);
                            dt10.Rows.Add();
                            foreach (DataRow dr2 in dt6.Rows)
                            {
                                DataView viewim1 = new DataView(dt4, "MGCODE='" + dr2["MGCODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt5 = viewim1.ToTable();//mg view

                                for (int i = 0; i < dt5.Rows.Count; i++)
                                {
                                    #region
                                    if (i == 0)
                                    {
                                        dr1 = dt10.NewRow();
                                        dr1["State"] = dt5.Rows[i]["mgname"].ToString().Trim();
                                        dr1["GroupCode"] = dt5.Rows[i]["mgcode"].ToString().Trim();
                                        dt10.Rows.Add(dr1);
                                    }
                                    dr1 = dt10.NewRow();
                                    dr1["State"] = dt5.Rows[i]["subname"].ToString().Trim();
                                    dr1["GroupCode"] = dt5.Rows[i]["subcode"].ToString().Trim();
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
                                dt10.Rows.Add();
                            }
                        }
                    }
                    if (dt10.Rows.Count > 0)
                    {
                        oporow = null;
                        oporow = dt10.NewRow();
                        foreach (DataColumn dc in dt10.Columns)
                        {
                            to_cons = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1)// dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 8)
                            {
                            }
                            else
                            {
                                mq1 = "sum(" + dc.ColumnName + ")";
                                to_cons += fgen.make_double(dt10.Compute(mq1, "").ToString());
                                oporow[dc] = to_cons;
                            }
                        }
                        oporow["GroupCode"] = "TOTAL";
                        dt10.Rows.Add(oporow);
                    }
                    if (dt10.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt10;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F50273":
                    #region
                    mq0 = hfcode.Value;//TYPE
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    header_n = "Pending Job Order List";
                    dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();
                    dtm.Columns.Add("SO_No", typeof(string));
                    dtm.Columns.Add("SO_Date", typeof(string));
                    dtm.Columns.Add("Job_No", typeof(string));
                    dtm.Columns.Add("Customer_Code", typeof(string));
                    dtm.Columns.Add("Customer_Name", typeof(string));
                    dtm.Columns.Add("Item_Code", typeof(string));
                    dtm.Columns.Add("Item_Name", typeof(string));
                    dtm.Columns.Add("Order_Bal_Qty", typeof(double));
                    dtm.Columns.Add("Cylinder", typeof(string));
                    dtm.Columns.Add("M_C", typeof(string));//
                    dtm.Columns.Add("SUBSTRATE", typeof(string));
                    dtm.Columns.Add("FG_Stock", typeof(double));
                    dtm.Columns.Add("Running_Mtr", typeof(double));
                    cond = ""; mq7 = "";
                    if (party_cd.Length < 2)
                    {
                        mq1 = "and substr(trim(a.acode),1,2)='16'";
                        mq7 = "and substr(trim(acode),1,2)='16'";
                    }
                    else
                    {
                        mq1 = "and a.acode in (" + party_cd + ")";
                        mq7 = "and acode in (" + party_cd + ")";
                    }
                    if (part_cd.Length > 2)
                    {
                        cond = "and trim(icode) in (" + part_cd + ")";
                        mq2 = "and trim(a.icode) in (" + part_cd + ")";
                    }
                    else
                    {
                        cond = "and trim(icode) like '9%'";
                        mq2 = "and trim(a.icode) like '9%'";
                    }
                    xprdrange1 = "between to_Date('01/04/" + year + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";

                    SQuery = "select TRIM(A.BRANCHCD)||A.TYPE||A.ORDNO||TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR, A.ORDNO AS SO_NO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS SO_Date,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD,TRIM(A.ACODE) AS ACODE,TRIM(a.ANAME) AS PARTY,TRIM(A.ICODE) AS ICODE,a.ciname as ITEMNAME,sum(NVL(A.QTYORD,0)) AS QTY,SUM(A.bal) AS Bal_Qty from (select a.branchcd,a.type,a.ordno,a.orddt,a.pordno,a.porddt,a.acode,a.aname,a.icode,a.ciname,a.cpartno,a.st_type,a.amdt1,a.tarrifno,A.TARRIFRATE ,a.nsp_flag,a.thru,a.cdisc,a.ipack,a.irate,a.class as Packperct,a.qtyord,nvl(b.qtyout,0) Qty_out,a.qtyord-nvl(b.qtyout,0) bal from (select s.branchcd,s.type,s.ordno,s.orddt,s.pordno,s.porddt,s.acode, f.aname,s.icode,s.ciname ,s.cpartno,s.thru,s.st_type,s.amdt1,s.cdisc,s.ipack,s.class,s.irate,s.qtyord,i.tarrifno,I.TARRIFRATE,i.nsp_flag from somas s , famst f,item i  where trim(s.icode)=trim(i.icode) and s.branchcd<>'AM' AND S.ICAT!='Y' and trim(f.acode)=trim(s.acode) and substr(s.type,1,1)='4')a left outer join (select type,branchcd,podate,ponum,acode,icode,sum(iqtyout) qtyout from ivoucher group by branchcd,type,ponum,podate,acode,icode) b on trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)=trim(b.branchcd)||trim(b.type)||trim(b.ponum)||to_char(b.podate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode) ) a where  A.BRANCHCD='" + mbr + "' AND A.TYPE in (" + mq0 + ") AND A.ORDDT " + xprdrange + " " + mq1 + " " + mq2 + "   group by TRIM(A.BRANCHCD)||A.TYPE||A.ORDNO||TO_cHAR(A.ORDDT,'DD/MM/YYYY'), A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY'),TO_CHAR(A.ORDDT,'YYYYMMDD'),TRIM(A.ACODE),TRIM(a.ANAME),TRIM(A.ICODE),a.ciname  having SUM(A.bal)>0 order by VDD,SO_NO ASC";
                    dt4 = fgen.getdata(frm_qstr, co_cd, SQuery); //main dt
                    ////
                    mq3 = "select  distinct trim(a.acode) as acode,trim(a.icode) as icode,a.srno,a.col1,a.col2,col13  from inspmst a where a.branchcd='" + mbr + "' and a.type='70' " + mq1 + " " + mq2 + " order by a.srno";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq3); //process plan

                    mq4 = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdrange1 + "  and store='Y' " + cond + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond + "  GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq4);//stock dt

                    mq4 = "SELECT DISTINCT is_number(COL14) AS RMTR,substr(CONVDATE,1,20) as CONVDATE,trim(ACODE) as acode,trim(ICODE) as icode,VCHNUM AS JOB_NO FROM COSTESTIMATE WHERE BRANCHCD ='" + mbr + "' AND TYPE='30'  " + cond + " ORDER BY JOB_NO";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq4);//jobcard dt

                    dr1 = dtm.NewRow();
                    if (dt4.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt4);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "ACODE", "ICODE"); //MAIN  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt4, "ACODE='" + dr0["ACODE"].ToString().Trim() + "' and ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt = viewim.ToTable();//main dt
                            //=======
                            if (dt1.Rows.Count > 0)
                            {
                                DataView viewim2 = new DataView(dt1, "acode='" + dr0["ACODE"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt3 = viewim2.ToTable(); //process plan view
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                #region
                                mq4 = ""; mq5 = ""; mq6 = "";
                                dr1 = dtm.NewRow();
                                mq6 = dt.Rows[i]["FSTR"].ToString().Trim();
                                dr1["SO_No"] = dt.Rows[i]["SO_NO"].ToString().Trim();
                                dr1["SO_Date"] = dt.Rows[i]["SO_Date"].ToString().Trim();
                                dr1["Customer_Code"] = dt.Rows[i]["ACODE"].ToString().Trim();
                                dr1["Customer_Name"] = dt.Rows[i]["PARTY"].ToString().Trim();
                                dr1["Item_Code"] = dt.Rows[i]["ICODE"].ToString().Trim();
                                dr1["Item_Name"] = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                                dr1["Order_Bal_Qty"] = fgen.make_double(dt.Rows[i]["Bal_Qty"].ToString().Trim());
                                for (int j = 0; j < dt3.Rows.Count; j++)
                                {
                                    mq4 = dt3.Rows[j]["col1"].ToString().Trim();
                                    if (mq4.Contains("MACHINE"))
                                    {
                                        dr1["M_C"] = dt3.Rows[j]["col2"].ToString().Trim();
                                    }
                                    if (mq4.Contains("SUBSTRATE"))
                                    {
                                        dr1["SUBSTRATE"] = dt3.Rows[j]["col2"].ToString().Trim();
                                    }
                                    dr1["Cylinder"] = dt3.Rows[j]["col13"].ToString().Trim();
                                }
                                dr1["FG_Stock"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["ICODE"].ToString().Trim() + "'", "Closing_Stk");
                                dr1["Running_Mtr"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt5, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["ICODE"].ToString().Trim() + "' and CONVDATE='" + mq6 + "'", "RMTR")), 0);
                                dr1["Job_No"] = fgen.make_double(fgen.seek_iname_dt(dt5, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["ICODE"].ToString().Trim() + "' and CONVDATE='" + mq6 + "'", "JOB_NO"));
                                dtm.Rows.Add(dr1);
                                #endregion
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F50274":
                    #region
                    header_n = "Production Report";
                    dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable();
                    dtm.Columns.Add("Job_No", typeof(string));
                    dtm.Columns.Add("Job_Dt", typeof(string));
                    dtm.Columns.Add("Party", typeof(string));
                    dtm.Columns.Add("Stage", typeof(string));
                    dtm.Columns.Add("Type_in_System", typeof(string));
                    dtm.Columns.Add("Input_in_KG", typeof(double));
                    dtm.Columns.Add("Output_in_KG", typeof(double));
                    dtm.Columns.Add("Difference", typeof(double));
                    dtm.Columns.Add("Wastage_Percentage", typeof(double));

                    SQuery = "select distinct sum(nvl(a.iqtyout,0)) as qty_GIVEN,sum(nvl(a.iqty_chl,0)) as req_qty,a.stage,a.invno as jobno,to_char(A.invdate,'dd/mm/yyyy') as jobdate  from ivoucher A where A.branchcd='" + mbr + "' and A.type='31' and A.vchdate " + xprdrange + " group by a.stage,a.invno,to_char(A.invdate,'dd/mm/yyyy') order by jobno desc";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //MAIN DT

                    mq0 = "select  a.enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,a.col21,b.name,round(sum(a.op_Qty),2) as op_qty,round(sum(a.inp_qty),2) as inp_qty from (select enqno,enqdt,type,col21,qty as op_QTY , 0 as inp_qty from costestimatek where branchcd='" + mbr + "' and type='40' and vchdate " + xprdrange + "  union all select enqno,enqdt,type,col21,0 as op_QTY,is_number(col4) as inp_qty from costestimatek where branchcd='" + mbr + "' and type='25' and vchdate " + xprdrange + " ) a,typegrp b where trim(a.col21)=trim(b.acref) and b.id='WI'  group by a.enqno,a.col21,to_char(a.enqdt,'dd/mm/yyyy') ,b.name order by a.col21";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0); //job card stages

                    mq1 = "select  distinct a.vchnum as jobno,to_char(a.vchdate,'dd/mm/yyyy') as jobdate, trim(a.acode) as acode,b.aname as party from costestimate a,famst b where  a.branchcd='" + mbr + "' and a.type='30' and trim(a.acode)=trim(b.acode) and a.srno='1'  order by jobno desc";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq1); //main jobcard dt for party nane                  

                    dt4 = new DataTable(); dt5 = new DataTable();
                    db = 0; db1 = 0; db2 = 0;
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "jobno", "jobdate"); //MAIN  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "jobno='" + dr0["jobno"].ToString().Trim() + "' and jobdate='" + dr0["jobdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt4 = viewim.ToTable();//main dt
                            //=======
                            if (dt1.Rows.Count > 0)
                            {
                                DataView viewim2 = new DataView(dt1, "enqno='" + dr0["jobno"].ToString().Trim() + "' and enqdt='" + dr0["jobdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt5 = viewim2.ToTable(); //stages view
                            }
                            dr1 = dtm.NewRow(); mq0 = "";
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                #region
                                mq0 = dt4.Rows[i]["stage"].ToString().Trim();
                                dr1 = dtm.NewRow();
                                dr1["Job_No"] = dt4.Rows[i]["jobno"].ToString().Trim();
                                dr1["Job_Dt"] = dt4.Rows[i]["jobdate"].ToString().Trim();
                                dr1["Party"] = fgen.seek_iname_dt(dt2, "jobno='" + dr0["jobno"].ToString().Trim() + "' and jobdate='" + dr0["jobdate"].ToString().Trim() + "'", "party");
                                dr1["Stage"] = "STORE";
                                dr1["Type_in_System"] = "31";
                                dr1["Input_in_KG"] = fgen.make_double(dt4.Rows[i]["req_qty"].ToString().Trim().Replace("Infinity", "0").Replace("Nan", "0"));
                                dr1["Output_in_KG"] = fgen.make_double(dt4.Rows[i]["qty_GIVEN"].ToString().Trim());
                                db1 = fgen.make_double(dr1["Output_in_KG"].ToString().Trim());//for last row
                                db2 = fgen.make_double(dr1["Input_in_KG"].ToString().Trim());
                                dr1["Difference"] = 0;
                                dtm.Rows.Add(dr1);
                                ///-===========
                                for (int j = 0; j < dt5.Rows.Count; j++)
                                {
                                    dr1 = dtm.NewRow();
                                    dr1["Stage"] = dt5.Rows[j]["name"].ToString().Trim();
                                    dr1["Type_in_System"] = dt5.Rows[j]["col21"].ToString().Trim();
                                    dr1["Input_in_KG"] = fgen.make_double(dt5.Rows[j]["inp_qty"].ToString().Trim());
                                    dr1["Output_in_KG"] = fgen.make_double(dt5.Rows[j]["op_qty"].ToString().Trim());
                                    db = fgen.make_double(dr1["Output_in_KG"].ToString().Trim());
                                    dr1["Difference"] = Math.Round(fgen.make_double(dr1["Input_in_KG"].ToString().Trim()) - db, 2);
                                    dr1["Wastage_Percentage"] = Math.Round(fgen.make_double(dr1["Difference"].ToString().Trim()) / fgen.make_double(dr1["Input_in_KG"].ToString().Trim()) * 100, 4);
                                    dtm.Rows.Add(dr1);
                                }
                                if (dt5.Rows.Count == 0)
                                {
                                    dr1 = dtm.NewRow();
                                    dr1["Stage"] = "NO STAGE FOR THIS JOBCARD";
                                    dtm.Rows.Add(dr1);
                                    dr1 = dtm.NewRow();
                                    dtm.Rows.Add(dr1);
                                }
                                #endregion
                            }
                            if (dt5.Rows.Count > 0)
                            {
                                dr1 = dtm.NewRow();
                                dr1["Stage"] = "TOTAL WASTAGE FOR THE JOB";
                                dr1["Output_in_KG"] = Math.Round(db1 - db, 2);
                                dtm.Rows.Add(dr1);
                                dr1 = dtm.NewRow();
                                dtm.Rows.Add(dr1);
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F50323":
                    #region
                    dt = new DataTable();
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq1 = ""; mq2 = "";
                    if (party_cd.Length > 1)
                    {
                        mq1 = "a.acode in (" + party_cd + ")";
                    }
                    else
                    {
                        mq1 = "a.acode like '16%'";
                    }
                    if (part_cd.Length > 1)
                    {
                        mq2 = "a.mg in (" + part_cd + ")";
                    }
                    else
                    {
                        mq2 = "a.mg like '9%'";
                    }
                    mq0 = ""; mq3 = "";
                    mq3 = frm_cDt2.Substring(6, 4);
                    mq0 = "select b.name as product,trim(a.acode) as Party_code,a.mg,trim(f.aname) as party_name,f.addr2 as city,f.staten,f.zoname,sum(a.apr) as april,sum(a.may) as may,sum(a.jun) as june,sum(a.jul) as july,sum(a.aug) as august,sum(a.sept) as september,sum(a.oct) as october,sum(a.nov) as november,sum(a.dec) as december,sum(a.jan) as january,sum(a.feb) as febrauary,sum(a.mar) as march from (select a.acode,substr(trim(a.icode),1,2) as mg,(Case when to_char(a.vchdate,'mm/yyyy')='04/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as APR,(Case when to_char(a.vchdate,'mm/yyyy')='05/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as may,(Case when to_char(a.vchdate,'mm/yyyy')='06/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as jun,(Case when to_char(a.vchdate,'mm/yyyy')='07/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as jul,(Case when to_char(a.vchdate,'mm/yyyy')='08/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as aug,(Case when to_char(a.vchdate,'mm/yyyy')='09/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as sept,(Case when to_char(a.vchdate,'mm/yyyy')='10/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as oct,(Case when to_char(a.vchdate,'mm/yyyy')='11/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as nov,(Case when to_char(a.vchdate,'mm/yyyy')='12/" + year + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as dec,(Case when to_char(a.vchdate,'mm/yyyy')='01/" + mq3 + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as jan,(Case when to_char(a.vchdate,'mm/yyyy')='02/" + mq3 + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as feb,(Case when to_char(a.vchdate,'mm/yyyy')='03/" + mq3 + "' then NVL(A.IAMOUNT,0)+NVL(A.CESS_PU,0)+NVL(A.EXC_AMT,0) else 0 end) as mar from ivoucher a where  A.VCHDATE " + xprdrange + " AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '4%' AND A.TYPE!='47' ) a,famst f,type b where trim(a.acode)=trim(f.acode) and  a.mg=trim(B.type1) and b.id='Y' and " + mq1 + " and " + mq2 + " GROUP BY b.name,trim(a.acode),trim(f.aname),f.staten,f.zoname,a.mg,f.addr2 order by party_name";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);//ivouhcer dt
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Party Wise,Month Wise Gross Sale Report for the Period " + frm_cDt1 + " To " + frm_cDt2 + "", frm_qstr);
                    }
                    #endregion
                    break;

                case "F50324":
                    #region
                    header_n = "Order Vs Execution Report";
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable();
                    dt.Columns.Add("SO_NO", typeof(string));
                    dt.Columns.Add("Party_Code", typeof(string));
                    dt.Columns.Add("Party_Name", typeof(string));
                    dt.Columns.Add("City", typeof(string));
                    dt.Columns.Add("State", typeof(string));
                    dt.Columns.Add("Zone", typeof(string));
                    dt.Columns.Add("Order_No", typeof(string));
                    dt.Columns.Add("Punch_Date", typeof(string));
                    dt.Columns.Add("Ord_Date", typeof(string));
                    dt.Columns.Add("Ord_Type", typeof(string));
                    dt.Columns.Add("Category", typeof(string));
                    dt.Columns.Add("Ord_Items", typeof(string));
                    dt.Columns.Add("Ord_Qty", typeof(double));
                    dt.Columns.Add("Ord_Value", typeof(double));
                    //below info from ivoucher
                    dt.Columns.Add("Inv_Items", typeof(double)); //so ke kitne item ka inv bana
                    dt.Columns.Add("Inv_Qty", typeof(double));
                    dt.Columns.Add("Inv_Value", typeof(double));
                    dt.Columns.Add("Inv_No", typeof(string));
                    dt.Columns.Add("No_of_Inv", typeof(double));
                    dt.Columns.Add("Discount", typeof(double));

                    SQuery = ""; mq0 = ""; mq1 = ""; mq2 = "";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq1 = ""; mq2 = "";
                    if (party_cd.Length > 1)
                    {
                        mq1 = "a.acode in (" + party_cd + ")";
                        mq3 = "a.rcode in (" + party_cd + ")";
                    }
                    else
                    {
                        mq1 = "a.acode like '16%'";
                        mq3 = "a.rcode like '16%'";
                    }
                    if (part_cd.Length > 1)
                    {
                        mq2 = "substr(trim(a.icode),1,2) in (" + part_cd + ")";
                    }
                    else
                    {
                        mq2 = "substr(trim(a.icode),1,2) like '9%'";
                    }

                    SQuery = "select trim(a.branchcd)||trim(a.acode)||substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as catg,t.name as mg,a.work_ordno,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode)  as acode,b.aname,b.staten,b.zoname as zone,b.addr2 as city,to_char(a.ent_Dt,'dd/mm/yyyy') as punch_dt,count(a.icode) as ord_items,sum(nvl(a.qtyord,0)) as ord_qty,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as ord_val from somas a,famst b,TYPE T where trim(a.acode)=trim(b.acode) and substr(trim(a.icode),1,2)=trim(t.type1) and a.branchcd='" + mbr + "' and a.type like '4%' and a.orddt " + xprdrange + " and t.id='Y' and " + mq1 + " and " + mq2 + " group by a.ordno,to_char(a.orddt,'dd/mm/yyyy') ,trim(a.acode) ,b.aname,a.work_ordno,to_char(a.ent_dt,'dd/mm/yyyy'),a.branchcd,a.type,b.staten,b.zoname,b.addr2,substr(trim(a.icode),1,2),t.name order by fstr";
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);//sale order dt.....main dt for loop

                    mq0 = "select trim(a.branchcd)||trim(a.rcode)||substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as catg,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ponum,a.podate,count(a.icode) as inv_items,sum(a.iqtyout) as inv_qty,sum(nvl(a.iqtyout,0) * nvl(a.irate,0)) as inv_val from ivoucher a where  a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " and " + mq3 + " and " + mq2 + " group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.ponum,a.podate,a.branchcd,a.type,a.rcode,substr(trim(a.icode),1,2) order by fstr";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq0);//invoice dt
                    i0 = 1;
                    if (dt1.Rows.Count > 0)
                    {
                        dv = new DataView(dt1);
                        dticode = new DataTable();
                        dticode = dv.ToTable(true, "fstr");
                        foreach (DataRow dr3 in dticode.Rows)
                        {
                            dv1 = new DataView(dt1, "fstr='" + dr3["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode2 = new DataTable();
                            dticode2 = dv1.ToTable();//main dt...somas dt
                            //=========================================
                            dt3 = new DataTable();
                            if (dt2.Rows.Count > 0)
                            {
                                dv = new DataView(dt2, "fstr='" + dr3["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt3 = dv.ToTable();
                            }
                            dr1 = dt.NewRow();
                            mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; db3 = 0; db4 = 0; db5 = 0; db6 = 0;
                            mq1 = ""; mq2 = ""; db = 0; db1 = 0; db2 = 0;
                            for (int j = 0; j < dticode2.Rows.Count; j++)
                            {
                                #region
                                dr1["so_no"] = i0;
                                mq3 += "," + dticode2.Rows[j]["ordno"].ToString().Trim();
                                mq4 += "," + dticode2.Rows[j]["punch_dt"].ToString().Trim();
                                mq5 += "," + dticode2.Rows[j]["orddt"].ToString().Trim();
                                db3 += fgen.make_double(dticode2.Rows[j]["ord_items"].ToString().Trim());
                                db4 += fgen.make_double(dticode2.Rows[j]["ord_qty"].ToString().Trim());
                                db5 += fgen.make_double(dticode2.Rows[j]["ord_val"].ToString().Trim());
                                ////for invoice details against SO                             
                                if (j == 0)
                                {
                                    for (int i = 0; i < dt3.Rows.Count; i++)
                                    {
                                        db += fgen.make_double(dt3.Rows[i]["inv_items"].ToString().Trim());
                                        db1 += fgen.make_double(dt3.Rows[i]["inv_qty"].ToString().Trim());
                                        db2 += fgen.make_double(dt3.Rows[i]["inv_val"].ToString().Trim());
                                        mq1 += "," + dt3.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["No_of_Inv"] = dt3.Rows[i]["inv_items"].ToString().Trim();
                                    }
                                }
                            }
                            dr1["Party_Code"] = dticode2.Rows[0]["acode"].ToString().Trim();
                            dr1["Party_Name"] = dticode2.Rows[0]["aname"].ToString().Trim();
                            dr1["City"] = dticode2.Rows[0]["city"].ToString().Trim();
                            dr1["State"] = dticode2.Rows[0]["staten"].ToString().Trim();
                            dr1["Zone"] = dticode2.Rows[0]["zone"].ToString().Trim();
                            dr1["Order_No"] = mq3.TrimStart(',');
                            dr1["Punch_Date"] = mq4.TrimStart(',');
                            dr1["Ord_Date"] = mq5.TrimStart(',');
                            dr1["Ord_Type"] = dticode2.Rows[0]["work_ordno"].ToString().Trim();
                            dr1["Category"] = dticode2.Rows[0]["mg"].ToString().Trim();
                            dr1["Ord_Items"] = db3;
                            dr1["Ord_Qty"] = db4;
                            dr1["Ord_Value"] = db5;
                            ////=================================
                            dr1["Inv_Items"] = db;
                            dr1["Inv_Qty"] = db1;
                            dr1["Inv_Value"] = db2;
                            dr1["Inv_No"] = mq1.TrimStart(',');
                            dr1["No_of_Inv"] = dt3.Rows.Count;
                            dt.Rows.Add(dr1);
                            i0++;
                                #endregion
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F50324_old":
                    #region
                    header_n = "Party wise,Item gp wise Ord vs Sale Report";
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable();
                    dt.Columns.Add("SO_NO", typeof(string));
                    dt.Columns.Add("Party_Code", typeof(string));
                    dt.Columns.Add("Party_Name", typeof(string));
                    dt.Columns.Add("City", typeof(string));
                    dt.Columns.Add("State", typeof(string));
                    dt.Columns.Add("Zone", typeof(string));
                    dt.Columns.Add("Order_No", typeof(string));
                    dt.Columns.Add("Punch_Date", typeof(string));
                    dt.Columns.Add("Ord_Date", typeof(string));
                    dt.Columns.Add("Ord_Type", typeof(string));
                    dt.Columns.Add("Category", typeof(string));
                    dt.Columns.Add("Ord_Items", typeof(string));
                    dt.Columns.Add("Ord_Qty", typeof(double));
                    dt.Columns.Add("Ord_Value", typeof(double));
                    //below info from ivoucher
                    dt.Columns.Add("Inv_Items", typeof(double));
                    dt.Columns.Add("Inv_Qty", typeof(double));
                    dt.Columns.Add("Inv_Value", typeof(double));
                    dt.Columns.Add("Inv_No", typeof(string));
                    dt.Columns.Add("No_of_Inv", typeof(double));
                    dt.Columns.Add("Discount", typeof(double));//this is pending...

                    SQuery = ""; mq0 = ""; mq1 = ""; mq2 = "";

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq1 = ""; mq2 = "";
                    if (party_cd.Length > 1)
                    {
                        mq1 = "a.acode in (" + party_cd + ")";
                        mq3 = "a.rcode in (" + party_cd + ")";
                    }
                    else
                    {
                        mq1 = "a.acode like '16%'";
                        mq3 = "a.rcode like '16%'";
                    }
                    if (part_cd.Length > 1)
                    {
                        mq2 = "substr(trim(a.icode),1,2) in (" + part_cd + ")";
                    }
                    else
                    {
                        mq2 = "substr(trim(a.icode),1,2) like '9%'";
                    }
                    SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.acode)||substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as catg,t.name as mg,a.work_ordno,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode)  as acode,b.aname,b.staten,b.zoname as zone,b.addr2 as city,to_char(a.ent_Dt,'dd/mm/yyyy') as punch_dt,count(a.icode) as ord_items,sum(nvl(a.qtyord,0)) as ord_qty,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as ord_val from somas a,famst b,TYPE T where trim(a.acode)=trim(b.acode) and substr(trim(a.icode),1,2)=trim(t.type1) and a.branchcd='" + mbr + "' and a.type like '4%' and a.orddt " + xprdrange + " and t.id='Y' and " + mq1 + " and " + mq2 + " group by a.ordno,to_char(a.orddt,'dd/mm/yyyy') ,trim(a.acode) ,b.aname,a.work_ordno,to_char(a.ent_dt,'dd/mm/yyyy'),a.branchcd,a.type,b.staten,b.zoname,b.addr2,substr(trim(a.icode),1,2),t.name order by fstr";
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);//sale order dt.....main dt for loop

                    mq0 = "select  trim(a.branchcd)||trim(a.type)||trim(a.ponum)||to_char(a.podate,'dd/mm/yyyy')||trim(a.rcode)||substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as catg,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ponum,a.podate,count(a.icode) as inv_items,sum(a.iqtyout) as inv_qty,sum(nvl(a.iqtyout,0) * nvl(a.irate,0)) as inv_val from ivoucher a where  a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " and " + mq3 + " and " + mq2 + " group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.ponum,a.podate,a.branchcd,a.type,a.rcode,substr(trim(a.icode),1,2) order by fstr";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq0);//invoice dt

                    if (dt1.Rows.Count > 0)
                    {
                        dv = new DataView(dt1);
                        dticode = new DataTable();
                        dticode = dv.ToTable(true, "fstr");
                        foreach (DataRow dr3 in dticode.Rows)
                        {
                            dv1 = new DataView(dt1, "fstr='" + dr3["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode2 = new DataTable();
                            dticode2 = dv1.ToTable();//main dt...somas dt
                            //=========================================
                            dt3 = new DataTable();
                            if (dt2.Rows.Count > 0)
                            {
                                dv = new DataView(dt2, "fstr='" + dr3["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt3 = dv.ToTable();
                            }
                            if (dticode2.Rows.Count > 0)
                            {
                                dr1 = dt.NewRow();
                                dr1["SO_NO"] = dticode2.Rows[0]["ordno"].ToString().Trim();
                                dr1["Party_Code"] = dticode2.Rows[0]["acode"].ToString().Trim();
                                dr1["Party_Name"] = dticode2.Rows[0]["aname"].ToString().Trim();
                                dr1["City"] = dticode2.Rows[0]["city"].ToString().Trim();
                                dr1["State"] = dticode2.Rows[0]["staten"].ToString().Trim();
                                dr1["Zone"] = dticode2.Rows[0]["zone"].ToString().Trim();
                                dr1["Order_No"] = dticode2.Rows[0]["ordno"].ToString().Trim();
                                dr1["Punch_Date"] = dticode2.Rows[0]["punch_dt"].ToString().Trim();
                                dr1["Ord_Date"] = dticode2.Rows[0]["orddt"].ToString().Trim();
                                dr1["Ord_Type"] = dticode2.Rows[0]["work_ordno"].ToString().Trim();
                                dr1["Category"] = dticode2.Rows[0]["mg"].ToString().Trim();
                                dr1["Ord_Items"] = dticode2.Rows[0]["ord_items"].ToString().Trim();
                                dr1["Ord_Qty"] = fgen.make_double(dticode2.Rows[0]["ord_qty"].ToString().Trim());
                                dr1["Ord_Value"] = fgen.make_double(dticode2.Rows[0]["ord_val"].ToString().Trim());
                                ////for invoice details against SO
                                mq1 = ""; mq2 = ""; db = 0; db1 = 0; db2 = 0; db3 = 0;
                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    db += fgen.make_double(dt3.Rows[i]["inv_items"].ToString().Trim());
                                    db1 += fgen.make_double(dt3.Rows[i]["inv_qty"].ToString().Trim());
                                    db2 += fgen.make_double(dt3.Rows[i]["inv_val"].ToString().Trim());
                                    mq1 += "," + dt3.Rows[i]["vchnum"].ToString().Trim();
                                    dr1["No_of_Inv"] = dt3.Rows[i]["inv_items"].ToString().Trim();
                                }
                                dr1["Inv_Items"] = db;
                                dr1["Inv_Qty"] = db1;
                                dr1["Inv_Value"] = db2;
                                dr1["Inv_No"] = mq1.TrimStart(',');
                                dr1["No_of_Inv"] = dt3.Rows.Count;
                                dt.Rows.Add(dr1);
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F50322":
                    #region
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                    dt.Columns.Add("PRODUCT", typeof(string));
                    dt.Columns.Add("SO_QTY", typeof(double));
                    dt.Columns.Add("SO_VALUE", typeof(double));
                    dt.Columns.Add("SALE_QTY", typeof(double));
                    dt.Columns.Add("SALE_VALUE", typeof(double));
                    header_n = "MIS Report";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");    //maingroup              
                    mq1 = ""; mq2 = "";
                    if (party_cd.Length > 1)
                    {
                        mq2 = "and substr(trim(a.icode),1,2) in (" + party_cd + ")";
                    }
                    else
                    {
                        mq2 = "and substr(trim(a.icode),1,2) like '9%'";
                    }
                    SQuery = "select substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as catg,t.name as mg,sum(nvl(a.qtyord,0)) as ord_qty,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as ord_val from somas a,TYPE T where substr(trim(a.icode),1,2)=trim(t.type1) and a.branchcd='" + mbr + "' and a.type like '4%' and a.orddt " + xprdrange + " and t.id='Y' " + mq2 + " group by a.branchcd,a.type,substr(trim(a.icode),1,2),t.name order by fstr";
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);//sale order dt.....main dt for loop

                    mq0 = "select substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as catg,sum(a.iqtyout) as inv_qty,sum(nvl(a.iqtyout,0) * nvl(a.irate,0)) as inv_val from ivoucher a where  a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " " + mq2 + " group by a.branchcd,a.type,substr(trim(a.icode),1,2) order by fstr";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq0);//invoice dt...showing basic value

                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dr1 = dt.NewRow();
                            mq0 = "";
                            mq0 = dt1.Rows[i]["catg"].ToString().Trim();
                            dr1["PRODUCT"] = dt1.Rows[i]["mg"].ToString().Trim();
                            dr1["SO_QTY"] = fgen.make_double(dt1.Rows[i]["ord_qty"].ToString().Trim());
                            dr1["SO_VALUE"] = fgen.make_double(dt1.Rows[i]["ord_val"].ToString().Trim());
                            dr1["SALE_QTY"] = fgen.make_double(fgen.seek_iname_dt(dt2, "catg='" + mq0 + "'", "inv_qty"));
                            dr1["SALE_VALUE"] = fgen.make_double(fgen.seek_iname_dt(dt2, "catg='" + mq0 + "'", "inv_val"));
                            dt.Rows.Add(dr1);
                        }
                        oporow = null;
                        oporow = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.Ordinal == 0)
                            {
                                oporow[0] = "Total";
                            }
                            else
                            {
                                double mysum = 0;
                                foreach (DataRow drc in dt.Rows)
                                {
                                    mysum += fgen.make_double(drc[dc].ToString());
                                    oporow[dc] = mysum;
                                }
                            }
                        }
                        dt.Rows.Add(oporow);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F50327":
                    #region
                    header_n = "Product Wise,Party Wise Comparison Report for Sale Order Qty Vs Sale Qty";
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable();
                    dt.Columns.Add("AC_CODE", typeof(string));
                    dt.Columns.Add("AC_Name", typeof(string));
                    dt.Columns.Add("CITY", typeof(string));
                    dt.Columns.Add("STATE", typeof(string));
                    dt.Columns.Add("ZONE", typeof(string));
                    mq0 = ""; mq1 = "";
                    mq0 = "select type1,name from type where id='Y' AND TYPE1 LIKE '9%' order by type1";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        dt.Columns.Add("" + dt1.Rows[i]["name"].ToString().Trim() + "_Order_Amt", typeof(double));
                        dt.Columns.Add("" + dt1.Rows[i]["name"].ToString().Trim() + "_Sale_Amt", typeof(double));
                    }
                    SQuery = "select trim(a.acode)||substr(trim(a.icode),1,2) as fstr,trim(a.acode) as acode,trim(b.aname) as party,b.zoname as zone,b.addr2 as city,b.staten,substr(trim(a.icode),1,2) as catg,sum(nvl(a.qtyord,0)) as ord_qty,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as ord_val from somas a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.orddt " + xprdrange + "  group by a.branchcd,trim(a.acode),a.type,substr(trim(a.icode),1,2),trim(b.aname),b.zoname,b.addr2,b.staten order by fstr";
                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);//sale order dt.....main dt for loop

                    mq1 = "select  trim(a.rcode)||substr(trim(a.icode),1,2) as fstr,trim(a.rcode) as rcode,substr(trim(a.icode),1,2) as catg,sum(a.iqtyout) as inv_qty,sum(nvl(a.iqtyout,0) * nvl(a.irate,0)) as inv_val from ivoucher a where  a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " group by a.branchcd,a.type,substr(trim(a.icode),1,2),trim(a.rcode) order by fstr";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq1);//invoice dt...showing basic value

                    if (dt2.Rows.Count > 0)
                    {
                        dv = new DataView(dt2);
                        dticode = new DataTable();
                        dticode = dv.ToTable(true, "fstr");
                        foreach (DataRow dr3 in dticode.Rows)
                        {
                            dv1 = new DataView(dt2, "fstr='" + dr3["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode2 = new DataTable();
                            dticode2 = dv1.ToTable();//main dt...somas dt
                            if (dt3.Rows.Count > 0)
                            {
                                dv = new DataView(dt3, "fstr='" + dr3["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt4 = dv.ToTable();//inv dt
                            }
                            for (int i = 0; i < dticode2.Rows.Count; i++)
                            {
                                dr1 = dt.NewRow();
                                dr1["AC_CODE"] = dticode2.Rows[i]["acode"].ToString().Trim();
                                dr1["AC_Name"] = dticode2.Rows[i]["party"].ToString().Trim();
                                dr1["CITY"] = dticode2.Rows[i]["city"].ToString().Trim();
                                dr1["STATE"] = dticode2.Rows[i]["staten"].ToString().Trim();
                                dr1["ZONE"] = dticode2.Rows[i]["zone"].ToString().Trim();
                                for (int j = 0; j < dt1.Rows.Count; j++)
                                {
                                    dr1["" + dt1.Rows[j]["name"].ToString().Trim() + "_Order_Amt"] = fgen.make_double(fgen.seek_iname_dt(dticode2, "catg='" + dt1.Rows[j]["type1"].ToString().Trim() + "' ", "ord_val"));
                                    if (dt4.Rows.Count > 0)
                                    {
                                        dr1["" + dt1.Rows[j]["name"].ToString().Trim() + "_Sale_Amt"] = fgen.make_double(fgen.seek_iname_dt(dt4, "catg='" + dt1.Rows[j]["type1"].ToString().Trim() + "' ", "inv_val"));
                                    }
                                    else
                                    {
                                        dr1["" + dt1.Rows[j]["name"].ToString().Trim() + "_Sale_Amt"] = 0;
                                    }
                                }
                                dt.Rows.Add(dr1);
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " for " + mq5 + "", frm_qstr);
                    }
                    #endregion
                    break;

                case "F50154":
                    #region
                    ph_tbl = new DataTable();
                    #region PENDING ORDER REGISTER-OMNI REPORT
                    ph_tbl.Columns.Add("DATED", typeof(string));
                    ph_tbl.Columns.Add("ORDNO", typeof(string));
                    ph_tbl.Columns.Add("CUSTOMER", typeof(string));
                    ph_tbl.Columns.Add("LOCATION", typeof(string));
                    ph_tbl.Columns.Add("CUST_PO_NO", typeof(string));
                    ph_tbl.Columns.Add("PARTNO", typeof(string));
                    ph_tbl.Columns.Add("ITEM_CODE", typeof(string));
                    ph_tbl.Columns.Add("PART_NAME", typeof(string));
                    ph_tbl.Columns.Add("UOM", typeof(string));
                    ph_tbl.Columns.Add("QTY_ORD", typeof(double));
                    ph_tbl.Columns.Add("DISP_QTY", typeof(double));
                    ph_tbl.Columns.Add("PENDING_ORDER", typeof(double));
                    ph_tbl.Columns.Add("PENDING_ORDER_IN_KGS", typeof(double));
                    ph_tbl.Columns.Add("INVNO_INVDATE_QTY", typeof(string));
                    #endregion
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt5 = new DataTable(); dt6 = new DataTable();
                    mq0 = ""; mq1 = ""; mq2 = "";
                    header_n = "Pending Order Register";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    cond1 = ""; cond = "";
                    if (party_cd.Length > 2)
                    {
                        cond = "and trim(a.acode) in ('" + party_cd + "')";
                    }
                    else
                    {
                        cond = "and trim(a.acode) like '16%'";
                    }
                    if (part_cd.Length > 2)
                    {
                        cond1 = "and trim(a.icode) in ('" + part_cd + "')";
                    }
                    else
                    {
                        cond1 = "and trim(a.icode) like '9%'";
                    }
                    mq3 = ""; mq0 = ""; mq1 = "";
                    mq0 = "select trim(a.branchcd)||trim(a.type)||trim(a.ordno)||a.orddt||trim(a.acode)||trim(a.icode) as fstr, a.orddt,a.ordno,a.acode,trim(b.aname) as customer,NVL(B.DISTRICT,'-') AS DISTRICT,a.icode,trim(c.iname) as iname,c.cpartno as partno,c.unit,C.wt_net ,a.qtyord as ordqty,a.soldqty as desp_Qty,a.bal_qty,a.pordno,a.porddt from (select branchcd,TYPE,ordno,orddt,trim(AcodE) as Acode,ERP_code as Icode,max(cu_chldt) as del_date,max(Srate) as Srate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,sum(Qtyord)-sum(Soldqty) as Bal_Qty,fstr,max(amdtno) as amdtno,max(pordno) as pordno,max(porddt) as porddt,invno,invdate,max(desc9) as desc9,max(cpartno) as cpartno,max(Srno) As Srno,max(ent_by) as ent_by,max(ent_dt) As ent_dt,max(app_by) As app_by,max(app_dt) As app_dt from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(cdrgno) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((irate*(100-cdisc)/100))*(case when nvl(CURR_RATE,0)=0 then 1 else nvl(CURR_RATE,0) end )  as srate,acode,branchcd,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,cu_chldt,TYPE,nvl(del_Wk,0) as amdtno,pordno,porddt,null as invno,null as invdate,(CASE WHEN NVL(desc9,'-')!='-' THEN  NVL(desc9,'-') ELSE NVL(CINAME,'-') END) AS DESC9,cpartno,srno,ent_by,ent_dt,app_by,app_dt from somas where branchcd='" + mbr + "' and type like '4%' and trim(icat)!='Y' and (trim(check_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and orddt " + xprdrange + "  union all SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(revis_no) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode,branchcd,ponum,to_char(podate,'dd/mm/yyyy') as podate,null as del_date, TYPE,null as amdtno,null as pordno,null as porddt,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as desc9,null as cpartno,null as srno,null as ent_by,null as ent_dt,null as app_by,null as app_dt  from ivoucher where branchcd='" + mbr + "' and type like '4%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate " + xprdrange + " ) group by fstr,branchcd,ordno,orddt,TYPE,trim(AcodE),ERP_code,invno,invdate having sum(Qtyord)-sum(Soldqty)>0 ) a ,famst b ,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) " + cond + " " + cond1 + "  order by orddt,acode";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);//main dt 

                    mq1 = "select trim(a.branchcd)||trim(a.type)||trim(a.ponum)||to_char(a.podate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,nvl(d.exc_rng,'-') as exc_rng, a.branchcd,a.type,nvl(b.cscode,'-') as cscode,A.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdt ,trim(a.acode) as acode,trim(a.icode) as icode,sum(a.iqtyout) as sale_qty,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate,a.prnum from ivoucher a,sale b left outer join csmst d on trim(b.cscode)=trim(d.acode) where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + mbr + "' and a.type like '4%'  and trim(a.acode) like '16%' and trim(a.icode) like '9%' group by  a.branchcd,a.type,a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.icode) ,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy'),a.prnum ,nvl(b.cscode,'-'),nvl(d.exc_rng,'-')  order by invno,invdt asc";//and a.vchdate  between to_date('01/04/2019','dd/mm/yyyy') and to_date('30/04/2019','dd/mm/yyyy')
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //invdate

                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "fstr");
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            dt3 = new DataTable(); dt4 = new DataTable();
                            DataView viewim = new DataView(dt, "FSTR='" + dr0["FSTR"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt3 = viewim.ToTable();//somas view
                            dr1 = ph_tbl.NewRow();
                            if (dt1.Rows.Count > 0)
                            {
                                DataView viewim1 = new DataView(dt1, "fstr='" + dr0["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt4 = viewim1.ToTable();
                            }
                            db6 = 0;//for bal order qty
                            db1 = 0;
                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                #region order details
                                mq3 = ""; mq4 = "";
                                dr1 = ph_tbl.NewRow();
                                dr1["DATED"] = dt3.Rows[i]["orddt"].ToString().Trim();
                                dr1["ORDNO"] = dt3.Rows[i]["ORDNO"].ToString().Trim();
                                dr1["CUSTOMER"] = dt3.Rows[i]["customer"].ToString().Trim();
                                dr1["CUST_PO_NO"] = dt3.Rows[i]["pordno"].ToString().Trim();
                                dr1["PARTNO"] = dt3.Rows[i]["partno"].ToString().Trim();
                                dr1["ITEM_CODE"] = dt3.Rows[i]["icode"].ToString().Trim();
                                dr1["PART_NAME"] = dt3.Rows[i]["iname"].ToString().Trim();
                                dr1["UOM"] = dt3.Rows[i]["unit"].ToString().Trim();
                                dr1["QTY_ORD"] = dt3.Rows[i]["ordqty"].ToString().Trim();
                                //=====================================    
                                if (i == 0) //same order per loop ho to inv ki ek bar loop chale
                                {
                                    for (int j = 0; j < dt4.Rows.Count; j++)
                                    {  //////// filling invoice details on basis of order                                         
                                        if (dt4.Rows[j]["cscode"].ToString().Trim() == "-")
                                        {
                                            dr1["LOCATION"] = dt4.Rows[j]["exc_rng"].ToString().Trim();
                                        }
                                        else
                                        {
                                            dr1["LOCATION"] = dt3.Rows[i]["DISTRICT"].ToString().Trim();
                                        }
                                        mq3 += "," + dt4.Rows[j]["invno"].ToString().Trim() + "," + dt4.Rows[j]["invdt"].ToString().Trim() + "," + dt4.Rows[j]["sale_qty"].ToString().Trim() + " / ";
                                        dr1["INVNO_INVDATE_QTY"] = mq3.TrimStart(',').TrimEnd('/');
                                        db1 += fgen.make_double(dt4.Rows[j]["sale_qty"].ToString().Trim());
                                        dr1["DISP_QTY"] = db1;
                                    }
                                }
                                dr1["PENDING_ORDER"] = fgen.make_double(dr1["QTY_ORD"].ToString().Trim()) - db1;
                                if (fgen.make_double(dr1["PENDING_ORDER"].ToString().Trim()) > 0)
                                {
                                    if (dr1["UOM"].ToString().Trim() == "KG" || dr1["UOM"].ToString().Trim() == "KGS")
                                    {
                                        dr1["PENDING_ORDER_IN_KGS"] = dr1["PENDING_ORDER"].ToString().Trim();
                                    }
                                    else
                                    {
                                        dr1["PENDING_ORDER_IN_KGS"] = fgen.make_double(dr1["PENDING_ORDER"].ToString().Trim()) * fgen.make_double(dt3.Rows[i]["wt_net"].ToString().Trim());
                                    }
                                }
                                if (hfval.Value == "Y")
                                {
                                    ph_tbl.Rows.Add(dr1);
                                }
                                else
                                {
                                    if (fgen.make_double(dr1["PENDING_ORDER"].ToString().Trim()) > 0)
                                    {
                                        ph_tbl.Rows.Add(dr1);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Pending Order Report from " + fromdt + " to " + todt + "", frm_qstr);
                    }
                    #endregion
                    break;

                case "F50129":
                    //SQuery = "select TRIM(a.acode) as party_code,b.aname as party_name,TRIM(a.icode) as item_code,c.iname as item_name,c.cpartno as partNo,c.unit,sum(a.sch_qty) as sch_qty,sum(a.desp_qty) as desp_qty from (select trim(acode) as acode,trim(icode) as icode,total as sch_qty,0 as desp_qty from schedule where branchcd='" + mbr + "' and type='46' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' union all select trim(acode) as acode,trim(icode) as icode,0 as sch_qty,iqtyout as desp_qty from ivoucher where branchcd='" + mbr + "' and type like '4%' and TO_CHAR(VCHDATE,'MM/YYYY')='" + hf1.Value + "' /*and nvl(iqtyout,0)>0*/) a ,famst b,item c where trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit order by party_code,item_code";

                    SQuery = "select a.branchcd as branch_code,a.vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,b.aname as Customer,b.name as TSI_NAme,b.acref ASM_NAme,b.acref2 as RSM_NAme,f.Name,a.tpt_Names as Sales_rep,a.branchcd as Unit_Code,a.acode as Cust_Code,b.addr1,b.addr2,b.addr3,c.cscode,c.csname,c.addr1 as cs_addr1,c.addr2 as cs_addr2,c.addr3 as cs_addr3,c.Staten as CS_State,c.cgst_no," +
               " d.iName as sg_name,a.Icode,a.exc_57f4 as Part_no,a.purpose as Item_Name,e.hscode,a.iqtyout as Qty_sold,e.unit,a.irate,a.ichgs as Disc_Perc," +
               " a.iamount as Basic_Val,(Case when a.iopr='IG' then a.exc_amt else 0 end) As IGST_AMT,(Case when a.iopr='CG' then a.exc_amt else 0 end) As CGST_AMT,(Case when a.iopr='CG' then a.cess_pu else 0 end) As SGST_AMT ,0 as utgst,a.tpt_names as ts_name," +
               " (Case when a.iopr='IG' then a.exc_rate else 0 end) As IGST_rate,(Case when a.iopr='CG' then a.exc_rate else 0 end) As CGST_rate,(Case when a.iopr='CG' then a.cess_percent else 0 end) As SGST_rate,c.grno as GR_No,c.grdate,c.vehi_fitno as dlv_status,c.ins_no as Tpt_Name,c.reach_by,c.reach_dt,c.cmrr_no,c.cmrr_dt,c.cmrr_QT,a.finvno as PO_no,a.ponum,a.podate,b.del_Cod as Dist_SDist,b.del_note as B2B_B2C,b.del_wayb as CNature,b.DLVBANK as Adhar_no,b.zoname,e.salloy as Item_Catg from " +
               " ivoucher a, (select a.acode,a.aname,a.addr1,a.addr2,a.addr3,a.del_Cod,a.del_note,a.del_wayb,a.DLVBANK,a.zoname,b.name,b.acref,b.acref2 from famst a left outer join (select type1,name,acref,acref2 from typegrp where branchcd!='DD' and id='EM') b on substr(trim(a.mktggrp),1,3)=TRIM(TYPE1)  )  b,(select m.branchcd||m.type||m.vchnum||to_Char(m.vchdate,'dd/mm/yyyy') As fstr,m.ins_no,m.vehi_fitno,m.grno,m.grdate,m.cscode,nvl(n.aname,'-') as csname,nvl(n.gst_no,'-') as cgst_no,n.Staten,n.addr1,n.addr2,n.addr3,m.reach_by,m.reach_Dt,m.cmrr_no,m.cmrr_dt,m.cmrr_QT from sale m " +
               " left outer join csmst n on trim(m.cscode)=trim(n.acode) where m.vchdate " + xprdrange + " ) c ,(select icode,iname from item where length(Trim(icode))=4) d,item e,type f   " +
               " where a.type like '4%' and f.id='V' and a.type=f.type1 and a." + branch_Cd + " and trim(A.acode)=trim(B.acode) and substr(a.icode,1,4)=trim(d.icode) and trim(a.icode)=trim(e.icode) and a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=c.fstr order by a.vchdate,a.vchnum";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                    fgen.Fn_open_rptlevel("Sales Date / DMP for the period (" + fromdt + " to " + todt + ")", frm_qstr);
                    break;

                case "F50330"://new CSV REPROT FOR VELVIN....23/05/2020
                    #region
                    ph_tbl = new DataTable();
                    #region headings
                    ph_tbl.Columns.Add("INV_NO", typeof(string)); //1.....p
                    ph_tbl.Columns.Add("INV_DT", typeof(string)); //2.............p
                    ph_tbl.Columns.Add("Tax_Point_Dt", typeof(string));//3 
                    ph_tbl.Columns.Add("INV_type", typeof(string)); //4............p
                    ph_tbl.Columns.Add("buyerid", typeof(string)); //5 ...pic buycode of party
                    ph_tbl.Columns.Add("Org_Inv_no", typeof(string)); //6
                    ph_tbl.Columns.Add("Org_Inv_dt", typeof(string)); //7
                    ph_tbl.Columns.Add("Org_delv_dt", typeof(string)); //8
                    ph_tbl.Columns.Add("Credit_Reson", typeof(string)); //9
                    ph_tbl.Columns.Add("po_number", typeof(string)); //10............p
                    ph_tbl.Columns.Add("Delv_note_number", typeof(string)); //11
                    ph_tbl.Columns.Add("Payment_Ref", typeof(string)); //12
                    ph_tbl.Columns.Add("Payment_Method", typeof(string)); //13
                    ph_tbl.Columns.Add("Form_of_Pymt", typeof(string)); //14
                    ph_tbl.Columns.Add("Pymt_terms", typeof(string)); //15............p
                    ph_tbl.Columns.Add("Net_Pymt_Days", typeof(string)); //16
                    ph_tbl.Columns.Add("Start_date", typeof(string)); //17
                    ph_tbl.Columns.Add("End_Date", typeof(string)); //18
                    ph_tbl.Columns.Add("Order_Date", typeof(string)); //19
                    ph_tbl.Columns.Add("Delv_Dt", typeof(string)); //20
                    ph_tbl.Columns.Add("Ship_Date", typeof(string)); //21............p
                    ph_tbl.Columns.Add("Declr_dt", typeof(string)); //22
                    ph_tbl.Columns.Add("Payment_due_by_dt", typeof(string)); //23
                    ph_tbl.Columns.Add("Early_pymt_dt", typeof(string)); //24
                    ph_tbl.Columns.Add("Mfg_dt", typeof(string)); //25
                    ph_tbl.Columns.Add("Expiry_Dt", typeof(string)); //26
                    ph_tbl.Columns.Add("Inv_From_Name", typeof(string)); //27
                    ph_tbl.Columns.Add("Inv_From_add1", typeof(string)); //28
                    ph_tbl.Columns.Add("Inv_From_add2", typeof(string)); //29
                    ph_tbl.Columns.Add("Inv_From_city", typeof(string)); //30
                    ph_tbl.Columns.Add("Inv_From_PostalCode", typeof(string)); //31
                    ph_tbl.Columns.Add("Inv_From_state", typeof(string)); //32
                    ph_tbl.Columns.Add("Inv_From_country", typeof(string)); //33
                    ph_tbl.Columns.Add("Supp_Qst_Tax_Reg_Num", typeof(string)); //34
                    ph_tbl.Columns.Add("Supp_Pst_Tax_Reg_Num", typeof(string)); //35
                    ph_tbl.Columns.Add("Supp_Gst_Tax_Reg_Num", typeof(string)); //36
                    ph_tbl.Columns.Add("Supp_Hst_Tax_Reg_Num", typeof(string)); //37
                    ph_tbl.Columns.Add("Main_Supp_Cont_Name", typeof(string)); //38
                    ph_tbl.Columns.Add("Main_Supp_Cont_Tel", typeof(string)); //39
                    ph_tbl.Columns.Add("Main_Supp_Cont_email", typeof(string)); //40
                    ph_tbl.Columns.Add("Cont_Num_for_INVQ", typeof(string)); //41
                    ph_tbl.Columns.Add("Cont_tel_for_INVQ", typeof(string)); //42
                    ph_tbl.Columns.Add("Cont_email_for_INVQ", typeof(string)); //43
                    ph_tbl.Columns.Add("Inv_to_name", typeof(string)); //44
                    ph_tbl.Columns.Add("Inv_to_add1", typeof(string)); //45
                    ph_tbl.Columns.Add("Inv_to_add2", typeof(string)); //46
                    ph_tbl.Columns.Add("Inv_to_city", typeof(string)); //47
                    ph_tbl.Columns.Add("Inv_to_Postalcode", typeof(string)); //48
                    ph_tbl.Columns.Add("Inv_to_state", typeof(string)); //49
                    ph_tbl.Columns.Add("Inv_to_country", typeof(string)); //50
                    ph_tbl.Columns.Add("Buyer_Cont_Name", typeof(string)); //51
                    ph_tbl.Columns.Add("Buyer_Cont_tel", typeof(string)); //52
                    ph_tbl.Columns.Add("Buyer_Cont_email", typeof(string)); //53
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_name", typeof(string)); //54
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_add1", typeof(string)); //55
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_add2", typeof(string)); //56
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_city", typeof(string)); //57
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_PostalCode", typeof(string)); //58
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_state", typeof(string)); //59
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_country", typeof(string)); //60
                    ph_tbl.Columns.Add("Supp_Fiscal_rep_TaxReg_Name", typeof(string)); //61
                    ph_tbl.Columns.Add("Ship_to_Name", typeof(string)); //62............p
                    ph_tbl.Columns.Add("Ship_to_Add1", typeof(string)); //63............p
                    ph_tbl.Columns.Add("Ship_to_Add2", typeof(string)); //64............p
                    ph_tbl.Columns.Add("Ship_to_City", typeof(string)); //65............p
                    ph_tbl.Columns.Add("Ship_to_PostalCode", typeof(string)); //66............p
                    ph_tbl.Columns.Add("Ship_to_state", typeof(string)); //67............p
                    ph_tbl.Columns.Add("Ship_to_Country", typeof(string)); //68............p
                    ph_tbl.Columns.Add("Ship_to_Tax_Reg_Num", typeof(string)); //69............p
                    ph_tbl.Columns.Add("Ship_from_Name", typeof(string)); //70............p
                    ph_tbl.Columns.Add("Ship_from_Add1", typeof(string)); //71............p
                    ph_tbl.Columns.Add("Ship_from_Add2", typeof(string)); //72............p
                    ph_tbl.Columns.Add("Ship_from_City", typeof(string)); //73............p
                    ph_tbl.Columns.Add("Ship_from_PostalCode", typeof(string)); //74............p
                    ph_tbl.Columns.Add("Ship_from_state", typeof(string)); //75............p
                    ph_tbl.Columns.Add("Ship_from_Country", typeof(string)); //76............p
                    ph_tbl.Columns.Add("Ship_from_Tax_Reg_Num", typeof(string)); //77............p
                    ph_tbl.Columns.Add("Ordered_By_Name", typeof(string)); //78
                    ph_tbl.Columns.Add("Ordered_By_Add1", typeof(string)); //79
                    ph_tbl.Columns.Add("Ordered_By_Add2", typeof(string)); //80
                    ph_tbl.Columns.Add("Ordered_By_City", typeof(string)); //81
                    ph_tbl.Columns.Add("Ordered_By_PostalCode", typeof(string)); //82
                    ph_tbl.Columns.Add("Ordered_By_state", typeof(string)); //83
                    ph_tbl.Columns.Add("Ordered_By_Country", typeof(string)); //84
                    ph_tbl.Columns.Add("Inv_Net_Amt", typeof(string)); //85............p///this is totalamt..no need to show
                    ph_tbl.Columns.Add("Inv_Tax_Amt", typeof(double)); //86............p
                    ph_tbl.Columns.Add("Inv_Gross_Amt", typeof(double)); //87............p
                    ph_tbl.Columns.Add("Currency", typeof(string)); //88............p
                    ph_tbl.Columns.Add("Local_Currency", typeof(string)); //89
                    ph_tbl.Columns.Add("Exchange_Rate", typeof(string)); //90
                    ph_tbl.Columns.Add("Local_Currency_Net_Amt", typeof(string)); //91
                    ph_tbl.Columns.Add("Local_Currency_Tax_Amt", typeof(string)); //92
                    ph_tbl.Columns.Add("Local_Currency_Gross_Amt", typeof(string)); //93
                    ph_tbl.Columns.Add("bank_name", typeof(string)); //94............p
                    ph_tbl.Columns.Add("bank_Address", typeof(string)); //95............p
                    ph_tbl.Columns.Add("sort_code", typeof(string)); //96............p
                    ph_tbl.Columns.Add("bank_Acc_Number", typeof(string)); //97............p
                    ph_tbl.Columns.Add("bank_Acc_name", typeof(string)); //98............p
                    ph_tbl.Columns.Add("IBAN", typeof(string)); //99
                    ph_tbl.Columns.Add("SWIFT", typeof(string)); //100
                    ph_tbl.Columns.Add("PO_Line_Num", typeof(string)); //101............p
                    ph_tbl.Columns.Add("Qty", typeof(double)); //102............p
                    ph_tbl.Columns.Add("UOM", typeof(string)); //103............p
                    ph_tbl.Columns.Add("Unit_Price", typeof(double)); //104............p
                    ph_tbl.Columns.Add("Line_Net_Amt", typeof(double)); //105............p
                    ph_tbl.Columns.Add("Supp_Part_Num", typeof(string)); //106............p
                    ph_tbl.Columns.Add("Supp_Part_Desc", typeof(string)); //107............p
                    ph_tbl.Columns.Add("Buyer_Part_Num", typeof(string)); //108............p
                    ph_tbl.Columns.Add("Mfg_Part_Num", typeof(string)); //109
                    ph_tbl.Columns.Add("Commodity_Code", typeof(string)); //110
                    ph_tbl.Columns.Add("TaX_Descriptor", typeof(string)); //111
                    ph_tbl.Columns.Add("Tax_Catg_code", typeof(string)); //112............p
                    ph_tbl.Columns.Add("Tax_Rate1", typeof(double)); //113............p
                    ph_tbl.Columns.Add("Tax_Amt1", typeof(double)); //114...line taxamt 1............p
                    ph_tbl.Columns.Add("Tax_Catg_Code2", typeof(string)); //115............p
                    ph_tbl.Columns.Add("Tax_Rate2", typeof(double)); //116............p
                    ph_tbl.Columns.Add("Tax_Amt2", typeof(double)); //117...line tax amt2............p
                    ph_tbl.Columns.Add("Tax_Catg_Code3", typeof(string)); //118............p
                    ph_tbl.Columns.Add("Tax_Rate3", typeof(double)); //119............p
                    ph_tbl.Columns.Add("Tax_Amt3", typeof(double)); //120..line taxamt3............p
                    ph_tbl.Columns.Add("Tax_catg_code4", typeof(string)); //121
                    ph_tbl.Columns.Add("Tax_Rate4", typeof(string)); //122
                    ph_tbl.Columns.Add("Tax_Amt4", typeof(string)); //123;..linetax amt4
                    ph_tbl.Columns.Add("Shipment_No", typeof(string)); //124
                    ph_tbl.Columns.Add("Buyer_Cost_Center", typeof(string)); //125
                    ph_tbl.Columns.Add("Bill_of_Loading", typeof(string)); //126
                    ph_tbl.Columns.Add("Contract_ID", typeof(string)); //127
                    ph_tbl.Columns.Add("Dun_and_Bradstreet_No", typeof(string)); //128
                    ph_tbl.Columns.Add("INCO_terms", typeof(string)); //129
                    ph_tbl.Columns.Add("WBS", typeof(string)); //130
                    ph_tbl.Columns.Add("Nature_of_Tax", typeof(string)); //131
                    ph_tbl.Columns.Add("Place_of_Issue", typeof(string)); //132
                    ph_tbl.Columns.Add("GL_No", typeof(string)); //133
                    ph_tbl.Columns.Add("Account_No", typeof(string)); //134
                    ph_tbl.Columns.Add("Utility_ID", typeof(string)); //135
                    ph_tbl.Columns.Add("ESR_Cust_Ref", typeof(string)); //136
                    ph_tbl.Columns.Add("ESR_Cust_Num", typeof(string)); //137
                    ph_tbl.Columns.Add("Weight", typeof(string)); //138
                    ph_tbl.Columns.Add("Net_Weight", typeof(string)); //139
                    ph_tbl.Columns.Add("Gross_Weight", typeof(string)); //140
                    ph_tbl.Columns.Add("No_of_pkg", typeof(string)); //141............p
                    ph_tbl.Columns.Add("Mode_of_Tpt", typeof(string)); //142............p
                    ph_tbl.Columns.Add("Exp_time_of_Arrival", typeof(string)); //143
                    ph_tbl.Columns.Add("Port_of_Loading", typeof(string)); //144
                    ph_tbl.Columns.Add("Port_of_Discharge", typeof(string)); //145
                    ph_tbl.Columns.Add("Chg_Catg", typeof(string)); //146
                    ph_tbl.Columns.Add("Withhold_Tax_Indicator", typeof(string)); //147
                    ph_tbl.Columns.Add("License_Number", typeof(string)); //148
                    ph_tbl.Columns.Add("Custom_Declaration_No", typeof(string)); //149
                    ph_tbl.Columns.Add("Custom_Office", typeof(string)); //150
                    ph_tbl.Columns.Add("Country_of_Origin", typeof(string)); //151
                    ph_tbl.Columns.Add("Rail_Truck_No", typeof(string)); //152............p
                    ph_tbl.Columns.Add("Batch_No", typeof(string)); //153
                    ph_tbl.Columns.Add("Batch_Qty", typeof(string)); //154
                    ph_tbl.Columns.Add("Goods_or_Service_Indicator", typeof(string)); //155
                    ph_tbl.Columns.Add("Month", typeof(string)); //156
                    ph_tbl.Columns.Add("Week", typeof(string)); //157
                    ph_tbl.Columns.Add("Hrs", typeof(string)); //158
                    ph_tbl.Columns.Add("Name", typeof(string)); //159
                    ph_tbl.Columns.Add("Location_Code", typeof(string)); //160
                    ph_tbl.Columns.Add("Approver_Code", typeof(string)); //161
                    ph_tbl.Columns.Add("Serail_No", typeof(string)); //162
                    ph_tbl.Columns.Add("Buyer_Affiliate", typeof(string)); //163
                    ph_tbl.Columns.Add("Export_No", typeof(string)); //164
                    ph_tbl.Columns.Add("Ticket_No", typeof(string)); //165
                    ph_tbl.Columns.Add("Tax_Regime", typeof(string)); //166
                    ph_tbl.Columns.Add("Booking_Ref", typeof(string)); //167
                    ph_tbl.Columns.Add("Delv_start_dt", typeof(string)); //168
                    ph_tbl.Columns.Add("Delv_End_date", typeof(string)); //169
                    ph_tbl.Columns.Add("Disc_Description", typeof(string)); //170
                    ph_tbl.Columns.Add("Disc_Amt", typeof(string)); //171
                    ph_tbl.Columns.Add("Disc_Tax_Catg1", typeof(string)); //172
                    ph_tbl.Columns.Add("Disc_Tax_Rate1", typeof(string)); //173
                    ph_tbl.Columns.Add("Disc_Tax_Amt1", typeof(string)); //174
                    ph_tbl.Columns.Add("Disc_Tax_Catg2", typeof(string)); //175
                    ph_tbl.Columns.Add("Disc_Tax_Rate2", typeof(string)); //176
                    ph_tbl.Columns.Add("Disc_Tax_Amt2", typeof(string)); //177
                    ph_tbl.Columns.Add("Disc_Tax_Catg3", typeof(string)); //178
                    ph_tbl.Columns.Add("Disc_Tax_Rate3", typeof(string)); //179
                    ph_tbl.Columns.Add("Disc_Tax_Amt3", typeof(string)); //180
                    ph_tbl.Columns.Add("Disc_Tax_Catg4", typeof(string)); //181
                    ph_tbl.Columns.Add("Disc_Tax_Rate4", typeof(string)); //182
                    ph_tbl.Columns.Add("Disc_Tax_Amt4", typeof(string)); //183
                    ph_tbl.Columns.Add("Special_Chg_Des", typeof(string)); //184
                    ph_tbl.Columns.Add("Special_Chg_Amt", typeof(string)); //185
                    ph_tbl.Columns.Add("Special_Chg_TaxCatg1", typeof(string)); //186
                    ph_tbl.Columns.Add("Special_Chg_TaxRate1", typeof(string)); //187
                    ph_tbl.Columns.Add("Special_Chg_TaxAmt1", typeof(string)); //188
                    ph_tbl.Columns.Add("Special_Chg_TaxCatg2", typeof(string)); //189
                    ph_tbl.Columns.Add("Special_Chg_TaxRate2", typeof(string)); //190
                    ph_tbl.Columns.Add("Special_Chg_TaxAmt2", typeof(string)); //191
                    ph_tbl.Columns.Add("Special_Chg_TaxCatg3", typeof(string)); //192
                    ph_tbl.Columns.Add("Special_Chg_TaxRate3", typeof(string)); //193
                    ph_tbl.Columns.Add("Special_Chg_TaxAmt3", typeof(string)); //194
                    ph_tbl.Columns.Add("Special_Chg_TaxCatg4", typeof(string)); //195
                    ph_tbl.Columns.Add("Special_Chg_TaxRate4", typeof(string)); //196
                    ph_tbl.Columns.Add("Special_Chg_TaxAmt4", typeof(string)); //197
                    ph_tbl.Columns.Add("Carriage_Desc", typeof(string)); //198
                    ph_tbl.Columns.Add("Carriage_Amt", typeof(string)); //199
                    ph_tbl.Columns.Add("Carriage_Tax_Catg1", typeof(string)); //200
                    ph_tbl.Columns.Add("Carriage_Tax_Rate1", typeof(string)); //201
                    ph_tbl.Columns.Add("Carriage_Tax_Amt1", typeof(string)); //202
                    ph_tbl.Columns.Add("Carriage_Tax_Catg2", typeof(string)); //203
                    ph_tbl.Columns.Add("Carriage_Tax_Rate2", typeof(string)); //204
                    ph_tbl.Columns.Add("Carriage_Tax_Amt2", typeof(string)); //205
                    ph_tbl.Columns.Add("Carriage_Tax_Catg3", typeof(string)); //206
                    ph_tbl.Columns.Add("Carriage_Tax_Rate3", typeof(string)); //207
                    ph_tbl.Columns.Add("Carriage_Tax_Amt3", typeof(string)); //208
                    ph_tbl.Columns.Add("Carriage_Tax_Catg4", typeof(string)); //209
                    ph_tbl.Columns.Add("Carriage_Tax_Rate4", typeof(string)); //210
                    ph_tbl.Columns.Add("Carriage_Tax_Amt4", typeof(string)); //211
                    ph_tbl.Columns.Add("Frieght_Des", typeof(string)); //212............p
                    ph_tbl.Columns.Add("Frieght_Amt", typeof(string)); //213............p
                    ph_tbl.Columns.Add("Freight_Tax_Catg1", typeof(string)); //214............p
                    ph_tbl.Columns.Add("Freight_Tax_Rate1", typeof(string)); //215............p
                    ph_tbl.Columns.Add("Freight_Tax_Amt1", typeof(string)); //216............p
                    ph_tbl.Columns.Add("Freight_Tax_Catg2", typeof(string)); //217............p
                    ph_tbl.Columns.Add("Freight_Tax_Rate2", typeof(string)); //218............p
                    ph_tbl.Columns.Add("Freight_Tax_Amt2", typeof(string)); //219............p
                    ph_tbl.Columns.Add("Freight_Tax_Catg3", typeof(string)); //220............p
                    ph_tbl.Columns.Add("Freight_Tax_Rate3", typeof(string)); //221............p
                    ph_tbl.Columns.Add("Freight_Tax_Amt3", typeof(string)); //222............p
                    ph_tbl.Columns.Add("Freight_Tax_Catg4", typeof(string)); //223
                    ph_tbl.Columns.Add("Freight_Tax_Rate4", typeof(string)); //224
                    ph_tbl.Columns.Add("Freight_Tax_Amt4", typeof(string)); //225
                    ph_tbl.Columns.Add("Insur_Desc", typeof(string)); //226
                    ph_tbl.Columns.Add("Insur_Amt", typeof(string)); //227
                    ph_tbl.Columns.Add("Insur_Tax_Catg1", typeof(string)); //228
                    ph_tbl.Columns.Add("Insur_Tax_Rate1", typeof(string)); //229
                    ph_tbl.Columns.Add("Insur_Tax_Amt1", typeof(string)); //230
                    ph_tbl.Columns.Add("Insur_Tax_Catg2", typeof(string)); //231
                    ph_tbl.Columns.Add("Insur_Tax_Rate2", typeof(string)); //232
                    ph_tbl.Columns.Add("Insur_Tax_Amt2", typeof(string)); //233
                    ph_tbl.Columns.Add("Insur_Tax_Catg3", typeof(string)); //234
                    ph_tbl.Columns.Add("Insur_Tax_Rate3", typeof(string)); //235
                    ph_tbl.Columns.Add("Insur_Tax_Amt3", typeof(string)); //236
                    ph_tbl.Columns.Add("Insur_Tax_Catg4", typeof(string)); //237
                    ph_tbl.Columns.Add("Insur_Tax_Rate4", typeof(string)); //238
                    ph_tbl.Columns.Add("Insur_Tax_Amt4", typeof(string)); //239
                    ph_tbl.Columns.Add("Pack_Desc", typeof(string)); //240
                    ph_tbl.Columns.Add("Pack_Amt", typeof(string)); //241
                    ph_tbl.Columns.Add("Pack_Tax_Catg1", typeof(string)); //242
                    ph_tbl.Columns.Add("Pack_Tax_Rate1", typeof(string)); //243
                    ph_tbl.Columns.Add("Pack_Tax_Amt1", typeof(string)); //244
                    ph_tbl.Columns.Add("Pack_Tax_Catg2", typeof(string)); //245
                    ph_tbl.Columns.Add("Pack_Tax_Rate2", typeof(string)); //246
                    ph_tbl.Columns.Add("Pack_Tax_Amt2", typeof(string)); //247
                    ph_tbl.Columns.Add("Pack_Tax_Catg3", typeof(string)); //248
                    ph_tbl.Columns.Add("Pack_Tax_Rate3", typeof(string)); //249
                    ph_tbl.Columns.Add("Pack_Tax_Amt3", typeof(string)); //250
                    ph_tbl.Columns.Add("Pack_Tax_Catg4", typeof(string)); //251
                    ph_tbl.Columns.Add("Pack_Tax_Rate4", typeof(string)); //252
                    ph_tbl.Columns.Add("Pack_Tax_Amt4", typeof(string)); //253
                    ph_tbl.Columns.Add("Admin_Chg_Desc", typeof(string)); //254
                    ph_tbl.Columns.Add("Admin_Chg_Amt", typeof(string)); //255
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Catg1", typeof(string)); //256
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Rate1", typeof(string)); //257
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Amt1", typeof(string)); //258
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Catg2", typeof(string)); //259
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Rate2", typeof(string)); //260
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Amt2", typeof(string)); //261
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Catg3", typeof(string)); //262
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Rate3", typeof(string)); //263
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Amt3", typeof(string)); //264
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Catg4", typeof(string)); //265
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Rate4", typeof(string)); //266
                    ph_tbl.Columns.Add("Admin_Chg_Tax_Amt4", typeof(string)); //267
                    ph_tbl.Columns.Add("Fuel_Surcharge_Desc", typeof(string)); //268
                    ph_tbl.Columns.Add("Fuel_Surcharge_Amt", typeof(string)); //269
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxCatg1", typeof(string)); //270
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxRate1", typeof(string)); //271
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxAmt1", typeof(string)); //272
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxCatg2", typeof(string)); //273
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxRate2", typeof(string)); //274
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxAmt2", typeof(string)); //275
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxCatg3", typeof(string)); //276
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxRate3", typeof(string)); //277
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxAmt3", typeof(string)); //278
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxCatg4", typeof(string)); //279
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxRate4", typeof(string)); //280
                    ph_tbl.Columns.Add("Fuel_Surcharge_TaxAmt4", typeof(string)); //281
                    ph_tbl.Columns.Add("Green_Tax_Desc", typeof(string)); //282
                    ph_tbl.Columns.Add("Green_Tax_Amt", typeof(string)); //283
                    ph_tbl.Columns.Add("Green_Tax_Catg1", typeof(string)); //284
                    ph_tbl.Columns.Add("Green_Tax_Rate1", typeof(string)); //285
                    ph_tbl.Columns.Add("Green_Tax_Amt1", typeof(string)); //286
                    ph_tbl.Columns.Add("Green_Tax_Catg2", typeof(string)); //287
                    ph_tbl.Columns.Add("Green_Tax_Rate2", typeof(string)); //288
                    ph_tbl.Columns.Add("Green_Tax_Amt2", typeof(string)); //289
                    ph_tbl.Columns.Add("Green_Tax_Catg3", typeof(string)); //290
                    ph_tbl.Columns.Add("Green_Tax_Rate3", typeof(string)); //291
                    ph_tbl.Columns.Add("Green_Tax_Amt3", typeof(string)); //292	
                    ph_tbl.Columns.Add("Green_Tax_Catg4", typeof(string)); //293
                    ph_tbl.Columns.Add("Green_Tax_Rate4", typeof(string)); //294
                    ph_tbl.Columns.Add("Green_Tax_Amt4", typeof(string)); //295
                    ph_tbl.Columns.Add("Rounding_Line_Desc", typeof(string)); //296
                    ph_tbl.Columns.Add("Rounding_Line_Amt", typeof(string)); //297
                    ph_tbl.Columns.Add("Rounding_Line_Taxcatg1", typeof(string)); //298
                    ph_tbl.Columns.Add("Rounding_Line_TaxRate1", typeof(string)); //299
                    ph_tbl.Columns.Add("Rounding_Line_TaxAmt1", typeof(string)); //300
                    ph_tbl.Columns.Add("Rounding_Line_Taxcatg2", typeof(string)); //301
                    ph_tbl.Columns.Add("Rounding_Line_TaxRate2", typeof(string)); //302
                    ph_tbl.Columns.Add("Rounding_Line_TaxAmt2", typeof(string)); //303
                    ph_tbl.Columns.Add("Rounding_Line_Taxcatg3", typeof(string)); //304
                    ph_tbl.Columns.Add("Rounding_Line_TaxRate3", typeof(string)); //305
                    ph_tbl.Columns.Add("Rounding_Line_TaxAmt3", typeof(string)); //306
                    ph_tbl.Columns.Add("Rounding_Line_Taxcatg4", typeof(string)); //307
                    ph_tbl.Columns.Add("Rounding_Line_TaxRate4", typeof(string)); //308
                    ph_tbl.Columns.Add("Rounding_Line_TaxAmt4", typeof(string)); //309
                    ph_tbl.Columns.Add("Demurrage", typeof(string)); //310
                    ph_tbl.Columns.Add("Demurrage_Amt", typeof(string)); //311
                    ph_tbl.Columns.Add("Demurrage_TaxCatg1", typeof(string)); //312
                    ph_tbl.Columns.Add("Demurrage_TaxRate1", typeof(string)); //313
                    ph_tbl.Columns.Add("Demurrage_TaxAmt1", typeof(string)); //314
                    ph_tbl.Columns.Add("Demurrage_TaxCatg2", typeof(string)); //315
                    ph_tbl.Columns.Add("Demurrage_TaxRate2", typeof(string)); //316
                    ph_tbl.Columns.Add("Demurrage_TaxAmt2", typeof(string)); //317
                    ph_tbl.Columns.Add("Demurrage_TaxCatg3", typeof(string)); //318
                    ph_tbl.Columns.Add("Demurrage_TaxRate3", typeof(string)); //319
                    ph_tbl.Columns.Add("Demurrage_TaxAmt3", typeof(string)); //320
                    ph_tbl.Columns.Add("Demurrage_TaxCatg4", typeof(string)); //321
                    ph_tbl.Columns.Add("Demurrage_TaxRate4", typeof(string)); //322
                    ph_tbl.Columns.Add("Demurrage_TaxAmt4", typeof(string)); //323
                    ph_tbl.Columns.Add("Adv_Recycle_FeeDesc", typeof(string)); //324
                    ph_tbl.Columns.Add("Adv_Recycle_FeeAmt", typeof(string)); //325
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_Taxcatg1", typeof(string)); //326
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxRate1", typeof(string)); //327
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxAmt1", typeof(string)); //328
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_Taxcatg2", typeof(string)); //329	
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxRate2", typeof(string)); //330
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxAmt2", typeof(string)); //331
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_Taxcatg3", typeof(string)); //332
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxRate3", typeof(string)); //333
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxAmt3", typeof(string)); //334
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_Taxcatg4", typeof(string)); //335
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxRate4", typeof(string)); //336
                    ph_tbl.Columns.Add("Adv_Recycle_Fee_TaxAmt4", typeof(string)); //337
                    ph_tbl.Columns.Add("Invoie_Dtl1", typeof(string)); //338
                    ph_tbl.Columns.Add("Invoie_Dtl2", typeof(string)); //339
                    ph_tbl.Columns.Add("Invoie_Dtl3", typeof(string)); //340
                    ph_tbl.Columns.Add("Disc_Per_Line", typeof(string)); //341
                    ph_tbl.Columns.Add("Disc_Per_Amt", typeof(string)); //342
                    ph_tbl.Columns.Add("Supp_Id", typeof(string)); //343
                    ph_tbl.Columns.Add("Inv_From_TaxRegNo", typeof(string)); //344
                    ph_tbl.Columns.Add("Inv_To_TaxRegNo", typeof(string)); //345
                    ph_tbl.Columns.Add("Third_Prty_delv_TicketNo", typeof(string)); //346
                    ph_tbl.Columns.Add("Delv_TaxReg_No", typeof(string)); //347
                    ph_tbl.Columns.Add("Endordement", typeof(string)); //348
                    ph_tbl.Columns.Add("Input_Tax_Credit", typeof(string)); //349
                    ph_tbl.Columns.Add("Payble_Tax_On_Rev_Chgs", typeof(string)); //350
                    ph_tbl.Columns.Add("ISD_no", typeof(string)); //351
                    ph_tbl.Columns.Add("Adv_Pymt_Amt", typeof(string)); //352
                    ph_tbl.Columns.Add("Party_No", typeof(string)); //353
                    ph_tbl.Columns.Add("PEC_Email", typeof(string)); //354
                    ph_tbl.Columns.Add("Natural_Person_Name", typeof(string)); //355
                    ph_tbl.Columns.Add("Natural_Person_SrName", typeof(string)); //356
                    ph_tbl.Columns.Add("Fiscal_code_Natural_Person", typeof(string)); //357
                    ph_tbl.Columns.Add("Supp_ord_No", typeof(string)); //358
                    ph_tbl.Columns.Add("Remit_to_name", typeof(string)); //359
                    ph_tbl.Columns.Add("Remit_To_Street1", typeof(string)); //360
                    ph_tbl.Columns.Add("Remit_To_Street2", typeof(string)); //361
                    ph_tbl.Columns.Add("Remit_To_city", typeof(string)); //362
                    ph_tbl.Columns.Add("Remit_To_state", typeof(string)); //363
                    ph_tbl.Columns.Add("Remit_To_Postal_Code", typeof(string)); //364
                    ph_tbl.Columns.Add("Remit_To_Country", typeof(string)); //365
                    ph_tbl.Columns.Add("Alt_Ref", typeof(string)); //366
                    ph_tbl.Columns.Add("Campaign_Name", typeof(string)); //367
                    ph_tbl.Columns.Add("Campaign_Id", typeof(string)); //368
                    ph_tbl.Columns.Add("Media_Type", typeof(string)); //369
                    ph_tbl.Columns.Add("Invoice_Period", typeof(string)); //370
                    ph_tbl.Columns.Add("Advertiser_Name", typeof(string)); //371
                    ph_tbl.Columns.Add("Advertiser_Brand", typeof(string)); //372
                    ph_tbl.Columns.Add("Supp_PAN_no", typeof(string)); //373
                    #endregion

                    hfcode.Value = value1;
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");//PARTY
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");  //FSTR

                    cond = "and a.acode like '" + mq1 + "%' ";
                    SQuery = "select a.fstr,a.fryt_amt,a.no_bdls as no_of_pkg,A.BANK_PF,a.morder,a.icode,a.buyer_part_no,a.net,a.amt_Exc,a.rvalue,a.buyer_Adres,a.gross,a.type,a.iopr,a.branchcd,a.inv_no,a.inv_date,A.BUYCODE,a.customer_Code,a.PO_LINE_NO,a.customer,a.vehicle_no,a.ship_to_Add1,a.ship_to_Add2,a.ship_to_Add3,a.ship_to_pinc,a.ship_to_gstin,A.gst_rate,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.cgst_rate,a.cgst_amt,a.igst_rate,a.igst_amt,a.sgst_rate,a.sgst_amt,a.tax_type_sg,a.tax_type_ig,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.currency,a.cscode,a.pono,a.podate,A.STATEN,A.COUNTRY,A.pymt_trm,A.mode_tpt,a.addr1,a.addr2,a.addr3,a.addr4,A.TADR,A.TADR1,A.TSTATE,A.ZIPCODE,A.BANKNAME,A.BANKADDR,A.BANKAC,A.IFSC_CODE,A.UNIT from (select (nvl(a.iqty_chl,0) * nvl(a.irate,0)) as fryt_amt,(case when nvl(a.exc_57f4,'-')!='-' then a.exc_57f4 else c.cpartno end) as buyer_part_no,s.no_bdls,a.morder,a.icode,(case when substr(a.type,1,1)='4' then '380' else '381'end) as type,TRIM(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'YYYY-MM-DD') as inv_date,trim(a.acode) as customer_Code,a.iopr,IS_NUMBER(A.binno) AS PO_LINE_NO,b.aname as customer,trim(b.addr1)||trim(b.addr2)||trim(b.addr3) as buyer_Adres,trim(b.addr1) as ship_to_Add1,trim(b.addr2) as ship_to_Add2,trim(b.addr3) as ship_to_Add3,trim(b.pincode) as ship_to_pinc ,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,B.BUYCODE,trim(b.gst_no) as ship_to_gstin,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,(Case when A.IOPR='CG' then a.exc_rate else 0 end ) as cgst_rate,(Case when A.IOPR='CG' then a.exc_amt else 0 end ) as cgst_amt,(Case when A.IOPR='IG' then a.exc_rate else 0 end ) as igst_rate,(Case when A.IOPR='IG' then a.exc_amt else 0 end ) as igst_amt,a.cess_percent as sgst_rate,a.cess_pu as sgst_amt,a.exc_rate as gst_rate,a.finvno as po_ref,c.hscode as hsn_code,T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,TRIM(T.ADDR) AS TADR,TRIM(T.ADDR1) AS TADR1,TRIM(T.PLACE) AS TSTATE,T.ZIPCODE,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number  ,(Case when A.IOPR='CG' then 'CGST/SGST' else 'IGST' end ) as tax_type_sg,(Case when A.IOPR='IG' then 'IGST' else '-' end ) as tax_type_ig  ,'INR'  as currency,s.cscode,s.mode_tpt,s.mo_vehi as vehicle_no,s.pono,to_char(s.podate,'dd/mm/yyyy') as podate,s.amt_sale as net,s.bill_tot as gross,nvl(s.amt_exc,0) as amt_Exc,nvl(s.rvalue,0) as rvalue,C.UNIT,C.HSCODE,B.STATEN,B.COUNTRY,s.ins_co as pymt_trm,T.BANKNAME,T.BANKADDR,T.BANKAC,T.IFSC_CODE,T.BANK_PF from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') " + cond + " AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + mq2 + ") and substr(trim(a.icode),1,2)!='59' and a.store='Y') a  ORDER BY a.morder";  //dt for only icode not freight
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    ///                  
                    mq0 = "select a.fstr,a.fryt_amt,a.no_bdls as no_of_pkg,a.morder,a.icode,a.net,a.amt_Exc,a.rvalue,a.buyer_Adres,a.gross,a.type,a.iopr,a.branchcd,a.inv_no,a.inv_date,A.BUYCODE,a.customer_Code,a.PO_LINE_NO,a.customer,a.vehicle_no,a.ship_to_Addr1,a.ship_to_Addr2,a.ship_to_Addr3,a.ship_to_pinc,a.ship_to_gstin,A.gst_rate,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.cgst_rate,a.cgst_amt,a.igst_rate,a.igst_amt,a.sgst_rate,a.sgst_amt,a.tax_type_sg,a.tax_type_ig,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.currency,a.cscode,a.pono,a.podate,A.STATEN,A.COUNTRY,A.pymt_trm,A.mode_tpt,a.addr1,a.addr2,a.addr3,a.addr4,A.TADR,A.TADR1,A.TSTATE,A.ZIPCODE,A.BANKNAME,A.BANKADDR,A.BANKAC,A.IFSC_CODE,A.UNIT from (select (nvl(a.iqty_chl,0) * nvl(a.irate,0)) as fryt_amt,s.no_bdls,a.morder,a.icode,(case when substr(a.type,1,1)='4' then '380' else '381'end) as type,TRIM(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'YYYY-MM-DD') as inv_date,trim(a.acode) as customer_Code,a.iopr,IS_NUMBER(A.binno) AS PO_LINE_NO,b.aname as customer,trim(b.addr1)||trim(b.addr2)||trim(b.addr3) as buyer_Adres,trim(b.addr1) as ship_to_Addr1,trim(b.addr2) as ship_to_Addr2,trim(b.addr3) as ship_to_Addr3,trim(b.) as ship_to_pinc,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,B.BUYCODE,trim(b.gst_no) as ship_to_gstin,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,(Case when A.IOPR='CG' then a.exc_rate else 0 end ) as cgst_rate,(Case when A.IOPR='CG' then a.exc_amt else 0 end ) as cgst_amt,(Case when A.IOPR='IG' then a.exc_rate else 0 end ) as igst_rate,(Case when A.IOPR='IG' then a.exc_amt else 0 end ) as igst_amt,a.cess_percent as sgst_rate,a.cess_pu as sgst_amt,a.exc_rate as gst_rate,a.finvno as po_ref,c.hscode as hsn_code,T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,TRIM(T.ADDR) AS TADR,TRIM(T.ADDR1) AS TADR1,TRIM(T.PLACE) AS TSTATE,T.ZIPCODE,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number  ,(Case when A.IOPR='CG' then 'CGST/SGST' else 'IGST' end ) as tax_type_sg,(Case when A.IOPR='IG' then 'IGST' else '-' end ) as tax_type_ig  ,'INR'  as currency,s.cscode,s.mode_tpt,s.mo_vehi as vehicle_no,s.pono,to_char(s.podate,'dd/mm/yyyy') as podate,s.amt_sale as net,s.bill_tot as gross,nvl(s.amt_exc,0) as amt_Exc,nvl(s.rvalue,0) as rvalue,C.UNIT,C.HSCODE,B.STATEN,B.COUNTRY,s.ins_co as pymt_trm,T.BANKNAME,T.BANKADDR,T.BANKAC,T.IFSC_CODE from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') " + cond + " AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + mq2 + ") and substr(trim(a.icode),1,2)='59' and a.store='X') a  ORDER BY a.morder";
                    mq0 = "select a.fstr,a.fryt_amt,a.no_bdls as no_of_pkg,A.BANK_PF,a.morder,a.icode,a.buyer_part_no,a.net,a.amt_Exc,a.rvalue,a.buyer_Adres,a.gross,a.type,a.iopr,a.branchcd,a.inv_no,a.inv_date,A.BUYCODE,a.customer_Code,a.PO_LINE_NO,a.customer,a.vehicle_no,a.ship_to_Address,a.ship_to_gstin,A.gst_rate,a.part_no,a.part_name,a.qty_sold as qty_sold,a.irate,a.basic as basic,a.cgst_rate,a.cgst_amt,a.igst_rate,a.igst_amt,a.sgst_rate,a.sgst_amt,a.tax_type_sg,a.tax_type_ig,a.po_ref,a.hsn_code,a.supplier_Company_Name_Addr,a.Supplier_GST_number,a.Supplier_PAN_number,a.currency,a.cscode,a.pono,a.podate,A.STATEN,A.COUNTRY,A.pymt_trm,A.mode_tpt,a.addr1,a.addr2,a.addr3,a.addr4,A.TADR,A.TADR1,A.TSTATE,A.ZIPCODE,A.BANKNAME,A.BANKADDR,A.BANKAC,A.IFSC_CODE,A.UNIT from (select (nvl(a.iqty_chl,0) * nvl(a.irate,0)) as fryt_amt,(case when nvl(a.exc_57f4,'-')!='-' then a.exc_57f4 else c.cpartno end) as buyer_part_no,s.no_bdls,a.morder,a.icode,(case when substr(a.type,1,1)='4' then '380' else '381'end) as type,TRIM(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.branchcd, a.vchnum as inv_no,to_char(a.vchdate,'YYYY-MM-DD') as inv_date,trim(a.acode) as customer_Code,a.iopr,IS_NUMBER(A.binno) AS PO_LINE_NO,b.aname as customer,trim(b.addr1)||trim(b.addr2)||trim(b.addr3) as buyer_Adres,trim(b.addr1)||trim(b.addr2)||trim(b.addr3)||trim(b.addr4)||trim(b.pincode) as ship_to_Address,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,B.BUYCODE,trim(b.gst_no) as ship_to_gstin,c.cpartno as part_no,c.iname as part_name,nvl(a.iqtyout,0) as qty_sold,a.irate,nvl(a.iamount,0) as basic,(Case when A.IOPR='CG' then a.exc_rate else 0 end ) as cgst_rate,(Case when A.IOPR='CG' then a.exc_amt else 0 end ) as cgst_amt,(Case when A.IOPR='IG' then a.exc_rate else 0 end ) as igst_rate,(Case when A.IOPR='IG' then a.exc_amt else 0 end ) as igst_amt,a.cess_percent as sgst_rate,a.cess_pu as sgst_amt,a.exc_rate as gst_rate,a.finvno as po_ref,c.hscode as hsn_code,T.name||','||T.addr||','||T.addr1||' '||T.ADDR2 as supplier_Company_Name_Addr,TRIM(T.ADDR) AS TADR,TRIM(T.ADDR1) AS TADR1,TRIM(T.PLACE) AS TSTATE,T.ZIPCODE,T.gst_no as Supplier_GST_number, T.gir_num as Supplier_PAN_number  ,(Case when A.IOPR='CG' then 'CGST/SGST' else 'IGST' end ) as tax_type_sg,(Case when A.IOPR='IG' then 'IGST' else '-' end ) as tax_type_ig  ,'INR'  as currency,s.cscode,s.mode_tpt,s.mo_vehi as vehicle_no,s.pono,to_char(s.podate,'dd/mm/yyyy') as podate,s.amt_sale as net,s.bill_tot as gross,nvl(s.amt_exc,0) as amt_Exc,nvl(s.rvalue,0) as rvalue,C.UNIT,C.HSCODE,B.STATEN,B.COUNTRY,s.ins_co as pymt_trm,T.BANKNAME,T.BANKADDR,T.BANKAC,T.IFSC_CODE,T.BANK_PF from ivoucher a,famst b,item c,type t,sale s  where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=s.branchcd||trim(s.type)||trim(s.vchnum)||to_char(s.vchdate,'dd/mm/yyyy') " + cond + " AND trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + mq2 + ") and substr(trim(a.icode),1,2)='59' and a.store='X') a  ORDER BY a.morder";  //dt for only icode not freight
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq0); //dt for only freight
                    //////
                    mq1 = "Select distinct trim(d.aname)||','||trim(d.addr1)||','||trim(d.addr2)||','||trim(d.addr3)||','||trim(d.addr4) as name,d.acode as acode,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d ";
                    mq1 = "Select distinct trim(d.aname)||','||trim(d.addr1)||','||trim(d.addr2)||','||trim(d.addr3)||','||trim(d.addr4) as fstr,d.aname,trim(d.addr1) as addr1,trim(d.addr2) as addr2,trim(d.addr3) as addr3,trim(d.addr4) as addr4,d.acode as acode,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);
                    dr1 = ph_tbl.NewRow();
                    //iqty_chl * irate===for freight val
                    mq3 = "select NAME,TRIM(ADDR)||TRIM(ADDR1) AS ADRES,GST_NO from type where id='B' AND TYPE1='" + mbr + "'";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);//type or supplier details
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "inv_no"); //MAIN                  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "inv_no='" + dr0["inv_no"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();

                            if (dt4.Rows.Count > 0)
                            {
                                DataView viewim1 = new DataView(dt4, "inv_no='" + dr0["inv_no"] + "'", "", DataViewRowState.CurrentRows);
                                dt5 = viewim1.ToTable();//freight dt
                            }
                            db19 = 0; int k = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                #region vel-vin
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0;
                                mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0; db22 = 0; db23 = 0;
                                // billtot-(amt_sale+amt_exc+rvalue)==roundof formula////if value=0 then no need to show otherwise need to show                             
                                db12 = fgen.make_double(dt1.Rows[i]["gross"].ToString().Trim());
                                db13 = fgen.make_double(dt1.Rows[i]["net"].ToString().Trim());
                                db14 = fgen.make_double(dt1.Rows[i]["amt_Exc"].ToString().Trim());
                                db15 = fgen.make_double(dt1.Rows[i]["rvalue"].ToString().Trim());
                                db11 = Math.Round(db12 - (db13 + db14 + db15), 2);//round of value
                                mq5 = dt1.Rows[i]["iopr"].ToString().Trim();
                                dr1 = ph_tbl.NewRow();
                                //===============
                                dr1["INV_NO"] = dt1.Rows[i]["inv_no"].ToString().Trim().Replace(",", " ");//1
                                dr1["INV_DT"] = dt1.Rows[i]["inv_date"].ToString().Trim().Replace(",", " ");//2
                                dr1["Tax_Point_Dt"] = "";//3                              
                                dr1["INV_type"] = dt1.Rows[i]["type"].ToString().Trim().Replace(",", " ");//4....set condtion on that as above
                                dr1["buyerid"] = dt1.Rows[i]["BUYCODE"].ToString().Trim().Replace(",", " ");//5
                                dr1["Org_Inv_no"] = "";//6
                                dr1["Org_Inv_dt"] = "";//7
                                dr1["Org_delv_dt"] = "";//8
                                dr1["Credit_Reson"] = "";//9
                                dr1["po_number"] = dt1.Rows[i]["pono"].ToString().Trim().Replace(",", " ");//10.............pick
                                dr1["Delv_note_number"] = "";//11
                                dr1["Payment_Ref"] = "";//12
                                dr1["Payment_Method"] = "";//13
                                dr1["Form_of_Pymt"] = "";//14
                                dr1["Pymt_terms"] = dt1.Rows[i]["PYMT_TRM"].ToString().Trim().Replace(",", " ");//15..................mode_tpt 
                                dr1["Net_Pymt_Days"] = "";//16
                                dr1["Start_date"] = "";//17
                                dr1["End_Date"] = "";//18
                                dr1["Order_Date"] = "";//19
                                dr1["Delv_Dt"] = dt1.Rows[i]["inv_date"].ToString().Trim().Replace(",", " ");//20
                                dr1["Ship_Date"] = "";//21
                                dr1["Declr_dt"] = "";//22
                                dr1["Payment_due_by_dt"] = "";//23
                                dr1["Early_pymt_dt"] = "";//24
                                dr1["Mfg_dt"] = "";//25
                                dr1["Expiry_Dt"] = "";//26
                                dr1["Inv_From_Name"] = "";//27
                                dr1["Inv_From_add1"] = "";//28
                                dr1["Inv_From_add2"] = "";//29
                                dr1["Inv_From_city"] = "";//30
                                dr1["Inv_From_PostalCode"] = "";//31
                                dr1["Inv_From_state"] = "";//32
                                dr1["Inv_From_country"] = "";//33
                                dr1["Supp_Qst_Tax_Reg_Num"] = "";//34
                                dr1["Supp_Pst_Tax_Reg_Num"] = "";//35
                                dr1["Supp_Gst_Tax_Reg_Num"] = "";//36
                                dr1["Supp_Hst_Tax_Reg_Num"] = "";//37
                                dr1["Main_Supp_Cont_Name"] = "";//38
                                dr1["Main_Supp_Cont_Tel"] = "";//39
                                dr1["Main_Supp_Cont_email"] = "";//40
                                dr1["Cont_Num_for_INVQ"] = "";//41
                                dr1["Cont_tel_for_INVQ"] = "";//42
                                dr1["Cont_email_for_INVQ"] = "";//43
                                dr1["Inv_to_name"] = "";//44
                                dr1["Inv_to_add1"] = "";//45
                                dr1["Inv_to_add2"] = "";//46
                                dr1["Inv_to_city"] = "";//47
                                dr1["Inv_to_Postalcode"] = "";//48
                                dr1["Inv_to_state"] = "";//49
                                dr1["Inv_to_country"] = "";//50
                                dr1["Buyer_Cont_Name"] = "";//51
                                dr1["Buyer_Cont_tel"] = "";//52
                                dr1["Buyer_Cont_email"] = "";//53
                                dr1["Supp_Fiscal_rep_name"] = "";//54
                                dr1["Supp_Fiscal_rep_add1"] = "";//55
                                dr1["Supp_Fiscal_rep_add2"] = "";//56
                                dr1["Supp_Fiscal_rep_city"] = "";//57
                                dr1["Supp_Fiscal_rep_PostalCode"] = "";//58
                                dr1["Supp_Fiscal_rep_state"] = "";//59
                                dr1["Supp_Fiscal_rep_country"] = "";//60
                                dr1["Supp_Fiscal_rep_TaxReg_Name"] = "";//61                                                    
                                if (dt1.Rows[i]["cscode"].ToString().Trim() == "-")
                                {
                                    dr1["Ship_to_Name"] = dt1.Rows[i]["customer"].ToString().Trim().Replace(",", " ");//62
                                    dr1["Ship_to_Add1"] = "";
                                    dr1["Ship_to_Add2"] = "";
                                    dr1["Ship_to_City"] = "";
                                    dr1["Ship_to_PostalCode"] = "";
                                    dr1["Ship_to_state"] = dt1.Rows[i]["STATEN"].ToString().Trim();//67===============p
                                    dr1["Ship_to_Country"] = dt1.Rows[i]["COUNTRY"].ToString().Trim();//68===============p
                                    dr1["Ship_to_Tax_Reg_Num"] = dt1.Rows[i]["ship_to_gstin"].ToString().Trim();//69===============p
                                }
                                else
                                {
                                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(aname)||'~'||trim(addr1)||'~'||trim(addr2)||'~'||trim(addr3)||'~'||trim(pincode)||'~'||trim(staten)||'~'||gst_no as mq0 from csmst where trim(acode)='" + dt1.Rows[i]["cscode"].ToString().Trim() + "' ", "mq0");
                                    if (mq0.Trim().Length > 2)
                                    {
                                        dr1["Ship_to_Name"] = mq0.Split('~')[0].ToString().Trim().ToUpper(); ;//62
                                        dr1["Ship_to_Add1"] = mq0.Split('~')[0].ToString().Trim().ToUpper();//63
                                        dr1["Ship_to_Add2"] = mq0.Split('~')[1].ToString().Trim().ToUpper();//64    
                                        dr1["Ship_to_City"] = mq0.Split('~')[0].ToString().Trim().ToUpper();//65
                                        dr1["Ship_to_PostalCode"] = mq0.Split('~')[1].ToString().Trim().ToUpper();//66================p
                                        dr1["Ship_to_state"] = mq0.Split('~')[1].ToString().Trim().ToUpper();//67===============p
                                        dr1["Ship_to_Country"] = dt1.Rows[i]["COUNTRY"].ToString().Trim();//68===============p
                                        dr1["Ship_to_Tax_Reg_Num"] = mq0.Split('~')[1].ToString().Trim().ToUpper();//69===============p
                                    }
                                }
                                dr1["Ship_from_Name"] = fgenCO.chk_co(frm_cocd); //70===============p
                                dr1["Ship_from_Add1"] = dt1.Rows[i]["TADR"].ToString().Trim().ToUpper().Replace(",", " ");//71===============p.MBR...ADDR
                                dr1["Ship_from_Add2"] = "";
                                mq3 = dt1.Rows[i]["TADR1"].ToString().Trim();
                                dr1["Ship_from_Add2"] = "";
                                mq4 = mq3.Split(',')[1].ToString().Trim().ToUpper();
                                dr1["Ship_from_City"] = mq3.Split(',')[0].ToString().Trim().ToUpper();
                                dr1["Ship_from_PostalCode"] = dt1.Rows[i]["ZIPCODE"].ToString().Trim();//74===============ZIPCODE
                                dr1["Ship_from_state"] = dt1.Rows[i]["TSTATE"].ToString().Trim();//75===============p
                                dr1["Ship_from_Country"] = "INDIA";//76===============p
                                dr1["Ship_from_Tax_Reg_Num"] = dt1.Rows[i]["Supplier_GST_number"].ToString().Trim();//77===============p//
                                dr1["Ordered_By_Name"] = "";//78
                                dr1["Ordered_By_Add1"] = "";//79
                                dr1["Ordered_By_Add2"] = "";//80
                                dr1["Ordered_By_City"] = "";//81
                                dr1["Ordered_By_PostalCode"] = "";//82
                                dr1["Ordered_By_state"] = "";//83
                                dr1["Ordered_By_Country"] = "";//84
                                dr1["Inv_Net_Amt"] = "";// fgen.make_double(dt1.Rows[i]["net"].ToString().Trim()); ///85......p                                                              
                                dr1["Inv_Tax_Amt"] = db14 + db15;
                                dr1["Inv_Gross_Amt"] = fgen.make_double(dt1.Rows[i]["gross"].ToString().Trim());   //87   .....p                             
                                dr1["Currency"] = dt1.Rows[i]["Currency"].ToString().Trim().Replace(",", " "); ;//88.....p    
                                dr1["Local_Currency"] = "";//89
                                dr1["Exchange_Rate"] = "";//90
                                dr1["Local_Currency_Net_Amt"] = "";//91
                                dr1["Local_Currency_Tax_Amt"] = "";//92
                                dr1["Local_Currency_Gross_Amt"] = "";//93
                                dr1["bank_name"] = dt1.Rows[i]["BANKNAME"].ToString().Trim();//94===============p
                                dr1["bank_Address"] = dt1.Rows[i]["BANKADDR"].ToString().Trim();//95===============p
                                dr1["sort_code"] = dt1.Rows[i]["IFSC_CODE"].ToString().Trim();//96===============p
                                dr1["bank_Acc_Number"] = dt1.Rows[i]["BANKAC"].ToString().Trim(); ;//97===============p
                                dr1["bank_Acc_name"] = fgenCO.chk_co(frm_cocd);//dt1.Rows[i]["BANK_PF"].ToString().Trim(); //98===============p
                                dr1["IBAN"] = "";//99
                                dr1["SWIFT"] = "";//100
                                dr1["PO_Line_Num"] = "";//101===============p
                                dr1["Qty"] = fgen.make_double(dt1.Rows[i]["QTY_SOLD"].ToString().Trim());//102===============p     
                                mq6 = dt1.Rows[i]["UNIT"].ToString().Trim();
                                if (mq6.Trim().ToUpper().Contains("NOS"))
                                {
                                    dr1["UOM"] = "NAR";//103==========P
                                }
                                else if (mq6.Trim().ToUpper().Contains("EACH"))
                                {
                                    dr1["UOM"] = "EA";//103==========P
                                }
                                else if (mq6.Trim().ToUpper().Contains("PIECE"))
                                {
                                    dr1["UOM"] = "PCE";//103==========P
                                }
                                else if (mq6.Trim().ToUpper().Contains("UNIT"))
                                {
                                    dr1["UOM"] = "UN";//103==========P
                                }
                                dr1["Unit_Price"] = fgen.make_double(dt1.Rows[i]["IRATE"].ToString().Trim());//104
                                dr1["Line_Net_Amt"] = fgen.make_double(dt1.Rows[i]["BASIC"].ToString().Trim());   //105                                                           
                                dr1["Supp_Part_Num"] = dt1.Rows[i]["HSN_CODE"].ToString().Trim();//106===============p
                                dr1["Supp_Part_Desc"] = dt1.Rows[i]["PART_NAME"].ToString().Trim();//107===============p
                                dr1["Buyer_Part_Num"] = dt1.Rows[i]["buyer_part_no"].ToString().Trim();//108===============p  
                                dr1["Mfg_Part_Num"] = "";//109
                                dr1["Commodity_Code"] = "";//110
                                dr1["TaX_Descriptor"] = "";//111
                                if (mq5 == "CG")
                                {
                                    dr1["Tax_Catg_code"] = "IN11";//112===============p
                                    dr1["Tax_Rate1"] = fgen.make_double(dt1.Rows[i]["CGST_RATE"].ToString().Trim());
                                    dr1["Tax_Amt1"] = fgen.make_double(dt1.Rows[i]["CGST_AMT"].ToString().Trim());//114===============p                                
                                    dr1["Tax_Catg_Code2"] = "IN12";//115===============p
                                    dr1["Tax_Rate2"] = fgen.make_double(dt1.Rows[i]["SGST_RATE"].ToString().Trim()); ;//116===============p                               
                                    dr1["Tax_Amt2"] = fgen.make_double(dt1.Rows[i]["SGST_AMT"].ToString().Trim()); ;//117===============p 
                                    dr1["Tax_Catg_Code3"] = "";
                                    dr1["Tax_Rate3"] = fgen.make_double(dt1.Rows[i]["IGST_RATE"].ToString().Trim()); ;//119===============p
                                    dr1["Tax_Amt3"] = fgen.make_double(dt1.Rows[i]["IGST_AMT"].ToString().Trim()); ;//120....line taxamt3===============p  
                                }
                                else
                                {
                                    dr1["Tax_Catg_code"] = "";//112===============p
                                    dr1["Tax_Rate1"] = fgen.make_double(dt1.Rows[i]["CGST_RATE"].ToString().Trim());
                                    dr1["Tax_Amt1"] = fgen.make_double(dt1.Rows[i]["CGST_AMT"].ToString().Trim());//114===============p                                
                                    dr1["Tax_Catg_Code2"] = "";//115===============p
                                    dr1["Tax_Rate2"] = fgen.make_double(dt1.Rows[i]["SGST_RATE"].ToString().Trim()); ;//116===============p                               
                                    dr1["Tax_Amt2"] = fgen.make_double(dt1.Rows[i]["SGST_AMT"].ToString().Trim()); ;//117===============p 
                                    dr1["Tax_Catg_Code3"] = "IN14";//118===============p
                                    dr1["Tax_Rate3"] = fgen.make_double(dt1.Rows[i]["IGST_RATE"].ToString().Trim()); ;//119===============p
                                    dr1["Tax_Amt3"] = fgen.make_double(dt1.Rows[i]["IGST_AMT"].ToString().Trim()); ;//120....line taxamt3===============p    
                                }
                                dr1["Tax_catg_code4"] = "";//121
                                dr1["Tax_Rate4"] = "";//122
                                dr1["Tax_Amt4"] = "";//123
                                dr1["Shipment_No"] = "";//124
                                dr1["Buyer_Cost_Center"] = "";//125
                                dr1["Bill_of_Loading"] = "";//126
                                dr1["Contract_ID"] = "";//127
                                dr1["Dun_and_Bradstreet_No"] = "";//128
                                dr1["INCO_terms"] = "";//129
                                dr1["WBS"] = "";//130
                                dr1["Nature_of_Tax"] = "";//131
                                dr1["Place_of_Issue"] = "";//132
                                dr1["GL_No"] = "";//133
                                dr1["Account_No"] = "";//134
                                dr1["Utility_ID"] = "";//135
                                dr1["ESR_Cust_Ref"] = "";//136
                                dr1["ESR_Cust_Num"] = "";//137
                                dr1["Weight"] = "";//138
                                dr1["Net_Weight"] = "";//139
                                dr1["Gross_Weight"] = "";//140
                                dr1["No_of_pkg"] = dt1.Rows[i]["no_of_pkg"].ToString().Trim().Replace(",", " ");//141===============p. 
                                dr1["Mode_of_Tpt"] = dt1.Rows[i]["mode_tpt"].ToString().Trim().Replace(",", " ");//142===============p...truck no need to ask
                                dr1["Exp_time_of_Arrival"] = "";//143
                                dr1["Port_of_Loading"] = "";//144
                                dr1["Port_of_Discharge"] = "";//145
                                dr1["Chg_Catg"] = "";//146
                                dr1["Withhold_Tax_Indicator"] = "";//147
                                dr1["License_Number"] = "";//148
                                dr1["Custom_Declaration_No"] = "";//149
                                dr1["Custom_Office"] = "";//150
                                dr1["Country_of_Origin"] = "";//151
                                dr1["Rail_Truck_No"] = dt1.Rows[i]["vehicle_no"].ToString().Trim().Replace(",", " ");//152===============p
                                dr1["Batch_No"] = "";//153
                                dr1["Batch_Qty"] = "";//154
                                dr1["Goods_or_Service_Indicator"] = "";//155
                                dr1["Month"] = "";//156
                                dr1["Week"] = "";//157
                                dr1["Hrs"] = "";//158
                                dr1["Name"] = "";//159
                                dr1["Location_Code"] = "";//160
                                dr1["Approver_Code"] = "";//161
                                dr1["Serail_No"] = "";//162
                                dr1["Buyer_Affiliate"] = "";//163
                                dr1["Export_No"] = "";//164
                                dr1["Ticket_No"] = "";//165
                                dr1["Tax_Regime"] = "";//166
                                dr1["Booking_Ref"] = "";//167
                                dr1["Delv_start_dt"] = "";//168
                                dr1["Delv_End_date"] = "";//169
                                dr1["Disc_Description"] = "";//170
                                dr1["Disc_Amt"] = "";//171
                                dr1["Disc_Tax_Catg1"] = "";//172
                                dr1["Disc_Tax_Rate1"] = "";//173
                                dr1["Disc_Tax_Amt1"] = "";//174
                                dr1["Disc_Tax_Catg2"] = "";//175
                                dr1["Disc_Tax_Rate2"] = "";//176
                                dr1["Disc_Tax_Amt2"] = "";//177
                                dr1["Disc_Tax_Catg3"] = "";//178
                                dr1["Disc_Tax_Rate3"] = "";//179
                                dr1["Disc_Tax_Amt3"] = "";//180
                                dr1["Disc_Tax_Catg4"] = "";//181
                                dr1["Disc_Tax_Rate4"] = "";//182
                                dr1["Disc_Tax_Amt4"] = "";//183
                                dr1["Special_Chg_Des"] = "";//184
                                dr1["Special_Chg_Amt"] = "";//185
                                dr1["Special_Chg_TaxCatg1"] = "";//186
                                dr1["Special_Chg_TaxRate1"] = "";//187
                                dr1["Special_Chg_TaxAmt1"] = "";//188
                                dr1["Special_Chg_TaxCatg2"] = "";//189
                                dr1["Special_Chg_TaxRate2"] = "";//190
                                dr1["Special_Chg_TaxAmt2"] = "";//191
                                dr1["Special_Chg_TaxCatg3"] = "";//192
                                dr1["Special_Chg_TaxRate3"] = "";//193
                                dr1["Special_Chg_TaxAmt3"] = "";//194
                                dr1["Special_Chg_TaxCatg4"] = "";//195
                                dr1["Special_Chg_TaxRate4"] = "";//196
                                dr1["Special_Chg_TaxAmt4"] = "";//197
                                dr1["Carriage_Desc"] = "";//198
                                dr1["Carriage_Amt"] = "";//199
                                dr1["Carriage_Tax_Catg1"] = "";//200
                                dr1["Carriage_Tax_Rate1"] = "";//201
                                dr1["Carriage_Tax_Amt1"] = "";//202
                                dr1["Carriage_Tax_Catg2"] = "";//203
                                dr1["Carriage_Tax_Rate2"] = "";//204
                                dr1["Carriage_Tax_Amt2"] = "";//205
                                dr1["Carriage_Tax_Catg3"] = "";//206
                                dr1["Carriage_Tax_Rate3"] = "";//207
                                dr1["Carriage_Tax_Amt3"] = "";//208
                                dr1["Carriage_Tax_Catg4"] = "";//209
                                dr1["Carriage_Tax_Rate4"] = "";//210
                                dr1["Carriage_Tax_Amt4"] = "";//211
                                //feight will coming from dt4...only 59 icode
                                if (i == 0)
                                {
                                    if (dt5 == null) { }
                                    else
                                    {
                                        k = i + 2;
                                        dr1["Frieght_Des"] = fgen.seek_iname_dt(dt5, "morder='" + k + "'", "PART_NAME");//212                                        
                                        for (int j = 0; j < dt5.Rows.Count; j++)
                                        {
                                            db16 = fgen.make_double(dt5.Rows[j]["CGST_RATE"].ToString().Trim());
                                            db17 += fgen.make_double(dt5.Rows[j]["CGST_AMT"].ToString().Trim());
                                            db18 = fgen.make_double(dt5.Rows[j]["SGST_RATE"].ToString().Trim());
                                            db19 += fgen.make_double(dt5.Rows[j]["SGST_AMT"].ToString().Trim());
                                            db20 = fgen.make_double(dt5.Rows[j]["IGST_RATE"].ToString().Trim());
                                            db21 += fgen.make_double(dt5.Rows[j]["IGST_amt"].ToString().Trim());
                                            db22 += fgen.make_double(dt5.Rows[j]["basic"].ToString().Trim());
                                        }
                                    }
                                    if (mq5 == "CG")
                                    {
                                        dr1["Frieght_Amt"] = Convert.ToString(db22);//213   fryt_amt...iamount
                                        dr1["Freight_Tax_Catg1"] = "IN11";//214
                                        dr1["Freight_Tax_Rate1"] = Convert.ToString(db16);//215
                                        dr1["Freight_Tax_Amt1"] = Convert.ToString(db17);//216
                                        dr1["Freight_Tax_Catg2"] = "IN12";//217
                                        dr1["Freight_Tax_Rate2"] = Convert.ToString(db18);//218
                                        dr1["Freight_Tax_Amt2"] = Convert.ToString(db19);//219
                                        dr1["Freight_Tax_Catg3"] = "";//220
                                        dr1["Freight_Tax_Rate3"] = "";//221
                                        dr1["Freight_Tax_Amt3"] = "";//222
                                    }
                                    else
                                    {
                                        dr1["Frieght_Amt"] = Convert.ToString(db22);//213   fryt_amt...iamount
                                        dr1["Freight_Tax_Catg1"] = "";//214
                                        dr1["Freight_Tax_Rate1"] = "";//215
                                        dr1["Freight_Tax_Amt1"] = "";//216
                                        dr1["Freight_Tax_Catg2"] = "";//217
                                        dr1["Freight_Tax_Rate2"] = "";//218
                                        dr1["Freight_Tax_Amt2"] = "";//219
                                        dr1["Freight_Tax_Catg3"] = "IN14";//220
                                        dr1["Freight_Tax_Rate3"] = Convert.ToString(db20); ;//221
                                        dr1["Freight_Tax_Amt3"] = Convert.ToString(db21); ;//222
                                    }
                                }
                                else
                                {
                                    k = k + 2;
                                }
                                dr1["Freight_Tax_Catg4"] = "";//223
                                dr1["Freight_Tax_Rate4"] = "";//224
                                dr1["Freight_Tax_Amt4"] = "";//225

                                dr1["Insur_Desc"] = "";//226
                                dr1["Insur_Amt"] = "";//227
                                dr1["Insur_Tax_Catg1"] = "";//228
                                dr1["Insur_Tax_Rate1"] = "";//229
                                dr1["Insur_Tax_Amt1"] = "";//230
                                dr1["Insur_Tax_Catg2"] = "";//231
                                dr1["Insur_Tax_Rate2"] = "";//232
                                dr1["Insur_Tax_Amt2"] = "";//233
                                dr1["Insur_Tax_Catg3"] = "";//234
                                dr1["Insur_Tax_Rate3"] = "";//235
                                dr1["Insur_Tax_Amt3"] = "";//236
                                dr1["Insur_Tax_Catg4"] = "";//237
                                dr1["Insur_Tax_Rate4"] = "";//238
                                dr1["Insur_Tax_Amt4"] = "";//239
                                dr1["Pack_Desc"] = "";//240
                                dr1["Pack_Amt"] = "";//241
                                dr1["Pack_Tax_Catg1"] = "";//242
                                dr1["Pack_Tax_Rate1"] = "";//243
                                dr1["Pack_Tax_Amt1"] = "";//244
                                dr1["Pack_Tax_Catg2"] = "";//245
                                dr1["Pack_Tax_Rate2"] = "";//246
                                dr1["Pack_Tax_Amt2"] = "";//247
                                dr1["Pack_Tax_Catg3"] = "";//248
                                dr1["Pack_Tax_Rate3"] = "";//249
                                dr1["Pack_Tax_Amt3"] = "";//250
                                dr1["Pack_Tax_Catg4"] = "";//251
                                dr1["Pack_Tax_Rate4"] = "";//252
                                dr1["Pack_Tax_Amt4"] = "";//253
                                dr1["Admin_Chg_Desc"] = "";//254
                                dr1["Admin_Chg_Amt"] = "";//255
                                dr1["Admin_Chg_Tax_Catg1"] = "";//256
                                dr1["Admin_Chg_Tax_Rate1"] = "";//257
                                dr1["Admin_Chg_Tax_Amt1"] = "";//258
                                dr1["Admin_Chg_Tax_Catg2"] = "";//259
                                dr1["Admin_Chg_Tax_Rate2"] = "";//260
                                dr1["Admin_Chg_Tax_Amt2"] = "";//261
                                dr1["Admin_Chg_Tax_Catg3"] = "";//262
                                dr1["Admin_Chg_Tax_Rate3"] = "";//263
                                dr1["Admin_Chg_Tax_Amt3"] = "";//264
                                dr1["Admin_Chg_Tax_Catg4"] = "";//265
                                dr1["Admin_Chg_Tax_Rate4"] = "";//266
                                dr1["Admin_Chg_Tax_Amt4"] = "";//267
                                dr1["Fuel_Surcharge_Desc"] = "";//268
                                dr1["Fuel_Surcharge_Amt"] = "";//269
                                dr1["Fuel_Surcharge_TaxCatg1"] = "";//270
                                dr1["Fuel_Surcharge_TaxRate1"] = "";//271
                                dr1["Fuel_Surcharge_TaxAmt1"] = "";//272
                                dr1["Fuel_Surcharge_TaxCatg2"] = "";//273
                                dr1["Fuel_Surcharge_TaxRate2"] = "";//274
                                dr1["Fuel_Surcharge_TaxAmt2"] = "";//275
                                dr1["Fuel_Surcharge_TaxCatg3"] = "";//276
                                dr1["Fuel_Surcharge_TaxRate3"] = "";//277
                                dr1["Fuel_Surcharge_TaxAmt3"] = "";//278
                                dr1["Fuel_Surcharge_TaxCatg4"] = "";//279
                                dr1["Fuel_Surcharge_TaxRate4"] = "";//280
                                dr1["Fuel_Surcharge_TaxAmt4"] = "";//281
                                dr1["Green_Tax_Desc"] = "";//282
                                dr1["Green_Tax_Amt"] = "";//283
                                dr1["Green_Tax_Catg1"] = "";//284
                                dr1["Green_Tax_Rate1"] = "";//285
                                dr1["Green_Tax_Amt1"] = "";//286
                                dr1["Green_Tax_Catg2"] = "";//287
                                dr1["Green_Tax_Rate2"] = "";//288
                                dr1["Green_Tax_Amt2"] = "";//289
                                dr1["Green_Tax_Catg3"] = "";//290
                                dr1["Green_Tax_Rate3"] = "";//291
                                dr1["Green_Tax_Amt3"] = "";//292
                                dr1["Green_Tax_Catg4"] = "";//293
                                dr1["Green_Tax_Rate4"] = "";//294
                                dr1["Green_Tax_Amt4"] = "";//295

                                if (i == 0)
                                {
                                    dr1["Rounding_Line_Desc"] = "Round Off";//296..p
                                    if (db11 == 0)
                                    {
                                        dr1["Rounding_Line_Amt"] = "";//297...p
                                    }
                                    else
                                    {
                                        dr1["Rounding_Line_Amt"] = Convert.ToString(db11);//297...p
                                    }
                                    dr1["Rounding_Line_Taxcatg1"] = "IN7";//298
                                    dr1["Rounding_Line_TaxRate1"] = "0";//299
                                    dr1["Rounding_Line_TaxAmt1"] = "0";//300
                                }
                                else
                                {
                                    dr1["Rounding_Line_Desc"] = "";
                                    dr1["Rounding_Line_Amt"] = "";
                                    dr1["Rounding_Line_Taxcatg1"] = "";//298
                                    dr1["Rounding_Line_TaxRate1"] = "";//299
                                    dr1["Rounding_Line_TaxAmt1"] = "";//300
                                }

                                dr1["Rounding_Line_Taxcatg2"] = "";//301
                                dr1["Rounding_Line_TaxRate2"] = "";//302
                                dr1["Rounding_Line_TaxAmt2"] = "";//303
                                dr1["Rounding_Line_Taxcatg3"] = "";//304
                                dr1["Rounding_Line_TaxRate3"] = "";//305
                                dr1["Rounding_Line_TaxAmt3"] = "";//306
                                dr1["Rounding_Line_Taxcatg4"] = "";//307
                                dr1["Rounding_Line_TaxRate4"] = "";//308
                                dr1["Rounding_Line_TaxAmt4"] = "";//309
                                dr1["Demurrage"] = "";//310
                                dr1["Demurrage_Amt"] = "";//311
                                dr1["Demurrage_TaxCatg1"] = "";//312
                                dr1["Demurrage_TaxRate1"] = "";//313
                                dr1["Demurrage_TaxAmt1"] = "";//314
                                dr1["Demurrage_TaxCatg2"] = "";//315
                                dr1["Demurrage_TaxRate2"] = "";//316
                                dr1["Demurrage_TaxAmt2"] = "";//317
                                dr1["Demurrage_TaxCatg3"] = "";//318
                                dr1["Demurrage_TaxRate3"] = "";//319
                                dr1["Demurrage_TaxAmt3"] = "";//320
                                dr1["Demurrage_TaxCatg4"] = "";//321
                                dr1["Demurrage_TaxRate4"] = "";//322
                                dr1["Demurrage_TaxAmt4"] = "";//323
                                dr1["Adv_Recycle_FeeDesc"] = "";//324
                                dr1["Adv_Recycle_FeeAmt"] = "";//325
                                dr1["Adv_Recycle_Fee_Taxcatg1"] = "";//326
                                dr1["Adv_Recycle_Fee_TaxRate1"] = "";//327
                                dr1["Adv_Recycle_Fee_TaxAmt1"] = "";//328
                                dr1["Adv_Recycle_Fee_Taxcatg2"] = "";//329
                                dr1["Adv_Recycle_Fee_TaxRate2"] = "";//330
                                dr1["Adv_Recycle_Fee_TaxAmt2"] = "";//331
                                dr1["Adv_Recycle_Fee_Taxcatg3"] = "";//332
                                dr1["Adv_Recycle_Fee_TaxRate3"] = "";//333
                                dr1["Adv_Recycle_Fee_TaxAmt3"] = "";//334
                                dr1["Adv_Recycle_Fee_Taxcatg4"] = "";//335
                                dr1["Adv_Recycle_Fee_TaxRate4"] = "";//336
                                dr1["Adv_Recycle_Fee_TaxAmt4"] = "";//337
                                dr1["Invoie_Dtl1"] = "";//338
                                dr1["Invoie_Dtl2"] = "";//339
                                dr1["Invoie_Dtl3"] = "";//340
                                dr1["Disc_Per_Line"] = "";//341
                                dr1["Disc_Per_Amt"] = "";//342
                                dr1["Supp_Id"] = "";//343
                                dr1["Inv_From_TaxRegNo"] = "";//344
                                dr1["Inv_To_TaxRegNo"] = "";//345
                                dr1["Third_Prty_delv_TicketNo"] = "";//346
                                dr1["Delv_TaxReg_No"] = "";//347
                                dr1["Endordement"] = "";//348
                                dr1["Input_Tax_Credit"] = "";//349
                                dr1["Payble_Tax_On_Rev_Chgs"] = "";//350
                                dr1["ISD_no"] = "";//351
                                dr1["Adv_Pymt_Amt"] = "";//352
                                dr1["Party_No"] = "";//353
                                dr1["PEC_Email"] = "";//354
                                dr1["Natural_Person_Name"] = "";//355
                                dr1["Natural_Person_SrName"] = "";//356
                                dr1["Fiscal_code_Natural_Person"] = "";//357
                                dr1["Supp_ord_No"] = "";//358
                                dr1["Remit_to_name"] = "";//359
                                dr1["Remit_To_Street1"] = "";//360
                                dr1["Remit_To_Street2"] = "";//361
                                dr1["Remit_To_city"] = "";//362
                                dr1["Remit_To_state"] = "";//363
                                dr1["Remit_To_Postal_Code"] = "";//364
                                dr1["Remit_To_Country"] = "";//365
                                dr1["Alt_Ref"] = "";//366
                                dr1["Campaign_Name"] = "";//367
                                dr1["Campaign_Id"] = "";//368
                                dr1["Media_Type"] = "";//369
                                dr1["Invoice_Period"] = "";//370
                                dr1["Advertiser_Name"] = "";//371
                                dr1["Advertiser_Brand"] = "";//373
                                dr1["Supp_PAN_no"] = "";//372
                                ph_tbl.Rows.Add(dr1);
                                #endregion
                            }
                            #region new
                            fileName = dt.Rows[0]["customer_Code"].ToString().Trim() + "_" + dr0["inv_no"].ToString().Trim() + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
                            filepath = @"c:\TEJ_ERP\UPLOAD\" + fileName;///testing
                            Session["send_dt"] = ph_tbl;
                            mq7 = @"c:\TEJ_ERP\UPLOAD\";
                            if (!Directory.Exists(mq7)) Directory.CreateDirectory(mq7);
                            fileName = dt.Rows[0]["customer_Code"].ToString().Trim() + "_" + dr0["inv_no"].ToString().Trim();
                            filepath = @"c:\TEJ_ERP\UPLOAD\" + fileName + ".csv";
                            CreateCSVFile(ph_tbl, filepath, "|");//uncmnt this line                                           
                            zipFilePath += "," + filepath;
                            zipFileName += "," + fileName;
                            ph_tbl.Clear();
                            #endregion
                        }
                        zipFilePath = zipFilePath.TrimStart(',');
                        zipFileName = zipFileName.TrimStart(',');
                        Session["FilePath"] = zipFilePath;
                        Session["FileName"] = "Tungston Report";
                        Response.Write("<script>");
                        Response.Write("window.open('../tej-base/makeZipDwnload.aspx','_blank')");
                        Response.Write("</script>");
                        ded1 = @"c:\TEJ_ERP\UPLOAD";
                        fgen.msg("-", "AMSG", "File has been downloaded");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please Select Invoices !! '13' You Want to See..");
                    }
                    #endregion
                    break;


                #region Studds Reportn qty based
                case "F50054":
                    /////////////after selecting item
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    #region maingrp case
                    if (hfHead.Value == "MAINGRP")
                    {
                        SQuery = "SELECT  FSTR as Acode,PARTY,qty,round((QTY/total*100),2) as sob FROM (  SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '0%'  AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,2)) in ('" + hfSales.Value + "') ) as total FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND TRIM(SUBSTR(A.ICODE,1,2))  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME)";
                        SQuery = "SELECT  FSTR,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%'  AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,2)) in ('" + hfSales.Value + "') AND STORE!='R' ) as total FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND STORE!='R' AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND TRIM(SUBSTR(A.ICODE,1,2)) IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME)";
                        SQuery = "SELECT fstr,icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%'  AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,2)) in ('" + hfSales.Value + "') AND STORE!='R' and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') ) as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.vchdate " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(A.ACODE),0,2) IN ('05','06') AND TRIM(SUBSTR(A.ICODE,1,2)) IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        SQuery = "SELECT fstr,icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN+NVL(A.REJ_RW,0)) AS QTY,SUM(IQTYIN+NVL(REJ_RW,0)) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%'  AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,2)) in ('" + hfSales.Value + "') AND STORE!='R' and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') ) as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.vchdate " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(A.ACODE),0,2) IN ('05','06') AND TRIM(SUBSTR(A.ICODE,1,2)) IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        SQuery = "SELECT fstr,icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN+NVL(A.REJ_RW,0)) AS QTY,(SELECT SUM(IQTYIN+NVL(REJ_RW,0)) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%'  AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,2)) in ('" + hfSales.Value + "') and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') AND STORE!='R'  ) as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.vchdate " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(A.ACODE),0,2) IN ('05','06') AND TRIM(SUBSTR(A.ICODE,1,2)) IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            oporow = null;
                            oporow = dt.NewRow();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 6)
                                {
                                    oporow[1] = "Total";
                                }
                                else
                                {
                                    double mysum = 0;
                                    foreach (DataRow drc in dt.Rows)
                                    {
                                        mysum += fgen.make_double(drc[dc].ToString());
                                        oporow[dc] = mysum;
                                    }
                                }
                                double total1 = fgen.make_double(dt.Rows[dt.Rows.Count - 1]["QTY"].ToString());
                            }
                            dt.Rows.Add(oporow);
                            Session["send_dt"] = dt;
                            fgen.Fn_open_rptlevelJS("Group Level Report from " + fromdt + " To '" + todt + "", frm_qstr);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Found");
                        }
                    }
                    #endregion
                    //--------------------------------------------
                    #region sub group level
                    if (hfHead.Value == "SUBGRP")
                    {
                        SQuery = "SELECT  FSTR as Acode,PARTY,qty,round((QTY/total*100),2) as sob FROM  (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,4)) in ('" + hfSales.Value + "') ) as total FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND TRIM(SUBSTR(A.ICODE,1,4))  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME)";
                        SQuery = "SELECT  FSTR,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,4)) in ('" + hfSales.Value + "') AND STORE!='R') as total FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND STORE!='R' AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND TRIM(SUBSTR(A.ICODE,1,4))  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME)";
                        SQuery = "SELECT  FSTR,icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,4)) in ('" + hfSales.Value + "') and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') AND STORE!='R') as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(a.ACODE),0,2) IN ('05','06') AND TRIM(SUBSTR(A.ICODE,1,4))  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        SQuery = "SELECT  FSTR,icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN+NVL(A.REJ_RW,0)) AS QTY,(SELECT SUM(IQTYIN+NVL(REJ_RW,0)) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,4)) in ('" + hfSales.Value + "') and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') AND STORE!='R') as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(a.ACODE),0,2) IN ('05','06') AND TRIM(SUBSTR(A.ICODE,1,4))  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            oporow = null;
                            oporow = dt.NewRow();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 6)
                                {
                                    oporow[1] = "Total";
                                }
                                else
                                {
                                    double mysum = 0;
                                    foreach (DataRow drc in dt.Rows)
                                    {
                                        mysum += fgen.make_double(drc[dc].ToString());
                                        oporow[dc] = mysum;
                                    }
                                }
                                double total1 = fgen.make_double(dt.Rows[dt.Rows.Count - 1]["QTY"].ToString());
                            }
                            dt.Rows.Add(oporow);
                            Session["send_dt"] = dt;
                            fgen.Fn_open_rptlevelJS("Sub Group Level Report From " + fromdt + " To " + todt + "", frm_qstr);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Found");
                        }
                    }
                    #endregion
                    //--------------------------------------------
                    #region ITEM LEVEL
                    if (hfHead.Value == "ITEM")
                    {
                        SQuery = "SELECT FSTR as Acode,ANAME AS PARTY,QTY,ROUND((QTY/TOTAL*100),2) as SOB FROM (SELECT A.ACODE AS FSTR,B.ANAME,SUM(A.IQTYIN) AS QTY,a.icode,(SELECT SUM(IQTYIN) AS TOTAL FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(ICODE)='" + hfSales.Value + "') AS TOTAL  FROM  IVOUCHER A,FAMST B  WHERE A.ICODE='" + hfSales.Value + "' AND TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '0%' and A.VCHDATE " + xprdrange + "  GROUP BY B.ANAME,A.ACODE,a.icode)";
                        SQuery = "SELECT FSTR,ANAME AS PARTY,QTY,ROUND((QTY/TOTAL*100),2) as SOB FROM (SELECT A.ACODE AS FSTR,B.ANAME,SUM(A.IQTYIN) AS QTY,a.icode,(SELECT SUM(IQTYIN) AS TOTAL FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(ICODE) in ('" + hfSales.Value + "') AND STORE!='R') AS TOTAL  FROM  IVOUCHER A,FAMST B WHERE A.ICODE in ('" + hfSales.Value + "') AND TRIM(A.ACODE)=TRIM(B.ACODE) AND STORE!='R'   AND A." + branch_Cd + " AND A.TYPE LIKE '0%' and A.VCHDATE " + xprdrange + "  GROUP BY B.ANAME,A.ACODE,a.icode)";
                        SQuery = "SELECT  FSTR,icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(icode) in ('" + hfSales.Value + "') and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') AND STORE!='R') as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(a.ACODE),0,2) IN ('05','06') AND TRIM(A.ICODE)  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        SQuery = "SELECT FSTR,icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN+NVL(A.REJ_RW,0)) AS QTY,(SELECT SUM(IQTYIN+NVL(REJ_RW,0)) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(icode) in ('" + hfSales.Value + "') and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') AND STORE!='R') as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(a.ACODE),0,2) IN ('05','06') AND TRIM(A.ICODE)  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            oporow = null;
                            oporow = dt.NewRow();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 6)
                                {
                                    oporow[1] = "Total";
                                }
                                else
                                {
                                    double mysum = 0;
                                    foreach (DataRow drc in dt.Rows)
                                    {
                                        mysum += fgen.make_double(drc[dc].ToString());
                                        oporow[dc] = mysum;
                                    }
                                }
                                double total1 = fgen.make_double(dt.Rows[dt.Rows.Count - 1]["QTY"].ToString());
                            }
                            dt.Rows.Add(oporow);
                            Session["seekSql"] = "";
                            fgen.send_cookie("seekSql", "");
                            Session["send_dt"] = dt;
                            fgen.Fn_open_rptlevelJS("Item Level Report From " + fromdt + " To " + todt + "", frm_qstr);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Found");
                        }
                    }
                    #endregion
                    break;
                #endregion
            }
        }
    }
    public void CreateCSVFile(DataTable dtDataTablesList, string strFilePath, string spratr)
    {
        if (File.Exists(strFilePath)) File.Delete(strFilePath);

        StreamWriter sw = new StreamWriter(strFilePath, false);

        //First we will write the headers.

        int iColCount = dtDataTablesList.Columns.Count;
        if (frm_cocd != "VELV" && frm_formID == "F50330")////PLEASE PUT CONDITON IN THIS FUN ON FGEN PAGE FOR VELVIN//in velv no need to show header
        {
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dtDataTablesList.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(spratr);
                }
            }
            sw.Write(sw.NewLine);
        }
        // Now write all the rows.

        foreach (DataRow dr in dtDataTablesList.Rows)
        {
            for (int i = 0; i < iColCount; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write(dr[i].ToString());
                }
                if (i < iColCount - 1)
                {
                    sw.Write(spratr);
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();
    }
}