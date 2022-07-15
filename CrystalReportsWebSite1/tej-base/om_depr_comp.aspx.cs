using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_depr_comp : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", eff_dt = "";
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
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
                    //frm_mbr = "01";
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                lblheader.Text = "Depreciation Calculator-Companies Act,2013";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            lbl1a.Visible = false;
            typePopup = "N";
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

        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = true;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false; btnprint.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btncal.Enabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

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
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true; btnprint.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btncal.Enabled = true;
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
        frm_tabname = "WB_FA_VCH";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "30");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
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
            case "TACODE":
                break;
            case "MRESULT":
                break;

            case "TICODE":
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                //pop3
                // to avoid repeat of item
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }

                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;
            case "New":
            case "Edit":
            case "Del":
                Type_Sel_query();
                break;

            case "Print":
                SQuery = "SELECT 'YES' AS FSTR,'YES' AS option_,'Do You want to see all assets including assets for which dep calculated is 0'  as text from dual union all SELECT 'NO' AS FSTR,'NO' AS option_,'Do You want to see all assets including assets for which dep calculated is 0'  as text from dual";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')||type as fstr,vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,type from WB_FA_VCH where branchcd='" + frm_mbr + "' and type='30' and VCHDATE " + DateRange + " AND  vchnum<>'000000' order by vchnum desc";
                break;
        }
        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del"))
        {
            btnval = btnval + "_E";
            hffield.Value = btnval;
            make_qry_4_popup();
        }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";

            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            switch (Prg_Id)
            {
                case "":
                    frm_vty = "10";
                    break;
            }
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
        btnsave.Disabled = true;
    }
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Value = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        string lastdepdt = "";
        lastdepdt = fgen.seek_iname(frm_qstr, frm_cocd, "select max(VCHDATE) as lastdt from WB_FA_VCH where type='" + frm_vty + "' and branchcd='" + frm_mbr + "'", "lastdt");
        if ((lastdepdt == "0") || (lastdepdt == "-") || (lastdepdt == ""))
        {
            if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
            {
                lastdepdt = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt where opt_id='W1076'", "opt_param");
                DateTime dlastdt = Convert.ToDateTime(lastdepdt).AddDays(-1);
                var vlastdt = dlastdt.ToShortDateString();
                txtlastdt.Text = vlastdt.Trim();
            }
            else
            {
                DateTime dlastdt = Convert.ToDateTime(frm_CDT1).AddDays(-1);
                var vlastdt = dlastdt.ToShortDateString();
                txtlastdt.Text = vlastdt.Trim();
            }
        }
        else
        {
            txtlastdt.Text = lastdepdt;
        }
        disablectrl();
        fgen.EnableForm(this.Controls);

        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        int j;
        //for (j = i; j < 10; j++)
        //{
        //    sg1_add_blankrows();
        //}
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";        
        #endregion
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            btnsave.Disabled = true; btncal.Enabled= false;
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
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
        //string chk_freeze = "";
        ////chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1043", txtvchdate.Text.Trim());
        //if (chk_freeze == "1")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
        //    return;
        //}
        //if (chk_freeze == "2")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
        //    return;
        //}

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        int dhd1 = fgen.ChkDate(txtlastdt.Text.ToString());
        if (dhd1 == 0)
        { fgen.msg("-", "AMSG", "Last Depreciation Date not Valid"); txtvchdate.Focus(); return; }
        if (txtvchdate.Text.ToString() == txtlastdt.Text.ToString())
        { fgen.msg("-", "AMSG", "Depreciation and Last Depreciation Date cannot be same."); txtvchdate.Focus(); return; }
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
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
        //checkcode rkvsv
        hffield.Value = "Print";
        //fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);

        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Depreciation Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
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
                //SQuery = fgen.seek_iname(frm_qstr, frm_cocd, "select max(nvl(trim(a.post),'-')) as post from WB_FA_VCH a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 16) + "'","post");
                //if (SQuery == "Y")
                //{
                //    fgen.msg("-", "AMSG", "Entry Posted in Accounts Module.'13' Deletion is not allowed");
                //    return;
                //}
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 16) + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
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
                    newCase(col1);
                    btnsave.Disabled = true;
                    break;
                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join FIN_MSYS b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl7.Value = dt.Rows[0]["ent_id"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";

                            sg1_dr["sg1_t1"] = dt.Rows[i]["OBJ_NAME"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["OBJ_CAPTION"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["OBJ_WIDTH"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["OBJ_VISIBLE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["col_no"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["obj_maxlen"].ToString().Trim();
                            sg1_dr["sg1_t7"] = "";

                            if (frm_tabname.ToUpper() == "SYS_CONFIG")
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[i]["OBJ_READONLY"].ToString().Trim();
                            }

                            sg1_dr["sg1_t8"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                    }
                    #endregion
                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
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
                    hf1.Value = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "select a.* from WB_FA_VCH a where trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')||a.type ='" + col1 + "' and a.branchcd='" + frm_mbr + "' order by a.vchnum,a.vchdate,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Value = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        //txtvchdate.Enabled = true;
                        //btncal.Enabled = true;
                        
                        DataTable fill_depgrid = new DataTable();
                        fill_depgrid = fgen.getdata(frm_qstr, frm_cocd, "SELECT cramt, acode from WB_FA_VCH where branchcd='" + frm_mbr + "' and type='30' AND trim(vchnum)='" + txtvchnum.Value.Trim() + "' and TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + txtvchdate.Text.Trim() + "' order by acode");
                        create_tab();
                        sg1_dr = null;
                        double totassetval=0,totadd=0,totdep=0;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            double mq0 = 0, mq1 = 0, mq2 = 0;
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
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

                            sg1_dr["sg1_f1"] = dt.Rows[i]["deprdays"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["depr"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["grpcode"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["mrr_ref"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["assetval"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["ASSETVAL1"].ToString().Trim();//swap iqtyin with asseval1
                            sg1_dr["sg1_t3"] = dt.Rows[i]["IQTYOUT"].ToString().Trim();//
                            sg1_dr["sg1_t4"] = dt.Rows[i]["ASSETVAL1"].ToString().Trim();
                            sg1_dr["sg1_t6"] = fgen.seek_iname_dt(fill_depgrid, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "'", "cramt");
                            sg1_dt.Rows.Add(sg1_dr);
                            mq0=fgen.make_double(dt.Rows[i]["assetval"].ToString().Trim());
                            mq1=fgen.make_double(dt.Rows[i]["ASSETVAL1"].ToString().Trim());
                            mq2=fgen.make_double(fgen.seek_iname_dt(fill_depgrid, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "'", "cramt")) ;
                            totassetval=totassetval+mq0;
                            totadd= totadd+mq1;
                            totdep= totdep+ mq2;
                        }
                        txtlbl4.Value= totassetval.ToString();
                        txtlbl7.Value=totadd.ToString();
                        txtlbl101.Value = totdep.ToString();

                        //int j;
                        //for (j = i; j < 30; j++)
                        //{
                        //    sg1_add_blankrows();
                        //}
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        btnsave.Disabled = true; btncal.Enabled = false; 
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Value = col1;
                    }
                    dt.Dispose();
                    SQuery = "select  EMPCODE AS COL1,NAME AS COL2, DEPTT_TEXT AS COL3,DESG_TEXT AS COL4,TO_CHAR(DTJOIN,'dd/MM/yyyy') AS COL6,ENT_DT,ENT_BY from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
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
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["col6"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
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

                    break;
                case "MRESULT":

                    if (col1.Length <= 0) return;
                    txtlbl101.Value = col1;
                    //txtlbl101a.Text = col2;
                    break;

                case "SG1_ROW_ADD":
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    setColHeadings();
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
                    setColHeadings();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        switch (Prg_Id)
        {
            case "F30111":
                frm_vty = "20";
                break;
        }
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select grpcode as  Groupcode,acode as Assetcode, mrr_ref as Asset_name,assetval as Original_cost,assetval1 as Addition, cramt as CurrentDepreciation,vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate,ent_by as Entryby ,  To_char(ent_dt,'dd/mm/yyyy') as EntryDate  from WB_FA_VCH WHERE branchcd='" + frm_mbr + "' and TYPE='30' AND  vchdate " + PrdRange + "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "Print" || hffield.Value == "Print_E")
        {
            
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);///
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "branchcd='"+frm_mbr+"'");//
            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70404");
            fgen.fin_acct_reps(frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            string last_entdt;
            //checks
            if (edmode.Value == "Y")
            {
            }
            else
            {
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
                if (last_entdt == "0")
                { }
                else
                {
                    string convdt = (Convert.ToDateTime(txtvchdate.Text)).ToString("dd/MM/yyyy");
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(convdt))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!"); txtvchdate.Focus();
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!"); txtvchdate.Focus();
            }
            //-----------------------------
            i = 0;
            hffield.Value = "";

            setColHeadings();

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

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Value.Trim();
                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {
                                {
                                    save_it = "Y";
                                }
                            }

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
                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname);


                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
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
                sg1.HeaderRow.Cells[20].Width = 140;
            }

        }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                break;


            case "SG1_ROW_ADD":
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
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

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":
                if (index < sg3.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG3_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG3_ROW_ADD":
                if (index < sg3.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG3_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG3_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }

    //------------------------------------------------------------------------------------

    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade ", frm_qstr);
    }
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MRESULT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr,frm_cocd, "select to_char(instdt,'dd/mm/yyyy') as vinstdt, trim(acode) as acode from wb_fa_pur where branchcd ='" + frm_mbr + "' and type='10'");
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {

            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["SRNO"] = i;
            oporow["grpcode"] = sg1.Rows[i].Cells[15].Text.Trim();
            oporow["acode"] = sg1.Rows[i].Cells[17].Text.Trim();
            oporow["assetval"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);// assets b/f
            oporow["assetval1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text); // current addition
            oporow["depr"] = sg1.Rows[i].Cells[14].Text.Trim();
            oporow["deprdays"] = sg1.Rows[i].Cells[13].Text.Trim();
            oporow["cramt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text);
            oporow["naration"] = "Depreciation @" + sg1.Rows[i].Cells[14].Text.Trim();
            oporow["sale_ent"] = "N";
            oporow["iunit"] = "-";
            oporow["mrr_ref"] = sg1.Rows[i].Cells[16].Text.Trim();
            string vinstdt = fgen.seek_iname_dt(dt2, "acode='" + sg1.Rows[i].Cells[17].Text.Trim() + "'", "vinstdt");
            oporow["instdt"] = vinstdt;

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"].ToString();
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
            case "F30111":
                SQuery = "SELECT '20' AS FSTR,'Quality Inward Certificate' as NAME,'20' AS CODE FROM dual";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "20");
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }


    protected void txt_TextChanged(object sender, EventArgs e)
    {
    }
    //------------------------------------------------------------------------------------   
    protected void btncal_Click(object sender, EventArgs e)
    {

        if (txtvchnum.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please press the new button");
            txtvchdate.Focus(); return;
        }

        string lastdepdt = "", abcdfld = "";
        string popsql = "";
        string new_depr = "Y", Msgresult = "1", xmainq = "";
        
        // code to calculate  depreciation part.
        int days = 0;
        int currdays = 0;

        int month = Convert.ToInt32(txtvchdate.Text.Substring(3, 2));


        if (month >= 04)
        {
            if (Convert.ToInt32(txtvchdate.Text.Substring(6, 4)) > Convert.ToInt32(frm_myear))
            {
                fgen.msg("-", "AMSG", "Please Select a Valid Date within the financial year logged -In");
                txtvchdate.Focus(); return;
            }
        }
        else
        {
            if (Convert.ToInt32(txtvchdate.Text.Substring(6, 4)) < Convert.ToInt32(frm_myear))
            {
                fgen.msg("-", "AMSG", "Please Select a Valid Date within the financial year logged -In"); txtvchdate.Focus(); return;
            }

        }
        if (txtlastdt.Text == "")
        {

            fgen.msg("-", "AMSG", "Please enter a Valid last  Depreciation Date or 31st March of Last financial year"); txtlastdt.Focus();

            return;


        }

        int dhd1 = fgen.ChkDate(txtlastdt.Text);
        int dhd2 = fgen.ChkDate(txtvchdate.Text);
        if ((dhd1 == 0) || (dhd2 == 0))
        {
            fgen.msg("-", "AMSG", "Invalid date format");
            txtlastdt.Focus();

            return;
        }

        string chklastdate = "";
        chklastdate = fgen.seek_iname(frm_qstr, frm_cocd, "select to_date('" + frm_CDT1 + "','dd/MM/yyyy')-1  as dd from dual", "dd");

        if (Convert.ToDateTime(txtlastdt.Text.Trim()) < Convert.ToDateTime(chklastdate))
        {
            fgen.msg("-", "AMSG", "Please Select a Valid last  Depreciation Date. Depreciation can't be calculated for more than a year");
            txtlastdt.Focus();
            return;
        }

        DataTable dt = new DataTable();
        SQuery = "select trim(acode) as acode,sum(Cramt) as tot from WB_FA_VCH where branchcd='" + frm_mbr + "' and to_date(sale_dt,'dd/mm/yyyy') < to_DaTE('" + txtvchdate.Text + "','dd/mm/yyyy') and type!='30' group by trim(Acode)";

        # region old code dont remove
        if (Msgresult == "1")
        {
            if (new_depr == "Y")
            {

                lastdepdt = fgen.seek_iname(frm_qstr, frm_cocd, "select max(VCHDATE) as lastdt from WB_FA_VCH where type='" + frm_vty + "' and cramt >0 and branchcd='" + frm_mbr + "'", "lastdt");
            }

            if (lastdepdt == "0")
            {
                lastdepdt = txtlastdt.Text.Trim();
            }
            if ((txtlastdt.Text.Trim() == "") && (lastdepdt == "0"))
            {
                lastdepdt = frm_CDT1;
            }
            // if user wants on installdt 
            abcdfld = "b.instdt";
            popsql = "select trim(a.acode),b.grpcode,b.assetname,b.instdt as vchdate,sum(a.dramt) as debit,sum(a.cramt) as credit,b.op_dep from WB_FA_VCH a,wb_fa_pur b where a.branchcd='" + frm_mbr + "'  and a.type='30' and b.type='10' and (a.branchcd||trim(a.acode)=b.branchcd||trim(b.acode)) and (a.branchcd||trim(a.acode)) not in (select branchcd||trim(acode) from wb_fa_vch where branchcd='" + frm_mbr + "'  AND type='20' and to_date(sale_dt,'dd/mm/yyyy')>to_Date('" + txtvchdate.Text + "','dd/mm/yyyy') and to_date(sale_dt,'dd/mm/yyyy') < to_Date('" + txtlastdt.Text + "','dd/mm/yyyy')) group by b.op_dep,a.acode,b.assetname,b.instdt,b.grpcode order by a.acode ";
        }
        xmainq = popsql;
        popsql = "select * from (" + popsql + ") where debit-credit>0 order by grpcode,acode";

        #endregion
        //fetching last depreciation date from table
        //new

        SQuery = "select a.branchcd||trim(a.acode)as fstr,b.op_dep as op_depr, b.life_end, b.instdt,b.original_cost as    origcost,b.sale_dt,trim(b.grpcode) as grpcode,b.deprpday ,trim(b.assetname) as aname,a.branchcd,trim(a.acode) as a_code,sum(a.purch) as purch_Val ,sum(a.sold) As sale_val,sum(a.purch)-sum(a.sold) as bal_Val from (select branchcd,acode,quantity as purch ,0 as sold from wb_fa_pur where branchcd='" + frm_mbr + "' and instdt < = to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and life_end >  to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') union all select branchcd,acode,0 as orig, iqtyout as salevalue from wb_fa_vch where branchcd='" + frm_mbr + "' and type='20' and TO_DATE(sale_dt,'DD/MM/YYYY') <= to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') )a , wb_fa_pur b where trim(A.branchcd)||trim(a.acode)=trim(B.branchcd)||trim(B.acode) group by b.op_dep , b.life_end, b.instdt,b.original_cost,b.sale_dt,trim(b.grpcode),b.deprpday,trim(b.assetname),a.branchcd,trim(a.acode) ,a.branchcd||trim(a.acode) having (sum(a.purch)-sum(a.sold)) >0    union all   select a.branchcd||trim(a.acode)as fstr,       0 as op_depr, b.life_end, a.instdt,a.dramt - a.cramt as origcost,b.sale_dt,trim(b.grpcode) as grpcode,a.deprdays,trim(b.assetname) as aname,a.branchcd,trim(a.acode) as a_code,1 as purch_Val ,0 As sale_val,1 as bal_Val from wb_fa_vch a, wb_fa_pur b where a.branchcd='" + frm_mbr + "' and a.type='50' and a.instdt < = to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and a.instdt >  to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')  and trim(A.branchcd)||trim(a.acode)=trim(B.branchcd)||trim(B.acode)";

        ////original
        //SQuery = "select a.branchcd||trim(a.acode)as fstr,b.op_dep as op_depr, b.life_end, b.instdt,b.original_cost as origcost,b.sale_dt,trim(b.grpcode) as grpcode,b.deprpday,trim(b.assetname) as aname,a.branchcd,trim(a.acode) as a_code,sum(a.purch) as purch_Val ,sum(a.sold) As sale_val,sum(a.purch)-sum(a.sold) as bal_Val from (select branchcd,acode,quantity as purch ,0 as sold from wb_fa_pur where branchcd='" + frm_mbr + "' and instdt < = to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and life_end >  to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') union all select branchcd,acode,0 as orig, iqtyout as salevalue from wb_fa_vch where branchcd='" + frm_mbr + "' and type='20' and TO_DATE(sale_dt,'DD/MM/YYYY') <= to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') )a , wb_fa_pur b where trim(A.branchcd)||trim(a.acode)=trim(B.branchcd)||trim(B.acode) group by b.op_dep , b.life_end, b.instdt,b.original_cost,b.sale_dt,trim(b.grpcode),b.deprpday,trim(b.assetname),a.branchcd,trim(a.acode) ,a.branchcd||trim(a.acode) having (sum(a.purch)-sum(a.sold)) >0 order by trim(a.acode)";

        //SQuery1 = "SELECT A.branchcd||A.acode as fstr,A.BRANCHCD,A.ACODE,A.DEPRDAYS,MAX(TO_CHAR(A.INSTDT,'DD/MM/YYYY')) AS EFFECTIVE_DATE, (TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY')-A.INSTDT)AS DEPRDAY,((TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY')-A.INSTDT)*A.DEPRDAYS) AS DEPR  FROM WB_FA_VCH A,WB_FA_PUR B  where A.branchcd='" + frm_mbr + "' and  A.TYPE='50' AND A.instdt between to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + txtvchdate.Text + "','dd/mm/yyyy') and B.life_end >  to_date('" + txtvchdate.Text + "','dd/mm/yyyy') AND A.branchcd||A.VCHNUM = B.branchcd||B.VCHNUM GROUP BY A.BRANCHCD,A.ACODE,A.DEPRDAYS,(TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY')-A.INSTDT),((TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY')-A.INSTDT)*A.DEPRDAYS)";
        //SQuery1 = "SELECT A.branchcd||A.acode as fstr,A.BRANCHCD,A.ACODE,A.DEPRDAYS,TO_CHAR(A.fvchdate,'DD/MM/YYYY') AS EFFECTIVE_DATE,a.dramt ROM WB_FA_VCH A,WB_FA_PUR B  where A.branchcd='" + frm_mbr + "' and  A.TYPE='50' and B.life_end >  to_date('" + txtvchdate.Text + "','dd/mm/yyyy') AND A.branchcd||A.VCHNUM = B.branchcd||B.VCHNUM GROUP BY A.BRANCHCD,A.ACODE,A.DEPRDAYS,(TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY')-A.fvchdate),((TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY')-A.fvchdate)*A.DEPRDAYS)";
        
        //DataTable dtadd_depr = new DataTable();

        //dtadd_depr = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

        if (Convert.ToDateTime(lastdepdt) > Convert.ToDateTime(txtvchdate.Text))
        {
            fgen.msg("-", "AMSG", "Your Last Depreciation date cannot be greater than  current one.Last Dep. Date entered/database contains '" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "'"); txtlastdt.Focus();
            return;
        }

        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //fetching data from wb_fa_pur and wb_fa_vch
        if (dt.Rows.Count <= 0)
        {
            fgen.msg("-", "AMSG", " No Data found.");
            return;
        }
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToDateTime(lastdepdt) <= Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim()))
                {
                    if (dt.Rows[i]["sale_dt"].ToString() != "")
                    {

                        if (Convert.ToDateTime(dt.Rows[i]["sale_dt"].ToString()) > Convert.ToDateTime(lastdepdt))
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
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


                            sg1_dr["sg1_f2"] = dt.Rows[i]["deprpday"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["grpcode"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["ANAME"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["a_code"].ToString().Trim();

                            if (((Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()) > Convert.ToDateTime(lastdepdt)) && (Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()) <= Convert.ToDateTime(txtvchdate.Text.Trim())))
                                || ((Convert.ToDateTime(dt.Rows[i]["sale_dt"].ToString().Trim()) > Convert.ToDateTime(lastdepdt)) && (Convert.ToDateTime(dt.Rows[i]["sale_dt"].ToString().Trim()) <= Convert.ToDateTime(txtvchdate.Text.Trim()))))
                            {
                                sg1_dr["sg1_t2"] = fgen.make_double(dt.Rows[i]["origcost"].ToString().Trim());
                                sg1_dr["sg1_t1"] = 0;


                                if ((Convert.ToDateTime(txtvchdate.Text.Trim()) < Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim())) || (Convert.ToDateTime(dt.Rows[i]["sale_dt"].ToString().Trim()) <= Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim())))
                                {
                                    //currdays = Convert.ToDateTime(dt.Rows[i]["sale_dt"].ToString().Trim()) - Convert.ToDateTime(lastdepdt);
                                    currdays = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select  to_date('" + Convert.ToDateTime(dt.Rows[i]["sale_dt"].ToString().Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')-to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') as dd from dual", "dd"));
                                    sg1_dr["sg1_f1"] = currdays + 1;
                                }

                                else
                                {
                                    //currdays = Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim()) - Convert.ToDateTime(lastdepdt);
                                    currdays = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select  to_date('" + dt.Rows[i]["life_end"].ToString().Trim() + "','dd/MM/yyyy')-to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') as dd from dual", "dd"));
                                    sg1_dr["sg1_f1"] = currdays + 1;
                                }


                            }

                            else
                            {
                                sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["original_cost"].ToString().Trim());
                                sg1_dr["sg1_t2"] = 0;


                                if (Convert.ToDateTime(txtvchdate.Text.Trim()) > Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim()))
                                {
                                    days = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select  to_date('" + dt.Rows[i]["life_end"].ToString().Trim() + "','dd/MM/yyyy')-to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')-1 as dd from dual", "dd"));
                                    sg1_dr["sg1_f1"] = days;
                                }

                                else
                                {
                                    days = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select to_date('" + txtvchdate.Text.Trim() + "','dd/MM/yyyy') - to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') as dd from dual", "dd"));
                                    sg1_dr["sg1_f1"] = days;

                                }


                            }
                            // sg1_dr["sg1_t3"] = dt.Rows[i]["cramt"].ToString().Trim();
                            // sg1_dr["sg1_t4"] = Convert.ToDouble(sg1_dr["sg1_t1"]) + Convert.ToDouble(sg1_dr["sg1_t2"]) - Convert.ToDouble(sg1_dr["sg1_t3"]);
                            sg1_dr["sg1_t5"] = Convert.ToDouble(dt.Rows[i]["op_dep"].ToString().Trim());
                            sg1_dr["sg1_t6"] = 0;

                            sg1_dr["sg1_t7"] = Convert.ToDouble(dt.Rows[i]["BAL_VAL"].ToString().Trim()) * Convert.ToInt64(sg1_dr["sg1_f1"]) * Convert.ToDouble(dt.Rows[i]["deprpday"].ToString().Trim());

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        else
                        {

                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = 0;
                            sg1_dr["sg1_f2"] = dt.Rows[i]["deprpday"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["grpcode"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "ASSET SOLD";
                            sg1_dr["sg1_f5"] = dt.Rows[i]["a_code"].ToString().Trim();
                            //  sg1_dr["sg1_t1"] = "LIFE CONSUMED";
                            sg1_dt.Rows.Add(sg1_dr);

                        }

                    }

                    else
                    {
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
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

                        sg1_dr["sg1_f2"] = dt.Rows[i]["deprpday"].ToString().Trim();
                        sg1_dr["sg1_f3"] = dt.Rows[i]["grpcode"].ToString().Trim();
                        sg1_dr["sg1_f4"] = dt.Rows[i]["ANAME"].ToString().Trim();
                        sg1_dr["sg1_f5"] = dt.Rows[i]["a_code"].ToString().Trim();

                        if ((Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()) > Convert.ToDateTime(lastdepdt)) && (Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()) <= Convert.ToDateTime(txtvchdate.Text.Trim())))
                        {
                            sg1_dr["sg1_t2"] = fgen.make_double(dt.Rows[i]["origcost"].ToString().Trim());
                            sg1_dr["sg1_t1"] = 0;


                            if (Convert.ToDateTime(txtvchdate.Text.Trim()) < Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim()))
                            {
                                // currdays = Convert.ToDateTime(txtvchdate.Text.Trim()) - Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim());

                                string mq1 = "select  to_date('" + txtvchdate.Text.Trim() + "','dd/MM/yyyy')-to_date('" + Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') as dd from dual";
                                currdays = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, mq1, "dd"));
                                sg1_dr["sg1_f1"] = Convert.ToInt64(currdays) + 1;
                            }

                            else
                            {
                                currdays = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select  to_date('" + Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')-to_date('" + Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') as dd from dual", "dd"));
                                sg1_dr["sg1_f1"] = Convert.ToInt64(currdays) + 1;
                            }
                        }

                        else
                        {
                            sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["origcost"].ToString().Trim());
                            sg1_dr["sg1_t2"] = 0;

                            if (Convert.ToDateTime(txtvchdate.Text.Trim()) > Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim()))
                            {
                                days = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select  to_date('" + Convert.ToDateTime(dt.Rows[i]["life_end"].ToString().Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')-to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')-1 as dd from dual", "dd"));
                                sg1_dr["sg1_f1"] = days + 1;
                            }
                            else
                            {
                                days = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select to_date('" + txtvchdate.Text.Trim() + "','dd/MM/yyyy') - to_date('" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')as dd from dual", "dd"));
                                sg1_dr["sg1_f1"] = days;
                            }
                        }
                        sg1_dr["sg1_t5"] = Convert.ToDouble(dt.Rows[i]["op_depr"].ToString().Trim());
                        sg1_dr["sg1_t6"] = (Convert.ToDouble(dt.Rows[i]["BAL_VAL"].ToString().Trim()) * Convert.ToInt64(sg1_dr["sg1_f1"]) * Convert.ToDouble(dt.Rows[i]["deprpday"].ToString().Trim()));
                        sg1_dt.Rows.Add(sg1_dr);
                    }
                }
                else
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    sg1_dr["sg1_f1"] = 0;
                    sg1_dr["sg1_f2"] = dt.Rows[i]["deprpday"].ToString().Trim();
                    sg1_dr["sg1_f3"] = dt.Rows[i]["grpcode"].ToString().Trim();
                    sg1_dr["sg1_f4"] = "LIFE CONSUMED";
                    sg1_dr["sg1_f5"] = dt.Rows[i]["a_code"].ToString().Trim();
                    sg1_dt.Rows.Add(sg1_dr);
                }

            }

            ViewState["sg1"] = sg1_dt;
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            dt.Dispose(); sg1_dt.Dispose();

            txtlastdt.Text = lastdepdt;

            double d = 0, d1 = 0, d2 = 0;
            double totbf = 0;
            double totadd = 0;
            double totdepr = 0;
            if (sg1.Rows.Count >= 1)
            {
                for (int i = 0; i < sg1.Rows.Count; i++)
                {
                    totbf = totbf + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                    totadd = totadd + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim());
                    totdepr = totdepr + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());
                }
                txtlbl4.Value = Convert.ToString(Math.Round(totbf, 2));
                txtlbl7.Value = Convert.ToString(Math.Round(totadd, 2));
                txtlbl101.Value = Convert.ToString(Math.Round(totdepr, 2));
            }
            btnsave.Disabled = false;
        }
    }
}
// purchase type=10 wb_fa_pur, sale=20 wb_fa_vch, 30= dep wb_fa_vch, 40= wb_fa_vch sale dep write back, 50=adjustment asset, 60=