using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class neopappmst : System.Web.UI.Page
{
    string uname, col1, vardate, fromdt, todt, DateRange, year, ulvl, SQuery;
    DataTable dt, dt1; DataRow oporow, dr1; int i;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
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
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); set_Val();
            }
            btnedit.Focus();
        }
    }
    public void enablectrl()
    {
        btnedit.Disabled = false; btnsave.Disabled = true; btnext.AccessKey = "X";
        btnext.Text = " Exit "; btnext.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void disablectrl()
    {
        btnedit.Disabled = true; btnsave.Disabled = false; btnext.AccessKey = "C";
        btnext.Text = "Cancel"; btnext.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void clearctrl()
    {
        hffield.Value = "";
    }
    public void set_Val()
    {
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (frm_formID)
        {
            case "F10550":
                lblhead.Text = "Complaint Master";
                frm_vty = "CM";
                break;
            case "F10551":
                lblhead.Text = "Type of Complaint Master";
                frm_vty = "TC";
                break;
            case "F10552":
                if (frm_cocd == "CCEL") lblhead.Text = "Department Master";
                else lblhead.Text = "Division of Complaint Master";
                frm_vty = "DC";
                break;
            case "F10554":
                lblhead.Text = "Visit Master";
                frm_vty = "VC";
                break;
            case "29521":
                lblhead.Text = "Liner Source Master";
                frm_vty = "LC";
                break;
            case "29522":
                lblhead.Text = "Finishing Master";
                frm_vty = "FC";
                break;
            case "29523":
                lblhead.Text = "MFG Joint Master";
                frm_vty = "MC";
                break;
            case "29524":
                lblhead.Text = "Packing Master";
                frm_vty = "PC";
                break;
            case "29525":
                lblhead.Text = "Cost Master";
                frm_vty = "CC";
                break;
            case "29526":
                lblhead.Text = "Transport Master";
                frm_vty = "TC";
                break;
            case "29527":
                lblhead.Text = "Printing Master";
                frm_vty = "PR";
                break;
            case "29528":
                lblhead.Text = "PLY Master";
                frm_vty = "PL";
                break;
            case "15541":
                lblhead.Text = "Bhatti Master";
                frm_vty = "BC";
                break;
            case "15542":
                lblhead.Text = "Chimni Master";
                frm_vty = "NC";
                break;
            case "15543":
                lblhead.Text = "Plot Master";
                frm_vty = "PT";
                break;
            case "15544":
                lblhead.Text = "Shift Master";
                frm_vty = "SF";
                break;
            case "15545":
                lblhead.Text = "Fixture Details";
                frm_vty = "FX";
                break;
            case "15590":
                lblhead.Text = "Formula Master";
                frm_vty = "FM";
                break;
            case "15125":
                lblhead.Text = "Man Power Recording";
                frm_vty = "MP";
                break;
            case "F40116":
                lblhead.Text = "QC Observation Master";
                frm_vty = "QM";
                break;
        }
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        edmode.Value = "Y";
        dt = new DataTable();
        set_Val();
        dt = fgen.getdata(frm_qstr, frm_cocd, "Select SRNO,name,nvl(num4,0) as rate,acref as tk1,acref2 as tk2,acref3 as tk3,pageno,lineno,ent_by as tk4 FROM TYPEGRP WHERE ID='" + frm_vty + "' AND TYPE1='000000' order by srno");
        if (frm_formID == "15125")
        {
            if (dt.Rows.Count > 0) { }
            else
            {
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select type1 as srno,name,null as rate,null as tk1,null as tk2,null as tk3,null as tk4,null as pageno,null as lineno from type where id='1' order by type1");
            }
        }
        create_tab();
        foreach (DataRow dr in dt.Rows)
        {
            dr1 = dt1.NewRow();
            dr1["srno"] = dr["srno"];
            dr1["name"] = dr["name"].ToString();
            dr1["rate"] = dr["rate"].ToString();
            dr1["tk1"] = dr["tk1"].ToString();
            dr1["tk2"] = dr["tk2"].ToString();
            dr1["tk3"] = dr["tk3"].ToString();
            dr1["tk4"] = dr["tk4"].ToString();
            dr1["val1"] = dr["pageno"].ToString();
            dr1["val2"] = dr["lineno"].ToString();
            dt1.Rows.Add(dr1);
        }
        if (frm_formID != "15125")
        {
            for (int i = 0; i < 30; i++)
            {
                add_blankrows();
            }
        }
        ViewState["sg1"] = dt1;
        sg1.DataSource = dt1;
        sg1.DataBind();
        if (frm_cocd == "LIVN")
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                ((DropDownList)gr.FindControl("dd1")).SelectedValue = fgen.seek_iname(frm_qstr, frm_cocd, "Select pageno from typegrp where id='FM' and type1='000000' and srno='" + gr.Cells[2].Text.Trim() + "'", "pageno").Trim();
                ((DropDownList)gr.FindControl("dd2")).SelectedValue = fgen.seek_iname(frm_qstr, frm_cocd, "Select lineno from typegrp where id='FM' and type1='000000' and srno='" + gr.Cells[2].Text.Trim() + "'", "lineno").Trim();
            }
        }
        fgen.EnableForm(this.Controls); disablectrl(); sg1.Focus();
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        if (sg1.Rows.Count > 0) fgen.msg("-", "SMSG", "Are you Sure!! you want to save");
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ") Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        else
        {
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            clearctrl();
            enablectrl();
            ViewState["sg1"] = null;
            sg1.DataSource = null;
            sg1.DataBind();
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = "";
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 != "Y") { }
        else
        {
            set_Val();            
            if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update TYPEGRP set branchcd='DD' WHERE ID='" + frm_vty + "' AND TYPE1='000000'");

            DataSet oDS = new DataSet();
            oDS = fgen.fill_schema(frm_qstr, frm_cocd, "TYPEGRP");

            foreach (GridViewRow r in sg1.Rows)
            {
                if (((TextBox)r.FindControl("txtname")).Text.Trim().ToUpper().Length > 1 || ((TextBox)r.FindControl("txtname")).Text.Trim().ToUpper() != "-")
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["id"] = frm_vty;
                    oporow["TYPE1"] = "000000";
                    oporow["srno"] = r.RowIndex + 1;
                    oporow["name"] = ((TextBox)r.FindControl("txtname")).Text.Trim().ToUpper();
                    oporow["NUM4"] = Convert.ToDouble(((TextBox)r.FindControl("txtrate")).Text.Trim().Replace("-", "0"));

                    oporow["ACREF"] = ((TextBox)r.FindControl("TK1")).Text.Trim().ToUpper();
                    oporow["ACREF2"] = ((TextBox)r.FindControl("TK2")).Text.Trim().ToUpper();
                    oporow["ACREF3"] = ((TextBox)r.FindControl("TK3")).Text.Trim().ToUpper();
                    oporow["pageno"] = ((DropDownList)r.FindControl("dd1")).SelectedValue;
                    oporow["lineno"] = ((DropDownList)r.FindControl("dd2")).SelectedValue;
                    oporow["ent_by"] = ((TextBox)r.FindControl("TK4")).Text.Trim().ToUpper();
                    oDS.Tables[0].Rows.Add(oporow);
                }
            }
            fgen.save_data(frm_qstr, frm_cocd, oDS, "TYPEGRP");

            if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "delete from typegrp where branchcd='DD' and id='" + frm_vty + "' AND TYPE1='000000'");
            fgen.msg("-", "AMSG", "Data Saved Successfully");
            fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
            ViewState["sg1"] = null;
            sg1.DataSource = null;
            sg1.DataBind();
        }
    }
    public void create_tab()
    {
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
        dt1.Columns.Add(new DataColumn("name", typeof(string)));
        dt1.Columns.Add(new DataColumn("rate", typeof(string)));
        dt1.Columns.Add(new DataColumn("tk1", typeof(string)));
        dt1.Columns.Add(new DataColumn("tk2", typeof(string)));
        dt1.Columns.Add(new DataColumn("tk3", typeof(string)));
        dt1.Columns.Add(new DataColumn("tk4", typeof(string)));
        dt1.Columns.Add(new DataColumn("val1", typeof(string)));
        dt1.Columns.Add(new DataColumn("val2", typeof(string)));
        dt1.Columns.Add(new DataColumn("val3", typeof(string)));
    }
    public void add_blankrows()
    {
        dr1 = dt1.NewRow();
        dr1["Srno"] = dt1.Rows.Count + 1;
        dr1["name"] = "-";
        dr1["rate"] = "0";
        dr1["tk1"] = "0";
        dr1["tk2"] = "0";
        dr1["tk3"] = "0";
        dr1["tk4"] = "0";
        dr1["val1"] = "0";
        dr1["val2"] = "0";
        dr1["val3"] = "0";
        dt1.Rows.Add(dr1);
    }
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "Rmv":
                if (index < sg1.Rows.Count - 1)
                {
                    dt = new DataTable();
                    dt = (DataTable)ViewState["sg1"];
                    dt.Rows[Convert.ToInt32(index)].Delete();
                    ViewState["sg1"] = dt;
                    sg1.DataSource = dt;
                    sg1.DataBind();
                }
                break;
            case "Add":
                if (ViewState["sg1"] != null)
                {
                    dt = new DataTable();
                    dt1 = new DataTable();
                    dt = (DataTable)ViewState["sg1"];
                    dt1 = dt.Clone();
                    dr1 = null;
                    for (i = 0; i < sg1.Rows.Count; i++)
                    {
                        dr1 = dt1.NewRow();
                        dr1["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                        dr1["name"] = ((TextBox)sg1.Rows[i].FindControl("txtname")).Text.Trim().ToUpper();
                        dr1["rate"] = ((TextBox)sg1.Rows[i].FindControl("txtrate")).Text.Trim().ToUpper();
                        dr1["TK1"] = ((TextBox)sg1.Rows[i].FindControl("TK1")).Text.Trim().ToUpper();
                        dr1["TK2"] = ((TextBox)sg1.Rows[i].FindControl("TK2")).Text.Trim().ToUpper();
                        dr1["TK3"] = ((TextBox)sg1.Rows[i].FindControl("TK3")).Text.Trim().ToUpper();
                        dr1["TK4"] = ((TextBox)sg1.Rows[i].FindControl("TK4")).Text.Trim().ToUpper();
                        dr1["val1"] = ((DropDownList)sg1.Rows[i].FindControl("dd1")).SelectedValue.ToString();
                        dr1["val2"] = ((DropDownList)sg1.Rows[i].FindControl("dd2")).SelectedValue.ToString();
                        dt1.Rows.Add(dr1);
                    }

                    add_blankrows();
                }

                ViewState["sg1"] = dt1;
                sg1.DataSource = dt1;
                sg1.DataBind();

                foreach (GridViewRow gr in sg1.Rows)
                {
                    foreach (DataRow dt1_dr in dt1.Rows)
                    {
                        if (gr.Cells[2].Text.Trim() == dt1_dr["srno"].ToString().Trim())
                        {
                            ((DropDownList)sg1.Rows[i].FindControl("dd1")).SelectedValue = dt1_dr["val1"].ToString();
                            ((DropDownList)sg1.Rows[i].FindControl("dd2")).SelectedValue = dt1_dr["val2"].ToString();
                        }
                    }
                }
                break;
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            sg1.HeaderRow.Cells[4].Style["display"] = "none";
            e.Row.Cells[4].Style["display"] = "none";
            sg1.HeaderRow.Cells[5].Style["display"] = "none";
            e.Row.Cells[5].Style["display"] = "none";
            sg1.HeaderRow.Cells[6].Style["display"] = "none";
            e.Row.Cells[6].Style["display"] = "none";
            sg1.HeaderRow.Cells[7].Style["display"] = "none";
            e.Row.Cells[7].Style["display"] = "none";
            sg1.HeaderRow.Cells[8].Style["display"] = "none";
            e.Row.Cells[8].Style["display"] = "none";
            sg1.HeaderRow.Cells[9].Style["display"] = "none";
            e.Row.Cells[9].Style["display"] = "none";
            sg1.HeaderRow.Cells[10].Style["display"] = "none";
            e.Row.Cells[10].Style["display"] = "none";
            if (frm_cocd != "STOR")
            {
                sg1.HeaderRow.Cells[4].Style["display"] = "none";
                e.Row.Cells[4].Style["display"] = "none";
                //sg1.HeaderRow.Cells[5].Style["display"] = "none";
                //e.Row.Cells[5].Style["display"] = "none";
            }
            if (frm_cocd == "STOR")
            {
                switch (frm_formID)
                {
                    case "29527":
                    case "29528":
                        //sg1.HeaderRow.Cells[3].Style["display"] = "none";
                        //e.Row.Cells[3].Style["display"] = "none";
                        sg1.HeaderRow.Cells[4].Text = "Value";
                        sg1.HeaderRow.Cells[5].Style["display"] = "none";
                        e.Row.Cells[5].Style["display"] = "none";
                        break;
                }
            }
            if (frm_cocd == "LIVN")
            {
                sg1.HeaderRow.Cells[4].Style["display"] = "none";
                e.Row.Cells[4].Style["display"] = "none";
                sg1.HeaderRow.Cells[5].Style["display"] = "show";
                e.Row.Cells[5].Style["display"] = "shoq";
                sg1.HeaderRow.Cells[6].Style["display"] = "show";
                e.Row.Cells[6].Style["display"] = "show";
                sg1.HeaderRow.Cells[7].Style["display"] = "show";
                e.Row.Cells[7].Style["display"] = "show";
                sg1.HeaderRow.Cells[8].Style["display"] = "show";
                e.Row.Cells[8].Style["display"] = "show";
                sg1.HeaderRow.Cells[9].Style["display"] = "show";
                e.Row.Cells[9].Style["display"] = "show";
                sg1.HeaderRow.Cells[10].Style["display"] = "show";
                e.Row.Cells[10].Style["display"] = "show";
                sg1.HeaderRow.Cells[5].Text = "Fabric";
                sg1.HeaderRow.Cells[6].Text = "Alluminium";
                sg1.HeaderRow.Cells[7].Text = "Palmat";
                sg1.HeaderRow.Cells[8].Text = "Drop";
                sg1.HeaderRow.Cells[9].Text = "Type";
                sg1.HeaderRow.Cells[10].Text = "Blind Type";
            }
            if (frm_formID == "15125")
            {
                sg1.HeaderRow.Cells[0].Style["display"] = "none";
                e.Row.Cells[0].Style["display"] = "none";
                sg1.HeaderRow.Cells[1].Style["display"] = "none";
                e.Row.Cells[1].Style["display"] = "none";
                sg1.HeaderRow.Cells[4].Style["display"] = "show";
                e.Row.Cells[4].Style["display"] = "shoq";

                sg1.HeaderRow.Cells[2].Text = "Stage";
                sg1.HeaderRow.Cells[3].Text = "Stage Name";
                sg1.HeaderRow.Cells[4].Text = "Man Power";
                ((TextBox)e.Row.FindControl("txtname")).ReadOnly = true;
            }
        }
    }
}