using System;
using System.Data;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_MIS_grid : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt1, dt2, dt3, dt4;
    int i = 0, z = 0; double db1 = 0;
    string header_n = "", mq0, mq1, mq2, mq3;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable dtCol = new DataTable();
    string Prg_Id, CSR; string fldvalue = "";
    string chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string vartype, chk_indust;

    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            btnnew.Focus();
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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                fgen.EnableForm(this.Controls);
                enablectrl();
                //if (lblTotcount.Text.Length == 0)
                //{
                //    div1.Visible = false;
                //}

                btnnew_ServerClick("", EventArgs.Empty);
            }
            set_Val();

            btnedit.Visible = false;
            btndel.Visible = false;
            //btnsave.Visible = false;
            btnprint.Visible = false;
            btnlist.Visible = false;
        }
        txtSearch.Enabled = true;
        setGridWidth();
    }
    //------------------------------------------------------------------------------------
    void setGridWidth()
    {
        #region
        int col_count = 0;
        double wid = 0;
        double ad = 50;
        if (sg1.Rows.Count > 0)
        {
            col_count = sg1.HeaderRow.Cells.Count;
            wid = 0;
            //for (int i = 0; i < col_count; i++)
            //{
            //    ad = 10;
            //    if (sg1.Rows[0].Cells[i].Text.Length < 2) ad = 30;
            //    else if (sg1.Rows[0].Cells[i].Text.Length < 5) ad = 25;
            //    else if (sg1.Rows[0].Cells[i].Text.Length > 50) ad = 2;
            //    else if (sg1.Rows[0].Cells[i].Text.Length > 25) ad = 5;
            //    else if (sg1.Rows[0].Cells[i].Text.Length > 20) ad = 8;
            //    wid += fgen.make_double(sg1.Rows[0].Cells[i].Text.Length, 0) * ad;
            //}
            //if (wid > 1500) wid = 1400;
            //try { sg1.Width = Convert.ToUInt16(wid + 100); }
            //catch { sg1.Width = 1500; }            

            //if (sg1.Width.Value <= 800) sg1.Width = Unit.Percentage(100);
        }
        #endregion
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;
        sg1.DataSource = null; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btnlist.Disabled = true;
        btnprint.Disabled = true;
        btndel.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnexit.Visible = false;
        btncancel.Visible = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        divMISReportsButton.Visible = false;
        divMISReportsFilter.Visible = false;
        switch (Prg_Id)
        {
            case "F61133":
                lblheader.Text = "Complaints Trend Customer Wise (12 Months)";
                break;

            case "F61136":
                lblheader.Text = "Complaint Trend Reaons Wise (12 Months)";
                break;

            case "F05133":
                lblheader.Text = "Sales Trend Party Wise (12 Months)";
                break;

            case "A3":
                lblheader.Text = "Sales Trend Item Wise (12 Months)";
                break;

            case "F05134":
                lblheader.Text = "Purchase Trend Party Wise (12 Months)";
                break;

            case "A4":
                lblheader.Text = "Purchase Trend Item Wise (12 Months)";
                break;

            case "F05126":
                lblheader.Text = "Debtor Ageing Report";
                break;

            case "F05127":
                lblheader.Text = "Creditor Ageing Report";
                break;

            case "F05143":
                lblheader.Text = "Downtime Report";
                break;

            case "F05144":
                lblheader.Text = "Rejection Report";
                break;

            case "F05145":
                lblheader.Text = "Month Wise Rejection";
                break;

            case "F05146":
                lblheader.Text = "Item Wise Rejection";
                break;

            case "F05147":
                lblheader.Text = "Machine Wise Rejection";
                break;

            case "F05148":
                lblheader.Text = "Shift Wise Rejection";
                break;

            case "F05149":
                lblheader.Text = "Machine,Month Wise Downtime";
                break;

            case "F05150":
                lblheader.Text = "Month,Machine,Item Wise Downtime";
                break;

            case "F05116":
                lblheader.Text = "Customer Wise Day Wise Sales (Value)";
                divMISReportsButton.Visible = true;
                divMISReportsFilter.Visible = true;
                break;
            case "F05117":
                lblheader.Text = "Item Wise Day Wise Sales (Value)";
                break;
            case "F05119":
                lblheader.Text = "Item Wise Day Wise Sales (Qty)";
                break;

            case "F61134":
                lblheader.Text = "Customer Wise Day Wise Complaints";
                break;

            case "F61137":
                lblheader.Text = "Reason Wise Day Wise Complaints";
                break;



            case "F05103":
            case "F05118": //main icon
                lblheader.Text = "Plant(Day) wise Sales (Value)";
                lblheader.Text = "31 day Chart - Plant wise Sales (Value)";
                break;


            case "F05112":
                lblheader.Text = "Customer(Month) wise Sales Tracking (Value)";
                lblheader.Text = "12 Month Trend of Sales - Customer Wise (Value)";
                break;

            case "F05113": //main icon
                lblheader.Text = "Plant (Month) Wise Sales Tracking (Value)";
                break;

            case "F05114":
                lblheader.Text = "Item(Month) Wise Sales Tracking (Qty)";
                lblheader.Text = "12 Month Trend of Sales - Item Wise(Qty)";
                break;
            case "F05120":
                lblheader.Text = "Item(Month) Wise Sales Tracking (Value)";
                lblheader.Text = "12 Month Trend of Sales - Item Wise(Value)";
                break;

            case "F05115":
                lblheader.Text = "Item(Day) wise Sales Qty Tracking";
                break;

            case "F05151":
                lblheader.Text = "Vendor Day Wise Purchase (Quantity)";
                break;

            case "F05152":
                lblheader.Text = "Vendor Month Wise Purchase (Quantity)";
                break;

            case "F05153":
                lblheader.Text = "Plant Wise Day Wise Purchase (Quantity)";
                break;

            case "F05154":
                lblheader.Text = "Plant Wise Month Wise Purchase (Quantity)";
                break;

            case "F05155":
                lblheader.Text = "Item Day Wise Purchase (Quantity)";
                break;

            case "F05156":
                lblheader.Text = "Item Month Wise Purchase (Quantity)";
                break;

            case "F05168":
                lblheader.Text = "Item (Day) Wise Inward";
                break;

            case "F05169":
                lblheader.Text = "Item (Month) Wise Inward";
                break;

            case "F05170":
                lblheader.Text = "Party (Day) Wise Inward";
                break;

            case "F05171":
                lblheader.Text = "Party (Month) Wise Inward";
                break;

            case "F05172":
                lblheader.Text = "Plant (Day) Wise Inward";
                break;

            case "F05173":
                lblheader.Text = "Plant (Month) Wise Inward";
                break;

            case "F05178":
                lblheader.Text = "Item (Day) Wise Outward";
                break;

            case "F05179":
                lblheader.Text = "Item (Month) Wise Outward";
                break;

            case "F05180":
                lblheader.Text = "Party (Day) Wise Outward";
                break;

            case "F05181":
                lblheader.Text = "Party (Month) Wise Outward";
                break;

            case "F05182":
                lblheader.Text = "Plant (Day) Wise Outward";
                break;

            case "F05183":
                lblheader.Text = "Plant (Month) Wise Outward";
                break;

            //default:
            //    lblheader.Text = "Management Review System";
            //    break;
        }
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";

        switch (btnval)
        {
            case "Filter1":
            case "Filter2":
            case "Filter3":
            case "Filter4":
            case "Filter5":
            case "Filter6":
                SQuery = "SELECT ACODE AS FSTR,ANAME,ACODE FROM FAMST ";
                break;
        }

        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" | btnval == "Print"))
        {
            btnval = btnval + "_E";
            hffield.Value = btnval;
            make_qry_4_popup();
        }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (chk_rights == "Y")
        {
            txtSearch.Text = "";
            if (frm_formID == "F05168" || frm_formID == "F05169" || frm_formID == "F05178" || frm_formID == "F05179")
            {
                hffield.Value = frm_formID;
                SQuery = "Select 'Q' as fstr,'Do You Want To See Report Qty Wise' as msg,'Q' as s from dual union all select 'V' as fstr,'Do You Want To See Report Value Wise' as msg,'V' as s from dual";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Choice", frm_qstr);
            }
            else
            {
                fgen.Fn_open_prddmp1("", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        string sSQuery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_JOBQUERY");
        DataTable dt1 = new DataTable();
        dt1 = fgen.getdata(frm_qstr, frm_cocd, sSQuery);
        fgen.exp_to_excel(dt1, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        sg1_dt = new DataTable();
        sg1.DataSource = null;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        //--
        string CP_BTN;
        CP_BTN = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_FROM");
        string CP_HF1;
        CP_HF1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_HF1");
        hf1.Value = CP_HF1;
        if (CP_BTN.Trim().Length > 1)
        {
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
            {
                btnval = CP_BTN;
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "0");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "0");
        //--
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {

        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "F05126":
                    SQuery = "SELECT A.SRNO,I.HSCODE AS HSCODE,A.IQTYOUT AS QTY,TO_CHAR(TRIM(A.IAMOUNT),'99,99,99,999.99') AS BASIC_VALUE,A.IRATE,A.ICODE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS DATED,I.INAME FROM IVOUCHER A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.INVNO)||TO_CHAR(A.INVDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + col1 + "' ORDER BY SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "-"); // THIS IS THE LAST POPUP SO IF USER SELECT ON THIS POPUP , PAGE DOES NOT SHOW BLANK POPUP
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;

                case "F05127":
                    SQuery = "SELECT A.SRNO,I.INAME,A.IQTYIN AS QTY,TO_CHAR(TRIM(A.IAMOUNT),'99,99,99,999.99') AS BASIC_VALUE,A.IRATE,A.ICODE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,A.ACODE FROM IVOUCHER A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.INVNO)||TO_CHAR(A.INVDATE,'DD/MM/YYYY')||TRIM(A.ACODE)='" + col1 + "' ORDER BY SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "-"); // THIS IS THE LAST POPUP SO IF USER SELECT ON THIS POPUP , PAGE DOES NOT SHOW BLANK POPUP
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                    break;

                case "F05168":
                case "F05169":
                case "F05178":
                case "F05179":
                    hf1.Value = col1;
                    fgen.Fn_open_prddmp1("", frm_qstr);
                    break;
                case "Filter1":
                    txtFilter1.Text = col1;
                    break;
                case "Filter2":
                    txtFilter2.Text = col1;
                    break;
                case "Filter3":
                    txtFilter3.Text = col1;
                    break;
                case "Filter4":
                    txtFilter4.Text = col1;
                    break;
                case "Filter5":
                    txtFilter5.Text = col1;
                    break;
                case "Filter6":
                    txtFilter6.Text = col1;
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "G")
        {
            fillgraph();
        }
        else
        {
            fillGrid();
        }
    }
    //------------------------------------------------------------------------------------
    void fillGrid()
    {
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        SQuery = ""; string m1, eff_Dt;
        vartype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MLD_PTYPE");
        chk_indust = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(Trim(opt_param)) as opt from fin_Rsys_opt where trim(opt_id)='W0000' ", "opt");
        //vartype = "90";

        string mhd = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", "INR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,99,99,999.99");

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt_pw where branchcd='" + frm_mbr + "' and opt_id='W2015'", "opt_param");
        if (mhd != "0" && mhd != "-") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", mhd);

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt_pw where branchcd='" + frm_mbr + "' and opt_id='W2016'", "opt_param");
        if (mhd != "0" && mhd != "-" && mhd != "I") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,999,999,999.99");

        fldvalue = "";
        string coma_sepr;
        coma_sepr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BR_COMMA");

        if (frm_formID == "F05118" || frm_formID == "F05115" || frm_formID == "F05116" || frm_formID == "F05117" || frm_formID == "F05119" || frm_formID == "F61134" || frm_formID == "F61137")
        {
            string mm1 = Convert.ToDateTime(todt).ToString("MMyyyy");
            string mm2 = Convert.ToDateTime(fromdt).ToString("MMyyyy");
            if (mm1 != mm2)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please select dates within a month only.!!");
                return;
            }
        }

        switch (frm_formID)
        {
            #region Account MIS
            case "F05126": // DEBTOR AGEING
                m1 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R01'", "params");
                eff_Dt = " vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy')";
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdataw as (select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD'  and  " + eff_Dt + "  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE BRANCHCD NOT IN ('DD','88') and SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO");
                SQuery = "Select trim(acode) as fstr,acode,party,address,total_outstanding,current_os,over_30_60,over_61_90,over_90_180,over_181,Totos as Tot,p_days as payment_Terms from (select m.aname as Party,m.ADDR1 as Address,to_char(sum(n.total),'99,99,99,999.99') as Total_Outstanding,to_char(sum(n.slab1),'99,99,99,999.99') as Current_Os,to_char(sum(n.slab2),'99,99,99,999.99') as OVER_30_60,to_char(sum(n.slab3),'99,99,99,999.99') as OVER_61_90,to_char(sum(n.slab4),'99,99,99,999.99') as OVER_90_180,to_char(sum(n.slab5),'99,99,99,999.99') as OVER_181,n.acode,sum(n.total) as totos,sum(n.slab1) as s1,sum(n.slab2) as s2,sum(n.slab3) as s3,sum(n.slab4) as s4,sum(n.slab5) as s5,m.Payment as P_days,m.Climit  as Cr_limit,m.acode as Zcode from (SELECT acode,dramt-cramt as total,(CASE WHEN (sysdate-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (sysdate-invdate BETWEEN 30 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (sysdate-invdate BETWEEN 60 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (sysdate-invdate BETWEEN 90 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (sysdate-invdate > 180) THEN dramt-cramt END) as slab5 from recdataw) n left outer join famst m on trim(n.acode)=trim(m.acode) where substr(n.acode,1,2) in ('16') and n.total<>0 group by m.aname,m.addr1,m.climit,m.payment,n.acode,m.acode having sum(n.total)>0) where totos>0 order by Party";
                break;

            case "F05127": // CREDITOR AGEING
                m1 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R01'", "params");
                eff_Dt = " vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy')";
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdataw as (select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD' and " + eff_Dt + "  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE BRANCHCD NOT IN ('DD','88') and SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO");
                //SQuery = "Select trim(acode) as fstr,acode,party,address,total_outstanding,nvl(current_os,0) as current_os,nvl(over_30_60,0) as over_30_60,nvl(over_61_90,0) as over_61_90,nvl(over_90_180,0) as over_90_180,nvl(over_181,0) as over_181,Totos as Tot from (select m.aname as Party,m.ADDR1 as Address,to_char(sum(n.total),'99,99,99,999.99') as Total_Outstanding,to_char(sum(n.slab1),'99,99,99,999.99') as Current_Os,to_char(sum(n.slab2),'99,99,99,999.99') as OVER_30_60,to_char(sum(n.slab3),'99,99,99,999.99') as OVER_61_90,to_char(sum(n.slab4),'99,99,99,999.99') as OVER_90_180,to_char(sum(n.slab5),'99,99,99,999.99') as OVER_181,n.acode,sum(n.total) as totos,sum(n.slab1) as s1,sum(n.slab2) as s2,sum(n.slab3) as s3,sum(n.slab4) as s4,sum(n.slab5) as s5 from (SELECT acode,dramt-cramt as total,(CASE WHEN (sysdate-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (sysdate-invdate BETWEEN 30 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (sysdate-invdate BETWEEN 60 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (sysdate-invdate BETWEEN 90 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (sysdate-invdate > 180) THEN dramt-cramt END) as slab5 from recdataw) n left outer join famst m on trim(n.acode)=trim(m.acode) where substr(n.acode,1,2) in ('05','06') group by m.aname,m.addr1,n.acode HAVING sum(n.total)<>0 ORDER BY M.ANAME) where totos<0 order by Party";
                SQuery = "Select trim(acode) as fstr,acode,party,address,total_outstanding,current_os,over_30_60,over_61_90,over_90_180,over_181,Totos as Tot from (select m.aname as Party,m.ADDR1 as Address,to_char(sum(n.total),'99,99,99,999.99') as Total_Outstanding,to_char(sum(n.slab1),'99,99,99,999.99') as Current_Os,to_char(sum(n.slab2),'99,99,99,999.99') as OVER_30_60,to_char(sum(n.slab3),'99,99,99,999.99') as OVER_61_90,to_char(sum(n.slab4),'99,99,99,999.99') as OVER_90_180,to_char(sum(n.slab5),'99,99,99,999.99') as OVER_181,n.acode,sum(n.total) as totos,sum(n.slab1) as s1,sum(n.slab2) as s2,sum(n.slab3) as s3,sum(n.slab4) as s4,sum(n.slab5) as s5 from (SELECT acode,dramt-cramt as total,(CASE WHEN (sysdate-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (sysdate-invdate BETWEEN 30 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (sysdate-invdate BETWEEN 60 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (sysdate-invdate BETWEEN 90 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (sysdate-invdate > 180) THEN dramt-cramt END) as slab5 from recdataw) n left outer join famst m on trim(n.acode)=trim(m.acode) where substr(n.acode,1,2) in ('05','06') group by m.aname,m.addr1,n.acode HAVING sum(n.total)<>0 ORDER BY M.ANAME) where totos<0 order by Party";
                break;

            case "F05133": // SALES TREND PARTY WISE
                SQuery = "select a.acode as fstr,trim(a.acode) as party_code, trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select vchnum,vchdate ,icode,acode,(case when to_char(VCHDATE,'mm')='04' then iqtyout else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyout else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyout else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyout else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyout else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyout else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyout else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyout else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyout else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyout else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  iqtyout else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyout else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' AND VCHDATE " + DateRange + " ) a, famst b where trim(a.acode)=trim(b.acode) group by b.aname,a.acode ";
                break;
            case "F61133":
                SQuery = "select a.acode as fstr,trim(a.acode) as party_code, trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select ccmno as vchnum,ccmdt as vchdate ,icode,acode,(case when to_char(ccmdt,'mm')='04' then 1  else 0 end ) as apr,(case when to_char(ccmdt,'mm')='05' then 1  else 0 end ) as may,(case when to_char(ccmdt,'mm')='06' then 1  else 0 end ) as jun,(case when to_char(ccmdt,'mm')='07' then 1  else 0 end ) as jul,(case when to_char(ccmdt,'mm')='08' then 1  else 0 end ) as aug,(case when to_char(ccmdt,'mm')='09' then 1  else 0 end ) as sep,(case when to_char(ccmdt,'mm')='10' then  1  else 0 end ) as oct,(case when to_char(ccmdt,'mm')='11' then  1  else 0 end ) as nov,(case when to_char(ccmdt,'mm')='12' then  1  else 0 end ) as dec,(case when to_char(ccmdt,'mm')='01' then  1  else 0 end ) as jan,(case when to_char(ccmdt,'mm')='02' then  1  else 0 end ) as feb,(case when to_char(ccmdt,'mm')='03' then 1  else 0 end ) as mar from wb_ccm_log where branchcd='" + frm_mbr + "' and type like 'CC%' AND ccmdt " + DateRange + " ) a, famst b where trim(a.acode)=trim(b.acode) group by b.aname,a.acode ";
                break;
            case "F61136":
                SQuery = "select a.acode as fstr,trim(a.acode) as Reason, trim(a.acode) as Reason_Name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select ccmno as vchnum,ccmdt as vchdate ,icode,comp_type as acode,(case when to_char(ccmdt,'mm')='04' then 1  else 0 end ) as apr,(case when to_char(ccmdt,'mm')='05' then 1  else 0 end ) as may,(case when to_char(ccmdt,'mm')='06' then 1  else 0 end ) as jun,(case when to_char(ccmdt,'mm')='07' then 1  else 0 end ) as jul,(case when to_char(ccmdt,'mm')='08' then 1  else 0 end ) as aug,(case when to_char(ccmdt,'mm')='09' then 1  else 0 end ) as sep,(case when to_char(ccmdt,'mm')='10' then  1  else 0 end ) as oct,(case when to_char(ccmdt,'mm')='11' then  1  else 0 end ) as nov,(case when to_char(ccmdt,'mm')='12' then  1  else 0 end ) as dec,(case when to_char(ccmdt,'mm')='01' then  1  else 0 end ) as jan,(case when to_char(ccmdt,'mm')='02' then  1  else 0 end ) as feb,(case when to_char(ccmdt,'mm')='03' then 1  else 0 end ) as mar from wb_ccm_log where branchcd='" + frm_mbr + "' and type like 'CC%' AND ccmdt " + DateRange + " ) a group by a.acode ";
                break;


            case "A3": // SALES TREND ITEM WISE
                SQuery = "select a.icode as fstr,trim(a.icode) as Item_code, trim(b.iname) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select vchnum,vchdate ,icode,(case when to_char(VCHDATE,'mm')='04' then iqtyout else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyout else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyout else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyout else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyout else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyout else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyout else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyout else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyout else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyout else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  iqtyout else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyout else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' AND VCHDATE " + DateRange + " ) a, item b  where trim(a.icode)=trim(b.icode)  group by b.iname,a.icode";
                break;

            case "F05134": // PURCHASE TREND PARTY WISE
                SQuery = "select distinct A.acode AS FSTR,trim(a.acode) as party_code , trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select vchnum,vchdate ,icode,acode,(case when to_char(VCHDATE,'mm')='04' then iqtyin else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyin else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyin else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyin else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyin  else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyin else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyin  else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyin else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyin else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyin  else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then iqtyin  else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyin else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' AND VCHDATE " + DateRange + " ) a, famst b  where trim(a.acode)=trim(b.acode)  group by b.aname,a.acode";
                break;

            case "A4": // PURCHASE TREND ITEM WISE
                SQuery = "select distinct A.Icode AS FSTR,trim(a.Icode) as iTEM_code , trim(b.Iname) as ITEM_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select vchnum,vchdate ,icode,acode,(case when to_char(VCHDATE,'mm')='04' then iqtyin else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyin else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyin else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyin else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyin  else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyin else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyin  else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyin else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyin else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyin  else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then iqtyin  else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyin else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' AND VCHDATE " + DateRange + ") a, ITEM b  where trim(a.Icode)=trim(b.Icode)  group by b.Iname,a.Icode";
                break;
            #endregion

            #region Production MIS
            case "F05143": // DOWNTIME
                SQuery = "select trim(col2) as fstr,trim(col2) as code,col1 as name,sum(is_number(day1)+is_number(day2)+is_number(day3)+is_number(day4)+is_number(day5)+is_number(day6)+is_number(day7)+is_number(day8)+is_number(day9)+is_number(day10)+is_number(day11)+is_number(day12)+is_number(day13)+is_number(day14)+is_number(day15)+is_number(day16)+is_number(day17)+is_number(day18)+is_number(day19)+is_number(day20)+is_number(day21)+is_number(day22)+is_number(day23)+is_number(day24)+is_number(day25)+is_number(day26)+is_number(day27)+is_number(day28)+is_number(day29)+is_number(day30)+is_number(day31)) as TotalMinutes,sum(is_number(day1)) as day1,sum(is_number(day2))as day2,sum(is_number(day3))as day3,sum(is_number(day4))as day4,sum(is_number(day5))as day5,sum(is_number(day6))as day6,sum(is_number(day7))as day7,sum(is_number(day8))as day8,sum(is_number(day9))as day9,sum(is_number(day10))as day10,sum(is_number(day11))as day11,sum(is_number(day12))as day12,sum(is_number(day13))as day13,sum(is_number(day14))as day14,sum(is_number(day15))as day15,sum(is_number(day16))as day16,sum(is_number(day17))as day17,sum(is_number(day18))as day18,sum(is_number(day19))as day19,sum(is_number(day20))as day20,sum(is_number(day21))as day21,sum(is_number(day22))as day22,sum(is_number(day23))as day23,sum(is_number(day24))as day24,sum(is_number(day25))as day25,sum(is_number(day26))as day26,sum(is_number(day27))as day27,sum(is_number(day28))as day28,sum(is_number(day29))as day29,sum(is_number(day30))as day30,sum(is_number(day31))as day31 from (select col1,col2,(case when to_char(vchdate,'dd')='01' then col3 else '0' end)  AS day1,(case when to_char(vchdate,'dd')='02' then col3 else '0' end)  AS day2,(case when to_char(vchdate,'dd')='03' then col3 else '0' end)  AS day3,(case when to_char(vchdate,'dd')='04' then col3 else '0' end)  AS day4,(case when to_char(vchdate,'dd')='05' then col3 else '0' end)  AS day5,(case when to_char(vchdate,'dd')='06' then col3 else '0' end)  AS day6,(case when to_char(vchdate,'dd')='07' then col3 else '0' end)  AS day7,(case when to_char(vchdate,'dd')='08' then col3 else '0' end)  AS day8,(case when to_char(vchdate,'dd')='09' then col3 else '0' end)  AS day9,(case when to_char(vchdate,'dd')='10' then col3 else '0' end)  AS day10,(case when to_char(vchdate,'dd')='11' then col3 else '0' end)  AS day11,(case when to_char(vchdate,'dd')='12' then col3 else '0' end)  AS day12,(case when to_char(vchdate,'dd')='13' then col3 else '0' end)  AS day13,(case when to_char(vchdate,'dd')='14' then col3 else '0' end)  AS day14,(case when to_char(vchdate,'dd')='15' then col3 else '0' end)  AS day15,(case when to_char(vchdate,'dd')='16' then col3 else '0' end)  AS day16,(case when to_char(vchdate,'dd')='17' then col3 else '0' end)  AS day17,(case when to_char(vchdate,'dd')='18' then col3 else '0' end)  AS day18,(case when to_char(vchdate,'dd')='19' then col3 else '0' end)  AS day19,(case when to_char(vchdate,'dd')='20' then col3 else '0' end)  AS day20,(case when to_char(vchdate,'dd')='21' then col3 else '0' end)  AS day21,(case when to_char(vchdate,'dd')='22' then col3 else '0' end)  AS day22,(case when to_char(vchdate,'dd')='23' then col3 else '0' end)  AS day23,(case when to_char(vchdate,'dd')='24' then col3 else '0' end)  AS day24,(case when to_char(vchdate,'dd')='25' then col3 else '0' end)  AS day25,(case when to_char(vchdate,'dd')='26' then col3 else '0' end)  AS day26,(case when to_char(vchdate,'dd')='27' then col3 else '0' end)  AS day27,(case when to_char(vchdate,'dd')='28' then col3 else '0' end)  AS day28,(case when to_char(vchdate,'dd')='29' then col3 else '0' end)  AS day29,(case when to_char(vchdate,'dd')='30' then col3 else '0' end)  AS day30,(case when to_char(vchdate,'dd')='31' then col3 else '0' end)  AS day31 FROM INSPVCH where branchcd='" + frm_mbr + "' and  type='55' and vchdate " + DateRange + ") group by col1,col2 order by name";
                break;

            case "F05144": // REJECTION
                SQuery = "select trim(col2) as fstr,trim(col2) as code,col1 as name,sum(is_number(day1)+is_number(day2)+is_number(day3)+is_number(day4)+is_number(day5)+is_number(day6)+is_number(day7)+is_number(day8)+is_number(day9)+is_number(day10)+is_number(day11)+is_number(day12)+is_number(day13)+is_number(day14)+is_number(day15)+is_number(day16)+is_number(day17)+is_number(day18)+is_number(day19)+is_number(day20)+is_number(day21)+is_number(day22)+is_number(day23)+is_number(day24)+is_number(day25)+is_number(day26)+is_number(day27)+is_number(day28)+is_number(day29)+is_number(day30)+is_number(day31)) as TotalRejQty,sum(is_number(day1)) as day1,sum(is_number(day2))as day2,sum(is_number(day3))as day3,sum(is_number(day4))as day4,sum(is_number(day5))as day5,sum(is_number(day6))as day6,sum(is_number(day7))as day7,sum(is_number(day8))as day8,sum(is_number(day9))as day9,sum(is_number(day10))as day10,sum(is_number(day11))as day11,sum(is_number(day12))as day12,sum(is_number(day13))as day13,sum(is_number(day14))as day14,sum(is_number(day15))as day15,sum(is_number(day16))as day16,sum(is_number(day17))as day17,sum(is_number(day18))as day18,sum(is_number(day19))as day19,sum(is_number(day20))as day20,sum(is_number(day21))as day21,sum(is_number(day22))as day22,sum(is_number(day23))as day23,sum(is_number(day24))as day24,sum(is_number(day25))as day25,sum(is_number(day26))as day26,sum(is_number(day27))as day27,sum(is_number(day28))as day28,sum(is_number(day29))as day29,sum(is_number(day30))as day30,sum(is_number(day31))as day31 from (select col1,col2,(case when to_char(vchdate,'dd')='01' then col3 else '0' end)  AS day1,(case when to_char(vchdate,'dd')='02' then col3 else '0' end)  AS day2,(case when to_char(vchdate,'dd')='03' then col3 else '0' end)  AS day3,(case when to_char(vchdate,'dd')='04' then col3 else '0' end)  AS day4,(case when to_char(vchdate,'dd')='05' then col3 else '0' end)  AS day5,(case when to_char(vchdate,'dd')='06' then col3 else '0' end)  AS day6,(case when to_char(vchdate,'dd')='07' then col3 else '0' end)  AS day7,(case when to_char(vchdate,'dd')='08' then col3 else '0' end)  AS day8,(case when to_char(vchdate,'dd')='09' then col3 else '0' end)  AS day9,(case when to_char(vchdate,'dd')='10' then col3 else '0' end)  AS day10,(case when to_char(vchdate,'dd')='11' then col3 else '0' end)  AS day11,(case when to_char(vchdate,'dd')='12' then col3 else '0' end)  AS day12,(case when to_char(vchdate,'dd')='13' then col3 else '0' end)  AS day13,(case when to_char(vchdate,'dd')='14' then col3 else '0' end)  AS day14,(case when to_char(vchdate,'dd')='15' then col3 else '0' end)  AS day15,(case when to_char(vchdate,'dd')='16' then col3 else '0' end)  AS day16,(case when to_char(vchdate,'dd')='17' then col3 else '0' end)  AS day17,(case when to_char(vchdate,'dd')='18' then col3 else '0' end)  AS day18,(case when to_char(vchdate,'dd')='19' then col3 else '0' end)  AS day19,(case when to_char(vchdate,'dd')='20' then col3 else '0' end)  AS day20,(case when to_char(vchdate,'dd')='21' then col3 else '0' end)  AS day21,(case when to_char(vchdate,'dd')='22' then col3 else '0' end)  AS day22,(case when to_char(vchdate,'dd')='23' then col3 else '0' end)  AS day23,(case when to_char(vchdate,'dd')='24' then col3 else '0' end)  AS day24,(case when to_char(vchdate,'dd')='25' then col3 else '0' end)  AS day25,(case when to_char(vchdate,'dd')='26' then col3 else '0' end)  AS day26,(case when to_char(vchdate,'dd')='27' then col3 else '0' end)  AS day27,(case when to_char(vchdate,'dd')='28' then col3 else '0' end)  AS day28,(case when to_char(vchdate,'dd')='29' then col3 else '0' end)  AS day29,(case when to_char(vchdate,'dd')='30' then col3 else '0' end)  AS day30,(case when to_char(vchdate,'dd')='31' then col3 else '0' end)  AS day31 FROM INSPVCH where branchcd='" + frm_mbr + "' and type='45' and vchdate " + DateRange + ") group by col1,col2 order by name";
                break;

            case "F05145": // MONTH WISE REJECTION
                SQuery = "SELECT TO_CHAR(a.VCHDATE,'mm/yyyy') as fstr,TO_CHAR(a.VCHDATE,'Month') AS MONTH,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11,SUM(nvl(A.A12,0)) AS A12,SUM(nvl(A.A13,0)) AS A13,SUM(nvl(A.A14,0)) AS A14,SUM(nvl(A.A15,0)) AS A15,SUM(nvl(A.A16,0)) AS A16,SUM(nvl(A.A17,0)) AS A17,SUM(nvl(A.A18,0)) AS A18,SUM(nvl(A.A19,0)) AS A19,SUM(nvl(A.A20,0)) AS A20,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD FROM PROD_SHEET A WHERE a.branchcd='" + frm_mbr + "' and A.TYPE='" + vartype + "' AND A.VCHDATE " + DateRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),TO_CHAR(a.VCHDATE,'mm/yyyy') HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY VDD";
                break;

            case "F05146": // ITEM WISE REJECTION
                SQuery = "SELECT TRIM(A.ICODE)||TO_CHAR(a.VCHDATE,'mm/yyyy') as fstr,A.ICODE,I.INAME,TO_CHAR(a.VCHDATE,'Month') AS MONTH,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11,SUM(nvl(A.A12,0)) AS A12,SUM(nvl(A.A13,0)) AS A13,SUM(nvl(A.A14,0)) AS A14,SUM(nvl(A.A15,0)) AS A15,SUM(nvl(A.A16,0)) AS A16,SUM(nvl(A.A17,0)) AS A17,SUM(nvl(A.A18,0)) AS A18,SUM(nvl(A.A19,0)) AS A19,SUM(nvl(A.A20,0)) AS A20,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND a.branchcd='" + frm_mbr + "' and A.TYPE='" + vartype + "' AND A.VCHDATE " + DateRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),TO_CHAR(a.VCHDATE,'mm/yyyy'),A.ICODE,I.INAME HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY INAME";
                break;

            case "F05147": // MACHINE WISE REJECTION
                SQuery = "SELECT TRIM(A.MCHCODE)||TO_CHAR(a.VCHDATE,'mm/yyyy') AS FSTR,TRIM(A.MCHCODE) AS CODE, A.ENAME AS MACHINE,TO_CHAR(a.VCHDATE,'Month') AS MONTH,MAX(I.CPARTNO) AS CPARTNO,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11,SUM(nvl(A.A12,0)) AS A12,SUM(nvl(A.A13,0)) AS A13,SUM(nvl(A.A14,0)) AS A14,SUM(nvl(A.A15,0)) AS A15,SUM(nvl(A.A16,0)) AS A16,SUM(nvl(A.A17,0)) AS A17,SUM(nvl(A.A18,0)) AS A18,SUM(nvl(A.A19,0)) AS A19,SUM(nvl(A.A20,0)) AS A20,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + DateRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),A.ENAME,TRIM(A.MCHCODE),TO_CHAR(a.VCHDATE,'mm/yyyy') HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY MACHINE";
                break;

            case "F05148": // SHIFT WISE REJECTION
                SQuery = "SELECT TRIM(A.SHFTCODE)||TO_CHAR(a.VCHDATE,'mm/yyyy') AS FSTR,TRIM(A.SHFTCODE) AS CODE, A.VAR_CODE AS SHIFT,TO_CHAR(a.VCHDATE,'Month') AS MONTH,MAX(I.CPARTNO) AS CPARTNO,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11,SUM(nvl(A.A12,0)) AS A12,SUM(nvl(A.A13,0)) AS A13,SUM(nvl(A.A14,0)) AS A14,SUM(nvl(A.A15,0)) AS A15,SUM(nvl(A.A16,0)) AS A16,SUM(nvl(A.A17,0)) AS A17,SUM(nvl(A.A18,0)) AS A18,SUM(nvl(A.A19,0)) AS A19,SUM(nvl(A.A20,0)) AS A20,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + DateRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),A.VAR_CODE,TRIM(A.SHFTCODE),TO_CHAR(a.VCHDATE,'mm/yyyy') HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY SHIFT";
                break;

            case "F05149": // MACHINE,MONTH WISE DOWNTIME
                SQuery = "select trim(mchcode)||to_char(vchdate,'mm/yyyy') as fstr,trim(mchcode) as code,ename as machine,TO_CHAR(VCHDATE,'Month') AS MONTH,round(SUM(nvl(NUM1,0)),2)+round(SUM(nvl(NUM2,0)),2) +round(SUM(nvl(NUM3,0)),2)+round(SUM(nvl(NUM4,0)),2)+round(SUM(nvl(NUM5,0)),2)+round(SUM(nvl(NUM6,0)),2) +round(SUM(nvl(NUM7,0)),2)+round(SUM(nvl(NUM8,0)),2)+round(SUM(nvl(NUM9,0)),2)+round(SUM(nvl(NUM10,0)),2)+round(SUM(nvl(NUM11,0)),2)+round(SUM(nvl(NUM12,0)),2) AS TotalMinutes,round(SUM(nvl(NUM1,0)),2) AS A1,round(SUM(nvl(NUM2,0)),2) AS A2,round(SUM(nvl(NUM3,0)),2) AS A3,round(SUM(nvl(NUM4,0)),2) AS A4,round(SUM(nvl(NUM5,0)),2) AS A5,round(SUM(nvl(NUM6,0)),2) AS A6,round(SUM(nvl(NUM7,0)),2) AS A7,round(SUM(nvl(NUM8,0)),2) AS A8,round(SUM(nvl(NUM9,0)),2) AS A9,round(SUM(nvl(NUM10,0)),2) AS A10,round(SUM(nvl(NUM11,0)),2) AS A11,round(SUM(nvl(NUM12,0)),2) AS A12,TO_CHAR(VCHDATE,'YYYYMM') AS VDd from prod_sheet where branchcd='" + frm_mbr + "' and type='" + vartype + "' and VCHDATE " + DateRange + " GROUP BY ename,TO_CHAR(VCHDATE,'Month'),TO_CHAR(VCHDATE,'YYYYMM'),trim(mchcode),to_char(vchdate,'mm/yyyy') order by vdd";
                break;

            case "F05150": // ITEM,MACHINE,MONTH WISE DOWNTIME
                SQuery = "select trim(a.mchcode)||trim(a.icode)||to_char(a.vchdate,'mm/yyyy') as fstr, a.ename as machine,trim(a.icode) as icode,b.iname,MAX(b.CPARTNO) AS CPARTNO, TO_CHAR(a.VCHDATE,'Month') AS MONTH,SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)) AS TOTPROD,round(SUM(nvl(a.NUM1,0)),2)+round(SUM(nvl(a.NUM2,0)),2) +round(SUM(nvl(a.NUM3,0)),2)+round(SUM(nvl(a.NUM4,0)),2)+round(SUM(nvl(a.NUM5,0)),2)+round(SUM(nvl(a.NUM6,0)),2) +round(SUM(nvl(a.NUM7,0)),2)+round(SUM(nvl(a.NUM8,0)),2)+round(SUM(nvl(a.NUM9,0)),2)+round(SUM(nvl(a.NUM10,0)),2)+round(SUM(nvl(a.NUM11,0)),2)+round(SUM(nvl(a.NUM12,0)),2) AS TotalMinutes,round(SUM(nvl(a.NUM1,0)),2) AS A1,round(SUM(nvl(a.NUM2,0)),2) AS A2,round(SUM(nvl(a.NUM3,0)),2) AS  A3,round(SUM(nvl(a.NUM4,0)),2) AS A4 ,round(SUM(nvl(a.NUM5,0)),2) AS  A5,round(SUM(nvl(a.NUM6,0)),2) AS A6,round(SUM(nvl(a.NUM7,0)),2) AS  A7,round(SUM(nvl(a.NUM8,0)),2) AS  A8,round(SUM(nvl(a.NUM9,0)),2) AS  A9,round(SUM(nvl(a.NUM10,0)),2) AS  A10,round(SUM(nvl(a.NUM11,0)),2) AS A11,round(SUM(nvl(a.NUM12,0)),2) AS A12 ,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDd from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and a.VCHDATE " + DateRange + " GROUP BY a.ename,TO_CHAR(a.VCHDATE,'Month'),TO_CHAR(a.VCHDATE,'YYYYMM'),trim(a.icode),b.iname,trim(a.mchcode),to_char(a.vchdate,'mm/yyyy') order by machine,vdd";
                break;
            #endregion

            #region Sale MIS
            case "F05116": // CUSTOMER DAY WISE SALE
                fldvalue = "round(iamount/100000,2)";
                cond = "";
                if (txtFilter1.Text.Length > 1) cond = " AND TRIM(A.ACODE)='" + txtFilter1.Text.Trim() + "' ";
                // filter comment to put here.
                SQuery = "SELECT trim(A.ACODE) as fstr,A.ACODE as code,d.aname as Party,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(DAY_01) as DAY_01,sum(DAY_02) as DAY_02,sum(DAY_03) as DAY_03,sum(DAY_04) as DAY_04,sum(DAY_05) as DAY_05,sum(DAY_06) as DAY_06,sum(DAY_07) as DAY_07,sum(DAY_08) as DAY_08,sum(DAY_09) as DAY_09,sum(DAY_10) as DAY_10,sum(DAY_11) as DAY_11,sum(DAY_12) as DAY_12,sum(DAY_13) as DAY_13,sum(DAY_14) as DAY_14,sum(DAY_15) as DAY_15,sum(DAY_16) as DAY_16,sum(DAY_17) as DAY_17,sum(DAY_18) as DAY_18,sum(DAY_19) as DAY_19,sum(DAY_20) as DAY_20,sum(DAY_21) as DAY_21,sum(DAY_22) as DAY_22,sum(DAY_23) as DAY_23,sum(DAY_24) as DAY_24,sum(DAY_25) as DAY_25,sum(DAY_26) as DAY_26,sum(DAY_27) as DAY_27,sum(DAY_28) as DAY_28,sum(DAY_29) as DAY_29,sum(DAY_30) as DAY_30,sum(DAY_31) as DAY_31 FROM (SELECT ACODE,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher  where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND  vchdate " + DateRange + ") a,famst D WHERE TRIM(A.ACODE)=TRIM(D.ACODE) " + cond + " group by A.ACODE, d.aname order by Party";
                break;
            case "F05117": // Item DAY WISE SALE
                fldvalue = "round(iamount/100000,2)";
                SQuery = "SELECT trim(A.ICODE) as fstr,A.ICODE as code,d.Iname as Party,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(DAY_01) as DAY_01,sum(DAY_02) as DAY_02,sum(DAY_03) as DAY_03,sum(DAY_04) as DAY_04,sum(DAY_05) as DAY_05,sum(DAY_06) as DAY_06,sum(DAY_07) as DAY_07,sum(DAY_08) as DAY_08,sum(DAY_09) as DAY_09,sum(DAY_10) as DAY_10,sum(DAY_11) as DAY_11,sum(DAY_12) as DAY_12,sum(DAY_13) as DAY_13,sum(DAY_14) as DAY_14,sum(DAY_15) as DAY_15,sum(DAY_16) as DAY_16,sum(DAY_17) as DAY_17,sum(DAY_18) as DAY_18,sum(DAY_19) as DAY_19,sum(DAY_20) as DAY_20,sum(DAY_21) as DAY_21,sum(DAY_22) as DAY_22,sum(DAY_23) as DAY_23,sum(DAY_24) as DAY_24,sum(DAY_25) as DAY_25,sum(DAY_26) as DAY_26,sum(DAY_27) as DAY_27,sum(DAY_28) as DAY_28,sum(DAY_29) as DAY_29,sum(DAY_30) as DAY_30,sum(DAY_31) as DAY_31 FROM (SELECT ICODE,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher  where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND  vchdate " + DateRange + ") a,item D WHERE TRIM(A.ICODE)=TRIM(D.ICODE) group by A.ICODE, d.iname order by Party";
                break;
            case "F05119": // Item DAY WISE Qty
                fldvalue = "round(iqtyout,2)";
                SQuery = "SELECT trim(A.ICODE) as fstr,A.ICODE as code,d.Iname as Party,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(DAY_01) as DAY_01,sum(DAY_02) as DAY_02,sum(DAY_03) as DAY_03,sum(DAY_04) as DAY_04,sum(DAY_05) as DAY_05,sum(DAY_06) as DAY_06,sum(DAY_07) as DAY_07,sum(DAY_08) as DAY_08,sum(DAY_09) as DAY_09,sum(DAY_10) as DAY_10,sum(DAY_11) as DAY_11,sum(DAY_12) as DAY_12,sum(DAY_13) as DAY_13,sum(DAY_14) as DAY_14,sum(DAY_15) as DAY_15,sum(DAY_16) as DAY_16,sum(DAY_17) as DAY_17,sum(DAY_18) as DAY_18,sum(DAY_19) as DAY_19,sum(DAY_20) as DAY_20,sum(DAY_21) as DAY_21,sum(DAY_22) as DAY_22,sum(DAY_23) as DAY_23,sum(DAY_24) as DAY_24,sum(DAY_25) as DAY_25,sum(DAY_26) as DAY_26,sum(DAY_27) as DAY_27,sum(DAY_28) as DAY_28,sum(DAY_29) as DAY_29,sum(DAY_30) as DAY_30,sum(DAY_31) as DAY_31 FROM (SELECT ICODE,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher  where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND  vchdate " + DateRange + ") a,item D WHERE TRIM(A.ICODE)=TRIM(D.ICODE) group by A.ICODE, d.iname order by Party";
                break;

            case "F61134": // Customer Day wise complaints WISE Qty
                fldvalue = "1";
                SQuery = "SELECT trim(A.ACODE) as fstr,A.ACODE as code,d.Aname as Party,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(DAY_01) as DAY_01,sum(DAY_02) as DAY_02,sum(DAY_03) as DAY_03,sum(DAY_04) as DAY_04,sum(DAY_05) as DAY_05,sum(DAY_06) as DAY_06,sum(DAY_07) as DAY_07,sum(DAY_08) as DAY_08,sum(DAY_09) as DAY_09,sum(DAY_10) as DAY_10,sum(DAY_11) as DAY_11,sum(DAY_12) as DAY_12,sum(DAY_13) as DAY_13,sum(DAY_14) as DAY_14,sum(DAY_15) as DAY_15,sum(DAY_16) as DAY_16,sum(DAY_17) as DAY_17,sum(DAY_18) as DAY_18,sum(DAY_19) as DAY_19,sum(DAY_20) as DAY_20,sum(DAY_21) as DAY_21,sum(DAY_22) as DAY_22,sum(DAY_23) as DAY_23,sum(DAY_24) as DAY_24,sum(DAY_25) as DAY_25,sum(DAY_26) as DAY_26,sum(DAY_27) as DAY_27,sum(DAY_28) as DAY_28,sum(DAY_29) as DAY_29,sum(DAY_30) as DAY_30,sum(DAY_31) as DAY_31 FROM (SELECT ACODE,decode(TO_CHAR(ccmdt,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(ccmdt,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(ccmdt,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(ccmdt,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(ccmdt,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(ccmdt,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(ccmdt,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(ccmdt,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(ccmdt,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(ccmdt,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(ccmdt,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(ccmdt,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(ccmdt,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(ccmdt,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(ccmdt,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(ccmdt,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(ccmdt,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(ccmdt,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(ccmdt,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(ccmdt,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(ccmdt,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(ccmdt,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(ccmdt,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(ccmdt,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(ccmdt,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(ccmdt,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(ccmdt,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(ccmdt,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(ccmdt,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(ccmdt,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(ccmdt,'DD'),'31'," + fldvalue + ",0) as DAY_31 from wb_ccm_log where branchcd='" + frm_mbr + "' and type like 'CC%' AND  ccmdt " + DateRange + ") a,famst D WHERE TRIM(A.aCODE)=TRIM(D.aCODE) group by A.aCODE, d.aname order by Party";
                break;
            case "F61137": // Reason Day wise complaints Qty
                fldvalue = "1";
                SQuery = "SELECT trim(A.aCODE) as fstr,A.aCODE as Comp_reason,a.acode as Comp_reason,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(DAY_01) as DAY_01,sum(DAY_02) as DAY_02,sum(DAY_03) as DAY_03,sum(DAY_04) as DAY_04,sum(DAY_05) as DAY_05,sum(DAY_06) as DAY_06,sum(DAY_07) as DAY_07,sum(DAY_08) as DAY_08,sum(DAY_09) as DAY_09,sum(DAY_10) as DAY_10,sum(DAY_11) as DAY_11,sum(DAY_12) as DAY_12,sum(DAY_13) as DAY_13,sum(DAY_14) as DAY_14,sum(DAY_15) as DAY_15,sum(DAY_16) as DAY_16,sum(DAY_17) as DAY_17,sum(DAY_18) as DAY_18,sum(DAY_19) as DAY_19,sum(DAY_20) as DAY_20,sum(DAY_21) as DAY_21,sum(DAY_22) as DAY_22,sum(DAY_23) as DAY_23,sum(DAY_24) as DAY_24,sum(DAY_25) as DAY_25,sum(DAY_26) as DAY_26,sum(DAY_27) as DAY_27,sum(DAY_28) as DAY_28,sum(DAY_29) as DAY_29,sum(DAY_30) as DAY_30,sum(DAY_31) as DAY_31 FROM (SELECT comp_type as ACODE,decode(TO_CHAR(ccmdt,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(ccmdt,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(ccmdt,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(ccmdt,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(ccmdt,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(ccmdt,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(ccmdt,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(ccmdt,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(ccmdt,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(ccmdt,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(ccmdt,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(ccmdt,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(ccmdt,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(ccmdt,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(ccmdt,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(ccmdt,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(ccmdt,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(ccmdt,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(ccmdt,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(ccmdt,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(ccmdt,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(ccmdt,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(ccmdt,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(ccmdt,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(ccmdt,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(ccmdt,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(ccmdt,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(ccmdt,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(ccmdt,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(ccmdt,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(ccmdt,'DD'),'31'," + fldvalue + ",0) as DAY_31 from wb_ccm_log where branchcd='" + frm_mbr + "' and type like 'CC%' AND  ccmdt " + DateRange + ") a group by A.acode order by a.acode";
                break;


            case "F05113": // PLANT MONTH WISE SALE
                fldvalue = "round(iamount/100000,2)";
                SQuery = "select a.branchcd as fstr, a.branchcd,c.name as branch_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from (select branchcd,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd != 'DD' and  type like '4%' and type!='47' AND VCHDATE " + DateRange + " ) a,type c where trim(a.branchcd)=trim(c.type1) and c.id='B' group by c.name, a.branchcd order by branchcd";
                break;

            case "F05112": // CUSTOMER MONTH WISE SALE
                //fldvalue = "(to_char(round(iamount/100000,2),'"+coma_sepr+"')";
                fldvalue = "round(iamount/100000,2)";
                SQuery = "select a.acode as fstr,trim(a.acode) as party_code, trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar from (select acode,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND VCHDATE " + DateRange + " ) a, famst b  where trim(a.acode)=trim(b.acode) group by b.aname,a.acode order by party_name";
                break;

            case "F05103": // PLANT DAY WISE SALE
            case "F05118":
                fldvalue = "round(iamount/100000,2)";
                SQuery = "SELECT a.branchcd as fstr, a.branchcd, d.name as branch_name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher  where  branchcd <> 'DD' and  type like '4%' and type!='47' AND vchdate " + DateRange + ") a,type d WHERE TRIM(A.branchcd)=TRIM(D.type1) and d.id='B' group by a.branchcd, d.name order by branch_name";
                break;

            case "F05114": // ITEM MONTH WISE SALE qty
                SQuery = "select a.icode as fstr,trim(a.icode) as Item_code, trim(b.iname) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select icode,(case when to_char(VCHDATE,'mm')='04' then iqtyout else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyout else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyout else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyout else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyout else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyout else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyout else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyout else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyout else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyout else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  iqtyout else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyout else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND VCHDATE " + DateRange + " ) a, item b  where trim(a.icode)=trim(b.icode)  group by b.iname,a.icode order by Item_name";
                break;
            case "F05120": // ITEM MONTH WISE SALE value
                SQuery = "select a.icode as fstr,trim(a.icode) as Item_code, trim(b.iname) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select icode,(case when to_char(VCHDATE,'mm')='04' then iamount else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iamount else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iamount else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iamount else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iamount else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iamount else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iamount else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iamount else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iamount else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iamount else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  iamount else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iamount else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND VCHDATE " + DateRange + " ) a, item b  where trim(a.icode)=trim(b.icode)  group by b.iname,a.icode order by Item_name";
                break;

            case "F05115": // ITEM DAY WISE SALE 
                SQuery = "SELECT trim(a.icode) as fstr,A.iCODE as code, b.iname as Item_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ICODE,decode(TO_CHAR(VCHDATE,'DD'),'01',iqtyout,0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02',iqtyout,0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03',iqtyout,0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04',iqtyout,0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05',iqtyout,0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06',iqtyout,0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07',iqtyout,0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08',iqtyout,0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09',iqtyout,0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10',iqtyout,0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11',iqtyout,0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12',iqtyout,0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13',iqtyout,0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14',iqtyout,0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15',iqtyout,0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16',iqtyout,0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17',iqtyout,0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18',iqtyout,0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19',iqtyout,0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20',iqtyout,0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21',iqtyout,0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22',iqtyout,0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23',iqtyout,0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24',iqtyout,0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25',iqtyout,0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26',iqtyout,0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27',iqtyout,0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28',iqtyout,0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29',iqtyout,0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30',iqtyout,0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31',iqtyout,0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND vchdate " + DateRange + ") a,item b WHERE TRIM(A.ICODE)=TRIM(B.ICODE) group by A.iCODE, b.iname order by Item_name";
                break;
            #endregion

            #region Purchase MIS
            case "F05151": // VENDOR DAY WISE PURCHASE 
                SQuery = "SELECT A.ACODE as fstr,A.ACODE as Code,d.aname as Vendor,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ACODE,decode(TO_CHAR(VCHDATE,'DD'),'01',iqtyin,0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02',iqtyin,0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03',iqtyin,0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04',iqtyin,0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05',iqtyin,0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06',iqtyin,0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07',iqtyin,0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08',iqtyin,0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09',iqtyin,0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10',iqtyin,0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11',iqtyin,0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12',iqtyin,0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13',iqtyin,0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14',iqtyin,0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15',iqtyin,0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16',iqtyin,0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17',iqtyin,0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18',iqtyin,0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19',iqtyin,0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20',iqtyin,0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21',iqtyin,0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22',iqtyin,0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23',iqtyin,0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24',iqtyin,0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25',iqtyin,0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26',iqtyin,0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27',iqtyin,0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28',iqtyin,0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29',iqtyin,0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30',iqtyin,0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31',iqtyin,0) as DAY_31 from ivoucher  where branchcd='" + frm_mbr + "' and type like '0%' and type!='04' AND vchdate " + DateRange + ") a,famst D WHERE TRIM(A.ACODE)=TRIM(D.ACODE) group by A.ACODE,d.aname order by Vendor";
                break;

            case "F05152":  // VENDOR MONTH WISE PURCHSE
                SQuery = "select distinct A.acode AS FSTR,trim(a.acode) as party_code,trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select acode,(case when to_char(VCHDATE,'mm')='04' then iqtyin else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyin else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyin else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyin else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyin  else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyin else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyin  else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyin else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyin else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyin  else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then iqtyin  else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyin else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type!='04' AND VCHDATE " + DateRange + " ) a, famst b  where trim(a.acode)=trim(b.acode)  group by b.aname,a.acode order by party_name";
                break;

            case "F05153": // PLANT DAY WISE PURCHASE
                SQuery = "SELECT a.branchcd as fstr, a.branchcd,d.name as branch_name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01',iqtyin,0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02',iqtyin,0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03',iqtyin,0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04',iqtyin,0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05',iqtyin,0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06',iqtyin,0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07',iqtyin,0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08',iqtyin,0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09',iqtyin,0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10',iqtyin,0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11',iqtyin,0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12',iqtyin,0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13',iqtyin,0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14',iqtyin,0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15',iqtyin,0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16',iqtyin,0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17',iqtyin,0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18',iqtyin,0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19',iqtyin,0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20',iqtyin,0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21',iqtyin,0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22',iqtyin,0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23',iqtyin,0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24',iqtyin,0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25',iqtyin,0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26',iqtyin,0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27',iqtyin,0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28',iqtyin,0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29',iqtyin,0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30',iqtyin,0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31',iqtyin,0) as DAY_31 from ivoucher  where branchcd<>'DD' and type like '0%' and type!='04' AND  vchdate " + DateRange + ") a,type d WHERE trim(a.branchcd)=trim(d.type1) and d.id='B' group by a.branchcd,d.name order by branch_name";
                break;

            case "F05154": // PLANT MONTH WISE PURCHASE
                SQuery = "select a.branchcd as fstr, a.branchcd,c.name as branch_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select branchcd,(case when to_char(VCHDATE,'mm')='04' then iqtyin else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyin else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyin else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyin else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyin else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyin else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyin else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyin else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyin else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyin else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  iqtyin else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyin else 0 end ) as mar from ivoucher where branchcd <> 'DD' and  type like '0%' and type!='04' AND VCHDATE " + DateRange + " ) a,type c where trim(a.branchcd)=trim(c.type1) and c.id='B'  group by c.name,a.branchcd order by branch_name";
                break;

            case "F05155": // ITEM DAY WISE PURCHASE
                SQuery = "Select A.iCODE as fstr,A.iCODE as code, b.iname as Item_Name,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ICODE,decode(TO_CHAR(VCHDATE,'DD'),'01',iqtyin,0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02',iqtyin,0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03',iqtyin,0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04',iqtyin,0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05',iqtyin,0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06',iqtyin,0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07',iqtyin,0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08',iqtyin,0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09',iqtyin,0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10',iqtyin,0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11',iqtyin,0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12',iqtyin,0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13',iqtyin,0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14',iqtyin,0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15',iqtyin,0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16',iqtyin,0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17',iqtyin,0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18',iqtyin,0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19',iqtyin,0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20',iqtyin,0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21',iqtyin,0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22',iqtyin,0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23',iqtyin,0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24',iqtyin,0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25',iqtyin,0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26',iqtyin,0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27',iqtyin,0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28',iqtyin,0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29',iqtyin,0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30',iqtyin,0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31',iqtyin,0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type!='04' AND vchdate " + DateRange + ") a,item b WHERE TRIM(A.ICODE)=TRIM(B.ICODE) group by A.iCODE, b.iname order by Item_Name";
                break;

            case "F05156": // ITEM MONTH WISE PURCHASE
                SQuery = "select distinct A.Icode AS FSTR,trim(a.Icode) as iTEM_code,trim(b.Iname) as ITEM_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select icode,(case when to_char(VCHDATE,'mm')='04' then iqtyin else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyin else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyin else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyin else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyin  else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyin else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyin  else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyin else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyin else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyin  else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then iqtyin  else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyin else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type!='04' AND VCHDATE " + DateRange + ") a, ITEM b  where trim(a.Icode)=trim(b.Icode)  group by b.Iname,a.Icode order by ITEM_name";
                break;
            #endregion




            #region Store MIS Inward
            case "F05168": // ITEM WISE(DAY) IWARD
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyin";
                }
                else
                {
                    fldvalue = "iamount";
                }
                SQuery = "SELECT trim(a.icode) as fstr,A.iCODE as code, b.iname as Item_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ICODE,BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type not in ('04','08') AND vchdate " + DateRange + ") a,item b where trim(a.icode)=trim(b.icode) group by a.icode, b.iname order by item_name";
                break;

            case "F05169": //ITEM WISE(MONTH) IWARD
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyin";
                }
                else
                {
                    fldvalue = "iamount";
                }
                SQuery = "select a.icode as fstr,trim(a.icode) as Item_code, trim(b.iname) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select icode,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type not in ('04','08') AND VCHDATE " + DateRange + " ) a, item b  where trim(a.icode)=trim(b.icode)  group by b.iname,a.icode order by Item_name";
                break;

            case "F05170": // PARTY WISE(DAY) IWARD
                fldvalue = "iamount";
                SQuery = "SELECT trim(a.acode) as fstr,A.acode as code, b.aname as Party_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ACODE,BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type not in ('04','08') AND vchdate " + DateRange + ") a,famst b where trim(a.acode)=trim(b.acode) group by a.acode, b.aname order by party_name";
                break;

            case "F05171": //PARTY WISE(MONTH) IWARD
                fldvalue = "iamount";
                SQuery = "select a.acode as fstr,trim(a.acode) as party_code, trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select acode,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type not in ('04','08') AND VCHDATE " + DateRange + " ) a, famst b  where trim(a.acode)=trim(b.acode)  group by b.aname,a.acode order by party_name";
                break;

            case "F05172": //PLANT WISE(DAY) IWARD
                fldvalue = "iamount";
                SQuery = "SELECT trim(a.branchcd) as fstr,A.branchcd as code, b.name as branch_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd!='DD' and type like '0%' and type not in ('04','08') AND vchdate " + DateRange + ") a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.branchcd,b.name order by code";
                break;

            case "F05173": //PLANT WISE(MONTH) IWARD
                fldvalue = "iamount";
                SQuery = "select a.branchcd as fstr,trim(a.branchcd) as code, trim(b.name) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select branchcd,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd!='DD' and type like '0%' and type not in ('04','08') AND vchdate " + DateRange + ") a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.branchcd,b.name order by code";
                break;
            #endregion

            #region Store MIS Outward
            case "F05178": // ITEM WISE(DAY) OUTWARD
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyout";
                }
                else
                {
                    fldvalue = "iamount";
                }
                SQuery = "SELECT trim(a.icode) as fstr,A.iCODE as code, b.iname as Item_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ICODE,BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and vchdate " + DateRange + ") a,item b where trim(a.icode)=trim(b.icode) group by a.icode, b.iname order by item_name";
                break;

            case "F05179": // ITEM WISE(MONTH) OUTWARD
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyout";
                }
                else
                {
                    fldvalue = "iamount";
                }
                SQuery = "select a.icode as fstr,trim(a.icode) as Item_code, trim(b.iname) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select icode,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and VCHDATE " + DateRange + " ) a, item b  where trim(a.icode)=trim(b.icode)  group by b.iname,a.icode order by Item_name";
                break;

            case "F05180": // PARTY WISE(DAY) Outward
                fldvalue = "iamount";
                SQuery = "SELECT trim(a.acode) as fstr,A.acode as code, b.aname as Party_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ACODE,BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and vchdate " + DateRange + ") a,famst b where trim(a.acode)=trim(b.acode) group by a.acode, b.aname order by party_name";
                break;

            case "F05181": // PARTY WISE(MONTH) OUTWARD
                fldvalue = "iamount";
                SQuery = "select a.acode as fstr,trim(a.acode) as party_code, trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select acode,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and VCHDATE " + DateRange + " ) a, famst b  where trim(a.acode)=trim(b.acode)  group by b.aname,a.acode order by party_name";
                break;

            case "F05182"://PLANT WISE(DAY) OUTWARD
                fldvalue = "iamount";
                SQuery = "SELECT trim(a.branchcd) as fstr,A.branchcd as code, b.name as branch_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd!='DD' and type like '2%' and vchdate " + DateRange + ") a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.branchcd,b.name order by code";
                break;

            case "F05183"://PLANT WISE(MONTH) OUTWARD
                fldvalue = "iamount";
                SQuery = "select a.branchcd as fstr,trim(a.branchcd) as code, trim(b.name) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select branchcd,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd!='DD' and type like '2%' and vchdate " + DateRange + ") a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.branchcd,b.name order by code";
                break;
            #endregion

            #region Store MIS Issuance
            //case "M13": // ITEM WISE(DAY) OUTWARD
            //    if (hf1.Value == "Q")
            //    {
            //        fldvalue = "iqtyout";
            //    }
            //    else
            //    {
            //        fldvalue = "iamount";
            //    }
            //    SQuery = "SELECT trim(a.icode) as fstr,A.iCODE as code, b.iname as Item_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ICODE,BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and vchdate " + DateRange + ") a,item b where trim(a.icode)=trim(b.icode) group by a.icode, b.iname order by item_name";
            //    break;

            //case "M14": // ITEM WISE(MONTH) OUTWARD
            //    if (hf1.Value == "Q")
            //    {
            //        fldvalue = "iqtyout";
            //    }
            //    else
            //    {
            //        fldvalue = "iamount";
            //    }
            //    SQuery = "select a.icode as fstr,trim(a.icode) as Item_code, trim(b.iname) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select icode,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and VCHDATE " + DateRange + " ) a, item b  where trim(a.icode)=trim(b.icode)  group by b.iname,a.icode order by Item_name";
            //    break;

            //case "M15": // PARTY WISE(DAY) Outward
            //    fldvalue = "iamount";
            //    SQuery = "SELECT trim(a.acode) as fstr,A.acode as code, b.aname as Party_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT ACODE,BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and vchdate " + DateRange + ") a,famst b where trim(a.acode)=trim(b.acode) group by a.acode, b.aname order by party_name";
            //    break;

            //case "M16": // PARTY WISE(MONTH) OUTWARD
            //    fldvalue = "iamount";
            //    SQuery = "select a.acode as fstr,trim(a.acode) as party_code, trim(b.aname) as party_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select acode,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and VCHDATE " + DateRange + " ) a, famst b  where trim(a.acode)=trim(b.acode)  group by b.aname,a.acode order by party_name";
            //    break;

            //case "M17"://PLANT WISE(DAY) OUTWARD
            //    fldvalue = "iamount";
            //    SQuery = "SELECT trim(a.branchcd) as fstr,A.branchcd as code, b.name as branch_Name,sum(A.DAY_01)+sum(A.DAY_02)+sum(A.DAY_03)+sum(A.DAY_04)+sum(A.DAY_05)+sum(A.DAY_06)+sum(A.DAY_07)+sum(A.DAY_08)+sum(A.DAY_09)+sum(A.DAY_10)+sum(A.DAY_11)+sum(A.DAY_12)+sum(A.DAY_13)+sum(A.DAY_14)+sum(A.DAY_15)+sum(A.DAY_16)+sum(A.DAY_17)+sum(A.DAY_18)+sum(A.DAY_19)+sum(A.DAY_20)+sum(A.DAY_21)+sum(A.DAY_22)+sum(A.DAY_23)+sum(A.DAY_24)+sum(A.DAY_25)+sum(A.DAY_26)+sum(A.DAY_27)+sum(A.DAY_28)+sum(A.DAY_29)+sum(A.DAY_30)+sum(A.DAY_31) as total,sum(A.DAY_01) as DAY_01,sum(A.DAY_02) as DAY_02,sum(A.DAY_03) as DAY_03,sum(A.DAY_04) as DAY_04,sum(A.DAY_05) as DAY_05,sum(A.DAY_06) as DAY_06,sum(A.DAY_07) as DAY_07,sum(A.DAY_08) as DAY_08,sum(A.DAY_09) as DAY_09,sum(A.DAY_10) as DAY_10,sum(A.DAY_11) as DAY_11,sum(A.DAY_12) as DAY_12,sum(A.DAY_13) as DAY_13,sum(A.DAY_14) as DAY_14,sum(A.DAY_15) as DAY_15,sum(A.DAY_16) as DAY_16,sum(A.DAY_17) as DAY_17,sum(A.DAY_18) as DAY_18,sum(A.DAY_19) as DAY_19,sum(A.DAY_20) as DAY_20,sum(A.DAY_21) as DAY_21,sum(A.DAY_22) as DAY_22,sum(A.DAY_23) as DAY_23,sum(A.DAY_24) as DAY_24,sum(A.DAY_25) as DAY_25,sum(A.DAY_26) as DAY_26,sum(A.DAY_27) as DAY_27,sum(A.DAY_28) as DAY_28,sum(A.DAY_29) as DAY_29,sum(A.DAY_30) as DAY_30,sum(A.DAY_31) as DAY_31 FROM (SELECT BRANCHCD,decode(TO_CHAR(VCHDATE,'DD'),'01'," + fldvalue + ",0) as DAY_01,decode(TO_CHAR(VCHDATE,'DD'),'02'," + fldvalue + ",0) as DAY_02,decode(TO_CHAR(VCHDATE,'DD'),'03'," + fldvalue + ",0) as DAY_03,decode(TO_CHAR(VCHDATE,'DD'),'04'," + fldvalue + ",0) as DAY_04,decode(TO_CHAR(VCHDATE,'DD'),'05'," + fldvalue + ",0) as DAY_05,decode(TO_CHAR(VCHDATE,'DD'),'06'," + fldvalue + ",0) as DAY_06,decode(TO_CHAR(VCHDATE,'DD'),'07'," + fldvalue + ",0) as DAY_07,decode(TO_CHAR(VCHDATE,'DD'),'08'," + fldvalue + ",0) as DAY_08,decode(TO_CHAR(VCHDATE,'DD'),'09'," + fldvalue + ",0) as DAY_09,decode(TO_CHAR(VCHDATE,'DD'),'10'," + fldvalue + ",0) as DAY_10,decode(TO_CHAR(VCHDATE,'DD'),'11'," + fldvalue + ",0) as DAY_11,decode(TO_CHAR(VCHDATE,'DD'),'12'," + fldvalue + ",0) as DAY_12,decode(TO_CHAR(VCHDATE,'DD'),'13'," + fldvalue + ",0) as DAY_13,decode(TO_CHAR(VCHDATE,'DD'),'14'," + fldvalue + ",0) as DAY_14,decode(TO_CHAR(VCHDATE,'DD'),'15'," + fldvalue + ",0) as DAY_15,decode(TO_CHAR(VCHDATE,'DD'),'16'," + fldvalue + ",0) as DAY_16,decode(TO_CHAR(VCHDATE,'DD'),'17'," + fldvalue + ",0) as DAY_17,decode(TO_CHAR(VCHDATE,'DD'),'18'," + fldvalue + ",0) as DAY_18,decode(TO_CHAR(VCHDATE,'DD'),'19'," + fldvalue + ",0) as DAY_19,decode(TO_CHAR(VCHDATE,'DD'),'20'," + fldvalue + ",0) as DAY_20,decode(TO_CHAR(VCHDATE,'DD'),'21'," + fldvalue + ",0) as DAY_21,decode(TO_CHAR(VCHDATE,'DD'),'22'," + fldvalue + ",0) as DAY_22,decode(TO_CHAR(VCHDATE,'DD'),'23'," + fldvalue + ",0) as DAY_23,decode(TO_CHAR(VCHDATE,'DD'),'24'," + fldvalue + ",0) as DAY_24,decode(TO_CHAR(VCHDATE,'DD'),'25'," + fldvalue + ",0) as DAY_25,decode(TO_CHAR(VCHDATE,'DD'),'26'," + fldvalue + ",0) as DAY_26,decode(TO_CHAR(VCHDATE,'DD'),'27'," + fldvalue + ",0) as DAY_27,decode(TO_CHAR(VCHDATE,'DD'),'28'," + fldvalue + ",0) as DAY_28,decode(TO_CHAR(VCHDATE,'DD'),'29'," + fldvalue + ",0) as DAY_29,decode(TO_CHAR(VCHDATE,'DD'),'30'," + fldvalue + ",0) as DAY_30,decode(TO_CHAR(VCHDATE,'DD'),'31'," + fldvalue + ",0) as DAY_31 from ivoucher where branchcd!='DD' and type like '2%' and vchdate " + DateRange + ") a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.branchcd,b.name order by code";
            //    break;

            //case "M18"://PLANT WISE(MONTH) OUTWARD
            //    fldvalue = "iamount";
            //    SQuery = "select a.branchcd as fstr,trim(a.branchcd) as code, trim(b.name) as Item_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select branchcd,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd!='DD' and type like '2%' and vchdate " + DateRange + ") a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.branchcd,b.name order by code";
            //    break;
            #endregion
        }
        if (SQuery.Length > 1)
        {
            // div1.Visible = true;
            lblDate.Text = "For the Period : " + fromdt + " To " + todt;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_JOBQUERY", SQuery);
            sg1_dt = new DataTable();
            sg1_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            #region DownTime And Rejection For Corrugation Industry
            if (frm_formID == "F05143" || frm_formID == "F05144")
            {
                if (sg1_dt.Rows.Count > 0)
                {
                    sg1_dr = sg1_dt.NewRow();
                    foreach (DataColumn dc in sg1_dt.Columns)
                    {
                        db1 = 0;
                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2)
                        { }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(sg1_dt.Compute(mq1, "").ToString());
                            sg1_dr[dc] = db1;
                        }
                    }
                    sg1_dr[2] = "TOTAL";
                    sg1_dt.Rows.InsertAt(sg1_dr, 0);
                }
            }
            #endregion

            Rej_Plastic_Moulding();
            Down_Plastic_Moulding();

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            ViewState["sg1"] = sg1_dt;
            lblTotcount.Text = "Total Rows : " + sg1_dt.Rows.Count;
            lblPageCount.Text = "   -  Now Displaying : " + sg1.Rows.Count;
            int x = 0;
            x = fgen.make_int(Session["hfWindowSize"].ToString());
            if (x == 0) x = 1000;

            int colFound = sg1_dt.Columns.Count;

            double gridFoundWidth = Math.Round(x * .95);
            string sepWidth = Math.Round((gridFoundWidth / (colFound - 1)) * 1).ToString();
            int multp = 3;
            string restWidth = ((Convert.ToInt32(sepWidth)) / (colFound - multp)).ToString();

            for (int i = 2; i <= colFound; i++)
            {
                if (i == 3) sg1.HeaderRow.Cells[i].Width = (Convert.ToInt16(sepWidth) * multp);
                else sg1.HeaderRow.Cells[i].Width = (Convert.ToInt16(sepWidth) - Convert.ToInt16(restWidth));
            }
        }
        btnsave.Disabled = false;
        setGridWidth();
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            z = 0;
            for (int i = z; i < e.Row.Cells.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.ToolTip = "You can double click this cell";
                cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }

            // COLUMN ALINGMENT
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                int CheckDataType = System.Text.RegularExpressions.Regex.Matches(e.Row.Cells[i].Text, @"[a-zA-Z]").Count;
                if (CheckDataType != 0)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                }
                else
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                }
            }

            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";
            if (sg1_dt.Rows.Count > 0)
            {
                sg1.Columns[0].HeaderStyle.CssClass = "hidden";
                e.Row.Cells[0].CssClass = "hidden";
            }
            e.Row.Cells[1].CssClass = "hidden";
            sg1.HeaderRow.Cells[1].CssClass = "hidden";
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------        
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow row = sg1.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (selectedCellIndex < 0) selectedCellIndex = 0;
        mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading
        if (selectedCellIndex > 0) selectedCellIndex -= 1;
        mq1 = row.Cells[1].Text.Trim();
        mq2 = row.Cells[3].Text.Trim();
        mq3 = "";
        vartype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MLD_PTYPE");
        chk_indust = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(Trim(opt_param)) as opt from fin_Rsys_opt where trim(opt_id)='W0000' ", "opt");
        //vartype = "90";
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        switch (frm_formID)
        {
            #region Account MIS
            case "F05126":
                hffield.Value = "F05126";
                header_n = "Invoice Details Of " + mq2;
                SQuery = "Select trim(invno)||to_char(a.invdate,'dd/mm/yyyy')||trim(acode) as fstr, invno as Invoice,to_char(a.invdate,'dd/mm/yyyy') as Dated,to_char(a.Dramt,'999999999.99') as Debit,to_char(a.cramt,'999999999.99') as Credits,to_char(a.dramt-a.cramt,'999999999.99') as Balance,' ' as cumu from recdataW a where trim(a.acode)='" + mq1 + "' order by a.invdate,a.invno";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05127":
                hffield.Value = "F05127";
                header_n = "Details Of " + mq2;
                SQuery = "Select trim(invno)||to_char(a.invdate,'dd/mm/yyyy')||trim(acode) as fstr,invno as Invoice,to_char(a.invdate,'dd/mm/yyyy') as Dated,to_char(a.Dramt,'999999999.99') as Debit,to_char(a.cramt,'999999999.99') as Credits,to_char(a.dramt-a.cramt,'999999999.99') as Balance,' ' as cumu from recdataW a where trim(a.acode)='" + mq1 + "' order by a.invdate,a.invno";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "A3":
                header_n = "Sale Record for the Item Code : " + mq1 + " and Item Name : " + mq2;
                SQuery = "Select A.iCODE as fstr,B.ANAME as party_name , A.aCODE as Party_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as Quantity  FROM IVOUCHER A , FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '4%' AND a.icode='" + mq1 + "' and a.vchdate " + DateRange + " order by vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "A4":
                header_n = "Purchase Record for the Item Code : " + mq1 + " Item Name : " + mq2;
                SQuery = "Select A.ICODE as fstr,B.ANAME as party_name , A.ACODE as PARTY_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as Quantity  FROM IVOUCHER A , FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '0%' AND A.ICODE='" + mq1 + "' and a.vchdate " + DateRange + " and a.store!='R' order by vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;
            #endregion

            #region Production MIS
            case "F05143":
                header_n = "Downtime Details For " + mq2 + " (" + mq1 + ")";
                SQuery = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.obsv15 as shift,a.title as machine,a.col3 as minutes,a.col4 as fromtime,a.col5 as totime,to_char(a.vchdate,'yyyymmdd') as vdd from inspvch a ,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='55' and a.col2='" + mq1 + "' and a.vchdate " + DateRange + " order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05144":
                header_n = "Rejection Details For " + mq2 + " (" + mq1 + ")";
                SQuery = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.obsv15 as shift,a.title as machine,a.col3 as qty,to_char(a.vchdate,'yyyymmdd') as vdd from inspvch a ,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and  a.type='45' and a.col2='" + mq1 + "' and a.vchdate " + DateRange + " order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05145":
                header_n = "Month Wise Rejection Details For The Month Of : " + row.Cells[2].Text.Trim();
                SQuery = "SELECT a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.mchcode,a.ename,a.var_code,a.iqtyin as ok_qty,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and to_char(a.vchdate,'mm/yyyy')='" + mq1 + "' order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05146":
                header_n = "Item Wise Rejection Details For The Code : " + row.Cells[2].Text.Trim() + " Item Name : " + mq2 + " Month : " + row.Cells[4].Text.Trim();
                SQuery = "SELECT a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.mchcode,a.ename,a.var_code,a.iqtyin as ok_qty,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and TRIM(A.ICODE)||TO_CHAR(a.VCHDATE,'mm/yyyy')='" + mq1 + "' order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05147":
                header_n = "Machine Wise Rejection Details For The Code : " + row.Cells[2].Text.Trim() + " Machine : " + mq2 + " Month : " + row.Cells[4].Text.Trim();
                SQuery = "SELECT a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.mchcode,a.ename,a.var_code,a.iqtyin as ok_qty,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and TRIM(A.MCHCODE)||TO_CHAR(a.VCHDATE,'mm/yyyy')='" + mq1 + "' order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05148":
                header_n = "Shift Wise Rejection Details For The Code : " + row.Cells[2].Text.Trim() + " Machine : " + mq2 + " Month : " + row.Cells[4].Text.Trim();
                SQuery = "SELECT a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.mchcode,a.ename,a.var_code,a.iqtyin as ok_qty,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and TRIM(A.SHFTCODE)||TO_CHAR(a.VCHDATE,'mm/yyyy')='" + mq1 + "' order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05149":
                header_n = "Downtime Details For The Code : " + row.Cells[2].Text.Trim() + " Machine : " + mq2 + " Month : " + row.Cells[4].Text.Trim();
                SQuery = "SELECT a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.mchcode,a.ename,a.var_code,a.iqtyin as ok_qty,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and TRIM(A.MCHCODE)||TO_CHAR(a.VCHDATE,'mm/yyyy')='" + mq1 + "' order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;

            case "F05150":
                header_n = "Downtime Details For The Code : " + mq2 + " Machine : " + row.Cells[2].Text.Trim() + " Month : " + row.Cells[6].Text.Trim();
                SQuery = "SELECT a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,i.iname,a.mchcode,a.ename,a.var_code,a.iqtyin as ok_qty,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and trim(a.mchcode)||trim(a.icode)||to_char(a.vchdate,'mm/yyyy')='" + mq1 + "' order by vdd,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
                break;
            #endregion

            #region Sale MIS
            case "F05116":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select day_name,Amount from (Select to_Char(a.vchdate,'dd') as day_Name,round(sum(a.iamount/100000),2) as Amount,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' and trim(A.ACODE)='" + mq1 + "' and a.vchdate " + DateRange + " group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sale Chart(Fig in Lakhs)", "spline", "For the Party -" + row.Cells[2].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.acode as fstr,a.vchnum as voucher_no,to_char(a.vchdate,'dd/mm/yyyy') as voucher_date,a.type,a.icode as item_code,b.iname as item_name, a.iamount,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and trim(A.ACODE)='" + mq1 + "' and a.vchdate " + DateRange + " order by vdd,voucher_no";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Sale Details for (" + row.Cells[2].Text.Trim() + ") " + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;
            case "F05117":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select day_name,Amount from (Select to_Char(a.vchdate,'dd') as day_Name,round(sum(a.iamount/100000),2) as Amount,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' and trim(A.iCODE)='" + mq1 + "' and a.vchdate " + DateRange + " group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sale Chart(Fig in Lakhs)", "spline", "For the Item -" + row.Cells[2].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.Icode as fstr,a.vchnum as voucher_no,to_char(a.vchdate,'dd/mm/yyyy') as voucher_date,a.type,a.icode as item_code,b.iname as item_name, a.iamount,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and trim(A.ICODE)='" + mq1 + "' and a.vchdate " + DateRange + " order by vdd,voucher_no";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Sale Details for (" + row.Cells[2].Text.Trim() + ") " + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;
            case "F05119":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select day_name,Amount from (Select to_Char(a.vchdate,'dd') as day_Name,round(sum(a.iqtyout),2) as Qty,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' and trim(A.iCODE)='" + mq1 + "' and a.vchdate " + DateRange + " group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sale Qty Chart", "spline", "For the Item -" + row.Cells[2].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.Icode as fstr,a.vchnum as voucher_no,to_char(a.vchdate,'dd/mm/yyyy') as voucher_date,a.type,a.icode as item_code,b.iname as item_name, a.iamount,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and trim(A.ICODE)='" + mq1 + "' and a.vchdate " + DateRange + " order by vdd,voucher_no";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Sale Details for (" + row.Cells[2].Text.Trim() + ") " + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;

            case "F05113":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round(sum(a.iamount/100000),2) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sales Chart (Fig in Lakhs)", "spline", "For the Branch - " + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.ACODE as fstr,B.ANAME , A.ICODE as item_code ,C.INAME as Item_name,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.Iamount ,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A , FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Sale Details for the Branch : " + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;

            case "F05112":
            case "F05133":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select Month_name,Amount from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(round(a.iamount/100000,2)) as Amount ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " AND a.ACODE='" + mq1 + "' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM') ) order by Mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sales Chart (Fig in Lakhs)", "spline", "For the Party -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                    //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Inward Quality Rejn Trend", "line", "Month Wise", "Incoming Rejection %", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.ACODE as fstr,A.ICODE as item_code ,C.INAME as Item_name,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as Quantity,a.iamount,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,ITEM C WHERE TRIM(A.ICODE)=TRIM(C.ICODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " AND a.ACODE='" + mq1 + "' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Sale Record for the Party : " + row.Cells[2].Text.Trim() + " Party Name : " + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;

            case "F05118":
            case "F05103": //plant wise sale day wise in web already exists icon
                if (selectedCellIndex == 1)
                {
                    SQuery = "select Day_name,Amt as Amount from (Select to_Char(a.vchdate,'dd') as day_Name,round(sum(a.iamount/100000)) as amt ,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd') ) order by dayz";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sales Chart (Fig in Lakhs)", "spline", "For the Branch-" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.vchnum as fstr,a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Date,c.aname as Party, b.iname as Item_name,a.iqtyin,b.unit,a.irate,a.iamount,b.cpartno,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + mq1 + "' and a.type like '4%' and a.type!='47' and a.vchdate " + DateRange + " order by vdd,a.vchnum";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Total Purchase of Plant :" + row.Cells[3].Text.Trim() + " (" + mq1 + ")", frm_qstr);
                }
                break;

            case "F05114":
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iqtyout) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' AND a.iCODE='" + mq1 + "' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sale Chart(Qty wise)", "spline", "For the Item-" + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim() + " ", "", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.iCODE as fstr,B.ANAME as party_name,A.aCODE as Party_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as Quantity,to_char(a.vchdate,'yyyymmdd') as vdd  FROM IVOUCHER A , FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' AND a.icode='" + mq1 + "' and a.vchdate " + DateRange + " order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Qty sold - Item : " + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim(), frm_qstr);
                }
                break;

            case "F05120":
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iamount) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' AND a.iCODE='" + mq1 + "' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sale Chart(Qty wise)", "spline", "For the Item-" + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim() + " ", "", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.iCODE as fstr,B.ANAME as party_name,A.aCODE as Party_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iamount as Quantity,to_char(a.vchdate,'yyyymmdd') as vdd  FROM IVOUCHER A , FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' AND a.icode='" + mq1 + "' and a.vchdate " + DateRange + " order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Qty sold - Item : " + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim(), frm_qstr);
                }
                break;

            case "F05115":
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Day_name,Quantity from (Select to_Char(a.vchdate,'dd') as day_Name,sum(a.iqtyout) as Quantity ,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " and a.icode='" + mq1 + "' group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd') ) order by dayz";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sales Chart(Qty wise)", "spline", "For the Item -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.iCODE as fstr,B.ANAME as party_name,A.aCODE as Party_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as Quantity,to_char(a.vchdate,'yyyymmdd') as vdd  FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " AND a.icode='" + mq1 + "' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Qty sold - Item : " + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim(), frm_qstr);
                }
                break;
            #endregion

            #region Purchase MIS
            case "F05151":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select day_name,Quantity from (Select to_Char(a.vchdate,'dd') as day_Name,sum(a.iqtyin) as Quantity ,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type!='04' and a.vchdate " + DateRange + " and a.acode ='" + mq1 + "' and a.store!='R' group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Purchase Chart", "line", "For the Party-" + row.Cells[1].Text.Trim() + "-" + row.Cells[3].Text.Trim() + " ", "For Month- " + row.Cells[4].Text.Trim() + "/ " + row.Cells[5].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.vchnum as fstr,a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Date,c.aname as Party, b.iname as Item_name,a.iqtyin,b.unit,a.irate,a.iamount,b.cpartno,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd from  ivoucher a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type!='04' and a.vchdate " + DateRange + " and a.acode ='" + mq1 + "' and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Purchase Detail For the Party " + row.Cells[1].Text.Trim() + " -" + row.Cells[3].Text.Trim() + "", frm_qstr);
                }
                break;

            case "F05152":
            case "F05134":
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iqtyin) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type!='04' AND a.ACODE='" + mq1 + "' and a.vchdate " + DateRange + " and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM') order by Month_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Purchase Chart", "line", "For the Party -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "Select a.acode as fstr,a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Date,c.aname as Party, b.iname as Item_name,a.iqtyin,b.unit,a.irate,a.iamount,b.cpartno,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,FAMST c,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND TRIM(A.aCODE)=TRIM(c.aCODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type!='04' AND a.ACODE='" + mq1 + "' and a.vchdate " + DateRange + " and a.store!='R' order by vdd,vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Purchase Record for the Code : " + row.Cells[2].Text.Trim() + " Party Name : " + row.Cells[2].Text.Trim() + " ", frm_qstr);
                }
                break;

            case "F05153":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select day_name,Quantity from (Select to_Char(a.vchdate,'dd') as day_Name,sum(a.iqtyin) as Quantity ,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '0%' and a.type!='04' and a.vchdate " + DateRange + " and a.store!='R' group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Purchase Chart", "line", "For the Branch-" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.branchcd as fstr,a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Date,c.aname as Party, b.iname as Item_name,a.iqtyin,b.unit,a.irate,a.iamount,b.cpartno,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + mq1 + "' and a.type like '0%' and a.type!='04' and a.vchdate " + DateRange + " and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Total Purchase of Plant :" + row.Cells[1].Text.Trim() + " -" + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;

            case "F05154":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iqtyin) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '0%' and a.type!='04' and a.vchdate " + DateRange + " and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Purchase Chart", "line", "For the Branch-" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                    return;
                }
                else
                {
                    SQuery = "Select A.ACODE as fstr,B.ANAME , C.INAME as Item_name,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as Quantity,a.irate,A.ICODE as item_code ,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A , FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '0%' and a.type!='04' and a.vchdate " + DateRange + "  and a.store!='R'  order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Purchase Record for the Branch : " + row.Cells[2].Text.Trim() + " Branch Name :" + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;

            case "F05155":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select Day_name,Quantity from (Select to_Char(a.vchdate,'dd') as day_Name,sum(a.iqtyin) as Quantity ,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type!='04' and a.vchdate " + DateRange + " and a.icode ='" + mq1 + "'  and a.store!='R' group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Purchase Chart", "line", "For the Item-" + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim() + " ", "For Month- " + row.Cells[4].Text.Trim() + "/" + row.Cells[5].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.icode as fstr,a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Date,c.aname as Party, b.iname as Item_name,a.iqtyin,b.unit,a.irate,a.iamount,b.cpartno,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type!='04' and a.vchdate " + DateRange + " and a.icode='" + mq1 + "'  and a.store!='R'  order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Purchase Detail Of Code : " + row.Cells[1].Text.Trim() + " Name : " + row.Cells[3].Text.Trim(), frm_qstr);
                }
                break;

            case "F05156":
                if (selectedCellIndex == 1)
                {
                    SQuery = "select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iqtyin) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' AND a.iCODE='" + mq1 + "' and a.vchdate " + DateRange + "  and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Purchase Chart", "line", "For the Item-" + row.Cells[2].Text.Trim() + "-" + row.Cells[3].Text.Trim() + " ", "", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.ICODE as fstr,B.ANAME as party_name , A.ACODE as PARTY_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as Quantity,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A , FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' AND A.ICODE='" + mq1 + "' and a.vchdate " + DateRange + "  and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Purchase Record for the Code : " + row.Cells[2].Text.Trim() + " Name :" + row.Cells[3].Text.Trim() + " ", frm_qstr);
                }
                break;
            #endregion

            #region Store MIS Inward
            case "F05168":
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyin";
                    mq3 = "Inward Chart (Qty Wise)";
                    mq0 = "Qty";
                }
                else
                {
                    fldvalue = "iamount";
                    mq3 = "Inward Chart (Value Wise)";
                    mq0 = "Value";
                }
                if (selectedCellIndex == 1)
                {
                    SQuery = "select day_name,Quantity from (select to_char(a.vchdate,'dd') as day_name,sum(a." + fldvalue + ") as quantity ,to_char(a.vchdate,'dd') as dayz from ivoucher a where a.branchcd='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + " and a.icode='" + mq1 + "'  and a.store!='R' group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Item -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.icode as fstr,b.aname as party_name,a.acode as party_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd  from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' AND a.type like '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + " AND a.icode='" + mq1 + "'  and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Inward - Item : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05169":
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyin";
                    mq3 = "Inward Chart (Qty Wise)";
                    mq0 = "Qty";
                }
                else
                {
                    fldvalue = "iamount";
                    mq3 = "Inward Chart (Value Wise)";
                    mq0 = "Value";
                }
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + " and a.icode='" + mq1 + "'  and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Item - " + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.ACODE as fstr,a.acode,B.ANAME ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as quantity,a.Iamount as basic_val ,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + " and a.icode='" + mq1 + "'  and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Inward - Item : " + row.Cells[3].Text.Trim() + " (" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05170":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD='" + frm_mbr + "' AND type like '0%' and type not in ('04','08') and vchdate " + DateRange + " and acode='" + mq1 + "' group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Party -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.acode as fstr,b.iname as item_name,a.icode as item_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' AND a.type like '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + " AND a.acode='" + mq1 + "'  and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Inward - Party : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05171":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + " and a.acode='" + mq1 + "'  and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Party - " + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.acode as fstr,b.iname as item_name,a.icode as item_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' AND a.type like '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + " AND a.acode='" + mq1 + "'  and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Inward - Party : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05172":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD='" + mq1 + "' AND type like '0%' and type not in ('04','08') and vchdate " + DateRange + " group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Party -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.branchcd as fstr,b.iname as item_name,a.icode as item_code,a.acode as code,f.aname as name,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b,famst f where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(f.acode) and a.branchcd='" + mq1 + "' AND a.type like '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + "  and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Inward - Plant : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05173":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + "   and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Plant -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.branchcd as fstr,b.iname as item_name,a.icode as item_code,a.acode as code,f.aname as name,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyin as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b,famst f where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(f.acode) and a.branchcd='" + mq1 + "' AND a.type like '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + "  and a.store!='R' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Inward - Plant : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;
            #endregion

            #region Store MIS Outward
            case "F05178":
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyout";
                    mq3 = "Outward Chart (Qty Wise)";
                    mq0 = "Qty";
                }
                else
                {
                    fldvalue = "iamount";
                    mq3 = "Outward Chart (Value Wise)";
                    mq0 = "Value";
                }
                if (selectedCellIndex == 1)
                {
                    SQuery = "select day_name,Quantity from (select to_char(a.vchdate,'dd') as day_name,sum(a." + fldvalue + ") as quantity ,to_char(a.vchdate,'dd') as dayz from ivoucher a where a.branchcd='" + frm_mbr + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + " and a.icode='" + mq1 + "' group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Item -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.icode as fstr,b.aname as party_name,a.acode as party_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd  from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' AND a.type like '2%' and a.vchdate " + DateRange + " AND a.icode='" + mq1 + "' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Outward - Item : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05179":
                if (hf1.Value == "Q")
                {
                    fldvalue = "iqtyout";
                    mq3 = "Outward Chart (Qty Wise)";
                    mq0 = "Qty";
                }
                else
                {
                    fldvalue = "iamount";
                    mq3 = "Outward Chart (Value Wise)";
                    mq0 = "Value";
                }
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + " and a.icode='" + mq1 + "' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Item - " + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "Select A.ACODE as fstr,a.acode,B.ANAME ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as quantity,a.Iamount as basic_val ,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + " and a.icode='" + mq1 + "' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Outward - Item : " + row.Cells[3].Text.Trim() + " (" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05180":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD='" + frm_mbr + "' AND type like '2%' and vchdate " + DateRange + " and acode='" + mq1 + "' group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Party -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.acode as fstr,b.iname as item_name,a.icode as item_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' AND a.type like '2%' and a.vchdate " + DateRange + " AND a.acode='" + mq1 + "' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Outward - Party : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05181":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + " and a.acode='" + mq1 + "' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Party - " + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.acode as fstr,b.iname as item_name,a.icode as item_code ,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' AND a.type like '2%' and a.vchdate " + DateRange + " AND a.acode='" + mq1 + "' order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Outward - Party : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05182":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD='" + mq1 + "' AND type like '2%' and vchdate " + DateRange + " group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Party -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.branchcd as fstr,b.iname as item_name,a.icode as item_code,a.acode as code,f.aname as name,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b,famst f where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(f.acode) and a.branchcd='" + mq1 + "' AND a.type like '2%' and a.vchdate " + DateRange + " order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Outward - Plant : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;

            case "F05183":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                mq0 = "Value";
                if (selectedCellIndex == 1)
                {
                    SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD='" + mq1 + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + "  group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "spline", "For the Plant -" + row.Cells[1].Text.Trim() + " ", " " + row.Cells[3].Text.Trim() + " ", SQuery, "");
                }
                else
                {
                    SQuery = "select a.branchcd as fstr,b.iname as item_name,a.icode as item_code,a.acode as code,f.aname as name,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,a.iqtyout as quantity,a.iamount as basic_amt,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b,famst f where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(f.acode) and a.branchcd='" + mq1 + "' AND a.type like '2%'  and a.vchdate " + DateRange + " order by vdd,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(mq0 + " Outward - Plant : " + row.Cells[3].Text.Trim() + "(" + row.Cells[2].Text.Trim() + ")", frm_qstr);
                }
                break;
            #endregion
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0) e.Row.TabIndex = 1;
            else e.Row.TabIndex = 2;
            if (Convert.ToDouble(e.Row.RowIndex.ToString()) == 0) e.Row.Attributes["onfocus"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "if (event.keyCode != 13) { javascript:return SelectSibling(event); }";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        sg1.PageIndex = e.NewPageIndex;
        fillGrid();
    }
    //------------------------------------------------------------------------------------
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        SQuery = "";
        SQuery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_JOBQUERY");
        if (SQuery.Length > 2)
        {
            dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];

            sg1_dt = new DataTable();
            sg1_dt = search_vip1(frm_qstr, frm_cocd, SQuery, txtSearch.Text.Trim().ToUpper(), dt);

            Rej_Plastic_Moulding();
            Down_Plastic_Moulding();
            ViewState["sg1"] = sg1_dt;
            if (sg1_dt != null)
            {
                sg1.DataSource = sg1_dt;
                sg1.DataBind(); sg1_dt.Dispose();
                lblTotcount.Text = "Total Rows : " + sg1_dt.Rows.Count;
            }
            else
            {
                sg1.DataSource = null;
                sg1.DataBind();
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void Rej_Plastic_Moulding()
    {
        if (frm_formID == "F05145" || frm_formID == "F05146" || frm_formID == "F05147" || frm_formID == "F05148")
        {
            // FOR GIVING DYNAMIC HEADING
            dt1 = new DataTable();
            dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,substr(trim(name),0,10) as name FROM TYPEWIP WHERE ID='RJC61' order by code");
            z = 1;
            try
            {
                if (sg1_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        sg1_dt.Columns["A" + z].ColumnName = dt1.Rows[i]["name"].ToString().Replace(" ", "_").Replace("/", "").Replace("&", "").Replace("(", "").Replace(")", "").Replace("%", "").Replace("-", "").Replace(".", "");
                        z++;
                    }
                }
            }
            catch { }
            if (sg1_dt.Rows.Count > 0)
            {
                sg1_dr = sg1_dt.NewRow();
                foreach (DataColumn dc in sg1_dt.Columns)
                {
                    db1 = 0;
                    if (frm_formID == "F05145")
                    {

                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 25)
                        { }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(sg1_dt.Compute(mq1, "").ToString());
                            sg1_dr[dc] = db1;
                        }
                    }
                    else if (frm_formID == "F05146")
                    {
                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 27)
                        { }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(sg1_dt.Compute(mq1, "").ToString());
                            sg1_dr[dc] = db1;
                        }
                    }
                    else if (frm_formID == "F05147" || frm_formID == "F05148")
                    {
                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 28)
                        { }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(sg1_dt.Compute(mq1, "").ToString());
                            sg1_dr[dc] = db1;
                        }
                    }
                }
                if (frm_formID == "F05148")
                {
                    sg1_dr[2] = "TOTAL";
                }
                else
                {
                    sg1_dr[1] = "TOTAL";
                }
                sg1_dt.Rows.InsertAt(sg1_dr, 0);
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void Down_Plastic_Moulding()
    {
        if (frm_formID == "F05149" || frm_formID == "F05150")
        {
            // FOR GIVING DYNAMIC HEADING
            dt1 = new DataTable();
            dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,substr(trim(name),0,10) as name FROM TYPEWIP WHERE ID='DTC61' order by code");
            z = 1;
            try
            {
                if (sg1_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        sg1_dt.Columns["A" + z].ColumnName = dt1.Rows[i]["name"].ToString().Replace(" ", "_").Replace("/", "").Replace("&", "").Replace("(", "").Replace(")", "").Replace("%", "").Replace("-", "").Replace(".", "");
                        z++;
                    }
                }
            }
            catch { }
            if (sg1_dt.Rows.Count > 0)
            {
                sg1_dr = sg1_dt.NewRow();
                foreach (DataColumn dc in sg1_dt.Columns)
                {
                    db1 = 0;
                    if (frm_formID == "F05149")
                    {
                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 17)
                        { }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(sg1_dt.Compute(mq1, "").ToString());
                            sg1_dr[dc] = db1;
                        }
                    }
                    else if (frm_formID == "F05150")
                    {
                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 20)
                        { }
                        else
                        {
                            mq1 = "sum(" + dc.ColumnName + ")";
                            db1 += fgen.make_double(sg1_dt.Compute(mq1, "").ToString());
                            sg1_dr[dc] = db1;
                        }
                    }
                }
                sg1_dr[2] = "TOTAL";
                sg1_dt.Rows.InsertAt(sg1_dr, 0);
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_Sorting(object sender, GridViewSortEventArgs e)
    {
        //  this.BindGrid(e.SortExpression);
        sg1_dt = new DataTable();
        sg1_dt = (DataTable)ViewState["sg1"];
        DataView dv = sg1_dt.AsDataView();
        this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
        dv.Sort = e.SortExpression + " " + this.SortDirection;
        sg1.DataSource = dv;
        sg1.DataBind();
    }
    //------------------------------------------------------------------------------------
    private string SortDirection
    {
        get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
        set { ViewState["SortDirection"] = value; }
    }
    //------------------------------------------------------------------------------------
    public DataTable search_vip1(string Qstr, string co_Cd, string Query, string SearchText, DataTable dt_SEARCH)
    {
        string mq0 = "";
        DataTable vdt = new DataTable();
        vdt = null;
        try
        {
            if (dt_SEARCH.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt_SEARCH.Columns)
                {
                    if (mq0.Length > 0) mq0 = mq0 + "||" + dc.ToString();
                    else mq0 = dc.ToString();
                }
            }
            else
            {
                DataTable dt_srch_vp = new DataTable();
                dt_srch_vp = fgen.getdata(frm_qstr, frm_cocd, "Select * from ( " + Query + " ) where rownum<3");

                foreach (DataColumn dc in dt_srch_vp.Columns)
                {
                    if (mq0.Length > 0) mq0 = mq0 + "||" + dc.ToString();
                    else mq0 = dc.ToString();
                }
            }
            vdt = fgen.getdata(frm_qstr, frm_cocd, "Select * from ( " + Query + " ) where upper(trim(" + mq0 + ")) like '%" + SearchText.Trim().ToUpper() + "%'");
        }
        catch (Exception ex)
        {
            //FILL_ERR("In Search String :=> " + ex.Message);
        }
        return vdt;
    }
    //------------------------------------------------------------------------------------
    protected void Graph_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "G";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    //-------------------------------------------------------------
    void fillgraph()
    {
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        SQuery = ""; string m1, eff_Dt;
        vartype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MLD_PTYPE");
        chk_indust = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(Trim(opt_param)) as opt from fin_Rsys_opt where trim(opt_id)='W0000' ", "opt");
        //vartype = "90";

        string mhd = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", "INR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,99,99,999.99");

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt_pw where branchcd='" + frm_mbr + "' and opt_id='W2015'", "opt_param");
        if (mhd != "0" && mhd != "-") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", mhd);

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt_pw where branchcd='" + frm_mbr + "' and opt_id='W2016'", "opt_param");
        if (mhd != "0" && mhd != "-" && mhd != "I") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,999,999,999.99");

        string fldvalue = "";
        string coma_sepr;
        coma_sepr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BR_COMMA");

        if (frm_formID == "F05118" || frm_formID == "F05115")
        {
            string mm1 = Convert.ToDateTime(todt).ToString("MMyyyy");
            string mm2 = Convert.ToDateTime(fromdt).ToString("MMyyyy");
            if (mm1 != mm2)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please select dates within a month only.!!");
                return;
            }
        }

        switch (Prg_Id)
        {
            case "F05112":
                SQuery = "Select Month_name,Amt as Amount from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round(sum(a.iamount/100000),2) as Amt ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER a WHERE  a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '4%' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                string SQuery2 = "Select Month_name,'Week : '||mdt as mdt,Amt as Amount from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,to_char(a.vchdate,'WW') as mdt,round(sum(a.iamount/100000),2) as Amt ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER a WHERE a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '4%' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM'),to_char(a.vchdate,'WW')) order by mdt,Month_name";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sales Chart", "line", "Figures in Lakhs from " + fromdt + " -" + todt + "", "For " + frm_mbr + " Branch", SQuery, "");
                fgen.Fn_FillChartDrill(frm_cocd, frm_qstr, "Sales Chart", "line", "Figures in Lakhs from " + fromdt + " -" + todt + "", "For " + frm_mbr + " Branch", SQuery, SQuery2, "", "container", "", "");
                break;

            case "F05118":
                SQuery = "select day_name,Amt as Amount from (Select to_Char(a.vchdate,'dd') as day_Name,round(sum(a.iamount/100000),2) as amt ,to_Char(a.vchdate,'dd') as dayz FROM IVOUCHER A WHERE a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sales Chart", "line", "Figures in Lakhs from " + fromdt + " -" + todt + "", "For All Branches", SQuery, "");
                break;

            case "F05113":
                SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iqtyout) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER a WHERE  a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '4%' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                fldvalue = "round(iamount/100000,2)";
                SQuery = "select a.branchcd as fstr, a.branchcd,c.name as branch_name,sum(apr+may+jun+jul+aug+sep+oct+nov+dec+jan+feb+mar) as total , sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from (select branchcd,(case when to_char(VCHDATE,'mm')='04' then " + fldvalue + " else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then " + fldvalue + " else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then " + fldvalue + " else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then " + fldvalue + " else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then " + fldvalue + " else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then " + fldvalue + " else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  " + fldvalue + " else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  " + fldvalue + " else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  " + fldvalue + " else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  " + fldvalue + " else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  " + fldvalue + " else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then " + fldvalue + " else 0 end ) as mar from ivoucher where branchcd != 'DD' and  type like '4%' and type!='47' AND VCHDATE " + DateRange + " ) a,type c where trim(a.branchcd)=trim(c.type1) and c.id='B' group by c.name, a.branchcd order by branchcd";

                col1 = fromdt.Substring(3, 2);
                col2 = todt.Substring(3, 2);

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT MTHNUM, MTHNAME FROM MTHS WHERE MTHNUM BETWEEN '" + col1 + "' AND '" + col2 + "' order by mthsno ");
                col1 = ""; col2 = "";
                foreach (DataRow dr in dt.Rows)
                {
                    col1 += "," + "(case when to_char(VCHDATE,'mm')='" + dr["mthnum"].ToString().Trim() + "' then " + fldvalue + " else 0 end ) as " + dr["MTHNAME"].ToString().Trim();
                    col2 += "," + "sum(" + dr["MTHNAME"].ToString().Trim() + ") as " + dr["MTHNAME"].ToString().Trim();
                }
                if (col1 != "")
                {
                    col1 = col1.TrimStart(',');
                    col2 = col2.TrimStart(',');
                }

                SQuery = "select c.name as branch_name," + col2 + " from (select branchcd, " + col1 + " from ivoucher where branchcd != 'DD' and  type like '4%' and type!='47' AND VCHDATE " + DateRange + " ) a,type c where trim(a.branchcd)=trim(c.type1) and c.id='B' group by c.name, a.branchcd order by branchcd";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, lblheader.Text + " Chart", "line", "Figures in Lakhs from " + fromdt + " -" + todt + "", lblheader.Text.Trim() + " (Figures in Lakhs)", SQuery, "");
                break;

            case "F05114":
                //SQuery = "Select Month_name,Amt as Amount from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round(sum(a.iamount/100000),2) as amt ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.TYPE LIKE '4%' and a.type!='47' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3) ,to_Char(a.vchdate,'YYYYMM')) order by mth";
                SQuery = "select branchcd,sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct ,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar  from(select branchcd,icode,(case when to_char(VCHDATE,'mm')='04' then iqtyout else 0 end ) as apr,(case when to_char(VCHDATE,'mm')='05' then iqtyout else 0 end ) as may,(case when to_char(VCHDATE,'mm')='06' then iqtyout else 0 end ) as jun,(case when to_char(VCHDATE,'mm')='07' then iqtyout else 0 end ) as jul,(case when to_char(VCHDATE,'mm')='08' then iqtyout else 0 end ) as aug,(case when to_char(VCHDATE,'mm')='09' then iqtyout else 0 end ) as sep,(case when to_char(VCHDATE,'mm')='10' then  iqtyout else 0 end ) as oct,(case when to_char(VCHDATE,'mm')='11' then  iqtyout else 0 end ) as nov,(case when to_char(VCHDATE,'mm')='12' then  iqtyout else 0 end ) as dec,(case when to_char(VCHDATE,'mm')='01' then  iqtyout else 0 end ) as jan,(case when to_char(VCHDATE,'mm')='02' then  iqtyout else 0 end ) as feb,(case when to_char(VCHDATE,'mm')='03' then iqtyout else 0 end ) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' AND VCHDATE " + DateRange + " ) a group by branchcd ";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, lblheader.Text, "funnel", "Figures in Lakhs from " + fromdt + " -" + todt + "", lblheader.Text.Trim() + " (Figures in Lakhs)", SQuery, "");
                break;

            case "F05115":
                break;
            //if (Prg_Id == "F05133" || Prg_Id == "F05116" || Prg_Id == "F05114" || Prg_Id == "F05118" || Prg_Id == "F05113" || Prg_Id == "F05115")
            //{
            //    SQuery = "Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iqtyout) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER a WHERE  a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '4%' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM') order by mth";
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            //    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Sales Chart", "line", "For the Period " + fromdt.Substring(6, 4) + " -" + todt.Substring(6, 4) + " ", "For "+ frm_mbr +" Branch", SQuery, "");
            //}

            //else if (Prg_Id == "F05134" || Prg_Id == "F05151" || Prg_Id == "F05152" || Prg_Id == "F05153" || Prg_Id == "F05154" || Prg_Id == "F05155" || Prg_Id == "F05156")
            //{
            //    SQuery = "Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.iqtyin) as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '0%' and a.vchdate " + DateRange + "  group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM') order by mth";
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            //    fgen.Fn_FillChart(frm_cocd, frm_qstr, "Purchase Chart", "line", "For the Period " + fromdt.Substring(6, 4) + " -" + todt.Substring(6, 4) + " ", "For All Branches", SQuery, "");
            //}
            #region Store MIS Inward
            case "F05168":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                SQuery = "select Day_name,Quantity from (select to_char(a.vchdate,'dd') as day_name,sum(a." + fldvalue + ") as quantity ,to_char(a.vchdate,'dd') as dayz from ivoucher a where a.branchcd='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + "  and a.store!='R' group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by dayz";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Items", "Day Wise", SQuery, "");
                break;

            case "F05169":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + "   and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Items", "Month Wise", SQuery, "");
                break;

            case "F05170":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD='" + frm_mbr + "' AND type like '0%' and type not in ('04','08') and vchdate " + DateRange + " group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Parties", "Day Wise", SQuery, "");
                break;

            case "F05171":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + "  and a.store!='R' group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Parties", "Month Wise", SQuery, "");
                break;

            case "F05172":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD!='DD' AND type like '0%' and type not in ('04','08') and vchdate " + DateRange + " group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Plants", "Day Wise", SQuery, "");
                break;

            case "F05173":
                fldvalue = "iamount";
                mq3 = "Inward Chart (Value Wise)";
                SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD!='DD' AND a.TYPE LIKE '0%' and a.type not in ('04','08') and a.vchdate " + DateRange + "  and a.store!='R'  group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Plants", "Month Wise", SQuery, "");
                break;
            #endregion

            #region Store MIS Outward
            case "F05178":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                SQuery = "Select Day_name,Quantity from (select to_char(a.vchdate,'dd') as day_name,sum(a." + fldvalue + ") as quantity ,to_char(a.vchdate,'dd') as dayz from ivoucher a where a.branchcd='" + frm_mbr + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + " group by to_Char(a.vchdate,'dd'),to_Char(a.vchdate,'dd')) order by Dayz";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Items", "Day Wise", SQuery, "");
                break;

            case "F05179":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Items", "Month Wise", SQuery, "");
                break;

            case "F05180":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD='" + frm_mbr + "' AND type like '2%' and vchdate " + DateRange + " group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Parties", "Day Wise", SQuery, "");
                break;

            case "F05181":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + " group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Parties", "Month Wise", SQuery, "");
                break;

            case "F05182":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                SQuery = "select to_char(vchdate,'dd') as day_name,sum(" + fldvalue + ") as quantity ,to_char(vchdate,'dd') as day from ivoucher where BRANCHCD!='DD' AND type like '2%' and vchdate " + DateRange + " group by to_Char(vchdate,'dd'),to_Char(vchdate,'dd') order by day_Name";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Plants", "Day Wise", SQuery, "");
                break;

            case "F05183":
                fldvalue = "iamount";
                mq3 = "Outward Chart (Value Wise)";
                SQuery = "Select Month_name,Quantity from (Select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(" + fldvalue + ") as Quantity ,to_Char(a.vchdate,'YYYYMM') as mth FROM IVOUCHER A WHERE a.BRANCHCD!='DD' AND a.TYPE LIKE '2%' and a.vchdate " + DateRange + "  group by substr(to_Char(a.vchdate,'MONTH'),1,3),to_Char(a.vchdate,'YYYYMM')) order by mth";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_FillChart(frm_cocd, frm_qstr, mq3, "line", "All Plants", "Month Wise", SQuery, "");
                break;
            #endregion
        }
    }
    //------------------------------------------------------------------------------------
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
    protected void btnFilter1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Filter1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Parameters to filter the Report", frm_qstr);
    }
    protected void btnFilter2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Filter2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Parameters to filter the Report", frm_qstr);
    }
    protected void btnFilter3_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Filter3";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Parameters to filter the Report", frm_qstr);
    }
    protected void btnFilter4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Filter4";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Parameters to filter the Report", frm_qstr);
    }
    protected void btnFilter5_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Filter5";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Parameters to filter the Report", frm_qstr);
    }
    protected void btnFilter6_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Filter6";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Parameters to filter the Report", frm_qstr);
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        btnhideF_s_Click("", EventArgs.Empty);
    }
}