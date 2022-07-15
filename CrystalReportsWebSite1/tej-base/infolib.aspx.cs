using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class om_appr : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, col1, col2, mbr, vardate, year, ulvl, HCID, xprdrange, cond, fromdt, todt, CSR;
    string frm_uname, frm_url, frm_qstr, frm_formID, DateRange, frm_UserID, cstr;
    string mdt1, mdt2, mprdrange;
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
        btnext.AccessKey = "X";
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
    }
    public void clearctrl()
    { hffield.Value = ""; }
    public void set_val()
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "N");
        HCID = frm_formID;
        switch (HCID)
        {
            case "F96107":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "DSL/STL Library";
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

    }
    protected void btnhideF_Click(object sender, EventArgs e) { }
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
            if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
            cond = "";
            switch (HCID)
            {
                case "F96107":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.DSLNO)||to_char(a.DSLdt,'dd/mm/yyyy') as fstr,to_Char(a.DSLdt,'yyyymmdd') as vdd,a.CCode,a.DSLNO as DSL_No,to_Char(A.DSLdt,'dd/mm/yyyy') as DSL_Dt,a.Emodule as Module,a.Eicon as css_Icon,a.epurpose as purposem,a.cont_name,a.cont_no,a.cont_email,a.REMARKS,a.wrkrmk,A.Ent_by,TO_CHAR(A.ENT_dT,'DD/mm/yyyy') as Ent_Dt from wb_DSL_log a where a.branchcd='" + mbr + "' and a.type='SL' and a.DSLdt " + mprdrange + " " + cond + " order by vdd,a.DSLNO";
                    break;
            }
            if (SQuery.Length > 0)
            {
                fgen.EnableForm(this.Controls); disablectrl();
                dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    ViewState["Squery"] = SQuery;
                    sg1.DataSource = dt;
                    sg1.DataBind();
                    sg1.Visible = true;
                    dt.Dispose();
                    int col_count = 0;
                    if (sg1.Rows.Count > 0)
                    {
                        col_count = sg1.HeaderRow.Cells.Count;
                        double wid = 1500;
                        for (int i = 0; i < col_count; i++)
                        {
                            wid += fgen.make_double(sg1.Columns[0].ItemStyle.Width.Value, 0);
                        }
                        if (col_count < 5) sg1.Width = 1200;
                        try { sg1.Width = Convert.ToUInt16(wid + 100); }
                        catch { sg1.Width = 1500; }
                    }

                    lblTotcount.Text = "Total Rows : " + sg1.Rows.Count;
                }
                else
                {
                    enablectrl(); fgen.DisableForm(this.Controls);
                    fgen.msg("-", "AMSG", "No Data for selected Time period");
                }
            }
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HCID = frm_formID;
            switch (HCID)
            {
                case "F96107":
                    string chk_rights = fgen.Fn_chk_can_edit(frm_qstr, co_cd, frm_UserID, frm_formID);
                    if (chk_rights != "Y")
                    {
                        e.Row.Cells[0].Style["display"] = "none";
                        sg1.HeaderRow.Cells[0].Style["display"] = "none";
                    }
                    e.Row.Cells[1].Style["display"] = "none";
                    sg1.HeaderRow.Cells[1].Style["display"] = "none";
                    e.Row.Cells[2].Style["display"] = "none";
                    sg1.HeaderRow.Cells[2].Style["display"] = "none";
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
            lblTotcount.Text = "";
        }
    }
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        HCID = frm_formID;

        switch (HCID)
        {
            case "F96107":
                try
                {
                    col2 = fgen.seek_iname(frm_qstr, co_cd, "SELECT FILENAME||'^'||FILEPATH AS FSTR from WB_DSL_LOG where branchcd||type||trim(dslno)||to_char(dsldt,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[1].Text.Trim() + "'", "FSTR");
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
}