using System;
using System.Data;
using System.Web;


public partial class dpt_mst : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, uname, col1, col2, col3, mbr, cstr, vchnum, vardate, fromdt, todt, DateRange, year, ulvl, mhd;
    DataTable dt; DataRow oporow;
    string vty, frm_url, frm_qstr, frm_formID;
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
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        fgen.EnableForm(this.Controls); disablectrl();
        lbldptcode.Text = fgen.next_no(frm_qstr, co_cd, "select max(type1) as vch from typegrp where id='DP' and branchcd='" + mbr + "'", 6, "vch");
        txtdptname.Focus();
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
        if (txtdptname.Text.Trim().Length <= 0 || txtdptname.Text.Trim() == null)
            fgen.msg("-", "AMSG", "Please Fill Department Name");
        else
        {
            mhd = fgen.seek_iname(frm_qstr, co_cd, "Select name from typegrp where id='DP' and branchcd='" + mbr + "' and trim(upper(name))='" + txtdptname.Text.Trim().ToUpper() + "'", "name");
            if (edmode.Value == "Y")
                fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
            else if (mhd == "0" && edmode.Value.Trim().Length == 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
            else fgen.msg("-", "AMSG", "Department Name is Already Available.");
        }
        txtdptname.Focus();
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
        SQuery = "select distinct type1 as Dept_code,name as dept_name from typegrp where branchcd='" + mbr + "' and id='DP' order by type1 desc";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

        fgen.Fn_open_rptlevel("List of Department", frm_qstr);
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
                fgen.execute_cmd(frm_qstr, co_cd, "delete from typegrp where branchcd||id||trim(type1)='" + edmode.Value + "'");
                fgen.msg("-", "AMSG", "Details are deleted for " + edmode.Value.Substring(4, 6) + "");
                clearctrl(); fgen.ResetForm(this.Controls);
                disablectrl();
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
                        lbldptcode.Text = dt.Rows[0]["type1"].ToString().Trim();
                        txtdptname.Text = dt.Rows[0]["name"].ToString().Trim();
                        edmode.Value = "Y";
                        break;
                    case "Del":
                        clearctrl();
                        edmode.Value = col1.Trim();
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete " + col1.Substring(4, 6) + "");
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
                fgen.execute_cmd(frm_qstr, co_cd, "update typegrp set branchcd='DD' where branchcd='" + mbr + "' and id='DP' and trim(type1)='" + lbldptcode.Text.Trim() + "'");
            
            DataSet oDS = new DataSet();
            oDS = fgen.fill_schema(frm_qstr, co_cd, "TYPEGRP");
            
            if (edmode.Value == "Y")
                vchnum = lbldptcode.Text;
            else
                vchnum = fgen.next_no(frm_qstr, co_cd, "select max(type1) as vch from typegrp where id='DP' and branchcd='" + mbr + "'", 6, "vch");

            oporow = oDS.Tables[0].NewRow();
            oporow["branchcd"] = mbr;
            oporow["id"] = "DP";
            oporow["type1"] = vchnum;
            oporow["name"] = txtdptname.Text.Trim().ToUpper();
            oDS.Tables[0].Rows.Add(oporow);
            fgen.save_data(frm_qstr, co_cd, oDS, "TYPEGRP");

            fgen.execute_cmd(frm_qstr, co_cd, "delete from typegrp where branchcd='DD' and id='DP' and trim(type1)='" + lbldptcode.Text.Trim() + "'");

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
                    SQuery = "select distinct branchcd||id||trim(type1) as fstr,type1 as Dept_code,name as dept_name from typegrp where branchcd='" + mbr + "' and id='DP' order by type1 desc";
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
