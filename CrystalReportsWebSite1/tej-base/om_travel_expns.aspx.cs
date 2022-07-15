using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_travel_expns : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4, sg1_dt;
    DataRow oporow, sg1_dr; DataSet oDS;

    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, cond = "";
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
                doc_addl.Value = "0";

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

        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "*******":
                tab3.Visible = false;
                tab4.Visible = false;
                break;
        }
        if (Prg_Id == "*******")
        {
            tab5.Visible = true;
        }
        lblheader.Text = "Travel Expense";
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;

        btnTrack.Disabled = true;

        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;

        btnTrack.Disabled = false;
        btnExpType.Enabled = true;
        btnLead.Enabled = true;
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
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "VCHDATE";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "EXP_BOOK";
        frm_vty = "EB";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        set_Val();
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            case "EXP":
                SQuery = "SELECT 'LD' AS FSTR,'AGAINST NEW LEAD' AS NAME,'LD' AS TYPE FROM DUAL UNION ALL SELECT 'CM' AS FSTR,'AGAINST CUSTOMER REQ' AS NAME,'CM' AS TYPE FROM DUAL UNION ALL SELECT 'EX' AS FSTR,'EXPENSE ENTRY' AS NAME,'EX' AS TYPE FROM DUAL";
                break;
            case "LEAD":
                switch (txtExpNo.Value)
                {
                    case "LD":
                        SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.vCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR,A.VCHNUM AS INVNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INVDT,B.ANAME AS CUSTOMER,C.INAME AS PRODUCT,C.CPARTNO,trim(B.ADDR1) as ADDR1,trim(B.ADDR2) as ADDR2,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,FAMST B ,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(a.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc ";
                        break;
                    case "CM":
                        SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||tRIM(A.vCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR,A.VCHNUM AS REQNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS REQDT,B.ANAME AS CUSTOMER,C.INAME AS PRODUCT,C.CPARTNO,trim(B.ADDR1) as ADDR1,trim(B.ADDR2) as ADDR2,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd FROM SCRATCH A,FAMST B ,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(a.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and a.type like 'CC%' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc ";
                        break;
                    case "EX":
                        SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.vCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR,A.VCHNUM AS INVNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INVDT,B.ANAME AS CUSTOMER,C.INAME AS PRODUCT,C.CPARTNO,trim(B.ADDR1) as ADDR1,trim(B.ADDR2) as ADDR2,a.acode,a.icode,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,FAMST B ,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(a.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and a.type in ('40','46') and substR(a.icode,1,2)!='59' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc ";
                        break;
                }
                break;
            case "New":
            case "List*":
            case "Edit*":
            case "Del*":
            case "Print*":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "List" || btnval == "Print")
                {
                    cond = "";
                    if (frm_ulvl != "0") cond = "and a.ent_by='" + frm_uname + "'";
                    SQuery = "SELECT DISTINCT TRIM(A." + doc_nf.Value + ")||TO_cHAR(A." + doc_df.Value + ",'DD/MM/YYYY') AS FSTR,A." + doc_nf.Value + " AS ENTRYNO,TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') AS ENTRYDT,A.col3 as leadno,a.col4 as leaddt,a.ent_by,a.ent_Dt,to_Char(A.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " " + cond + " order by vdd desc,a.vchnum desc ";
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

            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
            txtVchnum.Value = frm_vnum;
            txtvchdate.Value = vardate;
            disablectrl();
            fgen.EnableForm(this.Controls);

            create_tab();
            for (int i = 0; i < 30; i++)
            {
                sg1_add_blankrows();
            }
            sg1.DataSource = sg1_dt;
            sg1.DataBind();

            btnTrack_ServerClick(sender, EventArgs.Empty);
            btnExpType.Focus();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
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

    }

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
        sg1_dr["sg1_t17"] = "-";
        sg1_dr["sg1_t18"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " to Edit", frm_qstr);
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
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        fgen.fill_dash(this.Controls);
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " to Delete", frm_qstr);
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
        setColHeadings();

        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select " + lblheader.Text + " for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " for Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        set_Val();
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), DateTime.Now.ToString("dd/MM/yyyy"), frm_uname, "US", lblheader.Text.Trim() + " Deleted");
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
            btnval = hffield.Value;
            switch (btnval)
            {
                case "List":
                    if (col1 == "") return;
                    //
                    SQuery = "SELECT A." + doc_nf.Value + " AS ENTRYNO,TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') AS ENTRY_DT,A.COL3 AS LEAD_NO,A.COL4 AS LEAD_DT,a.col9 as track_address,A.COL10 AS Expense_ON,A.NUM1 AS AMT,A.COL5 AS ACODE,A.COL6 AS ERPCODE,a.srno FROM " + frm_tabname + " A ,ITEM C WHERE TRIM(A.COL6)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(A." + doc_nf.Value + ")||TO_cHAR(A." + doc_df.Value + ",'DD/MM/YYYY')='" + frm_mbr + frm_vty + col1 + "' order by a.srno  ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("-", frm_qstr);
                    break;
                case "EXP":
                    if (col1 == "") return;
                    txtExpNo.Value = col1;
                    txtExpHead.Value = col2;
                    btnLead.Focus();
                    break;
                case "LEAD":
                    if (col1 == "") return;

                    txtLeadNo.Value = col2;
                    txtLeadDt.Value = col3;

                    txtAname.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    txtIname.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                    txtCustAddr.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "") + " , " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8").ToString().Trim().Replace("&amp", "");

                    txtAcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");
                    txtIcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL10").ToString().Trim().Replace("&amp", "");

                    txtCustAddr.Focus();
                    break;
                case "New":
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");

                    disablectrl();
                    fgen.EnableForm(this.Controls);

                    // Popup asking for Copy from Older Data
                    fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    hffield.Value = "NEW_E";
                    break;
                    #endregion
                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    //hffield.Value = "Del_E";
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);                    
                    break;
                case "Del_E":
                    if (col1 == "") return;
                    clearctrl();
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Edit":
                    //edit_Click
                    #region Edit Start
                    double valueTot = 0;
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(A." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtVchnum.Value = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Value = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtExpNo.Value = dt.Rows[0]["col1"].ToString().Trim();
                        txtExpHead.Value = dt.Rows[0]["col2"].ToString().Trim();

                        txtLeadNo.Value = dt.Rows[0]["COL3"].ToString().Trim();
                        txtLeadDt.Value = dt.Rows[0]["COL4"].ToString().Trim();
                        txtAcode.Value = dt.Rows[0]["COL5"].ToString().Trim();

                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT b.aname,b.addr1||' ,'||b.addr2 AS ADDR FROM FAMST B WHERE TRIM(B.ACODE)='"+txtAcode.Value.Trim()+"' ");
                        if (dt2.Rows.Count > 0)
                        {
                            txtAname.Value = dt2.Rows[0]["ANAME"].ToString().Trim();
                            txtCustAddr.Value = dt2.Rows[0]["ADDR"].ToString().Trim();
                        }
                        txtIcode.Value = dt.Rows[0]["COL6"].ToString().Trim();
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT c.iname,c.cpartno FROM ITEM C WHERE TRIM(C.ICODE)='" + txtIcode.Value.Trim() + "'");
                        if (dt2.Rows.Count > 0)
                        {
                            txtIname.Value = dt2.Rows[0]["INAME"].ToString().Trim();
                        }
                        txtLat.Value = dt.Rows[0]["COL7"].ToString().Trim();
                        txtLong.Value = dt.Rows[0]["COL8"].ToString().Trim();
                        txtAddress.Text = dt.Rows[0]["COL9"].ToString().Trim();

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["col10"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["num1"].ToString().Trim();
                            valueTot += fgen.make_double(dt.Rows[i]["num1"].ToString().Trim());
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        for (i = 0; i < 10; i++)
                        {
                            sg1_add_blankrows();
                        }

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        if (frm_ulvl != "0")
                        {
                            btnTrack.Disabled = true;
                            txtAddress.Enabled = false;
                            btnExpType.Enabled = false;
                            btnLead.Enabled = false;
                            if (valueTot > 0)
                            {
                                btncancel_ServerClick("", EventArgs.Empty);
                                fgen.msg("-", "AMSG", "Can not Edit this Entry!!'13'Expense Already Filled");
                            }
                        }
                    }
                    #endregion
                    break;
                case "Print":
                    if (col1 == "") return;
                    SQuery = "Select a.*,b.aname,c.iname,c.cpartno,trim(b.addr1)||' ,'||trim(b.addr2) AS ADDR from " + frm_tabname + " a,famst b,item c where trim(a.col5)=trim(B.acodE) and trim(a.col6)=trim(c.icodE) and a.branchcd||a.type||trim(A." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "expBook", "expBook");
                    break;
                case "TRACKLOC":
                    if (Session["lat"] != null)
                    {
                        txtAddress.Text = (string)Session["addr"];
                        txtLat.Value = (string)Session["lat"];
                        txtLong.Value = (string)Session["long"];
                    }
                    btnExpType.Focus();
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
            cond = "";
            if (frm_ulvl != "0") cond = "and a.ent_by='" + frm_uname + "'";

            SQuery = "SELECT a.col2 as exp_type,TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') AS visit_DT,a.ent_by as mkt_person,A.COL3 AS LEAD_NO,A.COL4 AS LEAD_DT,B.ANAME AS CUSTOMER,B.ADDR1||' ,'||B.ADDR2 AS CLIENT_ADDRESS,a.col9 as track_address,c.iname as product_name,A.COL10 AS EXPeNSE_desc,A.NUM1 AS exp_AMT, A." + doc_nf.Value + " AS ENTRYNO,A.COL5 AS ACODE,A.COL6 AS ERPCODE,a.srno  FROM " + frm_tabname + " A,FAMST B ,ITEM C WHERE TRIM(A.COL5)=TRIM(B.ACODe) AND TRIM(A.COL6)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='EB' and a.vchdate " + PrdRange + " " + cond + " order by to_char(a.vchdate,'yyymmdd') desc,a.vchnum desc,a.srno  ";

            SQuery = "SELECT a.col2 as exp_type,TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') AS visit_DT,a.ent_by as mkt_person,A.COL3 AS LEAD_NO,A.COL4 AS LEAD_DT,a.col9 as track_address,c.iname as product_name,A.COL10 AS EXPeNSE_desc,A.NUM1 AS exp_AMT, A." + doc_nf.Value + " AS ENTRYNO,A.COL5 AS ACODE,A.COL6 AS ERPCODE,a.srno  FROM " + frm_tabname + " A,ITEM C WHERE TRIM(A.COL6)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='EB' and a.vchdate " + PrdRange + " " + cond + " order by to_char(a.vchdate,'yyymmdd') desc,a.vchnum desc,a.srno  ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            i = 0;
            //setColHeadings();

            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                try
                {
                    set_Val();

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

                    #region Number Gen and Save to Table
                    if (edmode.Value == "Y")
                    {
                        save_it = "Y";
                        frm_vnum = txtVchnum.Value;
                    }
                    else
                    {
                        save_it = "Y";
                        if (save_it == "Y")
                        {
                            string doc_is_ok = "";
                            frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Value.Trim(), frm_uname, Prg_Id);
                            doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                            if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                        }
                    }
                    #endregion
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
                        cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Saved Successfully ");
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
                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        int srno = 1;

        foreach (GridViewRow gr1 in sg1.Rows)
        {
            if (((TextBox)gr1.FindControl("sg1_t1")).Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["type"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Value;

                oporow["col1"] = txtExpNo.Value;
                oporow["col2"] = txtExpHead.Value;

                oporow["col3"] = txtLeadNo.Value;
                oporow["col4"] = txtLeadDt.Value;

                oporow["col5"] = txtAcode.Value;
                oporow["col6"] = txtIcode.Value;

                oporow["col7"] = txtLat.Value;
                oporow["col8"] = txtLong.Value;
                oporow["col9"] = txtAddress.Text;

                oporow["col10"] = ((TextBox)gr1.FindControl("sg1_t1")).Text;
                oporow["NUM1"] = fgen.make_double(((TextBox)gr1.FindControl("sg1_t2")).Text);

                oporow["srno"] = srno;

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
                srno++;
            }
        }
    }
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        SQuery = "SELECT 'ED' AS FSTR,'Record Efforts Done' as NAME,'ED' AS CODE FROM dual";
    }
    //------------------------------------------------------------------------------------   
    protected void btnTrack_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "TRACKLOC";
        string pagename = "../tej-base/getLocation.aspx";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + pagename + "?STR=" + frm_qstr + "','420px','500px','Pocketdriver Limited');", true);
    }
    protected void btnLead_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "LEAD";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnExpType_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "EXP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
}