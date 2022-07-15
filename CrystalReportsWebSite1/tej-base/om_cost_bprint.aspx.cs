using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_cost_bprint : System.Web.UI.Page
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
            txtWPer.Text = "%";
            txtOPer.Text = "%";
            txtDPer.Text = "Nos";
            txtTax3.Text = "%";
            btnlist.Visible = false;
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
        btnprint.Disabled = false; btnlist.Disabled = false; btnlbl4.Enabled = false; ImageButton42.Enabled = false;
        ImageButton1.Enabled = false; ImageButton2.Enabled = false;
        ImageButton3.Enabled = false; ImageButton4.Enabled = false;
        ImageButton5.Enabled = false; ImageButton6.Enabled = false;
        ImageButton7.Enabled = false; ImageButton8.Enabled = false;
        ImageButton9.Enabled = false; ImageButton10.Enabled = false;
        ImageButton11.Enabled = false; ImageButton12.Enabled = false;
        ImageButton13.Enabled = false; ImageButton14.Enabled = false;
        ImageButton15.Enabled = false; ImageButton16.Enabled = false;
        ImageButton17.Enabled = false; ImageButton18.Enabled = false;
        ImageButton19.Enabled = false; ImageButton20.Enabled = false;
        ImageButton21.Enabled = false; ImageButton22.Enabled = false;
        ImageButton23.Enabled = false; ImageButton24.Enabled = false;
        ImageButton25.Enabled = false; ImageButton26.Enabled = false;
        ImageButton27.Enabled = false; ImageButton28.Enabled = false;
        ImageButton29.Enabled = false; ImageButton30.Enabled = false;
        ImageButton31.Enabled = false; ImageButton32.Enabled = false;
        ImageButton33.Enabled = false; ImageButton34.Enabled = false;
        ImageButton35.Enabled = false; ImageButton36.Enabled = false;
        ImageButton37.Enabled = false; ImageButton38.Enabled = false;
        ImageButton39.Enabled = false; ImageButton40.Enabled = false;
        ImageButton41.Enabled = false; btnCal.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true; ImageButton42.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true;
        ImageButton1.Enabled = true; ImageButton2.Enabled = true;
        ImageButton3.Enabled = true; ImageButton4.Enabled = true;
        ImageButton5.Enabled = true; ImageButton6.Enabled = true;
        ImageButton7.Enabled = true; ImageButton8.Enabled = true;
        ImageButton9.Enabled = true; ImageButton10.Enabled = true;
        ImageButton11.Enabled = true; ImageButton12.Enabled = true;
        ImageButton13.Enabled = true; ImageButton14.Enabled = true;
        ImageButton15.Enabled = true; ImageButton16.Enabled = true;
        ImageButton17.Enabled = true; ImageButton18.Enabled = true;
        ImageButton19.Enabled = true; ImageButton20.Enabled = true;
        ImageButton21.Enabled = true; ImageButton22.Enabled = true;
        ImageButton23.Enabled = true; ImageButton24.Enabled = true;
        ImageButton25.Enabled = true; ImageButton26.Enabled = true;
        ImageButton27.Enabled = true; ImageButton28.Enabled = true;
        ImageButton29.Enabled = true; ImageButton30.Enabled = true;
        ImageButton31.Enabled = true; ImageButton32.Enabled = true;
        ImageButton33.Enabled = true; ImageButton34.Enabled = true;
        ImageButton35.Enabled = true; ImageButton36.Enabled = true;
        ImageButton37.Enabled = true; ImageButton38.Enabled = true;
        ImageButton39.Enabled = true; ImageButton40.Enabled = true;
        ImageButton41.Enabled = true; btnCal.Disabled = false;
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
        frm_tabname = "wb_tran_cost";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CP16");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
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
                SQuery = "select acode as fstr,trim(aname) as customer_name,acode as code,acode,addr1,addr2 from famst where length(trim(nvl(deac_by,'-')))<=1 and branchcd!='DD' and substr(Acode,1,2) ='16' order by aname";
                break;

            case "ICODE":
                SQuery = "select icode as fstr,trim(iname) as item_name,icode as code,cpartno,unit from item where length(trim(icode))>4 order by item_name";
                break;

            case "PTOP":
            case "PBTM":
            case "PTREY":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP15' order by code";
                break;

            case "LTOP":
            case "LBTM":
            case "LTREY":
                SQuery = "select trim(type1) as fstr,name,col1 as extra_value,num1 as rate,type1 as code,id from wb_master where id='CP01' order by code";
                break;

            case "PRTOP":
            case "PRBTM":
            case "PRTREY":
                SQuery = "select trim(type1) as fstr,name,col1 as color,num1 as rate,type1 as code,id from wb_master where id='CP02' order by code";
                break;

            case "PUTOP":
            case "PUBTM":
            case "PUTREY":
                SQuery = "select trim(type1) as fstr,name,col1 as color,num1 as rate,type1 as code,id from wb_master where id='CP03' order by code";
                break;

            case "SPTOP":
            case "SPBTM":
            case "SPTREY":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP04' order by code";
                break;

            case "MTOP":
            case "MBTM":
            case "MTREY":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP05' order by code";
                break;

            case "DRTOP":
            case "DRBTM":
            case "DRTREY":
                SQuery = "select trim(type1) as fstr,name,num1 as dripp_off_rate,type1 as code,id from wb_master where id='CP06' order by code";
                break;

            case "TOPSPOT":
            case "BTMSPOT":
            case "TREYSPOT":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP07' order by code";
                break;

            case "TOPFOIL":
            case "BTMFOIL":
            case "TREYFOIL":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP08' order by code";
                break;

            case "TOPPUNCHING":
            case "BTMPUNCHING":
            case "TREYPUNCHING":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP09' order by code";
                break;

            case "TOPEMBOSSING":
            case "BTMEMBOSSING":
            case "TREYEMBOSSING":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP10' order by code";
                break;

            case "WASTAGE":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP11' order by code";
                break;

            case "DELIVERYCHARGES":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP12' order by code";
                break;

            case "PAY":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP13' order by code";
                break;

            case "GTOP":
            case "GBTM":
            case "GTREY":
                SQuery = "select trim(type1) as fstr,name,num1 as rate,type1 as code,id from wb_master where id='CP14' order by code";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "select distinct a.branchcd||trim(a.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.acode as code,a.aname as cust_name,a.icode as item_code,A.INAME as item_name,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.acode as code,a.aname as cust_name,a.icode as item_code,A.INAME as item_name,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
            frm_vty = "CP16";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        setColHeadings();
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
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        Cal();        

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (txtlbl4a.Text.Length <= 1)
        {
            fgen.msg("-", frm_qstr, "Please Fill Customer");
            return;
        }
        if (txtIcode.Text.Length <= 1)
        {
            fgen.msg("-", frm_qstr, "Please Fill Item");
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CP16");
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + frm_vty + " Deleted");
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
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,to_chaR(a.edt_dt,'dd/mm/yyyy') as pedt_Dt from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["pent_Dt"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[0]["edt_by"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[0]["pedt_Dt"].ToString().Trim();

                        // PAPER
                        txtPTop.Text = dt.Rows[0]["COL1"].ToString().Trim();
                        txtPTL.Text = dt.Rows[0]["NUM1"].ToString().Trim();
                        txtPTW.Text = dt.Rows[0]["NUM2"].ToString().Trim();
                        txtPTGSM.Text = dt.Rows[0]["NUM3"].ToString().Trim();
                        txtPTRate.Text = dt.Rows[0]["NUM4"].ToString().Trim();
                        txtPTAmt.Text = dt.Rows[0]["NUM5"].ToString().Trim();

                        txtPBottom.Text = dt.Rows[0]["COL2"].ToString().Trim();
                        txtPBL.Text = dt.Rows[0]["NUM6"].ToString().Trim();
                        txtPBW.Text = dt.Rows[0]["NUM7"].ToString().Trim();
                        txtPBGSM.Text = dt.Rows[0]["NUM8"].ToString().Trim();
                        txtPBRate.Text = dt.Rows[0]["NUM9"].ToString().Trim();
                        txtPBAmt.Text = dt.Rows[0]["NUM10"].ToString().Trim();

                        txtPTrey.Text = dt.Rows[0]["COL3"].ToString().Trim();
                        txtPTrL.Text = dt.Rows[0]["NUM11"].ToString().Trim();
                        txtPTrW.Text = dt.Rows[0]["NUM12"].ToString().Trim();
                        txtPTrGSM.Text = dt.Rows[0]["NUM13"].ToString().Trim();
                        txtPTrRate.Text = dt.Rows[0]["NUM14"].ToString().Trim();
                        txtPTrAmt.Text = dt.Rows[0]["NUM15"].ToString().Trim();
                        txtPTot.Text = dt.Rows[0]["NUM16"].ToString().Trim();

                        // LAMINATION
                        txtLA_Top.Text = dt.Rows[0]["COL4"].ToString().Trim();
                        txtLA_Ts.Text = dt.Rows[0]["COL5"].ToString().Trim();
                        txtLA_TL.Text = dt.Rows[0]["NUM17"].ToString().Trim();
                        txtLA_TW.Text = dt.Rows[0]["NUM18"].ToString().Trim();
                        txtLA_TMRate.Text = dt.Rows[0]["NUM19"].ToString().Trim();
                        txtLA_TRate.Text = dt.Rows[0]["NUM20"].ToString().Trim();

                        txtLA_Btm.Text = dt.Rows[0]["COL6"].ToString().Trim();
                        txtLA_Bs.Text = dt.Rows[0]["COL7"].ToString().Trim();
                        txtLA_BL.Text = dt.Rows[0]["NUM21"].ToString().Trim();
                        txtLA_BW.Text = dt.Rows[0]["NUM22"].ToString().Trim();
                        txtLA_BMRate.Text = dt.Rows[0]["NUM23"].ToString().Trim();
                        txtLA_BRate.Text = dt.Rows[0]["NUM24"].ToString().Trim();

                        txtLA_Trey.Text = dt.Rows[0]["COL8"].ToString().Trim();
                        txtLA_TrS.Text = dt.Rows[0]["COL9"].ToString().Trim();
                        txtLA_TrL.Text = dt.Rows[0]["NUM25"].ToString().Trim();
                        txtLA_TrW.Text = dt.Rows[0]["NUM26"].ToString().Trim();
                        txtLA_TrMRate.Text = dt.Rows[0]["NUM27"].ToString().Trim();
                        txtLA_TRRate.Text = dt.Rows[0]["NUM28"].ToString().Trim();
                        txtLATot.Text = dt.Rows[0]["NUM29"].ToString().Trim();

                        // PRINTING
                        txtPR_TCon.Text = dt.Rows[0]["COL10"].ToString().Trim();
                        txtPR_TS.Text = dt.Rows[0]["COL11"].ToString().Trim();
                        txtPR_TRate.Text = dt.Rows[0]["NUM30"].ToString().Trim();

                        txtPR_BCon.Text = dt.Rows[0]["COL12"].ToString().Trim();
                        txtPR_BS.Text = dt.Rows[0]["COL13"].ToString().Trim();
                        txtPR_BRate.Text = dt.Rows[0]["NUM31"].ToString().Trim();

                        txtPR_TrCon.Text = dt.Rows[0]["COL14"].ToString().Trim();
                        txtPR_TrS.Text = dt.Rows[0]["COL15"].ToString().Trim();
                        txtPR_TrRate.Text = dt.Rows[0]["NUM32"].ToString().Trim();
                        txtPRTot.Text = dt.Rows[0]["NUM33"].ToString().Trim();

                        // PRINTING U.V.
                        txtPU_TCon.Text = dt.Rows[0]["COL16"].ToString().Trim();
                        txtPU_TS.Text = dt.Rows[0]["COL17"].ToString().Trim();
                        txtPU_TRate.Text = dt.Rows[0]["NUM34"].ToString().Trim();

                        txtPU_BCon.Text = dt.Rows[0]["COL18"].ToString().Trim();
                        txtPU_BS.Text = dt.Rows[0]["COL19"].ToString().Trim();
                        txtPU_BRate.Text = dt.Rows[0]["NUM35"].ToString().Trim();

                        txtPU_TrCon.Text = dt.Rows[0]["COL20"].ToString().Trim();
                        txtPU_TrS.Text = dt.Rows[0]["COL21"].ToString().Trim();
                        txtPU_TrRate.Text = dt.Rows[0]["NUM36"].ToString().Trim();
                        txtPUTot.Text = dt.Rows[0]["NUM37"].ToString().Trim();

                        // SCREEN PRINTING
                        txtS_TP.Text = dt.Rows[0]["COL22"].ToString().Trim();
                        txtS_TRate.Text = dt.Rows[0]["NUM38"].ToString().Trim();

                        txtS_BP.Text = dt.Rows[0]["COL23"].ToString().Trim();
                        txtS_BRate.Text = dt.Rows[0]["NUM39"].ToString().Trim();

                        txtS_TrP.Text = dt.Rows[0]["COL24"].ToString().Trim();
                        txtS_TrRate.Text = dt.Rows[0]["NUM40"].ToString().Trim();
                        txtSTot.Text = dt.Rows[0]["NUM41"].ToString().Trim();

                        // MICRO
                        txtM_Top.Text = dt.Rows[0]["COL25"].ToString().Trim();
                        txtM_TRate.Text = dt.Rows[0]["NUM42"].ToString().Trim();

                        txtM_Btm.Text = dt.Rows[0]["COL26"].ToString().Trim();
                        txtM_BRate.Text = dt.Rows[0]["NUM43"].ToString().Trim();

                        txtM_Trey.Text = dt.Rows[0]["COL27"].ToString().Trim();
                        txtM_TrRate.Text = dt.Rows[0]["NUM44"].ToString().Trim();
                        txtMTot.Text = dt.Rows[0]["NUM45"].ToString().Trim();

                        // DRIP OFF
                        txtD_TDName.Text = dt.Rows[0]["COL28"].ToString().Trim();
                        txtD_TDriff.Text = dt.Rows[0]["NUM46"].ToString().Trim();
                        txtD_TDM.Text = dt.Rows[0]["NUM47"].ToString().Trim();
                        txtD_TAmt.Text = dt.Rows[0]["NUM48"].ToString().Trim();

                        txtD_TGName.Text = dt.Rows[0]["COL29"].ToString().Trim();
                        txtD_TGRate.Text = dt.Rows[0]["NUM49"].ToString().Trim();
                        txtD_TG.Text = dt.Rows[0]["NUM50"].ToString().Trim();

                        txtD_BDName.Text = dt.Rows[0]["COL30"].ToString().Trim();
                        txtD_BDriff.Text = dt.Rows[0]["NUM51"].ToString().Trim();
                        txtD_BDM.Text = dt.Rows[0]["NUM52"].ToString().Trim();
                        txtD_BAmt.Text = dt.Rows[0]["NUM53"].ToString().Trim();

                        txtD_BGName.Text = dt.Rows[0]["COL31"].ToString().Trim();
                        txtD_BGRate.Text = dt.Rows[0]["NUM54"].ToString().Trim();
                        txtD_BG.Text = dt.Rows[0]["NUM55"].ToString().Trim();

                        txtD_TrDName.Text = dt.Rows[0]["COL32"].ToString().Trim();
                        txtD_TrDriff.Text = dt.Rows[0]["NUM56"].ToString().Trim();
                        txtD_TrDM.Text = dt.Rows[0]["NUM57"].ToString().Trim();
                        txtD_TrAmt.Text = dt.Rows[0]["NUM58"].ToString().Trim();

                        txtD_TrGName.Text = dt.Rows[0]["COL33"].ToString().Trim();
                        txtD_TrGRate.Text = dt.Rows[0]["NUM59"].ToString().Trim();
                        txtD_TrG.Text = dt.Rows[0]["NUM60"].ToString().Trim();

                        txtDTot.Text = dt.Rows[0]["NUM61"].ToString().Trim();
                        txtGTot.Text = dt.Rows[0]["NUM62"].ToString().Trim();

                        // FOILING PUNCHING SPOT U.V.
                        txtSpot_T.Text = dt.Rows[0]["COL34"].ToString().Trim();
                        txtSpot_TRate.Text = dt.Rows[0]["NUM63"].ToString().Trim();

                        txtSpot_B.Text = dt.Rows[0]["COL35"].ToString().Trim();
                        txtSpot_BRate.Text = dt.Rows[0]["NUM64"].ToString().Trim();

                        txtSpot_Tr.Text = dt.Rows[0]["COL36"].ToString().Trim();
                        txtSpot_TrRate.Text = dt.Rows[0]["NUM65"].ToString().Trim();
                        txtSpot_Tot.Text = dt.Rows[0]["NUM66"].ToString().Trim();

                        // FOILING PUNCHING FOIL
                        txtFoil_T.Text = dt.Rows[0]["COL37"].ToString().Trim();
                        txtFoil_TRate.Text = dt.Rows[0]["NUM67"].ToString().Trim();

                        txtFoil_B.Text = dt.Rows[0]["COL38"].ToString().Trim();
                        txtFoil_BRate.Text = dt.Rows[0]["NUM68"].ToString().Trim();

                        txtFoil_Tr.Text = dt.Rows[0]["COL39"].ToString().Trim();
                        txtFoil_TrRate.Text = dt.Rows[0]["NUM69"].ToString().Trim();
                        txtFoil_Tot.Text = dt.Rows[0]["NUM70"].ToString().Trim();

                        // FOILING PUNCHING PUNCNING
                        txtPunc_T.Text = dt.Rows[0]["COL40"].ToString().Trim();
                        txtPunc_TRate.Text = dt.Rows[0]["NUM71"].ToString().Trim();

                        txtPunc_B.Text = dt.Rows[0]["COL41"].ToString().Trim();
                        txtPunc_BRate.Text = dt.Rows[0]["NUM72"].ToString().Trim();

                        txtPunc_Tr.Text = dt.Rows[0]["COL42"].ToString().Trim();
                        txtPunc_TrRate.Text = dt.Rows[0]["NUM73"].ToString().Trim();
                        txtPunc_Tot.Text = dt.Rows[0]["NUM74"].ToString().Trim();

                        // FOILING PUNCHING EMBOSSING
                        txtEmb_T.Text = dt.Rows[0]["COL43"].ToString().Trim();
                        txtEmb_TRate.Text = dt.Rows[0]["NUM75"].ToString().Trim();

                        txtEmb_B.Text = dt.Rows[0]["COL44"].ToString().Trim();
                        txtEmb_BRate.Text = dt.Rows[0]["NUM76"].ToString().Trim();

                        txtEmb_Tr.Text = dt.Rows[0]["COL45"].ToString().Trim();
                        txtEmb_TrRate.Text = dt.Rows[0]["NUM77"].ToString().Trim();
                        txtEmb_Tot.Text = dt.Rows[0]["NUM78"].ToString().Trim();

                        txtGrossAmt.Text = dt.Rows[0]["GROSSAMT"].ToString().Trim();

                        // WASTAGE
                        txtW1.Text = dt.Rows[0]["COL46"].ToString().Trim();
                        txtW2.Text = dt.Rows[0]["NUM79"].ToString().Trim();
                        txtW3.Text = dt.Rows[0]["NUM80"].ToString().Trim();

                        // OTHER VALUE
                        txtO1.Text = dt.Rows[0]["COL47"].ToString().Trim();
                        txtO2.Text = dt.Rows[0]["NUM81"].ToString().Trim();
                        txtO3.Text = dt.Rows[0]["NUM82"].ToString().Trim();

                        // DELIVERY CHARGES
                        txtD1.Text = dt.Rows[0]["COL48"].ToString().Trim();
                        txtD2.Text = dt.Rows[0]["NUM83"].ToString().Trim();
                        txtD3.Text = dt.Rows[0]["NUM84"].ToString().Trim();

                        txtTot.Text = dt.Rows[0]["TOTAL"].ToString().Trim();

                        // PAY TERMS
                        txtPay1.Text = dt.Rows[0]["COL49"].ToString().Trim();
                        txtPay2.Text = dt.Rows[0]["NUM85"].ToString().Trim();
                        txtPay3.Text = dt.Rows[0]["COL50"].ToString().Trim();
                        txtPay4.Text = dt.Rows[0]["NUM86"].ToString().Trim();
                        txtPayTot.Text = dt.Rows[0]["PAYTOT"].ToString().Trim();

                        // TAX
                        txtTax1.Text = dt.Rows[0]["COL51"].ToString().Trim();
                        txtTax2.Text = dt.Rows[0]["NUM87"].ToString().Trim();
                        txtTax3.Text = dt.Rows[0]["COL52"].ToString().Trim();
                        txtTax4.Text = dt.Rows[0]["NUM88"].ToString().Trim();

                        txtGrandTot.Text = dt.Rows[0]["GRANDTOT"].ToString().Trim();

                        dt.Dispose();
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
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_engg_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    ImageButton42.Focus();
                    break;

                case "ICODE":
                    if (col1.Length <= 0) return;
                    txtIcode.Text = col1;
                    txtIname.Text = col2;
                    ImageButton1.Focus();
                    break;
                
                case "PTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP15' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPTop.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPTRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton2.Focus();
                    break;

                case "PBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP15' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {                        
                        txtPBottom.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPBRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton3.Focus();
                    break;

                case "PTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP15' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPTrey.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPTrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton4.Focus();
                    break;

                case "LTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP01' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtLA_Top.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtLA_Ts.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtLA_TMRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton5.Focus();
                    break;

                case "LBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP01' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtLA_Btm.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtLA_Bs.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtLA_BMRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton6.Focus();
                    break;

                case "LTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP01' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtLA_Trey.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtLA_TrS.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtLA_TrMRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton16.Focus();
                    break;

                case "PRTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP02' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPR_TCon.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtPR_TS.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPR_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton7.Focus();
                    break;

                case "PRBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP02' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPR_BCon.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtPR_BS.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPR_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton35.Focus();
                    break;

                case "PRTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP02' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPR_TrCon.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtPR_TrS.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPR_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton36.Focus();
                    break;

                case "PUTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP03' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPU_TCon.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtPU_TS.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPU_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton37.Focus();
                    break;

                case "PUBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP03' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPU_BCon.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtPU_BS.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPU_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton38.Focus();
                    break;

                case "PUTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,col1,num1,type1 from wb_master where id='CP03' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPU_TrCon.Text = dt.Rows[0]["col1"].ToString().Trim();
                        txtPU_TrS.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPU_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton39.Focus();
                    break;

                case "SPTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP04' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtS_TP.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtS_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton40.Focus();
                    break;

                case "SPBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP04' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtS_BP.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtS_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton41.Focus();
                    break;

                case "SPTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP04' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtS_TrP.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtS_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton10.Focus();
                    break;

                case "MTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP05' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtM_Top.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtM_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton19.Focus();
                    break;

                case "MBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP05' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtM_Btm.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtM_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton20.Focus();
                    break;

                case "MTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP05' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtM_Trey.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtM_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton8.Focus();
                    break;

                case "DRTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,num2,type1 from wb_master where id='CP06' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtD_TDName.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtD_TDM.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton11.Focus();
                    break;

                case "DRBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,num2,type1 from wb_master where id='CP06' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtD_BDName.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtD_BDM.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton12.Focus();
                    break;

                case "DRTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,num2,type1 from wb_master where id='CP06' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtD_TrDName.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtD_TrDM.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton23.Focus();
                    break;

                case "GTOP":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP14' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtD_TGName.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtD_TGRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton24.Focus();
                    break;

                case "GBTM":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP14' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtD_BGName.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtD_BGRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton25.Focus();
                    break;

                case "GTREY":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP14' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtD_TrGName.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtD_TrGRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton9.Focus();
                    break;

                case "TOPSPOT":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP07' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtSpot_T.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtSpot_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton27.Focus();
                    break;

                case "BTMSPOT":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP07' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtSpot_B.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtSpot_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton31.Focus();
                    break;

                case "TREYSPOT":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP07' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtSpot_Tr.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtSpot_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton17.Focus();
                    break;

                case "TOPFOIL":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP08' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtFoil_T.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtFoil_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton28.Focus();
                    break;

                case "BTMFOIL":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP08' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtFoil_B.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtFoil_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton32.Focus();
                    break;

                case "TREYFOIL":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP08' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtFoil_Tr.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtFoil_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton18.Focus();
                    break;

                case "TOPPUNCHING":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP09' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPunc_T.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPunc_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton29.Focus();
                    break;

                case "BTMPUNCHING":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP09' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPunc_B.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPunc_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton33.Focus();
                    break;

                case "TREYPUNCHING":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP09' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtPunc_Tr.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtPunc_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton26.Focus();
                    break;

                case "TOPEMBOSSING":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP10' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtEmb_T.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtEmb_TRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton30.Focus();
                    break;

                case "BTMEMBOSSING":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP10' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtEmb_B.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtEmb_BRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton34.Focus();
                    break;

                case "TREYEMBOSSING":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select name,num1,type1 from wb_master where id='CP10' and type1='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtEmb_Tr.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtEmb_TrRate.Text = dt.Rows[0]["num1"].ToString().Trim();
                        Cal();
                    }
                    ImageButton13.Focus();
                    break;

                case "WASTAGE":
                    if (col1.Length <= 0) return;
                    txtW1.Text = col2;
                    txtW2.Text = col3;
                    Cal();
                    txtO1.Focus();
                    break;

                case "OTHERVALUE":
                    if (col1.Length <= 0) return;
                    txtO1.Text = col2;
                    txtO2.Text = col3;
                    Cal();
                    ImageButton21.Focus();
                    break;

                case "DELIVERYCHARGES":
                    if (col1.Length <= 0) return;
                    txtD1.Text = col2;
                    txtD2.Text = col3;
                    txtD3.Text = col3;
                    ImageButton22.Focus();
                    break;

                case "PAY":
                    if (col1.Length <= 0) return;
                    txtPay1.Text = col2;
                    txtPay2.Text = col3;
                    txtPay3.Text = "%";
                    Cal();
                    txtTax1.Focus();
                    break;

                case "TAX":
                    if (col1.Length <= 0) return;
                    txtTax1.Text = col2;
                    txtTax2.Text = col3;
                    txtTax3.Text = "%";
                    Cal();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY"); ;
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select a.Vchnum as Master_no,to_char(a.vchdate,'dd/mm/yyyy') as Master_Dt,a.Col1 as Type_of_charges,a.col2 as description,a.col3 as True_or_false,a.col4 as Standard,a.num1 as rate,a.num2 as amt,a.Ent_by,a.ent_Dt,a.Edt_by,a.edt_Dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd,a.vchnum,a.srno";
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
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton42_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = txtvchdate.Text.Trim();
        oporow["acode"] = txtlbl4.Text.Trim().ToUpper();
        oporow["icode"] = txtIcode.Text.Trim().ToUpper();
        oporow["aname"] = txtlbl4a.Text.Trim().ToUpper();
        oporow["iname"] = txtIname.Text.Trim().ToUpper();

        // PAPER
        oporow["COL1"] = txtPTop.Text.Trim().ToUpper();
        oporow["NUM1"] = fgen.make_double(txtPTL.Text.Trim().ToUpper());
        oporow["NUM2"] = fgen.make_double(txtPTW.Text.Trim().ToUpper());
        oporow["NUM3"] = fgen.make_double(txtPTGSM.Text.Trim().ToUpper());
        oporow["NUM4"] = fgen.make_double(txtPTRate.Text.Trim().ToUpper());
        oporow["NUM5"] = fgen.make_double(txtPTAmt.Text.Trim().ToUpper());

        oporow["COL2"] = txtPBottom.Text.Trim().ToUpper();
        oporow["NUM6"] = fgen.make_double(txtPBL.Text.Trim().ToUpper());
        oporow["NUM7"] = fgen.make_double(txtPBW.Text.Trim().ToUpper());
        oporow["NUM8"] = fgen.make_double(txtPBGSM.Text.Trim().ToUpper());
        oporow["NUM9"] = fgen.make_double(txtPBRate.Text.Trim().ToUpper());
        oporow["NUM10"] = fgen.make_double(txtPBAmt.Text.Trim().ToUpper());

        oporow["COL3"] = txtPTrey.Text.Trim().ToUpper();
        oporow["NUM11"] = fgen.make_double(txtPTrL.Text.Trim().ToUpper());
        oporow["NUM12"] = fgen.make_double(txtPTrW.Text.Trim().ToUpper());
        oporow["NUM13"] = fgen.make_double(txtPTrGSM.Text.Trim().ToUpper());
        oporow["NUM14"] = fgen.make_double(txtPTrRate.Text.Trim().ToUpper());
        oporow["NUM15"] = fgen.make_double(txtPTrAmt.Text.Trim().ToUpper());
        oporow["NUM16"] = fgen.make_double(txtPTot.Text.Trim().ToUpper());

        // LAMINATION
        oporow["COL4"] = txtLA_Top.Text.Trim().ToUpper();
        oporow["COL5"] = txtLA_Ts.Text.Trim().ToUpper();
        oporow["NUM17"] = fgen.make_double(txtLA_TL.Text.Trim().ToUpper());
        oporow["NUM18"] = fgen.make_double(txtLA_TW.Text.Trim().ToUpper());
        oporow["NUM19"] = fgen.make_double(txtLA_TMRate.Text.Trim().ToUpper());
        oporow["NUM20"] = fgen.make_double(txtLA_TRate.Text.Trim().ToUpper());

        oporow["COL6"] = txtLA_Btm.Text.Trim().ToUpper();
        oporow["COL7"] = txtLA_Bs.Text.Trim().ToUpper();
        oporow["NUM21"] = fgen.make_double(txtLA_BL.Text.Trim().ToUpper());
        oporow["NUM22"] = fgen.make_double(txtLA_BW.Text.Trim().ToUpper());
        oporow["NUM23"] = fgen.make_double(txtLA_BMRate.Text.Trim().ToUpper());
        oporow["NUM24"] = fgen.make_double(txtLA_BRate.Text.Trim().ToUpper());

        oporow["COL8"] = txtLA_Trey.Text.Trim().ToUpper();
        oporow["COL9"] = txtLA_TrS.Text.Trim().ToUpper();
        oporow["NUM25"] = fgen.make_double(txtLA_TrL.Text.Trim().ToUpper());
        oporow["NUM26"] = fgen.make_double(txtLA_TrW.Text.Trim().ToUpper());
        oporow["NUM27"] = fgen.make_double(txtLA_TrMRate.Text.Trim().ToUpper());
        oporow["NUM28"] = fgen.make_double(txtLA_TRRate.Text.Trim().ToUpper());
        oporow["NUM29"] = fgen.make_double(txtLATot.Text.Trim().ToUpper());

        // PRINTING
        oporow["COL10"] = txtPR_TCon.Text.Trim().ToUpper();
        oporow["COL11"] = txtPR_TS.Text.Trim().ToUpper();
        oporow["NUM30"] = fgen.make_double(txtPR_TRate.Text.Trim().ToUpper());

        oporow["COL12"] = txtPR_BCon.Text.Trim().ToUpper();
        oporow["COL13"] = txtPR_BS.Text.Trim().ToUpper();
        oporow["NUM31"] = fgen.make_double(txtPR_BRate.Text.Trim().ToUpper());

        oporow["COL14"] = txtPR_TrCon.Text.Trim().ToUpper();
        oporow["COL15"] = txtPR_TrS.Text.Trim().ToUpper();
        oporow["NUM32"] = fgen.make_double(txtPR_TrRate.Text.Trim().ToUpper());
        oporow["NUM33"] = fgen.make_double(txtPRTot.Text.Trim().ToUpper());

        // PRINTING U.V.
        oporow["COL16"] = txtPU_TCon.Text.Trim().ToUpper();
        oporow["COL17"] = txtPU_TS.Text.Trim().ToUpper();
        oporow["NUM34"] = fgen.make_double(txtPU_TRate.Text.Trim().ToUpper());

        oporow["COL18"] = txtPU_BCon.Text.Trim().ToUpper();
        oporow["COL19"] = txtPU_BS.Text.Trim().ToUpper();
        oporow["NUM35"] = fgen.make_double(txtPU_BRate.Text.Trim().ToUpper());

        oporow["COL20"] = txtPU_TrCon.Text.Trim().ToUpper();
        oporow["COL21"] = txtPU_TrS.Text.Trim().ToUpper();
        oporow["NUM36"] = fgen.make_double(txtPU_TrRate.Text.Trim().ToUpper());
        oporow["NUM37"] = fgen.make_double(txtPUTot.Text.Trim().ToUpper());

        // SCREEN PRINTING
        oporow["COL22"] = txtS_TP.Text.Trim().ToUpper();
        oporow["NUM38"] = fgen.make_double(txtS_TRate.Text.Trim().ToUpper());

        oporow["COL23"] = txtS_BP.Text.Trim().ToUpper();
        oporow["NUM39"] = fgen.make_double(txtS_BRate.Text.Trim().ToUpper());

        oporow["COL24"] = txtS_TrP.Text.Trim().ToUpper();
        oporow["NUM40"] = fgen.make_double(txtS_TrRate.Text.Trim().ToUpper());
        oporow["NUM41"] = fgen.make_double(txtSTot.Text.Trim().ToUpper());

        // MICRO
        oporow["COL25"] = txtM_Top.Text.Trim().ToUpper();
        oporow["NUM42"] = fgen.make_double(txtM_TRate.Text.Trim().ToUpper());

        oporow["COL26"] = txtM_Btm.Text.Trim().ToUpper();
        oporow["NUM43"] = fgen.make_double(txtM_BRate.Text.Trim().ToUpper());

        oporow["COL27"] = txtM_Trey.Text.Trim().ToUpper();
        oporow["NUM44"] = fgen.make_double(txtM_TrRate.Text.Trim().ToUpper());
        oporow["NUM45"] = fgen.make_double(txtMTot.Text.Trim().ToUpper());

        // DRIP OFF
        oporow["COL28"] = txtD_TDName.Text.Trim().ToUpper();
        oporow["NUM46"] = fgen.make_double(txtD_TDriff.Text.Trim().ToUpper());
        oporow["NUM47"] = fgen.make_double(txtD_TDM.Text.Trim().ToUpper());
        oporow["NUM48"] = fgen.make_double(txtD_TAmt.Text.Trim().ToUpper());

        oporow["COL29"] = txtD_TGName.Text.Trim().ToUpper();
        oporow["NUM49"] = fgen.make_double(txtD_TGRate.Text.Trim().ToUpper());
        oporow["NUM50"] = fgen.make_double(txtD_TG.Text.Trim().ToUpper());

        oporow["COL30"] = txtD_BDName.Text.Trim().ToUpper();
        oporow["NUM51"] = fgen.make_double(txtD_BDriff.Text.Trim().ToUpper());
        oporow["NUM52"] = fgen.make_double(txtD_BDM.Text.Trim().ToUpper());
        oporow["NUM53"] = fgen.make_double(txtD_BAmt.Text.Trim().ToUpper());

        oporow["COL31"] = txtD_BGName.Text.Trim().ToUpper();
        oporow["NUM54"] = fgen.make_double(txtD_BGRate.Text.Trim().ToUpper());
        oporow["NUM55"] = fgen.make_double(txtD_BG.Text.Trim().ToUpper());

        oporow["COL32"] = txtD_TrDName.Text.Trim().ToUpper();
        oporow["NUM56"] = fgen.make_double(txtD_TrDriff.Text.Trim().ToUpper());
        oporow["NUM57"] = fgen.make_double(txtD_TrDM.Text.Trim().ToUpper());
        oporow["NUM58"] = fgen.make_double(txtD_TrAmt.Text.Trim().ToUpper());

        oporow["COL33"] = txtD_TrGName.Text.Trim().ToUpper();
        oporow["NUM59"] = fgen.make_double(txtD_TrGRate.Text.Trim().ToUpper());
        oporow["NUM60"] = fgen.make_double(txtD_TrG.Text.Trim().ToUpper());

        oporow["NUM61"] = fgen.make_double(txtDTot.Text.Trim().ToUpper());
        oporow["NUM62"] = fgen.make_double(txtGTot.Text.Trim().ToUpper());

        // FOILING PUNCHING SPOT U.V.
        oporow["COL34"] = txtSpot_T.Text.Trim().ToUpper();
        oporow["NUM63"] = fgen.make_double(txtSpot_TRate.Text.Trim().ToUpper());

        oporow["COL35"] = txtSpot_B.Text.Trim().ToUpper();
        oporow["NUM64"] = fgen.make_double(txtSpot_BRate.Text.Trim().ToUpper());

        oporow["COL36"] = txtSpot_Tr.Text.Trim().ToUpper();
        oporow["NUM65"] = fgen.make_double(txtSpot_TrRate.Text.Trim().ToUpper());
        oporow["NUM66"] = fgen.make_double(txtSpot_Tot.Text.Trim().ToUpper());

        // FOILING PUNCHING FOIL
        oporow["COL37"] = txtFoil_T.Text.Trim().ToUpper();
        oporow["NUM67"] = fgen.make_double(txtFoil_TRate.Text.Trim().ToUpper());

        oporow["COL38"] = txtFoil_B.Text.Trim().ToUpper();
        oporow["NUM68"] = fgen.make_double(txtFoil_BRate.Text.Trim().ToUpper());

        oporow["COL39"] = txtFoil_Tr.Text.Trim().ToUpper();
        oporow["NUM69"] = fgen.make_double(txtFoil_TrRate.Text.Trim().ToUpper());
        oporow["NUM70"] = fgen.make_double(txtFoil_Tot.Text.Trim().ToUpper());

        // FOILING PUNCHING PUNCNING
        oporow["COL40"] = txtPunc_T.Text.Trim().ToUpper();
        oporow["NUM71"] = fgen.make_double(txtPunc_TRate.Text.Trim().ToUpper());

        oporow["COL41"] = txtPunc_B.Text.Trim().ToUpper();
        oporow["NUM72"] = fgen.make_double(txtPunc_BRate.Text.Trim().ToUpper());

        oporow["COL42"] = txtPunc_Tr.Text.Trim().ToUpper();
        oporow["NUM73"] = fgen.make_double(txtPunc_TrRate.Text.Trim().ToUpper());
        oporow["NUM74"] = fgen.make_double(txtPunc_Tot.Text.Trim().ToUpper());

        // FOILING PUNCHING EMBOSSING
        oporow["COL43"] = txtEmb_T.Text.Trim().ToUpper();
        oporow["NUM75"] = fgen.make_double(txtEmb_TRate.Text.Trim().ToUpper());

        oporow["COL44"] = txtEmb_B.Text.Trim().ToUpper();
        oporow["NUM76"] = fgen.make_double(txtEmb_BRate.Text.Trim().ToUpper());

        oporow["COL45"] = txtEmb_Tr.Text.Trim().ToUpper();
        oporow["NUM77"] = fgen.make_double(txtEmb_TrRate.Text.Trim().ToUpper());
        oporow["NUM78"] = fgen.make_double(txtEmb_Tot.Text.Trim().ToUpper());

        oporow["GROSSAMT"] = fgen.make_double(txtGrossAmt.Text.Trim().ToUpper());

        // WASTAGE
        oporow["COL46"] = txtW1.Text.Trim().ToUpper();
        oporow["NUM79"] = fgen.make_double(txtW2.Text.Trim().ToUpper());
        oporow["NUM80"] = fgen.make_double(txtW3.Text.Trim().ToUpper());

        // OTHER VALUE
        oporow["COL47"] = txtO1.Text.Trim().ToUpper();
        oporow["NUM81"] = fgen.make_double(txtO2.Text.Trim().ToUpper());
        oporow["NUM82"] = fgen.make_double(txtO3.Text.Trim().ToUpper());

        // DELIVERY CHARGES
        oporow["COL48"] = txtD1.Text.Trim().ToUpper();
        oporow["NUM83"] = fgen.make_double(txtD2.Text.Trim().ToUpper());
        oporow["NUM84"] = fgen.make_double(txtD3.Text.Trim().ToUpper());

        oporow["TOTAL"] = fgen.make_double(txtTot.Text.Trim().ToUpper());

        // PAY TERMS
        oporow["COL49"] = txtPay1.Text.Trim().ToUpper();
        oporow["NUM85"] = fgen.make_double(txtPay2.Text.Trim().ToUpper());
        oporow["COL50"] = txtPay3.Text.Trim().ToUpper();
        oporow["NUM86"] = fgen.make_double(txtPay4.Text.Trim().ToUpper());
        oporow["PAYTOT"] = fgen.make_double(txtPayTot.Text.Trim().ToUpper());

        // TAX
        oporow["COL51"] = txtTax1.Text.Trim().ToUpper();
        oporow["NUM87"] = fgen.make_double(txtTax2.Text.Trim().ToUpper());
        oporow["COL52"] = txtTax3.Text.Trim().ToUpper();
        oporow["NUM88"] = fgen.make_double(txtTax4.Text.Trim().ToUpper());

        oporow["GRANDTOT"] = fgen.make_double(txtGrandTot.Text.Trim().ToUpper());

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
    void save_fun2()
    {

    }
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CP16");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
    }
    //------------------------------------------------------------------------------------   
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Paper ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Paper ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Paper ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "LTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Lamination ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "LBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Lamination ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "LTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Lamination ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PRTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Printing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PRBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Printing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton35_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PRTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Printing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton36_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PUTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Printing U.V", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton37_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PUBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Printing U.V", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton38_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PUTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Printing U.V", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton39_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SPTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Screen Printing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton40_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SPBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Screen Printing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton41_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SPTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Screen Printing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton10_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Micro", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton19_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Micro", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton20_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Micro", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DRTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Drip Off", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton11_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DRBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Drip Off", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton12_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DRTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Drip Off", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton23_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GTOP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Gloss Varnish", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton24_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GBTM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Gloss Varnish", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton25_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GTREY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Gloss Varnish", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TOPSPOT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Top Spot U.V", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TOPFOIL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Top Foil", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TOPPUNCHING";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Top Punching", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton26_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TOPEMBOSSING";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Top Embossing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton27_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTMSPOT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Bottom Spot U.V", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton28_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTMFOIL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Bottom Foil", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton29_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTMPUNCHING";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Bottom Punching", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton30_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTMEMBOSSING";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Bottom Embossing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton31_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TREYSPOT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Trey Spot U.V", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton32_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TREYFOIL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Trey Foil", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton33_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TREYPUNCHING";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Trey Punching", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton34_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TREYEMBOSSING";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Trey Embossing", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "WASTAGE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Wastage", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "OTHERVALUE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Other Value", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton21_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DELIVERYCHARGES";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Delivery Charges", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton22_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PAY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Payment Terms", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TAX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Tax", frm_qstr);
    }        
    //------------------------------------------------------------------------------------
    private void Cal()
    {
        txtPTAmt.Text = (Math.Round((fgen.make_double(txtPTL.Text) / 100) * (fgen.make_double(txtPTW.Text) / 100) * (fgen.make_double(txtPTGSM.Text) / 1000) * fgen.make_double(txtPTRate.Text), 2)).ToString();
        txtPBAmt.Text = (Math.Round((fgen.make_double(txtPBL.Text) / 100) * (fgen.make_double(txtPBW.Text) / 100) * (fgen.make_double(txtPBGSM.Text) / 1000) * fgen.make_double(txtPBRate.Text), 2)).ToString();
        txtPTrAmt.Text = (Math.Round((fgen.make_double(txtPTrL.Text) / 100) * (fgen.make_double(txtPTrW.Text) / 100) * (fgen.make_double(txtPTrGSM.Text) / 1000) * fgen.make_double(txtPTrRate.Text), 2)).ToString();
        txtPTot.Text = (fgen.make_double(txtPTAmt.Text) + fgen.make_double(txtPBAmt.Text) + fgen.make_double(txtPTrAmt.Text)).ToString();

        txtLA_TRate.Text = (Math.Round((fgen.make_double(txtLA_TL.Text) * fgen.make_double(txtLA_TW.Text)) * fgen.make_double(txtLA_TMRate.Text) / 100, 2)).ToString();
        txtLA_BRate.Text = (Math.Round((fgen.make_double(txtLA_BL.Text) * fgen.make_double(txtLA_BW.Text)) * fgen.make_double(txtLA_BMRate.Text) / 100, 2)).ToString();
        txtLA_TRRate.Text = (Math.Round((fgen.make_double(txtLA_TrL.Text) * fgen.make_double(txtLA_TrW.Text)) * fgen.make_double(txtLA_TrMRate.Text) / 100, 2)).ToString();
        txtLATot.Text = (fgen.make_double(txtLA_TRate.Text) + fgen.make_double(txtLA_BRate.Text) + fgen.make_double(txtLA_TRRate.Text)).ToString();

        txtPRTot.Text = (fgen.make_double(txtPR_TRate.Text) + fgen.make_double(txtPR_BRate.Text) + fgen.make_double(txtPR_TrRate.Text)).ToString();

        txtPUTot.Text = (fgen.make_double(txtPU_TRate.Text) + fgen.make_double(txtPU_BRate.Text) + fgen.make_double(txtPU_TrRate.Text)).ToString();

        txtSTot.Text = (fgen.make_double(txtS_TRate.Text) + fgen.make_double(txtS_BRate.Text) + fgen.make_double(txtS_TrRate.Text)).ToString();

        txtMTot.Text = (fgen.make_double(txtM_TRate.Text) + fgen.make_double(txtM_BRate.Text) + fgen.make_double(txtM_TrRate.Text)).ToString();

        txtD_TDriff.Text = (Math.Round((fgen.make_double(txtPTL.Text) * fgen.make_double(txtPTW.Text)) / 64500, 3)).ToString();
        txtD_TAmt.Text = (Math.Round(fgen.make_double(txtD_TDriff.Text) * fgen.make_double(txtD_TDM.Text), 3)).ToString();
        txtD_TG.Text = (Math.Round(fgen.make_double(txtD_TDriff.Text) * fgen.make_double(txtD_TGRate.Text), 3)).ToString();
        txtDTot.Text = (fgen.make_double(txtD_TAmt.Text) + fgen.make_double(txtD_BAmt.Text) + fgen.make_double(txtD_TrAmt.Text)).ToString();
        txtGTot.Text = (fgen.make_double(txtD_TG.Text) + fgen.make_double(txtD_BG.Text) + fgen.make_double(txtD_TrG.Text)).ToString();

        txtD_BDriff.Text = (Math.Round((fgen.make_double(txtPTL.Text) * fgen.make_double(txtPTW.Text)) / 64500, 3)).ToString();
        txtD_BAmt.Text = (Math.Round(fgen.make_double(txtD_BDriff.Text) * fgen.make_double(txtD_BDM.Text), 3)).ToString();
        txtD_BG.Text = (Math.Round(fgen.make_double(txtD_BDriff.Text) * fgen.make_double(txtD_BGRate.Text), 3)).ToString();
        txtDTot.Text = (fgen.make_double(txtD_TAmt.Text) + fgen.make_double(txtD_BAmt.Text) + fgen.make_double(txtD_TrAmt.Text)).ToString();

        txtD_TrDriff.Text = (Math.Round((fgen.make_double(txtPTL.Text) * fgen.make_double(txtPTW.Text)) / 64500, 3)).ToString();
        txtD_TrAmt.Text = (Math.Round(fgen.make_double(txtD_TrDriff.Text) * fgen.make_double(txtD_TrDM.Text), 3)).ToString();
        txtD_TrG.Text = (Math.Round(fgen.make_double(txtD_TrDriff.Text) * fgen.make_double(txtD_TrGRate.Text), 3)).ToString();
        txtDTot.Text = (fgen.make_double(txtD_TAmt.Text) + fgen.make_double(txtD_BAmt.Text) + fgen.make_double(txtD_TrAmt.Text)).ToString();
        txtGTot.Text = (fgen.make_double(txtD_TG.Text) + fgen.make_double(txtD_BG.Text) + fgen.make_double(txtD_TrG.Text)).ToString();

        txtSpot_Tot.Text = (fgen.make_double(txtSpot_TRate.Text) + fgen.make_double(txtSpot_BRate.Text) + fgen.make_double(txtSpot_TrRate.Text)).ToString();

        txtFoil_Tot.Text = (fgen.make_double(txtFoil_TRate.Text) + fgen.make_double(txtFoil_BRate.Text) + fgen.make_double(txtFoil_TrRate.Text)).ToString();

        txtPunc_Tot.Text = (fgen.make_double(txtPunc_TRate.Text) + fgen.make_double(txtPunc_BRate.Text) + fgen.make_double(txtPunc_TrRate.Text)).ToString();

        txtEmb_Tot.Text = (fgen.make_double(txtEmb_TRate.Text) + fgen.make_double(txtEmb_BRate.Text) + fgen.make_double(txtEmb_TrRate.Text)).ToString();

        txtGrossAmt.Text = (fgen.make_double(txtPTot.Text) + fgen.make_double(txtLATot.Text) + fgen.make_double(txtPRTot.Text) + fgen.make_double(txtPUTot.Text) + fgen.make_double(txtSTot.Text) + fgen.make_double(txtDTot.Text) + fgen.make_double(txtGTot.Text) + fgen.make_double(txtMTot.Text) + fgen.make_double(txtSpot_Tot.Text) + fgen.make_double(txtFoil_Tot.Text) + fgen.make_double(txtPunc_Tot.Text) + fgen.make_double(txtEmb_Tot.Text)).ToString();

        txtW3.Text = (Math.Round((fgen.make_double(txtGrossAmt.Text) * fgen.make_double(txtW2.Text)) / 100, 2)).ToString();

        txtO3.Text = (Math.Round((fgen.make_double(txtGrossAmt.Text) * fgen.make_double(txtO2.Text)) / 100,2)).ToString();

        txtTot.Text = (fgen.make_double(txtGrossAmt.Text) + fgen.make_double(txtW3.Text) + fgen.make_double(txtO3.Text) + fgen.make_double(txtD3.Text)).ToString();

        txtPay4.Text = (Math.Round((fgen.make_double(txtPay2.Text) / 100) * fgen.make_double(txtTot.Text), 2)).ToString();
        txtPayTot.Text = (Math.Round(fgen.make_double(txtPay4.Text) + fgen.make_double(txtTot.Text), 2)).ToString();

        txtTax4.Text = (Math.Round((fgen.make_double(txtTax2.Text) / 100) * fgen.make_double(txtPayTot.Text), 2)).ToString();
        txtGrandTot.Text = (Math.Round(fgen.make_double(txtTax4.Text) + fgen.make_double(txtPayTot.Text), 2)).ToString();
    }
    //------------------------------------------------------------------------------------
    protected void btnCal_ServerClick(object sender, EventArgs e)
    {
        Cal();
        btnsave.Disabled = false;
    }
    //------------------------------------------------------------------------------------   
}