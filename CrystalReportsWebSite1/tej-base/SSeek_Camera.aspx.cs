using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;

using System.Drawing;
using System.IO;

public partial class SSeek_Camera : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "-", Value2 = "-", Value3 = "-", Value4 = "-", Value5 = "-", Value6 = "-", Value7 = "-", Value8 = "-", Value9 = "-", Value10 = "-";
    string HCID, co_cd, fpath, fName; int col_count = 0;
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
            if (frm_qstr.Contains("~"))
            {
                if (frm_cocd != frm_qstr.Split('~')[0].ToString())
                {
                    frm_cocd = frm_qstr.Split('~')[0].ToString();
                }
            }
            co_cd = frm_cocd;
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            if (!Page.IsPostBack)
            {
                fName = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL10") + ".png";
                fpath = Server.MapPath(@"~/tej-base/Upload") + "\\" + fName;
                if (File.Exists(fpath))
                {
                    empImage.ImageUrl = "~/tej-base/Upload/" + fName;
                }
            }
        }
    }

    protected void btnhide_Click(object sender, EventArgs e)
    {
        if (imgData.Value == null | imgData.Value == "") { fpath = "-"; }
        else
        {
            string imgStr = imgData.Value;
            byte[] bytes = Convert.FromBase64String(imgStr);
            Image img = byteArrayToImage(bytes);

            fName = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL10") + ".png";
            fpath = Server.MapPath(@"~/tej-base/Upload") + "\\" + fName;
            img.Save(fpath, System.Drawing.Imaging.ImageFormat.Png);

            if (File.Exists(fpath))
            {
                empImage.ImageUrl = "~/tej-base/Upload/" + fName;
            }
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CAM1", fpath);

        //switch (HCID)
        //{
        //    case "Tejaxo":
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF');", true);
        //        break;
        //    case "FINSYS_S":
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#ContentPlaceHolder1_btnhideF_s');", true);
        //        break;
        //    case "DATA":
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
        //        break;
        //    case "YR":
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnhideF');", true);
        //        break;
        //    case "IBOX":
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btniBox');", true);
        //        break;
        //}
    }

    public Image byteArrayToImage(byte[] byteArrayIn)
    {
        Stream ms = new MemoryStream(byteArrayIn);
        Image returnImage = Image.FromStream(ms);
        return returnImage;
    }
}