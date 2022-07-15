using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_acct : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, frm_mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;

    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;

    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;

    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1 = "", xprdRange;

    string frm_AssiID;
    string frm_UserID;
    fgenDB fgen = new fgenDB();
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["mymst"] != null)
        {
            if (Session["mymst"].ToString() == "Y")
            {
                //this.Page.MasterPageFile = "~/tej-base/myNewMaster.master";
                this.Page.MasterPageFile = "~/tej-base/Fin_Master2.master";
            }
            else this.Page.MasterPageFile = "~/tej-base/Fin_Master.master";
        }
    }
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
                frm_mbr = mbr;
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "N");

        // asking for Branch Consolidate Popup
        if (hfaskBranch.Value == "Y")
        { hfaskBranch.Value = "Y"; fgen.msg("-", "CMSG", "Do you want to see consolidated report'13'(No for branch wise)"); }
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
                case "M03012":
                    SQuery = "select code AS FSTR,CODE,CODE AS S from co";
                    header_n = "Select Code";
                    break;
                case "P15005B":
                    SQuery = "SELECT VCHNUM AS FSTR,NAME AS DEPARTMENT,VCHNUM AS CODE FROM PROJ_MAST WHERE TYPE='P8' ORDER BY VCHNUM";
                    header_n = "Select Department";
                    break;
                case "P15005Z":
                    SQuery = "select * from (SELECT '1' AS FSTR,'Resource Wise' AS REPORT,'-' AS S FROM DUAL UNION ALL SELECT '2' AS FSTR,'Department Wise' as report,'-' as s from dual union all select '3' as fstr,'Project Wise' as report,'-' as s from dual union all select '4' as fstr,'BU Wise' as report,'-' as s from dual union all select '5' as fstr,'Resource wise & Project Wise' as report,'-' as s from dual union all select '6' as fstr,'Resource wise & Activity Wise' as report,'-' as s from dual union all select '7' as fstr,'Department wise & Project Wise' as report,'-' as s from dual union all select '8' as fstr,'Resource wise & Actual Hrs Worked' as report,'-' as s from dual union all select '9' as fstr,'Reason wise downtime analysis' as report,'-' as s from dual )";
                    header_n = "Select Report Type";
                    break;

                case "F25141":
                    // Matl. Inward Reg
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '0%' order by type1";
                    header_n = "Select Matl. Inward Type";
                    break;
                case "STKLEG":
                    fgen.Fn_open_ItemBox("-", frm_qstr);
                    break;
                case "F25142":
                    // Matl. Outward Reg
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '2%' order by type1";
                    header_n = "Select Mal. Outward Type";
                    break;

                case "F25143":
                // Matl. Issue Reg
                case "F25144":
                    // Matl. Return Reg
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '1%' and type1<'15' order by type1";
                    header_n = "Select Matl. Return Type";
                    break;

                case "F70239":
                    SQuery = "SELECT DISTINCT TYPE1 AS FSTR,NAME ,TYPE1 AS CODE,ACODE FROM TYPE WHERE substr(trim(TYPE1),1,1) in ('1','2') AND TYPE1 not in ('10','20') AND ID='V' ORDER BY CODE";
                    header_n = "Select Type";
                    break;

                case "F70240":
                    SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,b.aname,a.vchnum,TO_CHAR(a.vchdate,'dd/MM/yyyy')  as vchdate,a.cramt,a.type from voucher a,famst b where trim(a.rcode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '2%' and a.vchdate " + xprdrange + " AND CRAMT>0 order by vchdate desc ";
                    SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.rcode) as fstr,b.aname,a.vchnum,TO_CHAR(a.vchdate,'dd/MM/yyyy')  as vchdate,a.DRAMT AS cramt,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.rcode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '2%' and a.vchdate " + xprdrange + " AND A.cRAMT>0 order by vdd desc,a.vchnum desc,b.aname ";
                    header_n = "Select Entry";
                    break;

                case "F70136":// cheque Issue 
                    SQuery = "select trim(type1)||trim(acode) as fstr,name,type1 from type where type1 LIKE '2%'  and id='V' AND TYPE1 >'20' ORDER BY  TYPE1";
                    header_n = "Select Type";
                    break;

                case "F70137":// Bank Reco 
                    SQuery = "select  trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,  vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,type,Remarks as Bank_Name from costestimate where branchcd='" + mbr + "' and type='50' and vchdate " + xprdrange + " and srno='1' order  by vchnum desc";
                    header_n = "Select Entry No.";
                    break;

                case "F70148":// Balance sheet detail
                case "F70149":// Balance sheet summary
                case "F70241"://P&L SCHEDULE WISE
                case "F70242"://P&L SCHEDULE WISE
                    string chk_con = "";
                    chk_con = fgen.seek_iname(frm_qstr, co_cd, "SELECT can_con FROM evas WHERE UPPER(TRIM(username))='" + uname + "'", "can_con");
                    if (chk_con == "Y")
                    {
                        SQuery = "SELECT TYPE1 AS FSTR,TYPE1 AS BRANCH_CODE,NAME AS BRANCH_NAME FROM TYPE WHERE ID='B' order by type1";
                    }
                    else
                    {
                        SQuery = "SELECT TYPE1 AS FSTR,TYPE1 AS BRANCH_CODE,NAME AS BRANCH_NAME FROM TYPE WHERE ID='B' and type1='" + mbr + "'";
                    }
                    header_n = "Select Branch";
                    break;

                case "F70243"://DETAIL OF UNSECURED LOAN
                    SQuery = "select  distinct trim(a.acode) as fstr,trim(a.acode) as acode ,trim(b.aname) as Party from voucher a,famst b  where  substr(trim(a.acode),1,2)='04' and trim(a.acode)=trim(b.acode)";
                    header_n = "Select Acode";
                    break;

                case "F70223":
                    // BANK BOOK
                    //SQuery = "select distinct type1 as fstr, type1 as code,name from type where type1 like '2%' and id='V' order by code";
                    mq1 = fgen.seek_iname(frm_qstr, co_cd, "select trim(acode) as acode from type where id='V' and type1='20'", "acode");
                    SQuery = "select distinct TRIM(a.acode) AS FSTR,a.acode as code,b.aname as name from voucher a,famst b where trim(a.acode)=trim(b.acode) and substr(trim(a.acode),1,2) in ('03','12') and a.acode!='" + mq1 + "' order by name";
                    header_n = "Select Type";
                    break;

                case "F70229":
                case "F70230":
                case "F70268":
                case "F05100F":
                    SQuery = "select * from (SELECT TYPE1 AS FSTR,TYPE1 AS CODE,NAME  FROM TYPE WHERE ID='B' ORDER BY TYPE1) UNION ALL  SELECT '%' AS FSTR,'%' AS CODE ,'ALL BRANCH'  AS  NAME FROM DUAL";
                    header_n = "Select Branch";
                    break;

                case "F70252":
                case "F05133":
                    // SALES TREND
                    SQuery = "SELECT TRIM(aCODE) AS FSTR,ANAME,ACODE FROM FAMST WHERE substr(grp,1,1) LIKE '2%' AND ANAME LIKE '%SALE%' ORDER BY ACODE";
                    header_n = "Select Entry";
                    break;

                case "F70253":
                case "F05134":
                    // PURCHASE TREND
                    SQuery = "SELECT TRIM(aCODE) AS FSTR,ANAME,ACODE FROM FAMST WHERE substr(grp,1,1) LIKE '3%' ORDER BY ACODE";
                    header_n = "Select Entry";
                    break;

                ///BY YOGITA ON 23 MAY 2018

                case "F70254":
                    SQuery = "SELECT DISTINCT  TRIM(A.BRANCHCD)||TRIM(a.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE) AS FSTR,A.VCHNUM AS DOC_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DOC_DT,B.ANAME AS PARTY,A.ACODE AS PARTY_CODE,A.TYPE,A.ENT_BY FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE >'50' AND A.TYPE < '64' /*IN ('5A','57','61','62','63')*/  AND A.VCHDATE " + xprdrange + "  ORDER BY A.VCHNUM DESC,A.TYPE DESC";
                    header_n = "Select Entry";
                    break;

                case "F70255":
                    SQuery = "SELECT trim(TYPE1) AS FSTR,TYPE1 AS DOCUMENT_CODE,NAME AS DOCUMENT_NAME,ACODE AS ACCOUNT  FROM TYPE WHERE ID='V' AND SUBSTR(TRIM(TYPE1),1,2) NOT LIKE '4%' ORDER BY TYPE1";
                    SQuery = "select trim(a.branchcd)||TRIM(A.type)||TRIM(A.vchnum)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR, a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy') as vchdate,a.Type,sum(a.dramt) as Amount,b.aname,a.invno,a.ent_by from voucher a left outer join famst b on trim(a.rcode)=trim(b.acode) where a.branchcd='" + mbr + "' AND a.type='50' and a.VCHDATE  " + xprdrange + " and (a.tfcdr>0 or a.dramt>0) group by a.branchcd,a.Vchnum,a.Vchdate,a.Type,b.aname,a.invno,a.ent_by order by a.vchdate desc ,a.vchnum desc";
                    header_n = "Select Entry";
                    break;

                case "F25168**":
                case "F25167*":
                    SQuery = "SELECT 'YES' AS FSTR,'YES' AS CHOICE,'DO YOU WANT TO SEE MAIN GROUP'  AS  OPTION_  FROM DUAL UNION ALL SELECT 'NO' AS FSTR,'NO' AS CHOICE,'NO'  AS  OPTION_  FROM DUAL";
                    header_n = "Select Choice";
                    break;

                case "F25167":
                case "F25135":
                case "F25168"://main working sheet
                    SQuery = "SELECT DISTINCT TYPE1 AS FSTR,NAME,TYPE1 AS CODE,ADDR1 AS Store_type FROM TYPE where ID='Y' ORDER BY type1";
                    header_n = "Select Main Group Code";
                    break;

                case "F70270":
                case "F77209":
                case "F70271":
                case "F70144":
                case "F70146":
                case "F70270A":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F70264":
                case "F70260":
                    SQuery = "SELECT trim(Acode) AS FSTR, ANAME ,ACODE  FROM FAMST WHERE substr(acode,1,2) ='16' ORDER BY ACODE";
                    header_n = "Select Entry";
                    break;
                case "F70259":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70259");
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70265":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70265");
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70266":
                    SQuery = "SELECT Acode AS FSTR, ANAME ,ACODE  FROM FAMST WHERE substr(acode,1,2) >= ('30') ORDER BY ACODE";
                    header_n = "Select Entry";
                    break;
                case "F70298":// cross year
                case "F70141":// STATEMENT OF ACCOUNT
                case "F70141I":
                    fgen.Fn_open_Act_itm_prd("Select Branchcd / Party Code", frm_qstr);
                    break;
                case "F70141x":// STATEMENT OF ACCOUNT
                case "F70269":
                    SQuery = "select TRIM(TYPE1) AS FSTR,TRIM(TYPE1) AS  CODE,NAME from type where id='Z' AND TRIM(NVL(TYPE1,'-'))!='-' ORDER BY TYPE1";
                    header_n = "Select Main Group Code";
                    break;

                case "F70261":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70261");
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70263":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70263");
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70301": // SALES TREND HSN QTY WISE SUMMARY
                case "F70302": // SALES TREND HSN VALUE WISE SUMMARY
                case "F70303": // SALES TREND HSN QTY WISE DETAILED
                case "F70304": // SALES TREND HSN VALUE WISE DETAILED
                case "F70305": // SALES TREND HSN,TYPE QTY WISE SUMMARY
                case "F70306": // SALES TREND HSN,TYPE VALUES WISE SUMMARY
                case "F70307": // SALES TREND HSN,PARTY QTY WISE SUMMARY
                case "F70308": // SALES TREND HSN,PARTY VALUE WISE SUMMARY
                case "F70309": // PURCHASE TREND HSN QTY WISE SUMMARY
                case "F70310": // PURCHASE TREND HSN VALUE WISE SUMMARY
                case "F70311": // PURCHASE TREND HSN QTY WISE DETAILED
                case "F70312": // PURCHASE TREND HSN VALUE WISE DETAILED
                case "F70313": // PURCHASE TREND HSN,TYPE QTY WISE SUMMARY
                case "F70314": // PURCHASE TREND HSN,TYPE VALUE WISE SUMMARY
                case "F70315": // PURCHASE TREND HSN,PARTY QTY WISE SUMMARY
                case "F70316": // PURCHASE TREND HSN,PARTY VALUE WISE SUMMARY
                    SQuery = "select '2' AS FSTR,'2 DIGIT' AS hscode,'2' as Digit  from dual union all select '4' AS FSTR,'4 DIGIT' AS HSCODE,'4' AS DIGIT FROM DUAL UNION ALL SELECT '6' AS FSTR,'6 DIGIT' AS HSCODE,'6'  AS DIGIT FROM  DUAL UNION ALL SELECT '8' AS FSTR,'8 DIGIT' AS HSCODE,'8'  AS DIGIT FROM  DUAL";
                    header_n = "Select Choice";
                    break;

                case "F70132":
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where type1 like '1%' and id='V' union all SELECT '1%' AS FSTR,'1%' AS CODE,'ALL RECEIPT VOUCHER' as NAME FROM dual";
                    header_n = "Select Receipt Voucher Type";
                    break;

                case "F70133":
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where type1 like '2%' and id='V' union all SELECT '2%' AS FSTR,'2%' AS CODE,'ALL PAYMENT VOUCHEr' as NAME FROM dual";
                    header_n = "Select Payment Voucher Type";
                    break;

                case "F70245":
                    // PURCHASE REGISTER
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where type1 like '5%' and id='V' and type1!='54' order by type1";
                    header_n = "Select Purchase Type";
                    break;

                case "F70410":
                    SQuery = "select trim(branchcd)||trim(acode)||trim(vchnum)||to_char(vchdate,'dd/MM/yyyy') as fstr,acode,assetname,grpcode,assetsupp,assetsuppadd from wb_fa_pur where branchcd='" + mbr + "' and type='10' ORDER BY ACODE DESC";
                    header_n = "Select Asset";
                    break;

                case "F70262":
                    SQuery = "SELECT trim(Acode) AS FSTR, trim(ANAME) as aname ,trim(ACODE) as acode  FROM FAMST WHERE substr(trim(acode),1,2) in ('05','06') ORDER BY ACODE";
                    header_n = "Select Entry";
                    break;

                case "F70142":
                    SQuery = "select TYPE1 AS FSTR, NAME,type1 AS CODE from type where  id='Z' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;

                case "F70143":
                    SQuery = "select TYPE1 AS FSTR, NAME,type1 AS CODE from type where  id='V' ORDER BY TYPE1";
                    header_n = "Select Code";
                    break;
                //case "F70406":
                //    SQuery = "SELECT 'YES' AS FSTR,'YES' AS option_,'Do You want to see all assets including assets for which dep calculated is 0'  as text from dual union all SELECT 'NO' AS FSTR,'NO' AS option_,'Do You want to see all assets including assets for which dep calculated is 0'  as text from dual";
                //    header_n = "Select choice";
                //    break;
                case "F70231":
                case "F70232":
                case "F70233":
                case "F70234":
                case "F70235":
                case "F70236":
                case "F70151":
                case "F70237":
                case "F70238":
                case "F70348":
                case "F70506":
                case "F70507":
                case "F70508":
                case "F70509":
                case "F70285":
                case "F70204":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F70442":
                    //SQuery = "select trim(a.branchcd)||trim(a.acode) as fstr,a.acode,b.assetname from wb_Fa_vch a,wb_fa_pur b where trim(a.branchcd)||trim(a.acode)=trim(b.branchcd)||trim(b.acode) and a.type='20' and b.type='10' and a." + branch_Cd + "";
                    SQuery = "select  trim(a.branchcd)||trim(a.acode) as fstr,a.acode,a.assetname  from wb_fa_pur a where a.branchcd='" + mbr + "' and a.type='10' order by fstr";

                    break;

                case "F70378":
                    SQuery = "select mthnum as fstr,mthnum as code,mthname as month from mths";
                    header_n = "Select Month";
                    break;
                case "F70337":
                case "F70338":
                case "F70281":
                    SQuery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPEGRP WHERE BRANCHCD!='DD' AND ID='A' AND TYPE1 LIKE '16%' ORDER BY TYPE1 ";
                    header_n = "Select Marketing Code";
                    break;
                ///==========SUAMN REPORT
                case "SS01":
                case "SS02":
                case "SS03":
                case "SS04":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                //=======suman reports
                case "F70380"://BR
                case "F70381"://BR
                case "F70382"://PAYMT VCH
                case "F70383"://PAYMT VCH
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F70246":
                case "F70246L":
                    SQuery = "SELECT type1 as fstr,name as sale_Type,type1 as code  from type  where id='V' AND TYPE1 LIKE '4%' ORDER BY CODE";
                    header_n = "Select Sales Type";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F70142" || HCID == "F70410" || HCID == "F70148" || HCID == "F70149" || HCID == "F70241" || HCID == "F70242" || HCID == "F70229" || HCID == "F05100F" || HCID == "F70230" || HCID == "F70252" || HCID == "F05133" || HCID == "F70253" || HCID == "F05134" || HCID == "F70132" || HCID == "F70133" || HCID == "F70268" || HCID == "F70246L" || HCID == "F70246")
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
        // if coming after SEEK popup`
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            hfcode.Value = "";

            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F25141" || val == "F25142" || val == "F25143" || val == "F25144")
            {
                hfcode.Value = value1;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                fgen.Fn_open_PartyItemBox("", frm_qstr);
            }
            if (val == "F70223" || val == "F70233" || val == "F70239" || val == "F70235" || val == "F70232" || val == "F70236" || val == "F70231" || val == "F70136" || val == "F70229" || val == "F05100F" || val == "F70230" || val == "F70132" || val == "F70133" || val == "F70245" || val == "F70268" || val == "F70246" || val == "F70246L")
            {
                // by default it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else if (val == "F70281")
            {
                hfcode.Value = value1;
                fgen.Fn_open_Act_itm_prd("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F70234":
                        //SALE DAY REPORT FOR POP UP ////if(val=="F70234")
                        if (hf1.Value == "")
                        {
                            if (value1 == "NO")
                            {
                                SQuery = "SELECT DISTINCT TYPE1 AS FSTR,NAME ,TYPE1 AS CODE FROM TYPE WHERE TYPE1 LIKE '4%' AND ID='V' ORDER BY CODE";
                                header_n = "Select Type";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                                hf1.Value = value1;
                            }
                            else
                            {
                                hf1.Value = value1;
                                fgen.Fn_open_prddmp1("-", frm_qstr);
                            }
                        }
                        else
                        {
                            hfcode.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F70442":
                        hf1.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);
                        fgen.Fn_open_dtbox("-", frm_qstr);
                        break;

                    case "MCOL":
                        if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);
                            fgen.msg("-", "CMSG", "Do you want to see consolidated report?");
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Request.Cookies["REPLY"].Value);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_acct_reps(frm_qstr);
                        }
                        break;
                    case "FGVAL":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                        break;
                    case "F70382":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                        break;
                    case "F70383":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                        break;
                    case "CRYRLR":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                        break;
                    case "ACSTAT":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                        break;
                    case "F70240":
                        // PAYMENT ADVICE
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70240");
                        fgen.fin_acct_reps(frm_qstr);
                        break;
                    case "F70406": //depr chart\                   
                        hf1.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;
                    case "F70148":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70149":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70241"://PL SCHEDULE WISE
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70242"://PL SCHEDULE WISE
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    //BY YOGITA 5.5.18
                    case "F70243"://
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    //BY YOGITA 4MAY
                    case "F70137":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70137");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    //this is old
                    case "F70298":
                    case "F70141"://
                    case "F70141I":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VAL1", value1);
                        fgen.Fn_open_Act_itm_prd("Select Branchcd / Party Code", frm_qstr);
                        break;

                    case "F70252":
                    case "F05133":
                    case "F70253":
                    case "F05134":
                        hfcode.Value = value1;
                        //  fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    //BY YOGITA   23.05.18

                    case "F70254":
                        hf1.Value = value1; //                                                    
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);//for grade                           
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70254");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    case "F70255":
                        //if (hfval.Value == "")
                        //{
                        //    hfval.Value = value1; //select type                                                                              
                        //    SQuery = "select trim(a.branchcd)||TRIM(A.type)||TRIM(A.vchnum)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR, a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy') as vchdate,a.Type,sum(a.dramt) as Amount,b.aname,a.invno,a.ent_by from voucher a left outer join famst b on a.rcode=b.acode where a.VCHDATE  " + xprdrange + "  AND a.type='" + hfval.Value + "' and (a.tfcdr>0 or a.dramt>0) and  a.branchcd='" + mbr + "' group by a.branchcd,a.Vchnum,a.Vchdate,a.Type,b.aname,a.invno,a.ent_by order by a.vchdate desc ,a.vchnum desc";  // sir ki query                            
                        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        //    fgen.Fn_open_sseek(header_n, frm_qstr);
                        //}
                        //else
                        //{
                        //    hf1.Value = value1;
                        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                        //    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70255");
                        //    fgen.fin_acct_reps(frm_qstr);
                        //}
                        hf1.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70255");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    case "F70260":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70260");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    //case "F70262"://PUR TREND PARTY WISE GRAPH
                    //    hfcode.Value = value1;
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70262");
                    //    fgen.fin_acct_reps(frm_qstr);
                    //    break;

                    case "F70264":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70264");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    case "F70266":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70266");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    case "F25168*"://old
                    case "F25167*"://old
                        if (hf1.Value == "")
                        {
                            if (value1 == "YES")
                            {
                                SQuery = "SELECT DISTINCT TYPE1 AS FSTR,NAME,TYPE1 AS CODE,ADDR1 AS Store_type FROM TYPE where ID='Y' and trim(TYPE1)!='10' ORDER BY type1";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                                hf1.Value = value1;
                            }
                            else
                            {
                                fgen.send_cookie("srchSql", "SELECT DISTINCT type1 AS FSTR,name,type1 AS CODE FROM type where id='M' and trim(type1) in ('63','6E','6F') ORDER BY name");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                                hf1.Value = value1;
                            }
                        }
                        else
                        {
                            hfcode.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F25167":
                    case "F25135":
                    case "F25168"://main working sheet
                        hfcode.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70269":
                    case "F70262"://PUR TREND PARTY WISE GRAPH
                        hfcode.Value = value1; //                                                    
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70301":
                    case "F70302":
                    case "F70305":
                    case "F70306":
                    case "F70307":
                    case "F70308":
                    case "F70309":
                    case "F70310":
                    case "F70313":
                    case "F70314":
                    case "F70315":
                    case "F70316":
                    case "F70410":
                        hf1.Value = value1; //                                                    
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    case "F70303":
                    case "F70304":
                    case "F70311":
                    case "F70312":
                        #region
                        if (hf1.Value == "")
                        {
                            if (value1 == "2")
                            {
                                SQuery = "select trim(ACREF) as fstr,trim(ACREF) AS HSCODE,NAME AS HACODE_NAME  from typegrp where id='T1' AND LENGTH(replace(TRIM(ACREF),'.',''))='2' ORDER BY FSTR";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                                hf1.Value = value1;
                            }
                            if (value1 == "4")
                            {
                                SQuery = "select trim(ACREF) as fstr, ACREF AS HSCODE,NAME AS HACODE_NAME  from typegrp where id='T1' AND LENGTH(replace(TRIM(ACREF),'.',''))='4' ORDER BY FSTR";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                                hf1.Value = value1;
                            }
                            if (value1 == "6")
                            {
                                SQuery = "select trim(ACREF) as fstr, ACREF AS HSCODE,NAME AS HACODE_NAME  from typegrp where id='T1' AND LENGTH(replace(TRIM(ACREF),'.',''))='6' ORDER BY FSTR";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                                hf1.Value = value1;
                            }
                            if (value1 == "8")
                            {
                                SQuery = "select trim(ACREF) as fstr, ACREF AS HSCODE,NAME AS HACODE_NAME  from typegrp where id='T1' AND LENGTH(replace(TRIM(ACREF),'.',''))='8' ORDER BY FSTR";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                                hf1.Value = value1;
                            }
                        }
                        else
                        {
                            hf1.Value = value1; //                                                    
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_acct_reps(frm_qstr);
                        }
                        #endregion
                        break;

                    case "F70142"://GRP SUMMARY 
                    case "F70143"://Type SUMMARY  
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70335":
                    case "F70335A":
                        hfval.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                        //fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70336":
                        hfval.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F70204":
                        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                        if (hf1.Value == "")
                        {
                            xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                            cond = "";
                            if (value1 == "0") cond = " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.check_by,'-'))='-'";
                            if (value1 == "1") cond = " and trim(nvl(a.check_by,'-'))!='-' and trim(nvl(a.app_by,'-'))='-'";
                            if (value1 == "2") cond = " and trim(nvl(a.app_by,'-'))!='-'";
                            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE").Length > 2)
                                mq10 = "and a.type in (" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE") + ")";
                            else mq10 = " and a.type like '%'";
                            //cond += " and ((case when substr(type,1,1) in ('2','3','4') then a.tfcdr when type='59' then a.tfcdr when substr(type,1,1) in ('1','5','6') then a.tfccr end)>0 or (case when substr(type,1,1) in ('2','3','4') then a.cramt when type='59' then a.cramt when substr(type,1,1) in ('1','5','6') then a.dramt end)>0)  ";
                            SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum||' '||(case when trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.check_by,'-'))='-' then 'Un-Checked , Un Approved' when trim(nvl(a.check_by,'-'))!='-' and trim(nvl(a.app_by,'-'))='-' then 'Checked but Un-Approved' when trim(nvl(a.app_by,'-'))!='-' then 'Approved' end) as vch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Vch_Dt,b.Aname as Party,A.Ent_by,TO_CHAR(A.ENT_daTe,'DD/mm/yyyy') as Ent_Dt,a.vchnum as entryno,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.acodE)=trim(b.acode) and a.branchcd='" + mbr + "' " + mq10 + " and a.vchdate " + xprdrange + " " + cond + " AND A.SRNO=1 order by vdd desc,a.vchnum";
                            if (co_cd == "MEGH")
                            {
                                SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum||' '||(case when trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.check_by,'-'))='-' then 'Un-Checked , Un Approved' when trim(nvl(a.check_by,'-'))!='-' and trim(nvl(a.app_by,'-'))='-' then 'Checked but Un-Approved' when trim(nvl(a.app_by,'-'))!='-' then 'Approved' end) as vch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Vch_Dt,b.Aname as Party,A.Ent_by,TO_CHAR(A.ENT_daTe,'DD/mm/yyyy') as Ent_Dt,a.vchnum as entryno,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.acodE)=trim(b.acode) and a.branchcd='" + mbr + "' " + mq10 + " and a.vchdate " + xprdrange + " AND A.SRNO=1 order by vdd desc,a.vchnum";
                            }
                            else
                            {
                                SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum||' '||(case when trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.check_by,'-'))='-' then 'Un-Checked , Un Approved' when trim(nvl(a.check_by,'-'))!='-' and trim(nvl(a.app_by,'-'))='-' then 'Checked but Un-Approved' when trim(nvl(a.app_by,'-'))!='-' then 'Approved' end) as vch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Vch_Dt,b.Aname as Party,A.Ent_by,TO_CHAR(A.ENT_daTe,'DD/mm/yyyy') as Ent_Dt,a.vchnum as entryno,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.acodE)=trim(b.acode) and a.branchcd='" + mbr + "' " + mq10 + " and a.vchdate " + xprdrange + " " + cond + " AND A.SRNO=1 order by vdd desc,a.vchnum";
                            }
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                            hf1.Value = "Print";
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70203");
                            fgen.fin_acct_reps(frm_qstr);
                        }
                        break;

                    case "F70378":
                        hf1.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                        m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                        if (Convert.ToInt32(hf1.Value) > 3 && Convert.ToInt32(hf1.Value) <= 12)
                        {

                        }
                        else { year = (Convert.ToInt32(year) + 1).ToString(); }
                        m1 = m1 + " " + year;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hf1.Value + "/" + year);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", m1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70378");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    case "F70337":
                    case "F70338":
                        branch_Cd = "branchcd='" + mbr + "'";
                        cond = "";
                        if (value1.Length > 2)
                        {
                            cond = "AND b.BSSCH ='" + value1 + "'";
                            hfval.Value = value1;
                        }
                        todt = DateTime.Now.ToString("dd/MM/yyyy");
                        m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                        eff_Dt = " vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy')";
                        xprdrange = "BETWEEN TO_DATE('" + m1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') ";
                        //fgen.execute_cmd(frm_qstr, co_cd, "create or replace view recdata as(select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO");
                        fgen.execute_cmd(frm_qstr, co_cd, "create or replace view recdata as(select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT a.branchcd,a.ACODE,a.INVNO,a.INVDATE ,SUM(nvl(a.DRAMT,0)) AS DRAMT,SUM(nvl(a.CRAMT,0)) AS CRAMT ,sum(nvl(a.dramt,0))-sum(nvl(a.cramt,0)) as net FROM VOUCHER  a,famst b WHERE trim(a.acode)=trim(b.acode) and a.BRANCHCD!='88' AND a.BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(b.grp,1,2)IN('02','05','06','16') GROUP BY a.branchcd,a.ACODE,a.INVNO,a.INVDATE UNION ALL SELECT a.branchcd,a.ACODE,a.INVNO,a.INVDATE ,SUM(nvl(a.DRAMT,0)) AS DRAMT,SUM(nvl(a.CRAMT,0)) AS CRAMT ,sum(nvl(a.dramt,0))-sum(nvl(a.cramt,0)) as net FROM RECEBAL A,FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND SUBSTR(B.GRP,1,2)IN('02','05','06','16') GROUP BY A.branchcd,A.ACODE,A.INVNO,A.INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO");

                        SQuery = "select acode as fstr,code,partyname,p_email,sum(net) AS NET from (select a.acode,a.acode as code,b.aname as partyname,b.email as p_email,sum(A.dramt-A.cramt) as net,sum(to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM)) as ODUEdays,a.invno,a.invdate from recdata A,FAMST B where TRIM(a.ACODE)=TRIM(b.ACODE) and A." + branch_Cd + " " + cond + " having sum(A.dramt-A.cramt)>0 AND sum(to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM))!=0 group by a.acode,b.aname,b.email,a.invno,a.invdate ) GROUP BY acode,code,partyname,p_email order by partyname ";
                        if (co_cd == "INFI") SQuery = "select acode as fstr,code,partyname,p_email,net from (select a.acode,a.acode as code,b.aname as partyname,b.email as p_email,sum(A.dramt-A.cramt) as net,sum(to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM)) as ODUEdays from recdata A,FAMST B where TRIM(a.ACODE)=TRIM(b.ACODE) and A." + branch_Cd + " AND nvl(trim(b.email),'-') != '-' " + cond + " and a.invdate " + xprdrange + " having sum(A.dramt-A.cramt)>0 group by a.acode,b.aname,b.email ) order by partyname ";

                        if (val == "F70336")
                        {
                            SQuery = "SELECT DISTINCT a.acode AS FSTR,a.aname as party,a.acode AS CODE,a.email FROM famst a where 1=1 " + cond.Replace("b.", "a.") + " and nvl(trim(a.email),'-')!='-' ORDER BY a.aname";
                        }

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        if (co_cd == "TGIP")
                        {
                            fgen.Fn_open_sseek(header_n, frm_qstr);
                        }
                        else
                        {
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        break;
                    case "F70380":
                    case "F70381":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
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

                case "25156":// After Branch Consolidate Report  **************
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
                case "F70234":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F70335":
                case "F70335A":
                case "F70336":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    if (co_cd == "MLAB" && val == "F70335")
                    {
                        if (value1 == "Y") hfbr.Value = "ABR";
                        else hfbr.Value = "";
                        hf1.Value = value1;
                    }
                    //if (hf1.Value == "")
                    //{
                    //    if (value1 == "Y") hfbr.Value = "ABR";
                    //    else hfbr.Value = "";
                    //    hf1.Value = value1;
                    //    fgen.msg("-", "CMSG", "Do You want Report of Debtors'13'(No for Creditors)");
                    //}
                    //else if (hfval.Value == "")
                    //{
                    //    if (value1 == "Y")
                    //    {
                    //        hfval.Value = "16";
                    //        fgen.msg("-", "CMSG", "Do You Want to Select Debtors'13'(No for all Debtors)");
                    //    }
                    //    else
                    //    {
                    //        hfval.Value = "06";
                    //        fgen.msg("-", "CMSG", "Do You Want to Select Creditors'13'(No for all Creditors)");
                    //    }
                    //}
                    //else if (value1 == "Y")
                    //{
                    //    if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
                    //    else branch_Cd = "branchcd='" + mbr + "'";
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_BRANCH_CD", branch_Cd);
                    //    if (hfval.Value == "16") cond = " like '16%' ";
                    //    else cond = " like '%' and substr(b.GRP,1,2) in ('05','06') ";
                    //    todt = DateTime.Now.ToString("dd/MM/yyyy");
                    //    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                    //    eff_Dt = "A.vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy')";
                    //    xprdrange = "BETWEEN TO_DATE('" + m1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') ";

                    //    fgen.execute_cmd(frm_qstr, co_cd, "create or replace view recdata as(select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT a.branchcd,a.ACODE,a.INVNO,a.INVDATE ,SUM(nvl(a.DRAMT,0)) AS DRAMT,SUM(nvl(a.CRAMT,0)) AS CRAMT ,sum(nvl(a.dramt,0))-sum(nvl(a.cramt,0)) as net FROM VOUCHER  a,famst b WHERE trim(a.acode)=trim(b.acode) and a.BRANCHCD!='88' AND a.BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(b.grp,1,2)IN('02','05','06','16') GROUP BY a.branchcd,a.ACODE,a.INVNO,a.INVDATE UNION ALL SELECT a.branchcd,a.ACODE,a.INVNO,a.INVDATE ,SUM(nvl(a.DRAMT,0)) AS DRAMT,SUM(nvl(a.CRAMT,0)) AS CRAMT ,sum(nvl(a.dramt,0))-sum(nvl(a.cramt,0)) as net FROM RECEBAL A,FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND SUBSTR(B.GRP,1,2)IN('02','05','06','16') GROUP BY A.branchcd,A.ACODE,A.INVNO,A.INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO");

                    //    SQuery = "select acode as fstr,code,partyname,p_email,net from (select a.acode,a.acode as code,b.aname as partyname,b.email as p_email,sum(A.dramt-A.cramt) as net,sum(to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM)) as ODUEdays from recdata A,FAMST B where TRIM(a.ACODE)=TRIM(b.ACODE) and A." + branch_Cd + " and trim(B.GRP) " + cond + " having sum(A.dramt-A.cramt)>0 group by a.acode,b.aname,b.email ) order by partyname ";

                    //    SQuery = "select acode as fstr,code,partyname,p_email,net from (SELECT ACODE,CODE,PARTYNAME,P_EMAIL,SUM(NET) AS NET FROM (select a.acode,a.acode as code,b.aname as partyname,b.email as p_email,(A.dramt-A.cramt) as net,(to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM)) as ODUEdays from recdata A,FAMST B where TRIM(a.ACODE)=TRIM(b.ACODE) and A." + branch_Cd + " and trim(B.GRP) " + cond + " and (A.dramt-A.cramt)>0 AND (to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM))>0 ) GROUP BY ACODE,CODE,PARTYNAME,P_EMAIL ) order by ACODE,partyname ";

                    //    if (co_cd == "INFI") SQuery = "select acode as fstr,code,partyname,p_email,net from (select a.acode,a.acode as code,b.aname as partyname,b.email as p_email,sum(A.dramt-A.cramt) as net,sum(to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM)) as ODUEdays from recdata A,FAMST B where TRIM(a.ACODE)=TRIM(b.ACODE) and A." + branch_Cd + " AND nvl(trim(b.email),'-') != '-' and trim(B.GRP) " + cond + " and a.invdate " + xprdrange + " having sum(A.dramt-A.cramt)>0 group by a.acode,b.aname,b.email ) order by partyname ";

                    //    if (val == "F70336")
                    //    {
                    //        SQuery = "SELECT DISTINCT a.acode AS FSTR,a.aname as party,a.acode AS CODE,a.email FROM famst a where a.GRP " + cond.Replace("b.", "a.") + " and nvl(trim(a.email),'-')!='-' ORDER BY a.aname";
                    //    }

                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //    if (co_cd == "TGIP")
                    //    {
                    //        fgen.Fn_open_sseek(header_n, frm_qstr);
                    //    }
                    //    else
                    //    {
                    //        fgen.Fn_open_mseek(header_n, frm_qstr);
                    //    }
                    //}
                    //else
                    //{
                    //    fgen.Fn_open_dtbox("-", frm_qstr);
                    //}
                    break;

                case "F70406": //DEPR REPORT                
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    SQuery = "SELECT 'YES' AS FSTR,'YES' AS option_,'Do You want to see all assets including assets for which dep calculated is 0'  as text from dual union all SELECT 'NO' AS FSTR,'NO' AS option_,'Do You want to see all assets including assets for which dep calculated is 0'  as text from dual";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;
                case "F70407":
                case "F70408":
                case "F70409":
                case "F70414":
                case "F70432":
                case "F70433":
                case "F70434":
                case "F70435":
                case "F70436":
                case "F70412":
                case "F70413":
                case "F70416":
                case "F70417":
                case "F70418":
                case "F70425":
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "F70151":
                case "F70237":
                case "F70238":
                case "F70298***":
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
        }
        else
        {
            switch (val)
            {
                case "F25132":
                case "F25133":
                case "M03012":
                case "P15005B":
                case "P15005Z":
                case "F25141":
                case "F25142":
                case "F25143":
                case "F25144":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "F70298":
                case "F70141"://
                case "F70141I":
                    hfcode.Value = value1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VAL1", value1);
                    fgen.Fn_open_Act_itm_prd("Select Branchcd / Customer Code", frm_qstr);
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

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_BRANCH_CD", branch_Cd);

            // after prdDmp this will run            
            switch (val)
            {
                case "F25141": // MRR REG
                case "F25142":// Challan Reg
                case "F25143":// Issue Reg
                case "F25144": // Return Reg
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                // after prdDmp this will run--- MERGE BY MADHVI ON 27TH JAN 2018 , MADE BY YOGITA ---------- //               

                case "F70298": // CROSS YEAR ACCOUNT LEDGER                    
                case "F70222": // CASH BOOK
                case "F70141": // STATEMENT OF ACCOUNT
                case "F70141I":
                case "F70244"://GROUP SUMMARY OF UNSECURED LOAN                   
                case "F70228":// EXPENSE TREND
                case "F70248":// CASH MORE THAN 10,000
                case "F70249":// TOP 10 DEBTORS
                case "F05128":// MIS TOP 10 DEBTORS
                case "F70250":// TOP 10 CREDITORS
                case "F05131":// MIS CREDITORS WITH DEBIT BALANCE
                case "F70251": // CREDITORS WITH DEBIT BALANCE
                case "F05129":// MIS TOP 10 CREDITORS
                case "F25147":// ITEM WISE FG STOCK SUMMARY
                case "F70152":// HSN WISE FG STOCK SUMMARY
                case "F70144":// DEBTORS AGEING SUMMARY
                case "F70146":// CREDITORS AGEING SUMMARY
                case "F70270":// DEBTORS AGEING DETAILED
                case "F77209":
                case "F70271":// CREDITORS AGEING DETAILED
                case "F70265":// EXPENSE TREND GRAPH
                case "F70272": // NET SALES REPORT
                case "F70274": // NET PURCHASE REPORT
                case "F70280":
                case "F70281":
                case "F70299": // SALES SUMMARY WITH DEBIT CREDIT NOTES REPORT
                case "F70410":
                case "F70407":
                case "F70408":
                case "F70409":
                case "F70412":
                case "F70425":
                case "F70413":
                case "F70414"://only grp wise
                case "F70417":
                case "F70416":
                case "F70418":// Asset tag sticker print
                case "F70432":
                case "F70433":
                case "F70434":
                case "F70435":
                case "F70436":
                case "F70348":// cheque deposit slip              
                case "F70161":
                case "F70267":// PROFIT TREND BRANCH WISE
                case "F70151":
                case "F70237":
                case "F70238":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70236":// Cash DAY BOOK 07.02.18
                case "F70230": // P & L QuaterlY Trend 07.02.18
                case "F70229":// P & L Montly Trend 07.02.18
                case "F05100F":
                case "F70232":// PURC DAY BOOK 07.02.18
                case "F70235":// PURCHASE DAY BOOK 07.02.18
                case "F70233":// JOURNAL DAY BOOK 07.02.18
                case "F70239":// BANK DAY BOOK 07.02.18
                case "F70231":// Cash DAY BOOK 07.02.18
                case "F70136": //chq issue
                case "F70137": //back reco issue
                case "F70223":// CASH DAY BOOK
                case "F70245": // PURCHASE REGISTER
                case "F70268": // PROFIT TREND CONSOLIDATED
                case "F70246"://SALES REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70234": //SALES DAY bOOK col  07.02.18
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COl1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COl1", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70241"://PL SCHEDULE WISE
                case "F70242": //PL SCHEDULE WISE
                case "F70149"://balance sheet detail
                case "F70148": //balance sheet summary
                case "F70243"://UNSECURED LOAN DETAILS
                case "F25167":// RM STOCK SUMMARY
                case "F25135":
                case "F25168":// RM STOCK SUMMARY
                case "F70252": // SALES TREND (IN RS.000)
                case "F05133":// MIS SALES TREND (IN RS.000)
                case "F70253":// PURCASE TREND (IN RS.000)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70133":
                case "F70132":// RCPT REGISTER
                case "F05134":// MIS SALES TREND (IN RS.000)
                case "F70300": // SALES DETAILED WITH DEBIT CREDIT NOTES REPORT                
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "F70406":     // depreciation chart             
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70269":
                    // ACCOUNTS LEDGER
                    if (year != fromdt.Substring(6, 4))
                    {
                        i0 = Convert.ToInt32(year) + 1;
                        fgen.msg("-", "AMSG", "You have entered a Invalid Date !!.'13' Dates valid for this Year : 01/Apr/" + year + " to 31/Mar/" + i0);
                        return;
                    }
                    else
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70269");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgen.fin_acct_reps(frm_qstr);
                    }
                    break;


                case "F70256":
                case "F70258":
                case "F70257":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_smktg_reps(frm_qstr);
                    break;

                case "F70142"://group summary
                case "F70143"://type summary
                case "F70506":
                case "F70507":
                case "F70508":
                case "F70509":
                case "F70262":
                    // PAYMENT REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70285": // STUD ORDER REGISTRATION MAIL
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length <= 2)
                    {
                        fgen.msg("-", "AMSG", "Please Select Order First");
                        return;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "F70204":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    SQuery = "SELECT '0' AS FSTR,'Un-Approved , Un-Checked' as vouchers_type,'-' as v from dual union all SELECT '1' AS FSTR,'Un-Approved But Checked' as name,'-' as v from dual union all SELECT '2' AS FSTR,'Approved ' as name,'-' as v from dual ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;

                case "F70442":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1);
                    mq1 = "select sale_Dt,trim(branchcd)||trim(acode) as fstr from wb_Fa_vch where branchcd='" + mbr + "' and trim(branchcd)||trim(Acode)='" + hf1.Value + "' and type='20'";//kya daterange pass hogi isme
                    mq2 = fgen.seek_iname(frm_qstr, co_cd, mq1, "sale_Dt"); //for sale dt
                    //
                    mq4 = "select  to_char(life_end,'dd/mm/yyyy') as life_end,trim(branchcd)||trim(acode) as fstr  from wb_Fa_pur where branchcd='" + mbr + "' and type='10'  and  trim(branchcd)||trim(acode)='" + hf1.Value + "'";
                    mq5 = fgen.seek_iname(frm_qstr, co_cd, mq4, "life_end");
                    //
                    if (mq2.Length > 2)
                    {
                        if (Convert.ToDateTime(mq2) < Convert.ToDateTime(value1))
                        {
                            fgen.msg("-", "AMSG", "The asset already sold before this date");
                            return;
                        }
                    }
                    if (mq5.Length > 2)
                    {
                        if (Convert.ToDateTime(mq5) < Convert.ToDateTime(value1))
                        {
                            fgen.msg("-", "AMSG", "The asset already consumed/Life End before this date.Please see the FA-Ledger!!");
                            return;
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70335":
                case "F70335A":
                //case "F70336":
                case "F70337":
                case "F70338":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                    if (val != "F70337" && val != "F70338")
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70336":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "S06005E":
                    // open graph
                    SQuery = "select month_name,count(*) as  tot_bas,count(*) as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Query Graph", "line", "Month Wise", "-", SQuery, "");
                    break;

                case "MPAY":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "MCOL":
                    SQuery = "SELECT TYPE1 AS FSTR,NAME AS NAME,TYPE1 AS CODE FROM TYPEGRP WHERE ID='A' AND SUBSTR(TYPE1,1,2)='16' ORDER BY NAME";
                    header_n = "Select Account Group";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;
                case "SCOL":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "FGVAL":
                    SQuery = "select 'SC' as fstr, 'Std Cost' as name from dual union all select 'LI' as fstr, 'Latest Inv' as name from dual union all select 'LO' as fstr, 'Latest SO' as name from dual union all select 'SP' as fstr, 'Avg S.P' as name from dual ";
                    header_n = "Select Option";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;
                case "CRYRLR":
                    SQuery = "select acode as fstr,aname as name,trim(acode) as code from famst order by aname ";
                    header_n = "Select Party";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;
                case "ACSTAT":
                    SQuery = "select acode as fstr,aname as name,trim(acode) as code from famst order by aname ";
                    header_n = "Select Party";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;
                case "STKLEG":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
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

                case "F25133": // stock ledger summary
                case "F25132":// stock ledger 
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
                case "SS01":
                case "SS02":
                case "SS03":
                case "SS04":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_maint_reps(frm_qstr);
                    break;
                //==============suamn reports

                case "F70382":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "select distinct branchcd||type||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,ent_by from voucher where branchcd='" + frm_mbr + "' and type like '2%' and trim(acode)='" + col1 + "' and vchdate " + xprdrange + "";
                    header_n = "Select Voucher";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;
                case "F70383":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "select distinct branchcd||type||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,ent_by from voucher where branchcd='" + frm_mbr + "' and type like '1%' and trim(acode)='" + col1 + "' and vchdate " + xprdrange + "";
                    header_n = "Select Voucher";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;
                case "F70246L":
                    #region Sales Register
                    xprdRange = xprdrange;
                    DataTable dtDummy = new DataTable();
                    int m = 0;
                    dtDummy.Columns.Add("Invno", typeof(string));
                    dtDummy.Columns.Add("Invdate", typeof(string));
                    dtDummy.Columns.Add("vdd", typeof(string));
                    dtDummy.Columns.Add("Acode", typeof(string));
                    dtDummy.Columns.Add("Aname", typeof(string));

                    dtDummy.Columns.Add("Z1", typeof(double));
                    dtDummy.Columns.Add("Z2", typeof(double));
                    dtDummy.Columns.Add("Z3", typeof(double));
                    dtDummy.Columns.Add("Z4", typeof(double));
                    dtDummy.Columns.Add("Z5", typeof(double));
                    dtDummy.Columns.Add("Z6", typeof(double));

                    header_n = "";
                    mq0 = "";
                    // string frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    string frm_vty = hfcode.Value;
                    // LAST RUNNING QUERY   mq0 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type like '4%' and a.vchdate " + xprdRange + " and a.cramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1) in ('2','3')";
                    // mq0 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type='" + frm_vty + "' and a.vchdate " + xprdRange + " and a.cramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1)='2'";//old
                    mq0 = "select rownum,a.* from (select acode,aname,grp,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname,b.grp from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type IN (" + frm_vty + ") and a.vchdate " + xprdRange + " and a.cramt>0 group by a.acode,b.aname,b.grp) order by crtot desc) a where substr(trim(grp),1,1)='2'";//

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    int dtcnt = dt.Rows.Count;
                    int diff = 6 - dtcnt;
                    m = 5;
                    int n = 1;
                    for (int k = 0; k < dtcnt; k++)
                    {
                        dtDummy.Columns[m].ColumnName = "Z" + dt.Rows[k]["acode"].ToString().Trim();
                        m++; n++;
                    }
                    for (int k = 0; k < diff; k++)
                    {
                        dtDummy.Columns[m].ColumnName = "Z" + n;
                        m++; n++;
                    }

                    //dtDummy.Columns.Add("ZOthers", typeof(double));
                    dtDummy.Columns.Add("Other", typeof(double));
                    dtDummy.Columns.Add("Y1", typeof(double));
                    dtDummy.Columns.Add("Y2", typeof(double));
                    dtDummy.Columns.Add("Y3", typeof(double));
                    dtDummy.Columns.Add("Y4", typeof(double));
                    dtDummy.Columns.Add("Y5", typeof(double));
                    dtDummy.Columns.Add("Y6", typeof(double));

                    mq1 = "";
                    // LAST RUNNING QUERY  mq1 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type like '4%' and a.vchdate " + xprdRange + " and a.cramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1) not in ('2','3')";
                    // mq1 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type='" + frm_vty + "' and a.vchdate " + xprdRange + " and a.cramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1) in ('0','3')";//old
                    mq1 = "select rownum,a.* from (select acode,aname,grp,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname,b.grp from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type IN (" + frm_vty + ") and a.vchdate " + xprdRange + " and a.cramt>0 group by a.acode,b.aname,b.grp) order by crtot desc)a where substr(trim(grp),1,1) in ('0','3')";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    dtcnt = 0; diff = 0; n = 1;
                    dtcnt = dt1.Rows.Count;
                    diff = 6 - dtcnt;
                    m = 12;
                    for (int k = 0; k < dtcnt; k++)
                    {
                        dtDummy.Columns[m].ColumnName = "Y" + dt1.Rows[k]["acode"].ToString().Trim();
                        m++; n++;
                    }
                    for (int k = 0; k < diff; k++)
                    {
                        dtDummy.Columns[m].ColumnName = "Y" + n;
                        m++; n++;
                    }

                    //dtDummy.Columns.Add("YOthers", typeof(double));
                    dtDummy.Columns.Add("Others", typeof(double));
                    dtDummy.Columns.Add("TOT", typeof(double));

                    if (frm_vty == "'4F'")
                    {
                        dtDummy.Columns.Add("Currency", typeof(string));
                        dtDummy.Columns.Add("Currency_Price", typeof(string));
                        dtDummy.Columns.Add("Fx_amt", typeof(double));
                    }

                    //  mq2 = "select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.rcode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type='"+frm_vty+"' and a.vchdate " + xprdRange + " and a.cramt>0 and  substr(a.acode,1,2) not in ('16')  group by a.acode,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),b.aname,to_char(a.vchdate,'yyyymmdd') order by vchnum";//old
                    mq2 = "select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.tfcr,sum(a.tfcdr) As tfcdr,sum(A.tfccr) as tfccr,a.acode,b.aname,b.grp,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd,c.curren from voucher a,famst b,sale c where a.branchcd||a.type||Trim(a.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||Trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') and trim(a.Acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type IN (" + frm_vty + ") and a.vchdate " + xprdRange + " and a.cramt>0 and  trim(b.grp) not in ('16')  group by a.acode,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),b.aname,b.grp,to_char(a.vchdate,'yyyymmdd'),a.tfcr,c.curren order by vchnum";//new
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    mq3 = "select trim(acode) as acode ,trim(aname)||' (GST_NO:-'||trim(gst_no)||')' as aname from famst where acode like '16%'";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    if (dt2.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt2);
                        DataTable dt4 = new DataTable();
                        dt4 = view1.ToTable(true, "vchnum", "vchdate", "rcode");
                        DataRow dr1 = null;
                        foreach (DataRow dr2 in dt4.Rows)
                        {
                            DataView view2 = new DataView(dt2, "vchnum='" + dr2["vchnum"].ToString().Trim() + "' and vchdate='" + dr2["vchdate"].ToString().Trim() + "' and rcode='" + dr2["rcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            DataTable dt5 = new DataTable();
                            dt5 = view2.ToTable();

                            dr1 = dtDummy.NewRow();
                            double db1 = 0; double db2 = 0; double db3 = 0;
                            for (int i = 0; i < dt5.Rows.Count; i++)
                            {
                                dr1["INVNO"] = dt5.Rows[i]["vchnum"].ToString();
                                dr1["INVdate"] = dt5.Rows[i]["vchdate"].ToString();
                                dr1["vdd"] = dt5.Rows[i]["vdd"].ToString();
                                dr1["acode"] = dt5.Rows[i]["rcode"].ToString();
                                // dr1["aname"] = dt5.Rows[i]["aname"].ToString(); //old
                                dr1["aname"] = fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "'", "aname");
                                mq4 = dt5.Rows[i]["acode"].ToString().Trim().Substring(0, 1);
                                // ORIGINAL COND  if (mq4.Contains("2") || mq4.Contains("3"))
                                mq6 = dt5.Rows[i]["acode"].ToString().Trim(); //colm name 
                                //===============
                                for (int x1 = 0; x1 < dtDummy.Columns.Count; x1++)
                                {
                                    if (dtDummy.Columns[x1].ColumnName.Contains(dt5.Rows[i]["acode"].ToString().Trim()))
                                    {
                                        if (mq4.Contains("2"))
                                        {
                                            dr1["Z" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                            db1 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                            // dr1["ZOthers"] = db1; //this is total for x row wise total
                                            //dr1["other"] = 0;
                                        }
                                        else if (mq4.Contains("0") || mq4.Contains("3")) // ORIGINALLY THERE WAS ONLY ELSE CONDITION
                                        {
                                            dr1["Y" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                            db2 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                            //dr1["YOthers"] = db2; //this is total for y row wise total
                                            // dr1["Others"] = 0;
                                        }
                                        break;
                                    }
                                }
                                //  db3 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                db3 = db1 + db2;
                                #region old
                                //if (mq4.Contains("2"))
                                //{
                                //    try
                                //    {
                                //        dr1["Z" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                //    }
                                //    catch
                                //    {
                                //        db1 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                //        dr1["ZOthers"] = db1;
                                //    }
                                //    db3 += fgen.make_double(dt5.Rows[i]["crtot"].ToString()); ;
                                //}
                                //else if (mq4.Contains("0") || mq4.Contains("3")) // ORIGINALLY THERE WAS ONLY ELSE CONDITION
                                //{
                                //    try
                                //    {
                                //        dr1["Y" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                //    }
                                //    catch
                                //    {
                                //        db2 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                //        dr1["YOthers"] = db2;
                                //    }
                                //    db3 += fgen.make_double(dt5.Rows[i]["crtot"].ToString()); ;
                                //}   
                                #endregion

                                if (frm_vty == "'4F'")
                                {
                                    dr1["Currency"] = dt5.Rows[i]["curren"].ToString();
                                    dr1["Currency_Price"] = dt5.Rows[i]["tfcr"].ToString();
                                    dr1["Fx_amt"] = dt5.Rows[i]["tfccr"].ToString();
                                }

                            }
                            dr1["tot"] = db3;
                            dtDummy.Rows.Add(dr1);
                        }
                    }
                    if (dtDummy.Rows.Count > 0)
                    {

                        dtcnt = 0; diff = 0; n = 5;
                        dtcnt = dt.Rows.Count;
                        diff = 6 - dtcnt;
                        for (int k = 0; k < dtcnt; k++)
                        {
                            dtDummy.Columns[n].ColumnName = dt.Rows[k]["aname"].ToString().Trim();
                            n++;
                        }
                        #region
                        for (int k = 1; k <= diff; k++)
                        {
                            if (k == 1) dtDummy.Columns[n].ColumnName = "-";
                            if (k == 2) dtDummy.Columns[n].ColumnName = "--";
                            if (k == 3) dtDummy.Columns[n].ColumnName = "---";
                            if (k == 4) dtDummy.Columns[n].ColumnName = "----";
                            if (k == 5) dtDummy.Columns[n].ColumnName = "-----";
                            if (k == 6) dtDummy.Columns[n].ColumnName = "------";
                            n++;
                        }
                        #endregion

                        //=========
                        dtcnt = 0; diff = 0; n = 12;
                        dtcnt = dt1.Rows.Count;
                        diff = 6 - dtcnt;
                        for (int k = 0; k < dtcnt; k++)
                        {
                            dtDummy.Columns[n].ColumnName = dt1.Rows[k]["aname"].ToString().Trim();
                            n++;
                        }
                        #region
                        for (int k = 1; k <= diff; k++)
                        {
                            if (k == 1) dtDummy.Columns[n].ColumnName = "-,";
                            if (k == 2) dtDummy.Columns[n].ColumnName = "--,";
                            if (k == 3) dtDummy.Columns[n].ColumnName = "---,";
                            if (k == 4) dtDummy.Columns[n].ColumnName = "----,";
                            if (k == 5) dtDummy.Columns[n].ColumnName = "-----,";
                            if (k == 6) dtDummy.Columns[n].ColumnName = "------,";
                            n++;
                        }
                        #endregion
                        dtDummy.Columns.Remove("vdd");
                        dtDummy.Columns.Remove("Other");
                        dtDummy.Columns.Remove("Others");
                        Session["send_dt"] = dtDummy;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("Sales Register", frm_qstr);
                    }
                    else
                    {
                    }
                    #endregion
                    break;
                case "F70380":
                case "F70381":
                    SQuery = "Select '0' as fstr, 'Not Due only' as name,0 as srno from dual union all Select '1' as fstr, 'Due only' as name,1 as srno from dual union all Select '2' as fstr, 'Over Due only' as name,2 as srno from dual union all Select '3' as fstr, 'Long Due only' as name,3 as srno from dual union all Select '4' as fstr, 'Acc Block Days only' as name,4 as srno from dual ";
                    header_n = "Select Filter";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                    break;
            }
        }
    }
}