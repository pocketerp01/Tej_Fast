using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class om_mnu_opts : System.Web.UI.Page
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

            case "F99115":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_REQ_RZN", "Y");
                lblheader.Text = "ERP Features Management";
                break;

            default: lblheader.Text = "";
                break;
        }
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        set_val();
        clearctrl();
        //hffield.Value = "New";
        hffield.Value = "New_E";
        fgen.msg("-", "CMSG", "Do you want to Choose Modules(Preferred Mode)?'13'(No for all Modules)");
        //fgen.Fn_open_prddmp1("-", frm_qstr);

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
                { fgen.msg("-", "AMSG", "You Can not select both checkboxes'13'See at Entry No. " + row.Cells[6].Text.Trim()); i = 0; return; }
                else
                {
                    if (chk1.Checked == true || chk2.Checked == true)
                    {
                        if (HCID == "**M10015A" || HCID == "**M11015A") i = 1;
                        else
                        {
                            //if ((MREQ_RZN == "Y") && chk2.Checked == true && ((TextBox)row.FindControl("txtreason")).Text.Trim().Length < 1)
                            //{
                            //    fgen.msg("-", "AMSG", "Please enter the Reason  for Archival '13'See Option : " + row.Cells[6].Text.Trim() + " ID : " + row.Cells[5].Text.Trim());
                            //    i = 0;
                            //    return;
                            //}
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


            case "F99115":
                switch (btnval)
                {
                    case "New_E":
                        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                        if (col1 == "N")
                        {
                            SQuery = "select ID,Text,Form,mlevel,VISI,BRN,PRD,UPD_BY,UPD_DT,nvl(search_key,'-') as search_key,web_action,submenu,submenuid,param from FIN_MSYS order by form,id";
                        }

                        else
                        {
                            hffield.Value = "VW";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select distinct Form as fstr,Text as Module_Name,Form as Form_ID,search_key from FIN_MSYS where mlevel=1 order by Form");
                            fgen.Fn_open_mseek("-", frm_qstr);
                        }
                        break;
                    case "VW":
                        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                        SQuery = "select ID,Text,Form,mlevel,VISI,BRN,PRD,UPD_BY,UPD_DT,nvl(search_key,'-') as search_key,web_action,submenu,submenuid,param from FIN_MSYS where trim(form) in (" + col1 + ") order by form,id";
                        break;
                }
                break;

            case "*M10015B":

                break;
        }
        if (SQuery.Length > 0)
        {
            fgen.EnableForm(this.Controls); disablectrl();
            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
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
            mdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
            mdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
            mprdrange = "between to_date('" + mdt1 + "','dd/mm/yyyy') and to_date('" + mdt2 + "','dd/mm/yyyy')";
            ViewState["fromdt"] = col1; ViewState["todt"] = col2;
            SQuery = "";
            if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
            switch (HCID)
            {

                case "F99115":
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2").ToString().Trim().Replace("&amp", "");
                    ViewState["fromdt"] = col1; ViewState["todt"] = col2;
                    hffield.Value = "New_E";
                    fgen.msg("-", "CMSG", "Do you want to Choose Modules(Preferred Mode)?'13'(No for all Modules)");
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
            Create_Icons ICO = new Create_Icons();
            col1 = "";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                string mytable;
                mytable = "FIN_MSYS";
                foreach (GridViewRow row in sg1.Rows)
                {
                    CheckBox chk1 = (CheckBox)row.FindControl("chkapp");
                    CheckBox chk2 = (CheckBox)row.FindControl("chkrej");
                    TextBox tk = (TextBox)row.FindControl("txtcompdt");
                    TextBox mreason = (TextBox)row.FindControl("txtreason");
                    string menu_descr = mreason.Text.ToString();


                    string myappno;
                    string myappdt;
                    string myquery;

                    myappno = "app_by";
                    myappdt = "app_dt";
                    myquery = "";
                    // HCID = Request.Cookies["rid"].Value.ToString();
                    HCID = frm_formID;
                    switch (HCID)
                    {

                        case "F99115":
                            myappno = "upd_by";
                            myappdt = "upd_dt";
                            if (chk1.Checked == true)
                            {
                                //myquery = "update " + mytable + " set search_key='" + menu_descr + "',VISI='Y'," + myappno + "='" + frm_uname + "'," + myappdt + "=sysdate where trim(ID) ='" + row.Cells[5].Text.Trim() + "'";

                                myquery = "update " + mytable + " set id='DD" + row.Cells[5].Text.Trim() + "' where trim(ID) ='" + row.Cells[5].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);

                                ICO.add_icon(frm_qstr, row.Cells[5].Text.Trim(), fgen.make_int(row.Cells[8].Text.Trim()), row.Cells[6].Text.Trim(), 1, row.Cells[15].Text.Trim(), menu_descr, row.Cells[16].Text.Trim(), row.Cells[17].Text.Trim(), row.Cells[7].Text.Trim(), row.Cells[18].Text.Trim(), "fa-edit", row.Cells[10].Text.Trim(), row.Cells[11].Text.Trim(), "Y");

                                myquery = "Delete from " + mytable + " where  id='DD" + row.Cells[5].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            else if (chk2.Checked == true)
                            {
                                myquery = "update " + mytable + " set id='DD" + row.Cells[5].Text.Trim() + "' where trim(ID) ='" + row.Cells[5].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);

                                ICO.add_icon(frm_qstr, row.Cells[5].Text.Trim(), fgen.make_int(row.Cells[8].Text.Trim()), row.Cells[6].Text.Trim(), 1, row.Cells[15].Text.Trim(), menu_descr, row.Cells[16].Text.Trim(), row.Cells[17].Text.Trim(), row.Cells[7].Text.Trim(), row.Cells[18].Text.Trim(), "fa-edit", row.Cells[10].Text.Trim(), row.Cells[11].Text.Trim(), "N");

                                myquery = "Delete from " + mytable + " where  id='DD" + row.Cells[5].Text.Trim() + "'";
                                fgen.execute_cmd(frm_qstr, co_cd, myquery);
                            }
                            break;

                        case "M10024":
                            //lblheader.Text = "Sales Sch. Approval";
                            break;
                    }
                }
                if (HCID == "F15161" || HCID == "M02032" || HCID == "M10010B" || HCID == "M11010B") fgen.msg("-", "AMSG", "Document Checking Successfully completed");
                else fgen.msg("-", "AMSG", "Feature Activation / Archival Successfully completed");
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
                case "F99115":
                    ViewState["OrigData"] = e.Row.Cells[11].Text;
                    if (e.Row.Cells[11].Text.Length >= 25)
                    {
                        e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 25) + "...";
                        e.Row.Cells[11].ToolTip = ViewState["OrigData"].ToString();
                    }

                    //DateTime date1 = Convert.ToDateTime(vardate);

                    ((TextBox)(e.Row.Cells[2].FindControl("txtreason"))).Text = e.Row.Cells[14].Text;

                    e.Row.Cells[2].Style["display"] = "none";
                    sg1.HeaderRow.Cells[2].Style["display"] = "none";

                    e.Row.Cells[3].Style["display"] = "none";
                    sg1.HeaderRow.Cells[3].Style["display"] = "none";

                    //e.Row.Cells[4].Style["display"] = "none";
                    //sg1.HeaderRow.Cells[4].Style["display"] = "none";
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
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        HCID = frm_formID;

        switch (var)
        {
            case "Show":
                switch (HCID)
                {


                    case "F60186":
                        try
                        {
                            col2 = fgen.seek_iname(frm_qstr, co_cd, "SELECT FILENAME||'^'||FILEPATH AS FSTR from WB_CSS_ACT where branchcd||type||trim(actno)||to_char(actdt,'dd/mm/yyyy') ='" + sg1.Rows[rowIndex].Cells[5].Text.Trim() + "'", "FSTR");
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

        //fgen.send_mail("Tejaxo ERP", "info@neopaints.co.in", "", "info@pocketdriver.in", "Customer Complaint " + appr_Status.Replace("Y", "Approved").Replace("R", "Rejected"), xmail_body, "smtp.gmail.com", 587, 1, "rrrbaghel@gmail.com", "finsyserp123");
    }
}