using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class prsn_mst : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, uname, col1, col2, col3, mbr, cstr, vchnum, vardate, fromdt, todt, DateRange, year, ulvl, mhd;
    DataTable dt, dt1; DataRow oporow;

    string hfcid, vty, frm_url, frm_qstr, frm_formID;
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
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    hfhcid.Value = frm_formID;
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btnnew.Focus();
            }
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnext.Text = " Exit "; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = false;
        btnext.Text = "Cancel"; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    public void fill_dd()
    {
        dt1 = new DataTable();
        dt1 = fgen.getdata(frm_qstr, co_cd, "Select Distinct name,type1 from typegrp where id='DP' order by type1");
        dd1.DataSource = dt1;
        dd1.DataTextField = "Name";
        dd1.DataValueField = "type1";
        dd1.DataBind();
        dd1.Items.Insert(0, "---Select---");
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        fgen.EnableForm(this.Controls); disablectrl();
        if (hfhcid.Value == "25560A")
        {
            vchnum = fgen.next_no(frm_qstr, co_cd, "select substr(max(type1),2,3) as vch from typegrp where id='SE' and branchcd='" + mbr + "'", 3, "vch");
            txtvchnum.Text = "E" + vchnum;

        }
        if (hfhcid.Value == "25560B")
        {
            vchnum = fgen.next_no(frm_qstr, co_cd, "select substr(max(type1),2,3) as vch from typegrp where id='RT' and branchcd='" + mbr + "'", 3, "vch");
            txtvchnum.Text = vchnum;
        }

        else
        {

            vchnum = fgen.next_no(frm_qstr, co_cd, "select substr(max(type1),2,3) as vch from typegrp where id='SE' and branchcd='" + mbr + "'", 3, "vch");
            txtvchnum.Text = "E" + vchnum;

        }
        txtname.Focus();
        fill_dd();
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Edit";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        if (txtname.Text.Trim().Length <= 0 || txtname.Text.Trim() == null)
            fgen.msg("-", "AMSG", "Please Fill Person Name");
        else
        {
            if (co_cd == "SRDC") { dd1.SelectedIndex = 1; }

            if (dd1.SelectedItem.Text.Trim() == "---Select---")
            {

            }
            else
            {
                if (hfhcid.Value == "25560B") mhd = fgen.seek_iname(frm_qstr, co_cd, "Select name from typegrp where id='RT' and branchcd='" + mbr + "' and trim(upper(name))='" + txtname.Text.Trim().ToUpper() + "'", "name");
                else mhd = fgen.seek_iname(frm_qstr, co_cd, "Select name from typegrp where id='SE' and branchcd='" + mbr + "' and trim(upper(name))='" + txtname.Text.Trim().ToUpper() + "'", "name");
                if (edmode.Value == "Y")
                    fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                else if (mhd == "0" && edmode.Value.Trim().Length == 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                else fgen.msg("-", "AMSG", "Person Name is Already Available.");
            }
        }
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Del";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        if (hfhcid.Value == "25560B") SQuery = "select distinct type1 as Product_code,name as Product_Name,acref as Rate from typegrp where branchcd='" + mbr + "' and id='RT' order by type1 desc";
        else if (hfhcid.Value == "25560A") SQuery = "select distinct type1 as Person_code,name as Person_name,acref as Mobile_No from typegrp where branchcd='" + mbr + "' and id='SE' order by type1 desc";
        else SQuery = "select distinct type1 as Person_code,name as Person_name,acref as email_id from typegrp where branchcd='" + mbr + "' and id='SE' order by type1 desc";
        
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel(frm_qstr, "List of Persons");
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ")
        { Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr); }
        else
        {
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            clearctrl();
            enablectrl();
            dd1.DataSource = null; dd1.DataTextField = ""; dd1.DataValueField = ""; dd1.DataBind();
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            if (col1 == "Y")
            {
                if (hfhcid.Value == "25560B")
                {
                    fgen.execute_cmd(frm_qstr, co_cd, "delete from typegrp where branchcd||id||trim(type1)='" + edmode.Value + "'");
                }
                else
                {
                    fgen.execute_cmd(frm_qstr, co_cd, "delete from typegrp where branchcd||id||trim(type1)='" + edmode.Value + "'");
                }
                fgen.msg("-", "AMSG", "Details are deleted for " + edmode.Value.Substring(4, 3) + "");
                clearctrl(); fgen.ResetForm(this.Controls);
                enablectrl();
            }
            else
            { }
        }
        else
        {
            //if (Request.Cookies["Value1"].Value != null || Request.Cookies["Value2"].Value != null || Request.Cookies["Value3"].Value != null)
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
                col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

                switch (btnval)
                {
                    case "Edit":
                        clearctrl();
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, "select * from typegrp where branchcd||id||trim(type1)='" + col1 + "'");
                        fgen.EnableForm(this.Controls); disablectrl();
                        txtvchnum.Text = dt.Rows[0]["type1"].ToString().Trim();
                        txtname.Text = dt.Rows[0]["name"].ToString().Trim(); txtemailid.Text = dt.Rows[0]["acref"].ToString();
                        fill_dd();
                        if (dt.Rows[0]["acref4"].ToString().Trim().Length > 0)
                        { dd1.SelectedItem.Text = dt.Rows[0]["acref3"].ToString(); dd1.SelectedItem.Value = dt.Rows[0]["acref4"].ToString(); }
                        edmode.Value = "Y";
                        break;
                    case "Del":
                        clearctrl();
                        edmode.Value = col1.Trim();
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete ID: " + col1.Substring(4, 3) + "");
                        hffield.Value = "D";
                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = "";
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 != "Y")
        { }
        else
        {
            if (edmode.Value == "Y")
                if (hfhcid.Value == "25560B") fgen.execute_cmd(frm_qstr, co_cd, "update typegrp set branchcd='DD' where branchcd='" + mbr + "' and id='RT' and trim(type1)='" + txtvchnum.Text.Trim() + "'");

                else fgen.execute_cmd(frm_qstr, co_cd, "update typegrp set branchcd='DD' where branchcd='" + mbr + "' and id='SE' and trim(type1)='" + txtvchnum.Text.Trim() + "'");

            DataSet oDS = new DataSet();
            oDS = fgen.fill_schema(frm_qstr, co_cd, "TYPEGRP");
            DataTable pTable = oDS.Tables[0];
            if (edmode.Value == "Y")
                vchnum = txtvchnum.Text;
            else
            {
                if (hfhcid.Value == "25560B") vchnum = fgen.next_no(frm_qstr, co_cd, "select substr(max(type1),2,3) as vch from typegrp where id='RT' and branchcd='" + mbr + "'", 3, "vch");
                else vchnum = "E" + fgen.next_no(frm_qstr, co_cd, "select substr(max(type1),2,3) as vch from typegrp where id='SE' and branchcd='" + mbr + "'", 3, "vch");

            }


            oporow = oDS.Tables[0].NewRow();
            oporow["branchcd"] = mbr;
            if (hfhcid.Value == "25560B") oporow["id"] = "RT";
            else oporow["id"] = "SE";
            oporow["type1"] = vchnum;
            oporow["name"] = txtname.Text.Trim().ToUpper();
            oporow["acref"] = txtemailid.Text.Trim().ToUpper();
            oporow["acref3"] = dd1.SelectedItem;
            oporow["acref4"] = dd1.SelectedValue.Trim();
            oDS.Tables[0].Rows.Add(oporow);

            fgen.save_data(frm_qstr, co_cd, oDS, "TYPEGRP");

            if (hfhcid.Value == "25560B") fgen.execute_cmd(frm_qstr, co_cd, "delete from typegrp where branchcd='DD' and id='RT' and trim(type1)='" + txtvchnum.Text.Trim() + "'");
            else fgen.execute_cmd(frm_qstr, co_cd, "delete from typegrp where branchcd='DD' and id='SE' and trim(type1)='" + txtvchnum.Text.Trim() + "'");

            if (edmode.Value == "Y") { fgen.msg("-", "AMSG", "Data Updated Successfully"); fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); }
            else { fgen.msg("-", "AMSG", "Data Saved Successfully"); fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); }
        }
    }
    public void disp_data()
    {
        btnval = hffield.Value.Trim();
        switch (btnval)
        {
            default:
                if (btnval == "Del" || btnval == "Edit")
                {
                    if (hfhcid.Value == "25560B") SQuery = "select distinct branchcd||id||trim(type1) as fstr,type1 as Product_code,name as Product_Name,acref as Rate from typegrp where branchcd='" + mbr + "' and id='RT' order by type1 desc";
                    else if (hfhcid.Value == "25560A") SQuery = "select distinct branchcd||id||trim(type1) as fstr,type1 as Person_code,name as Person_name,acref as Mobile_No from typegrp where branchcd='" + mbr + "' and id='SE' order by type1 desc";
                    else SQuery = "select distinct branchcd||id||trim(type1) as fstr,type1 as Person_code,name as Person_name,acref as email_id from typegrp where branchcd='" + mbr + "' and id='SE' order by type1 desc";
                }
                break;
        }
        if (SQuery == null) { }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
}