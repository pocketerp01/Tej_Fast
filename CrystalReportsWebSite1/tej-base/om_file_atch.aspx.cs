using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;


public partial class om_file_atch : System.Web.UI.Page
{
    DataTable dt, sg1_dt = new DataTable();
    DataRow oporow, sg1_dr; DataSet oDS;
    string Checked_ok;
    string save_it;
    string vchnum, query, btnmode, daterange, SQuery1, col1, col2, ulevel, vardate, mlvl, mq1, DRID, typePopup = "N";
    string tco_cd, custom_filing_no, co_cd, cdt1, cdt2, scode, sname, seek, entby, edt, headername, xmlfile;
    string uright, can_add, can_edit, can_del, acessuser, filePath, SQuery;
    string fName, fpath, filename, mypath, compnay_code, extension;
    string sendtoemail, subject, xmltag, mailpath, mailport, branchname, col3, col4, mailmsg, mflag;
    int i, z = 0, srno, filesrno;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query, btnval;
    string frm_mbr, frm_vty, frm_vnum, frm_url, fromdt, todt, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    int ssl, port;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
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

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");

                set_Val();
                if (!Page.IsPostBack)
                {
                    create_tab();
                    ViewState["filesrno"] = 0;
                    openOldEntry();
                }
            }
        }
    }
    public void set_Val()
    {
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Length > 2)
        {
            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");

            string m_doc = "";
            m_doc = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR");

            string m_case = "3";

            if (m_doc.Contains("FAM"))
            {
                m_case = "1";
            }
            if (m_doc.Contains("ITM"))
            {
                m_case = "2";
            }
            if (m_doc.Contains("PCONT") || m_doc.Contains("SCONT") || m_doc.Contains("CONSG"))
            {
                m_case = "3";
            }
            switch (m_case)
            {
                case "1":
                    txtdocno.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(0, 7);
                    frm_vnum = txtdocno.Text;
                    txtdate.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(7, 10);

                    break;
                case "2":
                    txtdocno.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(0, 8);
                    frm_vnum = txtdocno.Text;
                    txtdate.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(8, 10);
                    break;
                case "3":
                    txtdocno.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(0, 6);
                    frm_vnum = txtdocno.Text;
                    txtdate.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(6, 10);
                    break;
            }
            lblHeading.Text = "Type : " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY") + " Entry No/Date : " + txtdocno.Text + " " + txtdate.Text;

        }
        lblheader.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_HEADER");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_FILE_ATCH";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        //typePopup = "N";     
    }
    void openOldEntry()
    {
        dt = new DataTable();

        //

        string m_doc = "";
        string chk_branch = "";
        chk_branch = frm_mbr;
        m_doc = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR");
        if (m_doc.Contains("FAM"))
        {
            m_doc = m_doc.ToString().Trim().Replace("FAM", "");
            chk_branch = "00";
        }
        if (m_doc.Contains("ITM"))
        {
            m_doc = m_doc.ToString().Trim().Replace("ITM", "");
            chk_branch = "00";
        }
        if (m_doc.Contains("PCONT"))
        {
            m_doc = m_doc.ToString().Trim().Replace("PCONT", "");
            chk_branch = "00";
        }
        if (m_doc.Contains("CONSG"))
        {
            m_doc = m_doc.ToString().Trim().Replace("CONSG", "");
            chk_branch = "00";
        }

        if (m_doc.Contains("SCONT"))
        {
            m_doc = m_doc.ToString().Trim().Replace("SCONT", "");
            chk_branch = "00";
        }

        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.* FROM " + frm_tabname + " A WHERE a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + chk_branch  + frm_vty + m_doc + "' order by a.srno");
        if (dt.Rows.Count > 0)
        {
            edmode.Value = "Y";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                sg1_dr["filename"] = dt.Rows[i]["file_name"].ToString();
                sg1_dr["fileorgname"] = dt.Rows[i]["FILE_ORIG_NAME"].ToString();
                sg1_dr["remarks"] = dt.Rows[i]["remarks"].ToString();
                sg1_dt.Rows.Add(sg1_dr);
            }
            txtdocno.Text = dt.Rows[0]["vchnum"].ToString();
            txtdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString()).ToString("dd/MM/yyyy");
            ViewState["ent_by"] = dt.Rows[0]["ent_by"].ToString();
            ViewState["ent_dt"] = dt.Rows[0]["ent_dt"].ToString();
            ViewState["sg1"] = sg1_dt;
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
        }
    }
    //===============================================
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        //--
        string CP_BTN;
        CP_BTN = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_FROM");
        string CP_HF1;
        CP_HF1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_HF1");
        hf1.Value = CP_HF1;
        if (CP_BTN.Trim().Length > 1)
        {
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3")
            {
                btnval = CP_BTN;
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "0");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "0");
        //--
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.VCHNUM)||to_char(a." + "VCHDATE" + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                fgen.ResetForm(this.Controls);
            }
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            switch (btnval)
            {
                case "SG1_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        DataTable sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        int z = dt.Rows.Count;
                        sg1_dt = dt.Clone();
                        DataRow sg1_dr = null;
                        int i = 0;
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["srno"] = sg1.Rows[i].Cells[1].Text.Trim();
                            sg1_dr["filename"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["fileorgname"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["remarks"] = ((TextBox)sg1.Rows[i].FindControl("txtRmk")).Text;
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                    }
                    #endregion
                    break;
            }
        }
    }
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        sg1_dt.Columns.Add(new DataColumn("srno", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("filename", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("fileorgname", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("remarks", typeof(string)));
        ViewState["sg1"] = sg1_dt;
    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt != null)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dt.Rows.Add(sg1_dr);
        }
    }

    public void fill_grid()
    {
        if (ViewState["sg1"] != null)
        {
            dt = new DataTable();
            sg1_dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            sg1_dt = dt.Clone();
            sg1_dr = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString().Length > 1)
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                    sg1_dr["filename"] = sg1.Rows[i].Cells[4].Text.Trim();
                    sg1_dr["fileorgname"] = sg1.Rows[i].Cells[5].Text.Trim();
                    sg1_dr["remarks"] = ((TextBox)sg1.Rows[i].FindControl("txtRmk")).Text;
                    sg1_dt.Rows.Add(sg1_dr);
                }
            }
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["srno"] = dt.Rows.Count + 1;
            sg1_dr["filename"] = filename;
            sg1_dr["fileorgname"] = txtAttch.Text.Trim().Length > 80 ? txtAttch.Text.Trim().Substring(0, 80) : txtAttch.Text.Trim();
            sg1_dt.Rows.Add(sg1_dr);
        }
        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        {
            Checked_ok = "Y";
            //-----------------------------            
            i = 0;
            hffield.Value = "";
            string chk_branch = "";
            chk_branch = frm_mbr;

            string m_doc = "";
            m_doc = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR");
            if (m_doc.Contains("FAM"))
            {
                m_doc = m_doc.ToString().Trim().Replace("FAM", "");
                chk_branch = "00";
            }
            if (m_doc.Contains("ITM"))
            {
                m_doc = m_doc.ToString().Trim().Replace("ITM", "");
                chk_branch = "00";
            }
            if (m_doc.Contains("PCONT"))
            {
                m_doc = m_doc.ToString().Trim().Replace("PCONT", "");
                chk_branch = "00";
            }
            if (m_doc.Contains("CONSG"))
            {
                m_doc = m_doc.ToString().Trim().Replace("CONSG", "");
                chk_branch = "00";
            }

            if (m_doc.Contains("SCONT"))
            {
                m_doc = m_doc.ToString().Trim().Replace("SCONT", "");
                chk_branch = "00";
            }

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "N")
            {
                btnsave.Disabled = false;
            }
            else
            {
                if (Checked_ok == "Y")
                {
                    try
                    {

                        frm_vnum = txtdocno.Text.Trim();
                        //if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(VCHNUM)||to_char(" + "VCHDATE" + ",'dd/mm/yyyy')='" + chk_branch  +frm_vty + m_doc + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = chk_branch ;
                            oporow["TYPE"] = frm_vty;
                            oporow["vchnum"] = frm_vnum;
                            oporow["vchdate"] = txtdate.Text;
                            oporow["srno"] = (i + 1);
                            oporow["file_path"] = sg1.Rows[i].Cells[4].Text;
                            oporow["file_name"] = sg1.Rows[i].Cells[4].Text;
                            oporow["FILE_ORIG_NAME"] = sg1.Rows[i].Cells[5].Text;
                            oporow["remarks"] = ((TextBox)sg1.Rows[i].FindControl("txtRmk")).Text;
                            if (edmode.Value == "Y")
                            {
                                oporow["ent_by"] = ViewState["ent_by"].ToString().Trim();
                                oporow["ent_dt"] = ViewState["ent_dt"].ToString().Trim();
                                oporow["edt_by"] = frm_uname;
                                oporow["edt_dt"] = vardate;
                            }
                            else
                            {
                                oporow["ent_by"] = frm_uname;
                                oporow["ent_dt"] = vardate;
                                oporow["edt_by"] = "-";
                                oporow["edt_dt"] = vardate;
                            }
                            oDS.Tables[0].Rows.Add(oporow);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        //if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtdocno.Text + " Saved Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(VCHNUM)||to_char(" + "VCHDATE" + ",'dd/mm/yyyy')='DD" + frm_vty + m_doc + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtdate.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls);
                        sg1_dt = new DataTable();
                        create_tab();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg1"] = null;
                        ViewState["filesrno"] = 0;
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
                    }
            #endregion
                }
            }
        }
    }

    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[3].Width = 30;
        }
    }
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";
        filepath = Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            string ext = System.IO.Path.GetExtension(Attch.FileName).ToLower();
            txtAttch.Text = Attch.FileName;
            if (ViewState["filesrno"] != null) filesrno = (int)ViewState["filesrno"];

            filepath = frm_cocd + "_" + frm_formID + "_" + frm_mbr + "_" + frm_vty + "_" + txtdocno.Text.Trim() + "_" + txtdate.Text.Replace(@"/", "_") + "_File_" + (filesrno + 1);
            filename = filepath + ext;
            Attch.PostedFile.SaveAs(@"c:\TEJ_ERP\UPLOAD\" + filename);

            filepath = Server.MapPath("~/tej-base/UPLOAD/") + filename;
            Attch.PostedFile.SaveAs(filepath);
            fill_grid();
            filesrno++;
            ViewState["filesrno"] = filesrno;
        }
        else
        {
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        if (txtdocno.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "Rmv":
                if (Convert.ToDateTime(txtdate.Text) < DateTime.Now && edmode.Value == "Y" && frm_ulvl.toDouble() > 1)
                {
                    fgen.msg("-", "AMSG", "File Removing not allowed, Please Contact to admin!!");
                    return;
                }
                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;

            case "Dwl":
                if (e.CommandArgument.ToString().Trim() != "")
                {
                    try
                    {
                        filePath = sg1.Rows[index].Cells[4].Text;

                        Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                        Session["FileName"] = sg1.Rows[index].Cells[4].Text;
                        Response.Write("<script>");
                        Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                        Response.Write("</script>");
                    }
                    catch { }
                }
                break;
            case "View":
                filePath = sg1.Rows[index].Cells[4].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','');", true);
                break;
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
    }
}