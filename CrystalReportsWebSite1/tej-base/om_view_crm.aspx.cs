using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_crm : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld, party_cd = "";
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

                case "F45136":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F45137":
                case "F45138":
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
                case "F45162":
                    SQuery = "select Aname as Contact_Name,Acode as Contact_Code,Pname as Alias_Name,Addr1,Addr2,Staten,GST_NO,GIRNO,PERSON,MOBILE,VENCODE as Parent_Cust,tdsnum,tcsnum from csmst_crm where branchcd!='DD' order by aname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("CRM Contacts List ", frm_qstr);
                    break;
                case "F45121":
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
                    SQuery = "SELECT a.LRCNO as LRC_NO,to_char(A.LRCDT,'dd/mm/yyyy') as LRC_Dt,a.Ldescr as Co_Name,a.Lead_dsg,a.LVertical as Industry,a.Lgrade as Grading,a.Lsubject as Interest_in,a.Lremarks as Lead_Remark,a.Oremarks as Our_Remark,a.Cont_name,a.Cont_No,a.Cont_Email,a.Ent_Dt,last_Action,last_Actdt,a.app_by,a.app_dt,to_chaR(a.LRCDT,'YYYYMMDD') as LRC_DTd  FROM WB_LEAD_LOG a WHERE a." + branch_Cd + " and A.TYPE='LR' AND A.LRCDT " + xprdrange + "  " + cond + " ORDER BY lrc_DTd,A.LRCNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Customer Leads Logging During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F45126":
                    if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
                    SQuery = "SELECT a.LACNO as LAC_NO,to_char(A.LACDT,'dd/mm/yyyy') as LAC_Dt,a.LRCNO as LEAD_NO,to_char(A.LRCDT,'dd/mm/yyyy') as LEad_Dt,a.LDESCR as Client,a.LSUBJECT as Specification,a.Cont_name,a.Cont_No,a.Cont_Email,a.Curr_Stat as Our_Analysis,a.ORemarks,a.Next_Folo as Next_Folo_Days,a.Ent_Dt,to_chaR(a.LACDT,'YYYYMMDD') as LAC_DTd  FROM WB_LEAD_ACT a WHERE a." + branch_Cd + " and A.TYPE='LA' AND A.LACDT " + xprdrange + "  " + cond + " ORDER BY lac_DTd,A.LaCNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Customer Leads Action During " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F45131":
                    cond = "";
                    if (ulvl != "0") cond = " and trim(a.ent_by)='" + uname + "'";
                    SQuery = "Select '1.Lead Logged' as L_Status,LDESCR,trim(LRCno) as LRCno,to_Char(LRCdt,'dd/mm/yyyy') as LRCdt,LSUBJECT,Cont_name,Cont_no,LRemarks as Remarks,ent_by,ent_dt,'-' as curr_Stat,to_Char(LRCdt,'yyyymmdd') as ldd,null as act_mode,null as Next_Folo from WB_LEAD_LOG where " + branch_Cd + " and TYPE='LR' AND LRCdt " + xprdrange + " union all  Select '2.Lead Action' as L_Status,LDESCR,trim(LRCno) as LRCno,to_Char(LRCdt,'dd/mm/yyyy') as LRCdt,Lsubject,Cont_name,Cont_no,ORemarks as remarks,ent_by,ent_dt,Curr_Stat,to_Char(LACdt,'yyyymmdd') as ldd,act_mode,Next_Folo from WB_lead_ACT where " + branch_Cd + " and TYPE='LA' AND LRCdt " + xprdrange + " ";

                    SQuery = "Select LRCno,LRCdt,LDESCR AS CLIENT,ent_dt,L_Status,LSUBJECT,Cont_name as Contact_person,Remarks,Curr_Stat,Act_Mode as Last_Action,Next_Folo as Next_Folo_Days,Cont_no,Ent_by,Ldd from (" + SQuery + ") order by ldd,LRCno,ent_dt";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Report of Lead Action Report Status,Date Wise During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F45132":
                case "F45133":
                case "F45139":
                case "F45144":
                case "F45145":
                case "F45146":
                case "F45148":
                    string mfield1 = "";
                    string mfield2 = "";
                    switch (val)
                    {
                        case "F45132":
                            mfield1 = "LVertical";
                            mfield2 = "Industry_Name";
                            break;
                        case "F45133":
                            mfield1 = "reffby";
                            mfield2 = "Salesman_Name";
                            break;
                        case "F45144":
                            mfield1 = "lead_type";
                            mfield2 = "Lead_Source";
                            break;
                        case "F45145":
                            mfield1 = "lead_cntry";
                            mfield2 = "Lead_Country";
                            break;
                        case "F45146":
                            mfield1 = "lead_state";
                            mfield2 = "Lead_State";
                            break;
                        case "F45139":
                            mfield1 = "Lgrade";
                            mfield2 = "Lead_Category";
                            break;
                        case "F45148":
                            mfield1 = "Last_Action";
                            mfield2 = "Last_Action";
                            break;
                    }



                    //mq1 = "select fld_name as fstr, '-' as gstr,fld_name as " + mfield2 + " , ctr as No_of_leads,Lead_Value,Quot_val from (select sum(quot_val) as quot_val,sum(a.expval) as Lead_Value,sum(a.ctr) as ctr,a.fld_name from (SELECT 1 as ctr,a." + mfield1 + " as fld_name,a.expval,0 as quot_val,a.branchcd||'-'||a.lrcno||'-'||to_char(a.lrcdt,'dd/mm/yyyy') As lead_link FROM WB_LEAD_LOG a where a." + branch_Cd + " and a.type='LR' and a.LRCdt  " + xprdrange + "  " + cond + " union all SELECT 0 as ctr,null as fld_name,0 as expval,basic as quot_val,polink FROM somasq a where a." + branch_Cd + " and a.type like '4%' and a.orddt " + xprdrange + "  )a group by a.fld_name) order by fld_name";
                    mq1 = "select fld_name as fstr, '-' as gstr,fld_name as " + mfield2 + " , sum(ctr) as No_of_leads,sum(Lead_Value) as Lead_Value,sum(Quot_val) as Quot_val,sum(Lead_Value)-sum(Quot_val) as Difference,(case when sum(Lead_Value)>0 then round((sum(Lead_Value)-sum(Quot_val))/sum(Lead_Value),2)*100 else 0 end) as In_Percent from (select sum(quot_val) as quot_val,sum(a.expval) as Lead_Value,sum(a.ctr) as ctr,max(fld_name) as fld_name,trim(a.lead_link) from (SELECT 1 as ctr,a." + mfield1 + " as fld_name,a.expval,0 as quot_val,a.branchcd||'-'||a.lrcno||'-'||to_char(a.lrcdt,'dd/mm/yyyy') As lead_link FROM WB_LEAD_LOG a where a." + branch_Cd + " and a.type='LR' and a.LRCdt  " + xprdrange + "  " + cond + " union all SELECT 0 as ctr,null as fld_name,0 as expval,basic as quot_val,polink FROM somasq a where a." + branch_Cd + " and a.type like '4%' and a.orddt " + xprdrange + "  )a group by trim(a.lead_link)) group by fld_name order by fld_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "4#5#6#7#8#", "3#4#5#6#7#8#", "750#100#100#100#100#100#");
                    mq2 = "select '-' as fstr, a." + mfield1 + " as gstr,a.Ldescr as Co_Name,a.Lead_dsg,a.Lgrade as L_Type,a.Lsubject as Interest_in,last_Action,last_Actdt,a.expval as Expected_Value,a.Lremarks as Lead_Remark,a.Oremarks as Our_Remark,a.Cont_name,a.Cont_No,a.Cont_Email,a.Ent_Dt,a.app_by,a.LRCNO as LRC_NO,to_char(A.LRCDT,'dd/mm/yyyy') as LRC_Dt,a.app_dt,to_chaR(a.LRCDT,'YYYYMMDD') as LRC_DTd FROM WB_LEAD_LOG a where a." + branch_Cd + " and a.type='LR' and a.LRCdt  " + xprdrange + "  " + cond + " order by a.Ldescr";
                    fgen.drillQuery(1, mq2, frm_qstr, "1#", "3#4#5#6#7#8#9#", "200#125#125#125#125#125#125#");
                    fgen.Fn_DrillReport(mfield2 +" Wise Leads Logged During " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F45150"://lead history
                    mq1 = "select lead_type as fstr, '-' as gstr,lead_type as Sales_Agent, ctr  from (select sum(a.ctr) as ctr,a.lead_type from (SELECT 1 as ctr,a.lead_type FROM WB_LEAD_LOG a where a." + branch_Cd + " and a.type='LR' and a.LRCdt  " + xprdrange + "  " + cond + " )a group by a.lead_type)";
                    fgen.drillQuery(0, mq1, frm_qstr, "2#", "3#4#", "900#100#");
                    mq2 = "select '-' as fstr, a.lead_type as gstr,a.Ldescr as Co_Name,a.Lead_dsg,a.Lgrade as L_Type,a.Lsubject as Interest_in,last_Action,last_Actdt,a.Lremarks as Lead_Remark,a.Oremarks as Our_Remark,a.Cont_name,a.Cont_No,a.Cont_Email,a.Ent_Dt,a.app_by,a.LRCNO as LRC_NO,to_char(A.LRCDT,'dd/mm/yyyy') as LRC_Dt,a.app_dt,to_chaR(a.LRCDT,'YYYYMMDD') as LRC_DTd FROM WB_LEAD_LOG a where a." + branch_Cd + " and a.type='LR' and a.LRCdt  " + xprdrange + "  " + cond + " order by a.Ldescr";
                    fgen.drillQuery(1, mq2, frm_qstr, "1#", "3#6#7#8#9#10#", "200#150#150#80#200#200#");
                    fgen.Fn_DrillReport("Source Wise Analysis of Leads During " + value1 + " to " + value2, frm_qstr);
                    break;


                case "F45136":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (party_cd.Length < 2)
                    {
                        cond = "ent_by like '%'";
                    }
                    else
                    {
                        cond = "ent_by='" + party_cd + "'";
                    }
                    SQuery = "select ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,trim(aname) as company,col7 as place,col8 as purpose,col9 as location,col11 as In_Time,col12 as Out_Time from exp_book where branchcd='" + mbr + "' and type='EB' and vchdate " + xprdrange + " and " + cond + " order by ent_by";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Detailed checkin Report for the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F45137":
                    SQuery = "select ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,trim(aname) as company,col7 as place,col8 as purpose,col9 as location,min(col11) as In_Time,max(col12) as Out_Time from exp_book where branchcd='" + mbr + "' and type='EB' and TO_CHAR(vchdate,'DD/MM/YYYY')='" + value1 + "' GROUP BY ent_by,ent_dt,aname ,col7 ,col8 ,col9 order by ent_by";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Daily checkin Report for the Date " + value1 + "", frm_qstr);
                    break;

                case "F45138":
                    SQuery = "select ent_by,TO_CHAR(ent_dt,'DD/MM/YYYY') AS ENT_DT,trim(aname) as company,col7 as place,col8 as purpose,col9 as location,col11 as In_Time,col12 as Out_Time from exp_book where branchcd='" + mbr + "' and type='EB' and to_char(vchdate,'dd/MM/yyyy')='" + value1 + "' AND NVL(COL11,'-')!='-' AND  NVL(COL12,'-')='-' order by ent_by";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("No check out Report on " + value1 + "", frm_qstr);
                    break;

                    
            }
        }
    }
}