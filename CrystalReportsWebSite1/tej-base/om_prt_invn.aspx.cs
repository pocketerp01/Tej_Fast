using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_invn : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;

    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string frm_qstr = "", frm_formID;
    string col1 = "";
    string frm_AssiID, frm_url;
    string frm_UserID, frm_PageName;
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
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
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
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                }
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
        fgen.send_cookie("REPLY", ""); i0 = 0;
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

                case "F25198A":
                case "F25198Bxx":
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '0%' order by type1";
                    header_n = "Select Matl. Inward Type";
                    break;
                case "F25198B":
                    SQuery = "select distinct TRIM(A.KCLREELNO) as fstr,b.Iname as PRODUCT,A.KCLREELNO AS BATCHNO,a.coreelno as vendorReelno,a.icode as erpcode from REELVCH a,ITEM b where a.branchcd='" + mbr + "' and a.type like '%' AND a." + "vchdate" + " " + xprdrange + " and  trim(a.Icode)=trim(B.IcodE) order by A.KCLREELNO";
                    header_n = "Select Batch/Reel/Roll Sticker";
                    i1 = 2;
                    break;
                case "F25141":
                case "F25242": // INWARD SUPPLIES WITH REJECTION
                    // MATL. INWARD REG
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '0%' order by type1";
                    header_n = "Select Matl. Inward Type";
                    i0 = 1;
                    break;

                case "F25142":
                    // MATL. OUTWARD REG
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '2%' order by type1";
                    header_n = "Select Matl. Outward Type";
                    i0 = 1;
                    break;

                case "F25143":
                    // MATL. ISSUE REG
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '3%' and type1<>'36' order by type1";
                    header_n = "Select Matl. Issue Type";
                    i0 = 1;
                    break;

                case "F25144":
                    // MATL. RETURN REG
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '1%' and type1<'15' order by type1";
                    header_n = "Select Matl. Return Type";
                    i0 = 1;
                    break;

                case "F15166":
                case "F25132":
                case "F25133":
                case "F25134":
                case "F25244L":
                case "F25244S":
                case "F25244T":
                case "F25244U":
                    SQuery = "";
                    fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
                    break;

                case "F25149":
                    // FG VALUATION
                    SQuery = "SELECT TYPE1 AS FSTR,NAME ,TYPE1  FROM TYPE WHERE TYPE1 LIKE '9%' AND ID='Y' ORDER BY TYPE1";
                    header_n = "Select Main Group";
                    i0 = 1;
                    break;

                case "F25139":
                case "F25244":
                case "F25236":
                case "F25236V":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F25194":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.vchnum, to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ent_by,a.type, b.name , C.NAME AS WNAME,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD  from ivoucher a , type b, TYPE C   where trim(a.ACODE)=trim(b.type1) AND TRIM(A.IOPR)= TRIM(C.TYPE1) and a.branchcd='" + mbr + "' and a.type='3A' and b.id='1'  and c.id='1' and a.acode='68'  and a.vchdate " + xprdrange + " ORDER BY VDD DESC,A.VCHNUM DESC";
                    header_n = "Select Entry No";
                    i1 = 2;
                    i0 = 1;
                    break;

                case "F25198":
                    SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,B.INAME AS PRODUCT,B.CPARTNO,A.ENT_BY FROM IVOUCHER A,ITEM B WHERE TRIM(a.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='15' AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM DESC,A.VCHDATE DESC ";
                    header_n = "Select Entry";
                    i1 = 2;
                    i0 = 1;
                    break;
                case "F25232":
                case "F25157":
                case "F25158":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F25241":
                    fgen.Fn_ValueBox("Enter the No. Of Days!! Since when the item has not moved", frm_qstr);
                    SQuery = "-";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
                    break;
                case "F25264"://TMI STOCK STATEMENT WFINSYS_ERP
                    SQuery = "select 'Bal' as fstr,'Do you want to see items with Bal Qty Only' as msg from dual union all select 'Nill' as fstr,'Do you want to include items with Nil Qty' as msg from dual";
                    break;

                case "F25262":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }

            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (i1 == 2) fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                if (i0 == 1) fgen.Fn_open_mseek(header_n, frm_qstr);
                else fgen.Fn_open_sseek(header_n, frm_qstr);
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F25242")
            {
                hfcode.Value = value1;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                fgen.Fn_open_PartyItemBox("", frm_qstr);
            }
            else if (val == "F25141" || val == "F25142" || val == "F25143" || val == "F25144")
            {
                // ADDED BY MADHVI ON 9TH APR 2018
                hfcode.Value = value1;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                fgen.Fn_open_Act_itm_prd("", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F25149":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", value1);
                            SQuery = "Select 'INAME' AS FSTR,'ITEM NAME' AS ITEM,'Y' AS OPT FROM DUAL UNION ALL SELECT 'ICODE' AS FSTR,'ITEM CODE' AS ITEM,'N' AS OPT FROM DUAL";
                            header_n = "Choose Order By";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;
                    case "F25241":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", value1);
                        fgen.Fn_open_dtbox("Select Date", frm_qstr);
                        break;
                    case "F25264":
                        hfcode.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;
                    case "F25198A":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                        SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as MRR_no,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Vendor,a.Invno,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,A.refnum as Chl_no,to_char(a.refdate,'dd/mm/yyyy') as chl_Dt,a.Genum as GE_No,to_char(a.gedate,'dd/mm/yyyy') as GE_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.pname as insp_by,a.finvno as vch_ref,to_Char(a.vchdate,'yyyymmdd') as vdd from IVOUCHER a,famst b where  a.branchcd='" + mbr + "' and a.type='" + value1 + "' AND a." + "vchdate" + " " + xprdrange + " and  trim(a.acode)=trim(B.acodE) and a.store!='R' order by vdd desc,a.vchnum desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                        break;
                    case "F25198Bxx":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                        SQuery = "select TRIM(A.KCLREELNO) as fstr,a.vchnum as MRR_no,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Iname as PRODUCT,A.KCLREELNO AS BATCHNO,to_Char(a.vchdate,'yyyymmdd') as vdd from REELVCH a,ITEM b where a.branchcd='" + mbr + "' and a.type='" + value1 + "' AND a." + "vchdate" + " " + xprdrange + " and  trim(a.Icode)=trim(B.IcodE) order by vdd desc,a.vchnum desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                        fgen.Fn_open_mseek(header_n, frm_qstr);
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
            string party_cd = "";
            string part_cd = "";
            switch (val)
            {
                case "F25133":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");


                    SQuery = "select month_name,count(*) as  tot_bas,count(*) as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Query Graph", "line", "Month Wise", "-", SQuery, "");
                    break;


                case "F15166":
                case "F25132":
                case "M03012":
                case "P15005B":
                case "P15005Z":
                case "F25141":
                case "F25142":
                case "F25143":
                case "F25144":
                case "F25242":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
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

            branch_Cd = "branchcd='" + mbr + "'";
            if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_BRANCH_CD", branch_Cd);

            // after prdDmp this will run            
            switch (val)
            {
                case "F25133": // STOCK LEDGER SUMMARY
                case "F25132":// STOCK LEDGER 
                case "F25134": // STOCK  SUMMARY with min max
                case "F25235":   // SHORT / EXCESS SUPPLIES                    
                case "F15166": // stock led from po approval screen
                case "F25244L":
                case "F25244S":
                case "F25244T":
                case "F25244U":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25240": // Group Item WISE consumption
                case "F25223":// Department wise Issue Comparison
                case "F25239":// Deptt Item WISE consumption
                case "F25238":  // gROUP ITEM WISE pO QTY
                case "F25242": // INWARD SUPPLIES WITH REJECTION
                case "F25237": // Supplier,Item Wise 12 Month Purch. Qty
                case "F25144":// RETURN REG
                case "F25142":// CHALLAN REG
                case "F25143": // ISSUE REG
                case "F25141": // MRR REG
                case "F25264"://TMI STOCK STATEMENT WFINSYS_ERP
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
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

                ////BY YOGITA 5/5/18
                case "F25222":
                    // Department wise Issue Summary                 
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F25222");
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25230":
                    // Department wise Issue Summary                 
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F25230");
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25147": // FG Stock Summary Item wise
                case "F25148": // FG Stock Summary HSN wise
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F25149":
                    // FG VALUATION
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F25149");
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25139":
                    // Only Pending (Qty & Value) RGP Wise
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F25139");
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25232":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F25232");
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25157": //JOB WORK REG
                case "F25158": //RGP VS MRR
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25271": //NIRM..PRAG REPORT.........MRR/CHLN... yogita 08.01.2019
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
                default:
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
            }
        }
    }
}