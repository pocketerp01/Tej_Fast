using System;
using System.Web;
using System.Web.Security;


public partial class signout : System.Web.UI.Page
{
    fgenLG fgen = new fgenLG();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string get_qstr = Session["LOGOUT"].ToString();
            //fgen.del_file(Server.MapPath("~/Log_File/" + get_qstr + ".txt"));
        }
        catch { }

        HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
        HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
        HttpContext.Current.Response.AddHeader("Expires", "0");

        //FormsAuthentication.SignOut();
        fgen.kill_cookie();
        //Session.Clear();
        //Session.Abandon();
        //Session.RemoveAll();
        Oracle.ManagedDataAccess.Client.OracleConnection.ClearAllPools();
        Response.Redirect("~/login.aspx");
    }
}