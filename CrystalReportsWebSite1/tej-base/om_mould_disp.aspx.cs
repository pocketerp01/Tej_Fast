using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_mould_disp : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col7, vardate, fromdt, todt, next_year, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    int mFlag = 0;
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
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    next_year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            set_Val();
            
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

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        lblheader.Text = "Mould Disposed Entry";
        frm_tabname = "WB_MASTER";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM20");
        frm_vty = "MM20";
        switch (frm_formID)
        {
            case "F75208": //Mould disposed
                SQuery = "select a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,trim(a.col1) mould_code,trim(b.name) as mould_name,a.cpartno as mould_id,a.col13 as done_by,a.naration as reason_for_deactivation,to_char(a.vchdate,'yyyymmdd') as vdd from wb_master a,typegrp b where trim(a.branchcd)||trim(a.col1)=trim(b.branchcd)||trim(b.type1) and a.branchcd='" + frm_mbr + "' and a.id='MM20' and b.id='MM' order by a.vchnum desc,vdd desc";
                break;
        }
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1.DataSource = dt;
        sg1.DataBind();
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (frm_ulvl == "3") cond = " and trim(a.ENT_BY)='" + frm_uname + "'";
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR.Trim() + "'";
        switch (btnval)
        {            
            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;           

            case "Item":
                SQuery = "SELECT TRIM(A.COL1) AS FSTR,A.COL1 AS MOULD_CODE,B.NAME AS MOULD_NAME,A.CPARTNO AS MOULD_ID FROM WB_MASTER A,TYPEGRP B WHERE  TRIM(A.BRANCHCD)||TRIM(A.COL1)=TRIM(B.BRANCHCD)||TRIM(B.TYPE1) AND A.BRANCHCD='" + frm_mbr + "' AND A.ID='MM01' AND NVL(TRIM(A.COL2),'-') != 'Y' ORDER BY COL1";                
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "SELECT trim(a.vchnum)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') as fstr, A.COL1 AS MOULD_ID,trim(a.vchnum) as Entry_no,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') as entry_dt,B.NAME FROM WB_MASTER A, TYPEGRP B WHERE TRIM(A.BRANCHCD)||trim(a.col1)=TRIM(B.BRANCHCD)||trim(b.type1) AND A.BRANCHCD='" + frm_mbr + "' AND a.ID='" + frm_vty + "' AND b.id='MM' ORDER BY TRIM(a.vchnum) DESC";
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
        //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "CSS : Query has been logged " + frm_vnum, html_body);
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            // else comment upper code 
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        //frm_vty = vty;
        //lbl1a.Text = vty;
        //string mq0 = "";
        ////mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "";
        ////for mould master
        //mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND type='" + frm_vty + "' and vchdate " + DateRange + "";
        //frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq0, 6, "VCH");

        //txtvchnum.Value = frm_vnum;
        //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        vty = "MM20";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND id='" + frm_vty + "'", 6, "VCH");
        txtvchnum.Value = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        disablectrl(); btnitem.Enabled = true; btnmrr.Focus();
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

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return;
        }

        if (txt_qty_rcv.Value.Length <= 2)
        {
            fgen.msg("-", "AMSG", "Please Fill Reason For Deactivation"); return;
        }
        if (txt_sample_tak.Value.Length <= 2)
        {
            fgen.msg("-", "AMSG", "Please Done By!!"); return;
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
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
        sg1.DataSource = null;        
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        SQuery = "select a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,trim(a.col1) mould_code,trim(b.name) as mould_name,a.cpartno as mould_id,a.col13 as done_by,a.naration as reason_for_deactivation from wb_master a,typegrp b where trim(a.branchcd)||trim(a.col1)=trim(b.branchcd)||trim(b.type1) and a.branchcd='" + frm_mbr + "' and a.id='MM20' and b.id='MM'";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim(), frm_qstr);
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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
            {
                btnval = CP_BTN;
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "0");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "0");

        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.id||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE WB_MASTER SET COL2='N', COL10='-', COL13='-' WHERE branchcd='" + frm_mbr + "' and id='MM01' and TRIM(COL1)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "'");
                // Saving Deleting History

                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0,6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6,10), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
               // fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
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

                case "mrr":
                    if (col1 == "") return;
                    SQuery = "select trim(a.vchnum) as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_date,trim(a.acode) as acode,trim(b.aname) as supplier,trim(a.icode) as icode,trim(a.invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,a.iqtyin as qty from ivoucher a , famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%'  and trim(a.vchnum)||to_chaR(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtmrrno.Value = dt.Rows[0]["mrr_no"].ToString().Trim();
                        txtmrrdate.Text = dt.Rows[0]["mrr_date"].ToString().Trim();
                        txtbill_no.Text = dt.Rows[0]["invno"].ToString().Trim();
                        txtbill_date.Text = dt.Rows[0]["invdate"].ToString().Trim();
                        txtsuppname.Value = dt.Rows[0]["supplier"].ToString().Trim();
                        txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                    }
                    btnitem.Focus();
                    //txtcustname.Value = col2;
                    break;
                case "Item":
                    if (col1 == "") return;
                    dt = new DataTable();
                    SQuery = "SELECT TRIM(A.COL1) AS MOULD_CODE,TRIM(B.NAME) AS MOULD_NAME,A.CPARTNO AS MOULD_ID,A.COL13 FROM WB_MASTER A , TYPEGRP B WHERE TRIM(A.BRANCHCD)||TRIM(A.COL1)=TRIM(B.BRANCHCD)||TRIM(B.TYPE1) AND  A.BRANCHCD='" + frm_mbr + "' AND A.ID='MM01' AND b.id='MM' AND TRIM(A.COL1)='" + col1 + "' ORDER BY A.COL1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txticode.Value = dt.Rows[0]["MOULD_CODE"].ToString().Trim();
                        txtitmname.Value = dt.Rows[0]["MOULD_NAME"].ToString().Trim();
                        txtacode.Value = dt.Rows[0]["MOULD_ID"].ToString().Trim();
                        //txtsuppname.Value = dt.Rows[0]["fstr"].ToString().Trim();
                    }
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

                    SQuery = "SELECT A.VCHNUM,A.VCHDATE,A.COL1,A.COL13,B.NAME,A.CPARTNO,A.NARATION,A.ent_by,A.ent_dt FROM WB_MASTER A, TYPEGRP B WHERE trim(a.col1)=trim(b.type1) AND A.BRANCHCD='" + frm_mbr + "' AND a.ID='" + frm_vty + "' AND b.id='MM' AND trim(a.vchnum)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "' ORDER BY TRIM(a.vchnum) DESC";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Value = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                       
                        txticode.Value = dt.Rows[0]["COL1"].ToString().Trim();
                        txtitmname.Value = dt.Rows[0]["NAME"].ToString().Trim();
                        txtacode.Value = dt.Rows[0]["CPARTNO"].ToString().Trim();
                        txt_sample_tak.Value = dt.Rows[0]["COL13"].ToString().Trim();
                        txt_qty_rcv.Value = dt.Rows[0]["NARATION"].ToString().Trim();
                                               
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        txtvchnum.Disabled = true;
                        txtvchdate.Enabled = false;
                        btnitem.Enabled = false;

                    }
                    #endregion
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "AK12");
                    fgen.fin_maint_reps(frm_qstr);
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
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
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select a.vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,trim(a.col1) mould_code,trim(b.name) as mould_name,a.cpartno as mould_id,a.col13 as done_by,a.naration as reason_for_deactivation from wb_master a,typegrp b where trim(a.branchcd)||trim(a.col1)=trim(b.branchcd)||trim(a.type1) and a.branchcd='"+frm_mbr+"' and a.id='"+frm_vty+"' and b.id='MM'";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and id='" + lbl1a_Text + "'  ", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    Checked_ok = "Y";
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("dd/MM/yyyy") + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                //Checked_ok = "N";
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("dd/MM/yyyy") + " ,Please Check !!");
            }
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
                            frm_vnum = txtvchnum.Value.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";

                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and id='" + frm_vty + "' ", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("yyyy-MM-dd"), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and id='" + frm_vty + "' ", 6, "vch");
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

                        if (edmode.Value == "Y")
                        {
                            //ddl_fld1 = fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                            //string type_depr = "40";
                            //ddl_fld2 = fgenMV.Fn_Get_Mvar(frm_qstr,"" ).Substring(0, 2) + type_depr + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr").Substring(3, 17);
                            string mycmd = "";
                            mycmd = "update " + frm_tabname + " set branchcd='DD' where branchcd||id||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_mbr + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        string mq0;
                        mq0 = "UPDATE WB_MASTER SET COL2='Y', COL10='" + txtvchdate.Text.Trim() + "', COL13='" + txt_sample_tak.Value + "' WHERE BRANCHCD='" + frm_mbr + "' AND id='MM01' AND TRIM(COL1)='" + txticode.Value.Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE WB_MASTER SET COL2='Y', COL10='" + txtvchdate.Text.Trim() + "', COL13='" + txt_sample_tak.Value + "' WHERE BRANCHCD='" + frm_mbr + "' AND id='MM01' AND TRIM(COL1)='" + txticode.Value.Trim() + "'");

                        if (edmode.Value == "Y")
                        {
                            string mycmd2 = "";
                            mycmd2 = "delete from " + frm_tabname + " where branchcd='DD' and id||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd2);
                        }
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value + " Saved Successfully!!");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); set_Val();
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

    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();

        oporow["BRANCHCD"] = frm_mbr;
        oporow["id"] = frm_vty;
        oporow["vchnum"] = txtvchnum.Value; // ENTRY NO
        oporow["VCHDATE"] = fgen.make_def_Date(txtvchdate.Text.Trim(), vardate); // ENTRY DATE
        oporow["acode"] = "-"; // PARTY CODE
        oporow["icode"] = "-"; // ITEM CODE
        oporow["cpartno"] = txtacode.Value.ToUpper().Trim(); // ACREF OF TYPEGRP
        oporow["name"] = "-"; // MODEL NAME
        oporow["col1"] = txticode.Value.ToUpper().Trim(); // TYPE1 OF TYPEGRP

        oporow["col2"] = "Y";
        oporow["col4"] = "-";
        oporow["col5"] = "-";
        oporow["col6"] = "-";
        oporow["col7"] = "-";
        oporow["col8"] = "-";
        oporow["col9"] = "-";

        oporow["col10"] = "-";
        oporow["col11"] = "-";
        oporow["col12"] = "-";
        oporow["col13"] = txt_sample_tak.Value.Trim().ToString().ToUpper();
        oporow["col14"] = "-";
        oporow["col15"] = "-";
        oporow["date1"] = vardate;
        oporow["num1"] = 0;
        oporow["num2"] = 0;
        oporow["num3"] = 0;
        oporow["num4"] = 0;
        oporow["num5"] = 0;
        oporow["num6"] = 0;
        oporow["num7"] = 0;
        oporow["num8"] = 0;
        oporow["num9"] = 0;
        oporow["num10"] = 0;
        oporow["num11"] = 0;
        oporow["num12"] = 0;
        oporow["num13"] = 0;
        oporow["num14"] = 0;
        oporow["num15"] = 0;

        oporow["remarks"] = "-";
        oporow["IMAGEF"] = "-";
        oporow["imagepath"] = "-";

        oporow["naration"] = txt_qty_rcv.Value.Trim().ToString().ToUpper();

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
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    }

    //------------------------------------------------------------------------------------
    protected void btnmrr_click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "mrr";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select MRR", frm_qstr);
    }

    protected void btnitem_Click(object sender, ImageClickEventArgs e)
    {       
        hffield.Value = "Item";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Mould", frm_qstr);
    }

    protected void btnprod_click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Prod";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Product", frm_qstr);
    }
}