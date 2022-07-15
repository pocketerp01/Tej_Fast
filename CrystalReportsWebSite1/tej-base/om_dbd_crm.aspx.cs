using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dbd_crm : System.Web.UI.Page
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