using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fin_sfiles_getLocation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnOK.Focus();
    }
    protected void btnOK_ServerClick(object sender, EventArgs e)
    {
        Session["lat"] = hfLat.Value;
        Session["long"] = hfLong.Value;
        Session["addr"] = hfAddr.Value;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
    }
}