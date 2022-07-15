using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_css : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string party_cd, part_cd;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID; TimeSpan Diff;
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
                case "F95133":
                case "F20127":
                case "F20121":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
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

                case "F15127":
                    SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;

                
                case "F20203":
                    fgen.Fn_open_dtbox("Select Date", frm_qstr);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", "Y");
                    break;

                case "F20204":
                    SQuery = "select distinct a.route_name as fstr, a.route_name , a.branchcd, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ent_by from wb_sa_route a where a.branchcd='" + mbr + "' and  type= '11' order by a.route_name";
                    header_n = "Select Route";
                    break;

                case "F20206":
                    //NFC APP REPORTS- View Registered Routes
                    SQuery = "select a.branchcd,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.route_name,a.location, a.ent_by from wb_sa_route a where a.branchcd='" + mbr + "' and  type= '11' order by a.route_name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Patrolling Route Checklist ", frm_qstr);
                    break;

                case "F20207":
                    //NFC APP REPORTS- View Registered Cards
                    SQuery = "select a.branchcd,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.card_no,a.location, a.ent_by from wb_sa_card a where a.branchcd='" + mbr + "' and  type= '10' order by a.location";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Registered Cards Checklist ", frm_qstr);
                    break;

                case "F20208":
                    SQuery = "SELECT distinct trim(branchcd)||'~'||trim(route_name)||'~'||trim(vchnum)||'~'||to_char(vchdate,'dd/mm/yyyy') as fstr, route_name,branchcd,to_char(vchdate,'dd/mm/yyyy') as vchdate ,trim(ent_by),to_char(ent_dt,'dd/mm/yyyy') as ent_dt FROM wb_sa_route WHERE type='11' and branchcd='" + mbr + "' orDER BY route_name";
                    header_n = "Select Route";
                    break;

                case "F20209":
                    SQuery = "SELECT trim(branchcd)||'~'||trim(card_no)||'~'||trim(vchnum)||'~'||to_char(vchdate,'dd/mm/yyyy')||'~'||trim(location) AS FSTR,branchcd, location ,Card_no, ent_by, To_char(ent_dt,'dd/mm/yyyy') as ent_date FROM wb_sa_card WHERE type='10' and branchcd='" + mbr + "' ORDER BY location";
                    header_n = "Select Card";
                    break;

                case "F20210":
                    SQuery = "select distinct trim(a.ent_by) as fstr, trim(a.ent_by) as ent_by, a.branchcd from wb_sa_record a where a.branchcd='" + mbr + "' and  type= '12' order by trim(a.ent_by)";
                    header_n = "Select User";
                    break;

                case "F20211":
                    fgen.Fn_open_dtbox("Select Date", frm_qstr);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", "Y");
                    break;

                case "F20205"://to view images stored
                    SQuery = "SELECT trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,username,to_char(vchdate,'dd/mm/yyyy') as vchdate,ent_by as entered_by,to_char(ent_dt) as entry_dt FROM WB_SA_IMG where branchcd='00' and type='10' order by vchnum";
                    header_n = "Select User clicked image you want to see!!";
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15127" || val == "F20204" || val == "F20210")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
            {
                value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                hfcode.Value = "";
                hfcode.Value = value1;
                col1 = value1;
                switch (val)
                {
                    case "F20208":
                        fgen.execute_cmd(frm_qstr, co_cd, "delete from wb_sa_route where trim(branchcd)||'~'||trim(route_name)||'~'||trim(vchnum)||'~'||to_char(vchdate,'dd/mm/yyyy')='" + value1 + "' ");
                        fgen.save_info(frm_qstr, co_cd, mbr, col1.Split('~')[2].ToString(), col1.Split('~')[3].ToString(), uname, "11", "SA Route Deleted");
                        fgen.msg("-", "AMSG", "Selected Route Entry deleted.");
                        break;
                    case "F20209":
                        fgen.execute_cmd(frm_qstr, co_cd, "delete from wb_sa_card where trim(branchcd)||'~'||trim(card_no)||'~'||trim(vchnum)||'~'||to_char(vchdate,'dd/mm/yyyy')||'~'||trim(location)='" + value1 + "' ");
                        fgen.save_info(frm_qstr, co_cd, mbr, col1.Split('~')[2].ToString(), col1.Split('~')[3].ToString(), uname, "11", "SA Card Deleted");
                        fgen.msg("-", "AMSG", "Selected Card Entry deleted.");
                        break;

                    case "F20205":
                        col1 = hfcode.Value;
                        string imgname = fgen.seek_iname(frm_qstr, co_cd, "select vchnum,username,imagepath  from wb_sa_img where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'", "imagepath");
                        //string filePath = "../tej-base/dp/" + imgname + "";
                        string filePath = Server.MapPath("../tej-base/dp/" + imgname + "");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
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


                }
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
                case "F95121":
                    // proj targets
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select vchnum as Proj_No,to_chaR(vchdate,'dd/mm/yyyy') as Proj_dt,srno as line_no,trim(CCode)as Cust,cust_name as Customer,mod_detail as Module_Detail,max(remarks)as Remarks,sum(Hrs_Allocated) as Hrs_Allocated,sum(hr_done) as Hr_done,sum(Hrs_Allocated)-sum(hr_done) as Hr_balance,max(Last_entry_by) as Last_entry_by,max(Last_entry) as Last_entry,to_chaR(vchdate,'yyyymmdd') as VDD from (Select vchnum,vchdate,srno,ccode,cust_name,mod_detail,remarks,mod_hrs as Hrs_Allocated,0 as hr_done,null as Last_entry_by,null as Last_entry from wb_proj_setup where branchcd='" + mbr + "' and ccode like '" + party_cd + "%'  union all Select vchnum,vchdate,srno,ccode,cust_name,mod_detail,null as remarks,0 as hr_req,train_hrs as train_done,ent_by as lent,ent_dt as last_ent from wb_proj_log where branchcd='" + mbr + "' and ccode like '" + party_cd + "%' and ent_by like '" + part_cd + "%' ) group by vchnum,vchdate,srno,trim(CCode),cust_name,mod_detail,to_chaR(vchdate,'yyyymmdd') having sum(Hrs_Allocated)-sum(hr_done)<=0 order by to_chaR(vchdate,'yyyymmdd') ,vchnum ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Project Delivery Vs Update Checklist Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F95132":
                    // proj update
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select vchnum as Proj_No,to_chaR(vchdate,'dd/mm/yyyy') as Proj_dt,srno as line_no,trim(CCode)as Cust,cust_name as Customer,mod_detail as Module_Detail,max(remarks)as Remarks,sum(Hrs_Allocated) as Hrs_Allocated,sum(hr_done) as Hr_done,sum(Hrs_Allocated)-sum(hr_done) as Hr_balance,max(Last_entry_by) as Last_entry_by,max(Last_entry) as Last_entry,to_chaR(vchdate,'yyyymmdd') as VDD from (Select vchnum,vchdate,srno,ccode,cust_name,mod_detail,remarks,mod_hrs as Hrs_Allocated,0 as hr_done,null as Last_entry_by,null as Last_entry from wb_proj_setup where branchcd='" + mbr + "' and ccode like '" + party_cd + "%'  union all Select vchnum,vchdate,srno,ccode,cust_name,mod_detail,null as remarks,0 as hr_req,train_hrs as train_done,ent_by as lent,ent_dt as last_ent from wb_proj_log where branchcd='" + mbr + "' and ccode like '" + party_cd + "%' and ent_by like '" + part_cd + "%' ) group by vchnum,vchdate,srno,trim(CCode),cust_name,mod_detail,to_chaR(vchdate,'yyyymmdd') having sum(Hrs_Allocated)-sum(hr_done)>0 order by to_chaR(vchdate,'yyyymmdd') ,vchnum ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Project Delivery Vs Update Checklist Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F95133":
                    // proj targt vs update
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select vchnum as Proj_No,to_chaR(vchdate,'dd/mm/yyyy') as Proj_dt,srno as line_no,trim(CCode)as Cust,cust_name as Customer,mod_detail as Module_Detail,max(remarks)as Remarks,sum(Hrs_Allocated) as Hrs_Allocated,sum(hr_done) as Hr_done,sum(Hrs_Allocated)-sum(hr_done) as Hr_balance,max(Last_entry_by) as Last_entry_by,max(Last_entry) as Last_entry,to_chaR(vchdate,'yyyymmdd') as VDD from (Select vchnum,vchdate,srno,ccode,cust_name,mod_detail,remarks,mod_hrs as Hrs_Allocated,0 as hr_done,null as Last_entry_by,null as Last_entry from wb_proj_setup where branchcd='" + mbr + "' and ccode like '" + party_cd + "%'  union all Select vchnum,vchdate,srno,ccode,cust_name,mod_detail,null as remarks,0 as hr_req,train_hrs as train_done,ent_by as lent,ent_dt as last_ent from wb_proj_log where branchcd='" + mbr + "' and ccode like '" + party_cd + "%' and ent_by like '" + part_cd + "%' ) group by vchnum,vchdate,srno,trim(CCode),cust_name,mod_detail,to_chaR(vchdate,'yyyymmdd')  order by to_chaR(vchdate,'yyyymmdd') ,vchnum ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Project Delivery Vs Update Checklist Period " + fromdt + " to " + todt, frm_qstr);
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
                    fgen.Fn_DrillReport("Gate Outward Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;




            }
        }
    }
}