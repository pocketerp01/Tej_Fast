using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_gate : System.Web.UI.Page
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
                    SQuery = "select a.branchcd,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.route_name,a.location, a.ent_by,a.time as act_time from wb_sa_route a where a.branchcd='" + mbr + "' and  type= '11' order by a.route_name,IS_NUMBER(REPLACE(a.LOCATION,'CH',''))";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Patrolling Route Checklist ", frm_qstr);
                    break;

                case "F20207":
                    //NFC APP REPORTS- View Registered Cards
                    SQuery = "select a.branchcd,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.card_no,a.location, a.ent_by from wb_sa_card a where a.branchcd='" + mbr + "' and  type= '10' order by IS_NUMBER(REPLACE(a.LOCATION,'CH',''))";
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
                        hfcode.Value = value1;
                        fgen.msg("-", "SMSG", "Are You Sure, You want to Delete!!");
                        break;
                    case "F20209":
                        hfcode.Value = value1;
                        fgen.msg("-", "SMSG", "Are You Sure, You want to Delete!!");
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
                case "F20121":
                    // Gate Inward Checklist
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='00' and a.acode like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' ", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate Inward Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F20126":
                    // Gate Outward Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='2G'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate Outward Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F20127":
                    // Gate PO Checklist
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.acode like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate PO Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F20128":
                    // Gate RGP Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate RGP Checklist for the Period " + value1 + " to " + value2, frm_qstr);
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

                case "F20203":
                    //NFC APP REPORTS-  Patrolling Route Report
                    SQuery = "select distinct A.VCHDATE,A.card_no,TRIM(A.route_name) AS ROUTE_NAME,max(A.tgt_time) as tgt_time,max(A.act_time) as act_time,A.ENT_BY as person,B.LOCATION from (Select type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,card_no,TRIM(route_name) AS route_name,srno,time as tgt_time,null as act_time,ENT_BY from wb_Sa_route where branchcd='" + mbr + "' union all Select type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,card_no,TRIM(route_name) AS route_name,null as srno,null as tgt_time,time as act_time,ENT_BY from wb_sa_record where branchcd='" + mbr + "' and to_char(vchdate,'dd/mm/yyyy')='" + value1 + "') A , WB_SA_CARD B WHERE TRIM(A.CARD_NO)=TRIM(B.CARD_NO) group by A.card_no,TRIM(A.route_name),A.VCHDATE,A.ENT_BY,B.LOCATION"; //BY AKSHAY
                    SQuery = "select max(a.ent_by) as ent_by,max(to_char(a.vchdate,'dd/mm/yyyy')) as ent_dt,b.location,a.card_no,a.route_name,max(a.tgt_time) as tgt_time,max(a.act_time) as act_time from (select trim(card_no) as card_no,trim(route_name) as route_name,time as tgt_time,null as act_time,null as ent_by,null as vchdate from wb_sa_route where branchcd='" + mbr + "' union all select trim(card_no) as card_no,trim(route_name) as route_name,null as tgt_time,time as act_time,ent_by,vchdate from wb_sa_record where branchcd='" + mbr + "' AND to_Char(vchdate,'dd/mm/yyyy')='" + value1 + "') a,wb_sa_card b where trim(a.card_no)=trim(b.card_no) group by a.card_no,a.route_name,b.location having length(max(a.ent_by))>1 order by b.location,a.route_name,a.card_no";
                    SQuery = "SELECT A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY HH24:MI') AS ENT_DT,B.LOCATION,A.CARD_NO,A.ROUTE_NAME,to_char(to_datE(sysdate||' '||B.TIME,'dd/mm/yyyy HH24:MI'),'HH24:MI') AS TGT_TIME,A.TIME AS ACT_TIME FROM WB_SA_RECORD A,WB_SA_ROUTE B WHERE TRIM(A.CARD_NO)||TRIM(A.ROUTE_NAME)=TRIM(b.CARD_NO)||TRIM(b.ROUTE_NAME) AND A.BRANCHCD='" + mbr + "' AND B.BRANCHCD='" + mbr + "' AND TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + value1 + "' ORDER BY A.ROUTE_NAME,IS_NUMBER(REPLACE(B.LOCATION,'CH','')),B.TIME ";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt.Columns.Add("Diff", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["tgt_time"].ToString().Trim().Length > 1 && dt.Rows[i]["act_time"].ToString().Trim().Length > 1)
                        {
                            date1 = Convert.ToDateTime(dt.Rows[i]["tgt_time"].ToString().Trim());
                            date2 = Convert.ToDateTime(dt.Rows[i]["act_time"].ToString().Trim());
                            Diff = date1 - date2;
                            dt.Rows[i]["Diff"] = (Diff.TotalHours.ToString()).toDouble(2);
                        }
                        else
                        {
                            dt.Rows[i]["Diff"] = "Invalid Time";
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Patrolling Route Tie up Checklist for " + value1 + "", frm_qstr);
                    break;

                case "F20204":
                    //NFC APP REPORTS-Patrolling Done route wise Report
                    SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ent_by,a.route_name,a.time as act_time, b.location from wb_sa_record a ,wb_sa_route b where a.branchcd='" + mbr + "' and  a.type= '12' and b.type='11' and a.vchdate " + xprdrange + "  and trim(a.card_no)=trim(b.card_no) and trim(a.route_name)='" + hfcode.Value + "'order by a.vchdate, a.ent_by";

                    SQuery = "SELECT A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY HH24:MI') AS ENT_DT,B.LOCATION,A.CARD_NO,A.ROUTE_NAME,to_char(to_datE(sysdate||' '||B.TIME,'dd/mm/yyyy HH24:MI'),'HH24:MI') AS TGT_TIME,A.TIME AS ACT_TIME FROM WB_SA_RECORD A,WB_SA_ROUTE B WHERE TRIM(A.CARD_NO)||TRIM(A.ROUTE_NAME)=TRIM(b.CARD_NO)||TRIM(b.ROUTE_NAME) AND A.BRANCHCD='" + mbr + "' AND B.BRANCHCD='" + mbr + "' AND A.VCHDATE " + xprdrange + " AND TRIM(A.ROUTE_NAME)='" + hfcode.Value + "' ORDER BY ENT_DT,A.ROUTE_NAME,IS_NUMBER(REPLACE(B.LOCATION,'CH','')),B.TIME ";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt.Columns.Add("Diff", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["tgt_time"].ToString().Trim().Length > 1 && dt.Rows[i]["act_time"].ToString().Trim().Length > 1)
                        {
                            date1 = Convert.ToDateTime(dt.Rows[i]["tgt_time"].ToString().Trim());
                            date2 = Convert.ToDateTime(dt.Rows[i]["act_time"].ToString().Trim());
                            Diff = date1 - date2;
                            dt.Rows[i]["Diff"] = (Diff.TotalHours.ToString()).toDouble(2);
                        }
                        else
                        {
                            dt.Rows[i]["Diff"] = "Invalid Time";
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Patrolling Route Checklist Route wise for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F20210":
                    //NFC APP REPORTS-Patrolling Done Person wise Report
                    SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ent_by,a.route_name,a.time as act_time, b.location from wb_sa_record a ,wb_sa_route b where a.branchcd='" + mbr + "' and  a.type= '12' and b.type='11' and a.vchdate " + xprdrange + "  and trim(a.card_no)=trim(b.card_no) and trim(a.ent_by)='" + hfcode.Value + "'order by a.vchdate, a.ent_by";
                    SQuery = "SELECT A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY HH24:MI') AS ENT_DT,B.LOCATION,A.CARD_NO,A.ROUTE_NAME,to_char(to_datE(sysdate||' '||B.TIME,'dd/mm/yyyy HH24:MI'),'HH24:MI') AS TGT_TIME,A.TIME AS ACT_TIME FROM WB_SA_RECORD A,WB_SA_ROUTE B WHERE TRIM(A.CARD_NO)||TRIM(A.ROUTE_NAME)=TRIM(b.CARD_NO)||TRIM(b.ROUTE_NAME) AND A.BRANCHCD='" + mbr + "' AND B.BRANCHCD='" + mbr + "' AND A.VCHDATE " + xprdrange + " AND TRIM(A.ENT_BY)='" + hfcode.Value + "' ORDER BY ENT_DT,A.ROUTE_NAME,IS_NUMBER(REPLACE(B.LOCATION,'CH','')),B.TIME ";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt.Columns.Add("Diff", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["tgt_time"].ToString().Trim().Length > 1 && dt.Rows[i]["act_time"].ToString().Trim().Length > 1)
                        {
                            date1 = Convert.ToDateTime(dt.Rows[i]["tgt_time"].ToString().Trim());
                            date2 = Convert.ToDateTime(dt.Rows[i]["act_time"].ToString().Trim());
                            Diff = date1 - date2;
                            dt.Rows[i]["Diff"] = (Diff.TotalHours.ToString()).toDouble(2);
                        }
                        else
                        {
                            dt.Rows[i]["Diff"] = "Invalid Time";
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Patrolling Route Checklist Person wise for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F20211":
                    //NFC APP REPORTS-Patrolling Done Date wise Report
                    SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.ent_by) as ent_by,trim(a.route_name) as route_name,trim(b.location) as location,b.time as tgt_time,a.time as act_time from wb_sa_record a ,wb_sa_route b where a.branchcd='" + mbr + "' and  a.type= '12' and b.type='11' and trim(a.card_no)=trim(b.card_no) and to_char(a.vchdate,'dd/mm/yyyy')='" + value1 + "' order by a.vchdate, a.ent_by";
                    SQuery = "SELECT A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY HH24:MI') AS ENT_DT,B.LOCATION,A.CARD_NO,A.ROUTE_NAME,to_char(to_datE(sysdate||' '||B.TIME,'dd/mm/yyyy HH24:MI'),'HH24:MI') AS TGT_TIME,A.TIME AS ACT_TIME FROM WB_SA_RECORD A,WB_SA_ROUTE B WHERE TRIM(A.CARD_NO)||TRIM(A.ROUTE_NAME)=TRIM(b.CARD_NO)||TRIM(b.ROUTE_NAME) AND A.BRANCHCD='" + mbr + "' AND B.BRANCHCD='" + mbr + "' AND TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + value1 + "' ORDER BY ENT_DT,A.ROUTE_NAME,IS_NUMBER(REPLACE(B.LOCATION,'CH','')),B.TIME ";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt.Columns.Add("Diff", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["tgt_time"].ToString().Trim().Length > 1 && dt.Rows[i]["act_time"].ToString().Trim().Length > 1)
                        {
                            date1 = Convert.ToDateTime(dt.Rows[i]["tgt_time"].ToString().Trim());
                            date2 = Convert.ToDateTime(dt.Rows[i]["act_time"].ToString().Trim());
                            Diff = date1 - date2;
                            dt.Rows[i]["Diff"] = (Diff.TotalHours.ToString()).toDouble(2);
                        }
                        else
                        {
                            dt.Rows[i]["Diff"] = "Invalid Time";
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Patrolling Route Checklist Date wise for the Period " + value1, frm_qstr);
                    break;

                case "F20212":
                    fgen.drillQuery(0, "SELECT DISTINCT trim(ENT_BY)||to_char(ent_dt,'dd/mm/yyyy') AS FSTR,'-' AS GSTR,ENT_BY,TO_CHAR(ENT_DT,'DD/MM/YYYY') AS ENT_dT  FROM WB_SA_RECORD WHERE BRANCHCD='" + mbr + "' AND TYPE='12' AND VCHDATE " + xprdrange + "", frm_qstr);
                    fgen.drillQuery(1, "SELECT DISTINCT trim(ENT_BY)||to_char(ent_dt,'dd/mm/yyyy')||trim(route_name) AS FSTR,trim(ENT_BY)||to_char(ent_dt,'dd/mm/yyyy') AS GSTR,trim(route_name) as Route_name, vchnum,to_char(vchdate,'dd/MM/yyyy') as vchdate,ENT_BY,TO_CHAR(ENT_DT,'DD/MM/YYYY') AS ENT_dT  FROM WB_SA_RECORD WHERE BRANCHCD='" + mbr + "' AND TYPE='12' AND VCHDATE " + xprdrange + "", frm_qstr);
                    fgen.drillQuery(2, "select a.* from (select distinct trim(a.card_no) as fstr,trim(a.ENT_BY)||to_char(a.ent_dt,'dd/mm/yyyy')||trim(a.route_name) as gstr, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.card_no,b.location,a.time from wb_sa_record a,wb_Sa_card b where trim(a.card_no)=trim(b.card_no) ) a", frm_qstr);
                    fgen.Fn_DrillReport("Drill Down Patrolling  Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F20213":
                    SQuery = "select distinct a.branchcd,a.vchnum,A.VCHDATE,TRIM(A.route_name) AS ROUTE_NAME,max(A.tgt_time) as tgt_time,max(A.act_time) as act_time,A.ENT_BY AS USER_ID,B.LOCATION from (Select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,card_no,TRIM(route_name) AS route_name,srno,time as tgt_time,null as act_time,ENT_BY from wb_Sa_route where branchcd='" + mbr + "' and vchdate " + xprdrange + " union all Select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,card_no,TRIM(route_name) AS route_name,null as srno,null as tgt_time,time as act_time,ENT_BY from wb_sa_record where branchcd='" + mbr + "' and vchdate " + xprdrange + ") A , WB_SA_CARD B WHERE TRIM(A.CARD_NO)=TRIM(B.CARD_NO)  group by TRIM(A.route_name),A.VCHDATE,A.ENT_BY,B.LOCATION,a.vchnum,a.branchcd ORDER BY VCHNUM,VCHDATE";
                    SQuery = "SELECT A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY HH24:MI') AS ENT_DT,B.LOCATION,A.CARD_NO,A.ROUTE_NAME,to_char(to_datE(sysdate||' '||B.TIME,'dd/mm/yyyy HH24:MI'),'HH24:MI') AS TGT_TIME,A.TIME AS ACT_TIME FROM WB_SA_RECORD A,WB_SA_ROUTE B WHERE TRIM(A.CARD_NO)||TRIM(A.ROUTE_NAME)=TRIM(b.CARD_NO)||TRIM(b.ROUTE_NAME) AND A.BRANCHCD='" + mbr + "' AND B.BRANCHCD='" + mbr + "' AND A.VCHDATE " + xprdrange + " ORDER BY ENT_DT,A.ENT_BY,A.ROUTE_NAME,IS_NUMBER(REPLACE(B.LOCATION,'CH','')),B.TIME ";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt.Columns.Add("Diff", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["tgt_time"].ToString().Trim().Length > 1 && dt.Rows[i]["act_time"].ToString().Trim().Length > 1)
                        {
                            date1 = Convert.ToDateTime(dt.Rows[i]["tgt_time"].ToString().Trim());
                            date2 = Convert.ToDateTime(dt.Rows[i]["act_time"].ToString().Trim());
                            Diff = date1 - date2;
                            dt.Rows[i]["Diff"] = (Diff.TotalHours.ToString()).toDouble(2);
                        }
                        else
                        {
                            dt.Rows[i]["Diff"] = "Invalid Time";
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;                    
                    fgen.Fn_open_rptlevel("Summary Patrolling Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F20208":
                    value1 = hfcode.Value;
                    if (Request.Cookies["REPLY"].Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, co_cd, "delete from wb_sa_route where trim(branchcd)||'~'||trim(route_name)||'~'||trim(vchnum)||'~'||to_char(vchdate,'dd/mm/yyyy')='" + value1 + "' ");
                        fgen.save_info(frm_qstr, co_cd, mbr, value1.Split('~')[2].ToString(), value1.Split('~')[3].ToString(), uname, "11", "SA Route Deleted");
                        fgen.msg("-", "AMSG", "Selected Route Entry deleted.");
                    }
                    break;
                case "F20209":
                    value1 = hfcode.Value;
                    if (Request.Cookies["REPLY"].Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, co_cd, "delete from wb_sa_card where trim(branchcd)||'~'||trim(card_no)||'~'||trim(vchnum)||'~'||to_char(vchdate,'dd/mm/yyyy')||'~'||trim(location)='" + value1 + "' ");
                        fgen.save_info(frm_qstr, co_cd, mbr, value1.Split('~')[2].ToString(), value1.Split('~')[3].ToString(), uname, "11", "SA Card Deleted");
                        fgen.msg("-", "AMSG", "Selected Card Entry deleted.");
                    }
                    break;
                case "F20130":
                    SQuery = "Select a.vchnum as go_no,to_char(a.vchdate,'dd/mm/yyyy') as go_date,b.aname as party_name,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,a.ent_by as GO_by,trim(to_char(d.remvdate,'dd/mm/yyyy')||' '||d.invtime) as inv_creation_date_time,TO_CHAR(A.ent_dt,'DD/MM/YYYY HH24:MI') AS Inv_Gate_Out_Date,round(round(a.ent_dt- to_Date(trim(to_char(d.remvdate,'dd/mm/yyyy')||' '||d.invtime),'dd/mm/yyyy hh24:mi:ss'),2) * 60 * 24) as time_taken_in_min,trim(a.icode) as item_code,trim(c.iname) as item_name,A.IQTYOUT AS QTY,d.MO_VEHI as vehicle from IVOUCHERP a,famst b,item c,sale d where trim(a.stage)||trim(a.iopr)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')=d.branchcd||d.type||trim(d.vchnum)||to_Char(d.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type ='ZG' and a.vchdate " + xprdrange + " order by a.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Invoice Gateout entry for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
            }
        }
    }
}