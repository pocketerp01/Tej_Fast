using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.IO;

public partial class dwnloadExcelFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string docType = "";
        if (Request.QueryString["DTR"].Length > 0)
        {
            string file_name = "", ext = "", exp_typ = "", formID = "", header_y_n="";
            DataTable dt = new DataTable();

            dt = (DataTable)HttpContext.Current.Session["EXP_DT"];
            exp_typ = Request.Cookies["exp_type"].Value.ToString();
            ext = Request.Cookies["ext"].Value.ToString();
            file_name = Request.Cookies["file_name"].Value.ToString();
            formID = Request.Cookies["formID"].Value.ToString();

            docType = Request.QueryString["DTR"].Trim().ToString().ToUpper();
            if (docType == "4")
            {
                GridView gridviewName = (GridView)Session["GRIDVIEW"];
                HttpContext.Current.Response.Write("<script>");
                HttpContext.Current.Response.Write("window.open('../tej-base/dwnloadExcelFile.aspx?DTR=4','_blank')");
                HttpContext.Current.Response.Write("</script>");

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Charset = "";
                string FileName = file_name;
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + file_name);
                gridviewName.GridLines = GridLines.Both;
                gridviewName.HeaderStyle.Font.Bold = true;
                gridviewName.RenderControl(htmltextwrtter);
                HttpContext.Current.Response.Write(strwritter.ToString());
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= " + file_name + "." + ext + "");
                HttpContext.Current.Response.ContentType = "application/" + exp_typ + "";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode;
                HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                string str = string.Empty;

                if (dt.Columns.Contains("FSTR")) dt.Columns.Remove("FSTR");
                if (dt.Columns.Contains("fstr")) dt.Columns.Remove("fstr");
                if (dt.Columns.Contains("GSTR")) dt.Columns.Remove("GSTR");
                if (dt.Columns.Contains("gstr")) dt.Columns.Remove("gstr");

                string firmName = "";
                if (file_name.Contains("_"))
                {
                    firmName = fgenCO.chk_co(file_name.Split('_')[0].ToString());
                }
                if (header_y_n == "Y")
                {
                    str = "";
                    HttpContext.Current.Response.Write(str + "Firm :");
                    str = "\t";
                    HttpContext.Current.Response.Write(str + firmName);
                    str = "\n\n";
                }
                foreach (DataColumn dtcol in dt.Columns)
                {
                    HttpContext.Current.Response.Write(str + dtcol.ColumnName);
                    str = "\t";
                }
                HttpContext.Current.Response.Write("\n");
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (formID == "F70717")
                        {
                            if (j == 4)
                                HttpContext.Current.Response.Write(str + Convert.ToString("'" + dr[j]));
                            else HttpContext.Current.Response.Write(str + Convert.ToString(dr[j]));
                        }
                        else HttpContext.Current.Response.Write(str + Convert.ToString(dr[j]));
                        str = "\t";
                    }
                    HttpContext.Current.Response.Write("\n");
                }
                HttpContext.Current.Response.End();
                dt.Dispose();
            }
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
    }
}