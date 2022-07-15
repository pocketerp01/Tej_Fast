using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;

public partial class om_csbs_est : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, SQuery5, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4, dt6, dt7, dt8; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z;

    DataTable sg1_dt; DataRow sg1_dr;

    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string a, b, c;
    fgenDB fgen = new fgenDB();
    string SQuery4, SQuery3;


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
                    lbl1a_Text = "CS";
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            txt_gross_unit.Value = "kg";
            txt_stckhgt_unit.Value = "mm";
            txt_box_stckd_unit.Value = "Nos";
            txt_load_unit.Value = "Kg";
            txtdepfacc.Value = "L/W Fac";

            if (!Page.IsPostBack)
            {
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                lblheader.Text = "CS-BS Estimation";
                fgen.DisableForm(this.Controls);
                enablectrl();
                bindflute();// filling drop down values  
            }
            set_Val();

            if (frm_ulvl != "0")
            {
                btndel.Visible = false;
            }
            if (CSR.Length > 1 || frm_ulvl == "3")
            {

            }

        }

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btncal.Enabled = false; btndel.Disabled = false;
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
        btncal.Enabled = true;
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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "wb_corrcst_csbs";
        frm_vty = "^1";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        typePopup = "N";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", frm_tabname1);
        // fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", frm_tabname2);


    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        // frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        // frm_tabname2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");

        btnval = hffield.Value;
        if (frm_ulvl == "3") cond = " and trim(a.ENT_BY)='" + frm_uname + "'";
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR.Trim() + "'";
        switch (btnval)
        {
            case "MGRBUT":
                SQuery = "select ANAME as Name,Acode as ERP_Code,Addr1 as Address,Addr2 as City from famst where substr(acode,1,2) in ('02')  order by aname";

                break;
            case "PLYBUT":
                SQuery = "select '3' as fstr,'3' as PLY1,'3' as PLY  from dual union all select '5' as fstr,'5' as PLY1,'5' as PLY  from dual ";
                break;
            case "STATBUT":
                SQuery = "select name as fstr ,name as State_Name ,type1 as code from type where id='{' order by Name";
                break;
            case "COSTBUT":
                SQuery = "Select Name,Type1 from typegrp where id='C' and length(Trim(type1))=3 order by type1 ";
                break;
            case "CTRYBUT":
                SQuery = "select name as fstr,name as Country ,type1 as code from typegrp where branchcd!='DD' and id='CN' order by name ";
                break;
            case "BNKACTBUT":
                SQuery = "Select Aname,ACode from Famst order by Acode1";
                break;
            case "IVLBUT":
                SQuery = "Select Name,Type1 from typegrp where id='C' and type1 like '-%' and length(Trim(type1))=5 order by type1";
                break;
            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E")
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                SQuery = "SELECT DISTINCT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,VCHNUM AS ENTRY_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,custname as cust_name , itemname as item_name FROM wb_corrcst_csbs WHERE BRANCHCD='" + frm_mbr + "' ORDER BY VCHNUM DESC ";

                if (btnval == "Print_E")
                    SQuery = "select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,trim(vchnum) as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(custname) as  customer,trim(itemname) as item from wb_corrcst_csbs where branchcd='" + frm_mbr + "' and type='^1' and vchdate " + DateRange + " order by vchnum desc";
                break;
        }
        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" | btnval == "Print"))
        {
            btnval = btnval + "_E";
            hffield.Value = btnval;
            make_qry_4_popup();
        }
        else
            if (SQuery.Length > 0)
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
                newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }

            if (frm_ulvl == "3")
            {

            }

        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;
        string mq1 = "";
        mq1 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE  branchcd='" + frm_mbr + "'";
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq1, 6, "VCH");
        txtVchnum.Value = frm_vnum;
        // txtVchnum.Value = fgen.next_no(frm_qstr, frm_cocd, "select max(code) as vch from " + frm_tabname + "  WHERE  branchcd='" + frm_mbr + "'", 6, "VCH");
        txtVchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

        if (Convert.ToDateTime(txtVchdate.Value) > Convert.ToDateTime(todt))
        {
            txtVchdate.Value = todt;
        }

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //fillindex();
        bindflute();// filling drop down values
        btn_img.ImageUrl = "";
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

        fgen.fill_dash(this.Controls);


        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (txtCustomer.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Customer";

        }
        if (txtItem.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "ItemName";

        }
        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
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
        btn_img.ImageUrl = "";
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
        fgen.Fn_open_sseek("Select " + lblheader.Text + "  for Print", frm_qstr);
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
        //   frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        //  frm_tabname2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info_mac(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
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
                    newCase(col1);
                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "CS";
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "CS";
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

                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    string mv_col, mv_col1, mv_col3;
                    mv_col = col1;
                    mv_col3 = frm_mbr + col1;
                    mv_col1 = col2;

                    SQuery = "SELECT A.* FROM " + frm_tabname + " A WHERE  TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + mv_col + "'";


                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtVchdate.Value = Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString()).ToString("dd/MM/yyyy");
                        txtVchnum.Value = dt.Rows[0]["vchnum"].ToString();
                        txtCustomer.Value = dt.Rows[i]["custname"].ToString();
                        txtItem.Value = dt.Rows[i]["itemname"].ToString();
                        txt_gross.Value = dt.Rows[i]["grosswt"].ToString();
                        txt_stckhgt.Value = dt.Rows[i]["stckhgt"].ToString();
                        txt_box_stckd.Value = dt.Rows[i]["noofboxes"].ToString();
                        txt_load.Value = dt.Rows[i]["loadbox"].ToString();
                        txt_storagea.Value = dt.Rows[i]["storageTM_Days"].ToString();
                        txt_strorageb.Value = dt.Rows[i]["StorageTM_val"].ToString();
                        txt_humida.Value = dt.Rows[i]["Humidpercnt"].ToString();
                        txt_humidb.Value = dt.Rows[i]["humidvalue"].ToString();
                        txt_columna.Value = dt.Rows[i]["columnalgnd"].ToString();
                        txt_columnb.Value = dt.Rows[i]["columnmisalgnd"].ToString();
                        txt_interlock.Value = dt.Rows[i]["interlockd"].ToString();
                        txt_overhng.Value = dt.Rows[i]["overhanged"].ToString();
                        txtdeckboard.Value = dt.Rows[i]["deckboard_gap"].ToString();
                        txtexchand.Value = dt.Rows[i]["exceshnd"].ToString();
                        txt_total_envir_facb.Value = dt.Rows[i]["tot_envr_fac"].ToString();
                        ddbox_fef.Value = dt.Rows[i]["boxtypecode"].ToString();
                        // txt_require_safetyb

                        //getting Yes/no Values

                        ddcolumna.Value = dt.Rows[i]["aligyn"].ToString();
                        ddcolumnb.Value = dt.Rows[i]["misaligyn"].ToString();
                        ddinterlock.Value = dt.Rows[i]["interyn"].ToString();
                        ddoverhang.Value = dt.Rows[i]["ovhgyn"].ToString();
                        dddeckboard.Value = dt.Rows[i]["dckgpyn"].ToString();
                        ddexchnd.Value = dt.Rows[i]["exvhdyn"].ToString();
                        txt_require_safetyb.Value = dt.Rows[i]["requiredSfac"].ToString();

                        txt_require_bctb.Value = dt.Rows[i]["REQUIRDBCT"].ToString();
                        txt_len.Value = dt.Rows[i]["length"].ToString();
                        txt_wid.Value = dt.Rows[i]["width"].ToString();
                        txt_hgt.Value = dt.Rows[i]["height"].ToString();
                        ddno_piles.Value = dt.Rows[i]["no_of_plies"].ToString();

                        ddflutep.Value = dt.Rows[i]["flute_profile"].ToString();
                        ddManf_Procs.Value = dt.Rows[i]["manf_proces"].ToString();
                        txt_board_callipr.Value = dt.Rows[i]["BOARD_CALLIPR"].ToString();


                        txt_area.Value = dt.Rows[i]["area"].ToString();
                        txt_req_ect.Value = dt.Rows[i]["req_ect"].ToString();
                        txt_req_rct.Value = dt.Rows[i]["req_rct"].ToString();
                        txt_topply.Value = dt.Rows[i]["top_perc"].ToString();
                        txt_linerply.Value = dt.Rows[i]["liner_perc"].ToString();
                        txt_fluteply.Value = dt.Rows[i]["flute_perc"].ToString();
                        txt_toplinera.Value = dt.Rows[i]["topliner_rct"].ToString();
                        txtflute1a.Value = dt.Rows[i]["flute1_rct"].ToString();
                        txtmidlinera.Value = dt.Rows[i]["midliner_rct"].ToString();
                        txtflute2a.Value = dt.Rows[i]["flute2_rct"].ToString();
                        txtinlinera.Value = dt.Rows[i]["innerliner_rct"].ToString();
                        txtdepfaca.Value = dt.Rows[i]["dep_fac_rct"].ToString();
                        ddtoplinerb.Value = dt.Rows[i]["Topliner_bf"].ToString();
                        ddflute1b.Value = dt.Rows[i]["flute1_bf"].ToString();
                        ddmidlinerb.Value = dt.Rows[i]["midliner_bf"].ToString();
                        ddflute2b.Value = dt.Rows[i]["flute2_bf"].ToString();

                        ddinlinerb.Value = dt.Rows[i]["innerliner_bf"].ToString();

                        txttoplinerc.Value = dt.Rows[i]["topliner_gsm_bf"].ToString();
                        txtflute1c.Value = dt.Rows[i]["flute1_gsm_bf"].ToString();
                        txtmidlinerc.Value = dt.Rows[i]["midliner_gsm_bf"].ToString();
                        txtflute2c.Value = dt.Rows[i]["flute2_gsm_bf"].ToString();
                        txtinlinerc.Value = dt.Rows[i]["innerliner_gsm_bf"].ToString();


                        ddtoplinerd.Value = dt.Rows[i]["topliner_gsm"].ToString();
                        ddfluted.Value = dt.Rows[i]["flute1_gsm"].ToString();
                        ddmidlinerd.Value = dt.Rows[i]["midliner_gsm"].ToString();
                        ddflute2d.Value = dt.Rows[i]["flute2_gsm"].ToString();


                        ddinlinerd.Value = dt.Rows[i]["innerliner_gsm"].ToString();
                        txtdepfacd.Value = dt.Rows[i]["dep_fac_gsm"].ToString();
                        txttotalgsm.Value = dt.Rows[i]["tot_board_gsm"].ToString();


                        txttotalwght.Value = dt.Rows[i]["tot_wght_car"].ToString();
                        txttotalboardbs.Value = dt.Rows[i]["tot_board_BS"].ToString();
                        txttotalcs.Value = dt.Rows[i]["Tot_CS"].ToString();

                        txttotalrct.Value = dt.Rows[i]["tot_rct_gsm_bf"].ToString();
                        txttotalect.Value = dt.Rows[i]["TOT_ECT_GSM_BF"].ToString();
                        txttotaldiff.Value = dt.Rows[i]["DIFF_REQ_ECT"].ToString();
                    }
                    fgen.EnableForm(this.Controls);
                    disablectrl();
                    edmode.Value = "Y";

                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_engg_reps(frm_qstr);
                    break;

                case "ACNBUT":
                    if (col1.Length <= 0) return;

                    break;

                case "MGRBUT":
                    if (col1.Length <= 0) return;
                    break;

                case "BNKACTBUT":
                    if (col1.Length <= 0) return;

                    break;

                case "PLYBUT":
                    if (col1.Length <= 0) return;


                    break;

                case "STATBUT":
                    if (col1.Length <= 0) return;

                    break;

                case "COSTBUT":
                    if (col1.Length <= 0) return;

                    break;

                case "CTRYBUT":
                    if (col1.Length <= 0) return;

                    break;

                case "IVLBUT":
                    if (col1.Length <= 0) return;

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
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;
                case "SG1_ROW_ADD":

                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }


                    //********* Saving in Hidden Field 
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    ////********* Saving in GridView Value
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    ////((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    //setColHeadings();
                    break;
                case "SG4_ROW_ADD11":
                    break;
                case "SG1_ROW_TAX":

                    break;
                case "SG1_ROW_DT":
                    // ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
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
                        //for (i = 0; i < sg4.Rows.Count - 1; i++)
                        //{
                        //    sg4_dr = sg4_dt.NewRow();
                        //    sg4_dr["sg4_srno"] = (i + 1);

                        //    sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                        //    sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();


                        //    sg4_dt.Rows.Add(sg4_dr);
                        //}

                        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg4_add_blankrows();

                        ViewState["sg4"] = sg4_dt;
                        //sg4.DataSource = sg4_dt;
                        //sg4.DataBind();
                    }
                    #endregion

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
            if (frm_ulvl == "3") cond = " and trim(a.ccode)='" + frm_uname + "'";
            if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
            SQuery = "SELECT a.VCHNUM,to_char(a.VCHDATE,'dd/mm/yyyy') as vchdate,a.CUSTNAME,a.ITEMNAME,a.GROSSWT,a.STCKHGT,a.NOOFBOXES,a.LOADBOX,a.STORAGETM_DAYS,a.STORAGETM_VAL,a.HUMIDPERCNT,a.HUMIDVALUE,a.COLUMNALGND,a.COLUMNMISALGND,a.INTERLOCKD,a.OVERHANGED,a.DECKBOARD_GAP,a.EXCESHND,a.TOT_ENVR_FAC,a.REQUIRDBCT,a.BOXTYPECODE,a.LENGTH,a.WIDTH,a.HEIGHT,a.NO_OF_PLIES,a.FLUTE_PROFILE,a.MANF_PROCES,a.BOARD_CALLIPR,a.AREA,a.REQ_ECT,a.REQ_RCT,a.TOP_PERC,a.LINER_PERC,a.FLUTE_PERC,a.TOPLINER_RCT,a.FLUTE1_RCT,a.MIDLINER_RCT,a.FLUTE2_RCT,a.INNERLINER_RCT,a.DEP_FAC_RCT,a.TOPLINER_BF,a.FLUTE1_BF,a.MIDLINER_BF,a.FLUTE2_BF,a.INNERLINER_BF,a.DEP_FAC_BF,a.TOPLINER_GSM_BF,a.FLUTE1_GSM_BF,a.MIDLINER_GSM_BF,a.FLUTE2_GSM_BF,a.INNERLINER_GSM_BF,a.DEP_FAC_GSM_BF,a.TOPLINER_GSM,a.FLUTE1_GSM,a.MIDLINER_GSM,a.FLUTE2_GSM,a.INNERLINER_GSM,a.DEP_FAC_GSM,a.TOT_BOARD_GSM,a.TOT_WGHT_CAR,a.TOT_BOARD_BS,a.TOT_CS,a.TOT_RCT_GSM_BF,a.TOT_ECT_GSM_BF,a.DIFF_REQ_ECT,a.DESC_,a.COL1,a.REMARKS,a.NARATION,a.REQUIREDSFAC,a.ALIGYN,a.MISALIGYN,a.INTERYN,a.OVHGYN,a.DCKGPYN,a.EXVHDYN,a.ENT_BY,a.ENT_DT,to_char(a.vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHDATE " + PrdRange + " order by vdd DESC,a.vchnum DESC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------


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



                        //oDS5 = new DataSet();
                        //oporow5 = null;
                        //oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        //oDS2 = new DataSet();
                        //oporow2 = null;
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);


                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();


                        //save_fun5();
                        // save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);


                        //oDS5.Dispose();
                        //oporow5 = null;
                        //oDS5 = new DataSet();
                        //oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        //oDS2.Dispose();
                        //oporow2 = null;
                        //oDS2 = new DataSet();
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);


                        if (edmode.Value == "Y")
                        {

                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "Y";


                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        //save_fun5();
                        // save_fun2();

                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld2 = fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                        if (edmode.Value == "Y")
                        {
                            ddl_fld1 = ddl_fld2.Substring(0, 6);

                        }
                        else
                        {
                            ddl_fld1 = ddl_fld2;
                        }

                        if (edmode.Value == "Y")
                        {

                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where trim(vchnum)='" + ddl_fld1 + "'AND branchcd='" + frm_mbr + "'");
                            //  fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname1 + " set trannum='DD' where trim(trannum)='" + frm_mbr + ddl_fld1 + "'");
                            //  fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname2 + " set trannum='DD' where trim(trannum)='" + frm_mbr + ddl_fld1 + "'");
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        // fgen.save_data(frm_cocd, oDS5, frm_tabname1);
                        // fgen.save_data(frm_cocd, oDS2, frm_tabname2);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM  " + frm_tabname + " where branchcd='DD'");
                            // fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM  " + frm_tabname1 + " where trannum='DD'");
                            // fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM  " + frm_tabname2 + " where trannum='DD'");

                        }
                        else
                        {
                            if (save_it == "Y")
                            {


                                fgen.msg("-", "AMSG", " Entry No " + txtVchnum.Value.Trim() + "Saved Successfully");
                                btn_img.ImageUrl = "";

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
            #endregion
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();



    }
    public void create_tab2()
    {


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

    public void create_tab4()
    {
        sg4_dt = new DataTable();
        sg4_dr = null;
        // Hidden Field

        sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        sg4_dt.Columns.Add(new DataColumn("sg4_item", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));

    }

    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();

        sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;




        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "0";
        sg1_dr["sg1_t4"] = "0";

        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {

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

    public void sg4_add_blankrows()
    {
        sg4_dr = sg4_dt.NewRow();


        sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
        sg4_dr["sg4_item"] = "-";
        sg4_dr["sg4_t1"] = "-";
        sg4_dr["sg4_t2"] = "-";
        sg4_dt.Rows.Add(sg4_dr);
    }

    //------------------------------------------------------------------------------------


    //------------------------------------------------------------------------------------




    //------------------------------------------------------------------------------------


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

        cal();
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();


        oporow["branchcd"] = frm_mbr;
        oporow["type"] = "^1";
        oporow["vchnum"] = txtVchnum.Value.ToUpper().Trim();
        oporow["vchdate"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
        oporow["custname"] = txtCustomer.Value.ToUpper().Trim();
        oporow["itemname"] = txtItem.Value.ToUpper().Trim();
        oporow["grosswt"] = txt_gross.Value.ToString().Trim();
        oporow["stckhgt"] = txt_stckhgt.Value.ToString().Trim();
        oporow["Noofboxes"] = txt_box_stckd.Value.ToString().Trim();
        oporow["loadbox"] = txt_load.Value.ToString().Trim();
        oporow["storageTM_days"] = txt_storagea.Value.ToString().Trim();
        oporow["storageTM_val"] = txt_strorageb.Value.ToString().Trim();
        oporow["humidpercnt"] = txt_humida.Value.ToString().Trim();

        oporow["humidvalue"] = txt_humidb.Value.ToString().Trim();
        oporow["columnalgnd"] = txt_columna.Value.ToString().Trim();
        oporow["columnmisalgnd"] = txt_columnb.Value.ToString().Trim();
        oporow["interlockd"] = txt_interlock.Value.ToString().Trim();
        oporow["overhanged"] = txt_overhng.Value.ToString().Trim();
        oporow["deckboard_gap"] = txtdeckboard.Value.ToString().Trim();
        oporow["exceshnd"] = txtexchand.Value.ToString().Trim();

        //enter yes and No 


        oporow["aligyn"] = ddcolumna.Value.ToString().Trim();
        oporow["misaligyn"] = ddcolumnb.Value.ToString().Trim();
        oporow["interyn"] = ddinterlock.Value.ToString().Trim();
        oporow["ovhgyn"] = ddoverhang.Value.ToString().Trim();
        oporow["dckgpyn"] = dddeckboard.Value.ToString().Trim();
        oporow["exvhdyn"] = ddexchnd.Value.ToString().Trim();
        oporow["requiredSfac"] = txt_require_safetyb.Value.ToString().Trim();



        oporow["tot_envr_fac"] = txt_total_envir_facb.Value.ToString().Trim();
        oporow["requirdBCT"] = txt_require_bctb.Value.ToString().Trim();
        oporow["Boxtypecode"] = ddbox_fef.Value.ToString().Trim();
        oporow["length"] = txt_len.Value.ToString().Trim();
        oporow["width"] = txt_wid.Value.ToString().Trim();
        oporow["height"] = txt_hgt.Value.ToString().Trim();
        oporow["no_of_plies"] = ddno_piles.Value.ToString().Trim();
        oporow["flute_profile"] = ddflutep.Value.ToString().Trim();
        oporow["manf_proces"] = ddManf_Procs.Value.ToString().Trim();
        oporow["board_CALLIPR"] = txt_board_callipr.Value.ToString().Trim();
        oporow["area"] = txt_area.Value.ToString().Trim();
        oporow["Req_ECT"] = txt_req_ect.Value.ToString().Trim();
        oporow["req_RCT"] = txt_req_rct.Value.ToString().Trim();
        oporow["top_perc"] = txt_topply.Value.ToString().Trim();
        oporow["liner_perc"] = txt_linerply.Value.ToString().Trim();
        oporow["flute_perc"] = txt_fluteply.Value.ToString().Trim();
        oporow["topliner_rct"] = txt_toplinera.Value.ToString().Trim();
        oporow["flute1_rct"] = txtflute1a.Value.ToString().Trim();
        oporow["midliner_rct"] = txtmidlinera.Value.ToString().Trim();
        oporow["flute2_rct"] = txtflute2a.Value.ToString().Trim();
        oporow["innerliner_RCT"] = txtinlinera.Value.ToString().Trim();


        oporow["dep_fac_RCT"] = txtdepfaca.Value.ToString().Trim();
        oporow["topliner_BF"] = ddtoplinerb.Value.ToString().Trim();
        oporow["Flute1_BF"] = ddflute1b.Value.ToString().Trim();
        oporow["Midliner_BF"] = ddmidlinerb.Value.ToString().Trim();
        oporow["flute2_bf"] = ddflute2b.Value.ToString().Trim();
        oporow["innerliner_BF"] = ddinlinerb.Value.ToString().Trim();
        oporow["dep_fac_bf"] = txtdepfaca.Value.ToString().Trim();
        oporow["topliner_gsm_bf"] = txttoplinerc.Value.ToString().Trim();
        oporow["flute1_gsm_bf"] = txtflute1c.Value.ToString().Trim();



        oporow["midliner_gsm_bf"] = txtmidlinerc.Value.ToString().Trim();
        oporow["flute2_gsm_bf"] = txtflute2c.Value.ToString().Trim();
        oporow["innerliner_GSM_bf"] = txtinlinerc.Value.ToString().Trim();
        // oporow["dep_fac_GSM_BF"] = ;
        oporow["Topliner_GSM"] = ddtoplinerd.Value.ToString().Trim();
        oporow["Flute1_GSM"] = ddfluted.Value.ToString().Trim();
        oporow["Midliner_GSM"] = ddmidlinerd.Value.ToString().Trim();
        oporow["Flute2_GSM"] = ddflute2d.Value.ToString().Trim();
        oporow["InnerLiner_GSM"] = ddinlinerd.Value.ToString().Trim();
        oporow["Dep_fac_GSM"] = txtdepfacd.Value.ToString().Trim();
        oporow["tot_board_gsm"] = txttotalgsm.Value.ToString().Trim();
        oporow["tot_wght_car"] = txttotalwght.Value.ToString().Trim();//carton weight
        oporow["tot_board_BS"] = txttotalboardbs.Value.ToString().Trim();
        oporow["tot_CS"] = txttotalcs.Value.ToString().Trim();
        oporow["tot_RCT_GSM_BF"] = txttotalrct.Value.ToString().Trim();
        oporow["TOT_ECT_GSM_BF"] = txttotalect.Value.ToString().Trim();
        oporow["diff_Req_ECT"] = txttotaldiff.Value.ToString().Trim();

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
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;

        }
        oDS.Tables[0].Rows.Add(oporow);

    }

    void save_fun5()
    {


    }

    void save_fun2()
    {


    }


    void Type_Sel_query()
    {
    }

    protected void btn_mgr_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MGRBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Code", frm_qstr);
    }

    protected void btn_costcent_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "COSTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Cost center code", frm_qstr);
    }
    protected void btn_stat_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STATBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select State", frm_qstr);
    }
    protected void btn_bnkacct_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BNKACTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select  Bank account", frm_qstr);
    }
    protected void btn_ctry_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CTRYBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Country", frm_qstr);
    }

    protected void btn_ivl_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "IVLBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select level cost center cost", frm_qstr);
    }

    protected void btn_ply_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PLYBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select  PLY ", frm_qstr);
    }



    public void cal()
    {
        double boxstacked, loadbox, storagetime, humid, ColumnAlignd, columnmisAlgnd, interlockd, overhng, deckboardGap, ExcessiveHand, TotenvFac;
        double reqRCT, reqECT, manfpro, fluteply, topply, linerply, noplies, fluteindex1, fluteindex2, gsmtop = 0, gsmflute1 = 0, gsmmi = 0, gsmflute2 = 0, gsminn = 0;
        double caliper = 0, area = 0;


        //checking mandatory fields

        if ((txt_gross.Value == "") || (txt_stckhgt.Value == "") || (txt_storagea.Value == "") || (txt_humida.Value == "") || (txt_len.Value == "") || (txt_wid.Value == "") || (txt_hgt.Value == "") || (txt_topply.Value == "") || (txt_linerply.Value == "") || (txt_fluteply.Value == ""))
        {
            fgen.msg("", "ASMG", "Please Enter all The required fields :-grosswt,StackedHeight,StorageTime,Humidity,Length,Width,Height,top,liner,flute percentage.!!");
            return;
        }

        //checking top +liner  percentage  value

        if (Convert.ToDouble(txt_topply.Value) + Convert.ToDouble(txt_linerply.Value) + Convert.ToDouble(txt_fluteply.Value) == 100)
        { }
        else
        {
            fgen.msg("", "ASMG", "The sum of Top Ply, Liner ply and Flute Ply Must be 100");
            return;
        }

        //calculation of area 
        Double vlen = fgen.make_double(txt_len.Value.Trim());
        Double vwid = fgen.make_double(txt_wid.Value.Trim());
        Double vhgt = fgen.make_double(txt_hgt.Value.Trim());
        string vboxtype = ddbox_fef.Value.Trim();
        Double varea = 0;

        switch (vboxtype)
        {
            case "0200":
                varea = (vhgt + 0.5 * vwid + 35) * (2 * vlen + 2 * vwid + 65) / 1000000;
                break;
            case "0201":
                varea = (vhgt + vwid + 35) * (2 * vlen + 2 * vwid + 65) / 1000000;//(($H$9+$H$8+35)*(2*$H$7+2*$H$8+65))/10000000011
                break;
            case "0203":
                varea = (vhgt + 2 * vwid + 35) * (2 * vlen + 2 * vwid + 65) / 1000000;//(($H$9+2*$H$8+35)*(2*$H$7+2*$H$8+65))/1000000
                break;
            case "0204":
                varea = (vhgt + vlen + 35) * (2 * vlen + 2 * vwid + 65) / 1000000;//(($H$9+$H$7+35)*(2*$H$7+2*$H$8+65))/1000000
                break;
            case "0206":
                varea = (vhgt + 2 * vwid + 35) * (2 * vlen + 2 * vwid + 65) / 1000000;//(($H$9+2*$H$8+35)*(2*$H$7+2*$H$8+65))/1000000
                break;
            case "0215":
                varea = (vhgt + 1.75 * vwid + 835) * (2 * vlen + 2 * vwid + 65) / 1000000;//(($H$9+1.75*$H$8+835)*(2*$H$7+2*$H$8+65))/1000000
                break;
            case "0225":
                varea = (2 * vhgt + .5 * vwid + 35) * (2 * vlen + 2 * vwid + 65) / 1000000;//((2*$H$9+0.5*$H$8+35)*(2*$H$7+2*$H$8+65))/1000000
                break;
            case "0226":
                varea = (vhgt + vwid + 35) * (2 * vlen + 2 * vwid + 65) / 1000000;//(($H$9+$H$8+35)*(2*$H$7+2*$H$8+65))/1000000
                break;
            case "0301":
                varea = (2 * vhgt + .5 * vwid + 35) * (2 * vhgt + 2 * vlen + 35) / 1000000;//((2*$H$9+0.5*$H$8+35)*(2*$H$9+$H$7+35))*2/1000000
                break;
            case "0310":
                varea = (vhgt + 25) * (2 * vlen + 2 * vwid + 65) + 2 * (vlen + 2 * vhgt + 35) * (vwid + 2 * vhgt + 35) / 1000000;//((H9+25)*(2*H7+2*H8+65)+2*(H7+2*H9+35)*(H8+2*H9+35))/1000000
                break;
        }
        //

        //getting GSM value on the basis of Bus factor Selected

        SQuery = "select * from wb_CORRCST_RCTM  WHERE bf='" + ddtoplinerb.Value.ToString().Trim() + "'";
        SQuery1 = "select * from wb_CORRCST_RCTM  WHERE bf='" + ddflute1b.Value.ToString().Trim() + "'";
        SQuery2 = "select * from wb_CORRCST_RCTM  WHERE bf='" + ddmidlinerb.Value.ToString().Trim() + "'";
        SQuery3 = "select * from wb_CORRCST_RCTM  WHERE bf='" + ddflute2b.Value.ToString().Trim() + "'";
        SQuery4 = "select * from wb_CORRCST_RCTM  WHERE bf='" + ddinlinerb.Value.ToString().Trim() + "'";

        //SQuery5 = "select area from wb_corrcst_fluteM  where trim(boxtypecode)='" + ddbox_fef.Value.ToString().Trim() + "'";

        dt = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); DataTable dt5 = new DataTable(); dt6 = new DataTable();

        dt7 = new DataTable();


        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
        dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
        dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery3);
        dt6 = fgen.getdata(frm_qstr, frm_cocd, SQuery4);
        dt7 = fgen.getdata(frm_qstr, frm_cocd, SQuery5);

        if (dt.Rows.Count < 0)
        {

            fgen.msg("", "ASMG", "There is no data available for this BUS Factor. Please Update in Paper Index form");
            return;
        }
        gsmtop = fgen.make_double(dt.Rows[0]["gsm"].ToString());
        gsmflute1 = fgen.make_double(dt3.Rows[0]["gsm"].ToString());
        gsmmi = fgen.make_double(dt4.Rows[0]["gsm"].ToString());
        gsmflute2 = fgen.make_double(dt5.Rows[0]["gsm"].ToString());
        gsminn = fgen.make_double(dt6.Rows[0]["gsm"].ToString());
        area = Math.Round(varea, 2);
        txt_area.Value = area.ToString().Trim();

        boxstacked = Math.Truncate(fgen.make_double(txt_stckhgt.Value.ToString().Trim()) / fgen.make_double(txt_hgt.Value.ToString().Trim()));
        loadbox = fgen.make_double(txt_gross.Value.ToString().Trim()) * (boxstacked - 1);

        boxstacked = Math.Round(boxstacked, 2);
        loadbox = Math.Round(loadbox, 2);
        txt_box_stckd.Value = boxstacked.ToString("#######.00").Trim();
        txt_load.Value = loadbox.ToString("#######.00").Trim();


        storagetime = -0.039 * Math.Log(Convert.ToDouble(txt_storagea.Value.ToString().Trim())) + 0.7292;
        storagetime = Math.Round(storagetime, 2);
        txt_strorageb.Value = storagetime.ToString("#######.00").Trim();

        double humper = Convert.ToDouble(txt_humida.Value.ToString().Trim()) / 100;

        //(-8.3333*Math.Pow(Convert.ToDouble(txt_humida.Value.ToString().Trim()),4))+(18.485*Math.Pow(Convert.ToDouble(txt_humida.Value.ToString().Trim()),3))-(15.795*Math.Pow(Convert.ToDouble(txt_humida.Value.ToString().Trim()),2))+(5.2959*Convert.ToDouble(txt_humida.Value.ToString().Trim())+0.4988))

        humid = -8.3333 * Math.Pow(humper, 4) + 18.485 * Math.Pow(humper, 3) - 15.795 * Math.Pow(humper, 2) + 5.2959 * humper + 0.4988;

        txt_humidb.Value = fgen.make_double(humid, 2).ToString("#######.00").Trim();

        if (ddcolumna.Value == "Y")
        {
            ColumnAlignd = 1.00;
        }
        else
        {
            ColumnAlignd = 1.00;
        }


        if (ddcolumnb.Value == "Y")
        {
            columnmisAlgnd = 0.88;
        }
        else
        {
            columnmisAlgnd = 1.00;
        }

        if (ddinterlock.Value == "Y")
        {
            interlockd = 0.50;
        }
        else
        {
            interlockd = 1.00;
        }

        if (ddoverhang.Value == "Y")
        {
            overhng = 0.70;
        }
        else
        {
            overhng = 1.00;
        }


        if (dddeckboard.Value == "Y")
        {
            deckboardGap = 0.83;
        }
        else
        {
            deckboardGap = 1.00;
        }


        if (ddexchnd.Value == "Y")
        {
            ExcessiveHand = 0.85;
        }
        else
        {
            ExcessiveHand = 1.00;
        }

        txt_columna.Value = ColumnAlignd.ToString("#######.00").Trim();
        txt_columnb.Value = columnmisAlgnd.ToString("#######.00").Trim();
        txt_interlock.Value = interlockd.ToString("#######.00").Trim();
        txt_overhng.Value = overhng.ToString("#######.00").Trim();
        txtdeckboard.Value = deckboardGap.ToString("#######.00").Trim();
        txtexchand.Value = ExcessiveHand.ToString("#######.00").Trim();

        TotenvFac = Math.Round(storagetime * humid * ColumnAlignd * columnmisAlgnd * interlockd * overhng * deckboardGap * ExcessiveHand, 2);

        double ect_ = 0;
        ect_ = 1 / (storagetime * humid * ColumnAlignd * columnmisAlgnd * interlockd * overhng * deckboardGap * ExcessiveHand);

        txt_total_envir_facb.Value = TotenvFac.ToString("#######.00").Trim();
        txt_require_safetyb.Value = fgen.make_double((1 / (storagetime * humid * ColumnAlignd * columnmisAlgnd * interlockd * overhng * deckboardGap * ExcessiveHand)), 2).ToString("#######.00").Trim();
        txt_require_bctb.Value = Math.Round((loadbox * (1 / (storagetime * humid * ColumnAlignd * columnmisAlgnd * interlockd * overhng * deckboardGap * ExcessiveHand)))).ToString("#######.00").Trim();

        //----------------------------------
        txtdepfaca.Value = fgen.make_double((Math.Pow(fgen.make_double(txt_hgt.Value.ToString().Trim()), -0.18) * 2.7059), 3).ToString("#######.000").Trim();
        txtdepfacd.Value = fgen.make_double((-0.0171 * Math.Pow(fgen.make_double(txt_len.Value.ToString().Trim()) / fgen.make_double(txt_wid.Value.ToString().Trim()), 2) - 0.015 * (fgen.make_double(txt_len.Value.ToString().Trim()) / fgen.make_double(txt_wid.Value.ToString().Trim())) + 1.103), 3).ToString("#######.000").Trim();

        double leng, wid, calliper;

        SQuery = "SELECT flute,caliper,ind1,ind2 FROM wb_corrcst_fluteM WHERE flute='" + ddflutep.Value.ToString().Trim() + "'";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count < 0)
        {

            fgen.msg("", "ASMG", "There is no data available for this BUS Factor. Please Update in Paper Flute, Caliper Index Master form");
            return;
        }
        fluteindex1 = fgen.make_double(dt.Rows[0]["ind1"].ToString());
        fluteindex2 = fgen.make_double(dt.Rows[0]["ind2"].ToString());

        calliper = fgen.make_double(dt.Rows[0]["caliper"].ToString());
        txtdepfacc.Value = "L/W Fac";

        leng = fgen.make_double(txt_len.Value.ToString().Trim());
        wid = fgen.make_double(txt_wid.Value.ToString().Trim());

        txt_board_callipr.Value = calliper.ToString().Trim();

        reqECT = (fgen.make_double(txt_require_bctb.Value) / (0.599 * Math.Sqrt((2 * leng + 2 * wid) * calliper)) / (Convert.ToDouble(txtdepfaca.Value) * Convert.ToDouble(txtdepfacd.Value)));

        txt_req_ect.Value = Math.Round(reqECT, 2).ToString("#######.00").Trim();


        if (ddManf_Procs.Value == "A")
        {
            manfpro = 1.2;
        }
        else
        {
            manfpro = 0.95;

        }


        reqRCT = reqECT / manfpro;

        txt_req_rct.Value = Math.Round(reqRCT, 2).ToString().Trim();

        topply = (fgen.make_double(txt_topply.Value)) / 100;

        linerply = (fgen.make_double(txt_linerply.Value)) / 100;



        reqECT = fgen.make_double(txt_req_ect.Value);
        reqRCT = fgen.make_double(txt_req_rct.Value);

        fluteply = fgen.make_double(txt_fluteply.Value.Trim());
        fluteply = Math.Round(fluteply / 100, 2);

        txt_toplinera.Value = Math.Round(((((fgen.make_double(txt_require_bctb.Value) / (0.599 * Math.Sqrt((2 * leng + 2 * wid) * calliper)) / (Convert.ToDouble(txtdepfaca.Value) * Convert.ToDouble(txtdepfacd.Value)))) / manfpro) * topply), 2).ToString("#######.00").Trim();

        txtflute1a.Value = Math.Round((((((fgen.make_double(txt_require_bctb.Value) / (0.599 * Math.Sqrt((2 * leng + 2 * wid) * calliper)) / (Convert.ToDouble(txtdepfaca.Value) * Convert.ToDouble(txtdepfacd.Value)))) / manfpro) * fluteply) / (ddno_piles.Value == "5" ? noplies = 2 : noplies = 1) / fluteindex1), 2).ToString("#######.00").Trim();

        txtmidlinera.Value = Math.Round((((reqECT / manfpro) * linerply) / (ddno_piles.Value == "5" ? noplies = 2 : noplies = 1)), 2).ToString("#######.00").Trim();

        if (ddno_piles.Value == "3")
        {
            txtflute2a.Value = "0";
        }
        else
        {
            txtflute2a.Value = Math.Round((((reqECT / manfpro) * fluteply) / (ddno_piles.Value == "5" ? noplies = 2 : noplies = 0) / fluteindex2), 2).ToString("#######.00").Trim();
        }

        if (ddno_piles.Value == "3")
        {
            txtinlinera.Value = "0";
        }
        else
        {
            txtinlinera.Value = Math.Round((((reqECT / manfpro) * linerply) / (ddno_piles.Value == "5" ? noplies = 2 : noplies = 0)), 2).ToString("#######.00").Trim();
        }
        // txtdepfaca.Value = (Math.Pow(fgen.make_double(txt_hgt.Value.ToString().Trim()), -0.18)*2.7059).ToString().Trim();

        txttoplinerc.Value = Math.Round((fgen.make_double((fgen.make_double(txt_toplinera.Value, 2) * 1000) / gsmtop, 2)), 2).ToString("#######.00").Trim();
        txtflute1c.Value = Math.Round((fgen.make_double((fgen.make_double(txtflute1a.Value, 2) * 1000) / gsmflute1, 2))).ToString("#######.00").Trim();
        txtmidlinerc.Value = Math.Round((fgen.make_double((fgen.make_double(txtmidlinera.Value, 2) * 1000) / gsmmi, 2)), 2).ToString("#######.00").Trim();
        txtflute2c.Value = Math.Round((fgen.make_double((fgen.make_double(txtflute2a.Value, 2) * 1000) / gsmflute2, 2))).ToString("#######.00").Trim();

        txtinlinerc.Value = Math.Round((fgen.make_double((fgen.make_double(txtinlinera.Value, 2) * 1000) / gsminn, 2)), 2).ToString("#######.00").Trim();

        // txtdepfacd.Value = (0.0171 * Math.Pow(fgen.make_double(txt_len.Value.ToString().Trim()) / fgen.make_double(txt_wid.Value.ToString().Trim()), 2) - 0.015 * (fgen.make_double(txt_len.Value.ToString().Trim()) / fgen.make_double(txt_wid.Value.ToString().Trim())) + 1.103).ToString().Trim();

        txttotalgsm.Value = (((Convert.ToDouble(ddtoplinerd.Value) + Convert.ToDouble(ddmidlinerd.Value) + Convert.ToDouble(ddinlinerd.Value)) + (fgen.make_double(ddfluted.Value)) * fluteindex1) + (fgen.make_double(ddflute2d.Value) * fluteindex2) * ((ddno_piles.Value == "5" ? noplies = 1 : noplies = 0))).ToString("#######.00").Trim();

        txttotalwght.Value = Math.Round((fgen.make_double(txt_area.Value) * fgen.make_double(txttotalgsm.Value))).ToString("#######.00").Trim();

        //important parameter calculation wise



        txttotalboardbs.Value = Math.Round((((fgen.make_double(ddtoplinerb.Value) * fgen.make_double(ddtoplinerd.Value)) + (fgen.make_double(ddmidlinerb.Value) * fgen.make_double(ddmidlinerd.Value)) + (fgen.make_double(ddinlinerb.Value) * (fgen.make_double(ddinlinerd.Value)) + (fgen.make_double(ddflute1b.Value) * fgen.make_double(ddfluted.Value)) * (ddManf_Procs.Value == "A" ? manfpro = 0.2 : manfpro = 0.4)) + (fgen.make_double(ddflute2b.Value, 2) * fgen.make_double(ddflute2d.Value, 2)) * (ddManf_Procs.Value == "A" ? manfpro = 0.2 : manfpro = 0.4)) / 1000), 2).ToString("#######.00").Trim();

        double totrct = Math.Round(((gsmtop * Convert.ToDouble(ddtoplinerd.Value)) + (gsmflute1 * Convert.ToDouble(ddfluted.Value) * fluteindex1) + (gsmmi * Convert.ToDouble(ddmidlinerd.Value)) + (gsmflute2 * Convert.ToDouble(ddflute2d.Value) * fluteindex1 * (ddno_piles.Value == "3" ? noplies = 0 : noplies = 1)) + (gsminn * Convert.ToDouble(ddinlinerd.Value) * (ddno_piles.Value == "3" ? noplies = 0 : noplies = 1)))) / 1000;

        txttotalrct.Value = Math.Round(totrct, 2).ToString("#######.00").Trim(); //hardcode



        txttotalect.Value = Math.Round((Convert.ToDouble(txttotalrct.Value.ToString().Trim()) * (ddManf_Procs.Value == "A" ? manfpro = 1.2 : manfpro = 0.95)), 2).ToString("#######.00").Trim();



        txttotalcs.Value = Math.Round((0.599 * Convert.ToDouble(txttotalect.Value) * Math.Sqrt((2 * leng + 2 * wid) * calliper) * (Convert.ToDouble(txtdepfaca.Value) * Convert.ToDouble(txtdepfacd.Value)))).ToString("#######.00").Trim();


        txttotaldiff.Value = Math.Round((fgen.make_double(txttotalect.Value) - fgen.make_double(txt_req_ect.Value)), 2).ToString("#######.00").Trim();

    }

    protected void btncal_Click(object sender, EventArgs e)
    {
        cal();
        btnsave.Disabled = false;
    }



    protected void btnboximg_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = fgen.seek_iname(frm_qstr, frm_cocd, "Select boxtypecode,imagepath||'\\'||image as fstr from wb_corrcst_flutem where branchcd !='DD' and trim(boxtypecode) ='" + ddbox_fef.Value.Trim() + "'", "fstr");
        //string filePath = Server.MapPath("../tej-base/dp/om_test.jpg ");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + filePath + "','90%','90%','Finsys Viewer');", true);
    }

    public void bindflute()
    {
        DataTable dt5 = new DataTable();
        SQuery = "select distinct boxtypecode, boxtypecode||'-'||name as fstr from wb_corrcst_fluteM where trim(boxtypecode) !='-' order by boxtypecode";
        dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        DataTable dt8 = new DataTable();
        SQuery = "select distinct Flute from wb_corrcst_fluteM where trim(flute) !='-' and type='^4' order by flute";
        dt8 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        ddflutep.DataSource = dt8;
        ddflutep.DataTextField = "Flute";
        ddflutep.DataValueField = "Flute";
        ddflutep.DataBind();

        ddbox_fef.DataSource = dt5;
        ddbox_fef.DataTextField = "fstr";
        ddbox_fef.DataValueField = "boxtypecode";
        ddbox_fef.DataBind();

        dt6 = new DataTable();
        SQuery = "select BF from wb_CORRCST_RCTM where bf not in('0','45') order by BF";
        dt6 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        ddtoplinerb.DataSource = dt6;
        ddtoplinerb.DataTextField = "BF";
        ddtoplinerb.DataValueField = "BF";
        ddtoplinerb.DataBind();

        ddflute1b.DataSource = dt6;
        ddflute1b.DataTextField = "BF";
        ddflute1b.DataValueField = "BF";
        ddflute1b.DataBind();

        ddinlinerb.DataSource = dt6;
        ddinlinerb.DataTextField = "BF";
        ddinlinerb.DataValueField = "BF";
        ddinlinerb.DataBind();

        ddmidlinerb.DataSource = dt6;
        ddmidlinerb.DataTextField = "BF";
        ddmidlinerb.DataValueField = "BF";
        ddmidlinerb.DataBind();

        ddflute2b.DataSource = dt6;
        ddmidlinerb.DataTextField = "BF";
        ddmidlinerb.DataValueField = "BF";
        ddmidlinerb.DataBind();

        ddflute2b.DataSource = dt6;
        ddflute2b.DataTextField = "BF";
        ddflute2b.DataValueField = "BF";
        ddflute2b.DataBind();

        txt_gross_unit.Value = "kg";
        txt_stckhgt_unit.Value = "mm";
        txt_box_stckd_unit.Value = "Nos";
        txt_load_unit.Value = "Kg";
        txtdepfacc.Value = "L/W Fac";

        txt_storagea.Value = "30";
        txt_humida.Value = "65";
        txt_len.Value = "100";
        txt_wid.Value = "100";
        txt_hgt.Value = "100";
        txt_topply.Value = "20";
        txt_linerply.Value = "40";
        txt_fluteply.Value = "40";
        txt_gross.Value = "20";
        txt_stckhgt.Value = "2500";
    }
    protected void btn_img_Click(object sender, EventArgs e)
    {

        string sboxcode = ddbox_fef.Value.Substring(0, 4).Trim();
        if (sboxcode == "-" || sboxcode == "0")
        {
            fgen.msg("Alert", frm_cocd, "Please select Box Type to see image");
        }
        else
        {
            string filepath = fgen.seek_iname(frm_qstr, frm_cocd, "select imagepath as filepath from wb_corrcst_flutem where trim(boxtypecode)= '" + sboxcode + "'", "filepath");
            string newPath = Server.MapPath(@"~\tej-base\upload\");
            string filename = Path.GetFileName(filepath);
            newPath += filename;
            btn_img.ImageUrl = @"~\tej-base\upload\" + filename + "";
        }
    }

    protected void ddbox_fef_changeindex(object sender, EventArgs e)
    {

    }

}



