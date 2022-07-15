using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


public partial class RangeBox : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "-", Value2 = "-", Value3 = "-", Value4 = "-", Value5 = "-", Value6 = "-", Value7 = "-", Value8 = "-";
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
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID1");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");

            btnFrom.Focus();
            if (frm_cocd == "MLGI" || frm_cocd == "WING")
            {
                rdPDF.SelectedValue = "1";
            }
        }
    }
    void makequery4popup()
    {
        query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        if (hffield.Value != "BTNACODE")
        {
            if (hf1.Value.Trim().Length > 2)
            {
                query1 = hf1.Value.Trim();
            }
        }
        if (hffield.Value == "BTNACODE") query1 = "SELECT ACODE,ANAME AS PARTY,ACODE AS CODE FROM FAMST ORDER BY TRIM(ACODE)";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOX");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", query1);
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btniBox_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "BTNFROM":
                txtFromVch.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                txtFromVchdt.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");

                txtToVch.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                txtToVchdt.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnTo.Focus();
                else btnFrom.Focus();
                break;
            case "BTNTO":
                txtToVch.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                txtToVchdt.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnsubmit.Focus();
                else btnTo.Focus();
                break;
            case "BTNACODE":
                txtAcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                txtAname.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnFrom.Focus();
                else btnAcode.Focus();
                break;
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        #region
        if (txtFromVch.Value.Length < 1 || txtToVch.Value.Length < 1)
        {
            fgen.msg("-", "AMSG", "Please Select From and To Document to print!!");
            return;
        }
        if (fgen.make_double(txtFromVch.Value.Trim()) > fgen.make_double(txtToVch.Value.Trim()))
        {
            fgen.msg("-", "AMSG", "The Starting Document number must be less then ending doucment number");
            return;
        }
        #endregion

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", txtFromVch.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", txtToVchdt.Value.Trim());

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR1", txtFromVch.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR2", txtFromVchdt.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR3", txtToVch.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR4", txtToVchdt.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR5", txtAcode.Value.Trim());

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", (chkOrig.Checked == true ? "Y" : "N"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", (chkDup.Checked == true ? "Y" : "N"));

        if (rdPDF.SelectedValue == "0") Value1 = "Y";
        else Value1 = "N";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", Value1);
        switch (HCID)
        {
            default:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup1();", true);
                break;
            case "FINSYS_K":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
                break;
        }        
    }
    protected void btnTo_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTNTO";
        if (hf1.Value.Trim().Length <= 2)
        {
            hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        }
        makequery4popup();
    }
    protected void btnFrom_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTNFROM";
        if (hf1.Value.Trim().Length <= 2)
        {
            hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
        }
        makequery4popup();
    }
    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        if (txtFromVch.Value.ToString().Length <= 2 || txtToVch.Value.ToString().Length <= 2)
        {
            fgen.msg("-", "AMSG", "Please Select Entry No. First!!");
            return;
        }
        else
        {
            hffield.Value = "BTNACODE";
            makequery4popup();
        }
    }
}