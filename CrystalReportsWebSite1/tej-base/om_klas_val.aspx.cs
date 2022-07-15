using System;
using System.Web;
using System.Web.UI;


public partial class om_klas_val : System.Web.UI.Page
{
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        tk1.Focus();
        tk1.Attributes.Add("onkeypress", "return clickEnter_text('" + tk2.ClientID + "', event)");
        tk2.Attributes.Add("onkeypress", "return clickEnter_text('" + tk3.ClientID + "', event)");
        tk3.Attributes.Add("onkeypress", "return clickEnter_text('" + tk4.ClientID + "', event)");
        tk4.Attributes.Add("onkeypress", "return clickEnter_text('" + tk5.ClientID + "', event)");
        tk5.Attributes.Add("onkeypress", "return clickEnter_text('" + btnok.ClientID + "', event)");
    }
    protected void btnok_ServerClick(object sender, EventArgs e)
    {
        if (tk1.Text.Length > 0 && tk2.Text.Length > 0 && tk3.Text.Length > 0 && tk4.Text.Length > 0 && tk5.Text.Length > 0)
        {
            fgen.send_cookie("Value1", tk1.Text.Trim() + "," + tk2.Text.Trim() + "," + tk3.Text.Trim() + "," + tk4.Text.Trim() + "," + tk5.Text.Trim());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "APopUP", "OpenSingle('om_Web_Rpt_KLAS.aspx');", true);
        }
        else
            lblerr.Text = "Please Fill All TextBox's";
    }
}
