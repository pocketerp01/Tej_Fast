using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("../login.aspx");


        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        DataRow dr;
        for (int i = 0; i < 100; i++)
        {
            dr = dt.NewRow();
            dr["ID"] = i;
            dr["Name"] = "Name " + i;
            dt.Rows.Add(dr);
        }
        sg1.DataSource = dt;
        sg1.DataBind();
    } 
}