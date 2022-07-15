using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_Pf_Config : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it, mq0, mq1, mq2, mq3, mq4;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    double db1, db2, db3, db4, db5;
    
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
                doc_addl.Value = "1";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            btnprint.Visible = false;
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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

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
        //tab1.Visible = false;
        //tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        create_tab();
        sg1_add_blankrows();
        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
        hf2.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_PTAX";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "PF");
        lblheader.Text = "PF Config";
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

            case "GRADE":
                col1 = "";
                SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by grade_code";
                break;
            case "STATE":
                col1 = "";
                SQuery = "SELECT TYPE1 AS FSTR,NAME AS STATE_NAME, TYPE1 AS CODE  FROM TYPE WHERE ID='{' ORDER BY TYPE1";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[17].Text.ToString() + gr.Cells[18].Text.ToString() + gr.Cells[19].Text.ToString() + "'";
                    else col1 = "'" + gr.Cells[17].Text.ToString() + gr.Cells[18].Text.ToString() + gr.Cells[19].Text.ToString() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                else { }
                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                SQuery = "";
                break;

            case "SG1_ROW_ITEM":
                col1 = "";
                int cnt = 0;
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (((TextBox)sg1.Rows[cnt].FindControl("sg1_t10")).Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)gr.FindControl("sg1_t10")).Text.Trim() + "'";
                        else col1 = "'" + ((TextBox)gr.FindControl("sg1_t10")).Text.Trim() + "'";
                    }
                    cnt = cnt + 1;
                }
                if (col1.Length <= 0) col1 = "'-'";                              
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.grade,b.name as grade_name,a.staten,a.ENT_BY ,to_char(a.ENT_dt,'dd/mm/yyyy') as ENT_dt FROM " + frm_tabname + " A,type b  WHERE trim(a.grade)=trim(b.type1) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' and b.id='I' and b.type1 like '0%' AND A.VCHDATE  " + DateRange + " ORDER BY A.VCHNUM DESC";
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
            frm_vty = "PF";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                hffield.Value = "NEW_E";
                //make_qry_4_popup();
                //fgen.Fn_open_sseek("-", frm_qstr);
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' and vchdate " + DateRange + " AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtstatus.Text = "N";
        txtentby.Text = frm_uname;
        txtentdt.Text = vardate;
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl7.Focus();
        sg1_dt = new DataTable();
        create_tab();
        for (int i = 0; i < 1; i++)
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
            switch (i)
            {
                case 0:                    
                    sg1_dr["sg1_t1"] = "PT";
                    break;
            }
            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1_add_blankrows();
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
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        Cal();
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        if (txtlbl4.Text.Trim() == "-")
        {
            fgen.msg("-", "AMSG", "Please Select Grade !!"); btnlbl4.Focus();
            return;
        }
        int dhd = fgen.ChkDate(txtefffrom.Text.ToString());
        int dhd1 = fgen.ChkDate(txtlbl4a.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Effective Date"); txtefffrom.Focus();
            return;
        }
        if (dhd1 == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Effective Date"); txtlbl4a.Focus();
            return;
        }

        if (txtlbl4.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select " + lbl4.Text);
            txtlbl4.Focus();
            return;
        }
        SQuery = "";
        dt = new DataTable();
        SQuery = "SELECT * FROM WB_PTAX WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='PF' AND VCHDATE " + DateRange + " AND GRADE='" + txtlbl4.Text.Trim() + "' AND ICAT='N'";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (edmode.Value != "Y")
        {
            if (dt.Rows.Count > 0)
            {
                fgen.msg("-", "AMSG", "On this Grade an Entry is Already Activated first Deactivate that Entry!!"); return;
            }
        }
      
        mq0 = "select EMPR_RATE,GRADE from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text + "' and ed_fld='DED1' and ed_name='PF'";
        db1 = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, mq0, "EMPR_RATE"));//pf rate
        if(db1>0)
        {
            db2 = fgen.make_double(txtpfpercent.Text);
            db3 = fgen.make_double(txtpflimit.Text);
            db4 = db2 + db3;
            if(db4>db1)
            {
                fgen.msg("-", "AMSG", "Total of EPS & EPF rate is more than PF rate !!"); return; //earning and deduction form
            }
            else if(db4<db1)
            {
                fgen.msg("-", "AMSG", "Total of EPS & EPF rate is more less than PF rate!!"); return;
            }     
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
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND FINPKFLD LIKE '" + frm_tabname + "%'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                //fgen.save_info(frm_qstr,frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0,6),vardate, frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
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
            else
            {
                txtstatus.Text = "N";
                newCase(frm_vty);
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
                        newCase(col1);
                    //-------------------------------------------
                    #endregion                    
                    break;

                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                   dt2 = new DataTable();
                    mq0 = "SELECT TYPE1,NAME FROM TYPE WHERE ID='I' AND TYPE1 LIKE '0%'";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    dt3 = new DataTable();
                    mq1 = "SELECT TYPE1 AS FSTR,NAME AS STATE_NAME, TYPE1   FROM TYPE WHERE ID='{' ORDER BY TYPE1";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);                   
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    SQuery = "Select * from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' and icat='N'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtefffrom.Text = Convert.ToDateTime(dt.Rows[0]["EFF_FROM"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4a.Text = Convert.ToDateTime(dt.Rows[0]["EFF_TO"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["GRADE"].ToString().Trim();
                        txtgraden.Text = fgen.seek_iname_dt(dt2, "TYPE1='" + dt.Rows[0]["GRADE"].ToString().Trim() + "'", "NAME");                   
                        txtefffrom.Text = Convert.ToDateTime(dt.Rows[0]["EFF_FROM"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4a.Text = Convert.ToDateTime(dt.Rows[0]["EFF_TO"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtstatus.Text = dt.Rows[0]["ICAT"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                        txtpfpercent.Text = dt.Rows[0]["MTH01"].ToString().Trim();
                        txtpflimit.Text = dt.Rows[0]["MTH02"].ToString().Trim();
                        txtpensionlimit.Text = dt.Rows[0]["MTH03"].ToString().Trim();
                        txtinsp.Text = dt.Rows[0]["MTH04"].ToString().Trim();
                        txtvpf.Text = dt.Rows[0]["MTH05"].ToString().Trim();
                        txtpensionfundlimit.Text = dt.Rows[0]["MTH06"].ToString().Trim();
                        txtwages.Text = dt.Rows[0]["MTH07"].ToString().Trim();
                        txtedlipercent.Text = dt.Rows[0]["MTH08"].ToString().Trim();
                        txtadmin.Text = dt.Rows[0]["MTH09"].ToString().Trim();
                        txtsalry.Text = dt.Rows[0]["MTH10"].ToString().Trim();
                        txtentby.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtentdt.Text = dt.Rows[0]["ent_Dt"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        #region
                        //create_tab();
                        //sg1_dr = null;
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    sg1_dr = sg1_dt.NewRow();

                        //    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        //    sg1_dr["sg1_h1"] = "-";
                        //    sg1_dr["sg1_h2"] = "-";
                        //    sg1_dr["sg1_h3"] = "-";
                        //    sg1_dr["sg1_h4"] = "-";
                        //    sg1_dr["sg1_h5"] = "-";
                        //    sg1_dr["sg1_h6"] = "-";
                        //    sg1_dr["sg1_h7"] = "-";
                        //    sg1_dr["sg1_h8"] = "-";
                        //    sg1_dr["sg1_h9"] = "-";
                        //    sg1_dr["sg1_h10"] = "-";

                        //    sg1_dr["sg1_f1"] = "-";
                        //    sg1_dr["sg1_f2"] = "-";
                        //    sg1_dr["sg1_f3"] = "-";
                        //    sg1_dr["sg1_f4"] = "-";
                        //    sg1_dr["sg1_f5"] = "-";

                        //    sg1_dr["sg1_t1"] = dt.Rows[i]["ED_NAME"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = dt.Rows[i]["SAL_FRM"].ToString().Trim();
                        //    sg1_dr["sg1_t3"] = dt.Rows[i]["SAL_UPTO"].ToString().Trim();
                        //    sg1_dr["sg1_t4"] = dt.Rows[i]["MTH01"].ToString().Trim();
                        //    sg1_dr["sg1_t5"] = dt.Rows[i]["MTH02"].ToString().Trim();
                        //    sg1_dr["sg1_t6"] = dt.Rows[i]["MTH03"].ToString().Trim();
                        //    sg1_dr["sg1_t7"] = dt.Rows[i]["MTH04"].ToString().Trim();
                        //    sg1_dr["sg1_t8"] = dt.Rows[i]["MTH05"].ToString().Trim();
                        //    sg1_dr["sg1_t9"] = dt.Rows[i]["MTH06"].ToString().Trim();
                        //    sg1_dr["sg1_t10"] = dt.Rows[i]["MTH07"].ToString().Trim();
                        //    sg1_dr["sg1_t11"] = dt.Rows[i]["MTH08"].ToString().Trim();
                        //    sg1_dr["sg1_t12"] = dt.Rows[i]["MTH09"].ToString().Trim();
                        //    sg1_dr["sg1_t13"] = dt.Rows[i]["MTH10"].ToString().Trim();
                        //    sg1_dr["sg1_t14"] = dt.Rows[i]["MTH11"].ToString().Trim();
                        //    sg1_dr["sg1_t15"] = dt.Rows[i]["MTH12"].ToString().Trim();
                        //    sg1_dr["sg1_t16"] = dt.Rows[i]["M_TOT"].ToString().Trim();
                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}
                        //sg1_add_blankrows();
                        //ViewState["sg1"] = sg1_dt;
                        //sg1_add_blankrows();
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();
                        #endregion
                        dt.Dispose(); 
                        //sg1_dt.Dispose();
                        btnlbl7.Focus();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                    }
                    #endregion
                    break;
               
                case "STATE":
                    if (col1 == "") return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    btnlbl4.Focus();
                    break;
                case "GRADE":
                    if (col1 == "") return;
                    dt = new DataTable();
                    SQuery = "SELECT * FROM WB_PTAX WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='PF' AND VCHDATE " + DateRange + " AND GRADE='" + col1 + "' AND ICAT='N'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        fgen.msg("-", "AMSG", "On this Grade an Entry is Already Activated first Deactivate that Entry!!"); return;
                    }
                    txtlbl4.Text = col1;
                    txtgraden.Text = col2;
                    txtefffrom.Focus();
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
                    SQuery = "Select a.* from " + frm_tabname + " a  where  a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.morder";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt2 = new DataTable();
                    mq0 = "SELECT TYPE1,NAME FROM TYPE WHERE ID='I' AND TYPE1 LIKE '0%'";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    dt3 = new DataTable();
                    mq1 = "SELECT TYPE1 AS FSTR,NAME AS STATE_NAME, TYPE1   FROM TYPE WHERE ID='{' ORDER BY TYPE1";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl4.Text = dt.Rows[0]["GRADE"].ToString().Trim();
                        txtgraden.Text = fgen.seek_iname_dt(dt2, "TYPE1='" + dt.Rows[0]["GRADE"].ToString().Trim() + "'", "NAME");
                       // txtlbl7.Text = dt.Rows[0]["STATEN"].ToString().Trim();
                       // txtlbl7a.Text = fgen.seek_iname_dt(dt3, "TYPE1='" + dt.Rows[0]["STATEN"].ToString().Trim() + "'", "STATE_NAME");
                        txtefffrom.Text = Convert.ToDateTime(dt.Rows[0]["EFF_FROM"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4a.Text = Convert.ToDateTime(dt.Rows[0]["EFF_TO"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtstatus.Text = dt.Rows[0]["ICAT"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                        txtpfpercent.Text = dt.Rows[0]["MTH01"].ToString().Trim();
                        txtpflimit.Text = dt.Rows[0]["MTH02"].ToString().Trim();
                        txtpensionlimit.Text = dt.Rows[0]["MTH03"].ToString().Trim();
                        txtinsp.Text = dt.Rows[0]["MTH04"].ToString().Trim();
                        txtvpf.Text = dt.Rows[0]["MTH05"].ToString().Trim();
                        txtpensionfundlimit.Text = dt.Rows[0]["MTH06"].ToString().Trim();
                        txtwages.Text = dt.Rows[0]["MTH07"].ToString().Trim();
                        txtedlipercent.Text = dt.Rows[0]["MTH08"].ToString().Trim();
                        txtadmin.Text = dt.Rows[0]["MTH09"].ToString().Trim();
                        txtsalry.Text = dt.Rows[0]["MTH10"].ToString().Trim();
                                           
                        //create_tab();
                        //sg1_dr = null;
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    mq0 = ""; db1 = 0;
                        //    sg1_dr = sg1_dt.NewRow();
                        //    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        //    sg1_dr["sg1_h1"] = "-";
                        //    sg1_dr["sg1_h2"] = "-";
                        //    sg1_dr["sg1_h3"] = "-";
                        //    sg1_dr["sg1_h4"] = "-";
                        //    sg1_dr["sg1_h5"] = "-";
                        //    sg1_dr["sg1_h6"] = "-";
                        //    sg1_dr["sg1_h7"] = "-";
                        //    sg1_dr["sg1_h8"] = "-";
                        //    sg1_dr["sg1_h9"] = "-";
                        //    sg1_dr["sg1_h10"] = "-";
                        //    sg1_dr["sg1_f1"] = dt.Rows[i]["ED_FLD"].ToString().Trim();
                        //    sg1_dr["sg1_f2"] = "";
                        //    sg1_dr["sg1_f3"] = "";
                        //    sg1_dr["sg1_f4"] = "";
                        //    sg1_dr["sg1_f5"] = "-";
                        //    sg1_dr["sg1_f6"] = "";                                                       
                        //    sg1_dr["sg1_t1"] = dt.Rows[i]["ED_NAME"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = fgen.make_double(dt.Rows[i]["SAL_FRM"].ToString().Trim());
                        //    sg1_dr["sg1_t3"] = fgen.make_double(dt.Rows[i]["SAL_UPTO"].ToString().Trim());
                        //    sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["MTH04"].ToString().Trim());
                        //    sg1_dr["sg1_t5"] = fgen.make_double(dt.Rows[i]["MTH05"].ToString().Trim());
                        //    sg1_dr["sg1_t6"] = fgen.make_double(dt.Rows[i]["MTH06"].ToString().Trim());
                        //    sg1_dr["sg1_t7"] = fgen.make_double(dt.Rows[i]["MTH07"].ToString().Trim());
                        //    sg1_dr["sg1_t8"] = fgen.make_double(dt.Rows[i]["MTH08"].ToString().Trim());
                        //    sg1_dr["sg1_t9"] = fgen.make_double(dt.Rows[i]["MTH09"].ToString().Trim());
                        //    sg1_dr["sg1_t10"] = fgen.make_double(dt.Rows[i]["MTH10"].ToString().Trim());
                        //    sg1_dr["sg1_t11"] = fgen.make_double(dt.Rows[i]["MTH11"].ToString().Trim());
                        //    sg1_dr["sg1_t12"] = fgen.make_double(dt.Rows[i]["MTH12"].ToString().Trim());
                        //    sg1_dr["sg1_t13"] = fgen.make_double(dt.Rows[i]["MTH01"].ToString().Trim());
                        //    sg1_dr["sg1_t14"] = fgen.make_double(dt.Rows[i]["MTH02"].ToString().Trim());
                        //    sg1_dr["sg1_t15"] = fgen.make_double(dt.Rows[i]["MTH03"].ToString().Trim());
                        //    sg1_dr["sg1_t16"] = fgen.make_double(dt.Rows[i]["M_TOT"].ToString().Trim());                            
                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}

                        //sg1_add_blankrows();
                        //ViewState["sg1"] = sg1_dt;
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();
                        dt.Dispose(); 
                        //sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        btnlbl4.Enabled = false;
                        edmode.Value = "Y";                        
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_qa_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    #region
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    //SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,trim(a.srno) as morder1,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.vchdate,'dd/mm/yyyy') as pvchdate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)='" + col1 + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = col1;
                        //txtlbl4a.Text = col2;
                    }
                    dt.Dispose();
                    // SQuery = "Select * from inspmst a where a.branchcd='" + frm_mbr + "' and a.icode='" + txtlbl7.Text + "' ORDER BY A.srno";
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
                        //edmode.Value = "Y";
                    }
                    #endregion
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

                case "LICNO":
                    if (col1.Length <= 0) return;                    
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();

                    //SQuery = "select distinct a.ponum as billno,to_char(a.podate,'dd/mm/yyyy') as bill_dt ,trim(b.icode) as icode,a.invno as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,SUM(a.iqtyout) AS IQTYOUT,c.iname,sum(a.iqtyout*a.iqty_chlwt) as value  from ivoucherp a,matl_spec b,item c where trim(a.tc_no)||to_char(a.refdate,'dd/mm/yyyy') =trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(b.icode)=trim(c.icode) and  a.branchcd='" + frm_mbr + "' and a.type='4F' and  a.ponum||to_char(a.podate,'dd/mm/yyyy')||a.invno||to_char(a.invdate,'dd/mm/yyyy')||trim(b.icode) ='" + col1 + "' GROUP BY a.ponum ,to_char(a.podate,'dd/mm/yyyy') ,trim(b.icode) ,a.invno ,a.iqty_chlwt,to_char(a.invdate,'dd/mm/yyyy'),c.iname ";
                    SQuery = "select trim(a.vchnum) as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdate,trim(b.ship_billno) as billno,trim(b.ship_billdt) as bill_dt,trim(c.vchnum) as tc_no,to_char(c.vchdate,'dd/mm/yyyy') as refdate,trim(c.icode) as icode,trim(i.iname) as iname from ivoucherp a ,wb_exp_imp b,matl_spec c,item i where trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.entry_no_bill)||to_char(b.entry_dt_bill,'dd/mm/yyyy') and trim(a.branchcd)||trim(a.tc_no)||to_char(a.refdate,'dd/mm/yyyy') =trim(c.branchcd)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy') and trim(c.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and  a.type='4F' and b.type='EX' and c.type='4F' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(b.ship_billno)||trim(b.ship_billdt)||trim(c.icode) ='" + col1 + "' group by trim(a.vchnum),to_char(a.vchdate,'dd/mm/yyyy'),trim(b.ship_billno) ,trim(b.ship_billdt),trim(c.icode),trim(i.iname),trim(c.vchnum),to_char(c.vchdate,'dd/mm/yyyy')";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt3 = new DataTable();
                    //mq1 = "select trim(c.vchnum) as vchnum, to_char(c.vchdate,'dd/mm/yyyy') as vchdate, trim(c.icode) as icode,i.iname ,sum(c.mat_qty) as qty from matl_spec c,item i where trim(c.icode)=trim(i.icode) and c.branchcd='" + frm_mbr + "' and c.type='4F' AND c.vchdate between to_date('" + txtlicdt.Text.Trim() + "','dd/mm/yyyy') and to_Date('" + txtexpvalid.Text.Trim() + "','dd/mm/yyyy')-100 GROUP BY trim(c.vchnum) , to_char(c.vchdate,'dd/mm/yyyy'), trim(c.icode),i.iname";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);


                    if (col1.Length <= 0) return;
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["billno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["bill_dt"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["invno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["invdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[19].Text = fgen.seek_iname_dt(dt3, "VCHNUM='" + dt.Rows[d]["TC_NO"].ToString().Trim() + "' and VCHDATE='" + dt.Rows[d]["REFDATE"].ToString().Trim() + "'and ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "'", "ICODE");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[20].Text = fgen.seek_iname_dt(dt3, "VCHNUM='" + dt.Rows[d]["TC_NO"].ToString().Trim() + "' and VCHDATE='" + dt.Rows[d]["REFDATE"].ToString().Trim() + "'and ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "'", "iname");

                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = fgen.seek_iname_dt(dt3, "VCHNUM='" + dt.Rows[d]["TC_NO"].ToString().Trim() + "' and VCHDATE='" + dt.Rows[d]["REFDATE"].ToString().Trim() + "'and ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "'", "QTY");
                        //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = (fgen.make_double(dt.Rows[d]["value"].ToString().Trim())).ToString();
                    }
                    setColHeadings();
                    break;

                case "SG1_ROW_ITEM":
                    if (col1.Length <= 0) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = col1;                    
                    hf2.Value = col1;                 
                    //SQuery = "select distinct num3 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type ='20' and trim(licno)='" + txtlbl4.Text.Trim() + "' and to_char(licdt,'dd/mm/yyyy')='" + txtlicdt.Text.Trim() + "' and trim(ciname)='" + col1 + "'";
                    //txtcurrqty.Text = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "qty");
                    //SQuery = "select wast_perc from wb_licrec where branchcd='" + frm_mbr + "' and type ='10' and flag='IM' and trim(licno)='" + txtlbl4.Text.Trim() + "' and to_char(licdt,'dd/mm/yyyy')='" + txtlicdt.Text.Trim() + "' and trim(ciname)='" + col1 + "'";
                    //txtwastperc.Text = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "wast_perc");
                    //db1=fgen.make_double(col3)+(fgen.make_double(col3) * fgen.make_double(txtwastperc.Text) / 100);                    
                    //txtimp_adjqty.Text = db1.ToString();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    if (ViewState["sg1"] != null)
                    {
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.ToString();
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
                        dt3 = new DataTable();
                        mq1 = "select trim(entry_no_bill) as invno,to_char(entry_dt_bill,'dd/mm/yyyy') as invdate,trim(ship_billno) as ship_billno,ship_billdt,foreign_val from wb_exp_imp where branchcd='" + frm_mbr + "' and type='EX'";
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                        for (int d = 0; d < dt3.Rows.Count; d++)
                        {
                            mq0 = ""; db1 = 0;
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
                            sg1_dr["sg1_f1"] = "";
                            sg1_dr["sg1_f2"] = "";
                            sg1_dr["sg1_f3"] = "";
                            sg1_dr["sg1_f4"] = "";
                            sg1_dr["sg1_f5"] = "";
                            sg1_dr["sg1_f6"] = "";
                            sg1_dr["sg1_t1"] = "-";
                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dt.Rows.Add(sg1_dr);                           
                        }
                    }
                    sg1_add_blankrows();
                    
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    Cal();
                    dt3.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    
                    #endregion
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
                        for (i = 0; i < dt.Rows.Count - 1; i++)
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
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim();
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
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            sg1.Rows[i].Cells[12].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();                    
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");            
            SQuery = "select  a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.grade,to_char(a.eff_from,'dd/mm/yyyy') as eff_from,to_char(a.eff_to,'dd/mm/yyyy') as eff_to,nvl(a.mth01,0) as Employer_Pension_fund_percent,nvl(a.mth02,0) as Employer_Pf_lmt,nvl(a.mth03,0) as Employer_Pension_Fund_lmt,nvl(a.mth04,0) as Admin_Insp_Chrgs,nvl(a.mth05,0) as Employer_VPF_Sal_Mlt,nvl(a.mth06,0) as Employer_Pens_Fund_Mlt,nvl(a.mth07,0) as Min_Wages,nvl(a.mth08,0) as Ac_21_Edli_percent,nvl(a.mth09,0) as EDLI_Admin_chrgs,nvl(a.mth10,0)Sal_Lmt,a.icat from WB_PTAX a where a.branchcd='" + frm_mbr + "' and a.type='PF' and a.vchdate " + PrdRange + "";           
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
            if (col1 == "N")
            {
                btnsave.Disabled = false;
            }
            else
            {
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
                           // cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";//OLD
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + txtvchnum.Text + txtvchdate.Text + "'";//new
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                           // cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + txtvchnum.Text + txtvchdate.Text + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
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
                        col1 = "N"; btnsave.Disabled = false;
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
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
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
    public void sg1_add_blankrows()
    {
        if (sg1_dt != null)
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
            sg1_dr["sg1_f1"] = "";
            sg1_dr["sg1_f2"] = "";
            sg1_dr["sg1_f3"] = "-";
            sg1_dr["sg1_f4"] = "-";
            sg1_dr["sg1_f5"] = "-";
            sg1_dr["sg1_f6"] = "";
            sg1_dr["sg1_t1"] = "";
            sg1_dr["sg1_t2"] = "";
            sg1_dr["sg1_t3"] = "";
            sg1_dr["sg1_t4"] = "";
            sg1_dr["sg1_t5"] = "";
            sg1_dr["sg1_t6"] = "";
            sg1_dr["sg1_t7"] = "";
            sg1_dr["sg1_t8"] = "";
            sg1_dr["sg1_t9"] = "";
            sg1_dr["sg1_t10"] = "";
            sg1_dr["sg1_t11"] = "";
            sg1_dr["sg1_t12"] = "";
            sg1_dr["sg1_t13"] = "";
            sg1_dr["sg1_t14"] = "";
            sg1_dr["sg1_t15"] = "";
            sg1_dr["sg1_t16"] = "";
            sg1_dt.Rows.Add(sg1_dr);
        }
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
                if (sg1r < 1)
                {
                    ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t1")).ReadOnly = true;
                    sg1.Rows[sg1r].Cells[10].Enabled = false;
                    sg1.Rows[sg1r].Cells[11].Enabled = false;
                }                
            }
            sg1.Columns[10].HeaderStyle.Width = 50;
            sg1.Columns[11].HeaderStyle.Width = 50;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 80;
            sg1.Columns[14].HeaderStyle.Width = 60;
            sg1.Columns[15].HeaderStyle.Width = 100;
            sg1.Columns[16].HeaderStyle.Width = 100;
            sg1.Columns[17].HeaderStyle.Width = 80;
            sg1.Columns[18].HeaderStyle.Width = 100;
            sg1.Columns[19].HeaderStyle.Width = 130;
            sg1.Columns[20].HeaderStyle.Width = 130;
            sg1.Columns[21].HeaderStyle.Width = 100;
            sg1.Columns[22].HeaderStyle.Width = 100;
            sg1.Columns[23].HeaderStyle.Width = 100;
            sg1.Columns[24].HeaderStyle.Width = 100;
            sg1.Columns[25].HeaderStyle.Width = 100;
            sg1.Columns[26].HeaderStyle.Width = 100;
            sg1.Columns[27].HeaderStyle.Width = 100;
            sg1.Columns[28].HeaderStyle.Width = 100;
            sg1.Columns[29].HeaderStyle.Width = 100;
            sg1.Columns[30].HeaderStyle.Width = 100;
            sg1.Columns[31].HeaderStyle.Width = 100;
            sg1.Columns[32].HeaderStyle.Width = 100;

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
            fgen.msg("-", "AMSG", "Doc No. Not Correct");
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This From The List");
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
                    // make_qry_4_popup();
                    // fgen.Fn_open_sseek("Select HSN Code", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    btnhideF_Click(sender, e);
                }
                break;
            case "SG1_ROW_ITEM":
                //if (index < sg1.Rows.Count - 1)
                //{
                hf1.Value = index.ToString();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //----------------------------

                hffield.Value = "SG1_ROW_ITEM";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Item Description", frm_qstr);
                //}
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "GRADE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TYPE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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
        hffield.Value = "STATE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select State", frm_qstr);
    }
    //------------------------------------------------------------------------------------   
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
          //  oporow["STATEN"] = txtestncd.Text.Trim();
            oporow["icat"] = txtstatus.Text.Trim();
            oporow["EFF_FROM"] = Convert.ToDateTime(txtefffrom.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["EFF_TO"] = Convert.ToDateTime(txtlbl4a.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["GRADE"] = txtlbl4.Text.Trim().ToUpper();           
            oporow["MTH01"] = txtpfpercent.Text.Trim();
            oporow["MTH02"] = txtpflimit.Text.Trim();
            oporow["MTH03"] = txtpensionlimit.Text.Trim();
            oporow["MTH04"] = txtinsp.Text.Trim();
            oporow["MTH05"] = txtvpf.Text.Trim();
            oporow["MTH06"] = txtpensionfundlimit.Text.Trim();
            oporow["MTH07"] = txtwages.Text.Trim();
            oporow["MTH08"] = txtedlipercent.Text.Trim();
            oporow["MTH09"] = txtadmin.Text.Trim();
            oporow["MTH10"] = txtsalry.Text.Trim();

            if (txtrmk.Text.Trim().Length > 80)
            {
                oporow["REMARK"] = txtrmk.Text.Trim().ToUpper().Substring(0, 79);
            }
            else
            {
                oporow["REMARK"] = txtrmk.Text.Trim().ToUpper();
            }
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
                oporow["edt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);        
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "PF");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    public void Cal()
    {
        double m1 = 0; double m2 = 0; double m3 = 0; double m4 = 0; double m5 = 0; double m6 = 0; double m7 = 0; double m8 = 0; double m9 = 0; double m10 = 0; double m11 = 0; double m12 = 0; double m13 = 0;        
        for (int sg1r = 0; sg1r < sg1.Rows.Count ; sg1r++)
        {
            m1 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t4")).Text.Trim());;
                m2 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t5")).Text.Trim());;
                m3 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t6")).Text.Trim());
                m4 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t7")).Text.Trim());
                m5 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t8")).Text.Trim());
                m6 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t9")).Text.Trim());
                m7 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t10")).Text.Trim());
                m8 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t11")).Text.Trim());
                m9 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t12")).Text.Trim());
                m10 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t13")).Text.Trim());
                m11 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t14")).Text.Trim());
                m12 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t15")).Text.Trim());
                m13 = (m1 * 1) + (m2 * 1) + (m3 * 1) + (m4 * 1) + (m5 * 1) + (m6 * 1) + (m7 * 1) + (m8 * 1) + (m9 * 1) + (m10 * 1) + (m11 * 1) + (m12 * 1);
                ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t16")).Text = m13.ToString();            
        }
    }
    //------------------------------------------------------------------------------------
}