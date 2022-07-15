using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Drawing;

public partial class om_pap_insp : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "";
    DataTable dt, dt1, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0, flag = 0;

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

        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = true;
        tab2.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        btnprint.Visible = true;

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnvalidat.Disabled = true;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnlist.Disabled = false;
        btnprint.Disabled = false;
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
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnvalidat.Disabled = false;
        btnlbl4.Enabled = true; btnprint.Disabled = true;
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
        frm_tabname = "papinsp";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");

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

            case "PARTYCODE":
                SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(b.aname) as Party_name,trim(a.acode) as acode,trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,sum(a.iqtyin) as Tot_Q from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type in ('02','07','08') and a.vchdate " + DateRange + " /*and substr(a.icode,1,2) in ('07','80','81')*/ and nvl(a.inspected,'-')!='Y' and nvl(a.store,'N')<>'R' group by a.branchcd,a.type,a.vchnum,a.vchdate,b.aname,a.acode,a.invno,a.invdate order by a.vchdate desc,a.vchnum desc";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                if (sg1.Rows.Count > 1)
                {
                    col1 = "";
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[17].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[17].Text.Trim() + "'";
                    }
                    SQuery = "Select a.kclreelno as fstr,a.kclreelno as Reel_No,b.Oprate1 as P_Size,b.oprate3 as P_GSM,a.icode,a.reelwin as Wt_Reel,a.srno,a.Coreelno from reelvch a, item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%'and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + txtlbl4.Text.Trim() + txtlbl4a.Text.Trim() + "' and TRIM(kclreelno) not in (" + col1 + ") order by a.srno";
                }
                else
                {
                    SQuery = "Select a.kclreelno as fstr,a.kclreelno as Reel_No,b.Oprate1 as P_Size,b.oprate3 as P_GSM,a.icode,a.reelwin as Wt_Reel,a.srno,a.Coreelno from reelvch a, item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + txtlbl4.Text.Trim() + txtlbl4a.Text.Trim() + "' order by a.srno";
                }
                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[12].Text;
                SQuery = "";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                }
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr ,trim(b.aname) as Supplier,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.MRRNUM,to_char(a.MRRdate,'dd/mm/yyyy') as MRRdate,a.INVNO,to_char(a.invdate,'dd/mm/yyyy') as invdate,A.type from papinsp A,famst b WHERE trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vchnum desc";
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
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_vty = "10";
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
        // btnvalidat.Disabled = true;
    }

    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' and vchdate " + DateRange + " AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl4a.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

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
        cal();
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        double db = 0, db1 = 0, db2 = 0;
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }

        if (txtlbl4.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Party First!!");
            return;
        }
        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Select Item");
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
        frm_vty = "10";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        string cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table               
                SQuery = "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + cond + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + cond + "'");

                // SQuery="delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + cond + "'";
                // Deleing data from Sr Ctrl Table
                // fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + cond + "'");
                // Saving Deleting History

                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, cond.Substring(0, 6), cond.Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                    break;

                case "COPY_OLD":
                    #region Copy from Old Temp
                    //if (col1 == "") return;
                    //clearctrl();
                    //SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join FIN_MSYS b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count > 0)
                    //{

                    //    txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                    //    txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                    //    txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                    //    txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                    //    txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                    //    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                    //    create_tab();
                    //    sg1_dr = null;
                    //    for (i = 0; i < dt.Rows.Count; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    //        sg1_dr["sg1_h1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                    //        sg1_dr["sg1_h2"] = dt.Rows[i]["text"].ToString().Trim();
                    //        sg1_dr["sg1_h3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                    //        sg1_dr["sg1_h4"] = "-";
                    //        sg1_dr["sg1_h5"] = "-";
                    //        sg1_dr["sg1_h6"] = "-";
                    //        sg1_dr["sg1_h7"] = "-";
                    //        sg1_dr["sg1_h8"] = "-";
                    //        sg1_dr["sg1_h9"] = "-";
                    //        sg1_dr["sg1_h10"] = "-";

                    //        sg1_dr["sg1_f1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                    //        sg1_dr["sg1_f2"] = dt.Rows[i]["text"].ToString().Trim();
                    //        sg1_dr["sg1_f3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                    //        sg1_dr["sg1_f4"] = "-";
                    //        sg1_dr["sg1_f5"] = "-";

                    //        sg1_dr["sg1_t1"] = dt.Rows[i]["OBJ_NAME"].ToString().Trim();
                    //        sg1_dr["sg1_t2"] = dt.Rows[i]["OBJ_CAPTION"].ToString().Trim();
                    //        sg1_dr["sg1_t3"] = dt.Rows[i]["OBJ_WIDTH"].ToString().Trim();
                    //        sg1_dr["sg1_t4"] = dt.Rows[i]["OBJ_VISIBLE"].ToString().Trim();
                    //        sg1_dr["sg1_t5"] = dt.Rows[i]["col_no"].ToString().Trim();
                    //        sg1_dr["sg1_t6"] = dt.Rows[i]["obj_maxlen"].ToString().Trim();
                    //        sg1_dr["sg1_t7"] = "";

                    //        if (frm_tabname.ToUpper() == "SYS_CONFIG")
                    //        {
                    //            sg1_dr["sg1_t7"] = dt.Rows[i]["OBJ_READONLY"].ToString().Trim();
                    //        }

                    //        sg1_dr["sg1_t8"] = "";

                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }

                    //    sg1_add_blankrows();
                    //    ViewState["sg1"] = sg1_dt;
                    //    sg1_add_blankrows();
                    //    sg1.DataSource = sg1_dt;
                    //    sg1.DataBind();
                    //    dt.Dispose(); sg1_dt.Dispose();
                    //    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                    //    fgen.EnableForm(this.Controls);
                    //    disablectrl();
                    //    setColHeadings();
                    //}
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
                    SQuery = "select a.*, trim(b.aname) as aname from papinsp A,famst b WHERE trim(A.acode)=trim(B.acode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno asc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl7.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["MRRNUM"].ToString().Trim();
                        txtlbl4a.Text = Convert.ToDateTime(dt.Rows[i]["MRRDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[i]["INVNO"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[i]["INVDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4b.Text = dt.Rows[i]["TARGETBF"].ToString().Trim();
                        txtlbl22.Text = dt.Rows[i]["STIFNESS"].ToString().Trim();
                        txtlbl21.Text = dt.Rows[i]["PHVALUE"].ToString().Trim();
                        txtlbl20.Text = dt.Rows[i]["INSP_STAT"].ToString().Trim();
                        txtrmk.Text = dt.Rows[i]["Remarks"].ToString().Trim();

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_f15"] = dt.Rows[i]["CO_RE_EL"].ToString().Trim();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["MRRGSM"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["MRRSIZE"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["ICODE"].ToString().Trim();
                            sg1_dr["sg1_srno"] = dt.Rows[i]["SRNO"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["REELNO"].ToString().Trim();

                            sg1_dr["sg1_t2"] = dt.Rows[i]["REELDIA"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["REELWT"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["ACTSIZE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["ACTGSM1"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["ACTGSM2"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["AVGGSM"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["BS1"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["BS2"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["BS3"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["BS4"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["AVGBS"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["BFACTOR"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["MOISTURE"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["FOLD"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["COLBVALUE"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["RCT1"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["RCT2"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["RCT3"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["CALIPER"].ToString().Trim();
                            sg1_dr["sg1_t21"] = dt.Rows[i]["APPEARANCE"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        SQuery1 = "select trim(a.acode) as acode,trim(A.Psize) as psize,trim(A.gsm) as gsm,a.iqty_chl,trim(a.icode) as icode,a.iqty_chlwt,a.ponum,a.podate,trim(b.iname) as item_name from ivoucher A, item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and SUBSTR(A.TYPE,1,1)='0' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + txtlbl4.Text.Trim() + txtlbl4a.Text.Trim() + "' and nvl(a.store,'N')<>'R' order by a.icode,a.srno";

                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

                        create_tab2();
                        sg2_dr = null;
                        for (i = 0; i < dt1.Rows.Count; i++)
                        {

                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = i + 1;
                            sg2_dr["sg2_f1"] = dt1.Rows[i]["iqty_chlwt"].ToString().Trim();
                            sg2_dr["sg2_f2"] = dt1.Rows[i]["psize"].ToString().Trim();
                            sg2_dr["sg2_f3"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            sg2_dr["sg2_f4"] = dt1.Rows[i]["iqty_chl"].ToString().Trim();
                            sg2_dr["sg2_f5"] = dt1.Rows[i]["item_name"].ToString().Trim();
                            sg2_dr["sg2_f6"] = dt1.Rows[i]["icode"].ToString().Trim();

                            sg2_dt.Rows.Add(sg2_dr);

                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();

                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        sg2_dt.Dispose();

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
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "A17");
                    fgen.fin_sales_reps(frm_qstr);
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

                case "PARTYCODE":  // at the time of mrr selection
                    if (col1.Length <= 0) return;
                    SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(b.aname) as Party_name,trim(a.acode) as acode,trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,sum(a.iqtyin) as Tot_Q from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.type in ('02','07','08') /*and substr(a.icode,1,2) in ('07','80','81')*/ and nvl(a.inspected,'-')!='Y' and nvl(a.store,'N')<>'R' and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + cond + "' group by a.branchcd,a.type,a.vchnum,a.vchdate,b.aname,a.acode,a.invno,a.invdate order by a.vchdate desc,a.vchnum desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    SQuery1 = "select trim(a.acode) as acode,trim(A.Psize) as psize,trim(A.gsm) as gsm,a.iqty_chl,trim(a.icode) as icode,a.iqty_chlwt,a.ponum,a.podate,trim(b.iname) as item_name from ivoucher A, item b where trim(a.icode)=trim(b.icode) and  SUBSTR(A.TYPE,1,1)='0' and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + cond + "' and nvl(a.store,'N')<>'R' order by a.icode,a.srno";

                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = dt.Rows[i]["vchnum"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["vchdate"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[i]["party_name"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["invno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["invdate"].ToString().Trim();
                        create_tab2();
                        sg2_dr = null;
                        for (i = 0; i < dt1.Rows.Count; i++)
                        {

                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_f1"] = dt1.Rows[i]["iqty_chlwt"].ToString().Trim();
                            sg2_dr["sg2_f2"] = dt1.Rows[i]["psize"].ToString().Trim();
                            sg2_dr["sg2_f3"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            sg2_dr["sg2_f4"] = dt1.Rows[i]["iqty_chl"].ToString().Trim();
                            sg2_dr["sg2_f5"] = dt1.Rows[i]["item_name"].ToString().Trim();
                            sg2_dr["sg2_f6"] = dt1.Rows[i]["icode"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        ViewState["sg2"] = sg2_dt;

                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose(); sg2_dt.Dispose();
                    }
                    break;

                case "SG1_ROW_ADD":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    if (ViewState["sg1"] != null)
                    {

                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = dt.Rows[i]["sg1_srno"].ToString();
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

                            sg1_dr["sg1_f15"] = dt.Rows[i]["sg1_f15"].ToString();

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
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        SQuery = "Select a.kclreelno as Reel_No,b.Oprate1 as P_Size,b.oprate3 as P_GSM,a.icode,a.reelwin as Wt_Reel,a.srno,a.Coreelno from reelvch a, item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.kclreelno='" + col1 + "' order by a.srno";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
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
                            sg1_dr["sg1_f15"] = dt.Rows[d]["Coreelno"].ToString().Trim();
                            sg1_dr["sg1_f1"] = dt.Rows[d]["P_GSM"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["P_Size"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["Reel_No"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["icode"].ToString().Trim();


                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = dt.Rows[d]["Wt_Reel"].ToString().Trim();
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "0";
                            sg1_dr["sg1_t8"] = "";
                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "0";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t16"] = "";
                            sg1_dr["sg1_t17"] = "";
                            sg1_dr["sg1_t18"] = "";
                            sg1_dr["sg1_t19"] = "";
                            sg1_dr["sg1_t20"] = "";
                            sg1_dr["sg1_t21"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    dt = new DataTable();
                    //txtlbl101.Text = col1;
                    //xStartDt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='R10'", "PARAMS");
                    //if (txtlbl101.Text == "No")
                    //{
                    //    SQuery = "select invno||invdate as fstr,invno as Job_No,invdate as Job_Dt,vchnum as Iss_no,vchdate as Iss_Dt,sum(iss)-sum(taken) as Balance,a.icode,trim(b.iname) as iname,b.unit,b.cpartno from (select trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,iqtyout as iss,0 as taken,icode from ivoucher where branchcd='" + frm_mbr + "' and type in('30','31','32') and substr(icode,1,2) in('01','02','03','07','10','81') and vchdate>=to_datE('" + xStartDt + "','dd/mm/yyyy') union all select trim(job_no) as job_no,trim(job_Dt)As job_dt,trim(var_code) as var_code,trim(glue_code) as glue_code,0 as iss,a1 as taken,icode from prod_sheet where branchcd='" + frm_mbr + "' and type='85' and vchdate>=to_Date('" + xStartDt + "','dd/mm/yyyy'))a,item b where trim(a.icode)=trim(b.icode) and trim(a.invno)||trim(a.invdate)='" + col1 + "' group by invno,invdate,vchnum,vchdate,a.icode,trim(b.iname),b.unit,b.cpartno order by invno desc";
                    //}
                    //if (txtlbl101.Text == "Yes")
                    //{
                    //    SQuery = "select invno||invdate as fstr,invno as Job_No,invdate as Job_Dt,vchnum as Iss_no,vchdate as Iss_Dt,sum(iss)-sum(taken) as Balance,a.icode,trim(b.iname) as iname,b.unit,b.cpartno from (select trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,iqtyout as iss,0 as taken,icode from ivoucher where branchcd='" + frm_mbr + "' and type in('30','31','32') and substr(icode,1,2) in('01','02','03','07','10','81') and vchdate>=to_datE('" + xStartDt + "','dd/mm/yyyy') union all select trim(job_no) as job_no,trim(job_Dt)As job_dt,trim(var_code) as var_code,trim(glue_code) as glue_code,0 as iss,a1 as taken,icode from prod_sheet where branchcd='" + frm_mbr + "' and type='85' and vchdate>=to_Date('" + xStartDt + "','dd/mm/yyyy'))a,item b  where trim(a.icode)=trim(b.icode) and trim(a.invno)||trim(a.invdate)='" + col1 + "' group by invno,invdate,vchnum,vchdate,a.icode,trim(b.iname),b.unit,b.cpartno having sum(iss)-sum(taken) >0 order by invno desc";
                    //}
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (col1.Length <= 0) return;
                    //for (int d = 0; d < dt.Rows.Count; d++)
                    //{
                    //    //********* Saving in Hidden Field 
                    //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[d]["Job_Dt"].ToString().Trim();
                    //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["Job_No"].ToString().Trim();
                    //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["icode"].ToString().Trim();
                    //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["INAME"].ToString().Trim();
                    //    //********* Saving in GridView Value
                    //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["unit"].ToString().Trim();
                    //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["Balance"].ToString().Trim();
                    //    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[d]["Balance"].ToString().Trim();
                    //}
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD1":
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
                            sg1_dr["sg1_f18"] = dt.Rows[i]["sg1_f18"].ToString();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                            sg1_dr["sg1_f15"] = dt.Rows[i]["sg1_f15"].ToString().Trim();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                            sg1_dr["sg1_f18"] = dt.Rows[i]["sg1_f18"].ToString();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["sg1_f6"].ToString();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["sg1_f7"].ToString();
                            sg1_dr["sg1_f8"] = dt.Rows[i]["sg1_f8"].ToString();
                            sg1_dr["sg1_f9"] = dt.Rows[i]["sg1_f9"].ToString();

                            sg1_dr["sg1_f10"] = dt.Rows[i]["sg1_f10"].ToString();
                            sg1_dr["sg1_f11"] = dt.Rows[i]["sg1_f11"].ToString();
                            sg1_dr["sg1_f12"] = dt.Rows[i]["sg1_f12"].ToString();
                            sg1_dr["sg1_f13"] = dt.Rows[i]["sg1_f13"].ToString();
                            sg1_dr["sg1_f14"] = dt.Rows[i]["sg1_f14"].ToString();
                            sg1_dr["sg1_f15"] = dt.Rows[i]["sg1_f15"].ToString();
                            sg1_dr["sg1_f16"] = dt.Rows[i]["sg1_f16"].ToString();
                            sg1_dr["sg1_f17"] = dt.Rows[i]["sg1_f17"].ToString();

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

                        string stage = "0"; string stagename = "";
                        hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL13");
                        int RowInsertAt = Convert.ToInt32(hf1.Value);
                        stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text + sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text;

                        dt = new DataTable();
                        SQuery = "select trim(a.type)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, trim(a.vchnum) as vchnum,trim(a.acode) as code,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate ,trim(a.icode) as icode,trim(b.iname) as iname,trim(a.unit) as unit,a.iqtyin from ivoucher a, item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='09' and a.vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(a.type)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") ";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            if (d == 0)
                            {
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
                                sg1_dr["sg1_f15"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.ToString().Trim();
                                sg1_dr["sg1_f1"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text.ToString().Trim();
                                sg1_dr["sg1_f2"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text.ToString().Trim();
                                sg1_dr["sg1_f3"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text.ToString().Trim();
                                sg1_dr["sg1_f4"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text.ToString().Trim();
                                sg1_dr["sg1_f5"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text.ToString().Trim();
                                sg1_dr["sg1_f18"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[19].Text.ToString().Trim();
                                sg1_dr["sg1_f6"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[20].Text.ToString().Trim();

                                sg1_dr["sg1_f7"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[21].Text.ToString().Trim();
                                sg1_dr["sg1_f8"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[22].Text.ToString().Trim();


                                sg1_dr["sg1_f16"] = dt.Rows[d]["vchnum"].ToString().Trim();
                                sg1_dr["sg1_f17"] = dt.Rows[d]["vchdate"].ToString().Trim();
                                sg1_dr["sg1_f9"] = dt.Rows[d]["invno"].ToString().Trim();
                                sg1_dr["sg1_f10"] = dt.Rows[d]["invdate"].ToString().Trim();
                                sg1_dr["sg1_f11"] = dt.Rows[d]["icode"].ToString().Trim();
                                sg1_dr["sg1_f12"] = dt.Rows[d]["iname"].ToString().Trim();
                                sg1_dr["sg1_f13"] = dt.Rows[d]["unit"].ToString().Trim();
                                sg1_dr["sg1_f14"] = dt.Rows[d]["iqtyin"].ToString().Trim();
                                sg1_dt.Rows.InsertAt(sg1_dr, RowInsertAt);
                            }
                            RowInsertAt++;
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        int d = 0;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[24].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[25].Text = dt.Rows[d]["vchdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[26].Text = dt.Rows[d]["invno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[27].Text = dt.Rows[d]["invdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[28].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[29].Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[30].Text = dt.Rows[d]["unit"].ToString().Trim();
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f14"))).Text = dt.Rows[d]["iqtyin"].ToString().Trim();

                    }
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    #endregion
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
                        dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
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
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f15"] = sg1.Rows[i].Cells[12].Text.Trim();

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
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        //if (edmode.Value == "Y")
                        //{
                        //    sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                        //    sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        //}
                        //else
                        //{
                        //    sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        //}

                        sg1_add_blankrows();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
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
        frm_vty = "10";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

            SQuery = "select TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,TRIM(invno) AS invno,TO_CHAR(invDATE,'DD/MM/YYYY') AS invdate,reelno, targetbf from papinsp where branchcd='" + frm_mbr + "' and type ='" + frm_vty + "' and vchdate " + PrdRange + "";
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
                            save_it = "Y";
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

        sg1_dt.Columns.Add(new DataColumn("sg1_f15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
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

        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t21", typeof(string)));


    }

    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));

        sg2_dt.Columns.Add(new DataColumn("sg2_h1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h10", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f7", typeof(string)));
    }

    public void create_tab3()
    {
        sg3_dt = new DataTable();
        sg3_dr = null;
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

            sg1_dr["sg1_f1"] = "-";
            sg1_dr["sg1_f2"] = "-";
            sg1_dr["sg1_f3"] = "-";
            sg1_dr["sg1_f4"] = "-";
            sg1_dr["sg1_f15"] = "-";

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
            sg1_dr["sg1_t17"] = "-";
            sg1_dr["sg1_t18"] = "-";
            sg1_dr["sg1_t19"] = "-";
            sg1_dr["sg1_t20"] = "-";
            sg1_dr["sg1_t21"] = "-";
            sg1_dr["sg1_f5"] = "-";
            sg1_dt.Rows.Add(sg1_dr);
        }
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
                    fgen.Fn_open_sseek("Select Reel No", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Reel No", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD1":
                if (sg1.Rows[Convert.ToInt32(index)].Cells[13].Text.Trim().Length > 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "SG1_ROW_ADD1";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select item", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Challan First!!");
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
        hffield.Value = "PARTYCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select MRR", frm_qstr);
    }

    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TYPE";
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
        hffield.Value = "MACHNECODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["SRNO"] = i + 1;

                oporow["TARGETBF"] = txtlbl4b.Text.Trim();

                oporow["MRRNUM"] = txtlbl4.Text.Trim().ToUpper();
                oporow["mrrdate"] = txtlbl4a.Text.Trim();
                oporow["mrrtype"] = "-";
                oporow["INVNO"] = txtlbl2.Text.Trim().ToUpper();
                oporow["invdate"] = txtlbl3.Text.Trim();
                oporow["acode"] = txtlbl7.Text.Trim();
                oporow["CO_RE_EL"] = sg1.Rows[i].Cells[12].Text.Trim().ToUpper();
                oporow["mrrgsm"] = fgen.make_double(sg1.Rows[i].Cells[14].Text.Trim().ToUpper());
                oporow["mrrsize"] = fgen.make_double(sg1.Rows[i].Cells[13].Text.Trim().ToUpper());
                oporow["icode"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
                oporow["reelno"] = fgen.make_double(sg1.Rows[i].Cells[17].Text.Trim().ToUpper());

                oporow["reeldia"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t2"))).Text.Trim().ToUpper();
                oporow["reelwt"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t3"))).Text.Trim().ToUpper();
                oporow["actsize"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t4"))).Text.Trim().ToUpper();
                oporow["actgsm1"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text.Trim().ToUpper();
                oporow["actgsm2"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t6"))).Text.Trim().ToUpper();
                oporow["avggsm"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t7"))).Text.Trim().ToUpper();
                oporow["bs1"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t8"))).Text.Trim().ToUpper();
                oporow["bs2"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t9"))).Text.Trim().ToUpper();
                oporow["bs3"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t10"))).Text.Trim().ToUpper();
                oporow["bs4"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t11"))).Text.Trim().ToUpper();
                oporow["avgbs"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t12"))).Text.Trim().ToUpper();
                oporow["bfactor"] = fgen.Make_decimal(((TextBox)(sg1.Rows[i].FindControl("sg1_t13"))).Text.Trim().ToUpper());
                oporow["moisture"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t14"))).Text.Trim().ToUpper();
                oporow["fold"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t15"))).Text.Trim().ToUpper();
                oporow["colbvalue"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t16"))).Text.Trim().ToUpper();
                oporow["rct1"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t17"))).Text.Trim().ToUpper();
                oporow["rct2"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t18"))).Text.Trim().ToUpper();
                oporow["rct3"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t19"))).Text.Trim().ToUpper();
                oporow["caliper"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t20"))).Text.Trim().ToUpper();
                oporow["Appearance"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t21"))).Text.Trim().ToUpper();
                oporow["TARGETGSM"] = fgen.make_double(sg1.Rows[i].Cells[39].Text.Trim().ToUpper());

                oporow["insp_stat"] = txtlbl20.Text.Trim();

                if (fgen.make_double(oporow["avggsm"].ToString().Trim() + 10) < fgen.make_double(oporow["mrrgsm"].ToString().Trim()))
                {
                    oporow["REELSTAT"] = "Non Acceptable";
                }
                else
                {
                    oporow["REELSTAT"] = "Acceptable";
                }
                oporow["phvalue"] = txtlbl21.Text.Trim().ToUpper();
                oporow["STIFNESS"] = txtlbl22.Text.Trim().ToUpper();
                oporow["Remarks"] = txtrmk.Text.Trim().ToUpper();

                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = ViewState["entby"].ToString();
                    oporow["eNt_dt"] = ViewState["entdt"].ToString();
                    //oporow["edt_by"] = frm_uname;
                    // oporow["edt_dt"] = vardate;
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    //oporow["edt_by"] = "-";
                    // oporow["edt_dt"] = vardate;
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }

    protected void txt_TextChanged(object sender, EventArgs e)
    {
        string dttoh = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
        string dttom = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
        string dtfromh = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
        string dtfromm = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;


        DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
        DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

        int timeDiff = dtFrom.Subtract(dtTo).Hours;
        int timediff2 = dtFrom.Subtract(dtTo).Minutes;


        TextBox txtName = ((TextBox)sg1.Rows[i].FindControl("sg1_t5"));
        txtName.Text = timeDiff.ToString();

        TextBox txtName1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t6"));
        txtName1.Text = timediff2.ToString();
    }
    //------------------------------------------------------------------------------------
    protected void btnProdReport_Click(object sender, EventArgs e)
    {
        hffield.Value = "ProdRep";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //----------------------------------------------------------------------------
    protected void cal()
    {
        fgen.fill_zero(this.Controls);
        try
        {
            foreach (GridViewRow r1 in sg1.Rows)
            {
                ((TextBox)r1.FindControl("sg1_t7")).Text = Math.Round((fgen.make_double(((TextBox)r1.FindControl("sg1_t5")).Text) + fgen.make_double(((TextBox)r1.FindControl("sg1_t6")).Text)) / 2, 2).ToString(); // for avg GSm
                ((TextBox)r1.FindControl("sg1_t12")).Text = Math.Round((fgen.make_double(((TextBox)r1.FindControl("sg1_t8")).Text) + fgen.make_double(((TextBox)r1.FindControl("sg1_t9")).Text) + fgen.make_double(((TextBox)r1.FindControl("sg1_t10")).Text) + fgen.make_double(((TextBox)r1.FindControl("sg1_t11")).Text)) / 4, 2).ToString(); //for avg BS
                ((TextBox)r1.FindControl("sg1_t13")).Text = Math.Round((fgen.make_double(((TextBox)r1.FindControl("sg1_t12")).Text) / fgen.make_double(((TextBox)r1.FindControl("sg1_t7")).Text)) * 1000, 2).ToString(); // for bfactor
            }
        }
        catch { fgen.fill_zero(this.Controls); }
    }
    protected void btnvalidate_Click(object sender, EventArgs e)
    {

    }
    protected void btnvalidat_ServerClick(object sender, EventArgs e)
    {
        btnsave.Disabled = false;
        string mq0, mq1, mq2, mq3, mq4;
        int req = 0, i = 0;
        //tollerance value as per MCPL
        double tol_moisture = 1; double tol_bs = 5 / 100;
        double tol_ring = 5 / 100;

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            mq1 = sg1.Rows[i].Cells[15].Text.Trim();
            mq0 = "select branchcd,icode,mat4 as std_bf,mat5 as std_bs,mat8 as std_ring , mqty9 as moisture from item where icode='" + mq1.Substring(0, 4) + "'";
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
            if (dt2.Rows.Count > 0)
            {
                if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text) < fgen.make_double(dt2.Rows[0]["moisture"].ToString()) - tol_moisture || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text) > fgen.make_double(dt2.Rows[0]["moisture"].ToString()) + tol_moisture)
                {
                    flag = 1;
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).BackColor = Color.IndianRed;
                    req = req + 1;
                }

                else
                {
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).BackColor = Color.White;
                }

                if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text) < fgen.make_double(dt2.Rows[0]["moisture"].ToString()) - tol_moisture || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text) > fgen.make_double(dt2.Rows[0]["moisture"].ToString()) + tol_moisture)
                {
                    flag = 1;
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).BackColor = Color.IndianRed;
                    req = req + 1;
                }
                else
                {
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).BackColor = Color.White;
                }

                if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text) < fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) - (fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) * tol_ring) || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text) > fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) + (fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) * tol_ring))
                {
                    flag = 1;
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).BackColor = Color.IndianRed;
                    req = req + 1;
                }
                else
                {
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).BackColor = Color.White;
                }

                if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text) < fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) - (fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) * tol_ring) || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text) > fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) + (fgen.make_double(dt2.Rows[0]["std_ring"].ToString()) * tol_ring))
                {
                    flag = 1;
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).BackColor = Color.IndianRed;
                    req = req + 1;
                }
                else
                {
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).BackColor = Color.White;
                }

                if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text) < fgen.make_double(dt2.Rows[0]["std_bs"].ToString()) - (fgen.make_double(dt2.Rows[0]["std_bs"].ToString()) * tol_bs) || fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text) > fgen.make_double(dt2.Rows[0]["std_bs"].ToString()) + (fgen.make_double(dt2.Rows[0]["std_bs"].ToString()) * tol_bs))
                {
                    flag = 1;
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).BackColor = Color.IndianRed;
                    req = req + 1;
                }
                else
                {
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).BackColor = Color.White;
                }
            }
            if (req != 0)
            {
                sg1.Rows[i].Cells[39].Text = req.ToString();
                req = 0;
            }
        }
    }
}