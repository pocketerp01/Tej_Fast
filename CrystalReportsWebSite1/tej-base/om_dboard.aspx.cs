using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dboard : System.Web.UI.Page
{
    string DateRange, PrdRange, sQuery, chartScript;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    string val_legnd, popUpType = "", cond;
    string squery1, squery2, squery3, squery4, squery5, squery6;
    string stitle1, stitle2, stitle3, stitle4, stitle5, stitle6;
    string wdays, CSR;
    string Client_Code;
    string Prg_Id;

    fgenDB fgen = new fgenDB();

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
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                }
                else Response.Redirect("~/login.aspx");
            }
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (!Page.IsPostBack)
            {
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_BYPASS1") == "Y")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_BYPASS1", "N");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", PrdRange);

                    printHeads();
                    printGraph();
                }
                else
                {
                    if (Prg_Id == "******")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "PARTY");
                    }
                    else fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    askPopUp();
                }
            }
        }
    }
    void askPopUp()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        popUpType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_POPUPTYPE");
        switch (Prg_Id)
        {
            case "S15115G":
                if (popUpType == "PARTY")
                {
                    sQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
                    fgen.Fn_open_sseek("", frm_qstr);
                }
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;
            case "P17005A":
                if (popUpType == "PARTY")
                {
                    sQuery = "SELECT Vchnum,Name,Vchnum AS CODE,proj_refno as Ref FROM proj_dtl WHERE branchcd!='DD' ORDER BY Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
                    fgen.Fn_open_sseek("", frm_qstr);
                }
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;

            default:
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;
        }
    }
    void printHeads()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        switch (Prg_Id)
        {
            case "F45141":
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_lead_log a where a.branchcd!='DD' and  a.lrcdt " + PrdRange + " ", "C1");
                lblBox1Header.Text = "Total Leads Recorderd";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Lead_Act a where a.branchcd!='DD' and  a.lrcdt " + PrdRange + " ", "C1");
                lblBox2Header.Text = "Total Lead Followups";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_lead_log a where a.branchcd!='DD' and  a.lrcdt " + PrdRange + " and upper(a.CURR_STAT)='WON'", "C1");
                lblBox3Header.Text = "Total Leads Won";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_lead_log a where a.branchcd!='DD' and  a.lrcdt " + PrdRange + " and upper(a.CURR_STAT)='LOST'", "C1");
                lblBox4Header.Text = "Total Leads Lost";
                break;


            case "F60141":
            case "F60146":
                Client_Code = "%";
                if (frm_ulvl == "0") Client_Code = "";
                if (frm_ulvl != "0") Client_Code = " and a.ent_by='" + frm_uname + "'";
                if (frm_ulvl == "3") Client_Code = " and trim(a.ccode)='" + frm_uname + "'";
                if (CSR.Length > 1) Client_Code = " and trim(a.ccode)='" + CSR + "'";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_log a where a.branchcd!='DD' and  a.cssdt " + PrdRange + " " + Client_Code + " and cssdt>=to_Date('01/10/2017','dd/mm/yyyy') ", "C1");
                lblBox1Header.Text = "Total CSS Requests";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_log a where a.branchcd!='DD' and  a.cssdt " + PrdRange + " " + Client_Code + " and cssdt>=to_Date('01/10/2017','dd/mm/yyyy') AND UPPER(DIR_COMP)='Y' ", "C1");
                lblBox2Header.Text = "Total CSS Cleared";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_asg a where a.branchcd!='DD' and  a.dsrdt " + PrdRange + " " + Client_Code + " and cssdt>=to_Date('01/10/2017','dd/mm/yyyy') ", "C1");
                lblBox3Header.Text = "Total CSS Assigned";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_asg a where a.branchcd!='DD' and  a.dsrdt " + PrdRange + " and trim(nvl(task_compl,'-'))!='Y' " + Client_Code + " and cssdt>=to_Date('01/10/2017','dd/mm/yyyy') ", "C1");
                lblBox4Header.Text = "Total Assg Pending Action";

                //lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_asg a where a.branchcd!='DD' and  a.dsrdt " + PrdRange + " and trim(nvl(app_by,'-'))='-' and trim(nvl(task_compl,'-'))='Y' " + Client_Code + " and cssdt>=to_Date('01/10/2017','dd/mm/yyyy') ", "C1");
                //lblBox4Header.Text = "Total Assg Pending Clearance";
                break;
            case "F60151":
                Client_Code = " ";
                cond = "";
                cond = " and trim(a.ent_by)='" + frm_uname + "'";
                if (CSR.Length > 1) { Client_Code = " and a.ccode='" + frm_uname + "'"; cond = ""; }
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_act a where a.branchcd!='DD' and  a.actdt " + PrdRange + " " + cond + " " + Client_Code + " ", "C1");
                lblBox1Header.Text = "Total Task Assigned";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_act a where a.branchcd!='DD' and  actdt " + PrdRange + " and trim(nvl(a.task_compl,'-'))='Y' " + cond + " " + Client_Code + " ", "C1");
                lblBox2Header.Text = "Total Task Completed";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_act a where a.branchcd!='DD' and  actdt " + PrdRange + " and trim(nvl(a.task_compl,'-'))='Y' and trim(nvl(app_by,'-'))!='-' " + cond + " " + Client_Code + " ", "C1");
                lblBox3Header.Text = "Total Task Approved";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_Css_act a where a.branchcd!='DD' and  actdt " + PrdRange + " and trim(nvl(a.task_compl,'-'))!='Y' " + cond + " " + Client_Code + " ", "C1");
                lblBox4Header.Text = "Total Task Pend Compl.";
                break;

            case "F61141":
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_ccm_log a where a.branchcd!='DD' and  a.ccmdt " + PrdRange + " ", "C1");
                lblBox1Header.Text = "Total CCM Recorderd";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_CCM_Act a where a.branchcd!='DD' and  a.CCMdt " + PrdRange + " ", "C1");
                lblBox2Header.Text = "Total CCM Followups";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_CCM_log a where a.branchcd!='DD' and  a.CCMdt " + PrdRange + " and substr(upper(a.CURR_STAT),1,5)='CLEAR'", "C1");
                lblBox3Header.Text = "Total CCM Cleared";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_CCM_log a where a.branchcd!='DD' and  a.CCMdt " + PrdRange + " and substr(upper(a.CURR_STAT),1,4)='CAPA'", "C1");
                lblBox4Header.Text = "Total CCM Capa Done";
                break;
            case "F94132":
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_STl_log a where a.branchcd!='DD'  ", "C1");
                lblBox1Header.Text = "Total STL Registered";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select count(*) as c1 from (Select  distinct Evertical from wb_STL_log a where a.branchcd!='DD') ", "C1");
                lblBox2Header.Text = "Total Verticals STL";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select count(*) as c1 from (Select  distinct ccode from wb_STL_log a where a.branchcd!='DD')", "C1");
                lblBox3Header.Text = "Total Clients STL";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_STL_log a where a.branchcd!='DD' and trim(nvl(app_by,'-'))='-' ", "C1");
                lblBox4Header.Text = "STL Pending Approval";
                break;
            case "F93132":
            case "F93133":
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_oms_log a where a.branchcd!='DD' and a.opldt " + PrdRange + " ", "C1");
                lblBox1Header.Text = "Clients Planned";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (Select  distinct ccode from wb_oms_act a where a.branchcd!='DD' and a.oacdt " + PrdRange + ") ", "C1");
                lblBox2Header.Text = "Clients Followed ";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (Select  distinct ccode from wb_oms_act a where a.agree_Amt>0 and a.branchcd!='DD' and a.oacdt " + PrdRange + ") ", "C1");
                lblBox3Header.Text = "Clients Agreed";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (Select  distinct ccode from wb_oms_act a where a.agree_Amt>0 and a.branchcd!='DD' and upper(Trim(a.act_mode))='VISIT' and a.oacdt " + PrdRange + ") ", "C1");
                lblBox4Header.Text = "Clients Visited";
                break;

            case "F96132":
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_DSL_log a where a.branchcd!='DD'  ", "C1");
                lblBox1Header.Text = "Total DSL Registered";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select count(*) as c1 from (Select  distinct Evertical from wb_DSL_log a where a.branchcd!='DD') ", "C1");
                lblBox2Header.Text = "Total Verticals DSL";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select count(*) as c1 from (Select  distinct ccode from wb_DSL_log a where a.branchcd!='DD')", "C1");
                lblBox3Header.Text = "Total Clients DSL";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_DSL_log a where a.branchcd!='DD' and trim(nvl(app_by,'-'))='-' ", "C1");
                lblBox4Header.Text = "DSL Pending Approval";
                break;
            case "F97132":
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_CAM_log a where a.branchcd!='DD'  ", "C1");
                lblBox1Header.Text = "Total CAM Logd";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_CAM_log a where a.branchcd!='DD' group by cam_type", "C1");
                lblBox2Header.Text = "Total CAM Types";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (Select  distinct tcode as c1 from wb_CAM_log a where a.branchcd!='DD') ", "C1");
                lblBox3Header.Text = "Total CAM Persons";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  count(*) as c1 from wb_CAM_log a where a.branchcd!='DD' and trim(nvl(app_by,'-'))='-' ", "C1");
                lblBox4Header.Text = "CAM Pending Approval";
                break;

            case "P17005A":
                lblHeader.Visible = true;
                lblHeader.Text = "Project Code: " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(utime) as c1 from proj_updt a where a.branchcd!='DD' and projcode ='" + Client_Code + "' and a.vchdate " + PrdRange + " ", "C1");
                lblBox1Header.Text = "Work Hours Reported";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(dt_hrs) as c1 from proj_dtime a where a.branchcd!='DD' and projcode ='" + Client_Code + "' and a.vchdate " + PrdRange + " ", "C1");
                lblBox2Header.Text = "DownTime Hours Reported";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(cad_submit+Drg_submit) as c1 from proj_updt a where a.branchcd!='DD' and projcode ='" + Client_Code + "' and a.vchdate " + PrdRange + " ", "C1");
                lblBox3Header.Text = "No. of CAD,Drg Sumitted";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(c1) as c1 from (Select  distinct tavchnum as c1 from proj_updt a where a.branchcd!='DD' and projcode ='" + Client_Code + "' and a.vchdate " + PrdRange + ")", "C1");
                lblBox4Header.Text = "No. of Projects Worked on";
                break;

            case "P17005C":

                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(utime) as c1 from proj_updt a where a.branchcd!='DD' and projcode like '" + Client_Code + "' and a.vchdate " + PrdRange + " ", "C1");
                lblBox1Header.Text = "Work Hours Reported";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(dt_hrs) as c1 from proj_dtime a where a.branchcd!='DD' and projcode like '" + Client_Code + "' and a.vchdate " + PrdRange + " ", "C1");
                lblBox2Header.Text = "DownTime Hours Reported";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(cad_submit+Drg_submit) as c1 from proj_updt a where a.branchcd!='DD' and projcode like '" + Client_Code + "' and a.vchdate " + PrdRange + " ", "C1");
                lblBox3Header.Text = "No. of CAD,Drg Sumitted";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(c1) as c1 from (Select  distinct tavchnum as c1 from proj_updt a where a.branchcd!='DD' and projcode like '" + Client_Code + "' and a.vchdate " + PrdRange + ")", "C1");
                lblBox4Header.Text = "No. of Projects Worked on";
                break;

            case "S15115G":
                lblHeader.Visible = true;
                lblHeader.Text = "Client Code: " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                wdays = fgen.seek_iname(frm_qstr, frm_cocd, "select to_date(to_chaR(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date('01'||to_char(sysdate,'/mm/yyyy'),'dd/mm/yyyy') as fstr from dual ", "fstr");

                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select sum(a.mtime*a.mtime1*(" + wdays + "+1)) as c1 from itwstage a where a.branchcd!='DD' and a.type='WL' and icode ='" + Client_Code + "' ", "C1");
                lblBox1Header.Text = "Budgeted Hours This Month";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT sum(whours) as c1 FROM cwork_Rec where branchcd!='DD' and type='ED' and icode = '" + Client_Code + "' and to_char(vchdate,'yyyymm')=to_char(sysdate,'yyyymm')", "C1");
                lblBox2Header.Text = "Efforts Hours For the Month";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (SELECT distinct acode FROM cwork_Rec where branchcd!='DD' and type='ED' and icode = '" + Client_Code + "' and to_char(vchdate,'yyyymm')=to_char(sysdate,'yyyymm'))", "C1");
                lblBox3Header.Text = "No. of Team Members Involved";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (SELECT distinct Wcode FROM cwork_Rec where branchcd!='DD' and type='ED' and icode = '" + Client_Code + "' and to_char(vchdate,'yyyymm')=to_char(sysdate,'yyyymm'))", "C1");
                lblBox4Header.Text = "No. of Acitvities Done";
                break;
            case "S15115H":

                wdays = fgen.seek_iname(frm_qstr, frm_cocd, "select to_date(to_chaR(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date('01'||to_char(sysdate,'/mm/yyyy'),'dd/mm/yyyy') as fstr from dual ", "fstr");
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select sum(a.mtime*a.mtime1*(" + wdays + "+1)) as c1 from itwstage a where a.branchcd!='DD' and a.type='WL' and icode like '" + Client_Code + "' ", "C1");
                lblBox1Header.Text = "Budgeted Hours This Month";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT sum(whours) as c1 FROM cwork_Rec where branchcd!='DD' and type='ED' and icode like '" + Client_Code + "' and to_char(vchdate,'yyyymm')=to_char(sysdate,'yyyymm')", "C1");
                lblBox2Header.Text = "Efforts Hours For the Month";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (SELECT distinct acode FROM cwork_Rec where branchcd!='DD' and type='ED' and icode like  '" + Client_Code + "' and to_char(vchdate,'yyyymm')=to_char(sysdate,'yyyymm'))", "C1");
                lblBox3Header.Text = "No. of Team Members Involved";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as c1 from (SELECT distinct Wcode FROM cwork_Rec where branchcd!='DD' and type='ED' and icode like '" + Client_Code + "' and to_char(vchdate,'yyyymm')=to_char(sysdate,'yyyymm'))", "C1");
                lblBox4Header.Text = "No. of Acitvities Done";
                break;
            case "S05005D":
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and ent_by='" + frm_uname + "'", "C1");
                lblBox1Header.Text = "Total Query Raised";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and ent_by='" + frm_uname + "' and last_action!='-'", "C1");
                lblBox2Header.Text = "Total Action Taken";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and ent_by='" + frm_uname + "' and last_action like 'Cleared%'", "C1");
                lblBox3Header.Text = "Total Query Cleared";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and ent_by='" + frm_uname + "' and trim(nvl(clo_by,'-'))!='-' ", "C1");
                lblBox4Header.Text = "Total Query Closed";
                break;

            case "S06005F":
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' ", "C1");
                lblBox1Header.Text = "Total Query Received";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and last_action!='-'", "C1");
                lblBox2Header.Text = "Total Action Taken";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and last_action like 'Cleared%'", "C1");
                lblBox3Header.Text = "Total Query Cleared";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and trim(nvl(clo_by,'-'))!='-' ", "C1");
                lblBox4Header.Text = "Total Query Closed";
                break;
        }


    }
    void printGraph()
    {
        val_legnd = "Value in Lacs";
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        switch (Prg_Id)
        {
            case "F45141":
                stitle1 = "Monthly Lead Registered";
                squery1 = "SELECT TO_cHAR(a.LRCdt,'Mon-YY') AS YR,count(a.LRCNO) AS Lead_Count FROM wb_lead_log A WHERE a.BRANCHCD!='DD' and a.lrcdt " + PrdRange + "  GROUP BY TO_cHAR(lrcdt,'Mon-YY'),TO_cHAR(a.lrcdt,'YYYYMM') order by TO_cHAR(a.lrcdt,'YYYYMM')";
                stitle2 = "Monthly Lead Followed";
                squery2 = "SELECT TO_cHAR(a.LaCdt,'Mon-YY') AS YR,count(a.LaCNO) AS Lead_Count FROM wb_lead_act A WHERE a.BRANCHCD!='DD' and a.lacdt " + PrdRange + "  GROUP BY TO_cHAR(lacdt,'Mon-YY'),TO_cHAR(a.lacdt,'YYYYMM') order by TO_cHAR(a.lacdt,'YYYYMM')";
                stitle3 = "Monthly Leads Won";
                squery3 = "SELECT TO_cHAR(a.LRCdt,'Mon-YY') AS YR,count(a.LRCNO) AS Lead_Count FROM wb_lead_log A WHERE a.BRANCHCD!='DD' and a.lrcdt " + PrdRange + "  and upper(trim(a.CURR_STAT))='WON' GROUP BY TO_cHAR(lrcdt,'Mon-YY'),TO_cHAR(a.lrcdt,'YYYYMM')  order by TO_cHAR(a.lrcdt,'YYYYMM')";
                stitle4 = "Monthly Leads Lost.";
                squery4 = "SELECT TO_cHAR(a.LRCdt,'Mon-YY') AS YR,count(a.LRCNO) AS Lead_Count FROM wb_lead_log A WHERE a.BRANCHCD!='DD' and a.lrcdt " + PrdRange + "  and upper(trim(a.CURR_STAT))='LOST' GROUP BY TO_cHAR(lrcdt,'Mon-YY'),TO_cHAR(a.lrcdt,'YYYYMM') order by TO_cHAR(a.lrcdt,'YYYYMM')";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;

            case "F60141*":
                if (frm_ulvl == "0") Client_Code = "";
                if (frm_ulvl != "0") Client_Code = " and a.ent_by='" + frm_uname + "'";
                if (frm_ulvl == "3") Client_Code = " and trim(a.ccode)='" + frm_uname + "'";
                if (CSR.Length > 1) Client_Code = " and trim(a.ccode)='" + CSR + "'";
                stitle1 = "Monthly CSS Requests";
                squery1 = "SELECT TO_cHAR(a.cssdt,'Mon-YY') AS YR,count(a.cssno) AS CSS_Count FROM wb_Css_log A WHERE a.BRANCHCD!='DD' and a.cssdt " + PrdRange + " " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') GROUP BY TO_cHAR(cssdt,'Mon-YY'),TO_cHAR(a.cssdt,'YYYYMM') order by TO_cHAR(a.cssdt,'YYYYMM')";
                stitle2 = "Monthly CSS Cleared";
                squery2 = "SELECT TO_cHAR(a.cssdt,'Mon-YY') AS YR,count(a.cssno) AS CSS_Count FROM wb_Css_log A WHERE a.BRANCHCD!='DD' and a.cssdt " + PrdRange + " " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') and nvl(app_by,'-')!='-' GROUP BY TO_cHAR(cssdt,'Mon-YY'),TO_cHAR(a.cssdt,'YYYYMM') order by TO_cHAR(a.cssdt,'YYYYMM')";
                stitle3 = "Monthly CSS Pending Conf.";
                squery3 = "SELECT TO_cHAR(a.cssdt,'Mon-YY') AS YR,count(a.cssno) AS CSS_Count FROM wb_Css_log A WHERE a.BRANCHCD!='DD' and a.cssdt " + PrdRange + " " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') and nvl(app_by,'-')='-' and nvl(fapp_by,'-')!='-' GROUP BY TO_cHAR(cssdt,'Mon-YY'),TO_cHAR(a.cssdt,'YYYYMM') order by TO_cHAR(a.cssdt,'YYYYMM')";
                stitle4 = "Monthly CSS Pending Act.";
                squery4 = "SELECT TO_cHAR(a.cssdt,'Mon-YY') AS YR,count(a.cssno) AS CSS_Count FROM wb_Css_log A WHERE a.BRANCHCD!='DD' and a.cssdt " + PrdRange + " " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') and nvl(fapp_by,'-')='-' GROUP BY TO_cHAR(cssdt,'Mon-YY'),TO_cHAR(a.cssdt,'YYYYMM') order by TO_cHAR(a.cssdt,'YYYYMM')";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "F60146":
            case "F60141":
                Client_Code = "%";
                if (frm_ulvl == "0") Client_Code = "";
                if (frm_ulvl != "0") Client_Code = " and a.ent_by='" + frm_uname + "'";
                if (frm_ulvl == "3") Client_Code = " and trim(a.ccode)='" + frm_uname + "'";
                if (CSR.Length > 1) Client_Code = " and trim(a.ccode)='" + CSR + "'";
                stitle1 = "Monthly CSS Requests";
                squery1 = "SELECT TO_cHAR(a.cssdt,'Mon-YY') AS YR,count(a.cssno) AS CSS_Count FROM wb_Css_log A WHERE a.BRANCHCD!='DD' and a.cssdt " + PrdRange + "  " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') GROUP BY TO_cHAR(cssdt,'Mon-YY'),TO_cHAR(a.cssdt,'YYYYMM') order by TO_cHAR(a.cssdt,'YYYYMM')";
                stitle2 = "Monthly CSS Assigned";
                squery2 = "SELECT TO_cHAR(a.dsrdt,'Mon-YY') AS YR,count(a.dsrno) AS CSS_Asgn FROM wb_Css_asg A WHERE a.BRANCHCD!='DD' and a.dsrdt " + PrdRange + " " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') GROUP BY TO_cHAR(dsrdt,'Mon-YY'),TO_cHAR(a.dsrdt,'YYYYMM') order by TO_cHAR(a.dsrdt,'YYYYMM')";
                stitle3 = "Monthly Assg Pending Action";
                squery3 = "SELECT TO_cHAR(a.dsrdt,'Mon-YY') AS YR,count(a.dsrno) AS CSS_Asgn FROM wb_Css_asg A WHERE a.BRANCHCD!='DD' and a.dsrdt " + PrdRange + " " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') and trim(nvl(task_compl,'-'))!='Y' GROUP BY TO_cHAR(dsrdt,'Mon-YY'),TO_cHAR(a.dsrdt,'YYYYMM') order by TO_cHAR(a.dsrdt,'YYYYMM')";
                stitle4 = "Monthly CSS Pending Act.";
                squery4 = "SELECT TO_cHAR(a.dsrdt,'Mon-YY') AS YR,count(a.dsrno) AS CSS_Asgn FROM wb_Css_asg A WHERE a.BRANCHCD!='DD' and a.dsrdt " + PrdRange + " " + Client_Code + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') and trim(nvl(app_by,'-'))='-' and trim(nvl(task_compl,'-'))='Y' GROUP BY TO_cHAR(dsrdt,'Mon-YY'),TO_cHAR(a.dsrdt,'YYYYMM') order by TO_cHAR(a.dsrdt,'YYYYMM')";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "F60151":
                cond = "";
                cond = " and trim(a.ent_by)='" + frm_uname + "'";
                stitle1 = "Monthly Task Assigned";
                squery1 = "SELECT TO_cHAR(a.dsrdt,'Mon-YY') AS YR,count(a.dsrno) AS CSS_Count FROM wb_Css_asg A WHERE a.BRANCHCD!='DD' and a.dsrdt " + PrdRange + "  " + cond + " GROUP BY TO_cHAR(dsrdt,'Mon-YY'),TO_cHAR(a.dsrdt,'YYYYMM') order by TO_cHAR(a.dsrdt,'YYYYMM')";
                
                stitle2 = "Monthly Task Completed";
                squery2 = "SELECT TO_cHAR(a.actdt,'Mon-YY') AS YR,count(a.actno) AS CSS_Asgn FROM wb_Css_act A WHERE a.BRANCHCD!='DD' and a.actdt " + PrdRange + " and trim(nvl(task_compl,'-'))='Y' " + cond + " GROUP BY TO_cHAR(actdt,'Mon-YY'),TO_cHAR(a.actdt,'YYYYMM') order by TO_cHAR(a.actdt,'YYYYMM')";

                stitle3 = "Monthly Task Approved";
                squery3 = "SELECT TO_cHAR(a.actdt,'Mon-YY') AS YR,count(a.actno) AS CSS_Asgn FROM wb_Css_act A WHERE a.BRANCHCD!='DD' and a.actdt " + PrdRange + " and trim(nvl(task_compl,'-'))='Y' and trim(nvl(app_by,'-'))!='-' " + cond + " GROUP BY TO_cHAR(actdt,'Mon-YY'),TO_cHAR(a.actdt,'YYYYMM') order by TO_cHAR(a.actdt,'YYYYMM')";

                stitle4 = "Monthly Task Pend Compl";
                squery4 = "SELECT TO_cHAR(a.actdt,'Mon-YY') AS YR,count(a.actno) AS CSS_Asgn FROM wb_Css_act A WHERE a.BRANCHCD!='DD' and a.actdt " + PrdRange + " and trim(nvl(task_compl,'-'))!='Y' " + cond + " GROUP BY TO_cHAR(actdt,'Mon-YY'),TO_cHAR(a.actdt,'YYYYMM') order by TO_cHAR(a.actdt,'YYYYMM')";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "F61141":
                stitle1 = "Monthly CCM Registered";
                squery1 = "SELECT TO_cHAR(a.CCMdt,'Mon-YY') AS YR,count(a.CCMNO) AS CCM_Count FROM wb_CCM_log A WHERE a.BRANCHCD!='DD' and a.CCMdt " + PrdRange + "  GROUP BY TO_cHAR(CCMdt,'Mon-YY'),TO_cHAR(a.CCMdt,'YYYYMM') order by TO_cHAR(a.CCMdt,'YYYYMM')";
                stitle2 = "Monthly CCM Action Taken";
                squery2 = "SELECT TO_cHAR(a.CACdt,'Mon-YY') AS YR,count(a.CACNO) AS CCM_Count FROM wb_CCM_act A WHERE a.BRANCHCD!='DD' and a.Cacdt " + PrdRange + "  GROUP BY TO_cHAR(cacdt,'Mon-YY'),TO_cHAR(a.cacdt,'YYYYMM') order by TO_cHAR(a.cacdt,'YYYYMM')";
                stitle3 = "Monthly CCM Cleared";
                squery3 = "SELECT TO_cHAR(a.CCMdt,'Mon-YY') AS YR,count(a.CCMNO) AS CCM_Count FROM wb_CCM_log A WHERE a.BRANCHCD!='DD' and a.CCMdt " + PrdRange + "  and substr(upper(trim(a.CURR_STAT)),1,5)='CLEAR' GROUP BY TO_cHAR(CCMdt,'Mon-YY'),TO_cHAR(a.CCMdt,'YYYYMM') order by TO_cHAR(a.CCMdt,'YYYYMM')";
                stitle4 = "Monthly CCM Capa Done";
                squery4 = "SELECT TO_cHAR(a.CCMdt,'Mon-YY') AS YR,count(a.CCMNO) AS CCM_Count FROM wb_CCM_log A WHERE a.BRANCHCD!='DD' and a.CCMdt " + PrdRange + "  and substr(upper(trim(a.CURR_STAT)),1,4)='CAPA' GROUP BY TO_cHAR(CCMdt,'Mon-YY'),TO_cHAR(a.CCMdt,'YYYYMM') order by TO_cHAR(a.CCMdt,'YYYYMM')";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "F93132":
            case "F93133":
                stitle1 = "Monthly Target Planned";
                squery1 = "SELECT TO_cHAR(a.opldt,'Mon-YY') AS YR,sum(a.month_Amt) AS OPL_Count FROM wb_OMS_log A WHERE a.BRANCHCD!='DD' and a.OPLdt " + DateRange + "   GROUP BY TO_cHAR(OPLdt,'Mon-YY'),TO_cHAR(a.OPLdt,'YYYYMM') order by TO_cHAR(a.OPLdt,'YYYYMM')";

                stitle2 = "Monthly Clients Agreeed";
                squery2 = "SELECT TO_cHAR(oacdt,'Mon-YY') AS YR,sum(agree_Amt) AS OAC_Count FROM (select oacdt,ccode,agree_Amt from wb_OMS_act WHERE BRANCHCD!='DD' and OaCdt " + DateRange + ")   GROUP BY TO_cHAR(oacdt,'Mon-YY'),TO_cHAR(oacdt,'YYYYMM') order by TO_cHAR(oacdt,'YYYYMM')";

                stitle3 = "Client wise Followup";
                squery3 = "SELECT username,count(ccode) AS OAC_Count FROM (select distinct a.oacdt,a.ccode,b.username from wb_OMS_act a,evas b WHERE trim(A.ccode)=trim(B.userid) and a.BRANCHCD!='DD' and a.OaCdt " + PrdRange + ")   GROUP BY username";

                stitle4 = "Day wise Followup";
                squery4 = "SELECT TO_cHAR(oacdt,'DD') AS YR,count(ccode) AS OAC_Count FROM (select distinct oacdt,ccode from wb_OMS_act WHERE BRANCHCD!='DD' and OaCdt " + PrdRange + ")   GROUP BY TO_cHAR(oacdt,'DD'),TO_cHAR(oacdt,'YYYYMM') order by TO_cHAR(oacdt,'DD')";


                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;

            case "F94132":
                stitle1 = "Monthly STL Registered";
                squery1 = "SELECT TO_cHAR(a.STLdt,'Mon-YY') AS YR,count(a.STLno) AS STL_Count FROM wb_STL_log A WHERE a.BRANCHCD!='DD' GROUP BY TO_cHAR(stldt,'Mon-YY'),TO_cHAR(a.STldt,'YYYYMM') order by TO_cHAR(a.STldt,'YYYYMM')";
                stitle2 = "Monthly STl Approved";
                squery2 = "SELECT TO_cHAR(a.STldt,'Mon-YY') AS YR,count(a.STlno) AS STl_Count FROM wb_STl_log A WHERE a.BRANCHCD!='DD' and trim(nvl(app_by,'-'))!='-' GROUP BY TO_cHAR(STldt,'Mon-YY'),TO_cHAR(a.STldt,'YYYYMM') order by TO_cHAR(a.STldt,'YYYYMM')";
                stitle3 = "Client wise STL";
                squery3 = "SELECT trim(a.CCODE) AS YR,count(a.STlno) AS STl_Count FROM wb_STl_log A WHERE a.BRANCHCD!='DD' GROUP BY trim(a.CCODE)  order by trim(a.CCODE)";
                stitle4 = "Biz Vertical Wise STL";
                squery4 = "SELECT trim(a.Evertical) AS YR,count(a.STlno) AS STl_Count FROM wb_STl_log A WHERE a.BRANCHCD!='DD' GROUP BY trim(a.Evertical)  order by trim(a.Evertical)";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;

            case "F96132":
                stitle1 = "Monthly DSL Registered";
                squery1 = "SELECT TO_cHAR(a.dsldt,'Mon-YY') AS YR,count(a.dslno) AS DSL_Count FROM wb_DSL_log A WHERE a.BRANCHCD!='DD' GROUP BY TO_cHAR(dsldt,'Mon-YY'),TO_cHAR(a.dsldt,'YYYYMM') order by TO_cHAR(a.dsldt,'YYYYMM')";
                stitle2 = "Monthly DSL Approved";
                squery2 = "SELECT TO_cHAR(a.dsldt,'Mon-YY') AS YR,count(a.dslno) AS DSL_Count FROM wb_DSL_log A WHERE a.BRANCHCD!='DD' and trim(nvl(app_by,'-'))!='-' GROUP BY TO_cHAR(dsldt,'Mon-YY'),TO_cHAR(a.dsldt,'YYYYMM') order by TO_cHAR(a.dsldt,'YYYYMM')";
                stitle3 = "Client wise DSL";
                squery3 = "SELECT trim(a.CCODE) AS YR,count(a.dslno) AS DSL_Count FROM wb_DSL_log A WHERE a.BRANCHCD!='DD' GROUP BY trim(a.CCODE)  order by trim(a.CCODE)";
                stitle4 = "Biz Vertical Wise DSL";
                squery4 = "SELECT trim(a.Evertical) AS YR,count(a.dslno) AS DSL_Count FROM wb_DSL_log A WHERE a.BRANCHCD!='DD' GROUP BY trim(a.Evertical)  order by trim(a.Evertical)";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "F97132":
                stitle1 = "Monthly CAM Registered";
                squery1 = "SELECT TO_cHAR(a.camdt,'Mon-YY') AS YR,count(a.camno) AS cam_Count FROM wb_cam_log A WHERE a.BRANCHCD!='DD' GROUP BY TO_cHAR(camdt,'Mon-YY'),TO_cHAR(a.camdt,'YYYYMM') order by TO_cHAR(a.camdt,'YYYYMM')";
                stitle2 = "Monthly CAM Approved";
                squery2 = "SELECT TO_cHAR(a.camdt,'Mon-YY') AS YR,count(a.camno) AS cam_Count FROM wb_cam_log A WHERE a.BRANCHCD!='DD' and trim(nvl(a.app_by,'-'))!='-' GROUP BY TO_cHAR(camdt,'Mon-YY'),TO_cHAR(a.camdt,'YYYYMM') order by TO_cHAR(a.camdt,'YYYYMM')";
                stitle3 = "Team wise CAM";
                squery3 = "SELECT trim(a.TCODE) AS YR,count(a.camno) AS cam_Count FROM wb_cam_log A WHERE a.BRANCHCD!='DD' GROUP BY trim(a.tCODE)  order by trim(a.tCODE)";
                stitle4 = "Type Wise CAM";
                squery4 = "SELECT trim(a.cam_type) AS YR,count(a.camno) AS DSL_Count FROM wb_cam_log A WHERE a.BRANCHCD!='DD' GROUP BY trim(a.cam_type)  order by trim(a.cam_type)";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;

            case "P17005A":
                stitle1 = "Monthly Effort Costs";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.utime*a.uhrcost) AS QTYOUT FROM proj_updt A WHERE a.BRANCHCD!='DD' and a.vchdate " + PrdRange + " and a.projcode='" + Client_Code + "' GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle2 = "Monthly Effort Hrs";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.utime) AS QTYOUT FROM proj_updt A WHERE a.BRANCHCD!='DD' and a.vchdate " + PrdRange + " and a.projcode='" + Client_Code + "' GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle3 = "Down Time Hrs";
                squery3 = "SELECT b.Name AS YR,sum(a.dt_hrs) AS QTYOUT FROM proj_dtime A,proj_mast b  WHERE a.BRANCHCD!='DD' and b.type='P4' and trim(a.dtcode)=trim(b.acode) and a.vchdate " + PrdRange + " and a.projcode='" + Client_Code + "' GROUP BY b.Name";
                stitle4 = "Employee Wise Hrs";
                squery4 = "SELECT b.Name AS YR,sum(a.utime) AS QTYOUT FROM proj_updt A,proj_mast b  WHERE a.BRANCHCD!='DD' and b.type='P7' and trim(a.asgecode)=trim(b.acode) and a.vchdate " + PrdRange + " and a.projcode='" + Client_Code + "' GROUP BY b.Name";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "P17005C":
                stitle1 = "Monthly Effort Costs";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.utime*a.uhrcost) AS QTYOUT FROM proj_updt A WHERE a.BRANCHCD!='DD' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle2 = "Monthly Effort Hrs";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.utime) AS QTYOUT FROM proj_updt A WHERE a.BRANCHCD!='DD' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle3 = "Project Wise Hrs";
                squery3 = "SELECT b.Name AS YR,sum(a.utime) AS QTYOUT FROM proj_updt A,proj_dtl b  WHERE a.BRANCHCD!='DD' and b.type='P0' and trim(a.projcode)=trim(b.vchnum) and a.vchdate " + PrdRange + " GROUP BY b.Name";
                stitle4 = "Employee Wise Hrs";
                squery4 = "SELECT b.Name AS YR,sum(a.utime) AS QTYOUT FROM proj_updt A,proj_mast b  WHERE a.BRANCHCD!='DD' and b.type='P7' and trim(a.asgecode)=trim(b.acode) and a.vchdate " + PrdRange + " GROUP BY b.Name";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;

            case "S15115G":

                stitle1 = "Costs By Month";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.whours*a.tmcost) AS QTYOUT FROM cwork_Rec A WHERE a.BRANCHCD!='DD' AND a.icode = '" + Client_Code + "' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle2 = "Hrs By Effort";
                squery2 = "SELECT b.Name AS YR,sum(a.whours) AS QTYOUT FROM cwork_Rec A,type b  WHERE b.id='#' and b.id2='TS' and trim(a.wcode)=b.type1 and a.BRANCHCD!='DD' AND a.icode = '" + Client_Code + "'  and a.vchdate " + PrdRange + " GROUP BY b.Name";
                stitle3 = "Hrs By Employee";
                squery3 = "SELECT b.Name AS YR,sum(a.whours) AS QTYOUT FROM cwork_Rec A,type b  WHERE b.id='#' and b.id2='TM' and trim(a.acode)=b.type1 and a.BRANCHCD!='DD' AND a.icode = '" + Client_Code + "'  and a.vchdate " + PrdRange + " GROUP BY b.Name";
                stitle4 = "Hrs Per Month";
                squery4 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.whours) AS QTYOUT FROM cwork_Rec A WHERE a.BRANCHCD!='DD' AND a.icode = '" + Client_Code + "'  and a.vchdate " + PrdRange + " GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";

                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "S15115H":
                Client_Code = "%";
                stitle1 = "Costs By Month";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.whours*a.tmcost) AS QTYOUT FROM cwork_Rec A WHERE a.BRANCHCD!='DD' AND a.icode like '" + Client_Code + "'  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle2 = "Hrs By Effort";
                squery2 = "SELECT b.Name AS YR,sum(a.whours) AS QTYOUT FROM cwork_Rec A,type b  WHERE b.id='#' and b.id2='TS' and trim(a.wcode)=b.type1 and a.BRANCHCD!='DD' AND a.icode like '" + Client_Code + "'  GROUP BY b.Name";
                stitle3 = "Hrs By Employee";
                squery3 = "SELECT b.Name AS YR,sum(a.whours) AS QTYOUT FROM cwork_Rec A,type b  WHERE b.id='#' and b.id2='TM' and trim(a.acode)=b.type1 and a.BRANCHCD!='DD' AND a.icode like '" + Client_Code + "'  GROUP BY b.Name";
                stitle4 = "Hours Per Month";
                squery4 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,sum(a.whours) AS QTYOUT FROM cwork_Rec A WHERE a.BRANCHCD!='DD' AND a.icode like '" + Client_Code + "'  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";

                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;


            case "S05005D":
                stitle1 = "By Month";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD' AND a.ent_by='" + frm_uname + "'  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle2 = "By Nature";
                squery2 = "SELECT trim(upper(qrytype)) AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD' AND a.ent_by='" + frm_uname + "'  GROUP BY trim(upper(qrytype)) order by trim(upper(qrytype))";
                stitle3 = "By Occurance";
                squery3 = "SELECT trim(upper(qryocc)) AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD' AND a.ent_by='" + frm_uname + "'  GROUP BY trim(upper(qryocc)) order by trim(upper(qryocc))";
                stitle4 = "By Closure";
                squery4 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD' AND a.ent_by='" + frm_uname + "' and trim(nvl(a.clo_by,'-'))!='-'  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";

                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;

            case "S06005F":
                stitle1 = "By Month";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD'  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle2 = "By Nature";
                squery2 = "SELECT trim(upper(qrytype)) AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD'  GROUP BY trim(upper(qrytype)) order by trim(upper(qrytype))";
                stitle3 = "By Occurance";
                squery3 = "SELECT trim(upper(qryocc)) AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD'  GROUP BY trim(upper(qryocc)) order by trim(upper(qryocc))";
                stitle4 = "By Closure";
                squery4 = "SELECT TO_cHAR(a.VCHDATE,'Mon-YY') AS YR,count(a.vchnum) AS QTYOUT FROM cquery_reg A WHERE a.BRANCHCD!='DD'  and trim(nvl(a.clo_by,'-'))!='-'  GROUP BY TO_cHAR(VCHDATE,'Mon-YY') order by TO_cHAR(VCHDATE,'Mon-YY')";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
        }

        lblChart1Header.Text = stitle1;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle1, "column", stitle1, val_legnd, squery1, val_legnd, "chart1", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart1", chartScript, false);

        lblChart2Header.Text = stitle2;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle2, "bar", stitle2, val_legnd, squery2, val_legnd, "chart2", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart2", chartScript, false);

        lblChart3Header.Text = stitle3;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle3, "pie", stitle3, val_legnd, squery3, val_legnd, "chart3", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart3", chartScript, false);

        lblChart4Header.Text = stitle4;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle4, "line", stitle4, val_legnd, squery4, val_legnd, "chart4", "", "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart4", chartScript, false);

        //lblChart5Header.Text = stitle5;
        //sQuery = "SELECT TO_cHAR(VCHDATE,'MM/YY') AS YR,ROUND(SUM(iqtyin/100),2) AS QTYOUT FROM IVOUCHER A WHERE BRANCHCD='00' AND TYPE LIKE '1%' GROUP BY TO_cHAR(VCHDATE,'MM/YY') ";
        //chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle5, "pie", stitle1, val_legnd , squery5 , "chart5");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart5", chartScript, false);

        //lblChart6Header.Text = stitle6;
        //sQuery = "SELECT TO_cHAR(orddt,'MM/YY') AS YR,ROUND(SUM(qtyord/100000),2) AS QTYOUT FROM pomas A WHERE BRANCHCD='00' AND TYPE LIKE '5%' GROUP BY TO_cHAR(orddt,'MM/YY') ";
        //chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle6, "gauge", stitle1, val_legnd , squery6 , "chart6");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart6", chartScript, false);

        //lblChart7Header.Text = "G.E";
        //sQuery = "SELECT TO_cHAR(VCHDATE,'MM/YY') AS YR,ROUND(SUM(iqtyin/100),2) AS QTYOUT FROM IVOUCHER A WHERE BRANCHCD='00' AND TYPE LIKE '1%' GROUP BY TO_cHAR(VCHDATE,'MM/YY') ";
        //chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, "G.E.", "line", stitle1, "Value in Lacs", sQuery, "chart7");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart7", chartScript, false);
    }

    protected void btnBox1_ServerClick(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        popUpType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_POPUPTYPE");
        printGraph();
        printHeads();
        switch (Prg_Id)
        {
            case "S15115G":
                sQuery = "Select * from wb_Css_log a where a.branchcd!='DD' and  a.cssdt " + PrdRange + "";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
                //fgen.Fn_open_sseek("", frm_qstr);
                break;
        }
    }
    protected void btnBox2_ServerClick(object sender, EventArgs e)
    {
        printGraph();
        printHeads();
        //sQuery = "Select * from wb_Css_log a where a.branchcd!='DD' and  a.cssdt " + PrdRange + "";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
        //fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnBox3_ServerClick(object sender, EventArgs e)
    {
        printGraph();
        printHeads();
        //sQuery = "Select * from wb_Css_log a where a.branchcd!='DD' and  a.cssdt " + PrdRange + "";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
        //fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnBox4_ServerClick(object sender, EventArgs e)
    {
        printGraph();
        printHeads();
        //sQuery = "Select * from wb_Css_log a where a.branchcd!='DD' and  a.cssdt " + PrdRange + "";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
        //fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        askPopUp();
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        printGraph();
        printHeads();
    }
}