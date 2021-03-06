using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Drawing;

public partial class om_multi_item_upt : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, nVty = "";
    DataTable dt, dt2, dt3, dt4, siname;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0, flag = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
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
                    if (frm_qstr.Contains("^"))
                    {
                        if (frm_cocd != frm_qstr.Split('^')[0].ToString())
                        {
                            frm_cocd = frm_qstr.Split('^')[0].ToString();
                        }
                    }
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
                btnedit.Visible = false;
                DataTable dtW = (DataTable)ViewState["dtn"];
                if (dtW != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtW, "", "ContentPlaceHolder1_datadiv").ToString(), false);
                }
            }
            setColHeadings();
            set_Val();
            btnprint.Visible = false;
            btnsave.Visible = false;
            btndel.Visible = false;
            btnlist.Visible = false;
            btnexptoexl.Visible = false;
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
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btnvalidate.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnupdate.Disabled = true;
        ImageButton_upt.Enabled = false; btnAcode.Enabled = false; btnRcode.Enabled = false; ImageButton1.Enabled = false; // BY MADHVI ON 28 MARCH 2019
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        ImageButton_upt.Enabled = true; btnAcode.Enabled = true; btnRcode.Enabled = true; ImageButton1.Enabled = true; // BY MADHVI ON 28 MARCH 2019
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
        doc_nf.Value = "icode";
        doc_df.Value = "icode";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "item";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F25217":
                lblheader.Text = "Item Master Balance Update";  // BY MADHVI ON 28 MARCH 2019
                break;

            case "F25219":
                lblheader.Text = "Item Master Min/Max/ROL Update";  // BY MADHVI ON 28 MARCH 2019
                Label3.Visible = false;
                ImageButton_upt.Visible = false;
                txtuptcol.Visible = false;
                Label9.Visible = false; btnAcode.Visible = false; txtmax.Visible = false;
                Label1.Visible = false; btnRcode.Visible = false; txtirate.Visible = false;
                Label2.Visible = false; ImageButton1.Visible = false; txtbinno.Visible = false;
                break;

            case "F25220":
                lblheader.Text = "Item Master Rate Update";
                Label3.Visible = false;
                ImageButton_upt.Visible = false;
                txtuptcol.Visible = false;
                Label9.Visible = false; btnAcode.Visible = false; txtmax.Visible = false;
                Label1.Visible = false; btnRcode.Visible = false; txtirate.Visible = false;
                Label2.Visible = false; ImageButton1.Visible = false; txtbinno.Visible = false;
                break;

            case "F10175":
                lblheader.Text = "Item Master Family Update";
                Label3.Visible = false;
                ImageButton_upt.Visible = false;
                txtuptcol.Visible = false;
                Label9.Visible = false; btnAcode.Visible = false; txtmax.Visible = false;
                Label1.Visible = false; btnRcode.Visible = false; txtirate.Visible = false;
                Label2.Visible = false; ImageButton1.Visible = false; txtbinno.Visible = false;
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "ZZ");
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
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                SQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name ";
                break;

            case "TACODE":
                SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '16%' order by acode";
                break;

            case "TRCODE":
                SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Y' /*and type1 not like '9%'*/ order by type1";
                break;

            case "SELBRANCHMAX":
                SQuery = "select 'Branchwise' as fstr,'BranchWise' as choice , 'BranchWise' as updated from dual union all  select 'HeadOffice' as fstr,'HeadOffice' as choice , 'HeadOffice'  as updated from dual";
                break;
            case "SELBRANCHIRATE":
                SQuery = "select 'Branchwise' as fstr,'BranchWise' as choice , 'BranchWise' as updated from dual union all  select 'HeadOffice' as fstr,'HeadOffice' as choice , 'HeadOffice'  as updated from dual";
                break;

            case "SELBRANCHBINNO":
                SQuery = "select 'Branchwise' as fstr,'BranchWise' as choice , 'BranchWise' as updated from dual union all  select 'HeadOffice' as fstr,'HeadOffice' as choice , 'HeadOffice'  as updated from dual";
                break;

            case "UPCOLUM":
                SQuery = "select 'iname' as CHOOSEFIELD , 'ITEM_NAME' as CHOOSEFIELD1 from dual   union all select 'hscode','HSCODE' from dual union all select 'cpartno','PART_NO'  from dual UNION ALL SELECT 'cdrgno','DRG_NO' from dual union all select 'ciname','ITEM_NAME_CUST' from dual union all select 'unit','PRIMARY_UNIT' from dual union all select 'irate' ,'STD_RATE' from dual union all select 'abc_class','A_B_C_CLASS' from dual union all select 'binno','LOCT' from dual union all select 'icat','CAT' from dual union all select 'iweight','GROSS_WT'  from dual union all select 'wt_net' ,'NET_WT'  from dual union all select 'servicable','CRITICAL_ITEM'  from dual union all select 'maker','BRAND_OR_REF'  from dual union all select 'packsize','STANDARD_PACKING' from dual union all select 'default_us','SHELF_LIFE_DAYS' from dual union all select 'imax','MAXIMUM' from dual union select 'imin','MINIMUM' FROM DUAL UNION ALL SELECT 'iord' ,'IORD' from  dual";
                break;

            case "New":
            case "List":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.col33 as pono,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (Prg_Id == "F25220" || Prg_Id == "F10175")
        {
            if (frm_mbr != "00")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
                return;
            }
        }
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        FileUpload1.Enabled = true;
        if (chk_rights == "Y")
        {
            frm_vty = "ZZ";
            btnexport.Disabled = true; // BY MADHVI ON 21 JULY 2018
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);
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
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());

        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

        DataView dv = new DataView(dtn);

        string crFound = "N";



        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        hfCNote.Value = "Y";
        if (txtmax.Value.ToString().ToUpper().Contains("MARUTI"))
        {
            hffield.Value = "SAVE";
            fgen.msg("-", "CMSG", "Do You want to Make Credit Note too!!'13'(Select No for Debit Note Only)");
        }
        else fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
            hffield.Value = "Del_E";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " ", frm_qstr);
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
        FileUpload1.Enabled = false;
        enablectrl();
        setColHeadings();
        btnexport.Disabled = false; // BY MADHVI ON 21 JULY 2018
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
                // Saving Updating History
                //fgen.save_info_mac(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Updated");
                fgen.msg("-", "AMSG", "Entry updated For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
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
        else if (hffield.Value == "SAVE")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y") hfCNote.Value = "Y";
            else hfCNote.Value = "N";
            DataTable dtn = new DataTable();
            dtn = (DataTable)ViewState["dtn"];
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
                    break;

                case "New":
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    //txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    // Popup asking for Copy from Older Data
                    fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    hffield.Value = "NEW_E";
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
                    SQuery = "Select a.*,b.Name as TM_Name,c.Name as CL_Name,d.name as Ef_Name from " + frm_tabname + " a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        // txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

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
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;

                case "TACODE":
                    txtirate.Value = col1;
                    txtmax.Value = col2;
                    break;

                case "SELBRANCHMAX":// THESE ARE SOME MODIFICATIONS SO I MA TAKING SAME TEXTBOX NAME OTHERWISE WHOLE CODE WILL CHANGE.
                    txtmax.Value = col2;
                    break;

                case "SELBRANCHIRATE":
                    txtirate.Value = col2;
                    break;

                case "SELBRANCHBINNO":
                    txtbinno.Value = col2;
                    break;

                case "TRCODE":
                    switch (frm_formID)
                    {
                        case "F10175":
                            SQuery = "SELECT ICODE AS ITEMCODE, INAME ,HSCODE,CPARTNO , CDRGNO ,CINAME ,UNIT ,NO_PROC ,IRATE ,ABC_CLASS ,BINNO,ICAT,IWEIGHT ,WT_NET ,SERVICABLE,MAKER ,PACKSIZE ,DEFAULT_US ,IMAX,IMIN ,IORD,BFACTOR FROM ITEM WHERE TRIM(SUBSTR(ICODE,1,2)) IN (" + col1 + ") and length(trim(icode))='8' order by icode";
                            break;

                        default:
                            SQuery = "SELECT ICODE AS ITEMCODE, INAME ,HSCODE,CPARTNO , CDRGNO ,CINAME ,UNIT ,NO_PROC ,IRATE ,ABC_CLASS ,BINNO,ICAT,IWEIGHT ,WT_NET ,SERVICABLE,MAKER ,PACKSIZE ,DEFAULT_US ,IMAX,IMIN ,IORD FROM ITEM WHERE TRIM(SUBSTR(ICODE,1,2)) IN (" + col1 + ") and length(trim(icode))>='8' order by icode";
                            break;
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.Columns.Add("SRNO", typeof(int)).SetOrdinal(0);
                    if (dt.Rows.Count > 0)
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("Download the Excel Format and don't change the Columns Positions", frm_qstr); // BY MADHVI ON 21 JULY 2018
                    }
                    else fgen.msg("-", "AMSG", "No Data to Export");
                    dt.Dispose();
                    break;

                case "UPCOLUM":
                    txtuptcol.Value = col1;
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
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
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

                        if (edmode.Value == "Y")
                        {
                            //frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "N";
                            save_it = "Y";


                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        ViewState["refNo"] = frm_vnum;
                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Updated");
                        }

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", "Data Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", "Data Saved Successfully");
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
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "srno"; // to change
            dtW = new DataTable();
            dtW = dvW.ToTable();

            // string icode = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(icode)+1),8,'0') as existcd from item where branchcd!='DD' and substr(icode,1,4)='" + gr1["subgp"] + "' and length(Trim(icode))>4  ", "existcd");


            foreach (DataRow gr1 in dtW.Rows)
            {
                oporow = oDS.Tables[0].NewRow();


                if (edmode.Value == "Y")
                {

                }
                else
                {
                    string chk_code;
                    string acnat;
                    acnat = gr1["LEDGGP"].ToString().Trim();
                    switch (acnat)
                    {
                        case "05":
                        case "06":
                        case "16":
                        case "02":
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select max(substr(acode,4,3)) as existcd from famst where branchcd!='DD' and substr(acode,3,1)='" + gr1["ACCT_NAME"].ToString().Trim().Substring(0, 1) + "' and substr(acode,1,2)='" + gr1["LEDGGP"].ToString().Trim() + "' ", "existcd");
                            break;
                        default:
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select max(substr(acode,3,4)) as existcd from famst where branchcd!='DD' and substr(acode,1,2)='" + gr1["LEDGGP"].ToString().Trim() + "' ", "existcd");
                            break;
                    }

                    if (chk_code == "0")
                    {
                        if (acnat == "05" || acnat == "06" || acnat == "16" || acnat == "02")
                        {
                            oporow["acode"] = acnat + gr1["ACCT_NAME"].ToString().Trim().Substring(0, 1) + "001";
                        }
                        else
                        {
                            oporow["acode"] = acnat + "0001";
                        }
                    }
                    else
                    {
                        if (acnat == "05" || acnat == "06" || acnat == "16" || acnat == "02")
                        {
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(substr(acode,4,3))+1),3,'0') as existcd from famst where branchcd!='DD' and substr(acode,3,1)='" + gr1["ACCT_NAME"].ToString().Trim().Substring(0, 1) + "' and substr(acode,1,2)='" + gr1["LEDGGP"].ToString().Trim() + "' ", "existcd");
                            oporow["acode"] = acnat + gr1["ACCT_NAME"].ToString().Trim().Substring(0, 1) + chk_code;
                        }
                        else
                        {
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(substr(acode,3,4))+1),4,'0') as existcd from famst where branchcd!='DD' and substr(acode,1,2)='" + gr1["LEDGGP"].ToString().Trim() + "' ", "existcd");
                            oporow["acode"] = acnat + chk_code;
                        }

                    }
                }
                oporow["BRANCHCD"] = frm_mbr;

                //CHANGED CODE FOR GRID

                oporow["grp"] = gr1["LEDGGP"].ToString().Trim();
                oporow["bssch"] = "-";

                oporow["pname"] = gr1["ALIAS_NAME"].ToString().Trim();
                oporow["girno"] = gr1["PAN_NO"].ToString().Trim();
                oporow["aname"] = gr1["ACCT_NAME"].ToString().Trim();
                oporow["district"] = "-";
                oporow["staten"] = gr1["STATE_NAME"].ToString().Trim();
                oporow["zoname"] = "-";
                oporow["country"] = gr1["COUNTRY"].ToString().Trim();
                oporow["continent"] = "-";


                oporow["addr1"] = gr1["ADDR1"].ToString().Trim();
                oporow["addr2"] = gr1["ADDR2"].ToString().Trim();
                oporow["addr3"] = gr1["ADDR3"].ToString().Trim();
                oporow["addr4"] = gr1["ADDR4"].ToString().Trim();

                oporow["telnum"] = "-";


                oporow["email"] = gr1["EMAILID"].ToString().Trim();
                oporow["email2"] = "-";
                oporow["person"] = gr1["CONTACT_PERSON"].ToString().Trim();
                oporow["mobile"] = gr1["MOBILE"].ToString().Trim();
                oporow["cin_no"] = gr1["CIN_NO"].ToString().Trim();
                oporow["gst_no"] = gr1["GST_NO"].ToString().Trim();
                oporow["gstoversea"] = "-";
                oporow["gstperson"] = gr1["COM_ACNT"].ToString().Trim();

                oporow["costcontrol"] = "-";
                oporow["STDRate"] = 0;
                oporow["dlvtime"] = 0;
                oporow["rateint"] = 0;
                oporow["RTG_Bank"] = "-";
                oporow["RTG_Acty"] = "-";
                //oporow["col13"] = gr1["sheetno"].ToString().Trim();

                oporow["RTG_Addr"] = "-";
                oporow["RTG_acno"] = "-";
                oporow["RTG_IFSC"] = "-";
                oporow["RTG_Swift"] = "-";

                oporow["Rtg_tel"] = "-";
                oporow["Payment"] = 0;
                oporow["Pay_num"] = 0;
                oporow["Balop"] = 0;
                oporow["climit"] = 0;
                oporow["del_term"] = "-";
                oporow["del_note"] = "-";
                oporow["oth_notes"] = "-";
                oporow["med_lic"] = "-";
                oporow["vencode"] = "-";
                oporow["buycode"] = "-";
                oporow["mktggrp"] = "-";
                oporow["custgrp"] = "-";
                oporow["tdsrate"] = 0;
                oporow["cessrate"] = 0;
                oporow["stkbal"] = 0;

                oporow["schgrate"] = 0;
                oporow["disc"] = 0;
                oporow["gstrating"] = 0;
                oporow["gstna"] = "-";

                oporow["gstpvexp"] = "-";


                oporow["hubstk"] = "-";
                oporow["hr_ml"] = "-";
                oporow["so_tolr"] = 0;

                oporow["dlno"] = "-";
                oporow["asa"] = "-";



                //if (hfCNote.Value == "Y") oporow["NUM10"] = 1;
                //else
                //{
                //    if (fgen.make_double(gr1["rrate"].ToString().Trim()) > 0) oporow["NUM10"] = 1;
                //    else oporow["NUM10"] = 0;
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
                fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                oDS.Dispose();
                oporow = null;
                oDS = new DataSet();
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
            }
        }
    }

    /*
    void save_fun2()
    {
        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "", schg_code = "", iopr = "", nVty = "";
        double dVal = 0; double dVal1 = 0; double dVal2 = 0; double qty = 0;
        DataTable dtSale = new DataTable();
        dtSale = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct branchcd,TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY') AS FSTR FROM SALE WHERE BRANCHCD!='DD' AND TYPE LIKE '4%' AND VCHDATE " + DateRange + " order by fstr ");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "icode";
            dtW = new DataTable();
            dtW = dvW.ToTable();

            int l = 1;
            string mhd = "";
            string saveTo = "Y";
            foreach (DataRow drw in dtW.Rows)
            {
                if (hfCNote.Value == "N")
                {
                    if (fgen.make_double(drw["diff"].ToString().Trim()) < 0) saveTo = "N";
                    else saveTo = "Y";
                }
                if (fgen.make_double(drw["diff"].ToString().Trim()) == 0) saveTo = "N";
                saveTo = "Y";
                if (saveTo == "Y")
                {
                    #region Complete Save Function
                    mhd = fgen.seek_iname_dt(dtSale, "fstr='" + fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6) + Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy") + "'", "branchcd");
                    if (mhd != "0")
                    {
                        string branchcd = mhd;
                        string invRmrk = "";
                        oDS1 = new DataSet();
                        oporow1 = null;
                        oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");

                        dVal = 0;
                        dVal1 = 0;
                        dVal2 = 0;

                        //*******************

                        oporow1 = oDS1.Tables[0].NewRow();
                        oporow1["BRANCHCD"] = branchcd;

                        if (fgen.make_double(drw["diff"].ToString().Trim()) > 0) nVty = "59";
                        else nVty = "58";
                        nVty = "59";

                        oporow1["TYPE"] = nVty;

                        i = 0;
                        do
                        {
                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + i + " as vch from IVOUCHER where branchcd='" + branchcd + "' and type='" + nVty + "' and VCHDATE " + DateRange + "", 6, "vch");
                            pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + branchcd + nVty + frm_vnum + frm_CDT1, branchcd, nVty, frm_vnum, txtvchdate.Text.Trim(), drw["pono"].ToString().Trim(), frm_uname);
                            if (i > 20)
                            {
                                fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + 0 + " as vch from IVOUCHER where branchcd='" + branchcd + "' and type='" + nVty + "' and VCHDATE " + DateRange + "", 6, "vch");
                                pk_error = "N";
                                i = 0;
                            }
                            i++;
                        }
                        while (pk_error == "Y");
                        string batchNo = drw["pono"].ToString().Trim();

                        oporow1["LOCATION"] = batchNo;

                        oporow1["vchnum"] = frm_vnum;
                        oporow1["vchdate"] = txtvchdate.Text.Trim();

                        oporow1["ACODE"] = txtirate.Value.Trim();
                        oporow1["VCODE"] = txtirate.Value.ToString().Trim();
                        oporow1["ICODE"] = drw["icode"].ToString().Trim();

                        oporow1["REC_ISS"] = "C";

                        oporow1["IQTY_CHL"] = drw["iqtyout"].ToString().Trim();
                        qty = fgen.make_double(drw["iqtyout"].ToString().Trim());
                        oporow1["PURPOSE"] = drw["iname"].ToString().Trim();

                        invRmrk = "PO No. :" + batchNo;
                        invRmrk = drw["remarks"].ToString().Trim() + " " + txtrmk.Text.Trim();
                        oporow1["NARATION"] = invRmrk;

                        oporow1["finvno"] = drw["PONO"].ToString().Trim();
                        oporow1["PODATE"] = Convert.ToDateTime(drw["PODT"].ToString().Trim()).ToString("dd/MM/yyyy");

                        oporow1["INVNO"] = fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6);
                        oporow1["INVDATE"] = Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy");

                        oporow1["UNIT"] = "NOS";

                        double Rate = fgen.make_double(drw["rrate"].ToString().Trim()) - fgen.make_double(drw["oldrate"].ToString().Trim());
                        if (Rate < 0) Rate = -1 * Rate;
                        oporow1["IRATE"] = Rate;

                        dVal = Math.Round(fgen.make_double(drw["iqtyout"].ToString().Trim()) * Rate, 2);
                        if (dVal < 0) dVal = -1 * dVal;
                        oporow1["IAMOUNT"] = dVal;

                        oporow1["NO_CASES"] = drw["hscode"].ToString().Trim();
                        oporow1["EXC_57F4"] = drw["hscode"].ToString().Trim();

                        if (fgen.make_double(drw["IGST"].ToString().Trim()) > 0)
                        {
                            oporow1["IOPR"] = "IG";
                            iopr = "IG";

                            oporow1["EXC_RATE"] = drw["IGST"].ToString().Trim();
                            dVal1 = Math.Round(dVal * (fgen.make_double(drw["IGST"].ToString().Trim()) / 100), 2);
                            oporow1["EXC_AMT"] = Math.Round(dVal1, 2);
                        }
                        else
                        {
                            iopr = "CG";
                            oporow1["IOPR"] = "CG";
                            oporow1["EXC_RATE"] = drw["CGST"].ToString().Trim();
                            dVal1 = Math.Round(dVal * (fgen.make_double(drw["CGST"].ToString().Trim()) / 100), 2);
                            oporow1["EXC_AMT"] = Math.Round(dVal1, 2);

                            oporow1["CESS_PERCENT"] = drw["SGST"].ToString().Trim();
                            dVal2 = Math.Round(dVal * (fgen.make_double(drw["SGST"].ToString().Trim()) / 100), 2);
                            oporow1["CESS_PU"] = Math.Round(dVal2, 2);
                        }


                        oporow1["STORE"] = "N";
                        oporow1["MORDER"] = 1;
                        oporow1["SPEXC_RATE"] = dVal;
                        oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2;

                        //*******************
                        par_code = txtirate.Value.Trim();
                        if (iopr == "CG")
                        {
                            if (tax_code.Length <= 0)
                            {
                                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                                tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
                            }
                        }
                        else
                        {
                            if (tax_code.Length <= 0)
                            {
                                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");
                            }
                        }
                        if (schg_code.Length <= 0)
                            schg_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(params) as param from controls where id='A41'", "param");

                        //***********************

                        oporow1["RCODE"] = sal_code;

                        oporow1["btchno"] = frm_mbr + ViewState["refNo"].ToString() + txtvchdate.Text.Trim();

                        if (edmode.Value == "Y")
                        {
                            oporow1["eNt_by"] = ViewState["entby"].ToString();
                            oporow1["eNt_dt"] = ViewState["entdt"].ToString();
                            oporow1["edt_by"] = frm_uname;
                            oporow1["edt_dt"] = vardate;
                        }
                        else
                        {
                            oporow1["eNt_by"] = frm_uname;
                            oporow1["eNt_dt"] = vardate;
                            oporow1["edt_by"] = "-";
                            oporow1["eDt_dt"] = vardate;
                        }

                        oDS1.Tables[0].Rows.Add(oporow1);

                        fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");

                        #region Voucher Saving
                        batchNo = "W" + batchNo;

                        if (nVty == "58")
                        {
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, dVal, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, dVal1, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");

                            if (tax_code2.Length > 0)
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, dVal2, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                            }
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                        }
                        else
                        {
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, dVal, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, dVal1, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");

                            if (tax_code2.Length > 0)
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, dVal2, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                            }
                        }
                        #endregion


                        l++;
                    }
                    #endregion
                }
            }
        }
    }
    */

    void save_fun2()
    {
    }

    void save_fun3()
    {

    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "SELECT 'ED' AS FSTR,'Record Efforts Done' as NAME,'ED' AS CODE FROM dual";
    }
    //------------------------------------------------------------------------------------   
    protected void btnupload_Click(object sender, EventArgs e)
    {
        if (frm_formID == "F25217")
        {
            if (txtuptcol.Value.Trim().Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Select The Fields You Want To Update");
                return;
            }
        }
        string ext = "", filesavepath = "";
        string excelConString = "";
        btnupdate.Disabled = true; // BY MADHVI ON 28 MARCH 2019
        if (FileUpload1.HasFile)
        {
            ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext == ".xls")
            {
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                FileUpload1.SaveAs(filesavepath);
                excelConString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filesavepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";

                //excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            }
            else
            {
                fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!");
                return;
            }
            try
            {
                OleDbConnection OleDbConn = new OleDbConnection();
                OleDbConn.ConnectionString = excelConString;
                OleDbConn.Open();
                DataTable dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                OleDbConn.Close();
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                OleDbCommand OleDbCmd = new OleDbCommand();
                String Query = "";
                Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                OleDbCmd.CommandText = Query;
                OleDbCmd.Connection = OleDbConn;
                OleDbCmd.CommandTimeout = 0;
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = OleDbCmd;
                objAdapter.SelectCommand.CommandTimeout = 0;
                dt = null;
                dt = new DataTable();
                objAdapter.Fill(dt);
                string chkname = "";

                DataTable dtn = new DataTable();
                dtn.Columns.Add("SRNO", typeof(int));
                dtn.Columns.Add("ITEMCODE", typeof(string));
                dtn.Columns.Add("iname", typeof(string));
                dtn.Columns.Add("HSCODE", typeof(string));
                dtn.Columns.Add("cpartno", typeof(string));
                dtn.Columns.Add("cdrgno", typeof(string));
                dtn.Columns.Add("ciname", typeof(string));
                dtn.Columns.Add("unit", typeof(string));
                dtn.Columns.Add("no_proc", typeof(string));
                dtn.Columns.Add("irate", typeof(double));
                dtn.Columns.Add("abc_class", typeof(string));
                dtn.Columns.Add("binno", typeof(string));
                dtn.Columns.Add("icat", typeof(string));
                dtn.Columns.Add("iweight", typeof(double));
                dtn.Columns.Add("wt_net", typeof(double));
                dtn.Columns.Add("servicable", typeof(string));
                dtn.Columns.Add("maker", typeof(string));
                dtn.Columns.Add("packsize", typeof(double));
                dtn.Columns.Add("default_us", typeof(double));
                dtn.Columns.Add("imax", typeof(double));
                dtn.Columns.Add("imin", typeof(double));
                dtn.Columns.Add("iord", typeof(double));

                switch (frm_formID)
                {
                    case "F10175":
                        dtn.Columns.Add("bfactor", typeof(string));
                        break;
                }

                DataRow drn = null;

                // for checking data headers , excel file must contain same column
                if (dt.Columns.Count == dtn.Columns.Count)
                {
                    for (int j = 0; j < dtn.Columns.Count; j++)
                    {
                        if (dtn.Columns[j].ColumnName.ToString().Trim().ToUpper() != dt.Columns[j].ColumnName.ToString().Trim().ToUpper())
                        {
                            fgen.msg("-", "AMSG", "Names are not as per the prescribed format. Original Column Name is " + dtn.Columns[j].ColumnName.ToString().Trim().ToUpper() + ".But you have changed the column name to " + dt.Columns[j].ColumnName.ToString().Trim().ToUpper() + "");
                            return;
                        }
                    }
                }
                else
                {
                    fgen.msg("-", "AMSG", " Please put exact number of columns as original");
                    return;
                }
                dtn.Columns.Add("Duplicate", typeof(string));
                dtn.Columns.Add("ReasonOfFailure", typeof(string));
                dtn.Columns.Add("dtsrno", typeof(int)); // for development point of view

                int count = 1, count1 = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        drn = dtn.NewRow();
                        drn["srno"] = count;
                        if (frm_cocd == "VITR")
                        {
                            drn["itemcode"] = dr[1].ToString().Trim().PadLeft(10, '0');                            
                        }
                        else
                        {
                            drn["itemcode"] = fgen.padlc(Convert.ToInt32(dr[1].ToString().Trim()), 8);
                        }
                        drn["iname"] = dr[2].ToString().Trim().Replace("””", "").Replace("’’", "~");
                        drn["hscode"] = dr[3].ToString().Trim();
                        drn["cpartno"] = dr[4].ToString().Trim();
                        drn["cdrgno"] = dr[5].ToString().Trim();
                        drn["ciname"] = dr[6].ToString().Trim();
                        drn["unit"] = dr[7].ToString().Trim();
                        drn["no_proc"] = dr[8].ToString().Trim();
                        drn["irate"] = fgen.make_double(dr[9].ToString().Trim());
                        drn["abc_class"] = dr[10].ToString().Trim();
                        drn["binno"] = dr[11].ToString().Trim();
                        drn["icat"] = dr[12].ToString().Trim();
                        drn["iweight"] = fgen.make_double(dr[13].ToString().Trim());
                        drn["wt_net"] = fgen.make_double(dr[14].ToString().Trim());
                        drn["servicable"] = dr[15].ToString().Trim();
                        drn["maker"] = dr[16].ToString().Trim();
                        drn["packsize"] = fgen.make_double(dr[17].ToString().Trim(), 2);
                        drn["default_us"] = fgen.make_double(dr[18].ToString().Trim(), 2);
                        drn["imax"] = fgen.make_double(dr[19].ToString().Trim(), 2);
                        drn["imin"] = fgen.make_double(dr[20].ToString().Trim(), 2);
                        drn["iord"] = fgen.make_double(dr[21].ToString().Trim(), 2);
                        switch (frm_formID)
                        {
                            case "F10175":
                                drn["bfactor"] = fgen.padlc(fgen.make_int(dr[22].ToString().Trim()), 3);
                                break;
                        }
                        drn["dtsrno"] = count1;
                        count++;
                        count1++;
                        if (count == 198)
                        {

                        }
                        dtn.Rows.Add(drn);
                    }
                    catch { }
                }

                ViewState["dtn"] = dtn;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

                fgen.msg("-", "AMSG", "Total Rows Imported : " + dtn.Rows.Count.ToString());
                btnvalidate.Disabled = false;
            }
            catch (Exception ex)
            {
                fgen.msg("-", "AMSG", "Please Select Excel File only in .xls format!!");
            }
        }
    }

    protected void btnbranchmax_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SELBRANCHMAX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Branch ", frm_qstr);
    }

    protected void btnbranchirate_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SELBRANCHIRATE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Branch ", frm_qstr);
    }

    protected void btnbranchbinno_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SELBRANCHBINNO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Branch ", frm_qstr);
    }

    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
    }

    protected void btnRcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TRCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Leadger ", frm_qstr);
    }

    // this command is to update only the selected columns that user wants 
    protected void btnupdatefields_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "UPCOLUM";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Fields to Update ", frm_qstr);
    }

    protected void btnvalidate_ServerClick(object sender, EventArgs e)
    {
        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];

        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
        DataView view = new DataView(dtn);
        DataTable distinctValues = view.ToTable(true, "iname");
        #region
        //checking duplicate values in dataview
        //foreach (DataRow dr1 in distinctValues.Rows)
        //{
        //    DataView view2 = new DataView(dtn, "iname='" + dr1["iname"].ToString().Trim().Replace("'", " ") + "'", "", DataViewRowState.CurrentRows);
        //    dt2 = new DataTable();
        //    dt2 = view2.ToTable();
        //    if (dt2.Rows.Count == 1)
        //    {

        //    }
        //    else
        //    {
        //        for (int l = 0; l < dt2.Rows.Count; l++)
        //        {
        //            flag = 1;
        //            dtn.Rows[Convert.ToInt32(dt2.Rows[l]["dtsrno"].ToString())]["duplicate"] = dt2.Rows[l]["iname"].ToString() + " " + "is Duplicate";
        //        }
        //    }
        //}
        #endregion
        int req = 0, i = 0;
        dt = new DataTable();
        DataRow dr = null;
        string app = "";

        #region checkexistitemname
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        siname = new DataTable();
        if (frm_cocd == "VITR")
            siname = fgen.getdata(frm_qstr, frm_cocd, "select lpad(trim(icode),10,'0') as icode from item where length(trim(icode))>= 8");
        dt2 = new DataTable();
        if (frm_formID == "F25217" || frm_formID == "F25219")
        {
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "select trim(icode) as icode,branchcd from itembal where length(trim(icode))=8 and branchcd='" + frm_mbr + "' order by icode");
        }
        else if (frm_formID == "F10175")
        {
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "select distinct trim(type1) as code,name from typegrp where id='^8' order by code");
        }
        string chkname1 = "", chkbranch = "", Familycode = "";
        for (int i1 = 0; i1 < dtn.Rows.Count; i1++)
        {
            chkname1 = fgen.seek_iname_dt(siname, "icode='" + dtn.Rows[i1]["ITEMCODE"].ToString().Trim() + "'", "icode");
            if (chkname1 == "0")
            {
                flag = 1;
                app = "This itemcode is not found in database";
                req = req + 1;
            }
            switch (Prg_Id)
            {
                case "F25217":
                    if (dtn.Rows[i1]["imax"].ToString().Trim().Length > 9)
                    {
                        flag = 1;
                        app += "imax Value can be upto in 9 Digits . Characters are exceeding";
                        req = req + 1;
                    }

                    if (dtn.Rows[i1]["imin"].ToString().Trim().Length > 9)
                    {
                        flag = 1;
                        app += "imin Value can be upto in 9 Digits . Characters are exceeding";
                        req = req + 1;
                    }
                    if (dtn.Rows[i1]["IORD"].ToString().Trim().Length > 9)
                    {
                        flag = 1;
                        app += "IORD Value can be upto in 9 Digits .Characters are exceeding";
                        req = req + 1;
                    }
                    chkbranch = fgen.seek_iname_dt(dt2, "icode='" + dtn.Rows[i1]["ITEMCODE"].ToString().Trim() + "'", "branchcd");
                    if (chkbranch != frm_mbr)
                    {
                        flag = 1;
                        app += "Item Not Availbale In This Branch";
                        req = req + 1;
                    }
                    if (dtn.Rows[i1]["IRATE"].ToString().Trim().Length > 11)
                    {
                        flag = 1;
                        app += "IRATE Value can be upto in 11 Digits . Characters are exceeding";
                        req = req + 1;
                    }
                    #region
                    //if (dtn.Rows[i1]["iname"].ToString().Length < 2)
                    //{
                    //    flag = 1;
                    //    app += "Item Name must be entered";
                    //    req = req + 1;
                    //}

                    //if (dtn.Rows[i1]["abc_class"].ToString().Length > 5)
                    //{
                    //    flag = 1;
                    //    app += "This A/B/C CLASS  must be specified in 1 character only";
                    //    req = req + 1;
                    //}
                    //if (dtn.Rows[i1]["unit"].ToString().Length > 10 || dtn.Rows[i1]["no_proc"].ToString().Length > 8)
                    //{
                    //    flag = 1;
                    //    app += "This primary  unit  must be 10 in length and secondary unit  must be in 8 in length";
                    //    req = req + 1;
                    //}
                    //if (dtn.Rows[i1]["servicable"].ToString().Length > 2)
                    //{
                    //    flag = 1;
                    //    app += "ThE input  must be in (Y/N) format.Only 2 Characters.";
                    //    req = req + 1;
                    //}
                    //if (dtn.Rows[i1]["iweight"].ToString().Length > 9)
                    //{
                    //    flag = 1;
                    //    app += "Gross Weight can be upto 9 Digits.Characters are exceeding";
                    //    req = req + 1;
                    //}

                    //if (dtn.Rows[i1]["wt_net"].ToString().Length > 9)
                    //{
                    //    flag = 1;
                    //    app += "Net Weight can be upto in 9 Digits .Characters are exceeding";
                    //    req = req + 1;
                    //}                
                    //if (dtn.Rows[i1]["BINNO"].ToString().Length > 40)
                    //{
                    //    flag = 1;
                    //    app += "Binno Value can be upto  40 Characters . Characters are exceeding";
                    //    req = req + 1;
                    //}
                    #endregion
                    break;

                case "F25219":
                    if (dtn.Rows[i1]["imax"].ToString().Trim().Length > 9)
                    {
                        flag = 1;
                        app += "imax Value can be upto in 9 Digits . Characters are exceeding";
                        req = req + 1;
                    }

                    if (dtn.Rows[i1]["imin"].ToString().Trim().Length > 9)
                    {
                        flag = 1;
                        app += "imin Value can be upto in 9 Digits . Characters are exceeding";
                        req = req + 1;
                    }
                    if (dtn.Rows[i1]["IORD"].ToString().Trim().Length > 9)
                    {
                        flag = 1;
                        app += "IORD Value can be upto in 9 Digits .Characters are exceeding";
                        req = req + 1;
                    }
                    chkbranch = fgen.seek_iname_dt(dt2, "icode='" + dtn.Rows[i1]["ITEMCODE"].ToString().Trim() + "'", "branchcd");
                    if (chkbranch != frm_mbr)
                    {
                        flag = 1;
                        app += "Item Not Availbale In This Branch";
                        req = req + 1;
                    }
                    break;

                case "F25220":
                    if (dtn.Rows[i1]["IRATE"].ToString().Trim().Length > 11)
                    {
                        flag = 1;
                        app += "IRATE Value can be upto in 11 Digits . Characters are exceeding";
                        req = req + 1;
                    }
                    break;

                case "F10175":
                    if (dtn.Rows[i1]["bfactor"].ToString().Length > 3)
                    {
                        flag = 1;
                        app += "BFACTOR Value can be upto in 3 Digits . Characters are exceeding";
                        req = req + 1;
                    }
                    Familycode = fgen.seek_iname_dt(dt2, "code='" + dtn.Rows[i1]["bfactor"].ToString().Trim() + "'", "code");
                    if (dtn.Rows[i1]["bfactor"].ToString().Trim() != Familycode)
                    {
                        flag = 1;
                        app += "BFACTOR Value is not Present in Family Master.Please Check";
                        req = req + 1;
                    }
                    break;
            }
            if (app != "")
            {
                dtn.Rows[i1]["reasonoffailure"] = app;
                app = "";
            }
        }
        #endregion

        if ((req > 0) || (flag == 1))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is not validated successfully .Please download the excel file(See last two columns of excel file.) ");
            btnexptoexl.Visible = true;
            btnvalidate.Disabled = true;
            return;
        }
        if (dtn.Rows.Count > 0)
        {
            dtn.Columns.Remove("dtsrno");
        }
        ViewState["dtn"] = dtn;
        dt = new DataTable();
        DataTable dtn1 = new DataTable();
        dtn1 = (DataTable)ViewState["dtn"];
        dt = dtn1.Copy();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "", "ContentPlaceHolder1_datadiv").ToString(), false);

        if (flag == 0)
        {
            btnsave.Disabled = false;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is validated successfully");
            btnupdate.Disabled = false;
            btnvalidate.Disabled = true;
            return;
        }
    }

    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)ViewState["dtn"];

        if (dt1.Rows.Count > 0)

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", ""); // THIS PARTICULAR LINE IS ADDED BY MADHVI ON 20 JULY 2018 AS IT IS EXPORTING WRONG DATA.
        Session["send_dt"] = dt1;
        fgen.Fn_open_rptlevel("", frm_qstr);
    }

    public void update_data()
    {
        if ((txtuptcol.Value.Length < 2))
        {
            fgen.msg("-", "AMSG", "Please Select  Which Column You want to Update. ");
            return;
        }
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "srno"; // to change
            dtW = new DataTable();
            dtW = dvW.ToTable();
            string[] myArray = txtuptcol.Value.Split(new Char[] { ',' });

            for (int i1 = 0; i1 < myArray.Length; i1++)
            {
                if ((myArray[i1].ToString().Trim() == "'imax'") || (myArray[i1].ToString().Trim() == "'imin'") || (myArray[i1].ToString().Trim() == "'iord'"))
                {
                    if ((txtmax.Value.Length < 2))
                    {
                        fgen.msg("-", "AMSG", "Please Select BranchWise Or HeadOffice for MAX ");
                        return;
                    }
                }

                if ((myArray[i1].ToString().Trim() == "'binno'"))
                {
                    if ((txtbinno.Value.Length < 2))
                    {
                        fgen.msg("-", "AMSG", "Please Select BranchWise Or HeadOffice for BINNO.");
                        return;
                    }
                }

                if ((myArray[i1].ToString().Trim() == "'irate'"))
                {
                    if ((txtirate.Value.Length < 2))
                    {
                        fgen.msg("-", "AMSG", "Please Select BranchWise Or HeadOffice for Irate");
                        return;
                    }
                }
            }

            for (int i = 0; i < myArray.Length; i++)
            {
                // string icode = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(icode)+1),8,'0') as existcd from item where branchcd!='DD' and substr(icode,1,4)='" + gr1["subgp"] + "' and length(Trim(icode))>4  ", "existcd");
                foreach (DataRow gr1 in dtW.Rows)
                {
                    if ((myArray[i].ToString().Trim() == "'imax'"))
                    {
                        if (txtmax.Value == "BranchWise")
                        {
                            SQuery = "update itembal set imax='" + gr1["imax"].ToString().Trim() + "'where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                        //if (txtmax.Value == "HeadOffice")
                        //{
                        //    SQuery = "update item set imax='" + gr1["imax"].ToString().Trim() + "' where  branchcd='00' and icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                        //    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        //}
                    }
                    else if (myArray[i].ToString().Trim() == "'imin'")
                    {
                        if (txtmax.Value == "BranchWise")
                        {
                            SQuery = "update itembal set imin='" + gr1["imin"].ToString().Trim() + "' where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                        //if (txtmax.Value == "HeadOffice")
                        //{
                        //    SQuery = "update item set imin='" + gr1["imin"].ToString().Trim() + "' where  branchcd='00' and icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                        //    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        //}
                    }
                    else if (myArray[i].ToString().Trim() == "'irate'")
                    {
                        if (txtirate.Value == "HeadOffice")
                        {
                            if (frm_cocd == "VITR")
                            {
                                SQuery = "update item set irate='" + gr1["irate"].ToString().Trim() + "' where  branchcd='00' and lpad(icode,10,'0') ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                            }
                            else
                            {
                                SQuery = "update item set irate='" + gr1["irate"].ToString().Trim() + "' where  branchcd='00' and icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";

                            }
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                        //if (txtirate.Value == "BranchWise")
                        //{
                        //    SQuery = "update item set irate='" + gr1["irate"].ToString().Trim() + "' where  branchcd='" + frm_mbr + "' and icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                        //    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        //}
                    }
                    else if (myArray[i].ToString().Trim() == "'binno'")
                    {
                        if (txtbinno.Value == "BranchWise")
                        {
                            SQuery = "update itembal set binno='" + gr1["binno"].ToString().Trim() + "' where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                        //if (txtbinno.Value == "HeadOffice")
                        //{
                        //    SQuery = "update item set binno='" + gr1["binno"].ToString().Trim() + "' where  branchcd='00' and icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                        //    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        //}

                    }
                    else if (myArray[i].ToString().Trim() == "'iord'")
                    {
                        if (txtmax.Value == "BranchWise")
                        {
                            SQuery = "update itembal set iord='" + gr1["IORD"].ToString().Trim() + "' where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                        //if (txtmax.Value == "HeadOffice")
                        //{
                        //    SQuery = "update item set iord='" + gr1["IORD"].ToString().Trim() + "' where  branchcd='00' and icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                        //    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        //}
                    }
                    else
                    {
                        SQuery = "update " + frm_tabname + " set " + myArray[i].ToString().Trim().Replace("'", "") + "='" + gr1["" + myArray[i].ToString().Trim().Replace("'", "") + ""].ToString().Trim() + "', edt_by='" + frm_uname + "',edt_dt =to_date('" + Convert.ToDateTime(vardate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') where icode='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                    }

                    //  SQuery = "update " + frm_tabname + " set hscode='" + gr1["HSCODE"].ToString().Trim() + "',cpartno='" + gr1["PART_NO"].ToString().Trim() + "', iname='" + gr1["iname"].ToString().Trim() + "',";
                    // SQuery += "ciname='" + gr1["iname_CUST"].ToString().Trim() + "', unit='" + gr1["unit"].ToString().Trim() + "', no_proc='" + gr1["no_proc"].ToString().Trim() + "',";
                    // SQuery += "servicable='" + gr1["servicable"].ToString().Trim() + "',irate='" + gr1["STD_RATE"].ToString().Trim() + "', packsize='" + gr1["STANDARD_PACKING"].ToString().Trim() + "',";
                    // SQuery += "default_us='" + gr1["SHELF_LIFE_DAYS"].ToString().Trim() + "',abc_class='" + gr1["abc_class"].ToString().Trim() + "', binno='" + gr1["LOCT"].ToString().Trim() + "',";
                    // SQuery += "icat='" + gr1["CAT"].ToString().Trim() + "',maker='" + gr1["BRAND_OR_REF"].ToString().Trim() + "', iweight='" + gr1["iweight"].ToString().Trim() + "' , wt_net='" + gr1["wt_net"].ToString().Trim() + "',edt_by='" + frm_uname + "',edt_dt =to_date('" + Convert.ToDateTime(vardate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')";
                    // SQuery += "where icode='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                }
            }
            fgen.msg("-", "AMSG", "DATA is updated successfully");
            fgen.DisableForm(this.Controls);
            enablectrl();
            txtuptcol.Value = "";
            txtmax.Value = "";
            txtirate.Value = "";
            txtbinno.Value = "";
            FileUpload1.Enabled = false; // BY MADHVI ON 28 MARCH 2019
            btnexport.Disabled = false;  // BY MADHVI ON 28 MARCH 2019
        }
    }

    public void update_data_min()
    {
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "srno"; // to change
            dtW = new DataTable();
            dtW = dvW.ToTable();

            foreach (DataRow gr1 in dtW.Rows)
            {
                SQuery = "update itembal set imax='" + gr1["imax"].ToString().Trim() + "'where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                SQuery = "update itembal set imin='" + gr1["imin"].ToString().Trim() + "' where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                SQuery = "update itembal set iord='" + gr1["IORD"].ToString().Trim() + "' where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }
        }

        fgen.msg("-", "AMSG", "DATA is updated successfully");
        fgen.DisableForm(this.Controls);
        enablectrl();
        txtuptcol.Value = "";
        txtmax.Value = "";
        txtirate.Value = "";
        txtbinno.Value = "";
        FileUpload1.Enabled = false; // BY MADHVI ON 28 MARCH 2019
        btnexport.Disabled = false;  // BY MADHVI ON 28 MARCH 2019
    }

    protected void btnupdate_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)ViewState["dtn"];
        ExportToExcel(dt1, "Temp_back" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F25217":
                update_data();
                // Saving Updating History
                //fgen.save_info_mac(frm_qstr, frm_cocd, frm_mbr, "",vardate, frm_uname, frm_vty, lblheader.Text.Trim() + " Updated");
                break;
            case "F25219":
                update_data_min();
                // Saving Updating History
                //fgen.save_info_mac(frm_qstr, frm_cocd, frm_mbr, "", vardate, frm_uname, frm_vty, lblheader.Text.Trim() + " Updated");
                break;

            case "F25220":
            case "F10175":
                Update_Rate_Family();
                break;
        }
    }

    public static void ExportToExcel(DataTable dt, string filename)// function to save file in a particular folder
    {
        filename = filename.Replace(" ", "_").Replace("/", "_").Replace(":", "_");
        StreamWriter wr = new StreamWriter(@"c:\\TEJ_erp\\UPLOAD\\" + filename + ".xls");
        try
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            }

            wr.WriteLine();

            //write rows to excel file
            for (int i = 0; i < (dt.Rows.Count); i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j] != null)
                    {
                        wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                    }
                    else
                    {
                        wr.Write("\t");
                    }
                }
                //go to next line
                wr.WriteLine();
            }
            //close file
            wr.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnexport_ServerClick(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (Prg_Id == "F25220" || Prg_Id == "F10175")
        {
            if (frm_mbr != "00")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
                return;
            }
        }
        hffield.Value = "TRCODE";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Main Group ", frm_qstr);
    }

    protected void Update_Rate_Family()
    {
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "srno"; // to change
            dtW = new DataTable();
            dtW = dvW.ToTable();

            switch (frm_formID)
            {
                case "F25220":
                    if (frm_cocd == "VITR")
                    {
                        foreach (DataRow gr1 in dtW.Rows)
                        {
                            SQuery = "update item set irate='" + gr1["irate"].ToString().Trim() + "'where lpad(trim(icode),10,'0') ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                    }
                    else
                    {
                        foreach (DataRow gr1 in dtW.Rows)
                        {
                            SQuery = "update item set irate='" + gr1["irate"].ToString().Trim() + "'where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                    }
                    
                    break;

                case "F10175":
                    foreach (DataRow gr1 in dtW.Rows)
                    {
                        SQuery = "update item set bfactor='" + gr1["bfactor"].ToString().Trim() + "' where icode ='" + gr1["ITEMCODE"].ToString().Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                    }
                    break;
            }
        }
        fgen.msg("-", "AMSG", "DATA is updated successfully");
        fgen.DisableForm(this.Controls);
        enablectrl();
        txtuptcol.Value = "";
        txtmax.Value = "";
        txtirate.Value = "";
        txtbinno.Value = "";
        FileUpload1.Enabled = false;
        btnexport.Disabled = false;
    }
}