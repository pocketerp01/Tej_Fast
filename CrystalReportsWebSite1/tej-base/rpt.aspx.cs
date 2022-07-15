using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class rpt : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, prdrange, fromdt, todt, mbr, branch_Cd, xprd1, cond;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string xprd2, opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;

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
                prdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            }

            hfhcid.Value = frm_formID;

            if (!Page.IsPostBack) show_data();
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
        if (HCID == "22141" || HCID == "F50024")
            fgen.msg("-", "CMSG", "Do you want to see consolidate report'13'(No for branch wise)");
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "89554":
                case "W90121":
                case "F20310":
                case "F20311":
                case "F20312":
                case "F20314":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "22610A":
                case "22610B":
                    fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
                    break;
                case "M03012":
                    SQuery = "select code AS FSTR,CODE,CODE AS S from co";
                    header_n = "Select Code";
                    break;
                case "F39131U":
                    SQuery = "SELECT A.USERID AS FSTR,A.USERNAME,A.USERID,A.ICONS AS BARCODE FROM EVAS A ORDER BY A.USERID";
                    break;
                case "F50148":
                    SQuery = "SELECT DISTINCT acode AS FSTR,aname,acode AS CODE FROM famst where substr(acode,1,2) in ('16') ORDER BY aname";
                    break;
                default:
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", HCID);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "F10283":
                case "F10284":
                    fgen.Fn_open_dtbox("-", frm_qstr);
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
            if (val == "M03012" || val == "F50148")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F39131U":
                        ds = new DataSet();
                        SQuery = "select userid,username,icons from evas where userid='" + value1 + "'";
                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);
                        dt1.TableName = "barcode";
                        dt1 = fgen.addBarCode(dt1, "icons", true);
                        ds.Tables.Add(dt1);
                        fgen.Print_Report_BYDS(co_cd, frm_qstr, mbr, "ustk", "ustk", ds, "");
                        break;
                    case "W90121":
                        hfcode.Value = value1;
                        if (hfcode.Value == "A")
                        {
                            header_n = "Completed Task";
                            SQuery = "select vchnum as Action_no,to_char(vchdate,'dd/mm/yyyy') as Action_dt,rtrim(xmlagg(xmlelement(e,reason||',')).extract('//text()').extract('//text()'),',') action_taken,to_date(col4,'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,col4 as complted_on,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and substr(col3,1,3)='[A]' and vchdate " + prdrange + " group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),col4,to_date(col4,'dd/mm/yyyy'),to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy')";
                            SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,ent_by||',')).extract('//text()').extract('//text()'),',') user_name,to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,col4 as priority,col14 as subject,remarks as message,COL28 as assign_by,COL29 as assign_dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 where branchcd='" + mbr + "' and type='TA' and substr(app_by,1,3)='[A]' and vchdate " + prdrange + " group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col4,col14,remarks ,COL28 ,COL29,to_char(vchdate,'yyyymmdd') ORDER BY VDD,entry_no";
                        }
                        else if (hfcode.Value == "R")
                        {
                            header_n = "Rejected Task";
                            SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(col4,'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,col4 as Rejected_on,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and substr(col3,1,3)='[R]' and vchdate " + prdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),col4,to_date(col4,'dd/mm/yyyy'),to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy')";
                            SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,col4 as priority,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt,to_char(vchdate,'yyyymmdd') as vdd from scratch where branchcd='" + mbr + "' and type='DK' and substr(col3,1,3)='[R]' and vchdate " + prdrange + " group by vchnum ,to_char(vchdate,'dd/mm/yyyy') ,to_char(docdate,'dd/mm/yyyy'),col4,col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by vdd,entry_no";
                        }
                        else if (hfcode.Value == "U")
                        {
                            header_n = "Task Pending For Approval";
                            SQuery = "select ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,trim(col14) as subject,trim(remarks) as message,'Pending' as Status,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate " + prdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy')";
                        }
                        else if (hfcode.Value == "V")
                        {
                            header_n = "Task Pending For Action Approval";
                            SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,ent_by||',')).extract('//text()').extract('//text()'),',') user_name,to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,col4 as priority,col14 as subject,remarks as message,COL28 as assign_by,COL29 as assign_dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 where branchcd='" + mbr + "' and type='TA' and nvl(trim(app_by),'-')='-' and vchdate " + prdrange + " group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col4,col14,remarks ,COL28 ,COL29,to_char(vchdate,'yyyymmdd') ORDER BY VDD,entry_no";
                        }
                        else if (hfcode.Value == "S")
                        {
                            header_n = "All Tasks";
                            SQuery = "select 'Completed Taks' as Status, vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(col4,'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,col4 as done_on,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and substr(col3,1,3)='[A]' and vchdate " + prdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),col4,to_date(col4,'dd/mm/yyyy'),to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') union all select 'Rejected Taks' as Status,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(col4,'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_date,col4 as Done_on,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where " + branch_Cd + " and type='DK' and substr(col3,1,3)='[R]' and vchdate " + xprdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),col4,to_date(col4,'dd/mm/yyyy'),to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') union all select 'Pending Taks' as Status,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_date,'NOT Done' as done_on,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where " + branch_Cd + " and type='DK' and nvl(col3,'-')='-' and vchdate " + xprdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy')";
                            SQuery = "select 'Completed Taks' as Status, vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_to_be_Completed_dt,col4 as priority,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch2 where branchcd='" + mbr + "' and type='TA' and substr(app_by,1,3)='[A]' and vchdate " + prdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),col4 union all select 'Rejected Taks' as Status,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(TO_CHAR(SYSDATE,'DD/MM/YYYY'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_date,col4 as PRIORITY,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and substr(col3,1,3)='[R]' and vchdate " + xprdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col4,col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy') union all select 'Pending Taks' as Status,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy') as due_days,to_char(docdate,'dd/mm/yyyy') as task_date,col4 as priority,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate " + xprdrange + " and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),to_date(to_char(docdate,'dd/mm/yyyy'),'dd/mm/yyyy'),col4";
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel(header_n, frm_qstr);
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
                case "S15115A":
                case "S15115B":
                case "S15115C":
                case "S15115D":
                case "S15115E":
                case "S15115F":
                case "S15115I":
                case "S06005D":
                case "S06005E":
                case "S06005C":
                case "S05005C":
                case "S07005B":
                case "S08005B":
                case "S09005B":
                case "S07005C":
                case "S08005C":
                case "S09005C":
                case "M02025A":
                case "M02025B":
                case "M10030A":
                case "M10030B":
                case "M10025A":
                case "M10025B":
                case "M11025A":
                case "M11025B":
                case "S07005D":
                case "F50024":
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
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

            tbl_flds = fgen.seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                table1 = tbl_flds.Split('@')[2].ToString();
                table2 = tbl_flds.Split('@')[3].ToString();
                table3 = tbl_flds.Split('@')[4].ToString();
                table4 = tbl_flds.Split('@')[5].ToString();
                sortfld = sortfld.Replace("`", "'");
                rep_flds = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }

            // after prdDmp this will run
            switch (val)
            {
                case "M02025B":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + " , " + table3 + " where a." + branch_Cd + "  and a.type like '5%' and " + datefld + " " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("P.o register for the Period " + value1 + " to " + value1, frm_qstr);
                    break;
                case "M02025A":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + "   where a." + branch_Cd + "  and a.type='60' and " + datefld + " " + xprdrange + " and trim(a.icode)=trim(b.icode)  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("P.R register for the Period " + value1 + " to " + value1, frm_qstr);
                    break;
                case "M10030A":
                    SQuery = "select month_name,round(sum(tot_bas)/100000,2) as tot_bas,round(sum(tot_qty)/100000,2) as tot_qty,mth from (select distinct substr(to_Char(a.orddt,'MONTH'),1,3) as Month_Name,(Basic) as tot_bas,(total) as tot_qty,to_Char(a.orddt,'YYYYMM') as mth,type||ordno||orddt  as fstr from somasp a where a." + branch_Cd + "  and a.type like '4%' and a.orddt " + xprdrange + " and substr(a.acode,1,2)!='02') group by month_name ,mth   order by mth";
                    fgen.Fn_FillChart(co_cd, frm_qstr, "PI Mthly Graph", "line", "Main Heading", "Sub Heading", SQuery, "");
                    break;
                case "M10030B":
                    SQuery = "select month_name,round(sum(tot_bas)/100000,2) as tot_bas,round(sum(tot_qty)/100000,2) as tot_qty,mth from (select distinct substr(to_Char(a.orddt,'MONTH'),1,3) as Month_Name,(Basic) as tot_bas,(total) as tot_qty,to_Char(a.orddt,'YYYYMM') as mth,type||ordno||orddt  as fstr from somas a where a." + branch_Cd + "  and a.type like '4%' and a.orddt " + xprdrange + " and substr(a.acode,1,2)!='02') group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "SO Mthly Graph", "line", "Main Heading", "Sub Heading", SQuery, "");
                    break;
                case "S06005E":
                    SQuery = "select month_name,count(*) as  tot_bas,count(*) as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";

                    //SQuery = "select month_name,tot_bas as tot_bas,tot_qty as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,count(*) as tot_bas,count(*) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Query Graph", "line", "Month Wise", "-", SQuery, "");
                    break;
                case "S06005D":
                    SQuery = "select qrytype as Qry_Type,qrytc as Qry_Code,sum(qrycnt) as Qry_Rcvd,sum(qryallt) as Qry_Alloted,sum(qrycnt)-sum(qryallt) as To_Allot from (select trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||lpad(trim(to_char(a.srno,'999')),3,'0') as fstr,qrytc,qrytype,1 as qrycnt,0 as qryallt from cquery_Reg a where a.branchcd!='DD' and type='CQ' and a.vchdate " + xprdrange + " union all select distinct qry_link,qrytc,qrytype,0 as aa,1 as qtycnt from cquery_alt where branchcd!='DD' and type='CA' and vchdate " + xprdrange + " ) group by qrytc,qrytype  order by qrytc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Query Rcvd vs Alloted for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "S15115I":
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                    fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                    fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                    fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("Gate Outward Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "S15115A":
                    //SQuery = "Select c.Name as CL_Name,d.name as Ef_Name,sum(a.whours) as Hours_Devoted,round(sum(a.whours*a.tmcost),2) as Cost_Incurred from cwork_rec a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.vchdate " + xprdrange + " group by c.Name,d.name order by c.Name";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Customer Wise,Effort Wise Summary for the Period of " + value1 + " to " + value2, frm_qstr);

                    fgen.drillQuery(0, "select TYPE1 as fstr,'-' as gstr,TYPE1 as code,name as name from TYPE where ID ='Z' order by type1", frm_qstr);
                    fgen.drillQuery(1, "select DISTINCT BSSCH AS FSTR,substr(bssch,1,2) as gstr,BSSCH as code1,aname as name,addr1 from famst ", frm_qstr);
                    fgen.drillQuery(2, "select ACODE as fstr,bssch as gstr,acode as code2,aname as name,addr2 from famst order by acode", frm_qstr);
                    fgen.drillQuery(3, "select acode as fstr,acode as gstr,acode as code3,aname as name,addr3 from famst order by acode", frm_qstr);

                    fgen.Fn_DrillReport("", frm_qstr);

                    break;
                case "S15115B":
                    SQuery = "Select c.Name as CL_Name,b.name as Team_Name,sum(a.whours) as Hours_Devoted,round(sum(a.whours*a.tmcost),2) as Cost_Incurred from cwork_rec a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.vchdate " + xprdrange + " group by c.Name,b.name order by c.Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise,Team Effort Summary for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "S15115C":
                    SQuery = "Select b.Name as TM_Name,d.name as Ef_Name,sum(a.whours) as Hours_Devoted,round(sum(a.whours*a.tmcost),2) as Cost_Incurred from cwork_rec a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.vchdate " + xprdrange + " group by b.Name,d.name order by b.Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Team Wise ,Effort Summary for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "S15115D":
                    SQuery = "Select to_char(a.vchdate,'YYYY MONTH') as Month_Ref,c.Name as CL_Name,sum(a.whours) as Hours_Devoted,round(sum(a.whours*a.tmcost),2) as Cost_Incurred,to_char(a.vchdate,'YYYYMM') as Sort_Mth  from cwork_rec a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.vchdate " + xprdrange + " group by c.Name,to_char(a.vchdate,'YYYY MONTH'),to_char(a.vchdate,'YYYYMM')  order by c.Name,to_char(a.vchdate,'YYYYMM') ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Monthly Effort Summary for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "S15115E":
                    SQuery = "Select to_char(a.vchdate,'YYYY MONTH') as Month_Ref,B.Name as TM_Name,c.Name as CL_Name,sum(a.whours) as Hours_Devoted,round(sum(a.whours*a.tmcost),2) as Cost_Incurred,to_char(a.vchdate,'YYYYMM') as Sort_Mth  from cwork_rec a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.vchdate " + xprdrange + " group by c.Name,b.Name,to_char(a.vchdate,'YYYY MONTH'),to_char(a.vchdate,'YYYYMM')  order by c.Name,to_char(a.vchdate,'YYYYMM'),b.name ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Team Wise Monthly Effort Summary for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "S15115F":
                    string wdays;
                    wdays = fgen.seek_iname(frm_qstr, co_cd, "select to_date('" + value2 + "','dd/mm/yyyy')-to_date('" + value1 + "','dd/mm/yyyy') as fstr from dual ", "fstr");

                    SQuery = "Select Cl_name as Client_Name,Eff_Name as Effort_Name,to_Char(" + wdays + "+1) as Tot_Days,sum(Hr_per_day) as Hr_per_day,sum(Budget_hours)as Budget_hours,sum(Hours_Devoted) as Hours_Devoted,sum(Budget_hours)-sum(Hours_Devoted) as Difference from (Select C.name as Cl_name,D.Name as Eff_Name,round(a.mtime*a.mtime1,2) as Hr_per_day,(a.mtime*a.mtime1*(" + wdays + "+1)) as Budget_hours,0 as Hours_Devoted from itwstage a,type c,type d where c.id2='CL' and d.id2='TS' and trim(a.icode)=trim(c.type1) and trim(a.stagec)=trim(d.type1) and a.type='WL' and a.branchcd!='DD'  union all Select C.name as Cl_name,D.Name as Eff_Name,0 as hrper_Day,0 as Budgets,sum(a.whours) as Hours_Devoted from cwork_rec a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.vchdate " + xprdrange + " group by C.name,D.Name) group by Cl_name,Eff_Name order by Cl_name,Eff_Name";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Team Wise Monthly Effort Summary for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;


                case "M10025A":
                    SQuery = "Select a.ORDNO as PI_No,to_char(a.oRDDT,'dd/mm/yyyy') as PI_Dt,decode(substr(trim(a.app_by),1,1),'-','(Un Approved)','(','(Cancel/Rej)','(Approved)') as Doc_Status,c.Aname as Customer,b.iname as Item_Name,b.cpartno as Part_No,a.qtyord as PI_Qty,a.irate as PI_Rate,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt,a.app_by,(Case when length(Trim(A.app_by))<2 then null else a.app_dt end) as App_Dt,a.Icat as Closed from somasp a, item b,famst c where a." + branch_Cd + "  and a.type like '4%' and a.type!='4F' and a.orddt " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a.orddt,a.ordno,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of P.I for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "M10025B":
                    SQuery = "Select a.ORDNO as SO_No,to_char(a.oRDDT,'dd/mm/yyyy') as SO_Dt,decode(substr(trim(a.app_by),1,1),'-','(Un Approved)','(','(Cancel/Rej)','(Approved)') as Doc_Status,c.Aname as Customer,b.iname as Item_Name,b.cpartno as Part_No,a.qtyord as SO_Qty,a.irate as SO_Rate,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt,a.app_by,(Case when length(Trim(A.app_by))<2 then null else a.app_dt end) as App_Dt,a.Icat as Closed  from somas a, item b,famst c where a." + branch_Cd + "  and a.type like '4%' and a.type!='4F' and a.orddt " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a.orddt,a.ordno,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of S.O for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "M11025A":
                    SQuery = "Select a.ORDNO as PI_No,to_char(a.oRDDT,'dd/mm/yyyy') as PI_Dt,decode(substr(trim(a.app_by),1,1),'-','(Un Approved)','(','(Cancel/Rej)','(Approved)') as Doc_Status,c.Aname as Customer,b.iname as Item_Name,b.cpartno as Part_No,a.qtyord as PI_Qty,a.irate as PI_Rate,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt,a.app_by,(Case when length(Trim(A.app_by))<2 then null else a.app_dt end) as App_Dt,a.Icat as Closed from somasp a, item b,famst c where a." + branch_Cd + "   and a.type='4F' and a.orddt " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a.orddt,a.ordno,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of P.I for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "M11025B":
                    SQuery = "Select a.ORDNO as SO_No,to_char(a.oRDDT,'dd/mm/yyyy') as SO_Dt,decode(substr(trim(a.app_by),1,1),'-','(Un Approved)','(','(Cancel/Rej)','(Approved)') as Doc_Status,c.Aname as Customer,b.iname as Item_Name,b.cpartno as Part_No,a.qtyord as SO_Qty,a.irate as SO_Rate,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt,a.app_by,(Case when length(Trim(A.app_by))<2 then null else a.app_dt end) as App_Dt,a.Icat as Closed  from somas a, item b,famst c where a." + branch_Cd + "  and a.type='4F' and a.orddt " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a.orddt,a.ordno,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of S.O for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S07005C":
                    SQuery = "Select nvl(a.acode,'-') as Client_Cd,a.alt_tgt as Allot_dt,a.qry_Wrkdt as Work_dt,a.QryTopic,a.Alt_rmk,a.Wrk_rmk,a.Qmark_name as Work_Tag,a.Ent_by,a.Ent_Dt from cquery_wrk a where a.branchcd!='DD' and a.type='10' and a.vchdate " + xprdrange + " ORDER BY a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Work Done By Support for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S07005D":
                    SQuery = "select a.acode as client_Cd,a.vchnum as Job_No,to_Char(A.vchdate,'dd/mm/yyyy') as Job_Dt,a.qrytopic as Qry_Topic,a.qryokay as Qry_Okay,a.Qryby,a.Qrydeptt,a.Last_Action,A.Ent_by,TO_CHAR(A.ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Clo_by,TO_CHAR(A.Clo_dT,'DD/mm/yyyy') as Clo_Dt,to_Char(a.vchdate,'yyyymmdd') as vdd from cquery_REg a where a.branchcd='" + mbr + "' and a.type='CQ' and a.vchdate " + xprdrange + "   order by vdd,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Work Done By Support for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S05005C":
                    SQuery = "select a.vchnum as Job_No,to_Char(A.vchdate,'dd/mm/yyyy') as Job_Dt,a.qrytopic as Qry_Topic,a.qryokay as Qry_Okay,a.Qryby,a.Qrydeptt,a.Last_Action,A.Ent_by,TO_CHAR(A.ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Clo_by,TO_CHAR(A.Clo_dT,'DD/mm/yyyy') as Clo_Dt,to_Char(a.vchdate,'yyyymmdd') as vdd from cquery_REg a where a.branchcd='" + mbr + "' and a.type='CQ' and a.vchdate " + xprdrange + " and a.ent_by='" + uname + "'  order by vdd,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Query Raised  for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S06005C":
                    SQuery = "select a.acode as client_Cd,a.vchnum as Job_No,to_Char(A.vchdate,'dd/mm/yyyy') as Job_Dt,a.qrytopic as Qry_Topic,a.qryokay as Qry_Okay,a.Qryby,a.Qrydeptt,a.Last_Action,A.Ent_by,TO_CHAR(A.ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Clo_by,TO_CHAR(A.Clo_dT,'DD/mm/yyyy') as Clo_Dt,to_Char(a.vchdate,'yyyymmdd') as vdd from cquery_REg a where a.branchcd='" + mbr + "' and a.type='CQ' and a.vchdate " + xprdrange + "   order by vdd,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of All Client Query Raised  for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S08005C":
                    SQuery = "Select nvl(a.acode,'-') as Client_Cd,a.alt_tgt as Allot_dt,a.qry_Wrkdt as Work_dt,a.QryTopic,a.Alt_rmk,a.Wrk_rmk,a.Qmark_name as Work_Tag,a.Ent_by,a.Ent_Dt from cquery_wrk a where a.branchcd!='DD' and a.type='20' and a.vchdate " + xprdrange + " ORDER BY a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Work Done By Mktg for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S09005C":
                    SQuery = "Select nvl(a.acode,'-') as Client_Cd,a.alt_tgt as Allot_dt,a.qry_Wrkdt as Work_dt,a.QryTopic,a.Alt_rmk,a.Wrk_rmk,a.Qmark_name as Work_Tag,a.Ent_by,a.Ent_Dt from cquery_wrk a where a.branchcd!='DD' and a.type='30' and a.vchdate " + xprdrange + " ORDER BY a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Work Done By Prog. for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;

                case "S07005B":
                    SQuery = "select qrytc as fst1,qrytype as Qry_Type,qrytc as Qty_Code,sum(qrycnt) as Qry_Cnt from (select qry_link as fstr,qmark_Cd as qrytc,qmark_Name as qrytype,1 as qrycnt from cquery_alt a where a.branchcd!='DD' and type='CA' and trim(nvl(a.clo_by,'-'))='-' and a.qmark_Cd='01' union all select distinct qry_link,qrytc,qrytype,-1 as qtycnt from cquery_wrk where branchcd!='DD' and type='10' and upper(Trim(Qmark_name)) like 'DONE%' ) group by trim(Fstr),qrytc,qrytype having sum(qrycnt)>0  order by qrytc";
                    SQuery = "Select nvl(a.acode,'-') as Client_Cd,a.QryType,a.QryModName,nvl(a.qrytopic,'-') as Qry_Topic,qryokay as Client_ok_if,nvl(a.qry_rmk,'-') as Allot_rmk,nvl(a.Qry_tgtdt,'-') as Allot_Tgt_Dt,a.Ent_by,a.Ent_Dt,a.Qry_Link as fstr from cquery_alt a,(" + SQuery + ") b where a.branchcd!='DD' and a.type='CA' and trim(a.Qmark_cd)='01' and trim(a.qry_link)=trim(b.fstr) ORDER BY a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Work Waiting for Support for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S08005B":
                    SQuery = "select qrytc as fst1,qrytype as Qry_Type,qrytc as Qty_Code,sum(qrycnt) as Qry_Cnt from (select qry_link as fstr,qmark_Cd as qrytc,qmark_Name as qrytype,1 as qrycnt from cquery_alt a where a.branchcd!='DD' and type='CA' and trim(nvl(a.clo_by,'-'))='-' and a.qmark_Cd='02' union all select distinct qry_link,qrytc,qrytype,-1 as qtycnt from cquery_wrk where branchcd!='DD' and type='20' and upper(Trim(Qmark_name)) like 'DONE%' ) group by trim(Fstr),qrytc,qrytype having sum(qrycnt)>0  order by qrytc";
                    SQuery = "Select nvl(a.acode,'-') as Client_Cd,a.QryType,a.QryModName,nvl(a.qrytopic,'-') as Qry_Topic,qryokay as Client_ok_if,nvl(a.qry_rmk,'-') as Allot_rmk,nvl(a.Qry_tgtdt,'-') as Allot_Tgt_Dt,a.Ent_by,a.Ent_Dt from cquery_alt a,(" + SQuery + ") b where a.branchcd!='DD' and a.type='CA' and trim(a.Qmark_cd)='02' and trim(a.qry_link)=trim(b.fstr) ORDER BY a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Work Waiting for Mktg for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;
                case "S09005B":
                    SQuery = "select qrytc as fst1,qrytype as Qry_Type,qrytc as Qty_Code,sum(qrycnt) as Qry_Cnt from (select qry_link as fstr,qmark_Cd as qrytc,qmark_Name as qrytype,1 as qrycnt from cquery_alt a where a.branchcd!='DD' and type='CA' and trim(nvl(a.clo_by,'-'))='-' and a.qmark_Cd='03' union all select distinct qry_link,qrytc,qrytype,-1 as qtycnt from cquery_wrk where branchcd!='DD' and type='30' and upper(Trim(Qmark_name)) like 'DONE%' ) group by trim(Fstr),qrytc,qrytype having sum(qrycnt)>0  order by qrytc";
                    SQuery = "Select nvl(a.acode,'-') as Client_Cd,a.QryType,a.QryModName,nvl(a.qrytopic,'-') as Qry_Topic,qryokay as Client_ok_if,nvl(a.qry_rmk,'-') as Allot_rmk,nvl(a.Qry_tgtdt,'-') as Allot_Tgt_Dt,a.Ent_by,a.Ent_Dt from cquery_alt a,(" + SQuery + ") b where a.branchcd!='DD' and a.type='CA' and trim(a.Qmark_cd)='03' and trim(a.qry_link)=trim(b.fstr) ORDER BY a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Work Waiting for Prog for the Period of " + value1 + " to " + value1, frm_qstr);
                    break;

                case "M03012":
                    SQuery = "select acode from famst where acode like '16%'";
                    // to open printout
                    fgen.Fn_Print_Report(co_cd, frm_qstr, mbr, SQuery, "famstTest", "famstTest");

                    // to open rptlevel
                    //fgen.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("", frm_qstr);

                    //to open rptlevel
                    //SQuery = "select code from co order by code";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("", frm_qstr);

                    break;
                case "F50024":
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, "Select distinct to_char(vchdate,'mm-yyyy') as vchd,to_char(vchdate,'yyyymm') as vdd from ivoucher where " + branch_Cd + " and vchdate " + xprdrange + " order by vdd");

                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        val = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + dr["vchd"].ToString() + "','mm-yyyy'),'Mon_YYYY') as vch from dual", "vch");
                        if (mq0.Length > 0) mq0 = mq0 + ", decode(to_Char(a.vchdate,'mm-yyyy'),'" + dr["vchd"].ToString() + "',sum(a.iqtyout),0) as " + val.ToString() + " ,decode(to_Char(a.vchdate,'mm-yyyy'),'" + dr["vchd"].ToString() + "',sum(a.iamount),0) as v" + val.ToString();
                        else mq0 = " decode(to_Char(a.vchdate,'mm-yyyy'),'" + dr["vchd"].ToString() + "',sum(a.iqtyout),0) as " + val.ToString() + " ,decode(to_Char(a.vchdate,'mm-yyyy'),'" + dr["vchd"].ToString() + "',sum(a.iamount),0) as v" + val.ToString();

                        if (mq1.Length > 0) mq1 = mq1 + " ,sum(" + val + ") as Q_" + val + " , sum(v" + val + ") as V_" + val;
                        else mq1 = " sum(" + val + ") as Q_" + val + " , sum(v" + val + ") as V_" + val;

                        if (mq2.Length > 0) mq2 = mq2 + "+" + val;
                        else mq2 = val;

                        if (mq3.Length > 0) mq3 = mq3 + " + " + "v" + val;
                        else mq3 = "v" + val;
                    }

                    if (mq0.Trim().Length > 0)
                    {
                        SQuery = "select aname as customer,mat1 as Technical_Partner,vehical_model, cpartno as part_no,iname as product," + mq1 + ", sum(" + mq2 + ") as net_qty,sum(" + mq3 + ") as net_sale_Value,(case when wt_cnc>0 then round(wt_cnc*sum(" + mq2 + "),3) else 0 end) as royl_cal_Q,(case when wt_rft>0 then round((wt_rft*sum(" + mq3 + "))/100,3) else 0 end) as royl_cal_V from ";
                        //fgen.send_cookie("sq1", SQuery);
                        SQuery = SQuery + " (Select b.aname ,d.iname as vehical_model, c.cpartno ,c.iname,c.mat1,c.wt_cnc,c.wt_rft , " + mq0 + " from ivoucher a,famst b,item c,item d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and substr(a.icode,1,4)=trim(d.icode) and a." + branch_Cd + " and a.type like '4%' and a.type not in ('47','45','48') and substr(trim(a.acode),1,2)!='02' and (c.wt_cnc>0 or c.wt_rft>0) and nvl(a.revis_no,'-') like '%' and a.vchdate " + xprdrange + " group by b.aname ,c.cpartno ,c.iname,to_Char(a.vchdate,'mm-yyyy'),d.iname,c.mat1,c.wt_cnc,c.wt_rft) group by aname ,cpartno ,vehical_model,iname,mat1,wt_cnc,wt_rft order by aname,vehical_model,iname";
                        //fgen.send_cookie("sq2", SQuery);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        if (co_cd == "NEOP") fgen.Fn_open_rptlevel("Commision Report for the period " + fromdt + " and " + todt + "", frm_qstr);
                        else fgen.Fn_open_rptlevel("Royality Report for the period " + fromdt + " and " + todt + "", frm_qstr);
                    }
                    else fgen.msg("-", "AMSG", "No Data Exist for selected time period");
                    break;

                case "F20310":
                    SQuery = "select col16 as Visitor,count(*) as total_Visits from (select distinct vchnum,col16 from scratch2 where " + branch_Cd + " and type='VM' and vchdate " + xprdrange + ") group by col16";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("Visitor Wise Report for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;

                case "F20311":
                    SQuery = "select distinct mth_yr,count(*) as total_Visitor,vdd as sorted_date from (select distinct vchnum,to_Char(vchdate,'MONth YYYY') as mth_yr,to_Char(vchdate,'YYYYMM') as vdd from scratch2 where " + branch_Cd + " and type='VM' and vchdate " + xprdrange + ") group by mth_yr,vdd order by vdd";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("Month Wise Report for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;

                case "F20312":
                    SQuery = "select STATUS,count(*) as REQUISITION from (select distinct '00' AS ID, vchnum,'PENDING' AS STATUS from scratch2 where type='VR' AND " + branch_Cd + " and vchdate " + xprdrange + " AND trim(NVL(app_by,'-')) = '-' UNION ALL select distinct '01' AS ID, vchnum,'REJECTED' AS STATUS from scratch2 where type='VR' AND " + branch_Cd + " and vchdate " + xprdrange + " AND SUBSTR(trim(app_by),1,3) = '[U]' UNION ALL select distinct '02' AS ID, vchnum,'APPROVED' AS STATUS from scratch2 where type='VR' AND " + branch_Cd + " and vchdate " + xprdrange + " AND SUBSTR(trim(app_by),1,3) <> '[U]' AND trim(NVL(app_by,'-')) <> '-' UNION ALL select distinct '03' AS ID, vchnum,'ALL' AS STATUS from scratch2 where type='VR' AND " + branch_Cd + " and vchdate " + xprdrange + ")  group by STATUS,ID||STATUS  ORDER BY ID||STATUS";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("Status Wise Report for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;

                case "F20314":
                    SQuery = "SELECT DISTINCT A.VCHNUM,to_char(B.docdate,'dd/mm/yyyy') as visit_date,A.col16 as visitor_name,A.col15 as comp_name,A.col17 as location,A.col12 as purpose,A.col1 as MEET_PERSON,A.COL7 as dept,A.COL37 AS TIMEIN,A.COL38 AS TIMEOUT,A.REASON AS REMARKS,A.col23 AS MOBILE,B.APP_BY,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD FROM SCRATCH2 A,SCRATCH2 B WHERE A.TYPE='VM' AND B.TYPE='VR' AND A.INVNO||TO_CHAR(A.INVDaTe,'DDMMYYYY')=B.VCHNUM||TO_CHAR(B.VCHDATE,'DDMMYYYY') AND  a." + branch_Cd + " and a.vchdate " + xprdrange + " order by VDD desc";
                    SQuery = "SELECT DISTINCT A.VCHNUM,to_char(A.VCHDATE,'dd/mm/yyyy') as visit_date,A.col16 as visitor_name,A.col15 as comp_name,A.col17 as location,A.col12 as purpose,A.col1 as MEET_PERSON,A.COL7 as dept,A.COL37 AS TIMEIN,A.COL38 AS TIMEOUT,A.REASON AS REMARKS,A.col23 AS MOBILE,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD FROM SCRATCH2 A WHERE A.TYPE='VM' AND  a." + branch_Cd + " and a.vchdate " + xprdrange + " order by VDD desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("Visitor Movement Report for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;

                case "W90121":
                    SQuery = "select 'U' as fstr,'Pending Only' as Status,'Only Show the Pending tasks' as Description from dual union all select 'A' as fstr,'Completed Only' as Status,'Only Show the Approved tasks' as Description from dual union all select 'R' as fstr,'Rejected Only' as Status,'Only Show the Rejected tasks' as Description from dual union all select 'S' as fstr,'Show All' as Status,'Show All tasks' as Description from dual";
                    SQuery = "select 'U' as fstr,'Pending Only' as Status,'Only Show the tasks Pending for Approval' as Description from dual union all select 'A' as fstr,'Completed Only' as Status,'Only Show the Approved tasks' as Description from dual union all select 'R' as fstr,'Rejected Only' as Status,'Only Show the Rejected tasks' as Description from dual union all select 'V' as fstr,'Pending For Action Approval' as Status,'Task Pending for Action taken Approval' as Description from dual union all select 'S' as fstr,'Show All' as Status,'Show All tasks' as Description from dual";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;
                case "FFF":

                    break;
                case "F10281":
                    fgen.drillQuery(0, "SELECT BRANCHCD||TYPE||TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,'-' AS GSTR,ENT_BY AS PERSON,COL2 AS TYPE,COL7 AS VISIT_PURPOSE,COL11 AS CHECKIN_DT_TIME,COL12 AS CHECKOUT_DT_TIME,COL3||'-'||COL4 AS REFDETAIL,SUM(NUM1) AS TOT_EXP,ANAME AS PARTY,COL9 AS ADDRESS,VCHNUM AS ENTRYNO,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD FROM EXP_BOOK WHERE " + branch_Cd + " AND TYPE='EB' AND VCHDATE " + xprdrange + " GROUP BY BRANCHCD||TYPE||TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY'),ENT_BY,COL2,COL7,COL11,COL12,COL3||'-'||COL4,COL9,VCHNUM,TO_CHAR(VCHDATE,'YYYYMMDD'),ANAME ORDER BY VDD DESC,VCHNUM DESC", frm_qstr);
                    fgen.drillQuery(1, "select '-' as fstr,BRANCHCD||TYPE||TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY') as gstr,ENT_BY AS PERSON,COL2 AS TYPE,COL7 AS VISIT_PURPOSE,COL11 AS CHECKIN_DT_TIME,COL12 AS CHECKOUT_DT_TIME,COL3||'-'||COL4 AS REFDETAIL,COL16 AS STATUS_REMARKS,COL10 AS EXP_HEAD,NUM1 AS EXPENSE,ANAME AS PARTY,COL9 AS ADDRESS,VCHNUM AS ENTRYNO,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD FROM EXP_BOOK WHERE " + branch_Cd + " AND TYPE='EB' AND VCHDATE " + xprdrange + " ORDER BY SRNO", frm_qstr);
                    fgen.Fn_DrillReport("Check-In Detail Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F10282":
                    fgen.drillQuery(0, "SELECT TRIM(A.COL3)||TRIM(A.COL4) AS FSTR,'-' AS GSTR,A.ANAME AS PARTY,A.COL3 AS LEADNO,A.COL4 AS LEADDT,B.INAME AS PRODUCT,B.CPARTNO AS PARTNO,SUM(A.NUM1) AS TOT_Expense FROM EXP_BOOK A,ITEM B WHERE TRIM(A.COL6)=TRIM(B.ICODE) AND A." + branch_Cd + " AND TYPE='EB' AND VCHDATE " + xprdrange + " AND NVL(COL3,'-')!='-' GROUP BY TRIM(A.COL3)||TRIM(A.COL4),A.COL3,A.COL4,B.INAME,B.CPARTNO,A.ANAME ORDER BY A.COL3", frm_qstr);
                    fgen.drillQuery(1, "select '-' as fstr,TRIM(COL3)||TRIM(COL4) as gstr,ENT_BY AS PERSON,COL2 AS TYPE,COL7 AS VISIT_PURPOSE,COL11 AS CHECKIN_DT_TIME,COL12 AS CHECKOUT_DT_TIME,COL3||'-'||COL4 AS REFDETAIL,ANAME AS PARTY,COL16 AS STATUS_REMARKS,COL10 AS EXP_HEAD,NUM1 AS EXPENSE,COL9 AS ADDRESS,VCHNUM AS ENTRYNO,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD FROM EXP_BOOK WHERE " + branch_Cd + " AND TYPE='EB' AND VCHDATE " + xprdrange + " ORDER BY SRNO", frm_qstr);
                    fgen.Fn_DrillReport("Lead Wise Expense Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F50148":
                    cond = "";
                    if (hfcode.Value.Length > 1) cond = "and trim(a.acode) like '" + hfcode.Value.Trim() + "%'";
                    SQuery = "select rtrim(xmlagg(xmlelement(e,a.vchnum||',')).extract('//text()').extract('//text()'),',') as inv_no,a.inv_dt,a.customer,a.location,a.no_of_pkg,a.disp_qty,a.gross_Wt,a.transporter,a.grno,a.tot_freight,a.frt_per_kg from (select a.vchnum,to_char(C.chldate,'dd/mm/yyyy') as inv_dt,c.aname as customer,UPPER(A.LOCATION) AS LOCATION,d.no_bdls as no_of_pkg,sum(a.iqtyout) as disp_qty,(d.weight) as gross_wt,d.INS_NO AS transporter,d.grno,sum(C.app_amt) as tot_freight,(case when sum(C.app_amt)>0 then round((sum(C.app_amt) / sum(a.iqtyout)),3) else 0 end) as frt_per_kg from ivoucher a,TPTDTL C,sale d where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=d.branchcd||D.type||trim(D.vchnum)||to_char(D.vchdate,'dd/mm/yyyy') AND a.branchcd||Trim(A.type)||TRIM(A.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')||Trim(A.ACODE)=c.branchcd||Trim(C.lcode)||TRIM(C.chlnum)||TO_CHAr(C.chldate,'DD/MM/YYYY')||Trim(C.BCODE) and a.type like '4%' AND C.TYPE='10' and a." + branch_Cd + " and a.vchdate " + xprdrange + " group by a.vchnum,to_char(C.chldate,'dd/mm/yyyy'),c.aname,UPPER(A.LOCATION),d.no_bdls,(d.weight) ,d.INS_NO,d.grno) a group by a.inv_dt,a.customer,a.location,a.no_of_pkg,a.disp_qty,a.gross_Wt,a.transporter,a.grno,a.tot_freight,a.frt_per_kg order by a.inv_dt";
                    if (SQuery.Length > 0)
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Outgoing Freigth Recording for the period " + fromdt + " and " + todt + "", frm_qstr);
                    }
                    else fgen.msg("-", "AMSG", "No Data Exist");
                    break;
                case "F10283":
                    SQuery = "SELECT USERNAME FROM (SELECT USERNAME,1 AS QTY FROM EVAS WHERE BRANCHCD='" + mbr + "' AND ALLOWIGRP NOT IN ('FINCOCD','LEAVE') UNION ALL SELECT DISTINCT ENT_BY,-1 AS QTY FROM EXP_BOOK WHERE BRANCHCD='" + mbr + "' AND TYPE='EB' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + value1 + "' AND NVL(TRIM(COL11),'-')!='-') GROUP BY USERNAME HAVING SUM(QTY)>0 ORDER BY USERNAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("No Check In Report on " + value1 + "", frm_qstr);
                    break;
                case "F10284":
                    SQuery = "SELECT ENT_BY,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS ENT_DT,ANAME AS PARTY,COL7 AS LOCATION,COL8 AS PURPOSE,COL9 AS ADDRESS,COL11 AS IN_TIME FROM EXP_BOOK WHERE BRANCHCD='" + mbr + "' AND TYPE='EB' AND TO_CHAR(ENT_DT,'DD/MM/YYYY')='" + value1 + "' AND COL11>'" + Convert.ToDateTime(value1).ToString("yyyy-MM-dd") + " 09:30:00' ORDER BY ENT_BY";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Late Coming Report on " + value1 + "", frm_qstr);
                    break;
            }
        }
    }
}