using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class fin_base_helpInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        fgenDB fgen = new fgenDB();
        string Squery, co_cd, frm_url, frm_qstr = "", HCID, formid = "";
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
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
                }
            }
            //--------------------------                        
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            formid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            if (formid == "F10194" || formid == "F05125E")
            {
                F10194.Visible = true;
            }
            else
            {
                F10194.Visible = false;
            }
        }
    }
}