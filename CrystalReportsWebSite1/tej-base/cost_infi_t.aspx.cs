using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class cost_infi_t : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt1, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0; string mq0, mq1, mq2, mq3;
    double b17 = 0, e5 = 0, e7 = 0, n32 = 0, n33 = 0, n34 = 0, j14 = 0, j15 = 0, A52 = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok; string cmd_query;
    string save_it;
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
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
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
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
            btnlist.Visible = false;
            btnprint.Visible = false;
            btnCal.Visible = false;
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;
        ImageButton1.Enabled = false; btnCal.Disabled = true;
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
        ImageButton1.Enabled = true; btnCal.Disabled = false;
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
        lblheader.Text = "Costing Sheet";
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "somas_anx";
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
            case "TICODE":
                SQuery = "select Icode as fstr,Iname as Item,Icode as Item_code,Cpartno,Cdrgno from Item where substr(icode,1,1) in ('9') and length(Trim(icode))>4 order by icode";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
            case "List":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Trav_no,to_char(a.vchdate,'dd/mm/yyyy') as Trav_dt,a.icode as item_code,b.INAME as item_name,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
        frm_vty = "TC";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        txtpt1.Focus();
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
        cal();
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        if (txticode.Text == "" || txticode.Text == "0")
        {
            fgen.msg("-", "AMSG", "Please select Item Name");
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
        clearctrl();
        fgen.msg("-", "AMSG", "Sorry! No print is available for now");
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
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), System.DateTime.Now.Date.ToString("dd/MM/yyyy"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                    if (col1 == "") return;
                    newCase(col1);
                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
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
                    doc_addl.Value = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                     #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "select a.*,b.iname,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,to_chaR(a.edt_dt,'dd/mm/yyyy') as pedt_Dt from " + frm_tabname + " A,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) and A.BRANCHCD||A.TYPE||trim(A.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') ='" + frm_mbr + frm_vty + col1 + "'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txticode.Text = dt.Rows[0]["icode"].ToString().Trim(); 
                        txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtpt1.Text = dt.Rows[0]["t1"].ToString().Trim();
                        txtpt2.Text = dt.Rows[0]["t2"].ToString().Trim();
                        txtpt3.Text = dt.Rows[0]["t3"].ToString().Trim();
                        txtpt4.Text = dt.Rows[0]["t4"].ToString().Trim();
                        txtpt5.Text = dt.Rows[0]["t5"].ToString().Trim();
                        txtpt6.Text = dt.Rows[0]["t6"].ToString().Trim();
                        txtpt7.Text = dt.Rows[0]["t7"].ToString().Trim();
                        txtpt8.Text = dt.Rows[0]["t8"].ToString().Trim();
                        txtpt9.Text = dt.Rows[0]["t9"].ToString().Trim();

                        txtpt10.Text = dt.Rows[0]["t10"].ToString().Trim();
                        txtpt11.Text = dt.Rows[0]["t11"].ToString().Trim();
                        txtpt12.Text = dt.Rows[0]["t12"].ToString().Trim();
                        txtpt13.Text = dt.Rows[0]["t13"].ToString().Trim();
                        txtpt14.Text = dt.Rows[0]["t14"].ToString().Trim();
                        txtpt15.Text = dt.Rows[0]["t15"].ToString().Trim();
                        txtpt16.Text = dt.Rows[0]["t16"].ToString().Trim();
                        txtpt17.Text = dt.Rows[0]["t17"].ToString().Trim();
                        txtpt18.Text = dt.Rows[0]["t18"].ToString().Trim();
                        txtpt19.Text = dt.Rows[0]["t19"].ToString().Trim();

                        txtpt20.Text = dt.Rows[0]["t20"].ToString().Trim();
                        txtpt21.Text = dt.Rows[0]["t21"].ToString().Trim();
                        txtpt22.Text = dt.Rows[0]["t22"].ToString().Trim();
                        txtpt23.Text = dt.Rows[0]["t23"].ToString().Trim();
                        txtpt24.Text = dt.Rows[0]["t24"].ToString().Trim();
                        txtpt25.Text = dt.Rows[0]["t25"].ToString().Trim();
                        txtpt26.Text = dt.Rows[0]["t26"].ToString().Trim();
                        txtpt27.Text = dt.Rows[0]["t27"].ToString().Trim();
                        txtpt28.Text = dt.Rows[0]["t28"].ToString().Trim();
                        txtpt29.Text = dt.Rows[0]["t29"].ToString().Trim();

                        txtpt30.Text = dt.Rows[0]["t30"].ToString().Trim();
                        txtpt31.Text = dt.Rows[0]["t31"].ToString().Trim();
                        txtpt32.Text = dt.Rows[0]["t32"].ToString().Trim();
                        txtpt33.Text = dt.Rows[0]["t33"].ToString().Trim();
                        txtpt34.Text = dt.Rows[0]["t34"].ToString().Trim();
                        txtpt35.Text = dt.Rows[0]["t35"].ToString().Trim();
                        txtpt36.Text = dt.Rows[0]["t36"].ToString().Trim();
                        txtpt37.Text = dt.Rows[0]["t37"].ToString().Trim();
                        txtpt38.Text = dt.Rows[0]["t38"].ToString().Trim();
                        txtpt39.Text = dt.Rows[0]["t39"].ToString().Trim();

                        txtpt40.Text = dt.Rows[0]["t40"].ToString().Trim();
                        txtpt41.Text = dt.Rows[0]["t41"].ToString().Trim();
                        txtpt42.Text = dt.Rows[0]["t42"].ToString().Trim();
                        txtpt43.Text = dt.Rows[0]["t43"].ToString().Trim();
                        txtpt44.Text = dt.Rows[0]["t44"].ToString().Trim();
                        txtpt45.Text = dt.Rows[0]["t45"].ToString().Trim();
                        txtpt46.Text = dt.Rows[0]["t46"].ToString().Trim();
                        txtpt47.Text = dt.Rows[0]["t47"].ToString().Trim();
                        txtpt48.Text = dt.Rows[0]["t48"].ToString().Trim();
                        txtpt49.Text = dt.Rows[0]["t49"].ToString().Trim();

                        txtpt50.Text = dt.Rows[0]["t50"].ToString().Trim();
                        txtpt51.Text = dt.Rows[0]["t51"].ToString().Trim();
                        txtpt52.Text = dt.Rows[0]["t52"].ToString().Trim();
                        txtpt53.Text = dt.Rows[0]["t53"].ToString().Trim();
                        txtpt54.Text = dt.Rows[0]["t54"].ToString().Trim();
                        txtpt55.Text = dt.Rows[0]["t55"].ToString().Trim();
                        //Grand Total
                        txtpt56.Text = dt.Rows[0]["t56"].ToString().Trim();
                        txtpt57.Text = dt.Rows[0]["t57"].ToString().Trim();
                        txtpt58.Text = dt.Rows[0]["t58"].ToString().Trim();
                        txtpt59.Text = dt.Rows[0]["t59"].ToString().Trim();

                        txtpt60.Text = dt.Rows[0]["t60"].ToString().Trim();
                        txtpt61.Text = dt.Rows[0]["t61"].ToString().Trim();
                        txtpt62.Text = dt.Rows[0]["t62"].ToString().Trim();
                        txtpt63.Text = dt.Rows[0]["t63"].ToString().Trim();
                        txtpt64.Text = dt.Rows[0]["t64"].ToString().Trim();
                        txtpt65.Text = dt.Rows[0]["t65"].ToString().Trim();
                        txtpt66.Text = dt.Rows[0]["t66"].ToString().Trim();
                        txtpt67.Text = dt.Rows[0]["t67"].ToString().Trim();
                        txtpt68.Text = dt.Rows[0]["t68"].ToString().Trim();
                        txtpt69.Text = dt.Rows[0]["t69"].ToString().Trim();

                        txtpt70.Text = dt.Rows[0]["t70"].ToString().Trim();
                        txtpt71.Text = dt.Rows[0]["t71"].ToString().Trim();
                        txtpt72.Text = dt.Rows[0]["t72"].ToString().Trim();
                        txtpt73.Text = dt.Rows[0]["t73"].ToString().Trim();
                        txtpt74.Text = dt.Rows[0]["t74"].ToString().Trim();
                        txtpt75.Text = dt.Rows[0]["t75"].ToString().Trim();
                        txtpt76.Text = dt.Rows[0]["t76"].ToString().Trim();
                        txtpt77.Text = dt.Rows[0]["t77"].ToString().Trim();
                        txtpt78.Text = dt.Rows[0]["t78"].ToString().Trim();
                        txtpt79.Text = dt.Rows[0]["t79"].ToString().Trim();

                        txtpt80.Text = dt.Rows[0]["t80"].ToString().Trim();
                        txtpt81.Text = dt.Rows[0]["t81"].ToString().Trim();
                        txtpt82.Text = dt.Rows[0]["t82"].ToString().Trim();
                        txtpt83.Text = dt.Rows[0]["t83"].ToString().Trim();
                        txtpt84.Text = dt.Rows[0]["t84"].ToString().Trim();
                        txtpt85.Text = dt.Rows[0]["t85"].ToString().Trim();
                        txtpt86.Text = dt.Rows[0]["t86"].ToString().Trim();
                        txtpt87.Text = dt.Rows[0]["t87"].ToString().Trim();
                        txtpt88.Text = dt.Rows[0]["t88"].ToString().Trim();
                        txtpt89.Text = dt.Rows[0]["t89"].ToString().Trim();

                        txtpt90.Text = dt.Rows[0]["t90"].ToString().Trim();
                        txtpt91.Text = dt.Rows[0]["t91"].ToString().Trim();
                        txtpt92.Text = dt.Rows[0]["t92"].ToString().Trim();
                        txtpt93.Text = dt.Rows[0]["t93"].ToString().Trim();
                        txtpt94.Text = dt.Rows[0]["t94"].ToString().Trim();
                        txtpt95.Text = dt.Rows[0]["t95"].ToString().Trim();
                        txtpt96.Text = dt.Rows[0]["t96"].ToString().Trim();
                        txtpt97.Text = dt.Rows[0]["t97"].ToString().Trim();
                        txtpt98.Text = dt.Rows[0]["t98"].ToString().Trim();
                        txtpt99.Text = dt.Rows[0]["t99"].ToString().Trim();

                        txtpt100.Text = dt.Rows[0]["t100"].ToString().Trim();
                        txtpt101.Text = dt.Rows[0]["t101"].ToString().Trim();
                        txtpt102.Text = dt.Rows[0]["t102"].ToString().Trim();
                        txtpt103.Text = dt.Rows[0]["t103"].ToString().Trim();

                        txtpt104.Text = dt.Rows[0]["t104"].ToString().Trim();
                        txtpt105.Text = dt.Rows[0]["t105"].ToString().Trim();
                        txtpt106.Text = dt.Rows[0]["t106"].ToString().Trim();

                        txtpt107.Text = dt.Rows[0]["t107"].ToString().Trim();
                        txtpt108.Text = dt.Rows[0]["t108"].ToString().Trim();
                        txtpt109.Text = dt.Rows[0]["t109"].ToString().Trim();

                        TextBox5.Text = txtpt68.Text.Trim(); TextBox7.Text = txtpt68.Text.Trim(); TextBox9.Text = txtpt68.Text.Trim();
                        TextBox6.Text = txtpt71.Text.Trim(); TextBox8.Text = txtpt77.Text.Trim(); TextBox10.Text = txtpt83.Text.Trim();
                        TextBox2.Text = txtpt2.Text.Trim(); TextBox3.Text = txtpt3.Text.Trim(); TextBox4.Text = txtpt7.Text.Trim();

                        dd1.SelectedIndex = Convert.ToInt32(dt.Rows[0]["cdrgno"].ToString().Trim());

                        TextBox11.Text = txtpt100.Text.Trim().ToString();
                        TextBox1.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt57.Text.Trim()) - Convert.ToDouble(txtpt100.Text.Trim()), 2));
                        TextBox12.Text = txtpt57.Text.Trim();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        dt.Dispose();
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", doc_addl.Value.Trim());// grade
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "List":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "List_E";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
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
                    txticode.Text = col1.Trim().ToString();
                    txtiname.Text = fgen.seek_iname(frm_qstr,frm_cocd, "Select iname from item where trim(icode)='" + col1.Trim() + "'", "iname");
                    txtpt3.Focus();
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
            SQuery = "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
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
            #endregion
                }
            }
        }
    }
    //------------------------------------------------------------------------------------ 
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr.Trim();
        oporow["TYPE"] = frm_vty.Trim();
        oporow["vchnum"] = txtvchnum.Text.Trim();
        oporow["vchdate"] = txtvchdate.Text.Trim();
        oporow["acode"] = "-";
        oporow["icode"] = txticode.Text.Trim();

        //if (frm_cocd == "JACL" || frm_cocd == "INFI" || frm_cocd == "SYDB")
        //{
        oporow["t1"] = txtpt1.Text.Trim();
        oporow["t2"] = txtpt2.Text.Trim();
        oporow["t3"] = txtpt3.Text.Trim();
        oporow["t4"] = txtpt4.Text.Trim();
        oporow["t5"] = txtpt5.Text.Trim();
        oporow["t6"] = txtpt6.Text.Trim();
        oporow["t7"] = txtpt7.Text.Trim();
        oporow["t8"] = txtpt8.Text.Trim();
        oporow["t9"] = txtpt9.Text.Trim();
        oporow["t10"] = txtpt10.Text.Trim();
        oporow["t11"] = txtpt11.Text.Trim();
        oporow["t12"] = txtpt12.Text.Trim();
        oporow["t13"] = txtpt13.Text.Trim();
        oporow["t14"] = txtpt14.Text.Trim();
        oporow["t15"] = txtpt15.Text.Trim();
        oporow["t16"] = txtpt16.Text.Trim();
        oporow["t17"] = txtpt17.Text.Trim();
        oporow["t18"] = txtpt18.Text.Trim();
        oporow["t19"] = txtpt19.Text.Trim();
        oporow["t20"] = txtpt20.Text.Trim();
        oporow["t21"] = txtpt21.Text.Trim();
        oporow["t22"] = txtpt22.Text.Trim();
        oporow["t23"] = txtpt23.Text.Trim();
        oporow["t24"] = txtpt24.Text.Trim();
        oporow["t25"] = txtpt25.Text.Trim();
        oporow["t26"] = txtpt26.Text.Trim();
        oporow["t27"] = txtpt27.Text.Trim();
        oporow["t28"] = txtpt28.Text.Trim();
        oporow["t29"] = txtpt29.Text.Trim();
        oporow["t30"] = txtpt30.Text.Trim();
        oporow["t31"] = txtpt31.Text.Trim();
        oporow["t32"] = txtpt32.Text.Trim();
        oporow["t33"] = txtpt33.Text.Trim();
        oporow["t34"] = txtpt34.Text.Trim();
        oporow["t35"] = txtpt35.Text.Trim();
        oporow["t36"] = txtpt36.Text.Trim();
        oporow["t37"] = txtpt37.Text.Trim();
        oporow["t38"] = txtpt38.Text.Trim();
        oporow["t39"] = txtpt39.Text.Trim();
        oporow["t40"] = txtpt40.Text.Trim();
        oporow["t41"] = txtpt41.Text.Trim();
        oporow["t42"] = txtpt42.Text.Trim();
        oporow["t43"] = txtpt43.Text.Trim();
        oporow["t44"] = txtpt44.Text.Trim();
        oporow["t45"] = txtpt45.Text.Trim();
        oporow["t46"] = txtpt46.Text.Trim();
        oporow["t47"] = txtpt47.Text.Trim();
        oporow["t48"] = txtpt48.Text.Trim();
        oporow["t49"] = txtpt49.Text.Trim();
        oporow["t50"] = txtpt50.Text.Trim();
        oporow["t51"] = txtpt51.Text.Trim();
        oporow["t52"] = txtpt52.Text.Trim();
        oporow["t53"] = txtpt53.Text.Trim();
        oporow["t54"] = txtpt54.Text.Trim();
        oporow["t55"] = txtpt55.Text.Trim();
        //Grand Total
        oporow["t56"] = txtpt56.Text.Trim();
        oporow["t57"] = txtpt57.Text.Trim();
        oporow["t58"] = txtpt58.Text.Trim();
        oporow["t59"] = txtpt59.Text.Trim();
        oporow["t60"] = txtpt60.Text.Trim();
        oporow["t61"] = txtpt61.Text.Trim();
        oporow["t62"] = txtpt62.Text.Trim();
        oporow["t63"] = txtpt63.Text.Trim();
        oporow["t64"] = txtpt64.Text.Trim();
        oporow["t65"] = txtpt65.Text.Trim();
        oporow["t66"] = txtpt66.Text.Trim();
        oporow["t67"] = txtpt67.Text.Trim();
        oporow["t68"] = txtpt68.Text.Trim();
        oporow["t69"] = txtpt69.Text.Trim();
        oporow["t70"] = txtpt70.Text.Trim();
        oporow["t71"] = txtpt71.Text.Trim();
        oporow["t72"] = txtpt72.Text.Trim();
        oporow["t73"] = txtpt73.Text.Trim();
        oporow["t74"] = txtpt74.Text.Trim();
        oporow["t75"] = txtpt75.Text.Trim();
        oporow["t76"] = txtpt76.Text.Trim();
        oporow["t77"] = txtpt77.Text.Trim();
        oporow["t78"] = txtpt78.Text.Trim();
        oporow["t79"] = txtpt79.Text.Trim();
        oporow["t80"] = txtpt80.Text.Trim();
        oporow["t81"] = txtpt81.Text.Trim();
        oporow["t82"] = txtpt82.Text.Trim();
        oporow["t83"] = txtpt83.Text.Trim();
        oporow["t84"] = txtpt84.Text.Trim();
        oporow["t85"] = txtpt85.Text.Trim();
        oporow["t86"] = txtpt86.Text.Trim();
        oporow["t87"] = txtpt87.Text.Trim();
        oporow["t88"] = txtpt88.Text.Trim();
        oporow["t89"] = txtpt89.Text.Trim();
        oporow["t90"] = txtpt90.Text.Trim();
        oporow["t91"] = txtpt91.Text.Trim();
        oporow["t92"] = txtpt92.Text.Trim();
        oporow["t93"] = txtpt93.Text.Trim();
        oporow["t94"] = txtpt94.Text.Trim();
        oporow["t95"] = txtpt95.Text.Trim();
        oporow["t96"] = txtpt96.Text.Trim();
        oporow["t97"] = txtpt97.Text.Trim();
        oporow["t98"] = txtpt98.Text.Trim();
        oporow["t99"] = txtpt99.Text.Trim();
        oporow["t100"] = txtpt100.Text.Trim();
        oporow["t101"] = txtpt101.Text.Trim();
        oporow["t102"] = txtpt102.Text.Trim();
        oporow["t103"] = txtpt103.Text.Trim();
        oporow["t104"] = txtpt104.Text.Trim();
        oporow["t105"] = txtpt105.Text.Trim();
        oporow["t106"] = txtpt106.Text.Trim();
        oporow["t107"] = txtpt107.Text.Trim();
        oporow["t108"] = txtpt108.Text.Trim();
        oporow["t109"] = txtpt109.Text.Trim();
        oporow["cdrgno"] = dd1.SelectedIndex.ToString().Trim();
        //}
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
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "TC");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
    }
    //------------------------------------------------------------------------------------   
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    public void cal()
    {
        try
        {
            txtpt5.Text = txtpt1.Text.Trim();
            if (txtpt10.Text.Trim() == "1")
                txtpt13.Text = "0";
            if (txtpt11.Text.Trim() == "1")
                txtpt104.Text = "0";
            if (txtpt13.Text.Trim() == "1")
                txtpt10.Text = "0";
            if (txtpt104.Text.Trim() == "1") txtpt11.Text = "0";

            fgen.fill_zero(this.Controls);

            txtpt4.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt2.Text.Trim()) * Convert.ToDouble(txtpt3.Text.Trim()), 2));

            txtpt17.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) / Convert.ToDouble(txtpt7.Text.Trim()), 2));

            if (Convert.ToDouble(txtpt17.Text.Trim()) < 4000) txtpt16.Text = "1";
            else txtpt16.Text = "0";

            if (Convert.ToDouble(txtpt17.Text.Trim()) >= 4000) txtpt18.Text = "1";
            else txtpt18.Text = "0";

            e5 = Convert.ToDouble(txtpt4.Text.Trim());

            if (e5 <= 600) { txtpt31.Text = "700"; txtpt35.Text = "700"; n33 = 1; b17 = 300; }
            else { txtpt31.Text = "1000"; txtpt35.Text = "1000"; n33 = 1.5; b17 = 600; }

            txtpt19.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt4.Text.Trim()) * Convert.ToDouble(txtpt6.Text.Trim()) * Convert.ToDouble(txtpt17.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100))) / 1550000, 2));
            txtpt20.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt19.Text.Trim()) * Convert.ToDouble(txtpt5.Text.Trim()), 2));

            if (txtpt21.Text == null || txtpt21.Text == "0") txtpt21.Text = Convert.ToString(Math.Round(b17 * Convert.ToDouble(txtpt9.Text.Trim()), 2));

            txtpt22.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt21.Text.Trim()) * 1, 2));

            e7 = Convert.ToDouble(txtpt9.Text.Trim());

            if (e7 == 0) n32 = 0;
            else if (e7 == 1) n32 = 300;
            else if (e7 == 2) n32 = 500;
            else if (e7 == 3) n32 = 650;
            else if (e7 == 4) n32 = 800;
            else if (e7 == 5) n32 = 1000;
            else if (e7 == 6) n32 = 1200;
            else if (e7 == 7) n32 = 1500;
            else if (e7 == 8) n32 = 1700;

            txtpt23.Text = Convert.ToString(Math.Round(n32 * n33 * 4000 * Convert.ToDouble(txtpt16.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100)) / 1000, 2));
            txtpt24.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt23.Text.Trim()) * 1, 2));

            txtpt25.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * n32 * n33 * Convert.ToDouble(txtpt18.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100)) / 1000, 2));
            txtpt26.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt25.Text.Trim()) * 1, 2));
            
            txtpt27.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt4.Text.Trim()) / 3) * (Convert.ToDouble(txtpt17.Text.Trim()) / 100), 2));
            txtpt28.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt27.Text.Trim()) * Convert.ToDouble(txtpt13.Text.Trim()), 2));

            j14 = Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * (Convert.ToDouble(txtpt15.Text.Trim()) / 100), 2);

            if (j14 > 200) j15 = 200;
            else j15 = j14;

            txtpt29.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt4.Text.Trim()) * (Convert.ToDouble(txtpt17.Text.Trim()) - j15) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100) - 0.02) * 0.09) / 100, 1));
            txtpt30.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt29.Text.Trim()) * Convert.ToDouble(txtpt10.Text.Trim()), 2));

            txtpt32.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt31.Text.Trim()) * Convert.ToDouble(txtpt11.Text.Trim()), 2));

            txtpt33.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt4.Text.Trim()) / 4.5 * Convert.ToDouble(txtpt17.Text.Trim()) / 100, 1));
            txtpt34.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt33.Text.Trim()) * Convert.ToDouble(txtpt11.Text.Trim()), 2));

            txtpt106.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt4.Text.Trim()) / 4.5 * Convert.ToDouble(txtpt17.Text.Trim()) / 100, 1));
            txtpt109.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt106.Text.Trim()) * Convert.ToDouble(txtpt104.Text.Trim()), 2));

            txtpt36.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt35.Text.Trim()) * Convert.ToDouble(txtpt12.Text.Trim()), 2));

            txtpt37.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt17.Text.Trim()) - j15) * Convert.ToDouble(txtpt4.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100)) * 4.5 / 1000, 2));
            txtpt38.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt37.Text.Trim()) * Convert.ToDouble(txtpt12.Text.Trim()), 2));

            txtpt39.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt7.Text.Trim()) * 100, 2));
            txtpt40.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt39.Text.Trim()) * 1, 2));

            txtpt41.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * 195 / 1000, 1));
            txtpt42.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt41.Text.Trim()) * 1, 2));

            txtpt43.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) * 6 / 1000, 2));
            txtpt44.Text = txtpt43.Text.Trim();

            txtpt107.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * 195 / 1000, 1));
            txtpt108.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt107.Text.Trim()) * Convert.ToDouble(txtpt105.Text.Trim()), 2));

            if (Convert.ToDouble(txtpt14.Text.Trim()) == 1) n34 = 80;
            else n34 = 50;

            txtpt45.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) * n34 / 1000, 2));
            txtpt46.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt45.Text.Trim()) * 1, 2));

            txtpt47.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) * 25 / 1000, 3));
            txtpt48.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt47.Text.Trim()) * 1, 2));

            fgen.fill_zero(this.Controls);

            txtpt49.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt20.Text.Trim()) + Convert.ToDouble(txtpt22.Text.Trim()) + Convert.ToDouble(txtpt24.Text.Trim()) + Convert.ToDouble(txtpt26.Text.Trim()) + Convert.ToDouble(txtpt28.Text.Trim())
                + Convert.ToDouble(txtpt30.Text.Trim()) + Convert.ToDouble(txtpt32.Text.Trim()) + Convert.ToDouble(txtpt34.Text.Trim()) + Convert.ToDouble(txtpt36.Text.Trim()) + Convert.ToDouble(txtpt38.Text.Trim()) + Convert.ToDouble(txtpt40.Text.Trim())
                + Convert.ToDouble(txtpt42.Text.Trim()) + Convert.ToDouble(txtpt44.Text.Trim()) + Convert.ToDouble(txtpt46.Text.Trim()) + Convert.ToDouble(txtpt48.Text.Trim()) + Convert.ToDouble(txtpt108.Text.Trim()) + Convert.ToDouble(txtpt109.Text.Trim()) + Convert.ToDouble(txtpt103.Text.Trim()), 2));

            if (Convert.ToDouble(txtpt50.Text.Trim()) == 0) txtpt51.Text = "0";
            else txtpt51.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt49.Text.Trim()) * (Convert.ToDouble(txtpt50.Text.Trim()) / 100), 2));

            txtpt52.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt49.Text.Trim()) + Convert.ToDouble(txtpt51.Text.Trim()), 2));
            txtpt53.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt52.Text.Trim()) / Convert.ToDouble(txtpt8.Text.Trim()), 2));

            txtpt55.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt54.Text.Trim()) / Convert.ToDouble(txtpt8.Text.Trim()), 2));

            txtpt61.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt58.Text.Trim()) * 0.03) + Convert.ToDouble(txtpt58.Text.Trim()), 2));
            txtpt62.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt59.Text.Trim()) * 0.03) + Convert.ToDouble(txtpt59.Text.Trim()), 2));
            txtpt63.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt60.Text.Trim()) * 0.03) + Convert.ToDouble(txtpt60.Text.Trim()), 2));

            txtpt64.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt62.Text.Trim()) + Convert.ToDouble(txtpt63.Text.Trim()) + 20, 2));
            txtpt65.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt61.Text.Trim()) + Convert.ToDouble(txtpt62.Text.Trim()) + 50 + 20, 2));

            txtpt66.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt64.Text.Trim()) / 25.4, 2));
            txtpt67.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt65.Text.Trim()) / 25.4, 2));
            //Area of Sheet
            TextBox2.Text = txtpt2.Text.Trim(); TextBox3.Text = txtpt3.Text.Trim(); TextBox4.Text = txtpt7.Text.Trim();
            txtpt68.Text = Convert.ToString(Math.Round(((Convert.ToDouble(TextBox2.Text.Trim()) * Convert.ToDouble(TextBox3.Text.Trim())) / 1550) / Convert.ToDouble(TextBox4.Text.Trim()), 4));
            //Paper Detail

            txtpt70.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt74.Text.Trim()) / 1000, 4));
            txtpt76.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt80.Text.Trim()) / 1000, 4));
            txtpt82.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt86.Text.Trim()) / 1000, 4));

            txtpt71.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt69.Text.Trim()) * Convert.ToDouble(txtpt70.Text.Trim()), 4));
            if (dd1.SelectedIndex == 0) A52 = 0.35;
            else if (dd1.SelectedIndex == 1) A52 = 0.45;
            else if (dd1.SelectedIndex == 2) A52 = 0.50;

            txtpt77.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt75.Text.Trim()) * Convert.ToDouble(txtpt76.Text.Trim())) * A52 + (Convert.ToDouble(txtpt75.Text.Trim()) * Convert.ToDouble(txtpt76.Text.Trim())), 4));
            txtpt83.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt81.Text.Trim()) * Convert.ToDouble(txtpt82.Text.Trim()), 4));
            //BS
            txtpt73.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt71.Text.Trim()) * Convert.ToDouble(txtpt72.Text.Trim()), 4));
            txtpt79.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt75.Text.Trim()) * Convert.ToDouble(txtpt76.Text.Trim()) * Convert.ToDouble(txtpt78.Text.Trim())) / 2, 4));
            txtpt85.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt83.Text.Trim()) * Convert.ToDouble(txtpt84.Text.Trim()), 4));

            TextBox5.Text = txtpt68.Text.Trim(); TextBox7.Text = txtpt68.Text.Trim(); TextBox9.Text = txtpt68.Text.Trim();
            TextBox6.Text = txtpt71.Text.Trim(); TextBox8.Text = txtpt77.Text.Trim(); TextBox10.Text = txtpt83.Text.Trim();

            txtpt87.Text = Convert.ToString(Math.Round(Convert.ToDouble(TextBox5.Text.Trim()) * Convert.ToDouble(TextBox6.Text.Trim()), 4));
            txtpt90.Text = Convert.ToString(Math.Round(Convert.ToDouble(TextBox7.Text.Trim()) * Convert.ToDouble(TextBox8.Text.Trim()), 4));
            txtpt93.Text = Convert.ToString(Math.Round(Convert.ToDouble(TextBox9.Text.Trim()) * Convert.ToDouble(TextBox10.Text.Trim()), 4));

            txtpt89.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt87.Text.Trim()) * Convert.ToDouble(txtpt88.Text.Trim()), 4));
            txtpt92.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt90.Text.Trim()) * Convert.ToDouble(txtpt91.Text.Trim()), 4));
            txtpt95.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt92.Text.Trim()) * Convert.ToDouble(txtpt93.Text.Trim()), 4));
            //Total Paper WG Cost
            txtpt96.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt87.Text.Trim()) + Convert.ToDouble(txtpt90.Text.Trim()) + Convert.ToDouble(txtpt93.Text.Trim()), 4));
            txtpt97.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt89.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim()) + Convert.ToDouble(txtpt95.Text.Trim()), 4));
            //Conversion , Rej
            txtpt98.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt96.Text.Trim()) * Convert.ToDouble(txtpt101.Text.Trim()), 4));
            txtpt99.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt97.Text.Trim()) + Convert.ToDouble(txtpt98.Text.Trim())) * (Convert.ToDouble(txtpt102.Text.Trim()) / 100), 4));
            //Total Cost in second tab 0.00 is for print cost
            txtpt100.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt97.Text.Trim()) + Convert.ToDouble(txtpt98.Text.Trim()) + Convert.ToDouble(txtpt99.Text.Trim()) + 0.00, 2));
            TextBox11.Text = txtpt100.Text.Trim();
            TextBox1.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt53.Text.Trim()) + Convert.ToDouble(txtpt55.Text.Trim()), 2));
            //Grand Total
            txtpt57.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt53.Text.Trim()) + Convert.ToDouble(txtpt55.Text.Trim()) + Convert.ToDouble(txtpt100.Text.Trim()) + Convert.ToDouble(txtpt103.Text.Trim()), 2));
            TextBox12.Text = txtpt57.Text.Trim();

            fgen.fill_zero(this.Controls);
        }
        catch
        {
            fgen.fill_zero(this.Controls);
        }
    }
    //------------------------------------------------------------------------------------
    #region Text Changed Event
    protected void txtpt1_TextChanged(object sender, EventArgs e)
    {
        //txtpt5.Text = txtpt1.Text.Trim();
        //cal();
        //txtpt2.Focus();
    }
    protected void txtpt2_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt3.Focus();
    }
    protected void txtpt3_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt7.Focus();
    }
    protected void txtpt7_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt6.Focus();
    }
    protected void txtpt6_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt9.Focus();
    }
    protected void txtpt9_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt10.Focus();
    }
    protected void txtpt10_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt8.Focus();
        //if (txtpt10.Text.Trim() == "1")
        //    txtpt13.Text = "0";
    }
    protected void txtpt8_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt11.Focus();
    }
    protected void txtpt11_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt12.Focus();
        //if (txtpt11.Text.Trim() == "1")
            //txtpt104.Text = "0";
    }
    protected void txtpt12_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt13.Focus();
    }
    protected void txtpt13_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt14.Focus();
        //if (txtpt13.Text.Trim() == "1")
        //    txtpt10.Text = "0";
    }
    protected void txtpt14_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt15.Focus();
    }
    protected void txtpt103_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt15.Focus();
    }
    protected void txtpt15_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt104.Focus();
    }
    protected void txtpt50_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt54.Focus();
    }
    protected void txtpt54_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //txtpt58.Focus();
    }
    protected void txtpt58_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt59.Focus();
    }
    protected void txtpt59_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt60.Focus();
    }
    protected void txtpt60_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt69.Focus();
    }
    protected void txtpt69_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt75.Focus();
    }
    protected void txtpt75_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt81.Focus();
    }
    protected void txtpt81_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt72.Focus();
    }
    protected void txtpt72_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt78.Focus();
    }
    protected void txtpt78_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt84.Focus();
    }
    protected void txtpt84_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt74.Focus();
    }
    protected void txtpt74_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt80.Focus();
    }
    protected void txtpt80_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt86.Focus();
    }
    protected void txtpt86_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt88.Focus();
    }
    protected void txtpt88_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt91.Focus();
    }
    protected void txtpt91_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt94.Focus();
    }
    protected void txtpt94_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt101.Focus();
    }
    protected void txtpt101_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt102.Focus();
    }
    protected void txtpt102_TextChanged(object sender, EventArgs e)
    {
        //cal(); btnsave.Focus();
    }
    protected void txtpt21_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt50.Focus();
    }
    protected void txtpt104_TextChanged(object sender, EventArgs e)
    {
        //cal();
        //if (txtpt104.Text.Trim() == "1") txtpt11.Text = "0";
        //txtpt105.Focus();
    }
    protected void txtpt105_TextChanged(object sender, EventArgs e)
    {
        //cal(); txtpt50.Focus();
    }
    #endregion
    protected void btnCal_ServerClick(object sender, EventArgs e)
    {
        cal();
    }
}