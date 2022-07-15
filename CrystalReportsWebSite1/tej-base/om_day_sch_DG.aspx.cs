using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_Day_Sch_Dg : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    double batchQty = 0;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id, mq0, mq1, mq2, mq3, xprdrange1, xprdrange;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, typePopup;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_tabname2, frm_tabname3;
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
            typePopup = "N";
            btnnew.Visible = false;
            btnlist.Visible = false;
            btndel.Visible = false;
            btnprint.Visible = false;
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
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

                #region hide hidden columns
                for (int i = 0; i < 10; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        tab2.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
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
        create_tab4();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();
        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnprint.Disabled = true; btnlist.Disabled = true;
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
        frm_tabname = "budgmst";
        frm_tabname2 = "budgmst2";
        frm_tabname3 = "budgmst3";
        frm_vty = "46";
        lbl1a.Text = "46";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        lblheader.Text = "Date Wise Sales Schedule";
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
                //pop1
                SQuery = "select type1,name as State ,type1 as code from type where id='D' and type1 like '1%' order by Name";
                break;
            case "TICODE":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='1' AND SUBSTR(TYPE1,1,1) IN ('6') and substr(type1,1,2) in ('61','62') order by TYPE1 ";
                break;
            case "TICODEX":
                SQuery = "select type1,name as State ,type1 as code from type where id='{' order by Name";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                //pop3
                // to avoid repeat of item
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[16].Text.Trim() + "'";
                    else col1 = "'" + gr.Cells[16].Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "select a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode) as fstr,a.ciname as item_name,trim(a.icode) as item_code,a.qtyord as qty,a.cpartno,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,to_char(a.orddt,'yyyymmdd') as vdd from somas a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + txtFstr.Text.Trim() + "' and trim(NVL(A.iCAT,'-'))!='Y' and a.icode not in (" + col1 + ") ORDER BY srno";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;

            case "MONTH":
                SQuery = "select trim(mthnum) as fstr,mthnum,mthname from mths";
                break;

            case "sg1_t5":
                SQuery = "select 'Y' as fstr,'If Yes , This Shall Show On Job Order Screen' as msg from dual union all select 'N','If No,This Shall Not Show On Job Order Screen' as msg from dual";
                break;

            case "SALES":
                SQuery = "select 'Y' AS FSTR,'SALE ORDER WISE' AS SELECTION,'YES' AS CHOICE_ FROM DUAL UNION  ALL  select 'N' AS FSTR,'ITEM WISE' AS SELECTION,'NO' AS CHOICE_ FROM DUAL";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.ordno||' '||a.type||'('||(case when nvl(trim(a.app_by),'-')='-' then 'Un Approved' else 'Approved' end)||')' as so_no,to_char(a.orddt,'dd/mm/yyyy') as order_date,a.acode as cust_code,f.aname as customer,trim(a.cpartno) as partno,a.ent_by,to_char(a.orddt,'dd/mm/yyyy') as ent_Dt,a.pordno,a.ordno,a.ciname,trim(a.icode) as erp_code,to_char(a.orddt,'yyyymmdd') as vdd from somas a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%'  order by vdd desc,a.ordno desc";
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
            hffield.Value = "New";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            set_Val();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
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
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        #endregion
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit_E";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Sales Order", frm_qstr);
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
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        double prodqty = 0, delvqty = 0;
        for (int i = 0; i < sg2.Rows.Count; i++)
        {
            mq2 = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
            mq3 = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
            delvqty = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim());
            prodqty = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim());
            if (mq2.Length > 1)
            {
                dhd = fgen.ChkDate(mq2);
                if (dhd == 0)
                {
                    fgen.msg("-", "AMSG", "Please Fill Valid Cust. Delv. Date At Line No. " + sg2.Rows[i].Cells[6].Text.Trim()); return;
                }
            }
            dhd = fgen.ChkDate(mq3);
            if (dhd == 0)
            {
                fgen.msg("-", "AMSG", "Please Fill Valid Delv. Date At Line No. " + sg2.Rows[i].Cells[6].Text.Trim()); return;
            }
            if (prodqty > 0 && delvqty > 0)
            {
                fgen.msg("-", "AMSG", "Please Fill Either Prodn Qty or Delv. Qty in one Row .Error At Line No. " + sg2.Rows[i].Cells[6].Text.Trim());
                return;
            }
        }

        if (edmode.Value == "")
        {
            if (sg1.Rows.Count <= 1)
            {
                fgen.msg("-", "SMSG", "Have You Transferred Data from Date Wise to Top Grid?"); return;
            }
            else
            {
                fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            }
        }
        else
        {
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        }
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
        sg4_dt = new DataTable();
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
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
        sg4_add_blankrows();
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        ViewState["sg4"] = null;
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
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
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl2.Text = frm_uname;
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
                    sg2_dt = new DataTable();
                    create_tab2();
                    sg2_add_blankrows();
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    setColHeadings();
                    ViewState["sg2"] = sg2_dt;
                    sg3_dt = new DataTable();
                    create_tab3();
                    sg3_add_blankrows();
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    setColHeadings();
                    ViewState["sg3"] = sg3_dt;
                    //-------------------------------------------
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    SQuery = "Select nvl(a.obj_name,'-') as udf_name from udf_config a where trim(a.frm_name)='" + Prg_Id + "' ORDER BY a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    create_tab4();
                    sg4_dr = null;
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = sg4_dt.Rows.Count + 1;

                            sg4_dr["sg4_t1"] = dt.Rows[i]["udf_name"].ToString().Trim();
                            sg4_dt.Rows.Add(sg4_dr);
                        }
                    }
                    sg4_add_blankrows();
                    ViewState["sg4"] = sg4_dt;
                    sg4.DataSource = sg4_dt;
                    sg4.DataBind();
                    dt.Dispose();
                    sg4_dt.Dispose();
                    //-------------------------------------------                    
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
                    SQuery = "select distinct vchnum from " + frm_tabname + " where solink='" + col1 + "' and branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and fromso!='Y'";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    // MAIN STOCK
                    fromdt = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                    todt = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                    xprdrange1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    xprdrange = " BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')-1";
                    dt3 = new DataTable();
                    mq0 = "select TRIM(A.ICODE) AS ICODE,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr from itembal where BRANCHCD='" + frm_mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='Y' GROUP BY trim(icode) ,branchcd) a where substr(trim(a.icode),1,1)>=7 GROUP BY TRIM(A.ICODE) ORDER BY ICODE";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    if (dt2.Rows.Count > 0)
                    {
                        #region
                        SQuery = "Select a.*,f.aname,i.iname,i.cpartno from " + frm_tabname + " a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.solink='" + col1 + "' ORDER BY A.SRNO";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        mq1 = "select distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,qty,icode,to_char(vchdate,'yyyymmdd') as vdd from costestimate where branchcd='" + frm_mbr + "' and type='30' and substr(trim(convdate),1,20)='" + col1 + "' order by vdd desc";
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                        if (dt.Rows.Count > 0)
                        {
                            frm_vty = dt.Rows[0]["type"].ToString().Trim();
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", dt.Rows[0]["vchnum"].ToString().Trim() + Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy"));
                            ViewState["fstr"] = dt.Rows[0]["vchnum"].ToString().Trim() + Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            txtlbl2.Text = dt.Rows[0]["SOCAT"].ToString().Trim();
                            txtlbl4.Text = dt.Rows[0]["ACODE"].ToString().Trim();
                            txtlbl4a.Text = dt.Rows[0]["ANAME"].ToString().Trim();
                            txtFstr.Text = dt.Rows[0]["SOLINK"].ToString().Trim();
                            //txtlbl3.Text = dt.Rows[0]["ENT_BY"].ToString().Trim();
                            //txtlbl8.Text = dt.Rows[0]["PENT_DT"].ToString().Trim();
                            create_tab2();
                            sg2_dr = null;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                                sg2_dr["sg2_h1"] = "-";
                                sg2_dr["sg2_h2"] = "-";
                                sg2_dr["sg2_h3"] = "-";
                                sg2_dr["sg2_h4"] = "-";
                                sg2_dr["sg2_h5"] = dt.Rows[i]["CFW_DATA"].ToString().Trim();
                                if (fgen.make_double(dt.Rows[i]["ACTUALCOST"].ToString().Trim()) > 0)
                                {
                                    mq2 = fgen.seek_iname_dt(dt4, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "fstr");
                                    if (mq2.Length > 1)
                                    {
                                        sg2_dr["sg2_h6"] = mq2;
                                    }
                                    else
                                    {
                                        sg2_dr["sg2_h6"] = dt.Rows[i]["JOBCARDNO"].ToString().Trim();
                                    }
                                    mq3 = fgen.seek_iname_dt(dt4, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "qty");
                                    if (mq3.Length > 1)
                                    {
                                        sg2_dr["sg2_f1"] = fgen.make_double(mq3);
                                    }
                                    else
                                    {
                                        sg2_dr["sg2_f1"] = dt.Rows[i]["JOBCARDQTY"].ToString().Trim();
                                    }
                                }

                                sg2_dr["sg2_h7"] = dt.Rows[i]["APP_DT"].ToString().Trim();
                                sg2_dr["sg2_h8"] = "-";
                                sg2_dr["sg2_h9"] = "-";
                                sg2_dr["sg2_h10"] = fgen.seek_iname_dt(dt3, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "Closing_Stk");
                                sg2_dr["sg2_h11"] = dt.Rows[i]["CLOSEIT"].ToString().Trim();
                                sg2_dr["sg2_f2"] = dt.Rows[i]["CPARTNO"].ToString().Trim();
                                sg2_dr["sg2_f3"] = dt.Rows[i]["ICODE"].ToString().Trim();
                                sg2_dr["sg2_f4"] = dt.Rows[i]["INAME"].ToString().Trim();
                                sg2_dr["sg2_f5"] = "-";
                                sg2_dr["sg2_t1"] = dt.Rows[i]["CUSTDLV"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dt.Rows[i]["DESC_"].ToString().Trim();
                                sg2_dr["sg2_t3"] = dt.Rows[i]["BUDGETCOST"].ToString().Trim();
                                sg2_dr["sg2_t4"] = dt.Rows[i]["ACTUALCOST"].ToString().Trim();
                                sg2_dr["sg2_t5"] = dt.Rows[i]["SOREMARKS"].ToString().Trim();
                                sg2_dr["sg2_t6"] = dt.Rows[i]["TOLERANCE"].ToString().Trim();
                                sg2_dr["sg2_t7"] = dt.Rows[i]["JOBCARDRQD"].ToString().Trim();
                                sg2_dr["sg2_t8"] = dt.Rows[i]["REQ_CLOSEDBY"].ToString().Trim();
                                sg2_dr["sg2_t9"] = dt.Rows[i]["REQ_CL_RSN"].ToString().Trim();
                                sg2_dr["sg2_t10"] = dt.Rows[i]["REV_RMK"].ToString().Trim();
                                sg2_dt.Rows.Add(sg2_dr);
                            }
                            ViewState["sg2"] = sg2_dt;
                            sg2.DataSource = sg2_dt;
                            sg2.DataBind();
                            dt.Dispose();
                            sg2_dt.Dispose();
                            dt.Dispose();
                            //ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                            //ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                            fgen.EnableForm(this.Controls);
                            disablectrl();
                            edmode.Value = "Y";
                            foreach (GridViewRow gr in sg2.Rows)
                            {
                                string hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                                if (hf == "Y")
                                {
                                    ((CheckBox)gr.FindControl("sg2_h11")).Checked = true;
                                }
                                else
                                {
                                    ((CheckBox)gr.FindControl("sg2_h11")).Checked = false;
                                }
                            }
                            create_tab();
                            sg1_add_blankrows();
                            ViewState["sg1"] = sg1_dt;
                            sg1.DataSource = sg1_dt;
                            sg1.DataBind();
                            setColHeadings();
                        }
                        #endregion
                    }
                    else
                    {
                        SQuery = "select a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.icode) as icode,a.ciname,a.qtyord,a.desp_to,to_char(a.cu_chldt,'dd/mm/yyyy') as cu_chldt,i.cpartno,a.acode,f.aname,a.work_ordno,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,desc_ from somas a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + col1 + "' ORDER BY srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        if (dt.Rows.Count > 0)
                        {
                            txtFstr.Text = col1;
                            txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim();
                            txtvchdate.Text = dt.Rows[0]["orddt"].ToString().Trim();
                            txtlbl3.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                            txtlbl8.Text = dt.Rows[i]["ent_dt"].ToString().Trim();
                            txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                            txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();
                            txtlbl2.Text = dt.Rows[i]["work_ordno"].ToString().Trim();
                            create_tab();
                            sg1_add_blankrows();
                            ViewState["sg1"] = sg1_dt;
                            sg1.DataSource = sg1_dt;
                            sg1.DataBind();
                            create_tab2();
                            sg2_dr = null;
                            i = 1;
                            foreach (DataRow dr in dt.Rows)
                            {
                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = i;
                                sg2_dr["sg2_h1"] = "-";
                                sg2_dr["sg2_h2"] = "-";
                                sg2_dr["sg2_h3"] = "-";
                                sg2_dr["sg2_h4"] = "-";
                                sg2_dr["sg2_h5"] = "-";
                                sg2_dr["sg2_h6"] = "-";
                                sg2_dr["sg2_h7"] = "-";
                                sg2_dr["sg2_h8"] = "-";
                                sg2_dr["sg2_h9"] = dr["desp_to"].ToString().Trim();
                                sg2_dr["sg2_h10"] = fgen.seek_iname_dt(dt3, "icode='" + dr["icode"].ToString().Trim() + "'", "Closing_Stk");
                                sg2_dr["sg2_h11"] = "N";
                                sg2_dr["sg2_f1"] = "-";
                                sg2_dr["sg2_f2"] = dr["cpartno"].ToString().Trim();
                                sg2_dr["sg2_f3"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_f4"] = dr["ciname"].ToString().Trim();
                                sg2_dr["sg2_f5"] = "-";
                                sg2_dr["sg2_t1"] = "";
                                sg2_dr["sg2_t2"] = dr["cu_chldt"].ToString().Trim();
                                sg2_dr["sg2_t3"] = "";
                                sg2_dr["sg2_t4"] = dr["qtyord"].ToString().Trim();
                                sg2_dr["sg2_t5"] = dr["desc_"].ToString().Trim();
                                sg2_dr["sg2_t6"] = "";
                                sg2_dr["sg2_t7"] = "Y";
                                sg2_dr["sg2_t8"] = "";
                                sg2_dr["sg2_t9"] = "";
                                sg2_dr["sg2_t10"] = "";
                                sg2_dt.Rows.Add(sg2_dr);
                                i++;
                            }
                            ViewState["sg2"] = sg2_dt;
                            sg2.DataSource = sg2_dt;
                            sg2.DataBind();
                            dt.Dispose();
                            sg2_dt.Dispose();
                            fgen.EnableForm(this.Controls);
                            disablectrl();
                            setColHeadings();
                            foreach (GridViewRow gr in sg2.Rows)
                            {
                                string hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                                if (hf == "Y")
                                {
                                    ((CheckBox)gr.FindControl("sg2_h11")).Checked = true;
                                }
                                else
                                {
                                    ((CheckBox)gr.FindControl("sg2_h11")).Checked = false;
                                }
                            }
                        }
                        fgen.msg("-", "AMSG", "No Schedule Feeded for this Order,'13'Prodn Qty Picked from Sales Order'13' Start Entry of Delv Dates.");
                    }
                    #endregion
                    break;

                case "MONTH":
                    if (col1.Length <= 0) return;
                    hffield.Value = "MONTH_E";
                    doc_addl.Value = col1;
                    SQuery = "select '1' as fstr,'[1] Pick Up All Items Of Grid' as msg from dual union all select '2' as fstr,'[2] Multi Item (Only in Case of Family items)' as msg from dual";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;

                case "MONTH_E":
                    if (col1.Length <= 0) return;
                    #region
                    doc_vty.Value = col1;
                    if (ViewState["sg2"] != null)
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = Convert.ToInt32(sg2.Rows[i].Cells[6].Text);
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
                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        SQuery = "select a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.icode) as icode,a.ciname,a.qtyord,a.desp_to,to_char(a.del_date,'dd/mm/yyyy') as del_date,i.cpartno from somas a,item i where trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + txtFstr.Text + "' ORDER BY srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        int colindex = 2; int colname = 1;
                        if (Convert.ToInt32(doc_addl.Value) > 3 && Convert.ToInt32(doc_addl.Value) <= 12)
                        {

                        }
                        else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

                        for (int l = 0; l < sg1.Rows.Count - 1; l++)
                        {
                            string dd = sg1.Rows[l].Cells[16].Text.Trim();
                            colindex = 2; colname = 1;
                            for (int k = 20; k < sg1.Columns.Count; k++)
                            {
                                if (fgen.make_double(((TextBox)sg1.Rows[l].FindControl("sg1_t" + colindex)).Text) > 0)
                                {
                                    sg2_dr = sg2_dt.NewRow();
                                    sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                                    sg2_dr["sg2_h1"] = "-";
                                    sg2_dr["sg2_h2"] = "-";
                                    sg2_dr["sg2_h3"] = "-";
                                    sg2_dr["sg2_h4"] = "-";
                                    sg2_dr["sg2_h5"] = "-";
                                    sg2_dr["sg2_h6"] = "-";
                                    sg2_dr["sg2_h7"] = "-";
                                    sg2_dr["sg2_h8"] = "-";
                                    sg2_dr["sg2_h9"] = fgen.seek_iname_dt(dt, "icode='" + sg1.Rows[l].Cells[16].Text.Trim() + "'", "desp_to");
                                    sg2_dr["sg2_h10"] = "-";
                                    sg2_dr["sg2_h11"] = "N";
                                    sg2_dr["sg2_f1"] = "-";
                                    sg2_dr["sg2_f2"] = fgen.seek_iname_dt(dt, "icode='" + sg1.Rows[l].Cells[16].Text.Trim() + "'", "cpartno");
                                    sg2_dr["sg2_f3"] = sg1.Rows[l].Cells[16].Text.Trim();
                                    sg2_dr["sg2_f4"] = sg1.Rows[l].Cells[17].Text.Trim();
                                    sg2_dr["sg2_f5"] = "-";
                                    if (colname < 10)
                                    {
                                        sg2_dr["sg2_t2"] = "0" + colname + "/" + doc_addl.Value + "/" + frm_myear;
                                    }
                                    else
                                    {
                                        sg2_dr["sg2_t2"] = colname + "/" + doc_addl.Value + "/" + frm_myear;
                                    }
                                    sg2_dr["sg2_t3"] = fgen.make_double(((TextBox)sg1.Rows[l].FindControl("sg1_t" + colindex)).Text);
                                    sg2_dt.Rows.Add(sg2_dr);
                                }
                                colindex++;
                                colname++;
                            }
                        }
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        setColHeadings();
                        foreach (GridViewRow gr in sg2.Rows)
                        {
                            string hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                            if (hf == "Y")
                            {
                                ((CheckBox)gr.FindControl("sg2_h11")).Checked = true;
                            }
                            else
                            {
                                ((CheckBox)gr.FindControl("sg2_h11")).Checked = false;
                            }
                        }
                    }
                    #endregion
                    break;

                case "sg1_t5":
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t7")).Text = col1;
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_ppc_reps(frm_qstr);
                    break;

                case "TACODE":
                    //-----------------------------
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    btnlbl7.Focus();
                    break;
                //-----------------------------

                case "BTN_10":
                    if (col1.Length <= 0) return;
                    txtlbl10.Text = col2;
                    btnlbl11.Focus();
                    break;

                case "BTN_11":
                    if (col1.Length <= 0) return;
                    txtlbl11.Text = col2;
                    btnlbl12.Focus();
                    break;

                case "BTN_12":
                    if (col1.Length <= 0) return;
                    txtlbl12.Text = col2;
                    btnlbl13.Focus();
                    break;

                case "BTN_13":
                    if (col1.Length <= 0) return;
                    txtlbl13.Text = col2;
                    btnlbl14.Focus();
                    break;

                case "BTN_14":
                    if (col1.Length <= 0) return;
                    txtlbl14.Text = col2;
                    break;

                case "BTN_15":

                    break;
                case "BTN_16":

                    break;
                case "BTN_17":

                    break;
                case "BTN_18":

                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
                    break;

                case "TICODEX":
                    if (col1.Length <= 0) return;
                    txtlbl2.Focus();
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[12].Text);
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
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();
                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        SQuery = "select a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.icode) as icode,a.ciname,a.qtyord from somas a where a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')||trim(a.icode) in (" + col1 + ") ORDER BY srno";
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
                            sg1_dr["sg1_f1"] = dt.Rows[d]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["ciname"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["ciname"].ToString().Trim();
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
                            sg1_dr["sg1_t17"] = "";
                            sg1_dr["sg1_t18"] = "";
                            sg1_dr["sg1_t19"] = "";
                            sg1_dr["sg1_t20"] = "";
                            sg1_dr["sg1_t21"] = "";
                            sg1_dr["sg1_t22"] = "";
                            sg1_dr["sg1_t23"] = "";
                            sg1_dr["sg1_t24"] = "";
                            sg1_dr["sg1_t25"] = "";
                            sg1_dr["sg1_t26"] = "";
                            sg1_dr["sg1_t27"] = "";
                            sg1_dr["sg1_t28"] = "";
                            sg1_dr["sg1_t29"] = "";
                            sg1_dr["sg1_t30"] = "";
                            sg1_dr["sg1_t31"] = "";
                            sg1_dr["sg1_t32"] = "";
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
                    //********* Saving in Hidden Field
                    dt = new DataTable();
                    SQuery = "select a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.icode) as icode,a.ciname,a.qtyord from somas a where a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')||trim(a.icode) ='" + col1 + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f1")).Text = dt.Rows[0]["qtyord"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f2")).Text = dt.Rows[0]["qtyord"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[0]["ciname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[0]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[0]["ciname"].ToString().Trim();
                    }
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
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();
                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
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

                case "SG2_ROW_ADD":
                    if (col1.Length < 2) return;
                    #region for gridview 2
                    if (col1.Length <= 0) return;
                    if (ViewState["sg2"] != null)
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = Convert.ToInt32(dt.Rows[i]["sg2_srno"].ToString());
                            sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                            sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                            sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();
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
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        sg2_dr = sg2_dt.NewRow();
                        sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                        sg2_dr["sg2_h1"] = col1;
                        sg2_dr["sg2_h2"] = col2;
                        sg2_dr["sg2_h3"] = "-";
                        sg2_dr["sg2_h4"] = "-";
                        sg2_dr["sg2_h5"] = "-";
                        sg2_dr["sg2_f1"] = col1;
                        sg2_dr["sg2_f2"] = col2;
                        sg2_dr["sg2_f3"] = "-";
                        sg2_dr["sg2_f4"] = "-";
                        sg2_dr["sg2_f5"] = "-";
                        sg2_dr["sg2_t1"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                        sg2_dr["sg2_t2"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                        sg2_dr["sg2_t3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                        sg2_dr["sg2_t4"] = "0";
                        sg2_dr["sg2_t5"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");
                        sg2_dt.Rows.Add(sg2_dr);
                    }
                    sg2_add_blankrows();
                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    dt.Dispose(); sg2_dt.Dispose();
                    ((TextBox)sg2.Rows[z].FindControl("sg2_t1")).Focus();
                    #endregion
                    setColHeadings();
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
                        for (i = 0; i < sg2.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = (i + 1);
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
                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        i = 0;
                        foreach (GridViewRow gr in sg2.Rows)
                        {
                            gr.Cells[6].Text = (i + 1).ToString();
                            string hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                            if (hf == "Y")
                            {
                                ((CheckBox)gr.FindControl("sg2_h11")).Checked = true;
                            }
                            else
                            {
                                ((CheckBox)gr.FindControl("sg2_h11")).Checked = false;
                            }
                            i++;
                        }
                    }
                    #endregion
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

                case "SG4_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg4_dt = new DataTable();
                        dt = (DataTable)ViewState["sg4"];
                        z = dt.Rows.Count - 1;
                        sg4_dt = dt.Clone();
                        sg4_dr = null;
                        i = 0;
                        for (i = 0; i < sg4.Rows.Count - 1; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = (i + 1);

                            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();


                            sg4_dt.Rows.Add(sg4_dr);
                        }

                        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg4_add_blankrows();

                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SALES":
                    doc_addl.Value = col1;
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        string party_cd = "";
        string part_cd = "";
        party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
        part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");        
        if (hffield.Value == "List")
        {
            SQuery = "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel(lblheader.Text + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SALES")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", doc_addl.Value);
            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
            fgen.fin_sales_reps(frm_qstr);
        }
        else if (hffield.Value == "JOB")
        {
            SQuery = "select distinct trim(B.INAME) as Item_Name,A.vchnum AS Job_No,to_char(A.vchdate,'dd/mm/yyyy') as Dated,A.QTY as Qty,TRIM(a.icode) AS  icode,TRIM(b.cpartno) as Part_No,decode(a.status,'Y','Completed','Current') as Jstatus,to_char(a.vchdate,'yyyymmdd') as vdd from costestimate A,ITEM B  WHERE trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' AND a.vchdate " + PrdRange + " and A.vchnum<>'000000' order by vdd desc,A.vchnum desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Job Order Status Report from " + fromdt + " to " + todt + " ", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SCHEDULE")
        {
            if (party_cd.Length > 2)
            {
                mq1 = "and a.acode ='" + party_cd + "'";
            }
            else
            {
                mq1 = "and a.acode like '" + party_cd + "%' ";
            }
            if (part_cd.Length > 2)
            {
                mq2 = "and a.icode ='" + part_cd + "'";
            }
            else
            {
                mq2 = "and a.icode like '" + part_cd + "%' ";
            }
            SQuery = "select trim(A.Soremarks) as Soremarks,trim(c.iname) as Item,trim(c.cpartno) as Part_No,trim(b.aname) as Customer,a.actualcost as Qty,a.desc_ as Delv_Dt,a.socat as Order_Stat,trim(a.vchnum) as Order_No,to_char(a.vchdate,'dd/mm/yyyy') as Orddt from budgmst a,famst b,item c where a.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(b.acode) and A.vchdate " + PrdRange + " AND trim(a.icode)=trim(c.icode) and a.JOBCARDRQD ='Y' " + mq1 + " " + mq2 + " order by b.aname,a.vchdate,a.vchnum,c.iname,a.desc_";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Schedule CheckList from " + fromdt + " to " + todt + " ", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "PENDING")
        {
            SQuery = "Select trim(ciname) as ciname,trim(cpartno) as cpartno,nvl(qtyord,0) as qtyord,nvl(qty_out,0) as qty_out,bal,pordno,to_char(porddt,'dd/mm/yyyy') as porddt,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,irate from pending_so_vu00 where branchcd='" + frm_mbr + "' and orddt " + PrdRange + "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Pending Report from " + fromdt + " to " + todt + " ", frm_qstr);
        }
        else if (hffield.Value == "ORDER")
        {
            if (party_cd.Length > 2)
            {
                mq1 = "and a.acode ='" + party_cd + "'";
            }
            else
            {
                mq1 = "and a.acode like '" + party_cd + "%' ";
            }
            if (part_cd.Length > 2)
            {
                mq2 = "and a.icode ='" + part_cd + "'";
            }
            else
            {
                mq2 = "and a.icode like '" + part_cd + "%' ";
            }
            SQuery = "select b.aname,c.iname,c.cpartno,trim(a.Order_No) as Order_No,a.Orddt,sum(a.qtyord)as Ord_qty,sum(a.sch_qty)As Sch_Qty,sum(a.prd_qty)As prd_Qty,sum(a.qtyord)-sum(a.sch_qty) as Diff1,sum(a.qtyord)-sum(a.prd_qty) as Diff2,trim(a.acode) as Acode,trim(a.icode)As Icode from (select a.ordno as Order_No,to_char(a.orddt,'dd/mm/yyyy') as Orddt,trim(a.acode) as acode,trim(a.icode) as icode,a.qtyord,0 as sch_qty,0 as prd_qty from SOMAS a where a.branchcd='" + frm_mbr + "' and A.orddt " + PrdRange + " " + mq1 + " " + mq2 + " and a.icat<>'Y' union all select a.vchnum as Order_No,to_char(a.vchdate,'dd/mm/yyyy') as Orddt,trim(a.acode) as acode,trim(a.icode) as icode,0 as qty_ord,a.budgetcost as Qty1,a.actualcost as Qty2 from budgmst a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and A.vchdate " + PrdRange + " " + mq1 + " " + mq2 + ") a,famst b, item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(C.icode) group by a.Order_No,a.Orddt,trim(a.acode),trim(a.icode),b.aname,c.iname,c.cpartno order by a.orddt,trim(a.order_no)";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Schedule CheckList from " + fromdt + " to " + todt + " ", frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            //string last_entdt;
            //checks
            //last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
            //if (last_entdt == "0") { }
            //else if (edmode.Value != "Y")
            //{
            //    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
            //    {
            //        Checked_ok = "N";
            //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
            //    }
            //}
            //last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            //if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            //{
            //    Checked_ok = "N";
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            //}
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

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);

                        oDS3 = new DataSet();
                        oporow3 = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname3);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        save_fun2();
                        save_fun3();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname3);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            //    if (sg1.Rows[i].Cells[9].Text.Trim().Length > 2)
                            //    {
                            //        save_it = "Y";
                            //    }
                            //}

                            //if (save_it == "Y")
                            //{
                            //    string doc_is_ok = "";
                            //    frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                            //    doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                            //    if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            //}
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        //if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        save_fun2();
                        save_fun3();

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname2 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname3 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'");
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname2);
                        fgen.save_data(frm_qstr, frm_cocd, oDS3, frm_tabname3);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname2 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname3 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'");
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t23", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t24", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t25", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t26", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t27", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t28", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t29", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t30", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t31", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t32", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
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
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
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
    public void create_tab4()
    {
        sg4_dt = new DataTable();
        sg4_dr = null;
        // Hidden Field
        sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));
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
        sg1_dr["sg1_t17"] = "";
        sg1_dr["sg1_t18"] = "";
        sg1_dr["sg1_t19"] = "";
        sg1_dr["sg1_t20"] = "";
        sg1_dr["sg1_t21"] = "";
        sg1_dr["sg1_t22"] = "";
        sg1_dr["sg1_t23"] = "";
        sg1_dr["sg1_t24"] = "";
        sg1_dr["sg1_t25"] = "";
        sg1_dr["sg1_t26"] = "";
        sg1_dr["sg1_t27"] = "";
        sg1_dr["sg1_t28"] = "";
        sg1_dr["sg1_t29"] = "";
        sg1_dr["sg1_t30"] = "";
        sg1_dr["sg1_t31"] = "";
        sg1_dr["sg1_t32"] = "";
        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_h1"] = "-";
        sg2_dr["sg2_h2"] = "-";
        sg2_dr["sg2_h3"] = "-";
        sg2_dr["sg2_h4"] = "-";
        sg2_dr["sg2_h5"] = "-";
        sg2_dr["sg2_h6"] = "-";
        sg2_dr["sg2_h7"] = "-";
        sg2_dr["sg2_h8"] = "-";
        sg2_dr["sg2_h9"] = "-";
        sg2_dr["sg2_h10"] = "-";
        sg2_dr["sg2_h11"] = "-";
        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dr["sg2_t3"] = "-";
        sg2_dr["sg2_t4"] = "-";
        sg2_dr["sg2_t5"] = "-";
        sg2_dr["sg2_t6"] = "-";
        sg2_dr["sg2_t7"] = "-";
        sg2_dr["sg2_t8"] = "-";
        sg2_dr["sg2_t9"] = "-";
        sg2_dr["sg2_t10"] = "-";
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
    public void sg4_add_blankrows()
    {
        sg4_dr = sg4_dt.NewRow();
        sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
        sg4_dr["sg4_t1"] = "-";
        sg4_dr["sg4_t2"] = "-";
        sg4_dt.Rows.Add(sg4_dr);
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
            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 30;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.HeaderRow.Cells[18].Text = "Tot_Qty";
            sg1.HeaderRow.Cells[19].Text = "1";
            sg1.HeaderRow.Cells[20].Text = "2";
            sg1.HeaderRow.Cells[21].Text = "3";
            sg1.HeaderRow.Cells[22].Text = "4";
            sg1.HeaderRow.Cells[23].Text = "5";
            sg1.HeaderRow.Cells[24].Text = "6";
            sg1.HeaderRow.Cells[25].Text = "7";
            sg1.HeaderRow.Cells[26].Text = "8";
            sg1.HeaderRow.Cells[27].Text = "9";
            sg1.HeaderRow.Cells[28].Text = "10";
            sg1.HeaderRow.Cells[29].Text = "11";
            sg1.HeaderRow.Cells[30].Text = "12";
            sg1.HeaderRow.Cells[31].Text = "13";
            sg1.HeaderRow.Cells[32].Text = "14";
            sg1.HeaderRow.Cells[33].Text = "15";
            sg1.HeaderRow.Cells[34].Text = "16";
            sg1.HeaderRow.Cells[35].Text = "17";
            sg1.HeaderRow.Cells[36].Text = "18";
            sg1.HeaderRow.Cells[37].Text = "19";
            sg1.HeaderRow.Cells[38].Text = "20";
            sg1.HeaderRow.Cells[39].Text = "21";
            sg1.HeaderRow.Cells[40].Text = "22";
            sg1.HeaderRow.Cells[41].Text = "23";
            sg1.HeaderRow.Cells[42].Text = "24";
            sg1.HeaderRow.Cells[43].Text = "25";
            sg1.HeaderRow.Cells[44].Text = "26";
            sg1.HeaderRow.Cells[45].Text = "27";
            sg1.HeaderRow.Cells[46].Text = "28";
            sg1.HeaderRow.Cells[47].Text = "29";
            sg1.HeaderRow.Cells[48].Text = "30";
            sg1.HeaderRow.Cells[49].Text = "31";
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
                hffield.Value = "SG2_ROW_ADD";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                col1 = "";
                foreach (GridViewRow gr2 in sg2.Rows)
                {
                    if (col1.Length > 0) col1 += ",'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                    else col1 = "'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                }

                SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,OPRATE1 AS SIZE_,OPRATE3 AS GSM,UNIT FROM ITEM WHERE TRIM(ICODE) LIKE '7%' ORDER BY ICODE ";
                SQuery = "SELECT TRIM(A.ICODE) AS FSTR,B.INAME AS PRODUCT,A.ICODE AS ERPCODE,A.KCLREELNO,A.COREELNO,B.OPRATE1,B.OPRATE3,B.UNIT,a.irate FROM REELVCH A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) and trim(a.icode)||trim(a.kclreelno) not in (" + col1 + ") ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Item", frm_qstr);
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
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "sg4_RMV":
                if (index < sg4.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "sg4_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "sg4_ROW_ADD":
                dt = new DataTable();
                sg4_dt = new DataTable();
                dt = (DataTable)ViewState["sg4"];
                z = dt.Rows.Count - 1;
                sg4_dt = dt.Clone();
                sg4_dr = null;
                i = 0;
                for (i = 0; i < sg4.Rows.Count; i++)
                {
                    sg4_dr = sg4_dt.NewRow();
                    sg4_dr["sg4_srno"] = (i + 1);
                    sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                    sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                    sg4_dt.Rows.Add(sg4_dr);
                }
                sg4_add_blankrows();
                ViewState["sg4"] = sg4_dt;
                sg4.DataSource = sg4_dt;
                sg4.DataBind();
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer ", frm_qstr);
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
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_19";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        mq1 = "";
        mq0 = "select distinct icode from budgmst where branchcd='" + frm_mbr + "' and type='46' and vchnum='" + txtvchnum.Text.Trim() + "'  and to_char(vchdate,'dd/mm/yyyy')='" + txtvchdate.Text + "' and substr(trim(icode),1,1)!='9'";
        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

        for (i = 0; i < sg2.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = lbl1a.Text;
            oporow["VCHNUM"] = txtvchnum.Text.Trim().ToUpper();
            oporow["VCHDATE"] = txtvchdate.Text.Trim().ToUpper();
            oporow["ACODE"] = txtlbl4.Text.Trim().ToUpper();
            oporow["SRNO"] = i + 1;
            oporow["ICODE"] = sg2.Rows[i].Cells[9].Text.Trim().ToUpper();
            oporow["DESC_"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
            oporow["BUDGETCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim().ToUpper());
            oporow["ACTUALCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
            oporow["SOLINK"] = txtFstr.Text.Trim().ToUpper();
            oporow["SOCAT"] = txtlbl2.Text.Trim().ToUpper();
            oporow["SOREMARKS"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper();
            oporow["JOBCARDNO"] = sg2.Rows[i].Cells[3].Text.Trim().ToUpper().Replace("&NBSP;", "-");
            oporow["JOBCARDQTY"] = fgen.make_double(sg2.Rows[i].Cells[7].Text.Trim().ToUpper());
            if (((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper() == "-")
            {
                oporow["JOBCARDRQD"] = "N";
            }
            else
            {
                oporow["JOBCARDRQD"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper();
            }

            oporow["REQ_CLOSEDBY"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim().ToUpper();
            oporow["TOLERANCE"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper());
            if (((CheckBox)sg2.Rows[i].FindControl("sg2_h11")).Checked == true)
            {
                oporow["CLOSEIT"] = "Y";
            }
            else
            {
                oporow["CLOSEIT"] = "N";
            }
            oporow["CLOSEBY"] = "-";
            if (sg2.Rows[i].Cells[9].Text.Trim().ToUpper().Substring(0, 1) == "9")
            {
                oporow["SPLCODE"] = "-";
            }
            else
            {
                oporow["SPLCODE"] = "CHILD";
            }
            oporow["APP_BY"] = "-";
            oporow["APP_DT"] = txtvchdate.Text;
            oporow["CUSTDLV"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper();
            oporow["DLV_DATE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
            oporow["ORG_PONO"] = vardate + " " + frm_uname;
            oporow["CFW_DATA"] = sg2.Rows[i].Cells[25].Text.Trim().ToUpper();
            oporow["REQ_CL_RSN"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim().ToUpper();
            oporow["REV_RMK"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim().ToUpper();
            oporow["FROMSO"] = "-";
            oporow["SODESC1"] = "-";
            oporow["BTCHNO"] = "-";
            oporow["CCPARTNO"] = "-";
            oporow["PPORDNO"] = "-";
            oporow["JOBUPS"] = 0;
           
            //if (edmode.Value == "Y")
            //{
            //    oporow["eNt_by"] = ViewState["entby"].ToString();
            //    oporow["eNt_dt"] = ViewState["entdt"].ToString();
            //    oporow["edt_by"] = frm_uname;
            //    oporow["edt_dt"] = vardate;
            //}
            //else
            //{
            //    oporow["eNt_by"] = frm_uname;
            //    oporow["eNt_dt"] = vardate;
            //    oporow["edt_by"] = "-";
            //    oporow["eDt_dt"] = vardate;
            //}
            oDS.Tables[0].Rows.Add(oporow);

            if (doc_vty.Value == "2")
            {
                if (fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper()) > 0)
                {
                    #region Child Parts
                    SQuery = "select trim(icode) as icode,trim(ibcode) as ibcode from itemosp2 where icode='" + sg2.Rows[i].Cells[9].Text.Trim().ToUpper() + "' order by srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        mq1 = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[k]["ibcode"].ToString().Trim() + "'", "icode");
                        if (mq1.Length <= 1)
                        {
                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = frm_mbr;
                            oporow["TYPE"] = lbl1a.Text;
                            oporow["VCHNUM"] = txtvchnum.Text.Trim().ToUpper();
                            oporow["VCHDATE"] = txtvchdate.Text.Trim().ToUpper();
                            oporow["ACODE"] = txtlbl4.Text.Trim().ToUpper();
                            oporow["SRNO"] = i + 1;
                            oporow["ICODE"] = dt.Rows[k]["ibcode"].ToString().Trim();
                            oporow["DESC_"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
                            oporow["BUDGETCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim().ToUpper());
                            oporow["ACTUALCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
                            oporow["SOLINK"] = txtFstr.Text.Trim().ToUpper();
                            oporow["SOCAT"] = txtlbl2.Text.Trim().ToUpper();
                            oporow["SOREMARKS"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper();
                            oporow["JOBCARDNO"] = sg2.Rows[i].Cells[3].Text.Trim().ToUpper().Replace("&NBSP;", "-");
                            oporow["JOBCARDQTY"] = fgen.make_double(sg2.Rows[i].Cells[7].Text.Trim().ToUpper());
                            if (((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper() == "-")
                            {
                                oporow["JOBCARDRQD"] = "N";
                            }
                            else
                            {
                                oporow["JOBCARDRQD"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper();
                            }
                            oporow["REQ_CLOSEDBY"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim().ToUpper();
                            oporow["TOLERANCE"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper());
                            if (((CheckBox)sg2.Rows[i].FindControl("sg2_h11")).Checked == true)
                            {
                                oporow["CLOSEIT"] = "Y";
                            }
                            else
                            {
                                oporow["CLOSEIT"] = "N";
                            }
                            oporow["CLOSEBY"] = "-";
                            oporow["SPLCODE"] = "CHILD";
                            oporow["APP_BY"] = "-";
                            oporow["APP_DT"] = txtvchdate.Text;
                            oporow["CUSTDLV"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper();
                            oporow["DLV_DATE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
                            oporow["ORG_PONO"] = vardate + " " + frm_uname;
                            oporow["CFW_DATA"] = sg2.Rows[i].Cells[25].Text.Trim().ToUpper();
                            oporow["REQ_CL_RSN"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim().ToUpper();
                            oporow["REV_RMK"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim().ToUpper();
                            oporow["FROMSO"] = "-";
                            oporow["SODESC1"] = "";
                            oporow["BTCHNO"] = "";
                            oporow["CCPARTNO"] = "";
                            oporow["PPORDNO"] = "";
                            oporow["JOBUPS"] = 0;
                            oDS.Tables[0].Rows.Add(oporow);
                        }
                    }
                    #endregion
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        z = 0;
        for (i = 0; i < sg2.Rows.Count; i++)
        {
            if (sg2.Rows[i].Cells[9].Text.Trim().ToUpper().Substring(0, 1) == "9")
            {
                oporow2 = oDS2.Tables[0].NewRow();
                oporow2["BRANCHCD"] = frm_mbr;
                oporow2["TYPE"] = lbl1a.Text;
                oporow2["VCHNUM"] = txtvchnum.Text.Trim().ToUpper();
                oporow2["VCHDATE"] = txtvchdate.Text.Trim().ToUpper();
                oporow2["ACODE"] = txtlbl4.Text.Trim().ToUpper();
                oporow2["SRNO"] = z + 1;
                oporow2["ICODE"] = sg2.Rows[i].Cells[9].Text.Trim().ToUpper();
                oporow2["DESC_"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
                oporow2["BUDGETCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim().ToUpper());
                oporow2["ACTUALCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
                oporow2["SOLINK"] = txtFstr.Text.Trim().ToUpper();
                oporow2["SOCAT"] = txtlbl2.Text.Trim().ToUpper();
                oporow2["SOREMARKS"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper();
                oporow2["JOBCARDNO"] = sg2.Rows[i].Cells[3].Text.Trim().ToUpper().Replace("&NBSP;", "-");
                oporow2["JOBCARDQTY"] = fgen.make_double(sg2.Rows[i].Cells[7].Text.Trim().ToUpper());
                if (((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper() == "-")
                {
                    oporow2["JOBCARDRQD"] = "N";
                }
                else
                {
                    oporow2["JOBCARDRQD"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper();
                }
                oporow2["REQ_CLOSEDBY"] = "-";
                oporow2["TOLERANCE"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper());
                if (((CheckBox)sg2.Rows[i].FindControl("sg2_h11")).Checked == true)
                {
                    oporow2["CLOSEIT"] = "Y";
                }
                else
                {
                    oporow2["CLOSEIT"] = "N";
                }
                oporow2["CLOSEBY"] = "-";
                oporow2["APP_BY"] = "-";
                oporow2["APP_DT"] = txtvchdate.Text;
                oporow2["DLV_DATE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
                oporow2["CFW_DATA"] = sg2.Rows[i].Cells[25].Text.Trim().ToUpper();
                oDS2.Tables[0].Rows.Add(oporow2);
                z++;
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        z = 0;
        for (i = 0; i < sg2.Rows.Count; i++)
        {
            if (sg2.Rows[i].Cells[9].Text.Trim().ToUpper().Substring(0, 1) == "9")
            {
                if (fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper()) > 0)
                {
                    oporow3 = oDS3.Tables[0].NewRow();
                    oporow3["BRANCHCD"] = frm_mbr;
                    oporow3["TYPE"] = lbl1a.Text;
                    oporow3["VCHNUM"] = txtvchnum.Text.Trim().ToUpper();
                    oporow3["VCHDATE"] = txtvchdate.Text.Trim().ToUpper();
                    oporow3["ACODE"] = txtlbl4.Text.Trim().ToUpper();
                    oporow3["SRNO"] = z + 1;
                    oporow3["ICODE"] = sg2.Rows[i].Cells[9].Text.Trim().ToUpper();
                    oporow3["DESC_"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
                    oporow3["BUDGETCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim().ToUpper());
                    oporow3["ACTUALCOST"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
                    oporow3["SOLINK"] = txtFstr.Text.Trim().ToUpper();
                    oporow3["SOCAT"] = txtlbl2.Text.Trim().ToUpper();
                    oporow3["SOREMARKS"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper();
                    oporow3["JOBCARDNO"] = sg2.Rows[i].Cells[3].Text.Trim().ToUpper().Replace("&NBSP;", "-");
                    oporow3["JOBCARDQTY"] = fgen.make_double(sg2.Rows[i].Cells[7].Text.Trim().ToUpper());
                    if (((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper() == "-")
                    {
                        oporow3["JOBCARDRQD"] = "N";
                    }
                    else
                    {
                        oporow3["JOBCARDRQD"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper();
                    }
                    oporow3["REQ_CLOSEDBY"] = "-";
                    oporow3["TOLERANCE"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper());
                    if (((CheckBox)sg2.Rows[i].FindControl("sg2_h11")).Checked == true)
                    {
                        oporow3["CLOSEIT"] = "Y";
                    }
                    else
                    {
                        oporow3["CLOSEIT"] = "N";
                    }
                    oporow3["CLOSEBY"] = "-";
                    oporow3["SPLCODE"] = "-";
                    oporow3["APP_BY"] = "-";
                    oporow3["APP_DT"] = txtvchdate.Text;
                    oporow3["CUSTDLV"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper();
                    oporow3["DLV_DATE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
                    oporow3["ORG_PONO"] = vardate + " " + frm_uname;
                    oporow3["CFW_DATA"] = sg2.Rows[i].Cells[25].Text.Trim().ToUpper();
                    oporow3["REV_RMK"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim().ToUpper();
                    oporow3["FROMSO"] = "-";
                    oDS3.Tables[0].Rows.Add(oporow3);
                    z++;
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {
    }
    //------------------------------------------------------------------------------------
    void save_fun5()
    {

    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50114":
            case "F35136":
                SQuery = "SELECT '11' AS FSTR,'Prodn Plan' as NAME,'11' AS CODE FROM dual";
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg2.Columns[0].HeaderStyle.Width = 30;
            sg2.Columns[1].HeaderStyle.Width = 30;
            sg2.Columns[2].HeaderStyle.Width = 50;
            sg2.Columns[4].HeaderStyle.Width = 50;
            sg2.Columns[5].HeaderStyle.Width = 50;
            sg2.Columns[6].HeaderStyle.Width = 50;
            sg2.Columns[10].HeaderStyle.Width = 250;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg2_t5_"))
        {
            hffield.Value = "sg1_t5";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t5_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select JobCard Requirement", frm_qstr);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnUpdate_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "MONTH";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Month", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnSales_Click(object sender, EventArgs e)
    {
        hffield.Value = "SALES";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Report Choice", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnJob_Click(object sender, EventArgs e)
    {
        hffield.Value = "JOB";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnSch_Click(object sender, EventArgs e)
    {
        hffield.Value = "SCHEDULE";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnItem_Click(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnPending_Click(object sender, EventArgs e)
    {
        hffield.Value = "PENDING";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnOrder_Click(object sender, EventArgs e)
    {
        hffield.Value = "ORDER";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
}