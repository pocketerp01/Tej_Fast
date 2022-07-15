using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class fin_pay_web_om_regn : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt1, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0; string mq0, mq1, mq2, mq3;

    DataTable dtCol = new DataTable();
    string Checked_ok; string grade;
    string save_it;
    string html_body = "";
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    fgenDB fgen = new fgenDB();
    string frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, printBar = "N";
    protected void Page_Load(object sender, EventArgs e)
    {

        btnnew.Focus();
        frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
        frm_cocd = "TEST";
        frm_mbr = "00";
        frm_uname = "FINTEAM";
        Prg_Id = "F00000";
        frm_qstr = frm_cocd + "^" + Guid.NewGuid().ToString("N").Substring(0, 20) + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
        str = frm_qstr;
        if (fgen.checkDB(frm_qstr, frm_cocd) == true)
        {

        }
        //str = Request.QueryString["STR"].Trim().ToString();
        //frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
        //frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
        //if (frm_url.Contains("STR"))
        //{
        //    if (Request.QueryString["STR"].Length > 0)
        //    {
        //        frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
        //        if (frm_qstr.Contains("@"))
        //        {
        //            frm_formID = frm_qstr.Split('@')[1].ToString();
        //            frm_qstr = frm_qstr.Split('@')[0].ToString();
        //            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
        //        }.
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "F00000");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_UNAME", "FINTEAM");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COCD", "TEST");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_UNAME", "FINTEAM");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_YEAR", "2019");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ULEVEL", "0");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MBR", "00");
        //DateRange = fgenMV.Fn_Set_Mvar(frm_qstr, "U_DATERANGE");
        //frm_UserID = fgenMV.Fn_Set_Mvar(frm_qstr, "U_USERID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_Cdt1", "01/04/2019");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_Cdt2", "31/03/2020");
        //    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
        vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        //    }
        //    else Response.Redirect("~/login.aspx");
        //}

        //if (!Page.IsPostBack)
        //{
        //    doc_addl.Value = "1";
        //    fgen.DisableForm(this.Controls);
        //    enablectrl();
        //    getColHeading();
        //}
        //setColHeadings();
        //

        //if (frm_ulvl != "0")
        //{
        //    btndel.Visible = false;
        //}
        if (!Page.IsPostBack)
        {
            btnedit.Visible = false;
            btnprint.Visible = false;
            btnlist.Visible = false;
            btnnew.Visible = false;
            btndel.Visible = false;
            btnsave.InnerText = "Submit";
            typePopup = "Y";
            FillDropDown();
            btnexit.Visible = false;
            fstdiv.Visible = false;
            scnddiv.Visible = false;

            //newCase(frm_vty);
        }
        set_Val();


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

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = false; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btnlist.Disabled = true;
        btnprint.Disabled = true;
        btndel.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnexit.Visible = false;
        btncancel.Visible = true;
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
        //lblheader.Text = "Online Free Seat Booking";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "SEMINAR";
        frm_vty = "00";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
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

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                SQuery = "select trim(branchcd)||trim(grade)||trim(empcode) as fstr,name as emp_name,empcode as emp_code,fhname as father_name,desg_text as desig,deptt_text as department,ent_by,ent_dt,conf_dt,cardno,old_empc from " + frm_tabname + "  where  branchcd='" + frm_mbr + "' AND GRADE='" + frm_vty + "' order by emp_code";
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            if (typePopup == "N")
            {
                newCase(frm_vty);
            }
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Grade", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = "00";
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " ", 6, "VCH");
        //txt_empcode.Value = col1 + frm_vnum;       
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        txtregn.Value = frm_vnum;
        txtvchdate.Text = vardate;
        disablectrl();
        fgen.EnableForm(this.Controls);


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
            fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        //Cal_CTC();
        fgen.fill_dash(this.Controls);

        if (dd_delegate.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select No of Delegates!!");
            dd_delegate.Focus();
            return;
        }
        if (txt_cocode1.Text.Trim().Length < 2)
        {

            fgen.msg("-", "AMSG", "Please Enter Company Code!!"); return;
        }
        SQuery = "select count(cocode) as cocnt FROM SEMINAR where trim(cocode)='" + txt_cocode1.Text + "' ";
        int cocnt = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "cocnt"));
        if (cocnt >= 2)
        {
            fgen.msg("-", "AMSG", "Already Registered With this Company!!"); return;
        }
        if (txt_Vname1.Value == "-") { fgen.msg("-", "AMSG", "Please Enter Delegate Name!!"); return; }
        if (scnddiv.Visible == true)
        {
            if (txt_Vname2.Value == "-") { fgen.msg("-", "AMSG", "Please Enter Delegate Name!!"); return; }
            if (txt_mob2.Value == "-") { fgen.msg("-", "AMSG", "Please Enter Mobile No!!"); return; }

        }

        if (txt_cocode2.Value == "-") { fgen.msg("-", "AMSG", "Already Registered With this Company!!"); return; }
        if (txt_coname1.Value == "-" || txt_coname2.Value == "-") { fgen.msg("-", "AMSG", "Please Select Valid Comapny Code Or Contact to Finsys!!"); return; }
        if (txt_mob1.Value == "-") { fgen.msg("-", "AMSG", "Please Enter Mobile No of first Delegate!!"); return; }


        fgen.msg("-", "ISMSG", "Are You Sure, You Want To Save!!");
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
            fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        BlankDropDown();
        FillDropDown();
        fstdiv.Visible = false; scnddiv.Visible = false;
        newCase(frm_vty);
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        set_Val();
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.grade)||trim(a.empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                //fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), System.DateTime.Now.Date.ToString("dd/MM/yyyy"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                //fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
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
                    //txt_Category.Value = col1;
                    //txt_CatgName.Value = col2;
                    //newCase(col1);                  
                    //txt_empname.Focus();
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
                case "BTN_20":
                    break;
                case "BTN_21":
                    break;
                case "BTN_22":
                    break;
                case "BTN_23":
                    break;
                case "TICODE":
                    if (col1.Length <= 0) return;
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List_E")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select * from " + frm_tabname + " where branchcd='" + frm_mbr + "' and grade='" + frm_vty + "' and ent_dt " + PrdRange + " order by empcode";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            // fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            i = 0;
            hffield.Value = "";

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
                        save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            //frm_vnum = txt_empcode.Value.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";

                            //frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(substr(vchnum)) AS VCH FROM " + frm_tabname + " ", 6, "VCH");
                            //frm_vnum = txt_Category.Value + frm_vnum;
                            //pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + System.DateTime.Now.ToString("dd/MM/yyyy"), frm_mbr, frm_vty, frm_vnum, System.DateTime.Now.ToString("dd/MM/yyyy"), "", frm_uname);
                            // string doc_is_ok = "";
                            SQuery = "select Max(trim(vchnum)) as vchn from seminar where branchcd='" + frm_mbr + "'";
                            string mq5 = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "vchn");
                            double db1 = fgen.make_double(mq5.ToString()) + 1;
                            frm_vnum = fgen.padlc(Convert.ToInt32(db1), 6);
                            //frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, "vchnum", "vchdate", frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                            //doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                            //if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                        }
                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where trim(branchcd)||TRIM(GRADE)||trim(empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        //send_msg(frm_cocd, frm_formID, col1);
                        send_mail(frm_cocd, frm_formID, col1);
                        if (edmode.Value == "Y")
                        {
                            // fgen.msg("-", "AMSG", lblheader.Text + " " + txtregn.Value + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' and TRIM(GRADE)||trim(empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr").Substring(2, 8) + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", "You have Been Registered Successfully!!");

                                newCase(frm_vty); BlankDropDown(); FillDropDown();
                                fstdiv.Visible = false; scnddiv.Visible = false;
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved"); newCase(frm_vty); BlankDropDown();
                            }
                        }

                        //fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txt_empcode, frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls);
                        fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); fgen.EnableForm(this.Controls); newCase(frm_vty);
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
                    }
                }
            #endregion
            }
        }
    }
    //------------------------------------------------------------------------------------      
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
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        if (txt_Vname1.Value.Trim().Length > 2)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["type"] = frm_vty;
            oporow["srno"] = 1;
            oporow["branchcd"] = frm_mbr;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
            oporow["ent_dt"] = vardate;
            oporow["sem_date"] = "04/10/2019";
            oporow["cocode"] = txt_cocode1.Text.Trim().Trim().ToUpper();
            oporow["coname"] = txt_coname1.Value.Trim().Trim().ToUpper();
            oporow["vname"] = txt_Vname1.Value.Trim().Trim().ToUpper();
            oporow["desg"] = txt_desg1.Value.Trim().Trim().ToUpper();
            oporow["deptt"] = txt_deptt1.Value.Trim().Trim().ToUpper();
            oporow["emailid"] = txt_email1.Value.Trim().Trim().ToUpper();
            oporow["mobile"] = txt_mob1.Value.Trim().Trim().ToUpper();
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    void save_fun2()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        if (txt_Vname2.Value.Trim().Length > 2)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["type"] = frm_vty;
            oporow["srno"] = 2;
            oporow["branchcd"] = frm_mbr;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
            oporow["ent_dt"] = vardate;
            oporow["sem_date"] = "04/10/2019";
            oporow["cocode"] = txt_cocode2.Value.Trim().Trim().ToUpper();
            oporow["coname"] = txt_coname2.Value.Trim().Trim().ToUpper();
            oporow["vname"] = txt_Vname2.Value.Trim().Trim().ToUpper();
            oporow["desg"] = txt_desg2.Value.Trim().Trim().ToUpper();
            oporow["deptt"] = txt_deptt2.Value.Trim().Trim().ToUpper();
            oporow["emailid"] = txt_email2.Value.Trim().Trim().ToUpper();
            oporow["mobile"] = txt_mob2.Value.Trim().Trim().ToUpper();
            oDS.Tables[0].Rows.Add(oporow);
        }
    }

    void Type_Sel_query()
    {
        SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by grade_code";
    }
    //------------------------------------------------------------------------------------     
    protected void txt_cocode1_TextChanged(object sender, EventArgs e)
    {
        mq0 = txt_cocode1.Text.ToUpper();
        SQuery = "select count(cocode) as cocnt FROM SEMINAR where trim(cocode)='" + mq0 + "' ";
        mq3 = "";
        int cocnt = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "cocnt"));
        if (cocnt >= 1)
        {
            mq3 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM SEMINAR where trim(cocode)='" + mq0 + "'";
            string regno = ""; string regndt = "";
            regno = fgen.seek_iname(frm_qstr, frm_cocd, mq3, "vchnum");
            regndt = fgen.seek_iname(frm_qstr, frm_cocd, mq3, "vchdate");

            fgen.msg("-", "AMSG", "Already Registered With this Company!! '13' Registration No : " + regno + " , Dated : " + regndt + " "); clearctrl(); fgen.DisableForm(this.Controls); disablectrl(); return;
        }

        dt = new DataTable();
        SQuery = "select trim(full_name) as full_name,username from evas where userid>'000050' and trim(username)='" + mq0 + "' and nvl(amc,'-')='Y' order by userid";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "full_name");
        if (mq1 == "0")
        {
            txt_cocode2.Value = "";
            txt_coname1.Value = "";
            txt_coname2.Value = "";
            fgen.msg("-", "AMSG", "Invalid Company Code. May be Due to Non-Payment of AMC / Some Technical Glitch.'13' This Code is not appearing in the list. Kindly Contact Finsys HelpDesk For Registration.'13' (Contact No. +91-9555333195/+91-9310008914/+91-9769650423/+91-9310008916) ");
            fgen.DisableForm(this.Controls); disablectrl();
            return;
        }
        else
        {
            txt_cocode2.Value = mq0;
            txt_cocode1.Text = mq0;
            txt_coname1.Value = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "full_name");
            txt_coname2.Value = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "full_name");
        }
    }
    protected void FillDropDown()
    {
        SQuery = "SELECT 'PLEASE SELECT' AS FSTR FROM DUAL UNION ALL SELECT '1' AS FSTR FROM DUAL UNION ALL SELECT '2' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_delegate.DataSource = dt;
        dd_delegate.DataTextField = "fstr";
        dd_delegate.DataValueField = "fstr";
        dd_delegate.DataBind();
    }
    protected void BlankDropDown()
    {
        dd_delegate.Items.Clear();
    }
    protected void dd_delegate_SelectedIndexChanged(object sender, EventArgs e)
    {
        mq0 = dd_delegate.SelectedItem.Text.Trim().ToUpper();
        if (mq0 == "1")
        {
            fstdiv.Visible = true; scnddiv.Visible = false;
        }
        else if (mq0 == "2") { fstdiv.Visible = true; scnddiv.Visible = true; }
        else
        {
            fstdiv.Visible = false; scnddiv.Visible = false;
        }
    }

    public void send_mail(string cocd, string formID, string appr_Status)
    {
        string emailTo = "", emailCC = "", emailSubj = "";
        System.Text.StringBuilder stb = new System.Text.StringBuilder();
        string username = frm_uname;
        stb.Append("<html><body>");
        stb.Append("<b>Dear Patron,</b> <br><br>");
        stb.Append("Team Tejaxo is Eagerly looking forward to recieve you at the seminar. <br><br>");
        stb.Append("We hope that you will be hugely benefited by the content and eminent speaker of the seminar. <br><br>");

        stb.Append("See You at the Venue. & Regards, <br><br>");

        stb.Append("<b>Thanks & Regards,</b> <br><br>");

        stb.Append("<b>Team Tejaxo</b><br><br>");
        stb.Append("<a href=\"www.pocketdriver.in\" target=\"_blank\">www.pocketdriver.in</a> | email :<a href=\"virender@pocketdriver.in\" target=\"_blank\">virender@pocketdriver.in </a> | Mobile : 9310008916 <br> ");
        stb.Append("Support numbers 9015-220-220 (10 Lines) |, <br>");
        stb.Append("We make Software, for increasing the Smoothness of your Business Operations., <br>");
        stb.Append("Pocketdriver Limited, the OEM of Tejaxo ERP packages, <br><br>");
        stb.Append("<b>Note: This is the system generated E-Mail. Please do not Reply in case any clarification contact Finsys Team</b><br><br>");
        emailSubj = "Delegate Regstration for Upcoming Seminar";
        stb.Append("</body></html>");
        emailTo = txt_email1.Value;
        emailCC = "accounts@pocketdriver.in,modi@pocketdriver.in";
        if (stb.ToString().Length > 2 && emailTo.Length > 2)
            fgen.send_mail(cocd, "Tejaxo ERP", emailTo, emailCC, "", emailSubj, stb.ToString());

    }
}

