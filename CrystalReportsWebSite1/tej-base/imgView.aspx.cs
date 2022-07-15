using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fin_base_imgView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["imgName"] != null)
        {
            Response.ContentType = "image/jpeg"; // for JPEG file
            string physicalFileName = Request.QueryString["imgName"].ToString().Trim();
            Response.WriteFile(physicalFileName);
        }
        if (Request.QueryString["viewfile"] != null)
        {
            Response.ContentType = "image/jpeg"; // for JPEG file
            string FileName = Request.QueryString["viewfile"].ToString().Trim();
            FileName = Server.MapPath("~\\tej-base\\UPLOAD\\") + FileName;
            Response.WriteFile(FileName);
        }
    }
}