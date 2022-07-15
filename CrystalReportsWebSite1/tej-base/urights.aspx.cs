using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;


public partial class urights : System.Web.UI.Page
{
    string btnval, squery, uname, col1, col2, col3, mbr, cstr, mhd = "", str1 = "", str2 = "";
    DataTable dt;
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    string vardate, fromdt, todt, year, ulvl, header, grc_Dt;
    int dhd = 0;
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            btnnew.Focus();

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
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }
            else Response.Redirect("~/login.aspx");

            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                try
                {
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "USER_COLOR"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add USER_COLOR VARCHAR(10) DEFAULT '414246'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "IDESC"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add IDESC VARCHAR(50) DEFAULT '-'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "RCAN_EDIT"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add RCAN_eDIT CHAR(1) DEFAULT 'Y'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "RCAN_ADD"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add RCAN_aDD CHAR(1) DEFAULT 'Y'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "RCAN_DEL"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add RCAN_DEL CHAR(1) DEFAULT 'Y'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "RCAN_PRN"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add RCAN_PRN CHAR(1) DEFAULT 'Y'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "BRN"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add BRN CHAR(1) DEFAULT '-'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "PRD"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add PRD CHAR(1) DEFAULT '-'");
                    mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "VISI"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS add VISI CHAR(1) DEFAULT '-'");
                }
                catch { }
            }
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; //btnsrch.Enabled = false;
        btnext.Text = " Exit "; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnuserid.Enabled = false; btnprint.Disabled = false;
        btnext.AccessKey = "X";
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; //btnsrch.Enabled = true;
        btnext.Text = "Cancel"; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnuserid.Enabled = true; btnprint.Disabled = false;
        btnext.AccessKey = "C";
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        fgen.EnableForm(this.Controls); disablectrl(); btnuserid.Focus();
        ViewState["sg1"] = null;
        hffield.Value = "New";
        fgen.msg("-", "CMSG", "Do you want to copy the icons from other user");
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Edit";
        disp_data();
        fgen.Fn_open_sseek("Select UserName", frm_qstr);
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        if (sg1.Rows.Count > 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        else fgen.msg("-", "AMSG", "Please Check any one node to allocate rights");
    }
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        disp_data();
        fgen.Fn_open_sseek("Select UserName", frm_qstr);
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Del";
        disp_data();
        fgen.Fn_open_sseek("Select UserName", frm_qstr);
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from FIN_MRSYS where trim(userid)='" + edmode.Value + "'");
                fgen.msg("-", "AMSG", "Details are deleted for userid " + edmode.Value + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "New")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                hffield.Value = "New_E";
                disp_data();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
        }
        else
        {
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") != null || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") != null || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") != null)
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");

                col1 = col1.Replace("&amp", "");
                col2 = col2.Replace("&amp", "");
                col3 = col3.Replace("&amp", "");

                switch (btnval)
                {
                    case "UID":
                        if (col1.Length > 0) { }
                        else return;
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT * FROM (select userid from FIN_MRSYS where userid='" + col1 + "') WHERE ROWNUM<3", "userid");
                        if (mhd != "0") { fgen.msg("-", "AMSG", "User Rights are already given to " + col2 + "'13'Please edit the User id"); btnuserid.Focus(); }
                        else { txtuserid.Value = col1; txtusername.Value = col2; }
                        btnSelectIcon.Focus();
                        break;
                    case "T_FORM":
                        if (col1.Length <= 0) return;
                        hfcode.Value = "'" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") + "'";
                        fillGridView(hfcode.Value);
                        hfcode.Value = col1;
                        hffield.Value = "2ND";
                        disp_data();
                        fgen.Fn_open_sseek("Select Menu 2nd Level", frm_qstr);
                        break;
                    case "2ND":
                        if (col1.Length <= 0) return;
                        hfcode.Value = "'" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") + "'";
                        fillGridView(hfcode.Value);
                        hfcode.Value = col1;
                        hffield.Value = "3RD";
                        disp_data();
                        fgen.Fn_open_mseek("Select Menu 3rd Level", frm_qstr);
                        break;
                    case "3RD":
                        if (col1.Length <= 0) return;
                        hfcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                        fillGridView(hfcode.Value);
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT * FROM (SELECT DISTINCT ID FROM FIN_MSYS WHERE VISI='Y' AND MLEVEL='4' AND FORM||TRIM(SUBMENUID) in (" + col1 + ")) WHERE ROWNUM<3 ", "ID");
                        if (mhd != "0")
                        {
                            hfcode.Value = col1;
                            hffield.Value = "4TH";
                            disp_data();
                            fgen.Fn_open_mseek("Select Menu 4th Level", frm_qstr);
                        }
                        break;
                    case "4TH":
                        if (col1.Length <= 0) return;
                        hfcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                        fillGridView(hfcode.Value);
                        break;
                    case "New_E":
                        if (col1.Length > 0) { }
                        else return;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select USERID,USERNAME,ENT_BY,ENT_DT,ID,RCAN_aDD,RCAN_eDIT,RCAN_DEL,RCAN_PRN from FIN_MRSYS where trim(userid)='" + col1 + "' AND NVL(VISI,'Y')!='N' order by id");
                        
                        DataTable dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT ID,MLEVEL,TEXT,ALLOW_LEVEL,WEB_ACTION,SEARCH_KEY,SUBMENU,SUBMENUID,FORM,PARAM,IMAGEF,CSS,BRN,PRD FROM FIN_MSYS WHERE NVL(VISI,'Y')!='N' AND ID IN (SELECT DISTINCT ID FROM FIN_MRSYS WHERE trim(userid)='" + col1 + "' AND NVL(VISI,'Y')!='N') ORDER BY ID");
                        DataTable dtn = new DataTable();

                        #region table structure
                        dtn.Columns.Add("ID", typeof(string));
                        dtn.Columns.Add("MLEVEL", typeof(string));
                        dtn.Columns.Add("TEXT", typeof(string));
                        dtn.Columns.Add("ALLOW_LEVEL", typeof(string));
                        dtn.Columns.Add("WEB_ACTION", typeof(string));
                        dtn.Columns.Add("SEARCH_KEY", typeof(string));
                        dtn.Columns.Add("SUBMENU", typeof(string));
                        dtn.Columns.Add("SUBMENUID", typeof(string));
                        dtn.Columns.Add("FORM", typeof(string));
                        dtn.Columns.Add("PARAM", typeof(string));

                        dtn.Columns.Add("CSS", typeof(string));
                        dtn.Columns.Add("BRN", typeof(string));
                        dtn.Columns.Add("PRD", typeof(string));
                        #endregion
                        DataRow dr;
                        foreach (DataRow drn in dt2.Rows)
                        {
                            dr = dtn.NewRow();
                            dr["ID"] = drn["ID"].ToString().Trim();
                            dr["MLEVEL"] = drn["MLEVEL"].ToString().Trim();
                            dr["TEXT"] = drn["TEXT"].ToString().Trim();
                            dr["ALLOW_LEVEL"] = drn["ALLOW_LEVEL"].ToString().Trim();
                            dr["WEB_ACTION"] = drn["WEB_ACTION"].ToString().Trim();
                            dr["SEARCH_KEY"] = drn["SEARCH_KEY"].ToString().Trim();
                            dr["SUBMENU"] = drn["SUBMENU"].ToString().Trim();
                            dr["SUBMENUID"] = drn["SUBMENUID"].ToString().Trim();
                            dr["FORM"] = drn["FORM"].ToString().Trim();
                            dr["PARAM"] = drn["PARAM"].ToString().Trim();

                            dr["CSS"] = drn["CSS"].ToString().Trim();
                            dr["BRN"] = drn["BRN"].ToString().Trim();
                            dr["PRD"] = drn["PRD"].ToString().Trim();
                            dtn.Rows.Add(dr);
                        }
                        ViewState["sg1"] = dtn;
                        sg1.DataSource = dtn;
                        sg1.DataBind();


                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "id");
                            if (mhd != "0") ((CheckBox)gr.FindControl("chk1")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_aDD");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk2")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_edit");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk3")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_del");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk4")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_PRN");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk5")).Checked = true;
                        }
                        fgen.EnableForm(this.Controls); disablectrl();
                        break;
                    case "Edit":
                        if (col1.Length > 0) { }
                        else return;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select USERID,USERNAME,ENT_BY,ENT_DT,ID,RCAN_aDD,RCAN_eDIT,RCAN_DEL,RCAN_PRN from FIN_MRSYS where trim(userid)='" + col1 + "' AND NVL(VISI,'Y')!='N' order by id");
                        txtuserid.Value = dt.Rows[0]["userid"].ToString().Trim();
                        txtusername.Value = dt.Rows[0]["username"].ToString().Trim();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT ID,MLEVEL,TEXT,ALLOW_LEVEL,WEB_ACTION,SEARCH_KEY,SUBMENU,SUBMENUID,FORM,PARAM,IMAGEF,CSS,BRN,PRD FROM FIN_MSYS WHERE NVL(VISI,'Y')!='N' AND ID IN (SELECT DISTINCT ID FROM FIN_MRSYS WHERE trim(userid)='" + col1 + "' AND NVL(VISI,'Y')!='N') ORDER BY ID");
                        dtn = new DataTable();

                        #region table structure
                        dtn.Columns.Add("ID", typeof(string));
                        dtn.Columns.Add("MLEVEL", typeof(string));
                        dtn.Columns.Add("TEXT", typeof(string));
                        dtn.Columns.Add("ALLOW_LEVEL", typeof(string));
                        dtn.Columns.Add("WEB_ACTION", typeof(string));
                        dtn.Columns.Add("SEARCH_KEY", typeof(string));
                        dtn.Columns.Add("SUBMENU", typeof(string));
                        dtn.Columns.Add("SUBMENUID", typeof(string));
                        dtn.Columns.Add("FORM", typeof(string));
                        dtn.Columns.Add("PARAM", typeof(string));

                        dtn.Columns.Add("CSS", typeof(string));
                        dtn.Columns.Add("BRN", typeof(string));
                        dtn.Columns.Add("PRD", typeof(string));
                        #endregion
                        dr = null;
                        foreach (DataRow drn in dt2.Rows)
                        {
                            dr = dtn.NewRow();
                            dr["ID"] = drn["ID"].ToString().Trim();
                            dr["MLEVEL"] = drn["MLEVEL"].ToString().Trim();
                            dr["TEXT"] = drn["TEXT"].ToString().Trim();
                            dr["ALLOW_LEVEL"] = drn["ALLOW_LEVEL"].ToString().Trim();
                            dr["WEB_ACTION"] = drn["WEB_ACTION"].ToString().Trim();
                            dr["SEARCH_KEY"] = drn["SEARCH_KEY"].ToString().Trim();
                            dr["SUBMENU"] = drn["SUBMENU"].ToString().Trim();
                            dr["SUBMENUID"] = drn["SUBMENUID"].ToString().Trim();
                            dr["FORM"] = drn["FORM"].ToString().Trim();
                            dr["PARAM"] = drn["PARAM"].ToString().Trim();

                            dr["CSS"] = drn["CSS"].ToString().Trim();
                            dr["BRN"] = drn["BRN"].ToString().Trim();
                            dr["PRD"] = drn["PRD"].ToString().Trim();
                            dtn.Rows.Add(dr);
                        }
                        ViewState["sg1"] = dtn;
                        sg1.DataSource = dtn;
                        sg1.DataBind();


                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "id");
                            if (mhd != "0") ((CheckBox)gr.FindControl("chk1")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_aDD");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk2")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_edit");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk3")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_del");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk4")).Checked = true;

                            mhd = fgen.seek_iname_dt(dt, "id='" + gr.Cells[1].Text + "'", "RCAN_PRN");
                            if (mhd == "Y") ((CheckBox)gr.FindControl("chk5")).Checked = true;
                        }
                        fgen.EnableForm(this.Controls); disablectrl(); edmode.Value = "Y";
                        break;
                    case "Del":
                        if (col1.Length > 0) { }
                        else return;
                        edmode.Value = col1;
                        hffield.Value = "D";
                        fgen.msg("-", "CMSG", "Are You Sure, You want to Delete !!");
                        break;
                    case "List":
                        if (col1.Length > 0) { }
                        else return;
                        squery = "SELECT ID AS FORMID,TEXT AS FORM_NAME,Rcan_add,Rcan_Edit,Rcan_del,Ent_by,Ent_dt,Edt_by,Edt_dt FROM FIN_MRSYS WHERE TRIM(USERID)='" + col1 + "' and NVL(VISI,'Y')!='N' ORDER BY ID";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
                        fgen.Fn_open_rptlevel("Rights given to " + col2 + "", frm_qstr);
                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = "";
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 == "Y")
        {
            if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MRSYS set branchcd='DD' where trim(userid)='" + txtuserid.Value.Trim() + "'");
            string vardate = "";
            vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
            DataRow oporow;
            DataSet oDS = new DataSet();
            oDS = fgen.fill_schema(frm_qstr, frm_cocd, "FIN_MRSYS");
            if (sg1.Rows.Count > 0)
            {
                foreach (GridViewRow gr1 in sg1.Rows)
                {
                    //if (((CheckBox)gr1.FindControl("chk1")).Checked == true)
                    {
                        oporow = oDS.Tables[0].NewRow();

                        oporow["USERID"] = txtuserid.Value.Trim();
                        oporow["USERNAME"] = txtusername.Value.Trim();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["ID"] = gr1.Cells[1].Text.Trim();
                        oporow["TEXT"] = gr1.Cells[2].Text.Trim();
                        oporow["MLEVEL"] = gr1.Cells[3].Text.Trim();
                        oporow["ALLOW_LEVEL"] = gr1.Cells[4].Text.Trim();
                        oporow["WEB_ACTION"] = gr1.Cells[5].Text.Trim();
                        oporow["SEARCH_KEY"] = gr1.Cells[6].Text.Trim();
                        oporow["SUBMENU"] = gr1.Cells[7].Text.Trim();
                        oporow["SUBMENUID"] = gr1.Cells[8].Text.Trim();
                        oporow["FORM"] = gr1.Cells[9].Text.Trim();
                        oporow["PARAM"] = gr1.Cells[10].Text.Trim();

                        oporow["VISI"] = "Y";

                        string color = "skin-blue";
                        if (chk1.Checked) color = "skin-blue";
                        if (chk2.Checked) color = "skin-blue-light";
                        if (chk3.Checked) color = "skin-yellow";
                        if (chk4.Checked) color = "skin-yellow-light";
                        if (chk5.Checked) color = "skin-green";
                        if (chk6.Checked) color = "skin-green-light";
                        if (chk7.Checked) color = "skin-purple";
                        if (chk8.Checked) color = "skin-purple-light";
                        if (chk9.Checked) color = "skin-red";
                        if (chk10.Checked) color = "skin-red-light";
                        if (chk11.Checked) color = "skin-black";
                        if (chk12.Checked) color = "skin-black-light";

                        oporow["user_color"] = color;

                        if (((CheckBox)gr1.FindControl("chk2")).Checked == true) oporow["RCAN_aDD"] = "Y";
                        else oporow["RCAN_aDD"] = "N";

                        if (((CheckBox)gr1.FindControl("chk3")).Checked == true) oporow["RCAN_EDIT"] = "Y";
                        else oporow["RCAN_EDIT"] = "N";

                        if (((CheckBox)gr1.FindControl("chk4")).Checked == true) oporow["RCAN_DEL"] = "Y";
                        else oporow["RCAN_DEL"] = "N";

                        if (((CheckBox)gr1.FindControl("chk5")).Checked == true) oporow["RCAN_PRN"] = "Y";
                        else oporow["RCAN_PRN"] = "N";

                        oporow["CSS"] = gr1.Cells[14].Text.Trim();
                        oporow["BRN"] = gr1.Cells[15].Text.Trim().ToUpper().Replace("&NBSP;", "-");
                        oporow["PRD"] = gr1.Cells[16].Text.Trim().ToUpper().Replace("&NBSP;", "-");

                        if (edmode.Value == "Y")
                        {
                            oporow["ent_by"] = ViewState["entby"].ToString();
                            oporow["ent_dt"] = ViewState["entdt"].ToString();

                            oporow["edt_by"] = frm_uname;
                            oporow["edt_dt"] = vardate;
                        }
                        else
                        {
                            oporow["eNt_by"] = frm_uname;
                            oporow["eNt_dt"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["eDt_dt"] = vardate;
                        }
                        oDS.Tables[0].Rows.Add(oporow);
                    }
                }
                fgen.save_data(frm_qstr, frm_cocd, oDS, "FIN_MRSYS");
            }
            if (edmode.Value == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from FIN_MRSYS where branchcd='DD' and trim(userid)='" + txtuserid.Value.Trim() + "'");
                fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
            }
            else fgen.msg("-", "AMSG", lblheader.Text + " " + " Saved Successfully ");
            fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl();
            sg1.DataSource = null;
            sg1.DataBind();
        }
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        ViewState["sg1"] = null;
        if (btnext.Text == " Exit ")
        { Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr); }
        else
        {
            clearctrl();
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            enablectrl();
            sg1.DataSource = null;
            sg1.DataBind();
        }
    }
    public void disp_data()
    {
        btnval = hffield.Value;
        switch (btnval)
        {
            case "UID":
                squery = "select userid as fstr,username,userid,deptt,emailid,contactno as contact from evas order by username";
                break;
            case "T_FORM":
                squery = "SELECT DISTINCT FORM AS FSTR,TEXT,ID FROM FIN_MSYS WHERE NVL(VISI,'-')!='N' AND MLEVEL='1' ORDER BY ID";
                break;
            case "2ND":
                squery = "SELECT DISTINCT FORM||SUBMENUID AS FSTR,TEXT,ID FROM FIN_MSYS WHERE NVL(VISI,'-')!='N' AND MLEVEL='2' AND FORM='" + hfcode.Value + "' ORDER BY ID";
                break;
            case "3RD":
                squery = "SELECT DISTINCT FORM||SUBMENUID AS FSTR,TEXT,ID FROM FIN_MSYS WHERE NVL(VISI,'-')!='N' AND MLEVEL='3' AND FORM||TRIM(SUBMENUID)='" + hfcode.Value + "' ORDER BY ID";
                break;
            case "4TH":
                squery = "SELECT DISTINCT FORM||SUBMENUID AS FSTR,TEXT,ID FROM FIN_MSYS WHERE NVL(VISI,'-')!='N' AND MLEVEL='4' AND FORM||TRIM(SUBMENUID) in (" + hfcode.Value + ") ORDER BY ID";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "List" || btnval == "New_E")
                    squery = "select distinct a.userid as FSTR,a.Username,a.Userid,a.Ent_by,a.Ent_Dt,a.Edt_by,a.Edt_Dt from FIN_MRSYS a,evas b where trim(a.userid)=trim(b.userid) order by a.userid";
                break;
        }
        if (squery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
        }
    }
    protected void btnuserid_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "UID";
        disp_data();
        fgen.Fn_open_sseek("Select Username", frm_qstr);
    }
    void fillGridView(string iconsID)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        if (ViewState["sg1"] != null)
        {
            dt = (DataTable)ViewState["sg1"];
        }
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                #region table structure
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("MLEVEL", typeof(string));
                dt.Columns.Add("TEXT", typeof(string));
                dt.Columns.Add("ALLOW_LEVEL", typeof(string));
                dt.Columns.Add("WEB_ACTION", typeof(string));
                dt.Columns.Add("SEARCH_KEY", typeof(string));
                dt.Columns.Add("SUBMENU", typeof(string));
                dt.Columns.Add("SUBMENUID", typeof(string));
                dt.Columns.Add("FORM", typeof(string));
                dt.Columns.Add("PARAM", typeof(string));

                dt.Columns.Add("CSS", typeof(string));
                dt.Columns.Add("BRN", typeof(string));
                dt.Columns.Add("PRD", typeof(string));
                #endregion
            }
            if (iconsID.Length > 0)
            {
                DataTable dtNew = new DataTable();
                dtNew = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT ID,MLEVEL,TEXT,ALLOW_LEVEL,WEB_ACTION,SEARCH_KEY,SUBMENU,SUBMENUID,FORM,PARAM,IMAGEF,CSS,BRN,PRD FROM FIN_MSYS WHERE NVL(VISI,'Y')!='N' AND ID IN (" + iconsID + ") ORDER BY ID");
                foreach (DataRow drn in dtNew.Rows)
                {
                    mhd = fgen.seek_iname_dt(dt, "ID='" + drn["ID"].ToString().Trim() + "'", "ID");
                    if (mhd == "0")
                    {
                        dr = dt.NewRow();
                        dr["ID"] = drn["ID"].ToString().Trim();
                        dr["MLEVEL"] = drn["MLEVEL"].ToString().Trim();
                        dr["TEXT"] = drn["TEXT"].ToString().Trim();
                        dr["ALLOW_LEVEL"] = drn["ALLOW_LEVEL"].ToString().Trim();
                        dr["WEB_ACTION"] = drn["WEB_ACTION"].ToString().Trim();
                        dr["SEARCH_KEY"] = drn["SEARCH_KEY"].ToString().Trim();
                        dr["SUBMENU"] = drn["SUBMENU"].ToString().Trim();
                        dr["SUBMENUID"] = drn["SUBMENUID"].ToString().Trim();
                        dr["FORM"] = drn["FORM"].ToString().Trim();
                        dr["PARAM"] = drn["PARAM"].ToString().Trim();

                        dr["CSS"] = drn["CSS"].ToString().Trim();
                        dr["BRN"] = drn["BRN"].ToString().Trim();
                        dr["PRD"] = drn["PRD"].ToString().Trim();
                        dt.Rows.Add(dr);
                    }
                }
            }
            ViewState["sg1"] = dt;
        }
        sg1.DataSource = dt;
        sg1.DataBind();
    }
    void BindData()
    {
        DataTable dtNew = new DataTable();
        //dtNew = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT ID,MLEVEL,TEXT,ALLOW_LEVEL,WEB_ACTION,SEARCH_KEY,SUBMENU,SUBMENUID,FORM,PARAM,IMAGEF,CSS,BRN,PRD FROM FIN_MSYS WHERE NVL(VISI,'Y')!='N' ORDER BY ID");
        //sg1.DataSource = dtNew;
        //sg1.DataBind();

        //sg2.DataSource = dtNew;
        //sg2.DataBind();
        //for (int i = 0; i < sg2.Rows.Count; i++)
        //{ sg2.Rows[i].Visible = false; }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[3].Style["display"] = "none";
            //sg1.HeaderRow.Cells[3].Style["display"] = "none";
            e.Row.Cells[4].Style["display"] = "none";
            sg1.HeaderRow.Cells[4].Style["display"] = "none";
            e.Row.Cells[5].Style["display"] = "none";
            sg1.HeaderRow.Cells[5].Style["display"] = "none";
            e.Row.Cells[6].Style["display"] = "none";
            sg1.HeaderRow.Cells[6].Style["display"] = "none";
            e.Row.Cells[7].Style["display"] = "none";
            sg1.HeaderRow.Cells[7].Style["display"] = "none";
            e.Row.Cells[8].Style["display"] = "none";
            sg1.HeaderRow.Cells[8].Style["display"] = "none";
            e.Row.Cells[9].Style["display"] = "none";
            sg1.HeaderRow.Cells[9].Style["display"] = "none";
            e.Row.Cells[10].Style["display"] = "none";
            sg1.HeaderRow.Cells[10].Style["display"] = "none";

            e.Row.Cells[14].Style["display"] = "none";
            sg1.HeaderRow.Cells[14].Style["display"] = "none";
            e.Row.Cells[15].Style["display"] = "none";
            sg1.HeaderRow.Cells[15].Style["display"] = "none";
            e.Row.Cells[16].Style["display"] = "none";
            sg1.HeaderRow.Cells[16].Style["display"] = "none";
        }
    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[3].Style["display"] = "none";
            //sg2.HeaderRow.Cells[3].Style["display"] = "none";
            //e.Row.Cells[4].Style["display"] = "none";
            //sg2.HeaderRow.Cells[4].Style["display"] = "none";
            //e.Row.Cells[5].Style["display"] = "none";
            //sg2.HeaderRow.Cells[5].Style["display"] = "none";
            //e.Row.Cells[6].Style["display"] = "none";
            //sg2.HeaderRow.Cells[6].Style["display"] = "none";
            //e.Row.Cells[7].Style["display"] = "none";
            //sg2.HeaderRow.Cells[7].Style["display"] = "none";
            //e.Row.Cells[8].Style["display"] = "none";
            //sg2.HeaderRow.Cells[8].Style["display"] = "none";
            //e.Row.Cells[9].Style["display"] = "none";
            //sg2.HeaderRow.Cells[9].Style["display"] = "none";
            //e.Row.Cells[10].Style["display"] = "none";
            //sg2.HeaderRow.Cells[10].Style["display"] = "none";

            //e.Row.Cells[14].Style["display"] = "none";
            //sg2.HeaderRow.Cells[14].Style["display"] = "none";
            //e.Row.Cells[15].Style["display"] = "none";
            //sg2.HeaderRow.Cells[15].Style["display"] = "none";
            //e.Row.Cells[16].Style["display"] = "none";
            //sg2.HeaderRow.Cells[16].Style["display"] = "none";
        }
    }
    protected void btnSelectIcon_Click(object sender, EventArgs e)
    {
        hffield.Value = "T_FORM";
        disp_data();
        fgen.Fn_open_sseek("Select Menu 1st Level", frm_qstr);
    }
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (index < sg1.Rows.Count)
        {
            hf1.Value = index.ToString();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            //----------------------------
            hffield.Value = "RMV";
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            dt.Rows[index].Delete();
            ViewState["sg1"] = dt;
            fillGridView("");
        }
    }
}