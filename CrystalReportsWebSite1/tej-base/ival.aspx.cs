using System;
using System.Web;
using System.Web.UI;


public partial class ival : System.Web.UI.Page
{

    string HCID, frm_url, frm_qstr, frm_formID;
    fgenLG fgen = new fgenLG();

    protected void Page_Load(object sender, EventArgs e)
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
                HCID = frm_formID;
                if (HCID == null)
                {
                    HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                }
                if (HCID == "22211")
                {
                    tdcaption.InnerText = "Enter Query to make DBF";
                    rd1.Items[0].Text = "From File";
                    rd1.Items[1].Text = "Using Query";
                    tk1.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
                }
                if (HCID == "F40122") tk1.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
                valper.Visible = false;
                tk1.Focus();
            }
        }
    }
    protected void btnok_ServerClick(object sender, EventArgs e)
    {
        if (tk1.Text.Length > 0)
        {
            if (HCID == "22211")
            {
                int i = rd1.SelectedIndex;
                fgen.send_cookie("REPLY", tk1.Text.Trim());
            }
            else
            {
                fgen.send_cookie("REPLY", rd1.SelectedValue.Trim() + "," + tk1.Text.Trim());

            }
            fgen.send_cookie("REPLY", tk1.Text.Trim());
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", tk1.Text.Trim());
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID") == "FINSYS_S") ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
            else ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
        }
        else lblerr.Text = "Please Fill some Value in TextBox";


    }
    protected void btnExit_ServerClick(object sender, EventArgs e)
    {
       ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
}