using System;
using System.Web;
using System.Web.UI;


public partial class ival_PROD : System.Web.UI.Page
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
                    string Filled = (string)Session["Filled"];
                    if (Filled == "Y")
                    {
                        tk1.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").ToString().Trim();
                        tk2.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").ToString().Trim();
                        tk3.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL3").ToString().Trim();                        
                    }
                }
                tk1.Attributes.Add("autocomplete", "off");
                tk2.Attributes.Add("autocomplete", "off");
                tk3.Attributes.Add("autocomplete", "off");                
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
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
    }
}