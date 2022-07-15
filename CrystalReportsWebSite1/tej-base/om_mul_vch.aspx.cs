using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_mul_vch : System.Web.UI.Page
{
    DataTable dt, sg1_dt, sg2_dt, dt1;
    DataRow dr1, sg1_dr, sg2_dr, oporow;
    DataSet oDS;
    //----------------------------
    string btnval, col1, col2, col3, fill_Date, tmp_var, vip = "", mq1, mq0;
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    //----------------------------
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string SQuery, HCID, merr = "0", eff_Dt, m1;
    int i, z = 0;
    double drtot, crtot = 0;
    fgenDB fgen = new fgenDB();
    //----------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        // for loading page 
        frm_tabname = "VOUCHER";
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
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
                    tmp_var = "A";
                }
                else Response.Redirect("~/login.aspx");
            }

            fill_Date = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                set_Val();
                getColHeading();

                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (col1.Length == 20)
                    editVoucher();
            }
            btnnew.Visible = false;
            btnedit.Visible = false;
            btnlist.Visible = false;
            btndel.Visible = false;
            btnprint.Visible = false;
            setColHeadings();
        }
    }
    //----------------------------------------------------------------------------------------
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
    //----------------------------------------------------------------------------------------
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

            //for (int K = 0; K < sg1.Rows.Count; K++)
            //{
            //    if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");
            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");

            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
            //}
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
                //sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                //if (fgen.make_double(mcol_width) > 0)
                //{
                //sg1.HeaderRow.Cells[sR].Text
                //    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                //    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                //}
            }
        }
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        tab1.Visible = false;
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;

        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //----------------------------------------------------------------------------------------
    public void enablectrl()
    {
        // for enable/disable some variables

        btnnew.Disabled = false;
        btnedit.Disabled = false;
        btncancel.Visible = false;
        btndel.Disabled = false;

        btnexit.Visible = true;
        btnsave.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;

        btnprint.Disabled = false;
        btnlist.Disabled = false;

        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = dt1;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false;
    }
    //----------------------------------------------------------------------------------------
    public void disablectrl()
    {
        // for disable/enable some variables
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btndel.Disabled = true;
        btnprint.Disabled = true;
        btnlist.Disabled = true;


        btncancel.Visible = true;
        btnexit.Visible = false;


        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;

    }
    //----------------------------------------------------------------------------------------
    public void clearctrl()
    {
        // for clearing some variables
        hffield.Value = "";
        edmode.Value = "";
    }
    //----------------------------------------------------------------------------------------
    public void set_Val()
    {
        // for setting radio button , table , head label on various options
        lblheaderx.Text = "Multiple Entry Voucher";
        frm_tabname = "voucher"; frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY"); ;
    }
    //----------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        // for making query based on button value selected
        btnval = hffield.Value; set_Val();
        frm_vty = popselected.Value.Trim();

        switch (btnval)
        {
            case "PARTY_POP":
                SQuery = "select Acode as fstr,ANAME as Party,Acode as Code,Addr1 as Address,Addr2 as City,Payment,nvl(schgrate,0) as CDR  from famst where trim(nvl(GRP,'-')) in ('02','16','06','05')  order by aname ";
                break;

            case "PARTY_POP2":
                SQuery = "SELECT ACODE AS FSTR,ANAME AS NAME,ACODE AS CODE,ADDR1,ADDR2 FROM FAMST ORDER BY ANAME";
                break;

            case "Row_Add":
            case "Row_Edit":
                if (sg1.Rows.Count > 1)
                {
                    col1 = ""; col2 = "";
                    foreach (GridViewRow r1 in sg1.Rows)
                    {
                        if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[3].Text.Trim() + "'";
                        else col2 = "'" + r1.Cells[3].Text.Trim() + "'";
                    }
                    col2 = "(" + col2 + ")";
                }
                else col2 = " ('-')";
                SQuery = "select acode as fstr,Aname as product,Acode as code from famst order by acode";
                break;

            case "SG1_ROW_ADD_ACC":
                SQuery = "select acode as fstr,Aname as product,Acode as code from famst order by acode";
                break;

            case "SG1_ROW_ADD_CC":
                SQuery = "SELECT TRIM(TYPE1) AS FSTR,NAME,TYPE1 AS CODE from typegrp where id='C' ORDER BY NAME";
                break;

            case "GSTCLASS":
                SQuery = "SELECT TYPE1,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='}' ORDER BY TYPE1";
                break;

            case "TAX":
                SQuery = "SELECT 'CG' AS FSTR,'CG' AS TAXTYPE,'WITHIN STATE' AS NAME FROM DUAL UNION ALL SELECT 'IG' AS FSTR,'IG' AS TAXTYPE,'OUT OF STATE' AS NAME FROM DUAL";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as vOUCHER_NO,to_char(a.vchdate,'dd/mm/yyyy') as VCH_Dt,A.TYPE,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                if (btnval == "New" || btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                switch (Prg_Id)
                {
                    case "F70101":
                        SQuery = "Select type1 as fstr,Name,Type1 as Code,Acode as Account,Addr1 as Users From Type where id='V' and (substr(type1,1,1)='1' ) order by type1";
                        break;
                    case "F70106":
                        SQuery = "Select type1 as fstr,Name,Type1 as Code,Acode as Account,Addr1 as Users From Type where id='V' and (substr(type1,1,1)='2' ) order by type1";
                        break;
                    case "F70111":
                        SQuery = "Select type1 as fstr,Name,Type1 as Code,Acode as Account,Addr1 as Users From Type where id='V' and (substr(type1,1,2)='30') order by type1";
                        break;
                }
                break;
        }

        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "DATA");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    //----------------------------------------------------------------------------------------
    protected void btnnew_Click(object sender, EventArgs e)
    {
        // for new button popup
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        set_Val();
        if (chk_rights == "Y")
        {
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");
    }
    //----------------------------------------------------------------------------------------
    protected void btnedit_Click(object sender, EventArgs e)
    {
        // for edit button popup
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        set_Val();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");
    }
    //----------------------------------------------------------------------------------------
    protected void btnsave_Click(object sender, EventArgs e)
    {
        // for save button checking & working
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in this form!!");
            return;
        }

        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N" && edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in edit mode!!");
            return;
        }
        else
        {
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdate.Focus(); return; }

        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1")) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2")))
        { fgen.msg("-", "AMSG", "Back Year Date is not allowed!!'13'Fill date for this year only"); txtvchdate.Focus(); return; }


        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }
        if (txtGstCode.Text.Length == 1)
        {
            fgen.msg("-", "AMSG", "Please Select GST Class");
            return;
        }
        z = 1;
        for (int i = 0; i < sg1.Rows.Count - 1; i++)
        {
            drtot += fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
            crtot += fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
            m1 = sg1.Rows[i].Cells[16].Text.Trim();
            if (m1.Length == 1)
            {
                fgen.msg("-", "AMSG", "Invalid Rev Code at Line " + z + " !!'13' Please Check.");
                return;
            }
            z++;
        }
        if (Math.Round(crtot, 2) != Math.Round(drtot, 2))
        {
            fgen.msg("-", "AMSG", "Credit Amt is not Matching with Debit Amt !!'13' Please Check.");
            return;
        }
        fgen.msg("-", "ISMSG", "Are you sure, you want to Save!!");
    }
    //----------------------------------------------------------------------------------------
    protected void btndel_Click(object sender, EventArgs e)
    {
        // for del button working
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to delete data in this form");
        }
        else
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type to Delete", frm_qstr);
        }
    }
    //----------------------------------------------------------------------------------------
    protected void btnexit_Click(object sender, EventArgs e)
    {
        // for exit button working
        //Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_OPEN_IN_EDIT") == "Y")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "N");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
        }
        else
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "OnlyClose();", true);
    }
    //----------------------------------------------------------------------------------------
    protected void btncancel_Click(object sender, EventArgs e)
    {
        // for cancel button working
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        dt1 = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = dt1;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; dt1.Dispose();
        ViewState["sg1"] = null;
        lblEdtBy.InnerText = "";
    }
    //----------------------------------------------------------------------------------------
    protected void btnlist_Click(object sender, EventArgs e)
    {
        // for list button 
        clearctrl();
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for List", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
    protected void cmdrep1_Click(object sender, EventArgs e)
    {
        // for doing print
        hffield.Value = "CMD_REP1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
    protected void cmdrep2_Click(object sender, EventArgs e)
    {
        // for doing print
        hffield.Value = "CMD_REP2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
    protected void btnprint_Click(object sender, EventArgs e)
    {
        // for doing print
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        // for doing multiple work on postback 
        set_Val();
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, popselected.Value.Substring(4, 6), popselected.Value.Substring(10, 10), frm_uname, popselected.Value.Substring(2, 2), "Voucher DELETED");
                fgen.msg("-", "AMSG", "Details are deleted for Voucher Entry " + popselected.Value.Substring(4, 6) + "");
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
                case "CMD_REP1":
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    fgen.Fn_open_prddmp1("Select Date Range for List Of Bom Listing", frm_qstr);
                    break;

                case "New":
                    clearctrl();
                    set_Val();
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbltypename.Text = col1 + " : " + col2;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "'", 6, "vch");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fill_Date;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.ACODE,B.ANAME FROM TYPE A,FAMST B WHERE TRIM(a.ACODE)=TRIM(b.ACODE) AND A.id='V' and trim(a.type1)='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {

                    }
                    disablectrl(); fgen.EnableForm(this.Controls);
                    break;
                case "Del":
                    clearctrl();
                    set_Val();
                    hffield.Value = "Del_E";
                    popselected.Value = col1;
                    frm_vty = col1;
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Voucher to delete", frm_qstr);
                    break;
                case "Del_E":
                    clearctrl();
                    popselected.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Edit":
                    // this is after type selection 
                    clearctrl();
                    set_Val();
                    hffield.Value = "Edit_E";
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to edit", frm_qstr);
                    break;
                case "Edit_E":
                    // this is after entry selection
                    if (col1 == "") return;
                    popselected.Value = col1;
                    editVoucher();
                    break;
                case "Print":
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Voucher Type to Print", frm_qstr);
                    break;
                case "Print_E":
                    frm_sql = "select * from " + frm_tabname + " where branchcd||type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + col1 + "' ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, frm_sql, "rpt_test", "rpt_test");
                    break;
                case "List":
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    fgen.Fn_open_prddmp1("Select Date Range for List Of BOMs", frm_qstr);
                    break;
                case "Row_Add":
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

                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = col1;
                            sg1_dr["sg1_f2"] = col2;
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    //dt.Dispose(); 
                    sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    break;
                case "Row_Edit":
                    if (col1 == "") return;
                    sg1.Rows[Convert.ToInt32(hf1.Value.Trim())].Cells[14].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value.Trim())].Cells[15].Text = col2;
                    break;
                case "SG1_ROW_ADD_ACC":
                    if (col1 == "") return;
                    sg1.Rows[Convert.ToInt32(hf1.Value.Trim())].Cells[17].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value.Trim())].Cells[18].Text = col2;
                    break;
                case "SG1_ROW_ADD_CC":
                    if (col1 == "") return;
                    sg1.Rows[Convert.ToInt32(hf1.Value.Trim())].Cells[10].Text = col1;
                    break;
                case "Rmv":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg1"] = dt;
                        sg1.DataSource = dt;
                        sg1.DataBind();
                        dt.Dispose();
                    }
                    break;
                case "GSTCLASS":
                    if (col1 == "") return;
                    txtGstCode.Text = col1;
                    txtGstName.Text = col2;
                    break;
                case "TAX":
                    if (col1 == "") return;
                    txtTaxCode.Text = col1;
                    txtTaxName.Text = col3;
                    break;
            }
        }
    }
    //----------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        // for doing save action 
        if (hffield.Value == "List")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum");
            fgen.Fn_open_rptlevel("Entry List", frm_qstr);
        }
        else if (hffield.Value == "CMD_REP1")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum");
            fgen.Fn_open_rptlevel("Entry List", frm_qstr);
        }
        else
        {
            col1 = "";
            set_Val();
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                try
                {
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_data();

                    oDS.Dispose();
                    oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    if (edmode.Value == "Y")
                        frm_vnum = txtvchnum.Text.Trim();
                    else
                    {
                        i = 0;
                        do
                        {
                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                            pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname + frm_mbr + frm_vty + frm_vnum + System.DateTime.Now.ToString("dd/MM/yyyy"), frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                            i++;
                        }
                        while (pk_error == "Y");

                    }
                    save_data();

                    if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.Trim() + "'");
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    //fgen.send_mail("Tejaxo ERP","pkgupta@Tejaxo.in","","","ITEWSTAGE",""

                    if (edmode.Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' and type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.ToString().Substring(2, 18) + "'");
                        //fgen.msg("-", "AMSG", "Voucher No." + frm_vnum + "  Updated Successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "OnlyClose();", true);
                    }
                    else
                    {
                        //fgen.msg("-", "AMSG", "Voucher No." + frm_vnum + " Saved Successfully ");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "OnlyClose();", true);
                    }
                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                    col1 = "N";
                }
                catch (Exception ex)
                {
                    btnsave.Disabled = false;
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            }
        }
    }
    //----------------------------------------------------------------------------------------
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
    }
    //----------------------------------------------------------------------------------------
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
    //----------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt == null) create_tab();
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
        sg1_dr["sg1_t19"] = "-";
        sg1_dr["sg1_t20"] = "-";
        sg1_dr["sg1_t21"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    //----------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();

        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;

        sg2_dr["sg2_h1"] = "-";
        sg2_dr["sg2_h2"] = "-";
        sg2_dr["sg2_h3"] = "-";
        sg2_dr["sg2_h4"] = "-";
        sg2_dr["sg2_h5"] = "-";

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
    //----------------------------------------------------------------------------------------
    protected void hptacode_Click(object sender, ImageClickEventArgs e)
    {
        // for popup in header block for item /party 
        hffield.Value = "PARTY_POP";
        make_qry_4_popup();
        if (frm_vty.Substring(0, 1) == "1") fgen.Fn_open_mseek("Select Party Name", frm_qstr);
        else fgen.Fn_open_sseek("Select Party Name", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
    protected void btnh_Click(object sender, EventArgs e)
    {
        // to add row on pressing enter in grid
        ((ImageButton)sg1.Rows[0].FindControl("btnadd")).Focus();
    }
    //----------------------------------------------------------------------------------------
    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    //----------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // for options in GRID add, rmv etc
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "SG1_RMV":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "Rmv";
                    fgen.msg("-", "FMSG", "Are You Sure!! You Want to Remove this Account from list");
                }
                break;
            case "SG1_ROW_ADD":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "Row_Edit";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Code", frm_qstr);
                }
                else
                {
                    hffield.Value = "Row_Add";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Code", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD_ACC":
                if (sg1.Rows[index].Cells[14].Text == "-")
                {
                    fgen.msg("-", "AMSG", "Please Select Account Code");
                    return;
                }
                else
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "SG1_ROW_ADD_ACC";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Rev Code", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD_CC":
                hf1.Value = index.ToString();
                hffield.Value = "SG1_ROW_ADD_CC";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Cost Center", frm_qstr);
                break;
        }
    }
    //----------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // for word wrap in case of large text , makes grid if std size
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < 41; i++)
            {
                if (i < 9 || i > 26 || i == 13 || i == 19)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[i].CssClass = "hidden";
                }
            }
            //sg1.HeaderRow.Cells[sR].Text
            //    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
            //    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);

            sg1.HeaderRow.Cells[10].Text = "Cost Center";
            sg1.HeaderRow.Cells[14].Text = "A/c Code";
            sg1.HeaderRow.Cells[15].Text = "A/c Name";
            sg1.HeaderRow.Cells[17].Text = "Rev Code";
            sg1.HeaderRow.Cells[18].Text = "A/c Name";
            sg1.Columns[18].HeaderStyle.Width = 200;
            sg1.HeaderRow.Cells[20].Text = "Dr. Amt";
            sg1.HeaderRow.Cells[21].Text = "Cr. Amt";
            sg1.HeaderRow.Cells[22].Text = "Bill No.";
            sg1.HeaderRow.Cells[23].Text = "Bill Dt.";
            sg1.HeaderRow.Cells[24].Text = "Remarks";
            sg1.Columns[24].HeaderStyle.Width = 200;
            sg1.HeaderRow.Cells[25].Text = "Tax Code";
            sg1.HeaderRow.Cells[26].Text = "Tax Rate";
        }
    }
    //----------------------------------------------------------------------------------------
    void save_data()
    {
        // to save data into virtual table and then final database    
        //string frm_ent_time = fgen.Fn_curr_dt_time(frm_cocd, frm_qstr);
        string vardate;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");

        int srno = 0;

        if (frm_vty.Substring(0, 1) == "1") { srno = 50; } else srno = 1;
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            drtot = 0; crtot = 0;
            drtot = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
            crtot = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
            if (crtot != 0 || drtot != 0)
            {


                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["srno"] = sg1.Rows[i].Cells[13].Text.Trim();

                oporow["acode"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["rcode"] = sg1.Rows[i].Cells[17].Text.Trim();

                oporow["oscl"] = 0;
                oporow["FCTYPE"] = 0;
                oporow["TFCR"] = 1;

                oporow["invno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
                oporow["invdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text, DateTime.Now.ToString("dd/MM/yyyy"));

                oporow["dramt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
                oporow["tfcdr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);

                oporow["tfccr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
                oporow["Cramt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);

                oporow["fcdramt"] = 0;
                oporow["fccramt"] = 0;
                oporow["fcrate1"] = 0;

                oporow["naration"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();

                oporow["tax"] = txtTaxCode.Text;

                oporow["stax"] = 0;
                oporow["post"] = 0;
                oporow["projcd"] = txtSTQty.Text;
                oporow["ccent"] = sg1.Rows[i].Cells[10].Text.Trim();
                oporow["st_entform"] = txtSt38.Text.Trim().ToUpper();
                //oporow["fcrate"] = sg1.Rows[i].Cells[5].Text.Trim();
                //oporow["fcrate1"] = sg1.Rows[i].Cells[6].Text.Trim();

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;

                oporow["mrnnum"] = txtMRRNo.Text.Trim();
                oporow["mrndate"] = txtMRRDt.Text.Trim();

                oporow["refnum"] = txtRefNo.Text.Trim();
                oporow["refdate"] = txtRefDt.Text.Trim();

                oporow["depcd"] = txtGstCode.Text.Trim();

                //oporow["bank_Date"] = null;
                //oporow["app_Date"] = System.DateTime.Now;

                oporow["quantity"] = 0;
                oporow["fc_rate"] = 0;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_date"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_date"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
        }
        //*********************************************************************************                
    }
    //----------------------------------------------------------------------------------------
    void editVoucher()
    {
        #region Edit Voucher
        clearctrl();
        set_Val();
        SQuery = "select a.* from voucher a  where a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' order by a.srno";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        popselected.Value = col1;
        // Filing textbox of the form
        if (dt.Rows.Count > 0)
        {
            frm_vty = dt.Rows[0]["type"].ToString().Trim();
            frm_mbr = dt.Rows[0]["branchcd"].ToString().Trim();

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_MBR", frm_mbr);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

            txtGstCode.Text = dt.Rows[0]["depcd"].ToString().Trim();
            txtGstName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='}' AND TYPE1='" + txtGstCode.Text.Trim().ToUpper() + "'", "NAME");

            txtRefNo.Text = dt.Rows[0]["refnum"].ToString().Trim();
            txtRefDt.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["refdate"].ToString().Trim(), DateTime.Now.ToString("dd/MM/yyyy"))).ToString("dd/MM/yyyy");

            txtMRRNo.Text = dt.Rows[0]["mrnnum"].ToString().Trim();
            txtMRRDt.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["mrndate"].ToString().Trim(), DateTime.Now.ToString("dd/MM/yyyy"))).ToString("dd/MM/yyyy");

            txtTaxCode.Text = dt.Rows[0]["tax"].ToString().Trim();
            txtTaxName.Text = "";

            ViewState["ent_by"] = dt.Rows[0]["ent_by"].ToString().Trim();
            ViewState["ent_dt"] = dt.Rows[0]["ent_date"].ToString().Trim();

            if (dt.Rows[0]["edt_by"].ToString().Trim().Length > 1)
            {
                mq0 = " , " + "Edited By : " + dt.Rows[0]["edt_by"].ToString().Trim() + " Dt. " + dt.Rows[0]["edt_date"].ToString() + "";
            }
            lblEdtBy.InnerText = "Entered By : " + dt.Rows[0]["ent_by"].ToString().Trim() + " Dt. " + dt.Rows[0]["ent_date"].ToString() + mq0;
            txtSTQty.Text = (fgen.make_double(dt.Rows[0]["projcd"].ToString().Trim())).ToString();

            txtremarks.Text = dt.Rows[0]["naration"].ToString().Trim();
            txtSt38.Text = dt.Rows[0]["st_entform"].ToString().Trim();

            create_tab();
            foreach (DataRow dr in dt.Rows)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_srno"] = dr["srno"];
                sg1_dr["sg1_h10"] = dr["ccent"].ToString().Trim();
                sg1_dr["sg1_f1"] = dr["acode"].ToString().Trim();
                sg1_dr["sg1_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(ACODE)='" + dr["acode"].ToString().Trim() + "'", "ANAME").Trim();
                sg1_dr["sg1_f3"] = dr["rcode"].ToString().Trim();
                sg1_dr["sg1_f4"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(ACODE)='" + dr["rcode"].ToString().Trim() + "'", "ANAME").Trim();
                sg1_dr["sg1_t1"] = dr["dramt"].ToString().Trim();
                sg1_dr["sg1_t2"] = dr["cramt"].ToString().Trim();
                sg1_dr["sg1_t3"] = dr["invno"].ToString().Trim();
                sg1_dr["sg1_t4"] = Convert.ToDateTime(dr["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                sg1_dr["sg1_t5"] = dr["naration"].ToString().Trim();
                sg1_dr["sg1_t6"] = dr["tax"].ToString().Trim();
                sg1_dt.Rows.Add(sg1_dr);
            }

            ViewState["sg1"] = sg1_dt;
            sg1_add_blankrows();

            sg1.DataSource = sg1_dt;
            sg1.DataBind();

            edmode.Value = "Y";
            disablectrl();
            fgen.EnableForm(this.Controls);
        }
        if (frm_vty.Trim() == "58" || frm_vty.Trim() == "59")
        {
            btnsave.Disabled = true;
        }
        else
        {
            btnsave.Disabled = false;
        }
        #endregion
    }
    //----------------------------------------------------------------------------------------
    protected void btnGstClass_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GSTCLASS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
    protected void btnTax_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TAX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        string filePath = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT msgtxt AS VAL FROM ATCHVCH WHERE branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value + "'", "VAL");
        if (filePath.Length > 2)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Tejaxo Viewer');", true);
        }
        else fgen.msg("-", "AMSG", "No File Attached!!");
    }
}