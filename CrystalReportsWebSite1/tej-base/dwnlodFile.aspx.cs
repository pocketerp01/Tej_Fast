using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fin_base_dwnlodFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = Session["FileName"].ToString();
        string filePath = Session["FilePath"].ToString();
        Response.Clear();
        Response.ClearContent();
        Response.ContentType = ContentType;
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        if (filePath.Contains("tiff")) Response.WriteFile("c:/tej_erp/tiff/" + fileName);
        else
        {
            try
            {
                Response.WriteFile("c:/tej_erp/Upload/" + filePath);
            }
            catch
            {
                Response.WriteFile(Server.MapPath("Upload/" + filePath));
            }
        }
        // else Response.WriteFile("c:/tej_erp/Upload/" );
        Response.End();
        Response.Flush();
        Response.Close();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
    }
}