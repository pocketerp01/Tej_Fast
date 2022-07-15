using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using IdeaSparx.CoolControls.Web;

using System.ComponentModel;
using System.Collections;

public partial class om_appr78 : System.Web.UI.Page
{
    string btnval, SQuery, mq0, frm_cocd, col1, col2, col3, frm_mbr, vardate, year, ulvl, HCID, xprdrange, cond, frm_cDt1, frm_cDt2, CSR;
    string frm_uname, frm_url, frm_qstr, frm_formID, DateRange, frm_UserID, fromdt, todt, cstr, xprdRange1, xprdRange, PrdRange;
    string mdt1, mdt2, mprdrange, otp, mobileno, smsModule = "N";
    string mhd = "", filePath = "", MV_CLIENT_GRP = "", party_cd, part_cd;
    double lowerLimit, upperLimit;
    int totCol = 50;
    bool signDsc = false;
    DataTable dt;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            btnnew.Focus();
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
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

                    MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP");


                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                set_val();
            }
            setGridWidt();
            if (vardate == "")
            {
                vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select to_date(to_char(sysdate,'dd/MM/YYYY'),'DD/MM/YYYY') AS DT FROM DUAL", "DT");
            }

            if (frm_formID == "F99164")
            {
                btnList.Visible = true; // DESKTOP RIGHTS
            }
            else
            {
                btnList.Visible = false;
            }
        }
    }
    void setGridWidt()
    {
        if (sg1.Rows.Count > 0)
        {
            for (int i = 7; i < sg1.Columns.Count; i++)
            {
                int widthMake = (sg1.Rows[0].Cells[i].Text.Trim().Length) * 10;
                if (widthMake < 80) widthMake = 80;
                if (widthMake > 200) widthMake = 200;
                sg1.Columns[i].HeaderStyle.Width = widthMake;
            }
        }
        //sg1.Columns[13].HeaderStyle.Width = 500;
    }
    public void enablectrl()
    {
        btnnew.Disabled = false;
        btnsave.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnext.Text = " Exit ";
        btnext.Enabled = true;
        btnext.AccessKey = "X";
        srch.Enabled = false;
        btnList.Disabled = false;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true;
        btnsave.Disabled = false;
        tkrow.Text = "20";
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnext.Text = "Cancel";
        btnext.Enabled = true;
        btnext.AccessKey = "C";
        srch.Enabled = true;
        btnList.Disabled = true;
    }
    public void clearctrl()
    { hffield.Value = ""; }
    public void set_val()
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "N");
        HCID = frm_formID;
        ul1.Visible = false;
        switch (HCID)
        {
            case "F90109":
                lblheader.Text = "Task Completion Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F60176":
                lblheader.Text = "CSS Cleared by Client";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F60181":
                lblheader.Text = "CSS Cleared by Assignor";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F60186":
                lblheader.Text = "Action Approved by Assignor";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F46106":
                lblheader.Text = "STL Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F96106":
                lblheader.Text = "DSL Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F97106":
                lblheader.Text = "CAM Log Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F10141":
                lblheader.Text = "Item Code Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F10142":
                lblheader.Text = "BOM Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F10143":
                lblheader.Text = "Process Plan Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F45149":
                lblheader.Text = "Lead/Enquiry Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;


            case "F15161":
                lblheader.Text = "Purchase Request/Indent Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F15162":
                lblheader.Text = "Purchase Request/Indent Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F15165":
                lblheader.Text = "Purchase Order Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F15166":
                lblheader.Text = "Purchase Order Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                ul1.Visible = true;
                Button1.Text = "Stock In Hand";
                Button2.Text = "Approved Rate List";
                Button3.Text = "Pending PR";
                Button4.Text = "Pending GE";
                break;
            case "F15607":
                lblheader.Text = "RFQ Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F81111":
                lblheader.Text = "Leaves Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F47126":
                lblheader.Text = "Domestic Sales Order Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                ul1.Visible = true;
                break;

            case "F49126":
                lblheader.Text = "Export Sales Order Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F49129":
                lblheader.Text = "Export Proforma Invoice Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F49130":
                lblheader.Text = "Export Proforma Invoice Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F70201":
                lblheader.Text = "Voucher Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F70203":
                lblheader.Text = "Voucher Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F25122C":
                lblheader.Text = "Challan Aprroval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F25122M":
                lblheader.Text = "MRR Aprroval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F50051":
                lblheader.Text = "Invoice Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F47127":
            case "F49127":
                lblheader.Text = "Sales Order Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F45110":
                lblheader.Text = "Quotation Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F47127M":
                lblheader.Text = "Master Sales Order Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F55128":
                lblheader.Text = "Export Sales Order Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F55129":
                lblheader.Text = "Export  Sales Order Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F15171":
                lblheader.Text = "Purchase Schedule Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F47128":
            case "F49128":
                lblheader.Text = "Sales Schedule Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F15176":
                lblheader.Text = "Approved Price list Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "M02032":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Purchase Order Checking";
                break;
            case "M02036":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Purchase Order Approval";
                break;
            case "M02040":
                lblheader.Text = "Purchase Schedule Approval";
                break;

            case "F15210":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Purchase Request/Indent Closure";
                break;
            case "F15211":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Purchase Order Closure";
                break;
            case "M02046":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Purchase Order Cancel";
                break;
            case "M10010B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Sales Order Checking(Dom.)";
                break;
            case "M10015B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Sales Order Approval(Dom.)";
                break;
            case "F47162":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Sales Order Closure(Domestic)";
                break;
            case "M11010B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Sales Order Checking(Exp.)";
                break;
            case "M11015B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Sales Order Approval(Export)";
                break;
            case "M11020B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Sales Order Closure(Exp.)";
                break;
            case "M10015A":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Proforma Invoice Approval(Domestic)";
                break;
            case "M11015A":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Proforma Invoice Approval(Exports)";
                break;
            case "F35110":
            case "F35106A":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Job Order Approval";

                ul1.Visible = true;
                Button1.Text = "Stock In Hand";
                Button2.Text = "Approved Rate List";
                Button3.Text = "Pending PR";
                Button4.Text = "Pending GE";
                break;
            case "F35111":// job order closure / re-call
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "Job Order Closure / Re-Call";
                lblsmallheader.Text = "( Check on 'Ok' if You want to Close :: Check on 'No' if You want to Re-Call JC )";
                break;
            case "M10024":
                lblheader.Text = "Sales Schedule Approval";
                break;
            case "M09008":
                lblheader.Text = "Lead Approval";
                break;
            case "M09028":
                lblheader.Text = "Quotation Approval";
                break;
            case "F10051":
                lblheader.Text = "Customer Request Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F10056":
                lblheader.Text = "Lead Approval";
                break;
            case "F20233":
                lblheader.Text = "Visitor Req Approval";
                if (frm_cocd == "BUPL" || frm_cocd == "DISP" || frm_cocd == "OMP") smsModule = "Y";
                break;
            case "F20235":
                lblheader.Text = "Visitor Outward Entry";
                break;
            case "W90108":
                lblheader.Text = "Approve Task Action Done";
                break;
            case "W90109":
                lblheader.Text = "Task Approval";
                break;
            case "F99164":
                lblheader.Text = "Desktop Rights";
                break;

            case "F47320":
                lblheader.Text = "Enquiry Closure";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F47323":
                lblheader.Text = "Quotation Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F81104":
                lblheader.Text = "Leave Request Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F81511":
                lblheader.Text = "Loan Request Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F85145":
                lblheader.Text = "Employee Master Approval";
                break;

            case "F85143":
                lblheader.Text = "Pay Increment Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F55160A":
            case "F79109":
                lblheader.Text = "Drawing / Artwork Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            default:
                lblheader.Text = "";
                break;
        }

        if (frm_cocd == "STUD" || frm_cocd == "MLGI")
        {
            signDsc = true;
        }
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        set_val();
        clearctrl();
        hffield.Value = "New";

        HCID = frm_formID;
        switch (HCID)
        {
            case "F47126":
            case "F49126":
            case "F49129":
            case "F49130":
            case "F45110":
            case "F47127M":
            case "F47127":
            case "F49127":

            case "F55128":
            case "F55129":
            case "F70201":
            case "F70203":
            case "F15211":
            case "F50051":
            case "F25122C":
            case "F25122M":
            case "F15166":
            case "F15607":
                fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                break;
            case "F99164":
                if (frm_mbr != "00")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
                    return;
                }
                SQuery = "SELECT USERID AS FSTR,USERNAME,USERID,EMAILID,ULEVEL,DEPTT FROM EVAS";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("-", frm_qstr);
                break;

            case "F47320":// AMAR - ENQUIRY CLOSURE
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,A.TYPE as enq_type,trim(a.acode) as cust_code,f.aname as customer,TRIM(a.icode) AS item_CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd  from  (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,acode,'ENQUIRY REGISTER' AS TYPE from wb_sorfq where branchcd='" + frm_mbr + "' and type ='ER' and nvl(trim(app_by),'-')='-' union all select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,acode,'ENG. CHANGE NOTIFICATION' AS TYPE from wb_sorfq where branchcd='" + frm_mbr + "' and type ='EC' and nvl(trim(app_by),'-')='-')a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) ORDER BY VDD,RFQ_NO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                fillGrid();
                fgen.EnableForm(this.Controls); disablectrl();
                break;

            case "F47323":// AMAR - QUOTATION APPROVAL
                SQuery = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS QUOTE_NO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS DATED,A.ACODE AS CUST_CODE,F.ANAME AS CUSTOMER,A.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM SOMASQ A,FAMST F ,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='FQ' AND NVL(TRIM(A.APP_BY),'-')='-' ORDER BY VDD,ORDNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                fillGrid();
                fgen.EnableForm(this.Controls); disablectrl();
                break;
            case "F35111":
                fgen.msg("Do You Want to See All Job Card", "CMSG", "Press Yes to See only Pending'13'(No to See All)");
                break;

            case "F85145":
                SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by grade_code";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Grade", frm_qstr);
                break;

            case "F85143":
                SQuery = "SELECT A.BRANCHCD||TRIM(A.GRADE)||TRIM(A.EMPCODE) AS FSTR,A.EMPCODE,A.NAME AS EMPLOYEE_NAME,A.VCHDATE,A.INC_APP_DT,A.GRADE,T.NAME AS GRADE_NAME,A.ER1,A.ER2,A.ER3,A.ER4,A.ER5,A.ER6,A.ER7,A.ER8,A.ER9,A.ER10,A.ER11,A.ER12,A.ER13,A.ER14,A.ER15,A.ER16,A.ER17,A.ER18,A.ER19,A.ER20 from PAYINCR A,TYPE T WHERE TRIM(A.GRADE)=TRIM(T.TYPE1) AND T.ID='I' AND A.BRANCHCD='" + frm_mbr + "' AND SUBSTR(NVL(TRIM(A.EMPIMG),'-'),1,3)!='[A]'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                fillGrid();
                fgen.EnableForm(this.Controls); disablectrl();
                break;

            default:
                fgen.Fn_open_prddmp1("-", frm_qstr);
                break;
        }
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        lbllink.Value = "";
        hffield.Value = "";
        int i = 0; //HCID = Request.Cookies["rid"].Value.ToString();
        HCID = frm_formID;
        foreach (GridViewRow row in sg1.Rows)
        {
            CheckBox chk1 = (CheckBox)row.FindControl("chkapp");
            CheckBox chk2 = (CheckBox)row.FindControl("chkrej");
            if (chk1.Checked == true || chk2.Checked == true)
            { i = 1; break; }
        }
        if (i != 0)
        {
            i = 1;
            string MREQ_RZN;
            MREQ_RZN = fgenMV.Fn_Get_Mvar(frm_qstr, "U_REQ_RZN");


            foreach (GridViewRow row in sg1.Rows)
            {
                CheckBox chk1 = (CheckBox)row.FindControl("chkapp");
                CheckBox chk2 = (CheckBox)row.FindControl("chkrej");
                TextBox tk = (TextBox)row.FindControl("txtcompdt");
                TextBox tkreason = (TextBox)row.FindControl("txtreason");

                row.BackColor = System.Drawing.Color.White;

                if (chk1.Checked == true && chk2.Checked == true)
                {
                    if (frm_formID == "F47323")
                    {
                        fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Quotation No. " + row.Cells[9].Text.Trim() + " Dt :" + row.Cells[10].Text.Trim()); i = 0;
                    }
                    if (frm_formID == "F81104")
                    {
                        fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Leave Request No. " + row.Cells[9].Text.Trim() + " Dt :" + row.Cells[10].Text.Trim()); i = 0;
                    }
                    if (frm_formID == "F81511")
                    {
                        fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Loan Request No. " + row.Cells[9].Text.Trim() + " Dt :" + row.Cells[10].Text.Trim()); i = 0;
                    }
                    if (frm_formID == "F85145")
                    {
                        fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Employee Code " + row.Cells[9].Text.Trim()); i = 0;
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Entry No. " + row.Cells[3].Text.Trim()); i = 0;
                    }
                    row.BackColor = System.Drawing.Color.LightPink;
                    return;
                }
                else
                {
                    if (chk1.Checked == true || chk2.Checked == true)
                    {
                        if (HCID == "**M10015A" || HCID == "**M11015A") i = 1;
                        else if (HCID == "F10051" || HCID == "F10056")
                        {
                            if (frm_cocd == "SEL")
                            {
                                if (tkreason.Text.Trim().Length <= 2)
                                {
                                    fgen.msg("-", "AMSG", "Please Select Employee to Depute for this Work'13'See at Entry No. " + row.Cells[9].Text.Trim() + " Dt :" + row.Cells[10].Text.Trim());
                                    row.BackColor = System.Drawing.Color.LightPink;
                                    i = 0;
                                    return;
                                }
                            }
                            else
                            {
                                i = fgen.ChkDate(tk.Text.Trim());
                                if (i != 0) i = 1;
                                else
                                {
                                    fgen.msg("-", "AMSG", "Not a valid date entered infront of'13'Complaint No. " + row.Cells[9].Text.Trim()); return;
                                }
                                if (Convert.ToDateTime(Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy")) < Convert.ToDateTime(System.DateTime.Now.ToShortDateString()))
                                {
                                    fgen.msg("-", "AMSG", "Date can not be less then present Date'13'See at Complaint No. " + row.Cells[9].Text.Trim()); i = 0; return;
                                }
                                if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                {
                                    fgen.msg("-", "AMSG", "Please enter the reason for Refusal'13'See at Complaint No. " + row.Cells[9].Text.Trim()); i = 0; return;
                                }
                            }
                        }
                        else
                        {
                            //i = fgen.ChkDate(tk.Text.Trim());
                            //if (i != 0) i = 1;
                            //else
                            //{ fgen.msg("-", "AMSG", "Not a valid date entered infront of'13'Entry No. " + row.Cells[3].Text.Trim()); return; }
                            //if (HCID == "F10051" && Convert.ToDateTime(tk.Text.Trim()) < Convert.ToDateTime(System.DateTime.Now.ToShortDateString()))
                            //{ fgen.msg("-", "AMSG", "Date can not be less then present Date'13'See at Entry No. " + row.Cells[3].Text.Trim()); i = 0; return; }
                            switch (frm_formID)
                            {
                                case "F47320":
                                    if ((MREQ_RZN == "Y") && chk1.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter The Reason For Closure '13'See At Enquiry No. " + row.Cells[9].Text.Trim() + " Dt :" + row.Cells[10].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    break;
                                case "F47323":
                                    if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter The Reason For Refusal '13'See At Quotation No. " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    break;
                                case "F81104":
                                    if (tk.Text.Length <= 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter Approval/Refusal Date '13'See At Leave Request No. " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    if (Convert.ToDateTime(tk.Text) < Convert.ToDateTime(row.Cells[10].Text.Trim()))
                                    {
                                        fgen.msg("-", "AMSG", "Approval/Refusal Date can not be Lesser Than Leave Request Date '13'See At Leave Request No. " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter The Reason For Refusal '13'See At Leave Request No. " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    break;
                                case "F81511":
                                    if (tk.Text.Length <= 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter Approval/Refusal Date '13'See At Loan Request No. " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    if (Convert.ToDateTime(tk.Text) < Convert.ToDateTime(row.Cells[10].Text.Trim()))
                                    {
                                        fgen.msg("-", "AMSG", "Approval/Refusal Date can not be Lesser Than Loan Request Date '13'See At Loan Request No. " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter The Reason For Refusal '13'See At Loan Request No. " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    break;
                                case "F35111":
                                    if (chk1.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter The Reason For Closure of Job Card");
                                        row.BackColor = System.Drawing.Color.Yellow;
                                        i = 0;
                                        return;
                                    }
                                    break;
                                case "F85145":
                                case "F85143":
                                    if ((chk1.Checked == true && ((TextBox)row.FindControl("txtcompdt")).Text.Trim().Length < 1) || (chk2.Checked == true && ((TextBox)row.FindControl("txtcompdt")).Text.Trim().Length < 1))
                                    {
                                        fgen.msg("-", "AMSG", "Please Enter Date. '13'See At Employee Code " + row.Cells[9].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    break;
                                case "F15161":
                                case "F15162":
                                    if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please enter the Reason for Refusal '13'See at P.R.No. " + row.Cells[10].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    break;
                                default:
                                    if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                    {
                                        fgen.msg("-", "AMSG", "Please enter the Reason for Refusal '13'See at PO.No. " + row.Cells[10].Text.Trim());
                                        row.BackColor = System.Drawing.Color.LightPink;
                                        i = 0;
                                        return;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            if (i != 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save !!");
        }
        else
        {
            if (HCID == "*M10015B") fgen.msg("-", "AMSG", "Please Approve any one row to save");
            else fgen.msg("-", "AMSG", "Please Approve or refuse any one row to save");
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        dt = new DataTable();
        col1 = "";
        SQuery = "";

        btnval = hffield.Value;
        HCID = frm_formID;
        switch (HCID)
        {
            case "99702":
            case "99001":
                switch (btnval)
                {
                    case "New_E":
                        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                        if (col1 == "N") SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + frm_mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy') and ent_by='" + frm_uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
                        else
                        {
                            hffield.Value = "VW";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select distinct type1 as fstr,type1 as code,name,acref as email_id from typegrp where id='SE' order by name");
                            fgen.Fn_open_mseek("-", frm_qstr);
                        }
                        break;
                    case "VW":
                        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                        if (col1.Trim().Length == 4) SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + frm_mbr + "' and type='DK' and nvl(col3,'-')'-' and vchdate between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy') and trim(acode) in ('" + col1 + "') and ent_by='" + frm_uname + "' GROUP BY vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
                        else SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + frm_mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy') and trim(acode) in (" + col1 + ") and ent_by='" + frm_uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
                        break;
                }
                break;
            case "70002":
                switch (btnval)
                {
                    case "New_E":
                        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                        if (col1 == "N") SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM TYPEGRP B, TYPEGRP C,SCRATCH2 A LEFT JOIN SCRATCH2 D ON  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  WHERE TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE='SD' and A.vchdate between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy')  ORDER BY A.VCHNUM";
                        else
                        {
                            SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM SCRATCH2 A,TYPEGRP B, TYPEGRP C ,SCRATCH2 D WHERE  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  AND TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE='SD' AND A.vchdate between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy') ORDER BY A.VCHNUM";
                        }
                        break;
                    case "VW":
                        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                        if (col1.Trim().Length == 4) SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM SCRATCH2 A,TYPEGRP B, TYPEGRP C ,SCRATCH2 D WHERE  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  AND TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE='SD' AND A.vchdate between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy') ORDER BY A.VCHNUM";
                        else SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM SCRATCH2 A,TYPEGRP B, TYPEGRP C ,SCRATCH2 D WHERE  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  AND TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE='SD' AND A.vchdate between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy') ORDER BY A.VCHNUM";
                        break;
                }
                break;

            case "*M10015B":
                switch (btnval)
                {
                    case "New_E":
                        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                        if (col1 == "N")
                        {
                            xprdrange = "between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy')";
                            if (frm_cocd == "NEOP")
                            {
                                if (ulvl == "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,a.ent_by,a.ent_Dt,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " order by vdd desc,a.ordno,A.TYPE";
                                else
                                {
                                    col1 = ""; col1 = "";
                                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + frm_uname + "'", "icons");
                                    if (col1.Length > 1)
                                    {
                                        string[] word = col1.Split(',');
                                        foreach (string vp in word)
                                        {
                                            if (col2.Length > 0) col2 = col2 + "," + "'" + vp.ToString().Trim() + "'";
                                            else col2 = "'" + vp.ToString().Trim() + "'";
                                        }
                                        if (col1 != "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " and trim(b.bssch) in (" + col2 + ") order by vdd desc ,a.ordno,A.TYPE";
                                    }
                                }
                            }
                            else SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,a.ent_by,a.ent_dt,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " order by vdd desc,a.ordno,A.TYPE";
                        }
                        else
                        {
                            hffield.Value = "VW";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select type1 as fstr,name as document_type,type1 as code from type where id='V' and substr(type1,1,1)='4' order by type1");
                            fgen.Fn_open_mseek("-", frm_qstr);
                        }
                        break;
                    case "VW":
                        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                        if (col1.Trim().Length == 4) cond = "and a.type in (" + col1 + ")";
                        else cond = "and a.type in (" + col1 + ")";
                        xprdrange = "between to_date('" + ViewState["frm_cDt1"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["frm_cDt2"].ToString() + "','dd/mm/yyyy')";
                        if (frm_cocd == "NEOP")
                        {
                            if (ulvl == "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " " + cond + " order by vdd desc,a.ordno,A.TYPE";
                            else
                            {
                                col1 = ""; col1 = "";
                                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + frm_uname + "'", "icons");
                                if (col1.Length > 1)
                                {
                                    string[] word = col1.Split(',');
                                    foreach (string vp in word)
                                    {
                                        if (col2.Length > 0) col2 = col2 + "," + "'" + vp.ToString().Trim() + "'";
                                        else col2 = "'" + vp.ToString().Trim() + "'";
                                    }
                                    if (col1 != "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " and trim(b.bssch) in (" + col2 + ") " + cond + " order by vdd desc ,a.ordno,A.TYPE";
                                }
                            }
                        }
                        else SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " " + cond + " order by vdd desc,a.ordno,A.TYPE";
                        break;
                }
                break;
            case "F10051":
            case "F10056":
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                switch (btnval)
                {
                    case "EMPLYEE":
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtreason")).Text = col2 + "~" + col1;
                        break;
                }
                break;
            case "F99164":
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_UPI", fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_UPN", fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"));
                lblF1.Text = "User ID : " + col1 + " User Name : " + col2;
                SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) AS FSTR,A.VCHNUM AS ENTRY_NO,TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DT,A.frm_title AS TILE,a.Ent_by,A.Ent_Dt FROM DSK_CONFIG a WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='80' order by a.vchnum,a.obj_name ";
                SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.OBJ_NAME) AS FSTR,A.VCHNUM AS ENTRY_NO,TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DT,A.frm_title AS TILE,a.Ent_by,A.Ent_Dt FROM DSK_CONFIG a WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='80' and substr(trim(obj_name),1,3) in('TXT','GRA') order by a.vchnum,a.obj_name";
                break;

            case "F85145":
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT A.BRANCHCD||TRIM(A.GRADE)||TRIM(A.EMPCODE) AS FSTR,A.EMPCODE AS CODE,A.NAME AS EMPLOYEE_NAME,A.FHNAME AS FATHERS_NAME,A.GRADE,T.NAME AS GRADE_NAME,A.WRKHOUR,A.DEPTT,A.DEPTT_TEXT,A.DESG,A.DESG_TEXT FROM EMPMAS A ,TYPE T WHERE TRIM(A.GRADE)=TRIM(T.TYPE1) AND T.ID='I' AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + col1 + "' AND SUBSTR(NVL(TRIM(APPR_BY),'-'),1,3)!='[A]' ORDER BY FSTR";
                break;

            case "F55160A":
            case "F79109":
                SQuery = "";
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (col1.Contains("~"))
                {
                    if (col1.Split('~')[1] == "NO")
                    {
                        fgen.msg("Download not allowed.", "AMSG", "This file is restricted from download/view.");
                        return;
                    }
                    else
                    {
                        string filePath = col1.Split('~')[0];
                        if (hffield.Value == "DWN")
                        {
                            try
                            {
                                Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                                Session["FileName"] = filePath;
                                Response.Write("<script>");
                                Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                                Response.Write("</script>");
                            }
                            catch { }
                        }
                        else
                        {
                            filePath = filePath.Replace("\\", "/").Replace("UPLOAD", "");
                            //filePath = Server.MapPath(@"../tej-base/Upload/" + filePath);
                            filePath = "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + filePath + "','90%','90%','Finsys Viewer');", true);
                        }
                    }
                }
                break;
        }
        if (SQuery.Length > 0)
        {
            fgen.EnableForm(this.Controls); disablectrl();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            fillGrid();
        }
        else
        {
            if (hffield.Value != "DWN")
            {
                hf1.Value = Request.Cookies["REPLY"].Value.ToString();
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
        }
    }
    void fillGrid()
    {
        if (dt.Rows.Count > 0)
        {
            DataTable neWDt = dt.Copy();
            ViewState["sg1"] = neWDt;
            ViewState["Squery"] = SQuery;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            makeColNameAsMine(dt);
            sg1.DataSource = dt;
            sg1.DataBind();
            sg1.Visible = true;
            hideAndRenameCol();
            if (frm_formID == "F47320")
            {
                for (int i = 0; i < sg1.Rows.Count; i++)
                {
                    sg1.Columns[1].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[i].Cells[1].CssClass = "hidden";

                    sg1.Columns[2].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[i].Cells[2].CssClass = "hidden";

                    sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[i].Cells[3].CssClass = "hidden";
                }
            }
            dt.Dispose();
            setGridWidt();

            extraColorCondition();
        }
        else
        {
            enablectrl(); fgen.DisableForm(this.Controls);
            fgen.msg("-", "AMSG", "No Data for selected Time period");
        }
    }
    void extraColorCondition()
    {
        switch (frm_formID)
        {
            case "F70201":
            case "F70203":
                dt = new DataTable();
                if (sg1.Rows.Count > 0)
                {
                    SQuery = "Select a.msgtxt as imagef,a.branchcd||a.type||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') as fstr from ATCHVCH a where a.branchcd='" + frm_mbr + "' ";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        mhd = "";
                        mhd = fgen.seek_iname_dt(dt, "FSTR='" + gr.Cells[8].Text.Trim() + "' ", "IMAGEF");
                        if (mhd.Length > 5)
                        {
                            lblsmallheader.Text = "Highlighted Rows have the Attachment. ";
                            gr.BackColor = System.Drawing.Color.GreenYellow;
                        }
                    }
                }
                break;
        }
    }

    void makeColNameAsMine(DataTable dtColNameTable)
    {
        int colFound = dtColNameTable.Columns.Count;
        for (int i = 1; i <= totCol; i++)
        {
            if (colFound > i) dtColNameTable.Columns[i].ColumnName = "sg1_f" + i;
            else dtColNameTable.Columns.Add("sg1_f" + i, typeof(string));
        }
    }
    void hideAndRenameCol()
    {
        DataTable dtColNameTab = (DataTable)ViewState["sg1"];
        int colFound = dtColNameTab.Columns.Count;
        int totResrvCol = 8;
        for (int i = totResrvCol; i <= totCol + totResrvCol; i++)
        {
            if (colFound + totResrvCol > i)
            {
                sg1.HeaderRow.Cells[i].Text = dtColNameTab.Columns[i - totResrvCol].ColumnName;
                int widthMake = (sg1.HeaderRow.Cells[i].Text.Length + 2) * 10;
                sg1.Columns[i].HeaderStyle.Width = widthMake;
            }
            else
            {
                sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                sg1.Rows[0].Cells[i].CssClass = "hidden";
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        set_val();

        double doc_rej = 0;

        if (hffield.Value == "New")
        {
            string party_cd = "";
            string part_cd = "";
            party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
            if (party_cd.Trim().Length <= 1)
            {
                party_cd = "%";
            }
            if (part_cd.Trim().Length <= 1)
            {
                part_cd = "%";
            }

            HCID = frm_formID;
            string chk_opt = "";
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            mdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
            mdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
            mprdrange = "between to_date('" + mdt1 + "','dd/mm/yyyy') and to_date('" + mdt2 + "','dd/mm/yyyy')";
            ViewState["frm_cDt1"] = col1; ViewState["frm_cDt2"] = col2;
            SQuery = "";
            if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
            switch (HCID)
            {
                case "F60176":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.CSSNO)||to_char(a.CSSdt,'dd/mm/yyyy') as fstr,to_Char(a.CSSdt,'yyyymmdd') as vdd,a.CCode,a.CSSNO as Css_No,to_Char(A.CSSdt,'dd/mm/yyyy') as css_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.dir_comp,a.Last_Action,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_log a where a.branchcd='" + frm_mbr + "' and a.type='CS' and a.CSSdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and upper(Last_action)='MARKED COMPLETE' " + cond + " order by vdd,a.CSSNO";
                    break;
                case "F60181":
                    //and upper(Work_action)!='-'
                    SQuery = "select distinct a.branchcd||a.type||trim(a.CSSNO)||to_char(a.CSSdt,'dd/mm/yyyy') as fstr,to_Char(a.CSSdt,'yyyymmdd') as vdd,a.CCode,a.CSSNO as Css_No,to_Char(A.CSSdt,'dd/mm/yyyy') as css_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.dir_comp,a.Work_Action,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_log a where a.branchcd='" + frm_mbr + "' and a.type='CS' and a.CSSdt " + mprdrange + " and trim(nvl(a.Fapp_by,'-'))='-'  order by vdd,a.CSSNO";
                    // ADD A NEW COND IN WHERE CLAUSE ON 28 JAN 2020
                    SQuery = "select distinct a.branchcd||a.type||trim(a.CSSNO)||to_char(a.CSSdt,'dd/mm/yyyy') as fstr,to_Char(a.CSSdt,'yyyymmdd') as vdd,a.CCode,a.CSSNO as Css_No,to_Char(A.CSSdt,'dd/mm/yyyy') as css_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.dir_comp,a.Work_Action,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_log a where a.branchcd='" + frm_mbr + "' and a.type='CS' and a.CSSdt " + mprdrange + " and trim(nvl(a.Fapp_by,'-'))='-' and trim(nvl(a.DIR_COMP,'-'))!='Y' order by vdd,a.CSSNO";
                    break;
                case "F60186":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.actno)||to_char(a.actdt,'dd/mm/yyyy') as fstr,to_Char(a.actdt,'yyyymmdd') as vdd,a.CCode,a.actno as Act_No,to_Char(A.Actdt,'dd/mm/yyyy') as Act_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.asg_agt as Assign_to,a.Act_status,a.remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_act a where a.branchcd='" + frm_mbr + "' and a.type='AC' and a.TASK_COMPL='Y' and  a.actdt " + mprdrange + "  and trim(nvl(a.app_by,'-'))='-' order by vdd,a.actno";
                    break;
                case "F90109":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.TRCNO)||to_char(a.TRCDT,'dd/mm/yyyy') as fstr,to_Char(a.TRCDT,'yyyymmdd') as vdd,a.CCode,a.TRCNO as Task_No,to_Char(A.trcdt,'dd/mm/yyyy') as Task_Dt,a.Client_name,a.Team_member,a.Task_type ,a.Tgt_days,a.Curr_Stat,a.oremarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_Task_Log a where a.branchcd='" + frm_mbr + "' and a.type='TR' and trim(nvl(a.TASK_Close,'-'))!='Y' and  a.TRCdt " + mprdrange + "  and trim(nvl(a.app_by,'-'))='-' order by vdd,a.TRCNO";
                    break;

                case "F94106":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.STlNO)||to_char(a.STldt,'dd/mm/yyyy') as fstr,to_Char(a.sTldt,'yyyymmdd') as vdd,a.CCode,a.STlNO as STl_No,to_Char(A.sTldt,'dd/mm/yyyy') as STl_Dt,a.Emodule as STl_Module,a.Eicon as STl_Name,a.EVERTICAL,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_STl_log a where a.branchcd='" + frm_mbr + "' and a.type='TG' and a.STldt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.STlNO";
                    break;

                case "F96106":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.DSLNO)||to_char(a.DSLdt,'dd/mm/yyyy') as fstr,to_Char(a.DSLdt,'yyyymmdd') as vdd,a.CCode,a.DSLNO as DSL_No,to_Char(A.DSLdt,'dd/mm/yyyy') as DSL_Dt,a.Emodule as DSL_Module,a.Eicon as DSL_Name,a.EVERTICAL,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_DSL_log a where a.branchcd='" + frm_mbr + "' and a.type='SL' and a.DSLdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.DSLNO";
                    break;
                case "F97106":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.CAMNO)||to_char(a.CAMdt,'dd/mm/yyyy') as fstr,to_Char(a.CAMdt,'yyyymmdd') as vdd,a.TCode,a.CAMNO as CAM_No,to_Char(A.cAMdt,'dd/mm/yyyy') as CAM_Dt,a.Cam_type as cAM_Type,a.Cam_Spec,a.Tcode as Team_member,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_CAM_log a where a.branchcd='" + frm_mbr + "' and a.type='EQ' and a.CAMdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.CamNO";
                    break;

                case "S06005B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||lpad(trim(to_char(a.srno,'999')),3,'0') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as Alt_No,to_Char(A.vchdate,'dd/mm/yyyy') as Alt_Dt,a.Acode as Client,a.qrytopic as Qry_Topic,a.qryokay as Qry_Okay,a.Qmark_Name as Alloted_To,a.Qry_rmk,a.Qry_Tgtdt,a.Last_Action,a.Qry_Link,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from cquery_alt a where a.branchcd='" + frm_mbr + "' and a.type='CA' and a.vchdate " + mprdrange + "  and trim(nvl(a.clo_by,'-'))='-' order by vdd,a.vchnum";
                    break;
                case "F10141":
                    SQuery = "select distinct trim(a.icode) as fstr,to_Char(a.ent_dt,'yyyymmdd') as vdd,a.Icode as ERP_Code,a.Iname as Item,a.cpartno as Part_No,a.CDrgno,A.Unit,a.HSCODe,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from item a where a.branchcd!='DD' and to_Date(to_char(a.ent_dt,'dd/mm/yyyy'),'dd/mm/yyyy') " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.deac_by,'-'))='-'  order by vdd,a.icode";
                    break;
                case "F10142":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as BOM_No,to_Char(A.vchdate,'dd/mm/yyyy') as BOM_Dt,c.iname as Item_Name,c.cpartno as Part_No,c.Cdrgno,c.unit,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from itemosp a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='BM' and a.vchdate " + mprdrange + " and  trim(nvl(a.app_by,'-'))='-'  order by vdd,a.vchnum";
                    break;

                case "F10143":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as PP_No,to_Char(A.vchdate,'dd/mm/yyyy') as PP_Dt,c.iname as Item_Name,c.cpartno as Part_No,c.Cdrgno,c.unit,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from inspmst a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='70' and a.vchdate " + mprdrange + " and  trim(nvl(a.app_by,'-'))='-'  order by vdd,a.vchnum";
                    break;
                case "F45149":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.lrcno)||to_char(a.lrcdt,'dd/mm/yyyy') as fstr,to_Char(a.lrcdt,'yyyymmdd') as vdd,a.lrcno as Lead_No,to_Char(A.lrcdt,'dd/mm/yyyy') as Lead_Dt,a.Ldescr as Contact_Name,a.Lsubject as Item_Name,a.Expval as Approx_Val,a.Lgrade as Lead_Status,a.Lremarks,a.Oremarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_lead_log a where a.branchcd='" + frm_mbr + "' and a.type='LR' and a.lrcdt " + mprdrange + " and  trim(nvl(a.app_by,'-'))='-'  and trim(nvl(a.FILENAME,'-'))='-'  order by vdd,a.lrcno";
                    break;

                case "F15161":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as Pr_No,to_Char(A.orddt,'dd/mm/yyyy') as Pr_Dt,a.Bank as Deptt,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PR_Qty,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='60' and a.orddt " + mprdrange + " and trim(nvl(a.chk_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and a.pflag!=0 order by vdd,a.ordno";
                    break;
                case "F15162":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as Pr_No,to_Char(A.orddt,'dd/mm/yyyy') as Pr_Dt,a.Bank as Deptt,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PR_Qty,c.unit,a.desc_ as Remarks,A.chk_by,(case when trim(nvl(A.chk_by,'-'))='-' then '-' ELSE TO_CHAR(A.chk_dT,'DD/mm/yyyy') end) as chk_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='60' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.pflag!=0 order by vdd,a.ordno";
                    break;
                case "F15165":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,A.Prate as rate,a.nxtmth as last_purch_rate,A.Pdisc as disc,a.rate_cd as po_value,b.payterm,c.unit,a.pr_no,to_char(a.pr_dt,'dd/mm/yyyy') as pr_dt,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.chk_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "F15166":
                    cond = "";
                    //if (fgen.make_double(ulvl) > 1)
                    {
                        lowerLimit = 0;
                        upperLimit = 0;
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select prate||'~'||pdisc as col1,allowedbr from pomst where type='20' and trim(acode)='" + frm_UserID + "'", "col1");
                        if (col1.Contains("~"))
                        {
                            lowerLimit = col1.Split('~')[0].ToString().toDouble();
                            upperLimit = col1.Split('~')[1].ToString().toDouble();
                        }
                        cond = " and a.rate_cd between " + lowerLimit + " and " + upperLimit + " ";
                        lblsmallheader.Text = " Allowed Limit " + lowerLimit + " to " + upperLimit + " ";
                    }
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,A.Prate,A.Pdisc,c.unit,a.desc_ as Remarks,A.chk_by,TO_CHAR(A.chk_dT,'DD/mm/yyyy') as chk_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.pflag!=1 " + cond + " order by vdd desc,a.ordno";
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,A.Prate as rate,((a.prate*(100-a.pdisc)/100))-a.pdiscamt as net_rate,a.nxtmth as last_purch_rate,A.Pdisc as disc,a.rate_cd as po_value,b.payterm,c.unit,a.pr_no,to_char(a.pr_dt,'dd/mm/yyyy') as pr_dt,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.chk_by,(case when trim(nvl(A.chk_by,'-'))='-' then '-' ELSE TO_CHAR(A.chk_dT,'DD/mm/yyyy') end) as chk_dt,a.acode,a.icode as erpcode from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and " +
                        "a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.chk_by,'-'))!='-' and a.pflag!=1 " + cond + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by vdd,a.ordno";
                    break;
                case "F15607":
                    cond = "";
                    //SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,A.Prate as rate,((a.prate*(100-a.pdisc)/100))-a.pdiscamt as net_rate,a.nxtmth as last_purch_rate,A.Pdisc as disc,a.rate_cd as po_value,b.payterm,c.unit,a.pr_no,to_char(a.pr_dt,'dd/mm/yyyy') as pr_dt,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.chk_by,(case when trim(nvl(A.chk_by,'-'))='-' then '-' ELSE TO_CHAR(A.chk_dT,'DD/mm/yyyy') end) as chk_dt,a.acode,a.icode as erpcode from WB_PORFQ a,FAMST b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' " + cond + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by vdd,a.ordno";
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as rfq_No,to_Char(A.orddt,'dd/mm/yyyy') as rfq_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as rfq_Qty,A.Prate as rate,((a.prate*(100-a.pdisc)/100))-a.pdiscamt as net_rate,a.nxtmth as last_purch_rate,A.Pdisc as disc,a.rate_cd as rfq_value,b.payterm,c.unit,a.pr_no,to_char(a.pr_dt,'dd/mm/yyyy') as pr_dt,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.chk_by,(case when trim(nvl(A.chk_by,'-'))='-' then '-' ELSE TO_CHAR(A.chk_dT,'DD/mm/yyyy') end) as chk_dt,a.acode,a.icode as erpcode from WB_PORFQ A,FAMST B,ITEM C where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' " + cond + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by vdd,a.ordno";
                    break;
                case "F35110":
                case "F35106A":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as job_No,to_Char(A.vchdate,'dd/mm/yyyy') as job_Dt,c.iname as Item_Name,c.cpartno as Part_No,a.qty as job_Qty,c.unit,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from costestimate a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '30%' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  and a.srno=0 order by vdd,a.vchnum";
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as job_No,to_Char(A.vchdate,'dd/mm/yyyy') as job_Dt,c.iname as Item_Name,c.cpartno as Part_No,b.qtyord as SO_Qty,a.qty as job_Qty,c.unit,d.aname as customer,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,b.ordno as so_no,to_char(b.orddt,'dd/mm/yyyy') as so_dt,b.ent_by as so_entby,to_char(b.ent_dt,'dd/mm/yyyy') as so_entdt from costestimate a,somas B,item c,famst d where trim(A.icodE)=trim(c.icode) and substr(a.convdate,1,20)||trim(a.icodE)=b.branchcd||b.type||trim(b.ordno)||to_char(b.orddt,'dd/mm/yyyy')||trim(b.icode) and trim(b.acode)=trim(d.acodE) and a.branchcd='" + frm_mbr + "' and a.type like '30%' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  and a.srno=0 order by vdd,a.vchnum";
                    break;
                case "F35111":// job order closure / re-call
                    if (hf1.Value == "Y")
                        cond = "and a.ACTUALcost>a.jobcardqty and a.jobcardrqd='Y' and length(Trim(nvl(a.req_Closedby,'-')))<=1";
                    else cond = "";
                    mq0 = "Select trim(a.solink)||trim(a.srno)||trim(a.actualcost)||trim(a.acode)||trim(a.icode) as fstr,'-' as vdd,trim(nvl(c.maker,'-'))||':'||a.socat as Category,B.aname as Customer,C.iname as Item,C.cpartno as Partno ,to_char(a.dlv_date,'dd/mm/yyyy') as Delv_date,BUDGETCOST as Delv_Qty,ACTUALCOST as Prod_Qty,a.icode,trim(a.solink)||trim(a.srno) as solink,a.SoRemarks,a.jobcardqty,a.jobcardno,a.dlv_date as delvdt,a.rowid as Iden,a.jobcardrqd,Req_Closedby,a.vchnum as Ordno,a.vchdate as Orddt,a.ACTUALCOST-a.jobcardqty as Balance_job,substr(a.jobcardno,6) as job_Cardno,a.app_dt from budgmst a, famst b , item c where a.branchcd='" + frm_mbr + "' and a.type='46' and 1=1 and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and a.dlv_Date " + mprdrange + " and trim(a.acode) like '%' and trim(a.icode) like '%' and a.socat like '%' and a.ACTUALCOST>0 " + cond + " and 1=1 order by a.dlv_date,B.aname,c.CPARTNO";
                    SQuery = "SELECT * FROM (" + mq0 + ") WHERE trim(icode)||SUBSTR(solink,1,20) NOT IN (sELECT trim(icode)||BRANCHCD||TYPE||ORDNO||TO_CHAR(ORDDT,'DD/MM/YYYY') FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND (trim(ICAT)='Y' or trim(nvl(app_by,'-'))='-') )";
                    break;
                case "F81111":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.lrqno)||to_char(a.lrqdt,'dd/mm/yyyy') as fstr,to_Char(a.lrqdt,'yyyymmdd') as vdd,a.lrqno as Lrq_No,to_Char(A.lrqdt,'dd/mm/yyyy') as Lrq_Dt,b.Name as Employee_Name,b.Deptt_Text as Department,b.desg_text as Designation,a.levfrom,a.levupto,a.LRemarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from WB_LEVREQ a,empmas b where a.branchcd||trim(A.empcode)=trim(b.branchcd)||b.grade||trim(B.empcode) and a.branchcd='" + frm_mbr + "' and a.type like 'LR%' and a.lrqdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.lrqno";
                    break;

                case "F15171":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as Sch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Sch_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Total as Sch_Qty,a.line_rmk as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.srno from schedule a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='66' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' order by vdd,a.vchnum,a.srno";
                    break;
                case "F47128":
                case "F49128":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as Sch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Sch_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Total as Sch_Qty,a.line_rmk as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.srno from schedule a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='46' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' order by vdd,a.vchnum,a.srno";
                    break;

                case "F15176":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as APL_No,to_Char(A.vchdate,'dd/mm/yyyy') as APL_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.irate as APL_rate,a.Disc as APL_Disc,c.unit,a.row_text as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.srno from appvendvch a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='10' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' order by vdd,a.vchnum,a.srno";
                    break;

                case "F47126":
                case "F49126":
                    if (frm_cocd == "PHGL")
                    {
                        cond = "";
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ALLOWFSEC FROM EVAS WHERE USERNAME='" + frm_uname + "'", "ALLOWFSEC");
                        if (mhd != "0")
                        {
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(TYPE1)||':'||TRIM(NAME) AS acref2 FROM TYPEGRP WHERE ID='EM' AND ACREF='" + mhd + "' ");
                            mhd = "";
                            foreach (DataRow dr in dt.Rows)
                            {
                                mhd += ",'" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT USERNAME FROM EVAS WHERE UPPER(TRIM(ALLOWIGRP))='" + dr["acref2"].ToString().Trim().ToUpper() + "'", "") + "'";
                            }
                            cond = "AND A.ENT_BY IN (" + mhd.TrimStart(',') + ")";

                            mq0 = " SELECT acode,sum(ODUEdays) AS odueday,SUM(dramt) AS dr,SUM(cramt) AS cr,SUM(net) AS net FROM (select TRIM(a.ACODE) as acode,a.branchcd,B.PAYMENT,b.mobile,A.invno,A.invdate,to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM) as ODUEdays,A.INVDATE+B.PAY_NUM AS Due_Dt,A.dramt,A.cramt,A.dramt-A.cramt as net,b.email as p_email from recdata A,FAMST B where TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd!='DD') GROUP BY acode HAVING SUM(ODUEdays)>0 and SUM(net)>0";
                            SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,b.climit as cr_limit,e.net as tot_ostanding,round(b.climit + e.net) as os_with_so,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a left outer join (" + mq0 + ") e on trim(a.acode)=trim(e.acodE),famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' " + cond + " order by vdd,a.ordno";
                        }
                    }
                    else SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,c.ciname,a.del_date as delivery_Date,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                    break;
                case "F49129":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,c.ciname,a.del_date as delivery_Date,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasq a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                    break;
                case "F49130":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,c.ciname,a.del_date as delivery_Date,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasq a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                    break;

                case "F70201":
                case "F70203":
                    mq0 = "";
                    string mhd1 = fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHER", "CHECK_BY");
                    if (mhd1 == "0")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHER ADD CHECK_BY VARCHAR(20) DEFAULT '-' ");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHER ADD CHECK_DATE DATE ");
                    }
                    cond = "and trim(nvl(a.app_by,'-'))='-' AND trim(nvl(a.check_by,'-'))!='-'";
                    if (frm_cocd == "MEGH" || frm_cocd == "SDM") cond = "and trim(nvl(a.app_by,'-'))='-'";
                    mq0 = "FIXEDON";
                    if (HCID == "F70201")
                    {
                        cond = "and trim(nvl(a.check_by,'-'))='-'";
                        mq0 = "allowedbr";
                    }

                    if (party_cd.Length < 2)
                    {
                        lowerLimit = 0;
                        upperLimit = 0;
                        //FIXEDON
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select " + mq0 + " as col1,allowedbr from pomst where type='21' and trim(acode)='" + frm_UserID + "'", "col1");
                        mq0 = "";
                        {
                            foreach (string s in col1.Split(';'))
                            {
                                mq0 += ",'" + s + "'";
                            }
                            mq0 = mq0.TrimStart(',');
                        }
                        cond += " and type in (" + mq0 + ")";
                    }
                    else cond += " and type='" + party_cd + "'";

                    string cond1 = " and ((case when substr(type,1,1) in ('2','3','4') then a.tfccr when type='59' then a.tfccr when substr(type,1,1) in ('1','5','6') then a.tfcdr end)>0 or (case when substr(type,1,1) in ('2','3','4') then a.cramt when type='59' then a.cramt when substr(type,1,1) in ('1','5','6') then a.dramt end)>0)  ";
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as VOUCHER_No,to_Char(A.vchdate,'dd/mm/yyyy') as VOUCHER_Dt,b.Aname as Party,A.Ent_by as entered_by,TO_CHAR(A.ENT_daTe,'DD/mm/yyyy') as entered_dt,A.Edt_by as edited_by,TO_CHAR(A.EdT_daTe,'DD/mm/yyyy') as edited_dt " + (HCID == "F70203" ? ",a.check_by as checked_by,to_char(a.check_date,'dd/mm/yyyy') as checked_dt" : "") + " ,a.branchcd as branch_code,a.type as voucher_type from voucher a,famst b where trim(A.rcodE)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.vchdate " + mprdrange + " " + cond + " " + cond1 + " AND A.SRNO=1 order by vdd desc,a.vchnum";
                    break;
                case "F50051":
                    cond = "and trim(nvl(a.app_by,'-'))='-' ";
                    cond = "";
                    if (party_cd.Length <= 2)
                        cond += " and c.type='" + party_cd + "'";
                    else cond += " and c.type like '4%'";

                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as vch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Vch_Dt,b.Aname as Party,A.ACODE,C.BILL_TOT,A.Ent_by,TO_CHAR(A.ENT_daTe,'DD/mm/yyyy') as Ent_Dt,a.branchcd,a.type from voucher a,famst b,SALE C where TRIM(A.BRANCHCD)||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)=TRIM(C.BRANCHCD)||C.TYPE||TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY')||TRIM(C.ACODE) AND trim(A.AcodE)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.vchdate " + mprdrange + " " + cond + " order by vdd desc,a.vchnum ";
                    break;
                case "F25122C":
                    cond = "";
                    if (party_cd.Length <= 2)
                        cond += " and A.type='" + party_cd + "'";
                    else cond += " and A.type like '2%'";
                    SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as CHL_no,to_char(a.vchdate,'dd/mm/yyyy') as CHL_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,a.refnum as ref_no,to_Char(a.refdate,'dd/mm/yyyy') as ref_dt,a.genum as ge_no,to_Char(a.gedate,'dd/mm/yyyy') as ge_dt from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' " + cond + " and a.vchdate " + DateRange + " and trim(nvl(trim(a.dsc_dtl),'-'))='-' order by vdd desc";
                    break;
                case "F25122M":
                    cond = "";
                    if (party_cd.Length <= 2)
                        cond += " and a.type='" + party_cd + "'";
                    else cond += " and a.type like '0%'";
                    SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,a.refnum as ref_no,to_Char(a.refdate,'dd/mm/yyyy') as ref_dt,a.genum as ge_no,to_Char(a.gedate,'dd/mm/yyyy') as ge_dt from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' " + cond + " and a.vchdate " + DateRange + "  and trim(nvl(trim(a.dsc_dtl),'-'))='-' order by vdd desc";
                    break;
                case "F47127":
                    chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where branchcd='00' and OPT_ID='W0047'", "fstr");

                    if (chk_opt == "Y")
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,c.ciname,to_char(a.del_date,'dd/mm/yyyy') as delivery_Date,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))!='-' and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                        if (frm_cocd == "PHGL")
                        {

                            mq0 = "select acode,sum(net) as net from (SELECT acode,(ODUEdays) AS odueday,SUM(dramt) AS dr,SUM(cramt) AS cr,SUM(net) AS net FROM (select TRIM(a.ACODE) as acode,a.branchcd,B.PAYMENT,b.mobile,A.invno,A.invdate,to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM) as ODUEdays,A.INVDATE+B.PAY_NUM AS Due_Dt,A.dramt,A.cramt,A.dramt-A.cramt as net,b.email as p_email from recdata A,FAMST B where TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd!='DD') where ODUEdays>0 GROUP BY acode,ODUEdays ) group by acode having sum(net)>0";
                            SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,b.climit as cr_limit,e.net as tot_ostanding,round(b.climit + e.net) as os_with_so,b.payment as pay_days,b.balop as grace_prd,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a left outer join (" + mq0 + ") e on trim(a.acode)=trim(e.acodE),famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))!='-' and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                        }
                    }
                    else
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,c.ciname,to_char(a.del_date,'dd/mm/yyyy') as delivery_Date,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                        if (frm_cocd == "PHGL")
                        {
                            mq0 = "select acode,sum(net) as net from (SELECT acode,(ODUEdays) AS odueday,SUM(dramt) AS dr,SUM(cramt) AS cr,SUM(net) AS net FROM (select TRIM(a.ACODE) as acode,a.branchcd,B.PAYMENT,b.mobile,A.invno,A.invdate,to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM) as ODUEdays,A.INVDATE+B.PAY_NUM AS Due_Dt,A.dramt,A.cramt,A.dramt-A.cramt as net,b.email as p_email from recdata A,FAMST B where TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd!='DD') where ODUEdays>0 GROUP BY acode,ODUEdays ) group by acode having sum(net)>0";
                            SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,b.climit as cr_limit,e.net as tot_ostanding,round(b.climit + e.net) as os_with_so,b.payment as pay_days,b.balop as grace_prd,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a left outer join (" + mq0 + ") e on trim(a.acode)=trim(e.acodE),famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                        }
                    }
                    if (frm_cocd == "SAIA")
                        SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                    break;
                case "F49127":
                    chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where branchcd='00' and OPT_ID='W0047'", "fstr");
                    if (chk_opt == "Y")
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type in ('4F') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                        if (frm_cocd == "PHGL")
                        {
                            cond = "";
                            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ALLOWFSEC FROM EVAS WHERE USERNAME='" + frm_uname + "'", "ALLOWFSEC");
                            if (mhd != "0")
                            {
                                dt = new DataTable();
                                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT ACREF2 FROM TYPEGRP WHERE ID='EM' AND ACREF='" + mhd + "' ");
                                mhd = "";
                                foreach (DataRow dr in dt.Rows)
                                {
                                    mhd += ",'" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT USERNAME FROM EVAS WHERE UPPER(TRIM(ALLOWIGRP))='" + dr["acref2"].ToString().Trim().ToUpper() + "'", "") + "'";
                                }
                                cond = "AND A.ENT_BY IN (" + mhd.TrimStart(',') + ")";

                                mq0 = " SELECT acode,sum(ODUEdays) AS odueday,SUM(dramt) AS dr,SUM(cramt) AS cr,SUM(net) AS net FROM (select TRIM(a.ACODE) as acode,a.branchcd,B.PAYMENT,b.mobile,A.invno,A.invdate,to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM) as ODUEdays,A.INVDATE+B.PAY_NUM AS Due_Dt,A.dramt,A.cramt,A.dramt-A.cramt as net,b.email as p_email from recdata A,FAMST B where TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd!='DD') GROUP BY acode HAVING SUM(ODUEdays)>0 and SUM(net)>0";
                                SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,b.climit as cr_limit,e.net as tot_ostanding,round(b.climit + e.net) as os_with_so,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a left outer join (" + mq0 + ") e on trim(a.acode)=trim(e.acodE),famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type in ('4F') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' " + cond + "  order by vdd,a.ordno";
                            }
                        }
                    }
                    else
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type in ('4F') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                        if (frm_cocd == "PHGL")
                        {
                            cond = "";
                            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ALLOWFSEC FROM EVAS WHERE USERNAME='" + frm_uname + "'", "ALLOWFSEC");
                            if (mhd != "0")
                            {
                                dt = new DataTable();
                                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT ACREF2 FROM TYPEGRP WHERE ID='EM' AND ACREF='" + mhd + "' ");
                                mhd = "";
                                foreach (DataRow dr in dt.Rows)
                                {
                                    mhd += ",'" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT USERNAME FROM EVAS WHERE UPPER(TRIM(ALLOWIGRP))='" + dr["acref2"].ToString().Trim().ToUpper() + "'", "") + "'";
                                }
                                cond = "AND A.ENT_BY IN (" + mhd.TrimStart(',') + ")";

                                mq0 = " SELECT acode,sum(ODUEdays) AS odueday,SUM(dramt) AS dr,SUM(cramt) AS cr,SUM(net) AS net FROM (select TRIM(a.ACODE) as acode,a.branchcd,B.PAYMENT,b.mobile,A.invno,A.invdate,to_DatE('" + todt + "','dd/mm/yyyy')-(A.INVDATE+B.PAY_NUM) as ODUEdays,A.INVDATE+B.PAY_NUM AS Due_Dt,A.dramt,A.cramt,A.dramt-A.cramt as net,b.email as p_email from recdata A,FAMST B where TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd!='DD') GROUP BY acode HAVING SUM(ODUEdays)>0 and SUM(net)>0";
                                SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,b.climit as cr_limit,e.net as tot_ostanding,round(b.climit + e.net) as os_with_so,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a left outer join (" + mq0 + ") e on trim(a.acode)=trim(e.acodE),famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type in ('4F') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' " + cond + " order by vdd,a.ordno";
                            }
                        }
                    }

                    break;

                case "F47127M":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,c.ciname,a.del_date as delivery_Date,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasm a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type like '4%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                    break;
                case "F45110":
                    chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0063'", "fstr");
                    string fam_tbl = "FAMST";
                    if (chk_opt == "Y")
                    { fam_tbl = "wbvu_fam_crm"; }
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as Quote_No,to_Char(A.orddt,'dd/mm/yyyy') as Quote_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as Quote_Qty,a.irate as Quote_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,c.ciname,a.del_date as delivery_Date,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasq a," + fam_tbl + " b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type like '4%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' and a.acode like '" + party_cd + "%' order by vdd,a.ordno";
                    break;

                case "F55128":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;
                case "F55129":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;


                case "M02032":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.chk_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-'  and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "M02036":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.chk_by,TO_CHAR(A.chk_dT,'DD/mm/yyyy') as chk_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.pflag!=1  order by vdd,a.ordno";
                    break;
                case "F15210":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as Pr_No,to_Char(A.orddt,'dd/mm/yyyy') as Pr_Dt,a.Bank as Deptt,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PR_Qty,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.Ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Chk_by,TO_CHAR(A.Chk_dT,'DD/mm/yyyy') as Chk_Dt,A.App_by,TO_CHAR(A.App_dT,'DD/mm/yyyy') as App_Dt from pomas a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='60' and a.orddt " + mprdrange + " and a.pflag!=0 order by vdd,a.ordno";
                    //vipin
                    SQuery = "select a.branchcd||'60'||substr(a.fstr,10,6)||substr(a.fstr,7,2)||'/'||substr(a.fstr,5,2)||'/'||substr(a.fstr,1,4)||trim(a.ERP_code) as fstr,b.Iname as Item_Name,substr(a.fstr,10,6) as prno,substr(a.fstr,7,2)||'/'||substr(a.fstr,5,2)||'/'||substr(a.fstr,1,4) as prdt,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,b.Irate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,(a.desc_) as Remarks,b.hscode,(a.bank) as Deptt,(a.delv_item) as Reqd_Dt,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Req_Qty,trim(a.Fstr) as PR_link,a.psize as fo_no,nvl(b.iweight,1) as iweight from (select branchcd,fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,max(bank) as bank,max(delv_item) As delv_item,max(desc_) as desc_,max(psize) as psize from (SELECT branchcd,to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,nvl(bank,'-') As bank,nvl(delv_item,'-') As delv_item,nvl(desc_,'-') as desc_,psize from pomas where branchcd='" + frm_mbr + "' and type='60' and trim(pflag)!=0 and trim(app_by)!='-' and orddt " + mprdrange + " union all SELECT branchcd,to_ChaR(pr_Dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,0 as Qtyord,qtyord,null as bank,null as delv_item,null as desc_,psize from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt>=to_Date('01/04/2017','dd/mm/yyyy'))  group by fstr,ERP_code,branchcd having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) order by B.Iname,trim(a.fstr)";
                    break;
                case "F15211":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.Ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Chk_by,TO_CHAR(A.Chk_dT,'DD/mm/yyyy') as Chk_Dt,A.App_by,TO_CHAR(A.App_dT,'DD/mm/yyyy') as App_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and a.pflag!=1 order by vdd,a.ordno";
                    SQuery = "select a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.Ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Chk_by,TO_CHAR(A.Chk_dT,'DD/mm/yyyy') as Chk_Dt,A.App_by,TO_CHAR(A.App_dT,'DD/mm/yyyy') as App_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and a.pflag!=1 order by vdd,a.ordno";
                    //vipin 
                    cond = "";
                    if (part_cd.Length > 5)
                        cond = " and trim(acode) in (" + part_cd + ")";
                    SQuery = "select replace(trim(a.Fstr),'-','') as fstr,'-' as sss,substr(a.fstr,19,6) as po_no,substr(a.fstr,16,2)||'/'||substr(a.fstr,14,2)||'/'||substr(a.fstr,10,4) as po_dt,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,a.Prate as item_Rate,(a.Qtyord)-(a.Soldqty) as Balance_Qty,b.Cdrgno,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,null as btchno,null as btchdt from (select fstr,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate from pomas where branchcd='" + frm_mbr + "' and type like '" + party_cd + "%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and orddt " + mprdrange + " " + cond + " union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,IQTYIN as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') " + cond + ")  group by fstr,ERP_code having (case when sum(Qtyord)>0 then sum(Qtyord)-sum(Soldqty) else max(prate) end)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by substr(a.fstr,19,6),B.Iname,trim(a.fstr)";
                    break;
                case "M02046":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.Ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Chk_by,TO_CHAR(A.Chk_dT,'DD/mm/yyyy') as Chk_Dt,A.App_by,TO_CHAR(A.App_dT,'DD/mm/yyyy') as App_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "M10010B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y'  order by vdd,a.ordno";
                    break;
                case "M10015B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "F47162":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + "  and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;

                case "M11010B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M11015B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M11020B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + "  and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M10015A":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasp a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M11015A":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasp a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M09028":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasq a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "99001":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["frm_cDt1"] = col1; ViewState["frm_cDt2"] = col2;
                    hffield.Value = "New_E";
                    fgen.msg("-", "CMSG", "Do you want to select user id'13'(No for all users)");
                    break;
                case "70002":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["frm_cDt1"] = col1; ViewState["frm_cDt2"] = col2;
                    hffield.Value = "New_E";
                    fgen.msg("-", "CMSG", "Do you want to see completed  jobs'13'(No for all jobs)");
                    break;
                case "*M10015B":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["frm_cDt1"] = col1; ViewState["frm_cDt2"] = col2;
                    hffield.Value = "New_E";
                    fgen.msg("-", "CMSG", "Do you want to select Order Type'13'(No for all)");
                    break;
                case "F10051":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["frm_cDt1"] = col1; ViewState["frm_cDt2"] = col2;
                    if (ulvl == "0") SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as req_no,to_Char(A.vchdate,'dd/mm/yyyy') as req_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,A.ENt_BY,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd";
                    else
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as req_no,to_Char(A.vchdate,'dd/mm/yyyy') as req_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') and trim(a.ent_by)='" + frm_uname + "' order by vdd";
                        if (frm_cocd == "SRIS") SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as req_no,to_Char(A.vchdate,'dd/mm/yyyy') as req_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,A.ENt_BY,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd";
                    }
                    //SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as req_no,to_Char(A.vchdate,'dd/mm/yyyy') as req_dt,a.col8 as machine_srno,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,A.ENt_BY,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd";
                    if (frm_cocd == "SEL")
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as req_no,to_Char(A.vchdate,'dd/mm/yyyy') as req_dt,a.col8 as machine_srno,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,A.ENt_BY,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd";
                    }
                    break;
                case "F10056":
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ENQ_MAST", "APP_BY");
                    if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ENQ_MAST ADD APP_BY VARCHAR(20) DEFAULT '-' ");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ENQ_MAST", "APP_DT");
                    if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ENQ_MAST ADD APP_DT DATE DEFAULT SYSDATE ");

                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["frm_cDt1"] = col1; ViewState["frm_cDt2"] = col2;
                    if (ulvl == "0") SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||UPPER(TRIM(ITEM)) as fstr,a.vchnum as LEAD_no,to_Char(A.vchdate,'dd/mm/yyyy') as LEAD_dt,A.CLIENT as customer,A.ITEM as product,A.CONTACT,A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,A.EPRIORITY AS PRIORITY,A.ACT_BY,A.ACODE,A.ICODE,to_Char(a.vchdate,'yyyymmdd') as vdd from enq_mast a where a.branchcd='" + frm_mbr + "' and a.type='20' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd desc,a.vchnum desc";
                    else SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||UPPER(TRIM(ITEM)) as fstr,a.vchnum as LEAD_no,to_Char(A.vchdate,'dd/mm/yyyy') as LEAD_dt,A.CLIENT as customer,A.ITEM as product,A.CONTACT,A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,A.EPRIORITY AS PRIORITY,A.ACT_BY,A.ACODE,A.ICODE,to_Char(a.vchdate,'yyyymmdd') as vdd from enq_mast a where a.branchcd='" + frm_mbr + "' and a.type='20' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd desc,a.vchnum desc";
                    break;
                case "F20233":
                    xprdrange = mprdrange;
                    SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,col16 as visitor_name,col15 as comp_name,  to_char(docdate,'dd/mm/yyyy') as visit_date,col32 as exp_time, REMARKS as visit_reason,COL17 AS LOCATION,COL19 AS DEPARTMENT,COL21 AS DESIGNATION,COL22 AS VISITOR_TYPE,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD,'-' AS COL1,'-' AS COL2,'-' AS COL3,'-' COL4 from scratch2  where branchcd ='" + frm_mbr + "' and type='VR' and vchdate " + xprdrange + " AND NVL(APP_BY,'-')='-' order by VDD desc,vchnum desc ";
                    break;
                case "F20235":
                    xprdrange = mprdrange;
                    SQuery = "SELECT DISTINCT branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no, TO_CHAR(ent_dt,'DD/MM/YYYY') as entry_date,col16 as visitor_name,col15 as comp_name,col17 as location,col12 as purpose,col19 as department,col21 as designation,to_char(docdate,'dd/mm/yyyy') as last_visited_on,col23 as mobile,col26 as mfg,col29 as serial_no,acode as empid,col1 as name,COL7 as emp_dept,COL8 as emp_desig,REMARKS,ent_by,TO_CHAR(ent_dt,'DD/MM/YYYY') AS ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD FROM scratch2 where type='VM' and branchcd ='" + frm_mbr + "' and vchdate " + xprdrange + " AND NVL(APP_BY,'-')='-' AND TRIM(NVL(COL38,'-'))='-' order by VDD desc,vchnum DESC";
                    break;
                case "W90108":
                    xprdrange = mprdrange;
                    SQuery = "select DISTINCT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY') AS FSTR,col6 AS TASKNO,TO_CHAR(col48,'DD/MM/YYYY') AS TASK_DATE ,COL2 AS EMAIL_ID,REASON FROM SCRATCH2 WHERE BRANCHCD='" + frm_mbr + "' AND  TYPE='TA' and vchdate " + xprdrange + " AND nvl(TRIM(APP_BY),'-')='-' ORDER BY col6 DESC";
                    SQuery = "select DISTINCT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DDMMYYYY') AS FSTR,A.col6 AS TASKNO,TO_CHAR(A.col48,'DD/MM/YYYY') AS TASK_DATE ,A.COL2 AS userid,A.REASON,(case when substr(trim(a.col2),1,1)='E' then t.acref else e.emailid end) as emailid FROM SCRATCH2 A LEFT JOIN EVAS E ON TRIM(A.COL2)=TRIM(E.USERID) LEFT JOIN TYPEGRP T ON TRIM(A.COL2)=TRIM(T.TYPE1) AND T.ID='SE' WHERE A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE='TA' and a.vchdate " + xprdrange + " AND nvl(TRIM(A.APP_BY),'-')='-' ORDER BY col6 DESC";
                    if (frm_cocd == "TEST")
                        SQuery = "select TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY') AS FSTR,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + frm_mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate " + xprdrange + " and ent_by='" + frm_uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY')";
                    break;

                case "W90109":
                    xprdrange = mprdrange;
                    SQuery = "select TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY') AS FSTR,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + frm_mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate " + xprdrange + " and ent_by='" + frm_uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy'),TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY')";
                    break;

                case "F81104":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.lrqno)||to_char(a.lrqdt,'dd/mm/yyyy') as fstr,a.lrqno as lv_req_no,to_char(a.lrqdt,'dd/mm/yyyy') as lv_req_dt,b.name,b.deptt_text as departemt,b.desg_text as designation,a.LEVFROM as from_dt,A.LEVUPTO as return_dt,a.CONT_NAME as alt_contact,A.CONT_NO as alt_contact_no,a.empcode,a.lv_time as leave_time,a.ret_time as return_time,a.tot_days as total_days,a.time_in_hrs, a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.lrqdt,'yyyymmdd') as vdd from  WB_LEVREQ a,empmas b where a.branchcd||trim(a.empcode)=b.branchcd||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.type='LR' and a.lrqdt between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') and nvl(trim(a.app_by),'-')='-' order by vdd,a.lrqno";
                    break;

                case "F81511":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    SQuery = "select trim(a.branchcd)||trim(a.type)||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.empcode,b.name as empname,a.grade,to_char(b.dtjoin,'dd/mm/yyyy') as joindt,to_char(a.inst_st_dt,'dd/mm/yyyy') as Installment_start_dt,a.deptt,nvl(dramt,0) as amt,nvl(a.cramt,0) as month,nvl(os_amt,0) as os_Amt,nvl(a.INSTAMT,0) as monthly_install,nvl(a.CURRSAL,0) as salry,a.remark,a.cur_loan,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt,a.edt_by,to_char(a.edt_dt,'dd/mm/yyyy') as edt_Dt,to_char(a.vchdate,'yyyymmdd') as vdd from wb_payloan a,empmas b where trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and a.branchcd='" + frm_mbr + "' and a.type='01' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') and nvl(trim(a.app_by),'-')='-' order by vdd,entry_no";
                    break;
                case "F55160A":
                case "F79109":
                    cond = "";
                    if (ulvl == "M")
                        cond = " and trim(a.acode)='" + frm_uname + "' ";
                    SQuery = "SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DD/MM/YYYY') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,c.iNAME INAME,TRIM(c.cpartno) AS CPARTNO,a.remarks,a.invno as lead_no,to_char(a.invdate,'dd/mm/yyyy') as lead_date,a.col5 as lead_subject,B.aNAME CUSTOMER,a.acode,a.ent_by,a.ent_dt from WB_DRAWREC a,FAMST B,ITEM C where TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.branchcd='" + frm_mbr + "' and A.type='DE' AND TRIM(NVL(A.APP_BY,'-'))='-' AND A.VCHDATE " + DateRange + " " + cond + " order by VDD DESC,A.VCHNUM ";
                    if (fgen.getOptionPW(frm_qstr, frm_cocd, "W2030", "OPT_ENABLE", frm_mbr) == "Y")
                        SQuery = "SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DD/MM/YYYY') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,B.NAME CUSTOMER,c.NAME INAME,TRIM(c.ACREF2) AS CPARTNO,trim(c.acref) as modal_no,a.acode,a.rno as revision_no,a.remarks,a.ent_by,a.ent_dt  from WB_DRAWREC a,TYPEGRP B,TYPEGRP C where TRIM(A.ACODE)=TRIM(B.TYPE1) AND B.ID='C1' AND TRIM(A.ICODE)=TRIM(C.TYPE1) AND C.ID='P1' AND A.branchcd='" + frm_mbr + "' and A.type='DE' AND TRIM(NVL(A.APP_BY,'-'))='-'  AND A.VCHDATE " + DateRange + " " + cond + " order by VDD DESC,A.VCHNUM ";
                    break;
            }
            if (SQuery.Length > 0)
            {
                fgen.EnableForm(this.Controls); disablectrl();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                fillGrid();
            }
        }
        else if (hffield.Value == "Btn1" || hffield.Value == "Btn2" || hffield.Value == "Btn3" || hffield.Value == "Btn4") buttonQuery();
        else
        {
            col1 = "";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                lbllink.Value = "";
                int index = 0;
                if (frm_formID == "F70201*" ||
                        frm_formID == "F70203*")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "runProgBar();", true);
                }
                else if (frm_formID == "F99164")
                {
                    for (int i = 0; i < sg1.PageCount; i++)
                    {
                        sg1.SetPageIndex(i);
                        foreach (GridViewRow row1 in sg1.Rows)
                        {
                            CheckBox chk1 = (CheckBox)row1.FindControl("chkapp");
                            CheckBox chk2 = (CheckBox)row1.FindControl("chkrej");
                            col1 = "";
                            if (chk1.Checked == true)
                            {
                                if (index == 0)
                                {
                                    SQuery = "DELETE FROM DSK_WCONFIG WHERE USERID='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_UPI") + "' AND USERNAME='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_UPN") + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                SQuery = "INSERT INTO DSK_WCONFIG (BRANCHCD,TYPE,VCHNUM,VCHDATE,SRNO,USERID,USERNAME,OBJ_NAME) VALUES ('" + frm_mbr + "','80','" + row1.Cells[9].Text.Trim() + "',to_date('" + row1.Cells[10].Text.Trim() + "','dd/mm/yyyy'),1,'" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_UPI") + "','" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_UPN") + "','" + row1.Cells[11].Text.Trim() + "') ";
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                index++;
                            }
                        }
                    }
                    fgen.msg("-", "AMSG", "Document Approval / Refusal Successfully completed"); lblF1.Text = "";
                    enablectrl(); sg1.DataSource = null; sg1.DataBind(); sg1.Visible = false; ViewState["sg1"] = null; sg1.PageIndex = 0; ViewState["CheckBoxArray"] = null;
                }
                else
                {
                    int totApp = 0, totRej = 0;
                    foreach (GridViewRow row in sg1.Rows)
                    {
                        CheckBox chk1 = (CheckBox)row.FindControl("chkapp");
                        CheckBox chk2 = (CheckBox)row.FindControl("chkrej");
                        TextBox tk = (TextBox)row.FindControl("txtcompdt");
                        TextBox mreason = (TextBox)row.FindControl("txtreason");
                        string rej_rsn = mreason.Text.ToString();
                        string mydoc;
                        string myrjflag;
                        string myappno;
                        string myappdt;
                        string myquery;
                        string mytable;
                        mydoc = "Doc";
                        myrjflag = "0";
                        myappno = "app_by";
                        myappdt = "app_dt";
                        myquery = "";
                        mytable = "";
                        // HCID = Request.Cookies["rid"].Value.ToString();
                        HCID = frm_formID;
                        switch (HCID)
                        {
                            case "F47162":
                            case "M11020B":
                            case "F15210":
                            case "F15211":
                            case "M02046":

                            case "F15161":
                            case "F15162":
                            case "F15165":
                            case "F15166":
                            case "M02032":
                            case "M02036":
                                // F15161 lblheader.Text = "P.R. Check";
                                // F15162 lblheader.Text = "P.R. Approval";    
                                // M02032 lblheader.Text = "P.O. Check";
                                // M02036 lblheader.Text = "P.O. Approval";    
                                if (HCID == "F15161" || HCID == "F15162" || HCID == "F15210")
                                {
                                    mydoc = "PR. ";
                                    myrjflag = "0";
                                }
                                if (HCID == "F15161" || HCID == "F15165" || HCID == "M02032")
                                {
                                    myappno = "chk_by";
                                    myappdt = "chk_dt";

                                }
                                if (HCID == "F15162" || HCID == "F15166" || HCID == "F47127" || HCID == "F45110" || HCID == "F47127M" || HCID == "F49127" || HCID == "F49130" || HCID == "F55129"
                                    )
                                {
                                    myappno = "app_by";
                                    myappdt = "app_dt";

                                }


                                if (HCID == "M02032" || HCID == "M02036" || HCID == "F15211" || HCID == "M02046")
                                {
                                    mydoc = "PO. ";
                                    myrjflag = "1";
                                }

                                if (chk1.Checked == true)
                                {
                                    myquery = "update pomas set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate,pbasis=to_Char(sysdate,'dd/mm/yyyy') where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                    //send_m("A", row.Cells[8].Text.Trim());

                                    if (signDsc) sign_dsc_background("F1004", frm_mbr + row.Cells[8].Text.Trim().Substring(2, 2) + row.Cells[10].Text.Trim() + ";" + row.Cells[11].Text.Trim());
                                }
                                else if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    if (HCID == "F15210" || HCID == "F15211" || HCID == "M02046" || HCID == "F47162" || HCID == "M11020B")
                                    {

                                        if (HCID == "F15210") { myquery = "update pomas set atch1='" + rej_rsn + "',desp_to='-',term=trim(term)||' Closed by " + frm_uname + "',pbasis=to_char(sysdate,'dd/mm/yyyy'),pflag=0 where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode) ='" + row.Cells[8].Text.Trim() + "'"; }
                                        if (HCID == "F15211") { myquery = "update pomas set APP_BY='(C)'||TRIM(NVL(APP_BY,'-')),desp_to='" + rej_rsn + "',pflag=1 ,term=trim(term)||' Closed by " + frm_uname + "',invdate=sysdate where trim(icode)||to_ChaR(orddt,'YYYYMMDD')||ordno||lpad(trim(cscode),4,'0') ='" + row.Cells[8].Text.Trim() + "'"; }
                                        if (HCID == "M02046") { myquery = "update pomas set term='* * CANCELLED P.O.* * '||' " + rej_rsn + " " + frm_uname + " '||trim(term),pflag=1 ,qtysupp=2, pr_no='-' where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'"; }
                                        if (HCID == "F47162" || HCID == "M11020B") { myquery = "update somas set shipmark='By " + frm_uname + "'||' on '||sysdate||' Reason " + rej_rsn + "',icat='Y' where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'"; }
                                    }
                                    else
                                    {
                                        myquery = "update pomas set " + myappno + "='(R)" + frm_uname + "'," + myappdt + "=sysdate,pbasis=to_Char(sysdate,'dd/mm/yyyy'),rate_diff='" + mydoc + " REJECTED (" + rej_rsn + ")',pflag=" + myrjflag + " where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    }

                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                    //send_m("A", row.Cells[8].Text.Trim());
                                }
                                break;
                            case "M10015A":
                            case "M11015A":
                            case "M09028":
                            case "M10010B":
                            case "M10015B":
                            case "M11010B":
                            case "M11015B":

                            case "F47126":
                            case "F49126":
                            case "F49129":
                            case "F49130":

                            case "F47127M":
                            case "F45110":
                            case "F47127":
                            case "F49127":
                            case "F15607":
                            case "F55128":
                            case "F55129":
                                //dom
                                // M10010B lblheader.Text = "S.O. Check";
                                // M10015B lblheader.Text = "S.O. Approval";    
                                //exp
                                // M11010B lblheader.Text = "S.O. Check";
                                // M11015B lblheader.Text = "S.O. Approval";    
                                mydoc = "SO. ";
                                myrjflag = "Y";
                                mytable = "somas";
                                if (HCID == "M10010B" || HCID == "M11010B" || HCID == "F47126" || HCID == "F49126" || HCID == "F49129" || HCID == "F55128")
                                {
                                    myappno = "check_by";
                                    myappdt = "check_dt";
                                }
                                if (HCID == "M10015B" || HCID == "M11015B" || HCID == "M10015A" || HCID == "M11015A" || HCID == "F49130" || HCID == "F49130" || HCID == "F47127" || HCID == "F45110" || HCID == "F47127M" || HCID == "F49127" || HCID == "F49130" || HCID == "F55129" || HCID == "F15607")
                                {
                                    myappno = "app_by";
                                    myappdt = "app_dt";
                                }
                                if (HCID == "M10015A" || HCID == "M11015A")
                                {
                                    mytable = "somasp";
                                }

                                if (HCID == "M09028")
                                {
                                    mytable = "somasq";
                                }
                                if (HCID == "F47127M")
                                {
                                    mytable = "somasm";
                                }
                                if (HCID == "F45110")
                                {
                                    mytable = "somasq";
                                }
                                if (HCID == "F15607") mytable = "WB_PORFQ";

                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    if (frm_cocd == "SAIA")
                                    {
                                        //myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                        myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    }
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                    if (signDsc) sign_dsc_background("F1004", frm_mbr + row.Cells[8].Text.Trim() + row.Cells[10].Text.Trim() + ";" + row.Cells[11].Text.Trim());
                                }
                                else if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate,shipmark='" + mydoc + " REJECTED (" + rej_rsn + ")',icat='" + myrjflag + "' where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F55160A":
                            case "F79109":
                                mytable = "WB_DRAWREC";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where trim(branchcd)||trim(type)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                else if (chk2.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='[C]" + frm_uname + "'," + myappdt + "=sysdate, FILENAME='" + rej_rsn + "' where trim(branchcd)||trim(type)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F81111":
                                mytable = "WB_LEVREQ";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where trim(branchcd)||trim(type)||trim(lrqno)||to_Char(lrqdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;

                            case "F10141":
                                mytable = "item";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where trim(icode) ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F10142":
                            case "F70201":
                            case "F70203":
                            case "F50051":
                                mytable = "itemosp";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (HCID == "F70201")
                                {
                                    myappno = "check_by";
                                    myappdt = "check_dAtE";
                                }
                                if (HCID == "F70203" || HCID == "F50051")
                                {
                                    myappno = "app_by";
                                    myappdt = "app_date";
                                }
                                if (HCID == "F70203" || HCID == "F70201" || HCID == "F50051")
                                {
                                    mytable = "VOUCHER";
                                }
                                if (chk1.Checked == true)
                                {
                                    if (HCID == "F70203")
                                    {
                                        myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate ,DRAMT=TFCDR, CRAMT=TFCCR where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                        fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                        if (signDsc) sign_dsc_background(frm_formID, row.Cells[8].Text.Trim());
                                    }
                                    if (HCID == "F50051")
                                    {
                                        myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                        fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                        if (signDsc) sign_dsc_background("F1006", row.Cells[8].Text.Trim());
                                    }
                                    else
                                    {
                                        myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                        fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                    }
                                }
                                if (HCID == "F50051")
                                {
                                    if (chk2.Checked == true)
                                    {
                                        doc_rej = doc_rej + 1;
                                        myquery = "update " + mytable + " set " + myappno + "='[R]" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                        fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                        send_mail(frm_cocd, frm_formID, "R", row.RowIndex.ToString());
                                    }
                                }
                                break;
                            case "F25122C":
                                //chl
                                if (chk1.Checked == true)
                                {
                                    SQuery = "update IVOUCHER set DSC_DTL='" + frm_uname + " " + DateTime.Now.ToString("dd/MM/yyyy") + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + row.Cells[8].Text.Trim().Substring(2, 2) + row.Cells[10].Text.Trim() + row.Cells[11].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                    if (signDsc) sign_dsc_background("F1007", frm_mbr + row.Cells[8].Text.Trim().Substring(2, 2) + row.Cells[10].Text.Trim() + row.Cells[11].Text.Trim());
                                }
                                break;
                            case "F25122M":
                                //mrr
                                if (chk1.Checked == true)
                                {
                                    SQuery = "update IVOUCHER set DSC_DTL='" + frm_uname + " " + DateTime.Now.ToString("dd/MM/yyyy") + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + row.Cells[8].Text.Trim().Substring(2, 2) + row.Cells[10].Text.Trim() + row.Cells[11].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                    if (signDsc) sign_dsc_background("F1002", frm_mbr + row.Cells[8].Text.Trim().Substring(2, 2) + row.Cells[10].Text.Trim() + row.Cells[11].Text.Trim());
                                }
                                break;
                            case "F10143":
                                mytable = "inspmst";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F45149":
                                mytable = "wb_lead_log";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(lrcno)||to_char(lrcdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    myquery = "update " + mytable + " set FILENAME='" + frm_uname + "->" + rej_rsn + "'," + myappno + "='-'," + myappdt + "=sysdate where branchcd||type||trim(lrcno)||to_char(lrcdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }

                                break;

                            case "F15176":
                                mytable = "appvendvch";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F47128":
                            case "F49128":
                            case "F15171":
                                mytable = "schedule";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(icode) ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;

                            case "F60176":
                                mytable = "wb_css_log";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(cssno)||to_char(cssdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F60181":
                                mytable = "wb_css_log";
                                myappno = "Fapp_by";
                                myappdt = "Fapp_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate,Last_action='Marked Complete',last_Actdt=to_date(sysdate,'dd/mm/yyyy') where branchcd||type||trim(cssno)||to_char(cssdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;

                            case "F60186":
                                mytable = "wb_css_act";
                                myappno = "app_by";
                                myappdt = "app_dt";

                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                    string Qlink2;
                                    Qlink2 = fgen.seek_iname(frm_qstr, frm_cocd, "select ent_by||'-'||act_Status As fstr from " + mytable + " where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'", "fstr");

                                    string Qlink;
                                    Qlink = fgen.seek_iname(frm_qstr, frm_cocd, "select branchcd||'CS'||cssno||to_char(Cssdt,'dd/mm/yyyy') As fstr from " + mytable + " where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'", "fstr");

                                    myquery = "update wb_Css_log set WORK_action='Action:" + Qlink2 + "' where branchcd||type||trim(cssno)||to_char(cssdt,'dd/mm/yyyy') ='" + Qlink + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                    myquery = "commit";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                }
                                break;
                            case "F90109":
                                mytable = "wb_task_Log";
                                myappno = "app_by";
                                myappdt = "app_dt";

                                if (chk1.Checked == true)
                                {
                                    string Qlink3;
                                    mhd = "select trim(nvl(curr_Stat,'-')) As fstr from " + mytable + " where branchcd||type||trim(trcno)||to_char(trcdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    Qlink3 = fgen.seek_iname(frm_qstr, frm_cocd, mhd, "fstr");
                                    if (Qlink3 == "-" || Qlink3 == "0")
                                    {
                                        myquery = "update " + mytable + " set Last_action='Task-Close',Curr_stat='Task-Close',task_close='Y'," + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate,last_actdt=sysdate where branchcd||type||trim(TRCno)||to_char(TRCdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    }
                                    else
                                    {
                                        myquery = "update " + mytable + " set task_close='Y'," + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(TRCno)||to_char(TRCdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    }

                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);


                                    myquery = "commit";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                }
                                break;

                            case "F94106":
                                mytable = "wb_STl_log";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(STlno)||to_char(STl    dt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F35110":
                            case "F35106A":
                                mytable = "costestimate";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F35111":// job order closure / re-call                                
                                mytable = "BUDGMST";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set JOBCARDRQD='N', REQ_CLOSEdBY='" + frm_uname + " " + vardate + "', REQ_CL_RSN='" + rej_rsn + "' where trim(solink)||trim(srno)||trim(actualcost)||trim(acode)||trim(icode) ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    myquery = "update " + mytable + " set JOBCARDRQD='Y', REQ_CLOSEdBY='-', REQ_CL_RSN='-' where trim(solink)||trim(srno)||trim(actualcost)||trim(acode)||trim(icode) ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F96106":
                                mytable = "wb_DSL_log";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(DSLno)||to_char(DSLdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "F97106":
                                mytable = "wb_cam_log";
                                myappno = "app_by";
                                myappdt = "app_dt";
                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(camno)||to_char(camdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;

                            case "S05005B":
                                mytable = "cquery_reg";
                                myappno = "clo_by";
                                myappdt = "clo_dt";

                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "S06005B":
                                mytable = "cquery_alt";
                                myappno = "clo_by";
                                myappdt = "clo_dt";

                                if (chk1.Checked == true)
                                {
                                    myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);

                                    string Qlink;
                                    Qlink = fgen.seek_iname(frm_qstr, frm_cocd, "select qry_link from " + mytable + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + row.Cells[8].Text.Trim() + "'", "qry_link");

                                    mytable = "cquery_reg";

                                    myquery = "update " + mytable + " set Last_action='Cleared by " + frm_uname + " on '||sysdate where trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + Qlink + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, myquery);
                                }
                                break;
                            case "M02040":
                                //lblheader.Text = "Purch Sch. Approval";
                                break;
                            case "M09008":
                                //lblheader.Text = "Lead Approval";
                                break;
                            case "M10024":
                                //lblheader.Text = "Sales Sch. Approval";
                                break;
                            case "F10051":
                                if (chk1.Checked == true || chk2.Checked == true)
                                {
                                    if (frm_cocd == "SEL")
                                    {
                                        string userName = ((TextBox)row.FindControl("txtreason")).Text.Trim();
                                        col1 = "";
                                        if (chk1.Checked == true) col1 = "[A]";
                                        else if (chk2.Checked == true) col1 = "[R]";
                                        fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch set app_by='" + col1 + "" + frm_uname + "',app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') , col15='" + userName.Split('~')[0] + "', col16='" + userName.Split('~')[1] + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'");
                                        send_mail(frm_cocd, frm_formID, col1, row.RowIndex.ToString());
                                        send_msg(frm_cocd, frm_formID, col1, row.RowIndex.ToString());
                                    }
                                    else
                                    {
                                        if (chk1.Checked == true)
                                        {
                                            SQuery = "update scratch set app_by='[A]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                        }
                                        else if (chk2.Checked == true)
                                        {
                                            doc_rej = doc_rej + 1;
                                            SQuery = "update scratch set app_by='[R]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                        }
                                    }
                                }
                                break;
                            case "F10056":
                                if (chk1.Checked == true || chk2.Checked == true)
                                {
                                    string userName = ((TextBox)row.FindControl("txtreason")).Text.Trim();
                                    col1 = "";
                                    if (chk1.Checked == true) col1 = "[A]";
                                    else if (chk2.Checked == true)
                                    {
                                        col1 = "[R]";
                                        doc_rej = doc_rej + 1;
                                    }
                                    fgen.execute_cmd(frm_qstr, frm_cocd, "update ENQ_MAST set app_by='" + col1 + "" + frm_uname + "',app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') , TCOL21='" + userName.Split('~')[0] + "', TCOL22='" + userName.Split('~')[1] + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||TRIM(UPPER(ITEM))='" + row.Cells[8].Text.Trim() + "'");
                                    send_mail(frm_cocd, frm_formID, col1, row.RowIndex.ToString());
                                    send_msg(frm_cocd, frm_formID, col1, row.RowIndex.ToString());
                                }
                                break;
                            case "F20233":
                                if (chk1.Checked == true || chk2.Checked == true)
                                {
                                    col1 = "";
                                    if (chk1.Checked == true) col1 = "[A]";
                                    else if (chk2.Checked == true)
                                    {
                                        doc_rej = doc_rej + 1;
                                        col1 = "[R]";
                                    }
                                    cond = "";
                                    if (smsModule == "Y")
                                    {
                                        otp = "";
                                        mobileno = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COL23 FROM SCRATCH2 WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY')='" + row.Cells[8].Text.Trim() + "'", "COL23");
                                        otp = fgen.gen_otp(frm_qstr, frm_cocd);
                                        cond = ", col28='" + otp + "'";
                                    }

                                    fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch2 set app_by='" + col1 + "" + frm_uname + "',app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') " + cond + " where branchcd||type||trim(vchnum)||to_char(vchdate,'ddmmyyyy')='" + row.Cells[8].Text.Trim() + "'");


                                    if (smsModule == "Y")
                                    {
                                        col2 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(col16) as name from scratch2 where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + row.Cells[8].Text.Trim() + "')", "name");
                                        fgen.send_sms(frm_qstr, frm_cocd, mobileno, "Dear " + col2 + ", Welcome to " + frm_cocd + ", Please show this OTP " + otp + " at the Gate.", frm_uname);
                                    }
                                }
                                break;
                            case "F20235":
                                if (chk1.Checked == true || chk2.Checked == true)
                                    fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch2 set app_by='" + frm_uname + "',app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') , COL38='" + DateTime.Now.ToString("HH:mm") + "', reason='" + rej_rsn + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'ddmmyyyy')='" + row.Cells[8].Text.Trim() + "'");
                                break;
                            case "W90108":
                                col1 = "";
                                if (chk1.Checked == true) col1 = "[A]";
                                else if (chk2.Checked == true)
                                {
                                    col1 = "[R]";
                                    doc_rej = doc_rej + 1;
                                }
                                if (chk1.Checked == true || chk2.Checked == true)
                                {
                                    if (frm_cocd == "TEST") fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch set COL38='" + DateTime.Now.ToString("HH:mm") + "', col4='" + rej_rsn + "', col3='" + col1 + "" + frm_uname + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'ddmmyyyy')='" + row.Cells[8].Text.Trim() + "'");
                                    else fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch2 set app_by='" + col1 + "" + frm_uname + "',app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') , COL38='" + DateTime.Now.ToString("HH:mm") + "', reason='" + rej_rsn + "'  where branchcd||type||trim(vchnum)||to_char(vchdate,'ddmmyyyy')='" + row.Cells[8].Text.Trim() + "'");
                                    #region sent mail
                                    System.Text.StringBuilder msb = new System.Text.StringBuilder();
                                    msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                                    if (chk1.Checked == true)
                                    {
                                        msb.Append("For your kind information " + frm_uname + " has approved the action taken by you for this task ");
                                        msb.Append(row.Cells[9].Text + " Dt. " + row.Cells[10].Text + " <br/>");
                                        msb.Append("This task is completed & closed. <br/>");
                                    }
                                    else if (chk2.Checked == true)
                                    {
                                        doc_rej = doc_rej + 1;
                                        msb.Append("For your kind information " + frm_uname + " has refused the action taken by you for this task ");
                                        msb.Append(row.Cells[9].Text + " Dt. " + row.Cells[10].Text + " <br/>");
                                        msb.Append(rej_rsn.Trim() + " (This is the additional comment given by the assignor.) <br/>");
                                        msb.Append("This task is incomplete & open. <br/>");
                                    }
                                    msb.Append("</table><br/><br/>");
                                    msb.Append("<br>===========================================================<br>");
                                    msb.Append("<br>This Report is Auto generated from the Tejaxo ERP.");
                                    msb.Append("<br>The above details are to be best of information and data available to the ERP system.");
                                    msb.Append("<br>Errors or Omissions if any are regretted.");
                                    msb.Append("Thanks and Regards,<br/>");
                                    msb.Append("" + fgenCO.chk_co(frm_cocd) + "");
                                    msb.Append("</body></html>");
                                    string subje = "Approval/Closer Email";
                                    fgen.send_mail(frm_cocd, "Tejaxo ERP", row.Cells[13].Text, "", "", subje, msb.ToString());
                                    #endregion
                                }
                                break;
                            case "W90109":
                                col1 = "";
                                if (chk1.Checked == true) col1 = "[A]";
                                else if (chk2.Checked == true)
                                {
                                    col1 = "[R]";
                                    doc_rej = doc_rej + 1;
                                }
                                if (chk1.Checked == true || chk2.Checked == true)
                                {
                                    fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch set COL38='" + DateTime.Now.ToString("HH:mm") + "', col6='" + rej_rsn + "', col3='" + col1 + "" + frm_uname + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'ddmmyyyy')='" + row.Cells[8].Text.Trim() + "'");
                                }
                                break;
                            //case "F99164": // DUE TO PAGENATION SAVING WAS NOT WORKING RIGHT SO CORRECT SAVING IS WRITTEN ABOVE
                            //    col1 = "";
                            //    if (chk1.Checked == true)
                            //    {
                            //        if (index == 0)
                            //        {
                            //            SQuery = "DELETE FROM DSK_WCONFIG WHERE USERID='" + frm_UserID + "' AND USERNAME='" + frm_uname + "'";
                            //            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                            //        }
                            //        SQuery = "INSERT INTO DSK_WCONFIG (BRANCHCD,TYPE,VCHNUM,VCHDATE,SRNO,USERID,USERNAME,OBJ_NAME) VALUES ('" + frm_mbr + "','80','" + row.Cells[9].Text.Trim() + "',to_date('" + row.Cells[10].Text.Trim() + "','dd/mm/yyyy'),1,'" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_UPI") + "','" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_UPN") + "','" + row.Cells[11].Text.Trim() + "') ";
                            //        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                            //    }
                            //    break;

                            case "F47320":
                                col1 = "";
                                if (chk1.Checked == true)
                                {
                                    // enquiry or ecn
                                    SQuery = "update wb_sorfq set app_by ='C' , app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                    // respond foundry
                                    SQuery = "update wb_sorfq set app_by ='C' , app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where trim(pordno)='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                    // machine shop
                                    SQuery = "update wb_sorfq set app_by ='C' , app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where trim(pbasis)='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                    // costing sheet
                                    SQuery = "update wb_cacost set app_by ='C' , app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where trim(pbasis)='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                    // final quotation
                                    SQuery = "update somasq set app_by ='C' , app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where trim(pbasis2)='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                break;

                            case "F47323":
                                col1 = "";
                                if (chk1.Checked == true)
                                {
                                    SQuery = "update somasq set app_by ='[A]" + frm_uname + "' , app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                else if (chk1.Checked == true)
                                {
                                    SQuery = "update somasq set app_by ='[R]" + frm_uname + "' , app_dt=to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),desc_='" + rej_rsn + "' where trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                break;

                            case "F81104":
                                if (chk1.Checked == true)
                                {
                                    SQuery = "update WB_LEVREQ set app_by='[A]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where branchcd||type||trim(LRQNO)||to_char(LRQDT,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                else if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    SQuery = "update WB_LEVREQ set app_by='[R]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),oremarks='" + rej_rsn.ToUpper() + "' where branchcd||type||trim(LRQNO)||to_char(LRQDT,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                if (chk1.Checked == true || chk2.Checked == true)
                                {
                                    if (chk1.Checked == true) col1 = "Approved";
                                    else if (chk2.Checked == true) col1 = "Rejected";
                                    send_mail(frm_cocd, frm_formID, col1, row.RowIndex.ToString());
                                }
                                break;

                            case "F81511":
                                if (chk1.Checked == true)
                                {
                                    SQuery = "update wb_payloan set app_by='[A]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                else if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    SQuery = "update wb_payloan set app_by='[R]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),rej_remarks='" + rej_rsn + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                if (chk1.Checked == true || chk2.Checked == true)
                                {
                                    if (chk1.Checked == true) col1 = "Approved";
                                    else if (chk2.Checked == true) col1 = "Rejected";
                                    send_mail(frm_cocd, frm_formID, col1, row.RowIndex.ToString());
                                }
                                break;

                            case "F85145":
                                if (chk1.Checked == true)
                                {
                                    SQuery = "update empmas set appr_by='[A]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where branchcd||trim(grade)||trim(empcode)='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                else if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    SQuery = "update empmas set appr_by='[R]" + frm_uname + "',app_dt=to_date('" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') where branchcd||trim(grade)||trim(empcode)='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                break;

                            case "F85143":
                                if (chk1.Checked == true)
                                {
                                    mhd = "select A.ER1,A.ER2,A.ER3,A.ER4,A.ER5,A.ER6,A.ER7,A.ER8,A.ER9,A.ER10,A.ER11,A.ER12,A.ER13,A.ER14,A.ER15,A.ER16,A.ER17,A.ER18,A.ER19,A.ER20 from empmas a where branchcd||trim(grade)||trim(empcode)='" + row.Cells[8].Text.Trim() + "'";
                                    dt = new DataTable();
                                    dt = fgen.getdata(frm_qstr, frm_cocd, mhd);

                                    if (dt.Rows.Count > 0)
                                    {
                                        SQuery = "update payincr set EMPIMG='[A]" + frm_uname + "|" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "' where branchcd||trim(grade)||trim(empcode)='" + row.Cells[8].Text.Trim() + "'";
                                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                        mq0 = "update empmas set ER1=" + (fgen.make_double(row.Cells[15].Text.Trim()) + fgen.make_double(dt.Rows[0]["er1"].ToString().Trim())) + ",ER2=" + (fgen.make_double(row.Cells[16].Text.Trim()) + fgen.make_double(dt.Rows[0]["er2"].ToString().Trim())) + ",ER3=" + (fgen.make_double(row.Cells[17].Text.Trim()) + fgen.make_double(dt.Rows[0]["er3"].ToString().Trim())) + ",ER4=" + (fgen.make_double(row.Cells[18].Text.Trim()) + fgen.make_double(dt.Rows[0]["er4"].ToString().Trim())) + ",ER5=" + (fgen.make_double(row.Cells[19].Text.Trim()) + fgen.make_double(dt.Rows[0]["er5"].ToString().Trim())) + ",ER6=" + (fgen.make_double(row.Cells[20].Text.Trim()) + fgen.make_double(dt.Rows[0]["er6"].ToString().Trim())) + ",ER7=" + (fgen.make_double(row.Cells[21].Text.Trim()) + fgen.make_double(dt.Rows[0]["er7"].ToString().Trim())) + ",ER8=" + (fgen.make_double(row.Cells[22].Text.Trim()) + fgen.make_double(dt.Rows[0]["er8"].ToString().Trim())) + ",ER9=" + (fgen.make_double(row.Cells[23].Text.Trim()) + fgen.make_double(dt.Rows[0]["er9"].ToString().Trim())) + ",ER10=" + (fgen.make_double(row.Cells[24].Text.Trim()) + fgen.make_double(dt.Rows[0]["er10"].ToString().Trim())) + ",ER11=" + (fgen.make_double(row.Cells[25].Text.Trim()) + fgen.make_double(dt.Rows[0]["er11"].ToString().Trim())) + ",ER12=" + (fgen.make_double(row.Cells[26].Text.Trim()) + fgen.make_double(dt.Rows[0]["er12"].ToString().Trim())) + ",ER13=" + (fgen.make_double(row.Cells[27].Text.Trim()) + fgen.make_double(dt.Rows[0]["er13"].ToString().Trim())) + ",ER14=" + (fgen.make_double(row.Cells[28].Text.Trim()) + fgen.make_double(dt.Rows[0]["er14"].ToString().Trim())) + ",ER15=" + (fgen.make_double(row.Cells[29].Text.Trim()) + fgen.make_double(dt.Rows[0]["er15"].ToString().Trim())) + ",ER16=" + (fgen.make_double(row.Cells[30].Text.Trim()) + fgen.make_double(dt.Rows[0]["er16"].ToString().Trim())) + ",ER17=" + (fgen.make_double(row.Cells[31].Text.Trim()) + fgen.make_double(dt.Rows[0]["er17"].ToString().Trim())) + ",ER18=" + (fgen.make_double(row.Cells[32].Text.Trim()) + fgen.make_double(dt.Rows[0]["er18"].ToString().Trim())) + ",ER19=" + (fgen.make_double(row.Cells[33].Text.Trim()) + fgen.make_double(dt.Rows[0]["er19"].ToString().Trim())) + ",ER20=" + (fgen.make_double(row.Cells[34].Text.Trim()) + fgen.make_double(dt.Rows[0]["er20"].ToString().Trim())) + " where branchcd||trim(grade)||trim(empcode)='" + row.Cells[8].Text.Trim() + "'";
                                        fgen.execute_cmd(frm_qstr, frm_cocd, mq0);
                                    }
                                }
                                else if (chk2.Checked == true)
                                {
                                    doc_rej = doc_rej + 1;
                                    SQuery = "update payincr set EMPIMG='[R]" + frm_uname + "|" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "' where branchcd||trim(grade)||trim(empcode)='" + row.Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                                break;
                        }
                        index++;
                        if (chk1.Checked == true) totApp++;
                        if (chk2.Checked == true) totRej++;
                    }

                    if (HCID == "F15161" || HCID == "M02032" || HCID == "M10010B" || HCID == "M11010B" || HCID == "F15165")
                    {

                        fgen.msg("-", "AMSG", "Document Checking Successfully completed");
                    }

                    else
                    {
                        if (doc_rej > 0)
                        {
                            if (totApp > 0 && totRej > 0)
                                fgen.msg("-", "AMSG", "Document Approval / Refusal Successfully completed.'13'Total rows approved : " + totApp + "'13'Total rows refused : " + totRej);
                            else fgen.msg("-", "AMSG", "Document Refusal Successfully completed.'13'Total rows refused : " + totRej);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Document Approval Successfully completed'13'Total rows approved : " + totApp + "");
                        }

                    }

                    enablectrl(); sg1.DataSource = null; sg1.DataBind(); sg1.Visible = false;
                    fgen.DisableForm(this.Controls); btnnew.Focus();
                }
            }

            if (lbllink.Value.Length > 0)
            {
                lbllink.Value = lbllink.Value.TrimStart('~');
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ss", "openLink();", true);
            }
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //return;
            HCID = frm_formID;
            sg1.Columns[0].HeaderStyle.Width = 30;
            e.Row.Cells[0].Width = 30;
            sg1.HeaderRow.Cells[0].Style["text-align"] = "center";
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            sg1.Columns[1].HeaderStyle.Width = 30;
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            sg1.HeaderRow.Cells[1].Style["text-align"] = "center";

            //btn2  btnv1

            if (frm_formID == "F15161" || frm_formID == "F15162" || frm_formID == "F47126" || frm_formID == "F49126" || frm_formID == "F49130" || frm_formID == "F49129" || frm_formID == "F45110" || frm_formID == "F47127M" || frm_formID == "F47127" || frm_formID == "F49127" ||
                frm_formID == "F70201" || frm_formID == "F70203" || frm_formID == "F50051")
            {

            }
            else
            {
                e.Row.Cells[4].CssClass = "hidden";
                sg1.Columns[4].HeaderStyle.CssClass = "hidden";
            }
            //btn3 btnv2
            if (frm_formID == "F10051" || frm_formID == "F70201" || frm_formID == "F70203" || frm_formID == "F50051*")
            {
                if (frm_cocd != "SEL" && frm_formID == "F10051")
                {
                    e.Row.Cells[5].CssClass = "hidden";
                    sg1.Columns[5].HeaderStyle.CssClass = "hidden";
                }
            }
            else
            {
                e.Row.Cells[5].CssClass = "hidden";
                sg1.Columns[5].HeaderStyle.CssClass = "hidden";
            }

            //btn4 btnv3 (v3)
            if (frm_formID == "F70203" || frm_formID == "F70201" || frm_formID == "F50051" || frm_formID == "F15166" || frm_formID == "F15607" || frm_formID == "F45110" || frm_formID == "F47127M" || frm_formID == "F47127" || frm_formID == "F25122C" ||
                frm_formID == "F25122M" || frm_formID == "F55160A" || frm_formID == "F79109")
            {
                if (frm_formID == "F70203" || frm_formID == "F70201" || frm_formID == "F79109")
                {
                }
                else
                    ((ImageButton)e.Row.FindControl("btnv4")).Visible = false;
            }
            else
            {
                e.Row.Cells[6].CssClass = "hidden";
                sg1.Columns[6].HeaderStyle.CssClass = "hidden";
            }

            switch (HCID)
            {
                case "S05005B":
                case "S06005B":
                case "F47162":
                case "M11020B":
                case "M09008":
                case "M09028":
                case "M10024":
                case "M10015A":
                case "M11015A":
                case "M02040":

                case "M10010B":
                case "M10015B":
                case "M11010B":
                case "M11015B":
                case "F10141":
                case "F10142":
                case "F10143":
                case "F45149":
                case "F15161":
                case "F15162":
                case "F15165":
                case "F15166":
                case "F15607":
                case "F35110":
                case "F35111":// job order closure / re-call
                case "F81111":
                case "F15171":
                case "F15176":

                case "F47126":
                case "F49126":
                case "F49129":
                case "F49130":

                case "F47127M":
                case "F45110":

                case "F47127":
                case "F49127":

                case "F47128":
                case "F49128":


                case "F55128":
                case "F55129":

                case "M02032":
                case "M02036":
                case "F15210":
                case "F15211":
                case "M02046":
                case "F60176":
                case "F60181":
                case "F60186":
                case "F90109":
                case "F94106":
                case "F96106":
                case "F97106":
                case "F35106A":
                case "F70201":
                case "F70203":
                case "F50051":
                case "F25122C":
                case "F25122M":
                case "F55160A":
                case "F79109":
                    //ok colm
                    if (HCID == "F35106A")
                    {
                        e.Row.Cells[1].CssClass = "hidden";
                        sg1.Columns[1].HeaderStyle.CssClass = "hidden";
                    }

                    if (HCID == "S06005B" || HCID == "S05005B" || HCID == "M02040" || HCID == "M09008" || HCID == "M10015A" || HCID == "M11015A" || HCID == "M09028" ||
                        HCID == "M10010B" || HCID == "M10015B" || HCID == "M11010B" || HCID == "M11015B" || HCID == "M10024" || HCID == "F25122C" || HCID == "F25122M")
                    {
                        //rej colm
                        e.Row.Cells[1].CssClass = "hidden";
                        sg1.Columns[1].HeaderStyle.CssClass = "hidden";
                        //remarks colm
                        e.Row.Cells[7].CssClass = "hidden";
                        sg1.Columns[7].HeaderStyle.CssClass = "hidden";
                    }

                    //completed Dt colm
                    sg1.Columns[2].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[2].CssClass = "hidden";

                    if (HCID == "F10141" || HCID == "F10142*" || HCID == "F10143*" || HCID == "F15171" || HCID == "F15176" || HCID == "F60176" || HCID == "F60181" || HCID == "F60186" || HCID == "F90109" || HCID == "F94106" || HCID == "F96106" || HCID == "F97106" || HCID == "F70203")
                    {
                        //View Doc
                        if (HCID != "F60186" && HCID != "F70203" && HCID != "F70201")
                        {
                            e.Row.Cells[3].CssClass = "hidden";
                            sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                        }
                        //No Chk
                        e.Row.Cells[1].CssClass = "hidden";
                        sg1.Columns[1].HeaderStyle.CssClass = "hidden";

                        //remarks colm
                        e.Row.Cells[7].CssClass = "hidden";
                        sg1.Columns[7].HeaderStyle.CssClass = "hidden";
                    }
                    if (HCID == "F50051")
                    {
                        e.Row.Cells[3].CssClass = "hidden";
                        sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                    }

                    //fstr colm                    
                    e.Row.Cells[9].CssClass = "hidden";
                    sg1.Columns[9].HeaderStyle.CssClass = "hidden";

                    if (HCID == "F15210" || HCID == "F15211" || HCID == "M02046" || HCID == "M11020B")
                    {
                        e.Row.Cells[0].CssClass = "hidden";
                        sg1.Columns[0].HeaderStyle.CssClass = "hidden";
                    }
                    if (HCID == "F55160A" || HCID == "F79109")
                    {
                        sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                        e.Row.Cells[3].CssClass = "hidden";
                    }
                    break;
                case "F20233":
                    e.Row.Cells[2].CssClass = "hidden";
                    sg1.Columns[2].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[3].CssClass = "hidden";
                    sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[7].CssClass = "hidden";
                    sg1.Columns[7].HeaderStyle.CssClass = "hidden";
                    break;
                case "F20235":
                    //No Chk
                    e.Row.Cells[1].CssClass = "hidden";
                    sg1.Columns[1].HeaderStyle.CssClass = "hidden";

                    e.Row.Cells[2].CssClass = "hidden";
                    sg1.Columns[2].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[3].CssClass = "hidden";
                    sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                    //e.Row.Cells[7].CssClass = "hidden";
                    //sg1.Columns[7].HeaderStyle.CssClass = "hidden";
                    break;
                case "F99164":
                    //No Chk
                    for (int h = 1; h < 8; h++)
                    {
                        e.Row.Cells[h].CssClass = "hidden";
                        sg1.Columns[h].HeaderStyle.CssClass = "hidden";
                    }

                    break;
                case "99001":
                    DateTime date = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date.ToString("yyyy-MM-dd");
                    e.Row.Cells[3].CssClass = "hidden";
                    sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[7].CssClass = "hidden";
                    sg1.Columns[7].HeaderStyle.CssClass = "hidden";
                    break;
                case "70002":
                    ViewState["OrigData"] = e.Row.Cells[14].Text;
                    if (e.Row.Cells[14].Text.Length >= 25)
                    {
                        e.Row.Cells[14].Text = e.Row.Cells[14].Text.Substring(0, 25) + "...";
                        e.Row.Cells[14].ToolTip = ViewState["OrigData"].ToString();
                    }
                    sg1.HeaderRow.Cells[2].Text = "Approved On";
                    DateTime date1 = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date1.ToString("yyyy-MM-dd");
                    e.Row.Cells[3].CssClass = "hidden";
                    sg1.Columns[3].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[7].CssClass = "hidden";
                    sg1.Columns[7].HeaderStyle.CssClass = "hidden";
                    break;
                case "F10051":
                case "F10056":
                    sg1.HeaderRow.Cells[5].Text = "Employee";
                    sg1.HeaderRow.Cells[7].Text = "Employee Name";
                    ((TextBox)(e.Row.FindControl("txtreason"))).ReadOnly = true;
                    e.Row.Cells[3].CssClass = "hidden";
                    sg1.Columns[3].HeaderStyle.CssClass = "hidden";

                    e.Row.Cells[5].CssClass = "GridviewScrollItem2";
                    sg1.Columns[5].HeaderStyle.CssClass = "GridviewScrollItem2";
                    break;

                case "F85145":
                    sg1.HeaderRow.Cells[2].Text = "Approved On";
                    date1 = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date1.ToString("yyyy-MM-dd");
                    break;

                case "F85143":
                    sg1.HeaderRow.Cells[2].Text = "Approved On";
                    date1 = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date1.ToString("yyyy-MM-dd");
                    break;

                case "W90109":
                    date1 = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date1.ToString("yyyy-MM-dd");
                    break;

                case "W90108":
                    date1 = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date1.ToString("yyyy-MM-dd");
                    break;
            }
            //fstr colm         
            e.Row.Cells[8].CssClass = "hidden";
            sg1.Columns[8].HeaderStyle.CssClass = "hidden";

            switch (HCID)
            {
                case "F15166":
                    int z = 0;
                    int i = 17;
                    //for (int i = z; i < e.Row.Cells.Count - 1; i++)
                    {
                        TableCell cell = e.Row.Cells[i];
                        cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                        cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                        cell.ToolTip = "You can click this cell to Check Rate history";
                        cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                        cell.BackColor = System.Drawing.Color.GreenYellow;
                    }
                    sg1.Columns[6].HeaderText = "DSC";

                    if (e.Row.Cells[18].Text.toDouble() > 0)
                    {
                        if (e.Row.Cells[18].Text.toDouble() < e.Row.Cells[17].Text.toDouble())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightPink;
                        }
                    }
                    if (frm_cocd != "STUD")
                    {
                        e.Row.Cells[6].CssClass = "hidden";
                        sg1.Columns[6].HeaderStyle.CssClass = "hidden";
                    }
                    break;
                case "F70201":
                case "F70203":
                    if (frm_cocd == "MEGH" || frm_cocd == "SDM")
                    {
                        sg1.HeaderRow.Cells[3].Text = "Voucher View";
                        sg1.Columns[3].HeaderStyle.Width = 50;
                        ((ImageButton)e.Row.FindControl("btnv1")).ToolTip = "Finance Voucher Print Preview";
                        sg1.HeaderRow.Cells[4].Text = "Paper Inspection View";
                        sg1.Columns[4].HeaderStyle.Width = 100;
                        ((ImageButton)e.Row.FindControl("btnv1")).ToolTip = "Paper Inspection Print Preview";
                        sg1.HeaderRow.Cells[5].Text = "MRR View";
                        sg1.HeaderRow.Cells[5].Text = "Scanned Bill View";
                        sg1.Columns[5].HeaderStyle.Width = 70;
                        ((ImageButton)e.Row.FindControl("btnv1")).ToolTip = "MRR Print Preview";
                        sg1.HeaderRow.Cells[6].Text = "Gate Inward View | &nbsp; MRR View";
                        sg1.Columns[6].HeaderStyle.Width = 180;
                        ((ImageButton)e.Row.FindControl("btnv3")).ToolTip = "Gate Inward View";
                        ((Label)e.Row.FindControl("lblSeprt")).Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |&nbsp;&nbsp;&nbsp;";
                        ((ImageButton)e.Row.FindControl("btnv4")).ToolTip = "MRR View";
                    }
                    else
                    {
                        sg1.Columns[4].HeaderText = "P.O";
                        sg1.Columns[5].HeaderText = "Atch";
                        sg1.Columns[6].HeaderText = "DSC";
                    }
                    break;
                case "F50051":
                    sg1.Columns[4].HeaderText = "Inv";
                    sg1.Columns[5].HeaderText = "S.O";
                    sg1.Columns[6].HeaderText = "DSC";
                    break;
                case "F47127M":
                case "F45110":
                case "F47127":
                case "F25122C":
                case "F25122M":
                    sg1.Columns[4].HeaderText = "Rate History";
                    sg1.Columns[6].HeaderText = "DSC";
                    break;
                case "F55160A":
                case "F79109":
                    sg1.HeaderRow.Cells[6].Text = "Artwork View &nbsp;|&nbsp; Upload File";
                    sg1.Columns[6].HeaderStyle.Width = 160;
                    ((ImageButton)e.Row.FindControl("btnv3")).ToolTip = "Artwork View";
                    ((Label)e.Row.FindControl("lblSeprt")).Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; | &nbsp;";
                    ((ImageButton)e.Row.FindControl("btnv4")).ToolTip = "Upload File";
                    break;
            }
        }
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ") Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        else
        {
            clearctrl();
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            enablectrl();
            sg1.DataSource = null;
            sg1.DataBind(); sg1.Visible = false; //dt.Dispose();
        }
    }
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string var = e.CommandName.ToString();
            int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
            int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
            hf1.Value = rowIndex.ToString();
            HCID = frm_formID;
            string mq0 = "";
            string mq1 = "";
            string mq2 = "";
            string mq3 = "";
            string party_cd = "";
            string part_cd = "";
            string xprd1 = "";
            string xprd2 = "";

            //print previews
            switch (var)
            {
                case "Show":
                    switch (HCID)
                    {
                        case "F10142":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim());
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10131" + "L");
                            fgen.fin_engg_reps(frm_qstr);
                            break;
                        case "F15210":
                        case "F15161":
                        case "F15162":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "60");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1003");
                            fgen.fin_purc_reps(frm_qstr);
                            break;
                        case "F15211":
                        case "F15165":
                        case "F15166":

                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
                            fgen.fin_purc_reps(frm_qstr);
                            break;
                        case "F15607":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15601");
                            fgen.fin_purc_reps(frm_qstr);
                            break;
                        case "F35106A":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F35101");
                            fgen.fin_prod_reps(frm_qstr);
                            break;
                        case "F47128":
                        case "F49128":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1015");
                            fgen.fin_smktg_reps(frm_qstr);
                            break;
                        case "F47126":
                        case "F49126":
                        case "F49129":
                        case "F49130":

                        case "F45110":
                        case "F47127M":
                        case "F47127":
                        case "F49127":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[11].Text.Trim() + sg1.Rows[rowIndex].Cells[12].Text.Trim() + "'");
                            if (frm_formID == "F47127M") fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47101");
                            else fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1005");
                            fgen.fin_smktg_reps(frm_qstr);
                            break;
                        case "F25122C":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR1", "");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR3", "");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1007");
                            fgen.fin_invn_reps(frm_qstr);
                            break;
                        case "F25122M":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR1", "");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR3", "");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1002");
                            fgen.fin_invn_reps(frm_qstr);
                            break;
                        case "F35110":
                        case "F35111":// job order closure / re-call
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F35101");
                            fgen.fin_prod_reps(frm_qstr);
                            break;

                        case "F10051":
                            //SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.col8 as machine_srno,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.col12 as guarantee,a.col13 as guarantee_dt,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' order by vdd desc,a.srno";
                            if (frm_cocd == "SEL")
                            {
                                SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.col8 as machine_srno,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.col12 as guarantee,a.col13 as guarantee_dt,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' order by vdd desc,a.srno";
                            }
                            else
                            {
                                SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' order by vdd desc,a.srno";
                            }
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Customer Request List", frm_qstr);
                            break;
                        case "F60186":
                            try
                            {
                                col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FILENAME||'^'||FILEPATH AS FSTR from WB_CSS_ACT where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'", "FSTR");
                                if (col2.Length > 5)
                                {
                                    string fileName = col2.Split('^')[0].ToString().Trim();
                                    filePath = col2.Split('^')[1].ToString().Trim();
                                    filePath = filePath.Substring(filePath.ToUpper().IndexOf("UPLOAD"), filePath.Length - filePath.ToUpper().IndexOf("UPLOAD"));
                                    Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                                    Session["FileName"] = fileName;
                                    Response.Write("<script>");
                                    Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                                    Response.Write("</script>");
                                }
                            }
                            catch { }
                            break;

                        case "F47323":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", sg1.Rows[rowIndex].Cells[8].Text.Trim());
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                            fgen.fin_smktg_reps(frm_qstr);
                            break;

                        case "F81104":
                            SQuery = "select a.lrqno as lv_req_no,to_char(a.lrqdt,'dd/mm/yyyy') as lv_req_dt,b.name,b.deptt_text as departemt,b.desg_text as designation,a.LEVFROM as from_dt,A.LEVUPTO as return_dt,a.CONT_NAME as alt_contact,A.CONT_NO as alt_contact_no,a.empcode,a.lv_time as leave_time,a.ret_time as return_time,a.tot_days as total_days,a.time_in_hrs, a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.lrqdt,'yyyymmdd') as vdd from  wb_levreq a,empmas b where a.branchcd||trim(a.empcode)=b.branchcd||trim(b.grade)||trim(b.empcode) and trim(a.branchcd)||trim(a.type)||trim(a.lrqno)||to_char(a.lrqdt,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' order by vdd desc,a.lrqno desc";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel(lblheader.Text, frm_qstr);
                            break;

                        case "F81511":
                            SQuery = "select a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.empcode,b.name as empname,a.grade,to_char(b.dtjoin,'dd/mm/yyyy') as joindt,to_char(a.inst_st_dt,'dd/mm/yyyy') as Installment_start_dt,a.deptt,nvl(dramt,0) as amt,nvl(a.cramt,0) as month,nvl(os_amt,0) as os_Amt,nvl(a.INSTAMT,0) as monthly_install,nvl(a.CURRSAL,0) as salry,a.remark,a.cur_loan,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt,a.edt_by,to_char(a.edt_dt,'dd/mm/yyyy') as edt_Dt,to_char(a.vchdate,'yyyymmdd') as vdd from wb_payloan a,empmas b where trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and trim(a.branchcd)||trim(a.type)||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' order by vdd,entry_no"; fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel(lblheader.Text, frm_qstr);
                            break;

                        case "F70201":
                        case "F70203":
                            frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70203");
                            fgen.fin_acct_reps(frm_qstr);
                            break;

                        case "F85145":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", sg1.Rows[rowIndex].Cells[12].Text.Trim());
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85132");
                            fgen.fin_pay_reps(frm_qstr);
                            break;

                        case "F85143":
                            SQuery = "SELECT A.EMPCODE,A.NAME AS EMPLOYEE_NAME,A.VCHDATE,A.INC_APP_DT,A.GRADE,T.NAME AS GRADE_NAME,A.ER1,A.ER2,A.ER3,A.ER4,A.ER5,A.ER6,A.ER7,A.ER8,A.ER9,A.ER10,A.ER11,A.ER12,A.ER13,A.ER14,A.ER15,A.ER16,A.ER17,A.ER18,A.ER19,A.ER20 from PAYINCR A,TYPE T WHERE TRIM(A.GRADE)=TRIM(T.TYPE1) AND T.ID='I' AND A.BRANCHCD||TRIM(A.GRADE)||TRIM(A.EMPCODE)='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'"; fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel(lblheader.Text, frm_qstr);
                            break;
                    }
                    break;
                case "btnv1":
                    switch (HCID)
                    {
                        case "F47126":
                        case "F49126":
                        case "F49129":
                        case "F49130":
                        case "F45110":
                        case "F47127M":
                        case "F47127":
                        case "F49127":
                            //SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' order by vdd desc,a.srno";

                            SQuery = "SELECT c.Aname as Customer,a.icode as erpcode,a.irate,a.cdisc as Disc,b.Cpartno AS Part_no,b.Cdrgno AS Drg_no,a.ent_by,a.ent_Dt,a.app_by,a.app_Dt,a.desc_ as Remarks,to_char(a.orddt,'dd/mm/yyyy') as Ord_Dt,a.ordno,to_char(a.orddt,'yyyymmdd') as VDD FROM somas  a, item b ,famst c WHERE trim(A.Acode)=trim(c.acode) and trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.icat<>'Y' and trim(nvl(a.app_by,'-'))!='-' and trim(a.icode) = '" + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(20, 8) + "' and length(Trim(nvl(b.deac_by,'-')))<=1 and length(Trim(nvl(b.hscode,'-')))>1  ORDER BY to_char(a.orddt,'yyyymmdd') desc  ";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Rate history For This Product", frm_qstr);
                            break;
                        case "F15210":
                            break;
                        case "F15161":
                        case "F15162":
                            string my_cyear = "";
                            my_cyear = "yr_" + frm_cDt1.Substring(6, 4);

                            xprd1 = " between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1";
                            xprd2 = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";

                            party_cd = sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(20, 8);
                            part_cd = sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(20, 8);

                            mq0 = "select b.Iname as Item_Name,b.Cpartno as Part_No,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,max(a.imin) As Min_lvl,max(qap) as Qa_pending,b.Unit,trim(a.icode) as Icode from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,0 as imin,0 as qap from ITEM where length(Trim(icode))>4 union all Select icode, " + my_cyear + " as opening,0 as cdr,0 as ccr,0 as clos,nvl(imin,0) as imin,0 as qap from ITEMBAL where branchcd='" + frm_mbr + "'  union all  ";
                            mq1 = "select icode,0 as op,0 as cdr,0 as ccr,0 as clos,0 as xmin,sum(iqty_chl) as qap from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprd2 + " and store='N' GROUP BY ICODE union all ";
                            mq2 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as xmin,0 as qap from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and store='Y' GROUP BY ICODE union all ";
                            mq3 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as xmin,0 as qap  from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and store='Y'  GROUP BY ICODE )a ,item b where trim(A.icode)=trim(B.icodE) and substr(A.icode,1,1) like '%' and substr(A.icode,1,8) like '" + part_cd + "%' group by b.iname,b.cpartno,b.unit,trim(a.icode),substr(a.icode,1,4)  order by substr(a.icode,1,4),b.iname";
                            SQuery = mq0 + mq1 + mq2 + mq3;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Stock Summary For This Item", frm_qstr);

                            break;
                        case "F15211":
                        case "F15165":
                        case "F15166":


                            break;

                        case "F10051":

                            break;
                        case "F60186":
                            //try
                            //{
                            //    col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FILENAME||'^'||FILEPATH AS FSTR from WB_CSS_ACT where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'", "FSTR");
                            //    if (col2.Length > 5)
                            //    {
                            //        string fileName = col2.Split('^')[0].ToString().Trim();
                            //        string filePath = col2.Split('^')[1].ToString().Trim();
                            //        filePath = filePath.Substring(filePath.ToUpper().IndexOf("UPLOAD"), filePath.Length - filePath.ToUpper().IndexOf("UPLOAD"));
                            //        Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                            //        Session["FileName"] = fileName;
                            //        Response.Write("<script>");
                            //        Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                            //        Response.Write("</script>");
                            //    }
                            //}
                            //catch { }
                            break;
                        case "F70201":
                        case "F70203":
                        case "F50051xxx":
                            if (HCID == "F50051")
                            {
                                SQuery = "UPDATE VOUCHER set APP_BY='" + frm_uname + "', APP_DATE=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, "F1006", frm_cDt1, sg1.Rows[rowIndex].Cells[8].Text.Trim());
                            }
                            else
                            {
                                if (frm_cocd == "MEGH" || frm_cocd == "SDM")
                                {
                                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(MRNNUM)||TO_cHAR(MRNDATE,'DD/MM/YYYY')||TRIM(ACODE) AS FSTR FROM VOUCHER WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + sg1.Rows[rowIndex].Cells[8].Text + "'  ", "FSTR");

                                    cond = "";
                                    if (sg1.Rows[rowIndex].Cells[8].Text.Substring(2, 2) == "56") cond = "07";
                                    else cond = "02";
                                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", cond);
                                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + cond + col1 + "'");
                                    col2 = frm_mbr + "10" + col1;
                                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY') AS FSTR FROM PAPINSP WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND TRIM(MRRNUM)||TO_CHAR(MRRDATE,'DD/MM/YYYY')||TRIM(ACODE)='" + col1 + "' ", "FSTR");
                                    if (col3 != "0")
                                    {
                                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col3);
                                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70203");
                                        fgen.fin_qa_reps(frm_qstr);
                                    }
                                    else
                                    {
                                        fgen.msg("-", "AMSG", "No Paper inspection report found!!");
                                        //SQuery = "SELECT trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Supplier,a.Invno,A.Refnum as chl_no,a.Iqty_chl as Supp_Qty,a.iqtyin+nvl(a.rej_rw,0) as Rcv_qty,a.acpt_ud as Acpt_qty,a.rej_Rw as Rejn,round((a.rej_Rw/a.iqty_chl)*100,2) as Rejn_percent,round((a.rej_Rw/a.iqty_chl)*1000000,2) as Rejn_PPM,a.ent_by,a.pname as insp_by,a.qcdate,a.purpose as rej_rmk,a.tc_no,a.btchno,to_char(A.vchdate,'yyyymmdd') as vdd from ivoucher a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')||trim(a.acode)='" + col2 + "' order by vdd desc,a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum  ";
                                        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                        //fgen.Fn_open_rptlevel("List of QA", frm_qstr);
                                    }
                                }
                                else
                                {
                                    SQuery = "UPDATE VOUCHER set APP_BY='" + frm_uname + "', APP_DATE=sysdate ,DRAMT=TFCDR, CRAMT=TFCCR where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                    fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, frm_formID, frm_cDt1, sg1.Rows[rowIndex].Cells[8].Text.Trim());
                                }
                            }
                            break;
                        case "F50051x":

                            //SQuery = "Select a.Vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,c.Aname as Customer,a.purpose  as Item_Name,a.exc_57f4 as Part_No,a.iqtyout as sale_Qty,a.Irate,a.ichgs as Disc,b.unit,b.hscode,a.Desc_,a.icode,a.ent_by,a.ent_Dt from " + "ivoucher" + " a, item b,famst c where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a.morder ";
                            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            //fgen.Fn_open_rptlevel("Invoice Detail", frm_qstr);

                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_MBR", frm_mbr);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(4, 16) + "'");

                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1006");
                            fgen.fin_sales_reps(frm_qstr);

                            break;
                    }
                    break;
                case "btnv2":
                    switch (HCID)
                    {
                        case "F10051":
                        case "F10056":
                            hffield.Value = "EMPLYEE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            SQuery = "SELECT type1 AS FSTR,name as USERNAME,type1 as USERID,acref as email,acref3 as mobileno FROM typegrp where id='SE' and branchcd!='DD' ORDER BY type1";
                            cond = "";
                            if (frm_cocd == "SEL") cond = " WHERE SUBSTR(DEPTT,1,1)='6' ";
                            SQuery = "SELECT USERID AS FSTR,USERNAME,USERID,EMAILID AS EMAIL,CONTACTNO FROM EVAS " + cond + " ORDER BY USERID,USERNAME";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("-", frm_qstr);
                            break;
                        case "F15162":

                            break;
                        case "F70201":
                        case "F70203":
                            col1 = sg1.Rows[rowIndex].Cells[8].Text.Trim();
                            SQuery = "Select a.MSGTXT AS IMAGEF from ATCHVCH a WHERE a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "') ";
                            filePath = "";
                            filePath = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "IMAGEF");
                            if (filePath.Length > 4)
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                            else
                                fgen.msg("No Attachment Found", "AMSG", "No File Attached against '13'Voucher Number - Date : " + (sg1.Rows[rowIndex].Cells[8].Text.Trim().Length > 10 ? sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(4, 6) + " - " + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(10, 10) : sg1.Rows[rowIndex].Cells[8].Text.Trim()));
                            break;
                    }
                    break;
                case "btnv3":
                    switch (HCID)
                    {



                        case "F50051":
                            if (HCID == "F50051")
                            {
                                SQuery = "UPDATE VOUCHER set APP_BY='" + frm_uname + "', APP_DATE=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, "F1006", frm_cDt1, sg1.Rows[rowIndex].Cells[8].Text.Trim());
                            }
                            else
                            {
                                if (frm_cocd == "MEGH" || frm_cocd == "SDM")
                                {
                                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(MRNNUM)||TO_cHAR(MRNDATE,'DD/MM/YYYY') AS FSTR FROM VOUCHER WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + sg1.Rows[rowIndex].Cells[8].Text + "'  ", "FSTR");

                                    cond = "";
                                    if (sg1.Rows[rowIndex].Cells[8].Text.Substring(2, 2) == "56") cond = "07";
                                    else cond = "02";
                                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", cond);
                                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + cond + col1 + "'");
                                    col2 = frm_mbr + cond + col1;
                                    //fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F30106");
                                    //fgen.fin_qa_reps(frm_qstr);

                                    SQuery = "SELECT trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Supplier,a.Invno,A.Refnum as chl_no,a.Iqty_chl as Supp_Qty,a.iqtyin+nvl(a.rej_rw,0) as Rcv_qty,a.acpt_ud as Acpt_qty,a.rej_Rw as Rejn,round((a.rej_Rw/a.iqty_chl)*100,2) as Rejn_percent,round((a.rej_Rw/a.iqty_chl)*1000000,2) as Rejn_PPM,a.ent_by,a.pname as insp_by,a.qcdate,a.purpose as rej_rmk,a.tc_no,a.btchno,to_char(A.vchdate,'yyyymmdd') as vdd from ivoucher a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')='" + col2 + "' order by vdd desc,a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum  ";
                                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                    fgen.Fn_open_rptlevel("List of QA", frm_qstr);
                                }
                                else
                                {
                                    SQuery = "UPDATE VOUCHER set APP_BY='" + frm_uname + "', APP_DATE=sysdate ,DRAMT=TFCDR, CRAMT=TFCCR where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                    fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, frm_formID, frm_cDt1, sg1.Rows[rowIndex].Cells[8].Text.Trim());
                                }
                            }
                            break;
                        case "F15166":
                            //SQuery = "update pomas set app_by='" + frm_uname + "', app_dt=sysdate,pbasis=to_Char(sysdate,'dd/mm/yyyy') where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'";
                            //fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);                        
                            fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, "F1004", frm_cDt1, frm_mbr + sg1.Rows[rowIndex].Cells[8].Text.Trim().Trim().Substring(2, 2) + sg1.Rows[rowIndex].Cells[10].Text.Trim() + ";" + sg1.Rows[rowIndex].Cells[11].Text.Trim());
                            break;
                        case "F45110":
                        case "F47127M":
                        case "F47127":
                            //so
                            //SQuery = "update somas set app_by='" + frm_uname + "', app_dt=sysdate,pbasis=to_Char(sysdate,'dd/mm/yyyy') where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'";
                            //fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                            fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, "F1005", frm_cDt1, frm_mbr + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + ";" + sg1.Rows[rowIndex].Cells[12].Text.Trim());
                            break;
                        case "F25122C":
                            //chl
                            SQuery = "update IVOUCHER set DSC_DTL='" + frm_uname + " " + DateTime.Now.ToString("dd/MM/yyyy") + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2) + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                            fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, "F1007", frm_cDt1, frm_mbr + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2) + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim());
                            break;
                        case "F25122M":
                            //mrr
                            SQuery = "update IVOUCHER set DSC_DTL='" + frm_uname + " " + DateTime.Now.ToString("dd/MM/yyyy") + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2) + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                            fgen.dPrintIE(frm_qstr, frm_cocd, frm_mbr, frm_uname, "F1002", frm_cDt1, frm_mbr + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2) + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim());
                            break;
                        case "F70201":
                        case "F70203":
                            if (frm_cocd == "MEGH" || frm_cocd == "SDM")
                            {
                                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "00");
                                cond = "";
                                if (sg1.Rows[rowIndex].Cells[8].Text.Substring(2, 2) == "56") cond = "07";
                                else cond = "02";

                                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(MRNNUM)||TO_cHAR(MRNDATE,'DD/MM/YYYY') AS FSTR FROM VOUCHER WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + sg1.Rows[rowIndex].Cells[8].Text + "'  ", "FSTR");
                                col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(GENUM)||TO_cHAR(GEDATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '" + cond + "%' AND TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + col1 + "'  ", "FSTR");

                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + col2 + "'");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1001");
                                fgen.fin_gate_reps(frm_qstr);
                            }
                            else
                            {
                                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "00");
                                cond = "";
                                if (sg1.Rows[rowIndex].Cells[8].Text.Substring(2, 2) == "56") cond = "07";
                                else cond = "02";

                                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(MRNNUM)||TO_cHAR(MRNDATE,'DD/MM/YYYY') AS FSTR FROM VOUCHER WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + sg1.Rows[rowIndex].Cells[8].Text + "'  ", "FSTR");
                                col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(POTYPE)||'~'||TRIM(PONUM)||TO_cHAR(PODATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '" + cond + "%' AND TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + col1 + "'  ", "FSTR");

                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col2.Split('~')[0]);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + col2.Split('~')[1] + "'");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
                                fgen.fin_purc_reps(frm_qstr);
                            }
                            break;
                        case "F55160A":
                        case "F79109":
                            hffield.Value = "DWN";
                            SQuery = "SELECT TRIM(f.msgtxt)||'~'||trim(f.MSGTO) as fstr,f.terminal as design_type,f.msgtxt as filename,b.aname as customer,c.iname as part_name,f.MSGFROM as activation,f.msgdt as srno FROM wb_drawrec a,atchvch f,famst b,item c where trim(a.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')=f.branchcd||f.type||trim(F.vchnum)||to_Char(f.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' AND A.TYPE='DE' AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(4, 16) + "' ORDER BY f.MSGDT ";
                            if (fgen.getOptionPW(frm_qstr, frm_cocd, "W2030", "OPT_ENABLE", frm_mbr) == "Y")
                                SQuery = "SELECT TRIM(f.msgtxt)||'~'||trim(f.MSGTO) as fstr,b.name as customer,c.name as part_name,f.msgtxt as filename,f.terminal as design_type,f.MSGFROM as activation,f.msgdt as srno FROM wb_drawrec a,atchvch f,typegrp b,typegrp c where trim(a.acode)=trim(B.type1) and b.id='C1' and trim(A.icode)=trim(c.type1) and c.id='P1' and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')=f.branchcd||f.type||trim(F.vchnum)||to_Char(f.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' AND A.TYPE='DE' AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(4, 16) + "' ORDER BY f.MSGDT ";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Drawing to download", frm_qstr);
                            break;
                    }
                    break;
                case "btnv4":
                    switch (HCID)
                    {
                        case "F70201":
                        case "F70203":
                            if (frm_cocd == "MEGH" || frm_cocd == "SDM")
                            {
                                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(MRNNUM)||TO_cHAR(MRNDATE,'DD/MM/YYYY') AS FSTR FROM VOUCHER WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + sg1.Rows[rowIndex].Cells[8].Text + "'  ", "FSTR");

                                cond = "";
                                if (sg1.Rows[rowIndex].Cells[8].Text.Substring(2, 2) == "56") cond = "07";
                                else cond = "02";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", cond);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + col1 + "'");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1002");
                                fgen.fin_invn_reps(frm_qstr);
                            }
                            else
                            {
                                SQuery = "Select a.msgtxt as imagef from ATCHVCH a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "') ";
                                mhd = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "IMAGEF");
                                if (mhd != "0")
                                {
                                    filePath = mhd;

                                    Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                                    Session["FileName"] = mhd;
                                    Response.Write("<script>");
                                    Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                                    Response.Write("</script>");
                                }
                            }
                            break;
                        case "F79109":
                            col1 = sg1.Rows[rowIndex].Cells[8].Text.Substring(4, 16);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "DE");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                            fgen.open_fileUploadPopup("Artwork Upload", frm_qstr);
                            break;
                    }
                    break;
            }
        }
        catch { }
    }
    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        SQuery = ViewState["Squery"].ToString();
        DataTable dt1 = new DataTable();
        dt1 = fgen.search_vip(frm_qstr, frm_cocd, SQuery, txtsearch.Text.Trim().ToUpper());
        if (dt1.Rows.Count > 0)
        {
            ViewState["sg1"] = dt1;

            dt = (DataTable)ViewState["sg1"];
            DataTable neWDt = dt.Copy();
            ViewState["sg1"] = neWDt;
            makeColNameAsMine(dt);
            sg1.DataSource = dt;
            sg1.DataBind();
            sg1.Visible = true;
            hideAndRenameCol();
        }
        else fgen.msg("-", "AMSG", "No Data Found Like'13'" + txtsearch.Text.Trim());
    }

    protected void tkrow_TextChanged(object sender, EventArgs e)
    {
        //fill_grid();
    }

    public void fill_grid()
    {
        hfqry.Value = "";
        dt = new DataTable();
        SQuery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        if (SQuery.Length > 5)
        {
            if (frm_cocd == "AVON")
            {
                if (SQuery.Contains("TEXT()"))
                {
                    SQuery = SQuery.Replace("TEXT()", "text()");
                }
            }
            dt = fgen.getdata(frm_qstr, frm_cocd, "select * from ( " + SQuery + " ) where rownum<=" + tkrow.Text.Trim() + "");
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "").ToString(), false);
        }
        else if (Session["send_dt"] != null)
        {
            dt = (DataTable)Session["send_dt"];
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "").ToString(), false);
        }

        sg1.DataSource = dt;
        sg1.DataBind();
        if (dt.Rows.Count > 0)
        {
            // datadiv.Visible = true; div2.Visible = false;
            lblTotcount.Text = "Total Rows : " + dt.Rows.Count;
        }
        else
        {
            // datadiv.Visible = false; div2.Visible = true;
        }
    }

    public void send_mail(string cocd, string formID, string appr_Status, string info)
    {
        string emailTo = "", emailCC = "", emailSubj = "";
        System.Text.StringBuilder stb = new System.Text.StringBuilder();
        switch (formID)
        {
            case "F10051":
            case "F10056":
                string headingNo = "Request No";
                if (formID == "F10056") headingNo = "Lead No";

                string username = ((TextBox)sg1.Rows[Convert.ToInt32(info)].FindControl("txtreason")).Text;
                stb.Append("<html><body>");
                stb.Append("Dear " + username.Split('~')[0].ToString() + ", <br><br>");
                stb.Append("" + headingNo + ". " + sg1.Rows[Convert.ToInt32(info)].Cells[9].Text.Trim() + ", Dated : " + sg1.Rows[Convert.ToInt32(info)].Cells[10].Text.Trim() + " has been assigned to you by " + frm_uname + "<br>");
                stb.Append("Machine Sr No. : " + sg1.Rows[Convert.ToInt32(info)].Cells[11].Text.Trim() + "<br>");
                stb.Append("Product : " + sg1.Rows[Convert.ToInt32(info)].Cells[14].Text.Trim() + "<br><br>");
                //stb.Append("To be Done By : " + ((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[14].FindControl("txtcompdt")).Text + "<br>");
                stb.Append("Thanks & Regards, <br>");
                stb.Append(fgenCO.chk_co(cocd) + "<br><br>");
                stb.Append("<b>Note: Please respond to concerned BUYER only as this is the system generated E-Mail. Buyer Name given in the pending details.</b><br>");
                emailSubj = "Customer Req No. #" + sg1.Rows[Convert.ToInt32(info)].Cells[9].Text.Trim() + " assigned to you";
                stb.Append("</body></html>");
                SQuery = "SELECT EMAILID||'~'||CONTACTNO AS EMAIL FROM EVAS WHERE TRIM(USERNAME)||'~'||TRIM(USERID)='" + username.Trim() + "'";
                col1 = fgen.seek_iname(frm_qstr, cocd, SQuery, "EMAIL");
                if (col1.Contains("~"))
                {
                    if (col1.Split('~')[0].Length > 2)
                    {
                        emailTo = col1.Split('~')[0].ToString();
                    }
                }
                emailCC = "";
                //emailTo = fgen.seek_iname(frm_qstr, cocd, "SELECT NVL(ACREF,'-') AS ACREF FROM TYPEGRP WHERE ID='SE' AND TRIM(NAME)||'~'||TRIM(TYPE1)='" + username.Trim() + "'", "ACREF");
                //emailCC = fgen.seek_iname(frm_qstr, cocd, "SELECT NVL(EMAILID,'-') AS ACREF FROM EVAS WHERE (USERNAME)='" + frm_uname.Trim() + "'", "ACREF");
                break;

            case "F81104":
                username = sg1.Rows[Convert.ToInt32(info)].Cells[23].Text;
                if (username.Trim().Substring(0, 1) == "E")
                {
                    username = fgen.seek_iname(frm_qstr, frm_cocd, "select name from empmas where branchcd||trim(empcode)='" + sg1.Rows[Convert.ToInt32(info)].Cells[23].Text.Split('E')[1] + "'", "name");
                    SQuery = "SELECT EMAIL FROM EMPMAS WHERE branchcd||trim(empcode)='" + sg1.Rows[Convert.ToInt32(info)].Cells[23].Text.Split('E')[1] + "'";
                    col1 = fgen.seek_iname(frm_qstr, cocd, SQuery, "EMAIL");
                }
                stb.Append("<html><body>");
                stb.Append("Dear " + username + ", <br><br>");
                stb.Append("Your Leave Request No . " + sg1.Rows[Convert.ToInt32(info)].Cells[9].Text.Trim() + ", Dated : " + sg1.Rows[Convert.ToInt32(info)].Cells[10].Text.Trim() + " has been " + appr_Status + " by " + frm_uname + "<br><br>");
                if (appr_Status == "Rejected")
                {
                    stb.Append("Due to the following Reason : " + ((TextBox)sg1.Rows[Convert.ToInt32(info)].FindControl("txtreason")).Text.ToUpper() + "<br><br>");
                }
                stb.Append("Thanks & Regards, <br>");
                stb.Append(fgenCO.chk_co(cocd) + "<br><br>");
                stb.Append("<b>Note: This is the system generated E-Mail. Please do not Reply on this mail.</b><br>");
                emailSubj = "Leave Request No. #" + sg1.Rows[Convert.ToInt32(info)].Cells[9].Text.Trim() + " " + appr_Status;
                stb.Append("</body></html>");
                emailTo = col1.ToString();
                SQuery = "SELECT EMAILID AS EMAIL FROM EVAS WHERE TRIM(USERNAME)='" + frm_uname.Trim() + "'";
                emailCC = fgen.seek_iname(frm_qstr, cocd, SQuery, "EMAIL");
                break;

            case "F81511":
                username = sg1.Rows[Convert.ToInt32(info)].Cells[24].Text;
                if (username.Trim().Substring(0, 1) == "E")
                {
                    username = fgen.seek_iname(frm_qstr, frm_cocd, "select name from empmas where branchcd||trim(empcode)='" + sg1.Rows[Convert.ToInt32(info)].Cells[24].Text.Split('E')[1] + "'", "name");
                    SQuery = "SELECT EMAIL FROM EMPMAS WHERE branchcd||trim(empcode)='" + sg1.Rows[Convert.ToInt32(info)].Cells[24].Text.Split('E')[1] + "'";
                    col1 = fgen.seek_iname(frm_qstr, cocd, SQuery, "EMAIL");
                }
                stb.Append("<html><body>");
                stb.Append("Dear " + username + ", <br><br>");
                stb.Append("Your Loan Request No . " + sg1.Rows[Convert.ToInt32(info)].Cells[9].Text.Trim() + ", Dated : " + sg1.Rows[Convert.ToInt32(info)].Cells[10].Text.Trim() + " has been " + appr_Status + " by " + frm_uname + "<br><br>");
                if (appr_Status == "Rejected")
                {
                    stb.Append("Due to the following Reason : " + ((TextBox)sg1.Rows[Convert.ToInt32(info)].FindControl("txtreason")).Text.ToUpper() + "<br><br>");
                }
                stb.Append("Thanks & Regards, <br>");
                stb.Append(fgenCO.chk_co(cocd) + "<br><br>");
                stb.Append("<b>Note: This is the system generated E-Mail. Please do not Reply on this mail.</b><br>");
                emailSubj = "Loan Request No. #" + sg1.Rows[Convert.ToInt32(info)].Cells[9].Text.Trim() + " " + appr_Status;
                stb.Append("</body></html>");
                emailTo = col1.ToString();
                SQuery = "SELECT EMAILID AS EMAIL FROM EVAS WHERE TRIM(USERNAME)='" + frm_uname.Trim() + "'";
                emailCC = fgen.seek_iname(frm_qstr, cocd, SQuery, "EMAIL");
                break;

            case "F50051":
                stb.Append("<html><body>");
                stb.Append("Dear Sir/Madam, <br><br>");
                stb.Append("Invoice No . " + sg1.Rows[Convert.ToInt32(info)].Cells[10].Text.Trim() + ", Dated : " + sg1.Rows[Convert.ToInt32(info)].Cells[11].Text.Trim() + " has been Rejected by " + frm_uname + "<br><br>");
                stb.Append("Due to the following Reason : " + ((TextBox)sg1.Rows[Convert.ToInt32(info)].FindControl("txtreason")).Text.ToUpper() + "<br><br>");

                stb.Append("Thanks & Regards, <br>");
                stb.Append(fgenCO.chk_co(cocd) + "<br><br>");

                stb.Append("<b>Note: This is the system generated E-Mail. Please do not Reply on this mail.</b><br>");
                emailSubj = "Invoice Rejected : Invoice No. #" + sg1.Rows[Convert.ToInt32(info)].Cells[10].Text.Trim();
                stb.Append("</body></html>");
                string mhd = fgen.seek_iname(frm_qstr, cocd, "select type1,name,replace(nvl(acref,'-'),';',''',''') as COL1,nvl(lineno,1) as lineno from typegrp where id='ML' and trim(upper(ACREF2))='YES' and TYPE1= '392'", "COL1");
                emailTo = "";
                if (mhd != "0")
                {
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, cocd, "SELECT emailid AS COL1 FROM EVAS WHERE trim(emailid)<>'-' and userid in ('" + mhd + "')");
                    foreach (DataRow dr in dt.Rows)
                    {
                        emailTo += "," + dr["col1"].ToString();
                    }

                    emailTo = emailTo.TrimStart(',');
                    SQuery = "SELECT EMAILID AS EMAIL FROM EVAS WHERE TRIM(USERNAME)='" + frm_uname.Trim() + "'";
                    emailCC = fgen.seek_iname(frm_qstr, cocd, SQuery, "EMAIL");
                }
                break;
        }
        if (stb.ToString().Length > 2 && emailTo.Length > 2)
            fgen.send_mail(cocd, "Tejaxo ERP", emailTo, emailCC, "", emailSubj, stb.ToString());
    }
    public void send_msg(string cocd, string formID, string appr_Status, string info)
    {
        System.Text.StringBuilder stb = new System.Text.StringBuilder();
        string mobileno = "";
        switch (formID)
        {
            case "F10051":
            case "F10056":
                string headingNo = "Req No";
                if (formID == "F10056") headingNo = "Lead No";

                string username = ((TextBox)sg1.Rows[Convert.ToInt32(info)].FindControl("txtreason")).Text;
                stb.Append("Dear " + username.Split('~')[0].ToString() + ", " + headingNo + " " + sg1.Rows[Convert.ToInt32(info)].Cells[9].Text.Trim() + " M/c No. " + sg1.Rows[Convert.ToInt32(info)].Cells[11].Text.Trim() + " assigned to you");
                SQuery = "SELECT CONTACTNO FROM EVAS WHERE TRIM(USERNAME)||'~'||TRIM(USERID)='" + username.Trim() + "'";
                col1 = fgen.seek_iname(frm_qstr, cocd, SQuery, "CONTACTNO");
                if (col1.Length > 2)
                    mobileno = col1;
                break;
        }
        if (stb.ToString().Length > 2 && mobileno.Length > 2)
            fgen.send_sms(frm_qstr, frm_cocd, mobileno, stb.ToString(), frm_uname);
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow row = sg1.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (selectedCellIndex < 0) selectedCellIndex = 0;
        string mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading        
        if (selectedCellIndex > 0) selectedCellIndex -= 1;

        switch (frm_formID)
        {
            case "F15166":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", row.Cells[30].Text.Trim());
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE", row.Cells[31].Text.Trim());
                //SQuery = "SELECT DISTINCT A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT,A.ACODE,A.ICODE AS ERPCODE,B.INAME,A.PRATE,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM POMAS A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.TYPE LIKE '5%' AND NVL(A.APP_BY,'-')!='-' AND A.ORDDT BETWEEN (SYSDATE-1) AND (SYSDATE-500) and trim(a.acode)='" + row.Cells[29].Text.Trim() + "' and trim(a.icode)='" + row.Cells[30].Text.Trim() + "' ORDER BY VDD ";
                //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //fgen.Fn_open_rptlevel("Rate History of Erp Code : " + row.Cells[30].Text.Trim() + " for Last 500 Days", frm_qstr);
                fgen.Fn_Open_More_Details("Options for viewing Rates", frm_qstr);
                break;
        }
    }
    void nextPageIndex()
    {
        dt = (DataTable)ViewState["sg1"];
        DataTable neWDt = dt.Copy();
        ViewState["sg1"] = neWDt;
        makeColNameAsMine(dt);
        sg1.DataSource = dt;
        sg1.DataBind();
        sg1.Visible = true;
        hideAndRenameCol();
    }
    protected void sg1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //holdOldVlaues();
        //sg1.PageIndex = e.NewPageIndex;
        //nextPageIndex();
        //reSetHoldValues();

        ArrayList CheckBoxArray;
        if (ViewState["CheckBoxArray"] != null)
        {
            CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
        }
        else
        {
            CheckBoxArray = new ArrayList();
        }

        dt = (DataTable)ViewState["sg1"];

        if (sg1.Rows.Count > 1)
        {
            int CheckBoxIndex;
            bool CheckAllWasChecked = false;
            CheckBox chkAll = (CheckBox)sg1.HeaderRow.Cells[0].FindControl("chkappall");
            string checkAllIndex = "chkAll-" + sg1.PageIndex;
            if (chkAll.Checked)
            {
                if (CheckBoxArray.IndexOf(checkAllIndex) == -1) //HERE -1 DENOTES NO OCCURENCE FOUND
                {
                    CheckBoxArray.Add(checkAllIndex);
                }
            }
            else
            {
                if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                {
                    CheckBoxArray.Remove(checkAllIndex);
                    CheckAllWasChecked = true;
                }
            }

            for (int i = 0; i < sg1.Rows.Count; i++)
            {
                if (sg1.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)sg1.Rows[i].Cells[0].FindControl("chkapp");
                    CheckBoxIndex = sg1.PageSize * sg1.PageIndex + (i + 1);
                    if (chk.Checked)
                    {
                        if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
                        {
                            CheckBoxArray.Add(CheckBoxIndex);
                        }
                    }
                    else
                    {
                        if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
                        {
                            CheckBoxArray.Remove(CheckBoxIndex);
                        }
                    }
                }
            }
        }
        ViewState["CheckBoxArray"] = CheckBoxArray;
        sg1.PageIndex = e.NewPageIndex;
        makeColNameAsMine(dt);
        sg1.DataSource = dt;
        sg1.DataBind();
        sg1.Visible = true;
        hideAndRenameCol();
        Preserve();
    }
    void holdOldVlaues()
    {
        DataTable dtHoldValues = new DataTable();
        dtHoldValues.Columns.Add("Srno", typeof(Int32));
        dtHoldValues.Columns.Add("PIndex", typeof(Int32));
        dtHoldValues.Columns.Add("CheckedA", typeof(bool));
        dtHoldValues.Columns.Add("CheckedR", typeof(bool));
        DataRow drHoldValues;

        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            drHoldValues = dtHoldValues.NewRow();
            drHoldValues["Srno"] = i;
            drHoldValues["PIndex"] = sg1.PageIndex;
            drHoldValues["CheckedA"] = ((CheckBox)sg1.Rows[i].FindControl("chkapp")).Checked == true ? true : false;
            drHoldValues["CheckedR"] = ((CheckBox)sg1.Rows[i].FindControl("chkrej")).Checked == true ? true : false;
            dtHoldValues.Rows.Add(drHoldValues);
        }

        ViewState["dtHoldValues"] = dtHoldValues;
    }
    void reSetHoldValues()
    {
        if (ViewState["dtHoldValues"] != null)
        {
            DataTable dtHoldValues = new DataTable();
            dtHoldValues = (DataTable)ViewState["dtHoldValues"];
            for (int i = 0; i < dtHoldValues.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtHoldValues.Rows[i]["PIndex"].ToString()) == sg1.PageIndex)
                {
                    ((CheckBox)sg1.Rows[i].FindControl("chkapp")).Checked = Convert.ToBoolean(dtHoldValues.Rows[i]["CheckedA"]) == true ? true : false;
                    ((CheckBox)sg1.Rows[i].FindControl("chkrej")).Checked = Convert.ToBoolean(dtHoldValues.Rows[i]["CheckedR"]) == true ? true : false;
                }
            }
        }
    }
    static BackgroundWorker _bw;
    public static int Percent = 0;

    [System.Web.Services.WebMethod]
    public static string GetText()
    {
        return Percent.ToString();
    }

    protected void btnGetData_Click(object sender, EventArgs e)
    {
        _bw = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
        _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
        _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

        _bw.RunWorkerAsync();
        //runProg
    }

    void bw_DoWork(object sender, DoWorkEventArgs e)
    {

        for (int i = 0; i <= 100; i += 2)
        {
            if (_bw.CancellationPending) { e.Cancel = true; return; }
            _bw.ReportProgress(i);
            System.Threading.Thread.Sleep(100);
        }
        e.Result = 123;
    }
    void bw_RunWorkerCompleted(object sender,

                                       RunWorkerCompletedEventArgs e)
    {

    }

    void bw_ProgressChanged(object sender,
                                    ProgressChangedEventArgs e)
    {
        Percent = e.ProgressPercentage;
        lblStatus.Text = " " + e.ProgressPercentage;
    }

    void Preserve()
    {
        if (ViewState["CheckBoxArray"] != null)
        {
            ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            string checkAllIndex = "chkAll-" + sg1.PageIndex;

            if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
            {
                CheckBox chkAll = (CheckBox)sg1.HeaderRow.Cells[0].FindControl("chkAll");
                chkAll.Checked = true;
            }
            for (int i = 0; i < sg1.Rows.Count; i++)
            {

                if (sg1.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                    {
                        CheckBox chk = (CheckBox)sg1.Rows[i].Cells[0].FindControl("chkapp");
                        chk.Checked = true;
                        sg1.Rows[i].Attributes.Add("style", "background-color:aqua");
                    }
                    else
                    {
                        int CheckBoxIndex = sg1.PageSize * (sg1.PageIndex) + (i + 1);
                        if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                        {
                            CheckBox chk = (CheckBox)sg1.Rows[i].Cells[0].FindControl("chkapp");
                            chk.Checked = true;
                            sg1.Rows[i].Attributes.Add("style", "background-color:aqua");
                        }
                    }
                }
            }
        }
    }

    protected void btnList_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,USERID,USERNAME,OBJ_NAME AS TILE from DSK_WCONFIG WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='80' ORDER BY USERNAME,VCHNUM";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("List of Users having Desktop Rights", frm_qstr);
    }

    void sign_dsc_background(string formID, string fstr)
    {
        string pageurl = "tej-wfin/tej-base/dprint.aspx?STR=ERP@" + DateTime.Now.ToString("dd") + "@" + frm_cocd + "@" + frm_cDt1.Substring(6, 4) + frm_mbr + "@" + frm_UserID + "@BVAL@" + formID + "@" + fstr + "@CLOSE";
        string url = HttpContext.Current.Request.Url.Authority;
        string finalurl = "http://" + url + "//" + pageurl;

        lbllink.Value += "~" + finalurl;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        hfClickEvent.Value = "1";
        hffield.Value = "Btn1";
        switch (frm_formID)
        {
            case "F15166":
                buttonQuery();
                break;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        hfClickEvent.Value = "1";
        hffield.Value = "Btn2";
        switch (frm_formID)
        {
            case "F15166":
                buttonQuery();
                break;
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        hfClickEvent.Value = "1";
        hffield.Value = "Btn3";
        switch (frm_formID)
        {
            case "F15166":
                buttonQuery();
                break;
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        hfClickEvent.Value = "1";
        hffield.Value = "Btn4";
        switch (frm_formID)
        {
            case "F15166":
                buttonQuery();
                break;
        }
    }

    void buttonQuery()
    {
        switch (hfClickEvent.Value)
        {
            case "1":
                switch (frm_formID)
                {
                    case "F15166":
                        switch (hffield.Value)
                        {
                            case "Btn1":
                            case "Btn2":
                                fgen.Fn_open_prddmp1("-", frm_qstr);
                                break;
                            case "Btn4":
                            case "Btn3":
                                fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                                break;
                        }
                        break;
                }
                hfClickEvent.Value = "2";
                break;
            case "2":
                switch (frm_formID)
                {
                    case "F15166":
                        switch (hffield.Value)
                        {
                            case "Btn1":
                                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                                SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F15129", "branchcd='" + frm_mbr + "'", "a.type='10'", PrdRange);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("Approved Price Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                                break;
                            case "Btn2":
                                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                                SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F15129", "branchcd='" + frm_mbr + "'", "a.type='10'", PrdRange);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("Approved Price Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                                break;
                            case "Btn3":
                                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                                if (part_cd.Length > 3) part_cd = part_cd.Substring(0, 2);
                                SQuery = "Select a.branchcd,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orrdt,trim(a.icode) as icode,trim(a.iname) as iname,trim(a.cpartno) as cpartno,a.unit,trim(a.deptt) as deptt,trim(b.type1) as Deptt_code,nvl(a.req_qty,0) as req_qty,nvl(a.ord_qty,0) as ord_qty,nvl(a.Bal_qty,0) as bal_qty from wbvu_pending_pr a, type b where trim(a.deptt)= trim(b.name) and b.id='M' and A.BRANCHCD='" + frm_mbr + "' and b.type1 like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' and a.orddt " + xprdrange + " order by a.orddt, a.ordno";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("Pending Purchase Requisition For the Period " + fromdt + " to " + todt, frm_qstr);
                                break;
                            case "Btn4":
                                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                                SQuery = "SELECT A.VCHNUM AS GE_Number,to_Char(a.VCHDATE,'dd/mm/yyyy') as Ge_Date,B.Aname as Supplier,b.addr1 as Address,c.Iname,c.Cpartno,a.iqty_chl as GE_Qty,c.unit,a.Invno as Inv_no,A.Refnum as Chl_no,b.Staten,a.prnum,to_Char(A.Invdate,'dd/mm/yyyy') as Inv_Dt,a.Icode,a.Acode,to_Char(a.vchdate,'yyyymmdd') as GE_Dt FROM IVOUCHERP a, famst b ,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and TRIM(A.iCODE)=TRIM(c.iCODE)  and a.BRANCHCD='" + frm_mbr + "' AND a.VCHDATE  " + xprdrange + " AND a.TYPE='00'  AND (a.VCHNUM||to_char(a.vchdate,'yyyymm')) IN (SELECT VCHNUM FROM (SELECT X.VCHNUM,SUM(X.aBC) AS CNT FROM (select distinct a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC from ivoucherp a  where branchcd='" + frm_mbr + "' and a.VCHDATE   " + xprdrange + " AND a.type='00' and a.vchnum<>'000000' UNION ALL select distinct a.GENUM||to_char(a.gedate,'yyyymm') as genum,a.type,1 AS ABC from ivoucher a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)='0' and a.VCHDATE  " + xprdrange + " AND a.vchnum<>'000000' ) X GROUP BY X.VCHNUM) WHERE CNT=1) order by to_Char(a.vchdate,'yyyymmdd') desc,A.VCHNUM desc ";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("Checklist of Gate Entry Pending MRR for the Period " + fromdt + " to " + todt, frm_qstr);
                                break;
                        }
                        break;
                }
                hfClickEvent.Value = "3";
                break;
        }
    }
}