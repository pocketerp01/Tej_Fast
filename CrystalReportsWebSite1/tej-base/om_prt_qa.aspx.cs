using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_qa : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joincond;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    // this is a test
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
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F30224": // INWARD SUPPLIES WITH REJECTION
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '0%' order by type1";
                    header_n = "Select Matl. Inward Type";
                    i0 = 1;
                    break;

                case "F30133":
                    SQuery = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS INSP_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INSP_DT,A.TYPE,B.ANAME,A.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO  FROM INSPMST A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND  A.BRANCHCD='" + mbr + "' AND A.TYPE='40' AND A.SRNO='1' ORDER BY A.VCHNUM DESC";
                    header_n = "Select Entry No.";
                    i0 = 1;
                    break;

                case "BD":
                case "BT":
                case "BF":
                case "DPCV":
                    SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname as customer,a.type,a.cpartno as wo_no from inspvch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and (a.type='83'||a.type='93') and a.vchdate " + xprdrange + " order by  a.vchnum desc";
                    break;

                case "BV":
                    SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname as customer,a.type,a.cpartno as wo_no from inspvch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type='93' and a.vchdate " + xprdrange + " order by  a.vchnum desc";
                    break;

                case "F79155": //test certificate
                    fgen.Fn_ValueBox("Please Enter Valve Tag No.", frm_qstr);
                    break;

            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
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
            if (val == "F30225" || val == "F30226" || val == "F30227")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F30224":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                        fgen.Fn_open_PartyItemBox("", frm_qstr);
                        break;

                    case "F30133":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_qa_reps(frm_qstr);
                        break;

                    case "BD":
                    case "BT":
                    case "BV":
                    case "BF":
                    case "DPCV":
                    case "F79155":
                        mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = "";
                        mq1 = value1;
                        mq2 = mq1.Substring(0, 2);
                        mq3 = mq1.Substring(2, 1); //3rd value

                        switch (mq2)
                        {
                            case "GN":
                                mq4 = "00";
                                break;

                            case "PN":
                                mq4 = "03";
                                break;

                            case "PG":
                                mq4 = "04";
                                break;

                            case "SG":
                                mq4 = "07";
                                break;
                        }
                        // mq4 = fgen.seek_iname(frm_qstr, co_cd, "SELECT type1 FROM TYPE WHERE ID='B' and trim(poprefix)='" + mq2.ToUpper() + "' order by type1", "type1");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);// TAG VALUE
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", mq3);// TRIRD CHAR OF VALVE
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", mq4);// BRANCH CODE
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_qa_reps(frm_qstr);
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

                case "F79155"://test certificate
                    mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = "";
                    mq1 = value1;
                    mq2 = mq1.Substring(0, 2);
                    mq3 = mq1.Substring(2, 1); //3rd value
                    mq4 = fgen.seek_iname(frm_qstr, co_cd, "SELECT type1 FROM TYPE WHERE ID='B' and trim(poprefix)='" + mq2 + "'", "type1");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);//tag value
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", mq3);// Trird char
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", mq4);// branch code
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_qa_reps(frm_qstr);
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
                case "F30224":
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
            if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
            else branch_Cd = "branchcd='" + mbr + "'";

            // after prdDmp this will run            
            switch (val)
            {
                default:
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_qa_reps(frm_qstr);
                    break;

                case "F30222": // SUPPLIER HISTORY CARD                      
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15241");// SAME REPORT IS PRESENT IN PURC MODULE THAT'S WHY ID OF PURC MODULE IS PASSED
                    fgen.fin_purc_reps(frm_qstr);
                    break;

                case "F30224":
                    // INWARD SUPPLIES WITH REJECTION
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);// SAME REPORT IS PRESENT IN INVN MODULE THAT'S WHY ID OF INVN MODULE IS PASSED
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F25242");
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F30225":
                    // SUPPLIERS 12 MONTH REJECTION TREND
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F30225");
                    fgen.fin_qa_reps(frm_qstr);
                    break;

                case "F30226":
                    // SUPPLIERS 12 MONTH REJECTION TREND
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F30226");
                    fgen.fin_qa_reps(frm_qstr);
                    break;

                case "F30227":
                    // DEPTT, ITEM 12 MONTH LINE REJECTION TREND
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F30227");
                    fgen.fin_qa_reps(frm_qstr);
                    break;
            }
        }
    }
}