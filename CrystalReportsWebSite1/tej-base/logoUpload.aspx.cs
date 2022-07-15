using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class logoUpload : System.Web.UI.Page
{
    string co_cd;
    string uname, mbr, vardate, fromdt, todt, DateRange, year, ulvl, frm_qstr, frm_url, frm_formID, frm_UserID;
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
        else
        {
            if (Request.Cookies["COCD"] != null)
                txtConfirmOtp.Value = Request.Cookies["COCD"].Value.Trim();
            txtConfirmOtp.Attributes.Add("onkeypress", "return clickEnter('" + btnOk.ClientID + "', event)");
            txtConfirmOtp.Focus();
        }
    }

    protected void btnchngpwd_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "0");
        string btnval = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BTNNAME");
        if (txtConfirmOtp.Value == null || txtConfirmOtp.Value == "") { lblerr.Text = "Please Enter Company Code!!"; txtConfirmOtp.Focus(); }
        else
        {
            if (fupl.HasFile)
            {
                if (!Directory.Exists(@"c:\TEJ_ERP")) Directory.CreateDirectory(@"c:\TEJ_ERP");
                if (!Directory.Exists(@"c:\TEJ_ERP\logo")) Directory.CreateDirectory(@"c:\TEJ_ERP\logo");                

                fupl.PostedFile.SaveAs(@"c:\TEJ_ERP\logo\mlogo_" + txtConfirmOtp.Value + ".jpg");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
            }
            else
            {
                lblerr.Text = "Please Upload Logo File!!";
            }
        }
    }
    protected void btnext_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
}
