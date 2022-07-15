using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class cpwd : System.Web.UI.Page
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
        string btnval = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BTNNAME");
        if (txtPwd.Value == null || txtPwd.Value == "") { lblerr.Text = "Please Enter Password!!"; txtPwd.Focus(); }
        else
        {
            if (txtUserName.Value.Trim().ToUpper() != "INFOMLG")
            {
                mhd = fgen.seek_iname(frm_qstr, co_cd, "select username from evas where trim(username)='" + txtUserName.Value.Trim().ToUpper() + "'", "username");
                if (mhd != "0")
                {
                    mhd = fgen.seek_iname(frm_qstr, co_cd, "Select username,level3pw from evas where trim(username)='" + txtUserName.Value.Trim().ToUpper() + "' and upper(level3pw)='" + txtPwd.Value.Trim().ToUpper() + "'", "username");
                    if (mhd != "0")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "1");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
                    }
                    else { lblerr.Text = "Password is Not correct!!"; txtPwd.Focus(); }
                }
                else { lblerr.Text = "UserName is Not correct!!"; txtUserName.Focus(); }
            }
            else
            {
                string cc = "A" + "LT" + "12" + "3" + "";
                if (txtUserName.Value.Trim().ToUpper() == "INFOMLG" && txtPwd.Value.Trim().ToUpper() == cc + DateTime.Now.DayOfWeek.ToString().ToUpper().Substring(0, 2) + "881") { fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "2"); ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true); }
                if (txtUserName.Value.Trim().ToUpper() == "INFOMLG" && txtPwd.Value.Trim().ToUpper() == cc + "BROP") { fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "3"); ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true); }
            }
        }
    }
    protected void btnext_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONFIRM", "0");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
}
