using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class cpwd4 : System.Web.UI.Page
{
    string mhd, co_cd;
    string btnval, SQuery, uname, col1, col2, col3, mbr, vchnum, vardate, fromdt, todt, DateRange, filepath = "", year, ulvl, merr = "0", frm_qstr, frm_url, frm_formID, frm_UserID;
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
        else
        {
            txtoldpwd.Focus();
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
            lblusername.Text = uname;
            txtoldpwd.Attributes.Add("onkeypress", "return clickEnter('" + txtnewpwd.ClientID + "', event)");
            txtnewpwd.Attributes.Add("onkeypress", "return clickEnter('" + txtconfpwd.ClientID + "', event)");
            txtconfpwd.Attributes.Add("onkeypress", "return clickEnter('" + btnchngpwd.ClientID + "', event)");
        }
    }

    protected void btnchngpwd_ServerClick(object sender, EventArgs e)
    {
        if (txtoldpwd.Text == null || txtoldpwd.Text == "")
        { lblerr.Text = "Please Enter Old Password!!"; txtoldpwd.Focus(); }
        else
        {
            mhd = fgen.seek_iname(frm_qstr, co_cd, "select level3pw from evas where trim(upper(username))='" + lblusername.Text.Trim() + "'", "level3pw");
            if ((co_cd == "LIVN" || co_cd == "JSGI") && mhd == "0")
                mhd = fgen.seek_iname(frm_qstr, co_cd, "select weblogin from famst where trim(upper(acode))='" + lblusername.Text.Trim() + "'", "weblogin");
            //if (mhd == EncryptDecrypt.Encrypt(txtoldpwd.Text.Trim().ToUpper()) || mhd == "ABCD")
            if (mhd == txtoldpwd.Text.Trim().ToUpper())
            {
                if (txtnewpwd.Text != "")
                {
                    if (txtconfpwd.Text.Trim().ToUpper() == txtnewpwd.Text.Trim().ToUpper())
                    {
                        if (txtnewpwd.Text.Trim().ToUpper() != "ABCD")
                        {
                            if (pwd1.Value == "WRONG")
                            {
                                lblerr.Text = "Password is not correct";
                                txtnewpwd.Focus();
                                return;
                            }

                            //********** Login Security
                            #region Login Security
                            if (co_cd == "MSES")
                            {
                                mhd = fgen.seek_iname(frm_qstr, co_cd, "select to_char(ent_dt,'DD/MM/YYYY') as ent_dt from log_track where trim(ent_by)='" + lblusername.Text.Trim() + "' and ent_dt between to_date(to_char(sysdate,'dd/MM/yyyy'),'dd/MM/yyyy')-365 and to_date(to_char(sysdate,'dd/MM/yyyy'),'dd/MM/yyyy')+1 and (upper(trim(opass))='" + txtnewpwd.Text.Trim().ToUpper() + "' OR upper(trim(npass))='" + txtnewpwd.Text.Trim().ToUpper() + "')", "ent_dt");
                                if (mhd != "0")
                                {
                                    lblerr.Text = "You have already used such password in the last 1 year, Please enter different password.";
                                    txtoldpwd.Focus();
                                    return;
                                }
                            }
                            #endregion


                            mhd = fgen.seek_iname(frm_qstr, co_cd, "select level3pw from evas where trim(upper(username))='" + lblusername.Text.Trim() + "'", "level3pw");
                            if (mhd != "0") fgen.execute_cmd(frm_qstr, co_cd, "update evas set level3pw='" + txtnewpwd.Text.Trim().ToUpper() + "' where trim(upper(username))='" + lblusername.Text.Trim() + "'");
                            else if ((co_cd == "LIVN" || co_cd == "JSGI") && mhd == "0") fgen.execute_cmd(frm_qstr, co_cd, "update famst set weblogin='" + txtnewpwd.Text.Trim().ToUpper() + "' where trim(upper(acode))='" + lblusername.Text.Trim() + "'");

                            //Saving Change PWD Tracking
                            if (co_cd == "MSES")
                            {
                                //fgen.track_save(co_cd, frm_qstr, "CHANGE PASSWORD", "CP", lblusername.Text.Trim(), txtoldpwd.Text.Trim().ToUpper(), txtnewpwd.Text.Trim().ToUpper());
                            }

                            lblerr.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc1", "alert('Dear " + lblusername.Text.Trim() + ", Password has been Changed!!');", true);
                            btnext_ServerClick("", EventArgs.Empty);
                        }
                        else
                        { lblerr.Text = "Please choose another password"; txtnewpwd.Focus(); }
                    }
                    else
                    { lblerr.Text = "New Password Not Matched with Confirm Password"; txtnewpwd.Focus(); }
                }
                else
                {
                    lblerr.Text = "Please Enter New Password"; txtnewpwd.Focus();
                }
            }
            else
            { lblerr.Text = "Old Password Not Matched!!"; txtoldpwd.Focus(); }
        }
    }
    protected void btnext_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
}
