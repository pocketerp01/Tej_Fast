using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_hrm : System.Web.UI.Page
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
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);//THIS LINE IS CMNT BY ME FOR VIEW EXPORT SO CHECKLIST
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F49132":
                case "F49133":
                case "F49134":
                    SQuery = "select TRIM(type1) as fstr,name,type1 as code from type where id='V' and type1='4F' ORDER BY code";
                    header_n = "Select Sale Type";
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
            if (val == "F49132")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            //THIS CASE ADD BY YOGITA
            else
            {
                switch (val)
                {
                    case "F49133":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Customers";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,A.ACODE AS CODE,B.ANAME AS NAME FROM somas A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.TYPE='4F' ORDER BY CODE"; //ONLY WAHI PARTY JINKA SO BNA HUA H
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
                    case "F49134":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Products";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(A.ICODE) AS FSTR,A.ICODE AS CODE,B.INAME AS NAME FROM somas A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.TYPE='4F' ORDER BY CODE";//ONLY WAHI ITEM JINKA SO BAN HUA H
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
                    default:
                        break;
                }
            }
            //END THIS CASE
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

            tbl_flds = fgen.seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                joinfld = tbl_flds.Split('@')[2].ToString();

                table1 = tbl_flds.Split('@')[3].ToString();
                table2 = tbl_flds.Split('@')[4].ToString();
                table3 = tbl_flds.Split('@')[5].ToString();
                table4 = tbl_flds.Split('@')[6].ToString();

                sortfld = sortfld.Replace("`", "'");
                joinfld = joinfld.Replace("`", "'");
                rep_flds = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }

            // after prdDmp this will run            
            switch (val)
            {
                case "F80126":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='10' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Training Need Identification Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F80127":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='20' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Training Done Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F80128":
                    SQuery = "select b.Name,b.Deptt_Text as Department,b.Desg_text as Designation,a.tr_name,sum(a.trainhrs) as Training_Hr_Reqd,sum(a.traindone) as Training_Hr_Done,sum(a.trainhrs)-sum(a.traindone) as GAP_Hours,a.empcode,a.branchcd,a.grade from (Select a.branchcd,a.grade,a.empcode,a.tr_name,a.trainhrs,0 as traindone from emptrain a where a." + branch_Cd + "  and a.type='10' and a.vchdate " + xprdrange + " union all Select a.branchcd,a.grade,a.empcode,a.tr_name,0 as thrs,a.trainhrs as traindone from emptrain a where a." + branch_Cd + "  and a.type='20' and a.vchdate " + xprdrange + ")a,empmas b where trim(A.branchcd)||trim(A.grade)||trim(A.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) group by b.Name,a.empcode,a.tr_name,a.branchcd,a.grade,b.Deptt_Text,b.Desg_text order by a.Tr_name,b.Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Training Need Vs Done Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F81126":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='LR' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Leave Request Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F81127":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='LR' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Leave Request Approval Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F83001":
                    SQuery = "Select a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,b.name,b.email,to_Char(a.hol_date,'dd/mm/yyyy') as target_dt,a.hol_title as title,a.ent_by as ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,(case when nvl(a.icat,'-')='Y' then 'Done' else 'Not Done' end) as status from payhol a,empmas b where substr(a.ucode,3,6)=trim(b.empcode) and a." + branch_Cd + " and a.type='RU' and a.vchdate " + xprdrange + " and length(trim(a.ucode))=8 union all Select a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,b.name,b.email,to_Char(a.hol_date,'dd/mm/yyyy') as target_dt,a.hol_title as title,a.ent_by as ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,(case when nvl(a.icat,'-')='Y' then 'Done' else 'Not Done' end) as status from payhol_bk a,empmas b where substr(a.ucode,3,6)=trim(b.empcode) and a." + branch_Cd + " and a.type='RU' and a.vchdate " + xprdrange + " and length(trim(a.ucode))=8";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Task List for the period " + value1 + " and " + value2 + "", frm_qstr);
                    break;
            }
        }
    }
}