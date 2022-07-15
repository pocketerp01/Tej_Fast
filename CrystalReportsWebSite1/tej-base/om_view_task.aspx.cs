using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_task : System.Web.UI.Page
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
                case "F10054":
                    SQuery = "select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.vchnum as reqno,to_char(a.vchdate,'dd/mm/yyyy') as reqdt,a.col8 as machine_srno,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,b.aname as customer,c.iname as product,a.acode,a.icode,to_Char(A.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='CC' and a.vchdate " + xprdrange + " order by vdd desc,a.vchnum desc";
                    SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.col22 as reqno,a.col23 as reqdt,a.col3 as nature_of_complaint,d.col8 as machine_srno,a.col5 as inv_no,a.col6 as inv_Dt,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(A.vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c,scratch d where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||trim(a.col22)||trim(a.col23)||Trim(A.acode)||trim(A.icode)=d.branchcd||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||Trim(d.acode)||trim(d.icode) and a.type='AC' and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " order by vdd desc,a.vchnum desc";
                    header_n = "Select Customer Request";
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

                    case "F10054":
                        SQuery = "select d.branchcd,d.vchnum as reqno,to_char(d.vchdate,'dd/mm/yyyy') as req_dt,d.acode as party_code,b.aname,b.addr1 as paddr1,b.addr2 as paddr2,c.iname,c.cpartno,d.col1 as application,d.col2 as type_of_complaint,d.col3 as nature_of_complaint,d.col4 as DivisionofComplaint,d.col8 as machine_srno,d.col9 as inv_qty,d.col13 as guaranty_dt,d.ent_by,to_char(d.ent_dt,'dd/mm/yyyy') as ent_dt,d.remarks,d.edt_by,(case when nvl(trim(d.edt_by),'-')!='-' then to_char(d.edt_dt,'dd/mm/yyyy') else '-' end) as edt_dt,d.app_by,(case when nvl(trim(d.app_by),'-')!='-' then to_char(d.app_dt,'dd/mm/yyyy') else '-' end) as app_dt,d.chk_by,(case when nvl(trim(d.chk_by),'-')!='-' then to_char(d.chk_dt,'dd/mm/yyyy') else '-' end) as chk_dt,d.invno,to_char(d.invdate,'dd/mm/yyyy') as invdate,a.vchnum as act_entry_no,to_char(a.vchdate,'dd/mm/yyyy') as act_entry_dt,a.col15 as reply_to_customer,A.col16 as actiontaken,a.col17 AS correctiveaction,a.col18 as factfinding,a.col19 as tktstatus,a.ent_by as act_taken_entby,to_char(a.ent_Dt,'dd/mm/yyyy') as act_taken_entdt,a.col27 as cost_if_any,a.col28 as our_person from scratch d,famst b,item c,scratch2 a where A.branchcd||trim(A.col22)||trim(A.col23)||trim(A.acode)||trim(a.icode)||a.srno=trim(D.branchcd)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(D.acode)||trim(d.icode)||d.srno and trim(a.acode)=trim(b.acodE) and trim(d.acode)=trim(b.acode) and trim(a.icode)=trim(c.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + value1 + "' and d.type='CC' order by a.srno";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Request Status", frm_qstr);
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
                case "F90121":
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
                    SQuery = "SELECT a.TRCNO as TRC_NO,to_char(A.TRCDT,'dd/mm/yyyy') as TRC_Dt,a.CCODE,a.Client_Name,a.Task_type,a.Team_member,a.Tgt_days as Time_Limit,a.Client_Person,a.Client_Phone,a.Oremarks,a.Ent_Dt,last_Action,last_Actdt,to_chaR(a.TRCDT,'YYYYMMDD') as TRC_DTd FROM WB_TASK_LOG a WHERE a." + branch_Cd + " and A.TYPE='TR' AND A.TRCDT " + xprdrange + "  " + cond + " ORDER BY trc_DTd,A.tRCNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Task Assigned During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F90126":
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
                    SQuery = "SELECT a.TACNO as TAC_No,to_char(A.TACDT,'dd/mm/yyyy') as TAC_Dt,a.TRCNO as Task_NO,to_char(A.TRCDT,'dd/mm/yyyy') as Task_Dt,a.Ccode,a.Client_name,a.Tgt_Days,a.Task_type,a.Team_member,a.Time_Taken,a.Act_mode,a.Curr_stat,a.Ent_Dt,a.Ent_by,to_chaR(a.TACDT,'YYYYMMDD') as LAC_DTd  FROM WB_Task_ACT a WHERE a." + branch_Cd + " and A.TYPE='TA' AND A.TACDT " + xprdrange + "  " + cond + " ORDER BY lac_DTd,A.TaCNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Assigned Task Action During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F90131":
                    cond = "";
                    if (ulvl != "0") cond = " and trim(a.ent_by)='" + uname + "'";
                    SQuery = "Select '1.Task Assigned' as L_Status,Client_name,trim(TRCno) as TRCno,to_Char(TRCdt,'dd/mm/yyyy') as TRCdt,Task_type,Tsubject,Team_member,Tgt_Days,ORemarks as Remarks,ent_by,ent_dt,'-' as curr_Stat,to_Char(TRCdt,'yyyymmdd') as ldd,null as act_mode,null as Time_Taken,ccode,0 as TAT_Days from WB_Task_LOG where " + branch_Cd + " and TYPE='TR' AND TRCdt " + xprdrange + " union all  Select '2.Task Action' as L_Status,Client_Name,trim(TRCno) as TRCno,to_Char(TRCdt,'dd/mm/yyyy') as TRCdt,Task_type,Tsubject,Team_member,Tgt_Days,ORemarks as remarks,ent_by,ent_dt,Curr_Stat,to_Char(TACdt,'yyyymmdd') as ldd,act_mode,Time_Taken,Ccode,tacdt-trcdt as TAT_Days from WB_Task_ACT where " + branch_Cd + " and TYPE='TA' AND TRCdt " + xprdrange + " ";
                    SQuery = "Select TRCno,TRCdt,CCode,CLIENT_Name,Ent_dt as Entry_Dt,L_Status,TSUBJECT,Team_member,Remarks,Curr_Stat,Act_Mode as Last_Action,TAT_Days,Time_Taken,Ent_by,Ldd from (" + SQuery + ") order by ldd,TRCno,ent_dt";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Task Assigned Report Status,Date Wise During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F90136":
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
                    SQuery = "SELECT a.TRCNO as TRC_NO,to_char(A.TRCDT,'dd/mm/yyyy') as TRC_Dt,a.CCODE,a.Client_Name,a.Task_type,a.Team_member,a.Tgt_days as Time_Limit,a.Client_Person,a.Client_Phone,a.Oremarks,a.Ent_Dt,last_Action,last_Actdt,to_chaR(a.TRCDT,'YYYYMMDD') as TRC_DTd FROM WB_TASK_LOG a WHERE a." + branch_Cd + " and A.TYPE='TR' AND A.TRCDT " + xprdrange + "  " + cond + " and trim(nvl(curr_stat,'-'))='-' ORDER BY trc_DTd,A.tRCNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Task Assigned ,Pending Action During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F10053":
                    SQuery = "";
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,'-' AS GSTR,(case when trim(nvl(a.chk_by,'-'))!='-' then 'Closed' else 'Pending' end) as status,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as cmplnt_no,to_char(a.vchdate,'dd/mm/yyyy') as cmpnt_dt,a.col8 as machine_srno,b.aname as party_name,c.iname as item_name,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt ,a.vchnum as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd='" + mbr + "' and a.type='CC' and a.vchdate " + xprdrange + " order by a.vchnum desc", frm_qstr);
                    fgen.drillQuery(1, "SELECT distinct '-' as fstr,a.branchcd||'CC'||trim(a.col22)||trim(a.col23)||trim(a.acode)||trim(a.icode) as gstr,a.col19 as status,a.col22 as reqno,a.col23 as reqdt,b.aname as party,c.iname as product,d.COL8 as machine_srno,a.vchnum as actionno,to_char(a.vchdate,'dd/mm/yyyy') as actiondt,a.col15 as reply_to_customer,a.col16 as action_taken,a.col17 as corrective_action,a.col18 as fact_finding,a.acode,a.col5 as invno,a.col6 as invdate,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,c.cpartno,a.icode as erpcode,to_char(a.vchdate,'yyyymmdd') as vdd from scratch2 a,famst b, item c,scratch d where trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and a." + branch_Cd + " and a.type='AC' and a.branchcd||trim(a.col22)||trim(a.col23)||Trim(A.acode)||trim(A.icode)=d.branchcd||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||Trim(d.acode)||trim(d.icode) and a.vchdate " + xprd2 + " order by vdd desc,a.vchnum desc", frm_qstr);
                    cond = "";
                    if (hfbr.Value == "ABR") cond = "Consolidate";
                    else cond = "Branch Wise";
                    fgen.Fn_DrillReport("Status Report of Customer Request`s from " + value1 + " to " + value2 + "(" + cond + ")", frm_qstr);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    break;
                case "F10052S":
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.col8 as machine_srno,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.col12 as guarantee,a.col13 as guarantee_dt,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a." + branch_Cd + " and a.type='CC' and a.vchdate " + xprdrange + " order by vdd desc,a.srno";
                    if (hfbr.Value == "ABR") cond = "Consolidate";
                    else cond = "Branch Wise";
                    //fgen.Fn_DrillReport("Summary Report of Customer Request`s from " + value1 + " to " + value2 + "(" + cond + ")", frm_qstr);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Summary Report of Customer Request`s from " + value1 + " to " + value2 + "(" + cond + ")", frm_qstr);
                    break;                
            }
        }
    }
}