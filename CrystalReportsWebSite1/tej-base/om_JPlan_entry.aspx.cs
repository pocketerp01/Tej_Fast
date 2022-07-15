using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_JPlan_entry : System.Web.UI.Page
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
    string ind_Ptype = "";
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
                    ind_Ptype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");

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
        if (dtCol == null) return;
        if (sg1.Rows.Count <= 0) return;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

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
                //  if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_name") == "SG1_F21") { }
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }

        //txtlbl8.Attributes.Add("readonly", "readonly");
        //txtlbl9.Attributes.Add("readonly", "readonly");

        // to hide and show to tab panel

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F35106":
                tab5.Visible = false;
                tab4.Visible = false;
                tab2.Visible = false;
                tab3.Visible = false;
                break;
        }
        if (Prg_Id == "M12008")
        {
            tab5.Visible = true;
            txtlbl8.Attributes.Remove("readonly");
            txtlbl9.Attributes.Remove("readonly");
        }
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

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose(); //fetch_col_rejection();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;

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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "PROD_SHEET";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "90");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
        if (lbl1.Text == "lbl1") lbl1.Text = "Plan.No";
        if (lblheader.Text == "") lblheader.Text = "Machine Loading (Machine Planning)";
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
                //pop1                
                SQuery = "select type1 as fstr,NAME,place,type1,id from type where id='D' and substr(type1,1,1)='1' order by name ";
                break;
            case "TICODE":
                //pop2
                SQuery = "select mchname as Machine_Name,trim(acode)||'/'||srno as Machine_Code,mch_seq, mchname as mch from pmaint where branchcd='" + frm_mbr + "' and type='10' order by acode,srno";
                break;

            case "STG":
                SQuery = "select trim(type1) as fstr,name,type1 from type where id='K' order by type1";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 2)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " and TRIM(icode) not in (" + col1 + ")";
                }

                else
                {
                    col1 = "";
                }
                SQuery = "SELECT Icode AS FSTR,Iname AS Item_Name,Cpartno,Cdrgno,unit,ent_by,Icode FROM Item where branchcd!='DD' and length(Trim(deac_by))<2  and length(Trim(icode))>4 " + col1 + " ORDER BY Iname ";
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
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "WPrint_E":
            case "Print_E":
                SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as wo_no,to_char(a.vchdate,'dd/mm/yyyy') as dated,t.name as stage,a.prevcode as shift,a.job_no,a.job_dt,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,type t where trim(a.stage)=trim(t.type1) and t.id='K' and a.branchcd='" + frm_mbr + "' and a.type='90' and a.vchdate " + DateRange + " order by vdd desc,wo_no desc";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as wo_no,to_char(a.vchdate,'dd/mm/yyyy') as dated,t.name as stage,a.prevcode as shift,a.job_no,a.job_dt,i.iname,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet a,item i,type t where trim(a.icode)=trim(i.icode) and trim(a.stage)=trim(t.type1) and t.id='K' and a.branchcd='" + frm_mbr + "' and a.type='90' and a.vchdate " + DateRange + " order by vdd desc,wo_no desc";
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
            hffield.Value = "New";
            frm_vty = "90";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            lbl1a.Text = "90";
            if (edmode.Value == "")
            {
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
                txtvchnum.Text = frm_vnum;
            }
            fgen.Fn_open_prddmp1("", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
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
            fgen.Fn_open_sseek("Select Work Order Entry To Edit", frm_qstr);
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
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        //checks
        //-----------------------------------------------------------------------
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date");
            txtvchdate.Focus();
            return;
        }
        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;
        if (txtlbl4.Text.Trim().Length == 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl4.Text;
        }
        if (txtlbl7.Text.Trim().Length == 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl7.Text;
        }

        if (txtlbl101.Text.Trim().Length == 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl101.Text;
        }
        if (sg2.Rows.Count < 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Some Items In Second Grid";
        }
        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please Fill " + reqd_flds);
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
            fgen.Fn_open_sseek("Select Work Order Entry To Delete", frm_qstr);
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
        // fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        vty = "90";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
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
                    newCase(col1);
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
                    SQuery = "select a.*,i.iname,t.name,i.cpartno,to_char(a.vchdate,'dd/mm/yyyy') as vch_date from prod_sheet a,item i,type t where trim(a.icode)=trim(i.icode) and trim(a.stage)=trim(t.type1) and t.id='K' and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = dt.Rows[0]["vch_date"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["shftcode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["prevcode"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["mchcode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["ename"].ToString().Trim();
                        txtlbl101.Text = dt.Rows[0]["stage"].ToString().Trim();
                        txtlbl101a.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["naration"].ToString().Trim();
                        txtlbl8.Text = dt.Rows[0]["subcode"].ToString().Trim();
                        create_tab2();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            //sg1_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_h1"] = dt.Rows[i]["job_dt"].ToString().Trim();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["job_no"].ToString().Trim();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg2_dr["sg2_h4"] = fgen.seek_iname(frm_qstr, frm_cocd, "select acode from famst where aname='" + dt.Rows[i]["tempr"].ToString().Trim() + "'", "acode");
                            sg2_dr["sg2_h5"] = dt.Rows[i]["tempr"].ToString().Trim();
                            sg2_dr["sg2_h6"] = fgen.seek_iname(frm_qstr, frm_cocd, "select col18 from costestimate where branchcd='" + frm_mbr + "' and type='30' and icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and vchnum='" + dt.Rows[i]["job_no"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[i]["job_dt"].ToString().Trim() + "'", "col18");
                            sg2_dr["sg2_h7"] = fgen.seek_iname(frm_qstr, frm_cocd, "select col19 from costestimate where branchcd='" + frm_mbr + "' and type='30' and icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and vchnum='" + dt.Rows[i]["job_no"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[i]["job_dt"].ToString().Trim() + "'", "col19");
                            sg2_dr["sg2_h8"] = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select col14 from costestimate where branchcd='" + frm_mbr + "' and type='30' and icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and vchnum='" + dt.Rows[i]["job_no"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[i]["job_dt"].ToString().Trim() + "'", "col14")) + fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select col15 from costestimate where branchcd='" + frm_mbr + "' and type='30' and icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and vchnum='" + dt.Rows[i]["job_no"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[i]["job_dt"].ToString().Trim() + "'", "col15"));
                            sg2_dr["sg2_h9"] = fgen.seek_iname(frm_qstr, frm_cocd, "select col13 from costestimate where branchcd='" + frm_mbr + "' and type='30' and icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and vchnum='" + dt.Rows[i]["job_no"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[i]["job_dt"].ToString().Trim() + "'", "col13");
                            sg2_dr["sg2_h10"] = dt.Rows[i]["empcode"].ToString().Trim();
                            sg2_dr["sg2_h11"] = dt.Rows[i]["stage"].ToString().Trim();
                            sg2_dr["sg2_h12"] = dt.Rows[i]["name"].ToString().Trim();
                            sg2_dr["sg2_f1"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg2_dr["sg2_f3"] = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + dt.Rows[i]["prevstage"].ToString().Trim() + "'", "name");
                            sg2_dr["sg2_t1"] = dt.Rows[i]["srno"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt.Rows[i]["total"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt.Rows[i]["un_melt"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt.Rows[i]["iqtyout"].ToString().Trim();
                            sg2_dr["sg2_t5"] = dt.Rows[i]["a2"].ToString().Trim();
                            sg2_dr["sg2_t6"] = dt.Rows[i]["hcut"].ToString().Trim();
                            sg2_dr["sg2_t7"] = dt.Rows[i]["a4"].ToString().Trim();
                            sg2_dr["sg2_t8"] = dt.Rows[i]["a5"].ToString().Trim();
                            sg2_dr["sg2_t9"] = dt.Rows[i]["a6"].ToString().Trim();
                            sg2_dr["sg2_t10"] = dt.Rows[i]["a7"].ToString().Trim();
                            sg2_dr["sg2_t11"] = dt.Rows[i]["a8"].ToString().Trim();
                            sg2_dr["sg2_t12"] = dt.Rows[i]["remarks2"].ToString().Trim();
                            sg2_dr["sg2_t13"] = dt.Rows[i]["remarks"].ToString().Trim();
                            sg2_dr["sg2_t14"] = dt.Rows[i]["num1"].ToString().Trim();
                            sg2_dr["sg2_t15"] = dt.Rows[i]["num2"].ToString().Trim();
                            sg2_dr["sg2_t16"] = dt.Rows[i]["num3"].ToString().Trim();
                            sg2_dr["sg2_t17"] = dt.Rows[i]["num4"].ToString().Trim();
                            sg2_dr["sg2_t18"] = dt.Rows[i]["num5"].ToString().Trim();
                            sg2_dr["sg2_t19"] = dt.Rows[i]["num6"].ToString().Trim();
                            sg2_dr["sg2_t20"] = dt.Rows[i]["num7"].ToString().Trim();
                            sg2_dr["sg2_t21"] = dt.Rows[i]["num8"].ToString().Trim();
                            sg2_dr["sg2_t22"] = dt.Rows[i]["num9"].ToString().Trim();
                            sg2_dr["sg2_t23"] = dt.Rows[i]["num10"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose(); sg2_dt.Dispose();
                        ((TextBox)sg2.Rows[z].FindControl("sg2_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        //fetch_col_rejection();
                        fillData();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1 == "") return;
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_prod_reps(frm_qstr);
                    break;
                case "WPrint_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40111");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    txtlbl8.Text = col3;
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
                    txtlbl7.Text = col2;
                    txtlbl7a.Text = col1;
                    btnlbl101.Focus();
                    break;

                case "SG1_ROW_ADD":
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

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();
                            //fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
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
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    try
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    }
                    catch { }
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
                        for (i = 0; i < sg2.Rows.Count - 0; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            // sg2_dr["sg2_srno"] = Convert.ToInt32(dt.Rows[i]["sg2_srno"].ToString());
                            sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                            sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                            sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();
                            sg2_dr["sg2_h6"] = dt.Rows[i]["sg2_h6"].ToString();
                            sg2_dr["sg2_h7"] = dt.Rows[i]["sg2_h7"].ToString();
                            sg2_dr["sg2_h8"] = dt.Rows[i]["sg2_h8"].ToString();
                            sg2_dr["sg2_h9"] = dt.Rows[i]["sg2_h9"].ToString();
                            sg2_dr["sg2_h10"] = dt.Rows[i]["sg2_h10"].ToString();
                            sg2_dr["sg2_h11"] = dt.Rows[i]["sg2_h11"].ToString();
                            sg2_dr["sg2_h12"] = dt.Rows[i]["sg2_h12"].ToString();
                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                            sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"].ToString();
                            sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"].ToString();
                            sg2_dr["sg2_f5"] = dt.Rows[i]["sg2_f5"].ToString();
                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                            sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                            sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();
                            sg2_dr["sg2_t11"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t11")).Text.Trim();
                            sg2_dr["sg2_t12"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t12")).Text.Trim();
                            sg2_dr["sg2_t13"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t13")).Text.Trim();
                            sg2_dr["sg2_t14"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t14")).Text.Trim();
                            sg2_dr["sg2_t15"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t15")).Text.Trim();
                            sg2_dr["sg2_t16"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t16")).Text.Trim();
                            sg2_dr["sg2_t17"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t17")).Text.Trim();
                            sg2_dr["sg2_t18"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t18")).Text.Trim();
                            sg2_dr["sg2_t19"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t19")).Text.Trim();
                            sg2_dr["sg2_t20"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t20")).Text.Trim();
                            sg2_dr["sg2_t21"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t21")).Text.Trim();
                            sg2_dr["sg2_t22"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t22")).Text.Trim();
                            sg2_dr["sg2_t23"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t23")).Text.Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        // fetch_col_rejection();
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

                case "STG":
                    if (col1.Length <= 0) return;
                    txtlbl101.Text = col1;
                    txtlbl101a.Text = col2;
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
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select a.vchnum as wo_no,to_char(a.vchdate,'dd/mm/yyyy') as wo_Dt,a.mchcode,a.ename as machine,a.shftcode as shiftcode,a.prevcode as shift,a.stage as stagecode,b.Name as stage,a.icode as item_code,c.Iname as Item_Name,a.total as make_ready,a.un_melt as production,C.Cpartno,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a,type b,item c where trim(A.stage)=trim(B.type1) and b.id='K' and trim(A.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd desc,wo_no,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of Work Order Entry for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "Print")
        {
            //Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            //fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
            //fgen.fin_prod_reps(frm_qstr);
        }
        else if (hffield.Value == "New")
        {
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            DateRange = PrdRange;
            fillData();
        }
        else
        {
            Checked_ok = "Y";
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
                            for (i = 0; i < sg2.Rows.Count - 0; i++)
                            {
                                //if (sg2.Rows[i].Cells[13].Text.Trim().Length > 2)
                                //{
                                save_it = "Y";
                                //}
                            }
                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
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
                                fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); setColHeadings();
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
        sg1_dt.Columns.Add(new DataColumn("sg1_h11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f23", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f24", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
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
        sg2_dt.Columns.Add(new DataColumn("sg2_h11", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h12", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t10", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t11", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t12", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t13", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t14", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t15", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t16", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t17", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t18", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t19", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t20", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t21", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t22", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t23", typeof(string)));
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
        sg1_dr["sg1_h11"] = "-";
        sg1_dr["sg1_h12"] = "-";
        sg1_dr["sg1_h13"] = "-";
        sg1_dr["sg1_h14"] = "-";
        sg1_dr["sg1_h15"] = "-";
        sg1_dr["sg1_h16"] = "-";
        sg1_dr["sg1_h17"] = "-";
        sg1_dr["sg1_h18"] = "-";
        sg1_dr["sg1_h19"] = "-";
        sg1_dr["sg1_h20"] = "-";
        sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
        sg1_dr["sg1_f1"] = "-";
        sg1_dr["sg1_f2"] = "-";
        sg1_dr["sg1_f3"] = "-";
        sg1_dr["sg1_f4"] = "-";
        sg1_dr["sg1_f5"] = "-";
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
            //sg1.HeaderRow.Cells[19].Style["display"] = "none";
            //e.Row.Cells[19].Style["display"] = "none";
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
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
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
                if (index < sg2.Rows.Count - 0)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;

            //case "SG2_ROW_ADD":
            //    dt = new DataTable();
            //    sg2_dt = new DataTable();
            //    dt = (DataTable)ViewState["sg2"];
            //    z = dt.Rows.Count - 1;
            //    sg2_dt = dt.Clone();
            //    sg2_dr = null;
            //    i = 0;
            //    for (i = 0; i < sg2.Rows.Count; i++)
            //    {
            //        sg2_dr = sg2_dt.NewRow();
            //        sg2_dr["sg2_srno"] = (i + 1);
            //        sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
            //        sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
            //        sg2_dt.Rows.Add(sg2_dr);
            //    }
            //    sg2_add_blankrows();
            //    ViewState["sg2"] = sg2_dt;
            //    sg2.DataSource = sg2_dt;
            //    sg2.DataBind();
            //    break;
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
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {

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
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg2.Rows.Count - 0; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            //save data into the prod_sheet table
            oporow["icode"] = sg2.Rows[i].Cells[2].Text.Trim().ToUpper();
            oporow["acode"] = "-";
            oporow["SRNO"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper();
            oporow["total"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper());
            oporow["un_melt"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim().ToUpper());
            oporow["iqtyout"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
            oporow["a1"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
            oporow["a2"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper());
            oporow["iqtyin"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper());
            oporow["HCUT"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper());
            oporow["a4"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper());
            oporow["mlt_loss"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper());
            oporow["a5"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim().ToUpper());
            oporow["a6"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim().ToUpper());
            oporow["a7"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim().ToUpper());
            oporow["a8"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t11")).Text.Trim().ToUpper());
            oporow["Remarks2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t12")).Text.Trim().ToUpper();
            oporow["Remarks"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t13")).Text.Trim().ToUpper());
            oporow["stage"] = sg2.Rows[i].Cells[10].Text.Trim().ToUpper();
            oporow["subcode"] = txtlbl8.Text.Trim().ToUpper();
            oporow["mchcode"] = txtlbl7.Text.Trim().ToUpper();
            oporow["ename"] = txtlbl7a.Text.Trim().ToUpper();
            oporow["prevcode"] = txtlbl4a.Text.Trim().ToUpper();
            oporow["SHFTCODE"] = txtlbl4.Text.Trim().ToUpper();
            oporow["job_no"] = sg2.Rows[i].Cells[1].Text.Trim().ToUpper();
            oporow["job_dt"] = sg2.Rows[i].Cells[0].Text.Trim().ToUpper();
            oporow["tempr"] = sg2.Rows[i].Cells[4].Text.Trim().ToUpper();
            oporow["naration"] = txtrmk.Text.Trim().ToUpper();
            oporow["empcode"] = sg2.Rows[i].Cells[9].Text.Trim().ToUpper();
            oporow["a3"] = 0;
            oporow["a9"] = 0;
            oporow["a10"] = 0;
            oporow["noups"] = 0;
            oporow["mcstart"] = "-";
            oporow["mcstop"] = "-";
            oporow["flag"] = 0;
            oporow["lmd"] = 0;
            oporow["bcd"] = 0;
            oporow["mtime"] = "-";
            oporow["exc_time"] = "-";
            oporow["irate"] = 0;
            oporow["mseq"] = 0;
            oporow["fm_fact"] = 1;
            oporow["pcpshot"] = 1;
            oporow["PBTCHNO"] = "-";
            oporow["OPR_DTL"] = "-";
            oporow["OEE_R"] = 0;
            oporow["ALSTTIM"] = 0;
            oporow["ALTCTIM"] = 0;
            oporow["CUST_REF"] = "-";
            oporow["CELL_REF"] = "-";
            oporow["CELL_REFN"] = "-";
            oporow["dcode"] = "-";
            oporow["prevstage"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ICODE='" + sg2.Rows[i].Cells[2].Text.Trim().ToUpper() + "' AND SRNO>(SELECT SRNO FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND  TYPE='10' AND ICODE='" + sg2.Rows[i].Cells[2].Text.Trim() + "' AND STAGEC='" + sg2.Rows[i].Cells[10].Text.Trim().ToUpper() + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");
            oporow["var_code"] = "-";//check krna h
            oporow["film_code"] = "N";
            oporow["TSLOT"] = 0;
            oporow["glue_code"] = "-";
            oporow["wo_no"] = "-";
            oporow["wo_dt"] = txtvchdate.Text.Trim().ToUpper();
            oporow["NTEMPR"] = 0;
            oporow["TOT_DT"] = 0;

            // add rejection columns
            oporow["num1"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t14")).Text.Trim().ToUpper());
            oporow["num2"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t15")).Text.Trim().ToUpper());
            oporow["num3"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t16")).Text.Trim().ToUpper());
            oporow["num4"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t17")).Text.Trim().ToUpper());
            oporow["num5"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t18")).Text.Trim().ToUpper());
            oporow["num6"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t19")).Text.Trim().ToUpper());
            oporow["num7"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t20")).Text.Trim().ToUpper());
            oporow["num8"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t21")).Text.Trim().ToUpper());
            oporow["num9"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t22")).Text.Trim().ToUpper());
            oporow["num10"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t23")).Text.Trim().ToUpper());
            oporow["num11"] = 0; // in web only 10 rej reasons are showing and this filed id is for 11 reason
            oporow["num12"] = 0; // in web only 10 rej reasons are showing and this filed id is for 12 reason

            //add downtime columns
            oporow["a11"] = 0;
            oporow["a12"] = 0;
            oporow["a13"] = 0;
            oporow["a14"] = 0;
            oporow["a15"] = 0;
            oporow["a16"] = 0;
            oporow["a17"] = 0;
            oporow["a18"] = 0;
            oporow["a19"] = 0;
            oporow["a20"] = 0;

            //oporow["a21"] = 0;
            //oporow["a22"] = 0;
            //oporow["a23"] = 0;
            //oporow["a24"] = 0;
            //oporow["a25"] = 0;
            //oporow["a26"] = 0;
            //oporow["a27"] = 0;
            //oporow["a28"] = 0;
            //oporow["a29"] = 0;
            //oporow["a30"] = 0;

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
    //------------------------------------------------------------------------------------
    void save_fun2()
    { }
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
        switch (Prg_Id)
        {
            case "F15101":
                SQuery = "SELECT '60' AS FSTR,'Purchase Request' as NAME,'60' AS CODE FROM dual";
                break;
        }
    }
    //------------------------------------------------------------------------------------   
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg2.Columns[12].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[12].CssClass = "hidden";

            for (int i = 30; i < sg2.Columns.Count; i++)
            {
                //e.Row.Cells[i].Style.Add("display", "none");
                //sg2.HeaderRow.Cells[i].Style.Add("display", "none");

                sg2.Columns[i].HeaderStyle.CssClass = "hidden";
                e.Row.Cells[i].CssClass = "hidden";
            }
        }
    }
    //------------------------------------------------------------------------------------
    void fillData()
    {
        create_tab();
        string corr_unit = "Y";
        txtvchdate.Text = vardate;
        fgen.EnableForm(this.Controls);

        string cond = "";
        if (ind_Ptype == "12" || ind_Ptype == "13")
        {
            corr_unit = "N";
            //cond = " where m.col7>0";
        }

        SQuery = "select DISTINCT A.vchnum AS Job_No,A.vchdate as Dated,nvl(a.col18,'-') as col18,nvl(a.col19,'-') as col19,a.ENQDT,A.QTY as Qty,a.icode,a.enqno as Iscancel,decode(a.jStatus,'Y','SComplete','U/Process') as jStatus,decode(a.Status,'Y','Complete','U/Process') as Status,a.acode,a.convdate,a.az_by,to_char(a.az_Dt,'dd/mm/yyyy') as az_dt,IS_NUMBER(A.COL14)*IS_NUMBER(A.COL13) AS COL14,ROUND((NVL(C.ISS,0)-IS_NUMBER(A.COL15))*IS_NUMBER(A.COL13),2) AS ISSU,IS_NUMBER(A.COL14) AS tsht,IS_NUMBER(A.COL15) AS REJALL,a.picode,a.col13,NVL(C.ISS,0) as iss,is_number(a.col7) as col7,a.col16 from costestimate A LEFT OUTER JOIN (sELECT job_no,job_dt,SUM(A5) AS ISS FROM PROD_sHEET WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='85' AND VCHDATE >=TO_dATe('" + fromdt + "','DD/MM/YYYY') GROUP BY job_no,job_dt) C ON A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(C.job_no)||TRIM(C.job_dt) WHERE a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + DateRange + " and length(Trim(nvl(a.app_by,'-')))>1 and a.status!='Y' and nvl(a.jstatus,'-')!='Y' and nvl(a.enqno,'-')!='Y' and a.srno=1 and nvl(a.jhold,'-') not like '%HOLD%'";
        if (corr_unit == "Y")
            SQuery = "Select X.Job_no,X.dated,'-' as Part_No,'-' as Item_name,x.ENQDT,x.col18,x.col19,X.qty as Job_qty,Nvl(y.prodn,0) as Prodn,round((Nvl(y.prodn,0)/(cASE WHEN X.qty=0 THEN 1 ELSE X.QTY END))*100,0)||'%' as Done,x.jstatus,x.status,x.Iscancel,x.az_by,x.az_Dt,X.COL14 AS TOT_SHEET,x.issu,X.icode,x.acode,x.convdate as fstr,x.picode,x.tsht,x.REJALL,x.col13 as Ups,x.iss,x.col7,x.col16 from (" + SQuery + ") x left outer join (select trim(icode) as icode,trim(invno) as invno,invdate,sum(iqtyin) as prodn from ivoucher where branchcd='" + frm_mbr + "' and (type='15' OR type='16') AND INVDATE >=TO_dATe('" + fromdt + "','DD/MM/YYYY') group by trim(icode),trim(invno),invdate) y on trim(x.icode)=trim(y.icode) and trim(x.job_no)=trim(y.invno) and x.dated=y.invdate ";
        else
            SQuery = "Select X.Job_no,X.dated,'-' as Part_No,'-' as Item_name,x.ENQDT,x.col18,x.col19,X.qty as Job_qty,Nvl(y.prodn,0) as Prodn,round((Nvl(y.prodn,0)/decode(X.ISSU,0,DECODE(X.COL14,0,1,X.COL14),X.ISSU))*100,0)||'%' as Done,x.jstatus,x.status,x.Iscancel,x.az_by,x.az_Dt,X.COL14 AS TOT_SHEET,x.issu,X.icode,x.acode,x.convdate as fstr,x.picode,x.tsht,x.REJALL,x.col13 as Ups,x.iss,x.col7,x.col16 from (" + SQuery + ") x left outer join (select trim(icode) as icode,trim(invno) as invno,invdate,sum(iqtyin) as prodn from ivoucher where branchcd='" + frm_mbr + "' and (type='15' OR type='16') AND INVDATE >=TO_dATe('" + fromdt + "','DD/MM/YYYY') group by trim(icode),trim(invno),invdate) y on trim(x.icode)=trim(y.icode) and trim(x.job_no)=trim(y.invno) and x.dated=y.invdate ";

        SQuery = "Select M.Job_no,m.col18,m.col19,to_char(M.dated,'dd/mm/yyyy') as dated,trim(nvl(N.Cpartno,'-')) as Part_No,trim(nvl(N.iname,'-')) as Item_name,m.ENQDT,M.Job_qty,M.Prodn,M.Done,M.status,M.az_by,M.az_Dt,M.TOT_SHEET,M.issu,M.icode,M.acode,M.fstr,m.picode,m.tsht,m.REJALL,m.Iscancel,m.ups,substr(trim(nvl(n.maker,'-')),1,10) as maker,m.iss,m.col7,m.col16 from (" + SQuery + ") M left outer join item N on trim(M.icode)=trim(N.icode) " + cond + " order by M.Dated desc ,M.job_no desc";

        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select a.srno,a.icode,a.stagec,b.name from itwstage a,(Select type1,name from type where id='K') b where a.branchcd!='DD' and trim(a.stagec)=b.type1 order by a.icode,a.srno");
        dt3 = new DataTable();
        dt3 = fgen.getdata(frm_qstr, frm_cocd, "select trim(a.job_no)||a.job_Dt as fstr,decode(trim(a.stage),'-','01',stage) as stage,a.icode,sum(nvl(a.iqtyout,0)) as plan,sum(nvl(a.iqtyin,0)) as prod from prod_sheet a where a.branchcd='" + frm_mbr + "' and a.type in('90') and to_Date(a.job_dt,'dd/mm/yyyy') " + DateRange + " and a.stage<>'08' and trim(nvl(a.job_dt,'-'))!='-' group by a.stage,a.icode,a.job_no,a.job_Dt order by a.job_Dt,a.job_no,a.icode");
        dt4 = new DataTable();
        dt4 = fgen.getdata(frm_qstr, frm_cocd, "select trim(decode(trim(a.stage),'-','01',stage))||trim(a.job_no)||a.job_Dt as fstr,a.icode,sum(nvl(decode(a.type,'85',a.iqtyout,a.a2),0)) as tot,sum(nvl(decode(a.type,'85',a.iqtyout,a.a2),0)) as prod,sum(nvl(a.a4,0)) as rej from prod_sheet a where LENGTH(TRIM(A.JOB_DT))=10 AND a.branchcd='" + frm_mbr + "' and a.type in('85','88','86') and to_Date(a.job_dt,'dd/mm/yyyy')>=TO_dATe('" + fromdt + "','DD/MM/YYYY') and a.stage<>'08' group by a.stage,a.icode,a.job_no,a.job_Dt order by a.job_Dt,a.job_no,a.icode");
        DataTable dt5 = new DataTable();
        dt5 = fgen.getdata(frm_qstr, frm_cocd, "select distinct trim(icode) as fstr,trim(nvl(Grade,'-')) as grade from inspmst where branchcd='" + frm_mbr + "' and type='70' order by trim(icode)");

        #region
        foreach (DataRow drn in dt.Rows)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_h1"] = drn["col18"].ToString().Trim();

            sg1_dr["sg1_h2"] = fgen.make_double(drn["tsht"].ToString().Trim()) + fgen.make_double(drn["REJALL"].ToString().Trim());
            if (ind_Ptype == "12" || ind_Ptype == "13")
                sg1_dr["sg1_h2"] = fgen.seek_iname_dt(dt5, "fstr='" + drn["icode"].ToString().Trim() + "'", "GRADE");

            sg1_dr["sg1_h3"] = drn["maker"].ToString().Trim();
            sg1_dr["sg1_h4"] = drn["ups"].ToString().Trim();
            sg1_dr["sg1_h5"] = drn["az_by"].ToString().Trim();
            sg1_dr["sg1_h6"] = drn["Status"].ToString().Trim();
            sg1_dr["sg1_h7"] = drn["iscancel"].ToString().Trim();
            sg1_dr["sg1_h8"] = drn["icode"].ToString().Trim();
            sg1_dr["sg1_h9"] = drn["job_no"].ToString().Trim();
            sg1_dr["sg1_h10"] = drn["dated"].ToString().Trim();
            sg1_dr["sg1_h11"] = drn["PART_NO"].ToString().Trim();
            sg1_dr["sg1_h12"] = drn["acode"].ToString().Trim();
            sg1_dr["sg1_h13"] = Convert.ToDateTime(drn["enqdt"].ToString().Trim()).ToString("dd/MM") + drn["ITEM_NAME"].ToString().Trim();
            sg1_dr["sg1_h14"] = Convert.ToDateTime(drn["enqdt"].ToString().Trim()).ToString("dd/MM");
            sg1_dr["sg1_h15"] = drn["JOB_qTY"].ToString().Trim();
            sg1_dr["sg1_h16"] = drn["prodn"].ToString().Trim();
            sg1_dr["sg1_h17"] = drn["done"].ToString().Trim();
            sg1_dr["sg1_h18"] = drn["acode"].ToString().Trim();
            sg1_dr["sg1_h19"] = drn["icode"].ToString().Trim();

            if (dt2.Rows.Count > 0)
            {
                DataView dv = new DataView(dt2, "icode='" + drn["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    i = 1;
                    for (int l = 0; l < dv.Count; l++)
                    {
                        if (i > 34) break;
                        if (dv[l]["name"].ToString().Length > 15) sg1_dr["sg1_f" + (i)] = dv[l]["name"].ToString().Substring(0, 15).PadRight(15, ' ').ToString() + " " + dv[l]["stagec"].ToString().ToString();
                        else sg1_dr["sg1_f" + (i)] = dv[l]["name"].ToString().PadRight(15, ' ').ToString() + " " + dv[l]["stagec"].ToString().ToString();

                        i = i + 2;
                    }
                }
            }
            sg1_dr["sg1_f22"] = fgen.seek_iname_dt(dt5, "fstr='" + drn["icode"].ToString().Trim() + "'", "GRADE");

            sg1_dr["sg1_f23"] = drn["col16"].ToString().Trim();
            sg1_dr["sg1_f24"] = drn["col7"].ToString().Trim();
            sg1_dt.Rows.Add(sg1_dr);
        }
        #endregion

        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        if (sg1_dt.Rows.Count <= 0) return;

        int x = 0;
        string stg = "", hs = "";
        string mcode = ""; DataView dv1 = null;

        for (x = 20; x < sg1.Columns.Count - 4; x++)
        //   for (x = 20; x < sg1.Columns.Count - 1; x++) // ORIGINAL
        {
            hs = "N";
            for (i = 0; i < sg1.Rows.Count; i++)
            {
                if (sg1.Rows[i].Cells[x].Text.Trim().Length > 3 && sg1.Rows[i].Cells[x].Text.Trim().ToUpper() != "&NBSP;")
                {
                    stg = sg1.Rows[i].Cells[x].Text.Trim().Substring(sg1.Rows[i].Cells[x].Text.Trim().Length - 2, 2);
                    mcode = sg1.Rows[i].Cells[9].Text.Trim() + Convert.ToDateTime(sg1.Rows[i].Cells[10].Text.Trim()).ToString("dd/MM/yyyy");
                    dv1 = new DataView();
                    if (dt3.Rows.Count > 0)
                        dv1 = new DataView(dt3, "fstr='" + mcode + "' and stage='" + stg + "'", "", DataViewRowState.CurrentRows);
                    if (dv1.Count > 0)
                    {
                        if (dv1.Count > 0)
                        {
                            for (int v = 0; v < dv1.Count; v++)
                            {
                                sg1.Rows[i].Cells[x + 1].Text = dv1[v]["plan"].ToString().PadRight(10, ' ');
                                if (hs != "Y")
                                    sg1.HeaderRow.Cells[x + 1].Text = ("Prodn").PadRight(8, ' ');
                            }
                        }
                        mcode = stg + mcode;
                        if (dt4.Rows.Count > 0)
                        {
                            DataView dv2 = new DataView(dt4, "fstr='" + mcode + "' ", "", DataViewRowState.CurrentRows);
                            if (dv2.Count > 0)
                            {
                                for (int v = 0; v < dv2.Count; v++)
                                {
                                    sg1.Rows[i].Cells[x + 1].Text = sg1.Rows[i].Cells[x + 1].Text.PadRight(8, ' ') + " | " + dv2[v]["tot"].ToString().PadRight(10, ' ');
                                    sg1.HeaderRow.Cells[x + 1].Text = ("Prodn").PadRight(8, ' ') + " | " + ("Plan").PadRight(8, ' ');
                                    hs = "Y";
                                }
                            }
                        }
                    }
                    else if (1 == 2)
                    {
                        //***********
                        stg = sg1.Rows[i].Cells[x].Text.Trim().Substring(sg1.Rows[i].Cells[x].Text.Trim().Length - 2, 2);
                        mcode = stg + sg1.Rows[i].Cells[9].Text.Trim() + Convert.ToDateTime(sg1.Rows[i].Cells[10].Text.Trim()).ToString("dd/MM/yyyy");
                        dv1 = new DataView(dt4, "fstr='" + mcode + "' ", "", DataViewRowState.CurrentRows);
                        if (dv1.Count > 0)
                        {
                            for (int v = 0; v < dv1.Count; v++)
                            {
                                sg1.Rows[i].Cells[x + 1].Text = dv1[v]["tot"].ToString().PadRight(10, ' ');
                                //sg1.HeaderRow.Cells[x + 1].Text = ("Prodn").PadRight(8, ' ');
                            }
                        }
                    }
                }
            }
        }
        if (sg1.Rows.Count > 0)
        {
            for (int K = 0; K < sg1.Columns.Count; K++)
            {
                //if (sg1.Rows[0].Cells[K].Text.Trim().Length < 3)
                //{
                //    sg1.HeaderRow.Cells[K].Width = (sg1.Rows[0].Cells[K].Text.Trim().Length + 1) * 50;
                //    sg1.Columns[K].ItemStyle.Width = (sg1.Rows[0].Cells[K].Text.Trim().Length + 1) * 50;
                //}
                //else if (sg1.Rows[0].Cells[K].Text.Trim().Length < 5)
                //{
                //    sg1.HeaderRow.Cells[K].Width = sg1.Rows[0].Cells[K].Text.Trim().Length * 20;
                //    sg1.Columns[K].ItemStyle.Width = sg1.Rows[0].Cells[K].Text.Trim().Length * 20;
                //}
                //else
                //{
                //    sg1.HeaderRow.Cells[K].Width = sg1.Rows[0].Cells[K].Text.Trim().Length * 15;
                //    sg1.Columns[K].ItemStyle.Width = sg1.Rows[0].Cells[K].Text.Trim().Length * 15;
                //}
                //if (K == 0)
                //{
                //    sg1.HeaderRow.Cells[K].Width = 30;
                //    sg1.Columns[K].ItemStyle.Width = 30;
                //}
                //sg1.Rows.Cells[K].Width = 200;
                sg1.HeaderRow.Cells[13].Width = 200;
                sg1.Columns[13].ItemStyle.Width = 200;


                //sg1.HeaderRow.Cells[23].Width = 20;
                //sg1.Columns[23].ItemStyle.Width = 20;

                sg1.HeaderRow.Cells[24].Width = 150;
                sg1.Columns[24].ItemStyle.Width = 150;

                //sg1.HeaderRow.Cells[25].Width = 20;
                //sg1.Columns[25].ItemStyle.Width = 20;

                sg1.HeaderRow.Cells[26].Width = 150;
                sg1.Columns[26].ItemStyle.Width = 150;

                //sg1.HeaderRow.Cells[27].Width = 20;
                //sg1.Columns[27].ItemStyle.Width = 20;

                sg1.HeaderRow.Cells[28].Width = 150;
                sg1.Columns[28].ItemStyle.Width = 150;
            }
            for (x = 1; x < sg1.HeaderRow.Cells.Count; x++)
            {
                if (sg1.HeaderRow.Cells[x].Text == "sg1_srno") sg1.HeaderRow.Cells[x].Text = "S.no";
                if (sg1.HeaderRow.Cells[x].Text == "sg1_f1") sg1.HeaderRow.Cells[x].Text = "Stage";
                if (sg1.HeaderRow.Cells[x].Text == "sg1_f2") sg1.HeaderRow.Cells[x].Text = "Qty";

                if (sg1.HeaderRow.Cells[x].Text == "sg1_f3") sg1.HeaderRow.Cells[x].Text = "Stage";
                if (sg1.HeaderRow.Cells[x].Text == "sg1_f4") sg1.HeaderRow.Cells[x].Text = "Qty";

                if (sg1.HeaderRow.Cells[x].Text == "sg1_f5") sg1.HeaderRow.Cells[x].Text = "Stage";
                if (sg1.HeaderRow.Cells[x].Text == "sg1_f6") sg1.HeaderRow.Cells[x].Text = "Qty";

                if (sg1.HeaderRow.Cells[x].Text == "sg1_f7") sg1.HeaderRow.Cells[x].Text = "Stage";
                if (sg1.HeaderRow.Cells[x].Text == "sg1_f8") sg1.HeaderRow.Cells[x].Text = "Qty";
            }
            //for (x = 30; x < sg1.HeaderRow.Cells.Count; x++) // ORIGINAL
            //{
            //    //sg1.Rows[x].Style["display"] = "none";
            //    sg1.Columns[x].Visible = false;
            //    //sg1.HeaderRow.Cells[x].Style["display"] = "none";
            //}
            for (x = 30; x <= 41; x++)
            {
                sg1.Columns[x].Visible = false;
            }
        }
        disablectrl();
        setColHeadings();
        ViewState["sg1_dt"] = sg1_dt;
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl8_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnGet_ServerClick(object sender, EventArgs e)
    {
        if (txtlbl101.Text.Trim().Length <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Stage First");
            btnlbl101.Focus();
            return;
        }
        col1 = ""; string[] check; col2 = ""; col3 = ""; string DUPLICATE = "";
        foreach (GridViewRow gr1 in sg1.Rows)
        {
            CheckBox chk1 = (CheckBox)gr1.FindControl("chk1");
            if (chk1.Checked == true)
            {
                //col1 += gr1.Cells[8].Text.Trim() + "." + gr1.Cells[9].Text.Trim() + ",";
                if (col1.Length > 0)
                {
                    col1 = col1 + ",'" + gr1.Cells[8].Text.Trim() + gr1.Cells[9].Text.Trim() + gr1.Cells[10].Text.Trim() + "'";
                }
                else
                {
                    col1 = "'" + gr1.Cells[8].Text.Trim() + gr1.Cells[9].Text.Trim() + gr1.Cells[10].Text.Trim() + "'";
                }
                if (col3.Length > 0)
                {
                    col3 = col3 + "," + gr1.Cells[8].Text.Trim() + gr1.Cells[9].Text.Trim() + gr1.Cells[10].Text.Trim() + "";
                }
                else
                {
                    col3 = "" + gr1.Cells[8].Text.Trim() + gr1.Cells[9].Text.Trim() + gr1.Cells[10].Text.Trim() + "";
                }
            }
        }
        if (col1.Length > 1)
        {
            check = col3.Split(',');
            if (ViewState["sg2"] != null)
            {
                // FOR MAINTAINING THE ALREADY STORED DATA IN THE GRID SG2
                dt = new DataTable();
                sg2_dt = new DataTable();
                dt = (DataTable)ViewState["sg2"];
                z = dt.Rows.Count - 1;
                sg2_dt = dt.Clone();
                sg2_dr = null;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    sg2_dr = sg2_dt.NewRow();
                    // sg2_dr["sg2_srno"] = Convert.ToInt32(dt.Rows[i]["sg2_srno"].ToString());
                    sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                    sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                    sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                    sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                    sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();
                    sg2_dr["sg2_h6"] = dt.Rows[i]["sg2_h6"].ToString();
                    sg2_dr["sg2_h7"] = dt.Rows[i]["sg2_h7"].ToString();
                    sg2_dr["sg2_h8"] = dt.Rows[i]["sg2_h8"].ToString();
                    sg2_dr["sg2_h9"] = dt.Rows[i]["sg2_h9"].ToString();
                    sg2_dr["sg2_h10"] = dt.Rows[i]["sg2_h10"].ToString();
                    sg2_dr["sg2_h11"] = dt.Rows[i]["sg2_h11"].ToString();
                    sg2_dr["sg2_h12"] = dt.Rows[i]["sg2_h12"].ToString();
                    sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                    sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                    sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"].ToString();
                    sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"].ToString();
                    sg2_dr["sg2_f5"] = dt.Rows[i]["sg2_f5"].ToString();
                    sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                    sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                    sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                    sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                    sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                    sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                    sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                    sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                    sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                    sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();
                    sg2_dr["sg2_t11"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t11")).Text.Trim();
                    sg2_dr["sg2_t12"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t12")).Text.Trim();
                    sg2_dr["sg2_t13"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t13")).Text.Trim();
                    sg2_dr["sg2_t14"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t14")).Text.Trim();
                    sg2_dr["sg2_t15"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t15")).Text.Trim();
                    sg2_dr["sg2_t16"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t16")).Text.Trim();
                    sg2_dr["sg2_t17"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t17")).Text.Trim();
                    sg2_dr["sg2_t18"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t18")).Text.Trim();
                    sg2_dr["sg2_t19"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t19")).Text.Trim();
                    sg2_dr["sg2_t20"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t20")).Text.Trim();
                    sg2_dr["sg2_t21"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t21")).Text.Trim();
                    sg2_dr["sg2_t22"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t22")).Text.Trim();
                    sg2_dr["sg2_t23"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t23")).Text.Trim();
                    sg2_dt.Rows.Add(sg2_dr);
                }
            }
            if (sg2_dt != null)
            {
                if (sg2_dt.Rows.Count > 0)
                {
                    // FOR CHECKING DUPLICATE ENTERIES IN THE GRID
                    for (int a = 0; a < check.Length; a++)
                    {
                        DataView checkDuplicate = new DataView(sg2_dt, "sg2_h3='" + check[a].ToString().Trim().Substring(0, 8) + "' and sg2_h2='" + check[a].ToString().Trim().Substring(8, 6) + "' and sg2_h1='" + check[a].ToString().Trim().Substring(14, 10) + "'", "", DataViewRowState.CurrentRows);
                        dt3 = new DataTable();
                        dt3 = checkDuplicate.ToTable(); // IF DT HAS ROWS IT MEANS DUPLICACY EXISTS
                        if (dt3.Rows.Count == 0)
                        {
                            if (col2.Length > 0)
                            {
                                col2 = col2 + ",'" + check[a].ToString().Trim() + "'";
                            }
                            else
                            {
                                col2 = "'" + check[a].ToString().Trim() + "'";
                            }
                        }
                        else
                        {
                            DUPLICATE += ", Job No. " + check[a].ToString().Trim().Substring(8, 6) + " Job.Dt. " + check[a].ToString().Trim().Substring(14, 10) + " Icode " + check[a].ToString().Trim().Substring(0, 8);
                        }
                    }
                }
                else
                {
                    // WHEN GRID HAS ONLY ONE ROW BUT USER HAS DELETE IT
                    create_tab2();
                    col2 = col1;
                }
            }
            else
            {
                // WHEN USER IS ENTERING DATA ON THE CLICK OF BUTTON NEW
                create_tab2();
                col2 = col1;
            }
            SQuery = "select distinct a.acode,f.aname,b.Iname as iname,a.Icode as iCode,b.Cpartno,a.vchnum,a.qty,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.Icode)||'.'||trim(a.vchnum) as fstr,a.col17,a.col16,a.col13,c.stagec,A.COL15,a.col18 ,a.col19,a.col14,d.grade,t.rate,t.name,t.addr1,t.balop from costestimate a, item b,itwstage c,inspmst d,type t,famst f where a.branchcd='" + frm_mbr + "' and a.type='30' and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(c.icode) and trim(a.icode)=trim(d.icode) and d.type='70' and c.type='10' and trim(c.stagec)=trim(t.type1) and t.id='K' and a.status='N' and c.stagec in (" + txtlbl101.Text.Trim() + ") and trim(a.Icode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + col2 + ") order by trim(a.Icode)||'.'||trim(a.vchnum) ";
            if (ind_Ptype == "12" || ind_Ptype == "13")
            {
                SQuery = "select distinct a.acode,f.aname,b.Iname as iname,a.Icode as iCode,b.Cpartno,a.vchnum,a.qty,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.Icode)||'.'||trim(a.vchnum) as fstr,a.col17,a.col16,a.col13,c.stagec,A.COL15,a.col18 ,a.col19,a.col14,d.grade,t.rate,t.name,0 as col7,t.addr1,t.balop from costestimate a, item b,itwstage c,inspmst d,type t,famst f where a.branchcd='" + frm_mbr + "' and a.type='30' and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(c.icode) and trim(a.icode)=trim(d.icode) and d.type='70' and c.type='10' and trim(c.stagec)=trim(t.type1) and t.id='K' and a.status='N' and c.stagec in (" + txtlbl101.Text.Trim() + ") and trim(a.Icode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + col2 + ") order by trim(a.Icode)||'.'||trim(a.vchnum) ";

                //SQuery = "select distinct a.acode,f.aname,b.Iname as iname,a.Icode as iCode,b.Cpartno,a.vchnum,a.qty,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.Icode)||'.'||trim(a.vchnum) as fstr,a.col17,a.col16,a.col13,c.stagec,A.COL15,a.col18 ,a.col19,a.col14 from costestimate a, item b,itwstage c where a.branchcd='" + frm_mbr + "' and a.type='30' and trim(a.icode)=trim(b.icode) and trim(a.icode)=trim(c.icode) and a.status='N' and c.stagec in (" + txtlbl101.Text.Trim() + ") and trim(a.Icode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + col2 + ") order by trim(a.Icode)||'.'||trim(a.vchnum) ";
            }
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            string stagecode = "";
            sg2_dr = null;
            foreach (DataRow drn in dt2.Rows)
            {
                sg2_dr = sg2_dt.NewRow();
                sg2_dr["sg2_h1"] = drn["vchdate"].ToString().Trim();
                sg2_dr["sg2_h2"] = drn["vchnum"].ToString().Trim();
                sg2_dr["sg2_h3"] = drn["icode"].ToString().Trim();
                sg2_dr["sg2_h4"] = drn["acode"].ToString().Trim();
                sg2_dr["sg2_h5"] = drn["aname"].ToString().Trim();
                sg2_dr["sg2_h6"] = drn["col18"].ToString().Trim();
                sg2_dr["sg2_h7"] = drn["col19"].ToString().Trim();
                sg2_dr["sg2_h8"] = fgen.make_double(drn["col14"].ToString().Trim()) + fgen.make_double(drn["col15"].ToString().Trim()); ;
                sg2_dr["sg2_h9"] = drn["col13"].ToString().Trim();
                sg2_dr["sg2_h10"] = drn["col16"].ToString().Trim();
                sg2_dr["sg2_h11"] = drn["stagec"].ToString().Trim();
                sg2_dr["sg2_h12"] = drn["name"].ToString().Trim();
                sg2_dr["sg2_f1"] = drn["iname"].ToString().Trim();
                sg2_dr["sg2_f2"] = drn["cpartno"].ToString().Trim();
                stagecode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ICODE='" + drn["icode"].ToString().Trim() + "' AND SRNO>(SELECT SRNO FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND  TYPE='10' AND ICODE='" + drn["icode"].ToString().Trim() + "' AND STAGEC='" + drn["stagec"].ToString().Trim() + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");
                sg2_dr["sg2_f3"] = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + stagecode + "'", "name");
                sg2_dr["sg2_t2"] = drn["rate"].ToString().Trim();

                sg2_dr["sg2_t4"] = drn["col14"].ToString().Trim().toDouble() + drn["col15"].ToString().Trim().toDouble();

                if (ind_Ptype == "12" || ind_Ptype == "13")
                {
                    //if (drn["balop"].ToString().Trim() == "0")
                    //    sg2_dr["sg2_t4"] = drn["col7"].ToString().Trim().toDouble();
                    //else 
                        sg2_dr["sg2_t4"] = drn["qty"].ToString().Trim().toDouble();
                }

                sg2_dt.Rows.Add(sg2_dr);
            }
            ViewState["sg2"] = sg2_dt;
            sg2.DataSource = sg2_dt;
            sg2.DataBind();
            //  fetch_col_rejection();
            foreach (GridViewRow gr1 in sg1.Rows)
            {
                CheckBox chk1 = (CheckBox)gr1.FindControl("chk1");
                if (chk1.Checked == true)
                {
                    chk1.Checked = false;
                }
            }
            if (DUPLICATE.Length > 1)
            {
                fgen.msg("-", "AMSG", "Duplicate Selection Of " + DUPLICATE.TrimStart(','));
            }
        }
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STG";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lbl101.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    public void fetch_col_rejection()
    {
        dt2 = new DataTable();
        SQuery = "select initcap(substr(Name,1,10)) as Name from (Select ID,Name,type1 from type where id='4' order by type1) where rownum<=10";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (sg2_dt == null) return;
        sg2_dr = sg2_dt.NewRow();
        i = 0;
        if (dt2.Rows.Count > 0)
        {
            int d = 30;
            do
            {
                sg2.HeaderRow.Cells[d].Text = dt2.Rows[i]["Name"].ToString().Trim();
                d = d + 1;
                i = i + 1;
            } while (i < dt2.Rows.Count);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnwprint_ServerClick(object sender, EventArgs e)
    {
        if (col1 == "") return;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "86");
        hffield.Value = "WPrint_E";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
    }
}