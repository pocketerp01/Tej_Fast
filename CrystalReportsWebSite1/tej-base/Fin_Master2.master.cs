using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class fin_base_Pocket : System.Web.UI.MasterPage
{
    string Cstr, str, co_cd = "", mbr = "", hname, br_name, btnval, yr, val1, tab_name, mhd, uname, cond, squery, col2,
       bval, get_qstr, url, usr_dept, frm_formID, frm_ulevel, frm_CDT1, frm_CDT2, frm_grp;
    fgenDB fgen = new fgenDB();
    DataTable dt, dt1;
    protected void btnReset_Click(object sender, EventArgs e)
    {
        //hfval.Value = "YR_CHANGE";
        //fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", "YR");
        //fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", "select to_char(fmdate,'yyyy')||'-'||to_char(todate,'yyyy') as fstr, 'Change To' as Change_to,to_char(fmdate,'dd/mm/yyyy') as from_date,to_char(todate,'dd/mm/yyyy') as To_Date from co where upper(Trim(code)) like '" + co_cd + "%' order by fstr desc");
        //fgen.Fn_open_sseek("Select Year", get_qstr);
    }
    //protected void btnbranch_ServerClick(object sender, EventArgs e)
    //{
    //    hfval.Value = "BR_CHANGE"; val1 = "-";
    //    dt1 = new DataTable();
    //    dt1 = fgen.getdata(get_qstr, co_cd, "select allowbr from evas where trim(upper(username))='" + lblusername.InnerText.ToUpper() + "'");
    //    if (dt1.Rows.Count > 0) val1 = dt1.Rows[0]["allowbr"].ToString().Trim();
    //    if (val1 == "-" || val1 == null || val1.Length == 0) squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode from type where id='B' order by type1";
    //    else
    //    {
    //        dt = new DataTable();
    //        dt = fgen.getdata(get_qstr, co_cd, "select distinct type1 from type where id='B'");
    //        col2 = "";
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            if (val1.Contains(dr["type1"].ToString().Trim()))
    //            {
    //                if (col2.Length > 0)
    //                {
    //                    col2 = col2 + "," + "'" + dr["type1"].ToString().Trim() + "'";
    //                }
    //                else
    //                {
    //                    col2 = "'" + dr["type1"].ToString().Trim() + "'";
    //                }
    //            }
    //        }
    //        squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode from type where id='B' and type1 in (" + col2 + ") order by type1";
    //    }
    //    fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", "YR");
    //    fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", squery);
    //    fgen.Fn_open_sseek("Select Your Branch", get_qstr);
    //}

    //protected void Unnamed_ServerClick(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/login.aspx");
    //}
    //protected void btnhideF_Click(object sender, EventArgs e)
    //{
    //    btnval = hfval.Value;
    //    switch (btnval)
    //    {
    //        case "LOGOUT":
    //            try
    //            {
    //                btnval = Request.Cookies["REPLY"].Value.ToString().Trim();
    //                if (btnval == "Y") Response.Redirect("~/tej-base/signout.aspx");
    //            }
    //            catch
    //            {
    //                Response.Redirect("~/tej-base/signout.aspx");
    //            }
    //            break;
    //        case "ERP_MSG":

    //            return;
    //        case "YR_CHANGE":
    //            val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
    //            if (val1.Length > 0) { }
    //            else return;

    //            dt = new DataTable();
    //            dt = fgen.GetYearDetails(get_qstr, co_cd, val1.Substring(0, 4));

    //            if (dt.Rows.Count > 0)
    //            {
    //                fgenMV.Fn_Set_Mvar(get_qstr, "U_YEAR", val1.Substring(0, 4));
    //                fgenMV.Fn_Set_Mvar(get_qstr, "U_FYEAR", dt.Rows[0]["fstr"].ToString().Trim());

    //                fgenMV.Fn_Set_Mvar(get_qstr, "U_DATERANGE", " between to_date('" + dt.Rows[0]["cdt1"].ToString().Trim() + "','dd/mm/yyyy') and to_date('" + dt.Rows[0]["cdt2"].ToString().Trim() + "','dd/mm/yyyy')");
    //                fgenMV.Fn_Set_Mvar(get_qstr, "U_CDT1", dt.Rows[0]["cdt1"].ToString().Trim());
    //                fgenMV.Fn_Set_Mvar(get_qstr, "U_CDT2", dt.Rows[0]["cdt2"].ToString().Trim());
    //            }
    //            yr = val1;
    //            fill_val();
    //            Response.Redirect("~/tej-base/desktop.aspx?STR=" + get_qstr, false);
    //            break;
    //        case "BR_CHANGE":
    //            val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
    //            if (val1.Length > 0) { }
    //            else return;
    //            fgen.send_cookie("BRANCH", val1);
    //            //lblbrcode.Text = val1;
    //            string branch_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
    //            fgenMV.Fn_Set_Mvar(get_qstr, "U_MBR", val1);
    //            fgenMV.Fn_Set_Mvar(get_qstr, "U_MBR_NAME", branch_name);


    //            string ind_Ptype = "";
    //            ind_Ptype = fgen.seek_iname(get_qstr, co_cd.ToUpper().Trim(), "select trim(upper(opt_param)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + mbr + "' and OPT_ID='W1000'", "fstr");
    //            if (ind_Ptype != "-")
    //            {
    //                fgenMV.Fn_Set_Mvar(get_qstr, "U_IND_PTYPE", ind_Ptype);
    //            }
    //            fill_val();
    //            Response.Redirect("~/tej-base/desktop.aspx?STR=" + get_qstr, false);
    //            break;
    //        case "OPEN_ICON":
    //            val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
    //            if (val1 == "EXIT") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OnlyClose();", true);
    //            else
    //            {
    //                if (val1.Length > 0) { }
    //                else return;
    //                fgenMV.Fn_Set_Mvar(get_qstr, "U_FORMID", val1);
    //                Response.Redirect("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
    //                //Server.Transfer("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
    //            }
    //            break;
    //        case "BOOKMARK":
    //            if (fgenMV.Fn_Get_Mvar(get_qstr, "U_COL5").ToString().Trim().Replace("&amp", "").Length > 3)
    //            {
    //                val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
    //                if (val1 == "EXIT") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OnlyClose();", true);
    //                else
    //                {
    //                    fgenMV.Fn_Set_Mvar(get_qstr, "U_FORMID", val1);
    //                    Response.Redirect("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
    //                    //Server.Transfer("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
    //                }
    //            }
    //            //else
    //            //bookMarkMenu(fgenMV.Fn_Get_Mvar(get_qstr, "U_COL3").ToString().Trim());
    //            break;
    //    }
    //}
    public void fill_val()
    {
        string logopath = "~/tej-base/images/t_logo_w.png";
        if (frm_grp == "S")
        {
            logopath = "~/tej-base/images/s_logo_w.png";
        }
        imglogo2.ImageUrl = logopath;

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        url = HttpContext.Current.Request.Url.AbsoluteUri;
        if (url.Contains("STR"))
        {
            if (Request.QueryString["STR"].Length > 0)
            {
                get_qstr = Request.QueryString["STR"].Trim().ToString();
                if (get_qstr.Contains("@"))
                {
                    frm_formID = get_qstr.Split('@')[1].ToString();
                    get_qstr = get_qstr.Split('@')[0].ToString();
                }

                fgenMV.Fn_Set_Mvar(get_qstr, "QSTR", get_qstr);
                co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");
                frm_grp = fgenMV.Fn_Get_Mvar(get_qstr, "U_COGRP");
                yr = fgenMV.Fn_Get_Mvar(get_qstr, "U_FYEAR");
                uname = fgenMV.Fn_Get_Mvar(get_qstr, "U_UNAME");
                cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");
                mbr = fgenMV.Fn_Get_Mvar(get_qstr, "U_MBR");
                br_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_MBR_NAME");
                usr_dept = fgenMV.Fn_Get_Mvar(get_qstr, "U_DEP_NAME");
                frm_ulevel = fgenMV.Fn_Get_Mvar(get_qstr, "U_ULEVEL");
                if (co_cd.Length <= 1) Response.Redirect("~/login.aspx");
                lblusername.InnerText = uname;
                txtcompname.InnerText = "Welcome " + "(" + co_cd + ")" + fgenCO.chk_co(co_cd) + " (" + mbr + ")";
                frm_CDT1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_CDT1");
                frm_CDT2 = fgenMV.Fn_Get_Mvar(get_qstr, "U_CDT2");
            }
            else Response.Redirect("~/login.aspx");
            hfPass.Value = fgenMV.Fn_Get_Mvar(get_qstr, "U_PWD");
            Page.Title = "Tejaxo ERP";
            lblcopyright.Text = "Tejaxo.com";
            lblcopyright.NavigateUrl = "http://www.tejaxo.com/";
            if (frm_grp == "S")
            {
                Page.Title = "Sofgen ERP";
                lblcopyright.Text = "Sofgen.org";
                lblcopyright.NavigateUrl = "http://www.sofgen.org/";
            }
            navmenu.Controls.Add(new LiteralControl(Mymenu(get_qstr)));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //if (hfWindowSize.Value.ToString() == "") hfWindowSize.Value = fgenMV.Fn_Get_Mvar(get_qstr, "FRMWINDOWSIZE");
            //fgenMV.Fn_Set_Mvar(get_qstr, "FRMWINDOWSIZE", hfWindowSize.Value.ToString());
            //Session["hfWindowSize"] = hfWindowSize.Value;
            if (!Page.IsPostBack)
            {
                //int _displayTimeInMiliSec = (Session.Timeout - 1) * 60000;
                //if (Session["ID"] == null)
                //{
                //    Session["ID"] = "New Session";
                //}
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(),
                //    "message",
                //    "<script type=\"text/javascript\" language=\"javascript\">Timer('" + _displayTimeInMiliSec + "');</script>",
                //    false);
                //Mymenu(get_qstr);
            }
            fill_val();
        }
    }
    string html = "";
    int cli = 0;
    string css = "fa fa-edit";

    private string Mymenu(string MyGuid)
    {
        try
        {

            cli = 0;

            fgenDB sgen = new fgenDB();

            if (Session[get_qstr + "_MenuHTML"] == null || Session[get_qstr + "_MenuHTML"].ToString().Trim() == "")
            {
                #region Get the value from DataBase
                DataTable dtparent = new DataTable();

                #region Command Section


                //string userid_mst = multiton.userid_mst;
                //string m_module1 = multiton.M_Module3;
                //string m_module2 = multiton.M_Module3;
                //string m_module3 = multiton.M_Module3;
                //string ulevel_mst = multiton.ulevel_mst;
                //string m_id = multiton.Module_id;
                //string utype_mst = multiton.utype_mst;
                //string role_mst = multiton.role_mst;
                //if (ulevel_mst == "") { ulevel_mst = "0"; }

                string mq = "";
                string tab_name = fgenMV.Fn_Get_Mvar(MyGuid, "U_ICONTAB");
                string cond = fgenMV.Fn_Get_Mvar(MyGuid, "U_ICONCOND");


                #endregion

                dtparent = sgen.fill_icon_grid(co_cd, tab_name, cond, MyGuid);

                dtparent.AcceptChanges();
                Session["dtparent"] = dtparent;

                DataTable dt0 = dtparent.AsEnumerable().Where(w => Convert.ToInt64(w["mlevel"]) == 1).Select(s => s).CopyToDataTable();
                //html = "<ul id='myMenu' class='nav side-menu'>";
                html = html + "<ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>";




                foreach (DataRow dr in dt0.Rows)
                {
                    //css = dr["css"].ToString().Trim();
                    if (!dr["submenu"].ToString().Equals("Y") && dr["web_action"].ToString().Trim().Length > 2)
                    {
                        cli++;
                        if (dr["web_action"].ToString().Trim().ToLower().Equals("prnfile"))
                        {
                            html = html + "<li id='l" + cli + "' class='nav-item'> " +
                                "<a  href='javascript:void(0);'  prd='" + (dr["prd"].ToString() + dr["brn"].ToString()).Trim().ToUpper() + "' onclick='ShowRPT(this);' id='m" + dr["id"].ToString() + "' >" +
                              dr["text"].ToString() + "</a></li>";
                        }
                        else if (dr["web_action"].ToString().Trim().ToLower().Equals("withfoo"))
                        {
                            html = html + "<li id='l" + cli + "'> <a  href='javascript:void(0);'  prd='" + (dr["prd"].ToString() + dr["brn"].ToString()).Trim().ToUpper() + "' onclick='withfoo(this);' id='m" + dr["id"].ToString() + "' >" +
                              dr["text"].ToString() + "</a></li>";
                        }
                        else
                        {
                            html = html + "<li id='l" + cli + "' class='nav-item'> <a  href='../" + dr["web_action"].ToString() + "?guid=" + EncryptDecrypt.Encrypt(MyGuid) +
                        "&mid=" + EncryptDecrypt.Encrypt(dr["id"].ToString()) + "&STR=" + MyGuid + "@" + dr["id"].ToString().Trim() + "' id='m" + dr["id"].ToString() + "' class='nav-link' >" +
                        "<i class='far fa-circle nav-icon'></i>" + dr["text"].ToString() + "</a></li>";
                        }
                    }
                    else
                    {

                        cli++;
                        html = html + "<li id='l" + cli + "' class='nav-item'> <a href='#' id='a" + cli + "'  class='nav-link'>" +
                            "<i class='nav-icon fas fa-forward'></i><p>" + dr["text"].ToString() + "" +
                            " <i class='fas fa-angle-left right'></i></p></a>";
                        html = html + "<ul id='u" + cli + "' class='nav nav-treeview'>";
                        makemenu(dtparent, dr["form"].ToString().Trim(), dr["submenuid"].ToString().Trim(), Convert.ToInt64(dr["mlevel"].ToString()) + 1, MyGuid);
                        html = html + " </ul>";
                    }
                }

                html = html + " </ul>";
                //html = html + " </ul>";
                #endregion
                Session[get_qstr + "_MenuHTML"] = html;

            }
            else
            {
                html = (String)Session[get_qstr + "_MenuHTML"];
            }
        }
        catch (Exception err)
        {

        }
        return html;
    }
    private void makemenu(DataTable dtparent, string module3, string module1, long level, string MyGuid)
    {
        if (module1.Trim() == "fin45_d1") { }
        if (level > 4) return;
        try
        {
            DataTable dtstatuswise;
            if (level == 2)
            {
                dtstatuswise = dtparent.Select("form='" + module3 + "' and mlevel='" + level + "'").CopyToDataTable();
            }
            else
            {
                dtstatuswise = dtparent.Select("submenuid='" + module1 + "' and mlevel='" + level + "'").CopyToDataTable();
            }



            foreach (DataRow dr in dtstatuswise.Rows)
            {
                //css = dr["css"].ToString().Trim();
                if (!dr["submenu"].ToString().Equals("Y") && dr["web_action"].ToString().Trim().Length > 2)
                {
                    cli++;
                    if (dr["web_action"].ToString().Trim().ToLower().Equals("prnfile"))
                    {
                        html = html + "<li id='l" + cli + "'> <a  href='javascript:void(0);'  prd='" + (dr["prd"].ToString() + dr["brn"].ToString()).Trim().ToUpper() + "' onclick='ShowRPT(this);' id='m" + dr["id"].ToString() + "' >" +
                                       dr["text"].ToString() + "</a></li>";
                    }
                    else if (dr["web_action"].ToString().Trim().ToLower().Equals("withfoo"))
                    {
                        html = html + "<li id='l" + cli + "'> <a  href='javascript:void(0);'  prd='" + (dr["prd"].ToString() + dr["brn"].ToString()).Trim().ToUpper() + "' onclick='withfoo(this);' id='m" + dr["id"].ToString() + "' >" +
                          dr["text"].ToString() + "</a></li>";
                    }
                    else
                    {
                        html = html + "<li id='l" + cli + "' class='nav-item'> <a  href='../" + dr["web_action"].ToString() + "?guid=" + EncryptDecrypt.Encrypt(MyGuid) +
                               "&mid=" + EncryptDecrypt.Encrypt(dr["id"].ToString()) + "&STR=" + MyGuid + "@" + dr["id"].ToString().Trim() + "' id='m" + dr["id"].ToString() + "' class='nav-link' >" +
                               "<i class='far fa-circle nav-icon'></i>" + dr["text"].ToString() + "</a></li>";
                    }
                }
                else
                {
                    cli++;
                    html = html + "<li id='l" + cli + "'  class='nav-item'> <a href='#' id='a" + cli + "'  class='nav-link'>" +
                                   "<i class='nav-icon fas fa-forward'></i><p>" + dr["text"].ToString() + "" +
                                   " <i class='fas fa-angle-left right'></i></p></a>";
                    html = html + "<ul id='u" + cli + "' class='nav nav-treeview'>";
                    makemenu(dtparent, dr["form"].ToString().Trim(), dr["submenuid"].ToString().Trim(), Convert.ToInt64(dr["mlevel"].ToString()) + 1, MyGuid);
                    html = html + " </ul>";
                }
            }
        }
        catch (Exception err) { }
    }

    //private void Create_Menu()
    //{
    //    string html_tag = "";
    //    fgenDB fgen = new fgenDB();
    //    co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");
    //    tab_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONTAB");
    //    cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");

    //    #region Get the value from DataBase
    //    DataTable dt = new DataTable();
    //    if (Session["dt_menu" + get_qstr] == null)
    //    {
    //        dt =fgen.fill_icon_grid(co_cd, tab_name, cond, get_qstr);
    //    }
    //    else dt = (DataTable)Session["dt_menu" + get_qstr];
    //    Session["dt_menu" + get_qstr] = dt;
    //    //dt = fgen.getdata("GTCF", "select * from ico_tab order by id");
    //    #endregion

    //    string color = "skin-purple";
    //    //if (tab_name.ToUpper() == "ico_wtab") color = dt.Rows[0]["user_color"].ToString().Trim();
    //    //body1.Attributes.Add("class", "hold-transition " + color + " sidebar-mini sidebar-collapse");
    //    //body1.Attributes.Add("class", "sidebar-mini sidebar " + color + "");

    //    #region Get the Distinct Form ID's
    //    var res = from ico_tab in dt.AsEnumerable()
    //              where ico_tab.Field<decimal>("mlevel") == 1
    //              select new
    //              {
    //                  WebForm_id = ico_tab.Field<string>("id"),
    //                  WebForm_Text = ico_tab.Field<string>("text"),
    //                  SubMenuID = ico_tab.Field<string>("Submenuid"),
    //                  Web_Action = ico_tab.Field<string>("web_action"),
    //                  ico_tab_Form = ico_tab.Field<string>("Form"),
    //                  ico_tab_Param = ico_tab.Field<string>("Param"),
    //                  ico_tab_CSS = ico_tab.Field<string>("CSS"),
    //                  ico_tab_mlevel = ico_tab.Field<decimal>("mlevel")
    //              };
    //    #endregion

    //    html_tag = html_tag + "<ul class='sidebar-menu'>";
    //    foreach (var r in res)
    //    {
    //        #region Fiiling 2nd Level / Child
    //        var result = from g in dt.AsEnumerable()
    //                     where g.Field<string>("form") == r.ico_tab_Form && g.Field<decimal>("mlevel") == 2
    //                     select new
    //                     {
    //                         Child_FormID = g.Field<string>("id"),
    //                         Child_FormName = g.Field<string>("text"),
    //                         Child_ico_tab_Form = g.Field<string>("form"),
    //                         Child_SubMenu = g.Field<string>("SUBMENUID"),
    //                         Child_WebAction = g.Field<string>("web_action"),
    //                         Child_mlevel = g.Field<decimal>("mlevel")
    //                     };
    //        #endregion

    //        #region Create 1st Level Icons / Grand Father
    //        html_tag = html_tag + "<li class='treeview'>";
    //        html_tag = html_tag + "<a href='" + r.Web_Action + "'?STR=" + get_qstr + "@" + r.WebForm_id + "> <i class='fa " + r.ico_tab_CSS + "'></i>";
    //        html_tag = html_tag + "<span>" + r.WebForm_Text + "</span>";
    //        html_tag = html_tag + " <i class='fa fa-angle-left pull-right'></i> </a>";
    //        #endregion

    //        #region Create 2nd Level / Father and 3rd Level / Child Icons
    //        html_tag = html_tag + "<ul class='treeview-menu'>";
    //        foreach (var data in result)
    //        {
    //            if (data.Child_WebAction.Length > 2)
    //                html_tag = html_tag + "<li class='treeview'><a href='" + data.Child_WebAction + "?STR=" + get_qstr + "@" + data.Child_FormID + "' ><i class='fa fa-circle-o'></i> <span>" + data.Child_FormName + "</span>  </a></li>";

    //            #region Filling 3rd Level / Child
    //            var result1 = from v in dt.AsEnumerable()
    //                          where v.Field<string>("submenuid") == data.Child_SubMenu && v.Field<decimal>("mlevel") == 3
    //                          select new
    //                          {
    //                              SubChild_FormID = v.Field<string>("id"),
    //                              SubChild_FormName = v.Field<string>("text"),
    //                              SubChild_SubMenu = v.Field<string>("SUBMENUID"),
    //                              SubChild_WebAction = v.Field<string>("web_action"),
    //                              SubChild_mlevel = v.Field<decimal>("mlevel")
    //                          };

    //            html_tag = html_tag + "<li>";

    //            if (data.Child_mlevel >= 2)
    //            {
    //                foreach (var data2 in result1)
    //                {
    //                    html_tag = html_tag + "<a href='#'><i class='fa fa-circle-o'></i> <span>" + data.Child_FormName + "</span><i class='fa fa-angle-left pull-right'></i></a>";
    //                    if (data.Child_SubMenu == data2.SubChild_SubMenu)
    //                        html_tag = html_tag + "<ul class='treeview-menu'>";
    //                    break;
    //                }
    //                foreach (var data2 in result1)
    //                {
    //                    if (data.Child_SubMenu == data2.SubChild_SubMenu)
    //                    {
    //                        html_tag = html_tag + "<li ><a href='" + data2.SubChild_WebAction + "?STR=" + get_qstr + "@" + data2.SubChild_FormID + "'><i class='fa fa-circle-o'></i><span>" + data2.SubChild_FormName + "</span></a></li>";
    //                    }
    //                }
    //                foreach (var data2 in result1)
    //                {
    //                    if (data.Child_SubMenu == data2.SubChild_SubMenu)
    //                        html_tag = html_tag + "</ul>";
    //                    break;
    //                }
    //            }

    //            html_tag = html_tag + "</li>";
    //            #endregion
    //        }
    //        html_tag = html_tag + "</ul>";
    //        html_tag = html_tag + "</li>";

    //        #endregion
    //    }
    //    html_tag = html_tag + "</ul>";

    //    cssmenu.Controls.Add(new LiteralControl(html_tag));
    //}
}
