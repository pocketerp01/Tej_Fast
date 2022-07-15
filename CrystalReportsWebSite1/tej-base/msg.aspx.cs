using System;
using System.Web;
using System.Web.UI;


public partial class msg : System.Web.UI.Page
{
    string M_ID, btnval;
    fgenLG fgen = new fgenLG();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            show();
    }
    public void show()
    {
        if (Request.Cookies["send_msg"] != null)
        {
            M_ID = Request.Cookies["mid"].Value.ToString();

            if (M_ID == "CMSG" || M_ID == "SMSG" || M_ID == "FMSG" || M_ID == "ISMSG" || M_ID == "ICMSG") { trconf.Visible = true; tralert.Visible = false; btnyes.Focus(); btn3.Visible = false; }
            else if (M_ID == "PMSG") { trconf.Visible = true; tralert.Visible = false; btnyes.Focus(); btn3.Visible = true; btnyes.InnerText = "1"; btnno.InnerText = "2"; }
            else { trconf.Visible = false; tralert.Visible = true; btnok.Focus(); }

            string m = HttpContext.Current.Server.UrlDecode(Request.Cookies["send_msg"].Value.ToString());
            lblmsg.Text = m.Replace("'13'", "<br />");
        }
    }
    public void clr_key()
    { hfval.Value = ""; btnval = ""; lblmsg.Text = ""; }

    public void key_val()
    {
        btnval = hfval.Value; M_ID = Request.Cookies["mid"].Value.ToString();
        if (btnval == null || btnval == "") { }
        else
        {
            fgen.send_cookie("REPLY", "" + btnval + "");
            switch (M_ID)
            {
                case "CMSG":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
                    break;
                case "PMSG":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
                    break;
                case "SMSG":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
                    break;
                case "AMSG":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "OnlyClose();", true);
                    break;
                case "FMSG":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup3();", true);
                    break;
                case "ISMSG":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup4();", true);
                    break;
                case "ICMSG":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup5();", true);
                    break;
            }
        }
    }
    protected void btnyes_ServerClick(object sender, EventArgs e)
    { clr_key(); hfval.Value = "Y"; key_val(); }
    protected void btnno_ServerClick(object sender, EventArgs e)
    { clr_key(); hfval.Value = "N"; key_val(); }
    protected void btn3_ServerClick(object sender, EventArgs e)
    { clr_key(); hfval.Value = "A"; key_val(); }
    protected void btnok_ServerClick(object sender, EventArgs e)
    { clr_key(); ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "OnlyClose();", true); }
}