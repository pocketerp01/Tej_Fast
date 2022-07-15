using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections.Generic;


public partial class chrtlevel1 : System.Web.UI.Page
{
    string Squery, newways, co_cd, newways2, newways3, xsname1, xsname2; DataTable dt;
    string mq0, mq1, mq2, val;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    Int64 way, way2, way3;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
        frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
        if (frm_url.Contains("STR"))
        {
            if (Request.QueryString["STR"].Length > 0)
            {
                frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
                if (frm_qstr.Contains("@"))
                {
                    frm_formID = frm_qstr.Split('@')[1].ToString();
                    val = frm_formID;
                    frm_qstr = frm_qstr.Split('@')[0].ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                }
                frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");

                fillCharts();
            }
            else Response.Redirect("~/login.aspx");
        }
    }
    void fillCharts()
    {
        string chartScript = fgenMV.Fn_Get_Mvar(frm_qstr, "GraphData");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", chartScript, false);
    }
}