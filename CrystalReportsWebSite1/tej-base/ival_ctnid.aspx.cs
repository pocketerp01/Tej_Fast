using System;
using System.Web;
using System.Web.UI;


public partial class ival_ctnid : System.Web.UI.Page
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
                if (!Page.IsPostBack)
                {
                    {
                        tk1.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").ToString().Trim();
                        tk2.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").ToString().Trim();
                        tk3.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL3").ToString().Trim();
                        tk4.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4").ToString().Trim();
                        tk5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL5").ToString().Trim();
                        tk6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL6").ToString().Trim();
                        tk7.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL7").ToString().Trim();
                        tk8.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL8").ToString().Trim();
                        tk9.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL9").ToString().Trim();
                    }
                }
                tk1.Attributes.Add("autocomplete", "off");
                tk2.Attributes.Add("autocomplete", "off");
                tk3.Attributes.Add("autocomplete", "off");
                tk4.Attributes.Add("autocomplete", "off");
                tk5.Attributes.Add("autocomplete", "off");
                tk6.Attributes.Add("autocomplete", "off");
                tk7.Attributes.Add("autocomplete", "off");
                tk8.Attributes.Add("autocomplete", "off");
                tk9.Attributes.Add("autocomplete", "off");
                tk1.Focus();
            }
        }
    }
    protected void btnok_ServerClick(object sender, EventArgs e)
    {
        fgen.send_cookie("REPLY", tk1.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL1", tk1.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL2", tk2.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL3", tk3.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL4", tk4.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL5", tk5.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL6", tk6.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL7", tk7.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL8", tk8.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL9", tk9.Text.Trim());
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
    }
    protected void btnNo_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclosePopup();", true);
    }
}