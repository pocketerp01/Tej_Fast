using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_esale : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;    
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
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F55141":
                case "F55142":
                case "F55143":
                    SQuery = "select  type1 as fstr,name ,type1  from type where type1='4F' AND ID='V' ORDER BY TYPE1";
                    header_n = "Select Sale Type";
                    break;

                case "F55145":
                    SQuery = "Select Distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, trim(a.vchnum) as invoice_no, to_char(a.vchdate,'dd/mm/yyyy') as inv_date,a.type,a.acode as party_code,b.aname as party_name from ivoucher a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type='4F' and a.vchdate " + xprdrange + "";
                    header_n = "Select Export Invoice";
                    break;

                case "F55146": //FOR SAGE ONLY...PACKING LIST
                    SQuery = "SELECT distinct TRIM(A.BRANCHCD)||trim(a.type)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS PL_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS PL_DT,A.COL9 AS INV_NO,A.COL10 AS INV_DT,B.ANAME AS CUSTOMER  FROM SCRATCH A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='PL' and a.vchdate " + xprdrange + " order by a.vchnum desc";
                    header_n = "Select Packing List No.";
                    break;

                case "F55161": //FOR CMPL ONLY...PACKING LIST
                    SQuery = "SELECT distinct TRIM(A.BRANCHCD)||trim(a.type)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS PL_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS PL_DT,A.COL9 AS INV_NO,A.COL10 AS INV_DT,B.ANAME AS CUSTOMER  FROM SCRATCH A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='PL' and a.vchdate " + xprdrange + " order by a.vchnum desc";
                    header_n = "Select Packing List No.";
                    break;
                case "F55514":
                    SQuery = "select distinct trim(licno)||to_char(licdt,'dd/mm/yyyy')||trim(ciname) as fstr, trim(licno) as license_no,to_char(licdt,'dd/mm/yyyy') as license_dt ,trim(ciname) as item_desc ,trim(ent_by) as ent_by from wb_licrec where branchcd='" + mbr + "' and  type='20'";
                    break;
                case "F55515":
                    SQuery = "select distinct trim(licno)||to_char(licdt,'dd/mm/yyyy')||trim(term) as fstr, trim(licno) as license_no,to_char(licdt,'dd/mm/yyyy') as license_dt ,trim(term) as item_desc ,trim(ent_by) as ent_by from wb_licrec where branchcd='" + mbr + "' and  type='30' ";
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
            if (val == "F55141")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            #region this region add by yogita
            //THIS ELSE STATEMENT ADD BY YOGITA 
            else
            {
                switch (val)
                {
                    case "F55142":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Customers";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,A.ACODE as code,B.ANAME as name FROM ivoucher A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.TYPE='4F' order by code"; 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hfcode.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;
                    case "F55143":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Products";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT trim(A.ICODE) AS FSTR,A.ICODE as code,B.INAME as name FROM ivoucher A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.TYPE='4F' order by code";////ONLY WAHI ITEM JINKA SO BAN HUA H
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hfcode.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F55145":
                    case "F55146":
                        hfcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_esales_reps(frm_qstr);
                        break;

                    case "F55161"://PACKING LIST FOR CMPL                 
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_esales_reps(frm_qstr);
                        break;
                    case "F55514"://
                    case "F55515"://
                        hfcode.Value = value1;
                        hfbr.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hfbr.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                        fgen.Fn_open_dtbox("", frm_qstr);
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
            }
        }
        else
        { 
            // ADD BY MADHVI FOR SHOWING THE DATE RANGE WHEN USER PRESS ESC 
            fgen.Fn_open_prddmp1("-", frm_qstr);
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
                case "F55141":
                
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_esales_reps(frm_qstr);
                    break;

                case "F55142":
                case "F55143":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_esales_reps(frm_qstr);
                    break;

                // LOAD REORT FAILED ERROR IS COMING

                //case "F55141":
                //    mq0 = "select trim( to_char(to_date('" + fromdt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + fromdt.Substring(6, 4) + "','yyyy'),'yyyy')) as FRMDATE  from dual";
                //    mq1 = fgen.seek_iname(frm_qstr, co_cd, mq0, "FRMDATE");
                //    mq2 = "select trim( to_char(to_date('" + todt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + todt.Substring(6, 4) + "','yyyy'),'yyyy')) as TODATE  from dual";
                //    mq3 = fgen.seek_iname(frm_qstr, co_cd, mq2, "TODATE");
                //    SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + mbr + "'  AND a.type='" + hfcode.Value + "' AND A.vchdate " + xprdrange + "  ORDER BY A.SRNO";
                //    fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "std_Sale_REG", "std_Sale_REG");
                //    break;

                //case "F55142":
                //    mq0 = "select trim( to_char(to_date('" + fromdt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + fromdt.Substring(6, 4) + "','yyyy'),'yyyy')) as FRMDATE  from dual";
                //    mq1 = fgen.seek_iname(frm_qstr, co_cd, mq0, "FRMDATE");
                //    mq2 = "select trim( to_char(to_date('" + todt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + todt.Substring(6, 4) + "','yyyy'),'yyyy')) as TODATE  from dual";
                //    mq3 = fgen.seek_iname(frm_qstr, co_cd, mq2, "TODATE");
                //    if (hfcode.Value.Length > 0)
                //    {
                //        SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + mbr + "'  AND a.type='" + hf1.Value + "' AND A.vchdate " + xprdrange + "AND A.ACODE in (" + hfcode.Value + ")  ORDER BY A.SRNO";
                //    }
                //    else
                //    {
                //        SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + mbr + "'  AND a.type='" + hf1.Value + "' AND A.vchdate " + xprdrange + "AND A.ACODE like '%'  ORDER BY A.SRNO";
                //    }
                //    fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "std_Sale_REG", "std_Sale_REG");
                //    break;

                //case "F55143":
                //    mq0 = "select trim( to_char(to_date('" + fromdt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + fromdt.Substring(6, 4) + "','yyyy'),'yyyy')) as FRMDATE  from dual";
                //    mq1 = fgen.seek_iname(frm_qstr, co_cd, mq0, "FRMDATE");
                //    mq2 = "select trim( to_char(to_date('" + todt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + todt.Substring(6, 4) + "','yyyy'),'yyyy')) as TODATE  from dual";
                //    mq3 = fgen.seek_iname(frm_qstr, co_cd, mq2, "TODATE");
                //    if (hfcode.Value.Length > 0)
                //    {
                //        SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + mbr + "'  AND a.type='" + hf1.Value + "' AND A.vchdate " + xprdrange + "AND A.ICODE in (" + hfcode.Value + ")  ORDER BY A.SRNO";
                //    }
                //    else
                //    {
                //        SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + mbr + "'  AND a.type='" + hf1.Value + "' AND A.vchdate " + xprdrange + "AND A.ICODE like '%'  ORDER BY A.SRNO";
                //    }
                //    fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "std_Sale_REG", "std_Sale_REG");
                //    break;
                case "F55514":
                case "F55515":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hfbr.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    hfval.Value = value1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_esales_reps(frm_qstr);
                    break;               
            }
        }
    }
}