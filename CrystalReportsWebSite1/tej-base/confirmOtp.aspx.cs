using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class confirmOtp : System.Web.UI.Page
{
    string co_cd;
    string uname, mbr, vardate, fromdt, todt, DateRange, year, ulvl, frm_qstr, frm_url, frm_formID, frm_UserID;
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

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                    }

                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(co_cd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }
            txtConfirmOtp.Attributes.Add("onkeypress", "return clickEnter('" + btnOk.ClientID + "', event)");
            txtConfirmOtp.Focus();
            if (lblerr.Text == "Please Enter OTP!!") lblerr.ForeColor = System.Drawing.Color.CadetBlue;
            else lblerr.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnchngpwd_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "0");
        string btnval = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BTNNAME");
        if (txtConfirmOtp.Value == null || txtConfirmOtp.Value == "") { lblerr.Text = "Please Enter OTP!!"; txtConfirmOtp.Focus(); }
        else
        {
            string _otp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OTP");
            if (_otp == txtConfirmOtp.Value.Trim())
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "1");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideFS');", true);
            }
            else
            {
                lblerr.Text = "Wrong OTP Entered!!";
                lblerr.ForeColor = System.Drawing.Color.Red;
                txtConfirmOtp.Focus();
            }
        }
    }
    protected void btnext_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "0");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
}
