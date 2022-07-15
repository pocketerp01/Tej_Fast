using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using System.Net;


public partial class frm_ShowImages : System.Web.UI.Page
{
    DataTable dt1; DataRow dr1;
    string frm_url, frm_cocd, frm_qstr, frm_formID;
    protected void Page_Load(object sender, EventArgs e)
    {
        frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
        if (frm_url.Contains("STR"))
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
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COCD", "ADVG");
        string getpath = "";
        dt1 = new DataTable();
        dr1 = null;
        string icode = "";
        string ordno = "";
        dt1.Columns.Add(new DataColumn("srno", typeof(string)));
        dt1.Columns.Add(new DataColumn("icode", typeof(string)));
        dt1.Columns.Add(new DataColumn("imagef", typeof(string)));
        dt1.Columns.Add(new DataColumn("pdf", typeof(string)));
        dt1.Columns.Add(new DataColumn("po", typeof(string)));
        dt1.Columns.Add(new DataColumn("itemdrg", typeof(string)));
        string attach1 = "";
        string poattachfilepath = "";
        if (Session["attach"] != null)
        {
            string file2 = "";
            DataTable path = new DataTable();
            string filepath = "";
            path = (DataTable)(Session["attach"]);
            for (int i = 0; i < path.Rows.Count; i++)
            {
                String[] check = path.Rows[i][0].ToString().Trim().Split('\\');
                for (int k = 0; k < check.Length; k++)
                {
                    string mq1 = check[k].ToString().Trim();
                    file2 = mq1;
                }
                lblheader.Text = "Showing Line Item Drawings and PO Attachments of PO No. " + path.Rows[i]["PRE_ORD"].ToString().Trim() + " Dated " + path.Rows[i]["orddt"].ToString().Trim() + "";
                ordno = path.Rows[i]["ordno"].ToString().Trim();
                icode = path.Rows[i][1].ToString().Trim();
                if (file2.Length > 1)
                {
                    filepath = Server.MapPath("~/UPLOAD/") + ordno + "@" + icode + "@" + file2;
                    string sourcePath = path.Rows[i][0].ToString();
                    string targetPath = filepath;
                    string sourceFile = System.IO.Path.Combine(sourcePath, "");
                    string destFile = System.IO.Path.Combine(targetPath, "");
                    System.IO.File.Copy(sourceFile, destFile, true);
                    getpath = Server.MapPath("~/UPLOAD/") + file2;
                    dr1 = dt1.NewRow();
                    dr1["icode"] = icode;
                    dr1["srno"] = i + 1;
                    string extension = Path.GetExtension(file2);
                    dr1["pdf"] = "~/UPLOAD/" + ordno + "@" + icode + "@" + file2;
                    dr1["po"] = "~/UPLOAD/" + ordno + "@" + icode + "@" + attach1;
                    dr1["itemdrg"] = file2;
                    dt1.Rows.Add(dr1);
                }
                sg1.DataSource = dt1;
                sg1.DataBind();
            }
        }
        if (Session["POAttachment"] != null)
        {
            DataTable dtPOAttach = new DataTable();
            dtPOAttach = (DataTable)(Session["POAttachment"]);
            for (int i = 0; i < dtPOAttach.Rows.Count; i++)
            {
                string[] poAttach = dtPOAttach.Rows[i][0].ToString().Trim().Split('\\');
                for (int m = 0; m < poAttach.Length; m++)
                {
                    string mq2 = poAttach[m].ToString().Trim();
                    attach1 = mq2;
                }
                ordno = dtPOAttach.Rows[i]["ordno"].ToString().Trim();
                poattachfilepath = Server.MapPath("~/UPLOAD/") + ordno + "@" + attach1;
                string sourcePath = dtPOAttach.Rows[i][0].ToString();
                string targetpath1 = poattachfilepath;
                string sourceFile = System.IO.Path.Combine(sourcePath, "");
                string destFile1 = System.IO.Path.Combine(targetpath1, "");
                System.IO.File.Copy(sourceFile, destFile1, true);
                A1.HRef = "~/UPLOAD/" + ordno + "@" + attach1;
                lbl.Text = attach1;
            }
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // for word wrap in case of large text , makes grid if std size
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = 50;
            sg1.HeaderRow.Cells[0].Width = 50;
            e.Row.Cells[1].Width = 100;
            sg1.HeaderRow.Cells[1].Width = 100;
            e.Row.Cells[2].Width = 100;
            sg1.HeaderRow.Cells[2].Width = 100;
        }
    }
}