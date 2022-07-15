using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class appr : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, uname, col1, col2, mbr, vardate, year, ulvl, HCID, xprdrange, cond, fromdt, todt, frm_url, frm_qstr, frm_formID, DateRange, frm_UserID, cstr;
    DataTable dt;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
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
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    // uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(co_cd, frm_qstr);
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
                vardate = fgen.seek_iname(frm_qstr,co_cd, "select to_date(to_char(sysdate,'dd/MM/YYYY'),'DD/MM/YYYY') AS DT FROM DUAL", "DT");
            }
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnsave.Disabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnext.Text = " Exit "; btnext.Enabled = true; srch.Enabled = false;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnsave.Disabled = false; tkrow.Text = "20"; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnext.Text = "Cancel"; btnext.Enabled = true; srch.Enabled = true;
    }
    public void clearctrl()
    { hffield.Value = ""; }
    public void set_val()
    {
        HCID = frm_formID;
        //  HCID = "99001";
        switch (HCID)
        {
            case "M02024":
                lblheader.Text = "Sales Order Checking";
                break;
            case "99702":
            case "99001":
                lblheader.Text = "Task Approval";
                break;
            case "25051":
                lblheader.Text = "Customer Complaint Approval";
                if (co_cd == "CCEL") lblheader.Text = "Customer Request Approval";
                break;
            case "70002":
                lblheader.Text = "SDR Completion Approval";
                if (co_cd == "CCEL") lblheader.Text = "Customer Request Approval";
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
            i = 0;
            foreach (GridViewRow row in sg1.Rows)
            {
                CheckBox chk1 = (CheckBox)row.FindControl("chkapp");
                CheckBox chk2 = (CheckBox)row.FindControl("chkrej");
                TextBox tk = (TextBox)row.FindControl("txtcompdt");
                if (HCID == "25051") { TextBox tkreason = (TextBox)row.FindControl("txtreason"); }

                if (chk1.Checked == true && chk2.Checked == true)
                { fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Entry No. " + row.Cells[3].Text.Trim()); i = 0; return; }
                else
                {
                    if (chk1.Checked == true || chk2.Checked == true)
                    {
                        if (HCID == "M02024") i = 1;
                        else
                        {
                            i = fgen.ChkDate(tk.Text.Trim());
                            if (i != 0) i = 1;
                            else
                            { fgen.msg("-", "AMSG", "Not a valid date entered infront of'13'Entry No. " + row.Cells[3].Text.Trim()); return; }
                            if (HCID == "25051" && Convert.ToDateTime(tk.Text.Trim()) < Convert.ToDateTime(System.DateTime.Now.ToShortDateString()))
                            { fgen.msg("-", "AMSG", "Date can not be less then present Date'13'See at Entry No. " + row.Cells[3].Text.Trim()); i = 0; return; }
                            if (HCID == "25051" && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                            { fgen.msg("-", "AMSG", "Please enter the reason for Refusal'13'See at Entry No. " + row.Cells[3].Text.Trim()); i = 0; return; }
                        }
                    }
                }
            }
            if (i != 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        }
        else
        {
            if (HCID == "M02024") fgen.msg("-", "AMSG", "Please approve any one row to save");
            else fgen.msg("-", "AMSG", "Please approve or refuse any one row to save");
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
                        if (col1 == "N") SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
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
                        if (col1.Trim().Length == 4) SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')'-' and vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') and trim(acode) in ('" + col1 + "') and ent_by='" + uname + "' GROUP BY vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
                        else SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') user_name,to_char(docdate,'dd/mm/yyyy') as task_date,col14 as subject,remarks as message,ent_by as assign_by,to_char(ent_dt,'dd/mm/yyyy') as assign_dt from scratch where branchcd='" + mbr + "' and type='DK' and nvl(col3,'-')='-' and vchdate between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy') and trim(acode) in (" + col1 + ") and ent_by='" + uname + "' group by vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(docdate,'dd/mm/yyyy'),col14,remarks,ent_by,to_char(ent_dt,'dd/mm/yyyy')";
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
            case "M02024":
                switch (btnval)
                {
                    case "New_E":
                        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                        if (col1 == "N")
                        {
                            xprdrange = "between to_date('" + ViewState["fromdt"].ToString() + "','dd/mm/yyyy') and to_date('" + ViewState["todt"].ToString() + "','dd/mm/yyyy')";
                            if (co_cd == "NEOP")
                            {
                                if (ulvl == "0") SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " order by vdd desc,a.ordno,A.TYPE";
                                else
                                {
                                    col1 = ""; col1 = "";
                                    col1 = fgen.seek_iname(frm_qstr,co_cd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + uname + "'", "icons");
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
                            else SQuery = "Select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(A.acode) as fstr, a.ordno as ord_no,to_char(a.orddt,'dd/mm/yyyy') as ord_dt,A.TYPE,b.aname as party,to_Char(a.orddt,'yyyymmdd') as vdd from somas a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '4%' and nvl(trim(a.check_by),'-')='-' and a.orddt " + xprdrange + " order by vdd desc,a.ordno,A.TYPE";
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
                                col1 = fgen.seek_iname(frm_qstr,co_cd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + uname + "'", "icons");
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
            dt = fgen.getdata(frm_qstr,co_cd, SQuery);
            if (dt.Rows.Count > 0)
            {
                ViewState["Squery"] = SQuery;
                ViewState["sg1"] = dt;
                sg1.DataSource = dt;
                sg1.DataBind();
                sg1.Visible = true;
                dt.Dispose();
            }
            else
            {
                enablectrl(); fgen.DisableForm(this.Controls);
                fgen.msg("-", "AMSG", "No Data for selected Time period");
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "New")
        {
            HCID = frm_formID;
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            SQuery = "";
            switch (HCID)
            {
                case "99702":
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
                case "M02024":
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
                        SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as complaint_no,to_Char(A.vchdate,'dd/mm/yyyy') as complaint_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') and trim(a.ent_by)='" + uname + "' order by vdd";
                        if (co_cd == "SRIS") SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as complaint_no,to_Char(A.vchdate,'dd/mm/yyyy') as complaint_dt,a.acode as code,b.aname as customer,c.iname as product,c.cpartno as partcode,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_dt,a.col2 as nature_of_cmplnt,a.col3 as type_of_complnt,A.ENt_BY,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='CC' and nvl(trim(a.app_by),'-')='-' and a.vchdate between to_date('" + col1 + "','dd/mm/yyyy') and to_date('" + col2 + "','dd/mm/yyyy') order by vdd";
                    }
                    break;
            }
            if (SQuery.Length > 0)
            {
                fgen.EnableForm(this.Controls); disablectrl();
                dt = fgen.getdata(frm_qstr,co_cd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    ViewState["Squery"] = SQuery;
                    sg1.DataSource = dt;
                    sg1.DataBind();
                    sg1.Visible = true;
                    dt.Dispose();
                }
                else
                {
                    enablectrl(); fgen.DisableForm(this.Controls);
                    fgen.msg("-", "AMSG", "No Data for selected Time period");
                }
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
                    // HCID = Request.Cookies["rid"].Value.ToString();
                    HCID = frm_formID;
                    switch (HCID)
                    {
                        case "99702":
                        case "99001":
                            if (chk1.Checked == true || chk2.Checked == true)
                            {
                                if (chk1.Checked == true) fgen.execute_cmd(frm_qstr,co_cd, "update scratch set col3='[A]" + uname + "',col4='" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "' where type='DK' and branchcd='" + mbr + "' and vchnum='" + row.Cells[5].Text.Trim() + "'");
                                else if (chk2.Checked == true) fgen.execute_cmd(frm_qstr,co_cd, "update scratch set col3='[R]" + uname + "',col4='" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "' where type='DK' and branchcd='" + mbr + "' and vchnum='" + row.Cells[5].Text.Trim() + "'");
                            }
                            break;
                        case "70002":
                            if (chk1.Checked == true || chk2.Checked == true)
                            {
                                if (chk1.Checked == true) fgen.execute_cmd(frm_qstr,co_cd, "update scratch2 set col3='[A]" + uname + "',col4='" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "' where type='SD' and branchcd='" + mbr + "' and vchnum='" + row.Cells[5].Text.Trim() + "'");
                                else if (chk2.Checked == true) fgen.execute_cmd(frm_qstr,co_cd, "update scratch set col3='[R]" + uname + "',col4='" + Convert.ToDateTime(tk.Text.Trim()).ToString("dd/MM/yyyy") + "' where type='SD' and branchcd='" + mbr + "' and vchnum='" + row.Cells[5].Text.Trim() + "'");
                            }
                            break;
                        case "M02024":
                            if (chk1.Checked == true) fgen.execute_cmd(frm_qstr,co_cd, "update somas set check_by='" + uname + "' , check_dt=TO_DATE('" + vardate + "','DD/MM/YYYY') where branchcd||type||trim(ordno)||to_Char(orddt,'dd/mm/yyyy')||trim(Acode)='" + row.Cells[5].Text.Trim() + "' ");
                            break;
                        case "25051":
                            if (chk1.Checked == true || chk2.Checked == true)
                            {
                                if (chk1.Checked == true)
                                {
                                    fgen.execute_cmd(frm_qstr,co_cd, "update scratch set app_by='[A]" + uname + "',app_dt=to_date('" + tk.Text.Trim() + "','dd/mm/yyyy') where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + row.Cells[5].Text.Trim() + "'");
                                    send_m("A", row.Cells[5].Text.Trim());
                                }
                                else if (chk2.Checked == true)
                                {
                                    fgen.execute_cmd(frm_qstr,co_cd, "update scratch set app_by='[R]" + uname + "',app_dt=to_date('" + tk.Text.Trim() + "','dd/mm/yyyy') where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + row.Cells[5].Text.Trim() + "'");
                                    send_m("R", row.Cells[5].Text.Trim());
                                }
                            }
                            break;
                    }
                }
                if (HCID == "M02024") fgen.msg("-", "AMSG", "Order Checking Successfully completed");
                else fgen.msg("-", "AMSG", "Approval / Refusal Successfully completed");
                enablectrl(); sg1.DataSource = null; sg1.DataBind(); sg1.Visible = false;
                fgen.DisableForm(this.Controls); btnnew.Focus();
            }
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HCID = frm_formID;
            sg1.HeaderRow.Cells[0].Width = 50;
            e.Row.Cells[0].Width = 50;
            sg1.HeaderRow.Cells[0].Style["text-align"] = "center";
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            sg1.HeaderRow.Cells[1].Width = 50;
            e.Row.Cells[1].Width = 50;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            sg1.HeaderRow.Cells[1].Style["text-align"] = "center";
            switch (HCID)
            {
                case "99702":
                case "99001":
                    DateTime date = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date.ToString("yyyy-MM-dd");
                    e.Row.Cells[3].Style["display"] = "none";
                    sg1.HeaderRow.Cells[3].Style["display"] = "none";
                    e.Row.Cells[4].Style["display"] = "none";
                    sg1.HeaderRow.Cells[4].Style["display"] = "none";
                    break;
                case "70002":
                    ViewState["OrigData"] = e.Row.Cells[11].Text;
                    if (e.Row.Cells[11].Text.Length >= 25)
                    {
                        e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 25) + "...";
                        e.Row.Cells[11].ToolTip = ViewState["OrigData"].ToString();
                    }
                    sg1.HeaderRow.Cells[2].Text = "Approved On";
                    DateTime date1 = Convert.ToDateTime(vardate);
                    ((TextBox)(e.Row.Cells[2].FindControl("txtcompdt"))).Text = date1.ToString("yyyy-MM-dd");
                    e.Row.Cells[3].Style["display"] = "none";
                    sg1.HeaderRow.Cells[3].Style["display"] = "none";
                    e.Row.Cells[4].Style["display"] = "none";
                    sg1.HeaderRow.Cells[4].Style["display"] = "none";
                    break;
                case "M02024":
                    e.Row.Cells[1].Style["display"] = "none";
                    sg1.HeaderRow.Cells[1].Style["display"] = "none";
                    e.Row.Cells[2].Style["display"] = "none";
                    sg1.HeaderRow.Cells[2].Style["display"] = "none";
                    e.Row.Cells[4].Style["display"] = "none";
                    sg1.HeaderRow.Cells[4].Style["display"] = "none";
                    e.Row.Cells[5].Style["display"] = "none";
                    sg1.HeaderRow.Cells[5].Style["display"] = "none";
                    break;
                case "25051":
                    e.Row.Cells[5].Style["display"] = "none";
                    sg1.HeaderRow.Cells[5].Style["display"] = "none";
                    break;
            }
        }
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ")
        { Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr); }
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
        HCID = Request.Cookies["rid"].Value.ToString();

        switch (var)
        {
            case "Show":
                switch (HCID)
                {
                    case "M02024":
                        SQuery = "select DISTINCT 'Sales Order Acknowledgement' as header,'SOMAS' AS TAB_NAME,g.*,d.aname as consname,d.addr1 as consaddr1,d.addr2 as consaddr2,d.addr3 as consaddr3 from (Select distinct a.branchcd,a.type,a.ordno, to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,a.cscode,a.st_type,a.srno,a.icode,c.iname,c.unit,a.ciname,a.cpartno,a.qtyord,a.irate,desc_,a.cdisc,a.pexc,a.ptax,to_char(a.del_date ,'dd/mm/yyyy') as del_date,a.weight,a.sd,a.ipack,a.class,a.iexc_addl,a.qtysupp,a.delivery,a.icat,a.pordno,to_char(a.porddt ,'dd/mm/yyyy') as porddt,a.amdt1,a.amdt2,a.amdt3,a.thru,a.del_wk,a.currency,a.curr_rate,a.work_ordno,a.gmt_shade,a.busi_expect,a.app_by,to_char(a.app_dt,'dd/mm/yyyy') as app_dt,a.othac1,a.othac2,a.othac3,a.othac4,a.othamt1,a.othamt2,a.othamt3,a.othamt4,a.shecess,a.basic,a.excise,a.cess,a.sta_amt,A.sta_rate,a.taxes,a.total,a.inspby,a.explic,a.attach1,a.desc3,a.desc2,a.desc1,a.ent_by,to_char( a.ent_dt,'dd/mm/yyyy') as ent_dt,b.aname,b.rc_num2 as pcstno,b.girno as ppanno,b.rc_num as ptinno,B.EXC_NUM as peccno,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.person as person,a.remark from somas a,famst b,item c  where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY')||trim(a.acode) in ('" + sg1.Rows[rowIndex].Cells[5].Text.Trim() + "'))g left outer join (select acode,aname,addr1,addr2,addr3 from csmst)d on trim(g.cscode)=trim(d.acode) order by g.orddt,g.ordno,g.srno";
                        fgen.Fn_Print_Report(co_cd, frm_qstr, mbr, SQuery, "sop_a", "sop_a");
                        break;
                    case "25051":
                        SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[rowIndex].Cells[5].Text.Trim() + "' order by vdd desc,a.srno";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Complaint List", frm_qstr);
                        break;
                }
                break;
        }
    }
    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        SQuery = ViewState["Squery"].ToString();
        DataTable dt1 = new DataTable();
        dt1 = fgen.search_vip(frm_qstr,co_cd, SQuery, txtsearch.Text.Trim().ToUpper());
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
        xmail_body = xmail_body + "Complaint No. " + info.Substring(4, 6) + " has been " + appr_Status.Replace("Y", "Approved").Replace("R", "Rejected") + " by " + uname + "<br><br>";
        xmail_body = xmail_body + "Thanks & Regards,<br>";
        //xmail_body = xmail_body + "For " + fgen.chk_co(co_cd) + "<br><br>";
        xmail_body = xmail_body + "<b>Note: Please respond to concerned BUYER only as this is the system generated E-Mail. Buyer Name given in the pending details.</b><br>";

        //fgen.send_mail("Tejaxo ERP", "info@neopaints.co.in", "", "info@pocketdriver.in", "Customer Complaint " + appr_Status.Replace("Y", "Approved").Replace("R", "Rejected"), xmail_body, "smtp.gmail.com", 587, 1, "rrrbaghel@gmail.com", "finsyserp123");
    }
}