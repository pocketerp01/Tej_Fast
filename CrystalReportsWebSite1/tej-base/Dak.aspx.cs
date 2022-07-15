using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class Dak : System.Web.UI.Page
{
    string btnval, SQuery, frm_cocd, frm_uname, frm_UserID, col1, col2, col3, frm_mbr, vardate, fromdt, todt, DateRange, frm_year, frm_ulvl, merr = "0", eID, cond = "", frm_formID;
    DataTable dt; DataRow oporow;
    fgenDB fgen = new fgenDB();
    string frm_url, frm_qstr, Checked_ok, chk_rights;
    DataSet oDS; string frm_vnum = "", frm_tabname, frm_vty, Prg_Id, save_it, frm_PageName, PrdRange, frm_CDT1;

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
                    frm_year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                btnnew.Focus();
                fgen.DisableForm(this.Controls);
                enablectrl();
            }
            set_Val();
            btnprint.Visible = false;
            btnlist.Visible = false;
            if (lblUpload.Text.Length > 1)
            {
                btnView1.Visible = true;
                btnDwnld1.Visible = true;
            }
        }
    }

    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false; imguserid.Enabled = false; Attch.Enabled = false;
    }

    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnprint.Disabled = true; btnlist.Disabled = true; imguserid.Enabled = true; Attch.Enabled = true;
    }

    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }

    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        doc_vty.Value = "DK";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = "DK"; frm_tabname = "scratch";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }

    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            case "USR":
                SQuery = "select distinct type1 as fstr,name,type1 as code,acref as email_id from typegrp where id='SE' order by name";
                break;

            case "USR1":
                SQuery = "select distinct userid as fstr,username as name,userid as code,emailid as email_id from evas order by username";
                break;

            case "CLIENT":
                SQuery = "select type1 as fstr,name as client_name,acref2 as client_code,type1 as erp_code from typegrp where id='SC' order by name";
                break;

            default:
                if (btnval == "Del" || btnval == "Edit" || btnval == "NEW_E")
                {
                    if (frm_ulvl == "0") cond = "";
                    else cond = "and ent_by='" + frm_uname.Trim() + "'";
                    // SQuery = "select distinct branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_Date,col1 as user_name,ent_by as assign_by,to_char(ent_Dt,'dd/mm/yyyy') as assign_Dt,col14 as Subject,(case when nvl(trim(col17),'-')='-' then 'NORMAL TASK' else col17 end) as task_type from " + frm_tabname + "  where branchcd='" + mbr + "' and type='DK' " + cond + " order by vchnum desc";
                    SQuery = "select distinct trim(" + doc_nf.Value + ")||to_Char(" + doc_df.Value + ",'dd/mm/yyyy') as fstr," + doc_nf.Value + " as entry_no,to_char(" + doc_df.Value + ",'dd/mm/yyyy') as entry_Date,col1 as user_name,ent_by as assign_by,to_char(ent_Dt,'dd/mm/yyyy') as assign_Dt,col14 as Subject,(case when nvl(trim(col17),'-')='-' then 'NORMAL TASK' else col17 end) as task_type from " + frm_tabname + "  where branchcd='" + frm_mbr + "' and type='DK' " + cond + " AND " + doc_df.Value + " " + DateRange + " order by vchnum desc";
                }
                break;
        }
        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }

    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            //            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            hffield.Value = "New";
            fgen.msg("-", "CMSG", "Do You Want To Copy Task From Old One");
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }

    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }

    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        if (txtuserid.Text.Trim().Length > 0 || txtuserid.Text.Trim() != "-")
        {
            string[] ml = txtuserid.Text.Trim().Replace(",", ";").Split(';');
            foreach (string mid in ml)
            {
                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select type1,acref as email_id from typegrp where id='SE' and upper(trim(type1))='" + mid.ToUpper() + "'", "type1");
                if (col1 == "0") col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select userid,username from evas where upper(trim(userid))='" + mid.ToUpper() + "'", "userid");
                if (col1 != "0")
                {
                    col1 = (fgen.ChkDate(txttskdate.Text.Trim().ToString())).ToString();
                    if (col1 != "0")
                    {
                        if (Convert.ToDateTime(txttskdate.Text.Trim()) >= Convert.ToDateTime(vardate))
                        {
                            if (txtmsg.Text.Trim().Length > 0)
                            {
                                fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
                                btnsave.Disabled = true;
                            }
                            else fgen.msg("-", "AMSG", "Please enter any message");
                        }
                        else fgen.msg("-", "AMSG", "Task Date can not be less then System Date");
                    }
                    else fgen.msg("-", "AMSG", "Not a valid date entered in Task Date");
                }
                else fgen.msg("-", "AMSG", "Wrong User id is inserted = " + mid);
            }
        }
        else fgen.msg("-", "AMSG", "Please fill user id");
    }

    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }

    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }

    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
    }

    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }

    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }

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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
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
        btnval = hffield.Value;
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + "  where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from wSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "New")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                hffield.Value = "NEW_E";
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            else
            {
                fgen.EnableForm(this.Controls); disablectrl();
                txtvchnum.Text = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "'", 6, "vch");
                imguserid.Focus(); txtvchdate.Text = vardate; txttskdate.Text = vardate;
                txtentby.Text = frm_uname;
                txtentdt.Text = vardate;
            }
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "USR":
                    if (col1 == "" || col1 == "-") return;
                    dt = new DataTable();
                    if (col1.Trim().Length == 4) SQuery = "select distinct name,type1 as code,acref as email_id,acref3 as Department from typegrp where id='SE' and type1 in ('" + col1 + "')";
                    else SQuery = "select distinct name,type1 as code,acref as email_id,acref3 as Department from typegrp where id='SE' and type1 in ('" + col1 + "')";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    col2 = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (col2.Length > 0)
                        { col2 = col2 + ";" + "" + dr["code"].ToString().Trim() + ""; }
                        else
                        { col2 = "" + dr["code"].ToString().Trim() + ""; }
                    }
                    if (txtuserid.Text.Length > 0) txtuserid.Text = txtuserid.Text.Trim() + ";" + col2; //col2
                    else txtuserid.Text = col1;
                    txtsubject.Focus();
                    break;

                case "USR1":
                    if (col1 == "" || col1 == "-") return;
                    dt = new DataTable();
                    if (col1.Trim().Length == 4) SQuery = "select distinct username,userid as code,emailid as email_id from evas where userid in ('" + col1 + "')";
                    else SQuery = "select distinct username,userid as code,emailid as email_id from evas where userid in (" + col1 + ")";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    col2 = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (col2.Length > 0)
                        { col2 = col2 + ";" + "" + dr["code"].ToString().Trim() + ""; }
                        else
                        { col2 = "" + dr["code"].ToString().Trim() + ""; }
                    }
                    if (txtuserid.Text.Length > 0) txtuserid.Text = txtuserid.Text.Trim() + ";" + col2;  //
                    else txtuserid.Text = col1;
                    txtsubject.Focus();
                    break;

                case "NEW_E":
                    if (col1 == "" || col1 == "-") return;
                    fgen.EnableForm(this.Controls); disablectrl();
                    txtvchnum.Text = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where type='DK' and branchcd='" + frm_mbr + "'", 6, "vch");
                    imguserid.Focus(); txtvchdate.Text = vardate; txttskdate.Text = vardate;
                    txtentby.Text = frm_uname; txtentdt.Text = vardate;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select * from " + frm_tabname + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and ent_by='" + frm_uname.Trim() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        fgen.EnableForm(this.Controls); disablectrl();
                        col2 = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (col2.Length > 0) { col2 = col2 + "," + "" + dr["acode"].ToString().Trim() + ""; }
                            else { col2 = "" + dr["acode"].ToString().Trim() + ""; }
                        }
                        txtuserid.Text = col2;
                        txtsubject.Text = dt.Rows[0]["col14"].ToString(); txtmsg.Text = dt.Rows[0]["remarks"].ToString();
                        if (dt.Rows[0]["col5"].ToString().Length > 1) txtemailcc.Text = dt.Rows[0]["col5"].ToString();
                        if (dt.Rows[0]["col4"].ToString().Length > 0) ddl1.SelectedItem.Value = dt.Rows[0]["col4"].ToString();
                    }
                    break;

                case "Edit":
                    if (col1.Length < 1) return;
                    clearctrl();
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select * from " + frm_tabname + "  where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and ent_by='" + frm_uname.Trim() + "'");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    if (dt.Rows.Count > 0)
                    {
                        fgen.EnableForm(this.Controls); disablectrl();
                        col2 = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (col2.Length > 0)
                            { col2 = col2 + ";" + "" + dr["acode"].ToString().Trim() + ""; }
                            else
                            { col2 = "" + dr["acode"].ToString().Trim() + ""; }
                        }
                        txtuserid.Text = col2; txtvchnum.Text = dt.Rows[0]["vchnum"].ToString(); txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString()).ToString("dd/MM/yyyy");
                        txtsubject.Text = dt.Rows[0]["col14"].ToString(); txtmsg.Text = dt.Rows[0]["remarks"].ToString();
                        txttskdate.Text = Convert.ToDateTime(dt.Rows[0]["docdate"].ToString()).ToString("dd/MM/yyyy"); if (dt.Rows[0]["col4"].ToString().Length > 0) ddl1.SelectedItem.Value = dt.Rows[0]["col4"].ToString();
                        txtDays.Text = dt.Rows[0]["num1"].ToString();
                        txtentby.Text = dt.Rows[0]["ent_by"].ToString(); txtentdt.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString()).ToString("dd/MM/yyyy");
                        edmode.Value = "Y"; ViewState["entby"] = dt.Rows[0]["ent_by"].ToString(); ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        if (dt.Rows[0]["naration"].ToString().Trim().Length > 1)
                        {
                            lblUpload.Text = dt.Rows[0]["naration"].ToString().Trim();
                            txtAttch.Text = dt.Rows[0]["col47"].ToString().Trim();
                        }
                    }
                    break;

                case "Del":
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    edmode.Value = col1.Trim();
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
            }
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel(lblheader.Text + " Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
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
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";
                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        sendMail();

                        if (edmode.Value == "Y")
                        {
                            if (merr == "0")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully & Mail Not Sent");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully & Mail Sent");
                            }
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                if (merr == "0")
                                {
                                    fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully & Mail Not Sent");
                                }
                                else
                                {
                                    fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully & Mail Sent");
                                }
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                    }
                    catch (Exception ex)
                    {
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N"; btnsave.Disabled = false;
                    }
                }
            }
            #endregion
        }
    }

    protected void imguserid_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "USR";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Users", frm_qstr);
    }

    protected void imguserid1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "USR1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Users", frm_qstr);
    }

    void sendMail()
    {
        System.Text.StringBuilder msb = new System.Text.StringBuilder();
        msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
        msb.Append("Dear " + dt.Rows[0]["name"].ToString().Trim() + ",<br/><br/>");
        if (edmode.Value == "Y") msb.Append("For your kind information below mentioned task is Re-assigned to you.<br/><br/>");
        else msb.Append("For your kind information below mentioned new task is assigned to you.<br/><br/>");
        msb.Append("<table border=1 cellspacing=2 cellpadding=2 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");
        msb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'><td><b>Assigned by</b></td><td><b>Assign No./Date</b></td><td><b>Subject</b></td><td><b>Due Date</b></td><td><b>Priority</b></td></tr>");
        msb.Append("<td>");
        msb.Append("Mr/Ms " + frm_uname);
        msb.Append("</td>");
        msb.Append("<td>");
        msb.Append(frm_vnum.ToUpper() + "/" + vardate.ToUpper());
        msb.Append("</td>");
        msb.Append("<td style='width:150px;'>");
        msb.Append(txtsubject.Text.Trim().ToUpper());
        msb.Append("</td>");
        msb.Append("<td>");
        msb.Append("" + txttskdate.Text.Trim().ToUpper() + "");
        msb.Append("</td>");
        msb.Append("<td>");
        msb.Append("" + ddl1.SelectedItem.Text.Trim().ToUpper() + "");
        msb.Append("</td>");
        msb.Append("</tr>");
        msb.Append("<tr>");
        msb.Append("<td>Details: ");
        msb.Append("</td>");
        msb.Append("<td colspan='5'>");
        msb.Append(txtmsg.Text.Trim().ToString().ToUpper() + "");
        msb.Append("</td>");
        msb.Append("</tr>");
        msb.Append("</table><br/><br/>");

        msb.Append("<br>===========================================================<br>");
        msb.Append("<br>This Report is Auto generated from the Tejaxo ERP.");
        msb.Append("<br>The above details are to be best of information and data available to the ERP system.");
        msb.Append("<br>Errors or Omissions if any are regretted.");
        msb.Append("Thanks and Regards,<br/>");
        msb.Append("" + fgenCO.chk_co(frm_cocd) + "");
        msb.Append("</body></html>");
        string cc = "";
        string subje = "New Task : [" + frm_vnum + "] " + txtsubject.Text.Trim() + " (" + ddl1.SelectedItem.ToString() + ")";
        if (txtemailcc.Text.Trim().Length > 2) cc = txtemailcc.Text.Trim();
        if (!eID.Contains("info@pocketdriver.in"))
        {
            if (lblUpload.Text.Length > 5)
            {
                string filepath = lblUpload.Text;
                merr = fgen.send_mail(frm_cocd, "Tejaxo ERP", eID, cc, "", subje, msb.ToString(), filepath);
            }
            else merr = fgen.send_mail(frm_cocd, "Tejaxo ERP", eID, cc, "", subje, msb.ToString());
        }
    }

    void save_fun()
    {
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        string[] ml = txtuserid.Text.Trim().Replace(",", ";").Split(';');
        foreach (string mid in ml)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["branchcd"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "select type1,name,acref as email_id from typegrp where id='SE' and upper(trim(type1))='" + mid.ToUpper() + "'");
            if (dt.Rows.Count > 0)
            {
                oporow["acode"] = dt.Rows[0]["type1"].ToString().Trim().ToUpper();
                oporow["col1"] = dt.Rows[0]["name"].ToString().Trim().ToUpper();
                oporow["col2"] = dt.Rows[0]["email_id"].ToString().Trim().ToUpper();
                eID = dt.Rows[0]["email_id"].ToString().Trim().ToUpper();
            }
            else
            {
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select distinct userid,username as name,emailid  from evas where upper(trim(userid))='" + mid.ToUpper() + "'");
                if (dt.Rows.Count > 0)
                {
                    oporow["acode"] = dt.Rows[0]["userid"].ToString().Trim().ToUpper();
                    oporow["col1"] = dt.Rows[0]["name"].ToString().Trim().ToUpper();
                    oporow["col2"] = dt.Rows[0]["emailid"].ToString().Trim().ToUpper();
                    eID = dt.Rows[0]["emailid"].ToString().Trim().ToUpper();
                }
            }
            //Approval Status
            oporow["col3"] = "-";
            //Priority
            oporow["col4"] = ddl1.SelectedItem.ToString().Trim().ToUpper();
            oporow["col5"] = txtemailcc.Text.Trim().ToUpper();
            oporow["col14"] = txtsubject.Text.Trim().ToUpper();
            oporow["remarks"] = txtmsg.Text.Trim().ToUpper();
            oporow["docdate"] = txttskdate.Text.Trim().ToUpper();
            oporow["NUM1"] = fgen.make_double(txtDays.Text.Trim().ToUpper());
            if (txtAttch.Text.Length > 1)
            {
                oporow["naration"] = lblUpload.Text.Trim();
                oporow["col47"] = txtAttch.Text.Trim();
            }
            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"];
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

    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_erp\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath = filepath + txtvchnum.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            filepath = Server.MapPath("~/tej-base/UPLOAD/") + txtvchnum.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            lblUpload.Text = filepath;

            btnView1.Visible = true;
            btnDwnld1.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
    }

    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }

    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));

            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }
}