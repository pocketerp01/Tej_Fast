using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_cport : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID;
    string party_cd = "", part_cd = "";
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
                case "F79141":
                case "F79142":
                case "F79143":
                case "F79144": //set prd n....and change web action prt to view
                case "F79145"://set prd n....and change web action prt to view
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F79141" || HCID == "F79142")
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15127")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            #region
            //THIS ELSE STATEMENT ADD BY YOGITA 
            else
            {
                switch (val)
                {
                    default:
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;
                }
            }
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
                case "F79122":
                    header_n = "Despatch Report";
                    SQuery = "select a.branchcd,t.name as branch_name,a.Vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,a.pono,to_char(a.podate,'dd/mm/yyyy') as podate,b.icode,b.purpose as itemname,b.exc_57f4 as cpartno,b.iqtyout as qty,to_char(a.remvdate,'dd/mm/yyyy') as remvdate,a.remvtime,a.mode_tpt,a.mo_vehi as vehicle,drv_name,drv_mobile from sale a,ivoucher b,famst f,type t where TRIM(A.ACODE)=TRIM(f.acode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd!='DD' AND a.type like '4%' and a.type!='47' and a.vchdate " + xprdrange + " and a.acode='" + uname + "' order by a.branchcd,a.vchdate,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("" + header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F79141":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (party_cd.Trim().Length > 1)
                    {
                        cond = " and a.icode in (" + party_cd + ")";
                    }
                    else
                    {
                        cond = " and a.icode like '%'";
                    }
                    SQuery = "Select a.branchcd,t.name as branch_name,A.ordno as MSO_No,to_char(a.orddt,'dd/mm/yyyy') as MSO_Dt,b.aname as Customer,c.iname as Item_name,c.cpartno as CPARTNO,a.qtyord as MSO_QTY,a.irate as MSO_RATE,C.UNIT as UOM,a.pordno as PO_Number,A.ICODE as ERP_CODE,to_char(a.porddt,'dd/mm/yyyy') as PO_Dated,a.acode as Cust_Code,a.desc9 as Cust_Part_Name,a.cpartno as Cust_Part_Num,a.type as MSO_type,to_char(a.ent_dt,'DD/MM/YYYY') as Ent_Dt,a.ENt_by as Ent_by,to_char(a.orddt,'yyyyMMdd') as vdd from SOMASM  A , FAMST B, item c,type t where a.branchcd!='DD' and a.type like '4%' and a.type!='47' and  a.acode='" + uname + "' " + cond + " and a.orddt  " + xprdrange + "  and TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) and trim(a.branchcd)=trim(t.type1) and t.id ='B' order by vdd,a.ordno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Master SO Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F79142":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (party_cd.Trim().Length > 1)
                    {
                        cond = " and a.icode in (" + party_cd + ")";
                    }
                    else
                    {
                        cond = " and a.icode like '%'";
                    }
                    SQuery = "Select a.branchcd,t.name as branch_name,A.ordno as SO_No,to_char(a.orddt,'dd/mm/yyyy') as SO_Dt,b.aname as Customer,a.ciname as Item_name,c.cpartno as CPARTNO,a.qtyord as SO_QTY,a.irate as SO_RATE,a.pordno as PO_Number,C.UNIT as SO_UNIT,to_char(a.porddt,'dd/mm/yyyy') as PO_Dated,A.ICODE as ERP_CODE,a.acode as Cust_Code,a.desc9 as Cust_Part_Name,a.cpartno as Cust_Part_Num,a.icat as SO_Closed,a.App_by as Approve_By,to_char(a.app_dt,'DD/MM/YYYY') as Approve_Dt,a.type as SO_type,to_char(a.ent_dt,'DD/MM/YYYY') as Ent_Dt,a.ENt_by as Ent_by,to_char(a.orddt,'yyyyMMdd') as vdd from SOMAS  A , FAMST B, ITEM C,TYPE T where a.branchcd!='DD' and a.type like '4%' and a.type!='47' and a.acode='" + uname + "' " + cond + " and a.orddt  " + xprdrange + "  and TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) and trim(a.branchcd)=trim(t.type1) and t.id ='B'  order by vdd,a.branchcd,a.ordno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supply SO Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F79143":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (party_cd.Trim().Length > 1)
                    {
                        cond = " and a.icode in (" + party_cd + ")";
                    }
                    else
                    {
                        cond = " and a.icode like '%'";
                    }
                    // schedule
                    SQuery = "Select A.vchnum as Sch_No,to_char(a.vchdate,'dd/mm/yyyy') as Sch_Dt,b.aname as Customer,c.iname as Item_name,c.cpartno as CPARTNO,a.Total as Sch_Qty,a.irate as Sch_RATE,C.UNIT as SO_UNIT,A.ICODE as ERP_CODE,a.acode as Cust_Code,a.ENt_by as Ent_by,to_char(a.ent_dt,'DD/MM/YYYY') as Ent_Dt,to_char(a.vchdate,'yyyyMMdd') as vdd from SCHEDULE  A,FAMST B, ITEM C where a.branchcd!='DD' and a.type='46' and a.acode= '" + uname + "' " + cond + " and a.vchdate " + xprdrange + " and TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) order by vdd,a.vchnum";
                    // budgmst
                    SQuery = "select a.branchcd,t.name as branchcd_name,a.vchnum as sch_no,to_char(a.vchdate,'dd/mm/yyyy') as sch_dt,b.aname as customer,s.ciname as item_name,trim(c.cpartno) as cpartno,c.unit as so_unit,sum(a.budgetcost) as qty,a.icode as erp_code,a.acode as cust_code,to_char(a.vchdate,'yyyymmdd') as vdd from budgmst a,somas s,famst b, item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.solink)||trim(a.icode)||trim(a.acode)=trim(s.branchcd)||trim(s.type)||trim(s.ordno)||to_char(s.orddt,'dd/mm/yyyy')||trim(s.icode)||trim(s.acode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.branchcd!='DD' and a.type='46' and a.acode= '" + uname + "' " + cond + " and a.vchdate " + xprdrange + " GROUP BY A.BRANCHCD,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),b.aname,s.ciname,trim(c.cpartno),C.UNIT,A.ICODE,a.acode,to_char(a.vchdate,'yyyymmdd'),t.name order by vdd,a.vchnum ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supply Schedule Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F79144":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (party_cd.Trim().Length > 1)
                    {
                        cond = " and a.icode in (" + party_cd + ")";
                    }
                    else
                    {
                        cond = " and a.icode like '%'";
                    }
                    // schedule
                    SQuery = "Select A.branchcd AS BRANCHCD_CODE,T.NAME AS BRANCH_NAME,b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select a.branchcd, trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd!='DD' and a.type='46' and a.vchdate " + xprdrange + " and a.acode='" + uname + "' " + cond + " group by trim(a.acode),trim(A.icode),a.branchcd union all  Select a.branchcd, trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2 from ivoucher a where a.branchcd!='DD' and a.type like '4%' and a.type!='47' and a.vchdate " + xprdrange + " and a.acode='" + uname + "' " + cond + " group by trim(a.acode),trim(a.Icode),a.branchcd) a,famst b,item c,TYPE T where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and TRIM(a.branchcd)=trim(t.type1) and t.id='B' group by b.Aname,c.Iname,c.Unit,c.cpartno,trim(A.icode),trim(A.acode),a.branchcd,T.NAME Order by B.aname,c.Iname,T.NAME";
                    //budgmst
                    SQuery = "Select A.branchcd AS BRANCHCD_CODE,T.NAME AS BRANCH_NAME,b.Aname as Customer_Name,trim(a.cIname) as Part_Name,trim(c.cpartno) as Part_Number,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select a.branchcd, trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.budgetcost) as Prd1,0 as prd2,trim(s.ciname) as ciname from budgmst a,somas s where trim(a.solink)||trim(a.icode)||trim(a.acode)=trim(s.branchcd)||trim(s.type)||trim(s.ordno)||to_char(s.orddt,'dd/mm/yyyy')||trim(s.icode)||trim(s.acode) and a.branchcd!='DD' and a.type='46' and a.vchdate " + xprdrange + " and a.acode='" + uname + "' " + cond + " group by trim(a.acode),trim(A.icode),a.branchcd,trim(s.ciname) union all  Select a.branchcd, trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2,trim(a.purpose) as purpose from ivoucher a where a.branchcd!='DD' and a.type like '4%' and a.type!='47' and a.vchdate " + xprdrange + " and a.acode='" + uname + "' " + cond + " group by trim(a.acode),trim(a.Icode),a.branchcd,trim(a.purpose)) a,famst b,item c,TYPE T where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and TRIM(a.branchcd)=trim(t.type1) and t.id='B' group by b.Aname,trim(a.cIname),c.Unit,trim(c.cpartno),trim(A.icode),trim(A.acode),a.branchcd,T.NAME Order by branch_name,Part_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Schedule Vs Despatch Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F79145":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (party_cd.Trim().Length > 1)
                    {
                        cond = " and a.icode in (" + party_cd + ")";
                    }
                    else
                    {
                        cond = " and a.icode like '%'";
                    }
                    SQuery = "Select a.branchcd,t.name as branch_name,a.Ordno as SO_No,to_char(A.orddt,'dd/mm/yyyy') as SO_DT,b.Aname as Customer_Name,a.DESC9 as Part_Name,c.cpartno as Part_Number,a.qtyord as Order_Qty,a.Soldqty as Despatch_Qty,a.bal_qty as Pend_Qty,c.Unit,round(a.bal_qty*a.srate,2) as Pend_Value,a.Pordno as Cust_po_no,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode,to_chaR(a.orddt,'yyyymmdd') as VDD from wbvu_pending_so a, famst b,item c,type T where a.branchcd!='DD' and a.orddt " + xprdrange + " and trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' and a.acode='" + uname + "' " + cond + " and a.bal_qty>0 Order by VDD,a.ordno,B.aname,c.Iname,branch_name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Order Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
            }
        }
    }
}