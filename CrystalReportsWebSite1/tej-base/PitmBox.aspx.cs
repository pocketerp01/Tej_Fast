using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


public partial class PitmBox : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "-", Value2 = "-", Value3 = "-", Value4 = "-", Value5 = "-", Value6 = "-", Value7 = "-", Value8 = "-", Value9 = "-", Value10 = "-";
    string HCID, co_cd; int col_count = 0;
    string frm_qstr, frm_url, frm_cocd, frm_mbr, frm_formID;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
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
                    if (frm_qstr.Contains("@"))
                    {
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                        frm_formID = frm_qstr.Split('@')[0].ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                    }
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                }
            }
            //--------------------------            
            co_cd = frm_cocd;

            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");

            btnAcode.Focus();
        }
    }
    void makequery4popup()
    {
        string squery = "";
        string cond = " like '16%'";

        switch (hffield.Value)
        {
            case "ACODE":
                cond = " like '06%'";
                if (frm_formID == "F05109") cond = " like '%'";
                squery = "SELECT ACODE,ANAME AS PARTY,ACODE AS CODE,ADDR1,EMAIL FROM FAMST WHERE ACODE " + cond + " ORDER BY ACODE";
                break;
            case "ICODE":
                cond = " not like '9%'";
                if (frm_formID == "F05109") cond = " like '%'";
                squery = "SELECT ICODE,INAME AS PRODUCT,ICODE AS CODE,CPARTNO,UNIT FROM ITEM WHERE LENGTH(TRIM(ICODE))>4 and TRIM(ICODE) " + cond + " ORDER BY ICODE";
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOX");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btniBox_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "ACODE":
                txtAcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                txtAname.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnIcode.Focus();
                else btnAcode.Focus();
                break;
            case "ICODE":
                txtIcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                txtIname.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnsubmit.Focus();
                else btnIcode.Focus();
                break;
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR1", txtAcode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR2", txtIcode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR3", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR4", "");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup1();", true);
    }
    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODE";
        makequery4popup();
    }
    protected void btnIcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ICODE";
        makequery4popup();
    }
}