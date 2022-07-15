using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class moreReports : System.Web.UI.Page
{
    string HCID, co_cd, uname, mbr, ulvl, year, xprdrange, cond;
    string tab_name, frm_qstr, frm_formID, mhd, sQuery;
    DataTable dt;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
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
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
            }
            if (!Page.IsPostBack) show_data();
        }
    }
    public void show_data()
    {
        co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
        tab_name = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB");
        cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONCOND");
        mhd = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAM FROM " + tab_name + " WHERE ID='" + frm_formID + "'", "PARAM");
        if (cond.Length > 2) cond = "and " + cond + " and PARAM='" + mhd + "'";
        else cond = " and PARAM='" + mhd + "'";

        sQuery = "select distinct trim(id) as fstr,web_Action as Web,trim(text) as Text,'-' as description,trim(id) as T_code from " + tab_name + " where nvl(web_Action,'-')!='-' " + cond + " and MLEVEL>3 order by trim(id),trim(text)";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "MREPORTS");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
        cond = "../tej-base/open_icon.aspx";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + cond + "?STR=" + frm_qstr + "','1000px','450px','Finsys Quick Menu');", true);
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        string val1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
        if (val1.Length > 2) { }
        else return;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", val1);
        Response.Redirect(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + frm_qstr + "@" + val1, false);
    }
}