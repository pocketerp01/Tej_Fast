using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;
using System.Drawing;

public partial class om_bom_tree : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string a, b, c;
    fgenDB fgen = new fgenDB();
    TreeNode childNode;

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
                    frm_qstr = frm_qstr.Split('@')[0].ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                }

                string frm_cocd, Squery;
                if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
                else
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
                                frm_qstr = frm_qstr.Split('@')[0].ToString();
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                            }
                            frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                            frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                            frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                            frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                            frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                            frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                            lbl1a_Text = "CS";
                            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                            CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                            vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                        }
                        else Response.Redirect("~/login.aspx");
                    }

                    if (!Page.IsPostBack)
                    {
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                        DataTable dt = this.GetData("select  distinct a.icode,b.iname,b.icode as code from  itemosp  a ,item b where trim(a.icode)=trim(b.icode) and  a.type='BM' order by a.ICODE");
                        this.PopulateTreeView(dt, 0, null);
                        //sg1_add_blankrows();
                        create_tab();
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                    }
                }
            }
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeView1.SelectedNodeStyle.BackColor = System.Drawing.Color.Yellow;
        TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Black;
        dt = new DataTable();
        SQuery = "select distinct a.ibcode as icode ,b.iname,b.unit,a.ibqty,b.cpartno,b.cdrgno,b.ent_dt,b.ent_by from  itemosp  a ,item b where trim(a.ibcode)=trim(b.icode) and  a.type='BM' and a.icode='" + TreeView1.SelectedNode.Value + "' order by a.type";
        #region
        // SQuery = "select icode,iname,unit,irate,cpartno,cdrgno,ent_dt,ent_by from item where length(trim(icode))=4 and substr(trim(icode),1,2)='" + TreeView1.SelectedNode.Value + "'";

        // SQuery = "select icode,iname,unit,irate,cpartno,cdrgno,ent_dt,ent_by from item where length(trim(icode))>=8 and substr(trim(icode),1,4)=" + TreeView1.SelectedNode.Value;

        //  if (TreeView1.SelectedNode.Value.Substring(0, 1).ToString() == "7" || TreeView1.SelectedNode.Value.Substring(0, 1).ToString() == "9")

        //{
        //    SQuery = "select  distinct a.ibcode as icode ,b.iname,b.unit,b.irate,b.cpartno,b.cdrgno,b.ent_dt,b.ent_by from  itemosp  a ,item b where trim(a.ibcode)=trim(b.icode) and  a.type='BM' and a.icode='" + TreeView1.SelectedNode.Value + "' order by a.type";

        //}
        #endregion
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = dt;
        sg1.DataBind();
        //     sg1.DataSource = sg1_dt;
        //     sg1.DataBind();
        ////  sg1_add_blankrows();
        lblitem.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where icode='" + TreeView1.SelectedNode.Value + "' order by iname", "INAME");// get it from seekname
        lblicode.Text = "(" + fgen.seek_iname(frm_qstr, frm_cocd, "select icode from item where icode='" + TreeView1.SelectedNode.Value + "' order by icode", "icode") + ")";// get it from seekname
    }

    public void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode)
    {
        foreach (DataRow row in dtParent.Rows)
        {
            TreeNode child = new TreeNode
            {
                Text = row["IName"].ToString(),
                Value = row["Icode"].ToString()
            };
            if (parentId == 0)
            {
                TreeView1.CollapseAll();
                TreeView1.Nodes.Add(child);
                DataTable dtChild = this.GetData("select  distinct a.ibcode ,b.iname from  itemosp  a ,item b where trim(a.ibcode)=trim(b.icode) and  a.type='BM' and a.icode=" + child.Value.Trim() + " order by a.ibcode desc");
                PopulateTreeView1(dtChild, int.Parse(child.Value), child);
            }
            else
            {
                treeNode.ChildNodes.Add(child);
            }
        }
    }

    public void PopulateTreeView1(DataTable dtParent, int parentId, TreeNode treeNode)
    {
        foreach (DataRow row in dtParent.Rows)
        {
            TreeNode child1 = new TreeNode
            {
                Text = row["IName"].ToString(),
                Value = row["Ibcode"].ToString()
            };

            treeNode.ChildNodes.Add(child1);
            DataTable dtChild = this.GetData("select  distinct a.ibcode ,b.iname from  itemosp  a ,item b where trim(a.ibcode)=trim(b.icode) and  a.type='BM' and a.icode=" + child1.Value.Trim() + " order by a.ibcode desc");
            PopulateTreeView2(dtChild, int.Parse(child1.Value), child1);
        }
    }

    public void PopulateTreeView2(DataTable dtParent, int parentId, TreeNode treeNode)
    {
        foreach (DataRow row in dtParent.Rows)
        {
            TreeNode child3 = new TreeNode
            {
                Text = row["IName"].ToString(),
                Value = row["Ibcode"].ToString()
            };

            treeNode.Collapse();
            treeNode.ChildNodes.Add(child3);
            DataTable dtChild = this.GetData("select  distinct a.ibcode ,b.iname from  itemosp  a ,item b where trim(a.ibcode)=trim(b.icode) and  a.type='BM' and a.icode=" + child3.Value.Trim() + " order by a.ibcode desc");
            PopulateTreeView2(dtChild, int.Parse(child3.Value), child3);
        }
    }

    private DataTable GetData(string query)
    {
        DataTable dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, query);
        return dt;
    }

    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg1.Columns.Count; j++)
                {
                    sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }

            //sg1.HeaderRow.Cells[0].Text = "Basic";
            sg1.HeaderRow.Cells[0].Text = "Code";
            sg1.HeaderRow.Cells[1].Text = "Description";
            sg1.HeaderRow.Cells[2].Text = "UOM";
            sg1.HeaderRow.Cells[3].Text = "Quantity";
            sg1.HeaderRow.Cells[4].Text = "Partno";
            sg1.HeaderRow.Cells[5].Text = "Purpose";
            sg1.HeaderRow.Cells[6].Text = "EntryDate";
            sg1.HeaderRow.Cells[7].Text = "EntryBy";
            sg1_add_blankrows();
            sg1_add_blankrows();

        }
    }

    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("icode", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("iname", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("unit", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("ibqty", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("cpartno", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("cdrgno", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("ent_dt", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("ent_by", typeof(string)));
    }

    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();
        sg1_dr["icode"] = "-";
        sg1_dr["iname"] = "-";
        sg1_dr["unit"] = "-";
        sg1_dr["ibqty"] = "-";
        sg1_dr["cpartno"] = "-";
        sg1_dr["cdrgno"] = "-";
        sg1_dr["ent_by"] = "-";
        sg1_dr["ent_dt"] = "-";
        //sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
        sg1_dt.Rows.Add(sg1_dr);
    }

    public void sg1_add_blankrows1()
    {
        dt = new DataTable();
        sg1_dt = new DataTable();
        sg1_dt = dt.Clone();
        sg1_dr = null;
        for (i = 0; i < dt.Rows.Count - 1; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["icode"] = "-";
            sg1_dr["iname"] = "-";
            sg1_dr["unit"] = "-";
            sg1_dr["ibqty"] = "-";
            sg1_dr["cpartno"] = "-";
            sg1_dr["cdrgno"] = "-";
            sg1_dr["date"] = "-";
            sg1_dr["entryby"] = "-";
        }

        sg1_dt.Rows.Add(sg1_dr);
        sg1.DataSource = sg1_dt;
        sg1.DataBind();

    }

    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
}