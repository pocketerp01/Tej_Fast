using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_label_ts : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
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
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    //double double_val2, double_val1;
    string srno;
    double true_false, master_rate, rate1, rate2, totamt;
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
                doc_addl.Value = "1";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
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
            dtCol = fgen.getdata(frm_qstr, frm_cocd, "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND FROM SYS_CONFIG WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
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
                #region hide hidden columns
                for (int i = 0; i < 10; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
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

        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
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
        btnprint.Disabled = true; btnlist.Disabled = true;
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
        lblheader.Text = "Label Transaction";
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "inspvch";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "LA");
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

            case "CUST":
                SQuery = "SELECT TRIM(ACODE) AS FSTR,ACODE AS CODE,ANAME AS CUSTOMER,ADDR1||ADDR2 AS ADDRESS,STATEN AS STATE FROM FAMST WHERE SUBSTR(TRIM(ACODE),1,2)='16' ORDER BY CUSTOMER";
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
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "select distinct a.branchcd||a.type||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Transaction_no,to_char(a.vchdate,'dd/mm/yyyy') as Transaction_Dt,a.obsv10 as sales_executive,(case when nvl(a.acode,'-')!='-' then f.aname else a.obsv11 end) as customer_name,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a left join famst f on trim(a.acode)=trim(f.acode) where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Transaction_no,to_char(a.vchdate,'dd/mm/yyyy') as Transaction_Dt,a.obsv10 as sales_executive,(case when nvl(a.acode,'-')!='-' then f.aname else a.obsv11 end) as customer_name,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a left join famst f on trim(a.acode)=trim(f.acode) where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                break;
        }
        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" | btnval == "Print"))
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
        typePopup = "Y";
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Label Master", frm_qstr);
            }
            // else comment upper code
            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            //txtvchnum.Text = frm_vnum;
            //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        //txtlbl2.Text = frm_uname;
        //txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        //txtlbl5.Text = "-";
        //txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl101.Focus();
        //sg1_dt = new DataTable();
        //create_tab();
        //int j;
        //for (j = i; j < 10; j++)
        //{
        //    sg1_add_blankrows();
        //}
        //sg1.DataSource = sg1_dt;
        //sg1.DataBind();
        //setColHeadings();
        //ViewState["sg1"] = sg1_dt;
        //// Popup asking for Copy from Older Data
        ////fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        ////hffield.Value = "NEW_E";        
        #endregion
    }
    //------------------------------------------------------------------------------------
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
        Cal();
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (txtlbl101.Text == "" || txtlbl101.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select Customer First!!"); txtlbl101.Focus(); return;
        }
        //string mandField = "";
        //mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        //if (mandField.Length > 1)
        //{
        //    fgen.msg("-", "AMSG", mandField);
        //    return;
        //}
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "LA");
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,to_chaR(a.vchdate,'dd/mm/yyyy') as vchDt from inspmst a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["vchDt"].ToString().Trim();
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
                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["num1"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["srno"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        newCase("LA");
                        setColHeadings();
                    }
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

                        txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
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
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,f.aname from " + frm_tabname + " a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl101.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl101a.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["mrrnum"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["mrrdate"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["title"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["obsv1"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["obsv2"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[0]["obsv3"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[0]["obsv4"].ToString().Trim();
                        txtlbl102.Text = dt.Rows[0]["obsv5"].ToString().Trim();
                        txtlbl102a.Text = dt.Rows[0]["obsv6"].ToString().Trim();
                        txtlbl103.Text = dt.Rows[0]["obsv7"].ToString().Trim();
                        txtlbl103a.Text = dt.Rows[0]["obsv8"].ToString().Trim();
                        txtlbl8.Text = dt.Rows[0]["obsv9"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["obsv10"].ToString().Trim();
                        txtTag.Text = dt.Rows[0]["obsv12"].ToString().Trim();
                        txtGSM.Text = dt.Rows[0]["obsv13"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[0]["obsv14"].ToString().Trim();
                        txtPaper_Size.Text = dt.Rows[0]["obsv15"].ToString().Trim();
                        txtColors.Text = dt.Rows[0]["CONTPLAN"].ToString().Trim();

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

                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";

                            sg1_dr["sg1_t1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["qty1"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["col5"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["qty2"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["qty3"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["qty4"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "CUST":
                    txtlbl101.Text = col1;
                    txtlbl101a.Text = col3;
                    txtlbl7a.Focus();
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

                case "SG1_ROW_ADD":
                    //#region for gridview 1
                    //if (col1.Length <= 0) return;
                    //if (ViewState["sg1"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    for (i = 0; i < dt.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                    //        sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                    //        sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                    //        sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                    //        sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                    //        sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                    //        sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                    //        sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                    //        sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                    //        sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                    //        sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                    //        sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                    //        sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                    //        sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                    //        sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                    //        sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }

                    //    dt = new DataTable();
                    //    if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                    //    else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //    for (int d = 0; d < dt.Rows.Count; d++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    //        sg1_dr["sg1_h1"] = dt.Rows[d]["userid"].ToString().Trim();
                    //        sg1_dr["sg1_h2"] = dt.Rows[d]["username"].ToString().Trim();
                    //        sg1_dr["sg1_h3"] = "-";
                    //        sg1_dr["sg1_h4"] = "-";
                    //        sg1_dr["sg1_h5"] = "-";
                    //        sg1_dr["sg1_h6"] = "-";
                    //        sg1_dr["sg1_h7"] = "-";
                    //        sg1_dr["sg1_h8"] = "-";
                    //        sg1_dr["sg1_h9"] = "-";
                    //        sg1_dr["sg1_h10"] = "-";

                    //        sg1_dr["sg1_f1"] = dt.Rows[d]["USERID"].ToString().Trim();
                    //        sg1_dr["sg1_f2"] = dt.Rows[d]["full_Name"].ToString().Trim();
                    //        sg1_dr["sg1_f3"] = dt.Rows[d]["username"].ToString().Trim();
                    //        sg1_dr["sg1_f4"] = dt.Rows[d]["contactno"].ToString().Trim();
                    //        sg1_dr["sg1_f5"] = dt.Rows[d]["emailid"].ToString().Trim();

                    //        sg1_dr["sg1_t1"] = "";
                    //        sg1_dr["sg1_t2"] = "";
                    //        sg1_dr["sg1_t3"] = "";
                    //        sg1_dr["sg1_t4"] = "";
                    //        sg1_dr["sg1_t5"] = "";
                    //        sg1_dr["sg1_t6"] = "";
                    //        sg1_dr["sg1_t7"] = "";
                    //        sg1_dr["sg1_t8"] = "";
                    //        sg1_dr["sg1_t9"] = "";
                    //        sg1_dr["sg1_t10"] = "";
                    //        sg1_dr["sg1_t11"] = "";
                    //        sg1_dr["sg1_t12"] = "";
                    //        sg1_dr["sg1_t13"] = "";
                    //        sg1_dr["sg1_t14"] = "";
                    //        sg1_dr["sg1_t15"] = "";
                    //        sg1_dr["sg1_t16"] = "";

                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }
                    //}
                    //sg1_add_blankrows();

                    //ViewState["sg1"] = sg1_dt;
                    //sg1.DataSource = sg1_dt;
                    //sg1.DataBind();
                    //dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    //#endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    //if (col1.Length <= 0) return;
                    ////********* Saving in Hidden Field 
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    ////********* Saving in GridView Value
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
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
                    //#region Remove Row from GridView
                    //if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    i = 0;
                    //    for (i = 0; i < sg1.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = (i + 1);
                    //        sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                    //        sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                    //        sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                    //        sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                    //        sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                    //        sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                    //        sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                    //        sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                    //        sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                    //        sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();

                    //        sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                    //        sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                    //        sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                    //        sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                    //        sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();

                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                    //        sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                    //        sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                    //        sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                    //        sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                    //        sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                    //        sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                    //        sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }

                    //    if (edmode.Value == "Y")
                    //    {
                    //        //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                    //        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    }
                    //    else
                    //    {
                    //        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    }

                    //    sg1_add_blankrows();

                    //    ViewState["sg1"] = sg1_dt;
                    //    sg1.DataSource = sg1_dt;
                    //    sg1.DataBind();
                    //}
                    //#endregion
                    setColHeadings();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = "LA";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select a.Vchnum as Transaction_no,to_char(a.vchdate,'dd/mm/yyyy') as Transaction_Dt,a.Col1 as Type_of_charges,a.col2 as description,a.col3 as True_or_false,a.col4 as Standard,a.qty1 as master_rate,a.qty2 as rate1,a.qty3 as rate2,a.qty4 as tot_amt, a.Ent_by,a.ent_Dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd ,a.vchnum ,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
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
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            }
            //-----------------------------
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
                        //save_it = "N";
                        //for (i = 0; i < sg1.Rows.Count - 0; i++)
                        //{
                        //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
                        //    {
                        //        save_it = "Y";
                        //    }
                        //}
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
                        cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                        cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
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
    //------------------------------------------------------------------------------------
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
    //------------------------------------------------------------------------------------
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
        sg1_dr["sg1_t11"] = "-";
        sg1_dr["sg1_t12"] = "-";
        sg1_dr["sg1_t13"] = "-";
        sg1_dr["sg1_t14"] = "-";
        sg1_dr["sg1_t15"] = "-";
        sg1_dr["sg1_t16"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();


        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }
    //------------------------------------------------------------------------------------
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
            sg1.Columns[12].HeaderStyle.Width = 80;
            sg1.Columns[18].HeaderStyle.Width = 300;
            sg1.Columns[19].HeaderStyle.Width = 200;
            sg1.Columns[20].HeaderStyle.Width = 100;
            sg1.Columns[21].HeaderStyle.Width = 120;
            sg1.Columns[22].HeaderStyle.Width = 100;
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
            //if (index < sg1.Rows.Count - 1)
            //{
            //    hf1.Value = index.ToString();
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            //    //----------------------------
            //    hffield.Value = "SG1_RMV";
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
            //    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
            //}
            //break;


            case "SG1_ROW_ADD":

                //if (index < sg1.Rows.Count - 1)
                //{
                //    hf1.Value = index.ToString();
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //    //----------------------------
                //    hffield.Value = "SG1_ROW_ADD_E";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_sseek("Select Item", frm_qstr);
                //}
                //else
                //{
                //    hffield.Value = "SG1_ROW_ADD";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_mseek("Select Item", frm_qstr);
                //    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                //}
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
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CUST";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl20_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl21_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl22_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            //if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
            //{
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["SRNO"] = i + 1;
            oporow["acode"] = txtlbl101.Text.Trim().ToUpper();
            oporow["mrrnum"] = txtlbl4.Text.Trim().ToUpper();
            oporow["mrrdate"] = txtlbl4a.Text.Trim().ToUpper();
            oporow["col1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["col2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
            oporow["col3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper();
            oporow["col4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
            oporow["col5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper();// master sheet srno
            oporow["qty1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            oporow["qty2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
            oporow["qty3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
            oporow["qty4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
            oporow["title"] = txtrmk.Text.Trim().Trim().ToUpper();
            oporow["obsv1"] = txtlbl2.Text.Trim().ToUpper();
            oporow["obsv2"] = txtlbl3.Text.Trim().ToUpper();
            oporow["obsv3"] = txtlbl5.Text.Trim().ToUpper();
            oporow["obsv4"] = txtlbl6.Text.Trim().ToUpper();
            oporow["obsv5"] = txtlbl102.Text.Trim().ToUpper();
            oporow["obsv6"] = txtlbl102a.Text.Trim().ToUpper();
            oporow["obsv7"] = txtlbl103.Text.Trim().ToUpper();
            oporow["obsv8"] = txtlbl103a.Text.Trim().ToUpper();
            oporow["obsv9"] = txtlbl8.Text.Trim().ToUpper();
            oporow["obsv10"] = txtlbl7a.Text.Trim().ToUpper();
            // oporow["obsv11"] = txtlbl101a.Text.Trim().ToUpper();
            oporow["obsv12"] = txtTag.Text.Trim().ToUpper();
            oporow["obsv13"] = txtGSM.Text.Trim().ToUpper();
            oporow["obsv14"] = txtlbl9.Text.Trim().ToUpper();
            oporow["obsv15"] = txtPaper_Size.Text.Trim().ToUpper();
            oporow["CONTPLAN"] = txtColors.Text.Trim().ToUpper();

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"].ToString();
                //oporow["edt_by"] = frm_uname;
                //oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                //oporow["edt_by"] = "-";
                //oporow["eDt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
            //}
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {


    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "select distinct a.branchcd||a.type||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Master_no,to_char(a.vchdate,'dd/mm/yyyy') as Master_Dt,a.col1 as type_of_charges,a.col2 as description,to_Char(a.vchdate,'yyyymmdd') as vdd from inspmst a where a.branchcd='" + frm_mbr + "' and a.type='LS' and a.srno='1' order by vdd desc,a.vchnum desc";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "LA");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------   
    protected void btnCal_ServerClick(object sender, EventArgs e)
    {
        Cal();
    }
    //------------------------------------------------------------------------------------
    protected void Cal()
    {
        if (ViewState["sg1"] != null)
        {
            double GrandTot = 0, tot29 = 0, tot30 = 0, tot31 = 0, tot34 = 0, tot35 = 0, tot36 = 0, rate_per_pc = 0;
            txtlbl5.Text = (Math.Round(fgen.make_double(txtlbl2.Text) / fgen.make_double(txtlbl3.Text), 0)).ToString();
            txtlbl102.Text = (Math.Round(fgen.make_double(txtlbl5.Text) + fgen.make_double(txtlbl6.Text), 0)).ToString();
            double Sheet_per = fgen.make_double(txtlbl6.Text) / fgen.make_double(txtlbl5.Text);
            txtlbl102a.Text = (Math.Round((Sheet_per * 100), 0)).ToString() + " %";
            txtlbl103a.Text = (Math.Round(fgen.make_double(txtlbl102.Text) / fgen.make_double(txtlbl103.Text), 0)).ToString();
            if (txtlbl103a.Text == "Infinity")
            {
                txtlbl103a.Text = "0";
            }

            txtlbl8.Text = (Math.Round(fgen.make_double(txtlbl102.Text) * fgen.make_double(txtlbl3.Text), 2)).ToString();

            for (int i = 0; i < sg1.Rows.Count; i++)
            {
                srno = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text;
                switch (srno)
                {
                    case "1":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        totamt = true_false * master_rate * fgen.make_double(txtlbl103a.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "2":
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                        totamt = master_rate * rate1 * rate2;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "3":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        totamt = true_false * master_rate * rate1;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "4":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        totamt = master_rate * true_false * rate1;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "5":
                    case "7":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) / 1000;
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        totamt = true_false * master_rate * rate1 * fgen.make_double(txtlbl102.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "6":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        totamt = master_rate * true_false * rate1;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "8":
                    case "9":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                        totamt = true_false * master_rate * rate1 * rate2 * fgen.make_double(txtlbl102.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "10":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        totamt = true_false * master_rate * fgen.make_double(txtlbl102.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "11":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        totamt = true_false * master_rate;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "12":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        totamt = true_false * master_rate * fgen.make_double(txtlbl102.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "13":
                    case "15":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                        totamt = true_false * master_rate * rate1 * rate2;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "14":
                    case "16":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                        totamt = true_false * master_rate * rate1 * rate2 * fgen.make_double(txtlbl102.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "19":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) / 1000;
                        totamt = true_false * master_rate * fgen.make_double(txtlbl102.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    //case "16":
                    //    true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                    //    master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) / 1000;
                    //    totamt = master_rate * true_false * fgen.make_double(txtlbl102.Text);
                    //    ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                    //    GrandTot += totamt;
                    //    break;

                    case "17":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) / 1000;
                        totamt = master_rate * true_false * fgen.make_double(txtlbl102.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "20":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        totamt = master_rate * true_false;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "18":
                    case "21":
                    case "26":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) / 1000;
                        totamt = master_rate * true_false * fgen.make_double(txtlbl8.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    case "22":
                    case "23":
                    case "24":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        totamt = master_rate * true_false * fgen.make_double(txtlbl8.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    //case "24":
                    //    true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                    //    master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                    //    rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                    //    rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                    //    totamt = true_false * master_rate * rate1 * rate2 * fgen.make_double(txtlbl8.Text);
                    //    ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                    //    GrandTot += totamt;
                    //    break;

                    case "25":
                        true_false = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                        totamt = master_rate * true_false * rate1 * rate2 * fgen.make_double(txtlbl8.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    // case "27":
                    case "28":
                    case "29":
                        master_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        totamt = master_rate * rate1;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        GrandTot += totamt;
                        break;

                    //case "29":
                    //    ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(GrandTot, 2)).ToString();
                    //    tot29 = GrandTot;
                    //    break;

                    case "30":
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(GrandTot, 2)).ToString();
                        tot30 = GrandTot;
                        //rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                        //totamt = (GrandTot * rate2) / 100;
                        //tot30 = totamt;
                        //((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        break;

                    case "31":
                        rate2 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                        totamt = (GrandTot * rate2) / 100;
                        tot31 = totamt;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        //totamt = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text);
                        //tot31 = totamt;
                        break;

                    //case "32":
                    //    totamt = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text);
                    //    tot32 = totamt;
                    //    break;

                    //case "33":
                    //    totamt = tot29 + tot30 + tot31 + tot32;
                    //    GrandTot = 0;
                    //    GrandTot = totamt;
                    //    ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                    //    break;

                    case "34":
                        totamt = tot30 + tot31;
                        GrandTot = 0;
                        GrandTot = totamt;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        tot34 = totamt;
                        //totamt = GrandTot / fgen.make_double(txtlbl2.Text);
                        //GrandTot = 0;
                        //GrandTot = totamt;
                        //((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        //rate_per_pc = Math.Round(totamt, 2);
                        break;

                    case "35":
                        //rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        //tot29 = (GrandTot * rate1) / 100;
                        //totamt = tot29 + GrandTot;
                        //GrandTot = 0;
                        //GrandTot = totamt;
                        totamt = tot34 / fgen.make_double(txtlbl2.Text);
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        tot35 = totamt;
                        rate_per_pc = Math.Round(totamt, 2);
                        break;

                    case "36":
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        //tot29 = (GrandTot * rate1) / 100;
                        totamt = (tot35) + rate1 * tot35 / 100;
                        tot36 = totamt;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        break;

                    case "37":
                        //GrandTot = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text);
                        //tot29 = GrandTot - rate_per_pc;
                        //totamt = tot29 / rate_per_pc;
                        rate1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                        totamt = (tot36) + rate1 * tot36 / 100;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text = (Math.Round(totamt, 2)).ToString();
                        break;

                    case "38":
                        GrandTot = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text);
                        tot29 = GrandTot - rate_per_pc;
                        totamt = tot29 / rate_per_pc;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text = (Math.Round(totamt, 2)).ToString();
                        break;
                }
            }
        }
    }
}