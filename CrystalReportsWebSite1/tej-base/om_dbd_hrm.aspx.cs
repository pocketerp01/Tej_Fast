using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dbd_hrm : System.Web.UI.Page
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
        double box1;
        double box2;
        double box3;

        switch (Prg_Id)
        {
            case "F80132":
                Client_Code = "%";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(trainhrs) as c1 from emptrain a where a.branchcd='"+ frm_mbr +"' and a.type='10' and  a.vchdate " + PrdRange + " ", "C1");
                lblBox1Header.Text = "Training hrs Planned";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(trainhrs) as c1 from emptrain a where a.branchcd='" + frm_mbr + "' and a.type='20' and  a.vchdate " + PrdRange + " ", "C1");
                lblBox2Header.Text = "Training hrs Done";


                box1 = 0;
                box2 = 0;
                box3 = 0;
                box1=fgen.make_double(lblBox1Count.Text);
                box2=fgen.make_double(lblBox2Count.Text);
                box3 = box1 - box2;

                lblBox3Count.Text = box3.ToString(); 
                lblBox3Header.Text = "Training Hrs Pending";
                box3 = 0;
                if (box1>0)
                {
                    box3 = (box2 / box1) * 100;
                }
                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  round("+ box3 +",2) as c1 from dual ", "C1");
                lblBox4Header.Text = "Achievment %";
                break;
            case "F81132":
                Client_Code = "%";
                squery1 = "Select  sum(to_datE(trim(levupto),'dd/mm/yyyy')-to_datE(trim(levfrom),'dd/mm/yyyy')) as c1 from wb_levReq a where a.branchcd='" + frm_mbr + "' and a.type='LR' and  a.lrqdt " + PrdRange + " ";
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, squery1 , "C1");
                lblBox1Header.Text = "Days Leave Requested";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  sum(to_datE(trim(levupto),'dd/mm/yyyy')-to_datE(trim(levfrom),'dd/mm/yyyy')) as c1 from wb_levReq a where a.branchcd='" + frm_mbr + "' and a.type='LR' and  a.lrqdt " + PrdRange + " and trim(nvl(a.app_by,'-'))!='-'", "C1");
                lblBox2Header.Text = "Days Leave Approved";

                box1 = 0;
                box2 = 0;
                box3 = 0;
                box1 = fgen.make_double(lblBox1Count.Text);
                box2 = fgen.make_double(lblBox2Count.Text);
                box3 = box1 - box2;

                lblBox3Count.Text = box3.ToString();
                lblBox3Header.Text = "Leave Days Pending Appr.";
                box3 = 0;
                if (box1 > 0)
                {
                    box3 = ((box1-box2) / box1) * 100;
                }
                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select  round(" + box3 + ",2) as c1 from dual ", "C1");
                lblBox4Header.Text = "Pendency %";
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
            case "F80132":
                stitle1 = "Monthly Training Hrs Planned";
                squery1 = "SELECT TO_cHAR(a.vchdate,'Mon-YY') AS YR,sum(a.trainhrs) AS Lead_Count FROM emptrain A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='10' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(a.vchdate,'Mon-YY'),TO_cHAR(a.vchdate,'YYYYMM') order by TO_cHAR(a.vchdate,'YYYYMM')";
                stitle2 = "Monthly Training Hrs Achieved";
                squery2 = "SELECT TO_cHAR(a.vchdate,'Mon-YY') AS YR,sum(a.trainhrs) AS Lead_Count FROM emptrain A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='20' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(a.vchdate,'Mon-YY'),TO_cHAR(a.vchdate,'YYYYMM') order by TO_cHAR(a.vchdate,'YYYYMM')";
                stitle3 = "Topic Wise Hrs Achieved";
                squery3 = "SELECT trim(upper(a.tr_name)) AS YR,sum(a.trainhrs) AS Lead_Count FROM emptrain A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='20' and a.vchdate " + PrdRange + "  GROUP BY trim(upper(a.tr_name)) order by trim(upper(a.tr_name))";
                stitle4 = "Monthly Employee Trained";
                squery4 = "SELECT TO_cHAR(a.vchdate,'Mon-YY') AS YR,count(a.empcode) AS Lead_Count FROM emptrain A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='20' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(a.vchdate,'Mon-YY'),TO_cHAR(a.vchdate,'YYYYMM') order by TO_cHAR(a.vchdate,'YYYYMM')";
                stitle5 = "";
                squery5 = "";
                stitle6 = "";
                squery6 = "";
                val_legnd = "";
                break;
            case "F81132":
                stitle1 = "Monthly Leave Days Requested";
                squery1 = "SELECT TO_cHAR(a.lrqdt,'Mon-YY') AS YR,sum(to_datE(trim(levupto),'dd/mm/yyyy')-to_datE(trim(levfrom),'dd/mm/yyyy')) AS Lead_Count FROM WB_LEVREQ A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='LR' and a.lrqdt " + PrdRange + "  GROUP BY TO_cHAR(a.lrqdt,'Mon-YY'),TO_cHAR(a.lrqdt,'YYYYMM') order by TO_cHAR(a.lrqdt,'YYYYMM')";
                stitle2 = "Monthly Leave Days Approved";
                squery2 = "SELECT TO_cHAR(a.lrqdt,'Mon-YY') AS YR,sum(to_datE(trim(levupto),'dd/mm/yyyy')-to_datE(trim(levfrom),'dd/mm/yyyy')) AS Lead_Count FROM WB_LEVREQ A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='LR' and a.lrqdt " + PrdRange + "  GROUP BY TO_cHAR(a.lrqdt,'Mon-YY'),TO_cHAR(a.lrqdt,'YYYYMM') and trim(nvl(app_by,'-'))!='-' order by TO_cHAR(a.lrqdt,'YYYYMM')";
                stitle3 = "Reason Wise Leave Days";
                squery3 = "SELECT trim(upper(a.Lreason1)) AS YR,sum(to_datE(trim(levupto),'dd/mm/yyyy')-to_datE(trim(levfrom),'dd/mm/yyyy')) AS Lead_Count FROM WB_LEVREQ A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='LR' and a.lrqdt " + PrdRange + "  GROUP BY trim(upper(a.lreason1)) order by trim(upper(a.Lreason1))";
                stitle4 = "Monthly Leave Requested";
                squery4 = "SELECT TO_cHAR(a.lrqdt,'Mon-YY') AS YR,sum(to_datE(trim(levupto),'dd/mm/yyyy')-to_datE(trim(levfrom),'dd/mm/yyyy')) AS Lead_Count FROM WB_LEVREQ A WHERE a.BRANCHCD='" + frm_mbr + "' and a.type='LR' and a.lrqdt " + PrdRange + "  GROUP BY TO_cHAR(a.lrqdt,'Mon-YY'),TO_cHAR(a.lrqdt,'YYYYMM') order by TO_cHAR(a.lrqdt,'YYYYMM')";
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