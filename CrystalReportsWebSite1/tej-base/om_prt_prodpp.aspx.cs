using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_prodpp : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2, WB_TABNAME;

    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
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
                col1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT BRN||'~'||PRD AS PP FROM fin_msys WHERE UPPER(TRIM(ID))='" + frm_formID + "'", "PP");
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

                case "22610A":
                case "22610B":
                    fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
                    break;

                //MADE BY AKSHAY...MERGED BY YOGITA
                case "F40132": //Daily Prodn Report(PP)
                    SQuery = "select mthnum as fstr,mthnum as code,mthname as month from mths";
                    header_n = "Select Month";
                    break;

                case "F40145":
                    if (co_cd == "SPIR" || co_cd == "STLC")
                    {
                        WB_TABNAME = "prod_sheetk";
                        mq7 = "86";
                    }
                    else
                    {
                        WB_TABNAME = "prod_sheet";
                        mq7 = "88";
                    }
                    SQuery = "SELECT  DISTINCT TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, TRIM(B.INAME) AS INAME ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BATCH_DT, A.VCHNUM AS BATCH_NO,C.MCHNAME,A.A1 AS BATCH_QTY,A.PREVCODE AS SHIFT,A.JOB_NO,A.JOB_DT  FROM " + WB_TABNAME + " A,ITEM B ,PMAINT C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND C.SRNO='1' AND  TRIM(A.MCHCODE)=TRIM(C.ACODE)||'/'||TRIM(C.SRNO) AND a.branchcd='" + mbr + "' and  A.TYPE='" + mq7 + "' AND A.VCHDATE " + xprdrange + "  ORDER BY A.VCHNUM DESC";
                    header_n = "Select Entry";
                    //SQuery = "SELECT  DISTINCT TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, TRIM(B.INAME) AS INAME ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BATCH_DT, A.VCHNUM AS BATCH_NO,C.MCHNAME,A.A1 AS BATCH_QTY,A.PREVCODE AS SHIFT,A.JOB_NO,A.JOB_DT  FROM PROD_SHEET A,ITEM B ,PMAINT C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND C.SRNO='1' AND  TRIM(A.MCHCODE)=TRIM(C.ACODE)||'/'||TRIM(C.SRNO) AND a.branchcd='" + mbr + "' and  A.TYPE='88' AND A.VCHDATE " + xprdrange + "  ORDER BY A.VCHNUM DESC";   
                    break;

                ///PRODUCTION SLIP
                case "F40140":
                case "F40141":
                case "F40139":
                    SQuery = "SELECT TYPE1 AS FSTR,TYPE1 AS DOCUMENT_CODE,NAME AS DOCUMENT_NAME  FROM TYPE WHERE ID='M' AND TYPE1>'14' AND TYPE1<'20' ORDER BY TYPE1";
                    header_n = "Select Document Type";
                    break;

                case "F40146":
                    SQuery = "SELECT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,trim(VCHNUM) as vchnum,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TYPE FROM COSTESTIMATE WHERE BRANCHCD='" + mbr + "' AND TYPE='60' AND VCHDATE " + xprdrange + " AND SRNO='1'  ORDER BY VCHNUM  DESC";    //AND C.SRNO='1' 
                    header_n = "Select Type";
                    break;

                case "15250H": // wfinsys_erp id  // ABOX REPORT
                case "F40309":
                    SQuery = "select trim(icode) as fstr ,trim(icode) as ERP_Code,trim(iname) as Item_name from item where length(trim(icode))='8'";
                    header_n = "Select Item for Detail";
                    break;

                case "WORK_DIRECT_DIRECT": // JOB WISE PRODUCTION FORM DIRECT PRINT if have to make prin report
                    SQuery = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')  AS FSTR ,A.VCHNUM,TO_CHAR(A.VCHDATE) AS VCHDATE,B.NAME AS NAME, A.ENAME,A.PREVCODE,A.TYPE,COUNT(A.ICODE) AS ITEM FROM PROD_SHEET A ,TYPE B  WHERE TRIM(A.STAGE)=TRIM(B.TYPE1) AND  TYPE='90'  AND B.ID='K' GROUP BY B.NAME, A.VCHNUM, A.VCHDATE,A.ENAME,A.PREVCODE,A.TYPE,A.BRANCHCD ORDER BY A.VCHDATE DESC";
                    header_n = "Select Entry";
                    break;

                case "F40344":
                    SQuery = "SELECT DISTINCT branchcd||type||trim(VCHNUM)||to_char(vchdate,'dd/mm/yyyy') as fstr ,vchnum , to_char(VCHDATE,'dd/mm/yyyy') as vchdate , TYPE  FROM PROD_SHEET WHERE BRANCHCD='" + mbr + "' AND TYPE ='10' AND VCHDATE " + xprdrange + " ORDER BY VCHNUM DESC";
                    header_n = "Select Entry";
                    break;

                case "F40345":
                    SQuery = "SELECT DISTINCT branchd||type||trim(VCHNUM),to_char(vchdate,'dd/mm/yyyy') as fstr ,vchnum , to_char(VCHDATE,'dd/mm/yyyy') as vchdate , TYPE  FROM MTHLYPLAN WHERE BRANCHCD='" + mbr + "' AND TYPE ='10' AND VCHDATE " + xprdrange + " ORDER BY VCHNUM DESC";
                    header_n = "Select Entry";
                    break;

                  ///accrapack sticker
                case "STKR":
                case "F35242":
                case "F40215":
                    SQuery = "select  trim(branchcd)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode) as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate from costestimate where branchcd='" + mbr + "' and type='40' and VCHDATE " + xprdrange + " order by fstr desc";
                    header_n = "Select Sticker";                
                    break;

                case "F40217": 
                case "F40219":
                    SQuery = "select mthnum as fstr,mthnum as code,mthname as month from mths";
                    header_n = "Select Month";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F40215")
                {
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                }
                else
                {
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15133" || val == "F40309" || val == "WORK_ORDER_DIRECT")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                { //need icon for this on test

                    case "STKR":
                    case "F35242":
                    case "F40215":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1); //FSTR                      
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_prodpp_reps(frm_qstr);
                        break;

                    case "F40145":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_prodpp_reps(frm_qstr);
                        break;

                    case "F40140":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1; //select type
                            SQuery = "select distinct trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'DD/MM/YYYY') AS FSTR,VCHNUM AS DOCUMENT,TO_CHAR(vchdate,'dd/mm/yyyy') as dated,ponum,to_char(podate,'dd/mm/yyyy') as podate  from ivoucher where branchcd='" + mbr + "' and type='" + hfval.Value + "' and vchdate " + xprdrange + " order by vchnum desc";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);//for grade                           
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40140");
                            fgen.fin_prodpp_reps(frm_qstr);
                        }
                        break;

                    case "F40141":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40141");
                        //fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F40146":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40146");
                        fgen.fin_prodpp_reps(frm_qstr);
                        // fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;

                    case "F40139":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40139");
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;

                    case "F40344":
                    case "F40345":
                        hfcode.Value = value1;
                        hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_prodpp_reps(frm_qstr);
                        break;

                    case "F40219":
                    case "F40217": 
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_prodpp_reps(frm_qstr);
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
                case "F40133":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40133");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "F40132":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40132");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                /////////5 APRIL TASK                                

                case "F40141":
                case "F40142":
                case "F40143":
                case "F40148":
                case "F40149":
                case "F40150":
                    //details of item rejected...nedd icon for this
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                ///TODAY
                case "F40139":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40139");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "15250H":// wfinsys_erp id  // ABOX REPORT
                case "F40309":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40309");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                default:
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "WORK_ORDER_DIRECT":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "WORK_ORDER_DIRECT");
                    fgen.fin_qa_reps(frm_qstr);
                    break;
                    ////05.02.2020====PRODN REPORTS FOR PCON
                case "F40216":
                case "F40218":
                case "F40220":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;
            }
        }
    }
}