using System;
using System.Web;
using System.Web.UI;
using System.Data;


public partial class prdemp2 : System.Web.UI.Page
{
    string year, co_cd, Today1, YR_SL, frm_url, frm_qstr,frm_formID;
    int mhd;
    string frm_cDt1, frm_cDt2;
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

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
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
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            if (!Page.IsPostBack)
            {
                txtfromdt.Text = Convert.ToDateTime(frm_cDt1).ToString("dd/MM/yyyy");
                txttodt.Text = Convert.ToDateTime(Today1).ToString("dd/MM/yyyy");
                txtfromdt.Focus();
            }
            txtfromdt.Attributes.Add("onkeypress", "return clickEnter('" + txttodt.ClientID + "', event)");
            txttodt.Attributes.Add("onkeypress", "return clickEnter('" + btnsubmit.ClientID + "', event)");
            checkYearDate();
        }
    }
    void checkYearDate()
    {
        string cdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
        string cdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
        if (Convert.ToDateTime(cdt2) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")))
        {
            txtfromdt.Text = Convert.ToDateTime(cdt1.ToString()).ToString("dd/MM/yyyy");
            txttodt.Text = Convert.ToDateTime(cdt2.ToString()).ToString("dd/MM/yyyy");
        }
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList2.ClearSelection();
        string cdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
        string cdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

        if (Convert.ToDateTime(cdt2) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")))
        {
            checkYearDate();
            return;
        }

        if (RadioButtonList1.SelectedIndex == 0)
        {
            //Y.T.D            
            txtfromdt.Text = Convert.ToDateTime(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1")).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 1)
        {
            //M.T.D
            txtfromdt.Text = Convert.ToDateTime("01/" + Today1.Substring(3, 3).ToString().Trim() + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 2)
        {
            //Pr.Mnth                        
            txtfromdt.Text = Convert.ToDateTime("01" + "/" + DateTime.Now.ToString("MM") + "/" + YR_SL).AddMonths(-1).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(txtfromdt.Text).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 3)
        {
            //Next.Mnth       
            txtfromdt.Text = Convert.ToDateTime("01" + "/" + DateTime.Now.ToString("MM") + "/" + YR_SL).AddMonths(1).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(txtfromdt.Text).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 4)
        {
            //Yestrdy
            txtfromdt.Text = Convert.ToDateTime(Today1).AddDays(-1).ToString("yyyy-MM-dd");
            txttodt.Text = txtfromdt.Text;
        }
        else if (RadioButtonList1.SelectedIndex == 5)
        {
            //Today
            txtfromdt.Text = Convert.ToDateTime(Today1.ToString()).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(Today1.ToString()).ToString("yyyy-MM-dd");
        }
    }
    protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList1.ClearSelection();
        YR_SL = fgenMV.Fn_Get_Mvar(frm_qstr, "U_year");
        string cdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

        if (RadioButtonList2.SelectedIndex == 0)
        {
            //curr.mnth 
            if (Convert.ToDateTime(cdt2) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")))
            {
                checkYearDate();
                return;
            }
            txtfromdt.Text = Convert.ToDateTime("01/" + DateTime.Now.ToString("MM/yyyy")).ToString("yyyy-MM-dd");
            string lastd = fgen.seek_iname(frm_qstr, co_cd, "SELECT LAST_DAY(SYSDATE) AS lastd FROM DUAL", "lastd");
            txttodt.Text = Convert.ToDateTime(lastd).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 1)
        {
            //FirstQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/04/" + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("30/06/" + YR_SL).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 2)
        {
            //SecQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/07/" + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("30/09/" + YR_SL).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 3)
        {
            //ThirdQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/10/" + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("31/12/" + YR_SL).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 4)
        {
            //FourthQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/01/" + Convert.ToString(Convert.ToDecimal(YR_SL) + 1).Trim()).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("31/03/" + Convert.ToString(Convert.ToDecimal(YR_SL) + 1).Trim()).ToString("yyyy-MM-dd");
        }
    }

    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        mhd = fgen.ChkDate(txtfromdt.Text.Trim());
        if (mhd == 0) lblerr.Text = "Not a Valid date in From Date";
        else
        {
            mhd = fgen.ChkDate(txttodt.Text.Trim());
            if (mhd == 0) lblerr.Text = "Not a Valid date in To Date";
            else
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT1", Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy"));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT2", Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy"));
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_DAYRANGE", " between to_date('01/" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");

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