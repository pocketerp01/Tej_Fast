using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Web.Configuration;

public partial class Fin_Master8 : System.Web.UI.MasterPage
{
    DataTable dt, dt1; string html_tag, byPass = "N", wAction = "";
    string Cstr, str, co_cd = "", mbr = "", hname, br_name, btnval, yr, val1, tab_name, mhd, uname, cond, squery, col2, bval, get_qstr, url, usr_dept, frm_formID, frm_ulevel;
    string frm_CDT1 = "", frm_CDT2 = "";
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Tejaxo";
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
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

                    co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");
                    yr = fgenMV.Fn_Get_Mvar(get_qstr, "U_FYEAR");
                    uname = fgenMV.Fn_Get_Mvar(get_qstr, "U_UNAME");
                    tab_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONTAB");
                    cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");
                    mbr = fgenMV.Fn_Get_Mvar(get_qstr, "U_MBR");
                    br_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_MBR_NAME");
                    usr_dept = fgenMV.Fn_Get_Mvar(get_qstr, "U_DEP_NAME");
                    frm_ulevel = fgenMV.Fn_Get_Mvar(get_qstr, "U_ULEVEL");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_CDT1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(get_qstr, "U_CDT2");

                    if (co_cd.Length <= 1) Response.Redirect("~/login.aspx");
                }
                else Response.Redirect("~/login.aspx");

                Page.Title = co_cd == "SRIS" ? "SRISOL ERP" : "Tejaxo";

                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                if (!this.IsPostBack)
                {
                    fgen.send_cookie("MY_VALS" + get_qstr, "U_COCD:" + co_cd + "~" + "U_FYEAR:" + yr + "~" + "U_MBR:" + mbr + "~" + "U_UNAME:" + uname + "~" + "U_ULEVEL:" + frm_ulevel + "~" + "U_CDT1:" + frm_CDT1 + "~" + "U_CDT2:" + frm_CDT2);

                    //Session["Reset"] = true;
                    if ((co_cd == "MEGH" && frm_formID == "F40116") || frm_ulevel == "0" || co_cd == "DREM") { }
                    else
                    {
                        int timeout = 5 * 1000 * 60;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);
                    }
                }
                if (hfWindowSize.Value.ToString() == "") hfWindowSize.Value = fgenMV.Fn_Get_Mvar(get_qstr, "FRMWINDOWSIZE");
                fgenMV.Fn_Set_Mvar(get_qstr, "FRMWINDOWSIZE", hfWindowSize.Value.ToString());
                Session["hfWindowSize"] = hfWindowSize.Value;
            }
            if (co_cd == "MEGH" && uname == "SANT2")
            {
                shortcut1.Visible = true;
                shortcut2.Visible = true;                
            }
            else
            {
                shortcut1.Visible = false;
                shortcut2.Visible = false;
            }
            if (!Page.IsPostBack)
            {
                if (co_cd == "" || co_cd == null) Response.Redirect("~/tej-base/signout.aspx");
                fill_val();
                byPass = fgenMV.Fn_Get_Mvar(get_qstr, "U_BYPASS");
                if (byPass == "Y" && frm_formID != null)
                {
                    if (frm_formID.Length > 2)
                    {
                        wAction = fgen.seek_iname(get_qstr, co_cd, "SELECT WEB_ACTION FROM FIN_MSYS WHERE ID='" + frm_formID + "'", "WEB_ACTION");
                        if (co_cd == "SVPL") wAction = "../tej-base/invn-reps.aspx";
                        if (wAction.Length > 2)
                        {
                            wAction = wAction + "?STR=" + get_qstr + "@" + frm_formID;
                            fgenMV.Fn_Set_Mvar(get_qstr, "U_BYPASS", "N");
                            fgenMV.Fn_Set_Mvar(get_qstr, "U_BYPASS1", "Y");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "window.location='" + wAction + "';", true);
                            return;
                        }
                    }
                }
            }
            create_menu();
        }
    }

    public void fill_val()
    {
        lblusername.Text = uname;
        lblusername1.Text = uname;
        lbluserept.Text = usr_dept;

        lblbrcode.Text = mbr.Trim();
        lblbrcode1.Text = mbr.Trim();

        lblbrname.Text = br_name;
        //lblbrname.Text = "PLANT 1";

        hname = fgenCO.chk_co(co_cd);
        lblbuilddt.Text = fgenMV.Fn_Get_Mvar(get_qstr, "U_EXETIME");
        lblHelpLine.InnerText = fgenMV.Fn_Get_Mvar(get_qstr, "U_HELPLINE");
        helpLin.Visible = false;
        if (fgenMV.Fn_Get_Mvar(get_qstr, "U_CLIENT_GRP") == "SG_TYPE")
            helpLin.Visible = true;
        lblserverIP.Text = fgenMV.Fn_Get_Mvar(get_qstr, "U_SERVERIP");
        if (lblbuilddt.Text == "0") lblbuilddt.Visible = false;
        lblbcode.Text = ": ";
        //if (hname.Contains("AKIT")) lblbcode.Text = ": ";
        //else lblbcode.Text = "(" + co_cd.Trim() + ")";
        lblBrHeader.Text = hname;
        lblyr.Text = yr;

        if (fgen.Make_date(frm_CDT1,"dd/MM/yyyy").ToString("yyyy") ==fgen.Make_date(frm_CDT2,"dd/MM/yyyy").ToString("yyyy"))
            lblyearS.Text = "(" + yr.Split('-')[0] + ")";
        else lblyearS.Text = "(" + yr + ")";
        //lblbr.Text = mbr;

        string logopath = "~/tej-base/images/finsysblue.jpg";
        if (co_cd == "SRIS")
        {
            logopath = "~/tej-base/images/sris_desktop.jpg";
        }
        imglogo.Src = logopath;

        string userDP = "~/tej-base/upload/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_DP_IMG");
        if (System.IO.File.Exists(MapPath(userDP)))
        {
            imgprofile.ImageUrl = userDP;
            Image1.ImageUrl = userDP;
        }

        string vardate = fgen.Fn_curr_dt(co_cd, get_qstr);
        fgenMV.Fn_Set_Mvar(get_qstr, "U_VARDATE", vardate);
    }

    public void create_menu()
    {
        string frm_cocd = "";
        string frm_qstr = get_qstr;
        string frm_tabname = "";
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
            strMenu.Append("<ul id='myUl'>");

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
                strMenu.Append("<a onmouseover='highLightMenu(this,myUl);' href='" + dvm1[i]["web_action"].ToString().Trim() + "' title='" + MenuDescToolTip + "'?STR=" + frm_qstr + "@" + dvm1[i]["id"].ToString().Trim() + ">");
                strMenu.Append("" + dvm1[i]["text"].ToString() + "");
                strMenu.Append("</a>" + MenuDesc);
                strMenu.Append("<div>");
                #endregion

                #region Filling 2nd Level / Father.....
                //Fatching 2nd Level / Father
                DataView dvm2 = new DataView(dt, "mlevel=2 and form='" + dvm1[i]["form"].ToString().Trim() + "'", "ID , form, submenuid", DataViewRowState.CurrentRows);
                strMenu.Append("<ul id='myUl2" + i + "'>");
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
                        strMenu.Append("<li><a onmouseover='highLightMenu(this,myUl2" + i + ");' href='" + dvm2[j]["web_action"].ToString().Trim() + "?STR=" + frm_qstr + "@" + dvm2[j]["ID"].ToString().Trim() + "' title='" + ChildMenuDescToolTip + "'  >" + dvm2[j]["TEXT"].ToString() + "</a> " + ChildMenuDesc + " </li>");

                    #region Filling 3rd Level / Child
                    //Fatching 3rd Level / Child
                    DataView dvm3 = new DataView(dt, "mlevel=3 and submenuid='" + dvm2[j]["submenuid"].ToString().Trim() + "'", "ID , form, submenuid", DataViewRowState.CurrentRows);

                    if (dvm3.Count > 0)
                    {
                        ChildMenuDesc = "";
                        ChildMenuDescToolTip = "";
                        if (dvm2[j]["search_key"].ToString().Trim().Length > 2)
                        {
                            ChildMenuDesc = "<h6 class='v1'>" + dvm2[j]["search_key"].ToString().Trim() + "</h6>";
                            ChildMenuDescToolTip = dvm2[j]["search_key"].ToString().Trim();
                        }
                        strMenu.Append("<li><a onmouseover='highLightMenu(this,myUl2" + i + ");' href='#' title='" + ChildMenuDescToolTip + "'>" + dvm2[j]["TEXT"].ToString() + "</a>" + ChildMenuDesc);
                        strMenu.Append("<div id='myDiv" + i + "_" + j + "'>");
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
                            strMenu.Append("<a onmouseover='highLightMenu(this,myDiv" + i + "_" + j + ");' href='" + dvm3[k]["web_action"].ToString().Trim() + "?STR=" + frm_qstr + "@" + dvm3[k]["id"].ToString().Trim() + "'   title='" + subMenuDescToolTip + "'>" + dvm3[k]["text"].ToString() + "</a>" + subMenuDesc);

                        #region Filling 4th Level / SubChild
                        {
                            //Fatching 4th Level / SubChild
                            //DataView dvm4 = new DataView(dt, "mlevel=4 and submenuid='" + dvm3[k]["submenuid"].ToString().Trim() + "' AND PARAM='" + dvm3[k]["PARAM"].ToString().Trim() + "'", "ID , form, submenuid ,Text", DataViewRowState.CurrentRows);
                            DataView dvm4 = new DataView(dt, "mlevel=4 and submenuid='" + dvm3[k]["submenuid"].ToString().Trim() + "' AND PARAM='" + dvm3[k]["PARAM"].ToString().Trim() + "'", "form, submenuid ,Text", DataViewRowState.CurrentRows);
                            if (dvm4.Count > 0)
                            {
                                strMenu.Append("<ul >");
                                ChildMenuDesc = "";
                                ChildMenuDescToolTip = "";

                                if (dvm3[k]["search_key"].ToString().Trim().Length > 2)
                                {
                                    ChildMenuDesc = "<h6 class='v1'>" + dvm3[k]["search_key"].ToString().Trim() + "</h6>";
                                    ChildMenuDescToolTip = dvm3[k]["search_key"].ToString().Trim();
                                }
                                strMenu.Append("<li ><a onmouseover='highLightMenu(this,myDiv" + i + "_" + j + ");' href='#' title='" + ChildMenuDescToolTip + "'>" + dvm3[k]["TEXT"].ToString() + "</a>" + ChildMenuDesc);

                                //strMenu.Append("<div style='overflow:scroll'>");
                                strMenu.Append("<div style='overflow:scroll;' >");
                            }
                            for (int z = 0; z < dvm4.Count; z++)
                            {
                                subMenuDesc = "";
                                subMenuDescToolTip = "";
                                if (dvm4[z]["search_key"].ToString().Trim().Length > 2)
                                {
                                    subMenuDesc = "<h6 class='v1'>" + dvm4[z]["search_key"].ToString().Trim() + "</h6>";
                                    subMenuDescToolTip = dvm4[z]["search_key"].ToString().Trim();
                                }
                                if (dvm4[z]["web_action"].ToString().Trim().Length > 2)
                                    strMenu.Append("<a href='" + dvm4[z]["web_action"].ToString().Trim() + "?STR=" + frm_qstr + "@" + dvm4[z]["id"].ToString().Trim() + "'   title='" + subMenuDescToolTip + "'>" + dvm4[z]["text"].ToString() + "</a>" + subMenuDesc);
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

    protected void save_selected_icon_id(object sender, EventArgs e)
    {

    }
    protected void btnlogout_ServerClick(object sender, EventArgs e)
    {
        url = HttpContext.Current.Request.Url.AbsoluteUri;
        if (url.Contains("STR"))
        {
            if (Request.QueryString["STR"].Length > 0)
            {
                get_qstr = Request.QueryString["STR"].Trim().ToString();
                if (get_qstr.Contains("@"))
                {
                    get_qstr = get_qstr.Split('@')[0].ToString();
                }
            }
        }
        Session["LOGOUT"] = get_qstr;
        hfval.Value = "LOGOUT";
        fgen.msg("-", "FMSG", "Are you sure! You want to Log Out");
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hfval.Value;
        switch (btnval)
        {
            case "LOGOUT":
                try
                {
                    btnval = Request.Cookies["REPLY"].Value.ToString().Trim();
                    if (btnval == "Y") Response.Redirect("~/tej-base/signout.aspx");
                }
                catch
                {
                    Response.Redirect("~/tej-base/signout.aspx");
                }
                break;
            case "ERP_MSG":

                return;
            case "YR_CHANGE":
                val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                if (val1.Length > 0) { }
                else return;

                dt = new DataTable();
                dt = fgen.GetYearDetails(get_qstr, co_cd, val1.Substring(0, 4));

                if (dt.Rows.Count > 0)
                {
                    fgenMV.Fn_Set_Mvar(get_qstr, "U_YEAR", val1.Substring(0, 4));
                    fgenMV.Fn_Set_Mvar(get_qstr, "U_FYEAR", dt.Rows[0]["fstr"].ToString().Trim());

                    fgenMV.Fn_Set_Mvar(get_qstr, "U_DATERANGE", " between to_date('" + dt.Rows[0]["cdt1"].ToString().Trim() + "','dd/mm/yyyy') and to_date('" + dt.Rows[0]["cdt2"].ToString().Trim() + "','dd/mm/yyyy')");
                    fgenMV.Fn_Set_Mvar(get_qstr, "U_CDT1", dt.Rows[0]["cdt1"].ToString().Trim());
                    fgenMV.Fn_Set_Mvar(get_qstr, "U_CDT2", dt.Rows[0]["cdt2"].ToString().Trim());
                }
                yr = val1;
                fill_val();
                Response.Redirect("~/tej-base/desktop.aspx?STR=" + get_qstr, false);
                break;
            case "BR_CHANGE":
                val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                if (val1.Length > 0) { }
                else return;
                fgen.send_cookie("BRANCH", val1);
                lblbrcode.Text = val1;
                string branch_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                fgenMV.Fn_Set_Mvar(get_qstr, "U_MBR", val1);
                fgenMV.Fn_Set_Mvar(get_qstr, "U_MBR_NAME", branch_name);


                string ind_Ptype = "";
                ind_Ptype = fgen.seek_iname(get_qstr, co_cd.ToUpper().Trim(), "select trim(upper(opt_param)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + mbr + "' and OPT_ID='W1000'", "fstr");
                if (ind_Ptype != "-")
                {
                    fgenMV.Fn_Set_Mvar(get_qstr, "U_IND_PTYPE", ind_Ptype);
                }
                fill_val();
                Response.Redirect("~/tej-base/desktop.aspx?STR=" + get_qstr, false);
                break;
            case "OPEN_ICON":
                val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                if (val1 == "EXIT") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OnlyClose();", true);
                else
                {
                    if (val1.Length > 0) { }
                    else return;
                    fgenMV.Fn_Set_Mvar(get_qstr, "U_FORMID", val1);
                    Response.Redirect("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
                    //Server.Transfer("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
                }
                break;
            case "BOOKMARK":
                if (fgenMV.Fn_Get_Mvar(get_qstr, "U_COL5").ToString().Trim().Replace("&amp", "").Length > 3)
                {
                    val1 = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                    if (val1 == "EXIT") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OnlyClose();", true);
                    else
                    {
                        fgenMV.Fn_Set_Mvar(get_qstr, "U_FORMID", val1);
                        Response.Redirect("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
                        //Server.Transfer("~/tej-base/" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL2").ToString().Trim().Replace("&amp", "").ToLower() + "?STR=" + get_qstr + "@" + val1, false);
                    }
                }
                else
                    bookMarkMenu(fgenMV.Fn_Get_Mvar(get_qstr, "U_COL3").ToString().Trim());
                break;
        }
    }
    protected void btnyear_ServerClick(object sender, EventArgs e)
    {
        hfval.Value = "YR_CHANGE";
        fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", "YR");
        fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", "select to_char(fmdate,'yyyy')||'-'||to_char(todate,'yyyy') as fstr, 'Change To' as Change_to,to_char(fmdate,'dd/mm/yyyy') as from_date,to_char(todate,'dd/mm/yyyy') as To_Date from co where upper(Trim(code)) like '" + co_cd + "%' order by fstr desc");
        fgen.Fn_open_sseek("Select Year", get_qstr);
    }
    protected void btnMsgs_ServerClick(object sender, EventArgs e)
    {
        hfval.Value = "ERP_MSG";
        string mUsrcode = fgen.seek_iname(get_qstr, co_cd, "select userid from evas where username='" + uname + "'", "userid");
        fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", "select vchnum as Vdd,MsgFrom,MsgTxt,Terminal as Origin_Dtl,to_char(Vchdate,'dd/mm/yyyy') as Msg_Dtd from Mailbox2 where Msgto='" + mUsrcode + "' order by Vdd desc ");
        fgen.Fn_open_sseek("Recent Messages for " + uname, get_qstr);
    }

    protected void btnbranch_ServerClick(object sender, EventArgs e)
    {
        hfval.Value = "BR_CHANGE"; val1 = "-";
        dt1 = new DataTable();
        dt1 = fgen.getdata(get_qstr, co_cd, "select allowbr from evas where trim(upper(username))='" + lblusername.Text.ToUpper() + "'");
        if (dt1.Rows.Count > 0) val1 = dt1.Rows[0]["allowbr"].ToString().Trim();
        if (val1 == "-" || val1 == null || val1.Length == 0) squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode from type where id='B' order by type1";
        else
        {
            dt = new DataTable();
            dt = fgen.getdata(get_qstr, co_cd, "select distinct type1 from type where id='B'");
            col2 = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (val1.Contains(dr["type1"].ToString().Trim()))
                {
                    if (col2.Length > 0)
                    {
                        col2 = col2 + "," + "'" + dr["type1"].ToString().Trim() + "'";
                    }
                    else
                    {
                        col2 = "'" + dr["type1"].ToString().Trim() + "'";
                    }
                }
            }
            squery = "select distinct type1 as fstr,type1 as Code,name as Branch_name,addr||','||addr1||','||addr2 as Address,acode from type where id='B' and type1 in (" + col2 + ") order by type1";
        }
        fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", "YR");
        fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", squery);
        fgen.Fn_open_sseek("Select Your Branch", get_qstr);
    }
    protected void btnquickmenu_ServerClick(object sender, EventArgs e)
    {
        hfval.Value = "OPEN_ICON";

        tab_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONTAB");

        co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");

        cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");

        if (cond.Length > 2) cond = " and " + cond;
        //*************
        url = HttpContext.Current.Request.Url.AbsoluteUri;
        squery = "select distinct trim(id) as fstr,web_Action ,trim(text) as Text,SEARCH_KEY ,trim(id) as id from FIN_MSYS where trim(nvl(web_Action,'-'))!='-' and trim(id) in (select trim(id) from " + tab_name + " where 1=1 " + cond + " ) and NVL(VISI,'Y')!='N' order by trim(text),trim(id)";
        fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", "OPEN_ICON");
        fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", squery);
        cond = "../tej-base/open_icon.aspx";

        //string heightper = "70%";
        //string widthper = "80%";
        //if (fgenMV.Fn_Get_Mvar(get_qstr, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(get_qstr, "FRMWINDOWSIZE").toDouble() < 800)
        //{
        //    heightper = "95%";
        //    widthper = "95%";
        //}

        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + cond + "?STR=" + get_qstr + "','" + widthper + "','" + heightper + "','Finsys Quick Menu');", true);

        fgen.Fn_open_icon("ERP Quick Menu", get_qstr);
    }
    protected void btncomm_ServerClick(object sender, EventArgs e)
    {
        hfval.Value = "OPEN_ICON";

        tab_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONTAB");

        co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");

        cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");

        if (cond.Length > 2) cond = " and " + cond;
        //*************
        url = HttpContext.Current.Request.Url.AbsoluteUri;
        squery = "select distinct trim(id) as fstr,web_Action ,trim(text) as Text,SEARCH_KEY ,trim(id) as id from FIN_MSYS where trim(nvl(web_Action,'-'))!='-' and trim(id) in (select trim(id) from " + tab_name + " where 1=1 " + cond + " ) and NVL(VISI,'Y')!='N' and NVL(BNR,'N')='Y' order by trim(id),trim(text)";
        fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", "OPEN_ICON");
        fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", squery);
        cond = "../tej-base/open_icon.aspx";
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + cond + "?STR=" + get_qstr + "','80%','70%','Finsys Quick Menu');", true);
        fgen.Fn_open_icon("ERP Quick Menu", get_qstr);
    }

    protected void txtsrch_TextChanged(object sender, EventArgs e)
    {
        string css = "sidebar-mini";
        if (txtsrch.Text.Trim().Length <= 0) return;

        cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");
        squery = "select * from FIN_MSYS where trim(nvl(web_Action,'-'))!='-' and trim(id) in (select trim(id) from " + tab_name + " where 1=1 " + cond + " ) and NVL(VISI,'Y')!='N' order by trim(id),trim(text)";
        css = "sidebar-mini ";
        dt = new DataTable();
        dt = fgen.search_vip1(get_qstr, co_cd, squery, txtsrch.Text.Trim(), dt);

        string color = "skin-blue";
        //if (tab_name.ToUpper() == "FIN_MRSYS") color = dt.Rows[0]["user_color"].ToString().Trim();
        //body1.Attributes.Add("class", "hold-transition " + color + " sidebar-mini sidebar-collapse");
        body1.Attributes.Add("class", "" + css + " " + color + "");
        System.Text.StringBuilder strB = new System.Text.StringBuilder();

        #region Get the Distinct Form ID's
        var res = from FIN_MSYS in dt.AsEnumerable()
                  select new
                  {
                      WebForm_id = FIN_MSYS.Field<string>("id"),
                      WebForm_Text = FIN_MSYS.Field<string>("text"),
                      SubMenuID = FIN_MSYS.Field<string>("Submenuid"),
                      Web_Action = FIN_MSYS.Field<string>("web_action"),
                      fin_rsys_Form = FIN_MSYS.Field<string>("Form"),
                      fin_rsys_Param = FIN_MSYS.Field<string>("Param"),
                      fin_rsys_CSS = FIN_MSYS.Field<string>("CSS"),
                      fin_rsys_mlevel = FIN_MSYS.Field<decimal>("mlevel")
                  };
        #endregion

        strB.Append("<ul id='myul' class='sidebar-menu'>");
        foreach (var r in res)
        {
            #region Fiiling 2nd Level / Child
            var result = from g in dt.AsEnumerable()
                         where g.Field<string>("form") == r.fin_rsys_Form
                         select new
                         {
                             Child_FormID = g.Field<string>("id"),
                             Child_FormName = g.Field<string>("text"),
                             Child_fin_rsys_Form = g.Field<string>("form"),
                             Child_SubMenu = g.Field<string>("SUBMENUID"),
                             Child_WebAction = g.Field<string>("web_action"),
                             Child_mlevel = g.Field<decimal>("mlevel")
                         };
            #endregion

            #region Create 1st Level Icons / Grand Father
            strB.Append("<li class='treeview'>");
            strB.Append("<a href='" + r.Web_Action + "'?STR=" + get_qstr + "@" + r.WebForm_id + "> <i class='fa " + r.fin_rsys_CSS + "'></i>");
            strB.Append("<span>" + r.WebForm_Text + "</span>");
            strB.Append(" <i class='fa fa-angle-left pull-right'></i> </a>");
            #endregion

            #region Create 2nd Level / Father and 3rd Level / Child Icons
            strB.Append("<ul class='treeview-menu'>");
            foreach (var data in result)
            {
                if (data.Child_WebAction.Length > 2)
                    strB.Append("<li class='treeview'><a href='" + data.Child_WebAction + "?STR=" + get_qstr + "@" + data.Child_FormID + "' ><i class='fa fa-circle-o'></i> <span>" + data.Child_FormName + "</span>  </a></li>");

                #region Filling 3rd Level / Child
                var result1 = from v in dt.AsEnumerable()
                              where v.Field<string>("submenuid") == data.Child_SubMenu && v.Field<decimal>("mlevel") == 3
                              select new
                              {
                                  SubChild_FormID = v.Field<string>("id"),
                                  SubChild_FormName = v.Field<string>("text"),
                                  SubChild_SubMenu = v.Field<string>("SUBMENUID"),
                                  SubChild_WebAction = v.Field<string>("web_action"),
                                  SubChild_mlevel = v.Field<decimal>("mlevel")
                              };

                strB.Append("<li>");

                if (data.Child_mlevel >= 2)
                {
                    foreach (var data2 in result1)
                    {
                        strB.Append("<a href='#'><i class='fa fa-circle-o'></i> <span>" + data.Child_FormName + "</span><i class='fa fa-angle-left pull-right'></i></a>");
                        if (data.Child_SubMenu == data2.SubChild_SubMenu)
                            strB.Append("<ul class='treeview-menu'>");
                        break;
                    }
                    foreach (var data2 in result1)
                    {
                        if (data.Child_SubMenu == data2.SubChild_SubMenu)
                        {
                            strB.Append("<li ><a href='" + data2.SubChild_WebAction + "?STR=" + get_qstr + "@" + data2.SubChild_FormID + "'><i class='fa fa-circle-o'></i><span>" + data2.SubChild_FormName + "</span></a></li>");
                        }
                    }
                    foreach (var data2 in result1)
                    {
                        if (data.Child_SubMenu == data2.SubChild_SubMenu)
                            strB.Append("</ul>");
                        break;
                    }
                }

                strB.Append("</li>");
                #endregion
            }
            strB.Append("</ul>");
            strB.Append("</li>");

            #endregion
        }
        strB.Append("</ul>");

        //cssmenu.Controls.Add(new LiteralControl(html_tag));
    }
    protected void btnRefresh_ServerClick(object sender, EventArgs e)
    {
        string landingPage = (fgenMV.Fn_Get_Mvar(get_qstr, "U_ULEVEL") == "M") ? "desktop_cv" : "desktop_wt";
        fgenMV.Fn_Set_Mvar(get_qstr, "FS_LOG", "Y");
        Response.Redirect("~/tej-base/" + landingPage + ".aspx?STR=" + get_qstr);
    }
    protected void btnLanguage_ServerClick(object sender, EventArgs e)
    {
        if (fgenMV.Fn_Get_Mvar(get_qstr, "U_LNG") == "2")
        {
            fgenMV.Fn_Set_Mvar(get_qstr, "U_LNG", "1");
            fgenMV.Fn_Set_Mvar(get_qstr, "U_SYS_COM_QRY", "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND,NVL(OBJ_CAPTION_REG,'-') AS OBJ_CAPTION_REG FROM SYS_CONFIG ");
        }
        else
        {
            fgenMV.Fn_Set_Mvar(get_qstr, "U_LNG", "2");
            fgenMV.Fn_Set_Mvar(get_qstr, "U_SYS_COM_QRY", "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND,NVL(OBJ_CAPTION,'-') AS OBJ_CAPTION_REG FROM SYS_CONFIG ");
        }
        string landingPage = (fgenMV.Fn_Get_Mvar(get_qstr, "U_ULEVEL") == "M") ? "desktop_cv" : "desktop_wt";
        fgenMV.Fn_Set_Mvar(get_qstr, "FS_LOG", "Y");
        Response.Redirect("~/tej-base/" + landingPage + ".aspx?STR=" + get_qstr);
    }
    protected void btnopenmenu_ServerClick(object sender, EventArgs e)
    {
        hfval.Value = "OPEN_ICON";

        tab_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONTAB");

        co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");

        cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");

        if (cond.Length > 2) cond = " and " + cond;
        //*************
        url = HttpContext.Current.Request.Url.AbsoluteUri;
        squery = "select distinct trim(id) as fstr,web_Action ,trim(text) as Text,SEARCH_KEY ,trim(id) as id from FIN_MSYS where trim(nvl(web_Action,'-'))!='-' and trim(id) in (select trim(id) from " + tab_name + " where 1=1 " + cond + " ) and NVL(VISI,'Y')!='N' order by trim(text),trim(id)";

        dt = new DataTable();
        dt1 = new DataTable();
        dt1 = fgen.search_vip1(get_qstr, co_cd, squery, hdsrchTxt.Text.Trim().ToUpper(), dt);

        Session["send_icondt"] = dt1;
        fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", "OPEN_ICON");
        fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", "");
        cond = "../tej-base/open_icon.aspx";
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + cond + "?STR=" + get_qstr + "','80%','70%','Finsys Quick Menu');", true);
        fgen.Fn_open_icon("ERP Quick Menu", get_qstr);
    }
    protected void btnBookmark_ServerClick(object sender, EventArgs e)
    {
        hfval.Value = "BOOKMARK";
        bookMarkMenu("");
    }
    /// <summary>
    /// 09/04/2020 -- VV
    /// New Bookmark menu added in Master page, it will show siblings of current menu, last opened menu path.
    /// </summary>
    /// <param name="selIL"></param>
    void bookMarkMenu(string selIL)
    {
        tab_name = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONTAB");
        co_cd = fgenMV.Fn_Get_Mvar(get_qstr, "U_COCD");
        cond = fgenMV.Fn_Get_Mvar(get_qstr, "U_ICONCOND");
        if (cond.Length > 2) cond = " and " + cond;
        if (Session["dt_menu" + get_qstr] == null)
        {
            dt = fgen.fill_icon_grid(co_cd, tab_name, cond, get_qstr);
        }
        else dt = (DataTable)Session["dt_menu" + get_qstr];

        if (frm_formID == "" || frm_formID == null)
            frm_formID = fgenMV.Fn_Get_Mvar(get_qstr, "U_FORMID");

        int iconLevel = 0;
        string s_f = "SUBMENUID";
        string submenuid = "", param = "";
        string header = "";
        if (selIL == "")
        {
            iconLevel = fgen.make_int(fgen.seek_iname_dt(dt, "ID='" + frm_formID + "'", "MLEVEL"));
            submenuid = fgen.seek_iname_dt(dt, "ID='" + frm_formID + "'", "SUBMENUID");
            if (iconLevel > 3) param = fgen.seek_iname_dt(dt, "ID='" + frm_formID + "'", "PARAM");
            header = fgen.seek_iname_dt(dt, "SUBMENUID='" + submenuid + "' AND MLEVEL='" + (iconLevel - 1) + "' ", "TEXT");
            if (iconLevel == 0)
            {
                s_f = "PARAM";
                submenuid = "-";
                iconLevel = 1;
                header = "Menu";
            }
        }
        else
        {
            submenuid = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL3");
            iconLevel = fgen.make_int(fgenMV.Fn_Get_Mvar(get_qstr, "U_COL4"));
            switch (iconLevel)
            {
                case 2:
                    submenuid = fgen.seek_iname_dt(dt, "SUBMENUID='" + submenuid + "' AND MLEVEL='" + iconLevel + "' ", "FORM");
                    s_f = "FORM";
                    if (submenuid == "0") submenuid = fgenMV.Fn_Get_Mvar(get_qstr, "U_COL3");
                    submenuid = fgen.seek_iname_dt(dt, "FORM='" + submenuid + "' AND MLEVEL='" + iconLevel + "' ", "FORM");
                    header = fgen.seek_iname_dt(dt, "FORM='" + submenuid + "' AND MLEVEL='" + "1" + "' ", "TEXT");
                    break;
                case 1:
                    s_f = "PARAM";
                    submenuid = "-";
                    header = "Menu";
                    break;
                default:
                    submenuid = fgen.seek_iname_dt(dt, "SUBMENUID='" + submenuid + "' AND MLEVEL='" + iconLevel + "' ", "SUBMENUID");
                    header = fgen.seek_iname_dt(dt, "SUBMENUID='" + submenuid + "' AND MLEVEL='" + (iconLevel - 1) + "' ", "TEXT");
                    if (iconLevel > 3) param = fgen.seek_iname_dt(dt, "ID='" + fgenMV.Fn_Get_Mvar(get_qstr, "U_COL1") + "'", "PARAM");
                    break;
            }
        }

        fgenMV.Fn_Set_Mvar(get_qstr, "U_COL3", submenuid);
        fgenMV.Fn_Set_Mvar(get_qstr, "U_COL4", iconLevel.ToString());

        DataView vd = new DataView();
        if (param == "")
            vd = new DataView(dt, "" + s_f + "='" + submenuid + "' AND MLEVEL='" + iconLevel + "' ", "", DataViewRowState.CurrentRows);
        else
            vd = new DataView(dt, "" + s_f + "='" + submenuid + "' AND PARAM='" + param + "' AND MLEVEL='" + iconLevel + "' ", "", DataViewRowState.CurrentRows);
        dt1 = new DataTable();
        dt1 = vd.ToTable();
        Session["send_icondt"] = dt1;
        fgenMV.Fn_Set_Mvar(get_qstr, "U_XID", hfval.Value);
        fgenMV.Fn_Set_Mvar(get_qstr, "U_SEEKSQL", "");
        //cond = "../tej-base/open_icon.aspx";        
        fgen.Fn_open_icon(header + "(Level: " + iconLevel + ")", get_qstr);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + cond + "?STR=" + get_qstr + "','80%','70%','" + header + " (Level : " + iconLevel + ")');", true);
    }
    protected void btnMsg_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/om_sms_msg.aspx?STR=" + get_qstr);
    }
    protected void btnShort1_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/om_appr.aspx?STR=" + get_qstr + "@F70201");
    }
    protected void btnShort2_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/om_appr.aspx?STR=" + get_qstr + "@F70203");
    }
}