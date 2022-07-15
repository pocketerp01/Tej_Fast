using System;
using System.Web;
using System.Web.UI;
using System.Data;


public partial class om_dtbox : System.Web.UI.Page
{
    string year, co_cd, Today1, YR_SL, frm_url, frm_qstr;
    int mhd;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
        else
        {
            //-----------------
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    YR_SL = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                }
            }
            //--------------------------            
            lblerr.Text = "";            
            Today1 = DateTime.Now.ToString("dd/MM/");            
            
            if (Convert.ToInt32(DateTime.Now.ToString("MM")) >= 1 && Convert.ToInt32(DateTime.Now.ToString("MM")) < 4)
            {
                Today1 = Today1 + Convert.ToString(Convert.ToDouble(YR_SL) + 1);
                YR_SL = Convert.ToString(Convert.ToDouble(YR_SL) + 1);
            }
            else
            {
                Today1 = Today1 + Convert.ToString(Convert.ToDouble(YR_SL));
                YR_SL = Convert.ToString(Convert.ToDouble(YR_SL));
            }
            //Today1 = Today1 + YR_SL;

            if (!Page.IsPostBack)
            {
                txtfromdt.Text = DateTime.Now.ToString("yyyy-MM-dd");
                
                txtfromdt.Focus();
            }
            //txtfromdt.Attributes.Add("onkeypress", "return clickEnter('" + txttodt.ClientID + "', event)");
            
        }
    }


    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {        
        mhd = fgen.ChkDate(txtfromdt.Text.Trim());
        if (mhd == 0) lblerr.Text = "Not a Valid date in From Date";
        else
        {
            
            if (mhd == 0) lblerr.Text = "Not a Valid date in To Date";
            else
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT1", Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy"));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT2", Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy"));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");

                lblerr.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
            }
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
}