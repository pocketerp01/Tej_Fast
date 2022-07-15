using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class fin_base_controls_menuAws : System.Web.UI.UserControl
{
    fgenDB fgen = new fgenDB();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname;
    string frm_tabname, frm_formID;
    string html_tag = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Tejaxo";
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx", false);
        else
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Request.QueryString["STR"].Trim().ToString();
                    if (frm_qstr.Contains("@"))
                    {
                        frm_formID = frm_qstr.Split('@')[1].ToString();
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                    }

                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");

                    if (frm_cocd.Length <= 1) Response.Redirect("~/login.aspx", false);
                }
                else Response.Redirect("~/login.aspx");

                if (!Page.IsPostBack)
                    fillMenu();
            }
        }
    }
    void fillMenu()
    {
        frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB");
        string cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONCOND");
        DataTable dt = new DataTable();
        System.Text.StringBuilder strMenu = new System.Text.StringBuilder();

        if (Session["html_tag" + frm_qstr] == null)
        {
            #region Get the value from DataBase
            dt = new DataTable();
            if (Session["dt_menu" + frm_qstr] == null)
            {
                dt = fgen.fill_icon_grid(frm_cocd, frm_tabname, cond, frm_qstr);
            }
            else dt = (DataTable)Session["dt_menu" + frm_qstr];
            Session["dt_menu" + frm_qstr] = dt;
            #endregion

            if (dt.Rows.Count <= 0)
            {
                fgen.msg("-", "AMSG", "Icons Not Allocated!!'13'Please Contact to Administrator");
                return;
            }

            #region Menu Filling Method
            //Fatching 1st Level / Grand Father
            DataView dvm1 = new DataView(dt, "mlevel=1", "ID, form, submenuid", DataViewRowState.CurrentRows);
            strMenu.Append("<nav id='mysidebarmenu' class='amazonmenu'>");
            strMenu.Append("<ul>");

            for (int i = 0; i < dvm1.Count; i++)
            {
                #region filling 1st Level / Grand Father
                strMenu.Append("<li>");
                string MenuDesc = "";
                string MenuDescToolTip = "";
                if (dvm1[i]["search_key"].ToString().Trim().Length > 2)
                {
                    MenuDesc = "<h6 class='v1'>" + dvm1[i]["search_key"].ToString().Trim() + "</h6>";
                    MenuDescToolTip = dvm1[i]["search_key"].ToString().Trim();
                }
                strMenu.Append("<a href='" + dvm1[i]["web_action"].ToString().Trim() + "' title='" + MenuDescToolTip + "'?STR=" + frm_qstr + "@" + dvm1[i]["id"].ToString().Trim() + ">");
                strMenu.Append("" + dvm1[i]["text"].ToString().Trim() + "");
                strMenu.Append("</a>" + MenuDesc);
                strMenu.Append("<div>");
                #endregion

                #region Filling 2nd Level / Father.....
                //Fatching 2nd Level / Father
                DataView dvm2 = new DataView(dt, "mlevel=2 and form='" + dvm1[i]["form"].ToString().Trim() + "'", "ID , form, submenuid", DataViewRowState.CurrentRows);
                strMenu.Append("<ul>");
                for (int j = 0; j < dvm2.Count; j++)
                {
                    string ChildMenuDesc = "";
                    string ChildMenuDescToolTip = "";
                    if (dvm2[j]["search_key"].ToString().Trim().Length > 2)
                    {
                        ChildMenuDesc = "<h6 class='v1'>" + dvm2[j]["search_key"].ToString().Trim() + "</h6>";
                        ChildMenuDescToolTip = dvm2[j]["search_key"].ToString().Trim();
                    }
                    if (dvm2[j]["web_action"].ToString().Trim().Length > 2)
                        strMenu.Append("<li><a href='" + dvm2[j]["web_action"].ToString().Trim() + "?STR=" + frm_qstr + "@" + dvm2[j]["ID"].ToString().Trim() + "' title='" + ChildMenuDescToolTip + "'  >" + dvm2[j]["TEXT"].ToString().Trim() + "</a> " + ChildMenuDesc + " </li>");

                    #region Filling 3rd Level / Child
                    //Fatching 3rd Level / Child
                    DataView dvm3 = new DataView(dt, "mlevel=3 and submenuid='" + dvm2[j]["submenuid"].ToString().Trim() + "'", "ID , form, submenuid", DataViewRowState.CurrentRows);
                    if (dvm3.Count > 0)
                    {
                        if (dvm2[j]["search_key"].ToString().Trim().Length > 2)
                        {
                            ChildMenuDesc = "<h6 class='v1'>" + dvm2[j]["search_key"].ToString().Trim() + "</h6>";
                            ChildMenuDescToolTip = dvm2[j]["search_key"].ToString().Trim();
                        }
                        strMenu.Append("<li ><a href='#' title='" + ChildMenuDescToolTip + "'>" + dvm2[j]["TEXT"].ToString().Trim() + "</a>" + ChildMenuDesc);
                        strMenu.Append("<div >");
                    }
                    for (int k = 0; k < dvm3.Count; k++)
                    {
                        string subMenuDesc = "";
                        string subMenuDescToolTip = "";
                        if (dvm3[k]["search_key"].ToString().Trim().Length > 2)
                        {
                            subMenuDesc = "<h6 class='v1'>" + dvm3[k]["search_key"].ToString().Trim() + "</h6>";
                            subMenuDescToolTip = dvm3[k]["search_key"].ToString().Trim();
                        }
                        if (dvm3[k]["web_action"].ToString().Trim().Length > 2)
                            strMenu.Append("<a href='" + dvm3[k]["web_action"].ToString().Trim() + "?STR=" + frm_qstr + "@" + dvm3[k]["id"].ToString().Trim() + "'   title='" + subMenuDescToolTip + "'>" + dvm3[k]["text"].ToString().Trim() + "</a>" + subMenuDesc);

                        #region Filling 4th Level / SubChild
                        {
                            //Fatching 4th Level / SubChild
                            DataView dvm4 = new DataView(dt, "mlevel=4 and submenuid='" + dvm3[k]["submenuid"].ToString().Trim() + "' AND PARAM='" + dvm3[k]["PARAM"].ToString().Trim() + "'", "ID , form, submenuid", DataViewRowState.CurrentRows);
                            if (dvm4.Count > 0)
                            {
                                strMenu.Append("<ul >");
                                if (dvm3[k]["search_key"].ToString().Trim().Length > 2)
                                {
                                    ChildMenuDesc = "<h6 class='v1'>" + dvm3[k]["search_key"].ToString().Trim() + "</h6>";
                                    ChildMenuDescToolTip = dvm3[k]["search_key"].ToString().Trim();
                                }
                                strMenu.Append("<li ><a href='#' title='" + ChildMenuDescToolTip + "'>" + dvm3[k]["TEXT"].ToString().Trim() + "</a>" + ChildMenuDesc);

                                strMenu.Append("<div style='overflow:scroll;'>");

                                //strMenu.Append("<div style='overflow:scroll'>");

                            }
                            for (int z = 0; z < dvm4.Count; z++)
                            {
                                if (dvm4[z]["search_key"].ToString().Trim().Length > 2)
                                {
                                    subMenuDesc = "<h6 class='v1'>" + dvm4[z]["search_key"].ToString().Trim() + "</h6>";
                                    subMenuDescToolTip = dvm4[z]["search_key"].ToString().Trim();
                                }
                                if (dvm4[z]["web_action"].ToString().Trim().Length > 2)
                                    strMenu.Append("<a href='" + dvm4[z]["web_action"].ToString().Trim() + "?STR=" + frm_qstr + "@" + dvm4[z]["id"].ToString().Trim() + "'   title='" + subMenuDescToolTip + "'>" + dvm4[z]["text"].ToString().Trim() + "</a>" + subMenuDesc);
                            }
                            if (dvm4.Count > 0)
                            {
                                strMenu.Append("</div>");
                                strMenu.Append("</li>");
                                strMenu.Append("</ul>");
                            }
                        }
                        #endregion
                    }
                    if (dvm3.Count > 0)
                    {
                        strMenu.Append("</div>");
                        strMenu.Append("</li>");
                    }
                    #endregion
                }
                strMenu.Append("</ul>");
                strMenu.Append("</div>");
                strMenu.Append("</li>");
                #endregion
            }

            strMenu.Append("</ul>");
            strMenu.Append("</nav>");
            #endregion

            html_tag = strMenu.ToString();
            Session["html_tag" + frm_qstr] = html_tag;
        }
        else html_tag = (string)Session["html_tag" + frm_qstr];
        cssmenu.Controls.Add(new LiteralControl(html_tag));
    }
}