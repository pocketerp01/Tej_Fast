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
using System.Net;

public partial class om_any_upload : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, nVty = "";
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0, flag = 0;

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
            btndel.Visible = false;
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
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btnvalidate.Disabled = true; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;

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
        frm_tabname = "ANY_EXCEL";

        lblheader.Text = "Multi Fixed Asset Upload";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
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
            //case "SG1_ROW_ADD":
            //case "SG1_ROW_ADD_E":
            //    SQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name ";
            //    break;
            //case "TACODE":
            //    SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '16%' order by acode";
            //    break;
            //case "TRCODE":
            //    SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '2%' order by acode";
            //    break;
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
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        FileUpload1.Enabled = true;
        if (chk_rights == "Y")
        {
            txtvchnum.Text = frm_vnum;
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

        //check duplicate rows in dataview


        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        hfCNote.Value = "Y";
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
        FileUpload1.Enabled = false;
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')||trim(a.COL33)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "'");
                // Deleing data from Sr Ctrl Table               
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'  ");
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
                    txtvchnum.Text = frm_vnum;
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
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
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
                //case "TACODE":
                //    txtacode.Value = col1;
                //    txtAname.Value = col2;
                //    break;
                //case "TRCODE":
                //    txtRcode.Value = col1;
                //    Text2.Value = col2;
                //    break;
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
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "SELECT acode as assetcode,assetid,assetname,grp as groupcode,basiccost,install_cost,custom_duty,other_chrgs,original_cost,op_dep,life as life_yrs,totlife as life_in_days,ballife,depableval,vchnum as Entryno, to_char(vchdate,'dd/mm/yyyy') as vdate,ent_by,ent_dt FROM " + frm_tabname + " where vchdate " + PrdRange + "  and type='10' and branchcd='" + frm_mbr + "'order by acode,vchnum,vchdate desc";
            
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
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
                        // save_fun();

                        //oDS.Dispose();
                        //oporow = null;
                        //oDS = new DataSet();
                        //oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
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
                        }
                        //fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        //save_fun2();

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
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "Hello", "test Mail");
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

        string chkname = "";
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");        
        DataTable dtW = (DataTable)ViewState["dtn"];

        DataTable mg_dt= new DataTable();
        mg_dt = fgen.getdata(frm_qstr, frm_cocd, "select BRANCHCD||trim(acode) as fstr from EMPMAS");

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
                    //string acnat;
                    //acnat = gr1["GROUPCODE"].ToString().Trim();
                    chk_code = fgen.next_no(frm_qstr, frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");

                    if (chk_code == "0")
                    {
                        oporow["vchnum"] = "000001";
                    }
                    else
                    {
                        oporow["vchnum"] = chk_code;
                    }
                }
                try
                {
                    oporow["BRANCHCD"] = fgen.padlc(fgen.make_int(gr1["Code"].ToString().ToUpper().Trim().Replace("FIN", "")), 2);
                    oporow["type"] = frm_vty;
                    oporow["vchdate"] = (txtvchdate.Text).ToString().Trim();
                    oporow["Code"] = gr1["Code"].ToString().ToUpper().Trim().Replace("FIN", "");
                    oporow["LOCATION"] = gr1["LOCATION"].ToString().ToUpper().Trim();
                    oporow["EMP_CODE"] = gr1["EMP_CODE"].ToString().Trim().ToUpper().Trim().Replace("FIN", ""); 
                    oporow["EMP_NAME"] = gr1["EMP_NAME"].ToString().ToUpper().Trim();
                    oporow["DESIGNATION"] = gr1["DESIGNATION"].ToString().ToUpper().Trim();
                    oporow["DEPARTMENT"] = gr1["DEPARTMENT"].ToString().ToUpper().Trim();
                    oporow["DOJ"] = fgen.make_def_Date(Convert.ToDateTime(gr1["DOJ"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);
                    oporow["BASIC"] = fgen.make_double(gr1["BASIC"].ToString().Trim());
                    oporow["HRA"] = fgen.make_double(gr1["HRA"].ToString().Trim());
                    oporow["OTHERS"] = fgen.make_double(gr1["OTHERS"].ToString().Trim());
                    oporow["SPCLALL"] = fgen.make_double(gr1["SPCLALL"].ToString().Trim());
                    oporow["PHONE"] = fgen.make_double(gr1["PHONE"].ToString().Trim());
                    oporow["CONVEYANCE"] = fgen.make_double(gr1["CONVEYANCE"].ToString().Trim());
                    oporow["MEDICAL"] = fgen.make_double(gr1["MEDICAL"].ToString().Trim());
                    oporow["GROSS"] = fgen.make_double(gr1["GROSS"].ToString().Trim());
                    oporow["EMPLOYER_PF"] = fgen.make_double(gr1["EMPLOYER_PF"].ToString().Trim());
                    oporow["ESIC"] = fgen.make_double(gr1["ESIC"].ToString().Trim());
                    oporow["LTA"] = fgen.make_double(gr1["LTA"].ToString().Trim());
                    oporow["BONUS"] = fgen.make_double(gr1["BONUS"].ToString().Trim());
                    oporow["TVP"] = fgen.make_double(gr1["TVP"].ToString().Trim());
                    oporow["CTC"] = fgen.make_double(gr1["CTC"].ToString().Trim());
                    oporow["WASHING"] = fgen.make_double(gr1["WASHING"].ToString().Trim());
                    oporow["MOBILE_EXP"] = fgen.make_double(gr1["MOBILE_EXP"].ToString().Trim());
                    oporow["LWF_DED"] = fgen.make_double(gr1["LWF_DED"].ToString().Trim());
                    oporow["E_PF_CONTRI"] = fgen.make_double(gr1["E_PF_CONTRI"].ToString().Trim());

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
                catch { }
            }
        }
    }
  

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
        string ext = "", filesavepath = "";
        string excelConString = "";

        if (FileUpload1.HasFile)
        {
            ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext == ".xls")
            {
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                FileUpload1.SaveAs(filesavepath);
                excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            }
            else
            {
                fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!. Once the file is uploaded, data is validated through Validate button. If the data is validated then only data will be saved after pressing Save button.");
                return;
            }
            try
            {
                OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
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
                dtn.Columns.Add("Code", typeof(string));
                dtn.Columns.Add("LOCATION", typeof(string));
                dtn.Columns.Add("EMP_CODE", typeof(string));
                dtn.Columns.Add("EMP_NAME", typeof(string));
                dtn.Columns.Add("DESIGNATION", typeof(string));
                dtn.Columns.Add("DEPARTMENT", typeof(string));
                dtn.Columns.Add("DOJ", typeof(string));
                dtn.Columns.Add("BASIC", typeof(double));
                dtn.Columns.Add("HRA", typeof(double));
                dtn.Columns.Add("OTHERS", typeof(double));
                dtn.Columns.Add("SPCLALL", typeof(double));
                dtn.Columns.Add("PHONE", typeof(double));
                dtn.Columns.Add("CONVEYANCE", typeof(double));
                dtn.Columns.Add("MEDICAL", typeof(double));
                dtn.Columns.Add("GROSS", typeof(double));
                dtn.Columns.Add("EMPLOYER_PF", typeof(double));
                dtn.Columns.Add("ESIC", typeof(double));
                dtn.Columns.Add("LTA", typeof(double));
                dtn.Columns.Add("BONUS", typeof(double));
                dtn.Columns.Add("TVP", typeof(double));
                dtn.Columns.Add("CTC", typeof(double));
                dtn.Columns.Add("WASHING", typeof(double));
                dtn.Columns.Add("MOBILE_EXP", typeof(double));
                dtn.Columns.Add("LWF_DED", typeof(double));
                dtn.Columns.Add("E_PF_CONTRI", typeof(double));
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
                    drn = dtn.NewRow();
                    drn["srno"] = count;
                    drn["Code"] = dr[1].ToString().Trim().Replace("””", "").Replace("’’", "~").Replace("-", " ");
                    drn["LOCATION"] = dr[2].ToString().Trim().Replace("””", "").Replace("’’", "~").Replace("-", " ");
                    drn["EMP_CODE"] = dr[3].ToString().Trim().Replace("””", "").Replace("’’", "~").Replace("-", " ");
                    drn["EMP_NAME"] = dr[4].ToString().Trim().Replace("””", "").Replace("’’", "~").Replace("-", " ");
                    drn["DESIGNATION"] = dr[5].ToString().Trim();
                    drn["DEPARTMENT"] = dr[6].ToString().Trim().Replace("””", "").Replace("’’", "~").Replace("-", " ");
                    drn["DOJ"] =dr[7].ToString().Trim();
                    drn["BASIC"] = Math.Round(fgen.make_double(dr[8].ToString().Trim()), 2);
                    drn["HRA"] = Math.Round(fgen.make_double(dr[9].ToString().Trim()), 2);
                    drn["OTHERS"] = Math.Round(fgen.make_double(dr[10].ToString().Trim()), 2);
                    drn["SPCLALL"] = Math.Round(fgen.make_double(dr[11].ToString().Trim()), 2);
                    drn["PHONE"] = Math.Round(fgen.make_double(dr[12].ToString().Trim()), 2);
                    drn["CONVEYANCE"] = Math.Round(fgen.make_double(dr[13].ToString().Trim()), 2);
                    drn["MEDICAL"] = fgen.make_double(dr[14].ToString().Trim());
                    drn["GROSS"] = fgen.make_double(dr[15].ToString().Trim());
                    drn["EMPLOYER_PF"] = fgen.make_double(dr[16].ToString().Trim());
                    drn["ESIC"] = fgen.make_double(dr[17].ToString().Trim());
                    drn["LTA"] = fgen.make_double(dr[18].ToString().Trim());
                    drn["BONUS"] = fgen.make_double(dr[19].ToString().Trim());
                    drn["TVP"] = fgen.make_double(dr[20].ToString().Trim());
                    drn["CTC"] = fgen.make_double(dr[21].ToString().Trim());
                    drn["WASHING"] = fgen.make_double(dr[22].ToString().Trim());
                    drn["MOBILE_EXP"] = fgen.make_double(dr[23].ToString().Trim());
                    drn["LWF_DED"] = fgen.make_double(dr[24].ToString().Trim());
                    drn["E_PF_CONTRI"] = fgen.make_double(dr[25].ToString().Trim());
                    
                    drn["dtsrno"] = count1;
                    count++;
                    count1++;
                    dtn.Rows.Add(drn);
                }

                ViewState["dtn"] = dtn;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

                fgen.msg("-", "AMSG", "Please press validate after successfull import. Total Rows Imported : " + dtn.Rows.Count.ToString());
                btnvalidate.Disabled = false;
                FileUpload1.Enabled = false;
            }
    
            catch (Exception ex)
            {
                fgen.msg("-", "AMSG", "Please Select Excel File only in .xls(Excel workbook 97-2003) format!!");
            }
        }
    }

    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
    }
    protected void btnRcode_Click(object sender, ImageClickEventArgs e)
    {
        //hffield.Value = "TRCODE";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Leadger ", frm_qstr);
    }

    protected void btnvalidate_ServerClick(object sender, EventArgs e)
    {
        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
        //DataView view = new DataView(dtn);
        //DataTable distinctValues = view.ToTable(true, "ASSETID");

        //string VCH_DATE = "";
  //      VCH_DATE = fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy')< MAX(VCHDATE) then 'NO' else 'OK' end) as fstr from  WB_FA_PUR WHERE branchcd='" + frm_mbr + "' and TYPE='10'", "fstr");

  //      if (VCH_DATE == "NO")
  //      {
  //          fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Asset Entries Already Made in Dates Later then Current Doc Date, Entry not Allowed ,Please Re Check");
  //          return;
  //      }

  //      // checking duplicate values in dataview
  //      foreach (DataRow dr1 in distinctValues.Rows)
  //      {
  //          DataView view2 = new DataView(dtn, "ASSETID='" + dr1["ASSETID"].ToString().Trim().Replace("'", "") + "'", "", DataViewRowState.CurrentRows);
  //          dt2 = new DataTable();
  //          dt2 = view2.ToTable();
  //          if (dt2.Rows.Count == 1)
  //          {

  //          }
  //          else
  //          {
  //              for (int l = 0; l < dt2.Rows.Count; l++)
  //              {
  //                  flag = 1;
  //                  dtn.Rows[Convert.ToInt32(dt2.Rows[l]["dtsrno"].ToString())]["duplicate"] = dt2.Rows[l]["ASSETID"].ToString() + " " + "is Duplicate";
  //              }
  //          }
  //      }
  //      int req = 0, i = 0;
  //      dt = new DataTable();
  //      DataRow dr = null;
  //      string app = "";

        
  //      #region checkexistASSETID

  //      SQuery = "select type1 from TYPEGRP where branchcd !='DD' and id='FA'";
  //      dt2 = new DataTable();// to seek groupname
  //      dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
  //        SQuery = "select assetid from wb_fa_pur where branchcd='"+frm_mbr +"' AND TYPE='10'";
 //         dt3 = new DataTable();// to keep assetid
 //         dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
  //      string chkassetcd = "";
  //      string chkgname = "";
  //     for (int i1 = 0; i1 < dtn.Rows.Count; i1++)
  //    {
            

  //          chkassetcd = fgen.seek_iname_dt(dt3, "assetid='" + dtn.Rows[i1]["assetid"].ToString().Trim()+"'", "assetid");
  //          if (chkassetcd != "0")
  //          {
  //              flag = 1;
  //              app = "The asset id " + dtn.Rows[i1]["assetid"].ToString().Trim() + " already exists in  database.";
  //              req = req + 1;
  //          }

   //     if (dtn.Rows[i1]["groupcode"].ToString().Length < 1)
  //          {
  //              flag = 1;
  //              app += "Please enter groupcode.";
  //              req = req + 1;
  //          }

  //          chkgname = fgen.seek_iname_dt(dt3, "CODE='" + dtn.Rows[i1]["CODE"].ToString().Trim() + "'", "CODE"); 
  //          if (chkgname == "0")
  //          {
  //              flag = 1;
  //              app = "The group code " + dtn.Rows[i1]["groupcode"].ToString().Trim() + " does not exist in  database.";
  //              req = req + 1;
  //          }

  //          //COMPLUSORY FIELDS

  //          if (dtn.Rows[i1]["assetid"].ToString().Length < 1)
  //          {
  //              flag = 1;
  //              app += "Please enter Asset Id, this must be unique for an asset.";
  //              req = req + 1;
  //          }

            
  //          if (dtn.Rows[i1]["nameofasset"].ToString().Length < 1)
  //          {
  //              flag = 1;
  //              app += "Please enter name of asset.";
  //              req = req + 1;
  //          }


  //          if (dtn.Rows[i1]["installdt"].ToString().Length < 1)
  //          {
  //              flag = 1;
  //              app += "Please enter installdt.Depreciation will be calculated from this date only.";
  //              req = req + 1;
  //          }

  //       // //  if (dtn.Rows[i1]["INVOICEDATE"].ToString().Length < 1)
  //       //   {
  //       //       flag = 1;
  //       //       app += "Please enter invoice date.";
  //       //       req = req + 1;
  //       //   }
           
  //       // //  if (dtn.Rows[i1]["life"].ToString().Length < 1)
  //       //   {
  //       //       flag = 1;
  //       //       app += "Please enter life in years.";
  //       //       req = req + 1;
  //       //   }
  //       ////   if (dtn.Rows[i1]["life"].ToString() == "0")
  //       //   {
  //       //       flag = 1;
  //       //       app += "Life cannot be 0.";
  //       //       req = req + 1;
  //       //   }

  //       ////   if (dtn.Rows[i1]["life"].ToString().Contains("."))
  //       //   {
  //       //       flag = 1;
  //       //       app += "Please enter life in years, not in part of years.";
  //       //       req = req + 1;
  //       //   }

  //          if (dtn.Rows[i1]["basiccost"].ToString().Length < 1)
  //          {
  //              flag = 1;
  //              app += "Please enter basiccost.";
  //              req = req + 1;
  //          }

  //        ////  if (dtn.Rows[i1]["USED_LIFE"].ToString().Length < 1)
  //        //  {
  //        //      flag = 1;
  //        //      app += "Please enter Used life in days.";
  //        //      req = req + 1;
  //        //  }

  //////          if (dtn.Rows[i1]["DEPPERDAY"].ToString().Length == 0)
  ////          {
  ////              flag = 1;
  ////              app += "Please enter DEPPERDAY.";
  ////              req = req + 1;
  ////          }

  //          if (dtn.Rows[i1]["basiccost"].ToString().Length > 13)
  //          {
  //              flag = 1;
  //              app += "Basiccost can be upto 13 characters";
  //              req = req + 1;
  //          }

  //          if (dtn.Rows[i1]["openingDep"].ToString().Length > 13)
  //          {
  //              flag = 1;
  //              app += "OpeningDep can be upto 13 characters";
  //              req = req + 1;
  //          }
  //////          if (dtn.Rows[i1]["supplyby"].ToString().Length > 75)
  ////          {
  ////              flag = 1;
  ////              app += "Supplyby can be upto 75 characters";
  ////              req = req + 1;
  ////          }
  //////          if (dtn.Rows[i1]["supplyadd"].ToString().Length > 75)
  ////          {
  ////              flag = 1;
  ////              app += "Supplyadd can be upto 75 characters";
  ////              req = req + 1;
  ////          }

  //// //         if (dtn.Rows[i1]["tangible"].ToString().Length > 3)
  ////          {
  ////              flag = 1;
  ////              app += "Tangible must be Y/N format.";
  ////              req = req + 1;
  ////          }

  //         if (Convert.ToDouble(dtn.Rows[i1]["basiccost"].ToString().Trim()) < 0)
  //          {
  //              flag = 1;
  //              app += " Basic cost cannot be negative.";
  //              req = req + 1;
  //          }

  //          if ((dtn.Rows[i1]["Warranty_days"].ToString().Trim() != "Y") && (dtn.Rows[i1]["Warranty_days"].ToString().Trim() != "N"))
  //          {
  //              flag = 1;
  //              app += " Warranty Days must be in Y/N format.";
  //              req = req + 1;
  //          }

          
  //          if ((Convert.ToInt64(dtn.Rows[i1]["Quantity"].ToString()) == 0))
  //          {
  //              flag = 1;
  //              app += " Quantity cannot be 0. ";
  //              req = req + 1;
  //          }
  //     ////     if ((dtn.Rows[i1]["Quantity"].ToString().Length > 1) || (Convert.ToInt64(dtn.Rows[i1]["Quantity"].ToString()) < 0) || (Convert.ToInt64(dtn.Rows[i1]["Quantity"].ToString()) != 1))
  //     //     {
  //     //         flag = 1;
  //     //         app += " Quantity  can be 1 only, as all assets are unique. ";
  //     //         req = req + 1;
  //     //     }
           
  //          if (dtn.Rows[i1]["assetid"].ToString().Length > 20)
  //          {
  //              flag = 1;
  //              app += "ASSET id  can be upto 20 characters and must be unique. ";
  //              req = req + 1;
  //          }

  //          if (dtn.Rows[i1]["residual_value"].ToString().Length < 1)
  //          {
  //              flag = 1;
  //              app += "Residual value  must be entered.It is required to calculate Depreciation per day as (Original Cost - Residual Value)";

  //              req = req + 1;
  //          }

  //          if ((dtn.Rows[i1]["installcost"].ToString().Length > 11) || (dtn.Rows[i1]["customduty"].ToString().Length > 11) || (dtn.Rows[i1]["otherchrges"].ToString().Length > 11))
  //          {
  //              flag = 1;
  //              app += "Installcost/ Custom duty/ Other charges can be upto 11 characters ";
  //              req = req + 1;
  //          }

  //          int dhd = fgen.ChkDate(dtn.Rows[i1]["installdt"].ToString().Trim());
  //          if (dtn.Rows[i1]["installdt"].ToString().Trim().Length != 10 && dhd == 0)
  //          {
  //              flag = 1;
  //              app += "Install Date should be in DD/MM/YYYY format.";
  //              req = req + 1;
  //          }
  //          else
  //          {
  //              dtn.Rows[i1]["installdt"] = fgen.make_def_Date(Convert.ToDateTime(dtn.Rows[i1]["installdt"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);
  //          }

  //          if (dtn.Rows[i1]["warranty_days"].ToString().Trim() == "Y")
  //          {
  //              int dhd2 = fgen.ChkDate(dtn.Rows[i1]["warranty_date"].ToString().Trim());
  //              if (dtn.Rows[i1]["warranty_date"].ToString().Trim().Length != 10 && dhd2 == 0)
  //              {
  //                  flag = 1;
  //                  app += "Warranty Date should be in DD/MM/YYYY format.";
  //                  req = req + 1;
  //              }
  //              else
  //              {
  //                  dtn.Rows[i1]["warranty_date"] = fgen.make_def_Date(Convert.ToDateTime(dtn.Rows[i1]["warranty_date"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);
  //              }
  //          }

  //     //     int dhd1 = fgen.ChkDate(dtn.Rows[i1]["invoicedate"].ToString().Trim());
  //          //if (dtn.Rows[i1]["invoicedate"].ToString().Trim().Length != 10 && dhd1 == 0)
  //          //{
  //          //    flag = 1;
  //          //    app += "(Invoice Date should be in DD/MM/YYYY format)";

  //          //    req = req + 1;
  //          //}
  //          //else
  //          //{
  //          //    dtn.Rows[i1]["invoicedate"] = fgen.make_def_Date(Convert.ToDateTime(dtn.Rows[i1]["invoicedate"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);
  //          //}

  //          if ((dtn.Rows[i1]["AMC"].ToString().Trim() != "Y") && (dtn.Rows[i1]["AMC"].ToString().Trim() != "N"))
  //          {
  //              flag = 1;
  //              app += " AMC must be in Y/N format.";
  //              req = req + 1;
  //          }

  //         if(dtn.Rows[i1]["PUR_TYPE"].ToString().Trim().Length>=2)
  //         {

  //          if ((dtn.Rows[i1]["PUR_TYPE"].ToString().Trim().Substring(0,1)!="5") && (dtn.Rows[i1]["PUR_TYPE"].ToString().Trim().Length!=2))
  //          {
  //              flag = 1;
  //              app += " Type  must be in 2 Digits starting with 5.";
  //              req = req + 1;
  //          }


  //         }
  //          if (app != "")
  //          {
  //              dtn.Rows[i1]["reasonoffailure"] = app;
  //              app = "";
  //          }

  //      }

  //      #endregion
  //      if (dtn.Rows.Count > 0)
  //      {
  //          dtn.Columns.Remove("dtsrno");
  //      }
  //      ViewState["dtn"] = dtn;
  //      dt = new DataTable();
  //      DataTable dtn1 = new DataTable();
  //      dtn1 = (DataTable)ViewState["dtn"];
  //      dt = dtn1.Copy();
  //      ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "", "ContentPlaceHolder1_datadiv").ToString(), false);

  //      if ((req > 0) || (flag == 1))
  //      {
  //          fgen.msg("-", "AMSG", "Dear " + frm_uname + ", The form is not validated successfully .Please download the excel file by clicking the PROCEED button at the DOWNwards LEFT corner.See last two columns of excel file to explain the errors, correct the data and when reuploading, delete the last two columns ");
  //          btnexptoexl.Visible = true;
  //          btnvalidate.Disabled = true;
  //          btnupload.Enabled = false;
           
  //          return;
  //      }

        if (flag == 0)
        {
            btnsave.Disabled = false;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is validated successfully. Press Save button to save.");
            btnvalidate.Disabled = true;
            btnupload.Enabled = false;
            return;
        }
        
    }

    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)ViewState["dtn"];

        if (dt1.Rows.Count > 0)
        {
            Session["send_dt"] = dt1;
            fgen.Fn_open_rptlevel("list of errors", frm_qstr);
        }
    }


    protected void btnhelp_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        DataTable dt1 = new DataTable();
        //dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT 'SRNO' as SRNO,'ASSETID' AS ASSETID,'GROUPCODE' AS GROUPCODE,'NAMEOFASSET' AS NAMEOFASSET,'SUPPLYBY' AS SUPPLYBY,'SUPPLYADD' AS SUPPLYADD,'INVOICENO' AS INVOICENO,'INVOICEDATE' AS INVOICEDATE,'LOCATION' AS LOCATION,'DOM_IMP'AS DOM_IMP,'BASICCOST' AS BASICCOST,'INSTALLCOST' AS INSTALLCOST,'CUSTOMDUTY' AS CUSTOMDUTY,'OTHERCHRGES' AS OTHERCHRGES,'ORIGINALCOST' AS ORIGINALCOST,'INSTALLDT' AS INSTALLDT,'OPENINGDEP' AS OPENINGDEP,'QUANTITY' AS QUANTITY,'TANGIBLE' AS TANGIBLE,'LIFE' AS LIFE,'DEPPERDAY' AS DEPPERDAY,'USED_LIFE' AS USED_LIFE,'RESIDUAL_VALUE' AS RESIDUAL_VALUE FROM DUAL");
        dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT 'SRNO' as SRNO,'ASSETID' AS ASSETID,'GROUPCODE' AS GROUPCODE,'NAMEOFASSET' AS NAMEOFASSET,'SUPPLYBY' AS SUPPLYBY,'SUPPLYADD' AS SUPPLYADD,'INVOICENO' AS INVOICENO,'INVOICEDATE' AS INVOICEDATE,'LOCATION' AS LOCATION,'DOM_IMP'AS DOM_IMP,'BASICCOST' AS BASICCOST,'INSTALLCOST' AS INSTALLCOST,'CUSTOMDUTY' AS CUSTOMDUTY,'OTHERCHRGES' AS OTHERCHRGES,'INSTALLDT' AS INSTALLDT,'OPENINGDEP' AS OPENINGDEP,'QUANTITY' AS QUANTITY,'TANGIBLE' AS TANGIBLE,'LIFE' AS LIFE,'DEPPERDAY' AS DEPPERDAY,'USED_LIFE' AS USED_LIFE,'RESIDUAL_VALUE' AS RESIDUAL_VALUE,'OWNER' AS OWNER,'DEPARTMENT_CODE' AS DEPARTMENT_CODE, 'WARRANTY_DAYS' AS WARRANTY_DAYS,'WARRANTY_DATE' AS WARRANTY_DATE, 'AMC' AS AMC ,'PUR_TYPE' AS PUR_TYPE,'OLD_TAG' AS OLD_TAG FROM DUAL");

        if (dt1.Rows.Count > 0)
            //fgen.exp_to_excel(dt1, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
            //else fgen.msg("-", "AMSG", "No Data to Export");
            // dt1.Dispose();
            Session["send_dt"] = dt1;
        fgen.Fn_open_rptlevel("Download The Excel File, Delete 1st Row, fill data and don't change the column positions.", frm_qstr);
    }
}