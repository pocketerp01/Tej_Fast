using System;
using System.Web;
using System.Web.UI;

using System.Web.UI.WebControls;

public partial class fin_base_Fin_Master2 : System.Web.UI.MasterPage
{
    string Cstr, str, co_cd = "", mbr = "", hname, br_name, btnval, yr, val1, tab_name, mhd, uname, cond, squery, col2, bval, get_qstr, url, usr_dept, frm_formID, frm_ulevel;
    protected void Page_Load(object sender, EventArgs e)
    {
        url = HttpContext.Current.Request.Url.AbsoluteUri;
        if (url.Contains("STR"))
        {
            if (Request.QueryString["STR"].Length > 0)
            {
                get_qstr = Request.QueryString["STR"].Trim().ToString();
                if (get_qstr.Contains("@"))
                {
                    frm_formID = get_qstr.Split('@')[1].ToString();
                    get_qstr = get_qstr.Split('@')[0].ToString();
                }

                co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");
                yr = fgenMV.Fn_Get_Mvar(get_qstr, "U_FYEAR");
                uname = fgenMV.Fn_Get_Mvar(get_qstr, "U_UNAME");
                cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");
                mbr = fgenMV.Fn_Get_Mvar(get_qstr, "U_MBR");
                br_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_MBR_NAME");
                usr_dept = fgenMV.Fn_Get_Mvar(get_qstr, "U_DEP_NAME");
                frm_ulevel = fgenMV.Fn_Get_Mvar(get_qstr, "U_ULEVEL");

                if (co_cd.Length <= 1) Response.Redirect("~/login.aspx");
            }
            else Response.Redirect("~/login.aspx");

            Page.Title = co_cd == "SRIS" ? "SRISOL ERP" : "Tejaxo";

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (hfWindowSize.Value.ToString() == "") hfWindowSize.Value = fgenMV.Fn_Get_Mvar(get_qstr, "FRMWINDOWSIZE");
            fgenMV.Fn_Set_Mvar(get_qstr, "FRMWINDOWSIZE", hfWindowSize.Value.ToString());
            Session["hfWindowSize"] = hfWindowSize.Value;
            if (!Page.IsPostBack)
            {
                fill_val();
            }
        }
    }
    public void fill_val()
    {
        //        lblbrcode.Text = mbr.Trim();
        lblbrcode1.Text = mbr.Trim();

        lblbrname.Text = br_name;
        lblusername.Text = uname;

        hname = fgenCO.chk_co(co_cd);
        lblbuilddt.Text = fgenMV.Fn_Get_Mvar(get_qstr, "U_EXETIME");
        lblHelpLine.InnerText = fgenMV.Fn_Get_Mvar(get_qstr, "U_HELPLINE");
        helpLin.Visible = false;
        if (fgenMV.Fn_Get_Mvar(get_qstr, "U_CLIENT_GRP") == "SG_TYPE")
            helpLin.Visible = true;
        lblserverIP.Text = fgenMV.Fn_Get_Mvar(get_qstr, "U_SERVERIP");
        if (lblbuilddt.Text == "0") lblbuilddt.Visible = false;
        if (hname.Substring(0, 4) == "AKIT") lblbcode.Text = ": ";
        else lblbcode.Text = "(" + co_cd.Trim() + ")";
        lblBrHeader.Text = hname;
        //lblyr.Text = yr;

        if (fgenMV.Fn_Get_Mvar(get_qstr, "U_CDT1").Substring(6, 4) == fgenMV.Fn_Get_Mvar(get_qstr, "U_CDT2").Substring(6, 4))
            lblyearS.Text = "(" + yr.Split('-')[0] + ")";
        else lblyearS.Text = "(" + yr + ")";
        //lblbr.Text = mbr;

        string logopath = "~/tej-base/images/finsysblue.jpg";
        if (co_cd == "SRIS")
        {
            logopath = "~/tej-base/images/sris_desktop.jpg";
        }
        imglogo.Src = logopath;

        string userDP = "~/tej-base/upload/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_DP_IMG");
        if (System.IO.File.Exists(MapPath(userDP)))
        {
            imgprofile.ImageUrl = userDP;            
        }
    }
}
