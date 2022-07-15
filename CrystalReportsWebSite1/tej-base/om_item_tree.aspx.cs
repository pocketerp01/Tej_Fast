using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;

public partial class om_item_tree : System.Web.UI.Page
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

                        lblheader.Text = "Item Tree View";
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");

                        DataTable dt = this.GetData("select type1,type1||'-'||name as Name from type where id='Y' order by type1");
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

        if (TreeView1.SelectedNode.Value.ToString().Length < 4)
        {
            SQuery = "select icode,iname,unit,irate,cpartno,cdrgno,ent_dt,ent_by from item where length(trim(icode))=4 and substr(trim(icode),1,2)='" + TreeView1.SelectedNode.Value.Trim() + "' order by iname";

        }
        else
        {
            SQuery = "select icode,iname,unit,irate,cpartno,cdrgno,ent_dt,ent_by from item where length(trim(icode))>=6 and substr(trim(icode),1,4)='" + TreeView1.SelectedNode.Value.Trim() + "' order by iname";
        }

        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1.DataSource = dt;
        sg1.DataBind();
        lblitem.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='Y'and type1='" + TreeView1.SelectedNode.Value.Trim().Substring(0, 2) + "' order by name", "NAME");// get it from seekname

    }

    public void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode)
    {
        foreach (DataRow row in dtParent.Rows)
        {
            TreeNode child = new TreeNode()
            {
                Text = row["Name"].ToString(),
                Value = row["Type1"].ToString()
            };
            if (parentId == 0)
            {
                TreeView1.CollapseAll();
                TreeView1.Nodes.Add(child);
                DataTable dtChild = this.GetData("select icode, iname from item where length(trim(icode))=4 and substr(trim(icode),1,2) = '" + child.Value + "' order by iname");
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
                Value = row["Icode"].ToString()
            };
            treeNode.ChildNodes.Add(child1);
        }
    }

    public void PopulateTreeView2(DataTable dtParent, int parentId, TreeNode treeNode)
    {
        foreach (DataRow row in dtParent.Rows)
        {
            TreeNode child3 = new TreeNode
            {
                Text = row["IName"].ToString(),
                Value = row["Icode"].ToString()
            };

            treeNode.Collapse();
            treeNode.ChildNodes.Add(child3);

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

            sg1.HeaderRow.Cells[0].Text = "Item_Code";
            sg1.HeaderRow.Cells[1].Text = "Item_Name";
            sg1.HeaderRow.Cells[2].Text = "Part_no";
            sg1.HeaderRow.Cells[3].Text = "Unit";
            sg1.HeaderRow.Cells[4].Text = "Drg_No";
            sg1.HeaderRow.Cells[5].Text = "Std_Rate";
            sg1.HeaderRow.Cells[6].Text = "Entry_Dt";
            sg1.HeaderRow.Cells[7].Text = "Entry_By";
            //sg1_add_blankrows();

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
        sg1_dt.Columns.Add(new DataColumn("irate", typeof(string)));
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
        sg1_dr["irate"] = "-";
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
            sg1_dr["irate"] = "-";
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

    //void fillIconTree()
    //{
    //    TreeNode parentNode = null;
    //    dt = new DataTable();

    //    // dt = fgen.fill_icon_grid(frm_cocd, tab_name, cond);
    //    dt = fgen.getdata(frm_qstr, frm_cocd, "select type1,Name from type where id='Y' order by type1");
    //    if (dt.Rows.Count > 0)
    //    {
    //        #region Get the Distinct Form ID's
    //        var res = from type in dt.AsEnumerable()
    //                  where type.Field<string>("id") == "Y"
    //                  select new
    //                  {
    //                      WebForm_id = type.Field<string>("type"),
    //                      WebForm_Text = type.Field<string>("Name"),
    //                      //SubMenuID = type.Field<string>("Submenuid"),
    //                      //Web_Action = type.Field<string>("web_action"),
    //                      //fin_rsys_Form = type.Field<string>("Form"),
    //                      //fin_rsys_Param = type.Field<string>("Param"),
    //                      //fin_rsys_UserCol = FIN_MSYS.Field<string>("user_color")
    //                  };
    //        #endregion

    //        foreach (var r in res)
    //        {
    //            //if (r.fin_rsys_UserCol.Length > 2) userStyle.Style.Add("background-color", "#" + dt.Rows[0]["user_color"].ToString().Trim());
    //            //else userStyle.Style.Add("background-color", "#00578b");

    //            parentNode = new TreeNode(r.WebForm_Text, r.WebForm_id);
    //            #region Fiiling 2nd Level / Child
    //            var result = from ITEM in dt.AsEnumerable()
    //                         where ITEM.Field<string>("SUBSTR(ICODE),1,2") == r.WebForm_Text && ITEM.Field<string>("length(trim(icode))")="4"
    //                         select new
    //                         {
    //                             Child_FormID = item.Field<string>("icode"),
    //                             Child_FormName = item.Field<string>("iname"),
    //                             //                     Child_fin_rsys_Form = g.Field<string>("form"),
    //                             //                     Child_SubMenu = g.Field<string>("SUBMENUID"),
    //                             //                     Child_WebAction = g.Field<string>("web_action")
    //                         };
    //            #endregion
    //            foreach (var data in result)
    //            {
    //                // Second Level Icon
    //                childNode = new TreeNode(data.Child_FormName, data.Child_FormID);
    //                parentNode.ChildNodes.Add(childNode);

    //                #region Filling 3rd Level / Child
    //                var result1 = from item in dt.AsEnumerable()
    //                              where item.Field<string>("SUBSTR(ICODE),1,4") == data.Child_FormID && item.Field<decimal>("length(trim(icode))") >= 8
    //                              select new
    //                              {
    //                                  SubChild_FormID = item.Field<string>("icode"),
    //                                  SubChild_FormName = item.Field<string>("iname"),
    //                                  //SubChild_SubMenu = v.Field<string>("SUBMENUID"),
    //                                  //SubChild_WebAction = v.Field<string>("web_action")
    //                              };
    //                #endregion
    //                foreach (var data1 in result1)
    //                {
    //                    TreeNode cnodeInner = new TreeNode(data1.SubChild_FormName, data1.SubChild_FormID);
    //                    childNode.ChildNodes.Add(cnodeInner);
    //                    childNode.Collapse();
    //                }
    //            }
    //            parentNode.Collapse();
    //            trview.Nodes.Add(parentNode);
    //        }
    //    }
    //    //else fgen.msg("-", "AMSG", "Rights are not allowed'13'Please ask Admin to allocate rights");
    //}

}




