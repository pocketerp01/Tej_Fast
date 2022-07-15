using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class activPwd : System.Web.UI.Page
{
    string mhd, co_cd;
    string btnval, SQuery, uname, col1, col2, col3, mbr, vchnum, vardate, fromdt, todt, DateRange, filepath = "", year, ulvl, merr = "0", frm_qstr, frm_url, frm_formID, frm_UserID;
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
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
                    }
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack) txtUserName.Value = uname;
            txtUserName.Attributes.Add("onkeypress", "return clickEnter('" + txtPwd.ClientID + "', event)");
            //txtPwd.Attributes.Add("onkeypress", "return clickEnter('" + btnOk.ClientID + "', event)");
            txtUserName.Focus();
            txtPwd.Attributes.Add("type", "password");
        }
    }

    protected void btnchngpwd_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "0");
        if (txtPwd.Value == null || txtPwd.Value == "") { lblerr.Text = "Please Enter Password!!"; txtPwd.Focus(); }
        else
        {
            string cc = "A" + "LT" + "12" + "3" + "";
            if (txtUserName.Value.Trim().ToUpper() == "INFOMLG" && txtPwd.Value.Trim().ToUpper() == cc + "SACT881")
            {
                fgen.AllowPC();
                
                string mail_title = "Tejaxo ERP";
                System.Text.StringBuilder msb = new System.Text.StringBuilder();
                msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                msb.Append("Dear Sir,<br/><br/>");
                msb.Append("tej-wfin installed in [" + co_cd + "] " + fgenCO.chk_co(co_cd) + " <br/><br/>");
                msb.Append("Thanks & Regards,<br>");
                msb.Append("</body></html>");

                //Sending E-mail
                fgen.send_mail("FINS", mail_title, "info@pocketdriver.in", "info@pocketdriver.in", "", "tej-Wfin Activated for Client " + co_cd, msb.ToString());
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "2"); ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
            }
        }
    }
    protected void btnext_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "0");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
}
