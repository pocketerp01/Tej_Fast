using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Data.OleDb;


public partial class om_party_bal : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_grps, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    //double double_val2, double_val1;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {

            btnnew.Focus();
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
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");
                doc_addl.Value = "-";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

        }
    }
    //------------------------------------------------------------------------------------
    void getColHeading()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            dtCol = fgen.getdata(frm_qstr, frm_cocd, fgenMV.Fn_Get_Mvar(frm_qstr, "U_SYS_COM_QRY") + " WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
        }
        ViewState["d" + frm_qstr + frm_formID] = dtCol;
    }
    //------------------------------------------------------------------------------------
    void setColHeadings()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            getColHeading();
        }
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        #region hide hidden columns
        sg1.Columns[0].Visible = false;
        sg1.Columns[1].Visible = false;
        sg1.Columns[2].Visible = false;
        sg1.Columns[3].Visible = false;
        sg1.Columns[4].Visible = false;
        sg1.Columns[5].Visible = false;
        sg1.Columns[6].Visible = false;
        sg1.Columns[7].Visible = false;
        sg1.Columns[8].Visible = false;
        sg1.Columns[9].Visible = false;
        #endregion
        if (dtCol == null) return;
        if (sg1.Rows.Count <= 0) return;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

            for (int K = 0; K < sg1.Rows.Count; K++)
            {
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
            }
            orig_name = orig_name.ToUpper();
            //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
            if (sR == tb_Colm)
            {
                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    sg1.Columns[sR].Visible = false;
                }
                // Setting Heading Name

                sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");

                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }



        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = false;


        //if (Prg_Id == "M12008")
        //{
        //    tab5.Visible = true;
        //    txtlbl8.Attributes.Remove("readonly");
        //    txtlbl9.Attributes.Remove("readonly");
        //}
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;




        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F70801":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_GRPS", "('02','16')");
                frm_tabname = "recebal";
                break;
            case "F70802":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_GRPS", "('06')");
                frm_tabname = "recebal";
                break;
            case "F70803":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_GRPS", "('03','12')");
                frm_tabname = "voucherop";
                break;

        }
        Type_Sel_query();
        btnlist.Visible = false;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            case "BTN_10":
                break;
            case "BTN_11":
                break;
            case "BTN_12":
                break;
            case "BTN_13":
                break;
            case "BTN_14":
                break;
            case "BTN_15":
                break;
            case "BTN_16":
                break;
            case "BTN_17":
                break;
            case "BTN_18":
                break;
            case "BTN_19":
                break;

            case "TACODE":
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                string grp_cond = "";
                switch (Prg_Id)
                {
                    case "F70801":
                        grp_cond = "substr(A.grp,1,2) in ('02','16') and ";
                        break;
                    case "F70802":
                        grp_cond = "substr(A.grp,1,2)='06' and ";
                        break;
                    case "F70803":
                        grp_cond = "substr(A.grp,1,2) in ('03','12') and ";
                        break;

                }
                SQuery = "SELECT a.ACODE AS FSTR,replacE(a.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,a.ADDR1,a.ADDR2,a.staten as state,a.Pay_num,trim(nvl(a.gst_no,'-')) as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.Grp,a.BUYCODE as oldcode FROM famst a where " + grp_cond + " length(Trim(nvl(a.deac_by,'-')))<=1  and (case when length(nvl(a.showinbr,'-'))>1 then instr(nvl(a.showinbr,'-'),'" + frm_mbr + "') else 1 end)>0   order by a.ANAME ";

                break;
            case "TICODE":

                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                switch (Prg_Id)
                {
                    case "F70801":
                    case "F70802":
                        SQuery = "SELECT a.ACODE AS FSTR,replacE(a.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,a.ADDR1,a.ADDR2,a.staten as state,a.Pay_num,trim(nvl(a.gst_no,'-')) as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.Grp,a.BUYCODE as oldcode FROM famst a where trim(a.acode)='" + txtlbl4.Text + "' length(Trim(nvl(a.deac_by,'-')))<=1  and (case when length(nvl(a.showinbr,'-'))>1 then instr(nvl(a.showinbr,'-'),'" + frm_mbr + "') else 1 end)>0   order by a.ANAME ";
                        break;
                    case "F70803":
                        SQuery = "SELECT a.ACODE AS FSTR,replacE(a.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,a.ADDR1,a.ADDR2,a.staten as state,a.Pay_num,trim(nvl(a.gst_no,'-')) as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.Grp,a.BUYCODE as oldcode FROM famst a where trim(a.acode)!='" + txtlbl4.Text + "' and substr(a.grp,1,2)in ('02','06','07','12','14','16','17') and  length(Trim(nvl(a.deac_by,'-')))<=1  and (case when length(nvl(a.showinbr,'-'))>1 then instr(nvl(a.showinbr,'-'),'" + frm_mbr + "') else 1 end)>0   order by a.ANAME ";
                        break;
                }

                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":

                break;


            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                {
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_GRPS");
                    SQuery = "select distinct trim(A.acode)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,b.Aname,a.ent_by,to_char(a.ent_Date,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.acode as ERP_Code from " + frm_tabname + " a,famst b where trim(a.acode)=trim(B.acode) and b.GRP in " + frm_vty + " and a.branchcd='" + frm_mbr + "' order by vdd desc,a.vchnum desc";
                }
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
            frm_vnum = fgen.next_no(frm_qstr, frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);
            btnlbl4.Focus();

            frm_vty = col1;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
            sg1_dt = new DataTable();
            create_tab();
            sg1_add_blankrows();


            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            setColHeadings();
            ViewState["sg1"] = sg1_dt;
            // Popup asking for Copy from Older Data
            //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
            hffield.Value = "NEW_E";
            fgen.EnableForm(this.Controls);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Edit_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        // fgen.open_pwdbox("-", frm_qstr);
        // return;
        //chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        //clearctrl();
        //if (chk_rights == "Y")
        //{
        //    hffield.Value = "Edit";
        //    make_qry_4_popup();
        //    fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        //}
        //else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
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

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();

            hffield.Value = "Del_E";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);

            ////hffield.Value = "DEL_C";
            ////fgen.open_pwdbox("-", frm_qstr);
            ////return;

            //set_Val();
            //hffield.Value = "Del";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();

        sg1_dt = new DataTable();
        sg2_dt = new DataTable();
        sg3_dt = new DataTable();
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();

        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();


        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.acode)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");


                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "NEW_E")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "COPY_OLD";
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
        }

        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "New":

                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);


                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    btnlbl4.Focus();

                    sg1_dt = new DataTable();
                    create_tab();
                    sg1_add_blankrows();


                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    setColHeadings();
                    ViewState["sg1"] = sg1_dt;
                    // Popup asking for Copy from Older Data
                    fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    hffield.Value = "NEW_E";
                    break;

                    #endregion

                case "COPY_OLD":

                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "EDIT_C":
                    //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CONFIRM") == "2")
                    //{
                    // frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
                    //}
                    break;
                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
                    break;
                case "Del_E":
                    if (col1 == "") return;
                    clearctrl();
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();

                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                    switch (Prg_Id)
                    {
                        case "F70801":
                        case "F70802":
                            SQuery = "Select to_char(a.invdate,'dd/mm/yyyy') As inv_dtd,b.aname,a.* from " + frm_tabname + " a,famst b where trim(a.acode)=trim(B.acode) and trim(a.acode)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ORDER BY A.vchid";
                            break;
                        case "F70803":
                            SQuery = "Select to_char(a.refdate,'dd/mm/yyyy') As ref_dtd,b.aname,a.* from " + frm_tabname + " a,famst b where trim(a.acode)=trim(B.acode) and trim(a.acode)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ORDER BY A.srno";
                            break;

                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(upper(acode))=upper(Trim('" + dt.Rows[i]["acode"].ToString().Trim() + "'))", "aname");


                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["aname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";


                            switch (Prg_Id)
                            {
                                case "F70801":
                                case "F70802":
                                    sg1_dr["sg1_t1"] = dt.Rows[i]["invno"].ToString().Trim();
                                    sg1_dr["sg1_t2"] = dt.Rows[i]["inv_dtd"].ToString().Trim();
                                    break;
                                case "F70803":
                                    sg1_dr["sg1_t1"] = dt.Rows[i]["refnum"].ToString().Trim();
                                    sg1_dr["sg1_t2"] = dt.Rows[i]["ref_dtd"].ToString().Trim();
                                    break;

                            }

                            sg1_dr["sg1_t3"] = dt.Rows[i]["dramt"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["cramt"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["fctype"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["fcrate"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["fcdramt"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["fccramt"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_date"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.vchnum as entry_no,to_Char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.srno,a.frm_name as FormID,a.frm_Title as FormName,a.obj_name as field_Name,a.obj_caption as Field_Title,a.obj_visible as to_show,a.obj_Width,a.col_no,a.obj_maxlen as field_maxlen,a.obj_readonly as readonly,A.obj_fmand,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " a where A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_vty + col1 + "') order by a.srno,A." + doc_nf.Value + " ";
                    if (frm_tabname.ToUpper() == "UDF_CONFIG")
                        SQuery = "select a.vchnum as entry_no,to_Char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.srno,a.frm_name as FormID,a.frm_Title as FormName,a.obj_name as field_Name,a.obj_caption as Field_Title,a.obj_visible as to_show,a.obj_Width,a.col_no,a.obj_maxlen as field_maxlen,a.obj_readonly as readonly,'-' AS obj_fmand,A.OBJ_FIELD_CAST,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " a where A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_vty + col1 + "') order by a.srno,A." + doc_nf.Value + " ";
                    if (frm_tabname.ToUpper() == "SYS_CONFIG")
                        SQuery = "select a.vchnum as entry_no,to_Char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.srno,a.frm_name as FormID,a.frm_Title as FormName,a.obj_name as field_Name,a.obj_caption as Field_Title,a.obj_visible as to_show,a.obj_Width,a.col_no,a.obj_maxlen as field_maxlen,a.obj_readonly as readonly,A.obj_fmand,A.OBJ_FIELD_CAST,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " a where A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_vty + col1 + "') order by a.srno,A." + doc_nf.Value + " ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Form Configuration of ==> " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", ""), frm_qstr);
                    //fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    txtlbl6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    btnlbl7.Focus();
                    break;
                case "BTN_10":
                    break;
                case "BTN_11":
                    break;
                case "BTN_12":
                    break;
                case "BTN_13":
                    break;
                case "BTN_14":
                    break;
                case "BTN_15":
                    break;
                case "BTN_16":
                    break;
                case "BTN_17":
                    break;
                case "BTN_18":
                    break;
                case "BTN_19":
                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
                    break;
                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (sg1.Rows.Count <= 0) return;
                    if (col1.Length <= 0) return;
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            if (sg1.Rows.Count > 0) { } else return;
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                            sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_h1"] = col1;
                        sg1_dr["sg1_h2"] = col2;
                        sg1_dr["sg1_h3"] = "-";
                        sg1_dr["sg1_h4"] = "-";
                        sg1_dr["sg1_h5"] = "-";
                        sg1_dr["sg1_h6"] = "-";
                        sg1_dr["sg1_h7"] = "-";
                        sg1_dr["sg1_h8"] = "-";
                        sg1_dr["sg1_h9"] = "-";
                        sg1_dr["sg1_h10"] = "-";

                        sg1_dr["sg1_f1"] = col1;
                        sg1_dr["sg1_f2"] = col2;
                        sg1_dr["sg1_f3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                        sg1_dr["sg1_f4"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                        sg1_dr["sg1_f5"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                        sg1_dr["sg1_t1"] = "";
                        sg1_dr["sg1_t2"] = frm_CDT1;
                        sg1_dr["sg1_t3"] = "";
                        sg1_dr["sg1_t4"] = "";
                        sg1_dr["sg1_t5"] = "";
                        sg1_dr["sg1_t6"] = "";
                        sg1_dr["sg1_t7"] = "";
                        sg1_dr["sg1_t8"] = "";
                        sg1_dr["sg1_t9"] = "";

                        sg1_dt.Rows.Add(sg1_dr);
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    if (sg1.Rows.Count > 0)
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    //********* Saving in Hidden Field 
                    try
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                        setColHeadings();
                    }
                    catch { }
                    break;
                case "SG3_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg3"] != null)
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                            sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                            sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;

                            sg3_dr["sg3_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg3_dr["sg3_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg3_dr["sg3_t1"] = "";
                            sg3_dr["sg3_t2"] = "";
                            sg3_dr["sg3_t3"] = "";
                            sg3_dr["sg3_t4"] = "";
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                    }
                    sg3_add_blankrows();

                    ViewState["sg3"] = sg3_dt;
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    dt.Dispose(); sg3_dt.Dispose();
                    ((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    #endregion
                    break;


                case "SG2_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        i = 0;
                        for (i = 0; i < sg2.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = (i + 1);

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();


                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg2_add_blankrows();

                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;
                case "SG3_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        i = 0;
                        for (i = 0; i < sg3.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = (i + 1);
                            sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim();
                            sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim();

                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg3_add_blankrows();

                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        i = 0;
                        for (i = 0; i < sg1.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = (i + 1);
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();

                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        if (edmode.Value == "Y")
                        {
                            //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }
                        else
                        {
                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }

                        sg1_add_blankrows();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "REFRESH":
                    ////if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CONFIRM") == "2")
                    ////{
                    ////    set_Val();
                    ////    DataTable dtNet4 = new DataTable();
                    ////    string cssIP = "118.67.250.52";
                    ////    cssIP = "213.136.94.9";//103.47.13.64
                    ////    cssIP = "164.52.208.47";
                    ////    using (System.Data.OracleClient.OracleConnection con1 = new System.Data.OracleClient.OracleConnection(fin_wTejaxo_DLL.ConnInfo.connStringSys("SYSTEM", "LEADER", cssIP, "XE")))
                    ////    {
                    ////        System.Data.OracleClient.OracleDataAdapter adp = new System.Data.OracleClient.OracleDataAdapter("SELECT * FROM FINTEST." + frm_tabname + "", con1);
                    ////        adp.Fill(dtNet4);
                    ////    }

                    ////    if (dtNet4.Rows.Count > 0)
                    ////    {
                    ////        dt2 = new DataTable();
                    ////        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT * FROM " + frm_tabname);
                    ////        string saveFull = "Y";
                    ////        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME = '" + frm_tabname + "_OLD'", "TNAME");
                    ////        if (col1 != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "DROP TABLE " + frm_tabname + "_OLD");
                    ////        fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE " + frm_tabname + "_OLD AS SELECT * FROM " + frm_tabname + "");

                    ////        if (dt2.Rows.Count <= 0) saveFull = "N";

                    ////        int c = dtNet4.Columns.Count;
                    ////        dt = new DataTable();
                    ////        DataRow dr = null;
                    ////        dt = dtNet4.Clone();
                    ////        foreach (DataRow drNet4 in dtNet4.Rows)
                    ////        {
                    ////            col1 = "";
                    ////            if (saveFull == "Y")
                    ////            {
                    ////                col1 = fgen.seek_iname_dt(dt2, "FRM_NAME='" + drNet4["FRM_NAME"].ToString().Trim() + "' and FRM_TITLE='" + drNet4["FRM_TITLE"].ToString().Trim() + "'", "FRM_NAME");
                    ////            }
                    ////            if (col1.Length < 2)
                    ////            {
                    ////                dr = dt.NewRow();
                    ////                foreach (DataColumn dcNet4 in dtNet4.Columns)
                    ////                {
                    ////                    dr[dcNet4.ColumnName] = drNet4[dcNet4.ColumnName].ToString().Trim();
                    ////                }
                    ////                dt.Rows.Add(dr);
                    ////            }
                    ////        }
                    ////        if (dt.Rows.Count > 0)
                    ////        {
                    ////            oDS = new DataSet();
                    ////            oDS.Tables.Add(dt);
                    ////            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                    ////        }

                    ////        fgen.msg("-", "AMSG", "Data Refreshed Successfully!!");
                    ////    }
                    ////    else fgen.msg("-", "AMSG", "Table Does Not Exist!!'13'#" + frm_tabname);
                    ////}
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select a.ORDNO as PO_No,to_char(a.oRDDT,'dd/mm/yyyy') as Dated,decode(substr(trim(a.app_by),1,1),'-','(Un Approved)','(','(Cancel/Rej)','(Approved)') as Doc_Status,a.bank as Department,c.Aname as Supplier,b.iname as Item_Name,b.cpartno as Part_No,a.qtyord as Req_Qty,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt,a.app_by,a.app_dt  from " + frm_tabname + " a, item b,famst c where a.type='" + frm_vty + "' and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.srno ";
            SQuery = "SELECT branchcd,type,vchnum,vchdate,srno FROM " + frm_tabname + "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            if (txtlbl4.Text.Trim().Length < 2)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Party Not Filled Correctly !!");
                return;
            }

            int i = 0;
            for (i = 0; i < sg1.Rows.Count - 0; i++)
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) < 0 || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) < 0 || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text) < 0 || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text) < 0 || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text) < 0))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Amounts Not Filled Correctly at Line " + (i + 1) + "  !!");
                    return;
                }

                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) <= 0))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Either Debit / Credit Amounts Not Filled Correctly at Line " + (i + 1) + "  !!");
                    return;
                }

                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) > 0 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) > 0))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Either Debit / Credit Amounts Should be Filled Correctly at Line " + (i + 1) + "  !!");
                    return;
                }

                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.CheckIsDate((((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text)) == false)
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date Not Filled Correctly at Line " + (i + 1) + "  !!");
                    return;
                }

            }

            //for (i = 0; i < sg1.Rows.Count - 0; i++)
            //{
            //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) <= 0)
            //    {
            //        Checked_ok = "N";
            //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
            //        i = sg1.Rows.Count;
            //    }
            //}


            i = 0;
            hffield.Value = "";

            setColHeadings();

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y" && Checked_ok == "Y")
            {
                try
                {
                    oDS = new DataSet();
                    oporow = null;
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);



                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_fun();
                    //save_fun2();


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
                        save_it = "N";
                        for (i = 0; i < sg1.Rows.Count - 0; i++)
                        {
                            if (sg1.Rows[i].Cells[13].Text.Trim().Length >= 2)
                            {
                                save_it = "Y";
                            }
                        }

                        if (save_it == "Y")
                        {

                            i = 0;


                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' ", 6, "vch");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' ", 6, "vch");
                                    pk_error = "N";
                                    i = 0;
                                }
                                i++;
                            }
                            while (pk_error == "Y");
                        }
                    }

                    // If Vchnum becomes 000000 then Re-Save
                    if (frm_vnum == "000000") btnhideF_Click(sender, e);

                    save_fun();
                    //save_fun2();

                    if (edmode.Value == "Y")
                    {
                        cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||trim(Acode)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                    }

                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                        cmd_query = "delete from " + frm_tabname + " where branchcd||trim(Acode)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "Hello", "test Mail");
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Saved");
                        }
                    }
                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                    ViewState["sg1"] = null;
                }
                catch (Exception ex)
                {


                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            #endregion
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_h1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h10", typeof(string)));

        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));

        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));

    }
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field

        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));

    }

    public void create_tab3()
    {


        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field

        sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));

    }

    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt == null) return;
        sg1_dr = sg1_dt.NewRow();
        sg1_dr["sg1_h1"] = "-";
        sg1_dr["sg1_h2"] = "-";
        sg1_dr["sg1_h3"] = "-";
        sg1_dr["sg1_h4"] = "-";
        sg1_dr["sg1_h5"] = "-";
        sg1_dr["sg1_h6"] = "-";
        sg1_dr["sg1_h7"] = "-";
        sg1_dr["sg1_h8"] = "-";
        sg1_dr["sg1_h9"] = "-";
        sg1_dr["sg1_h10"] = "-";

        sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;


        sg1_dr["sg1_f1"] = "-";
        sg1_dr["sg1_f2"] = "-";
        sg1_dr["sg1_f3"] = "-";
        sg1_dr["sg1_f4"] = "-";
        sg1_dr["sg1_f5"] = "-";

        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "-";
        sg1_dr["sg1_t4"] = "-";
        sg1_dr["sg1_t5"] = "-";
        sg1_dr["sg1_t6"] = "-";
        sg1_dr["sg1_t7"] = "-";
        sg1_dr["sg1_t8"] = "-";
        sg1_dr["sg1_t9"] = "-";
        sg1_dr["sg1_t10"] = "-";
        sg1_dr["sg1_t11"] = "-";
        sg1_dr["sg1_t12"] = "-";
        sg1_dr["sg1_t13"] = "-";
        sg1_dr["sg1_t14"] = "-";
        sg1_dr["sg1_t15"] = "-";
        sg1_dr["sg1_t16"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();


        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }
    public void sg3_add_blankrows()
    {
        sg3_dr = sg3_dt.NewRow();

        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
        sg3_dr["sg3_f1"] = "-";
        sg3_dr["sg3_f2"] = "-";
        sg3_dr["sg3_t1"] = "-";
        sg3_dr["sg3_t2"] = "-";
        sg3_dr["sg3_t3"] = "-";
        sg3_dr["sg3_t4"] = "-";

        sg3_dt.Rows.Add(sg3_dr);
    }


    //------------------------------------------------------------------------------------
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
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;


            case "SG1_ROW_ADD":

                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Account", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);

                    sg1RowAdd(txtlbl4.Text, txtlbl4a.Text);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG2_RMV":
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG2_ROW_ADD":
                dt = new DataTable();
                sg2_dt = new DataTable();
                dt = (DataTable)ViewState["sg2"];
                z = dt.Rows.Count - 1;
                sg2_dt = dt.Clone();
                sg2_dr = null;
                i = 0;
                for (i = 0; i < sg2.Rows.Count; i++)
                {
                    sg2_dr = sg2_dt.NewRow();
                    sg2_dr["sg2_srno"] = (i + 1);
                    sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                    sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                    sg2_dt.Rows.Add(sg2_dr);
                }
                sg2_add_blankrows();
                ViewState["sg2"] = sg2_dt;
                sg2.DataSource = sg2_dt;
                sg2.DataBind();
                break;
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":

                break;
            case "SG3_ROW_ADD":

                break;
        }
    }

    //------------------------------------------------------------------------------------

    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account ", frm_qstr);
    }
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl20_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl21_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl22_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl23_Click(object sender, ImageClickEventArgs e)
    {

    }

    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.ToUpper().Length >= 1 && ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.ToUpper() != "-")
            {

                oporow = oDS.Tables[0].NewRow();

                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["VCHNUM"] = frm_vnum;
                oporow["VCHDATE"] = txtvchdate.Text.Trim();
                oporow["ACODE"] = txtlbl4.Text;

                switch (Prg_Id)
                {
                    case "F70801":
                    case "F70802":

                        oporow["INVNO"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.ToUpper();
                        oporow["INVDATE"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text, vardate);

                        oporow["DRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        oporow["CRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);


                        oporow["FC_TYPE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.ToUpper();

                        oporow["FCTYPE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.ToUpper();
                        oporow["FCRATE"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text);
                        oporow["FCDRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        oporow["FCCRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);


                        oporow["REFNUM"] = "-";

                        oporow["NARATION"] = "-";
                        oporow["VCHID"] = i + 1;
                        oporow["ENT_BY"] = frm_uname;
                        oporow["ENT_DATE"] = vardate;
                        oporow["OSCL"] = 0;

                        break;
                    case "F70803":

                        oporow["INVNO"] = frm_vnum + "/" + i;
                        oporow["INVDATE"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text, vardate);
                        oporow["RCODE"] = sg1.Rows[i].Cells[13].Text.Trim();

                        oporow["refnum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
                        oporow["refdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text, vardate);

                        oporow["DRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        oporow["CRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
                        oporow["NARATION"] = "-";
                        oporow["FCTYPE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.ToUpper();
                        oporow["FCRATE"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text);
                        oporow["FCDRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        oporow["FCCRAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);

                        oporow["TAX"] = "-";
                        oporow["STAX"] = 0;
                        oporow["POST"] = 0;
                        oporow["PROJCD"] = "-";


                        oporow["DEPTT"] = "-";
                        oporow["REF1"] = "-";
                        oporow["STFORM"] = "-";
                        oporow["ST_ENTFORM"] = "-";

                        oporow["WEIGHT"] = "-";
                        oporow["QUANTITY"] = 0;
                        oporow["GRNO"] = "-";
                        oporow["GRDATE"] = vardate;
                        oporow["MRNNUM"] = "-";

                        if (edmode.Value == "Y")
                        {
                            if (fgen.CheckIsDate((((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text)) == false)
                            {
                                //oporow["BANK_DATE"] = null;
                            }
                            else
                            {
                                oporow["BANK_DATE"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text, vardate);
                            }
                        }
                        else
                        {
                            //oporow["BANK_DATE"] = null;
                        }

                        oporow["APP_BY"] = "-";
                        oporow["APP_DATE"] = vardate;


                        if (edmode.Value == "Y")
                        {
                            oporow["eNt_by"] = ViewState["entby"].ToString();
                            oporow["eNt_date"] = ViewState["entdt"].ToString();
                            oporow["edt_by"] = frm_uname;
                            oporow["edt_date"] = vardate;

                        }
                        else
                        {
                            oporow["eNt_by"] = frm_uname;
                            oporow["eNt_date"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["eDt_date"] = vardate;

                        }





                        oporow["PFLAG"] = "-";
                        oporow["DEPCD"] = "-";
                        oporow["COSTCD"] = "-";
                        oporow["RG23NO"] = "-";
                        oporow["MRNDATE"] = vardate;
                        oporow["SRNO"] = i;
                        oporow["FCRATE1"] = 0;
                        oporow["CCENT"] = "-";

                        oporow["OSCL"] = 0;
                        oporow["TFCR"] = 0;
                        oporow["TFCDR"] = 0;
                        oporow["TFCCR"] = 0;
                        break;

                }

                oDS.Tables[0].Rows.Add(oporow);

            }
        }
    }
    void save_fun2()
    {

    }
    void save_fun3()
    {

    }
    void save_fun4()
    {


    }
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {

            case "F70801":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "16");
                break;
            case "F70802":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "06");
                break;

        }
    }
    //------------------------------------------------------------------------------------   
    void sg1RowAdd(string col1, string col2)
    {
        #region for gridview 1
        if (col1.Length <= 0) return;
        if (ViewState["sg1"] != null)
        {
            dt = new DataTable();
            sg1_dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            z = dt.Rows.Count - 1;
            sg1_dt = dt.Clone();
            sg1_dr = null;
            for (i = 0; i < dt.Rows.Count - 1; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                sg1_dt.Rows.Add(sg1_dr);
            }

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
            sg1_dr["sg1_h1"] = col1;
            sg1_dr["sg1_h2"] = col2;
            sg1_dr["sg1_h3"] = "-";
            sg1_dr["sg1_h4"] = "-";
            sg1_dr["sg1_h5"] = "-";
            sg1_dr["sg1_h6"] = "-";
            sg1_dr["sg1_h7"] = "-";
            sg1_dr["sg1_h8"] = "-";
            sg1_dr["sg1_h9"] = "-";
            sg1_dr["sg1_h10"] = "-";

            sg1_dr["sg1_f1"] = col1;
            sg1_dr["sg1_f2"] = col2;
            sg1_dr["sg1_f3"] = col1;
            sg1_dr["sg1_f4"] = "-";
            sg1_dr["sg1_f5"] = "-";

            sg1_dr["sg1_t1"] = "";
            sg1_dr["sg1_t2"] = frm_CDT1;
            sg1_dr["sg1_t3"] = "";
            sg1_dr["sg1_t4"] = "";
            sg1_dr["sg1_t5"] = "";
            sg1_dr["sg1_t6"] = "";
            sg1_dr["sg1_t7"] = "";
            sg1_dr["sg1_t8"] = "";

            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1_add_blankrows();

        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        dt.Dispose(); sg1_dt.Dispose();
        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
        #endregion
        setColHeadings();
    }
    protected void btnupload_Click(object sender, EventArgs e)
    {
        create_tab();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        string ext = "", excelConString = "", filesavepath = "";
        if (FileUpload1.HasFile)
        {
            ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext == ".xls" || ext == ".xlsx")
            {
                if (ext == ".xls")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filesavepath + ";Extended Properties=Excel 8.0;";
                }
                else if (ext == ".xlsx")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xlsx";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                }

                OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
                OleDbConn.Open();
                DataTable dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                OleDbConn.Close();
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                OleDbCommand OleDbCmd = new OleDbCommand();
                String Query = "";
                Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                OleDbCmd.CommandText = Query;
                OleDbCmd.Connection = OleDbConn;
                OleDbCmd.CommandTimeout = 0;
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = OleDbCmd;
                objAdapter.SelectCommand.CommandTimeout = 0;
                dt = null;
                dt = new DataTable();
                objAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = i + 1;
                        sg1_dr["sg1_h1"] = dt.Rows[i][3].ToString().Trim();
                        sg1_dr["sg1_h2"] = dt.Rows[i][4].ToString().Trim();
                        sg1_dr["sg1_h3"] = dt.Rows[i][3].ToString().Trim();
                        sg1_dr["sg1_h4"] = "-";
                        sg1_dr["sg1_h5"] = "-";
                        sg1_dr["sg1_h6"] = "-";
                        sg1_dr["sg1_h7"] = "-";
                        sg1_dr["sg1_h8"] = "-";
                        sg1_dr["sg1_h9"] = "-";
                        sg1_dr["sg1_h10"] = "-";

                        sg1_dr["sg1_f1"] = dt.Rows[i][3].ToString().Trim();
                        sg1_dr["sg1_f2"] = dt.Rows[i][4].ToString().Trim();
                        sg1_dr["sg1_f3"] = dt.Rows[i][3].ToString().Trim();
                        sg1_dr["sg1_f4"] = "-";
                        sg1_dr["sg1_f5"] = "-";

                        sg1_dr["sg1_t1"] = dt.Rows[i][5].ToString().Trim();
                        sg1_dr["sg1_t2"] = dt.Rows[i][6].ToString().Trim();
                        sg1_dr["sg1_t3"] = dt.Rows[i][8].ToString().Trim();
                        sg1_dr["sg1_t4"] = dt.Rows[i][7].ToString().Trim();
                        sg1_dr["sg1_t5"] = dt.Rows[i][9].ToString().Trim();
                        sg1_dr["sg1_t6"] = dt.Rows[i][10].ToString().Trim();

                        sg1_dr["sg1_t7"] = dt.Rows[i][11].ToString().Trim();
                        sg1_dr["sg1_t8"] = dt.Rows[i][12].ToString().Trim();



                        sg1_dt.Rows.Add(sg1_dr);
                    }
                    sg1_add_blankrows();

                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    setColHeadings();
                }
            }
            else fgen.msg("-", "AMSG", "File must be in xls or xlsx format!!");
        }
    }
    protected void btnRef_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "REFRESH";
        fgen.open_pwdbox("-", frm_qstr);
    }
}