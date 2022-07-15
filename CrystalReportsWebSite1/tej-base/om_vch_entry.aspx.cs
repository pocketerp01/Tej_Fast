using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_vch_entry : System.Web.UI.Page
{
    DataTable dt, dt1;
    DataRow dr1, oporow;
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

            btnnew.Focus();
            btnedit.Visible = false;
            txtbalamt.ReadOnly = true;
            fill_Date = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                set_Val();
                getColHeading();
            }
            cal();
            //if (sg1.Rows.Count > 1) myfun();            
            txtvchdate.Attributes.Add("onkeypress", "return clickEnter('" + btnacode.ClientID + "', event)");
            setColHeadings();
        }
    }
    //----------------------------------------------------------------------------------------
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F70101":
            case "F70106":
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;

                break;
        }
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
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
        add_blankrows();
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
        lblheader.Text = "Voucher Entry";
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
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.vCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.ACODE,A.TYPE,a.invno as billno,to_Char(A.invdate,'dd/mm/yyyy') as billdt,b.aname,a.mode_Tpt,a.refnum,b.staten from ivoucher a,famst b where TRIM(A.ACODE)=TRIM(b.ACODe) AND A.branchcd='" + frm_mbr + "' AND A.TYPE LIKE '0%' AND A.VCHDATE " + DateRange + " ORDER BY A.VCHNUM ";
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
                else col2 = " ('')";
                SQuery = "select icode as fstr,iname as product,icode as code from item where icode like '1%' and length(trim(icode))>4 order by icode";
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
                    case "F70116":
                        SQuery = "Select type1 as fstr,Name,Type1 as Code,Acode as Account,Addr1 as Users From Type where id='V' and (substr(type1,1,1)='5') order by type1";
                        break;
                }

                break;
        }

        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
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
        calc();
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

        if (txttrefnum.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Chq No.";
        }

        if (txtchqdt.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Chq Dt.";

        }


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        btnsave.Disabled = true;
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
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
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
        add_blankrows();
        sg1.DataSource = dt1;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; dt1.Dispose();
        ViewState["sg1"] = null;
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
                        lblrcode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        lblrname.Text = dt.Rows[0]["aname"].ToString().Trim();
                    }
                    disablectrl(); fgen.EnableForm(this.Controls);
                    btnacode.Focus();
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
                    popselected.Value = col1;
                    SQuery = "select a.*,b.aname,c.iname,c.cpartno,c.unit from voucher a,famst b,item c where trim(a.acodE)=trim(B.acodE) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' order by a.morder ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    // Filing textbox of the form
                    txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdate.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                    txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();

                    //txtdrvname.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                    //txttrefnum.Text = dt.Rows[0]["invno"].ToString().Trim(); txtbillamount.Text = dt.Rows[0]["invdate"].ToString().Trim();
                    //txttamt.Text = dt.Rows[0]["refnum"].ToString().Trim(); txtbillamount.Text = dt.Rows[0]["refdate"].ToString().Trim();
                    create_tab();
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr1 = dt1.NewRow();
                        dr1["srno"] = dr["srno"];
                        dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                        dr1["unit"] = dt.Rows[i]["unit"].ToString().Trim();
                        dr1["poqty"] = "0";
                        dr1["chlqty"] = dt.Rows[i]["iqty_chl"].ToString().Trim(); ;
                        dr1["chlwt"] = dt.Rows[i]["Iqty_chlwt"].ToString().Trim(); ;
                        dr1["size"] = dt.Rows[i]["psize"].ToString().Trim();
                        dr1["gsm"] = dt.Rows[i]["gsm"].ToString().Trim();
                        dr1["rate"] = dt.Rows[i]["irate"].ToString().Trim();
                        dr1["rmk"] = "-";
                        dt1.Rows.Add(dr1);
                    }
                    break;
                case "PARTY_POP":
                    if (col1.Length < 2) return;
                    frm_vty = popselected.Value;

                    //txtacode.Text = col1;
                    //txtaname.Text = col2;

                    SQuery = "SELECT * FROM IVOUCHER WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + col1 + "' ORDER BY SRNO";

                    dt = new DataTable();
                    dt1 = new DataTable();
                    create_tab();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtacode.Text = dt.Rows[0]["acode"].ToString();
                        txtaname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE='" + dt.Rows[0]["acode"].ToString() + "'", "ANAME");

                        txttrefnum.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                        txtchqdt.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("yyy-MM-dd");

                        txtmrrno.Text = dt.Rows[0]["VCHNUM"].ToString().Trim();
                        txtmrrdt.Text = Convert.ToDateTime(dt.Rows[0]["VCHDATE"].ToString().Trim()).ToString("yyy-MM-dd");
                    }
                    double cum_bal;
                    cum_bal = 0;
                    //foreach (DataRow dr in dt.Rows)
                    {
                        dr1 = dt1.NewRow();
                        dr1["acode"] = "300007";
                        dr1["invno"] = dt.Rows[0]["invno"];
                        dr1["invdate"] = Convert.ToDateTime(dt.Rows[0]["invdate"]).ToString("dd/MM/yyyy");
                        dr1["camt"] = 0;
                        dr1["damt"] = 0;
                        dr1["net"] = 0;
                        dr1["passamt"] = "0";
                        cum_bal = cum_bal + Math.Round(Convert.ToDouble(0), 2);
                        dr1["cumbal"] = cum_bal;
                        dr1["manualamt"] = "0";


                        //Math.Round(Convert.ToDouble(dr["Dramt"]) - Convert.ToDouble(dr["cramt"]), 2).ToString();

                        dr1["rmk"] = "-";
                        dt1.Rows.Add(dr1);

                        dr1 = dt1.NewRow();
                        dr1["acode"] = "17V001";
                        dr1["invno"] = dt.Rows[0]["invno"];
                        dr1["invdate"] = Convert.ToDateTime(dt.Rows[0]["invdate"]).ToString("dd/MM/yyyy");
                        dr1["camt"] = 0;
                        dr1["damt"] = 0;
                        dr1["net"] = 0;
                        dr1["passamt"] = "0";
                        cum_bal = cum_bal + Math.Round(Convert.ToDouble(0), 2);
                        dr1["cumbal"] = cum_bal;
                        dr1["manualamt"] = "0";


                        //Math.Round(Convert.ToDouble(dr["Dramt"]) - Convert.ToDouble(dr["cramt"]), 2).ToString();

                        dr1["rmk"] = "-";
                        dt1.Rows.Add(dr1);
                    }
                    add_blankrows();
                    sg1.DataSource = dt1; sg1.DataBind(); ViewState["sg1"] = dt1;
                    //myfun();
                    break;
                case "PARTY_POP2":
                    txtothac.Text = col1; txtothname.Text = col2;
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
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        dt1 = dt.Clone();
                        dr1 = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dt1.Rows.Count + 1;
                            dr1["invno"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["invdate"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["camt"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            dr1["damt"] = dt.Rows[i]["unit"].ToString().Trim();
                            dr1["net"] = "0";
                            dr1["passamt"] = "0";
                            dr1["cumbal"] = "0";
                            dr1["manualamt"] = "0";
                            dr1["rmk"] = "-";
                            dt1.Rows.Add(dr1);
                        }
                        if (col1.Trim().Length == 8) SQuery = "select distinct icode,iname,cpartno,unit from item where trim(icode) in ('" + col1 + "')";
                        else SQuery = "select distinct icode,iname,cpartno,unit from item where trim(icode) in (" + col1 + ")";

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dt1.Rows.Count + 1;
                            dr1["invno"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["invdate"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["camt"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            dr1["damt"] = dt.Rows[i]["unit"].ToString().Trim();
                            dr1["net"] = "0";
                            dr1["passamt"] = "0";
                            dr1["cumbal"] = "0";
                            dr1["manualamt"] = "0";
                            dr1["rmk"] = "-";

                            dt1.Rows.Add(dr1);
                        }
                    }
                    add_blankrows();

                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    dt.Dispose(); dt1.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("txtchlqty")).Focus();
                    break;
                case "Row_Edit":
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select icode,iname,cpartno,unit from item where trim(icodE)='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = dt.Rows[0]["iname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = dt.Rows[0]["unit"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtchlqty")).Focus();
                    }
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
            if (col1 == "N")
            {
                btnsave.Disabled = false;
            }
            else
            {
                try
                {
                    //myfun();
                    calc();
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
                        fgen.msg("-", "AMSG", "Voucher No." + frm_vnum + "  Updated Successfully");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' and type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.ToString().Substring(2, 18) + "'");
                    }
                    else { fgen.msg("-", "AMSG", "Voucher No." + frm_vnum + " Saved Successfully "); }
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
        // for making a table structure
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("srno", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("acode", typeof(string)));
        dt1.Columns.Add(new DataColumn("Invno", typeof(string)));
        dt1.Columns.Add(new DataColumn("invdate", typeof(string)));
        dt1.Columns.Add(new DataColumn("camt", typeof(string)));
        dt1.Columns.Add(new DataColumn("damt", typeof(string)));
        dt1.Columns.Add(new DataColumn("net", typeof(string)));
        dt1.Columns.Add(new DataColumn("passamt", typeof(string)));
        dt1.Columns.Add(new DataColumn("manualamt", typeof(string)));
        dt1.Columns.Add(new DataColumn("cumbal", typeof(string)));
        dt1.Columns.Add(new DataColumn("rmk", typeof(string)));
    }
    //----------------------------------------------------------------------------------------

    public void add_blankrows()
    {
        // for making a blank table row 
        dr1 = dt1.NewRow();
        dr1["acode"] = "-";
        dr1["invno"] = "-";
        dr1["invdate"] = "-";
        dr1["camt"] = "0";
        dr1["damt"] = "0";
        dr1["net"] = "0";
        dr1["passamt"] = "0";
        dr1["manualamt"] = "0";
        dr1["cumbal"] = "0";
        dr1["rmk"] = "-";
        dt1.Rows.Add(dr1);
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
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // for options in GRID add, rmv etc
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "Rmv":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "Rmv";
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove this item from list");
                }
                break;
            case "Row_Add":
                if (txtacode.Text == "" || txtacode.Text == "0")
                    fgen.msg("-", "AMSG", "First Please Select the Item!!");
                else
                {
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        hffield.Value = "Row_Edit";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                    }
                    else
                    {
                        hffield.Value = "Row_Add";
                        make_qry_4_popup();
                        fgen.Fn_open_mseek("Select Your Product(s)", frm_qstr);
                    }
                    this.cal();
                }
                break;
        }
    }
    //----------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // for word wrap in case of large text , makes grid if std size
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[4].Attributes.Add("style", "white-space: nowrap;");
            //ViewState["OrigData"] = e.Row.Cells[4].Text;
            //if (e.Row.Cells[4].Text.Length >= 25) //Just change the value of 30 based on your requirements
            //{
            //    e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 25) + "...";
            //    e.Row.Cells[4].ToolTip = ViewState["OrigData"].ToString();
            //}

            e.Row.Cells[0].Style["display"] = "none";
            sg1.HeaderRow.Cells[0].Style["display"] = "none";
        }
    }
    //----------------------------------------------------------------------------------------
    void cal()
    {
        // for calculation in grid
        double vp = 0;
        for (int zk = 0; zk < sg1.Rows.Count - 1; zk++)
        {
            //vp1 = Convert.ToDouble(((TextBox)sg1.Rows[zk].FindControl("txtfld1")).Text.Trim());
            //vp += vp1;
        }
        lblqtysum.InnerHtml = vp.ToString();
    }
    //----------------------------------------------------------------------------------------
    public void myfun()
    {
        vip = ""; mq1 = "ContentPlaceHolder1_";
        vip = vip + "<script type='text/javascript'>function calculateSum() {";
        vip = vip + "var vp=0;var vp1=0; var fill_amt=0;";
        mq0 = "";
        for (int zk = 0; zk < sg1.Rows.Count; zk++)
        {
            vip = vip + "var chk_result" + zk + " = document.getElementById('ContentPlaceHolder1_sg1_chk1_" + zk + "').checked;";
            vip = vip + "if(chk_result" + zk + "==true) { document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value= fill_zero('" + sg1.Rows[zk].Cells[7].Text.Trim() + "'); ";

            vip = vip + " if(fill_zero(document.getElementById('ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "').value) > 0) { fill_amt = document.getElementById('ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "').value; }";
            vip = vip + " else { fill_amt = document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value; }";

            vip = vip + "}";
            vip = vip + "else { document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value= 0; fill_amt = 0; }";

            vip = vip + "vp=(vp*1) + (document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value * 1);";

            vip = vip + "vp1=(vp1*1) + (fill_amt * 1);";

            //if ((i + 1) < sg1.Rows.Count)
            //    ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Attributes.Add("onkeypress", "return clickEnter('" + ((CheckBox)sg1.Rows[i + 1].FindControl("chk1")).ClientID + "', event)");
        }

        vip = vip + "document.getElementById('ContentPlaceHolder1_lblqtysum').innerHTML = vp; ";
        vip = vip + "document.getElementById('ContentPlaceHolder1_txtbillamount').value = vp1; ";
        vip = vip + "document.getElementById('ContentPlaceHolder1_txtbalamt').value = fill_zero(document.getElementById('ContentPlaceHolder1_txtbillamount').value) - fill_zero(document.getElementById('ContentPlaceHolder1_txttamt').value); ";
        vip = vip + "}";
        vip = vip + "function fill_zero(val){ if(isNaN(val)) return 0; if(isFinite(val)) return val; }</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", vip.ToString(), false);
    }
    //(fill_zero(t1) + fill_zero(t2) + fill_zero(t3)).toFixed(3);
    //vip = vip + "document.getElementById('ContentPlaceHolder1_txtbalamt').value = fill_zero(document.getElementById('ContentPlaceHolder1_txtbillamount').value) - fill_zero(document.getElementById('ContentPlaceHolder1_txttamt').value); ";
    //----------------------------------------------------------------------------------------
    public void calc()
    {
        double double_Vp1 = 0, fill_amt = 0;

        for (int zk = 0; zk < sg1.Rows.Count; zk++)
        {
            CheckBox chk1 = ((CheckBox)sg1.Rows[zk].FindControl("chk1"));
            if (chk1.Checked == true)
            {
                ((TextBox)sg1.Rows[zk].FindControl("txtpassfor")).Text = sg1.Rows[zk].Cells[7].Text.Trim();
                if (fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text) > 0) fill_amt = fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text);
                else fill_amt = fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtpassfor")).Text);
            }
            else
            {
                fill_amt = 0;
                ((TextBox)sg1.Rows[zk].FindControl("txtpassfor")).Text = "0";
            }
            double_Vp1 += fill_amt;
        }

        //txtbillamount.Text = double_Vp1.ToString();
        //txtbalamt.Text = ((fgen.make_double(txtbillamount.Text) * 1) - (fgen.make_double(txttamt.Text) * 1)).ToString();
    }
    //----------------------------------------------------------------------------------------
    protected void btnh_Click(object sender, EventArgs e)
    {
        // to add row on pressing enter in grid
        ((ImageButton)sg1.Rows[0].FindControl("btnadd")).Focus();
    }
    //----------------------------------------------------------------------------------------
    void save_data()
    {
        // to save data into virtual table and then final database    
        //string frm_ent_time = fgen.Fn_curr_dt_time(frm_cocd, frm_qstr);
        string vardate;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        int srno = 0; double largest_value_amt = 0, to_fill_amt = 0; string largest_value_acode = "";
        if (frm_vty.Substring(0, 1) == "1") { srno = 50; } else srno = 1;
        for (i = 0; i <= sg1.Rows.Count - 1; i++)
        {
            if (Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim()) > 0)
            {
                to_fill_amt = 0;

                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["ACODE"] = sg1.Rows[i].Cells[2].Text.Trim();
                oporow["rCODE"] = lblrcode.Text.Trim();

                if (largest_value_amt > 0)
                {
                    if (Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim()) > largest_value_amt)
                    {
                        largest_value_amt = Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim());
                        largest_value_acode = sg1.Rows[i].Cells[2].Text.Trim();
                    }
                }
                else
                {
                    largest_value_amt = Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim());
                    largest_value_acode = sg1.Rows[i].Cells[2].Text.Trim();
                }

                oporow["srno"] = srno;

                oporow["oscl"] = 0;
                oporow["FCTYPE"] = 0;
                oporow["TFCR"] = 1;

                oporow["invno"] = sg1.Rows[i].Cells[3].Text.Trim();
                oporow["invdate"] = sg1.Rows[i].Cells[4].Text.Trim();
                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                oporow["quantity"] = 0;

                if (Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim()) > 0) to_fill_amt = Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim());
                else to_fill_amt = Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim());

                //to_fill_amt = 2;

                if (frm_vty.Substring(0, 1) == "1")
                {
                    oporow["dramt"] = 0;
                    oporow["tfcdr"] = 0;

                    oporow["tfccr"] = to_fill_amt;
                    oporow["Cramt"] = to_fill_amt;
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = 0;
                    oporow["fcrate1"] = 0;
                }
                else
                {
                    oporow["dramt"] = to_fill_amt;
                    oporow["tfcdr"] = to_fill_amt;

                    oporow["tfccr"] = 0;
                    oporow["Cramt"] = 0;
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = 0;
                    oporow["fcrate1"] = 0;
                }

                oporow["naration"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim();
                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;
                oporow["fcrate"] = sg1.Rows[i].Cells[5].Text.Trim();
                oporow["fcrate1"] = sg1.Rows[i].Cells[6].Text.Trim();

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                //oporow["bank_Date"] = null;
                //oporow["app_Date"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_date"] = ViewState["ent_Dt"].ToString();
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

        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = txtvchdate.Text.Trim();

        oporow["rCODE"] = largest_value_acode;
        oporow["ACODE"] = lblrcode.Text.Trim();

        if (srno > 50) srno = 1; else srno = 50;

        oporow["srno"] = srno;

        oporow["FCTYPE"] = 0;
        oporow["TFCR"] = 1;
        oporow["oscl"] = 0;

        oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
        oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
        oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
        oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

        oporow["quantity"] = 0;

        if (frm_vty.Substring(0, 1) == "1")
        {
            //oporow["dramt"] = txtbillamount.Text.Trim();
            oporow["tfccr"] = 0;
            //oporow["tfcdr"] = txtbillamount.Text.Trim();
            oporow["Cramt"] = 0;
            oporow["fcdramt"] = 0;
            oporow["fccramt"] = 0;
        }
        else
        {
            oporow["dramt"] = 0;
            //oporow["tfccr"] = txtbillamount.Text.Trim();
            oporow["tfcdr"] = 0;
            //oporow["Cramt"] = txtbillamount.Text.Trim();
            oporow["fcdramt"] = 0;
            oporow["fccramt"] = 0;
        }

        oporow["naration"] = "-";

        oporow["tax"] = "-";
        oporow["stax"] = 0;
        oporow["post"] = 0;
        oporow["fcrate"] = 0;
        oporow["fcrate1"] = 0;

        oporow["grno"] = "-";
        oporow["grdate"] = vardate;
        oporow["mrndate"] = vardate;

        //oporow["bank_Date"] = null;
        //oporow["app_Date"] = System.DateTime.Now;

        oporow["mrndate"] = System.DateTime.Now;

        if (edmode.Value == "Y")
        {
            oporow["ent_by"] = ViewState["ent_by"].ToString();
            oporow["ent_dAtE"] = ViewState["ent_Dt"].ToString();
            oporow["edt_by"] = frm_uname;
            oporow["edt_dAtE"] = vardate;
        }
        else
        {
            oporow["ent_by"] = frm_uname;
            oporow["ent_date"] = vardate;
            oporow["edt_by"] = "-";
            oporow["edt_date"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
        //*********************************************************************************
        if (txtothac.Text.Trim().Length > 2 && txtothamt.Text != "0")
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["rCODE"] = lblrcode.Text.Trim();
            oporow["ACODE"] = largest_value_acode;

            oporow["srno"] = 100;

            oporow["FCTYPE"] = 0;
            oporow["TFCR"] = 1;
            oporow["oscl"] = 0;

            oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

            oporow["quantity"] = 0;

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["dramt"] = txtothamt.Text.Trim();
                oporow["tfccr"] = 0;
                oporow["tfcdr"] = txtothamt.Text.Trim();
                oporow["Cramt"] = 0;
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = 0;
            }
            else
            {
                oporow["dramt"] = 0;
                oporow["tfccr"] = txtothamt.Text.Trim();
                oporow["tfcdr"] = 0;
                oporow["Cramt"] = txtothamt.Text.Trim();
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = 0;
            }

            oporow["naration"] = "-";

            oporow["tax"] = "-";
            oporow["stax"] = 0;
            oporow["post"] = 0;
            oporow["fcrate"] = 0;
            oporow["fcrate1"] = 0;

            oporow["grno"] = "-";
            oporow["grdate"] = vardate;
            oporow["mrndate"] = vardate;

            //oporow["bank_Date"] = null;
            //oporow["app_Date"] = System.DateTime.Now;


            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["ent_by"].ToString();
                oporow["ent_dAtE"] = ViewState["ent_Dt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dAtE"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_date"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);

            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["rCODE"] = largest_value_acode;
            oporow["ACODE"] = lblrcode.Text.Trim();

            oporow["srno"] = 102;

            oporow["FCTYPE"] = 0;
            oporow["TFCR"] = 1;
            oporow["oscl"] = 0;

            oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

            oporow["quantity"] = 0;

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["dramt"] = 0;
                oporow["tfccr"] = txtothamt.Text.Trim(); ;
                oporow["tfcdr"] = 0;
                oporow["Cramt"] = txtothamt.Text.Trim();
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = 0;
            }
            else
            {
                oporow["dramt"] = txtothamt.Text.Trim();
                oporow["tfccr"] = 0;
                oporow["tfcdr"] = txtothamt.Text.Trim();
                oporow["Cramt"] = 0;
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = 0;
            }

            oporow["naration"] = "-";

            oporow["tax"] = "-";
            oporow["stax"] = 0;
            oporow["post"] = 0;
            oporow["fcrate"] = 0;
            oporow["fcrate1"] = 0;

            oporow["grno"] = "-";
            oporow["grdate"] = vardate;
            oporow["mrndate"] = vardate;

            //oporow["bank_Date"] = null;
            //oporow["app_Date"] = System.DateTime.Now;

            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["ent_by"].ToString();
                oporow["ent_dAtE"] = ViewState["ent_Dt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dAtE"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_date"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //-------------------------------------------------------
    protected void btnacode_Click(object sender, ImageClickEventArgs e)
    {
        m1 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R01' and enable_yn='Y' ", "params");
        if (m1 != "0")
        {
            eff_Dt = " vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') ";
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdata as(select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdataml as(select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO ");
        }
        else
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdata as(SELECT * FROM (select branchcd,ACODE,INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD'  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,ACODE,INVNO,INVDATE )  WHERE NET<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO  ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdataml as(SELECT * FROM (select branchcd,ACODE,INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD'  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,ACODE,INVNO,INVDATE )  WHERE NET<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO  ");
        }
        hffield.Value = "PARTY_POP";
        make_qry_4_popup();
        if (frm_vty.Substring(0, 1) == "1") fgen.Fn_open_mseek("Select Party", frm_qstr);
        else fgen.Fn_open_sseek("Select Party", frm_qstr);
    }
    //-------------------------------------------------------
    protected void btnothrac_Click(object sender, ImageClickEventArgs e)
    {
        // for edit button popup                
        hffield.Value = "PARTY_POP2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Other Link Code", frm_qstr);
    }
    //-------------------------------------------------------
    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event); ";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
}