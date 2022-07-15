using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class rpt_DevA : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld;
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
        if ((HCID == "22141" || HCID == "F15132" || HCID == "F20121" || HCID == "F20126" || HCID == "F20131" || HCID == "F20132" || HCID == "F61121" || HCID == "F61126" || HCID == "F61131" || HCID == "F45121" || HCID == "F45126" || HCID == "F45131" || HCID == "F60126" || HCID == "F60131" || HCID == "F60132" || HCID == "F60133" || HCID == "F60134" || HCID == "F60135" || HCID == "F60136" || HCID == "F60137" || HCID == "F96121" || HCID == "F96126" || HCID == "F96131" || HCID == "F97121") || HCID == "F92121" || HCID == "F92126" || hfaskBranch.Value == "Y")
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
                case "P12001C":
                case "P18005A":
                case "P18005C":
                case "P18005E":
                case "P15005A":
                case "P15005C":
                case "P15005E":
                case "P15005G":
                case "P15005I":
                case "P15005K":
                case "P15005M":
                case "P15005M1":
                case "P15005O":
                case "P15005Q":
                case "P15005S":
                case "P15005U":
                case "P15005W":
                case "P17005E":
                case "P17005G":
                case "P17005I":
                case "S07005D":
                case "P15005X":
                case "P15005T":
                case "P15005D":
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
                case "F60131A":
                    SQuery = "Select username as fstr,username as compcode,full_name as comp_name from evas where length(trim(username))<=4 order by username";
                    hf1.Value = "";
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F60131A*")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            if (val == "F94121")
            {
                //vvvvvv
                string filePath = value1.Substring(value1.ToUpper().IndexOf("UPLOAD"), value1.Length - value1.ToUpper().IndexOf("UPLOAD"));

                Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                Session["FileName"] = filePath.Split('~')[1].ToString().Trim();
                Response.Write("<script>");
                Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                Response.Write("</script>");
            }
            if (val == "F60131A" && hf1.Value == "REP")
            {
                if (hfcode.Value.Length > 1) cond = " and trim(ccode)='" + hfcode.Value + "'";
                dt3 = new DataTable();
                dt3.Columns.Add("Information");
                dt3.Columns.Add("Time_Period");
                dt3.Columns.Add("Qnty");
                dt3.Columns.Add("Cost_Taken");
                dt3.Columns.Add("Amount");

                DataRow dr3;

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, co_cd, "SELECT FMDATE,TODATE FROM CO WHERE CODE LIKE '" + co_cd + "%' ");
                SQuery = "";
                branch_Cd = " branchcd='" + mbr + "'";
                foreach (DataRow dr in dt.Rows)
                {
                    if (SQuery == "")
                        SQuery = "SELECT COUNT(*) AS NUMB,'" + Convert.ToDateTime(dr["fmdate"]).ToString("dd MMM yyyy") + " - " + Convert.ToDateTime(dr["todate"]).ToString("dd-MMM-yyyy") + "' as Time_Period from WB_CSS_LOG  where " + branch_Cd + " and type='CS' " + cond + " and cssdt between to_Date('" + Convert.ToDateTime(dr["fmdate"]).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_Date('" + Convert.ToDateTime(dr["todate"]).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') ";
                    else SQuery += " UNION ALL SELECT COUNT(*) AS NUMB,'" + Convert.ToDateTime(dr["fmdate"]).ToString("dd MMM yyyy") + " - " + Convert.ToDateTime(dr["todate"]).ToString("dd-MMM-yyyy") + "' as Time_Period from WB_CSS_LOG  where " + branch_Cd + " and type='CS' " + cond + " and cssdt between to_Date('" + Convert.ToDateTime(dr["fmdate"]).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_Date('" + Convert.ToDateTime(dr["todate"]).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, co_cd, "SELECT * FROM (" + SQuery + ") WHERE NUMB>0");
                double cost = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Numb"].ToString().toDouble() > 0)
                    {
                        dr3 = dt3.NewRow();
                        dr3["Information"] = "Call Centre - Recorded Calls";
                        dr3["Time_Period"] = dr["Time_Period"];
                        dr3["Qnty"] = dr["Numb"];
                        dr3["Cost_Taken"] = value1;
                        dr3["Amount"] = Math.Round(dr3["Qnty"].ToString().toDouble() * dr3["Cost_Taken"].ToString().toDouble(), 2);
                        cost += dr3["Amount"].ToString().toDouble();
                        dt3.Rows.Add(dr3);
                    }
                }

                dr3 = dt3.NewRow();
                dr3["Information"] = "Unrecorded calls";
                dr3["Time_Period"] = "";
                dr3["Qnty"] = "0";
                dr3["Cost_Taken"] = 600;
                dr3["Amount"] = Math.Round(dr3["Qnty"].ToString().toDouble() * dr3["Cost_Taken"].ToString().toDouble(), 2);
                //cost += dr3["Cost_Taken"].ToString().toDouble();
                dt3.Rows.Add(dr3);

                dr3 = dt3.NewRow();
                dr3["Information"] = "Statutory Changes service";
                dr3["Time_Period"] = "";
                dr3["Qnty"] = "0";
                dr3["Cost_Taken"] = 800;
                dr3["Amount"] = Math.Round(dr3["Qnty"].ToString().toDouble() * dr3["Cost_Taken"].ToString().toDouble(), 2);
                //cost += dr3["Cost_Taken"].ToString().toDouble();
                dt3.Rows.Add(dr3);

                dr3 = dt3.NewRow();
                dr3["Cost_Taken"] = "Total Cost";
                dr3["Amount"] = cost;
                dt3.Rows.Add(dr3);

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                Session["send_dt"] = dt3;
                //SQuery = "SELECT a.CSSNO as CSS_NO,to_char(A.CSsDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,b.ALLOWIGRP as client_type,a.dir_comp,a.Emodule as Module_Name,a.Eicon as Option_Name,a.Remarks,a.Req_type,a.Iss_type as Issue_Type,a.Cont_name,a.Cont_No,a.Cont_Email,a.ent_by,a.Ent_Dt,last_Action,last_Actdt,a.wrkrmk,a.app_by,a.app_dt,a.root as root_cause,a.corrective as corrective_action,a.preventive as preventive_action_suggestion,a.solvedby,a.start_time,a.end_time,a.time_Taken,to_chaR(a.CSSDT,'YYYYMMDD') as CSS_DTd FROM WB_CSS_LOG a left outer join evas b on trim(a.ccode)=trim(B.USERNAME) where a." + branch_Cd + " and a.type='CS' and a.cssdt " + xprdrange + " " + cond + " order by a.cssno ";
                //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevelJS("Report of Customer Complain Action During " + value1 + " to " + value2, frm_qstr);
            }
            if (val == "F60131A" && hf1.Value != "REP")
            {
                hf1.Value = "REP";
                hfcode.Value = value1;
                fgen.Fn_ValueBox("Please Enter Cost of single Call", frm_qstr);
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
                case "P12001C":
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    frm_AssiID = fgen.seek_iname(frm_qstr, co_cd, "select trim(Acode) as acode from proj_mast where branchcd!='DD' and type='P7' and log_ref='" + frm_UserID + "'", "acode");
                    cond = ""; mq0 = "";
                    mq0 = ",a.ment_by as ent_By";
                    mq1 = ",a.ment_by";
                    if (fgen.make_double(ulvl) > 1)
                    {
                        cond = " and trim(a.ment_by)='" + frm_UserID + "' ";
                        mq0 = "";
                        mq1 = "";
                    }
                    SQuery = "SELECT Proj_name as NAME,Dpc_no,ASGeName as Assignee_Name,Assgn_dt,Assgn_time,Est_hrs,Target_Dt,Remarks1,Remarks2,Ment_by as Assigned_By,to_char(Vchdate,'yyyymmdd') as Ta_dt from proj_asgn where " + branch_Cd + " " + cond + " order by Ta_dt,to_Char(vchdate,'dd/mm/yyyy')||vchnum||type||branchcd";
                    SQuery = "Select DISTINCT a.vchnum as task_record_no,b.proj_name as project,b.ment_By as assinor,a.ustart_dt as start_dt,a.ustart_time as start_time,a.uend_dt as end_dt,a.uend_time as end_time,is_number(round((round(24*(to_date(uend_Dt||' '||uend_time,'dd/mm/yyyy hh24:mi')-to_date(ustart_Dt||' '||ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as hr_workd,c.name as status " + mq0 + " from proj_updt a,proj_asgn b,proj_mast c where trim(a.projcode)=trim(b.pjcode) and trim(a.stcode)=trim(c.acode) and a." + branch_Cd + " " + cond + " and length(uend_time)>2 and c.type='P5'  order by a.vchnum desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Assigned Tasks for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005X":
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    mq0 = ",b.ment_by as ent_By";
                    mq1 = ",B.ment_by";
                    if (fgen.make_double(ulvl) > 1)
                    {
                        cond = " and trim(b.ment_by)='" + frm_UserID + "' ";
                        mq0 = "";
                        mq1 = "";
                    }
                    SQuery = "SELECT DISTINCT A.VCHNUM AS ASSIGNMENT_NO,a.proj_name as project,a.asgename as assignor,a.assgn_dt,a.assgn_time,a.given_hr as est_hr,is_number(round((round(24*(to_date(uend_Dt||' '||uend_time,'dd/mm/yyyy hh24:mi')-to_date(ustart_Dt||' '||ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as hr_workd,c.name as status " + mq0 + " from proj_asgn a,proj_updt b,proj_mast c where trim(a.pjcode)=trim(b.projcode) and trim(b.stcode)=trim(c.acode) and a." + branch_Cd + " " + cond + " and c.type='P5' and length(B.uend_time)>2  order by a.vchnum ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Assigned Tasks for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005Y":
                    SQuery = "SELECT A.PROJCODE AS PROJ_NO,A.Proj_name AS PROJECT,B.MILESTONE,C.NAME AS DEPTT,b.EST_HRs AS TOTAL_HRS,b.GIVEN_Hr AS MS_HRS,is_number(round(sum(round(24*(to_date(a.uend_Dt||' '||a.uend_time,'dd/mm/yyyy hh24:mi')-to_date(a.ustart_Dt||' '||a.ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as wrkd_hrs FROM PROJ_UPDT A,PROJ_aSGN B,PROJ_MAST C WHERE TRIM(A.PROJCODE)||trim(a.milestonecode)=TRIM(B.PJCODE)||trim(b.milestonecode) AND TRIM(B.DPCODE)=TRIM(C.ACODE) AND C.TYPE='P8' and A." + branch_Cd + " group by A.PROJCODE,A.Proj_name,B.MILESTONE,C.NAME,b.EST_HRs,b.GIVEN_Hr order by a.projcode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Project wise budgeted Revenue Vs. Actual Revenue for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005A":
                    SQuery = "Select to_char(vchdate,'yyyy MON') as Mth_Name,Proj_name,Asgename,round(sum(a.utime*60)/60,2) as Act_hrs ,/*round(sum(a.utime*a.uhrcost),2) as Costs_incurred,*/ to_char(vchdate,'yyyyMM') as Mth_No from proJ_updt a where a." + branch_Cd + "  and a.type like 'UP%' and a.vchdate " + xprdrange + " group by to_char(vchdate,'yyyy MON'),to_char(vchdate,'yyyyMM'),Proj_name,Asgename order by to_char(vchdate,'yyyyMM')";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Man Hour Utilization for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005C":
                    SQuery = "Select to_char(vchdate,'yyyy MON') as Mth_Name,Proj_name,Asgename,round(sum(a.utime*60)/60,2) as billed_hrs,round(sum(a.utime*a.uhrcost),2) as Billed_Costs,to_char(vchdate,'yyyyMM') as Mth_No from proJ_updt a where a." + branch_Cd + "  and a.type like 'UP%' and a.vchdate " + xprdrange + " group by to_char(vchdate,'yyyy MON'),to_char(vchdate,'yyyyMM'),Proj_name,Asgename order by to_char(vchdate,'yyyyMM')";
                    SQuery = "Select to_char(a.vchdate,'yyyy MON') as Mth_Name,b.Name as Proj_Name,round(sum(a.JOBUPS),2) as billed_hrs,round(sum(a.JOBUPS*a.actualcost),2) as Billed_Costs,to_char(a.vchdate,'yyyyMM') as Mth_No from BUDGMST a, proj_dtl b where a.branchcd||trim(a.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy')=b.branchcd||trim(b.vchnum)||to_Char(b.vchdate,'dd/mm/yyyy') and a." + branch_Cd + "  and a.type like 'BB%' and a.vchdate " + xprdrange + " group by to_char(a.vchdate,'yyyy MON'),to_char(a.vchdate,'yyyyMM'),b.name order by to_char(a.vchdate,'yyyyMM')";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Billed Hours Report the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005E":
                    SQuery = "Select to_char(vchdate,'yyyy MON') as Mth_Name,Proj_name,Asgename,round(sum(a.Utime*60)/60,2) as Billed_hrs,round(sum(a.utime*60)/60,2) as Act_hrs,to_char(vchdate,'yyyyMM') as Mth_No from proJ_updt a where a." + branch_Cd + "  and a.type like 'UP%' and a.vchdate " + xprdrange + " group by Proj_name,Asgename,to_char(vchdate,'yyyy MON'),to_char(vchdate,'yyyyMM') order by to_char(vchdate,'yyyyMM')";
                    SQuery = "Select to_char(b.vchdate,'yyyy MON') as Mth_Name,b.Proj_name,Asgename,round(sum(a.JOBUPS),2) as Billed_hrs,round(sum(b.utime*60)/60,2) as Act_hrs,to_char(b.vchdate,'yyyyMM') as Mth_No from budgmst a,proJ_updt b where trim(A.vchnum)=trim(b.projcode) and a." + branch_Cd + " and a.type='BB' and b.type like 'UP%' and b.vchdate " + xprdrange + " group by b.Proj_name,b.Asgename,to_char(b.vchdate,'yyyy MON'),to_char(b.vchdate,'yyyyMM') order by to_char(b.vchdate,'yyyyMM')";
                    SQuery = "select a.mth_name,a.proj_name,a.Asgename,a.act_hrs,sum(b.jobups) as billed_hrs,a.mth_no from (Select b.projcode,to_char(b.vchdate,'yyyy MON') as Mth_Name,b.Proj_name,b.Asgename,round(sum(b.utime*60)/60,2) as Act_hrs,to_char(b.vchdate,'yyyyMM') as Mth_No from proJ_updt b where b." + branch_Cd + " and b.type like 'UP%' and b.vchdate " + xprdrange + " group by b.Proj_name,b.Asgename,b.projcode,to_char(b.vchdate,'yyyy MON'),to_char(b.vchdate,'yyyyMM')) a,budgmst b where trim(a.projcode)=trim(b.vchnum) and b.type='BB' group by a.mth_name,a.proj_name,a.Asgename,a.act_hrs,a.mth_no order by mth_name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Resource Efficiency for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005G":
                    SQuery = "Select to_char(a.vchdate,'yyyy MON') as Mth_Name,a.Proj_name,b.Name as Down_Time_reason,round(sum(a.dt_hrs*60)/60,0) as DT_hrs,round(sum(a.dt_hrs*a.uhrcost),0) as DT_cost,to_char(a.vchdate,'yyyyMM') as Mth_No from proj_dtime a ,proj_mast b where trim(a.dtcode)=trim(b.acode) and b.type='P4' and a." + branch_Cd + "  and a.type like 'UP%' and a.vchdate " + xprdrange + " group by a.Proj_name,b.Name,to_char(a.vchdate,'yyyy MON'),to_char(a.vchdate,'yyyyMM') order by to_char(a.vchdate,'yyyyMM')";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Down Time Analysis for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005I":
                    SQuery = "select trim(vchnum) as Proj_code,trim(Name) as Proj_name,max(end_Dt) as Proj_Dlv_Date,sum(proj_cost) as Budget_cost,sum(act_cost) as Act_Cost,sum(proj_cost)-sum(act_cost) as Bal_Budget,round(((sum(act_cost)) /sum(proj_cost))*100,2) as Budget_percent, sum(proj_hrs) as Budget_hrs,sum(act_hrs) as Act_Hrs,sum(proj_hrs)-sum(act_hrs) as Balance_Hrs,to_char(max(lastupdt),'dd/mm/yyyy') as Last_update from (select a.vchnum,a.Name,a.end_Dt,a.proj_cost,a.proj_hrs,0 as act_cost,0 as act_hrs,null as lastupdt from proj_dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.projcode,a.proj_name,null as ent_dt,0 as proj_cost,0 as proj_hrs,a.utime*a.uhrcost as actcost,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Name) order by trim(vchnum)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Project Status for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005K":
                    SQuery = "select trim(vchnum) as Proj_code,trim(Name) as Proj_name,max(end_Dt) as Proj_Dlv_Date,sum(proj_hrs) as Budget_hrs,sum(act_hrs) as Act_Hrs,sum(proj_hrs)-sum(act_hrs) as Balance_Hrs,to_char(max(lastupdt),'dd/mm/yyyy') as Last_update from (select a.vchnum,a.Name,a.end_Dt,a.proj_cost,a.proj_hrs,0 as act_cost,0 as act_hrs,null as lastupdt from proj_dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.projcode,a.proj_name,null as ent_dt,0 as proj_cost,0 as proj_hrs,a.utime*a.uhrcost as actcost,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Name) order by trim(vchnum)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Budget Vs Actual Hrs for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005U":
                    SQuery = "select trim(vchnum) as Assgn_No,max(Asgn_by) as Assign_By,trim(Proj_Name) as Proj_name,max(Asgename) as Assign_to,max(Assgn_Dt) as Assign_Date,sum(est_hrs) as Estimated_hrs,sum(act_hrs) as Act_Hrs,sum(est_hrs)-sum(act_hrs) as Balance_Hrs,to_char(max(lastupdt),'dd/mm/yyyy') as Last_update from (select a.vchnum,a.ment_by as Asgn_by,a.Proj_Name,a.Asgename,a.Assgn_Dt,a.est_hrs,0 as act_hrs,null as lastupdt from proj_asgn a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.tavchnum,null as Asgn_by,a.proj_name,null as Asgename,null as ent_dt,0 as proj_hrs,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Proj_Name) order by trim(vchnum)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Estimated Vs Actual Hrs for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;

                case "P15005W":
                    SQuery = "SELECT b.Name AS YR,sum(a.dt_hrs) AS DT_Hours,sum(a.dt_hrs*a.uhrcost) AS DT_Amount FROM proj_dtime A,proj_mast b  WHERE a.BRANCHCD!='DD' and b.type='P4' and trim(a.dtcode)=trim(b.acode) and a.vchdate " + xprdrange + "  GROUP BY b.Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Down Time (Reaons Wise) for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005M":
                    SQuery = "select trim(vchnum) as Proj_code,trim(Name) as Proj_name,max(end_Dt) as Proj_Dlv_Date,sum(proj_hrs) as Budget_hrs,sum(act_hrs) as Billed_Hrs,sum(proj_hrs)-sum(act_hrs) as Balance_Hrs,to_char(max(lastupdt),'dd/mm/yyyy') as Last_update from (select a.vchnum,a.Name,a.end_Dt,a.proj_cost,a.proj_hrs,0 as act_cost,0 as act_hrs,null as lastupdt from proj_dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.projcode,a.proj_name,null as ent_dt,0 as proj_cost,0 as proj_hrs,a.utime*a.uhrcost as actcost,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Name) order by trim(vchnum)";
                    SQuery = "Select a.pjcode as code,a.milestone,b.proj_name as project,a.vchdate as date_,is_number(round(sum(round(24*(to_date(b.uend_Dt||' '||b.uend_time,'dd/mm/yyyy hh24:mi')-to_date(b.ustart_Dt||' '||b.ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as wrkd_hrs,sum(is_numbeR(a.est_hrs)) as budgt_hrs,sum(c.JOBUPS) as billed_hrs from proj_Asgn a,proj_updt b,budgmst c where trim(A.milestonecode)||trim(a.pjcode)=trim(b.milestonecode)||trim(b.projcode) and trim(A.pjcode)=trim(c.vchnum) and a." + branch_Cd + " and a.vchdate " + xprdrange + " and c.type='BB' group by a.pjcode,b.proj_name,a.vchdate,a.milestone order by a.pjcode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Budget Vs Billed Hrs for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005M1":
                    SQuery = "SELECT A.PROJCODE AS PROJ_NO,A.Proj_name AS PROJECT,B.MILESTONE,C.NAME AS DEPTT,b.EST_HRs AS TOTAL_HRS,b.GIVEN_Hr AS MS_HRS,is_number(round(sum(round(24*(to_date(a.uend_Dt||' '||a.uend_time,'dd/mm/yyyy hh24:mi')-to_date(a.ustart_Dt||' '||a.ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as wrkd_hrs FROM PROJ_UPDT A,PROJ_aSGN B,PROJ_MAST C WHERE TRIM(A.PROJCODE)||trim(a.milestonecode)=TRIM(B.PJCODE)||trim(b.milestonecode) AND TRIM(B.DPCODE)=TRIM(C.ACODE) AND C.TYPE='P8' and A." + branch_Cd + " group by A.PROJCODE,A.Proj_name,B.MILESTONE,C.NAME,b.EST_HRs,b.GIVEN_Hr order by a.projcode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Budgeted Hours to reach milestone Vs Actual Hours for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005T":
                    SQuery = "SELECT A.PROJCODE AS PROJ_NO,A.Proj_name AS PROJECT,C.NAME AS DEPTT,B.MILESTONE,b.remarks1 as activity,b.vchdate as assign_dt,b.ment_by as assinor,b.ASGeName as assignee,sum(b.EST_HRs) AS TOTAL_HRS,is_number(round(sum(round(24*(to_date(a.uend_Dt||' '||a.uend_time,'dd/mm/yyyy hh24:mi')-to_date(a.ustart_Dt||' '||a.ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as wrkd_hrs FROM PROJ_UPDT A,PROJ_aSGN B,PROJ_MAST C,proj_mast d WHERE TRIM(A.PROJCODE)||trim(a.milestonecode)=TRIM(B.PJCODE)||trim(b.milestonecode) AND TRIM(B.DPCODE)=TRIM(C.ACODE) and trim(a.stcode)=trim(d.acode) AND C.TYPE='P8' and d.type='P5' and A." + branch_Cd + " group by A.PROJCODE,A.Proj_name,C.NAME,B.MILESTONE,b.remarks1,b.vchdate,b.ment_by,b.ASGeName order by a.PROJCODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Activity Report for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005D":
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, "Select vchnum,UPPER(name) AS NAME,srno from proj_mast where type='PD' order by vchnum,srno");
                    mq0 = ""; mq1 = "";
                    foreach (DataRow dtr in dt.Rows)
                    {
                        if (mq0.Length > 0) mq0 = mq0 + ", " + "DECODE(UPPER(B.DESC_),'" + dtr["name"].ToString().Trim() + "',REQ_CL_RSN,'-') AS S_" + dtr["name"].ToString().Trim().Replace(" ", "_").Replace("/", "").Replace("-", "_");
                        else mq0 = "DECODE(UPPER(B.DESC_),'" + dtr["name"].ToString().Trim() + "',REQ_CL_RSN,'-') AS S_" + dtr["name"].ToString().Trim().Replace(" ", "_").Replace("/", "").Replace("-", "_");

                        if (mq1.Length > 0) mq1 = mq1 + ", " + "MAX(S_" + dtr["name"].ToString().Trim().Replace(" ", "_").Replace("/", "").Replace("-", "_") + ") AS S_" + dtr["name"].ToString().Trim().Replace(" ", "_").Replace("/", "").Replace("-", "_");
                        else mq1 = "MAX(S_" + dtr["name"].ToString().Trim().Replace(" ", "_").Replace("/", "").Replace("-", "_") + ") AS S_" + dtr["name"].ToString().Trim().Replace(" ", "_").Replace("/", "").Replace("-", "_");
                    }
                    SQuery = "SELECT CODE,PROJECT," + mq1 + " FROM (SELECT A.VCHNUM AS CODE,A.NAME AS PROJECT, " + mq0 + " FROM PROJ_DTL A,BUDGMST B WHERE A.BRANCHCD||TRIM(a.VCHNUM)||TO_cHAr(A.VCHDATE,'DD/MM/YYYY')=B.BRANCHCD||TRIM(B.VCHNUM)||TO_cHAR(B.VCHDATE,'DD/MM/YYYY') AND A." + branch_Cd + " AND B.TYPE='PD' AND A.VCHDATE " + xprdrange + " ) GROUP BY CODE,PROJECT ORDER BY CODE DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Project Documentation Status Report for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005O":
                    SQuery = "select trim(vchnum) as Proj_code,trim(Name) as Proj_name,max(end_Dt) as Proj_Dlv_Date,sum(proj_hrs) as Budget_hrs,sum(act_hrs) as Act_Hrs,sum(proj_hrs)-sum(act_hrs) as Balance_Hrs,to_char(max(lastupdt),'dd/mm/yyyy') as Last_update from (select a.vchnum,a.Name,a.end_Dt,a.proj_cost,a.proj_hrs,0 as act_cost,0 as act_hrs,null as lastupdt from proj_dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.projcode,a.proj_name,null as ent_dt,0 as proj_cost,0 as proj_hrs,a.utime*a.uhrcost as actcost,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Name) order by trim(vchnum)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Productivity for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005Q":
                    SQuery = "select trim(vchnum) as Proj_code,trim(Name) as Proj_name,max(end_Dt) as Proj_Dlv_Date,sum(proj_cost) as Budget_cost,sum(act_cost) as Act_Cost,sum(proj_cost)-sum(act_cost) as Bal_Budget,round(((sum(proj_cost)-sum(act_cost)) /sum(proj_cost))*100,2) as Profit_percent from (select a.vchnum,a.Name,a.end_Dt,a.proj_cost,a.proj_hrs,0 as act_cost,0 as act_hrs,null as lastupdt from proj_dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.projcode,a.proj_name,null as ent_dt,0 as proj_cost,0 as proj_hrs,a.utime*a.uhrcost as actcost,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Name) order by trim(vchnum)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Profitability for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005S":
                    SQuery = "select trim(vchnum) as Proj_code,trim(Name) as Proj_name,max(end_Dt) as Proj_Dlv_Date,sum(proj_hrs) as Budget_hrs,sum(act_hrs) as Act_Hrs,sum(proj_hrs)-sum(act_hrs) as Balance_Hrs,round(((sum(act_hrs)) /sum(proj_hrs))*100,2) as Budget_Hr_Consumed, to_char(max(lastupdt),'dd/mm/yyyy') as Last_update from (select a.vchnum,a.Name,a.end_Dt,a.proj_cost,a.proj_hrs,0 as act_cost,0 as act_hrs,null as lastupdt from proj_dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.projcode,a.proj_name,null as ent_dt,0 as proj_cost,0 as proj_hrs,a.utime*a.uhrcost as actcost,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Name) order by trim(vchnum)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Performance for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P17005E":
                    // open graph
                    SQuery = "Select Asgename,round(sum(a.utime*60)/60,2) as billed_hrs,round(sum(a.utime*a.uhrcost),2) as Billed_Costs,Asgecode from proJ_updt a where a." + branch_Cd + "  and a.type like 'UP%' and a.vchdate " + xprdrange + " group by Asgename,Asgecode order by Asgecode";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Utilization Graph", "column", "Assignee Wise", "-", SQuery, "");
                    break;
                case "P17005G":
                    // open graph
                    SQuery = "select trim(Name) as Proj_name,round(((sum(proj_cost)-sum(act_cost)) /sum(proj_cost))*100,2) as Profit_percent,sum(proj_cost) as Budget_cost,sum(act_cost) as Act_Cost  from (select a.vchnum,a.Name,a.end_Dt,a.proj_cost,a.proj_hrs,0 as act_cost,0 as act_hrs,null as lastupdt from proj_dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " union all select a.projcode,a.proj_name,null as ent_dt,0 as proj_cost,0 as proj_hrs,a.utime*a.uhrcost as actcost,a.utime,a.vchdate from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + ") group by trim(vchnum),trim(Name) order by trim(vchnum)";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Project Wise Performance", "column", "Project Wise", "-", SQuery, "");
                    break;

                case "P17005I":
                    // open graph
                    SQuery = "select b.Name as month_name,sum(a.tot_bas) as  tot_bas,sum(a.tot_qty) as tot_qty,trim(A.dtcode) as Dtcode from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,dt_hrs as tot_bas,dt_hrs*uhrcost as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,dtcode from proj_Dtime a where a." + branch_Cd + "  and a.vchdate " + xprdrange + " ) a,proj_mast b where trim(a.dtcode)=trim(B.acode) and b.type='P4' group by b.Name ,trim(A.dtcode) order by b.Name";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Reason Wise Down Time Graph", "pie", "Reason Wise", "-", SQuery, "");
                    break;
                case "P18005A":
                    SQuery = "Select a.vchnum as Proj_no,to_char(a.vchdate,'dd/mm/yyyy') as Proj_Dt,a.Name,a.Start_dt,a.End_Dt,a.Proj_refno,A.Proj_hrs,a.acode,a.Ment_by,a.Ment_Dt,a.Mapp_by,(Case when length(Trim(A.Mapp_by))<2 then null else a.Mapp_dt end) as App_Dt from proj_Dtl a where a." + branch_Cd + " and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Project Log Book During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P18005C":

                    SQuery = "Select a.vchnum as Assign_no,to_char(a.vchdate,'dd/mm/yyyy') as Assign_Dt,a.Proj_name,a.AsgeName,a.Est_hrs,a.Assgn_Dt,A.Assgn_Time,a.Target_dt,a.Alert_Dt,a.IAC_Filled,a.Ment_by,a.Ment_Dt,a.Mapp_by,(Case when 3(Trim(A.Mapp_by))<2 then null else a.Mapp_dt end) as App_Dt from proj_asgn a where a." + branch_Cd + " and a.vchdate " + xprdrange + "  order by a.vchdate,a.vchnum,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Task Assignment Log Book During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "P18005E":
                    SQuery = "Select a.Vchnum as Updt_no,to_char(a.vchdate,'dd/mm/yyyy') as Updt_Dt,a.Proj_Name,a.Ustart_dt,a.Ustart_time,A.Uend_dt,a.Uend_time,A.Utime,a.Cad_submit,a.Drg_Submit,a.Ment_by,a.Ment_Dt from proj_updt a where a." + branch_Cd + " and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Task Updates Log Book During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "M10025A":
                    SQuery = "Select a.ORDNO as PI_No,to_char(a.oRDDT,'dd/mm/yyyy') as PI_Dt,decode(substr(trim(a.app_by),1,1),'-','(Un Approved)','(','(Cancel/Rej)','(Approved)') as Doc_Status,c.Aname as Customer,b.iname as Item_Name,b.cpartno as Part_No,a.qtyord as PI_Qty,a.irate as PI_Rate,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt,a.app_by,(Case when length(Trim(A.app_by))<2 then null else a.app_dt end) as App_Dt,a.Icat as Closed from somasp a, item b,famst c where a." + branch_Cd + "  and a.type like '4%' and a.type!='4F' and a.orddt " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a.orddt,a.ordno,a.srno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of P.I for the Period of " + value1 + " to " + value2, frm_qstr);
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

                case "S06005E":
                    // open graph
                    SQuery = "select month_name,count(*) as  tot_bas,count(*) as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Query Graph", "line", "Month Wise", "-", SQuery, "");
                    break;
                case "S06005D":
                    // open normal rptlevel
                    SQuery = "select qrytype as Qry_Type,qrytc as Qry_Code,sum(qrycnt) as Qry_Rcvd,sum(qryallt) as Qry_Alloted,sum(qrycnt)-sum(qryallt) as To_Allot from (select trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||lpad(trim(to_char(a.srno,'999')),3,'0') as fstr,qrytc,qrytype,1 as qrycnt,0 as qryallt from cquery_Reg a where a.branchcd!='DD' and type='CQ' and a.vchdate " + xprdrange + " union all select distinct qry_link,qrytc,qrytype,0 as aa,1 as qtycnt from cquery_alt where branchcd!='DD' and type='CA' and vchdate " + xprdrange + " ) group by qrytc,qrytype  order by qrytc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Query Rcvd vs Alloted for the Period of " + value1 + " to " + value2, frm_qstr);
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
                case "P15005B":
                    if (hfcode.Value == "") return;
                    SQuery = "Select DISTINCT a.vchnum as task_record_no,b.proj_name as project,b.ment_By as assinor,a.ustart_dt as start_dt,a.ustart_time as start_time,a.uend_dt as end_dt,a.uend_time as end_time,is_number(round((round(24*(to_date(uend_Dt||' '||uend_time,'dd/mm/yyyy hh24:mi')-to_date(ustart_Dt||' '||ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as hr_workd,c.name as status from proj_updt a,proj_asgn b,proj_mast c where trim(a.projcode)=trim(b.pjcode) and trim(a.stcode)=trim(c.acode) and a." + branch_Cd + " and length(uend_time)>2 and c.type='P5' and b.dpcode='" + hfcode.Value + "' order by a.vchnum desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Parameter wise Report for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "P15005Z":
                    if (hfcode.Value == "") return;
                    //Resource
                    if (hfcode.Value == "1")
                    {
                        header_n = "Resource wise Worked Hours";
                        SQuery = "";
                    }
                    //Department
                    if (hfcode.Value == "2")
                    {
                        header_n = "Department wise Worked Hours";
                        SQuery = "Select  a.vchnum as task_record_no,b.proj_name as project,b.ment_By as assinor,a.ustart_dt as start_dt,a.ustart_time as start_time,a.uend_dt as end_dt,a.uend_time as end_time,is_number(round(sum(round(24*(to_date(uend_Dt||' '||uend_time,'dd/mm/yyyy hh24:mi')-to_date(ustart_Dt||' '||ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as hr_workd,c.name as status from proj_updt a,proj_asgn b,proj_mast c where trim(a.projcode)=trim(b.pjcode) and trim(a.stcode)=trim(c.acode) and a." + branch_Cd + " and length(uend_time)>2 and c.type='P5' /*and b.dpcode='" + hfcode.Value + "'*/ group by a.vchnum ,b.proj_name ,b.ment_By ,a.ustart_dt ,a.ustart_time ,a.uend_dt ,a.uend_time,c.name order by a.vchnum desc ";
                    }
                    //Project
                    if (hfcode.Value == "3")
                    {
                        header_n = "Project wise Worked Hours";
                        SQuery = "SELECT A.PROJCODE AS PROJ_NO,A.Proj_name AS PROJECT,B.MILESTONE,C.NAME AS DEPTT,b.EST_HRs AS TOTAL_HRS,b.GIVEN_Hr AS MS_HRS,is_number(round(sum(round(24*(to_date(a.uend_Dt||' '||a.uend_time,'dd/mm/yyyy hh24:mi')-to_date(a.ustart_Dt||' '||a.ustart_time,'dd/mm/yyyy hh24:mi')),2))-0,2)) as wrkd_hrs FROM PROJ_UPDT A,PROJ_aSGN B,PROJ_MAST C WHERE TRIM(A.PROJCODE)||trim(a.milestonecode)=TRIM(B.PJCODE)||trim(b.milestonecode) AND TRIM(B.DPCODE)=TRIM(C.ACODE) AND C.TYPE='P8' and A." + branch_Cd + " group by A.PROJCODE,A.Proj_name,B.MILESTONE,C.NAME,b.EST_HRs,b.GIVEN_Hr order by a.projcode";
                    }
                    //BU
                    if (hfcode.Value == "4")
                    {
                        header_n = "BU wise Worked Hours";
                        SQuery = "";
                    }
                    //Resource & Project
                    if (hfcode.Value == "5")
                    {
                        header_n = "Resource wise & Project wise Worked Hours";
                        SQuery = "";
                    }
                    //Resource wise Activity
                    if (hfcode.Value == "6")
                    {
                        header_n = "Resource wise & Activity wise Worked Hours";
                        SQuery = "";
                    }
                    //Department wise & Project Wise
                    if (hfcode.Value == "7")
                    {
                        header_n = "Department wise & Project wise Worked Hours";
                        SQuery = "";
                    }
                    //Resource wise Actual
                    if (hfcode.Value == "8")
                    {
                        header_n = "Resource wise Actual Hrs vs Estimate Hrs";
                        SQuery = "";
                    }
                    //Reason wise downtime analysis
                    if (hfcode.Value == "9")
                    {
                        header_n = "Resource wise Downtime";
                        SQuery = "";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("" + header_n + " for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F15131":
                    SQuery = "select a.ordno as PR_no,to_char(a.orddt,'dd/mm/yyyy') as PR_Dt,b.Name as Deptt_Name,c.Iname as Item_Name,C.Cpartno,a.delv_item as Reqd_by,a.qtyord as PR_Qty,c.Unit,a.splrmk as End_use,a.doc_thr as Item_Make,a.Prate as Approx_rt,a.Desc_ as Remarks,a.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.app_by,(Case when length(trim(nvl(a.app_by,'-')))<=1 then '-' else to_char(a.app_Dt,'dd/mm/yyyy') end) as app_dt,to_Char(a.orddt,'yyyymmdd') as vdd,a.srno from pomas a,type b,item c where trim(A.acode)=trim(B.type1) and b.id='M' and trim(A.icode)=trim(c.icode) and a." + branch_Cd + " and A.TYPE='60' AND A.orddt " + xprdrange + " order by vdd ,a.ordno ,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Purchase Requests for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F15132":
                    SQuery = "select a.ordno as PO_no,to_char(a.orddt,'dd/mm/yyyy') as PO_Dt,b.AName as Supplier,c.Iname as Item_Name,C.Cpartno,a.delv_item as Reqd_by,a.qtyord as PR_Qty,c.Unit,a.Desc_ as Remarks,a.splrmk as End_use,a.doc_thr as Item_Make,a.Prate as Approx_rt,a.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.app_by,(Case when length(trim(nvl(a.app_by,'-')))<=1 then '-' else to_char(a.app_Dt,'dd/mm/yyyy') end) as app_dt,to_Char(a.orddt,'yyyymmdd') as vdd,a.srno from pomas a,famsr b,item c where trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and a." + branch_Cd + " and A.TYPE like '5%' AND A.orddt " + xprdrange + " order by vdd ,a.ordno ,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Purchase Orders for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F20121":
                    SQuery = "select a.vchnum as GE_no,to_char(a.vchdate,'dd/mm/yyyy') as GE_Dt,b.Aname as Supplier,a.Invno,to_chaR(a.Invdate,'dd/mm/yyyy') as Inv_Dt,c.Iname as Item_Name,C.Cpartno,a.iqty_chl as GE_Qty,c.Unit,a.iqty_chlwt as GE_wt,a.iqty_wt as Pkgs,a.Desc_ as Remarks,a.Refnum,to_chaR(a.refdate,'dd/mm/yyyy') as Ref_Dt,a.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from ivoucherp a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and a." + branch_Cd + " and A.TYPE='00' AND A.vchdate " + xprdrange + " order by vdd ,a.vchnum ,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Gate Inward for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F20126":
                    SQuery = "select a.vchnum as GO_no,to_char(a.vchdate,'dd/mm/yyyy') as GO_Dt,b.Aname as Party_Name,a.Invno,to_chaR(a.Invdate,'dd/mm/yyyy') as Inv_Dt,c.Iname as Item_Name,C.Cpartno,a.iqty_chl as GO_Qty,c.Unit,a.iqty_chlwt as GO_wt,a.iqty_wt as Pkgs,a.Desc_ as Remarks,a.Refnum,to_chaR(a.refdate,'dd/mm/yyyy') as Ref_Dt,a.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from ivoucherp a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and a." + branch_Cd + " and A.TYPE='2G' AND A.vchdate " + xprdrange + " order by vdd ,a.vchnum ,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Gate Outward for the Period of " + value1 + " to " + value2, frm_qstr);
                    break;


                case "F60121":
                    if (CSR.Length > 1 || ulvl == "3") cond = " and trim(a.ccode)='" + uname + "'";
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR.Trim() + "'";
                    if (ulvl.toDouble() > 0) cond = " and trim(a.ent_by)='" + uname.Trim() + "'";
                    SQuery = "SELECT a.CSSNO as CSS_NO,to_char(A.CSsDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Eicon as Option_Name,a.Req_type,a.Iss_type as Issue_Type,a.Cont_name,a.Ent_by,a.Ent_Dt,last_Action,last_Actdt,a.app_by,a.app_dt,a.Cont_No,a.Cont_Email,to_chaR(a.CSSDT,'YYYYMMDD') as CSS_DTd  FROM WB_CSS_LOG a WHERE a." + branch_Cd + " and A.TYPE='CS' AND A.CSSDT " + xprdrange + "  " + cond + "  and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') ORDER BY CSS_DTd,A.CSSNO";
                    SQuery = "SELECT a.CSSNO as CSS_NO,to_char(A.CSsDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,b.ALLOWIGRP as client_type,a.dir_comp,a.Emodule as Module_Name,a.Eicon as Option_Name,a.Remarks,a.Req_type,a.Iss_type as Issue_Type,a.Cont_name,a.Cont_No,a.Cont_Email,a.ent_by,a.Ent_Dt,last_Action,last_Actdt,a.wrkrmk,a.app_by,a.app_dt,a.root as root_cause,a.corrective as corrective_action,a.preventive as preventive_action_suggestion,a.solvedby,a.start_time,a.end_time,a.time_Taken,to_chaR(a.CSSDT,'YYYYMMDD') as CSS_DTd FROM WB_CSS_LOG a left outer join evas b on trim(a.ccode)=trim(B.USERNAME) where a." + branch_Cd + " and a.type='CS' and a.cssdt " + xprdrange + " " + cond + " order by a.cssno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Customer Support Request During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F60126":
                    SQuery = "SELECT a.DSRNO as Asg_NO,to_char(A.DSRDT,'dd/mm/yyyy') as ASg_Dt,a.CSSNO as CSS_NO,to_char(A.CSSDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.Emodule as Module_Name,A.Asg_agt,a.priority as Dlv_Hrs,a.Asg_Dpt,a.Asg_asys as Our_Analysis,a.Remarks,a.CSS_Status ,(Case when a.TASK_COMPL='Y' then 'Task Done' else 'Task Pending' end) as Task_Status,a.last_Action,a.last_Actdt,a.Ent_Dt,a.Ent_by,to_chaR(a.DSRDT,'YYYYMMDD') as DSR_DTd  FROM WB_CSs_ASG a WHERE a." + branch_Cd + " and A.TYPE='DF' AND A.DSRDT " + xprdrange + "  ORDER BY DSR_DTd,A.DSRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of CSS Work Assignment During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F60131":
                    cond = "";
                    if (ulvl != "0") cond = " and trim(a.ent_by)='" + uname + "'";
                    SQuery = "SELECT a.ActNO as Action_NO,to_char(A.ActDT,'dd/mm/yyyy') as Action_Dt,a.CSSNO as CSS_NO,to_char(A.CSSDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Asg_Dpt,A.Asg_agt,a.Act_Status ,a.Remarks,a.Task_compl,a.Next_tgt_date,a.Ent_Dt,a.Ent_by,to_chaR(a.ACTDT,'YYYYMMDD') as ACT_DT  FROM WB_CSs_Act a WHERE a." + branch_Cd + " and A.TYPE='AC' AND A.actDT " + xprdrange + "  " + cond + " ORDER BY ACT_DT,A.ActNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of CSS Work Done During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F60132":
                    cond = "";
                    if (ulvl != "0") cond = " and trim(a.ent_by)='" + uname + "'";
                    SQuery = "Select '1.Query Log' as Q_Status,Ccode,(Case when nvl(app_by,'-')!='-' then 'Accepted' else (Case when nvl(DIR_COMP,'-')='Y' then 'Dir-Close' else '-' end) end) as Task_Compl,trim(cssno) as CSSno,to_Char(cssdt,'dd/mm/yyyy') as CSSdt,Emodule,Req_type,Cont_name,Remarks,ent_dt,app_by,work_action,to_Char(cssdt,'yyyymmdd') as cdd from WB_CSS_LOG where " + branch_Cd + " and TYPE='CS' AND cssdt " + xprdrange + " union all Select '2.Query Assigned' as Q_Status,Ccode,Task_Compl,trim(cssno) as CSSno,to_Char(cssdt,'dd/mm/yyyy') as CSSdt,Emodule,'-' as Req_type,asg_agt as Cont_name,Remarks,ent_dt,app_by,'-' as work_action,to_Char(cssdt,'yyyymmdd') as cdd from WB_CSS_Asg where " + branch_Cd + " and TYPE='DF' AND cssdt " + xprdrange + " union all Select '3.Query Action' as Q_Status,Ccode,Task_Compl,trim(cssno) as CSSno,to_Char(cssdt,'dd/mm/yyyy') as CSSdt,Emodule,'-' as Req_type,ent_by as Cont_name,Remarks,ent_dt,app_by,'-' as work_action,to_Char(cssdt,'yyyymmdd') as cdd from WB_CSS_Act where " + branch_Cd + " and TYPE='AC' AND cssdt " + xprdrange + " ";
                    SQuery = "Select CSSno,CSSdt,Ccode,ent_dt,Q_Status,Cont_name as Persons,Task_Compl,Emodule,Req_type,Remarks,app_by,work_action,cdd from (" + SQuery + ") order by cdd desc,CSSno desc,ent_dt desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of CSS Resolution Status,Date Wise During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F60133":
                    SQuery = "select CSSno as CSS_No,CSSdt as CSS_DT,ccode as Client_Code,max(Emodule) as Module_name,(Case when sum(dasg)>0 then 'Yes' else 'NO' end ) as Pend_Asg,(Case when sum(conf)>0 then 'Yes' else 'NO' end ) as Pend_Conf,max(ent_By) as Ent_by,max(Req_type) as Req_type,max(Iss_Type) as Iss_Type,Max(Eicon)As Eicon,max(Remarks) as Remarks,max(Cont_name) as Cont_name,max(Cont_No) as Cont_No,max(Cont_email) as Cont_email from (Select ent_by,ccode,trim(cssno) as CSSno,to_Char(cssdt,'dd/mm/yyyy') as CSSdt,Emodule,Req_type,Iss_Type,1 as dasg,1 as conf,Eicon,Cont_name,Cont_No,Cont_email,Remarks from WB_CSS_LOG where " + branch_Cd + " and TYPE='CS' AND CSSDT " + xprdrange + " and trim(nvl(Fapp_by,'-'))='-' union all Select null as ent_by,ccode,trim(cssno) as cssno,to_Char(cssdt,'dd/mm/yyyy') as cssdt,null as Emod,null as Req_type,null as Iss_Type, -1 as dasg,0 as impl,null as Eicon,null as Cont_name,null as Cont_No,null as Cont_email,null as Remarks from wb_Css_asg where " + branch_Cd + " and TYPE='DF' AND CSSDT " + xprdrange + " AND UPPER(TRIM(CSS_STATUS))='ASSIGNED/ UNDER PROCESSING' union all Select null as ent_by,ccode,trim(cssno) as cssno,to_Char(cssdt,'dd/mm/yyyy') as cssdt,null as Emod,null as Req_type,null as Iss_Type, 0 as dasg,-1 as impl,null as Eicon,null as Cont_name,null as Cont_No,null as Cont_email,null as Remarks  from wb_Css_asg where " + branch_Cd + " and TYPE='DF' AND CSSDT " + xprdrange + " AND UPPER(TRIM(CSS_STATUS))='CONFIRMATION REQUIRED'  ) group by CSSno||CSSdt||ccode,CSSno,CSSdt,ccode having (sum(dasg)>0 or sum(conf)>0)  order by CSSdt ,CSSno ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of CSS Pending Assignment During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F60134":
                    SQuery = "SELECT a.DSRNO as Asg_NO,to_char(A.DSRDT,'dd/mm/yyyy') as ASg_Dt,a.CSSNO as CSS_NO,to_char(A.CSSDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.Emodule as Module_Name,A.Asg_agt as Assign_to,a.priority as Dlv_Hrs,round(sysdate-a.ent_Dt,2) as Pend_Days,a.Asg_asys Asg_Dpt,a.Remarks,a.last_Action,a.last_Actdt,a.Ent_Dt,a.Ent_by,to_chaR(a.DSRDT,'YYYYMMDD') as DSR_DTd  FROM WB_CSs_ASG a WHERE a." + branch_Cd + " and A.TYPE='DF' AND A.DSRDT " + xprdrange + " and nvl(a.TASK_COMPL,'-')!='Y' ORDER BY DSR_DTd,A.DSRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Work Assignment Pending Action During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F60135":
                    //SQuery = "select distinct a.branchcd||a.type||trim(a.CSSNO)||to_char(a.CSSdt,'dd/mm/yyyy') as fstr,to_Char(a.CSSdt,'yyyymmdd') as vdd,a.CCode,a.CSSNO as Css_No,to_Char(A.CSSdt,'dd/mm/yyyy') as css_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.dir_comp,a.Work_Action,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_log a where a.branchcd='" + mbr + "' and a.type='CS' and a.CSSdt " + mprdrange + " and trim(nvl(a.Fapp_by,'-'))='-'  order by vdd,a.CSSNO";
                    SQuery = "SELECT a.CSSNO as CSS_NO,to_char(A.CSsDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Eicon as Option_Name,a.Ent_by,a.dir_Comp,a.Iss_type as Issue_Type,a.Cont_name,a.Ent_Dt,a.Remarks,a.wrkRmk,a.time_taken,a.last_Action,a.last_Actdt,a.app_by,a.app_dt,a.Cont_No,a.Cont_Email,to_chaR(a.CSSDT,'YYYYMMDD') as CSS_DTd  FROM WB_CSs_LOG a ,evas b WHERE trim(A.ent_by)=trim(b.username) and b.mdeptt like 'A%' and a." + branch_Cd + " and A.TYPE='CS' AND A.CSSDT " + xprdrange + " and trim(nvl(a.Fapp_by,'-'))='-' and trim(nvl(a.DIR_COMP,'-'))!='Y' ORDER BY CSS_DTd,A.CSSNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of CSS Pending Closure by Support Lead During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F60136":
                    cond = "";
                    if (ulvl != "0") cond = " and trim(a.ent_by)='" + uname + "'";
                    SQuery = "select trim(CCODE) as Client,trim(ASG_AGT) as Asg_TO,CSSNO,to_char(CSSDT,'dd/mm/yyyy') as CSSDTD,DSRNO,to_ChaR(DSRDT,'dd/mm/yyyy') as DSRDT,trim(EMODULE) as CModule,max(ASG_ASYS) as CAsys,max(actdt) as actdt,max(PRIORITY) as ASG_HR,sum(Taskg) as Task_Given,sum(Taskd) as Task_Done,sum(taskc) as task_Compl,sum(taska) as task_Approved,to_char(CSSDT,'YYYYMMDD') as csdd  from (select CCODE,ASG_AGT,CSSNO,CSSDT,DSRNO,DSRDT,null as actdt,EMODULE,ASG_ASYS,PRIORITY,1 as Taskg,0 as Taskd,0 as taskc,0 as taska from wb_Css_asg WHERE " + branch_Cd + " and TYPE='DF' AND DSRDT " + xprdrange + " union all select CCODE,ASG_AGT,CSSNO,CSSDT,DSRNO,DSRDT,to_ChaR(actdt,'dd/mm/yyyy') as actdy,EMODULE,null as ASG_ASYS,0 as PRIORITY,0 as Taskg,1 as Taskd,0 as taskc,0 as taska from wb_Css_act WHERE " + branch_Cd + " and TYPE='AC' AND DSRDT " + xprdrange + " union all select CCODE,ASG_AGT,CSSNO,CSSDT,DSRNO,DSRDT,to_ChaR(actdt,'dd/mm/yyyy'),EMODULE,null as ASG_ASYS,0 as PRIORITY,0 as Taskg,0 as Taskd,1 as taskc,0 as taska from wb_Css_act WHERE " + branch_Cd + " and TYPE='AC' AND DSRDT " + xprdrange + " and task_compl='Y' union all select CCODE,ASG_AGT,CSSNO,CSSDT,DSRNO,DSRDT,to_ChaR(actdt,'dd/mm/yyyy'),EMODULE,null as ASG_ASYS,0 as PRIORITY,0 as Taskg,0 as Taskd,0 as taskc,1 as taska from wb_Css_act WHERE " + branch_Cd + " and TYPE='AC' AND DSRDT " + xprdrange + " and  trim(nvl(app_by,'-'))!='-') group by trim(CCODE),trim(ASG_AGT),CSSNO,to_char(CSSDT,'dd/mm/yyyy'),DSRNO,to_ChaR(DSRDT,'dd/mm/yyyy'),trim(EMODULE),to_char(CSSDT,'YYYYMMDD') order by to_char(CSSDT,'YYYYMMDD'),CSSNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Assigned Work Status During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F60137":

                    value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    fromdt = value1;

                    cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('01/10/2017','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')";
                    xprd3 = "between to_date('01/10/2017','dd/mm/yyyy') and to_date('31/03/2050','dd/mm/yyyy')";

                    //SQuery = "SELECT a.CSSNO as CSS_NO,to_char(A.CSsDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Eicon as Option_Name,a.Req_type,a.Iss_type as Issue_Type,a.Cont_name,a.Ent_Dt,last_Action,last_Actdt,a.app_by,a.app_dt,a.Cont_No,a.Cont_Email,to_chaR(a.CSSDT,'YYYYMMDD') as CSS_DTd  FROM WB_CSs_LOG a WHERE a." + branch_Cd + " and A.TYPE='CS' AND A.CSSDT " + xprdrange + " and trim(upper(last_action))='MARKED COMPLETE' and trim(nvl(app_by,'-'))='-'   ORDER BY CSS_DTd,A.CSSNO";

                    mq0 = "select asg_agt as Team_Member,sum(opening) as Opening_Tasks,sum(cdr) as Assigned_Tasks,sum(ccr) As Cleared_Tasks,sum(opening)+sum(cdr)-sum(ccr) as Closing_Tasks,sum(pendcompl) as Under_Progess,sum(pendadpp) as Pend_Approval from (Select distinct upper(Trim(asg_Agt)) as asg_Agt,0 as opening,0 as cdr,0 as ccr,0 as clos,0 as pendadpp,0 as pendcompl from wb_Css_Asg where branchcd!='DD' union all ";
                    mq1 = "select upper(Trim(asg_Agt)) as asg_Agt,count(dsrno) as op,0 as cdr,0 as ccr,0 as clos,0 as pendadpp,0 as pendcompl from wb_css_asg where branchcd!='DD' and dsrdt " + xprd1 + "  GROUP BY upper(Trim(asg_Agt)) union all ";
                    mq2 = "select upper(Trim(asg_Agt)) as asg_Agt,count(actno)*-1 as op,0 as cdr,0 as ccr,0 as clos,0 as pendadpp,0 as pendcompl from wb_css_act where branchcd!='DD' and actdt " + xprd1 + "  and trim(nvl(app_by,'-'))!='-' GROUP BY upper(Trim(asg_Agt)) union all ";
                    mq3 = "select upper(Trim(asg_Agt)) as asg_Agt,0 as op,count(dsrno) as cdr,0 as ccr,0 as clos,0 as pendadpp,0 as pendcompl from wb_css_asg where branchcd!='DD' and dsrdt " + xprd2 + "  GROUP BY upper(Trim(asg_Agt)) union all ";
                    mq4 = "select upper(Trim(asg_Agt)) as asg_Agt,0 as op,0 as cdr,0 as ccr,0 as clos,count(actno) as pendadpp,0 as pendcompl from wb_css_act where branchcd!='DD' and actdt " + xprd3 + "  and trim(nvl(app_by,'-'))='-' and trim(nvl(task_Compl,'-'))='Y' GROUP BY upper(Trim(asg_Agt))  union all  ";
                    mq5 = "select upper(Trim(asg_Agt)) as asg_Agt,0 as op,0 as cdr,0 as ccr,0 as clos,0 as pendadpp,count(actno) as pendcompl from wb_css_act where branchcd!='DD' and actdt " + xprd3 + "  and trim(nvl(app_by,'-'))='-' and trim(nvl(task_Compl,'-'))='N' GROUP BY upper(Trim(asg_Agt))  union all  ";
                    mq6 = "select upper(Trim(asg_Agt)) as asg_Agt,0 as op,0 as cdr,count(actno) as ccr,0 as clos,0 as pendadpp,0 as pendcompl from wb_css_act where branchcd!='DD' and actdt " + xprd2 + "  and trim(nvl(app_by,'-'))!='-' GROUP BY upper(Trim(asg_Agt))) group by asg_agt Order by asg_agt ";

                    SQuery = mq0 + mq1 + mq2 + mq3 + mq4 + mq5 + mq6;

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Assignment Status Team Member Wise During " + value1 + " to " + value2, frm_qstr);
                    break;


                // open drill down form                    
                //fgen.drillQuery(0, "select ccode as fstr,'-' as gstr,count(CSSNO) as tot_Query,ccode as company from WB_CSS_LOG where type='CS' group by ccode order by ccode", frm_qstr);
                //fgen.drillQuery(1, "select trim(a.ccode) as fstr,to_char(a.ccode) as gstr,a.ccode,a.cssno,a.cssdt from WB_CSS_LOG a", frm_qstr);


                //fgen.drillQuery(0, "select Upper(Trim(asg_agt)) as fstr,'-' as gstr,Upper(Trim(asg_agt)) as Assignee,count(Dsrno) as Task_Assigned,trim(Ccode) as Client from WB_CSS_Asg where branchcd!='DD' and type='DF' group by Upper(Trim(asg_agt)),trim(Ccode) order by Upper(Trim(asg_agt))", frm_qstr);
                //fgen.drillQuery(1, "select Upper(Trim(a.asg_agt)) as fstr,Upper(Trim(a.asg_agt)) as gstr,a.Ccode,a.Emodule,a.Eicon,a.Cssno,a.Cssdt,a.act_Status,a.task_Compl,a.Next_tgt_Date,a.app_by from WB_CSS_Act a where a.branchcd!='DD' and a.type='AC' order by a.Actdt", frm_qstr);

                //fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                //fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                //fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                //fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                //fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                //fgen.Fn_DrillReport("", frm_qstr);

                //fgen.Fn_DrillReport("", frm_qstr);
                case "F60138":
                    SQuery = "select Client_code,nvl(day1,0)+nvl(day2,0)+nvl(day3,0)+nvl(day4,0)+nvl(day5,0)+nvl(day6,0)+nvl(day7,0)+nvl(day8,0)+nvl(day9,0)+nvl(day10,0)+nvl(day11,0)+nvl(day12,0)+nvl(day13,0)+nvl(day14,0)+nvl(day15,0)+nvl(day16,0)+nvl(day17,0)+nvl(day18,0)+nvl(day19,0)+nvl(day20,0)+nvl(day21,0)+nvl(day22,0)+nvl(day23,0)+nvl(day24,0)+nvl(day25,0)+nvl(day26,0)+nvl(day27,0)+nvl(day28,0)+nvl(day29,0)+nvl(day30,0)+nvl(day31,0) Total_CSS,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 from (WITH pivot_data AS (SELECT to_Char(a.cssdt,'dd') as  Mth_no, trim(a.ccode) as Client_Code, count(a.cssno)  as sal FROM wb_Css_log a where a.branchcd!='88' and a.cssdt " + xprdrange + " group by to_Char(a.cssdt,'dd'),trim(a.ccode) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 ))) order by Client_Code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Day Wise Client wise CSS Recorded during " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F60139":
                    SQuery = "select Client_code,nvl(Apr,0)+nvl(May,0)+nvl(Jun,0)+nvl(Jul,0)+nvl(Aug,0)+nvl(Sep,0)+nvl(Oct,0)+nvl(Nov,0)+nvl(Dec,0)+nvl(Jan,0)+nvl(Feb,0)+nvl(Mar,0) as Total_CSS,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec,Jan,Feb,Mar from (WITH pivot_data AS (SELECT to_Char(a.cssdt,'mm') as  Mth_no, trim(a.ccode) as Client_Code, count(a.cssno)  as sal FROM wb_Css_log a where a.branchcd!='88' and a.cssdt " + xprdrange + " group by to_Char(a.cssdt,'mm'),trim(a.ccode) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('04' as Apr,'05' as May,'06' as Jun,'07' as Jul,'08' as Aug,'09' as Sep,'10' as Oct,'11' as Nov,'12' as Dec,'01' as Jan,'02' as Feb,'03' as Mar))) order by Client_Code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Month Wise Client wise CSS Recorded during " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F60153":
                    SQuery = "select * from (select ent_by as Team_Member,sum(Support_Rec) as Support_Rec,sum(Visit_Rec) as Visit_Rec,sum(NEW_DEV_REC) as NEW_DEV_REC,sum(Support_Rec)+sum(Visit_Rec)+sum(NEW_DEV_REC) as Total_Rec,sum(Support_time) as Support_time,sum(Visit_Time) as Visit_Time,sum(NEW_DEV_time) as NEW_DEV_time,sum(Support_time)+sum(Visit_Time)+sum(NEW_DEV_time) as Total_time from (select ent_by,dir_Comp,(case when upper(req_type)='SUPPORT' then 1 else 0 end ) as Support_Rec,(case when upper(req_type)='VISIT' then 1 else 0 end ) as Visit_Rec,(case when upper(req_type)='NEW DEV.' then 1 else 0 end ) as NEW_DEV_REC,(case when upper(req_type)='SUPPORT' then time_Taken else 0 end ) as Support_time,(case when upper(req_type)='VISIT' then time_Taken else 0 end ) as Visit_Time,(case when upper(req_type)='NEW DEV.' then time_Taken else 0 end ) as NEW_DEV_time from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ") group by ent_by union all select 'ZZ-Total-ZZ' as Team_Member,sum(Support_Rec) as Support_Rec,sum(Visit_Rec) as Visit_Rec,sum(NEW_DEV_REC) as NEW_DEV_REC,sum(Support_Rec)+sum(Visit_Rec)+sum(NEW_DEV_REC) as Total_Rec,sum(Support_time) as Support_time,sum(Visit_Time) as Visit_Time,sum(NEW_DEV_time) as NEW_DEV_time,sum(Support_time)+sum(Visit_Time)+sum(NEW_DEV_time) as Total_time from (select ent_by,dir_Comp,(case when upper(req_type)='SUPPORT' then 1 else 0 end ) as Support_Rec,(case when upper(req_type)='VISIT' then 1 else 0 end ) as Visit_Rec,(case when upper(req_type)='NEW DEV.' then 1 else 0 end ) as NEW_DEV_REC,(case when upper(req_type)='SUPPORT' then time_Taken else 0 end ) as Support_time,(case when upper(req_type)='VISIT' then time_Taken else 0 end ) as Visit_Time,(case when upper(req_type)='NEW DEV.' then time_Taken else 0 end ) as NEW_DEV_time from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ")) order by  Team_Member ";
                    SQuery = "select * from (select ent_by as Team_Member,sum(Support_Rec) as Support_Rec,sum(Visit_Rec) as Visit_Rec,sum(NEW_DEV_REC) as NEW_DEV_REC,sum(Support_Rec)+sum(Visit_Rec)+sum(NEW_DEV_REC) as Total_Rec,sum(Support_time) as Support_time,sum(Visit_Time) as Visit_Time,sum(NEW_DEV_time) as NEW_DEV_time,sum(Support_time)+sum(Visit_Time)+sum(NEW_DEV_time) as Total_time from (select ent_by,dir_Comp,(case when upper(req_type)='SUPPORT' then 1 else 0 end ) as Support_Rec,(case when upper(req_type)='VISIT' then 1 else 0 end ) as Visit_Rec,(case when upper(req_type)='NEW DEV.' then 1 else 0 end ) as NEW_DEV_REC,(case when upper(req_type)='SUPPORT' then time_Taken else 0 end ) as Support_time,(case when upper(req_type)='VISIT' then time_Taken else 0 end ) as Visit_Time,(case when upper(req_type)='NEW DEV.' then time_Taken else 0 end ) as NEW_DEV_time from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ") group by ent_by UNION ALL SELECT DISTINCT TRIM(ENT_BY) AS ENT_BY,0 AS C1,0 AS C2,0 AS C3,0 AS C4,0 AS C5,0 AS C6,0 AS C7,0 AS C8  from wb_Css_log where branchcd!='88' and cssdt between to_date('" + fromdt + "','dd/mm/yyyy')-60 AND to_date('" + fromdt + "','dd/mm/yyyy') AND TRIM(ENT_BY) NOT IN (SELECT DISTINCT TRIM(ENT_BY) AS ENT_BY from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ") AND LENGTH(ENT_BY)>2 union all select 'ZZ-Total-ZZ' as Team_Member,sum(Support_Rec) as Support_Rec,sum(Visit_Rec) as Visit_Rec,sum(NEW_DEV_REC) as NEW_DEV_REC,sum(Support_Rec)+sum(Visit_Rec)+sum(NEW_DEV_REC) as Total_Rec,sum(Support_time) as Support_time,sum(Visit_Time) as Visit_Time,sum(NEW_DEV_time) as NEW_DEV_time,sum(Support_time)+sum(Visit_Time)+sum(NEW_DEV_time) as Total_time from (select ent_by,dir_Comp,(case when upper(req_type)='SUPPORT' then 1 else 0 end ) as Support_Rec,(case when upper(req_type)='VISIT' then 1 else 0 end ) as Visit_Rec,(case when upper(req_type)='NEW DEV.' then 1 else 0 end ) as NEW_DEV_REC,(case when upper(req_type)='SUPPORT' then time_Taken else 0 end ) as Support_time,(case when upper(req_type)='VISIT' then time_Taken else 0 end ) as Visit_Time,(case when upper(req_type)='NEW DEV.' then time_Taken else 0 end ) as NEW_DEV_time from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ")) order by  TOTAL_TIME DESC,Team_Member";
                    //SQuery = "select * from (select * from (select ent_by as Team_Member,sum(Support_Rec) as Support_Rec,sum(Visit_Rec) as Visit_Rec,sum(NEW_DEV_REC) as NEW_DEV_REC,sum(Support_Rec)+sum(Visit_Rec)+sum(NEW_DEV_REC) as Total_Rec,sum(Support_time) as Support_time,sum(Visit_Time) as Visit_Time,sum(NEW_DEV_time) as NEW_DEV_time,sum(Support_time)+sum(Visit_Time)+sum(NEW_DEV_time) as Total_time from (select ent_by,dir_Comp,(case when upper(req_type)='SUPPORT' then 1 else 0 end ) as Support_Rec,(case when upper(req_type)='VISIT' then 1 else 0 end ) as Visit_Rec,(case when upper(req_type)='NEW DEV.' then 1 else 0 end ) as NEW_DEV_REC,(case when upper(req_type)='SUPPORT' then time_Taken else 0 end ) as Support_time,(case when upper(req_type)='VISIT' then time_Taken else 0 end ) as Visit_Time,(case when upper(req_type)='NEW DEV.' then time_Taken else 0 end ) as NEW_DEV_time from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ") group by ent_by UNION ALL SELECT DISTINCT TRIM(ENT_BY) AS ENT_BY,0 AS C1,0 AS C2,0 AS C3,0 AS C4,0 AS C5,0 AS C6,0 AS C7,0 AS C8  from wb_Css_log where branchcd!='88' and cssdt between to_date('" + fromdt + "','dd/mm/yyyy')-60 AND to_date('" + fromdt + "','dd/mm/yyyy') AND TRIM(ENT_BY) NOT IN (SELECT DISTINCT TRIM(ENT_BY) AS ENT_BY from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ") AND LENGTH(ENT_BY)>2 )) union all select 'ZZ-Total-ZZ' as Team_Member,sum(Support_Rec) as Support_Rec,sum(Visit_Rec) as Visit_Rec,sum(NEW_DEV_REC) as NEW_DEV_REC,sum(Support_Rec)+sum(Visit_Rec)+sum(NEW_DEV_REC) as Total_Rec,sum(Support_time) as Support_time,sum(Visit_Time) as Visit_Time,sum(NEW_DEV_time) as NEW_DEV_time,sum(Support_time)+sum(Visit_Time)+sum(NEW_DEV_time) as Total_time from (select ent_by,dir_Comp,(case when upper(req_type)='SUPPORT' then 1 else 0 end ) as Support_Rec,(case when upper(req_type)='VISIT' then 1 else 0 end ) as Visit_Rec,(case when upper(req_type)='NEW DEV.' then 1 else 0 end ) as NEW_DEV_REC,(case when upper(req_type)='SUPPORT' then time_Taken else 0 end ) as Support_time,(case when upper(req_type)='VISIT' then time_Taken else 0 end ) as Visit_Time,(case when upper(req_type)='NEW DEV.' then time_Taken else 0 end ) as NEW_DEV_time from wb_Css_log where branchcd!='88' and cssdt " + xprdrange + ") order by  TOTAL_TIME DESC,Team_Member";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Team Support Logging during " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F60154":

                    SQuery = "select upper(ent_by) as Ent_by,sum(regd) As CSS_logged,sum(dir_close) as dir_close,sum(Final_close) as Final_close  from (select ent_by,1 as regd,0 as dir_close,0 as Final_close,time_taken from wb_css_log where cssdt " + xprdrange + " union all select ent_by,0 as regd,1 as closed,0 as final_close,0 as timetk from wb_css_log where cssdt " + xprdrange + "  and dir_comp='Y'  union all select ent_by,0 as regd,0 as closed,1 as final_close,0 as timetk from wb_css_log where cssdt " + xprdrange + " and last_action='Marked Complete'  ) group by upper(ent_by) order by upper(ent_by) ";


                    //SQuery = "select Client_code,nvl(Apr,0)+nvl(May,0)+nvl(Jun,0)+nvl(Jul,0)+nvl(Aug,0)+nvl(Sep,0)+nvl(Oct,0)+nvl(Nov,0)+nvl(Dec,0)+nvl(Jan,0)+nvl(Feb,0)+nvl(Mar,0) as Total_CSS,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec,Jan,Feb,Mar from (WITH pivot_data AS (SELECT to_Char(a.cssdt,'mm') as  Mth_no, trim(a.ccode) as Client_Code, count(a.cssno)  as sal FROM wb_Css_log a where a.branchcd!='88' and a.cssdt " + xprdrange + " group by to_Char(a.cssdt,'mm'),trim(a.ccode) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('04' as Apr,'05' as May,'06' as Jun,'07' as Jul,'08' as Aug,'09' as Sep,'10' as Oct,'11' as Nov,'12' as Dec,'01' as Jan,'02' as Feb,'03' as Mar))) order by Client_Code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Team Support Logging during " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F60142":
                    SQuery = "select Team_code,nvl(day1,0)+nvl(day2,0)+nvl(day3,0)+nvl(day4,0)+nvl(day5,0)+nvl(day6,0)+nvl(day7,0)+nvl(day8,0)+nvl(day9,0)+nvl(day10,0)+nvl(day11,0)+nvl(day12,0)+nvl(day13,0)+nvl(day14,0)+nvl(day15,0)+nvl(day16,0)+nvl(day17,0)+nvl(day18,0)+nvl(day19,0)+nvl(day20,0)+nvl(day21,0)+nvl(day22,0)+nvl(day23,0)+nvl(day24,0)+nvl(day25,0)+nvl(day26,0)+nvl(day27,0)+nvl(day28,0)+nvl(day29,0)+nvl(day30,0)+nvl(day31,0) Total_CSS,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 from (WITH pivot_data AS (SELECT to_Char(a.cssdt,'dd') as  Mth_no, trim(a.ent_by) as Team_Code, count(a.cssno)  as sal FROM wb_Css_log a where a.branchcd!='88' and a.cssdt " + xprdrange + " group by to_Char(a.cssdt,'dd'),trim(a.ent_by) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 ))) order by Team_Code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Day Wise Client wise CSS Recorded during " + value1 + " to " + value2, frm_qstr);
                    break;


                case "F61121":
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
                    SQuery = "SELECT a.CCMNO as CCM_NO,to_char(A.CCMDT,'dd/mm/yyyy') as CCM_Dt,a.Cust_name as Co_Name,a.Comp_type,a.Cdescr as Product,a.Compcatg as Grading,a.Lremarks as CCM_Remark,a.Oremarks as Our_Remark,a.Cont_name,a.Cont_No,a.Cont_Email,a.Ent_Dt,last_Action,last_Actdt,a.app_by,a.app_dt,to_chaR(a.CCMDT,'YYYYMMDD') as CCM_DTd  FROM WB_CCM_LOG a WHERE a." + branch_Cd + " and A.TYPE='CC' AND A.CCMDT " + xprdrange + "  " + cond + " ORDER BY CCM_DTd,A.CCMNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Customer Complain Logging During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F61126":
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
                    SQuery = "SELECT a.CACNO as Action_NO,to_char(A.CACDT,'dd/mm/yyyy') as Action_Dt,a.CCMNO as CCM_NO,to_char(A.CCMDT,'dd/mm/yyyy') as CCM_Dt,a.Cust_name as Co_Name,a.Comp_type,a.Cdescr as Product,a.Compcatg as Grading,a.Oremarks as Our_Remark,a.Cont_name,a.Curr_stat,a.Act_mode,a.Next_Folo as Close_by,a.Ent_Dt,a.app_by,a.app_dt,to_chaR(a.CACDT,'YYYYMMDD') as CAC_DTd  FROM WB_CCM_Act a WHERE a." + branch_Cd + " and A.TYPE='CA' AND A.CACDT " + xprdrange + "  " + cond + " ORDER BY CAC_DTd ,A.CCMNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Customer Complain Action During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F61131":

                    cond = "";
                    if (ulvl != "0") cond = " and trim(a.ent_by)='" + uname + "'";

                    SQuery = "Select '1.Complain Logged' as L_Status,Cust_Name,CDESCR as Product,trim(CCMno) as CCMno,to_Char(CCMdt,'dd/mm/yyyy') as CCMdt,Cont_name,Cont_no,LRemarks as Remarks,ent_by,ent_dt,'-' as curr_Stat,to_Char(CCMdt,'yyyymmdd') as CCMdd,null as act_mode,null as Next_Folo from WB_CCM_LOG where " + branch_Cd + " and TYPE='CC' AND CCMdt " + xprdrange + " union all  Select '2.Lead Action    ' as L_Status,Cust_Name,CDESCR as Product,trim(CCMno) as CCMno,to_Char(CCMdt,'dd/mm/yyyy') as CCMdt,Cont_name,Cont_no,ORemarks as remarks,ent_by,ent_dt,Curr_Stat,to_Char(CCMdt,'yyyymmdd') as ldd,act_mode,Next_Folo from WB_CCM_ACT where " + branch_Cd + " and TYPE='CA' AND CACdt " + xprdrange + " ";

                    SQuery = "Select CCMno,CCMdt,Cust_Name,ent_dt,Product,Curr_Stat,Cont_name as Contact_person,Cont_No,Remarks,ent_by,Act_Mode as Last_Action,Next_Folo as Resolve_in_Days from (" + SQuery + ") order by CCMdd,CCMno,ent_dt";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Customer Complain Status Report,Date Wise During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F93121":
                    cond = "";
                    SQuery = "select a.OACNO as ACtion_no,to_char(a.OACdt,'dd/mm/yyyy') as Action_Dt,c.Username as Action_by,b.aname,b.acode as Client_Code,a.Agree_Amt as Agree_Tgt,to_Char(a.Agree_DT,'dd/mm/yyyy') as Agree_DT,a.Remarks,a.Naration,a.CCode,a.Ent_by,a.ent_Dt ,to_Char(a.oacdt,'yyyymmdd') as vdd,a.srno from wb_oms_act a,famst b,evas c where trim(A.ccode)=trim(B.acode) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OF' and oacdt " + xprdrange + " order by c.Username,vdd ,a.oacno ,a.srno";
                    if (co_cd == "TEST")
                        SQuery = "select a.OACNO as ACtion_no,to_char(a.OACdt,'dd/mm/yyyy') as Action_Dt,c.Username as Action_by,b.Full_name,b.Username as Client_Code,a.Agree_Amt as Agree_Tgt,to_Char(a.Agree_DT,'dd/mm/yyyy') as Agree_DT,a.Remarks,a.Naration,a.CCode,a.Ent_by,a.ent_Dt ,to_Char(a.oacdt,'yyyymmdd') as vdd,a.srno from wb_oms_act a,evas b,evas c where trim(A.ccode)=trim(B.userid) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OF' and oacdt " + xprdrange + " order by c.Username,vdd ,a.oacno ,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of OMS Followup Team Wise During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F93126":
                    cond = "";
                    SQuery = "select a.OACNO as ACtion_no,to_char(a.OACdt,'dd/mm/yyyy') as Action_Dt,c.Username as Action_by,b.aname,b.acode as Client_Code,a.Agree_Amt as Agree_Tgt,to_Char(a.Agree_DT,'dd/mm/yyyy') as Agree_DT,a.Remarks,a.Naration,a.CCode,a.Ent_by,a.ent_Dt ,to_Char(a.oacdt,'yyyymmdd') as vdd,a.srno from wb_oms_act a,famst b,evas c where trim(A.ccode)=trim(B.acode) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OF' and oacdt " + xprdrange + " order by b.aname,vdd ,a.oacno ,a.srno";
                    if (co_cd == "TEST")
                        SQuery = "select a.OACNO as ACtion_no,to_char(a.OACdt,'dd/mm/yyyy') as Action_Dt,c.Username as Action_by,b.Full_name,b.Username as Client_Code,a.Agree_Amt as Agree_Tgt,to_Char(a.Agree_DT,'dd/mm/yyyy') as Agree_DT,a.Remarks,a.Naration,a.CCode,a.Ent_by,a.ent_Dt ,to_Char(a.oacdt,'yyyymmdd') as vdd,a.srno from wb_oms_act a,evas b,evas c where trim(A.ccode)=trim(B.userid) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OF' and oacdt " + xprdrange + " order by b.Username,vdd ,a.oacno ,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of OMS Followup Client Wise During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F93131":
                    cond = "";
                    SQuery = "select a.Action_by,a.Client_Name,a.Month_Tgt,a.Agree_amt,a.Difference,a.No_of_Followup,a.Last_folo_upon,a.Last_Commit_date,substr(a.Last_remarks,12,150) as Last_remarks from (select Action_by,Client_Name,sum(Month_Tgt) as Month_Tgt,sum(Agree_amt) as Agree_amt,sum(Month_Tgt)-sum(Agree_amt)  as Difference,sum(Folo_cnt) as No_of_Followup,max(folo_Dt) as Last_folo_upon,max(Agree_Dt) as Last_Commit_date,max(folo_rmk) as Last_remarks from (select c.Username as Action_by,b.aname as Client_Name,a.Month_Amt as Month_Tgt,0 as Agree_amt,0 as Folo_cnt,null as folo_Dt,null as agree_dt,null as folo_rmk from wb_oms_log a,famst b,evas c where trim(A.ccode)=trim(B.acode) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OP' and opldt " + xprdrange + " union all select c.Username as Action_by,b.aname as Client_Name,0 as Month_Tgt,a.agree_amt as Agree_amt,1 as Folo_cnt,to_char(oacdt,'dd/mm/yyyy') as folo_Dt,to_char(agree_Dt,'yyyy-mm-dd') as Agree_Dt,to_char(oacdt,'yyyy-mm-dd')||'-'||remarks as remarks from wb_oms_act a,famst b,evas c where trim(A.ccode)=trim(B.acode) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OF' and oacdt " + xprdrange + ") group by Action_by,Client_Name) a order by a.Action_by,a.Client_Name";
                    if (co_cd == "TEST")
                        SQuery = "select a.Action_by,a.Client_Name as Ccode,b.Full_name as Client_Name,a.Month_Tgt,a.Agree_amt,a.Difference,a.No_of_Followup,a.Last_folo_upon,a.Last_Commit_date,substr(a.Last_remarks,12,150) as Last_remarks from (select Action_by,Client_Name,sum(Month_Tgt) as Month_Tgt,sum(Agree_amt) as Agree_amt,sum(Month_Tgt)-sum(Agree_amt)  as Difference,sum(Folo_cnt) as No_of_Followup,max(folo_Dt) as Last_folo_upon,max(Agree_Dt) as Last_Commit_date,max(folo_rmk) as Last_remarks from (select c.Username as Action_by,b.Username as Client_Name,a.Month_Amt as Month_Tgt,0 as Agree_amt,0 as Folo_cnt,null as folo_Dt,null as agree_dt,null as folo_rmk from wb_oms_log a,evas b,evas c where trim(A.ccode)=trim(B.userid) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OP' and opldt " + xprdrange + " union all select c.Username as Action_by,b.Username as Client_Name,0 as Month_Tgt,a.agree_amt as Agree_amt,1 as Folo_cnt,to_char(oacdt,'dd/mm/yyyy') as folo_Dt,to_char(agree_Dt,'yyyy-mm-dd') as Agree_Dt,to_char(oacdt,'yyyy-mm-dd')||'-'||remarks as remarks from wb_oms_act a,evas b,evas c where trim(A.ccode)=trim(B.userid) and trim(A.tcode)=trim(c.userid) and  a." + branch_Cd + " and a.type='OF' and oacdt " + xprdrange + ") group by Action_by,Client_Name)a,evas b where trim(a.Client_Name)=trim(b.username) order by  a.Action_by,a.Client_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of OMS Followup Target Vs Action Wise During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F94121":
                    cond = "";
                    SQuery = "SELECT a.filepath,a.STLNO as STL_NO,to_char(A.STLDT,'dd/mm/yyyy') as STL_Dtd,a.Evertical as Vertical_Name,a.Eicon,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Remarks,a.wrkRmk,a.Ent_Dt,a.Ent_by,to_chaR(a.STlDT,'YYYYMMDD') as STl_DT  FROM WB_STl_log a WHERE a." + branch_Cd + " and A.TYPE='TG' AND A.STlDT " + xprdrange + "  " + cond + " ORDER BY ent_by,STl_DT,A.STlno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Report of STl LOG Implementor Wise During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F94126":
                    cond = "";
                    SQuery = "SELECT a.STlNO as STl_NO,to_char(A.STlDT,'dd/mm/yyyy') as STl_Dtd,a.Evertical as Vertical_Name,a.Eicon,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Remarks,a.wrkRmk,a.Ent_Dt,a.Ent_by,to_chaR(a.STlDT,'YYYYMMDD') as STl_DT  FROM WB_STl_log a WHERE a." + branch_Cd + " and A.TYPE='TG' AND A.STlDT " + xprdrange + "  " + cond + " ORDER BY evertical,STl_DT,A.STlno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of STl LOG Vertical Wise During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F94131":
                    cond = "";
                    SQuery = "SELECT a.STlNO as STl_NO,to_char(A.STlDT,'dd/mm/yyyy') as STl_Dtd,a.Evertical as Vertical_Name,a.Eicon,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Remarks,a.wrkRmk,a.Ent_Dt,a.Ent_by,to_chaR(a.STlDT,'YYYYMMDD') as STl_DT  FROM WB_STl_log a WHERE a." + branch_Cd + " and A.TYPE='TG' AND A.STlDT " + xprdrange + "  " + cond + " ORDER BY ccode,STl_DT,A.STlno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of STl LOG Client Wise During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F96121":
                    cond = "";
                    SQuery = "SELECT a.DSLNO as DSL_NO,to_char(A.DSLDT,'dd/mm/yyyy') as DSL_Dt,a.Evertical as Vertical_Name,a.Eicon,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Remarks,a.wrkRmk,a.Ent_Dt,a.Ent_by,to_chaR(a.DSLDT,'YYYYMMDD') as ACT_DT  FROM WB_DSL_log a WHERE a." + branch_Cd + " and A.TYPE='SL' AND A.DSLDT " + xprdrange + "  " + cond + " ORDER BY ent_by,DSL_DT,A.dslno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of DSL LOG Dev Wise During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F96126":
                    cond = "";
                    SQuery = "SELECT a.DSLNO as DSL_NO,to_char(A.DSLDT,'dd/mm/yyyy') as DSL_Dt,a.Evertical as Vertical_Name,a.Eicon,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Remarks,a.wrkRmk,a.Ent_Dt,a.Ent_by,to_chaR(a.DSLDT,'YYYYMMDD') as ACT_DT  FROM WB_DSL_log a WHERE a." + branch_Cd + " and A.TYPE='SL' AND A.DSLDT " + xprdrange + "  " + cond + " ORDER BY evertical,DSL_DT,A.dslno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of DSL LOG Vertical Wise During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F96131":
                    cond = "";
                    SQuery = "SELECT a.DSLNO as DSL_NO,to_char(A.DSLDT,'dd/mm/yyyy') as DSL_Dt,a.Evertical as Vertical_Name,a.Eicon,a.CCODE as Client_Code,a.Emodule as Module_Name,a.Remarks,a.wrkRmk,a.Ent_Dt,a.Ent_by,to_chaR(a.DSLDT,'YYYYMMDD') as ACT_DT  FROM WB_DSL_log a WHERE a." + branch_Cd + " and A.TYPE='SL' AND A.DSLDT " + xprdrange + "  " + cond + " ORDER BY ccode,DSL_DT,A.dslno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of DSL LOG Client Wise During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F97121":
                    cond = "";
                    SQuery = "SELECT a.CAMNO as CAM_NO,to_char(A.CAMDT,'dd/mm/yyyy') as CAM_Dt,a.Cam_type,a.Cam_spec,a.TCODE as Team_member,a.Cam_Purpose,a.Cam_durn as Duration,a.Ent_by,a.Ent_Dt,a.app_by,a.app_Dt,to_chaR(a.CAMDT,'YYYYMMDD') as CAMDTD  FROM WB_CAM_log a WHERE a." + branch_Cd + " and A.TYPE='EQ' AND A.CAMDT " + xprdrange + "  " + cond + " ORDER BY ent_by,CAMDTD,A.CAMno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of CAM Log During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F92121":
                    cond = "";
                    SQuery = "Select a.Tcode as Team_Code,C.username as Team_Member,to_chaR(a.VISIT_DT,'dd/mm/yyyy') as VISIT_DT,b.username as client_cd,b.full_name as client_name,a.ALFNO as Plan_no,to_chaR(a.ALFDT,'dd/mm/yyyy') as Plan_Dt,to_chaR(a.ent_dt,'dd/mm/yyyy') as Planned_Dt,a.Ent_by,to_chaR(a.VISIT_DT,'yyyymmdd') as VDD from wb_Alf_plan a,evas b,evas c where trim(a.ccode)=trim(b.userid) and trim(a.tcode)=trim(c.userid) and a." + branch_Cd + " and A.TYPE='AL' AND A.alfdt " + xprdrange + "  ORDER BY Vdd,A.SRNO";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of ALF PLAN During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F92126":
                    cond = "";
                    SQuery = "select client_cd,client_name,Team_Member,VISIT_DT,Actual_Dt,Team_Code From (Select a.Tcode as Team_Code,C.username as Team_Member,to_chaR(a.VISIT_DT,'dd/mm/yyyy') as VISIT_DT,null as Actual_Dt,b.username as client_cd,b.full_name as client_name,to_char(a.VISIT_DT,'yyyymmdd') as VDD from wb_Alf_plan a,evas b,evas c where trim(a.ccode)=trim(b.userid) and trim(a.tcode)=trim(c.userid) and a." + branch_Cd + " and A.TYPE='AL' AND A.alfdt " + xprdrange + " union all Select c.userid as Team_Code,a.Ent_by as Team_Member,null as Visit_dt,to_chaR(a.cssdt,'dd/mm/yyyy') as Actual_DT,a.Ccode as client_cd,b.full_name as client_name,null as VDD from wb_css_log a,evas b,evas c where trim(a.ccode)=trim(b.username) and trim(a.ent_by)=trim(c.username) and a." + branch_Cd + " and A.TYPE='CS' AND A.cssdt " + xprdrange + " and upper(trim(a.req_type))='VISIT') order by client_cd,VDD ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of ALF PLAN vs Actual During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F92127":
                    SQuery = "select * from (select Client_code,'02-Work' as Data_Grp,nvl(day1,0)+nvl(day2,0)+nvl(day3,0)+nvl(day4,0)+nvl(day5,0)+nvl(day6,0)+nvl(day7,0)+nvl(day8,0)+nvl(day9,0)+nvl(day10,0)+nvl(day11,0)+nvl(day12,0)+nvl(day13,0)+nvl(day14,0)+nvl(day15,0)+nvl(day16,0)+nvl(day17,0)+nvl(day18,0)+nvl(day19,0)+nvl(day20,0)+nvl(day21,0)+nvl(day22,0)+nvl(day23,0)+nvl(day24,0)+nvl(day25,0)+nvl(day26,0)+nvl(day27,0)+nvl(day28,0)+nvl(day29,0)+nvl(day30,0)+nvl(day31,0) Total_CSS,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 from (WITH pivot_data AS (SELECT to_Char(a.cssdt,'dd') as  Mth_no, trim(a.ccode) as Client_Code, count(a.cssno)  as sal FROM wb_Css_log a where a.branchcd!='88' and a.cssdt " + xprdrange + " and upper(a.req_type)='VISIT' group by to_Char(a.cssdt,'dd'),trim(a.ccode) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 ))) " +
                    "union all select Client_code,'01-Plan' as Data_Grp,nvl(day1,0)+nvl(day2,0)+nvl(day3,0)+nvl(day4,0)+nvl(day5,0)+nvl(day6,0)+nvl(day7,0)+nvl(day8,0)+nvl(day9,0)+nvl(day10,0)+nvl(day11,0)+nvl(day12,0)+nvl(day13,0)+nvl(day14,0)+nvl(day15,0)+nvl(day16,0)+nvl(day17,0)+nvl(day18,0)+nvl(day19,0)+nvl(day20,0)+nvl(day21,0)+nvl(day22,0)+nvl(day23,0)+nvl(day24,0)+nvl(day25,0)+nvl(day26,0)+nvl(day27,0)+nvl(day28,0)+nvl(day29,0)+nvl(day30,0)+nvl(day31,0) Total_CSS,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 from (WITH pivot_data AS (SELECT to_Char(a.visit_Dt,'dd') as  Mth_no, trim(b.username) as Client_Code, count(a.alfno)  as sal FROM wb_alf_plan a,evas b where trim(A.ccode)=trim(b.userid) and a.branchcd!='88' and a.visit_Dt " + xprdrange + "  group by to_Char(a.visit_Dt,'dd'),trim(b.username) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 )))) order by  Client_code,Data_grp";

                    SQuery = "select * from (select Client_code,'02-Work' as Data_Grp,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 from (WITH pivot_data AS (SELECT to_Char(a.cssdt,'dd') as  Mth_no, trim(a.ccode) as Client_Code, max(a.ent_by)  as sal FROM wb_Css_log a where a.branchcd!='88' and a.cssdt " + xprdrange + " and upper(a.req_type)='VISIT' group by to_Char(a.cssdt,'dd'),trim(a.ccode) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 ))) " +
                    "union all select Client_code,'01-Plan' as Data_Grp,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 from (WITH pivot_data AS (SELECT to_Char(a.visit_Dt,'dd') as  Mth_no, trim(b.username) as Client_Code, max(c.username)  as sal FROM wb_alf_plan a,evas b,evas c where trim(A.ccode)=trim(b.userid) and trim(A.tcode)=trim(c.userid) and a.branchcd!='88' and a.visit_Dt " + xprdrange + "  group by to_Char(a.visit_Dt,'dd'),trim(b.username) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 )))) order by  Client_code,Data_grp";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Day Wise Client wise Visits Recorded during " + value1 + " to " + value2, frm_qstr);
                    break;
            }
        }
    }
}