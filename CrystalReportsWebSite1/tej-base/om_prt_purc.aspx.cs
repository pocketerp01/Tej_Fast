using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_purc : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, mq11, mq12, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
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
        // else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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

                case "F15132":
                case "F15251":
                    //pkgg  Import PO register
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F15133":
                    SQuery = "select * from (SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' union all SELECT '5%' AS FSTR,'All PO Type' as NAME,'5%' AS CODE FROM dual) order by code";
                    header_n = "Select Type";
                    break;

                case "F15134":
                    //SQuery = "SELECT 'YES' AS FSTR,'Do You Want to See Print Figures in Thousands' as MSGS,'Y' as che from dual union all select 'NO' as fstr,'Do You Want to See Print Figures in Thousands' as MSGS,'N' as che from dual";
                    if (co_cd == "BUPL")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", "N");
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    }
                    else
                        fgen.msg("-", "CMSG", "Do You Want to See Print Figures in Thousands");
                    //fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F15137":
                    SQuery = "select DISTINCT trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,A.ORDNO AS PO_NO,TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS PO_DT,trim(A.ACODE) AS SUPP_CODE,trim(B.ANAME) AS SUPPLIER FROM POMAS A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE = '54' AND A.orddt " + xprdrange + " ORDER BY A.ORDNO DESC";
                    header_n = "Select Entry";
                    break;

                case "F15143":
                case "F15142":
                case "F15250":
                    SQuery = "select * from (SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' union all SELECT '5%' AS FSTR,'All PO Type' as NAME,'5%' AS CODE FROM dual) order by code ";
                    header_n = "Select Type";
                    break;

                case "F15144":
                    SQuery = "select psize as fstr,ordno AS PR_NO,to_char(orddt,'dd/mm/yyyy') as PR_dt,psize AS WO_NO  from pomas where substr(type,1,1)='6'  and LENGTH(PSIZE)>1  order by PR_NO DESC ";
                    SQuery = "select distinct  trim(ordno)||to_char(orddt,'dd/mm/yyyy')||psize as fstr,ordno AS PR_NO,to_char(orddt,'dd/mm/yyyy') as PR_dt,psize AS WO_NO  from pomas where branchcd='" + mbr + "' and substr(type,1,1)='6'  and LENGTH(PSIZE)>1  order by PR_NO DESC ";//prno,prdt,wono wise
                    header_n = "Select Work Order No.";
                    break;

                case "F15222":
                case "F15223":
                    SQuery = "SELECT TRIM(MTHNUM) AS FSTR,MTHNUM,MTHNAME FROM MTHS ORDER BY MTHSNO";
                    header_n = "Select Month";
                    break;

                case "F15135":
                case "F15136":
                case "F15140":
                case "F15141":
                case "F15230":
                case "F15231":
                case "F15232":
                case "F15235":
                case "F15237":
                case "F15238":
                case "F15239":
                case "F15240":
                case "F15244":
                case "F15247":
                case "F15248":
                case "F15249":
                case "F15236":
                case "F15241":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F15605"://request for quotation comparision
                    SQuery = "select distinct trim(a.icode) as fstr, trim(a.icode) as item_code,trim(b.iname) as item_name from WB_PORFQ a,item b where A.BRANCHCD='" + mbr + "' AND A.TYPE='50' AND trim(a.icode)=trim(b.icode)";
                    header_n = "Select Item Code";
                    break;

                case "F15189":
                    SQuery = "SELECT TYPE1 AS FSTR,NAME AS DOCUMNET_TYPE,TYPE1 AS CODE FROM TYPE WHERE ID='M'  AND TYPE1 LIKE ('5%') ORDER BY TYPE1";
                    header_n = "Select PO Type";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F15222" || HCID == "F15223" || HCID == "F15605" || HCID == "F15189")
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
            switch (val)
            {

                case "F15144":
                    //case "F15143":
                    hf1.Value = value1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F15142":
                case "F15250":
                    hf1.Value = value1;
                    //  fgen.Fn_open_prddmp1("-", frm_qstr);
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F15143":
                case "F15133":
                case "F15127":
                    hfcode.Value = value1;
                    hf1.Value = value1;
                    SQuery = "";
                    fgen.Fn_open_Act_itm_prd("Choose Party / Item for more filter", frm_qstr);
                    break;

                //fgen.Fn_open_prddmp1("-", frm_qstr);
                //break;

                case "F15137":
                    hfcode.Value = value1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15137");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15311":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    hf1.Value = value1;
                    break;

                case "F15222": // SCHEDULE VS RECEIPT DAY WISE REPORT
                case "F15223": // SCHEDULE VS RECEIPT TOTAL BASIS REPORT
                    //  fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                    hf1.Value = value1;
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    // fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    // fgen.fin_purc_reps(frm_qstr);
                    break;
                case "F15605":
                    if (hfval.Value == "")
                    {
                        hfval.Value = value1; //select icode
                        SQuery = "SELECT trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode) as fstr,a.ordno as entry_no,to_char(a.orddt,'dd/mm/yyyy') as entry_date FROM WB_PORFQ a where a.branchcd='" + mbr + "' and a.type='50' and a.orddt " + xprdrange + " and trim(a.icode)='" + hfval.Value + "' order by entry_no desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else
                    {
                        hf1.Value = value1; //                                                    
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);//for ordno                          
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_hrm_reps(frm_qstr);
                    }
                    break;

                case "F15189":
                    if (hf1.Value == "")
                    {
                        hf1.Value = value1;
                        SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.ordno||(case when nvl(a.app_by,'-')='-' then ' UnApproved' else ' Approved' end) as po_no,to_char(a.orddt,'dd/mm/yyyy') as order_Dt,b.aname as party,b.email,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt,a.ordno from POMAS a,famst b where trim(a.acode)=trim(B.acode) and a.branchcd='" + mbr + "' and a.type='" + value1 + "' and a.orddt " + xprdrange + " and a.app_by!='-' order by a.ordno desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_sseek(header_n, frm_qstr);
                    }
                    else
                    {
                        if (co_cd == "ADVG")
                        {
                            #region ADVG PO Report
                            Session["POAttachment"] = null;
                            Session["attach"] = null;
                            string PO = "";
                            if (value1 == "" || value1 == "-") return;
                            col1 = "" + value1 + "";
                            DataTable dtSch = new DataTable();
                            DataTable dtPoTerm = new DataTable();
                            mq0 = fgen.seek_iname(frm_qstr, co_cd, "select ordno from pomas a where a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "')", "ordno");
                            mq1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(a.orddt,'dd/mm/yyyy') as orddt from pomas a where a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "')", "orddt");
                            mq2 = fgen.seek_iname(frm_qstr, co_cd, "select f.aname from famst f,pomas a  where trim(a.acode)=trim(f.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "')", "aname");
                            PO = "select  distinct '" + mq0 + "' as ordno,'" + mq1 + "' as orddt,'" + mq2 + "' as aname,a.* from poterm a where  a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') in ('" + col1 + "') order by sno";
                            PO = "select  distinct T.POPREFIX||'-'||'" + mq0 + "' as ordno,'" + mq1 + "' as orddt,'" + mq2 + "' as aname,A.TERMS||':'||A.CONDI AS TERM_REM ,a.* from poterm a,TYPE T where TRIM(A.BRANCHCD)=TRIM(T.TYPE1) AND T.ID='B' AND a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') in ('" + col1 + "') order by sno";

                            mq0 = fgen.seek_iname(frm_qstr, co_cd, "select aname from pomas a,famst d where trim(a.othac1)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "') and SUBSTR(trim(a.APP_BY),1,3)<> '[U]'", "aname");
                            mq1 = fgen.seek_iname(frm_qstr, co_cd, "select aname from pomas a,famst d where trim(a.othac2)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "') and SUBSTR(trim(a.APP_BY),1,3)<> '[U]'", "aname");
                            mq2 = fgen.seek_iname(frm_qstr, co_cd, "select aname from pomas a,famst d where trim(a.othac3)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "') and SUBSTR(trim(a.APP_BY),1,3)<> '[U]'", "aname");
                            mq3 = fgen.seek_iname(frm_qstr, co_cd, "select name from type where type1='" + hf1.Value.ToString() + "' and id='M'", "name");
                            mq4 = fgen.seek_iname(frm_qstr, co_cd, "select TAX from POMAS A where a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "')", "TAX");
                            mq5 = fgen.seek_iname(frm_qstr, co_cd, "select name from type where type1='" + mq4.Trim() + "' and id='P'", "name");
                            dtPoTerm = fgen.getdata(frm_qstr, co_cd, PO);

                            string poterm = "";
                            string final = "";
                            string POAttachment = "";
                            DataTable dtPOAttachment = new DataTable();

                            POAttachment = "SELECT DISTINCT ATTACH1,ORDNO,TO_CHAR(ORDDT,'DD/MM/YYYY') AS ORDDT FROM POMAS A WHERE a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "') and attach1!='-' ";
                            POAttachment = "SELECT DISTINCT ATTACH1,ORDNO,TO_CHAR(ORDDT,'DD/MM/YYYY') AS ORDDT,T.POPREFIX||'-'||A.ORDNO AS PRE_ORD FROM POMAS A,TYPE T WHERE TRIM(A.BRANCHCD)=TRIM(T.TYPE1) AND T.ID='B' And a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "') AND A.ATTACH1!='-' ";
                            dtPOAttachment = fgen.getdata(frm_qstr, co_cd, POAttachment);

                            if (dtPoTerm.Rows.Count > 0)
                            {
                                poterm = " Terms & Conditions";
                                final = "Please find attached" + poterm;
                            }
                            if (co_cd == "ADVG")
                            {
                                SQuery = "select distinct '" + final + "' as poterm , a.branchcd,'" + mq0 + "' as othacn1,'" + mq1 + "' as othacn2,'" + mq2 + "' as othacn3,'" + mq5 + "' AS TAXNAME,'Rate in Kg' as cond, a.pexc,a.type,T.POPREFIX||'-'||A.ORDNO AS PRE_ORD,a.ordno,a.DEL_MTH, to_char(a.orddt,'dd/mm/yyyy') as orddt,a.srno,a.icode,trim(b.iname) as iname,b.cpartno,b.iweight,b.unit, a.ciname as cinm,a.desc_,a.qtyord,a.prate as rate,a.pdisc as disc,a.pexc as ed,a.pcess as cess,a.invno,a.ptax, to_char(a.del_date ,'dd/mm/yyyy') as delvdt , to_char(a.effdate ,'dd/mm/yyyy') as effdate,'" + mq3 + "' AS PO_TYPE, (CASE WHEN trim(NVL(a.app_by,'-')) = '-' THeN 'DRAFT P.O.' else 'PURCHASE ORDER' end ) as App_status,   a.app_by,to_char(a.app_dt ,'dd/mm/yyyy') as app_dt,a.pamt,a.psize,a.acode,a.inst,a.term,a.qtysupp,a.qtybal,a.pordno, to_char(a.porddt ,'dd/mm/yyyy') as porddt,to_char(a.invdate ,'dd/mm/yyyy') as invdate,a.delivery,a.del_mth as adelmth,a.del_wk, a.delv_term,to_char(a.refdate ,'dd/mm/yyyy') as refdate,a.mode_tpt,a.tr_insur,a.desp_to,a.freight,a.doc_thr,a.packing,a.payment,a.bank,a.stax,a.exc,a.iopr,a.pr_no as prnum,a.amd_no,a.del_sch as wono,a.wk1,a.wk2,a.wk3,a.wk4 as pnf,a.vend_wt,a.store_no,a.ent_by,to_char(a.ent_dt ,'dd/mm/yyyy') as ent_dt,d.aname,d.rc_num2 as pcstno,d.girno as ppanno,d.rc_num as ptinno,d.EXC_NUM as peccno,d.addr1 as caddr1,d.addr2 as caddr2,d.addr3 as caddr3,d.addr4 as caddr4,D.RC_NUM AS CTIN,d.telnum as telephone,d.person as person,d.mobile as mobile,d.email as email,D.EMAIL AS p_email,d.staten,d.country,d.fax as cfax, a.issue_no,a.pflag,to_char(a.pr_dt ,'dd/mm/yyyy') as prdate,a.test,a.pbasis,a.rate_ok,a.rate_cd,a.rate_rej,a.delv_item,a.transporter, a.st38no,a.nxtmth2,a.currency,a.remark,a.pexcamt as edr,a.pdiscamt as discr,a.amdtno,a.orignalbr,a.o_prate,a.o_qty,a.chl_ref,a.othac1,a.othac2,a.othac3,a.othamt1,a.othamt2,a.othamt3,a.st31no,a.d18no,a.tdisc_amt,a.CHECK_BY AS chk_by,d.gst_no as dgst_no,substr(d.gst_no,0,2) as dstatecode,b.hscode,A.TAX from pomas a, item b, IVCHCTRL c,famst d,TYPE T where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode) AND TRIM(A.BRANCHCD)=TRIM(T.TYPE1) AND T.ID='B' and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "') and SUBSTR(trim(a.APP_BY),1,3)<> '[U]' order by orddt,a.ordno,a.srno";
                                mq2 = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + mbr + "' AND TYPE='70' AND DOCTYPE='PO' ORDER BY SRNO";
                                dt2 = new DataTable();
                                dt2 = fgen.getdata(frm_qstr, co_cd, mq2);
                                mq10 = "";
                                dt3 = new DataTable();
                                DataRow dr1 = null;
                                dt3.Columns.Add("po_terms", typeof(string));
                                for (int i = 0; i < dt2.Rows.Count; i++)
                                {
                                    dr1 = dt3.NewRow();
                                    mq10 += dt2.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                                    dr1["po_terms"] = mq10;
                                }
                                dt3.Rows.Add(dr1);
                                dt3.TableName = "PO_TERMS";
                            }
                            else SQuery = "select distinct '" + final + "' as poterm , a.branchcd,'" + mq0 + "' as othacn1,'" + mq1 + "' as othacn2,'" + mq2 + "' as othacn3,'" + mq5 + "' AS TAXNAME,'Rate in Kg' as cond, a.pexc,a.type,a.ordno,a.DEL_MTH, to_char(a.orddt,'dd/mm/yyyy') as orddt,a.srno,a.icode,b.iname,b.cpartno,b.iweight,b.unit, a.ciname as cinm,a.desc_,a.qtyord,a.prate as rate,a.pdisc as disc,a.pexc as ed,a.pcess as cess,a.invno,a.ptax, to_char(a.del_date ,'dd/mm/yyyy') as delvdt , to_char(a.effdate ,'dd/mm/yyyy') as effdate,'" + mq3 + "' AS PO_TYPE, (CASE WHEN trim(NVL(a.app_by,'-')) = '-' THeN 'DRAFT P.O.' else 'Purchase Order' end ) as App_status,   a.app_by,to_char(a.app_dt ,'dd/mm/yyyy') as app_dt,a.pamt,a.psize,a.acode,a.inst,a.term,a.qtysupp,a.qtybal,a.pordno, to_char(a.porddt ,'dd/mm/yyyy') as porddt,to_char(a.invdate ,'dd/mm/yyyy') as invdate,a.delivery,a.del_mth as adelmth,a.del_wk, a.delv_term,to_char(a.refdate ,'dd/mm/yyyy') as refdate,a.mode_tpt,a.tr_insur,a.desp_to,a.freight,a.doc_thr,a.packing,a.payment,a.bank,a.stax,a.exc,a.iopr,a.pr_no as prnum,a.amd_no,a.del_sch as wono,a.wk1,a.wk2,a.wk3,a.wk4 as pnf,a.vend_wt,a.store_no,a.ent_by,to_char(a.ent_dt ,'dd/mm/yyyy') as ent_dt,d.aname,d.rc_num2 as pcstno,d.girno as ppanno,d.rc_num as ptinno,d.EXC_NUM as peccno,d.addr1 as caddr1,d.addr2 as caddr2,d.addr3 as caddr3,d.addr4 as caddr4,D.RC_NUM AS CTIN,d.telnum as telephone,d.person as person,d.mobile as mobile,d.email as email,D.EMAIL AS p_email,d.staten,d.country,d.fax as cfax, a.issue_no,a.pflag,to_char(a.pr_dt ,'dd/mm/yyyy') as prdate,a.test,a.pbasis,a.rate_ok,a.rate_cd,a.rate_rej,a.delv_item,a.transporter, a.st38no,a.nxtmth2,a.currency,a.remark,a.pexcamt as edr,a.pdiscamt as discr,a.amdtno,a.orignalbr,a.o_prate,a.o_qty,a.chl_ref,a.othac1,a.othac2,a.othac3,a.othamt1,a.othamt2,a.othamt3,a.st31no,a.d18no,a.tdisc_amt,a.chk_by from pomas a, item b, IVCHCTRL c,famst d where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode)  and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + col1 + "') and SUBSTR(trim(a.APP_BY),1,3)<> '[U]' order by orddt,a.ordno,a.srno";

                            fgen.send_cookie("Send_Mail", "Y");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            string icode = "";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                icode += "," + "'" + dt.Rows[i]["icode"].ToString().Trim() + "'";
                            }
                            string query = "select nvl(trim(a.imagef),'-') as imagef,a.ICODE,b.ordno,to_char(b.orddt,'dd/MM/yyyy') as orddt,T.POPREFIX||'-'||b.ORDNO AS PRE_ORD from item a, pomas b,type t where trim(a.icode)=trim(b.icode) and trim(b.branchcd)=trim(t.type1) and t.id='B' and a.icode in (" + icode.TrimStart(',') + ") and b.BRANCHCD||b.TYPE||TRIM(b.ordno)||TO_CHAr(b.orddt,'DD/MM/YYYY') in ('" + col1 + "')";

                            if (dt.Rows.Count > 0)
                            {
                                dt1 = new DataTable();
                                dt1 = fgen.getdata(frm_qstr, co_cd, query);
                            }
                            if (dtPOAttachment.Rows.Count > 0)
                            {
                                Session["POAttachment"] = dtPOAttachment;
                            }
                            DataTable dtPO = new DataTable();
                            dtPO = fgen.getdata(frm_qstr, co_cd, SQuery);

                            Session["RPTDATA1"] = PO;
                            string rptfile = "~/tej-base/REPORT/popAdvnAnn.rpt";
                            fgen.send_cookie("RPTFILE1", rptfile);
                            if (dtPoTerm.Rows.Count > 0)
                            {
                                fgen.Fn_Print_Report(co_cd, frm_qstr, mbr, PO, "popAdvnAnn", "popAdvnAnn");
                            }
                            Session["attach"] = dt1;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            ds = new DataSet();
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            dt.TableName = "Prepcur";
                            ds.Tables.Add(dt);
                            ds.Tables.Add(dt3);
                            fgen.Print_Report_BYDS_ADVG(co_cd, frm_qstr, mbr, "popAdvn", "popAdvn", ds, "");
                            #endregion
                        }
                        else
                        {
                            #region Approved P.O. For Mail
                            col1 = value1;
                            string opt = fgen.getOption(frm_qstr, co_cd, "W0012", "OPT_ENABLE");
                            cond = ""; mq1 = "";
                            if (co_cd == "NAHR")
                            {
                                cond = "TRIM(A.SPLRMK)";
                                mq1 = "Reels";
                            }
                            else
                            {
                                cond = "TRIM(C.CPARTNO)";
                                mq1 = "Part No.";
                            }
                            if (hf1.Value != "54")
                            {
                                SQuery = "SELECT a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,d.ANAME,TRIM(D.ANAME) AS CUST,TRIM(D.ADDR1) AS ADRES1,TRIM(D.ADDR2) AS ADRES2,TRIM(D.ADDR3) AS ADRES3,TRIM(D.GIRNO) AS CUSTPAN,TRIM(D.STAFFCD) AS STAFFCD,TRIM(D.PERSON) AS CPERSON,TRIM(D.EMAIL) AS CMAIL,TRIM(D.TELNUM) AS CONT,TRIM(D.STATEN) AS CSTATE, TRIM(D.GST_NO) AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,TRIM(B.NAME) AS TYPENAME,TRIM(C.INAME) AS INAME," + cond + " AS  PARTNO,TRIM(C.PUR_UOM) AS CMT,TRIM(C.NO_PROC) AS Sunit,TRIM(C.UNIT) AS CUNIT,TRIM(C.HSCODE) AS HSCODE,A.*,(case WHEN  A.app_by='-' Then 'DRAFT P.O.' ELSE  'PURCHASE ORDER' END) AS CASE,nvl(d.email,'-') as p_email,'" + mq1 + "' as gidheading,A.srno FROM POMAS A,TYPE B,ITEM C,FAMST D WHERE TRIM(A.TYPE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) and B.ID='M' AND TRIM(A.ACODE)=TRIM(D.ACODE) AND a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') ='" + col1 + "' ORDER BY a.orddt,a.ordno,A.srno ";
                            }
                            else
                            {
                                SQuery = " select distinct a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'Import Purchase Order' as header,NVL(a.currency,0) AS currency,trim(a.delv_item) as delv_item,a.amdtno, trim(b.aname) as aname,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.addr4) as addr4,trim(b.email) as email,B.TELNUM,B.MOBILE,trim(c.hscode) as hscode,trim(c.iname) as iname,trim(c.ciname) as ciname,trim(c.unit) as cunit,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(a.icode) as icode,nvl(a.qtyord,0) as qtyord,nvl(a.prate,0) as prate,nvl(a.pdisc,0) as pdisc,trim(a.payment) as pay_term,trim(a.transporter) as shipp_frm,trim(a.desp_to) as shipp_to,trim(a.mode_tpt) as mode_tpt,trim(a.delv_term) as etd,trim(a.tr_insur) as insurance,trim(a.packing) as packing,trim(a.remark) as remark,a.cscode1,a.cscode,nvl(a.pdiscamt,0) as pdiscamt,nvl(a.qtybal,0) as qtybal,trim(d.aname) as consign,trim(d.addr1) as daddr1,trim(d.addr2) as daddr2,trim(d.addr3) as daddr3,trim(d.addr4) as daddr4,trim(d.telnum) as dtel, trim(d.rc_num) as dtinno,trim(d.exc_num) as dcstno,trim(d.acode) as mycode,trim(d.staten) as dstaten,trim(d.gst_no) as dgst_no,trim(d.girno) as dpanno,substr(d.gst_no,0,2) as dstatecode,nvl(b.email,'-') as p_email,TRIM(b.PERSON) AS CPERSON,a.type,a.desc_,A.srno from  famst b,item c,pomas a left join csmst d on trim(a.cscode1)=trim(d.acode) where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') ='" + col1 + "' ORDER BY a.ordno,A.srno";
                            }
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            ds = new DataSet();
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                dt.TableName = "Prepcur";
                                //BarCode adding
                                dt = fgen.addBarCode(dt, "fstr", true);
                                ds.Tables.Add(fgen.mTitle(dt, 1));
                                SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + mbr + "' AND TYPE='70' AND DOCTYPE='PO' ORDER BY SRNO";
                                dt1 = new DataTable();
                                dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);
                                dt1.TableName = "type1";
                                mq10 = "";
                                dt3 = new DataTable();
                                oporow = null;
                                dt3.Columns.Add("poterms", typeof(string));
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    mq10 += dt1.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                                }
                                oporow = dt3.NewRow();
                                oporow["poterms"] = mq10;
                                dt3.Rows.Add(oporow);
                                dt3.TableName = "type1";
                                ds.Tables.Add(dt3);
                                fgen.send_cookie("Send_Mail", "Y");
                                if (hf1.Value == "54")
                                {
                                    fgen.Print_Report_BYDS(co_cd, frm_qstr, mbr, "std_Imp_PO", "std_Imp_PO", ds, "Import P.O. Entry Report", "Y");
                                }
                                else
                                {
                                    fgen.Print_Report_BYDS(co_cd, frm_qstr, mbr, "std_po_mails", "std_po_mails", ds, "P.O. Entry Report", "Y");
                                }
                            }
                            #endregion
                        }
                    }
                    break;
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

                case "F15134":
                    // BY MADHVI ON 14/03/2018
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", value1);
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
        }
        //FOR BYDEFAULT DATERANGE AFTER POPUP
        else
        {
            switch (val)
            {
                case "F15134":
                    // BY MADHVI ON 14/03/2018
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", value1);
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
                case "F15132":
                    // P.R. REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15132");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15133":
                    // P.O.REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COl1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15133");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                //case "F15142":
                //    // pending P.O.REGISTER
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COl1", hfcode.Value);
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15142");
                //    fgen.fin_purc_reps(frm_qstr);
                //    break;

                // FROM HERE CODE IS MERGED BUT NOT CHECKED AS FOR ID F15141,F15143 BCOZ THEY ARE CREATED ON BASIS OF OLD QUERY.
                // SO NEW QUERY HAVE TO TAKE FROM PUNEET SIR

                case "F15134":
                    // PURCHASE SCHEDULE
                    if (co_cd == "BUPL") fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");

                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15134");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                // MERGE BY MADHVI ON 22ND JAN 2018 , CREATED BY YOGITA ON 22ND JAN 2018 -----//
                case "F15135":
                    // APPROVED PRICE REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15135");
                    fgen.fin_purc_reps(frm_qstr);
                    break;
                // -----------------------------------------------------------------------------//

                case "F15136":  // CHECKED
                    // CLOSED P.R. REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15136");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15140":
                    // CREATED BY MADHVI ON 23RD JAN 2018
                    //OLD QUERY  SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,C.NAME AS MGNAME,A.DEPARTMENT,substr(TRIM(A.icode),1,2) AS MGCODE, A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT,A.ICODE,A.QTYORD,A.REMARK,A.PFLAG,A.ACODE,A.TYPE,A.INAME,A.UNIT,A.CPARTNO FROM PENDING_PR_VU A ,type C where  substr(TRIM(A.icode),1,2)=TRIM(C.TYPE1) AND C.ID='Y' and A.BRANCHCD='" + mbr + "' AND A.orddt " + xprdrange + " order by A.orddt";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15140");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15141":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15141");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15142":  // CHECKED
                    // PENDING P.O. REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15142");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15144":  // CHECKED
                    // PR/PO/MRR Work order no wise Report (Grip report ) 
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15144");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                //case "F15142":  // CHECKED
                //    // PENDING P.O. REGISTER
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15142");
                //    fgen.fin_purc_reps(frm_qstr);
                //    break;

                case "F15143"://PO VS MRR REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15143");
                    fgen.fin_purc_reps(frm_qstr);
                    break;


                case "F15251":
                    // SCHEDULE VS RECEIPT DAY WISE REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15251");
                    fgen.fin_purc_reps(frm_qstr);
                    break;


                case "F15222": // SCHEDULE VS RECEIPT DAY WISE REPORT
                case "F15223": // SCHEDULE VS RECEIPT TOTAL BASIS REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hfval.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15230":
                    // PRICE COMPARISON CHART VENDOR WISE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15230");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15231":
                    // PRICE COMPARISON CHART ITEM WISE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15231");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15232":
                    // PRICE COMPARISON CHART PLANT WISE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15232");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15235":
                    // MATL. CONSUMPTION REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15235");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15236":
                    // SUPPLIER,ITEM WISE 12 MONTH P.O. QTY REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15236");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15237":
                    // SUPPLIER,ITEM WISE 12 MONTH P.O. VALUE REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15237");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15238":
                    // DELIVERY DATE VS RECEIPT DATE REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15238");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15239":
                    // PO ITEM WITH RATE INC/DECREASE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15239");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15241":
                    // SUPPLIER HISTORY CARD
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15241");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15244":
                    // CLOSED P.O. REGISTER
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15244");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15247":
                    // SCHEDULE VS DESPATCH (QTY BASED)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15247");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15248":
                    // SCHEDULE VS DESPATCH (VALUE BASED)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15248");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15249":
                    // SCHEDULE VS DESPATCH (QTY + VALUE BASED)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15249");
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F15250":
                    // PENDING P.O. REGISTER WITHOUT LINE NO.
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15250");
                    fgen.fin_purc_reps(frm_qstr);
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

                //  //BY YOGITA
                case "F15240":
                    // PO ITEM WITH RATE INC/DECREASE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15240");
                    fgen.fin_purc_reps(frm_qstr);
                    break;
            }
        }
    }
}