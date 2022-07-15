using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class rj_app : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, col1, col2, mbr, vardate, year, ulvl, HCID, xprdrange, cond, fromdt, todt, CSR;
    string frm_uname, frm_url, frm_qstr, frm_formID, DateRange, frm_UserID, cstr;
    string mdt1, mdt2, mprdrange;
    int totCol = 50;
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
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");

                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(co_cd, frm_qstr);
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                set_val();
            }
            if (vardate == "")
            {
                vardate = fgen.seek_iname(frm_qstr, co_cd, "select to_date(to_char(sysdate,'dd/MM/YYYY'),'DD/MM/YYYY') AS DT FROM DUAL", "DT");
            }
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false;
        btnsave.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnext.Text = " Exit ";
        btnext.Enabled = true;
        srch.Enabled = false;
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
        srch.Enabled = true;
    }
    public void clearctrl()
    { hffield.Value = ""; }
    public void set_val()
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "N");
        HCID = frm_formID;
        switch (HCID)
        {

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

            case "F15161":
                lblheader.Text = "P.R. Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F15162":
                lblheader.Text = "P.R. Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F15165":
                lblheader.Text = "P.O. Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F15166":
                lblheader.Text = "P.O. Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F81111":
                lblheader.Text = "Leaves Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F47126":
            case "F49126":
                lblheader.Text = "S.O. Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F47127":
            case "F49127":
                lblheader.Text = "S.O. Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F55126":
                lblheader.Text = "Export P.I. Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F55127":
                lblheader.Text = "Export P.I. Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F55128":
                lblheader.Text = "Exp. S.O. Checking";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F55129":
                lblheader.Text = "Exp. S.O. Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;


            case "F15171":
                lblheader.Text = "Purch Schedule Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;
            case "F47128":
            case "F49128":
                lblheader.Text = "Sales Schedule Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "F15176":
                lblheader.Text = "APL Approval";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                break;

            case "M02032":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "P.O. Checking";
                break;
            case "M02036":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "P.O. Approval";
                break;
            case "M02040":
                lblheader.Text = "Purch Sch. Approval";
                break;

            case "F15210":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "P.R. Closure";
                break;
            case "F15211":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "P.O. Closure";
                break;
            case "M02046":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "P.O. Cancel";
                break;
            case "M10010B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "S.O. Checking(Dom.)";
                break;
            case "M10015B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "S.O. Approval(Dom.)";
                break;
            case "F47162":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "S.O. Closure(Dom.)";
                break;
            case "M11010B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "S.O. Checking(Exp.)";
                break;
            case "M11015B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "S.O. Approval(Exp.)";
                break;
            case "M11020B":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "S.O. Closure(Exp.)";
                break;
            case "M10015A":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "P.I. Approval(Dom.)";
                break;
            case "M11015A":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "P.I. Approval(Exp.)";
                break;
            case "M10024":
                lblheader.Text = "Sales Sch. Approval";
                break;
            case "M09008":
                lblheader.Text = "Lead Approval";
                break;
            case "M09028":
                lblheader.Text = "Quotation Approval";
                break;
            default: lblheader.Text = "";
                break;
        }
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        set_val();
        clearctrl();
        hffield.Value = "New";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
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

                if (chk1.Checked == true && chk2.Checked == true)
                { fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Entry No. " + row.Cells[3].Text.Trim()); i = 0; return; }
                else
                {
                    if (chk1.Checked == true || chk2.Checked == true)
                    {
                        if (HCID == "**M10015A" || HCID == "**M11015A") i = 1;
                        else
                        {
                            //i = fgen.ChkDate(tk.Text.Trim());
                            //if (i != 0) i = 1;
                            //else
                            //{ fgen.msg("-", "AMSG", "Not a valid date entered infront of'13'Entry No. " + row.Cells[3].Text.Trim()); return; }
                            //if (HCID == "25051" && Convert.ToDateTime(tk.Text.Trim()) < Convert.ToDateTime(System.DateTime.Now.ToShortDateString()))
                            //{ fgen.msg("-", "AMSG", "Date can not be less then present Date'13'See at Entry No. " + row.Cells[3].Text.Trim()); i = 0; return; }
                            if (co_cd == "HIME" && frm_uname == "ASHEESH") { }
                            else
                            {
                                if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                                {
                                    fgen.msg("-", "AMSG", "Please enter the Reason for Refusal '13'See at Entry No. " + row.Cells[13].Text.Trim() + " " + row.Cells[13].Text.Trim());
                                    i = 0;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            if (i != 0) fgen.msg("-", "SMSG", "Are you sure, you want to Proceed !!");
        }
        else
        {
            if (HCID == "*M10015B") fgen.msg("-", "AMSG", "Please Approve any one row to save");
            else fgen.msg("-", "AMSG", "Please Approve or refuse any one row to save");
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        dt = new DataTable(); col1 = ""; SQuery = "";
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
                        if (col1 == "N") SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') and ent_by='" + frm_uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
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
                        if (col1.Trim().Length == 4) SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')'-' and vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') and trim(acode) in ('" + col1 + "') and ent_by='" + frm_uname + "' GROUP BY vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
                        else SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') and trim(acode) in (" + col1 + ") and ent_by='" + frm_uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
                        break;
                }
                break;
            case "70002":
                switch (btnval)
                {
                    case "New_E":
                        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                        if (col1 == "N") SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM TYPEGRP B, TYPEGRP C,SCRATCH2 A LEFT JOIN SCRATCH2 D ON  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  WHERE TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + mbr + "' AND  A.TYPE='SD' and A.vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy')  ORDER BY A.VCHNUM";
                        else
                        {
                            SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM SCRATCH2 A,TYPEGRP B, TYPEGRP C ,SCRATCH2 D WHERE  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  AND TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + mbr + "' AND  A.TYPE='SD' AND A.vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') ORDER BY A.VCHNUM";
                        }
                        break;
                    case "VW":
                        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                        if (col1.Trim().Length == 4) SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM SCRATCH2 A,TYPEGRP B, TYPEGRP C ,SCRATCH2 D WHERE  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  AND TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + mbr + "' AND  A.TYPE='SD' AND A.vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') ORDER BY A.VCHNUM";
                        else SQuery = "SELECT A.VCHNUM AS SDR_NO, TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY')) AS SDR_DATE,A.ACODE AS CLIENT_CODE,B.NAME AS CLIENT, A.ICODE AS DEVELOPER_CODE,C.NAME AS DEVELOPER,A.REMARKS AS TASK,D.VCHNUM AS SDR_UPDATE_NO,TO_CHAR(D.VCHDATE,'DD/MM/YYYY') AS SDR_UPDATE_DATE,D.COL1 AS WORK_START_DATE,D.COL2 AS WORK_COMPLETION_DATE  FROM SCRATCH2 A,TYPEGRP B, TYPEGRP C ,SCRATCH2 D WHERE  TRIM(A.VCHNUM)=TRIM(D.INVNO) AND TRIM(TO_CHAR(A.VCHDATE,'DD/MM/YYYY'))=TRIM(TO_CHAR(D.INVDATE,'DD/MM/YYYY'))  AND TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.TYPE1)  AND B.ID='SC' AND C.ID='SE'  AND A.BRANCHCD='" + mbr + "' AND  A.TYPE='SD' AND A.vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') ORDER BY A.VCHNUM";
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
                            xprdrange = "between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy')";
                            if (co_cd == "NEOP")
                            {
                                if (ulvl == "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,a.ent_by,a.ent_Dt,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " order by vdd desc,a.ordno,A.TYPE";
                                else
                                {
                                    col1 = ""; col1 = "";
                                    col1 = fgen.seek_iname(frm_qstr, co_cd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + frm_uname + "'", "icons");
                                    if (col1.Length > 1)
                                    {
                                        string[] word = col1.Split(',');
                                        foreach (string vp in word)
                                        {
                                            if (col2.Length > 0) col2 = col2 + "," + "'" + vp.ToString().Trim() + "'";
                                            else col2 = "'" + vp.ToString().Trim() + "'";
                                        }
                                        if (col1 != "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " and trim(b.bssch) in (" + col2 + ") order by vdd desc ,a.ordno,A.TYPE";
                                    }
                                }
                            }
                            else SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,a.ent_by,a.ent_dt,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " order by vdd desc,a.ordno,A.TYPE";
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
                        xprdrange = "between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy')";
                        if (co_cd == "NEOP")
                        {
                            if (ulvl == "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " " + cond + " order by vdd desc,a.ordno,A.TYPE";
                            else
                            {
                                col1 = ""; col1 = "";
                                col1 = fgen.seek_iname(frm_qstr, co_cd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + frm_uname + "'", "icons");
                                if (col1.Length > 1)
                                {
                                    string[] word = col1.Split(',');
                                    foreach (string vp in word)
                                    {
                                        if (col2.Length > 0) col2 = col2 + "," + "'" + vp.ToString().Trim() + "'";
                                        else col2 = "'" + vp.ToString().Trim() + "'";
                                    }
                                    if (col1 != "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " and trim(b.bssch) in (" + col2 + ") " + cond + " order by vdd desc ,a.ordno,A.TYPE";
                                }
                            }
                        }
                        else SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " " + cond + " order by vdd desc,a.ordno,A.TYPE";
                        break;
                }
                break;
        }
        if (SQuery.Length > 0)
        {
            fgen.EnableForm(this.Controls); disablectrl();
            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
            fillGrid();
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "New")
        {
            HCID = frm_formID;
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            mdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
            mdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
            mprdrange = "between to_date('" + mdt1 + "','dd/mm/yyyy') and to_date('" + mdt2 + "','dd/mm/yyyy')";
            ViewState["fromdt"] = col1; ViewState["todt"] = col2;
            SQuery = "";
            if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
            switch (HCID)
            {
                case "F60176":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.CSSNO)||to_char(a.CSSdt,'dd/mm/yyyy') as fstr,to_Char(a.CSSdt,'yyyymmdd') as vdd,a.CCode,a.CSSNO as Css_No,to_Char(A.CSSdt,'dd/mm/yyyy') as css_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.dir_comp,a.Last_Action,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_log a where a.branchcd='" + mbr + "' and a.type='CS' and a.CSSdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and upper(Last_action)='MARKED COMPLETE' " + cond + " order by vdd,a.CSSNO";
                    break;
                case "F60181":
                    //and upper(Work_action)!='-'
                    SQuery = "select distinct a.branchcd||a.type||trim(a.CSSNO)||to_char(a.CSSdt,'dd/mm/yyyy') as fstr,to_Char(a.CSSdt,'yyyymmdd') as vdd,a.CCode,a.CSSNO as Css_No,to_Char(A.CSSdt,'dd/mm/yyyy') as css_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.dir_comp,a.Work_Action,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_log a where a.branchcd='" + mbr + "' and a.type='CS' and a.CSSdt " + mprdrange + " and trim(nvl(a.Fapp_by,'-'))='-'  order by vdd,a.CSSNO";
                    break;
                case "F60186":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.actno)||to_char(a.actdt,'dd/mm/yyyy') as fstr,to_Char(a.actdt,'yyyymmdd') as vdd,a.CCode,a.actno as Act_No,to_Char(A.Actdt,'dd/mm/yyyy') as Act_Dt,a.Emodule as css_Module,a.Eicon as css_Icon,a.asg_agt as Assign_to,a.Act_status,a.remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_css_act a where a.branchcd='" + mbr + "' and a.type='AC' and a.TASK_COMPL='Y' and  a.actdt " + mprdrange + "  and trim(nvl(a.app_by,'-'))='-' order by vdd,a.actno";
                    break;
                case "F94106":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.STlNO)||to_char(a.STldt,'dd/mm/yyyy') as fstr,to_Char(a.sTldt,'yyyymmdd') as vdd,a.CCode,a.STlNO as STl_No,to_Char(A.sTldt,'dd/mm/yyyy') as STl_Dt,a.Emodule as STl_Module,a.Eicon as STl_Name,a.EVERTICAL,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_STl_log a where a.branchcd='" + mbr + "' and a.type='TG' and a.STldt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.STlNO";
                    break;

                case "F96106":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.DSLNO)||to_char(a.DSLdt,'dd/mm/yyyy') as fstr,to_Char(a.DSLdt,'yyyymmdd') as vdd,a.CCode,a.DSLNO as DSL_No,to_Char(A.DSLdt,'dd/mm/yyyy') as DSL_Dt,a.Emodule as DSL_Module,a.Eicon as DSL_Name,a.EVERTICAL,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_DSL_log a where a.branchcd='" + mbr + "' and a.type='SL' and a.DSLdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.DSLNO";
                    break;
                case "F97106":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.CAMNO)||to_char(a.CAMdt,'dd/mm/yyyy') as fstr,to_Char(a.CAMdt,'yyyymmdd') as vdd,a.TCode,a.CAMNO as CAM_No,to_Char(A.cAMdt,'dd/mm/yyyy') as CAM_Dt,a.Cam_type as cAM_Type,a.Cam_Spec,a.Tcode as Team_member,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_CAM_log a where a.branchcd='" + mbr + "' and a.type='EQ' and a.CAMdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.CamNO";
                    break;

                case "S06005B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||lpad(trim(to_char(a.srno,'999')),3,'0') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as Alt_No,to_Char(A.vchdate,'dd/mm/yyyy') as Alt_Dt,a.Acode as Client,a.qrytopic as Qry_Topic,a.qryokay as Qry_Okay,a.Qmark_Name as Alloted_To,a.Qry_rmk,a.Qry_Tgtdt,a.Last_Action,a.Qry_Link,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from cquery_alt a where a.branchcd='" + mbr + "' and a.type='CA' and a.vchdate " + mprdrange + "  and trim(nvl(a.clo_by,'-'))='-' order by vdd,a.vchnum";
                    break;
                case "F10141":
                    SQuery = "select distinct trim(a.icode) as fstr,to_Char(a.ent_dt,'yyyymmdd') as vdd,a.Icode as ERP_Code,a.Iname as Item,a.cpartno as Part_No,a.CDrgno,A.Unit,a.HSCODe,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from item a where a.branchcd!='DD' and to_Date(to_char(a.ent_dt,'dd/mm/yyyy'),'dd/mm/yyyy') " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.deac_by,'-'))='-'  order by vdd,a.icode";
                    break;
                case "F10142":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as BOM_No,to_Char(A.vchdate,'dd/mm/yyyy') as BOM_Dt,c.iname as Item_Name,c.cpartno as Part_No,c.Cdrgno,c.unit,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from itemosp a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='BM' and a.vchdate " + mprdrange + " and  trim(nvl(a.app_by,'-'))='-'  order by vdd,a.vchnum";
                    break;

                case "F10143":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as PP_No,to_Char(A.vchdate,'dd/mm/yyyy') as PP_Dt,c.iname as Item_Name,c.cpartno as Part_No,c.Cdrgno,c.unit,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from inspmst a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='70' and a.vchdate " + mprdrange + " and  trim(nvl(a.app_by,'-'))='-'  order by vdd,a.vchnum";
                    break;

                case "F15161":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as Pr_No,to_Char(A.orddt,'dd/mm/yyyy') as Pr_Dt,a.Bank as Deptt,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PR_Qty,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='60' and a.orddt " + mprdrange + " and trim(nvl(a.chk_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and a.pflag!=0 order by vdd,a.ordno";
                    break;
                case "F15162":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as Pr_No,to_Char(A.orddt,'dd/mm/yyyy') as Pr_Dt,a.Bank as Deptt,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PR_Qty,c.unit,a.desc_ as Remarks,A.chk_by,TO_CHAR(A.chk_dT,'DD/mm/yyyy') as chk_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='60' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.pflag!=0 order by vdd,a.ordno";
                    break;
                case "F15165":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,A.Prate,A.Pdisc,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.chk_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "F15166":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,A.Prate,A.Pdisc,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "F81111":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.lrqno)||to_char(a.lrqdt,'dd/mm/yyyy') as fstr,to_Char(a.lrqdt,'yyyymmdd') as vdd,a.lrqno as Lrq_No,to_Char(A.lrqdt,'dd/mm/yyyy') as Lrq_Dt,b.Name as Employee_Name,b.Deptt_Text as Department,b.desg_text as Designation,a.levfrom,a.levupto,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_lev_Req a,empmas b where a.branchcd||trim(A.empcode)=trim(b.branchcd)||b.grade||trim(B.empcode) and a.branchcd='" + mbr + "' and a.type like 'LR%' and a.lrqdt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-'  order by vdd,a.lrqno";
                    break;

                case "F15171":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as Sch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Sch_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Total as Sch_Qty,a.line_rmk as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.srno from schedule a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='66' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' order by vdd,a.vchnum,a.srno";
                    break;
                case "F47128":
                case "F49128":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as Sch_No,to_Char(A.vchdate,'dd/mm/yyyy') as Sch_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Total as Sch_Qty,a.line_rmk as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.srno from schedule a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' order by vdd,a.vchnum,a.srno";
                    break;

                case "F15176":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.vchdate,'yyyymmdd') as vdd,a.vchnum as APL_No,to_Char(A.vchdate,'dd/mm/yyyy') as APL_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.irate as APL_rate,a.Disc as APL_Disc,c.unit,a.Remarks as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt,a.srno from price_list a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='10' and a.vchdate " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' order by vdd,a.vchnum,a.srno";
                    break;

                case "F47126":
                case "F49126":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;
                case "F47127":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;
                case "F49127":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type in ('4F') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;
                case "F55126":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasq a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;
                case "F55127":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasq a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;
                case "F55128":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;
                case "F55129":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.type,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,a.irate as SO_Rate,a.Cdisc as Disc,c.unit,a.desc_ as Remarks,A.Check_by,TO_CHAR(A.Check_dT,'DD/mm/yyyy') as Check_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type in ('4F','4E') and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.icat!='Y' order by vdd,a.ordno";
                    break;


                case "M02032":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.chk_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-'  and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "M02036":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.chk_by,TO_CHAR(A.chk_dT,'DD/mm/yyyy') as chk_Dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and a.pflag!=1  order by vdd,a.ordno";
                    break;
                case "F15210":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as Pr_No,to_Char(A.orddt,'dd/mm/yyyy') as Pr_Dt,a.Bank as Deptt,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PR_Qty,c.unit,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.Ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Chk_by,TO_CHAR(A.Chk_dT,'DD/mm/yyyy') as Chk_Dt,A.App_by,TO_CHAR(A.App_dT,'DD/mm/yyyy') as App_Dt from pomas a,item c where trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='60' and a.orddt " + mprdrange + " and a.pflag!=0 order by vdd,a.ordno";
                    break;
                case "F15211":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.Ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Chk_by,TO_CHAR(A.Chk_dT,'DD/mm/yyyy') as Chk_Dt,A.App_by,TO_CHAR(A.App_dT,'DD/mm/yyyy') as App_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "M02046":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as PO_No,to_Char(A.orddt,'dd/mm/yyyy') as PO_Dt,b.Aname as Supplier,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as PO_Qty,c.unit,a.Prate as PO_Rate,a.PDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.Ent_dT,'DD/mm/yyyy') as Ent_Dt,A.Chk_by,TO_CHAR(A.Chk_dT,'DD/mm/yyyy') as Chk_Dt,A.App_by,TO_CHAR(A.App_dT,'DD/mm/yyyy') as App_Dt from pomas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + mprdrange + " and a.pflag!=1 order by vdd,a.ordno";
                    break;
                case "M10010B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y'  order by vdd,a.ordno";
                    break;
                case "M10015B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "F47162":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + "  and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;

                case "M11010B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + " and trim(nvl(a.check_by,'-'))='-' and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M11015B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M11020B":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somas a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + "  and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M10015A":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasp a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type!='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M11015A":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasp a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.type='4F' and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "M09028":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode)||lpad(trim(to_char(a.srno,'9999')),4,'0') as fstr,to_Char(a.orddt,'yyyymmdd') as vdd,a.ordno as SO_No,to_Char(A.orddt,'dd/mm/yyyy') as SO_Dt,b.Aname as Customer,c.iname as Item_Name,c.cpartno as Part_No,a.Qtyord as SO_Qty,c.unit,a.Irate as SO_Rate,a.CDisc as Disc_Perc,a.desc_ as Remarks,a.Check_by,a.Check_dt,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from Somasq a,famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%'  and a.orddt " + mprdrange + " and trim(nvl(a.app_by,'-'))='-' and trim(nvl(a.icat,'-'))<>'Y' order by vdd,a.ordno";
                    break;
                case "99001":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["fromdt"] = col1; ViewState["todt"] = col2;
                    hffield.Value = "New_E";
                    fgen.msg("-", "CMSG", "Do you want so select user id'13'(No for all users)");
                    break;
                case "70002":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["fromdt"] = col1; ViewState["todt"] = col2;
                    hffield.Value = "New_E";
                    fgen.msg("-", "CMSG", "Do you want to see completed  jobs'13'(No for all jobs)");
                    break;
                case "*M10015B":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["fromdt"] = col1; ViewState["todt"] = col2;
                    hffield.Value = "New_E";
                    fgen.msg("-", "CMSG", "Do you want so select Order Type'13'(No for all)");
                    break;
                case "25051":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["fromdt"] = col1; ViewState["todt"] = col2;
                    if (ulvl == "0") SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as complaint_no,to_Char(A.vchdate,'dd/mm/yyyy') as complaint_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,A.ENt_BY,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd";
                    else
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as complaint_no,to_Char(A.vchdate,'dd/mm/yyyy') as complaint_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') and trim(a.ent_by)='" + frm_uname + "' order by vdd";
                        if (co_cd == "SRIS") SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as complaint_no,to_Char(A.vchdate,'dd/mm/yyyy') as complaint_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,A.ENt_BY,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd";
                    }
                    break;
            }
            if (SQuery.Length > 0)
            {
                fgen.EnableForm(this.Controls); disablectrl();
                dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                fillGrid();
            }
        }
        else
        {
            col1 = "";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
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
                            if (HCID == "F15162" || HCID == "F15166" || HCID == "F47127" || HCID == "F49127" || HCID == "F55127" || HCID == "F55129")
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
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                                //send_m("A", row.Cells[8].Text.Trim());
                            }
                            else if (chk2.Checked == true)
                            {
                                if (HCID == "F15210" || HCID == "F15211" || HCID == "M02046" || HCID == "F47162" || HCID == "M11020B")
                                {

                                    if (HCID == "F15210") { myquery = "update pomas set atch1='" + rej_rsn + "',desp_to='-',term=trim(term)||' Closed by " + frm_uname + "',pbasis=to_char(sysdate,'dd/mm/yyyy'),pflag=0 where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'"; }
                                    if (HCID == "F15211") { myquery = "update pomas set APP_BY='(C)'||TRIM(NVL(APP_BY,'-')),desp_to='" + rej_rsn + "',pflag=1 ,term=trim(term)||' Closed by " + frm_uname + "',invdate=sysdate where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'"; }
                                    if (HCID == "M02046") { myquery = "update pomas set term='* * CANCELLED P.O.* * '||' " + rej_rsn + " " + frm_uname + " '||trim(term),pflag=1 ,qtysupp=2, pr_no='-' where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'"; }
                                    if (HCID == "F47162" || HCID == "M11020B") { myquery = "update somas set shipmark='By " + frm_uname + "'||' on '||sysdate||' Reason " + rej_rsn + "',icat='Y' where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'"; }
                                }
                                else
                                {
                                    myquery = "update pomas set " + myappno + "='(R)" + frm_uname + "'," + myappdt + "=sysdate,pbasis=to_Char(sysdate,'dd/mm/yyyy'),rate_diff='" + mydoc + " REJECTED (" + rej_rsn + ")',pflag=" + myrjflag + " where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                }

                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
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
                        case "F47127":
                        case "F49127":

                        case "F55126":
                        case "F55127":

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
                            if (HCID == "M10010B" || HCID == "M11010B" || HCID == "F47126" || HCID == "F49126" || HCID == "F55126" || HCID == "F55128")
                            {
                                myappno = "check_by";
                                myappdt = "check_dt";
                            }
                            if (HCID == "M10015B" || HCID == "M11015B" || HCID == "M10015A" || HCID == "M11015A" || HCID == "F47127" || HCID == "F49127" || HCID == "F55127" || HCID == "F55129")
                            {
                                myappno = "app_by";
                                myappdt = "app_dt";
                            }
                            if (HCID == "M10015A" || HCID == "M11015A")
                            {
                                mytable = "somasp";
                            }
                            if (HCID == "M09028" || HCID == "F55126" || HCID == "F55127")
                            {
                                mytable = "somasq";
                            }

                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            else if (chk2.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate,shipmark='" + mydoc + " REJECTED (" + rej_rsn + ")',icat='" + myrjflag + "' where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;
                        case "F81111":
                            mytable = "wb_lev_Req";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where trim(branchcd)||trim(type)||trim(lrqno)||to_Char(lrqdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "F10141":
                            mytable = "item";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where trim(icode) ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;
                        case "F10142":
                            mytable = "itemosp";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "F10143":
                            mytable = "inspmst";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "F15176":
                            mytable = "price_list";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(icode)||lpad(trim(to_char(srno,'9999')),4,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
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
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "F60176":
                            mytable = "wb_css_log";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(cssno)||to_char(cssdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;
                        case "F60181":
                            mytable = "wb_css_log";
                            myappno = "Fapp_by";
                            myappdt = "Fapp_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate,Last_action='Marked Complete',last_Actdt=to_date(sysdate,'dd/mm/yyyy') where branchcd||type||trim(cssno)||to_char(cssdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "F60186":
                            mytable = "wb_css_act";
                            myappno = "app_by";
                            myappdt = "app_dt";

                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                                string Qlink2;
                                Qlink2 = fgen.seek_iname(frm_qstr, co_cd, "select ent_by||'-'||act_Status As fstr from " + mytable + " where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'", "fstr");

                                string Qlink;
                                Qlink = fgen.seek_iname(frm_qstr, co_cd, "select branchcd||'CS'||cssno||to_char(Cssdt,'dd/mm/yyyy') As fstr from " + mytable + " where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'", "fstr");

                                myquery = "update wb_Css_log set WORK_action='Action:" + Qlink2 + "' where branchcd||type||trim(cssno)||to_char(cssdt,'dd/mm/yyyy') ='" + Qlink + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);

                                myquery = "commit";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);

                            }
                            break;
                        case "F94106":
                            mytable = "wb_STl_log";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(STlno)||to_char(STl    dt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "F96106":
                            mytable = "wb_DSL_log";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(DSLno)||to_char(DSLdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;
                        case "F97106":
                            mytable = "wb_cam_log";
                            myappno = "app_by";
                            myappdt = "app_dt";
                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(camno)||to_char(camdt,'dd/mm/yyyy') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "S05005B":
                            mytable = "cquery_reg";
                            myappno = "clo_by";
                            myappdt = "clo_dt";

                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;
                        case "S06005B":
                            mytable = "cquery_alt";
                            myappno = "clo_by";
                            myappdt = "clo_dt";

                            if (chk1.Checked == true)
                            {
                                myquery = "update " + mytable + " set " + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + row.Cells[8].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);

                                string Qlink;
                                Qlink = fgen.seek_iname(frm_qstr, co_cd, "select qry_link from " + mytable + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + row.Cells[8].Text.Trim() + "'", "qry_link");

                                mytable = "cquery_reg";

                                myquery = "update " + mytable + " set Last_action='Cleared by " + frm_uname + " on '||sysdate where trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||lpad(trim(to_char(srno,'999')),3,'0') ='" + Qlink + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
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
                    }
                }
                if (HCID == "F15161" || HCID == "M02032" || HCID == "M10010B" || HCID == "M11010B") fgen.msg("-", "AMSG", "Documet Checking Successfully completed");
                else fgen.msg("-", "AMSG", "Document Approval / Refusal Successfully completed");
                enablectrl(); sg1.DataSource = null; sg1.DataBind(); sg1.Visible = false;
                fgen.DisableForm(this.Controls); btnnew.Focus();
            }
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg1.Columns[0].HeaderStyle.Width = 30;
            e.Row.Cells[0].Width = 30;
            sg1.HeaderRow.Cells[0].Style["text-align"] = "center";
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            sg1.Columns[1].HeaderStyle.Width = 30;
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            sg1.HeaderRow.Cells[1].Style["text-align"] = "center";

            HCID = frm_formID;
            e.Row.Cells[4].CssClass = "hidden";
            sg1.HeaderRow.Cells[4].CssClass = "hidden";

            e.Row.Cells[5].CssClass = "hidden";
            sg1.HeaderRow.Cells[5].CssClass = "hidden";

            e.Row.Cells[6].CssClass = "hidden";
            sg1.HeaderRow.Cells[6].CssClass = "hidden";

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
                case "F15161":
                case "F15162":
                case "F15165":
                case "F15166":
                case "F81111":
                case "F15171":
                case "F15176":

                case "F47126":
                case "F49126":

                case "F47127":
                case "F49127":

                case "F47128":
                case "F49128":

                case "F55126":
                case "F55127":
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
                case "F94106":
                case "F96106":
                case "F97106":
                    //ok colm
                    //e.Row.Cells[0].CssClass = "hidden";
                    //sg1.Columns[0].HeaderStyle.CssClass = "hidden";

                    if (HCID == "S06005B" || HCID == "S05005B" || HCID == "M02040" || HCID == "M09008" || HCID == "M10015A" || HCID == "M11015A" || HCID == "M09028" || HCID == "M10010B" || HCID == "M10015B" || HCID == "M11010B" || HCID == "M11015B" || HCID == "M10024")
                    {
                        //rej colm
                        e.Row.Cells[1].CssClass = "hidden";
                        sg1.Columns[1].HeaderStyle.CssClass = "hidden";
                        //remarks colm
                        e.Row.Cells[7].CssClass = "hidden";
                        sg1.Columns[7].HeaderStyle.CssClass = "hidden";
                    }

                    //completed Dt colm
                    e.Row.Cells[2].CssClass = "hidden";
                    sg1.Columns[2].HeaderStyle.CssClass = "hidden";

                    if (HCID == "F10141" || HCID == "F10142" || HCID == "F10143" || HCID == "F15171" || HCID == "F47128" || HCID == "F49128" || HCID == "F15176" || HCID == "F60176" || HCID == "F60181" || HCID == "F60186" || HCID == "F94106" || HCID == "F96106" || HCID == "F97106")
                    {
                        //View Doc
                        if (HCID != "F60186")
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

                    //fstr colm
                    //e.Row.Cells[8].CssClass = "hidden";
                    //sg1.Columns[8].HeaderStyle.CssClass = "hidden";

                    //e.Row.Cells[9].CssClass = "hidden";
                    //sg1.Columns[9].HeaderStyle.CssClass = "hidden";

                    if (HCID == "F15210*" || HCID == "F15211" || HCID == "M02046" || HCID == "F47162" || HCID == "M11020B")
                    {
                        e.Row.Cells[0].CssClass = "hidden";
                        sg1.Columns[0].HeaderStyle.CssClass = "hidden";
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
            }
        }
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ") Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr, false);
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
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        HCID = frm_formID;

        switch (var)
        {
            case "Show":
                switch (HCID)
                {

                    case "F15210":
                    case "F15161":
                    case "F15162":
                        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "60");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[13].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1003");
                        fgen.fin_purc_reps(frm_qstr);
                        break;
                    case "F15211":
                    case "F15165":
                    case "F15166":

                        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[10].Text.Trim() + sg1.Rows[rowIndex].Cells[11].Text.Trim() + "'");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
                        fgen.fin_purc_reps(frm_qstr);
                        break;

                    case "F47126":
                    case "F49126":
                    case "F47127":
                    case "F49127":
                        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[8].Text.Trim().Substring(2, 2));
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + sg1.Rows[rowIndex].Cells[11].Text.Trim() + sg1.Rows[rowIndex].Cells[12].Text.Trim() + "'");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1005");
                        fgen.fin_smktg_reps(frm_qstr);
                        break;
                    case "25051":
                        SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "' order by vdd desc,a.srno";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Complaint List", frm_qstr);
                        break;
                    case "F60186":
                        try
                        {
                            col2 = fgen.seek_iname(frm_qstr, co_cd, "SELECT FILENAME||'^'||FILEPATH AS FSTR from WB_CSS_ACT where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[8].Text.Trim() + "'", "FSTR");
                            if (col2.Length > 5)
                            {
                                string fileName = col2.Split('^')[0].ToString().Trim();
                                string filePath = col2.Split('^')[1].ToString().Trim();
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
                }
                break;
        }
    }
    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        SQuery = ViewState["Squery"].ToString();
        DataTable dt1 = new DataTable();
        dt1 = fgen.search_vip(frm_qstr, co_cd, SQuery, txtsearch.Text.Trim().ToUpper());
        if (dt1.Rows.Count > 0)
        {
            sg1.DataSource = dt1;
            sg1.DataBind();
            dt1.Dispose();
        }
        else fgen.msg("-", "AMSG", "No Data Found Like'13'" + txtsearch.Text.Trim());
    }
    public void send_m(string appr_Status, string info)
    {
        string xmail_body = "";
        xmail_body = xmail_body + "<html><body>";
        xmail_body = xmail_body + "Sir, <br><br>";
        xmail_body = xmail_body + "Complaint No. " + info.Substring(4, 6) + " has been " + appr_Status.Replace("Y", "Approved").Replace("R", "Rejected") + " by " + frm_uname + "<br><br>";
        xmail_body = xmail_body + "Thanks & Regards,<br>";
        xmail_body = xmail_body + "For " + fgenCO.chk_co(co_cd) + "<br><br>";
        xmail_body = xmail_body + "<b>Note: Please respond to concerned BUYER only as this is the system generated E-Mail. Buyer Name given in the pending details.</b><br>";

        //fgen.send_mail("ERP ERP", "info@neopaints.co.in", "", "vipin@ERP.in", "Customer Complaint " + appr_Status.Replace("Y", "Approved").Replace("R", "Rejected"), xmail_body, "smtp.gmail.com", 587, 1, "ERPerpmail@gmail.com", "ERPerp123");
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
            dt.Dispose();
            setGridWidt();
        }
        else
        {
            enablectrl(); fgen.DisableForm(this.Controls);
            fgen.msg("-", "AMSG", "No Data for selected Time period");
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
}